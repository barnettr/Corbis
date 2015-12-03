using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Data;
using System.Text;

namespace Corbis.Web.UI.Presenters.Tools.Interfaces
{
    public interface IImageDetailView
    {
        string ImageUrl { set; }
        XmlDocument FASTAttributes { set; }
        NameValueCollection Query { get; }
    }
}
