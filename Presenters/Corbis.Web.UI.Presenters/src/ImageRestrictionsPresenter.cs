using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Image.Contracts.V1;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.Presenters
{
    public class ImageRestrictionsPresenter:BasePresenter
    {
        private IImageRestrictionsView _view;
        private DisplayImage _image;

        public const string DoubleDollarIconUrl = "/Images/icon_$$_Res.gif";
        public const string TripleDollarIconUrl = "/Images/icon_$$$_Res.gif";

        public ImageRestrictionsPresenter(IImageRestrictionsView restrictionsView, DisplayImage image)
        {
            if (restrictionsView == null)
            {
                throw new ArgumentNullException("restrictionsView", "restrictionsView can't be null");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image", "image can't be null");
            }
            _view = restrictionsView;
            _image = image;
        }

        public void SetRestrictions()
        {
            _view.LanguageName = Language.CurrentLanguage;

            switch (_image.PricingIconDisplay)
            {
                case PricingIcon.DoubleDollar:
                    _view.ShowPricingIcon = true;
                    _view.PricingIconUrl = DoubleDollarIconUrl;
                    break;
                case PricingIcon.TripleDollar:
                    _view.ShowPricingIcon = true;
                    _view.PricingIconUrl = TripleDollarIconUrl;
                    break;
                default:
                    _view.ShowPricingIcon = false;
                    _view.PricingIconUrl = String.Empty;
                    break;
            }

            _view.RestrictionsDataSource = _image.Restrictions;
            _view.ModelRelease = _image.ModelReleaseStatus;

            if (_image.PropertyReleaseStatus)
            {
                _view.PropertyReleaseText = "PropertyReleaseTrue.Text";
            }
            else
            {
                _view.PropertyReleaseText = "PropertyReleaseFalse.Text";
            }

            if (_image.DomesticEmbargoDate > DateTime.Now)
            {
                _view.ShowDomesticEmbargoDate = true;
                _view.DomesticEmbargoDate = ((DateTime)_image.DomesticEmbargoDate).ToString("d");
            }
            else
            {
                _view.ShowDomesticEmbargoDate = false;
            }

            if (_image.InternationalEmbargoDate > DateTime.Now)
            {
                _view.ShowInternationalEmbargoDate = true;
                _view.InternationalEmbargoDate = ((DateTime)_image.InternationalEmbargoDate).ToString("d");
            }
            else
            {
                _view.ShowInternationalEmbargoDate = false;
            }

        }
    }
}
