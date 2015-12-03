using System;

namespace Corbis.Web.UI.Accounts
{
    public partial class MyAccount : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            IsAccountPage = true;
            base.OnInit(e);
        }
    }
}
