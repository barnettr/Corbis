using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Globalization;
using System.Threading;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.src
{
    public class CorbisWebService : WebService
    {
        public CorbisWebService()
        {
            languageSetup();
        }
        private void languageSetup()
        {
            string languageName;
            if (HttpContext.Current.Request.Cookies["Profile"] == null || HttpContext.Current.Request.Cookies["Profile"]["Culture"] == null)
                languageName = "en-US";
            else
            {
                languageName = HttpContext.Current.Request.Cookies["Profile"]["Culture"];
                languageName = languageName.Replace("%2D", "-");
                if (languageName == "zh-CHS")
                    languageName = "zh-CN";
            }
            CultureInfo info = new CultureInfo(languageName, false);
            info.NumberFormat.CurrencySymbol = string.Empty;
            Language.CurrentCulture = info;
        }
    }
}
