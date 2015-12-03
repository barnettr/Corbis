using System;
using System.Collections.Generic;
using System.Text;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Framework.Globalization;
using Corbis.Image.Contracts.V1;
using Languages = Corbis.Framework.Globalization.Language;
using Corbis.RFCD.Contracts.V1;

namespace Corbis.Web.UI.Presenters.Pricing
{
    public partial class PricingPresenter
    {
        public void SendRequestForPrice()
        {
            Corbis.Email.Contracts.V1.RequestPriceDetail emailDetail = new Corbis.Email.Contracts.V1.RequestPriceDetail();
            if (requestPriceView.LightboxId > 0)
            {
                LightboxDetail lightboxDetail = lightboxCartService.GetLightboxDetail(requestPriceView.LightboxId);
                emailDetail.LightboxName = lightboxDetail.LightboxName;
            }

            emailDetail.Container = requestPriceView.ContainerName;
            emailDetail.CountryCode = requestPriceView.CountryCode;
            emailDetail.RegionCode = requestPriceView.StateCode;

            emailDetail.FirstName = requestPriceView.FirstName;
            emailDetail.LastName = requestPriceView.LastName;
            emailDetail.Email = requestPriceView.Email;
            emailDetail.Phone = requestPriceView.Phone;
            emailDetail.Comments = requestPriceView.Comments;

            emailDetail.ImageDetails = requestPriceView.LightboxItems.ConvertAll<Corbis.Email.Contracts.V1.ImageDetail>(
                new Converter<LightboxDisplayImage, Corbis.Email.Contracts.V1.ImageDetail>(
                    delegate(LightboxDisplayImage displayImage)
                    {
                        Corbis.Email.Contracts.V1.ImageDetail imageDetail = new Corbis.Email.Contracts.V1.ImageDetail();
                        imageDetail.CorbisId = displayImage.CorbisId;
                        imageDetail.LicenseModelText = requestPriceView.GetLicenceModelLocalizedText(displayImage.LicenseModel);
                        imageDetail.Title = displayImage.Title;
                        return imageDetail;
                    }));

            imageService.SendRequestPriceEmail(Profile.UserName, emailDetail, Language.CurrentCulture.Name);
        }

        public void GetAllLightboxItems()
        {
            int itemCount = lightboxCartService.GetLightboxProductCount(requestPriceView.LightboxId);

            requestPriceView.LightboxItems = lightboxCartService.GetLightboxProductDetails(
                requestPriceView.LightboxId,
                Language.CurrentCulture.Name,
                Profile.CountryCode,
                1,
                itemCount,
                false,
                Profile.UserName);
        }

        public void GetImageThumbnailDetail()
        {
            List<LightboxDisplayImage> mediaList = new List<LightboxDisplayImage>();
            // Need to handle the case for Anonymous users
            string cultureName = String.IsNullOrEmpty(Profile.CultureName)
                ? Language.CurrentLanguage.LanguageCode
                : Profile.CultureName;
            DisplayImage image = imageService.GetDisplayImage(requestPriceView.CorbisId, cultureName, Profile.IsAnonymous);
            if (image != null)
            {
                LightboxDisplayImage lightboxDisplayImage = new LightboxDisplayImage();
                lightboxDisplayImage.AspectRatio = image.AspectRatio;
                lightboxDisplayImage.Category = image.Category;
                lightboxDisplayImage.CorbisId = image.CorbisId;
                lightboxDisplayImage.IsImageAvailable = image.IsAvailable;
                lightboxDisplayImage.IsOutLine = image.IsOutline;
                lightboxDisplayImage.LicenseModel = image.LicenseModel;
                lightboxDisplayImage.MediaUid = image.MediaUid;
                lightboxDisplayImage.Title = image.Title;
                lightboxDisplayImage.Url128 = image.Url128;

                mediaList.Add(lightboxDisplayImage);
            }
            else
            {
                RFCDEntity entity = rfcdService.GetRFCDDetails(requestPriceView.CorbisId, Profile.CountryCode, Language.CurrentCulture.Name);
                if( entity != null)
                {
                    LightboxDisplayImage lightboxDisplayImage = new LightboxDisplayImage();
                    lightboxDisplayImage.AspectRatio = 1;
                    lightboxDisplayImage.CorbisId = entity.VolumeNumber.ToString();
                    lightboxDisplayImage.LicenseModel = Corbis.CommonSchema.Contracts.V1.LicenseModel.RF;
                    lightboxDisplayImage.Title = entity.Title;
                    lightboxDisplayImage.Url128 = entity.Url128;

                    mediaList.Add(lightboxDisplayImage);
               }
            }

            requestPriceView.LightboxItems = mediaList;
        }

        public void PopulateRequestPriceForm()
        {


            if (!Profile.IsAnonymous)
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    requestPriceView.FirstNameAsian = Profile.FirstName;
                    requestPriceView.LastNameAsian = Profile.LastName;
                    requestPriceView.NameAsianVisibility = true;
                    requestPriceView.NameDefaultVisibility = false;
                }
                else
                {
                    requestPriceView.FirstName = Profile.FirstName;
                    requestPriceView.LastName = Profile.LastName;
                    requestPriceView.NameAsianVisibility = false;
                    requestPriceView.NameDefaultVisibility = true;
                }
                requestPriceView.Email = Profile.Email;
                requestPriceView.Phone = Profile.BusinessPhoneNumber;
                requestPriceView.CountryCode = Profile.CountryCode;
                if (Profile.AddressDetail != null)
                {
                    requestPriceView.StateCode = Profile.AddressDetail.RegionCode;
                }
            }
            // Need to handle the case for Anonymous users
            else
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    requestPriceView.NameAsianVisibility = true;
                    requestPriceView.NameDefaultVisibility = false;
                }
                else
                {
                    requestPriceView.NameAsianVisibility = false;
                    requestPriceView.NameDefaultVisibility = true;
                }
            }           
        }
    }
}
