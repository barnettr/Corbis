using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using PaymentMethod = Corbis.WebOrders.Contracts.V1.PaymentMethod;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;

namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout_PaymentMethod : CorbisBaseUserControl,IPayment
    {

       // private CheckoutPresenter checkoutPresenter;

        protected void Page_Load(object sender, EventArgs e)
        {       
        

                        
        }
        public Corbis.Web.UI.Controls.AjaxDropDownList SelectPaymentMethod
        {
            
            get
            {
                return selectPaymentMethod;
            }
        }

        public void PopulatepaymentMethod(bool hasCorporateAccount, bool isCreditEnabled)
        {
            
             bool isComanyDefault = false;
             bool IsFirst = true;
            if (hasCorporateAccount)
            {
                SelectPaymentMethod.Items.Add(new ListItem("----------------------", "none"));
                foreach (Company company in CorporateAccounts)
                {
                    SelectPaymentMethod.Items.Add(new ListItem(company.Name, company.CompanyId.ToString()));
                    SelectPaymentMethod.Items[SelectPaymentMethod.Items.Count-1].Attributes.Add("type", Corbis.WebOrders.Contracts.V1.PaymentMethod.CorporateAccount.ToString());
                    SelectPaymentMethod.Items[SelectPaymentMethod.Items.Count-1].Attributes.Add("CreditApproved", company.IsCreditApproved.ToString());
                    if (IsFirst)
                    {
                        IsFirst = false;
                        SelectPaymentMethod.SelectedIndex = SelectPaymentMethod.Items.Count-1;

                    }
                    if (company.IsDefault)
                    {
                        SelectPaymentMethod.SelectedIndex = SelectPaymentMethod.Items.Count-1;
                        isComanyDefault = true;

                    }
                }
                
            }
            if (CreditCards != null)
            {
                List<CreditCard> creditCards = CreditCards;
                bool IsCreditDefault = false;
              
                var results = from card in creditCards
                              select new { CardGuid = card.CreditCardUid, IsDefault = card.IsDefault, ExpirationDate = card.ExpirationDate, DisplayText = CorbisBasePage.GetResourceString("Resource", card.CreditCardTypeCode + "_card") + card.CardNumberViewable };
                SelectPaymentMethod.Items.Add(new ListItem("----------------------", "none"));
                foreach (var result in results)
                {                 
                   
                    bool cardExpired = ExpiredCard(result.ExpirationDate);
                    SelectPaymentMethod.Items.Add(new ListItem(result.DisplayText.ToString(), result.CardGuid.ToString()));
                    SelectPaymentMethod.Items[SelectPaymentMethod.Items.Count-1].Attributes.Add("type", Corbis.WebOrders.Contracts.V1.PaymentMethod.CreditCard.ToString());
                    SelectPaymentMethod.Items[SelectPaymentMethod.Items.Count-1].Attributes.Add("cardExpired", cardExpired.ToString());
                    if (IsFirst && !hasCorporateAccount)
                    {
                        IsFirst = false;
                        SelectPaymentMethod.SelectedIndex = SelectPaymentMethod.Items.Count-1;

                    }
                    if (!hasCorporateAccount && result.IsDefault)
                    {
                        SelectPaymentMethod.SelectedIndex = SelectPaymentMethod.Items.Count-1;
                        IsCreditDefault = true;
                    }
                }


            SelectPaymentMethod.Items.Add(new ListItem("----------------------", "none"));

                if (SelectPaymentMethod.Items.Count > 0 && !IsCreditDefault && !isComanyDefault)
                {
                    SelectPaymentMethod.SelectedIndex = 0;
                }

            }

            if (isCreditEnabled)
            {
                SelectPaymentMethod.Items.Add(new ListItem(GetLocalResourceObject("AddNewCard").ToString(), "Add New"));

                if (CreditCards == null)
                {
                    SelectPaymentMethod.SelectedIndex = 0;
                }
            }



        }
        public bool ExpiredCard(string expirationDate)
        {           
            if(!string.IsNullOrEmpty(expirationDate))
            {
                char[] chSplit={'/'};                                        
                string[]data=expirationDate.Split(chSplit);
                int yy = DateTime.Now.Year;
                int mm = DateTime.Now.Month;
                Int32.TryParse(data[1],out yy);
                Int32.TryParse(data[0],out mm);
              return DateTime.Now.Year > yy || (DateTime.Now.Year == yy && DateTime.Now.Month > mm);             
            }
            return false;
            }        

        #region IPayment Members
        private PaymentMethod method;
        public PaymentMethod Method
        {
            get
            {
                return method;
            }
            set
            {
               method=value;
            }
        }

        private int selectedCorporateAccountID=0;
        public int SelectedCorporateAccountID
        {
            get {return selectedCorporateAccountID; }
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
             creditCardUid=value;
            }
        }

        private string creditCardValidation=string.Empty;
        public string CreditCardValidationCode
        {
            get
            {
                return creditCardValidation;
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
               defaultPaymentMethod=value;
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
                promoCode=value;
            }
        }

        private CreditCard creditCard;
        public CreditCard CreditCard
        {
            get
            {
                return creditCard;
            }
            set
            {
                creditCard=value;
            }
        }

        private List<Corbis.Web.Entities.ContentItem> cardtypes;
        public List<Corbis.Web.Entities.ContentItem> CardTypes
        {
            get
            {
               return cardtypes;
            }
            set
            {
                cardtypes=value;
            }
        }
        private List<CreditCard> creditCards;
        public List<CreditCard> CreditCards
        {
            get
            {
              return creditCards;
            }
            set
            {
               creditCards=value;
            }
        }

        private List<Company> corprateAccount;
        public List<Company> CorporateAccounts
        {
            get
            {
                return corprateAccount;
            }
            set
            {
                corprateAccount = value;
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

        private int defaultPayment;
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

        #endregion
    }
}