using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Search;
using System.Collections.Generic;
using Corbis.Web.UI.MasterPages;
using Corbis.Web.Utilities;
using System.Web.Configuration;


namespace Corbis.Web.UI.Search
{
    public partial class LightboxItems : CorbisBaseUserControl
    {
        public bool isOverMax = false;
        private RepeaterCommandEventHandler deleteItemHandler;
        private List<Guid> cartItems;
        private List<LightboxDisplayImage> lightboxDetails;

        protected override void OnDataBinding(EventArgs e)
        {            
            if (LightboxId != -1)
            {
                if (Page is SearchResults && ((SearchResults)Page).SearchPresenter != null)
                {
                    lightboxDetails = ((SearchResults)Page).SearchPresenter.LoadLightboxDetails(LightboxId, true, Convert.ToInt32(WebConfigurationManager.AppSettings["LightboxBuddyImageCount"]), out isOverMax);
                    searchBuddyLightbox.DataSource = lightboxDetails;
                }
                else if(Page is Corbis.Web.UI.ImageGroups.ImageGroups && ((Corbis.Web.UI.ImageGroups.ImageGroups )Page)._imageGroupsPresenter != null)
                {
					lightboxDetails = ((Corbis.Web.UI.ImageGroups.ImageGroups)Page)._imageGroupsPresenter.LoadLightboxDetails(LightboxId, true);
                    searchBuddyLightbox.DataSource = lightboxDetails;
					this.ItemCount = ((Corbis.Web.UI.ImageGroups.ImageGroups)Page)._imageGroupsPresenter.GetLightboxImageCount(LightboxId);
                }
                else if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch && ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter != null)
                {
                    lightboxDetails = ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter.LoadLightboxDetails(LightboxId, true);
                    searchBuddyLightbox.DataSource = lightboxDetails;
                }

            }
            base.OnDataBinding(e);
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            //if (isOverMax)
            //{
            //    Corbis.Web.UI.Controls.LinkButton btn =
            //        this.Controls[this.Controls.Count - 1].FindControl("moreThan50") as
            //        Corbis.Web.UI.Controls.LinkButton;
            //    btn.Visible = false;
            //}
            base.OnPreRender(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchBuddyLightbox.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.DoItemDelete);
        }
        #endregion

