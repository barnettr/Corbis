using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.Controls;
using System.Globalization;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.Entities;
using ListBox=Corbis.Web.UI.Controls.ListBox;

namespace Corbis.Web.UI.Search
{
    public partial class Products : CorbisBaseUserControl, IPriceImageLink
	{
		const int tooltipImgHeight = 256;
		private const int MAXIUMCAPTIONLENGTH = 160;
		protected HtmlAnchor priceImageLink;
		protected HtmlImage pricingImage;
		protected CenteredImageContainer thumbWrap;
		protected Corbis.Web.UI.Controls.Label searchCategory;
		protected Corbis.Web.UI.Controls.Label marketingCollection;

		protected HtmlContainerControl productBlock;
		protected HtmlContainerControl iconSimilarBlock;
		protected HtmlContainerControl iconLightboxBlock;
		protected HtmlContainerControl iconPricingBlock;
		protected HtmlContainerControl iconCartBlock;
		protected HtmlContainerControl iconExpressCheckoutBlock;
		//protected HtmlContainerControl iconQuickpicOffBlock;
		//protected HtmlContainerControl iconQuickpicOnBlock;
        protected HtmlContainerControl iconQuickpicBlock;
        protected HtmlImage qpIconImage;

		protected HtmlContainerControl permissionsWrap;
		protected HtmlContainerControl mediaRestrictionsPermissions;
		protected HtmlContainerControl pricingLevelPermissions;
		protected HtmlImage dollarIndicator;

        protected override void OnInit(EventArgs e)
        {
            ShowDownloadingProhibited = Profile.IsChinaUser;
        }
        
        public object WebProductList
		{
			set
			{
				results.DataSource = value;
				results.DataBind();
			}
			get
			{
				return results.DataSource;
			}
		}

