using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Framework.Globalization;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.MarketingCollection.Contracts.V3;
using Corbis.MarketingCollection.ServiceAgents.V3;
using Corbis.Membership.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.RFCD.Contracts.V1;
using Corbis.RFCD.ServiceAgents.V1;
using Corbis.Web.Utilities;


namespace Corbis.Web.UI.Presenters.ImageGroups
{
    public class ImageGroupsPresenter : BasePresenter
    {
        #region Constants

        private const string BEGIN_DATE = "mm/dd/yyyy";
        private const string END_DATE_FORMAT = "MM/dd/yyyy";

        #endregion

        #region Private members

        private IImageContract _imageAgent;
        private ILightboxCartContract _lightboxCartAgent;
        private IMarketingCollectionContract _marketingCollectionAgent;
        private IRFCDContract _rfcdAgent;
        private IImageGroupsView imageGroupsView;

        private StateItemCollection stateItems;
        private IView view;
        private IImageGroupHeader headerView;
        private IRFCDHeaderView rfcdHeaderView;
        private IOutlineHeaderView outlineHeaderView;
        private IStorySetHeaderView storySetHeaderView;
        private IAlbumHeaderView albumHeaderView;
        private ISameModelHeaderView sameModelHeaderView;
        private IPromotionalSetHeaderView promotionalSetHeaderView;
        private ISamePhotoshootHeaderView samePhotoshootHeaderView;

        #endregion

        #region Constructors

        public ImageGroupsPresenter(IView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("SearchPresenter: SearchPresenter() - Search view cannot be null.");
            }

            this.view = view;
            imageGroupsView = view as IImageGroupsView;

            stateItems = new StateItemCollection(HttpContext);
        }

        #endregion

        #region Properties

        public IImageGroupHeader ImageGroupHeaderView
        {
            set
            {
                headerView = value;
                outlineHeaderView = headerView as IOutlineHeaderView;
                storySetHeaderView = headerView as IStorySetHeaderView;
                albumHeaderView = headerView as IAlbumHeaderView;
                sameModelHeaderView = headerView as ISameModelHeaderView;
                promotionalSetHeaderView = headerView as IPromotionalSetHeaderView;
                samePhotoshootHeaderView = headerView as ISamePhotoshootHeaderView;
                rfcdHeaderView = headerView as IRFCDHeaderView;
            }
        }
        public IRFCDContract RFCDAgent
        {
            get { return _rfcdAgent ?? new RFCDServiceAgent(); }
            set { _rfcdAgent = value; }
        }

        public IImageContract ImageAgent
        {
            get { return _imageAgent ?? new ImageServiceAgent(); }
            set { _imageAgent = value; }
        }

        public ILightboxCartContract LightboxCartAgent
        {
            get { return _lightboxCartAgent ?? new LightboxCartServiceAgent(); }
            set { _lightboxCartAgent = value; }
        }

        public IMarketingCollectionContract MarketingCollectionAgent
        {
            get { return _marketingCollectionAgent ?? new MarketingCollectionServiceAgent(); }
            set { _marketingCollectionAgent = value; }
        }

        #endregion

        private MediaSet _mediaSet = null;
        public void GetMediaSetProducts()
        {
            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
            _mediaSet = ImageAgent.GetMediaSetDetail(
                imageGroupsView.ImageGroupId, 
                countryCode, 
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode, 
                imageGroupsView.CurrentPageNumber, 
                imageGroupsView.ItemsPerPage,
                ReadActiveLightboxId());

            if (_mediaSet != null && _mediaSet.DisplayImageList != null && _mediaSet.DisplayImageList.Count > 0)
            {
                GetProductsFromDisplayImageList(_mediaSet.DisplayImageList);
                imageGroupsView.TotalRecords = _mediaSet.TotalImages;
                imageGroupsView.CurrentPageHitCount = _mediaSet.DisplayImageList.Count;

                ImageMediaSetType imageGroupType = Enum.IsDefined(typeof(ImageMediaSetType), _mediaSet.MediaSetTypeId) ? (ImageMediaSetType)(Enum.Parse(typeof(ImageMediaSetType), _mediaSet.MediaSetTypeId.ToString())) : ImageMediaSetType.Unknown;
                if (imageGroupType == ImageMediaSetType.StorySet)
                {
                    string captionText = String.Empty;

                    if (!String.IsNullOrEmpty(_mediaSet.DatePhotographed))
                    {
                        captionText +=
                            DateTime.Parse(_mediaSet.DatePhotographed, Language.EnglishUS.CultureInfo).ToShortDateString()
                                .ToString(Language.CurrentCulture);
                    }

                    if (!string.IsNullOrEmpty(_mediaSet.DisplayLocation))
                    {
                        if (captionText != string.Empty)
                        {
                            captionText += ", ";
                        }
                        captionText += _mediaSet.DisplayLocation;
                    }

                    if (!string.IsNullOrEmpty(_mediaSet.IntroductionText))
                    {
                        if (captionText != string.Empty)
                        {
                            captionText += " - ";
                        }

                        captionText += _mediaSet.IntroductionText;
                    }

                    if (captionText != string.Empty)
                    {
                        imageGroupsView.CaptionText = captionText;
                        imageGroupsView.ShowCaptionButtonAndText = true;
                    }
                }
            }
            else
            {
                imageGroupsView.ShowZeroResults = true;
            }
            
        }

