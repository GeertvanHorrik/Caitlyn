// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualStudioLogListener.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2014 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Caitlyn.Logging
{
    using System;
    using Catel.Logging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    public class VisualStudioLogListener : LogListenerBase
    {
        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData)
        {
            var outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            Guid generalPaneGuid = VSConstants.GUID_OutWindowGeneralPane; // P.S. There's also the GUID_OutWindowDebugPane available.
            IVsOutputWindowPane generalPane;
            outWindow.GetPane(ref generalPaneGuid, out generalPane);

            if (generalPane != null)
            {
                generalPane.OutputString(message);
                generalPane.Activate(); // Brings this pane into view
            }
        }
    }
}