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
    public partial class beta : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Beta, "Beta");
            RequiresSSL = true;
            base.OnInit(e);
            
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblEulaTitle.Text = Resources.Legal.BetaSiteUsageAgreementTitle;
            lblEulaText.Text = Resources.Legal.BetaSiteUsageAgreementText;
            pnlFailure.Visible = false;
            pnlNotAccepted.Visible = false;

            //this.SetAnalytics("LND:Home:Beta:SignIn"); // "Home:SearchCreative", "Home:SearchCurrentEvents", "Home:SearchCore"

            //Pre-populate username to improve UX
            if (txtUsername.Text == "" && Profile.Current.UserName != "Anonymous")
            {
                ClientScript.RegisterStartupScript(typeof(Page), "setUsername", "<script type=\"text/javascript\">$('" + (txtUsername.ClientID) + "').value='" + Profile.Current.UserName + "';</script>");

               
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userIPAddress = ClientIPHelper.GetClientIpAddress();

            try
            {
                //We are just ghoing to step through the validation criteria in order of display

                //Check user accepts Terms and Conditions
                if (acceptTerms.Checked)
                {
                    //Clean CSS Required Field Validator for username
                    if (txtUsername.Text == "")
                    {
                        SetNoUsernamePanel();
                    }
                    else
                    {
                        //Clean CSS Required Field Validator for password
                        if (txtPassword.Text == "")
                        {
                            SetNoPasswordPanel();
                        }
                        else
                        {
                            //Finally check AUTH to see if the user has BETA Access
                            if (BetaSiteAuthentication.AuthenticateUser(txtUsername.Text, txtPassword.Text, userIPAddress))
                            {
                                //Woohoo!
                                Response.Redirect(SiteUrls.Home);
                            }
                            else
                            {
                                //Boo!
                                SetNotAuthorizedPanel();
                            }
                        }
                    }
                    
                }
                else
                {
                    SetNonAcceptancePanel();

                }
                
            }
            catch(Exception ex)
            {
                //Do we need to log the exception?
                string errorText = ex.Message;
                errorChecks.Text = errorText;
                SetNotAuthorizedPanel();
            }
            
        }


        /// <summary>
        /// For use when a page needs to be sure it's redirecting to a non-secure host
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        private string GetClearURL(string pageName)
        {
            if (pageName == null || pageName.Trim().Length == 0)
            {
                return "/default.aspx";
            }

            if (!Request.IsSecureConnection)
            {
                return pageName;
            }

            string returnValue = "http://" + Request.Url.Host;

            if(pageName.StartsWith("/"))
            {
                returnValue += pageName;
            }
            else
            {
                returnValue += "/" + pageName;
            }

            return returnValue;
        }

        private void SetNotAuthorizedPanel()
        {

            lblFailureText.Text = "Please enter a valid Username and password.";

            pnlFailure.Visible = true;
        }

        private void SetNonAcceptancePanel()
        {

            lblNotAcceptedText.Text = "Accept Terms &amp; Conditions to access the beta site";

            pnlNotAccepted.Visible = true;
        }
        private void SetNoUsernamePanel()
        {

            lblNoUsernameText.Text = "Please enter a valid Username.";

            pnlNoUsername.Visible = true;
        }
        private void SetNoPasswordPanel()
        {

            lblNoPasswordText.Text = "Please enter a valid Password";

            pnlNoPassword.Visible = true;
        }
    }
}
