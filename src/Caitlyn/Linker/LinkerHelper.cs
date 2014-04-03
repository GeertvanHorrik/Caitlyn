// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkerHelper.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Catel;
    using Catel.Collections;
    using EnvDTE;

    public static class LinkerHelper
    {
        #region Methods
        /// <summary>
        /// Gets the related projects of the specified project. For example, when a project
        /// with the name <c>Catel.Core.NET35</c> is used, the following values will be
        /// returned:
        /// <list type="bullet">
        /// <item>
        /// <description>Catel.Core.NET40.</description>
        /// </item>
        /// <item>
        /// <description>Catel.Core.NET45.</description>
        /// </item>
        /// <item>
        /// <description>Catel.Core.SL4.</description>
        /// </item>
        /// <item>
        /// <description>Catel.Core.SL5.</description>
        /// </item>
        /// <item>
        /// <description>Catel.Core.WP7.</description>
        /// </item>
        /// <item>
        /// <description>Catel.Core.WIN80.</description>
        /// </item>
        /// </list>
        /// .
        /// </summary>
        /// <param name="rootProject">The root project.</param>
        /// <param name="smartFallDown">If set to <c>true</c>, only "lower" projects will be returned.</param>
        /// <returns>The list of linked projects.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="rootProject" /> is <c>null</c>.</exception>
        /// <remarks>Note that the <paramref name="rootProject" /> itself will not be included in the list
        /// of linked projects.</remarks>
        public static Project[] GetRelatedProjects(this Project rootProject, bool smartFallDown = true)
        {
            Argument.IsNotNull("rootProject", rootProject);

            if (rootProject.ParentProjectItem == null)
            {
                return new Project[] { };
            }

            var solution = rootProject.DTE.Solution;

            var linkedProjects = new List<Project>();

            string projectName = StripAllPossibleProjectTargets(rootProject.Name);
            var rootProjectType = rootProject.GetProjectType();

            var enumNames = ProjectTypeHelper.GetAvailableProjectTypesAsStrings();
            foreach (var item in enumNames)
            {
                string linkedProjectName = string.Format("{0}.{1}", projectName, item);
                if (string.Equals(linkedProjectName, rootProject.Name))
                {
                    continue;
                }

                var project = solution.GetProjectFromSolution(linkedProjectName);
                if (project != null)
                {
                    if (smartFallDown)
                    {
                        var linkedProjectType = project.GetProjectType();
                        if ((int)linkedProjectType > (int)rootProjectType)
                        {
                            linkedProjects.Add(project);
                        }
                    }
                    else
                    {
                        linkedProjects.Add(project);
                    }
                }
            }

            return linkedProjects.SortProjects().ToArray();
        }

        /// <summary>
        /// Strips all possible project targets from the name. For example when a name of a project
        /// is <c>Catel.Core.NET35</c>, this method will return <c>Catel.Core</c>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The project name without the project target.</returns>
        /// <exception cref="ArgumentException">The <paramref name="name" /> is <c>null</c> or whitespace.</exception>
        public static string StripAllPossibleProjectTargets(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            string projectName = name;

            foreach (var item in Enum.GetNames(typeof(ProjectType)))
            {
                projectName = projectName.Replace(item, string.Empty);
            }

            if (projectName.EndsWith("."))
            {
                projectName = projectName.Substring(0, projectName.Length - 1);
            }

            return projectName;
        }

        /// <summary>
        /// Determines whether the specified project item is an actual file in the specified project. A file is considered
        /// an actual file when it is a physical, thus not linked, file in the project.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="project">The project.</param>
        /// <returns><c>true</c> if the item is an actual file in the specified project; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="item" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="project" /> is <c>null</c>.</exception>
        public static bool IsActualFileInProject(this ProjectItem item, Project project)
        {
            Argument.IsNotNull("item", item);
            Argument.IsNotNull("project", project);

            string relativeItemName = item.GetNameRelativeToRoot();
            return GetProjectItem(project.ProjectItems, relativeItemName, item.FileNames[0]) != null;
        }

        /// <summary>
        /// Determines whether the specified project item is an actual file in one of the specified projects. A file is considered
        /// an actual file when it is a physical, thus not linked, file in the project.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="relatedProjects">The related projects.</param>
        /// <returns><c>true</c> if the item is an actual file in one of the specified projects; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="item" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="relatedProjects" /> is <c>null</c>.</exception>
        public static bool IsActualFileInAnyRelatedProject(this ProjectItem item, Project[] relatedProjects)
        {
            Argument.IsNotNull("item", item);
            Argument.IsNotNull("relatedProjects", relatedProjects);

            // Generated files might be linked
            if (item.Name.Contains(".Designer"))
            {
                return true;
            }

            foreach (var relatedProject in relatedProjects)
            {
                if (IsActualFileInProject(item, relatedProject))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the project item by its name relative to the project root.
        /// </summary>
        /// <param name="projectItems">
        /// The project items.
        /// </param>
        /// <param name="relativeName">
        /// Name of the item to find, relative to the project root.
        /// </param>
        /// <param name="fullItemName">
        /// Full name of the item. Can be <c>null</c>, but if it is entered, it will be checked as well.
        /// </param>
        /// <returns>
        /// The <see cref="ProjectItem"/> or <c>null</c> if not found.
        /// </returns>
        public static ProjectItem GetProjectItem(this ProjectItems projectItems, string relativeName, string fullItemName = null)
        {
            Argument.IsNotNull("projectItems", projectItems);
            Argument.IsNotNullOrWhitespace("relativeName", relativeName);

            foreach (ProjectItem projectItem in projectItems)
            {
                if (string.Compare(relativeName, projectItem.GetNameRelativeToRoot()) == 0)
                {
                    bool rightItem = true;

                    if (!string.IsNullOrWhiteSpace(fullItemName))
                    {
                        if (string.Compare(fullItemName, projectItem.FileNames[0]) != 0)
                        {
                            rightItem = false;
                        }
                    }

                    if (rightItem)
                    {
                        if (projectItem.IsLinkedFile())
                        {
                            return null;
                        }

                        return projectItem;
                    }
                }

                var childItem = projectItem.ProjectItems.GetProjectItem(relativeName, fullItemName);
                if (childItem != null)
                {
                    return childItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the name of the target file by replace the source directory by the target directory.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="targetProject">
        /// The target project.
        /// </param>
        /// <returns>
        /// The target file name based on the source file name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="source"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="targetProject"/> is <c>null</c>.
        /// </exception>
        public static string GetTargetFileName(this ProjectItem source, Project targetProject)
        {
            Argument.IsNotNull("source", source);
            Argument.IsNotNull("targetProject", targetProject);

            string sourceProjectDirectory = source.ContainingProject.GetProjectDirectory();
            string targetProjectDirectory = targetProject.GetProjectDirectory();

            return source.FileNames[0].Replace(sourceProjectDirectory, targetProjectDirectory);
        }
        #endregion
    }
}