using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Corbis.DisplayText.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IDisplayTextView
    {
        Dictionary<int, DisplayTextEntity> DisplayTextEntities { set; }
    }
}
