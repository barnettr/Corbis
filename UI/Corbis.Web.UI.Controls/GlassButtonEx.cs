using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace Corbis.Web.UI.Controls
{
    public class GlassButtonEx : CompositeControl, IButtonControl
    {
        public enum ProgressPositions
        {
            None,
            Left,
            Right
        }
        private GlassButton _btn = new GlassButton();

        public string Text
        {
            get { return _btn.Text;}
            set { _btn.Text = value; }
        }

        [DefaultValue(ProgressPositions.None)]
        public ProgressPositions ProgressPosition
        {
            get; set;
        }
        public int DisablePeriod 
        { 
            get { return _btn.DisablePeriod; }
            set { _btn.DisablePeriod = value; }
        }

        public bool CausesValidation
        {
            get { return _btn.CausesValidation; }
            set { _btn.CausesValidation = value; }
        }


        public string CommandArgument
        {
            get { return _btn.CommandArgument; }
            set { _btn.CommandArgument = value; }
        }

        public string CommandName
        {
            get { return _btn.CommandName; }
            set { _btn.CommandName = value; }
        }

        public string PostBackUrl
        {
            get { return _btn.PostBackUrl; }
            set { _btn.PostBackUrl = value; }
        }

        public string ValidationGroup
        {
            get { return _btn.ValidationGroup; }
            set { _btn.ValidationGroup = value; }
        }

        public event EventHandler Click;
        public event CommandEventHandler Command;

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }
        public string OnClientClick
        {
            get { return _btn.OnClientClick; }
            set { _btn.OnClientClick = value; }
        }

        protected override void CreateChildControls()
        {
            if (ProgressPosition == ProgressPositions.Left)
                Controls.Add(CreateProgressElement());
            _btn.ID = "glassBtnId";
            _btn.IsGlassButtonEx = true;
            Controls.Add(_btn);

            if (ProgressPosition == ProgressPositions.Right)
                Controls.Add(CreateProgressElement());

            _btn.Click += new EventHandler(Button_Click);
            _btn.Command += new CommandEventHandler(Button_Command);
        }

        void Button_Command(object sender, CommandEventArgs e)
        {
            if (Command != null)
            {
                Command(sender, e);
            }
        }

        void Button_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }

        private HtmlGenericControl CreateProgressElement()
        {
            HtmlGenericControl divElement = new HtmlGenericControl("span");
            HtmlGenericControl imgElement = new HtmlGenericControl("img");
            divElement.ID = "progressSignal";
            divElement.Attributes["class"] = "progressSignal";
            imgElement.Attributes["border"] = "0";
            imgElement.Attributes["src"] = "/images/buddyLoading.gif";
            imgElement.Attributes["class"] = "hdn";

            divElement.Controls.Add(imgElement);
            return divElement;
        }
        protected override void OnPreRender(EventArgs e)
        {
            //if (String.IsNullOrEmpty(this.OnClientClick) && this.ButtonStyle != ButtonStyles.Outline)
            if (String.IsNullOrEmpty(this.OnClientClick))
            {
                this.Attributes.Add("onclick", string.Format("passClickToChild('{0}')", this.ClientID));
            }
        }
    }
}
