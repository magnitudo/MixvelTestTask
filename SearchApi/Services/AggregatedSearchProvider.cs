using Microsoft.AspNetCore.Http.HttpResults;
using SearchApi.Cache;
using SearchApiShared;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SearchApi.Services
{
    public class AggregatedSearchProvider
    {
        private readonly IEnumerable<ISearchProvider> _providers;
        private readonly ILogger<AggregatedSearchProvider> _logger;
        private readonly SearchResultsCache _searchResultsCache;

        public AggregatedSearchProvider(
            IEnumerable<ISearchProvider> providers,            
            SearchResultsCache searchResultsCache,
            ILogger<AggregatedSearchProvider> logger) { 
            _providers = providers;
            _searchResultsCache = searchResultsCache;
            _logger = logger;
        }

        /*
         Вот тут есть очень много вариантов как это сделать правильно в рамках предметной области.         
         Не описано, а что должно происходить, если какой-то/все из провайдров сейчас недоступны и т.д.         
         */
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken) => Task.FromResult(true);

        public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken) => 
            request.Filters?.OnlyCached == true 
            ? CacheSearchAsync(request, cancellationToken)
            : OnlineSearchAsync(request, cancellationToken);


        /*
         Вот тут есть очень много вариантов как это сделать правильно в рамках предметной области.
         Как минимум приведённые в задаче структуры данных не позволяют в итоге получить информацию о том, а какой из
         провайдеров вернул тот или иной маршрут.
         Кроме того не описано, а что должно происходить, если какой-то из провайдров сейчас недоступен и т.д.
         Поэтому использую самое простое решение: запускаю запрос ко всем: кто ответил -- тот молодец.
         Ошибки игнорируем.
         */
        private async Task<SearchResponse> OnlineSearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var resultList = new List<SearchApiShared.Route>();
            var responses = new ConcurrentBag<SearchResponse>();
            var result = new SearchResponse();


            await Parallel.ForEachAsync(
                _providers,
                cancellationToken,
                async (i, c) =>
                {
                    try
                    {
                        if (await i.IsAvailableAsync(cancellationToken))
                        {
                            var providerResponse = await i.SearchAsync(request, cancellationToken);
                            var providerResult = new SearchResponse();
                            var matchedRoutes = providerResponse.Routes.Where(r => IsMatched(r, request.Filters, providerResult)).ToArray();
                            providerResult.Routes = matchedRoutes;
                            responses.Add(providerResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                });
            
            result.MinPrice = responses.Min(r => r.MinPrice);
            result.MaxPrice = responses.Max(r => r.MaxPrice);
            result.MinMinutesRoute = responses.Min(r => r.MinMinutesRoute);
            result.MaxMinutesRoute = responses.Max(r => r.MaxMinutesRoute);
            result.Routes = responses.SelectMany(r => r.Routes).ToArray();

            return result;
        }

        private Task<SearchResponse> CacheSearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var result = new SearchResponse();

            var matchedRoutes = _searchResultsCache
                .Find(request)
                .Where(r => IsMatched(r, request.Filters, result)).ToArray();

            result.Routes = matchedRoutes;

            return Task.FromResult(result);
        }

        public bool IsMatched(SearchApiShared.Route? route, SearchFilters? filters, SearchResponse result)
        {
            if (route == null) return false;

            _searchResultsCache.Add(route);

            if (filters == null) return true;
            if (filters.DestinationDateTime != null && route.DestinationDateTime > filters.DestinationDateTime) return false;            
            if (filters.MaxPrice != null && route.Price > filters.MaxPrice) return false;
            if (filters.MinTimeLimit != null && route.TimeLimit < filters.MinTimeLimit) return false;

            if (route.Price < result.MinPrice) result.MinPrice = route.Price;
            if (route.Price > result.MaxPrice) result.MaxPrice = route.Price;
            var routeMinutes = (int)Math.Round((route.DestinationDateTime - route.OriginDateTime).TotalMinutes, MidpointRounding.AwayFromZero);
            if (routeMinutes < result.MinMinutesRoute) result.MinMinutesRoute = routeMinutes;
            if (routeMinutes > result.MaxMinutesRoute) result.MaxMinutesRoute = routeMinutes;
           
            return true;
        }

    }
}