using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Checkout;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.ViewInterfaces;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Framework.Globalization;
namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout_PricingBox : CorbisBasePage, ICheckoutView, IProject, IDelivery, ISubmit, IPayment, IRFPricing
    {
        private CheckoutPresenter checkoutPresenter;
        private PricingPresenter pricingPresenter;
        private StateItemCollection stateItems;
        
        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressCheckout, "ExpressCheckoutCSS");
            stateItems = new StateItemCollection(HttpContext.Current);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                checkoutPresenter = new CheckoutPresenter(this);
                pricingPresenter = new PricingPresenter(this);
                corbisId = Request["CorbisId"];
                productUid = Guid.Empty;
                if (!string.IsNullOrEmpty(Request["ProductUid"]))
                {
                    GuidHelper.TryParse(Request["ProductUid"], out productUid);
                    ProductUids.Add(productUid);
                }
                bool isValidPayment = false;
                attributeValueUid = Guid.Empty;
                if (!string.IsNullOrEmpty(Request["hidAttributeValueUID"]))
                {
                    GuidHelper.TryParse(Request["hidAttributeValueUID"], out attributeValueUid);
                }
                string licenseStartDate = string.Empty;
                if (!string.IsNullOrEmpty(Request["StartDate"]))
                {
                    licenseStartDate = Request["StartDate"];
                }

                #region project

                if (!string.IsNullOrEmpty(Request["projectName"]))
                {
                    Name = Request["projectName"];
                }
                if (!string.IsNullOrEmpty(Request["jobNumber"]))
                {
                    JobNumber = Request["jobNumber"];
                }
                if (!string.IsNullOrEmpty(Request["poNumber"]))
                {
                    PONumber = Request["poNumber"];
                }
                if (!string.IsNullOrEmpty(Request["licensee"]))
                {
                    Licensee = Request["licensee"];
                }
                else
                {
                    Licensee = Resources.Resource.Required;
                }
                #endregion

                #region Payment
                string paymentValue = Request["thePaymentMethod"];
                string paymentType = Request["thePaymentType"];
                string contractType = Request["ContractType"];
                PromoCode = Request["PromoCode"];
                //by default make it false
                if (!string.IsNullOrEmpty(contractType) &&
                    Enum.IsDefined(typeof(ContractType), contractType))
                {
                    ContractType = (ContractType)Enum.Parse(typeof(ContractType), contractType);
                }
                if (!string.IsNullOrEmpty(paymentType) &&
                    Enum.IsDefined(typeof(PaymentMethod), paymentType))
                {
                    Method = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), paymentType);
                }
                if (Method == PaymentMethod.CreditCard)
                {
                    CreditCardUid = paymentValue;
                    isValidPayment = true;
                }
                else if (Method == PaymentMethod.CorporateAccount)
                {
                    int.TryParse(paymentValue, out selectedCorporateAccountID);
                    isValidPayment = true;
                }
                #endregion

                string priceCode = Profile.CurrencyCode;
                lblThePrice.Text = string.Format("{0} {1}", CurrencyHelper.GetLocalizedCurrency("0"), priceCode);
                piTotal.InnerText = string.Format("{0} {1}", CurrencyHelper.GetLocalizedCurrency("0"), priceCode);
                checkoutPresenter.GetDeliveryEmails();

                // Get the product detail.
                bool promoIsValid = true;
                if (!String.IsNullOrEmpty(PromoCode))
                {
                    promoIsValid = checkoutPresenter.ValidatePromoCode(PromoCode);
                }
                if (promoIsValid)
                {
                    Response.AddHeader("promoCodeValidation", "True");
                }
                else
                {
                    Response.AddHeader("promoCodeValidation", "False");
                    PromoCode = String.Empty;
                }
                OrderPreviewDetails orderPreview = null;
                // Don't need to preview if we're creating the order
                bool createOrder = false;
                bool.TryParse(Request["createOrder"], out createOrder);
                if (!createOrder)
                {
                    orderPreview =
                       checkoutPresenter.ExpressCheckoutPreviewOrder(
                       corbisId,
                       AttributeValueUid,
                       isValidPayment);
                }
                if (orderPreview != null)
                {
                    AgessaFlag = orderPreview.Costs.AgessaFlag;
                }
                if (Profile.CountryCode == "DE")
                {
                    ksaHolder.Visible = true;
                }
                if (AgessaFlag == true)
                {
                    Response.AddHeader("AgessaFlag", bool.TrueString);
                }
                else
                {
                    Response.AddHeader("AgessaFlag", bool.FalseString);
                }

                Response.AddHeader("TaxError", taxError.ToString());

                if (productUid != null && productUid != Guid.Empty)
                {
                    Response.AddHeader("LicenseStartDateRequired", pricingPresenter.IsLicenseStartDateRequired(productUid).ToString());
                }
                else
                {
                    //Default make it to true
                    Response.AddHeader("LicenseStartDateRequired", true.ToString());
                }
            }
            catch
            {
                Response.Clear();
                Response.Write(HttpContext.GetLocalResourceObject(SiteUrls.ExpressCheckout, "pricingCalculationErrorMessage"));
                Response.End();
            }
        }
        protected bool Is_Numeric(string thePrice){
            try
            {
                Convert.ToInt32(thePrice);
                return true;
            }
            catch
            {
                try
                {
                    Convert.ToDouble(thePrice);
                    return true;
                } 
                catch 
                {
                    return false;
                }

            }
        }
        #region ICheckoutView Members

        List<Guid> productUids;
        public List<Guid> ProductUids
        {
            get
            {
                if (productUids == null)
                {
                    productUids = new List<Guid>();
                }

                return productUids;
            }
            set
            {
                productUids = value;
            }
        }

        public bool HasRFCD
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<Corbis.Web.Entities.CheckoutProduct> CheckoutProducts
        {
            set { throw new NotImplementedException(); }
        }

        PreviewAndCreateRequest previewRequest=null;
        public Corbis.WebOrders.Contracts.V1.PreviewAndCreateRequest PreviewRequest
        {
            get
            {
                // Lazy Load
                if (previewRequest == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    previewRequest = stateItems.GetStateItemValue<PreviewAndCreateRequest>(
                        "ExpressCheckout",
                        "PreviewRequest",
                        StateItemStore.AspSession);
                }
                return previewRequest;
            }
            set
            {
                previewRequest = value;
                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<PreviewAndCreateRequest> previewStateItem =
                    new StateItem<PreviewAndCreateRequest>(
                    "ExpressCheckout",
                    "PreviewRequest",
                    previewRequest,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (previewRequest != null)
                {
                    stateItems.SetStateItem<PreviewAndCreateRequest>(previewStateItem);
                    //LoadSubmitStepData(value);
                }
                else
                {
                    stateItems.DeleteStateItem<PreviewAndCreateRequest>(previewStateItem);
                }
            }
        }

        public bool IsCreditEnable
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

        public bool HasCorporateAccount
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

        private ContractType contractType;
        public Corbis.CommonSchema.Contracts.V1.ContractType ContractType
        {
            get
            {
                return contractType;
            }
            set
            {
                contractType = value;
            }
        }

        public IProject Project
        {
            get { return this; }
        }

        public IDelivery Delivery
        {
            get { return this; }
        }

        public IPayment Payment
        {
            get { return this; }
        }

        public ISubmit Submit
        {
            get { return this; }
        }

        public string SubTotal
        {
            get
            {
                return this.piPrice.InnerText;
            }
            set
            {
                this.piPrice.InnerText = value;
            }
        }

        public string TotalCost
        {
            get
            {
                return this.piTotal.InnerText;
            }
            set
            {
                this.piTotal.InnerText = value;
            }
        }

        public string Tax
        {
            get
            {
                return this.piTax.InnerText;
            }
            set
            {
                this.piTax.InnerText = value;
            }
        }

        public string TaxLabel
        {
            get
            {
                return this.taxLabel.Text;
            }
            set
            {
                this.taxLabel.Text = value;
            }
        }

        public string KsaTax
        {
            get
            {
                return this.piKsa.InnerText;
            }
            set
            {
                
                this.piKsa.InnerText = value;
            }
        }

        public string PromotionDiscount
        {
            get
            {
                return this.piPromotion.InnerText;
            }
            set
            {
                this.piPromotion.InnerText = value;
            }
        }

        private bool agessaFlag;
        public bool AgessaFlag
        {
            get
            {
                return agessaFlag;
            }
            set
            {
                agessaFlag = value;
            }
        }

        public bool AcceptRestriction
        {
            get { throw new NotImplementedException(); }
        }

        public bool AcceptLicenseAgreement
        {
            get { throw new NotImplementedException(); }
        }

        public void PriceUpdate()
        {
        }

        private bool taxError = false;
        public void TaxError()
        {
            // Set tax error flag.
            taxError = true;
        }
        public bool IsCoffFlow
        {
            get
            {
                return WorkflowHelper.IsCOFFWorkflow(Request);
            }
        }

        #endregion

        #region IProject Members


        /// <summary>
        /// Project Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string JobNumber { get; set; }

        public string PONumber { get; set; }

        public string Licensee { get; set; }
        #endregion

        #region IDelivery Members


        public Corbis.CommonSchema.Contracts.V1.DeliveryMethod RfcdDeliveryMethod
        {
            get
            {
                return DeliveryMethod.RFCD_Download;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private string deliveryEmails;
        public string DeliveryEmails
        {
            get
            {
                return deliveryEmails;
            }
            set
            {
                deliveryEmails = value;
            }
        }

        public List<Corbis.Office.Contracts.V1.ShippingPriority> ShippingPriorities
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

        public bool IsCoffRMResolution1280
        {
            get
            {
               return false;
            }
            set
            {
            }
        }
        public string SpecialInstruction
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        public string DeliverySubject
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        public string ShippingPriority
        {
            get
            {
                return string.Empty;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid ShippingAddressUid
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

        public Corbis.Membership.Contracts.V1.MemberAddress ShippingAddress
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid SelectedAddress
        {
            set { throw new NotImplementedException(); }
        }

        public List<Corbis.Membership.Contracts.V1.MemberAddress> SavedShippingAddresses
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

        public Corbis.CommonSchema.Contracts.V1.DeliveryMethod NonRfcdDeliveryMethod
        {
            get
            {
                return DeliveryMethod.Download;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<Corbis.CommonSchema.Contracts.V1.DeliveryMethod> AvailableNonRfcdDeliveryMethods
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

        public List<Corbis.CommonSchema.Contracts.V1.DeliveryMethod> AvailableRfcdDeliveryMethods
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

        #region ISubmit Members

        public string CorporateAccountName
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

        public string CreditCardType
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

        public string CreditCardNumberViewable
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

        public string CreditCardExirationMonth
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

        public string CreditCardExpirationYear
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

        public string CreditCardCardHolderName
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

        public string PromoDiscount
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

        public string ShippingCost
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

        public string TaxOrVat
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

        public Corbis.Membership.Contracts.V1.MemberAddress BillingAddress
        {
            set { throw new NotImplementedException(); }
        }

        public void LoadSubmitStepData(Corbis.WebOrders.Contracts.V1.OrderPreviewDetails data)
        {
        }

        public List<Corbis.Web.Entities.ProductRestriction> AllRestrictions
        {
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IPayment Members
        PaymentMethod method;
        public Corbis.WebOrders.Contracts.V1.PaymentMethod Method
        {
            get
            {
                return method;
            }
            set
            {
                method = value;
            }
        }

        private int selectedCorporateAccountID=0;
        public int SelectedCorporateAccountID
        {
            get { return selectedCorporateAccountID; }
            set
            {
                selectedCorporateAccountID = value;
            }
        }

        private string creditCardUid;
        public string CreditCardUid
        {
            get
            {
                return creditCardUid;
            }
            set
            {
                creditCardUid = value;
            }
        }

        private string creditCardValidationCode=string.Empty;
        public string CreditCardValidationCode
        {
            get { return creditCardValidationCode; }
        }

        public Corbis.Membership.Contracts.V1.PaymentMethod DefulatPaymentMethod
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

        private string promoCode;
        public string PromoCode
        {
            get
            {
                return promoCode;
            }
            set
            {
                promoCode = value;
            }
        }

        public Corbis.Membership.Contracts.V1.CreditCard CreditCard
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

        public List<Corbis.Web.Entities.ContentItem> CardTypes
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

        public List<Corbis.Membership.Contracts.V1.CreditCard> CreditCards
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

        public List<Corbis.Membership.Contracts.V1.Company> CorporateAccounts
        {
            get
            {
              throw new NotImplementedException(); 
            }
            set { throw new NotImplementedException(); }
        }

        public string CorporateAccountText
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

        public bool DisplyCorporateAccountIcon
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

        public bool DisplySavedCreditCardIcon
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

        public bool DisplyNewCreditCardIcon
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

        public bool DisplayPaymentChosen_Error
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

        public bool DisplayCreditReviewError
        {
            set { throw new NotImplementedException(); }
        }

        public bool DisplayValidatePromoError
        {
            set { throw new NotImplementedException(); }
        }

        public bool SetCorporateAsDefault
        {
            set { throw new NotImplementedException(); }
        }

        public bool SetCreditAsDefult
        {
            set { throw new NotImplementedException(); }
        }

        public bool SetCreditNewAsDefult
        {
            set { throw new NotImplementedException(); }
        }

        public int DefaultPayment
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

        #region IRFPricing Members

        private string corbisId;
        public string CorbisId
        {
            get
            {
                return corbisId;
            }
            set
            {
                corbisId = value;
            }
        }

        private Guid productUid;
        public Guid ProductUid
        {
            get
            {
                return productUid;
            }
            set
            {
                productUid = value;
            }
        }

        private RfPriceList priceList;
        public RfPriceList PriceList
        {
            get
            {
                return priceList;
            }
            set
            {
                priceList = value;
            }
        }

        public Corbis.Web.Entities.ParentPage ParentPage
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

        private Guid useCategoryUid;
        public Guid UseCategoryUid
        {
            get
            {
                return useCategoryUid;
            }
            set
            {
                useCategoryUid = value;
            }
        }

        Guid useTypeUid;
        public Guid UseTypeUid
        {
            get
            {
                return useTypeUid;
            }
            set
            {
                useTypeUid = value;
            }
        }

        public void AddToCompletedUsageAVPairList(Guid useAttributeUid, object value, bool clearListBeforeAdd)
        {
            throw new NotImplementedException();
        }

        public IPricingHeader PricingHeader
        {
            get { return null; }
        }

        public PriceStatus EffectivePriceStatus
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

        public string EffectivePriceText
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

        public IImageRestrictionsView RestrictionsControl
        {
            get { throw new NotImplementedException(); }
        }

        private Guid attributeValueUid;
        public Guid AttributeValueUid
        {
            get
            {
                return attributeValueUid;
            }
            set
            {
                attributeValueUid = value;
            }
        }

        public bool PricedByAE
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

        public bool CustomPriced
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

        public bool ValidLicense
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

        public string DimensionText
        {
            set { throw new NotImplementedException(); }
        }

        public string FileSizeText
        {
            set { throw new NotImplementedException(); }
        }

        public ImageSize ImageFileSize
        {
            set { throw new NotImplementedException(); }
        }

        public bool ShowPriceStatusMessageForRF
        {
            set
            {
                throw new System.NotImplementedException();
            }

        }

        #endregion

    }
}
