using System.ComponentModel.DataAnnotations;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// CoalGenerator
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.GenerationReportEntities.FossilFuelGenerator" />
    public class CoalGenerator : FossilFuelGenerator
    {
        #region Properties

        /// <summary>
        /// Gets or sets the total heat input.
        /// </summary>
        /// <value>
        /// The total heat input.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The TotalHeatInput is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalHeatInput is not a valid double datatype")]
        public double TotalHeatInput { get; set; }

        /// <summary>
        /// Gets or sets the actual net generation.
        /// </summary>
        /// <value>
        /// The actual net generation.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The ActualNetGeneration is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "ActualNetGeneration is not a valid double datatype")]
        public double ActualNetGeneration { get; set; }

        /// <summary>
        /// Gets the actual heat rate.
        /// </summary>
        /// <value>
        /// The actual heat rate.
        /// </value>
        public ActualHeatRate ActualHeatRate
        {
            get
            {
                // Coal Emissions
                var coalHeatRate = TotalHeatInput / ActualNetGeneration;

                var coalActualHeatRate = new ActualHeatRate
                {
                    Name = Name,
                    HeatRate = coalHeatRate
                };

                return coalActualHeatRate;
            }
        }

        #endregion
    }
}

