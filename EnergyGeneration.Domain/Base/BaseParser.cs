using EnergyGeneration.Domain.Entities.GenerationReportEntities;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EnergyGeneration.Domain.Base
{
    /// <summary>
    /// BaseParser
    /// </summary>
    public abstract class BaseParser
    {
        #region Properties

        /// <summary>
        /// The generators from to be parsed file
        /// </summary>
        public virtual List<BaseGenerator> Generators { get; set; }

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        /// <value>
        /// The delimiter.
        /// </value>
        public virtual string Delimiter { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser"/> class.
        /// </summary>
        public BaseParser()
        {
        }

        /// <summary>
        /// Invoking the default constructor to load Results property
        /// </summary>
        /// <param name="FileName"></param>
        public BaseParser(string FileName) : this()
        {
            this.FileName = FileName;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Read the file rows and process it for Xml, CSV,PIPE and other delimiters in case of further support
        /// </summary>
        public abstract void Read();

        /// <summary>
        /// Generations the output.
        /// </summary>
        public abstract void GenerationOutput();

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Simples the stream axis.
        /// </summary>
        /// <param name="inputXml">The input XML.</param>
        /// <param name="matchName">Name of the match.</param>
        /// <returns></returns>
        protected virtual IEnumerable<XElement> SimpleStreamAxis(string inputXml, string matchName)
        {
            using (XmlReader reader = XmlReader.Create(inputXml))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == matchName)
                            {
                                XElement el = XElement.ReadFrom(reader) as XElement;
                                if (el != null)
                                    yield return el;
                            }
                            break;
                    }
                }
                reader.Close();
            }
        }

        #endregion
    }
}
