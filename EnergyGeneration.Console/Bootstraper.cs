using EnergyGeneration.Domain.SeedWork;
using EnergyGeneration.Infrastructure.Facades;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace EnergyGeneration.Console
{
    /// <summary>
    /// Bootstraper class for initialisation
    /// </summary>
    class Bootstraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<Bootstraper> lazy = new Lazy<Bootstraper>(() => new Bootstraper());
        private static FileSystemWatcher fileWatcher;
        private static FileParserFacade facade;

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

            // Begin watching.
            fileWatcher.EnableRaisingEvents = true;

            // Instantiate the facade
            facade = new FileParserFacade();

            // Initialize the reference data          
            facade.ParseFile(Constants.ReferenceDataFileFullName, true);
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

                Logger.Info($"{Environment.NewLine}File: {e.FullPath} {e.ChangeType}");
                Logger.Warn($"File: {Constants.FileNameToProcess} changes are in progress.{Environment.NewLine}Waiting to finish the changes and meanwhile listener has been disabled. Any new addition/modification of the file will not process.");

                // Wait for the whole file to get copied to watch folder
                WaitForFile(e.FullPath);

                // Begin reading
                BeginReading(e.FullPath);
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

        /// <summary>
        /// Begins the reading.
        /// </summary>
        /// <param name="fullFileName">Full name of the file.</param>
        private static void BeginReading(string fullFileName)
        {
            // Begin reading
            facade.ParseFile(fullFileName);
        }
    }
}
