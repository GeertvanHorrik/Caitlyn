namespace Caitlyn.Views
{
    using System.Windows;
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ProjectMappingView.xaml.
    /// </summary>
    public partial class ProjectMappingView : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingView"/> class.
        /// </summary>
        public ProjectMappingView()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ProjectMappingView(ProjectMappingViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
    }
}
