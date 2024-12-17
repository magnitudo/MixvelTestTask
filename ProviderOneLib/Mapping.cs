using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ProviderOneLib
{
    [Mapper]
    public partial class ProviderOneLibMappings
    {
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.Origin), 
            nameof(ApiSdk.Models.ProviderOneSearchRequest.From))]
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.Destination), 
            nameof(ApiSdk.Models.ProviderOneSearchRequest.To))]
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.OriginDateTime), 
            nameof(ApiSdk.Models.ProviderOneSearchRequest.DateFrom))]
        [MapProperty(
            [nameof(SearchApiShared.SearchRequest.Filters), nameof(SearchApiShared.SearchRequest.Filters.DestinationDateTime)],
            nameof(ApiSdk.Models.ProviderOneSearchRequest.DateTo))]
        [MapProperty(
            [nameof(SearchApiShared.SearchRequest.Filters), nameof(SearchApiShared.SearchRequest.Filters.MaxPrice)],
            nameof(ApiSdk.Models.ProviderOneSearchRequest.MaxPrice))]
        public partial ApiSdk.Models.ProviderOneSearchRequest SearchRequestToProviderOneSearchRequest(
            SearchApiShared.SearchRequest sharedSearchRequest);


        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MinPrice))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MaxPrice))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MinMinutesRoute))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MaxMinutesRoute))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneSearchResponse.Routes),
            nameof(SearchApiShared.SearchResponse.Routes))]
        private partial SearchApiShared.SearchResponse PrivateProviderOneSearchResultToSearchResponse(
            ApiSdk.Models.ProviderOneSearchResponse providerOneSearchResponse);

        [UserMapping(Default = true)]
        public SearchApiShared.SearchResponse ProviderOneSearchResultToSearchResponse(
            ApiSdk.Models.ProviderOneSearchResponse providerOneSearchResponse)
        {            
            var response = PrivateProviderOneSearchResultToSearchResponse(providerOneSearchResponse);
            response.MinPrice = response.Routes.Min(r => r.Price);
            response.MaxPrice = response.Routes.Max(r => r.Price);
            response.MinMinutesRoute = (int)Math.Round(
                response.Routes.Min(r => r.DestinationDateTime - r.OriginDateTime).TotalMinutes, MidpointRounding.AwayFromZero);
            response.MaxMinutesRoute = (int)Math.Round(
                response.Routes.Max(r => r.DestinationDateTime - r.OriginDateTime).TotalMinutes, MidpointRounding.AwayFromZero);
            return response;
        }

        [MapperIgnoreTarget(nameof(SearchApiShared.Route.Id))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.From),
            nameof(SearchApiShared.Route.Origin))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.To),
            nameof(SearchApiShared.Route.Destination))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.DateFrom),        
            nameof(SearchApiShared.Route.OriginDateTime))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.DateTo),
            nameof(SearchApiShared.Route.DestinationDateTime))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.Price),
            nameof(SearchApiShared.Route.Price))]
        [MapProperty(
            nameof(ApiSdk.Models.ProviderOneRoute.TimeLimit),
            nameof(SearchApiShared.Route.TimeLimit))]
        public partial SearchApiShared.Route ProviderOneRouteToRoute(ApiSdk.Models.ProviderOneRoute providerOneRoute);

        public DateTime DateTimeOffsetToDateTime(DateTimeOffset source) => source.UtcDateTime;
    }
}
