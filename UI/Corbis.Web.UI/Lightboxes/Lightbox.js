/*

                                .do-"""""'-o..                         
  GREETINGS HUMAN!           .o""            ""..                       
  DO YOU KNOW              ,,''                 ``b.                   
  JAVASCRIPT?             d'                      ``b                   
                         d`d:                       `b.                 
                        ,,dP                         `Y.               
                       d`88                           `8.               
 ooooooooooooooooood888`88'                            `88888888888bo, 
d"""    `""""""""""""Y:d8P                              8,          `b 
8                    P,88b                             ,`8           8 
8                   ::d888,                           ,8:8.          8 
:                   dY88888                           `' ::          8 
:                   8:8888                               `b          8 
:                   Pd88P',...                     ,d888o.8          8 
:                   :88'dd888888o.                d8888`88:          8 
:                  ,:Y:d8888888888b             ,d88888:88:          8 
:                  :::b88d888888888b.          ,d888888bY8b          8 
                    b:P8;888888888888.        ,88888888888P          8 
                    8:b88888888888888:        888888888888'          8 
                    8:8.8888888888888:        Y8888888888P           8 
,                   YP88d8888888888P'          ""888888"Y            8 
:                   :bY8888P"""""''                     :            8 
:                    8'8888'                            d            8 
:                    :bY888,                           ,P            8 
:                     Y,8888           d.  ,-         ,8'            8 
:                     `8)888:           '            ,P'             8 
:                      `88888.          ,...        ,P               8 
:                       `Y8888,       ,888888o     ,P                8 
:                         Y888b      ,88888888    ,P'                8 
:                          `888b    ,888888888   ,,'                 8 
:                           `Y88b  dPY888888OP   :'                  8 
:                             :88.,'.   `' `8P-"b.                   8 
:.                             )8P,   ,b '  -   ``b                  8 
::                            :':   d,'d`b, .  - ,db                 8 
::                            `b. dP' d8':      d88'                 8 
::                             '8P" d8P' 8 -  d88P'                  8 
::                            d,' ,d8'  ''  dd88'                    8 
::                           d'   8P'  d' dd88'8                     8 
 :                          ,:   `'   d:ddO8P' `b.                   8 
 :                  ,dooood88: ,    ,d8888""    ```b.                8 
 :               .o8"'""""""Y8.b    8 `"''    .o'  `"""ob.           8 
 :              dP'         `8:     K       dP''        "`Yo.        8 
 :             dP            88     8b.   ,d'              ``b       8 
 :             8.            8P     8""'  `"                 :.      8 
 :            :8:           :8'    ,:                        ::      8 
 :            :8:           d:    d'                         ::      8 
 :            :8:          dP   ,,'                          ::      8 
 :            `8:     :b  dP   ,,                            ::      8 
 :            ,8b     :8 dP   ,,                             d       8 
 :            :8P     :8dP    d'                       d     8       8 
 :            :8:     d8P    d'                      d88    :P       8 
 :            d8'    ,88'   ,P                     ,d888    d'       8 
 :            88     dP'   ,P                      d8888b   8        8 
 '           ,8:   ,dP'    8.                     d8''88'  :8        8 
             :8   d8P'    d88b                   d"'  88   :8        8 
             d: ,d8P'    ,8P""".                      88   :P        8 
             8 ,88P'     d'                           88   ::        8 
            ,8 d8P       8                            88   ::        8 
            d: 8P       ,:  -hrr-                    :88   ::        8 
            8',8:,d     d'                           :8:   ::        8 
           ,8,8P'8'    ,8                            :8'   ::        8 
           :8`' d'     d'                            :8    ::        8 
           `8  ,P     :8                             :8:   ::        8 
            8, `      d8.                            :8:   8:        8 
            :8       d88:                            d8:   8         8 
 ,          `8,     d8888                            88b   8         8 
 :           88   ,d::888                            888   Y:        8 
 :           YK,oo8P :888                            888.  `b        8 
 :           `8888P  :888:                          ,888:   Y,       8 
 :            ``'"   `888b                          :888:   `b       8 
 :                    8888                           888:    ::      8 
 :                    8888:                          888b     Y.     8, 
 :                    8888b                          :888     `b     8: 
 :                    88888.                         `888,     Y     8: 
 ``ob...............--"""""'----------------------`""""""""'"""`'"""""


*/
/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Lightbox = {};

/***************************
    MODELS
****************************/

//COFF Model
var fileSizeTemplateString = '';
var coffImages = null;
CorbisUI.Lightbox.CoffImages = function() {
    if (!coffImages) {
        coffImages = new CorbisUI.Lightbox.COFFSelectableImages('coffProduct', 'li', $('coffProduct').getElement('a.selectAllLink'), new Array($('coffProduct').getElement('div.GlassButton')));
    }
    return coffImages;
}

CorbisUI.Lightbox.ProductUtil = new Class({
    isRFCD: function(image) {
        //Check if the image is of RFCD type
        return image != null && image.LicenseModel == 3;
    },
    isValidCOFFItem: function(image) {
        //return true only if the validation status is InvalidRFSize
        return image.CoffValidationResult.ValidationStatus == 3;
    }
});

/**
TODO: Refactor. Merge with copy functionality to work independent of containers with more configuration support by context
*/
CorbisUI.Lightbox.COFFSelectableImages = new Class({

    Implements: CorbisUI.Lightbox.SelectableImages,
    selectableImages: null,
    imagesFailedValidationCount: 0,
    imagesFailedValidation: null,
    productUtil: null,
    noneCanBeCheckedOut: false,

    initialize: function(imageContainer, imageTagSpecifier, selectAllControl, linkedControls) {
        this.selectableImages = new CorbisUI.Lightbox.SelectableImages(imageContainer, imageTagSpecifier, selectAllControl, linkedControls);
        this.linkedControls = this.selectableImages.linkedControls;
        this.productUtil = new CorbisUI.Lightbox.ProductUtil();
        this.updateLinkedControls();
    },
    RegisterToolTips: function() {
        var usageButton = $('showFileSizeModalSpan');
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
        new Tips('#ctl00_mainContent_invalidCoffProducts_showFileSizeModal', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            offsets: { x: 5, y: -20 },
            className: 'TIP-license-details mochaContent FileSize-ToolTip',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    },
    getUtil: function() {
        return this.productUtil;
    },
    getImages: function() {
        return this.selectableImages.getImages();
    },
    HideModal: function(modalId) {
        this.closeLightbox(modalId);
    },
    closeLightbox: function(modalId) {
        this.deselectAll();
        HideModal(modalId);
        $(CorbisUI.GlobalVars.Lightbox.coffItemsButtonDiv).removeClass('selected');
    },
    getSelectedImages: function() {
        return this.selectableImages.getSelectedImages();
    },
    updateLinkedControls: function() {
        this.setLinkedControls();
        fixIeDisplay();
        this.updateSelectionCount();
    },
    updateSelectionCount: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
        if (activeLightbox) {
            var totalItems = $('header2').getElement('input.totalItems').value;
            var selectedImages = this.selectableImages.getSelectedImages();
            var countStr = String.format(CorbisUI.GlobalVars.Lightbox.coffItemCountTemplateString, selectedImages.length, totalItems);
            $('coffProduct').getElement('div.coffSelectedItemCount').getElement('span').innerHTML = countStr;
        }
    },
    selectItem: function(e, item) {
        this.selectableImages.selectItem(e, item);
        this.updateLinkedControls();
    },
    lightboxSelected: function() {
        return true;
    },
    checkoutCoffItems: function() {
        CorbisUI.Auth.Check(2, CorbisUI.Auth.ActionTypes.Execute, "(function(){CorbisUI.Lightbox.CoffImages().checkoutCoffItemsAuthenticated(); return false;}).run();");
    },
    checkoutCoffItemsAuthenticated: function() {
        HideModal('invalidCoffProducts');
        if (CorbisUI.Lightbox.CoffImages().imagesFailedValidationCount > 0) {
            //Checkout the coff items both the valid ones that are stored in session and the invalid ones that
            //are displayed to the user ( except RFCD's as they are not valid COFF items)
            var imagesFailedValidation = CorbisUI.Lightbox.CoffImages().imagesFailedValidation;
            //Remove the RFCD items from the list
            var coffImages = this;
            var imageList = new Array();
            var index = 0;
            var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
            for (var i = 0; i < this.imagesFailedValidationCount; i++) {
                /**
                For each valid image create a QuickPicOrderImage and submit for checkout
                */
                var el = this.imagesFailedValidation[i];
                if (this.getUtil().isValidCOFFItem(el)) {
                    var lightboxItemContainer = $('invalidCoffItems');
                    var image = new Corbis.Web.Entities.COFFOrderImage();
                    var dropDown = lightboxItemContainer.getElement('select')
                    image.FileSize = dropDown.options[dropDown.selectedIndex].value;
                    image.ImageUid = el.CoffValidationResult.MediaUid;
                    image.CorbisId = el.CoffValidationResult.CorbisId;
                    image.ProductUid = el.CoffValidationResult.ProductUid;
                    imageList[index] = image;
                    index++;
                }
            }
            var serviceCall = Corbis.Web.UI.Lightboxes.LightboxScriptService._staticInstance.ContinueToCheckoutCOFFItems(activeLightbox.get('id'), imageList, CorbisUI.Lightbox.CoffImages().checkoutSucceeded, CorbisUI.Lightbox.CoffImages().checkoutFailed);
            executer = serviceCall.get_executor();
        }
    },
    checkoutSucceeded: function(result) {
        window.location.href = "../" + result.CheckoutUrl;
    },
    checkoutFailed: function() {
    },
    setLinkedControls: function() {
        if (this.linkedControls && this.linkedControls.length > 0) {
            for (var i = 0; i < this.linkedControls.length; i++) {
                var control = this.linkedControls[i];
                if (control.hasClass('GlassButton')) {
                    setGlassButtonDisabled(control, (this.selectableImages.noImageSelected));
                }
                else {
                    if (this.selectableImages.noImageSelected) {
                        if (!control.hasClass('disabled')) control.addClass('disabled');
                    }
                    else {
                        control.removeClass('disabled');
                    }
                }
            };
        }
    },
    toggleSelectAll: function() {
        this.selectableImages.toggleSelectAll();
        this.updateLinkedControls();
    },
    selectAll: function() {
        this.selectableImages.selectAll();
        this.updateLinkedControls();
    },
    deselectAll: function() {
        this.selectableImages.deselectAll();
        this.updateLinkedControls();
    },
    validateSelectedItemsSucceeded: function(result) {
        if (!result.ValidationStatus) {
            return;
        }
        if (result.InvalidItemCount == 0) {
            window.location = result.CheckoutUrl;
            return;
        }
        CorbisUI.Lightbox.CoffImages().imagesFailedValidationCount += result.InvalidItemCount;
        CorbisUI.Lightbox.CoffImages().imagesFailedValidation = result.ValidatedCoffItem;
        CorbisUI.Lightbox.CoffImages().coffSelectedItemsValidationFailed(result, 0);
    },
    layoutImages: function(result, inst) {
        var selectstring = '<select class="{0}">{1}</select>';
        var optiontemplate = '<option value="{0}">{1}</option>';

        if (result) {
            var imagesString = "";
            var templatestring = '<li imageuid="{0}" productuid="{4}" {8}><div class="imageWrap"><img src="{1}" title="{2}" style="{3}"/></div><div class="displayWrap{5}"><span class="floatLeft LicenseModel{5}">{5}</span><span class="CorbisuidStyle">{6}</span></div><div class="{9}">{7}</div></li>';
            var optionsstring = "";
            var totalInvalid = 0;
            result.each(function(image) {
                var selectClass = "selectImageSizes";

                var cssClass = "";
                var containerHeight = 128;
                var containerWidth = 128;
                var width = containerWidth;
                var height = containerHeight;
                var marginTop = 0;
                var aspectRatio = image.AspectRatio;
                if (aspectRatio == 0) aspectRatio = 1;
                var marginBottom = 0;
                if (image.AspectRatio > 1) {
                    height = containerWidth / aspectRatio;
                    marginTop = (containerHeight - height) / 2;
                    marginBottom = marginTop;
                }
                else {
                    width = containerHeight * aspectRatio;
                }
                if (!inst.getUtil().isValidCOFFItem(image)) {
                    totalInvalid++;
                }
                var selectstr = "";
                if (image.AvailableSizes != null) {
                    for (var i = 0; i < image.AvailableSizes.length; i++) {
                        optionsstring += String.format(optiontemplate, image.AvailableSizes[i].Size, image.AvailableSizes[i].LocalizedValue, selectClass);
                    }
                    selectstr = String.format(selectstring, selectClass, optionsstring);
                } else {
                    selectClass = "invalidCoffItemMessage";
                    selectstr = $('InvalidCOFFItemMessage').get('html');
                    cssClass = "class=\"invalidCoffImageWrap\"";
                }
                var imageStyle = 'margin-top: ' + marginTop + 'px; width: ' + width + 'px; height: ' + height + 'px; margin-bottom:' + marginBottom + 'px;';
                imagesString += String.format(templatestring, image.CoffValidationResult.MediaUid, image.CoffValidationResult.Url128, image.CoffValidationResult.CorbisId + ' ' + HtmlEncode(image.CoffValidationResult.Title), imageStyle, image.CoffValidationResult.ProductUid, image.LicenseModelText, image.CoffValidationResult.CorbisId, selectstr, cssClass, selectClass);
                optionsstring = "";
            });

            return imagesString;
        }
        return "";
    },
    coffSelectedItemsValidationFailed: function(rst, context) {
        var allItemsInvalid = rst.AllItemsInvalid;
        var disabled = allItemsInvalid;
        var continueToCheckoutButton = $(CorbisUI.GlobalVars.Lightbox.continueToCheckoutCoffItemsButtonID);

        if (disabled) {
            if (continueToCheckoutButton && !continueToCheckoutButton.hasClass('disabled')) {
                continueToCheckoutButton.addClass('disabled');
                setGlassButtonDisabled(continueToCheckoutButton, true);
            }
        } else {
            if (continueToCheckoutButton && !continueToCheckoutButton.hasClass('enabled')) {
                continueToCheckoutButton.addClass('enabled');
                setGlassButtonDisabled(continueToCheckoutButton, false);
            }
        }

        var result = rst.ValidatedCoffItem;
        //Create the validation failed modal popup with the items that failed validation
        var imagesContainer = $('invalidCoffItems');
        CorbisUI.GlobalVars.Lightbox.coffItemsPagesDownloaded = 0;
        var totalItems = CorbisUI.Lightbox.CoffImages().imagesFailedValidationCount;
        var pageHeight = CorbisUI.GlobalVars.Lightbox.coffItemPageSize / 5 * 110;
        var noOfPages = Math.floor(totalItems / CorbisUI.GlobalVars.Lightbox.coffItemPageSize);
        var lastPageImageCount = totalItems - (noOfPages * CorbisUI.GlobalVars.Lightbox.coffItemPageSize);
        var lastPageHeight = 0;
        var pageNumber = 0;

        if (lastPageImageCount > 0) {
            noOfPages++;
            lastPageHeight = Math.ceil(lastPageImageCount / 5) * 110
        }

        imagesContainer.scrollTo(0, 0);
        var imagesPagesString = "";
        CorbisUI.GlobalVars.Lightbox.coffItemsPages = noOfPages;

        //Creating the blank page lists first
        noOfPages.times(function(index) {
            imagesPagesString += String.format('<ul page="{0}" style="height:{1}px;"></ul>', pageNumber, pageNumber + 1 == noOfPages && lastPageHeight > 0 ? lastPageHeight : pageHeight);
            pageNumber++;
        });
        noOfPages = 1;
        imagesContainer.set('html', imagesPagesString);
        var lighboxImages = $('invalidCoffItems');
        var imageThumbnailContainer = lighboxImages.getElement('ul[page=' + context + ']');
        imageThumbnailContainer.innerHTML = this.layoutImages(result, this);

        CorbisUI.Lightbox.CoffImages().HideModal('coffProduct');
        //Show the modal
        var coffProductModal = new CorbisUI.Popup('invalidCoffProducts', {
            createFromHTML: false,
            showModalBackground: true,
            closeOnLoseFocus: false,
            positionVert: 'center',
            positionHoriz: 'center',
            width: 850,
            height: 512,
            onHide: function() { }
        });
    },
    resetSelection: function() {
        this.deselectAll();
        this.selectableImages.changeToSelectAll();
    },
    resetLocals: function() {
        CorbisUI.Lightbox.CoffImages().imagesFailedValidationCount = 0;
        CorbisUI.Lightbox.CoffImages().imagesFailedValidation = null;
    },

    OpenFileSizeModal: function(event) {
        if (fileSizeTemplateString == "") {
            var fileSizeModal = $('fileSizeModalWrap').clone(true, true);
            fileSizeModal.set('html', fileSizeModal.get('html').replace(new RegExp('HideModal', 'g'), 'CloseModal'));
            fileSizeModal.getElement('div').setStyle('display', 'block');
            fileSizeTemplateString = fileSizeModal.get('html');
        }

        var bodyClick = function(event) {
            if ($(parent.document).getElement('div#fileSizeModalWindow') && event.target.id != 'showFileSizeModal') parent.MochaUI.CloseModal('fileSizeModal');
        }
        //alert(event.clientX + "::" + event.clientY);
        var yy = event.clientY - 50;
        var properties = {
            height: 175,
            width: 300,
            onClose: function() { $(document).removeEvent('click', bodyClick); }
        };
        parent.OpenHtmlModal('fileSizeModal', fileSizeTemplateString, properties)

        //add the required stylesheet.
        var css = new parent.Element('link', {
            id: 'fileSizeCss',
            rel: 'stylesheet',
            type: 'text/css',
            href: '../StyleSheets/MyLightboxes.css'
        });

        var parentDocument = $(parent.document);
        if (!parentDocument.getElement('head').getElement('link[id=fileSizeCss]')) {
            css.inject(parentDocument.getElement('head'));
        }

        //Setting close on click of background
        parentDocument.getElement('div#modalOverlay').addEvent('click', function() { parent.MochaUI.CloseModal('fileSizeModal'); });
        $(document).addEvent('click', bodyClick);
    },
    validateSelectedItemsForCoff: function() {
        CorbisUI.Auth.Check(2, CorbisUI.Auth.ActionTypes.Execute, "(function(){CorbisUI.Lightbox.CoffImages().validateSelectedItemsForCoffAuthenticated(); return false;}).run();");
    },
    validateSelectedItemsForCoffAuthenticated: function() {

        var selectedImages = this.selectableImages.getSelectedImages();
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');

        if (activeLightbox && selectedImages.length > 0) {
            var imageUids = new Array(selectedImages.length);
            selectedImages.each(function(el, index) {
                imageUids[index] = el.getProperty('productuid');
            });

            HideModal('coffProduct');
            this.imagesCopied = 0;
            this.imageCopyCallCount = Math.ceil(imageUids.length / CorbisUI.GlobalVars.Lightbox.maxImageCOFFPerCall);
            this.imagesToCopy = imageUids
            CorbisUI.Lightbox.CoffImages().resetLocals();
            for (i = 0; i < this.imageCopyCallCount; i++) {
                var startIndex = i * CorbisUI.GlobalVars.Lightbox.maxImageCOFFPerCall;
                var endIndex = ((i + 1) * CorbisUI.GlobalVars.Lightbox.maxImageCOFFPerCall);
                var imagesToCopyText = i == (this.imageCopyCallCount - 1) ? String.format('CorbisUI.Lightbox.CoffImages().imagesToCopy.slice({0})', startIndex.toString()) : String.format('CorbisUI.Lightbox.CoffImages().imagesToCopy.slice({0}, {1})', startIndex.toString(), endIndex.toString());
                setTimeout(String.format('Corbis.Web.UI.Lightboxes.LightboxScriptService.ValidateItemsForCOFF({0}, {1}, CorbisUI.Lightbox.CoffImages().validateSelectedItemsSucceeded, CorbisUI.Lightbox.Handler.methodFailed);', activeLightbox.get('id'), imagesToCopyText), 0);
            }
        }
    }
});

