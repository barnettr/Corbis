using System;
using System.Collections.Generic;
using System.Text;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.RFCD.Contracts.V1;
using Corbis.Web.Utilities;


namespace Corbis.Web.Entities
{
    public class SearchResultProduct
    {
        #region Private Variables

        private SearchDisplayImage _searchDisplayImage;
        private RfcdDisplayImage _rfcdDisplayImage;
        private ImageInLightboxCart _imageInLightboxCart;
        private QuickPicFlags _userQuickPicFlags;
        private List<Permission> _userPermissions;
        Boolean _userIsFastLaneEnabled;
        private Boolean _isRfcd;
        private Int32 _lightboxId;
        private Boolean _isInQuickPick;

        #endregion Private Variables

        #region Constructor

        public SearchResultProduct(
            SearchDisplayImage searchDisplayImage, 
            QuickPicFlags userQuickPicFlags,
            List<Permission> userPermissions,
            Boolean userIsFastLaneEnabled,
            Int32 lightboxId,
            Boolean isInQuickPick,
            Boolean isRfcd)
        {
            this._searchDisplayImage = searchDisplayImage;
            this._imageInLightboxCart = searchDisplayImage.InLightboxCart;
            this._userQuickPicFlags = userQuickPicFlags;
            this._userPermissions = userPermissions;
            this._userIsFastLaneEnabled = userIsFastLaneEnabled;
            this._lightboxId = lightboxId;
            this._isInQuickPick = isInQuickPick;
            this._isRfcd = isRfcd;
        }

        public SearchResultProduct(
            DisplayImage searchDisplayImage,
            ImageInLightboxCart imageInLightboxCart,
            QuickPicFlags userQuickPicFlags,
            List<Permission> userPermissions,
            Boolean userIsFastLaneEnabled,
            Int32 lightboxId,
            Boolean isInQuickPick,
            Boolean isRfcd)
        {
            this._imageInLightboxCart = imageInLightboxCart;
            this._userQuickPicFlags = userQuickPicFlags;
            this._userPermissions = userPermissions;
            this._userIsFastLaneEnabled = userIsFastLaneEnabled;
            this._lightboxId = lightboxId;
            this._isInQuickPick = isInQuickPick;
            this._isRfcd = false;
            this._searchDisplayImage = ConertToSearchDisplayIMage(searchDisplayImage);
            this._searchDisplayImage.QuickPicFlags = userQuickPicFlags;
        }

        private SearchDisplayImage ConertToSearchDisplayIMage(DisplayImage displayImage)
        {
            SearchDisplayImage searchDisplayImage = new SearchDisplayImage();
            searchDisplayImage.AspectRatio = displayImage.AspectRatio;
            searchDisplayImage.Caption = displayImage.Caption;
            searchDisplayImage.Title = displayImage.Title;
            searchDisplayImage.Category = displayImage.Category;
            searchDisplayImage.CorbisId = displayImage.CorbisId;
            searchDisplayImage.CreditLine = string.Empty;
            searchDisplayImage.Url128 = displayImage.Url128;
            searchDisplayImage.Url170 = displayImage.Url170;
            searchDisplayImage.Url256 = displayImage.Url256;
            searchDisplayImage.LicenseModel = displayImage.LicenseModel;
            searchDisplayImage.MediaUid = displayImage.MediaUid;
            searchDisplayImage.MarketingCollection = displayImage.MarketingCollection;
            searchDisplayImage.HasRelatedImages = displayImage.HasRelatedImages;
            searchDisplayImage.IsOutline = displayImage.IsOutline;
            searchDisplayImage.CreditLine = displayImage.CreditLine;
            searchDisplayImage.Location = displayImage.Location;
            return searchDisplayImage;
        }

        public SearchResultProduct(
    RfcdDisplayImage searchDisplayImage,
    ImageInLightboxCart imageInLightboxCart,
    QuickPicFlags userQuickPicFlags,
    List<Permission> userPermissions,
    Boolean userIsFastLaneEnabled,
    Int32 lightboxId,
    Boolean isInQuickPick,
    Boolean isRfcd)
        {
            this._rfcdDisplayImage = searchDisplayImage;
            this._imageInLightboxCart = imageInLightboxCart;
            this._userQuickPicFlags = userQuickPicFlags;
            this._userPermissions = userPermissions;
            this._userIsFastLaneEnabled = userIsFastLaneEnabled;
            this._lightboxId = lightboxId;
            this._isInQuickPick = isInQuickPick;
            this._isRfcd = false;
            this._searchDisplayImage = ConvertRfcdDisplayImageToSearchDisplayImage(searchDisplayImage);
            this._searchDisplayImage.QuickPicFlags = userQuickPicFlags;
        }

        private SearchDisplayImage ConvertRfcdDisplayImageToSearchDisplayImage(RfcdDisplayImage rfcdDisplayImage)
        {
            SearchDisplayImage searchDisplayImage =  new SearchDisplayImage();
            searchDisplayImage.AspectRatio = rfcdDisplayImage.AspectRatio;
            searchDisplayImage.Caption = rfcdDisplayImage.Caption;
            searchDisplayImage.Title = rfcdDisplayImage.Title;
            searchDisplayImage.Category = rfcdDisplayImage.Category;
            searchDisplayImage.CorbisId = rfcdDisplayImage.CorbisId;
            searchDisplayImage.CreditLine = string.Empty;
            searchDisplayImage.Url128 = rfcdDisplayImage.Url128;
            searchDisplayImage.Url170 = rfcdDisplayImage.Url170;
            searchDisplayImage.Url256 = rfcdDisplayImage.Url256;
            searchDisplayImage.LicenseModel = CommonSchema.Contracts.V1.LicenseModel.RF;
            searchDisplayImage.MediaUid = rfcdDisplayImage.MediaUid;
            searchDisplayImage.MarketingCollection = rfcdDisplayImage.MarketingCollection;
            searchDisplayImage.HasRelatedImages = rfcdDisplayImage.HasRelatedImages;
            searchDisplayImage.Location = rfcdDisplayImage.Location;
            searchDisplayImage.CreditLine = rfcdDisplayImage.CreditLine;
            

            
            return searchDisplayImage;
        }

