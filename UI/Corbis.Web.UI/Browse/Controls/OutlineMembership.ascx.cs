using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

using Resources;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.CustomerService;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Browse
{
	public partial class OutlineMembership : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				LoadCountryData();
				PopulateOfficeDropdown();
			}
		}

        protected void CancelEmail(object sender, EventArgs e) 
        {
            this.CompanyName.Text = this.Telephone.Text = this.Name.Text = this.EmailAddress.Text = string.Empty;
            this.country.SelectedValue = "US";

        }

        protected void SendOutlineEmail(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Response.IsClientConnected)
            {
                return;
            }

            string bodyText = "";
            string mailRecipient = SelectedCountry.Value;
            //string mailRecipient = "sunny.zhang@corbis.com";

            bodyText = "Name " + this.Name.Text
                +"\r\n" + "Company: " + this.CompanyName.Text
                +"\r\n" + "Country/Region: " + this.country.SelectedValue.ToString()
                +"\r\n" + "Phone: " + this.Telephone.Text
                +"\r\n" + "Email Address: " + this.EmailAddress.Text;

          
            MailAddress addressTo = new MailAddress(mailRecipient);
            MailAddress addressFrom = new MailAddress(EmailAddress.Text);
            MailMessage mail = new MailMessage(addressFrom, addressTo);

            mail.Subject = "Apply for Outline MemberShip";
            mail.Priority = MailPriority.High;
            mail.Body = bodyText;

            SmtpClient smtp = new SmtpClient(Environment.MachineName);
            smtp.Send(mail);
		}

		public List<ContentItem> GetCountries()
        {
            List<ContentItem> list = null;
            try
            {
                Corbis.Web.Content.CountriesContentProvider countries = Corbis.Web.Content.ContentProviderFactory.CreateProvider(ContentItems.Country) as Corbis.Web.Content.CountriesContentProvider;
                list = countries.GetCountries();
                list.Insert(0, new ContentItem(String.Empty, GetLocalResourceObject("SelectOne").ToString()));                
            }
            catch (Exception ex)
            {
                // Nothing to do but write out the error message
                Response.Write(ex.Message);
            }
            return list;
		}

		protected virtual void ContactInformation_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (e.Item.DataItem is DisplayValue<OfficeCountry>)
				{
					Literal officeLocation = (Literal)e.Item.FindControl("OfficeLocation");
					DisplayValue<OfficeCountry> data = (DisplayValue<OfficeCountry>)e.Item.DataItem;
					string office = data.Value.ToString();

					officeLocation.Text = "<li id=\"" + office + "\">" + 
						Offices.ResourceManager.GetString(office) + "</li>";
				}
			}
		}

		#region Private Methods

		private void LoadCountryData()
		{
			ContactInformation.DataSource = CorbisBasePage.GetEnumDisplayValues<OfficeCountry>();
			ContactInformation.DataBind();
		}

		private void PopulateOfficeDropdown()
		{
            this.OfficeList.DataSource = CorbisBasePage.GetEnumDisplayValues<OfficeCountry>();
			this.OfficeList.DataValueField = "Id";
			this.OfficeList.DataTextField = "Text";
			this.OfficeList.DataBind();
		}

		#endregion
	}
}