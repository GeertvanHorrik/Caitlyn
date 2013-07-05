// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingsView.xaml.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Views
{
    using System.Windows;

    using Caitlyn.ViewModels;

    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ProjectMappingsView.xaml.
    /// </summary>
    public partial class ProjectMappingsView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsView"/> class.
        /// </summary>
        public ProjectMappingsView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsView"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The view model to inject.
        /// </param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ProjectMappingsView(ProjectMappingsViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}