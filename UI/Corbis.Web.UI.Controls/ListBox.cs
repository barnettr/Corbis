using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Controls
{
    public class ListBox : System.Web.UI.WebControls.ListBox
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
