using System;
using System.Web.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;
using Corbis.Framework.Globalization;
using Corbis.Framework.Logging;
using Corbis.Web.UI.Properties;

namespace Corbis.Web.UI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                CustomErrorsSection customErrors = (CustomErrorsSection)System.Configuration.ConfigurationManager.GetSection("system.web/customErrors");
                if (!customErrors.Mode.Equals(CustomErrorsMode.Off))
                {
                    List<string> categories = new List<string>();
                    categories.Add(Settings.Default.LoggingCategory);
                    ILogging loggingContext = new LoggingContext(categories);

                    Exception ex = (Context.Error.GetBaseException() == null ? Context.Error : Context.Error.GetBaseException());

                    if (ex != null)
                    {
                        if (ex.GetType().Equals(typeof(System.Web.HttpException)))
                        {
                            if (((System.Web.HttpException)ex).GetHttpCode() == 404)
                            {
                                return;
                            }
                        }

                        loggingContext.LogErrorMessage("Unexpected Error", ex.Message, ex);
                    }

                    Server.ClearError();
                    Response.Redirect(SiteUrls.UnexpectedError, false);
                }
            }
            catch (Exception ex2)
            {
                Response.Write(ex2.Message);
            }
        }

        public override string GetVaryByCustomString(System.Web.HttpContext context, string custom) {
            string key = string.Empty;

            if (String.IsNullOrEmpty(custom))
                return key;

            if (custom.Equals("CurrentLanguage"))
            {
                key = Language.CurrentLanguage.LanguageCode;
            }
            return key;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// KevinH 8/25/2008: This is required even though there is no implementation. 
        /// For some reason IE7 starts a new session with every requiest if this isn't here.
        /// </summary>
        public void Session_Start(object sender, EventArgs e) { }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            Corbis.Web.Authentication.Profile.Current = null;
            Corbis.Web.Utilities.StateManagement.StateItemCollection.BeginRequest();
        }

        public void Application_EndRequest(object sender, EventArgs e)
        {
            Corbis.Web.Authentication.Profile.Current = null;
            Corbis.Web.Utilities.StateManagement.StateItemCollection.EndRequest();
        }
    }
}