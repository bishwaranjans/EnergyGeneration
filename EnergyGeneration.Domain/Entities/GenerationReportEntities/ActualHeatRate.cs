namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// ActualHeatRate
    /// </summary>
    public class ActualHeatRate
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the heat rate.
        /// </summary>
        /// <value>
        /// The heat rate.
        /// </value>
        public double HeatRate { get; set; }
    }
}
