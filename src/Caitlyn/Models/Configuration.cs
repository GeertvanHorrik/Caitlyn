// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    using Catel.Data;

    /// <summary>
    /// Configuration Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    public class Configuration : SavableModelBase<Configuration>, IConfiguration
    {
        #region Constructor & destructor
        public Configuration()
        {
        }

        protected Configuration(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the list of ignored items.
        /// </summary>
        public ObservableCollection<RootProject> RootProjects
        {
            get { return GetValue<ObservableCollection<RootProject>>(RootProjectsProperty); }
            set { SetValue(RootProjectsProperty, value); }
        }

        /// <summary>
        /// Register the RootProjects property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RootProjectsProperty = RegisterProperty("RootProjects", typeof(ObservableCollection<RootProject>), () => new ObservableCollection<RootProject>());

        /// <summary>
        /// Gets or sets the list of project mappings.
        /// </summary>
        public ObservableCollection<ProjectMapping> ProjectMappings
        {
            get { return GetValue<ObservableCollection<ProjectMapping>>(ProjectMappingsProperty); }
            set { SetValue(ProjectMappingsProperty, value); }
        }

        /// <summary>
        /// Register the ProjectMappings property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectMappingsProperty = RegisterProperty("ProjectMappings", typeof(ObservableCollection<ProjectMapping>), () => new ObservableCollection<ProjectMapping>());

        [DefaultValue(true)]
        public bool EnableAutoLink { get; set; }
        #endregion
    }
}