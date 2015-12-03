using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Corbis.Web.UI.HttpModules
{
    /// <summary>
    /// This module takes incoming requests, maps them to the correct domain (IE: pro.corbis.com instead of an IP address)
    /// Also converts requests to pro.corbis.com/ to pro.corbis.com/default.aspx
    /// Configuration done via 
    /// </summary>
    public class RedirectionModule : IHttpModule
    {
        private string applicationPath;
        private ArrayList redirections;
        private Corbis.Framework.Logging.LoggingContext loggingContext;
        private const string ERROR403 = "403;";
        private const string ERROR404 = "404;";
        private const string HTTPURL = "http://";
        private const string HTTPSURL = "https://";

        public RedirectionModule()
        {
            applicationPath = (HttpRuntime.AppDomainAppVirtualPath.Length > 1) ? HttpRuntime.AppDomainAppVirtualPath : String.Empty;
        }

        private string HandleTilde(string url)
        {
            if ((url == null) || (url.Length < 1))
            {
                return url;
            }
            if (url.StartsWith("^~/"))
            {
                return "^" + applicationPath + url.Substring("^~/".Length - 1);
            }
            else
            {
                if (url.StartsWith("~/"))
                {
                    return applicationPath + url.Substring("~/".Length - 1);
                }
            }
            return url;
        }

        private string GetRequestedUrl(HttpRequest request)
        {
            string requestedUrl = request.RawUrl;

            // check for 404 from .net page
            if (!string.IsNullOrEmpty(request.QueryString["aspxerrorpath"]))
            {
                requestedUrl = request.QueryString["aspxerrorpath"];
            }
            // check for 404 from non-.net page
            if (!string.IsNullOrEmpty(request.ServerVariables["QUERY_STRING"]))
            {
                if (request.ServerVariables["QUERY_STRING"].Contains(ERROR404))
                {
                    // Get the root requested url from the 404
                    string tempString = request.ServerVariables["QUERY_STRING"].Replace(ERROR404, string.Empty).Replace(HTTPURL, string.Empty).Replace(HTTPSURL, string.Empty);
                    requestedUrl = tempString.Substring(tempString.IndexOf("/"));
                }
            }
            // check for 403 for directories
            if (!string.IsNullOrEmpty(request.ServerVariables["QUERY_STRING"]))
            {
                if (request.ServerVariables["QUERY_STRING"].Contains(ERROR403))
                {
                    // Get the root requested url from the 404
                    string tempString = request.ServerVariables["QUERY_STRING"].Replace(ERROR403, string.Empty).Replace(HTTPURL, string.Empty).Replace(HTTPSURL, string.Empty);
                    requestedUrl = tempString.Substring(tempString.IndexOf("/"));
                }
            }
            return requestedUrl;
        }

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            ConfigRedirections redirectionsFromConfig;

            redirections = new ArrayList();

            List<string> categories = new List<string>();
            categories.Add("RedirectionModule");
            loggingContext = new Corbis.Framework.Logging.LoggingContext(categories);

            try
            {

                redirectionsFromConfig = (ConfigRedirections)ConfigurationManager.GetSection("redirections");
                foreach (Redirection redirection in redirectionsFromConfig.Redirections)
                {
                    string targetUrl;

                    targetUrl = redirection.targetUrl;
                    targetUrl = HandleTilde(targetUrl);
                    if (redirection.ignoreCase)
                    {
                        redirection.Regex = new Regex(targetUrl, RegexOptions.IgnoreCase /* | RegexOptions.Compiled*/);
                    }
                    else
                    {
                        redirection.Regex = new Regex(targetUrl/*, RegexOptions.Compiled*/);
                    }
                    redirections.Add(redirection);
                }
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("Redirection config error", ex);
            }

            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app;
            string requestUrl;

            app = sender as HttpApplication;
            requestUrl = GetRequestedUrl(app.Request);
            

            try
            {
                foreach (Redirection redirection in redirections)
                {
                    if (redirection.Regex.IsMatch(requestUrl))
                    {
                        string destinationUrl;

                        destinationUrl = redirection.Regex.Replace(requestUrl, redirection.destinationUrl, 1);
                        destinationUrl = HandleTilde(destinationUrl);

                        if (redirection.permanent)
                        {
                            app.Response.StatusCode = 301; // make a permanent redirect
                            app.Response.AddHeader("Location", destinationUrl);
                            app.Response.End();
                        }
                        else
                        {
                            app.Context.RewritePath(destinationUrl);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("Redirection error", ex);
            }
        }

        #endregion
    }

}
