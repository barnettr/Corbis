using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Web.UI.Lightboxes.ViewInterfaces
{
    public interface ITransferLightboxView
    {
        int LightboxId { get; }
        List<string> AssociateUserNames { get; }
        bool RemoveSource { get; }
    }
}
