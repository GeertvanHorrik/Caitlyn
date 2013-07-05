namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;

    public interface IConfiguration
    {
        /// <summary>
        /// Gets or sets the list of ignored items.
        /// </summary>
        ObservableCollection<RootProject> RootProjects { get; set; }
    }
}