using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Corbis.Framework.Globalization;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.QuickPic
{
	public partial class QuickPic : CorbisBasePage
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page.ClientScript.RegisterClientScriptInclude("QuickPicScript", SiteUrls.QuickPicScript);
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.QuickPic, "QuickPicCSS");

			ScriptManager manager = ScriptManager.GetCurrent(Page);
            //todo:  add the following URL to SiteURLs
			manager.Services.Add(new ServiceReference("~/QuickPic/QuickPicScriptService.asmx"));
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            // Check the user's authentication status. Only Fully authenticated users can access the site.
            if (!Profile.IsAuthenticated)
            {
                Response.Redirect(SiteUrls.Home);
            }
            else
            {
                copyrightMsg.Text = CopyrightHelper.CopyrightText;
                contactMessage.Text = String.Format(GetLocalResourceString("contactMessage"), Corbis.Web.UI.SiteUrls.CustomerService);

                // display TRUSTe?
                if (Profile.CountryCode != UScountryCode)
                {
                    this.TRUSTeLink.Visible = false;
                }

                projectName.Text = DateHelper.GetLocalizedDate(DateTime.Now, DateFormat.ShortDateFormat);
                projectName.Attributes.Add("onblur", "if(this.value=='') this.value = '" + DateHelper.GetLocalizedDate(DateTime.Now, DateFormat.ShortDateFormat) + "';");
            }
       
		}
	}
}
