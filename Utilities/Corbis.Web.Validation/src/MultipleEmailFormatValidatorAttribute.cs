using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    public class MultipleEmailFormatValidatorAttribute : ValueValidatorAttribute
    {
        public MultipleEmailFormatValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new MultipleEmailFormatValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
