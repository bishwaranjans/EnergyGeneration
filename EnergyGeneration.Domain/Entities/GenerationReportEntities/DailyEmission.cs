using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// DailyEmission
    /// </summary>
    public class DailyEmission
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the name of the generation type.
        /// </summary>
        /// <value>
        /// The name of the generation type.
        /// </value>
        public string GenerationTypeName { get; set; }

        /// <summary>
        /// Gets or sets the daily emission value.
        /// </summary>
        /// <value>
        /// The daily emission value.
        /// </value>
        public double DailyEmissionValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the generator.
        /// </summary>
        /// <value>
        /// The type of the generator.
        /// </value>
        public GeneratorType GeneratorType { get; set; }
    }
}
