using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace Corbis.Web.Entities
{
    public class Membership
    {
        #region Private Members

        private string email;
        private string passwordQuestion;
        private string passwordAnswer;
        private bool isApproved;
        private bool isOnline;

        #endregion

        #region Public Properties

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public bool IsOnline
        {
            get { return IsOnline; }
            set { isOnline = value; }
        }

        #endregion
    }
}
