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
    using System.ComponentModel;
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
        private readonly List<ProjectSelection> _selectableProjects; 
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectsViewModel" /> class.
        /// </summary>
        /// <param name="visualStudio">The visual studio.</param>
        /// <param name="existingProjects">The existing projects.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="visualStudio" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="visualStudio" /> is <c>null</c>.</exception>
        public SelectProjectsViewModel(DTE2 visualStudio, IEnumerable<Project> existingProjects)
        {
            Argument.IsNotNull("visualStudio", visualStudio);
            Argument.IsNotNull("existingProjects", existingProjects);

            _visualStudio = visualStudio;
            AvailableProjects = new List<Project>(existingProjects);
            _selectableProjects = AvailableProjects.Select(x => new ProjectSelection(x)).ToList();
            SelectableProjects = new ObservableCollection<ProjectSelection>();

            RootProject = _visualStudio.GetActiveProject();
        }
        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return "Select the projects you want to link";
            }
        }

        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets a list of available projects.
        /// </summary>
        public List<Project> AvailableProjects { get; set; }

        /// <summary>
        /// Gets or sets the selected root project.
        /// </summary>
        public Project RootProject { get; set; }

        /// <summary>
        /// Gets the list of items that are selectable.
        /// </summary>
        public ObservableCollection<ProjectSelection> SelectableProjects { get; set; }

        /// <summary>
        /// Gets or sets whether missing files should be removed.
        /// </summary>
        [DefaultValue(true)]
        public bool RemoveMissingFiles { get; set; }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            UpdateFilter();
        }

        private void OnFilterChanged()
        {
            UpdateFilter();
        }

        private void OnRootProjectChanged()
        {
            if (RootProject != null)
            {
                foreach (var project in _selectableProjects)
                {
                    project.IsChecked = false;
                }

                var linkedProjects = RootProject.GetRelatedProjects();
                foreach (var linkedProject in linkedProjects)
                {
                    var rightProject = (from selectableProject in _selectableProjects
                                        where string.Compare(selectableProject.Name, linkedProject.Name) == 0
                                        select selectableProject).First();
                    rightProject.IsChecked = true;
                }
            }
        }

        private void UpdateFilter()
        {
            var selectableProjects = _selectableProjects.Select(x => x);

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                string lowerCaseFilter = Filter.ToLower();
                selectableProjects = selectableProjects.Where(x => x.Name.ToLower().Contains(lowerCaseFilter) || x.IsChecked);
            }

            SelectableProjects.ReplaceRange(selectableProjects.ToList());
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (RootProject == null)
            {
                validationResults.Add(FieldValidationResult.CreateError("RootProject", "Root project is required"));
            }
        }
        #endregion
    }

    /// <summary>
    /// Display class to be able to select projects.
    /// </summary>
    public class ProjectSelection : ObservableObject
    {
        #region Fields
        private bool _isChecked;
        #endregion

        #region Constructors
        public ProjectSelection(Project project)
        {
            Argument.IsNotNull("project", project);

            Project = project;
        }
        #endregion

        #region Properties
        public Project Project { get; private set; }

        public string Name
        {
            get
            {
                return Project.Name;
            }
        }

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