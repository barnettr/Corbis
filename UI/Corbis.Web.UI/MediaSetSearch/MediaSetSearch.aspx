<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MediaSetSearch.aspx.cs" Inherits="Corbis.Web.UI.MediaSetSearch.MediaSetSearch"
    EnableEventValidation="false" MasterPageFile="~/MasterPages/MasterBase.Master" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="MediaSetsProducts" Src="MediaSetsProducts.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultHeader" Src="~/Search/SearchResultHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultFooter" Src="~/Search/SearchResultFooter.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="CreateLightbox" Src="~/UserControls/LightboxControls/CreateLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="LightboxItems" Src="~/Search/LightboxItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicItems" Src="~/Search/QuickPicItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>

<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="searchResultsContent" runat="server" ContentPlaceHolderID="mainContent">
	<!-- $$$ [ new search buddy starts here ]-->
	<div id="contentBar">
		<div class="wrap searchContent">
			<div id="SearchColumnedContent" class="columnLayout twoColumn ">
				<div class="leftColumnWrap">
					<div class="rightColumnWrap">
						<div class="MediaSetsProducts rightColumn">
							<!-- SEARCH RESULTS CONTENT start -->
							<div class="searchResultsContent">
								<!--h2><Corbis:Localize id="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2-->
								<asp:Panel ID="ClarificationPanel" Visible="false" runat="server">
									<Corbis:Localize ID="DidYouMean" runat="server" meta:resourcekey="DidYouMean" />&nbsp;
								</asp:Panel>
								<asp:Panel runat="server" ID="HeaderPanel" />
							 </div>
							<Corbis:SearchResultHeader ID="searchResultHeader" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand" ShowHeader="true" OnPageSizeCommand="searchResultHeader_PageSizeCommand" />
							<div id="zeroSearchResultsWrapper" class="zeroSearchResults" runat="server" visible="false">
                                <div id="zeroSearchResultsPadding" class="zeroSearchResultsPadding">
                                 <Corbis:Label ID="Label1" CssClass="zeroTitle" runat="server" meta:resourcekey="imageGroupZeroresults" /><br /><br />
                                 <Corbis:GlassButton ID="PreviousButton" runat="server" ButtonBackground="gray36" Visible="false" OnClientClick="javascript:history.go(-1);return false;"  meta:resourcekey="previousButton" /> 
                                </div>
                            </div>
							<Corbis:MediaSetsProducts  ID="products" runat="server" />
					        <Corbis:SearchResultFooter ID="searchResultFooter" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand">
						    </Corbis:SearchResultFooter>
							
							<!-- SEARCH RESULTS CONTENT end -->
						</div>
					</div>
					<div class="leftColumn">
						<!-- SEARCH BUDDY start -->
						<div style="top: 205px" class="SearchBuddy" id="SearchBuddy">
							<div class="inSearchBuddy">
								<ul class="SB_tabs">
									<li id="SBT_filters" class="SBT_lightboxes ON">
										<img src="../Images/spacer.gif" alt="" /></li>
									<li id="SBT_quickpic" class="SBT_quickpic" runat="server">
										<img src="../Images/spacer.gif" alt="" /></li>
								</ul>
								
								<div id="SBBX_filters" class="SBBX_filters">
									<!-- $$$ start of lightbox -->
									<div id="lightboxHeaderDiv">
										<span>&nbsp</span>
										<asp:UpdateProgress ID="lightboxUpdateProgress" runat="server" AssociatedUpdatePanelID="detailViewUpdatelightbox">
											<ProgressTemplate>
												<div class="lightboxWrap">
													<img border="0" alt="" src="/images/buddyLoading.gif" />
												</div>
											</ProgressTemplate>
											
										</asp:UpdateProgress>
									</div>
									<div id="lightboxHeader">
										<a href="../Lightboxes/MyLightboxes.aspx">
											<Corbis:Localize ID="createLightboxTitle" runat="server" meta:resourcekey="createLightboxTitle" />
											<img id="goToLightbox" src="../Images/icon-LB-small.gif" class="ML_5" runat="server" alt="" meta:resourcekey="goToLightbox" />
									    </a>
									</div>
									<asp:UpdatePanel ID="detailViewUpdatelightbox" UpdateMode="Conditional" runat="server">
										<ContentTemplate>
											<asp:DropDownList ID="lightboxList" class="lightboxList" runat="server" AutoPostBack="true"
												OnSelectedIndexChanged="lightboxList_SelectedIndexChanged" />
											<input type="button" id="hiddenRefresh" class="hdn" runat="server" onserverclick="hiddenRefresh_OnChange" />
											<div class="CreateLBXPositioner">
												<div class="BTN-orange-1a1a1a BTN-orange" onclick="">
													<div class="right">
														<div class="center">
															<asp:Panel runat="server" ID="pnlCreateLightBox">
																<Corbis:LinkButton ID="createNew" OnClientClick="javascript:openCreateLightbox();return false;"
																	runat="server" meta:resourcekey="createNew" />
															</asp:Panel>
														</div>
													</div>
												</div>
											</div>
											<div id="LBXContainer" class="LBXContainer">
												<!-- lightbox control -->
												<Corbis:LightboxItems ID="lightboxItems" runat="server" EnableViewState="true">
												</Corbis:LightboxItems>
												<!-- end lightbox control -->
												<div class="centerMe">
													
													<Corbis:HyperLink ID="signIn" runat="server" meta:resourcekey="signIn" NavigateUrl="../Default.aspx"
														Visible="false" />
												</div>
												<br />
												<br />
												<br />
											</div>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="hiddenRefresh"/>
											<asp:AsyncPostBackTrigger ControlID="createLightbox" />
										</Triggers>
									</asp:UpdatePanel>
									<!-- $$$ end of lightbox -->
								</div>
								<div id="SBBX_quickpic" class="SBBX_quickpic hdn">
									<div id="quickPicHeaderDiv">
										<span>&nbsp;</span>
										<asp:UpdateProgress ID="QuickPicUpdateProgress" runat="server" AssociatedUpdatePanelID="QuickPicUpdatePanel">
											<ProgressTemplate>
												<div class="lightboxWrap">
													<img border="0" alt="" src="/images/buddyLoading.gif" />
												</div>
											</ProgressTemplate>
										</asp:UpdateProgress>
									
									</div>
									<div id="quickPicHeader">
										Quick Pic
									</div>
									<div class="QPDownloadPositioner">
										<span class="actLikeLink" onclick="CorbisUI.Handlers.Quickpic.downloadAll();"><Corbis:Localize ID="downloadAll" runat="server" meta:resourcekey="downloadAll" /></span>
									</div>
									<!-- $$$ thumbnails start here -->
									<div id="quickPicsContainer" class="quickPicsContainer">
										
										<asp:UpdatePanel ID="QuickPicUpdatePanel" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<!-- QuickPic control -->
												<input type="button" id="quickpicField" class="hdn" runat="server" onserverclick="updateQuickPick" />
												<Corbis:QuickPicItems ID="quickPicItems" runat="server" EnableViewState="true"></Corbis:QuickPicItems>
												<!-- end QuickPic control -->
											
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="quickpicField" />
											</Triggers>
										</asp:UpdatePanel>
									</div>
									
								</div>
								<div class="searchBuddyFooter">
										&nbsp;</div>
							</div>
							<!-- SEARCH BUDDY end -->
						</div>
					</div>
				</div>
			</div>
		</div>
		<Corbis:CreateLightbox ID="createLightbox" runat="server"/>
        <div style="width:222px;height:100%;background:transparent;position:absolute;top:180px;display:none;" id="searchBuddyMask"></div>
        
		
		
		<asp:HiddenField ID="zeroSearchTrue" runat="server" Value="false" />
		<script language="javascript" type="text/javascript">
	    CorbisUI.GlobalVars.SearchResults = {
			refreshLightboxControl: '<%= hiddenRefresh.ClientID %>',
		    isAnonymous : <%= (Corbis.Web.Authentication.Profile.Current.IsAnonymous? "true":"false") %>,
		    urls: {
		        downloadQuickPic: "<%= Corbis.Web.UI.SiteUrls.QuickPic %>"
		    }
		};
        // see Search.js for endRequestHandler
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        //Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);
        
        // *****************************************
        // Need to grab server side variable "cultureName",
        // so need to have this function on this page.
        // *****************************************
        function getSearchTipsWindow() {
            var cultureName = '<%=Corbis.Framework.Globalization.Language.CurrentCulture.Name%>';
            tipsWindow = window.open("/Creative/SearchTips/Page.aspx","tipsWindow", "left=0,top=0,width=1009,height=633,toolbar=0,resizable=0,scrollbars=0,menubar=0");
            tipsWindow.moveTo(0, 0);
        }
       
        </script>
	</div>
</asp:Content>
