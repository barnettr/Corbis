<%@ Control Language="C#" AutoEventWireup="true" Codebehind="QuickPicItems.ascx.cs"
    Inherits="Corbis.Web.UI.Search.QuickPicItems" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<Corbis:Repeater runat="server" ID="searchBuddyQuickPic" OnItemDataBound="searchBuddyQuickPic_ItemDataBound">
    <ItemTemplate>
        <div id="lboxBlock" class="quickPicBlock" runat="server">
            <Corbis:HoverButton runat="server" ID="btnClose" CssClass="hoverBtn closeIcon"  >
            </Corbis:HoverButton>
            <Corbis:CenteredImageContainer ImgUrl='<%# ((String)DataBinder.Eval(Container.DataItem, "Url128")) %>'
                Size="90" IsAbsolute="true" runat="server" ImageID="image" Ratio='<%#DataBinder.Eval(Container.DataItem, "AspectRatio") %>'
                ID="imageThumb" AltText='<%# String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "CorbisId"), DataBinder.Eval(Container.DataItem, "Title")) %>' />
            <div class='<%# Eval("LicenseModel")+"color infoBox" %>'>
                <div class="license">
                    <span>
                        <Corbis:Label ID="licenseModel" runat="server" />
                    </span>
                </div>
            </div>
        </div>
    </ItemTemplate>
</Corbis:Repeater>
<div class="centerMe" id="centerMe" runat="server" visible="true">
	<Corbis:Localize ID="emptyQuickpicView" runat="server" meta:resourcekey="emptyQuickPicMessage"
		Visible="true" />
	<Corbis:HyperLink ID="signInFromQuickPic" runat="server" meta:resourcekey="signIn" NavigateUrl="../Default.aspx"
		Visible="false" />
</div>
<br />
<br />
<br />
