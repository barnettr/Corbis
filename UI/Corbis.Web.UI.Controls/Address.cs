using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using AjaxControlToolkit;
using Corbis.Framework.Globalization;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Validation;

namespace Corbis.Web.UI.Controls
{
    [ToolboxData("<{0}:Address runat='server'></{0}:Address>")]
    public class Address : CompositeControl, IViewPropertyValidator /* , IValidator, IGroupValidator*/
    {
        private const string POSTALCODE_HK = ".";
        private const string COUNTRY_HONGKONG = "HK";
        private const string VS_TAG_DISPLAYONLY = "VS_TAG_DISPLAYONLY";
        private const string VS_TAG_ERROR_POSTALCODE = "VS_TAG_ERROR_POSTALCODE";
        private const string VS_TAG_ERROR_CITY = "VS_TAG_ERROR_CITY";
        private const string VS_TAG_ERROR_COUNTRY = "VS_TAG_ERROR_COUNTRY";
        private const string VS_TAG_ERROR_REGION = "VS_TAG_ERROR_REGION";
        private const string VS_TAG_ERROR_ADDRESS1 = "VS_TAG_ERROR_ADDRESS1";
        private const string VS_TAG_VALIDATIONGROUP = "VS_TAG_VALIDATIONGROUP";
        private const string VS_TAG_SELECT_REQUIRED = "VS_TAG_SELECT_REQUIRED";
        private const string VS_TAG_SELECT_OPTIONAL = "VS_TAG_SELECT_OPTIONAL";
        private const string COUNTRY_US = "US";
        public event EventHandler CountryDataChange; 

        #region Private Constants and Variables

        #region Constants

        private const string BEGIN_DIV = "<div>";
        private const string BEGIN_DIV_FORMAT = "<div class=\"{0}\">";
        private const string END_DIV = "</div>";
        private const string BEGIN_ROW = "<tr>";
        private const string BEGIN_ROW_FORMAT = "<tr runat=\"server\" class=\"{0}\">";
        private const string END_ROW = "</tr>";
        private const string BEGIN_COLUMN = "<td>";
        private const string BEGIN_COLUMN_FORMAT = "<td class=\"{0}\">";
        private const string END_COLUMN = "</td>";

        private const string VS_TAG_ROWSTYLE = "VS_TAG_ROWSTYLE";
        private const string VS_TAG_LABELSTYLE = "VS_TAG_LABELSTYLE";
        private const string VS_TAG_FORMFIELDSTYLE = "VS_TAG_FORMFIELDSTYLE";
        private const string VS_TAG_OPTIONAL = "VS_TAG_OPTIONAL";
        private const string VS_TAG_OPTIONALSTYLE = "VS_TAG_OPTIONALSTYLE";
        private const string VS_TAG_ADDRESS1_CAPTION = "ADDRESS1_CAPTION";
        private const string VS_TAG_ADDRESS2_CAPTION = "ADDRESS2_CAPTION";
        private const string VS_TAG_ADDRESS3_CAPTION = "ADDRESS3_CAPTION";
        private const string VS_TAG_CITY_CAPTION = "CITY_CAPTION";
        private const string VS_TAG_COUNTRY_CAPTION = "COUNTRY_CAPTION";
        private const string VS_TAG_REGION_CAPTION = "REGION_CAPTION";
        private const string VS_TAG_POSTALCODE_CAPTION = "POSTALCODE_CAPTION";
        
        private const string VS_TAG_VALIDATION_ERROR_MESSAGE = "VALIDATION_ERROR_MESSAGE";
        private const string VS_TAG_VALID_CONTROL = "VS_TAG_VALID_CONTROL";
        
        private const string VS_TAG_COUNTRY_DATA = "VS_TAG_COUNTRY_DATA";
        private const string VS_TAG_REGION_DATA = "VS_TAG_REGION_DATA";

        private const string ROWSTYLE_DEFAULT = "";
        private const string LABELSTYLE_DEFAULT = "";
        private const string FORMFIELDSTYLE_DEFAULT = "";

