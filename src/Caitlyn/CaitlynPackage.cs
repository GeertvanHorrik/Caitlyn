// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaitlynPackage.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;

    using Caitlyn.Services;
    using Caitlyn.ViewModels;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;

    using EnvDTE;

    using EnvDTE80;

    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// 
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideBindingPath]
    [ProvideAutoLoad("{adfc4e64-0397-11d1-9f4e-00a0c911004f}")] // autoload when no solution exists
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")] // autoload when solution exists
    [Guid(GuidList.guidCaitlynPkgString)]
    public sealed class CaitlynPackage : Package
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CaitlynPackage"/> class. 
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public CaitlynPackage()
        {
#if DEBUG
            LogManager.AddDebugListener(false);
#endif

            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("JetBrains"));
            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("Telerik"));
            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("DevExpress"));
            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("SyncFusion"));
            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("Microsoft.TeamFoundation"));
            TypeCache.ShouldIgnoreAssemblyEvaluators.Add(x => x.FullName.Contains("FSharp"));

            CatelEnvironment.BypassDevEnvCheck = true;

            var assembly = GetType().Assembly;
            var configPath = string.Format("{0}.config", assembly.Location);
            LogManager.LoadListenersFromConfigurationFile(configPath, assembly);

            Log.Info("Caitlyn started");
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Methods
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Log.Info("Initializing");

            var currentApplication = Application.Current ?? new Application();
            currentApplication.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/Catel.Extensions.Controls;component/themes/generic.xaml", UriKind.RelativeOrAbsolute) });

            base.Initialize();

            var typeFactory = this.GetTypeFactory();

            Log.Info("Retrieving instance of Visual Studio");

            var vs = GetVisualStudio();

            Log.Info("Setting up service locator");

            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterInstance<DTE2>(vs);
            serviceLocator.RegisterType<IVisualStudioService, VisualStudioService>();

            var configurationService = new ConfigurationService(vs);
            serviceLocator.RegisterInstance<IConfigurationService>(configurationService);

            var autoLinkerService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<AutoLinkerService>(vs);
            serviceLocator.RegisterInstance<IAutoLinkerService>(autoLinkerService);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                Log.Info("Creating menu entries");

                // Link files
                var linkProjectsMenuItemCommandId = new CommandID(GuidList.guidCaitlynCmdSet, (int)PkgCmdIDList.LinkFiles);
                var linkProjectsMenuItem = new MenuCommand(OnLinkProjects, linkProjectsMenuItemCommandId);
                mcs.AddCommand(linkProjectsMenuItem);

                // Configuration
                var configurationMenuItemCommandId = new CommandID(GuidList.guidCaitlynCmdSet, (int)PkgCmdIDList.Configuration);
                var configurationMenuItem = new MenuCommand(OnConfiguration, configurationMenuItemCommandId);
                mcs.AddCommand(configurationMenuItem);

                // Add rule
                var addRuleMenuItemCommandId = new CommandID(GuidList.guidCaitlynCmdSet, (int)PkgCmdIDList.AddRule);
                var addRuleMenuItem = new MenuCommand(OnAddRule, addRuleMenuItemCommandId);
                mcs.AddCommand(addRuleMenuItem);
            }

            Log.Info("Initialized");
        }

        /// <summary>
        /// Called when the link projects menu item is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnLinkProjects(object sender, EventArgs e)
        {
            Log.Info("Linking projects");

            if (!IsSolutionOpenend())
            {
                return;
            }

            var serviceLocator = ServiceLocator.Default;
            var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            var configurationService = serviceLocator.ResolveType<IConfigurationService>();

            Log.Info("Loading configuration for current solution");

            var dte = GetVisualStudio();
            var configuration = configurationService.LoadConfigurationForCurrentSolution();

            var vm = new SelectProjectsViewModel(dte, dte.Solution.GetAllProjects());
            if (uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Log.Info("Linking projects dialog returned success, linking projects");

                var pleaseWaitService = serviceLocator.ResolveType<IPleaseWaitService>();
                pleaseWaitService.Show("Linking projects, please be patient");

                var activeProject = vm.RootProject;
                var projectsToLink = (from selectableProject in vm.SelectableProjects where selectableProject.IsChecked select selectableProject.Project).ToArray();

                var linker = new Linker(activeProject, projectsToLink, configuration);
                linker.RemoveMissingFiles = vm.RemoveMissingFiles;
                linker.LinkFiles();

                Log.Info("Linked projects");

                pleaseWaitService.Hide();
            }
        }

        /// <summary>
        /// Called when the configuration menu item is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnConfiguration(object sender, EventArgs e)
        {
            Log.Info("Opening configuration");

            if (!IsSolutionOpenend())
            {
                return;
            }

            var serviceLocator = ServiceLocator.Default;
            var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            var configurationService = serviceLocator.ResolveType<IConfigurationService>();

            Log.Info("Loading configuration for current solution");

            var configuration = configurationService.LoadConfigurationForCurrentSolution();

            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<ConfigurationViewModel>(configuration);
            if (uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Log.Info("Configuration dialog returned success, saving configuration");

                configurationService.SaveConfigurationForCurrentSolution();
            }
        }

        /// <summary>
        /// Called when the add rule menu item is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnAddRule(object sender, EventArgs e)
        {
            Log.Info("Adding rule");

            if (!IsSolutionOpenend())
            {
                return;
            }

            var serviceLocator = ServiceLocator.Default;
            var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            var configurationService = serviceLocator.ResolveType<IConfigurationService>();

            Log.Info("Loading configuration for current solution");

            var configuration = configurationService.LoadConfigurationForCurrentSolution();

            var vm = TypeFactory.Default.CreateInstanceWithParametersAndAutoCompletion<AddRuleViewModel>(configuration);
            if (uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Log.Info("Adding rule dialog returned success, saving configuration");

                configurationService.SaveConfigurationForCurrentSolution();
            }
        }

        /// <summary>
        /// Determines whether visual studio has currently a solution opened. If not, this method will show an error
        /// message to the user and return <c>false</c>. Otherwise it will return <c>true</c>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if visual studio currently has a solution opened; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSolutionOpenend()
        {
            var dte = GetVisualStudio();
            if (!dte.IsSolutionOpened())
            {
                Log.Warning("No solution is opened, cannot continue");

                var messageService = ServiceLocator.Default.ResolveType<IMessageService>();
                messageService.ShowError("This extension cannot be used without opening a solution first");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the visual studio object.
        /// </summary>
        /// <returns>The <see cref="DTE" /> representing visual studio.</returns>
        private DTE2 GetVisualStudio()
        {
            // return System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.11.0") as DTE2;
            return (DTE2)Package.GetGlobalService(typeof(SDTE));
        }
        #endregion
    }
}