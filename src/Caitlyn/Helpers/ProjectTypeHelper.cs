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
        #region Methods
        public static ProjectType[] GetAvailableProjectTypes()
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

            return types.Distinct().ToArray();
        }
        #endregion
    }
}