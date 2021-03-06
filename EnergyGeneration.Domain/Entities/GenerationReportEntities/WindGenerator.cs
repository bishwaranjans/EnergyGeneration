﻿using System.ComponentModel.DataAnnotations;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// WindGenerator
    /// </summary>
    /// <seealso cref="BaseGenerator" />
    public class WindGenerator : BaseGenerator
    {
        #region Properties

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Location is Empty")]
        public string Location { get; set; }

        #endregion
    }
}
