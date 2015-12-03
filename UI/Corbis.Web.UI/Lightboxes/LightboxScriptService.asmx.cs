using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.Script.Services;

using Corbis.Framework.Logging;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Membership.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.Entities.src;
using Corbis.Web.UI.src;

namespace Corbis.Web.UI.Lightboxes
{
    /// <summary>
    /// Summary description for LightboxScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
	public class LightboxScriptService : CorbisWebService, IView, IMemberAssociatesView, ITransferLightboxView
    {
        private ILogging loggingContext;
        private LightboxesPresenter presenter;
        private COFFCheckoutPresenter checkoutPresenter;
        private PricingPresenter pricingPresenter;
        public LightboxScriptService()
        {
            presenter = new LightboxesPresenter(this);
            checkoutPresenter = new COFFCheckoutPresenter(this);
            pricingPresenter = new PricingPresenter(this);
        }

        [WebMethod(true)]
        public Guid UpdateLightboxProductNote(Guid productUid, Guid notedUid, string noteText)
        {
            if (!String.IsNullOrEmpty(noteText))
            {
                if (notedUid == Guid.Empty)
                {
                    notedUid = Guid.NewGuid();
                    presenter.AddLightboxProductNote(productUid, notedUid, noteText);
                }
                else
                {
                    presenter.UpdateFolderProductNote(productUid, notedUid, noteText);
                }
            }
            else
            {
                DeleteLightboxProductNote(productUid);
            }

            return notedUid;
        }

        [WebMethod(true)]
        public List<LightboxCoffImage> GetLightboxCOFFImages(
            int lightboxId,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber == 0)
            {
                presenter.ClearCOFFItemsFromSession();
            }

            return ConvertToCoffImage(presenter.GetLightboxCopyImages(lightboxId,pageNumber,pageSize));
        }

        private List<LightboxCoffImage> ConvertToCoffImage(List<LightboxCopyImage> copyImages)
        {
            List<LightboxCoffImage> images = new List<LightboxCoffImage>();
            if (copyImages != null && copyImages.Count > 0)
            {
                foreach (LightboxCopyImage copyImage in copyImages)
                {
                    LightboxCoffImage coffImage = new LightboxCoffImage();
                    coffImage.AspectRatio = copyImage.AspectRatio;
                    coffImage.CorbisId = copyImage.CorbisId;
                    coffImage.ExtensionData = copyImage.ExtensionData;
                    coffImage.LicenseModel = copyImage.LicenseModel;
                    try
                    {
                        coffImage.LicenseModelText = HttpContext.GetGlobalResourceObject("Resource", copyImage.LicenseModel.ToString() + "Text").ToString();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    coffImage.MediaUid = copyImage.MediaUid;
                    coffImage.ProductUid = copyImage.ProductUid;
                    coffImage.Title = copyImage.Title;
                    coffImage.Url128 = copyImage.Url128;
                    images.Add(coffImage);
                }
            }
            return images;
        }

        [WebMethod(true)]
        public List<LightboxCopyImage> GetLightboxCopyImages(
            int lightboxId,
            int pageNumber,
            int pageSize)
        {
            return presenter.GetLightboxCopyImages(
                lightboxId,
                pageNumber,
                pageSize);
        }

        [WebMethod(true)]
        public int CopyLightboxImages(
            int fromLightboxId,
            int toLightboxId,
            List<Guid> imageUids)
        {
            return presenter.CopyLightboxImages(
                fromLightboxId,
                toLightboxId,
                imageUids);
        }

        [WebMethod(true)]
        public COFFValidationResponse ValidateItemsForCOFF(
            int fromLightboxId,
            List<Guid> imageUids)
        {
            List<ValidatedCoffItem> validatedCoffItems = null;
            COFFValidationResponse coffValidationResponse = presenter.ValidateItemsForCOFF(fromLightboxId,imageUids);
            if (imageUids == null)
            {
                return ContinueToCheckoutCOFFItems(fromLightboxId, null);
            }
            validatedCoffItems = coffValidationResponse.ValidatedCoffItem;
            if (validatedCoffItems != null && validatedCoffItems.Count > 0)
            {
                //TODO localize the string
                LocalizeAvailableSizes(validatedCoffItems);
            }
            else
            {
                return ContinueToCheckoutCOFFItems(fromLightboxId, null);
            }
            coffValidationResponse.CheckoutUrl = SiteUrls.CoffCheckout;
            return coffValidationResponse;
        }

        [WebMethod(true)]
        public COFFValidationResponse ContinueToCheckoutCOFFItems(int fromLightboxId,List<COFFOrderImage> coffOrderImages)
        {
            if(coffOrderImages != null && coffOrderImages.Count > 0 )
                pricingPresenter.UpdateCOFFLightboxProductsCompletedUsage(fromLightboxId, coffOrderImages);
            presenter.ContinueToCheckoutCOFFItems(fromLightboxId,coffOrderImages);
            COFFValidationResponse response = new COFFValidationResponse();
            response.ValidationStatus = true;
            response.ValidatedCoffItem = null;
            response.InvalidItemCount = 0;
            response.CheckoutUrl = SiteUrls.CoffCheckout;
            return response;
        }

		[WebMethod(true)]
		public List<KeyValuePair<string, int>> GetLightBoxDropDownListForCopy(LightboxTreeSort lightboxTreeSortBy)
		{
			List<KeyValuePair<string, int>> returnList = new List<KeyValuePair<string, int>>();
			returnList.Add(new KeyValuePair<string, int>(HttpContext.GetGlobalResourceObject("Resource", "SelectOne").ToString(), 0));
            List<Lightbox> lightboxes = presenter.GetLightboxList(lightboxTreeSortBy, false);
			if (lightboxes == null) lightboxes = new List<Lightbox>();
			returnList.AddRange(
				lightboxes.ConvertAll<KeyValuePair<string, int>>(
					new Converter<Lightbox, KeyValuePair<string, int>>(
						delegate(Lightbox lightbox)
						{
                            // Encode lightbox name because JS is building drop down using weird HTML which does not encode it internally.
                            return new KeyValuePair<string, int>((lightbox.LightboxLevel > 1 ? new System.Text.StringBuilder(" ").Insert(0, "| ", lightbox.LightboxLevel - 1).ToString() : "") + Server.HtmlEncode(lightbox.LightboxName) + (!String.IsNullOrEmpty(lightbox.SharedBy) || lightbox.SharedOut ? " (" + HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "lightboxTree.SharedByFormat").ToString() + ")" : ""), lightbox.LightboxId);
						}
					)
				)
			);

			return returnList;
		}	
			
