using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SearchApiShared;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace SearchApi.Filters;

public class SearchRequestFilter : ISchemaFilter
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(SearchRequest))
        {
            var exmaple = new SearchRequest()
            {
                Destination = "Sochi",
                Origin = "Moscow",
                OriginDateTime = DateTime.Now.AddHours(3),
                Filters = new()
                {
                    DestinationDateTime = DateTime.Now.AddDays(1),
                    MaxPrice = 20000,
                    MinTimeLimit = DateTime.Now.AddHours(1),
                }
            };

            schema.Example = new OpenApiString(JsonSerializer.Serialize(exmaple, jsonSerializerOptions));
        }
    }
}
