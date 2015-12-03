<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoundCorners.ascx.cs" Inherits="Corbis.Web.UI.Accounts.RoundCorners" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<AJAXToolkit:RoundedCornersExtender ID="roundCornersExtender" runat="server" Color="#dbdbdb" Corners="All" Radius="5" TargetControlID="round" />
<div class="clear"></div>
<div id="round" runat="server" class="RoundCorners"></div>
