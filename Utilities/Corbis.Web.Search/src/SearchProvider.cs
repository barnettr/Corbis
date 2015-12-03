using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using Corbis.Search.Contracts.V1;

namespace Corbis.Web.Search
{
    /// <summary>
    /// Summary description for SearchProvider
    /// </summary>
    public abstract class SearchProvider : ProviderBase
    {
        /// <summary>
        /// SearchProvider constructor
        /// </summary>
        public SearchProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Name property backer variable
        /// </summary>
        protected string name = string.Empty;

        /// <summary>
        /// Name Property
        /// </summary>
        public override string Name
        {
            get { return name; }
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
        public abstract SearchResults Search(NameValueCollection query, int itemsPerPage, int startPosition, string countryCode, string languageCode);
    }
}