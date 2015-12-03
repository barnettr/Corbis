using System;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Legal
{
    public partial class Imprint : CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imprintText.Text = Resources.Legal.ImprintText;
            this.Title = imprintLabel.Text = Resources.Legal.ImprintTitle;
        }

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);
        }
    }
}
