#region File Header
// Copyright Corbis Corporation 2001-2009							
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Origination: Mark Kola
#endregion


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using Corbis.Framework.Logging;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters;
using Corbis.Web.Utilities;
    
namespace Corbis.Web.UI
{
    /// <summary>
    /// This is the Master Page Base for all corbis master pages
    /// </summary>
    public class CorbisMasterBase : System.Web.UI.MasterPage
    {
        
        ILogging loggingContext;



        public ILogging LoggingContext
        {
            get { return loggingContext; }
        }

        public Profile Profile
        {
            get { return Profile.Current; }
        }

        public CorbisMasterBase()
        {
            
            loggingContext = new LoggingContext(new List<string>());
        }

        protected override void OnInit(EventArgs e)
        {
            #region MAJOR BIT OF TEXT EXPLAINING REALLY IMPORTANT STUFF (Beta site code)

            // We added this code to handle access to the beta site.  If the user is 
            // accessing the site using a beta host (pro.corbisbeta.com, for example)
            // we'll make sure they've been authorized to access the beta site.  We'll do 
            // this by validating their username and password, and then verifying that their
            // Member account has been set up for beta access.  If they have, we'll drop a 
            // cookie on their browser and the code below will check for the presence of
            // that cookie before letting them proceed.  If no cookie exists (and they are 
            // accessing the site through a beta host) we'll redirect them to beta.aspx to
            // log in.

            // Some things to be aware of:
            //  
            //  We won't support bookmarking.  All access to the beta site goes through the front
            //  door every time.
            //
            //  We considered making the name of the beta host configurable but decided against it
            //  because we didn't want to indlude environment-specific values in config files.  If you're
            //  reading this and think that was a bad decision, you can bite me.  Walk a mile in my
            //  moccasins before you judge.
            //  [You bite me dude! We're putting it in the config! -Jarett 12/4/08] :)
            //
            //  The mecahinsm for managing who has access is still in flux but will most likely be a 
            //  service call to the Member service.  The intent is to give the UX team control over access
            //  and get development out of the way.
            //  [See the BetaAccessManager tool -jf]
            //
            //  Darren Davis - 10/09/08

            #endregion

            // Are we accessing the code through a BETA host?
            if (IsInBeta())
            {
                if (!ClientIPHelper.GetClientIpAddress().StartsWith("172.16.") && !ClientIPHelper.GetClientIpAddress().StartsWith("10.0."))
                {

                    //If they haven't logged into the beta site during this browser session:
                    if (!IsAuthorizedForBeta() && Page.Request.Url.LocalPath != SiteUrls.BetaLogin)
                    {
                        Response.Redirect(SiteUrls.BetaLogin, true);
                    }
                    
                }
            }

            string environment = ConfigurationManager.AppSettings["Environment"];
            if (Page.Request.Url.LocalPath != SiteUrls.UnsupportedBrowser && (string.IsNullOrEmpty(environment) || environment.ToLower() != "dev"))
            //if (Page.Request.Url.LocalPath != SiteUrls.UnsupportedBrowser)
            
            {
                // safari 3 and above have "version" in the user agent string. older version do not.
                // see http://developer.apple.com/internet/safari/faq.html#acnchor2
                if ((Page.Request.Browser.Browser == "IE" && Page.Request.Browser.MajorVersion < 7) || (Page.Request.Browser.Browser == "Firefox" && Page.Request.Browser.MajorVersion < 3) || (Page.Request.Browser.Browser.Contains("Safari") && !Page.Request.UserAgent.ToLower().Contains("(khtml, like gecko) version/")))
                {
                    Response.Redirect(SiteUrls.UnsupportedBrowser, true);
                }
            }
            
            base.OnInit(e);
        }


        #region Public Methods

        /// <summary>
        /// Used to add another script to a content page.  Content pages cannot have there own
        /// links.
        /// </summary>
        /// <param name="script">virtual path of the script</param>
        /// <param name="linkId">ID as it will be viewed on the page.</param>
        public void AddScriptToPage(string script, string linkId)
        {
            if (this.Page.Header != null)
            {
                script = script.Contains("?") ? script + "&" : script + "?";
                script = script + "v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string scriptBlock = "<Script type=\"text/javascript\" language=\"javascript\" src=\"" + ResolveUrl(script) + "\"></Script>";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), linkId, scriptBlock, false);
            }
            else
            {
                LoggingHelper.LogInformationMessage(
                    String.Format(CultureInfo.InvariantCulture,
                    "The following script {0} couldn't be added as part of the Master Page." +
                    " This could be due to the Header not being properly Initialized",
                    script));
            }
        }

        public TimeSpan GetFormsAuthTimeout()
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            AuthenticationSection authSection = (AuthenticationSection)config.GetSection("system.web/authentication");
            FormsAuthenticationConfiguration formsAuth = authSection.Forms;
            TimeSpan ts = formsAuth.Timeout;
            return ts;
        }
        #endregion

        #region Beta Access Methods

        private const string BetaAccessConfigSetting = "InBetaMode";

        private static bool IsAuthorizedForBeta()
        {
            return BetaSiteAuthentication.IsAuthorizedToUseBeta();
        }


        public string ServerInfo
        {
            get
            {
                return string.Format("{0}-{1}-{2}",
                Assembly.GetExecutingAssembly().GetName().Version,
                Server.MachineName,
                DateTime.Now);

            }
        }

        // Whether we are in beta or not is determined by a config setting
        internal static bool IsInBeta()
        {
#if DEBUG
            // In debug mode, do not show beta page
            return false;
#else
            // We are in beta only if the setting exists and does has a value of "true"
            string betaAccessSetting = ConfigurationManager.AppSettings[BetaAccessConfigSetting];
            return !string.IsNullOrEmpty(betaAccessSetting) && betaAccessSetting.ToLower() == "true";
#endif        
        }

        #endregion

    }
}
