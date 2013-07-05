// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectProjectItemView.xaml.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Views
{
    using System.Windows;
    using System.Windows.Controls;

    using Caitlyn.ViewModels;

    using Catel.Windows;

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
        /// <param name="viewModel">
        /// The view Model.
        /// </param>
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
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = (TreeView)sender;

            var viewModel = ViewModel as SelectProjectItemViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedProjectItemPath = treeView.SelectedValue as string;
            }
        }
        #endregion
    }
}