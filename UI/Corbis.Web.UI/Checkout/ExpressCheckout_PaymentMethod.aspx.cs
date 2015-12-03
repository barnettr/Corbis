using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout_PaymentMethod1 : CorbisBasePage, ICheckoutView, IPayment
    {
        private bool hasCorporateAccount;
        private bool isCreditEnable;
        protected ExpressCheckout_PaymentMethod paymentMethodControl;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckoutPresenter checkoutPresenter = new CheckoutPresenter(this);    
            checkoutPresenter.GetPaymentAccounts();
            paymentMethodControl.PopulatepaymentMethod(this.hasCorporateAccount, this.isCreditEnable);
            
        }

        #region ICheckoutView Members

        public System.Collections.Generic.List<Guid> ProductUids
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

        public bool HasRFCD
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

        public System.Collections.Generic.List<Corbis.Web.Entities.CheckoutProduct> CheckoutProducts
        {
            set { throw new NotImplementedException(); }
        }

        public Corbis.WebOrders.Contracts.V1.PreviewAndCreateRequest PreviewRequest
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

        public bool IsCreditEnable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                this.isCreditEnable = value;
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
                this.hasCorporateAccount = value;
            }
        }

        public Corbis.CommonSchema.Contracts.V1.ContractType ContractType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                
            }
        }

        public IProject Project
        {
            get { throw new NotImplementedException(); }
        }

        public IDelivery Delivery
        {
            get { throw new NotImplementedException(); }
        }

        public IPayment Payment
        {
            get {
                return this;
            }
        }

        public ISubmit Submit
        {
            get { throw new NotImplementedException(); }
        }

        public string SubTotal
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

        public string TotalCost
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

        public string Tax
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

        public string TaxLabel
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

        public string KsaTax
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

        public string PromotionDiscount
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

        public bool AgessaFlag
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
            throw new NotImplementedException();
        }

        public void TaxError()
        {
            throw new NotImplementedException();
        }

        public bool IsCoffFlow
        {
            get
            {
                return WorkflowHelper.IsCOFFWorkflow(Request);
            }
        }

        #endregion

        #region IPayment Members

        public Corbis.WebOrders.Contracts.V1.PaymentMethod Method
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                
            }
        }
        private int _selectedCorporateAccountID;

        public int SelectedCorporateAccountID
        {
            get { return this._selectedCorporateAccountID; }
            set { this._selectedCorporateAccountID = value; }
        }

        public string CreditCardUid
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

        public string CreditCardValidationCode
        {
            get { throw new NotImplementedException(); }
        }

        public Corbis.Membership.Contracts.V1.PaymentMethod DefulatPaymentMethod
        {
            get
            {
                return Corbis.Membership.Contracts.V1.PaymentMethod.NotSet;
            }
            set
            {
            }
        }

        public string PromoCode
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

        public Corbis.Membership.Contracts.V1.CreditCard CreditCard
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                
            }
        }

        public System.Collections.Generic.List<Corbis.Web.Entities.ContentItem> CardTypes
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

        public System.Collections.Generic.List<Corbis.Membership.Contracts.V1.CreditCard> CreditCards
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                this.paymentMethodControl.CreditCards = value;
            }
        }

        public System.Collections.Generic.List<Corbis.Membership.Contracts.V1.Company> CorporateAccounts
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                this.paymentMethodControl.CorporateAccounts = value;
            }
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
                
            }
        }

        public bool DisplayCreditReviewError
        {
            set {  }
        }

        public bool DisplayValidatePromoError
        {
            set { }
        }

        public bool SetCorporateAsDefault
        {
            set {  }
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
               
            }
        }

        #endregion

    }

    public class ViewManager
    {
        public static string RenderView(string path)
        {
            return RenderView(path, null);
        }

        public static string RenderView(string path, object data)
        {
            Page pageHolder = new Page();
            HtmlForm form1 = new HtmlForm();
            form1.Attributes.Add("runat", "server");

            UserControl viewControl = (UserControl)pageHolder.LoadControl(path);
            form1.Controls.Add(viewControl);
            pageHolder.Controls.Add(form1);

            if (data != null)
            {
                Type viewControlType = viewControl.GetType();
                FieldInfo field = viewControlType.GetField("Data");

                if (field != null)
                {
                    field.SetValue(viewControl, data);
                }
                else
                {
                    throw new Exception("View file: " + path + " does not have a public Data property");
                }
            }

            pageHolder.Controls.Add(viewControl);

            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(pageHolder, output, false);

            return output.ToString();
        }

    }

}
