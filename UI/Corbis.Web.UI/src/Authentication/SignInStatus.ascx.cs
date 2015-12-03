using System;
using System.Web.Security;
using Corbis.Web.UI.Controls;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using System.Collections.Generic;
using System.Web;

namespace Corbis.Web.UI.Authentication
{
    public partial class SignInStatus : CorbisBaseUserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //register.NavigateUrl = SiteUrls.Register;
            string registerText = string.Empty;
            registerText = GetLocalResourceObject("register").ToString();
            registerText = registerText.Replace("$register", "javascript:redirectToRegister();");
            register.Text = registerText;
            //register.NavigateUrl = "javascript:redirectToRegister();";
            //signIn.NavigateUrl = string.Format("{0}?ReturnUrl={1}", SiteUrls.SignIn, Request.Url.PathAndQuery);
            string signInOrRegisterText = string.Empty;
            signInOrRegisterText = GetLocalResourceObject("signInOrRegister").ToString();
            //signInOrRegisterText = signInOrRegisterText.Replace("$signIn", string.Format("{0}?ReturnUrl={1}", SiteUrls.SignIn, Request.Url.PathAndQuery));
            signInOrRegisterText = signInOrRegisterText.Replace("$register", SiteUrls.Register);
            signInOrRegister.Text = signInOrRegisterText;

            signOut.NavigateUrl = Corbis.Web.UI.Properties.Settings.Default.HttpUrl + SiteUrls.Home + "?SignOut=y";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // partially- or fully-authenticated
            if (!Profile.IsAnonymous)
            {
				name.Text = GetLocalResourceObject("displayName").ToString()
                    .Replace("$firstName", Server.HtmlEncode(Profile.FirstName))
                    .Replace("$lastName", Server.HtmlEncode(Profile.LastName));
                register.Visible = false;
                signIn.Visible = false;

                // fully-authenticated
                if (Profile.IsAuthenticated)
                {
                    signInOrRegister.Visible = false;
                }
                // partially-authenticated
                else
                {
                    signOut.Visible = false;
                }
            }
            // anonymous
            else
            {
                name.Text = string.Empty;
                signOut.Visible = false;
                signInOrRegister.Visible = false;
                signIn.Attributes.CssStyle.Add("border", "none");
            }
        }

        protected void SignOut_Click(object sender, EventArgs e)
        {
            if (!Profile.IsAnonymous)
            {
                // sign out, stay partially-authenticated
                if (Profile.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
					Dictionary<Guid, CartProductContainerInfo> productsContainerInfo = stateItems.GetStateItemValue<Dictionary<Guid, CartProductContainerInfo>>(LightboxCartKeys.Name, LightboxCartKeys.CartProductUids, StateItemStore.AspSession);
                    StateItem<Dictionary<Guid, CartProductContainerInfo>> stateItem = new StateItem<Dictionary<Guid, CartProductContainerInfo>>(LightboxCartKeys.Name, LightboxCartKeys.CartProductUids, productsContainerInfo, StateItemStore.AspSession);
                    stateItems.DeleteStateItem<Dictionary<Guid, CartProductContainerInfo>>(stateItem);
                }
            }
            Response.Redirect(SiteUrls.Home);
        }
    }
}