        protected void searchBuddyLightbox_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
				Guid productUid = (Guid)DataBinder.Eval(e.Item.DataItem, "ProductUid");
				Guid mediaUid = (Guid)DataBinder.Eval(e.Item.DataItem, "MediaUid");
				HtmlGenericControl lboxBlock = (HtmlGenericControl)e.Item.FindControl("lboxBlock");
                //HtmlGenericControl cartbutton = (HtmlGenericControl)e.Item.FindControl("cartbutton");
                Corbis.Web.UI.Controls.LinkButton addToCart = (Corbis.Web.UI.Controls.LinkButton)e.Item.FindControl("addToCart");
                Corbis.Web.UI.Controls.LinkButton expressCheckout = (Corbis.Web.UI.Controls.LinkButton)e.Item.FindControl("expressCheckout");
                Corbis.Web.UI.Controls.CenteredImageContainer imageThumb = (Corbis.Web.UI.Controls.CenteredImageContainer)e.Item.FindControl("imageThumb");
                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)imageThumb.FindControl(imageThumb.ImageID);
                bool isRFCD = (bool)DataBinder.Eval(e.Item.DataItem, "IsRfcd");
                bool isOutline = (bool)DataBinder.Eval(e.Item.DataItem, "IsOutline");
                if (isRFCD)
                {
                    imageThumb.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", (int)Corbis.Image.Contracts.V1.ImageMediaSetType.RFCD, DataBinder.Eval(e.Item.DataItem, "CorbisId"));

                }
                else
                {
					imageThumb.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&puid={1}&caller=lightbox');return false;", DataBinder.Eval(e.Item.DataItem, "CorbisId"), productUid.ToString());
                }
                addToCart.Visible = Profile.IsECommerceEnabled;
                imageThumb.Attributes.CssStyle.Add("background", "#262626");
                imageThumb.Attributes.CssStyle.Add("cursor", "pointer");

                if (IsOfferingInCart((Guid)DataBinder.Eval(e.Item.DataItem, "MediaUid")))
                {
                    lboxBlock.Attributes.Add("class", "lightboxBlock inCart");
                    //cartbutton.Attributes.Add("class", "ICN_cart ICN_cart_selected");
                    addToCart.Enabled = false;
                }
                else
                {
                    lboxBlock.Attributes.Add("class", "lightboxBlock");
                    //cartbutton.Attributes.Add("class", "ICN_cart");
                    addToCart.Enabled = true;
                }

                if (!(Profile.IsFastLaneEnabled && Profile.IsECommerceEnabled && !isRFCD && !isOutline))
                {
                    expressCheckout.Visible = false;
                }
                else
                {
                    expressCheckout.OnClientClick = string.Format("CorbisUI.ExpressCheckout.Open('{0}','{1}', {2});return false;", DataBinder.Eval(e.Item.DataItem, "CorbisId"), productUid.ToString(), LightboxId.ToString());
                }
                lboxBlock.Attributes.Add("mediaUid", mediaUid.ToString());
				lboxBlock.Attributes.Add("corbisID", DataBinder.Eval(e.Item.DataItem, "CorbisId").ToString());

                Decimal ratio = (Decimal)DataBinder.Eval(e.Item.DataItem, "AspectRatio");
                if (ratio > 1)
                    img.Width = Unit.Parse("90px");
                else
                    img.Height = Unit.Parse("90px");
               
                
                Corbis.Web.UI.Controls.HoverButton btnClose = (Corbis.Web.UI.Controls.HoverButton)e.Item.FindControl("btnClose");
				btnClose.CommandArgument = productUid.ToString() + "|" + mediaUid.ToString();
            }
        }

        private void DoItemDelete(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            Guid productUId = new Guid(e.CommandArgument.ToString().Split('|')[0]);
            if (Page is SearchResults)
            {
                ((SearchResults)Page).SearchPresenter.DeleteImageFromLightbox(LightboxId, productUId);
            }
            else if (Page is Corbis.Web.UI.ImageGroups.ImageGroups)
            {
                ((Corbis.Web.UI.ImageGroups.ImageGroups)Page)._imageGroupsPresenter.DeleteImageFromLightbox(LightboxId, productUId);
            }
            else if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch)
            {
                ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter.DeleteImageFromLightbox(LightboxId, productUId);
            }

            DataBind();

			if (deleteItemHandler != null)
			{
				deleteItemHandler(source, e);
			}
        }

        protected void AddToCart_Click(object sender, CommandEventArgs e)
        {
            LinkButton addTocart = (LinkButton)sender;
            if (Page is SearchResults)
            {
                ((SearchResults)Page).SearchPresenter.AddToCart(new Guid(e.CommandArgument.ToString()));
                ((NoSearchBar)Page.Master).GlobalNav.UpdateCartCount();

            }
            else if (Page is Corbis.Web.UI.ImageGroups.ImageGroups)
            {
                ((Corbis.Web.UI.ImageGroups.ImageGroups)Page)._imageGroupsPresenter.AddToCart(new Guid(e.CommandArgument.ToString()));
                ((Master)Page.Master).GlobalNav.UpdateCartCount();

            }
            else if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch)
            {
                ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter.AddToCart(new Guid(e.CommandArgument.ToString()));
                ((Master)Page.Master).GlobalNav.UpdateCartCount();

            }

            //UpdatePanel cartCount = (UpdatePanel)((Corbis.Web.UI.Navigation.GlobalNav)Page.Master.FindControl("globalNav")).FindControl("cartUpdate");
            //cartCount.Update();

        }

        public int LightboxId
        {
            set
            {
                ViewState["LightboxId"] = value;
                //Search result does loading on client side now.
                if (!(Page is Corbis.Web.UI.Search.SearchResults)) DataBind();
            }
            get
            {
                return ViewState["LightboxId"] != null ? (int)ViewState["LightboxId"] : -1;
            }

        }

		public int ItemCount
		{
			get
			{
				return ViewState["ItemCount"] != null ? (int)ViewState["ItemCount"] : searchBuddyLightbox.Items.Count;
			}
			set
			{
				ViewState["ItemCount"] = value;
			}
		}

        private bool IsOfferingInCart(Guid offeringUid)
        {
            return CartItems.Contains(offeringUid);
        }

        private List<Guid> CartItems
        {
            get
            {
                if (cartItems == null)
                {
                    cartItems = new List<Guid>();
                    if (Page is SearchResults)
                    {
                        foreach (CartDisplayImage item in ((SearchResults)Page).SearchPresenter.GetCartItems())
                        {
                            cartItems.Add(item.MediaUid);
                        }
                    }
                    else if (Page is Corbis.Web.UI.ImageGroups.ImageGroups)
                    {
                        foreach (CartDisplayImage item in ((Corbis.Web.UI.ImageGroups.ImageGroups)Page)._imageGroupsPresenter.GetCartItems())
                        {
                            cartItems.Add(item.MediaUid);
                        }
                    }
                    else if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch)
                    {
                        foreach (CartDisplayImage item in ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter.GetCartItems())
                        {
                            cartItems.Add(item.MediaUid);
                        }
                    }
                }
                return cartItems;
            }
        }
		public event RepeaterCommandEventHandler DeleteItem
		{
			add { deleteItemHandler += value; }
			remove { deleteItemHandler -= value; }
		}
    }
}