using System;
using System.Collections.Generic;
using System.Globalization;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Framework.Globalization;
using Corbis.Web.Authentication;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.Web.Content;

namespace Corbis.Web.UI.Presenters
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
