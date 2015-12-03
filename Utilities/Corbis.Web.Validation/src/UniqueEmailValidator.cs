using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Corbis.Framework;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;

namespace Corbis.Web.Validation
{
    /// <summary>
    /// Validates unique or non unique email information based on the negated flag
    /// </summary>
    public class UniqueEmailValidator : ValueValidator
    {
        private MembershipServiceAgent _agent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="resourceType"></param>
        /// <param name="negated">false: checks uniqueness, true: vice versa</param>
        public UniqueEmailValidator(string messageTemplate, Type resourceType, bool negated)
            : base(messageTemplate, null, negated)
        {
            base.MessageTemplate = messageTemplate;
            _agent = new MembershipServiceAgent();
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
                RegexValidator emailValidator = new RegexValidator(RegularExpressions.EmailFormat);
                ValidationResults results = emailValidator.Validate(email);

                // validate only if email is valid, otherwise use algorithmic validator on your form
                if (results.IsValid)
                {
                    Member member = _agent.GetMemberByUsername(email);

                    // user already exists or user does not exist, and it should depend on the negated flag
                    if (((member != null && member.Email != null && member.Email.ToLower().Equals(email.ToLower()) && Negated == false) ||
                            ((member == null || member.Email == null || !member.Email.ToLower().Equals(email.ToLower()))) && Negated == true))
                    {
                        LogValidationResult(validationResults, MessageTemplate, currentTarget, key);
                    }
                }
            }
        }
    }
}
