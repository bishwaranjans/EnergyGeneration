using EnergyGeneration.Domain.Entities.GenerationReportEntities;
using System.Collections.Generic;

namespace EnergyGeneration.Domain.Base
{
    /// <summary>
    /// BaseParser
    /// </summary>
    public abstract class BaseParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser"/> class.
        /// </summary>
        public BaseParser()
        {
            Results = new List<GenerationReportResult>();
        }

        /// <summary>
        /// Invoking the default constructor to load Results property
        /// </summary>
        /// <param name="FileName"></param>
        public BaseParser(string FileName) : this()
        {
            this.FileName = FileName;
        }

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        /// <value>
        /// The delimiter.
        /// </value>
        public virtual string Delimiter { get; set; }

        /// <summary>
        /// Read the file rows and process it for Xml, CSV,PIPE and other delimiters in case of further support
        /// </summary>
        public abstract void Read();

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reference data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is reference data; otherwise, <c>false</c>.
        /// </value>
        public bool IsReferenceData { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public List<GenerationReportResult> Results { get; set; }
    }
}
