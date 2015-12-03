<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IFrameTunnel.aspx.cs" Inherits="Corbis.Web.UI.Common.IFrameTunnel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="reloadParent" runat="server" visible="false">
        <script>
            try {
                if (parent.parent.window.opener) {
                    parent.parent.window.opener.location = parent.parent.window.opener.location;
                }
                parent.parent.parent.setTimeout('eval("CorbisUI.GlobalNav.RefreshGlobalNav()")', 500);
            } catch (ex) { }
            parent.parent.window.location = parent.parent.window.location;
        </script>
    </div>

    <div id="redirectParent" runat="server" visible="false">
        <script>
            parent.parent.window.location = '<%=ActionArg %>';
        </script>
    </div>

    <div id="execParent" runat="server" visible="false">
        <script>
            try {
            <% if (closeLogin.Visible) { %>
            var runScript = "<%=ActionArg %>";
            parent.parent.setTimeout('eval("' + runScript + '")', 500);
            parent.parent.setTimeout('eval("CorbisUI.GlobalNav.RefreshGlobalNav()")', 600);
            <% } else { %>
            parent.parent.<%=ActionArg %>;
            <% } %>
            }
            catch (ex) { }
        </script>
    </div>

    <div id="closeLogin" runat="server" visible="false">
        <script>
            parent.parent.MochaUI.CloseModal('<%=WindowId %>');
        </script>
    </div>
    
    </form>
</body>
</html>
