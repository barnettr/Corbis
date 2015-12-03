if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}

function OpenModal(divId) {
    /// <summary> Mike: be careful with this one. Try to use OpenModal2.
    ///  this OpenModal will be depricated later.
    /// It is not compatible with UpdatePanel, because "createFromHTML= true"
    /// </summary>
    new CorbisUI.Popup(divId, { 
        showModalBackground: true,
        closeOnLoseFocus: false });
}
function OpenModal2(divId, backgroundDivId) {
    /// <summary> this is to open a popup on another popup </summary>
    /// <param name="divId"> this is the div id of current popup</param>
    /// <param name="backgroundDivId"> this is the background div id </param>
    /// <returns> none </returns>
    if (MochaUI.ModalExists(divId)) {
        MochaUI.ShowModal(divId);
    } else {
        new CorbisUI.Popup(divId, {
            showModalBackground: true,
            closeOnLoseFocus: false,
            createFromHTML: false,
            backgroundElement: backgroundDivId
        });
    }
    
    ResizeModal(divId);
}

function OpenCloseWarning(divId, opener) {

    new CorbisUI.Popup(divId, { showModalBackground: false, closeOnLoseFocus: true, centerOverElement: opener, positionVert: 'bottom', positionHoriz: 'left'  } );
}

function HideModal(divId) {
    MochaUI.HideModal(divId);
}

function CloseModal(divId) {
    MochaUI.CloseModal(divId);
}

function ResizeModal(divId) {
    if ($(divId + "Window")) {
        MochaUI.dynamicResize($(divId + "Window"));
    }
}
function ResizeIModalXY(divId, w, h) {
    if ($(divId + 'Window')) {
        $(divId + 'Window').setStyle('width', w + 'px');
        $(divId + 'Window_iframe').setStyle('width', w + 'px');
        $(divId + 'Window_contentWrapper').setStyle('width', w + 'px');
        $(divId + 'Window').setStyle('height', (h + 23) + 'px');
        $(divId + 'Window_iframe').setStyle('height', h + 'px');
        $(divId + 'Window_contentWrapper').setStyle('height', h + 'px');
        ResizeModal(divId);
    }
}

function ResizeIModal(divId, height) {
    if ($(divId + 'Window')) {
        $(divId + 'Window').setStyle('height', (height + 23) + 'px');
        $(divId + 'Window_iframe').setStyle('height', height + 'px');
        $(divId + 'Window_contentWrapper').setStyle('height', height + 'px');
        var currentInstance = MochaUI.Windows.instances.get($(divId + 'Window').id);
        var contentWrapperEl = currentInstance.contentWrapperEl;
        var contentEl = currentInstance.contentEl.childNodes[0];
        contentWrapperEl.setStyle('height', contentEl.offsetHeight);
        currentInstance.drawWindow($(divId + 'Window'));
    }
}

function OpenIModal(source, modalWidth, modalHeight)
{
    OpenNewIModal(source, modalWidth, modalHeight, 'pricing');
}

