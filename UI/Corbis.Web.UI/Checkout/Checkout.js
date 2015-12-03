/*                      BATMAN WAS HERE

MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MM.:  .:'   `:::  .:`MMMMMMMMMMM|`MMM'|MMMMMMMMMMM':  .:'   `:::  .:'.MM
MMMM.     :          `MMMMMMMMMM  :*'  MMMMMMMMMM'        :        .MMMM
MMMMM.    ::    .     `MMMMMMMM'  ::   `MMMMMMMM'   .     ::   .  .MMMMM
MMMMMM. :   :: ::'  :   :: ::'  :   :: ::'      :: ::'  :   :: ::.MMMMMM
MMMMMMM    ;::         ;::         ;::         ;::         ;::   MMMMMMM
MMMMMMM .:'   `:::  .:'   `:::  .:'   `:::  .:'   `:::  .:'   `::MMMMMMM
MMMMMM'     :           :           :           :           :    `MMMMMM
MMMMM'______::____      ::    .     ::    .     ::     ___._::____`MMMMM
MMMMMMMMMMMMMMMMMMM`---._ :: ::'  :   :: ::'  _.--::MMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMM::.         ::  .--MMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMM-.     ;::-MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM. .:' .M:F_P:MMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM.   .MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\ /MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMVMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM

 
*/
/******************************
CORBIS CHECKOUT
*******************************/

/***************************
    GLOBAL VARIABLES
****************************/
var projectCheck, thumbTips, checkoutTabs, ccVerificationTips;

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}

