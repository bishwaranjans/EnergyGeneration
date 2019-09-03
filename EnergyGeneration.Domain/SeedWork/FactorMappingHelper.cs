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
        /// <param name="factor">The factor.</param>
        /// <param name="factorType">Type of the factor.</param>
        /// <param name="generatorType">Type of the generator.</param>
        /// <returns></returns>
        public static double GetValueFactorMappingReference(BaseFactor factor, FactorType factorType, GeneratorType generatorType)
        {
            double factorMappingReferenceData = 0;

            // Make sure to initialize the mapping information
            if (Constants.GeneratorsTypeToFactorMapper.ContainsKey(generatorType))
            {
                if (factorType == FactorType.ValueFactor)
                {
                    var valueFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[generatorType];
                    var valueFactorMapping = valueFactorMappingTuple.Item1.Value;

                    switch (valueFactorMapping)
                    {
                        case ValueFactorType.Low:
                            factorMappingReferenceData = factor.Low;
                            break;
                        case ValueFactorType.Medium:
                            factorMappingReferenceData = factor.Medium;
                            break;
                        case ValueFactorType.High:
                            factorMappingReferenceData = factor.High;
                            break;
                    };
                }
                else
                {
                    var emissionFactorMappingTuple = Constants.GeneratorsTypeToFactorMapper[generatorType];
                    var emissionFactorMapping = emissionFactorMappingTuple.Item2.Value;

                    switch (emissionFactorMapping)
                    {
                        case EmissionsFactorType.NA:
                            factorMappingReferenceData = 0;
                            break;
                        case EmissionsFactorType.Low:
                            factorMappingReferenceData = factor.Low;
                            break;
                        case EmissionsFactorType.Medium:
                            factorMappingReferenceData = factor.Medium;
                            break;
                        case EmissionsFactorType.High:
                            factorMappingReferenceData = factor.High;
                            break;
                    };
                }

            }

            return factorMappingReferenceData;
        }
    }
}
