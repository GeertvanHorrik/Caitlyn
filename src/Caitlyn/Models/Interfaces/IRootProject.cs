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
        string Name { get; set; }

        ObservableCollection<Rule> Rules { get; }
        #endregion
    }
}