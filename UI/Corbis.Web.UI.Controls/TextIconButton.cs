using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TextIconButton runat=server></{0}:TextIconButton>")]
    public class TextIconButton : CompositeControl
    {
        #region Member Variables
        private string _Text = string.Empty;
        private HtmlAnchor _lButton = new HtmlAnchor();
        private Icons _Icon = Icons.selectAll;
        private bool _Disabled = false;
        #endregion

        #region Enum Types
        public enum Icons
        {
            selectAll,
            deleteAll,
            clearAll,
            yellowAlert
        }
        #endregion

        #region Public Properties
        [Localizable(true)]
        public string Text
        {
            get { return _lButton.InnerText; }
            set { _lButton.InnerText = value; }
        }

        public Icons Icon
        {
            get { return this._Icon; }
            set { this._Icon = value; }
        }
        public bool IsDisabled
        {
            get { return this._Disabled; }
            set { this._Disabled = value; }
        }
        public bool CausesValidation
        {
            get { return _lButton.CausesValidation; }
            set { _lButton.CausesValidation = value; }
        }
        public string OnClientClick
        {
            get { return _lButton.Attributes["exec"]; }
            set { _lButton.Attributes["exec"] = value; }
        }
        public string ValidationGroup
        {
            get { return _lButton.ValidationGroup; }
            set { _lButton.ValidationGroup = value; }
        }
        #endregion

        #region Methods


        #endregion

        #region Initialization / Control Rendering
        protected override void CreateChildControls()
        {
            this._lButton.ID = "TextIconBtn";
            this._lButton.Attributes["class"] = string.Format(
                "textIconButton {0}",
                this.IsDisabled ? " disabled" : ""
            );
            this._lButton.HRef = "";
            this.Controls.Add(this._lButton);
            this.CssClass = string.Format("textIconButtonContainer {0}{1}", 
                this.Icon,
                this.IsDisabled ? " disabled" : "");
            this.Attributes["onclick"] = string.Format("sendClickToAnchor('{0}')", this.ClientID);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("TextIconButton"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "TextIconButton",
            //        "<LINK REL='stylesheet' TYPE='text/css' HREF='/Stylesheets/controls/TextIconButton.css'/>"
            //    );
            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "TextIconButton2",
            //        "<script TYPE='text/javascript' src='/scripts/TextIconButton.js'></script>"
            //    );
            //}

        }

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Span; }
        }
        #endregion
    }
}
