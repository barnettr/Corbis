using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Controls;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Enlargement.ViewInterfaces
{
    public interface IEnlargementServiceView : IView
    {
        string CorbisId { set; get; }
        int ImageListPageSize { get; }
        string Caller { get; }
        string ImageListQuery { get; }
        int LightboxId { get; }

        int TotalImageCount { get; set; }
        List<string> ImageList { get; set; }
        int ImageListPageNo { get; set; }
    }
}
