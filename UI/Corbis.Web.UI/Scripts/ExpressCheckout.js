/* 

                                                     ___,------, 
             _,--.---.                         __,--'         / 
           ,' _,'_`._ \                    _,-'           ___,| 
          ;--'       `^-.                ,'        __,---'   || 
        ,'               \             ,'      _,-'          || 
       /                  \         _,'     ,-'              || 
      :                    .      ,'     _,'                 |: 
      |                    :     `.    ,'                    |: 
      |           _,-      |       `-,'                      :: 
     ,'____ ,  ,-'  `.   , |,         `.                     : \ 
     ,'    `-,'       ) / \/ \          \                     : : 
     |      _\   o _,-'    '-.           `.                    \ \ 
      `o_,-'  `-,-' ____   ,` )-.______,'  `.                   : : 
       \-\    _,---'    `-. -'.\  `.  /     `.                  \  \ 
        / `--'             `.   \   \:        \                  \,.\ 
       (              ____,  \  |    \\        \                 :\ \\ 
        )         _,-'    `   | |    | \        \                 \\_\\ 
       /      _,-'            | |   ,'-`._      _\                 \,' 
       `-----' |`-.           ;/   (__ ,' `-. _;-'`\           _,--' 
     ,'        |   `._     _,' \-._/  Y    ,-'      \      _,-' 
    /        _ |      `---'    :,-|   |    `     _,-'\_,--'   \ 
   :          `|       \`-._   /  |   '     `.,-' `._`         \ 
   |           _\_    _,\/ _,-'|                     `-._       \ 
   :   ,-         `.-'_,--'    \                         `       \ 
   | ,'           ,--'      _,--\           _,                    : 
    )         .    \___,---'   ) `-.____,--'                      | 
   _\    .     `    ||        :            \                      ; 
 ,'  \    `.    )--' ;        |             `-.                  / 
|     \     ;--^._,-'         |                `-._            _/_\ 
\    ,'`---'                  |                    `--._____,-'_'  \ 
 \_,'                         `._                          _,-'     ` 
      -LEAVE ME ALONE!-     ,-'  `---.___           __,---' 
                          ,'             `---------' 
                        ,' 


*/

