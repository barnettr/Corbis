using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Search;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Lightboxes;

namespace Corbis.Web.UI.Search
{
	/// <summary>
	/// Summary description for SearchScriptService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ScriptService]
	[ToolboxItem(false)]
    public class SearchScriptService : Corbis.Web.UI.src.CorbisWebService, IView
	{
       
        private QuickPicPresenter quickPicPresenter;
	    private LightboxesPresenter lightboxPresenter;
	    private SearchPresenter searchPresenter;
       
	
        public SearchScriptService()
        {
            quickPicPresenter = new QuickPicPresenter();
            lightboxPresenter = new LightboxesPresenter(this);
            searchPresenter = new SearchPresenter(this);
        }

        [WebMethod(true)]
        public void DeleteMoreSearchOptions()
        {
            searchPresenter.DeleteMoreSearchOptions();
        }

        [WebMethod(true)]
        public string AddItemToQuickPick(string corbisID, string url128, string licenceModel, string aspectRation, string title)
        {
            QuickPicItem quickPic = new QuickPicItem(corbisID, url128, licenceModel, aspectRation, title);
            if (quickPicPresenter.AddItemToQuickPick(quickPic))
            {
                return corbisID;
            }
            else
            {
                return string.Empty;
            }
        }

		[WebMethod(true)]
        public string RemoveItemFromQuickPick(string corbisID)
		{        
            quickPicPresenter.RemoveItemToQuickPick(corbisID);
		    return corbisID;
		}
        
        [WebMethod(true)]
        public bool displayOptionsClarifications(bool showclarificationstatus)
        {
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            stateItems.SetStateItem<string>(new StateItem<string>(SearchKeys.Name, SearchKeys.ShowClarificationCookieKey, showclarificationstatus.ToString(), StateItemStore.Cookie,StatePersistenceDuration.NeverExpire));
            return showclarificationstatus;
		}

        [WebMethod(true)]
        public void SavePreviewPreference(int previewState)
        {
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            stateItems.SetStateItem<int>(new StateItem<int>(SearchKeys.Name, SearchKeys.SearchPreviewKey, previewState, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
        }

        #region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
        [WebMethod(true)]
        public List<LightboxDisplayImage> GetLightBoxItems(int lightboxId)
        {
            List<LightboxDisplayImage> items = new List<LightboxDisplayImage>();
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current); 
            stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxId.ToString(), StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
            return lightboxPresenter.GetLightboxDetails(lightboxId, 
                Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["LightboxBuddyImageCount"]), 1, false);
        }

        [WebMethod(true)]
        public void DeleteProductFromLightbox(int lightboxId, Guid productUid)
        {
            lightboxPresenter.DeleteProductFromLightbox(lightboxId, productUid);
        }
        [WebMethod(true)]
        public void AddProductToCart(string mediaUid, string productUID)
        {
            Guid finalMediaUid = Guid.Empty;
            Guid finalProductUid = Guid.Empty;
            Utilities.GuidHelper.TryParse(productUID, out finalProductUid);
            Utilities.GuidHelper.TryParse(mediaUid, out finalMediaUid);
            searchPresenter.AddToCart(finalMediaUid, finalProductUid);
        }

        [WebMethod(true)]
        public List<Guid> GetCartMediaUidList()
        {
            List<CartDisplayImage> cartDisplayImages = searchPresenter.GetCartItems();
            List<Guid> mediaUidList = new List<Guid>();
            foreach (CartDisplayImage image in cartDisplayImages)
            {
                mediaUidList.Add(image.MediaUid);
            }
            return mediaUidList;
        }

	    
        [WebMethod(true)]
        public Guid AddSearchItemToLightbox(Guid mediaUid, int lightboxId)
        {
            lightboxPresenter.AddToLightBox(mediaUid, lightboxId);
            return mediaUid;
        }
        [WebMethod(true)]
        public SearchItem AddSearchItemToLightboxNew(string corbisId, Guid mediaUid, int lightboxId)
        {
            SearchItem item = new SearchItem();
            lightboxPresenter.AddToLightBox(mediaUid, lightboxId);
            //TODO The RealProductId should be returned by lightboxPresenter.AddToLightBox method
            //instead of calling lightboxPresenter.getProductIdFromOfferingID to avoid a db roundtrip
            item.RealProductUid = lightboxPresenter.getProductIdFromOfferingID(mediaUid, lightboxId);
            item.CorbisID = corbisId;
            item.MediaUID = mediaUid;

            return item;
        }

        [WebMethod(true)]
        public LightboxDetail CreateLightbox(string username, string lighboxName)
        {
            int lightboxId = lightboxPresenter.CreateLightbox(username, lighboxName);

            return lightboxPresenter.GetLightboxHeaderDetails(lightboxId);
        }
    }


}
