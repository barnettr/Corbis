<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SearchResults.aspx.cs"
	Inherits="Corbis.Web.UI.Search.SearchResults" MasterPageFile="~/MasterPages/NoSearchBar.Master"
	Title="<%$ Resources: windowTitle %>" EnableViewStateMac="false" 
	EnableEventValidation="false" ValidateRequest="false" EnableViewState="true" %>
<%@ Import Namespace="Corbis.Framework.Globalization"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="aspx" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Products" Src="Products.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Search" Src="~/src/Navigation/Search.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultHeader" Src="SearchResultHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultFooter" Src="SearchResultFooter.ascx" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToLightbox" Src="~/UserControls/LightboxControls/AddToLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="LightboxItems" Src="~/Search/LightboxItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicItems" Src="~/Search/QuickPicItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="CreateLightbox" Src="~/UserControls/LightboxControls/CreateLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicMaxAlert" Src="~/UserControls/ModalControls/QuickPicMaxAlert.ascx" %>

<asp:Content ID="searchResultsContent" runat="server" ContentPlaceHolderID="mainContent">
    <Corbis:Search ID="searchControl" runat="server" />
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
								<aspx:UpdatePanel ID="searchPanel" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<Corbis:SearchResultHeader ID="searchResultHeader" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand"
											OnPageSizeCommand="searchResultHeader_PageSizeCommand" OnGenericCommand="searchResultHeader_GenericCommand" MaxSearchItems="5000"/>
								        <%--<Corbis:Localize ID="zeroSearchResultsDiv" runat="server" Text="This is empty SearchResults" />--%>
										<Corbis:Products ParentPage="Search" ID="products" runat="server" EnableViewState="false" />
								        <Corbis:SearchResultFooter ID="searchResultFooter" runat="server" Visible="true" OnPageCommand="searchResult_PageCommand" />
								        <asp:HiddenField ID="showClarificationPopup" runat="server" Value="false" />
									</ContentTemplate>
								</aspx:UpdatePanel>	
	                            <%--<aspx:UpdateProgress ID="searchProgIndicator" runat="server" AssociatedUpdatePanelID="searchPanel">
									<ProgressTemplate>
										<div class="mask"></div>
										    <div id="searchProgContents">
											    <img border="0" alt="" src="/images/ajax-loader2.gif" />
											    <br />
											    <h1><Corbis:Label ID="searching" runat="server" meta:resourcekey="updatingResultsText" /></h1>
											    <div id="processingFilters">
	                                                <Corbis:Label ID="procFilters" runat="server" meta:resourcekey="procFilters" />
	                                            </div>
											</div>
									</ProgressTemplate> 
								</aspx:UpdateProgress> --%>
							</div>
							<!-- SEARCH RESULTS CONTENT end -->
						</div>
					</div>
					<div class="leftColumn">
						<!-- SEARCH BUDDY start -->
						<div style="top: 205px" class="SearchBuddy zeroResultsBuddy" id="SearchBuddy">
							<div class="inSearchBuddy">
								<ul class="SB_tabs">
									<li id="SBT_filters" class="SBT_filters ON">
										<img src="../Images/spacer.gif" alt="" /></li>
									<li id="SBT_lightboxes" class="SBT_lightboxes">
										<img src="../Images/spacer.gif" alt="" /></li>
									<li id="SBT_quickpic" class="SBT_quickpic" runat="server">
										<img src="../Images/spacer.gif" alt="" /></li>
								</ul>
								<div id="SBBX_filters" class="SBBX_filters">
									<div id="refineHeaderDiv">
										<span>&nbsp;</span></div>
									<div id="refineHeader">
										<Corbis:Localize ID="searchBuddyRefineTab" runat="server" meta:resourcekey="searchBuddyRefineTab" /></div>
									<div class="resetContainer">
										<Corbis:Label  class="actLikeLink" runat="server"  id="searchBuddyResultsReset" CssClass="searchBuddyResultsReset" meta:resourcekey="resetAlt" onclick="resetSearchCategoryChecks();return false;" >
											<Corbis:Localize ID="reset1" runat="server" meta:resourcekey="reset" /></Corbis:Label></div>
									<div class="InsetBox MT_5">
										<Corbis:ImageCheckbox EnableViewState="false" ID="creative" runat="server" Checked="true" meta:resourceKey="creative"
											 TextStyle="Heading" />
										<div class="imgDivider MB_5">
											<img alt="" class="imgDivider" src="../Images/spacer.gif" />
										</div>
										<Corbis:ImageCheckbox EnableViewState="false" ID="editorial" runat="server" Checked="true" meta:resourceKey="editorial"
                                            OnClientChanged="EditorialChanged(this.checked);" TextStyle="Heading" />
										<div class="ML_20" id="EditorialChildren" style="padding-right: 0px">
											<Corbis:ImageCheckbox EnableViewState="false" ID="documentary" runat="server" Checked="true" meta:resourceKey="documentary"
												OnClientChanged="setEditorialCheckedState()" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="fineArt" runat="server" Checked="true" meta:resourceKey="fineArt"
												OnClientChanged="setEditorialCheckedState()" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="archival" runat="server" Checked="true" meta:resourceKey="archival"
												OnClientChanged="setEditorialCheckedState()" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="currentEvents" runat="server" Checked="true" meta:resourceKey="currentEvents"
												OnClientChanged="setEditorialCheckedState()" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="entertainment" runat="server" Checked="true" meta:resourceKey="entertainment"
												OnClientChanged="setEditorialCheckedState()" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="outline" runat="server" Checked="true" meta:resourceKey="outline"
												OnClientChanged="setEditorialCheckedState()" />
										</div>
										<div class="imgDivider MB_5">
											<img src="../Images/spacer.gif" class="imgDivider" alt="" /></div>
										<Corbis:ImageCheckbox EnableViewState="false" ID="rightsManaged" runat="server" Checked="true" meta:resourceKey="rightsManaged" />
										<Corbis:ImageCheckbox EnableViewState="false" ID="royaltyFree" runat="server" Checked="true" meta:resourceKey="royaltyFree" />
										<!-- END - LICENSE TYPES -->
									</div>
									<div style="height: 5px; overflow: hidden">
									</div>
									<div class="FilterBox NoPeople">
										<div class="FilterBoxInner" style="height: 23px">
											<div id="divNoPeopleIcon" style="float: right; width: 23px; height: 23px; background: url(../Images/BuddyNoPeople.gif) no-repeat top right;
												margin-right: 6px">
											</div>
											<Corbis:ImageCheckbox EnableViewState="false" ID="noPeople" runat="server" Checked="false" meta:resourceKey="noPeople"
												OnClientChanged="noPeopleChanged(this.getParent('div'));CorbisUI.MSOSearch.verifyNumberOfPeopleChecked();" Style="float: left"
												TextStyle="Heading" />
										</div>
									</div>
									<div style="height: 5px; overflow: hidden">
									</div>
									<div class="FilterBox">
										<div class="FilterBoxInner">
											<Corbis:ImageCheckbox EnableViewState="false" ID="photography" runat="server" Checked="true" meta:resourceKey="photography" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="illustration" runat="server" Checked="true" meta:resourceKey="illustration" />
											<div class="imgDivider MB_5" style="margin-right: 8px">
												<img src="../Images/spacer.gif" class="imgDivider" alt="" /></div>
											<Corbis:ImageCheckbox EnableViewState="false" ID="color" runat="server" Checked="true" meta:resourceKey="color" />
											<Corbis:ImageCheckbox EnableViewState="false" ID="blackWhite" runat="server" Checked="true" meta:resourceKey="blackWhite" />
											<div class="imgDivider MB_5" style="margin-right: 8px">
												<img alt="" class="imgDivider" src="../Images/spacer.gif" /></div>
											<Corbis:ImageCheckbox EnableViewState="false" ID="modelReleased" runat="server" Checked="false" meta:resourceKey="modelReleased" />
										</div>
									</div>
									<br />
									<Corbis:GlassButton ID="goButton" CssClass="SearchBuddyGoButton" runat="server" ButtonBackground="gray4b" ButtonStyle="Orange" meta:resourceKey="goButton" OnClientClick="LogOmnitureEvent('event6');return CorbisUI.ExtendedSearch.combineSearchBuddy();" />
								</div>
								<div id="SBBX_lightboxes" class="SBBX_lightboxes hdn">
									<!-- $$$ start of lightbox -->
									<div id="lightboxHeaderDiv">
										<span>&nbsp</span>
										<div class="lightboxWrap" id="lightboxProgress">
											<img border="0" alt="" src="/images/buddyLoading.gif" />
										</div>
									</div>
									<div id="lightboxHeader">
										<a href="../Lightboxes/MyLightboxes.aspx">
											<Corbis:Localize ID="createLightboxTitle" runat="server" meta:resourcekey="createLightboxTitle" />
											<img id="goToLightbox" src="../Images/icon-LB-small.gif" class="ML_5" runat="server" alt="" meta:resourcekey="goToLightbox" />
									    </a>
									</div>
									<asp:UpdatePanel ID="detailViewUpdatelightbox" UpdateMode="Conditional" runat="server" EnableViewState="false">
										<ContentTemplate>
											<asp:DropDownList ID="lightboxList" class="lightboxList" runat="server" onchange="CorbisUI.Handlers.Lightbox.getLightboxItems($(document.body).getElement('select[name$=lightboxList]').getSelected()[0].value);" />
											<input type="button" id="hiddenRefresh" class="hdn" runat="server" onserverclick="hiddenRefresh_OnChange" />
											<div class="CreateLBXPositioner">
												<div class="BTN-orange-1a1a1a BTN-orange" onclick="">
													<div class="right">
														<div class="center">
															<asp:Panel runat="server" ID="pnlCreateLightBox">
																<Corbis:LinkButton ID="createNew" OnClientClick="javascript:openCreateLightbox();return false;"
																	runat="server" CssClass="createNew" meta:resourcekey="createNew" />
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
												<div class="centerMe" style="display: none;">
													<Corbis:Localize ID="emptyLightboxMessage" runat="server" meta:resourcekey="emptyLightboxMessage"
														Visible="false" />
													<Corbis:HyperLink ID="signIn" runat="server" meta:resourcekey="signIn" NavigateUrl="javascript:CorbisUI.SearchBuddy.lightboxLogin()"
														Visible="false" />
												</div>
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
										<Corbis:Localize ID="quickPic" runat="server" meta:resourcekey="quickPic" />
									</div>
									<div class="QPDownloadPositioner">
										<span class="actLikeLink" id="quickPicDownloadAllLink" runat="server"  onclick="CorbisUI.Handlers.Quickpic.downloadAll();"><Corbis:Localize ID="downloadAll" runat="server" meta:resourcekey="downloadAll" /></span>
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
		<corbis:AddToCart ID="addToCartControl" runat="server"/>	
		
        <div style="width:222px;height:100%;background:transparent;position:absolute;top:180px;display:none;" id="searchBuddyMask"></div>
        
		<%--CLARIFICATION POPUP--%>
		<div id="ambiguousModal" style="width:340px; height: auto; display:none;">
		    <div id="titleWrapper" class="titleWrapper">
                <Corbis:Label ID="ambiguousTitle" CssClass="ambiguousTitle" runat="server" meta:resourcekey="ambiguousTitle" />
                <div class="ambiguousCloseButton" id="ambiguousCloseButton">
                    <Corbis:Image ID="clarificationPopupHideImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="HideModal('ambiguousModal');return false;" class="Close" meta:resourcekey="clarificationPopupHideImage"/>
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
                                        <Corbis:ImageCheckbox EnableViewState="false" ID="Clarifications" OnClientChanged="updateClarificationCount(this.checked)" CssClass="clarificationsImageCheckbox" Text='<%# DataBinder.Eval(Container.DataItem, "Text")%>' value='<%# DataBinder.Eval(Container.DataItem, "Query")%>' runat="server"></Corbis:ImageCheckbox>
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
         
            <div id="clarificationFooter" class="clarificationFooter">
                <div style="float:right;margin-top:10px;">
                    <Corbis:GlassButton ID="clarificatonUpdate" runat="server" ButtonBackground="e8e8e8"
                        OnClientClick="CorbisUI.ExtendedSearch.validateSearch(); return false;"
                        meta:resourcekey="updateClarification"
                    />
                    <Corbis:GlassButton ID="cancelButton" runat="server" CssClass="clarifyCancel" Buttonstyle="Gray" 
                        Onclientclick="CorbisUI.ExtendedSearch.getClarifactionChecked('cancel');return false;" ButtonBackground="e8e8e8"
                        meta:resourcekey="clarifyCancelButton"
                    />
                </div>
                <div style="float:left;width:80px; margin-top:10px;"><Corbis:LinkButton ID="clarificationOff" CssClass="clarificationOff" 
                runat="server" meta:resourcekey="clarificationOffLink"  OnClientClick="javascript:termsClarificationOffModal();return false;" /></div>
            </div>
            <div class="Clr"></div>
            <%--END CLARIFICATION POPUP--%> 
            
            
		</div>
		<div id="zeroSearchResult"> <!--added this Div to fix the problem at No Search Section(Content area is not an appropriate height. You can’t see the messaging)-->
		<div id="zeroSearchResultsWrapper" class="zeroSearchResults rounded" runat="server" visible="false">
		    <div id="zeroSearchResultsPadding" class="zeroSearchResultsPadding">
		        <Corbis:Label ID="zeroTitle" CssClass="zeroTitle" runat="server" meta:resourcekey="zeroTitle" /><br /><br />
		        <ul class="zeroList">
		            <li><Corbis:Localize ID="zeroResultsItem1" runat="server"  meta:resourcekey="zeroResultsItem1" /></li>
		            <li><Corbis:Localize ID="zeroResultsItem2" runat="server" meta:resourcekey="zeroResultsItem2" /></li>
		            <li><Corbis:Localize ID="zeroResultsItem3" runat="server" meta:resourcekey="zeroResultsItem3" /></li>
		            <%--<li><Corbis:Localize ID="zeroResultsItem4" runat="server" meta:resourcekey="zeroResultsItem4" /></li>--%>
		        </ul>
		    </div>
		</div></div>
		
		<asp:HiddenField ID="zeroSearchTrue" runat="server" Value="false" />
		<script language="javascript" type="text/javascript">
		
		
		CorbisUI.GlobalVars.SearchResults = {
		    isAnonymous : <%= (Corbis.Web.Authentication.Profile.Current.IsAnonymous? "true":"false") %>,
		    isFastLaneEnabled : <%= Corbis.Web.Authentication.Profile.Current.IsFastLaneEnabled? "true":"false" %>,
		    isECommerceEnabled : <%= Corbis.Web.Authentication.Profile.Current.IsECommerceEnabled? "true":"false" %>,
		    isBasket : <%=  Language.CurrentLanguage == Language.ChineseSimplified || 
		                    Language.CurrentLanguage == Language.EnglishUS ||
		                    Language.CurrentLanguage == Language.PortugueseBrazil ||
		                    Language.CurrentLanguage == Language.Spanish ||
		                    Language.CurrentLanguage == Language.Japanese ||
		                    Language.CurrentLanguage == Language.Italian ? "false" : "true"  %>,
		    showClarificationPopup : '<%=showClarificationPopup.ClientID %>',
		    clarificationUpdate: "<%=clarificatonUpdate.ClientID%>",
		    clarificationQueryFlags: "<%=clarificationCheckedQueryFlags.ClientID%>",
		    zeroSearchResults: "<%=zeroSearchTrue.ClientID%>",
		    buddySignInLink: "<%=signIn.ClientID%>",
		    checkBoxIDS : {
		        noPeople : '<%= noPeople.ClientID %>',
		        rmLicense : '<%=rightsManaged.ClientID %>',
		        rfLicense : '<%=royaltyFree.ClientID %>',
		        creative : '<%=creative.ClientID %>',
		        editorial : '<%=editorial.ClientID %>',
		        documentary : '<%= documentary.ClientID %>',
		        fineArt : '<%= fineArt.ClientID %>',
		        archival : '<%= archival.ClientID %>',
		        currentEvents : '<%=currentEvents.ClientID %>',
		        entertainment : '<%=entertainment.ClientID %>',
		        outline : '<%=outline.ClientID %>',
		        photography : '<%=photography.ClientID %>',
		        illustration : '<%=illustration.ClientID %>',
		        color : '<%=color.ClientID %>',
		        blackWhite : '<%=blackWhite.ClientID %>',
		        onlyModelReleased : '<%= modelReleased.ClientID %>'
		    },
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

        CorbisUI.ExtendedSearch.vars.currentPage = <%=searchResultHeader.CurrentPage %>;
		CorbisUI.ExtendedSearch.vars.totalPages = <%=searchResultHeader.TotalPages %>;

		
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
       
        </script>
	</div>
	
	<Corbis:ModalPopup ID="filterErrorModal" meta:resourcekey="filterError" runat="server" ContainerID="filterError" Width="300">
	    <div id="filterErrorCategories">
	        <Corbis:Label ID="filterErrorCategories" meta:resourcekey="filterErrorCategories" runat="server" />
	    </div>
	    <div id="filterErrorColor">
	        <Corbis:Label ID="filterErrorColor" meta:resourcekey="filterErrorColor" runat="server" />
	    </div>
	    <div id="filterErrorPhotography">
	        <Corbis:Label ID="filterErrorPhotography" meta:resourcekey="filterErrorPhotography" runat="server" />
	    </div>
	    <div id="filterErrorRMRF">
	        <Corbis:Label ID="filterErrorRMRF" meta:resourcekey="filterErrorRMRF" runat="server" />
	    </div>
	    <div style="margin: 5px 0 5px 0">
	        <img src="/images/redleftarrow.gif" alt="" style="margin-right:5px;" />
	        <Corbis:Label ID="filterErrorToTheLeft" CssClass="Required" meta:resourcekey="filterErrorToTheLeft" runat="server" />
	    </div>
	    <Corbis:GlassButton ID="closeButton" meta:resourcekey="closeButton" OnClientClick="HideModal('filterError');return false;" CausesValidation="false" runat="server" />
	</Corbis:ModalPopup>
	<Corbis:QuickPicMaxAlert runat="server" />
</asp:Content>
