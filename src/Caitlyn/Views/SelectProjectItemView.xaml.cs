// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectProjectItemView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SelectProjectItemView.xaml.
    /// </summary>
    public partial class SelectProjectItemView : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectItemView"/> class.
        /// </summary>
        public SelectProjectItemView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectProjectItemView"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SelectProjectItemView(SelectProjectItemViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the selection on the tree view changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = (TreeView) sender;

            var viewModel = ViewModel as SelectProjectItemViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedProjectItemPath = treeView.SelectedValue as string;
            }
        }
        #endregion
    }
}