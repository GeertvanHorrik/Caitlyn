// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using Caitlyn.Models;
    using Caitlyn.Services;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    /// <summary>
    /// Rule view model.
    /// </summary>
    public class RuleViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;

        #region Variables
        private readonly IProjectItem _rootProjectItem;
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleViewModel" /> class.
        /// </summary>
        /// <param name="rootProject">The root project this rule belongs to.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="visualStudioService">The visual studio service.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        public RuleViewModel(RootProject rootProject, Rule rule, IVisualStudioService visualStudioService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => rootProject);
            Argument.IsNotNull(() => rule);
            Argument.IsNotNull(() => visualStudioService);
            Argument.IsNotNull(() => uiVisualizerService);

            _uiVisualizerService = uiVisualizerService;

            var project = visualStudioService.GetProjectByName(rootProject.Name);
            if (project != null)
            {
                _rootProjectItem = new ProjectItem(project);
            }

            Rule = rule;

            RuleTypes = Enum<RuleType>.ToList();
            ProjectTypes = new ObservableCollection<SelectableProjectType>();

            foreach (var projectType in ProjectTypeHelper.GetAvailableProjectTypes())
            {
                bool isSelected = rule.ProjectTypes.Contains(projectType);
                var selectableProjectType = new SelectableProjectType(projectType, isSelected);
                this.SubscribeToWeakPropertyChangedEvent(selectableProjectType, OnSelectableProjectTypePropertyChanged);
                ProjectTypes.Add(selectableProjectType);
            }

            SelectProjectItem = new Command(OnSelectProjectItemExecute, OnSelectProjectItemCanExecute);
        }
        #endregion

        #region Commands

        #region Properties
        /// <summary>
        /// Gets the SelectProjectItem command.
        /// </summary>
        public Command SelectProjectItem { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to check whether the SelectProjectItem command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
        private bool OnSelectProjectItemCanExecute()
        {
            return (_rootProjectItem != null);
        }

        /// <summary>
        /// Method to invoke when the SelectProjectItem command is executed.
        /// </summary>
        private void OnSelectProjectItemExecute()
        {
            var vm = new SelectProjectItemViewModel(_rootProjectItem);
            if (_uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Name = vm.SelectedProjectItemPath;
            }
        }
        #endregion

        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return string.Format("Ignored item '{0}'", Rule.Name);
            }
        }

        /// <summary>
        /// Gets the ignored item.
        /// </summary>
        [Model]
        public Rule Rule { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ViewModelToModel("Rule")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rule type.
        /// </summary>
        [ViewModelToModel("Rule")]
        public RuleType Type { get; set; }

        /// <summary>
        /// Gets or sets the list of available rule types.
        /// </summary>
        public List<RuleType> RuleTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of selectable projects.
        /// </summary>
        public ObservableCollection<SelectableProjectType> ProjectTypes { get; set; }
        #endregion

        #region Methods
        private void OnSelectableProjectTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Validate(true);
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError("Name", "Name is required"));
            }
        }

        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            if (ProjectTypes != null)
            {
                bool hasAnythingSelected = (from projectType in ProjectTypes where projectType.IsSelected select projectType).Any();
                if (!hasAnythingSelected)
                {
                    validationResults.Add(BusinessRuleValidationResult.CreateError("At least one target project type must be selected"));
                }
            }
        }

        protected override bool Save()
        {
            foreach (var selectableProjectType in ProjectTypes)
            {
                if (selectableProjectType.IsSelected)
                {
                    if (!Rule.ProjectTypes.Contains(selectableProjectType.ProjectType))
                    {
                        Rule.ProjectTypes.Add(selectableProjectType.ProjectType);
                    }
                }
                else
                {
                    Rule.ProjectTypes.Remove(selectableProjectType.ProjectType);
                }
            }

            return base.Save();
        }

        private void OnNameChanged()
        {
            RaisePropertyChanged(() => Title);
        }
        #endregion
    }
}