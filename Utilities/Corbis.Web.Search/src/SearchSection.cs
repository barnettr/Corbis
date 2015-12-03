using System;
using System.Configuration;

namespace Corbis.Web.Search
{
    /// <summary>
    /// Summary description for SearchSection
    /// </summary>
    public class SearchSection : ConfigurationSection
    {
        /// <summary>
        /// SearchSection constructor
        /// </summary>
        public SearchSection()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        /// <summary>
        /// Providers property
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get 
            {
                return (ProviderSettingsCollection)this["providers"];
            }
        }

        /// <summary>
        /// DefaultProvider property
        /// </summary>
        [StringValidator(MinLength = 1), ConfigurationProperty("defaultProvider", DefaultValue = "TexisSearchProvider")]
        public string DefaultProvider
        {
            get
            {
                return (string)this["defaultProvider"];
            }
            set
            {
                this["defaultProvider"] = value;
            }
        }
    }
}