<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="Corbis.Web.UI.Navigation.Footer" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div id="LargeFooter" runat="server">
    <div id="FooterContent" class="gray zeroResults">
        <div id="Left">
            <div class="Column">
                <div><span class="Header"><Corbis:Localize ID="relatedSites" runat="server" meta:resourcekey="relatedSites" /></span></div>
                <div><Corbis:HyperLink ID="corbisMotion" runat="server" NavigateUrl="http://www.corbismotion.com/" meta:resourcekey="corbisMotion" Target="_blank"/></div>
                <div><Corbis:HyperLink ID="GreenLight" runat="server" meta:resourcekey="greenLight" Target="_blank"/></div>
                <div><Corbis:HyperLink ID="mobile" runat="server" NavigateUrl="http://mobile.corbis.com/default.aspx?linkid=150000" meta:resourcekey="mobile" Target="_blank"/></div>
            </div>
            <div class="Column">
                <div><span class="Header"><Corbis:Localize ID="corporate" runat="server" meta:resourcekey="corporate" /></span></div>
                <div><Corbis:HyperLink ID="aboutCorbis" Target="_blank" runat="server" meta:resourcekey="aboutCorbis" /></div>
                <div><Corbis:HyperLink ID="pressroom" Target="_blank" runat="server" meta:resourcekey="pressroom"/></div>
                <div><Corbis:HyperLink ID="employment" Target="_blank" runat="server" meta:resourcekey="employment" /></div>
                <div><Corbis:HyperLink ID="customerService" runat="server" meta:resourcekey="customerService" /></div>
            </div>
            <div class="Column">
                <div><span class="Header"><Corbis:Localize ID="legal" runat="server" meta:resourcekey="legal" /></span></div>
                <div id="imprintDiv" runat="server"><a href="javascript:CorbisUI.Legal.OpenImprintIModal();"><Corbis:Localize ID="imprint" runat="server" meta:resourcekey="Imprint"  Visible="false"/></a></div>
                <div><a href="javascript:CorbisUI.Legal.OpenSAMIModal();"><Corbis:Localize ID="siteUsageAgreementOpen" runat="server" meta:resourcekey="siteUsageAgreement" /></a></div>
                <div><a href="javascript:CorbisUI.Legal.OpenPolicyIModal();"><Corbis:Localize ID="privacyPolicyLinkOpen" runat="server" meta:resourcekey="privacyPolicyLink" /></a></div>
                <div class="LicenseInformation">
                    <Corbis:HyperLink Resizable="true" ID="licenseInformation" ChildWindow="true" ChildWindowHeight="800" ChildWindowWidth="700" runat="server" meta:resourcekey="licenseInformation" /> 
                        <div class="Reader" id="AcrobatReader">
                                <img onclick="javascript:CorbisUI.Legal.OpenAcrobatReaderPopup('AcrobatReader')" src="/Images/info.gif" meta:resourcekey="downloadAdobeReader" runat="server" />
                        </div>
                 </div>
            </div>
        </div>
        <div id="Right">
            <Corbis:HyperLink onclick="javascript:CorbisUI.Legal.OpenRIRModal();return false;" CssClass="Header" ID="requestImageResearch" runat="server" meta:resourcekey="requestImageResearch" />
            <hr />
            <div class="Copyright"><Corbis:Localize ID="copyright" runat="server" /></div>
        </div>
    </div>
</div>
<div id="SmallFooter" runat="server">
    <div class="Copyright"><Corbis:Localize ID="copyright2" runat="server" /></div>
</div>

<%--SITE USAGE AGREEMENT IFRAME MODAL--%>

<div id="SiteUsageAgreementModal" runat="server" style="display:none;">
    
</div>

<%--PRIVACY POLICY IFRAME MODAL--%>

<div id="PrivacyPolicyModal" runat="server" style="display:none;">
    
</div>

<%--IMPRINT IFRAME MODAL--%>

<div id="ImprintModal" runat="server" style="display:none;">
    
</div>

<%--REQUEST IMAGE RESEARCH IFRAME MODAL--%>

<Corbis:ModalPopup ID="ModalPopup1" ContainerID="getAcrobat" runat="server" Width="300" meta:resourcekey="getAcrobat">
    
    <Corbis:Label ID="getReader" runat="server" meta:resourcekey="getReader"></Corbis:Label>
    
</Corbis:ModalPopup>

<Corbis:ModalPopup 
    ID="RIRModal" ContainerID="RIRModalContainer" 
    runat="server" Width="350" Title="Request Image Research" UseDefaultPadding="false"
    >
</Corbis:ModalPopup>

<%-- RIR Success Modal Popup --%>
    <Corbis:ModalPopup ID="ModalSuccess" ContainerID="success" runat="server" Width="350" meta:resourcekey="successModalHeading">
        <Corbis:Label ID="successModalContent" runat="server" meta:resourcekey="successModalContent"></Corbis:Label>
        <Corbis:GlassButton ID="GlassButton2" runat="server" CausesValidation="false" meta:resourcekey="close" OnClientClick="HideModal('success');return false;" />  
    </Corbis:ModalPopup>
    




