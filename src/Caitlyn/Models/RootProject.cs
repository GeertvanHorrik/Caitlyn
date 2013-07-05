namespace Caitlyn.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Catel.Data;

    /// <summary>
    /// RootProject Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class RootProject : DataObjectBase<RootProject>, IRootProject
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new object from scratch.
        /// </summary>
        public RootProject()
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains the information.</param>
        /// <param name="context"><see cref="StreamingContext"/>.</param>
        protected RootProject(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the root project.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof (string), string.Empty);

        /// <summary>
        /// Gets or sets the list of rules.
        /// </summary>
        public ObservableCollection<Rule> Rules
        {
            get { return GetValue<ObservableCollection<Rule>>(RulesProperty); }
            private set { SetValue(RulesProperty, value); }
        }

        /// <summary>
        /// Register the Rules property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RulesProperty = RegisterProperty("Rules", typeof (ObservableCollection<Rule>), () => new ObservableCollection<Rule>());
        #endregion

        #region Methods
        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">The validation results, add additional results to this list.</param>
        /// <remarks></remarks>
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError(NameProperty, "Name is required"));
            }
        }
        #endregion
    }
}