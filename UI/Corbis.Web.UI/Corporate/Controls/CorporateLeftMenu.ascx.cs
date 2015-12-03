using System;
using System.Web;
using System.Web.UI.HtmlControls;

using Corbis.Framework.Globalization;
using Corbis.Web.UI.Controls;


namespace Corbis.Web.UI.Corporate
{
    public partial class CorporateLeftMenu : CorbisBaseUserControl
	{

		protected override void OnInit(EventArgs e)
		{
			AboutCorbis.NavigateUrl = "~/Corporate/Overview/Overview.aspx";
			Pressroom.NavigateUrl = "~/Corporate/Pressroom/Pressroom.aspx";
			PressReleases.NavigateUrl = "~/Corporate/Pressroom/PressReleases.aspx";
			PressFactSheet.NavigateUrl = "~/Corporate/Pressroom/PressFactSheet.aspx";
			Employment.NavigateUrl = "~/Corporate/Employment/Employment.aspx";
			CustomerService.NavigateUrl = SiteUrls.CustomerService;

			base.OnInit(e);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
		}
    }
}
