<%@ Page Language="C#" MasterPageFile="~/MasterPages/AccountsMaster.Master" AutoEventWireup="True" Codebehind="OrderHistory.aspx.cs" Inherits="Corbis.Web.UI.OrderHistory.OrderHistory" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/OrderHistory/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="OrderHistoryHeader" Src="~/OrderHistory/OrderHistoryHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="OrderHistoryFooter" Src="~/OrderHistory/OrderHistoryFooter.ascx" %>


<asp:Content ID="orderHistoryContent" ContentPlaceHolderID="accountsContent" runat="server">
	<div id="orderHistory">
		<div class="OrderHistoryHeading">
			<h2 class="PageTitle">
				<Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" />
			</h2>
			<Corbis:Label ID="expirationText" runat="server" CssClass="ExpirationText" /> 

		    <div class="Printer" >
		        <a onclick="javascript:window.print();" class="PrintButtonRoundedCorners"><Corbis:Localize ID="printPage1" runat="server" meta:resourcekey="printPage" /><span class="top-left"></span><span class="top-right"></span><span class="bottom-left"></span><span class="bottom-right"></span></a>
            </div>
		</div>
		
    <div class="OrderHistoryContent rounded4">
        
        <Corbis:OrderHistoryHeader id="orderHistoryHeader" runat="server" 
            OnPageCommand="PageChanged" OnPageSizeCommand="PageSizeChanged" OnGenericCommand="SortOrderChanged"
            PrevCssClass="PrevButton" NextCssClass="NextButton" 
            PrevDisabledCssClass="PrevButtonDisabled" NextDisabledCssClass="NextButtonDisabled" 
            PageNumberCssClass="NavPageNumber" PrefixLabelCssClass="PagerLabelPreFix" 
            PostfixLabelCssClass="PagerLabelPostFix" />
		     
        <div id="orderHistoryContentMain" class="OrderHistoryContentMain">	     
            <div id="orderHistoryRecords" class="OrderHistoryRecords" runat="server">	       
                <div class="OrderRecordTitle" runat="server" id="orderRecordTitle">
			        <Corbis:Localize ID="OrderRecoredTitle" runat="server"/>
			    </div>

			   		    
			    <asp:Repeater ID="ordersRepeater" runat="server" EnableViewState="false" OnItemDataBound="OrderBound">
			        <ItemTemplate>
				        <div id="ProjectName" class="OrderHistoryRecordProjectName" ><asp:HyperLink ID="hyperProjectName" runat="server" /></div>
				        
                        <div id="Items" class="OrderHistoryRecordsDetailsExpirations" style="line-height:1" >
                            <asp:Label CssClass="OrderDetailsProjectName"  ID="ItemsLabel" runat="server" meta:resourcekey="ItemsLabel" ></asp:Label> <%# DataBinder.Eval(Container.DataItem, "ItemCount")%><br />
                            

                                <div  id="ExpirationsDiv" runat="server">
                                <div id="manDiv" runat="server" style="float:left;padding-top:4px;padding-right:4px;">
                                   <asp:Label CssClass="OrderDetailsProjectName"   ID="ExpirationsLabel" runat="server" meta:resourcekey="ExpirationsLabel" Visible="false" ></asp:Label> 
                                   </div>
                                    <div id="expiredImageDiv" runat="server" style="float:left;" >
                                        <img id="Img1" src="/Images/alertRed.gif" class="thumbWrap infoIcon" meta:resourcekey="ExpiredPopup" runat="server" alt=""/>
                                        <asp:Label runat="server" ID="ExpiredTextMessage"  CssClass="ExpiredLicenseText" />
                                    </div> 
                                        
                                    <div id="expiringImageDiv" runat="server" style="float:left; padding-left:5px;" >
                                        <img src="/Images/alertYellow.gif" class="thumbWrap infoIcon" meta:resourcekey="ExpiringPopup" runat="server" alt=""/>
                                        <asp:Label runat="server" ID="ExpiringTextMessage"  CssClass="ExpiredLicenseText" />
                                    </div>

                               </div>

                        </div>
                        
                        <div id="Total" class="OrderHistoryRecordsDetailsTotal" >
                            <asp:Label CssClass="OrderDetailsProjectName" ID="TotalLabel" runat="server" meta:resourcekey="TotalLabel" ></asp:Label> <%# DataBinder.Eval(Container.DataItem, "OrderTotal")%> <%# DataBinder.Eval(Container.DataItem, "CurrencyCode")%>
                        </div>
                        
                        <div id="date" class="OrderHistoryRecordsDetailsDate"  >
                         <asp:Label CssClass="OrderDetailsProjectName" ID="DateLabel" runat="server" meta:resourcekey="DateLabel" ></asp:Label> <asp:Label ID="orderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OrderDate")%>'></asp:Label> <br /> 
                         <asp:Label ID="OrderNumberLabel" CssClass="OrderDetailsProjectName" runat="server" meta:resourcekey="OrderNumberLabel" ></asp:Label>  <%# DataBinder.Eval(Container.DataItem, "OrderNumber")%>
                        </div>
                        
                        
				    </ItemTemplate>
				    <SeparatorTemplate>
				        <div class="Seperator"><img alt="" src="../Images/Divider.gif" /> </div>   
				    </SeparatorTemplate>
			    </asp:Repeater>
			    
			    
			    
		    </div>
		 </div>  
		
		<Corbis:OrderHistoryFooter id="orderHistoryFooter" runat="server" Visible="true" OnPageCommand="PageChanged" />
			
		<div class="OrderHistoryRecords" id="orderHistoryBlank" runat="server" visible="false">
		    <Corbis:Localize ID="blankOrder" runat="server"   />
		</div>

		</div>
</div>

	<script type="text/javascript">

	 
        //Setup some page events
        window.addEvent('load', function() { 
            CorbisUI.Order.registerTooltips();
         } );
	function printOrderHistory()
	{
        var pageIndex = <%= PageIndex.ToString() %>;
        var pageSize = <%= ((int)PageSize).ToString() %>;
        var sortOrder = '<%= SortOrdersBy.ToString() %>';
        window.open("PrintOrderHistory.aspx?pageIndex="+pageIndex+"&pageSize="+pageSize+"&sortOrder="+sortOrder,"Popup","width=790, menubar=no, resizable=yes,scrollbars=1");
	}
	</script>

</asp:Content>