CorbisUI.Checkout = {

    closeAllNoticeWindows: function() {
        var eles = $$('div.fc-tbx');
        if (eles.length > 0) {
            eles.each(function(el) {
                el.getElement('a.close').fireEvent('mouseup');
            });
        }
    },

    //note:this is the same as in another place, we will merge it to common.js later
    registerTooltips: function(isFirstTime) {

        $('aspnetForm').getAllNext('.TIP-license-details').destroy();
        var tipShowDelay = 500;
        var tipHideDelay = 250;
        var tipShowMethod = "in";
        var tipHideMethod = "out";
        if (Browser.Engine.trident) {
            tipShowDelay = 0;
            tipHideDelay = 0;
            tipShowMethod = "show";
            tipHideMethod = "hide";
        }
        thumbTips = new Tips('.thumbWrap', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            className: 'TIP-license-details mochaContent',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });

        ccVerificationTips = new Tips('.iconhelp', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            className: 'TIP-cc-verification',
            offsets: { x: 8, y: -150 },
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });


    },

    setupThumbTips: function() {

    },

    setupCCVTips: function() {

    },

    Tabs: new Class({
        Implements: Options,
        options: {
            //The default value of our options 
            //If we don't specify any of these, the 
            //default values are used 
            selectedTabCssClass: 'selected',
            selectedSectionCssClass: 'selected',
            firstShow: 0,
            trueTabs: true,
            name: '',
            dataBackupControl: null
        },
        tabs: [],
        initialize: function(containers, tabs, options)
        //containers is the sub content
        //tabs is on the top
        {
            if (options) {
                this.setOptions(options);
            }
            //We need to make sure that the containers 
            //and tabs are an Elements collection 
            //so we pass them each through $$ 
            containers = $$(containers);
            //For each tab passed in, we'll iterate over it 
            //and pass both the tab and the corresponding 
            //container to our new method that adds sections for 
            //us 
            $$(tabs).each(function(tab, index) {
                this.addSection(tab, containers[index]);
            }, this);
            this.show(this.options.firstShow); //Show the first tab on startup
        },

        addSection: function(tab, container) {
            //Include the tab in the tabs array; use 
            //.include in case for some reason, it's already 
            //in there 
            this.tabs.include(tab);
            //Store a reference between the tab and its 
            //container 
            tab.store('container', container);
            //Pass the tab to our attach method 
            this.attach(tab);
        },
        //Our attach method has changed; now it takes 
        //as its argument a single tab to monitor 
        attach: function(tab) {
            tab.addEvent('click', function(event) {
                UpdateBasedOnCurrentTabIndex(event);
                this.resetError();
                event.preventDefault();
                // hide any validation notices that might be up
                CorbisUI.Checkout.closeAllNoticeWindows();

                //                if (this.options.dataBackupControl != null) {
                //                    this.options.dataBackupControl.value = tab.id;
                //                }

                //And we send the instruction to display a 
                //tab's content to a new "show" method, which 
                //can be invoked at any time, not just here

                this.show(this.tabs.indexOf(tab));
            } .bind(this));
        },
        resetError: function() {
            if (this.options.name == 'payment') {
                $(CorbisUI.Checkout.PaymentOptions.weDontUnderstandError).setStyle('display', 'none');
            }
        },
        show: function(index, isValidating) {
            if (this.options.trueTabs && !isValidating && (index > this.current)) return;
            if (index == -1) return;
            if (this.current === index) return;
            this.tabs.each(function(tab, i) {
                var container = tab.retrieve('container');
                //If we're showing the tab, add the CSS classes, 
                //else remove them
                if (index === i) {
                    if (this.options.dataBackupControl != null) {
                        this.options.dataBackupControl.value = tab.id;
                    }
                    tab.addClass(this.options.selectedTabCssClass);
                    if (container != null) {
                        container.addClass(this.options.selectedSectionCssClass);
                        container.setStyle('display', 'block');
                    }
                } else {
                    // added this as part of the tab navigation fix
                    if (!tab.hasClass('disabled') && (i > index) && this.options.trueTabs) tab.addClass('disabled');

                    tab.removeClass(this.options.selectedTabCssClass);
                    if (container != null) {
                        container.removeClass(this.options.selectedSectionCssClass);
                        container.setStyle('display', 'none');
                    }
                }
                if (tab.hasClass('tab_ON')) tab.removeClass('tab_ON');
            }, this); //Now we're using 'this' inside this 
            //function, so we must specify a binding  
            //here to keep 'this' pointed to our  
            //instance. 
            this.current = index;
        }
        /*/,
        deselectAll: function() {

            this.tabs.each(function(tab, i) {
        var container = tab.retrieve('container');
        tab.removeClass(this.options.selectedTabCssClass);
        if (container != null) {
        container.removeClass(this.options.selectedSectionCssClass);
        container.setStyle('display', 'none');
        }
        }, this);
        this.current = -10;
        }
        /**/
    }),

    checkOutTabsHover: function(e, index) {
        //console.log(index + " : " + checkoutTabs.current);
        //console.log((index < checkoutTabs.current));
        //console.log(e.toSource());
        if ((index != checkoutTabs.current) && (index < checkoutTabs.current)) {

            switch (e.type) {

                case "mouseover":
                    this.addClass('tab_ON');
                    this.removeClass('disabled');
                    break;
                case "mouseout":
                    this.removeClass('tab_ON');
                    this.addClass('disabled');
                    break;
            }
        }
    },
    /* reuse the same code as order page */
    OpenProgressModal: function() {
        keyWatch.start();
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false
        };
        new CorbisUI.Popup('downloadProgress', options);
        $('modalOverlay').setStyle('opacity', 0.8);
    },

    setPaymentTabs: function() {
        paymentTabs = new CorbisUI.Checkout.Tabs(
                            null,
                            $$('#' + CorbisUI.GlobalVars.MainCheckout.Tabs.paymentTabs + ' a'),
                            {
                                selectedTabCssClass: "selected",
                                firstShow: paymentTabsIndex,
                                trueTabs: false,
                                name: 'payment',
                                dataBackupControl: $(document.body).getElement('input[id$=selectedPayment]')
                            }
                       );
    }
};

/*****************************************
    Delivery options selector functions
******************************************/
	
