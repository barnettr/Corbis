using System;
using System.Configuration;

namespace Corbis.Web.Utilities.CustomConfigurationSettings
{
    /// <summary>
    /// Summary description for ListItem
    /// </summary>
    public class ListItem : ConfigurationElement
    {
        
        
        [ConfigurationProperty("Text", IsRequired = true)]
        public string Text
        {
            get
            {
                return this["Text"] as string;
            }
            set
            {
                this["Text"] = value;
            }


        }


        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["Value"] as string;
            }
            set
            {
                this["Value"] = value;
            }
        }
    }

}