function OpenNewIModal(source, modalWidth, modalHeight, windowID)
{
    windowID = windowID + 'Window';
    // If the windowID object is there, don't open again.
    if ($(windowID) != null) {
        //console.log('window exists');
        return;
    }
    if (!$('modalOverlay')) {
        MochaUI.Modal = new MochaUI.Modal();
    }
    new MochaUI.Window({
		id: windowID,
		title: '',
		loadMethod: 'iframe',
		contentBgColor: '#dbdbdb',
		useCanvasControls: false,
		headerStartColor:[219,219,219],
		headerStopColor:[219,219,219],
		footerStartColor:[219,219,219],
		footerStopColor: [219, 219, 219],
		bodyBgColor: [219, 219, 219],
		cornerRadius: 5,
		headerHeight: 8,
		footerHeight: 5,
		padding: 0,
		collapsible: false,
		minimizable: false,
       	scrollbars: false,
       	closable: false,
		contentURL: source,
		type: 'modal',
		width: modalWidth,
		height: modalHeight
        ,
        onContentLoaded: function(id, source) {
            var instance = MochaUI.Windows.instances.get(id);
            //MochaUI.focusWindow($(id), true);
            //console.log('on CONTENT LOADED');
            //console.log(instance.options.contentURL);
            //if (!Browser.Engine.trident) {
                // set automagically the focus event
                if (!instance.iframeEl.hasEvent('focusMe')) {
                    // add custom event to be fired when opened
                    instance.iframeEl.addEvent('focusMe', iFrame_windowFocus.bind(instance));
                    // hijack attempts to click away from iframe
                    // if user clicks off iframe, this will fire and refocus on first element again
                    //if(!Browser.Engine.trident)instance.iframeEl.addEvent('blur', iFrame_windowFocus.bind(instance));
                }
                // fire focus event on non-https pages.
                //
                // NOTE: for https pages you will need to
                // put a focus event on page load
                // (see bottom of ExpressCheckout.js)
                if (!Browser.isHttps(instance.options.contentURL)) instance.iframeEl.fireEvent('focusMe');
            //}
        } .pass(windowID, source).bind(window)/*,
        onBeforeBuild: function(id, source) {
            console.log('on BEFORE BUILD');
        } .pass(windowID, source).bind(this),
        onFocus: function() {
            console.log('on FOCUS');
        } .pass(windowID, source).bind(this),
        onBlur: function() {
            console.log('on BLUR');
        } .pass(windowID, source).bind(this),
        onResize: function() {
            console.log('on RESIZE');
        } .pass(windowID, source).bind(this),
        onMinimize: function() {
            console.log('on MINIMIZE');
        } .pass(windowID, source).bind(this),
        onMaximize: function() {
            console.log('on MAXIMIZE');
        } .pass(windowID, source).bind(this),
        onRestore: function() {
            console.log('on RESTORE');
        } .pass(windowID, source).bind(this),
        onClose: function(id, source) {
            console.log('on CLOSE');
        } .pass(windowID, source).bind(this),
        onCloseComplete: function() {
            console.log('on CLOSE COMPLETE');
        } .pass(windowID, source).bind(this)*/
	});
    $('modalOverlay').setStyle('opacity', 0.4);
    var scrollPos = document.getScroll();
    var styleTop = scrollPos.y + (window.getSize().y/2) - ($(windowID).getCoordinates().height/2);
    if (styleTop < 10) { styleTop = 10; }
    $(windowID).setStyle('top', styleTop);
}
//  **********************************************************
//  TODO: We have rolled back the changes on bug #18338. If we
//  decide to use this function for Enlargement centering of the Pricing Module
//  it is here
//  **********************************************************
//function OpenPricingIModal(source, modalWidth, modalHeight) {
//    OpenNewPricingIModal(source, modalWidth, modalHeight, 'pricing');
//}

