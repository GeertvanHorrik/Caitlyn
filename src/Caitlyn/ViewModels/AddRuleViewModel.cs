namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Models;
    using Services;

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
        /// Initializes a new instance of the <see cref="AddRuleViewModel"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration"/> is <c>null</c>.</exception>
        public AddRuleViewModel(IConfiguration configuration)
        {
            Argument.IsNotNull("configuration", configuration);

            _configuration = configuration;

            var visualStudioService = GetService<IVisualStudioService>();

            AvailableProjects = new List<string>();
            AvailableProjects.AddRange(from project in visualStudioService.GetAllProjects()
                                       select project.Name);

            RootProject = visualStudioService.GetCurrentProject().Name;

            ItemsToAdd = new List<string>(visualStudioService.GetSelectedItems());

            RuleTypes = Enum<RuleType>.ToList();

            ProjectTypes = new ObservableCollection<SelectableProjectType>();
            foreach (var projectType in Enum<ProjectType>.ToList())
            {
                var selectableProjectType = new SelectableProjectType(projectType);
                this.SubscribeToWeakPropertyChangedEvent(selectableProjectType, OnSelectableProjectTypePropertyChanged);
                ProjectTypes.Add(selectableProjectType);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Add rule"; }
        }

        /// <summary>
        /// Gets or sets the available projects.
        /// </summary>
        public List<string> AvailableProjects
        {
            get { return GetValue<List<string>>(AvailableProjectsProperty); }
            set { SetValue(AvailableProjectsProperty, value); }
        }

        /// <summary>
        /// Register the AvailableProjects property so it is known in the class.
        /// </summary>
        public static readonly PropertyData AvailableProjectsProperty = RegisterProperty("AvailableProjects", typeof(List<string>));

        /// <summary>
        /// Gets or sets the selected root project.
        /// </summary>
        public string RootProject
        {
            get { return GetValue<string>(RootProjectProperty); }
            set { SetValue(RootProjectProperty, value); }
        }

        /// <summary>
        /// Register the RootProject property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RootProjectProperty = RegisterProperty("RootProject", typeof(string));

        /// <summary>
        /// Gets the list of items that will be added.
        /// </summary>
        public List<string> ItemsToAdd
        {
            get { return GetValue<List<string>>(ItemsToAddProperty); }
            set { SetValue(ItemsToAddProperty, value); }
        }

        /// <summary>
        /// Register the ItemsToAdd property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ItemsToAddProperty = RegisterProperty("ItemsToAdd", typeof(List<string>));

        /// <summary>
        /// Gets or sets the list of available rule types.
        /// </summary>
        public List<RuleType> RuleTypes
        {
            get { return GetValue<List<RuleType>>(RuleTypesProperty); }
            set { SetValue(RuleTypesProperty, value); }
        }

        /// <summary>
        /// Register the RuleTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RuleTypesProperty = RegisterProperty("RuleTypes", typeof(List<RuleType>));

        /// <summary>
        /// Gets or sets the selected rule.
        /// </summary>
        public RuleType RuleType
        {
            get { return GetValue<RuleType>(RuleTypeProperty); }
            set { SetValue(RuleTypeProperty, value); }
        }

        /// <summary>
        /// Register the RuleType property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RuleTypeProperty = RegisterProperty("RuleType", typeof(RuleType));

        /// <summary>
        /// Gets or sets the list of selectable projects.
        /// </summary>
        public ObservableCollection<SelectableProjectType> ProjectTypes
        {
            get { return GetValue<ObservableCollection<SelectableProjectType>>(ProjectTypesProperty); }
            set { SetValue(ProjectTypesProperty, value); }
        }

        /// <summary>
        /// Register the ProjectTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectTypesProperty = RegisterProperty("ProjectTypes", typeof(ObservableCollection<SelectableProjectType>));
        #endregion

        #region Commands
        #endregion

        #region Methods
        /// <summary>
        /// Called when a property on a selectable project type has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnSelectableProjectTypePropertyChanged(object sender, EventArgs e)
        {
            Validate(true);
        }

        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(RootProject))
            {
                validationResults.Add(FieldValidationResult.CreateError(RootProjectProperty, "Root project is required"));
            }
        }

        /// <summary>
        /// Validates the business rules of this object. Override this method to enable
        /// validation of business rules.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            if ((ItemsToAdd != null) && (ItemsToAdd.Count == 0))
            {
                validationResults.Add(BusinessRuleValidationResult.CreateError("No items were selected, cannot add a rule for zero items"));
            }

            if (ProjectTypes != null)
            {
                bool hasAnythingSelected = (from projectType in ProjectTypes
                                            where projectType.IsSelected
                                            select projectType).Any();
                if (!hasAnythingSelected)
                {
                    validationResults.Add(BusinessRuleValidationResult.CreateError("At least one target project type must be selected"));
                }
            }
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <returns><c>true</c> if successful; otherwise <c>false</c>.</returns>
        protected override bool Save()
        {
            var rootProject = _configuration.RootProjects.FirstOrDefault(project => string.Compare(RootProject, project.Name) == 0);
            if (rootProject == null)
            {
                rootProject = new RootProject();
                rootProject.Name = RootProject;
                _configuration.RootProjects.Add(rootProject);
            }

            foreach (var itemToAdd in ItemsToAdd)
            {
                bool alreadyContainsRule = (from rule in rootProject.Rules
                                            where string.Compare(rule.Name, itemToAdd) == 0
                                            select rule).Any();
                if (!alreadyContainsRule)
                {
                    var rule = new Rule
                    {
                        Name = itemToAdd,
                        Type = RuleType
                    };

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