<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstantServiceButton.ascx.cs" Inherits="Corbis.Web.UI.Chat.InstantServiceButton" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<script language="javaScript" type="text/javascript">
   document.write('<img src="http' + '<asp:Literal id="litSecure" runat="server" />://admin.instantservice.com/resources/smartbutton/6163/<asp:Literal id="litDepartment" runat="server" />/available.gif?'+Math.floor(Math.random()*10001)+'" style="width:0;height:0;visiblity:hidden;position:absolute;" onLoad="agents_available()" onError="agents_not_available()">');
</script>

<div id="divInstantService" class="instantServiceDiv" style="display:none;" >
    
    <div class="instantServiceDivText">
        <Corbis:Hyperlink ID="hypInstantService" ChildWindow="true" ChildWindowHeight="320" 
        ChildWindowWidth="500"  meta:resourcekey="hypInstantService" runat="server" /> 
    </div>
</div>
