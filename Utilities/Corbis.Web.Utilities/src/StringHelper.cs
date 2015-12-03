using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Corbis.Framework.Globalization;

namespace Corbis.Web.Utilities
{
    public static class StringHelper
    {
        public static string GetEmptyIfNull(IDictionary<string, string> collection, string key)
        {
            return collection.ContainsKey(key) ? collection[key] : string.Empty;
        }

        public static string GetEmptyIfNull(string evalValue)
        {
            return evalValue ?? string.Empty;
        }

        public static string GetDefaultIfNull(string evalValue, string defaultValue)
        {
            return IsNullOrTrimEmpty(evalValue) ? defaultValue : evalValue;
        }

        /// <summary>
        /// Checks if a supplied text is NULL or Empty.
        /// The supplied text is trimmed(checking for blank spaces) before checking if its empty or not.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if Null or Trim Empty, false otherwise</returns>
        public static bool IsNullOrTrimEmpty(string text)
        {
            bool result = false;

            //check if null
            if (text == null)
            {
                result = true;
            }
            else
            {
                //trim the text before check
                if (text.Trim().Length == 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static string GetColumnNameFromExpression(string sortExpression)
        {
            if (String.IsNullOrEmpty(sortExpression))
                return String.Empty;
            string[] data = sortExpression.Split(' ');
            return data[0];
        }
        public static ListSortDirection GetSortDirectionFromExpression(string sortExpression)
        {
            ListSortDirection direction = ListSortDirection.Ascending;

            if (String.IsNullOrEmpty(sortExpression))
                return direction;

            string[] data = sortExpression.Split(' ');
            if (data.Length == 2)
            {
                if (String.Compare(data[1], "desc", true) != 0)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    direction = ListSortDirection.Descending;
                }
            }
            return direction;
        }
        public static string SetSortExpression(string columnName, ListSortDirection direction)
        {
            if (String.IsNullOrEmpty(columnName))
                return String.Empty;

            if (direction == ListSortDirection.Ascending)
            {
                return columnName + " DESC";
            }
            else
            {
                return columnName + " ASC";
            }
        }

        public static string Truncate(string strText, int intCharCount)
        {
            string strReturn = string.Empty;
            if (strText != null)
            {
                if (strText.Length > intCharCount)
                {
                    strReturn = strText.Substring(0, intCharCount) + "...";
                }
                else
                {
                    strReturn = strText;
                }
            }
            return strReturn;
        }

        public static string ConvertBracketsToItalic(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, @"<([^<>]*)>", @"<i>$1</i>");
            }
            return text;
        }

        /// <summary>
        /// Gets the filesize for display.
        /// </summary>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <returns></returns>
        public static string GetFilesizeForDisplay(int sizeInBytes)
		{
			if (sizeInBytes > 1048576)
			{
                return (sizeInBytes / 1048576).ToString("0.0", Language.CurrentCulture) + " MB";
			}
			else
			{
                return (sizeInBytes / 1024).ToString("0", Language.CurrentCulture) + " KB";
			}
		}

        /// <summary>
        /// Encodes a .NET string to be used as a javascript string variable
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>
        /// The encoded string
        /// </returns>
        /// <remarks>
        /// Currently only escaping backslashes and Apostrophes (char 39).
        /// Add more functionality as needed
        /// </remarks>
        public static string EncodeToJsString(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Replace(@"\", @"\\");
                s = s.Replace("'", @"\'");
                s = s.Trim();
                s = s.Replace("\n", "\\n");
                s = s.Replace("\r", "\\r");
                return s;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Replace multiple whitespace characters (space, cr, lf, tab, etc.) with 
        /// single space character.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(string value)
        {
            string trimValue = value == null ? string.Empty : value.Trim();
            return Regex.Replace(trimValue, @"\s+", " ").Trim();
        }
        public static string RemoveFirstWord(string initialString)
        {
            return initialString.Substring(initialString.IndexOf(' ') + 1);
        }
    }

    // TODO: Put this enum in a separate code file
    /// <summary>
    /// Used to Get a sort direction without having to reference Web libraries
    /// </summary>
    public enum ListSortDirection
    {
        /// <summary>
        /// Used for Ascending Sorts
        /// </summary>
        Ascending,
        /// <summary>
        /// Descending Sorts
        /// </summary>
        Descending
    }
   
}
