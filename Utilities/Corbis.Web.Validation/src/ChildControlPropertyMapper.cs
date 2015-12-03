using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Provides the mapping from a property on a view to the child control
    /// which contains the actual invalid Control.
    /// </summary>
    public class ChildControlPropertyMapper : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildControlPropertyMapper"/> class.
        /// </summary>
        /// <param name="childPropertyName">
        /// Name of the Property that maps to the Child control the Invalid control lives on.
        /// </param>
        /// <param name="mappedControlName">
        /// Name of the Property on the Child control responsible for mapping to the
        /// Invalid control.
        /// </param>
        public ChildControlPropertyMapper(string childPropertyName, string mappedControlName)
        {
            ChildPropertyName = childPropertyName;
            MappedControlName = mappedControlName;
        }

        /// <summary>
        /// Name of the Property that will return the 
        /// child control on which the invalid control lives.
        /// The child control must implement <see cref="IViewPropertyValidator"/>
        /// </summary>
        /// <value>The name of the child property.</value>
        public String ChildPropertyName { get; set; }

        /// <summary>
        /// The name of the Property on the Child 
        /// control responsible for mapping to the Invalid control
        /// </summary>
        public String MappedControlName { get; set; }

    }
}