//function OpenNewPricingIModal(source, modalWidth, modalHeight, windowID) {
//    windowID = windowID + 'Window';
//    // If the windowID object is there, don't open again.
//    if ($(windowID) != null) {
//        console.log('window exists');
//        return;
//    }
//    if (!$('modalOverlay')) {
//        MochaUI.Modal = new MochaUI.Modal();
//    }
//    new MochaUI.Window({
//        id: windowID,
//        title: '',
//        loadMethod: 'iframe',
//        contentBgColor: '#dbdbdb',
//        useCanvasControls: false,
//        headerStartColor: [219, 219, 219],
//        headerStopColor: [219, 219, 219],
//        footerStartColor: [219, 219, 219],
//        footerStopColor: [219, 219, 219],
//        bodyBgColor: [219, 219, 219],
//        cornerRadius: 5,
//        headerHeight: 8,
//        footerHeight: 5,
//        padding: 0,
//        collapsible: false,
//        minimizable: false,
//        scrollbars: false,
//        closable: false,
//        contentURL: source,
//        type: 'modal',
//        x: 145,
//        y: 30,
//        width: modalWidth,
//        height: modalHeight
//        ,
//        onContentLoaded: function(id, source) {
//            var instance = MochaUI.Windows.instances.get(id);
//            console.log('on CONTENT LOADED');
//            console.log(instance.options.contentURL);
//            if (!instance.iframeEl.hasEvent('focusMe')) {
//                instance.iframeEl.addEvent('focusMe', iFrame_windowFocus.bind(instance));
//            }
//            if (!Browser.isHttps(instance.options.contentURL)) instance.iframeEl.fireEvent('focusMe');
//        } .pass(windowID, source).bind(window)
//    });
//    $('modalOverlay').setStyle('opacity', 0.4);
//    $(windowID).setStyles({
//        'position': 'absolute',
//        'left': 145,
//        'top': 30
//    });
//}
// MAGIC FOCUSER FOR IFRAMES
function iFrame_windowFocus(e) {
    //alert('CALLING: iFrame_windowFocus');
    try {
        var item;
        if (window.frames.length > 1) {
            $each(window.frames, function(el, idx) {
                if (el.frameElement.name == this.iframeEl.name) {
                    item = $(el.document);
                }
                //console.log(el);
            }, this);
        } else {
            item = $(window.frames[0].document);
        }
        //if(item == null) item = $(window.window[this.name].document);
        //    try {
        var form = $(item.forms[0]);
        var Body = $(window.frames[0].document.body);
        //        var tempInput = window.frames[0].CorbisUI.createDummyInput().inject(Body);
        //        tempInput.focus();
        //        form.focus();
        //        tempInput.destroy();
        //    } catch (e) { }
        var formDoc = $try(
            function() {
                //alert('option 1');
                // testing for pricing iframe modals
                return form.getElement('input[id$=cartButton_GlassButton]');
            },
            function() {
                //alert('option 2');
                // other standard iframe modals like credit card
                return form.getElement('div.titleWrapper').getNext('div').getElement('select');
            },
            function() {
                //alert('option 3');
                // other standard iframe modals like credit card
                return form.getElement('div.titleWrapper').getNext('div').getElement('input[type != hidden]');
            },
            function() {
                //alert('option 4');
                // other standard iframe modals like credit card
                return form.getElement('div.titleWrapper').getNext('div').getElement('div.GlassButton input');
            },
            function() {
                // uh oh...nothing could be found
                //alert('crap. Its false');
                return false;
            }
        );
        if (formDoc) {
            var item = formDoc;
            //        .getNext('div')
            //        .getElement('input[type!=hidden]');
            //        alert(item);
            item.focus();
        }
    } catch (e) {}
}


