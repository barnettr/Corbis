using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Corbis.Web.Validation.Integration.AspNet;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;

namespace Corbis.Web.UI.Controls
{
    public class ValidationGroupSummary : ValidationSummary
    {
        bool renderUplevel = false;
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string[] errorMessages;
            bool flag;
            if (base.DesignMode)
            {
                errorMessages = new string[] { "ValSummary_error_message_1","ValSummary_error_message_2" };
                flag = true;
                this.renderUplevel = false;
            }
            else
            {
                if (!this.Enabled)
                {
                    return;
                }
                errorMessages = this.GetErrorMessages();
                flag = this.ShowSummary;
                if (!flag && this.renderUplevel)
                {
                    base.Style["display"] = "none";
                }
            }
            if (this.Page != null)
            {
                this.Page.VerifyRenderingInServerForm(this);
            }
            bool flag3 = this.renderUplevel || flag;
            if (flag3)
            {
                this.RenderBeginTag(writer);
            }
            if (flag)
            {
                string str;
                string str2;
                string str3;
                string str4;
                string str5;
                switch (this.DisplayMode)
                {
                    case ValidationSummaryDisplayMode.List:
                        str = "b";
                        str2 = string.Empty;
                        str3 = string.Empty;
                        str4 = "b";
                        str5 = string.Empty;
                        break;

                    case ValidationSummaryDisplayMode.SingleParagraph:
                        str = " ";
                        str2 = string.Empty;
                        str3 = string.Empty;
                        str4 = " ";
                        str5 = "b";
                        break;

                    default:
                        str = string.Empty;
                        str2 = "<div class=\"ValidationSummary\"><ul>";
                        str3 = "<li>";
                        str4 = "</li>";
                        str5 = "</ul></div>";
                        break;
                }
                if (this.HeaderText.Length > 0)
                {
                    writer.Write(this.HeaderText);
                    this.WriteBreakIfPresent(writer, str);
                }
                if (errorMessages != null)
                {
                    writer.Write(str2);
                    for (int i = 0; i < errorMessages.Length; i++)
                    {
                        writer.Write(str3);
                        writer.Write(errorMessages[i]);
                        this.WriteBreakIfPresent(writer, str4);
                    }
                    this.WriteBreakIfPresent(writer, str5);
                }
            }
            if (flag3)
            {
                this.RenderEndTag(writer);
            }

        }

        private void WriteBreakIfPresent(HtmlTextWriter writer, string text)
        {
            if (text == "b")
            {
                writer.WriteBreak();
            }
            else
            {
                writer.Write(text);
            }
        }

        private bool highlightRows = false;
        public bool HighlightRows
        {
            set { highlightRows = value; }
            get { return highlightRows; }
        }

        public string[] GetErrorMessages()
        {
            string[] strArray = null;
            
            int num = 0;
            ValidatorCollection validators = this.Page.GetValidators(this.ValidationGroup);

            for (int i = 0; i < this.Page.Validators.Count;i++ )
            {
                IGroupValidator groupValidator = this.Page.Validators[i] as IGroupValidator;
                if (groupValidator != null && groupValidator.ValidationGroup == this.ValidationGroup)
                {
                    validators.Add((IValidator)groupValidator);

                }
            }
        

            for (int i = 0; i < validators.Count; i++)
            {
                IValidator validator = validators[i];
                if (!validator.IsValid)
                {
                    if (validator.ErrorMessage.Length != 0)
                    {
                        num++;
                    }

                    if (this.HighlightRows)
                    {
						BaseValidator v = validators[i] as BaseValidator;
                        if (v != null)
                        {
                            try
                            {
                                Control c = this.Parent.FindControl(v.ControlToValidate).Parent;
                                while (c != null && !(
                                            (c is HtmlTableRow &&
                                                (
                                                    ((HtmlTableRow)c).Attributes["class"].StartsWith("Error") ||
                                                    ((HtmlTableRow)c).Attributes["class"].StartsWith("FormRow")
                                                )
                                            ) ||
                                            (c is HtmlGenericControl &&
                                                (
                                                    ((HtmlGenericControl)c).Attributes["class"].StartsWith("Error") ||
                                                    ((HtmlGenericControl)c).Attributes["class"].StartsWith("FormRow")
                                                )
                                            )
                                        )
                                    )c = c.Parent;
                                if (c != null)
                                {
                                    if (c is HtmlTableRow)
                                    {
                                        
                                        HtmlTableRow tr = (HtmlTableRow)c;
                                        //string className = tr.Attributes["class"];
                                        //string replace = className.StartsWith("Error") ? "Error" : "FormRow";
                                        tr.Attributes["class"] = v.IsValid ? "FormRow" : "ErrorHighlight FormRow";
                                    }
                                    else if (c is HtmlGenericControl)
                                    {
                                        HtmlGenericControl div = (HtmlGenericControl)c;
                                        string className = div.Attributes["class"];
                                        div.Attributes["class"] = className.Replace("FormRow", v.IsValid ? "FormRow" : "ErrorHighlight");
                                    }
                                }
                            }
                            catch (Exception)
                            { }

                        }
                    }
                }
            }
            if (num != 0)
            {
                strArray = new string[num];
                int index = 0;
                for (int j = 0; j < validators.Count; j++)
                {
                    IValidator validator2 = validators[j];
                    if ((!validator2.IsValid && (validator2.ErrorMessage != null)) && (validator2.ErrorMessage.Length != 0))
                    {
                        strArray[index] = string.Copy(validator2.ErrorMessage);
                        index++;
                    }
                }
            }
            return strArray;

        }
    }
}