// product block model
// TODO: clean up variable naming. There are some BIG incosistencies with how we name variables.
CorbisUI.Lightbox.ProductBlock = new Class({
    Extends: CorbisUI.SearchModels.ProductBlock,

    PR: null,
    noteUid: null,
    noteText: null,

    priceStatus: null,
    price: null,
    isSetup: false,

    // TODO: need to clean up.
    // two of these because of coding inconsistencies in variable naming.
    corbisID: null,
    corbisId: null,

    productUID: null,
    productUid: null,

    mediaUID: null,
    mediaUid: null,

    isValid: true,

    initialize: function(corbisId) {
        if (corbisId) {
            if (typeof corbisId == 'string') {
                this.productBlock = $('LightboxProducts').getElement('span[corbisid=' + corbisId + ']');
            }
            else {
                this.productBlock = $(corbisId);
            }

            if (!this.productBlock) {
                this.isValid = false;
                return;
            }
        }
        else {
            this.isValid = false;
            return;
        }

        var properties = this.productBlock.getProperties('licensemodel', 'productuid', 'mediauid');

        this.licenseModel = properties.licensemodel;

        // We really need to work on variable naming conventions.
        this.productUID = properties.productuid;
        this.productUid = this.productUID;
        this.mediaUID = properties.mediauid;
        this.mediaUid = this.mediaUID;
        this.corbisID = corbisId;
        this.corbisId = corbisId;

        // store objecet reference on product block
        //this.productBlock.store('objectReference', this);
        CorbisUI.ProductCache.addObject(this.productUID, this);
        CorbisUI.ProductCache.addObject(this.corbisID, this.productUID);

        //console.log('IN CART? '+CorbisUI.cartMediaUidList.has(this.productUID));
        if (CorbisUI.cartMediaUidList.has(this.productUID)) this.updateIcon('CT', 'selectIcon');

        this.thumbWrap = this.productBlock.getElement('.thumbWrap');

        this.isSelected = this.productBlock.hasClass('ProductSelected');

        this.QPenabled = (this.productBlock.getElement('.ICN_quickpic') == null) ? false : true;
        this.CTenabled = (this.productBlock.getElement('li[class^=ICN_cart]') == null) ? false : true;

        if (this.QPenabled) {
            this.QP = this.productBlock.getElement('.ICN_quickpic');
            this.activeStates.QP = this.QP.hasClass('ICN_quickpic_selected');
        }

        if (this.CTenabled) {
            this.CT = this.productBlock.getElement('li[class^=ICN_cart]');

            this.activeStates.CT = this.CT.getProperty('class').contains('_selected');
        }

        //console.log(this.productBlock);

        var noteContent = this.productBlock.getElement('div.noteContent');
        this.noteUid = noteContent.getProperty('noteuid');
        this.noteText = noteContent.innerHTML;

        // add test click stuff
        this.productBlock.getElement('div.LT').addEvent('click', this.test.bindWithEvent(this));

        this.isSetup = true;
    },

    updateNote: function(noteText) {
        this.noteText = noteText;
        Corbis.Web.UI.Lightboxes.LightboxScriptService.set_defaultUserContext(this);
        Corbis.Web.UI.Lightboxes.LightboxScriptService.UpdateLightboxProductNote(this.productUID, this.noteUid, noteText, this.refreshNote, CorbisUI.Lightbox.Handler.methodFailed);

        HideModal('modalAddEditNoteTemplate');
    },

    deleteNote: function() {
        Corbis.Web.UI.Lightboxes.LightboxScriptService.set_defaultUserContext(this);
        Corbis.Web.UI.Lightboxes.LightboxScriptService.DeleteLightboxProductNote(this.productUID, this.refreshNote, CorbisUI.Lightbox.Handler.methodFailed);

        HideModal('modalDeleteNoteTemplate');
    },

    refreshNote: function(results, context, methodName) {
        var noteContent = context.productBlock.getElement('div.noteContent');
        var noteBlock = context.productBlock.getElement('div.note');
        var noteIcon = context.productBlock.getElement('div.noteIcon');

        if (methodName == 'UpdateLightboxProductNote') {
            noteContent.setProperty('noteUid', results);
            noteContent.set('html', HtmlEncodeTextArea(context.noteText));
            noteBlock.removeClass('hdn');
            noteIcon.setProperty('title', editNoteTooltip);
        }

        if (methodName == 'DeleteLightboxProductNote') {
            noteContent.setProperty('noteUid', '00000000-0000-0000-0000-000000000000');
            noteContent.set('text', '');
            if (!noteBlock.hasClass('hdn')) noteBlock.addClass('hdn');
            noteIcon.setProperty('title', addNoteTooltip);
        }
    },

    addProductToCart: function() {
        if (!this.activeStates.CT) {
            var addToCart = new CorbisUI.Cart.AddToCart(this.mediaUID);
            addToCart.context = this;
            addToCart.onSuccess = this.refreshCartItem;
            addToCart.addProductToCart(this.corbisID, this.productUID);
        }
    },

    addRfcdToCart: function() {
        if (!this.activeStates.CT) {
            var addToCart = new CorbisUI.Cart.AddToCart(this.mediaUID);
            addToCart.context = this;
            addToCart.onSuccess = this.refreshCartItem;
            addToCart.addOfferingToCart();
        }
    },

    refreshCartItem: function(results) {
        //Update thumbnail
        if (this.context) {
            this.context.selectIcon('CT');
        }
        else {
            this.selectIcon('CT');
        }

        //update cart count
        UpdateCartCount(results);
    },

    addToQuicPick: function(icon, Url128, licenseModel, aspectRatio, title) {
        CorbisUI.Handlers.Quickpic.moveQuickpic(icon, this.corbisID, Url128, licenseModel, aspectRatio, title);
    },

    deleteQuickPic: function(obj) {
        //console.log('CALLING: CorbisUI.Lightbox.ProductBlock.deleteQuickPic');
        CorbisUI.Handlers.Quickpic.moveQuickpic(this.corbisID);
    }
});


//Static object for copy image
var copyImages = null;
CorbisUI.Lightbox.CopyImages = function()
{
	if (!copyImages)
	{
		copyImages = new CorbisUI.Lightbox.SelectableImages('copyProduct', 'li', $('copyProduct').getElement('a.selectAllLink'), new Array($('copyProduct').getElement('div.copyItemsModalButton')));
	}
	
	return copyImages;
};

