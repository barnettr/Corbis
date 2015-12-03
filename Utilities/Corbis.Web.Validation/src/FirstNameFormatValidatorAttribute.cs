using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// FirstName Validator attributes
    /// </summary>
    public class FirstNameFormatValidatorAttribute : ValueValidatorAttribute
    {

        public FirstNameFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new FirstNameFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
