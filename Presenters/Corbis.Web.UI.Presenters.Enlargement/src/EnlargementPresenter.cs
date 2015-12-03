using System;
using System.Configuration;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using Corbis.Framework.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Enlargement.ViewInterfaces;
using Corbis.Web.UI.Presenters;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Office.ServiceAgents.V1;
using Corbis.Office.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Framework.IpToCountry;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI;
using System.Web.UI;
using Corbis.RFCD.Contracts.V1;
using Corbis.RFCD.ServiceAgents.V1;

namespace Corbis.Web.UI.Presenters.Enlargement
{
    public class EnlargementPresenter : BasePresenter
    {

        #region Members

        public const string EnlargementPathRoot = "EnlargementPathRoot";

		private IEnlargementView enlargementView;
        private IEnlargementServiceView enlargementServiceView;
        private IMembershipContract membershipAgent;
        private IImageContract imageAgent;
		private ILightboxCartContract lightboxCartAgent;
        private IOfficeContract officeAgent;
        private DisplayImage displayImage;
		private IPriceImageLink priceImageView;
		private IRFCDContract rfcdAgent;
		private PriceStatus effectivePriceStatus;

        #endregion

        #region Constructors

        public EnlargementPresenter(IView view)
        {
			enlargementView = view as IEnlargementView;
            enlargementServiceView = view as IEnlargementServiceView;
			priceImageView = view as IPriceImageLink;
		}

        public IImageContract ImageAgent
        {
            get { return imageAgent ?? new ImageServiceAgent(); }
            set { imageAgent = value; }
        }

        public IMembershipContract MembershipAgent
		{
			get { return membershipAgent ?? new MembershipServiceAgent(); }
			set { membershipAgent = value; }
		}

		public ILightboxCartContract LightboxCartAgent
		{
			get { return lightboxCartAgent ?? new LightboxCartServiceAgent(); }
			set { lightboxCartAgent = value; }
		}

		public IOfficeContract OfficeAgent
		{
			get { return officeAgent ?? new OfficeServiceAgent(); }
			set { officeAgent = value; }
		}

		public IRFCDContract RFCDAgent
		{
			get { return rfcdAgent ?? new RFCDServiceAgent(); }
			set { rfcdAgent = value; }
		}


		#endregion

        #region Public Methods

		#region Public Methods used by ImageDetail

		public void PopulatePage()
		{
			if (!String.IsNullOrEmpty(enlargementView.CorbisId))
			{
				PopulateImageValues(enlargementView.CorbisId, enlargementView.CurrentLightboxId);
				PopulateAssociatedData();
			}
		}

		public void PopulateAssociatedData()
		{
			//The only associated data for rfcd is the icon toolset
			if (!enlargementView.IsRfcd)
			{
				//todo:  the contructor for the presenter should only take in a View.  The two inputs below should be added as properties on the view. 
				ImageRestrictionsPresenter restrictionsPresenter = new ImageRestrictionsPresenter(enlargementView.ImageRestrictionView, displayImage);
				restrictionsPresenter.SetRestrictions();

				PopulateDimensionList();
				if (enlargementView.ProductUid != Guid.Empty)
				{
					PopulateImagePrice();
				}

				// calculator
				PricingPresenter.InitializePriceImageLink(priceImageView, displayImage, enlargementView.ProductUid, !String.IsNullOrEmpty(enlargementView.Price));
			}
			SetIconToolset();
		}

        public void PopulateDimensionList()
        {
            List<ImageDimension> imageDimensionList = new List<ImageDimension>();
            imageDimensionList = ImageAgent.GetImageDimensions(enlargementView.CorbisId, Language.CurrentLanguage.LanguageCode);
            enlargementView.DimensionList = imageDimensionList;
        }

