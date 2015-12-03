using System;
using System.Configuration;

namespace Corbis.Web.Utilities.CustomConfigurationSettings
{

    /// <summary>
    /// Summary description for ListItemConfigurationSection
    /// </summary>
    public class ListItemConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Returns an ListItemConfiguration instance
        /// </summary>
        public static ListItemConfiguration GetConfig(string configurationName)
        {
            return ConfigurationManager.GetSection(configurationName) as ListItemConfiguration;
        }

        [ConfigurationProperty("Items")]
        public ListItemCollection Items
        {
            get 
            {
                return this["Items"] as ListItemCollection; 
            }
        } 
    }
}
