using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Utilities;
using System.Web.Security;
using System.Web.Configuration;

namespace Corbis.Web.UI.MasterPages
{
    public partial class AccountsMaster : CorbisMasterBase
    {
        List<NavigationTab> tabs;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Profile != null && Profile.IsChinaUser)
            {
                ApplyForCredit.Visible = false;
            }
            else
            {
                applyCredit.NavigateUrl = SiteUrls.ApplyCredit;
            }

            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();
        }

        protected void Page_Load()
        {
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Accounts, "AccountsCSS");

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sessionTimeout", "setTimeout(\"CorbisUI.GlobalNav.SessionTimedOut()\"," + this.GetFormsAuthTimeout().TotalMilliseconds.ToString() + ");", true);

            if (!IsPostBack)
            {
                LoadNavigationTabs();
            }
        }

        private void LoadNavigationTabs()
        {
            tabs = new List<NavigationTab>();
            if (Page.Theme == Theme.BlackBackground.ToString())
            {
                tabs.Add(new NavigationTab("../../App_Themes/BlackBackground/Images/iconDarkProfilePref.png", SiteUrls.MyProfile, GetLocalResourceObject("profileTabText").ToString()));
            }
            else // if (Page.Theme == Theme.WhiteBackground.ToString())
            {
                tabs.Add(new NavigationTab("../../App_Themes/WhiteBackground/Images/iconLightProfilePref.png", SiteUrls.MyProfile, GetLocalResourceObject("profileTabText").ToString()));
            }
            if (Profile != null && !Profile.IsChinaUser)
            {
                tabs.Add(new NavigationTab("../../Images/iconOrders.png", SiteUrls.MyOrders, GetLocalResourceObject("OrderTabText").ToString()));
            }
            //if (Page.Theme == Theme.BlackBackground.ToString())
            //{
            //    tabs.Add(new NavigationTab("../../App_Themes/BlackBackground/Images/iconDarkUsages.png", SiteUrls.MyUsages, GetLocalResourceObject("UsageTabText").ToString()));
            //}
            //else // if (Page.Theme == Theme.WhiteBackground.ToString())
            //{
            //    tabs.Add(new NavigationTab("../../App_Themes/WhiteBackground/Images/iconLightUsages.png", SiteUrls.MyUsages, GetLocalResourceObject("UsageTabText").ToString()));
            //}
            navigation.DataSource = tabs;
            navigation.DataBind();

            for (int i = 0; i < tabs.Count; ++i)
            {
                if (ResolveUrl(Request.Url.AbsolutePath).Equals(tabs[i].NavigateUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    ((HtmlGenericControl)navigation.Items[i].FindControl("tab")).Attributes["class"] += " Active";
                    ((HyperLink)navigation.Items[i].FindControl("hyperLink")).NavigateUrl = string.Empty;
                    break;
                }
            }
        }
    }

    public class NavigationTab   
    {
        public string imageUrl;
        public string navigateUrl;
        public string text;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }
        public string NavigateUrl
        {
            get { return navigateUrl; }
            set { navigateUrl = value; }
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public NavigationTab(string imageUrl, string navigateUrl, string text)
        {
            this.imageUrl = imageUrl;
            this.navigateUrl = navigateUrl;
            this.text = text;
        }
    }
}