        #endregion Constructor

        #region Private Properties

        private Boolean IsFastLaneEnabled
        {
            get
            {
                return _userIsFastLaneEnabled;
            }
        }

        #endregion Private Properties

        #region Public Properties

        public Boolean ShowRelatedImagesIconForThumbnail
        {
            get
            {
                return HasRelatedImages;
            }
        }

        public Boolean ShowLightboxIconForThumbnail
        {
            get
            {
                return IsInLightbox;
            }
        }

        public Boolean ShowQuickPickIconForThumbnail
        {
            get
            {
                //member has permissions to quick pick one( or more NonApprovalMediaUsageType, mostly one.)
                //an Image can be quick picked for one or more NonApprovalMediaUsageType
                //if both member and image quick picks match then return true.
                if ((int)(_userQuickPicFlags & _searchDisplayImage.QuickPicFlags) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean ShowCalculatorIconForThumbnail
        {
            get
            {
                return !IsRFCD;
            }
        }

        public String MediaRestrictionIndicatorForThumbnail
        {
            get
            {
                if (_searchDisplayImage.PricingIconDisplay == Corbis.Image.Contracts.V1.PricingIcon.DoubleDollar)
                {
                    return Corbis.Image.Contracts.V1.PricingIcon.DoubleDollar.ToString();
                }
                else if (_searchDisplayImage.PricingIconDisplay == Corbis.Image.Contracts.V1.PricingIcon.TripleDollar)
                {
                    return Corbis.Image.Contracts.V1.PricingIcon.TripleDollar.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public String PricingLevelIndicatorForThumbnail
        {
            get
            {
                if (_userPermissions.Contains(Permission.HasPermissionViewPricingLevelAbbreviations))
                {
                    return _searchDisplayImage.PricingLevelIndicator;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public Int32 LightboxID
        {
            get
            {
                return _lightboxId;
            }
        }

        public String CorbisId
        {
            get 
            {
                return _searchDisplayImage.CorbisId;
            }
        }

        public Boolean HasRelatedImages
        {
            get
            {
                return _searchDisplayImage.HasRelatedImages;
            }
        }

        public Boolean IsAvailable
        {
            get
            {
                return _searchDisplayImage.IsAvailable;
            }
        }

        public Boolean IsRestrictedFineArt
        {
            get
            {
                return _searchDisplayImage.IsRestrictedFineArt;
            }
        }

        public Boolean IsInLightbox
        {
            get
            {
                return _imageInLightboxCart.IsInLightbox;
            }
        }

        public Boolean IsRFCD
        {
            get
            {
                return this._isRfcd;
            }
        }

        public Boolean IsInCart
        {
            get
            {
                return _imageInLightboxCart.IsInCart;
            }
        }

        public Boolean IsOutline
        {
            get
            {
                return this._searchDisplayImage.IsOutline;
            }
        }

        public String Url128
        {
            get
            {
                return _searchDisplayImage.Url128;
            }

            set
            {
                _searchDisplayImage.Url128 = value;
            }
        }

        public String Url170
        {
            get
            {
                return _searchDisplayImage.Url170;
            }
            set
            {
                _searchDisplayImage.Url170 = value;
            }
        }

        public String Url256
        {
            get
            {
                return _searchDisplayImage.Url256;
            }
            set
            {
                _searchDisplayImage.Url256 = value;
            }
        }

        public Category Category
        {
            get
            {
                return _searchDisplayImage.Category;
            }
        }

        public Corbis.CommonSchema.Contracts.V1.LicenseModel LicenseModel
        {
            get
            {
                return _searchDisplayImage.LicenseModel;
            }
        }

        public decimal AspectRatio
        {
            get
            {
                return _searchDisplayImage.AspectRatio;
            }
        }

        public String Title
        {
            get
            {
                return StringHelper.ConvertBracketsToItalic(_searchDisplayImage.Title);
            }
        }

        public String Caption
        {
            get
            {
                return StringHelper.ConvertBracketsToItalic(_searchDisplayImage.Caption);
            }
        }

        public String CreditLine
        {
            get
            {
                return _searchDisplayImage.CreditLine;
            }
        }

        public String DatePhotographed
        {
            get
            {
                if (_searchDisplayImage != null)
                {
                    DateTime result = DateTime.Now;
                    bool parseCheck = DateTime.TryParse(_searchDisplayImage.DatePhotographed,out result);
                    if (parseCheck)
                    {
                        return Convert.ToDateTime(_searchDisplayImage.DatePhotographed).ToString("MMMM dd, yyyy");
                    }
                }
                return _searchDisplayImage.DatePhotographed;
            }   
        }

        public String Location
        {
            get
            {
                return _searchDisplayImage.Location;
            }
        }

        public String MarketingCollection
        {
            get
            {
                return _searchDisplayImage.MarketingCollection;
            }
        }

        public Guid MediaUid
        {
            
            get
            {
                return _searchDisplayImage.MediaUid;
            }
        }

        public Boolean IsInQuickPick
        {
            get
            {
                return _isInQuickPick;
            }
        }

        #endregion Public Properties
    }
}
