namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// GasGenerator
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.GenerationReportEntities.BaseGenerator" />
    public class GasGenerator : BaseGenerator
    {
        /// <summary>
        /// Gets or sets the emissions rating.
        /// </summary>
        /// <value>
        /// The emissions rating.
        /// </value>
        public double EmissionsRating { get; set; }
    }
}
