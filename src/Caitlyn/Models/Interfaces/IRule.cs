namespace Caitlyn.Models
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Rule types.
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// Rule that the item should not be added to the specified target projects.
        /// </summary>
        DoNotAdd,

        /// <summary>
        /// Rule that the item should not be removed from the specified target projects.
        /// </summary>
        DoNotRemove
    }

    public interface IRule
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the rule type.
        /// </summary>
        /// <value>The rule type.</value>
        /// <remarks></remarks>
        RuleType Type { get; set; }

        /// <summary>
        /// Gets or sets the list of project types to ignore the item for.
        /// </summary>
        ObservableCollection<ProjectType> ProjectTypes { get; set; }
    }
}