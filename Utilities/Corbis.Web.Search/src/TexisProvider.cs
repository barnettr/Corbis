using System;
using System.Collections.Specialized;
using System.Configuration;
using Corbis.Search.Contracts.V1;
using Corbis.Search.Contracts.V1.Constants;
using Corbis.Search.ServiceAgents.V1;

namespace Corbis.Web.Search
{
    /// <summary>
    /// Summary description for TexisProvider
    /// </summary>
    public class TexisProvider : SearchProvider
    {
        SearchServiceAgent searchServiceAgent;

        /// <summary>
        /// TexisProvider constructor
        /// </summary>
        public TexisProvider()
        {
            searchServiceAgent = new SearchServiceAgent();
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
                throw new ArgumentNullException("TexisProvider: Initialize() - config");
            }
            base.name = string.IsNullOrEmpty(name) ? "TexisProvider" : name;
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
            SearchRequest request = BuildRequest(query, itemsPerPage, startPosition, countryCode, languageCode);
            return searchServiceAgent.Search(request);
        }

        /// <summary>
        /// Create the search request
        /// </summary>
        /// <param name="query"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="startPosition"></param>
        /// <param name="countryCode"></param>
        /// <param name="languageCode"></param>
        /// <returns>Search request.</returns>
        private SearchRequest BuildRequest(NameValueCollection query, int itemsPerPage, int startPosition, string countryCode, string languageCode)
        {
            SearchRequest request = new SearchRequest();
            request.SearchFilters = new System.Collections.Generic.List<SearchFilter>();
            request.CountryCode = countryCode;
            request.LanguageCode = languageCode;
            request.ItemsPerPage = itemsPerPage;
            request.StartPosition = startPosition;

            NameValueCollection collection = new NameValueCollection(query.Count);

            string[] keys = query.AllKeys;
            for (int i = 0; i < keys.Length; ++i)
            {
                string[] values = query.GetValues(i);
                if (values.Length > 0)
                {
                    switch (keys[i])
                    {
                        case SearchFilterKeys.KEYWORD:
                            request.SearchFilters.Add(new KeywordFilter(values[0]));
                            break;
                    }
                }
            }

            return request;
        }
    }
}