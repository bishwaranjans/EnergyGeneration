using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnergyGeneration.Infrastructure.Factories;
using EnergyGeneration.Infrastructure.Facades;
using static EnergyGeneration.Domain.SeedWork.Constants;
using EnergyGeneration.Infrastructure.Parsers;

namespace EnergyGeneration.Tests.Factories
{
    [TestClass]
    public class XmlParserFactoryTest
    {
        [TestMethod]
        public void XmlParserFactory_Should_Be_User_For_Xml_File_Parsing()
        {
            //Arrange
            var facade = new FileParserFacade();
            var factory = new ParserFactory();

            //Act
            var type = facade.GetExtention("a.xml");
            var parser = factory.GetObject(type.ToString());

            // Assert
            Assert.IsNotNull(type);
            Assert.IsNotNull(parser);
            Assert.IsInstanceOfType(type, typeof(ParserType));
            Assert.IsInstanceOfType(parser, typeof(EnergyXmlParser));
            Assert.AreEqual(type, ParserType.Xml);
        }
    }
}
