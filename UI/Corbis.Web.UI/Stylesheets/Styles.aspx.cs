using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yahoo.Yui.Compressor;

namespace Corbis.Web.UI.Stylesheet
{
    public partial class Styles : System.Web.UI.Page
    {
        private const string STYLESHEETCACHE = "StylesheetCache";
        private string collectionName;
        private StringBuilder theScript = new StringBuilder();
        private string outScript = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Which script collection are we doing?
            collectionName = Request.QueryString["collection"];
            if (string.IsNullOrEmpty(collectionName))
            {
                return;
            }
            GetContent();
            WriteContent();
        }

        private void GetContent() 
        {
            if (Cache.Get(STYLESHEETCACHE + collectionName) != null)
            {
                outScript = (string)Cache.Get(STYLESHEETCACHE + collectionName);
            }
            else
            {

                // Get the scripts for the collection
                GetScripts();
                try
                {
#if DEBUG
                    outScript = theScript.ToString();
#else
                    outScript = Yahoo.Yui.Compressor.CssCompressor.Compress(theScript.ToString()); 
                    Cache.Insert(STYLESHEETCACHE + collectionName, outScript);
#endif
                }
                catch (Exception ex)
                {
                    outScript = "/* Compression Failed: Reason: " + ex.Message + Environment.NewLine +
                        "Copy this into http://www.refresh-sf.com/yui/ to find the line containing the error */" + Environment.NewLine +
                        theScript.ToString();
                }
            }
        }

        private void WriteContent()
        {
            Response.ContentType = ("text/css");
            Response.Clear();
#if DEBUG
#else
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(365, 0, 0, 0));
            Response.Cache.SetExpires(DateTime.Now.AddYears(1));
#endif

            Response.BufferOutput = true;
            Response.Write(outScript);
        }

        private void GetScripts()
        {
            // Open up the configuration XML file
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(Request.MapPath("/Stylesheets/Styles.config"));
            XmlNode xn = xdoc.DocumentElement.SelectSingleNode("collection[@name='" + collectionName + "']");
            if (xn != null)
            {
                foreach (XmlNode childNode in xn.ChildNodes)
                {
                    AppendScriptFile(childNode.InnerText);
                }
            }
        }

        private void AppendScriptFile(string fileName)
        {
            // Add begin and end tag comments for debugging, compression will remove them anyway
            theScript.Append("/* Begin: ");
            theScript.Append(fileName);
            theScript.Append(" */");
            theScript.Append(Environment.NewLine);
            theScript.Append(Environment.NewLine);
            theScript.Append(File.ReadAllText(Server.MapPath(fileName)));
            theScript.Append(Environment.NewLine);
            theScript.Append("/* End: ");
            theScript.Append(fileName);
            theScript.Append(" */");
            theScript.Append(Environment.NewLine);
        }

    }
}
