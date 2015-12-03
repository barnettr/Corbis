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
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Registration
{
    public partial class JapaneseAndChineseCultureBusinessAddress : CorbisBaseUserControl, ICultureSpecificAddressView, IViewPropertyValidator
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCountryData();
             
            }
           
        }

        private void LoadCountryData()
        {
            country.DataTextField = "ContentValue";
            country.DataValueField = "Key";
            country.BindDataWithActions(this.PagePresenter.GetCountries());
            country.SelectedIndex = 0;

        }


        #region ICultureSpecificView Implementation
        private RegisterPresenter pagePresenter;
        public RegisterPresenter PagePresenter
        {
            get { return pagePresenter; }
            set { pagePresenter = value; }
        }

        [PropertyControlMapper("address1")]
        public string Address1
        {
            get { return address1.Text; }
        }

        [PropertyControlMapper("address2")]
        public string Address2
        {
            get { return address2.Text; }
        }

        [PropertyControlMapper("address3")]
        public string Address3
        {
            get { return address3.Text; }
        }

        [PropertyControlMapper("city")]
        public string City
        {
            get { return city.Text; }
        }

        [PropertyControlMapper("zip")]
        public string Zip
        {
            get { return zip.Text; }
            set { zip.Text = value; }
        }

        [PropertyControlMapper("state")]
        public string State
        {
            get { return state.SelectedValue; }
        }

        [PropertyControlMapper("country")]
        public string Country
        {
            get { return country.SelectedValue; }
        }


        #endregion
    }
}