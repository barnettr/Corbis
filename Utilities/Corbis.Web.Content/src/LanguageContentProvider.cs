using System;
using System.Collections.Generic;
using Corbis.Framework.Logging;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.CustomConfigurationSettings;
using System.Xml;

namespace Corbis.Web.Content
{
    public class LanguageContentProvider : ContentProvider
    {
        private ListItemCollection dataSource;
        private static XmlDocument configDoc = null;
        
        public LanguageContentProvider()
        {
            
        }

        public void SetDataSourceBehavior(ListItemCollection source)
        {
            dataSource = source;
        }

        #region IContentProvider Members

        public List<ContentItem> GetLanguages()
        {
            const string LANGUAGE_CONFIG_SECTION = "LanguageListSelector";

            List<ContentItem> data = new List<ContentItem>();

            List<ContentItem> cacheData = CachePersistenceHelper.RetrieveFromCache(CacheItem.LanguageList) as List<ContentItem>;

            if (cacheData != null)
            {
                data = cacheData;
            }
            else
            {
                if (dataSource == null)
                {
                    try
                    {
                        dataSource = ListItemConfiguration.GetConfig(LANGUAGE_CONFIG_SECTION).Items;
                    }
                    catch
                    {
                        loggingContext.LogErrorMessage(
                            "LanguageContentProvider:Configuration Section Error"
                            , String.Format("Unable to retrieve values from {0}", LANGUAGE_CONFIG_SECTION));

                        return data;
                    }

                }
                if (dataSource != null && dataSource.Count > 0)
                {
                    foreach (ListItem li in dataSource)
                    {
                        data.Add(new ContentItem(li.Value, li.Text));
                    }
                    CachePersistenceHelper.SaveToCache(CacheItem.LanguageList, data);
                }
            }

            return data;

        }

        public static XmlDocument InitConfigDoc(string docPath)
        {
			if (configDoc == null || docPath.ToLower().IndexOf("browse") > 0)
            {
                configDoc = new XmlDocument();
                configDoc.Load(docPath);
            }
            return configDoc;         
        }

        public List<ContentItem> GetLanguages(string docPath)
        {
            List<ContentItem> list = new List<ContentItem>();
            ContentItem item;
            XmlDocument doc = InitConfigDoc(docPath);                   
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Page/LanguageListSelector/Items/Item");
            foreach (XmlNode n in nodes)
            {
                item = new ContentItem(n.Attributes["Value"].Value, n.Attributes["Text"].Value);
                list.Add(item);
            }            
            return list;
        }
        #endregion

        
    }
}

