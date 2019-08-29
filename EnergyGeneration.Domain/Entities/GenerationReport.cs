using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Domain.Entities
{
    /// <summary>
    /// GenerationReport
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.GenerationReportResult" />
    public class GenerationReport : GenerationReportResult
    {
        /// <summary>
        /// Gets or sets the wind.
        /// </summary>
        /// <value>
        /// The wind.
        /// </value>
        public List<WindGenerator> Wind { get; set; }

        /// <summary>
        /// Gets or sets the coal.
        /// </summary>
        /// <value>
        /// The coal.
        /// </value>
        public List<CoalGenerator> Coal { get; set; }

        /// <summary>
        /// Gets or sets the gas.
        /// </summary>
        /// <value>
        /// The gas.
        /// </value>
        public List<GasGenerator> Gas { get; set; }
    }
}
