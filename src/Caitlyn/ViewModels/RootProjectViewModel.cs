// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootProjectViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Caitlyn.Models;
    using Caitlyn.Services;

    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    /// <summary>
    /// RootProject view model.
    /// </summary>
    public class RootProjectViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RootProjectViewModel" /> class.
        /// </summary>
        /// <param name="rootProject">The root project.</param>
        /// <param name="visualStudioService">The visual studio service.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="messageService">The message service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        public RootProjectViewModel(RootProject rootProject, IVisualStudioService visualStudioService, IUIVisualizerService uiVisualizerService,
            IMessageService messageService)
        {
            Argument.IsNotNull(() => rootProject);
            Argument.IsNotNull(() => visualStudioService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;

            AvailableProjects = new List<string>();
            var availableProjects = visualStudioService.GetAllProjects();

            foreach (var availableProject in availableProjects)
            {
                AvailableProjects.Add(availableProject.Name);
            }

            RootProject = rootProject;

            Add = new Command(OnAddExecute);
            Edit = new Command(OnEditExecute, OnEditCanExecute);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
        }
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
            var rule = new Rule();
            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<RuleViewModel>(RootProject, rule);

            if (_uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Rules.Add(rule);
                SelectedRule = rule;
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
            return (SelectedRule != null);
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private void OnEditExecute()
        {
            var rule = SelectedRule;
            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<RuleViewModel>(RootProject, rule);

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
            return (SelectedRule != null);
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            if (_messageService.Show("Are you sure you want to remove the selected rule including all it's settings?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                Rules.Remove(SelectedRule);
                SelectedRule = null;
            }
        }
        #endregion

        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return string.Format("Root project '{0}'", RootProject.Name);
            }
        }

        [Model]
        public RootProject RootProject { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ViewModelToModel("RootProject")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the available root projects.
        /// </summary>
        public List<string> AvailableProjects { get; set; }

        /// <summary>
        /// Gets or sets the list of rules.
        /// </summary>
        [ViewModelToModel("RootProject")]
        public ObservableCollection<Rule> Rules { get; set; }

        /// <summary>
        /// Gets or sets the selected rule.
        /// </summary>
        public Rule SelectedRule { get; set; }
        #endregion

        private void OnNameChanged()
        {
            RaisePropertyChanged(() => Title);
        }
    }
}