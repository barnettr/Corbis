using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Class used to determine why properties on a view are invalid.
    /// </summary>
    /// <typeparam name="T">
    /// The type of Enum that provides the reson it's invalid and the 
    /// error messaging
    /// </typeparam>
    public class ViewValidationError<T>
    {
        public ViewValidationError(
            string invalidPropertyName, 
            T invalidReason,
            bool hightlightRow,
            bool showInSummary)
        {
            InvalidPropertyName = invalidPropertyName;
            InvalidReason = invalidReason;
            HighlightRow = hightlightRow;
            ShowInSummary = showInSummary;
        }

        public string InvalidPropertyName { get; set; }
        public T InvalidReason { get; set; }
        public bool HighlightRow { get; set; }
        public bool ShowInSummary { get; set; }

    }
}

