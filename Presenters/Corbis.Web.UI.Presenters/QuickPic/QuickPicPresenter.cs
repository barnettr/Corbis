using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.MediaDownload.Contracts.V1;
using Corbis.MediaDownload.ServiceAgents.V1;
using Corbis.WebOrders.Contracts.V1;
using Corbis.WebOrders.ServiceAgents.V1;
using Corbis.Common.ServiceFactory.Validation;

namespace Corbis.Web.UI.Presenters.QuickPic
{
	public class QuickPicPresenter : BasePresenter
	{

		#region Private Variable

		private StateItemCollection _stateItems;
        private IQuickPicDownloadView _quickPicDownloadView;
        private IWebOrdersContract _webOrdersAgent;
        private IMediaDownloadContract _mediaDownloadAgent;

        #endregion


		#region Constructors

		public QuickPicPresenter()
		{
			_stateItems = new StateItemCollection(HttpContext);
            _webOrdersAgent = new WebOrdersServiceAgent();
            _mediaDownloadAgent = new MediaDownloadServiceAgent();
		}

        public QuickPicPresenter(IQuickPicDownloadView downloadView)
            : this(downloadView, new WebOrdersServiceAgent(), new MediaDownloadServiceAgent()) {}

        public QuickPicPresenter(
            IQuickPicDownloadView downloadView, 
            IWebOrdersContract webOrdersAgent,
            IMediaDownloadContract mediaDownloadAgent)
        {
            if (downloadView == null)
            {
                throw new ArgumentNullException("downloadView");
            }
            if (webOrdersAgent == null)
            {
                throw new ArgumentNullException("webOrdersAgent");
            }
            if (mediaDownloadAgent == null)
            {
                throw new ArgumentNullException("mediaDownloadAgent");
            }
            _stateItems = new StateItemCollection(HttpContext);
            _quickPicDownloadView = downloadView;
            _webOrdersAgent = webOrdersAgent;
            _mediaDownloadAgent = mediaDownloadAgent;
        }

		#endregion

		#region Public  Methods

		#region QuickPicList

		public bool AddItemToQuickPick(QuickPicItem quickPicItem)
		{
			if (quickPicItem == null)
			{
				throw new ArgumentNullException("quickPicItem is null");
			}
			else
			{
				List<QuickPicItem> quickpicList = QuickPicList;
				if (quickpicList == null)
				{
					quickpicList = new List<QuickPicItem>();
				}

				if (quickpicList.Count < 20)
				{

					if (!quickpicList.Exists(new Predicate<QuickPicItem>(delegate(QuickPicItem quickPic) { return quickPic.CorbisID == quickPicItem.CorbisID; })))
					{
						quickpicList.Insert(0, quickPicItem);
						QuickPicList = quickpicList;
					}
					return true;
				}

			}

			return false;
		}

		public void RemoveItemToQuickPick(string CorbisId)
		{
			if (string.IsNullOrEmpty(CorbisId))
			{
				throw new ArgumentNullException("QuickPicItem corbis ID is null or Empty");
			}
			else
			{
				List<QuickPicItem> quickpicList = QuickPicList;
				QuickPicItem quickPicListItem = quickpicList.Find(new Predicate<QuickPicItem>(delegate(QuickPicItem quickItem) { return quickItem.CorbisID == CorbisId; }));
				if (quickPicListItem != null)
				{
					quickpicList.Remove(quickPicListItem);
					QuickPicList = quickpicList;
				}
			}
		}

		public List<QuickPicItem> QuickPicList
		{
			get
			{
                List<QuickPicItem> quickPicList = _stateItems.GetStateItemValue<List<QuickPicItem>>(QuickPicKeys.Name, QuickPicKeys.QuickPickListKey, StateItemStore.AspSession);
				return quickPicList;
			}
			set
			{
                List<QuickPicItem> quickPicList = value;
				_stateItems.SetStateItem<List<QuickPicItem>>(new StateItem<List<QuickPicItem>>(QuickPicKeys.Name, QuickPicKeys.QuickPickListKey, quickPicList, StateItemStore.AspSession));
			}
		}

		#endregion

        #region QuickPicDownload

        public void GetQuickPicImagesToDownload()
        {
            // create a local Variable so we don't have to hit state all the time
            List<QuickPicItem> nonStateQuickPicList = QuickPicList;
            if (nonStateQuickPicList == null || nonStateQuickPicList.Count == 0)
            {
                return;
            }

            List<string> corbisids = new List<string>(nonStateQuickPicList.Count);
            foreach (QuickPicItem item in nonStateQuickPicList)
            {
                corbisids.Add(item.CorbisID);
            }

            List<DownloadableQuickPicImage> downloadableImages =
                _webOrdersAgent.GetQuickPickImagesForDownload(
                    Profile.UserName,
                    corbisids,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode);
            _quickPicDownloadView.DownloadableQuickPicImages = downloadableImages;
        }

