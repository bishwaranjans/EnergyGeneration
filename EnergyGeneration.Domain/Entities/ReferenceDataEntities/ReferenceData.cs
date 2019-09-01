namespace EnergyGeneration.Domain.Entities.ReferenceDataEntities
{
    /// <summary>
    /// ReferenceData
    /// </summary>
    public class ReferenceData
    {
        /// <summary>
        /// Gets or sets the value factor.
        /// </summary>
        /// <value>
        /// The value factor.
        /// </value>
        public BaseFactor ValueFactor { get; set; }

        /// <summary>
        /// Gets or sets the emission factor.
        /// </summary>
        /// <value>
        /// The emission factor.
        /// </value>
        public BaseFactor EmissionFactor { get; set; }
    }
}