// Pop Up Window for Enlarge Image from Search Page
function EnlargeImagePopUp(URL)
{
	popupHeight = 618;
	popupWidth = 980;

	popupLeft = Math.floor((parent.window.screen.width/2) - (popupWidth/2));
	popupTop = Math.floor((parent.window.screen.height/2) - (popupHeight/2));

	var caller = $pick(URL.match(new RegExp('(caller=)([^?&]*)', 'i')), new Array('', '', ''))[2];
	var corbisId = $pick(URL.match(new RegExp('(id=)([^?&]*)', 'i')), new Array('', '', ''))[2];
    var pager = $('Pager');
    var imageList;
    var totalItems;
    var pageSize;
    var pageNo;
    var searchQuery;
    var imageIndex;
    var lightboxId;

    switch (caller) {
        case 'search':
        case 'imagegroups':
            //set up parameters that's needed
            var pager = $('Pager');

            if (pager) {
                //set up parameters that's needed
                totalItems = $('Pager').getElement('input.totalItems').value;
                imageList = $('ProductResults').getElements('span.ProductBlock').map(function(el, index) {
                    return el.getProperty('corbisID');
                });

                pageSize = $('Pager').getElement('input.pageSize').value;
                pageNo = $('Pager').getElement('input.NavPageNumber').value;
            }
            searchQuery = window.location.href.substring(window.location.href.indexOf('?') + 1);
            break;
        case 'lightbox':
            //setting up from lightbox buddy of search page, image groups or media search
            if (window.location.href.test('searchresults.aspx', 'i') ||
                window.location.href.test('imagegroups.aspx', 'i') ||
                window.location.href.test('mediasetsearch.aspx', 'i')) {
                var lightboxContainer = $('LBXContainer');

                //Getting total count from attribute rather than length because we can't gaurantee the lightbox is completely loaded.
                totalItems = lightboxContainer.getProperty('imageCount');

                var currentIndex = lightboxContainer.getElements('div.lightboxBlock').map(function(el, index) {
                    return el.getProperty('corbisid');
                }).indexOf(corbisId);

                //lightbox buddy images are reverse ordered.
                imageIndex = totalItems - currentIndex;

                //Will default page size to 50 and since we do not know if page completely loaded, set image list to null to load servers-side.
                lightboxId = $('SearchBuddy').getElement('select.lightboxList').value
            }
            //setting up lightbox page
            else if (window.location.href.test('MyLightboxes.aspx', 'i') || window.location.href.test('emailLightboxview.aspx', 'i')) {
                totalItems = $('Pager').getElement('input.totalItems').value;
                imageList = $('LightboxProducts').getElements('span.ProductBlock').map(function(el, index) {
                    return el.getProperty('corbisID');
                });

                pageSize = $('Pager').getElement('input.pageSize').value;
                pageNo = $('Pager').getElement('input.NavPageNumber').value
                
                if (window.location.href.test('emailLightboxview.aspx', 'i')) {
                    lightboxId = $(CorbisUI.GlobalVars.Lightbox.lightboxId).value;
                }
                else {
                    lightboxId = $('Tree').getElement('div.Active').id;
                }
            }

            break;
        case 'mediaset':
            var mediaSetId = $pick(URL.match(new RegExp('(mediasetid=)([^?&]*)', 'i')), new Array('', '', ''))[2];
            var mediaSet = $('relatedImageGroups').getElement('ul[mediasetid=' + mediaSetId + ']');
            var mediaSetItems = mediaSet.getElements('img');

            totalItems = mediaSetItems.length;
            imageList = mediaSetItems.map(function(el, index) {
                return el.getProperty('title');
            });

            pageSize = totalItems;
            pageNo = 1;

            break;
        case 'quickpic':
            lightboxItems = $('quickPicsContainer').getElements('div.quickPicBlock');
            totalItems = lightboxItems.length;
            imageList = lightboxItems.map(function(el, index) {
                return el.getProperty('corbisid');
            });

            pageSize = totalItems;
            pageNo = 1;

            break;
        default:
            break;
    }
    
    //window.open should have unique id for each window.
    var windowId = (new Date()).getTime().toString();
    var form = $('enlargementPostForm');
    if (!form) {
        form = new Element('form');
        form.setProperty('id', 'enlargementPostForm');
        form.setProperty('name', 'enlargementPostForm');
        form.setProperty('method', 'post');
        form.inject($(window.document.body));
    }
    form.setProperty('target', windowId);
    form.setProperty('action', URL);

    GetHiddenControl('totalItems', form).value = totalItems;
    GetHiddenControl('imageList', form).value = imageList;
    GetHiddenControl('pageSize', form).value = pageSize;
    GetHiddenControl('pageNo', form).value = pageNo;
    GetHiddenControl('imageIndex', form).value = imageIndex;
    GetHiddenControl('lightboxId', form).value = lightboxId;
    GetHiddenControl('searchQuery', form).value = searchQuery;

    //Open form just before we submit
    window.open('', windowId, String.format('titlebar=0, toolbar=0, scrollbars=1, location=0, status=0, menubar=0 , resizable=1, height={0}, width={1}, left={2}, top={3}', popupHeight, popupWidth, popupLeft, popupTop))
	form.submit();
}

