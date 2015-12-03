using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.DisplayText.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Web.UI.Presenters.Tools.Interfaces;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.Presenters.Tools
{
    public class DisplayTextPresenter : BasePresenter
    {
        private IDisplayTextView view;
        private IDisplayTextContract agent;

        public DisplayTextPresenter(IDisplayTextView view)
            : this(view, new DisplayTextServiceAgent())
        {
        }

        public DisplayTextPresenter(IDisplayTextView view, IDisplayTextContract agent)
        {
            if (view == null)
            {
                throw new ArgumentNullException("DisplayTextPresenter: DisplayTextPresenter() - Display Text View cannot be null.");
            }
            if (agent == null)
            {
                throw new ArgumentNullException("DisplayTextPresenter: DisplayTextPresenter() - Dispaly Text service agent cannot be null.");
            }
            this.view = view;
            this.agent = agent;
        }

        public void DisplayCountries()
        {
            this.view.DisplayCountries = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.Country", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayMediaTypes()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add(MediaType.Illustration.ToString(), "(1) " + MediaType.Illustration.ToString());
            dictionary.Add(MediaType.Photography.ToString(), "(2) " + MediaType.Photography.ToString());
            this.view.DisplayMediaTypes = dictionary;
        }

        public void DisplayColorFormats()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(ColorFormat.Color.ToString(), "(1) " + ColorFormat.Color.ToString());
            dictionary.Add(ColorFormat.BlackAndWhite.ToString(), "(2) " + ColorFormat.BlackAndWhite.ToString());
            this.view.DisplayColorFormats = dictionary;
        }

        public void DisplayOrientation()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "(1) Horizontal Panoramic");
            dictionary.Add("2", "(2) Horizontal");
            dictionary.Add("3", "(3) Square");
            dictionary.Add("4", "(4) Vertical");
            dictionary.Add("5", "(5) Vertical Panoramic");
            this.view.DisplayOrientation = dictionary;
        }

        public void DisplayExternalAddedDate() 
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "Any Date");
            dictionary.Add("2", "ON");
            dictionary.Add("3", "BETWEEN");
            dictionary.Add("4", "BEFORE");
            dictionary.Add("5", "AFTER");
            this.view.DisplayExternalAddedDate = dictionary;
        }

        public void DisplayMagazinePublishDate()
        { 
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "Any Date");
            dictionary.Add("2", "ON");
            dictionary.Add("3", "BETWEEN");
            dictionary.Add("4", "BEFORE");
            dictionary.Add("5", "AFTER");
            this.view.DisplayMagazinePublishDate = dictionary;
        }

        public void DisplayInternalAddedDate() {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "Any Date");
            dictionary.Add("2", "ON");
            dictionary.Add("3", "BETWEEN");
            dictionary.Add("4", "BEFORE");
            dictionary.Add("5", "AFTER");
            this.view.DisplayInternalAddedDate = dictionary;
        }

        public void DisplayDatePhotographed() {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "Any Date");
            dictionary.Add("2", "ON");
            dictionary.Add("3", "BETWEEN");
            dictionary.Add("4", "BEFORE");
            dictionary.Add("5", "AFTER");
            this.view.DisplayDatePhotographed = dictionary;
        }

        public void DisplayMediaRatings()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "(1) SS");
            dictionary.Add("2", "(2) S");
            dictionary.Add("3", "(3) B");
            dictionary.Add("4", "(4) C");
            dictionary.Add("5", "(5) D");
            this.view.DisplayMediaRatings = dictionary;
        }

        public void DisplayCategories()
        {
            this.view.DisplayCategories = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.Category", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplaySearchSortOrder()
        {
            this.view.DisplaySearchSortOrder = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.SearchSortOrder", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayMarketingCollections()
        {
            this.view.DisplayMarketingCollections = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.MarketingCollection", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayNumberOfPeople()
        {
            this.view.DisplayNumberOfPeople = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.NumberOfPeople", Language.CurrentLanguage.LanguageCode);
        }
        
        public void DisplayPointOfViews() 
        {
            this.view.DisplayPointOfViews = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.PointOfView", Language.CurrentLanguage.LanguageCode);
        }
        
        public void DisplayRMImageSize()
        {
            this.view.DisplayRMImageSize = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.FileSize", Language.CurrentLanguage.LanguageCode);
        }
        
        public void DisplayContentTypes() {
            this.view.DisplayContentTypes = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.ContentType", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayPrimarySubject() {
            this.view.DisplayPrimarySubject = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.PrimarySubject", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayArchiveCollections()
        {
            this.view.DisplayArchiveCollections = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.ArchiveCollection", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayRightOfFirstSale()
        {
            this.view.DisplayRightOfFirstSale = agent.GetAllEntitiesByType("Corbis.DisplayText.Contracts.V1.CCDRestrictionsRightOfFirstSale", Language.CurrentLanguage.LanguageCode);
        }

        public void DisplayDetails(string entityType)
        {
            try
            {
                Dictionary<int, DisplayTextEntity> collection = agent.GetAllEntitiesByType(entityType, Language.CurrentLanguage.LanguageCode);
                UpdateViewFrom(collection);
            }
            catch (Exception ex)
            {
                HandleException(ex,null,"DisplayTextPresenter: DisplayDetails() - Unable to get display text.");
            }
        }

        private void UpdateViewFrom(Dictionary<int, DisplayTextEntity> collection)
        {
            view.DisplayTextEntities = collection;
        }


    }
}
