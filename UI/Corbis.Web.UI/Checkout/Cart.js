/*

                          `?XXX.  `T{/:.   %X/!!x "?x.
                             "4{7@( '!+!!X(:.`4!!X!x.?h7h
                         `!(:. ~!!!f(~!!!+!!{{.'~+h!tX!!?hh:.
                    '`X!.  !(d!X!!H!?{{``"!:?{{!{X*!?tX!!H*))h.
                  ...  '!X(!X!{{?@f!!!{!{x.!!%!!!%!!!)@Thh!!X)!).
                   ^!!!{:!(((!!: ~((({!!!h+!{{!X!+%?+{!!?!+)!+X(!+
               -    `\tXX{(~!!!!!:.!.%%(!!!!!!!!!X!))!!!!X%``%!!!(>
               ^X>:x. {!!!!X: ~!!*!{!!!{!~!X!)%!{!!!)?@!!!?!)?!!!>~
                 `X(!!:!!!{{(!!.)!%(:\!!:%~!~\!t!! `H!)~~!!!!!!(?@
                  `!X: `)!!!C44XX!!!.%%.X:>-> %!!X! /!~!.'!> !S!!!
              +{..  \X%\.'{??X!!!t!!~!!{!~!~'.!~~~ -~` {> !~ /!X`
                `X!XXM!!4!%\(4!!!!%(`,zccccd$$$$$$$$$ccx ` .~
                  "XLS@!)!!%L44X!!! d$$$$$$$$$$$$$$$$$$$,  '^
                   `!X?%:!!??X!4?*';$$$$$$$$$$$$$$$$$$$$$
                  `iXM:!!?Xt!XH!!! 9$$$$$$$$$$$$$$$$$$$$$
                   `X3tiXS#?WH!X!! $$$$$$$$$$$$$$$$$$$$$$
                   .MX?*StXX?X!!W? $$$$$$$>?$$$$$$$$$$$$
                    8??M%T%' r `  ;$$$$$$$$$,?$$$$$$$$$F
                    'StMX!': J$$d$$$$$$$$$$$$h ?$$$$$$"
                     tM9MH d$$$$$$$$$$$$$$C???r{$$$F,r
                     4M?t':$$$$$$$$$$$$$$$$h. $$$,cP"   JAVASCRIPT !
                     'M>.d$$$$$$$$$$$$$$$$$>d$$${.,     JAVASCRIPT !!
                      ,d$$$$$$$$$$$$$$$$$'cd$$$$r"      JAVASCRIPT !!!!
                      `$$$$$$$$$$$$$$$??$Jcii`?$h       HARDCORE !!!!!
                       $$$$$$$$$$$$$F,;;, "?h,`$$h
                      j$$$$$$$$$$$$$.CC>>>>c`"  `"        ..,g q,
                   .'!$$$$$$$$$$$$$' `''''''            aq`?g`$.Bk
               ,- '  "?$$$$$$$$$$$$$$d$$$$$c, .         .)od$$$$$$
          , -'           `""'   `"?$$$$$$$$$??=      .d$$$$$$$$F'
        ,'                           `??$$P       .ed$$P""   `
       ,                                `.      z$$$"
       `:dbe,                          x,/    e$$F'
       :$$$$P'`>                       $F  z$$$"
      d$$$P"'  >                       $Fe$$$"
    .$$$?F     ;                       $$$$"
    $$$$$$eeu. >                       >P"
     `""???$$$$$eu,._''uWb,            )
               `""??$P$$$$$$b.         :
                >     ?$$$"'           {
                F      `"              `:
                >                       `>
                >                        ?
               J                          :
               X                ..  .     ?
               "{ 4{!~;/'!>{`~{>~.>! ~! '"
                '>!>=.%=.;~~>~4~`{'>>>~!
                 4'!/>!\\!{~~:/{;!{;`;/=':
                 `=;!~:`~!>{.-; "(>=.':!;'
                  :;=.~{`;`~>!~> ?!/>>~!!{'
                  ~:~'!!;`;`~:>); ;(.uJL!~~
                    >L.(.:,L;L:-+d$$$$$$
                    :4$$$$$$$L   ?$$#$$$>
                     '$$$B$$$>    $$$MB$&
                      $$$$$$$      $$$@$F
                      `$$$$$$>     R$$$$
                       $$$$$$     {$$@$P
                       $R$$$R     `!)=!>
                       $$$6T       $$$$'
                       $$R$B      ;$$$F,._
                       !=!(!    .'        ``= .
                       $$$$F    (.             '\
                     ,{$$$$(      ``~'`` --:.._.,)
                    ;   ``  `-.
                    (          "\.
                     ` -{._       ".
                           `~:,._ .:


*/

/******************************
CORBIS CART 
DRAG AND DROP SUPASTAR
CHRIS
*******************************/

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Cart = {};

CorbisUI.QueueManager.addQueue('cartMacros', {
    canRerun: true
});
/***************************
    STATICS
****************************/

CorbisUI.Cart.DragZones = {
    // defined ondomready
    Checkout: null,
    Priced: null,
    UnPriced: null,
    PriceMultiple: null
};

CorbisUI.Cart.DropZones = {
    // defined ondomready
    // see function at bottom
    Checkout: null,
    Priced: null,
    UnPriced: null,
    PriceMultiple: null
};
CorbisUI.Cart.LastAction = {
    // drapdrop helper class
    DragZone: null,
    DropZone: null,
    CorbisId:null
};

CorbisUI.Cart.Carousel = {
    Next: null,
    Previous: null
}


/***************************
    MODELS
****************************/

