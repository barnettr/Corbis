using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ImageRadioButton runat=server></{0}:ImageRadioButton>")]
    public class ImageRadioButton : WebControl
    {
        #region Member Variables
        private System.Web.UI.WebControls.RadioButton _rb = new System.Web.UI.WebControls.RadioButton();
        private bool _Disabled = false;
        HtmlImage _img = new HtmlImage();
        private string _Changed;
        HiddenField _hdn = new HiddenField();
        #endregion

       
        #region Public Properties
        [Localizable(true)]
        public string Text
        {
            get { return _rb.Text; }
            set { _rb.Text = value; }
        }

        
        public string ContainerID
        {
            get { return this.Attributes["containerID"]; }
            set { this.Attributes["containerid"] = value; }
        }

        public bool IsDisabled
        {
            get { return _Disabled; }
            set { _Disabled = value; }
        }

        public string GroupName
        {
            get { return _rb.GroupName; }
            set
            {
                _rb.GroupName = value;
                Attributes["class"] = "imageRadio radioSelection" + value;
                Attributes["groupname"] = value;
            }
        }

        public bool CausesValidation
        {
            get { return _rb.CausesValidation; }
            set { _rb.CausesValidation = value; }
        }
        public bool Checked
        {
            get { return _rb.Checked; }
            set { _rb.Checked = value; }
        }
       
        public string ValidationGroup
        {
            get { return _rb.ValidationGroup; }
            set { _rb.ValidationGroup = value; }
        }
        public event EventHandler CheckedChanged
        {
            add { _rb.CheckedChanged += value; }
            remove { _rb.CheckedChanged -= value; }
        }

        public string OnClientChanged
        {
            get { return _Changed; } 
            set { _Changed = value; }
        }
        #endregion

        #region Initialization
        protected override void CreateChildControls()
        {
            _rb.EnableViewState = true;
            _img.Src = _rb.Checked ? "/images/radio_on.png" : "/images/radio_off.png";
            _img.Attributes["class"] = "radio";
            Controls.Add(_img);
            _rb.CssClass = "imageRadio";
            Controls.Add(_rb);
            _img.Attributes["onclick"] = string.Format("radioClicked($('{0}'))", this.ClientID);
            _rb.Attributes["onclick"] = string.Format("radioClicked($('{0}'))", this.ClientID);
            if (OnClientChanged != null)
            {
                string rep = OnClientChanged.Replace("this.ClientID", ClientID);
                rep = rep.Replace("this.checked", String.Format("getRadioState($('{0}'))", ClientID));
                _rb.InputAttributes["changed"] = IsDisabled ? String.Format("return false;{0}", rep) : rep;
            }
            _rb.Attributes["onselectstart"] = "event.returnValue = false;return false;";
            _img.ID = _rb.ClientID + "_img";
            Controls.Add(_hdn);
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            // This javascript is included in the base javascript on every page - Optimization task - Travis
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("ImageRadio"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "ImageRadio",
            //        "<LINK REL='stylesheet' TYPE='text/css' HREF='/Stylesheets/controls/ImageRadio.css'/>\r\n<script TYPE='text/javascript' src='/scripts/ImageRadio.js'></script>"
            //    );
            //}
        }

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }
        #endregion




        
    }
}
