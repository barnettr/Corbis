using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Corbis.Framework;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Validates that a value was selected from a dropdown.
    /// </summary>
    public class ValueNotSelectedValidator : ValueValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="resourceType"></param>
        /// <param name="negated">false: checks uniqueness, true: vice versa</param>
        public ValueNotSelectedValidator(string messageTemplate, Type resourceType, bool negated)
            : base(messageTemplate, null, negated)
        {
            base.MessageTemplate = messageTemplate;
        }

        /// <summary>
        /// Default
        /// </summary>
        protected override string DefaultNegatedMessageTemplate
        {
            get { return MessageTemplate; }
        }

        /// <summary>
        /// Default
        /// </summary>
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return MessageTemplate; }
        }

        /// <summary>
        /// Validation logic
        /// </summary>
        /// <param name="objectToValidate"></param>
        /// <param name="currentTarget"></param>
        /// <param name="key"></param>
        /// <param name="validationResults"></param>
        protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            string formValue = (objectToValidate != null) ? objectToValidate.ToString() : "";

            RegexValidator regexValidator = new RegexValidator(RegularExpressions.ValueNotSelected);
            ValidationResults results = regexValidator.Validate(formValue);
            
            // If not valid, otherwise use algorithmic validator on your form
            if (results.IsValid == false)
            {
                LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
            }
        }
    }
}
