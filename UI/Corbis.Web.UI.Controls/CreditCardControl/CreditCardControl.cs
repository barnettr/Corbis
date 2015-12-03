using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Controls
{
    [ToolboxData("<{0}:CreditCard runat=server></{0}:CreditCard>")]
    public class CreditCardControl : CompositeControl/*, IValidator, IGroupValidator*/
    {
        public const string CARDTYPE_AMEX = "A";
        public const string CARDTYPE_MC = "M";
        public const string CARDTYPE_VISA = "V";

        private const string VS_TAG_CARDTYPE_CAPTION = "VS_TAG_CARDTYPE_CAPTION";
        private const string VS_TAG_CARDHOLDER_CAPTION = "VS_TAG_CARDHOLDER_CAPTION";
        private const string VS_TAG_CARDNUMBER_CAPTION = "VS_TAG_CARDNUMBER_CAPTION";
        private const string VS_TAG_EXPIRATIONDATE_CAPTION = "VS_TAG_EXPIRATIONDATE_CAPTION";
        private const string VS_TAG_FORM_LABELS_CSS = "VS_TAG_FORM_LABELS_CSS";
        private const string VS_TAG_FORM_FIELDS_CSS = "VS_TAG_FORM_FIELDS_CSS";
        private const string VS_TAG_FORM_ROWS_CSS = "VS_TAG_FORM_ROWS_CSS";
        private const string VS_TAG_READONLY_FIELDS_CSS = "VS_TAG_READONLY_FIELDS_CSS";
        private const string VS_TAG_VALID_CONTROL = "VS_TAG_VALID_CONTROL";
        private const string VS_TAG_VALIDATION_ERROR_MESSAGE = "VS_TAG_VALIDATION_ERROR_MESSAGE";
        private const string VS_TAG_EDITMODE = "VS_TAG_EDITMODE";
        private const string VS_TAG_VALIDATIONGROUP = "VS_TAG_VALIDATIONGROUP";
        private const string VS_TAG_ERROR_CARDTYPE = "VS_TAG_ERROR_CARDTYPE";
        private const string VS_TAG_ERROR_CARDNUMBER = "VS_TAG_ERROR_CARDNUMBER";
        private const string VS_TAG_ERROR_EXPIRATIONDATE = "VS_TAG_ERROR_EXPIRATIONDATE";
        private const string VS_TAG_ERROR_CARDHOLDER = "VS_TAG_ERROR_CARDHOLDER";
        private const string VS_TAG_YEAR_FORMAT = "VS_TAG_YEAR_FORMAT";
        private const string VS_TAG_MONTH_FORMAT = "VS_TAG_MONTH_FORMAT";
        private const string LINE_BREAK = "<br />";
        private const string VS_TAG_CARDTYPE_DATA = "VS_TAG_CARDTYPE_DATA";

        private Label cardTypeCaption = new Label();
        private Label cardNumberCaption = new Label();
        private Label expirationDateCaption = new Label();
        private Label cardholderNameCaption = new Label();
        private DropDownList expirationMonth = new DropDownList();
        private DropDownList expirationYear = new DropDownList();
        private TextBox expirationDate = new TextBox();
        private DropDownList cardType = new DropDownList();
        private TextBox cardholderName = new TextBox();
        private TextBox cardNumber = new TextBox();
        private HtmlGenericControl validatorContainer = new HtmlGenericControl();

        #region Data Properties

        [Browsable(false)]
        public object CardTypeData
        {
            get
            {
                this.EnsureChildControls();
                return ViewState[VS_TAG_CARDTYPE_DATA];
            }
            set
            {
                ViewState[VS_TAG_CARDTYPE_DATA] = value;
            }
        }

        [Category("Card Data"),
        Description("Gets/Sets the cardholder name"),
        DefaultValue("")]
        public string CardholderName
        {
            get
            {
                this.EnsureChildControls();
                return cardholderName.Text;
            }
            set
            {
                this.EnsureChildControls();
                cardholderName.Text = value;
            }
        }

        [Category("Appearance"),
        Description("Gets/Sets the card number"),
        DefaultValue("")]
        public string CardNumber
        {
            get
            {
                this.EnsureChildControls();
                return cardNumber.Text;
            }
            set
            {
                this.EnsureChildControls();
                cardNumber.Text = value;
            }
        }

        [Category("Card Data"),
        Description("Gets/Sets the card type"),
        DefaultValue("")]
        public string CardType
        {
            get
            {
                this.EnsureChildControls();
                return cardType.SelectedValue;
            }
            set
            {
                this.EnsureChildControls();
                cardType.SelectedValue = value;
            }
        }

        [Category("Card Data"),
        Description("Gets/Sets the cardholder name"),
        DefaultValue("")]
        public string ExpirationMonth
        {
            get
            {
                this.EnsureChildControls();
                return expirationMonth.SelectedValue;
            }
            set
            {
                this.EnsureChildControls();
                if (expirationMonth.Items.FindByValue(value) != null)
                {
                    expirationMonth.SelectedValue = value;
                }
                else
                {
                    expirationMonth.Text = value;
                }
            }
        }

        [Category("Card Data"),
        Description("Gets/Sets the cardholder name"),
        DefaultValue("")]
        public string ExpirationYear
        {
            get
            {
                this.EnsureChildControls();
                return expirationYear.SelectedValue;
            }
            set
            {
                this.EnsureChildControls();
                expirationYear.SelectedValue = value;
            }
        }

        #endregion

        #region Caption Properties
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CardTypeCaption
        {
            get
            {
                String s = (String)ViewState[VS_TAG_CARDTYPE_CAPTION];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_CARDTYPE_CAPTION] = value;
                this.ChildControlsCreated = false;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CardholderCaption
        {
            get
            {
                String s = (String)ViewState[VS_TAG_CARDHOLDER_CAPTION];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_CARDHOLDER_CAPTION] = value;
                this.ChildControlsCreated = false;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CardNumberCaption
        {
            get
            {
                String s = (String)ViewState[VS_TAG_CARDNUMBER_CAPTION];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_CARDNUMBER_CAPTION] = value;
                this.ChildControlsCreated = false;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ExpirationDateCaption
        {
            get
            {
                String s = (String)ViewState[VS_TAG_EXPIRATIONDATE_CAPTION];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_EXPIRATIONDATE_CAPTION] = value;
                this.ChildControlsCreated = false;
            }
        }

















        #endregion

        #region Styles

        [Category("Styles")]
        [DefaultValue("")]
        public string FormLabelsCSS
        {
            get
            {
                String s = (String)ViewState[VS_TAG_FORM_LABELS_CSS];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_FORM_LABELS_CSS] = value;
            }
        }

        [Category("Styles")]
        [DefaultValue("")]
        public string FormFieldsCSS
        {
            get
            {
                String s = (String)ViewState[VS_TAG_FORM_FIELDS_CSS];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_FORM_FIELDS_CSS] = value;
            }
        }

        [Category("Styles")]
        [DefaultValue("")]
        public string FormRowsCSS
        {
            get
            {
                String s = (String)ViewState[VS_TAG_FORM_ROWS_CSS];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_FORM_ROWS_CSS] = value;
            }
        }

        [Category("Styles")]
        [DefaultValue("")]
        public string ReadOnlyFieldsCSS
        {
            get
            {
                String s = (String)ViewState[VS_TAG_READONLY_FIELDS_CSS];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_READONLY_FIELDS_CSS] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string YearFormat
        {
            get
            {
                String s = (String)ViewState[VS_TAG_YEAR_FORMAT];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_YEAR_FORMAT] = value;
                this.ChildControlsCreated = false;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string MonthFormat
        {
            get
            {
                String s = (String)ViewState[VS_TAG_MONTH_FORMAT];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState[VS_TAG_MONTH_FORMAT] = value;
                this.ChildControlsCreated = false;
            }
        }

        #endregion

        protected override void CreateChildControls()
        {
            SetInitialValuesForLabels();
            SetInitialValuesForFields();

            HtmlTableRow tr = new HtmlTableRow();
            tr.Attributes["class"] = FormRowsCSS;
            HtmlTableCell td = new HtmlTableCell();
            td.Attributes["class"] = FormLabelsCSS;
            td.Controls.Add(cardTypeCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCSS;
            td.Controls.Add(validatorContainer);
            if (EditMode)
            {
                cardType.Enabled = false;
            }
            td.Controls.Add(cardType);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = FormRowsCSS;
            td = new HtmlTableCell();
            td.Attributes["class"] = FormLabelsCSS;
            td.Controls.Add(cardNumberCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCSS;
            if (EditMode)
            {
                cardNumber.Enabled = false;
            }
            td.Controls.Add(cardNumber);
            tr.Controls.Add(td);
            Controls.Add(tr);



























































































            tr = new HtmlTableRow();
            tr.Attributes["class"] = FormRowsCSS;
            td = new HtmlTableCell();
            td.Attributes["class"] = FormLabelsCSS;
            td.Controls.Add(expirationDateCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCSS;
            expirationMonth.CssClass = "widthAuto";
            expirationYear.CssClass = "widthAuto";
            expirationDate.CssClass = "displayNone";
            td.Controls.Add(expirationMonth);
            td.Controls.Add(expirationYear);
            td.Controls.Add(expirationDate);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = FormRowsCSS;
            td = new HtmlTableCell();
            td.Attributes["class"] = FormLabelsCSS;
            td.Controls.Add(cardholderNameCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCSS;
            td.Controls.Add(cardholderName);
            tr.Controls.Add(td);
            Controls.Add(tr);


            this.DataBind();

        }
        /*
                private void CreateBeginRow()
                {
                    this.Controls.Add(new LiteralControl("<tr>"));
                }

                private void CreateBeginRowFormat(string cssClass)
                {
                    this.Controls.Add(new LiteralControl(string.Format("<tr runat=\"server\" class=\"{0}\">", cssClass)));
                }

                private void CreateEndRow()
                {
                    this.Controls.Add(new LiteralControl("</tr>"));
                }

                private void CreateBeginColumn()
                {
                    this.Controls.Add(new LiteralControl("<td>"));
                }

                private void CreateBeginColumnFormat(string cssClass)
                {
                    this.Controls.Add(new LiteralControl(string.Format("<td class=\"{0}\">", cssClass)));
                }

                private void CreateEndColumn()
                {
                    this.Controls.Add(new LiteralControl("</td>"));
                }
        */
        private void SetInitialValuesForLabels()
        {
            cardTypeCaption.ID = "cardTypeCaption";
            cardTypeCaption.Text = this.CardTypeCaption;

            cardNumberCaption.ID = "cardNumberCaption";
            cardNumberCaption.Text = this.CardNumberCaption;

            expirationDateCaption.ID = "expirationDateCaption";
            expirationDateCaption.Text = this.ExpirationDateCaption;

            cardholderNameCaption.ID = "cardholderNameCaption";
            cardholderNameCaption.Text = this.CardholderCaption;
        }

        public CreditCard ExportCreditCardInformation()
        {
            CreditCard card = new CreditCard();
            if (!EditMode)
            {
                card.CreditCardType = cardType.SelectedItem.Text;
                card.CreditCardTypeCode = cardType.SelectedValue;
            }
            card.CardNumber = cardNumber.Text;
            card.ExpirationDate = expirationMonth.SelectedValue + "/" + expirationYear.SelectedValue;
            card.NameOnCard = cardholderName.Text;
            return card;
        }

        private void SetInitialValuesForFields()
        {
            cardType.ID = "cardType";
            cardType.SelectedIndexChanged += new EventHandler(CardType_SelectedIndexChanged);
            cardType.AutoPostBack = true;

            cardType.EnableViewState = true;
            cardType.DataTextField = "ContentValue";
            cardType.DataValueField = "Key";
            cardType.DataSource = this.CardTypeData;
            cardType.ValidateControl = true;
            cardType.ValidationGroup = ValidationGroup;
            cardType.ValidationObjectType = typeof(CreditCard).FullName;
            cardType.ValidationPropertyName = "CreditCardTypeCode";
            cardType.ValidationRulesetName = "DefaultRuleSet";
            cardType.ValidatorContainer = "validatorContainer";

            cardNumber.ID = "cardNumber";
            cardNumber.MaxLength = 16;
            cardNumber.ValidateControl = true;
            cardNumber.ValidationGroup = ValidationGroup;
            cardNumber.ValidationObjectType = typeof(CreditCard).FullName;
            cardNumber.ValidationPropertyName = "CardNumber";
            cardNumber.ValidationRulesetName = "AddMemberCreditCard";
            cardNumber.ValidatorContainer = "validatorContainer";

            expirationMonth.ID = "expirationMonth";
            expirationMonth.AutoPostBack = true;
            expirationMonth.SelectedIndexChanged += delegate { SetExpirationDate(); };
            LoadMonths();

            expirationYear.ID = "expirationYear";
            expirationYear.AutoPostBack = true;
            expirationYear.SelectedIndexChanged += delegate { SetExpirationDate(); };
            LoadYears();

            expirationDate.ID = "expirationDate";
            expirationDate.ValidateControl = true;
            expirationDate.ValidationGroup = ValidationGroup;
            expirationDate.ValidationObjectType = typeof(CreditCard).FullName;
            expirationDate.ValidationPropertyName = "ExpirationDate";
            expirationDate.ValidationRulesetName = "DefaultRuleSet";
            expirationDate.ValidatorContainer = "validatorContainer";

            cardholderName.ID = "cardholderName";
            cardholderName.Text = this.CardholderName;
            cardholderName.MaxLength = 100;
            cardholderName.ValidateControl = true;
            cardholderName.ValidationGroup = ValidationGroup;
            cardholderName.ValidationObjectType = typeof(CreditCard).FullName;
            cardholderName.ValidationPropertyName = "NameOnCard";
            cardholderName.ValidationRulesetName = "DefaultRuleSet";
            cardholderName.ValidatorContainer = "validatorContainer";

            validatorContainer.ID = "validatorContainer";
            validatorContainer.Attributes["class"] = "displayNone";

            this.DataBind();
        }

        [Category("Styles"),
        Description("Determines whether the credit card is in edit mode"),
        DefaultValue(false)]
        public bool EditMode
        {
            get
            {
                if (ViewState[VS_TAG_EDITMODE] == null)
                    return false;

                return (bool)ViewState[VS_TAG_EDITMODE];
            }
            set
            {
                ViewState[VS_TAG_EDITMODE] = value;
                this.ChildControlsCreated = false;
            }
        }

        private void LoadYears()
        {
            this.expirationYear.Items.Clear();
            this.expirationYear.Items.Add(new ListItem(YearFormat));

            int year = DateTime.Now.Year;

            for (int i = 0; i < 15; i++)
            {
                int yearData = (year + i);

                this.expirationYear.Items.Add(
                    new ListItem(yearData.ToString()));
            }
        }

        private void LoadMonths()
        {
            this.expirationMonth.Items.Clear();
            this.expirationMonth.Items.Add(new ListItem(MonthFormat));

            for (int i = 1; i < 13; i++)
            {
                this.expirationMonth.Items.Add(new ListItem(i.ToString("00")));
            }

        }

        private void SetExpirationDate()
        {
            try
            {
                expirationDate.Text = new DateTime(int.Parse(ExpirationYear), int.Parse(ExpirationMonth), 1).AddMonths(1).AddDays(-1).ToShortDateString();
            }
            catch
            {
                expirationDate.Text = string.Empty;
            }
        }

        protected void CardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCardLengthProperties();
        }

        private void SetCardLengthProperties()
        {
            switch (cardType.SelectedValue)
            {
                case CARDTYPE_AMEX:
                    this.cardNumber.MaxLength = AmexCardNumberValidation.MaxLength;
                    break;
                case CARDTYPE_MC:
                    this.cardNumber.MaxLength = MasterCardCardNumberValidation.MaxLength;
                    break;
                case CARDTYPE_VISA:
                    this.cardNumber.MaxLength = VisaCardNumberValidation.MaxLength;
                    break;
                default:
                    this.cardNumber.MaxLength = CardNumberValidationBase.MaxLength;
                    break;

            }
            cardNumber.Text = String.Empty;
        }

        [Category("Error Messages"),
        Description("Sets the Validation Group For The Control"),
        DefaultValue("")]
        public string ValidationGroup
        {
            get
            {
                if (ViewState[VS_TAG_VALIDATIONGROUP] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_VALIDATIONGROUP];
            }
            set
            {
                ViewState[VS_TAG_VALIDATIONGROUP] = value;
            }
        }

        #region IValidator Members
        /*
        public string ErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_VALIDATION_ERROR_MESSAGE] == null)
                    return String.Empty;
                return (string)ViewState[VS_TAG_VALIDATION_ERROR_MESSAGE];
            }
            set
            {
                ViewState[VS_TAG_VALIDATION_ERROR_MESSAGE] = value;
            }
        }

        public bool IsValid
        {
            get
            {
                if (ViewState[VS_TAG_VALID_CONTROL] == null)
                    return false;
                return (bool)ViewState[VS_TAG_VALID_CONTROL];

            }
            set
            {
                ViewState[VS_TAG_VALID_CONTROL] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
                Page.Validators.Add(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (Page != null)
                Page.Validators.Remove(this);
        }
*/
        [Category("Error Messages"),
        Description("Sets the Error Message For Card Type"),
        DefaultValue(""),
        Localizable(true)]
        public string CardTypeErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_ERROR_CARDTYPE] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_CARDTYPE];
            }
            set
            {
                ViewState[VS_TAG_ERROR_CARDTYPE] = value;
            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Card Number"),
        DefaultValue(""),
        Localizable(true)]
        public string CardNumberErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_ERROR_CARDNUMBER] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_CARDNUMBER];
            }
            set
            {
                ViewState[VS_TAG_ERROR_CARDNUMBER] = value;
            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Expiration Date"),
        DefaultValue(""),
        Localizable(true)]
        public string ExpirationDateErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_ERROR_EXPIRATIONDATE] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_EXPIRATIONDATE];
            }
            set
            {
                ViewState[VS_TAG_ERROR_EXPIRATIONDATE] = value;

            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Cardholder"),
        DefaultValue(""),
        Localizable(true)]
        public string CardholderErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_ERROR_CARDHOLDER] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_CARDHOLDER];
            }
            set
            {
                ViewState[VS_TAG_ERROR_CARDHOLDER] = value;
            }
        }
        /*
                public void Validate()
                {
                    bool result = true;
                    this.ErrorMessage = String.Empty;

                    if (!this.EditMode)
                    {
                        if (this.cardType.SelectedValue.Length == 0)
                        {
                            this.ErrorMessage += this.CardTypeErrorMessage + LINE_BREAK;
                            result = false;
                        }
                        if (!IsCardNumberValid())
                        {
                            this.ErrorMessage += this.CardNumberErrorMessage + LINE_BREAK;
                            result = false;
                        }
                    }

                    if (!IsExpirationValid())
                    {
                        this.ErrorMessage += this.ExpirationDateErrorMessage + LINE_BREAK;
                        result = false;
                    }

                    if (this.cardholderName.Text.Length == 0)
                    {
                        this.ErrorMessage += this.CardholderErrorMessage + LINE_BREAK;
                        result = false;
                    }

                    this.IsValid = result;
                }

                private bool IsExpirationValid()
                {
                    try
                    {
                        int year = Int32.Parse(this.expirationYear.SelectedValue);

                        if (year > DateTime.Now.Year)
                            return true;

                        int month = Int32.Parse(this.expirationMonth.SelectedValue);

                        return month >= DateTime.Now.Month;
                    }
                    catch (FormatException ex)
                    {
                        return false;
                    }
                }

                private bool IsCardNumberValid()
                {
                    if (this.cardNumber.Text.Length == 0)
                        return false;

                    CardNumberValidationBase card;
                    switch (cardType.SelectedValue)
                    {
                        case CARDTYPE_AMEX:
                            card = new AmexCardNumberValidation(cardNumber.Text);
                            break;
                        case CARDTYPE_MC:
                            card = new MasterCardCardNumberValidation(cardNumber.Text);
                            break;
                        case CARDTYPE_VISA:
                            card = new VisaCardNumberValidation(cardNumber.Text);
                            break;
                        default:
                            card = new CardNumberValidationBase(cardNumber.Text);
                            break;

                    }

                    return card.IsValid();
                }
        */
        #endregion
    }





}

