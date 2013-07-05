// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRootProject.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    public interface IRootProject
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the root project.
        /// </summary>
        [XmlAttribute]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of rules.
        /// </summary>
        ObservableCollection<Rule> Rules { get; }
        #endregion
    }
}