using System;
using System.Configuration;

namespace Corbis.Web.Utilities.CustomConfigurationSettings
{
    /// <summary>
    /// Summary description for ListItemCollection
    /// </summary>
    public class ListItemCollection : ConfigurationElementCollection
    {
        
        public ListItem this[int index]
        {
            get
            {
                return base.BaseGet(index) as ListItem;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public void Add(ListItem item)
        {
            BaseAdd(item);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ListItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ListItem)element).Value;
        }
    }

}