        private OutlineSessionSet _sessionSet = null;
        public void GetSessionSetProducts()
        {
            int outlineSessionId;
            int.TryParse(imageGroupsView.ImageGroupId, out outlineSessionId);
            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
             _sessionSet = ImageAgent.GetOutlineSessionDetail(
                outlineSessionId, 
                countryCode, 
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode, 
                imageGroupsView.CurrentPageNumber, 
                imageGroupsView.ItemsPerPage,
                ReadActiveLightboxId());

            if (_sessionSet != null && _sessionSet.DisplayImageList != null && _sessionSet.DisplayImageList.Count > 0)
            {
                GetProductsFromDisplayImageList(_sessionSet.DisplayImageList);
                imageGroupsView.TotalRecords = _sessionSet.TotalCount;
                imageGroupsView.CurrentPageHitCount = _sessionSet.DisplayImageList.Count;

            }
            else
            {
                imageGroupsView.ShowZeroResults = true;
            }
        }

        private void GetProductsFromDisplayImageList(List<DisplayImage> displayImageList)
        {
            List<SearchResultProduct> searchResultPRoduct = new List<SearchResultProduct>();
            displayImageList = ScrubImageTitle(displayImageList);
            Dictionary<string, QuickPicItem> quickPicItems = GetQuickPicItems();

            if (null != displayImageList)
            {
                searchResultPRoduct = ConvertSearchResults(displayImageList, quickPicItems, ReadActiveLightboxId());
            }

            imageGroupsView.SearchResultProducts = searchResultPRoduct;
        }

        private List<DisplayImage> ScrubImageTitle(List<DisplayImage> displayImageList)
        {
            if(displayImageList != null && displayImageList.Count > 0)
            {
                displayImageList.ForEach(
                   delegate(DisplayImage img)
                   {
                       if (img.Title != null && img.Title.Contains(@"'"))
                       {
                           img.Title = img.Title.Replace(@"'", @"&rsquo;");
                       }
                   }
                   );

            }
            return displayImageList;
        }


