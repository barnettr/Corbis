using System;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Corbis.Web.UI.Content
{
    public partial class XSLTransform : CorbisBasePage
    {
        public String xmlSource;
        public String xslSource;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Create XML doc and load source
            XmlDocument docXml = new XmlDocument();
            docXml.Load(Server.MapPath(xmlSource));

            //Create XSL transform and load source XSLT
            XslCompiledTransform docXsl = new XslCompiledTransform();
            docXsl.Load(Server.MapPath(xslSource));
            docXsl.Transform(docXml, null, Response.Output);
        }
    }
}

