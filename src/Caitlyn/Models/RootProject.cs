// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootProject.cs" company="Caitlyn development team">
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
    using System.Xml.Serialization;
    using Catel.Data;

    /// <summary>
    /// RootProject Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    [Serializable]
    public class RootProject : ModelBase, IRootProject
    {
        #region Constructor & destructor
        public RootProject()
        {
        }

        protected RootProject(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        [DefaultValue("")]
        [XmlAttribute]
        public string Name { get; set; }

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
        public static readonly PropertyData RulesProperty = RegisterProperty("Rules", typeof(ObservableCollection<Rule>), () => new ObservableCollection<Rule>());
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