        //TODO: This logic belongs in the presenter
		protected void Result_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
		{
			// This event is raised for the header, the footer, separators, and items.

			// Execute the following logic for Items and Alternating Items.
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SearchResultProduct product = e.Item.DataItem as SearchResultProduct;

				////////////////////////////////
				// DEFINE SOME NEEDED ELEMENTS
				////////////////////////////////
                priceImageLink = (HtmlAnchor)e.Item.FindControl("priceImageLink");
                pricingImage = (HtmlImage)e.Item.FindControl("pricingImage");
				thumbWrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
				productBlock = (HtmlContainerControl)e.Item.FindControl("productBlock");
				searchCategory = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("searchCategory");
				marketingCollection = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("marketingCollection");
				iconSimilarBlock = (HtmlContainerControl)e.Item.FindControl("iconSimilarBlock");
				iconLightboxBlock = (HtmlContainerControl)e.Item.FindControl("iconLightboxBlock");
				iconPricingBlock = (HtmlContainerControl)e.Item.FindControl("iconPricingBlock");
				iconCartBlock = (HtmlContainerControl)e.Item.FindControl("iconCartBlock");
				iconExpressCheckoutBlock = (HtmlContainerControl)e.Item.FindControl("iconExpressCheckoutBlock");
				//iconQuickpicOffBlock = (HtmlContainerControl)e.Item.FindControl("iconQuickpicOffBlock");
				//iconQuickpicOnBlock = (HtmlContainerControl)e.Item.FindControl("iconQuickpicOnBlock");
                iconQuickpicBlock = (HtmlContainerControl)e.Item.FindControl("iconQuickpicBlock");
                
				permissionsWrap = (HtmlContainerControl)e.Item.FindControl("permissionsWrap");
				mediaRestrictionsPermissions = (HtmlContainerControl)e.Item.FindControl("mediaRestrictionsPermissions");
				pricingLevelPermissions = (HtmlContainerControl)e.Item.FindControl("pricingLevelPermissions");
				dollarIndicator = (HtmlImage)e.Item.FindControl("dollarIndicator");

				Boolean similarIconGood = true;
				Boolean pricingIconGood = true;
				Boolean quickpicIconGood = true;

				// DO SOME THUMB SETUP
                SetThumbAttributes(product);

                HtmlAnchor addToLightboxLink = (HtmlAnchor)e.Item.FindControl("addToLightboxLink");

				// category name and collection name setup
				searchCategory.Visible = true;
				searchCategory.Text = CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.Category>(product.Category);
				marketingCollection.Visible = true;
				marketingCollection.Text = product.MarketingCollection;

				productBlock.Attributes["class"] += string.Format(@" IMAGE-{0}", product.MediaUid);

				productBlock.Attributes["licenseModel"] = product.LicenseModel.ToString();

				productBlock.Attributes["productUID"] = product.MediaUid.ToString();

				productBlock.Attributes["corbisID"] = product.CorbisId;

				// product block border
				if (product.IsInLightbox
					|| product.IsInCart
					|| product.IsInQuickPick)
				{
					productBlock.Attributes["class"] += " ProductSelected";
				}

				// check for RFCD
				if (product.IsRFCD)
				{
					similarIconGood = false;
					pricingIconGood = false;
					quickpicIconGood = false;

					productBlock.Attributes["isRFCD"] = "true";
				}

				// check for outline - assuming no outline get through search
				if (product.IsOutline)
				{
					productBlock.Attributes["isOutline"] = "true";
				}

				// similar images
				if (product.ShowRelatedImagesIconForThumbnail && similarIconGood)
				{
					HtmlAnchor similarLink = (HtmlAnchor)e.Item.FindControl("viewSimilarLink");
					similarLink.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&tab=related&caller=search');return false;", product.CorbisId);
				}
				else
				{
					iconSimilarBlock.Visible = false;
				}

				// quickpic
				if (product.ShowQuickPickIconForThumbnail && quickpicIconGood)
				{
					//iconQuickpicOffBlock.Visible = true;
					//iconQuickpicOnBlock.Visible = true;
                    iconQuickpicBlock.Visible = true;
					if (product.IsInQuickPick)
					{
						//iconQuickpicOffBlock.Attributes["class"] += " ICN_quickpic_selected";
						//iconQuickpicOnBlock.Attributes["class"] += " ICN_quickpic_selected";
                        qpIconImage = (HtmlImage)e.Item.FindControl("qpIcon");
                        qpIconImage.Attributes["title"] = GetLocalResourceObject("RemoveFromQuickPic.Title").ToString();
                        qpIconImage.Attributes["alt"] = GetLocalResourceObject("RemoveFromQuickPic.Alt").ToString();
                        String currentClass = iconQuickpicBlock.Attributes["class"];
                        iconQuickpicBlock.Attributes["class"] = currentClass.Replace("QP_off","QP_on");
                        iconQuickpicBlock.Attributes["class"] += " ICN_quickpic_selected";
					}
				}
				else
				{
					//iconQuickpicOffBlock.Visible = false;
					//iconQuickpicOnBlock.Visible = false;
                    iconQuickpicBlock.Visible = false;
				}

				// pricing
				if (product.ShowCalculatorIconForThumbnail && pricingIconGood)
				{
					iconPricingBlock.Visible = true;
				}
				else
				{
					iconPricingBlock.Visible = false;
				}

				// cart
				if (Profile.IsECommerceEnabled)
				{
					iconCartBlock.Visible = true;
					if (product.IsInCart)
					{
						iconCartBlock.Attributes["class"] += String.Format(" {0}_selected", iconCartBlock.Attributes["class"]);
					}
				}
				else
				{
					iconCartBlock.Visible = false;
				}

                if(product.IsInLightbox)
                {
                    iconLightboxBlock.Attributes["class"] += String.Format(" {0}_selected",
                                                                           iconLightboxBlock.Attributes["class"]);
                }

			    // express checkout
				if (Profile.IsFastLaneEnabled && Profile.IsECommerceEnabled && !product.IsRFCD && !product.IsOutline)
				{
					iconExpressCheckoutBlock.Visible = true;
				}
				else
				{
					iconExpressCheckoutBlock.Visible = false;
				}


				// permissions wrap

				// This is probably not right 
				// There needs to be another flag for this

				if (String.IsNullOrEmpty(product.MediaRestrictionIndicatorForThumbnail)
					&& String.IsNullOrEmpty(product.PricingLevelIndicatorForThumbnail))
				{
					permissionsWrap.Visible = false;

				}
				else
				{
					if (String.IsNullOrEmpty(product.MediaRestrictionIndicatorForThumbnail))
					{
						mediaRestrictionsPermissions.Visible = false;
					}
					else
					{
						dollarIndicator.Attributes["class"] = "ICN_" + product.MediaRestrictionIndicatorForThumbnail;
					}

					if (String.IsNullOrEmpty(product.PricingLevelIndicatorForThumbnail))
					{
						pricingLevelPermissions.Visible = false;
					}

				}



				////////////////
				// FINAL STUFF
				////////////////

				// calculator
				PricingPresenter.InitializePriceImageLink(this, product);



				// centered image
				CenteredImageContainer wrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
				int marginTop = 0;
                decimal aspectRatio = GetImageAspectRatio(product);
				if (aspectRatio > 1)
				{
					marginTop = (tooltipImgHeight - (int)Math.Round(tooltipImgHeight / aspectRatio)) >> 1;
				}
				string diams = string.Empty;
				if (!string.IsNullOrEmpty(product.DatePhotographed) && !string.IsNullOrEmpty(product.Location))
					diams = " &bull; "; // "&middot;";
				string caption = product.Caption;
				if (!string.IsNullOrEmpty(product.Caption) && product.Caption.Length > MAXIUMCAPTIONLENGTH)
				{
					caption = product.Caption.Substring(0, MAXIUMCAPTIONLENGTH); // +caption;
					caption = caption.Substring(0, caption.LastIndexOf(' ')) + "...";
				}
				string brokenVBar = string.Empty;
				if (!string.IsNullOrEmpty(product.Caption) && (!string.IsNullOrEmpty(product.DatePhotographed) || !string.IsNullOrEmpty(product.Location)))
				{
					brokenVBar = " | ";
				}

				string rel;
                string title = string.IsNullOrEmpty(product.Title) ? "" : product.Title.Replace(@"&lt;", @"<i>").Replace(@"&gt;", @"</i>");
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
                        GetImage256Url(product),
						marginTop,
						product.CreditLine, 
						title,
						product.DatePhotographed + diams, 
						product.Location + brokenVBar,
						caption 
						);
				rel = HttpUtility.HtmlEncode(rel);
				wrap.Attributes["rel"] = rel;
			    string a = GetFiltersQuery(Request.QueryString);
				Corbis.Web.UI.Controls.Image imageThumb = (Corbis.Web.UI.Controls.Image)wrap.Controls[0];
				imageThumb.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}{1}&caller=search');return false;", product.CorbisId, a);

