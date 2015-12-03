using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Accounts
{
    public partial class TestEditShippingAddressControl : CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void linkAddShippingAddress_OnClick(object sender, EventArgs e)
        {
            Corbis.Web.UI.Controls.LinkButton lb = (Corbis.Web.UI.Controls.LinkButton)sender;
            this.editShippingAddress.EditMode = Corbis.Web.UI.Presenters.Accounts.AccountsEditMode.Delete;
            this.editShippingAddress.AddressUid = new Guid(lb.CommandArgument);
            this.editShippingAddressModalPopupExtender.Show();
        }

        public void CloseEditShippingAddressModalPopup(object sender, EventArgs e)
        {
            this.editShippingAddressModalPopupExtender.Hide();
        }

    }
}
