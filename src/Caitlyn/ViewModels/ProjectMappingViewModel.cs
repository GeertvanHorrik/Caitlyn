// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.ViewModels
{
    using System.Collections.Generic;
    using Catel;
    using Catel.MVVM;
    using EnvDTE;
    using Models;
    using Services;

    /// <summary>
    /// ProjectMapping view model.
    /// </summary>
    public class ProjectMappingViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingViewModel"/> class.
        /// </summary>
        public ProjectMappingViewModel(ProjectMapping projectMapping)
        {
            Argument.IsNotNull("projectMapping", projectMapping);

            ProjectMapping = projectMapping;

            AvailableProjects = new List<Project>(GetService<IVisualStudioService>().GetAllProjects());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Project mapping"; }
        }

        [Model]
        public ProjectMapping ProjectMapping { get; private set; }

        [ViewModelToModel("ProjectMapping")]
        public string SourceProject { get; set; }

        [ViewModelToModel("ProjectMapping")]
        public string TargetProject { get; set; }

        public List<Project> AvailableProjects { get; set; }
        #endregion
    }
}