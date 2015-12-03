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

namespace Corbis.Web.UI.Image
{
	public partial class Filmstrip : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptInclude("FilmstripJS", SiteUrls.FilmstripScript);
		}

		public string FilmstripClass
		{
			get { return this.filmstrip.Attributes["class"]; }
			set { this.filmstrip.Attributes["class"] = value; }
		}

		public string PreviousClass
		{
			get { return this.previous.Attributes["class"]; }
			set { this.previous.Attributes["class"] = value; }
		}

		public string NextClass
		{
			get { return this.next.Attributes["class"]; }
			set { this.next.Attributes["class"] = value; }
		}

		public string WindowClass
		{
			get { return this.itemsWindow.Attributes["class"]; }
			set { this.itemsWindow.Attributes["class"] = value; }
		}

		public string WindowMessageClass
		{
			get { return this.itemsWindowMessage.Attributes["class"]; }
			set { this.itemsWindowMessage.Attributes["class"] = value; }
		}

		public string ItemsClass
		{
			get { return this.items.Attributes["class"]; }
			set { this.items.Attributes["class"] = value; }
		}
	}
}