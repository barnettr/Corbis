using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Email validator atributes
    /// </summary>
    public class UniqueEmailValidatorAttribute : ValueValidatorAttribute
    {

        public UniqueEmailValidatorAttribute() : base() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new UniqueEmailValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