CorbisUI.Checkout.DeliveryOptions = {
    DeliveryMethod:null,
    selectedDeliveryMethod:null
   
};
 /*,
    initialize: function(){
        deliveryOptionContainer = $$('div.optionsBlock');
        deliveryOptionContainer.get('a');
       
        deliveryOptionContainer.each(function(el){
            console.log(el);
            el.addEvents({
                'click': addClass('selected').bindWithEvent(el, index),
                'mouseover': el.addClass('hover'),
                'mouseout': el.removeClass('hover')
            });
        });
    }*/
/*****************************************
    Payment options selector functions
******************************************/

CorbisUI.Checkout.PaymentOptions = {
    CorporatePane: null,
    CreditPane: null,
    selectedPayment: null
};

/*****************************************
    MODAL functions for checkout
******************************************/

CorbisUI.Checkout.modals = {

    securityQuestions: function(context, notInFooter, newPosition) {

        LogOmnitureEvent("event37");  
        
        var modalPosition = {
            vertical: "top",
            horizontal: -5
        }

        if (notInFooter) modalPosition = newPosition;

        new CorbisUI.Popup('securityQuestionsModal', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $(context),
            width: 500,
            height: 300,
            positionVert: modalPosition.vertical,
            positionHoriz: modalPosition.horizontal
        });
    },

    shippingDeliveryQuestions: function(context) {

        LogOmnitureEvent("event38");  
        
        new CorbisUI.Popup('shippingDeliveryModal', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $(context),
            width: 500,
            height: 300,
            positionVert: 'top',
            positionHoriz: -5
        });
    },

    shippingPriorityQuestions: function(context) {

        new CorbisUI.Popup('shippingPriorityModal', {
            createFromHTML: false,
            showModalBackground: true,
            closeOnLoseFocus: false,
            //centerOverElement: $(context), 
            width: 340,
            height: 450
            //            ,
            //            positionVert: 'bottom',
            //            positionHoriz: -5
            //            
        });
    },

    emailConfirmationQuestion: function(context) {

        new CorbisUI.Popup('emailConfirmationLayer', {
            createFromHTML: true,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $(context),
            width: 360,
            height: 300,
            positionVert: 'top',
            positionHoriz: -5
        });
    },
    addShippingAddress: function(context) {

        new CorbisUI.Popup('addShippingLayer2', {
            createFromHTML: false,
            showModalBackground: true,
            closeOnLoseFocus: false,
            centerOverElement: $(context)
            //            , 
            //            positionVert: 'middle', 
            //            positionHoriz: 'middle'
            //
        });
    },
    quitCheckout: function(context) {

        new CorbisUI.Popup('quitCheckoutModal', {
            createFromHTML: true,
            showModalBackground: true,
            closeOnLoseFocus: false,
            centerOverElement: $(context),
            width: 360,
            height: 300,
            positionVert: 'middle',
            positionHoriz: 'right'
        });
    },

    newCreditCardModal: function(createNew) {
        source = "EditPaymentInformation.aspx";
        if (createNew) {
            source = source + "?mode=add";
        }
        else {
            source = source + "?DefUid=" + $(CorbisUI.Checkout.PaymentOptions.selectedCreditUid).value;
        }
        OpenNewIModal(source, 400, 280, 'editPaymentInfoModalPopup');
    },

    addNewShippingAddress: function() {

        OpenModal2('editShippingAddressModalPopup', 'shippingPriorityModalWindow');

        // now reset it - this happens only when it opens

        // this doesn't work. needs to be an event handler on the button and x

        //var NSAW = $('editShippingAddressModalPopupWindow');
        //var NSAWform = new CorbisUI.Checkout.modals.newShippingReset(NSAW);
        //NSAWform = $lambda(false);

    },

    newShippingReset: new Class({
        items: [],
        window: null,
        initialize: function(el) {
            this.window = el;
            this.items = el.getElement('div.ModalPopupContent').getElements('select'); // get selects first
            this.items.combine(el.getElement('div.ModalPopupContent').getElements('input[type=checkbox]'));
            this.items.combine(el.getElement('div.ModalPopupContent').getElements('input[type=text]'));
            this.reset();
        },
        reset: function() {
            this.items.each(function(item) {
                switch (item.get('tag')) {
                    case "input":
                        switch (item.get('type')) {
                            case "checkbox":
                                if (item.checked) item.checked = false;
                                break;
                            case "text":
                                if (!item.hasClass('Optional')) { item.value = ''; }
                                break;
                        }
                        break;
                    case "select":
                        if (item.selectedIndex > 0) item[0].selected = true;
                        break;
                }
            });
            return this;
        }
    })

};
	
