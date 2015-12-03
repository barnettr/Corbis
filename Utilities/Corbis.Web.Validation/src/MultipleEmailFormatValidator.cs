using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Corbis.Framework;

namespace Corbis.Web.Validation
{
    public class MultipleEmailFormatValidator : ValueValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="resourceType"></param>
        /// <param name="negated">false: checks uniqueness, true: vice versa</param>
        public MultipleEmailFormatValidator(string messageTemplate, Type resourceType, bool negated)
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
            string email = (objectToValidate != null) ? objectToValidate.ToString() : "";

            // validate only if non null as another validator is checking that condition
            if (email.Length > 0)
            {
                string[] emails = SplitMultipleEmails(email);
                for (int i = 0; i < emails.Length; i++)
                {
                    RegexValidator regexValidator = new RegexValidator(RegularExpressions.EmailFormat);
                    ValidationResults results = regexValidator.Validate(emails[i]);

                    // If not valid, otherwise use algorithmic validator on your form
                    if (results.IsValid == false)
                    {
                        LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
                        break;
                    }
                }
            }
        }

        private string[] SplitMultipleEmails(string emailsDelimited)
        {
            // Delimiters are either a comma or semicolon
            string[] delimiter = new string[2] { ",", ";" };
            return emailsDelimited.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
