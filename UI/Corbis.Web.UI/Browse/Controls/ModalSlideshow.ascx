<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalSlideshow.ascx.cs" Inherits="Corbis.Web.UI.Browse.ModalSlideshow" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="XSLControls" TagName="XSLTransform" src="~/UserControls/XSLControls/XSLTransform.ascx" %>

<Corbis:ModalPopup ContainerID="example1" runat="server" Width="920" Title="Example Title">
	<XSLControls:XSLTransform runat="server" ID="myXSLTransform" />
</Corbis:ModalPopup>