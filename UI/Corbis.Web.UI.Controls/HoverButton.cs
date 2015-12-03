using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using System.Web.UI;

namespace Corbis.Web.UI.Controls
{
    /// <summary>
    /// this button will use stacked image to show mouse hover effect
    /// this is a simple image button without text
    /// </summary>
    public class HoverButton : CompositeControl, IButtonControl
    {
        #region Fields

        private Button button;

        #endregion

        #region Properties

        public string OnClientClick
        {
            get
            {
                EnsureChildControls();
                return button.OnClientClick;
            }
            set
            {
                EnsureChildControls();
                button.OnClientClick = value;
            }
        }
        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        } 
        #endregion

        #region IButtonControl Implementation

        public bool CausesValidation
        {
            get
            {
                EnsureChildControls();
                return button.CausesValidation;
            }
            set
            {
                EnsureChildControls();
                button.CausesValidation = value;
            }
        }

        public string CommandArgument
        {
            get
            {
                EnsureChildControls();
                return button.CommandArgument;
            }
            set
            {
                EnsureChildControls();
                button.CommandArgument = value;
            }
        }

        public string CommandName
        {
            get
            {
                EnsureChildControls();
                return button.CommandName;
            }
            set
            {
                EnsureChildControls();
                button.CommandName = value;
            }
        }

        public string PostBackUrl
        {
            get
            {
                EnsureChildControls();
                return button.PostBackUrl;
            }
            set
            {
                EnsureChildControls();
                button.PostBackUrl = value;
            }
        }

        public string Text
        {
            get
            {
                EnsureChildControls();
                return button.Text;
            }
            set
            {
                EnsureChildControls();
                button.Text = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return button.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                button.ValidationGroup = value;
            }
        }

        public event EventHandler Click;
        public event CommandEventHandler Command;

        #endregion

        #region Methods

        protected override void CreateChildControls()
        {
            button = new Button();
            button.Click += new EventHandler(Button_Click);
            button.Command += new CommandEventHandler(Button_Command);
            button.ID = "HoverButton";
            button.Attributes["class"] = "hovable";

            //container = new HtmlGenericControl("div");
            //container.Attributes["class"] = CssClass;
            //container.Controls.Add(right);

            Controls.Add(button);
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
    }
}
