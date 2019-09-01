using System;
using System.Collections.Generic;
using System.Configuration;

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
        /// The file name to process
        /// </summary>
        public static string FileNameToProcess = ConfigurationManager.AppSettings["FileNameToProcess"];

        /// <summary>
        /// The output file folder
        /// </summary>
        public static string OutputFileFolder = ConfigurationManager.AppSettings["OutputFileFolder"];

        /// <summary>
        /// The output file name
        /// </summary>
        public static string OutputFileName = ConfigurationManager.AppSettings["OutputFileName"];

        /// <summary>
        /// The reference data file full name
        /// </summary>
        public static string ReferenceDataFileFullName = ConfigurationManager.AppSettings["ReferenceDataFileFullName"];

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

        /// <summary>
        /// GeneratorType
        /// </summary>
        public enum GeneratorType
        {
            OffshoreWind,
            OnshoreWind,
            Coal,
            Gas
        }

        /// <summary>
        /// FactorType
        /// </summary>
        public enum FactorType
        {
            ValueFactor,
            EmissionFactor
        }

        /// <summary>
        /// ValueFactorType
        /// </summary>
        public enum ValueFactorType
        {
            Low,
            Medium,
            High
        }

        public enum EmissionFactorType
        {
            NA,
            Low,
            Medium,
            High
        }

        #endregion

        #region GeneratorsTypeToFactor Maping

        /// <summary>
        /// The generators type to factor mapper
        /// </summary>
        public static Dictionary<GeneratorType, Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>> GeneratorsTypeToFactorMapper = new Dictionary<GeneratorType, Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>>
        {
            { GeneratorType.OffshoreWind,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Low),new KeyValuePair<FactorType, EmissionFactorType>(FactorType.EmissionFactor,EmissionFactorType.NA))},
            { GeneratorType.OnshoreWind,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.High),new KeyValuePair<FactorType, EmissionFactorType>(FactorType.EmissionFactor,EmissionFactorType.NA))},
            { GeneratorType.Gas,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Medium),new KeyValuePair<FactorType, EmissionFactorType>(FactorType.EmissionFactor,EmissionFactorType.Medium))},
            { GeneratorType.Coal,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Medium),new KeyValuePair<FactorType, EmissionFactorType>(FactorType.EmissionFactor,EmissionFactorType.High))},
         };

        #endregion
    }
}