        private const string ADDRESS1_CAPTION_DEFAULT = "Street Address 1:";
        private const string ADDRESS2_CAPTION_DEFAULT = "Street Address 2:";
        private const string ADDRESS3_CAPTION_DEFAULT = "Street Address 3:";
        private const string CITY_CAPTION_DEFAULT = "City:";
        private const string COUNTRY_CAPTION_DEFAULT = "Country / Region:";
        private const string REGION_CAPTION_DEFAULT = "State / Province:";
        private const string POSTALCODE_CAPTION_DEFAULT = "Postal Code:";
        
        private const int ADDRESS_MAX_LENGTH = 40;
        private const int CITY_MAX_LENGTH = 30;
        private const int POSTALCODE_MAX_LENGTH = 10;

        #endregion

        #region Labels

        private Label address1Caption = new Label();
        private Label address2Caption = new Label();
        private Label address3Caption = new Label();
        private Label cityCaption = new Label();
        private Label countryCaption = new Label();
        private Label regionCaption = new Label();
        private Label postalCodeCaption = new Label();

        private Label address1DisplayOnly = new Label();
        private Label address2DisplayOnly = new Label();
        private Label address3DisplayOnly = new Label();
        private Label cityDisplayOnly = new Label();
        private Label countryDisplayOnly = new Label();
        private Label regionDisplayOnly = new Label();
        private Label postalCodeDisplayOnly = new Label();

        #endregion

        #region Fields

        private TextBox address1 = new TextBox();
        private TextBox address2 = new TextBox();
        private TextBox address3 = new TextBox();
        private TextBox city = new TextBox();
        private DropDownList countries = new DropDownList();
        private DropDownList regions = new DropDownList();
        private TextBox postalCode = new TextBox();
        private TextBoxWatermarkExtender optionalAddress2 = new TextBoxWatermarkExtender();
        private TextBoxWatermarkExtender optionalAddress3 = new TextBoxWatermarkExtender();
        private HtmlGenericControl validatorContainer = new HtmlGenericControl();

        #endregion

        #endregion

        #region Overrides

        protected override void CreateChildControls()
        {
            SetControlIds();
            SetDefaultLabelProperties();
            SetDefaultFieldProperties();

            switch (Language.CurrentLanguage.LanguageCode.ToLower())
            {
                // TODO: Where to put supported cultures????
                case "ja-jp":
                case "zh-chs":
                    if (!DisplayOnly)
                        {
                            LoadControlsInAlternateOrder();
                        }
                        else
                        {
                            LoadControlsInDisplayModeAlternateOrder();
                        }
                    break;
                default:
                    if (!DisplayOnly)
                    {
                        LoadControlsDefaultOrder();
                    }
                    else
                    {
                        LoadControlsInDisplayModeDefaultOrder();
                    }
                    break;
            }

            countries.DataSource = this.CountryData;
            regions.DataSource = this.RegionData;

            EnsureChildControls();
            this.DataBind();
        }
/*
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if(Page != null)
                Page.Validators.Add(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if(Page != null)
                Page.Validators.Remove(this);
        }
*/
        #endregion

        #region Private Methods