function GetHiddenControl(controlId, parent) {
    var controlToGet = parent.getElement('#' + controlId);
    if (!controlToGet) {
        controlToGet = new Element('input', {
            'type': 'hidden',
            'id': controlId,
            'name': controlId
        });
        controlToGet.inject(parent);
    }

    return controlToGet;
}

function OpenHtmlModal(windowId, html, options)
{
	windowId += 'Window';
	
	var properties = {
		title: '',
		collapsible: false,
		minimizable: false,
		contentBgColor: '#E8E8E8',
		useEffects: false,
		useCanvasControls: false,
		headerStartColor: [219, 219, 219],
		headerStopColor: [219, 219, 219],
		footerStartColor: [219, 219, 219],
		footerStopColor: [219, 219, 219],
		cornerRadius: 5,
		headerHeight: 8,
		footerHeight: 5,
		padding: 0,
		scrollbars: false,
		closable: false,
		type: 'modal',
		id: windowId,
		showModalBackground: false,
		closeOnLoseFocus: true, 
		loadMethod: 'html',
		content: html
	};	
	
	if (options) properties =  $merge(properties, options);

	new MochaUI.Window(properties);
	
	var scrollPos = document.getScroll();
	var styleTop = scrollPos.y + (window.getSize().y/2) - ($(windowId).getCoordinates().height/2);
	if (styleTop < 10) { styleTop = 10; }
	$(windowId).setStyle('top', styleTop);
}

var EnlargementManager = new Class(
{
    hash: null,

    initialize: function() {
        this.hash = new Hash({});
    },

    addWindow: function(childWin) {
        this.hash.set(childWin.location.href, childWin);
    },

    removeWindow: function(childWin) {
        if (this.hash.has(childWin.location.href)) this.hash.erase(childWin.location.href);
    },

    minimizeAll: function() {
        // IE
        if (Browser.Engine.trident) {
            this.hash.each(function(value, key) {
                if (value && !value.closed) {
                    value.IsJsMinimized = 'Y';
                    value.resizeTo(value.MinimumWidth, value.MinimumHeight);
                    value.moveTo(screen.width, screen.height);
                }
            });
        }
        else {
            // NON IE
            this.hash.each(function(value, key) {
                if (value && !value.closed) {
                    value.IsJsMinimized = 'Y';
                    value.resizeTo(value.MinimumWidth, value.MinimumHeight);
                    value.moveTo(screen.width, screen.height);
                }
            });
        }
    },

    closeAll: function() {
        this.hash.each(function(value, key) {
            value.close();
            this.hash.erase(key);
        }, this);
    }

});

CorbisUI.QueueManager.afterDomReady.addItem('windowManagerSetup', function() {
    EnlargementManager = new EnlargementManager();
});

