<%@ Control Language="C#" AutoEventWireup="true" Codebehind="RfcdProducts.ascx.cs" Inherits="Corbis.Web.UI.Rfcd.RfcdProducts" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="~/Accounts/RoundCorners.ascx" TagName="RoundCorners" TagPrefix="Corbis" %>
<div id="ProductResults" class="ProductResults">
    <Corbis:LinkButton ID="hiddenRFPricingLink" CssClass="displayNone" runat="server"
        CausesValidation="false"></Corbis:LinkButton>
    <Corbis:Repeater ID="results" runat="server" OnItemDataBound="Result_ItemDataBound">
        <ItemTemplate>
            <span class="ProductBlock" id="productBlock" runat="server">
                <asp:UpdatePanel ID="addToCartPanel" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <Corbis:CenteredImageContainer 
                            IsAbsolute="true" 
                            runat="server" 
                            ImageID="image" 
                            id="thumbWrap"
                        />
                        <div class="Details">
                            <div id="addCartMessageDiv" runat="server" class="ModalPopupPanel AddToCart" visible="false">
                                <div class="ModalPopupContent">
                                    <Corbis:Label ID="addCartMessage" runat="server" meta:resourcekey="AddToCartMessage"></Corbis:Label><br />
                                    <br />
                                    <Corbis:LinkButton ID="gotoCart" runat="server" CommandArgument='<%# Eval("CorbisId") %>'
                                        OnCommand="GoToCart_Click" meta:resourcekey="GoToCart" /><br />
                                    <Corbis:LinkButton ID="continueShopping" CommandArgument='<%# Eval("CorbisId") %>'
                                        OnCommand="ContinueShopping_Click" runat="server" meta:resourcekey="ContinueShopping" />
                                </div>
                            </div>

                            <div class="categoryInfo">
                                <asp:Label  id="searchCategory" runat="server"></asp:Label>
                            </div>
                            <div class="collectionInfo">
                                <asp:Label id="marketingCollection" runat="server"></asp:Label>
                            </div>
                            
                            <div class="valueType <%# Eval("LicenseModel")+"_color" %>">
                                <Corbis:Label ID="Label1" runat="server" Text='<%# Eval("CorbisId") %>'></Corbis:Label>
                            </div>
                            
                            <div class="controlWrap">
					            <div class="CTL_Item ACTV_Details LT active hasTooltip" title="<%# GetLocalResourceObject(Eval("LicenseModel") + "LongText") %>"><span alt="<%# GetLocalResourceObject(Eval("LicenseModel") + "ShortText") %>" title="<%# GetLocalResourceObject(Eval("LicenseModel") + "LongText") %>"><%# Eval("LicenseModel") %></span></div>
								<div class="clr"> </div>
				            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
        </ItemTemplate>
    </Corbis:Repeater>
</div>
