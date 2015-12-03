using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Utilities;
using WebOrderContracts = Corbis.WebOrders.Contracts.V1;
using System.Collections.Generic;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Framework.Globalization;
namespace Corbis.Web.UI.Checkout
{
    public partial class StepPayment : Corbis.Web.UI.CorbisBaseUserControl, IPayment
    {
        //protected HtmlImage iconhelp;
        private bool paymentApproved;          
        public bool IspaymentApproved
        {
            get
            {
               return  paymentApproved;
            }
            set
            {
                paymentApproved = value;                
            }
        }
        public string CreditType
        {
            get
            {

                if (String.IsNullOrEmpty((string)ViewState["CreditType"]))
                {
                    foreach (CreditCardType method in Enum.GetValues(typeof(CreditCardType)))
                    {
                        if (this.selectedPayment.Value.Contains(method.ToString()))
                            return method.ToString();
                    }
                    return CreditCardType.Unknown.ToString();
                }
                return (string)(ViewState["CreditType"]);
                
            }
           
            set
            {
                ViewState["CreditType"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string rel = string.Empty;
            rel =
                            string.Format(
                                @"
                                {0}
                                <br />
                                <div id='ccVerificationHoverLeft'>
                                <h4>{1}</h4>
                                <img alt='' id='VisaBack' src='/Images/{2}/MCVisaVerification.png' />
                                </div>
                                <div id='ccVerificationHoverRight'>
                                <h4>{3}</h4>
                                <img alt='' id='AmexBack' src='/Images/{2}/AmexVerification.png' />
                                </div>
                                <br clear='all' />
                            ", 
                            (string)GetLocalResourceObject("ccVerificationExplanationLabel"),
                            (string)GetLocalResourceObject("PopupMCVisa"),
                            Language.CurrentLanguage.LanguageCode, 
                            (string)GetLocalResourceObject("PopupAmex")
                           );
                    
            this.iconhelp.Attributes["rel"] = rel;
            this.iconhelp.Attributes["title"] = (string)GetLocalResourceObject("PopupTitle");
            if (!IsPostBack)
            {
                setErrorMessages();
            }
            string scriptManagerId = Request.Form[Page.Master.FindControl("scriptManager").UniqueID] ?? "";
            if (scriptManagerId.EndsWith("updateCreditDisplay"))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "focusCCV", "focusOnCCV(); CorbisUI.QueueManager.CheckoutMacros.runItem('ccvReattach');", true);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            //List<CreditCard> cards = Session["credits"] as List<CreditCard>;
            //if (cards != null)   
            //   CreditCards = cards;
            base.OnInit(e);
            //corporateAccountsList.SelectedIndexChanged += new EventHandler(corporateAccountsList_SelectedIndexChanged);
        }
        protected void validatePromoLB_Click(object sender, EventArgs e)
        {
            btnPaymentNext.Enabled = true;
            bool promoValid = promoField.Text.Trim() == "" || 
                            ((MainCheckout)this.Page).CheckoutPresenter.ValidatePromoCode(promoField.Text);

            switch (Method)
            {
                case WebOrderContracts.PaymentMethod.CreditCard:
                    IspaymentApproved = true;
                    break;
                case WebOrderContracts.PaymentMethod.CorporateAccount:
                    bool isApproved = false;
                    List<Company> accounts = CorporateAccounts;
                    Company selectedAccount = accounts.Find(delegate(Company account)
                    {
                        return account.CompanyId == SelectedCorporateAccountID;
                    });
                    if (selectedAccount != null)
                    {
                        isApproved = selectedAccount.IsCreditApproved;
                    }
                    this.CorporateAccountCreditHandling(isApproved);
                    break;
                default:
                    Response.Redirect(SiteUrls.OrderSubmissionError);
                    break;
            }
            if (promoValid && IspaymentApproved)
            {
                try
                {
                    if (Method != WebOrderContracts.PaymentMethod.CorporateAccount)
                    {
                        //string[] msgs = null;
                        Page.Validate("PaymentStepCVVGroupSaved");
                        //msgs = paymentValidationSummarySaved.GetErrorMessages();
                        selectedCardUpdatePanel.Update();
                        if (CreditCardUid != Guid.Empty.ToString())
                        {
                            GoToPreview();
                        }
                    }
                    else
                    {
                        GoToPreview();
                    }
                    
                }
                catch (Exception )
                {
                    Response.Redirect(SiteUrls.OrderSubmissionError);
                    return;
                }           
            }
            DisplayValidatePromoError = !promoValid;            
            //CreditType = string.Empty;
            //CreditCardUid = Guid.Empty.ToString();
            
        }

        protected void GoToPreview()
        {
            ((MainCheckout)Page).CheckoutPresenter.PreviewOrder();
            string startupScript = "UpdatePanelRefresh({tabIndex:3});checkoutTabs.show(3, true);submitBtnHandler();UpdateBasedOnCurrentTabIndex({tabIndex:3});";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "promoValidate", startupScript, true);
            UpdatePanel submitPanel = (UpdatePanel)HtmlHelper.FindControlIterative(Parent, "stepSubmitUpdatePanel");
            submitPanel.Update();
        }

