namespace Imperial.CommercialRoutes.Infrastructure.Helpers
{
    /// <summary>
    /// Contains the names of the API routes to avoid
    /// "magic strings" duplicated in the source code.
    /// </summary>
    public static class RouteApiNames
    {
        /// <summary>
        /// Gets route name to get the price breakdown of a route.
        /// </summary>
        public const string PriceBreakDown = nameof(PriceBreakDown);

        /// <summary>
        /// Gets route name to get the optimal aircraft of a route.
        /// </summary>
        public const string OptimalAircraft = nameof(OptimalAircraft);
    }
}
