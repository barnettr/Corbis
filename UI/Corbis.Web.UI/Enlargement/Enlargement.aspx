 <%@ Page Language="C#" AutoEventWireup="True" Codebehind="Enlargement.aspx.cs" Inherits="Corbis.Web.UI.Enlargement.Enlargement"
    MasterPageFile="~/MasterPages/NoGlobalNav.Master" EnableViewState="true" %>

<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToLightbox" Src="~/UserControls/LightboxControls/AddToLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Restrictions" Src="../src/Image/Restrictions.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Filmstrip" Src="~/src/Image/Filmstrip.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicMaxAlert" Src="~/UserControls/ModalControls/QuickPicMaxAlert.ascx" %>

<asp:Content ID="enlargementContent" ContentPlaceHolderID="mainContent" runat="server">

    <!-- Give a Background Color -->
    <div id="backgroundDiv" class="print-colors">
        <!-- Div for Top "Text Header Tabs, Sign In, Pager Control -->
        <div id="topNavDiv" class="print-colors">
            <img src="../Images/corbis-logo_sm.gif" class="show-for-print" />
            <!-- "Image Detail" Text Header Tab -->
            <a id="imageDetails" class="selectedTab" onclick="javascript:CorbisUI.Enlargement.selectTab('imageDetails');CorbisUI.Enlargement.hideCorner();return false;">
                <b>
                    <Corbis:Localize ID="imageDetailsText" runat="server" meta:resourcekey="imageDetailsText" />
                </b>
            </a>
            <!-- "Corbis Keywords" Text Header Tab -->
            <a id="corbisKeywords" class="tab hide-for-print" onclick="javascript:CorbisUI.Enlargement.selectTab('corbisKeywords');CorbisUI.Enlargement.showCorner();return false;" runat="server" visible="false">
                <b>
                    <Corbis:Localize ID="corbisKeywordsText" runat="server" meta:resourcekey="corbisKeywordsText" />
                </b>
            </a>
            <!-- "Related images" Text Header Tab -->
            <a id="relatedImages" class="tab hide-for-print" onclick="javascript:CorbisUI.Enlargement.selectTab('relatedImages');CorbisUI.Enlargement.showCorner();return false;" runat="server" visible="false">
                <b>
                    <Corbis:Localize ID="relatedImagesText" runat="server" meta:resourcekey="relatedImagesText" />
                </b>
            </a>
            <!-- Sign in To Remove Watermarks Text -->
            <span id="signIn">
                <Corbis:HyperLink ID="signInLink" NavigateUrl="javascript:CorbisUI.Auth.Check(1,CorbisUI.Auth.ActionTypes.Execute,'refreshEnlargementPage()')" runat="server" meta:resourcekey="signIn" />&nbsp;
                <Corbis:Localize ID="waterMark" meta:resourcekey="waterMark" runat="server" />
            </span>
            <!-- Paging Control -->
            <div id="imagePaging" class="floatRight hide-for-print">
                <Corbis:Pager ID="imagePager" runat="server" OnPageCommand="PageChanged" PageSize="1" 
                    PrevCssClass="PrevButton" NextCssClass="NextButton" PrevDisabledCssClass="PrevButtonDisabled"
                    NextDisabledCssClass="NextButtonDisabled" PageNumberCssClass="NavPageNumber"
                    PrefixLabelCssClass="PagerLabelPreFix" PostfixLabelCssClass="PagerLabelPostFix" EnableViewState="true" meta:resourcekey="imagePager" />
            </div>
        </div>
        <!-- Tabs Content -->
        <div id="imageDetailsDiv" class="print-colors">
            <!-- Spacing -->
            <div id="TabTop" class=" hide-for-print">
                <div class="inTabTop hdn">&nbsp;</div>
            </div>
            <!-- Image Details Tab Content -->
            <div id="imageDetailsContent" class="print-colors">
                <!-- Image Detail Tab Content Header Bar -->
                <div id="controlsDiv">
                    <h1>
                        <asp:Label ID="category" runat="server"  CssClass="ImageDetailsCategory" />
                    </h1>
                    <div class="iconToolset hide-for-print">
                        <Corbis:LinkButtonList ID="iconToolset" runat="server" Spacing="20px" ValueIsScript="true" />
                    </div>
                    <div id="viewDimensions" class="BTN-border-only hide-for-print<%= (this.IsRfcd? " hdn": "") %>" onclick="javascript:CorbisUI.Enlargement.OpenViewDimensions();">
                        <div class="right">
                            <div class="center" title="<%= GetLocalResourceString("viewDimensionTooltip") %>">
                                <asp:Label ID="viewDimension" runat="server" meta:resourceKey="viewDimension" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Image Detail Tab Content Header Bar:Ends -->
                <!-- Left Side Image detail -->
                <div class="detailWrap print-colors">
                    <div id="loading">
                    </div>
                    <table cellspacing="0" class="centerImage">
                        <tr>
                            <td>
                                <asp:Image ID="enlargementImage" runat="server" CssClass="enlargementImage"/>
                            </td>
                        </tr>
                    </table>
                    <div class="show-for-print coverAnchors">&nbsp;</div>
                    <div class="infoWrap print-colors">
                        <div id="imageInfo" class="info" runat="server">
                            <div class="header">
                                <div id="imageTitleAndNumber">
                                    <b>
                                        <asp:Label ID="priceTier" runat="server" CssClass="imagetitle MB_0"></asp:Label></b>
                                        <asp:Label ID="licenseModel" runat="server" CssClass="imagetitle MB_0" /><br />
                                    <b>
                                        <asp:Label ID="imageID" runat="server" CssClass="infoWrap1 MT_0 enlargementImageID"></asp:Label>
                                    </b>
                                </div>
                                
                                <b class="print-colors">
                                    <asp:HyperLink ID="price" runat="server" CssClass="orangeText infoWrap1 MT_0"></asp:HyperLink>
                                    <asp:HyperLink ID="priceStatus" runat="server" CssClass="orangeText infoWrap1 MT_0"></asp:HyperLink>
                                </b>
                                <br class="hide-for-print" />
                                <b>
                                <asp:Label ID="contentWarnings" CssClass="infoWrap1 warningText" runat="server"></asp:Label>
                                </b>
                                <hr size="1" class=".c9c9c9_HorizontalRule hide-for-print" />
                                <b>
                                    <asp:Label ID="imageTitle" runat="server" CssClass="imagetitle MB_0 mainImageTitle"></asp:Label></b>
                                <div class="infoWrap1 MT_0">
                                    <asp:Literal ID="imageCaption" runat="server"></asp:Literal></div>
                                <div class="clr">
                                </div>
                            </div>
                            <div class="MT_0">
                                <div class="MB_5">
                                    <asp:Panel ID="panelImageText" runat="server">
                                        <div class="MT_0 MB_5 imageTitleAndCreditLine">
                                            <b>
                                                <Corbis:Localize ID="imageText" runat="server" /></b><br class="hide-for-print" />
                                                <asp:Label ID="creditLine" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelartworkText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="artworkText" runat="server" meta:resourcekey="artworkText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="fineArtCreditLine" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelcreatorNameText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="creatorNameText" runat="server" meta:resourcekey="creatorNameText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="contentCreator" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="paneldataCreatedText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="dataCreatedText" runat="server" meta:resourcekey="dataCreatedText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="createDate" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="paneldatePhotographedText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="datePhotographedText" runat="server" meta:resourcekey="datePhotographedText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="photoDate" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panellocationText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="locationText" runat="server" meta:resourcekey="locationText" /></b><br class="hide-for-print" />
                                            <span class="orangeText">
                                                <asp:HyperLink ID="location" CssClass="actLikeLink" runat="server" target="_blank" /></span><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelphotographerText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="photographerText" runat="server" meta:resourcekey="photographerText" />
                                            </b>
                                            <br class="hide-for-print" />
                                            <span class="orangeText">
                                                <asp:HyperLink ID="photographer" CssClass="actLikeLink" runat="server" target="_blank" /></span><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelcollectionText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="collectionText" runat="server" meta:resourcekey="collectionText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="collection" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelmagazineText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="magazineText" runat="server" meta:resourcekey="magazineText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="magazine" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="paneldatePublishedText" runat="server" cssClass="detailPair">
                                        <div class="MT_0 MB_5">
                                            <b>
                                                <Corbis:Localize ID="datePublishedText" runat="server" meta:resourcekey="datePublishedText" /></b><br class="hide-for-print" />
                                            <asp:Label ID="publishDate" runat="server"></asp:Label><br class="hide-for-print" />
                                        </div>
                                    </asp:Panel>
                                    </div>
                            </div>
                            <hr size="1" class=".c9c9c9_HorizontalRule hide-for-print" />
                            <!--- restrictions-->
                            <Corbis:Restrictions ID="ImageRestrictions" runat="server" UsePopupMode="true"></Corbis:Restrictions>
                            <!--end-->
                        </div>
                        <div id="rfcdInfo" class="info" runat="server">
                            <div class="header">
                                <div id="imageTitleAndNumber">
                                    <b>
                                        <asp:Label ID="rfcdTitle" runat="server" CssClass="imagetitle MB_0" style="color:#646464;" />
                                    </b>
                                    <br />
                                    <b>
                                        <asp:Label ID="rfcdId" runat="server" CssClass="infoWrap1 MT_0 enlargementImageID"></asp:Label>
                                    </b>
                                </div>
                                <br class="hide-for-print" />
                                <hr size="1" class=".c9c9c9_HorizontalRule hide-for-print" />
                                <br class="hide-for-print" />
                                <b class="hide-for-print">
                                    <asp:HyperLink ID="viewRfcdLink" runat="server" CssClass="orangeText infoWrap1 MT_0" meta:resourcekey="viewRfcdLink"></asp:HyperLink>
                                </b>
                                <div class="clr">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Left Side Image detail:Ends -->
                <div class="clr">
                </div>
                <div class="enlargeImgFooter hide-for-print">
                    <div class="enlargeImgContent hide-for-print">
                        <div>
                            <asp:Label ID="copyRight" runat="server" CssClass="enlargeImgCopyright" />
                        </div>
                        <div class="enlargeImgOtherLinks hide-for-print" id="disclaimerLinkDiv">
                            <b><a id="disclaimerLink" href="javascript:showDisclaimer('disclaimerLink')"><Corbis:Localize ID="captionDisclaimer" runat="server" meta:resourcekey="captionDisclaimerText" /></a> | <a href="javascript:OpenModal('feedbackPopup')"><Corbis:Localize ID="imageFeedback" runat="server" meta:resourcekey="imageFeedbackText" /> </a>
                            </b>
                        </div>
                    </div>
					<img src="../Images/enlargement-corner_bottomright.gif" alt="" class="hide-for-print" />
                </div>
            </div>
            <!-- Keywords and RelatedImages Tab Content -->
			<div id="keywordsRelatedImages" class="hdn">
			    <!-- Keywords and Related Images Left Side -->
				<div id="imageDetailSummary" class="imageDetailSummary rounded">
					<div class="tabContent">
                      <div class="info">
                        <div class="header">
                                <div class="thumb MB_0">
                                  <span class="ProductBlock" id="productBlock" runat="server">
			                        <Corbis:CenteredImageContainer IsAbsolute="true" runat="server" ImageID="enlargement" Size="170" id="thumbWrap"/>                            
                                  </span>
                                </div>
                                <br class="hide-for-print" />
                                <div class="thumbInfo">
                                    <asp:Label ID="categoryKW" runat="server" CssClass= "whiteLabel MB_5 " />
                                    <asp:Label ID="priceTierKW" runat="server" CssClass="priceTierKW"  /> <asp:Label ID="licenseModelKW" runat="server" CssClass="priceTierKW "/>
                                    <asp:Label ID="imageIDKW" runat="server" CssClass="idKW " ></asp:Label>
                                    <br class="hide-for-print" />
                                </div>
                                <div class="clr"> </div>
                        </div>
                        <br class="hide-for-print" />
                        <asp:Label ID="imageTitleKW" runat="server" CssClass="imagetitle1 MB_0"></asp:Label><br class="hide-for-print" />
                        <p class="imageCaptionKW MT_0">
                            <asp:Literal ID="imageCaptionKW" runat="server"></asp:Literal>
                        </p>                             
                        <asp:Panel ID="panelImageTextPW" runat="server">
                            <p class="whiteLabel1">
                                <b>
                                <Corbis:Localize ID="imageTextKW" runat="server" />
                                </b>
                                <br class="hide-for-print" />
                                <asp:Label ID="creditLineKW" runat="server" CssClass="imageCaptionKW case"></asp:Label><br class="hide-for-print" />
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="paneldatePhotographedTextKW" runat="server">
                            <p class="whiteLabel1">
                                <b>
                                <Corbis:Localize ID="datePhotographedTextKW" runat="server" meta:resourcekey="datePhotographedText" /></b><br class="hide-for-print" />
                                <asp:Label ID="photoDateKW" runat="server" CssClass="imageCaptionKW case"></asp:Label><br class="hide-for-print" />
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="paneldataCreatedTextKW" runat="server">
                            <p class="whiteLabel1">
                                <b>
                                <Corbis:Localize ID="dataCreatedTextKW" runat="server" meta:resourcekey="dataCreatedText" /></b><br class="hide-for-print" />
                                <asp:Label ID="createDateKW" runat="server" CssClass="imageCaptionKW case"></asp:Label><br class="hide-for-print" />
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="panellocationTextKW" runat="server">
                            <p class="whiteLabel1">
                                <b>
                                <Corbis:Localize ID="locationTextKW" runat="server" meta:resourcekey="locationText" /></b><br class="hide-for-print" />
                                <span class="orangeText MT_0 case">
                                    <asp:HyperLink ID="locationKW" CssClass="actLikeLink" runat="server" />
                                </span>
                            </p>
                        </asp:Panel>
                        </div>
                     </div>	
                </div>
				<!-- Keywords Right Side -->
				<div id="corbisKeywordsContent" class="hdn rounded">
					<div class="keywordPadding">
						<div class="tabContent">
							<h3>
								<Corbis:Localize ID="keywordsTitle" runat="server" meta:resourcekey="keywordsTitle" />
							</h3>
							<p>
								<Corbis:Localize ID="keywordsIntro" runat="server" meta:resourcekey="keywordsIntro" />
							</p>
							<div class="keywordsDisplay">
							    <asp:DataList ID="KeywordRepeater" RepeatColumns="2" RepeatDirection="Vertical" EnableViewState="true" runat="server">
								    <ItemTemplate>                            
									    <div class="keywordsSection">
									        <Corbis:ImageCheckbox ID="Keywordscheckbox" runat="server" OnClientChanged="javascript:enableOrDisableSearchClearOnCheck();" Value='<%# DataBinder.Eval(Container.DataItem, "Term")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Term")%>'></Corbis:ImageCheckbox>
									    </div>
								    </ItemTemplate>
							    </asp:DataList>
							</div>
							<div class="FormButtons searchButton">
								 <Corbis:GlassButton ID="SearchNowButton" runat="server" ButtonStyle="Orange" ButtonBackground="gray36" CausesValidation="false" meta:resourcekey="searchnow" />  
								 <span id="clearKeywords">
										<Corbis:TextIconButton Icon="clearAll" runat="server" ID="clearKeywordCheckbox" OnClientClick="javascript:clearKeywordsCheckBox();" meta:resourcekey="clearItems"/>
								 </span>
								 <div class="clr">
								 </div>
							</div>
						</div>
					</div>
				</div>
				<div id="relatedImagesContent" class="hdn rounded">
					<div class="tabContent">
						<div id="relatedImageGroupsWrapper">
							<h2>
							    <Corbis:Localize ID="relatedImagesHeading" runat="server" meta:resourcekey="relatedImagesHeading" />
							</h2>
							<div class="relatedImageMessage hdn">
							    <Corbis:Label ID="relatedImagesMessage" runat="server" meta:resourcekey="relatedImagesMessage" />
							 </div>
							<div id="relatedImageGroups">
						    </div>
						</div>
					</div>
				</div>
			<div id="TabBottom">
					<div class="inTabBottom">&nbsp;</div>
				</div>
			</div>
		</div>
	</div>	
	<div class="hdn">
		<div id="imageGroupTemplate">
			<div class="relatedImageGroupHeading">
				<div class="relatedImageGroupTitle hdn">
				</div>
				<img class="hdn" src="../Images/icon-smallCD.gif" alt="" height="16px" width="17px"/>
				<h3>
				</h3>
				<div class="relatedImageGroupDetail">
				    <Corbis:Localize ID="relatedImageGroupDetail" runat="server" meta:resourcekey="relatedImageGroupDetail" />
				</div>
			</div>
			<div class="filmstripWrapper">
				<Corbis:Filmstrip runat="server" FilmstripClass="filmstrip" PreviousClass="filmstripPrevious disabled"
					NextClass="filmstripNext disabled" WindowClass="filmstripItemsWindow hdn" WindowMessageClass="filmstripLoading"
					ItemsClass="filmstripItems"/>
			</div>
		</div>
	</div>
	
	<ul class="hdn">
		<li id="imageTemplate" class="relatedImageItem">
			<div>
				<a href="#">
				</a>
			</div>
		</li>		
	</ul>	
		
	<Corbis:AddToLightbox ID="addToLightboxPopup" runat="server" UseDefalutHandler="true" PopulateDropdown="true" OnAddToNewLightboxHandler="AddToLightboxHandler" OnAddToLightboxHandler="AddToLightboxHandler"/>
	<corbis:AddToCart ID="addToCartControl" runat="server"/>

    <script language="javascript" type="text/javascript">
		var isAnonymous  = <%= (Corbis.Web.Authentication.Profile.Current.IsAnonymous? "true":"false") %>;
    </script>

    <!-- Disclaimer Model Popup -->
    <Corbis:ModalPopup ContainerID="disclaimerPopup" Width="500" ID="disclaimerTitle"
        runat="server" meta:resourcekey="disclaimerTitle">
            <div class="disclaimerContent">
	            <p><Corbis:Label ID="disclaimerPara1" runat="server" meta:resourcekey="disclaimerPara1" /></p>
	            <p><Corbis:Label ID="disclaimerPara2" runat="server" meta:resourcekey="disclaimerPara2" /></p>
	            <p><Corbis:Label ID="disclaimerPara3" runat="server" meta:resourcekey="disclaimerPara3" /></p>
	            <p><Corbis:Label ID="disclaimerPara4" runat="server" meta:resourcekey="disclaimerPara4" /></p>
	        </div>
    </Corbis:ModalPopup>
    <!-- Disclaimer Model Popup:Ends -->

    <!-- Image Feedback Model Popup -->
    <div id="feedbackPopup" style="width:500px;display:none;">
        <div class="ModalPopupPanelDialog" style="width:500px;">
            <div class="ModalTitleBar">
                <span class="TitlebarTitle">
                    <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width: 0px;"
                    onclick="closeFeedBackPopup();return false;" />
                    <Corbis:Label ID="feedbackPopupTitle" runat="server" meta:resourcekey="feedbackPopupTitle" />
                </span>
                
            </div>
            <div class="ModalPopupFeedback">
                
                <div class="PaneContent" id="validationPane">
                     <Corbis:ValidationHub 
                        ID="vHub" runat="server"  ContainerID="validationPane"
                        IsIModal="false" IsPopup="true" PopupID="feedbackPopup" ResizeScript="ResizeModal('feedbackPopup')" SubmitForm="true" 
                      />
               
               

                <asp:UpdatePanel ID="FeedbackPanel" EnableViewState="true" runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
                   <ContentTemplate>
	            <asp:HiddenField ID="emailErrorFromServer" value="false" runat="server"/>
	            <div class="feedbackMsg"><Corbis:Localize ID="feedbackText" runat="server" EnableViewState="false" /></div>
	            <div class="dataRowNoBorder">
	                <Corbis:Label ID="userNameLabel" runat="server" CssClass="formL" meta:resourceKey="userNameLabel"></Corbis:Label>
	                <span class="formR" >
	                    <asp:Label runat="server"  ID="userName"></asp:Label>
	                </span>
	            </div>
	            <div class="dataRowNoBorder">
	                <Corbis:Label ID="nameLabel" runat="server" CssClass="formL"  meta:resourceKey="nameLabel"></Corbis:Label>
	                <span class="formR ">
	                    
	                    <Corbis:TextBox runat="server" validate = "required" CssClass="dataRowInput" ID="name" meta:resourceKey="name" ValidateControl="false"></Corbis:TextBox>
	                </span>
	            </div>
	            <div class="dataRowNoBorder" >
	                <Corbis:Label ID="emailLabel" runat="server" CssClass="formL" meta:resourceKey="emailLabel"></Corbis:Label>
	                <span class="formR ">
	                    <Corbis:TextBox runat="server"  validate = "custom1" custom1="validateEmail();"  CssClass="dataRowInput" ID="userEmail" meta:resourceKey="email" ValidateControl="false" ></Corbis:TextBox>
	                </span>
	            </div>
	            <div class="dataRowNoBorder">
	                <Corbis:Label ID="phoneNumberLabel" runat="server" CssClass="formL" meta:resourceKey="phoneNumberLabel"></Corbis:Label>
	                <span class="formR">
	                    <Corbis:TextBox runat="server" CssClass="dataRowInput" ID="phoneNumber" ValidateControl="false" />
	                </span>
	            </div>
	            <div class="dataRowNoBorder">
	                <Corbis:Label ID="roleLabel" runat="server" CssClass="formL" meta:resourceKey="roleLabel"></Corbis:Label>
	                <span class="formR">
	                    <Corbis:DropDownList Width="250px" runat="server" validate = "custom1" custom1="validateRole();"  meta:resourceKey="role" CssClass="dataRowInput" ID="role" ValidateControl="false" />
	                </span>
	            </div>
	            <div class="grayRow">
	                <div class="grayRowContent"><Corbis:Localize ID="whatsThisImageLabel" runat="server" meta:resourcekey="whatsThisImageLabel" ></Corbis:Localize></div>
	            </div>                            
                
                <div class="feedbackRadioButtons">
                
                <Corbis:RadioButtonList ID="issueType" runat="server" CssClass="checkboxesAdjustment">
                  <asp:ListItem Value="Imaging" meta:resourceKey="imaging" Selected="True"></asp:ListItem>
                  <asp:ListItem Value="ImageInformation" meta:resourceKey="imageInfo"></asp:ListItem>
                  <asp:ListItem Value="Legal" meta:resourceKey="legal"></asp:ListItem>
                  <asp:ListItem Value="Editing" meta:resourceKey="editing"></asp:ListItem>
                  <asp:ListItem Value="Other" meta:resourceKey="other"></asp:ListItem>
                </Corbis:RadioButtonList>
                </div>
                
                <div class="FormRow">          
	            <div class="grayRow ">
	                <div class="grayRowContent"><Corbis:Localize ID="commentsLabel" runat="server" EnableViewState="false" meta:resourcekey="commentsLabel" ></Corbis:Localize>  </div>                                          
	            </div>
                <Corbis:TextBox runat="server" ID="comments" validate="custom1" custom1="validateComments()" Wrap="true" TextMode="MultiLine"  meta:resourceKey="comments" ValidateControl="false" ></Corbis:TextBox>                
               </div>
                
                <Corbis:LinkButton ID="lb" runat="server" OnClick="FeedbackSubmit_Click"  CssClass="ValidateClickLB displayNone" />
                   </ContentTemplate>
                   
                </asp:UpdatePanel>   
                
	            <div class="FormButtons">
                    <Corbis:GlassButton ID="feebackCancel" runat="server" ButtonStyle="Gray" CausesValidation="false" meta:resourceKey="feebackCancel" OnClientClick="closeFeedBackPopup();return false;" />  

                    <Corbis:GlassButton ID="feedbackSubmit" runat="server"  meta:resourceKey="feedbackSubmit" OnClientClick="resetServerErrors();_Validation.validateAll();return false;" />  
                </div>
                
              </div> 
            </div>
        </div>
    </div>
    <!-- Image Feedback Model Popup:Ends -->
    
    <!-- Image Feedback Success Model Popup -->
    <Corbis:ModalPopup containerId="feedbackSuccess" runat="server" meta:resourcekey="feedbackSuccess">
        <Corbis:GlassButton runat="server" CausesValidation="false" meta:resourcekey="feedbackSuccessClose" OnClientClick="HideModal('feedbackSuccess');return false;" />  
    </Corbis:ModalPopup>
    <!-- Image Feedback Success Model Popup:Ends -->
    
     <!-- Dimensions Modal Popup-->
     <div id="dimensions" style="display:none;">
        <div id="viewDimensionsModal" >
            <div id="dataContainer" class="dataContainer">
                <span id="dimensionsTitle" class="dimensionsTitle"><Corbis:Localize ID="title" runat="server" meta:resourcekey="title" /></span>
                <div class="dimensionCloseButton" id="closeButton">
                    <img alt="" onclick="javascript:CorbisUI.Enlargement.hideViewDimensionsWindow();" class="Close" src="/Images/iconClose.gif" />
                </div>
                <br class="hide-for-print" /><br class="hide-for-print" />
                <div id="dimensionsDescription" class="dimensionsDescription">
                    <span><Corbis:Localize ID="description" runat="server" meta:resourcekey="description" /></span>
                </div>
                <div class="dimensionsGrid">
                    <div class="dimensionsHeaderTable">
                        
                        <table cellpadding="5" cellspacing="0" class="dimensionsTable">
                            <tr>
                                <td class="dimensionsHeaderLeft"><Corbis:Label runat="server" ID="filesizeHeading" meta:resourcekey="filesize" /></td>
                                <td class="dimensionsHeaderMiddle"><Corbis:Label runat="server" ID="Label1" meta:resourcekey="dimensions" /></td>
                                <td class="dimensionsHeaderRight"><Corbis:Label runat="server" ID="Label2" meta:resourcekey="availability" /></td>
                            </tr>
                        </table>
                        <asp:Repeater ID="dimensionsRepeater" OnItemDataBound="dimensionsRepeater_ItemDataBound" runat="server" >
                            <ItemTemplate>                   
                                <div>
                                    <div class="dimensionsRow">
                                        <ul>
                                            <li style="width:150px;">
                                                <Corbis:Label ID="imageSize" runat="server" />
                                                (<Corbis:Label ID="uncompressedFileSize" runat="server" /><sup>*</sup>)
                                            </li>                                
                                            <li >
                                                <div id="dimensionsWrapper" runat="server">
                                                    <Corbis:Label ID="dimensionsInPixels" runat="server" />                              
                                                    &#149;                                 
                                                    <Corbis:Label ID="height" runat="server" />                            
                                                    <Corbis:Label ID="measurementUnit" runat="server" />
                                                    x
                                                    <Corbis:Label ID="width" runat="server" />
                                                    <Corbis:Label ID="measurementUnit2" runat="server" />
                                                    @                                                          
                                                    <Corbis:Label ID="resolution" runat="server" /> 
                                                    <Corbis:Label ID="ppi" runat="server" meta:resourcekey="ppi" />                                                
                                                </div>
                                            </li>
                                            <li class="Right">
                                                <Corbis:Label width="71px" ID="availability" runat="server" />
                                                <a href="javascript:void(0);" onclick="javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this);return false;">
                                                <Corbis:Label width="71px" ID="contactUsLink" runat="server" Visible=false /></a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div> 
                </div>
                <div class="dimensionsFileSizeNote">
					<asp:Label runat="server" ID="filesizenote" meta:resourcekey="filesizenote" />
                </div>
            </div>
        </div>
     </div>
     <!-- Dimensions Modal Popup:End -->
     <Corbis:ModalPopup ID="registerSuccessDiffCountry" ContainerID="registerSuccessDiffCountry" runat="server"  meta:resourcekey="alertTitle" CloseScript="javascript:self.close();return false;">
       <div>
        <Corbis:Image ImageUrl="/Images/alertYellow.gif" runat="server"/>
        <Corbis:Localize ID="warningDifferentCountry" runat="server" meta:resourcekey="warningDifferentCountry" />
         <Corbis:HyperLink ID="contactCorbis" runat="server"  meta:resourcekey="contactCorbis" /> 
        <Corbis:Localize ID="warningDifferentCountryEnd" runat="server" meta:resourcekey="warningDifferentCountryEnd" />
        </div>
		<Corbis:GlassButton CssClass="closeSuccess"  ID="closeSuccessDiffCountry" runat="server"  OnClientClick="javascript:self.close();return false;" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" />
	</Corbis:ModalPopup>
	<Corbis:ContactCorbis runat="server" />
	<Corbis:QuickPicMaxAlert runat="server" />
	
     <!-- Hidden Variables -->
     <input id="mediaUid" runat="server" type="hidden" class="mediaUid" />
     <input id="corbisId" type="hidden" value="<% =CorbisId %>" />
     <asp:HiddenField ID="licenseModelText" runat="server"/>
     <input type="hidden" id="parentQueryString" class="parentQueryString" runat="server" />
     <input type="hidden" id="parentImageList" class="parentImageList" runat="server" />
     <input type="hidden" id="parentPageSize" class="parentPageSize" runat="server" />
     <input type="hidden" id="parentPageNo" class="parentPageNo" runat="server" />
     <input type="hidden" id="lightboxId" class="lightboxId" runat="server" />
     <input type="hidden" id="pageAction" class="pageAction" runat="server" />

     <script language="javascript" type="text/javascript">
		CorbisUI.GlobalVars.Enlargement = {
			isAnonymous : <%= (Corbis.Web.Authentication.Profile.Current.IsAnonymous? "true":"false") %>
		};
		
         var registerPageUrl = '<%=Corbis.Web.UI.SiteUrls.Register %>';
         var registerWindowTitle = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("registerWindowTitle.Text")) %>';
         var isFeedbackRequest = false;
         
         function redirectToRegister() {
            
             try {
                 PricingModalPopupExit();
                 if (window.opener != null && typeof (window.opener) != undefined) {

                     window.blur();

                     window.opener.redirectToRegister();
                     

                 } else {

                 window.open(HttpsUrl + registerPageUrl, registerWindowTitle);
                 }
             } catch (e) {

             window.open(HttpsUrl + registerPageUrl, registerWindowTitle);
       
             }
         }

         function checkSignInAndCountryStatus() {
             if (signInLevelWhenLoaded < 1) {
                 Corbis.Web.UI.Registration.SignInStatus.SignInStateAndCountry(SignInAndCountryStatusSuccess);
             }
         }

         function SignInAndCountryStatusSuccess(result) {
             if (result.signInState == 2) {
                 signInLevelWhenLoaded = 2;
                 if (result.country == countryWhenLoaded) {
                 	refreshEnlargementPage("");
                 } else {
                     OpenModal('registerSuccessDiffCountry');
                 }
             } else {
                 setTimeout("checkSignInAndCountryStatus()", 10000);
             }

         }

         var pageManager = Sys.WebForms.PageRequestManager.getInstance();
         pageManager.add_beginRequest(BeginRequestHandler);
         pageManager.add_endRequest(EndRequestHandler);

         function BeginRequestHandler(sender, args) {
             
             isFeedbackRequest = false;
             if (args.get_postBackElement().id == '<%=lb.ClientID %>') {
                 isFeedbackRequest = true;
             }
            
         }
         function EndRequestHandler(sender, args) {

             if (isFeedbackRequest) {
                 _Validation = null;
                 initValidation();
                 if ($('<%=emailErrorFromServer.ClientID %>').value == 'true') {
                     _Validation.highlightRow(true, $('<%=userEmail.ClientID %>'));
                     _Validation.errorDiv.removeClass('displayNone');
                     var errorItem = '<li elId=' + '<%=userEmail.ClientID %>' + '_error>' + $('<%=userEmail.ClientID %>').getProperty('custom1_message') + '</li>'
                     _Validation.errorTarget.set('html', errorItem);
                     ResizeModal('feedbackPopup');
                 } else {
                     _Validation.reset();
                     HideModal('feedbackPopup');
                     OpenModal('feedbackSuccess');
                 }  
             } 
         }

         function resetServerErrors() {
             $('<%=emailErrorFromServer.ClientID %>').value = 'false';
             _Validation.highlightRow(false, $('<%=userEmail.ClientID %>'));
         }

         function validateComments() {
             var comments = $('<%=comments.ClientID %>');
             var expression = comments.value.length == 0;
             return !expression;
         }

         function validateEmail() {
             var email = $('<%=userEmail.ClientID %>');
             if (email.value == '') {
                 _Validation.highlightRow(true, email);
                 return false;
             } else {
             var result = _Validation.options.regexp.email.test(email.value);
                _Validation.highlightRow(!result, email);
                 return result; 
             }
         }

         function closeFeedBackPopup() {
             _Validation.reset();
             HideModal('feedbackPopup');
         }
         function validateRole() {
             var roleList = $('<%=role.ClientID %>');
             var expression = roleList.selectedIndex != 0;
             _Validation.highlightRow(!expression, roleList);
             return expression;
         }
         
        function getOptionStylesIE() {
            if (Browser.Engine.trident) {
                var selectControl = $('<%= role.ClientID %>');
                selectControl.setStyles({
                    'border': '0px solid Transparent',
                    'color': '#333333',
                    'fontSize': '12px',
                    'textAlign': 'left',
                    'lineHeight': '18px',
                    'width': '100%'
                });
            }
        }
         
         window.addEvent('domready', function () { getOptionStylesIE() });
         
     </script>
</asp:Content>
