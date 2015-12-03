using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using Corbis.Web.Authentication;
using Corbis.Web.UI.ViewInterfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Framework.Logging;

namespace Corbis.Web.UI
{
    public class CorbisBaseUserControl : UserControl, IView
    {
        ILogging log;

        public Profile Profile
        {
            get { return Profile.Current; }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CorbisBaseUserControl class.
        /// </summary>
        public CorbisBaseUserControl()
        {
        }

        #endregion


        #region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {

                if (log == null)
                {
                    if (this.Page is CorbisBasePage)
                    {
                        log = ((CorbisBasePage)Page).LoggingContext;
                    }
                    else
                    {
                        log = new LoggingContext(new List<string>());
                    }
                }

                return log;
            }
            set
            {
                log = value;
            }
        }


        /// <summary>
        /// Sets the validation error.
        /// </summary>
        /// <typeparam name="T">An Enum the view can use to determine the correct error message</typeparam>
        /// <param name="invalidControlName">Name of the invalid control, derived using the
        /// <see cref="Corbis.Web.Validation.PropertyControlMapperAttribute"/>
        /// attribute of the property on the view.</param>
        /// <param name="errorEnumValue">The enum value the view should use to generate the error message</param>
        public void SetValidationError<T>(string invalidControlName, T errorEnumValue)
        {
            SetValidationError<T>(invalidControlName, errorEnumValue, true, true);
        }

        /// <summary>
        /// Sets the validation error.
        /// </summary>
        /// <typeparam name="T">An Enum the view can use to determine the correct error message</typeparam>
        /// <param name="invalidControlName">Name of the invalid control, derived using the
        /// <see cref="Corbis.Web.Validation.PropertyControlMapperAttribute"/>
        /// attribute of the property on the view.</param>
        /// <param name="errorEnumValue">The enum value the view should use to generate the error message</param>
        /// <param name="showInSummary">if set to <c>true</c> [show in summary].</param>
        public void SetValidationError<T>(string invalidControlName, T errorEnumValue, bool showInSummary)
        {
            SetValidationError<T>(invalidControlName, errorEnumValue, showInSummary, true);
        }

        /// <summary>
        /// Sets the validation error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="invalidControlName">Name of the invalid control.</param>
        /// <param name="errorEnumValue">The error enum value.</param>
        /// <param name="showInSummary">if set to <c>true</c> [show in summary].</param>
        /// <param name="showHilite">if set to <c>true</c> [show hilite].</param>
        public virtual void SetValidationError<T>(
            string invalidControlName,
            T errorEnumValue,
            bool showInSummary,
            bool showHilite)
        {
            Control invalidControl = null;
            System.Reflection.FieldInfo invalidControlFieldInfo = 
                this.GetType().GetField(
                invalidControlName,
                System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public);
            if (invalidControlFieldInfo != null)
            {
                invalidControl = invalidControlFieldInfo.GetValue(this) as Control;
            }
            if (invalidControl != null)
            {
                CorbisBasePage parent = this.Page as CorbisBasePage;
                if (parent != null)
                {
                    parent.SetValidationHubError<T>(
                        invalidControl,
                        errorEnumValue,
                        showInSummary,
                        showHilite);
                }
            }
        }


		public IList<ValidationDetail> ValidationErrors
		{
			get { return ((CorbisBasePage)Page).ValidationErrors; }
			set { ((CorbisBasePage)Page).ValidationErrors = value; }
		}


		#endregion
    }
}