/***************************
    WINDOW DOM READY
****************************/

CorbisUI.QueueManager.addQueue('CheckoutMacros', { canRerun: true, delay: true });

CorbisUI.addQueueItem('CheckoutMacros', 'checkoutTooltips', function() { CorbisUI.Checkout.registerTooltips(true); });
CorbisUI.addQueueItem('CheckoutMacros', 'ccvReattach', function() {
    //console.log('RUNNING MACRO: ccvReattach');
    //CorbisUI.Checkout.registerTooltips(true);
    try {
        //console.log('Trying to reattach tips');
        //console.log(ccVerificationTips);
        ccVerificationTips.attach('.iconhelp');
        //ccVerificationTips
        //CorbisUI.Checkout.registerTooltips(true);
    } catch (Error) {
        //console.log('there was error');
    }
});
CorbisUI.addQueueItem('CheckoutMacros', 'updateCreditDisplay', function() {
    //console.log('RUNNING MACRO: updateCreditDisplay');
    //console.log(window.location);
    $(CorbisUI.Checkout.PaymentOptions.updateCreditDisplay).click();
});
CorbisUI.addQueueItem('CheckoutMacros', 'closePaymentModal', function() {
    //console.log('RUNNING MACRO: closePaymentModal');
    MochaUI.CloseModal('editPaymentInfoModalPopup');
});
CorbisUI.addQueueItem('CheckoutMacros', 'checkForCCErrors', function() {
    //console.log('RUNNING MACRO: checkForCCErrors');
    var paymentBlock = $('paymentChosenBlock').getElement('div[id$=creditDisplay]');
    if (paymentBlock.getStyle('display') != 'none') {
        var nextBTN = $('paymentPane').getElement('.buttonBar').getElement('div[id$=btnPaymentNext]');
        if (!paymentBlock.getElement('.ErrorRow')) {

            setGlassButtonDisabled(nextBTN, false);
        } else {
            setGlassButtonDisabled(nextBTN, true);
        }
    }
});
CorbisUI.addQueueItem('CheckoutMacros', 'checkForCorporateErrors', function() {
    //console.log('RUNNING MACRO: checkForCorporateErrors');
    var paymentBlock = $('paymentChosenBlock').getElement('div[id$=corporateDisplay]');
    if (paymentBlock.getStyle('display') != 'none') {
        var nextBTN = $('paymentPane').getElement('.buttonBar').getElement('div[id$=btnPaymentNext]');
        if (!paymentBlock.getElement('.ErrorRow')) {
            setGlassButtonDisabled(nextBTN, false);
        } else {
            setGlassButtonDisabled(nextBTN, true);
        }
    }
});

//var keyWatch;

//CorbisUI.addQueueItem('domReady', 'keyWatchSetup', function() {

//    keyWatch = new CorbisUI.Events.KeyWatch(['space', 'esc'], {toDo: "stop"});

//});

