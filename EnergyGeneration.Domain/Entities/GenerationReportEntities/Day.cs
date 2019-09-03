using System.ComponentModel.DataAnnotations;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// Day
    /// </summary>
    public class Day
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Date is Empty")]
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Energy is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "Energy is not a valid double datatype")]
        public double Energy { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Price is Empty")]
        [Range(0, double.MaxValue, ErrorMessage = "Price is not a valid double datatype")]
        public double Price { get; set; }
    }
}
