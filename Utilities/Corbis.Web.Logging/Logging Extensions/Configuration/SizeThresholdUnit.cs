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

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration
{
    /// <summary>
    /// File size unit for the <see cref="RollingFileTraceListenerData.SizeThreshold"/> property.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum SizeThresholdUnit
	{
        /// <summary>
        /// No size limit.
        /// </summary>
        None = 0,

		/// <summary>
		/// Kilobytes (KB).
		/// </summary>
		Kilobytes = 1024,

		/// <summary>
		/// Megabyte (MB).
		/// </summary>
		Megabytes = 1048576,

		/// <summary>
		/// Gigabytes (GB).
		/// </summary>
		Gigabytes = 1073741824,
	}
}
