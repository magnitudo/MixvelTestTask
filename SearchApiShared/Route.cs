using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CS8618
namespace SearchApiShared;

public class Route
{
    // Mandatory
    // Identifier of the whole route
    public Guid Id { get; set; }

    // Mandatory
    // Start point of route
    public string Origin { get; set; }

    // Mandatory
    // End point of route
    public string Destination { get; set; }

    // Mandatory
    // Start date of route
    public DateTime OriginDateTime { get; set; }

    // Mandatory
    // End date of route
    public DateTime DestinationDateTime { get; set; }

    // Mandatory
    // Price of route
    public decimal Price { get; set; }

    // Mandatory
    // Timelimit. After it expires, route became not actual
    public DateTime TimeLimit { get; set; }
}

public class RouteEqualityComparer : IEqualityComparer<Route>
{
    public bool Equals(Route? x, Route? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null) 
            || ReferenceEquals(y, null)) return false;

        return x.Id == y.Id
            && x.Origin == y.Origin
            && x.Destination == y.Destination
            && x.OriginDateTime == y.OriginDateTime
            && x.DestinationDateTime == y.DestinationDateTime
            && x.Price == x.Price
            && x.TimeLimit == y.TimeLimit;        
    }

    public int GetHashCode([DisallowNull] Route obj) 
        => HashCode.Combine(
            obj.Origin, 
            obj.Destination, 
            obj.OriginDateTime, 
            obj.DestinationDateTime, 
            obj.Price, 
            obj.TimeLimit);
}
#pragma warning restore CS8618