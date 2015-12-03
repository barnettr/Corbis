<%@ Import Namespace="Corbis.LightboxCart.Contracts.V1"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CartItems.ascx.cs" Inherits="Corbis.Web.UI.Checkout.CartItems" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>



<Corbis:Repeater ID="cartItemsID" runat="server" OnItemDataBound="cartItemsID_ItemDataBound">
    <ItemTemplate>
    <% if (Zone == Corbis.Web.Entities.CartContainerEnum.PriceMultiple)
       {%>
       <div id="<%# "pinky_" +  Eval("ProductUid") %>" class="pinkyThumb">
            <div class="pinkyWrap">
                <asp:Image ID="cartThumb2" AlternateText='<%# Eval("CorbisId") %>' ImageUrl='<%# Eval("Url128") %>'
                                runat="server"  />
            </div>
        </div>
    <% }
       else
       {

           if (Zone == Corbis.Web.Entities.CartContainerEnum.Checkout)
           {%>
		    <li>
		    <% } %>
            <div id='<%# "cartBlock_" +  Eval("ProductUid") %>' class="cartBlock " nonLocalizedLicenseModel='<%#Eval("LicenseModel") %>' effectivepricestatus='<%#Eval("EffectivePriceStatus") %>'
                isimageavailable='<%#Eval("IsImageAvailable") %>' isoutline='<%# (bool)Eval("IsOutLine") && ((PriceStatus)Eval("EffectivePriceStatus")!=PriceStatus.PricedByAE) %>'
                isrfcd='<%#Eval("IsRfcd") %>'> 
                <Corbis:HoverButton runat="server" ID="btnClose" CssClass="hoverBtn closeIcon" meta:resourcekey="btnDelete" />
                <Corbis:HoverButton runat="server" CssClass="hoverBtn infoIcon" OnClientClick="showDetails(this);return false;" meta:resourcekey="btnInfo" />
                <div class="clr" style="font-size: 12px; color: #999999; margin-bottom: 3px;">
                    <Corbis:Label ID="corbisId" runat="server" Text='<%# Eval("CorbisId") %>' CssClass="corbisID"></Corbis:Label>
                </div>
                <asp:Panel ID="thumbPanel" CssClass="thumbPanel" runat="server">
                    <div onmouseout="" onmouseover="" class="thumbWrap Relative">
                        <div class="handle">
                            <asp:Image ID="cartThumb" AlternateText='<%# Eval("CorbisId") %>' 
                                runat="server" /></div>
                    </div>
                    <div class='<%# Eval("LicenseModel")+"color infoBox" %>'>
                        <div class="license">
                            <span>
                                <%# GetLocalResourceObject(Eval("LicenseModel") + "ShortText") %>
                            </span>
                        </div>
                       
                        <div class="action">
                            <span class="actLikeLink" id="priceTag" runat="server"></span>
                        </div>
                    </div>
                    <div id="pricedByAE" runat="server" class='<%# "pricedByAE " + Eval("LicenseModel") %>' ><%# GetLocalResourceObject("PricedByAEText") %></div>
                    <asp:Image ID="imageNotAvailable" AlternateText="" runat="server" CssClass="imageNotAvailable" />
                </asp:Panel>
                <asp:Panel ID="infoPanel" CssClass='<%# "detailsWrap " + Eval("LicenseModel") %>'
                    runat="server" Style="opacity: 0;">
                    <Corbis:Label ID="infoPanelCorbisId" runat="server" Text='<%# Eval("CorbisId") %>'></Corbis:Label><br />
                    <!--<span class="licenseType">
                        <%# Eval("LicenseModel") %>
                    </span>
                    <br />-->
                    <span class="creditLine" style="white-space:normal;"><%# Eval("CreditLine") == null ? "" : Server.HtmlEncode(Eval("CreditLine").ToString().Replace("/"," / "))%></span><br />
                    <br />
                    <asp:HyperLink CssClass="actLikeLink"  ID="imgDetail" runat="server" />
                        <br />
                    <asp:Label CssClass="actLikeLink" runat="server" ID="licenseDetails"></asp:Label> 
                        <br />
                        
                        
                        
                    <img onclick="refreshIcon(this);" class="RefreshMePlease" title="" alt="" src="/images/spacer.gif" />
                </asp:Panel>
            </div>
		    <% if (Zone == Corbis.Web.Entities.CartContainerEnum.Checkout)
            {%>
		    </li>
	        <% }
        } %>
    </ItemTemplate>
</Corbis:Repeater>
<% if (Zone == Corbis.Web.Entities.CartContainerEnum.Checkout)
		{%>
<asp:Panel ID="instructionId" CssClass="instructions" runat="server" Visible="false">
    <Corbis:Label ID="noItemMessage" runat="server" />
</asp:Panel>
<% } %>