using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class ProjectBlock : System.Web.UI.UserControl
    {
		public bool EnableEdit { get; set; }

        public string ProjectName
        {
            get { return projectNameData.Text; }
            set { projectNameData.Text = Server.HtmlEncode(value); }
        }
        public string JobNumber
        {
            set
            {
                jobNumber.Text = Server.HtmlEncode(value);
                if (!string.IsNullOrEmpty(value))
                    jobNumberRow.Visible = true;
                else
                    jobNumberRow.Visible = false;
            }
        }
        public string PoNumber
        {
            set
            {
                poNumber.Text = Server.HtmlEncode(value);
                if (!string.IsNullOrEmpty(value))
                    poNumberRow.Visible = true;
                else
                    poNumberRow.Visible = false;
            }
        }
        public string Licensee
        {
            set { licensee.Text = Server.HtmlEncode(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!EnableEdit)
			{
				editLink.Visible = false;
			}
			base.OnInit(e);
		}
    }
}