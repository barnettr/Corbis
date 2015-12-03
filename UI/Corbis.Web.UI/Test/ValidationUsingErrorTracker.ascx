<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValidationUsingErrorTracker.ascx.cs" Inherits="Corbis.Web.UI.Test.ValidationUsingErrorTracker" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div class="ModalDescriptionText">
    This is a form
</div>
<div ID="ErrorSummaryPanel" class="ValidationSummary displayNone">
    <ul></ul>
</div>

<table cellpadding="1" cellspacing="0" class="pop350">
<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>text field</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox ID="field1" runat="server" />
    </td>
</tr>
<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>another field</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox ID="field2" runat="server" />
    </td>
</tr>
</table>
<div class="ButtonRow350">
    <Corbis:GlassButton Text="Validate text field" OnClientClick="custom1();return false;" runat="server" />
    <Corbis:GlassButton Text="Validate other field" OnClientClick="custom2();return false;" runat="server" />
</div>

<script>

    function custom1() {
        var tb = $('<%=field1.ClientID %>');
        if (tb.value == '' || tb.value == 'x')
            _errorTracker.addError(tb.id, 'text field error message');
        else
            _errorTracker.removeError(tb.id);
    }
    function custom2() {
        var tb = $('<%=field2.ClientID %>');
        if (tb.value == '' || tb.value == 'badness')
            _errorTracker.addError(tb.id, 'another field error message');
        else
            _errorTracker.removeError(tb.id);
    }
    var _errorTracker;
    function initETModal(containerId) {
        if (_errorTracker)
            _errorTracker.reset();
        else
            _errorTracker = new CorbisUI.FormUtilities.ErrorTracker({ container: containerId });
    }
</script>