using System;

namespace Corbis.Web.UI.Browse
{
    public partial class FeaturedItems : CorbisBaseUserControl
	{
		private string _xmlSource = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			myXSLTransform.xmlSource = _xmlSource;
			myXSLTransform.xslSource = "xslt/FeaturedItems.xslt";
		}

		public string XmlSource
		{
			get { return _xmlSource; }
			set { _xmlSource = value; }
		}
    }
}