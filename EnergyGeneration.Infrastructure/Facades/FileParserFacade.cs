using EnergyGeneration.Domain.Base;
using EnergyGeneration.Infrastructure.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Infrastructure.Facades
{
    /// <summary>
    /// FileParserFacade
    /// </summary>
    public class FileParserFacade
    {
        ParserFactory Factory = null;
        BaseParser parser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileParserFacade"/> class.
        /// </summary>
        public FileParserFacade()
        {
            Factory = new ParserFactory();

        }

        /// <summary>
        /// Parses the file.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        public void ParseFile(string FileName)
        {
            //ValidateFile.Validate(FileName);
            ParserType type = GetExtention(FileName);
            parser = Factory.GetObject(type.ToString());
            parser.FileName = FileName;
            parser.Read();


        }

        /// <summary>
        /// Gets the extention.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public ParserType GetExtention(string FileName)
        {
            string strFileType = Path.GetExtension(FileName).ToLower();
            switch (strFileType)
            {
                case ".xml":
                    return ParserType.Xml;
                default:
                    return ParserType.Xml;
            }
        }
    }
}
