using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Email Validator attributes
    /// </summary>
    public class EmailFormatValidatorAttribute : ValueValidatorAttribute
    {

        public EmailFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new EmailFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
