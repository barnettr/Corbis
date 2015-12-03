using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Corbis.Web.UI.Controls
{
    public class CardNumberValidationBase
    {
        private const int MAXLENGTH_CEILING = 30;
        string number = String.Empty;


        public CardNumberValidationBase(string cardNumber)
        {
            this.number = cardNumber;
        }

        public static int MaxLength
        {
            get 
            {
                return MAXLENGTH_CEILING;    
            }
        }
    
        public virtual string CardNumber
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        public virtual bool IsValid()
        {
            return ValidateCardNumber();
        }


        public virtual bool BeginningNumbersMatch(string[] beginningCharacters)
        {
            for (int i = 0; i < beginningCharacters.Length; i++)
            {
                if(Regex.IsMatch(number, String.Format("^({0})",beginningCharacters[i])))
                    return true;
            }
            return false;
            
        }

        public virtual bool CardNumberLengthIsValid()
        {
            return number.Length <= MAXLENGTH_CEILING;
        }


        protected bool CardNumberLengthIsValid(int[] validLengths)
        {
            for (int i = 0; i < validLengths.Length; i++)
            {
                if (CardNumber.Length == validLengths[i])
                    return true;

            }
            return false;
        }

        #region Old External Site Validation Logic

        /// <summary>
        /// This code was brought over from the existing external site.
        /// Performs a validation using Luhn's Formula.
        /// </summary>
        protected bool ValidateCardNumber()
        {
            string cardNumber = number;

            try
            {
                // Array to contain individual numbers
                System.Collections.ArrayList CheckNumbers = new ArrayList();

                // So, get length of card
                int CardLength = cardNumber.Length;

                // Double the value of alternate digits, starting with the second digit
                // from the right, i.e. back to front.

                // Loop through starting at the end
                for (int i = CardLength - 2; i >= 0; i = i - 2)
                {
                    // Now read the contents at each index, this
                    // can then be stored as an array of integers

                    // Double the number returned
                    CheckNumbers.Add(Int32.Parse(cardNumber[i].ToString()) * 2);
                }

                int CheckSum = 0;	// Will hold the total sum of all checksum digits

                // Second stage, add separate digits of all products
                for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++)
                {
                    int _count = 0;	// will hold the sum of the digits

                    // determine if current number has more than one digit
                    if ((int)CheckNumbers[iCount] > 9)
                    {
                        int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
                        // add count to each digit
                        for (int x = 0; x < _numLength; x++)
                        {
                            _count = _count + Int32.Parse(((int)CheckNumbers[iCount]).ToString()[x].ToString());
                        }
                    }
                    else
                    {
                        _count = (int)CheckNumbers[iCount];	// single digit, just add it by itself
                    }

                    CheckSum = CheckSum + _count;	// add sum to the total sum
                }

                // Stage 3, add the unaffected digits
                // Add all the digits that we didn't double still starting from the right
                // but this time we'll start from the rightmost number with alternating digits
                int OriginalSum = 0;
                for (int y = CardLength - 1; y >= 0; y = y - 2)
                {
                    OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString());
                }

                // Perform the final calculation, if the sum Mod 10 results in 0 then
                // it's valid, otherwise its false.
                return (((OriginalSum + CheckSum) % 10) == 0);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
