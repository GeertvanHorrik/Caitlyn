// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationHelper.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System;
    using System.IO;

    using Catel;
    using Catel.Data;
    using Catel.Logging;

    using EnvDTE;

    public static class ConfigurationHelper
    {
        #region Constants
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        /// <summary>
        /// Gets the name of the configuration file for the specified solution.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <returns>The name of the configuration file for the specified solution.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="solution" /> is <c>null</c>.</exception>
        public static string GetConfigurationFileName(this Solution solution)
        {
            Argument.IsNotNull("solution", solution);

            return Path.Combine(Path.GetDirectoryName(solution.FileName), "Caitlyn.xml");
        }

        /// <summary>
        /// Loads the configuration of creates a default one if no configuration is found.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <returns>The loaded configuration or the newly created one.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="solution" /> is <c>null</c>.</exception>
        public static Configuration LoadConfiguration(this Solution solution)
        {
            Argument.IsNotNull("solution", solution);

            string fileName = solution.GetConfigurationFileName();

            Configuration configuration = null;

            try
            {
                if (File.Exists(fileName))
                {
                    using (var fileStream = File.OpenRead(fileName))
                    {
                        configuration = Configuration.Load(fileStream, SerializationMode.Xml);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to load configuration, falling back to default configuration");
            }

            if (configuration == null)
            {
                configuration = new Configuration();

                var allProjects = solution.GetAllProjects();
                foreach (Project project in allProjects)
                {
                    if (project.GetProjectType() == ProjectType.NET35)
                    {
                        configuration.RootProjects.Add(new RootProject { Name = project.Name });
                    }
                }
            }

            return configuration;
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <param name="solution">The solution to save the configuration for.</param>
        /// <param name="configuration">The configuration to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="solution" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration" /> is <c>null</c>.</exception>
        public static void SaveConfiguration(this Solution solution, Configuration configuration)
        {
            Argument.IsNotNull("solution", solution);
            Argument.IsNotNull("configuration", configuration);

            string configurationFileName = solution.GetConfigurationFileName();
            configuration.Save(configurationFileName, SerializationMode.Xml);
        }
        #endregion
    }
}