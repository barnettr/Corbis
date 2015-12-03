using System;
using System.Collections.Generic;
using Corbis.Framework.Logging;

namespace Corbis.Web.Content
{
    public class ContentProvider : IContentProvider
    {
        protected ILogging loggingContext;


        public void SetLoggingBehavior(ILogging log)
        {
            loggingContext = log;
        }

        /// <summary>
        /// Initializes a new instance of the ContentProvider class.
        /// </summary>
        public ContentProvider()
        {
            loggingContext = new LoggingContext(new List<string>());
        }


        

    }
}
