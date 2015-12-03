/****************************************************
    Corbis UI Express Checkout
***************************************************/

// Express Checkout
// Putting this here due to the number of calls needed to the parent page
// Travis O
var cvcHadFocus = false;
CorbisUI.ExpressCheckout = {
    vars: {
        DateFormat: null,
        DateSeparator: null,
        licenseDateAlreadyUpdated: false,
        IsDirty: null,
        OriginalProjectName: null,
        RequiredText: null,
        ProjectField: null,
        JobField: null,
        PurchaseOrderField: null,
        LicenseeField: null,
        ASCCompliant: null,
        LicenseType: null,
        PriceLabel: null,
        PriceCode: null,
        HidAttributeUID: null,
        HidAttributeValueUID: null,
        HidProductUid: null,
        PaymentMethodBox: null,
        ThePaymentMethodGuid: null,
        ThePaymentType: null,
        CardExpired: null,
        MyPricingList: null,
        ThumbTips: null,
        SavedUsageDropdown: null,
        FavoriteUseLabel: null,
        LicenseDetails: null,
        CorbisID: null,
        AjaxLoader: null,
        AjaxUpdater: null,
        AjaxMessage: null,
        SavedUseTitle: null,
        SavedUseValue: null,
        SavedUsageText: null,
        SavedUsageTruncated: null,
        PricedStartDateTextBox: null,
        PricedByAEStartDateTextBox: null,
        AlreadyPricedTextBox: null,
        DefaultDate: null,
        ImageThumbnail: null,
        ContractType: null,
        PromoCodeBox: null,
        LoadPricingText: null,
        LoadLicensingText: null,
        PurchaseButton: null,
        CreditCardCVC: null,
        LightboxId: 0,
        HidPaymentMethod: null,
        CorporateID: null,
        CreditCardUID: null,
        PriceCodeDiv: null,
        TotalCodeDiv: null,
        TotalPrice: null

    },
    FixValidationCodeCursor: function(over, elem) {
        if ((document.activeElement.id && document.activeElement.id == CorbisUI.ExpressCheckout.vars.CreditCardCVC.get('id'))) {
            cvcHadFocus = true;
            if (over) {
                CorbisUI.ExpressCheckout.vars.CreditCardCVC.blur();
                return;
            }
        }
        if (cvcHadFocus) {
            CorbisUI.ExpressCheckout.vars.CreditCardCVC.focus();
            cvcHadFocus = false;
        }
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
            offsets: { x: -340, y: -130 },
            className: 'TIP-license-details mochaContent',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    },

    Resize: function() {
        if (Browser.Engine.trident) { IEAddHeight = "10"; } else { IEAddHeight = 0; }

        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = CorbisUI.ExpressCheckout.getParentProtocolBasedURL() + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=ResizeIModal('expressCheckout'," + (parseInt(GetDocumentHeight()) + parseInt(IEAddHeight)) + ")&noclose=true";
    },

    setupRFRepeater: function() {
        var delay = 0;
        if (Browser.Engine.trident) delay = 400;

        if (this.vars.HidAttributeValueUID.value != null && this.vars.HidAttributeValueUID.value != "") {
            var rowFlag = this.vars.HidAttributeValueUID.value;
            (function() {

                var parrent = $('repeaterInnerDiv').getElement("input[value=" + rowFlag + "]");
                if (parrent) {
                    parrent = parrent.getNext('ul');
                    var savedRow = parrent.getParent();
                    savedRow.setStyle('color', 'black');
                    savedRow.setStyle('cursor', 'pointer');
                    CorbisUI.ExpressCheckout.vars.MyPricingList.pricingListClick(parrent);
                    CorbisUI.ExpressCheckout.vars.DataChanged = false;
                }
            }).delay(delay);
        };

    },


    lockStep2: function() {
        //console.log('lock step 2');
        $$('.selectUse')[0].set('opacity', .66);
        $$('.selectUse .mask')[0].setStyle('display', '');
    },

    unlockStep2: function() {
        //console.log('unlockStep2');
        $$('.selectUse')[0].set('opacity', 1);
        $$('.selectUse .mask')[0].setStyle('display', 'none');
    },

    lockStep3: function() {
        //console.log('lock step 3');
        this.lockPromoButton();
        $$('.purchaseInfo')[0].set('opacity', .66);
        $$('.purchaseInfo .mask')[0].setStyle('display', '');

    },
    unlockStep3: function() {
        if (this.CheckLicenseeFieldValid()) {
            this.unlockPromoButton();
            CorbisUI.ExpressCheckout.vars.PromoCodeBox.disabled = false;
            $$('.purchaseInfo')[0].set('opacity', 1);
            $$('.purchaseInfo .mask')[0].setStyle('display', 'none');
        }
    },
    lockStep4: function() {
        //console.log('lock stp4');
        //LOCK PURCHASE BUTTON
        //if (!this.vars.PurchaseButton.hasClass('DisabledGlassButton')) 
        //setGlassButtonDisabled(this.vars.PurchaseButton, true)

        this.vars.PurchaseButton.set('opacity', .66);
        $$('.purchaseButtonMask')[0].setStyle('display', '');
        this.vars.PurchaseButton.disabled = true;
    },
    unlockStep4: function() {
        //UNLOCK THE PURCHASE BUTTON
        //console.log('unlockStep4');

        //if (this.vars.PurchaseButton.hasClass('DisabledGlassButton'))
        //setGlassButtonDisabled(this.vars.PurchaseButton, false);
        this.vars.PurchaseButton.set('opacity', 1);
        $$('.purchaseButtonMask')[0].setStyle('display', 'none');
        this.vars.PurchaseButton.disabled = false;
        var gb = CorbisUI.ExpressCheckout.vars.PromoCodeButton;
        CorbisUI.ExpressCheckout.setPromoButtonDisabled(gb, true);

    },
    lockPromoButton: function() {
        //LOCK THE PROMO BUTTON
        //console.log('lockPromoButton');
        //CorbisUI.ExpressCheckout.togglePromoButtonDisabled();
        CorbisUI.ExpressCheckout.vars.PromoCodeBox.disabled = true;
        setOutlineButtonDisabled(CorbisUI.ExpressCheckout.vars.PromoCodeButton, true);
    },
    unlockPromoButton: function() {
        //UNLOCK THE PROMO BUTTON
        //console.log('unlockPromoButton');
        //CorbisUI.ExpressCheckout.togglePromoButtonDisabled();
        setOutlineButtonDisabled(CorbisUI.ExpressCheckout.vars.PromoCodeButton, false);
    },
    validatePromoButton: function(promoBoxText) {
        //Confirm checkboxes are checked
        var checkboxList = $('purchaseCheckboxes').getElements("span.ImageCheckbox input");
        var passed = null;
        checkboxList.each(function(el) {
            //Loop through checkboxes to see if both are checked
            if (el.checked == true)
                passed += 1;
            else
                passed = 0;
        });
        //Confirm promoBox has a value and that a payment method is selected
        if (promoBoxText != '') {
            this.unlockPromoButton();
        } else {
            this.lockPromoButton();
            var gb = CorbisUI.ExpressCheckout.vars.PromoCodeButton;
            CorbisUI.ExpressCheckout.setPromoButtonDisabled(gb, true);
        }
    },
    isValidStartDate: function(startDate) {
        return CorbisUI.ExpressCheckout.ConvertToDateUsingLocaleFormat(startDate) >= new Date().setHours(0, 0, 0, 0);
    },
    doValidateStartDate: function() {
        if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RF') {
            //There is no license start date for RF images always return true for validation
            return true;
        }
        if (CorbisUI.ExpressCheckout.vars.licenseStartDate == null || CorbisUI.ExpressCheckout.vars.licenseStartDate == "") {
            CorbisUI.ExpressCheckout.vars.licenseStartDate = CorbisUI.ExpressCheckout.GetStartDate();
        }
        if (CorbisUI.ExpressCheckout.vars.licenseStartDate == null || CorbisUI.ExpressCheckout.vars.licenseStartDate == "") {
            return false;
        }
        var warningMode = CorbisUI.ExpressCheckout.ConvertToDateUsingLocaleFormat(CorbisUI.ExpressCheckout.vars.licenseStartDate) < new Date().setHours(0, 0, 0, 0);
        if (warningMode) {
            $('errorBlock').setStyle('display', 'block');
            $$('.StartDate').each(function(el) { el.addClass('WarningMode'); });
            this.Resize();
        }
        else {
            $('errorBlock').setStyle('display', 'none');
            $$('.StartDate').each(function(el) { el.removeClass('WarningMode'); });
        }
        return (!warningMode);
    },
    validateStep3: function() {
        if (CorbisUI.ExpressCheckout.vars.CardExpired == 'True') {
            this.lockStep4();
            return;
        }
        var checkboxList = $('purchaseCheckboxes').getElements("span.ImageCheckbox input");
        var passed = null;
        checkboxList.each(function(el) {
            //Loop through checkboxes to see if both are checked
            if (el.checked == true)
                passed += 1;
            else
                passed = 0;
        });
        //IF both checkboxes have been checked (restrictions,license agreement), call step4 unlocker
        if (passed > 1 && this.vars.PaymentMethodBox[this.vars.PaymentMethodBox.selectedIndex].value != 'none' && this.vars.PaymentMethodBox.options[this.vars.PaymentMethodBox.selectedIndex].getAttribute('creditapproved') != "False") {
            this.unlockStep4();
        } else {
            if(this.vars.PaymentMethodBox.options[this.vars.PaymentMethodBox.selectedIndex].getAttribute('creditapproved') == "False")
                OpenModal('CoporateAccountCreditReview');
                
            this.lockStep4();
        }

    },
    validateStep2: function() {
        // remove the keyup event from licensee
        if (this.vars.LicenseeField) {
            this.vars.LicenseeField.removeEvents('keyup');
        }
        // RF validation
        if ($$('.SubTitle span')[0].hasClass('RF_green')) {

            if (this.vars.HidAttributeValueUID.value.length == 0 || this.vars.HidAttributeValueUID.value == "00000000-0000-0000-0000-000000000000") {
                this.lockStep3();
            } else {
                this.unlockStep3();
            }
        } else {
            // RM validation
            if (this.doValidateStartDate(this.GetStartDate())) {
                if (CorbisUI.ExpressCheckout.vars.PricedByAE == 'True') {
                    this.unlockStep3();
                }
                else {
                    // Check if it's already priced or a saveed usage has been selected
                    if ((CorbisUI.ExpressCheckout.vars.AlreadyPriced == 'True' ||
                        CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.selectedIndex > 0) && (this.isValidStartDate(this.GetStartDate()))) {
                        this.unlockStep3();
                    } else {
                        // not valid
                        this.lockStep3();
                    }
                }
            }
            else {
                this.lockStep3();
            }
        }

    },
    ConvertToDateUsingLocaleFormat: function(dateString) {
        var dateParts = CorbisUI.ExpressCheckout.vars.DateFormat.split(CorbisUI.ExpressCheckout.vars.DateSeparator);
        var mm, dd, yyyy;
        try {
            var dateValues = dateString.split(CorbisUI.ExpressCheckout.vars.DateSeparator);
            for (var i = 0; i < dateParts.length; i++) {
                if (dateParts[i].indexOf('M') > -1) {
                    mm = dateValues[i];
                }
                if (dateParts[i].indexOf('y') > -1) {
                    yyyy = dateValues[i];
                }
                if (dateParts[i].indexOf('d') > -1) {
                    dd = dateValues[i];
                }
            }
            return new Date(mm + "/" + dd + "/" + yyyy);
        } catch (ex) {
            return null;
        }
    },
    ConvertToDateStringUsingLocaleFormat: function(dateString) {
        //This function should be moved to common/util namespace
        var dateParts = CorbisUI.ExpressCheckout.vars.DateFormat.split(CorbisUI.ExpressCheckout.vars.DateSeparator);
        var finalstr = null;
        var tempstr = null;
        if (typeof (dateString) == 'string') {
            try {
                var dateValues = dateString.split(CorbisUI.ExpressCheckout.vars.DateSeparator);
                for (var i = 0; i < dateParts.length; i++) {
                    if (dateParts[i].indexOf('M') > -1) {
                        tempstr = dateValues[i];
                    }
                    if (dateParts[i].indexOf('y') > -1) {
                        tempstr = dateValues[i];
                    }
                    if (dateParts[i].indexOf('d') > -1) {
                        tempstr = dateValues[i];
                    }
                    if (finalstr == null) {
                        finalstr = tempstr;
                    } else {
                        finalstr += CorbisUI.ExpressCheckout.vars.DateSeparator + tempstr;
                    }
                }
            } catch (ex) {
                return false;
            }
        }
        if (typeof (dateString) == 'number') {
            dateString = new Date(dateString);
            try {
                for (var i = 0; i < dateParts.length; i++) {
                    if (dateParts[i].indexOf('M') > -1) {
                        tempstr = dateString.getMonth() + 1;
                    }
                    if (dateParts[i].indexOf('y') > -1) {
                        tempstr = dateString.getFullYear();
                    }
                    if (dateParts[i].indexOf('d') > -1) {
                        tempstr = dateString.getDate();
                    }
                    if (finalstr == null) {
                        tempstr = tempstr;
                        finalstr = tempstr;
                    } else {
                        finalstr += CorbisUI.ExpressCheckout.vars.DateSeparator + tempstr;
                    }
                }
            } catch (ex) {
                return false;
            }
        }


        var finaldateParts = finalstr.split(CorbisUI.ExpressCheckout.vars.DateSeparator);
        for (var i = 0; i < finaldateParts.length; i++) {
            if (isNaN(finaldateParts[i])) {
                return "";
            }
        }

        return finalstr;
    },
    handleLicenseDateChange: function(elem) {
        if (elem != null) {
            CorbisUI.ExpressCheckout.vars.licenseStartDate = CorbisUI.ExpressCheckout.ConvertToDateStringUsingLocaleFormat(elem.value);
            CorbisUI.ExpressCheckout.UpdateStartDate(CorbisUI.ExpressCheckout.vars.licenseStartDate);
        }
        //Check if the date is string or date type and do string conversion accordingly

        CorbisUI.ExpressCheckout.validateStep2();
        if (CorbisUI.ExpressCheckout.isValidStartDate(CorbisUI.ExpressCheckout.vars.licenseStartDate)) {
            CorbisUI.ExpressCheckout.UpdateStartDate(CorbisUI.ExpressCheckout.vars.licenseStartDate);
            var hidProductUid = CorbisUI.ExpressCheckout.vars.HidProductUid.value;
            //console.log('RMProductUidCheck RMProductUidCheck ' + hidProductUid);
            //If not blank ProductUid
            if (hidProductUid != '00000000-0000-0000-0000-000000000000' && hidProductUid != '') {
                if ($('licenseStartDateDiv') != null) {
                    $('licenseStartDateDiv').innerHTML = CorbisUI.ExpressCheckout.vars.licenseStartDate;
                }
            }
            $('errorBlock').setStyle('display', 'none');
            $$('.StartDate').each(function(el) { el.removeClass('WarningMode'); });
        } else {
            $('errorBlock').setStyle('display', 'block');
            $$('.StartDate').each(function(el) { el.addClass('WarningMode'); });
            this.Resize();
        }
    },
    validateStartDate: function(sender, args) {
        CorbisUI.ExpressCheckout.vars.licenseStartDate = CorbisUI.ExpressCheckout.ConvertToDateStringUsingLocaleFormat(sender._selectedDate.setHours(0, 0, 0, 0));
        CorbisUI.ExpressCheckout.UpdateStartDate(CorbisUI.ExpressCheckout.vars.licenseStartDate);
        CorbisUI.ExpressCheckout.validateStep2();
        CorbisUI.ExpressCheckout.handleLicenseDateChange(null);
    },
    validateStep1: function(event) {

        if (event) {

            if (event.target.id.indexOf('projectField') > -1) {
                if (this.vars.ProjectField.value.trim().length == 0) {
                    this.vars.ProjectField.value = this.vars.OriginalProjectName;
                }
                this.vars.IsDirty = true;

            }
            if (event.target.id.indexOf('jobField') > -1) {
                this.vars.IsDirty = true;

            }
            if (event.target.id.indexOf('poField') > -1) {
                this.vars.IsDirty = true;

            }
            if (event.target.id.indexOf('licenseeField') > -1) {
                this.vars.IsDirty = true;
                if (this.vars.LicenseeField.value.trim().length == 0 || this.vars.LicenseeField.value == this.vars.RequiredText) {
                    this.vars.LicenseeField.setStyle('background-color', '#ffffcc');
                    this.lockStep2();
                    this.lockStep3();
                    this.lockStep4();

                } else {
                    this.vars.LicenseeField.setStyle('background-color', '#ffffff');
                    this.unlockStep2();
                    this.validateStep2();
                }
            }

        } else {
            //console.log(this.vars.LicenseeField.value);
            //console.log(this.vars.RequiredText);

            if (this.vars.LicenseeField.value.trim().length == 0 || this.vars.LicenseeField.value == this.vars.RequiredText) {
                this.lockStep2();
                this.lockStep3();
                this.lockStep4();

            } else {
                this.vars.LicenseeField.setStyle('background-color', '#ffffff');
                this.unlockStep2();
                this.validateStep2();

            }
        }


    },
    ValidateProjectASCText: function() {

        var req = new Request.HTML({
            method: 'post',
            url: "/Checkout/CheckoutService.asmx/ValidateProjectEncoding",
            data: {
                'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
                'projectNameClientId': CorbisUI.ExpressCheckout.vars.ProjectField.id,
                'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
                'jobNumberClientId': CorbisUI.ExpressCheckout.vars.JobField.id,
                'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
                'poNumberClientId': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.id,
                'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
                'licenseeClientId': CorbisUI.ExpressCheckout.vars.LicenseeField.id
            },
            onRequest: function() {

            },
            onSuccess: function(responseTree, responseElements, responseHtml) {

                this.fireEvent('ajaxSuccess', responseTree);
                var errorIndex = responseHtml.indexOf('<ScriptServiceValidationError');
                if (errorIndex == -1) {
                    CorbisUI.ExpressCheckout.ASCCompliant = "True";
                    $$('.projectWrap input').each(function(el) {
                        el.setStyle('background-color', '#ffffff');
                    });

                    if (CorbisUI.ExpressCheckout.CheckLicenseeFieldValid() == true) {
                        CorbisUI.ExpressCheckout.unlockStep2();

                    }

                    CorbisUI.ExpressCheckout.validateStep3();
                    $("projectValidate").addClass('displayNone');
                    CorbisUI.ExpressCheckout.Resize();

                    //Call final form validation
                    CorbisUI.ExpressCheckout.submitValidationCheck();


                } else {
                    CorbisUI.ExpressCheckout.ASCCompliant = "False";
                    var result;
                    if (Browser.Engine.trident) {
                        result = responseElements.filter('scriptservicevalidationerror');

                    } else {
                        var test = responseHtml.split('?>');
                        test.shift();
                        test = (test.length == 1) ? test[0] : test.join('?>');

                        var ser = new CorbisUI.JSSerializer();
                        test = ser.getDom(test);

                        result = $(test).getElements('ScriptServiceValidationError');

                    }
                    $$('.projectWrap input').each(function(el) {
                        el.setStyle('background-color', '#ffffff');
                    });
                    if (result.length > 0) {
                        CorbisUI.ExpressCheckout.lockStep2();
                        CorbisUI.ExpressCheckout.lockStep3();
                        $("projectValidate").removeClass('displayNone');
                        CorbisUI.ExpressCheckout.Resize();

                    }
                    result.each(function(item) {
                        if (item.getElement('ClientId')) {
                            var elId = item.getElement('ClientId').get('text');
                            //console.log('elId: ' + elId);
                            $(elId).setStyle('background-color', '#ffffcc');
                        }
                    }, this);


                }

            },
            onFailure: function(response) {
                //Message? This would only be a failure to call the Service, not an invalid response
            }

        }).send();
    },
    CheckLicenseeFieldValid: function() {
        return !(this.vars.LicenseeField.value.trim().length == 0 || this.vars.LicenseeField.value == this.vars.RequiredText);
    },

    trimFields: function(event) {
        if (event.target.id.indexOf('licenseeField') > -1) {
            if (this.vars.LicenseeField.value.trim().length == 0) {
                this.vars.LicenseeField.value = '';
            }
        }
    },

    setupPanels: function() {
        CorbisUI.Watermark.initialize();
        this.setupForm1Validation();
        this.lockStep2();
        this.lockStep3();
        this.lockStep4();
        this.lockPromoButton();
        window.setTimeout("CorbisUI.ExpressCheckout.Resize()", 500);
    },
    setupForm1Validation: function() {
        this.vars.OriginalProjectName = this.vars.ProjectField.value;

        this.vars.ProjectField.addEvent('change', function(event) { CorbisUI.ExpressCheckout.validateStep1(event); });

        this.vars.LicenseeField.addEvent('change', function(event) { CorbisUI.ExpressCheckout.trimFields(event); });
        this.vars.LicenseeField.addEvent('blur', function(event) { CorbisUI.ExpressCheckout.validateStep1(event); });
        this.vars.LicenseeField.addEvent('keyup', function(event) { CorbisUI.ExpressCheckout.validateStep1(event); });

        this.vars.JobField.addEvent('change', function(event) { CorbisUI.ExpressCheckout.validateStep1(event); });

        this.vars.PurchaseOrderField.addEvent('change', function(event) { CorbisUI.ExpressCheckout.validateStep1(event); });

        this.vars.HidAttributeValueUID.addEvent('change', function() { CorbisUI.ExpressCheckout.validateStep2(); });
        this.vars.HidProductUid.addEvent('change', function() { CorbisUI.ExpressCheckout.validateStep2(); });
    },
    getProtocol: function() {
        if (window.location.protocol == "https:")
            return "https";
        return "http";
    },
    OpenExpressCheckoutModal: function(corbisId, productUid, lightboxId, caller) {
        if (caller == null) {
            caller = "NotSet";
        }
        var expressCheckoutPage = HttpsUrl + '/Checkout/ExpressCheckout.aspx?corbisId=' + corbisId + "&protocol=" + CorbisUI.ExpressCheckout.getProtocol() + "&caller=" + caller;
        if (typeof (productUid) != undefined) {
            expressCheckoutPage += "&productUid=" + productUid;

            if (typeof (lightboxId) != undefined) {
                expressCheckoutPage += "&LightboxId=" + lightboxId;
            }
        }

        OpenNewIModal(expressCheckoutPage, 960, 630, 'expressCheckout');
    },

    openCreateNewUsage: function(corbisId) {
        // need to use double quotes to pass parameters as encodeURIComponent will not
        // escape single quotes, but they're OK in the query string
        var exec = "CorbisUI.ExpressCheckout.hideExpressCheckoutAndShowCreateNewUsage(\"" +
            corbisId + "\", \"" +
            encodeURIComponent(CorbisUI.ExpressCheckout.vars.ProjectField.value.trim()) + "\", \"" +
            encodeURIComponent(CorbisUI.ExpressCheckout.vars.JobField.value.trim()) + "\", \"" +
            encodeURIComponent(CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value.trim()) + "\", \"" +
            encodeURIComponent(CorbisUI.ExpressCheckout.vars.LicenseeField.value.trim()) + "\")";
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = CorbisUI.ExpressCheckout.getParentProtocolBasedURL() + "/Common/IFrameTunnel.aspx?windowId=expressCheckout&action=execute&noclose=true&actionArg=" + escape(exec);
    },

    hideExpressCheckoutAndShowCreateNewUsage: function(corbisId, projectName, jobNumber, poNumber, licensee) {
        var pricingUrl = "/Pricing/RMPricing.aspx?ParentPage=ExpressCheckout&CorbisId=" + corbisId +
            "&projectName=" + projectName +
            "&jobNumber=" + jobNumber +
            "&poNumber=" + poNumber +
            "&licensee=" + licensee;
        //console.log(pricingUrl);
        // Hide express checkout
        HideModal('expressCheckout');
        // Show RM pricing, for now...
        PriceImage(pricingUrl, 700, 545);
    },

    closeCreateNewUsageAndShowExpressCheckout: function() {
        CloseModal('pricing');
        MochaUI.ShowModal('expressCheckout');
        var noProdUid = $('expressCheckoutWindow_iframe').src;
        noProdUid = noProdUid.replace("productUid", "oldPuid");
        noProdUid = noProdUid.replace("&isNewUsage=True", "");
        $('expressCheckoutWindow_iframe').src = noProdUid + "&isNewUsage=True&refresh=" + Math.random();
    },
    getCurrentProtocolBasedURL: function() {
        if (window.location.protocol == "https:") {
            return HttpsUrl;
        }
        return HttpUrl;
    },
    getParentProtocolBasedURL: function() {
        try {
            return ParentProtocol;
        } catch (ex) {

        }
    },

    // Called from the iFrame itself
    DoCloseExpressCheckoutModal: function() {
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = CorbisUI.ExpressCheckout.getParentProtocolBasedURL() + "/Common/IFrameTunnel.aspx?windowId=expressCheckout&action=close";
    },
    CloseExpressCheckoutModal: function(element) {
        if (this.vars.IsDirty) {
            OpenCloseWarning('confirmClose', element);
        } else {
            this.DoCloseExpressCheckoutModal();
        }
    },

    Open: function(mediaUid, productUid, lightboxId, caller) {
        CorbisUI.Auth.Check(CorbisUI.Auth.SignInLevels.LoggedIn, CorbisUI.Auth.ActionTypes.Execute, "CorbisUI.ExpressCheckout.OpenExpressCheckoutModal('" + mediaUid + "','" + productUid + "','" + lightboxId + "','" + caller + "')");
    },

    OpenLegal: function() {
        var exec = "CorbisUI.Legal.OpenPolicyIModal()";
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = CorbisUI.ExpressCheckout.getParentProtocolBasedURL() + "/Common/IFrameTunnel.aspx?windowId=expressCheckout&action=execute&noclose=true&actionArg=" + escape(exec);
    }

}
