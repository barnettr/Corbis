using System;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.DisplayText.Contracts.V1;

namespace Corbis.Web.Content
{
    public class DisplayServiceProvider : ContentProvider
    {
        IDisplayTextContract displayService;

        /// <summary>
        /// Initializes a new instance of the DisplayServiceProvider class.
        /// </summary>
        public DisplayServiceProvider()
        {
            displayService = new DisplayTextServiceAgent();
        }

        public void SetDisplayTextServiceBehavior(IDisplayTextContract service)
        {
            displayService = service;
        }

        public IDisplayTextContract DisplayService
        {
            get
            {
                return displayService;
            }
        }
    }
}