				productBlock.Attributes["class"] += string.Format(@" {0}", product.LicenseModel);
			}
		}
                

	    private string GetImage170Url(SearchResultProduct product)
        {
            if (product.IsRestrictedFineArt & Profile.IsAnonymous)
                return string.Format(SiteUrls.RestrictedCopyright170Format, Language.CurrentLanguage.LanguageCode);
            else
                return product.Url170;
        }

        private string GetImage256Url(SearchResultProduct product)
        {
            if (product.IsRestrictedFineArt && Profile.IsAnonymous)
                return string.Format(SiteUrls.RestrictedCopyright256Format, Language.CurrentLanguage.LanguageCode);
            else 
                return product.Url256;
        }

        private decimal GetImageAspectRatio(SearchResultProduct product)
        {
            if ((product.IsRestrictedFineArt && Profile.IsAnonymous)|| !product.IsAvailable)
            {
                return 1.0M;
            }
            else
            {
                return product.AspectRatio;
            }
        }
        
        private void SetThumbAttributes(SearchResultProduct product)
        {
            thumbWrap.ImgUrl = GetImage170Url(product);
            thumbWrap.Ratio = GetImageAspectRatio(product);
            thumbWrap.Size = 170;
            thumbWrap.AltText = String.Format("{0} - {1}", product.CorbisId, product.Title);
		}

		#region IPriceImageLink Members

		private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
		public Corbis.Web.Entities.ParentPage ParentPage
		{
			get { return parentPage; }
			set { parentPage = value; }
		}

		public bool ShowPricingLink
		{
			get { return priceImageLink.Visible; }
			set { priceImageLink.Visible = value; }
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
				this.pricingImage.Alt = pricingAltText;
				this.pricingImage.Attributes.Add("title", pricingAltText);
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
				priceImageLink.Attributes.Add("onclick", value);
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
			get { return 547; }
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
        
        private string GetFiltersQuery(NameValueCollection querySrting)
        {
            string query = "&";
            if (querySrting.Count > 0)
            {
                if ( querySrting.GetValues(SearchDropFilter.Category) != null && querySrting.GetValues(SearchDropFilter.Category).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.Category, "=",
                                           querySrting.GetValues(SearchDropFilter.Category)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.ColorFormat) != null && querySrting.GetValues(SearchDropFilter.ColorFormat).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.ColorFormat, "=",
                                           querySrting.GetValues(SearchDropFilter.ColorFormat)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.LicenseModel) != null && querySrting.GetValues(SearchDropFilter.LicenseModel).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.LicenseModel, "=",
                                           querySrting.GetValues(SearchDropFilter.LicenseModel)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.ModelReleased) != null && querySrting.GetValues(SearchDropFilter.ModelReleased).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.ModelReleased, "=",
                                           querySrting.GetValues(SearchDropFilter.ModelReleased)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.NumberOfPeople) != null && querySrting.GetValues(SearchDropFilter.NumberOfPeople).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.NumberOfPeople, "=",
                                           querySrting.GetValues(SearchDropFilter.NumberOfPeople)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.Sort) != null && querySrting.GetValues(SearchDropFilter.Sort).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.Sort, "=",
                                           querySrting.GetValues(SearchDropFilter.Sort)[0], "&");
                }

                if (querySrting.GetValues(SearchDropFilter.MediaType) != null && querySrting.GetValues(SearchDropFilter.MediaType).Length > 0)
                {
                    query += string.Concat(SearchDropFilter.MediaType, "=",
                                           querySrting.GetValues(SearchDropFilter.MediaType)[0], "&");
                }
            }
            return query.TrimEnd('&');

        }


        #region IPriceImageLink Members


        public bool ShowDownloadingProhibited
        {
            get
            {
                return this.DownloadingProhibitedDiv.Visible;
            }
            set
            {
                this.DownloadingProhibitedDiv.Visible = value;
            }
        }

        #endregion
    }

    internal static class SearchDropFilter
    {
        public const string Category = "cat";
        public const string ColorFormat = "cf";
        public const string LicenseModel = "lic";
        public const string ModelReleased = "mr";
        public const string NumberOfPeople = "np";
        public const string Sort = "sort";
        public const string MediaType = "mt";
    }

    
  
}
