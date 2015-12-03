using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Corbis.Web.UI.Controls
{
    public partial class XSLTransform : CorbisBaseUserControl
    {
        public string xmlSource = string.Empty;
        public string xslSource = string.Empty;

		protected override void Render(HtmlTextWriter writer)
		{
			try
			{
				//Create XML doc and load source
				XmlDocument docXml = new XmlDocument();
				docXml.Load(Server.MapPath(xmlSource));

				//Create XSL transform and load source XSLT
				XslCompiledTransform docXsl = new XslCompiledTransform();
				docXsl.Load(Server.MapPath(xslSource));
				docXsl.Transform(docXml, null, writer);
			}
			catch
			{
				//writer.WriteLine("The xml/xsl files could not be loaded.");
			}
		}
    }
}