		public void GetImageGroupHeader()
        {
            ImageMediaSetType imageGroupType = Enum.IsDefined(typeof(ImageMediaSetType), int.Parse(headerView.ImageGroupName)) ? (ImageMediaSetType)(Enum.Parse(typeof(ImageMediaSetType), headerView.ImageGroupName)) : ImageMediaSetType.Unknown;
            MediaSet mediaSet = _mediaSet != null ? _mediaSet : new MediaSet();
            switch (headerView.GetType().BaseType.Name.ToString())
            {
                case "StorySetHeader":
					if (imageGroupType == ImageMediaSetType.StorySet)
					{
						storySetHeaderView = headerView as IStorySetHeaderView;
						//check for null on the next line
                        if (mediaSet.MediaSetId.ToString() != storySetHeaderView.ImageGroupId)
                        {
                            mediaSet = GetMediaSet(storySetHeaderView.ImageGroupId);
                        }
						if (null != mediaSet)
						{
							storySetHeaderView.Title = mediaSet.Title;
							storySetHeaderView.Id = mediaSet.MediaSetId.ToString();
							storySetHeaderView.ImageCount = mediaSet.TotalImages.ToString();
                            if (!String.IsNullOrEmpty(mediaSet.DatePhotographed))
                            {
                                storySetHeaderView.Date = DateTime.Parse(mediaSet.DatePhotographed, Language.EnglishUS.CultureInfo).ToShortDateString()
                                .ToString(Language.CurrentCulture);
                            }
                            storySetHeaderView.Location = mediaSet.DisplayLocation;
							storySetHeaderView.Photographer = mediaSet.CreditLine;
						}
					}
                    break;
                case "AlbumHeader":
					if (imageGroupType == ImageMediaSetType.Album)
					{
						albumHeaderView = headerView as IAlbumHeaderView;
						//check for null on the next line
                        if (mediaSet.MediaSetId.ToString() != albumHeaderView.ImageGroupId)
                        {
                            mediaSet = GetMediaSet(albumHeaderView.ImageGroupId);
                        }
						if (null != mediaSet)
						{
							albumHeaderView.Title = mediaSet.Title;
							albumHeaderView.Id = mediaSet.MediaSetId.ToString();
							albumHeaderView.ImageCount = mediaSet.TotalImages.ToString();
						}
					}
                    break;
                case "PromotionalSetHeader":
					if (imageGroupType == ImageMediaSetType.Promotional)
					{
						promotionalSetHeaderView = headerView as IPromotionalSetHeaderView;
						//check for null on the next line
                        if (mediaSet.MediaSetId.ToString() != promotionalSetHeaderView.ImageGroupId)
                        {
                            mediaSet = GetMediaSet(promotionalSetHeaderView.ImageGroupId);
                        }
						if (null != mediaSet)
						{
                            // TODO: Security - need to call string helper to handle angular brackets.
							promotionalSetHeaderView.Title = mediaSet.Title;
							promotionalSetHeaderView.Id = mediaSet.MediaSetId.ToString();
							promotionalSetHeaderView.ImageCount = mediaSet.TotalImages.ToString();
						}
					}
                    break;
                case "SameModelHeader":
					if (imageGroupType == ImageMediaSetType.SameModel)
					{
						sameModelHeaderView = headerView as ISameModelHeaderView;
						//check for null on the next line
                        if (mediaSet.MediaSetId.ToString() != sameModelHeaderView.ImageGroupId)
                        {
                            mediaSet = GetMediaSet(sameModelHeaderView.ImageGroupId);
                        }
						if (null != mediaSet)
						{
							sameModelHeaderView.Id = mediaSet.MediaSetId.ToString();
						    sameModelHeaderView.ImageCount = mediaSet.TotalImages.ToString();
						    sameModelHeaderView.Photographer = mediaSet.CreditLine;
						}
					}
                    break;
                case "SamePhotoshootHeader":
					if (imageGroupType == ImageMediaSetType.PhotoShoot)
					{
						samePhotoshootHeaderView = headerView as ISamePhotoshootHeaderView;
						//check for null on the next line
                        if (mediaSet.MediaSetId.ToString() != samePhotoshootHeaderView.ImageGroupId)
                        {
                            mediaSet = GetMediaSet(samePhotoshootHeaderView.ImageGroupId);
                        }
						if (null != mediaSet)
						{
							samePhotoshootHeaderView.Id = mediaSet.MediaSetId.ToString();
							samePhotoshootHeaderView.ImageCount = mediaSet.TotalImages.ToString();
                            samePhotoshootHeaderView.Photographer = mediaSet.CreditLine;
						}
					}
                    break;
                 case "OutlineHeader":
					if (imageGroupType == ImageMediaSetType.OutlineSession)
					{

						int outlineSessionId;
						int.TryParse(outlineHeaderView.ImageGroupId, out outlineSessionId);
                        OutlineSessionSet sessionSet = _sessionSet != null ? _sessionSet : null;
                        if (sessionSet == null || outlineSessionId != sessionSet.OutlineSessionId)
                        {
                           string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
                           sessionSet = ImageAgent.GetOutlineSessionDetail(
                                outlineSessionId,
                                countryCode,
                                Profile.UserName,
                                Language.CurrentLanguage.LanguageCode,
                                1,
                                1,
                                ReadActiveLightboxId());
                        }
						if (sessionSet != null)
						{
							outlineHeaderView.FeaturedCelebrities = sessionSet.CelebrityNames;
						    outlineHeaderView.ImageCount = sessionSet.TotalCount.ToString();
							outlineHeaderView.Photographer = sessionSet.PhotographerName;
						    outlineHeaderView.DatePublished = sessionSet.PublicationDate.ToString(Language.CurrentCulture);
							outlineHeaderView.CreditLine = sessionSet.CreditLine;

						}
					}
                    break;
            }
        }

