using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Common.ViewInterfaces;
using Corbis.Web.UI.Presenters.Common;
using System.Web.Script.Services;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Framework.Logging;
using Corbis.Web.Utilities;
using Resources;
using Corbis.Web.UI.src;

namespace Corbis.Web.UI.Common
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class CommonWebService : CorbisWebService, ICountriesAndStatesView
    {
        private CommonPresenter presenter;
        
        public CommonWebService()
        {
            presenter = new CommonPresenter();
            presenter.CountriesAndStatesView = this;
        }


        [WebMethod(true)]
        public List<ContentItem> GetStates(string country)
        {
            this.CountryCode = country;
            presenter.GetStates();

            return this.StateList;
        }

        [WebMethod]
        public List<ContentItem> GetCountries()
        {
            presenter.GetCountries();

            return this.CountryList;
        }

        #region ICommonView Members

        public string SelectOne
        {
            get { return Resource.SelectOne; }
        }

        public string Dashes
        {
            get { return Resource.Dashes; }
        }
        
        private string countryCode;
        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = value;
            }
        }

        private List<ContentItem> countryList;
        public List<ContentItem> CountryList
        {
            get
            {
                return this.countryList;
            }
            set
            {
                this.countryList = value;
            }
        }

        private List<ContentItem> stateList;
        public List<ContentItem> StateList
        {
            get
            {
                return this.stateList;
            }
            set
            {
                this.stateList = value;
            }
        }

        #endregion

        #region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
