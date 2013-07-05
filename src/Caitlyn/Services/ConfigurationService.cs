// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Services
{
    using Caitlyn.Models;

    using Catel;

    using EnvDTE80;

    public class ConfigurationService : IConfigurationService
    {
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