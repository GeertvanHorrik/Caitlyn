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
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Caitlyn.Runtime.Serialization;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    /// <summary>
    /// Rule Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    [SerializerModifier(typeof(RuleSerializerModifier))]
    public class Rule : ModelBase, IRule
    {
        #region Constructor & destructor
        public Rule()
        {
        }

        protected Rule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        [DefaultValue("")]
        public string Name { get; set; }

        [DefaultValue(RuleType.DoNotAdd)]
        public RuleType Type { get; set; }

        public ObservableCollection<ProjectType> ProjectTypes
        {
            get { return GetValue<ObservableCollection<ProjectType>>(ProjectTypesProperty); }
            set { SetValue(ProjectTypesProperty, value); }
        }

        /// <summary>
        /// Register the ProjectTypes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectTypesProperty = RegisterProperty("ProjectTypes", typeof(ObservableCollection<ProjectType>), () => new ObservableCollection<ProjectType>());
        #endregion

        #region Methods
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError("Name", "Name is required"));
            }
        }
        #endregion
    }
}