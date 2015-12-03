using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Value not selected Validator attributes
    /// </summary>
    public class ValueNotSelectedValidatorAttribute : ValueValidatorAttribute
    {

        public ValueNotSelectedValidatorAttribute() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new ValueNotSelectedValidator(MessageTemplateResourceName, MessageTemplateResourceType, Negated);
        }

    }
}
