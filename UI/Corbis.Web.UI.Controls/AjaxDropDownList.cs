using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.ViewInterfaces;
using AjaxControlToolkit.WCSFExtensions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using System.Collections;

namespace Corbis.Web.UI.Controls
{
    public class AjaxDropDownList : System.Web.UI.WebControls.DropDownList
    {

        private bool enableViewState = false;
        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        private string entityType;
        [Description("The type of data that the control will display.")]
        public string EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }

        private string commandText;
        [Description("Text of ListItem to insert at position 0.")]
        public string CommandText
        {
            get { return commandText; }
            set { commandText = value; }
        }

        private string promptText;
        [Description("Text of ListItem to insert at position 0, supercedes CommandText.")]
        public string PromptText
        {
            get { return promptText; }
            set { promptText = value; }
        }

        private ValidatorDisplay display = ValidatorDisplay.Dynamic;
        public ValidatorDisplay Display
        {
            get { return display; }
            set { display = value; }
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

        private PropertyProxyValidator propertyProxyValidator;
        public PropertyProxyValidator PropertyProxyValidator
        {
            get { return propertyProxyValidator; }
        }
        private String dropDownType;
        public String DropDownType
        {
            get { return dropDownType; }
            set { dropDownType = value; }
        }
        private ServerSideValidationExtender serverSideValidationExtender;

        public ServerSideValidationExtender ServerSideValidationExtender
        {
            get { return serverSideValidationExtender; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //See if we need to add validator controls for this textbox
            if (this.ValidateControl && this.Enabled && this.Visible)
            {
                propertyProxyValidator = new PropertyProxyValidator();
                propertyProxyValidator.ControlToValidate = this.ID;
                propertyProxyValidator.EnableClientScript = this.EnableClientScript;
                propertyProxyValidator.PropertyName = this.ValidationPropertyName;
                propertyProxyValidator.SourceTypeName = this.ValidationObjectType;
                propertyProxyValidator.RulesetName = this.ValidationRulesetName;
                propertyProxyValidator.Display = this.Display;
                propertyProxyValidator.CssClass = this.ErrorCssClass;
                propertyProxyValidator.ID = this.ID + "ProxyValidator";
                propertyProxyValidator.Enabled = this.Enabled;
                propertyProxyValidator.ValidationGroup = this.ValidationGroup;

                if (this.ValidatorContainer != null)
                {
                    Control container = FindControl(this, this.ValidatorContainer);
                    if (container != null)
                    {
                        container.Controls.Add(propertyProxyValidator);
                    }
                }

                //See if we should add ajax validator.
                if (this.ValidateAjax)
                {
                    serverSideValidationExtender = new ServerSideValidationExtender();
                    serverSideValidationExtender.TargetControlID = propertyProxyValidator.ID;
                    serverSideValidationExtender.Enabled = this.Enabled;
                    Control container = FindControl(this, this.ValidatorContainer);
                    if (container != null)
                    {
                        container.Controls.Add(serverSideValidationExtender);
                    }
                }
            }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            if (!StringHelper.IsNullOrTrimEmpty(promptText))
            {
                if ((this.Items.Count == 0) || (this.Items.Count > 0 && this.Items[0].Text != promptText))
                {
                    this.Items.Insert(0, new ListItem(promptText, String.Empty));
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
               this.RenderContents(writer);
           
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



        public void BindDataWithActions(IEnumerable list)
        {
            this.DataSource = list;


            this.DataBind();



        }

        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                this.Items.Clear();
                if (!StringHelper.IsNullOrTrimEmpty(commandText))
                {
                    this.Items.Insert(0, new ListItem(commandText, String.Empty));
                }

                this.AppendDataBoundItems = true;
                base.DataSource = value;
            }
        }




    }
}