        protected void corporateAccountsList_SelectedIndexChanged(object sender, EventArgs e)
        {
           // ((MainCheckout)this.Page).CheckoutPresenter.GetCreditApproved();
            //Set corporate Account ID
            Debug.WriteLine(corporateAccountsList.SelectedIndex.ToString());
            bool isApproved = CorporateAccounts[corporateAccountsList.SelectedIndex].IsCreditApproved;
            this.CorporateAccountCreditHandling(isApproved);
            string startupScript = "CorbisUI.QueueManager.CheckoutMacros.runItem('checkForCorporateErrors');";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "checkCorpErrors", startupScript, true);

        }

        //protected void creditCardList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Debug.WriteLine(creditCardList.SelectedIndex.ToString());
        //    //this.cardCtrl.ImportCreditCard(CreditCards[creditCardList.SelectedIndex]);
        //}

        // TODO: Replace these values with page control values
        #region IPayment Members


        private Corbis.WebOrders.Contracts.V1.PaymentMethod method;
        public Corbis.WebOrders.Contracts.V1.PaymentMethod Method
        {
            get
            {
                foreach (Corbis.WebOrders.Contracts.V1.PaymentMethod method in Enum.GetValues(typeof(Corbis.WebOrders.Contracts.V1.PaymentMethod)))
                {
                    if (this.selectedPayment.Value.Contains(method.ToString()))
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
                string result;
                int id = -1;
                if (!string.IsNullOrEmpty(corporateAccountID.Value))
                {
                    result = corporateAccountID.Value;
                }
                else
                {
                    result = corporateAccountsList.SelectedValue;
                }
                int.TryParse(result, out id);
                return id;
            }
            set
            {
             //   corporateAccountNumber = value;
            }
        }      
        public string CreditCardUid
        {
            get
            {
                return selectedCreditUid.Value;
            }
            set
            {
                Debug.WriteLine("creditcardUid=" + value);
                selectedCreditUid.Value = value;
            }
        }                
        public string CreditCardValidationCode
        {
            get
            {
                    return ccVerificationCodeTextBox.Text;
            }         
        }        
        public string PromoCode
        {
            get
            {
                return promoField.Text;
            }
            set
            {
                promoField.Text = value;
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
            selectCardBlock.Visible = true;
            selectCardBlockErr.Visible = false;
            cardExpiredErr.Visible = value.IsExpired;
            selectedCreditUid.Value = value.CreditCardUid.ToString();
            this.creditDisplyText.Text = CorbisBasePage.GetResourceString("Resource", value.CreditCardTypeCode + "_card") + value.CardNumberViewable;
            selectedCardUpdatePanel.Update();
        }
       }
       //private List<CreditCard> creditCards; 
       public List<CreditCard> CreditCards 
       {
         get
         {
             return null;
         }     
        set
        {
            
        }
       }
       //private List<ContentItem> cardTypes;
       public List<ContentItem> CardTypes
       {
           get
           {
               return null;
           }
           set
           {
               
           }
       }

       public List<Company> CorporateAccounts
       {
           get
           {
               return ViewState["CorporateAccounts"] as List<Company>;
           }
           set
           {
               ViewState["CorporateAccounts"] = value;
               int defaultIndex = 0;
               if (value.Count > 1)
               {
                   corporateAccountsList.DataSource = value;
                   corporateAccountsList.DataTextField = "Name";
                   corporateAccountsList.DataValueField = "companyId";
                   corporateAccountsList.DataBind();
                   //setup the prefered item as selected
                   for (int i = 0; i < value.Count; i++ )
                   {
                       if (value[i].IsDefault)
                           defaultIndex = i;
                   }
                   corporateAccountMultiple.Visible = true;
                   corporateAccountSingle.Visible = false;
               }
               else
               {
                   corporateAccountText.Text = value[0].Name;
                   corporateAccountID.Value = value[0].CompanyId.ToString();
                   corporateAccountMultiple.Visible = false;
                   corporateAccountSingle.Visible = true;
               }
               corporateAccountsList.SelectedIndex = defaultIndex;
               //now deal with the unapproved case
               CorporateAccountCreditHandling(value[defaultIndex].IsCreditApproved);
           }
       }

        private void CorporateAccountCreditHandling(bool isApproved)
        {

            if (isApproved)
            {
                corporateAccountPanel.CssClass = corporateAccountPanel.CssClass.Replace("ErrorRow", "");
                corporateAccountMultipleErr.Visible = false;
                corporateAccountSingleErr.Visible = false;
                corporateAccountErrorBlock.Visible = false;
                btnPaymentNext.Enabled = true;
            }
            else
            {
                if (!corporateAccountPanel.CssClass.Contains("ErrorRow"))
                    corporateAccountPanel.CssClass += " ErrorRow";
                corporateAccountErrorBlock.Visible = true;
                corporateAccountSingleErr.Visible = corporateAccountSingle.Visible;
                corporateAccountMultipleErr.Visible = corporateAccountMultiple.Visible;
                //btnPaymentNext.Enabled = false;
            }
            IspaymentApproved = isApproved;
           
            
        }
       public string CorporateAccountText
       {
           get
           {
               return corporateAccountText.Text;
           }
           set
           {
               corporateAccountText.Text = value;
           }
       }

       public bool DisplyCorporateAccountIcon
       {
           get
           {
              return paymentOptionsCorporateAccount.Visible;
           }
           set
           {
               paymentOptionsCorporateAccount.Visible = value;
               corporateDisplay.Visible = value;

           }           
       }

       //public bool DisplyCorporateAccountList
       //{
       //    get
       //    {
       //        return corporateAccountMultiple.Visible;
       //    }
       //    set
       //    {
       //        corporateAccountMultiple.Visible = value;

       //    }
       //}

       //public bool DisplyCorporateAccountSingle
       //{
       //    get
       //    {  
       //        return corporateAccountSingle.Visible;
       //    }
       //    set
       //    {
       //        corporateAccountSingle.Visible = value;

       //    }
       //}
       public bool DisplySavedCreditCardIcon
       {
           get
           {
               return paymentOptionsSavedCreditCard.Visible;
           }
           set
           {
               paymentOptionsSavedCreditCard.Visible = value;
               if (!value)
               {
                   creditDisplay.Style.Add("display", "none");
               }
           }
       }
       public bool DisplyNewCreditCardIcon
       {
           get
           {
               return paymentOptionsNewCreditCard.Visible;
           }
           set
           {
               paymentOptionsNewCreditCard.Visible = value;
           }
       }

      public  bool DisplayPaymentChosen_Error
      {
          get
          {
             return paymentChosen_Error.Visible;
          }
          set
          {
              paymentChosen_Error.Visible = value;
          }
      }
      

      public bool DisplayCreditReviewError
      {
          set
          {
              creditReview.Visible = value;
          }
      }

       public bool DisplayValidatePromoError
       {
           set
           {
               paymentPromoCodeBlock.Style.Add("background-color", value ? "#ffffcc" : "transparent");
               //string startupScript = string.Format("var myScroller = new Fx.Scroll($('{0}'));$('{0}').toBottom();", this.Page.FindControl("paymentPaneLayout").ClientID);
               //ScriptManager.RegisterStartupScript(Page, this.GetType(), "promoValidate", startupScript, true);
               promoValidator.Visible = value;
               
           }
       }

       //public bool DisplayCorporateSection
       //{
       //    get
       //    {
       //        return corporateDisplay.Visible;
       //    }
       //    set
       //    {
       //        paymentOptionsCorporate.Visible = value;
       //        corporateDisplay.Visible = value;

       //        //if (value)
       //        //{
       //        //    corporateDisplay.Attributes["style"] = "display:block";
       //        //}
       //        //else
       //        //{
       //        //    corporateDisplay.Attributes["style"] = "display:none";
       //        //}
               
       //    }

       //}
      
       public bool SetCorporateAsDefault
       {
           set
           {
               ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", "paymentTabsIndex = 0;", true);
               //paymentOptionsCorporate.Attributes["class"] += " selected";
               //corporateDisplay.Attributes["style"] = value? "display:block" : "display:none";
           }
       }
       public bool SetCreditAsDefult
       {
           set
           {
               int index = 1;
               if (!DisplyCorporateAccountIcon)
                   index = 0; 
               ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", string.Format("paymentTabsIndex = {0};", index), true);
               //this.paymentOptionsSavedCredit.Attributes["style"] = value ? "display:block" : "display:none";
               //creditDisplay.Attributes["style"] = value? "display:block" : "display:none";
               //paymentOptionsSavedCredit.Attributes.Add("class", value);
           }
       }

       public bool SetCreditNewAsDefult
       {
           set
           {
               int index = 0;
               if (DisplyCorporateAccountIcon)
                   index ++;
               if (this.DisplySavedCreditCardIcon)
                   index++;
               ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPaymentDefault", string.Format("paymentTabsIndex = {0};", index), true);
               //this.paymentOptionsNewCredit.Attributes["style"] = value ? "display:block" : "display:none";
               //creditAddDisplay.Attributes["style"] = value? "display:block" : "display:none";
               //paymentOptionsNewCredit.Attributes.Add("class", value);
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
               //if (value == 0) Method = PaymentMethod.CorporateAccount;
               //else
                  
               defaultPayment = value;
           }

       }
       private PaymentMethod defaultPaymentMethod;
       public PaymentMethod DefulatPaymentMethod
       {
           get
           {
               return defaultPaymentMethod;
           }
           set
           {
               defaultPaymentMethod = value;
               if (value == PaymentMethod.CorporateAccount)
               {
                   creditDisplay.Style.Add("display", "none");
               }
               else
               {
                   corporateDisplay.Style.Add("display", "none");
               }
           }
       }     
       private void setErrorMessages()
       {
           creditReview.Visible = false;
           makeAnotherSelection.Visible = false;
           contactCorbis.Visible = false;
           forAssistance2.Visible = false;
           promoValidator.Visible = false;
           paymentChosen_Error.Visible = false;
       }

       protected void cardSelector_CreditCardSelected(object sender, EventArgs e)
       {
           this.CreditCardUid = this.selectedCreditUid.Value;
           try
           {
               if (!string.IsNullOrEmpty(CreditCardUid))
               {
                   AccountsPresenter accountsPresenter = new AccountsPresenter(this);
                   CreditCard cc = accountsPresenter.GetCreditCard(this.CreditCardUid);
                   this.CreditCard = cc;
                   //((MainCheckout)Page).CheckoutPresenter.GetPaymentAccounts();
                   //paymentMethodsUpdatePanel.Update();
                   if (!this.IsPostBack)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showCC", "CorbisUI.Checkout.setPaymentTabs();", true);
               }
               else
               {
                   this.CreditCardUid = Guid.Empty.ToString();
               }
           }
           catch (Exception)
           {
               // not sure what to do here..
           }
       }
        

        protected void btnPaymentNext_OnClick(object sender, EventArgs e)
        {
            //((MainCheckout) Page).CheckoutPresenter.MikeTest();
            //UpdatePanel panel = (UpdatePanel) Parent.FindControl("TabContainersUpdatePanel");
            //panel.Update();
            //Page.Validate(vgsSaved.ValidationGroup
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "PaymentNext", "UpdatePanelRefresh({tabIndex:3});checkoutTabs.show(3,true);", true);
       
        }

        #endregion
    }
}

internal enum PaymentError
{
    noErr,
    noIdeaErr,
    cardExpiredErr
}
