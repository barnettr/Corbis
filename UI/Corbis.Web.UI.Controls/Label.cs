namespace Corbis.Web.UI.Controls
{
    public class Label : System.Web.UI.WebControls.Label
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
