using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Domain.Entities
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
