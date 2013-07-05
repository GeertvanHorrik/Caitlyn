// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootProjectViewModel.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Caitlyn.Models;
    using Caitlyn.Services;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.MVVM.Services;

    /// <summary>
    /// RootProject view model.
    /// </summary>
    public class RootProjectViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RootProjectViewModel"/> class.
        /// </summary>
        /// <param name="rootProject">
        /// The root project.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="rootProject"/> is <c>null</c>.
        /// </exception>
        public RootProjectViewModel(RootProject rootProject)
        {
            Argument.IsNotNull("rootProject", rootProject);

            var visualStudioService = GetService<IVisualStudioService>();
            var availableProjects = visualStudioService.GetAllProjects();

            foreach (var availableProject in availableProjects)
            {
                AvailableProjects.Add(availableProject.Name);
            }

            RootProject = rootProject;

            Add = new Command(OnAddExecute);
            Edit = new Command(OnEditExecute, OnEditCanExecute);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
        }
        #endregion

        #region Constants
        /// <summary>
        /// Register the RootProject property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RootProjectProperty = RegisterProperty("RootProject", typeof(RootProject));

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty, (sender, e) => ((RootProjectViewModel)sender).RaisePropertyChanged("Title"));

        /// <summary>
        /// Register the AvailableProjects property so it is known in the class.
        /// </summary>
        public static readonly PropertyData AvailableProjectsProperty = RegisterProperty("AvailableProjects", typeof(List<string>), () => new List<string>());

        /// <summary>
        /// Register the Rules property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RulesProperty = RegisterProperty("Rules", typeof(ObservableCollection<Rule>));

        /// <summary>
        /// Register the SelectedRule property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedIgnoredItemProperty = RegisterProperty("SelectedRule", typeof(Rule));
        #endregion

        #region Commands

        #region Properties
        /// <summary>
        /// Gets the Add command.
        /// </summary>
        public Command Add { get; private set; }

        /// <summary>
        /// Gets the Edit command.
        /// </summary>
        public Command Edit { get; private set; }

        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public Command Remove { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private void OnAddExecute()
        {
            var rule = new Rule();
            var vm = new RuleViewModel(RootProject, rule);

            var uiVisualizerService = GetService<IUIVisualizerService>();
            if (uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Rules.Add(rule);
                SelectedRule = rule;
            }
        }

        /// <summary>
        /// Method to check whether the Edit command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
        private bool OnEditCanExecute()
        {
            return (SelectedRule != null);
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private void OnEditExecute()
        {
            var rule = SelectedRule;
            var vm = new RuleViewModel(RootProject, rule);

            var uiVisualizerService = GetService<IUIVisualizerService>();
            uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
        private bool OnRemoveCanExecute()
        {
            return (SelectedRule != null);
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            var messageService = GetService<IMessageService>();
            if (messageService.Show("Are you sure you want to remove the selected rule including all it's settings?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                Rules.Remove(SelectedRule);
                SelectedRule = null;
            }
        }
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return string.Format("Root project '{0}'", RootProject.Name);
            }
        }

        /// <summary>
        /// Gets the root project.
        /// </summary>
        [Model]
        public RootProject RootProject
        {
            get
            {
                return GetValue<RootProject>(RootProjectProperty);
            }
            private set
            {
                SetValue(RootProjectProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ViewModelToModel("RootProject")]
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
        /// Gets or sets the available root projects.
        /// </summary>
        public List<string> AvailableProjects
        {
            get
            {
                return GetValue<List<string>>(AvailableProjectsProperty);
            }
            set
            {
                SetValue(AvailableProjectsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of rules.
        /// </summary>
        [ViewModelToModel("RootProject")]
        public ObservableCollection<Rule> Rules
        {
            get
            {
                return GetValue<ObservableCollection<Rule>>(RulesProperty);
            }
            set
            {
                SetValue(RulesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected rule.
        /// </summary>
        public Rule SelectedRule
        {
            get
            {
                return GetValue<Rule>(SelectedIgnoredItemProperty);
            }
            set
            {
                SetValue(SelectedIgnoredItemProperty, value);
            }
        }
        #endregion
    }
}