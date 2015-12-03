using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.WCSFExtensions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using Corbis.Web.Validation.Integration.AspNet;

namespace Corbis.Web.UI.Controls
{
    public class TextBox : System.Web.UI.WebControls.TextBox, INamingContainer
    {
        private bool allowPaste = true;
        public bool AllowPaste
        {
            get { return allowPaste; }
            set { allowPaste = value; }
        }

        private ValidatorDisplay display = ValidatorDisplay.Dynamic;
        public ValidatorDisplay Display
        {
            get { return display; }
            set { display = value; }
        }
        
        private bool enableViewState = false;
        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        private bool enableClientScript = true;
        public bool EnableClientScript
        {
            get { return enableClientScript; }
            set { enableClientScript = value; }
        }

        private string errorCssClass = "Error";
        public string ErrorCssClass
        {
            get { return errorCssClass; }
            set { errorCssClass = value; }
        }

        private string validationPropertyName;
        public string ValidationPropertyName
        {
            get { return validationPropertyName; }
            set { validationPropertyName = value; }
        }

        private bool validateControl = false;
        public bool ValidateControl
        {
            get { return validateControl; }
            set { validateControl = value; }
        }
        
        public string validate
        {
            get { return this.Attributes["validate"]; }
            set { this.Attributes["validate"] = value; }
        }

		private bool validateAjax = false;
		public bool ValidateAjax
		{
			get { return validateAjax; }
			set { validateAjax = value; }
		}

		private string validatorContainer = null;
		public string ValidatorContainer
		{
			get { return validatorContainer; }
			set { validatorContainer = value; }
		}

		private string validationObjectType;
        public string ValidationObjectType
        {
            get { return validationObjectType; }
            set { validationObjectType = value; }
        }

		private string validationRulesetName;
		public string ValidationRulesetName
        {
			get { return validationRulesetName; }
			set { validationRulesetName = value; }
        }

        private string watermarkText;
        public string WatermarkText
        {
            get { return watermarkText; }
            set { watermarkText = value; }
        }

        private string watermarkCssClass;
        public string WatermarkCssClass
        {
            get { return watermarkCssClass; }
            set { watermarkCssClass = value; }
        }

    	private PropertyProxyValidator propertyProxyValidator;
		public PropertyProxyValidator PropertyProxyValidator
		{
			get { return propertyProxyValidator; }
		}

		private ParameterProxyValidator parameterProxyValidator;
		public ParameterProxyValidator ParameterProxyValidator
		{
			get { return parameterProxyValidator; }
		}

		private ServerSideValidationExtender serverSideValidationExtender;
		public ServerSideValidationExtender ServerSideValidationExtender
		{
			get { return serverSideValidationExtender; }
		}

		private string validationOperationName;
		public string ValidationOperationName
		{
			get { return validationOperationName; }
			set { validationOperationName = value; }
		}

		private string validationParameterName;
		public string ValidationParameterName
		{
			get { return validationParameterName; }
			set { validationParameterName = value; }
		}

        private string validationParameterTypes = null;
        public string ValidationParameterTypes
        {
            get { return validationParameterTypes; }
            set { validationParameterTypes = value; }
        }

        private string fieldDescription = string.Empty;
        public string FieldDescription
        {
            get { return fieldDescription; }
            set { fieldDescription = value; }
        }

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			//See if we need to add validator controls for this textbox
			if (this.ValidateControl && this.Enabled && this.Visible)
			{
				WebControl webValidator = null;

				//TODO: Add control caching.
				//Getting the parameter validator 
				if (!string.IsNullOrEmpty(this.validationOperationName) && !string.IsNullOrEmpty(this.validationParameterName))
				{
					parameterProxyValidator = new ParameterProxyValidator();
					parameterProxyValidator.ControlToValidate = this.ID;
					parameterProxyValidator.SourceTypeName = this.ValidationObjectType;
					parameterProxyValidator.OperationName = this.ValidationOperationName;
					parameterProxyValidator.ParameterName = this.validationParameterName;
					parameterProxyValidator.ParameterTypes = this.ValidationParameterTypes;
					parameterProxyValidator.Display = this.Display;
					parameterProxyValidator.CssClass = this.ErrorCssClass;
					parameterProxyValidator.ID = this.ID + "ProxyValidator";
					parameterProxyValidator.Enabled = this.Enabled;
					parameterProxyValidator.EnableClientScript = this.EnableClientScript;
                    parameterProxyValidator.ValidationGroup = this.ValidationGroup;
                    webValidator = (WebControl)parameterProxyValidator;
				}
				//Getting the property validator
				else if (!string.IsNullOrEmpty(this.ValidationPropertyName))
				{
					propertyProxyValidator = new PropertyProxyValidator();
					propertyProxyValidator.ControlToValidate = this.ID;
					propertyProxyValidator.PropertyName = this.ValidationPropertyName;
					propertyProxyValidator.SourceTypeName = this.ValidationObjectType;
					propertyProxyValidator.RulesetName = this.ValidationRulesetName;
                    propertyProxyValidator.Display = this.Display;
					propertyProxyValidator.CssClass = this.ErrorCssClass;
					propertyProxyValidator.ID = this.ID + "ProxyValidator";
					propertyProxyValidator.Enabled = this.Enabled;
					propertyProxyValidator.EnableClientScript = this.EnableClientScript;
                    propertyProxyValidator.ValidationGroup = this.ValidationGroup;
                    propertyProxyValidator.Text = this.FieldDescription;
                    
					webValidator = (WebControl)propertyProxyValidator;
				}
                

				//Add the validator to the container
                if (ValidatorContainer != null && webValidator != null)
                {
                    Control container = FindControl(this, this.ValidatorContainer);
                    if (container != null)
                    {
                        container.Controls.Add(webValidator);
                    }
                }

				//See if we should add ajax validator.
				if (this.ValidateAjax && ValidatorContainer != null && webValidator != null)
				{
					serverSideValidationExtender = new ServerSideValidationExtender();
					serverSideValidationExtender.TargetControlID = webValidator.ID;
					serverSideValidationExtender.Enabled = this.Enabled;
                    Control container = FindControl(this, this.ValidatorContainer);
                    if (container != null)
                    {
                        container.Controls.Add(serverSideValidationExtender);
                    }
				}

			}
		}

		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				if (this.propertyProxyValidator != null)
					this.propertyProxyValidator.Enabled = value;
				if (this.ServerSideValidationExtender != null)
					this.ServerSideValidationExtender.Enabled = value;
			}
		}

		private Control FindControl(Control parentControl, string controlId)
		{
			Control returnControl = null;
			if (parentControl != null && controlId != null)
			{
				returnControl = parentControl.FindControl(controlId);
				if (returnControl == null)
				{
					returnControl = FindControl(parentControl.Parent, controlId);
				}
			}
			
			return returnControl;
		}

        protected override void OnPreRender(EventArgs e)
        {
            if (!allowPaste)
            {
                Attributes.Add("onkeydown", "return NoPasteKey(event)");
                Attributes.Add("onpaste", "return false");
                Attributes.Add("oncontextmenu", "return false");
            }
            if (!string.IsNullOrEmpty(watermarkText))
            {
                Attributes.Add("title", watermarkText);
                this.CssClass += " Watermarked";
                Attributes.Add("watermarkcss", watermarkCssClass);
            }
            base.OnPreRender(e);
        }

    }
}
