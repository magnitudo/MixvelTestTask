using ApiSdk;
using ProviderOneLib;
using SearchApiShared;
using System.Net.WebSockets;

namespace ProviderOneLib
{
    public class ProviderOneSearchProvider : ISearchProvider
    {
        private readonly ProviderOneApiClient _apiClient;
        private readonly ProviderOneLibMappings _mapper;

        public ProviderOneSearchProvider(ProviderOneApiClient apiClient, ProviderOneLibMappings mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }


        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _apiClient.Api.V1.Ping.GetAsync();
                return result == true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.Api.V1.Search.PostAsync(
                    _mapper.SearchRequestToProviderOneSearchRequest(request),
                    cancellationToken: cancellationToken);

                return _mapper.ProviderOneSearchResultToSearchResponse(response ?? new());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}