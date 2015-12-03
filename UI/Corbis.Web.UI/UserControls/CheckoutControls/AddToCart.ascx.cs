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

namespace Corbis.Web.UI.Checkout
{
	public partial class AddToCart : CorbisBaseUserControl
	{
        /// <summary>
        /// Stand alone control for adding media to cart
        /// 
        /// To use:
        ///		1. Register control on page
        ///			<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
        ///		2. Add control on page
        ///			<Corbis:AddToCart ID="addToCartControl" runat="server" />
        ///	    3. Add script include, and service reference in the page's code behind
        ///	        Page.ClientScript.RegisterClientScriptInclude("AddToCartScript", SiteUrls.AddToCartScript);
        ///	        ScriptManager manager = ScriptManager.GetCurrent(Page);
        ///	        manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
        ///		3. Hook up control to add item to cart
        ///			onclick="javascript:(new CorbisUI.Lightbox.ProductBlock('<%# Eval("CorbisId") %>')).addProductToCart(); return false;"
        ///		4. Setup function to call web service and handle on success event
        ///         addProductToCart: function(){
		///             var addToCart = new CorbisUI.Cart.AddToCart(this.productUID);
		///             addToCart.onSuccess = this.refreshCartItem;
		///             addToCart.addOfferingToCart(); 
        ///         }
        ///         refreshCartItem: function(results){
        ///             //add code to refresh page cart item
        ///         }
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}