namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Models;

    /// <summary>
    /// SelectProjectItem view model.
    /// </summary>
    public class SelectProjectItemViewModel : ViewModelBase
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectItemViewModel"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProjectItem"/> is <c>null</c>.</exception>
        public SelectProjectItemViewModel(IProjectItem rootProjectItem)
        {
            Argument.IsNotNull("rootProjectItem", rootProjectItem);

            foreach (var projectItem in rootProjectItem.ProjectItems)
            {
                RootProjectItems.Add(projectItem);
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
            get { return "Please select a project item"; }
        }

        /// <summary>
        /// Gets the list of root project items.
        /// </summary>
        public List<IProjectItem> RootProjectItems
        {
            get { return GetValue<List<IProjectItem>>(RootProjectItemsProperty); }
            set { SetValue(RootProjectItemsProperty, value); }
        }

        /// <summary>
        /// Register the RootProjectItems property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RootProjectItemsProperty = RegisterProperty("RootProjectItems", typeof(List<IProjectItem>), () => new List<IProjectItem>());

        /// <summary>
        /// Gets or sets the selected project item path.
        /// </summary>
        public string SelectedProjectItemPath
        {
            get { return GetValue<string>(SelectedProjectItemPathProperty); }
            set { SetValue(SelectedProjectItemPathProperty, value); }
        }

        /// <summary>
        /// Register the SelectedProjectItemPath property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedProjectItemPathProperty = RegisterProperty("SelectedProjectItemPath", typeof(string));
        #endregion

        #region Methods
        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        /// <remarks></remarks>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(SelectedProjectItemPath))
            {
                validationResults.Add(FieldValidationResult.CreateError(SelectedProjectItemPathProperty, "Please select a node to retrieve its name"));
            }
        }
        #endregion
    }
}