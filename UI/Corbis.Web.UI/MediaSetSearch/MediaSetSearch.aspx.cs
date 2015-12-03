using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.MediaSetSearch;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Web.UI.Navigation;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Utilities.StateManagement;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.Utilities;
using Corbis.Web.Entities;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.UI.Search;
using System.Web.UI.HtmlControls;


namespace Corbis.Web.UI.MediaSetSearch
{
    public partial class MediaSetSearch : CorbisBasePage, IMediasetSearchView 
    {
        private int _currentPageNumber;
        private int itemsPerPage;
        private StateItemCollection stateItems;
        private List<LightboxDisplayImage> searchlightboxItems;
        public MediasetSearchPresenter _mediaSetspresenter;
        #region Public Properties
        
        public int CurrentPageNumber
        {
            get { return searchResultHeader.CurrentPage; }
            set
            {
                _currentPageNumber = value;
                searchResultHeader.CurrentPage = value;
                searchResultFooter.CurrentPage = value;
            }
        }

        public int TotalRecords
        {
            get { return this.searchResultHeader.TotalRecords; }
            set
            {
                this.searchResultFooter.PageSize = this.searchResultHeader.PageSize;
                this.searchResultHeader.TotalRecords = value;
                this.searchResultFooter.TotalRecords = value;
                this.searchResultHeader.TotalSearchHitCount = value;
                this.searchResultFooter.TotalSearchHitCount = value;
            }
        }

        #endregion
        #region Search Header Fields
        public int ItemsPerPage
        {
            get { return (int)this.searchResultHeader.PageSize; }
            set { itemsPerPage = value; }
        }
        public int TotalSearchHitCount
        {
            set
            {
                this.searchResultFooter.PageSize = searchResultHeader.PageSize;
                this.searchResultHeader.TotalSearchHitCount = value;
                this.searchResultFooter.TotalSearchHitCount = value;
            }
        }

        public int CurrentPageHitCount
        {
            get { return 0; }
            set
            {
                this.searchResultHeader.CurrentPageHitCount = value;
                this.searchResultFooter.CurrentPageHitCount = value;
            }
        }

        
        public bool ShowZeroResults
        {
            set
            {
                this.zeroSearchResultsWrapper.Visible = value;
                this.searchResultHeader.Visible = true;
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MediaSetHeader, "MediaSetHeaderCSS");
                if (zeroSearchResultsWrapper.Visible && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_REFERER"]))
                {
                    PreviousButton.Visible = true;
                }
                HtmlGenericControl headerDiv = (HtmlGenericControl)searchResultHeader.FindControl("header2Div");
                headerDiv.Visible = false;
                this.searchResultFooter.Visible = false;
                this.zeroSearchTrue.Value = value.ToString();
            }
        }

        #endregion
        #region Lightboxes

        

        public List<Lightbox> LightboxList
        {
            set
            {
                lightboxList.DataSource = LightboxesPresenter.GetLightboxesDropdownSource(value, 1);
                lightboxList.DataValueField = "Value";
                lightboxList.DataTextField = "Key";
                lightboxList.DataBind();
            }
        
        }

