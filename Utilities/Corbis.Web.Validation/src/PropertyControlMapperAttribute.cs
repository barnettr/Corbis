using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Provides the mapping from a property on a view to the control
    /// to which it is mapped to.
    /// </summary>
    public class PropertyControlMapperAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Corbis.Web.Validation.PropertyControlMapperAttribute"/> 
        /// class.
        /// </summary>
        /// <param name="mappedControlName">
        /// Name of the control to which the property is mapped to.
        /// </param>
        public PropertyControlMapperAttribute(string mappedControlName)
        {
            MappedControlName = mappedControlName;
        }
        public String MappedControlName { get; set; }
    }
}