        private MediaSet GetMediaSet(string ImageGroupId)
        {
            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
            MediaSet mediaSet = ImageAgent.GetMediaSetDetail(
                ImageGroupId, 
                countryCode, 
                Profile.UserName, 
                Language.CurrentLanguage.LanguageCode, 
                1, 
                1,
                ReadActiveLightboxId());
            return mediaSet;
        }

        private MediaSet GetMediaSet()
        {
            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
            MediaSet mediaSet =
                ImageAgent.GetMediaSetDetail(
                imageGroupsView.ImageGroupId, 
                countryCode, 
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode, 
                1, 
                5000,
                ReadActiveLightboxId());
            return mediaSet;
        }

        RFCDEntity _rfcdEntity = null;
        ///<summary>
        ///Displays the RFCD information on the RFCD results page
        ///</summary>
        public void DisplyRFCDResults()
        {
            if (imageGroupsView == null)
            {
                throw new Exception(string.Format("Invalid View. Expected IRFCDResultsView; Actual {0}", this.view.GetType().Name));
            }

            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
            _rfcdEntity = RFCDAgent.GetRFCDDetailsWithPagination(
                imageGroupsView.ImageGroupId, 
                countryCode,
                Language.CurrentLanguage.LanguageCode,
                imageGroupsView.ItemsPerPage, 
                imageGroupsView.CurrentPageNumber, 
                Profile.UserName,
                ReadActiveLightboxId());
            int totalResults = _rfcdEntity.TotalImages;


            imageGroupsView.TotalRecords = totalResults;
            imageGroupsView.CurrentPageHitCount = _rfcdEntity.MediaItems.Count;
            
            

            List<SearchResultProduct> resultProducts = new List<SearchResultProduct>();
            if (_rfcdEntity != null && _rfcdEntity.MediaItems != null && _rfcdEntity.MediaItems.Count > 0)
            {
                _rfcdEntity.MediaItems.ForEach(
                    delegate(RfcdDisplayImage img)
                    {
                        if (img.Title != null && img.Title.Contains(@"'"))
                        {
                            img.Title = img.Title.Replace(@"'", @"&rsquo;");
                        }
                    }
                    );

                List<string> corbisIds = GetCorbisIdsFromRFCDResults(_rfcdEntity.MediaItems);
                int lightboxId = ReadActiveLightboxId();
                Dictionary<string, QuickPicItem> quickPickImages = GetImagesInQuickPick();
        
                resultProducts =
                    ConvertSearchResults(_rfcdEntity.MediaItems, quickPickImages, lightboxId);
            }
            else
            {
                imageGroupsView.ShowZeroResults = true;
            }

            imageGroupsView.SearchResultProducts = resultProducts;
        }

        private Dictionary<string, QuickPicItem> GetQuickPicItems()
        {
            Dictionary<string, QuickPicItem> quickPickImages = GetImagesInQuickPick();
            return quickPickImages;
        }

        private Dictionary<string, ImageInLightboxCart> GetLightboxCartImageDetails(List<DisplayImage> imageLIst, IView view)
        {
            List<string> corbisIds = GetCorbisIDsFromDisplayImageList(imageLIst);
            int lightboxId = ReadActiveLightboxId();
            Dictionary<string, ImageInLightboxCart> lightboxCartImages =
                GetImagesInLightboxCart(Profile.UserName, corbisIds, lightboxId);
            return lightboxCartImages;
        }

        public void DisplayRFCDHeader()
        {
            if (rfcdHeaderView == null)
            {
                throw new Exception(string.Format("Invalid View. Expected IRFCDResultsView; Actual {0}", this.view.GetType().Name));
            }

            if (Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff))
            {
                rfcdHeaderView.AddAllImagetoLightBoxVisible = true;
            }
            else
            {
                rfcdHeaderView.AddAllImagetoLightBoxVisible = false;
            }

