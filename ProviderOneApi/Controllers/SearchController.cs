using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestTask;

namespace ProviderOneApi.Controllers
{
    [ApiController]    
    [Route("api/v1")]
    public class SearchController : ControllerBase
    {
        [HttpPost("search")]
        public ProviderOneSearchResponse SearchAsync(ProviderOneSearchRequest request, CancellationToken cancellationToken)
        {
            return CreateResponseMock();
        }

        [HttpGet("ping")]
        
        public ActionResult<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            return true;
        }

        private ProviderOneSearchResponse CreateResponseMock()
        {
            var routes = new List<ProviderOneRoute>();
            for (int i = 0; i < 48; i++)
            {
                routes.Add(CreateMoscowSochi(DateTime.Now.Date.AddHours(i)));
            }

            return new ProviderOneSearchResponse()
            {
                Routes = routes.ToArray()
            };
        }

        private ProviderOneRoute CreateMoscowSochi(DateTime dateFrom) => new ProviderOneRoute()
        {
            From = "Moscow",
            To = "Sochi",
            DateFrom = dateFrom,
            DateTo = dateFrom.AddHours(3),
            Price = 10000m,
            TimeLimit = dateFrom.AddHours(-1),
        };
    }
}
