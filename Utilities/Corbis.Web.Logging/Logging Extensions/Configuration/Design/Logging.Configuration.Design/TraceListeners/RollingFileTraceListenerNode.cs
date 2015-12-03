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
using System.ComponentModel;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Resources = LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design.Properties.Resources;
using System.Diagnostics;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design.TraceListeners
{
    public sealed class RollingFileTraceListenerNode: FlatFileTraceListenerNode
    {
        #region Private fields
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

        #region Constructors
        /// <summary>
        /// Initialize a new instance of the <see cref="RollingFileTraceListenerNode"/> class.
        /// </summary>
        public RollingFileTraceListenerNode()
            : this( GetDefaultData() )
        {
        }

		/// <summary>
        /// Initialize a new instance of the <see cref="RollingFileTraceListenerNode"/> class with a <see cref="RollingFileTraceListenerData"/> instance.
		/// </summary>
        /// <param name="traceListenerData">A <see cref="RollingFileTraceListenerData"/> instance.</param>
        public RollingFileTraceListenerNode(RollingFileTraceListenerData traceListenerData):
            base( EnsureDataNotNull(traceListenerData) )
        {
            this._ageThreshold = traceListenerData.AgeThreshold;
            this._ageUnit = traceListenerData.AgeUnit;
            this._sizeThreshold = traceListenerData.SizeThreshold;
            this._sizeUnit = traceListenerData.SizeUnit;
            this._timeStampFormat = traceListenerData.TimestampFormat;
            this._maximumLogFilesBeforePurge = traceListenerData.MaximumLogFilesBeforePurge;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the <see cref="RollingFileTraceListenerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="RollingFileTraceListenerData"/> this node represents.
        /// </value>
        //public override Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData  TraceListenerData
        //{
        //    get
        //    {
        //        string formatterName = (this.Formatter != null ? this.Formatter.Name : String.Empty);
        //        RollingFileTraceListenerData data = new RollingFileTraceListenerData
        //            (this.Name, this.Filename, this.Header, this.Footer, formatterName,
        //            this.TraceOutputOptions, this._timeStampFormat, this._ageThreshold,
        //            this._ageUnit, this._sizeThreshold, this._sizeUnit, this._maximumLogFilesBeforePurge);

        //        return data;
        //    }
        //}

        /// <summary>
        /// Gets or sets the format string for the time stamp in a rolled-over log file name.
        /// </summary>
        [SRDescription("RollingFileTraceListenerTimestampFormat", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
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
        [SRDescription("RollingFileTraceListenerAgeThreshold", typeof(Resources))]
        [SRCategory("CategoryAgeThreshold", typeof(Resources))]
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
        [SRDescription("RollingFileTraceListenerAgeUnit", typeof(Resources))]
        [SRCategory("CategoryAgeThreshold", typeof(Resources))]
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
        [SRDescription("RollingFileTraceListenerSizeThreshold", typeof(Resources))]
        [SRCategory("CategorySizeThreshold", typeof(Resources))]
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
        [SRDescription("RollingFileTraceListenerSizeUnit", typeof(Resources))]
        [SRCategory("CategorySizeThreshold", typeof(Resources))]
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
        [SRDescription("RollingFileTraceListenerMaximumLogFilesBeforePurge", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
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
        /// Returns an instance of <see cref="RollingFileTraceListenerData"/> intialized
        /// with default values.
        /// </summary>
        /// <returns>An instance of <see cref="RollingFileTraceListenerData"/> intialized
        /// with default values.</returns>
        private static RollingFileTraceListenerData GetDefaultData()
        {
            RollingFileTraceListenerData data = new RollingFileTraceListenerData(
                Resources.RollingFileTraceListenerNode,
                DefaultValues.RollingFileTraceListenerFileName,
                DefaultValues.RollingFileTraceListenerHeader,
                DefaultValues.RollingFileTraceListenerFooter,
                String.Empty,
                TraceOptions.None,
                DefaultValues.RollingFileTraceListenerTimestampFormat,
                DefaultValues.RollingFileTraceListenerAgeThreshold,
                DefaultValues.RollingFileTraceListenerAgeUnit,
                DefaultValues.RollingFileTraceListenerSizeThreshold,
                DefaultValues.RollingFileTraceListenerSizeUnit,
                DefaultValues.RollingFileTraceListenerMaximumNumberOfLogs);

            return data;
        }
        #endregion
    }
}
