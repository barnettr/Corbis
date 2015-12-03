<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintOrderHistory.aspx.cs" Inherits="Corbis.Web.UI.OrderHistory.PrintOrderHistory" Title="<%$ Resources: Title %>"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:localize id="Title" meta:resoursekey="Title" /></title>
    		<link href="../Stylesheets/Accounts.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color:#D8D8D8" >
<div >
    <div id="orderHistoryContent" class="OrderHistoryContent">
        <div id="orderHistoryContentMain" class="OrderHistoryContentMain" >
    
    <a href="javascript:window.print()"><asp:localize id="PrintLinkLabel" meta:resoursekey="PrintLink" /></a>
     <div id="orderHistoryRecords" class="OrderHistoryRecords" runat="server">
    <asp:Repeater ID="ordersRepeater" runat="server" EnableViewState="false" >
			    <ItemTemplate>
				    <div id="ProjectName" class="OrderHistoryRecordProjectName" ><%# DataBinder.Eval(Container.DataItem, "ProjectName")%></div>
				        <div id="Items" class="OrderHistoryRecordsDetailsExpirations" >
				            <asp:Label ID="ItemsLabel" CssClass="OrderDetailsProjectName" runat="server" meta:resourcekey="ItemsLabel" ></asp:Label> <%# DataBinder.Eval(Container.DataItem, "ItemCount")%><br /><asp:Label ID="ExpirationsLabel" runat="server" meta:resourcekey="ExpirationsLabel" Visible="false" ></asp:Label> <asp:Label ID="ExpiringTextLabel" runat="server"></asp:Label><asp:Label ID="ExpiredTextMessage" runat="server" />  <asp:Label runat="server" ID="ExpiringTextMessage" /> 
				        </div>
				        <div id="Total" class="OrderHistoryRecordsDetailsTotal" >
				            <asp:Label ID="TotalLabel" CssClass="OrderDetailsProjectName" runat="server" meta:resourcekey="TotalLabel" ></asp:Label> <%# DataBinder.Eval(Container.DataItem, "OrderTotal")%>
				        </div>
				        <div id="date" class="OrderHistoryRecordsDetailsDate"> 
				            <asp:Label ID="DateLabel" CssClass="OrderDetailsProjectName" runat="server" meta:resourcekey="DateLabel" ></asp:Label> <%# DataBinder.Eval(Container.DataItem, "OrderDate", "{0:MM/dd/yyyy}")%><br />
				            <asp:Label  ID="OrderNumberLabel" CssClass="OrderDetailsProjectName"  runat="server" meta:resourcekey="OrderNumberLabel"  ></asp:Label>  <%# DataBinder.Eval(Container.DataItem, "OrderNumber")%></div>
				</ItemTemplate>
				<SeparatorTemplate>
				   <div class="Seperator">
				       <img id="seperator" src="../Images/divider.gif" alt=""  />
				   </div>
				</SeparatorTemplate>
			</asp:Repeater>
			</div>
	</div>
    </div>
</div>

</body>
</html>
