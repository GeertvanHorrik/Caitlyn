// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectItem.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;

    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using EnvDTE;

    /// <summary>
    /// ProjectItem Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    public class ProjectItem : ModelBase, IProjectItem
    {
        #region Constructor & destructor
        public ProjectItem(Project project)
        {
            Argument.IsNotNull("project", project);

            Initialize(string.Empty, ProjectItemType.Folder);

            foreach (EnvDTE.ProjectItem subProjectItem in project.ProjectItems)
            {
                ProjectItems.Add(new ProjectItem(subProjectItem, this));
            }
        }

        public ProjectItem(EnvDTE.ProjectItem projectItem, IProjectItem parent = null)
        {
            Argument.IsNotNull("projectItem", projectItem);

            Initialize(projectItem.GetObjectName(), projectItem.IsFolder() ? ProjectItemType.Folder : ProjectItemType.File, parent);

            foreach (EnvDTE.ProjectItem subProjectItem in projectItem.ProjectItems)
            {
                ProjectItems.Add(new ProjectItem(subProjectItem, this));
            }
        }

        public ProjectItem(string name, ProjectItemType projectItemType, IProjectItem parent = null)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            Initialize(name, projectItemType, parent);
        }

        protected ProjectItem(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        [NoWeaving]
        public IProjectItem Parent { get; set; }

        public string Name { get; set; }

        [DefaultValue(ProjectItemType.File)]
        public ProjectItemType Type { get; set; }

        /// <summary>
        /// Gets the project items that are a contained by this project item.
        /// </summary>
        public ObservableCollection<IProjectItem> ProjectItems
        {
            get { return GetValue<ObservableCollection<IProjectItem>>(ProjectItemsProperty); }
            private set { SetValue(ProjectItemsProperty, value); }
        }

        /// <summary>
        /// Register the ProjectItems property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectItemsProperty = RegisterProperty("ProjectItems", typeof(ObservableCollection<IProjectItem>), () => new ObservableCollection<IProjectItem>());

        public string FullName
        {
            get
            {
                if (Parent == null)
                {
                    return Name;
                }

                return Path.Combine(Parent.FullName, Name);
            }
        }
        #endregion

        #region Methods
        private void Initialize(string name, ProjectItemType projectItemType, IProjectItem parent = null)
        {
            if (parent == null)
            {
                Argument.IsNotNull("name", name);
            }
            else
            {
                Argument.IsNotNullOrWhitespace("name", name);
            }

            Name = name;
            Type = projectItemType;
            Parent = parent;
        }
        #endregion
    }
}