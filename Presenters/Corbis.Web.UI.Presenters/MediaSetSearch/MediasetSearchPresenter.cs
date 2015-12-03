using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Corbis.Framework.Globalization;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Presenters.ImageGroups;


namespace Corbis.Web.UI.Presenters.MediaSetSearch
{
    public class MediasetSearchPresenter : BasePresenter
    {
        #region Privte members
        private IMediaSetsProducts view;
        private IMediasetSearchView mediaSetSearchView;
        private IImageContract imageAgent;
        private ILightboxCartContract lightboxAgent;
        private IMediaSetRedirectionView redirectionView;

        #endregion

        #region Constructors
        public MediasetSearchPresenter(IMediaSetsProducts view, IImageContract imageAgent)
        {
            if(null == view)
            {
                throw new ArgumentNullException("View can not be null");
            }

            if(imageAgent == null)
            {
                throw new ArgumentNullException("Agent can not be null");
            }

            this.view = view;
            this.imageAgent = imageAgent;
            this.lightboxAgent = new LightboxCartServiceAgent();
        }

        public MediasetSearchPresenter(IMediaSetsProducts view)
            : this(view, new ImageServiceAgent())
        {
        }
        
        public MediasetSearchPresenter (IMediasetSearchView view)
            :this(view, new ImageServiceAgent())
        {
        }

        public MediasetSearchPresenter(IMediasetSearchView view, IImageContract imageAgent)
        {
            if (null == view)
            {
                throw new ArgumentNullException("View can not be null");
            }

            if (imageAgent == null)
            {
                throw new ArgumentNullException("Agent can not be null");
            }

            this.mediaSetSearchView = view;
            this.imageAgent = imageAgent;
            this.lightboxAgent = new LightboxCartServiceAgent();
        }

        public MediasetSearchPresenter(IMediaSetRedirectionView view, IImageContract imageAgent)
        {
            if (null == view)
            {
                throw new ArgumentNullException("View can not be null");
            }

            if (imageAgent == null)
            {
                throw new ArgumentNullException("Agent can not be null");
            }

            this.redirectionView = view;
            this.imageAgent = imageAgent;
            this.lightboxAgent = new LightboxCartServiceAgent();
        }

