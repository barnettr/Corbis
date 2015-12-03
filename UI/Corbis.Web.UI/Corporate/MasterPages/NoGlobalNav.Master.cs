using System;
using Corbis.Web.Utilities;
using System.Web.UI.HtmlControls;


namespace Corbis.Web.UI.Corporate.MasterPages
{
    public partial class NoGlobalNav : CorbisMasterBase
    {
        protected override void OnInit(EventArgs e)
		{
			if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
			{   // BUGBUG: this is IE 6 and below - why is it named IE7? (ron)
				this.AddScriptToPage(SiteUrls.IE7Script, "IE7JS");
			}

			this.AddScriptToPage(SiteUrls.MasterBaseJavascript, "MBScript");
			this.AddScriptToPage("/Scripts/CorporateBase.js", "BrowseBaseScript");

			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MasterBaseStyles, "MasterBaseStyles");
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.CorporateBase, "CorporatePageBaseStyle");

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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetCopyrightText();
        }

        public bool ShowFooter
        {
            set
            {
                this.footerDiv.Visible = value;
            }
        }

        private void SetCopyrightText()
        {
            this.footer.Text = CopyrightHelper.CopyrightText;
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
