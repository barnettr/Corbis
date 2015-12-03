using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Test
{
    public partial class Validation : CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AddScriptToPage(SiteUrls.ErrorTrackerScript, "ErrorTracker");
        }

        //protected void dd_changed(object sender, EventArgs e) 
        //{
        //    tb.Text = dd.SelectedValue;
        //}

        //protected void btn_click(object sender, EventArgs e)
        //{
        //    lbl.Text = tb.Text;
        //}
    }
}
