using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Presenters.MediaSetSearch
{
    public interface IMediaSetsProducts : IView
    {
        List<MediaSet> MediasetList { set; }
    }
}
