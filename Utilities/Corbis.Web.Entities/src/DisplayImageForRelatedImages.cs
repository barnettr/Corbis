using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    // This class is desinged to reduce the data being 
    // sent over network when for AJAX Web Services.
    public class DisplayImageForRelatedImages
    {
        #region Private Variables
        private string _corbisId;
        private decimal _aspectRatio;
        private string _url128;
        #endregion Private Variables

        #region Constructor
        public DisplayImageForRelatedImages()
        {
            this._corbisId = string.Empty;
            this._aspectRatio = 0;
            this._url128 = string.Empty;
        }

        public DisplayImageForRelatedImages(string corbisId, decimal aspectRatio, string url128)
        {
            this._corbisId = corbisId;
            this._aspectRatio = aspectRatio;
            this._url128 = url128;
        }
        #endregion Constructor

        #region Public Properties
        public decimal AspectRatio
        {
            get
            {
                return this._aspectRatio;
            }
            set
            {
                this._aspectRatio = value;
            }
        }
        public string CorbisId
        {
            get
            {
                return this._corbisId;
            }
            set
            {
                this._corbisId = value;
            }
        }
        public string Url128
        {
            get
            {
                return this._url128;
            }
            set
            {
                this._url128 = value;
            }
        }
        #endregion Public Properties
    }
}
