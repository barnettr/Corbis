using System;
using System.IO;

using Corbis.Framework.Globalization;
using Corbis.Web.Utilities;


namespace Corbis.Web.UI.Browse
{
    public partial class Slideshow : CorbisBaseUserControl
	{
		#region Properties
		private string _xmlSource = string.Empty;
		private string _slideshowName = string.Empty;
		#endregion

        protected void Page_Load(object sender, EventArgs e)
		{
			// Check the querystring for a collection name (if any)
			LoadQueryString();

			myXSLTransform.xmlSource = GetXmlPath(_xmlSource);
			myXSLTransform.xslSource = "~/Browse/xslt/Slideshow.xslt";

			HtmlHelper.AddJavascriptToPage(this.Page, "/Scripts/swfobject.js", "SwfObjectScript");
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

		/// <summary>
		/// Check to see if the language-specific file exists, if so return that,
		/// if not, return the english version
		/// </summary>
		private string GetXmlPath(string fileName)
		{
			string returnFilePath = fileName;

			// If a "/" is found in the fileName provided, we will
			// assume the entire path to the file has already been provided
			if (fileName.IndexOf("/") <= 0)
			{
				string language = Language.CurrentLanguage.LanguageCode;
				string englishFilePath = "~/Browse/xml/Slideshows/en-US/" + fileName;
				string languageFilePath = "~/Browse/xml/Slideshows/" + language + "/" + fileName;

				if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(languageFilePath)))
					returnFilePath = languageFilePath;
				else
					returnFilePath = englishFilePath;
			}

			return returnFilePath;
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