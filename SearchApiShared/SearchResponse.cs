using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618
namespace SearchApiShared;

public class SearchResponse
{
    // Mandatory
    // Array of routes
    public Route[] Routes { get; set; }

    // Mandatory
    // The cheapest route
    public decimal MinPrice { get; set; } = decimal.MaxValue;

    // Mandatory
    // Most expensive route
    public decimal MaxPrice { get; set; } = decimal.MinValue;

    // Mandatory
    // The fastest route
    public int MinMinutesRoute { get; set; } = int.MaxValue;

    // Mandatory
    // The longest route
    public int MaxMinutesRoute { get; set; } = int.MinValue;
}

#pragma warning restore CS8618
