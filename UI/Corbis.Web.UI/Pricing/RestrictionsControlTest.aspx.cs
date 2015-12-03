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

using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.Web.UI.Presenters;


namespace Corbis.Web.UI.Pricing
{
    public partial class yo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageServiceAgent agent = new ImageServiceAgent();
            DisplayImage displayImage = agent.GetDisplayImage("OUT17566042", "en-US", true);
            ImageRestrictionsPresenter restrictionsPresenter =
                new ImageRestrictionsPresenter(this.ImageRestrictions, displayImage);
            restrictionsPresenter.SetRestrictions();
        }
    }
}