// model for thumb objects
CorbisUI.Cart.ThumbItem = new Class({

    Implements: [Events],

    ghosted: false,
    currentZone: null,

    status: null,
    price: null,
    license: null,
    nonLocalizedLicense: null,
    id: null,
    productUID: null,
    priceByAE: false,
    perContract: false,
    isOutline: false,
    imageAvailable: true,
    isRfcd: false,
    effectivePriceStatus: null,

    // references
    el: null,       // actual DOM item 
    clone: null,    // clone JS object
    handle: null,   // drag handle 
    pricingThumb: null, // pricing element JS object
    validator: null, // validator JS object

    // drag reference
    drag: null,
    scroller: null,

    initialize: function(ele) {

        // store some useful information
        this.el = ele;
        this.currentZone = CorbisUI.Cart.DetermineItemZone(ele);

        this.id = ele.getElement('span.corbisID').get('text').clean();
        this.license = ele.getElement('div.license span').get('text').clean();
        this.nonLocalizedLicense = this.el.getProperty('nonLocalizedLicenseModel');
        switch (this.currentZone) {
            case "Priced":
            case "Checkout":
                this.status = "priced";
                this.price = this.getPrice(this.el.getElement('div.action span.actLikeLink').get('text'));
                this.productUID = ele.get('id').replace('cartBlock_', '');
                break;
            default:
                this.status = "unpriced";
                this.productUID = ele.get('id').replace('cartBlock_', '');
                break;
        }

        var pinkyCheck = $('pinky_' + this.productUID);
        if (pinkyCheck) this.createPricingThumb(pinkyCheck);

        this.effectivePriceStatus = this.el.getProperty('effectivepricestatus');

        if (this.el.getProperty('isimageavailable') == 'False') this.imageAvailable = false;
        //console.log(this.effectivePriceStatus.contains('PricedByAE'));
        if (this.effectivePriceStatus.contains('PricedByAE')) this.pricedByAE = true;
        if (this.effectivePriceStatus.contains('AsPerContract')) this.perContract = true;
        this.isOutline = (this.el.getProperty('isoutline') == 'True');
        this.isRfcd = (this.el.getProperty('isrfcd') == 'True');
        // drag handle - but is actually image thumb
        // so can be used elsewhere if needed
        this.handle = ele.getElement('div.handle img');
        var imgNotAvailable = ele.getElement('div .imageNotAvailable');
        if (imgNotAvailable != null)
            this.handle = imgNotAvailable;

        // hack for IE7 to fix drag problem
        if (Browser.Engine.trident5) this.handle.ondragstart = function() { return false; };

        // store this JS object on the actual DOM object
        // so it can be accessed via other functions
        this.el.store('objectReference', this);

        // setup the drag functionality
        if (this.pricingThumb == null || this.imageAvailable) {
            this.draggableSetup();
        } else {
            this.handle.getParent().getParent().setStyle('cursor', 'default');
        }

        // test event
        ele.getElement('div.license').addEvent('click', this.test.bindWithEvent(this));

    },
    getPrice: function(displayText) {
        //remove currency code first
        return Number.parseLocale(displayText.replace(/[A-Z]/g, ''));
    },
    test: function(event) {
        //console.log('+=== ITEM INFORMATION ===================================================+');
        //console.log('     ID: ' + this.id);
        //console.log('     license: ' + this.license);
        //console.log('     nonLocalizedLicense: ' + this.nonLocalizedLicense);
        //console.log('     price: ' + this.price);
        //console.log('     zone: ' + this.currentZone);
        //console.log('     productUID: ' + this.productUID);
        //console.log('     pricedByAE: ' + this.pricedByAE);
        //console.log('     perContract: ' + this.perContract);
        //console.log('     isOutline: ' + this.isOutline);
        //console.log('     effectivePriceStatus: ' + this.effectivePriceStatus);
        //console.log('     imageAvailable: ' + this.imageAvailable);
    },

    draggableSetup: function() {
        this.scroller = new Scroller(document.body, {
            area: 100,
            velocity: 0.25,
            onChange: function(x, y) {
                this.element.scrollTo(0, y);
            }
        });
        this.handle.addEvent('mousedown', this.dragEvent.bindWithEvent(this, this.el));
    },

    disableDrag: function() {
        if (this.drag) this.drag.detach();
        if (this.scroller) this.scroller.stop();
    },

    enableDrag: function() {
        if (this.drag) this.drag.attach();
        if (this.scroller) this.scroller.start();
    },

    dragEvent: function(event, element) {

        if (this.drag == null) {

            event.stop();

            // get clone first    
            this.clone = new CorbisUI.Cart.CloneItem(this);
            this.clone.el.inject(document.body);

            // mark original selected 
            this.select();

            // setup droppables
            var droppables = [
                CorbisUI.Cart.DragZones.Checkout,
                CorbisUI.Cart.DragZones.Priced,
                CorbisUI.Cart.DragZones.UnPriced,
                CorbisUI.Cart.DragZones.PriceMultiple
            ];

            //setup drag object
            ///////////////////////////////////////
            //  THIS IS CONDITIONAL HELL         //
            ///////////////////////////////////////
            this.drag = new Drag.Move(this.clone.el, {

                droppables: droppables,

                handle: this.handle,

                onDrop: function(element, droppable) {

                    var dropZone = CorbisUI.Cart.DetermineZone(droppable);

                    var objRef = element.retrieve('cloneCaller');
                    objRef.rel.disableDrag()
                    element.destroy();

                    if (objRef.rel.currentZone == dropZone) {
                        objRef.rel.reset();
                        return;
                    }

                    CorbisUI.Cart.LastActionUpdate(objRef.rel.currentZone, dropZone, objRef.rel.id);

                    switch (dropZone) {
                        case 'Checkout':
                            // why you dragging an unpriced to checkout??
                            if (objRef.rel.currentZone == 'UnPriced') {
                                if (!objRef.rel.isOutline) {
                                    new CorbisUI.Popup('unpricedToInvalidTargetModal', {
                                        createFromHTML: true,
                                        replaceText: [objRef.rel.id, objRef.rel.nonLocalizedLicense]
                                    });
                                }
                                objRef.rel.reset();
                                // check if draggable outline 
                            } else if (objRef.rel.isOutline && objRef.rel.pricedByAE) {
                                objRef.rel.validate();
                                // check price status
                            } else {
                                if (objRef.rel.isOutline || objRef.rel.effectivePriceStatus.contains('ContactUs')
                                || objRef.rel.effectivePriceStatus.contains('UpdateUse')
                                || objRef.rel.effectivePriceStatus.contains('CountryOrCurrencyError')
                                ) {
                                    objRef.rel.reset();
                                    break;
                                }
                                switch (objRef.rel.effectivePriceStatus) {
                                    //                                    case "ContactOutline":                             
                                    //                                        objRef.rel.reset();                      
                                    //                                        break;                     
                                    case "UpdateUse":
                                        objRef.rel.reset();
                                        break;
                                    case "ContactUs":
                                        objRef.rel.reset();
                                        // need some alert for contactUs
                                        new CorbisUI.Popup('licenseAlertModal', {
                                            createFromHTML: true,
                                            replaceText: [objRef.rel.id, objRef.rel.nonLocalizedLicense, objRef.rel.productUID]
                                        });
                                        break;
                                    default:
                                        objRef.rel.validate();
                                        break;
                                }
                            }
                            break;
                        case 'Priced':
                            if (objRef.rel.currentZone == 'Checkout') {
                                if (objRef.rel.perContract == true) {
                                    removeFromCheckoutTotals(1, DisplayTextPerContract);
                                }
                                else {
                                    removeFromCheckoutTotals(1, objRef.rel.price);
                                }

                                objRef.rel.moveToPriced();

                            }
                            if (objRef.rel.currentZone == 'UnPriced') {
                                if (!objRef.rel.isOutline) {
                                    new CorbisUI.Popup('unpricedToInvalidTargetModal', {
                                        createFromHTML: true,
                                        replaceText: [objRef.rel.id, objRef.rel.nonLocalizedLicense]
                                    });
                                }
                                objRef.rel.reset();
                            }
                            objRef.rel.reset();
                            break;
                        case 'PriceMultiple':
                            if (objRef.rel.currentZone == 'Checkout' ||
                               objRef.rel.effectivePriceStatus.contains('PricedByAE') ||
                               objRef.rel.isRfcd ||
                               (objRef.rel.currentZone == 'UnPriced' && objRef.rel.isOutline) ||
                               (objRef.rel.currentZone == 'Priced' && objRef.rel.isOutline)) {

                                objRef.rel.reset();
                                // else, its good!! VALIDATE IT NOW 
                            } else {
                                objRef.rel.createPricingThumb();
                            }
                            break;

                        case 'UnPriced':
                        default:
                            objRef.rel.reset();
                            break;
                    }

                },

                onEnter: function(element, droppable) {
                    var objRef = element.retrieve('cloneCaller');
                    var hoverZone = CorbisUI.Cart.DetermineZone(droppable);

                    switch (objRef.rel.currentZone) {
                        case 'Checkout':
                            switch (hoverZone) {
                                case 'Priced':
                                    objRef.showIndicator('plus');
                                    break;
                                case 'PriceMultiple':
                                case 'UnPriced':
                                    objRef.showIndicator('minus');
                                    break;
                            }
                            break;
                        case 'Priced':
                            switch (hoverZone) {
                                case 'Checkout':
                                    if (objRef.rel.isOutline && !objRef.rel.pricedByAE) {
                                        objRef.showIndicator('minus');
                                    }
                                    else if (objRef.rel.effectivePriceStatus == "UpdateUse"
                                        || objRef.rel.effectivePriceStatus.contains('ContactUs')
                                        || objRef.rel.effectivePriceStatus.contains('UpdateUse')
                                        || objRef.rel.effectivePriceStatus.contains('CountryOrCurrencyError')
                                        || objRef.rel.imageAvailable == false) {
                                        objRef.showIndicator('minus');
                                    }
                                    else {
                                        objRef.showIndicator('plus');
                                    }
                                    break;
                                case 'PriceMultiple':
                                    if (objRef.rel.isRfcd) {
                                        objRef.showIndicator('minus');
                                        break;
                                    }
                                    switch (objRef.rel.effectivePriceStatus) {
                                        case "PricedByAE":
                                            objRef.showIndicator('minus');
                                            break;
                                        case "ContactOutline":
                                            objRef.showIndicator('minus');
                                            break;
                                        case "ContactUs":
                                            objRef.showIndicator('plus');
                                            break;
                                        default:
                                            objRef.showIndicator('plus');
                                            break;
                                    }
                                    break;
                                case 'UnPriced':
                                    objRef.showIndicator('minus');
                                    break;
                            }
                            break;
                        case 'UnPriced':
                            switch (hoverZone) {
                                case 'Checkout':
                                case 'Priced':
                                    objRef.showIndicator('minus');
                                    break;
                                case 'PriceMultiple':
                                    if (objRef.rel.pricedByAE || objRef.rel.isRfcd || objRef.rel.isOutline) {
                                        objRef.showIndicator('minus');
                                    } else {
                                        objRef.showIndicator('plus');
                                    }
                                    break;
                            }
                            break;
                        case 'PriceMultiple':
                            objRef.showIndicator('plus');
                            break;
                    }
                },

                onLeave: function(element, droppable) {
                    var objRef = element.retrieve('cloneCaller');

                    var hoverZone = CorbisUI.Cart.DetermineZone(droppable);
                    switch (objRef.rel.effectivePriceStatus) {
                        case "ContactOutline":
                        case "ContactUs":
                        case "UpdateUse":
                            objRef.showIndicator('minus');
                            return;
                    }

                    switch (objRef.rel.currentZone) {
                        case 'Checkout':
                            switch (hoverZone) {
                                case 'Priced':
                                    objRef.hideIndicator('plus');
                                    break;
                                case 'PriceMultiple':
                                case 'UnPriced':
                                    objRef.hideIndicator('minus');
                                    break;
                            }
                            break;
                        case 'Priced':
                            switch (hoverZone) {
                                case 'Checkout':
                                    if (objRef.rel.isOutline && !objRef.rel.priceByAE) {
                                        objRef.hideIndicator('minus');
                                    } else {
                                        objRef.hideIndicator('plus');
                                    }
                                    break;
                                case 'PriceMultiple':
                                    switch (objRef.rel.effectivePriceStatus) {
                                        case "PricedByAE":
                                            objRef.hideIndicator('minus');
                                            break;
                                        case "ContactOutline":
                                            objRef.hideIndicator('minus');
                                            break;
                                        case "ContactUs":
                                            objRef.hideIndicator('plus');
                                            break;
                                        default:
                                            objRef.hideIndicator('plus');
                                            break;
                                    }
                                    break;
                                case 'UnPriced':
                                    objRef.hideIndicator('minus');
                                    break;
                            }
                            break;
                        case 'UnPriced':
                            switch (hoverZone) {
                                case 'Checkout':
                                case 'Priced':
                                    objRef.hideIndicator('minus');
                                    break;
                                case 'PriceMultiple':
                                    if (objRef.rel.pricedByAE || objRef.rel.isRfcd || objRef.rel.isOutline) {
                                        objRef.hideIndicator('minus');
                                    } else {
                                        objRef.hideIndicator('plus');
                                    }
                                    break;
                            }
                            break;
                        case 'PriceMultiple':
                            objRef.hideIndicator('plus');
                            break;
                    }
                },

                onCancel: function(element, droppable) {
                    var objRef = element.retrieve('cloneCaller');
                    objRef.rel.destroyClone();
                    objRef.rel.reset();
                },

                onStart: function(element) {
                    var objRef = element.retrieve('cloneCaller');
                    objRef.rel.deselect();
                    objRef.rel.ghost();
                    objRef.rel.scroller.start();
                },
                onComplete: function(element) {
                    var objRef = element.retrieve('cloneCaller');
                    objRef.rel.scroller.stop();
                }


            });
            this.drag.start(event);
        }
    },

    destroyClone: function() {
        this.clone.el.destroy();
        this.clone = null;
    },

    createPricingThumb: function(thumb) {
        this.disableDrag();
        if (thumb) {
            this.ghost();
            this.pricingThumb = new CorbisUI.Cart.PricingItem(this, thumb);
        } else {
            this.pricingThumb = new CorbisUI.Cart.PricingItem(this);
        }
        setPricingControls();
    },

    destroyPricingThumb: function() {
        this.draggableSetup();
        this.pricingThumb.el.destroy();
        this.pricingThumb = null;
        this.reset();
        setPricingControls();
    },
    destroyMe: function() {
        this.el.destroy();
        if (this.pricingThumb) this.destroyPricingThumb();
        CorbisUI.Cart.DeleteInstance(this);
    },

    ghost: function() {
        this.ghosted = true;
        if (!this.el.hasClass('cartBlock_ghost')) this.el.addClass('cartBlock_ghost');
    },

    revive: function() {
        this.ghosted = false;
        if (this.el.hasClass('cartBlock_ghost')) this.el.removeClass('cartBlock_ghost');
    },

    select: function() { this.el.addClass('cartBlock_select'); },

    deselect: function() { this.el.removeClass('cartBlock_select'); },

    reset: function() {
        this.disableDrag();
        this.drag = null;
        this.draggableSetup();
        this.revive();
        this.deselect();
    },

    validate: function() {
        this.validator = new CorbisUI.Cart.ItemValidator(this);
    },

    moveToPriced: function(fromBatch) {

        if (this.currentZone == 'UnPriced') return;
        if (this.currentZone == 'Checkout') {
            var tmp = this.el.getParent();
            // remove from session
            if (!fromBatch) this.removeFromCheckoutSession();

        }
        //console.log('OBJREF--->moveToPriced');
        this.el.inject(CorbisUI.Cart.DropZones.Priced, 'top');
        if (tmp) tmp.destroy();
        this.currentZone = 'Priced';
        this.status = "priced";


        //var priceLink = this.el.getElement('div.infoBox').getElement('div.action span.actLikeLink');
        CorbisUI.Cart.UpdatePriceLink(this, 'priceTag');

        // reset checkout controls
        setScrollControls();
        CorbisUI.Cart.DisplayPricedInstructions();
        CorbisUI.Cart.DisplayUnpricedInstructions();
        if (!fromBatch) CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
        adjustColumnHeight();
    },

    moveToCheckout: function(fromBatch) {
        LogOmnitureEvent("event20");  
        
        var tmp = new Element('li');
        this.el.inject(tmp);
        tmp.inject(CorbisUI.Cart.DropZones.Checkout, 'top');
        if (this.validator) this.validator = null;
        if (this.productUID && this.currentZone == 'UnPriced') {

            CorbisUI.Cart.UpdatePriceLink(this, 'priceTag');

            //            var linkEl = this.el.getElement('div.action span.actLikeLink');
            //            var linkOnclick = linkEl.getProperty('onclick').toString();
            //            linkEl.removeProperty('onclick');
            //            
            //            linkEl.onclick = linkOnclick.replace(oldGuidLink, newGuidLink);
            //
        }
        this.currentZone = 'Checkout';
        this.reset();
        // reset checkout controls
        setScrollControls();
        CorbisUI.Cart.DisplayPricedInstructions();
        if (!fromBatch) CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
    },


    buildAjaxParameters: function() {
        var tmp = {};
        tmp.guid = this.productUID;
        tmp.corbisId = this.id;
        return tmp;
    },

    finishValidation: function() {
        this.validator = null;
        this.revive();
        this.reset();
    },

    // remove from checkout session
    // NOTE: add to checkout session
    // should be handled by validation
    // service

    removeFromCheckoutSession: function() {
        var list = new Array();
        list[0] = this.productUID;

        //Corbis.Web.UI.Checkout.CartScriptService.ValidateItems(list, this.removeFromCheckoutSessionCallback.bind(this));
        Corbis.Web.UI.Checkout.CartScriptService.MoveItemWithInCart(list, 'Checkout', this.removeFromCheckoutSessionCallback.bind(this));
    },

    removeFromCheckoutSessionCallback: function(results, context, methodName) {
        //console.log(results);
        afterDeleteAction();
    },

    updatePrice: function(price, currency) {
        this.price = price;
        this.effectivePriceStatus = 'Ok';
        //console.log(this.el.getElement('div.action span.actLikeLink'));
        //this.el.getElement('div.action span.actLikeLink').set('text', price + ' ' + currency);
        // there maybe more things we have to update, but this is the basic
        Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';
        this.el.getElement('div.action span.actLikeLink').set('text', price.toFloat().localeFormat('C') + ' ' + currency);
    }

});

