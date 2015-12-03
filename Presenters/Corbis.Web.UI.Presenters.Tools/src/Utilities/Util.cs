using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
//using System.Drawing;
using System.Reflection; //For EnumString
//using System.Windows.Forms; //For Application.Exit()

namespace Corbis.Web.UI.Presenters.Tools
{
    public class Mailer
    {
        private Mailer.eServer m_es = eServer.Smtp;
        private SmtpDeliveryMethod m_sdm = SmtpDeliveryMethod.Network;
        private bool m_bHtml = true;

        public Mailer() { }

        public enum eServer
        {
            Smtp = 0,
            DNS = 1,
        }

        public bool IsBodyHTML
        {
            get { return m_bHtml; }
            set { m_bHtml = value; }
        }

        public Mailer.eServer Server
        {
            get { return m_es; }
            set { m_es = value; }
        }

        public SmtpDeliveryMethod SmtpDeliveryMethod
        {
            get { return m_sdm; }
            set { m_sdm = value; }
        }

        public void Send(string Host, MailAddress From, MailAddressCollection To, MailAddressCollection CC, MailAddressCollection Bcc, string Subject, string Body)
        {
            MailMessage mail = null;
            MailAddressCollection amc = null;
            SmtpClient sc = null;

            if (From == null) throw new Exception("Mail from contains null value");
            if (From.Equals(string.Empty)) throw new Exception("Mail from contains empty value");
            if (Subject == null) throw new Exception("Mail subject contains null value");
            Subject = Subject.Trim();
            if (Subject.Equals(string.Empty)) throw new Exception("Mail subject contains empty value");
            if (Body == null) throw new Exception("Mail body contains null value");
            Body = Body.Trim();
            if (Body.Equals(string.Empty)) throw new Exception("Mail body contains empty value");

            if (To == null && CC == null && Bcc == null) throw new Exception("No mail recepients");

            if (m_es == eServer.Smtp)
            {
                mail = new MailMessage();
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = m_bHtml;

                //mail.Sender = From;
                mail.From = From;
                amc = mail.To;
                foreach (MailAddress ma in To)
                {
                    amc.Add(ma);
                }
                if (CC != null)
                {
                    amc = mail.CC;
                    foreach (MailAddress ma in CC)
                    {
                        amc.Add(ma);
                    }
                }
                if (Bcc != null)
                {
                    amc = mail.Bcc;
                    foreach (MailAddress ma in Bcc)
                    {
                        amc.Add(ma);
                    }
                }

                mail.Body = Body;
                mail.Subject = Subject;

                sc = new SmtpClient();
                sc.Host = Host;
                sc.DeliveryMethod = m_sdm;
                lock (this)
                {
                    sc.Send(mail);
                }
                mail = null;
                sc = null;
            }
        }
    }

    /// <summary>
    /// Create random based on number of digits.
    /// </summary>
    public class RandomNumber
    {
        public string CreateRandomByDigit(int iDigit)
        {
            byte iLower = 0;
            byte iUpper = 9;
            int iNum = 0;
            string sRand = null;
            Random rand = null;

            if (iDigit > 0)
            {
                rand = new Random(DateTime.Now.Millisecond);
                sRand = string.Empty;
                for (int i = 0; i < iDigit; i++)
                {
                    iNum = rand.Next(iLower, iUpper);
                    sRand = sRand + iNum.ToString();
                }
            }
            return sRand;
        }

        /// <summary>
        /// Minimum starts from 0.
        /// </summary>
        /// <param name="iMaxValue"></param>
        /// <returns></returns>
        public string CreateRandomByMaxValue(int iMaxValue)
        {
            byte iLower = 0;
            string sRand = null;
            Random rand = null;

            if (iMaxValue >= iLower)
            {
                rand = new Random(DateTime.Now.Millisecond);
                sRand = rand.Next(iLower, iMaxValue).ToString();
            }
            return sRand;
        }
    }

    /// <summary>
    /// Data structure operations for string type variable.
    /// </summary>
    public class DSString
    {
        public DSString() { }

