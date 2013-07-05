// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Services
{
    using Catel;
    using EnvDTE;
    using EnvDTE80;
    using Models;
    using Configuration = Models.Configuration;

    public class ConfigurationService : IConfigurationService
    {
        private readonly DTE2 _visualStudio;
        private Configuration _currentConfiguration;

        public ConfigurationService(DTE2 visualStudio)
        {
            Argument.IsNotNull("visualStudio", visualStudio);

            _visualStudio = visualStudio;

            OnSolutionOpened();

            _visualStudio.Events.SolutionEvents.Opened += OnSolutionOpened;
            _visualStudio.Events.SolutionEvents.AfterClosing += OnSolutionClosed;
        }

        private void OnSolutionOpened()
        {
            LoadConfigurationForCurrentSolution();
        }

        private void OnSolutionClosed()
        {
            _currentConfiguration = null;
        }

        public Configuration LoadConfigurationForCurrentSolution()
        {
            if (_currentConfiguration == null)
            {
                var currentSolution = _visualStudio.Solution;
                if (currentSolution != null && !string.IsNullOrWhiteSpace(currentSolution.FullName))
                {
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
                    currentSolution.SaveConfiguration(_currentConfiguration);
                }
            }
        }
    }
}