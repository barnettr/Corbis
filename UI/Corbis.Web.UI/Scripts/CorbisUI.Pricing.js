/****************************************************
    Corbis UI Pricing
****************************************************/

/* Request price/Contact us/Contact outline form */

// * CorbisUI.Pricing
// * Support for pricing
// * TravisO
CorbisUI.Pricing = {
    IsAuthenticated: function(controlId) {
        if (CorbisUI.Auth.GetSignInLevel() < CorbisUI.Auth.SignInLevels.AutoLoggedIn) {
            CorbisUI.Auth.Check(CorbisUI.Auth.SignInLevels.AutoLoggedIn, CorbisUI.Auth.ActionTypes.Execute, "$('" + controlId + "').click()");
            return false;
        } else {
            return true;
        }
    }
};

CorbisUI.Pricing.ContactUs = new Class({

    vars: {
        source: null,
        corbisid: null,
        lightboxid: null,
        container: null,
        optionselected: null,
        popupAnchor: null
    },

    init: function() {
        this.vars.source = '/Pricing/RequestPrice.aspx';
        this.vars.popupAnchor = $('getThankYouWindow');
    },

    OpenRequestForm: function(corbisid, lightboxid, selectedoption, containername) {

        //        console.log('CORBISID: ' + corbisid);
        //        console.log('LIGHTBOXID: ' + lightboxid);
        //        console.log('SELECTED: ' + selectedoption);
        //        console.log('CONTAINER: ' + containername);

        this.init();
        //Appending the required parameters to the query string
        this.vars.source = this.vars.source + '?corbisid=' + corbisid + '&lightboxid=' + lightboxid + '&optionselected=' + selectedoption + '&container=' + containername;
        OpenNewIModal(this.vars.source, 500, 600, 'requestPriceModalPopup');
    },

    HideRequestForm: function() {
        parent.MochaUI.CloseModal('requestPriceModalPopup');
    },

    // Calling Request price form on clicking contact A.E corbis button 
    OpenContactCorbis: function(corbisid, lightboxid, selectedoption, containername) {
        parent.CorbisUI.Pricing.ContactUs.OpenRequestForm(corbisid, null, 1, containername);
    },

    clearGetInTouchForm: function(openThankYou) {
        var g_firefox = document.getElementById && !document.all;
        if (g_firefox) {
            var control = $('formTable').getElements('input');
        } else {
            var control = $('aspnetForm').getElements('input');
        }

        // Clear out text inputs
        control.each(function(el) {
            if (el.type == "text") {
                el.value = '';
            }
        }, this);

        // Clear select boxes - set index to 0
        control = $('formTable').getElements('select');
        control.each(function(el) {
            el.selectedIndex = 0;
        }, this);

        // Set textarea to default text
        control = $('formTable').getElements('textarea');
        control.each(function(el) {
            el.value = '';
        }, this);

        if (openThankYou) CorbisUI.QueueManager.thankyou.run();
    },

    setupThankYouQueue: function() {
        CorbisUI.QueueManager.addQueue('thankyou');
        CorbisUI.QueueManager.thankyou.addItem('openThankYou', function() { CorbisUI.Pricing.ContactUs.OpenThankYouPopup(); });
    },

    OpenThankYouPopup: function() {
        (function() {
            new CorbisUI.Popup('getThankYou', {
                showModalBackground: false,
                closeOnLoseFocus: true,
                positionVert: 'middle',
                positionHoriz: 'bottom'
            });

        }).delay(200);
    }
});
CorbisUI.Pricing.ContactUs = new CorbisUI.Pricing.ContactUs();