            RFCDEntity rfcdEntity = _rfcdEntity;
            if (rfcdEntity == null || (rfcdEntity.VolumeNumber != rfcdHeaderView.ImageGroupId))
            {
                string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;                 
                rfcdEntity = RFCDAgent.GetRFCDDetailsWithPagination(
                    rfcdHeaderView.ImageGroupId, 
                    countryCode, 
                    Language.CurrentLanguage.LanguageCode,
                    1, 
                    1, 
                    Profile.UserName);
            }
            rfcdHeaderView.Title = rfcdEntity.Title;
            rfcdHeaderView.RFCDUid = rfcdEntity.RFCDUid.ToString();
            rfcdHeaderView.ImageCount = rfcdEntity.ImageCount.ToString();
            rfcdHeaderView.Id = rfcdEntity.VolumeNumber;
            rfcdHeaderView.RFCDImageURL = rfcdEntity.Url128;
            if (rfcdEntity.BasePrice != null)
            {
                //rfcdHeaderView.Price = Convert.ToDecimal(rfcdEntity.BasePrice).ToString("#,###.00") +" " + "("+ rfcdEntity.BasePriceUnit + ")";
                rfcdHeaderView.Price = CurrencyHelper.GetLocalizedCurrency(rfcdEntity.BasePrice) +" " + "("+ rfcdEntity.BasePriceUnit + ")";
            }
            
