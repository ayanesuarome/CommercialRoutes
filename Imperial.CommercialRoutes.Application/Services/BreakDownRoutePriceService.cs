using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Exceptions;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Services
{
    /// <summary>
    /// Route price breakdown service implementation.
    /// </summary>
    public class BreakDownRoutePriceService : IBreakDownRoutePriceService
    {
        #region Fields
        
        private string DecimalFormat => ConfigurationManager.AppSettings.Get("DecimalFormat");
        private readonly IEmpirePriceWebService _empirePriceWebService;
        private readonly ISecurityPriceService _securityPriceService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="BreakDownRoutePriceService"/>.
        /// </summary>
        /// <param name="empirePriceWebService">Empire distance web service instance.</param>
        /// <param name="securityPriceService">Securoty price service instance.</param>
        public BreakDownRoutePriceService(
            IEmpirePriceWebService empirePriceWebService,
            ISecurityPriceService securityPriceService)
        {
            _empirePriceWebService = empirePriceWebService;
            _securityPriceService = securityPriceService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the distance price breakdown including taxes.
        /// </summary>
        /// <param name="dayOfWeek">Day of week.</param>
        /// <param name="origin">Origin planet</param>
        /// <param name="destination">Destination planet</param>
        /// <param name="distance">Distance.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Route price breakdown</returns>
        public async Task<BreakdownRoutePrice> CalculateBreakdownRoutePrice(
            int dayOfWeek,
            Planet origin,
            Planet destination,
            Distance distance,
            CancellationToken cancellationToken = default)
        {
            FuelPriceSearchOptions priceSearchOptions = new FuelPriceSearchOptions
            {
                DayOfWeek = dayOfWeek,
                Sector = origin.Sector
            };

            IList<FuelPrice> originPrices = await _empirePriceWebService.GetFuelPricesAsync(priceSearchOptions, cancellationToken);
            FuelPrice originPrice = originPrices.FirstOrDefault();

            if(originPrice == null)
            {
                throw new BadRequestException($"Could not find a price for the origin planet '{origin.Name}' in the empire service");
            }

            decimal pricePerLunarDay = originPrice.PricePerLunarDay;

            if (origin.Sector != destination.Sector)
            {
                priceSearchOptions.Sector = destination.Sector;
                IList<FuelPrice> destinationPrices = await _empirePriceWebService.GetFuelPricesAsync(priceSearchOptions, cancellationToken);
                FuelPrice priceDestination = destinationPrices.FirstOrDefault();

                if (priceDestination == null)
                {
                    throw new BadRequestException($"Could not find a price for the planet '{destination.Name}' in the empire service");
                }

                pricePerLunarDay += priceDestination.PricePerLunarDay;
            }
            
            decimal totalAmount = Convert.ToDecimal((distance.LunarDays * pricePerLunarDay * _securityPriceService.CalculateSecurityPrice(origin, destination)).ToString(DecimalFormat));
            decimal eliteStromTrooperTax = Convert.ToDecimal((_securityPriceService.CalculateEliteStromTrooperTax(origin, destination)).ToString(DecimalFormat));

            BreakdownRoutePrice priceBreakdown = new BreakdownRoutePrice
            {
                TotalAmount = totalAmount,
                PricePerLunarDay = Convert.ToDecimal(pricePerLunarDay.ToString(DecimalFormat)),
                Tax = new RouteTax
                {
                    OriginDefenseCost = Convert.ToDecimal(origin.DefenseCost.ToString(DecimalFormat)),
                    DestinationDefenseCost = Convert.ToDecimal(destination.DefenseCost.ToString(DecimalFormat)),
                    EliteDefenseCost = eliteStromTrooperTax
                }
            };

            return priceBreakdown;
        }

        #endregion
    }
}
