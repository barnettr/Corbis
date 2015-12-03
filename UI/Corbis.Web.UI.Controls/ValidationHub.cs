using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Controls
{
    [ToolboxData("<{0}:ValidationHub runat=server></{0}:ValidationHub>")]
    public class ValidationHub : CompositeControl
    {
        #region Member Variables
        private bool _autoInit = true;
        private bool _hasSummaryErrors = false;
        private Dictionary<String,ErrorItem> _errors = new Dictionary<string,ErrorItem>();
        private const string ROWCLASS = "FormRow";
        private const string HILITECLASS = "FormRow ErrorHighlight";
        private string _uniqueName = String.Empty; 
        private bool _isPopup = true;
        private bool _isIModal = true;
        private string _popupID;
        private string _successScript;
        private string _failScript;
        private bool _highlightField;
        private string _resizeScript;
        private string _initScript;
        private bool _submitForm = false;
        private bool _submitByAjax = false;
        private bool _useStandardAjaxBehavior = false;
        private bool _useOldAjaxBehavior = false;
        private string _ajaxUrl;
        private string _onAjaxRequest;
        private string _containerID;
        private HtmlGenericControl _ul = new HtmlGenericControl("ul");
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Element id of container
        /// </summary>
        public string ContainerID
        {
            get { return _containerID; }
            set { _containerID = value; }
        }

        /// <summary>
        /// Use UniqueName when js vars need to be unique when more than one form exists 
        /// on a containing page. Credit card ascx is an example of this. Use a simple nickname.
        /// </summary>
        public string UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }

        /// <summary>
        /// Defaults to true. Is this a popup that needs to be resized, etc. (e.g., Sign In or Change Password)
        /// or within the body of a main page (e.g., Contact Customer Service).
        /// </summary>
        public bool IsPopup
        {
            get { return _isPopup; }
            set { _isPopup = value; }
        }

        /// <summary>
        /// Is popup an IModal. Defaults to false.
        /// </summary>
        public bool IsIModal
        {
            get { return _isIModal; }
            set { _isIModal = value; }
        }

        /// <summary>
        /// The id of the PopupContainer. Required if IsPopup=true.
        /// Used to pass into ResizeModal, HideModal, etc. See modalpopup.js
        /// </summary>
        public string PopupID
        {
            get { return _popupID; }
            set { _popupID = value; }
        }

        /// <summary>
        /// Script to run once the form has been successfully validated on the client. 
        /// Use either this or ClickButtonId
        /// </summary>
        public string SuccessScript
        {
            get { return _successScript; }
            set { _successScript = value; }
        }
        /// <summary>
        /// script to run once the form vailidation is failed on the client.
        /// </summary>
        public string FailScript
        {
            get { return _failScript; }
            set { _failScript = value; }
        }
        /// <summary>
        /// if the field is not under the row, and we can use this attribute to highlight the field
        /// </summary>
        public bool HighlightField
        {
            get { return _highlightField; }
            set { _highlightField = value; }
        }

        public string ResizeScript
        {
            get 
            {
                if (_resizeScript == null)

                    return String.Format("{0}", this.IsIModal ?
                        String.Format("parent.ResizeIModal('{0}', GetDocumentHeight())", this.PopupID) :
                        String.Format("ResizeModal('{0}')", this.PopupID)
                        );

                else return _resizeScript;
            }
            set { _resizeScript = value; }
        }

        /// <summary>
        /// Script to add to the init function after validation is initialized.
        /// </summary>
        public string InitScript
        {
            get { return _initScript; }
            set { _initScript = value; }
        }

        /// <summary>
        /// Triggers a postback by clicking an element you include (<asp:LinkButton CssClass="ValidateClickLB displayNone" runat="server" OnClick="lb_Click" />). This linkbutton is required.
        /// </summary>
        public bool SubmitForm
        {
            get { return _submitForm; }
            set { _submitForm = value; }
        }

        /// <summary>
        /// Use an ajax call after form has been validated on the client. Must define AjaxUrl and on OnAjaxRequest.
        /// </summary>
        public bool SubmitByAjax
        {
            get { return _submitByAjax; }
            set { _submitByAjax = value; }
        }

        /// <summary>
        /// Errors will be automatically dumped into the summary panel and element rows highlighted. 
        /// Must write an appropriate function to use OnAjaxRequest
        /// </summary>
        public bool UseStandardAjaxBehavior
        {
            get { return _useStandardAjaxBehavior; }
            set
            {
                _useStandardAjaxBehavior = value;
                if (value) SubmitByAjax = true;// setting this true should automagically set submitbyajax to true
            }
        }

        /// <summary>
        /// Old ajax behavior
        /// </summary>
        public bool UseOldAjaxBehavior
        {
            get { return _useOldAjaxBehavior; }
            set
            {
                _useOldAjaxBehavior = value;
                if (value) SubmitByAjax = true;// setting this true should automagically set submitbyajax to true
            }
        }

        /// <summary>
        /// The url of the wsdl function being called.
        /// </summary>
        public string AjaxUrl
        {
            get { return _ajaxUrl; }
            set { _ajaxUrl = value; }
        }

        /// <summary>
        /// Name of function being called. This is a function pointer so use OnAjaxRequest="myFunction"
        /// instead of OnAjaxRequest="myFunction()". See ClientInstanceVariableName comments for code sample
        /// </summary>
        public string OnAjaxRequest
        {
            get { return _onAjaxRequest; }
            set { _onAjaxRequest = value; }
        }

        /// <summary>
        /// This is the variable to use when setting the OnAjaxRequest function. Use inline asp in the function
        /// e.g., <%=myValidationHub.ClientInstanceVariableName %>.options.ajaxData = {
        //                'param1': $('<%=textbox1.ClientID %>').value,
        //                'param2': $('<%=textbox2.ClientID %>').value
        //                };
        /// </summary>
        public string ClientInstanceVariableName
        {
            get { return string.Format("_{0}Validation", this.UniqueName); }
        }

        /// <summary>
        /// Automatically initializes the script based on the usage params. Set to false when other scripts 
        /// will control how each form validation needs to init. We set this to false in checkout because other scripts
        /// control when each section is active. This prevents the control from setting itself to active when 
        /// the controls are hidden (setting focus to first element is problematic)
        /// </summary>
        public bool AutoInit
        {
            get { return _autoInit; }
            set { _autoInit = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Set an error from the view. Highlights the controls containing row and appends the message to the list.
        /// Just need the message and the control. Set message to empty to remove the error message.
        /// </summary>
        /// <param name="control">Control with error</param>
        /// <param name="message">Error Message</param>
        public void SetError(Control control, String message, bool showInSummary, bool showHilite)
        {
            if (_errors.ContainsKey(control.ClientID))
            {
                ErrorItem e = _errors[control.ClientID];
                e.ErrorMessage = message;
                e.ShowInSummary = showInSummary;
                e.ShowHighlight = showHilite;
            }
            else
            {
                _errors.Add(control.ClientID, new ErrorItem(control, message, showInSummary, showHilite));
            }
        }

        public void SetError(Control control, string message, bool show)
        {
            SetError(control, message, show, show);
        }

        public void SetError(Control control, string message)
        {
            SetError(control, message, true, true);
        }

        /// <summary>
        /// Removes the error messages from the error panel and unhighlights any highlighted rows.
        /// </summary>
        public void ResetErrors()
        {
            _errors.Clear();
        }
        #endregion

        #region Initialization / Control Rendering
        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        private void HighlightRow(Control c, bool hilite)
        { 
            // walk the dom until a div or tr class=FormRow is found
            while (c != null && !(
                        (c is HtmlTableRow &&
                            ((HtmlTableRow)c).Attributes["class"].StartsWith(ROWCLASS)
                        ) ||
                        (c is HtmlGenericControl &&
                            ((HtmlGenericControl)c).Attributes["class"].StartsWith(ROWCLASS)
                        )
                    )
                ) c = c.Parent;

            // hilite/unhilite the row or div
            // note that nulls will just be swallowed and no highlighting will occur
            if (c != null)
            {
                if (c is HtmlTableRow)
                {
                    HtmlTableRow tr = (HtmlTableRow)c;
                    tr.Attributes["class"] = hilite ? HILITECLASS : ROWCLASS;
                }
                else if (c is HtmlGenericControl)
                {
                    HtmlGenericControl div = (HtmlGenericControl)c;
                    div.Attributes["class"] = hilite ? HILITECLASS : ROWCLASS;
                }
            }
        }

        protected override void CreateChildControls() 
        {
            this.ID = "ErrorSummaryPanel";
            this.Controls.Add(_ul);
            if (_errors != null && _errors.Count > 0)
            {
                // add error strings to yellow panel and highlight / unhighlight approp rows
                foreach (ErrorItem er in _errors.Values)
                {
                    if (er.ShowInSummary)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerHtml = er.ErrorMessage;
                        _ul.Controls.Add(li);
                    }
                    if (er.ShowInSummary) _hasSummaryErrors = true;
                    HighlightRow(er.Control, er.ShowHighlight);
                }
            }
            this.CssClass = _hasSummaryErrors ? "ValidationSummary" : "ValidationSummary displayNone";
        }

        protected override void OnPreRender(EventArgs e)
        {

            if (!Page.ClientScript.IsClientScriptBlockRegistered(String.Format("{0}validation", this.UniqueName)))
            {
                // build the client instance script
                StringBuilder js = new StringBuilder();
                if ((!this.IsPopup || this.IsIModal) && this.AutoInit) 
                    js.AppendFormat("\r\nwindow.addEvent('load', init{0}Validation);", this.UniqueName);
                js.AppendFormat("\r\nvar {0};", this.ClientInstanceVariableName);
                js.AppendFormat("\r\nfunction init{0}Validation()", this.UniqueName);
                js.Append("{");
                if (this.IsPopup)
                {
                    js.AppendFormat("\r\n\t{0};", this.ResizeScript);
                    js.AppendFormat("\r\n\tif ({0})", this.ClientInstanceVariableName);
                    js.Append("{");
                    js.AppendFormat("\r\n\t\t{0}.reset();", this.ClientInstanceVariableName);
                    js.Append("\r\n\t}\r\n\telse {");
                }
                js.AppendFormat("\r\n\t{0} = new CorbisFormValidator('aspnetForm', ", this.ClientInstanceVariableName);
                js.Append("{");
                if (this.FailScript != null)
                {
                    js.AppendFormat("\r\n\t\tfailScript: \"{0}\",", this.FailScript);
                }
                if (this.SuccessScript != null)
                    js.AppendFormat("\r\n\t\tsuccessScript: \"{0}\",", this.SuccessScript);
                else if (this.SubmitForm)
                {
                    js.Append("\r\n\t\tsubmitForm: true,");
                }
                if (this.SuccessScript != null)
                    js.AppendFormat("\r\n\t\thighlightField: \"{0}\",", this.HighlightField);
                if (this.SubmitByAjax)
                {
                    js.Append("\r\n\t\tsubmitByAjax: true,");
                    js.AppendFormat("\r\n\t\tajaxUrl: '{0}',", this.AjaxUrl);
                    js.AppendFormat("\r\n\t\tonAjaxRequest: {0},", this.OnAjaxRequest);
                }
                if (this.UseStandardAjaxBehavior)
                    js.Append("\r\n\t\tuseStandardAjaxBehavior: true,");
                else if (this.UseOldAjaxBehavior)
                    js.Append("\r\n\t\tuseOldAjaxBehavior: true,");
                if (this.ContainerID != null)
                    js.AppendFormat("\r\n\t\tcontainerID: '{0}',", this.ContainerID);
                js.AppendFormat("\r\n\t\tresizeScript: \"{0}\"", this.ResizeScript);
                js.Append("\r\n\t\t});");
                if (this.InitScript != null)
                    js.AppendFormat("\r\n\t{0};", InitScript);
                js.Append("\r\n}");
                if (IsPopup) 
                    js.Append("\r\n}\r\n");
                if (IsPopup && !IsIModal)
                    js.AppendFormat("\r\n\tinit{0}Validation();\r\n", this.UniqueName);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), String.Format("{0}validation", this.UniqueName), js.ToString(), true);
            }
        }
        #endregion
    }

    public class ErrorItem
    {
        private Control _control;
        private String _message;
        private Boolean _showHighlight;
        private Boolean _showInSummary;

        public string ErrorMessage
        {
            set { _message = value; }
            get { return _message; }
        }

        public Control Control
        {
            get { return _control; }
            set { _control = value; }
        }

        public bool ShowHighlight
        {
            set { _showHighlight = value; }
            get { return _showHighlight; }
        }

        public bool ShowInSummary
        {
            get { return _showInSummary; }
            set { _showInSummary = value; }
        }

        public ErrorItem(Control c, String msg) : this(c, msg, true, true){}

        public ErrorItem(Control c, String msg, bool showInSummary) : this(c, msg, showInSummary, showInSummary){}

        public ErrorItem(Control c, String msg, bool showInSummary, bool showHighlight)
        {
            this.Control = c;
            this.ErrorMessage = msg;
            this.ShowInSummary = showInSummary;
            this.ShowHighlight = showHighlight;
        }
    }
}
