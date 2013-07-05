// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableProjectType.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using Catel.Data;

    /// <summary>
    /// Helper class to be able to select projec types in the view.
    /// </summary>
    public class SelectableProjectType : ObservableObject
    {
        #region Fields
        private bool _isSelected;

        private string _name;

        private ProjectType _projectType;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableProjectType"/> class.
        /// </summary>
        /// <param name="projectType">
        /// Type of the project.
        /// </param>
        /// <param name="isSelected">
        /// If set to <c>true</c>, the project type is selected.
        /// </param>
        public SelectableProjectType(ProjectType projectType, bool isSelected = false)
        {
            ProjectType = projectType;
            Name = projectType.ToString();
            IsSelected = isSelected;
        }
        #endregion

        #region Properties
        public ProjectType ProjectType
        {
            get
            {
                return _projectType;
            }
            private set
            {
                _projectType = value;
                RaisePropertyChanged(() => ProjectType);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
        #endregion
    }
}