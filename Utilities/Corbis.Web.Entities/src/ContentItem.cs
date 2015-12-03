using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    [Serializable]
    public class ContentItem
    {
        private string key;

        /// <summary>
        /// Initializes a new instance of the ContentItem class.
        /// </summary>
        public ContentItem()
        {
        }

        public ContentItem(string itemKey, string value)
        {

            key = itemKey;
            contentValue = value;
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private string contentValue;

        public string ContentValue
        {
            get { return contentValue; }
            set
            {
                contentValue = value;
            }
        }
    }
}
