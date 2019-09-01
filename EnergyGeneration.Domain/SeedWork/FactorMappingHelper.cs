using EnergyGeneration.Domain.Entities.ReferenceDataEntities;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Domain.SeedWork
{
    /// <summary>
    /// FactorMappingHelper
    /// </summary>
    public class FactorMappingHelper
    {
        /// <summary>
        /// Gets the value factor mapping reference.
        /// </summary>
        /// <param name="valueFactor">The value factor.</param>
        /// <param name="generatorType">Type of the generator.</param>
        /// <returns></returns>
        public static double GetValueFactorMappingReference(BaseFactor valueFactor, GeneratorType generatorType)
        {
            double valueFactorMappingReferenceData = 0;

            // Make sure to initialize the mapping information
            if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(generatorType))
            {
                var valueFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[generatorType];
                var valueFactorMapping = valueFactorMappingTuple.Item1.Value;

                switch (valueFactorMapping)
                {
                    case ValueFactorType.Low:
                        valueFactorMappingReferenceData = valueFactor.Low;
                        break;
                    case ValueFactorType.Medium:
                        valueFactorMappingReferenceData = valueFactor.Medium;
                        break;
                    case ValueFactorType.High:
                        valueFactorMappingReferenceData = valueFactor.High;
                        break;
                };
            }

            return valueFactorMappingReferenceData;
        }

        /// <summary>
        /// Gets the emission factor mapping reference.
        /// </summary>
        /// <param name="emissionFactor">The emission factor.</param>
        /// <param name="generatorType">Type of the generator.</param>
        /// <returns></returns>
        public static double GetEmissionFactorMappingReference(BaseFactor emissionFactor, GeneratorType generatorType)
        {
            double emissionFactorMappingReferenceData = 0;

            if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(generatorType))
            {
                var emissionFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[generatorType];
                var emissionFactorMapping = emissionFactorMappingTuple.Item2.Value;

                switch (emissionFactorMapping)
                {
                    case EmissionsFactorType.NA:
                        emissionFactorMappingReferenceData = 0;
                        break;
                    case EmissionsFactorType.Low:
                        emissionFactorMappingReferenceData = emissionFactor.Low;
                        break;
                    case EmissionsFactorType.Medium:
                        emissionFactorMappingReferenceData = emissionFactor.Medium;
                        break;
                    case EmissionsFactorType.High:
                        emissionFactorMappingReferenceData = emissionFactor.High;
                        break;
                };
            }

            return emissionFactorMappingReferenceData;
        }
    }
}
