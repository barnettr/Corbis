<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="RequestPrice.aspx.cs" Inherits="Corbis.Web.UI.Pricing.RequestPrice" MasterPageFile="~/MasterPages/ModalPopup.Master"%>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="requestPrice" ContentPlaceHolderID="mainContent" runat=server>
<div id="validationContainer">
<div>
    <%-- Contact Us/Request Price Header--%>
    <div id="titleWrapper" class="titleWrapper">
        <Corbis:Localize ID="contactUsTitle" runat="server" meta:resourcekey="contactUsTitle" />
        <div class="samCloseButton" id="ambiguousCloseButton">
            <Corbis:Image ID="contactUsForPriceCloseImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="CorbisUI.Pricing.ContactUs.HideRequestForm();return false;" class="Close" meta:resourcekey="contactUsForPriceCloseImage"/>
        </div>
    </div>
                    
    <%-- Request Price content --%>
    <div class="GetInTouch contentWrapper">
        <div id="getInTouchPaneHeader" class="contactUsDesc">
            <div class="Right">
                <div class="Center">
                    <Corbis:Localize ID="contactUsDescription" runat="server" meta:resourcekey="contactUsDescription" />
                </div>
            </div>
        </div>
        <div class="PaneContent">
            <Corbis:ValidationHub 
                ID="vhub" runat="server" IsPopup="true" IsIModal="true" 
                PopupID="requestPriceModalPopup" SubmitForm="true"
                ContainerID="validationContainer" InitScript="initProvinces()"
            />
            <table cellspacing="0" id="formTable">
            <tr id="firstNameDefaultDiv" runat="server" class="FormRow">
                <td class="FormLeft">
                     <Corbis:Localize runat="server" meta:resourceKey="firstnameLabel"/>
                </td>
                <td class="FormRight">
                   <Corbis:TextBox validate="required" meta:resourcekey="firstName" MaxLength="40" ID="firstName" runat="server"  />
                </td>
            </tr>
            <tr id="lastNameDefaultDiv" runat="server" class="FormRow">
                <td class="FormLeft">
                    <Corbis:Localize runat="server" meta:resourcekey="lastNameLabel"/>
                </td>
                <td class="FormRight">
                    <Corbis:TextBox validate="required" meta:resourcekey="lastName" MaxLength="40" ID="lastName" runat="server" />
                </td>
            </tr>
            <tr id="lastNameAsianDiv" runat="server" class="FormRow">
                <td class="FormLeft">
                    <Corbis:Localize ID="lastNameAsianLabel" runat="server" meta:resourcekey="lastNameLabel"></Corbis:Localize>
                </td>
                <td class="FormRight">
                    <Corbis:TextBox validate="required" ID="lastNameAsian" MaxLength="80" meta:resourcekey="lastName" runat="server" />
                </td>
            </tr>
            <tr id="firstNameAsianDiv" runat="server" class="FormRow">
                <td class="FormLeft">
                     <Corbis:Localize ID="firstnameAsianLabel" runat="server" meta:resourceKey="firstnameLabel"></Corbis:Localize>
                </td>
                <td class="FormRight">
                   <Corbis:TextBox validate="required" ID="firstNameAsian" MaxLength="80" meta:resourcekey="firstName" runat="server"  />
                </td>
            </tr>
            <tr class="FormRow">
                 <td class="FormLeft">
                    <Corbis:Localize runat="server"  meta:resourcekey="emailLabel"/>
                </td>
                <td class="FormRight">
                    <Corbis:TextBox validate="required;email" meta:resourcekey="email" MaxLength="100" ID="email" runat="server" /> 
                </td>
            </tr>
            <tr class="FormRow">
                <td class="FormLeft">
                     <Corbis:Localize runat="server"  meta:resourcekey="telephoneLabel"/>
                </td>
                <td class="FormRight">
                    <Corbis:TextBox validate="required;phone" ID="phone" runat="server" MaxLength="30" meta:resourcekey="phone" />
                </td>
            </tr>
            <tr class="FormRow">
                <td class="FormLeft"><Corbis:Localize runat="server" meta:resourcekey="countryLabel" /></td>
                <td class="FormRight">
                    <Corbis:DropDownList ID="countryList" validate="custom1" custom1="_provinceBehavior.validateCountry(true)" meta:resourcekey="country" runat="server" />
                </td>
            </tr>
            <tr class="FormRow">
                <td class="FormLeft"><Corbis:Localize runat="server" meta:resourcekey="stateLabel" /></td>
                <td class="FormRight">
                    <Corbis:DropDownList Enabled="false" validate="custom1" custom1="_provinceBehavior.validateProvince(true)" meta:resourcekey="stateList" ID="stateList" runat="server"/>
                    <asp:HiddenField ID="provinceName" runat="server" />
                    <asp:HiddenField ID="provinceCode" runat="server" />
                </td>
            </tr>
            <tr class="FormRow">
                <td class="FormLeft clear commentsSpacing">
                    <Corbis:Localize runat="server" meta:resourcekey="commentsLabel" />
                </td>
                <td class="FormRight">
                    <Corbis:TextArea ID="comments" validate="required" name="comments" runat="server" TextMode="MultiLine" MaxLength="255"  style="height: 60px; overflow-y:scroll;" meta:resourcekey="comments" />
                </td>
            </tr>
            </table>
        </div>
    </div>
