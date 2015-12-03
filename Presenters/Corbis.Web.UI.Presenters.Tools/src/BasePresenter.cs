using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using Corbis.Common.FaultContracts;
using Corbis.Framework.Logging;
using Corbis.Common.ServiceFactory;

namespace Corbis.Web.UI.Presenters.Tools
{
    public class BasePresenter
    {
        public CorbisFault HandleServiceException(Exception exception)
        {

            FaultException<CorbisFault> faultException = exception as FaultException<CorbisFault>;

            if (faultException != null)
            {
                return faultException.Detail;
            }

            return null;
        }

        public CorbisFault HandleException(Exception exception, ILogging loggingContext, string subject)
        {
            if (loggingContext == null)
            {
                loggingContext = new LoggingContext(new List<string>());
            }

            CorbisFault corbisFault = HandleServiceException(exception);
            if (corbisFault != null)
            {
                loggingContext.LogErrorMessage(subject, corbisFault.ClientMessage, exception);
            }
            else
            {
                loggingContext.LogErrorMessage(subject, exception);
            }
            return corbisFault;
        }
    }
}
