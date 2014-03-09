// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectProjectItemViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Caitlyn.Models;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;

    /// <summary>
    /// SelectProjectItem view model.
    /// </summary>
    public class SelectProjectItemViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectItemViewModel" /> class.
        /// </summary>
        /// <param name="rootProjectItem">The root Project Item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProjectItem" /> is <c>null</c>.</exception>
        public SelectProjectItemViewModel(IProjectItem rootProjectItem)
        {
            Argument.IsNotNull("rootProjectItem", rootProjectItem);

            RootProjectItems = new List<IProjectItem>();
            foreach (var projectItem in rootProjectItem.ProjectItems)
            {
                RootProjectItems.Add(projectItem);
            }
        }
        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return "Please select a project item";
            }
        }

        /// <summary>
        /// Gets the list of root project items.
        /// </summary>
        public List<IProjectItem> RootProjectItems { get; set; }

        /// <summary>
        /// Gets or sets the selected project item path.
        /// </summary>
        public string SelectedProjectItemPath { get; set; }
        #endregion

        #region Methods
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(SelectedProjectItemPath))
            {
                validationResults.Add(FieldValidationResult.CreateError("SelectedProjectItemPath", "Please select a node to retrieve its name"));
            }
        }
        #endregion
    }
}