// model for clone objects
CorbisUI.Cart.CloneItem = new Class({
    
    Implements: [Options, Events],
    
    options: {},
    
    // references
    el: null, // refers to clone DOM object
    rel: null,  // refers to main JS object
    plus: null,
    minus: null, 
   
    
    initialize: function(ele,options){
        if(options) this.setOptions(options);
        this.rel = ele;
        
        // create some coordinates object based on 
        // the original item and merge it with some
        // styles just for the clone
        var coords = $merge(this.rel.el.getCoordinates(), {'position':'absolute','cursor':'move','z-index':100,'overflow':'visible'});
        
        // create clone container
        this.el = new Element('div',{id: 'clone_'+this.rel.id}).setStyles(coords);    
        
        // THE CLONE!!!!
        this.rel.el.clone().setStyle('opacity',0.5).inject(this.el);
        
        this.el.getElement('.infoIcon').setStyle('display','none');
        this.el.getElement('.closeIcon').setStyle('display','none');
        
         // add the plus/minus indicators
        this.plus = new Element('div').addClass('canIndicator').inject(this.el);
        this.plus.fade('hide');
        
        this.minus = new Element('div').addClass('cantIndicator').inject(this.el);
        this.minus.fade('hide');
       
        // store clone object on clone dom element
        this.el.store('cloneCaller',this);
   
    },
    
    showIndicator: function(type){
        if(Browser.Engine.trident){
            this[type].fade('show');
        }else{
            this[type].fade('in');
        }
    },
    
    hideIndicator: function(type){
        if(Browser.Engine.trident){
            this[type].fade('hide');
        }else{
            this[type].fade('out');
        }
    }
});

// model for pricing doppelganger
CorbisUI.Cart.PricingItem = new Class({

    Implements: [Events],
    
    // references
    el: null, // refers to pricing doppelganger DOM object
    rel: null,  // refers to main JS object corresponding to the original
    handle: null, // refers to image DOM element in pinky for drag handle
    
    drag: null,
    
    initialize: function(ele,thumb){
        // store the reference to the original JS object
        this.rel = ele;
  
        if(thumb){
            this.el = thumb;
        }else{
            this.el = CorbisUI.Cart.CreatePinkyObject(this.rel);
            this.el.inject(CorbisUI.Cart.DropZones.PriceMultiple,'top');
        }
        this.handle = this.el.getElement('img');
        
        // hack for IE7 to fix drag problem
        if(Browser.Engine.trident5) this.handle.ondragstart = function(){ return false; };
        
        if(!thumb) this.addToSession();
        this.dragSetup();
        
        // store this JS object on the actual DOM object
        // so it can be accessed via other functions
        this.el.store('objectReference',this);
        
    },
    
    dragSetup: function(){
        this.handle.addEvent('mousedown',this.dragEvent.bindWithEvent(this,this.el));
    },
    
    dragEvent: function(event, element){
    
        event.stop();
            
            // get clone first    
            this.clone = this.el.clone().setStyles($merge(this.el.getCoordinates(),{
                'position':'absolute',
                'cursor':'move',
                'opacity':0.5
            })).inject(document.body);
            
            // setup droppables
            var droppables = [
                CorbisUI.Cart.DragZones.PriceMultiple
            ];
            
            this.clone.store('cloneCaller',this);
            
            //setup drag object
            this.drag = new Drag.Move(this.clone,{
            
                droppables: droppables,
                
                handle: this.handle,
                
                onDrop: function(element,droppable){
                    
                    // kill the clone
                    element.destroy();
                    
                    // if droppable is null, then its good! nuke it!
                    if(droppable == null){
                        var objRef = element.retrieve('cloneCaller');
                        objRef.removeFromSession();
                        objRef.rel.destroyPricingThumb();  
                    }
                    
                },
                
                onCancel: function(element,droppable){
                    
                    element.destroy();
                }
                
            }); 
            this.drag.start(event);
    
    },
    
    // function to update session
    addToSession: function(){
        var list = new Array();

        list[0] = this.rel.productUID;
        
        Corbis.Web.UI.Checkout.CartScriptService.AddItemsToCartContainer(list, 'PriceMultiple', this.addToSessionCallback.bind(this));
    },
    
    addToSessionCallback: function(results, context, methodName){
        // add something meaningful here?
        //console.log(results);
    },
    
    // remove from session
    removeFromSession: function(){
        var list = new Array();
        list[0] = this.rel.productUID;
        
        Corbis.Web.UI.Checkout.CartScriptService.MoveItemWithInCart(list, 'PriceMultiple', this.removeFromSessionCallback.bind(this));
    },
    
    removeFromSessionCallback: function(results, context, methodName){
        // add something meaningful here?
        //console.log(results);
        afterDeleteAction();
    },
    
    destroyClone: function(){

        this.clone = $lambda(false);
    }

});

// model for validator
CorbisUI.Cart.ItemValidator = new Class({
    Implements: [Events],

    // references
    el: null, //refers to visual validator DOM object
    rel: null, // refers to main JS object of item that is being validated

    data: null, // data returned form validation

    initialize: function(ele) {
        this.rel = ele;

        this.el = new Element('li');

        var coords = this.rel.el.getCoordinates();

        var vWrap = new Element('div')
                    .addClass('validatorBlock')
                    .setStyles({
                        width: coords.width,
                        height: coords.height
                    })
                    .inject(this.el);
        var vIndicator = new Element('div')
                        .addClass(this.rel.nonLocalizedLicense + '_validator')
                        .addClass(this.rel.nonLocalizedLicense + '_background')
                        .set('html', '&nbsp;')
                        .inject(vWrap);



        this.el.inject(CorbisUI.Cart.DropZones.Checkout, 'top');

        CorbisUI.Cart.DisplayCheckoutInstructions();

        this.validate();

    },

    validate: function() {

        var list = new Array();

        list[0] = this.rel.buildAjaxParameters();

        Corbis.Web.UI.Checkout.CartScriptService.ValidateItems(list, this.validateCallback.bind(this));

    },

    validateCallback: function(results, context, methodName) {

        var result = results[0];
        //console.log(result);
        var ids = "";

        // NOTE
        // Status 2, 3, 4 need to be defined more thoroughly        

        switch (result.Status) {
            // SUCCESS          
            case 1:
                this.el.destroy();
                this.rel.moveToCheckout();

                this.rel.price = result.RecalculatedPrice.EffectivePrice;

                if (result.RecalculatedPrice.EffectivePriceStatus != Corbis.LightboxCart.Contracts.V1.PriceStatus.AsPerContract) {
                    this.rel.updatePrice(result.RecalculatedPrice.EffectivePrice, result.RecalculatedPrice.CurrencyCode);
                    addToCheckoutTotals(1, result.RecalculatedPrice.EffectivePrice);
                }
                else {
                    addToCheckoutTotals(1, DisplayTextPerContract);
                }

                break;

            // IMAGE NOT AVAILABLE         
            case 2:
                ids += "CorbisId: " + result.RecalculatedPrice.CorbisId + "\n";
                ids += "Status: " + result.Status + "\n";
                this.el.destroy();
                this.rel.finishValidation();
                break;

            // PRICING CHANGE    
            case 3:
                //console.log('CorbisUI.Cart.ItemValidator : PRICING CHANGE');
                ids += "CorbisId: " + result.RecalculatedPrice.CorbisId + "\n";
                ids += "Status: " + result.Status + "\n";
                ids += "OldPrice: " + result.OldPrice + "\n";
                ids += "NewPrice: " + result.RecalculatedPrice.EffectivePrice + "\n";

                var PCA = $('pricingChangeAlert');

                // create pinky object
                PCA.getElement('td.picture')
                    .empty()
                    .grab(CorbisUI.Cart.CreatePinkyObject(this.rel));

                // format dataToPass in JSON format
                var datapass = '{\'productUID\': \'' + result.ProductUid + '\',\'newPrice\': \'' + result.RecalculatedPrice.EffectivePrice + '\', \'currencyCode\':\'' + result.RecalculatedPrice.CurrencyCode + '\'}';

                //store dataToPass in hidden div
                PCA.getElement('div.dataToPass').set('text', datapass);

                // launch pricing change modal
                new CorbisUI.Popup('pricingChangeAlert', {
                    createFromHTML: true,
                    replaceText: [result.OldPrice, result.RecalculatedPrice.CurrencyCode, result.RecalculatedPrice.EffectivePrice, result.RecalculatedPrice.CurrencyCode]
                });

                this.el.destroy();
                this.rel.finishValidation();
                break;

            // LICENSING CHANGED               
            case 4:
                // COMMENTING OUT TILL THIS IS FIXED
                // DON'T SEE THE LICENSE ALERT TEMPLATE DIV
                new CorbisUI.Popup('licenseAlertModal', {
                    createFromHTML: true,
                    replaceText: [this.rel.id, this.rel.nonLocalizedLicense, this.rel.productUID]
                });
                this.el.destroy();
                this.rel.finishValidation();
                break;
        }
        if (ids != "" && result.Status != 3) {
            //this is a more generic way to deal with error (however this won't work for pricing change)
            ids = Corbis.LightboxCart.Contracts.V1.DragStatus.toLocalizedString(result.Status);
            var pinkyEle = CorbisUI.Cart.CreatePinkyObject(this.rel);
            new CorbisUI.Popup('modalErrorMsgTemplate', {
                createFromHTML: true,
                showModalBackground: true,
                closeOnLoseFocus: false,
                positionVert: 'middle',
                positionHoriz: 'middle',
                replaceText: [pinkyEle.innerHTML, ids]
            });
        }
        // update instructions
        CorbisUI.Cart.DisplayCheckoutInstructions();
        setScrollControls();
        afterDeleteAction();
    }

});




