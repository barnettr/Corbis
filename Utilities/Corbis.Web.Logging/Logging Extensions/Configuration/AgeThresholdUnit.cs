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
	/// Time unit for the <see cref="RollingFileTraceListenerData.AgeThreshold"/> property.
	/// </summary>
	public enum AgeThresholdUnit
	{
        /// <summary>
        /// No roll over based on age.
        /// </summary>
        None = 0,

		/// <summary>
		/// Roll over X minute(s) after file creation.
		/// </summary>
		Minutes,

		/// <summary>
        /// Roll over X hour(s) after file creation.
		/// </summary>
		Hours,

		/// <summary>
        /// Roll over X day(s) after file creation.
		/// </summary>
		Days,
		
		/// <summary>
        /// Roll over X week(s) after file creation.
		/// </summary>
		Weeks,
		
		/// <summary>
        /// Roll over X month(s) after file creation.
		/// </summary>
		Months,
	}
}
