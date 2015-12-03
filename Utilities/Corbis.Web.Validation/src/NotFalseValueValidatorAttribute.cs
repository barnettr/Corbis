using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Empty value Validator attributes
    /// </summary>
    public class NotFalseValueValidatorAttribute : ValueValidatorAttribute
    {

        public NotFalseValueValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new NotFalseValueValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
