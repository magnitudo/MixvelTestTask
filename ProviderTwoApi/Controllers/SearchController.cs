using Microsoft.AspNetCore.Mvc;
using ProviderTwoShared;

namespace ProviderTwoApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class SearchController : ControllerBase
    {
        [HttpPost("search")]
        public ProviderTwoSearchResponse SearchAsync(ProviderTwoSearchRequest request, CancellationToken cancellationToken)
        {
            return CreateResponseMock();
        }

        [HttpGet("ping")]
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        private ProviderTwoSearchResponse CreateResponseMock()
        {
            var routes = new List<ProviderTwoRoute>();
            for (int i = 0; i < 48; i++)
            {
                routes.Add(CreateMoscowSochi(DateTime.Now.Date.AddHours(i)));
            }

            return new ProviderTwoSearchResponse()
            {
                Routes = routes.ToArray()
            };
        }

        private ProviderTwoRoute CreateMoscowSochi(DateTime dateFrom) => new ProviderTwoRoute()
        {
            Departure = new()
            {
                Point = "Moscow",
                Date = dateFrom,
            },
            Arrival = new()
            {
                Point = "Sochi",
                Date = dateFrom.AddHours(3.5),
            },                      
            Price = 10500m,
            TimeLimit = dateFrom.AddHours(-1.5),
        };
    }
}
