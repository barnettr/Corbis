<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestrictionsControlTest.aspx.cs" Inherits="Corbis.Web.UI.Pricing.yo" %>
<%@ Register Src="../src/Image/Restrictions.ascx" TagName="Restrictions" TagPrefix="IR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <IR:restrictions id="ImageRestrictions" runat="server"></IR:restrictions>
    </div>
    </form>
</body>
</html>
