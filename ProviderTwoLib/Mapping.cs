using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ProviderTwoLib
{
    [Mapper]
    public partial class ProviderTwoLibMappings
    {
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.Origin), 
            nameof(ProviderTwoShared.ProviderTwoSearchRequest.Departure))]
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.Destination), 
            nameof(ProviderTwoShared.ProviderTwoSearchRequest.Arrival))]
        [MapProperty(
            nameof(SearchApiShared.SearchRequest.OriginDateTime), 
            nameof(ProviderTwoShared.ProviderTwoSearchRequest.DepartureDate))]
        [MapProperty(
            [nameof(SearchApiShared.SearchRequest.Filters), nameof(SearchApiShared.SearchRequest.Filters.MinTimeLimit)],
            nameof(ProviderTwoShared.ProviderTwoSearchRequest.MinTimeLimit))]
        public partial ProviderTwoShared.ProviderTwoSearchRequest SearchRequestToProviderTwoSearchRequest(
            SearchApiShared.SearchRequest sharedSearchRequest);


        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MinPrice))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MaxPrice))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MinMinutesRoute))]
        [MapperIgnoreTarget(nameof(SearchApiShared.SearchResponse.MaxMinutesRoute))]
        [MapProperty(
            nameof(ProviderTwoShared.ProviderTwoSearchResponse.Routes),
            nameof(SearchApiShared.SearchResponse.Routes))]
        private partial SearchApiShared.SearchResponse PrivateProviderTwoSearchResultToSearchResponse(
            ProviderTwoShared.ProviderTwoSearchResponse ProviderTwoSearchResponse);

        [UserMapping(Default = true)]
        public SearchApiShared.SearchResponse ProviderTwoSearchResultToSearchResponse(
            ProviderTwoShared.ProviderTwoSearchResponse ProviderTwoSearchResponse)
        {            
            var response = PrivateProviderTwoSearchResultToSearchResponse(ProviderTwoSearchResponse);
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
            [nameof(ProviderTwoShared.ProviderTwoRoute.Departure), nameof(ProviderTwoShared.ProviderTwoRoute.Departure.Point)],
            nameof(SearchApiShared.Route.Origin))]
        [MapProperty(
            [nameof(ProviderTwoShared.ProviderTwoRoute.Arrival), nameof(ProviderTwoShared.ProviderTwoRoute.Departure.Point)],
            nameof(SearchApiShared.Route.Destination))]
        [MapProperty(
            [nameof(ProviderTwoShared.ProviderTwoRoute.Departure), nameof(ProviderTwoShared.ProviderTwoRoute.Departure.Date)],
            nameof(SearchApiShared.Route.OriginDateTime))]
        [MapProperty(
            [nameof(ProviderTwoShared.ProviderTwoRoute.Arrival), nameof(ProviderTwoShared.ProviderTwoRoute.Departure.Date)],
            nameof(SearchApiShared.Route.DestinationDateTime))]
        [MapProperty(
            nameof(ProviderTwoShared.ProviderTwoRoute.Price),
            nameof(SearchApiShared.Route.Price))]
        [MapProperty(
            nameof(ProviderTwoShared.ProviderTwoRoute.TimeLimit),
            nameof(SearchApiShared.Route.TimeLimit))]
        public partial SearchApiShared.Route ProviderTwoRouteToRoute(ProviderTwoShared.ProviderTwoRoute ProviderTwoRoute);

        public DateTime DateTimeOffsetToDateTime(DateTimeOffset source) => source.UtcDateTime;
    }
}
