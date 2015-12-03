<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LightboxItems.ascx.cs"
    Inherits="Corbis.Web.UI.Search.LightboxItems" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<Corbis:Repeater runat="server" ID="searchBuddyLightbox" OnItemDataBound="searchBuddyLightbox_ItemDataBound">
    <ItemTemplate>
        <div id="lboxBlock" runat="server">
            <Corbis:HoverButton runat="server" ID="btnClose" CssClass="hoverBtn closeIcon"></Corbis:HoverButton>
            <Corbis:CenteredImageContainer ImgUrl='<%# ((String)DataBinder.Eval(Container.DataItem, "Url128")) %>'
                Size="90" IsAbsolute="true" runat="server" ImageID="image" ID="imageThumb" Ratio='<%#DataBinder.Eval(Container.DataItem, "AspectRatio") %>'
                AltText='<%# String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "CorbisId"), DataBinder.Eval(Container.DataItem, "Title")) %>' />
            <div class='<%# Eval("LicenseModel")+"color infoBox" %>'>
                <div class="license">
                    <span>
                        <%# Eval("LicenseModel") %>
                    </span>
                </div>
                <Corbis:LinkButton ID="expressCheckout" runat="server" meta:resourcekey="ExpressCheckout"></Corbis:LinkButton>
                <Corbis:LinkButton ID="addToCart" runat="server"  CommandArgument='<%# Eval("MediaUid") %>'
                    OnCommand="AddToCart_Click" meta:resourcekey="AddToCart"></Corbis:LinkButton>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        <%--the action is the same as "My Lightboxes" link action--%>
        <%if (isOverMax)
          {%>
        <Corbis:LinkButton Visible="true" runat="server" CssClass="viewAll" ID="moreThan50" OnClientClick="CorbisUI.Auth.Check(1,CorbisUI.Auth.ActionTypes.ReturnUrl,'/Lightboxes/MyLightboxes.aspx')" meta:resourcekey="moreThan50" />
        <%} %>
    </FooterTemplate>
</Corbis:Repeater>
