using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Presenters
{
    public class ContactUsPresenter : BasePresenter
    {
        private IContactUsView view;

        public ContactUsPresenter(IContactUsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("View object must not be null");
            }
            this.view = view;
        }

        public void SaveDetails()
        {
            // TODO:
        }

        public void GetContactInformation(string country)
        {
            /* TODO: replace below with following when have service
            return service.getcontent("country", country); etc
            */
            switch (country)
            {
                case "Singapore":
                    view.ContactInformationForDetails = "<b>Singapore</b><br/>" +
                        "<b>Rights Managed & Royalty-Free</b><br/>" +
                        "<b>Corbis</b><br/>" +
                        "19 China Street<br/>" +
                        "#03-04/05 Far East Square<br/>" +
                        "Singapore 049561<br/>" +
                        "Tel: 800.186.0032<br/>" +
                        "Fax: +60 3.6201.3111<br/>" +
                        "E-mail: \"<a href=\"mailto:southasiasales@corbis.com\">southasiasales@corbis.com</a><br/>";
                    break;
                default:
                    view.ContactInformationForDetails = "<b>Rights managed & Royalty-Free</b><br/>" +
                        "Tel.: 1.800.260.0444<br/>" +
                        "Fax Chicago: 312.525.3177<br/>" +
                        "Fax Los Angeles: 323.602.5701<br/>" +
                        "Fax New York: 212.358.9018<br/>" +
                        "Fax Seattle: 206.373.6100";
                    break;
            };
        }
    }
}