/***************************
    MAIN CLASS FUNCTIONS
****************************/

// Our Drag and Drop overlord.
CorbisUI.Cart.DragOverlord = new Class({
    
    Implements: [Options, Events],
    options: {},
    
    cartBlocks: null,
    
    initialize: function(options){
        
        this.cartBlocks = new Hash({});
        
        // get all cartblocks
        var cartBlocks = $$('div.cartBlock');
        
        cartBlocks.each(function(el) {
            var id = el.getElement('span.corbisID');
            this.cartBlocks.set(id, new CorbisUI.Cart.ThumbItem(el));
        }, this);
        // need to chunk this.
//        cartBlocks.chunk(function(el) {
//            var id = el.getElement('span.corbisID');
//            this.cartBlocks.set(id,new CorbisUI.Cart.ThumbItem(el));
//        } .bind(this), 100, null, function() {
//            //CorbisUI.Cart.BatchValidate = new CorbisUI.Cart.BatchValidate();
//            CorbisUI.QueueManager.cartMacros.runItem('batch_checkoutCapable');
//            // call adjustment to column heights
//            adjustColumnHeight();
//        },15);
        
        // get all priceMultiple blocks
        var pricingBlocks = $$('div.pricingBlock');
          
    }
    
});




/***************************
    HELPER FUNCTIONS
****************************/

CorbisUI.Cart.productUid = function(guid, corbisId) {
    //console.log('CorbisUI.Cart.productUid');
    this.guid = guid;
    this.corbisId = corbisId;
}

CorbisUI.Cart.DetermineItemZone = function(el) {
    //console.log('CorbisUI.Cart.DisplayItemZone');
    el = $(el);
    if(el.getParent().get('tag') == 'li'){
        return 'Checkout';
    }
    else{
        var check = el.getParent().get('id');
        return check.replace('Zone','');
    }
}

CorbisUI.Cart.DetermineZone = function(el) {
    //console.log('CorbisUI.Cart.DetermineZone');
    el = $(el);
    if (!el)
        return '';
    return el.get('id').replace('Box', '');
}

CorbisUI.Cart.DisplayCheckoutInstructions = function() {
    //console.log('CorbisUI.Cart.DisplayCheckoutInstructions');
    var check = CorbisUI.Cart.DropZones.Checkout.getElements('li');
    var listContainer = $('cartCarouselWithItems');
    
    if(check.length > 0){
        $('emptyCartCarosel').addClass('displayNone');
        if(listContainer.hasClass('displayNone')) listContainer.removeClass('displayNone');
    }else{
        $('emptyCartCarosel').removeClass('displayNone');
        if(!listContainer.hasClass('displayNone')) listContainer.addClass('displayNone');
    }

}

CorbisUI.Cart.DisplayPricedInstructions = function() {
    //console.log('CorbisUI.Cart.DisplayPricedInstructions');
    var check = CorbisUI.Cart.DropZones.Priced.getElements('div.cartBlock');
    var listContainer = CorbisUI.Cart.DropZones.Priced.getElement('div.instructions');

    if (check.length == 0) {
        //$('emptyCartCarosel').addClass('displayNone');
        if (listContainer.hasClass('displayNone')) listContainer.removeClass('displayNone');
    } else {
        //$('emptyCartCarosel').removeClass('displayNone');
        if (!listContainer.hasClass('displayNone')) listContainer.addClass('displayNone');
    }

}

CorbisUI.Cart.DisplayUnpricedInstructions = function() {
    //console.log('CorbisUI.Cart.DisplayUnpricedInstructions');
    var check = CorbisUI.Cart.DropZones.UnPriced.getElements('div.cartBlock');
    var listContainer = CorbisUI.Cart.DropZones.UnPriced.getElement('div.instructions');

    if (check.length == 0) {
        //$('emptyCartCarosel').addClass('displayNone');
        if (listContainer.hasClass('displayNone')) listContainer.removeClass('displayNone');
    } else {
        //$('emptyCartCarosel').removeClass('displayNone');
        if (!listContainer.hasClass('displayNone')) listContainer.addClass('displayNone');
    }

}

CorbisUI.Cart.GetTriage = function() {
    //console.log('CorbisUI.Cart.GetTriage');
    var TRIAGE = $('TRIAGE');
    if(!TRIAGE) TRIAGE = new Element('div',{id: 'TRIAGE'}).setStyle('height',1).inject(document.body);
    return TRIAGE;
}

CorbisUI.Cart.DeleteInstance = function(objRef) {
    //console.log('CorbisUI.Cart.DeleteInstance');
    // helper to delete object's instance
    // good for destroying clones and main thumbs, etc
    objRef = $lambda(false);
}

CorbisUI.Cart.ScaleImage = function(ele,options){
    //console.log('CorbisUI.Cart.ScaleImage');
    var Base = {
        marginTop: 128, //basic dimensions of container
        width: 128, // minimum width
        height: 128 // minimum height
    };
    if(options) Base = $merge(Base,options);

    //if(!margin) margin = 128;
    
    //console.log(Base);
    
    var OrigCoords = ele.getCoordinates();
    var Coords = {width: 0, height: 0};
    Coords.width = OrigCoords.width;
    Coords.height = OrigCoords.height;
    
    var newValues = Base;
    
    if (Coords.height > Coords.width)
    {
        newValues.width = newValues.width * (Coords.width / Coords.height);    
    }
    else
    {
        newValues.height = newValues.height * (Coords.height / Coords.width); 
    }
    
    var wrap, img, m;
    
    if(newValues.height <= Base.marginTop){
        wrap = (Base.marginTop / 2); //half of wrapper
        img = (newValues.height / 2); //half of image
        newValues.marginTop = (wrap - img);
    }
    
    if(Coords.height < options.marginTop){
       newValues.width = 90;
   }
 
    return newValues;
}

// PINKY OBJECT HELPER
// Thanks to Eli on this
    CorbisUI.Cart.CreatePinkyObject = function(objRef){
        //console.log('CorbisUI.Cart.CreatePinkyObject');
        var el = new Element('div',{
            id: 'pinky_'+objRef.productUID
        }).addClass('pinkyThumb');
        
        var pinkyWrap = new Element('div').addClass('pinkyWrap').inject(el);

        
        var newImage = objRef.handle.clone()
                        .setStyle('margin-top',0)
                        .inject(pinkyWrap);
        

        //var coords = objRef.handle.getCoordinates();
        
        //switch to dynamic solution instead of using pinky url
        //var newHeight = (90-coords.height)/2;
        
        // finagle the margin-top
        /*if(coords.height < 128){
            // x 90 / 128 / 2 = 0.3515625
            var newHeight = ((128-coords.height) * 0.3515625).round();
            newImage.setStyles({
                'margin-top' : newHeight,
                'width' : '90px', 
                'height' : (coords.height * 0.7031).round()
                });
        }
        else 
        {
            newImage.setStyle('height', 90);
        }*/
        newImage.setStyles(CorbisUI.Cart.ScaleImage(objRef.handle,{marginTop: 90, width: 90, height: 90}));
        // return the pinky dom object
        return el;

    }

    // this gets called from single item update (like dragging from priced to checkout)
    CorbisUI.Cart.priceChangeConfirmed = function(data) {
        //console.log('CorbisUI.Cart.priceChangeConfirmed');
        CorbisUI.Cart.PricedItemUpdateCallback(data.productUID, data.newPrice, data.currencyCode);
    }

    CorbisUI.Cart.PricedItemUpdateCallback = function(PID, PRICE, CURRENCY, fromBatch) {
        //console.log('CorbisUI.Cart.PricedItemUpdateCallback');
        // setup object to be bound to response
        var tempObject = {
            productUID: PID,
            productPrice: PRICE,
            productCurrency: CURRENCY,
            fromBatch: false
        }

        if (fromBatch) tempObject.fromBatch = true;

        // update price via webService and bind data to response
        Corbis.Web.UI.Checkout.CartScriptService.UpdateCartItemPrice(PID, CorbisUI.Cart.PricedItemUpdateCallback_response.bind(tempObject));

    }

    CorbisUI.Cart.PricedItemUpdateCallback_response = function(results, context, methodName) {
        //console.log('CorbisUI.Cart.PricedItemUpdateCallback_response');
        // update block on page
        var item = $('cartBlock_' + this.productUID);
        var objRef = item.retrieve('objectReference');
        objRef.updatePrice(this.productPrice, this.productCurrency);
        objRef.currentZone = 'Priced';
        objRef.moveToCheckout(this.fromBatch);

        // udpate totals
        addToCheckoutTotals(1, this.productPrice);

        // update instructions
        CorbisUI.Cart.DisplayCheckoutInstructions();
        setScrollControls();
        afterDeleteAction();
    }

    

/*****************************
    CHECKOUT ALL FUNCTIONS
******************************/

