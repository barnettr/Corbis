using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Navigation;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.Utilities.StateManagement;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.Utilities;
using Corbis.Web.Entities;
using Corbis.CommonSchema.Contracts.V1;



 namespace Corbis.Web.UI.Search
{
    public partial class SearchResults : CorbisBasePage, IPostSearchView
    {
        #region Constants
        
        private const string Session_SearchQueryString_Name = "Search";
        private const string Session_SearchQueryString_Key = "Querystring";
        private const string CountNumberFormat = "<span class=\"SearchCount\">({0:#,##0})</span>";
        private const string MSOTRIGGER = "hiddenMSOTrigger";
        
        #endregion

        #region Fields

        private StateItemCollection stateItems;
        private List<CartDisplayImage> searchlightboxItems;
        private int _currentPageNumber;

        #endregion

        #region Public Properties

        public SearchPresenter SearchPresenter
        {
            get 
            {
                return searchControl.SearchPresenter;
            }
        }

        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                searchResultHeader.CurrentPage = value;
                searchResultFooter.CurrentPage = value;
            }
        }

        #endregion

        #region Page Events

        protected override void OnInit(EventArgs e)
		{
            base.OnInit(e);

            this.searchControl.SearchPresenter.PostSearchView = this;

            this.AddScriptToPage(SiteUrls.SearchResultsJavascript, "SearchJS");
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/CustomerService/CustomerServiceWebService.asmx"));

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Search, "SearchCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.SearchMSO, "SearchMSOCSS");

            stateItems = new StateItemCollection(System.Web.HttpContext.Current);

            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();		
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Override the events on the pager controls
            searchResultHeader.NextButton.OnClientClick = "CorbisUI.ExtendedSearch.nextPage(); return false;";
            searchResultHeader.PreviousButton.OnClientClick = "CorbisUI.ExtendedSearch.previousPage(); return false;";
            searchResultHeader.PageNumberTextBox.Attributes["onkeypress"] = "return CorbisUI.ExtendedSearch.pageNumberKeypress(event,this);";
            searchResultHeader.PageNumberTextBox.Attributes.Remove("onchange");

            searchResultFooter.NextButton.OnClientClick = "CorbisUI.ExtendedSearch.nextPage(); return false;";
            searchResultFooter.PreviousButton.OnClientClick = "CorbisUI.ExtendedSearch.previousPage(); return false;";
            searchResultFooter.PageNumberTextBox.Attributes["onkeypress"] = "return CorbisUI.ExtendedSearch.pageNumberKeypress(event,this);";
            searchResultFooter.PageNumberTextBox.Attributes.Remove("onchange");
            

            //if (IsPostBack && Request.Form["__EVENTTARGET"] == quickpicField.ClientID)
            //{
            //    QuickPicUpdatePanel.Update();
            //}
            // this block is executed when the page is invoked for the first time or
            // when Response.Redirect is invoked
            // There are two places where Response.Redirect is triggered manually
            // Page_Load events of SearchResults.aspx.cs and 
            // DoSearch()[which is invoked when GO button is pressed] method of Search.ascx.cs
            if (!IsPostBack)
            {
                InitalizeCheckboxControls();
                stateItems.SetStateItem<string>(new StateItem<string>(
                    Session_SearchQueryString_Name,
                    Session_SearchQueryString_Key,
                    Server.UrlDecode(Request.QueryString.ToString()),
                    StateItemStore.AspSession));

                SearchPresenter.PopulateSearchFlyoutAndSearchBuddyFilterState();
                SearchPresenter.LoadLightBoxData();
                SearchPresenter.SetSearchBuddyTabs();
                SearchPresenter.PopulateSearchFlyoutAndSearchBuddyFilterState(Request.QueryString);
                OutputEnumClientScript<ImageMediaSetType>(Page);
            }
            if (!ScriptManager.GetCurrent(this).IsInAsyncPostBack)
            {
                OutputEnumClientScript<LicenseModel>(Page);
            }
            searchResultHeader.showHidden = ShowClarification.ToString();
            if (Request.Form["__EVENTTARGET"] != quickpicField.ClientID.Replace('_','$'))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetPageFocus", "CorbisUI.QueueManager.SearchMacros.runItem('pageScrollToTop');", true);
            }
            AnalyticsData["prop5"] = Request.QueryString.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // This condition Builds a Query manually and redirects the page(all other Page Events are not executed)
            // the page must not be redirected when None of the Clarifications selected for Keyword/s with Clarifications
            //if (IsPostBack && (Request.Form["__EVENTTARGET"] == "searchFilter" || !string.IsNullOrEmpty(ClarificationsQueryFlags) || SearchPresenter.SharedFiltersChangedWithClarification))
            //{
            //    // dont redirect the page if No Clarifications are selected
            //    if (SearchPresenter.AtleastOneClarificationSelected(ClarificationsQueryFlags))
            //    {
            //        string query = SearchPresenter.BuildQuery();
            //        SearchPresenter.SaveSearchPreferences();
            //        SearchPresenter.SaveDefaultSearchFilters();
            //        Response.Redirect(string.Format("{0}?{1}", SiteUrls.SearchResults, query));
            //    }

            //}

			string scriptManagerId = Request.Form[Master.FindControl("scriptManager").UniqueID] ?? "";
            string quickCheckout = "isQuickCheckoutEnabled = false;";
            if (Profile.IsFastLaneEnabled && Profile.IsECommerceEnabled)
            {
                quickCheckout = "isQuickCheckoutEnabled = true;";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickCheckoutVars", quickCheckout, true);

			if (scriptManagerId.EndsWith(detailViewUpdatelightbox.UniqueID) ||
				scriptManagerId.Contains(createLightbox.UniqueID) ||
				scriptManagerId.Contains(hiddenRefresh.UniqueID))
			{
				//updated selected lightbox and refresh light box
				string selectedValue = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
				if (lightboxList.Items.FindByValue(selectedValue) == null) SearchPresenter.LoadLightBoxData();
				lightboxList.SelectedValue = selectedValue;
				lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
				ScriptManager.RegisterStartupScript(this, this.GetType(), "SelectNRefreshLightboxSearchBuddy", "$('SBT_lightboxes').fireEvent('click');CorbisUI.Search.Handler.syncLightboxToImages();", true);
			}
			else if (scriptManagerId.Contains(lightboxItems.UniqueID) || scriptManagerId.Contains(lightboxList.UniqueID))
			{
				//just refresh lightbox
				lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
				ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshLightboxSearchBuddy", "CorbisUI.Search.Handler.syncLightboxToImages();", true);
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
				}

                SearchPresenter.SaveSearchPreferences();
                SearchPresenter.SaveDefaultSearchFilters(Request.QueryString);
                if(!scriptManagerId.Contains(MSOTRIGGER))
                {
                    int pageIndex;
                    // convert p param from querystring to determine page number.
                    if (string.IsNullOrEmpty(Request.QueryString["p"]) 
                        || !int.TryParse(Request.QueryString["p"], out pageIndex))
                    {
                        pageIndex = 1;
                    }
                    SearchPresenter.GetImageSearchResults(Request.QueryString, (int)this.searchResultHeader.PageSize, pageIndex);
                }
			}
            if (!Profile.IsAnonymous)
                emptyLightboxMessage.Visible = true;

            if (!this.IsPostBack && Profile.IsQuickPicEnabled && quickPicItems.Count == 0)
            {
                quickPicDownloadAllLink.Attributes["class"] += " hdn";
            }
        }

        #endregion

        #region Event Handlers

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

        protected void searchResultHeader_GenericCommand(object sender, CommandEventArgs e)
        {

            CurrentPageNumber = 1;
            RedirectToPage(CurrentPageNumber);
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

        protected void clarificationGroups_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater clarificationTerms = (Repeater)item.FindControl("clarificationTerms");
                Clarification group = (Clarification)item.DataItem;
                clarificationTerms.DataSource = group.ClarifiedTerms;
                clarificationTerms.DataBind();
            }
        }

        #endregion

        #region ISearchBaseView

        public string KeywordSearch
        {
            get 
            { 
                return this.searchControl.KeywordSearch;
            }
            set 
            {
                this.searchControl.KeywordSearch = value;
            }
        }

        #endregion

        #region IPostSearchView

        #region Filter checked state

        public bool Creative
        {
            get { return creative.Checked; }
            set
            {
                creative.Checked = value;
                //this.searchControl.Creative = value;
            }
        }

        public bool Editorial
        {
            get { return editorial.Checked; }
            set 
            { 
                editorial.Checked = value;
                //this.searchControl.Creative = value;
            }
        }

        public bool Documentary
        {
            get { return documentary.Checked; }
            set 
            {
                documentary.Checked = value;
                //this.searchControl.Documentary = value;
            }
        }

        public bool Archival
        {
            get { return archival.Checked; }
            set 
            { 
                archival.Checked = value;
                //this.searchControl.Archival = value;
            }
        }

        public bool CurrentEvents
        {
            get { return currentEvents.Checked; }
            set 
            {
                currentEvents.Checked = value;
                //this.searchControl.CurrentEvents = value;
            }
        }

        public bool FineArt
        {
            get { return fineArt.Checked; }
            set 
            { 
                fineArt.Checked = value;
                //this.searchControl.FineArt = value;
            }
        }

        public bool Entertainment
        {
            get { return entertainment.Checked; }
            set
            {
                entertainment.Checked = value;
                //this.searchControl.Entertainment = value;
            }
        }

        public bool Outline
        {
            get { return outline.Checked; }
            set 
            {
                outline.Checked = value;
                //this.searchControl.Outline = value;
            }
        }

        public bool RightsManaged
        {
            get { return rightsManaged.Checked; }
            set 
            {
                rightsManaged.Checked = value;
                //this.searchControl.RightsManaged = value;
            }
        }

        public bool RoyaltyFree
        {
            get { return royaltyFree.Checked; }
            set 
            { 
                royaltyFree.Checked = value;
                //this.searchControl.RoyaltyFree = value;
            }
        }

        public bool Photography
        {
            get { return photography.Checked; }
            set 
            { 
                photography.Checked = value;
                //this.searchControl.Photography = value;
            }
        }

        public bool NoPeople
        {
            get { return noPeople.Checked; }
            set 
            {
                noPeople.Checked = value;
                //this.searchControl.NoPeople = value;
            }
        }

        public bool Illustration
        {
            get { return illustration.Checked; }
            set 
            { 
                illustration.Checked = value;
                //this.searchControl.Illustration = value;
            }
        }

        public bool Color
        {
            get { return color.Checked; }
            set 
            { 
                color.Checked = value;
                //this.searchControl.Color = value;
            }
        }

        public bool BlackWhite
        {
            get { return blackWhite.Checked; }
            set 
            {
                blackWhite.Checked = value;
                //this.searchControl.BlackWhite = value;
            }
        }

        public bool ModelReleased
        {
            get { return modelReleased.Checked; }
            set 
            { 
                modelReleased.Checked = value;
                //this.searchControl.ModelReleased = value;
            }
        }
        public bool ShowZeroResults
        {
            set
            {
                this.zeroSearchResultsWrapper.Visible = value;
                this.searchResultHeader.Visible = false;
                this.searchResultFooter.Visible = false;
                this.products.ShowDownloadingProhibited = !value;
                this.zeroSearchTrue.Value = value.ToString();
            }
        }

        #endregion

        #region Lightboxes

        public bool ShowAddToLightboxPopup
        {
            set
            {
				if (Profile.IsAnonymous)
				{
					lightboxItems.Visible = false;
					signIn.Visible = true;                  
                  	pnlCreateLightBox.CssClass = "disabled";
				}
            }
        }

        

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

        public List<CartDisplayImage> LightboxItems
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

       // [StateItemDesc(TermClarifications.Name, TermClarifications.ShowClarificationKey, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        public bool ShowClarification
        {
            //get
            //{
            //    HttpCookieCollection cookieCollection = this.Request.Cookies;
            //    bool show;
            //    bool.TryParse(showTermsClarification.Value, out show);
            //    return show;
            //}
            //set
            //{
            //    showTermsClarification.Value = value.ToString();
            //}
            get
            {
                string show = stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.ShowClarificationCookieKey, StateItemStore.Cookie);
                if (string.IsNullOrEmpty(show))
                {
                    return true;
                }
                else
                {
                  return   bool.Parse(show);
                }        

            }
        }

        public bool ShowClarificationPopup
        { 
            get
            {
                return bool.Parse(showClarificationPopup.Value);
            }
            set
            {
                showClarificationPopup.Value = value.ToString();
            }
        }

        public string ClarificationsQueryFlags 
        { 
            get
            {
                return clarificationCheckedQueryFlags.Value;
            }
        }

        public List<Clarification>  Clarifications
        {
            get
            {
                return clarificationGroups.DataSource as List<Clarification>;
            }
            set
            {
                clarificationGroups.DataSource = value;
                clarificationGroups.DataBind();
            } 
        }

        #endregion

        #endregion

        #region Search Header Fields

        public int TotalSearchHitCount
        {
            set
            {
                this.searchResultHeader.TotalSearchHitCount = value;
                this.searchResultFooter.PageSize = searchResultHeader.PageSize;
                this.searchResultFooter.TotalSearchHitCount = value;
            }
        }

        public int CurrentPageHitCount
        {
            set
            {
                this.searchResultHeader.CurrentPageHitCount = value;
                this.searchResultFooter.CurrentPageHitCount = value;
            }
        }

        public string SearchResultTitle
        {
            set { this.searchResultHeader.SearchResultTitle = value; }
        }

        public string SearchResultPhotographerName
        {
            set
            {
                this.searchResultHeader.SetPhotoGrapherNameSearchResultTitle = value;
            }
        }
        public string SearchResultLocation
        {
            set
            {
                this.searchResultHeader.SetLocationSearchResultTitle = value;
            }
        }

        #endregion

        #region Other View Interface members

        public List<SearchResultProduct> SearchResultProducts
        {
            set
            {
                products.WebProductList = value;
                //if(value.Count == 0)
                //{
                //    this.zeroSearchResultsDiv.Visible = true;
                //}
                //else
                //{
                //    this.zeroSearchResultsDiv.Visible = false;
                //}
            }
            get { return (List<SearchResultProduct>)products.WebProductList; }
          
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
#if DEBUG
                //this.SBT_quickpic.Visible = true;
#endif
            }
        }

        public void AdjustStatusForUser()
        {
            if (Profile.IsAnonymous)
            {
                lightboxList.Visible = false;
                createNew.Visible = false;
                emptyLightboxMessage.Visible = false;
                //lightboxList.CssClass = "disabled";
                //lightboxList.Enabled = false;
            }
        }

        #endregion

        #region UpdateQuickPic


        protected void updateQuickPick(object sender, EventArgs e)
        {
            QuickPicUpdatePanel.Update();

        }
        

        #endregion

        #region Helper Methods

        private void RedirectToPage(int index)
        {
            NameValueCollection queryCollection = new NameValueCollection();
            foreach (string item in Request.QueryString)
            {
                if (item != "options")
                {
                    queryCollection.Add(item, Request.QueryString[item]);
                }
            }

            string query = SearchPresenter.BuildQueryStringForPagingAndSort(index, ClarificationsQueryFlags, queryCollection);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "RefreshParentScript", "CorbisUI.runQueue('searchDomReady', true);", true);
        
            //Response.Redirect(string.Format("{0}?{1}", SiteUrls.SearchResults, query));
        }

        private void InitalizeCheckboxControls()
        {
            this.archival.Text = GetEnumDisplayText<Category>(Category.Archival);
            this.archival.Value = ((int)Category.Archival).ToString();

            this.creative.Text = GetEnumDisplayText<Category>(Category.Creative);
            this.creative.Value = ((int)Category.Creative).ToString();

            this.currentEvents.Text = GetEnumDisplayText<Category>(Category.CurrentEvents);
            this.currentEvents.Value = ((int)Category.CurrentEvents).ToString();

            this.documentary.Text = GetEnumDisplayText<Category>(Category.Documentary);
            this.documentary.Value = ((int)Category.Documentary).ToString();

            this.entertainment.Text = GetEnumDisplayText<Category>(Category.Entertainment);
            this.entertainment.Value = ((int)Category.Entertainment).ToString();

            this.fineArt.Text = GetEnumDisplayText<Category>(Category.FineArt);
            this.fineArt.Value = ((int)Category.FineArt).ToString();

            this.outline.Text = GetEnumDisplayText<Category>(Category.Outline);
            this.outline.Value = ((int)Category.Outline).ToString();
            this.outline.Visible = Profile.CanSeeOutline;

            this.rightsManaged.Value = ((int)LicenseModel.RM).ToString(); 
            this.royaltyFree.Value = ((int)LicenseModel.RF).ToString();

            this.noPeople.Value = ((int)NumberOfPeople.WithoutPeople).ToString();
            
            this.photography.Value = ((int)MediaType.Photography).ToString();
            this.illustration.Value = ((int)MediaType.Illustration).ToString();
            
            this.color.Value = ((int)ColorFormat.Color).ToString();
            this.blackWhite.Value = ((int)ColorFormat.BlackAndWhite).ToString();
            
            this.modelReleased.Value = ((int)ModelRelease.OnFile).ToString();

        }

        #endregion

        #region IPostSearchView Members


        public string RecentImageId
        {
            set 
            {
                this.searchResultHeader.RecentImageId = value;
            }
        }

        public string RecentImageURL
        {
            set
            {
                this.searchResultHeader.RecentImageURL = value;
            }
        }

        public decimal RecentImageRadio
        {
            set { this.searchResultHeader.RecentImageRadio = value; }
        }

        #endregion

        #region IPostSearchView Members


        public SearchSort SearchSortOption
        {
            get
            {
                return this.searchResultHeader.SearchSortOption;
            }
            set
            {
                this.searchResultHeader.SearchSortOption = value;
            }
        }
        public SearchSort? SearchSortOptionHidden
        {
            get
            {
                return searchResultHeader.SearchSortOptionHidden;
            }
            set
            {
                searchResultHeader.SearchSortOptionHidden = value;
            }
        }

        #endregion
    }
}
