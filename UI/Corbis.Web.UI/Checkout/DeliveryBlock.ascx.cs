using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Checkout
{
    public partial class DeliveryBlock : System.Web.UI.UserControl
    {
        //private DeliveryMethod _orderDelivery = DeliveryMethod.Download;
        public bool EnableEdit { get; set; }
        public string DeliverySubject {
            set
            {
                this.deliverySubject.Visible = value != null && !value.Equals(String.Empty);
                this.deliverySubject.Text = Server.HtmlEncode(value);
            }
        }
        public string DeliverySpecialInstructions { 
            set
            {
                this.deliverySpecialInstructions.Visible = value != null && !value.Equals(String.Empty);
                this.deliverySpecialInstructions.Text = Server.HtmlEncode(value);
            }
        }
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
        protected override void  OnPreRender(EventArgs e)
        {
 	        base.OnPreRender(e);
            bool visible = false;
            int colspan = 1;
            string width = "200px";
            string preWidth = "225px";
            bool coffWorkflow = WorkflowHelper.IsCOFFWorkflow(HttpContext.Current.Request);
            if (!coffWorkflow)
            {
                visible = false;
                colspan = 3;
            }
            else
            {
                if (this.deliverySubject != null && this.deliverySubject.Text.Trim().Equals(String.Empty))
                {
                    if (this.deliverySpecialInstructions != null && this.deliverySpecialInstructions.Text.Trim().Equals(String.Empty))
                    {
                        visible = false;
                        colspan = 3;
                    }
                    else
                    {
                        visible = true;
                        colspan = 1;
                    }
                }
                else
                {
                    visible = true;
                    colspan = 1;
                }
            }
            this.deliverySubject.Visible = visible;
            this.deliverySpecialInstructions.Visible = visible;
            this.subject.Visible = visible;
            this.specialInstructions.Visible = visible;
            this.subjectSpan.Visible = visible;
            this.deliverySubjectSpan.Visible = visible;
            this.deliveryEmailsSpan.ColSpan = colspan;
            this.specialInstructionsSpan.Visible = visible;
            this.deliverySpecialInstructionsSpan.Visible = visible;
            this.deliveryMethodSpan.ColSpan = colspan;

            if (visible)
            {
                this.deliveryMethodSpan.Width = preWidth;
                this.deliveryEmailsSpan.Width = preWidth;
                this.subjectSpan.Width = width;
                this.deliverySubjectSpan.Width = width;
                this.specialInstructionsSpan.Width = width;
                this.deliverySpecialInstructionsSpan.Width = width;
            }
        }
    }
}
