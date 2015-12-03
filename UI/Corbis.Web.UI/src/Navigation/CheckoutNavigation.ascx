<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register src="CheckoutControl.ascx" tagname="CheckoutControl" tagprefix="uc1" %>

<Corbis:ModalPopup ID="ModalPopup1" containerId="noItems" runat="server" width="300" meta:resourceKey="noItems">
    <Corbis:GlassButton ID="continue" runat="server" CausesValidation="false" OnClientClick="HideModal('noItems');return false;" meta:resourceKey="continueSearching"/>
</Corbis:ModalPopup>

<div id="CheckoutWidget">
    <uc1:CheckoutControl ID="CheckoutControl1" runat="server" />
</div>    

<script language="javascript" type="text/javascript">
    function GoToCartByFlow(byFlow) {
        if (byFlow) {
            var flow = '<%=Request.Params["OrderType"]%>';
            if (flow == '<%=Corbis.Web.Utilities.WorkflowHelper.COFF_FLOW%>') {
                window.location = '<%=Corbis.Web.UI.SiteUrls.MyLightBoxesAbsoluteHttpUrl%>';
                return;
            }
        }
        GoToCart();
    }
    function GoToCart() {
        if (CorbisUI.Auth.GetSignInLevel() < 1) {
            CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, "CorbisUI.GlobalNav.RefreshCartStatus(true)");
        } else {
            if ($('cartCount')) {
                var cartItems = parseInt($('cartCount').innerHTML, 10);
                if (!isNaN(cartItems) && cartItems > 0)
                {
                    window.location = '<%=Corbis.Web.UI.SiteUrls.Cart %>';
                }
                else
                {
                    OpenNoItems();
                }
            }
        }
    }
    
    function OpenNoItems()
    {
        new CorbisUI.Popup('noItems', { showModalBackground: false, closeOnLoseFocus: true, centerOverElement: 'cartCountDiv', 
            positionVert: -15, positionHoriz: -215  } );
    }

</script>