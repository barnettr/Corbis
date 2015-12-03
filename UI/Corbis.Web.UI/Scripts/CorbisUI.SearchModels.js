/****************************
    NAMESPACES
****************************/
if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}


/////////////////////////////////////
// MODELS
/////////////////////////////////////
CorbisUI.SearchModels = {

    // product block model
    ProductBlock: new Class({
        Implements: [Options, Events],

        productBlock: null,

        LB: null,
        //QPon: null,
        //QPoff: null,
        QP: null,
        CT: null,

        activeStates: {
            LB: false,
            QP: false,
            CT: false
        },

        licenseModel: null,
        productUID: null,
        corbisID: null,
        isSelected: false,

        isOutline: false,

        QPenabled: false,
        CTenabled: false,

        basketCase: false,

        refreshPage: false,

        thumbWrap: null,

        isSetup: false,

        initialize: function(el) {
            if (!el) return false;
            this.productBlock = $(el);
            this.isSetup = false;

            var properties = this.productBlock.getProperties('licensemodel', 'productuid', 'corbisid', 'isoutline');
            //console.log(properties);
            this.licenseModel = properties.licensemodel;
            this.productUID = properties.productuid;
            this.corbisID = properties.corbisid;
            this.isOutline = properties.isoutline;

            // store objecet reference on product block
            //this.productBlock.store('objectReference', this);
            CorbisUI.ProductCache.addObject(this.productUID, this);
            CorbisUI.ProductCache.addObject(this.corbisID, this.productUID);

            //console.log('IN CART? '+CorbisUI.cartMediaUidList.has(this.productUID));
            if (CorbisUI.cartMediaUidList.has(this.productUID)) this.updateIcon('CT', 'selectIcon');

            // add test click stuff
            // TODO: remove later
            if (Browser.Engine.trident) {
                return false;
            } else {
            //this.productBlock.getElement('div.LT').addEvent('click', this.test.bindWithEvent(this));
            }
        },

        // this is to give a method to setup an object on demand
        setupObject: function() {
            if (!this.isSetup) {
                this.thumbWrap = this.productBlock.getElement('.thumbWrap');

                this.isSelected = this.productBlock.hasClass('ProductSelected');

                this.QPenabled = (this.productBlock.getElement('.ICN_quickpic') == null) ? false : true;
                this.CTenabled = (this.productBlock.getElement('li[class^=ICN_cart]') == null) ? false : true;

                this.LB = this.productBlock.getElement('.ICN_lightbox');

                this.activeStates.LB = (this.LB == null) ? false : this.LB.hasClass('ICN_lightbox_selected');

                if (this.QPenabled) {
                    /*this.QPon = this.productBlock.getElement('.QP_on');
                    this.QPoff = this.productBlock.getElement('.QP_off');
                    this.activeStates.QP = this.QPon.hasClass('ICN_quickpic_selected');*/

                    this.QP = this.productBlock.getElement('.ICN_quickpic');
                    this.activeStates.QP = this.QP.hasClass('ICN_quickpic_selected');
                }

                if (this.CTenabled) {
                    this.CT = this.productBlock.getElement('li[class^=ICN_cart]');
                    this.basketCase = this.CT.hasClass('ICN_cartBasket');

                    if (this.basketCase) {
                        this.activeStates.CT = this.CT.hasClass('ICN_cartBasket_selected');
                    } else {
                        this.activeStates.CT = this.CT.hasClass('ICN_cart_selected');
                    }

                }
                this.isSetup = true;
            }
            return this;
        },

        // quick way to check lightbox status
        checkLightboxStatus: function() {
            var status = (this.isSetup) ? this.activeStates.LB : this.productBlock.getElement('.ICN_lightbox').hasClass('ICN_lightbox_selected');
            return status;
        },


        // TODO: remove later
        test: function(event) {
            this.setupObject();
//            console.log('+=== ITEM INFORMATION ===================================================+');

//            console.log('     license: ' + this.licenseModel);
//            console.log('     corbisID: ' + this.corbisID);
//            console.log('     productUID: ' + this.productUID);
//            console.log('     mediaUID: ' + this.mediaUID);
//            console.log('     isSelected: ' + this.isSelected);
//            console.log('     isOutline: ' + this.isOutline);
//            console.log('     QPenabled: ' + this.QPenabled);
//            console.log('     CTenabled: ' + this.CTenabled);
//            console.log('     basketCase: ' + this.basketCase);
//            console.log('     activeStates: ' + this.activeStates.toSource());
        },

        updateIcon: function(type, action) {
            this.setupObject();
            //console.log(type+' : '+action);
            this[action](type);
        },

        selectIcon: function(type) {
            switch (type) {
                case "LB":
                    //console.log(this.LB.hasClass('ICN_lightbox_selected'));
                    if (!this.LB.hasClass('ICN_lightbox_selected')) this.LB.toggleClass('ICN_lightbox_selected');
                    this.activeStates.LB = true;
                    break;
                case "QP":
                    if (this.QPenabled) {
                        //if (!this.QPon.hasClass('ICN_quickpic_selected')) this.QPon.addClass('ICN_quickpic_selected');
                        //if (!this.QPoff.hasClass('ICN_quickpic_selected')) this.QPoff.addClass('ICN_quickpic_selected');
                        if (!this.QP.hasClass('QP_on')) this.QP.addClass('QP_on');
                        if (this.QP.hasClass('QP_off')) this.QP.removeClass('QP_off');
                        if (!this.QP.hasClass('ICN_quickpic_selected')) this.QP.addClass('ICN_quickpic_selected');
                        var image = this.QP.getElement('img[id$=qpIcon]');
                        if (image) {
                            image.setProperties({
                                'title': CorbisUI.GlobalVars.SearchResults.text.removeQuickpicAlt,
                                'alt': CorbisUI.GlobalVars.SearchResults.text.removeQuickpicAlt
                            });
                        }
                        this.activeStates.QP = true;
                    }
                    break;
                case "CT":
                    if (this.CTenabled) {
                        if (this.basketCase) {
                            if (!this.CT.hasClass('ICN_cartBasket_selected')) this.CT.addClass('ICN_cartBasket_selected');
                        } else {
                            if (!this.CT.hasClass('ICN_cart_selected')) this.CT.addClass('ICN_cart_selected');
                        }
                        this.activeStates.CT = true;
                        CorbisUI.cartMediaUidList.set(this.productUID, true);
                    }
                    break;
            }
            // update productblock glow
            this.highlightBlock();
        },

        deselectIcon: function(type) {
            //console.log('deselectIcon(\''+type+'\')');
            switch (type) {
                case "LB":
                    if (this.LB.hasClass('ICN_lightbox_selected')) this.LB.toggleClass('ICN_lightbox_selected');
                    this.activeStates.LB = false;
                    break;
                case "QP":
                    if (this.QPenabled) {
                        //if (this.QPon.hasClass('ICN_quickpic_selected')) this.QPon.toggleClass('ICN_quickpic_selected');
                        //if (this.QPoff.hasClass('ICN_quickpic_selected')) this.QPoff.toggleClass('ICN_quickpic_selected');
                        if (this.QP.hasClass('QP_on')) this.QP.removeClass('QP_on');
                        if (!this.QP.hasClass('QP_off')) this.QP.addClass('QP_off');
                        if (this.QP.hasClass('ICN_quickpic_selected')) this.QP.removeClass('ICN_quickpic_selected');
                        var image = this.QP.getElement('img[id$=qpIcon]');
                        if (image) {
                            image.setProperties({
                                'title': CorbisUI.GlobalVars.SearchResults.text.addQuickpicAlt,
                                'alt': CorbisUI.GlobalVars.SearchResults.text.addQuickpicAlt
                            });
                        }
                        this.activeStates.QP = false;
                    }
                    break;
                case "CT":
                    if (this.CTenabled) {
                        if (this.basketCase) {
                            if (this.CT.hasClass('ICN_cartBasket_selected')) this.CT.toggleClass('ICN_cartBasket_selected');
                        } else {
                            if (this.CT.hasClass('ICN_cart_selected')) this.CT.toggleClass('ICN_cart_selected');
                        }
                        this.activeStates.CT = false;
                    }
                    break;
            }
            // update productblock glow
            this.unhighlightBlock();
        },

        highlightBlock: function() {
            if (this.activeStates.LB
                || (this.QPenabled && this.activeStates.QP)
                || (this.CTenabled && this.activeStates.CT)) {
                if (!this.productBlock.hasClass('ProductSelected')) this.productBlock.toggleClass('ProductSelected');
            }
        },

        unhighlightBlock: function() {
            if (!this.activeStates.LB
                && !this.activeStates.QP
                && !this.activeStates.CT) {
                if (this.productBlock.hasClass('ProductSelected')) this.productBlock.toggleClass('ProductSelected');
            }
        },

        refreshObject: function() {
            // the whole purpose of this method is
            // because update panel messes our scripts
            // up by redrawing the dom when we have 
            // JS object attached to certain dom elements

            var item = CorbisUI.DomCache.get('ProductResults').getElement('span[productuid=' + this.productUID + ']');
            if (item) {
                // reattach this to the current dom object
                this.initialize(item);
                // reattach tooltips - stupid update panel
                this.setupObject();
            }
            //(function(){registerTooltips(false);}).delay(200);
            return this;
        }


    }),

    /* model for lightbox item */

    lightboxBlock: new Class({

        Implements: [CorbisUI.SearchConstructors.pinkyBlockConstructor, Options, Events],

        constructorType: 'lightbox',

        options: {},

        tempObject: {},

        initialize: function(mediaUID, corbisID, options, realProductUid) {
            //$extend(this,CorbisUI.Search.pinkyBlockConstructor);
            //debugger;
            //console.log($type(mediaUID));
            // this first item is to detect if its supposed
            // to create a new block from the lazy loader
            if ($type(mediaUID) == 'object') {

                // console.log(tempObject);
                this.tempObject = mediaUID;
                this.mediaUID = this.tempObject.MediaUid;
                this.corbisID = this.tempObject.CorbisId;

                this.createBlock(true, 'lightbox'); // true, that it is new and not clone

                // else this creates a block when adding to lightbox
                // from search results
            } else {
                if (options) this.setOptions(options);

                this.mediaUID = mediaUID;
                this.corbisID = corbisID;
                this.realProductUID = realProductUid;
                this.createBlock(false, 'lightbox'); // false, because it is cloning instead of creating

                // setup block here
            }

            if (this.isInCart()) this.block.addClass('inCart');
            if (isQuickCheckoutEnabled && !this.isOutline && !this.isRfcd) {
                var expressIconType = CorbisUI.GlobalVars.SearchResults.isECommerceEnabled ? (CorbisUI.GlobalVars.SearchResults.isBasket ? 'lightboxExpressBasket' : 'lightboxExpressCheckout') : '';
                var quickCheckoutAnchor = new Element('a')
                    .setProperties({
                        alt: CorbisUI.GlobalVars.SearchResults.text.expressCheckoutAlt,
                        title: CorbisUI.GlobalVars.SearchResults.text.expressCheckoutAlt
                    })
                    .addClass(CorbisUI.GlobalVars.SearchResults.isBasket ? 'lightboxExpressBasket' : 'lightboxExpressCheckout')
                    .addEvent('click', this.expressCheckoutButtonEvent.bindWithEvent(this))
                    .inject(this.licenseBlock);

                this.quickCheckoutButton = quickCheckoutAnchor;
            }
            // need to attach event to this
            var cartIconType = CorbisUI.GlobalVars.SearchResults.isECommerceEnabled ? (CorbisUI.GlobalVars.SearchResults.isBasket ? 'lightboxBasket' : 'lightboxCart') : '';
            var cartAnchor = new Element('a')
                .setProperties({
                    alt: CorbisUI.GlobalVars.SearchResults.text.addToCartAlt,
                    title: CorbisUI.GlobalVars.SearchResults.text.addToCartAlt
                })
                .addClass(cartIconType)
                .addEvent('click', this.cartButtonEvent.bindWithEvent(this))
                .inject(this.licenseBlock);

            //var cartIcon = new Element('div').addClass("ICN_cart").inject(cartAnchor);
            this.cartButton = cartAnchor;

            // add delete and cart events here
            if (CorbisUI.ProductCache.has(this.mediaUID)) CorbisUI.ProductCache.get(this.mediaUID).updateIcon('LB', 'selectIcon');

            //inject it into the lightbox pane
            CorbisUI.DomCache.get('LBXContainer', true).grab(this.block, this.injectWhere);
        },

        // events
        deleteButtonEvent: function() {
            //var lightboxId = CorbisUI.DomCache.get('SBBX_lightboxes').getElement('select.lightboxList');
            var lightboxId = CorbisUI.DomCache.get('SBBX_lightboxes').getElement('select.lightboxList').getSelected()[0].value;
            Corbis.Web.UI.Search.SearchScriptService.DeleteProductFromLightbox(lightboxId, this.realProductUID);
            if (CorbisUI.ProductCache.has(this.mediaUID)) CorbisUI.ProductCache.get(this.mediaUID).updateIcon('LB', 'deselectIcon');

            //decrement image count for lightbox
            var imageCountProperty = CorbisUI.DomCache.get('LBXContainer').getProperty('imageCount');
            var imageCount = parseInt(imageCountProperty);
            if (!isNaN(imageCount)) {
                imageCount -= 1;
            }
            else {
                //item already delete, so don't need to decrement.
                imageCount = CorbisUI.DomCache.get('LBXContainer').getElements('div.lightboxBlock').length;
            }
            CorbisUI.DomCache.get('LBXContainer').setProperty('imageCount', imageCount.toString());

            //show empty lighbox message if no images
            if (imageCount == 0 && CorbisUI.DomCache.get('LBXContainer').getElement('.centerMe') != null) {
                CorbisUI.DomCache.get('LBXContainer').getElement('.centerMe').setStyle('display', 'block');
            }

            this.block.destroy();
            //$lambda(false);
            // now delete this item

        },

        cartButtonEvent: function() {
            if (!this.isInCart()) {
                Corbis.Web.UI.Search.SearchScriptService.AddProductToCart(this.mediaUID, this.realProductUID);
                //adjust cart counter here
                var cartCounter = CorbisUI.DomCache.get('cartCount');
                if (cartCounter) {
                    var count = cartCounter.getProperty('text');
                    count++;
                    cartCounter.setProperty('text', count);
                    fixIECheckoutWidgetWidth();
                }
                //highligh the control to show it is in the cart
                this.highlightIcon('cart');
                //add mediaUID to the list
                //cartMediaUidList.include(this.mediaUID);
                CorbisUI.cartMediaUidList.set(this.mediaUID, true);
                if (CorbisUI.ProductCache.has(this.mediaUID)) {
                    CorbisUI.ProductCache.get(this.mediaUID).updateIcon('CT', 'selectIcon');
                }

            }
        },
        expressCheckoutButtonEvent: function() {
            this.lightboxId = $(document.body).getElement('select[name$=lightboxList]').getSelected()[0].value;
            CorbisUI.ExpressCheckout.Open(this.corbisID, this.realProductUID, this.lightboxId);
            //return false;
        }

    }),

    /* model for quickpic item */
    quickPicBlock: new Class({

        Implements: [CorbisUI.SearchConstructors.pinkyBlockConstructor, Options, Events],

        constructorType: 'quickPic',

        options: {},

        initialize: function(mediaUID, corbisID, options) {
            //$extend(this,CorbisUI.Search.pinkyBlockConstructor);

            if (options) this.setOptions(options);

            if ($type(mediaUID) == 'object') {
                this.tempObject = mediaUID;
                this.mediaUID = this.tempObject.MediaUid;
                this.corbisID = this.tempObject.CorbisId;
                this.createBlock(true, 'quickpic'); // true, that it is new and not clone

                // else this creates a block when adding to lightbox
                // from search results
            } else {
                if (options) this.setOptions(options);

                this.mediaUID = mediaUID;
                this.corbisID = corbisID;

                this.createBlock(false, 'quickpic'); // false, because it is cloning instead of creating

                // setup block here
            }

            // add delete and cart events here
            if (CorbisUI.ProductCache.has(this.mediaUID)) CorbisUI.ProductCache.get(this.mediaUID).updateIcon('QP', 'selectIcon');

            var downloadAll = $('SBBX_quickpic').getElement('span[id$=quickPicDownloadAllLink]');
            CorbisUI.DomCache.addObject('quickPicDownloadAll', downloadAll);
            // setup block here

            //inject it into the lightbox pane
            var target = CorbisUI.DomCache.get('quickPicsContainer').getFirst();
            //.getFirst()
            //.getElement('.centerMe');

            this.block.inject(target, this.injectWhere);
        },

        // events
        deleteButtonEvent: function() {
            CorbisUI.Handlers.Quickpic.deleteItem(this.corbisID);
        }

    })

};