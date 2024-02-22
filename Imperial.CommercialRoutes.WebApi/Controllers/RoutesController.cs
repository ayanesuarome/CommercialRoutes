using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Services;
using Imperial.CommercialRoutes.Infrastructure.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Imperial.CommercialRoutes.WebApi.Controllers
{
    /// <summary>
    /// Routes API controller.
    /// </summary>
    [RoutePrefix("api/routes")]
    public class RoutesController : ApiController
    {
        private readonly ICommercialRoutesService _commercialRoutesService;

        /// <summary>
        /// Initializes a new instance of the class <see cref="RoutesController"/>.
        /// </summary>
        /// <param name="commercialRoutesService">Commercial distance service instance.</param>
        public RoutesController(ICommercialRoutesService commercialRoutesService)
        {
            _commercialRoutesService = commercialRoutesService;
        }

        /// <summary>
        /// Gets commercial routes.
        /// </summary>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of commercial routes.</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetRoutes(CancellationToken cancellationToken = default)
        {
            return Ok(await _commercialRoutesService.GetCommercialRoutesAsync(cancellationToken));
        }

        /// <summary>
        /// Gets price breakdown of a given route.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Price breakdown of the route</returns>
        [HttpPost, Route(RouteApiNames.PriceBreakDown)]
        public async Task<IHttpActionResult> RoutePriceBreakDown(
            [FromBody] RouteRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!request.DayOfWeek.HasValue)
            {
                request.DayOfWeek = (int)DateTime.Today.DayOfWeek;
            }

            return Ok(await _commercialRoutesService.GetRoutePriceBreakdown(request, cancellationToken));
        }

        /// <summary>
        /// Gets optimal aircraft reference that suits the needs of the specified route.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Optimal aircraft.</returns>
        [HttpPost, Route(RouteApiNames.OptimalAircraft)]
        public async Task<IHttpActionResult> GetOptimalAircraft(
            [FromBody] RouteRequest request,
            CancellationToken cancellationToken = default)
        {
            return Ok(await _commercialRoutesService.GetOptimalAircraft(request, cancellationToken));
        }
    }
}