<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopRound.ascx.cs" Inherits="Corbis.Web.UI.Accounts.TopRound" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<AJAXToolkit:RoundedCornersExtender ID="topRoundCorner_Extender" runat="server" Color="#dbdbdb" Corners="Top" Radius="5" TargetControlID="topRoundDiv" />
<div id="topRoundDiv" runat="server" class="RoundCorners"></div>
