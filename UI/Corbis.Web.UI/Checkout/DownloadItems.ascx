<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadItems.ascx.cs" Inherits="Corbis.Web.UI.Checkout.DownloadItems" EnableViewState="false" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div id="downloadProducts">
	<asp:Repeater ID="products" runat="server" OnItemDataBound="products_ItemDataBound">
		<ItemTemplate>
			<div class="productBlock" imageUid="<%# Eval("ImageUid") %>" offeringType="<%# Eval("OfferingType") %>" corbisId="<%# Eval("CorbisId") %>">
				<div class="rfcdCount<%# Eval("OfferingType").ToString() == Corbis.CommonSchema.Contracts.V1.OfferingType.RFCD.ToString()? "": " hdn" %>"><%# Eval("RfcdImageCount")%></div>
				<div class="productContent">
					<Corbis:CenteredImageContainer IsAbsolute="true" runat="server" ImageID="image" ID="thumbWrap" ImgUrl='<%# Eval("Url128") %>' Size="128" Ratio='<%# Eval("AspectRatio") %>' ToolTip='<%# Eval("CorbisId") + " - " + Eval("Title") %>' />
				</div>
				<asp:DropDownList ID="fileSize" runat="server" CssClass="fileSize" /> 
				<Corbis:GlassButton ID="downloadButton" runat="server" CssClass="downloadbutton" Text='<%# GetLocalResourceObject("download") %>' OnClientClick="javascript:CorbisUI.Order.DownloadImage(this); return false;" />
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
<script language="javascript" type="text/javascript">
	var downLoadText = '<%= this.GetLocalResourceObject("download") %>';
	var pendingText = '<%= this.GetLocalResourceObject("pending") %>';
</script>