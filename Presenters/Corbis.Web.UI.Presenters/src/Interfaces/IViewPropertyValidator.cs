using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    /// <summary>
    /// Classes that implement this interface can map the Property on the view to
    /// the control that Property gets or sets using the
    /// <see cref="Corbis.Web.Validation.PropertyControlMapperAttribute"/>
    /// </summary>
    public interface IViewPropertyValidator
    {

        /// <summary>
        /// Sets the validation error.
        /// </summary>
        /// <typeparam name="T">An Enum the view can use to determine the correct error message</typeparam>
        /// <param name="invalidControlName">Name of the invalid control, derived using the
        /// <see cref="Corbis.Web.Validation.PropertyControlMapperAttribute"/>
        /// attribute of the property on the view.</param>
        /// <param name="errorEnumValue">The enum value the view should use to generate the error message</param>
        /// <param name="showInSummary">if set to <c>true</c> [show in summary].</param>
        /// <param name="showHilite">if set to <c>true</c> [show hilite].</param>
        void SetValidationError<T>(
            string invalidControlName, 
            T errorEnumValue, 
            bool showInSummary, 
            bool showHilite);

    }
}