CorbisUI.ContactCorbis = {
    contactCorbisOption: null,

    ShowContactCorbisModal: function(elementToCenterOver) {
        Corbis.Web.UI.CustomerService.CustomerServiceWebService.GetContactCorbisOffice(CorbisUI.ContactCorbis.GetContactCorbisOfficeSucceeded, CorbisUI.ContactCorbis.GetContactCorbisOfficeFailed, elementToCenterOver);
    },

    GetContactCorbisOfficeSucceeded: function(result, context) {
        //add the required stylesheet.
        var windowDocument = $(document);
        if (!windowDocument.getElement('head').getElement('link[id=contactCorbisCss]')) {
            var css = new Element('link', {
                id: 'contactCorbisCss',
                rel: 'stylesheet',
                type: 'text/css',
                href: '../StyleSheets/ContactCorbis.css'
            });

            css.inject(windowDocument.getElement('head'));
        }

        //Fill in the contact info
        var contactInfo = $('contactInfo');
        if (result.UseDefaultOfficeInfo) {
            if (!contactInfo.hasClass('defaultOffice')) contactInfo.addClass('defaultOffice');
            contactInfo.set('html', result.DefaultOfficeInfo);
        }
        else {
            contactInfo.removeClass('defaultOffice');
            var addressTemplate = $('contactAddressTemplate').get('html');
            contactInfo.set('html', String.format(addressTemplate, result.ContactName, result.OfficeAddress, result.OfficePhoneNumber, result.OfficeFaxNumber, result.EmailAddress));
            //attribute values get escaped, {4} becomes %7B4%7D so we set the href here instead.
            contactInfo.getElement('a').set('href', 'mailto:' + result.EmailAddress);
            if (!result.ContactName || result.ContactName == '') {
                contactInfo.getElement('tr').addClass('hdn');
            }
        }

        var options = {
            showModalBackground: false,
            closeOnLoseFocus: true,
            createFromHTML: false,
            centerOverElement: $(context),
            bodyBgColor: [219, 219, 219]
        };
        var linkOptions = null;

        switch (context.id) {
            case 'enlargementContactLink':
                //setting position after modal is visible as we need to center it to the parent modal after the modal is resized.
                break;
            case 'rmPricingContactLink':
            case 'cartFooter':
            case 'accountMultipleError':
            case 'accountSingleError':
            case 'promoCodeError':
                linkOptions = { positionVert: 'top', positionHoriz: 'left' };
                break;
            case 'filesizeContactLink':
                linkOptions = { positionVert: 'top', positionHoriz: -200 };
                //adding this event because modal overlay is already attached to FileSize modal.
                $('fileSizeModalWindow').addEvent('click', function() { HideModal('contactCorbisModal'); });
                break;
            case 'rfPricingContactLink':
            case 'rsPricingContactLink':
            case 'learnMoreContactLink':
                linkOptions = { positionVert: 'top', positionHoriz: 'right' };
                break;
            case 'quickPicContactLink':
                linkOptions = { positionVert: 15, positionHoriz: 'left' };
                break;
            case 'feedbackContactLink':
                linkOptions = { positionVert: 15, positionHoriz: -133 };
                //adding this event because modal overlay is already attached to feedbackPopup modal
                if (context.id == 'feedbackContactLink') {
                    $('feedbackPopupWindow').addEvent('click', function() { HideModal('contactCorbisModal'); });
                }
                break;
            case 'quickPicTermsContactLink':
                linkOptions = { positionVert: 'top', positionHoriz: 'right' };
                break;
            case 'requestPriceContactLink':
                linkOptions = { positionVert: 35, positionHoriz: 'middle' };
                break;
        }

        options = $merge(linkOptions, options);

        CorbisUI.ContactCorbis.contactCorbisOption = options;

        (function() {
            new CorbisUI.Popup('contactCorbisModal', options);
            ResizeModal('contactCorbisModal');

            //For cases in pricing where the modal can ended up behind the learn more modal.
            var contactModalStyles = {'z-index': 11100};

            //image feedback is a dialog modal, so need to adjust the background opacity
            if (context.id == 'feedbackContactLink') {
                $('modalOverlay').setStyle('opacity', '0.4');
            }

            //Due to postion of contact link differing due to localization and the size of the parent window we are going to center the contact window for this
            //we also have to do this heere because the height of the contact modal is dynamic
            if (context.id == 'enlargementContactLink') {
                contactModalStyles = $merge(contactModalStyles, CorbisUI.ContactCorbis.GetCenterPosition($('viewDimensionsModalWindow')));
            }
            
            $('contactCorbisModalWindow').setStyles(contactModalStyles);
        }).delay(100);
    },

    GetCenterPosition: function(parentWindow) {
        var parentWindowPosition = parentWindow.getCoordinates();
        var contactModalPosition = $('contactCorbisModal').getCoordinates(); ;

        var horizontalposition = parentWindowPosition.left + ((parentWindowPosition.width - contactModalPosition.width) / 2);
        var verticalPosition = parentWindowPosition.top + ((parentWindowPosition.height - contactModalPosition.height) / 2);

        return { top: verticalPosition, left: horizontalposition };
    },

    GetContactCorbisOfficeFailed: function(result, context) {
        //TODO: determine what to do here.
    }
};
