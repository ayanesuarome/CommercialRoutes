using System;
using System.Collections.Generic;

namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents an empire aircraft.
    /// </summary>
    public class EmpireAircraft
    {
        /// <summary>
        /// Gets or sets dictionary of aircraft types.
        /// The key represents the aircraft type.
        /// </summary>
        public Dictionary<string, AircraftType> AircraftsTypes { get; set; }

        /// <summary>
        /// Gets or sets list of aircrafts references.
        /// </summary>
        public List<Aircraft> Aircrafts { get; set; }

        /// <summary>
        /// Represents an aircraft type.
        /// </summary>
        public class AircraftType
        {
            /// <summary>
            /// Gets or sets.
            /// </summary>
            public decimal MaxDistance { get; set; }

            /// <summary>
            /// Gets or sets.
            /// </summary>
            public int SupportedAttack { get; set; }

            /// <summary>
            /// Gets or sets.
            /// </summary>
            public int Crew { get; set; }
        }

        /// <summary>
        /// Represents an empire aircraft reference.
        /// </summary>
        public class Aircraft
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
            /// Gets or sets sector.
            /// </summary>
            public string Sector { get; set; }
        }
    }
}
