using System;
using System.Web.UI;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Controls
{
    public class Localize : System.Web.UI.WebControls.Localize
    {
        private string replaceKey = string.Empty;
        private string replaceValue = string.Empty;
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        public string ReplaceKey
        {
            get { return this.replaceKey; }
            set { this.replaceKey = value; }
        }

        public string ReplaceValue
        {
            get { return this.replaceValue; }
            set { this.replaceValue = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                if (StringHelper.IsNullOrTrimEmpty(ReplaceKey))
                {
                    writer.Write(this.Text);
                }
                else
                {
                    writer.Write(this.Text.Replace(ReplaceKey, ReplaceValue));
                }
            }
            catch (Exception)
            { }
        }
    }
}
