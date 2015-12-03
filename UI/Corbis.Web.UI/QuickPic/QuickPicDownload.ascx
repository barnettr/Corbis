<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickPicDownload.ascx.cs" Inherits="Corbis.Web.UI.QuickPic.QuickPicDownload" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Restrictions" Src="../src/Image/Restrictions.ascx" %>
<Corbis:Repeater runat="server" ID="rptQuickPic" OnItemDataBound="rptQuickPic_ItemDataBound">
	<ItemTemplate>
		<div class="DownloadQuickPic" imageUid="<%# Eval("ImageUid") %>" corbisId="<%# Eval("CorbisId") %>">
			<Corbis:HoverButton runat="server" ID="btnClose" CssClass="hoverBtn closeIcon" OnClientClick="CorbisUI.QuickPic.DeleteDownloadQuickPic(this);return false;" />
			<div class="QuickPicDetailsThumbnail">
				<Corbis:CenteredImageContainer ImageID="image" ID="imageThumb" Size="128" IsAbsolute="true" runat="server" />
				<div class='<%# Eval("LicenseModel")+"color infoBox" %>'>
					<div class="license">
						<span><%#  Corbis.Web.UI.CorbisBasePage.GetResourceString("Resource", Eval("LicenseModel") + "Text")%></span>
					</div>
					<div class="corbisId">
						<%# Eval("CorbisId") %>
					</div>
					<asp:DropDownList ID="fileSize" runat="server" CssClass="fileSize" />
				</div>
			</div>
			<div class="QuickPicRestriction">
				<!--- restrictions-->
				<Corbis:Restrictions ID="ImageRestrictions" runat="server"></Corbis:Restrictions>
				<!--end-->
			</div>
		</div>
	</ItemTemplate>
</Corbis:Repeater>
