namespace Caitlyn.Models
{
    using Catel.Data;

    /// <summary>
    /// Helper class to be able to select projec types in the view.
    /// </summary>
    public class SelectableProjectType : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableProjectType"/> class.
        /// </summary>
        /// <param name="projectType">Type of the project.</param>
        /// <param name="isSelected">if set to <c>true</c>, the project type is selected.</param>
        public SelectableProjectType(ProjectType projectType, bool isSelected = false)
        {
            ProjectType = projectType;
            Name = projectType.ToString();
            IsSelected = isSelected;
        }

        private ProjectType _projectType;

        public ProjectType ProjectType
        {
            get { return _projectType; }
            private set
            {
                _projectType = value;
                RaisePropertyChanged(() => ProjectType);
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}