CorbisUI.Lightbox.SelectableImages = new Class({
    Implements: [Options, Events],

    imageContainer: null,
    imageTagSpecifier: null,
    linkedControls: null,
    selectAllControl: null,
    images: null,
    lastSelectItemIndex: null,
    noImageSelected: true,
    imageCopyCallCount: 0,
    imagesCopied: 0,
    copyToLightboxId: null,
    imagesToCopy: null,

    initialize: function(imageContainer, imageTagSpecifier, selectAllControl, linkedControls) {
        this.imageContainer = $(imageContainer);
        this.imageTagSpecifier = imageTagSpecifier;
        this.selectAllControl = $(selectAllControl);
        this.linkedControls = linkedControls;
    },

    getImages: function() {
        return this.imageContainer.getElements(this.imageTagSpecifier);
    },

    getSelectedImages: function() {
        return this.imageContainer.getElements(this.imageTagSpecifier + '.selected');
    },

    selectItem: function(e, item) {
        item = $(item);
        var shift = e.shift == undefined ? e.shiftKey : e.shift;
        var images = this.getImages();

        if (shift && lastSelectItemIndex != null) {
            var currentIndex = images.indexOf(item);
            var startIndex = currentIndex;
            var endIndex = lastSelectItemIndex;

            if (startIndex > endIndex) {
                startIndex = lastSelectItemIndex;
                endIndex = currentIndex;
            }

            for (var i = startIndex; i <= endIndex; i++) {
                if (!images[i].hasClass('selected')) images[i].addClass('selected');
            }
        }
        else {
            item.toggleClass('selected');
        }

        lastSelectItemIndex = images.indexOf(item);

        if (item.hasClass('selected') || this.imageContainer.getElements(this.imageTagSpecifier + '.selected').length > 0) {
            this.noImageSelected = false;
        }
        else {
            this.noImageSelected = true;
        }

        this.setLinkedControls();
    },

    lightboxSelected: function() {
        var selectedLightboxId = $('copyProduct').getElement('div.copyControls').getElement('select').value;
        return (selectedLightboxId != "0" && selectedLightboxId != "");
    },

    setLinkedControls: function() {
        if (this.linkedControls && this.linkedControls.length > 0) {
            for (var i = 0; i < this.linkedControls.length; i++) {
                var control = this.linkedControls[i];
                if (control.hasClass('GlassButton')) {
                    setGlassButtonDisabled(control, (this.noImageSelected || !this.lightboxSelected()));
                }
                else {
                    if (this.noImageSelected || !this.lightboxSelected()) {
                        if (!control.hasClass('disabled')) control.addClass('disabled');
                    }
                    else {
                        control.removeClass('disabled');
                    }
                }
            };
        }
    },
    changeToSelectAll: function() {
        if (!this.selectAllControl.hasClass('disabled')) {
            var selectText = this.selectAllControl.getProperty('select');
            var deselectText = this.selectAllControl.getProperty('deselect');
            if (this.selectAllControl.get('text') == deselectText) {
                this.deselectAll();
                this.selectAllControl.set('text', selectText);
            }
        }
    },
    toggleSelectAll: function() {
        if (!this.selectAllControl.hasClass('disabled')) {
            var selectText = this.selectAllControl.getProperty('select');
            var deselectText = this.selectAllControl.getProperty('deselect');

            if (this.selectAllControl.get('text') == selectText) {
                this.selectAll();
                this.selectAllControl.set('text', deselectText);
            }
            else {
                this.deselectAll();
                this.selectAllControl.set('text', selectText);
            }
        }
    },

    selectAll: function() {
        this.getImages().each(function(el) {
            if (!el.hasClass('selected')) el.addClass('selected');
        });

        this.noImageSelected = false;
        this.setLinkedControls();
        fixIeDisplay();
    },

    deselectAll: function() {
        this.getImages().each(function(el) {
            el.removeClass('selected');
        });

        this.noImageSelected = true;
        this.setLinkedControls();
        fixIeDisplay();
    },

    copySelectedItems: function() {
        var lightboxDropdown = $('copyProduct').getElement('select');
        var selectedImages = this.getSelectedImages();
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
        //console.log(activeLightbox);
        var selectedLightbox = lightboxDropdown.options[lightboxDropdown.selectedIndex].value;

        if (activeLightbox && selectedImages.length > 0) {

            LogOmnitureEvent("event11");

            var productUids = new Array(selectedImages.length);
            selectedImages.each(function(el, index) {
                productUids[index] = el.getProperty('productuid');
            });

            HideModal('copyProduct');
            this.imagesCopied = 0;
            this.imageCopyCallCount = Math.ceil(productUids.length / CorbisUI.GlobalVars.Lightbox.maxImageCopyPerCall);
            this.copyToLightboxId = selectedLightbox
            this.imagesToCopy = productUids


            for (i = 0; i < this.imageCopyCallCount; i++) {
                var startIndex = i * CorbisUI.GlobalVars.Lightbox.maxImageCopyPerCall;
                var endIndex = ((i + 1) * CorbisUI.GlobalVars.Lightbox.maxImageCopyPerCall);
                var imagesToCopyText = i == (this.imageCopyCallCount - 1) ? String.format('CorbisUI.Lightbox.CopyImages().imagesToCopy.slice({0})', startIndex.toString()) : String.format('CorbisUI.Lightbox.CopyImages().imagesToCopy.slice({0}, {1})', startIndex.toString(), endIndex.toString());

                setTimeout(String.format('Corbis.Web.UI.Lightboxes.LightboxScriptService.CopyLightboxImages({0}, {1}, {2}, CorbisUI.Lightbox.CopyImages().copySelectedItemsSucceeded, CorbisUI.Lightbox.Handler.methodFailed);', activeLightbox.get('id'), selectedLightbox, imagesToCopyText), 0);
            }
        }
    },

    copySelectedItemsSucceeded: function(result) {
        CorbisUI.Lightbox.CopyImages().imagesCopied += result;

        if (CorbisUI.Lightbox.CopyImages().imageCopyCallCount <= 0) {
            CorbisUI.Lightbox.CopyImages().copySelectedItemsCompleted();
        }
    },

    copySelectedItemsCompleted: function() {
        $('copySuccess').getElement('div.ModalPopupContent').getElement('div').set('html', String.format(CorbisUI.GlobalVars.Lightbox.copySuccessTemplate, this.imagesCopied));
        var itemID = this.copyToLightboxId;
        //CorbisUI.DomCache.addObject('ActiveLightbox', $(itemID));
        //console.log(itemID);
        //console.log(CorbisUI.DomCache.get('ActiveLightbox').id);

        var itemCountDiv = $try(
            function() {
                return $(itemID).getElement('div.info');
            },
            function() {
                return $(itemID).getElement('span.imageCount');
            },
            function() {
                return false;
            }
        );

        if (itemCountDiv) {
            var itemCountDivText = String.trim(itemCountDiv.get('text'));
            var itemCount = this.imagesCopied;
            if (itemCountDivText && itemCountDivText != '') {
                itemCount += parseInt(itemCountDivText);
            }

            itemCountDiv.set('text', String.format(itemCount == 1 ? CorbisUI.GlobalVars.Lightbox.itemTemplate : CorbisUI.GlobalVars.Lightbox.itemsTemplate, itemCount));
        }

        new CorbisUI.Popup('copySuccess', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'),
            positionVert: 'bottom',
            positionHoriz: 'left',
            onHide: function() { $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv).removeClass('selected'); }
        });

        //set event to deselect menu button 
        $('copySuccess').getElements('input').each(function(el) {
            el.addEvent('click', function() { $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv).removeClass('selected'); });
        });

        GetLB(CorbisUI.DomCache.get('ActiveLightbox').id, true);

        HideModal('copyProgress');
    }
});

//Static object for transfer
var _transferLightbox = null;
CorbisUI.Lightbox.transferLightbox = function(reset)
{
	if (!_transferLightbox)
	{
		_transferLightbox = new CorbisUI.Lightbox.transferLightboxModal();
	}
	else if (reset)
	{
		_transferLightbox.reset();
	}
	
	return _transferLightbox;
}

//transfer model
CorbisUI.Lightbox.transferLightboxModal = new Class({
    Implements: [Options, Events],

    memberList: null,
    transferTo: null,
    transferModal: null,
    deleteMemberLink: null,
    addMemberName: null,
    lightboxId: null,
    modalWindow: null,
    noMemberMessage: null,
    noTransferToMessage: null,
    associatesToDelete: null,
    addTransferTo: null,
    removeTransferTo: null,
    removeFromLightbox: null,
    removeFromLightboxCtl: null,
    transferButton: null,
    activeModal: null,
    errorTracker: null,

    initialize: function() {
        this.lightboxId = CorbisUI.DomCache.get('ActiveLightbox').get('id');
        this.transferModal = $('transferLightbox');
        this.memberList = $('corbisMembers');
        this.transferTo = $('transferTo');
        this.deleteMemberLink = this.transferModal.getElement('a.deleteMembers');
        this.addMemberName = this.transferModal.getElement('input.addMemberName');
        this.noMemberMessage = $('noMemberMessage');
        this.noTransferToMessage = $('noTransferToMessage');
        this.addTransferTo = $('addTransferTo');
        this.removeTransferTo = $('removeTransferTo');
        this.removeFromLightbox = this.transferModal.getElement('input[type=checkbox]');
        this.removeFromLightboxCtl = this.removeFromLightbox.getParent().getParent().id;
        this.transferButton = this.transferModal.getElement('div.transfer');
        this.setModalTitle($('lightboxesContent').getElement('.LightboxNameSpan').get('html'));
        Corbis.Web.UI.Lightboxes.LightboxScriptService.GetMemberAssociates(this.populateMembers, CorbisUI.Lightbox.Handler.methodFailed, this);
    },

    reset: function() {
        this.lightboxId = CorbisUI.DomCache.get('ActiveLightbox').get('id');
        this.setModalTitle($('lightboxesContent').getElement('.LightboxNameSpan').get('html'));
        this.addMemberName.value = '';

        if (this.removeFromLightbox.checked) toggleCheckedState(this.removeFromLightboxCtl);

        this.removeFromLightbox.checked = false;
        this.transferTo.getChildren().each(function(el) {
            el.setProperty('selected', true);
        });

        this.moveMembersBewteenLists(false);
    },

    setModalTitle: function(title) {
        var titleSpan = this.transferModal.getElement('span.Title');
        var lightboxNameSpan = titleSpan.getElement('span');
        if (!lightboxNameSpan) {
            lightboxNameSpan = new Element('span', {
                'class': 'title'
            });

            lightboxNameSpan.inject(titleSpan);
        }

        lightboxNameSpan.set('html', title);
    },

    populateMembers: function(result, context) {
        context.memberList.empty();

        if (result && result.length > 0) {
            result.each(function(el) {
                var optionElement = new Element('option', {
                    'value': el.Key,
                    'html': el.Value
                });
                optionElement.inject(context.memberList);
            });
            context.memberList.removeProperty('disabled');
        };

        context.updateNoMember();
        context.updateMemberSelect();
    },

    addAssociate: function() {
        this.errorTracker.removeError(this.addMemberName.id);
        Corbis.Web.UI.Lightboxes.LightboxScriptService.AddMemberAssociate(this.addMemberName.value, this.addAssociateSucceeded, CorbisUI.Lightbox.Handler.methodFailed, this);
    },

    addAssociateSucceeded: function(result, context) {
        if (result.ErrorMessage == null) {
            //make sure user not already on list
            if ((context.memberList.getElement('option[value=' + result.Username + ']') == null) &&
                (context.transferTo.getElement('option[value=' + result.Username + ']') == null)) {
                var optionElement = new Element('option', {
                    'value': result.Username,
                    'html': result.AssociateDisplay
                });

                var listIndex = 0;

                if (context.memberList.length > 0) {
                    //Find the correct location to insert the new member.
                    while (listIndex < context.memberList.length && result.AssociateDisplay.toLowerCase() > context.memberList[listIndex].value.toLowerCase()) {
                        listIndex++;
                    }

                    //If end of the list then just add it to the end.
                    if (listIndex == context.memberList.length) {
                        optionElement.inject(context.memberList[listIndex - 1], 'after');
                    }
                    else {
                        optionElement.inject(context.memberList[listIndex], 'before');
                    }
                }
                else {
                    optionElement.inject(context.memberList);
                }

                context.updateNoMember();
            }

            context.addMemberName.value = '';
        }
        else {
            context.errorTracker.addError(context.addMemberName.getProperty('id'), result.ErrorMessage);
        }
    },

    updateNoMember: function() {
        if (this.memberList.length > 0) {
            if (!this.noMemberMessage.hasClass('hdn')) this.noMemberMessage.addClass('hdn');
            this.memberList.removeClass('disabled');
            this.memberList.removeProperty('disabled')
        }
        else {
            this.noMemberMessage.removeClass('hdn');
            if (!this.memberList.hasClass('disabled')) this.memberList.addClass('disabled');
            this.memberList.setProperty('disabled', 'disabled');
        }
    },

    updateNoTransferTo: function() {
        if (this.transferTo.length > 0) {
            if (!this.noTransferToMessage.hasClass('hdn')) this.noTransferToMessage.addClass('hdn');
            this.transferTo.removeClass('disabled');
            this.transferTo.removeProperty('disabled')
        }
        else {
            this.noTransferToMessage.removeClass('hdn');
            if (!this.transferTo.hasClass('disabled')) this.transferTo.addClass('disabled');
            this.transferTo.setProperty('disabled', 'disabled');
        }

        setGlassButtonDisabled(this.transferButton, this.transferTo.length == 0);
    },

    updateMemberSelect: function() {
        if (this.memberList && this.memberList.getElement('option[selected]')) {
            this.deleteMemberLink.removeProperty('disabled');
            this.deleteMemberLink.removeClass('disabled');
            this.addTransferTo.removeClass('disabled');
            this.addTransferTo.removeProperty('disabled');
        }
        else {
            this.deleteMemberLink.setProperty('disabled', 'disabled');
            if (!this.deleteMemberLink.hasClass('disabled')) this.deleteMemberLink.addClass('disabled');
            if (!this.addTransferTo.hasClass('disabled')) this.addTransferTo.addClass('disabled');
            this.addTransferTo.setProperty('disabled', 'disabled');
        }
    },

    updateTransferToSelect: function() {
        if (this.transferTo && this.transferTo.getElement('option[selected]')) {
            this.removeTransferTo.removeClass('disabled');
            this.removeTransferTo.removeProperty('disabled');
        }
        else {
            if (!this.removeTransferTo.hasClass('disabled')) this.removeTransferTo.addClass('disabled');
            this.removeTransferTo.setProperty('disabled', 'disabled');
        }
    },

    moveMembersBewteenLists: function(toTransferToList) {
        var fromList = toTransferToList ? this.memberList : this.transferTo;
        var toList = toTransferToList ? this.transferTo : this.memberList;

        var selectedMembers = fromList.getElements('option[selected]');
        if (selectedMembers.length > 0) {
            var toListChildrens = toList.getChildren();
            var toListIndex = 0;

            //because both list should be sorted already, we should be able to do this in a single pass.
            selectedMembers.each(function(el) {
                el.removeProperty('selected');

                while (toListIndex < toListChildrens.length && el.value > toListChildrens[toListIndex].value) {
                    toListIndex++;
                }

                if (toListIndex < toListChildrens.length) {
                    el.inject(toListChildrens[toListIndex], 'before');
                }
                else {
                    el.inject(toList, 'bottom');
                }
            });

            this.updateNoMember();
            this.updateNoTransferTo();
            this.updateMemberSelect();
            this.updateTransferToSelect();
        }
    },

    deleteAssociates: function() {
        var selectedItems = this.memberList.getElements('option[selected]');
        if (selectedItems) {
            var usernameArray = new Array(selectedItems.length);
            selectedItems.each(function(el, index) {
                usernameArray[index] = el.value;
            });

            this.associatesToDelete = selectedItems;

            Corbis.Web.UI.Lightboxes.LightboxScriptService.RemoveMemberAssociates(usernameArray, this.deleteAssociatesSucceeded, CorbisUI.Lightbox.Handler.methodFailed, this);
        }
    },

    deleteAssociatesSucceeded: function(result, context) {
        if (context.associatesToDelete) {
            context.associatesToDelete.each(function(el) {
                el.destroy();
            });
        }

        context.updateNoMember();
        context.updateMemberSelect();
    },

    showModal: function() {
        this.modalWindow = new CorbisUI.Popup('transferLightbox', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'),
            positionVert: 'bottom',
            positionHoriz: 'left',
            onHide: function() { _transferLightbox.hideModal(false); }
        });

        this.activeModal = 'transferLightbox';
        this.fireEvent('show');

        window.addEvent('resize', this.setPosition.bind(this.modalWindow))
    },

    hideModal: function(afterTransfer) {
        HideModal('transferLightbox');

        //If we are going to show the success modal then we don't do the following yet.
        if (!afterTransfer) {
            this.fireEvent('hide');
            window.removeEvent('resize', this.setPosition);
        }
    },

    showTransferSuccess: function() {
        _transferLightbox.modalWindow = new CorbisUI.Popup('transferSuccess', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'),
            positionVert: 'bottom',
            positionHoriz: 'left',
            onHide: function() { _transferLightbox.hideTransferSuccess(); }
        });

        _transferLightbox.activeModal = 'transferSuccess';
        _transferLightbox.fireEvent('show');

        window.addEvent('resize', _transferLightbox.setPosition.bind(_transferLightbox.modalWindow));

        Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(_transferLightbox.showTransferSuccess);
    },

    hideTransferSuccess: function() {
        HideModal('transferSuccess');

        this.fireEvent('hide');
        window.removeEvent('resize', this.setPosition);
    },

    setPosition: function() {
        //context of this is the mochaUI modal.
        this.setPosition(CorbisUI.Lightbox.transferLightbox().activeModal);
    },

    transferLightbox: function() {
        LogOmnitureEvent("event9");
        
        var selectedAssociates = this.transferTo.getChildren();
        var associateUserNames = new Array(selectedAssociates.length);

        selectedAssociates.each(function(el, index) {
            associateUserNames[index] = el.value;
        });

        setGlassButtonDisabled($('transferLightbox').getElement('div.transfer'), true); //disable glass button for click muit-times
        Corbis.Web.UI.Lightboxes.LightboxScriptService.TransferLightbox(this.lightboxId, associateUserNames, this.removeFromLightbox.checked, this.transferLightboxSucceeded, CorbisUI.Lightbox.Handler.methodFailed, this);


    },

    transferLightboxSucceeded: function(result, context) {
        setGlassButtonDisabled($('transferLightbox').getElement('div.transfer'), false);
        if (context.removeFromLightbox.checked) {
            if ($$('div.Lightbox').length == 1) {
                // if lightbox is last one, will destroy transfer modal and refresh page to get defaul lightbox
                var transferredLightbox = $(context.lightboxId);
                if (transferredLightbox) {
                    transferredLightboxDiv = transferredLightbox.getParent();
                    transferredLightboxDiv.destroy();
                    window.location.reload(true);
                }
            } else {
                var transferredLightbox = $(context.lightboxId);
                if (transferredLightbox) {
                    transferredLightboxDiv = transferredLightbox.getParent();
                    var newActiveLightbox = transferredLightboxDiv.getNext();
                    if (!newActiveLightbox) newActiveLightbox = transferredLightboxDiv.getPrevious();

                    transferredLightboxDiv.destroy();

                    if (newActiveLightbox) {
                        GetLB(newActiveLightbox.getElement('div.LightboxRow'));
                    }
                }
            };

            //Have to show modal after the lightbox page is refreshed for IE7, otherwise the success modal gets shifted to top left corner.
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(_transferLightbox.showTransferSuccess);
        }
        else {
            context.showTransferSuccess();
        }

        context.hideModal(true);
    },

    initTransferValidation: function(containerId) {
        if (this.errorTracker)
            this.errorTracker.reset();
        else
            this.errorTracker = new CorbisUI.FormUtilities.ErrorTracker({ container: containerId });
    }
});	

