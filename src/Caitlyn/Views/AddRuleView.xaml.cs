// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddRuleView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for AddRuleView.xaml.
    /// </summary>
    public partial class AddRuleView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRuleView"/> class.
        /// </summary>
        public AddRuleView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRuleView"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public AddRuleView(AddRuleViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion
    }
}