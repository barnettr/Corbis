using System;
using System.Collections.Specialized;
using Corbis.Search.Contracts.V1;

namespace Corbis.Web.Search
{
    /// <summary>
    /// Summary description for FastESPProvider
    /// </summary>
    public class FastEspProvider : SearchProvider
    {
        /// <summary>
        /// FastESPProvider constructor
        /// </summary>
        public FastEspProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Initialize provider
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="config">System.Collections.Specialized.NameValueCollection</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("FastEspProvider: Initialize() - config");
            }
            base.name = string.IsNullOrEmpty(name) ? "FaseEspProvider" : name;
            base.Initialize(name, config);
            config.Remove("name");
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="query">The NameValueCollection from the QueryString.</param>
        /// <param name="itemsPerPage">Number of items to return.</param>
        /// <param name="startPosition">Current page of items to return.</param>
        /// <param name="countryCode">Country code of the user.</param>
        /// <param name="languageCode">Language code of the user.</param>
        /// <returns>Search results</returns>
        public override SearchResults Search(NameValueCollection query, int itemsPerPage, int startPosition, string countryCode, string languageCode)
        {
 	        throw new Exception("The method or operation is not implemented.");
        }
    }
}