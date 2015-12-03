//===============================================================================
// Enterprise Library 2.0 Extensions
//===============================================================================
// Copyright © 2006 Erwyn van der Meer.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design.TraceListeners;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design
{
	sealed class LoggingCommandRegistrar : CommandRegistrar
	{	
		public LoggingCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddRollingFileTraceListenerCommand();
			AddDefaultCommands(typeof(RollingFileTraceListenerNode));
		}

		private void AddRollingFileTraceListenerCommand()
		{
			AddMultipleChildNodeCommand(Resources.RollingFileTraceListenerUICommandText,
				Resources.RollingFileTraceListenerUICommandLongText,
				typeof(RollingFileTraceListenerNode),
                typeof(TraceListenerCollectionNode));
		}
	}
}
