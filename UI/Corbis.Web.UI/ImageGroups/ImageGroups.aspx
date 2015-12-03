<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ImageGroups.aspx.cs" Inherits="Corbis.Web.UI.ImageGroups.ImageGroups"
    EnableEventValidation="false" MasterPageFile="~/MasterPages/MasterBase.Master" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="aspx" %>
<%@ Register TagPrefix="Corbis" TagName="Search" Src="~/src/Navigation/Search.ascx" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Products" Src="ImageGroupsProducts.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultHeader" Src="~/Search/SearchResultHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultFooter" Src="~/Search/SearchResultFooter.ascx" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToLightbox" Src="~/UserControls/LightboxControls/AddToLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="LightboxItems" Src="~/Search/LightboxItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicItems" Src="~/Search/QuickPicItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="CreateLightbox" Src="~/UserControls/LightboxControls/CreateLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="OutlineHeader" Src="~/ImageGroups/OutlineHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="StorySetHeader" Src="~/ImageGroups/StorySetHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="PromotionalSetHeader" Src="~/ImageGroups/PromotionalSetHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SameModelHeader" Src="~/ImageGroups/SameModelHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AlbumHeader" Src="~/ImageGroups/AlbumHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SamePhotoshootHeader" Src="~/ImageGroups/SamePhotoshootHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RFCDHeader" Src="~/ImageGroups/RFCDHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicMaxAlert" Src="~/UserControls/ModalControls/QuickPicMaxAlert.ascx" %>

