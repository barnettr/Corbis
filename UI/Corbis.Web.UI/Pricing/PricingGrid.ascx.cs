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
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.Entities;
using System.Collections.Generic;

namespace Corbis.Web.UI.Pricing
{
    public partial class PricingGrid : CorbisBaseUserControl
    {
        #region Page events
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Properties
        public List<RsUseTypeAttributeValue> RSUseTypeAttributeValue
        {
            get 
            {
                return ( List<RsUseTypeAttributeValue> ) rpRSPricing.DataSource;
            }
            set 
            {
                if (value.Count > 0)
                {
                    rpRSPricing.DataSource = value;
                    rpRSPricing.DataBind();
                    rpRSPricing.Visible = true;
                }
                else
                {
                    rpRSPricing.Visible = false;
                }
            }
        }

        public string PricingGridHeader
        {
            get
            {
                return lblLicensingDetails.Text;
            }
            set
            {
                lblLicensingDetails.Text = value.ToString();
            }
        }
        #endregion
    }
}