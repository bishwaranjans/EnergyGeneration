using EnergyGeneration.Domain.SeedWork;
using EnergyGeneration.Infrastructure.Facades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyGeneration.Console
{
    /// <summary>
    /// Bootstraper class for initialisation
    /// </summary>
    class Bootstraper
    {
        private static readonly Lazy<Bootstraper> lazy = new Lazy<Bootstraper>(() => new Bootstraper());
        private static FileSystemWatcher fileWatcher;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Bootstraper Instance { get { return lazy.Value; } }

        /// <summary>
        /// Prevents a default instance of the <see cref="Bootstraper"/> class from being created.
        /// </summary>
        private Bootstraper()
        {
        }

        /// <summary>
        /// Initializes the related work for this instance.
        /// </summary>
        public void Initialize()
        {
            // Create a new FileSystemWatcher and set its properties.
            fileWatcher = new FileSystemWatcher(Constants.FolderToWatch);

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            fileWatcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            // Only watch text files.
            fileWatcher.Filter = Constants.FileNameToProcess;

            // Add event handlers.
            fileWatcher.Changed += OnChanged;
            fileWatcher.Created += OnChanged;

            // Begin watching.
            fileWatcher.EnableRaisingEvents = true;

        }

        /// <summary>
        /// Called when [changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                // Stop listening and Start processing
                fileWatcher.EnableRaisingEvents = false;

                /* do my stuff once asynchronously */
                // Specify what is done when a file is changed, created, or deleted.
                System.Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

                System.Console.WriteLine($"File: {Constants.FileNameToProcess} creation is in progress. Listener has been disabled while processing and hence any new addition/modification of the file will not process.");

                // Wait for the whole file to get copied to watch folder
                WaitForFile(e.FullPath);

                System.Console.WriteLine($"File: {Constants.FileNameToProcess} is available now and processing started.");

                Stopwatch sw = new Stopwatch();
                sw.Start();

                // Begin reading
                FileParserFacade facade = new FileParserFacade();
                facade.ParseFile(e.FullPath);

                // Begin Report generation

                sw.Stop();
            }

            finally
            {
                fileWatcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Waits for file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        private static void WaitForFile(string fullPath)
        {
            while (true)
            {
                try
                {
                    using (StreamReader stream = new StreamReader(fullPath))
                    {
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