<asp:Content ID="searchResultsContent" runat="server" ContentPlaceHolderID="mainContent">
	<!-- $$$ [ new search buddy starts here ]-->
	<div id="contentBar">
		<div class="wrap searchContent">
			<div id="SearchColumnedContent" class="columnLayout twoColumn ">
				<div class="leftColumnWrap">
					<div class="rightColumnWrap">
						<div class="rightColumn">
							<!-- SEARCH RESULTS CONTENT start -->
							<div class="searchResultsContent">
								<!--h2><Corbis:Localize id="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2-->
								<asp:Panel ID="ClarificationPanel" Visible="false" runat="server">
									<Corbis:Localize ID="DidYouMean" runat="server" meta:resourcekey="DidYouMean" />&nbsp;
								</asp:Panel>
								<asp:Panel runat="server" ID="HeaderPanel" />
								<%--<Corbis:AlbumHeader ID="AlbumHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:PromotionalSetHeader ID="PromotionalSetHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:SameModelHeader ID="SameModelHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:SamePhotoshootHeader ID="SamePhotoshootHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:OutlineHeader ID="OutlineHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:StorySetHeader ID="StorySetHeader" runat="server" EnableViewState="true" Visible=false />
								<Corbis:RFCDHeader ID="RFCDHeader" runat="server" EnableViewState="true" Visible=false />--%>
										<div id="zeroSearchResultsWrapper" class="zeroSearchResults" runat="server" visible="false">
		                                    <div id="zeroSearchResultsPadding" class="zeroSearchResultsPadding">
		                                     <Corbis:Label ID="Label1" CssClass="zeroTitle" runat="server" meta:resourcekey="imageGroupZeroresults" /><br /><br />
                                             <Corbis:GlassButton ID="PreviousButton" runat="server" ButtonBackground="gray36" Visible="false" OnClientClick="javascript:history.go(-1);return false;"  meta:resourcekey="previousButton" /> 
		                                    </div>
		                                </div>
							 </div>
								<aspx:UpdatePanel ID="searchPanel" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
									    
										<Corbis:SearchResultHeader ID="searchResultHeader" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand"
											OnPageSizeCommand="searchResultHeader_PageSizeCommand" />
								        <%--<Corbis:Localize ID="zeroSearchResultsDiv" runat="server" Text="This is empty SearchResults" />--%>
										<Corbis:Products  ID="products" runat="server" EnableViewState="false" />
								        <Corbis:SearchResultFooter ID="searchResultFooter" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand">
									    </Corbis:SearchResultFooter>
									</ContentTemplate>
								</aspx:UpdatePanel>	
	                          
								
							
							<!-- SEARCH RESULTS CONTENT end -->
						</div>
					</div>
					<div class="leftColumn">
						<!-- SEARCH BUDDY start -->
						<div style="top: 205px" class="SearchBuddy" id="SearchBuddy">
							<div class="inSearchBuddy">
								<ul class="SB_tabs">
									<li id="SBT_lightboxes" class="SBT_lightboxes ON">
										<img src="../Images/spacer.gif" alt="" /></li>
									<li id="SBT_quickpic" class="SBT_quickpic" runat="server">
										<img src="../Images/spacer.gif" alt="" /></li>
								</ul>
								
								<div id="SBBX_lightboxes" class="SBBX_lightboxes">
									<!-- $$$ start of lightbox -->
									<div id="lightboxHeaderDiv">
										<span>&nbsp</span>
										<asp:UpdateProgress ID="lightboxUpdateProgress" runat="server" AssociatedUpdatePanelID="detailViewUpdatelightbox">
											<ProgressTemplate>
												<div class="lightboxWrap" id="lightboxProgress">
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
											<%-- <asp:DropDownList ID="lightboxList" class="lightboxList" runat="server" AutoPostBack="true"
												OnSelectedIndexChanged="lightboxList_SelectedIndexChanged" />--%>
											<asp:DropDownList ID="lightboxList" class="lightboxList" runat="server" 
											    onchange="CorbisUI.Handlers.Lightbox.getLightboxItems($(document.body).getElement('select[name$=lightboxList]').getSelected()[0].value);" />
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
												<%--<Corbis:LightboxItems ID="lightboxItems" runat="server" EnableViewState="true">
												</Corbis:LightboxItems>--%>
												<!-- end lightbox control -->
												<div class="centerMe">
													<Corbis:Localize ID="emptyLightboxMessage" runat="server" meta:resourcekey="emptyLightboxMessage"
														Visible="false" />
													<Corbis:HyperLink ID="signIn" runat="server" meta:resourcekey="signIn" NavigateUrl="javascript:CorbisUI.Auth.Check(1)"
														Visible="false" />
													<br />
												    <br />
												    <br />
												</div>
												
											</div>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="hiddenRefresh"/>
											<asp:AsyncPostBackTrigger ControlID="addToLightboxPopup"/>
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
										<Corbis:Localize ID="quickPic" runat="server" meta:resourcekey="quickPic" />
									</div>
									<div class="QPDownloadPositioner">
										<span class="actLikeLink" id="quickPicDownloadAllLink" onclick="CorbisUI.Handlers.Quickpic.downloadAll();"><Corbis:Localize ID="downloadAll" runat="server" meta:resourcekey="downloadAll" /></span>
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
		
		<Corbis:AddToLightbox ID="addToLightboxPopup" runat="server" EnableViewState="true"/>
		<Corbis:CreateLightbox ID="createLightbox" runat="server"/>
		<corbis:AddToCart ID="addToCartControl" runat="server"/>	
		
        <div style="width:222px;height:100%;background:transparent;position:absolute;top:180px;display:none;" id="searchBuddyMask"></div>
        
		<%--CLARIFICATION POPUP--%>
		<div id="ambiguousModal" style="width:340px; display:none;">
		    <div id="titleWrapper" class="titleWrapper">
                <Corbis:Label ID="ambiguousTitle" CssClass="ambiguousTitle" runat="server" meta:resourcekey="ambiguousTitle" />
                <div class="ambiguousCloseButton" id="ambiguousCloseButton">
                    <Corbis:Image ID="clarificationModalPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="HideModal('ambiguousModal');return false;" class="Close" meta:resourcekey="clarificationModalPopupImage"/>
                </div>
            </div>
            <div id="dataContainer" class="clarifcationsDataContainer">
			    <asp:Repeater ID="clarificationGroups" runat="server" OnItemDataBound="clarificationGroups_ItemDataBound">
				    <ItemTemplate>
                        <div id="" class="Clarification"> 
                            <div class="searchResultWrapper"><Corbis:Label ID="SearchResultTitle" CssClass="searchResultTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Keyword")%>' /></div>           
					        <asp:Repeater ID="clarificationTerms" runat="server">
						        <ItemTemplate>
                                    <div class="checkboxWrap">
                                        <Corbis:ImageCheckbox ID="Clarifications" OnClientChanged="updateClarificationCount(this.checked)" CssClass="clarificationsImageCheckbox" Text='<%# DataBinder.Eval(Container.DataItem, "Text")%>' value='<%# DataBinder.Eval(Container.DataItem, "Query")%>' runat="server"></Corbis:ImageCheckbox>
                                    </div>
						        </ItemTemplate>
					        </asp:Repeater>
					        <div id="seperatorLine" class="clarifySeperatorLine"></div>
                        </div>
				    </ItemTemplate>
			    </asp:Repeater>            
            </div>
            <div class="Clr"></div>
            
            <asp:HiddenField ID="clarificationCheckedQueryFlags" runat="server" />
         
            <asp:HiddenField ID="showClarificationPopup" runat="server" Value="false" />
            
            <div id="clarificationFooter" class="clarificationFooter" style="width:284px; height:25px;">
                <div style="float:left;width:80px; margin-top:10px;"><Corbis:LinkButton ID="clarificationOff" CssClass="clarificationOff" 
                runat="server" meta:resourcekey="clarificationOffLink"  OnClientClick="javascript:termsClarificationOffModal();return false;" /></div>
                <div style="position:absolute; right:0px; margin-top:10px; margin-right:10px;">
                    <Corbis:GlassButton ID="clarificatonUpdate" runat="server" ButtonBackground="e8e8e8"
                        OnClientClick="return getClarifactionChecked();"
                        meta:resourcekey="updateClarification"
                    />
                    <Corbis:GlassButton ID="cancelButton" runat="server" CssClass="clarifyCancel" Buttonstyle="Gray" 
                        Onclientclick="HideModal('ambiguousModal');return false;" ButtonBackground="e8e8e8"
                        meta:resourcekey="clarifyCancelButton"
                    />
                </div>
            </div>
            <%--END CLARIFICATION POPUP--%> 
            
            
		</div>
		<Corbis:QuickPicMaxAlert runat="server" />
		
		<asp:HiddenField ID="zeroSearchTrue" runat="server" Value="false" />
		<script language="javascript" type="text/javascript">
		
		CorbisUI.GlobalVars.SearchResults = {
		    isAnonymous : <%= (Corbis.Web.Authentication.Profile.Current.IsAnonymous? "true":"false") %>,
		    isFastLaneEnabled : <%= Corbis.Web.Authentication.Profile.Current.IsFastLaneEnabled? "true":"false" %>,
		    isECommerceEnabled : <%= Corbis.Web.Authentication.Profile.Current.IsECommerceEnabled? "true":"false" %>,
		    showClarificationPopup : '<%=showClarificationPopup.ClientID %>',
		    clarificationUpdate: "<%=clarificatonUpdate.ClientID%>",
		    clarificationQueryFlags: "<%=clarificationCheckedQueryFlags.ClientID%>",
		    zeroSearchResults: "<%=zeroSearchTrue.ClientID%>",
		    checkBoxIDS : {		    },
		    text: {
		        addToCartAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("addToCartAlt.Text")) %>',
		        expressCheckoutAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("expressCheckoutAlt.Text")) %>',
		        deleteBtnAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("deleteBtnAlt.Text")) %>',
		        addQuickpicAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("AddToQuickPicAlt.Text")) %>',
		        removeQuickpicAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("RemoveFromQuickPicAlt.Text")) %>'
		    },
		    urls: {
		        downloadQuickPic: "<%= Corbis.Web.UI.SiteUrls.QuickPic %>"
		    }
		   
		};
		
		// MOVED BULK OF THIS TO Search.js. Above is the Global vars it needed.
		
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
      
//  Failed attempt at getting hover state on celebrity link =   Holly
// window.addEvent('load',        
//      
//      function() {          
//           $('SearchColumnedContent').getElements('select.scrollBoxOLContent option').each(function(el)
//          {
//                el.style.borderWidth="1px";        
//                el.style.borderStyle="dotted";
//                el.style.borderColor="#DD602B";
//                el.addClass('actLikeLink');
//                el.setProperty('onmouseover', 'alert(\'hi there\');');
//                el.addEvent('mouseover',
//                function()
//                {       
//                 //this.style.color='red';
//                 alert('ji there');
//                })
//          });
//});
        </script>
	</div>
</asp:Content>
