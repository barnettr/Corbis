<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicenseDetail.ascx.cs" Inherits="Corbis.Web.UI.Checkout.LicenseDetail" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div class="licenseDetailHeader">
	<div class="licenseDetailsImageThumb">
		<img id="imageThumb" runat="server"/>
	</div>
	<span class="licenseDetailHead">
		<Corbis:Localize ID="imagePCT" runat="server"/>
		<Corbis:Localize ID="imageID" runat="server"/><br />
	</span>
	<span id="licenseModelText" runat="server"><Corbis:Localize ID="imageType" runat="server" /></span><br />
	<span class="rfcdTitle normal"><Corbis:Localize ID="imageCaption" runat="server" /></span><br />
	<span class="normal"><h3><Corbis:Localize ID="imagePrice" runat="server" /></h3></span>
	<div class="clr"></div>
</div>
<table class="orderSummaryTables t100" cellspacing="2">
	<tbody>
		<asp:Repeater ID="licensingDetailsRepeater" runat="server">
			<ItemTemplate>
				<tr>
					<td class="leftLabels">
						<p>
							<%# Eval("Key") %>
						</p>
					</td>
					<td>
						<p>
							<%# Eval("Value") %>
						</p>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater>
	</tbody>
</table>
<div class="licenseImageDetails">
	<p id="restrictionTitle" runat="server" class="restrictionsTitle"><Corbis:Localize ID="restrictionsTitle" runat="server" meta:resourcekey="restrictionsTitle"/></p>
	<div class="restrictionContent">
		<img src="/images/spacer.gif" alt="" title="" id="pricingIcon" runat="server" />
		<p id="releaseDetails" runat="server">
			<Corbis:Localize ID="modelReleased" runat="server" /><br />
			<Corbis:Localize ID="propertyReleased" runat="server" /><br />
			<Corbis:Localize ID="domEmbargoDate" runat="server" />
			<Corbis:Localize ID="intEmbargoDate" runat="server" />
			<br />
		</p>
		<div id="restrictionsHolder" runat="server" class="restrictionDiv">
			<ul class="restrictionsList">
				<asp:Repeater ID="restrictionsRepeater" runat="server">
					<ItemTemplate>
						<li><%# Eval("Notice")%></li>
					</ItemTemplate>
				</asp:Repeater>                                                            
			</ul>
		</div>
		<p id="moreRestrictions" runat="server"><a class="restrictionsToggler" href="javascript:void(0)"><Corbis:Localize ID="moreRestrictionsLink" runat="server" meta:resourcekey="moreRestrictionsLink"/></a></p>
		<br />
		<div class="sizeFootNote" id="sizeFoootNoteDiv" visible="false" runat="server"><Corbis:Localize ID="sizeFootNote" runat="server" meta:resourcekey="sizeFootNote"/></div>
	</div>
</div>

