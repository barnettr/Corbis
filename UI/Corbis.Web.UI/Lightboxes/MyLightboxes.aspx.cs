using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Search;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;
using System.Web;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Lightboxes
{
    public partial class MyLightboxes : CorbisBasePage, IMyLightboxesView
    {
		protected const int maxImagesForCopy = 1000;

		private List<Lightbox> lightboxes;
        private LightboxesPresenter presenter;
        private StateItemCollection stateItems;
        private List<LightboxDisplayImage> products;
        private List<string> cartItems;
        private List<string> quickPicList;
        public bool IsEmpty
        {
            get { return empty.Visible; }
            set { empty.Visible = value; }
        }

        public List<Lightbox> Lightboxes
        {
            get { return lightboxes; }
            set { lightboxes = value; }
        }
        public DateTime ModifiedDate
        {
            get { return DateTime.Parse(this.Modified.Text); }
            set { this.Modified.Text = value.ToShortDateString(); }
        }
        public DateTime CreatedDate
        {
            get { return DateTime.Parse(this.Created.Text); }
            set { this.Created.Text = value.ToShortDateString(); }
        }

        public string OwnerUsername
        {
            get { return this.Owner.Text; }
            set { this.Owner.Text = Server.HtmlEncode(value); }
        }

        public string Client
        {
            get { return this.ClientName.Text; }
            set {
				if (value != null)
				{
                    this.ClientName.Text = Server.HtmlEncode(value);
					this.txtClientName.Text = value;
				}
				else
				{
					this.ClientName.Text = this.txtClientName.Text = string.Empty;
				}
            }
        }

        public string Notes
        {
            get { return Note1.Text; }
            set
            {
                String note = string.Empty;

                if (value != null)
                {
                    txtNotes.Text = value;
                    System.Text.RegularExpressions.MatchCollection charColl = System.Text.RegularExpressions.Regex.Matches(txtNotes.Text, @" ");
                    if (charColl.Count == 0 && !string.IsNullOrEmpty(txtNotes.Text))
                    {
                        int y = 100;
                        note = txtNotes.Text;
                        for (int i = 0; i < txtNotes.Text.Length / 100; i++)
                        {
                            note = note.Insert(y, " ");
                            y += 100;
                        }
                    }
                    else
                    {
                        note = txtNotes.Text;
                    }

                    this.Note1.Text = txtNotes.Text = note;
                }
                else if (note.Length > 2000 && !string.IsNullOrEmpty(note))
                {
                    this.Note1.Text = txtNotes.Text = StringHelper.Truncate(note, 2000);
                }
                else
                {
                    this.Note1.Text = txtNotes.Text = note;
                }
            }
         }


        public string Shared
        {
            get
            {
                return SharedBy.Text;
            }
            set
            {
                string shareby = value.Trim(',');
                if (!string.IsNullOrEmpty(shareby))
                {
                    shareby = shareby.Replace(",", ",<br/>");

                    this.SharedBy.Text = shareby; 
                    this.lbShared.Visible = true;
                }
                else
                {
                    this.SharedBy.Text = "";
                    this.lbShared.Visible = false;
                }
            }
        }

        public List<LightboxDisplayImage> Products
        {
            get {
                return products;
            }
            set
            {
                products = value;
                lightboxProducts.WebProductList = value;
            }
        }

        public List<string> CartItems
        {
            get { return cartItems; }
            set 
            {
                cartItems = value;
                this.lightboxProducts.CartItems = value;
            }
        }

        public List<string> QuickPicList
        {
            get { return quickPicList; }
            set
            {
                quickPicList = value;
                this.lightboxProducts.QuickPicList = value;
            }
        }

        public int PageSize
        {
            get
            {
                return (int)this.searchResultHeader.PageSize;
            }
            set
            {
                this.searchResultHeader.PageSize = (ItemsPerPage) Enum.Parse(typeof (ItemsPerPage), value.ToString());
                this.searchResultFooter.PageSize = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), value.ToString());
           }
        }

        public int TotalRecords
        {
            get 
            {
                return this.searchResultHeader.TotalRecords;
            }
            set
            {
                
                this.searchResultFooter.PageSize = searchResultHeader.PageSize;
                
                this.searchResultHeader.TotalRecords = value;
                this.searchResultFooter.TotalRecords = value;
                this.searchResultHeader.TotalSearchHitCount = value;
                this.searchResultFooter.TotalSearchHitCount = value;
                this.searchResultHeader.CurrentPageHitCount = Products.Count;
                this.searchResultFooter.CurrentPageHitCount = Products.Count;
				this.copyItemsButton.Enabled = true;

				if (value > 0 && value <= maxImagesForCopy)
				{	
					this.copyItemsButton.ToolTip = "";
					this.copyItemsButton.CssClass = this.copyItemsButton.CssClass.Replace(" disabled", "");
					this.copyItemsButtonDiv.Attributes["class"] = this.copyItemsButtonDiv.Attributes["class"].Replace(" disabled", "");
				}
				else
				{
					if (!this.copyItemsButton.CssClass.Contains(" disabled")) this.copyItemsButton.CssClass += " disabled";
					if (!this.copyItemsButtonDiv.Attributes["class"].Contains(" disabled")) this.copyItemsButtonDiv.Attributes["class"] += " disabled";
					if (value == 0) this.copyItemsButton.ToolTip = GetLocalResourceObject("CopyNoImage").ToString();
					if (value > maxImagesForCopy) this.copyItemsButton.ToolTip = GetLocalResourceObject("CopyTooManyImages").ToString();
				}
			}
        }

        public int LightboxId
        {
            get { return int.Parse(selectedLightbox.Value); }
        }
        
        
		public Guid LightboxUid
		{
			get
			{
				return new Guid(lightboxUid.Value);
			}
			set
			{
				lightboxUid.Value = value.ToString();
			}
		}
		public string LightboxName
		{
			get
			{
				return LightboxNameTitle.Text;
			}
			set
			{
				LightboxNameTitle.Text = Server.HtmlEncode(value);
			}
		}
        public Guid NotesUid
        {
            get
            {
                return new Guid(this.notesUid.Text);
            }
            set
            {
                notesUid.Text = value.ToString();
            }
        }

		public LightboxTreeSort LightboxTreeSortBy
		{
			get
			{
				switch (sortBy.SelectedValue)
				{
					case "name":
						return LightboxTreeSort.Name;
					default:
						return LightboxTreeSort.Date;
				}
			}
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.ClientScript.RegisterClientScriptInclude("LightboxJS", SiteUrls.LightboxScript);
            Page.ClientScript.RegisterClientScriptInclude("SearchJs", SiteUrls.SearchScript);
            Page.ClientScript.RegisterClientScriptInclude("AddToCartScript", SiteUrls.AddToCartScript);
            Page.ClientScript.RegisterClientScriptInclude("ErrorTrackerScript", SiteUrls.ErrorTrackerScript);
          
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Lightboxes/LightboxScriptService.asmx"));

			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MyLightboxes, "MyLightboxesCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Tooltips, "TooltipsCSS");
            presenter = new LightboxesPresenter(this);
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);

			
        }

        private void HandleProfileBasedViewSettings()
        {
            this.trustImageCOFF1.Visible = Profile.CountryCode == UScountryCode;
            this.trustImageCOFF2.Visible = Profile.CountryCode == UScountryCode;
        }        
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleProfileBasedViewSettings();
            contactMessage.Text = String.Format(GetLocalResourceString("contactMessage"), Corbis.Web.UI.SiteUrls.CustomerService);
            if (!Profile.IsAnonymous)
            {
				ScriptManager scriptManager = ScriptManager.GetCurrent(this);
				bool isAsyncPostback = scriptManager.IsInAsyncPostBack;

                //skip over this stuff for ajax call.
                if (!isAsyncPostback)
                {
					OutputEnumClientScript<LicenseModel>(Page);

                    if (!IsPostBack)
                    {
                        sortBy.SelectedValue = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey, StateItemStore.Cookie);
                    }

                    BuildLightboxTree(sender, e);
                }
               
                // Display Coff option on lightbox menu.
                if (Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoffRF) ||
                    Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoffRM) ||
                    Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoffOutline))
                {
                    coffItemsButtonDiv.Visible = true;
                }
                else
                {
                    coffItemsButtonDiv.Visible = false;
                }

				if (IsPostBack && Request.Form["__EVENTTARGET"] == quickpicField.ClientID)
				{
					QuickPicUpdatePanel.Update();
				}

				if (!isAsyncPostback || 
						(isAsyncPostback && 
							(Request.Form["__EVENTTARGET"] == "ctl00_mainContent_selectedLightbox" || 
							Request.Form["__EVENTTARGET"] == "ctl00$mainContent$sortBy" ||
							scriptManager.AsyncPostBackSourceElementID == refreshLightboxDetails.UniqueID)))
				{
					presenter.GetLightboxHeaderDetails((string)GetLocalResourceObject("readonly"));
					HeaderPanel.Update();
				}
            }
            else
            {
                Response.Redirect(SiteUrls.GetAuthenticateUrl());
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
            string tooltip = string.Format(
                                @"<div style='margin-left:10px;text-align:left;'><div class='rfFileSizesItem'>{0}</div><div class='rfFileSizesItem'>{1}</div><div class='rfFileSizesItem'>{2}</div><div class='rfFileSizesItem'>{3}</div><div class='rfFileSizesItem'>{4}</div></div><div class='footerMessageRfFileSizes'>{5}</div>", (string)GetLocalResourceObject("rfWebSize"), (string)GetLocalResourceObject("rfSmallSize"), (string)GetLocalResourceObject("rfMediumSize"), (string)GetLocalResourceObject("rfLargeSize"), (string)GetLocalResourceObject("rfXLargeSize"), (string)GetLocalResourceObject("footerMessage"));

                this.showFileSizeModal.Attributes["rel"] = tooltip;
                this.showFileSizeModal.Attributes["title"] = (string)GetLocalResourceObject("fileSizeModal");
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

        protected void searchResult_PageCommand(object sender, PagerEventArgs e)
        {
            int index = e.PageIndex;
            CurrentPageNumber = index;  
            StateItem<int> currentPageNumber = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, CurrentPageNumber, StateItemStore.AspSession, StatePersistenceDuration.Session);
            StateItem<int> currentLightboxId = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, LightboxId, StateItemStore.AspSession, StatePersistenceDuration.Session);
            stateItems.SetStateItem(currentPageNumber);
            stateItems.SetStateItem(currentLightboxId);
            GetLightboxDetails(sender, e);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            presenter.DeleteProductFromLightbox(LightboxId, new Guid(selectedProduct.Value));
            BuildLightboxTree(sender, e);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDeleteModal", "HideModal('modalDeleteTemplate');ClearightboxIdForCopy();LoadActiveLightbox();", true);
        }  

		protected void sortBy_Changed(object sender, EventArgs e)
        {
			stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey, sortBy.SelectedValue, StateItemStore.Cookie));
        }

        protected void GetChangedLightboxDetails(object sender, EventArgs e)
        {
            CurrentPageNumber = 1;
            GetLightboxDetails(sender, e);
        }

        protected void GetLightboxDetails(object sender, EventArgs e)
        {
            stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, selectedLightbox.Value, StateItemStore.Cookie));

            if (string.IsNullOrEmpty(selectedLightbox.Value))
            {
                selectedLightbox.Value = "0";
				sendButton.Enabled = false;
            }

			int storedPageNumber = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, StateItemStore.AspSession);

			//if it's a post back or no stored page
			if (IsPostBack || storedPageNumber == 0)
            {
                presenter.LoadLightboxDetails(false);
            }
            else
            {
                presenter.LoadLightboxDetails(LightboxId, storedPageNumber, false);
            }
            lightboxProducts.QuickPicList = QuickPicList;
            lightboxProducts.CartItems = CartItems;

			//refresh the tooltips and lightbox info, but only on ajax post back
			ScriptManager scriptManager = ScriptManager.GetCurrent(this);
			if (scriptManager.IsInAsyncPostBack)
			{
				ScriptManager.RegisterStartupScript(this, this.GetType(), "toolTipRefresh", "thumbTips = null; registerLightboxTooltips();", true);

				//refresh lightbox information
				if (scriptManager.AsyncPostBackSourceElementID == selectedLightbox.ClientID || scriptManager.AsyncPostBackSourceElementID == refreshLightboxDetails.UniqueID)
				{
					string itemCountText = String.Format((TotalRecords == 1 ? lightboxTree.OneItemCountFormat : lightboxTree.ItemCountFormat), TotalRecords);
					string modifiedDate = String.Format(lightboxTree.UpdatedFormat, ModifiedDate.ToShortDateString());

					ScriptManager.RegisterStartupScript(this, this.GetType(), "updateLightboxInfo", String.Format("CorbisUI.Lightbox.Handler.updateLightboxInfo('{0}', '{1}', '{2}', '{3}');", LightboxId, itemCountText, modifiedDate, lightboxTree.CountsSeparator), true);
				}
			}			
        }

        protected void Pager_PageCommand(object sender, PagerEventArgs e)
        {
            this.CurrentPageNumber = e.PageIndex;
            GetLightboxDetails(sender, e);
        }
        #region UpdateQuickPic


        protected void updateQuickPick(object sender, EventArgs e)
        {
            QuickPicUpdatePanel.Update();

        }


        #endregion

		protected void DeleteLightbox(object sender, EventArgs e)
		{
			if (!Profile.IsAnonymous)
			{
				List<int> lightboxIds = new List<int>(1);
				lightboxIds.Add(LightboxId);

				presenter.DeleteLightbox(Profile.UserName, lightboxIds);
				stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, "", StateItemStore.Cookie));

				Response.Redirect(SiteUrls.MyLightBoxes);
			}
			else
			{
                Response.Redirect(SiteUrls.GetAuthenticateUrl());
			}
		}

		protected void CreateLightbox(object sender, EventArgs e)
		{
			Response.Redirect(SiteUrls.Lightboxes);
		}
        public int CurrentPageNumber
        {
            get { return searchResultHeader.CurrentPage; }
            set
            {
                searchResultHeader.CurrentPage = value;
                searchResultFooter.CurrentPage = value;
            }
        }
        protected void searchResultHeader_PageSizeCommand(object sender, PageSizeEventArgs e)
        {
            CurrentPageNumber = 1;
            this.PageSize = (int)e.PageSize;
            GetLightboxDetails(sender, e);
        }

        private void BuildLightboxTree(object sender, EventArgs e)
        {
            presenter.LoadLightboxTreePane(Profile.UserName);
            if (Profile.IsQuickPicEnabled)
            {
                SBT_quickpic.Visible = SBT_filters.Visible = true;
            }
            else
            {
                SBT_quickpic.Visible = SBT_filters.Visible = false;
            }

            if (Lightboxes != null && Lightboxes.Count > 0)
            {
                lightboxTree.LightboxDirectory = BuildLightboxDir(Lightboxes);

                int lightboxId;
                bool hasCookie = int.TryParse(stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie), out lightboxId);
                bool isSharedBy;

                if (!hasCookie || !lightboxTree.LightboxDirectory.ContainsKey(lightboxId))
                {
                    lightboxId = Lightboxes[0].LightboxId;
                    isSharedBy = !string.IsNullOrEmpty(Lightboxes[0].SharedBy);

                    //It is not a valid lightbox. Clear entry from the cookie.
                    if (hasCookie)
                    {
                        StateItem<string> stateItem = new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, null, StateItemStore.Cookie);
                        stateItems.DeleteStateItem<string>(stateItem);
                    }
                }
                else
                {
                    isSharedBy = !string.IsNullOrEmpty(lightboxTree.LightboxDirectory[lightboxId].SharedBy);
                }

                selectedLightbox.Value = lightboxId.ToString();

                GetLightboxDetails(sender, e);
                lightboxTree.SelectedLightboxId = lightboxId;
                lightboxTree.SelectedLightboxShareBy = isSharedBy;
            }
            
            lightboxTree.DataSource = Lightboxes;
            lightboxTree.DataBind();
        }

        private Dictionary<int, Lightbox> BuildLightboxDir(List<Lightbox> lightboxList)
        {
            Dictionary<int, Lightbox> lightbocDict = new Dictionary<int, Lightbox>();
            foreach (var lightbox in lightboxList)
            {
                lightbocDict[lightbox.LightboxId] = lightbox;
                if (lightbox.LightboxChildren != null && lightbox.LightboxChildren.Count > 0)
                {
                    Dictionary<int, Lightbox> childrenLightbocDict = BuildLightboxDir(lightbox.LightboxChildren);
                    foreach (var childLightbox in childrenLightbocDict)
                    {
                        if (childLightbox.Value != null)
                        {
                            lightbocDict[childLightbox.Key] = childLightbox.Value;
                        }
                    }
                }
            }

            return lightbocDict;
        }
    }
}
