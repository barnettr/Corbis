/****************************************************
    Corbis UI MyProfile
****************************************************/


//  CorbisUI.MyProfile Class
//  Contains IModals, Popups, Tooltip
//  and Pane functionality for the Accordion.
//  Also Password for the MyAccounts pages because it
//  is a secure area and different from the SignIn password. 
//  Auth: Travis O. & Rob B.

CorbisUI.MyProfile = new Class({

    vars: {
        source: null,
        sourceEditPayment: null,
        sourceBusiness: null,
        sourceMailingAddress: null,
        popupAnchor: null,
        base: null,
        g_firefox: null,
        g_IE: null,
        ThumbTips: null,
        sourceShippingAddress: null,
        updateBusinessInfoPaneButton: null,
        updatePersonalInfoPaneButton: null,
        updatePreferencesPaneButton: null,
        updatePaymentPaneButton: null,
        hiddenPanesGroupA: null,
        hiddenPanesGroupB: null,
        hiddenPanesGroupC: null,
        hiddenPanesGroupD: null,
        refreshOnce: false
    },

    init: function() {
        this.vars.source = '/Accounts/ChangePersonalInformation.aspx';
        this.vars.sourceEditPayment = '/Accounts/EditPaymentInformation.aspx';
        this.vars.sourceBusiness = '/Accounts/EditBusinessInformation.aspx';
        this.vars.sourceMailingAddress = '/Accounts/EditMailingAddress.aspx';
        this.vars.sourceShippingAddress = '/Accounts/EditShippingAddress.aspx';
        this.vars.popupAnchor = $('getThankYouWindow');
        this.vars.base = $('getExpressCheckout');
        this.vars.g_firefox = document.getElementById && !document.all;
        this.vars.g_IE = Browser.Engine.trident;
        this.vars.hiddenPanesGroupA = [$('businessInfoPaneDiv'), $('paymentPaneDiv'), $('preferencesPaneDiv')];
        this.vars.hiddenPanesGroupA_v2 = [$('businessInfoPaneDiv'), $('preferencesPaneDiv')];
        this.vars.hiddenPanesGroupB = [$('personalInfoPaneDiv'), $('paymentPaneDiv'), $('preferencesPaneDiv')];
        this.vars.hiddenPanesGroupB_v2 = [$('personalInfoPaneDiv'), $('preferencesPaneDiv')];
        this.vars.hiddenPanesGroupC = [$('personalInfoPaneDiv'), $('businessInfoPaneDiv'), $('preferencesPaneDiv')];
        this.vars.hiddenPanesGroupD = [$('personalInfoPaneDiv'), $('businessInfoPaneDiv'), $('paymentPaneDiv')];
        this.vars.hiddenPanesGroupD_v2 = [$('personalInfoPaneDiv'), $('businessInfoPaneDiv')];
    },

    OpenMyPersonalInformation: function() {
        this.init();
        OpenNewIModal(this.vars.source, 360, 540, 'personalInfoModalPopup');
    },

    OpenEditPaymentInformation: function(guid, mode) {
        this.init();
        var ht;
        var source = this.vars.sourceEditPayment;
        if (guid) {
            source = source + "?cardId=" + guid;
        }
        if (mode) {
            source = source + "&mode=" + mode;
        }
        if (this.vars.g_firefox) {
            ht = 250;
        } else {
            ht = 280;
        }
        if (mode == "delete") {
            ht = 115;
        }
        OpenNewIModal(source, 400, ht, 'editPaymentInfoModalPopup');
    },

    OpenEditBusinessInformation: function() {
        if (this.vars.g_firefox) {
            this.init();
            OpenNewIModal(this.vars.sourceBusiness, 350, 440, 'editBusinessInformation');
        } else {
            this.init();
            OpenNewIModal(this.vars.sourceBusiness, 350, 500, 'editBusinessInformation');
        }
    },

    OpenEditMailingAddress: function() {
        if (this.vars.g_firefox) {
            this.init();
            OpenNewIModal(this.vars.sourceMailingAddress, 350, 300, 'editMailingAddress');
        } else {
            this.init();
            OpenNewIModal(this.vars.sourceMailingAddress, 350, 360, 'editMailingAddress');
        }
    },

    OpenSuccessPopup: function() {
        new CorbisUI.Popup('getSuccess', {
            showModalBackground: false,
            closeOnLoseFocus: true,
            positionVert: 'middle',
            positionHoriz: 'bottom'
        });
    },

    OpenSuccessChinaPopup: function() {
    new CorbisUI.Popup('registerSuccessDiffCountry', {
            showModalBackground: false,
            closeOnLoseFocus: true,
            positionVert: 'middle',
            positionHoriz: 'bottom'
        });
    },

    registerTooltips: function(isFirstTime) {
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
        this.vars.ThumbTips = new Tips('.thumbWrap', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            offsets: { x: 0, y: -130 },
            className: 'TIP-license-details mochaContent lineHeight',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    },

    refreshPersonalInfoPane: function() {
        this.vars.updatePersonalInfoPaneButton.click();
    },

    refreshBusinessInfoPane: function() {
        this.vars.updateBusinessInfoPaneButton.click();
    },

    refreshPreferencesPane: function() {
        this.vars.updatePreferencesPaneButton.click();
    },

    refreshPaymentPane: function() {
        this.vars.updatePaymentPaneButton.click();
    },

    ResizePaymentInfo: function() {
        if (this.vars.refreshOnce) {
            this.vars.refreshOnce = false;
        } else {
            var divPay = $$('.paymentPaneDiv .PaneContent')[0];
            var divPayContent = $$('.paymentPaneDiv .PaneContent div')[0];
            divPay.setStyle('height', '66px');
            divPay.setStyle('height', divPayContent.getSize().y + 8);
            // Whenever we update payment, we need to refresh preferences too
            this.vars.refreshOnce = true;
            this.refreshPreferencesPane();
        }
    },

    ResizeBusinessInfo: function() {
        var divPay = $('businessInfoPaneContent');
        divPay.setStyle('height', divPay.getScrollSize().y);
    },

    ResizePersonalInfo: function() {
        var divPay = $('personalInfoPaneContent');
        divPay.setStyle('height', divPay.getScrollSize().y);
    },

    ResizePreferences: function() {
        if (this.vars.refreshOnce) {
            this.vars.refreshOnce = false;
        } else {
            this.vars.refreshOnce = true;
            this.refreshPaymentPane();
        }
        this.registerTooltips(false);
    },

    doChangePasswordSuccess: function() {
        MochaUI.CloseModal('changePassword');
        OpenModal('passwordSuccessDiv');
    },

    changePassword: function() {
        OpenNewIModal('/Accounts/ChangePassword.aspx', 350, 200, 'changePassword');
        var iframeCP = $(document.body).getElement('iframe');
        iframeCP.setProperty('scrolling', 'no');
        iframeCP.setStyle('overflow', 'hidden');
    },

    GetPane: function() {
        var pane = '';
        var setPane = 0;
        $('personalInfoPaneDiv').setStyle('overflow', 'visible');
        var querystring = window.location.search.substring(1);
        var querystringArray = querystring.split("&");
        for (i = 0; i < querystringArray.length; i++) {
            var querystringKeyValue = querystringArray[i].split("=");
            if (querystringKeyValue[0] == 'Pane') {
                pane = querystringKeyValue[1];
            }
        }
        switch (pane) {
            case 'personalInfoPane':
                setPane = 0;
                $('personalInfoPaneDiv').setStyle('overflow', 'visible');
                break;
            case 'businessInfoPane':
                setPane = 1;
                $('businessInfoPaneDiv').setStyle('overflow', 'visible');
                break;
            case 'paymentPane':
                setPane = 2;
                $('paymentPaneDiv').setStyle('overflow', 'visible');
                break;
            //            case 'shippingPane':                    
            //                setPane = 3;                    
            //                break;                    
            case 'preferencesPane':
                setPane = 3;
                $('preferencesPaneDiv').setStyle('overflow', 'visible');
                break;
        }
        return setPane;
    },

    getPaneDiv: function(div) {

        this.init();

        switch (div) {
            case 0:
                $('personalInfoPaneDiv').setStyle('overflow', 'visible');
                if ($('paymentPaneDiv') != null) {
                    this.vars.hiddenPanesGroupA.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').removeClass('AccountsContentExpanded');
                } else {
                    this.vars.hiddenPanesGroupA_v2.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').removeClass('AccountsContentExpanded_noPaymentPane');
                }

                $('preferencesPaneDiv').removeClass('MB_10');
                break;
            case 1:
                $('businessInfoPaneDiv').setStyle('overflow', 'visible');
                if ($('paymentPaneDiv') != null) {
                    this.vars.hiddenPanesGroupB.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').removeClass('AccountsContentExpanded');
                } else {
                    this.vars.hiddenPanesGroupB_v2.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').removeClass('AccountsContentExpanded_noPaymentPane');
                }

                $('preferencesPaneDiv').removeClass('MB_10');
                break;
            case 2:
                $('paymentPaneDiv').setStyle('overflow', 'visible');
                this.vars.hiddenPanesGroupC.each(function(item) { item.setStyle('overflow', 'hidden') });
                $('AccountsContent').removeClass('AccountsContentExpanded');
                $('preferencesPaneDiv').removeClass('MB_10');
                break;
            case 3:
                $('preferencesPaneDiv').setStyle('overflow', 'visible').addClass('MB_10');
                if ($('paymentPaneDiv') != null) {
                    this.vars.hiddenPanesGroupD.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').addClass('AccountsContentExpanded');
                } else {
                    this.vars.hiddenPanesGroupD_v2.each(function(item) { item.setStyle('overflow', 'hidden') });
                    $('AccountsContent').addClass('AccountsContentExpanded_noPaymentPane');
                }

                break;
        }
    }

});
CorbisUI.MyProfile = new CorbisUI.MyProfile();