using System;
using System.Collections.Generic;
using System.ServiceModel;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Presenters.Rfcd.ViewInterfaces;
using Corbis.RFCD.Contracts.V1;
using Corbis.RFCD.ServiceAgents.V1;
using Corbis.Common.ServiceFactory;
using Corbis.Common.FaultContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.Globalization;
using Corbis.Web.Authentication;
using System.Xml;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;


namespace Corbis.Web.UI.Presenters.Rfcd
{
    public class RfcdPresenter : BasePresenter
    {
        IView view;
        IRfcdCategoryView rfcdCategoryView;
        IRFCDResultsView rfcdResultsView;
        IRFCDContract serviceAgent;

        public RfcdPresenter(IView view) : this(view, new RFCDServiceAgent())
        {

        }

        public RfcdPresenter(IView view, IRFCDContract serviceAgent)    
        {
            if (view == null)
            {
                throw new ArgumentNullException("RfcdPresenter: RfcdPresenter() - View Object cannot be null.");
            }
            if (serviceAgent == null)
            {
                throw new ArgumentNullException("RfcdPresenter: RfcdPresenter() - RfCd Service agent cannot be null.");
            }

            this.view = view;
            this.serviceAgent = serviceAgent;

            rfcdCategoryView = view as IRfcdCategoryView;
            rfcdResultsView = view as IRFCDResultsView;
        }

        public bool IsLanguageChanged
        {
            get
            {
                if (rfcdCategoryView.PreviousLanguageCode != Language.CurrentLanguage.LanguageCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ReloadCategoriesOnLanguageChange()
        {
            if (IsLanguageChanged)
            {
                GetAllRFCDCategories();
            }
        }

        public void GetAllRFCDCategories()
        {
            try
            {
                RFCDCategory category = serviceAgent.GetAllRFCDCategories(Language.CurrentLanguage.LanguageCode);
                if (category != null)
                {
                    string catString = "<?xml version=\"1.0\"?>" +
                    "<categories>" +
                        BuildXmlString(category).ToString() +
                    "</categories>";
                    rfcdCategoryView.RFCDCategories = catString;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext, "RfCdCategoryPresenter: GetAllRFCDCategories()");
                throw;
            }
        }

        public void SetPreviousLanguageCode()
        {
            rfcdCategoryView.PreviousLanguageCode = Language.CurrentLanguage.LanguageCode;
        }

        public void GetRFCDByCategory()
        {
            try
            {
                rfcdCategoryView.RFCDsByFirstLetterOrCategory = serviceAgent.GetRFCDByCategory(rfcdCategoryView.CategoryUID, Profile.CountryCode);
            }
            catch (Exception ex)
            {
                HandleException(ex, null, "RfCdCategoryPresenter: GetRFCDByCategory()");
                throw;
            }
        }

        public void GetRFCDByFirstLetter()
        {
            try
            {
                rfcdCategoryView.RFCDsByFirstLetterOrCategory = serviceAgent.GetRFCDByFirstLetter(Convert.ToChar(rfcdCategoryView.CategoryTitle), Profile.CountryCode);
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext, "RfCdCategoryPresenter: GetRFCDByFirstLetter()");
                throw;
            }
        }

        private string BuildXmlString(RFCDCategory category)
        {
            String xmlString = string.Empty;
            String tagText = string.Empty;
            String tagString = "category";

            if (category != null)
            {
                tagText = category.DisplayText;
                if (!String.IsNullOrEmpty(tagText))
                {
                    tagText = HttpContext.Server.HtmlEncode(tagText);
                }

                xmlString = GetChildrenXml(category, xmlString, tagText, tagString);
            }

            return xmlString;
        }

        private String GetChildrenXml(RFCDCategory category, String xmlString, String tagText, String tagString)
        {
            if (category.ChildrenCategories != null && category.ChildrenCategories.Count > 0)
            {
                if (!String.IsNullOrEmpty(tagText))
                {
                    xmlString = "<" + tagString + " name=\"" + tagText + "\" value=\"" + category.CategoryUid.ToString() + "\">";
                }
                foreach (RFCDCategory childCategory in category.ChildrenCategories)
                {
                    xmlString = xmlString + BuildXmlString(childCategory).ToString();
                }
                if (!String.IsNullOrEmpty(tagText))
                {
                    xmlString = xmlString + "</" + tagString + ">";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(tagText))
                {
                    xmlString = "<" + tagString + " name=\"" + tagText + "\" value=\"" + category.CategoryUid.ToString() + "\">" + "</" + tagString + ">";
                }
            }
            return xmlString;
        }

        ///<summary>
        ///Displays the RFCD information on the RFCD results page
        ///</summary>
        public void DisplyRFCDResults()
        {
            string countryCode = Profile.IsChinaUser ? "CN" : Profile.CountryCode;
            RFCDEntity rfcdEntity = serviceAgent.GetRFCDDetailsWithPagination(
                rfcdResultsView.VolumeNumber, countryCode, 
                Language.CurrentLanguage.LanguageCode, 50, 1, Profile.UserName);
            
            if (rfcdResultsView == null)
            {
                throw new Exception(string.Format("Invalid View. Expected IRFCDResultsView; Actual {0}", this.view.GetType().Name));
            }

            rfcdResultsView.CdName = rfcdEntity.Title;
            rfcdResultsView.NumberOfImages = rfcdEntity.ImageCount.ToString();
            rfcdResultsView.RfcdID = rfcdEntity.VolumeNumber;
            rfcdResultsView.RfcdImage = rfcdEntity.Url256;
            if (rfcdEntity.BasePrice != null)
            {
                rfcdResultsView.ImagePrice = rfcdEntity.BasePrice + rfcdEntity.BasePriceUnit;
            }
            rfcdResultsView.ImageSize = rfcdEntity.FileSize;
            rfcdResultsView.RfcdID2 = rfcdEntity.VolumeNumber;
            rfcdResultsView.NumberOfImages2 = rfcdEntity.ImageCount.ToString();
            rfcdResultsView.Copyright = rfcdEntity.CopyrightDate + "/" + rfcdEntity.ContentProvider;
            rfcdResultsView.InterestedRepeater = rfcdEntity.RelatedRfcds;
            rfcdResultsView.WebProductList = rfcdEntity.MediaItems;
            rfcdResultsView.VolumeGuid = rfcdEntity.RFCDUid;
        }

        ///<summary>
        ///Add the CD to Cart
        ///</summary>
        public void AddToCart(Guid memberUid)
        {
            //TODO:  Connect to cart once service method is implemented.
            
            //LightboxCartServiceAgent lightboxCartServiceAgent = new LightboxCartServiceAgent();
            //lightboxCartServiceAgent.AddOfferingToCart(memberUid, rfcdResultsView.VolumeGuid, Profile.Current.CultureName, Profile.Current.CountryCode);
        }
    }
}
