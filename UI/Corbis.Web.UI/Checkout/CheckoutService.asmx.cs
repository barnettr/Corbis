using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using System.Web.Script.Services;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Framework.Logging;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.MediaDownload.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.RFCD.Contracts.V1;
using Corbis.Web.Validation;
using System.Web.Security;

namespace Corbis.Web.UI.Checkout
{
    /// <summary>
    /// Summary description for CheckoutService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
	public class CheckoutService : Corbis.Web.UI.src.CorbisWebService, ICheckoutScriptServiceView, IProject

    {
        private ProductRestriction _viewRestrictions;
        private CheckoutPresenter presenter;
        private ILogging loggingContext;

        public CheckoutService()
        {
            presenter = new CheckoutPresenter(this);           
        }

        [WebMethod(true)]
        public ProductRestriction GetViewRestrictions(string corbisId)
        {
            ProductRestriction productRestriction =presenter.GetRestrictions(corbisId);
            if(productRestriction!=null)
            {
                if (productRestriction.PropertyReleaseStatus)
                {
                    productRestriction.PropertyReleaseStatusText=Resources.Resource.Order_PropertyRelease;
                }
                else
                {
                     productRestriction.PropertyReleaseStatusText=Resources.Resource.Order_PropertyNoRelease;
                }
                if (productRestriction.ModelReleaseStatus == "NotReleased")
                {
                    productRestriction.ModelReleaseStatus = Resources.Resource.Order_ModelNoRelease;
                }
                else
                {
                    productRestriction.ModelReleaseStatus = Resources.Resource.Order_ModelRelease;
                }
            }
            return productRestriction;
           
        }

        [WebMethod(true)]
        public bool DeleteItemFromCheckout(Guid productUid)
        {
            return presenter.DeleteItemFromCheckout(productUid);
        }

		[WebMethod(true)]
		public DownloadResult DownloadImage(Guid orderUid, string orderNumber, Guid imageUid, FileSize filesize, OfferingType offeringType)
		{
			ImageDownloadFileSize imageToDownload = new ImageDownloadFileSize();
			imageToDownload.FileSize = filesize;
			imageToDownload.ImageUid = imageUid;
			imageToDownload.OfferingType = offeringType;

			List<ImageDownloadFileSize> imagesToDownload = new List<ImageDownloadFileSize>();
			imagesToDownload.Add(imageToDownload);

			return DownloadImages(orderUid, orderNumber, imagesToDownload);
		}

		[WebMethod(true)]
		public DownloadResult DownloadImages(Guid orderUid, string orderNumber, List<ImageDownloadFileSize> imagesToDownload)
		{
            int failedDownloadCount;
			List<PackagedImagesDetails> packagedImagesDetailList = presenter.PackageOrderImages(orderUid, orderNumber, imagesToDownload, out failedDownloadCount);
			List<KeyValuePair<string, string>> returnFileList = new List<KeyValuePair<string, string>>();

			foreach(PackagedImagesDetails packagedImagesDetail in packagedImagesDetailList)
			{
				string filenameText = String.Format(HttpContext.GetLocalResourceObject("~/Checkout/CheckoutService.asmx", "fileNameDisplay").ToString(),
					packagedImagesDetail.FileName,
					StringHelper.GetFilesizeForDisplay(packagedImagesDetail.SizeInBytes));
				string navigationUrl = String.Format("/Checkout/{0}?{1}={2}&{3}={4}&{5}={6}",
                    packagedImagesDetail.FileName,
                    ZipHandlerKeys.OrderUid,
					orderUid,
                    ZipHandlerKeys.ArchiveUid,
                    packagedImagesDetail.ArchiveUid,
                    ZipHandlerKeys.FileSize,
                    packagedImagesDetail.SizeInBytes);

				returnFileList.Add(new KeyValuePair<string, string>(filenameText, navigationUrl));
			}

			DownloadResult returnResult = new DownloadResult();
			returnResult.FailedCount = failedDownloadCount;
			returnResult.DownloadImages = returnFileList;
			return returnResult;
		}

		[WebMethod(true)]
		public List<RfcdDisplayImage> GetRfcdImagesByVolumeNumber(string volumeNumber)
		{
			return presenter.GetRfcdImagesByVolumeNumber(volumeNumber);
		}

        [WebMethod(true)]
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        /// <summary>
        /// Validates the project and licensee for Non-Ascii data.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectNameClientId">The clientId of the project control.</param>
        /// <param name="jobNumber">The job number.</param>
        /// <param name="jobNumberClientId">The client id of the job number control</param>
        /// <param name="poNumber">The po number.</param>
        /// <param name="poNumberClientId">The client id of the ponumber control.</param>
        /// <param name="licensee">The licensee.</param>
        /// <param name="licenseeClientId">The clientId of the licensee control.</param>
        /// <returns>
        /// A KeyValue pair where the key is the clientId is the control's clientId
        /// and the value is the error message
        /// </returns>
        [WebMethod(true)]
        public List<ScriptServiceValidationError> ValidateProjectEncoding(
            string projectName,
            string projectNameClientId,
            string jobNumber,
            string jobNumberClientId,
            string poNumber,
            string poNumberClientId,
            string licensee,
            string licenseeClientId)
        {
            Name = projectName;
            JobNumber = jobNumber;
            PONumber = poNumber;
            Licensee = licensee;
            CheckoutPresenter presenter = new CheckoutPresenter(this);
            List<string> invalidProperties = presenter.ValidateProjectEncoding();
            List<ScriptServiceValidationError> errors = new List<ScriptServiceValidationError>();
            if (invalidProperties != null && invalidProperties.Count > 0)
            {
                // Only show the NonAscii validation error once
                bool showInSummary = true;
                string errorMessage = 
                    HttpContext.GetGlobalResourceObject("Resource", "ContainsNonAsciiCharacters").ToString();
                errors = new List<ScriptServiceValidationError>();
                foreach (string invalidProperty in invalidProperties)
                {
                    ScriptServiceValidationError error = null;
                    switch (invalidProperty)
                    {
                        case "Name":
                            error = new ScriptServiceValidationError(
                                projectNameClientId,
                                errorMessage,
                                true,
                                showInSummary);
                            break;
                        case "JobNumber":
                            error = new ScriptServiceValidationError(
                                jobNumberClientId,
                                errorMessage,
                                true,
                                showInSummary);
                            break;
                        case "PONumber":
                            error = new ScriptServiceValidationError(
                                poNumberClientId,
                                errorMessage,
                                true,
                                showInSummary);
                            break;
                        case "Licensee":
                            error = new ScriptServiceValidationError(
                                licenseeClientId,
                                errorMessage,
                                true,
                                showInSummary);
                            break;
                    }
                    errors.Add(error);
                    showInSummary = false;
                    errorMessage = null;
                }
            }
            return errors;
        }



		#region ICheckoutScriptServiceView Members

        public ProductRestriction ViewRestrictions
        {
            get 
              {
                  return _viewRestrictions;
              }
              set 
              {
                _viewRestrictions = value;
              }
        }

		#endregion

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


        #region IProject Members

        public string Name { get; set; }

        public string JobNumber { get; set; }

        public string PONumber { get; set; }

        public string Licensee { get; set; }

        #endregion
    }

	public class DownloadResult
	{
		private int failedCount;
		public int FailedCount
		{
			get { return failedCount; }
			set { failedCount = value; }
		}

		private List<KeyValuePair<string, string>> downloadImages;
		public List<KeyValuePair<string, string>> DownloadImages
		{
			get { return downloadImages; }
			set { downloadImages = value; }
		}
	}
}
