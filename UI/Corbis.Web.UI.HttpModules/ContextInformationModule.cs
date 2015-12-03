using System;
using System.Web;

namespace Corbis.Web.UI.HttpModules
{
    public class ContextInformationModule : IHttpModule
    {
        #region Member Variables

        HttpApplication application = null;

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            application = context;

            //subscribe to the begin request event
            application.BeginRequest += new EventHandler(application_BeginRequest);
        }

        private void application_BeginRequest(object sender, EventArgs e)
        {
            //create request context guid for each request so that
            //we can uniquely identify any errors or logs for a particular given
            //context
            application.Context.Items.Add(Properties.Settings.Default.RequestContextUidKey,
                                    Guid.NewGuid().ToString());
        }

        #endregion
    }
}
