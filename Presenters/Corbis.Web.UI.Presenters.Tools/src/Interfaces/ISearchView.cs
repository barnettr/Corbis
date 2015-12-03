using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Data;
using System.Text;

namespace Corbis.Web.UI.Presenters.Tools.Interfaces
{
    public interface ISearchView
    {
        //string KeywordSearch { get; set; }
        //bool RightsManaged { get; set; }
        //bool RoyaltyFree { get; set; }

        //double ElapsedTime { set; }
        //int TotalHitsFound { set; }

        DataSet CorbisIDList { set; }
        DataSet ThumbnailList { set; }
        DataSet ThumbnailDisplayList { set; }
        NameValueCollection Query { get; }
        SearchPresenter.DisplayMode DisplayMode { set; }
        XmlDocument InputFields { set; }
        XmlDocument CorbisQuery { set; }
        XmlDocument FASTQuery { set; }
        XmlDocument FASTStatistics { set; }
        XmlDocument FASTNavigators { set; }
    }
}
