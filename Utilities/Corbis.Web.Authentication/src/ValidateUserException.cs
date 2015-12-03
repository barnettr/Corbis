using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Authentication
{
    /// <summary>
    /// An exception that contains the reason ValidateUser failed 
    /// in the StatusCode property
    /// </summary>
    public class ValidateUserException : ApplicationException
    {
        private ProviderValidateUserStatus _statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateUserException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="status">The reason the call to ValidateUser failed</param>
        public ValidateUserException(string message, ProviderValidateUserStatus status) : base(message)
        {
            _statusCode = status;
        }

        /// <summary>
        /// The reason the call to ValidateUser failed
        /// </summary>
        public ProviderValidateUserStatus StatusCode
        {
            get { return _statusCode; }
        }
    }

    /// <summary>
    /// Status of a <see cref="ValidateUserException"/> after calling ValidateUser
    /// </summary>
    public enum ProviderValidateUserStatus
    {
        /// <summary>
        /// Status has not been set
        /// </summary>
        None,
        /// <summary>
        /// Username does not exist and no registration requests are pending
        /// </summary>
        InvalidUserName,
        /// <summary>
        /// Username exists, but password is incorrect
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// There is an existing registration request pending for this user
        /// </summary>
        RegistrationRequestPending,
        /// <summary>
        /// An unknown exception ocurred
        /// </summary>
        ProviderError,
        /// <summary>
        /// User was successfully validated
        /// </summary>
        UserValidated
    }
}
