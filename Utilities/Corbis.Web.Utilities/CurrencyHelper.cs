using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Corbis.Framework.Globalization;

namespace Corbis.Web.Utilities
{
   public static class CurrencyHelper
    {
       public static string GetLocalizedCurrency(string value)
       {           
           //CultureInfo cultureInfo=Language.CurrentCulture;
           //cultureInfo.NumberFormat.CurrencySymbol=string.Empty;           
           NumberStyles style = NumberStyles.Number;          
           double result = default(double);
           double.TryParse(value, style, CultureInfo.InvariantCulture, out result); 
     
           return string.Format("{0:C}", result);                   
        
       }
       public static string GetLocalizedCurrencyInvariant(string value)
       {
           CultureInfo cultureInfo = Language.CurrentCulture;  
           NumberStyles style = NumberStyles.Number;
           cultureInfo.NumberFormat.CurrencySymbol = string.Empty;  
           double result = default(double);
           double.TryParse(value, style, cultureInfo,out result); 
           return string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:0,0.00}", result);
           
       }    
    }
   public static class CurrencyFormat
   {
       public static readonly string defaultCurrency ="{0:C}";
   }
}
