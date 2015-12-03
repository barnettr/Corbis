namespace Corbis.Web.UI.Controls
{
    public class Button : System.Web.UI.WebControls.Button
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