function showNotes(el) {
    el = $(el);
    var showItemLink = el.getElement('span.showLink');
    var showStyle = showItemLink.getStyle('display');
    var hideItemLink = el.getElement('span.hideLink');
    var hideStyle = hideItemLink.getStyle('display');
    var headerDetails = $('detailStructure');
    if (showStyle == 'block') {
        showItemLink.setStyle('display', 'none');
        hideItemLink.setStyle('display', 'block');
        headerDetails.setStyle('display', 'block');
    }
    else {
        hideItemLink.setStyle('display', 'none');
        showItemLink.setStyle('display', 'block');
        headerDetails.setStyle('display', 'none');
   }

    if (hideStyle == 'none') {
        hideItemLink.setStyle('display', 'block');
        showItemLink.setStyle('display', 'none');
        headerDetails.setStyle('display', 'block');
    }
    else {
        hideItemLink.setStyle('display', 'none');
        showItemLink.setStyle('display', 'block');
        headerDetails.setStyle('display', 'none');
       $('notesGlassButton').setStyle('display', 'none');
    }
}
function showEdit(el) {
    var bulletPoint = $('lightboxDetailsEdit').getElement('span.bulletPoint');
    var editLink = $('lightboxDetailsEdit').getElement('span.editLink');
    var hideLink = $('lightboxDetailsEdit').getElement('span.hideLink');
    var showLink = $('lightboxDetailsEdit').getElement('span.showLink');
    var lblName = $('DetailsViewSide').getElement('span.lblName');
    var lblNotes = $('DetailsViewSide').getElement('textarea[id$=Note1]'); //$('DetailsViewSide').getElement('span.lblNotes');
    var txtName = $('DetailsViewSide').getElement('input.txtName');
    var txtNotes = $('DetailsViewSide').getElement('textarea.txtNote');
    var notesButtons = $('buttonsCenter').getElement('div.notesbuttonCSS');
    var noteText = lblNotes.value;
    txtName.value = lblName.get('text');
    var notes2 = '';
    if (noteText != '') {
        txtNotes.value = noteText;
    }
	bulletPoint.style.display= editLink.style.display = hideLink.style.display = showLink.style.display = lblName.style.display =lblNotes.style.display= "none";
    txtName.style.display = txtNotes.style.display = notesButtons.style.display = $('detailStructure').style.display = $('buttonsCenter').style.display = "block";
}
function fixIeDisplay()
{
	//resizing to fix IE problem where some newly added images are not displayed.
	var bodywidth = window.document.body.scrollWidth;
//	window.document.body.setStyle('width', bodywidth+1);
//	window.document.body.setStyle('width', bodywidth);
}

function registerLightboxTooltips()
{
    //if (!showTooltip() || thumbTips) return;
    
    $('aspnetForm').getAllNext('.TIP-product-block').destroy();
    
    var tipShowDelay = 500;
    var tipHideDelay = 100;
    var tipShowMethod = "in";
    var tipHideMethod = "out";
    if(Browser.Engine.trident) {
        tipShowDelay = 0;
        tipHideDelay = 0;
        tipShowMethod = "show";
        tipHideMethod = "hide";
    }
    thumbTips = new Tips('#LightboxProducts .thumbWrap', {
        showDelay: tipShowDelay,
        hideDelay: tipHideDelay,
        className: 'TIP-product-block', 
        onHide: function(tip){
            if (showTooltip()) tip.fade(tipHideMethod);
        }, 
        onShow:function(tip) {
            if (showTooltip()) tip.fade(tipShowMethod);
        }
    });

}

function showTooltip()
{
    return ($$('.previewOffSelected').length == 0);
}

function ToggleTree(lightbox)
{
    var img = lightbox.childNodes[0].childNodes[0];
    var children = lightbox.lastChild;
    
    if (children && children.Expanded == true)
    {
         // TODO: replace image source
        img.src = "../../Images/iconCaretCollapsed.gif";
        children.style.display = 'none';
        children.Expanded = false;
    }
    else
    {
        // TODO: replace image source
        img.src = "../../Images/iconCaretExpanded.gif";
        children.style.display = '';
        children.Expanded = true;
        //IE can use text-overflow style so don't need this.
        if (!Browser.Engine.trident) {
            TruncateNames($(lightbox).getElements('span.LightboxName'));
        }
    }
}

function TruncateNames(lightboxCollection)
{
    if (lightboxCollection) {
        if (lightboxCollection.length > 50) {
            lightboxCollection.chunk(function(el) {
                SetNameLength(el, CorbisUI.GlobalVars.Lightbox.sidbarWidth);
            }, 50);
        }
        else {
            lightboxCollection.each(function(el) {
                SetNameLength(el, CorbisUI.GlobalVars.Lightbox.sidbarWidth);
            });        
        }
	}
}

function SetNameLength(lightbox, containerWidth)
{
    var lightboxName;
    var truncated = false;
    var lbNameWidth = containerWidth - lightbox.offsetLeft;

    while (lightbox.offsetWidth > lbNameWidth) 
	{
		lightboxName = GetInnerText(lightbox);
		if (lightboxName.length > 0)
		{
			truncated = true;
			lightbox.innerHTML = HtmlEncode(lightboxName.substring(0, lightboxName.length - 1));
		}
	}
	
	if (truncated) 
	{
		lightboxName = GetInnerText(lightbox);
		if (lightboxName.length >= 3)
		{
			lightboxName = lightboxName.substring(0, lightboxName.length - 3);
			lightboxName = lightboxName.replace(/\s+$/, '') + '...';
			lightbox.innerHTML = HtmlEncode(lightboxName);
		}
		else
		{
			lightbox.innerHTML = '...';
		}
	}
}

function GetInnerText(textNode)
{
	var innerText = '';
	//IE
	if (textNode.innerText)
	{
		innerText = textNode.innerText;
	}
	//Firefox
	if (textNode.textContent)
	{
		innerText = textNode.textContent;
	}
	return innerText;
}

function GetLB(lightboxRow,refreshOnly) {
    lightboxRow = $(lightboxRow);
	//Don't get active lightbox again for delete click.
 	var modalPop = $('deleteLightboxModalPopupWindow');
	if (!deleteClicked || modalPop.style.display=='none') // click window anywhere close the modal, and do check visibility.
	{
		if(activeLightBoxArray[0]=='')
		{
			activeLightBoxArray[0]=document.getElementById(CorbisUI.GlobalVars.Lightbox.postbackName).value;
		}
		else
		{
		    currentActive=document.getElementById(activeLightBoxArray[0]);
		  
		    if(currentActive)
		    {
    		    currentActive.className='LightboxRow';
    		    currentActive.childNodes[1].style.display='none';       
		    }
		}
       
        //expand the tree
        ExpandTree(lightboxRow.id);
        
		lightboxRow.childNodes[1].style.display='';
		lightboxRow.className = "Active";

		CorbisUI.DomCache.addObject('ActiveLightbox', lightboxRow);

		//scroll lightbox so delete is visible
		var lightboxTree = CorbisUI.DomCache.get('Tree');
		CorbisUI.DomCache.get('LBXContainer').scrollLeft = (lightboxRow.getCoordinates().left-lightboxTree.getCoordinates().left);

		activeLightBoxArray[0]=lightboxRow.id;

		if (!refreshOnly) {
		    if (document.getElementById(CorbisUI.GlobalVars.Lightbox.postbackName).value == lightboxRow.id) {
		        document.getElementById(CorbisUI.GlobalVars.Lightbox.refreshButton).click();
		    }
		    else {
		        document.getElementById(CorbisUI.GlobalVars.Lightbox.postbackName).value = lightboxRow.id;
		        __doPostBack(CorbisUI.GlobalVars.Lightbox.postbackName);
		    }
		}    
	}
}

