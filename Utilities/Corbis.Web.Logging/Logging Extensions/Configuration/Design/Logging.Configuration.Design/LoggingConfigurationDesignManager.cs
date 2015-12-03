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
using System.Configuration;
using System.Diagnostics;
//using System.Windows.Forms;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the exception handling settings configuration section.
	/// </summary>
    public sealed class LoggingConfigurationDesignManager : ConfigurationDesignManager
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="LoggingConfigurationDesignManager"/> class.
		/// </summary>
        public LoggingConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			LoggingCommandRegistrar cmdRegistrar = new LoggingCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			LoggingNodeMapRegistrar registrar = new LoggingNodeMapRegistrar(serviceProvider);
			registrar.Register();
        }
   }
}