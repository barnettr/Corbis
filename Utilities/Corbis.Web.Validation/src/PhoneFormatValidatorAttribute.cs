using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Phone Validator attributes
    /// </summary>
    public class PhoneFormatValidatorAttribute : ValueValidatorAttribute
    {

        public PhoneFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new PhoneFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