function ExpandTree(currentLightbox) {
    currentLightbox = $(currentLightbox);
    if (currentLightbox) {
        childrenDiv = currentLightbox.getParent().getParent();
        if (childrenDiv.className == 'Children') {
            lightbox = $(childrenDiv.getProperty('parentid'));
            var lightboxParentNode = lightbox.getParent();
            if (lightbox && lightboxParentNode) {
                if (lightboxParentNode.getElement('div.Arrow img').getProperty('src').contains('Collapsed')) {
                    ToggleTree(lightbox.parentNode);
                }
                
                ExpandTree(lightbox.id);
            }
        }
    } 
}

function LoadActiveLightbox() {
    oldLightbox=document.getElementById(CorbisUI.GlobalVars.Lightbox.postbackName);
    LoadActive(oldLightbox.value);
}

function LoadActive(oldLightbox) {
	if(oldLightbox && oldLightbox.value!="" && document.getElementById(oldLightbox))
	{     
		childrenDiv=document.getElementById(oldLightbox).parentNode.parentNode;
		if(childrenDiv.className=='Children')
		{
			lightbox=document.getElementById(childrenDiv.attributes['parentid'].value);

			if(typeof(lightbox.parentNode) != 'undefined') 
			{
				ToggleTree(lightbox.parentNode);
				
				  LoadActive(lightbox.id);
			}
		} 
	} 
}

var LightboxDOP = true;
/**
    Do not enable the COFF button if none of the lightboxes are selected in the right or if the selected lightbox doesnt contain
    any items
**/
function ModifyCOFFOptions() {
    var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
    var disable = false;
    if (!activeLightbox) {
        disable = true;
    } else {
        var totalItems = $('header2').getElement('input.totalItems').value;
        if (totalItems == 0) {
            disable = true;
        }
    }
    if (disable) {
        var coffButton = $(CorbisUI.GlobalVars.Lightbox.coffButton);
        if (coffButton) {
            coffButton.setStyle('display', 'none');
        }
    }
}
// MyLightbox.aspx page includes SearchResultsHeader - #header1 and #header2.
// And this function removes the #header2, terms Clarification On/Off DIV elements for the Display Options model
function ModifyDisplayOptions()
{        
    var divMoreDisplayOptions = $('moreDisplayOptions');        
    var divThumbContentContainer = $('thumbContentContainer');
    var divheader2= $('header2');
            
    //Resizing the window
    divMoreDisplayOptions.setStyle('height', '100px');
    /*var groupDivs = new Hash({
        'divTermClarifications':$('lowerThumbContentContainer'),
        'divSeperatorLine':$('seperatorLine')
    });
    groupDivs.each(function(value,key,hash){
    hash.get(key).destroy();
    }); */
    if($type($('lowerThumbContentContainer'))) $('lowerThumbContentContainer').destroy();
    if($type($('seperatorLine'))) $('seperatorLine').destroy();
}

var closeLightboxEvent;
function OpenLightbox(divId) {
    var newlightbox = $('NewLightboxButton');
    if (!newlightbox.hasClass('selected')) newlightbox.addClass('selected');
    newlightbox.store('activated', true);


    this.pop = new CorbisUI.Popup('createLightboxModalPopup', { 
                showModalBackground: false, 
                createFromHTML: false, 
                closeOnLoseFocus: true, 
                centerOverElement: 'DetailsViewSide', 
                positionVert: 'bottom', 
                positionHoriz: 'right'
                });
                var clmpw = $('createLightboxModalPopup');
    //Use cancel button ID to retrieve the cancel button . See Bug 17818    
    var group = new Hash({
        'mo': $('modalOverlay'),
        'close':clmpw.getElement('div.ModalTitleBar input.Close'),
        'cancel': $(CorbisUI.GlobalVars.CreateLightbox.cancelButtonID)
    });
    
    closeLightboxEvent = CloseLightbox.bindWithEvent(newlightbox,group);
        
    group.each(function(value,key,hash){
    hash.get(key).addEvent('click',closeLightboxEvent);
    });            
}
   
function CloseLightbox(e,grp)
{
this.removeClass('selected');
this.store('activated', false);

var clmpw = $('createLightboxModalPopup');
var validationSummary = clmpw.getElement('.ValidationSummary');
if (validationSummary != null) {
    validationSummary.setStyle('display', 'none');
}

var newNameField = clmpw.getElement('input[type=text]');
newNameField.value = '';
newNameField.setStyle('color', '#333333');

grp.each(function(value,key,hash){
hash.get(key).removeEvent('click',closeLightboxEvent);
});
grp = $lambda(false);    
}

function ShowDelete(lightboxId, childrenCount, ischildShareTo, ischildSharebyOwner) 
{
deleteClicked = true;
var lightboxRow = document.getElementById(lightboxId);
	
if (lightboxRow)
{
var childrenWarningTemplate = $('childrenMessageTemplate');
var childrenLightboxMessage = $('childrenLightboxMessage');
var childrenSharedP = $('sharedchildrenParentTemplate');
var childrenShared = $('sharedchildrenTemplate');
var childrenWarning = $('childrenMessage');
var Buttons = document.getElementById('Buttons');
var modalTitle = $('title');
var modalTitleSorry = $('titleSorry');
		
if (childrenWarning && childrenWarningTemplate && childrenLightboxMessage )
{
if (childrenCount > 0)
{
if( ischildShareTo == 'True')
{
childrenWarning.innerHTML = childrenShared.innerHTML;
modalTitleSorry.style.display = 'block';
modalTitle.style.display = 'none';
Buttons.style.display='none';
}
else if(ischildSharebyOwner == 'True')
{
childrenWarning.innerHTML = childrenSharedP.innerHTML;
modalTitle.style.display = 'block';
modalTitleSorry.style.display = 'none';
Buttons.style.display='block';
}
else
{
childrenWarning.innerHTML = childrenWarningTemplate.innerHTML.replace('{0}', childrenCount);
modalTitle.style.display = 'block';
modalTitleSorry.style.display = 'none';
Buttons.style.display='block';
}
				
}
else
{
if( ischildShareTo == 'True')
{
childrenWarning.innerHTML = childrenShared.innerHTML;
modalTitleSorry.style.display = 'block';
modalTitle.style.display = 'none';
Buttons.style.display='none';
}
else if(ischildSharebyOwner == 'True')
{
childrenWarning.innerHTML = childrenSharedP.innerHTML;
modalTitle.style.display = 'block';
modalTitleSorry.style.display = 'none';
Buttons.style.display='block';
}
else{
			    
childrenWarning.innerHTML = childrenLightboxMessage.innerHTML.replace('{0}', lightboxRow.title);
modalTitle.style.display = 'block';
modalTitleSorry.style.display = 'none';
Buttons.style.display='block';
}
}
}
OpenDeleteLightboxModal( lightboxRow);
}
}
function ClearightboxIdForCopy() {
    $('lightboxImages').removeProperty('lightboxId');

}
function OpenDeleteLightboxModal(objLightboxRow)
{
var cur =  findPos($(objLightboxRow));
var p = new CorbisUI.Popup('deleteLightboxModalPopup', { 
showModalBackground: false,
centerOverElement: 'SearchBuddy',
closeOnLoseFocus: true,
positionVert: 0, 
positionHoriz: 0
});    

ResizeModal('deleteLightboxModalPopup');
var left = cur[0] + objLightboxRow.offsetWidth;
var top = cur[1] ;

//Reposition because getCoordinate() does not work so well for Safari.
var LB = $('LBXContainer');
var elementSrollPos= getElementScrollXY(LB);
if(elementSrollPos[0] > 0 )
{
left = left - elementSrollPos[0];
}
var windowScrollPos = getScrollXY();
if(windowScrollPos[1]>0)
{
top =  top-elementSrollPos[1] + windowScrollPos[1] -objLightboxRow.offsetHeight;
}
else
{
top =  top-elementSrollPos[1] -objLightboxRow.offsetHeight ;
}
$('deleteLightboxModalPopupWindow').setStyles({
top: top,
left: left 
});
}

function getElementScrollXY(obj) {
var scrOfX = 0, scrOfY = 0;
if( typeof( obj.pageYOffset ) == 'number' ) {
//Netscape compliant
scrOfY = obj.pageYOffset;
scrOfX = obj.pageXOffset;
} else if( obj && ( obj.scrollLeft || obj.scrollTop ) ) {
//DOM compliant
scrOfY = obj.scrollTop;
scrOfX = obj.scrollLeft;
} else if( obj && (obj.scrollLeft || obj.scrollTop ) ) {
//IE6 standards compliant mode
scrOfY = obj.scrollTop;
scrOfX = obj.scrollLeft;
}
return [ scrOfX, scrOfY ];
}

function getScrollXY() {
var scrOfX = 0, scrOfY = 0;
if( typeof( window.pageYOffset ) == 'number' ) {
//Netscape compliant
scrOfY = window.pageYOffset;
scrOfX = window.pageXOffset;
} else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) {
//DOM compliant
scrOfY = document.body.scrollTop;
scrOfX = document.body.scrollLeft;
} else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) {
//IE6 standards compliant mode
scrOfY = document.documentElement.scrollTop;
scrOfX = document.documentElement.scrollLeft;
}
return [ scrOfX, scrOfY ];
}

function findPos(obj) 
{
// Find any element postion x and y in the windows.
var curleft = curtop = 0;
if (obj.offsetParent) {
curleft = obj.offsetLeft
curtop = obj.offsetTop
while (obj = obj.offsetParent) {
curleft += obj.offsetLeft
curtop += obj.offsetTop
}
} 
return [curleft,curtop];	    
}

function CloseDeleteLightboxPopup(modalPopupName)
{
deleteClicked = false;
HideModal('deleteLightboxModalPopup');
}


/**********************
 Request Price 
 *********************/
 CorbisUI.RequestPricing = new Class({
 
    Implements: [Options, Events],
    corbisId: null,
    lightboxid: null,
    continuebtn: null,
    selecteditem:null,
    container:null,
    productBlock:null,
    productUid: null,
    licenseModel: null,
    
    modalWindow: null,
    continueButton: null,
    continueButtonEvent: null,
    
    initialize: function (){ /* just to initialize */ },
    
    getModalWindow: function(){
        if(this.modalWindow){
            MochaUI.ShowModal('requestPriceImagesModal');
        }else{
            var popup = new CorbisUI.Popup('requestPriceImagesModal', {
                createFromHTML: false,            
                showModalBackground: false,
                closeOnLoseFocus: true,
                positionVert: 'top',
                positionHoriz: 'right',
                width:308,
                height:150
            });
            this.modalWindow = $('requestPriceImagesModalWindow');
            this.images = this.modalWindow.getElements('input[type=radio]');
            this.continuebtn = this.modalWindow.getElements('input[type=submit]')[1];
            this.continueButtonEvent = this.continueButtonEvent_action.bindWithEvent(this);
            this.continuebtn.addEvent('click',this.continueButtonEvent);
        }
        if(!this.images[0].checked && !this.images[1].checked) this.images[0].checked = true;
    },
    
    continueButtonEvent_action: function(){
        // 1- The selected image 2-All images in lightbox 0- no option selected
            if(this.images[0].checked)
            {
                //This picks the first radio option "the image you selected, Or"
                this.selecteditem = '1';
            }
            else if(this.images[1].checked)
            {
                //This picks the second radio option "ALL images in this lightbox"
                this.selecteditem = '2';
            }
            else
            {
                this.selecteditem = '0';
            }
         
        HideModal('requestPriceImagesModal');
        //CloseModal('requestPriceImagesModal');
        CorbisUI.Pricing.ContactUs.OpenRequestForm(this.corbisId, this.lightboxId, this.selecteditem, this.container );
        return false;
    },
    

    
    showModal: function (corbisid, lightboxid, container){
         var totalItemsInLB = $('header2').getElement('input.totalItems').value;//
         this.corbisId = corbisid;
         this.lightboxId = lightboxid;
         this.container = container;
        if(totalItemsInLB > 1) 
        {
            this.getModalWindow();
        }
        else
        {
            CorbisUI.Pricing.ContactUs.OpenRequestForm(this.corbisId, this.lightboxId, null, this.container );
        }
    }
    
 });
 CorbisUI.RequestPricing = new CorbisUI.RequestPricing();
 
 
 

