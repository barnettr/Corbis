using System;
using Corbis.Web.Utilities;


namespace Corbis.Web.UI.Browse
{
	public partial class ModalSlideshow : CorbisBaseUserControl
	{
		#region Properties
		private string _xmlSource = string.Empty;
		private string _slideshowName = string.Empty;
		#endregion

        protected void Page_Load(object sender, EventArgs e)
		{
			// Check the querystring for a collection name (if any)
			LoadQueryString();

			// Order of preference is XmlSource explicitly set then querystring
			if (string.IsNullOrEmpty(_xmlSource) && !string.IsNullOrEmpty(_slideshowName))
			{
				SetXmlSourceFromQueryString();
			}

			myXSLTransform.xmlSource = _xmlSource;
			myXSLTransform.xslSource = "~/Browse/xslt/Slideshows.xslt";

			HtmlHelper.AddJavascriptToPage(this.Page, "/Scripts/swfobject.js", "SwfObjectScript");
		}

		#region Private Methods
		/// <summary>
		/// If a slideshow name has been found in the querystring,
		/// use that as the name of the xml file for the slideshow
		/// </summary>
		private void SetXmlSourceFromQueryString()
		{
			_xmlSource = CorbisBrowseBasePage.XmlLanguageFilePath(_slideshowName + ".xml");
		}

		/// <summary>
		/// Check the querystring for relevant variables
		/// </summary>
		private void LoadQueryString()
		{
			if (!string.IsNullOrEmpty(Request.QueryString["name"]))
			{
				_slideshowName = Request.QueryString["name"];
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets and sets the path the XML file for the collection
		/// </summary>
		public string XmlSource
		{
			get { return _xmlSource; }
			set { _xmlSource = value; }
		}
		#endregion
    }
}