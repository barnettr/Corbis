<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RFCDBrowse.ascx.cs" Inherits="Corbis.Web.UI.Browse.RFCDBrowse" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:HiddenField ID="previousLanguageCode" EnableViewState="true" runat="server" />
<asp:UpdatePanel ID="rfcdCategoryUpdatePanel" runat="server">
	<ContentTemplate>
		<div class="RFCDBrowse Node">
			<h3><Corbis:Localize ID="BrowseCDsTitle" runat="server" meta:resourcekey="BrowseCDsTitle" /></h3>
			<p><Corbis:Localize ID="BrowseCDs" runat="server" meta:resourcekey="BrowseCDs" /></p>
			
			<h4><Corbis:Localize ID="ByAlphabetTitle" runat="server" meta:resourcekey="ByAlphabetTitle" /></h4>
			<ul class="Alphabetical">           
				<li><Corbis:LinkButton ID="A" runat="server" meta:resourcekey="alphabetA" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="B" runat="server" meta:resourcekey="alphabetB" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="C" runat="server" meta:resourcekey="alphabetC" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="D" runat="server" meta:resourcekey="alphabetD" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="E" runat="server" meta:resourcekey="alphabetE" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="F" runat="server" meta:resourcekey="alphabetF" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="G" runat="server" meta:resourcekey="alphabetG" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="H" runat="server" meta:resourcekey="alphabetH" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="I" runat="server" meta:resourcekey="alphabetI" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="J" runat="server" meta:resourcekey="alphabetJ" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="K" runat="server" meta:resourcekey="alphabetK" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="L" runat="server" meta:resourcekey="alphabetL" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="M" runat="server" meta:resourcekey="alphabetM" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="N" runat="server" meta:resourcekey="alphabetN" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="O" runat="server" meta:resourcekey="alphabetO" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="P" runat="server" meta:resourcekey="alphabetP" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="Q" runat="server" meta:resourcekey="alphabetQ" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="R" runat="server" meta:resourcekey="alphabetR" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="S" runat="server" meta:resourcekey="alphabetS" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="T" runat="server" meta:resourcekey="alphabetT" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="U" runat="server" meta:resourcekey="alphabetU" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="V" runat="server" meta:resourcekey="alphabetV" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="W" runat="server" meta:resourcekey="alphabetW" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="X" runat="server" meta:resourcekey="alphabetX" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="Y" runat="server" meta:resourcekey="alphabetY" OnClick="RfcdByAlphabet_Click" /></li>
				<li><Corbis:LinkButton ID="Z" runat="server" meta:resourcekey="alphabetZ" OnClick="RfcdByAlphabet_Click" /></li>
			</ul>
	        <div id="byCategoryContainer" runat="server" class="CategoryContainer">
				<h4><Corbis:Localize ID="Category" runat="server" meta:resourcekey="Category" /></h4>
				<div class="CategoryTree">
					<asp:XmlDataSource ID="xmlDataSourceRfcdByCategory" EnableCaching="false" runat="server" />
					<asp:TreeView ID="CategoryTreeView" runat="server" 
						DataSourceID="xmlDataSourceRfcdByCategory" 
						OnSelectedNodeChanged="CategoryTreeView_OnSelectedNodeChanged" 
						ShowExpandCollapse="false" ExpandDepth="1"> 
						<DataBindings>
							<asp:TreeNodeBinding DataMember="category" TextField="name" valueField="value" />
							<asp:TreeNodeBinding DataMember="categories" TextField="" value="" />
						</DataBindings>
					</asp:TreeView>
				</div>
			</div>
			<div class="EntityContainer">
				<h4><Corbis:Localize ID="CDTitle" runat="server" meta:resourcekey="CDTitle" /></h4>
				<div class="Entities">
					<%--Note: browser will not work if the categoryTitle below is removed--%>
					<h5><Corbis:Label ID="categoryTitle" runat="server" /></h5>
					<asp:Repeater ID="repeaterRfcdEntity" runat="server" OnItemDataBound="RfCdEntity_ItemDataBound">
						<HeaderTemplate><ul></HeaderTemplate>
						<ItemTemplate>
							<li>
								<Corbis:LinkButton id="linkRfcdEntity" runat="server" CommandName='<%# DataBinder.Eval(Container.DataItem,"VolumeNumber") %>'> <%# DataBinder.Eval(Container.DataItem,"Title") %>  <%# !string.IsNullOrEmpty((DataBinder.Eval(Container.DataItem,"BasePrice") == null) ? string.Empty : DataBinder.Eval(Container.DataItem,"BasePrice").ToString()) ? " (" + DataBinder.Eval(Container.DataItem,"BasePrice") + " " +  DataBinder.Eval(Container.DataItem,"BasePriceUnit") + ")" :  "" %> </Corbis:LinkButton>
							</li>
						</ItemTemplate>
						<FooterTemplate></ul></FooterTemplate>
					</asp:Repeater>
				</div>
            </div>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>                 