/***************************
My lightbox BUDDY
****************************/
CorbisUI.MyLightboxes = {

	init: new Class({
	    
		tabs: null,
		buddy: null,
		
		activeWindow: null,
		activeTab: null,
		
		initialize: function(){
		    //console.log('CorbisUI.MyLightboxes INIT');
			this.buddy = $('SearchBuddy');
			this.tabs = new Hash({});
		   if (!window.location.href.toLowerCase().contains('emaillightboxview.aspx') && this.buddy != null)
		   {
		   
			 var tabItems = this.buddy.getElement('ul.SB_tabs').getChildren();
		   
			//console.log($type(this.tabs));
			
			tabItems.each(function(el){
				var id = el.get('id'); 
				id = id.substr(id.indexOf('SBT_'));
				id = id.replace('SBT_','');
				//console.log($type(this.tabs));
				switch(id){
					case "filters":
					case "lightboxes":
					case "quickpic":
						this.tabs.set(id, new CorbisUI.MyLightboxes.tab(el,this));
						break;
				}
			},this);
			}
				
		}
	}),
	
	fireLightboxEvent: function(ele){
		var el = $(ele).getParent().getParent().getParent();
		
		var lbId = el.get('id').replace('cartBlock_','LBX_');
		//el.setStyle('border','1px solid red');
		
		// check to see if lbx item exists already
		if(!$(lbId)){
			var lbItem = el.getElement('div.thumbWrap').getFirst().getFirst().getFirst().clone().removeProperties('title','class');
			lbItem.setProperty('id',lbId).setStyles({
				'float':'left',
				'clear':'both',
				'margin':4
			})
		
			var lb = CorbisUI.MyLightboxes.init.tabs.get('lightboxes');
		
			lb.el.fireEvent('click');
		
			lb.panel.getElement('div.LBXContainer').grab(lbItem,'top');
		}
	},
	
	tab: new Class({
		name: null,
		el:null,
		buddy: null,
		panel: null,
		initialize: function(el,buddy){
			this.el = el;
			this.name = el.get('id').substr( el.get('id').indexOf('SBT_') + 4);
			this.buddy = buddy;
			this.panel = $('SBBX_'+this.name);
			this.el.addEvent('click',this.clickEvent.bindWithEvent(this));
			if(this.el.hasClass('ON')){
				this.buddy.activeTab = this.name;
			}
		},
		
		clickEvent: function(){
			if(this.buddy.activeTab != this.name){
				var active = this.buddy.tabs.get(this.buddy.activeTab);
				active.el.removeClass('ON');
				active.panel.addClass('hdn');
				this.buddy.activeTab = this.name;
				this.el.addClass('ON');
				this.panel.removeClass('hdn');
			}
		}
	}),

	/* SEARCH BUDDY FLOATER */
	floater: new Class({

	    box: null,
	    wrap: null,
	    footer: null,
	    progressContainer: null,
	    progressLoader: null,
	    windowCoordinates: null,

	    initialize: function(el, wrap, footer, progressContainer) {
	        if (!window.location.href.toLowerCase().contains('emaillightboxview.aspx')) {
	            this.box = el;
	            this.wrap = wrap;
	            this.footer = footer;
	            this.progressContainer = progressContainer;
	            var wC = this.wrap.getCoordinates();
	            this.box.setStyles({
	                'top': wC.top + 10
	            });

	            this.wC = {
	                top: wC.top,
	                bottom: wC.top + wC.height
	            };

	            this.windowCoordinates = window.getCoordinates();
	            // add window events
	            window.addEvent('scroll', this.windowScroll.bindWithEvent(this));
	            window.addEvent('resize', this.windowResize.bindWithEvent(this));

	            // run the Scroll for the first time
	            // fixes problem of coming back to a page
	            // and the buddy is messed up
	            this.windowScroll();
	        }
	    },

	    windowResize: function() {
	        // WINDOW COORDINATES
	        this.windowCoordinates = window.getCoordinates();
	        this.windowScroll();
	    },

	    windowScroll: function() {
	        //Close create lightbox popup so we don't have to reposotion it.
	        HideModal('createLightboxModalPopup');

	        // WINDOW SCROLL
	        var wScroll = window.getScroll();
	        //console.log('WINDOW SCROLL: '+wScroll.toSource());

	        // WRAP COORDS
	        var wrapC = this.wrap.getCoordinates();

	        // FOOTER COORDINATES
	        var fC = this.footer.getCoordinates();

	        // Magical powers detect the SCROLL-X
	        (wScroll.x > 0) ? this.box.setStyle('left', '-' + wScroll.x + 'px') : this.box.setStyle('left', 0);

	        // Magical powers detect the SCROLL-Y

	        // if global nav still visible
	        if (wScroll.y < this.wC.top) {
	            this.box.setStyle('top', (wrapC.top - wScroll.y) + 15);
	        } else {
	            if (this.box.getStyle('top').toInt() != 10) this.box.setStyle('top', 10);
	        }

	        // detect distance from top of footer

	        // get value for footer distance check
	        var fCheck = fC.top - this.windowCoordinates.height;

	        // if footer visible
	        if (wScroll.y > fCheck) {
	            this.box.setStyle('bottom', (this.windowCoordinates.height - (fC.top - wScroll.y)) + 10);
	            // if footer is not visible
	        } else {
	            if (this.box.getStyle('bottom').toInt() != -50) this.box.setStyle('bottom', -10);
	        }
	    }
    })
};

/***************************
    HANDLER FUNCTIONS
****************************/