        private void LoadControlsInDisplayModeAlternateOrder()
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Controls.Add(countryDisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(postalCodeDisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(regionDisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(cityDisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(address1DisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(address2DisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(address3DisplayOnly);
            Controls.Add(div);
        }

        public void LoadControlsInDisplayModeDefaultOrder()
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Controls.Add(address1DisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(address2DisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(address3DisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(cityDisplayOnly);
            div.Controls.Add(regionDisplayOnly);
            div.Controls.Add(new LiteralControl("&nbsp;"));
            div.Controls.Add(postalCodeDisplayOnly);
            Controls.Add(div);

            div = new HtmlGenericControl("div");
            div.Controls.Add(countryDisplayOnly);
            Controls.Add(div);
        }

        private void LoadControlsDefaultOrder()
        {
            HtmlTableRow tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            HtmlTableCell td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address1Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(validatorContainer);
            td.Controls.Add(address1);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address2Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(address2);
            td.Controls.Add(optionalAddress2);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address3Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(address3);
            td.Controls.Add(optionalAddress3);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(cityCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(city);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(countryCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(countries);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(regionCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(regions);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(postalCodeCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(postalCode);
            tr.Controls.Add(td);
            Controls.Add(tr);
        }

        private void LoadControlsInAlternateOrder()
        {
            HtmlTableRow tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            HtmlTableCell td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(countryCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(validatorContainer);
            td.Controls.Add(countries);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(postalCodeCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(postalCode);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(regionCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(regions);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(cityCaption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(city);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address1Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(address1);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address2Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(address2);
            td.Controls.Add(optionalAddress2);
            tr.Controls.Add(td);
            Controls.Add(tr);

            tr = new HtmlTableRow();
            tr.EnableViewState = false;
            tr.Attributes["class"] = RowCssClass;
            td = new HtmlTableCell();
            td.Attributes["class"] = LabelsCssClass;
            td.Controls.Add(address3Caption);
            tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = FormFieldsCssClass;
            td.Controls.Add(address3);
            td.Controls.Add(optionalAddress3);
            tr.Controls.Add(td);
            Controls.Add(tr);
        }
/*
        private void CreateBeginDiv()
        {
            this.Controls.Add(new LiteralControl(BEGIN_DIV));
        }

        private void CreateBeginDivFormat(string cssClass)
        {
            this.Controls.Add(new LiteralControl(string.Format(BEGIN_DIV_FORMAT, cssClass)));
        }

        private void CreateEndDiv()
        {
            this.Controls.Add(new LiteralControl(END_DIV));
        }

        private void CreateBeginRow()
        {
            this.Controls.Add(new LiteralControl(BEGIN_ROW));
        }

        private void CreateBeginRowFormat(string cssClass)
        {
            this.Controls.Add(new LiteralControl(string.Format(BEGIN_ROW_FORMAT, cssClass)));
        }

        private void CreateEndRow()
        {
            this.Controls.Add(new LiteralControl(END_ROW));
        }

        private void CreateBeginColumn()
        {
            this.Controls.Add(new LiteralControl(BEGIN_COLUMN));
        }

        private void CreateBeginColumnFormat(string cssClass)
        {
            this.Controls.Add(new LiteralControl(string.Format(BEGIN_COLUMN_FORMAT, cssClass)));
        }

        private void CreateEndColumn()
        {
            this.Controls.Add(new LiteralControl(END_COLUMN));
        }
*/
        private void SetControlIds()
        {
            address1Caption.ID = "address1Caption";
            address2Caption.ID = "address2Caption";
            address3Caption.ID = "address3Caption";
            cityCaption.ID = "cityCaption";
            countryCaption.ID = "countryCaption";
            regionCaption.ID = "regionCaption";
            postalCodeCaption.ID = "postalCodeCaption";

            address1.ID = "address1";
            address2.ID = "address2";
            address3.ID = "address3";
            city.ID = "city";
            countries.ID = "countries";
            regions.ID = "regions";
            postalCode.ID = "postalCode";

            optionalAddress2.ID = "optionalAddress2";
            optionalAddress3.ID = "optionalAddress3";

            validatorContainer.ID = "validatorContainer";
        }

        private void SetDefaultFieldProperties()
        {
            address1.MaxLength = ADDRESS_MAX_LENGTH;
            address1.EnableClientScript = false;
            
            address2.MaxLength = ADDRESS_MAX_LENGTH;
            
            address3.MaxLength = ADDRESS_MAX_LENGTH;
            
            city.MaxLength = CITY_MAX_LENGTH;
            city.EnableClientScript = false;

            countries.SelectedIndexChanged += new EventHandler(Countries_SelectedIndexChanged);
            countries.EnableViewState = true;
            countries.AutoPostBack = true;
            countries.DataTextField = "ContentValue";
            countries.DataValueField = "Key";
            countries.PromptText = this.DropdownRequiredText;
            countries.EnableClientScript = false;

            optionalAddress2.TargetControlID = address2.ID;
            optionalAddress2.WatermarkCssClass = this.OptionFieldCssClass;
            optionalAddress2.WatermarkText = this.OptionalText;

            optionalAddress3.TargetControlID = address3.ID;
            optionalAddress3.WatermarkCssClass = this.OptionFieldCssClass;
            optionalAddress3.WatermarkText = this.OptionalText;
            
            regions.EnableViewState = true;
            regions.DataTextField = "ContentValue";
            regions.DataValueField = "Key";
            regions.PromptText = this.DropdownOptionalText;
            regions.EnableClientScript = false;
            
            postalCode.MaxLength = POSTALCODE_MAX_LENGTH;
            postalCode.EnableClientScript = false;
            postalCode.ValidateControl = true;

            validatorContainer.Attributes["class"] = "displayNone";
        }

        private void SetDefaultLabelProperties()
        {
            address1Caption.Text = this.Address1Caption;
            address2Caption.Text = this.Address2Caption;
            address3Caption.Text = this.Address3Caption;
            cityCaption.Text = this.CityCaption;
            countryCaption.Text = this.CountryCaption;
            regionCaption.Text = this.RegionCaption;
            postalCodeCaption.Text = this.PostalCodeCaption;
        }

        #endregion

        #region Form Label Properties

        [Category("Appearance"), 
        Description("Sets the Caption For Address1"), 
        DefaultValue(ADDRESS1_CAPTION_DEFAULT), 
        Localizable(true)]
        public string Address1Caption
        {
            get
            {
                
                if (ViewState[VS_TAG_ADDRESS1_CAPTION] == null)
                    return ADDRESS1_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_ADDRESS1_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_ADDRESS1_CAPTION] = value;
                this.ChildControlsCreated = false;
                
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For Address2"), 
        DefaultValue(ADDRESS2_CAPTION_DEFAULT),
        Localizable(true)]
        public string Address2Caption
        {
            get
            {
                if (ViewState[VS_TAG_ADDRESS2_CAPTION] == null)
                    return ADDRESS2_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_ADDRESS2_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_ADDRESS2_CAPTION] = value;
                this.ChildControlsCreated = false;
                
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For Address3"), 
        DefaultValue(ADDRESS3_CAPTION_DEFAULT), 
        Localizable(true)]
        public string Address3Caption
        {
            get
            {

                if (ViewState[VS_TAG_ADDRESS3_CAPTION] == null)
                    return ADDRESS3_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_ADDRESS3_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_ADDRESS3_CAPTION] = value;
                this.ChildControlsCreated = false;
                
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For City"), 
        DefaultValue(CITY_CAPTION_DEFAULT), 
        Localizable(true)]
        public string CityCaption
        {
            get
            {

                if (ViewState[VS_TAG_CITY_CAPTION] == null)
                    return CITY_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_CITY_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_CITY_CAPTION] = value;
                this.ChildControlsCreated = false;
                
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For Country"), 
        DefaultValue(COUNTRY_CAPTION_DEFAULT),
        Localizable(true)]
        public string CountryCaption
        {
            get
            {

                if (ViewState[VS_TAG_COUNTRY_CAPTION] == null)
                    return COUNTRY_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_COUNTRY_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_COUNTRY_CAPTION] = value;
                this.ChildControlsCreated = false;
                
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For Region"), 
        DefaultValue(REGION_CAPTION_DEFAULT),
        Localizable(true)]
        public string RegionCaption
        {
            get
            {

                if (ViewState[VS_TAG_REGION_CAPTION] == null)
                    return REGION_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_REGION_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_REGION_CAPTION] = value;
                this.ChildControlsCreated = false;
            }
        }

        [Category("Appearance"), 
        Description("Sets the Caption For PostalCode"), 
        DefaultValue(POSTALCODE_CAPTION_DEFAULT), 
        Localizable(true)]
        public string PostalCodeCaption
        {
            get
            {
                
                if (ViewState[VS_TAG_POSTALCODE_CAPTION] == null)
                    return POSTALCODE_CAPTION_DEFAULT;

                return (string)ViewState[VS_TAG_POSTALCODE_CAPTION];
            }
            set
            {
                ViewState[VS_TAG_POSTALCODE_CAPTION] = value;
                this.ChildControlsCreated = false;
               
            }
        }

        [Category("Appearance"),
        Description("Sets the optional text for Textboxes"),
        DefaultValue(""),
       Localizable(true)]
        public string OptionalText
        {
            get
            {

                if (ViewState[VS_TAG_OPTIONAL] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_OPTIONAL];
            }
            set
            {
                ViewState[VS_TAG_OPTIONAL] = value;
                this.ChildControlsCreated = false;

            }
        }

        [Category("Appearance"),
        Description("Sets the required Select One text for a dropdown"),
        DefaultValue(""),
       Localizable(true)]
        public string DropdownRequiredText
        {
            get
            {

                if (ViewState[VS_TAG_SELECT_REQUIRED] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_SELECT_REQUIRED];
            }
            set
            {
                ViewState[VS_TAG_SELECT_REQUIRED] = value;
                this.ChildControlsCreated = false;

            }
        }

        [Category("Appearance"),
        Description("Sets the required Select One text for a dropdown"),
        DefaultValue(""),
        Localizable(true)]
        public string DropdownOptionalText
        {
            get
            {

                if (ViewState[VS_TAG_SELECT_OPTIONAL] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_SELECT_OPTIONAL];
            }
            set
            {
                ViewState[VS_TAG_SELECT_OPTIONAL] = value;
                this.ChildControlsCreated = false;

            }
        }
        #endregion

        #region Error Messages


        [Category("Error Messages"),
        Description("Sets the Validation Group For The Control"),
        DefaultValue("")
        ]
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

        [Category("Error Messages"),
        Description("Sets the Error Message For PostalCode"),
        DefaultValue(""),
        Localizable(true)]
        public string PostalCodeErrorMessage
        {
            get
            {

                if (ViewState[VS_TAG_ERROR_POSTALCODE] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_POSTALCODE];
            }
            set
            {
                ViewState[VS_TAG_ERROR_POSTALCODE] = value;

            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For City"),
        DefaultValue(""),
        Localizable(true)]
        public string CityErrorMessage
        {
            get
            {

                if (ViewState[VS_TAG_ERROR_CITY] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_CITY];
            }
            set
            {
                ViewState[VS_TAG_ERROR_CITY] = value;

            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Country"),
        DefaultValue(""),
        Localizable(true)]
        public string CountryErrorMessage
        {
            get
            {

                if (ViewState[VS_TAG_ERROR_COUNTRY] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_COUNTRY];
            }
            set
            {
                ViewState[VS_TAG_ERROR_COUNTRY] = value;

            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Region"),
        DefaultValue(""),
        Localizable(true)]
        public string RegionErrorMessage
        {
            get
            {
                if (ViewState[VS_TAG_ERROR_REGION] == null)
                    return String.Empty;

                return (string)ViewState[VS_TAG_ERROR_REGION];
            }
            set
            {
                ViewState[VS_TAG_ERROR_REGION] = value;
            }
        }

        [Category("Error Messages"),
        Description("Sets the Error Message For Address1"),
        DefaultValue(""),
        Localizable(true)]
        public string Address1ErrorMessage
        {
            get
            {

                if (ViewState[VS_TAG_ERROR_ADDRESS1] == null)
                    return String.Empty;
                return (string)ViewState[VS_TAG_ERROR_ADDRESS1];
            }
            set
            {
                ViewState[VS_TAG_ERROR_ADDRESS1] = value;
            }
        }

        #endregion

        #region Style Properties

        [Category("Styles"),
        Description("Sets the default Css For Rows"),
        DefaultValue(ROWSTYLE_DEFAULT)]
        public string RowCssClass
        {
            get
            {
                if (ViewState[VS_TAG_ROWSTYLE] == null)
                    return ROWSTYLE_DEFAULT;

                return (string)ViewState[VS_TAG_ROWSTYLE];
            }
            set
            {
                ViewState[VS_TAG_ROWSTYLE] = value;
            }
        }

        [Category("Styles"), 
        Description("Sets the default Css For Labels"), 
        DefaultValue(LABELSTYLE_DEFAULT)]
        public string LabelsCssClass
        {
            get
            {
                if (ViewState[VS_TAG_LABELSTYLE] == null)
                    return LABELSTYLE_DEFAULT;

                return (string)ViewState[VS_TAG_LABELSTYLE];
            }
            set
            {
                ViewState[VS_TAG_LABELSTYLE] = value;
            }
        }

        [Category("Styles"), 
        Description("Sets the default Css For Labels"), 
        DefaultValue(FORMFIELDSTYLE_DEFAULT)]
        public string FormFieldsCssClass
        {
            get
            {
                if (ViewState[VS_TAG_FORMFIELDSTYLE] == null)
                    return FORMFIELDSTYLE_DEFAULT;

                return (string)ViewState[VS_TAG_FORMFIELDSTYLE];
            }
            set
            {
                ViewState[VS_TAG_FORMFIELDSTYLE] = value;
            }
        }

        [Category("Styles"),
            Description("Sets the default Css For Optional Fields"),
            DefaultValue("")]
        public string OptionFieldCssClass
        {
            get
            {
                if (ViewState[VS_TAG_OPTIONALSTYLE] == null)
                    return "";

                return (string)ViewState[VS_TAG_OPTIONALSTYLE];
            }
            set
            {
                ViewState[VS_TAG_OPTIONALSTYLE] = value;
            }
        }

        [Category("Styles"), 
        Description("Determines whether the address is just display only"),
        DefaultValue(false)]
        public bool DisplayOnly
        {
            get
            {
                if (ViewState[VS_TAG_DISPLAYONLY] == null)
                    return false;

                return (bool)ViewState[VS_TAG_DISPLAYONLY];
            }
            set
            {
                ViewState[VS_TAG_DISPLAYONLY] = value;
            }
        }

        #endregion

        public void LoadAddressFromMemberAddress(MemberAddress memberAddress)
        {
            if (memberAddress == null)
            {
                ClearAddress();
            }
            else
            {
                this.regions.ClearSelection();
                this.countries.ClearSelection();
                this.Address1 = memberAddress.Address1;
                this.Address2 = memberAddress.Address2;
                this.Address3 = memberAddress.Address3;
                this.City = memberAddress.City;
                this.Country = memberAddress.CountryCode;
                this.Region = memberAddress.RegionCode;
                this.PostalCode = memberAddress.PostalCode;
            }
        }

        #region Text Properties
        [Category("Address Data"), Description("Sets the Text For Address1"), DefaultValue("")]
        [PropertyControlMapper("address1")]
        public string Address1
        {
            get
            {
                this.EnsureChildControls();
                return address1.Text;
            }
            set
            {
                this.EnsureChildControls();
                this.address1.Text = value;
                this.address1DisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);

            }
        }

        [Category("Address Data"), Description("Sets the Text For Address2"), DefaultValue("")]
        [PropertyControlMapper("address2")]
        public string Address2
        {
            get
            {
                this.EnsureChildControls();
                return address2.Text;
            }
            set
            {
                this.EnsureChildControls();
                address2.Text = value;
                this.address2DisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);
                this.address2DisplayOnly.Visible = !string.IsNullOrEmpty(value);
            }
        }

        [Category("Address Data"), Description("Sets the Text For Address3"), DefaultValue("")]
        [PropertyControlMapper("address3")]
        public string Address3
        {
            get
            {
                this.EnsureChildControls();
                return address3.Text;
            }
            set
            {
                this.EnsureChildControls();
                address3.Text = value;
                this.address3DisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);
                this.address3DisplayOnly.Visible = !string.IsNullOrEmpty(value);
            }
        }

        [Category("Address Data"), Description("Sets the Text For City"), DefaultValue("")]
        [PropertyControlMapper("city")]
        public string City
        {
            get
            {
                this.EnsureChildControls();
                return city.Text;
            }
            set
            {
                this.EnsureChildControls();
                city.Text = value;
                this.cityDisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);
            }
        }

        [Category("Address Data"), Description("Sets the Text For Country"), DefaultValue("")]
        [PropertyControlMapper("countries")]
        public string Country
        {
            get
            {
                this.EnsureChildControls();
                if (!this.DisplayOnly)
                {
                    return countries.SelectedValue;
                }
                else
                {
                    return countryDisplayOnly.Text;
                }
            }
            set
            {
                this.EnsureChildControls();
                if (!string.IsNullOrEmpty(value))
                {
                    countries.SelectedValue = value;
                }
                this.countryDisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);
            }
        }

        [Category("Address Data"), Description("Sets the Text For Region"), DefaultValue("")]
        [PropertyControlMapper("regions")]
        public string Region
        {
            get
            {
                this.EnsureChildControls();
                if (!this.DisplayOnly)
                {
                    return regions.SelectedValue;
                }
                else
                {
                    return regionDisplayOnly.Text;
                }
            }
            set
            {
                this.EnsureChildControls();
                if (DisplayOnly && !string.IsNullOrEmpty(value))
                {
                    if (Language.CurrentLanguage == Language.Japanese ||
                        Language.CurrentLanguage == Language.ChineseSimplified)
                    {
                        regionDisplayOnly.Text = value;
                    }
                    else
                    {
                        regionDisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(string.Concat(", ", value));
                    }
                }
                else if (regions.Enabled)
                {
                    regions.SelectedValue = value;
                }
            }
        }

        [Category("Address Data"), Description("Sets the Text For PostalCode"), DefaultValue("")]
        [PropertyControlMapper("postalCode")]
        public string PostalCode
        {
            get
            {
               this.EnsureChildControls();
               return postalCode.Text;
            }
            set
            {
               this.EnsureChildControls();
               postalCode.Text = value;
               postalCodeDisplayOnly.Text = System.Web.HttpUtility.HtmlEncode(value);
            }
        }

        #endregion

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
                    return true;
                return (bool)ViewState[VS_TAG_VALID_CONTROL];
            }
            set
            {
                ViewState[VS_TAG_VALID_CONTROL] = value;
            }
        }

        public void Validate()
        {
            this.ErrorMessage = String.Empty;

            if (DisplayOnly)
            {
                this.IsValid = true;
                return;
            }

            bool result = true;

            if (this.address1.Text.Length == 0)
            {
                this.ErrorMessage += this.Address1ErrorMessage + CreateBreak();
                result = false;
            }

            if (this.city.Text.Length == 0)
            {
                this.ErrorMessage += this.CityErrorMessage + CreateBreak();
                result = false;
            }

            if (String.IsNullOrEmpty(postalCode.Text) || !IsPostalCodeValid())
            {
                this.ErrorMessage += this.PostalCodeErrorMessage + CreateBreak();
                result = false;
            }

            if (this.countries.SelectedValue.Length == 0)
            {
                this.ErrorMessage += this.CountryErrorMessage + CreateBreak();
                result = false;
            }

            if (this.regions.Items.Count > 1 && this.regions.SelectedValue.Length == 0)
            {
                this.ErrorMessage += this.RegionErrorMessage + CreateBreak();
                result = false;
            }

            this.IsValid = result;
        }

        private bool IsPostalCodeValid()
        {
            string zipcodeRegex = @"^\d{5}$|^\d{5}-\d{4}$";
            
            if (countries.SelectedValue == COUNTRY_US && !Regex.IsMatch(postalCode.Text, zipcodeRegex))
                return false;

            if(countries.SelectedValue == COUNTRY_HONGKONG)
            {
                if (String.Compare(postalCode.Text, POSTALCODE_HK) != 0)
                    return false;
                }

                return true;
        }

        private string CreateBreak()
        {
            return "<br />";
        }
*/
        public object CountryData
        {
            get
            {
                this.EnsureChildControls();
                return ViewState[VS_TAG_COUNTRY_DATA];
            }
            set
            {
                ViewState[VS_TAG_COUNTRY_DATA] = value;
            }
        }

        public object RegionData
        {
            get
            {
                this.EnsureChildControls();
                List<ContentItem> items = ViewState[VS_TAG_REGION_DATA] as List<ContentItem>;
                if (items == null || items.Count == 0)
                {
                    this.regions.Enabled = false;
                    this.regions.PromptText = this.DropdownOptionalText;
                }
                else
                {
                    this.regions.Enabled = true;
                    this.regions.PromptText = this.DropdownRequiredText;
                }

                return ViewState[VS_TAG_REGION_DATA];
            }

            set
            {
            	ViewState[VS_TAG_REGION_DATA] = value;
                List<ContentItem> items = ViewState[VS_TAG_REGION_DATA] as List<ContentItem>;
                if (items == null || items.Count == 0)
                {
                    this.regions.Enabled = false;
                    this.regions.PromptText = this.DropdownOptionalText;
                }
                else
                {
                    this.regions.Enabled = true;
                    this.regions.PromptText = this.DropdownRequiredText;
                }
                this.ChildControlsCreated = false;
            }
        }

        private void SetPostalValidation()
        {
            if (countries.SelectedValue == COUNTRY_HONGKONG)
            {
                this.PostalCode = POSTALCODE_HK;
                this.postalCode.Enabled = false;
            }
            else
            {
                if (this.postalCode.Text == POSTALCODE_HK)
                    this.PostalCode = String.Empty;

                this.postalCode.Enabled = true;
            }
        }

        protected void Countries_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Country = countries.SelectedValue;

            if (CountryDataChange != null)
            {
                CountryDataChange(sender, e);
            }

            SetPostalValidation();

            this.ChildControlsCreated = false;
            this.Country = countries.SelectedValue;
        }

        #endregion

        public void ClearAddress()
        {
            this.RegionData = null;
            this.Address1 = String.Empty;
            this.Address2 = String.Empty;
            this.Address3 = String.Empty;
            this.City = String.Empty;
            this.Region = String.Empty;
            this.Country = String.Empty;
            this.PostalCode = String.Empty;
        }

        public MemberAddress ExportAddress()
        {
            MemberAddress address = new MemberAddress();
            address.Address1 = address1.Text;
            address.Address2 = address2.Text;
            address.Address3 = address3.Text;
            address.City = city.Text;
            address.RegionCode = Region;
            address.CountryCode = Country;
            address.PostalCode = postalCode.Text;

            return address;
        }

        #region IViewPropertyValidator Members

        public void SetValidationError<T>(
            string invalidControlName, 
            T errorEnumValue, 
            bool showInSummary, 
            bool showHilite)
        {
            System.Web.UI.Control invalidControl = null;
            switch (invalidControlName)
            {
                case "address1":
                    invalidControl = address1 as System.Web.UI.Control;
                    break;
                case "address2":
                    invalidControl = address2 as System.Web.UI.Control;
                    break;
                case "address3":
                    invalidControl = address3 as System.Web.UI.Control;
                    break;
                case "city":
                    invalidControl = city as System.Web.UI.Control;
                    break;
                case "countries":
                    invalidControl = countries as System.Web.UI.Control;
                    break;
                case "regions":
                    invalidControl = regions as System.Web.UI.Control;
                    break;
                case "postalCode":
                    invalidControl = postalCode as System.Web.UI.Control;
                    break;
                default:
                    // N/A
                    break;
            }

            if (invalidControl != null)
            {
                IValidationHubErrorSetter parent = this.Page as IValidationHubErrorSetter;
                if (parent != null)
                {
                    parent.SetValidationHubError<T>(
                        invalidControl,
                        errorEnumValue,
                        showInSummary,
                        showHilite);
                }
            }
        }

        #endregion
    }
}
