using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Corbis.Web.UI.Test
{
    /// <summary>
    /// Summary description for ValidationTestService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ValidationTestService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetAlwaysSuccess(string param1, string param2)
        {
            return "success";
        }

        [WebMethod]
        public string ValidateLength(string param1, string param2)
        {
            return param1.Length >= 2 && param1.Length <= 4 ? 
                "success" :
                "(wsdl) enter a string between 2 and 4 chars long";
        }

        [WebMethod]
        public string GetStringConcatParams(string param1, string param2)
        {
            return string.Format("{0} {1}");
        }
    }
}
