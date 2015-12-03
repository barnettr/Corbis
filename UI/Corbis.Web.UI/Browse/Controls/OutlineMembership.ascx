<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutlineMembership.ascx.cs" Inherits="Corbis.Web.UI.Browse.OutlineMembership" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div class="OfficeSelector Module" id="OfficeSelector">
	<h4>outline&trade;</h4>
	<asp:Repeater ID="ContactInformation" runat="server" OnItemDataBound="ContactInformation_OnItemDataBound">
		<HeaderTemplate><ol id="ContactInformation"></HeaderTemplate>
		<ItemTemplate><asp:Literal ID="OfficeLocation" runat="server" /></ItemTemplate>
		<FooterTemplate></ol></FooterTemplate>
	</asp:Repeater>
	<p>
		<Corbis:DropDownList ID="OfficeList" runat="server" />
	</p>
</div>
<div class="OutlineMembership Module">
	<h4>outline&trade; Membership</h4>
	<p>
		<Corbis:Localize ID="OutlineMembershipIntro" runat="server" />
	</p>
    <fieldset>
		<asp:HiddenField ID="SelectedCountry" runat="server" />
        <ol>
            <li>
				<label for="Name"><Corbis:Localize ID="NameLabel" runat="server" meta:resourcekey="NameLabel" /></label>
				<asp:TextBox ID="Name" runat="server" />
			</li>
            <li>
				<label for="CompanyName"><Corbis:Localize ID="CompanyNameLabel" runat="server" meta:resourcekey="CompanyNameLabel" /></label>
				<asp:TextBox ID="CompanyName" runat="server" />
			</li>
            <li>
				<label for="CountryRegion"><Corbis:Localize ID="CountryRegionLabel" runat="server" meta:resourcekey="CountryRegionLabel" /></label>
                <Corbis:DropDownList class="item" validate="custom1" custom1="validateMailingCountry()" meta:resourcekey="country" ID="country" EntityType="Country" runat="server" />
			</li>
            <li>
				<label for="Telephone"><Corbis:Localize ID="TelephoneLabel" runat="server" meta:resourcekey="TelephoneLabel" /></label>
				<asp:TextBox ID="Telephone" runat="server" />
			</li>
            <li>
				<label for="EmailAddress"><Corbis:Localize ID="EmailAddressLabel" runat="server" meta:resourcekey="EmailAddressLabel" /></label>
				<asp:TextBox ID="EmailAddress" runat="server" />
			</li>
        </ol>
    </fieldset>
    <div class="PreferenceButtons">
        <Corbis:GlassButton ID="sendEmail" runat="server" meta:resourcekey="SavePreference" OnClick="SendOutlineEmail" />
        <Corbis:GlassButton ID="cancelEmail" runat="server" meta:resourcekey="CancelPreference" OnClick="CancelEmail" CausesValidation="false" ButtonStyle="gray" />
    </div>
</div>

<script type="text/javascript">
	function attachOnChangeEvent(){
	    var select = $('<%= OfficeList.ClientID %>');
		select.addEvent('change', function(){
			var selected = select.options[select.selectedIndex].text.replace(" ","");
			setHiddenFieldValue(selected);
			show(selected);
		});
	}
	
	function setHiddenFieldValue(country){
		var emailAddress = getEmailAddress(country);

		var hiddenField = $('<%= SelectedCountry.ClientID %>'); 
		hiddenField.setProperty('value', emailAddress);
	}
	
	function getEmailAddress(country){
		var emailAddress = 'sales@corbis.com';
		
		if ($(country) == null)
			country = 'UnitedStates';
		
		var emails = $(country).getElements('a[href^=mailto:]');
		if (emails != null){
			emails.each(function(item){
				var href = item.getProperty('href');
				var index = href.indexOf("mailto:") + 7;
				href = href.substring(index);
				emailAddress = href;
			});
		}
		return emailAddress;		
	};
	
	function show(what){
		var showwhat = $(what);
		showwhat.addClass('active');
		hideothers(what);
	}
	
	function hideothers(exempt){
		var selector = 'li[id!=' + exempt + ']';
		var list = $('ContactInformation').getElements(selector).each(function(el){
			el.removeClass('active');
		});
	}
	
	window.addEvent('domready', function() {
		setHiddenFieldValue('UnitedStates');
		attachOnChangeEvent();
		show('UnitedStates');
	});
	
</script>
