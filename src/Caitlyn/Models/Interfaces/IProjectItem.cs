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
        /// <summary>
        /// Gets the parent. If the parent is <c>null</c>, this item is the root.
        /// </summary>
        IProjectItem Parent { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Gets or sets the project item type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        ProjectItemType Type { get; }

        /// <summary>
        /// Gets the project items that are a contained by this project item.
        /// </summary>
        /// <value>
        /// The project items.
        /// </value>
        ObservableCollection<IProjectItem> ProjectItems { get; }

        /// <summary>
        /// Gets the full name, which is calculated by walking up the tree via the <see cref="Parent"/> property.
        /// </summary>
        string FullName { get; }
        #endregion
    }
}