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
using System.Collections;
using System.IO;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners
{
	/// <summary>
	/// Helper class that implements <see cref="IComparer"/> and compares the creation times of files.
	/// </summary>
    internal static class FileComparer
	{
        /// <summary>
        /// Compares the last modified times of two files.
        /// </summary>
        /// <param name="fileName1">The file name (including path) of file 1.</param>
        /// <param name="fileName2">The file name (including path) of file 2.</param>
        /// <returns><c>&lt;0</c>, if file 2 was modified after file 1. <c>=0</c>, if <paramref name="fileName1"/> equals <paramref name="fileName2"/>.
        /// <c>&gt;0</c>, if file 1 was modified after file 2.</returns>
        public static int CompareLastModifiedTimes(string fileName1, string fileName2)
        {
            int result = 0;

            DateTime file1LastModifiedTimeUtc = new FileInfo(fileName1).LastWriteTimeUtc;
            DateTime file2LastModifiedTimeUtc = new FileInfo(fileName2).LastWriteTimeUtc;

            // Last modified time is not entirely accurate. If the difference is smaller
            // than five seconds compare the file name.
            int buffer = 5;
            TimeSpan diff = file1LastModifiedTimeUtc.Subtract(file2LastModifiedTimeUtc);

            if (Math.Abs(diff.TotalSeconds) < buffer)
            {
                result = fileName1.CompareTo(fileName2);
            }
            else
            {
                result = file1LastModifiedTimeUtc.CompareTo(file2LastModifiedTimeUtc);
            }

            return result;
        }
    }
}
