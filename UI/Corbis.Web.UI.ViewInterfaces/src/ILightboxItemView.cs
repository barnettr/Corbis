using System;
using System.Collections.Generic;
using Corbis.Lightbox.Contracts.v1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface ILightboxItemView
    {
        /// <summary>
        /// 
        /// </summary>
        List<Corbis.Lightbox.Contracts.v1.Lightbox> LightboxItemList { set; }
        string SelectedValue { get; set; }
    }
}
