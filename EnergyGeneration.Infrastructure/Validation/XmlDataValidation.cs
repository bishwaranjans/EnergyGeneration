using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnergyGeneration.Infrastructure.Validation
{
    /// <summary>
    /// XmlDataValidation
    /// </summary>
    public static class XmlDataValidation
    {
        /// <summary>
        /// Validates the specified XML object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObject">The XML object.</param>
        /// <returns></returns>
        public static List<string> Validate<T>(this T xmlObject)
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(xmlObject);
            List<string> errors = new List<string>();
            bool Isvalid = Validator.TryValidateObject(xmlObject, context, results);
            if (!Isvalid)
            {
                foreach (var validationResult in results)
                {
                    errors.Add(validationResult.ErrorMessage);
                }
            }
            return errors;
        }
    }

}
