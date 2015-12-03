<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeliveryBlock.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.DeliveryBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="deliveryPane" class="orderSummaryPanes">
    <hr />
    <h3>
        <corbis:localize id="delivery" runat="server" meta:resourcekey="delivery" />
        <Corbis:LinkButton runat="server" ID="editLink" OnClientClick="checkoutTabs.show(1);UpdateBasedOnCurrentTabIndex({tabIndex:1});return false;" meta:resourcekey="editLink" />
    </h3>
    <table class="orderSummaryTables t100">
        <thead>
            <tr>
                <th colspan="4">
                    <corbis:localize id="imageDelivery" runat="server" meta:resourcekey="imageDelivery" />
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="leftLabels" id="deliveryMethodLevelSpan" runat="server">
                    <p><corbis:localize id="method" runat="server" meta:resourcekey="method" /></p>
                </td>
                <td id="deliveryMethodSpan" runat="server">
                    <p><Corbis:Localize ID="deliveryMethod" runat="server" /></p>
                </td>
                <td class="rightLabels" id="subjectSpan" runat="server"><p><Corbis:Localize ID="subject" runat="server" meta:resourcekey="subject" /></p></td>
                <td id="deliverySubjectSpan" runat="server"><p><Corbis:Localize ID="deliverySubject" runat="server" /></p></td>
            </tr>
            <tr>
                <td class="leftLabels" id="emailConfirmSpan" runat="server">
                    <p>
                        <Corbis:Localize ID="email" runat="server" Visible="false" meta:resourcekey="email" />
                        <Corbis:Localize ID="confirmEmail" runat="server" meta:resourcekey="confirmEmail"/>
                    </p>
                </td>
                <td id="deliveryEmailsSpan" runat="server">
                    <p>
                        <Corbis:Label ID="deliveryEmails" runat="server" />
                    </p>
                </td>
                <td class="rightLabels" id="specialInstructionsSpan" runat="server"><p><Corbis:Localize ID="specialInstructions" runat="server" meta:resourcekey="specialInstructions"></Corbis:Localize></p></td>
                 <td id="deliverySpecialInstructionsSpan" runat="server"><p><Corbis:Localize ID="deliverySpecialInstructions" runat="server"></Corbis:Localize></p></td>
            </tr>
        </tbody>
    </table>
</div>
