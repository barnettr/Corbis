using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// LastName Validator attributes
    /// </summary>
    public class LastNameFormatValidatorAttribute : ValueValidatorAttribute
    {

        public LastNameFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new LastNameFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
