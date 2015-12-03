using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
using Corbis.Framework.Globalization;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.Web.Entities;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Pricing.ViewInterfaces;
using Corbis.Web.Content;
using Corbis.RFCD.Contracts.V1;
using Corbis.RFCD.ServiceAgents.V1;

namespace Corbis.Web.UI.Presenters.Pricing
{
    public partial class PricingPresenter : BasePresenter
    {
        private IView view;
        private IRFPricing rfPricingView;
        private IRMPricing rmPricingView;
        private IRSPricing rsPricingView;
        private IPricingHeader pricingHeaderView;
        private IRMPricingExpressCheckout rmPricingExpressCheckoutView;
        private IRequestPriceView requestPriceView;
        private IPriceImageLink priceImageLinkView;
        private ILightboxCartContract lightboxCartService;
        private IImageContract imageService;
        private IRFCDContract rfcdService;
        private DisplayImage displayImage;
        internal CompletedUsage completedUsage;
        private Guid _fileSizeAttributeUid = new Guid("{649187DA-D26B-4952-87CE-91729B204D91}");
        private Guid _LicenseStartDateAttributeUid=new Guid ("{fd83d2f4-7b12-45f7-9651-e5a15521e55c}");
        private RegionsContentProvider regionsContentProvider;
        private CountriesContentProvider countriesContentProvider;

        public PricingPresenter(IView view) : this(view, new LightboxCartServiceAgent(), new ImageServiceAgent(), new RFCDServiceAgent())
        {

        }

        public PricingPresenter(IView view, ILightboxCartContract lightboxCartService)
            : this(view, lightboxCartService, new ImageServiceAgent(), new RFCDServiceAgent())
        {

        }

        public PricingPresenter(IView view, ILightboxCartContract lightboxCartService, IImageContract imageService, IRFCDContract rfcdService)
        {
            if (view == null)
            {
                throw new ArgumentNullException("PricingPresenter: PricingPresenter() - View Object cannot be null.");
            }
            if (lightboxCartService == null)
            {
                throw new ArgumentNullException("PricingPresenter: PricingPresenter() - Lightbox Cart Service agent cannot be null.");
            }
            if (imageService == null)
            {
                throw new ArgumentNullException("PricingPresenter: PricingPresenter() - Image service agent cannot be null.");
            }

            
            this.view = view;
            this.lightboxCartService = lightboxCartService;
            this.imageService = imageService;
            this.rfcdService = rfcdService;
            rfPricingView = this.view as IRFPricing;
            rmPricingView = this.view as IRMPricing;
            rsPricingView = this.view as IRSPricing;
            rmPricingExpressCheckoutView = this.view as IRMPricingExpressCheckout;
            pricingHeaderView = this.view as IPricingHeader;
            requestPriceView = this.view as IRequestPriceView;
            priceImageLinkView = this.view as IPriceImageLink;

            completedUsage = new CompletedUsage();
            completedUsage.AttributeValuePairs = new List<CompletedUsageAttributeValuePair>();
        }

        public void GetRFPriceList(string contactUsText, string perContractText)
        {
            List<string> corbisIdList = new List<string>();
            corbisIdList.Add(rfPricingView.CorbisId);
            List<RfPriceList> rfPriceListList = lightboxCartService.GetRfPriceList(
                Profile.UserName, 
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode, 
                corbisIdList);
            RfPriceList rfPriceList = rfPriceListList.Find(delegate(RfPriceList list)
            {
                return list.CorbisId == rfPricingView.CorbisId;
            });

            string currencyCode = string.Empty;

            foreach (PricedUseTypeAttributeValue avItem in rfPriceList.PricedAttributeValues)
            {
                string tmpCurrencyCode = avItem.CurrencyCode;
                avItem.CurrencyCode = String.Empty;

                if (!Profile.IsECommerceEnabled)
                {
                    avItem.EffectivePrice = contactUsText;
                }
                else if (Profile.ContractType == ContractType.Purchasing)
                {
                    avItem.EffectivePrice = perContractText;
                }
                else
                {
                    switch (avItem.EffectivePriceStatus)
                    {
                        case PriceStatus.Unknown:
                        case PriceStatus.ContactUs:
                        case PriceStatus.ContactUs | PriceStatus.DisplayPrice:
                        case PriceStatus.ContactOutline:
                        case PriceStatus.PricedByAE:
                            avItem.EffectivePrice = contactUsText;
                            break;
                        case PriceStatus.AsPerContract:
                            avItem.EffectivePrice = perContractText;
                            break;
                        case PriceStatus.Ok:
                           // avItem.EffectivePrice = String.Format("{0}.00", avItem.EffectivePrice);
                            avItem.EffectivePrice = CurrencyHelper.GetLocalizedCurrency(avItem.EffectivePrice);
                            avItem.CurrencyCode = string.Format("({0})", tmpCurrencyCode);
                            currencyCode = avItem.CurrencyCode;
                            break;
                    }
                }
            }

            rfPricingView.PriceList = rfPriceList;
            rfPricingView.UseCategoryUid = rfPriceList.UseCategoryUid;
            rfPricingView.UseTypeUid = rfPriceList.UseTypeUid;
            if (rfPricingView.PricingHeader != null)
            {
                rfPricingView.PricingHeader.CurrencyCode = currencyCode;
            }
        }