CorbisUI.Pricing.RM = {
    vars: {
        SaveFavoriteUsageButtonID: null
    },
    RegisterToolTips: function() {
        var usageButton = $(CorbisUI.Pricing.RM.vars.SaveFavoriteUsageButtonID);
        if (usageButton == null)
            return;
        if (usageButton.hasClass('DisabledGlassButton'))
            return;
        $('aspnetForm').getAllNext('.TIP-license-details').destroy();
        var tipShowDelay = 500;
        var tipHideDelay = 100;
        var tipShowMethod = "in";
        var tipHideMethod = "out";
        if (Browser.Engine.trident) {
            tipShowDelay = 0;
            tipHideDelay = 0;
            tipShowMethod = "show";
            tipHideMethod = "hide";
        }
        new Tips('#' + usageButton.getProperty('id'), {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            offsets: { x: 20, y: 20 },
            className: 'TIP-license-details mochaContent',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    }
};
CorbisUI.Pricing.RF = {
    vars: {
        priceLabel: null,
        localizedValue: null,
        priceCode: null,
        hidAttributeUID: null,
        hidAttributeValueUID: null,
        delay: 0,
        myPricingList: null,
        cartButton: null,
        lightboxButton: null,
        dataChanged: false
    },

    highlightSelectedRow: function() {
        if (CorbisUI.Pricing.RF.vars.hidAttributeValueUID.value != null && CorbisUI.Pricing.RF.vars.hidAttributeValueUID.value != "") {
            var rowFlag = CorbisUI.Pricing.RF.vars.hidAttributeValueUID.value;
            (function() {
                if ($('repeaterInnerDiv')
                                .getElement("input[value=" + rowFlag + "]")) {
                    var parrent = $('repeaterInnerDiv')
                                .getElement("input[value=" + rowFlag + "]")
                                .getNext('ul');

                    var savedRow = parrent.getParent();
                    savedRow.setStyle('color', 'black');
                    savedRow.setStyle('cursor', 'pointer');
                    CorbisUI.Pricing.RF.vars.myPricingList.pricingListClick(parrent);
                    CorbisUI.Pricing.RF.dataChanged = false;
                }

            }).delay(CorbisUI.Pricing.RF.vars.delay);
        }
    },

    getLocalizedPayment: function() {
        CorbisUI.Pricing.RF.vars.priceLabel.innerHTML = CorbisUI.Pricing.RF.vars.localizedValue;
    },

    setLabelValue: function(value) {
        CorbisUI.Pricing.RF.vars.priceLabel.innerHTML = value.localeFormat('C');
        //window.setTimeout("CorbisUI.Pricing.RF.getLocalizedPayment()", 100);
    },

    setPriceLabel: function(amount) {
        var speed = 5;
        var steps = 95;
        if (isNaN(amount)) {
            var currentPrice = CorbisUI.Pricing.RF.vars.priceLabel.innerHTML;
            CorbisUI.Pricing.RF.vars.priceLabel.innerHTML = amount;
            CorbisUI.Pricing.RF.vars.priceCode.style.display = 'inline-block';
        } else {

            if (CorbisUI.Pricing.RF.vars.localizedValue && (perContractDisplayText == CorbisUI.Pricing.RF.vars.localizedValue)) {
                CorbisUI.Pricing.RF.vars.priceLabel.innerHTML = CorbisUI.Pricing.RF.vars.localizedValue;
                CorbisUI.Pricing.RF.vars.priceCode.style.display = 'inline-block';
            } else {
                if (isNaN(parseFloat(CorbisUI.Pricing.RF.vars.priceLabel.innerHTML))) {
                    CorbisUI.Pricing.RF.vars.priceLabel.innerHTML = (0.00).localeFormat('C');
                }
                var currentPrice = parseFloat(CorbisUI.Pricing.RF.vars.priceLabel.innerHTML);
                var gotoPrice = parseFloat(amount);

                for (var i = 0; i < steps; i++) {
                    value = currentPrice + (((gotoPrice - currentPrice) / steps) * i);
                    window.setTimeout("CorbisUI.Pricing.RF.setLabelValue(" + value + ")", i * speed);
                }
                CorbisUI.Pricing.RF.vars.priceCode.style.display = '';
                window.setTimeout("CorbisUI.Pricing.RF.setLabelValue(" + gotoPrice + ")", steps * speed);
            }
        }
    },

    doRFClick: function(el) {
        var inputs = el.getParent().getChildren("input");
        // Att UID
        CorbisUI.Pricing.RF.vars.hidAttributeUID.value = inputs[0].value;
        // Value UID
        CorbisUI.Pricing.RF.vars.hidAttributeValueUID.value = inputs[1].value;
        setGlassButtonDisabled(this.vars.cartButton, false);
        setGlassButtonDisabled(this.vars.lightboxButton, false);
        $$('div .imageContainer').setStyle('display', 'none');
        $$('div .licenseAlertDiv').setStyle('display', 'none');
        this.vars.dataChanged = true;
        CorbisUI.Pricing.RF.vars.localizedValue = inputs[3].value;
        this.setPriceLabel(inputs[2].value, inputs[3].value);

    },

    DomReady: function() {
        if (Browser.Engine.trident) {
            CorbisUI.Pricing.RF.vars.delay = 400;
        }
        CorbisUI.Pricing.RF.highlightSelectedRow();
    },

    Resize: function() {
        parent.ResizeIModal('pricing', GetDocumentHeight());
    },

    OpenRestrictions: function(element) {
        new CorbisUI.Popup('restrictionsPopup', {
            showModalBackground: false,
            centerOverElement: element,
            closeOnLoseFocus: true,
            positionVert: 'top',
            positionHoriz: 'right'
        });
    },

    OpenCustomPriceExpired: function(element) {
        new CorbisUI.Popup('customPriceExpired');
    },

    updatedPricingDataCheck: function(link) {
        PricingModalPopupExit();
        return false;
    },

    checkTab: function(el) {
        if ((document.all) && (9 == event.keyCode)) {
            $('OuterContainer').focus();
        }
    },

    ShowAddToLightboxModal: function(offeringUid) {
        if (CorbisUI.Auth.GetSignInLevel() < 1) {
            CorbisUI.CookieEvents.addCookieEvent_altPath('/Pricing', function() {
                CorbisUI.Pricing.RF.vars.hidAttributeValueUID.value = this.vars.AV;
                CorbisUI.Pricing.RF.highlightSelectedRow();
                DoAddToLightboxModal(this.vars.OID, null, false);
            }, { OID: offeringUid, AV: this.vars.hidAttributeValueUID.value });
                var temp = JSON.decode("{\"tempFunc\":function(){OpenIModal('" + window.location + "', 640, 480);}}");
                window.parent.CorbisUI.CookieEvents.addCookieEvent(temp.tempFunc);
                CorbisUI.Auth.Check(1, 'execute', 'window.parent.location = window.parent.location;');
        } else {

            DoAddToLightboxModal(offeringUid, null, false);
        }
    }
};

CorbisUI.Pricing.RF.PricingList = new Class({
                initialize: function(elements, options){
	                this.setupListEvents();
                },
                setupListEvents: function(){
                    
                    $$('div.RFPricingRepeater ul').each(function(el){
                        //Reset some styling that may have been done by click event
                        var row = el.getParent();
                        row.setStyle('color', 'black');
                        row.setStyle('cursor', 'pointer');
                        el.addEvents({
                                'mouseover': function(){
                                    this.pricingListOn(el);
                                }.bind(this),
                                'mouseout': function(){
                                    this.pricingListOut(el);
                                }.bind(this),
                                'mousedown': function(){
                                    this.pricingListMouseDown(el);
                                }.bind(this),
                                'click': function(){
                                    
                                    $$('div.RFPricingRepeater ul').each(function(ele){
                                        this.pricingListOut(ele);
                                    }, this);
                                    
                                    this.pricingListClick(el);
                                }.bind(this)
                        });
                        
                   }, this);
                },
                pricingListOn: function(el){

                    //Move background positions for mouse over
                    var row = el.getParent();
                    row.getPrevious().setStyle('background-position', '0 -90px');
                    row.setStyle('background-position', '0 -120px');
                    row.getNext().setStyle('background-position', '0 -150px');
                },
                pricingListOut: function(el){
                    
                    //Move background position for mouseout
                    var row = el.getParent();
                    row.getPrevious().setStyle('background-position', '0 0px');
                    row.setStyle('background-position', '0 -30px');
                    row.getNext().setStyle('background-position', '0 -60px');
                    
                    
                    
                },
                pricingListMouseDown: function(el){
                    var row = el.getParent();
                    row.getPrevious().setStyle('background-position', '0 -180px');
                    row.setStyle('background-position', '0 -210px');
                    row.getNext().setStyle('background-position', '0 -240px');
                },
                pricingListClick: function(el){
                    
                    //move backgroud position for click
                    //also remove mouseout and mouseover events and reset any removed events to other items
                    //Then finally send ValueUID to price changer
                    $$('ul').removeEvents();
                    this.setupListEvents();
                    CorbisUI.Pricing.RF.doRFClick(el);
                    var row = el.getParent();
                    row.getPrevious().setStyle('background-position', '0 -180px');
                    row.setStyle('background-position', '0 -210px');
                    row.getNext().setStyle('background-position', '0 -240px');
                    //set some styles
                    row.setStyle('color', 'white');
                    row.setStyle('cursor', 'default');
                    el.removeEvents();
                }
        });
        