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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration;
using System.Globalization;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners
{
    /// <summary>
    /// Helper class that is used internally by the RollingFileTraceListener.
    /// Parses the base filename and generates new filenames for rolling log files.
    /// </summary>
    internal sealed class FileNameBuilder
    {
        #region Private fields
        /// <summary>
        /// Configuration settings.
        /// </summary>
        private RollingFileTraceListenerSettings _settings;

        /// <summary>
        /// A regular expression matching file names with a counter in them.
        /// </summary>
        private static readonly Regex _counterPattern = new Regex(@"\[(\d{7})\]", RegexOptions.Compiled);

        /// <summary>
        /// The directory path for the log files.
        /// </summary>
        private string _path;

        /// <summary>
        /// The extension to use for log files.
        /// </summary>
        private string _extension;

        /// <summary>
        /// The file name (without extension) to use for the current log file.
        /// </summary>
        private string _fileNameWithoutExtension;
        
        /// <summary>
        /// A wild card pattern matching log file names.
        /// </summary>
        private string _fileNamePattern;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="settings">The configuration settings.</param>
        public FileNameBuilder(RollingFileTraceListenerSettings settings)
        {
            _settings = settings;

            _path = Path.GetDirectoryName(_settings.FileName);
            _extension = Path.GetExtension(_settings.FileName);
            _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_settings.FileName);

            _fileNamePattern = _fileNameWithoutExtension + "*" + _extension;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Generates and returns a new unique filename with a timestamp and/or a incremental counter.
        /// </summary>
        /// <returns>Full path of new filename.</returns>
        public string GetNewFilename()
        {
            string suffix = GetSuffix();

            string newFileName = GetFileNameWithSuffix(suffix);

            return MakeFileNameUnique(newFileName, suffix);
        }

        /// <summary>
        /// Returns a list of file names sorted on the creation time of the files (oldest first).
        /// </summary>
        /// <returns>A sorted list of file names.</returns>
        public List<string> GetSortedFiles()
        {
            string[] files = GetFileNamesMatchingPattern();

            List<string> list = new List<string>(files);
            list.Sort(FileComparer.CompareLastModifiedTimes);
            return list;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Returns an array of all file names in the configured directory that match the pattern
        /// of the configured file name.
        /// </summary>
        /// <returns>An array of all file names in the configured directory that match the pattern
        /// of the configured file name.</returns>
        public string[] GetFileNamesMatchingPattern()
        {
            return Directory.GetFiles(_path, _fileNamePattern);
        }
        
        /// <summary>
        /// Returns the suffix to use for creating a unique file name.
        /// </summary>
        /// <returns></returns>
        private string GetSuffix()
        {
            if (!String.IsNullOrEmpty(_settings.TimestampFormat))
            {
                return GetTimestamp();
            }
            else
            {
                return GetUniqueCounterSuffix();
            }
        }

        /// <summary>
        /// Creates a new file name by inserting <paramref name="suffix"/> just before
        /// the extension.
        /// </summary>
        /// <param name="suffix">The suffix to insert into the file name.</param>
        /// <returns>The new file name.</returns>
        private string GetFileNameWithSuffix(string suffix)
        {
            StringBuilder nameBuilder = new StringBuilder(_path);

            nameBuilder.Append(Path.DirectorySeparatorChar);

            nameBuilder.Append(_fileNameWithoutExtension);

            nameBuilder.Append(suffix);

            nameBuilder.Append(_extension);

            return nameBuilder.ToString();
        }

        /// <summary>
        /// Ensures that no file with the <paramref name="fileName"/> exists yet.
        /// If a file with the specified name already exists, it generates 
        /// a new file name by appending a counter value.
        /// </summary>
        /// <param name="fileName">The file name that should be checked.</param>
        /// <param name="suffix">The suffix to use.</param>
        /// <returns><paramref name="fileName"/> if no file with this name exists yet. Or
        /// a new file name with a counter value inserted.</returns>
        private string MakeFileNameUnique(string fileName, string suffix)
        {
            if (File.Exists(fileName))
            {
                suffix += GetUniqueCounterSuffix();
                return GetFileNameWithSuffix(suffix);
            }
            else
            {
                return fileName;
            }
        }

        /// <summary>
        /// Creates a unique suffix based on a counter value. It looks at the files already present
        /// in the configured directory to determine a value that is not already in use. 
        /// </summary>
        private string GetUniqueCounterSuffix()
        {
            int highestUsedCounter = GetHighestUsedCounter() + 1;

            return highestUsedCounter.ToString("[000000#]", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the highest counter value currently used in file names matching
        /// the file name pattern in the configured directory.
        /// </summary>
        /// <returns>The highest counter value currently used. <c>0</c> if no
        /// counter value has been used.</returns>
        private int GetHighestUsedCounter()
        {
            int highestUsedCounter = 0;
            string[] fileNames = GetFileNamesMatchingPattern();
            for (int i = 0; i < fileNames.Length; i++)
            {
                string fileName = fileNames[i];
                Match match = _counterPattern.Match(fileName);

                if (match.Success)
                {
                    string counterString = match.Groups[1].Value;
                    int counter = Convert.ToInt32(counterString, CultureInfo.InvariantCulture);
                    if (counter > highestUsedCounter)
                    {
                        highestUsedCounter = counter;
                    }
                }
            }

            return highestUsedCounter;
        }

        /// <summary>
        /// Returns the current date time formatted using the configured format specifier.
        /// </summary>
        /// <returns>The current date time formatted using the configured format specifier.</returns>
        private string GetTimestamp()
        {
            return DateTime.Now.ToString(_settings.TimestampFormat, CultureInfo.InvariantCulture);
        }
        #endregion

    }
}
