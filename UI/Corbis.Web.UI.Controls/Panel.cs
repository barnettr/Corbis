namespace Corbis.Web.UI.Controls
{
    public class Panel : System.Web.UI.WebControls.Panel
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
