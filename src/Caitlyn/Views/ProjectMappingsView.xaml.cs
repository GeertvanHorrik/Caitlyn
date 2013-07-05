namespace Caitlyn.Views
{
    using System.Windows;
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ProjectMappingsView.xaml.
    /// </summary>
    public partial class ProjectMappingsView : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsView"/> class.
        /// </summary>
        public ProjectMappingsView()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMappingsView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ProjectMappingsView(ProjectMappingsViewModel viewModel)
            : base(viewModel)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles(Application.Current.Resources, Resources);

            InitializeComponent();
        }
    }
}
