using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Common
{
    public partial class IFrameTunnel : System.Web.UI.Page
    {
        private const string RELOAD = "reload";
        private const string RETURNURL = "returnurl";
        private const string EXECUTE = "execute";
        private const string CLOSE = "close";
       

        public string ActionArg = string.Empty;
        public string WindowId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string actionType = Request.QueryString["action"];
            string noClose = Request.QueryString["noclose"];
            WindowId = Request.QueryString["windowid"];
            ActionArg = Request.QueryString["actionArg"];

            if (!string.IsNullOrEmpty(actionType))
            {
                switch (actionType.ToLower())
                {
                    case CLOSE:
                        closeLogin.Visible = true;
                        break;
                    case RELOAD:
                        closeLogin.Visible = true;
                        reloadParent.Visible = true;
                        break;
                    case RETURNURL:
                        closeLogin.Visible = true;
                        redirectParent.Visible = true;
                        break;
                    case EXECUTE:
                        closeLogin.Visible = true;
                        execParent.Visible = true;
                        break;
                 }
            }
            if (!string.IsNullOrEmpty(noClose))
            {
                closeLogin.Visible = false;
            }
        }

    }
}
