using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Framework.Globalization;
using Corbis.Web.Entities;
namespace Corbis.Web.UI.Chat
{
    public partial class InstantServiceButtonSmall : CorbisBaseUserControl
    {
        
       
        protected void Page_Load(object sender, EventArgs e)
        {           
          this.litSecure.Text = Request.IsSecureConnection ? "s" : "";
          this.litDepartment.Text = getDepartment();
        }

      
       
        private string getDepartment()
        {
            string departmentNumber = "0";
            string cultureName = Language.CurrentLanguage.LanguageCode;
			if (string.IsNullOrEmpty(cultureName)) cultureName = "en-US";
            switch (cultureName)
            {
                case "de-DE":
                    departmentNumber = "21323";
                    break;
                case "en-US":
                    departmentNumber = "21319";
                    break;
                case "es-ES":
                    departmentNumber = "21324";
                    break;
                case "fr-FR":
                    if (this.Profile.CountryCode == "CA")
                    {
                        departmentNumber = "21321";
                    }
                    else
                    {
                        departmentNumber = "21322";
                    }
                    break;
                case "it-IT":
                    departmentNumber = "21325";
                    break;
                case "en-GB":
                    departmentNumber = "21320";
                    break;
                case "nl-NL":
                    departmentNumber = "21326";
                    break;
            }
            return departmentNumber;
        }


      

              
    }
}