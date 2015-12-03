using System;

namespace Corbis.Web.UI.Browse
{
	public partial class ModuleText : CorbisBaseUserControl
    {
		private string _xmlSource = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
		{
			myXSLTransform.xmlSource = _xmlSource;
			myXSLTransform.xslSource = "xslt/ModuleText.xslt";
        }

		public string XmlSource
		{
			get { return _xmlSource; }
			set { _xmlSource = value; }
		}
    }
}