        public MediasetSearchPresenter(IMediaSetRedirectionView view)
            : this(view, new ImageServiceAgent())
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        public void GetMediasetSearchResults(Dictionary<string, string> searchParameters)
        {
            string id, pct, qlink, rdt, search, sort, text, type, setType, pageNumber;
            searchParameters.TryGetValue(MediaSetFilterKeys.PrimaryContentType, out pct);
            searchParameters.TryGetValue(MediaSetFilterKeys.Qlink,out qlink) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.RDT, out rdt);
            searchParameters.TryGetValue(MediaSetFilterKeys.Serach, out search) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.SetType, out setType) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.Sort, out sort) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.Text, out text) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.Type, out type) ;
            searchParameters.TryGetValue(MediaSetFilterKeys.ID, out id);
            searchParameters.TryGetValue(MediaSetFilterKeys.PageNumber, out pageNumber);

            if(!string.IsNullOrEmpty(pageNumber))
            {
                int pNumber;
                if(int.TryParse(pageNumber, out pNumber))
                {
                    mediaSetSearchView.CurrentPageNumber = pNumber;
                }

            }

            int rdtNumber;
            bool parseFlag = Int32.TryParse(rdt, out rdtNumber);
            MediaSetSearchParameters parameters = new MediaSetSearchParameters();
            parameters.Pct = ParseInput(pct);
            parameters.Qlink = qlink;
            if(parseFlag)
            {
                parameters.Rdt = rdt;
            }
            else
            {
                parameters.Rdt = string.Empty;
            }
            parameters.Search = null;
            setType =  ParseInput(setType);
            parameters.SetType = ValidateSetTypes(setType);
            parameters.Sort = ParseInput(sort);
            parameters.Text = (ParseInput(text) +","+ ParseInput(id)).TrimStart(',');
            parameters.Type = null;

            try
            {
                string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
                MediaSetSearchResponse response = imageAgent.MediaSetSearch(parameters, countryCode,
                                                                        Profile.UserName,
                                                                        Language.CurrentLanguage.LanguageCode,
                                                                        mediaSetSearchView.CurrentPageNumber,
                                                                        mediaSetSearchView.ItemsPerPage);

                if (response != null && response.MediaSet != null && response.MediaSet.Count > 0)
                {
                    mediaSetSearchView.MediasetList = response.MediaSet;
                    mediaSetSearchView.TotalRecords = response.MediaSetCount;
                    mediaSetSearchView.CurrentPageHitCount = response.MediaSet.Count;
                }
                else
                {
                    mediaSetSearchView.MediasetList = new List<MediaSet>();
                    mediaSetSearchView.ShowZeroResults = true;
                }

            }
            catch (Exception ex)
            {
                mediaSetSearchView.ShowZeroResults = true;
                HandleException(ex, view.LoggingContext, "MediaSetSearchException");
            }

           
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadLightBoxData()
        {
            if (mediaSetSearchView != null)
            {
                if (Profile.IsAnonymous)
                {
                    return;
                }
                else
                {
                    LoadLightboxDetails();
                }
            }
        }
        public void SetSearchBuddyTabs()
        {
            if (mediaSetSearchView != null)
            {
                mediaSetSearchView.ShowQuickPicTab = Profile.IsQuickPicEnabled;
            }
        }
        public void LoadLightboxDetails()
        {

            if (!Profile.IsAnonymous)
            {
                List<Lightbox> lightboxList = lightboxAgent.GetLightboxTreeFlattenedByUserName(Profile.UserName, LightboxTreeSort.Date, false);
                mediaSetSearchView.LightboxList = lightboxList;
                string activeLightbox = mediaSetSearchView.ActiveLightbox;
                int activeLightboxId = -1;
                if(!string.IsNullOrEmpty(activeLightbox))
                {
                    activeLightboxId = int.Parse(activeLightbox);
                }
                else
                {
                    activeLightboxId = -1;
                }

                if(activeLightboxId != -1)
                {
                    mediaSetSearchView.LightboxItems = lightboxAgent.GetLightboxProductDetails(activeLightboxId, Profile.CultureName,
                                               Profile.CountryCode,
                                               1,
                                               50, true,
                                               Profile.UserName);
                }
                else if(lightboxList != null && lightboxList.Count > 0)
                {
                    mediaSetSearchView.LightboxItems = lightboxAgent.GetLightboxProductDetails(lightboxList[0].LightboxId , Profile.CultureName,
                                               Profile.CountryCode,
                                               1,
                                               50, true,
                                               Profile.UserName);
                    mediaSetSearchView.ActiveLightbox = lightboxList[0].LightboxId.ToString();
                }
                else
                {
                    return;
                }
            }
            else
            {
                mediaSetSearchView.LightboxItems = new List<LightboxDisplayImage>();
            }
        }
        public List<LightboxDisplayImage> LoadLightboxDetails(int lightboxId, bool isQuickpickSort)
        {

            return lightboxAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode, Profile.CountryCode, 1, 50, isQuickpickSort, Profile.UserName);
        }
        public void DeleteImageFromLightbox(int lightboxId, Guid productUid)
        {
            try
            {
                lightboxAgent.DeleteProductUidFromLightBox(lightboxId, productUid, Profile.UserName);
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "DeleteImageFromLightbox");
                throw;
            }
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
                    this.lightboxAgent.AddOfferingToCart(Profile.MemberUid, offeringUid,
                                                             Language.CurrentLanguage.LanguageCode,
                                                             Profile.CountryCode);
                    Profile.CartItemsCount = this.lightboxAgent.GetCartCount(Profile.MemberUid);
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
        public void GetLightboxTree()
        {
            if (!Profile.IsAnonymous)
            {
                
            }
            else
            {
                mediaSetSearchView.LightboxList = new List<Lightbox>();
            }
        }
        public List<CartDisplayImage> GetCartItems()
        {
            return lightboxAgent.GetCartContent(
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode,
                Profile.CountryCode);
        }

        public int GetLightboxImageCount(int lightboxId)
        {
            return this.lightboxAgent.GetLightboxProductCount(lightboxId);
        }

        #endregion

        #region Private and helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string ParseInput(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                string[] inputstrings = inputString.Split(',');
                Regex reg = new Regex("[0-9]+");
                string outputString = string.Empty;
                if (inputstrings.Length > 0)
                {
                    foreach (string s in inputstrings)
                    {
                        if (reg.IsMatch(s))
                        {
                            Match ma = reg.Match(s);
                            outputString += string.Concat(ma.ToString(), ",");
                        }
                    }
                }
                
                return outputString.TrimEnd(',');
                
            }
            return string.Empty;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string ValidateSetTypes(string inputString)
        {
            string outPut = string.Empty;
            if(!string.IsNullOrEmpty(inputString))
            {
                string[] inputs = inputString.Split(',');
                foreach (string s in inputs)
                {
                    //ImageMediaSetType setType = (ImageMediaSetType) Enum .Parse(typeof (ImageMediaSetType), s,true);
                    int rdtNumber;
                    bool parseFlag = Int32.TryParse(s, out rdtNumber); 
                    if(parseFlag)
                    {
                        if (Enum.IsDefined(typeof(ImageMediaSetType), rdtNumber))
                        {
                            outPut+= string.Concat(s, ",");
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                
            }
            return outPut.TrimEnd(',');
        }

        #endregion

        public void GetMediaSetId()
        {
            if (redirectionView != null)
            {
                ImageGroupRedirectData redirectData = imageAgent.GetImageGroupRedirectData(redirectionView.MediaSetType, redirectionView.MediaSetUid);
                redirectionView.MediaSetType = redirectData.ImageGroupType;
                if (redirectData.MediaSetId.HasValue)
                {
                    redirectionView.MediaSetId = redirectData.MediaSetId.Value;
                }
            }
        }

        public void GetSessionSetId()
        {
            if (redirectionView != null)
            {
                ImageGroupRedirectData redirectData = imageAgent.GetImageGroupRedirectData(redirectionView.MediaSetType, redirectionView.SessionSetUid);
                if (redirectData.OutlineSessionId.HasValue)
                {
                    redirectionView.SessionSetId = redirectData.OutlineSessionId.Value;
                }
            }
        }

        public void GetRfcdVolume()
        {
            if (redirectionView != null)
            {
                ImageGroupRedirectData redirectData = imageAgent.GetImageGroupRedirectData(redirectionView.MediaSetType, redirectionView.RfcdUid);
                redirectionView.RfcdVolume = redirectData.RfcdVolume;
            }
        }

        public void GetCorbisIdForImage()
        {
            if (redirectionView != null)
            {
                List<Guid> idList = new List<Guid>();
                idList.Add(redirectionView.ImageUid);

                List<DisplayImage> imageList = imageAgent.GetDisplayImagesByImageUid(idList, Language.CurrentCulture.Name, true);

                if (imageList != null && imageList.Count > 0)
                {
                    redirectionView.CorbisId = imageList[0].CorbisId;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MediaSetFilterKeys
    {
        public const string PrimaryContentType = "pct";
        public const string ID = "id";
        public const string Qlink = "qlnk";
        public const string RDT = "rdt";
        public const string Serach = "search";
        public const string SetId = "setid";
        public const string SetType = "settype";
        public const string Sort = "sort";
        public const string Text = "txt";
        public const string Type = "typ";
        public const string PageNumber = "p";
    }
}
