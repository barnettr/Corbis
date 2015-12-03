//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration.Design
{
    /// <summary>
    /// Default values for the configuration tool.
    /// </summary>
    internal static class DefaultValues
    {
        public const string RollingFileTraceListenerFileName = "trace.log";

        public const string RollingFileTraceListenerHeader = "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=";

        public const string RollingFileTraceListenerFooter = "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-";

        public const int RollingFileTraceListenerAgeThreshold = 0;
        
        public const AgeThresholdUnit RollingFileTraceListenerAgeUnit = AgeThresholdUnit.None;

        public const int RollingFileTraceListenerSizeThreshold = 0;
		
        public const SizeThresholdUnit RollingFileTraceListenerSizeUnit = SizeThresholdUnit.None;

        public const int RollingFileTraceListenerMaximumNumberOfLogs = 0;

        public const string RollingFileTraceListenerTimestampFormat = "yyyy-MM-dd HH-mm-ss";
    }
}
