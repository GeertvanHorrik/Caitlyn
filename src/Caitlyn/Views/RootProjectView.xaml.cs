// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootProjectView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for RootProjectView.xaml.
    /// </summary>
    public partial class RootProjectView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RootProjectView"/> class.
        /// </summary>
        public RootProjectView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootProjectView"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public RootProjectView(RootProjectViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}