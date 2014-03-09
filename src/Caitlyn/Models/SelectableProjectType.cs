// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableProjectType.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using Catel.Data;

    /// <summary>
    /// Helper class to be able to select projec types in the view.
    /// </summary>
    public class SelectableProjectType : ModelBase
    {
        #region Constructors
        public SelectableProjectType(ProjectType projectType, bool isSelected = false)
        {
            ProjectType = projectType;
            Name = projectType.ToString();
            IsSelected = isSelected;
        }
        #endregion

        #region Properties
        public ProjectType ProjectType { get; private set; }

        public string Name { get; private set; }

        public bool IsSelected { get; set; }
        #endregion
    }
}