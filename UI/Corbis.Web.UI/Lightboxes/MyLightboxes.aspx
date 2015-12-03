<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MyLightboxes.aspx.cs" Inherits="Corbis.Web.UI.Lightboxes.MyLightboxes"
    EnableEventValidation="false" MasterPageFile="~/MasterPages/MasterBase.Master" EnableViewState="true"  %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="DeleteLightbox" Src="~/UserControls/LightboxControls/DeleteLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="CreateLightbox" Src="~/UserControls/LightboxControls/CreateLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Products" Src="~/Lightboxes/LightboxProducts.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicItems" Src="~/Search/QuickPicItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultHeader" Src="../Search/SearchResultHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultFooter" Src="../Search/SearchResultFooter.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="EmailLightbox" Src="~/UserControls/LightboxControls/EmailLightbox.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RFFileSize" Src="~/CommonUserControls/RFFileSizes.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="QuickPicMaxAlert" Src="~/UserControls/ModalControls/QuickPicMaxAlert.ascx" %>

<asp:Content ID="LbContent" ContentPlaceHolderID="mainContent" runat="server">
    <script TYPE='text/javascript' src='MoveLightbox.js'></script>
    <div id="lightboxesContent">
   	<!-- $$$ [ new MyLBox buddy starts here ]-->
		<div class="wrap searchContent">
			<div id="SearchColumnedContent" class="columnLayout twoColumn ">
				<div class="leftColumnWrap">
					<div class="rightColumnWrap">
						<div class="rightColumn">
						    <!-- LBox RESULTS CONTENT start -->
						    <div class="searchResultsContent">
						            <!-- LBOX Header content - Create Light Box header-->
									<asp:UpdatePanel ID="HeaderPanel" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate >    
                                          <div class="DetailsViewSideTop" id="DetailsViewSideTop" style="margin-top:0">
                                                <div class="DetailsViewSideLeft"></div>
                                                <div id="spacer" class="menuButtons menuRight" style="margin-right: 10px;"></div>
					                            <div id="coffItemsButtonDiv" class="menuButtons menuRight" runat="server" >
												    <Corbis:LinkButton ID="coffItemsButton" OnClientClick="javascript:CorbisUI.Lightbox.Handler.validateSelectedItemsForCoff();return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="coffItemsButton" />
											    </div>
												<div id="transferButton" class="menuButtons menuRight">
													<Corbis:LinkButton ID="transferButton" OnClientClick="javascript:CorbisUI.Lightbox.Handler.openTransferModal(this); return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="transferButton" /> 
												</div>            
												<div id="emailButton" class="menuButtons menuRight">
													<Corbis:LinkButton ID="emailButton" OnClientClick="LogOmnitureEvent('event8');javascript:CorbisUI.Lightbox.Handler.emailLightbox(); return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="emailButton" /> 
												</div>            
												<% // <div id="printButton" class="menuButtons menuRight">
													//<Corbis:LinkButton ID="printButton" OnClientClick="alert('Print to be implemented');return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="printButton" /> 
												//</div>%>
												<div id="copyItemsButtonDiv" class="menuButtons menuRight" runat="server">
													<Corbis:LinkButton ID="copyItemsButton" OnClientClick="CorbisUI.Lightbox.Handler.copyLightboxItems();return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="copyItemsButton" /> 
												</div>            
												<div id="NewLightboxButton" class="menuButtons menuLeft" style="margin-left: 10px;">
													<Corbis:LinkButton ID="createNew" OnClientClick="LogOmnitureEvent('event7');newLightbox();return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="createNew" /> 
												</div>  
												<div id="moveButton" class="menuButtons menuLeft">
													<Corbis:LinkButton ID="moveLink" OnClientClick="javascript:CorbisUI.Lightbox.Handler.moveLightbox();return false;" runat="server" CssClass="menuLinkButton" meta:resourcekey="moveButton" />													
												</div>  
                                                <div class="DetailsViewSideRight"></div>
                                          </div>
						                <!-- LBOX Header content - Light Box name and Details-->
    	                     			<div id="DetailsViewSide" class="DetailsViewSide rounded4" style="margin-top:0"  isbottomonly="true" isInternal="false">                            
										   <div class="LightboxNameInHeader">
												<span class="LightboxNameSpan"><asp:Literal ID="LightboxNameTitle" runat="server" /></span><a class="renameLink" href="javascript:void(0)" onclick="javascript:CorbisUI.Lightbox.Handler.lightboxNameEdit(true); return false;"><Corbis:Localize ID="RenameLabel" runat="server" meta:resourcekey="renameText" /></a>
												<div id="lightboxNameEdit" class="hdn">
													<input type="text" id="newLightboxName" maxlength="40" onkeypress="return WebForm_FireDefaultButton(event, '<%=renameLightbox.FindControl("GlassButton").ClientID %>');"/>
													<Corbis:GlassButton ID="renameLightbox" runat="server" ButtonStyle="Orange" ButtonBackground="gray36" EnableViewState="false" meta:resourcekey="renameLightbox" OnClientClick="javascript:CorbisUI.Lightbox.Handler.renameLightbox(); return false;" />
													<Corbis:GlassButton ID="cancelRename" runat="server" ButtonStyle="Gray" ButtonBackground="gray36" EnableViewState="false" meta:resourcekey="cancelButton" OnClientClick="javascript:CorbisUI.Lightbox.Handler.lightboxNameEdit(false); return false;" />													
												</div>
		    								</div>
		    								<div id="lightboxDetailsEdit" >
    	                                     <div class="seperatorDottedLine"></div>
                                             <div class="DetailHeader">
    	                                        <corbis:label ID="detail" runat="server"  CssClass="lblDetails" meta:resourcekey="Details" /> 
    	                                        <span class="actLikeLink" onclick="showNotes(this);" ><Corbis:Label  CssClass="showLink"  ID="show" runat="server" meta:resourcekey="show" />
    	                                        <Corbis:Label  CssClass="hideLink"  ID="hide" runat="server" meta:resourcekey="hide" /></span>
    	                                        <span class="bulletPoint">&bull;</span><span class="actLikeLink" onclick="showEdit(this);" >  <Corbis:Label  CssClass="editLink" id="edit" runat="server"  meta:resourcekey="edit" /></span>
    	                                        
                                             </div>
                                             <div id="detailStructure" ><asp:TextBox ID="notesUid" CssClass="notesUid" runat="server" style="display:none" />
                                                    <table cellspacing="0" class="details">
                                                           <tr>
                                                             <td class="clientName"  >
	                                                           <corbis:label ID="lbClientName" runat="server"  meta:resourcekey="Client"   />
	                                                        </td>
	                                                        <td class="clientData">
	                                                             <asp:Label ID="ClientName" runat="server" CssClass="lblName"  />
	                                                             <asp:TextBox ID="txtClientName" MaxLength="100" runat="server" CssClass="txtName"/>
	                                                        </td>
	                                                        <td rowspan="3" class="sectionSpacing">
	                                                        </td>
	                                                        <td rowspan="3" class="fineDetailsWrap">
																<div class="fineDetails">
    																<div class="fineDetailsCell"> 
    																	<div class="headertitles">
    																		<corbis:label ID="lbModified" runat="server" meta:resourcekey="Modified" />
    																		<asp:Label ID="Modified" runat="server" CssClass="textModified" />
																		</div>
																		<div class="headertitles">
																			<corbis:label ID="lbCreated" runat="server"  meta:resourcekey="Created" />
																			<asp:Label ID="Created" runat="server" CssClass="text2"  />
																			<div class="seperatorDottedLine1"></div><br />
																		</div>
																  </div>
																  <div class="fineDetailsCell">
																	  <div class="headertitles1">  
																			  <corbis:label ID="lbOwner" runat="server" meta:resourcekey="Owner" />
																			  <asp:Label ID="Owner" runat="server" CssClass="text2" />
																		</div>
																		<div class="headertitles">  
																				  <corbis:label ID="lbShared" runat="server" meta:resourcekey="Shared" CssClass="sharedCss" />
																			  <asp:Label ID="SharedBy" runat="server" CssClass="text2 SharedByPeople">
		                                                                      
																			  </asp:Label>
		    															</div>
		    														</div>
		    													</div>    	                                   
	                                                        </td>
	                                                        </tr>
                                                            <tr>
	    	                                                        <td colspan="2" style="height:2px;background-color:#313031;">
                                                                   </td>
                                                            </tr>
                                                           <tr>
                                                            <td class="noteName" >
                                                                <corbis:label ID="lbNote" runat="server" meta:resourcekey="Note" />
                                                             </td>
                                                              <td class="noteData">
                                                                <Corbis:TextArea ID="Note1" runat="server" TextMode="MultiLine" CssClass="lblNotes" ReadOnly="true"  />
                                                                 <Corbis:TextArea ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="txtNote" MaxLength="2000" />
                                                             </td>
                                                            </tr>
                                                           
                                                        </table>
		    	                                  </div>
		    	                                </div>
		    	                                <div class="LightboxDetails hdn" id="buttonsCenter">
															 <div Class="notesbuttonCSS" id="notesGlassButton" >
		    	                                                <Corbis:GlassButton ID="notesSave" runat="server" ButtonStyle="Orange" ButtonBackground="gray36" EnableViewState="false" meta:resourcekey="renameLightbox" OnClientClick="javascript:CorbisUI.Lightbox.Handler.editNotesLightbox(); return false;" />
													            <Corbis:GlassButton ID="notesCancel" runat="server" ButtonStyle="Gray" ButtonBackground="gray36" EnableViewState="false" meta:resourcekey="cancelButton" OnClientClick="javascript:CorbisUI.Lightbox.Handler.lightboxNotesCancel(); return false;" />													
                                                            </div>
															
															</div>
    									</div>  	                         
	    	                            <!-- LBOX - Header Pagination-->
    								</ContentTemplate>
    	                         </asp:UpdatePanel>

                            

                                <div id="Details" runat="server" class="DetailsView">
                                    <asp:UpdatePanel ID="detailViewUpdate" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
											<asp:HiddenField ID="lightboxUid" runat="server" />
                                            <input ID="refreshLightboxDetails" runat="server" class="hdn" type="submit" value="" onserverclick="GetLightboxDetails" />
                                            <asp:HiddenField ID="selectedLightbox" runat="server" OnValueChanged="GetChangedLightboxDetails" />
										    <Corbis:SearchResultHeader ID="searchResultHeader" runat="server" ShowHeader="false" OnPageCommand="searchResult_PageCommand"
											    OnPageSizeCommand="searchResultHeader_PageSizeCommand" />
										    <Corbis:Products ParentPage="Lightbox" ID="lightboxProducts" runat="server" EnableViewState="false" />
										    <Corbis:SearchResultFooter ID="searchResultFooter" runat="server" OnPageCommand="searchResult_PageCommand" />
									    </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="refreshLightboxDetails" />
                                            <asp:AsyncPostBackTrigger ControlID="deleteProductModal" />                                           
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    
                                </div>

                        </div>
					    <!-- SEARCH RESULTS CONTENT end -->
						 	</div>
					</div><!-- END of Right Column Wrap -->
					


					
					  <!-- left columnBUDDY start -->
					  <div class="leftColumn">
				 	  <div  class="SearchBuddy" id="SearchBuddy">
							<div class="inSearchBuddy">
								<ul class="SB_tabs">
									<li id="SBT_filters" class="SBT_lightboxes ON" runat="server">
										<img src="../Images/spacer.gif" alt="" /></li>
									<li id="SBT_quickpic" class="SBT_quickpic" runat="server">
										<img src="../Images/spacer.gif" alt="" /></li>
								</ul>
								<div id="SBBX_filters" class="SBBX_filters">
								<div id="refineHeaderDiv">
								<span>&nbsp; </span></div>
								<div id="refineHeader">
										<Corbis:Localize ID="mylightboxTab" runat="server" meta:resourcekey="mylightboxTab" />
							  </div>                      
                                  
                                <div id="SortByDiv"><br />
				                    <asp:DropDownList ID="sortBy" CssClass="SortBy" name="sortBy" Width="150" runat="server" OnSelectedIndexChanged="sortBy_Changed" AutoPostBack="true">
					                    <asp:ListItem Value="date" meta:resourcekey="sortByDate" />
					                    <asp:ListItem Value="name" meta:resourcekey="sortByName" />
                                    </asp:DropDownList>
                                </div>
                                    <div id="LBXContainer" class="LBXContainer">
                                    <asp:UpdatePanel ID="deleteLightboxPanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="Tree">
                                                <Corbis:LightboxTree ID="lightboxTree" runat="server" EnableViewState="false" meta:resourcekey="lightboxTree" />
                                                <div id="highLight" class="HightlightColor"></div>
                                                <div id="empty" runat="server" class="Empty">
                                                    <Corbis:Localize ID="noLightboxes" runat="server" meta:resourcekey="noLightboxes"></Corbis:Localize>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="deleteProductModal" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                                <!-- not implement Quick pic yet -->
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
										<Corbis:Localize ID="quickPicLabel" runat="server" meta:resourcekey="quickPicText" />
									</div>
									<div class="QPDownloadPositioner">
										<span class="actLikeLink" onclick="CorbisUI.Handlers.Quickpic.downloadAll();" id="quickPicDownloadAllLink"><Corbis:Localize ID="downloadAll" runat="server" meta:resourcekey="downloadAll" /></span>
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
                            <div class="searchBuddyFooter">   </div>
                    </div>
                   <!-- left column BUDDY end -->
			</div>
		</div>
        
    </div>
    
                  </div>
              </div>
    </div>

    <input type="hidden" id="selectedCorbisId" />
    <Corbis:DeleteLightbox ID="deleteLightbox" runat="server" OnDeleteLightboxHandler="DeleteLightbox"/>
	<Corbis:CreateLightbox id="createLightbox" runat="server" OnCreateLightboxHandler="CreateLightbox" />
	<Corbis:AddToCart ID="addToCartControl" runat="server" />
    <asp:UpdatePanel ID="deleteProductPanel" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="selectedProduct" runat="server"/>
            <Corbis:ModalPopup ID="deleteProductModal" ContainerID="modalDeleteTemplate" Width="353" runat="server" meta:resourcekey="modalDeleteTemplate" CloseScript="HideModal('modalDeleteTemplate');return false;">                   
                <div class="modalDeleteThumb">
                    <div class="thumbWrap Relative">
                        <div class="pinkyThumb" style="margin-left:0px">
                        </div>
                    </div>
                    <div class="clr">
                    </div>
                </div>
                
                <div class="modalDeleteMessage">
                    <Corbis:Localize ID="modalDeleteMessage" runat="server" meta:resourcekey="modalDeleteMessage" />
                    <div class="modalDeleteButtons">
                        <Corbis:GlassButton runat="server" 
                            ButtonStyle="Gray" 
                            ID="btnCancel" 
                            OnClientClick="HideModal('modalDeleteTemplate');return false;" 
                            CssClass="BTN_cancel" meta:resourcekey="noThanks" />
                        <Corbis:GlassButton 
                            runat="server" 
                            ID="btnDelete" 
                            CssClass="BTN_delete" meta:resourcekey="btnDelete" OnClick="btnDelete_Click" style="margin-left:0;" />
                    </div>
                </div>
	        </Corbis:ModalPopup>
	    </ContentTemplate>
    </asp:UpdatePanel>
    <Corbis:ModalPopup ID="addEditNoteModal" ContainerID="modalAddEditNoteTemplate" Width="340" runat="server" CloseScript="HideModal('modalAddEditNoteTemplate');return false;">                   
        <div class="modalDeleteThumb">
            <div class="thumbWrap Relative">
                <div class="pinkyThumb" style="margin-left:0px">
                </div>
            </div>
            <div class="clr">
            </div>
        </div>
        
        <div class="modalNoteMessage">
            <div class="modalNoteButtons">
                <textarea id="noteText" rows="5"></textarea>
                <Corbis:GlassButton runat="server" 
                    ButtonStyle="Gray" 
                    ID="btnNotecancel" 
                    OnClientClick="HideModal('modalAddEditNoteTemplate');return false;" 
                    CssClass="btnNotecancel" meta:resourcekey="btnNotecancel" />
                <Corbis:GlassButton 
                    runat="server" 
                    ID="btnNoteSave" 
                    OnClientClick="(new CorbisUI.Lightbox.ProductBlock($('selectedCorbisId').value)).updateNote($('noteText').value);return false;"
                    CssClass="btnNoteSave" meta:resourcekey="btnNoteSave"/>
            </div>
        </div>
    </Corbis:ModalPopup>
    <Corbis:ModalPopup ID="deleteNoteModal" ContainerID="modalDeleteNoteTemplate" Width="300" runat="server" CloseScript="HideModal('modalDeleteNoteTemplate');return false;" meta:resourcekey="deleteNoteModal">
        <Corbis:GlassButton ID="cancel" runat="server" CausesValidation="false" ButtonStyle="Gray" EnableViewState="false" meta:resourceKey="Cancel" OnClientClick="HideModal('modalDeleteNoteTemplate');return false;" />
		<Corbis:GlassButton ID="delete" runat="server" CausesValidation="false" EnableViewState="false" meta:resourceKey="Delete" OnClientClick="(new CorbisUI.Lightbox.ProductBlock($('selectedCorbisId').value)).deleteNote(); return false;" />
    </Corbis:ModalPopup>    
    <Corbis:EmailLightbox ID="EmailLightboxModal" runat="server" />
    <Corbis:ModalPopup ID="moveLightbox" runat="server" ContainerID="moveLightbox" Width="350" CloseScript="closeModal(); return false;" meta:resourcekey="MoveHeader">
	    <div class="instructions" id="mbInstructions"><Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="MoveLimit" /></div>
	    <div class="ModalPopupContent">
            <div class="clear"></div>
             <table id="moveFields" cellpadding="0" cellspacing="0" width="350px">
			    <tr class="moveRow" id="moveRow">
				    <td class="moveto"><Corbis:Localize  ID="Localize2" runat="server" meta:resourcekey="Moveto" /></td>
				    <td class ="FormRight" >
				    <select id="ddlLightBox" name="ddlLightBox" onchange="setGlassButtonDisabled($(CorbisUI.GlobalVars.Lightbox.moveGlassButton), this.value=='0');">
				        <option value="0"><Corbis:Localize  ID="Localize3" runat="server" meta:resourcekey="Selectone" /></option>
				        <option value="1"><Corbis:Localize  ID="Localize4" runat="server" meta:resourcekey="MovetoTop" /></option>
				    </select>				    
			    </td></tr>
		      </table>
	    </div>
	    <div class="modalButtons">
		    <Corbis:GlassButton CssClass="cancelButton" ID="cancelButton" CausesValidation="False" runat="server" ButtonStyle="Gray" ButtonBackground="dbdbdb" meta:resourcekey="cancelButton" OnClientClick="closeModal();return false;" EnableViewState="false" />
		    <Corbis:GlassButton CssClass="sendButton" ID="sendButton" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="moveButton" OnClientClick="moveLightBox();return false;" Enabled="false"/>		    
	    </div>  
            <div class="displayNone">
		    <asp:TextBox CssClass="refresh" ID="refresh" runat="server" />				
	    </div> 
    </Corbis:ModalPopup>   
    <Corbis:ModalPopup ID="copyProduct" runat="server" ContainerID="copyProduct" Width="600" meta:resourcekey="copyProduct">
		<div class="instructions"><Corbis:Localize ID="instructions" runat="server" meta:resourcekey="instructions" /></div>
		<div class="copyPanel">
			<div class="copyControls">
				<span class="floatLeft"><Corbis:Localize ID="copyTo" runat="server" meta:resourcekey="copyTo" /></span>
				<Corbis:DropDownList ID="copyToLightbox" runat="server" PromptText="<%$ Resources:Resource, SelectOne %>" EnableViewState="false" onchange="CorbisUI.Lightbox.CopyImages().setLinkedControls();" />
				<Corbis:GlassButton CssClass="copyItemsModalButton" ID="copyItemsModalButton" runat="server" ButtonBackground="e8e8e8" meta:resourcekey="copyItemsButton" EnableViewState="false" OnClientClick="CorbisUI.Lightbox.CopyImages().copySelectedItems(); return false;" />
			</div>
			<div class="countSelectAll">
				<div class="itemCount"><span id="itemCount"></span>&nbsp;<Corbis:Localize ID="items" runat="server" meta:resourcekey="items" /></div>
				<div class="selectAllDiv"><a id="A1" class="selectAllLink" href="javascript:void(0)" onclick="CorbisUI.Lightbox.CopyImages().toggleSelectAll()" runat="server" meta:resourcekey="selectAll"><Corbis:Localize ID="selectAllText" runat="server" meta:resourcekey="selectAllText" /></a></div>
			</div>
		</div>
		<div id="lightboxImages" class="lightboxImages"></div>
    </Corbis:ModalPopup>
    
