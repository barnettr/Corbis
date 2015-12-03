using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Controls
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }
    }
}
