<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/ErrorsMaster.Master" Theme="BlackBackground" CodeBehind="UnexpectedError.aspx.cs" Inherits="Corbis.Web.UI.Errors.UnexpectedError" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<asp:Content ContentPlaceHolderID="ErrorContent" runat="server">

     <div id="sorryMessage"> 
             <div class="rc5px clear">
                <div class="Top">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
            </div>
            <p>
                <Corbis:Localize ID="Message" runat="server" meta:resourcekey="Message" />  
           </p>
            <div class="rc5px clear">
                <div class="Bottom">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
          </div>
     </div>

</asp:Content>