<div id="searchProgIndicator" onclick="CorbisUI.ExtendedSearch.hideMSOWindow(true)">
    <div class="mask"></div>
    <div id="searchProgContents">
	    <img border="0" alt="" src="/images/ajax-loader2.gif" />
	    <br />
	    <h1><Corbis:Label ID="searching" runat="server" meta:resourcekey="searching" /></h1>
	    <div id="processingFilters">
	        <Corbis:Label ID="procFilters" runat="server" meta:resourcekey="procFilters" />
	    </div>
    </div>
</div>
    
<div id="downloadProgress" style="display: none;">
	<img border="0" alt="" src="/images/ajax-loader2.gif" />
	<br />
	<div class="standBy"><Corbis:Localize ID="Localize16" runat="server" meta:resourcekey="standByMessage" /></div>
</div>	
    
<div id="coffProgress1" style="display: none;">
	<img border="0" alt="" src="/images/ajax-loader2.gif" />
	<br />
	<div class="standBy"><Corbis:Localize ID="Localize14" runat="server" meta:resourcekey="standByMessage" /></div>
	<br />
	<div class="copyMessage"><Corbis:Localize ID="Localize15" runat="server" meta:resourcekey="coffCheckoutMessage" /></div>
</div>	
    
	<div id="copyProgress" style="display: none;">
		<img border="0" alt="" src="/images/ajax-loader2.gif" />
		<br />
		<div class="standBy"><Corbis:Localize ID="standByMessage" runat="server" meta:resourcekey="standByMessage" /></div>
		<div class="copyMessage"><Corbis:Localize ID="copyMessage" runat="server" meta:resourcekey="copyMessage" /></div>
	</div>	
	<Corbis:ModalPopup ID="copySuccess" ContainerID="copySuccess" runat="server" meta:resourcekey="copySuccess">
		<Corbis:GlassButton CssClass="closeButton" ID="closeSuccessModal" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" OnClientClick="javascript:HideModal('copySuccess'); return false;"/>
	</Corbis:ModalPopup>
    <Corbis:ModalPopup ID="errorModal" ContainerID="errorModal" Width="372" runat="server" CloseScript="HideModal('errorModal');return false;">
		<div class="errorMessage">
		</div>
		<div class="contactMessage"><Corbis:Localize ID="contactMessage" runat="server" /></div>
		<Corbis:GlassButton ID="closeErrorModal" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('errorModal');return false;" CssClass="closeButton" />
    </Corbis:ModalPopup>
    <Corbis:ModalPopup ID="transferLightbox" runat="server" ContainerID="transferLightbox" Width="600" meta:resourcekey="transferLightbox" CloseScript="CorbisUI.Lightbox.transferLightbox().hideModal(false);return false;">
		<div class="instructions"><Corbis:Localize ID="transferMessage" runat="server" meta:resourcekey="transferMessage" /></div>
		<div ID="ErrorSummaryPanel" class="ValidationSummary displayNone">
			<ul>
		        
			</ul>
		</div>
		<div class="addMemberPanel">
			<asp:Panel ID="Panel1" CssClass="addMemberControls FormRow" DefaultButton="addMemberButton$GlassButton" runat="server">
				<span class="addMemberHeading"><Corbis:Localize ID="addmember" runat="server" meta:resourcekey="addmember" /></span><br />
				<Corbis:TextBox class="addMemberName" ID="addMemberName" runat="server" EnableViewState="false"/>
				<Corbis:GlassButton ID="addMemberButton" runat="server" ButtonBackground="e8e8e8" meta:resourcekey="addMemberButton" EnableViewState="false" OnClientClick="CorbisUI.Lightbox.transferLightbox().addAssociate(); return false;" />
				<span class="addMemberInstruction"><Corbis:Localize ID="addMemberInstruction" runat="server" meta:resourcekey="addMemberInstruction"/></span>
			</asp:Panel>
		</div>
		<asp:Panel ID="Panel2" CssClass="memberSelect" DefaultButton="transfer$GlassButton" runat="server">
			<div class="corbisMembers">
				<Corbis:Localize ID="corbisMembersLabel" runat="server" meta:resourcekey="corbisMembersLabel" /><br />
				<select id="corbisMembers" disabled="disabled" multiple="multiple" size="8" onchange="javascript:CorbisUI.Lightbox.transferLightbox().updateMemberSelect();">
				</select><br />
				<a class="deleteMembers" href="javascript:void(0);" onclick="CorbisUI.Lightbox.transferLightbox().deleteAssociates(); return false;">
					<Corbis:Localize ID="deleteMembersText" runat="server" meta:resourcekey="deleteMembersText" />			
				</a>
				<div id="noMemberMessage"><Corbis:Localize ID="noMemberMessage" runat="server" meta:resourcekey="noMemberMessage"/></div>
			</div>
			<div class="listControls">
				<input type="submit" id="addTransferTo" disabled="disabled" class="disabled" onclick="javascript:CorbisUI.Lightbox.transferLightbox().moveMembersBewteenLists(true);return false;" value=""/><br/>
				<input type="submit" id="removeTransferTo" disabled="disabled" class="disabled" onclick="javascript:CorbisUI.Lightbox.transferLightbox().moveMembersBewteenLists(false);return false;" value=""/>
			</div>
			<div class="transferTo">
				<Corbis:Localize ID="transferToLabel" runat="server" meta:resourcekey="transferToLabel" /><br />
				<select id="transferTo" disabled="disabled" multiple="multiple" size="8" onchange="javascript:CorbisUI.Lightbox.transferLightbox().updateTransferToSelect();">
				</select><br />
				<Corbis:ImageCheckbox ID="removeFromLightbox" runat="server" meta:resourcekey="removeFromLightbox" />
				<div id="noTransferToMessage"><Corbis:Localize ID="noTransferToMessage" runat="server" meta:resourcekey="noTransferToMessage"/></div>
			</div>
			<div class="clr"></div>
		</asp:Panel>
		<div class="transferButtons">
		    <Corbis:GlassButton CssClass="cancelTransfer" ID="cancelTransfer" CausesValidation="False" runat="server" ButtonStyle="Gray" ButtonBackground="dbdbdb" meta:resourcekey="cancelButton" OnClientClick="CorbisUI.Lightbox.transferLightbox().hideModal(false);return false;" EnableViewState="false" />
		    <Corbis:GlassButton CssClass="transfer" ID="transfer" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="transfer" OnClientClick="CorbisUI.Lightbox.transferLightbox().transferLightbox();" Enabled="false" EnableViewState="false"/>		    
		</div>
    </Corbis:ModalPopup>
	<Corbis:ModalPopup ID="transferSuccess" ContainerID="transferSuccess" runat="server" meta:resourcekey="transferSuccess" CloseScript="javascript:CorbisUI.Lightbox.transferLightbox().hideTransferSuccess(); return false;">
		<Corbis:GlassButton CssClass="closeButton" ID="transferSuccessModal" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" OnClientClick="javascript:CorbisUI.Lightbox.transferLightbox().hideTransferSuccess(); return false;"/>
	</Corbis:ModalPopup>
    <Corbis:COFFModalPopup ID="coffProduct" runat="server" ContainerID="coffProduct" width="850" meta:resourcekey="coffProduct" ItemCountText="<%$ Resources: coffSelectedItemCountText %>">
        <div class="selectCOFFItems">
            <div class="selectCOFFItemsContainer">
                <div class="instructionText">
                    <Corbis:Localize ID="Localize5" runat="server" meta:resourcekey="coffInstructions" />
                </div>
                <div class="controlSection">
                    <div class="controlButton">
                        <Corbis:GlassButton ID="GlassButton2" CssClass="copyItemsModalButton" CausesValidation="False" runat="server" ButtonStyle="Orange" ButtonBackground="e8e8e8" meta:resourcekey="coffSelectedItemsButton" OnClientClick="CorbisUI.Lightbox.CoffImages().validateSelectedItemsForCoff(); return false;" EnableViewState="false" />
                    </div>                    
                    <div class="selectAll"><a id="selectAllLink" class="selectAllLink" href="javascript:void(0)" onclick="CorbisUI.Lightbox.CoffImages().toggleSelectAll()" runat="server" meta:resourcekey="selectAll"><Corbis:Localize ID="Localize12" runat="server" meta:resourcekey="selectAllText" /></a></div>
                </div>
            </div>
            <br />
            <div id="coffLightboxImages" class="lightboxImages"></div>
		    <div id="cancelCOFFDiv">
                <div class="cancelCOFFLink">
                    <a href="javascript:void(0)" class="selectAllLinkNoImage" onclick="CorbisUI.Lightbox.CoffImages().closeLightbox('coffProduct'); return false;"><Corbis:Localize ID="cancelCOFFText" runat="server" meta:resourcekey="cancelCOFF" /></a>
                </div>	                    
                <div class="trusteimageclass">
                    <Corbis:HyperLink NavigateUrl="javascript:CorbisUI.Legal.OpenPolicyIModal();"
                        ID="trustImageCOFF1" Localize="true" ImageUrl="/Images/truste.gif" runat="server" />
                </div>		                
		    </div>
        </div>
    </Corbis:COFFModalPopup>
    
    <Corbis:ModalPopup ID="invalidCoffProducts" runat="server" ContainerID="invalidCoffProducts" Width="850" meta:resourcekey="coffAlert" >
        <div class="containerPanel">
            <div class="messagePanel">
                <div class="alertMessagePanel">
                    <Corbis:Image ID="invalidCoffProductsError" ImageUrl="/Images/iconError.png" runat="server" class="errorImage" meta:resourcekey="invalidCoffProductsError"/>
		            <Corbis:Localize ID="Localize7" runat="server" meta:resourcekey="coffAlertMessage" />
                </div>
                    <div class="rfcdCoffAlert">
    		            <Corbis:Localize ID="Localize9" runat="server" meta:resourcekey="rfcdCoffAlertMessage" />                
                </div>
            </div>
	        <div class="reviewMessage">
	            <div class="reviewMessageText">
	                    <span class="coffAlertFileSizePreText">		    
		                    <Corbis:Localize ID="Localize6" runat="server" meta:resourcekey="reviewMessagePreFileSize" />
                        </span>	        
	                    <span class="coffAlertFileSize">
	                        <a ID="showFileSizeModal" href="javascript:void(0)" runat="server" >
	                        <Corbis:Localize ID="Localize10" runat="server" meta:resourcekey="reviewMessageFileSize" /></a>
	                    </span>
	                    <span class="coffAlertFileSizePostText">
	                        <Corbis:Localize ID="Localize11" runat="server" meta:resourcekey="reviewMessagePostFileSize" />
	                    </span>
	             </div>
	             <div class="controlButton">
    	                <Corbis:GlassButton ID="continueToCheckoutCoffItemsButton" CausesValidation="False" runat="server" ButtonStyle="Orange" CssClass="copyItemsModalButton" ButtonBackground="e8e8e8" meta:resourcekey="continueToCheckout" OnClientClick="CorbisUI.Lightbox.CoffImages().checkoutCoffItems(); return false;" EnableViewState="false" />
                 </div>    	                
	        </div>
		    <div id="invalidCoffItems" class="lightboxImages"></div>
		    <div id="cancelCOFFDiv">
                <div class="cancelCOFFLink"><a href="javascript:void(0)" class="selectAllLinkNoImage" onclick="CorbisUI.Lightbox.CoffImages().closeLightbox('invalidCoffProducts'); return false;"><Corbis:Localize ID="cancelCOFFText2" runat="server" meta:resourcekey="cancelCOFF" /></a></div>
                <div class="trusteimageclass">
                    <Corbis:HyperLink NavigateUrl="javascript:CorbisUI.Legal.OpenPolicyIModal();"
                        ID="trustImageCOFF2" Localize="true" ImageUrl="/Images/truste.gif" runat="server" />
                </div>
		    </div>
		    <div id="InvalidCOFFItemMessage" style="display:none; visibility:hidden;">
		        <Corbis:Localize ID="InvalidCOFFItemMessageText" runat="server" meta:resourcekey="invalidCOFFItemMessage" />
            </div>
	    </div>
    </Corbis:ModalPopup>
	<Corbis:ContactCorbis ID="ContactCorbis1" runat="server" />
	<Corbis:QuickPicMaxAlert runat="server" />
    <div id="blanket">
    </div>
   
    <script language="javascript" type="text/javascript">
   		CorbisUI.GlobalVars.Lightbox = {
		    moveDisabledMessage: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "MoveError").ToString()) %>',
		    deleteDisabledMessage: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "DeleteError").ToString()) %>',
		    notesDisabledMessage: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "NotesError").ToString()) %>',
		    noteshideLink : $('<%= hide.ClientID %>'),
		    txtClient : $('<%= txtClientName.ClientID %>'),
            txtNotes : $('<%= txtNotes.ClientID %>'),
            ClientName : $('<%= ClientName.ClientID %>'),
            note : $('<%= Note1.ClientID %>'),
            notesUid : $('<%= notesUid.ClientID %>'),
            notesCancelGlassButton: $('<%= notesCancel.ClientID %>'),
		    notesSavelGlassButton: $('<%= notesSave.ClientID %>'),
			moveGlassButton: '<%= sendButton.ClientID %>',
			refreshButton: '<%= refreshLightboxDetails.ClientID %>',
			coffButton: '<%=coffItemsButton.ClientID%>',
			selectedProduct: '<%= selectedProduct.ClientID %>',
			postbackName: '<%= selectedLightbox.ClientID %>',
			lightboxUid: '<%= lightboxUid.ClientID %>',
			copyItemsButtonDiv: '<%= copyItemsButtonDiv.ClientID %>',
            continueToCheckoutCoffItemsButtonID: '<%= continueToCheckoutCoffItemsButton.ClientID %>',
			copyItemPageSize: 40, //should be divisible by 5, as there's 5 images per row.
			maxImageCopyPerCall: 150,
			maxImageCOFFPerCall: 1000,
			copyItemsPagesDownloaded: 0,
			copyItemsPages: 0,
			coffItemsButtonDiv: '<%= coffItemsButtonDiv.ClientID %>',			
			coffItemPageSize: 200, //should be divisible by 5, as there's 5 images per row.
			coffItemsPagesDownloaded: 0,			
			coffItemsPages: 0,
            coffItemCountTemplateString: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "coffSelectedItemCountText").ToString()) %>',
			copySuccessTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "copySuccessTemplate").ToString()) %>',
			itemsTemplate: '<%=  Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "lightboxTree.ItemCountFormat").ToString()) %>',
			itemTemplate: '<%=  Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "lightboxTree.OneItemCountFormat").ToString()) %>',
			sidbarWidth: 130,
			errorTitles: new Hash({
				'Default':  '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorTitleDefault").ToString()) %>',
				'CopyLightboxImages': '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorTitleCopyLightboxImages").ToString()) %>',
				'TransferLightbox': '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorTitleTransferLightbox").ToString()) %>'
			}),
			errorBody: new Hash({
				'Default':  '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorBodyDefault").ToString()) %>',
				'CopyLightboxImages': '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorBodyCopyLightboxImages").ToString()) %>',
				'TransferLightbox': '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "errorBodyTransferLightbox").ToString()) %>'
			})
		};
		
		CorbisUI.GlobalVars.SearchResults = {
		    urls: {
		        downloadQuickPic: "<%= Corbis.Web.UI.SiteUrls.QuickPic %>"
		    },
		    text: {
		        addToCartAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "addToCartAlt.Text").ToString()) %>',
		        expressCheckoutAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "expressCheckoutAlt.Text").ToString()) %>',
		        deleteBtnAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "deleteBtnAlt.Text").ToString()) %>',
		        addQuickpicAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "AddToQuickPicAlt.Text").ToString()) %>',
		        removeQuickpicAlt: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Lightboxes/MyLightboxes.aspx", "RemoveFromQuickPicAlt.Text").ToString()) %>'
		    }
		};
		
		var deleteClicked = false;
	    var activeLightBoxArray=new Array();
	    activeLightBoxArray[0]=document.getElementById('<%= selectedLightbox.ClientID %>').value;

	    function newLightbox() {
	        OpenLightbox('createLightboxModalPopup');
	        ResizeModal('createLightboxModalPopup');
	        setTimeout("$('<%=createLightbox.FindControl("lightboxName").ClientID %>').focus();$('<%=createLightbox.FindControl("lightboxName").ClientID %>').select();", 500);
	    }
	    
		<%// This AJAX stuff does not play well in the js file %>
		Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
		Sys.Net.WebRequestManager.add_completedRequest(onComplete);

		function onInvoke(sender, args)
		{
			if (args.get_webRequest().get_url() == "MyLightboxes.aspx")
			{
				CorbisUI.Lightbox.Handler.disablePageInput();
			};
			if (args.get_webRequest().get_url().endsWith("CopyLightboxImages"))
			{
				CorbisUI.Lightbox.Handler.openProgressModal('copyProgress');
			}
			if (args.get_webRequest().get_url().endsWith("ValidateItemsForCOFF"))
			{
				CorbisUI.Lightbox.Handler.openProgressModalOverlay('downloadProgress' , 'coffProduct');
			}
            if( args.get_webRequest().get_url().endsWith("ContinueToCheckoutCOFFItems")){
				CorbisUI.Lightbox.Handler.openProgressModalOverlay('downloadProgress' , 'invalidCoffProducts');
				//CorbisUI.Lightbox.Handler.openProgressModal('coffProgress1' , 'invalidCoffProducts');
			}
			//Cancel to clear client name and note fields so we don't get the 500 error for script characters
			if (!args.get_webRequest().get_url().endsWith("UpdateSharedLightbox") && $('detailStructure').getElement('textarea.txtNote').getStyle('display') != 'none')
			{
				CorbisUI.Lightbox.Handler.lightboxNotesCancel();
			}
		}

		function onComplete(sender, args)
		{
			if (sender.get_webRequest().get_url().endsWith('GetLightboxCopyImages'))
			{		
				CorbisUI.GlobalVars.Lightbox.copyItemsPagesDownloaded++;
			};
			if (sender.get_webRequest().get_url().endsWith('GetLightboxCOFFImages'))
			{		
				CorbisUI.GlobalVars.Lightbox.coffItemsPagesDownloaded++;
			};
			if (sender.get_webRequest().get_url().endsWith("CopyLightboxImages"))
			{
				CorbisUI.Lightbox.CopyImages().imageCopyCallCount--;		
			}
			if (sender.get_webRequest().get_url().endsWith("CoffLightboxItems"))
			{
				CorbisUI.Lightbox.CoffImages().imageCopyCallCount--;
			}
			if (sender.get_webRequest().get_url().endsWith("ValidateItemsForCOFF"))
			{
				HideModal('downloadProgress');
			}
			if( sender.get_webRequest().get_url().endsWith("ContinueToCheckoutCOFFItems")){
				HideModal('downloadProgress');
			}
			//error occurred, so will probably not hit the onPageLoaded eventhandler
			if( sender.get_statusCode() >= 400)
			{
				CorbisUI.Lightbox.Handler.enablePageInput();
			}
			
			
			
		}
		
		<%//Use this for page ajax postbacks %>
		Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(onInitialize);
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(onPageLoaded);

		function onInitialize(sender, args)
		{	
			CorbisUI.Lightbox.Handler.disablePageInput();

			//Hide the edit lightbox stuff, also have to remove the client name and notes parameters, because it is already rolled into the body.
			if (!args.get_request().get_url().endsWith("UpdateSharedLightbox"))
			{
				var noteControl = $('detailStructure').getElement('textarea.txtNote');
				var clientNameControl = $('detailStructure').getElement('input.txtName');
				if (noteControl.getStyle('display') != 'none' )
				{
					CorbisUI.Lightbox.Handler.lightboxNotesCancel();
					var noteId = noteControl.id.replace(/_/g, '%24') + '=';
					var nameId = clientNameControl.id.replace(/_/g, '%24') + '=';
					var itemsToDelete = new Array();
					
					var paramArray = args.get_request().get_body().split('&');
					paramArray.each(function(item, index){
						if (item.contains(noteId) || item.contains(nameId)) itemsToDelete.push(item);
					});
					itemsToDelete.each(function(item){
						paramArray.erase(item);
					});
					
					args.get_request().set_body(paramArray.join('&'));
				}
			}
		}
		
		function onPageLoaded(sender, args)
		{
			CorbisUI.Lightbox.Handler.enablePageInput();

			var postBackSettings = sender._postBackSettings;
			//if lightbox change then we set controls
			if (postBackSettings && postBackSettings.sourceElement && $(postBackSettings.sourceElement).getParent('div.Lightbox'))
			{
				CorbisUI.Lightbox.Handler.setControlStates(CorbisUI.DomCache.get('Tree').getElement('div.Active'));
			}
			 if (Browser.Engine.trident) {
			window.scroll(0,0); //IE7: Search Buddy: SB can become invisible bug 17820
			}
			
			//DetailsViewSide loses the bottom rounded corners on ajax call. So we check and add it if it's missing. 
			if($('DetailsViewSide').getChildren().length < 4)
			{
				var details = $$('div.DetailsViewSide');
				new MooRC(details, { radius: { x: 4, y: 4} });
			}
		}
		
        window.addEvent('load', function() {
            CorbisUI.Lightbox.CoffImages().RegisterToolTips();
        });
		
    </script>
</asp:Content>
