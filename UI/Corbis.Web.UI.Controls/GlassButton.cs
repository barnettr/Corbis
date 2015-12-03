using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using System.Web.UI;
using System.Text;
using System.ComponentModel;

namespace Corbis.Web.UI.Controls
{
    public class GlassButton : CompositeControl, IButtonControl
    {
        #region Fields / Member Variables
        private HtmlGenericControl _right = new HtmlGenericControl("span");
        private Button _button = new Button();
        private LinkButton _linkButton = new LinkButton();
        private ButtonBackgrounds _btnBg = ButtonBackgrounds.dbdbdb;
        private ButtonStyles _bStyle = ButtonStyles.Orange;
        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }
        #endregion

        #region Enum types
        public enum ButtonStyles
        {
            Orange,
            Gray,
            Outline
        }

        public enum ButtonBackgrounds// append to cssclass
        {
            dbdbdb,
            e8e8e8,
            gray36,
            gray4b
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Sets whether button is orange or gray. Orange by default.
        /// Place this value be
        /// </summary>
        public ButtonStyles ButtonStyle
        {
            get { return _bStyle; }
            set { _bStyle = value; }
        }

        /// <summary>
        /// Sets the hex for the background - these correspond to css classes in 
        /// glassbutton.css and need an accompanying sprite file. 
        /// See the css file for more info
        /// </summary>
        public ButtonBackgrounds ButtonBackground
        {
            get { return _btnBg; }
            set { _btnBg = value; }
        }

