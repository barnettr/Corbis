using System;
using System.Drawing;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Navigation
{
    public partial class CheckoutControl : CorbisBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Profile.IsECommerceEnabled)
            {
                int count = Profile.CartItemsCount;
                cartCountLit.Text = count.ToString();             
            }
            else
            {
                Visible = false;
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public string CartCount
        {
            get
            {
               return  cartCountLit.Text;
            }

            set
            {

                cartCountLit.Text = value;
            }
            
        }
    }
}