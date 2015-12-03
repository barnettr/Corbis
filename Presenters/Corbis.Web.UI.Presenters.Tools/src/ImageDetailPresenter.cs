using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Data;
using System.Text;
using System.Xml;
using Contracts = Corbis.Search.Contracts.V1;
using Corbis.Search.ServiceAgents.V1;
using Corbis.Web.UI.Presenters.Tools.Interfaces;

namespace Corbis.Web.UI.Presenters.Tools
{
    public class ImageDetailPresenter : BasePresenter
    {
        IImageDetailView imageDetailView;
        string MediaID, ImageUrl;

        public ImageDetailPresenter(IImageDetailView imageDetailView)
        {
            if (imageDetailView == null)
            {
                throw new ArgumentNullException("ImageDetailPresenter: ImageDetailPresenter() - Image detail view cannot be null.");
            }

            this.imageDetailView = imageDetailView;
        }

        public void ParseQueryString()
        {
            string temp;
            NameValueCollection query = imageDetailView.Query;

            temp = query["MediaID"];
            if (!string.IsNullOrEmpty(temp))
            {
                MediaID = temp;
            }

            temp = query["ImageUrl"];
            if (!string.IsNullOrEmpty(temp))
            {
                ImageUrl = Decode(temp);
                imageDetailView.ImageUrl = ImageUrl;
            }
        }

        public static string Decode(string data)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode " + e.Message);
            }
        }

        public void GetImageDetails()
        {
            XMLService xmlService = new XMLService();
            XmlElement xmlElement, xmlElementParent;
            XmlDocument xmlDocument;

            //Populate Input Fields
            xmlDocument = new XmlDocument();
            xmlElementParent = xmlService.CreateElement(xmlDocument, xmlDocument, "FastAttributes", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElementParent, "Text", "FAST Attributes", "");

            //xmlElement = xmlService.CreateElement(xmlDocument, xmlElement, "HitNumber", string.Empty, string.Empty);
            //xmlService.CreateAttribute(xmlElement, "Text", "Hit number : " + this.TotalHitsFound.ToString(), "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "Rank", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Rank : xx", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "RankLog", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Ranklog: xx", "");

            imageDetailView.FASTAttributes = xmlDocument;
        }
    }
}