CorbisUI.addQueueItem('domReady', 'checkoutTabs', function() {

    CorbisUI.QueueManager.CheckoutMacros.runItem('checkoutTooltips');

    

    /*
    check if there is an invalid payment method
    */
    if (CorbisUI.GlobalVars.MainCheckout.invalidPayment) {
        /**/
        // make invalid payment pane show
        $('invalidPaymentPane').setStyle('display', 'block');

        // need to have another function to disable all tabs
        /**/
    } else {

        var mainTabs = $$('#topNavDiv a');

        mainTabs.each(function(el, index) {
            if (el.hasClass('tab_ON')) el.removeClass('tab_ON');
            el.addEvents({
                mouseover: CorbisUI.Checkout.checkOutTabsHover.bindWithEvent(el, index),
                mouseout: CorbisUI.Checkout.checkOutTabsHover.bindWithEvent(el, index)
            });
        });

        // valid payment method, so initiate tabs
        checkoutTabs = new CorbisUI.Checkout.Tabs(
                            $$('#tabContainers div.container'),
                            mainTabs,
                            {
                                selectedTabCssClass: "selectedTab",
                                selectedSectionCSSClass: "selectedSection",
                                firstShow: checkoutTabsFirstShow
                            }
                        );

        nonRfcdTabs = new CorbisUI.Checkout.Tabs(
                            null,
                            $$('#' + CorbisUI.GlobalVars.MainCheckout.Tabs.nonRfcdTabs + ' a'),
                            {
                                selectedTabCssClass: "selected",
                                firstShow: 0,
                                trueTabs: false,
                                dataBackupControl: $(document.body).getElement('input[id$=optionsBlockSelected]')
                            }
                       );
        rfcdTabs = new CorbisUI.Checkout.Tabs(
                            null,
                            $$('#' + CorbisUI.GlobalVars.MainCheckout.Tabs.rfcdTabs + ' a'),
                            {
                                selectedTabCssClass: "selected",
                                firstShow: 0,
                                trueTabs: false,
                                dataBackupControl: $(document.body).getElement('input[id$=optionsBlockSelectedRFCD]')
                            }
                       );
        CorbisUI.Checkout.setPaymentTabs();
    }


    //console.log(screen.width);

});


/***************************
    SERVICE FUNCTIONS?
****************************/

function PaymentPane(paymentType) {
    if(paymentType=='CreditCard')
    {       
        if ($(CorbisUI.Checkout.PaymentOptions.CreditPane))
            $(CorbisUI.Checkout.PaymentOptions.CreditPane).setStyle('display','block');
        if ($(CorbisUI.Checkout.PaymentOptions.CorporatePane))
            $(CorbisUI.Checkout.PaymentOptions.CorporatePane).setStyle('display', 'none');
        //hack for 17035, ie7
        $(CorbisUI.Checkout.PaymentOptions.CreditPane).getElement('input[id$=ccVerificationCodeTextBox]').focus();
    }
    else
    {
        if ($(CorbisUI.Checkout.PaymentOptions.CorporatePane))
            $(CorbisUI.Checkout.PaymentOptions.CorporatePane).setStyle('display', 'block');
        if ($(CorbisUI.Checkout.PaymentOptions.CreditPane))
            $(CorbisUI.Checkout.PaymentOptions.CreditPane).setStyle('display', 'none');
        setPaymentMethod('CorporateAccount')
        CorbisUI.QueueManager.CheckoutMacros.runItem('checkForCorporateErrors');      
    }    
}
function setPaymentMethod(method)
{
    $(CorbisUI.Checkout.PaymentOptions.selectedPayment).value=method;
}
function focusOnCCV() {
    if ($('paymentChosen_Indicator') != null && $('paymentChosen_Indicator').getElement('input[id$=ccVerificationCodeTextBox]') != null) {
        $('paymentChosen_Indicator').getElement('input[id$=ccVerificationCodeTextBox]').focus();
    }
}

function focusOnCCV() {
    if ($('paymentChosen_Indicator') != null && $('paymentChosen_Indicator').getElement('input[id$=ccVerificationCodeTextBox]') != null) {
        $('paymentChosen_Indicator').getElement('input[id$=ccVerificationCodeTextBox]').focus();
    }
}

