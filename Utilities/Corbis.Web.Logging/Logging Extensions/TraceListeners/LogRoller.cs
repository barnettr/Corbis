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
using System.Diagnostics;

using LogicaCMG.EnterpriseLibraryExtensions.Logging.Configuration;
using System.Text;

namespace LogicaCMG.EnterpriseLibraryExtensions.Logging.TraceListeners
{
    /// <summary>
    /// Helper class that is used internally by <see cref="RollingFileTraceListener"/> to evaluate rollover thresholds
    /// and to rename a log file that has exceeded thresholds.
    /// </summary>
    internal sealed class LogRoller
    {
        #region Private constants
        /// <summary>
        /// The buffer size to use for writing to log files.
        /// </summary>
        private const int _LogFileBufferSize = 0x1000;
        #endregion

        #region Private fields
        /// <summary>
        /// The configuration settings.
        /// </summary>
        private RollingFileTraceListenerSettings _settings;
        
        /// <summary>
        /// The trace listener that writes to the configured log file.
        /// </summary>
        private TextWriterTraceListener _traceListener;

        /// <summary>
        /// Information about the current log file.
        /// </summary>
        private FileInfo _currentLogFileInfo;

        /// <summary>
        /// Helper to construct file names of log files.
        /// </summary>
        private FileNameBuilder _fileNameBuilder;

        /// <summary>
        /// The maximum number of bytes for the size of a file. If it is larger, it is rolled over 
        /// the next time it is written to. A value of <c>0</c> indicates that the size is not
        /// relevant for roll over.
        /// </summary>
        private long _rolloverSizeThresholdInBytes;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="settings">Rolling File Sink configuration settings.</param>
        /// <param name="traceListener">The <see cref="TextWriterTraceListener"/> instance
        /// for which the file to which it writes, should be rolled over.</param>
        public LogRoller(RollingFileTraceListenerSettings settings, TextWriterTraceListener traceListener)
        {
            _settings = settings;
            _traceListener = traceListener;

            _fileNameBuilder = new FileNameBuilder(_settings);

            _rolloverSizeThresholdInBytes = _settings.SizeThreshold * ((int)_settings.SizeUnit);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Rolls over the log file if one or more thresholds are exceeded.
        /// </summary>
        public void RolloverIfNecessary()
        {
            _currentLogFileInfo = new FileInfo(_settings.FileName);
            if (AreThresholdsExceeded())
            {
                PerformRenameRollover();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Evaluate the age and size threshold.
        /// </summary>
        /// <returns>Return true if the file has exceeded the thresholds.</returns>
        private bool AreThresholdsExceeded()
        {
            if (!_currentLogFileInfo.Exists)
                return false;

            return CheckExceededByteThreshold() || CheckExceededAgeThreshold();
        }
        
        /// <summary>
        /// Archive the current log file by renaming it with today's timestamp.
        /// Generate a new filename for the current log file using a <see cref="FileNameBuilder"/>.
        /// </summary>
        private void PerformRenameRollover()
        {
            Purge();
            if ( _currentLogFileInfo.Exists )
            {
                string archiveFileName = _fileNameBuilder.GetNewFilename();
                
                _traceListener.Writer.Close();

                File.Move(_currentLogFileInfo.FullName, archiveFileName);

                _traceListener.Writer = GetNewLogFileWriter();
            }
        }

        /// <summary>
        /// Creates a new current log file and explicitly sets its creation <see cref="DateTime"/> to <see cref="DateTime.Now"/>.
        /// </summary>
        /// <remarks>Explicitly creating the new file and explicitly setting its creation <see cref="DateTime"/> is
        /// necessary due to file system tunneling. Due the file system tunneling a new file will get the creation <see cref="DateTime"/>
        /// of an older file that existed with the same name but that was deleted or renamed within 15 seconds of
        /// the creation operation. Unfortunately, this means that the file has to be opened and closed a couple of times. That shouldn't
        /// be a problem because the roll over occurs rarely.</remarks>
        private TextWriter GetNewLogFileWriter()
        {
            // Create the file and close it.
            FileStream newLogFileStream = _currentLogFileInfo.Create();
            newLogFileStream.Close();

            // This also opens and closes the file.
            _currentLogFileInfo.CreationTime = DateTime.Now;

            // And then we open it again.
            return new StreamWriter(_currentLogFileInfo.FullName, true, Encoding.UTF8, _LogFileBufferSize);
        }

        /// <summary>
        /// Evaluate the file size threshold.
        /// </summary>
        /// <returns>Returns true if the file has grown larger than the AgeThreshold.</returns>
        private bool CheckExceededByteThreshold()
        {
            bool exceed = false;

            if (_rolloverSizeThresholdInBytes > 0)
            {
                if (_currentLogFileInfo.Length >= _rolloverSizeThresholdInBytes)
                {
                    exceed = true;
                }
            }
            return exceed;
        }

        /// <summary>
        /// Evaluate the age threshold by comparing the file's creation date against today.
        /// </summary>
        /// <returns>Returns true if the file has grown larger than the AgeThreshold.</returns>
        private bool CheckExceededAgeThreshold()
        {
            bool exceed = false;
            if (_settings.AgeThreshold > 0)
            {
                double elapsedValue = GetElapsedAgeValue();

                if (elapsedValue >= _settings.AgeThreshold)
                {
                    exceed = true;
                }
            }

            return exceed;
        }

        /// <summary>
        /// Returns the age of the current log file in the configured age units (e.g., in days).
        /// </summary>
        /// <returns></returns>
        private double GetElapsedAgeValue()
        {
            TimeSpan age = DateTime.Now.Subtract(_currentLogFileInfo.CreationTime);

            double elapsedValue = 0;
            switch (_settings.AgeUnit)
            {
                case (AgeThresholdUnit.Minutes):
                    elapsedValue = age.TotalMinutes;
                    break;
                case (AgeThresholdUnit.Hours):
                    elapsedValue = age.TotalHours;
                    break;
                case (AgeThresholdUnit.Days):
                    elapsedValue = age.TotalDays;
                    break;
                case (AgeThresholdUnit.Weeks):
                    elapsedValue = age.TotalDays / 7;
                    break;
                case (AgeThresholdUnit.Months):
                    elapsedValue = age.TotalDays / 30;
                    break;
                default:
                    break;
            }
            return elapsedValue;
        }

        /// <summary>
        /// Purges old log files if purging is enabled.
        /// </summary>
        private void Purge()
        {
            if (_settings.MaximumLogFilesBeforePurge > 0)
            {
                List<string> sortedFiles = _fileNameBuilder.GetSortedFiles();
                DeleteFiles(sortedFiles);
            }
        }

        /// <summary>
        /// Deletes files using the specified list of file names. It starts at the start of the list and continues
        /// until the number of files left no longer exceeds the maximum number of log files.
        /// </summary>
        /// <param name="sortedFiles">A sorted list of file names (oldest files first).</param>
        /// <remarks>The current log file is not deleted.</remarks>
        private void DeleteFiles(List<string> sortedFiles)
        {
            int numberOfFilesToDelete = sortedFiles.Count - _settings.MaximumLogFilesBeforePurge;
            
            int numberOfFilesDeleted = 0;
            for (int index = 0; index < sortedFiles.Count && numberOfFilesDeleted < numberOfFilesToDelete; index++)
            {
                string fileName = sortedFiles[index];

                // Do not delete the current log file.
                if (fileName == _settings.FileName) 
                {
                    continue;
                }

                File.Delete(fileName);
                numberOfFilesDeleted++;
            }
        }
        #endregion
    }
}
