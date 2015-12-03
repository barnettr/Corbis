using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IContentItemView
    {
        List<ContentItem> ContentItemList { set; }
    }



}
