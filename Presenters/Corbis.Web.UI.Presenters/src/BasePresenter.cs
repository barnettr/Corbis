using System;
using System.Collections.Generic;
using Corbis.Common.FaultContracts;
using Corbis.Framework.Logging;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Authentication;
using Corbis.Web.Utilities;
using Corbis.Web.Validation;
using Corbis.DisplayText.ServiceAgents.V1;

namespace Corbis.Web.UI.Presenters
{
    /// <summary>
    /// Represents the base class for all presenters
    /// </summary>
    public class BasePresenter
    {
        #region Member Variables
        DisplayTextServiceAgent displayAgent;
        private System.Web.HttpContextBase _httpContext;
                
        #endregion

        public virtual Profile Profile
        {
            get { return Profile.Current; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BasePresenter()
        {
            _httpContext = null;
        }

        #region Handling Exception
 
        public CorbisFault HandleException(Exception exception, ILogging loggingContext, string subject)
        {
            if (loggingContext == null)
            {
                loggingContext = new LoggingContext(new List<string>());
            }

            CorbisFault corbisFault = ServiceHelper.HandleServiceException(exception);
            if (corbisFault != null)
            {
                loggingContext.LogErrorMessage(subject, corbisFault.ClientMessage, exception);
            }
            else
            {
                loggingContext.LogErrorMessage(subject, exception);
            }
            return corbisFault;
        }

        /// <summary>
        /// Is the country Ecommerce enabled call to service.
        /// Returns false on any error
        /// </summary>
        /// <param name="countryCode">this would be any value non blank</param>
        /// <returns>true if country is enabled, false if not</returns>
        public bool IsCountryECommerceEnabled(string countryCode)
        {

            if(String.IsNullOrEmpty(countryCode))
                return false;

            if(displayAgent == null)
            {
                displayAgent = new DisplayTextServiceAgent();
            }

            try
            {
                return displayAgent.IsCountryEcommerceEnabled(countryCode);
            }
            catch 
            {
                return false;
            }

        }

        #endregion

        internal void SetDisplayTextServiceAgent(DisplayTextServiceAgent agent)
        {
            displayAgent = agent;
        }

        protected System.Web.HttpContextBase HttpContext
        {
            get
            {
                if (_httpContext == null && System.Web.HttpContext.Current != null)
                {
                    _httpContext = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
                }
                return _httpContext;
            }
            set
            {
                _httpContext = value;
            }
        }

        protected void SetValidationErrorsOnView<T>(
            IViewPropertyValidator view, 
            List<ViewValidationError<T>> validationErrors)
        {
            if (view == null) { return; }

            foreach (ViewValidationError<T> error in validationErrors)
            {
                System.Reflection.PropertyInfo viewProperty = view.GetType().GetProperty(error.InvalidPropertyName);
                if (viewProperty != null)
                {
                    // Check if it's on a child control first
                    object[] validationAtts = viewProperty.GetCustomAttributes(typeof(ChildControlPropertyMapper), true);
                    if (validationAtts != null && validationAtts.Length > 0 && validationAtts[0] != null)
                    {
                        string childPropertyName = ((ChildControlPropertyMapper)validationAtts[0]).ChildPropertyName;
                        System.Reflection.PropertyInfo childControlProperty =
                            view.GetType().GetProperty(childPropertyName);
                        if (childControlProperty != null)
                        {
                            // Recursively set the errors
                            IViewPropertyValidator childPropertyValidator = childControlProperty.GetValue(view, null) as IViewPropertyValidator;
                            if (childPropertyValidator != null)
                            {
                                string childControlPropertyMapper = ((ChildControlPropertyMapper)validationAtts[0]).MappedControlName;
                                List<ViewValidationError<T>> recursiveErrorList = new List<ViewValidationError<T>>(1);
                                recursiveErrorList.Add(new ViewValidationError<T>(
                                    childControlPropertyMapper,
                                    error.InvalidReason,
                                    error.HighlightRow,
                                    error.ShowInSummary));
                                SetValidationErrorsOnView<T>(childPropertyValidator, recursiveErrorList);
                            }
                        }
                    }
                    else
                    {
                        // The invalid control lives on this view
                        validationAtts = viewProperty.GetCustomAttributes(typeof(PropertyControlMapperAttribute), true);
                        if (validationAtts != null && validationAtts.Length > 0 && validationAtts[0] != null)
                        {
                            string mappedControlName = ((PropertyControlMapperAttribute)validationAtts[0]).MappedControlName;
                            view.SetValidationError<T>(mappedControlName, error.InvalidReason, error.ShowInSummary, error.HighlightRow);
                        }
                    }
                }
            }
        }
	}

}

