using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Checkout
{
    /// <summary>
    /// this will be migrated to a common folder and used as a generic control
    /// to replace all other cases across our site
    /// </summary>
    public partial class CreditCardBlock : Corbis.Web.UI.CorbisBaseUserControl, ICreditInformationView, IViewPropertyValidator
    {
        private const string CREDITCARDS = "creditCards";
        private const string VS_TAG_CARDTYPE_DATA = "VS_TAG_CARDTYPE_DATA";
        public string CardType;
        
        //public string ValidationGroup
        //{
            
        //    //get { return vgs.ValidationGroup; }
        //    //set
        //    //{
        //    //    vgs.ValidationGroup = cardNumber.ValidationGroup = expirationDateText.ValidationGroup = cardHolder.ValidationGroup = value;
        //    //}
        //}
        private AccountsPresenter accountsPresenter;
        private void InitializeDateDropdown()
        {
            if (cardMonth.Items.Count > 0)
                return;
            int month = DateTime.Now.Month;
            for (int i = 1; i < 13; i++)
            {
                ListItem li = new ListItem(i.ToString("00"));
                this.cardMonth.Items.Add(li);
                if (i == month)
                    li.Selected = true;
            }
            //load year
            int currentYear = (CreditCardUsageType == CreditCardUsageType.CreateNewCard) ? DateTime.Now.Year : DateTime.Now.Year - 3;
            for (int i = 0; i < 18; i++)
            {
                ListItem li = new ListItem((currentYear + i).ToString());
                this.cardYear.Items.Add(li);
                if (i == 0)
                    li.Selected = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            accountsPresenter = new AccountsPresenter(this);
            InitializeDateDropdown();
            if (CreditCardUsageType == CreditCardUsageType.CreateNewCard)
            {
                CardType = "New";
                accountsPresenter.LoadNewCreditInformation();
            }
            else
            {
                CardType = "Saved";
                accountsPresenter.LoadSavedCreditInformation();
                cardNumber.ValidateControl = false;
                
                ////load month
            }
            
            if (DateHelper.YearMonthPattern[0] == 'y')
            {
                cardYear.Parent.Controls.Remove(cardMonth);
                int yearIndex = cardYear.Parent.Controls.IndexOf(this.cardYear);
                cardYear.Parent.Controls.AddAt(yearIndex + 1, cardMonth);
            }

            vhub.PopupID = this.CardType == "New" ? "AddNewCreditCart" : "UseSavedCreditCard";
            vhub.ContainerID = vhub.PopupID;//String.Format("{0}CardContainer", this.CardType);
            vhub.UniqueName = this.CardType;

            this.okBtn.OnClientClick = vhub.ClientInstanceVariableName + ".validateAll(); return false;";
            base.OnInit(e);
        }

        //  public CreditCardUsageType UseType { get; set; }
        //public event EventHandler CreditCardAdded;
        public event CommandEventHandler CreditCardAdded;
        public event CommandEventHandler CreditCardSelected;
        public event EventHandler CancelClick;

        public string CloseButtonJS { get; set; }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            SetExpiryHiddenText();
        }
        protected void okBtn_OnClick(object sender, EventArgs e)
        {
            bool success = CreditCardUsageType != CreditCardUsageType.CreateNewCard && !this.ExpiredCard;
            if (CreditCardUsageType == CreditCardUsageType.CreateNewCard)
            {
                success = accountsPresenter.AddCreditCard();
                
                if (success && CreditCardAdded != null)
                {
                    CreditCardAdded(this, new CommandEventArgs("addCard", CreditCard));
                }
            }
            else if (CreditCardUsageType == CreditCardUsageType.SelectFromSavedCards && CreditCardSelected != null)
            {
                SetExpiryHiddenText();
                if (this.isCardChanged())
                {
                    
                    success = accountsPresenter.UpdateCreditCard();
                }
                if (success)
                {
                    CreditCardSelected(this, new CommandEventArgs("creditCardSelected", CreditCards[creditCardList.SelectedIndex]));
                }
            }
            // close the popup
            if (success)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeCreditCardModalPopup", CloseButtonJS, true);
                Reset();
            }
            else
            {
                //vhub.SetError
                //invalidCardNumber.Visible = true;
                //ErrorSummaryPanel.Attributes["class"] = "ValidationSummary";
                ResizeModal();
            }
        }

        private void Reset()
        {
            if (CreditCardUsageType != CreditCardUsageType.SelectFromSavedCards)
            {
                cardNumber.Text = string.Empty;
                this.cardTypeList.SelectedIndex = 0;
                cardMonth.SelectedValue = DateTime.Now.Month.ToString("00");
                cardYear.SelectedValue = DateTime.Now.Year.ToString();
                cardHolder.Text = string.Empty;
            }

        }

        protected void cancelBtn_OnClick(object sender, EventArgs e)
        {
            //don't call cancelclick to clean up the fields, Reset would do the job
            if (CancelClick != null)
            {
            //    CancelClick(this, e);
            }
            // close the popup
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeCreditCardModalPopup", CloseButtonJS, true);
            Reset();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void ResizeModal()
        {
            ScriptManager.RegisterStartupScript(
                    Page, this.GetType(),
                    "resizeCreditCard" + this.CardType,
                    String.Format("ResizeModal('{0}');bindExpDropdowns{1}();", creditCardUsageType == CreditCardUsageType.CreateNewCard ? "AddNewCreditCart" : "UseSavedCreditCard", this.CardType), true);
        }
        private bool isCardChanged()
        {
            bool dataChanged = false;
            CreditCard card = CreditCards[this.creditCardList.SelectedIndex] as CreditCard;
            if (card != null)
            {
                if (this.cardHolder.Text != card.NameOnCard)
                    dataChanged = true;
                else
                {
                    string date = expirationDateText.Text;
                    if (date != card.ExpirationDate)
                        dataChanged = true;
                }
            }
            return dataChanged;
        }
        protected void creditCardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string trigger = Request["__EVENTTARGET"];
            if (!string.IsNullOrEmpty(trigger) && trigger.EndsWith("creditCardList"))
                UpdateContent();
        }

        private void UpdateContent()
        {
            //CreditCard card;
            
            CreditCard = CreditCards[creditCardList.SelectedIndex];
            if (CreditCard != null)
            {
                this.cardTypeDisplayText.Text = Server.HtmlEncode(CreditCard.CreditCardType);
                this.cardNumberDisplayText.Text = Server.HtmlEncode(CreditCard.CardNumberViewable);
                this.cardNumber.Text = Server.HtmlEncode(CreditCard.CardNumber);
                this.cardHolder.Text = Server.HtmlEncode(CreditCard.NameOnCard);
                this.cardMonth.SelectedValue = CreditCard.ExpirationDate.Substring(0, 2);
                int y = Int32.Parse(CreditCard.ExpirationDate.Substring(3, 4));
                if (y < DateTime.Now.Year - 3)
                    this.cardYear.SelectedIndex = 0;
                else
                    this.cardYear.SelectedValue = CreditCard.ExpirationDate.Substring(3, 4);
                SetExpiryHiddenText();
                expiredCardRow.Attributes["class"] = this.ExpiredCard ? "" : "displayNone";
                ResizeModal();
            }
        }

        private void SetExpiryHiddenText()
        {
            expirationDateText.Text = string.Format("{0}/{1}", cardMonth.SelectedValue, cardYear.SelectedValue);
            expirationDateText.Attributes["custom1"] = String.Format("validate{0}Expiry()", this.CardType);
        }

        public bool ExpiredCard
        {
            get
            {
                int yy = Int32.Parse(this.cardYear.SelectedValue);
                int mm = Int32.Parse(this.cardMonth.SelectedValue);
                return DateTime.Now.Year > yy || (DateTime.Now.Year == yy && DateTime.Now.Month > mm);
            }
        }

        public void SelectCreditCard(Guid ccUid)
        {
            try
            {
                creditCardList.SelectedValue = ccUid.ToString();
                int index = 0;

                List<CreditCard> creditCards=CreditCards;
                if(CreditCards!=null)
                {
                   for (int i = 0; i < creditCards.Count; i++)
                    {
                      
                        if (creditCards[i].CreditCardUid.ToString()==ccUid.ToString())
                        {
                            index = i;
                            break;
                        }
                    }
                    creditCardList.SelectedIndex = index;
                    creditCardList.CssClass = "savedCardsList";
                    UpdateContent();
                }
            }
            catch (Exception)
            { }
        }
        #region ICreditInformationView Members

        private CreditCardUsageType creditCardUsageType;
        public CreditCardUsageType CreditCardUsageType
        {
            get { return creditCardUsageType; }
            set { creditCardUsageType = value; }
        }



        private CreditCard creditCard;
        public CreditCard CreditCard
        {
            get
            {
                if (creditCard == null)
                {
                    creditCard = new CreditCard();
                    if (CreditCardUsageType == CreditCardUsageType.CreateNewCard)
                    {
                        creditCard.CreditCardType = cardTypeDisplayText.Text;
                        creditCard.CardNumber = cardNumber.Text;
                        creditCard.CreditCardTypeCode = cardTypeList.SelectedValue;
                        creditCard.NameOnCard = cardHolder.Text;
                        if (creditCard.CreditCardUid == Guid.Empty)
                        {
                            creditCard.CreditCardUid = Guid.NewGuid();
                        }
                        creditCard.ExpirationDate = creditCard.ExpirationDate = expirationDateText.Text;
                    }
                    else if (CreditCardUsageType == CreditCardUsageType.SelectFromSavedCards)
                    {
                        creditCard = CreditCards[creditCardList.SelectedIndex];
                        creditCard.ExpirationDate = expirationDateText.Text;
                        //creditCard.CardNumberViewable = string.Empty;
                        creditCard.CardNumber = CreditCard.CardNumberViewable;
                        creditCard.NameOnCard = CardHolderName;

                    }
                }
                return creditCard;
            }

            set
            {
                creditCard = value;
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
                        "CreditCardBlock",
                        "CreditCards",
                        StateItemStore.AspSession);
                }
                return creditCards;
            }
            set
            {
                //if (this.IsPostBack)
                //    return;

                creditCards = value;
                InitializeDateDropdown();
                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<List<CreditCard>> creditCardsStateItem =
                    new StateItem<List<CreditCard>>(
                    "CreditCardBlock",
                    "CreditCards",
                    creditCards,
                    StateItemStore.AspSession,
                    StatePersistenceDuration.Session);
                if (creditCards != null)
                {
                    stateItems.SetStateItem<List<CreditCard>>(creditCardsStateItem);
                    var results = from card in value
                                  select new { CardGuid = card.CreditCardUid, DisplayText = card.CreditCardType + card.CardNumberViewable };
                    
                    creditCardList.DataSource = results;

                    creditCardList.DataTextField = "DisplayText";
                    creditCardList.DataValueField = "CardGuid";
                    creditCardList.DataBind();
                    int index = 0; 

                    for (int i = 0; i < value.Count; i++)
                    {
                      
                        if (value[i].IsDefault)
                        {
                            index = i;
                            break;
                        }
                    }
                    creditCardList.SelectedIndex = index;
                    creditCardList.CssClass = "savedCardsList";
                    UpdateContent();

                    string myTrigger = Request["__EVENTTARGET"];
                    Debug.WriteLine("trigger=" + myTrigger);
                    if (!IsPostBack || (!string.IsNullOrEmpty(myTrigger) && myTrigger.EndsWith("savedCardSubmit")))
                    {
                        Debug.WriteLine("trigger, I am in!!!");
                        if (this.CreditCardSelected != null)
                        {
                            CreditCardSelected(this, new CommandEventArgs("creditCardSelected", CreditCards[this.creditCardList.SelectedIndex] as CreditCard));
                        }
                    }
                }

                else
                {
                    stateItems.DeleteStateItem<List<CreditCard>>(creditCardsStateItem);
                }
            }
        }

        public List<ContentItem> CardTypes
        {
            get
            {
                //if (ViewState[VS_TAG_CARDTYPE_DATA] == null)
                //{
                //    AccountsPresenter accountP = new AccountsPresenter(null);
                //    ViewState[VS_TAG_CARDTYPE_DATA] = accountP.GetCardTypes(Profile.AddressDetail.CountryCode);
                //}
                return (List<ContentItem>)ViewState[VS_TAG_CARDTYPE_DATA];
            }
            set
            {
                cardTypeList.DataSource = value;
                cardTypeList.DataTextField = "ContentValue";
                cardTypeList.DataValueField = "Key";
                cardTypeList.DataBind();
                ViewState[VS_TAG_CARDTYPE_DATA] = value;
            }
        }


        public string CardTypeDisplayText
        {
            get
            {
                return cardTypeDisplayText.Text;
            }
            set
            {
                cardTypeDisplayText.Text = Server.HtmlEncode(value);
            }
        }

        [PropertyControlMapper("cardNumber")]
        public string CardNumber
        {
            get
            {
                return cardNumber.Text;
            }
            set
            {
                cardNumber.Text = value;
            }
        }

        public string CardNumberDisplayText
        {
            get
            {
                return cardNumberDisplayText.Text;
            }
            set
            {
                cardNumberDisplayText.Text = value;
            }
        }

        public string CardMonth
        {
            get
            {
                return cardMonth.SelectedValue;
            }
            set
            {
                cardMonth.SelectedValue = value;
            }
        }

        public string CardYear
        {
            get
            {
                return cardYear.SelectedValue;
            }
            set
            {
                cardYear.SelectedValue = value;
            }
        }

        public string CardHolderName
        {
            get
            {
                return cardHolder.Text;
            }
            set
            {
                cardHolder.Text = value;
            }
        }

        private bool displayCardListSection = false;
        public bool DisplayCardListSection
        {
            get
            {

                return displayCardListSection;
            }
            set
            {
                displayCardListSection = value;
                cardListSection.Attributes["class"] = value ? "FormRow" : "displayNone";
            }
        }
        public bool DisplayCardNumberDisplayText
        {
            get
            {
                return cardNumberDisplayText.Visible;
            }
            set
            {
                cardNumberDisplayText.Visible = value;
            }
        }
        public bool DisplayCardTypeDisplayText
        {
            get
            {
                return cardTypeDisplayText.Visible;
            }
            set
            {
                cardTypeDisplayText.Visible = value;
            }
        }
        public bool DisplayCardTypeList
        {
            get
            {
                return cardTypeList.Visible;
            }
            set
            {
                cardTypeList.Visible = value;
            }
        }
        public bool DisplayCardNumber
        {
            get
            {
                return cardNumber.Visible;
            }
            set
            {
                cardNumber.Visible = value;
            }
        }

        #endregion

        #region Validation Hub Error Override

        public override void  SetValidationError<T>(string invalidControlName, T errorEnumValue, bool showInSummary, bool showHilite)
        {
            string errorMessage = CorbisBasePage.GetEnumDisplayText<T>(errorEnumValue, Resources.Accounts.ResourceManager);
            this.vhub.SetError(
                this.cardNumber,
                errorMessage,
                true,
                true);
        }
        #endregion
    }

}
