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
        public ProjectMapping()
        {
        }

        protected ProjectMapping(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        public string SourceProject { get; set; }

        public string TargetProject { get; set; }
        #endregion

        #region Methods
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(SourceProject))
            {
                validationResults.Add(FieldValidationResult.CreateError("SourceProject", "Source project is required"));
            }

            if (string.IsNullOrWhiteSpace(TargetProject))
            {
                validationResults.Add(FieldValidationResult.CreateError("TargetProject", "Target project is required"));
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