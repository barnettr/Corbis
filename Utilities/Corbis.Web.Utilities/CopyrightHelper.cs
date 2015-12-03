using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Corbis.Web.Utilities
{
    public class CopyrightHelper
    {
        public static string CopyrightText
        {
            get
            {
                string copyrightText = HttpContext.GetGlobalResourceObject("Legal", "Copyright") as string;
                if (copyrightText != null)
                {
                    copyrightText = copyrightText.Replace("{YEAR}", DateTime.Now.Year.ToString());
                }
                else
                {
                    copyrightText = string.Empty;
                }
                return copyrightText;
            }
        }
    }
}
