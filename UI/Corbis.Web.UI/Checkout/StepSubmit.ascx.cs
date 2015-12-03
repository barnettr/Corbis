using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.Presenters.CustomerService;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Checkout
{
    public partial class StepSubmit : Corbis.Web.UI.CorbisBaseUserControl, ISubmit

    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                String restrictionsLink = "ViewOrderRestrictions(); return false;";
                restrictionsIveReadText.Text = String.Format((string)GetLocalResourceObject("RestrictionsIveRead.Text"), restrictionsLink);

                LegalPresenter legalPresenter = new LegalPresenter();
                //eulaLink.HRef = legalPresenter.GetLicenseURL();
                String eulaLink = legalPresenter.GetLicenseURL();
                termsIveReadText.Text = String.Format((string)GetLocalResourceObject("TermsIveRead.Text"), OpenPopup(eulaLink));
                
                CustomerServicePresenter customerService = new CustomerServicePresenter();
                standByMessage3.Text = standByMessage3.Text.Replace("{0}", customerService.GetRepresentativeOfficePhone());

                btnSubmitNext.ToolTip = GetLocalResourceObject("submitToolTip.Text").ToString();
            }

        }

        private string OpenPopup(string eulaLink)
        {
            return string.Format("javascript:NewWindow('{0}',700,800,'True')", eulaLink);
        }

        #region ISubmit Members

        public string CorporateAccountName
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

        public string CreditCardType
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

        public string CreditCardNumberViewable
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

        public string CreditCardExirationMonth
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

        public string CreditCardExpirationYear
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

        public string CreditCardCardHolderName
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

        public string PromoDiscount
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

        public string ShippingCost
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

        public string SubTotal
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

        public string TaxOrVat
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

        public string KsaTax
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

        public string TotalCost
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                totalNumber.Text = value;
            }
        }

        #endregion

        #region IProject Members

        public string Name
        {
            get
            {
                return this.projectBlock.ProjectName;
            }
            set
            {
                this.projectBlock.ProjectName = value;
            }
        }

        public string JobNumber
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                this.projectBlock.JobNumber = value;
            }
        }

        public string PONumber
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                this.projectBlock.PoNumber = value;
            }
        }

        public string Licensee
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                this.projectBlock.Licensee = value;
            }
        }
        #endregion
        public DeliveryMethod OrderDelivery
        {
            set
            {
                deliveryBlock.OrderDelivery = value;
            }
        }
        public Corbis.WebOrders.Contracts.V1.PaymentMethod OrderPayment
        {
            set
            {
                paymentBlock.OrderPayment = value;
            }
        }
        public MemberAddress BillingAddress 
        { 
            set
            {
                this.paymentBlock.BillingAddress = value;
            } 
        }
        //public string Restrictions
        //{
        //    set
        //    {
        //        this.restrictionsList.OnClientClick = string.Format("ViewOrderRestrictions('{0}'); return false;", value);
        //    }
        //}
        public CreditCard CreditCardInformation
        {
            set
            {
                this.paymentBlock.CreditCardInformation = value;
            }
        }
        public string ShippingPriority
        {
            set
            {
                
            }
        }
        public string DeliveryEmails
        {
            set
            {
                this.deliveryBlock.DeliveryEmails = value;
                this.notificationEmails.Text = value;
            }
        }

        public void LoadSubmitStepData(OrderPreviewDetails data)
        {
            this.Name = data.Project.Name;
            this.JobNumber = data.Project.JobNumber;
            this.PONumber = data.Project.PONumber;
            this.Licensee = data.Project.Licensee;

            //delivery
//#if DEBUG
//            if (data.ShippingAddress == null)
//            {
//                data.ShippingAddress = new MemberAddress();
//                data.ShippingAddress.Name = "rajul' fault";
//                data.ShippingAddress.Address1 = "sample addr1";
//                data.ShippingAddress.City = "my city";
//                data.ShippingAddress.PostalCode = "12345";
//                data.ShippingAddress.RegionCode = "RC";
//                data.ShippingAddress.CountryCode = "usa";
//            }
//#endif     
            if (data.Delivery.NonRfcdMethod != DeliveryMethod.Unknown)
            {
                OrderDelivery = data.Delivery.NonRfcdMethod;
                //ShippingAddress = data.ShippingAddress;
            }
            else  
            {
                OrderDelivery=data.Delivery.RfcdMethod;
            
            }
            //ShippingPriority = data.Delivery.ShippingPriorityCode;
            DeliveryEmails = data.Delivery.ConfirmationEmailAddresses;

            // payment
            OrderPayment = data.Payment.Method;
            BillingAddress = data.BillingAddress;
            //Todo: Nahom, Change Credit Card Information CreditCardType To Enumeration 
            if (data.Payment.Method != Corbis.WebOrders.Contracts.V1.PaymentMethod.CorporateAccount
                && data.CreditCardInformation != null)
            {
               
                data.CreditCardInformation.CreditCardTypeCode = data.CreditCardInformation.CreditCardType;
               
                CreditCardInformation = data.CreditCardInformation;
            }
            else
            {
                this.paymentBlock.CorporateAccountName = data.Payment.CorporateAccountName;
            }
            if (data != null && data.Delivery != null)
            {
                this.deliveryBlock.DeliverySpecialInstructions = data.Delivery.SpecialInstruction;
                this.deliveryBlock.DeliverySubject = data.Delivery.Subject;
            }
        }
        public List<ProductRestriction> AllRestrictions
        {
            set
            {
                multipleRestractions.DataSource = value;
                multipleRestractions.DataBind();
            }
        }
        protected void multipleRestractions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ProductRestriction restriction = e.Item.DataItem as ProductRestriction;
                CenteredImageContainer container = (CenteredImageContainer)e.Item.FindControl("thumbWrap1");
                container.ImgUrl = restriction.Url128.Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=90&im=");
                container.Ratio = restriction.AspectRatio;
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("modelRelease")).Text = restriction.ModelReleaseStatus ;
                ((System.Web.UI.WebControls.Label)e.Item.FindControl("propertyRelease")).Text = restriction.PropertyReleaseStatusText;

                PlaceHolder holder = e.Item.FindControl("allRestrictions") as PlaceHolder;
                foreach(Restriction res in restriction.Restrications)
                {
                    HtmlGenericControl ctrl = new HtmlGenericControl("p");
                    ctrl.InnerText = res.Notice;
                    holder.Controls.Add(ctrl);
                }
            }
        }
        protected void submit_Click(object sender, EventArgs e)
        {
            MainCheckout parent = (MainCheckout) Page;
            OrderConfirmationDetails order;
            try
            {
                order = parent.CheckoutPresenter.CreateOrder();

            }
            catch (Exception)
            {
                Response.Redirect(SiteUrls.OrderSubmissionError, true);
                return;
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
                stateItems.SetStateItem<OrderConfirmationDetails>(
                    new StateItem<OrderConfirmationDetails>(
                        OrderKeys.Name,
                        OrderKeys.Details + "_" + order.OrderUid.ToString().ToLower(),
                        order,
                        StateItemStore.AspSession,
                        StatePersistenceDuration.Session));
                //2. go to orderconfirmation page
                if (WorkflowHelper.IsCOFFWorkflow(Request))
                {
                    Response.Redirect(SiteUrls.CoffOrderComplete, true);
                }
                else
                {
                    Response.Redirect(SiteUrls.OrderComplete, true);
                }
            }
        }
    }
}