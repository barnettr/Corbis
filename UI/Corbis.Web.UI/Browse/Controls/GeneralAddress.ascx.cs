using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Registration.ViewInterfaces;
using Corbis.Web.UI.Presenters.Registration;
using Corbis.Web.UI.Properties;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Web.Content;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using System.Globalization;

namespace Corbis.Web.UI.Browse.Controls
{

    public partial class GeneralAddress : System.Web.UI.UserControl, ICultureSpecificAddressView, IRegisterView
    {
        private RegisterPresenter presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                presenter = new RegisterPresenter();
                LoadCountryData();
              
            }

           
        }
        private void LoadCountryData()
        {
            country.DataTextField = "ContentValue";
            country.DataValueField = "Key";
            country.BindDataWithActions(this.presenter.GetCountries());
            country.SelectedIndex = 0;
            
        }

      

        #region ICultureSpecificView Implementation

        private RegisterPresenter pagePresenter;
        public RegisterPresenter PagePresenter
        {
            get { return pagePresenter; }
            set { pagePresenter = value; }
        }

        public string Address1
        {
            get { return address1.Text; }
        }
        public string Address2
        {
            get { return address2.Text; }
        }

        public string Address3
        {
            get { return address2.Text; }
        }

        public string City
        {
            get { return city.Text; }
        }

        public string Zip
        {
            get { return zip.Text; }
            set { zip.Text = value; }
        }

        public string State
        {
            get { return state.SelectedValue; }
        }

        public string Country
        {
            get { return country.SelectedValue; }
        }

       
        #endregion

        #region IRegisterView Members

        public string SecurityQuestion { get { throw new NotImplementedException(); } }
        public string SecurityAnswer { get { throw new NotImplementedException(); } }

        public string NoRegionsText
        {
            get { throw new NotImplementedException(); }
        }

        public string SelectOneText
        {
            get { return Resources.Resource.SelectOne; }
        }

        public string ForeignZipCodeText
        {
            get { throw new NotImplementedException(); }
        }

        public bool Accept
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingAddress1
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingAddress2
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingAddress3
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingCity
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingCountryCode
        {
            get { throw new NotImplementedException(); }
        }

        public string CompanyName
        {
            get { throw new NotImplementedException(); }
        }

        public string ConfirmEmail
        {
            get { throw new NotImplementedException(); }
        }

        public string ConfirmPassword
        {
            get { throw new NotImplementedException(); }
        }

        public string Email
        {
            get { throw new NotImplementedException(); }
        }

        public string EmailFormat
        {
            get { throw new NotImplementedException(); }
        }

        public string FirstName
        {
            get { throw new NotImplementedException(); }
        }

        public string FuriganaFirstName
        {
            get { throw new NotImplementedException(); }
        }

        public string FuriganaLastName
        {
            get { throw new NotImplementedException(); }
        }

        public string JobTitle
        {
            get { throw new NotImplementedException(); }
        }

        public string Language
        {
            get { throw new NotImplementedException(); }
        }

        public string LastName
        {
            get { throw new NotImplementedException(); }
        }

        public string Password
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessPhoneNumber
        {
            get { throw new NotImplementedException(); }
        }

        public bool SendEmail
        {
            get { throw new NotImplementedException(); }
        }

        public bool SendSnailMail
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingRegionCode
        {
            get { throw new NotImplementedException(); }
        }

        public string UserName
        {
            get { throw new NotImplementedException(); }
        }

        public string MailingPostalCode
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

        public bool ShowUnicodeErrorMessage
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

        public bool EmailExistsMessage
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

        public bool UsernameExistsMessage
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

        public string BusinessAddress1
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessAddress2
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessAddress3
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessCity
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessCountryCode
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessRegionCode
        {
            get { throw new NotImplementedException(); }
        }

        public string BusinessPostalCode
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
