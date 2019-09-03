using EnergyGeneration.Domain.Base;
using EnergyGeneration.Domain.Entities.GenerationReportEntities;
using EnergyGeneration.Domain.Entities.ReferenceDataEntities;
using EnergyGeneration.Domain.SeedWork;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyXmlParser"/> class.
        /// </summary>
        /// <param name="FileName"></param>
        public EnergyXmlParser(string FileName) : base(FileName)
        {
            Generators = new List<BaseGenerator>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyXmlParser"/> class.
        /// </summary>
        public EnergyXmlParser() : base()
        {
            Generators = new List<BaseGenerator>();
        }

        #endregion

        #region Abstract Implementation

        /// <summary>
        /// Read the file rows and process it for Xml, CSV,PIPE and other delimiters in case of further support
        /// </summary>
        public override void Read()
        {
            try
            {
                // Populate the factor reference data during initialization of application.
                // Once aplication starts, reference data can not be modified
                if (IsReferenceData)
                {
                    ParseReferenceData();
                }
                else
                {
                    ParseGenerateReportData();
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Error occurred! Message : {ex.Message}. StackTrace : {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Generations the output.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void GenerationOutput()
        {
            XDocument doc = new XDocument(
                            new XComment("This is energy generation output file."),
                            // <Generator>
                            new XElement(Constants.GenerationOutput,

                                               // <Totals>
                                               new XElement(Constants.Totals, from generation in Generators.Select(s => s.TotalGeneratorGenerationByName)
                                                                                  // <Generator>
                                                                              select new XElement(Constants.Generator,
                                                                                         new XElement(Constants.Name, generation.Key),
                                                                                         new XElement(Constants.Total, generation.Value))),
                                               // <MaxEmissionGenerators>
                                               new XElement(Constants.MaxEmissionGenerators, from generation in (from dailyEmission in Generators.Where(s => s.IsFossilFuel).SelectMany(s => ((FossilFuelGenerator)s).GeneratorDailyEmissions)
                                                                                                                 group dailyEmission by dailyEmission.Date into dateWiseGroup
                                                                                                                 select new DailyEmission
                                                                                                                 {
                                                                                                                     Date = dateWiseGroup.Key,
                                                                                                                     DailyEmissionValue = dateWiseGroup.Max(s => s.DailyEmissionValue),
                                                                                                                     GenerationTypeName = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GenerationTypeName,
                                                                                                                     GeneratorType = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GeneratorType
                                                                                                                 }).OrderByDescending(s => s.DailyEmissionValue)
                                                                                                 // <Day>
                                                                                             select new XElement(Constants.Day,
                                                                                                        new XElement(Constants.Name, generation.GenerationTypeName),
                                                                                                        new XElement(Constants.Date, generation.Date),
                                                                                                        new XElement(Constants.Emission, generation.DailyEmissionValue))),
                                              // <ActualHeatRates>
                                              from actualHeatRate in Generators.Where(s => s.GeneratorType == GeneratorType.Coal).Select(s => ((CoalGenerator)s).ActualHeatRate)
                                              select new XElement(Constants.ActualHeatRates,
                                                         new XElement(Constants.Name, actualHeatRate.Name),
                                                         new XElement(Constants.HeatRate, actualHeatRate.HeatRate))
                                        )
                            );

            // Save
            var fileFullName = Path.Combine(OutputFileFolder, OutputFileName);
            doc.Save(fileFullName);
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

        #endregion

        #region Parsing Helpers

        /// <summary>
        /// Parses the reference data.
        /// </summary>
        private void ParseReferenceData()
        {
            // Reference data are constant and can not be modified while application is running
            // This will be initialized when the aplication starts
            Constants.ReferenceData = new ReferenceData()
            {
                ValueFactor = GetFactors(FactorType.ValueFactor),
                EmissionsFactor = GetFactors(FactorType.EmissionsFactor),
            };
        }

        /// <summary>
        /// Parses the generate report data.
        /// </summary>
        private void ParseGenerateReportData()
        {
            // Wind generators
            Generators.AddRange(GetWindGenerators());

            // Coal generators
            Generators.AddRange(GetCoalGenerator());

            // Gas generators
            Generators.AddRange(GetGasGenerators());

            Logger.Info($"Generation report parsed!{Environment.NewLine}Wind Generator count: {Generators.Where(s => s.GeneratorType == GeneratorType.OffshoreWind || s.GeneratorType == GeneratorType.OnshoreWind).ToList().Count}{Environment.NewLine}Coal Generator count: {Generators.Where(s => s.GeneratorType == GeneratorType.Coal).ToList().Count}{Environment.NewLine}Gas Generator count: {Generators.Where(s => s.GeneratorType == GeneratorType.Coal).ToList().Count}{Environment.NewLine}");
        }

        #endregion

        #region XmlDocDataReading

        private BaseFactor GetFactors(FactorType factorType)
        {
            var factors = (from factor in base.SimpleStreamAxis(FileName, factorType.ToString())
                           select new BaseFactor
                           {
                               High = double.Parse(factor.ElementAnyNS(EmissionsFactorType.High.ToString()).Value),
                               Medium = double.Parse(factor.ElementAnyNS(EmissionsFactorType.Medium.ToString()).Value),
                               Low = double.Parse(factor.ElementAnyNS(EmissionsFactorType.Low.ToString()).Value),
                           }).FirstOrDefault();

            return factors;
        }

        /// <summary>
        /// Gets the wind generators.
        /// </summary>
        /// <returns></returns>
        private List<WindGenerator> GetWindGenerators()
        {
            var windGenerators = (from windGeneratorElement in base.SimpleStreamAxis(FileName, Constants.WindGenerator)
                                  select new WindGenerator
                                  {
                                      GeneratorType = (windGeneratorElement.ElementAnyNS(Constants.Location) != null) ? (windGeneratorElement.ElementAnyNS(Constants.Location).Value.Contains(Constants.Offshore, StringComparison.OrdinalIgnoreCase) ? GeneratorType.OffshoreWind : GeneratorType.OnshoreWind) : GeneratorType.OnshoreWind,
                                      IsFossilFuel = false,
                                      Name = (windGeneratorElement.ElementAnyNS(Constants.Name) != null) ? windGeneratorElement.ElementAnyNS(Constants.Name).Value : string.Empty,
                                      Location = (windGeneratorElement.ElementAnyNS(Constants.Location) != null) ? windGeneratorElement.ElementAnyNS(Constants.Location).Value : string.Empty,
                                      Generation = windGeneratorElement.ElementAnyNS(Constants.Generation) != null ? (from dayGeneration in windGeneratorElement.ElementAnyNS(Constants.Generation).Elements()
                                                                                                                      select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
                                                                                                                      {
                                                                                                                          Date = dayGeneration.ElementAnyNS(Constants.Date).Value,
                                                                                                                          Energy = Double.Parse(dayGeneration.ElementAnyNS(Constants.Energy).Value),
                                                                                                                          Price = Double.Parse(dayGeneration.ElementAnyNS(Constants.Price).Value)
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
            var gasGenerators = (from gasGeneratorElement in base.SimpleStreamAxis(FileName, Constants.GasGenerator)
                                 select new GasGenerator
                                 {
                                     GeneratorType = GeneratorType.Gas,
                                     IsFossilFuel = true,
                                     Name = (gasGeneratorElement.ElementAnyNS(Constants.Name) != null) ? gasGeneratorElement.ElementAnyNS(Constants.Name).Value : string.Empty,
                                     EmissionsRating = (gasGeneratorElement.ElementAnyNS(Constants.EmissionsRating) != null) ? Double.Parse(gasGeneratorElement.ElementAnyNS(Constants.EmissionsRating).Value) : 0.0,
                                     Generation = gasGeneratorElement.ElementAnyNS(Constants.Generation) != null ? (from dayGeneration in gasGeneratorElement.ElementAnyNS(Constants.Generation).Elements()
                                                                                                                    select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
                                                                                                                    {
                                                                                                                        Date = dayGeneration.ElementAnyNS(Constants.Date).Value,
                                                                                                                        Energy = Double.Parse(dayGeneration.ElementAnyNS(Constants.Energy).Value),
                                                                                                                        Price = Double.Parse(dayGeneration.ElementAnyNS(Constants.Price).Value)
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
            var gasGenerators = (from coalGeneratorElement in base.SimpleStreamAxis(FileName, Constants.CoalGenerator)
                                 select new CoalGenerator
                                 {
                                     GeneratorType = GeneratorType.Coal,
                                     IsFossilFuel = true,
                                     Name = (coalGeneratorElement.ElementAnyNS(Constants.Name) != null) ? coalGeneratorElement.ElementAnyNS(Constants.Name).Value : string.Empty,
                                     EmissionsRating = (coalGeneratorElement.ElementAnyNS(Constants.EmissionsRating) != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS(Constants.EmissionsRating).Value) : 0.0,
                                     TotalHeatInput = (coalGeneratorElement.ElementAnyNS(Constants.TotalHeatInput) != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS(Constants.TotalHeatInput).Value) : 0.0,
                                     ActualNetGeneration = (coalGeneratorElement.ElementAnyNS(Constants.ActualNetGeneration) != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS(Constants.ActualNetGeneration).Value) : 0.0,
                                     Generation = coalGeneratorElement.ElementAnyNS(Constants.Generation) != null ? (from dayGeneration in coalGeneratorElement.ElementAnyNS(Constants.Generation).Elements()
                                                                                                                     select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
                                                                                                                     {
                                                                                                                         Date = dayGeneration.ElementAnyNS(Constants.Date).Value,
                                                                                                                         Energy = Double.Parse(dayGeneration.ElementAnyNS(Constants.Energy).Value),
                                                                                                                         Price = Double.Parse(dayGeneration.ElementAnyNS(Constants.Price).Value)
                                                                                                                     }).ToList() : null
                                 }).ToList();

            return gasGenerators;
        }

        #endregion
    }
}
