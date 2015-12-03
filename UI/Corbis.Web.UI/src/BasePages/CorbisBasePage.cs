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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Resources;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using Corbis.Framework.Logging;
using Corbis.Framework.Globalization;
using Themes = Corbis.Membership.Contracts.V1.Theme;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Properties;
using Corbis.Web.Entities;
using Corbis.Web.Authentication;
using Corbis.Web.Utilities;
using Corbis.Web.Analytics;
using Corbis.WebAnalytics.Contracts.V1;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Web.UI.Controls;
using System.Web.UI.WebControls;
using Corbis.Web.Validation.Integration.AspNet;
using Resources;
using Corbis.Web.Utilities.StateManagement;
using Yahoo.Yui.Compressor.MsBuild;
using System.IO;

namespace Corbis.Web.UI
{
    /// <summary>
    /// Used to create a consistent foundation for all web pages
    /// </summary>
    public partial class CorbisBasePage : System.Web.UI.Page, IView
    {

        #region Fields

		private ILogging loggingContext;
		private IList<ValidationDetail> validationErrors;
		private bool? requiresSSL = false;
        private bool isAccountPage;
        private Dictionary<string, string> analyticsData;

		private const string DefaultCulture = "en-US";
		public const string UScountryCode = "US";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CorbisBasePage class.
        /// </summary>
        public CorbisBasePage()
        {

            List<string> categories = new List<string>();
            categories.Add(Settings.Default.LoggingCategory);
            SetGuidAsCategory(categories);
            loggingContext = new LoggingContext(categories);
            analyticsData = new Dictionary<string, string>();
        }

        #endregion

