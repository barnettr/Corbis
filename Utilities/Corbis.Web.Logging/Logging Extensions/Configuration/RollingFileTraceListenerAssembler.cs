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
using System.Diagnostics;

using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.FlatFileTraceListener"/> described by a <see cref="RollingFileTraceListenerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="RollingFileTraceListenerData"/> type
    /// and it is used by the <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.TraceListenerCustomFactory"/> 
    /// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
    /// </remarks>
    public sealed class RollingFileTraceListenerAssembler : TraceListenerAsssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="RollingFileTraceListener"/> based on an instance of <see cref="RollingFileTraceListenerData"/>.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="RollingFileTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="RollingFileTraceListener"/>.</returns>
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            RollingFileTraceListenerData configurationData
                = (RollingFileTraceListenerData) objectConfiguration;

            ILogFormatter formatter = GetFormatter(context, configurationData.Formatter, configurationSource, reflectionCache);

            TraceListener createdObject
                = new RollingFileTraceListener(configurationData, formatter);

            return createdObject;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RollingFileTraceListenerAssembler()
        {
        }
    }
}
