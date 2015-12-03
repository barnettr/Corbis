using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Corbis.Web.Logging
{
    public static class ActualLogging
    {
        public static void WriteLine(string message)
        {
            Logger.Write(message);
            Console.WriteLine(message);
        }
    }
}
