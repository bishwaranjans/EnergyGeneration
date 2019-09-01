namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// CoalGenerator
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.GenerationReportEntities.GasGenerator" />
    public class CoalGenerator : GasGenerator
    {
        /// <summary>
        /// Gets or sets the total heat input.
        /// </summary>
        /// <value>
        /// The total heat input.
        /// </value>
        public double TotalHeatInput { get; set; }

        /// <summary>
        /// Gets or sets the actual net generation.
        /// </summary>
        /// <value>
        /// The actual net generation.
        /// </value>
        public double ActualNetGeneration { get; set; }
    }
}
