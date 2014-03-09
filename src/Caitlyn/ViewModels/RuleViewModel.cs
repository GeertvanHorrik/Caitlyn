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
        private readonly IVisualStudioService _visualStudioService;
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

            _visualStudioService = visualStudioService;
            _uiVisualizerService = uiVisualizerService;

            var project = visualStudioService.GetProjectByName(rootProject.Name);
            if (project != null)
            {
                _rootProjectItem = new ProjectItem(project);
            }

            Rule = rule;

            RuleTypes = Enum<RuleType>.ToList();
            ProjectTypes = new ObservableCollection<SelectableProjectType>();

            foreach (var projectType in Enum<ProjectType>.ToList())
            {
                bool isSelected = rule.ProjectTypes.Contains(projectType);
                var selectableProjectType = new SelectableProjectType(projectType, isSelected);
                this.SubscribeToWeakPropertyChangedEvent(selectableProjectType, OnSelectableProjectTypePropertyChanged);
                ProjectTypes.Add(selectableProjectType);
            }

            SelectProjectItem = new Command(OnSelectProjectItemExecute, OnSelectProjectItemCanExecute);
        }
        #endregion

        #region Constants
        /// <summary>
        /// Register the Rule property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IgnoredItemProperty = RegisterProperty("Rule", typeof(Rule));

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty, (sender, e) => ((RuleViewModel)sender).RaisePropertyChanged("Title"));

        /// <summary>
        /// Register the Type property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TypeProperty = RegisterProperty("Type", typeof(RuleType));

        /// <summary>
        /// Register the RuleTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RuleTypesProperty = RegisterProperty("RuleTypes", typeof(List<RuleType>));

        /// <summary>
        /// Register the ProjectTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectTypesProperty = RegisterProperty("ProjectTypes", typeof(ObservableCollection<SelectableProjectType>));
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
                return string.Format("Ignored item '{0}'", Rule.Name);
            }
        }

        /// <summary>
        /// Gets the ignored item.
        /// </summary>
        [Model]
        public Rule Rule
        {
            get
            {
                return GetValue<Rule>(IgnoredItemProperty);
            }
            private set
            {
                SetValue(IgnoredItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ViewModelToModel("Rule")]
        public string Name
        {
            get
            {
                return GetValue<string>(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rule type.
        /// </summary>
        [ViewModelToModel("Rule")]
        public RuleType Type
        {
            get
            {
                return GetValue<RuleType>(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of available rule types.
        /// </summary>
        public List<RuleType> RuleTypes
        {
            get
            {
                return GetValue<List<RuleType>>(RuleTypesProperty);
            }
            set
            {
                SetValue(RuleTypesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of selectable projects.
        /// </summary>
        public ObservableCollection<SelectableProjectType> ProjectTypes
        {
            get
            {
                return GetValue<ObservableCollection<SelectableProjectType>>(ProjectTypesProperty);
            }
            set
            {
                SetValue(ProjectTypesProperty, value);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when a property on a selectable project type has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void OnSelectableProjectTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Validate(true);
        }

        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results, add additional results to this list.
        /// </param>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError(NameProperty, "Name is required"));
            }
        }

        /// <summary>
        /// Validates the business rules of this object. Override this method to enable
        /// validation of business rules.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results, add additional results to this list.
        /// </param>
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

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <returns>
        /// <c>true</c> if successful; otherwise <c>false</c>.
        /// </returns>
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
        #endregion
    }
}