using System.Collections.Generic;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// BaseGenerator
    /// </summary>
    public class BaseGenerator
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the generation.
        /// </summary>
        /// <value>
        /// The generation.
        /// </value>
        public List<Day> Generation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fossil fuel.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is fossil fuel; otherwise, <c>false</c>.
        /// </value>
        public bool IsFossilFuel { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the generator.
        /// </summary>
        /// <value>
        /// The type of the generator.
        /// </value>
        public GeneratorType GeneratorType { get; set; }
    }
}
