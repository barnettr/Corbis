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
using Corbis.Web.Utilities;
using Corbis.Web.Authentication;

namespace Corbis.Web.UI
{
    public partial class BetaThankYou : CorbisBasePage
    {
        public string myUsername = "";
        public string myEmail = "";
        protected override void OnInit(EventArgs e)
        {
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Beta, "Beta");
            RequiresSSL = false;
            base.OnInit(e);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.SetAnalytics("LND:CustomerService:Beta:ThankYou"); 

            myUsername = Profile.UserName;
            myEmail = Profile.Email;

           
            }
        }
    
}