/******************************
CORBIS CHECKOUT
*******************************/
var currentPaymentMethodSelection = null;
var lastValidationMethod = null;
var ExpressCheckoutObj = $extend(CorbisUI.ExpressCheckout, {
    ExpressCheckoutAjax: function(CorbisId, ProductUid, SavedUsageUid) {
        //console.log('ExpressCheckoutAjax');
        var tmpStartDate = this.GetStartDate();

        //Make the AJAX call
        var req = new Request({
            method: 'post',
            url: 'ExpressCheckout_LicenseDetails.aspx',
            data: { 'CorbisId': CorbisId,
                'productUid': ProductUid,
                'savedUsageUid': SavedUsageUid,
                'licenseStartDate': tmpStartDate
            },
            onRequest: function() {

                //console.log('ExpressCheckoutAjax onRequest');
                var tmpAjaxBox = CorbisUI.ExpressCheckout.vars.AjaxLoader.get('div.msg');
                if (tmpAjaxBox) {
                    tmpAjaxBox.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadLicensingText);
                    CorbisUI.ExpressCheckout.vars.AjaxLoader.fade(0.9);
                }
                else {
                    CorbisUI.ExpressCheckout.vars.AjaxLoader.fade(0);
                }
            },
            onComplete: function(response) {
                //console.log('ExpressCheckoutAjax onComplete');
                CorbisUI.ExpressCheckout.vars.AjaxUpdater.set('html', response);
                CorbisUI.ExpressCheckout.vars.AjaxLoader.fade(0);

                //Set default date
                if (this.getHeader('licenseStartDate')) {
                    CorbisUI.ExpressCheckout.vars.DefaultDate = this.getHeader('licenseStartDate');
                }
                //Handle Agessa and TaxError validation
                var showAgessa = this.getHeader('AgessaFlag');
                var showTaxError = this.getHeader('TaxError');
                if (showAgessa == 'True' || showTaxError == 'True')
                    $('important').setStyle('display', '');
                if (showTaxError == 'True')
                    $('taxErrorBox').setStyle('display', '');
                if (showAgessa == 'True')
                    $('agessaBox').setStyle('display', '');

                //Handle validation from Presenters
                var validationCode = this.getHeader('validationResult');
                lastValidationMethod = validationCode;
                //console.log(validationCode);
                switch (validationCode) {
                    case "Failure":
                        CorbisUI.ExpressCheckout.ShowPricingExpiredError();
                        break;
                    case "PricedByAE":
                        //Call PricedByAE method
                        //console.log('ExpressCheckoutAjax onComplete PricedByAE');
                        CorbisUI.ExpressCheckout.RMExpressCheckoutPricingAjax();
                        CorbisUI.ExpressCheckout.ShowPricedByAE(this.getHeader("LicenseStartDateRequired"), CorbisUI.ExpressCheckout.vars.SavedUsageText, CorbisUI.ExpressCheckout.vars.SavedUsageValue);
                        CorbisUI.ExpressCheckout.vars.PricedByAEStartDateTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
                        CorbisUI.ExpressCheckout.ShowStartOver();
                        break;
                    case "AEPriceExpired":
                        if (!MochaUI.ModalExists('customPriceExpired')) {
                            OpenModal('customPriceExpired');
                        }
                        break;
                    case "Success":
                        //Turn on the RM Pricing next step (date)
                        CorbisUI.ExpressCheckout.ShowRMPriced(this.getHeader("LicenseStartDateRequired"));
                        CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
                        CorbisUI.ExpressCheckout.RMExpressCheckoutPricingAjax();
                        CorbisUI.ExpressCheckout.ShowStartOver();
                        break;
                    case "CheckoutNotAllowed":
                        // Modal alert
                        OpenModal('imageNotAvailableForCheckout');
                        break;
                    case "NoUsage":
                        CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
                        break;
                }
                //Resize the iframe window after AJAX request complete
                CorbisUI.ExpressCheckout.Resize();
            },
            onFailure: function() {
                //console.log('ExpressCheckoutAjax onFailure');
                CorbisUI.ExpressCheckout.vars.AjaxUpdater.set('html', CorbisUI.ExpressCheckout.vars.licenseDetailsErrorMessage);
            }
        }).send();


    },
    UpdateStartDate: function(newStartDate) {
        if (CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox) {
            CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value = newStartDate;
            CorbisUI.ExpressCheckout.vars.PricedByAEStartDateTextBox.value = newStartDate;
        }
        if (CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox) {
            CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox.value = newStartDate;
        }
    },
    GetStartDate: function() {
        //console.log('GetStartDate');
        var tmpStartDate = '';
        if (CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox) {
            tmpStartDate = (CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value == '') ? CorbisUI.ExpressCheckout.vars.PricedByAEStartDateTextBox.value : CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value;
        }
        if (tmpStartDate == '' && CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox) tmpStartDate = CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox.value;
        return tmpStartDate;
    },

    //    ExpressCheckoutCustomPricingAjax: function() {
    //        //RM ONLY
    //        //console.log('ExpressCheckoutCustomPricingAjax');
    //        //RM Only
    //        var tmpLicenseBox = CorbisUI.ExpressCheckout.vars.AjaxLoader;
    //        var licenseAjaxMsg = tmpLicenseBox.getElement('div.msg');
    //        var myItem = CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.selectedIndex;
    //        var tmpStartDate = CorbisUI.ExpressCheckout.GetStartDate();




    //        var req = new Request({
    //            method: 'post',
    //            url: 'ExpressCheckout_LicenseDetails.aspx',
    //            data: { 'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
    //                'startDate': tmpStartDate,
    //                'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
    //                'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
    //                'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
    //                'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
    //                'lightboxId': CorbisUI.ExpressCheckout.vars.LightboxId,
    //                'productUid': CorbisUI.ExpressCheckout.vars.HidProductUid.value,
    //                'currencyCode': CorbisUI.ExpressCheckout.vars.PriceCode.value,
    //                'hidAttributeValueUID': CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value,
    //                'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
    //                'thePaymentMethod': CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid.value,
    //                'thePaymentType': CorbisUI.ExpressCheckout.vars.ThePaymentType.value,
    //                'contractType': CorbisUI.ExpressCheckout.vars.ContractType,
    //                'promoCode': CorbisUI.ExpressCheckout.vars.PromoCodeBox.value
    //            },
    //            onRequest: function() {
    //                //console.log('ExpressCheckoutCustomPricingAjax onRequest');
    //                licenseAjaxMsg.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadLicensingText);
    //                tmpLicenseBox.fade(0.9);

    //            },
    //            onComplete: function(response) {
    //                //console.log('ExpressCheckoutCustomPricingAjax onComplete');
    //                licenseAjaxMsg.set('html', response);
    //                CorbisUI.ExpressCheckout.vars.LicenseDetails.set('html', response);
    //                if (this.getHeader('licenseStartDate')) {
    //                    CorbisUI.ExpressCheckout.vars.DefaultDate = this.getHeader('licenseStartDate');
    //                }
    //                //Handle Agessa validation
    //                var showAgessa = this.getHeader('AgessaFlag');
    //                if (showAgessa == 'True')
    //                    $('agessaBox').setStyle('display', '');

    //                tmpLicenseBox.fade(0);
    //                //Handle validation from Presenters
    //                var validationCode = this.getHeader('validationResult');
    //                switch (validationCode) {
    //                    case "Failure":
    //                        $$('.RMPricingError').setStyle('display', 'block');
    //                        CorbisUI.ExpressCheckout.lockStep3();
    //                        break;
    //                    case "PricedByAE":
    //                        $$('.RMPricingError').setStyle('display', 'none');
    //                        $$('.RMPricingInnerWrap').setStyle('display', 'none');
    //                        $$('.RMPricedByAEInnerWrap').setStyle('display', 'block');

    //                        $$('.StartDate').setStyle('display', 'block');
    //                        CorbisUI.ExpressCheckout.vars.PricedByAEStartDateTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
    //                        ///Set text for and show Favorite Use labels
    //                        CorbisUI.ExpressCheckout.ShowFavoriteUseLabel();

    //                        break;
    //                    case "NoUsage":
    //                        break;
    //                    case "AEPriceExpired":
    //                        if (!MochaUI.ModalExists('customPriceExpired')) {
    //                            OpenModal('customPriceExpired');
    //                        }
    //                        break;
    //                    case "Success":
    //                        $$('.RMPricingError').setStyle('display', 'none');
    //                        $('StartDatePriced').setStyle('display', 'block');
    //                        CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
    ////                        if (CorbisUI.ExpressCheckout.vars.ThePaymentType != 'none')
    ////                            CorbisUI.ExpressCheckout.RMExpressCheckoutPricingAjax();

    //                        break;
    //                    case "CheckoutNotAllowed":
    //                        // Modal alert
    //                        OpenModal('imageNotAvailableForCheckout');
    //                        CorbisUI.ExpressCheckout.lockStep3();
    //                        break;
    //                }

    //                //console.log('this is it');



    //                //Resize the iframe window after AJAX request complete
    //                CorbisUI.ExpressCheckout.Resize();
    //            },
    //            onFailure: function() {
    //                //console.log('ExpressCheckoutCustomPricingAjax onFailure');
    //                CorbisUI.ExpressCheckout.vars.LicenseDetails.set('html', CorbisUI.ExpressCheckout.vars.pricingCalculationErrorMessage);
    //                tmpLicenseBox.fade(0);
    //            }
    //        }).send();

    //    },
    RMExpressCheckoutPricingAjax: function() {
        //console.log('RMExpressCheckoutPricingAjax');
        //make the ajax call 
        var tmpStartDate = this.GetStartDate();
        var tmpPricingBox = $('ajaxPriceLoader');
        var pricingAjaxMsg = tmpPricingBox.getElement('div.msg');
        var pricingBox = $('pricingPane');

        //console.log(CorbisUI.ExpressCheckout.vars.PriceCode);
        //console.log(CorbisUI.ExpressCheckout.vars.PriceCode.value);

        var req = new Request({
            method: 'post',
            url: 'ExpressCheckout_PricingBox.aspx',
            data: { 'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
                'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
                'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
                'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
                'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
                'startDate': tmpStartDate,
                'productUid': CorbisUI.ExpressCheckout.vars.HidProductUid.value,
                'lightboxId': CorbisUI.ExpressCheckout.vars.LightboxId,
                'currencyCode': CorbisUI.ExpressCheckout.vars.PriceCode.value,
                'thePaymentMethod': CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid,
                'thePaymentType': CorbisUI.ExpressCheckout.vars.ThePaymentType,
                'contractType': CorbisUI.ExpressCheckout.vars.ContractType,
                'promoCode': CorbisUI.ExpressCheckout.vars.PromoCodeBox.value
            },
            onRequest: function() {
                pricingAjaxMsg.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadPricingText);
                tmpPricingBox.height = pricingBox.getStyle('height');
                tmpPricingBox.fade(0.9);
                // Lock Step 2 to prevent multiple requests
                CorbisUI.ExpressCheckout.lockStep2();
            },
            onComplete: function(response) {
                if (lastValidationMethod != null) {
                    switch (lastValidationMethod) {
                        case "PricedByAE":
                            CorbisUI.ExpressCheckout.ShowPricedByAE(this.getHeader("LicenseStartDateRequired"));
                            break;
                        case "AEPriceExpired":
                            CorbisUI.ExpressCheckout.ShowPricedByAE(this.getHeader("LicenseStartDateRequired"));
                            break;
                        default:
                            CorbisUI.ExpressCheckout.ShowRMPriced(this.getHeader("LicenseStartDateRequired"));
                            break;
                    }
                }
                //console.log('RMExpressCheckoutPricingAjax onComplete');
                pricingAjaxMsg.set('html', response);
                if (this.getHeader('licenseStartDate')) {
                    CorbisUI.ExpressCheckout.vars.DefaultDate = this.getHeader('licenseStartDate');
                }

                //Handle Agessa and TaxError validation
                var showAgessa = this.getHeader('AgessaFlag');
                var showTaxError = this.getHeader('TaxError');
                if (showAgessa == 'True' || showTaxError == 'True')
                    $('important').setStyle('display', '');
                if (showTaxError == 'True')
                    $('taxErrorBox').setStyle('display', '');
                if (showAgessa == 'True')
                    $('agessaBox').setStyle('display', '');

                //Check to see if promo code is there and valid
                var validationCode = this.getHeader('promoCodeValidation');
                if (validationCode != 'True') {
                    OpenModal('badPromoCode');
                }
                tmpPricingBox.fade(0);
                pricingBox.set('html', response);
                CorbisUI.ExpressCheckout.Resize();
                CorbisUI.ExpressCheckout.validateStep1();
            },
            onFailure: function() {
                pricingAjaxMsg.set('html', CorbisUI.ExpressCheckout.vars.pricingCalculationErrorMessage);
                CorbisUI.ExpressCheckout.validateStep1();
            }
        }).send();

    },
    RFExpressCheckoutPricingAjax: function() {
        //console.log('RFExpressCheckoutPricingAjax');
        var tmpPricingBox = $('ajaxPriceLoader');

        //console.log(tmpPricingBox);
        var pricingAjaxMsg = tmpPricingBox.getElement('div.msg');
        var pricingBox = $('pricingPane');

        if (CorbisUI.ExpressCheckout.vars.MyPricingList.getSelectedRow() ||
            CorbisUI.ExpressCheckout.vars.PricedByAE == 'True') {

            var thePrice;
            if (CorbisUI.ExpressCheckout.vars.MyPricingList.getSelectedRow()) {

                var el = null;
                el = CorbisUI.ExpressCheckout.vars.MyPricingList.getSelectedRow();
                var inputs = el.getParent().getChildren("input");
                thePrice = inputs[2].value;
            }

            var req = new Request({
                method: 'post',
                url: 'ExpressCheckout_PricingBox.aspx',
                data: { 'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
                    'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
                    'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
                    'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
                    'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
                    'productUid': CorbisUI.ExpressCheckout.vars.HidProductUid.value,
                    'lightboxId': CorbisUI.ExpressCheckout.vars.LightboxId,
                    'hidAttributeValueUID': CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value,
                    'thePrice': thePrice,
                    'currencyCode': CorbisUI.ExpressCheckout.vars.PriceCode.value,
                    'thePaymentMethod': CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid,
                    'thePaymentType': CorbisUI.ExpressCheckout.vars.ThePaymentType,
                    'contractType': CorbisUI.ExpressCheckout.vars.ContractType,
                    'promoCode': CorbisUI.ExpressCheckout.vars.PromoCodeBox.value
                },

                onRequest: function() {
                    //console.log('RFExpressCheckoutPricingAjax onRequest');
                    //console.log(CorbisUI.ExpressCheckout.vars.ProjectField.value);
                    //console.log(CorbisUI.ExpressCheckout.vars.JobField.value);
                    //console.log(CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value);
                    //console.log(CorbisUI.ExpressCheckout.vars.LicenseeField.value);

                    pricingAjaxMsg.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadPricingText);
                    tmpPricingBox.height = pricingBox.getStyle('height');
                    tmpPricingBox.fade(0.9);
                    // Lock Step 2 to prevent multiple requests
                    CorbisUI.ExpressCheckout.lockStep2();

                },
                onComplete: function(response) {
                    //console.log('RFExpressCheckoutPricingAjax onComplete');
                    //CorbisUI.ExpressCheckout.vars.LicenseDetails.set('html', response);
                    if (this.getHeader('licenseStartDate')) {
                        CorbisUI.ExpressCheckout.vars.DefaultDate = this.getHeader('licenseStartDate');
                    }

                    //Handle Agessa and TaxError validation
                    var showAgessa = this.getHeader('AgessaFlag');
                    var showTaxError = this.getHeader('TaxError');
                    if (showAgessa == 'True' || showTaxError == 'True')
                        $('important').setStyle('display', '');
                    if (showTaxError == 'True')
                        $('taxErrorBox').setStyle('display', '');
                    if (showAgessa == 'True')
                        $('agessaBox').setStyle('display', '');


                    //Handle validation from Presenters
                    var validationCode = this.getHeader('validationResult');
                    //console.log(validationCode);
                    switch (validationCode) {
                        case "Failure":
                            //$$('.RMPricingError').setStyle('display', 'block');
                            //console.log('RFExpressCheckoutPricingAjax failure');
                            break;
                        case "PricedByAE":
                            //Probably not RF
                            break;
                        case "NoUsage":
                            break;
                        case "AEPriceExpired":
                            if (!MochaUI.ModalExists('customPriceExpired')) {
                                OpenModal('customPriceExpired');
                            }
                            break;
                        case "Success":
                            break;
                        case "CheckoutNotAllowed":
                            // Modal alert
                            //Probably won't happen in RF
                            OpenModal('imageNotAvailableForCheckout');
                            break;
                    }
                    
                    //Check to see if promo code is there and valid
                    var validationCode = this.getHeader('promoCodeValidation');
                    if (validationCode != 'True') {
                        OpenModal('badPromoCode');
                    }
                    tmpPricingBox.fade(0);
                    pricingBox.set('html', response);
                    CorbisUI.ExpressCheckout.Resize();

                    //Hide currency labels if price is 'Contact us'
                    if (thePrice == 'Contact us') {
                        CorbisUI.ExpressCheckout.lockStep3();
                        OpenModal('imageNotAvailableForCheckout');
                        CorbisUI.ExpressCheckout.vars.PriceCodeDiv.setStyle('display', 'none');
                        CorbisUI.ExpressCheckout.vars.TotalCodeDiv.setStyle('display', 'none');
                        CorbisUI.ExpressCheckout.vars.TotalPrice.setStyle('display', 'none');
                    }

                    // Unlock Step 2 if step 1 is valid
                    CorbisUI.ExpressCheckout.validateStep1();

                    //Resize the iframe window after AJAX request complete
                    CorbisUI.ExpressCheckout.Resize();
                },
                onFailure: function() {
                    //console.log('RFExpressCheckoutPricingAjax onFailure');

                    pricingAjaxMsg.set('html', CorbisUI.ExpressCheckout.vars.pricingCalculationErrorMessage);
                    tmpPricingBox.fade(0);
                    // Unlock Step 2
                    CorbisUI.ExpressCheckout.unlockStep2();
                }
            }).send();
        }
    },
    ShowStartOver: function() {
        //console.log('ShowStartOver');
        $$('.StartOver')[0].setStyle('display', 'block');
        if (Browser.Engine.webkit) $$('.StartOver')[0].setStyle('margin-top', '-10px');
    },
    ShowFavoriteUseLabel: function() {
        //console.log('ShowFavoriteUseLabel');
        CorbisUI.ExpressCheckout.vars.SavedUsageText = CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.options[CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.selectedIndex].text;
        CorbisUI.ExpressCheckout.vars.SavedUsageTruncated = (CorbisUI.ExpressCheckout.vars.SavedUsageText.length > 20) ? CorbisUI.ExpressCheckout.vars.SavedUsageText.substr(0, 20) + '...' : CorbisUI.ExpressCheckout.vars.SavedUsageText;
        CorbisUI.ExpressCheckout.vars.FavoriteUseLabel.set('text', CorbisUI.ExpressCheckout.vars.SavedUsageTruncated);
        CorbisUI.ExpressCheckout.vars.FavoriteUseLabel.set('title', CorbisUI.ExpressCheckout.vars.SavedUsageText);
    },
    ShowPricedByAE: function(licenseStartDateRequired) {
        //console.log('ShowPricedByAE');
        $$('.RMPricingError').setStyle('display', 'none');
        $$('.RMPricingInnerWrap').setStyle('display', 'none');
        $$('.RMPricedAlreadyInnerWrap').setStyle('display', 'none');
        $$('.RMPricedByAEInnerWrap').setStyle('display', 'block');
        CorbisUI.ExpressCheckout.DisplayLicenseStartDateIFF(licenseStartDateRequired);
        this.ShowFavoriteUseLabel();
    },
    ShowPricingExpiredError: function() {
        //console.log('ShowPricingExpiredError');
        $$('.RMPricingError').setStyle('display', 'block');
    },
    OpenCustomPriceExpired: function(delay) {
        OpenModal('customPriceExpired');
    },
    DisplayLicenseStartDateIFF: function(licenseStartDateRequired) {
        if (licenseStartDateRequired == null) {
            licenseStartDateRequired = 'True';
        }
        if (licenseStartDateRequired == 'True') {
            $$('.StartDate').setStyle('display', 'block');
        } else {
            $$('.StartDate').setStyle('display', 'none');
        }
    },
    ShowRMPriced: function(licenseStartDateRequired) {
        //console.log('ShowRMPriced');
        CorbisUI.ExpressCheckout.vars.AlreadyPriced = 'True'
        $$('.RMPricingInnerWrap').setStyle('display', 'none');
        $$('.RMPricedByAEInnerWrap').setStyle('display', 'none');
        $$('.RMPricedAlreadyInnerWrap').setStyle('display', 'block');
        CorbisUI.ExpressCheckout.DisplayLicenseStartDateIFF(licenseStartDateRequired);
        this.ShowFavoriteUseLabel();
    },
    RMProductUidCheck: function() {
        //console.log('RMProductUidCheck');
        //Get productUID and if exists, make subsequent AJAX request
        //Fires on page load (register script in code-behind)
        var hidProductUid = CorbisUI.ExpressCheckout.vars.HidProductUid.value;
        //console.log('RMProductUidCheck RMProductUidCheck ' + hidProductUid);
        //If not blank ProductUid
        if (hidProductUid != '00000000-0000-0000-0000-000000000000' && hidProductUid != '') {

            if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RM') {



                ///Set text for and show Favorite Use labels
                this.ShowFavoriteUseLabel();

                //Get objects to be modified by AJAX request
                CorbisUI.ExpressCheckout.vars.AjaxLoader = $('ajaxLicenseLoader');
                CorbisUI.ExpressCheckout.vars.AjaxMessage = $('ajaxLicenseLoader').getElement('div.msg');
                CorbisUI.ExpressCheckout.vars.AjaxUpdater = CorbisUI.ExpressCheckout.vars.LicenseDetails;

                //Call the daddy Ajax function
                this.ExpressCheckoutAjax(
                    CorbisUI.ExpressCheckout.vars.CorbisID,
                    hidProductUid,
                    CorbisUI.ExpressCheckout.vars.hidSavedUsageUid.value);
                this.validateStep2();
            }
        } else {
            // new usage, set default date
            CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox.value = CorbisUI.ExpressCheckout.vars.DefaultDate;
        }
    },
    ProcessSavedUsageChangeEvent: function() {
        //console.log('ProcessSavedUsageChangeEvent');
        CorbisUI.ExpressCheckout.ShowFavoriteUseLabel();
        CorbisUI.ExpressCheckout.vars.AjaxLoader = $('ajaxLicenseLoader');
        CorbisUI.ExpressCheckout.vars.AjaxMessage = $('ajaxLicenseLoader').getElement('div.msg');
        CorbisUI.ExpressCheckout.vars.AjaxUpdater = CorbisUI.ExpressCheckout.vars.LicenseDetails;

        var myItem = CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.selectedIndex;
        var savedUsageUID = CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.options[myItem].value;
        CorbisUI.ExpressCheckout.vars.hidSavedUsageUid.value = savedUsageUID;

        //Call the daddy Ajax function
        this.ExpressCheckoutAjax(
            CorbisUI.ExpressCheckout.vars.CorbisID,
            CorbisUI.ExpressCheckout.vars.HidProductUid.value,
            CorbisUI.ExpressCheckout.vars.hidSavedUsageUid.value);
        this.validateStep2();
        CorbisUI.ExpressCheckout.validateStep2();
        $$('.licenseDetailsHeaderLbl').each(function(el) {

            el.setStyle('display', 'inline');

        });
        $('createNewUsageDiv').setStyle('display', 'none');
        $('Or').setStyle('display', 'none');

        CorbisUI.ExpressCheckout.ShowStartOver();
    },
    RMRegisterSelectEvent: function(CorbisId, savedUsageUID, AttributeValueUid) {

        //console.log('RMRegisterSelectEvent');

        //Call the daddy Ajax function
        this.RMProductUidCheck();

        //Attach AJAX call to each list item change event
        CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.addEvent('change', function(el) {
            CorbisUI.ExpressCheckout.ProcessSavedUsageChangeEvent();
        });
    },
    setPriceLabel: function(amount, priceCode, priceLabel) {
        //console.log('setPriceLabel');
        if (isNaN(amount)) {
            var currentPrice = priceLabel.innerHTML;
            priceLabel.innerHTML = amount;
            priceCode.style.display = 'none';
        } else {
            if (isNaN(parseFloat(priceLabel.innerHTML))) {
                priceLabel.innerHTML = "0.00";
            }
            var currentPrice = parseFloat(priceLabel.innerHTML);
            var gotoPrice = parseFloat(amount);

            priceLabel.innerHTML = gotoPrice.toFixed(2);

        }
    },
    NoPaymentMethod: function() {
        //If we get a 'true' response from the server, launch Default Payment method dialog
        //console.log('NoPaymentMethod');
        if (CorbisUI.ExpressCheckout.vars.invalidPayment == true)
            OpenModal('noDefaultPayment');
    },
    UpdatePaymentInfo: function() {
        CorbisUI.ExpressCheckout.vars.hidPaymentMethod.value = CorbisUI.ExpressCheckout.vars.ThePaymentType;
        if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CreditCard') {
            CorbisUI.ExpressCheckout.vars.creditCardUID.value = CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid;
        } else {
            CorbisUI.ExpressCheckout.vars.corporateID.value = CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid;
        }

        //If we get a 'true' response from the server, show update credit card error DIV
        if (CorbisUI.ExpressCheckout.vars.CardExpired == 'True') {
            $$('.CreditCardExpiryError').removeClass('displayNone');
            $$('.creditCardValidationCodeBox').addClass('displayNone');
            CorbisUI.ExpressCheckout.Resize();
        }
        else {
            //Fire off AJAX request to update payment box
            CorbisUI.ExpressCheckout.vars.AjaxLoader = $('ajaxPriceLoader');
            var pricingAjaxMsg = $('ajaxPriceLoader').getElement('div.msg');

            //Hide error box
            $$('.CreditCardExpiryError').each(function(el) {
                el.addClass('displayNone');
            });
            if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CreditCard')
                $$('.creditCardValidationCodeBox').removeClass('displayNone');

            if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RF') {
                //console.log('UpdatePaymentInfo RF')
                this.RFExpressCheckoutPricingAjax();

            } else {
                //console.log('UpdatePaymentInfo RM');
                //Make sure product UID is not empty

                if (CorbisUI.ExpressCheckout.vars.HidProductUid.value != '00000000-0000-0000-0000-000000000000' && CorbisUI.ExpressCheckout.vars.HidProductUid.value != '') {
                    if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RM') {
                        if (CorbisUI.ExpressCheckout.vars.SavedUsageDropdown.selectedIndex != 0) {
                            this.RMExpressCheckoutPricingAjax();
                        }
                    } else {
                        this.RMExpressCheckoutPricingAjax();
                    }
                }
            }
            CorbisUI.ExpressCheckout.Resize();
        }
    },
    IsValidPaymentMethod: function() {
        try {
            CorbisUI.ExpressCheckout.vars.CardExpired = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].getProperty('cardExpired');
            CorbisUI.ExpressCheckout.vars.ThePaymentType = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].getProperty('type');
        }
        catch (Ex) {
            CorbisUI.ExpressCheckout.vars.CardExpired = 'False';
            CorbisUI.ExpressCheckout.vars.ThePaymentType = 'none';
        }

        if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CreditCard') {
            if (CorbisUI.ExpressCheckout.vars.CardExpired == 'True')
                return false;
        }
        return true;
    },
    ProcessPaymentMethodSelectionChange: function(event, paymentValidation) {
        //First validate step 1 and step2 and then process the damn event. User can always move the mouse from step1/step2 and still
        //successfully change the dropdown value even after providing invalid values in step1 step2
        if (!this.CheckLicenseeFieldValid() || !this.doValidateStartDate(this.GetStartDate())) {
            if (currentPaymentMethodSelection != null) {
                CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex = currentPaymentMethodSelection;
            }
            if (!paymentValidation) {
                return false;
            } else {
                //See bug 17931. When the page is first loaded the payment validation need to be triggered so process the payment
                //selection change.
            }
        }
        try {
            CorbisUI.ExpressCheckout.vars.CardExpired = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].getProperty('cardExpired');
            CorbisUI.ExpressCheckout.vars.ThePaymentType = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].getProperty('type');
        }
        catch (Ex) {
            CorbisUI.ExpressCheckout.vars.CardExpired = 'False';
            CorbisUI.ExpressCheckout.vars.ThePaymentType = 'none';
        }

        CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].value;
        CorbisUI.ExpressCheckout.vars.hidPaymentMethod.value = CorbisUI.ExpressCheckout.vars.ThePaymentType;


        if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CreditCard') {
            CorbisUI.ExpressCheckout.vars.creditCardUID.value = CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid;
            $$('.creditCardValidationCodeBox').removeClass('displayNone');

        } else {
            if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CorporateAccount') {
                CorbisUI.ExpressCheckout.vars.corporateID.value = CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid;
            }

            $$('.creditCardValidationCodeBox').addClass('displayNone');

        }

        CorbisUI.ExpressCheckout.validateStep3();
        var mySelected = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.getSelected()[0].value;

        if (mySelected == 'Add New')
            CorbisUI.ExpressCheckout.newCreditCardModal('selectUseInner');

        //if (mySelected != 'Add New' && mySelected != 'none') // - Commented for Bug 16518
        if (mySelected != 'Add New')                           // - Added for bug 16518
            CorbisUI.ExpressCheckout.UpdatePaymentInfo();
    },
    SetupPaymentMethod: function() {
        CorbisUI.ExpressCheckout.vars.PaymentMethodBox.addEvent('focus', function(event) {
            currentPaymentMethodSelection = CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex;
        });
        CorbisUI.ExpressCheckout.vars.PaymentMethodBox.addEvent('change', function(event) {
            CorbisUI.ExpressCheckout.ProcessPaymentMethodSelectionChange(event);
        });
    },
    RefreshPaymentMethods: function(ccUid) {
        CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid = ccUid;
        CorbisUI.ExpressCheckout.vars.ThePaymentType = 'CreditCard';
        var req = new Request({
            method: 'post',
            url: 'ExpressCheckout_PaymentMethod.aspx',
            onSuccess: function(response) {

                //console.log(ccUid);

                var creditCardsString = '<select id="paymentMethodSelect">';
                creditCardsString += response.substring(response.indexOf("<option"), response.lastIndexOf("</option") + 9);
                creditCardsString += '</select>';


                $('paymentMethodHolder').set('html', creditCardsString);

                CorbisUI.ExpressCheckout.vars.PaymentMethodBox = $('paymentMethodSelect'); //Resetting global for loop below
                for (var i = 0; i < $('paymentMethodSelect').options.length; i++) {

                    //console.log(CorbisUI.ExpressCheckout.vars.PaymentMethodBox.options[i].value + " : " + CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid);
                    if (CorbisUI.ExpressCheckout.vars.PaymentMethodBox.options[i].value == CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid) {

                        CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex = i;
                    }

                }
                CorbisUI.ExpressCheckout.Resize();
                //console.log(CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex);
                CorbisUI.ExpressCheckout.SetupPaymentMethod();
                CorbisUI.ExpressCheckout.ProcessPaymentMethodSelectionChange();

            }
        }).send();

    },
    setPromoButtonDisabled: function(btn, disable) {
        var gb = CorbisUI.ExpressCheckout.vars.PromoCodeButton;

        var aItem = gb.getElements('a')[0];
        var spanElement = null;
        var oldColor = null;
        if (aItem != null) {
            var parent = aItem.getParent();
            spanElement = parent;
            parent.removeChild(aItem);
            oldColor = aItem.getProperty('oldColor');
            if (oldColor == null) {
                aItem.getProperty('color');
            }
        } else {
            spanElement = gb.getLast('span');
        }
        var applyAnchor = new Element('a', {
            'href': aItem.getProperty('href'),
            'class': aItem.getProperty('class'),
            'html': aItem.getProperty('html'),
            'title': aItem.getProperty('title'),
            'events': {
                'click': function() {
                    if (!disable) {
                        CorbisUI.ExpressCheckout.ApplyPromoCode();
                        return false;
                    }
                }
            }
        });
        if (!applyAnchor.hasClass('enabled'))
            applyAnchor.addClass('enabled');
        if (!applyAnchor.hasClass('disabled'))
            applyAnchor.addClass('disabled');

        applyAnchor.removeClass(disable ? 'enabled' : 'disabled').addClass(disable ? 'disabled' : 'enabled').inject(spanElement);
    },
    togglePromoButtonDisabled: function() {
        var gb = CorbisUI.ExpressCheckout.vars.PromoCodeButton;

        setOutlineButtonDisabled(gb, !gb.hasClass('DisabledGlassButton'));
        return false;
    },
    PromoCodeListener: function() {
        CorbisUI.ExpressCheckout.vars.PromoCodeBox.addEvent('keyup', function(el) {
            var gb = CorbisUI.ExpressCheckout.vars.PromoCodeButton;
            var promoBoxText = CorbisUI.ExpressCheckout.vars.PromoCodeBox.value;
            if (promoBoxText != '') {
                CorbisUI.ExpressCheckout.setPromoButtonDisabled(gb, false);
            } else {
                CorbisUI.ExpressCheckout.setPromoButtonDisabled(gb, true);
            }
            CorbisUI.ExpressCheckout.validatePromoButton();
        });
    },
    ApplyPromoCode: function() {
        //make sure there is a value before sending ajax request

        if ((CorbisUI.ExpressCheckout.vars.PromoCodeBox.value != '' || CorbisUI.ExpressCheckout.vars.PromoCodeBox.text != '') && CorbisUI.ExpressCheckout.vars.PaymentMethodBox[CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex].value != 'none')
            CorbisUI.ExpressCheckout.UpdatePaymentInfo();
    },
    ViewRestrictions: function(element) {
        //console.log('ViewRestrictions');
        //make ajax call
        Corbis.Web.UI.Checkout.CheckoutService.GetViewRestrictions(CorbisUI.ExpressCheckout.vars.CorbisID, this.GetViewRestrictionsCompleted, this.FailedCallback, element, null);

    },
    GetViewRestrictionsCompleted: function(results, context, methodName) {
        //console.log('GetViewRestrictionsCompleted');
        var photo = CorbisUI.ExpressCheckout.vars.ImageThumbnail;
        var margin = photo.getStyle('margin-top').toInt();
        margin = margin * 1.8;
        var widthheight;
        if (margin == 0)
            widthheight = "height:90px;"
        else
            widthheight = "width:90px;";
        var url = '<div class="thumbWrap"><img src="' + photo.getProperty('src') + '" style="margin-top:' + margin + 'px;' + widthheight + '" /></div>';
        var restrictions = '';
        results.Restrications.each(function(item) { restrictions = restrictions + '<p>' + item.Notice + '</p>'; });
        new CorbisUI.Popup('restrictionsPopup', {
            createFromHTML: true,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $(context),
            width: 550,
            positionVert: -190,
            positionHoriz: 0,
            replaceText: [url, results.ModelReleaseStatus, results.PropertyReleaseStatusText, restrictions]
        });
    },
    openProgressModalOverlay: function(modalId, container) {
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false
        };
        new CorbisUI.Popup('downloadProgress', options);
        $('modalOverlay').setStyle('opacity', 0.7);
    },
    submitExpressCheckoutForm: function() {
        var startDate = CorbisUI.ExpressCheckout.GetStartDate();
        var asynch = true;
        if (Browser.Engine.webkit)
            asynch = false;
        //For Bug 16115. Donot submit the form synchronous but instead submit the form asynchronously
        var url = String.format(CorbisUI.ExpressCheckout.vars.ExpressCheckoutSubmitUrl,
            CorbisUI.ExpressCheckout.vars.CorbisID,
            CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value,
            CorbisUI.ExpressCheckout.vars.PriceCode.value,
            CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid,
            CorbisUI.ExpressCheckout.vars.ThePaymentType,
            CorbisUI.ExpressCheckout.vars.ContractType,
            CorbisUI.ExpressCheckout.vars.PromoCodeBox.value,
            CorbisUI.ExpressCheckout.vars.LightboxId,
            startDate
        );
        if (!asynch) {
            theForm.action = url;
            theForm.submit();
            return;
        } else {
            theForm.set('send', {
                url: url, method: 'post',
                onRequest: function() {
                    CorbisUI.ExpressCheckout.openProgressModalOverlay('downloadProgress', 'downloadProgress');
                },
                onSuccess: function(response) {
                    var doc = new Request.HTML().processHTML(response);
                    window.location.href = $(doc).getElements('input[name$=hidRedirectUrl]')[0].value;
                },
                onFailure: function(response) {
                    window.location.href = $(doc).getElements('input[name$=hidRedirectUrl]')[0].value;
                }
            }
            );

            theForm.send();

        }
    },
    submitValidationCheck: function() {
        //console.log('submitValidationCheck');
        if (!this.CheckLicenseeFieldValid() || !this.doValidateStartDate(this.GetStartDate())) {
            if (currentPaymentMethodSelection != null) {
                CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex = currentPaymentMethodSelection;
            }
            return false;
        }
        if (!this.IsValidPaymentMethod()) {
            this.ProcessPaymentMethodSelectionChange();
            return false;
        }
        var CVCValue = CorbisUI.ExpressCheckout.vars.CreditCardCVC.value;
        //This hack is needed to raise the button click event on the server side
        CorbisUI.ExpressCheckout.vars.SubmitPurchaseClicked.value = "True";
        if (CorbisUI.ExpressCheckout.vars.ThePaymentType == 'CreditCard') {

            if (CorbisUI.ExpressCheckout.ASCCompliant == "False" || CorbisUI.ExpressCheckout.ASCCompliant == "undefined") {
                return false;
            }
            if (CVCValue != '' && CVCValue != null && CVCValue != 'undefined') {
                if (isNaN(CVCValue) || CVCValue.length < 3 || CVCValue.length > 4) {
                    $$('.creditCardValidationCodeBox').addClass('Error');

                    return false;  //NO pass validation
                }

            } else {

                $$('.creditCardValidationCodeBox').addClass('Error');
                return false;  //NO pass validation
            }
        }
        //Also run promo code validation
        var tmpLicenseBox = CorbisUI.ExpressCheckout.vars.AjaxLoader;
        var licenseAjaxMsg = tmpLicenseBox.getElement('div.msg');
        var thePrice;
        if (CorbisUI.ExpressCheckout.vars.LicenseDetails == "RF") {
            el = CorbisUI.ExpressCheckout.vars.MyPricingList.getSelectedRow();

            var inputs = el.getParent().getChildren("input");
            thePrice = inputs[2].value;
        }
        var a = false;
        var complete = false;

        var req = new Request({
            method: 'post',
            url: 'ExpressCheckout_PricingBox.aspx',
            data: { 'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
                'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
                'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
                'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
                'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
                'productUid': CorbisUI.ExpressCheckout.vars.HidProductUid.value,
                'lightboxId': CorbisUI.ExpressCheckout.vars.LightboxId,
                'hidAttributeValueUID': CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value,
                'thePrice': thePrice,
                'currencyCode': CorbisUI.ExpressCheckout.vars.PriceCode.value,
                'thePaymentMethod': CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid,
                'thePaymentType': CorbisUI.ExpressCheckout.vars.ThePaymentType,
                'contractType': CorbisUI.ExpressCheckout.vars.ContractType,
                'promoCode': CorbisUI.ExpressCheckout.vars.PromoCodeBox.value,
                'createOrder': CorbisUI.ExpressCheckout.vars.SubmitPurchaseClicked.value
            },
            onRequest: function() {

                //console.log('submitValidationCheck onRequest');
                licenseAjaxMsg.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadPricingText);
                tmpLicenseBox.fade(0.9);
                // Lock Step 2 to prevent multiple requests
                CorbisUI.ExpressCheckout.lockStep2();
            },
            onSuccess: function(response) {
                //Handle Agessa validation
                licenseAjaxMsg.set('html', response);
                tmpLicenseBox.fade(0);
                //Handle Agessa and TaxError validation
                var showAgessa = this.getHeader('AgessaFlag');
                var showTaxError = this.getHeader('TaxError');
                if (showAgessa == 'True' || showTaxError == 'True')
                    $('important').setStyle('display', '');
                if (showTaxError == 'True')
                    $('taxErrorBox').setStyle('display', '');
                if (showAgessa == 'True')
                    $('agessaBox').setStyle('display', '');

                //handle validation for Promo code
                var validationCode = this.getHeader('promoCodeValidation');
                if (validationCode != 'True') {
                    OpenModal('badPromoCode');
                } else {
                    CorbisUI.ExpressCheckout.submitExpressCheckoutForm();
                }
                tmpLicenseBox.fade(0);
                // Set purchaseClicked to false and Unlock Step 2
                CorbisUI.ExpressCheckout.vars.SubmitPurchaseClicked.value = 'False';


            },
            onFailure: function(response) {
                licenseAjaxMsg.set('html', CorbisUI.ExpressCheckout.vars.pricingCalculationErrorMessage);
                tmpLicenseBox.fade(0);
                // Set purchaseClicked to false and Unlock Step 2
                CorbisUI.ExpressCheckout.vars.SubmitPurchaseClicked.value = 'False';


            }
        }).send();

        return false;
    },
    FailedCallback: function(error) {
        return false;
    },
    OpenLearnMore: function(element) {
        new CorbisUI.Popup('learnMorePopup', {
            showModalBackground: false,
            centerOverElement: element,
            closeOnLoseFocus: true,
            positionVert: 'bottom',
            positionHoriz: 'right'
        });
    },

    newCreditCardModal: function() {
        OpenNewIModal('EditPaymentInformation.aspx?mode=add', '400', '200', 'editPaymentInfoModalPopup');
    },
    editCreditCardModal: function(ccUid) {
        OpenNewIModal('EditPaymentInformation.aspx?ccUid=' + ccUid, '400', '200', 'editPaymentInfoModalPopup');
    }
});

