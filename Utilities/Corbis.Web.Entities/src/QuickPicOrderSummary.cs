using System;
using System.Collections.Generic;
using System.Text;
using Corbis.MediaDownload.Contracts.V1;

namespace Corbis.Web.Entities
{   [Serializable]
    public class QuickPicOrderSummary
    {
        public string OrderNumber { get; set; }
        public string ProjectName { get; set; }
        public string ConfirmationEmail { get; set; }
        public List<string> PackagedCorbisIds { get; set; }
        public int FailedCount { get; set; }
        public List<KeyValuePair<string, string>> DownloadPackages { get; set; }
    }
}
