using log4net;
using System;

namespace EnergyGeneration.Console
{
    /// <summary>
    /// Energy Generation program
    /// </summary>
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Logger.Info($"Welcome to Energy Generation Report Application.{Environment.NewLine}Application started @ {DateTime.Now}");

            Bootstraper.Instance.Initialize();

            System.Console.ReadLine();
        }
    }
}
