namespace Caitlyn.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using Catel.Data;

    /// <summary>
    /// Configuration Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class Configuration : SavableDataObjectBase<Configuration>, IConfiguration
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new object from scratch.
        /// </summary>
        public Configuration()
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains the information.</param>
        /// <param name="context"><see cref="StreamingContext"/>.</param>
        protected Configuration(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
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
        public static readonly PropertyData RootProjectsProperty = RegisterProperty("RootProjects", typeof (ObservableCollection<RootProject>), () => new ObservableCollection<RootProject>());

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

        /// <summary>
        /// Gets or sets whether this solution should autolink.
        /// </summary>
        public bool EnableAutoLink
        {
            get { return GetValue<bool>(EnableAutoLinkProperty); }
            set { SetValue(EnableAutoLinkProperty, value); }
        }

        /// <summary>
        /// Register the EnableAutoLink property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EnableAutoLinkProperty = RegisterProperty("EnableAutoLink", typeof(bool), true);
        #endregion

        #region Methods
        #endregion
    }
}