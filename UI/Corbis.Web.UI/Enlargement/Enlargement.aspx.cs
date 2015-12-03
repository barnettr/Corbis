#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Framework.Globalization;
using Corbis.Image.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Enlargement.ViewInterfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.UI.Presenters.Enlargement;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.CustomerService;
using Corbis.Office.Contracts.V1;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;

#endregion

namespace Corbis.Web.UI.Enlargement
{
	public partial class Enlargement : CorbisBasePage, IEnlargementView, IPriceImageLink, IEnlargementServiceView
    {
        #region Constants
        //The Google should be in the following format.  Add it to the end of the existing query string:
        //&ext=1

        const string GOOGLE_QUERYSTRING_ARGUMENT1 = "ext";
        private const string REQUEST_IMAGE_ID = "id";
        private const string REQUEST_PRODUCT_UID = "puid";
		private const string REQUEST_QUERY_TEXT = "q";
		private const string REQUEST_CALLER = "caller";
		private const string REQUEST_IMAGEGROUPID = "imagegroupid";

        #endregion

        #region Member Variables

        private EnlargementPresenter presenter;
	    private string titleFeedback;
        protected Corbis.Web.UI.Controls.CenteredImageContainer thumbWrap;
        private Corbis.CommonSchema.Contracts.V1.LicenseModel _licenseModel;
        private Corbis.CommonSchema.Contracts.V1.Category categoryId;
      

        #endregion

        #region Event Handlers

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!string.IsNullOrEmpty(Request.QueryString[GOOGLE_QUERYSTRING_ARGUMENT1]))
            {
                this.MasterPageFile = SiteUrls.MasterBaseMasterFileLoc;
            }
            else
            {
                this.MasterPageFile = SiteUrls.NoGlobalNavMasterFileLoc;
            }

            presenter = new EnlargementPresenter(this);