/***************************
RF PRICING LIST CLASS
****************************/
var PricingList = new Class({

    initialize: function(elements, options) {
        //console.log('PricingList initialize');
        this.setupListEvents();

    },
    setupListEvents: function() {
        //console.log('PricingList setupListEvents');
        $$('div.RFPricingRepeater ul').each(function(el) {
            //Reset some styling that may have been done by click event

            var row = el.getParent();
            row.setStyle('color', 'black');
            row.setStyle('cursor', 'pointer');
            el.addEvents({
                'mouseover': function() {
                    this.pricingListOn(el);
                } .bind(this),
                'mouseout': function() {
                    this.pricingListOut(el);
                } .bind(this),
                'mousedown': function() {
                    this.pricingListMouseDown(el);
                } .bind(this),
                'click': function(event) {
                    $$('div.RFPricingRepeater ul').each(function(ele) {
                        this.pricingListOut(ele);
                    }, this);

                    this.pricingListClick(el);

                    //make the ajax call 
                    var tmpPricingBox = $('ajaxPriceLoader');
                    var pricingAjaxMsg = tmpPricingBox.getElement('div.msg');
                    var pricingBox = $('pricingPane');
                    var inputs = el.getParent().getChildren("input");
                    var thePrice = inputs[2].value;

                    if (CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex > 1 && thePrice != 'Contact us') { // make sure not to do request if no payment method
                        var req = new Request({
                            method: 'post',
                            url: 'ExpressCheckout_PricingBox.aspx',
                            data: { 'CorbisId': CorbisUI.ExpressCheckout.vars.CorbisID,
                                'projectName': CorbisUI.ExpressCheckout.vars.ProjectField.value,
                                'jobNumber': CorbisUI.ExpressCheckout.vars.JobField.value,
                                'poNumber': CorbisUI.ExpressCheckout.vars.PurchaseOrderField.value,
                                'licensee': CorbisUI.ExpressCheckout.vars.LicenseeField.value,
                                'productUid': CorbisUI.ExpressCheckout.vars.HidProductUid.value,
                                'lightboxId': CorbisUI.ExpressCheckout.vars.LightboxId,
                                'hidAttributeValueUID': CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value,
                                'thePrice': thePrice,
                                'currencyCode': CorbisUI.ExpressCheckout.vars.PriceCode.value,
                                'thePaymentMethod': CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid,
                                'thePaymentType': CorbisUI.ExpressCheckout.vars.ThePaymentType,
                                'contractType': CorbisUI.ExpressCheckout.vars.ContractType,
                                'promoCode': CorbisUI.ExpressCheckout.vars.PromoCodeBox.value
                            },
                            onRequest: function() {
                                pricingAjaxMsg.set('html', '<img border="0" alt="" src="/images/ajax-loader.gif" /><br />' + CorbisUI.ExpressCheckout.vars.LoadPricingText);
                                tmpPricingBox.height = pricingBox.getStyle('height');
                                tmpPricingBox.fade(0.9);
                                // Lock Step 2 to prevent multiple requests
                                CorbisUI.ExpressCheckout.lockStep2();
                            },
                            onComplete: function(response) {

                                //Handle Agessa and TaxError validation
                                var showAgessa = this.getHeader('AgessaFlag');
                                var showTaxError = this.getHeader('TaxError');
                                if (showAgessa == 'True' || showTaxError == 'True')
                                    $('important').setStyle('display', '');
                                if (showTaxError == 'True')
                                    $('taxErrorBox').setStyle('display', '');
                                if (showAgessa == 'True')
                                    $('agessaBox').setStyle('display', '');

                                var validationCode = this.getHeader('promoCodeValidation');
                                if (validationCode != 'True') {
                                    OpenModal('badPromoCode');
                                }

                                pricingBox.set('html', response);
                                tmpPricingBox.fade(0);
                                // Unlock Step 2
                                CorbisUI.ExpressCheckout.unlockStep2();

                                CorbisUI.ExpressCheckout.Resize();
                            },
                            onFailure: function(xhr) {

                                pricingAjaxMsg.set('html', '<p>There was an error. Please try again. ' + xhr + '</p>');
                                // Unlock Step 2
                                CorbisUI.ExpressCheckout.unlockStep2();
                            }
                        }).send();
                    } else {
                        if (CorbisUI.ExpressCheckout.vars.PriceLabel)
                            CorbisUI.ExpressCheckout.setPriceLabel(inputs[2].value, CorbisUI.ExpressCheckout.vars.PriceCode, CorbisUI.ExpressCheckout.vars.PriceLabel);

                        //Hide currency labels if price is 'Contact us'
                        if (thePrice == 'Contact us') {
                            CorbisUI.ExpressCheckout.vars.PriceCodeDiv.setStyle('display', 'none');
                            CorbisUI.ExpressCheckout.vars.TotalCodeDiv.setStyle('display', 'none');
                            CorbisUI.ExpressCheckout.vars.TotalPrice.setStyle('display', 'none');
                            CorbisUI.ExpressCheckout.lockStep3();
                            OpenModal('imageNotAvailableForCheckout');

                        }
                    }



                } .bind(this)
            });

        }, this);
    },
    pricingListOn: function(el) {
        //console.log('PricingList pricingListOn');
        //Move background positions for mouse over
        var row = el.getParent();
        row.getPrevious().setStyle('background-position', '0 -90px');
        row.setStyle('background-position', '0 -120px');
        row.getNext().setStyle('background-position', '0 -150px');
    },
    pricingListOut: function(el) {
        //console.log('PricingList pricingListOut');
        //Move background position for mouseout
        var row = el.getParent();
        row.getPrevious().setStyle('background-position', '0 0px');
        row.setStyle('background-position', '0 -30px');
        row.getNext().setStyle('background-position', '0 -60px');
    },
    pricingListMouseDown: function(el) {
        //console.log('PricingList pricingListMouseDown');
        var row = el.getParent();
        row.getPrevious().setStyle('background-position', '0 -180px');
        row.setStyle('background-position', '0 -210px');
        row.getNext().setStyle('background-position', '0 -240px');
    },
    getSelectedRow: function() {
        //console.log('PricingList getSelectedRow');
        var tmpEL = null;
        $$('div.RFPricingRepeater ul').each(function(el) {
            //console.log(el);
            //console.log(el.getStyle('background-position'));
            var tmpBGStyle = el.getParent().getStyle('background-position');
            if (tmpBGStyle == '0px -210px' || tmpBGStyle == '0pt -210px') {
                tmpEL = el;
            }
        });
        return tmpEL;
    },
    doRFClick: function(el) {
        //console.log('doRFClick');
        //console.log(el);
        var inputs = el.getParent().getChildren("input");
        // Att UID
        CorbisUI.ExpressCheckout.vars.HidAttributeUID.value = inputs[0].value;
        // Value UID
        CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value = inputs[1].value;
        CorbisUI.ExpressCheckout.validateStep2();
        $$('div .imageContainer').setStyle('display', 'none');
        $$('div .licenseAlertDiv').setStyle('display', 'none');
        CorbisUI.ExpressCheckout.vars.DataChanged = true;

        if (CorbisUI.ExpressCheckout.vars.PriceLabel)
            CorbisUI.ExpressCheckout.setPriceLabel(inputs[2].value, CorbisUI.ExpressCheckout.vars.PriceCode, CorbisUI.ExpressCheckout.vars.PriceLabel);
    },

    pricingListClick: function(el) {
        //console.log('pricingListClick');

        this.doRFClick(el);

        //move backgroud position for click
        //also remove mouseout and mouseover events and reset any removed events to other items
        //Then finally send ValueUID to price changer
        $$('ul').removeEvents();
        this.setupListEvents();

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
    
// lets focus on something  
window.addEvent('load', function() {
    $('expressCheckoutStageWrap')
        .getElement('input[id$=projectField]').focus();
});
    
