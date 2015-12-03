using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Enlargement.ViewInterfaces
{
    public interface IEnlargementView : IView
	{
		#region properties used by Image Details

		List<string> ImageList { get; set;}
		int ImageListPageNo { get; set;}
		int ImageListPageSize { get; }
		string ImageListQuery { get; }
		Guid MediaUid { get; set;}
		int CurrentImageIndex { get; set;}
		int TotalImageCount { get; set;}
        bool IsOutline { get; set;}
        PriceTier PriceTier { set;}
        string Collection { set;}
        Corbis.CommonSchema.Contracts.V1.Category Category { get; set;}
		Corbis.CommonSchema.Contracts.V1.LicenseModel LicenseModel { get; set; }
		bool IsInCart { get; set;}
		bool IsInLightbox { get; set;}
		bool IsInQuickPick { get; set;}
		void SetIconToolset(object datasource, string cssClassField, string valueField);
		bool HasRelatedImages { get; set;}
        bool HasCorbisKeywords { get; set;}
		QuickPicFlags QuickPicFlags { get; set;}
		string Caller { get; }
		int LightboxId { get; }
        string FineArtRestrictedImageURL { get; }
        

		#endregion

		string CorbisId { set; get; }
		string CreditLine { set; }
        string ImageTitle { set; }
        string ImageCaption { set; }
        string FineArtCreditLine { set; }
        string Photographer { set; }
        string PhotoDate { set; }
        string ContentCreator { set; }
        string CreateDate { set; }
		string Magazine { set; }
		string PublishDate { set; }
        string Location { set; }
        string EnlargementImageUrl { set; }
		string Price { set; get;}
        string PriceStatus { set;}
        List<ContentWarning> ContentWarnings { set;}
        Guid ProductUid { set; get;}
        List<ImageDimension> DimensionList { set; }
        List<Keyword> Keywords { set; }
		string ImageUrl { set; }
		decimal AspectRatio { set; }
		string PhotoDateKW { set; }
        string CreateDateKW { set; }
		string ImageCaptionKW { set; }
		IImageRestrictionsView ImageRestrictionView { get; }
		int? CurrentLightboxId { get; }
		bool IsRfcd { set; get; }

        //feedback form
        string TitleFeedback { set; get; }
        string ContactPhone { set; }
        string UserName { set; }
        string Name { set; get; }
        string Email { set; get; }
        string PhoneNumber { set; get; }
        string Role { get; }
        string LicenseModelText { set; get;}
        string Comments { set; get; }
        string BrowserName { get; }
        string BrowserVersion { get; }
        string Platform { get; }
        string IssueTypeText { get; }
        Corbis.CommonSchema.Contracts.V1.Image.ImageFeedbackIssueType IssueType { get; }
        void SetFeedbackText();
        void ShowEmailError();
    }

}
