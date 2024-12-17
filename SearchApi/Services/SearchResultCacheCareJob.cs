using Quartz;
using SearchApi.Cache;

namespace SearchApi.Services
{
    public class SearchResultCacheCareJob : IJob
    {
        private readonly SearchResultsCache _searchResultsCache;

        public SearchResultCacheCareJob(SearchResultsCache searchResultsCache)
        {
            _searchResultsCache = searchResultsCache;
        }

        private void CleanUp()
        {
            var keys = _searchResultsCache.Keys;

            foreach (var key in keys)
            {
                _searchResultsCache.CleanUp(key);
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            CleanUp();
            return Task.CompletedTask;
        }
    }
}