CorbisUI.Lightbox.Handler = {

    downloadOverlay: null,

    showDeleteModal: function(element) {
        target = new CorbisUI.Lightbox.ProductBlock($(element).getParent().getParent().getProperty('corbisId'));

        var image = target.thumbWrap.getElement('img');
        $(CorbisUI.GlobalVars.Lightbox.selectedProduct).value = target.productUid
        var imageDiv = $('modalDeleteTemplate').getElement('div.pinkyThumb');
        var pinkyEle = CorbisUI.Lightbox.CreatePinkyObject(image, target.productUid).getElement('div.pinkyWrap');

        imageDiv.empty();
        pinkyEle.inject(imageDiv);

        new CorbisUI.Popup('modalDeleteTemplate', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: target.productBlock,
            positionVert: 'middle',
            positionHoriz: 'right'
        });

        var notePopup = $('modalDeleteTemplateWindow');
        notePopup.setStyle("left", Math.min($(document).getCoordinates().width - notePopup.getCoordinates().width, notePopup.getCoordinates().left));
    },

    refreshCartItem: function(corbisId, cartItemCount) {
        var productBlock = new CorbisUI.Lightbox.ProductBlock(corbisId);
        if (productBlock.isValid) {
            productBlock.refreshCartItem(cartItemCount);
        }
        else {
            //update cart count
            UpdateCartCount(cartItemCount);
        }
    },

    showNoteModal: function(element) {
        var noteIcon = $(element)
        var target = new CorbisUI.Lightbox.ProductBlock(noteIcon.getParent().getParent().getProperty('corbisId'));

        var image = target.thumbWrap.getElement('img');
        $('selectedCorbisId').value = target.corbisId
        var imageDiv = $('modalAddEditNoteTemplate').getElement('div.pinkyThumb');
        var pinkyEle = CorbisUI.Lightbox.CreatePinkyObject(image, target.productUid).getElement('div.pinkyWrap');

        imageDiv.empty();
        pinkyEle.inject(imageDiv);

        var noteTemplate = $('modalAddEditNoteTemplate');
        noteTemplate.getElement('span.Title').set('text', noteIcon.getParent().getProperty('title'));
        noteTemplate.getElement('#noteText').value = HtmlDecodeTextArea(target.noteText)

        new CorbisUI.Popup('modalAddEditNoteTemplate', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: target.productBlock,
            positionVert: 'middle',
            positionHoriz: 'right'
        });

        var notePopup = $('modalAddEditNoteTemplateWindow');
        notePopup.setStyle("left", Math.min($(document).getCoordinates().width - notePopup.getCoordinates().width, notePopup.getCoordinates().left));
    },

    showDeleteNoteModal: function(element) {
        var deleteNoteIcon = $(element)
        var target = new CorbisUI.Lightbox.ProductBlock(deleteNoteIcon.getParent().getParent().getParent().getParent().getParent().getProperty('corbisId'));

        $('selectedCorbisId').value = target.corbisId;
        new CorbisUI.Popup('modalDeleteNoteTemplate', {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: target.productBlock,
            positionVert: 'middle',
            positionHoriz: 'right'
        });
    },

    refreshQuickPicBuddy: function() {
        var quickpic = $('quickPicsContainer').getElement('input[id$=quickpicField]');
        quickpic.onclick(); //AJAX control
        var quickpicTab = $('SearchBuddy').getElement('li.SBT_quickpic');
        quickpicTab.fireEvent('click'); //MooTool event
    },

    emailLightbox: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');

        if (activeLightbox) {
            var lightboxId = activeLightbox.get('id');
            var lightboxUid = $(CorbisUI.GlobalVars.Lightbox.lightboxUid).value;
            var lightboxName = $('lightboxesContent').getElement('.LightboxNameSpan').get('text')
            var emailLightbox = $('emailButton');
            if (!emailLightbox.hasClass('selected')) emailLightbox.addClass('selected');

            CorbisUI.EmailLightbox.Handler.showEmailModal(lightboxId, lightboxUid, lightboxName, function() { $('emailButton').removeClass('selected'); });
        }
    },

    moveLightbox: function() {
        var moveButton = $('moveButton');
        if (!moveButton.hasClass('disabled')) {
            if (!moveButton.hasClass('selected'))
                moveButton.addClass('selected');
            CorbisUI.MoveLightbox.Handler.showMoveModal(moveButton);
        }
    },

    copyLightboxItems: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');

        if (activeLightbox) {
            var copyItemsButton = $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv);

            //return if copy button is disabled
            if (copyItemsButton.getElement('a.disabled')) {
                return;
            }

            var copyProduct = $('copyProduct');
            var lightboxId = activeLightbox.get('id');

            //Make the button selected
            if (!copyItemsButton.hasClass('selected')) copyItemsButton.addClass('selected');

            var imagesContainer = $('lightboxImages');
            var loadedLightbox = imagesContainer.getProperty('lightboxId');

            //Set the title
            var lightboxName = $('lightboxesContent').getElement('.LightboxNameSpan').get('text')
            var titleSpan = copyProduct.getElement('span.Title');

            var lightboxNameSpan = titleSpan.getElement('span');
            if (!lightboxNameSpan) {
                lightboxNameSpan = new Element('span', {
                    'styles': { 'font-weight': 'normal' }
                });

                lightboxNameSpan.inject(titleSpan);
            }

            lightboxNameSpan.set('html', HtmlFieldEncode(lightboxName));

            //Calculate image paging and get images, but only if not loaded or it's different.
            if (!loadedLightbox || loadedLightbox != lightboxId) {
                var totalItems = $('header2').getElement('input.totalItems').value;
                copyProduct.getElement('span#itemCount').set('text', totalItems);

                //disable glass button
                setGlassButtonDisabled(copyProduct.getElement('div.copyItemsModalButton'), true);

                //reset link to select all
                $('copyProduct').getElement('a.selectAllLink').set('text', $('copyProduct').getElement('a.selectAllLink').getProperty('select'));

                //Get lightbox dropdown
                var sortBy
                try {
                    sortBy = Corbis.LightboxCart.Contracts.V1.LightboxTreeSort.parse($('lightboxesContent').getElement('select.SortBy').value);
                }
                catch (e) {
                    sortBy = Corbis.LightboxCart.Contracts.V1.LightboxTreeSort.Date;
                }
                Corbis.Web.UI.Lightboxes.LightboxScriptService.GetLightBoxDropDownListForCopy(sortBy, CorbisUI.Lightbox.Handler.getLightBoxDropDownListForCopySucceeded, CorbisUI.Lightbox.Handler.methodFailed);

                //Calculate and get image pages
                CorbisUI.GlobalVars.Lightbox.copyItemsPagesDownloaded = 0;
                var pageHeight = CorbisUI.GlobalVars.Lightbox.copyItemPageSize / 5 * 110;
                var noOfPages = Math.floor(totalItems / CorbisUI.GlobalVars.Lightbox.copyItemPageSize);
                var lastPageImageCount = totalItems - (noOfPages * CorbisUI.GlobalVars.Lightbox.copyItemPageSize);
                var lastPageHeight = 0;
                var pageNumber = 0;

                if (lastPageImageCount > 0) {
                    noOfPages++;
                    lastPageHeight = Math.ceil(lastPageImageCount / 5) * 110
                }

                imagesContainer.scrollTo(0, 0);
                copyProduct.getElement('a.selectAllLink').addClass('disabled');

                var imagesPagesString = "";
                CorbisUI.GlobalVars.Lightbox.copyItemsPages = noOfPages;

                //Creating the blank page lists first
                noOfPages.times(function(index) {
                    imagesPagesString += String.format('<ul page="{0}" style="height:{1}px;"></ul>', pageNumber, pageNumber + 1 == noOfPages && lastPageHeight > 0 ? lastPageHeight : pageHeight);
                    pageNumber++;
                });

                imagesContainer.set('html', imagesPagesString);
                //set the lightbox id so we can keep track of what lightbox is loaded.
                imagesContainer.setProperty('lightboxId', lightboxId);

                //Populate the page lists
                for (var i = 0; i < noOfPages; i++) {
                    window.setTimeout(String.format('Corbis.Web.UI.Lightboxes.LightboxScriptService.GetLightboxCopyImages(\'{0}\', {1}, CorbisUI.GlobalVars.Lightbox.copyItemPageSize, CorbisUI.Lightbox.Handler.getLightboxCopyImagesSucceeded, CorbisUI.Lightbox.Handler.methodFailed, {2})', lightboxId, i + 1, i), 0);
                }
            }

            //Show the modal
            var copyProductModal = new CorbisUI.Popup('copyProduct', {
                createFromHTML: false,
                showModalBackground: false,
                closeOnLoseFocus: true,
                centerOverElement: $('DetailsViewSide'),
                positionVert: 'bottom',
                positionHoriz: 'left',
                onHide: function() { $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv).removeClass('selected'); }
            });

            //set event to deselect menu button 
            copyProduct.getElement('input.Close').addEvent('click', function() { $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv).removeClass('selected'); });
        }
    },

    getLightBoxDropDownListForCopySucceeded: function(result) {
        var selectDropdown = $('copyProduct').getElement('div.copyControls').getElement('select');

        if (result && result.length > 0) {
            //neither way works for all browsers, so we have to do this instead.
            if (Browser.Engine.gecko) {
                var optionsString = '';
                selectDropdown.empty();
                result.each(function(item) {
                    optionsString += String.format('<option value="{0}"><span>{1}</span></option>', item.Value, item.Key);
                });
                selectDropdown.set('html', optionsString);
            }
            else {
                selectDropdown.empty();
                result.each(function(item) {
                    var optionItem = new Element('option', {
                        'html': '<span>' + item.Key + '</span>',
                        'value': item.Value
                    });
                    optionItem.inject(selectDropdown);
                });
            }
        }

        //making the controls redraw to resolve problem in IE where the button sits in the middle of the dropdown after we repopulate the dropdown.
        var copyControls = $('copyProductWindow').getElement('div.copyControls');
        copyControls.setStyle('display', copyControls.getStyle('display'));
    },

    getLightboxCopyImagesSucceeded: function(result, context) {
        var lighboxImages = $('lightboxImages');

        if (result) {
            var imagesString = "";
            var templatestring = '<li productuid="{0}" onclick="CorbisUI.Lightbox.CopyImages().selectItem(event, this);"><div class="imageWrap"><img src="{1}" title="{2}" style="{3}"/></div></li>';

            result.each(function(image) {
                var width = 90;
                var height = 90;
                var marginTop = 0;
                if (image.AspectRatio > 1) {
                    height = 90 / image.AspectRatio;
                    marginTop = (90 - height) / 2;
                }
                else {
                    width = 90 * image.AspectRatio;
                }
                var imageStyle = 'margin-top: ' + marginTop + 'px; width: ' + width + 'px; height: ' + height + 'px';

                imagesString += String.format(templatestring, image.ProductUid, image.Url128, image.CorbisId + ' ' + HtmlEncode(image.Title), imageStyle);
            });

            var imageThumbnailContainer = lighboxImages.getElement('ul[page=' + context + ']');
            imageThumbnailContainer.innerHTML = imagesString;
        }

        window.setTimeout('fixIeDisplay()', 2000);

        //all downloaded, enable selectall link
        if (CorbisUI.GlobalVars.Lightbox.copyItemsPages == CorbisUI.GlobalVars.Lightbox.copyItemsPagesDownloaded) {
            $('copyProduct').getElement('a.selectAllLink').removeClass('disabled');
        }
    },

    disablePageInput: function() {
        new CorbisUI.Popup('blanket', {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false,
            showModalBackground: true,
            closeOnLoseFocus: false
        });
        $('modalOverlay').setStyle('opacity', 0.001);
    },

    enablePageInput: function() {
        HideModal('blanket');
    },
    openProgressModalOverlay: function(modalId, container) {
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false
        };
        new CorbisUI.Popup(modalId, options);
        $('modalOverlay').setStyle('opacity', 0.7);
    },
    openProgressModal: function(modalId, container) {
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false,
            backgroundElement: container
        };
        new CorbisUI.Popup(modalId, options);
    },
    openProgressModal: function(modalId) {
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false,
            backgroundElement: 'copyProduct'
        };
        new CorbisUI.Popup(modalId, options);
    },
    lightboxNameEdit: function(showEditControls) {
        var lightboxHeader = $('DetailsViewSide');
        if (showEditControls) {
            $('newLightboxName').value = lightboxHeader.getElement('span.LightboxNameSpan').get('text').substring(0, 40);
            $('lightboxNameEdit').removeClass('hdn');
            var lightboxNameSpan = lightboxHeader.getElement('span.LightboxNameSpan');
            if (lightboxNameSpan && !lightboxNameSpan.hasClass('hdn')) lightboxNameSpan.addClass('hdn');
            var lightboxRenameLink = lightboxHeader.getElement('a.renameLink');
            if (lightboxRenameLink && !lightboxRenameLink.hasClass('hdn')) lightboxRenameLink.addClass('hdn');
        }
        else {
            lightboxHeader.getElement('span.LightboxNameSpan').removeClass('hdn');
            lightboxHeader.getElement('a.renameLink').removeClass('hdn');
            var lightboxEdit = $('lightboxNameEdit');
            if (lightboxEdit && !lightboxEdit.hasClass('hdn')) lightboxEdit.addClass('hdn');
        }
    },
    editNotesLightbox: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
        $('notesGlassButton').setStyle('display', 'block');
        $('lightboxDetailsEdit').getElement('span.bulletPoint').setStyle('display', 'inline-block');
        var ClientName = $('detailStructure').getElement('input.txtName').value
        var newNotes = $('detailStructure').getElement('textarea.txtNote').value;
        var notesUid = $('detailStructure').getElement('input.notesUid').value;
        var lightboxName = $('lightboxesContent').getElement('.LightboxNameSpan').get('text');
        if (activeLightbox) {
            var lightboxId = activeLightbox.getProperty('id');
            Corbis.Web.UI.Lightboxes.LightboxScriptService.UpdateSharedLightbox(parseInt(lightboxId), lightboxName, ClientName, notesUid, newNotes, CorbisUI.Lightbox.Handler.updateNotesLightboxSucceeded, CorbisUI.Lightbox.Handler.methodFailed);
        }
    },

    updateNotesLightboxSucceeded: function(results) {
        var editLink = $('lightboxDetailsEdit').getElement('span.editLink');
        var hideLink = $('lightboxDetailsEdit').getElement('span.hideLink');
        editLink.style.display = hideLink.style.display = "block";
        var detailStructure = $('detailStructure');
        var clientName = detailStructure.getElement('input.txtName');
        var clientNameDisplay = detailStructure.getElement('span.lblName');
        var notes = detailStructure.getElement('textarea.txtNote');
        var notesDisplay = detailStructure.getElement('textarea.lblNotes');
        notesDisplay.value = notes.value;
        notesDisplay.style.display = "block";
        clientNameDisplay.set('text', clientName.value);
        clientNameDisplay.style.display = "block";

        clientName.style.display = "none";
        clientName.value = '';
        notes.style.display = "none";
        notes.value = '';
        $('notesGlassButton').setStyle('display', 'none');

        //update lightbox tree
        var activeLightboxId = CorbisUI.DomCache.get('ActiveLightbox').getProperty('id');
        var lightboxRow = $(activeLightboxId);
        var lightbox = lightboxRow.getParent();
        var parent = lightbox.getParent();
        var injectPosition;
        var injectParent;

        //see if it is a subfolder
        if (parent.hasClass('Children') || $('SortByDiv').getElement('select').value == 'name') {
            var currentLightbox = parent.getFirst();
            while (currentLightbox && (lightbox == currentLightbox || results[0].toLowerCase() > currentLightbox.getElement('div.LightboxRow').get('title').toLowerCase())) {
                currentLightbox = currentLightbox.getNext();
            }
            if (currentLightbox) {
                injectPosition = 'before';
                injectParent = currentLightbox;
            }
            else {
                injectPosition = 'bottom';
                injectParent = parent;
            }
        }
        else {
            injectPosition = 'top';
            injectParent = parent;
        }

        //   lightboxRow.setProperty('title', HtmlDecode(results));

        //update modifieddate in tree and note

        var modifiedDateEl = lightboxRow.getFirst().getLast();
        var modifiedDateNotes = $('detailStructure').getElement('span.textModified');

        var dateParts = modifiedDateEl.get('text').split(' ');
        modifiedDateEl.set('text', results[1]);
        modifiedDateNotes.innerHTML = results[1];
        //if the position is where the lightbox is, then don't need to move.
        if (lightbox != injectParent && lightbox != injectParent.getPrevious()) {
            lightbox.inject(injectParent, injectPosition);
        }
    },

    lightboxNotesCancel: function() {
        var bulletPoint = $('lightboxDetailsEdit').getElement('span.bulletPoint');
        var editLink = $('lightboxDetailsEdit').getElement('span.editLink');
        var hideLink = $('lightboxDetailsEdit').getElement('span.hideLink');
        var showLink = $('lightboxDetailsEdit').getElement('span.showLink');
        var lblName = $('DetailsViewSide').getElement('span.lblName');
        var lblNotes = $('DetailsViewSide').getElement('textarea.lblNotes');
        var txtName = $('DetailsViewSide').getElement('input.txtName');
        var txtNotes = $('DetailsViewSide').getElement('textarea.txtNote');
        var notesButtons = $('buttonsCenter').getElement('div.notesbuttonCSS');
        txtName.value = '';
        txtNotes.value = '';
        editLink.style.display = hideLink.style.display = lblName.style.display = lblNotes.style.display = $('detailStructure').style.display = "block";
        showLink.style.display = txtName.style.display = txtNotes.style.display = notesButtons.style.display = "none";
        bulletPoint.setStyle('display', 'inline-block');
    },

    renameLightbox: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
        var newLightboxName = String.trim($('newLightboxName').value);
        if (activeLightbox) {
            var oldLightboxName = String.trim(activeLightbox.getProperty('title'));
            if (newLightboxName.length == 0) { // check Renaming Lighbox with Empty Name
                $('newLightboxName').value = oldLightboxName;
            } else {
                if (oldLightboxName != newLightboxName) {
                    var lightboxId = activeLightbox.getProperty('id');
                    Corbis.Web.UI.Lightboxes.LightboxScriptService.RenameLightbox(parseInt(lightboxId), newLightboxName, CorbisUI.Lightbox.Handler.renameLightboxSucceeded, CorbisUI.Lightbox.Handler.methodFailed, lightboxId);
                }
                else {
                    CorbisUI.Lightbox.Handler.lightboxNameEdit(false);
                }
            }
        }
    },

    renameLightboxSucceeded: function(results, lightboxId) {
        if (results[0] && results[0] != '') {
            $('DetailsViewSide').getElement('span.LightboxNameSpan').set('html', HtmlFieldEncode(results[0]));

            //update lightbox tree
            var lightboxRow = $(lightboxId)
            var lightbox = lightboxRow.getParent();
            var parent = lightbox.getParent();
            var injectPosition;
            var injectParent;

            //see if it is a subfolder
            if (parent.hasClass('Children') || $('SortByDiv').getElement('select').value == 'name') {
                var currentLightbox = parent.getFirst();
                while (currentLightbox && (lightbox == currentLightbox || results[0].toLowerCase() > currentLightbox.getElement('div.LightboxRow').get('title').toLowerCase())) {
                    currentLightbox = currentLightbox.getNext();
                }
                if (currentLightbox) {
                    injectPosition = 'before';
                    injectParent = currentLightbox;
                }
                else {
                    injectPosition = 'bottom';
                    injectParent = parent;
                }
            }
            else {
                injectPosition = 'top';
                injectParent = parent;
            }

            lightboxRow.setProperty('title', results[0]);
            //have to set text first otherwise the angle bracket characters will not show.
            var lightboxNameSpan = lightboxRow.getElement('span.LightboxName');

            lightboxNameSpan.innerHTML = HtmlFieldEncode(results[0]);
            SetNameLength(lightboxNameSpan, CorbisUI.GlobalVars.Lightbox.sidbarWidth);
            var modifiedDateEl = lightboxRow.getFirst().getLast();
            var modifiedDateNotes = $('detailStructure').getElement('span.textModified');

            modifiedDateEl.set('text', results[1]);
            modifiedDateNotes.innerHTML = results[1];

            //if the position is where the lightbox is, then don't need to move.
            if (lightbox != injectParent && lightbox != injectParent.getPrevious()) {
                lightbox.inject(injectParent, injectPosition);
            }
        }

        CorbisUI.Lightbox.Handler.lightboxNameEdit(false);
    },

    methodFailed: function(results, context, methodName) {
        //reset the menu bar
        $('DetailsViewSideTop').getElements('div.selected').each(function(el) {
            el.removeClass('selected');
        });

        switch (methodName) {
            case 'CopyLightboxImages':
                //set event to deselect menu button 
                $('copySuccess').getElements('input').each(function(el) {
                    el.addEvent('click', function() { $(CorbisUI.GlobalVars.Lightbox.copyItemsButtonDiv).removeClass('selected'); });
                });

                HideModal('copyProgress');
                break;
            case 'TransferLightbox':
                HideModal('transferLightbox');
                break;
        };

        CorbisUI.Lightbox.Handler.OpenErrorModal(methodName);
    },

    OpenErrorModal: function(type) {
        var erroModal = $('errorModal');
        var errorTitle = CorbisUI.GlobalVars.Lightbox.errorTitles.get(type);
        var errorBody = CorbisUI.GlobalVars.Lightbox.errorBody.get(type);
        if (!errorTitle || errorTitle == '' || !errorBody || errorBody == '') {
            errorTitle = CorbisUI.GlobalVars.Lightbox.errorTitles.get('Default');
            errorBody = CorbisUI.GlobalVars.Lightbox.errorBody.get('Default');
        }

        erroModal.getElement('span.Title').set('html', errorTitle);
        erroModal.getElement('div.errorMessage').set('html', errorBody);

        OpenModal('errorModal');
    },

    setControlStates: function(activeLightbox) {
        if (activeLightbox) {
            ModifyCOFFOptions();
            var renameLink = $('DetailsViewSide').getElement('a.renameLink');
            var removeCheckbox = $('transferLightbox').getElement('div.imageCheckbox');
            var removeCheckboxInput = removeCheckbox.getElement('input');
            var moveButton = $('moveButton');
            var moveButtonAnchor = moveButton.getElement('a');
            var detailsEdit = $('lightboxDetailsEdit');
            var editLink = detailsEdit.getElement('span.editLink');
            var bulletPoint = detailsEdit.getElement('span.bulletPoint');
            //disable all delete and notes icons on thumbnail for readonly Lightboxes
            var thumbnailDeletebuttons_array = $$('#LightboxProducts div.closeIcon');
            var thumbnailDeleteNotebuttons_array = $$('#LightboxProducts div.noteIcon');
            var thumbnailInput_array = $$('#LightboxProducts input.hovable');
            if (activeLightbox.getProperty('shared') == 'True') {
                if (!renameLink.hasClass('hdn')) renameLink.addClass('hdn');
                if (!removeCheckbox.hasClass('disabled')) removeCheckbox.addClass('disabled');
                removeCheckboxInput.setProperty('disabled', 'disabled');
                if (!moveButton.hasClass('disabled')) moveButton.addClass('disabled');
                if (!moveButtonAnchor.hasClass('disabled')) moveButtonAnchor.addClass('disabled');
                moveButtonAnchor.setProperty('disabled', 'disabled');
                moveButton.setProperty('title', CorbisUI.GlobalVars.Lightbox.moveDisabledMessage);
                //  if (!thumbnailDeletebuttons_array.hasClass('disabled')) {
                //                    thumbnailDeletebuttons_array.each(function(div) { 
                //                        thumbnailDeletebuttons_array.addClass('disabled');})
                //     }
                thumbnailInput_array.setProperty('disabled', 'disabled');
                thumbnailInput_array.addClass('hdn'); // must add hdn to make tooltip works, weired.

                thumbnailDeletebuttons_array.addClass('disabled');
                thumbnailDeletebuttons_array.setProperty('title', CorbisUI.GlobalVars.Lightbox.deleteDisabledMessage);
                thumbnailDeleteNotebuttons_array.addClass('disabled');
                thumbnailDeleteNotebuttons_array.setProperty('title', CorbisUI.GlobalVars.Lightbox.notesDisabledMessage);

                if (!editLink.hasClass('hdn')) editLink.addClass('hdn');
                if (!bulletPoint.hasClass('hdn')) bulletPoint.addClass('hdn');
            }
            else {
                renameLink.removeClass('hdn');
                removeCheckbox.removeClass('disabled');
                removeCheckboxInput.removeProperty('disabled');
                moveButton.removeClass('disabled');
                moveButtonAnchor.removeClass('disabled');
                moveButtonAnchor.removeProperty('disabled');
                thumbnailDeletebuttons_array.removeClass('disabled');
                //thumbnailDeletebuttons_array.removeProperty('title', CorbisUI.GlobalVars.Lightbox.deleteDisabledMessage);
                thumbnailDeleteNotebuttons_array.removeClass('disabled');
                //thumbnailDeleteNotebuttons_array.removeProperty('title', CorbisUI.GlobalVars.Lightbox.notesDisabledMessage);
                thumbnailInput_array.removeProperty('disabled');
                moveButton.removeProperty('title', CorbisUI.GlobalVars.Lightbox.moveDisabledMessage);
                editLink.removeClass('hdn');
                bulletPoint.removeClass('hdn');
            }
        }
    },

    openTransferModal: function(transferButton) {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');

        if (activeLightbox && !$(transferButton).hasClass('disabled')) {
            //create transfer modal object
            var transferModal = CorbisUI.Lightbox.transferLightbox(true);

            //add events handler for showing and hiding menu button.
            transferModal.addEvent('show', function() {
                var transferLightbox = $('transferButton');
                if (!transferLightbox.hasClass('selected')) transferLightbox.addClass('selected');
            });

            transferModal.addEvent('hide', function() {
                $('transferButton').removeClass('selected');
            });

            transferModal.showModal();
            transferModal.initTransferValidation('transferLightbox');
        }
    },
    validateSelectedItemsForCoff: function() {
        var activeLightbox = CorbisUI.DomCache.get('ActiveLightbox');
        if (activeLightbox) {
            CorbisUI.Lightbox.CoffImages().resetSelection();
            var coffItemsButton = $(CorbisUI.GlobalVars.Lightbox.coffItemsButtonDiv);
            //return if copy button is disabled
            if (coffItemsButton.getElement('a.disabled')) {
                return;
            }

            var coffProduct = $('coffProduct');
            var lightboxId = activeLightbox.get('id');

            //Make the button selected
            if (!coffItemsButton.hasClass('selected')) coffItemsButton.addClass('selected');

            var imagesContainer = $('coffLightboxImages');
            var loadedLightbox = imagesContainer.getProperty('lightboxId');
            //Calculate image paging and get images, but only if not loaded or it's different.
            if (!loadedLightbox || loadedLightbox != lightboxId) {
                //Set the title
                var lightboxName = $('lightboxesContent').getElement('.LightboxNameSpan').get('text')
                var titleSpan = coffProduct.getElement('span.Title');
                var totalItems = $('header2').getElement('input.totalItems').value;
                //reset link to select all
                $('coffProduct').getElement('a.selectAllLink').set('text', $('coffProduct').getElement('a.selectAllLink').getProperty('select'));
                var rowHeight = 148;
                //Calculate and get image pages
                CorbisUI.GlobalVars.Lightbox.coffItemsPagesDownloaded = 0;
                var pageHeight = CorbisUI.GlobalVars.Lightbox.coffItemPageSize / 5 * rowHeight;
                var noOfPages = Math.floor(totalItems / CorbisUI.GlobalVars.Lightbox.coffItemPageSize);
                var lastPageImageCount = totalItems - (noOfPages * CorbisUI.GlobalVars.Lightbox.coffItemPageSize);
                var lastPageHeight = 0;
                var pageNumber = 0;

                if (lastPageImageCount > 0) {
                    noOfPages++;
                    lastPageHeight = Math.ceil(lastPageImageCount / 5) * rowHeight;
                }
                //just use 1 page
                noOfPages = 1;
                imagesContainer.scrollTo(0, 0);
                coffProduct.getElement('a.selectAllLink').addClass('disabled');

                var imagesPagesString = "";
                CorbisUI.GlobalVars.Lightbox.coffItemsPages = noOfPages;

                //Creating the blank page lists first
                noOfPages.times(function(index) {
                    imagesPagesString += String.format('<ul page="{0}" style="height:{1}px;"></ul>', pageNumber, pageNumber + 1 == noOfPages && lastPageHeight > 0 ? lastPageHeight : pageHeight);
                    pageNumber++;
                });
                imagesContainer.set('html', imagesPagesString);
                //set the lightbox id so we can keep track of what lightbox is loaded.
                imagesContainer.setProperty('lightboxId', lightboxId);
                //Populate the page lists
                for (var i = 0; i < noOfPages; i++) {
                    //window.setTimeout(String.format('Corbis.Web.UI.Lightboxes.LightboxScriptService.GetLightboxCOFFImages(\'{0}\', {1}, CorbisUI.GlobalVars.Lightbox.copyItemPageSize, CorbisUI.Lightbox.Handler.getLightboxCOFFImagesSucceeded, CorbisUI.Lightbox.Handler.methodFailed, {2})', lightboxId, i + 1, i), 0);
                    window.setTimeout(String.format('Corbis.Web.UI.Lightboxes.LightboxScriptService.GetLightboxCOFFImages(\'{0}\', {1}, {3}, CorbisUI.Lightbox.Handler.getLightboxCOFFImagesSucceeded, CorbisUI.Lightbox.Handler.methodFailed, {2})', lightboxId, i + 1, i, totalItems), 0);
                }
            }

            //Show the modal
            var coffProductModal = new CorbisUI.Popup('coffProduct', {
                createFromHTML: false,
                showModalBackground: true,
                closeOnLoseFocus: false,
                centerOverElement: $('DetailsViewSide'),
                positionVert: 'center',
                positionHoriz: 'center',
                width: 850,
                height: 500,
                onHide: function() { $(CorbisUI.GlobalVars.Lightbox.coffItemsButtonDiv).removeClass('selected'); }
            });

            ResizeModal('coffProduct');

            //set event to deselect menu button
            coffProduct.getElement('input.Close').addEvent('click', function() { $(CorbisUI.GlobalVars.Lightbox.coffItemsButtonDiv).removeClass('selected'); });
        }
    },

    getLightboxCOFFImagesSucceeded: function(result, context) {
        CorbisUI.Lightbox.CoffImages();
        var lighboxImages = $('coffLightboxImages');

        if (result) {
            var imagesString = "";
            var templatestring = '<li imageuid="{0}" productuid="{4}" onclick="CorbisUI.Lightbox.CoffImages().selectItem(event, this);"><div class="imageWrap"><img src="{1}" title="{2}" style="{3}"/></div><div class="displayWrap{5}"><span class="floatLeft LicenseModel{5}">{5}</span><span class="CorbisuidStyle">{6}</span></div></li>';
            result.each(function(image) {
                var containerHeight = 128;
                var containerWidth = 128;
                var width = containerWidth;
                var height = containerHeight;
                var marginTop = 0;
                var marginBottom = 0;
                if (image.AspectRatio > 1) {
                    height = containerWidth / image.AspectRatio;
                    marginTop = (containerHeight - height) / 2;
                    marginBottom = marginTop;
                }
                else {
                    width = containerHeight * image.AspectRatio;
                }
                var imageStyle = 'margin-top: ' + marginTop + 'px; width: ' + width + 'px; height: ' + height + 'px;' + 'margin-bottom: ' + marginBottom + 'px;';
                // imagesString += String.format(templatestring, image.MediaUid, image.Url128, image.CorbisId + ' ' + HtmlEncode(image.Title), imageStyle, image.ProductUid, image.LicenseModelText, image.CorbisId);
                imagesString += String.format(templatestring, image.MediaUid, image.Url128, image.CorbisId + ' ' + HtmlEncode(image.Title), imageStyle, image.ProductUid, image.LicenseModelText, image.CorbisId);
            });

            var imageThumbnailContainer = lighboxImages.getElement('ul[page=' + context + ']');
            imageThumbnailContainer.innerHTML = imagesString;
        }
        //all downloaded, enable selectall link
        if (CorbisUI.GlobalVars.Lightbox.coffItemsPages == CorbisUI.GlobalVars.Lightbox.coffItemsPagesDownloaded) {
            $('coffProduct').getElement('a.selectAllLink').removeClass('disabled');
            window.setTimeout('fixIeDisplay()', 2000);
        }
    },

    updateLightboxInfo: function(lightboxId, itemCountText, modifiedText, countSeperator) {
        var lightbox = $(lightboxId);
        var itemCountSpan = lightbox.getElement('span.imageCount');

        if (lightbox) {
            if (itemCountSpan.getParent().get('text') != itemCountSpan.get('text')) itemCountText += countSeperator;
            itemCountSpan.set('text', itemCountText);
            lightbox.getElement('div.modifiedDate').set('text', modifiedText);
        }
    }
}