            ShowDownloadingProhibited = Profile.IsChinaUser;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
            this.AddScriptToPage(SiteUrls.EnlargementScript, "EnlargementScript");
			// this.AddScriptToPage(SiteUrls.ImageDetailScript, "ImageDetailScript");
			this.AddScriptToPage(SiteUrls.AddToCartScript, "AddToCartScript");
			ScriptManager manager = (ScriptManager)Master.FindControl("scriptManager");
			manager.Services.Add(new ServiceReference("~/Enlargement/EnlargementScriptService.asmx"));
			manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
			manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Registration/SignInStatus.asmx"));
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Enlargement, "EnlargementCSS");
            contactCorbis.NavigateUrl = "";
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopulatePostSearchFilterQuerystring();

            if (!String.IsNullOrEmpty(Request.QueryString["print"]))
            {
                AnalyticsData["events"] = AnalyticsEvents.EnlargementPrint;
                HtmlGenericControl bodyTag = (HtmlGenericControl)Master.FindControl("body");
                bodyTag.Attributes.Add("class", "printVersion");
            }
            else
            {
                AnalyticsData["events"] = AnalyticsEvents.EnlargementDetailsLoad;
            }

            // Insert the clientscript if the Request is not from Google.
            // If the request is from Google the regular postback should work.
            if (string.IsNullOrEmpty(Request.QueryString[GOOGLE_QUERYSTRING_ARGUMENT1]))
            {
                this.SearchNowButton.OnClientClick = "searchKeywordsNowAction('false','" + this.PostSearchFilterQuerystring + "'); return false;";
            }
            else
            {
                imagePager.Visible = false;
                this.SearchNowButton.OnClientClick = "searchKeywordsNowAction('true','" + this.PostSearchFilterQuerystring + "'); return false;";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "authenticatedStatus", "var signInLevelWhenLoaded = " + getSignInSevel() + ";", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "country", "var countryWhenLoaded = '" + Profile.CountryCode + "';", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setupSignInAndCountryScript", "window.addEvent('domready',function(){checkSignInAndCountryStatus();});", true);
			
			if (!IsPostBack)
			{
				//If it's not postback but method is post then it means it's new window posted from one of our pages
				if (Request.HttpMethod.ToLower() == "post")
				{	
					//Add some handlers for updating the query string.
					((Button)imagePager.FindControl("previous")).OnClientClick = "if (!CorbisUI.Enlargement.updateRelativeImageQuery(-1)) return false;" + ((Button)imagePager.FindControl("previous")).OnClientClick;
                    ((Button)imagePager.FindControl("next")).OnClientClick = "if (!CorbisUI.Enlargement.updateRelativeImageQuery(1)) return false;" + ((Button)imagePager.FindControl("next")).OnClientClick;
                    ((TextBox)imagePager.FindControl("pageNumber")).Attributes["onchange"] = "if (!CorbisUI.Enlargement.updateImageQuery(parseInt(this.value))) return false;" + ((TextBox)imagePager.FindControl("pageNumber")).Attributes["onchange"];

					int pageNo = 0;
					int pageSize = 0;
					int totalItems;
					int imageNumber = 0;
					int lightboxId;
					List<string> imageList;

					int.TryParse(Request.Form["totalItems"], out totalItems);

					if (totalItems > 1)
					{
						TotalImageCount = totalItems;

						switch (Caller.ToLower())
						{
							case "search":
							case "imagegroups":
								ImageList = imageList = new List<string>(Request.Form["imageList"].Split(','));

								//with these we have to calculate the image number from the page and current index of the image in the page.
								if (int.TryParse(Request.Form["pageNo"], out pageNo) && int.TryParse(Request.Form["pageSize"], out pageSize) && imageList.Count > 1 && imageList.Contains(CorbisId))
								{
									int currentIndex = imageList.FindIndex(new Predicate<string>(delegate(string item) { return item == CorbisId; }));
									CurrentImageIndex = ((pageNo - 1) * pageSize) + currentIndex + 1;
								}
								else
								{
									imagePager.Visible = false;
								}

								ImageListQuery = Request.Form["searchQuery"];
								ImageListPageNo = pageNo;
								ParentPageSize = pageSize.ToString();
								break;
							case "lightbox":
								if (int.TryParse(Request.Form["lightboxId"], out lightboxId))
								{
									LightboxId = lightboxId;
									int.TryParse(Request.Form["pageSize"], out pageSize);

									//if pagsize is zero (from lightbox buddy) set default to 50 and get image list
									if (pageSize == 0)
									{
										if (int.TryParse(Request.Form["imageIndex"], out imageNumber))
										{
                                            CurrentImageIndex = imageNumber;
											pageSize = 50;
											ImageListPageNo = (int)Math.Floor((decimal)imageNumber / pageSize) + 1;
											ParentPageSize = pageSize.ToString();

											presenter.GetImageList(imageNumber);
											imageList = this.ImageList;
										}
										else
										{
											imagePager.Visible = false;
										}
									}
									else
									{
										ImageList = imageList = new List<string>(Request.Form["imageList"].Split(','));

										//with these we have to calculate the image number from the page and current index of the image in the page.
										if (int.TryParse(Request.Form["pageNo"], out pageNo) && int.TryParse(Request.Form["pageSize"], out pageSize) && imageList.Count > 1 && imageList.Contains(CorbisId))
										{
											int currentIndex = imageList.FindIndex(new Predicate<string>(delegate(string item) { return item == CorbisId; }));
											CurrentImageIndex = ((pageNo - 1) * pageSize) + currentIndex + 1;
										}
										else
										{
											imagePager.Visible = false;
										}

										ImageListPageNo = pageNo;
										ParentPageSize = pageSize.ToString();
									}
								}
								else
								{
									imagePager.Visible = false;
								}
								break;
							case "mediaset":
							case "quickpic":
								//Should only be one page, as it's not paged.
								ImageListPageNo = 1;
								ImageList = imageList = new List<string>(Request.Form["imageList"].Split(','));
								CurrentImageIndex = imageList.FindIndex(new Predicate<string>(delegate(string item) { return item == CorbisId; })) + 1;
								int.TryParse(Request.Form["pageSize"], out pageSize);
								ParentPageSize = (pageSize == 0 ? totalItems : pageSize).ToString();
								break;
							default:
								break;
						}
					}
					else
					{
						imagePager.Visible = false;
					}
				}
				//if it's just a get of the page, then it's safe to assume it's a single image, so no page control needed
				else
				{
					imagePager.Visible = false;
				}

				if (Request.QueryString["tab"] != null && Request.QueryString["tab"] == "related")
				{
                    AnalyticsData["events"] = AnalyticsEvents.EnlargementRelatedImagesLoad;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "selectRelatedImages", "CorbisUI.Enlargement.selectTab('relatedImages');CorbisUI.Enlargement.showCorner();", true);
				}


				PopulateFeedbackRoleDropdown();

				this.ImageUrl = string.Format(SiteUrls.RestrictedCopyright170Format, Language.CurrentLanguage.LanguageCode);
				this.EnlargementImageUrl = string.Format(SiteUrls.RestrictedCopyright256Format, Language.CurrentLanguage.LanguageCode);
				this.AspectRatio = 1.0M;

				presenter.PopulatePage();
				presenter.PopulateFeedbackForm();
			}
			else if (Request.Form["__EVENTTARGET"] == "refreshEnlargementPage")
			{
				//page refresh after signin, this needs to be a page post to retain page information
				//as the user can navigate a number of images and/or it's parent before performing an action that requires signin.
				int pageNumber;
				if (int.TryParse(((System.Web.UI.WebControls.TextBox)imagePager.FindControl("pageNumber")).Text, out pageNumber))
				{
					presenter.GetImageDetail(pageNumber);
					SetFeedbackText();
				}
			}

            this.SearchNowButton.Enabled = false;
            this.clearKeywordCheckbox.IsDisabled = true;

			if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack) OutputEnumClientScript<ImageMediaSetType>(Page);
			
			if (!Profile.IsAnonymous)
			{
				signInLink.Visible = false;
				waterMark.Visible = false;
			}

			if (pageAction.Value != "")
			{
				//if it's a page refresh after sign-in we should refresh parent after the action has been carried out so actions like add to cart etc will be reflected in the parent.
				ScriptManager.RegisterStartupScript(this, this.GetType(), "pageAction", String.Format("window.addEvent('domready', function(){{ExecutePageAction('{0}');{1}}});", pageAction.Value, Request.Form["__EVENTTARGET"] == "refreshEnlargementPage"? "RefreshOpener();": ""), true);
				pageAction.Value = "";
			}
		}

        private string getSignInSevel()
        {
            if (Profile.IsAnonymous)
            {
                return "0";
            }
            else
            {
                // Return "2" for logged in user
                if (Profile.IsAuthenticated)
                {
                    return "2";
                }
                else
                {
                    // Return "1" for partially logged in user
                    return "1";
                }
            }
        }

        // TO DO: this logic should be moved to the presenter
        private void PopulatePostSearchFilterQuerystring()
        {
            string postSearchFilterQuerystring = string.Empty;

            if (!String.IsNullOrEmpty(Request.QueryString[SearchPresenter.SearchFilterKeys.LicenseModel]))
            {
                postSearchFilterQuerystring = "&" + SearchPresenter.SearchFilterKeys.LicenseModel 
                    + "=" + Request.QueryString[SearchPresenter.SearchFilterKeys.LicenseModel];
            }
            if(!String.IsNullOrEmpty(Request.QueryString[SearchPresenter.SearchFilterKeys.MediaType]))
            {
                postSearchFilterQuerystring += "&" + SearchPresenter.SearchFilterKeys.MediaType
                    + "=" + Request.QueryString[SearchPresenter.SearchFilterKeys.MediaType];
            }
            if (!String.IsNullOrEmpty(Request.QueryString[SearchPresenter.SearchFilterKeys.ColorFormat]))
            {
                postSearchFilterQuerystring += "&" + SearchPresenter.SearchFilterKeys.ColorFormat
                    + "=" + Request.QueryString[SearchPresenter.SearchFilterKeys.ColorFormat];
            }
            if (!String.IsNullOrEmpty(Request.QueryString[SearchPresenter.SearchFilterKeys.Category]))
            {
                postSearchFilterQuerystring += "&" + SearchPresenter.SearchFilterKeys.Category
                    + "=" + Request.QueryString[SearchPresenter.SearchFilterKeys.Category];
            }
            if(!String.IsNullOrEmpty(Request.QueryString[SearchPresenter.SearchFilterKeys.NumberOfPeople]))
            {
                if (Request.QueryString[SearchPresenter.SearchFilterKeys.NumberOfPeople] == NumberOfPeople.WithoutPeople.ToString())
                {
                    postSearchFilterQuerystring += "&" + SearchPresenter.SearchFilterKeys.NumberOfPeople 
                        + "=" + NumberOfPeople.WithoutPeople.ToString();
                }
            }
            if (!String.IsNullOrEmpty(Request.QueryString["sort"]))
            {
                postSearchFilterQuerystring += "&sort=" + Request.QueryString["sort"];
            }
            this.PostSearchFilterQuerystring =  postSearchFilterQuerystring;


        }

        private string DoubleQuoteKeywordsWithSpecialCharacters(string keyword)
        {
            char[] specialChars = new char[] {' '};
            string returnKeyword = string.Empty;

            

            if (keyword.IndexOfAny(specialChars, 0) > 0)
            {
                returnKeyword = "\"" + keyword + "\"";
            }
            else
            {
                returnKeyword = keyword;
            }
            return returnKeyword;
        }
     
        protected void FeedbackSubmit_Click(object sender, EventArgs e)
        {
          
           presenter.SendImageFeedback();
           
        }

        public void PageChanged(object sender, PagerEventArgs e)
        {
            presenter.GetImageDetail(e.PageIndex);
            
            SetFeedbackText();
        }

		protected Corbis.Web.UI.Controls.Label imageSize;
		protected Corbis.Web.UI.Controls.Label uncompressedFileSize;
	    protected System.Web.UI.Control dimensionsWrapper;
		protected Corbis.Web.UI.Controls.Label dimensionsInPixels;
		protected Corbis.Web.UI.Controls.Label height;
		protected Corbis.Web.UI.Controls.Label width;
        protected Corbis.Web.UI.Controls.Label measurementUnit;
        protected Corbis.Web.UI.Controls.Label measurementUnit2;
        protected Corbis.Web.UI.Controls.Label resolution;
		protected Corbis.Web.UI.Controls.Label availability;
	    protected Corbis.Web.UI.Controls.Label contactUsLink;
       
        protected void dimensionsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				Corbis.Web.UI.Controls.Label lblMediaSetType = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("MediaSetType");

                ImageDimension imageDimension = (ImageDimension)e.Item.DataItem;
				((Corbis.Web.UI.Controls.Label)e.Item.FindControl("imageSize")).Text = GetKeyedEnumDisplayText(imageDimension.ImageSize, this.LicenseModel.ToString());
				((Corbis.Web.UI.Controls.Label)e.Item.FindControl("uncompressedFileSize")).Text = imageDimension.UnCompressedFileSize;
				((Corbis.Web.UI.Controls.Label)e.Item.FindControl("dimensionsInPixels")).Text = imageDimension.DimensionsInPixels;

                if (Language.CurrentLanguage == Language.EnglishUS)
                {
                    ((Corbis.Web.UI.Controls.Label)e.Item.FindControl("height")).Text = imageDimension.HeightInches.ToString();
                    ((Corbis.Web.UI.Controls.Label)e.Item.FindControl("width")).Text = imageDimension.WidthInches.ToString();
                    ((Corbis.Web.UI.Controls.Label)e.Item.FindControl("measurementUnit")).Text = GetLocalResourceString("inchAbbreviation");
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("measurementUnit2")).Text = GetLocalResourceString("inchAbbreviation");
                }
                else
                {
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("height")).Text = imageDimension.HeightCms.ToString();
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("width")).Text = imageDimension.WidthCms.ToString();
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("measurementUnit")).Text = GetLocalResourceString("centimeterAbbreviation");
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("measurementUnit2")).Text = GetLocalResourceString("centimeterAbbreviation");
                }

                if (imageDimension.Availability != ImageFileAvailability.ContactUs)
                {
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("resolution")).Text = imageDimension.Resolution.ToString();
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("availability")).Text = GetEnumDisplayText(imageDimension.Availability, true);
                }
                else
                {
					((HtmlGenericControl)e.Item.FindControl("dimensionsWrapper")).Visible = false;
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("availability")).Visible = false;
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("contactUsLink")).Visible = true;
					((Corbis.Web.UI.Controls.Label)e.Item.FindControl("contactUsLink")).Text = GetEnumDisplayText(imageDimension.Availability, true);
                }
            }
        }

		protected void AddToLightboxHandler(object sender, EventArgs e)
		{
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "addToLightbox", String.Format("refreshLightbox('{0}', '{1}', '{2}')", addToLightboxPopup.LightboxId.ToString(), this.CorbisId, addToLightboxPopup.NewLightboxName), true);
		    addToLightboxPopup.NewLightboxName = "";
		}

		#endregion 

		#region Properties

		public List<string> ImageList
		{
			get 
			{
				if (!String.IsNullOrEmpty(this.parentImageList.Value))
				{
					return new List<string>(this.parentImageList.Value.Split(','));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.parentImageList.Value = String.Join(",", value.ToArray());
			}
		}

		public int ImageListPageNo
		{
			get
			{
				int result;
				int.TryParse(this.parentPageNo.Value, out result);
				return result;
			}
			set
			{
				this.parentPageNo.Value = value.ToString();
			}
		}

		public int ImageListPageSize
		{
			get
			{
				int result;
				int.TryParse(this.parentPageSize.Value, out result);
				return result;
			}
		}

		public string ImageListQuery
		{
			get
			{
				return this.parentQueryString.Value;
			}
			set
			{
				this.parentQueryString.Value = value;
			}
		}

		public string ParentPageSize
		{
			get { return this.parentPageSize.Value; }
			set { this.parentPageSize.Value = value; }
		}

		public int CurrentImageIndex
		{
			get { return this.imagePager.PageIndex; }
			set { this.imagePager.PageIndex = value; }
		}

		public int TotalImageCount
		{
			get { return this.imagePager.TotalRecords; }
			set { this.imagePager.TotalRecords = value; }
		}

        public string Price
        {
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.price.Visible = false;
                }
                else
                {
                    this.price.Visible = true;
                    this.price.Text = (string)GetLocalResourceObject("price") + " " + value;
                }
            }
			get
			{
				return this.price.Text;
			}
        }

        public string PriceStatus
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.priceStatus.Visible = false;
                }
                else
                {
                    this.priceStatus.Text = (string) GetLocalResourceObject(value);
                }
            }
        }

        private Guid productUid;
        public Guid ProductUid
	    {
	        get {
                if (!string.IsNullOrEmpty(base.GetQueryString(REQUEST_PRODUCT_UID)))
                {
                    productUid = new Guid(base.GetQueryString(REQUEST_PRODUCT_UID));
                }
                return productUid; 
            }
            set { productUid = new Guid(base.GetQueryString(REQUEST_PRODUCT_UID)); }
	    }

        public Corbis.CommonSchema.Contracts.V1.Category Category
		{
			get 
            {
                return this.categoryId; 
            }
            set 
            {
                this.categoryId = value;
                this.categoryKW.Text = this.category.Text = CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.Category>(this.categoryId); 
            }
		}

        public Guid MediaUid
		{
			get { return new Guid(mediaUid.Value.ToString()); }
			set { mediaUid.Value = value.ToString(); }
		}
        
        //todo - move switch statement to the presenter
        public PriceTier PriceTier
        {
            set {
                string priceTierText = GetEnumDisplayText(value);

                switch (_licenseModel.ToString().ToUpper())
                {
                    case "RF":
                        priceTier.Text = priceTierText + " " + GetEnumDisplayText(LicenseModel.RF);
                        priceTierKW.Text = priceTierText;
                        break;
                    case "RS":
                        priceTier.Text = priceTierText + " " + GetEnumDisplayText(LicenseModel.RS);
                        priceTierKW.Text = priceTierText;
                        break;
                    case "RM":    
                    default:
                        priceTier.Text = priceTierText + " " + GetEnumDisplayText(LicenseModel.RM);
                        priceTierKW.Text = priceTierText;
                        break;
                }
            }
        }
		
        //todo - the color formatting below needs be moved out of here into the CSS
        //todo - the switch statement needs to be moved into the presenter
        public Corbis.CommonSchema.Contracts.V1.LicenseModel LicenseModel
        {
            get { return _licenseModel; }
            set
            {
                _licenseModel = value;
                AnalyticsData["prop4"] = _licenseModel.ToString();
                switch (_licenseModel.ToString().ToUpper())
                {
                    case "RM":
                        licenseModel.ForeColor = licenseModelKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.ForeColor = priceTierKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.Text = priceTierKW.Text = priceTier.Text + " " + (string)GetLocalResourceObject("rm.Text");
                        licenseModel.Text = "(" + CorbisBasePage.GetEnumDisplayText<LicenseModel>(value) + ")";
                        licenseModelKW.Text = CorbisBasePage.GetEnumDisplayText<LicenseModel>(value);
                        LicenseModelText = (string)GetLocalResourceObject("rm.Text");
                        break;
                    case "RF":
                        licenseModel.ForeColor = licenseModelKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.ForeColor = priceTierKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.Text = priceTierKW.Text = priceTier.Text + " " + (string)GetLocalResourceObject("rf.Text");
                        licenseModel.Text = "(" + CorbisBasePage.GetEnumDisplayText<LicenseModel>(value) + ")";
                        licenseModelKW.Text = CorbisBasePage.GetEnumDisplayText<LicenseModel>(value);
                        LicenseModelText = (string)GetLocalResourceObject("rf.Text");
                        break;
                    case "RS":
                        licenseModel.ForeColor = licenseModelKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.ForeColor = priceTierKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.Text = priceTierKW.Text = priceTier.Text + " " + (string)GetLocalResourceObject("rs.Text");
                        licenseModel.Text = "(" + CorbisBasePage.GetEnumDisplayText<LicenseModel>(value) + ")";
                        licenseModelKW.Text = CorbisBasePage.GetEnumDisplayText<LicenseModel>(value);
                        LicenseModelText = (string)GetLocalResourceObject("rs.Text");
                        break;
                    case "UNKNOWN":
                        licenseModel.Text = licenseModelKW.Text = string.Empty;
                        LicenseModelText = licenseModelKW.Text = "UNKNOWN";
                        break;
                    default:
                        licenseModel.ForeColor = licenseModelKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.ForeColor = priceTierKW.ForeColor = Color.FromArgb(0x646464);
                        priceTier.Text = priceTierKW.Text = priceTier.Text + " " + (string)GetLocalResourceObject("rm.Text");
                        licenseModel.Text = "(" + CorbisBasePage.GetEnumDisplayText<LicenseModel>(value) + ")";
                        licenseModelKW.Text = CorbisBasePage.GetEnumDisplayText<LicenseModel>(value);
                        LicenseModelText = (string)GetLocalResourceObject("rm.Text");
                        break;
                }
            }
        }

        private bool isOutLine;
		public bool IsOutline
		{
			get { return isOutLine;}
			set { isOutLine = value;}
		}

        private bool isInCart;        
        public bool IsInCart
		{
			get { return isInCart; }
			set { isInCart = value; }
		}

        private bool isInLightbox;
        public bool IsInLightbox
		{
			get { return isInLightbox; }
			set { isInLightbox = value; }
		}

        private bool isInQuickPick;
        public bool IsInQuickPick
		{
			get { return isInQuickPick; }
			set { isInQuickPick = value; }
		}

		public bool HasRelatedImages
		{
			get { return relatedImages.Visible; }
			set { relatedImages.Visible = value; }
		}

        public bool HasCorbisKeywords
        {
            get { return corbisKeywords.Visible; }
            set { corbisKeywords.Visible = value; }
        }

        public string CreditLine
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    panelImageTextPW.Visible = panelImageText.Visible = false;
                }
                else
                {
                    panelImageTextPW.Visible = panelImageText.Visible = true;
                    creditLine.Text = this.creditLineKW.Text = value;
                    imageText.Text = this.imageTextKW.Text = (string)GetLocalResourceObject("imageText.Text").ToString().ToUpper();
                    copyRight.Text = (string)GetLocalResourceObject("imageText.Text") + " " + value;
                }
            }
        }

        public string ImageTitle
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    imageTitle.Text = imageTitleKW.Text = rfcdTitle.Text = string.Empty;
                }
                else
                {
                    string text = StringHelper.ConvertBracketsToItalic(value);
                    imageTitle.Text = text;
                    imageTitleKW.Text = text;
                    rfcdTitle.Text = text;
                }
            }
        }

        public List<ContentWarning> ContentWarnings
        {
            set
            {
                // TODO: This logic belongs in the presenter
                string contentWarningsText = string.Empty;

                foreach (ContentWarning warning in value)
                {
                    contentWarningsText += GetEnumDisplayText<ContentWarning>(warning) + "<br>";
                }

                if (!string.IsNullOrEmpty(contentWarningsText))
                {
                    contentWarnings.Text = contentWarningsText;
                }
                else
                {
                    contentWarnings.Text = string.Empty;
                }
            }
        }

        public string ImageCaption
        {
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    imageCaption.Text = imageCaptionKW.Text =  StringHelper.ConvertBracketsToItalic(value);
                }
                else
                {
                    imageCaption.Text = imageCaptionKW.Text = string.Empty;
                }
            }
        }

        public string FineArtCreditLine
        {
            set
            {
                panelartworkText.Visible = !String.IsNullOrEmpty(value);
                fineArtCreditLine.Text = value;
            }
        }

        public string Photographer
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    panelphotographerText.Visible = false;
                }
                else
                {
                    panelphotographerText.Visible = true;
                    photographer.Text = value;
                    photographer.Attributes["onclick"] = String.Format(
                        "reloadparentCloseChild('{0}','{1}','{2}','{3}')",
                        SearchPresenter.SearchFilterKeys.Photographer,
                        StringHelper.EncodeToJsString(photographer.Text), 
                        CorbisId, 
                        this.PostSearchFilterQuerystring);
                }
            }
        }

        public string PostSearchFilterQuerystring { get; set; }

        public string PhotoDate
        {
            set
            {
                paneldatePhotographedText.Visible = !String.IsNullOrEmpty(value);
                photoDate.Text = DateHelper.GetLocalizedLongDateWithoutWeekday(value);
            }
        }

        public string ContentCreator
        {
            set
            {
                panelcreatorNameText.Visible = !String.IsNullOrEmpty(value);
                contentCreator.Text = value;
            }
        }

        public string CreateDate
        {
            set
            {

                paneldataCreatedText.Visible = !String.IsNullOrEmpty(value);
                createDate.Text = value;
            }
        }

        public string Magazine
        {
            set
            {
                panelmagazineText.Visible = !String.IsNullOrEmpty(value);
                magazine.Text = value;
            }
        }

        public string PublishDate
        {
            set
            {
                paneldatePublishedText.Visible = !String.IsNullOrEmpty(value);
                publishDate.Text = DateHelper.GetLocalizedLongDateWithoutWeekday(value);
            }
        }

        public string Location
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    panellocationText.Visible = panellocationTextKW.Visible = false;
                }
                else
                {
                    panellocationText.Visible = panellocationTextKW.Visible = true;
                    location.Text = value;
                    locationKW.Text = value;

                    string reloadParentJS = String.Format(
                        "reloadparentCloseChild('{0}','{1}','{2}','{3}')",
                        SearchPresenter.SearchFilterKeys.Location,
                        StringHelper.EncodeToJsString(locationKW.Text),
                        CorbisId,
                        this.PostSearchFilterQuerystring);

                    location.Attributes["onclick"] = reloadParentJS;
                    locationKW.Attributes["onclick"] = reloadParentJS;
                }
            }
        }
 

        public string EnlargementImageUrl
        {
            set { enlargementImage.ImageUrl = value; }
        }

        public string Collection
        {
            set
            {
                panelcollectionText.Visible = !String.IsNullOrEmpty(value);
                collection.Text = value;
            }
        }

        public string Name
        {
            get { return name.Text; }
            set { name.Text = value; }
        }

        public string Email
        {
            get { return userEmail.Text; }
            set { userEmail.Text = value; }
        }
		
        public string CorbisId
        {
			get { return (ViewState["CorbisId"] == null ? base.GetQueryString(REQUEST_IMAGE_ID) : ViewState["CorbisId"].ToString()); }
			set 
            {
                AnalyticsData["products"] = value;
                ViewState["CorbisId"] = imageID.Text = imageIDKW.Text = rfcdId.Text = value; 
            }
        }

        public string PhoneNumber
        {
            get
            {
                return phoneNumber.Text;
            }
            set
            {
                phoneNumber.Text = Server.HtmlEncode(value);
            }
        }

        public string Role
        {
            get { return role.SelectedValue; }
        }

        public string UserName
        {
            set { userName.Text = Server.HtmlEncode(value); }
        }

        public string TitleFeedback
        {
            get { return titleFeedback; }
            set { titleFeedback = value; }
        }

        public string LicenseModelText
        {
            get
            {
                return licenseModelText.Value;
            }
            set 
            { 
                licenseModelText.Value = value; 
            }
        }

        public string Comments
        {
            get { return comments.Text; }
            set { this.comments.Text = value; }
        }

        public string BrowserName
        {
            get { return HttpContext.Current.Request.Browser.Type; }
        }

        public string BrowserVersion
        {
            get { return HttpContext.Current.Request.Browser.Version; }
        }

        public string Platform
        {
            get { return HttpContext.Current.Request.Browser.Platform; }
        }

        public Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType IssueType
        {
            get 
            {
                Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType issue = 
                    Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.Unknown;

                switch(this.issueType.SelectedValue)
                {
                    case "Imaging":
                        issue = Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.Imaging;
                        break;
                    case "ImageInformation":
                        issue = Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.ImageInformation;
                        break;
                    case "Legal":
                        issue = Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.Legal;
                        break;
                    case "Editing":
                        issue = Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.Editing;
                        break;
                    default :
                        issue = Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType.Other;
                        break;
                }

                return issue; 
            }
        }

        public string IssueTypeText
        {
            get 
            {
                if (this.issueType.SelectedItem != null)
                {
                    return this.issueType.SelectedItem.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        string contactPhone;
        public string ContactPhone
        {
            set { contactPhone = value;
                  description.Text = string.Format(description.Text, value);}            
            get { return contactPhone; }
        }

        public List<ImageDimension> DimensionList
        {
            set
            {
                List<ImageDimension> dimensionsList;
                dimensionsList = value;

                if(dimensionsList.Count > 0)
                {
                    this.dimensionsRepeater.DataSource = dimensionsList;
                    this.dimensionsRepeater.DataBind();
                }
            }
        }

        public List<Keyword> Keywords
        {
            set
            {
                List<Keyword> keywordList;
                keywordList = value;

                foreach (Keyword item in keywordList)
                {
                    item.Term = StringHelper.ConvertBracketsToItalic(item.Term);
                }

                if (keywordList.Count > 0)
                {
                    KeywordRepeater.DataSource = value;
                    KeywordRepeater.DataBind();
                }
            }
        }
        
		public string ImageUrl
		{
			set
			{
				thumbWrap.ImgUrl = value;
			}
		}

		public decimal AspectRatio
		{
			set
			{
				thumbWrap.Ratio = value;
			}
		}

        public string CreateDateKW
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    paneldataCreatedTextKW.Visible = false;
                }
                else
                {
                    createDateKW.Text = value;
                }
            }
        }

        public string PhotoDateKW
		{
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.paneldatePhotographedTextKW.Visible = false;
				}
				else
				{
                    DateTime result = DateTime.Now;
                    bool parseCheck = DateTime.TryParse(value, out result);
                    if (!parseCheck)
                    {
                        photoDateKW.Text = value;
                    }
                    else
                    {
                       photoDateKW.Text  = DateHelper.GetLocalizedDate(Convert.ToDateTime(value), DateFormat.YearFormat);
                    }
				}
			}
		}

		public string ImageCaptionKW
		{
			set
			{
				imageCaptionKW.Text = value;
			}
		}

		public int? CurrentLightboxId
		{
			get
			{
				int storedId;
				int? currentLightboxId = null;

				StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
				if (int.TryParse(stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie), out storedId))
				{
					currentLightboxId = storedId;
				}
				return currentLightboxId;
			}
		}

		private bool isRfcd;
		public bool IsRfcd
		{
			set
			{
				isRfcd = value;
				imageInfo.Visible = !isRfcd;
				rfcdInfo.Visible = isRfcd;
				if (isRfcd)
				{
					category.Text = GetLocalResourceString("rfcdHeader");
					viewRfcdLink.Attributes["onclick"] = String.Format("javascript:CorbisUI.Enlargement.viewRfcdImages('{0}','{1}');", (int)Corbis.Image.Contracts.V1.ImageMediaSetType.RFCD, this.CorbisId);
				}
			}
			get
			{
				return isRfcd;
			}
		}

		public IImageRestrictionsView ImageRestrictionView
		{
			get { return this.ImageRestrictions; }
		}

		private QuickPicFlags quickPicFlags;
		public QuickPicFlags QuickPicFlags
		{
			get { return quickPicFlags; }
			set { quickPicFlags = value; }
		}

		public string Caller
		{
			get
			{
				return Request.QueryString[REQUEST_CALLER];
			}
		}

		public int LightboxId
		{
			get 
			{
				int returnValue;
				if (int.TryParse(lightboxId.Value, out returnValue))
				{
					return returnValue;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				lightboxId.Value = value.ToString();
			}
		}

		public string ImageGroupId
		{
			get
			{
				return Request.QueryString[REQUEST_IMAGEGROUPID];
			}
		}

	    public string FineArtRestrictedImageURL
	    {
            get { return SiteUrls.RestrictedCopyright170Format; }
	    }
		#endregion

        #region Methods

        public void PopulateFeedbackRoleDropdown()
        {
            string[] keys = new string[5]
                                {
                                    "selectOneRole", "notContributorRole", "representingRole", "contributorRole",
                                    "forThisImageRole"
                                };
            for (int i = 0; i < keys.Length; i++)
            {
                string data = GetLocalResourceObject(keys[i]).ToString();
                if (keys[i] == "selectOneRole")
                {
                    role.Items.Add(new ListItem(data, ""));
                }
                else
                {
                    role.Items.Add(new ListItem(data, data));
                }
            }
        }

        public void SetIconToolset(object datasource, string cssClassField, string valueField)
        {
            this.iconToolset.DataSource = datasource;
            this.iconToolset.CssClassField = cssClassField;
            this.iconToolset.ValueField = valueField;
            this.iconToolset.DataBind();
        }


        public void SetFeedbackText()
        {
            feedbackText.Text = GetLocalResourceString("feedbackMessage.Text");
            feedbackText.Text = feedbackText.Text.Replace("{0}", LicenseModelText);
            feedbackText.Text = feedbackText.Text.Replace("{1}", CorbisId);
            feedbackText.Text = feedbackText.Text.Replace("{2}", StringHelper.ConvertBracketsToItalic(TitleFeedback));
        }

        public void ShowEmailError()
        {
            emailErrorFromServer.Value = "true";
        }

		//todo - move to presenter
        private string BuildNavigationLink(string corbisId)
		{
			NameValueCollection newQueryString = new NameValueCollection(Request.QueryString);
			newQueryString.Set(REQUEST_IMAGE_ID, corbisId);
			return SiteUrls.Enlargement + "?" + ConvertCollectionToQueryString(newQueryString);
		}

		//todo move to presenter
        private string BuildReturnToSearchLink()
		{
			NameValueCollection newQueryString = new NameValueCollection(Request.QueryString);
			newQueryString.Remove(REQUEST_IMAGE_ID);
			return SiteUrls.SearchResults + "?" + ConvertCollectionToQueryString(newQueryString);
		}
        
        //todo move to presenter
		private static string ConvertCollectionToQueryString(NameValueCollection coll)
		{
			StringBuilder queryString = new StringBuilder();

			string delimiter = "";
			foreach (string key in coll.Keys)
			{
				queryString.Append(delimiter);
				queryString.Append(key);
				queryString.Append("=" + HttpUtility.UrlEncode(coll[key]));
				delimiter = "&";
			}

			return queryString.ToString();
		}

		#endregion

        #region IPriceImageLink Members

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Enlargement;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

        public bool ShowPricingLink
        {
            get { return price.Visible; }
            set { price.Visible = value; }
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
                //this.price.Alt = pricingAltText;
                //this.price.Attributes.Add("title", pricingAltText);
               
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
                this.price.Attributes.Add("onclick", value);
                switch (this.priceStatus.Text.ToLower())
                {
                    case "contact outline":
                    case "contact us":
                        this.priceStatus.Attributes["onclick"] = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" 
                            + this.CorbisId + "'," 
                            + (this.CurrentLightboxId.HasValue ? this.CurrentLightboxId.ToString() : "null")
                            + ", 1,'Enlargement');return false;";
                        break;
                    default:
                        this.priceStatus.Attributes.Add("onclick", value);
                        break;
                }
               
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
            get { return 550; }
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

        #region IPriceImageLink Members


        public bool ShowDownloadingProhibited
        {
            get
            {
                return false;
            }
            set {  }
        }

        #endregion
    }
}
