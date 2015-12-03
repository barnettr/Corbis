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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="RollingFileTraceListener"/>.
    /// </summary>
    [Assembler(typeof(RollingFileTraceListenerAssembler))]
    public sealed class RollingFileTraceListenerData: FlatFileTraceListenerData
    {
        #region Internal Constants
        /// <summary>
        /// The name of the timestampFormat configuration property.
        /// </summary>
        internal const string TimestampFormatProperty = "timestampFormat";

        /// <summary>
        /// The name of the maximumNumberOfLogs configuration property.
        /// </summary>
        internal const string MaxNumberOfLogsProperty = "maximumNumberOfLogs";

        /// <summary>
        /// The name of the sizeThreshold configuration property.
        /// </summary>
        internal const string SizeThresholdProperty = "sizeThreshold";

        /// <summary>
        /// The name of the ageThreshold configuration property.
        /// </summary>
        internal const string AgeThresholdProperty = "ageThreshold";

        /// <summary>
        /// The name of the sizeUnit configuration property.
        /// </summary>
        internal const string SizeUnitProperty = "sizeUnit";

        /// <summary>
        /// The name of the ageUnit configuration property.
        /// </summary>
        internal const string AgeUnitProperty = "ageUnit";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="RollingFileTraceListenerData"/>.
        /// </summary>
        public RollingFileTraceListenerData()
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="RollingFileTraceListenerData"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="timestampFormat">The format string for the time stamp in a rolled-over log file name.</param>
        /// <param name="ageThreshold">The numeric value representing the file age threshold in the configured units.</param>
        /// <param name="ageUnit">The age unit in which the value of <paramref name="ageThreshold"/> is configured.</param>
        /// <param name="sizeThreshold">The numeric value representing the file size threshold
        /// in the configured units.</param>
        /// <param name="sizeUnit">he size unit in which the value of <paramref name="sizeThreshold"/> is configured.</param>
        /// <param name="maximumLogFilesBeforePurge">The maximum number of logfiles allowed before the oldest ones should be purged.</param>
        public RollingFileTraceListenerData(string name, string fileName, 
            string header, string footer, string formatterName, 
            TraceOptions traceOutputOptions,
            string timestampFormat, int ageThreshold, AgeThresholdUnit ageUnit, int sizeThreshold, SizeThresholdUnit sizeUnit, 
            int maximumLogFilesBeforePurge)
            : base(name, fileName, header, footer, formatterName, traceOutputOptions)
        {
            this.TimestampFormat = timestampFormat;
            this.AgeThreshold = ageThreshold;
            this.AgeUnit = ageUnit;
            this.SizeThreshold = sizeThreshold;
            this.SizeUnit = sizeUnit;
            this.MaximumLogFilesBeforePurge = maximumLogFilesBeforePurge;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the format string for the time stamp in a rolled-over log file name.
        /// </summary>
        /// <remarks>If the value is the empty string, no time stamp will be put in a rolled-over
        /// log file. A counter value will be used instead.</remarks>
        [ConfigurationProperty(TimestampFormatProperty, IsRequired = false, DefaultValue = "")]
        public string TimestampFormat
        {
            get
            {
                return (string) base[TimestampFormatProperty];
            }
            set
            {
                base[TimestampFormatProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric value representing the file age threshold in the configured units.
        /// </summary>
        /// <remarks>A value of <c>0</c> means no roll over should take place based on the age
        /// of the current log file.</remarks>
        [ConfigurationProperty(AgeThresholdProperty, IsRequired = false, DefaultValue = 0)]
        [IntegerValidator(MinValue = 0)]
        public int AgeThreshold
        {
            get
            {
                return (int) base[AgeThresholdProperty];
            }
            set
            {
                base[AgeThresholdProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the age unit in which the value of the <see cref="AgeThreshold"/> property 
        /// is configured.
        /// </summary>
        /// <remarks>A value of <see cref="AgeThresholdUnit.None"/> means
        /// no roll over should take place based on the age
        /// of the current log file.</remarks>
        [ConfigurationProperty(AgeUnitProperty, IsRequired = false, DefaultValue = AgeThresholdUnit.None)]
        public AgeThresholdUnit AgeUnit
        {
            get
            {
                return (AgeThresholdUnit) base[AgeUnitProperty];
            }
            set
            {
                base[AgeUnitProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the numeric value representing the file size threshold
        /// in the configured units.
        /// </summary>
        /// <remarks>A value of <c>0</c> means no roll over should take place based on the size
        /// of the current log file.</remarks>
        [ConfigurationProperty(SizeThresholdProperty, IsRequired = false, DefaultValue = 0)]
        [IntegerValidator(MinValue = 0)]
        public int SizeThreshold
        {
            get
            {
                return (int) base[SizeThresholdProperty];
            }
            set
            {
                base[SizeThresholdProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the size unit in which the value of the <see cref="SizeThreshold"/>
        /// property is configured.
        /// </summary>
        /// <remarks>A value of <see cref="SizeThresholdUnit.None"/> means
        /// no roll over should take place based on the size
        /// of the current log file.</remarks>
        [ConfigurationProperty(SizeUnitProperty, IsRequired = false, DefaultValue = SizeThresholdUnit.None)]
        public SizeThresholdUnit SizeUnit
        {
            get
            {
                return (SizeThresholdUnit) base[SizeUnitProperty];
            }
            set
            {
                base[SizeUnitProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of logfiles allowed before the oldest ones should be purged.
        /// </summary>
        /// <remarks>A value of <c>0</c> means no purging should take place, so then all logfiles are kept.</remarks>
        [ConfigurationProperty(MaxNumberOfLogsProperty, IsRequired = false, DefaultValue = 0)]
        [IntegerValidator(MinValue = 0, MaxValue = 1000)]
        public int MaximumLogFilesBeforePurge
        {
            get
            {
                return (int) base[MaxNumberOfLogsProperty];
            }
            set
            {
                base[MaxNumberOfLogsProperty] = value;
            }
        }
        #endregion
    }

}
