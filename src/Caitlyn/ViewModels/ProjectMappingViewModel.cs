// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System.Collections.Generic;

    using Caitlyn.Models;
    using Caitlyn.Services;

    using Catel;
    using Catel.MVVM;

    using EnvDTE;

    /// <summary>
    /// ProjectMapping view model.
    /// </summary>
    public class ProjectMappingViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingViewModel" /> class.
        /// </summary>
        /// <param name="projectMapping">The project Mapping.</param>
        /// <param name="visualStudioService">The visual studio service.</param>
        public ProjectMappingViewModel(ProjectMapping projectMapping, IVisualStudioService visualStudioService)
        {
            Argument.IsNotNull("projectMapping", projectMapping);
            Argument.IsNotNull("visualStudioService", visualStudioService);

            ProjectMapping = projectMapping;

            AvailableProjects = new List<Project>(visualStudioService.GetAllProjects());
        }
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
                return "Project mapping";
            }
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