// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Linker.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Caitlyn.Models;

    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM.Services;

    using EnvDTE;

    using VSLangProj;

    using ProjectItem = EnvDTE.ProjectItem;

    /// <summary>
    /// The supported project types.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum ProjectType
    {
        /// <summary>
        /// NET35.
        /// </summary>
        NET35, 

        /// <summary>
        /// NET40.
        /// </summary>
        NET40, 

        /// <summary>
        /// NET45.
        /// </summary>
        NET45, 

        /// <summary>
        /// Silverlight 4.
        /// </summary>
        SL4, 

        /// <summary>
        /// Silverlight 5.
        /// </summary>
        SL5, 

        /// <summary>
        /// Windows Phone 7 (Mango).
        /// </summary>
        WP7, 

        /// <summary>
        /// Windows Phone 8.
        /// </summary>
        WP8, 

        /// <summary>
        /// Windows 8.0
        /// </summary>
        WIN80,

        /// <summary>
        /// Windows 8.1
        /// </summary>
        WIN81
    }

    /// <summary>
    /// Project item actions.
    /// </summary>
    public enum ProjectItemAction
    {
        Add, 

        Rename, 

        Remove
    }

    /// <summary>
    /// The linker that does the actual work of linking the projects.
    /// </summary>
    public class Linker
    {
        #region Constants
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Linker" /> class.
        /// </summary>
        /// <param name="rootProject">The root project.</param>
        /// <param name="targetProjects">The projects to link.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetProjects" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="configuration" /> is <c>null</c>.</exception>
        public Linker(Project rootProject, Project[] targetProjects, IConfiguration configuration)
        {
            Argument.IsNotNull("rootProject", rootProject);
            Argument.IsNotNull("targetProjects", targetProjects);
            Argument.IsNotNull("configuration", configuration);

            RootProject = rootProject;
            TargetProjects = targetProjects;
            Configuration = configuration;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether missing files should be removed. If <c>true</c>, a linked file
        /// that is found in a target project will be removed.
        /// </summary>
        /// <value><c>true</c> if missing files should be removed; otherwise, <c>false</c>.</value>
        /// <remarks>When this value is <c>true</c>, it will never delete files that are not linked.</remarks>
        public bool RemoveMissingFiles { get; set; }

        /// <summary>
        /// Gets the root project.
        /// </summary>
        public Project RootProject { get; private set; }

        /// <summary>
        /// Gets the projects to link.
        /// </summary>
        public Project[] TargetProjects { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Links the files for all projects.
        /// </summary>
        /// <exception cref="InvalidOperationException">The root project file does not exist.</exception>
        public void LinkFiles()
        {
            Log.Debug("Linking {0} projects to {1}", TargetProjects.Length, RootProject.Name);

            foreach (var targetProject in TargetProjects)
            {
                Log.Debug("Linking project {0}", targetProject.Name);

                LinkFiles(targetProject);

                Log.Debug("Linked project");
            }

            Log.Debug("Linked {0} projects", TargetProjects.Length);
        }

        /// <summary>
        /// Handles the change of a project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <param name="action">The action that happened to the project item.</param>
        /// <param name="oldName">The old name in case of a rename, can be <c>null</c>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">action</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="projectItem" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The root project file does not exist.</exception>
        /// <exception cref="ArgumentException">The <paramref name="oldName" /> is <c>null</c> or whitespace but the action is <see cref="ProjectItemAction.Rename" />.</exception>
        public void HandleProjectItemChange(ProjectItem projectItem, ProjectItemAction action, string oldName = null)
        {
            Argument.IsNotNull("projectItem", projectItem);
            if (action == ProjectItemAction.Rename)
            {
                Argument.IsNotNullOrWhitespace("oldName", oldName);
            }

            Log.Debug("Linking {0} projects to {1}", TargetProjects.Length, RootProject.Name);

            var sourceProjectItems = projectItem.Collection;
            var fileFilter = new List<string>(new[] { projectItem.GetNameRelativeToRoot() });
            if (!string.IsNullOrWhiteSpace(oldName))
            {
                // Handle delete of old item
                var relativeName = projectItem.GetNameRelativeToRoot();
                var relativeNameWithoutFile = relativeName.Substring(0, relativeName.Length - projectItem.Name.Length);

                var oldRelativeName = Path.Combine(relativeNameWithoutFile, oldName);
                fileFilter.Add(oldRelativeName);
            }

            foreach (var targetProject in TargetProjects)
            {
                Log.Debug("Linking project {0}", targetProject.Name);

                var targetProjectItems = GetTargetProjectItems(sourceProjectItems, targetProject);

                switch (action)
                {
                    case ProjectItemAction.Add:
                        AddFilesAndFolders(sourceProjectItems, targetProjectItems, targetProject.GetProjectType(), 1, fileFilter);
                        break;

                    case ProjectItemAction.Rename:
                        RemoveFilesAndFolders(sourceProjectItems, targetProjectItems, targetProject.GetProjectType(), 1, fileFilter);
                        AddFilesAndFolders(sourceProjectItems, targetProjectItems, targetProject.GetProjectType(), 1, fileFilter);
                        break;

                    case ProjectItemAction.Remove:
                        RemoveFilesAndFolders(sourceProjectItems, targetProjectItems, targetProject.GetProjectType(), 1, fileFilter);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("action");
                }

                Log.Debug("Linked project");
            }

            Log.Debug("Linked {0} projects", TargetProjects.Length);
        }

        /// <summary>
        /// Links the files for the specified project.
        /// </summary>
        /// <param name="targetProject">The project to link.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="targetProject" /> is <c>null</c>.</exception>
        private void LinkFiles(Project targetProject)
        {
            Argument.IsNotNull("targetProject", targetProject);

            SynchronizeProjectItem(RootProject.ProjectItems, targetProject.ProjectItems, targetProject.GetProjectType(), true, null);
        }

        /// <summary>
        /// Gets the target project items.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetProject">The target project.</param>
        /// <returns>ProjectItems.</returns>
        /// <exception cref="System.NotSupportedException">Only folders are supported</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetProject" /> is <c>null</c>.</exception>
        private ProjectItems GetTargetProjectItems(ProjectItems source, Project targetProject)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("targetProject", targetProject);

            var project = source.Parent as Project;
            if (project != null)
            {
                return targetProject.ProjectItems;
            }

            var projectItem = (ProjectItem)source.Parent;
            if (!projectItem.IsFolder())
            {
                throw new NotSupportedException("Only folders are supported");
            }

            var sourceRelativeName = projectItem.GetNameRelativeToRoot();
            var sourceFullName = projectItem.ContainingProject.GetProjectDirectory();
            var targetFullName = targetProject.GetProjectDirectory();

            var splittedFolder = sourceRelativeName.Split(new[] { '\\' });
            var targetProjectItems = targetProject.ProjectItems;
            for (int i = 0; i < splittedFolder.Length; i++)
            {
                var sourceItemName = splittedFolder[i];
                sourceFullName = Path.Combine(sourceFullName, sourceItemName);
                targetFullName = Path.Combine(targetFullName, sourceItemName);

                var folder = (from targetProjectItem in targetProjectItems.Cast<ProjectItem>() where string.Equals(targetProjectItem.Name, sourceItemName, StringComparison.InvariantCultureIgnoreCase) && targetProjectItem.IsFolder() select targetProjectItem).FirstOrDefault();

                if (folder != null)
                {
                    targetProjectItems = folder.ProjectItems;
                }
                else
                {
                    var createdProjectFolder = Directory.Exists(targetFullName) ? targetProjectItems.AddFromDirectory(targetFullName) : targetProjectItems.AddFolder(sourceFullName);
                    targetProjectItems = createdProjectFolder.ProjectItems;
                }
            }

            return targetProjectItems;
        }

        /// <summary>
        /// Synchronizes the project items collection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <param name="recursive">If <c>true</c>, this method will synchronize recursive; otherwise only the top-level will be synchronized.</param>
        /// <param name="fileFilter">An enumerable of files that should be handled with care, can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="target" /> is <c>null</c>.</exception>
        private void SynchronizeProjectItem(ProjectItems source, ProjectItems target, ProjectType targetProjectType, bool recursive, IEnumerable<string> fileFilter)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("target", target);

            string sourceParentName = source.Parent.GetObjectName();
            string targetParentName = target.Parent.GetObjectName();

            Log.Debug("Synchronizing source '{0}' to target '{1}'", sourceParentName, targetParentName);

            AddFilesAndFolders(source, target, targetProjectType, recursive ? -1 : 1, fileFilter);

            if (RemoveMissingFiles)
            {
                RemoveFilesAndFolders(source, target, targetProjectType, recursive ? -1 : 1, fileFilter);
            }

            Log.Debug("Synchronized source '{0}' to target '{1}'", sourceParentName, targetParentName);
        }

        /// <summary>
        /// Adds the files and folders.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <param name="levels">The number of levels to walk down the chain. If <c>-1</c>, it will handle all levels.</param>
        /// <param name="fileFilter">An enumerable of files that should be handled with care, can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="target" /> is <c>null</c>.</exception>
        private void AddFilesAndFolders(ProjectItems source, ProjectItems target, ProjectType targetProjectType, int levels, IEnumerable<string> fileFilter)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("target", target);

            string targetParentName = target.Parent.GetObjectName();

            Log.Debug("Adding files and folders to target '{0}'", targetParentName);

            if (levels == 0)
            {
                return;
            }

            levels--;

            foreach (ProjectItem sourceItem in source)
            {
                if (sourceItem.IsLinkedFile())
                {
                    Log.Debug("Skipping item '{0}' because it is a linked file (so has another root project)", sourceItem.GetObjectName());

                    continue;
                }

                if (ShouldSkipAddingOfItem(sourceItem, targetProjectType))
                {
                    Log.Debug("Skipping item '{0}' because it is ignored by a rule for target project {1}", sourceItem.GetObjectName(), targetProjectType);

                    continue;
                }

                ProjectItem existingTargetItem = null;
                foreach (ProjectItem targetItem in target)
                {
                    if (string.Equals(targetItem.Name, sourceItem.Name))
                    {
                        existingTargetItem = targetItem;
                        break;
                    }
                }

                bool isFolder = sourceItem.IsFolder();
                if (existingTargetItem == null)
                {
                    if (!isFolder)
                    {
                        if (sourceItem.IsXamlFile() && ((targetProjectType == ProjectType.NET40) || (targetProjectType == ProjectType.NET45)))
                        {
                            Log.Debug("File '{0}' is a xaml file and the target project is NET40 or NET45. There is a bug in Visual Studio that does not allow to link xaml files in NET40, so a copy is created.", sourceItem.FileNames[0]);

                            // string targetFile = sourceItem.GetTargetFileName(target);
                            // File.Copy(sourceItem.FileNames[0], targetFile);
                            existingTargetItem = target.AddFromFileCopy(sourceItem.FileNames[0]);
                        }
                        else
                        {
                            Log.Debug("Adding link to file '{0}'", sourceItem.FileNames[0]);

                            try
                            {
                                // Linked file
                                existingTargetItem = target.AddFromFile(sourceItem.FileNames[0]);
                            }
                            catch (Exception ex)
                            {
                                var messageService = ServiceLocator.Default.ResolveType<IMessageService>();
                                messageService.ShowError(ex);
                            }
                        }
                    }
                    else
                    {
                        Log.Debug("Adding folder '{0}'", sourceItem.FileNames[0]);

                        string targetDirectory = sourceItem.GetTargetFileName(target.ContainingProject);
                        existingTargetItem = Directory.Exists(targetDirectory) ? target.AddFromDirectory(targetDirectory) : target.AddFolder(sourceItem.Name);
                    }

                    if (existingTargetItem != null)
                    {
                        Log.Debug("Added item '{0}'", existingTargetItem.Name);
                    }
                }

                if (sourceItem.IsResourceFile())
                {
                    SynchronizeResourceFileProperties(sourceItem, existingTargetItem);
                }

                if (isFolder)
                {
                    AddFilesAndFolders(sourceItem.ProjectItems, existingTargetItem.ProjectItems, targetProjectType, levels, fileFilter);
                }
            }

            Log.Debug("Added files and folders to target '{0}'", targetParentName);
        }

        /// <summary>
        /// Removes the files and folders.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <param name="levels">The number of levels to walk down the chain. If <c>-1</c>, it will handle all levels.</param>
        /// <param name="fileFilter">An enumerable of files that should be handled with care, can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="target" /> is <c>null</c>.</exception>
        private void RemoveFilesAndFolders(ProjectItems source, ProjectItems target, ProjectType targetProjectType, int levels, IEnumerable<string> fileFilter)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("target", target);

            if (fileFilter == null)
            {
                fileFilter = new string[] { };
            }

            string targetParentName = target.Parent.GetObjectName();

            Log.Debug("Removing files and folders from target '{0}'", targetParentName);

            if (levels == 0)
            {
                return;
            }

            levels--;

            // Yep, this is right, we start at 1... :(
            for (int i = 1; i < target.Count + 1; i++)
            {
                var targetItem = target.Item(i);

                if (ShouldSkipRemovingOfItem(source.ContainingProject, targetItem, targetProjectType))
                {
                    Log.Debug("Skipping item '{0}' because it is ignored by a rule for target project {1}", targetItem.GetObjectName(), targetProjectType);
                    continue;
                }

                var existingSourceItem = (from sourceItem in source.Cast<ProjectItem>() where string.Equals(targetItem.Name, sourceItem.Name, StringComparison.InvariantCultureIgnoreCase) select sourceItem).FirstOrDefault();

                if (existingSourceItem != null && !fileFilter.Any(x => string.Equals(x, targetItem.GetNameRelativeToRoot(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    // Check if the item should be removed (when the item should not be added as linked file, then we should remove it)
                    if (!ShouldSkipAddingOfItem(existingSourceItem, targetProjectType))
                    {
                        RemoveFilesAndFolders(existingSourceItem.ProjectItems, targetItem.ProjectItems, targetProjectType, levels, fileFilter);
                        continue;
                    }

                    Log.Debug("Found linked file '{0}' that is now ignored by a rule, removing it", targetItem.FileNames[0]);
                }

                // Get it once, we don't want to retrieve this several times
                var relatedProjects = source.ContainingProject.GetRelatedProjects(false);

                if (targetItem.IsFolder())
                {
                    RemoveNestedItems(targetItem.ProjectItems, relatedProjects);

                    if (targetItem.ProjectItems.Count == 0)
                    {
                        Log.Debug("Removing folder '{0}' because it no longer contains items", targetItem.Name);

                        targetItem.Remove();
                        i--;
                    }
                }
                else
                {
                    // If this is a linked file and not an actual file in another related project, remove it
                    if (targetItem.IsLinkedFile() && !targetItem.IsActualFileInAnyRelatedProject(relatedProjects))
                    {
                        Log.Debug("Removing file '{0}' because it is a linked file to the root project", targetItem.FileNames[0]);

                        targetItem.Remove();
                        i--;
                    }
                }
            }

            Log.Debug("Removed files and folders from target '{0}'", targetParentName);
        }

        /// <summary>
        /// Removes the nested items of an item of which the soure does not exists.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="relatedProjects">The related projects.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="projectItems" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="relatedProjects" /> is <c>null</c>.</exception>
        private void RemoveNestedItems(ProjectItems projectItems, Project[] relatedProjects)
        {
            Argument.IsNotNull("projectItems", projectItems);
            Argument.IsNotNull("relatedProjects", relatedProjects);

            for (int i = 1; i < projectItems.Count + 1; i++)
            {
                ProjectItem folderItem = projectItems.Item(i);
                if (folderItem.IsFolder())
                {
                    RemoveNestedItems(folderItem.ProjectItems, relatedProjects);

                    if (folderItem.ProjectItems.Count == 0)
                    {
                        Log.Debug("Removing folder '{0}' because it no longer contains items", folderItem.Name);

                        folderItem.Remove();
                        i--;
                    }
                }
                else
                {
                    // If this is a linked file and not an actual file in another related project, remove it
                    if (folderItem.IsLinkedFile() && !folderItem.IsActualFileInAnyRelatedProject(relatedProjects))
                    {
                        folderItem.Remove();
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Synchronizes the resource file properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="target" /> is <c>null</c>.</exception>
        private void SynchronizeResourceFileProperties(ProjectItem source, ProjectItem target)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("target", target);

            Log.Debug("Synchronizing resource file properties for '{0}'", source.Name);

            target.Properties.Item("CustomTool").Value = source.Properties.Item("CustomTool").Value;
            target.Properties.Item("CustomToolNamespace").Value = source.Properties.Item("CustomToolNamespace").Value;

            Log.Debug("Running custom tool to generate 'code-behind' file");

            // We need to run the custom tool first
            var vsProjectItem = (VSProjectItem)target.Object;
            vsProjectItem.RunCustomTool();

            Log.Debug("Ran custom tool to generate 'code-behind' file");

            // string customOutput = (string) source.Properties.Item("CustomToolOutput").Value;
            // customOutput = customOutput.Replace(".cs", string.Format(".{0}.cs", targetProjectType));
            // target.Properties.Item("CustomToolOutput").Value = customOutput;
            Log.Debug("Synchronized resource file properties", source.Name);
        }

        /// <summary>
        /// Determines whether the specified item should be skipped based on the rules in the configuration.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <returns><c>true</c> if the item should be skipped; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source" /> is <c>null</c>.</exception>
        private bool ShouldSkipAddingOfItem(ProjectItem source, ProjectType targetProjectType)
        {
            Argument.IsNotNull("source", source);

            var rootProjectConfiguration = GetRootProjectConfiguration(source.ContainingProject);

            return MatchesRule(source, rootProjectConfiguration, RuleType.DoNotAdd, targetProjectType);
        }

        /// <summary>
        /// Determines whether the specified item should be skipped based on the rules in the configuration.
        /// </summary>
        /// <param name="sourceProject">The source project.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <returns><c>true</c> if the item should be skipped; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceProject" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="target" /> is <c>null</c>.</exception>
        private bool ShouldSkipRemovingOfItem(Project sourceProject, ProjectItem target, ProjectType targetProjectType)
        {
            Argument.IsNotNull("sourceProject", sourceProject);
            Argument.IsNotNull("target", target);

            var rootProjectConfiguration = GetRootProjectConfiguration(sourceProject);

            return MatchesRule(target, rootProjectConfiguration, RuleType.DoNotRemove, targetProjectType);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="item" /> matches the specified rule.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="rootProjectConfiguration">The root project configuration.</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="targetProjectType">Type of the target project.</param>
        /// <returns><c>true</c> if the item matches the rule; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="item" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProjectConfiguration" /> is <c>null</c>.</exception>
        private bool MatchesRule(ProjectItem item, RootProject rootProjectConfiguration, RuleType ruleType, ProjectType targetProjectType)
        {
            Argument.IsNotNull("item", item);
            Argument.IsNotNull("rootProjectConfiguration", rootProjectConfiguration);

            foreach (var rule in rootProjectConfiguration.Rules)
            {
                if (rule.Type == ruleType)
                {
                    if (string.Compare(rule.Name, item.GetNameRelativeToRoot()) == 0)
                    {
                        foreach (var ruledProjectType in rule.ProjectTypes)
                        {
                            if (ruledProjectType == targetProjectType)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the root project configuration.
        /// </summary>
        /// <param name="rootProject">The root project.</param>
        /// <returns>The <see cref="RootProject" /> configuration. If the configuration does not exist, and empty one is returned.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        private RootProject GetRootProjectConfiguration(Project rootProject)
        {
            Argument.IsNotNull("rootProject", rootProject);

            foreach (var rootProjectConfig in Configuration.RootProjects)
            {
                if (string.Compare(rootProjectConfig.Name, rootProject.Name) == 0)
                {
                    return rootProjectConfig;
                }
            }

            return new RootProject();
        }
        #endregion
    }
}