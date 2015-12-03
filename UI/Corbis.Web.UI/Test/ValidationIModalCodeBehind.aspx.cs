using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Test
{
    public partial class ValidationIModalCodeBehind : Corbis.Web.UI.CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lb_Click(object sender, EventArgs e)
        {
            vhub.SetError(textbox1, textbox1.Text != "happy" ? "(codebehind) textbox1 is not happy" : string.Empty);
            vhub.SetError(textbox2, textbox2.Text != "amused" ? "(codebehind) textbox2 is not amused" : string.Empty);

        }
    }
}