/***************************
    Helpers
****************************/
function appendToString(element, index, array) {
    // "this" is the context parameter, i.e. ''.
    result += element + this + index + '';
}

function centerInStr2(inStr, outStr, extra2right) {
    var inSize = inStr.length;
    var outSize = outStr.length;
    var len = 50;
    var re;
    var rtrnVal;

    if (inSize <= outSize) {
        if (extra2right)
            re = new RegExp("(.{" + len + "})(.{" + inSize + "})(.*)");
        else
            re = new RegExp("(.*)(.{" + inSize + "})(.{" + len + "})");

        rtrnVal = outStr.replace(re, "$1" + inStr + "$3");
    } else {
        rtrnVal = extractMiddle(inStr, outSize, extra2right);
    }

    return rtrnVal;
} //eof centerInStr2

CorbisUI.Lightbox.CreatePinkyObject = function(image, productUid){

        var el = new Element('div',{
            id: 'pinky_'+productUid
        }).addClass('pinkyThumb');
        
        var pinkyWrap = new Element('div').addClass('pinkyWrap').inject(el);
        
        var newImage = image.clone()
                        .setStyle('margin-top',0)
                        .inject(pinkyWrap);
       
        newImage.setStyles(CorbisUI.Lightbox.ScaleImage(image,{marginTop: 90, width: 90, height: 90}));
        // return the pinky dom object
        return el;

}

CorbisUI.Lightbox.ScaleImage = function(ele,options){
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

/***************************
    WINDOW DOM READY
****************************/

window.addEvent('domready', function() {
    CorbisUI.DomCache.add([
		'Tree',
		'LBXContainer'
	]);

    var lightboxTree = CorbisUI.DomCache.get('Tree');
    if (lightboxTree) {
        CorbisUI.DomCache.addObject('ActiveLightbox', lightboxTree.getElement('div.Active'));
    }

    registerLightboxTooltips();

    // setup search buddy
    CorbisUI.MyLightboxes.init = new CorbisUI.MyLightboxes.init();
    if (!window.location.href.toLowerCase().contains('emaillightboxview.aspx')) {
        // initialize search buddy
        Floater = new CorbisUI.MyLightboxes.floater($('SearchBuddy'), $('MainContent'), $('FooterContent'));

        var NLB = $('NewLightboxButton');
        NLB.store('activated', false);

        //IE can use text-overflow style so don't need this.
        if (!Browser.Engine.trident) {
            TruncateNames(lightboxTree.getElements('span.LightboxName'));
        }
    };

    LoadActiveLightbox();

    //horizontally align active lightbox
    if (lightboxTree != null) {
        var activelightbox = CorbisUI.DomCache.get('ActiveLightbox');
        var treePosition = lightboxTree.getPosition();
        var lightboxPosition = activelightbox.getPosition();
        var lightboxConatiner = CorbisUI.DomCache.get('LBXContainer');
        if (activelightbox) {
            lightboxConatiner.scrollLeft = (lightboxPosition.left - lightboxPosition.left);
            lightboxConatiner.scrollTop = (lightboxPosition.top - lightboxPosition.top);
            CorbisUI.Lightbox.Handler.setControlStates(activelightbox);
        }
    }
});

window.addEvent('load', function() {
    ModifyDisplayOptions();
});