        public string OnClientClick
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.OnClientClick;
                }
                else
                {
                    return _button.OnClientClick;
                }
            }
            set
            {
                _linkButton.OnClientClick = value;
                _button.OnClientClick = value;
                if ((this.IsGlassButtonEx && Click == null && Command == null) || (!IsGlassButtonEx && Click != null && Command != null))
                {
                    _linkButton.OnClientClick += ";return false;";
                    _button.OnClientClick += ";return false;";
                }
            }
        }

        public bool CausesValidation
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.CausesValidation;
                }
                else
                {
                    return _button.CausesValidation;
                }
            }
            set
            {
                _button.CausesValidation = value;
                _linkButton.CausesValidation = value;
            }
        }

        public string CommandArgument
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.CommandArgument;
                }
                else
                {
                    return _button.CommandArgument;
                }
            }
            set
            {
                _button.CommandArgument = value;
                _linkButton.CommandArgument = value;
            }
        }

        public string CommandName
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.CommandName;
                }
                else
                {
                    return _button.CommandName;
                }
            }
            set
            {
                _button.CommandName = value;
                _linkButton.CommandName = value;
            }
        }

        public string PostBackUrl
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.PostBackUrl;
                }
                else
                {
                    return _button.PostBackUrl;
                }
            }
            set
            {
                _button.PostBackUrl = value;
                _linkButton.PostBackUrl = value;
            }
        }

        public string Text
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.Text;
                }
                else
                {
                    return _button.Text;
                }
            }
            set
            {
                _button.Text = value;
                _linkButton.Text = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                if (this.ButtonStyle == ButtonStyles.Outline)
                {
                    return _linkButton.ValidationGroup;
                }
                else
                {
                    return _button.ValidationGroup;
                }
            }
            set
            {
                _button.ValidationGroup = value;
                _linkButton.ValidationGroup = value;
            }
        }

        [DefaultValue(0)]
        public int DisablePeriod { get; set; }

        [DefaultValue(false)]
        public bool IsGlassButtonEx { get; set; }
        #endregion

        #region IButtonControl Implementation

        public event EventHandler Click;
        public event CommandEventHandler Command;

        #endregion

        #region Methods
        protected override void CreateChildControls()
        {
			//TODO: Note: if cssclass is set and viewstate is enabled, the glassbutton will lose the cssclass on postback
			//because of the 'this.CssClass.Contains("GlassButton") || this.CssClass.Contains("btnOrange")' condition
            if (this.ButtonStyle == ButtonStyles.Outline)
            {
                _linkButton.Click += new EventHandler(Button_Click);
                _linkButton.Command += new CommandEventHandler(Button_Command);
                _linkButton.ID = "GlassButton";

                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes["class"] = this.Enabled ? "Center" : "Center DisabledGlassButton";

                //_linkButton.Attributes["class"] = this.Enabled ? "Center" : "Center DisabledGlassButton";
                span.Controls.Add(_linkButton);

                _right.Attributes["class"] = this.Enabled ? "Right" : "Right DisabledGlassButton";
                _right.Controls.Add(span);

                // ensure onclick is set on the client button in case the client script
                // enables a button.
                if (!this.Enabled)
                    _linkButton.Attributes["onclick"] = string.Format("{0}{1}{2}",
                        _linkButton.Attributes["onclick"], this.OnClientClick,
                        Click == null && Command == null ? ";return false;" : string.Empty
                        );
                Controls.Add(_right);
                this.CssClass = String.Format("GlassButton btn{0}{1}{2}{3}",
                    this.ButtonStyle,
                    this.ButtonBackground,
                    this.Enabled ? string.Empty : " DisabledGlassButton",
                    this.CssClass.Contains("GlassButton") || this.CssClass.Contains("btnOrange") ?
                    String.Empty : " " + this.CssClass
                );

            }
            else
            {
                _button.Click += new EventHandler(Button_Click);
                _button.Command += new CommandEventHandler(Button_Command);
                _button.ID = "GlassButton";
                _button.Attributes["class"] = this.Enabled ? "Center" : "Center DisabledGlassButton";
                _right.Attributes["class"] = this.Enabled ? "Right" : "Right DisabledGlassButton";
                _right.Controls.Add(_button);

                // ensure onclick is set on the client button in case the client script
                // enables a button.
                if (!this.Enabled)
                    _button.Attributes["onclick"] = string.Format("{0}{1}{2}",
                        _button.Attributes["onclick"], this.OnClientClick,
                        Click == null && Command == null ? ";return false;" : string.Empty
                        );
                Controls.Add(_right);
                this.CssClass = String.Format("GlassButton btn{0}{1}{2}{3}",
                    this.ButtonStyle,
                    this.ButtonBackground == ButtonBackgrounds.gray36 ?
                        "363636" : this.ButtonBackground == ButtonBackgrounds.gray4b ?
                        "4b4b4b" : this.ButtonBackground.ToString(),
                    this.Enabled ? string.Empty : " DisabledGlassButton",
                    this.CssClass.Contains("GlassButton") || this.CssClass.Contains("btnOrange") ?
                    String.Empty : " " + this.CssClass
                );
            }

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            if (Command != null)
            {
                Command(sender, e);
            }
        }

        #endregion

        #region Initialization
        protected override void OnPreRender(EventArgs e)
        {
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("glassButtonCSS"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "glassButtonCSS",
            //        "<LINK REL='stylesheet' TYPE='text/css' HREF='/Stylesheets/controls/GlassButton.css'/>"
            //    );

            //    Page.ClientScript.RegisterClientScriptBlock(
            //        Page.GetType(), "glassButtonDisabler",
            //        "<script TYPE='text/javascript' src='/scripts/glassbutton.js'></script>"
            //    );
            //}

            if (!IsGlassButtonEx && String.IsNullOrEmpty(this.OnClientClick) && this.ButtonStyle != ButtonStyles.Outline)
            {
                this.Attributes.Add("onclick", string.Format("passClickToChild('{0}')", this.ClientID));
            }
            else
            {
//                this.Attributes.Add("onclick", this.OnClientClick + ";return false;");
            }
            if (this.IsGlassButtonEx && !this.OnClientClick.Contains("setGlassButtonDisabledWithinPeriod"))
            {
                this.OnClientClick = string.Format("setGlassButtonDisabledWithinPeriod(this, {0});", DisablePeriod) +
                                     this.OnClientClick;
            }
        }
        #endregion
    }
}
