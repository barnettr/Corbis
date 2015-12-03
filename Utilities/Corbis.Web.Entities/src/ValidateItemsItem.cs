using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    public class ValidateItemsItem
    {
        private Guid guid;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        private string corbisId;

        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

    }
}
