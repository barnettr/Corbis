using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Corbis.Web.UI.HttpModules
{
    public class Redirection
    {
        [XmlAttribute("targetUrl")]
        public string targetUrl;

        [XmlAttribute("destinationUrl")]
        public string destinationUrl;

        [XmlAttribute("permanent")]
        public bool permanent = false;

        [XmlAttribute("ignoreCase")]
        public bool ignoreCase = false;

        [XmlIgnore]
        public Regex Regex;
    }

    [XmlRoot("redirections")]
    public class ConfigRedirections
    {
        [XmlElement("add")]
        public Redirection[] Redirections;
    }

}