        public QuickPicOrderSummary DownloadQuickPicImages(string projectName, List<QuickPicOrderImage> images)
        {
            QuickPicOrderSummary orderSummary = new QuickPicOrderSummary
                {
                    ProjectName = projectName,
                    ConfirmationEmail = string.Empty,
                    DownloadPackages = null,
                    PackagedCorbisIds = null,
                    FailedCount = 0,
                    OrderNumber = string.Empty
                };

            if (projectName == null || images == null || images.Count == 0)
            {
                orderSummary.FailedCount = images != null ? images.Count : 0;
                return orderSummary;
            }

            string localizedQuickPicText = HttpContext.GetLocalResourceObject("~/QuickPic/QuickPic.aspx", "pageTitle.Text").ToString();
            
            OrderConfirmationDetails orderDetails = _webOrdersAgent.CreateQuickPicOrder(
                Profile.UserName,
                localizedQuickPicText,
                projectName,
                images,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode);

            orderSummary.ConfirmationEmail = orderDetails.OrderSummary.Delivery.ConfirmationEmailAddresses;
            orderSummary.OrderNumber = orderDetails.OrderNumber;

            List<ImageDownloadFileSize> downloadSizes = new List<ImageDownloadFileSize>();
            foreach (QuickPicOrderImage quickPicImage in images)
            {
                downloadSizes.Add(new ImageDownloadFileSize
                {
                    ImageUid = quickPicImage.ImageUid,
                    FileSize = quickPicImage.FileSize,
                    OfferingType = OfferingType.Stills
                });
            }

            
            // Filename's can't contain NonAscii characters. If it does, just use english
            string uniqueFileNameFormat = HttpContext.GetLocalResourceObject(
                "~/Checkout/CheckoutService.asmx", 
                "fileName").ToString();
            if (NonAsciiValidator.HasNonWesternCharacters(uniqueFileNameFormat))
            {
                uniqueFileNameFormat = HttpContext.GetLocalResourceObject(
                    "~/Checkout/CheckoutService.asmx",
                    "fileName", Language.EnglishUS.CultureInfo).ToString();
            }
            Random random = new Random();
            string uniqueFileName = String.Format(
                uniqueFileNameFormat, 
                orderDetails.OrderNumber, 
                random.Next());

            PackageImagesResult packageResult = _mediaDownloadAgent.PackageImages(
                Profile.UserName,
                orderDetails.OrderUid,
                downloadSizes,
                uniqueFileName,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode);

            if (packageResult.FileDetails != null && packageResult.FileDetails.Count > 0)
            {
                // Save the ArchiveUids to Session
                List<Guid> archiveUids = new List<Guid>();
                foreach (PackagedImagesDetails details in packageResult.FileDetails)
                {
                    archiveUids.Add(details.ArchiveUid);
                }

                StateItem<List<Guid>> archiveUidsStateItem =
                    new StateItem<List<Guid>>(
                    OrderKeys.Name,
                    OrderKeys.ArchiveUids,
                    archiveUids,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (archiveUids != null)
                {
                    _stateItems.SetStateItem<List<Guid>>(archiveUidsStateItem);
                }
                else
                {
                    _stateItems.DeleteStateItem<List<Guid>>(archiveUidsStateItem);
                }

                orderSummary.DownloadPackages = new List<KeyValuePair<string, string>>();
                foreach (PackagedImagesDetails packagedImagesDetail in packageResult.FileDetails)
                {
                    string filenameText = String.Format(HttpContext.GetLocalResourceObject("~/Checkout/CheckoutService.asmx", "fileNameDisplay").ToString(),
                        packagedImagesDetail.FileName,
                        StringHelper.GetFilesizeForDisplay(packagedImagesDetail.SizeInBytes));
                    string navigationUrl = String.Format("/Checkout/{0}?{1}={2}&{3}={4}&{5}={6}",
                        packagedImagesDetail.FileName,
                        ZipHandlerKeys.OrderUid,
                        orderDetails.OrderUid,
                        ZipHandlerKeys.ArchiveUid,
                        packagedImagesDetail.ArchiveUid,
                        ZipHandlerKeys.FileSize,
                        packagedImagesDetail.SizeInBytes);

                    orderSummary.DownloadPackages.Add(new KeyValuePair<string, string>(filenameText, navigationUrl));
                }
            }

            #region remove from session
            // remove the images that succeeded from session
            // Get a list of All corbisIds requested
            orderSummary.PackagedCorbisIds = new List<string>();
            foreach (QuickPicOrderImage quickPicOrderImage in images)
            {
                orderSummary.PackagedCorbisIds.Add(quickPicOrderImage.CorbisId);
            }
            // Check if any failed and remove them from the successful list
            if (packageResult.FailedImageUids != null && packageResult.FailedImageUids.Count > 0)
            {
                orderSummary.FailedCount = packageResult.FailedImageUids.Count;
                foreach (Guid failedUid in packageResult.FailedImageUids)
                {
                    QuickPicOrderImage failedImage = images.Find(delegate(QuickPicOrderImage image)
                    {
                        return image.ImageUid == failedUid;
                    });
                    if (failedImage != null)
                    {
                        orderSummary.PackagedCorbisIds.Remove(failedImage.CorbisId);
                    }
                }
            }

            // Now remove all the successes from session
            if (orderSummary.PackagedCorbisIds.Count > 0)
            {
                // create a local Variable so we don't have to hit state all the time
                List<QuickPicItem> nonStateQuickPicList = QuickPicList;
                nonStateQuickPicList.RemoveAll(delegate(QuickPicItem qpi)
                {
                    return orderSummary.PackagedCorbisIds.Contains(qpi.CorbisID);
                });
                // Now set the QuickPicItems back in session
                QuickPicList = nonStateQuickPicList;
            }
            #endregion
            return orderSummary;
        }

        #endregion

        #endregion
    }
}
