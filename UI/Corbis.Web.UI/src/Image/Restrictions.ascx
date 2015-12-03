<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Restrictions.ascx.cs" Inherits="Corbis.Web.UI.Image.Restrictions" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<asp:Panel ID="popupMode" runat="server">
    <div id="popupRestrictionsContent">
        <Corbis:ModalPopup ContainerID="restrictionsPopup" runat="server" Width="600" ID="restrictionsModal"
            CloseScript="MochaUI.HideModal('restrictionsPopup'); return false;" meta:resourceKey="restrictionsModal">
        
            <div id="base" class="base">
                <div class="contentContainer">
                    <div class="">
                        <Corbis:Label ID="popupModelReleaseLabel" CssClass="releaseLabel" runat="server" meta:resourceKey="modelReleaseLabel" />&nbsp;<Corbis:Label ID="popupModelReleaseText" CssClass="ModelReleaseLabelNoBold" runat="server" />
                    </div>
                    <div>
                        <Corbis:Label ID="popupPropertyReleaseLabel" CssClass="releaseLabel" runat="server" meta:resourceKey="propertyReleaseLabel" />&nbsp;<Corbis:Label ID="popupPropertyReleaseText" CssClass="PropertyReleaseLabelNoBold" runat="server" />
                    </div>
                    <asp:BulletedList ID="popupRestrictionsList" CssClass="RestrictionsList" runat="server" />
                    <div id="popupDomesticEmbargo" runat="server">
                        <Corbis:Label ID="popupDomesticEmbargoLabel" runat="server" meta:resourceKey="domesticEmbargoLabel" />&nbsp;<Corbis:Label ID="popupDomesticEmbargoDate" runat="server" />
                    </div>
                    <div id="popupInternationalEmbargo" runat="server">
                        <Corbis:Label ID="popupInternationalEmbargoLabel" runat="server" meta:resourceKey="internationalEmbargoLabel" />&nbsp;<Corbis:Label ID="popupInternationalEmbargoDate" runat="server" />
                    </div>
                </div>
            </div>
     
        </Corbis:ModalPopup>
    </div>
    <div id="popupRestrictionsHeader">
        <ul>
            <li class="RestrictionTitleList">
                <img src="../../Images/alertYellow.gif" style="vertical-align: middle; margin-bottom: 3px" /></li>
            <li class="RestrictionTitleList">
                <a id="viewRestrictions" href="javascript:showRestrictions();">
                <Corbis:Label ID="viewRestrictionsLabel" meta:resourcekey="viewRestrictionsText"
                    runat="server" /></a></li>
            <li class="PricingIcon">
                <asp:Image ID="popupPricingIcon" runat="server" /></li>
        </ul>
        <br />
    </div>

    <script type="text/javascript">
        function showRestrictions() {
            new CorbisUI.Popup('restrictionsPopup', {
                showModalBackground: false,
                closeOnLoseFocus: true,
                positionVert: 16,
                positionHoriz: -5
            });
            ResizeModal('restrictionsPopup');
        }
    </script>

</asp:Panel>
<asp:Panel ID="inlineMode" runat="server">
    <div id="inlineRestrictionsContent" class="restrictionsContent">
        <div runat="server" id="headerDiv" class="inlineMode">
            <ul>
                <li class="RestrictionTitleList">
                    <img src="../../Images/alertYellow.gif" style="margin-bottom: 2px" /></li>
                <li class="RestrictionTitleList" style="margin-left:0px;">
                    <Corbis:Label CssClass="FS_14" ID="headerLabel" runat="server" meta:resourceKey="headerLabel" /></li>
                <li class="PricingIcon RestrictionTitleList" >
                    <asp:Image ID="pricingIcon" runat="server" /></li>
            </ul>
        </div>
         <div class = "restrictionValues">
        <div class = "modelProperties">
        <div class="">
            <Corbis:Label ID="modelReleaseLabel" CssClass="releaseLabel bold" runat="server" meta:resourceKey="modelReleaseLabel" />&nbsp;<Corbis:Label
                ID="modelReleaseText" runat="server" />
        </div>
        <div>
            <Corbis:Label ID="propertyReleaseLabel" CssClass="releaseLabel bold" runat="server" meta:resourceKey="propertyReleaseLabel" />&nbsp;<Corbis:Label
                ID="propertyReleaseText" runat="server" />
        </div>
        </div>
        <asp:BulletedList ID="restrictionsList" CssClass="RestrictionsList" runat="server" />
        <div id="domesticEmbargo" runat="server">
            <Corbis:Label ID="domesticEmbargoLabel" CssClass="releaseLabel" runat="server" meta:resourceKey="domesticEmbargoLabel" />&nbsp;<Corbis:Label
                ID="domesticEmbargoDate" runat="server" />
        </div>
        <div id="internationalEmbargo" runat="server">
            <Corbis:Label ID="internationalEmbargoLabel" CssClass="releaseLabel" runat="server"
                meta:resourceKey="internationalEmbargoLabel" />&nbsp;<Corbis:Label ID="internationalEmbargoDate"
                    runat="server" />
        </div>
        </div>
    </div>
</asp:Panel>
