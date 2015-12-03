<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailLightboxView.aspx.cs" Inherits="Corbis.Web.UI.Lightboxes.EmailLightboxView" EnableEventValidation="false" MasterPageFile="~/MasterPages/MasterBase.Master" %>

<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Products" Src="~/Lightboxes/LightboxProducts.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultHeader" Src="../Search/SearchResultHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="SearchResultFooter" Src="../Search/SearchResultFooter.ascx" %>

<asp:Content ID="LbContent" ContentPlaceHolderID="mainContent" runat="server">
    <div id="lightboxesContent">
   	<!-- $$$ [ new MyLBox buddy starts here ]-->
		<div class="wrap searchContent">
			<div id="SearchColumnedContent" class="EmailcolumnLayout ">
				<div class="leftColumnWrap">
					<div class="rightColumnWrap">
						<div class="rightColumn">
						    <!-- LBox RESULTS CONTENT start -->
						    <div class="searchResultsContent">
						            						              
    	                     		<div id="DetailsViewSide" class="DetailsViewSide rounded4" style="margin-top:0"   isbottomonly="false" isInternal="false">                            
                                          <!-- LBOX Header content - Light Box name and Details-->
                                             
                                       
                                        <div class="LightboxNameInHeader" style="font-size:18px;">
                                            <span class="LightboxNameSpan"><asp:Literal ID="LightboxNameTitle" runat="server" Text=asdfg /></span>
		    	                        </div>
		    	                         <div class="seperatorDottedLine"></div>
                                         <div class="DetailHeader" >
		    	                            <corbis:label ID="detail" runat="server"  meta:resourcekey="Details"  />
                                         </div>
		    	                        <table id="header" cellspacing="0" style="background-color: #4d4d4d; width:100%;" >
		    	                        <tr>
		    	                        <td width="75%" >
		    	                          <div class="ClientName">
		    	                            <corbis:label ID="lbClientName" runat="server"  meta:resourcekey="Client"  /><asp:Label ID="ClientName" runat="server" CssClass="text1" />
                                          </div>
                                          <hr style="background-color:#333333;color:#333333;height:2px;"/> 
                                            <table ><tr><td>
                                            <div class="NoteName" >
                                                <corbis:label ID="lbNote" runat="server" meta:resourcekey="Note" />
                                            </div> 
                                            </td><td width="10px"></td>
                                            <td width="95%">
                                            <div class="text">
                                                <asp:Label ID="Note" runat="server"  />
                                            </div>
                                            </td></tr></table>
		    	                        </td>
		    	                        <td style="border-left:10px solid #333">
		    	                            <div class="headertitles">
		    	                              <corbis:label ID="lbModified" runat="server" meta:resourcekey="Modified" /><asp:Label ID="Modified" runat="server" CssClass="text2" />
                                            </div>
                                            <div class="headertitles">
                                              <corbis:label ID="lbCreated" runat="server"  meta:resourcekey="Created" /><asp:Label ID="Created" runat="server" CssClass="text2"  />
                                              <div class="seperatorDottedLine1"></div>
                                            </div>
                                            <div class="headertitles">  
                                              <corbis:label ID="lbOwner" runat="server" meta:resourcekey="Owner" /><asp:Label ID="Owner" runat="server" CssClass="text2" />
                                            </div>
                                           
		    	                        </td>
		    	                        </tr>
		    	                       
		    	                        </table>
		    	                       <!-- Separator -->
    	                                <div class="LightboxDetails">
                                               <div>
			                                        <div class="right">
				                                        <div class="center"><span>&nbsp;</span></div>
		    	                                    </div>
	    	                                   </div>
    	                                </div>    	                                
    	                            </div>    	                         
    	                            <!-- LBOX - Header Pagination-->

                                <div id="Details" runat="server" class="DetailsView">
									<asp:HiddenField ID="lightboxId" runat="server" />
                                    <asp:HiddenField ID="lightboxUid" runat="server" />
                                    <asp:HiddenField ID="lightboxNameHid" runat="server" />
                                    <asp:UpdatePanel ID="detailViewUpdate" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <input ID="refreshLightboxDetails" runat="server" class="hdn" type="submit" value="" onserverclick="GetLightboxDetails" />
                                            <asp:HiddenField ID="selectedLightbox" runat="server" OnValueChanged="GetChangedLightboxDetails" />
										    <Corbis:SearchResultHeader ID="searchResultHeader" runat="server" ShowHeader="false" OnPageCommand="searchResult_PageCommand"
											    OnPageSizeCommand="searchResultHeader_PageSizeCommand" />
										    <Corbis:Products ParentPage="Lightbox" ID="lightboxProducts" runat="server" />									
										    <Corbis:SearchResultFooter ID="searchResultFooter" runat="server" OnPageCommand="searchResult_PageCommand" />
									    </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="refreshLightboxDetails" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    
                                </div>

                        </div>
					    <!-- SEARCH RESULTS CONTENT end -->
						 	</div>
					</div><!-- END of Right Column Wrap -->
        
    </div>
                  </div>
              </div>
    </div>
    <input type="hidden" id="selectedCorbisId" />
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
    <script language="javascript" type="text/javascript">
		CorbisUI.GlobalVars.Lightbox = {
			refreshButton: '<%= refreshLightboxDetails.ClientID %>',
			postbackName: '<%= selectedLightbox.ClientID %>',
			lightboxName: '<%= lightboxNameHid.ClientID %>',
			lightboxId: '<%= lightboxId.ClientID %>'
		};
		
		var deleteClicked = false;
	    var activeLightBoxArray=new Array();
	    activeLightBoxArray[0]=document.getElementById('<%= selectedLightbox.ClientID %>').value;
	    
    </script>
</asp:Content>
