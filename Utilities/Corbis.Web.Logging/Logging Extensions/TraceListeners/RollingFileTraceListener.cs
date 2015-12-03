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
using System.IO;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration;
using System.Collections.Generic;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners
{
	/// <summary>
	/// Represents a flat text file file trace listener with roll over capabilities.  
	/// </summary>
	public sealed class RollingFileTraceListener : FlatFileTraceListener
    {
        #region Private fields
        /// <summary>
        /// The configuration settings.
        /// </summary>
        private RollingFileTraceListenerSettings _configurationSettings;

        /// <summary>
        /// The helper class to roll over log files.
        /// </summary>
        private LogRoller _logRoller;
        #endregion

        #region Constructor
        /// <summary>
		/// Initializes a new instance of <see cref="RollingFileTraceListener"/> with a file name and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
        /// <param name="configurationData">The configuration dataa for this instance.</param>
		/// <param name="formatter">The formatter.</param>
        public RollingFileTraceListener(RollingFileTraceListenerData configurationData, ILogFormatter formatter)
            : base(EnsureDataNotNull(configurationData).FileName, configurationData.Header, configurationData.Footer, formatter)
		{
            _configurationSettings = new RollingFileTraceListenerSettings(configurationData);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when <paramref name="configurationData"/>
        /// is <c>null</c>.
        /// </summary>
        /// <param name="configurationData">The value to check for <c>null</c>.</param>
        /// <returns><paramref name="configurationSettings"/></returns>
        private static RollingFileTraceListenerData EnsureDataNotNull(RollingFileTraceListenerData configurationData)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException("configurationData");
            }

            return configurationData;
        }
        
        /// <summary>
        /// Adjusts the filename in the configuration data to the actual absolute path that is used
        /// by the base class <see cref="System.Diagnostics.TextWriterTraceListener"/>.
        /// </summary>
        /// <remarks>The <see cref="FlatFileTraceListener"/> base class supports relative paths and converts them internally to
        /// absolute paths. To roll over a log file we need to know the absolute path.</remarks>
        private void AdjustFileName()
        {
            StreamWriter streamWriter = this.Writer as StreamWriter;
            if (streamWriter == null) 
            {
                return;
            }

            FileStream fileStream = streamWriter.BaseStream as FileStream;
            if (fileStream == null)
            {
                return;
            }

            _configurationSettings.FileName = fileStream.Name;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Delivers the trace data to the underlying file.
        /// </summary>
        /// <param name="eventCache">The context information provided by System.Diagnostics.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType,
            int id, object data)
        {
            // Delay adjusting the filename to here, to lower the chance of the log file being locked
            // by another RollingFileTraceListener instance using the same log file if
            // that instance has not been disposed when this instance is constructed.
            if (_logRoller == null)
            {

                AdjustFileName();

                _logRoller = new LogRoller(_configurationSettings, this);
            }

            _logRoller.RolloverIfNecessary();
            base.TraceData(eventCache, source, eventType, id, data);
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Declare the supported attributes for <see cref="RollingFileTraceListener"/>.
		/// </summary>
		protected override string[] GetSupportedAttributes()
		{
            List<string> attributes = new List<string>(base.GetSupportedAttributes());

            attributes.Add(RollingFileTraceListenerData.TimestampFormatProperty);
            attributes.Add(RollingFileTraceListenerData.AgeThresholdProperty);
            attributes.Add(RollingFileTraceListenerData.AgeUnitProperty);
            attributes.Add(RollingFileTraceListenerData.SizeThresholdProperty);
            attributes.Add(RollingFileTraceListenerData.SizeUnitProperty);
            attributes.Add(RollingFileTraceListenerData.MaxNumberOfLogsProperty);

            return attributes.ToArray();
        }
        #endregion
    }
}
