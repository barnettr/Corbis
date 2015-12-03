using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using Corbis.Framework.Globalization;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Image.ServiceAgents.V1;
using Corbis.Image.Contracts.V1;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.Entities.src;

namespace Corbis.Web.UI.Presenters.Lightboxes
{
    public class LightboxesPresenter : BasePresenter
    {
        private IView baseView;
        private ILightboxCartContract lightboxServiceAgent;
        private IImageContract imageServiceAgent;
        private IMyLightboxesView myLightboxView;
        private IEmailLightboxView emailedLightboxView;
        private ITransferLightboxView transferLightboxView;
        public LightboxesPresenter(IView view) :
            this(view, new LightboxCartServiceAgent())
        {
        }

        public LightboxesPresenter(IView view, ILightboxCartContract serviceAgent)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view",
                                                "LightboxesPresenter: LightboxesPresenter() - Lightboxes view cannot be null.");
            }
            else if (serviceAgent == null)
            {
                throw new ArgumentNullException("serviceAgent",
                                                "LightboxesPresenter: LightboxesPresenter() - LightboxCart service agent cannot be null.");
            }

            baseView = view;
            myLightboxView = view as IMyLightboxesView;
            emailedLightboxView = view as IEmailLightboxView;
            transferLightboxView = view as ITransferLightboxView;
            lightboxServiceAgent = serviceAgent;
            imageServiceAgent = new ImageServiceAgent();
        }

        public void LoadLightboxTreePane(string username)
        {
            try
            {
                List<Lightbox> lightboxes =
                    lightboxServiceAgent.GetLightboxTreeByUserName(username, myLightboxView.LightboxTreeSortBy);

                myLightboxView.Lightboxes = lightboxes;
                if (lightboxes != null && lightboxes.Count != 0)
                {
                    myLightboxView.IsEmpty = false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, myLightboxView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: GetLightboxTreeByUsername() - Error getting lightbox tree for member '{0}'.",
                                    username));
                throw;
            }
        }

        public void LoadLightboxDetails(bool isQuickpickSort)
        {
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            List<QuickPicItem> quickPicList = stateItems.GetStateItemValue<List<QuickPicItem>>(QuickPicKeys.Name, QuickPicKeys.QuickPickListKey, StateItemStore.AspSession) ?? new List<QuickPicItem>();
            myLightboxView.QuickPicList = quickPicList.ConvertAll<string>(new Converter<QuickPicItem, string>(delegate(QuickPicItem quickPicItem) { return quickPicItem.CorbisID; }));

            List<CartDisplayImage> cartItems = lightboxServiceAgent.GetCartContent(Profile.UserName, Language.CurrentLanguage.LanguageCode, Profile.CountryCode) ?? new List<CartDisplayImage>();
            myLightboxView.CartItems = cartItems.ConvertAll<string>(new Converter<CartDisplayImage, string>(delegate(CartDisplayImage cartDisplayImage) { return cartDisplayImage.CorbisId; }));
            List<LightboxDisplayImage> lightboxDisplayImageList = new List<LightboxDisplayImage>();
           
            lightboxDisplayImageList =
                lightboxServiceAgent.GetLightboxProductDetails(myLightboxView.LightboxId, Language.CurrentLanguage.LanguageCode,
                                                               Profile.CountryCode, myLightboxView.CurrentPageNumber,
                                                               myLightboxView.PageSize,
                                                               isQuickpickSort,
                                                               Profile.UserName);

            if(lightboxDisplayImageList.Count == 0 && myLightboxView.CurrentPageNumber > 1)
            {
                myLightboxView.CurrentPageNumber = myLightboxView.CurrentPageNumber - 1;
                lightboxDisplayImageList = lightboxServiceAgent.GetLightboxProductDetails(myLightboxView.LightboxId, Language.CurrentLanguage.LanguageCode,
                                                               Profile.CountryCode, myLightboxView.CurrentPageNumber,
                                                               myLightboxView.PageSize,
                                                               isQuickpickSort,
                                                               Profile.UserName);
                StateItem<int> currentPageNumber = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, myLightboxView.CurrentPageNumber, StateItemStore.AspSession, StatePersistenceDuration.Session);
                stateItems.SetStateItem(currentPageNumber);
            }

            myLightboxView.Products = lightboxDisplayImageList;
            myLightboxView.TotalRecords = lightboxServiceAgent.GetLightboxProductCount(myLightboxView.LightboxId);

            // We need a seperate service call to get this property because GetLightboxHeaderDetail doesn't return it.
            myLightboxView.LightboxUid = lightboxServiceAgent.GetLightboxUid(myLightboxView.LightboxId);
        }

        public void LoadLightboxDetails(int lightboxId, int pageNumber, bool isQuickpickSort)
        {
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            List<QuickPicItem> quickPicList = stateItems.GetStateItemValue<List<QuickPicItem>>(QuickPicKeys.Name, QuickPicKeys.QuickPickListKey, StateItemStore.AspSession) ?? new List<QuickPicItem>();
            myLightboxView.QuickPicList = quickPicList.ConvertAll<string>(new Converter<QuickPicItem, string>(delegate(QuickPicItem quickPicItem) { return quickPicItem.CorbisID; }));

            List<CartDisplayImage> cartItems = lightboxServiceAgent.GetCartContent(Profile.UserName, Language.CurrentLanguage.LanguageCode, Profile.CountryCode) ?? new List<CartDisplayImage>();
            myLightboxView.CartItems = cartItems.ConvertAll<string>(new Converter<CartDisplayImage, string>(delegate(CartDisplayImage cartDisplayImage) { return cartDisplayImage.CorbisId; }));

            int storedLightboxId =
                stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                  StateItemStore.AspSession);
			if (storedLightboxId == 0) storedLightboxId = lightboxId;

            List<LightboxDisplayImage> lightboxDisplayImageList = new List<LightboxDisplayImage>();
            //Check if lightbox view was on page other than 1 and it is same as active lightbox
            if (pageNumber > 0 && storedLightboxId > 0)
            {
                myLightboxView.CurrentPageNumber = pageNumber;
                lightboxDisplayImageList =
                    lightboxServiceAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode,
                                                   Profile.CountryCode,pageNumber , myLightboxView.PageSize,
                                                   isQuickpickSort,
                                                   Profile.UserName);

				if (pageNumber > 1 && lightboxDisplayImageList != null && lightboxDisplayImageList.Count == 0)
                {
                    myLightboxView.CurrentPageNumber = pageNumber -1;
                    lightboxDisplayImageList =
                        lightboxServiceAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode,
                                                       Profile.CountryCode, pageNumber - 1, myLightboxView.PageSize,
                                                       isQuickpickSort,
                                                       Profile.UserName);

                    StateItem<int> currentPageNumber = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPageNumber, myLightboxView.CurrentPageNumber, StateItemStore.AspSession, StatePersistenceDuration.Session);
                    StateItem<int> currentLightboxId = new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxId, StateItemStore.AspSession, StatePersistenceDuration.Session);
                    stateItems.SetStateItem(currentPageNumber);
                    stateItems.SetStateItem(currentLightboxId);

                }
            }
            myLightboxView.Products = lightboxDisplayImageList;
            myLightboxView.TotalRecords = lightboxServiceAgent.GetLightboxProductCount(lightboxId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lightboxId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="isQuickpickSort"></param>
        /// <returns></returns>
        public List<LightboxDisplayImage> GetLightboxDetails(int lightboxId, int pageSize, int pageNumber,  bool isQuickpickSort)
        {
            List<LightboxDisplayImage> lightboxDisplayImageList = new List<LightboxDisplayImage>();
            if(!Profile.IsAnonymous)
            {
                lightboxDisplayImageList =
                        lightboxServiceAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode,
                                                       Profile.CountryCode, pageNumber, pageSize,
                                                       isQuickpickSort,
                                                       Profile.UserName);
            }
            return lightboxDisplayImageList;
        }

        public void DeleteLightbox(string username, List<int> lightboxIds)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username",
                                                "LightboxesPresenter: DeleteLightbox() - Username cannot be null or empty.");
            }
            else if (lightboxIds == null || lightboxIds.Count == 0)
            {
                throw new ArgumentNullException("lightboxIds",
                                                "LightboxesPresenter: DeleteLightbox() - LightboxIDs cannot be null or empty.");
            }

            try
            {
                lightboxServiceAgent.DeleteLightBoxes(username, lightboxIds, Language.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                HandleException(ex, myLightboxView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: DeleteLightbox() - Error deleting lightboxes for member '{0}'.",
                                    username));
                throw;
            }
        }

        public int CreateLightbox(string username, string lighboxName)
        {
            return CreateLightbox(username, lighboxName, null, null);
        }

        public int CreateLightbox(string username, string lighboxName, string clientName, int? parentLightboxId)
        {
            int returnValue = 0;

            try
            {
                returnValue = lightboxServiceAgent.CreateLightbox(username, lighboxName, clientName, parentLightboxId);
            }
            catch (FaultException<ValidationFault> vfex)
            {
                ValidationFault validationfault = vfex.Detail;
                baseView.ValidationErrors = validationfault.Details;
            }
            catch (Exception ex)
            {
                HandleException(ex, myLightboxView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: CreateLightbox() - Error creating lightboxes for member '{0}' with lightbox name '{1}'.",
                                    username, lighboxName));
                throw;
            }

            return returnValue;
        }

        public void AddToLightBox(Guid offeringUid, int lightboxId)
        {
            if(!Profile.IsAnonymous)
            {
                AddToLightBox(Profile.UserName, offeringUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, lightboxId);

            }
            return;
        }

        public Guid getProductIdFromOfferingID(Guid OfferingId,int lightBoxID)
        {
            return lightboxServiceAgent.GetLightboxProductUid(Profile.UserName, OfferingId, lightBoxID);
        }

        public void AddToLightBox(string userName, Guid offeringUid, string countryCode, string cultureName,
                                  int lightboxId)
        {
            try
            {
                lightboxServiceAgent.AddOfferingToLightBox(userName, offeringUid, countryCode, cultureName, lightboxId);
            }
            catch (FaultException<ValidationFault> vfex)
            {
                ValidationFault validationfault = vfex.Detail;
                baseView.ValidationErrors = validationfault.Details;
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "SearchPresenter: AddToNewLightBox() - Error adding offering to lightbox for offering '{0}' with lightbox id '{1}'.",
                                    offeringUid.ToString(), lightboxId.ToString()));
                throw;
            }
        }

        public int AddToNewLightBox(string userName, Guid offeringUid, string countryCode, string cultureName,
                                    string lightboxName)
        {
            try
            {
                int newLightboxId = lightboxServiceAgent.CreateLightbox(userName, lightboxName, null, null);
                lightboxServiceAgent.AddOfferingToLightBox(userName, offeringUid, countryCode, cultureName,
                                                           newLightboxId);
                return newLightboxId;
            }
            catch (FaultException<ValidationFault> vfex)
            {
                ValidationFault validationfault = vfex.Detail;
                baseView.ValidationErrors = validationfault.Details;
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "SearchPresenter: AddToNewLightBox() - Error adding offering to lightbox for member '{0}' with lightbox name '{1}'.",
                                    Profile.UserName, lightboxName));
                throw;
            }
            return 0;
        }

        public List<Lightbox> GetLightboxList(string userName, bool includeReadOnlyLightbox)
        {
            try
            {
                StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
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
                    return lightboxServiceAgent.GetLightboxTreeFlattenedByUserName(userName, LightboxTreeSort.Date, includeReadOnlyLightbox);
                }
                else
                {
                    return lightboxServiceAgent.GetLightboxTreeFlattenedByUserName(userName, LightboxTreeSort.Name, includeReadOnlyLightbox);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: GetLightboxTreeByFlattenedByUsername() - Error getting lightbox tree for member '{0}'.",
                                    userName));
            }

            return null;
        }

        public List<Lightbox> GetLightboxList(LightboxTreeSort sortBy, bool includeReadOnlyLightbox)
        {
            List<Lightbox> lightboxes = null;

            try
            {
                lightboxes = lightboxServiceAgent.GetLightboxTreeFlattenedByUserName(Profile.UserName, sortBy, includeReadOnlyLightbox);
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: GetLightboxTreeByFlattenedByUsername() - Error getting lightbox tree for member '{0}'.",
                                    Profile.UserName));
            }

            return lightboxes;
        }

        public static List<KeyValuePair<string, int>> GetLightboxesDropdownSource(List<Lightbox> lightboxes, int level)
        {
            List<KeyValuePair<string, int>> returnList = new List<KeyValuePair<string, int>>();
            if (lightboxes != null)
            {
                foreach (Lightbox lightbox in lightboxes)
                {
                    StringBuilder prefix = new StringBuilder();
                    for (int i = 1; i < lightbox.LightboxLevel; i++)
                    {
                        prefix.Append("      |      ");
                    }
                    returnList.Add(
                        new KeyValuePair<string, int>(String.Format("{0}{1}", prefix, lightbox.LightboxName),
                                                      lightbox.LightboxId));
                }
            }

            return returnList;
        }

        /// <summary>
        /// Delete Lightbox Note
        /// </summary>
        /// <param name="folderProductNoteUid"></param>
        public void DeleteFolderProductNote(Guid folderProductUid)
        {
            //Call service agent to delete FolderProductNote
            lightboxServiceAgent.DeleteLightboxProductNote(folderProductUid);
        }

        /// <summary>
        /// Delete Lightbox Note
        /// </summary>
        /// <param name="lightboxId"></param>
        public void DeleteLightboxNote(int lightboxId)
        {
            //Call service agent to delete LightboxNote
            
        }

        /// <summary>
        /// Update Product Note
        /// </summary>
        /// <param name="folderProductUid"></param>
        /// <param name="folderProductNoteUid"></param>
        /// <param name="folderProductNote"></param>
        public void UpdateFolderProductNote(Guid folderProductUid, Guid folderProductNoteUid, string folderProductNote)
        {
            //Call service agent to update Update ProductNote
            lightboxServiceAgent.UpdateLightboxProductNote(folderProductUid, folderProductNoteUid, folderProductNote);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lightboxId"></param>
        /// <param name="productUid"></param>
        public void DeleteProductFromLightbox(int lightboxId, Guid productUid)
        {
            lightboxServiceAgent.DeleteProductUidFromLightBox(lightboxId, productUid, Profile.UserName);
        }
		
        /// <summary>
        /// Update LIghtbox Note 
        /// </summary>
        /// <param name="lightbodId"></param>
        /// <param name="lightboxNote"></param>
        public void UpdateLightboxNote(int lightbodId, string lightboxNote)
        {
            //Call service agent to update Update Lightbox Note
        }

        /// <summary>
        /// Update Lightbox, call this one for rename, update client name, make them globally accessible, note text
        /// </summary>
        /// <param name="lightboxId"></param>
        /// <param name="lightboxName"></param>
        /// <param name="clientName"></param>
        /// <param name="isGloballyAccessible"></param>
        /// <param name="noteUid"></param>
        /// <param name="noteText"></param>
        public LightboxDetail UpdateLightbox(int lightboxId, string lightboxName, string clientName, bool isGloballyAccessible,
                                   Guid noteUid, string noteText)
        {
            LightboxDetail detail = null;

            if (string.IsNullOrEmpty(lightboxName))
            {
                throw new ArgumentNullException("Lightbox Name can not be null");
            }
            try
            {
                detail = lightboxServiceAgent.UpdateLightbox(lightboxId, Profile.UserName, lightboxName, clientName,
                                                    isGloballyAccessible, noteUid, noteText);
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext, "Exception in UpdatLightbox");
            }

            return detail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderProductUid"></param>
        /// <param name="folderProductNoteUid"></param>
        /// <param name="noteText"></param>
        public void AddLightboxProductNote(Guid folderProductUid, Guid folderProductNoteUid, string noteText)
        {
            lightboxServiceAgent.AddLightboxProductNote(folderProductUid,folderProductNoteUid,noteText);
        }

		public void EmailLightbox()
		{
			try
			{
				ISendEmailLightboxView sendEmailView = (ISendEmailLightboxView)baseView;
				lightboxServiceAgent.EmailLightbox(sendEmailView.LightboxId,
                    sendEmailView.FromEmail, sendEmailView.ToEmails, sendEmailView.Subject, sendEmailView.Message, 
                    sendEmailView.LightboxLink, Profile.UserName);
			}
			catch (FaultException<ValidationFault> vfex)
			{
				ValidationFault validationfault = vfex.Detail;
				baseView.ValidationErrors = validationfault.Details;
			}
		}

        public bool GetEmailedLightbox()
        {
            if (emailedLightboxView == null)
            {
                throw new InvalidOperationException("IEmailLightboxView is Null");
            }
            LightboxDetail lightbox = lightboxServiceAgent.GetEmailedLightbox(
                emailedLightboxView.LightboxUid,
                Language.CurrentLanguage.LanguageCode,
                Profile.CountryCode,
                emailedLightboxView.CurrentPageNumber,
                emailedLightboxView.PageSize,
                Profile.UserName);
            if (lightbox != null)
            {
                emailedLightboxView.LightboxId = lightbox.LightboxId;
                emailedLightboxView.Client = lightbox.ClientName;
                emailedLightboxView.CreatedDate = lightbox.CreatedAt.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern, Language.CurrentCulture.DateTimeFormat);
                emailedLightboxView.LightboxName = lightbox.LightboxName;
                emailedLightboxView.Notes = lightbox.Note;
                emailedLightboxView.ModifiedDate = lightbox.ModifiedAt.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern, Language.CurrentCulture.DateTimeFormat);

                emailedLightboxView.OwnerUsername = lightbox.OwnerUsername;
                emailedLightboxView.Products = lightbox.Items;
                emailedLightboxView.TotalRecords = lightbox.ProductCount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TransferLightbox()
        {
            if (transferLightboxView == null)
            {
                throw new InvalidOperationException("ITransferLightboxView is Null");
            }
            lightboxServiceAgent.TransferLightbox(
                Profile.UserName,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                transferLightboxView.LightboxId,
                transferLightboxView.AssociateUserNames,
                transferLightboxView.RemoveSource);
        }

        public List<LightboxCopyImage> GetLightboxCopyImages(
            int lightboxId,
            int pageNumber,
            int pageSize)
        {
            return lightboxServiceAgent.GetLightboxCopyImages(
                Profile.UserName,
                lightboxId,
                pageNumber,
                pageSize,
                Language.CurrentLanguage.LanguageCode,
                Profile.CountryCode);
        }

        public int CopyLightboxImages(
            int fromLightboxId,
            int toLightboxId,
            List<Guid> imageUids)
        {
            return lightboxServiceAgent.CopyLightboxImages(
                Profile.UserName,
                fromLightboxId,
                toLightboxId,
                imageUids,
                Profile.CountryCode);
        }

        public void MoveLightbox(int lightboxIdToMove, int newParentLightboxId)
        {
            try
            {
                lightboxServiceAgent.MoveLightbox(
                    Profile.UserName,
                    lightboxIdToMove,
                    newParentLightboxId == 1 ? null : (int?)newParentLightboxId);
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: MoveLightbox() - Error moving lightbox for member '{0}'.",
                                    Profile.UserName));
            }

        }

        public List<Lightbox> GetLightBoxListForMove(int lightboxIdToMove, LightboxTreeSort lightboxTreeSortBy)
        {
            List<Lightbox> lightboxes = null;
            try
            {
                lightboxes = lightboxServiceAgent.GetLightboxesToMoveTo(
                    Profile.UserName,
                    lightboxIdToMove,
                    lightboxTreeSortBy);
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                    "LightboxesPresenter: GetLightBoxListForMove() - Error getting lightbox move list");
            }

            return lightboxes;
        }


        public Lightbox RenameLightbox(int lightboxId, string newLightboxName)
        {
            Lightbox renamedLightbox = new Lightbox();
            if (string.IsNullOrEmpty(newLightboxName))
            {
                throw new ArgumentNullException("Lightbox Name can not be null");
            }
            try
            {
                renamedLightbox = lightboxServiceAgent.RenameLightbox(lightboxId, newLightboxName, Profile.UserName); ;
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext, "Exception in RenameLightbox");
            }

            return renamedLightbox;
        }

        public LightboxDetail GetLightboxHeaderDetails(int lightboxId)
        {
            LightboxDetail details = null;
            try
            {
                details = lightboxServiceAgent.GetLightboxDetail(lightboxId);
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext, "Exception in GetLightboxHeaderDetails");
            }

            return details;
        }

        public void GetLightboxHeaderDetails(string readOnlyLocalizedText)
        {
            LightboxDetail details = null;
            try
            {
                details = GetLightboxHeaderDetails(myLightboxView.LightboxId);
                myLightboxView.Notes = details.Note;
                myLightboxView.CreatedDate = details.CreatedAt;
                myLightboxView.ModifiedDate = details.ModifiedAt;
                myLightboxView.OwnerUsername = details.OwnerUsername;
                myLightboxView.Client = details.ClientName;
                myLightboxView.NotesUid = details.NoteUid;
				myLightboxView.LightboxName = details.LightboxName;

                if (details.SharedOut)
                {
                    myLightboxView.Shared = BuildSharedByList(details.SharedBy, readOnlyLocalizedText);
                }
                else
                {
                    myLightboxView.Shared = string.Empty;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext, "Exception in GetLightboxHeaderDetails");
            }
        }

        private string BuildSharedByList(List<SharedByDetail> sharedByDetails, string readOnlyLocalizedText)
        {
            string sharedBy = string.Empty;
            foreach (SharedByDetail detail in sharedByDetails)
            {
                // Append comma if it is not the start of string.
                if (sharedBy != string.Empty)
                {
                    sharedBy += ", ";
                }

                sharedBy += ((Page)baseView).Server.HtmlEncode(detail.UserName);
                if (!detail.WritePermit)
                {
                    // Append read only text if the lightbox is read only.
                    sharedBy += readOnlyLocalizedText;
                }
            }

            return sharedBy;
        }

        public COFFValidationResponse ValidateItemsForCOFF(int fromLightboxId, List<Guid> imageUids)
        {
            COFFValidationResponse coffValidationResponse = new COFFValidationResponse();

            try
            {
                List<CoffValidationResult> coffValidationResult = lightboxServiceAgent.ValidateCoffItems(imageUids, Profile.UserName, Language.CurrentLanguage.LanguageCode, Profile.CountryCode);
                int total = imageUids.Count;
                coffValidationResponse.ValidationStatus = true;
                coffValidationResponse.AllItemsInvalid = false;//default make it false
                //Capture items in Session for later checkout process
                Dictionary<Guid, CoffValidationResult> guidToResultMap = new Dictionary<Guid, CoffValidationResult>();
                foreach (var validationResult in coffValidationResult)
                {
                    guidToResultMap.Add(validationResult.ProductUid, validationResult);
                }

                StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
                StateItem<Dictionary<Guid, CoffValidationResult>> stateItem = new StateItem<Dictionary<Guid, CoffValidationResult>>(LightboxCOFFKeys.Name, LightboxCOFFKeys.COFFValidationResults, guidToResultMap, StateItemStore.AspSession);
                stateItems.SetStateItem<Dictionary<Guid, CoffValidationResult>>(stateItem);

                //Remove Valid COFF items and retain Invalid COFF items.
                List<CoffValidationResult> validItems = RemoveValidCOFFItems(coffValidationResult , coffValidationResponse);

                if (validItems.Count != total)
                {
                    List<ValidatedCoffItem> validatedCoffItem = ConvertItems(coffValidationResult);
                    coffValidationResponse.ValidatedCoffItem = validatedCoffItem;
                    coffValidationResponse.InvalidItemCount = validatedCoffItem.Count;
                }
                else
                {
                    //all are valid so continue to checkout directly by returning empty list
                    coffValidationResponse.ValidatedCoffItem = null;
                    coffValidationResponse.InvalidItemCount = 0;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, baseView.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: CoffLightboxImages() - Error validating items for COFF for member '{0}'.",
                                    Profile.UserName));
                coffValidationResponse.ValidationStatus = false;
            }
            return coffValidationResponse;

        }
        private List<ValidatedCoffItem> ConvertItems(List<CoffValidationResult> coffValidationResult)
        {
            List<string> corbisIds = new List<string>();
            List<ValidatedCoffItem> validatedCoffItems = new List<ValidatedCoffItem>();
            foreach (var item in coffValidationResult)
            {
                corbisIds.Add(item.CorbisId);
                //Invoke the new service api in ImageServiceAgent that will return only the image sizes 
                ValidatedCoffItem validatedCoffItem = new ValidatedCoffItem();
                validatedCoffItem.CoffValidationResult = item;
                validatedCoffItem.AspectRatio = item.AspectRatio;
                validatedCoffItems.Add(validatedCoffItem);
            }
            return validatedCoffItems;
        }


        /// <summary>
        /// Remove valid COFF items in the lightbox
        /// </summary>
        private List<CoffValidationResult> RemoveValidCOFFItems(List<CoffValidationResult> coffValidationResult , COFFValidationResponse coffValidationResponse)
        {
            List<CoffValidationResult> validItems = new List<CoffValidationResult>();
            bool atleastOneInvalidSize = false;
            coffValidationResult.RemoveAll(
                new Predicate<CoffValidationResult>(
                    delegate(CoffValidationResult validationResult)
                    {
                        bool valid = validationResult.ValidationStatus == CoffValidationStatus.Success;
                        if (!atleastOneInvalidSize)
                        {
                            atleastOneInvalidSize = validationResult.ValidationStatus == CoffValidationStatus.InvalidRFSize;
                        }
                        if (valid)
                            validItems.Add(validationResult);

                        return (valid);
                    }
                )
            );
            //if atleast one item is size related then allow user to checkout
            if (validItems.Count == 0 && !atleastOneInvalidSize)
            {
                coffValidationResponse.AllItemsInvalid = true;
            }
            return validItems;
        }
        ///<summary>
        ///Merge both the valid ones and the invalid ones and then delegate to checkout presenter
        ///<summary>
        public void ContinueToCheckoutCOFFItems(int fromLightboxId, List<COFFOrderImage> coffOrderImages)
        {
            Dictionary<Guid, FileSize> coffImageItems = new Dictionary<Guid, FileSize>(); 
            //Session has the valid items captured 

            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            Dictionary<Guid, CoffValidationResult> validationResults = stateItems.GetStateItemValue<Dictionary<Guid, CoffValidationResult>>(LightboxCOFFKeys.Name, LightboxCOFFKeys.COFFValidationResults, StateItemStore.AspSession);
            foreach (Guid productUid in validationResults.Keys)
            {
                //Take only the valid ones from the session
                if (validationResults[productUid].ValidationStatus == CoffValidationStatus.Success)
                {
                    coffImageItems.Add(productUid, FileSize.Unknown);
                }
            }
            if (coffOrderImages != null && coffOrderImages.Count > 0)
            {
                CoffValidationResult coffValidationResult = null;
                foreach (var coffOrderImage in coffOrderImages)
                {
                    coffValidationResult = new CoffValidationResult();
                    validationResults.TryGetValue(coffOrderImage.ProductUid, out coffValidationResult);
                    //take only the invalid RF size entries
                    if (validationResults != null && validationResults.ContainsKey(coffOrderImage.ProductUid)
                         && coffValidationResult.ValidationStatus == CoffValidationStatus.InvalidRFSize)
                    {
                        coffImageItems.Add(coffOrderImage.ProductUid, coffOrderImage.FileSize);
                    }
                }
            }

            //Retain the Checkout items in Session
            StateItemCollection checkoutItems = new StateItemCollection(System.Web.HttpContext.Current);
            StateItem<Dictionary<Guid, FileSize>> stateItem = new StateItem<Dictionary<Guid, FileSize>>(LightboxCOFFKeys.Name, LightboxCOFFKeys.COFFCheckoutItems, coffImageItems, StateItemStore.AspSession);
            checkoutItems.SetStateItem<Dictionary<Guid, FileSize>>(stateItem);
        }
        public void ClearCOFFItemsFromSession()
        {
            StateItemCollection sessionItems = new StateItemCollection(System.Web.HttpContext.Current);
            sessionItems.DeleteStateItem(new StateItem<Dictionary<Guid, FileSize>>(LightboxCOFFKeys.Name, LightboxCOFFKeys.COFFCheckoutItems, null, StateItemStore.AspSession));
            sessionItems.DeleteStateItem(new StateItem<Dictionary<Guid, CoffValidationResult>>(LightboxCOFFKeys.Name, LightboxCOFFKeys.COFFValidationResults, null,StateItemStore.AspSession));
        }
    }
}
