using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Console
{
    /// <summary>
    /// Energy Generation program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"Welcome to Energy Generation Report Application.{Environment.NewLine}Application started @ {DateTime.Now}");

            Bootstraper.Instance.Initialize();

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.ReadLine();
        }
    }
}
