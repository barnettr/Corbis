using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Empty value Validator attributes
    /// </summary>
    public class EmptyValueValidatorAttribute : ValueValidatorAttribute
    {

        public EmptyValueValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new EmptyValueValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
