using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Xml;

using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using System.Configuration;


namespace Corbis.Web.UI
{
	public partial class Default : CorbisBrowseBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            RequiresSSL = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GetQueryString("ReturnUrl")))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showLogin", "window.addEvent('load', function() { CorbisUI.Auth.Check(2,CorbisUI.Auth.ActionTypes.ReturnUrl,'" + Server.HtmlEncode(GetQueryString("ReturnUrl")) + "')});", true);
            }

            if (!string.IsNullOrEmpty(GetQueryString("SignOut")))
            {
                DoSignOut();
            }
		}

		#region Private Methods
		private void DoSignOut()
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
		#endregion
	}
}
