using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Domain.Entities
{
    /// <summary>
    /// WindGenerator
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.BaseGenerator" />
    public class WindGenerator : BaseGenerator
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
    }
}
