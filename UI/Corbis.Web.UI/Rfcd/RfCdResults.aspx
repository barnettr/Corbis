<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RfCdResults.aspx.cs" Inherits="Corbis.Web.UI.RfCd.RfCdResults" MasterPageFile="~/MasterPages/MasterBase.Master" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Products" Src="~/Rfcd/RfcdProducts.ascx" %>


<asp:Content ID="rfCdResultsContent" runat="server" ContentPlaceHolderID="mainContent">
    <div>
        <h2><Corbis:Localize id="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
        
        <div>
  <div>
    <Corbis:Label ID="cdNameLabel" meta:resourcekey="cdNameLabel" runat="server"></Corbis:Label>
    <Corbis:Label ID="cdName" runat="server"></Corbis:Label>
  </div>
  <div>
    <Corbis:Label ID="numberOfImages" runat="server"></Corbis:Label> <Corbis:Label ID="imagesLabel" meta:resourcekey="imagesLabel" runat="server"></Corbis:Label>, 
    <Corbis:Label ID="rfcdID" runat="server"></Corbis:Label>
    <Corbis:HyperLink ID="imagesToLightbox" meta:resourcekey="imagesToLightbox" runat="server"></Corbis:HyperLink> |
    <Corbis:HyperLink ID="cdToLightbox" meta:resourcekey="cdToLightbox" runat="server"></Corbis:HyperLink> |
    <Corbis:LinkButton ID="cdToCart" meta:resourcekey="cdToCart" OnCommand="AddToCartClick" runat="server"></Corbis:LinkButton>
  </div>
</div>
<div class="RfcdResultsLeftNav">
    <div>
        <Corbis:Image ID="rfcdImage" IsAbsolute="true" runat="server" />
    </div>
    <div>
        <Corbis:Label ID="imagePrice" runat="server"></Corbis:Label>
        <Corbis:Label ID="imageSize" runat="server"></Corbis:Label><br />
        <Corbis:Label ID="rfcdID2" runat="server"></Corbis:Label>
        <Corbis:Label ID="rf" meta:resourcekey="rf" runat="server"></Corbis:Label>
        <Corbis:Label ID="numberOfImages2" runat="server"></Corbis:Label>  <Corbis:Label ID="imagesLabel2" meta:resourcekey="imagesLabel" runat="server"></Corbis:Label><br />
        &copy; <Corbis:Label ID="copyright" runat="server"></Corbis:Label><br />
        <Corbis:HyperLink ID="cdToLightbox2" meta:resourcekey="cdToLightbox" runat="server"></Corbis:HyperLink><br />
        <Corbis:LinkButton ID="cdToCart2" meta:resourcekey="cdToCart" OnCommand="AddToCartClick" runat="server"></Corbis:LinkButton>
    </div>
    <div>
        <Corbis:Label ID="rfcdText" meta:resourcekey="rfcdText" runat="server"></Corbis:Label>
    </div>
    <div>
        <Corbis:Label ID="interestedText" meta:resourcekey="interestedText" runat="server"></Corbis:Label><br />
        <Corbis:Repeater ID="interestedRepeater" OnItemDataBound="InterestedRepeater_ItemDataBound" runat="server" >
            <ItemTemplate>
                <ContentTemplate>
                    <Corbis:HyperLink id="categoryLink" Text='<%# Eval("Title") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VolumeNumber") %>' runat="server" />
                    <Corbis:Label Id="imageCount" runat="server" Text='<%# Eval("ImageCount") %>'></Corbis:Label>  <Corbis:Label ID="imagesLabel3" meta:resourcekey="imagesLabel" runat="server"></Corbis:Label>
                    <br />
                </ContentTemplate>
            </ItemTemplate>
        </Corbis:Repeater>
        <Corbis:HyperLink ID="browseRFCD" meta:resourcekey="browseRFCD" runat="server"></Corbis:HyperLink>
    </div>
</div>
<div class="RfcdResultsProducts">
    <Corbis:Products id="products" runat="server" />
</div>
    </div>
</asp:Content>