        public void InitializeRFPricingForm(string contactUsText, string perContractText)
        {
            DisplayImage displayImage = GetDisplayImage();

            if (rfPricingView.RestrictionsControl != null)
            {
                ImageRestrictionsPresenter restrictionsPresenter =
                    new ImageRestrictionsPresenter(rfPricingView.RestrictionsControl, displayImage);
                restrictionsPresenter.SetRestrictions();
            }

            if (!displayImage.IsAvailable)
            {
                rfPricingView.ShowPriceStatusMessageForRF = true;
            }
            

            rfPricingView.CustomPriced = false;
            rfPricingView.EffectivePriceStatus = PriceStatus.Unknown;

            if (rfPricingView.ProductUid != Guid.Empty)
            {
                // Get the saved RF product detail with Usage.
                FolderProduct product = GetRFProductWithUsage();

                if (rfPricingView.PricedByAE && ((product.EffectivePriceStatus & PriceStatus.CustomPriceExpired) == 0))
                {
                    rfPricingView.CustomPriced = true;
                    rfPricingView.EffectivePriceText =  CurrencyHelper.GetLocalizedCurrency(product.EffectivePrice);
                }
            }

            GetRFPriceList(contactUsText, perContractText);
        }

        private FolderProduct GetRFProductWithUsage()
        {
            FolderProduct product = lightboxCartService.GetProductByProductUid(
                Profile.UserName,
                rfPricingView.ProductUid,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                true,
                false);

            if (product.Usage != null)
            {
                completedUsage = product.Usage;

                CompletedUsageAttributeValuePair attribValuePair = completedUsage.AttributeValuePairs.Find(
                    delegate(CompletedUsageAttributeValuePair attValPair)
                    {
                        return attValPair.UseAttributeUid == _fileSizeAttributeUid;
                    });
                // For backwards compatibility when the FileSize attribute was used 
                // instead of the RF Broad Rights Attribute
                if (attribValuePair == null)
                {
                    attribValuePair = new CompletedUsageAttributeValuePair();
                    attribValuePair.UseAttributeUid = _fileSizeAttributeUid;
                    attribValuePair.Value = Guid.Empty;
                }

                // Check if the license details are invalid.
                if (product.RfLicenseDetail == null)
                {
                    // Set an empty guid to tell view it is an invalid license.
                    rfPricingView.AttributeValueUid = Guid.Empty;
                    rfPricingView.ValidLicense = false;
                }
                else
                {
                    rfPricingView.ValidLicense = true;
                    rfPricingView.AttributeValueUid = (Guid)attribValuePair.Value;
                    AssignLicenseDetails(product);
                }

                // Load the UseTypes and use categories for this product
                rfPricingView.AttributeValueUid = (Guid)attribValuePair.Value;
                rfPricingView.EffectivePriceStatus = product.EffectivePriceStatus;
                rfPricingView.UseCategoryUid = completedUsage.UseCategoryUid;
                rfPricingView.UseTypeUid = completedUsage.UseTypeUid;
                rfPricingView.PricedByAE = ((product.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE);
            }

            return product;
        }

        private void AssignLicenseDetails(FolderProduct product)
        {
            rfPricingView.FileSizeText = string.Format(" ({0}*)", product.RfLicenseDetail.UncompressedFileSize);
            rfPricingView.ImageFileSize = product.RfLicenseDetail.ImageSize;

            string unit = (Profile.CountryCode.Equals("US", StringComparison.InvariantCultureIgnoreCase)) ? "in" : "cm";

            rfPricingView.DimensionText = string.Format("{0}px x {1}px {2} {3}{4} x {5}{4} @ {6}ppi",
                product.RfLicenseDetail.PixelHeight,
                product.RfLicenseDetail.PixelWidth,
                ((char)183).ToString(),
                product.RfLicenseDetail.ImageHeight,
                unit,
                product.RfLicenseDetail.ImageWidth,
                product.RfLicenseDetail.Resolution);
        }

        /// <summary>
        /// Gets the RS price list.
        /// </summary>
        /// <param name="contactUsText">The contact us text.</param>
        /// <param name="username">The username.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="isCommerceEnabled">if set to <c>true</c> [is commerce enabled].</param>
        public void GetRSPriceList(string contactUsText, string username, string countryCode, bool isCommerceEnabled)
        {
            // Set corbisid from view.
            string rsCorbisId = rsPricingView.CorbisId;
            List<string> corbisIdList = new List<string>();
            corbisIdList.Add(rsCorbisId);

            // Call service to get the RS pricelists.
            List<RsPriceList> rsPriceListList = lightboxCartService.GetRsPriceList(username, countryCode, Language.CurrentLanguage.LanguageCode, corbisIdList);

            RsPriceList rsPriceList = rsPriceListList.Find(delegate(RsPriceList list)
            {
                return list.CorbisId == rsCorbisId; 
            });

            if (rsPriceList == null)
            {
                return;
            }

            Dictionary<string, List<RsUseTypeAttributeValue>> rsUseTypeAttributeCollection = new Dictionary<string, List<RsUseTypeAttributeValue>>();

            string currencyCode = string.Empty;

            foreach (RsUseType rsUseType in rsPriceList.RsUseTypes)
            {
                // RS usetype attribute object to populate data.
                List<Corbis.Web.Entities.RsUseTypeAttributeValue> rsUseTypeAttributes = new List<Corbis.Web.Entities.RsUseTypeAttributeValue>();

                currencyCode = SetRSPricingAttributeValues(contactUsText, isCommerceEnabled, rsPriceList, currencyCode, rsUseType, rsUseTypeAttributes);

                // Populate pricelist attributes.
                if (rsUseTypeAttributes.Count > 0)
                {
                    rsUseTypeAttributeCollection.Add(rsUseType.DisplayText, rsUseTypeAttributes);
                }
            }

            rsPricingView.UseCategoryUid = rsPriceList.UseCategoryUid;
            rsPricingView.RSUseTypeAttributes = rsUseTypeAttributeCollection;
            rsPricingView.PricingHeader.CurrencyCode = currencyCode;
        }

        private static string SetRSPricingAttributeValues(string contactUsText, bool isCommerceEnabled, RsPriceList rsPriceList, string currencyCode, RsUseType rsUseType, List<Corbis.Web.Entities.RsUseTypeAttributeValue> rsUseTypeAttributes)
        {
            foreach (PricedUseTypeAttributeValue avItem in rsUseType.PricedAttributeValues)
            {
                RsUseTypeAttributeValue rsUseTypeAttrib = new RsUseTypeAttributeValue();
                switch (avItem.EffectivePriceStatus)
                {
                    case PriceStatus.Unknown:
                    case PriceStatus.ContactUs:
                    case PriceStatus.ContactUs | PriceStatus.DisplayPrice:
                    case PriceStatus.ContactOutline:
                    case PriceStatus.AsPerContract:
                    case PriceStatus.PricedByAE:
                        rsUseTypeAttrib.EffectivePrice = contactUsText;
                        break;
                    case PriceStatus.Ok:

                        if (!isCommerceEnabled)
                        {
                            rsUseTypeAttrib.EffectivePrice = string.Empty;
                        }
                        else
                        {
                            rsUseTypeAttrib.EffectivePrice = CurrencyHelper.GetLocalizedCurrency(avItem.EffectivePrice);
                            currencyCode = avItem.CurrencyCode = "(" + avItem.CurrencyCode + ")";
                        }

                        break;
                }

                // Set all pricelist attributes
                rsUseTypeAttrib.DisplayText = avItem.DisplayText;
                rsUseTypeAttrib.Description = avItem.Description;
                rsUseTypeAttrib.CorbisId = rsPriceList.CorbisId;
                rsUseTypeAttrib.AttributeUid = avItem.AttributeUid;
                rsUseTypeAttrib.CurrencyCode = avItem.CurrencyCode;
                rsUseTypeAttrib.ValueUid = avItem.ValueUid;
                rsUseTypeAttrib.UseTypeUid = rsUseType.UseTypeUid;
                rsUseTypeAttrib.RequiresAeNegotiation = avItem.RequiresAeNegotiation;

                rsUseTypeAttributes.Add(rsUseTypeAttrib);
            }
            return currencyCode;
        }
        
        public void GetHeaderDetails()
        {
            GetDisplayImage();
            pricingHeaderView.OfferingUid = displayImage.MediaUid;
            pricingHeaderView.ImageTitle = displayImage.Category;
            pricingHeaderView.ImageId = displayImage.CorbisId;
            pricingHeaderView.PricingTier = displayImage.PriceTierDisplayText;
            pricingHeaderView.LicenseModel = displayImage.LicenseModel.ToString();
            pricingHeaderView.ImageThumbnail = displayImage.Url128;
            pricingHeaderView.IsThumbPortrait = displayImage.AspectRatio < 1;
           
        }



        public void AddToCompletedAVPairList(Guid useAttributeUid, object value, bool clearListBeforeAdd)
        {
            if (clearListBeforeAdd)
            {
                completedUsage.AttributeValuePairs.Clear();
            }
                
            CompletedUsageAttributeValuePair avPair = new CompletedUsageAttributeValuePair();
            if (rfPricingView != null && Guid.Empty.Equals(useAttributeUid))
            {
                avPair.UseAttributeUid = _fileSizeAttributeUid;
            }
            else
            {
                avPair.UseAttributeUid = useAttributeUid;
            }

            avPair.Value = value;
            completedUsage.AttributeValuePairs.Add(avPair);
        }


        /// <summary>
        /// Updates the license start date on a custom priced product.
        /// </summary>
        /// <param name="productUid">The product uid.</param>
        /// <param name="licenseDtartDate">The license dtart date.</param>
        /// <returns></returns>
        public static bool UpdateCustomProductLicenseStartDate(Guid productUid, DateTime licenseStartDate)
        {
            try
            {
                // Need a new ServiceAgent as it's a static method
                LightboxCartServiceAgent staticAgent = new LightboxCartServiceAgent();
                staticAgent.UpdateCustomProductLicenseStartDate(productUid, licenseStartDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddImageToCart()
        {
            if (rfPricingView != null)
            {
                int cartCount = lightboxCartService.AddImageToCart(
                    Profile.UserName,
                    rfPricingView.CorbisId,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode);
                Profile.CartItemsCount = cartCount;
            }
        }

        /// <summary>
        /// Updates the cart products.
        /// </summary>
        /// <param name="rsSelectedUseTypeUid">
        /// The rs selected use type uid. Pass empty guid for non RS Images.
        /// </param>
        public void UpdateCartProducts(Guid rsSelectedUseTypeUid)
        {
            DateTime licenseStartDate = DateTime.MinValue;
            List<string> corbisIdList = SetUsageFromView(rsSelectedUseTypeUid);

            if (rmPricingView != null && rmPricingView.CustomPriced)
            {
                // Custom Pricing
                if (rmPricingView.LicenseStartDate.HasValue)
                {
                    licenseStartDate = rmPricingView.LicenseStartDate.Value;
                }

                lightboxCartService.UpdateCartProducts(Profile.UserName, corbisIdList, (Guid)rmPricingView.CustomPricedProductUid,
                    licenseStartDate, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            else if (rfPricingView != null && rfPricingView.CustomPriced)
            {
                // Update RF cutom priced item.
                lightboxCartService.UpdateCartProducts(Profile.UserName, corbisIdList, rfPricingView.ProductUid,
                    licenseStartDate, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            else if (completedUsage.UseCategoryUid == Guid.Empty || completedUsage.UseTypeUid == Guid.Empty ||
                completedUsage.AttributeValuePairs == null || completedUsage.AttributeValuePairs.Count == 0)
            {
                lightboxCartService.AddImageToCart(Profile.UserName, corbisIdList[0], Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            else
            {
                lightboxCartService.UpdateCartProducts(Profile.UserName, corbisIdList, completedUsage, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            
            Profile.CartItemsCount = lightboxCartService.GetCartCount(Profile.MemberUid);
       }

        /// <summary>
        /// Updates the products in a Lightbox.
        /// </summary>
        /// <param name="rsSelectedUseTypeUid">
        /// The rs selected use type uid. Pass empty guid for non RS Images.
        /// </param>
        public void UpdateLightboxProducts(int lightboxId, Guid rsSelectedUseTypeUid)
        {
            DateTime licenseStartDate = DateTime.MinValue;
            List<string> corbisIdList = SetUsageFromView(rsSelectedUseTypeUid);

            if (rmPricingView != null && rmPricingView.CustomPriced)
            {
                if (rmPricingView.LicenseStartDate.HasValue)
                {
                    licenseStartDate = rmPricingView.LicenseStartDate.Value;
                }
                // Custom Pricing
                lightboxCartService.UpdateLightboxProducts(Profile.UserName, lightboxId, corbisIdList, (Guid)rmPricingView.CustomPricedProductUid,
                    licenseStartDate, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            else if (rfPricingView != null && rfPricingView.CustomPriced)
            {
                // Update RF cutom priced item.
                lightboxCartService.UpdateLightboxProducts(Profile.UserName, lightboxId, corbisIdList, rfPricingView.ProductUid,
                    licenseStartDate, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
            else if (completedUsage.UseCategoryUid == Guid.Empty || completedUsage.UseTypeUid == Guid.Empty || 
                completedUsage.AttributeValuePairs == null || completedUsage.AttributeValuePairs.Count == 0)
            {
                lightboxCartService.AddImageToLightBox(Profile.UserName, corbisIdList[0], Profile.CountryCode, Language.CurrentLanguage.LanguageCode, lightboxId);
            }
            else
            {
                lightboxCartService.UpdateLightboxProducts(Profile.UserName, lightboxId, corbisIdList, completedUsage, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            }
        }
        public void UpdateCOFFLightboxProductsCompletedUsage(int lightboxId , List<COFFOrderImage> coffOrderImages)
        {
            List<string> temp = new List<string>();
            foreach (COFFOrderImage image in coffOrderImages)
            {
                CompletedUsage completedUsage = GetRFUsagefromFileSize(image.FileSize, image.CorbisId);
                temp.Add(image.CorbisId);
                try
                {
                    lightboxCartService.UpdateLightboxProducts(Profile.UserName, lightboxId, temp, completedUsage, Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
                }
                catch (Exception ex)
                {
                    HandleException(ex, rfPricingView.LoggingContext,
                                    string.Format(
                                        "PricingPresenter: UpdateCOFFLightboxProductsCompletedUsage() - Error updating lightbox product for member '{0}'.",
                                        Profile.UserName));
                    
                }
                finally
                {
                    temp.Remove(image.CorbisId);
                }
            }
        }
        public void SetLightboxAndCartButtonState()
        {
            List<string> corbisIdList = new List<string>();
            corbisIdList.Add(pricingHeaderView.CorbisId);
            List<ImageInLightboxCart> imageInLightboxCartList;

            imageInLightboxCartList = lightboxCartService.GetLightBoxCartStatusByCorbisIds(Profile.UserName, corbisIdList, null);

            if (imageInLightboxCartList[0].IsInCart)
            {
                pricingHeaderView.CartButtonEnabled = false;
                pricingHeaderView.UpdatingCart = true;
            }
            else
            {
                pricingHeaderView.UpdatingCart = false;
                // Never enable it from ExpressCheckout
                if (pricingHeaderView.ParentPage == ParentPage.ExpressCheckout)
                {
                    pricingHeaderView.CartButtonEnabled = false;
                }
                else
                {
                    pricingHeaderView.CartButtonEnabled = true;
                }
            }


            if (pricingHeaderView.ParentPage == ParentPage.Lightbox)
            {
                pricingHeaderView.LightboxButtonEnabled = false;
                pricingHeaderView.UpdatingLightbox = true;
            }
            else
            {
                pricingHeaderView.UpdatingLightbox = false;
                // Never enable it from ExpressCheckout
                if (pricingHeaderView.ParentPage == ParentPage.ExpressCheckout)
                {
                    pricingHeaderView.LightboxButtonEnabled = false;
                }
                else
                {
                    pricingHeaderView.LightboxButtonEnabled = true;
                }
            }
        }

        public DisplayImage GetDisplayImage()
        {
            if (displayImage == null)
            {
                string corbisID = string.Empty;
                if (rmPricingView != null)
                {
                    corbisID = rmPricingView.CorbisId;
                }
                else if (rfPricingView != null)
                {
                    corbisID = rfPricingView.CorbisId;
                }
                else if (rsPricingView != null)
                {
                    corbisID = rsPricingView.CorbisId;
                }
                else if (pricingHeaderView != null)
                {
                    corbisID = pricingHeaderView.CorbisId;
                }
                displayImage = imageService.GetDisplayImage(corbisID, Language.CurrentLanguage.LanguageCode, Profile.IsAnonymous);
            }
            return displayImage;
        }


        
        public static void InitializePriceImageLink(IPriceImageLink priceImageLink, CartDisplayImage image)
        {
            if (priceImageLink == null)
            {
                throw new ArgumentNullException("priceImageLink", "priceImageLink can't be null");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image", "image can't be null");
            }
            bool hasUsage = image.EffectivePriceStatus != PriceStatus.Unknown;
            InitializePriceImageLink(
                priceImageLink,
                image.CorbisId,
                image.ProductUid,
                image.IsRfcd,
                hasUsage,
                image.LicenseModel);
            
        }

        public static void InitializePriceImageLink(
            IPriceImageLink priceImageLink, 
            DisplayImage displayImage,
            Guid productUid,
            bool isPriced)
        {
            if (priceImageLink == null)
            {
                throw new ArgumentNullException("priceImageLink", "priceImageLink can't be null");
            }
            if (displayImage == null)
            {
                throw new ArgumentNullException("displayImage", "displayImage can't be null");
            }
            LicenseModel licenseModel = LicenseModel.Unknown;

            if (Enum.IsDefined(typeof(LicenseModel), displayImage.LicenseModel))
            {
                licenseModel = (LicenseModel)Enum.Parse(typeof(LicenseModel), displayImage.LicenseModel.ToString());
            }

            InitializePriceImageLink(
                priceImageLink,
                displayImage.CorbisId,
                productUid,
                false,
                isPriced,
                licenseModel);
        }

        public static void InitializePriceImageLink(IPriceImageLink priceImageLink, SearchResultProduct image)
        {
            if (priceImageLink == null)
            {
                throw new ArgumentNullException("priceImageLink", "priceImageLink can't be null");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image", "image can't be null");
            }
            LicenseModel licenseModel = LicenseModel.Unknown;

            if (Enum.IsDefined(typeof(LicenseModel), image.LicenseModel))
            {
                licenseModel = (LicenseModel)Enum.Parse(typeof(LicenseModel), image.LicenseModel.ToString());
            }
            // Opening Request price form
            if (!Profile.Current.IsECommerceEnabled)
            {
                priceImageLink.ShowPricingLink = true;
                priceImageLink.PricingAltText = "priceImage.ReqPriceText";
                priceImageLink.PricingNavigateUrl = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                          image.CorbisId + "', null, 1, '" + priceImageLink.ParentPage.ToString() + "'); return false;";
            }
            else if (image.IsOutline)
            {
                priceImageLink.ShowPricingLink = true;
                priceImageLink.PricingAltText = "priceImage.ContactOutlineText";
                priceImageLink.PricingNavigateUrl = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                          image.CorbisId + "', null, 1, '" + priceImageLink.ParentPage.ToString() + "'); return false;";
            }
            else
            {
                // Since Search Results will never know the ProductUid it is always Empty.
                InitializePriceImageLink(
                    priceImageLink,
                    image.CorbisId,
                    Guid.Empty,
                    false,
                    false,
                    licenseModel);
            }
        }

        public static string GetPostAddToCartScript(ParentPage parent, string corbisId, int cartItemCount)
		{
			string scriptString = "";
            switch (parent)
            {
                case ParentPage.ImageGroups:
                case ParentPage.Search:
                    scriptString = String.Format("parent.PricingModalPopupExit();parent.CorbisUI.Search.Handler.refreshCartItem('{0}', '{1}');", corbisId, cartItemCount.ToString());
                    break;
                case ParentPage.Lightbox:
                    scriptString = String.Format("parent.PricingModalPopupExit();parent.CorbisUI.Lightbox.Handler.refreshCartItem('{0}', '{1}');", corbisId, cartItemCount.ToString());
                    break;
                case ParentPage.Enlargement:
                    scriptString = String.Format("parent.PricingModalPopupExit();parent.refreshCartItem('{0}');", cartItemCount.ToString());
                    break;
                case ParentPage.Cart:
                    scriptString = String.Format("parent.PricingModalPopupExit();parent.CorbisUI.Cart.Handler.moveItemToPriced('{0}');", corbisId);
                    break;
                default:
                    // N/A
                    break;
            }

			return scriptString;
		}

        public static string GetPostAddToLightboxScript(ParentPage parent, int lightboxId, string corbisId, string lightboxName)
		{
			string scriptString = "";

            switch (parent)
            {
                case ParentPage.ImageGroups:
                case ParentPage.Search:
                    scriptString = string.Format("parent.PricingModalPopupExit();parent.CorbisUI.Handlers.Lightbox.refreshItemAdded('{0}', '{1}', '{2}');", corbisId, lightboxId, lightboxName);
                    break;
                case ParentPage.Lightbox:
                    scriptString = String.Format("parent.PricingModalPopupExit();parent.GetLB('{0}');", lightboxId);
                    break;
                case ParentPage.Enlargement:
					scriptString = String.Format("parent.PricingModalPopupExit();parent.refreshLightbox('{0}', '{1}', '{2}');parent.refreshEnlargementPage('');", lightboxId.ToString(), corbisId, lightboxName);
                    break;
                case ParentPage.Cart:
                    scriptString = String.Format("parent.PricingModalPopupExit();");
                    break;
                default:
                    // N/A
                    break;
            }

			return scriptString;
		}
		
       
        #region private methods

        /// <summary>
        /// Sets the usage from a view.
        /// </summary>
        /// <param name="rsSelectedUseTypeUid">
        /// The rs selected use type uid. Pass empty guid for non RS Images.
        /// </param>
        /// <returns>
        /// The list of CorbisIds to Add/Update
        /// </returns>
        private List<string> SetUsageFromView(Guid rsSelectedUseTypeUid)
        {
            List<string> corbisIdList = new List<string>();
            if (rfPricingView != null)
            {
                corbisIdList.Add(rfPricingView.CorbisId);

                if (rfPricingView.UseCategoryUid == Guid.Empty || rfPricingView.UseTypeUid == Guid.Empty)
                {
                    // Rajul - This is a very rare senario when user is unauthenticated and tries to add an image from pricing calculator.
                    // Get the Uids from pricelist object.
                    List<RfPriceList> rfPriceLists = lightboxCartService.GetRfPriceList(
                                    Profile.UserName,
                                    Profile.CountryCode,
                                    Language.CurrentLanguage.LanguageCode,
                                    corbisIdList);

                    if (rfPriceLists != null && rfPriceLists[0] != null)
                    {
                        rfPricingView.UseCategoryUid = rfPriceLists[0].UseCategoryUid;
                        rfPricingView.UseTypeUid = rfPriceLists[0].UseTypeUid;
                    }
                }

                completedUsage.UseCategoryUid = rfPricingView.UseCategoryUid;
                completedUsage.UseTypeUid = rfPricingView.UseTypeUid;
            }
            else if (rsPricingView != null)
            {
                completedUsage.UseCategoryUid = rsPricingView.UseCategoryUid;
                completedUsage.UseTypeUid = rsSelectedUseTypeUid;
                corbisIdList.Add(rsPricingView.CorbisId);
            }
            else if (rmPricingView != null)
            {
                corbisIdList.Add(rmPricingView.CorbisId);
            }
            return corbisIdList;
        }

        public CompletedUsage GetRFPricingUsage(Guid useAttributeUid, object value, bool clearListBeforeAdd)
        {
            AddToCompletedAVPairList(useAttributeUid, value, clearListBeforeAdd);
            SetUsageFromView(Guid.Empty);

            return completedUsage;
        }

        public CompletedUsage GetRMPricingUsage(Guid productUid,string startDate)
        {
            CompletedUsage usage = null;
            bool hasStartDate = false; ;
            FolderProduct product = lightboxCartService.GetProductByProductUid(
            Profile.UserName, rfPricingView.ProductUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, true, false);

            List<CompletedUsageAttributeValuePair> arrtibute = new List<CompletedUsageAttributeValuePair>();
            if (product.Usage != null && product.Usage.UseCategoryUid!=Guid.Empty)
            {
                foreach (CompletedUsageAttributeValuePair attributePair in product.Usage.AttributeValuePairs)
                {
                    if (attributePair.UseAttributeUid == _LicenseStartDateAttributeUid)
                    {
                        attributePair.Value = startDate;
                        hasStartDate = true;
                        break;
                    }
                }
                if (!hasStartDate)
                {
                    if (usage != null)
                    {
                        CompletedUsageAttributeValuePair startDateAttribute = new CompletedUsageAttributeValuePair();
                        startDateAttribute.UseAttributeUid = _LicenseStartDateAttributeUid;
                        startDateAttribute.Value = startDate;
                        usage.AttributeValuePairs.Add(startDateAttribute);
                    }
                }
            }

            return product.Usage;
        }


        public const string priceImageUrlFormat = "javascript:PriceImage('..{0}?CorbisId={1}&ProductUid={2}&ParentPage={3}',{4},{5});return false;";
        private static void InitializePriceImageLink(
            IPriceImageLink priceImageLink,
            string corbisId,
            Guid productUid,
            bool isRfcd,
            bool hasUsage,
            LicenseModel licenseModel)
        {
            if (!isRfcd)
            {
                string rawUrl = String.Empty;
                int pricingPageWidth = 0;
                int pricingPageHeight = 0;

                priceImageLink.PricingNavigateUrl = string.Empty;

                switch (licenseModel)
                {
                    case LicenseModel.RF:
                        priceImageLink.PricingAltText = hasUsage ? "priceImage.UpdateRFText" : "priceImage.PriceNewText";
                        rawUrl = priceImageLink.RFRawUrl;
                        pricingPageWidth = priceImageLink.RFPricingPageWidth;
                        pricingPageHeight = priceImageLink.RFPricingPageHeight;
                        break;
                    case LicenseModel.RM:
                        priceImageLink.PricingAltText = hasUsage ? "priceImage.UpdateRMText" : "priceImage.PriceNewText";
                        rawUrl = priceImageLink.RMRawUrl;
                        pricingPageWidth = priceImageLink.RMPricingPageWidth;
                        pricingPageHeight = priceImageLink.RMPricingPageHeight;
                        break;
                    case LicenseModel.RS:
                        priceImageLink.PricingAltText = hasUsage ? "priceImage.UpdateRSText" : "priceImage.PriceNewText";
                        rawUrl = priceImageLink.RSRawUrl;
                        pricingPageWidth = priceImageLink.RSPricingPageWidth;
                        pricingPageHeight = priceImageLink.RSPricingPageHeight;
                        break;
                    default:
                        priceImageLink.PricingAltText = "priceImage.PriceNewText";  
                        priceImageLink.PricingNavigateUrl = priceImageLink.CustomerServiceUrl;
                        break;
                }
                if (priceImageLink.PricingNavigateUrl != priceImageLink.CustomerServiceUrl)
                {
                    priceImageLink.PricingNavigateUrl = String.Format(
                        priceImageUrlFormat,
                        rawUrl,
                        corbisId,
                        hasUsage ? productUid : Guid.Empty,
                        priceImageLink.ParentPage,
                        pricingPageWidth,
                        pricingPageHeight);
                }
                priceImageLink.ShowPricingLink = true;
            }
            else
            {
                priceImageLink.ShowPricingLink = false;
                priceImageLink.PricingAltText = String.Empty;
                priceImageLink.PricingNavigateUrl = String.Empty;
            }

        }

        public CompletedUsage GetRFUsagefromFileSize(Corbis.CommonSchema.Contracts.V1.Image.FileSize imageSize, string corbisId)
        {
            CompletedUsage usage = null;
            List<string> corbisIds = new List<string>();
            corbisIds.Add(corbisId);

            List<RfPriceList> rfPriceLists = lightboxCartService.GetRfPriceList(
                Profile.UserName,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                corbisIds);

            if (rfPriceLists.Count > 0 && rfPriceLists[0].PricedAttributeValues != null)
            {
                PricedUseTypeAttributeValue useTypeAttribute = rfPriceLists[0].PricedAttributeValues.Find(
                    new Predicate<PricedUseTypeAttributeValue>(
                        delegate(PricedUseTypeAttributeValue attrib)
                        {
                            return attrib.MaximumFileSizeCode == imageSize.GetHashCode();
                        }));

                if (useTypeAttribute != null)
                {
                    // Craete a new RF Usage.
                    usage = new CompletedUsage();
                    usage.UseCategoryUid = rfPriceLists[0].UseCategoryUid;
                    usage.UseTypeUid = rfPriceLists[0].UseTypeUid;

                    usage.AttributeValuePairs = new List<CompletedUsageAttributeValuePair>();
                    CompletedUsageAttributeValuePair attributeValuePair = new CompletedUsageAttributeValuePair();
                    attributeValuePair.UseAttributeUid = useTypeAttribute.AttributeUid;
                    attributeValuePair.Value = useTypeAttribute.ValueUid;

                    usage.AttributeValuePairs.Add(attributeValuePair);
                }
            }

            return usage;
        }

        #endregion

        public bool IsLicenseStartDateRequired(Guid productUid)
        {
            if (productUid != null && productUid != Guid.Empty)
            {
                FolderProduct product = lightboxCartService.GetProductByProductUid(Profile.UserName, rfPricingView.ProductUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, true, false);

                List<CompletedUsageAttributeValuePair> arrtibute = new List<CompletedUsageAttributeValuePair>();
                if (product.Usage != null && product.Usage.UseCategoryUid != Guid.Empty)
                {
                    foreach (CompletedUsageAttributeValuePair attributePair in product.Usage.AttributeValuePairs)
                    {
                        if (attributePair.UseAttributeUid == _LicenseStartDateAttributeUid)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }  
}
