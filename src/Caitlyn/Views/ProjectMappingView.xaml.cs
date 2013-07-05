// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMappingView.xaml.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Views
{
    using System.Windows;

    using Caitlyn.ViewModels;

    using Catel.Windows;

    /// <summary>
    /// Interaction logic for ProjectMappingView.xaml.
    /// </summary>
    public partial class ProjectMappingView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingView"/> class.
        /// </summary>
        public ProjectMappingView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingView"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The view model to inject.
        /// </param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ProjectMappingView(ProjectMappingViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}