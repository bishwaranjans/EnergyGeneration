using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Domain.SeedWork
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        #region Configuration

        /// <summary>
        /// The folder to watch
        /// </summary>
        public static string FolderToWatch = ConfigurationManager.AppSettings["FolderToWatch"];

        /// <summary>
        /// The output file folder
        /// </summary>
        public static string OutputFileFolder = ConfigurationManager.AppSettings["OutputFileFolder"];

        /// <summary>
        /// The file name to process
        /// </summary>
        public static string FileNameToProcess = ConfigurationManager.AppSettings["FileNameToProcess"];

        #endregion

        #region Enums

        /// <summary>
        /// Suported ParserType
        /// </summary>
        public enum ParserType
        {
            Xml,
            // e.g. CSV or more depending upon the supported format       
        }

        #endregion
    }
}
