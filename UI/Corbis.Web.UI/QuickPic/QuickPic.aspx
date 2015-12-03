<%@ Page Language="C#" MasterPageFile="~/MasterPages/ModalPopup.Master" AutoEventWireup="true" CodeBehind="QuickPic.aspx.cs" Inherits="Corbis.Web.UI.QuickPic.QuickPic" Title="<%$ Resources: pageTitle.Text %>" EnableViewState="false" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicDownload" Src="~/QuickPic/QuickPicDownload.ascx" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="FileSize" Src="~/CommonUserControls/FileSizes.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="quickPicContent" ContentPlaceHolderID="mainContent" runat="server">
	<div id="quickPicTitleBar" class="TitleBar">
		<div class="quickPicTitle">
			<Corbis:Localize ID="pageTitle" meta:resourcekey="pageTitle" runat="server"/>
		</div>
		<div class="closeButton">
			<a href="javascript:parent.CloseModal('QuickPic');">
				<img alt="" id="XClose" src="/Images/iconClose.gif" />
			</a>
		</div>
		 <div  class="ChatDiv" meta:resourcekey="chatDiv" runat="server">
			     <UC:InstantService id="instantService1" runat="server" />
		    </div>
	</div>
	<div id="quickPicBody">
		<div id="selectImage">
        <Corbis:ValidationHub 
            ID="quickPicValidationHub" runat="server" IsPopup="false"
            ContainerID="quickPicBody" UniqueName="QuickPic"
            SuccessScript="CorbisUI.QuickPic.DownloadQuickPicImages();"
            SubmitByAjax="true" AjaxUrl="/Checkout/CheckoutService.asmx/ValidateProjectEncoding"
            OnAjaxRequest="SetQuickPicAjaxData" UseStandardAjaxBehavior="true"
        />
        <script type="text/javascript">
            function SetQuickPicAjaxData() {
                <%=quickPicValidationHub.ClientInstanceVariableName %>.options.ajaxData = {
                    'projectName' : $('<%=projectName.ClientID %>').value,
                    'projectNameClientId' : '<%=projectName.ClientID %>',
                    'jobNumber' : '',
                    'jobNumberClientId' : 'foo',
                    'poNumber' : '',
                    'poNumberClientId' : 'foo',
                    'licensee' : '',
                    'licenseeClientId' : 'foo'
                };
            }
        </script>
		
		
			<div class="quickPicText">
				<Corbis:Localize ID="quickPicText" meta:resourcekey="quickPicText" runat="server"/>
			</div>
			<div class="projectName"> 
				<Corbis:Label ID="projectNamelabel" meta:resourcekey="projectNamelabel" runat="server" /><br />
				<Corbis:TextBox ID="projectName" runat="server" />
			</div>
			<div class="downloadwrap rounded4">
				<div class="download">
					<span class="resolutionTitle">
						<Corbis:Localize ID="resolutionTitle" meta:resourcekey="resolutionTitle" runat="server"/>
					</span><br />
					<span class="resolutionText">
						<Corbis:Localize ID="resolutionText" meta:resourcekey="resolutionText" runat="server"/>
					</span><br />
					<a href="javascript:void(0);" onclick="new CorbisUI.Popup('fileSizeModal', {createFromHTML: false, showModalBackground: false, closeOnLoseFocus: true, centerOverElement: $('quickPicContent'), positionVert: 'center', positionHoriz: 'middle'}); return false;">
						<Corbis:Localize ID="fileSizeQuestions" meta:resourcekey="fileSizeQuestions" runat="server"/>
					</a><br /><br />
					<span class="downloadSelect">
						<Corbis:Localize ID="downloadLabel" meta:resourcekey="downloadLabel" runat="server"/>&nbsp;
						<a href="javascript:void(0);" onclick="CorbisUI.QuickPic.SetFileSize('smallest'); return false;">
						<Corbis:Localize ID="lowestResolution" meta:resourcekey="lowestResolution" runat="server"/></a>&nbsp;/&nbsp;
						<a href="javascript:void(0);" onclick="CorbisUI.QuickPic.SetFileSize('largest'); return false;">
						<Corbis:Localize ID="highestResolution" meta:resourcekey="highestResolution" runat="server"/></a>
					</span>
					<div class="quickPicImages">
						<Corbis:QuickPicDownload ID="quickPicDownloadItems" runat="server"/>					
					</div>
				</div>
			</div>
			<div class="termsConditionsTitle">
				<Corbis:Localize ID="termsConditionsTitle" runat="server" meta:resourcekey="termsConditionsTitle"/>
			</div>
			<div class="termsConditionAccept">
				<Corbis:ImageCheckbox ID="acceptTermsConditions" runat="server" OnClientChanged="setGlassButtonDisabled($('selectImage').getElement('div.downloadImages'), !this.checked);" />
				<div class="acceptTermsConditionsDiv">
					<Corbis:Localize ID="acceptTermsConditionslabel" runat="server" meta:resourcekey="acceptTermsConditionslabel"/></div>
			</div>
			<div class="buttons">
				<Corbis:GlassButton ID="cancel" runat="server" ButtonBackground="dbdbdb" ButtonStyle="Gray" meta:resourcekey="cancel" OnClientClick="javascript:parent.CloseModal('QuickPic'); return false;" />
				<Corbis:GlassButton ID="downloadImages" CssClass="downloadImages" runat="server" ButtonBackground="dbdbdb" meta:resourcekey="downloadImages" OnClientClick="_QuickPicValidation.validateAll();return false;" Enabled="false" />
			</div>
		</div>
		<div id="download" class="hdn"> 
			<div class="bold"><Corbis:Localize ID="orderNumberLabel" runat="server" meta:resourcekey="orderNumberLabel" />&nbsp;<span id="orderNumber"></span></div>
			<div class="projectName"><span id="projectName"></span></div>
			<div class="orderText"><Corbis:Localize ID="quickPicOrderText" runat="server" meta:resourcekey="quickPicOrderText" EnableViewState="False" ReplaceKey="" 
					ReplaceValue="" />&nbsp;<span id="emailedTo"></span></div>
			<div class="downloadwrap rounded4">
				<div class="download">
					<div class="downloadProgressTitle">
						<Corbis:Localize ID="downloadProgressTitle" runat="server" meta:resourcekey="downloadProgressTitle" EnableViewState="False" ReplaceKey="" 
							ReplaceValue="" />
					</div>
					<div class="downloadProgressText">
						<Corbis:Localize ID="downloadProgressText" runat="server" meta:resourcekey="downloadProgressText" EnableViewState="False" ReplaceKey="" 
							ReplaceValue="" />
					</div>
					<div class="downloadLinks">
					</div>
					<div class="dots"></div>
					<div class="helpText">
						<Corbis:Localize ID="helpText" runat="server" meta:resourcekey="helpText"/>
					</div>
				</div>
			</div>
			<div class="closeButtonWrap">
				<Corbis:GlassButton CssClass="closeButton" ID="close" runat="server" ButtonBackground="dbdbdb" meta:resourcekey="close" OnClientClick="javascript:parent.CloseModal('QuickPic'); return false;" />
			</div>
		</div>
		<div id="contentFooter">
			<div class="contentFooterText">
				<a href="#" onclick="parent.CorbisUI.Legal.OpenSAMIModal(); return false;"><Corbis:Localize ID="siteUsage" meta:resourcekey="siteUsage" runat="server"/></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<a href="#" onclick="parent.CorbisUI.Legal.OpenPolicyIModal(); return false;"><Corbis:Localize ID="privacyPolicy" meta:resourcekey="privacyPolicy" runat="server"/></a><br /><br />
				<Corbis:Localize ID="copyrightMsg" runat="server" />
			</div>
			<a class="etrust" href="#" onclick="parent.CorbisUI.Legal.OpenPolicyIModal(); return false;" id="TRUSTeLink" runat="server" ><img src="/Images/truste.gif" alt="" id="TRUSTe" runat="server" /></a>
		</div>
	</div>
	<div id="quickPicFooter">
	</div>
	<Corbis:FileSize ID="fileSizeModal" runat="server" />
	<%// Modal for downloading images %>
	<div id="downloadProgress" style="display: none;">
		<img border="0" alt="" src="/images/ajax-loader2.gif" />
		<br />
		<div class="standBy"><Corbis:Localize ID="standByMessage" runat="server" meta:resourcekey="standByMessage" /></div>
		<div class="downloadMessage"><Corbis:Localize ID="downloadMessage" runat="server" meta:resourcekey="downloadMessage" /></div>
		
	</div>	
    <Corbis:ModalPopup ID="downloadErrorModal" ContainerID="downloadErrorModal" Width="372" runat="server" CloseScript="HideModal('downloadErrorModal');$(parent.document).getElement('#QuickPicWindow_overlay').setStyle('background-image', 'none');return false;" meta:resourcekey="downloadErrorModal">
		<div class="errorMessage">
			<Corbis:Localize ID="errorMessage" runat="server" meta:resourcekey="errorMessage" />
		</div>
		<div class="contactMessage">
			<Corbis:Localize ID="contactMessage" runat="server" />
		</div>
		<Corbis:GlassButton ID="closeErrorModal" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('downloadErrorModal');$(parent.document).getElement('#QuickPicWindow_overlay').setStyle('background-image', 'none');return false;" CssClass="closeButton" />
    </Corbis:ModalPopup>
    <Corbis:ContactCorbis runat="server" />
	<script type="text/javascript">
		<%// This AJAX stuff does not play well in the js file %>
		Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
		Sys.Net.WebRequestManager.add_completedRequest(onComplete);

		function onInvoke(sender, args)
		{
			if(!args.get_webRequest().get_url().endsWith('GetContactCorbisOffice'))
			{
				CorbisUI.QuickPic.OpenProgressModal(args.get_webRequest().get_url());
			}
		}

		function onComplete(sender, args)
		{
			CorbisUI.QuickPic.HideProgressModal();
		}

		function pageUnload()
		{
			Sys.Net.WebRequestManager.remove_invokingRequest(onInvoke);
			Sys.Net.WebRequestManager.remove_completedRequest(onComplete);
		}
		
		window.addEvent('domReady',$('<%=projectName.ClientID %>').focus());
	</script>	
</asp:Content>
