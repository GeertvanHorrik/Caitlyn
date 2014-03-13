// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Services
{
    using Caitlyn.Models;

    using Catel;
    using Catel.Logging;
    using EnvDTE80;

    public class ConfigurationService : IConfigurationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Fields
        private readonly DTE2 _visualStudio;

        private Configuration _currentConfiguration;
        #endregion

        #region Constructors
        public ConfigurationService(DTE2 visualStudio)
        {
            Argument.IsNotNull("visualStudio", visualStudio);

            _visualStudio = visualStudio;

            OnSolutionOpened();

            _visualStudio.Events.SolutionEvents.Opened += OnSolutionOpened;
            _visualStudio.Events.SolutionEvents.AfterClosing += OnSolutionClosed;
        }
        #endregion

        #region IConfigurationService Members
        public Configuration LoadConfigurationForCurrentSolution()
        {
            if (_currentConfiguration == null)
            {
                var currentSolution = _visualStudio.Solution;
                if (currentSolution != null && !string.IsNullOrWhiteSpace(currentSolution.FullName))
                {
                    Log.Info("Loading configuration for current solution: '{0}'", currentSolution.FullName);

                    _currentConfiguration = currentSolution.LoadConfiguration();
                }
            }

            return _currentConfiguration;
        }

        public void SaveConfigurationForCurrentSolution()
        {
            if (_currentConfiguration != null)
            {
                var currentSolution = _visualStudio.Solution;
                if (currentSolution != null)
                {
                    Log.Info("Saving configuration for current solution: '{0}'", currentSolution.FullName);

                    currentSolution.SaveConfiguration(_currentConfiguration);
                }
            }
        }
        #endregion

        #region Methods
        private void OnSolutionOpened()
        {
            LoadConfigurationForCurrentSolution();
        }

        private void OnSolutionClosed()
        {
            _currentConfiguration = null;
        }
        #endregion
    }
}