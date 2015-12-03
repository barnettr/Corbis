using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Text;
using Corbis.Framework.Logging;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.BetaAccess.ServiceAgents.V1;
using Corbis.BetaAccess.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.Utilities
{
    public class BetaSiteAuthentication
    {
        public const string BetaSessionCookieName = "B_S";
        public const string BetaAccessCookieName = "B_A";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthorizedToUseBeta()
        {
            try
            {
                // Checking for a special value in the url to bypass the beta check.
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;
                if (!string.IsNullOrEmpty(queryString["BetaBypass"]) && 
                    !string.IsNullOrEmpty(ConfigurationManager.AppSettings["BetaBypassKey"]) &&
                    queryString["BetaBypass"] == ConfigurationManager.AppSettings["BetaBypassKey"])
                {
                    return true;
                }

                string betaCookie = GetCookieValue(BetaAccessCookieName);
                if (string.IsNullOrEmpty(betaCookie))
                {
                    return false;
                }
                else
                {
                    //The format of the cookie is IPAddress_UserName.
                    string userIP = ClientIPHelper.GetClientIpAddress();
                    string cookieValue = EncryptionWrapper.Decrypt(betaCookie);

                    int pos = cookieValue.IndexOf("_");

                    string cookieIP = cookieValue.Substring(0, pos);
                    string username = cookieValue.Substring(pos + 1);

                    //Make sure the IP encrypted in the cookie matches the IP of the current request.
                    //If it does, make sure the user has been validated in the current session.
                    //
                    //if (userIP == cookieIP && UserSessionHasBeenValidated(username, userIP))
                    //
                    //It turns out that the user's IP address may not be constant (access via a proxy
                    //for example).  We will not be valicating against the user's IP address, but will
                    //log discrepancies as warnings.
                    if (userIP != cookieIP)
                    {
                        LoggingHelper.LogWarningMessage("IP Addresses do not match during Beta Authorization",
                            string.Format("CookieIP = {0}; UserID = {1}", cookieIP, userIP));
                    }
                    
                    if (UserSessionHasBeenValidated(username, userIP))
                    {
                        return true;
                    }
                    else //The current IP and the IP in the cookie don't match, or the user has been denied access since they last logged in.
                    {
                        StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                        stateItems.DeleteStateItem<string>(CreateStateItem(BetaAccessCookieName, null));
                        return false;
                    }

                }
            }
            // Let's assume that if something has gone wrong getting the value out of the cookie, then it's
            //not valid
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Nake sure the Beta Session cookie exists.  If it does all is good and we move on.  
        /// If not, pass the username and IP address to the UserMayAccessBeta method to verify
        /// and log their access.  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private static bool UserSessionHasBeenValidated(string username, string ipAddress)
        {
            string betaSessionCookie = GetCookieValue(BetaSessionCookieName);
            if (string.IsNullOrEmpty(betaSessionCookie))
            {
                if (UserMayAccessBeta(username, ipAddress))
                {
                    SetBetaSessionCookie();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else //The Session cookie has a value
            {
                if (betaSessionCookie == bool.TrueString)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool AuthenticateUser(string username,
            string password,
            string ipAddress)
        {
            if (UserIsValid(username, password)
                && UserMayAccessBeta(username, ipAddress))
            {
                SetUserCookie(username);
                SetBetaSessionCookie();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Uses call to Membership.UserMayAccessBeta service to determine if the username passed in
        /// has been granted access.  The request and IP are logged whether the access has been granted or not.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool UserMayAccessBeta(string username, string ipAddress)
        {
            IBetaAccessContract betaAccessAgent = new BetaAccessServiceAgent();
            return betaAccessAgent.UserMayAccessBeta(username, ipAddress);
        }

        /// <summary>
        /// Uses existing call to the Membership serivce to determine if the username/password combination are valid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool UserIsValid(string username,
            string password)
        {

            IMembershipContract membershipServiceAgent = new MembershipServiceAgent();

            string passwordHash = MemberPassword.ComputeHashForUserValidation(password);
            ValidateUserStatus status = membershipServiceAgent.ValidateUser(username, passwordHash);

            if (status == ValidateUserStatus.UserValidated 
                || status == ValidateUserStatus.PasswordChangeRequired)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void SetUserCookie(string username)
        {
            try
            {
                string ipAddress = ClientIPHelper.GetClientIpAddress();
                string cookieValue = EncryptionWrapper.Encrypt(string.Concat(ipAddress, "_", username));
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<string> betaAccessStateItem = CreateStateItem(BetaAccessCookieName, cookieValue);
                betaAccessStateItem.Duration = StatePersistenceDuration.NeverExpire;
                stateItems.SetStateItem<string>(betaAccessStateItem);
            }
            catch
            {
            }
        }

        private static void SetBetaSessionCookie()
        {
            try
            {
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<string> betaSessionStateItem = CreateStateItem(BetaSessionCookieName, bool.TrueString);
                stateItems.SetStateItem<string>(betaSessionStateItem);
            }
            catch
            {
            }
        }

        private static string GetCookieValue(string name)
        {
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            return stateItems.GetStateItemValue<string>(name, null, StateItemStore.Cookie);
        }

        private static StateItem<string> CreateStateItem(string name, string value)
        {
            return new StateItem<string>(
                name,
                null,
                value,
                StateItemStore.Cookie);

        }
        public static string BetaAccessUsername
        {
            get
            {
                string userName = String.Empty;
                try
                {
                    string betaCookie = GetCookieValue(BetaAccessCookieName);
                    if (!string.IsNullOrEmpty(betaCookie))
                    {
                        string cookieValue = EncryptionWrapper.Decrypt(betaCookie);
                        int pos = cookieValue.IndexOf("_");
                        userName = cookieValue.Substring(pos + 1);
                    }
                }
                catch { }
                return userName;
            }
        }
    }

}
