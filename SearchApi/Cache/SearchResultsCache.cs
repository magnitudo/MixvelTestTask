using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using SearchApiShared;
using System.Collections.Concurrent;
using Route = SearchApiShared.Route;

namespace SearchApi.Cache
{
    public class SearchResultsCache
    {
        private readonly  ConcurrentDictionary<string, List<Route>> _cache = new ();
        private readonly RouteEqualityComparer _routeEqualityComparer = new RouteEqualityComparer();
               
        public IEnumerable<string> Keys => _cache.Keys;

        public void Add(Route route)
        {
            Task.Factory.StartNew(() =>
            {
                var key = RouteCacheKey(route);
                var list = _cache.GetOrAdd(key, new List<Route>());

                lock (list)
                {
                    if (!list.Contains(route, _routeEqualityComparer))
                    {
                        list.Add(route);
                    }
                }                
            });
        }

        public IEnumerable<Route> Find(SearchRequest request)
        {
            var key = RouteCacheKey(request);
            return Find(key);
        }

        public IEnumerable<Route> Find(string key)
        {
            if (_cache.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    return list.ToList();
                }
            }
            else return [];
        }

        public void CleanUp(string key)
        {
            if (_cache.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    var toRemove = new List<int>();
                    // Идём от конца к началу, чтобы можно было безболезненно удалять по индексу
                    for(int i=list.Count-1; i >=0; i--)
                    {
                        if (list[i].TimeLimit < DateTime.Now)
                        {
                            toRemove.Add(i);
                        }
                    }

                    foreach(var index in toRemove)
                    {
                        list.RemoveAt(index);
                    }
                }
            }            
        }

        private string RouteCacheKey(Route route) => $"{route.Origin}@{route.Destination}";

        private string RouteCacheKey(SearchRequest request) => $"{request.Origin}@{request.Destination}";
    }

    public class SearchResultsCacheRow
    {
        public string Key { get; set; }
        public ConcurrentBag<Route> Routes { get; set; }

        public SearchResultsCacheRow(string key, Route route) {
            Key = key;
            Routes = [route];
        }
    } 
}
