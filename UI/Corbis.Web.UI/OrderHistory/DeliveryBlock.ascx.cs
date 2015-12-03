using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class DeliveryBlock : System.Web.UI.UserControl
    {
        //private DeliveryMethod _orderDelivery = DeliveryMethod.Download;
        public bool EnableEdit { get; set; }

        public string DeliveryEmails
        {
            set { this.deliveryEmails.Text = value; }
        }
        //public string ShippingPriority
        //{
        //    set { this.shippingPriority.Text = value; }
        //}
        public DeliveryMethod OrderDelivery
        {
            set
            {
                if (value == DeliveryMethod.Email)
                {
                    email.Visible = true;
                    confirmEmail.Visible = false;
                }
				this.deliveryMethod.Text = CorbisBasePage.GetEnumDisplayText<DeliveryMethod>(value);
            }
        }

        //public MemberAddress ShippingAddress
        //{
        //    set
        //    {
        //        this.shippingName.Text = value.Name;
        //        this.shippingAddress123.Text = value.Address1;
        //        if (string.IsNullOrEmpty(value.Address2))
        //        {
        //            this.shippingAddress123.Text += @"<br />" + value.Address2;
        //            if (!string.IsNullOrEmpty(value.Address3))
        //                this.shippingAddress123.Text += @"<br />" + value.Address3;
        //        }
        //        this.shippingCity.Text = value.City;
        //        this.shippingState.Text = value.RegionCode;
        //        this.shippingZip.Text = value.PostalCode;
        //        this.shippingCountry.Text = value.CountryCode;
        //    }
        //}
        protected override void OnInit(EventArgs e)
        {
            if (!EnableEdit)
            {
                editLink.Visible = false;
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}