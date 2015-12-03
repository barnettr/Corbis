<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LightboxPanel.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.LightboxPanel" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="DeleteLightbox" Src="~/Lightboxes/DeleteLightbox.ascx" %>

<div class="inSearchBuddy">
	<ul class="SB_tabs">
		<li id="SBT_filters" class="SBT_Mylightboxes ON" runat="server">
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
        <asp:DropDownList ID="sortBy" Width="150" runat="server" OnSelectedIndexChanged="sortBy_Changed" AutoPostBack="true">
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
    <div id="SBBX_quickpic" class="SBBX_quickpic hdn"> </div>
<div class="searchBuddyFooter">   </div>
    </div>
   <!-- left column BUDDY end -->
    <asp:HiddenField ID="selectedLightbox" runat="server" />
			
	    <asp:UpdatePanel ID="deleteProductPanel" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:HiddenField ID="selectedProduct" runat="server"/>
        <Corbis:ModalPopup ID="deleteProductModal" ContainerID="modalDeleteTemplate" Width="340" runat="server" meta:resourcekey="modalDeleteTemplate" CloseScript="HideModal('modalDeleteTemplate');return false;">                   
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
                        CssClass="BTN_delete" meta:resourcekey="btnDelete" OnClick="btnDelete_Click" />
                </div>
            </div>
        </Corbis:ModalPopup>
    </ContentTemplate>
</asp:UpdatePanel>
    <Corbis:DeleteLightbox  ID="deleteLightbox" runat="server" OnDeleteLightboxHandler="DeleteLightbox"/>
  <asp:HiddenField ID="HiddenField1" runat="server" />

    
    	<script language="javascript" type="text/javascript">
	    var deleteClicked = false;
	    var activeLightBoxArray=new Array();
	    var selectedProduct = '<%= selectedProduct.ClientID %>';
	    var postbackName = '<%= selectedLightbox.ClientID %>';
	    activeLightBoxArray[0]=document.getElementById('<%= selectedLightbox.ClientID %>').value;
	    TruncateNames();
	    InitializeToggleImage();
	    LoadActiveLightbox();  
        		
    </script>