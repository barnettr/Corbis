using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Empty value Validator attributes
    /// </summary>
    public class NotZeroValueValidatorAttribute : ValueValidatorAttribute
    {

        public NotZeroValueValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new NotZeroValueValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
