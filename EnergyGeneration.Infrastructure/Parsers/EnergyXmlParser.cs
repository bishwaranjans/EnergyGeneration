using EnergyGeneration.Domain.Base;
using EnergyGeneration.Domain.Entities.GenerationReportEntities;
using EnergyGeneration.Domain.Entities.ReferenceDataEntities;
using EnergyGeneration.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private ReferenceData ReferenceData;
        private GenerationReport GenerationReportData;
        private List<DailyGeneration> DailyGenerations = new List<DailyGeneration>();

        private Dictionary<string, double> TotalOnshoreWindGeneration;
        private Dictionary<string, double> TotalOffshoreWindGeneration;
        private Dictionary<string, double> TotalGasGenerationByName;
        private Dictionary<string, double> TotalCoalGenerationByName;

        private Dictionary<string, double> TotalDailyGenerationByName;
        private List<DailyEmission> DailyEmissions = new List<DailyEmission>();
        private List<ActualHeatRate> ActualHeatRates = new List<ActualHeatRate>();
        private List<DailyEmission> MaxEmissionGenerators;

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
                    CalculateDailyGenerationValue();
                    CalculateDailyEmissionValue();
                    CalculateActualHeatRateValue();
                    GenerationOutput();
                }
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"Error occurred! Message : {ex.Message}. StackTrace : {ex.StackTrace}");
                System.Console.ForegroundColor = ConsoleColor.Green;
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
                                new XElement("GenerationOutput",
                                                   new XElement("Totals", from generation in TotalDailyGenerationByName
                                                                          select new XElement("Generator",
                                                                                     new XElement("Name", generation.Key),
                                                                                     new XElement("Total", generation.Value))),
                                                   new XElement("MaxEmissionGenerators", from generation in MaxEmissionGenerators
                                                                                         select new XElement("Day",
                                                                                                    new XElement("Name", generation.GenerationTypeName),
                                                                                                    new XElement("Date", generation.Date),
                                                                                                    new XElement("Emission", generation.DailyEmissionValue))),
                                                  from actualHeatRate in ActualHeatRates
                                                  select new XElement("ActualHeatRates",
                                                             new XElement("Name", actualHeatRate.Name),
                                                             new XElement("HeatRate", actualHeatRate.HeatRate))
                                            )
                                );

            // Save
            var fileFullName = Path.Combine(OutputFileFolder, OutputFileName);
            doc.Save(fileFullName);
        }

        /// <summary>
        /// Calculates the daily generation value.
        /// </summary>
        public void CalculateDailyGenerationValue()
        {
            // Dailywise wind generation
            foreach (var windGeneration in GenerationReportData.Wind)
            {
                foreach (var dayWiseWindGeneration in windGeneration.Generation)
                {
                    var windGenerationDay = dayWiseWindGeneration.Date;
                    double valueFactorMappingReferenceData = 1;

                    if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(windGeneration.GeneratorType))
                    {
                        var valueFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[windGeneration.GeneratorType];
                        var valueFactorMapping = valueFactorMappingTuple.Item1.Value;

                        switch (valueFactorMapping)
                        {
                            case ValueFactorType.Low:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Low;
                                break;
                            case ValueFactorType.Medium:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Medium;
                                break;
                            case ValueFactorType.High:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.High;
                                break;
                        };

                    }
                    var windGenerationByDay = dayWiseWindGeneration.Energy * dayWiseWindGeneration.Price * valueFactorMappingReferenceData;

                    var windDailyGeneration = new DailyGeneration
                    {
                        Date = windGenerationDay,
                        GenerationTypeName = windGeneration.Name,
                        GeneratorType = windGeneration.GeneratorType,
                        GenerationValue = windGenerationByDay
                    };

                    DailyGenerations.Add(windDailyGeneration);
                }
            }

            // Gas generation
            foreach (var gasGeneration in GenerationReportData.Gas)
            {
                foreach (var dayWiseGasGeneration in gasGeneration.Generation)
                {
                    var gasGenerationDay = dayWiseGasGeneration.Date;
                    double valueFactorMappingReferenceData = 1;

                    if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(gasGeneration.GeneratorType))
                    {
                        var valueFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[gasGeneration.GeneratorType];
                        var valueFactorMapping = valueFactorMappingTuple.Item1.Value;

                        switch (valueFactorMapping)
                        {
                            case ValueFactorType.Low:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Low;
                                break;
                            case ValueFactorType.Medium:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Medium;
                                break;
                            case ValueFactorType.High:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.High;
                                break;
                        };

                    }
                    var gasGenerationByDay = dayWiseGasGeneration.Energy * dayWiseGasGeneration.Price * valueFactorMappingReferenceData;

                    var gasDailyGeneration = new DailyGeneration
                    {
                        Date = gasGenerationDay,
                        GenerationTypeName = gasGeneration.Name,
                        GeneratorType = GeneratorType.Gas,
                        GenerationValue = gasGenerationByDay
                    };

                    DailyGenerations.Add(gasDailyGeneration);
                }
            }

            // Coal generation
            foreach (var coalGeneration in GenerationReportData.Coal)
            {
                foreach (var dayWiseCoalGeneration in coalGeneration.Generation)
                {
                    var coalGenerationDay = dayWiseCoalGeneration.Date;
                    double valueFactorMappingReferenceData = 1;

                    if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(coalGeneration.GeneratorType))
                    {
                        var valueFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[coalGeneration.GeneratorType];
                        var valueFactorMapping = valueFactorMappingTuple.Item1.Value;

                        switch (valueFactorMapping)
                        {
                            case ValueFactorType.Low:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Low;
                                break;
                            case ValueFactorType.Medium:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.Medium;
                                break;
                            case ValueFactorType.High:
                                valueFactorMappingReferenceData = ReferenceData.ValueFactor.High;
                                break;
                        };

                    }
                    var coalGenerationByDay = dayWiseCoalGeneration.Energy * dayWiseCoalGeneration.Price * valueFactorMappingReferenceData;

                    var coalDailyGeneration = new DailyGeneration
                    {
                        Date = coalGenerationDay,
                        GenerationTypeName = coalGeneration.Name,
                        GeneratorType = GeneratorType.Coal,
                        GenerationValue = coalGenerationByDay
                    };

                    DailyGenerations.Add(coalDailyGeneration);
                }
            }

            // Total offshore wind type generation
            TotalOnshoreWindGeneration = DailyGenerations.Where(s => s.GeneratorType == GeneratorType.OnshoreWind)
                                        .GroupBy(s => s.GenerationTypeName)
                                        .ToDictionary(s => s.Key, s => s.Select(p => p.GenerationValue).Sum());

            // Total onshore wind type generation
            TotalOffshoreWindGeneration = DailyGenerations.Where(s => s.GeneratorType == GeneratorType.OffshoreWind)
                                        .GroupBy(s => s.GenerationTypeName)
                                        .ToDictionary(s => s.Key, s => s.Select(p => p.GenerationValue).Sum());

            // Total Gas Generation By Name
            TotalGasGenerationByName = DailyGenerations.Where(s => s.GeneratorType == GeneratorType.Gas)
                                     .GroupBy(s => s.GenerationTypeName)
                                     .ToDictionary(s => s.Key, s => s.Select(p => p.GenerationValue).Sum());

            // Total Coal Generation By Name
            TotalCoalGenerationByName = DailyGenerations.Where(s => s.GeneratorType == GeneratorType.Coal)
                                      .GroupBy(s => s.GenerationTypeName)
                                      .ToDictionary(s => s.Key, s => s.Select(p => p.GenerationValue).Sum());

            TotalDailyGenerationByName = TotalCoalGenerationByName.Concat(TotalGasGenerationByName).Concat(TotalOffshoreWindGeneration).Concat(TotalOnshoreWindGeneration).GroupBy(d => d.Key)
             .ToDictionary(d => d.Key, d => d.First().Value);
        }

        /// <summary>
        /// Calculates the daily emission value.
        /// </summary>
        public void CalculateDailyEmissionValue()
        {
            // Dailywise wind emission
            // Wind is not emitting anything
            // No Emissions factor and value is provided in contract xml format 

            // Gas Emissions
            foreach (var gasGeneration in GenerationReportData.Gas)
            {
                foreach (var dayWiseGasGeneration in gasGeneration.Generation)
                {
                    var gasGenerationDay = dayWiseGasGeneration.Date;
                    double emissionFactorMappingReferenceData = 1;

                    if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(gasGeneration.GeneratorType))
                    {
                        var emissionFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[gasGeneration.GeneratorType];
                        var emissionFactorMapping = emissionFactorMappingTuple.Item2.Value;

                        switch (emissionFactorMapping)
                        {
                            case EmissionFactorType.NA:
                                emissionFactorMappingReferenceData = 0;
                                break;
                            case EmissionFactorType.Low:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.Low;
                                break;
                            case EmissionFactorType.Medium:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.Medium;
                                break;
                            case EmissionFactorType.High:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.High;
                                break;
                        };

                    }
                    var gasEmissionByDay = dayWiseGasGeneration.Energy * gasGeneration.EmissionsRating * emissionFactorMappingReferenceData;

                    var gasDailyEmission = new DailyEmission
                    {
                        Date = gasGenerationDay,
                        GenerationTypeName = gasGeneration.Name,
                        GeneratorType = GeneratorType.Gas,
                        DailyEmissionValue = gasEmissionByDay
                    };

                    DailyEmissions.Add(gasDailyEmission);
                }
            }

            // Coal Emissions
            foreach (var coalGeneration in GenerationReportData.Coal)
            {
                foreach (var dayWiseCoalGeneration in coalGeneration.Generation)
                {
                    var coalGenerationDay = dayWiseCoalGeneration.Date;
                    double emissionFactorMappingReferenceData = 1;

                    if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(coalGeneration.GeneratorType))
                    {
                        var emissionFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[coalGeneration.GeneratorType];
                        var emissionFactorMapping = emissionFactorMappingTuple.Item2.Value;

                        switch (emissionFactorMapping)
                        {
                            case EmissionFactorType.NA:
                                emissionFactorMappingReferenceData = 0;
                                break;
                            case EmissionFactorType.Low:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.Low;
                                break;
                            case EmissionFactorType.Medium:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.Medium;
                                break;
                            case EmissionFactorType.High:
                                emissionFactorMappingReferenceData = ReferenceData.EmissionFactor.High;
                                break;
                        };

                    }
                    var coalEmissionByDay = dayWiseCoalGeneration.Energy * coalGeneration.EmissionsRating * emissionFactorMappingReferenceData;

                    var coalDailyEmission = new DailyEmission
                    {
                        Date = coalGenerationDay,
                        GenerationTypeName = coalGeneration.Name,
                        GeneratorType = GeneratorType.Coal,
                        DailyEmissionValue = coalEmissionByDay
                    };

                    DailyEmissions.Add(coalDailyEmission);
                }
            }

            MaxEmissionGenerators = (from dailyEmission in DailyEmissions
                                     group dailyEmission by dailyEmission.Date into dateWiseGroup
                                     select new DailyEmission
                                     {
                                         Date = dateWiseGroup.Key,
                                         DailyEmissionValue = dateWiseGroup.Max(s => s.DailyEmissionValue),
                                         GenerationTypeName = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GenerationTypeName,
                                         GeneratorType = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GeneratorType
                                     }).OrderByDescending(s=>s.DailyEmissionValue).ToList();
        }

        /// <summary>
        /// Calculates the actual heat rate value.
        /// </summary>
        public void CalculateActualHeatRateValue()
        {
            // Coal Emissions
            foreach (var coalGeneration in GenerationReportData.Coal)
            {
                var coalHeatRate = coalGeneration.TotalHeatInput / coalGeneration.ActualNetGeneration;

                var coalDailyEmission = new ActualHeatRate
                {
                    Name = coalGeneration.Name,
                    HeatRate = coalHeatRate
                };

                ActualHeatRates.Add(coalDailyEmission);
            }
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
        /// Parses the reference data.
        /// </summary>
        private void ParseReferenceData()
        {
            ReferenceData = new ReferenceData()
            {
                ValueFactor = GetValueFactors(),
                EmissionFactor = GetEmissionsFactors()
            };
        }

        /// <summary>
        /// Parses the generate report data.
        /// </summary>
        private void ParseGenerateReportData()
        {
            GenerationReportData = new GenerationReport()
            {
                Wind = GetWindGenerators(),
                Coal = GetCoalGenerator(),
                Gas = GetGasGenerators()
            };

            System.Console.WriteLine($"Generation report parsed!{Environment.NewLine}Wind Generator count: {GenerationReportData.Wind.Count}{Environment.NewLine}Coal Generator count: {GenerationReportData.Coal.Count}{Environment.NewLine}Gas Generator count: {GenerationReportData.Gas.Count}{Environment.NewLine}");
        }

        /// <summary>
        /// Gets the value factors.
        /// </summary>
        /// <returns></returns>
        private BaseFactor GetValueFactors()
        {
            var valueFactor = (from factor in SimpleStreamAxis(FileName, "ValueFactor")
                               select new BaseFactor
                               {
                                   High = double.Parse(factor.ElementAnyNS("High").Value),
                                   Medium = double.Parse(factor.ElementAnyNS("Medium").Value),
                                   Low = double.Parse(factor.ElementAnyNS("Low").Value),
                               }).FirstOrDefault();

            return valueFactor;
        }

        /// <summary>
        /// Gets the Emissions factors.
        /// </summary>
        /// <returns></returns>
        private BaseFactor GetEmissionsFactors()
        {
            var emissionsFactor = (from factor in SimpleStreamAxis(FileName, "EmissionsFactor")
                                   select new BaseFactor
                                   {
                                       High = double.Parse(factor.ElementAnyNS("High").Value),
                                       Medium = double.Parse(factor.ElementAnyNS("Medium").Value),
                                       Low = double.Parse(factor.ElementAnyNS("Low").Value),
                                   }).FirstOrDefault();

            return emissionsFactor;
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
                                      IsFossilFuel = false,
                                      Name = (windGeneratorElement.ElementAnyNS("Name") != null) ? windGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                      Location = (windGeneratorElement.ElementAnyNS("Location") != null) ? windGeneratorElement.ElementAnyNS("Location").Value : string.Empty,
                                      GeneratorType = (windGeneratorElement.ElementAnyNS("Location") != null) ? (windGeneratorElement.ElementAnyNS("Location").Value.Contains("Offshore", StringComparison.OrdinalIgnoreCase) ? GeneratorType.OffshoreWind : GeneratorType.OnshoreWind) : GeneratorType.OnshoreWind,
                                      Generation = windGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in windGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                              select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
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
                                     GeneratorType = GeneratorType.Gas,
                                     IsFossilFuel = true,
                                     Name = (gasGeneratorElement.ElementAnyNS("Name") != null) ? gasGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                     EmissionsRating = (gasGeneratorElement.ElementAnyNS("EmissionsRating") != null) ? Double.Parse(gasGeneratorElement.ElementAnyNS("EmissionsRating").Value) : 0.0,
                                     Generation = gasGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in gasGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                            select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
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
                                     GeneratorType = GeneratorType.Coal,
                                     IsFossilFuel = true,
                                     Name = (coalGeneratorElement.ElementAnyNS("Name") != null) ? coalGeneratorElement.ElementAnyNS("Name").Value : string.Empty,
                                     EmissionsRating = (coalGeneratorElement.ElementAnyNS("EmissionsRating") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("EmissionsRating").Value) : 0.0,
                                     TotalHeatInput = (coalGeneratorElement.ElementAnyNS("TotalHeatInput") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("TotalHeatInput").Value) : 0.0,
                                     ActualNetGeneration = (coalGeneratorElement.ElementAnyNS("ActualNetGeneration") != null) ? Double.Parse(coalGeneratorElement.ElementAnyNS("ActualNetGeneration").Value) : 0.0,
                                     Generation = coalGeneratorElement.ElementAnyNS("Generation") != null ? (from dayGeneration in coalGeneratorElement.ElementAnyNS("Generation").Elements()
                                                                                                             select new EnergyGeneration.Domain.Entities.GenerationReportEntities.Day
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
