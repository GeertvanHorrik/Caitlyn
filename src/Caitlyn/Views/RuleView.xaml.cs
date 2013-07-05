// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for RuleView.xaml.
    /// </summary>
    public partial class RuleView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleView"/> class.
        /// </summary>
        public RuleView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleView"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public RuleView(RuleViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}