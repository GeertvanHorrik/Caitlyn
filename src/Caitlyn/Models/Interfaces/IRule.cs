// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRule.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Rule types.
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// Rule that the item should not be added to the specified target projects.
        /// </summary>
        DoNotAdd, 

        /// <summary>
        /// Rule that the item should not be removed from the specified target projects.
        /// </summary>
        DoNotRemove
    }

    public interface IRule
    {
        #region Properties
        string Name { get; set; }

        RuleType Type { get; set; }

        ObservableCollection<ProjectType> ProjectTypes { get; set; }
        #endregion
    }
}