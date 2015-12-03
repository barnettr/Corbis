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
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Browse.CustomControls
{
	public partial class ModuleCustom : System.Web.UI.UserControl
	{
		public ModuleCustom()
		{
		}

		public ModuleCustom(string xmlSource, string xslSource, string stylesheet)
		{
			if (!string.IsNullOrEmpty(stylesheet))
				HtmlHelper.AddStylesheetToPage(this.Page, stylesheet, "ModuleCustomStyles");

			myXSLTransform.xmlSource = xmlSource;
			myXSLTransform.xslSource = xslSource;
		}
	}
}