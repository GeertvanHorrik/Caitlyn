// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectProjectsView.xaml.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Views
{
    using System.Windows;

    using Caitlyn.ViewModels;

    using Catel.Windows;

    /// <summary>
    /// Interaction logic for SelectProjectsView.xaml.
    /// </summary>
    public partial class SelectProjectsView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectsView"/> class.
        /// </summary>
        public SelectProjectsView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectsView"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The view Model.
        /// </param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SelectProjectsView(SelectProjectsViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}