		[WebMethod(true)]
        public void DeleteLightboxProductNote(Guid productUid)
        {
            presenter.DeleteFolderProductNote(productUid);
        }

        [WebMethod(true)]
        public void SavePreviewPreference(int previewState)
        {
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            stateItems.SetStateItem<int>(new StateItem<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPreviewKey, previewState, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)); 
        }

        [WebMethod(true)]
        public String[] GetLightBoxDropDownListForMove(int lightboxIdToMove, LightboxTreeSort lightboxTreeSortBy)
        {
            List<Lightbox> lightboxes = presenter.GetLightBoxListForMove(lightboxIdToMove, lightboxTreeSortBy);
			if (lightboxes == null) lightboxes = new List<Lightbox>();
			List<string> ret = lightboxes.ConvertAll<string>(
				new Converter<Lightbox, string>(
					delegate(Lightbox lightbox)
					{
                        // Encode lightbox name because JS is building drop down using weird HTML which does not encode it internally.
						string key = (lightbox.LightboxLevel > 1 ? new System.Text.StringBuilder(" ").Insert(0, "| ", lightbox.LightboxLevel - 1).ToString() : "") 
                            + Server.HtmlEncode(lightbox.LightboxName) + (lightbox.SharedOut ? " (" + HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "lightboxTree.SharedByFormat").ToString() + ")" : "");
						return key + "~" + lightbox.LightboxId;
					}
				)
			);

            return ret.ToArray<string>();
        }

        [WebMethod(true)]
        public void MoveLightBox(int lightboxIdToMove, int newParentLightboxId)
        {
            presenter.MoveLightbox(lightboxIdToMove, newParentLightboxId);
        }

        [WebMethod(true)]
        public void CreateLightbox(string username, string lighboxName)
        {
            presenter.CreateLightbox(username, lighboxName);
        }

        [WebMethod(true)]
        public String[] RenameLightbox(int lightboxId, string newLightboxName)
        {
            Lightbox lb =  presenter.RenameLightbox(lightboxId, newLightboxName);
            string[] renameLb = new string[]{lb.LightboxName, lb.ChangedAt.ToString("MM/dd/yy")}; 
            return renameLb;
        }

        [WebMethod(true)]
        public String[] UpdateSharedLightbox(int lightboxId, string lightboxName, string clientName, Guid noteUid, string noteText)
        {
            LightboxDetail lb = presenter.UpdateLightbox(lightboxId, lightboxName, clientName, true, noteUid, noteText);
            string[] renameLb = new string[] { lb.LightboxName, lb.ModifiedAt.ToString("MM/dd/yy") };
            return renameLb;
        }
        //public void UpdateSharedLightbox(int lightboxId, string lightboxName, string clientName, Guid noteUid, string noteText)
        //{
        //    presenter.UpdateLightbox(lightboxId, lightboxName, clientName, true, noteUid, noteText);
        //}
        [WebMethod(true)]
        public List<KeyValuePair<string, string>> GetMemberAssociates()
		{
			AccountsPresenter accountsPresenter = new AccountsPresenter(this);
			accountsPresenter.GetMemberAssociates();
            List<KeyValuePair<string, string>> associates = new List<KeyValuePair<string, string>>();
            if (_associates != null)
            {
                foreach (MemberAssociateDisplay display in _associates)
                {
                    associates.Add(new KeyValuePair<string, string>(display.Username, display.AssociateDisplay));
                }
            }
            return associates;
        }

        [WebMethod(true)]
        public MemberAssociateDisplay AddMemberAssociate(string associateUserName)
        {
            AccountsPresenter accountsPresenter = new AccountsPresenter(this);
            _selectedAssociateUserNames = new List<string>();
            _selectedAssociateUserNames.Add(associateUserName);
            accountsPresenter.AddMemberAssociates();
            MemberAssociateDisplay newAssociate = null;
            if (_associates != null && _associates.Count == 1)
            {
                newAssociate = _associates[0];
				if (newAssociate.AddErrorOcurred && newAssociate.Status !=MemberAssociateStatus.Existing)
                {
                    newAssociate.AssociateDisplay = null;
                    newAssociate.ErrorMessage = CorbisBasePage.GetEnumDisplayText<MemberAssociateStatus>(newAssociate.Status);
                }
            }
            return newAssociate;
        }

        [WebMethod(true)]
        public void CheckoutQuickPicImages(List<QuickPicOrderImage> images)
        {
            //No op
        }

        [WebMethod(true)]
        public void RemoveMemberAssociates(List<string> associateUserNames)
        {
            AccountsPresenter accountsPresenter = new AccountsPresenter(this);
			_selectedAssociateUserNames = associateUserNames;
            accountsPresenter.RemoveMemberAssociates();
        }
        private void LocalizeAvailableSizes(List<ValidatedCoffItem> validatedCoffItems)
        {
            if (validatedCoffItems == null)
            {
                //nothing to validate
                return;
            }
            foreach(var validatedItem in validatedCoffItems) {

                try
                {
                    if (validatedItem.CoffValidationResult != null)
                    {
                        validatedItem.LicenseModelText = HttpContext.GetGlobalResourceObject("Resource", validatedItem.CoffValidationResult.LicenseModel.ToString() + "Text").ToString();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
                if (validatedItem.CoffValidationResult.RFImageSizes == null)
                    continue;
                List<ItemAvailableSize> availableSizes = validatedItem.CoffValidationResult.RFImageSizes.ConvertAll<ItemAvailableSize>(
                    new Converter<FileSize, ItemAvailableSize>(
                        delegate(FileSize availableFileSize)
                        {
                            ItemAvailableSize returnItem = new ItemAvailableSize();
                            returnItem.Size = availableFileSize.GetHashCode().ToString();
                            returnItem.LocalizedValue= CorbisBasePage.GetEnumDisplayText<FileSize>(availableFileSize);
//                            availableImageSizes.Add(String.Format("'{0}'", availableFileSize.GetHashCode().ToString()));
                            return returnItem;
                        }
                    )
                );
                validatedItem.AvailableSizes = availableSizes;
            }
        }

		[WebMethod(true)]
		public void TransferLightbox(int lightboxId, List<string> associateUserNames, bool removeSource)
		{
			LightboxesPresenter lightboxPresenter = new LightboxesPresenter(this);
			_lightboxId = lightboxId;
			_associateUserNames = associateUserNames;
			_removeSource = removeSource;
			lightboxPresenter.TransferLightbox();
		}
	
		#region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                return loggingContext;        
            }
            set
            {
                loggingContext = value;
            }
        }

        public System.Collections.Generic.IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IMemberAssociatesView Members

        private List<MemberAssociateDisplay> _associates = null;
        public List<MemberAssociateDisplay> Associates
        {
            set { _associates = value; }
        }

        private List<string> _selectedAssociateUserNames = null;
        public List<string> SelectedAssociateUserNames
        {
            get { return _selectedAssociateUserNames; }
        }

        #endregion

		#region ITransferLightboxView Members

		private int _lightboxId;
		public int LightboxId
		{
			get { return _lightboxId; }
		}

		private List<string> _associateUserNames;
		public List<string> AssociateUserNames
		{
			get { return _associateUserNames; }
		}

		private bool _removeSource;
		public bool RemoveSource
		{
			get { return _removeSource; }
		}

		#endregion
	}
}
