namespace Corbis.Web.UI.Controls
{
    public class CheckBox : System.Web.UI.WebControls.CheckBox
    {
        private string value = string.Empty;
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
