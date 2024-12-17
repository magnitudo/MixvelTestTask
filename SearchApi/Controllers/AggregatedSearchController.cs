using Microsoft.AspNetCore.Mvc;
using SearchApi.Services;
using SearchApiShared;

namespace SearchApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AggregatedSearchController : ISearchService
    {
        private readonly AggregatedSearchProvider _provider;

        public AggregatedSearchController(AggregatedSearchProvider provider) { 
            _provider = provider;
        }

        [HttpGet("ping")]
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken) 
            => _provider.IsAvailableAsync(cancellationToken);

        [HttpPost("search")]
        public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
            => _provider.SearchAsync(request, cancellationToken);
    }
}
