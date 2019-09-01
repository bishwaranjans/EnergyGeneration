namespace EnergyGeneration.Domain.Mapping
{
    /// <summary>
    /// GeneratorsTypeAndFactorEntity
    /// </summary>
    public class GeneratorsTypeAndFactorEntity
    {
        /// <summary>
        /// Gets or sets the type of the generator.
        /// </summary>
        /// <value>
        /// The type of the generator.
        /// </value>
        public string GeneratorType { get; set; }

        /// <summary>
        /// Gets or sets the value factor.
        /// </summary>
        /// <value>
        /// The value factor.
        /// </value>
        public string ValueFactor { get; set; }

        /// <summary>
        /// Gets or sets the emission factor.
        /// </summary>
        /// <value>
        /// The emission factor.
        /// </value>
        public string EmissionFactor { get; set; }
    }
}
