using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.Controls
{
    public class VisaCardNumberValidation : CardNumberValidationBase
    {
        string[] beginningNumbersTable = new string[] { "4" };
        
        
        static int[] validLengthsOrderedByMaxLength = new int[] { 16, 13 };

        public VisaCardNumberValidation(string cardNumber) : base(cardNumber){}
        
        public new static int MaxLength
        {
            get
            {
                return validLengthsOrderedByMaxLength[0];
            }
        }

        public override bool IsValid()
        {
            if (!CardNumberLengthIsValid(validLengthsOrderedByMaxLength))
                return false;

            if (!BeginningNumbersMatch())
                return false;

            return ValidateCardNumber();

        }

        public override bool CardNumberLengthIsValid()
        {
            return CardNumberLengthIsValid(validLengthsOrderedByMaxLength);
        }


        public bool BeginningNumbersMatch()
        {
            return base.BeginningNumbersMatch(beginningNumbersTable);
        }
    }
}
