// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddRuleViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Caitlyn.Models;
    using Caitlyn.Services;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;

    /// <summary>
    /// AddRule view model.
    /// </summary>
    public class AddRuleViewModel : ViewModelBase
    {
        #region Variables
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRuleViewModel" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="visualStudioService">The visual studio service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="visualStudioService" /> is <c>null</c>.</exception>
        public AddRuleViewModel(IConfiguration configuration, IVisualStudioService visualStudioService)
        {
            Argument.IsNotNull(() => configuration);
            Argument.IsNotNull(() => visualStudioService);

            _configuration = configuration;

            AvailableProjects = new List<string>();
            AvailableProjects.AddRange(from project in visualStudioService.GetAllProjects() 
                                       select project.Name);

            RootProject = visualStudioService.GetCurrentProject().Name;

            ItemsToAdd = new List<string>(visualStudioService.GetSelectedItems());

            RuleTypes = Enum<RuleType>.ToList();

            ProjectTypes = new ObservableCollection<SelectableProjectType>();
            foreach (var projectType in ProjectTypeHelper.GetAvailableProjectTypes())
            {
                var selectableProjectType = new SelectableProjectType(projectType);
                this.SubscribeToWeakPropertyChangedEvent(selectableProjectType, OnSelectableProjectTypePropertyChanged);
                ProjectTypes.Add(selectableProjectType);
            }
        }
        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return "Add rule";
            }
        }

        /// <summary>
        /// Gets or sets the available projects.
        /// </summary>
        public List<string> AvailableProjects { get; set; }

        /// <summary>
        /// Gets or sets the selected root project.
        /// </summary>
        public string RootProject { get; set; }

        /// <summary>
        /// Gets the list of items that will be added.
        /// </summary>
        public List<string> ItemsToAdd { get; set; }

        /// <summary>
        /// Gets or sets the list of available rule types.
        /// </summary>
        public List<RuleType> RuleTypes { get; set; }

        /// <summary>
        /// Gets or sets the selected rule.
        /// </summary>
        public RuleType RuleType { get; set; }

        /// <summary>
        /// Gets or sets the list of selectable projects.
        /// </summary>
        public ObservableCollection<SelectableProjectType> ProjectTypes { get; set; }
        #endregion

        #region Methods
        private void OnSelectableProjectTypePropertyChanged(object sender, EventArgs e)
        {
            Validate(true);
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(RootProject))
            {
                validationResults.Add(FieldValidationResult.CreateError("RootProject", "Root project is required"));
            }
        }

        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            if ((ItemsToAdd != null) && (ItemsToAdd.Count == 0))
            {
                validationResults.Add(BusinessRuleValidationResult.CreateError("No items were selected, cannot add a rule for zero items"));
            }

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
            var rootProject = _configuration.RootProjects.FirstOrDefault(project => string.Equals(RootProject, project.Name));
            if (rootProject == null)
            {
                rootProject = new RootProject();
                rootProject.Name = RootProject;
                _configuration.RootProjects.Add(rootProject);
            }

            foreach (var itemToAdd in ItemsToAdd)
            {
                bool alreadyContainsRule = (from rule in rootProject.Rules 
                                            where string.Equals(rule.Name, itemToAdd) 
                                            select rule).Any();
                if (!alreadyContainsRule)
                {
                    var rule = new Rule { Name = itemToAdd, Type = RuleType };

                    foreach (var selectableProjectType in ProjectTypes)
                    {
                        if (selectableProjectType.IsSelected)
                        {
                            rule.ProjectTypes.Add(selectableProjectType.ProjectType);
                        }
                    }

                    rootProject.Rules.Add(rule);
                }
            }

            return true;
        }
        #endregion
    }
}