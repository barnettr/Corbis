using System;


namespace Corbis.Web.UI.Browse
{
	public partial class Slideshows : CorbisBrowseBasePage
	{
		#region Properties
		private string _xmlSource = string.Empty;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			// Check the querystring for a collection name (if any)
			LoadQueryString();

			Slideshow.XmlSource = _xmlSource;
		}

		#region Private Methods
		/// <summary>
		/// Check the querystring for relevant variables
		/// </summary>
		private void LoadQueryString()
		{
			if (!string.IsNullOrEmpty(Request.QueryString["name"]))
			{
				_xmlSource = Request.QueryString["name"] + ".xml";
			}
		}
		#endregion
	}
}
