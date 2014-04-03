namespace Caitlyn
{
    using System;
    using System.Linq;
    using Catel;
    using EnvDTE;
    using System.Collections.Generic;

    public static class ProjectHelper
    {
        /// <summary>
        /// Gets the type of the project based on the naming conventions. Might be determined by
        /// another way in the future.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The <see cref="ProjectType"/> or <see cref="ProjectType.NET35"/> if the project type could not be determined.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="project"/> is <c>null</c>.</exception>
        public static ProjectType GetProjectType(this Project project)
        {
            Argument.IsNotNull("project", project);

            foreach (var projectType in ProjectTypeHelper.GetAvailableProjectTypesAsStrings())
            {
                if (project.Name.Contains(projectType))
                {
                    return (ProjectType)Enum.Parse(typeof(ProjectType), projectType.Replace(".", "_"));
                }
            }

            return ProjectType.NET35;
        }

        /// <summary>
        /// Gets the name of the project without the project type.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The project name without the project type.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="project"/> is <c>null</c>.</exception>
        public static string GetProjectNameWithoutProjectType(this Project project)
        {
            Argument.IsNotNull("project", project);

            var name = project.Name;

            foreach (var projectType in ProjectTypeHelper.GetAvailableProjectTypesAsStrings())
            {
                name = name.Replace(projectType, string.Empty);
            }

            return name;
        }

        /// <summary>
        /// Sorts the projects by name and then by project type.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns>The sorted projects.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="projects"/> is <c>null</c>.</exception>
        public static IEnumerable<Project> SortProjects(this IEnumerable<Project> projects)
        {
            Argument.IsNotNull("projects", projects);

            // Create groups
            var projectGroups = new Dictionary<string, List<Project>>();
            foreach (var project in projects)
            {
                var projectNameWithoutProjectType = project.GetProjectNameWithoutProjectType();
                if (!projectGroups.ContainsKey(projectNameWithoutProjectType))
                {
                    projectGroups[projectNameWithoutProjectType] = new List<Project>();
                }

                projectGroups[projectNameWithoutProjectType].Add(project);
            }

            // Sort each group
            foreach (var projectGroup in projectGroups)
            {
                var projectGroupProjects = projectGroup.Value;
                var sortedProjectGroupProjects = new List<Project>();

                foreach (var projectType in ProjectTypeHelper.GetAvailableProjectTypesAsStrings())
                {
                    foreach (var project in projectGroupProjects)
                    {
                        if (project.Name.Contains(projectType))
                        {
                            sortedProjectGroupProjects.Add(project);
                            break;
                        }
                    }
                }

                projectGroups[projectGroup.Key].Clear();
                projectGroups[projectGroup.Key].AddRange(sortedProjectGroupProjects);
            }

            // Create one list again
            var sortedProjects = new List<Project>();
            foreach (var projectGroupName in projectGroups.Keys.OrderBy(x => x))
            {
                foreach (var projectGroupProject in projectGroups[projectGroupName])
                {
                    sortedProjects.Add(projectGroupProject);
                }
            }

            return sortedProjects;
        }
    }
}