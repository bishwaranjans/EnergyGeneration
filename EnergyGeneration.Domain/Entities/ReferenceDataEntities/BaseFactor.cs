using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnergyGeneration.Domain.Entities.ReferenceDataEntities
{
    /// <summary>
    /// BaseFactor
    /// </summary>
    public class BaseFactor
    {
        /// <summary>
        /// Gets or sets the high.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Factor - High is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "Factor - High is not a valid double datatype")]
        public double High { get; set; }

        /// <summary>
        /// Gets or sets the medium.
        /// </summary>
        /// <value>
        /// The medium.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Factor - Medium is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "Factor - Medium is not a valid double datatype")]
        public double Medium { get; set; }

        /// <summary>
        /// Gets or sets the low.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Factor - Low is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "Factor - Low is not a valid double datatype")]
        public double Low { get; set; }

        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        public List<string> ErrorMessages { get; set; }
    }
}
