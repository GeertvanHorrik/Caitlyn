// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMapping.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Catel.Data;

    /// <summary>
    /// ProjectMapping Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    public class ProjectMapping : ModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMapping"/> class. 
        /// Initializes a new object from scratch.
        /// </summary>
        public ProjectMapping()
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMapping"/> class. 
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info">
        /// <see cref="SerializationInfo"/> that contains the information.
        /// </param>
        /// <param name="context">
        /// <see cref="StreamingContext"/>.
        /// </param>
        protected ProjectMapping(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the source project.
        /// </summary>
        public string SourceProject
        {
            get
            {
                return GetValue<string>(SourceProjectProperty);
            }
            set
            {
                SetValue(SourceProjectProperty, value);
            }
        }

        /// <summary>
        /// Register the SourceProject property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SourceProjectProperty = RegisterProperty("SourceProject", typeof(string), null);

        /// <summary>
        /// Gets or sets the name of the target project.
        /// </summary>
        public string TargetProject
        {
            get
            {
                return GetValue<string>(TargetProjectProperty);
            }
            set
            {
                SetValue(TargetProjectProperty, value);
            }
        }

        /// <summary>
        /// Register the TargetProject property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TargetProjectProperty = RegisterProperty("TargetProject", typeof(string), null);
        #endregion

        #region Methods
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(SourceProject))
            {
                validationResults.Add(FieldValidationResult.CreateError(SourceProjectProperty, "Source project is required"));
            }

            if (string.IsNullOrWhiteSpace(TargetProject))
            {
                validationResults.Add(FieldValidationResult.CreateError(TargetProjectProperty, "Target project is required"));
            }
        }

        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            if (!string.IsNullOrWhiteSpace(SourceProject) && !string.IsNullOrWhiteSpace(TargetProject))
            {
                if (string.Equals(SourceProject, TargetProject))
                {
                    validationResults.Add(BusinessRuleValidationResult.CreateError("Source and target project cannot be the same project"));
                }
            }
        }
        #endregion
    }
}