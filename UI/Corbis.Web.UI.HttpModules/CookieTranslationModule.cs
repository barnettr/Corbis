using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;

namespace Corbis.Web.UI.HttpModules
{
    public class CookieTranslationModule : IHttpModule
    {
        private const string OldPreferencesCookie = "Preferences";
        private const string NewPreferencesCookie = "UserPreferences";
        
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            //get the current context
            HttpApplication context = sender as HttpApplication;

            if (context != null)
            {
                HttpCookie oldPrefCookie = context.Request.Cookies[OldPreferencesCookie];

                if (context.Request[NewPreferencesCookie] == null)
                {
                    //check for the presence of old site cookie info
                    if (oldPrefCookie != null)
                    {
                        //if old site cookie found, create a new cookie for the new site 
                        HttpCookie newPrefCookie = new HttpCookie(NewPreferencesCookie);
                        newPrefCookie.Expires = DateTime.Now.AddYears(1);
                        //newPrefCookie.Domain = System.Web.Configuration.WebConfigurationManager.AppSettings["PreferencesCookieDomain"];
                        
                        if (!string.IsNullOrEmpty(oldPrefCookie.Value))
                        {
                            newPrefCookie.Value = oldPrefCookie.Value;
                        }
                        else if (oldPrefCookie.Values.Count > 0)
                        {
                            newPrefCookie.Values.Add(oldPrefCookie.Values);
                        }

                        newPrefCookie.Values.Add("Version", System.Web.Configuration.WebConfigurationManager.AppSettings["PreferencesCookieVersion"]);

                        //add the new cookie to the Request's Cookies collection because
                        //the later part of the code in the http request pipeline might be looking for this New cookie
                        context.Request.Cookies.Add(newPrefCookie);

                        //add the new cookie to the http response so that it gets 
                        //added to the user's browser
                        context.Response.Cookies.Add(newPrefCookie);
                    }
                }
            }
        }

        #endregion
    }
}