		private void PopulateImagePrice()
        {
            FolderProduct folderProduct = LightboxCartAgent.GetProductByProductUid(
                Profile.UserName,
                enlargementView.ProductUid, 
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                false,
                false);

            effectivePriceStatus = folderProduct.EffectivePriceStatus;
            if ((effectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE)
            {
                effectivePriceStatus = PriceStatus.PricedByAE;
            }
            else if ((effectivePriceStatus & PriceStatus.ContactOutline) == PriceStatus.ContactOutline)
            {
                effectivePriceStatus = PriceStatus.ContactOutline;
            }
            else if ((effectivePriceStatus & PriceStatus.ContactUs) == PriceStatus.ContactUs)
            {
                effectivePriceStatus = PriceStatus.ContactUs;
            }
            else if ((effectivePriceStatus & PriceStatus.CountryOrCurrencyError) == PriceStatus.CountryOrCurrencyError)
            {
                effectivePriceStatus = PriceStatus.CountryOrCurrencyError;
            }
            //enlargementView.Price = folderProduct.EffectivePrice;
            decimal thePrice = 0.00M;
            switch (effectivePriceStatus)
            {
                case PriceStatus.Ok:
                case PriceStatus.PricedByAE:
                    decimal.TryParse(folderProduct.EffectivePrice, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out thePrice);
                    enlargementView.Price = 
                        string.Format(
                        "{0} {1} ",
                        thePrice.ToString("#,###.00"), 
                        folderProduct.CurrencyCode);
                    break;
                case PriceStatus.CountryOrCurrencyError:
                    enlargementView.Price =
                        string.Format(
                        "{0} {1} ",
                        thePrice.ToString("#,###.00"),
                        folderProduct.CurrencyCode);
                    break;

                case PriceStatus.AsPerContract:
                case PriceStatus.ContactOutline:
                case PriceStatus.ContactUs:
                case PriceStatus.UpdateUse:
                    enlargementView.PriceStatus = effectivePriceStatus.ToString();
                    break;              
            }
        }

        public void PopulateImageValues(string corbisId)
		{
			if (enlargementView == null)
			{
				throw new Exception("bad view");
			}
            string enlargementUrl = "";
			displayImage = ImageAgent.GetEnlargementDisplayImage(corbisId, Language.CurrentLanguage.LanguageCode, Profile.IsAnonymous);
			if (displayImage == null)
			{
                string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
                RFCDEntity rfcdEntity = RFCDAgent.GetRFCDDetails(corbisId, countryCode, Language.CurrentLanguage.LanguageCode);
				if (rfcdEntity != null)
				{
					enlargementView.CorbisId = corbisId;
					enlargementView.ImageTitle = rfcdEntity.Title;
					enlargementView.EnlargementImageUrl = rfcdEntity.Url256;
					enlargementView.ImageUrl = rfcdEntity.Url256;
					enlargementView.AspectRatio = 1;
					enlargementView.IsRfcd = true;
					enlargementView.HasCorbisKeywords = false;
					enlargementView.HasRelatedImages = false;
					enlargementView.MediaUid = rfcdEntity.RFCDUid;
				}
				else
				{
					throw new ApplicationException(string.Format("Image '{0}' not found.", corbisId));
				}
			}
			else
			{
				enlargementView.IsRfcd = false;
				enlargementView.CorbisId = displayImage.CorbisId;
				enlargementView.ImageCaption = displayImage.Caption;
				enlargementView.LicenseModel = displayImage.LicenseModel;
				enlargementView.CreditLine = displayImage.CreditLine;
				enlargementView.ImageTitle = displayImage.Title;
                enlargementView.TitleFeedback = displayImage.Title;
				enlargementView.FineArtCreditLine = displayImage.FineArtCreditLine;
				enlargementView.Photographer = displayImage.PhotographerName;
				enlargementView.PhotoDate = displayImage.DatePhotographed;
				enlargementView.ContentCreator = displayImage.CreatorName;
				enlargementView.CreateDateKW = displayImage.SubjectDate;
                enlargementView.CreateDate = displayImage.SubjectDate;
				enlargementView.Magazine = displayImage.MagazineName;
				enlargementView.Location = displayImage.Location;
				if (!(displayImage.IsRestrictedFineArt && Profile.IsAnonymous))
				{
					//enlargementView.EnlargementImageUrl = displayImage.EnlargementUrl+"&uniqID="+Guid.NewGuid().ToString();
                    enlargementUrl = displayImage.EnlargementUrl;
                    if (enlargementUrl != null && !"".Equals(enlargementUrl))
                    {
                        if (enlargementUrl.Contains("?"))
                        {
                            enlargementUrl = enlargementUrl + "&uniqID=" + Guid.NewGuid().ToString();
                        }
                        else
                        {
                            enlargementUrl = enlargementUrl + "?uniqID=" + Guid.NewGuid().ToString();
                        }
                    }
                    enlargementView.EnlargementImageUrl = enlargementUrl;
					enlargementView.ImageUrl = displayImage.Url170;
					enlargementView.AspectRatio = displayImage.AspectRatio;
				}
				else
				{
					enlargementView.EnlargementImageUrl = string.Format(enlargementView.FineArtRestrictedImageURL, Language.CurrentLanguage.LanguageCode);
				}
				enlargementView.Category = displayImage.Category;
				enlargementView.MediaUid = displayImage.MediaUid;
				enlargementView.IsOutline = displayImage.IsOutline;
				enlargementView.QuickPicFlags = displayImage.QuickPicFlags;
				/*enlargementView.Keywords = displayImage.Keywords;*/
				enlargementView.PhotoDateKW = displayImage.DatePhotographed;

				if (!string.IsNullOrEmpty(displayImage.Title) && !string.IsNullOrEmpty(displayImage.Caption))
				{
					if ((displayImage.Title.Length + displayImage.Caption.Length) > 205 && displayImage.Title.Length < 205)
					{
						enlargementView.ImageCaptionKW = StringHelper.Truncate(displayImage.Caption.ToString(), 205 - displayImage.Title.Length);
					}
				}

				if (Enum.IsDefined(typeof(PriceTier), displayImage.PriceTierId))
				{
                    //enlargementView.PriceTier = displayImage.PriceTierDisplayText.ToUpper();

				    enlargementView.PriceTier = (PriceTier)displayImage.PriceTierId;
				}
				else
				{
					enlargementView.PriceTier = PriceTier.Unknown;
				}
				enlargementView.Collection = displayImage.MarketingCollection;
				if (displayImage.ContentWarnings != null && displayImage.ContentWarnings.Count > 0)
				{
					string prefix = string.Empty;
					List<ContentWarning> warningsList = new List<ContentWarning>();
					foreach (ContentWarning currValue in displayImage.ContentWarnings)
					{
						warningsList.Add(currValue);
					}

					enlargementView.ContentWarnings = warningsList;

				}
				enlargementView.HasRelatedImages = displayImage.HasRelatedImages;

				if (displayImage.Keywords == null || displayImage.Keywords.Count < 2)
				{
					enlargementView.HasCorbisKeywords = false;
				}
				else
				{
					enlargementView.HasCorbisKeywords = true;
					enlargementView.Keywords = displayImage.Keywords;
				}
				if (displayImage.DatePublished.HasValue && displayImage.DatePublished != DateTime.MinValue)
				{
					enlargementView.PublishDate = displayImage.DatePublished.Value.ToString("MMM dd, yyyy", Language.CurrentCulture);
				}
				else
				{
					enlargementView.PublishDate = string.Empty;
				}
				enlargementView.SetFeedbackText();
			}
		}

		public void PopulateImageValues(string corbisId, int? currentLightboxId)
		{
			PopulateImageValues(corbisId);
			List<ImageInLightboxCart> lightboxCartStatus = LightboxCartAgent.GetLightBoxCartStatusByCorbisIds(Profile.UserName, new List<string>(new string[] { corbisId }), currentLightboxId);

			if (lightboxCartStatus != null && lightboxCartStatus.Count == 1)
			{
				enlargementView.IsInCart = lightboxCartStatus[0].IsInCart;
				enlargementView.IsInLightbox = lightboxCartStatus[0].IsInLightbox;

				StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
				List<QuickPicItem> quickPickList = stateItems.GetStateItemValue<List<QuickPicItem>>(QuickPicKeys.Name, QuickPicKeys.QuickPickListKey, StateItemStore.AspSession);
				enlargementView.IsInQuickPick = (quickPickList == null? false: quickPickList.Exists(new Predicate<QuickPicItem>(delegate(QuickPicItem item) { return item.CorbisID == enlargementView.CorbisId; })));
			}
		}

		public void GetImageDetail(int imageNumber)
		{
			if (enlargementView.ImageListPageNo != 0 && enlargementView.ImageListPageSize != 0)
			{
				if (imageNumber > enlargementView.TotalImageCount) imageNumber = enlargementView.TotalImageCount;
				int pageNeeeded = (int)Math.Ceiling((decimal)imageNumber / (decimal)enlargementView.ImageListPageSize);
				int imageListIndex = (imageNumber - ((pageNeeeded - 1) * enlargementView.ImageListPageSize)) - 1;

				switch (enlargementView.Caller)
				{
					case "search":
						if (pageNeeeded != enlargementView.ImageListPageNo || enlargementView.ImageList == null)
						{
							GetImageList(imageNumber);
						}
						else
						{
							//sets the maxlenth of the textbox
							enlargementView.TotalImageCount = enlargementView.TotalImageCount;
						}

						enlargementView.CorbisId = enlargementView.ImageList[imageListIndex];
						PopulatePage();

						break;
					case "lightbox":
						if (pageNeeeded != enlargementView.ImageListPageNo || enlargementView.ImageList == null)
						{
							GetImageList(imageNumber);
						}
						else
						{
							//sets the maxlenth of the textbox
							enlargementView.TotalImageCount = enlargementView.TotalImageCount;
						}

						enlargementView.CorbisId = enlargementView.ImageList[imageListIndex];
						PopulatePage();

						break;
					case "imagegroups":
						if (pageNeeeded != enlargementView.ImageListPageNo || enlargementView.ImageList == null)
						{
							GetImageList(imageNumber);
						}
						else
						{
							//sets the maxlenth of the textbox
							enlargementView.TotalImageCount = enlargementView.TotalImageCount;
						}

						enlargementView.CorbisId = enlargementView.ImageList[imageListIndex];
						PopulatePage();

						break;
					case "mediaset":
					case "quickpic":
					case "cart":
						enlargementView.CorbisId = enlargementView.ImageList[imageListIndex];
						PopulatePage();

						break;
					default:
						break;
				}
			}
			else
			{
				enlargementView.TotalImageCount = 1;
                PopulatePage();
			}
		}

		public void GetImageList(int imageNumber)
		{
			int pageNeeeded = (int)Math.Ceiling((decimal)imageNumber / (decimal)enlargementServiceView.ImageListPageSize);
			switch (enlargementServiceView.Caller)
			{
				case "search":
					NameValueCollection query = new NameValueCollection();
					foreach (string queryParameter in enlargementServiceView.ImageListQuery.Split('&'))
					{
						string[] keyValue = queryParameter.Split('=');
						if (keyValue.Length == 2)
						{
							if (keyValue[0] == "p")
							{
								query.Add(keyValue[0], pageNeeeded.ToString());
							}
							else
							{
								query.Add(keyValue[0], keyValue[1]);
							}
						}
					}

					SearchPresenter searchPresenter = new SearchPresenter(enlargementServiceView);
					ClientSearchResults searchResult = searchPresenter.Search(query, enlargementServiceView.ImageListPageSize, ((pageNeeeded - 1) * enlargementServiceView.ImageListPageSize) + 1, Profile.UserName, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
					if (searchResult.SearchResultProducts != null && searchResult.SearchResultProducts.Count > 0)
					{
						enlargementServiceView.ImageList = searchResult.SearchResultProducts.ConvertAll<string>
						(
							new Converter<SearchResultProduct, string>
							(
								delegate(SearchResultProduct item) { return item.CorbisId; }
							)
						);
					}

					//just in case total have changed for the search
					enlargementServiceView.TotalImageCount = searchResult.TotalRecords;
					enlargementServiceView.ImageListPageNo = pageNeeeded;

					break;
				case "lightbox":
					List<LightboxDisplayImage> lightboxDisplayImageList =
						LightboxCartAgent.GetLightboxProductDetails(enlargementServiceView.LightboxId, Language.CurrentLanguage.LanguageCode,
							Profile.CountryCode, pageNeeeded, enlargementServiceView.ImageListPageSize, false, Profile.UserName);
					enlargementServiceView.ImageList = lightboxDisplayImageList.ConvertAll<string>(
						new Converter<LightboxDisplayImage, string>
						(
							delegate(LightboxDisplayImage item) { return item.CorbisId; }
						)
					);

					//just in case total have changed
					enlargementServiceView.TotalImageCount = LightboxCartAgent.GetLightboxProductCount(enlargementServiceView.LightboxId);
					enlargementServiceView.ImageListPageNo = pageNeeeded;
					break;
				case "imagegroups":
					Regex regex = new Regex("(typ=)([^&]*)", RegexOptions.IgnoreCase);
					string imageGroupTypeString = regex.Match(enlargementServiceView.ImageListQuery).Groups[2].Value;
					ImageMediaSetType imageGroupType = Enum.IsDefined(typeof(ImageMediaSetType), int.Parse(imageGroupTypeString)) ? (ImageMediaSetType)(Enum.Parse(typeof(ImageMediaSetType), imageGroupTypeString)) : ImageMediaSetType.Unknown;
					regex = new Regex("(id=)([^&]*)", RegexOptions.IgnoreCase);
					string imageGroupId = regex.Match(enlargementServiceView.ImageListQuery).Groups[2].Value;

                    string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
					if (imageGroupType != ImageMediaSetType.Unknown && !String.IsNullOrEmpty(imageGroupId))
					{
						switch (imageGroupType)
						{
							case ImageMediaSetType.Album:
							case ImageMediaSetType.StorySet:
							case ImageMediaSetType.Promotional:
							case ImageMediaSetType.PhotoShoot:
							case ImageMediaSetType.SameModel:
								MediaSet mediaSet = ImageAgent.GetMediaSetDetail(
									imageGroupId, 
                                    countryCode, 
                                    Profile.UserName,
									Language.CurrentLanguage.LanguageCode, 
                                    pageNeeeded, 
                                    enlargementServiceView.ImageListPageSize,
                                    ReadActiveLightboxId());
								enlargementServiceView.ImageList = mediaSet.DisplayImageList.ConvertAll<string>(
									new Converter<DisplayImage, string>(
										delegate(DisplayImage imageItem)
										{
											return imageItem.CorbisId;
										}
									)
								);

								//just in case total have changed for the search
								enlargementServiceView.TotalImageCount = mediaSet.TotalImages;

								break;
							case ImageMediaSetType.OutlineSession:
								int outlineSessionId;

								if (int.TryParse(imageGroupId, out outlineSessionId))
								{
 									OutlineSessionSet sessionSet = ImageAgent.GetOutlineSessionDetail(
										outlineSessionId, 
                                        countryCode, 
                                        Profile.UserName,
										Language.CurrentLanguage.LanguageCode, 
                                        pageNeeeded, 
                                        enlargementServiceView.ImageListPageSize,
                                        ReadActiveLightboxId());

									enlargementServiceView.ImageList = sessionSet.DisplayImageList.ConvertAll<string>(
										new Converter<DisplayImage, string>(
											delegate(DisplayImage imageItem)
											{
												return imageItem.CorbisId;
											}
										)
									);

									//just in case total have changed for the search
									enlargementServiceView.TotalImageCount = sessionSet.TotalCount;
								}

								break;
							case ImageMediaSetType.RFCD:
                                RFCDEntity rfcdEntity = RFCDAgent.GetRFCDDetailsWithPagination(
									imageGroupId, countryCode, Language.CurrentLanguage.LanguageCode,
									enlargementServiceView.ImageListPageSize, pageNeeeded, Profile.UserName);

								enlargementServiceView.ImageList = rfcdEntity.MediaItems.ConvertAll<string>(
									new Converter<RfcdDisplayImage, string>(
										delegate(RfcdDisplayImage imageItem)
										{
											return imageItem.CorbisId;
										}
									)
								);

								//just in case total have changed for the search
								enlargementServiceView.TotalImageCount = rfcdEntity.ImageCount;

								break;
							default:
								break;
						}
					}

					//just in case total have changed for the search
					enlargementServiceView.ImageListPageNo = pageNeeeded;
					break;
				default:
					break;
			}
		}
	
		private void SetIconToolset()
		{
			Dictionary<string, string> iconToolsetItems;
			iconToolsetItems = new Dictionary<string, string>();
            bool eCommerceEnabled = Profile.IsECommerceEnabled;
            bool isOutline = (enlargementView.LicenseModel == Corbis.CommonSchema.Contracts.V1.LicenseModel.RM && enlargementView.IsOutline);
		    bool isRFCD = enlargementView.IsRfcd;
			//Lightbox icon
            if (displayImage != null && displayImage.IsAvailable == false)
            {
                iconToolsetItems.Add("AddToLightbox", "disabled");
            }
			else if (enlargementView.IsInLightbox)
			{
				iconToolsetItems.Add("AddToLightboxSelected", "return CorbisUI.Enlargement.iconToolsetSelect('AddToLightbox');");
			}
			else
			{
				iconToolsetItems.Add("AddToLightbox", "return CorbisUI.Enlargement.iconToolsetSelect('AddToLightbox');");
			}

			//Quickpick icon
			if (Profile.IsQuickPicEnabled && !isRFCD && (Profile.QuickPicType & enlargementView.QuickPicFlags)!=0)
			{
                if (displayImage != null && displayImage.IsAvailable == false)
                {
                    iconToolsetItems.Add("AddToQuickPic", "disabled");
                }
				else if (enlargementView.IsInQuickPick)
				{
					iconToolsetItems.Add("AddToQuickPicSelected", "return false;");
				}
				else
				{
				    // TODO:Security - strip off angular brackets before using title.
					iconToolsetItems.Add("AddToQuickPic", String.Format("CorbisUI.Enlargement.addToQuickPic('{0}', '{1}', '{2}', '{3}', '{4}'); return false;", displayImage.CorbisId, displayImage.Url128, displayImage.LicenseModel, displayImage.AspectRatio, ((Page)enlargementView).Server.HtmlEncode(displayImage.Title).Replace("\\", "\\\\").Replace("'", "\\'")));
				}
			}

			//Calculator icon
			//no pricing icon for RFCD
			if (!enlargementView.IsRfcd)
			{
				if (!Profile.IsAnonymous || enlargementView.LicenseModel == LicenseModel.RF)
				{
					if (!eCommerceEnabled)
					{
						//Opening request price for all non-ecommerce images in enlargement
						iconToolsetItems.Add("NonEcommercePriceLink", "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
									   enlargementView.CorbisId + "', null, 1, 'Enlargement'); return false;");
					}
					else if (enlargementView.IsOutline && effectivePriceStatus != PriceStatus.PricedByAE)
					{
						//Opening request price for outline images in enlargement
						iconToolsetItems.Add("OutlinePriceLink", "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
									   enlargementView.CorbisId + "', null, 1, 'Enlargement'); return false;");
                    }
                    else if (displayImage != null && displayImage.IsAvailable == false)
                    {
                        iconToolsetItems.Add("CalculatePrice", "disabled");
                    }
                    else
                    {
                        iconToolsetItems.Add("CalculatePrice", priceImageView.PricingNavigateUrl);
                    }
				}
				else
				{
					iconToolsetItems.Add("CalculatePrice", "CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, 'refreshEnlargementPage(\\'CalculatePrice\\');'); return false;");
				}
			}

			//AddToCart icon
            if (!Profile.IsAnonymous)
            {
                if (eCommerceEnabled)
                {
                    if (displayImage != null && displayImage.IsAvailable == false)
                    {
                        iconToolsetItems.Add("AddToCart", "disabled");
                    }
                    else if (enlargementView.IsInCart)
                    {
                        iconToolsetItems.Add("AddToCartSelected", "return false;");
                    }
                    else
                    {
                        iconToolsetItems.Add("AddToCart", "return CorbisUI.Enlargement.iconToolsetSelect('AddToCart');");
                    }
                }
            }
            else
            {
				iconToolsetItems.Add("AddToCart", "CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, 'refreshEnlargementPage(\\'AddToCart\\');'); return false;");
            }

			//Expresscheckout icon
			if (eCommerceEnabled && Profile.IsFastLaneEnabled && !isOutline && !isRFCD  )
			{
				iconToolsetItems.Add("ExpressCheckout", "return CorbisUI.Enlargement.iconToolsetSelect('ExpressCheckout');");
			}

			//Print icon
			iconToolsetItems.Add("PrintPage", "return CorbisUI.Enlargement.iconToolsetSelect('PrintPage');");

			enlargementView.SetIconToolset(iconToolsetItems, "Key", "Value");
		}

        public void SendImageFeedback()
        {
            try
            {
                Corbis.Email.Contracts.V1.ImageFeedbackDetail imageFeedback = new Corbis.Email.Contracts.V1.ImageFeedbackDetail();
                imageFeedback.CorbisId = enlargementView.CorbisId;
                imageFeedback.LicenseModel = enlargementView.LicenseModelText;
                imageFeedback.Date = DateTime.Now.ToString();
                imageFeedback.BrowserName = enlargementView.BrowserName;
                imageFeedback.BrowserVersion = enlargementView.BrowserVersion;
                imageFeedback.Platform = enlargementView.Platform;
                string userName = string.Empty;
                if (Profile.IsAnonymous)
                {
                    userName = String.Empty;
                }
                else
                {
                    userName = Profile.UserName;
                }
                imageFeedback.Name = enlargementView.Name;
                imageFeedback.Email = enlargementView.Email;
                imageFeedback.Phone = enlargementView.PhoneNumber;
                imageFeedback.Role = enlargementView.Role;
                imageFeedback.Comments = enlargementView.Comments;
                imageFeedback.IssuesWithImage = enlargementView.IssueTypeText;
                imageFeedback.IssuesType = enlargementView.IssueType;

                ImageAgent.SendImageFeedbackEmail(imageFeedback, userName, Language.CurrentLanguage.LanguageCode);
            }
            catch (FaultException<Corbis.Common.FaultContracts.CorbisFault> vfex)
            {
                enlargementView.ShowEmailError();
            }
        }

		#endregion

        #endregion

        #region Public RelatedImage

        public List<RelatedImagesSetForScriptService> GetRelatedImages(string corbisId)
        {
            List<RelatedImagesSetForScriptService> relatedImageSetForScriptService = null;
            if (string.IsNullOrEmpty(corbisId))
            {
                throw new ArgumentNullException("corbis ID empty ");
            }
            else
            {
                string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
                List<RelatedImagesSet> relatedImageSet = ImageAgent.GetRelatedImages(corbisId, Language.CurrentLanguage.LanguageCode, countryCode, Profile.UserName, true);
                relatedImageSetForScriptService = ConvertForScriptService(relatedImageSet);
                return relatedImageSetForScriptService;
            }
        }
        #endregion

		//public DisplayImage GetDisplayImage(string corbisID)
		//{
		//    displayImage = ImageAgent.GetEnlargementDisplayImage(corbisID, Language.CurrentLanguage.LanguageCode, Profile.IsAnonymous);
		//    if (displayImage == null)
		//    {
		//        RFCDEntity rfcdEntity = RFCDAgent.GetRFCDDetails(corbisID, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
			
		//        if (rfcdEntity != null)
		//        {
		//            displayImage = new DisplayImage();
		//            displayImage.CorbisId = corbisID;
		//            displayImage.Title = rfcdEntity.Title;
		//            displayImage.EnlargementUrl = rfcdEntity.Url256;

		//            displayImage.Url170 = rfcdEntity.Url256;
		//            displayImage.AspectRatio = 1;
		//        }
		//        else
		//        {
		//            throw new ApplicationException(string.Format("Image '{0}' not found.", corbisID));
		//        }
		//    }
		//    return displayImage;
		//}

		public void PopulateFeedbackForm()
        {
            // LicenseModelText is populated when enlargementView.LicenseModel is Set in the
            // Method call PopulateImageValues(string corbisId,string culture)

            Corbis.Office.Contracts.V1.Office office;

			if (!Profile.IsAnonymous)
            {
                office = OfficeAgent.GetOffice(Profile.AddressDetail.CountryCode, Profile.AddressDetail.RegionCode, OfficeContactType.ContactAE);
                enlargementView.PhoneNumber = Profile.BusinessPhoneNumber;
                enlargementView.Email = Profile.Email;
                enlargementView.Name = Profile.FirstName + " " + Profile.LastName;
                enlargementView.UserName = Profile.UserName;
                enlargementView.Comments = "";
                enlargementView.ContactPhone = office.PhoneNumber;
            }
            else
            {
                string regionCode = "";
                office = OfficeAgent.GetOffice(Profile.CountryCode, regionCode, OfficeContactType.RepresentativeOffice);

                //Couldnt find office from IP address
                if (null == office)
                {
                    office = OfficeAgent.GetOffice(Profile.CountryCode, string.Empty, OfficeContactType.AssignedCorbisOffice);
                }
                enlargementView.PhoneNumber = Profile.BusinessPhoneNumber;
                enlargementView.Name = "";
                enlargementView.Email = Profile.Email;
                enlargementView.Comments = "";
                enlargementView.ContactPhone = office.PhoneNumber;
            }
        }

        public int ReadActiveLightboxId()
        {
            int activeLightboxId = 0;
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            String activeLightbox =
                stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                     StateItemStore.Cookie);
            if (!String.IsNullOrEmpty(activeLightbox))
            {
                activeLightboxId =
                    int.Parse(
                        stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                             StateItemStore.Cookie));
            }

            return activeLightboxId;
        }


