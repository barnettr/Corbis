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
using System.Diagnostics;
using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration
{
    /// <summary>
    /// A class that contains the configuration settings for a <see cref="RollingFileTraceListener"/>.
    /// This class only contains data and has no behavior. It doesn't validate the settings.
    /// </summary>
    /// <remarks>This class duplicates all needed configuration properties of <see cref="RollingFileTraceListenerData"/>. 
    /// The settings in an instance of <see cref="RollingFileTraceListenerData"/> cannot be modified 
    /// at run-time or they are written back to a configuration file. However, 
    /// the <see cref="RollingFileTraceListenerSettings"/> class allows run-time modification of
    /// settings without any side-effects on the configuration file that is used to populate 
    /// these settings on application startup.</remarks>
    internal sealed class RollingFileTraceListenerSettings
    {
        #region Private fields
        /// <summary>
        /// The file name (including path) for the log file.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The format string for the time stamp in a rolled-over log file name.
        /// </summary>
        private string _timeStampFormat;

        /// <summary>
        /// The numeric value representing the file age threshold in the configured units.
        /// </summary>
        private int _ageThreshold;

        /// <summary>
        /// The age units in which the value of the <see cref="AgeThreshold"/> property is configured.
        /// in Minutes, Hours, Days, Weeks or Months.
        /// </summary>
        private AgeThresholdUnit _ageUnit;

        /// <summary>
        /// The numeric value representing the file size threshold
        /// in the configured units.
        /// </summary>
        private int _sizeThreshold;

        /// <summary>
        /// The size unit in which the value of the <see cref="SizeThreshold"/>
        /// property is configured.
        /// </summary>
        private SizeThresholdUnit _sizeUnit;

        /// <summary>
        /// The maximum number of logfiles allowed before the oldest ones should be purged.
        /// </summary>
        /// <remarks>A value of <c>0></c> means no purging should take place, so then all logfiles are kept.</remarks>
        private int _maximumLogFilesBeforePurge;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes an instance from a <see cref="RollingFileTraceListenerData"/> instance.
        /// </summary>
        /// <param name="data">The <see cref="RollingFileTraceListenerData"/> instance with settings
        /// to initialize this instance with.</param>
        public RollingFileTraceListenerSettings(RollingFileTraceListenerData data)
        {
            this.FileName = data.FileName;
            this.TimestampFormat = data.TimestampFormat;
            this.AgeThreshold = data.AgeThreshold;
            this.AgeUnit = data.AgeUnit;
            this.SizeThreshold = data.SizeThreshold;
            this.SizeUnit = data.SizeUnit;
            this.MaximumLogFilesBeforePurge = data.MaximumLogFilesBeforePurge;

        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the file name (including path) for the log file.
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the format string for the time stamp in a rolled-over log file name.
        /// </summary>
        public string TimestampFormat
        {
            get
            {
                return _timeStampFormat;
            }
            set
            {
                _timeStampFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric value representing the file age threshold in the configured units.
        /// </summary>
        public int AgeThreshold
        {
            get
            {
                return _ageThreshold;
            }
            set
            {
                _ageThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the age unit in which the value of the <see cref="AgeThreshold"/> property 
        /// is configured.
        /// </summary>
        public AgeThresholdUnit AgeUnit
        {
            get
            {
                return _ageUnit;
            }
            set
            {
                _ageUnit = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric value representing the file size threshold
        /// in the configured units.
        /// </summary>
        public int SizeThreshold
        {
            get
            {
                return _sizeThreshold;
            }
            set
            {
                _sizeThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the size unit in which the value of the <see cref="SizeThreshold"/>
        /// property is configured.
        /// </summary>
        public SizeThresholdUnit SizeUnit
        {
            get
            {
                return _sizeUnit;
            }
            set
            {
                _sizeUnit = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of logfiles allowed before the oldest ones should be purged.
        /// </summary>
        /// <remarks>A value of <c>0></c> means no purging should take place, so then all logfiles are kept.</remarks>
        public int MaximumLogFilesBeforePurge
        {
            get
            {
                return _maximumLogFilesBeforePurge;
            }
            set
            {
                _maximumLogFilesBeforePurge = value;
            }
        }
        #endregion
    }
}
