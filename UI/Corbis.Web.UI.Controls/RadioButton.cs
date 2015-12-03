using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Controls
{
    public class RadioButton : System.Web.UI.WebControls.RadioButton
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
