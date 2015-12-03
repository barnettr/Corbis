using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Search;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class LightboxPanel : CorbisBaseUserControl, IMyLightboxesView
    {
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

        public List<LightboxDisplayImage> Products
        {
            get
            {
                return products;
            }
            set
            {
                products = value;
               // lightboxProducts.WebProductList = value;
            }
        }
        public List<string> CartItems
        {
            get { return cartItems; }
            set
            {
                cartItems = value;
                //this.lightboxProducts.CartItems = value;
            }
        }

        public List<string> QuickPicList
        {
            get { return quickPicList; }
            set
            {
                quickPicList = value;
              //  this.lightboxProducts.QuickPicList = value;
            }
        }

        public int PageSize
        {
            get
            {
               return 1;
            }
            set
            {
               // this.searchResultHeader.PageSize = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), value.ToString());
               // this.searchResultFooter.PageSize = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), value.ToString());
            }
        }
        public int TotalRecords
        {
            get
            {
                return 1; // this.searchResultHeader.TotalRecords;
            }
            set
            {}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Profile.IsAnonymous)
            {
                ScriptManager manager = ScriptManager.GetCurrent(Page);
                //skip over this stuff for ajax call.
                if (!manager.IsInAsyncPostBack)
                {
                      if (!IsPostBack)
                     {
                    sortBy.SelectedValue = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey, StateItemStore.Cookie);
                    }

                    BuildLightboxTree(sender, e);
                }
            }
            else
            {
                Response.Redirect(SiteUrls.SignIn);
            }

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MyLightboxes, "MyLightboxesCSS");

            presenter = new LightboxesPresenter(this);
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
        }
       

        public int CurrentPageNumber
        {
            get
            {
                return 1; 
            }
            set
            {
               // searchResultHeader.CurrentPage = value;
               // searchResultFooter.CurrentPage = value;
            }
        }
        public LigtboxTreeSort LightboxTreeSortBy
        {
            get
            {
                switch (sortBy.SelectedValue)
                {
                    case "name":
                        return LigtboxTreeSort.Name;
                    default:
                        return LigtboxTreeSort.Date;
                }
            }
        }
        protected void sortBy_Changed(object sender, EventArgs e)
        {
            stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey, sortBy.SelectedValue, StateItemStore.Cookie));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            presenter.DeleteProductFromLightbox(int.Parse(selectedLightbox.Value), new Guid(selectedProduct.Value));
            BuildLightboxTree(sender, e);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDeleteModal", "HideModal('modalDeleteTemplate');InitializeToggleImage();LoadActiveLightbox();", true);
        }

        protected void DeleteLightbox(object sender, EventArgs e)
        {
            if (!Profile.IsAnonymous)
            {
                List<int> lightboxIds = new List<int>(1);
                lightboxIds.Add(int.Parse(selectedLightbox.Value));

                presenter.DeleteLightbox(Profile.UserName, lightboxIds);
                stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, "", StateItemStore.Cookie));
                Response.Redirect(SiteUrls.ImageGroups);
            }
            else
            {
                Response.Redirect(SiteUrls.SignIn);
            }
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

            List<Lightbox> lbList = presenter.GetLightboxList(Profile.UserName);
            if (Lightboxes != null && Lightboxes.Count > 0)
            {
                string lightboxId = stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);

                if (String.IsNullOrEmpty(lightboxId))
                {
                    lightboxId = lightboxes[0].LightboxId.ToString();
                }

               selectedLightbox.Value = lightboxId;
               lightboxTree.SelectedLightboxId = int.Parse(lightboxId);
            }
            lightboxTree.FlatLightboxTree = lbList;
            lightboxTree.DataSource = Lightboxes;
            lightboxTree.DataBind();
        }
    }
}