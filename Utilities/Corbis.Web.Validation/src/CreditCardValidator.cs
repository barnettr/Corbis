using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.Validation
{
	public enum CardType
	{
		MasterCard,
		VISA,
		Amex,
		DinersClub,
		enRoute,
		Discover,
		JCB
	}

	public class CreditCardValidator : BaseValidator
    {
        #region Fields

        private string cardNumber;
        private CardType cardType;
        private int expirationMonth;
        private int expirationYear;
        private string cardNumberControlID;
        private string cardTypeControlID;
        private string expirationMonthControlID;
        private string expirationYearControlID;

        #endregion

        #region Properties

        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        public CardType CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        public int ExpirationMonth
        {
            get { return expirationMonth; }
            set { expirationMonth = value; }
        }

        public int ExpirationYear
        {
            get { return expirationYear; }
            set { expirationYear = value; }
        }

        public string CardNumberControlID
        {
            get { return cardNumberControlID; }
            set { cardNumberControlID = value; }
        }

        public string CardTypeControlID
        {
            get { return cardTypeControlID; }
            set { cardTypeControlID = value; }
        }

        public string ExpirationMonthControlID
        {
            get { return expirationMonthControlID; }
            set { expirationMonthControlID = value; }
        }

        public string ExpirationYearControlID
        {
            get { return expirationYearControlID; }
            set { expirationYearControlID = value; }
        }

        #endregion

        public CreditCardValidator()
		{
        }

        #region Overrides

        protected override bool ControlPropertiesValid()
		{
            try
            {
                Control cardNumberControl = FindControl(CardNumberControlID);
                if (cardNumberControl != null && cardNumberControl is TextBox)
                {
                    CardNumber = ((TextBox)cardNumberControl).Text;
                }
                else
                {
                    return false;
                }

                Control cardTypeControl = FindControl(CardTypeControlID);
                if (cardTypeControl != null && cardTypeControl is DropDownList)
                {
                    CardType = (CardType)Enum.Parse(typeof(CardType), ((DropDownList)cardTypeControl).SelectedItem.Text);
                }
                else
                {
                    return false;
                }

                Control expirationMonthControl = FindControl(ExpirationMonthControlID);
                if (expirationMonthControl != null && expirationMonthControl is DropDownList)
                {
                    ExpirationMonth = int.Parse(((DropDownList)expirationMonthControl).SelectedValue);
                }
                else
                {
                    return false;
                }

                Control expirationYearControl = FindControl(ExpirationYearControlID);
                if (expirationYearControl != null && expirationYearControl is DropDownList)
                {
                    ExpirationYear = int.Parse(((DropDownList)expirationYearControl).SelectedValue);
                }
                else
                {
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("CreditCardValidator: ControlPropertiesValid()", ex);
            }
		}

		protected override bool EvaluateIsValid()
		{
            // TODO: localize text
            try
            {
                CardNumber = CardNumber.Trim().Replace(" ", "").Replace("-", "");

                if (string.IsNullOrEmpty(CardNumber))
                {
                    Text = "Please enter a credit card number.";
                    return false;
                }

                DateTime expiredDate = new DateTime(ExpirationYear, ExpirationMonth, 1).AddMonths(1);
                if (DateTime.Now.CompareTo(expiredDate) == 1)
                {
                    Text = "Expiration date is invalid.";
                    return false;
                }

                if (!IsValidCardType(CardType, CardNumber) || !IsValidCardNumber(CardNumber))
                {
                    Text = "Invalid card number.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("CreditCardValidator: EvaluateIsValid()", ex);
            }
        }

        #endregion

        private static bool IsValidCardType(CardType cardType, string cardNumber)
		{
            switch (cardType)
			{
				case CardType.Amex:
                    return Regex.IsMatch(cardNumber, "^(34|37)") && cardNumber.Length == 15;
				case CardType.MasterCard:
                    return Regex.IsMatch(cardNumber, "^(51|52|53|54|55)") && cardNumber.Length == 16;
				case CardType.VISA:
                    return Regex.IsMatch(cardNumber, "^(4)") && (cardNumber.Length == 13 || cardNumber.Length == 16);
                /*
                case CardType.DinersClub:
					return Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)") && cardNumber.Length == 14;
                case CardType.enRoute:
					return Regex.IsMatch(cardNumber, "^(2014|2149)") && cardNumber.Length == 15;
                case CardType.Discover:
					return Regex.IsMatch(cardNumber, "^(6011)") && cardNumber.Length == 16;
                case CardType.JCB:
					return (Regex.IsMatch(cardNumber, "^(3)") && cardNumber.Length == 16) ||
                        (Regex.IsMatch(cardNumber, "^(2131|1800)") && cardNumber.Length == 15);
                */
                default:
                    return false;
            }
		}

        private static bool IsValidCardNumber(string cardNumber)
		{
			try 
			{
				ArrayList checkNumbers = new ArrayList();
                int cardLength = cardNumber.Length;
                int sum1 = 0;
                int sum2 = 0;

				for (int i = cardLength - 2; i >= 0; i -= 2)
				{
					checkNumbers.Add(int.Parse(cardNumber[i].ToString())*2);
				}
			
				for (int i = 0; i <= checkNumbers.Count-1; ++i)
				{
                    int sum = 0;

					if ((int)checkNumbers[i] > 9)
					{
						int length = ((int)checkNumbers[i]).ToString().Length;
                        for (int j = 0; j < length; j++)
						{
                            sum += int.Parse(((int)checkNumbers[i]).ToString()[j].ToString());
						}
					}
					else
					{
						sum = (int)checkNumbers[i];
					}

                    sum1 += sum;
				}

				for (int i = cardLength - 1; i >= 0; i -= 2)
				{
                    sum2 += int.Parse(cardNumber[i].ToString());
				}

                return (sum1 + sum2) % 10 == 0;
			}
			catch (Exception ex)
			{
                throw new Exception("CreditCardValidator: ValidateCardNumber()", ex);
			}
		}
	}
}
