using System;

namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents an aircraft response.
    /// </summary>
    public class AircraftResponse
    {
        /// <summary>
        /// Gets or sets reference.
        /// </summary>
        public Guid? Reference { get; set; }

        /// <summary>
        /// Gets or sets type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets message to output.
        /// </summary>
        public string Message { get; set; }
    }
}