// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.ObjectModel;

    using Caitlyn.Models;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    /// <summary>
    /// Configuration view model.
    /// </summary>
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationViewModel" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="messageService">The message service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration" /> is <c>null</c>.</exception>
        public ConfigurationViewModel(Configuration configuration, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => configuration);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            Configuration = configuration;

            Add = new Command(OnAddExecute);
            Edit = new Command(OnEditExecute, OnEditCanExecute);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
            ManageProjectMappings = new Command(OnManageProjectMappingsExecute);
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
                return "Modify configuration";
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        [Model]
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Gets or sets the list of root projects.
        /// </summary>
        [ViewModelToModel("Configuration")]
        public ObservableCollection<RootProject> RootProjects { get; set; }

        /// <summary>
        /// Gets or sets the selected root project.
        /// </summary>
        public RootProject SelectedRootProject { get; set; }

        [ViewModelToModel("Configuration")]
        public bool EnableAutoLink { get; set; }
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

        /// <summary>
        /// Gets the ManageProjectMappings command.
        /// </summary>
        public Command ManageProjectMappings { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private void OnAddExecute()
        {
            var rootProject = new RootProject();

            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<RootProjectViewModel>(rootProject);

            if (_uiVisualizerService.ShowDialog(vm) ?? false)
            {
                RootProjects.Add(rootProject);
                SelectedRootProject = rootProject;
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
            return SelectedRootProject != null;
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private void OnEditExecute()
        {
            var rootProject = SelectedRootProject;
            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<RootProjectViewModel>(rootProject);

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
            return SelectedRootProject != null;
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            if (_messageService.Show("Are you sure you want to remove the selected root project including all it's settings?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                RootProjects.Remove(SelectedRootProject);
                SelectedRootProject = null;
            }
        }

        /// <summary>
        /// Method to invoke when the ManageProjectMappings command is executed.
        /// </summary>
        private void OnManageProjectMappingsExecute()
        {
            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<ProjectMappingsViewModel>(Configuration);
            _uiVisualizerService.ShowDialog(vm);
        }
        #endregion

        #endregion
    }
}