function setCreditCard(ccguid) {
    $('paymentChosen_Indicator').getElement('input[id$=ccVerificationCodeTextBox]').value = '';
    if (ccguid != null) {
        setPaymentMethod('CreditCard');
        $(CorbisUI.Checkout.PaymentOptions.selectedCreditUid).value = ccguid;
        //$(CorbisUI.Checkout.PaymentOptions.updateCreditDisplay).click();

        CorbisUI.QueueManager.CheckoutMacros.runSequence('updateCreditDisplay', 'closePaymentModal', 'ccvReattach', 'checkForCCErrors');
        
    } else {
        $(CorbisUI.Checkout.PaymentOptions.selectedCreditUid).value == null;
        $(CorbisUI.Checkout.PaymentOptions.weDontUnderstandError).setStyle('display', 'block');
        $(CorbisUI.Checkout.PaymentOptions.CreditPane).setStyle('display', 'none');
        
        CorbisUI.QueueManager.CheckoutMacros.runItem('closePaymentModal');
    }
}
function DeliveryPane(deliveryType)
{
    if (deliveryType=='FTP')
    {       
        setDeliveryMethod('FTP');     
    }
    else
    {
        setDeliveryMethod('Download');     
    }    
}

function viewRestrictions(element, corbisId) {
    if (corbisId == null) {
        //get productuid
        var row = $(element).getParent().getParent().getParent();
        corbisId = row.getElement('.checkoutItemNumber').innerHTML.trim();
    }    
    $('loadingViewRestrictions').setStyle('display', 'block');
    //make ajax call
    Corbis.Web.UI.Checkout.CheckoutService.GetViewRestrictions(corbisId, GetViewRestrictionsCompleted, FailedCallback, element, null);
    
}

function GetViewRestrictionsCompleted(results, context, methodName)
{
    $('loadingViewRestrictions').setStyle('display', 'none');
    //show popup here
    //var url = '<img src="../Images/alertYellow.gif"  />';
    var link = $(context);
    var row = link.getParent().getParent().getParent();
    
    var photo = row.getElement('img');
    var margin = photo.getStyle('margin-top').toInt();
    margin = margin * 1.8;
    var widthheight;
    if (margin == 0)
        widthheight = "height:90px;"
    else
        widthheight = "width:90px;";
    var url = '<div class="thumbWrap"><img src="' + photo.getProperty('src') + '" style="margin-top:' + margin + 'px;' + widthheight + '" /></div>';
    var restrictions = '';
    results.Restrications.each(function(item) { restrictions =  restrictions + '<p>' +  item.Notice + '</p>'; });
    new CorbisUI.Popup('viewRestrictionsTemplate', { 
        createFromHTML: true,
        showModalBackground: false,
        closeOnLoseFocus: true,
        centerOverElement: $(context),
        width: 550,
        positionVert: 'bottom', 
        positionHoriz: 35,
        replaceText: [ url, results.ModelReleaseStatus, results.PropertyReleaseStatusText,restrictions]
    });
}

function ViewOrderRestrictions(context) {
    new CorbisUI.Popup('ViewOrderRestrictions', {
        createFromHTML: false,
        showModalBackground: false,
        closeOnLoseFocus: true,
        centerOverElement: $(context),
        height:465,
        positionVert: 'top',
        positionHoriz: 'right'
        
    });
}

