using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Corbis.Web.UI.CommonUserControls
{
	public partial class FileSizes : CorbisBaseUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			HtmlLink stylesheet = new HtmlLink();
			stylesheet.ID = "fileSizeCss";
			stylesheet.Href = Stylesheets.FileSizes;
			stylesheet.Attributes["rel"] = "stylesheet";
			stylesheet.Attributes["type"] = "text/css";
			stylesheet.Attributes["media"] = "all";

			Page.Header.Controls.Add(stylesheet);
		}
	}
}