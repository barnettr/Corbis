using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using Corbis.Web.Entities;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Membership.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.WebOrders.Contracts.V1;
using WebOrderContracts = Corbis.WebOrders.Contracts.V1;
using ServiceReference=System.Web.UI.ServiceReference;
using Corbis.Web.Utilities.StateManagement;
using System.Web.Security;
using System.Configuration;
using System.Web.Configuration;

namespace Corbis.Web.UI.Checkout
{
    public partial class MainCheckout : CorbisBasePage,ICheckoutView
    {
        private COFFCheckoutPresenter checkoutPresenter;

        protected override void OnInit(EventArgs e)
        {
            //sessionTimeoutRedirect.Click += new EventHandler(Cancel_Click);
            this.RequiresSSL = true;
            base.OnInit(e);
            this.AddScriptToPage(SiteUrls.CheckoutScript, "CheckoutScript");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Checkout, "CheckoutCSS");
            //HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.FormCheck, "FormCheckCSS");
            ScriptManager manager = (System.Web.UI.ScriptManager)Master.FindControl("scriptManager");
            manager.Services.Add(new ServiceReference("~/Checkout/CheckoutService.asmx"));
            checkoutPresenter = new COFFCheckoutPresenter(this);
            //if (Profile.CountryCode == "FR")
            //{
            //    agessa.Visible = true;
            //}
            if (!this.IsPostBack)
            {
                AnalyticsData["events"] = AnalyticsEvents.CheckoutStart;
                AnalyticsData["prop3"] = "standard";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "TabIndex", "checkoutTabsFirstShow=0;", true);
            }
        }
        public CheckoutPresenter Presenter
        {
            get { return checkoutPresenter; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TRUSTeLink.Visible = Profile.CountryCode == UScountryCode;

            if (!IsPostBack)
            {
                TimeSpan ts = ((CorbisMasterBase)this.Master).GetFormsAuthTimeout();

                if (ts != null)
                {
                    string sessionTimeOutJs = "setTimeout(\"CorbisUI.GlobalNav.SessionTimedOut()\"," + ts.TotalMilliseconds+ ");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SessionTimeOut", sessionTimeOutJs, true);
                }
            }
            if (WorkflowHelper.IsCOFFWorkflow(Request))
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                   
                    Response.Redirect(SiteUrls.MyLightBoxesAbsoluteHttpUrl);
                    return;
                }
                if (!this.IsPostBack)
                {
                    try {
                        checkoutPresenter.CheckOutCOFFItems();
                    }catch(Exception ex) {
                        if(ex.Message.Equals(COFFCheckoutPresenter.NO_ITEMS_TO_CHECKOUT))
                        {
                            Response.Redirect(SiteUrls.MyLightBoxesAbsoluteHttpUrl);
                        }
                    }
                }
                return;
            }
            if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
            {
                Debug.WriteLine(ScriptManager.GetCurrent(this).AsyncPostBackSourceElementID.ToString());
                this.CheckoutPresenter.GetShippingAddresses(Guid.Empty);
            }
            if (Request.UrlReferrer != null &&
                (Request.UrlReferrer.LocalPath.EndsWith("Cart.aspx", false, null) || Request.UrlReferrer.LocalPath.EndsWith("MainCheckout.aspx", false, null)
                    || Request.UrlReferrer.AbsoluteUri.EndsWith(Request.Url.PathAndQuery, false, null)
                ) 
                //&&!String.IsNullOrEmpty(Request.Form["__EVENTARGUMENT"])
                )
			{
				//ProcessCheckout();
			}
			else
			{
				//Redirect to cart to start the checkout process.
				Response.Redirect(SiteUrls.Cart);
			    return;
			}
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                //Response.Redirect(SiteUrls.GetAuthenticateRedirectUrl(SiteUrls.Cart));
                Response.Redirect(SiteUrls.Cart);
                return;
            }


            if (!this.IsPostBack)
            {
                try
                {
                    checkoutPresenter.LoadCheckoutData();
                }
                catch (Exception)
                {
                    Response.Redirect(SiteUrls.Cart);
                }
            }
        }

        protected void SessionTimeOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            if (WorkflowHelper.IsCOFFWorkflow(Request))
            {
                Response.Redirect(SiteUrls.MyLightBoxesAbsoluteHttpUrl);
            }
            else
            {
                Response.Redirect(SiteUrls.CartAbsoluteHttpUrl);
            }
        }

        #region Public Properties

        /// <summary>
        /// Exposes the Preseneter for Child Controls
        /// </summary>
        public CheckoutPresenter CheckoutPresenter
        {
            get { return checkoutPresenter; }
        }


        #region ICheckoutView Members

        public IProject Project
        {
            get { return this.steps.Project; }
        }

        public IDelivery Delivery
        {
            get { return this.steps.Delivery; }
        }

        public IPayment Payment
        {
            get { return this.steps.Payment; }
        }

        public ISubmit Submit
        {
            get { return this.steps.Submit; }
        }
        private ContractType contractType;
        public ContractType ContractType
        {
            get
            {
                // Lazy Load
                if (contractType == ContractType.None)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    contractType = stateItems.GetStateItemValue<ContractType>(
                        "Checkout",
                        "contractType",
                        StateItemStore.AspSession);
                }

                return contractType;
            }
            set
            {
                contractType = value;
                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<ContractType> contractTypeItem =
                    new StateItem<ContractType>(
                    "Checkout",
                    "contractType",
                    contractType,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (contractType != ContractType.None)
                {
                    stateItems.SetStateItem<ContractType>(contractTypeItem);
                }
                else
                {
                    stateItems.DeleteStateItem<ContractType>(contractTypeItem);
                }
            }
        }

        public List<DeliveryMethod> AvailableNonRfcdDeliveryMethods
        {
            set
            {
                steps.Delivery.AvailableNonRfcdDeliveryMethods = value;
            }   
        }

        public string Tax 
        {
            get
            {
                return this.itemSummaryBlock.Tax;
            }
            set
            {
                this.itemSummaryBlock.Tax = value;
            }
        }
        public string TaxLabel
        {
            get
            {
                return this.itemSummaryBlock.TaxLabel;
            }
            set
            {
                this.itemSummaryBlock.TaxLabel = value;
            }     

        }           
            
        public string KsaTax 
       {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                this.itemSummaryBlock.KsaTax = value;
            }
        }
        public string PromotionDiscount
        {         
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                this.itemSummaryBlock.PromotionAmount = value;
            }
        }
        public bool AgessaFlag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                importantSection.Visible = value;
                this.importantBlock.Update();
                agessa.Visible = value;
            }             
        }

        public Guid OrderUid { get; set; }

        #region Products

        private List<Guid>productUids;
        public  List<Guid> ProductUids
        {
            get
            {
                // Lazy Load
                if (productUids == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    productUids = stateItems.GetStateItemValue<List<Guid>>(
                        "Checkout",
                        "CheckoutProductUids",
                        StateItemStore.AspSession);
                }
                return productUids;
            }
            set
            {
                productUids = value;
                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<List<Guid>> productUidsStateItem =
                    new StateItem<List<Guid>>(
                    "Checkout",
                    "CheckoutProductUids",
                    productUids,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (productUids != null)
                {
                    stateItems.SetStateItem<List<Guid>>(productUidsStateItem);
                }
                else
                {
                    stateItems.DeleteStateItem<List<Guid>>(productUidsStateItem);
                }
            }
        }

        public List<CheckoutProduct> CheckoutProducts
        {
            set
            {
                itemSummaryBlock.CheckoutProducts = value;
                itemCount.InnerText = value.Count.ToString();

                //string corbisIds = string.Empty;
                //foreach(CheckoutProduct product in value)
                //{
                //    corbisIds += product.CorbisID + ",";
                //}
                //this.steps.Submit.Restrictions = corbisIds;
            }
        }

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
                        "Checkout",
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
                    "Checkout",
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
      
        public bool HasRFCD
        {
            get
            {
                if (ViewState["HasRFCD"] != null)
                {
                    return (bool)ViewState["HasRFCD"];
                }
                return false;
            }

            set
            {
                ViewState["HasRFCD"] = value;
            }
        }

        #endregion

        #region costs

        public string SubTotal
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                this.itemSummaryBlock.SubTotal = value;
            }
        }

        public string TotalCost
        {
            get
            {
                return itemSummaryBlock.Total;
            }
            set
            {
                itemSummaryBlock.Total = value;
                totalCost.InnerText = value;
                this.Submit.TotalCost = value;
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
        public void PriceUpdate()
        {
            this.itemSummaryBlock.PriceUpdate();
            totalCostUpdatePanel.Update();
        }
        //public void Agessa()
        //{
        //    this.importantBlock.Visible = true;
        //    this.importantBlock.Update();
        //    agessa.Visible = true;
        //}
        public void TaxError()
        {
            this.importantSection.Visible = true;
            this.importantBlock.Update();
            taxError.Visible = true;
        }

        #endregion

        #region Acceptances

        public bool AcceptRestriction
        {
            get
            {
                // TODO: Replace this with checkbox value
                return true;
            }

        }

        public bool AcceptLicenseAgreement
        {
            get
            {
                // TODO: Replace this with checkbox value
                return true;
            }
        }

        #endregion

        #endregion

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
    }
}