        /// <summary>
        /// Clean string by removing spaces on the head and tail. If sValue is empty string, assign null.
        /// </summary>
        /// <param name="sValue">a string variable to be trimmed.</param>
        /// <returns>a string value whose spaces are removed at both head and tail.</returns>
        public static string CleanString(string sValue)
        {
            if (sValue != null)
            {
                sValue = sValue.Trim();
                if (sValue.Equals(string.Empty))
                    sValue = null;
            }

            return sValue;
        }

        /// <summary>
        /// Add double quote around a string variable.
        /// </summary>
        /// <param name="sValue">a string variable to be wrapped up by double quote.</param>
        /// <returns>a string value wrapped by double quote at both head and tail.</returns>
        public static string DQ(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) sValue = string.Empty;

            //Escape character
            sValue = "\"" + sValue + "\"";
            return sValue;
        }

        /// <summary>
        /// Add single quote around a string variable.
        /// </summary>
        /// <param name="sValue">a string variable to be wrapped up by single quote.</param>
        /// <returns>a string value wrapped by single quote at both head and tail.</returns>
        public static string SQ(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) sValue = string.Empty;

            sValue = "'" + sValue + "'";
            return sValue;
        }

        public static string ConvertToSqlDateTimeString(DateTime dt, bool bDateOnly)
        {
            string sValue;

            sValue = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
            if (!bDateOnly)
                sValue += " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();

            return sValue;
        }


        public static string CRLF
        {
            get
            {
                char chrCR = (char)13;
                string sCR = chrCR.ToString();
                char chrLF = (char)10;
                string sLF = chrLF.ToString();
                string sCRLF = sCR + sLF;
                return sCRLF;
            }
        }

        public static string ConcateString(ArrayList al, string sDelimitor)
        {
            IEnumerator enumTemp;
            string sTemp = null;

            if (al != null && sDelimitor != null)
            {
                enumTemp = al.GetEnumerator();
                while (enumTemp.MoveNext())
                {
                    if (sTemp != null) sTemp += sDelimitor;
                    sTemp += enumTemp.Current;
                }
            }
            return sTemp;
        }

        public static string Strip(string str, string sStrip)
        {
            string s = null;
            int i;

            if (str != null)
            {
                s = str;
                while (s.IndexOf(sStrip) >= 0)
                {
                    i = s.IndexOf(sStrip);
                    s = s.Substring(0, i) + s.Substring(i + sStrip.Length);
                }
            }

            return s;
        }

        public static string ConvertToJavascriptLineFeed(string sVal)
        {
            return sVal.Replace(DSString.CRLF, "\\n");
        }

        public static string ConvertLineFeedToHtmlBR(string sVal)
        {
            return sVal.Replace(DSString.CRLF, "<BR>");
        }

        public static string ConvertHtmlBRToJavascriptLineFeed(string sVal)
        {
            return sVal.Replace("<BR>", "\\n");
        }

        public static string ConvertHtmlBRToLineFeed(string sVal)
        {
            return sVal.Replace("<BR>", DSString.CRLF);
        }

        /// <summary>
        /// Strip off Html tags and convert to text
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTML(string source)
        {
            try
            {
                string result;
                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating speces becuase browsers ignore them
                result = Regex.Replace(result,
                                                                      @"( )+", " ");
                // Remove the header (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         RegexOptions.IgnoreCase);
                // remove all scripts (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         RegexOptions.IgnoreCase);
                //result = Regex.Replace(result, 
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty, 
                //         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         RegexOptions.IgnoreCase);
                // remove all styles (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         RegexOptions.IgnoreCase);
                // insert tabs in spaces of <td> tags
                result = Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         RegexOptions.IgnoreCase);
                // insert line breaks in places of <BR> and <LI> tags
                result = Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         RegexOptions.IgnoreCase);
                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                // Remove remaining tags like <a>, links, images,
                // comments etc - anything thats enclosed inside < >
                result = Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         RegexOptions.IgnoreCase);
                // replace special characters:
                result = Regex.Replace(result,
                         @" ", " ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&bull;", " * ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lsaquo;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&rsaquo;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&trade;", "(tm)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&frasl;", "/",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lt;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&gt;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&copy;", "(c)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&reg;", "(r)",
                         RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         RegexOptions.IgnoreCase);
                // for testng
                //Regex.Replace(result, 
                //       this.txtRegex.Text,string.Empty, 
                //       RegexOptions.IgnoreCase);
                // make line breaking consistent
                result = result.Replace("\n", "\r");
                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4. 
                // Prepare first to remove any whitespaces inbetween
                // the escaped characters and remove redundant tabs inbetween linebreaks
                result = Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                // Remove multible tabs followind a linebreak with just one tab
                result = Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         RegexOptions.IgnoreCase);
                // Initial replacement target string for linebreaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
                // Thats it.
                return result;
            }
            catch
            {
                return source;
            }
        }

        /// <summary>
        /// If the text is too long for display, this can add carriage return in every breakLine characters.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="breakLine"></param>
        /// <returns></returns>
        public static string WrapTextWithJavascriptLineFeed(string source, int breakLine)
        {
            string ret = string.Empty;
            int startIdx = 0, nextIdx;

            if (!string.IsNullOrEmpty(source))
            {
                ret = source;
                while (startIdx != -1)
                {
                    nextIdx = ret.IndexOf("\r\n", startIdx);
                    if (nextIdx >= 0)
                    {
                        while ((nextIdx - startIdx) > breakLine)
                        {
                            ret = ret.Insert(startIdx + breakLine, "\r\n");
                            nextIdx += 4;
                            startIdx = startIdx + breakLine + 4;
                        }
                        nextIdx += 4;
                    }
                    else
                    {
                        while ((ret.Length - startIdx) >= breakLine)
                        {
                            ret = ret.Insert(startIdx + breakLine, "\r\n");
                            startIdx = startIdx + breakLine + 4;
                        }
                    }
                    startIdx = nextIdx;
                }
            }

            return ret;
        }

        public static List<string> DivideString(string source, string delimiter)
        {
            List<string> list = new List<string>();
            //char[] splitter = new char[sDelimitor.Length];;
            //string[] arr;
            string s;
            int i;

            if (!string.IsNullOrEmpty(source))
            {
                s = source;
                while (s.IndexOf(delimiter) >= 0)
                {
                    i = s.IndexOf(delimiter);
                    list.Add(s.Substring(0, i));
                    s = s.Substring(i + delimiter.Length);
                }
                if (!s.Equals(string.Empty))
                    list.Add(s);

                /*splitter = sDelimiter.ToCharArray();
                arr = str.Split(splitter);			
                for(int x = 0; x < arr.Length; x++)
                    al.Add(arr[x]);
                */
            }

            return list;
        }

        public static string CharSQLSingleQuote(string str)
        {
            if (str != null)
                str = str.Replace("'", "' + char(39) + '");

            return str;
        }

        //C# only
        public static bool IsNumeric(string sValue)
        {
            bool bNumeric = true, bDot = false;
            string sTmp = null;

            if (sValue.Trim().StartsWith("-")) sTmp = sValue.Trim().Substring(1);
            else sTmp = sValue.Trim();

            foreach (char c in sTmp)
            {
                if (c == '.' && bDot)
                {
                    bNumeric = false;
                    break;
                }
                if (c == '.' && !bDot) bDot = true;

                if (!Char.IsNumber(c) && c != '.')
                {
                    bNumeric = false;
                    break;
                }
            }

            return bNumeric;
        }

        public static string AddDoubleQuote(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) return sValue;

            sValue = "\"" + sValue + "\"";
            return sValue;
        }

        public static string AddSingleQuote(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) return sValue;

            sValue = "'" + sValue + "'";
            return sValue;
        }

        public static string StripQuote(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) return sValue;

            if (sValue.StartsWith("\"") && sValue.EndsWith("\""))
            {
                sValue = sValue.Remove(1, 1);
                sValue = sValue.Remove(sValue.Length, 1);
            }
            else if (sValue.StartsWith("'") && sValue.EndsWith("'"))
            {
                sValue = sValue.Remove(1, 1);
                sValue = sValue.Remove(sValue.Length, 1);
            }

            return sValue;
        }

        /// <summary>
        /// This is not for Sql insert/update escape. It's for javascript escape.
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string EscapeDoubleQuote(string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) return sValue;

            sValue = sValue.Replace("\"", "\\\"");

            return sValue;
        }

        public static string[] Split(string value, char[] delimit)
        {
            byte i;
            string[] arrSplit;
            arrSplit = value.Split(delimit);

            for (i = 0; i <= arrSplit.GetUpperBound(0); i++)
                arrSplit[i] = arrSplit[i].Trim();

            return arrSplit;
        }

        public static string EscapeXmlCharacter(string original)
        {
            string ret = null;

            if (!string.IsNullOrEmpty(original))
                ret = original.Replace("&", "&amp;").Replace(",", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

            return ret;
        }

        public static string UnescapeXmlCharacter(string original)
        {
            string ret = null;

            if (!string.IsNullOrEmpty(original))
                ret = original.Replace("&amp;", "&").Replace("&apos;", ",").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");

            return ret;
        }
    }

    /// <summary>
    /// Get network IP and host name
    /// </summary>
    public class LocalMachine
    {
        public static string LookbackAddress
        {
            get
            {
                return IPAddress.Loopback.ToString();
            }
        }

        public static string HostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        public ArrayList GetIPAddresses()
        {
            ArrayList al = new ArrayList();
            IPHostEntry ihe = null;

            ihe = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ipaddress in ihe.AddressList)
            {
                al.Add(ipaddress.ToString());
            }

            return al;
        }

        public bool IsLocalMachine(string sCompare)
        {
            bool bRet = false;
            ArrayList al = null;
            int i = 0;

            if (sCompare.ToLower().Equals("localhost"))
                bRet = true;
            else if (sCompare.Equals(LocalMachine.LookbackAddress))
                bRet = true;
            else if (sCompare.ToLower().Equals(LocalMachine.HostName))
                bRet = true;
            else
            {
                al = GetIPAddresses();
                for (i = 0; i < al.Count; i++)
                {
                    if (sCompare.Equals(al[i].ToString()))
                        bRet = true;
                }
            }

            return bRet;
        }
    }

    [XmlRoot("dictionary")]
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public SerializableDictionary()
        {
        }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// As the IDictionary is not supported for IXmlSerializable in .NET 2.0, make a new substitute.
    /// Source code from http://weblogs.asp.net/pwelter34/archive/2006/05/03/444961.aspx
    /// </summary>
    /*
    [XmlRoot("dictionary")]
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
 
            if (wasEmpty) return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");

                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
     */

    #region Class StringEnum

    /// <summary>
    /// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
    /// </summary>
    public class StringEnum
    {
        private Type _enumType;
        private static Hashtable _stringValues = new Hashtable();

        /// <summary>
        /// Creates a new <see cref="StringEnum"/> instance.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        public StringEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", enumType.ToString()));

            _enumType = enumType;
        }

        /// <summary>
        /// Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public string GetStringValue(string valueName)
        {
            Enum enumType;
            string stringValue = null;
            try
            {
                enumType = (Enum)Enum.Parse(_enumType, valueName);
                stringValue = GetStringValue(enumType);
            }
            catch (Exception) { }//Swallow!

            return stringValue;
        }

        /// <summary>
        /// Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public Array GetStringValues()
        {
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    values.Add(attrs[0].Value);

            }

            return values.ToArray();
        }

        /// <summary>
        /// Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public IList GetListValues()
        {
            Type underlyingType = Enum.GetUnderlyingType(_enumType);
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(_enumType, fi.Name), underlyingType), attrs[0].Value));

            }

            return values;

        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue)
        {
            return Parse(_enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue, bool ignoreCase)
        {
            return Parse(_enumType, stringValue, ignoreCase) != null;
        }

        /// <summary>
        /// Gets the underlying enum type for this instance.
        /// </summary>
        /// <value></value>
        public Type EnumType
        {
            get { return _enumType; }
        }


        /// <summary>
        /// Gets a string value for a particular enum value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            if (_stringValues.ContainsKey(value))
                output = (_stringValues[value] as StringValueAttribute).Value;
            else
            {
                //Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    _stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }

            }
            return output;
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return Parse(enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return Parse(enumType, stringValue, ignoreCase) != null;
        }
    }

    #endregion

    #region Class StringValueAttribute

    /// <summary>
    /// Simple attribute class for storing String Values
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        private string _value;

        /// <summary>
        /// Creates a new <see cref="StringValueAttribute"/> instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value></value>
        public string Value
        {
            get { return _value; }
        }
    }

    #endregion

}