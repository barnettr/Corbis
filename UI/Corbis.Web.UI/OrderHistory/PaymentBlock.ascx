<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentBlock.ascx.cs"
    Inherits="Corbis.Web.UI.OrderHistory.PaymentBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="paymentPane" class="orderSummaryPanes">
    <hr />
    <h3>
        <Corbis:Localize runat="server" ID="Localize2"  meta:resourcekey="payment" />
        <asp:LinkButton runat="server" ID="editLink" OnClientClick="checkoutTabs.show(2);UpdateBasedOnCurrentTabIndex({tabIndex:2});return false;">Edit</asp:LinkButton></h3>
    <table class="orderSummaryTables t100">
        <thead>
            <tr>
                <th colspan="2" class="leftSection">
                <Corbis:Localize runat="server" ID="Localize1"  meta:resourcekey="paymentMethod" />
                </th>
                <th class="rightSection">
                    <Corbis:Localize runat="server" ID="companyAddressLbl"  meta:resourcekey="companyAddressLbl" />
                    <Corbis:Localize runat="server" ID="billingAddressLbl" meta:resourcekey="billingAddressLbl" />
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="leftLabels">
                    <p>
                        <Corbis:Localize runat="server" ID="accountLabel" meta:resourcekey="accountLabel" />
                        <Corbis:Localize runat="server" ID="cartTypeLabel" Visible="false" meta:resourcekey="cartTypeLabel" />
                    </p>
                </td>
                <td class="rightValue">
                    <p>
                        <Corbis:Label ID="accountOrCard" runat="server" />
                    </p>
                </td>
                <td rowspan="3" class="rightSection t50">
                    <p>
                        <Corbis:Label runat="server" ID="billingName" />
                        <Corbis:Label runat="server" ID="billingAddress123" /><br />
                       
                        <Corbis:Label runat="server" ID="billingCity" />
                        <Corbis:Label runat="server" ID="billingState" />&nbsp;<Corbis:Label runat="server"
                            ID="billingZip" /><br />
                        <Corbis:Label runat="server" ID="billingCountry" />
                    </p>
                </td>
            </tr>
            <tr id="secondRow" runat="server">
                <td class="leftLabels">
                    <p>
                        <Corbis:Label ID="cardDisplayNumber" runat="server" meta:resourcekey="cardDisplayNumber" /></p>
                </td>
                <td class="rightValue">
                    <p>
                        <Corbis:Label ID="cardDisplayNumberValue" runat="server" /></p>
                </td>
            </tr>
            <tr id="thirdRow" runat="server">
                <td class="leftLabels">
                    <p>
                        <Corbis:Label ID="cardHolderName" runat="server" meta:resourcekey="cardHolderName" /></p>
                </td>
                <td class="rightValue">
                    <p>
                        <Corbis:Label ID="cardHolderNameValue" runat="server" /></p>
                </td>
            </tr>
        </tbody>
    </table>
</div>
