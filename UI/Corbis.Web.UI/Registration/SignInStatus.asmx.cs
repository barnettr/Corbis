using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Corbis.Web.Authentication;

namespace Corbis.Web.UI.Registration
{
    /// <summary>
    /// Summary description for SignInStatus
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class SignInStatus : Corbis.Web.UI.src.CorbisWebService
    {
        public class SignInStatusAndCountry
        {
            public string signInState { get;set;}
            public string country { get; set; }
        }
        [WebMethod(true)]
        public string SignInState()
        {
            Profile profile = Profile.Current;
            // Return "0" for Anon user
            if (profile.IsAnonymous)
            {
                return "0";
            }
            else
            {
                // Return "2" for logged in user
                if (profile.IsAuthenticated)
                {
                    return "2";
                }
                else
                {
                    // Return "1" for partially logged in user
                    return "1";
                }
            }
        }

        [WebMethod(true)]
        public SignInStatusAndCountry SignInStateAndCountry()
        {
            Profile profile = Profile.Current;

            SignInStatusAndCountry status = new SignInStatusAndCountry();
            status.signInState = SignInState();
            status.country = profile.CountryCode;
            return status;
        }
    }
}
