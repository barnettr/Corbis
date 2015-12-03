using System;
using System.Collections.Generic;
using System.Web.Security;
using WebSecurity = System.Web.Security;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Framework.Logging;
using System.Configuration;

namespace Corbis.Web.Authentication
{
    /// <summary>
    /// This class will be used to override the basic membership implementations
    /// A not supported exception will be added for those instances where the 
    /// membership methods will not be implemented.
    /// </summary>
    public class CorbisMembershipProvider : MembershipProvider
    {
        #region Members
        IMembershipContract _membershipAgent;
        private string applicationName;
        ILogging loggingContext;
        #endregion

        public ILogging LoggingContext
        {
            get
            {
                return loggingContext;
            }
            set
            {
                if (loggingContext == value)
                    return;
                loggingContext = value;
            }
        }

        #region Constructors


        public void SetMembershipService(IMembershipContract membership)
        {
            _membershipAgent = membership;
        }

        /// <summary>
        /// Initializes a new instance of the MembershipProvider class.
        /// </summary>
        public CorbisMembershipProvider()
        {
            _membershipAgent = new MembershipServiceAgent();
            List<string> categories = new List<string>();
            categories.Add("MembershipProvider");
            loggingContext = new LoggingContext(categories);

        }

        #endregion

        /// <summary>
        /// This returns the application name to store the data in. It can also set it
        /// This is ideal if you have multiple sites that need different membership info
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        #region Currently Not Implemented

        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
        /// <summary>
        /// Not Currently Implemented
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not currently implemented
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override WebSecurity.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented here
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }// throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(WebSecurity.MembershipUser user)
        {
            throw new Exception("The method or operation is not implemented."); 
        } 
        #endregion

        #region Overrides

        /// <summary>
        /// Initializes base values for the provider class
        /// </summary>
        /// <param name="name">Name is in the config file as the default provider to use</param>
        /// <param name="config">This is the config entries as they have been brought in</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ConfigurationErrorsException("Membership configuration doesn't exist");
            }

            base.Initialize(name, config);

            string tempApplicationName = config.Get("applicationName");
            if (String.IsNullOrEmpty(tempApplicationName))
            {
                applicationName = "Corbis.Web";
            }
            else
            {
                applicationName = tempApplicationName;
            }
        }
        
        /// <summary>
        /// Get's a user by they're username, we're assuming this 
        /// is the emailaddress for now
        /// </summary>
        /// <param name="username">
        /// The username/email address
        /// </param>
        /// <param name="userIsOnline">
        /// Ignored
        /// </param>
        /// <returns>A <see cref="CorbisMemberShipUser"/></returns>
        public override WebSecurity.MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
            }

            if (!ValidateAgents("GetUser"))
            {
                return null;
            }

            try
            {
                //TODO: Change this to GetUserByUsername once the data are fixed
                Member member = _membershipAgent.GetMemberByUsername(username);
                MembershipUser user = new CorbisMembershipUser(member);
                return user;

            }
            catch (Exception ex)
            {
                this.LoggingContext.LogErrorMessage("MembershipProvider:GetUser", "An unknown service exception occurred", ex);
                throw;
            }
        }

        /// <summary>
        /// Get's a Member by they're memberUid
        /// </summary>
        /// <param name="providerUserKey">
        /// The Member's Uid
        /// </param>
        /// <param name="userIsOnline">
        /// Ignored
        /// </param>
        /// <returns>A <see cref="CorbisMemberShipUser"/></returns>
        public override WebSecurity.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null || 
                providerUserKey.GetType() != typeof(System.Guid) || 
                ((System.Guid)providerUserKey) == Guid.Empty)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidProviderUserKey);
            }

            if (!ValidateAgents("GetUser"))
            {
                return null;
            }

            try
            {
                Member member = _membershipAgent.GetMemberByUid((Guid)providerUserKey);
                MembershipUser user = new CorbisMembershipUser(member);
                return user;
            }
            catch (Exception ex)
            {
                this.LoggingContext.LogErrorMessage("MembershipProvider:GetUser", "An unknown service exception occurred", ex);
                throw;
            }
            
        }

        #endregion

        #region Private Methods

        private bool ValidateAgents(string methodName)
        {
            bool retVal = true;
            if (_membershipAgent == null)
            {
                this.LoggingContext.LogWarningMessage(
                    String.Format("MembershipProvider:{0}", methodName),
                    "Membership ServiceAgent was null");
                retVal = false;
            }

            return retVal;
        }

        #endregion
    }
}
