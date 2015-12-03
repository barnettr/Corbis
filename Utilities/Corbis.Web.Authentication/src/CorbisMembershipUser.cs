using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;

namespace Corbis.Web.Authentication
{
    public class CorbisMembershipUser : MembershipUser
    {
        #region member variables

		private Member _member = null;
        private MembershipServiceAgent _agent = null;

		private string _comment = string.Empty;
		private bool _isApproved = false;
		private DateTime _lastActivityDate = DateTime.MinValue;
		private DateTime _lastLoginDate = DateTime.MinValue;

        #endregion

        #region default constructor

        public CorbisMembershipUser()
        {
			_member = new Member();
            _agent = new MembershipServiceAgent();
        }

		public CorbisMembershipUser(Member member)
        {
            if (member != null)
            {
                _member = member;
            }
            else
            {
                _member = new Member();
            }

            _agent = new MembershipServiceAgent();
        }

        #endregion

		#region public properties

		public Member Member
        {
			get { return _member; }
			set { _member = value; }
        }
        
        #endregion

		#region MembershipUser overrides
		 
		// Summary:
		//     Gets or sets application-specific information for the membership user.
		//
		// Returns:
		//     Application-specific information for the membership user.
		public override string Comment
		{
			get
			{
				return _comment;
			}
			set
			{
				_comment = value;
			}
		}

		//
		// Summary:
		//     Gets the date and time when the user was added to the membership data store.
		//
		// Returns:
		//     The date and time when the user was added to the membership data store.
		public override DateTime CreationDate
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		//
		// Summary:
		//     Gets or sets the e-mail address for the membership user.
		//
		// Returns:
		//     The e-mail address for the membership user.
		public override string Email
		{
			get
			{
				return _member.Email;
			}
			set
			{
				_member.Email = value;
			}
		}

		//
		// Summary:
		//     Gets or sets whether the membership user can be authenticated.
		//
		// Returns:
		//     true if the user can be authenticated; otherwise, false.
		public override bool IsApproved
		{
			get
			{
				return _isApproved;
			}
			set
			{
				_isApproved = value;
			}
		}

		//
		// Summary:
		//     Gets a value indicating whether the membership user is locked out and unable
		//     to be validated.
		//
		// Returns:
		//     true if the membership user is locked out and unable to be validated; otherwise,
		//     false.
		public override bool IsLockedOut
		{
			get
			{
				return false;
			}
		}

		//
		// Summary:
		//     Gets or sets the date and time when the membership user was last authenticated
		//     or accessed the application.
		//
		// Returns:
		//     The date and time when the membership user was last authenticated or accessed
		//     the application.
		public override DateTime LastActivityDate
		{
			get
			{
				return _lastActivityDate;
			}
			set
			{
				_lastActivityDate = value;
			}
		}

		//
		// Summary:
		//     Gets the most recent date and time that the membership user was locked out.
		//
		// Returns:
		//     A System.DateTime object that represents the most recent date and time that
		//     the membership user was locked out.
		public override DateTime LastLockoutDate
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		//
		// Summary:
		//     Gets or sets the date and time when the user was last authenticated.
		//
		// Returns:
		//     The date and time when the user was last authenticated.
		public override DateTime LastLoginDate
		{
			get
			{
				return _lastLoginDate;
			}
			set
			{
				_lastLoginDate = value;
			}
		}

		//
		// Summary:
		//     Gets the date and time when the membership user's password was last updated.
		//
		// Returns:
		//     The date and time when the membership user's password was last updated.
		public override DateTime LastPasswordChangedDate
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		//
		// Summary:
		//     Gets the password question for the membership user.
		//
		// Returns:
		//     The password question for the membership user.
		public override string PasswordQuestion
		{
			get
			{
				return null;
			}
		}

		//
		// Summary:
		//     Gets the name of the membership provider that stores and retrieves user information
		//     for the membership user.
		//
		// Returns:
		//     The name of the membership provider that stores and retrieves user information
		//     for the membership user.
		public override string ProviderName
		{
			get
			{
                return "MembershipProvider";
			}
		}

		//
		// Summary:
		//     Gets the user identifier from the membership data source for the user.
		//
		// Returns:
		//     The user identifier from the membership data source for the user.
		public override object ProviderUserKey
		{
			get
			{
				return _member.MemberUid;
			}
		}

		//
		// Summary:
		//     Gets the logon name of the membership user.
		//
		// Returns:
		//     The logon name of the membership user.
		public override string UserName
		{
			get
			{
				return _member.UserName;
			}
		}

		// Summary:
		//     Updates the password for the membership user in the membership data store.
		//
		// Parameters:
		//   newPassword:
		//     The new password for the membership user.
		//
		//   oldPassword:
		//     The current password for the membership user.
		//
		// Returns:
		//     true if the update was successful; otherwise, false.
		//
		// Exceptions:
		//   System.ArgumentNullException:
		//     oldPassword is null.-or-newPassword is null.
		//
		//   System.ArgumentException:
		//     oldPassword is an empty string.-or-newPassword is an empty string.
		public override bool ChangePassword(string oldPassword, string newPassword)
		{
            throw new NotImplementedException("The method or operation is not implemented.");
        }

		//
		// Summary:
		//     Updates the password question and answer for the membership user in the membership
		//     data store.
		//
		// Parameters:
		//   newPasswordQuestion:
		//     The new password question value for the membership user.
		//
		//   newPasswordAnswer:
		//     The new password answer value for the membership user.
		//
		//   password:
		//     The current password for the membership user.
		//
		// Returns:
		//     true if the update was successful; otherwise, false.
		//
		// Exceptions:
		//   System.ArgumentException:
		//     password is an empty string.-or-newPasswordQuestion is an empty string.-or-newPasswordAnswer
		//     is an empty string.
		//
		//   System.ArgumentNullException:
		//     password is null.
		public override bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		//
		// Summary:
		//     Gets the password for the membership user from the membership data store.
		//
		// Returns:
		//     The password for the membership user.
		public override string GetPassword()
		{
            throw new NotImplementedException("The method or operation is not implemented.");
		}

		//
		// Summary:
		//     Gets the password for the membership user from the membership data store.
		//
		// Parameters:
		//   passwordAnswer:
		//     The password answer for the membership user.
		//
		// Returns:
		//     The password for the membership user.
		public override string GetPassword(string passwordAnswer)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		//
		// Summary:
		//     Resets a user's password to a new, automatically generated password.
		//
		// Returns:
		//     The new password for the membership user.
		public override string ResetPassword()
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		//
		// Summary:
		//     Resets a user's password to a new, automatically generated password.
		//
		// Parameters:
		//   passwordAnswer:
		//     The password answer for the membership user.
		//
		// Returns:
		//     The new password for the membership user.
		public override string ResetPassword(string passwordAnswer)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		//
		// Summary:
		//     Returns the user name for the membership user.
		//
		// Returns:
		//     The System.Web.Security.MembershipUser.UserName for the membership user.
		public override string ToString()
		{
            return _member.UserName;
		}

		//
		// Summary:
		//     Clears the locked-out state of the user so that the membership user can be
		//     validated.
		//
		// Returns:
		//     true if the membership user was successfully unlocked; otherwise, false.
		public override bool UnlockUser()
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		#endregion
	}
}