        #region Private Methods

        private List<RelatedImagesSetForScriptService> ConvertForScriptService(List<RelatedImagesSet> relatedImageSetList)
        {
            List<RelatedImagesSetForScriptService> relatedImagesSetForScriptServiceList = new List<RelatedImagesSetForScriptService>();
            if (relatedImageSetList != null && relatedImageSetList.Count > 0)
            {
                relatedImagesSetForScriptServiceList = new List<RelatedImagesSetForScriptService>();
                foreach (RelatedImagesSet relatedImageSet in relatedImageSetList)
                {
                    List<DisplayImageForRelatedImages> displayImageForRelatedImagesList = new List<DisplayImageForRelatedImages>();
                    foreach (DisplayImage displayImage in relatedImageSet.DisplayImageList)
                    {
                        DisplayImageForRelatedImages displayImageForRelatedImages =
                            new DisplayImageForRelatedImages(
                                displayImage.CorbisId,
                                displayImage.AspectRatio,
                                displayImage.Url128
                                );
                        displayImageForRelatedImagesList.Add(displayImageForRelatedImages);
                    }
                    RelatedImagesSetForScriptService relatedImagesSetForScriptService = 
                        new RelatedImagesSetForScriptService(
                            relatedImageSet.Name,
                            relatedImageSet.Description,
                            relatedImageSet.MediaType,
                            relatedImageSet.MediaSetId,
                            displayImageForRelatedImagesList
                            );
                    relatedImagesSetForScriptServiceList.Add(relatedImagesSetForScriptService);
                }
            }
            return relatedImagesSetForScriptServiceList;
        }

        #endregion

    }
}



