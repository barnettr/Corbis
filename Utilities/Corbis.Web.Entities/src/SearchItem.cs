using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    [Serializable]
    public class SearchItem
    {
        private string corbisId;
        private Guid mediaUid;
        private Guid realProductUid;
        public string CorbisID
        {
            get { return corbisId; }
            set { corbisId = value; }
        }
        public Guid MediaUID
        {
            get { return mediaUid; }
            set { mediaUid = value; }
        }
        public Guid RealProductUid
        {
            get { return realProductUid; }
            set { realProductUid = value; }
        }
    }
}
