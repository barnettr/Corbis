using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    public class ImageFeedbackInfo
    {
        #region Instance Variables

        private String _licenseModelText;
        private String _corbisID;
        private String _title;
        private String _localContactPhoneNumber;
        private String _localEmailContactUs;
        private String _userName;
        private String _userEmail;
        private String _firstAndLastName;
        private String _userPhoneNumber;

        #endregion

        #region Constructor

        // Default Constructor
        public ImageFeedbackInfo()
        {
            _licenseModelText = String.Empty;
            _corbisID = String.Empty;
            _title = String.Empty;
            _localContactPhoneNumber = String.Empty;
            _localEmailContactUs = String.Empty;
            _userName = String.Empty;
            _userEmail = String.Empty;
            _firstAndLastName = String.Empty;
            _userPhoneNumber = String.Empty;
        }

        // Contructor for Authenticated User
        public ImageFeedbackInfo(
            String licenseModelText,
            String corbisID,
            String title,
            String localContactPhoneNumber,
            String localEmailContactUs,
            String userName,
            String userEmail,
            String firstAndLastName,
            String userPhoneNumber
            )
        {
            _licenseModelText = licenseModelText;
            _corbisID = corbisID;
            _title = title;
            _localContactPhoneNumber = localContactPhoneNumber;
            _localEmailContactUs = localEmailContactUs;
            _userName = userName;
            _userEmail = userEmail;
            _firstAndLastName = firstAndLastName;
            _userPhoneNumber = userPhoneNumber;
        }

        /// <summary>
        /// Constructor for UnAuthenticated User
        /// </summary>
        /// <param name="licenseModelText"></param>
        /// <param name="corbisID"></param>
        /// <param name="title"></param>
        /// <param name="localContactPhoneNumber"></param>
        /// <param name="localEmailContactUs"></param>
        public ImageFeedbackInfo(
            String licenseModelText,
            String corbisID,
            String title,
            String localContactPhoneNumber,
            String localEmailContactUs
            )
        {
            _licenseModelText = licenseModelText;
            _corbisID = corbisID;
            _title = title;
            _localContactPhoneNumber = localContactPhoneNumber;
            _localEmailContactUs = localEmailContactUs;
            _userName = String.Empty;
            _userEmail = String.Empty;
            _firstAndLastName = String.Empty;
            _userPhoneNumber = String.Empty;
        }

        #endregion

        #region Public Properties

        public String LicenseModelText
        {
            get { return _licenseModelText; }
            set { _licenseModelText = value; }
        }

        public String CorbisID
        {
            get { return _corbisID; }
            set { _corbisID = value; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String LocalContactPhoneNumber
        {
            get { return _localContactPhoneNumber; }
            set { _localContactPhoneNumber = value; }
        }

        public String LocalEmailContactUs
        {
            get { return _localEmailContactUs; }
            set { _localEmailContactUs = value; }
        }

        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public String UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        public String FirstAndLastName
        {
            get { return _firstAndLastName; }
            set { _firstAndLastName = value; }
        }

        public String UserPhoneNumber
        {
            get { return _userPhoneNumber; }
            set { _userPhoneNumber = value; }
        }

        #endregion

    }
}
