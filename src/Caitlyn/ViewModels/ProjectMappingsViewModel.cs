// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingsViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Models;

    /// <summary>
    /// ProjectMappings view model.
    /// </summary>
    public class ProjectMappingsViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsViewModel"/> class.
        /// </summary>
        public ProjectMappingsViewModel(Configuration configuration)
        {
            Argument.IsNotNull("configuration", configuration);

            ProjectMappings = configuration.ProjectMappings;

            Add = new Command(OnAddExecute);
            Edit = new Command(OnEditExecute, OnEditCanExecute);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Project mappings"; }
        }

        public ObservableCollection<ProjectMapping> ProjectMappings { get; private set; }

        public ProjectMapping SelectedProjectMapping { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the Add command.
        /// </summary>
        public Command Add { get; private set; }

        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private void OnAddExecute()
        {
            var projectMapping = new ProjectMapping();
            var vm = new ProjectMappingViewModel(projectMapping);

            var uiVisualizerService = GetService<IUIVisualizerService>();
            if (uiVisualizerService.ShowDialog(vm) ?? false)
            {
                ProjectMappings.Add(projectMapping);
                SelectedProjectMapping = projectMapping;
            }
        }

        /// <summary>
        /// Gets the Edit command.
        /// </summary>
        public Command Edit { get; private set; }

        /// <summary>
        /// Method to check whether the Edit command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnEditCanExecute()
        {
            return SelectedProjectMapping != null;
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private void OnEditExecute()
        {
            var projectMapping = SelectedProjectMapping;
            var vm = new ProjectMappingViewModel(projectMapping);

            var uiVisualizerService = GetService<IUIVisualizerService>();
            uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public Command Remove { get; private set; }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveCanExecute()
        {
            return SelectedProjectMapping != null;
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            var messageService = GetService<IMessageService>();
            if (messageService.Show("Are you sure you want to remove the selected project mapping?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                ProjectMappings.Remove(SelectedProjectMapping);
                SelectedProjectMapping = null;
            }
        }
        #endregion
    }
}