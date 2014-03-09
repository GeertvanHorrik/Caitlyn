// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoLinkerService.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Services
{
    using System;
    using System.Linq;

    using Catel;

    using EnvDTE;

    using EnvDTE80;

    public class AutoLinkerService : Catel.Services.ViewModelServiceBase, IAutoLinkerService
    {
        #region Fields
        private readonly ProjectItemsEvents _projectItemsEvents;

        private readonly SolutionEvents _solutionEvents;

        private readonly ProjectItemsEvents _solutionItemsEvents;

        private readonly DTE2 _visualStudio;
        private readonly IConfigurationService _configurationService;
        private readonly IVisualStudioService _visualStudioService;

        private bool _isSolutionLoaded;
        #endregion

        #region Constructors
        public AutoLinkerService(DTE2 visualStudio, IConfigurationService configurationService, IVisualStudioService visualStudioService)
        {
            Argument.IsNotNull("visualStudio", visualStudio);
            Argument.IsNotNull("configurationService", configurationService);
            Argument.IsNotNull("visualStudioService", visualStudioService);

            _visualStudio = visualStudio;
            _configurationService = configurationService;
            _visualStudioService = visualStudioService;

            var events = (Events2)_visualStudio.Events;

            _solutionEvents = events.SolutionEvents;
            _solutionEvents.Opened += OnSolutionOpened;
            _solutionEvents.AfterClosing += OnSolutionClosed;

            _solutionItemsEvents = events.MiscFilesEvents;
            _solutionItemsEvents.ItemAdded += OnSolutionItemAdded;
            _solutionItemsEvents.ItemRemoved += OnSolutionItemRemoved;
            _solutionItemsEvents.ItemRenamed += OnSolutionItemRenamed;

            _projectItemsEvents = events.ProjectItemsEvents;
            _projectItemsEvents.ItemAdded += OnSolutionItemAdded;
            _projectItemsEvents.ItemRemoved += OnSolutionItemRemoved;
            _projectItemsEvents.ItemRenamed += OnSolutionItemRenamed;
        }
        #endregion

        #region Methods
        private void OnSolutionOpened()
        {
            _isSolutionLoaded = true;
        }

        private void OnSolutionClosed()
        {
            _isSolutionLoaded = false;
        }

        private void OnSolutionItemAdded(ProjectItem projectItem)
        {
            HandleProjectItemChange(projectItem, ProjectItemAction.Add);
        }

        private void OnSolutionItemRemoved(ProjectItem projectItem)
        {
            HandleProjectItemChange(projectItem, ProjectItemAction.Remove);
        }

        private void OnSolutionItemRenamed(ProjectItem projectItem, string oldName)
        {
            HandleProjectItemChange(projectItem, ProjectItemAction.Rename, oldName);
        }

        private void HandleProjectItemChange(ProjectItem projectItem, ProjectItemAction action, string oldName = null)
        {
            Argument.IsNotNull("projectItem", projectItem);

            if (!_isSolutionLoaded)
            {
                return;
            }

            var configuration = _configurationService.LoadConfigurationForCurrentSolution();
            if (configuration.EnableAutoLink)
            {
                var sourceProject = projectItem.ContainingProject;
                var sourceProjectName = sourceProject.Name;
                var projectsToLink = (from projectMapping in configuration.ProjectMappings 
                                      where string.Equals(projectMapping.SourceProject, sourceProjectName, StringComparison.OrdinalIgnoreCase) 
                                      select _visualStudioService.GetProjectByName(projectMapping.TargetProject)).ToArray();

                var linker = new Linker(sourceProject, projectsToLink, configuration);
                linker.RemoveMissingFiles = true;
                linker.HandleProjectItemChange(projectItem, action, oldName);
            }
        }
        #endregion
    }
}