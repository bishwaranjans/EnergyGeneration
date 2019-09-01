﻿using log4net;
using System;
using System.IO;

namespace EnergyGeneration.Infrastructure.Validation
{
    /// <summary>
    /// ValidateFile
    /// </summary>
    public class ValidateFile
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Validates the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.Exception">File: {fileName}</exception>
        public static void Validate(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Logger.Error($"File: {fileName} not found!");
                throw new Exception($"File: {fileName} not found!");
            }
        }
    }
}