//    CorbisUI.QueueManager.cartMacros.addItem('batch_checkoutCapable',function(){
//        CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable(); 
//    });
    CorbisUI.Cart.BatchValidate = new Class({
        queue: null,
        batchQueue: [],
        resultQueue: null,
        paused: false,
        isCanceled: false,
        currentResult: null,
        queueTimer: null,
        cancelBatch: [],

        priceChangeItem: null,
        checkoutAllButton: null,

        keyPressEvent: null,

        dimmer: null,

        fragment: null,
        priceAmount: 0,
        itemsAmount: 0,

        isFinished: true,
        watchingKeys: false,

        initialize: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.initialize');
            this.dimmer = $('batchValidatorIndicator');
            $(document.body).grab(this.dimmer);
            this.queue = new Hash({});
            this.checkoutAllButton = $(CorbisUI.GlobalVars.Cart.checkoutAllId);
            this.areAnyItemsCheckoutCapable();
        },

        areAnyItemsCheckoutCapable: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable');
            var items = CorbisUI.Cart.DropZones.Priced.getElements('div.cartBlock');
            var good = false;
            items.each(function(el) {
                var objRef = el.retrieve('objectReference');
                if ($type(objRef.price) || objRef.perContract || objRef.pricedByAE) {
                    if (objRef.perContract && objRef.effectivePriceStatus.contains('CountryOrCurrencyError')) {
                        //do nothing -- see bug 16005
                    } else {
                        good = true;
                    }
                }

            });
            (good) ? this.enableCheckoutAllButton() : this.disableCheckoutAllButton();
            return good;
        },

        enableCheckoutAllButton: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.enableCheckoutAllButton');
            if (this.checkoutAllButton.hasClass('DisabledGlassButton')) setGlassButtonDisabled(this.checkoutAllButton, false);
        },

        disableCheckoutAllButton: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.disableCheckoutAllButton');
            if (!this.checkoutAllButton.hasClass('DisabledGlassButton')) setGlassButtonDisabled(this.checkoutAllButton, true);
        },

        newBatch: function() {
            if (this.isFinished) {
                //console.log('CALLING: CorbisUI.Cart.BatchValidate.newBatch');
                //console.profile()
                // reset some things
                this.queue = new Hash({});
                this.paused = false;
                this.isCanceled = false;
                this.cancelBatch = [];
                this.currentResult = null;

                // get all items in priced
                var items = CorbisUI.Cart.DropZones.Priced.getElements('div.cartBlock');

                // loop through and find which ones are good
                items.each(function(el) {
                    var objRef = el.retrieve('objectReference');

                    if (($type(objRef.price) || objRef.perContract || objRef.pricedByAE) && !this.queue.hasValue(objRef.productUID)) {
                        this.queue.set(objRef.productUID, objRef);
                    }
                }, this);

                if (this.queue.getLength() == items.length) {
                    this.secondValidationStep();

                    // if not all the items are theoretically good, then alert the user  
                    // i.e. Contact Us, Update Use  
                } else {

                    new CorbisUI.Popup('someItemsCantCheckoutModal', {
                        createFromHTML: true
                    });
                    return false;

                }
            }
        },

        buildBatchData: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.buildBatchData');

            this.batchQueue = [];

            this.queue.each(function(val, key) {

                var count = this.batchQueue.length;
                val.ghost();
                this.batchQueue[count] = val.buildAjaxParameters();

            }, this);

            return this.batchQueue;

        },

        cancelValidation: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.cancelValidation');
            this.isCanceled = true;
        },

        secondValidationStep: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.secondValidationStep');
            this.fragment = document.createDocumentFragment();
            this.priceAmount = 0;
            this.itemsAmount = 0;
            this.memoryCount = 0;
            this.isFinished = false;
            this.watchingKeys = false;
            this.dimmer.fade('show');
            this.startKeyPress_watch();
            this.buildBatchData();

            if (this.batchQueue.length > 0) this.validate();
        },

        startKeyPress_watch: function() {
            if (!this.watchingKeys) {
                //console.log('START: keypress watch');
                this.keyPressEvent = this.keyPress_watchEvent.bind(this);
                ((Browser.Engine.trident) ? $(document.body) : $(window)).addEvent('keydown', this.keyPressEvent);
                this.watchingKeys = true;
            }
        },

        stopKeyPress_watch: function() {
            //console.log('STOP: keypress watch');
            ((Browser.Engine.trident) ? $(document.body) : $(window)).removeEvent('keydown', this.keyPressEvent);
        },

        keyPress_watchEvent: function(event) {
            //console.log('-->EVENT: keypressed');
            //console.log(event);
            var keysToWatch = ['space', 'esc'];
            if (keysToWatch.contains(event.key)) event.preventDefault();
        },

        validate: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.validate');
            this.currentResult = 0;
            Corbis.Web.UI.Checkout.CartScriptService.ValidateItems(this.batchQueue, this.validateCallback.bind(this));

        },

        validateCallback: function(results, context, methodName) {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.validateCallback');
            this.resultQueue = results;

            // TEST DATA - PLEASE REMOVE
            this.resultQueue.each(function(obj, i) {
                if (obj.ProductUid == "71691d53-26f1-4179-aefe-ddd14f59d4f1") {
                    this.resultQueue[i].Status = 3;
                    this.resultQueue[i].OldPrice = this.resultQueue[i].RecalculatedPrice.EffectivePrice;
                    this.resultQueue[i].RecalculatedPrice.EffectivePrice = Math.floor(this.resultQueue[i].OldPrice * 1.3);
                }
            }, this);
            // END TEST DATA

            this.resultLoop();
        },

        timerFunction: function() {

            if (this.paused == false) {
                //console.log('UNPAUSED');
                $clear(this.queueTimer);
                this.currentResult++;
                this.resultLoop();
            } else {
                //console.log('waiting...');
            }
        },

        startTimer: function() {
            //console.log('paused');
            this.paused = true;
            this.queueTimer = this.timerFunction.periodical(2000, this);
        },

        stopTimer: function() {
            this.paused = false;
            this.memoryCount = 0;
            $clear(this.queueTimer);
        },

        freeMemory_start: function() {
            //console.log('FREEING MEMORY --[START]');
            this.freeMemory_stop.delay(250, this);
        },

        freeMemory_stop: function() {
            //console.log('FREEING MEMORY --[STOP]');
            this.memoryCount = 0;
            this.resultLoop();
        },

        resultLoop: function() {

            if (this.memoryCount == 10 && !this.isCanceled) {
                this.freeMemory_start();
            } else {
                if (this.currentResult < this.resultQueue.length && !this.paused && !this.isCanceled) {
                    //console.log('[[]] current result: ' + this.currentResult);
                    this.resultQueue[this.currentResult].processed = true;
                    var nextResult = this.resultQueue[this.currentResult];
                    this.itemStatusUpdate(nextResult);
                }
                // THE END! NO MORE TO VALIDATE!
                if (this.currentResult == this.resultQueue.length && !this.isCanceled) {

                    this.batchFinished();
                }
            }

            if (this.isCanceled) this.cancelTheseItems();


        },

        itemStatusUpdate: function(result) {

            var ids = "";

            var rel = this.queue.get(result.ProductUid);

            // NOTE
            // Status 2, 3, 4 need to be defined more thoroughly        

            switch (result.Status) {
                // SUCCESS                                                        
                case 1:

                    //rel.moveToCheckout(true);
                    this.fragment.appendChild(quickerMoveToCheckout.bind(rel).run());
                    rel.revive();
                    rel.price = result.RecalculatedPrice.EffectivePrice;
                    this.itemsAmount++;
                    this.priceAmount = this.priceAmount + result.RecalculatedPrice.EffectivePrice.toInt();

                    //addToCheckoutTotals(1, result.RecalculatedPrice.EffectivePrice);
                    this.memoryCount++;
                    this.currentResult++;
                    this.resultLoop();
                    break;

                // IMAGE NOT AVAILABLE                                                                                                                  
                case 2:
                    // We're not notifying the user if the image
                    // is not available, and just moving it back to priced.

                    // need some function called here to change the
                    // image product block to reflect no image available

                    this.currentResult++;
                    this.resultLoop();

                    rel.revive();
                    rel.finishValidation();

                    // NOTE: we should add a method on the 
                    // main block object to show the image 
                    // not available graphic
                    break;

                // PRICING CHANGE                                                                                                                  
                case 3:
                    //console.log('BATCH VALIDATE: PRICING CHANGE');
                    $('batchPricingChangeAlert')
                        .getElement('td.picture')
                        .empty()
                        .grab(CorbisUI.Cart.CreatePinkyObject(rel));

                    new CorbisUI.Popup('batchPricingChangeAlert', {
                        createFromHTML: true,
                        replaceText: [result.OldPrice, result.RecalculatedPrice.CurrencyCode, result.RecalculatedPrice.EffectivePrice, result.RecalculatedPrice.CurrencyCode]
                    });

                    this.startTimer();
                    this.priceChangeItem = result;


                    break;

                // LICENSING CHANGED                                                                                                                  
                case 4:

                    $('batchItemLicenseAlert')
                        .getElement('td.picture')
                        .empty()
                        .grab(CorbisUI.Cart.CreatePinkyObject(rel));

                    new CorbisUI.Popup('batchItemLicenseAlert', {
                        createFromHTML: true,
                        replaceText: [rel.handle.src]
                    });

                    this.startTimer();
                    rel.revive();
                    rel.finishValidation();

                    break;
            }

        },

        batchFinished: function() {
            if (!this.isFinished) {
                this.isFinished = true;
                //console.log('CALLING: CorbisUI.Cart.BatchValidate.batchFinished');
                CorbisUI.Cart.DropZones.Checkout.insertBefore(this.fragment, CorbisUI.Cart.DropZones.Checkout.getFirst());
                addToCheckoutTotals(this.itemsAmount, this.priceAmount);

                CorbisUI.Cart.DisplayCheckoutInstructions();
                setScrollControls();
                afterDeleteAction();
                this.areAnyItemsCheckoutCapable();

                this.dimmer.fade('hide');

                this.stopKeyPress_watch();
                //console.profileEnd()
            }
        },

        cancelTheseItems: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.canceltheseItems');
            this.isCanceled = false;

            //this.dimmer.fade('hide');

            this.cancelBatch = [];

            this.resultQueue.each(function(item, index) {
                if (item.ProductUid && !item.processed) {
                    var rel = this.queue.get(item.ProductUid);
                    rel.revive();
                    this.cancelBatch.include(item.ProductUid);
                }
            }, this);

            //this.stopKeyPress_watch();
            Corbis.Web.UI.Checkout.CartScriptService.MoveItemWithInCart(this.cancelBatch, 'Checkout', this.cancelTheseItemsCallBack.bind(this));

        },

        cancelTheseItemsCallBack: function(results, context, methodName) {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.cancelTheseItemsCallBack');
            //            this.areAnyItemsCheckoutCapable();
            //            CorbisUI.Cart.DisplayCheckoutInstructions();
            //            setScrollControls();
            //            afterDeleteAction();
            this.batchFinished();
        },

        priceChangeMove: function() {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.priceChangeMove');
            var rel = this.queue.get(this.priceChangeItem.ProductUid);

            this.PricedItemUpdateCallback(this.priceChangeItem.ProductUid, this.priceChangeItem.RecalculatedPrice.EffectivePrice, this.priceChangeItem.RecalculatedPrice.CurrencyCode, rel);

            //            rel.finishValidation();

            //            rel.revive();

            //            this.priceChangeItem = null;
            //            this.paused = false;

        },

        PricedItemUpdateCallback: function(PID, PRICE, CURRENCY, rel) {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.PricedItemUpdateCallback');
            // setup object to be bound to response
            var tempObject = {
                productUID: PID,
                productPrice: PRICE,
                productCurrency: CURRENCY,
                objRef: rel,
                parentObj: this
            }


            // update price via webService and bind data to response
            Corbis.Web.UI.Checkout.CartScriptService.UpdateCartItemPrice(PID, this.PricedItemUpdateCallback_response.bind(tempObject));

        },

        PricedItemUpdateCallback_response: function(results, context, methodName) {
            //console.log('CALLING: CorbisUI.Cart.BatchValidate.PricedItemUpdateCallback_response');
            // update block on page

            this.parentObj.fragment.appendChild(quickerMoveToCheckout.bind(this.objRef).run());

            this.objRef.updatePrice(this.productPrice, this.productCurrency);

            this.parentObj.itemsAmount++;
            this.parentObj.priceAmount = this.parentObj.priceAmount + this.productPrice.toInt();

            //this.objRef.finishValidation();

            //this.objRef.revive();

            this.parentObj.priceChangeItem = null;
            this.parentObj.paused = false;

            //            var item = $('cartBlock_' + this.productUID);
            //            var objRef = item.retrieve('objectReference');
            //            objRef.updatePrice(this.productPrice, this.productCurrency);
            //            objRef.currentZone = 'Priced';
            //            objRef.moveToCheckout(this.fromBatch);

            //            // udpate totals
            //            addToCheckoutTotals(1, this.productPrice);

            //            // update instructions
            //            CorbisUI.Cart.DisplayCheckoutInstructions();
            //            setScrollControls();
            //            afterDeleteAction();
        }


    });

    CorbisUI.Cart.UpdatePriceLink = function(context, linkType, isContactLink, twice) {
        //console.log('CALLING: CorbisUI.Cart.UpdatePriceLink');
        // linkType can be 'licenseDetails' or 'priceTag'
        // isContactLink is a boolean

        var crazyMap = {
            'priceTag': 'licenseDetails',
            'licenseDetails': 'priceTag'
        }

        var priceLink = context.el.getElement('span[id$=' + linkType + ']');
        var priceLinkAction = priceLink.getProperty('onclick');
        var changed = false;

        this.updateLink = function(Link, Action) {
            Link.removeProperty('onclick');

            if (Browser.Engine.trident) {
                Link.onclick = Action;
            } else {
                Link.setProperty('onclick', Action);
            }
        }

        if (!context.isRfcd && $type(priceLinkAction)) {
            priceLinkAction = priceLinkAction.toString();


            if (priceLinkAction.contains('ProductUid=00000000-0000-0000-0000-000000000000') && !isContactLink) {
                var newGuidLink = 'ProductUid=' + context.productUID;
                var oldGuidLink = 'ProductUid=00000000-0000-0000-0000-000000000000';
                var newPriceLink = priceLinkAction.replace(oldGuidLink, newGuidLink);
                if (Browser.Engine.trident) {
                    // silly JSON hack to get string back into native JS object
                    var temp = JSON.decode("{\"newFunc\":" + newPriceLink + "}");
                    newPriceLink = temp.newFunc;
                }
                this.updateLink(priceLink, newPriceLink);

            }

            if (isContactLink) {
                var newPriceLink = "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" + context.id + "', null, 1, 'Cart');return false;";
                if (Browser.Engine.trident) {
                    // silly JSON hack to get string back into native JS object
                    var temp = JSON.decode("{\"newFunc\": function(){" + newPriceLink + "}}");
                    newPriceLink = temp.newFunc;
                }
                this.updateLink(priceLink, newPriceLink);

            }

            if (!twice) CorbisUI.Cart.UpdatePriceLink(context, crazyMap[linkType], isContactLink, true);


        }
    };



