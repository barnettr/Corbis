using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IAttributeValidator
    {
        /// <summary>
        /// Gets or sets the error text resource key to use.
        /// </summary>
        /// <value>The error text resource.</value>
        string ErrorTextResourceKey { get; set;}
        
        /// <summary>
        /// Gets or sets the error summary text resource key to use.
        /// </summary>
        /// <value>The error summary text resource.</value>
        string ErrorSummaryTextResourceKey { get; set;}
        
        /// <summary>
        /// Sets the display style.
        /// </summary>
        /// <value>The display style.</value>
        AttributeValidatorDisplayStyle DisplayStyle { set; }
    }

    public enum AttributeValidatorDisplayStyle
    {
        None,
        Static,
        Dynamic
    }


}
