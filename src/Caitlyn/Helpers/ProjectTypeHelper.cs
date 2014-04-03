// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectTypeHelper.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2014 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Caitlyn
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class ProjectTypeHelper
    {
        private static ProjectType[] _projectTypes;
        private static string[] _projectTypesAsStrings; 

        static ProjectTypeHelper()
        {
            InitializeAvailableProjectTypes();
            InitializeAvailableProjectTypesAsStrings();
        }

        #region Methods
        private static void InitializeAvailableProjectTypes()
        {
            var types = new List<ProjectType>();

            foreach (var projectType in Enum<ProjectType>.ToList())
            {
                if (projectType.ToString().Equals("WP8"))
                {
                    // WP8 is deprecated
                    continue;
                }

                types.Add(projectType);
            }

            _projectTypes =  types.Distinct().ToArray();
        }

        public static void InitializeAvailableProjectTypesAsStrings()
        {
            var strings = new List<string>();

            foreach (var projectType in GetAvailableProjectTypes())
            {
                strings.Add(projectType.ToString().Replace("_", "."));
            }

            _projectTypesAsStrings = strings.ToArray();
        }

        public static ProjectType[] GetAvailableProjectTypes()
        {
            return _projectTypes;
        }

        public static string[] GetAvailableProjectTypesAsStrings()
        {
            return _projectTypesAsStrings;
        }
        #endregion
    }
}