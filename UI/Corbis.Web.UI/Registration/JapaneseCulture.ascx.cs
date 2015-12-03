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

namespace Corbis.Web.UI.Registration
{
	public partial class JapaneseCulture : System.Web.UI.UserControl, ICultureSpecificView
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCountryData();
                LoadRegionData();
            }
        }

        private void LoadCountryData()
        {
            country.DataTextField = "ContentValue";
            country.DataValueField = "Key";
            country.BindDataWithActions(this.PagePresenter.GetCountries());
        }

        private void LoadRegionData()
        {
            state.DataTextField = "ContentValue";
            state.DataValueField = "Key";
            //state.BindDataWithActions(this.PagePresenter.GetRegions());
        }

		protected void Country_SelectedIndexChanged(object sender, EventArgs e)
		{
            LoadRegionData();
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

		public string LastName
		{
			get { return lastName.Text; }
		}

		public string FirstName
		{
			get { return firstName.Text; }
		}

		public string CompanyName
		{
            get { return companyName.Text; }
		}

		public string FuriganaFirstName
		{
			get { return furiganaFirstName.Text; }
		}

		public string FuriganaLastName
		{
			get { return furiganaLastName.Text; }
		}

        public bool RegionEnabled
        {
            get { return state.Enabled; }
            set { state.Enabled = value; }
        }

        public bool ZipEnabled
        {
            get { return zip.Enabled; }
            set { zip.Enabled = value; }
        }

		#endregion
	}
}