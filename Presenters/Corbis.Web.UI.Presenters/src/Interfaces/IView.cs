using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Framework.Logging;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IView
    {
        ILogging LoggingContext { get; set; }

		IList<ValidationDetail> ValidationErrors { get; set; }
    }
}
