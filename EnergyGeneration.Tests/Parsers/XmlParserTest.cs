using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnergyGeneration.Infrastructure.Facades;
using EnergyGeneration.Infrastructure.Factories;
using System.IO;
using EnergyGeneration.Domain.SeedWork;
using EnergyGeneration.Infrastructure.Validation;
using EnergyGeneration.Domain.Entities.GenerationReportEntities;
using EnergyGeneration.Domain.Entities.ReferenceDataEntities;
using System.Linq;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Tests.Parsers
{
    [TestClass]
    public class XmlParserTest
    {
        [TestMethod]
        public void Parsing_ReferenceData_Success()
        {
            // Arrange
            var facade = new FileParserFacade();
            var factory = new ParserFactory();
            string fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFile", "ReferenceData.xml");

            // Act
            var type = facade.GetExtention(fullFileName);
            var parser = factory.GetObject(type.ToString());
            parser.IsReferenceData = true;
            parser.FileName = fullFileName;
            parser.Read();

            // Assert
            Assert.IsInstanceOfType(Constants.ReferenceData.ValueFactor, typeof(Domain.Entities.ReferenceDataEntities.BaseFactor));
            Assert.IsNotNull(Constants.ReferenceData);
            Assert.AreEqual(Constants.ReferenceData.ValueFactor.High, 0.946);
            Assert.AreEqual(Constants.ReferenceData.EmissionsFactor.Low, 0.312);
        }

        [TestMethod]
        public void Parsing_NotFoundFile_Validation_Fail()
        {
            // Arrange
            string fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFile", "DoesNotExistsFile.xml");

            // Act
            var isValid = ValidateFile.Validate(fullFileName);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Parsing_BadFormatFile_Resultant_ParseValue_Null()
        {
            // Arrange
            var facade = new FileParserFacade();
            var factory = new ParserFactory();
            string fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFile", "BadFomratGenerationOutput.xml");

            // Act
            var type = facade.GetExtention(fullFileName);
            var parser = factory.GetObject(type.ToString());
            parser.IsReferenceData = true;
            parser.FileName = fullFileName;
            parser.Read();

            // Assert
            Assert.IsNull(Constants.ReferenceData.ValueFactor);
            Assert.IsNull(Constants.ReferenceData.EmissionsFactor);
        }

        [TestMethod]
        public void Parsing_GenerationReport_Success()
        {
            // Arrange
            var facade = new FileParserFacade();
            var factory = new ParserFactory();
            string referenceDatafullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFile", "ReferenceData.xml");
            string generationOutputfullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDataFile", "GenerationReport.xml");

            // Act
            // Initializes reference data first
            var type = facade.GetExtention(referenceDatafullFileName);
            var parser = factory.GetObject(type.ToString());
            parser.IsReferenceData = true;
            parser.FileName = referenceDatafullFileName;
            parser.Read();

            // read generation data
            parser.IsReferenceData = false;
            parser.FileName = generationOutputfullFileName;
            parser.Read();

            // Assert
            Assert.IsInstanceOfType(Constants.ReferenceData.ValueFactor, typeof(BaseFactor));
            Assert.IsNotNull(Constants.ReferenceData);
            Assert.AreEqual(Constants.ReferenceData.ValueFactor.High, 0.946);
            Assert.AreEqual(Constants.ReferenceData.EmissionsFactor.Low, 0.312);

            Assert.IsInstanceOfType(parser.Generators.FirstOrDefault(), typeof(BaseGenerator));
            Assert.IsNotNull(parser.Generators);
            Assert.AreEqual(parser.Generators.Count, 4);
            Assert.AreEqual(parser.Generators.Where(s => s.GeneratorType == Constants.GeneratorType.OnshoreWind).ToList().Count, 1);
            Assert.IsNotNull(parser.Generators.Where(s => s.Name == "Wind[Onshore]").FirstOrDefault());

            var windOnshoreTotalEmission = parser.Generators.Where(s => s.Name == "Wind[Onshore]").FirstOrDefault().TotalGeneratorGenerationByName.Value;
            Assert.AreEqual(windOnshoreTotalEmission, 4869.4539173939993);

            var maxEmission = (from dailyEmission in parser.Generators.Where(s => s.IsFossilFuel).SelectMany(s => ((FossilFuelGenerator)s).GeneratorDailyEmissions)
                               group dailyEmission by dailyEmission.Date into dateWiseGroup
                               select new DailyEmission
                               {
                                   Date = dateWiseGroup.Key,
                                   DailyEmissionValue = dateWiseGroup.Max(s => s.DailyEmissionValue),
                                   GenerationTypeName = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GenerationTypeName,
                                   GeneratorType = dateWiseGroup.Where(s => s.DailyEmissionValue == dateWiseGroup.Max(p => p.DailyEmissionValue)).First().GeneratorType
                               }).OrderByDescending(s => s.DailyEmissionValue).FirstOrDefault().DailyEmissionValue;

            Assert.AreEqual(maxEmission, 137.175004008);

            var firstCoalHeatRate = parser.Generators.Where(s => s.GeneratorType == GeneratorType.Coal).Select(s => ((CoalGenerator)s).ActualHeatRate).FirstOrDefault();
            Assert.AreEqual(firstCoalHeatRate.HeatRate, 12.849293200);
        }
    }
}
