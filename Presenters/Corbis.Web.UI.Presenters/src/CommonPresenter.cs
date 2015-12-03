using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Web.Entities;
using Corbis.Web.Content;
using Corbis.Web.UI.Common.ViewInterfaces;

namespace Corbis.Web.UI.Presenters.Common
{
    public class CommonPresenter : BasePresenter
    {
        private RegionsContentProvider regionsContentProvider;
        private CountriesContentProvider countriesContentProvider;

        public CommonPresenter()
        {
            
        }

        private ICountriesAndStatesView countriesAndStatesView;
        public ICountriesAndStatesView CountriesAndStatesView
        {
            get
            {
                return countriesAndStatesView;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("View object must not be null");
                }
                countriesAndStatesView = value;
            }
        }

        public void GetStates()
        {
            try
            {
                List<ContentItem> states = new List<ContentItem>();

                string countryCode = CountriesAndStatesView.CountryCode;

                if (!string.IsNullOrEmpty(countryCode))
                {
                    regionsContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Region) as RegionsContentProvider;
                    states = regionsContentProvider.GetRegionsByCountryCode(countryCode);
                    if (states.Count == 0)
                    {
                        states.Insert(0, new ContentItem("dash", CountriesAndStatesView.Dashes));
                    }
                    else
                    {
                        states.Insert(0, new ContentItem("selectOne", CountriesAndStatesView.SelectOne));
                    }

                }
                else
                {
                    states.Insert(0, new ContentItem(String.Empty, CountriesAndStatesView.Dashes));
                }

                CountriesAndStatesView.StateList = states;
            }
            catch (Exception ex)
            {
                HandleException(ex, CountriesAndStatesView.LoggingContext, "CommonPresenter: GetStatesFromWebService()");
                throw;
            }
        }

        public void GetCountries()
        {
            try
            {
                List<ContentItem> countries = new List<ContentItem>();

                countriesContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
                countries = countriesContentProvider.GetCountries();

                countries.Insert(0, new ContentItem(String.Empty, CountriesAndStatesView.SelectOne));

                CountriesAndStatesView.CountryList = countries;
            }
            catch (Exception ex)
            {
                HandleException(ex, CountriesAndStatesView.LoggingContext, "CommonPresenter: GetCountries()");
                throw;
            }
        }
    }
}
