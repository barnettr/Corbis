using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

#region Sample Implementation code
/*
    sample code:
   
       <corbis:ImageCheckbox OnClientChanged="alert('I just got clicked')" TextStyle=Heading
           id=foo runat=server Text="my image checkbox"
       />
		
       <corbis:ImageCheckbox TextStyle=Link
           id=ImageCheckbox1 runat=server Text="my other image checkbox"
       />
		
       <corbis:ImageCheckbox OnClientChanged="alert('Hi, my checked state is ' + this.checked)"
           id=ImageCheckbox2 runat=server Text="my last image checkbox"
       />
       <br />
       <div onclick="toggleCheckedState('<%=ImageCheckbox2.ClientID %>')">i toggle</div>
       <br />
       <div onclick="setCheckedState('<%=ImageCheckbox2.ClientID %>', true)">i check</div>
       <br />
       <div onclick="setCheckedState('<%=ImageCheckbox2.ClientID %>', false)">i uncheck</div>
     
*/
#endregion

namespace Corbis.Web.UI.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ImageCheckbox runat=server></{0}:ImageCheckbox>")]
    public class ImageCheckbox : CompositeControl, ICheckBoxControl
    {
        #region Member Variables
        private CheckBox _cb = new CheckBox();
        private bool _Disabled = false;
        private TextStyles _TextStyle = TextStyles.Normal;
        HtmlImage _img = new HtmlImage();
        private string _appendHTML = string.Empty;
        #endregion

        #region Enums
        public enum TextStyles
        { 
            Normal,
            Heading,
            Link
        }
        //public enum CheckedStates
        //{
        //    Checked,
        //    Unchecked,
        //    HalfChecked
        //}
        #endregion

        #region Public Properties
        [Localizable(true)]
        public string Text
        {
            get { return _cb.Text; }
            set { _cb.Text = value; }
        }

        public bool Validate
        {
            set { _cb.InputAttributes["validate"] = "checkbox;required"; }
        }

        public string checkbox_message
        {
            set
            {
                _cb.InputAttributes["checkbox_message"] = value;
                _cb.InputAttributes["required_message"] = value;
            }
        }
        public bool AutoPostBack
        {
            get { return _cb.AutoPostBack; }
            set { _cb.AutoPostBack = value; }
        }
        public string AppendHTML
        {
            get { return _appendHTML; }
            set { _appendHTML = value; }
        }
        public TextStyles TextStyle
        {
            get { return this._TextStyle; }
            set { this._TextStyle = value; }
        }
        public bool IsDisabled
        {
            get { return this._Disabled; }
            set { this._Disabled = value; }
        }
        public bool CausesValidation
        {
            get { return _cb.CausesValidation; }
            set { _cb.CausesValidation = value; }
        }
        public bool Checked
        {
            get { return _cb.Checked; }
            set 
            { 
                _cb.Checked = value;
                _img.Src = value ? "/images/checked.png" : "/images/unchecked.png";
            }
        }
        public string Value
        {
            get { return _cb.Value; }
            set { 
                _cb.Value = value; 
                // the Value attribute doesn't actually get rendered, so we have to add it to the attributes.
                _cb.InputAttributes.Add("Value", value);
            }
        }
        public string ValidationGroup
        {
            get { return _cb.ValidationGroup; }
            set { _cb.ValidationGroup = value; }
        }
        public event EventHandler CheckedChanged
        {
            add { _cb.CheckedChanged += value; }
            remove { _cb.CheckedChanged -= value; }
        }

        public string OnClientChanged
        {
            get { return _cb.InputAttributes["onclick"]; }
            set
            {
                //string[] s = value.Split(new string[] { "javascript:" }, StringSplitOptions.RemoveEmptyEntries);
                if (this.IsDisabled)
                {
                    _cb.InputAttributes["onclick"] = string.Format("return false;{0}", value);
                }
                else
                {
                    _cb.InputAttributes["onclick"] = value;
                }
            }
        }

        #endregion

        #region Initialization
        protected override void CreateChildControls()
        {
            
            this.Controls.Add(_img);
            _cb.CssClass = this._TextStyle == TextStyles.Heading ?
                "ImageCheckbox checkboxHeading" : this._TextStyle == TextStyles.Link ?
                "ImageCheckbox checkboxLinkedText" :
                "ImageCheckbox";
            this.Controls.Add(_cb);
            
            _img.Src = _cb.Checked ? "/images/checked.png" : "/images/unchecked.png";
            _img.Attributes["class"] = "checkbox";

            if (!AutoPostBack)
                _img.Attributes["onclick"] = "toggleCheckedState($(this).getParent());";
            this.CssClass = "imageCheckbox";
            this.OnClientChanged = String.Format("toggleCheckImage($(this));{0}", this.OnClientChanged ?? "");
            _cb.Attributes["onselectstart"] = "event.returnValue = false;return false;";
            
            if (this.AppendHTML != string.Empty)
            {
                HtmlGenericControl c = new HtmlGenericControl();
                c.TagName = "font";
                c.InnerHtml = this.AppendHTML;
                this.Controls.Add(c);
            }
            if (AutoPostBack)
                this.Attributes["onclick"] = String.Format("setCheckedState(this, !$('{0}').checked);", _cb.ClientID);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("ImageCheckbox"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "ImageCheckbox",
            //        "<LINK REL='stylesheet' TYPE='text/css' HREF='/Stylesheets/controls/ImageCheckbox.css'/>\r\n<script TYPE='text/javascript' src='/scripts/ImageCheckbox.js'></script>"
            //    );
            //}
            //if (AutoPostBack && Page.IsPostBack && Page.Request.Form["__EVENTTARGET"].Replace('$', '_') == _cb.ClientID) 
            //    _cb.Checked = !_cb.Checked;
            _img.Src = _cb.Checked ? "/images/checked.png" : "/images/unchecked.png";
            
        }

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }
        #endregion
    }
   
}
