using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.MasterPages;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Authentication;
using System.Collections.Generic;
using System.Globalization;
using Corbis.Search.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using AjaxControlToolkit;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.Entities;
using Corbis.Image.Contracts.V1;
using LinkButton = System.Web.UI.WebControls.LinkButton;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Lightboxes
{
    public partial class Products : CorbisBaseUserControl, IPriceImageLink
    {
        private const int tooltipImgHeight = 256;
        private const int MAXIUMCAPTIONLENGTH = 160;
        protected System.Web.UI.HtmlControls.HtmlAnchor priceImageLink;
        protected System.Web.UI.HtmlControls.HtmlImage pricingImage;
        protected Corbis.Web.UI.Controls.CenteredImageContainer thumbWrap;
        protected System.Web.UI.WebControls.Label searchCategory;
        protected System.Web.UI.WebControls.Label marketingCollection;
        private StateItemCollection stateItems;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            stateItems = new StateItemCollection(HttpContext.Current);
            ShowDownloadingProhibited = Profile.IsChinaUser;
        }
   
    public  string LightboxId   
    {
        get
        {           
             return  stateItems.GetStateItemValue<string>("LightboxCart", "lightboxId", StateItemStore.Cookie);          
          
        }       

    }

        public List<LightboxDisplayImage> WebProductList
        {
            set
            {
                if (value != null && value.Count > 0)
                {
                    results.Visible = true;
                    emptyLightboxMessage.Visible = false;
                    results.DataSource = value;
                    results.DataBind();
                }
                else
                {
                    results.Visible = false;
                    emptyLightboxMessage.Visible = true;
                    // Temporary solution for centering the Text
                    emptyLightboxMessage.Text = "<b><div class='emptyLightboxMessage'> " + (string)GetLocalResourceObject("emptyLightboxMessage.Text") + "</div></b>";           

                }
            }
        }

        private List<string> quickPicList;
        public List<string> QuickPicList
        {
            get { return quickPicList; }
            set { quickPicList = value; }
        }

        private List<string> cartItems;
        public List<string> CartItems
        {
            get { return cartItems; }
            set { cartItems = value; }
        }

        //TOSO: This logic belongs in the presenter
        protected void Result_ItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || 
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
				LightboxDisplayImage currentItem = e.Item.DataItem as LightboxDisplayImage;
                if (currentItem != null)
                {
					CenteredImageContainer thumbWrapControl = e.Item.FindControl("thumbWrap") as CenteredImageContainer;
					thumbWrapControl.ImgUrl = currentItem.Url128;
					thumbWrapControl.Size = 128;
					thumbWrapControl.Ratio = currentItem.AspectRatio;
                    HtmlAnchor cartLinkControl = e.Item.FindControl("cartLink") as HtmlAnchor;
                    Corbis.Web.UI.Controls.HyperLink hyperUpdateUse = e.Item.FindControl("hyperUpdateUse") as Corbis.Web.UI.Controls.HyperLink;
					thumbWrapControl.AltText = String.Format("{0} - {1}", currentItem.CorbisId,  StringHelper.ConvertBracketsToItalic(currentItem.Title));
                    if(!currentItem.IsRfcd)
                    {
						thumbWrapControl.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&puid={1}&caller=lightbox');return false;", currentItem.CorbisId, currentItem.ProductUid);
                        cartLinkControl.Attributes.Add("onclick", "javascript:(new CorbisUI.Lightbox.ProductBlock('" + currentItem.CorbisId + "')).addProductToCart();return false;");
                    }
                    else
                    {
                        thumbWrapControl.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", (int)ImageMediaSetType.RFCD, currentItem.CorbisId);
                        cartLinkControl.Attributes.Add("onclick", "javascript:(new CorbisUI.Lightbox.ProductBlock('" + currentItem.CorbisId + "')).addRfcdToCart();return false;");
                    }

                    HtmlContainerControl productBlock = (HtmlContainerControl)e.Item.FindControl("productBlock");
 					productBlock.Attributes["licenseModel"] = currentItem.LicenseModel.ToString();
                    productBlock.Attributes["productUID"] = currentItem.ProductUid.ToString();
                    productBlock.Attributes["mediaUID"] = currentItem.MediaUid.ToString();
                    productBlock.Attributes["corbisID"] = currentItem.CorbisId;

					productBlock.Attributes["class"] += string.Format(@" {0}", currentItem.LicenseModel);

                    if (Page is Corbis.Web.UI.Lightboxes.EmailLightboxView) 
                    {
                        ((HtmlControl)e.Item.FindControl("icons")).Visible = ((HtmlControl)e.Item.FindControl("permissionsWrap")).Visible = false;
                        ((Corbis.Web.UI.Controls.HoverButton)e.Item.FindControl("btnClose")).Visible = false;
                        ((Corbis.Web.UI.Controls.HoverButton)e.Item.FindControl("btnNote")).Attributes.CssStyle.Add("top", "3px");
                        if (Profile.IsAnonymous && !Profile.IsAuthenticated)
                        {
                            if (!currentItem.IsRfcd)
                            {
								thumbWrapControl.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&caller=lightbox');return false;", currentItem.CorbisId);
                            }
                            else
                            {
                                thumbWrapControl.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", (int)ImageMediaSetType.RFCD, currentItem.CorbisId);
                            }
                        }
                    }
                    else
                    {
                        bool isInCart = cartItems.Contains(currentItem.CorbisId);
                        bool isInQuickPic = QuickPicList.Contains(currentItem.CorbisId);

                        if (isInCart || isInQuickPic)
                        {
                            productBlock.Attributes["class"] += " ProductSelected";
                        }

					    if (currentItem.IsRfcd)
					    {
						    productBlock.Attributes["isRFCD"] = "true";
					    }
                    
                        //Icons
                        ((HtmlControl)e.Item.FindControl("iconSimilarBlock")).Visible = currentItem.HasRelatedImages && !currentItem.IsRfcd;
                        ((HtmlControl)e.Item.FindControl("iconQuickpicBlock")).Visible = Profile.IsQuickPicEnabled;
                        ((HtmlControl)e.Item.FindControl("iconPricingBlock")).Visible = !currentItem.IsRfcd;
                        ((HtmlControl)e.Item.FindControl("iconExpressCheckoutBlock")).Visible = !currentItem.IsRfcd && !currentItem.IsOutLine && Profile.IsECommerceEnabled && Profile.IsFastLaneEnabled;

                        PricingPresenter.InitializePriceImageLink(this, currentItem);
                        HtmlAnchor priceImageLinkControl = e.Item.FindControl("priceImageLink") as HtmlAnchor;
                        HtmlImage priceImageLink  = (HtmlImage)e.Item.FindControl("pricingImage");
                        priceImageLinkControl.Visible = ShowPricingLink;
                        

                        priceImageLinkControl.Attributes.Add("onclick", PricingNavigateUrl);
                        ((HtmlImage)e.Item.FindControl("pricingImage")).Attributes["title"] = PricingAltText;

                        HtmlControl iconCartBlock = ((HtmlControl)e.Item.FindControl("iconCartBlock"));
                        if (Profile.IsECommerceEnabled)
                        {
                            iconCartBlock.Visible = true;
                            if (isInCart)
                            {
                                iconCartBlock.Attributes["class"] += String.Format(" {0}_selected", iconCartBlock.Attributes["class"]);
                            }

                        }
                        else
                        {
                            iconCartBlock.Visible = false;
                            
                            ((HtmlGenericControl)e.Item.FindControl("requestPriceDiv")).Visible = true;

                             // Request price form opening when price image icon is clicked for all non-ecommerce enabled lightbox images
                            if (((HtmlControl)e.Item.FindControl("iconPricingBlock")).Visible == true)
                            {
                                priceImageLinkControl.Attributes.Add("onclick", "javascript:CorbisUI.RequestPricing.showModal('" + currentItem.CorbisId + "'," + this.LightboxId + ",'Lightbox');return false;");
                                ((HtmlImage)e.Item.FindControl("pricingImage")).Attributes["title"] = GetLocalResourceObject("priceImage.ReqPriceText").ToString();
                               // priceImageLink.Alt = GetLocalResourceObject("priceImage.ReqPriceText").ToString();
                            } 
                        }

                        HtmlControl iconQuickPicBlock = ((HtmlControl)e.Item.FindControl("iconQuickpicBlock"));
                        if ((Profile.QuickPicType & currentItem.QuickPicFlags) > 0 && !currentItem.IsRfcd)
                        {
                            iconQuickPicBlock.Visible = true;
                            if (isInQuickPic)
                            {
                                //iconQuickPicBlock.Attributes["class"] += String.Format(" {0}_selected", iconQuickPicBlock.Attributes["class"]);
                                HtmlImage qpIconImage = ((HtmlImage)e.Item.FindControl("qpIcon"));
                                qpIconImage.Attributes["title"] = GetLocalResourceObject("RemoveFromQuickPic.Title").ToString();
                                qpIconImage.Attributes["alt"] = GetLocalResourceObject("RemoveFromQuickPic.Alt").ToString();

                                String currentClass = iconQuickPicBlock.Attributes["class"];
                                iconQuickPicBlock.Attributes["class"] = currentClass.Replace("QP_off", "QP_on");
                                iconQuickPicBlock.Attributes["class"] += " ICN_quickpic_selected";
                            }
                        }
                        else
                        {
                            iconQuickPicBlock.Visible = false;
                        }
                        
                        //Permission & pricing level
                        HtmlControl mediaRestrictionsPermissions = (HtmlControl)e.Item.FindControl("mediaRestrictionsPermissions");
                        HtmlControl pricingLevelPermissions = (HtmlControl)e.Item.FindControl("pricingLevelPermissions");

                        mediaRestrictionsPermissions.Visible = (currentItem.IsOutLine == true && (currentItem.PricingIconDisplay == Corbis.LightboxCart.Contracts.V1.PricingIcon.DoubleDollar || currentItem.PricingIconDisplay == Corbis.LightboxCart.Contracts.V1.PricingIcon.TripleDollar));
                        pricingLevelPermissions.Visible = Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionViewPricingLevelAbbreviations) && !String.IsNullOrEmpty(currentItem.PricingLevelIndicator);

                        ((HtmlControl)e.Item.FindControl("permissionsWrap")).Visible = mediaRestrictionsPermissions.Visible || pricingLevelPermissions.Visible;

                        //Pricing status, only price By AE and normal status displays the price.
						bool displayPrice = false;
                        HtmlGenericControl priceStatus = (HtmlGenericControl)e.Item.FindControl("priceStatus");
                        if (currentItem.IsOutLine && (currentItem.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.Unknown)
                        {
                            priceStatus.InnerText = (string)GetLocalResourceObject("contactOutline");
                            priceStatus.Attributes["onclick"] = "javascript:CorbisUI.RequestPricing.showModal('" + currentItem.CorbisId + "'," + this.LightboxId + ",'Lightbox');return false;";
                            priceStatus.Attributes["class"] = "contactUS";
                            ((HtmlImage)e.Item.FindControl("pricingImage")).Attributes["title"] = GetLocalResourceObject("priceImage.ContactOutlineText").ToString();
                               
                            //priceImageLink.Alt = GetLocalResourceObject("priceImage.ContactOutlineText").ToString();
                            // Request price form opening when price image icon clicked for Outline images 
                            if (((HtmlControl)e.Item.FindControl("iconPricingBlock")).Visible == true)
                            {
                                priceImageLinkControl.Attributes.Add("onclick", "javascript:CorbisUI.RequestPricing.showModal('" + currentItem.CorbisId + "'," + this.LightboxId + ",'Lightbox');return false;");
                            }                        
                        }
                        else if ((currentItem.EffectivePriceStatus & (PriceStatus.CountryOrCurrencyError | PriceStatus.UpdateUse)) != PriceStatus.Unknown)              
                        {
                            priceStatus.Attributes.Add("onclick", PricingNavigateUrl);
                            priceStatus.InnerText = (string)GetLocalResourceObject("updateUse");
                            priceStatus.Attributes["class"] = "contactUS";

                        }
						else if ((currentItem.EffectivePriceStatus & PriceStatus.ContactUs) == PriceStatus.ContactUs)
                        {
                            priceStatus.InnerText = (string)GetLocalResourceObject("contactUs");
                            // Request price form opening for contact us link in thumbnails 
                            priceStatus.Attributes["onclick"] = "javascript:CorbisUI.RequestPricing.showModal('" + currentItem.CorbisId + "'," + this.LightboxId + ",'Lightbox');return false;";
                            priceStatus.Attributes["class"] = "contactUS";
                        }
						else if ((currentItem.EffectivePriceStatus & PriceStatus.AsPerContract) == PriceStatus.AsPerContract)
                        {
                            priceStatus.InnerText = (string)GetLocalResourceObject("asPerContract");
                            priceStatus.Attributes.Add("onclick", PricingNavigateUrl);
                            priceStatus.Attributes["class"] = "contactUS";
                        }
						else if ((currentItem.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE)
						{
							priceStatus.InnerText = (string)GetLocalResourceObject("priceByAE");
							displayPrice = true;
						}
                        else
                        {
                            priceStatus.Visible = false;
							displayPrice = true;
                        }

						//Price
						HtmlGenericControl price = (HtmlGenericControl)e.Item.FindControl("price");
						if (displayPrice && !String.IsNullOrEmpty(currentItem.EffectivePrice ))
						{
							price.InnerHtml = String.Format("{0} {1}", CurrencyHelper.GetLocalizedCurrency(currentItem.EffectivePrice), currentItem.CurrencyCode);

							if (!currentItem.IsRfcd)
							{
								price.Attributes["onclick"] = "$(this).getParent().getParent().getElement('li.ICN_pricing a').onclick()";
							}
							else
							{
								price.Attributes["class"] += " nohand";
							}
						}
						else
						{
							price.Visible = false;
						}
                    }
                    // centered image
                    CenteredImageContainer wrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
                    int marginTop = 0;
                    if (currentItem.AspectRatio > 1)
                    {
                        marginTop = (tooltipImgHeight - (int)Math.Round(tooltipImgHeight / currentItem.AspectRatio)) >> 1;
                    }
                    string diams = string.Empty;
                    if (!string.IsNullOrEmpty(currentItem.DatePhotographed) && !string.IsNullOrEmpty(currentItem.Location))
                        diams = " &bull; "; // "&middot;";
                    string caption = currentItem.Caption;
                    if (!string.IsNullOrEmpty(currentItem.Caption) && currentItem.Caption.Length > MAXIUMCAPTIONLENGTH)
                    {
                        caption = currentItem.Caption.Substring(0, MAXIUMCAPTIONLENGTH); // +caption;
                        caption = caption.Substring(0, caption.LastIndexOf(' ')) + "...";
                    }
                    string brokenVBar = string.Empty;
                    if (!string.IsNullOrEmpty(currentItem.Caption) && (!string.IsNullOrEmpty(currentItem.DatePhotographed) || !string.IsNullOrEmpty(currentItem.Location)))
                    {
                        brokenVBar = " | ";
                    }

                    string rel;

                    rel = string.Format(@"
                            <div class='thumbtool-photo'>
                                <img src='{0}' border='0' style='margin-top:{1}px' />
                            </div>
                            <div class='thumbtool-creditline'>{2}</div>
                            <div class='thumbtool-title'>{3} </div>
                            <div class='thumbtool-detail'>
                                <span class='thumbtool-detail-date'>{4}</span> 
                                <span class='thumbtool-detail-location'>{5}</span>
                                <span class='thumbtool-detail-caption'>{6}</span>
                            </div>
                            ",
                            currentItem.Url256,
                            marginTop,
                            currentItem.CreditLine, // "2008 Foto G Rapher/Corbis", 
                            StringHelper.ConvertBracketsToItalic(currentItem.Title),
                            GetPhotographedDate(currentItem.DatePhotographed) + diams, // DateTime.Now.ToString(), 
                            currentItem.Location + brokenVBar,
                            caption // "this is the caption, this is the caption, this is the caption, this is the caption, this is the caption, this is the caption, this is the caption"
                            );
                    rel = HttpUtility.HtmlEncode(rel);
                    wrap.Attributes["rel"] = rel;

                    //note
                    if (String.IsNullOrEmpty(currentItem.Note))
                    {
                        ((HoverButton)e.Item.FindControl("btnNote")).ToolTip = GetLocalResourceObject("addNote").ToString();
                    }
                    else
                    {
                        ((HoverButton)e.Item.FindControl("btnNote")).ToolTip = GetLocalResourceObject("editnote").ToString();
                    }
                }
            }
        }

        private string GetPhotographedDate(string date)
        {
            if (! string.IsNullOrEmpty(date))
            {
                DateTime result = DateTime.Now;
                bool parseCheck = DateTime.TryParse(date, out result);
                if (parseCheck)
                {
                    return Convert.ToDateTime(date).ToString("MMMM dd, yyyy");
                }
            }
            return date;
        }



        #region IPriceImageLink Members

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Lightbox;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

        public bool ShowDownloadingProhibited
        {
            get { return DownloadingProhibitedDiv.Visible; }
            set { DownloadingProhibitedDiv.Visible = value; }
        }

		private bool _showPricingLink;
        public bool ShowPricingLink
        {
			get { return _showPricingLink; }
			set { _showPricingLink = value; }
        }

        private string pricingAltText;
        public string PricingAltText
        {
            get
            {
                return pricingAltText;
            }
            set
            {
                pricingAltText = (String)GetLocalResourceObject(value);
            }
        }

        private string pricingNavigateUrl;
        public string PricingNavigateUrl
        {
            get
            {
                return pricingNavigateUrl;
            }
            set
            {
                pricingNavigateUrl = value;
            }
        }

        public string RMRawUrl
        {
            get { return SiteUrls.RMPricing; }
        }

        public string RFRawUrl
        {
            get { return SiteUrls.RFPricing; }
        }

        public string RSRawUrl
        {
            get { return SiteUrls.RSPricing; }
        }

        public string CustomerServiceUrl
        {
            get { return "javascript:location.href='" + SiteUrls.CustomerService + "';"; }
        }

        public int RFPricingPageWidth
        {
            get { return 640; }
        }

        public int RFPricingPageHeight
        {
            get { return 480; }
        }

        public int RMPricingPageWidth
        {
            get { return 700; }
        }

        public int RMPricingPageHeight
        {
            get { return 545; }
        }

        public int RSPricingPageWidth
        {
            get { return 640; }
        }

        public int RSPricingPageHeight
        {
            get { return 480; }
        }

		public bool IsAnonymous
		{
			get { return Profile.IsAnonymous; }
		}

		#endregion
    }
}
