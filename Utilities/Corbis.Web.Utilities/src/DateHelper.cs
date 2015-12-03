using System;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using Corbis.Framework.Globalization;


namespace Corbis.Web.Utilities
{
    public static class DateHelper
    {     

        public static ListItemCollection GetMonths()
        {
            ListItemCollection collection = new ListItemCollection();
            collection.Add(new ListItem("MM", string.Empty));
            collection.Add(new ListItem("01", "1"));
            collection.Add(new ListItem("02", "2"));
            collection.Add(new ListItem("03", "3"));
            collection.Add(new ListItem("04", "4"));
            collection.Add(new ListItem("05", "5"));
            collection.Add(new ListItem("06", "6"));
            collection.Add(new ListItem("07", "7"));
            collection.Add(new ListItem("08", "8"));
            collection.Add(new ListItem("09", "9"));
            collection.Add(new ListItem("10", "10"));
            collection.Add(new ListItem("11", "11"));
            collection.Add(new ListItem("12", "12"));
            return collection;
        }
        

        public static ListItemCollection GetYears()
        {
            ListItemCollection collection = new ListItemCollection();
            collection.Add(new ListItem("YYYY", string.Empty));
            for (int i = 0; i < 20; ++i)
            {
                collection.Add(new ListItem(DateTime.Now.AddYears(i).Year.ToString()));
            }
            return collection;
        }

        public static string GetLocalizedDate(DateTime date)
        {
            return date.ToString("d", Language.CurrentCulture);
        }

        public static string GetLocalizedDate(string date)
        {
            DateTime initialDate;
            string localizedDate = string.Empty;

            if (DateHelper.TryParse(date, out initialDate))
            {
                localizedDate = initialDate.ToString("d", Language.CurrentCulture);

            }
            else
            {
                localizedDate = date;
            }

            return localizedDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
         public static string GetLocalizedDate(DateTime date,string dateFormat)
        {
            return string.Format(Language.CurrentCulture, dateFormat, date);            
             
        }
        public static string GetLocalizedLongDateWithoutWeekday(DateTime date)
        {
            string localizedDate = string.Empty;
            CultureInfo currentCulture = Language.CurrentCulture;

            localizedDate = string.Format(currentCulture, DateFormat.LongDateFormat, date);

            if (currentCulture.Name != Language.EnglishUK.LanguageCode && 
                currentCulture.Name != Language.Polish.LanguageCode && 
                currentCulture.Name != Language.ChineseSimplified.LanguageCode && 
                currentCulture.Name != Language.Japanese.LanguageCode)
            {
                localizedDate = StringHelper.RemoveFirstWord(localizedDate);
            }

            return localizedDate;
        }

        public static string GetLocalizedLongDateWithoutWeekday(string date)
        {
            DateTime initialDate;
            string localizedDate = string.Empty;

            if (DateHelper.TryParse(date, out initialDate))
            {
                localizedDate = DateHelper.GetLocalizedLongDateWithoutWeekday(initialDate);
            }
            else
            {
                localizedDate = date;
            }
            return localizedDate;
        }

        public static string YearMonthPattern
        {
            get
            {
                return Language.CurrentCulture.DateTimeFormat.YearMonthPattern;
            }
        }

        /// <summary>
        /// Tries to parse the date string according to the current culture
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static bool TryParse(string dateString, out DateTime date)
        {
            if (DateTime.TryParse(dateString,
                    Language.CurrentCulture.DateTimeFormat,
                    DateTimeStyles.None,
                out date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static class DateFormat
    {          /// <summary>
          /// Eg. 4/17/2009
         /// </summary>
          public static readonly string ShortDateFormat ="{0:d}";
          /// <summary>
          /// Eg. Monday April 17, 2006
          /// </summary>
          public static readonly string LongDateFormat = "{0:D}"; 
          /// <summary>
          /// Eg. Monday, April 17, 2006 2:22:48 PM
          /// </summary>
          public static readonly string FullDateLongTimeFormat = "{0:F}";
          /// <summary>
          /// Eg. April, 2006
          /// </summary>
          public static readonly string YearFormat = "{0:Y}"; 

    }


}