            FileSize fileSize = Enum.IsDefined(typeof(FileSize), int.Parse(rfcdEntity.FileSize)) ? (FileSize)(Enum.Parse(typeof(FileSize), rfcdEntity.FileSize.ToString())) : FileSize.Unknown;
            rfcdHeaderView.FileSize = fileSize.ToString();
            rfcdHeaderView.Copyright = rfcdEntity.CopyrightDate + "/" + rfcdEntity.ContentProvider;
            rfcdHeaderView.ShowAddToCartButton = Profile.IsECommerceEnabled;
            
        }
        
        public string LoadDefaultFilters()
        {
            return stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.DefaultSearchCookieKey, StateItemStore.Cookie);
        }

        private static Dictionary<string, QuickPicItem> GetImagesInQuickPick()
        {
            Dictionary<string, QuickPicItem> imageCollection = null;
            QuickPicPresenter quickPicPresenter = new QuickPicPresenter();


            List<QuickPicItem> imagesInQuickPick = quickPicPresenter.QuickPicList;

            if (imagesInQuickPick != null)
            {
                imageCollection = new Dictionary<string, QuickPicItem>();
                foreach (QuickPicItem item in imagesInQuickPick)
                {
                    imageCollection.Add(item.CorbisID, item);
                }
            }
            return imageCollection;
        }


        private List<SearchResultProduct> ConvertSearchResults(
            List<RfcdDisplayImage> results,
            Dictionary<string, QuickPicItem> quickPickImages, 
            int lightboxId)
        {
            return results.ConvertAll<SearchResultProduct>(
                new Converter<RfcdDisplayImage, SearchResultProduct>(
                    delegate(RfcdDisplayImage displayImage)
                    {
                        return new SearchResultProduct(
                            displayImage,
                            displayImage.InLightboxCart,
                            Profile.IsQuickPicEnabled ? Profile.QuickPicType : QuickPicFlags.None,
                            Profile.Permissions,
                            Profile.IsFastLaneEnabled,
                            lightboxId,
                            CheckCorbisIdInQuickPickList(displayImage.CorbisId, quickPickImages),
                            true
                            );
                    }));
        }

        private List<SearchResultProduct> ConvertSearchResults(
            List<DisplayImage> result, 
            Dictionary<string, QuickPicItem> quickPickImages, 
            int lightboxId)
        {
            return result.ConvertAll<SearchResultProduct>(
                 new Converter<DisplayImage, SearchResultProduct>(
                     delegate(DisplayImage displayImage)
                      {
                          return
                              new SearchResultProduct(
                                  displayImage,
                                  displayImage.InLightboxCart,
                                  Profile.IsQuickPicEnabled ? displayImage.QuickPicFlags : QuickPicFlags.None,
                                  Profile.Permissions,
                                  Profile.IsFastLaneEnabled,
                                  lightboxId,
                                  CheckCorbisIdInQuickPickList(displayImage.CorbisId, quickPickImages),
                                  false);
                      })
                );
        }

        /// <summary>
        /// Adds an offering to cart
        /// </summary>
        /// <param name="offeringUid"></param>
        public bool AddToCart(Guid offeringUid)
        {
            try
            {
                if (!Profile.IsAnonymous)
                {
                    //TODO:- have AddOfferingToCart to return an Int for CartItemCount instead of void.
                    this.LightboxCartAgent.AddOfferingToCart(Profile.MemberUid, offeringUid,
                                                             Language.CurrentLanguage.LanguageCode,
                                                             Profile.CountryCode);
                    Profile.CartItemsCount = this.LightboxCartAgent.GetCartCount(Profile.MemberUid);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "AddItemToCart");
                throw;
            }
        }

        /// <summary>
        /// Adds an offering to cart
        /// </summary>
        /// <param name="offeringUid"></param>
        public void DeleteImageFromLightbox(int lightboxId, Guid productUid)
        {
            try
            {
                LightboxCartAgent.DeleteProductUidFromLightBox(lightboxId, productUid, Profile.UserName);
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "DeleteImageFromLightbox");
                throw;
            }
        }

        /// <summary>
        /// Shows or hides Search Buddy tabs based on business logic. 
        /// Only Quickpic is currently implemented
        /// </summary>
        public void SetSearchBuddyTabs()
        {
            if (imageGroupsView != null)
            {
                imageGroupsView.ShowQuickPicTab = Profile.IsQuickPicEnabled;
                imageGroupsView.AdjustStatusForUser();
            }
        }

        #region Lightbox Operations

        public List<Lightbox> GetLightboxList(string userName)
        {
            try
            {
                string lightboxSortType = string.Empty;
                if (
                    stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey,
                                                         StateItemStore.Cookie) == null)
                {
                    lightboxSortType = "date";
                }
                else
                {
                    lightboxSortType =
                        stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey,
                                                             StateItemStore.Cookie).ToString();
                }

                if (lightboxSortType != "name")
                {
                    return LightboxCartAgent.GetLightboxTreeFlattenedByUserName(userName, LightboxTreeSort.Date, true);
                }
                else
                {
                    return LightboxCartAgent.GetLightboxTreeByUserName(userName, LightboxTreeSort.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: GetLightboxTreeByFlattenedByUsername() - Error getting lightbox tree for member '{0}'.",
                                    userName));
            }

            return null;
        }

        public void LoadLightBoxData()
        {
            if (imageGroupsView != null)
            {
                if (Profile.IsAnonymous)
                {
                    imageGroupsView.ShowAddToLightboxPopup = false;
                }
                else
                {
                    List<Lightbox> lightboxes = GetLightboxList(Profile.UserName);
                    if (lightboxes != null && lightboxes.Count > 0)
                    {
                        imageGroupsView.LightboxList = lightboxes;
                        //LoadLightboxDetails(lightboxes[0].LightboxId);
                        imageGroupsView.ActiveLightbox =
                            stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
                    }
                }
            }
        }

        public List<LightboxDisplayImage> LoadLightboxDetails(int lightboxId, bool isQuickpickSort)
        {
            
            return LightboxCartAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode, Profile.CountryCode, 1, 50, isQuickpickSort, Profile.UserName);
        }

		public int GetLightboxImageCount(int lightboxId)
		{
			return LightboxCartAgent.GetLightboxProductCount(lightboxId);
		}

        public List<CartDisplayImage> GetCartItems()
        {
            return LightboxCartAgent.GetCartContent(
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode,
                Profile.CountryCode);
        }

        #endregion

        #region Search Filters

        //private static string GetFilterQueryValue<T>(List<T> selectedFilters)
        //{
        //    string queryValue = "";

        //    if (selectedFilters != null)
        //    {
        //        foreach (T filter in selectedFilters)
        //        {
        //            queryValue += "," + filter.GetHashCode();
        //        }
        //    }

        //    return queryValue.TrimStart(',');
        //}

        #endregion

        public void GetImageGroupResults()
        {
            string recentImageId = HttpContext.Request.QueryString["ri"];
            
            // for enlargement page
            if (!string.IsNullOrEmpty(recentImageId))
            {
                DisplayImage image = ImageAgent.GetDisplayImage(recentImageId,
                                       Language.CurrentLanguage.LanguageCode,
                                       Profile.IsAnonymous);
                if (image != null)
                {
                    imageGroupsView.RecentImageRadio = image.AspectRatio;
                }
            }
        }

        private Boolean CheckCorbisIdInQuickPickList(String corbisId, Dictionary<string, QuickPicItem> quickPickImages)
        {
            Boolean isInQuickPick = false;
            if (quickPickImages != null && quickPickImages.Count > 0)
            {
                if (quickPickImages.ContainsKey(corbisId))
                {
                    QuickPicItem item = quickPickImages[corbisId];
                    if (item != null)
                    {
                        isInQuickPick = true;
                    }
                }
            }
            return isInQuickPick;
        }

        private Dictionary<string, ImageInLightboxCart> GetImagesInLightboxCart(string username,
                                                                               List<string> resultsCorbisId,
                                                                               int lightboxId)
        {
            List<string> corbisIds = resultsCorbisId;
            Dictionary<string, ImageInLightboxCart> imageCollection = null;

            if (corbisIds != null && corbisIds.Count > 0)
            {
                List<ImageInLightboxCart> imagesInLightboxCart =
                    LightboxCartAgent.GetLightBoxCartStatusByCorbisIds(username, corbisIds, lightboxId);

                imageCollection = new Dictionary<string, ImageInLightboxCart>();
                foreach (ImageInLightboxCart item in imagesInLightboxCart)
                {
                    imageCollection.Add(item.CorbisId, item);
                }
            }
            return imageCollection;
        }

        private static List<string> GetCorbisIDsFromDisplayImageList(List<DisplayImage> displayImageLIst)
        {
            List<string> corbisIds = new List<string>();
            if(displayImageLIst != null && displayImageLIst.Count > 0)
            {
                corbisIds = displayImageLIst.ConvertAll<string>(
                    delegate(DisplayImage displayImage) { return displayImage.CorbisId; });
            }
            return corbisIds;
        }


        private static List<string> GetCorbisIdsFromRFCDResults(List<RfcdDisplayImage> results)
        {
            List<string> corbisIds = new List<string>();
            if (results != null && results.Count > 0)
            {
                corbisIds = results.ConvertAll<string>(
                    delegate(RfcdDisplayImage displayImage) { return displayImage.CorbisId; });
            }
            return corbisIds;
        }

        public int ReadActiveLightboxId()
        {
            int activeLightboxId = 0;
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

        public DisplayImage GetRecentImage(string corbisId)
        {
            DisplayImage displayImage = null;
            if (!string.IsNullOrEmpty(corbisId))
            {
                displayImage = ImageAgent.GetDisplayImage(corbisId, Language.CurrentLanguage.LanguageCode,
                                                          Profile.IsAnonymous);

                //TODO: Set view's recent image URL property to displayImage.Url128
            }
            else
            {
                return null;
            }
            return displayImage;
        }

        public void AddAllMediaSetImagesToLightbox(string mediaSetId, int lightboxId)
        {
            if(!Profile.IsAnonymous && lightboxId > 0)
            {
                LightboxCartAgent.AddMediaSetImagesToCart(mediaSetId, Profile.UserName, Profile.CountryCode,
                                                      Language.CurrentLanguage.LanguageCode,
                                                      lightboxId);
            }
            return; 

        }

        public void AddAllOutlineSessionSetImagesToLightbox(int outlineSessionId, int lightboxId)
        {
            if (!Profile.IsAnonymous && lightboxId > 0)
            {
                LightboxCartAgent.AddOutlineSessionImagesToLightbox(outlineSessionId, Profile.UserName,
                                                                    Profile.CountryCode,
                                                                    Language.CurrentLanguage.LanguageCode, 
                                                                    lightboxId);
            }
            return;

        }

        public void AddRfcdToLightbox(Guid rfcdOfferingUid, int lightboxId)
        {
            if(!Profile.IsAnonymous && lightboxId > 0)
            {
                LightboxCartAgent.AddOfferingToLightBox(Profile.UserName, rfcdOfferingUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, lightboxId);
            }
        }

        public void AddAllRfcdImageToLightbox(string rfcdVolumeNumber, int lightboxId)
        {
            if(!Profile.IsAnonymous && lightboxId> 0)
                LightboxCartAgent.AddRfcdImagesToLightbox(Profile.UserName, rfcdVolumeNumber, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, lightboxId);
        }
        public int AddAllRfcdImageToNewLightbox(string userName , string lightboxName , string rfcdVolumeNumber)
        {
            int newLightboxId = LightboxCartAgent.CreateLightbox(userName, lightboxName, null, null);

            if (!Profile.IsAnonymous && newLightboxId > 0)
                LightboxCartAgent.AddRfcdImagesToLightbox(Profile.UserName, rfcdVolumeNumber, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, newLightboxId);

            return newLightboxId;
        }
    }
}
