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
            EmissionsFactor
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

        public enum EmissionsFactorType
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
        public static Dictionary<GeneratorType, Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>> GeneratorsTypeToFactorMapper = new Dictionary<GeneratorType, Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>>
        {
            { GeneratorType.OffshoreWind,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Low),new KeyValuePair<FactorType, EmissionsFactorType>(FactorType.EmissionsFactor,EmissionsFactorType.NA))},
            { GeneratorType.OnshoreWind,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.High),new KeyValuePair<FactorType, EmissionsFactorType>(FactorType.EmissionsFactor,EmissionsFactorType.NA))},
            { GeneratorType.Gas,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Medium),new KeyValuePair<FactorType, EmissionsFactorType>(FactorType.EmissionsFactor,EmissionsFactorType.Medium))},
            { GeneratorType.Coal,new Tuple<KeyValuePair<FactorType, ValueFactorType>, KeyValuePair<FactorType, EmissionsFactorType>>(new KeyValuePair<FactorType, ValueFactorType>(FactorType.ValueFactor,ValueFactorType.Medium),new KeyValuePair<FactorType, EmissionsFactorType>(FactorType.EmissionsFactor,EmissionsFactorType.High))},
         };

        // This will be initialized during the application first time run
        public static Entities.ReferenceDataEntities.ReferenceData ReferenceData;

        #endregion

        #region Xml Nodes

        public const string GenerationOutput = "GenerationOutput";
        public const string Totals = "Totals";
        public const string Generator = "Generator";
        public const string Name = "Name";
        public const string Total = "Total";
        public const string MaxEmissionGenerators = "MaxEmissionGenerators";
        public const string Day = "Day";
        public const string Date = "Date";
        public const string Emission = "Emission";
        public const string ActualHeatRates = "ActualHeatRates";
        public const string HeatRate = "HeatRate";

        public const string WindGenerator = "WindGenerator";
        public const string Location = "Location";
        public const string Offshore = "Offshore";
        public const string Generation = "Generation";
        public const string Energy = "Energy";
        public const string Price = "Price";

        public const string GasGenerator = "GasGenerator";
        public const string CoalGenerator = "CoalGenerator";
        public const string EmissionsRating = "EmissionsRating";
        public const string ActualNetGeneration = "ActualNetGeneration";
        public const string TotalHeatInput = "TotalHeatInput";

        #endregion
    }
}
