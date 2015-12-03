<%@ Control Language="C#" AutoEventWireup="True" Codebehind="ItemSummary.ascx.cs" Inherits="Corbis.Web.UI.Checkout.ItemSummary" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>


<div id="itemSummaryContainer" runat="server" class="rounded4" internal="true">
    <div id="summaryItems">
        <asp:Repeater ID="summaryItemsRepeater" runat="server" EnableViewState="false" OnItemDataBound="summaryItemsRepeater_ItemDataBound">
            <HeaderTemplate>
                <h3>
                    <div class="padMe">
                        <span id="itemSummaryTitle"><Corbis:Localize ID="itemSummary2" runat="server" meta:resourcekey="itemSummary"></Corbis:Localize></span>
                        <span class="editMyCart">
                            <a href="javascript:void(0)" id="blankEditMyCartLink" style="cursor:default;"></a>                        
                            <Corbis:WorkflowBlock ID="WorkflowBlock1" WorkflowType="CART" runat="server">
                                <a href="javascript:GoToCart()" id="editMyCartLink"><Corbis:Localize ID="editCart" runat="server" meta:resourcekey="editCart"></Corbis:Localize></a>
                            </Corbis:WorkflowBlock>
                            <img id="loadingViewRestrictions" style="float: right; display: none;" src="../Images/ajax-loader.gif" />
                        </span>
                    </div>
                </h3>
                <div class="clr"></div>
                <div id="summaryItemsContainer">
                    <table cellspacing="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="checkoutItem" productuid="<%# Eval("ProductUid") %>">
                            <td class="checkoutItemPhoto">
                                <Corbis:CenteredImageContainer IsAbsolute="true" runat="server" ImageID="image" ID="thumbWrap"
                                    Ratio='<%# Eval("AspectRatio") %>' Size='50' ImgUrl='<%#Eval("URL128").ToString().Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=90&im=") %>' />
                            </td>
                            <td class="checkoutItemDetails">
                                <div class="checkoutItemNumber">
                                    <%# Eval("CorbisId") %>
                                </div>
                                <div id="checkoutItemType" runat="server" >
<%--                                    <%# Eval("PriceTier") %>
                                    <%# Eval("LicenseModel") %>--%>
                                </div>
                                <div class="checkoutItemRestrictions" runat="server" id="checkoutItemRestrictions">
                                    <a onclick="viewRestrictions(this,'<%# Eval("CorbisId") %>');return false;">
                                        <Corbis:Localize runat="server" meta:resourcekey="viewRestrictions"></Corbis:Localize></a>
                                </div>
                            </td>
                            <td class="checkoutItemPrice">
                                
                                <%#Eval("EffectivePrice")%>
                                
                                <%-- COMMENTED OUT FOR NOW - Chris
                                <div class="">
                                    <a href="#" onclick="deleteItemSummaryItem(this);return false;">[ X ]</a>
                                </div>
                                --%>
                                
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                </table>
             </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <asp:UpdatePanel runat="server" ID="priceUpdateBlock" UpdateMode="Conditional" >
        <ContentTemplate >
            <div class="summaryTotals">
                <div class="subTotal">
                    <div>
                        <div id="subTotalAmount" runat="server"></div>
                        <div id="subTotalName">
                            <Corbis:Localize ID="Subtotal" runat="server" meta:resourcekey="Subtotal"></Corbis:Localize>
                        </div>
                        <div class="clr" ></div>
                    </div>
                    <div runat="server" id="promotionZone" visible="false" class="promotionZone">
                        <div id="promotionAmount" runat="server" >
                        </div>
                        <div>
                            <Corbis:Localize runat="server" meta:resourcekey="promotionCode"></Corbis:Localize>
                        </div>
                    </div>
                </div>
                <div id="taxAndShipping">
                    <div id="ksaBlock" class="KSAtax" runat="server" visible="false" ><!-- this should only appear if the user is German -->
                        <!-- make this visibility server side: it should not appear until step 4 -->
                        <div id="KSAtaxDiv" runat="server" >&nbsp;</div>
                        <Corbis:Localize ID="KSAtaxValue" runat="server" meta:resourcekey="KSAtaxValue"></Corbis:Localize>
                    </div>
                    <div class="tax">
                        <!-- make this visibility server side: it should not appear until step 4 -->
                        <div id="taxDiv" runat="server">&nbsp;
                        </div>
                        <Corbis:Localize ID="Tax2" runat="server" meta:resourcekey="Tax"></Corbis:Localize>
                    </div>
                    <div class="shipping">
                        <!-- make this visibility server side: it should not appear until step 4 -->
                        <div id="shippingDiv" runat="server" style="visibility: hidden;">&nbsp;
                            25.00 <Corbis:Localize ID="currency3" runat="server" meta:resourcekey="currency"></Corbis:Localize></div>
                        <Corbis:Localize ID="Shipping2" runat="server" meta:resourcekey="Shipping"></Corbis:Localize>
                    </div>
                </div>
            </div>
            <div class="total">
                <div class="totalDivLabel">
                    <Corbis:Localize ID="Total2" runat="server" meta:resourcekey="Total"></Corbis:Localize>
                </div>
                <div id="totalDiv" class="totalDivValue" runat="server">
                    <asp:Label runat="server" ID="summaryTotal"></asp:Label>
                </div>
                <div class="clr"></div>
            </div>
        </ContentTemplate>  
    </asp:UpdatePanel>
</div>



<div id="viewRestrictionsTemplate" style="display:none;">
	    <div class="ModalPopupPanelDialog">
	        <div class="ModalTitleBar">
	            <span class="Title">
	                <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width:0px;" onclick="javascript:MochaUI.CloseModal('viewRestrictionsTemplate');return false;" />
                    <Corbis:Localize ID="Localize3" runat="server" meta:resourcekey="restrictions"></Corbis:Localize> </span>
            </div>
		    <div class="ModalPopupContent">
		        {0}
                <div id="Div1">
                    <Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="modelReleased"></Corbis:Localize><span class="value"> {1}</span>
                </div>
                <div id="Div2">
                        <Corbis:Localize ID="Localize2" runat="server" meta:resourcekey="propertyReleased"></Corbis:Localize><span class="value"> {2}</span>
                </div>
                <div class="padParagraphs">
                    {3}
                </div>
            </div>
        </div>
	</div>

<%-- COMMENTING OUT FOR NOW -- Chris

<Corbis:ModalPopup ID="deleteItemLayer" ContainerID="deleteItemLayer" runat="server"  meta:resourcekey="deleteItemLayer">

    <div style="float:left;margin:5px;">{0}</div>
    <Corbis:Localize ID="deleteItemLabel" runat="server" meta:resourcekey="deleteItemLabel"></Corbis:Localize>

     <Corbis:GlassButton ID="btnQuitCheckout" OnClientClick="alert('delete!');MochaUI.CloseModal('deleteItemLayer');return false;"
            runat="server" CssClass="" ButtonStyle="Gray"
            meta:resourcekey="btnDeleteItem"></Corbis:GlassButton>

            <Corbis:GlassButton ID="btnContinueCheckout" OnClientClick="MochaUI.CloseModal('deleteItemLayer');return false;"
            runat="server" CssClass="" ButtonStyle="Orange"
            meta:resourcekey="btnNoThanks"></Corbis:GlassButton>
</Corbis:ModalPopup>

--%>

<script language="javascript" type="text/javascript">
    CorbisUI.GlobalVars.Checkout = {
		editMyCartLinkText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Checkout/ItemSummary.ascx", "editCart.Text").ToString()) %>'
    }
</script>

