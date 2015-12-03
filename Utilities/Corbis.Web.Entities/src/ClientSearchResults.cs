using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.Entities
{
    public class ClientSearchResults
    {
        #region Private Variables
        private List<Clarification> _clarifications;
        private FilterCount _filterCounts;
        private int _totalRecords;
        private List<SearchResultProduct> _searchResultProducts;

        #endregion

        #region Constructor

        public ClientSearchResults(
            List<Clarification> clarifications,
            FilterCount filterCounts,
            int totalRecords,
            List<SearchResultProduct> searchResultProducts)
        {
            _clarifications = clarifications;
            _filterCounts = filterCounts;
            _totalRecords = totalRecords;
            _searchResultProducts = searchResultProducts;
        }

        public ClientSearchResults()
        {
            _clarifications = new List<Clarification>();
            _filterCounts = new FilterCount();
            _totalRecords = 0;
            _searchResultProducts = new List<SearchResultProduct>();

        }

        #endregion 

        #region Public Properties

        public List<Clarification> Clarifications 
        {
            get
            {
                return _clarifications;
            }
        }

        public FilterCount FilterCounts
        {
            get
            {
                return _filterCounts;
            }
        }

        public int TotalRecords
        {
            get
            {
                return _totalRecords;
            }
        }

        public List<SearchResultProduct> SearchResultProducts 
        {
            get
            {
                return _searchResultProducts;
            }
        }

        #endregion


    }
}
