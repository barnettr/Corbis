<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeliveryBlock.ascx.cs"
    Inherits="Corbis.Web.UI.OrderHistory.DeliveryBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="deliveryPane" class="orderSummaryPanes">
    <hr />
    <h3>
        <Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="delivery" />
        <asp:LinkButton runat="server" ID="editLink" OnClientClick="checkoutTabs.show(1);UpdateBasedOnCurrentTabIndex({tabIndex:1});return false;">Edit</asp:LinkButton>
    </h3>
    <table class="orderSummaryTables t100">
        <thead>
            <tr>
                <th colspan="2">
                    <corbis:localize id="imageDelivery" runat="server" meta:resourcekey="imageDelivery" />
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="leftLabels">
                    <p><corbis:localize id="method" runat="server" meta:resourcekey="method" /></p>
                </td>
                <td>
                    <p><Corbis:Localize ID="deliveryMethod" runat="server" /></p>
                </td>
            </tr>
            <tr>
                <td class="leftLabels">
                    <p>
                        <Corbis:Localize ID="email" runat="server" Visible="false" meta:resourcekey="email" />
                        <Corbis:Localize ID="confirmEmail" runat="server" meta:resourcekey="confirmEmail"/>
                    </p>
                </td>
                <td>
                    <p>
                        <Corbis:Label ID="deliveryEmails" runat="server" />
                    </p>
                </td>
            </tr>
        </tbody>
    </table>
</div>
