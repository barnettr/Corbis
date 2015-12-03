<%@ Page Language="C#" AutoEventWireup="true" Codebehind="RSPricing.aspx.cs" MasterPageFile="~/MasterPages/ModalPopup.Master"
    Inherits="Corbis.Web.UI.Pricing.RSPricing" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagName="PricingHeader" TagPrefix="Corbis" Src="PricingHeader.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="rsPricingContent" ContentPlaceHolderID="mainContent" runat="server">
    <!-- start -->
   <div class="OuterContainer">   
    
    <div class="TitleBar">
    
                <div class="PriceImageTitle">
                    <Corbis:Label ID="pageTitle" CssClass="Title" meta:resourcekey="pageTitle" runat="server" />
                    <span class="SubTitle"><Corbis:Label ID="pageSubTitle" meta:resourcekey="pageSubTitle" runat="server" /></span>
                </div>
                
                
                <div class="CloseButton">
                    <Corbis:imageButton ID="continue" runat="server" CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" 
        OnClientClick="PricingModalPopupExit();return false;" ></Corbis:imageButton>
                </div>
                
                <div class="PrintDiv">
                    <asp:HyperLink runat="server" NavigateUrl="#" ID="printImgHyperlink" CssClass="PrintIconImg" meta:resourcekey="printImgHyperlink" />
                </div>
                
                <div class="ChatDiv">
                    <asp:HyperLink runat="server" NavigateUrl="#" CssClass="ChatIconText" ID="chatHyperlink" meta:resourcekey="chatHyperlink" />
                    <asp:HyperLink runat="server" NavigateUrl="#" CssClass="ChatIconImg" ID="chatImgHyperlink" meta:resourcekey="chatImgHyperlink" />
                </div>
        
    </div>
            
    <Corbis:PricingHeader id="pricingHeader" runat="server" />
    <input type="hidden" name="hidAttributeUID" id="hidAttributeUID" runat="server" />
    <input type="hidden" name="hidAttributeValueUID" id="hidAttributeValueUID" runat="server" />
    <input type="hidden" name="hidUseTypeUID" id="hidUseTypeUID" runat="server" />

    <div class="PricingGrid">
        <asp:PlaceHolder  ID="PricegridHolder" runat="server">
        </asp:PlaceHolder>
    </div>

    <asp:Label runat="server" CssClass="RFPricingRepeaterNoResults" Visible="true" ID="rpRSPricingLabel" meta:resourcekey="rpRSPricingLabel" />
    
    <div class="PricingBottomRS">
        <div class="PriceMultipleRF">
            <asp:Label runat="server" ID="lblPriceMayVary" meta:resourcekey="lblPriceMayVary" />
        </div>
        <div class="ViewRestrictions">
            <asp:HyperLink runat="server" NavigateUrl="#" ID="lnkViewRestrictions" meta:resourcekey="lnkViewRestrictions" />
        </div>
    </div>   
    
    <div class="PricingFooter">
        <asp:Label runat="server" ID="lblPricingFooterDisclaimer" meta:resourcekey="lblPricingFooterDisclaimer" />
    </div>
 </div>
	<Corbis:ContactCorbis runat="server" />
    <script type="text/javascript" language="javascript">

        function setupListEvents(){
            $$('ul').addEvents({
                'mouseover': function(){
                    pricingListOn(this);
                },
                'mouseout': function(){
                    pricingListOut(this);
                },
                'click': function(){
                    
                    $$('ul').each(function(el){
                        pricingListOut(el);
                    });
                    pricingListClick(this);
                   
                }
            });
        }
        setupListEvents();
        
        function pricingListOn(el){
            //Move background positions for mouse over
            var row = el.getParent();
            row.getPrevious().setStyle('background-position', '0 -90px');
            row.setStyle('background-position', '0 -120px');
            row.getNext().setStyle('background-position', '0 -150px');
        }
        function pricingListOut(el){
            //Move background position for mouseout
            var row = el.getParent();
            row.getPrevious().setStyle('background-position', '0 0px');
            row.setStyle('background-position', '0 -30px');
            row.getNext().setStyle('background-position', '0 -60px');
        }
        function pricingListClick(el){
            //move backgroud position for click
            //also remove mouseout and mouseover events and reset any removed events to other items
            //Then finally send ValueUID to price changer
            $$('ul').removeEvents();
            setupListEvents();
            doRFClick(el);
            var row = el.getParent();
            row.getPrevious().setStyle('background-position', '0 -180px');
            row.setStyle('background-position', '0 -210px');
            row.getNext().setStyle('background-position', '0 -240px');
            el.removeEvents();
        }

        var priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
        var priceCode = $('<%= pricingHeader.piPriceCode.ClientID %>');
        var hidAttributeUID = $('<%= hidAttributeUID.ClientID %>');
        var hidAttributeValueUID = $('<%= hidAttributeValueUID.ClientID %>');
        var hidUseTypeUID = $('<%= hidUseTypeUID.ClientID %>');
               
        setGlassButtonDisabled($('<%=pricingHeader.lightboxButton.ClientID %>'), true);
        
        function doRFClick(el)
        {
            var inputs = el.getParent().getChildren("input");

            // Att UID
            hidAttributeUID.value = inputs[0].value;
            // Value UID
            hidAttributeValueUID.value = inputs[1].value;
            // useType UID
            hidUseTypeUID.value = inputs[2].value;     
                
            setGlassButtonDisabled($('<%= pricingHeader.cartButton.ClientID %>'), false);
            setGlassButtonDisabled($('<%=pricingHeader.lightboxButton.ClientID %>'), false);
            setPriceLabel(inputs[3].value);
        }
        
        function setPriceLabel(amount)
        {
            var speed = 5;
            var steps = 95;
            if ( isNaN(amount) ) {
                 var currentPrice = priceLabel.innerHTML;
                 priceLabel.innerHTML = amount;
                 priceCode.style.display='none';
                 
            }else{
                 if (isNaN(parseFloat(priceLabel.innerHTML)))
                 {
                    priceLabel.innerHTML = "0.00";
                 }
                 var currentPrice = parseFloat(priceLabel.innerHTML);
                 var gotoPrice = parseFloat(amount);
                 
                  for (var i=0; i <steps; i++)
                    {
                        value = currentPrice + (((gotoPrice - currentPrice) / steps) * i);
                        window.setTimeout("setLabelValue(" + value + ")", i * speed);
                    }
                    priceCode.style.display='';
                    window.setTimeout("setLabelValue(" + gotoPrice + ")", steps * speed);
            }
        }
        function setLabelValue(value)
        {
            priceLabel.innerHTML = value.toFixed(2);
        }
        
</script>
    <!-- end -->
   
</asp:Content>
