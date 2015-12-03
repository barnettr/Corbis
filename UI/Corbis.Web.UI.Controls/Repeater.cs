using System;
using System.Web.UI;

namespace Corbis.Web.UI.Controls
{
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        private bool enableViewState = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(System.Web.UI.WebControls.RepeaterItem))]
        public override ITemplate ItemTemplate
        {
            get { return base.ItemTemplate; }
            set { base.ItemTemplate = value; }
        }
    }
}
