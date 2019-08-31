using System;
using System.IO;

namespace EnergyGeneration.Infrastructure.Validation
{
    /// <summary>
    /// ValidateFile
    /// </summary>
    public class ValidateFile
    {
        /// <summary>
        /// Validates the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.Exception">File: {fileName}</exception>
        public static void Validate(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception($"File: {fileName} not found!");
            }
        }
    }
}