        public string ActiveLightbox
        {
            get
            {
                return stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                     StateItemStore.Cookie); 
            }
            set
            {
                StateItem<string> activeLightbox = new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                                 value, StateItemStore.Cookie);
                stateItems.SetStateItem(activeLightbox);
            }
        }


        public List<LightboxDisplayImage> LightboxItems
        {
            get
            {
                return searchlightboxItems;
            }
            set
            {
                searchlightboxItems = value;
            }

        }


        #endregion
        #region Other View Interface members

        public List<MediaSet> MediasetList
        {
            set
            {
                products.MediasetList = value;
            }

        }

        public bool ShowQuickPicTab
        {
            get
            {
                return this.SBT_quickpic.Visible;
            }
            set
            {
                this.SBT_quickpic.Visible = value;
            }
        }

        public void AdjustStatusForUser()
        {
            if (Profile.IsAnonymous)
            {
                lightboxList.CssClass = "disabled";
                lightboxList.Enabled = false;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.AddScriptToPage(SiteUrls.SearchResultsJavascript, "ImageGrupsSearchJS");

            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Search, "SearchCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MediaSetSearch, "MediaSetSearchCSS");
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            _mediaSetspresenter = new MediasetSearchPresenter(this);
            int selectedValue = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (zeroSearchResultsWrapper.Visible && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_REFERER"]))
                {
                    PreviousButton.Visible = true;
                }
                _mediaSetspresenter.SetSearchBuddyTabs();

                if (!Profile.IsAnonymous)
                {
                    _mediaSetspresenter.LoadLightBoxData();
                }
            }

            if (Profile.IsAnonymous)
            {
                signIn.Visible = true;
            }

            this.searchResultHeader.SearchResultTitle = GetLocalResourceObject("relatedsets.Text").ToString();

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string scriptManagerId = Request.Form[Master.FindControl("scriptManager").UniqueID] ?? "";

            if (scriptManagerId.EndsWith(detailViewUpdatelightbox.UniqueID) ||
                scriptManagerId.Contains(createLightbox.UniqueID) ||
                scriptManagerId.Contains(hiddenRefresh.UniqueID))
            {
                int lightboxId = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                  StateItemStore.Cookie);
                //updated selected lightbox and refresh light box
                int selectedValue = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
                if (lightboxList.Items.FindByValue(selectedValue.ToString()) == null) _mediaSetspresenter.LoadLightBoxData();
                lightboxList.SelectedValue = selectedValue.ToString();
                lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SelectNRefreshLightboxSearchBuddy", "$('SBT_filters').fireEvent('click');CorbisUI.Search.Handler.syncLightboxToImages();", true);
            }
            else if (scriptManagerId.Contains(lightboxItems.UniqueID) || scriptManagerId.Contains(lightboxList.UniqueID))
            {
                //just refresh lightbox
                lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
                //ActiveLightbox = lightboxList.SelectedValue;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshLightboxSearchBuddy", "CorbisUI.Search.Handler.syncLightboxToImages();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateImageCount", String.Format("$('LBXContainer').setProperty('imageCount', '{0}');", lightboxItems.ItemCount), true);
            }

            else if (scriptManagerId.Contains(quickpicField.UniqueID))
            {
                //QuickPicUpdatePanel.Update();
                //this.RebindTooltip();
            }
            else if (!(scriptManagerId.Contains(createLightbox.UniqueID)) && !(scriptManagerId.Contains(lightboxList.UniqueID)))
            {
                if (!Profile.IsAnonymous &&
                    !string.IsNullOrEmpty(lightboxList.SelectedValue)
                )
                {
                    string selectedValue = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);

                    if (!String.IsNullOrEmpty(selectedValue)
                        && stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey, StateItemStore.Cookie) != "date")
                    {
                        lightboxList.SelectedValue = selectedValue;
                    }
                    lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateImageCount", String.Format("$('LBXContainer').setProperty('imageCount', '{0}');", lightboxItems.ItemCount), true);
                }

                ItemsPerPage = (int)this.searchResultHeader.PageSize;


               // _mediaSetspresenter.GetImageGroupResults();
                //_mediaSetspresenter.SetCaptionVisibility();
            }

            //don't need to build images if we are just refreshing the lightbox buddy or deleting item.
            if (!scriptManagerId.Contains(hiddenRefresh.UniqueID) && 
                !scriptManagerId.Contains("btnClose$HoverButton") &&
                !scriptManagerId.Contains(lightboxList.UniqueID))
            {
                BuildSearchFiltersImage(Request.QueryString);
            }
        }
      
        protected void lightboxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxList.SelectedValue, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
            //Just update lightbox in OnPreRender
        }
        protected void searchResult_PageCommand(object sender, PagerEventArgs e)
        {
            int index = e.PageIndex;
            CurrentPageNumber = index;
            //RebindTooltip();
            RedirectToPage(index);
        }

        protected void searchResultHeader_PageSizeCommand(object sender, PageSizeEventArgs e)
        {
            //searchPresenter.SavePreferences();
            CurrentPageNumber = 1;
            //RebindTooltip();
            RedirectToPage(1);
        }
        protected void hiddenRefresh_OnChange(object sender, EventArgs e)
        {
            //Just to refresh lightbox in OnPreRender
        }
        protected void updateQuickPick(object sender, EventArgs e)
        {
            QuickPicUpdatePanel.Update();

        }

        private void BuildSearchFiltersImage(NameValueCollection query)
        {
            string[] keys = query.AllKeys;
            List<string> meidaSearchKeys = new List<string>(keys.Length);
            try
            {
                Dictionary<string, string> searchParameters = GetMediaSearchParameters(query);
                //Call Presenter
                _mediaSetspresenter.GetMediasetSearchResults(searchParameters);
            }
            catch
            {
                this.MediasetList = new List<MediaSet>();
            }
        }
        private static Dictionary<string, string> GetMediaSearchParameters(NameValueCollection searchParameters)
        {
            string[] paras =
                searchParameters.AllKeys;

            Dictionary<string, string> searchParam = new Dictionary<string, string>();
            for (int paramCount = 0; paramCount < paras.Length; paramCount++)
            {
                searchParam.Add(paras[paramCount], searchParameters.GetValues(paras[paramCount])[0]);
            }
            return searchParam;
        }
        #region Helper Methods

        private void RedirectToPage(int index)
        {
            string curUrl = Request.Url.PathAndQuery;
            if (curUrl.Contains("&p="))
            {
                curUrl = curUrl.Substring(0, curUrl.IndexOf("&p="));
            }
            if (!curUrl.Contains("?"))
            {
                Response.Redirect(curUrl + "?p=" + index.ToString());
            }
            else
            {
                Response.Redirect(curUrl + "&p=" + index.ToString());
            }
        }

        #endregion

    }
}
