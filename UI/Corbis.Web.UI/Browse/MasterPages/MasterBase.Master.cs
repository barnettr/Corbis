using System;
using Corbis.Web.Utilities;
using System.Web.UI.HtmlControls;


namespace Corbis.Web.UI.Browse.MasterPages
{
    public partial class MasterBase : CorbisMasterBase
    {
        public Corbis.Web.UI.Navigation.GlobalNav GlobalNav
        {
            get { return globalNav; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {   // BUGBUG: this is IE 6 and below - why is it named IE7? (ron)
                this.AddScriptToPage(SiteUrls.IE7Script, "IE7JS");
            }

			this.AddScriptToPage(SiteUrls.MasterBaseJavascript, "MBScript");
			//this.AddScriptToPage("/Scripts/BrowseBase.js", "BrowseBaseScript");

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MasterBaseStyles, "MasterBaseStyles");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.BrowseBase, "BrowsePageBaseStyle");

            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie6Styles, "IeStylesCSS");
            }
			else if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion == 7)
			{
				HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie7Styles, "Ie7StylesCSS");
			}

			base.OnInit(e);
		}

		/// <summary>
		/// Adds another css class to the html body tag of the page
		/// without losing any existing css class
		/// </summary>
		public void AddClassToBodyTag(string cssClass)
		{
			HtmlGenericControl bodyTag = (HtmlGenericControl)this.FindControl("BodyTag");

			if (bodyTag != null)
				bodyTag.Attributes.Add("class", bodyTag.Attributes["class"] + " " + cssClass);
		}
    }
}
