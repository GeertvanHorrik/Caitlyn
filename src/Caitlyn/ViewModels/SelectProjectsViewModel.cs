// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectProjectsViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;

    using EnvDTE;

    using EnvDTE80;

    /// <summary>
    /// SelectProjects view model.
    /// </summary>
    public class SelectProjectsViewModel : ViewModelBase
    {
        #region Variables
        private readonly DTE2 _visualStudio;
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectsViewModel"/> class.
        /// </summary>
        /// <param name="visualStudio">
        /// The visual studio.
        /// </param>
        /// <param name="existingProjects">
        /// The existing projects.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="visualStudio"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="existingProjects"/> is <c>null</c>.
        /// </exception>
        public SelectProjectsViewModel(DTE2 visualStudio, IEnumerable<Project> existingProjects)
        {
            Argument.IsNotNull("visualStudio", visualStudio);
            Argument.IsNotNull("existingProjects", existingProjects);

            _visualStudio = visualStudio;
            AvailableProjects = new List<Project>(existingProjects);
            SelectableProjects = new ObservableCollection<ProjectSelection>();

            RootProject = _visualStudio.GetActiveProject();
        }
        #endregion

        #region Constants
        /// <summary>
        /// Register the AvailableProjects property so it is known in the class.
        /// </summary>
        public static readonly PropertyData AvailableProjectsProperty = RegisterProperty("AvailableProjects", typeof(List<Project>));

        /// <summary>
        /// Register the RootProject property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RootProjectProperty = RegisterProperty("RootProject", typeof(Project), null, (sender, e) => ((SelectProjectsViewModel)sender).OnRootProjectChanged());

        /// <summary>
        /// Register the SelectableProjects property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectableProjectsProperty = RegisterProperty("SelectableProjects", typeof(ObservableCollection<ProjectSelection>));

        /// <summary>
        /// Register the RemoveMissingFiles property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RemoveMissingFilesProperty = RegisterProperty("RemoveMissingFiles", typeof(bool), true);
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
                return "Select the projects you want to link";
            }
        }

        /// <summary>
        /// Gets or sets a list of available projects.
        /// </summary>
        public List<Project> AvailableProjects
        {
            get
            {
                return GetValue<List<Project>>(AvailableProjectsProperty);
            }
            set
            {
                SetValue(AvailableProjectsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected root project.
        /// </summary>
        public Project RootProject
        {
            get
            {
                return GetValue<Project>(RootProjectProperty);
            }
            set
            {
                SetValue(RootProjectProperty, value);
            }
        }

        /// <summary>
        /// Gets the list of items that are selectable.
        /// </summary>
        public ObservableCollection<ProjectSelection> SelectableProjects
        {
            get
            {
                return GetValue<ObservableCollection<ProjectSelection>>(SelectableProjectsProperty);
            }
            set
            {
                SetValue(SelectableProjectsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets whether missing files should be removed.
        /// </summary>
        public bool RemoveMissingFiles
        {
            get
            {
                return GetValue<bool>(RemoveMissingFilesProperty);
            }
            set
            {
                SetValue(RemoveMissingFilesProperty, value);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the <see cref="RootProject"/> property changes.
        /// </summary>
        private void OnRootProjectChanged()
        {
            SelectableProjects.ReplaceRange(from project in AvailableProjects select new ProjectSelection(project));

            if (RootProject != null)
            {
                var linkedProjects = RootProject.GetRelatedProjects();
                foreach (var linkedProject in linkedProjects)
                {
                    var rightProject = (from selectableProject in SelectableProjects where string.Compare(selectableProject.Name, linkedProject.Name) == 0 select selectableProject).First();
                    rightProject.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results, add additional results to this list.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (RootProject == null)
            {
                validationResults.Add(FieldValidationResult.CreateError(RootProjectProperty, "Root project is required"));
            }
        }

        /// <summary>
        /// Validates the business rules of this object. Override this method to enable
        /// validation of business rules.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results, add additional results to this list.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            // TODO: check if at least a few projects are selected
        }
        #endregion
    }

    /// <summary>
    /// Display class to be able to select projects.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ProjectSelection : ObservableObject
    {
        #region Fields
        private bool _isChecked;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSelection"/> class.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="project"/> is <c>null</c>.
        /// </exception>
        public ProjectSelection(Project project)
        {
            Argument.IsNotNull("project", project);

            Project = project;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Project Project { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// </remarks>
        public string Name
        {
            get
            {
                return Project.Name;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// </remarks>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        #endregion
    }
}