</div>

<%-- Single/Multiple images product control --%>
<div class="ContactUSimages" id="ContactUSimages">
<Corbis:Repeater ID="results" runat="server" OnItemDataBound="Result_ItemDataBound">
    <ItemTemplate>
    <div id="ProductBlock" class="ProductBlock <%# Eval("LicenseModel")%>">
        <div id="productTitle" class="productTitle" ><%# Eval("CorbisId") %></div>
         <span class="product" id="product" runat="server">
            <Corbis:CenteredImageContainer ID="thumbWrap" IsAbsolute="true" runat="server" ImageID="image" ImgUrl='<%# Eval("Url128").ToString().Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=0&im=") %>' 
            Size="128" Ratio='<%# Eval("AspectRatio") %>'/>	
         </span>
        <div class="controlWrap">
        <%--<div class="active"><span>RM</span></div>--%>
            <div class="CTL_Item ACTV_Details LT active hasTooltip" title="<%# Corbis.Web.UI.CorbisBasePage.GetKeyedEnumDisplayText<Corbis.CommonSchema.Contracts.V1.LicenseModel>((Corbis.CommonSchema.Contracts.V1.LicenseModel)Eval("LicenseModel"), "LongText") %>">
			    <span><%# Corbis.Web.UI.CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.LicenseModel>((Corbis.CommonSchema.Contracts.V1.LicenseModel)Eval("LicenseModel"))%></span>
		    </div>
        </div>
    </div>			
    </ItemTemplate>
</Corbis:Repeater>    
</div>   
                 
<%-- Send/cancel buttons --%>
<div class="PaneContentRound rc5px clear MB_10">
    <div class="Bottom">
        <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
    </div>
</div>
<div>
    <div>
        <a class="privacyPolicy" href="javascript:parent.CorbisUI.Legal.OpenPolicyIModal();"><Corbis:Localize ID="privacyPolicyLinkOpen" runat="server" meta:resourcekey="privacyPolicyLabel" /></a>
        
        <%--<Corbis:LinkButton ID="privacyPolicy" CssClass="privacyPolicy" runat="server" OnClientClick="" meta:resourcekey="privacyPolicyLabel">                            
        </Corbis:LinkButton>--%>
    </div>
    <div class="alignRight buttonPadding">
        <Corbis:GlassButton ID="save" runat="server" 
            meta:resourcekey="Save" validate="submit" CssClass="buttonsSpacing"
        />
        <Corbis:LinkButton ID="lb" runat="server" CssClass="ValidateClickLB displayNone" OnClick="Save_Click" />
        <Corbis:GlassButton ButtonStyle="Gray" ID="cancel" 
            CausesValidation="false" meta:resourcekey="Cancel" CssClass="buttonsSpacing"
            runat="server" OnClientClick="javascript:CorbisUI.Pricing.ContactUs.HideRequestForm();return false;" 
        />    
    </div>                    
</div>
<div class="PaneContentRound rc5px clear MB_10">
    <div class="Bottom">
        <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
    </div>
</div>
<Corbis:ModalPopup ID="ModalPopup1" ContainerID="getThankYou" runat="server" Width="350" meta:resourcekey="openThankYouPopup">
    
    <Corbis:Label ID="getThankyouContent" runat="server" meta:resourcekey="getThankyouContent"/>
    <br />
    <Corbis:GlassButton ID="CloseButton" runat="server" CausesValidation="false" Text="Close" OnClientClick="HideModal('OpenThankYouPopup');CorbisUI.Pricing.ContactUs.HideRequestForm();return false;" />  

</Corbis:ModalPopup>
<Corbis:ContactCorbis runat="server" />
<script type="text/javascript">

    var _countries, _statesEl;
    function initProvinces() {
        _countries = $('<%=countryList.ClientID %>');
        _statesEl = $('<%=stateList.ClientID %>');
        _provinceBehavior = new CorbisUI.FormUtilities.ProvinceBehavior({
            countriesDropdownId: '<%=countryList.ClientID %>',
            provinceDropdownId: '<%=stateList.ClientID %>',
            provinceNameHdnId: '<%=provinceName.ClientID %>',
            provinceCodeHdnId: '<%=provinceCode.ClientID %>',
            ajaxUrl: '/Common/CommonWebService.asmx/GetStates',
            topOptionText: '<%=this.SelectOne %>',
            validationClass: <%=vhub.ClientInstanceVariableName %>
        });
    }
</script>
</div>

</asp:Content>
