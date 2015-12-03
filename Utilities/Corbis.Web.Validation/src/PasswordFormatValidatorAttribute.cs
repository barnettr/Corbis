using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Password Validator attributes
    /// </summary>
    public class PasswordFormatValidatorAttribute : ValueValidatorAttribute
    {

        public PasswordFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new PasswordFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
