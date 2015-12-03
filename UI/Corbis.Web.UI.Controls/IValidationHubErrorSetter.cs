using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Web.UI.Controls
{
    public interface IValidationHubErrorSetter
    {
        void SetValidationHubError<T>(
            System.Web.UI.Control control,
            T errorEnumValue,
            bool showInSummary,
            bool showHilite);
    }
}
