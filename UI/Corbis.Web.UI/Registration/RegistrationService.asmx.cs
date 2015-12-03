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
using Corbis.Web.UI.CustomerService.ViewInterfaces;
using Corbis.Web.UI.Presenters.CustomerService;
using System.Web.Script.Services;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Framework.Logging;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Presenters.Registration;


namespace Corbis.Web.UI.Registration
{
    /// <summary>
    /// Summary description for RegistrationService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class RegistrationService : Corbis.Web.UI.src.CorbisWebService
    {
        private RegisterPresenter presenter;

        public RegistrationService()
        {
            presenter = new RegisterPresenter();
            
        }

        [WebMethod]
        public List<ContentItem> GetStates(string country)
        {
            return presenter.GetRegions(country);
        }
    }
}