/***************************
    DOM READY ACTIONS
****************************/
    window.addEvent('domready', function() {

        // SETUP - DRAG ZONES
        CorbisUI.Cart.DragZones.Checkout = $('CheckoutBox');
        CorbisUI.Cart.DragZones.Priced = $('PricedBox');
        CorbisUI.Cart.DragZones.UnPriced = $('UnPricedBox');
        CorbisUI.Cart.DragZones.PriceMultiple = $('PriceMultipleBox');

        // SETUP - DROP ZONES
        CorbisUI.Cart.DropZones.Checkout = $('cartCarouselItems');
        CorbisUI.Cart.DropZones.Priced = $('PricedZone');
        CorbisUI.Cart.DropZones.UnPriced = $('UnPricedZone');
        CorbisUI.Cart.DropZones.PriceMultiple = $('PriceMultipleZone');

        // SETUP CAROUSEL NEXT AND PREVIOUS CACHED LINKS
        CorbisUI.Cart.Carousel.Previous = CorbisUI.Cart.DragZones.Checkout.getElement("img.cartCarouselPrevious");
        CorbisUI.Cart.Carousel.Next = CorbisUI.Cart.DragZones.Checkout.getElement("img.cartCarouselNext");

        // SUMMON OUR DRAG-n-DROP OVERLORD
        new CorbisUI.Cart.DragOverlord();

        CorbisUI.Cart.BatchValidate = new CorbisUI.Cart.BatchValidate();

        // call adjustment to column heights
        adjustColumnHeight();

        // how to purchase tooltip
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
        var HowToPurchaseTips = new Tips('.howToPurchaseTooltip', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            className: 'TIP-license-details',
            offsets: { x: 5, y: -150 },
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
        
    });


/***************************
    CODE COPIED FROM
    Cart.aspx
    
    needs to be cleaned up
    
    Code by others not Chris
****************************/

function showDetails(item)
{

    item = $(item);
    var target = item.getParent().getParent();
    // check if ghosted
    if(target.hasClass('cartBlock_ghost')) return;

    if (item.getParent().hasClass('infoIcon_ON')) {
        refreshIcon(target.getElement('img.RefreshMePlease'));
        return false;
    }else{
        item.getParent().addClass('infoIcon_ON');
        var detailsWrap = target.getElement('div.detailsWrap');
        detailsWrap.fade('hide');
        detailsWrap.fade('in');
        return false;
    }
}; 

function refreshIcon(item)
{
    item = $(item);
    var infoIcon = item.getParent().getParent().getElement('.infoIcon');
    var detailsWrap = item.getParent().getParent().getElement('div.detailsWrap');
    if(infoIcon.hasClass('infoIcon_ON')) infoIcon.removeClass('infoIcon_ON');
    detailsWrap.fade('out');
    
}    
    
var maxHeight;
window.addEvent('domready', function(){
        window.addEvent('resize', adjustColumnHeight); 
        adjustColumnHeight();
        setPricingControls();
});

function adjustColumnHeight()
{
    var maxHeight = 0;
    var items = $$('.sameHeight');
    items.each(function(floatLDiv){
        maxHeight = Math.max(maxHeight, getDivContainerHeight(floatLDiv));
    });
    items.each(function(div){
        //div.tween('height', maxHeight);
        div.setStyle('height', maxHeight);
    });
    //$('pricedZone').setStyle('height', maxHeight);

    setCarouselWidth();
}

function getDivContainerHeight(divContainer)
{
    var divHeight;
    var blocks = divContainer.getElements('.cartBlock');
    var lastblock = null;
    blocks.each(function(block){
        if (lastblock == null || lastblock.getPosition(divContainer).y < block.getPosition(divContainer).y)
        {
            lastblock = block;
        }
    });
    if (lastblock != null)
    {
        return lastblock.getCoordinates(divContainer).bottom + 10;
    }
    return 200;
    
}    

// this still requires some other stuff to make it "appear" to work flawless
// THIS IS ONLY FOR BATCH MOVES TO CHECKOUT
function quickerMoveToCheckout() {
        var tmp = new Element('li');
        this.el.inject(tmp);
        //tmp.inject(CorbisUI.Cart.DropZones.Checkout, 'top');

        if (this.productUID && this.currentZone == 'UnPriced') {
            CorbisUI.Cart.UpdatePriceLink(this, 'priceTag');
        }
        this.currentZone = 'Checkout';
        this.reset();
        this.revive();
        // reset checkout controls
//        setScrollControls();
//        CorbisUI.Cart.DisplayPricedInstructions();
        //        if (!fromBatch) CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
        return tmp;
    }
// this still requires some other stuff to make it "appear" to work flawless
// THIS IS ONLY FOR BATCH MOVES TO PRICED
function quickerMoveToPriced(){
        this.currentZone = 'Priced';
        this.status = "priced";
        CorbisUI.Cart.UpdatePriceLink(this, 'priceTag');
        this.reset();
}
function clearCheckoutItems() {
	var carouselItems = $$('#cartCarouselItems div.cartBlock');
    if (carouselItems && carouselItems.length > 0) {
		var productUidList = new Array(carouselItems.length);
        var afterList = new Array(carouselItems.length);
        var fragment = document.createDocumentFragment();
        var minusPrice = 0;
        var listCount = carouselItems.length;
		carouselItems.each
		(
			function(item, index) {
				productUidList[index] = item.id.replace('cartBlock_', '');
				var objRef = item.retrieve('objectReference');
			    minusPrice = minusPrice + objRef.price;
			    fragment.appendChild(objRef.el);
			    quickerMoveToPriced.bind(objRef).run();
			    //afterList[index] = objRef;
			    //				removeFromCheckoutTotals(1, objRef.price);

			    //			    objRef.moveToPriced(true); // true that it is batched
			    //			    
			    //				objRef.reset();
								
			}
		);
			removeFromCheckoutTotals(listCount, minusPrice);
			//CorbisUI.Cart.DropZones.Priced.grab(fragment, 'top');

			CorbisUI.Cart.DropZones.Priced.insertBefore(fragment, CorbisUI.Cart.DropZones.Priced.getFirst());
		
		//deleteFromCartContainer(productUidList, 'Checkout');
        Corbis.Web.UI.Checkout.CartScriptService.MoveItemWithInCart(productUidList, 'Checkout', deleteFromCartContainerCallback);
//        afterList.chunk(function(obj) {
//            removeFromCheckoutTotals(1, obj.price);
//            var container = obj.el.getParent('li');
//            obj.moveToPriced(true); // true that it is batched
//            obj.reset();
//            container.destroy();
//        }, 150, null, function() {
//            setScrollControls();
//            HideModal('deleteCheckoutModal');
//        },15);
//		if (removeFromCart)
//		{
//			//deleteFromCartContainer(productUidList, 'Priced');
//		}
//		else
//		{
//			adjustColumnHeight();
//		}
	}
	
	var carouselItems = $('cartCarouselItems').getElementsByTagName('li');
	while(carouselItems[0])
	{
		$('cartCarouselItems').removeChild(carouselItems[0]);
	}

	setScrollControls();
	CorbisUI.Cart.DisplayPricedInstructions();
    CorbisUI.Cart.DisplayUnpricedInstructions();
    CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
    adjustColumnHeight();
	HideModal('deleteCheckoutModal');
}

function OpenClearCheckoutModal()
{
	if($('cartCarouselItems').getElementsByTagName('li').length > 0) 
		OpenModal('deleteCheckoutModal');
}

function setCarouselWidth()
{
	availableCarouselWidth = $get('cartCarousel').clientWidth - 44;
	var carouselItems = $$('#cartCarouselItems li');

	if(carouselItems.length >= 2)
	{
		setCarouselItemData(carouselItems);
	}
	else
	{
		$('cartCarouselWithItems').setStyle('width', availableCarouselWidth);
	}
	
	setScrollControls();
}

function setCarouselItemData(carouselItems)
{
	//Getting the width this way for browser and margin independence.
	carouselItemWidth = carouselItems[1].offsetLeft - carouselItems[0].offsetLeft;
	carouselItemCount = Math.floor(availableCarouselWidth / carouselItemWidth);
	
	carouselWidth = carouselItemCount * carouselItemWidth + 2;
	
	$('cartCarouselWithItems').setStyle('width', carouselWidth);
}
        
var carouselItemCount;
var carouselItemWidth;
var availableCarouselWidth;
function setScrollControls()
{

    var cartCarouselItems = $('cartCarouselItems');
    var carouselItems = $$('#cartCarouselItems li');
    var cartCarouselPrevious = CorbisUI.Cart.Carousel.Previous;
    var cartCarouselNext = CorbisUI.Cart.Carousel.Next;
	// var cartCarouselPrevious = $('cartCarouselPrevious');
	// var cartCarouselNext = $('cartCarouselNext');
	var cartCarouselContainerLeft = $('cartCarouselWithItems').offsetLeft;
	var emptyCartCarosel = $('emptyCartCarosel');
	var cartCarouselWithItems = $('cartCarouselWithItems');
	var checkoutButton = $('checkout');
	var clearCheckout = $('itemToCheckoutClear').getElement('span.textIconButtonContainer');
	
	cartCarouselPrevious.removeClass('disabled');
	cartCarouselNext.removeClass('disabled');
	emptyCartCarosel.removeClass('displayNone');
	cartCarouselWithItems.removeClass("displayNone");
	setGlassButtonDisabled(checkoutButton.getElement('div.GlassButton'), (carouselItems.length == 0));
	setTextIconButtonDisabled(clearCheckout, carouselItems.length == 0);

	if (carouselItems.length == 0)
	{
		cartCarouselWithItems.addClass('displayNone');
		cartCarouselPrevious.addClass('disabled');
		cartCarouselNext.addClass('disabled');	
	}
	else 
	{
		emptyCartCarosel.addClass('displayNone');
		
		if (!carouselItemWidth && carouselItems.length >= 2)
		{
			setCarouselItemData(carouselItems);
		}
		
		if (carouselItems.length < 2 || carouselItems.length <= carouselItemCount)
		{
			cartCarouselPrevious.addClass('disabled');
			cartCarouselNext.addClass('disabled');
			cartCarouselItems.setStyle('left', 0);
		}
		else 
		{
			if (cartCarouselItems.offsetLeft >= 0)
			{
				cartCarouselPrevious.addClass('disabled');
			}

			if (cartCarouselItems.offsetLeft <= ((carouselItemCount - carouselItems.length) * carouselItemWidth))
			{
				cartCarouselNext.addClass("disabled");
			}
		}
	}
}

function scrollCarousel(scrollPrevious)
{
	var scrollAmount = null;
	
	
	if(scrollPrevious)
	{
	    if (!CorbisUI.Cart.Carousel.Previous.hasClass('disabled'))
		{
			scrollAmount = carouselItemWidth * carouselItemCount;
		}
	}
	else
	{
	    if (!CorbisUI.Cart.Carousel.Next.hasClass('disabled'))
		{
			scrollAmount = carouselItemWidth * -1 * carouselItemCount;
		}
	}
	
	if (scrollAmount != null)
	{
		$('cartCarouselItems').setStyle('left', ($('cartCarouselItems').offsetLeft + scrollAmount)); 
		setScrollControls();
	}
}

function GetProductToCheckout(controlId)
{
	var commandArgument = '';
	var seperator = '';
	var carouselItems = $$('#cartCarouselItems div.cartBlock');

	if (carouselItems && carouselItems.length > 0)
	{
		carouselItems.each
		(
			function(item, index)
			{
				commandArgument = commandArgument + seperator + item.id.replace('cartBlock_', '');
				if (index == 0) seperator = ',';
			}
		);

		theForm.__EVENTARGUMENT.value = commandArgument;
		
		return true;
	}
	else
	{
		return false;
	}
}

//        $addHandler(window, "resize", adjustColumnHeight); 
//        function adjustColumnHeight() 
//        {
//            //document.body.style.height = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight) + "px";
//            var unpricedZone = $get('unpricedZone');
//            if (unpricedZone != null)
//            {
//                var pricedZone = $get('pricedZone');
//                var pricingZone = $get('pricingZone');
//                var maxHeight = Math.max(pricingZone.clientHeight, Math.max(unpricedZone.clientHeight, pricedZone.clientHeight)) ;
//                var pricedHeight;
//                //unpricedZone.getAttribute('height', unpricedHeight);
//                //pricedZone.getAttribute('height', pricedHeight);
//                if (unpricedZone.clientHeight < maxHeight)
//                {
//                    unpricedZone.style.minHeight= maxHeight + 'px';
//                }
//                if (pricedZone.clientHeight < maxHeight)
//                {
//                    pricedZone.style.minHeight= maxHeight + 'px';
//                }
//                if (pricingZone.clientHeight < maxHeight)
//                {
//                    pricingZone.style.minHeight= maxHeight + 'px';
//                }
//            }    

//        }
function addToCheckoutTotals(count, cost) {

    var totalCountElementId = $('checkoutTotalCount1').getFirst();
    var totalCount = totalCountElementId.get('text').toInt();
    totalCountElementId.set('text', totalCount + parseInt(count));

    var totalCostElementId = $('checkoutTotalCost1').getFirst();

    var numberCheck = totalCostElementId.get('text');
    if (numberCheck.substr(0, 1) == "$") numberCheck = numberCheck.substr(1, numberCheck.length);
    
    var totalCost = Number.parseLocale(numberCheck);
    if (!$type(totalCost)) totalCost = 0;

    if (cost == DisplayTextPerContract || numberCheck == DisplayTextPerContract) {
        // assume "per contract" already shows up here
        //totalCostElementId.set('text', DisplayTextPerContract);
    }
    else {
        var newCost = (totalCost.toFloat() + cost.toFloat()).localeFormat('C');
        totalCostElementId.set('text', (newCost.substr(0,1) == "$")?newCost.substr(1,newCost.length):newCost);
    }
}

function removeFromCheckoutTotals(count, cost) {

    var totalCountElementId = $('checkoutTotalCount1').getFirst();
    var totalCount = totalCountElementId.get('text').toInt();

    var newCount = totalCount.toInt() - count.toInt();
    totalCountElementId.set('text', ((newCount < 0) ? 0 : newCount));
    
    var totalCostElementId = $('checkoutTotalCost1').getFirst();
    var numberCheck = totalCostElementId.get('text');

    if (cost == DisplayTextPerContract || numberCheck == DisplayTextPerContract) {
        return;
    }
    
    var totalCost = Number.parseLocale(totalCostElementId.get('text'));
    
    var costCheck = parseFloat(totalCost - parseFloat(cost));

    var newCost = (costCheck < 0 || !$type(costCheck))?parseFloat(0).localeFormat('C'):(totalCost.toFloat() - cost.toFloat()).localeFormat('C');

    totalCostElementId.set('text', (newCost.substr(0, 1) == "$") ? newCost.substr(1, newCost.length) : newCost);
}

function myDeleteFunction(pid, zone){
    var list = new Array();
    var target = $('cartBlock_' + pid);
    var objRef = target.retrieve('objectReference');
    zone = objRef.currentZone;
    /*if (zone == 'checkout')
    {
        zone = 'Checkout';
    }*/
    list[0] = pid;
                                            
    Corbis.Web.UI.Checkout.CartScriptService.DeleteItemsFromCartContainer(list, zone, deleteThumbCompleted);

    return true;
}

function deleteThumbCompleted(results, context, methodName)
{
    var type = typeof (results);
    if (type == "object"){
        if (results != null && results.length){
            var ids = "";
            for (var i = 0; i < results.length; i++ ){
                ids += "ProductUid: " + results[i].ProductUid + "\n";
                ids += "DeleteSucceeded: " + results[i].DeleteSucceeded + "\n";
                ids += "\n";
                if (results[i].DeleteSucceeded) {
                    var block = $('cartBlock_' + results[i].ProductUid);
                    var objRef = block.retrieve('objectReference');
                    if (objRef.price != null && $type(objRef.price.toInt()) == 'number')
                    {
                        removeFromCheckoutTotals(1, objRef.price.toInt());
                    }


                    block.destroy();
                    /**/
                    if (objRef.currentZone == 'Priced') {
                        CorbisUI.Cart.DisplayPricedInstructions();
                    }
                    if (objRef.currentZone == 'UnPriced') {
                        CorbisUI.Cart.DisplayUnpricedInstructions();
                    }
                    /**/
                    
                }
            }
            CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
            afterDeleteAction();
            
            var carouselItems = $$('#cartCarouselItems .cartBlock');
            var checkoutButton = $('checkout').getElement('div.GlassButton');
            var clearCheckout = $(CorbisUI.GlobalVars.Cart.clearAllButton);
            var clearCheckoutLink = $(CorbisUI.GlobalVars.Cart.clearAllButton).getParent().getElement('a');
            var emptyCartCarosel = $('emptyCartCarosel');
            
            if(carouselItems.length == 0)
            {
                // Disabling the buttons Checkout/Clear Items.Enabling the Text
                setGlassButtonDisabled(checkoutButton, true);
                setTextIconButtonDisabled(clearCheckout, true);
                setTextIconButtonDisabled(clearCheckoutLink, true);
                emptyCartCarosel.removeClass("displayNone");
            }
            
        }
    }

}


function modalDeleteAlert(target, zone)
{
    target = $(target).getParent().getParent();
    
    //Get position on page in order to properly align modal
    targetCoords = $(target).getCoordinates();
    var hPosition = targetCoords['left'];
    hPosition = (hPosition >= 500) ? "left" : "right"; 

    // check if ghosted
    if(target.hasClass('cartBlock_ghost')) return;
    
    type = CorbisUI.Cart.DetermineZone(target);
    
    var objRef = target.retrieve('objectReference');
    
    var pinkyEle = CorbisUI.Cart.CreatePinkyObject(objRef);
    
    new CorbisUI.Popup('modalDeleteTemplate', { 
        createFromHTML: true,
        showModalBackground: false,
        closeOnLoseFocus: true,
        centerOverElement: target, 
        positionVert: 'middle', 
        positionHoriz: hPosition,
        replaceText: [ pinkyEle.innerHTML, objRef.productUID, zone ]
        });
}

//<!-- ****************************************************************************************** -->
    //<!-- ************************** This is the start of semi-redundant code that Mike ************ -->
    //<!-- ************************** wants to consolidate with the delete image code using a flag ** -->
    //<!-- ****************************************************************************************** -->
function modalDeleteAlertTempDeleteAll(zone,el)
{
    CorbisUI.Cart.DropZones[zone].addClass('selectAllBlocks');
    
    //Get position on page in order to properly align modal
    targetCoords = $(el).getCoordinates();
    var hPosition = targetCoords['left'];
    hPosition = (hPosition >= 500) ? "left" : "right"; 
    
    new CorbisUI.Popup(zone, { 
        showModalBackground: false,
        closeOnLoseFocus: true,
        centerOverElement: el, 
        positionVert: 'top', 
        positionHoriz: hPosition,
        onHide: function() { orangeBorderOff(zone); }
    });
    
}

function orangeBorderOff(zone)
{

    CorbisUI.Cart.DropZones[zone].removeClass('selectAllBlocks');
    HideModal(zone);
    
}
    //<!-- ****************************************************************************************** -->
    //<!-- ************************** This is the end of semi-redundant code that Mike ************** -->
    //<!-- ************************** wants to consolidate with the delete image code using a flag ** -->
    //<!-- ****************************************************************************************** -->

function clearPricingItems()
{
	var pricingZoneItems = $$('#PriceMultipleZone div.pinkyThumb');

	if (pricingZoneItems && pricingZoneItems.length > 0)
	{
	
	    var ids = new Array();
	    
	    pricingZoneItems.each
		(
			function(item, index)
			{
				var objRef = item.retrieve('objectReference');
				ids[index] = objRef.rel.productUID;
                objRef.rel.destroyPricingThumb();
			}
		);
		
		deleteFromCartContainer(ids, 'PriceMultiple');
    	afterDeleteAction();
		
	}
	
	//HideModal('deletePriceMultipleModal');
} 
function deleteAllItems(zone)
{
	var ZoneItems = CorbisUI.Cart.DropZones[zone].getElements('div.cartBlock');
	/*var selectAllGlow = zone+"Zone";
	selectAllGlow.addClass('borderOn');
	UnPricedZone.addClass('borderOn');*/
	/*$('UnPricedZone').addClass('borderOn');*/
	if (ZoneItems && ZoneItems.length > 0)
	
	{
	    var ids = new Array();
	    ZoneItems.each
		(
			function(item, index)
			{
				var objRef = item.retrieve('objectReference');
				ids[index] = objRef.productUID;
                objRef.destroyMe();
			}
		);
		deleteFromCartContainer(ids, zone);
    	afterDeleteAction();
} 

	orangeBorderOff(zone);
} 
function OpenPricingModal()
{
	if($$('#PriceMultipleZone div.pinkyThumb').length > 0) 
		OpenModal('deletePriceMultipleModal');
}

function deleteFromCartContainer(ids, cartContainer){
    //console.log(ids);
    if (cartContainer == 'PriceMultiple')
    {
        Corbis.Web.UI.Checkout.CartScriptService.MoveItemWithInCart(ids, cartContainer, deleteFromCartContainerCallback);
    }
    else 
    {
        Corbis.Web.UI.Checkout.CartScriptService.DeleteItemsFromCartContainer(ids, cartContainer, deleteFromCartContainerCallback);
        if (cartContainer == 'Priced') 
        {
            CorbisUI.Cart.DisplayPricedInstructions();
            var deleteAllPriced = $(CorbisUI.GlobalVars.Cart.deleteAllPriced);
            var deleteAllPricedLink = null;
            if(deleteAllPriced != null)
            {
                deleteAllPricedLink = deleteAllPriced.getParent().getElement('a');
            }
            
            setTextIconButtonDisabled(deleteAllPriced,true);
            setTextIconButtonDisabled(deleteAllPricedLink,true);

                
        }
        if (cartContainer == 'UnPriced') 
        {
            CorbisUI.Cart.DisplayUnpricedInstructions();
            
            var deleteAllUnpriced = $(CorbisUI.GlobalVars.Cart.deleteAllUnpriced);
            var deleteAllUnpricedLink = null;
            if(deleteAllUnpriced != null)
            {
                deleteAllUnpricedLink = deleteAllUnpriced.getParent().getElement('a');
            }
            
            setTextIconButtonDisabled(deleteAllUnpriced,true);
            setTextIconButtonDisabled(deleteAllUnpricedLink,true);
        }
    }
}

function deleteFromCartContainerCallback(results, context, methodName){
    //console.log(results);
    CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
    afterDeleteAction();
}
    
function setPricingControls()
{
	var pricingZoneItems = $$('#PriceMultipleZone div.pinkyThumb');
	var pricingButton = $('priceNow').getElement('div.GlassButton');
	var clearPricing = $('clearPriceParent').getElement('span.textIconButtonContainer');
	var pricingZoneMessage = $$('#PriceMultipleZone div.instructions');
	var hasDisabledClass = clearPricing.hasClass('disabled');

	if (pricingZoneItems.length == 0)
	{
		if (!hasDisabledClass) 
		{
			setTextIconButtonDisabled(clearPricing, true);
			pricingZoneMessage.removeClass('displayNone');
			setGlassButtonDisabled(pricingButton, true);
		}
	}
	else
	{
		if (hasDisabledClass) 
		{
			setTextIconButtonDisabled(clearPricing, false);
			pricingZoneMessage.addClass('displayNone');
			setGlassButtonDisabled(pricingButton, false);
		}
	}
}

function openPriceWindow(CorbisId, LicenseModel, ProductUID) {

    if (ProductUID != null) {
        ProductUID = '&ProductUid=' + ProductUID;
    }
    switch(LicenseModel)
    {
        case "RM":
            OpenIModal(RMPRICINGURL + CorbisId + ProductUID, 700, 675);
            break;
        case "RF":
            OpenIModal(RFPRICINGURL + CorbisId + ProductUID, 640, 480);
            break;
        case "RS":
            OpenIModal(RSPRICINGURL + CorbisId + ProductUID, 640, 480);
            break;
        default:
            break;
    }
}
function adjustCounters()
{
    $('cartCount').innerHTML = $$('.cartBlock').length;
    fixIECheckoutWidgetWidth();
    $('detailTotal').innerHTML = $$('.cartBlock').length;
    $('detailPriced').innerHTML = $$('#PricedZone .cartBlock').length + $$('#cartCarouselWithItems .cartBlock').length;
    $('detailUnPriced').innerHTML = $$('#UnPricedZone .cartBlock').length;
    adjustCheckoutCounters();
}

function adjustCheckoutCounters() {
    var items = CorbisUI.Cart.DropZones.Checkout.getElements('div.cartBlock');
    var totalCost = 0;

    items.each(function(el) {
    var objRef = el.retrieve('objectReference');
        if ($type(objRef.price)) {
            totalCost = totalCost.toFloat() + objRef.price.toFloat();
        }
    });

    var totalCostElementId = $('checkoutTotalCost1').getFirst();
    if (totalCostElementId.getProperty('text') != DisplayTextPerContract) {
        totalCostElementId.set('text', totalCost.localeFormat('C'));
    }
    var totalCountElementId = $('checkoutTotalCount1').getFirst();
    totalCountElementId.set('text', items.length);
    
}

function adjustButtons()
/// summery:disable "deleteall" if the container is empty
/// waiting for Ron to provide new function
{
    
}

function adjustEmptyContainerMsg()
///Nick will adjust here
{
}

function afterDeleteAction()
{
    adjustButtons();
    adjustColumnHeight();
    adjustCounters();
    adjustEmptyContainerMsg();
    
}


/***************************
    HANDLER FUNCTIONS
****************************/

CorbisUI.Cart.Handler = {
    moveItemToPriced: function(corbisId) {
        //console.log('CorbisUI.Cart.Handler.moveItemToPriced');
        var cartItem = $(parent.document).getElement('span.corbisID[text=' + corbisId + ']').getParent().getParent().retrieve('objectReference');
        var argsToPass = {
            methodArgs: {
                _script: 'Cart.js',
                _method: 'CorbisUI.Cart.Handler.moveItemToPriced',
                _corbisId: corbisId
            }
        };
        //Corbis.Web.UI.Checkout.CartScriptService.GetPricedPricingDisplay(cartItem.productUID, CorbisUI.Cart.Handler.GetPricedPricingDisplayCallback, CorbisUI.MethodFailed.bind(argsToPass), cartItem);
        Corbis.Web.UI.Checkout.CartScriptService.GetPricedPricingDisplay(cartItem.productUID, CorbisUI.Cart.Handler.GetPricedPricingDisplayCallback, methodFailed, cartItem);
        //Corbis.Web.UI.Checkout.CartScriptService.GetPricedPricingDisplay(cartItem.productUID, CorbisUI.Cart.Handler.GetPricedPricingDisplayCallback, methodFailed, cartItem);
    },

    GetPricedPricingDisplayCallback: function(methodResults, context, methodName) {
        //console.log('CorbisUI.Cart.Handler.GetPricedPricingDisplayCallback');
        var results = methodResults.result;

        //console.log(methodResults);
        //console.log(results);


        if (context.pricedByAE && methodResults.status == 'Ok' && context.el.getElement('.pricedByAE')) {
            context.el.getElement('.pricedByAE').destroy();
        }
        context.el.getElement('div.action').getElement('span.actLikeLink').set('text', results);
        context.price = Number.parseLocale(stripeCurrencyCode(results)).toInt();

        //ie bug
        if (Browser.Engine.trident5) {
            context.el.getElements('input').each(function(btn) {
                btn.value = ' ';
            });
        }
        if (context.currentZone == 'UnPriced' || context.currentZone == 'Priced') {
            if (results != 'Contact us' && (context.id == CorbisUI.Cart.LastAction.CorbisId) && (CorbisUI.Cart.LastAction.DropZone == 'Checkout')) {
                context.validate();
            }
            else if (context.currentZone == 'UnPriced') {
                context.currentZone = 'Priced';
                if (methodResults.status.contains('ContactUs')) {

                    context.effectivePriceStatus = 'ContactUs';
                    context.el.setProperty('effectivePriceStatus', 'ContactUs');

                    //console.log('UH OH!! Needs to ContactUs');
                    CorbisUI.Cart.UpdatePriceLink(context, 'priceTag', true);


                }
                else {
                    context.effectivePriceStatus = 'Ok';
                    context.el.setProperty('effectivePriceStatus', 'Ok');
                    if (results == 'Per contract') context.perContract = true;
                }
                context.moveToPriced();

                CorbisUI.Cart.UpdatePriceLink(context, 'licenseDetails');

                //licenseSpan.setProperty('onclick') = licenseSpan.getProperty('onclick').replace(oldGuidLink, newGuidLink);
                //context.el.innerHTML = context.el.innerHTML.replace(oldGuidLink, newGuidLink);
            }
            else if (context.currentZone == 'Priced') {
                //console.log('in pricing');
                if (methodResults.status.contains('ContactUs')) {
                    //console.log('contact us');
                    context.effectivePriceStatus = 'ContactUs';
                    context.el.setProperty('effectivePriceStatus', 'ContactUs');
                    //console.log('UH OH!! Needs to ContactUs');
                    CorbisUI.Cart.UpdatePriceLink(context, 'priceTag', true);
                }
                // this is for bug 15125
                else if (methodResults.status.contains('UpdateUse')) {
                    //console.log('update da use');
                    context.effectivePriceStatus = 'UpdateUse';
                    context.el.setProperty('effectivePriceStatus', 'UpdateUse');
                    //console.log('UH OH!! Needs to UpdateUse');
                    CorbisUI.Cart.UpdatePriceLink(context, 'priceTag');
                }
                else {
                    //console.log('seems ok');
                    context.effectivePriceStatus = 'Ok';
                    context.el.setProperty('effectivePriceStatus', 'Ok');
                }
            }
        }
        else if (context.currentZone == 'Checkout') {
            adjustCheckoutCounters();
            //CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
        }
        adjustCounters();
        CorbisUI.Cart.BatchValidate.areAnyItemsCheckoutCapable();
    }
}
CorbisUI.Cart.LastActionUpdate = function(curZone, dropZone, corbisId) {
    CorbisUI.Cart.LastAction.CorbisId = corbisId;
    CorbisUI.Cart.LastAction.DragZone = curZone;
    CorbisUI.Cart.LastAction.DropZone = dropZone;
}
CorbisUI.Cart.CanContinue = function(controlId) {
    if (!GetProductToCheckout(controlId)) { return true; };
    if (CorbisUI.Auth.GetSignInLevel() < 2) {
        CorbisUI.Auth.Check(2, CorbisUI.Auth.ActionTypes.Execute, "$('" + controlId + "').click()");
        // If this doesn't return false, the user goes to the next page and then gets sent to the home page.
        return false;
    } else {
        return true;
    }
}

//need a better solution later
var ItemNotAvailableImage = 'http://cachens.corbis.com/pro/item_notavail_256.gif';

var methodFailed = function(results, context, methodName) {
    alert(results._message);
    //CorbisUI.MethodFailed(results, context, methodName);
}

function pageLoad() {
    Corbis.Web.UI.Checkout.CartScriptService.set_defaultFailedCallback(methodFailed);
    
    // this is for culture stuffs
    if (Sys != null && Sys.CultureInfo != null) {
        Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';
    }
}
function stripeCurrencyCode(fullStr) {
    var fullString = new String(fullStr);
    var space = fullString.lastIndexOf(' ');
    if (space == -1)
        return fullStr;
    return fullString.substr(0, space);

}

CorbisUI.Cart.AnonymousUserRedirect = function() {

    var data = JSON.decode("{\"runMe\":function() { CorbisUI.Auth.OpenSSLSignIn(\"execute\", \"window.location='" + CorbisUI.GlobalVars.Cart.cartUrl + "'\"); }}");
    //console.log(data);
    CorbisUI.CookieEvents.addCookieEvent_altPath("/", data.runMe);
    (function() { window.location = CorbisUI.GlobalVars.Cart.defaultUrl; }).delay(0);
}