function deleteItemSummaryItem(element){
    //get productuid
    var row = $(element).getParent().getParent().getParent();
    var corbisId = row.getElement('.checkoutItemNumber').innerHTML.trim();
    
    //$('loadingViewRestrictions').setStyle('display', 'block');
    //make ajax call
    GetDeleteItemInformationCompleted(element);
    
}
function GetDeleteItemInformationCompleted(context){
    //There is no back=end function to support piping and image into this modal yet
    //
    /*
    var link = $(context);
    var row = link.getParent().getParent().getParent();
    
    var photo = row.getElement('img');
    var margin = photo.getStyle('margin-top').toInt();
    margin = margin * 1.8;
    var widthheight;
    if (margin == 0)
        widthheight = "height:90px;"
    else
        widthheight = "width:90px;";
    var url = '<div class="thumbWrap"><img src="' + photo.getProperty('src') + '" style="margin-top:' + margin + 'px;' + widthheight + '" /></div>';
    var restrictions = '';
    results.Restrications.each(function(item) { restrictions =  restrictions + '<p>' +  item.Notice + '</p>'; });
   */   
    new CorbisUI.Popup('deleteItemLayer', { 
        createFromHTML: true,
        showModalBackground: false,
        closeOnLoseFocus: true,
        centerOverElement: $(context), 
        width: 360,
        positionVert: 'middle', 
        positionHoriz: 'right'
    });

}
function FailedCallback(error)
{
//    // Display the error.    
//    var RsltElem = document.getElementById("Results");
//    RsltElem.innerHTML = "Service Error: " + error.get_message();
}

function Addresses_Validate(addrCount) 
{
    if (addrCount > 0)
        $('deliveryPhysical_Error').addClass('displayNone');
    else
        $('deliveryPhysical_Error').removeClass('displayNone');
}



/********************************
  GENERIC UPDATE PANEL REFRESH
*********************************/

function UpdatePanelRefresh(params) {
    //console.log('======>>>>>>CALLING: UpdatePanelRefresh');

    try {

        //console.log('---->TRYING: UpdatePanelRefresh');
        //if (params == "paymentTab") {
        //CorbisUI.QueueManager.CheckoutMacros.runItem('ccvReattach');

        try {
            if (ccVerificationTips)
                ccVerificationTips.attach('.iconhelp');
        } catch (Error) {
        }
        
        //        checkoutTabs = new CorbisUI.Checkout.Tabs(
        //                            $$('#tabContainers div.container'),
        //                            $$('#topNavDiv a'),
        //                            {
        //                                selectedTabCssClass: "selectedTab",
        //                                selectedSectionCSSClass: "selectedSection",
        //                                firstShow: 3
        //                            }
        //                        );
        //}

        if (params.tabIndex) {

            var mainTabs = $$('#topNavDiv a');

            mainTabs.each(function(el, index) {
                if (el.hasClass('tab_ON')) el.removeClass('tab_ON');
                el.addEvents({
                    mouseover: CorbisUI.Checkout.checkOutTabsHover.bindWithEvent(el, index),
                    mouseout: CorbisUI.Checkout.checkOutTabsHover.bindWithEvent(el, index)
                });
            });
            checkoutTabs = new CorbisUI.Checkout.Tabs(
                                $$('#tabContainers div.container'),
                                mainTabs,
                                {
                                    selectedTabCssClass: "selectedTab",
                                    selectedSectionCSSClass: "selectedSection",
                                    firstShow: params.tabIndex
                                }
                            );
        }
    } catch (Error) { }
}

function UpdateBasedOnCurrentTabIndex(params) {
    var agessa = $('checkoutStage').getElement('div[class=agessa]');
    if (params == null || params.tabIndex == null || params.tabIndex != 3) {
        if (agessa != null)
            agessa.addClass('hdn');
        setCheckedState($('reviewPane').getElement('div[id$=restrictionsCheckbox]').id, false);
        setCheckedState($('reviewPane').getElement('div[id$=termsCheckbox]').id, false);
    }
    enablePaymentNext();
    if (params.tabIndex == 3 && $('editMyCartLink')) {
        $('editMyCartLink').setStyle('display', 'none');
    } else {
        if ($('editMyCartLink')) {
            $('editMyCartLink').setStyle('display', 'inline');
        }
    }
}
function enablePaymentNext() {
    var nextBTN = $('paymentPane').getElement('.buttonBar').getElement('div[id$=btnPaymentNext]');
//    if (isEnable == null) {
//        isEnable = true;
//    }
    //console.log('enable button, isEnabled=' + isEnable);
    setGlassButtonDisabled(nextBTN.id + '_glassBtnId', false);
}
