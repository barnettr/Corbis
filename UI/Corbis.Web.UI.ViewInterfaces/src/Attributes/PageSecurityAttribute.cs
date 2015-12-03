using System;
using System.Configuration;
using System.Web;


namespace Corbis.Web.UI.Attributes
{
    /// <summary>
    /// Create a page security attribute that is used for login and redirections
    /// This attribute can only be set at the Class Level and can be inherited
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=true,AllowMultiple=false)]
    public class PageSecurityAttribute : Attribute
    {
        #region Members
        private string loginRedirectUrl;
        private bool loginRequired ;
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor sets allows for no security rules
        /// </summary>
        public PageSecurityAttribute()
        {
            loginRedirectUrl = String.Empty;
            loginRequired = false;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Read only property based upon a redirection URL
        /// </summary>
        public bool LoginRequired
        {
            get 
            {
                return loginRequired; 
            }
            set
            {
                loginRequired = value;
            }
        }
        /// <summary>
        /// This is the URL that will be used for a redirection
        /// 
        /// </summary>
        public string RedirectUrl
        {
            get 
            { 
                return loginRedirectUrl; 
            }
            set
            {
                loginRedirectUrl = value;
            }
        }
        #endregion
    }
}
