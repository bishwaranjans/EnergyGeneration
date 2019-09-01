﻿namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
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
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        public double Energy { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Price { get; set; }
    }
}