        #region Properties

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string HttpUrl
        {
            get
            {
                return Settings.Default.HttpUrl;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string HttpsUrl
        {
            get
            {
                return Settings.Default.HttpsUrl;
            }
        }

        public bool IsAccountPage
        {
            get
            {
                return isAccountPage;
            }
            set
            {
                isAccountPage = value;
            }
        }

        public ILogging LoggingContext
        {
            get
            {
                return loggingContext;
            }
            set
            {
                if (value == null)
                {
                    loggingContext = new LoggingContext(new List<string>());
                    return;
                }

                if (loggingContext == value)
                    return;
                loggingContext = value;
            }
        }

		public IList<ValidationDetail> ValidationErrors
		{
			get { return validationErrors; }
			set 
			{
				validationErrors = value;
				if (validationErrors != null && validationErrors.Count > 0)
				{
					SetValidationErrors(validationErrors);
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public Dictionary<string, string> AnalyticsData
        {
            get { return analyticsData; }
            set { analyticsData = value; }
        }

        public Profile Profile
        {
            get { return Profile.Current; }
        }

        public int ProfileCookieExpireDays
        {
            get { return Convert.ToInt32(WebConfigurationManager.AppSettings["ProfileCookieExpiryTimeInDays"]); }
        }

        /// <summary>
        /// Looks at the Attribute for the class
        /// and returns whether the class is a login
        /// </summary>
        /// <returns></returns>
        public string RedirectPage
        {
            get
            {
                return string.Empty;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased", MessageId = "Member")]
        public bool? RequiresSSL
        {
            get
            {
                return requiresSSL;
            }
            set
            {
                requiresSSL = value;
            }
        }

        public string ServerRootUrl
        {
            get
            {
                string port = Request.Url.Port.ToString();

                if (!string.IsNullOrEmpty(port) && port == "80")
                {
                    port = string.Empty;
                }
                else
                {
                    port = string.Concat(":", port);
                }

                string url = string.Concat("http://", Request.Url.Host, port);

                return url;
            }
        }

        #endregion

		#region Static methods


        public static void OutputEnumClientScript<T>(Control control)
		{
			Dictionary<T, string> localizedEnum = GetEnumDisplayTexts<T>();
			int enumMembersCount = localizedEnum.Count;
			Type t = typeof(T);
			string enumJs = "";
			string namespaceParts = "";
			foreach (string namespacePart in t.Namespace.Split('.'))
			{
				namespaceParts += (namespaceParts==""? "": ".") + namespacePart;
				enumJs += String.Format("if (!{0}) Type.registerNamespace('{0}');\r\n", namespaceParts);
			}
			enumJs += String.Format("{0} = function() {{}};\r\n", t.FullName);
			enumJs += String.Format("{0}.prototype = {{\r\n", t.FullName);
			int enumIndex = 1;

			foreach (T enumValue in Enum.GetValues(t))
			{
				enumJs += String.Format("{0}: {1}{2} \r\n", enumValue.ToString(), enumValue.GetHashCode(), (enumIndex == enumMembersCount? "": ","));
				enumIndex++;
			}

			enumJs += "}\r\n";
            enumJs += String.Format("if (!{0}.__enum) {0}.registerEnum('{0}');\r\n", t.FullName);
			enumJs += String.Format("{0}.toLocalizedString = function(value){{", t.FullName);
			enumJs += "switch(value){\r\n";

			foreach (KeyValuePair<T, string> enumValue in localizedEnum)
			{
				enumJs += String.Format("case {0}: return '{1}'; \r\n", enumValue.Key.GetHashCode(), StringHelper.EncodeToJsString(enumValue.Value));
			}

			enumJs += "}\r\n}\r\n";

			ScriptManager.RegisterStartupScript(control, control.GetType(), t.FullName + "ClientScript", enumJs, true);
		}

        public static string EncodeToJsString(string stringToEncode)
        {
            return StringHelper.EncodeToJsString(stringToEncode);
        }

		#endregion

		#region Overrides

		#region Setting Culture

		/// <summary>
        /// override to set the current culture to the user's preferred culture
        /// </summary>
        protected override void InitializeCulture()
		{
		    base.InitializeCulture();

		    //NOTE: PRIORITY FOR GETTING LANGUAGE IS
		    // 1. Language selector cookie
		    // 2. Logged in user profile's language 
            // 3. QueryString Parameter
            // 4. Requesting Browser's Languages

		    CultureInfo cultureInfo = null;

		    //Get culture from cookie
		    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
		    string userPrefCulture = stateItems.GetStateItemValue<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, StateItemStore.Cookie);
		    if (userPrefCulture != null)
		    {
		        cultureInfo = GetCultureInfo(userPrefCulture);
		    }

		    //Get culture from profile
		    if (cultureInfo == null && !String.IsNullOrEmpty(Profile.CultureName))
		    {
		        cultureInfo = GetCultureInfo(Profile.CultureName);
		    }

		    //Get culture from QueryString parameter
		    if (cultureInfo == null && Request.QueryString["lcd"] != null)
		    {
		        cultureInfo = GetCultureInfo(Request.QueryString["lcd"]);
		    }

		    //Get culture from browser
		    if (cultureInfo == null)
		    {
		        if (Request.UserLanguages != null && Request.UserLanguages.GetLength(0) > 0)
		        {
		            foreach (string userLanguage in Request.UserLanguages)
		            {
		                cultureInfo = GetCultureInfo(userLanguage);

		                if (cultureInfo != null)
		                {
		                    break;
		                }
		            }
		        }
		    }

		    //Get default culture
		    if (cultureInfo == null)
		    {
		        cultureInfo = new CultureInfo(DefaultCulture);
		    }

		    //set the culture of the current thread
		    Language.CurrentCulture = cultureInfo;
		    Language.CurrentCulture.NumberFormat.CurrencySymbol = string.Empty;
		    string cssName = "/Stylesheets/language/" + cultureInfo.EnglishName.Substring(0, cultureInfo.EnglishName.IndexOf('(') - 1) + "-" +
		                     cultureInfo.ThreeLetterWindowsLanguageName + ".css";
		    //string localPath = Server.MapPath(cssName);
		    //if (!File.Exists(localPath))
		    if (cultureInfo.ThreeLetterWindowsLanguageName == "ENU")
		        return;
		    string languageCss = string.Format(@"<link href='{0}' type='text/css' rel='stylesheet' />", cssName);
		    ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "languageCSS", languageCss, false);
		}

        /// <summary>
        /// Gets the Culture Info for the supplied language
        /// </summary>
        /// <param name="userLanguage"></param>
        /// <param name="useUserOverride"></param>
        /// <returns></returns>
        private static CultureInfo GetCultureInfo(string userLanguage)
        {
            CultureInfo result = null;

            #region Get User Language Name

            int semiColonIndex = userLanguage.IndexOf(";");

            if (semiColonIndex != -1)
            {
                userLanguage = userLanguage.Substring(0, semiColonIndex);
            }

            #endregion

			try
			{
				//if we have culture neutral language

                CultureInfo currentCulture = new CultureInfo(userLanguage);
                if (currentCulture.IsNeutralCulture)
                {
                    userLanguage = currentCulture.TextInfo.CultureName;
                }

			    Language language = Language.GetByCultureName(userLanguage);

				if (language != null)
				{
					result = new CultureInfo(userLanguage);
				}
			}
			catch { }
			
			return result;
        }

        #endregion

        /// <summary>
        /// PreInit
        /// </summary>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            SetThemeOnPage();
        }

        protected override void OnInit(EventArgs e)
        {
            SetupJavascriptServerVariables();
            PasswordChangeRequired();
            SetUrlPath();
            base.OnInit(e);
            
        }
        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            AdjustThemeCssHeadSequence();
        }

        /// <summary>
        /// to adjust the sequence of css file
        /// </summary>
        private void AdjustThemeCssHeadSequence()
        {
            //todo: if we have more items, we will store them into a list and handle them in a better way "not hard coded"
            //List<string> topLinks = new List<string>() { "Master.css", "MasterBase.css" };
            //List<string> bottomLinks = new List<string>() { "iestyles.css", "ie7styles.css", "ie6styles.css" };

            //int masterIndex = GetCssIndex("Master.css");
            //int masterBaseIndex = GetCssIndex(Stylesheets.MasterBase);
            //if (masterIndex > -1 && masterBaseIndex > -1)
            //{
            //    HtmlLink masterLink = Page.Header.Controls[masterIndex] as HtmlLink;
            //    HtmlLink masterBaseLink = Page.Header.Controls[masterBaseIndex] as HtmlLink;
            //    Page.Header.Controls.Remove(masterLink);
            //    Page.Header.Controls.Remove(masterBaseLink);
            //    Page.Header.Controls.AddAt(0, masterBaseLink);
            //    Page.Header.Controls.AddAt(1, masterLink);
                
            //}
            int ieIndex = GetCssIndex("iestyles.css");
            int ie7Index = GetCssIndex("ie7styles.css");
            int ie6Index = GetCssIndex("ie6styles.css");

            HtmlLink ieLink = (ieIndex > -1) ? Page.Header.Controls[ieIndex] as HtmlLink : null;
            HtmlLink ie7Link = (ie7Index > -1) ? Page.Header.Controls[ie7Index] as HtmlLink : null;
            HtmlLink ie6Link = (ie6Index > -1) ? Page.Header.Controls[ie6Index] as HtmlLink : null;

            if (ieLink != null)
                Page.Header.Controls.Remove(ieLink);
            if (ie7Link != null)
                Page.Header.Controls.Remove(ie7Link);
            if (ie6Link != null)
                Page.Header.Controls.Remove(ie6Link);

            if (ieLink != null)
                Page.Header.Controls.Add(ieLink);
            if (ie7Link != null)
                Page.Header.Controls.Add(ie7Link);
            if (ie6Link != null)
                Page.Header.Controls.Add(ie6Link);
        }
        private int GetCssIndex(string cssName)
        {
            HtmlLink curLink;
            for (int i = 0; i < Page.Header.Controls.Count; i++)
            {
                curLink = Page.Header.Controls[i] as HtmlLink;
                if (curLink != null)
                {
                    if (curLink.Href.Contains(cssName))
                        return i;
                }
            }
            return -1;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //AnalyticsData.Add(EventDataType.PageName.ToString(), Request.FilePath);
            this.SetAnalytics();
            AnalyticsHelper.LogWebAnalyticsEvent(AnalyticsLoggingLocation.All, EventType.PageLoaded, AnalyticsData);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            RegisterSafariAjaxFixJS();
        }
        
        /// <summary>
        /// This method is overridden to solve the problem os composite 
        /// controls being able to have a validation group assigned.
        /// For example:  The address custom control needs to be assigned 
        /// a ValidationGroup sometimes.  Since it didn't inherit from 
        /// base validator, it needed to have this method overridden.
        /// This is almost the same exact code as the framework, except with
        /// noted exception in the method.
        /// </summary>
        /// <param name="validationGroup"></param>
        public override void Validate(string validationGroup)
        {
            base.Validate(validationGroup);
            if (this.Page.Validators != null)
            {
                ValidatorCollection validators = new ValidatorCollection();
                for (int i = 0; i < this.Page.Validators.Count; i++)
                {
                    if (this.Page.Validators[i] is IGroupValidator &&
                        ((IGroupValidator)this.Page.Validators[i]).ValidationGroup == validationGroup)
                    {
                        this.Page.Validators[i].Validate();
                    }
                    else if (this.Page.Validators[i] is BaseValidator &&
                        ((BaseValidator)this.Page.Validators[i]).ValidationGroup == validationGroup)
                    {
                        BaseValidator validator = this.Page.Validators[i] as BaseValidator;
                        validator.Validate();

                        // find the containing row
                        Control control = validator.FindControl(validator.ControlToValidate);
						if (control != null && //make sure validator is attached to a control
							control.Parent != null && // HtmlTableCell
                            control.Parent.Parent != null && // HtmlTableRow
                            control.Parent.Parent is HtmlTableRow)
                        {
                            // set CSS style on the control's row container
                            if (!validator.IsValid)
                            {
                                ((HtmlTableRow)control.Parent.Parent).Attributes["class"] = "FormRow ErrorRow";
                            }
                            else
                            {
                                ((HtmlTableRow)control.Parent.Parent).Attributes["class"] = "FormRow";
                            }
                        }
                    }
                }
            }
        }


        #endregion

        #region Public methods

        public static string GetResourceString(string classKey, string resourceKey)
        {
            string displayText = String.Empty;
            if (!string.IsNullOrEmpty(classKey) 
                && !string.IsNullOrEmpty(resourceKey)
                && HttpContext.Current != null)
            {
                try
                {
                    displayText = HttpContext.GetGlobalResourceObject(classKey, resourceKey).ToString();
                }
                catch { }
            }
            return displayText;

        }

        public string GetLocalResourceString(string resourceKey)
        {
            string displayText = String.Empty;
            if (!String.IsNullOrEmpty(resourceKey))
            {
                try
                {
                    displayText = (string)GetLocalResourceObject(resourceKey);
                }
                catch { }
                if (String.IsNullOrEmpty(displayText))
                {
                    // If we don't have any text, look in the general global resource file
                    try
                    {
                        CorbisBasePage.GetResourceString("Resources.Resource", resourceKey);
                    }
                    catch { }
                }
            }
            return displayText;
        }
        #endregion


        #region Protected Methods

        /// <summary>
        /// Helper method that gets a querystring value based on a key.
        /// </summary>
        /// <param name="key">string</param>
        /// <returns>string</returns>
        protected string GetQueryString(string key)
        {
            string queryStringValue = string.Empty;

            if (!StringHelper.IsNullOrTrimEmpty(this.Request.QueryString[key]))
            {
                queryStringValue = this.Request.QueryString[key];
            }

            return queryStringValue;
        }

        /// <summary>
        /// Sets the validation error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="invalidControlName">Name of the invalid control.</param>
        /// <param name="errorEnumValue">The error enum value.</param>
        /// <param name="showInSummary">if set to <c>true</c> [show in summary].</param>
        /// <param name="showHilite">if set to <c>true</c> [show hilite].</param>
        public virtual void SetValidationError<T>(
            string invalidControlName, 
            T errorEnumValue,
            bool showInSummary,
            bool showHilite)
        {
            Control invalidControl = null;
            System.Reflection.FieldInfo invalidControlFieldInfo =
                this.GetType().GetField(
                invalidControlName,
                System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public);
            if (invalidControlFieldInfo != null)
            {
                invalidControl = invalidControlFieldInfo.GetValue(this) as Control;
            }
            if (invalidControl != null)
            {
                this.SetValidationHubError<T>(
                    invalidControl,
                    errorEnumValue,
                    showInSummary,
                    showHilite);
            }
        }

        /// <summary>
        /// When overriden in a derived class, sets an error on the view.
        /// /// </summary>
        /// <typeparam name="T">
        /// The type of Enum that contains the localized error text
        /// </typeparam>
        /// <param name="control">Control with error</param>
        /// <param name="errorEnumValue">
        /// The Enum value that contains the localized error text
        /// </param>
        /// <param name="showInSummary">if set to <c>true</c> [show in summary].</param>
        /// <param name="showHilite">if set to <c>true</c> [show hilite].</param>
        /// <exception cref="NotImplementedException">
        /// Thrown if the method is not overridden
        /// </exception>
        public virtual void SetValidationHubError<T>(
            Control control,
            T errorEnumValue,
            bool showInSummary,
            bool showHilite)
        {
            throw new NotImplementedException();
        }

		public void SetValidationErrors(IList<ValidationDetail> validationErrors)
		{
			SetValidationErrors(validationErrors, Page.Validators);
		}

		public void SetValidationErrors(IList<ValidationDetail> validationErrors, ValidatorCollection validators)
		{
			List<IValidator> pageValidators = new List<IValidator>();
			foreach (IValidator pageValidator in validators)
			{
				pageValidators.Add(pageValidator);
			}

			foreach (ValidationDetail validationError in validationErrors)
			{
				IValidator matchValidator = pageValidators.Find
				(
					new Predicate<IValidator>
					(
						delegate(IValidator validator)
						{
                            if (validator is PropertyProxyValidator)
                            {
                                PropertyProxyValidator proxyValidator = validator as PropertyProxyValidator;
                                if (proxyValidator != null &&
                                    proxyValidator.Enabled == true &&
                                    proxyValidator.Visible == true &&
                                    proxyValidator.PropertyName == validationError.Key &&
                                    proxyValidator.SourceTypeName == validationError.Tag)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else if (validator is ParameterProxyValidator)
                            {
                                ParameterProxyValidator parameterValidator = validator as ParameterProxyValidator;
                                if (parameterValidator != null &&
                                    parameterValidator.Enabled == true &&
                                    parameterValidator.Visible == true &&
                                    parameterValidator.OperationName == validationError.Key &&
                                    parameterValidator.ParameterName == validationError.Tag)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
						}
					)
				) as IValidator;
				if (matchValidator != null)
				{
					matchValidator.IsValid = false;
					matchValidator.ErrorMessage = validationError.Message;
				}
			}
		}

        /// <summary>
        /// move viewstate to the bottom of the page
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {

            System.IO.StringWriter stringWriter = new System.IO.StringWriter();

            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            base.Render(htmlWriter);

            string html = stringWriter.ToString();

            int viewStateStart = html.IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\"");

            if (viewStateStart >= 0 && html.IndexOf("</form>") > 0)
            {

                int viewStateEnd = html.IndexOf("/>", viewStateStart) + 2;

                string viewstateInput = html.Substring(viewStateStart, viewStateEnd - viewStateStart);

                html = html.Remove(viewStateStart, viewStateEnd - viewStateStart);

                int formEnd = html.IndexOf("</form>") - 1;

                if (formEnd >= 0)
                {
                    html = html.Insert(formEnd, viewstateInput);
                }

            }

            writer.Write(html);

        }
        #endregion

        #region Private Methods

        private void SetupJavascriptServerVariables()
        {
            System.Web.Configuration.HttpCookiesSection httpCookiesSection =
                (System.Web.Configuration.HttpCookiesSection)WebConfigurationManager.GetSection("system.web/httpCookies");
            string serverVars = "var HttpUrl = '" + Settings.Default.HttpUrl + "'; var HttpsUrl = '" +
                Settings.Default.HttpsUrl + "'; var CookieDomain = '" + httpCookiesSection.Domain + "'; var localizedWordToday = '"+EncodeToJsString(GetResourceString("Resource","Today"))+"';";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ServerVariables", serverVars, true);
        }

        private void RegisterSafariAjaxFixJS()
        {
            // This fix is only for webkit browsers
            if (IsSafariBrowserRequest())
            {
                ScriptManager.RegisterClientScriptInclude(this.Page, this.GetType(), "SafariHack", "/scripts/MicrosoftAjaxSafari3Fix.js");
            }
        }
        protected bool IsSafariBrowserRequest()
        {
            return Request.Browser.Browser.Contains("WebKit") || Request.Browser.Browser.Contains("AppleMAC-Safari");

        }
        private void PasswordChangeRequired()
        {
            if (IsAccountPage &&
                Profile.IsAuthenticated &&
                Profile.PasswordChangeRequired &&
                Request.Url.AbsolutePath != ResolveUrl(SiteUrls.ChangePassword))
            {
                Response.Redirect(SiteUrls.ChangePassword);
            }
        }

        private void SetUrlPath()
        {
            if (this.RequiresSSL.HasValue)
            {
                if (this.RequiresSSL == true && !Request.IsSecureConnection)
                {
                    // Redirect to this page under SSL
                    Response.Redirect(this.HttpsUrl + Request.RawUrl, true);
                return;
                }

                if (RequiresSSL == false && Request.IsSecureConnection)
                {
                    Response.Redirect(this.HttpUrl + Request.RawUrl, true);
                    return;
                }
            }
        }

        private void SetGuidAsCategory(List<string> categories)
        {
            string pageId = Context.Items[Properties.Settings.Default.RequestContextUidKey] as string;
            if (pageId != null)
                categories.Add(pageId);
            else
                categories.Add(Guid.NewGuid().ToString());
        }

        private void SetThemeOnPage()
        {
            //StateItem<Themes> pageThemeStateItem = new StateItem<Themes>(
            //    ThemeSelectorKeys.Name,
            //    ThemeSelectorKeys.ThemeKey,
            //    Themes.None,
            //    StateItemStore.Cookie | StateItemStore.AspSession,
            //    StatePersistenceDuration.NeverExpire
            //    );

            //StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            //pageThemeStateItem.Value = stateItems.GetStateItemValue<Themes>(
            //    pageThemeStateItem.Name,
            //    pageThemeStateItem.Key,
            //    StateItemStore.Cookie | StateItemStore.AspSession);
            
            //if (pageThemeStateItem.Value == Themes.None)
            //{
            //    pageThemeStateItem.Value = Themes.BlackBackground;
            //    stateItems.SetStateItem<Themes>(pageThemeStateItem);
            //}
            //Page.Theme = pageThemeStateItem.Value.ToString();
            Page.Theme = Themes.BlackBackground.ToString();
        }

        /// <summary>
        /// Used to add another script to a content page.  this function is a copy from the master page.
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

        internal void SetAnalytics()
        {
            if (!AnalyticsData.ContainsKey("events"))
            {
                AnalyticsData["events"] = AnalyticsEvents.PageView;
            }

            string[] pageName = Request.FilePath.Split('.');
            
            AnalyticsData["pageName"] = pageName[0].Substring(pageName[0].LastIndexOf('/') + 1);
            
            FunctionalArea functionalArea = this.GetFunctionalAreaFromFilePath();
            AnalyticsData["channel"] = functionalArea.ToString();

            if (functionalArea == FunctionalArea.Errors)
            {
                AnalyticsData["pageType"] = "errorPage";
            }
            else
            {
                AnalyticsData["pageType"] = "standardPage";
            }
            AnalyticsData["prop11"] = Language.CurrentLanguage.LanguageCode;
            AnalyticsData["prop9"] = Profile.IsAnonymous.ToString();
            AnalyticsData["eVar13"] = Profile.CountryCode;
            
            AnalyticsData["eVar14"] = Profile.UserName;
            AnalyticsData["state"] = Profile.AddressDetail.RegionCode;
            AnalyticsData["zip"] = Profile.AddressDetail.PostalCode;

        }
        private FunctionalArea GetFunctionalAreaFromFilePath()
        {
            string[] filePathArray = Request.FilePath.Split('/');
            FunctionalArea functionalArea = Enum.IsDefined(typeof(FunctionalArea), filePathArray[1]) ? (FunctionalArea)Enum.Parse(typeof(FunctionalArea), filePathArray[1]) : FunctionalArea.Homepage;
            return functionalArea;
        }
        #endregion

    }
}
