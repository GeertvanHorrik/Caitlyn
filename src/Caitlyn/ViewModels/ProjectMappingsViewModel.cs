// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingsViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System.Collections.ObjectModel;

    using Caitlyn.Models;

    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    /// <summary>
    /// ProjectMappings view model.
    /// </summary>
    public class ProjectMappingsViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly IViewModelFactory _viewModelFactory;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsViewModel" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="messageService">The message service.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public ProjectMappingsViewModel(Configuration configuration, IUIVisualizerService uiVisualizerService, 
            IMessageService messageService, IViewModelFactory viewModelFactory)
        {
            Argument.IsNotNull(() => configuration);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => viewModelFactory);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _viewModelFactory = viewModelFactory;

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
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return "Project mappings";
            }
        }

        public ObservableCollection<ProjectMapping> ProjectMappings { get; private set; }

        public ProjectMapping SelectedProjectMapping { get; set; }
        #endregion

        #region Commands

        #region Properties
        /// <summary>
        /// Gets the Add command.
        /// </summary>
        public Command Add { get; private set; }

        /// <summary>
        /// Gets the Edit command.
        /// </summary>
        public Command Edit { get; private set; }

        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public Command Remove { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private void OnAddExecute()
        {
            var projectMapping = new ProjectMapping();
            var vm = _viewModelFactory.CreateViewModel<ProjectMappingViewModel>(projectMapping);

            if (_uiVisualizerService.ShowDialog(vm) ?? false)
            {
                ProjectMappings.Add(projectMapping);
                SelectedProjectMapping = projectMapping;
            }
        }

        /// <summary>
        /// Method to check whether the Edit command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
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
            var vm = _viewModelFactory.CreateViewModel<ProjectMappingViewModel>(projectMapping);

            _uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
        private bool OnRemoveCanExecute()
        {
            return SelectedProjectMapping != null;
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            if (_messageService.Show("Are you sure you want to remove the selected project mapping?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                ProjectMappings.Remove(SelectedProjectMapping);
                SelectedProjectMapping = null;
            }
        }
        #endregion

        #endregion
    }
}