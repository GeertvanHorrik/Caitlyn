// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectItem.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Project item types.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum ProjectItemType
    {
        /// <summary>
        /// File.
        /// </summary>
        File,

        /// <summary>
        /// Folder.
        /// </summary>
        Folder
    }

    public interface IProjectItem
    {
        #region Properties
        IProjectItem Parent { get; }

        string Name { get; }

        ProjectItemType Type { get; }

        ObservableCollection<IProjectItem> ProjectItems { get; }

        string FullName { get; }
        #endregion
    }
}