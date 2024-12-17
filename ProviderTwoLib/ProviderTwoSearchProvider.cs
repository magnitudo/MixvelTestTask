using ProviderTwoLib;
using ProviderTwoShared;
using SearchApiShared;
using System.Net.Http.Json;

namespace ProviderTwoLib;

public class ProviderTwoSearchProvider : ISearchProvider
{
    private readonly HttpClient _httpClient;
    private readonly ProviderTwoLibMappings _mapper;

    public ProviderTwoSearchProvider(IHttpClientFactory httpClientFactory, ProviderTwoLibMappings mapper)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ProviderTwoSearchProvider));
        _mapper = mapper;
    }


    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync("ping");
            return response.StatusCode == System.Net.HttpStatusCode.OK;
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
            var providerRequest = _mapper.SearchRequestToProviderTwoSearchRequest(request);
            var response  = await _httpClient.PostAsJsonAsync("search",providerRequest,cancellationToken);
            response.EnsureSuccessStatusCode();
            var providerResponse = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>();
            var searchResponse = _mapper.ProviderTwoSearchResultToSearchResponse(providerResponse ?? new());
            return searchResponse;
            
        }
        catch (Exception)
        {
            throw;
        }
    }
}
