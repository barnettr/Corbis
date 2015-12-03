using System;
using System.Collections.Generic;
using System.Threading;
using Corbis.Framework.Globalization;
using Corbis.Web.Entities;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.MasterPages;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Content;
using Corbis.Web.UI.Presenters;
using System.Diagnostics.CodeAnalysis;

using Corbis.Web.Utilities.StateManagement;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.IO;

namespace Corbis.Web.UI.Navigation
{
    public partial class GlobalNav : CorbisBaseUserControl
    {
        private DropDownMenuPresenter dropDownMenuPresenter;
        private static XmlDocument configDoc = null;

        public static XmlDocument InitConfigDoc(string docPath)
        {
			if (configDoc == null || docPath.ToLower().IndexOf("browse") > 0)
            {
                configDoc = new XmlDocument();
                configDoc.Load(docPath);
            }
            return configDoc;
        }
        protected override void OnInit(EventArgs e)
        {
            #region Set links

            myAccount.NavigateUrl = "javascript:CorbisUI.Auth.Check(2,CorbisUI.Auth.ActionTypes.ReturnUrl,'" + SiteUrls.MyProfile + "')";
            myLightboxes.NavigateUrl = "javascript:CorbisUI.Auth.Check(1,CorbisUI.Auth.ActionTypes.ReturnUrl,'" + SiteUrls.Lightboxes + "')";
            home.NavigateUrl = SiteUrls.Home;
            customerService.NavigateUrl = SiteUrls.CustomerService;

            #endregion


            #region Load Language List
            List<ContentItem> languageList = null;
			string absPath = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

			if (absPath.IndexOf("browse") > 0)
			{
				string curPath = string.Empty;
				string curPage = Path.GetFileNameWithoutExtension(HttpContext.Current.Request.FilePath);

				if (absPath.IndexOf("browse") > 0)
					curPath = "~/Browse/xml/" + curPage + "/" + curPage + ".xml";

                string docPath = System.Web.HttpContext.Current.Server.MapPath(curPath);
                XmlDocument doc = InitConfigDoc(docPath);
                XmlNode attr = doc.DocumentElement.SelectSingleNode("/Page/LanguageListSelector");

                if(attr.Attributes["suppressAll"].Value.ToLower() == "false")
                {
                    languageList = ((LanguageContentProvider)ContentProviderFactory.CreateProvider(ContentItems.Language)).GetLanguages(docPath);
                }
				else
				{
                    languageList = ((LanguageContentProvider)ContentProviderFactory.CreateProvider(ContentItems.Language)).GetLanguages();
                }

            }
            else
			{
                languageList = ((LanguageContentProvider)ContentProviderFactory.CreateProvider(ContentItems.Language)).GetLanguages();
            }

            languageSelectorMenuID.LanguageList = languageList;
            languageSelectorMenuID.ItemCommand += new EventHandler<LanguageMenuChangedEventArgs>(LanguageSelectorMenu_ItemCommandChange);
            #endregion

            #region Set Language

			languageSelectorMenuID.SelectedCultureValue = Language.CurrentLanguage.LanguageCode;
			selectedLanguageText.Text = languageSelectorMenuID.SelectedCultureName;

            #endregion

            #region Logo

            if (Request.Url.AbsolutePath.Equals(ResolveUrl(SiteUrls.Home), StringComparison.InvariantCultureIgnoreCase))
            {
                // no hover or navigation on home page
                home.NavigateUrl = string.Empty;
                logoOver.Visible = false;
            }

            #endregion

            #region Browse Images

            dropDownMenuPresenter = new DropDownMenuPresenter();
            dropDownMenu.DropDownItems = dropDownMenuPresenter.GetDropDownMenuData();
            dropDownMenu.ItemCommand += new EventHandler<DropDownMenuChangedEventArgs>(DropDown_ItemCommandChange);

            #endregion

            #region Beta Logo

            BetaFlag.Visible = CorbisMasterBase.IsInBeta();

            #endregion
            
            base.OnInit(e);
        }

        /// <summary>
        /// ThemeSelector_ThemeIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThemeSelector_ThemeIndexChanged(object sender, ThemeChangedEventArgs e)
        {
            Theme theme;

            if (e == null)
            {
                theme = Theme.BlackBackground;
            }
            else
            {
                theme = (Theme)Enum.Parse(typeof(Theme), e.CurrentThemeValue);
            }

            if (theme == Theme.None)
            {
                theme = Theme.BlackBackground;
            }

            StateItem<Theme> pageThemeStateItem = new StateItem<Theme>(
                "ThemeSelector",
                "Theme",
                theme,
                StateItemStore.Cookie | StateItemStore.AspSession,
                StatePersistenceDuration.NeverExpire
                );

            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            stateItems.SetStateItem<Theme>(pageThemeStateItem);

            Response.Redirect(Request.Url.ToString());
        }

        protected void ChangeLanguageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ProfileCookie cookie = new ProfileCookie();
            //cookie.SetProperty(ProfileCookieKeys.CultureName, changeLanguageList.SelectedItem.Value);
            //cookie.Save();

            //Response.Redirect(Request.Url.ToString());
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected void DropDown_ItemCommandChange(object sender, DropDownMenuChangedEventArgs e)
        {
            if (e == null || String.IsNullOrEmpty(e.NavigateUrl))
            {
                Response.Redirect(SiteUrls.PageNotFound);
                return;
            }

            Response.Redirect(e.NavigateUrl);
        }

        protected void LanguageSelectorMenu_ItemCommandChange(object sender, LanguageMenuChangedEventArgs e)
        {
            if (e == null || String.IsNullOrEmpty(e.LanguageCultureValue))
            {              
                return;
            }

            // Save culture info to Profile and session only if it changed
            if (Language.CurrentCulture.Name != e.LanguageCultureValue)
            {
                StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
                stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, e.LanguageCultureValue, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                Response.Redirect(Request.Url.AbsolutePath + "?"+Request.QueryString);
            }
        }

        public void UpdateCartCount()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "updateCartCount", "CorbisUI.GlobalNav.RefreshGlobalNav()", true);
            //checkout.CartCount = Profile.CartItemsCount.ToString();
            //cartUpdate.Update();
        }
    }
}
