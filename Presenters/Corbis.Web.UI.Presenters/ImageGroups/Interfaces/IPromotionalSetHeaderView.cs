﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IPromotionalSetHeaderView : IImageGroupHeader  
    {
        string Title { set; }
        string Id { set; }
        string ImageCount { set; }
    }
}
