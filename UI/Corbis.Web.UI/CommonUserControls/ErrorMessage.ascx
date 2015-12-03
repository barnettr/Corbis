<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ErrorMessage.ascx.cs" Inherits="Corbis.Web.UI.CommonUserControls.ErrorMessage" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div id="errorControlDiv" style="width:100%;background-color:#FFFFCC;"  runat="server">

    <div  style="float:left;width:70px; text-align:right;">
     
     <asp:Image id="errorImage" ImageUrl="~/Images/iconError.png" runat="server" />
    </div>
    <div style="float:left;padding-left:10px;" >
     <Corbis:ValidationGroupSummary ID="validationSummaryGroup" runat="server" CssClass="Error" ForeColor="black"  />
     
    </div>

</div>
