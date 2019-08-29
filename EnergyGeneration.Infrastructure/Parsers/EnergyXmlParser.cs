using EnergyGeneration.Domain.Base;
using EnergyGeneration.Domain.Entities;
using EnergyGeneration.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Infrastructure.Parsers
{
    /// <summary>
    /// EnergyXmlParser
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Base.BaseParser" />
    public class EnergyXmlParser : BaseParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyXmlParser"/> class.
        /// </summary>
        /// <param name="FileName"></param>
        public EnergyXmlParser(string FileName) : base(FileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyXmlParser"/> class.
        /// </summary>
        public EnergyXmlParser() : base()
        {
        }

        /// <summary>
        /// Read the file rows and process it for Xml, CSV,PIPE and other delimiters in case of further support
        /// </summary>
        public override void Read()
        {
            var generationReport = new GenerationReport()
            {
                Wind = GetWindGenerators(),
                Coal = GetCoalGenerator(),
                Gas = GetGasGenerators()
            };
        }

        /// <summary>
        /// Generates the output.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void GenerateOutput()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ParserType.Xml.ToString();
        }

        #region Helper Methods

        /// <summary>
        /// Simples the stream axis.
        /// </summary>
        /// <param name="inputXml">The input XML.</param>
        /// <param name="matchName">Name of the match.</param>
        /// <returns></returns>
        private IEnumerable<XElement> SimpleStreamAxis(string inputXml, string matchName)
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

        /// <summary>
        /// Gets the wind generators.
        /// </summary>
        /// <returns></returns>
        private List<WindGenerator> GetWindGenerators()
        {
            var windGenerators = (from windGeneratorElement in SimpleStreamAxis(FileName, "WindGenerator")
                                  select new WindGenerator
                                  {
                                      Name = (windGeneratorElement.ElementAnyNS("Name") != null) ? windGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                      Location = (windGeneratorElement.ElementAnyNS("Location") != null) ? windGeneratorElement.ElementAnyNS("Location").Value : string.Empty,
                                      Generation = windGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in windGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                              select new Day
                                                                                                              {
                                                                                                                  Date = dayGeneration.ElementAnyNS("Date").Value,
                                                                                                                  Energy = Double.Parse(dayGeneration.ElementAnyNS("Energy").Value),
                                                                                                                  Price = Double.Parse(dayGeneration.ElementAnyNS("Price").Value)
                                                                                                              }).ToList() : null
                                  }).ToList();

            return windGenerators;
        }

        /// <summary>
        /// Gets the gas generators.
        /// </summary>
        /// <returns></returns>
        private List<GasGenerator> GetGasGenerators()
        {
            var gasGenerators = (from gasGeneratorElement in SimpleStreamAxis(FileName, "GasGenerator")
                                 select new GasGenerator
                                 {
                                     Name = (gasGeneratorElement.ElementAnyNS("Name") != null) ? gasGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                     EmissionsRating = (gasGeneratorElement.ElementAnyNS("EmissionsRating") != null) ? Double.Parse(gasGeneratorElement.ElementAnyNS("EmissionsRating").Value) : 0.0,
                                     Generation = gasGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in gasGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                            select new Day
                                                                                                            {
                                                                                                                Date = dayGeneration.ElementAnyNS("Date").Value,
                                                                                                                Energy = Double.Parse(dayGeneration.ElementAnyNS("Energy").Value),
                                                                                                                Price = Double.Parse(dayGeneration.ElementAnyNS("Price").Value)
                                                                                                            }).ToList() : null
                                 }).ToList();

            return gasGenerators;
        }

        /// <summary>
        /// Gets the coal generator.
        /// </summary>
        /// <returns></returns>
        private List<CoalGenerator> GetCoalGenerator()
        {
            var gasGenerators = (from coalGeneratorElement in SimpleStreamAxis(FileName, "CoalGenerator")
                                 select new CoalGenerator
                                 {
                                     Name = (coalGeneratorElement.ElementAnyNS("Name") != null) ? coalGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                     EmissionsRating = (coalGeneratorElement.ElementAnyNS("EmissionsRating") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("EmissionsRating").Value) : 0.0,
                                     TotalHeatInput = (coalGeneratorElement.ElementAnyNS("TotalHeatInput") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("TotalHeatInput").Value) : 0.0,
                                     ActualNetGeneration = (coalGeneratorElement.ElementAnyNS("ActualNetGeneration") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("ActualNetGeneration").Value) : 0.0,
                                     Generation = coalGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in coalGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                             select new Day
                                                                                                             {
                                                                                                                 Date = dayGeneration.ElementAnyNS("Date").Value,
                                                                                                                 Energy = Double.Parse(dayGeneration.ElementAnyNS("Energy").Value),
                                                                                                                 Price = Double.Parse(dayGeneration.ElementAnyNS("Price").Value)
                                                                                                             }).ToList() : null
                                 }).ToList();

            return gasGenerators;
        }

        #endregion
    }
}
