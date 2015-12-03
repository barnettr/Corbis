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
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;


namespace Corbis.Web.UI.Checkout
{
    public partial class EditPaymentInformation : CorbisBasePage, ICreditInformationView, IViewPropertyValidator
    {
        private const string VS_TAG_CARDTYPE_DATA = "VS_TAG_CARDTYPE_DATA";
        
        private string CreditCardID
        {
            get
            {
                return creditCardList.SelectedValue;
            }
        }

        private string Mode
        {
            get
            {
                return Request.QueryString["mode"];
            }
        }

        private string CCUid
        {
            get
            {
                return Request.QueryString["CCUid"];
            }
        }
        private string DefUid
        {
            get
            {
                return Request.QueryString["DefUid"];
            }
        }
        private AccountsPresenter accountsPresenter;
        #region Page events
        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            accountsPresenter = new AccountsPresenter(this);
            InitializeDateDropdown();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupCreditCardBlock();
            SetExpiryHiddenText();
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Accounts, "accounts");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Checkout, "checkout");
        }

        #endregion

        #region Form initialization
        // Initialize the date dropdown
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
                if ((CreditCardUsageType == CreditCardUsageType.CreateNewCard && i == 0) || (CreditCardUsageType != CreditCardUsageType.CreateNewCard && i == 3))
                    li.Selected = true;
            }
            if (DateHelper.YearMonthPattern[0] == 'y')
            {
                cardYear.Parent.Controls.Remove(cardMonth);
                int yearIndex = cardYear.Parent.Controls.IndexOf(this.cardYear);
                cardYear.Parent.Controls.AddAt(yearIndex + 1, cardMonth);
            }
        }

        private void SetupCreditCardBlock()
        {
            if (!IsPostBack)
            {
                if (this.Mode == "add")
                {
                    this.SetAddMode();
                }
                else
                {
                    //creditCard.EditMode = true;
                    this.SetEditMode();
                    CreditCardSelected(null, null);
                }
            }
        }

        protected void CreditCardSelected(object sender, EventArgs e)
        {
            this.CreditCard = accountsPresenter.GetCreditCard(this.CreditCardID);
        }

        private void SetExpiryHiddenText()
        {
            expirationDateText.Text = string.Format("{0}/{1}", cardMonth.SelectedValue, cardYear.SelectedValue);
            expirationDateText.Attributes["custom1"] = "validateExpiry()";
        }

        private void SetEditMode()
        {
            accountsPresenter.LoadNewCreditInformation();
            accountsPresenter.LoadSavedCreditInformation();
            this.creditCardTitleAdd.Visible = false;
            this.cardTypeDisplayText.Visible = true;
            this.cardTypeList.Visible = false;
            this.cardNumber.Visible = false;
            this.defaultCCRow.Visible = false;
            if (!string.IsNullOrEmpty(this.CCUid))
            {
                try
                {
                    creditCardList.SelectedValue = this.CCUid;
                    savedCCRow.Style.Add("display", "none");
                }
                catch (Exception) { }
            }
            else if (!string.IsNullOrEmpty(this.DefUid))
            {
                try
                {
                    creditCardList.SelectedValue = this.DefUid;   
                }
                catch (Exception)
                {
                }

            }
        }

        private void SetAddMode()
        {
            accountsPresenter.LoadNewCreditInformation();
            this.savedCCRow.Visible = false;
            this.creditCardTitleEdit.Visible = false;
        }
        #endregion

        #region Save actions
        protected void okBtn_OnClick(object sender, EventArgs e)
        {
            bool success = CreditCardUsageType != CreditCardUsageType.CreateNewCard && !this.ExpiredCard;
            if (CreditCardUsageType == CreditCardUsageType.CreateNewCard)
            {
                success = accountsPresenter.AddCreditCard();

                if (success)
                {
                    DoSuccess();
                }
            }
            else if (CreditCardUsageType == CreditCardUsageType.SelectFromSavedCards)
            {
                SetExpiryHiddenText();
                success = accountsPresenter.UpdateCreditCard();
                if (success)
                {
                    DoSuccess();
                }
            }
            // close the popup
            if (!success)
            {
                ResizeModal();
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CreditCardID))
            {
                accountsPresenter.DeleteCreditCard(this.CreditCardID);
            }
            DoSuccess();
        }
        #endregion

        #region Helper methods
        private void ResizeModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResizeModal", "parent.ResizeIModal('editPaymentInfoModalPopup', GetDocumentHeight());", true);
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

        private void DoSuccess()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.setCreditCard('" + this.creditCard.CreditCardUid + "');parent.MochaUI.CloseModal('editPaymentInfoModalPopup');", true);
        }
        #endregion

        #region ICreditInformationView Members

        public CreditCardUsageType CreditCardUsageType
        {
            get { return String.IsNullOrEmpty(Mode) ? CreditCardUsageType.SelectFromSavedCards : CreditCardUsageType.CreateNewCard; }
            set { }
        }

        private CreditCard creditCard = null;
        public CreditCard CreditCard
        {
            get
            {
                creditCard = new CreditCard();
                if (CreditCardUsageType == CreditCardUsageType.SelectFromSavedCards)
                {
                    creditCard = accountsPresenter.GetCreditCard(this.CreditCardID);
                    creditCard.CardNumber = creditCard.CardNumberViewable;
                }
                else
                {
                    creditCard.CreditCardUid = Guid.NewGuid();
                    creditCard.CreditCardType = cardTypeList.SelectedItem.Text;
                    creditCard.CardNumber = cardNumber.Text;
                    creditCard.IsDefault = this.defaultCreditCard.Checked;
                    creditCard.CreditCardTypeCode = cardTypeList.SelectedValue;
                }
                creditCard.NameOnCard = cardHolder.Text;
                creditCard.ExpirationDate = creditCard.ExpirationDate = expirationDateText.Text;
                return creditCard;
            }

            set
            {
                this.defaultCreditCard.Checked = value.IsDefault;
                this.cardTypeList.SelectedValue = value.CreditCardTypeCode;
                this.cardNumber.Text = value.CardNumberViewable;
                string[] expirationDateParse = value.ExpirationDate.Split('/');
                this.cardMonth.SelectedValue = expirationDateParse[0];
                this.cardYear.SelectedValue = expirationDateParse[1];
                this.cardHolder.Text = value.NameOnCard;
                this.cardNumberDisplayText.Text = value.CardNumberViewable;
                this.cardTypeDisplayText.Text = GetResourceString("Resource", value.CreditCardTypeCode + "_card");
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
                var results = from card in value
                              select new { CardGuid = card.CreditCardUid, DisplayText = GetResourceString("Resource", card.CreditCardTypeCode + "_card") + card.CardNumberViewable };

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
            }
        }

        [PropertyControlMapper("cardTypeList")]
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
                cardTypeDisplayText.Text = value;
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

        [PropertyControlMapper("cardYear")]
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

        [PropertyControlMapper("cardHolder")]        
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
                //cardListSection.Attributes["class"] = value ? "FormRow" : "displayNone";
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
                // Always display this
            }
        }


        #endregion

        #region Validation Error Overrides

        public override void SetValidationHubError<T>(Control control, T errorEnumValue, bool showInSummary, bool showHilite)
        {
            string errorMessage = GetEnumDisplayText<T>(errorEnumValue, Resources.Accounts.ResourceManager);
            this.vHub.SetError(control, errorMessage, showInSummary, showHilite);
        }
        #endregion

    }
}
