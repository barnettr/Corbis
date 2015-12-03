using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Corbis.Web.UI.HttpModules
{
    /// <summary>
    /// Configuration section handler that deserializes connfiguration settings to an object.
    /// </summary>
    /// <remarks>The configuration node must have a type attribute defining the type to deserialize to.</remarks>
    public class XmlSerializerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Implemented by all configuration section handlers to parse the XML of the configuration section.
        /// The returned object is added to the configuration collection and is accessed by <see cref="System.Configuration.ConfigurationSettings.GetConfig"/>.
        /// </summary>
        /// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
        /// <param name="configContext">An <see cref="System.Web.Configuration.HttpConfigurationContext"/> when this method is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is a null reference (Nothing in Visual Basic).</param>
        /// <param name="section">The <see cref="System.Xml.XmlNode"/> that contains the configuration information from the configuration file. Provides direct access to the XML contents of the configuration section.</param>
        /// <returns>A configuration object.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XPathNavigator navigator;
            XmlSerializer serializer;
            Type type;
            string typeName;

            navigator = section.CreateNavigator();
            typeName = (string)navigator.Evaluate("string(@type)");
            type = Type.GetType(typeName, true);
            serializer = new XmlSerializer(type);
            return serializer.Deserialize(new XmlNodeReader(section));
        }
    }
}
