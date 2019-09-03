using EnergyGeneration.Domain.SeedWork;
using System.Collections.Generic;
using static EnergyGeneration.Domain.SeedWork.Constants;

namespace EnergyGeneration.Domain.Entities.GenerationReportEntities
{
    /// <summary>
    /// FossilFuelGenerator
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Entities.GenerationReportEntities.BaseGenerator" />
    public class FossilFuelGenerator : BaseGenerator
    {
        #region Properties

        /// <summary>
        /// Gets or sets the emissions rating.
        /// </summary>
        /// <value>
        /// The emissions rating.
        /// </value>
        public double EmissionsRating { get; set; }

        /// <summary>
        /// Gets the generator daily emissions.
        /// </summary>
        /// <value>
        /// The generator daily emissions.
        /// </value>
        public List<DailyEmission> GeneratorDailyEmissions
        {
            get
            {
                var dailyEmissions = new List<DailyEmission>();

                foreach (var dayWiseGeneratorGeneration in Generation)
                {
                    var generatorGenerationDay = dayWiseGeneratorGeneration.Date;
                    var referenceFactor = FactorMappingHelper.GetValueFactorMappingReference(Constants.ReferenceData.EmissionsFactor, FactorType.EmissionsFactor, GeneratorType);
                    var generatorEmissionByDay = dayWiseGeneratorGeneration.Energy * this.EmissionsRating * referenceFactor;

                    var generatorDailyEmission = new DailyEmission
                    {
                        Date = generatorGenerationDay,
                        GenerationTypeName = Name,
                        GeneratorType = GeneratorType,
                        DailyEmissionValue = generatorEmissionByDay
                    };

                    dailyEmissions.Add(generatorDailyEmission);
                }

                return dailyEmissions;
            }
        }

        #endregion
    }
}
