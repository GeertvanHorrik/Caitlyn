// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfiguration.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;

    public interface IConfiguration
    {
        #region Properties
        /// <summary>
        /// Gets or sets the list of ignored items.
        /// </summary>
        ObservableCollection<RootProject> RootProjects { get; set; }
        #endregion
    }
}