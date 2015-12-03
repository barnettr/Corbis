<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoveLightbox.ascx.cs" Inherits="Corbis.Web.UI.Lightboxes.MoveLightbox" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<Corbis:ModalPopup ID="moveLightbox" runat="server" ContainerID="moveLightbox" Width="350" CloseScript="closeModal(); return false;" meta:resourcekey="MoveHeader">
	<div class="instructions" id="instructions"><Corbis:Localize ID="instructions" runat="server" meta:resourcekey="MoveLimit" /></div>
	<div id="errorblock" class="errorblock" style="display:none"><asp:Image id="errorbimg" ImageUrl="~/Images/iconError.png" runat="server" /> <Corbis:Localize ID="error"  runat="server" meta:resourcekey="SelectoneError" /> </div>
	<div class="ModalPopupContent">
        <div class="clear"></div>
         <table id="moveFields" cellpadding="0" cellspacing="0" width="350px">
			<tr class="moveRow" id="moveRow">
				<td class="moveto"><Corbis:Localize  ID="Localize1" runat="server" meta:resourcekey="Moveto" /></td>
				<td class ="FormRight" ><asp:DropDownList ID="ddlLightbox" EnableViewState = "true" runat="server" />
			    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateDdl" ControlToValidate="ddlLightbox" EnableClientScript="true"></asp:CustomValidator>
			</td></tr>
		  </table>
	</div>
	<div class="modalButtons">
		<Corbis:GlassButton CssClass="cancelButton" ID="cancelButton" CausesValidation="False" runat="server" ButtonStyle="Gray" ButtonBackground="dbdbdb" meta:resourcekey="cancelButton" OnClientClick="closeModal();return false;" />
		<Corbis:GlassButton CssClass="sendButton" ID="moveButton" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="moveButton" OnClick="moveButton_Click"/>

	</div>  
        <div class="displayNone">
		<asp:TextBox CssClass="refresh" ID="refresh" runat="server" />				
	</div> 
</Corbis:ModalPopup>
<script type="text/javascript">
    function ValidateDdl(val, args) {
        $('errorblock').style.display = (args.Value == "0") ? "block" : "none";
        $('instructions').style.display = (args.Value == "0") ? "none" : "block";
        $('moveLightbox').getElement('tr.moveRow').style.backgroundColor = (args.Value == "0") ? "#FFFFCD" : ""  ;

        args.IsValid = !(args.Value == "0");
    }           
</script> 


 



