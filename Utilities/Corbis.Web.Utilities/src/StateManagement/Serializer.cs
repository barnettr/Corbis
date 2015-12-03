using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Corbis.Web.Utilities.StateManagement
{
    internal static class Serializer
    {

        public static T DeserializeObject<T>(String serializedObject)
        {
            T deserializedObject = default(T);

            if (typeof(T).IsEnum)
            {
                deserializedObject = (T)Enum.Parse(typeof(T), serializedObject);
            }
            else
            {
                // if the type is IConvertible, just convert it
                bool isConvertible = (typeof(T).GetInterface("IConvertible") != null);
                if (isConvertible)
                {
                    deserializedObject = (T)System.Convert.ChangeType(serializedObject, typeof(T));
                }
                else
                {
                    // Create a reader
                    using (StringReader stringReader = new StringReader(serializedObject))
                    {
                        // Create an XmlSerializer to do the work
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                        // De-serialize the string to our object
                        deserializedObject = (T)xmlSerializer.Deserialize(stringReader);
                        // Close the reader
                        stringReader.Close();
                    }
                }
            }
            return deserializedObject;
        }

        public static String SerializeObject(Object persistentObject)
        {
            string serializedValue = null;

            // if the type is IConvertible, just use the string, otherwise use an XmlSerializer
            bool isConvertible = (persistentObject.GetType().GetInterface("IConvertible") != null);
            if (isConvertible)
            {
                serializedValue = System.Convert.ToString(persistentObject);
            }
            else
            {
                // Create the XmlSerializer to do the work
                XmlSerializer xmlSerializer = new XmlSerializer(persistentObject.GetType());
                // Create Builder and Writer
                StringBuilder serializedObject = new StringBuilder();
                using (StringWriter stringWriter = new StringWriter(serializedObject))
                {
                    // Serialize the collection object to XML
                    xmlSerializer.Serialize(stringWriter, persistentObject);
                    // Close the writer
                    stringWriter.Close();
                }
                serializedValue = serializedObject.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", String.Empty);
            }
            return serializedValue;
        }
    }
}
