// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rule.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    using Catel.Data;

    /// <summary>
    /// Rule Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    public class Rule : ModelBase, IRule
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class. 
        /// Initializes a new object from scratch.
        /// </summary>
        public Rule()
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class. 
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info">
        /// <see cref="SerializationInfo"/> that contains the information.
        /// </param>
        /// <param name="context">
        /// <see cref="StreamingContext"/>.
        /// </param>
        protected Rule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return GetValue<string>(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty);

        /// <summary>
        /// Gets or sets the rule type.
        /// </summary>
        public RuleType Type
        {
            get
            {
                return GetValue<RuleType>(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        /// <summary>
        /// Register the Type property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TypeProperty = RegisterProperty("Type", typeof(RuleType), RuleType.DoNotAdd);

        /// <summary>
        /// Gets or sets the list of project types to ignore the item for.
        /// </summary>
        public ObservableCollection<ProjectType> ProjectTypes
        {
            get
            {
                return GetValue<ObservableCollection<ProjectType>>(ProjectTypesProperty);
            }
            set
            {
                SetValue(ProjectTypesProperty, value);
            }
        }

        /// <summary>
        /// Register the ProjectTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectTypesProperty = RegisterProperty("ProjectTypes", typeof(ObservableCollection<ProjectType>), () => new ObservableCollection<ProjectType>());
        #endregion

        #region Methods
        /// <summary>
        /// Validates the field values of this object. Override this method to enable
        /// validation of field values.
        /// </summary>
        /// <param name="validationResults">
        /// The validation results, add additional results to this list.
        /// </param>
        /// <remarks>
        /// </remarks>
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