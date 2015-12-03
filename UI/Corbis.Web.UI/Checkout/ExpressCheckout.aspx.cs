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
using Corbis.Web.UI.Cart.ViewInterfaces;
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
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Office.Contracts.V1;
using System.Linq;
using Corbis.Web.UI.Presenters.CustomerService;
using Corbis.Framework.Globalization;
using Corbis.Common.ServiceFactory.Validation;


namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout : CorbisBasePage, IExpressCheckout, IPricingHeader, IRFPricing, IRMPricingExpressCheckout, IProject, IPayment, ICheckoutView, IDelivery, ISubmit
    {
        private CheckoutPresenter checkoutPresenter;
        private PricingPresenter pricingPresenter;
        private StateItemCollection stateItem;
        private string parentProtocol;

        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            StandAlone,
            protocol
        }

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            ScriptManager manager = (System.Web.UI.ScriptManager)Master.FindControl("scriptManager");
            manager.Services.Add(new ServiceReference("~/Checkout/CheckoutService.asmx"));
            stateItem = new StateItemCollection(HttpContext.Current);

            if (!Page.IsPostBack)
            {
                string localizedProject = GetLocalResourceString("project");
                string projectName = DateHelper.GetLocalizedDate(DateTime.Now, DateFormat.ShortDateFormat);
                if (NonAsciiValidator.IsStringASCII(localizedProject))
                {
                    projectName = projectName + " " + localizedProject;
                }
                LoadingLicenseText = (string)GetLocalResourceObject("LoadLicensingText");
                LoadingPriceText = (string)GetLocalResourceObject("LoadPricingText");
                projectField.Text = projectName;
                projectField.Attributes.Add("onblur", "if(this.value=='') this.value = '" + projectName + "';");
                CustomerServicePresenter customerService = new CustomerServicePresenter();
                standByMessage3.Text = string.Format(standByMessage3.Text, customerService.GetRepresentativeOfficePhone());
                ProjectFieldText = projectName;
            }

            this.AddScriptToPage(SiteUrls.ExpressCheckoutScript, "ExpressCheckoutScript");

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ColumnLayouts, "ColumnLayoutsCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressCheckout, "ExpressCheckoutCSS");
            CheckParentProtocol();
        }
        private void CheckParentProtocol()
        {
            // if opening URL is HTTPS, open iframe using https protocol
            string js = "var ParentProtocol = '";
            if (GetQueryString(QueryString.protocol.ToString()) == "https")
            {
                js += HttpsUrl + "';";
                parentProtocol = HttpsUrl;
            }
            else
            {
                js += HttpUrl + "';";
                parentProtocol = HttpUrl;
            }
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "parentProtocol", js, true);
        }


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AnalyticsData["events"] = AnalyticsEvents.ExpressCheckoutStart;
            AnalyticsData["prop3"] = "express";
            
            Guid productUid = Guid.Empty;
            CorbisId = Request.QueryString["CorbisId"];
            LightboxId = Request.QueryString["LightboxId"];

            pricingPresenter = new PricingPresenter(this);
            checkoutPresenter = new CheckoutPresenter(this);
            DisplayPriceBox();
            checkoutPresenter.GetPaymentAccounts();

            LegalPresenter legalPresenter = new LegalPresenter();
            eulaLink.HRef = legalPresenter.GetLicenseURL();

            //Show Agessa
            if (this.AgessaFlag)
            {
                agessageMessage.Visible = true;
            }

            if (!this.IsPostBack)
            {
                TimeSpan ts = ((CorbisMasterBase)this.Master).GetFormsAuthTimeout();

                if (ts != null)
                {
                    string sessionTimeOutJs = "setTimeout(\"CorbisUI.GlobalNav.SessionTimedOut()\"," + ts.TotalMilliseconds + ");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SessionTimeOut", sessionTimeOutJs, true);
                }

                GuidHelper.TryParse(Request.QueryString["ProductUid"], out productUid);
                if (productUid != Guid.Empty)
                {
                    productUids = new List<Guid>();
                    productUids.Add(productUid);
                }
                hidProductUid.Value = productUid.ToString();
                InitializeView();
            }

            checkoutPresenter.GetDeliveryEmails();
            InitializeHtmlProperties();

            if (this.submitPurchaseClicked.Value == "True")
            {
                Submit_Purchase(this.purchaseButton, EventArgs.Empty);
            }
            HandleProfileBasedViewSettings();
        }

        private void InitializeView()
        {
            this.licenseeImageInfoIcon.Attributes.Add("title", GetLocalResourceString("licensee.Text"));
            // TODO:Security - We don't need it. 
            this.licenseeImageInfoIcon.Attributes.Add("rel", Server.HtmlEncode(GetLocalResourceString("licenseeInfo.Text")));
            mainStatement.Text = string.Format(GetLocalResourceString("mainStatement.Text"),
              "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
              CorbisId + "', null, 1, '" + ParentPage.ToString() + "');");

            this.piPrice.InnerText = CurrencyHelper.GetLocalizedCurrency("0");
            this.piTotal.InnerText = CurrencyHelper.GetLocalizedCurrency("0");
            CustomPriceExpiredDelay = "0";
            string caller = Request.QueryString["caller"];
            checkoutPresenter.InitializeExpressCheckout(caller);
            pricingPresenter.GetHeaderDetails();
            RFCustomPricedDisplay.Visible = false;


            if (this.LicenseModel == "RF")
            {
                pricingPresenter.InitializeRFPricingForm((string)GetLocalResourceObject("ContactUs"), (string)GetLocalResourceObject("AsPerContract"));
                pricesVaryWrap.Visible = true;
                fileSizeDiv.Visible = true;
            }
            else if (this.LicenseModel == "RM")
            {
                pricingPresenter.SetRMExpressCheckoutPricedByAE();
                pricingPresenter.GetAllExpressCheckoutSavedUsages();

                // Check for a new saved usage ...
                this.hidTriggerChangeEvent.Value = "False";//default
                bool isNewSavedUsage = false;
                bool.TryParse(Request.QueryString["isNewUsage"], out isNewSavedUsage);
                if (isNewSavedUsage)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    if (stateItems.TryGetStateItemValue<Guid>(
                            ExpressCheckoutKeys.Name,
                            ExpressCheckoutKeys.NewSavedUsageUid,
                            StateItemStore.AspSession,
                            out savedUsageUid))
                    {
                        // need to set the property so the dropdown gets selected.
                        SavedUsageUid = savedUsageUid;
                        this.hidTriggerChangeEvent.Value = "True";
                        //Remove the item from session
                        stateItems.DeleteStateItem<Guid>(new StateItem<Guid>(
                            ExpressCheckoutKeys.Name,
                            ExpressCheckoutKeys.NewSavedUsageUid,
                            Guid.Empty,
                            StateItemStore.AspSession));
                    }
                    ProjectInformation projectInfo = null;
                    if (stateItems.TryGetStateItemValue<ProjectInformation>(
                        ExpressCheckoutKeys.Name,
                        ExpressCheckoutKeys.ProjectInformation,
                        StateItemStore.AspSession,
                        out projectInfo))
                    {
                        projectFieldText = projectInfo.Name;
                        this.jobField.Text = projectInfo.JobNumber;
                        this.poField.Text = projectInfo.PONumber;
                        this.licenseeField.Text = projectInfo.Licensee;
                        stateItems.DeleteStateItem<ProjectInformation>(new StateItem<ProjectInformation>(
                            ExpressCheckoutKeys.Name,
                            ExpressCheckoutKeys.ProjectInformation,
                            projectInfo,
                            StateItemStore.AspSession));
                    }
                }

            }
            paymentMethodControl.PopulatepaymentMethod(this.HasCorporateAccount, this.IsCreditEnable);
            SetPaymentDropDown();
        }

        private void HandleProfileBasedViewSettings()
        {
            this.truste.Visible = Profile.CountryCode == UScountryCode;
        }

        private void InitializeHtmlProperties()
        {
            createNewUsageButton.OnClientClick = "CorbisUI.ExpressCheckout.openCreateNewUsage('" + CorbisId + "');return false;";
            noPaymentContact.OnClientClick = String.Format("parent.window.location='{0}';return false;", SiteUrls.CustomerService);
            //contactCorbis.OnClientClick = String.Format("parent.window.location='{0}';return false;", SiteUrls.CustomerService);
            contactCorbis.OnClientClick = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                               CorbisId + "', null, 1, '" + ParentPage.ToString() + "'); return false;";
            //contactCorbis.OnClientClick = "javascript:CorbisUI.ExpressCheckout.openContactCorbis('" +
            //CorbisId + "', null, 1, '" + ParentPage.ToString() + "');";
        }

        private void DisplayPriceBox()
        {

            if (Profile.CountryCode == "FR")
            {
                TaxLabel = (string)HttpContext.GetGlobalResourceObject("Resource", "French_Tax");
            }
            else if (Profile.CountryCode == "CA")
            {
                TaxLabel = (string)HttpContext.GetGlobalResourceObject("Resource", "Canada_Tax");
            }
            if (Profile.CountryCode == "DE")
            {
                ksaHolder.Visible = true;
            }
            CurrencyCode = string.Format("({0})", Profile.CurrencyCode);

            //else
            //{
            //    TaxLabel = "Tax";
            //}
        }
        public void RFPricing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PricedUseTypeAttributeValue attribute = (PricedUseTypeAttributeValue)e.Item.DataItem;
                ((Label)e.Item.FindControl("lblDisplayText")).Text =
                    GetKeyedEnumDisplayText(attribute.ImageSize, "RF");
                
                Label uncommpressed = (Label)e.Item.FindControl("UncompressedFileSize");
                uncommpressed.Text = string.Format("({0})", uncommpressed.Text);

                Label pixelWidth = (Label)e.Item.FindControl("pixelWidth");
                pixelWidth.Text = string.Format("{0}px ", pixelWidth.Text);

                Label pixelHeight = (Label)e.Item.FindControl("pixelHeight");
                pixelHeight.Text = string.Format(" {0}px x", pixelHeight.Text);

                Label imageWidth = (Label)e.Item.FindControl("imageWidth");
                Label imageHeight = (Label)e.Item.FindControl("imageHeight");

                Label lblPriceText = (Label)e.Item.FindControl("lblPriceText");

                if (Profile.CountryCode.Equals("US", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageWidth.Text = string.Format("{0}in @ ", imageWidth.Text);
                    imageHeight.Text = string.Format("{0}in x", imageHeight.Text);
                }
                else
                {
                    imageWidth.Text = string.Format("{0}cm @ ", imageWidth.Text);
                    imageHeight.Text = string.Format("{0}cm x", imageHeight.Text);
                }

                Label resolution = (Label)e.Item.FindControl("resolution");
                resolution.Text = string.Format("{0}ppi", resolution.Text);

                HtmlInputHidden valueUid = (HtmlInputHidden)e.Item.FindControl("ValueUID");
                HtmlInputHidden hidEffectivePrice = (HtmlInputHidden)e.Item.FindControl("HidEffectivePrice");
                HtmlInputHidden currencyCode = (HtmlInputHidden)e.Item.FindControl("CurrencyCode");
                HtmlGenericControl rfPricingRepeaterDataRow = (HtmlGenericControl)e.Item.FindControl("RFPricingRepeaterDataRow");
                HtmlGenericControl rfPricingRowLeft = (HtmlGenericControl)e.Item.FindControl("RFPricingRowLeft");
                HtmlGenericControl rfPricingRowRight = (HtmlGenericControl)e.Item.FindControl("RFPricingRowRight");
            }
        }

        protected void StartOver_Click(object sender, EventArgs e)
        {
            //AnalyticsData["events"] = AnalyticsEvents.PricingRMStartOver;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccessAnalytics", "LogOmnitureEvent('event26');", true);
            
            ProductUid = Guid.Empty;
            hidSavedUsageUid.Value = Guid.Empty.ToString();
            projectFieldText = projectField.Text;
            this.paymentMethodControl.SelectPaymentMethod.Items.Clear();
            ListItem selectOne = new ListItem(Resources.Resource.SelectOne, "none");
            selectOne.Attributes.Add("type", "none");
            this.paymentMethodControl.SelectPaymentMethod.Items.Add(selectOne);
            InitializeView();
        }

        protected void SessionTimeOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExitExpressCheckoutAfterTimeout", "window.addEvent('domready', function() { CloseExpressCheckoutIModalAfterTimeout()});", true);
        }

        private void SetPaymentDropDown()
        {
            string itemToSelectBasedOnValue = "none";
            if (!this.IsPostBack)
            {
                if (defaultPaymentMethod == Corbis.Membership.Contracts.V1.PaymentMethod.CorporateAccount)
                {
                    itemToSelectBasedOnValue = SelectedCorporateAccountID.ToString();
                }
                if (defaultPaymentMethod == Corbis.Membership.Contracts.V1.PaymentMethod.CreditCard || defaultPaymentMethod == Corbis.Membership.Contracts.V1.PaymentMethod.NotSet)
                {
                    CreditCard card = CreditCard;
                    if (card != null)
                    {
                        itemToSelectBasedOnValue = card.CreditCardUid.ToString();
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(hidPaymentMethod.Value) &&
                    Enum.IsDefined(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod), hidPaymentMethod.Value))
                {
                    Method = (Corbis.WebOrders.Contracts.V1.PaymentMethod)Enum.Parse(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod), hidPaymentMethod.Value);
                    if (Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CorporateAccount)
                    {
                        int selectedCorporateId = 0;
                        if (int.TryParse(corporateID.Value, out selectedCorporateId))
                        {
                            itemToSelectBasedOnValue = corporateID.Value;
                        }
                    }
                    else if (Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CreditCard)
                    {
                        Guid selectedCardUid = Guid.Empty;
                        if (GuidHelper.TryParse(creditCardUID.Value, out selectedCardUid))
                        {
                            itemToSelectBasedOnValue = creditCardUID.Value;
                        }
                    }
                }
            }
            foreach (ListItem item in paymentMethodControl.SelectPaymentMethod.Items)
            {
                if (item.Value == itemToSelectBasedOnValue)
                {
                    item.Attributes["Selected"] = "selected";
                    break;
                }
            }
        }

        //Added for Credit Card modal - eH
        protected void cardCreator_CreditCardAdded(object sender, CommandEventArgs e)
        {
            CreditCard card = e.CommandArgument as CreditCard;
            CreditCard = card;
            if (card != null)
            {
                foreach (ListItem item in paymentMethodControl.SelectPaymentMethod.Items)
                {
                    if (item.Value == card.CreditCardUid.ToString())
                    {
                        item.Attributes["Selected"] = "selected";
                        break;
                    }
                }


            }
        }


        protected void cardCreator_CancelClick(object sender, EventArgs e)
        {
            /*
             if (creditDisplyTextNew.Text == string.Empty)
            {

                //error case
                //CreditCardUid = Guid.Empty.ToString();
                //addCardBlock.Visible = false;
                //addCardBlockErr.Visible = true;
                //addCardUpdatePanel.Update();
            }
             */
        }
        protected void cardSelector_CancelClick(object sender, EventArgs e)
        {
            /*
             if (creditDisplyTextNew.Text == string.Empty)
            {

                //error case
                //CreditCardUid = Guid.Empty.ToString();
                //addCardBlock.Visible = false;
                //addCardBlockErr.Visible = true;
                //addCardUpdatePanel.Update();
            }
             */
        }

        #region Purchase Sumbit

        protected void Submit_Purchase(Object sender, EventArgs e)
        {
            //Check all properties
            var soabSafari = IsSafariBrowserRequest();
            int lightbox = 0;
            int.TryParse(LightboxId, out lightbox);

            // TODO: LiceseStart Date should really be implemented on the interface
            // See technical bug 17010
            DateTime licensStartDate;
            DateHelper.TryParse(Request["StartDate"], out licensStartDate);

            #region Payment

            //by default make it false
            bool isValidPayment = false;
            string paymentValue = Request["thePaymentMethod"];
            string paymentType = Request["thePaymentType"];
            string contractType = Request["ContractType"];
            PromoCode = Request["PromoCode"];
            if (!string.IsNullOrEmpty(contractType) &&
                Enum.IsDefined(typeof(ContractType), contractType))
            {
                ContractType = (ContractType)Enum.Parse(typeof(ContractType), contractType);
            }
            if (!string.IsNullOrEmpty(paymentType) &&
                Enum.IsDefined(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod), paymentType))
            {
                Method = (Corbis.WebOrders.Contracts.V1.PaymentMethod)Enum.Parse(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod), paymentType);
            }
            if (Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CreditCard)
            {
                CreditCardUid = paymentValue;
                isValidPayment = true;
            }
            else if (Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CorporateAccount)
            {
                int corporateAccountId = 0;
                if (int.TryParse(paymentValue, out corporateAccountId))
                {
                    corporateID.Value = corporateAccountId.ToString();
                }
                isValidPayment = true;
            }
            #endregion

            checkoutPresenter = new CheckoutPresenter(this);

            OrderConfirmationDetails order = null;
            if (isValidPayment)
            {
                try
                {
                    order = checkoutPresenter.ExpressCheckoutCreateOrder(CorbisId, licensStartDate);
                }
                catch (Exception)
                {

                    this.hidRedirectUrl.Value = SiteUrls.ExpressCheckoutOrderSubmissionError;
                    if (soabSafari)
                        Response.Redirect(SiteUrls.ExpressCheckoutOrderSubmissionError, true);
                    return;
                }
            }

            if (order != null)
            {
                //1. setup the property 
                //parent.OrderUid = order.OrderUid;
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                stateItems.SetStateItem<Guid>(
                    new StateItem<Guid>(
                        OrderKeys.Name,
                        OrderKeys.OrderUid,
                        order.OrderUid,
                        StateItemStore.AspSession,
                        StatePersistenceDuration.Session));
                //2. go to orderconfirmation page
                this.hidRedirectUrl.Value = string.Format("{0}?orderuid={1}&protocol={2}", SiteUrls.ExpressDownload, order.OrderUid, GetQueryString(QueryString.protocol.ToString()));
                if (soabSafari)
                    Response.Redirect(string.Format("{0}?orderuid={1}&protocol={2}", SiteUrls.ExpressDownload, order.OrderUid, GetQueryString(QueryString.protocol.ToString())));
            }
        }
        #endregion


        public string StyleSheet
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    HtmlHelper.CreateStylesheetControl(this.Page, value, "ExpressCheckoutCSS");
                    HtmlHelper.CreateStylesheetControl(this.Page, value, "ColumnLayoutsCSS");
                    HtmlHelper.CreateStylesheetControl(this.Page, value, "TooltipsCSS");
                }
            }
        }

        #region IExpressCheckout Members
        //public  ICheckoutView CheckoutView 
        //{
        //    get
        //    {
        //        return this;
        //    }
        //}
        private String loadingPriceText;
        public String LoadingPriceText
        {
            get
            {
                return loadingPriceText;
            }
            set
            {
                loadingPriceText = value;
            }
        }
        private String projectFieldText;
        public String ProjectFieldText
        {
            get { return StringHelper.EncodeToJsString(projectFieldText); }
            set { projectFieldText = value; }
        }

        private String loadingLicenseText;
        public String LoadingLicenseText
        {
            get
            {
                return loadingLicenseText;
            }
            set
            {
                loadingLicenseText = value;
            }
        }
        private Guid offeringUid = Guid.Empty;
        public Guid OfferingUid
        {
            get
            {
                return offeringUid;
            }
            set
            {
                offeringUid = value;
            }
        }

        public IPricingHeader PricingHeader
        {
            get
            {
                return this;
            }
        }

        public IRFPricing RFPricing
        {
            get
            {
                return this;
            }
        }


        #region ICheckOutView

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
        List<Guid> productUids;
        public List<Guid> ProductUids
        {
            get
            {
                if (productUids == null || productUids.Count == 0)
                {
                    productUids = new List<Guid>();
                    productUids.Add(ProductUid);
                }

                return productUids;
            }
            set
            {
                productUids = value;
            }
        }


        public List<Corbis.Web.Entities.CheckoutProduct> CheckoutProducts
        {
            set { throw new NotImplementedException(); }
        }

        private ContractType contractType;
        public ContractType ContractType
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
        public Guid OrderUid { get; set; }
        private bool hasRfcd;
        public bool HasRFCD
        {
            get
            {
                return hasRfcd;
            }
            set
            {
                hasRfcd = value;
            }
        }
        public void PriceUpdate()
        {

        }
        public void TaxError()
        {
        }
        #region Products

        PreviewAndCreateRequest previewRequest = null;
        public PreviewAndCreateRequest PreviewRequest
        {
            get
            {
                // Lazy Load
                if (previewRequest == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    previewRequest = stateItems.GetStateItemValue<PreviewAndCreateRequest>(
                        ExpressCheckoutKeys.Name,
                        ExpressCheckoutKeys.PreviewRequest,
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
                    ExpressCheckoutKeys.Name,
                    ExpressCheckoutKeys.PreviewRequest,
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

        #endregion

        #region costs

        public string Tax
        {
            get
            {
                //return this.itemSummaryBlock.Tax;
                return this.piTax.InnerText;
            }
            set
            {
                //this.itemSummaryBlock.Tax = value;
                this.piTax.InnerText = value;
            }
        }
        public string TaxLabel
        {
            get
            {
                //return this.itemSummaryBlock.TaxLabel;
                return taxLabel.Text;
            }
            set
            {
                //this.itemSummaryBlock.TaxLabel = value;
                this.taxLabel.Text = value;
                //this.piPromotionCode.InnerText = value;
                //this.piTotalCode.InnerText = value;
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
                //this.itemSummaryBlock.KsaTax = value;
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
                //this.itemSummaryBlock.PromotionAmount = value;
                this.piPromotion.InnerText = value;
            }
        }
        //Todo How to Get Agessa???
        public bool AgessaFlag
        {
            get
            {
                return false;
            }
            set
            {
                //importantSection.Visible = value;
                //this.importantBlock.Update();
                //agessa.Visible = value;
            }
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
                //return itemSummaryBlock.Total;
                return this.piTotal.InnerText;
            }
            set
            {
                //itemSummaryBlock.Total = value;
                this.piTotal.InnerText = value;
                //this.Submit.TotalCost = value;
            }
        }
        private bool isCreditEnable;
        public bool IsCreditEnable
        {

            get
            {
                return isCreditEnable;
            }

            set
            {
                isCreditEnable = value;
            }
        }
        private bool hasCorporateAccount;
        public bool HasCorporateAccount
        {

            get
            {

                return hasCorporateAccount;
            }

            set
            {
                hasCorporateAccount = value;

            }
        }
        #endregion

        #region Acceptances

        public bool AcceptRestriction
        {
            get
            {
                return acceptRestrictions.Checked;

            }

        }

        public bool AcceptLicenseAgreement
        {
            get
            {
                return acceptLicenseAgreement.Checked;
            }
        }

        #endregion



        #endregion

        #region IPricingHeader Members
        string corbisId = string.Empty;
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }
        string lightboxId = string.Empty;
        public string LightboxId
        {
            get { return lightboxId; }
            set
            {
                lightboxId = value;
                ParentPage = ParentPage.ExpressCheckout;
                //// Commenting out this logic as i m not sure if parent page needs to be ExpressCheckout or Search/Lightbox.
                //if (string.IsNullOrEmpty(value))
                //{
                //    ParentPage = ParentPage.Search;
                //}
                //else
                //{
                //    ParentPage = ParentPage.Lightbox;
                //}
            }
        }
        public string ImageThumbnail
        {
            set
            {
                imageThumbnail.ImageUrl = value.Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=90&im=");
                rstImageThumbnail.ImageUrl = value.Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=90&im=");
            }

        }

        public bool IsThumbPortrait
        {
            set
            {
                if (value) imageThumbnail.Height = 90;
                else imageThumbnail.Width = 90;
            }
        }

        public Corbis.CommonSchema.Contracts.V1.Category ImageTitle
        {
            set { imageTitle.Text = CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.Category>(value); }
        }

        public string ImageId
        {
            set { imageId.Text = value; }
        }

        public string PricingTier
        {
            set { pricingTier.Text = value; }
        }

        private string licenseModel;
        public string LicenseModel
        {
            get
            {
                return licenseModel;
            }
            set
            {
                licenseModel = value;
                pricingTier.CssClass = licenseModel + "_color";
                licenseModal.Text = GetGlobalResourceObject("Resource", value + "Text") as string;
                licenseModal.CssClass = value + "_color"; 
                if (licenseModel == "RF")
                {
                    imageLicenseType.Text = GetLocalResourceObject("rfDisplayText").ToString().ToUpper();
                    imageLicenseType.CssClass = "RF_green";
                    RMPricingWrap.Visible = false;
                    youAreAboutToPurchase.Text = GetLocalResourceString("youAreAboutToPurchase.RF");
                }
                else if (licenseModel == "RM")
                {
                    imageLicenseType.Text = GetLocalResourceObject("rmDisplayText").ToString().ToUpper();
                    imageLicenseType.CssClass = "RM_blue";

                    //TODO: Move this to Page Event area...maybe
                    RFPricingWrap.Visible = false;
                    RMPricingWrap.Visible = true;
                    RFCustomPricedDisplay.Visible = false;

                    youAreAboutToPurchase.Text = GetLocalResourceString("youAreAboutToPurchase.RM");
                }
            }
        }

        public bool CartButtonEnabled
        {
            get
            {
                return false;
            }
            set
            {
            }
        }


        public string CartButtonText
        {
            set { }
        }


        public bool UpdatingCart { get; set; }

        public string LightboxButtonText
        {
            set { }
        }

        public bool LightboxButtonEnabled
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool UpdatingLightbox { get; set; }

        public bool LicenseDetailsVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool imageContainerVisible
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public string CurrencyCode
        {
            set { piPriceCode.InnerText = value; piTotalCode.InnerText = value; }
        }

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

        #endregion

        #region IRFPricing Members
        public Corbis.LightboxCart.Contracts.V1.RfPriceList PriceList
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                List<PricedUseTypeAttributeValue> sourceList = ((RfPriceList)value).PricedAttributeValues;

                if (sourceList.Count > 0)
                {
                    rpRFPricing.DataSource = sourceList;
                    rpRFPricing.DataBind();

                    rpRFPricingLabel.Visible = false;
                }
                else
                {
                    rpRFPricing.Visible = false;
                }
            }
        }

        //TODO: KevinH 3/22/2009 - Filed as bug 16670
        //      There is way too much logic in this property setter, not to mention the fact 
        //      That it relies on Properties being set in a specific order. 
        //      The following Properties have to be set before this property can be set:
        //      1) EffectivePriceStatus; 2)CorbisId; 3) ParentPage; 4) ValidLicense
        private bool pricedByAE = false;
        public bool PricedByAE
        {
            get { return pricedByAE; }
            set
            {
                this.hidCustomPricingExpired.Value = "False";
                pricedByAE = value;
                if (value)
                {
                    CustomPricedExpired = (EffectivePriceStatus & PriceStatus.CustomPriceExpired) == PriceStatus.CustomPriceExpired;
                    if (CustomPricedExpired)
                    {
                        AnalyticsData["events"] = AnalyticsEvents.PricingExpired;
                        
                        this.hidCustomPricingExpired.Value = "True";
                        RFCustomPricedDisplay.Visible = false;


                        // Display Cutom Price Expired Model pop up.
                        if (!this.IsPostBack && Request.Browser.Browser == "IE")
                        {
                            CustomPriceExpiredDelay = "500";
                        }
                        else
                        {
                            CustomPriceExpiredDelay = "0";
                        }
                    }
                    else
                    {
                        if (this.LicenseModel == "RF")
                        {
                            // Display Priced by AE atert message & Licensing details on UI. 
                            RFCustomPricedDisplay.Visible = true;
                            rpRFPricing.Visible = false;
                            //RMPricingDiv.Visible = false;
                            hidAttributeValueUID.Value = string.Empty;

                            if (!ValidLicense ||
                                (EffectivePriceStatus & PriceStatus.CountryOrCurrencyError) == PriceStatus.CountryOrCurrencyError
                                )
                            {
                                // Priced by AE and license details are invalid.
                                // Show the message label for license expired Priced by AE image.
                                fileSizeWrapper.Visible = false;
                                dimensionSizeWrapper.Visible = false;

                                //lblContactStatement.Text = string.Format(GetLocalResourceString("lblContactStatement.Text"),
                                //   "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                                //   CorbisId + "', null, 1, '" + ParentPage.ToString() + "');");
                                btnContactAE.OnClientClick = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                                   CorbisId + "', null, 1, '" + ParentPage.ToString() + "'); return false;";
                                itemNotAvailable.Visible = true;
                            }
                            else
                            {
                                hidAttributeValueUID.Value = "valid";
                            }
                        }
                    }
                }
                else
                {
                    // Hide Priced by AE atert message on UI.
                    RFCustomPricedDisplay.Visible = false;
                }
            }
        }

        private PriceStatus effectivePriceStatus;
        public PriceStatus EffectivePriceStatus
        {
            get { return effectivePriceStatus; }
            set { effectivePriceStatus = value; }
        }

        private string effectivePriceText;
        public string EffectivePriceText
        {
            get { return effectivePriceText; }
            set
            {
                if (value == "0.00")
                {
                    AnalyticsData["events"] = AnalyticsEvents.PricingFailure;
                }
                else
                {
                    AnalyticsData["events"] = AnalyticsEvents.PricingFinish;       
                }
                
                effectivePriceText = value;
                piPrice.InnerText = effectivePriceText;
            }
        }

        private bool customPriced = false;
        public bool CustomPriced
        {
            get
            {
                return customPriced;
            }
            set
            {
                customPriced = value;
            }
        }

        private Guid useCatgoryUid;
        public Guid UseCategoryUid
        {
            get { return useCatgoryUid; }
            set
            {
                useCatgoryUid = value;
            }
        }

        private Guid useTypeUid;
        public Guid UseTypeUid
        {
            get { return useTypeUid; }
            set { useTypeUid = value; }
        }

        public Guid ProductUid
        {
            get
            {
                Guid productUid = Guid.Empty;
                GuidHelper.TryParse(hidProductUid.Value, out productUid);
                return productUid;
            }
            set
            {
                hidProductUid.Value = value.ToString();
                productUids = new List<Guid>();
                productUids.Add(value);
            }
        }

        public List<CompletedUsageAttributeValuePair> CompletedUsageAVPairList
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public IImageRestrictionsView RestrictionsControl
        {
            get { return null; }
        }

        private Guid valueGuid = Guid.Empty;
        public Guid AttributeValueUid
        {
            get { return valueGuid; }
            set
            {
                valueGuid = value;
                hidAttributeValueUID.Value = valueGuid.ToString();
            }
        }

        private bool validLicense = true;
        public bool ValidLicense
        {
            get { return validLicense; }
            set
            {
                validLicense = value;
            }
        }

        public string FileSizeText
        {
            set
            {
                fileSizeValue.Text = value;
            }
        }

        public string DimensionText
        {
            set
            {
                dimensionsValue.Text = value;
            }
        }

        public ImageSize ImageFileSize
        {
            set
            {
                //fileSizeValue.Text = CorbisBasePage.LocalizeEnum<ImageSize>(value) + fileSizeValue.Text;
                fileSizeValue.Text = value.ToString() + fileSizeValue.Text;
            }
        }

        public void AddToCompletedUsageAVPairList(Guid useAttributeUid, object value, bool clearListBeforeAdd)
        {
            pricingPresenter.AddToCompletedAVPairList(useAttributeUid, value, clearListBeforeAdd);
        }

        public bool ShowPriceStatusMessageForRF
        {
            set
            {
               // throw new System.NotImplementedException();
                if (value)
                {
                    // Set a timeout for IE if it's not a postback to avoid Operation Aborted exception
                    if (!this.IsPostBack)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "imageNotAvailableForCheckout", "window.addEvent('load', function() {OpenModal('imageNotAvailableForCheckout')});", true);
                    }
                }
                 
            }

        }

        #endregion

        #region IRMPricing Members

        private Guid savedUsageUid;
        public Guid SavedUsageUid
        {
            get { return savedUsageUid; }
            set
            {
                savedUsageUid = value;
                if (value != Guid.Empty)
                {
                    savedUsages.SelectedValue = value.ToString();
                }
                else
                {
                    savedUsages.SelectedIndex = 0;
                }
            }
        }

        private List<KeyValuePair<System.Guid, string>> savedUsagesFullText = null;
        public List<KeyValuePair<System.Guid, string>> SavedUsages
        {
            set
            {
                savedUsagesFullText = value;
                if (savedUsagesFullText != null && savedUsagesFullText.Count > 0)
                {
                    //Dictionary<Guid, string> savedUsagesTruncated = new Dictionary<Guid, string>(savedUsagesFullText.Count);
                    //foreach (Guid uid in savedUsagesFullText.Keys)
                    //{
                    //    savedUsagesTruncated.Add(uid, StringHelper.Truncate(savedUsagesFullText[uid], 30));
                    //}
                    savedUsages.Enabled = true;
                    savedUsages.DataSource = savedUsagesFullText;
                    savedUsages.DataValueField = "Key";
                    savedUsages.DataTextField = "Value";
                    savedUsages.DataBind();
                    InsertDropdownPromptText(savedUsages);
                    favoriteUseContainer.Attributes["class"] = "PD_10";
                }
                else
                {
                    savedUsages.Enabled = false;
                    favoriteUseContainer.Attributes["class"] = "PD_10 disabled";
                }
            }
            get
            {
                if (savedUsagesFullText == null)
                {
                    pricingPresenter.GetAllSavedUsages();
                }
                return savedUsagesFullText;
            }
        }

        public List<UseCategory> UseCategories
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

        public Guid SelectedUseCategoryUid
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

        public List<UseType> UseTypes
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

        public Guid SelectedUseTypeUid
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

        public UseType UseTypeWithAttributes
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

        public string UseTypeHelpText
        {
            get { throw new NotImplementedException(); }
        }

        public string UseCategoryLicenseDetails
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LicenseStartDate
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

        public string UseTypeLicenseDetails
        {
            get { throw new NotImplementedException(); }
        }

        public string AttributeLicenseDetails
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

        public string AllLicenseDetails
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

        public bool CustomPricedExpired { get; set; }

        public string CustomPriceExpiredDelay { get; set; }

        public bool EmptyUsage { get; set; }

        public bool ShowPriceStatusMessage
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

        public void DisplayLicenseDetail()
        {
        }

        private void InsertDropdownPromptText(Corbis.Web.UI.Controls.DropDownList dd)
        {
            if (!StringHelper.IsNullOrTrimEmpty(dd.PromptText))
            {
                if (dd.Items.Count == 0 || dd.Items[0].Text != dd.PromptText)
                {
                    dd.Items.Insert(0, new ListItem(dd.PromptText, String.Empty));
                }
            }
        }
        #endregion

        #region IProject Members

        public string Name
        {
            get
            {
                return this.projectField.Text;
            }
            set
            {
                this.projectField.Text = value;
            }
        }

        public string JobNumber
        {
            get
            {
                return this.jobField.Text;
            }
            set
            {
                this.jobField.Text = value;
            }
        }

        public string PONumber
        {
            get
            {
                return this.poField.Text;
            }
            set
            {
                this.poField.Text = value;
            }
        }

        public string RequiredText
        {
            get
            {
                return StringHelper.EncodeToJsString(Resources.Resource.Required);
            }
        }
        public string Licensee
        {
            get
            {
                return this.licenseeField.Text;
            }
            set
            {
                this.licenseeField.Text = value;
            }
        }

        #endregion
        #region IDelivery Members

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
                return String.Empty;
            }
            set
            {
            }
        }
        public string DeliverySubject
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

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
                // TODO : Call membership to get the default email for the user.
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

        private Corbis.WebOrders.Contracts.V1.PaymentMethod method;
        public Corbis.WebOrders.Contracts.V1.PaymentMethod Method
        {
            get
            {
                foreach (Corbis.WebOrders.Contracts.V1.PaymentMethod method in Enum.GetValues(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod)))
                {
                    if (this.hidPaymentMethod.Value.Contains(method.ToString()))
                        return method;
                }
                return Corbis.WebOrders.Contracts.V1.PaymentMethod.Unknown;
            }
            set
            {
                method = value;
            }
        }
        //private int corporateAccountNumber = -1;
        public int SelectedCorporateAccountID
        {
            get
            {
                int corporateAccountId = 0;
                int.TryParse(corporateID.Value, out corporateAccountId);
                return corporateAccountId;
            }
            set
            {
                corporateID.Value = value.ToString();
            }
        }
        public string CreditCardUid
        {
            get
            {
                return creditCardUID.Value;
            }
            set
            {
                //selectedCreditUid.Value = value;
            }
        }
        public string CreditCardValidationCode
        {
            get
            {
                return creditCardValidationCode.Text;
            }
        }
        public string PromoCode
        {
            get
            {
                return promotionCode.Text;
            }
            set
            {
                promotionCode.Text = value;
            }
        }





        //private CreditCard creditCard;
        public CreditCard CreditCard
        {
            get
            {
                return ViewState["creditCard"] as CreditCard;
            }
            set
            {

                ViewState["creditCard"] = value;
            }
        }
        private List<CreditCard> creditCards;
        public List<CreditCard> CreditCards
        {
            get
            {
                // Lazy Load
                if (creditCards == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    creditCards = stateItems.GetStateItemValue<List<CreditCard>>(
                        ExpressCheckoutKeys.Name,
                        ExpressCheckoutKeys.CreditCards,
                        StateItemStore.AspSession);
                }
                return creditCards;
            }
            set
            {
                creditCards = value;
                paymentMethodControl.CreditCards = value;
                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<List<CreditCard>> creditsStateItem =
                    new StateItem<List<CreditCard>>(
                    ExpressCheckoutKeys.Name,
                    ExpressCheckoutKeys.CreditCards,
                    creditCards,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (creditCards != null)
                {
                    stateItems.SetStateItem<List<CreditCard>>(creditsStateItem);
                    //LoadSubmitStepData(value);
                }
                else
                {
                    stateItems.DeleteStateItem<List<CreditCard>>(creditsStateItem);
                }
            }


        }

        //private List<ContentItem> cardTypes;
        public List<ContentItem> CardTypes
        {
            get
            {
                return paymentMethodControl.CardTypes;
            }
            set
            {
                paymentMethodControl.CardTypes = value;
            }
        }

        public List<Company> CorporateAccounts
        {
            get
            {
                return this.paymentMethodControl.CorporateAccounts;
            }
            set
            {
                //ViewState["CorporateAccounts"] = value;
                //int defaultIndex = 0;
                //if (value.Count > 1)
                //{
                //    corporateAccountsList.DataSource = value;
                //    corporateAccountsList.DataTextField = "Name";
                //    corporateAccountsList.DataValueField = "companyId";
                //    corporateAccountsList.DataBind();
                //    //setup the prefered item as selected
                //    for (int i = 0; i < value.Count; i++)
                //    {
                //        if (value[i].IsDefault)
                //            defaultIndex = i;
                //    }
                //    corporateAccountMultiple.Visible = true;
                //    corporateAccountSingle.Visible = false;
                //}
                //else
                //{
                //    corporateAccountText.Text = value[0].Name;
                //    corporateAccountID.Value = value[0].CompanyId.ToString();
                //    corporateAccountMultiple.Visible = false;
                //    corporateAccountSingle.Visible = true;
                //}
                //corporateAccountsList.SelectedIndex = defaultIndex;
                ////now deal with the unapproved case
                //CorporateAccountCreditHandling(value[defaultIndex].IsCreditApproved);
                this.paymentMethodControl.CorporateAccounts = value;
            }
        }

        //private void CorporateAccountCreditHandling(bool isApproved)
        //{

        //    if (isApproved)
        //    {
        //        corporateAccountPanel.CssClass = corporateAccountPanel.CssClass.Replace("ErrorRow", "");
        //        corporateAccountMultipleErr.Visible = false;
        //        corporateAccountSingleErr.Visible = false;
        //        corporateAccountErrorBlock.Visible = false;
        //        btnPaymentNext.Enabled = true;
        //    }
        //    else
        //    {
        //        if (!corporateAccountPanel.CssClass.Contains("ErrorRow"))
        //            corporateAccountPanel.CssClass += " ErrorRow";
        //        corporateAccountErrorBlock.Visible = true;
        //        corporateAccountSingleErr.Visible = corporateAccountSingle.Visible;
        //        corporateAccountMultipleErr.Visible = corporateAccountMultiple.Visible;
        //        btnPaymentNext.Enabled = false;
        //    }
        //    IspaymentApproved = isApproved;


        //}
        public string CorporateAccountText
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                //corporateAccountText.Text = value;
            }
        }

        private bool displyCorporateAccountIcon;
        public bool DisplyCorporateAccountIcon
        {
            get
            {
                return displyCorporateAccountIcon;
            }
            set
            {
                displyCorporateAccountIcon = value;
            }
        }
        private bool displySavedCreditCardIcon;
        public bool DisplySavedCreditCardIcon
        {
            get
            {
                return displySavedCreditCardIcon;
            }
            set { displySavedCreditCardIcon = value; }
        }
        private bool displyNewCreditCardIcon;
        public bool DisplyNewCreditCardIcon
        {
            get
            {
                return displyNewCreditCardIcon;
            }
            set
            {
                displyNewCreditCardIcon = value;
            }
        }

        public bool DisplayPaymentChosen_Error
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                //paymentChosen_Error.Visible = value;
            }
        }

        private bool displayCreditReviewError;
        public bool DisplayCreditReviewError
        {
            set
            {
                //creditReview.Visible = value;
                displayCreditReviewError = value;
            }
        }

        public bool DisplayValidatePromoError
        {
            set
            {
                //paymentPromoCodeBlock.Style.Add("background-color", value ? "#ffffcc" : "transparent");
                //promoValidator.Visible = value;
            }
        }

        private bool setCorporateAsDefault;
        public bool SetCorporateAsDefault
        {
            set
            {
                setCorporateAsDefault = value;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", "paymentTabsIndex = 0;", true);
            }
        }
        private bool setCreditAsDefult;
        public bool SetCreditAsDefult
        {
            set
            {
                setCreditAsDefult = value;
                int index = 1;
                if (!DisplyCorporateAccountIcon)
                    index = 0;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", string.Format("paymentTabsIndex = {0};", index), true);
            }
        }

        private bool setCreditNewAsDefault;
        public bool SetCreditNewAsDefult
        {
            set
            {
                setCreditNewAsDefault = value;
                int index = 0;
                if (DisplyCorporateAccountIcon)
                    index++;
                if (this.DisplySavedCreditCardIcon)
                    index++;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", string.Format("paymentTabsIndex = {0};", index), true);
            }
        }
        private int defaultPayment { get; set; }
        public int DefaultPayment
        {
            get
            {
                return defaultPayment;
            }
            set
            {
                defaultPayment = value;
            }

        }
        private Corbis.Membership.Contracts.V1.PaymentMethod defaultPaymentMethod;
        public Corbis.Membership.Contracts.V1.PaymentMethod DefulatPaymentMethod
        {
            get
            {
                return defaultPaymentMethod;
            }
            set
            {
                defaultPaymentMethod = value;
            }
        }
        //private void setErrorMessages()
        //{
        //creditReview.Visible = false;
        //makeAnotherSelection.Visible = false;
        //contactCorbis.Visible = false;
        //forAssistance2.Visible = false;
        //promoValidator.Visible = false;
        //paymentChosen_Error.Visible = false;
        //}

        #endregion

        #region ICheckoutView Members


        public bool IsCoffFlow
        {
            get
            {
                return WorkflowHelper.IsCOFFWorkflow(Request);
            }
        }

        #endregion

        #endregion

        #region error messages

        public string LicenseDetailsErrorMessage
        {
            get { return StringHelper.EncodeToJsString(GetLocalResourceString("licenseDetailsErrorMessage")); }
        }

        public string PricingCalculationErrorMessage
        {
            get { return StringHelper.EncodeToJsString(GetLocalResourceString("pricingCalculationErrorMessage")); }
        }

        #endregion
    }

}
