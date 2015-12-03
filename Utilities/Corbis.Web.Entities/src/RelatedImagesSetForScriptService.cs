using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;

namespace Corbis.Web.Entities
{
    public class RelatedImagesSetForScriptService
    {
        #region Private Variables
        private string _name;
        private string _description;
        private ImageMediaSetType _mediaType;
        private string _mediaSetId;
        private List<DisplayImageForRelatedImages> _displayImageList;
        #endregion Private Variables

        #region Constructor

        public RelatedImagesSetForScriptService()
        {
            this._name = string.Empty;
            this._description = string.Empty;
            this._mediaType = ImageMediaSetType.Unknown;
            this._mediaSetId = string.Empty;
            this._displayImageList = new List<DisplayImageForRelatedImages>();
        }

        public RelatedImagesSetForScriptService(string name, string description, ImageMediaSetType mediaType, string mediaSetId, List<DisplayImageForRelatedImages> displayImageList)
        {
            this._name = name;
            this._description = description;
            this._mediaType = mediaType;
            this._mediaSetId = mediaSetId;
            this._displayImageList = displayImageList;
        }

        #endregion Constructor

        #region Public Properties
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }
        public ImageMediaSetType MediaType
        {
            get
            {
                return this._mediaType;
            }
            set
            {
                this._mediaType = value;
            }
        }

        public string MediaSetId
        {
            get
            {
                return this._mediaSetId;
            }
            set
            {
                this._mediaSetId = value;
            }
        }

        public List<DisplayImageForRelatedImages> DisplayImageList
        {
            get
            {
                return this._displayImageList;
            }
            set
            {
                this._displayImageList = value;
            }
        }

        #endregion Public Properties
    }
}
