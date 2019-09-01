using System.Collections.Generic;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// GenerationReportResult
    /// </summary>
    public class GenerationReportResult
    {
        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        List<string> ErrorMessages { get; set; }
    }
}
