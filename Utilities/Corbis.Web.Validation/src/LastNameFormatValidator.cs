using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Framework;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Validates LastName format.
    /// </summary>
    public class LastNameFormatValidator : ValueValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="resourceType"></param>
        /// <param name="negated">false: checks uniqueness, true: vice versa</param>
        public LastNameFormatValidator(string messageTemplate, Type resourceType, bool negated)
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
            string lastName = (objectToValidate != null) ? objectToValidate.ToString() : "";

            // validate only if non null as another validator is checking that condition
            if (lastName.Length > 0)
            {   
                RegexValidator regexValidator = new RegexValidator(RegularExpressions.LastNameFormat);
                ValidationResults results = regexValidator.Validate(lastName);
                
                // If not valid, otherwise use algorithmic validator on your form
                if (results.IsValid == false)
                {
                    LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
                }
            }
        }
    }
}
