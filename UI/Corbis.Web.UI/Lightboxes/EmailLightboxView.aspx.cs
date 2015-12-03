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

namespace Corbis.Web.UI.Lightboxes
{
    public partial class EmailLightboxView : CorbisBasePage, IEmailLightboxView
    {
        private LightboxesPresenter presenter;
        private StateItemCollection stateItems;
        private List<LightboxDisplayImage> products;
        Guid selectedUid = Guid.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.ClientScript.RegisterClientScriptInclude("LightboxJS", SiteUrls.LightboxScript);
            Page.ClientScript.RegisterClientScriptInclude("SearchJs", SiteUrls.SearchScript);
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Lightboxes/LightboxScriptService.asmx"));

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MyLightboxes, "MyLightboxesCSS");
           
            presenter = new LightboxesPresenter(this);
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           // TODO: GetLightboxUid from Quesrystring
            this.lightboxUid.Value = Request.QueryString["Uid"];
            if (!presenter.GetEmailedLightbox())
                Response.Redirect(SiteUrls.Home);
            else
                presenter.GetEmailedLightbox();
        }

        protected void searchResult_PageCommand(object sender, PagerEventArgs e)
        {
            int index = e.PageIndex;
            CurrentPageNumber = index;
            StateItem<int> currentPageNumber = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, CurrentPageNumber, StateItemStore.AspSession, StatePersistenceDuration.Session);
            StateItem<int> currentLightboxId = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, int.Parse("7803056"), StateItemStore.AspSession, StatePersistenceDuration.Session);
            stateItems.SetStateItem(currentPageNumber);
            stateItems.SetStateItem(currentLightboxId);
            GetLightboxDetails(sender, e);
        }
       

        protected void GetChangedLightboxDetails(object sender, EventArgs e)
        {
            CurrentPageNumber = 1;
            GetLightboxDetails(sender, e);
        }

        protected void GetLightboxDetails(object sender, EventArgs e)
        {  
            stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, string.Empty, StateItemStore.Cookie));

            //refresh the tooltips, but only on ajax post back
            if (ScriptManager.GetCurrent(this).IsInAsyncPostBack && stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPreviewKey, StateItemStore.Cookie) == SearchItemPreview.previewOn.GetHashCode())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toolTipRefresh", "thumbTips = null; registerLightboxTooltips();", true);
            }

			searchResultHeader.CurrentPage = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, StateItemStore.AspSession);
            if (!presenter.GetEmailedLightbox())
                Response.Redirect(SiteUrls.Home);
            else
                presenter.GetEmailedLightbox();
         
        }

        protected void Pager_PageCommand(object sender, PagerEventArgs e)
        {
            this.CurrentPageNumber = e.PageIndex;
            GetLightboxDetails(sender, e);
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

        #region IEmailLightboxView members 

        public string LightboxName
        {
            get { return this.LightboxNameTitle.Text; }
            set { this.LightboxNameTitle.Text = Server.HtmlEncode(value); }
        }

        public string ModifiedDate
        {
            get { return this.Modified.Text; }
            set { this.Modified.Text = Server.HtmlEncode(value); }
        }

        public string CreatedDate
        {
            get { return this.Created.Text; }
            set { this.Created.Text = Server.HtmlEncode(value); }
        }

        public string OwnerUsername
        {
            get { return this.Owner.Text; }
            set { this.Owner.Text = Server.HtmlEncode(value); }
        }

        public string Client
        {
            get { return this.ClientName.Text; }
            set { this.ClientName.Text = Server.HtmlEncode(value); }
        }

        public Guid LightboxUid
        {
            get
            {
                if (GuidHelper.TryParse(this.lightboxUid.Value, out selectedUid))
                {
                    return new Guid(lightboxUid.Value);
                }
                else
                {
                    Response.Redirect(SiteUrls.Home);
                    return selectedUid;
                }
            }
        }

        public int LightboxId
        {
            get 
            {
                int returnValue;
                int.TryParse(lightboxId.Value, out returnValue);
                return returnValue;
            }
            set { lightboxId.Value = value.ToString(); }
        }

        public string Notes
        {
            get { return Note.Text; }
            set { this.Note.Text = Server.HtmlEncode(value); }
        }

        public List<LightboxDisplayImage> Products
        {
            get
            {
                return products;
            }
            set
            {
               
                    products = value;
                    lightboxProducts.WebProductList = value;
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
                this.searchResultHeader.PageSize = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), value.ToString());
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
                if (this.Products != null)
                {
                    this.searchResultHeader.CurrentPageHitCount = Products.Count;
                    this.searchResultFooter.CurrentPageHitCount = Products.Count;
                }
                else
                {
                    this.searchResultHeader.CurrentPageHitCount = 0;
                    this.searchResultFooter.CurrentPageHitCount = 0;
                }


            }
        }

        #endregion
    }
}
