using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Class used to return alidation errors from Webservice methods
    /// </summary>
    [Serializable]
    public class ScriptServiceValidationError
    {
        public ScriptServiceValidationError() {}

        public ScriptServiceValidationError(
            string clientId,
            string errorMessage,
            bool hightlightRow,
            bool showInSummary)
        {
            ClientId = clientId;
            ErrorMessage = errorMessage;
            HighlightRow = hightlightRow;
            ShowInSummary = showInSummary;
        }

        public string ClientId { get; set; }
        public string ErrorMessage { get; set; }
        public bool HighlightRow { get; set; }
        public bool ShowInSummary { get; set; }

    }
}

