using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Image.Contracts.V1;
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


namespace Corbis.Web.UI.ImageGroups
{

    public partial class ImageGroups : CorbisBasePage, IImageGroupsView
    {
        #region Constants

        private const string Session_SearchQueryString_Name = "Search";
        private const string Session_SearchQueryString_Key = "Querystring";
        private const string CountNumberFormat = "<font style=color:#999999> ({0:#,##0})</font>";

        #endregion

        #region Fields

        private StateItemCollection stateItems;
        private List<CartDisplayImage> searchlightboxItems;
        public ImageGroupsPresenter _imageGroupsPresenter;
        private string imageGroupName;
        private string imageGroupId;
        private int itemsPerPage;
        private int _currentPageNumber;

        #endregion

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

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.AddScriptToPage(SiteUrls.SearchResultsJavascript, "ImageGrupsSearchJS");
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ImageGroups, "ImageGrupsSearchCSS");

            _imageGroupsPresenter = new ImageGroupsPresenter(this);
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);

            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ScriptManager.GetCurrent(this).IsInAsyncPostBack)
            {
                OutputEnumClientScript<LicenseModel>(Page);
            }

            if (IsPostBack && Request.Form["__EVENTTARGET"] == quickpicField.ClientID)
            {
                QuickPicUpdatePanel.Update();
            }

            if (!IsPostBack)
            {
                stateItems.SetStateItem<string>(new StateItem<string>(
                    Session_SearchQueryString_Name,
                    Session_SearchQueryString_Key,
                    Server.UrlDecode(Request.QueryString.ToString()),
                    StateItemStore.AspSession));

                _imageGroupsPresenter.LoadLightBoxData();
                _imageGroupsPresenter.SetSearchBuddyTabs();
                if (zeroSearchResultsWrapper.Visible && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_REFERER"]))
                {
                  //  ViewState["ReferrerUrl"] = Request.ServerVariables["HTTP_REFERER"].ToString();
                    PreviousButton.Visible = true;
                }
                searchResultHeader.showHidden = ShowClarification.ToString();
                int curPage;
                if (!int.TryParse(Request.QueryString["p"], out curPage))
                    curPage = 1;
                CurrentPageNumber = curPage;
                OutputEnumClientScript<ImageMediaSetType>(Page);
            }

			ImageMediaSetType imageGroupType = Enum.IsDefined(typeof(ImageMediaSetType), int.Parse(Request.QueryString["typ"])) ? (ImageMediaSetType)(Enum.Parse(typeof(ImageMediaSetType), Request.QueryString["typ"])) : ImageMediaSetType.Unknown;
			Detectheader(imageGroupType);
        }

		private void Detectheader(ImageMediaSetType key)
        {
            

            switch(key)
            {
				case ImageMediaSetType.StorySet:
                    HeaderUserControl = LoadControl("StorySetHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetMediaSetProducts();
                    break;
				case ImageMediaSetType.Album:
                    HeaderUserControl = LoadControl("AlbumHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetMediaSetProducts();
                    break;
				case ImageMediaSetType.Promotional:
                    HeaderUserControl = LoadControl("PromotionalSetHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetMediaSetProducts();
                    break;
				case ImageMediaSetType.SameModel:
                    HeaderUserControl = LoadControl("SameModelHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetMediaSetProducts();
                    break;
				case ImageMediaSetType.PhotoShoot:
                    HeaderUserControl = LoadControl("SamePhotoshootHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetMediaSetProducts();
                    break;
				case ImageMediaSetType.OutlineSession:
                    HeaderUserControl = LoadControl("OutlineHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.GetSessionSetProducts();
                    break;
				case ImageMediaSetType.RFCD:
                    HeaderUserControl = LoadControl("RFCDHeader.ascx");
                    ShowHeader((IImageGroupHeader)HeaderUserControl);
                    _imageGroupsPresenter.DisplyRFCDResults();
                    break;
            }
            this.HeaderPanel.Controls.Add(HeaderUserControl);
        }

        private void ShowHeader(IImageGroupHeader header)
        {
            header.Visible = true;
            header.RecentImageId = Request.QueryString["ri"];
            if (!string.IsNullOrEmpty(header.RecentImageId))
            {
                DisplayImage image = _imageGroupsPresenter.GetRecentImage(header.RecentImageId);
                if(null != image)
                {
                    header.RecentImageURL = image.Url128;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string scriptManagerId = Request.Form[Master.FindControl("scriptManager").UniqueID] ?? "";

            if (scriptManagerId.Contains(addToLightboxPopup.UniqueID) ||
                scriptManagerId.EndsWith(detailViewUpdatelightbox.UniqueID) ||
                scriptManagerId.Contains(createLightbox.UniqueID) ||
                scriptManagerId.Contains(hiddenRefresh.UniqueID))
            {
                //updated selected lightbox and refresh light box
                string selectedValue = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
                if (lightboxList.Items.FindByValue(selectedValue) == null) _imageGroupsPresenter.LoadLightBoxData();
                lightboxList.SelectedValue = selectedValue;
                //lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SelectNRefreshLightboxSearchBuddy", "$('SBT_lightboxes').fireEvent('click');CorbisUI.Search.Handler.syncLightboxToImages();", true);
            }
            //else if (scriptManagerId.Contains(lightboxItems.UniqueID) || scriptManagerId.Contains(lightboxList.UniqueID))
            //{
            //    //just refresh lightbox
            //    lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
            //    ActiveLightbox = lightboxList.SelectedValue;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshLightboxSearchBuddy", "CorbisUI.Search.Handler.syncLightboxToImages();", true);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateImageCount", String.Format("$('LBXContainer').setProperty('imageCount', '{0}');", lightboxItems.ItemCount), true);
            //}

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
                    //lightboxItems.LightboxId = int.Parse(lightboxList.SelectedValue);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateImageCount", String.Format("$('LBXContainer').setProperty('imageCount', '{0}');", lightboxItems.ItemCount), true);
                }

                //_imageGroupsPresenter.SavePreferences();
                //_imageGroupsPresenter.SaveDefaultFilters();
                ImageGroupName = Request.QueryString["typ"];
                ImageGroupId = Request.QueryString["gId"];
                ItemsPerPage = (int) this.searchResultHeader.PageSize;
                

                _imageGroupsPresenter.GetImageGroupResults();
                //_imageGroupsPresenter.SetCaptionVisibility();
            }

            //if (lightboxItems.LightboxId > 0 && lightboxItems.ItemCount == 0)
            //{
            //    emptyLightboxMessage.Visible = true;
            //}
            //else
            //{
            //    emptyLightboxMessage.Visible = false;
            //}

            string quickCheckout = "isQuickCheckoutEnabled = false;";
            if (Profile.IsFastLaneEnabled && Profile.IsECommerceEnabled)
            {
                quickCheckout = "isQuickCheckoutEnabled = true;";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickCheckoutVars", quickCheckout, true);
        }

        #endregion

        #region Event Handlers
        //protected void Previouspage_Click(object sender, EventArgs e)
        //{
        //    if(!string.IsNullOrEmpty(ViewState["ReferrerUrl"].ToString()))
        //    {
        //    Response.Redirect(ViewState["ReferrerUrl"].ToString());
        //    }
        //}

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


        #region IPostSearchView

        #region Filter checked state
       
        public bool ShowZeroResults
        {
            set
            {
                this.zeroSearchResultsWrapper.Visible = value;
                this.searchResultHeader.Visible = false;
                this.searchResultFooter.Visible = false;
                this.zeroSearchTrue.Value = value.ToString();
            }
        }

        Control headerUserControl = null;
        public Control HeaderUserControl
        {
            get { return headerUserControl; }
            set { headerUserControl = value; }
        }

        public string ImageGroupName
        {
            get { return imageGroupName; }
            set { imageGroupName = value; }
        }

        public string ImageGroupId
        {
            get { return Request.QueryString["Id"]; }
            set { imageGroupId = Request.QueryString["Id"]; }
        }

        public int ItemsPerPage
        {
            get { return (int)this.searchResultHeader.PageSize; }
            set { itemsPerPage = value; }
        }
       

        #endregion

        #region Lightboxes

        public bool ShowAddToLightboxPopup
        {
            set
            {
                if (Profile.IsAnonymous)
                {
                    //lightboxItems.Visible = false;
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
                addToLightboxPopup.LightboxList = value;
            }
        }

        public string ActiveLightbox
        {
            set
            {
                addToLightboxPopup.ActiveLightbox = value;
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
            get
            {
                string show = stateItems.GetStateItemValue<string>(TermClarifications.Name, TermClarifications.ShowClarificationKey, StateItemStore.Cookie);
                if (string.IsNullOrEmpty(show))
                {
                    return true;
                }
                else
                {
                    return bool.Parse(show);
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

        public List<Clarification> Clarifications
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
            get { return 0; }
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

        #endregion

        #region Other View Interface members

        public List<SearchResultProduct> SearchResultProducts
        {
            set
            {
                products.WebProductList = value;
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

        #region UpdateQuickPic


        protected void updateQuickPick(object sender, EventArgs e)
        {
            QuickPicUpdatePanel.Update();

        }


        #endregion

        #region Helper Methods

        private void RedirectToPage(int index)
        {
            string curUrl = Request.Url.PathAndQuery;
            if (curUrl.Contains("&p="))
            {
                curUrl = curUrl.Substring(0, curUrl.IndexOf("&p="));
            }
            Response.Redirect(curUrl + "&p=" + index.ToString());
        }

        #endregion

         #region IImageGroups Members


        //public string RecentImageURL
        //{
        //    set 
        //    {
        //        this.RFCDHeader.RecentImageURL = value;
        //    }
        //}


        //public string RecentImageId
        //{
        //    set
        //    {
        //        this.RFCDHeader.RecentImageId  = value;
                
        //    }
        //}

        public Decimal RecentImageRadio
        {
            set
            {
                ((IImageGroupHeader)HeaderUserControl).RecentImageRadio = value;
            }
        }

        public bool ShowCaptionButtonAndText
        {
            set
            {
                products.ShowCaptionButtonAndText = value;
            }
        }

        public string CaptionHeader
        {
            get
            {
                return products.CaptionHeader; 
            }
            set
            {
                products.CaptionHeader = value;
            }
        }
        
        public string CaptionText
        {
            set
            {
                if(String.IsNullOrEmpty(value))
                {
                    this.ShowCaptionButtonAndText = false;
                }
                else
                {
                    products.Caption = value;
                }
            }
        }

        public ImageGroupsPresenter Presenter
        {
            get { return _imageGroupsPresenter; }
        }
        #endregion


        
    }
}
