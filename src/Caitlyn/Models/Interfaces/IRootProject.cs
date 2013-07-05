namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    public interface IRootProject
    {
        /// <summary>
        /// Gets or sets the name of the root project.
        /// </summary>
        [XmlAttribute]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of rules.
        /// </summary>
        ObservableCollection<Rule> Rules { get; }
    }
}