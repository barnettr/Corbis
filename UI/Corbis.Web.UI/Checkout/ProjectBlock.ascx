<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectBlock.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.ProjectBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="projectPane" class="orderSummaryPanes">
    <h3>
        <corbis:localize id="project" runat="server" meta:resourcekey="project" />
        <Corbis:LinkButton runat="server" ID="editLink" OnClientClick="checkoutTabs.show(0);UpdateBasedOnCurrentTabIndex({tabIndex:0});return false;" meta:resourcekey="editLink" />
    </h3>
    <table class="orderSummaryTables t100">
        <thead>
            <tr>
                <th colspan="2">
                    <corbis:localize id="projectDetails" runat="server" meta:resourcekey="projectDetails"></corbis:localize>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="leftLabels">
                    <p>
                        <corbis:localize id="projectName" runat="server" meta:resourcekey="projectName"></corbis:localize>
                    </p>
                </td>
                <td>
                    <p><Corbis:Label ID="projectNameData" runat="server" />
                        </p>
                </td>
            </tr>
            <tr runat="server" id="jobNumberRow" visible="false" >
                <td class="leftLabels">
                    <p>
                        <corbis:localize id="jobNumberLabel" runat="server" meta:resourcekey="jobNumberLabel"></corbis:localize>
                    </p>
                </td>
                <td>
                    <p>
                        <Corbis:Label ID="jobNumber" runat="server" /></p>
                </td>
            </tr>
            <tr runat="server" id="poNumberRow" visible="false" >
                <td class="leftLabels">
                    <p>
                        <corbis:localize id="poNumberLabel" runat="server" meta:resourcekey="poNumberLabel"></corbis:localize>
                    </p>
                </td>
                <td>
                    <p>
                        <Corbis:Label ID="poNumber" runat="server" /></p>
                </td>
            </tr>
            <tr>
                <td class="leftLabels">
                    <p>
                        <corbis:localize id="licenseeLabel" runat="server" meta:resourcekey="licenseeLabel"></corbis:localize>
                    </p>
                </td>
                <td>
                    <p>
                        <Corbis:Label ID="licensee" runat="server" /></p>
                </td>
            </tr>
        </tbody>
    </table>
</div>
