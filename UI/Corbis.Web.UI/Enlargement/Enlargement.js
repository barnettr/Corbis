if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Enlargement = {
    ImageListUpdating: false,

    OpenViewDimensions: function() {

        LogOmnitureEvent("event30");

        if ($('viewDimensionsModalWindow')) {
            $('viewDimensionsModalWindow').setStyle('display', 'block');

        } else {
            var el = $('viewDimensionsModal');
            el.setStyle('display', 'block');
            var elDimensions = el.getCoordinates();
            var properties = {
                title: '',
                collapsible: false,
                minimizable: false,
                contentBgColor: '#e8e8e8',
                headerStartColor: [219, 219, 219],
                headerStopColor: [219, 219, 219],
                bodyBgColor: [232, 232, 232],
                useCanvasControls: false,
                cornerRadius: 6,
                headerHeight: 32,
                footerHeight: 4,
                padding: 0,
                scrollbars: false,
                closable: false,
                type: 'window',
                id: el.getProperty('id') + "Window",
                height: 340,
                width: 575,
                x: 358,
                y: 143,
                content: '',
                draggable: false,
                resizable: false,
                shadowBlur: 7
            };
            MochaUI.NewWindowFromDiv(el, properties);
            $(document.body).addEvent('mousedown', this.detectDisplayOptionsclick.bindWithEvent($('viewDimensionsModalWindow')));
            ResizeModal('viewDimensionsModal');
        }
    },

    detectDisplayOptionsclick: function(ev) {
        var MSOcor = this.getCoordinates();
        if (
			((ev.page.y < MSOcor.top) || (ev.page.y > (MSOcor.top + MSOcor.height)))
			||
			(ev.page.x < MSOcor.left) || (ev.page.x > (MSOcor.left + MSOcor.width))
		) {
            (function() { CorbisUI.Enlargement.hideViewDimensionsWindow() }).delay(10);
        }
    },

    hideViewDimensionsWindow: function() {
        $('viewDimensionsModalWindow').setStyle('display', 'none');
    },

    selectTab: function(tabToSelect) {

        if (tabToSelect == 'imageDetails') {
            LogOmnitureEvent("event36");
        }
        else if (tabToSelect == 'corbisKeywords') {
            LogOmnitureEvent("event34");
        }
        else if (tabToSelect == 'relatedImages') {
            LogOmnitureEvent("event35");
        }
        //Show hide content
        $$('#imageDetailsContent, #corbisKeywordsContent, #relatedImagesContent').each
		(
			function(div) {
			    if (div.id.indexOf(tabToSelect) >= 0) {
			        if (div.hasClass('hdn')) div.removeClass('hdn');
			    }
			    else {
			        if (!div.hasClass('hdn')) div.addClass('hdn');
			    }
			}
		)

        var keywordsRelatedImages = $('keywordsRelatedImages');
        if (tabToSelect == 'imageDetails') {
            if (!keywordsRelatedImages.hasClass('hdn')) keywordsRelatedImages.addClass('hdn');
        }
        else {
            if (keywordsRelatedImages.hasClass('hdn')) keywordsRelatedImages.removeClass('hdn');
        }

        //show hide tabs
        $$('#topNavDiv a').each
		(
			function(anchor) {
			    if (anchor.id.indexOf('signInLink') < 0) {
			        if (anchor.id.indexOf(tabToSelect) >= 0) {
			            anchor.removeClass('tab');
			            if (!anchor.hasClass('selectedTab')) anchor.addClass('selectedTab');
			        }
			        else {
			            anchor.removeClass('selectedTab');
			            if (!anchor.hasClass('tab')) anchor.addClass('tab');
			        }
			    }
			}
		)

        if (tabToSelect == 'imageDetails') {
            $('imagePaging').removeClass('hdn');
        }
        else {
            if (tabToSelect == 'relatedImages' && !relatedImagesLoaded) {
                CorbisUI.Enlargement.getRelatedImages();
            }

            if (!$('imagePaging').hasClass('hdn')) $('imagePaging').addClass('hdn');
        }
    },

    showCorner: function() {
        var underTabs = $('TabTop').getElement('div.inTabTop');

        if (underTabs.hasClass('hdn')) {
            underTabs.removeClass('hdn');
        }
    },

    hideCorner: function() {
        var underTabs = $('TabTop').getElement('div.inTabTop');
        if (!underTabs.hasClass('hdn')) {
            underTabs.addClass('hdn');
        }
    },
    getQueryStringParam: function(param) {
        var querystring = window.location.search.substring(1);
        var querystringArray = querystring.split("&");
        for (i = 0; i < querystringArray.length; i++) {
            var querystringKeyValue = querystringArray[i].split("=");
            if (querystringKeyValue[0] == param) {
                return querystringKeyValue[1];
            }
        }
    },
    getProductUid: function() {
        return CorbisUI.Enlargement.getQueryStringParam('puid');
    },
    getFolderId: function() {
        var caller = CorbisUI.Enlargement.getQueryStringParam('caller');
        if (caller == 'cart') {
            //Cart page doesnt have the hidden lightbox(cart folder id)id. so neeed to get it somehow
            return "";
        } else {
            return $('MainContent').getElement('input.lightboxId[type=hidden]').value;
        }
    },
    iconToolsetSelect: function(value) {
        var caller = window.location.queryStringParam('caller');
        switch (value) {
            case 'AddToLightbox':
                ShowAddToLightboxModal($('MainContent').getElement('input.mediaUid').value, null, false);
                break;
            case 'AddToQuickPic':
                //Calls CorbisUI.Enlargement.addToQuickPic, and setup in presenter
                break;
            case 'CalculatePrice':
                //pricing url dependent on image, handled in presenter
                break;
            case 'ExpressCheckout':
                //existing code not changed
                CorbisUI.ExpressCheckout.Open($('corbisId').value, CorbisUI.Enlargement.getProductUid(), CorbisUI.Enlargement.getFolderId(), caller);
                break;
            case 'AddToCart':
                if ($('controlsDiv').getElement('a.AddToCartSelected') == null) {
                    //Check the caller. if the caller is from lightbox then instead of adding the item as an
                    //offering , add the item as a product to the cart instead.
                    var addToCart = new CorbisUI.Cart.AddToCart($('MainContent').getElement('input.mediaUid').value);
                    switch (caller) {
                        case 'lightbox':
                            addToCart.onSuccess = refreshCartItem;
                            addToCart.addProductToCart($('corbisId').value, CorbisUI.Enlargement.getProductUid());
                            break;
                        default:
                            addToCart.onSuccess = refreshCartItem;
                            addToCart.addOfferingToCart();
                            break;
                    }
                }
                break;
            case 'PrintPage':
                // Print verson of enlargement page.

                var originalURL = location.href;
                var urlArray = originalURL.split("?");
                var urlWithoutQuerystring = urlArray[0];

                var corbisId = $('corbisId').value;

                var querystring = window.location.search.substring(1);

                var querystringArray = querystring.split("&");
                var querystringKeyValue = null;


                var tmpQueryString = Browser.getQueryStringValues(window.location.href);
                var qsHash = new Hash(tmpQueryString);
                var newQS = '';
                qsHash.each(function(value, key, idx) {
                    if (key == 'id') qsHash.set(key, corbisId);
                    newQS += key + '=' + qsHash.get(key) + '&';
                });


                /* This 'if' statement detects if the URL contains "&tab=related", and removes it from the print version if it is. Bug no. 17608, April 2009 */
                var windowURL = urlWithoutQuerystring + "?" + newQS + "&print=true";
                if (windowURL.match('&tab=related')) {
                    windowURL = windowURL.replace(/&tab=related/, "");
                    window.open(windowURL, 'window', 'status=1, scrollbars=1, resizable=1, width=800, height=700');
                }
                else {
                    window.open(windowURL, 'window', 'status=1, scrollbars=1, resizable=1, width=800, height=700');
                }
                break;
        }

        return false;
    },

    getRelatedImages: function() {
        relatedImagesLoaded = true;
        Corbis.Web.UI.Enlargement.EnlargementScriptService.GetRelatedImages($('corbisId').value, displayRelatedImages, methodFailed)
    },

    addToQuickPic: function(corbisId, Url128, licenseModel, aspectRatio, title) {
        //console.log('CALLING: CorbisUI.Enlargement.addToQuickPic');
        Corbis.Web.UI.Search.SearchScriptService.AddItemToQuickPick(corbisId, Url128, licenseModel, aspectRatio, title, addToQuickPicSuccess, methodFailed);
    },

    viewRfcdImages: function(mediaSetType, corbisId) {
        var rfcdUrl = String.format('/imagegroups/imagegroups.aspx?typ={0}&id={1}', mediaSetType, corbisId);
        try {
            opener.window.location.href = rfcdUrl;
        }
        catch (e) {
            window.open(rfcdUrl);
        }

        window.close();
    },

    updateRelativeImageQuery: function(relativeNumber) {
        //In safari enter key triggers submit of first submit button, so we need to check and not submit it again if it's a number change.
        if (Browser.Engine.webkit && this.ImageListUpdating) return false;
        var currentImageNumber = parseInt($('Pager').getElement('input.origPageNumber').value);
        return this.updateImageQuery(currentImageNumber + relativeNumber);
    },

    updateImageQuery: function(imageNumber) {
        this.ImageListUpdating = true;
        var pageForm = $('aspnetForm');
        var totalItems = parseInt($('Pager').getElement('input.totalItems').value);
        var currentPage = parseInt(pageForm.getElement('input.parentPageNo').value);
        var pageSize = parseInt(pageForm.getElement('input.parentPageSize').value);

        imageNumber = imageNumber < 1 ? 1 : imageNumber;
        imageNumber = imageNumber > totalItems ? totalItems : imageNumber;
        var pageNeeded = Math.ceil(imageNumber / pageSize);

        if (pageNeeded == currentPage) {
            var imageList = pageForm.getElement('input.parentImageList').value.split(',');
            var imageIndex = (imageNumber - ((currentPage - 1) * pageSize)) - 1;

            if (imageIndex < imageList.length) {
                var corbisId = imageList[imageIndex];
                pageForm.setProperty('action', pageForm.getProperty('action').replace(/id=[^&]*/, 'id=' + corbisId));
            }

            return true;
        }
        else {
            Corbis.Web.UI.Enlargement.EnlargementScriptService.GetImageList(imageNumber, window.location.queryStringParam('caller'), pageSize, pageForm.getElement('input.parentQueryString').value, pageForm.getElement('input.lightboxId').value, this.GetImageListSucceeded, methodFailed, imageNumber)

            return false;
        };

    },

    GetImageListSucceeded: function(result, context) {
        var pageForm = $('aspnetForm')
        pageForm.setProperty('action', pageForm.getProperty('action').replace(/id=[^&]*/, 'id=' + result[0]));
        var pager = $('Pager')
        pager.getElement('input.totalItems').value = result[1];
        pageForm.getElement('input.parentPageNo').value = result[2];
        result.splice(0, 3);
        pageForm.getElement('input.parentImageList').value = result.join(',');

        //will just use the pagenumber change to handle the back and forth buttons as well.
        var navNumber = pager.getElement('input.NavPageNumber')
        navNumber.value = context;
        setTimeout('__doPostBack(\''+navNumber.name+'\',\'\')', 0);
    }
};

var fullURL = parent.document.URL;
if (fullURL.match("&print=true")) {
    AddRounded = function() { };
}

window.onload = isItPrintVersion;

function isItPrintVersion() {

    if (fullURL.match("&print=true")) {

        $(window.document.body).removeClass('noGlobalBody');
        $(window.document.body).addClass('printVersion');
        window.focus();
        window.print();
    }
}

var webServiceResult;
var imageGroups;
var imageTemplate;
var relatedImagesLoaded;
function displayRelatedImages(results, context, methodName) {
    var imageGroupTemplate = $('imageGroupTemplate');
    var imageGroupContainer = $('relatedImageGroups');
    imageTemplate = $('imageTemplate');

    //add the groups
    webServiceResult = results;
    imageGroups = new Array(results.length);
    var RIGW = $('relatedImageGroupsWrapper');

    RIGW.getElement('.relatedImageMessage')
	    .set('html', $('relatedImageGroupsWrapper')
	                    .getElement('.relatedImageMessage')
	                    .get('html')
	                    .replace('{0}', imageGroups.length)
	        );
    RIGW.getElement('.relatedImageMessage')
	    .removeClass('hdn');

    results.each(function(item, i) {
        //imageGroups[i] = imageGroupTemplate.clone(true, false); 
        //using cloneNode because clone set image width to 0px;
        imageGroups[i] = $(imageGroupTemplate.cloneNode(true));
        imageGroupContainer.appendChild(imageGroups[i]);

        window.setTimeout('populateImageGroup(' + i + ')', 0);
    });

    //add a blank div for footer spacing, for FF
    var paddingdiv = new Element('div', {
        'styles': {
            'height': '36px'
        }
    });
    imageGroupContainer.appendChild(paddingdiv);

    Rounded('filmstripWrapper', 4, 4, 4, 4);
}

function populateImageGroup(groupIndex) {
    imageGroups[groupIndex]
		    .getElement('.relatedImageGroupTitle')
		    .set('html', webServiceResult[groupIndex].Name);
    if (webServiceResult[groupIndex].MediaType == Corbis.Image.Contracts.V1.ImageMediaSetType.RFCD) imageGroups[groupIndex].getElement('img').removeClass('hdn');
    if (webServiceResult[groupIndex].MediaType == Corbis.Image.Contracts.V1.ImageMediaSetType.RFCD ||
			webServiceResult[groupIndex].MediaType == Corbis.Image.Contracts.V1.ImageMediaSetType.StorySet) imageGroups[groupIndex].getElement('div.relatedImageGroupTitle').removeClass('hdn');
    imageGroups[groupIndex]
		    .getElement('h3')
		    .set('html', GetImageGroupLink(webServiceResult[groupIndex]));
    imageGroups[groupIndex]
		    .getElement('.relatedImageGroupDetail')
		    .set('html', imageGroups[groupIndex]
		                .getElement('.relatedImageGroupDetail')
		                .get('html')
		                .replace('{0}', webServiceResult[groupIndex].DisplayImageList.length)
		     );
    window.setTimeout('populateImages(' + groupIndex + ')', 0);
}

function populateImages(groupIndex) {
    var imageList = webServiceResult[groupIndex].DisplayImageList;
    var filmstrip = imageGroups[groupIndex].getElement('ul');
    var currentImage = $('corbisId').value;

    filmstrip.setProperty('mediaSetId', webServiceResult[groupIndex].MediaSetId);

    imageList.chunk(function(item, j, arr) {
        var newImage = imageTemplate.clone();
        var thumbNail = $(new Image());
        var imageLink = newImage.getElement('a')

        thumbNail.setProperties({
            'title': item.CorbisId,
            id: 'Image_' + groupIndex + '_' + j,
            'aspectRatio': item.AspectRatio,
            'imageCount': imageList.length
        }).addEvent('load', function() { checkLoading($(this)); });

        imageLink.appendChild(thumbNail);

        if (item.CorbisId == currentImage) {
            newImage.addClass('selected');
            imageLink.setProperty('href', "#")
			    .addEvent('click', function() {
			        CorbisUI.Enlargement.selectTab('imageDetails');
			        return false;
			    });
        }
        else {
            imageLink.setProperty('href', "#")
			    .addEvent('click', function() {
                    EnlargeImagePopUp(String.format('{0}?id={1}&mediaSetId={2}&caller=mediaset', document.location.href.substring(0, document.location.href.indexOf('?')), item.CorbisId, webServiceResult[groupIndex].MediaSetId), 1000, 650);
			        return false;
			    });

            newImage.addEvents({
                'mouseover': function() { $(this).addClass('selected'); },
                'mouseout': function() { $(this).removeClass('selected'); }
            });

        }

        filmstrip.appendChild(newImage);

        //need to set image source after event .. for IE
        thumbNail.src = item.Url128;
    }, 50);
}

CorbisUI.QueueManager.addQueue('ToolsetMacros', {
    canRerun: true,
    delay: true ,
    runOnDomReady: true,
     delayTime: 500
}).addItem('iconToolset',function(){CorbisUI.Enlargement.iconToolsetSelect()});

function checkLoading(image) {
    image = $(image);

    var ATTR = image.getProperties('aspectRatio', 'imageCount');

    if (ATTR.aspectRatio < 1) {
        image.width = 90 * ATTR.aspectRatio;
        image.height = 90;
    }
    else {
        image.width = 90;
        image.height = 90 / ATTR.aspectRatio;
        image.style.marginTop = Math.floor((90 - image.height) / 2);

    }

    //taking into account the current completing image.
    var imagesLoaded = 1;
    var filmstripImages = image.getParent('ul');

    filmstripImages.getElements('img').each
	(
		function(image) { if (image.complete) imagesLoaded++; }
	);

    //if we have loaded all or 50 images, remove the spinning thing.
    if (imagesLoaded >= Math.min(ATTR.imageCount, 50)) {
        var filmstrip = filmstripImages.getParent().getParent();
        filmstrip.getElement('.filmstripLoading').addClass('hdn');
        filmstrip.getElement('.filmstripItemsWindow').removeClass('hdn');
        CorbisUI.Filmstrip.setScrollControls(filmstrip);
    }
}

function addToQuickPicSuccess(corbisId, context, methodName) {
    if (corbisId == "") {
        new CorbisUI.Popup('quickpicMaximumAlert', {
            showModalBackground: false,
            closeOnLoseFocus: true,
            positionVert: 'middle',
            positionHoriz: 'right'
        });
    }
    else {
        //update enlargement icon
        var quickPicIcon = $('controlsDiv').getElement('a.AddToQuickPic');
        if (quickPicIcon && !quickPicIcon.hasClass('AddToQuickPicSelected')) {
            quickPicIcon.removeClass('AddToQuickPic');
            quickPicIcon.addClass('AddToQuickPicSelected');
        }

        var parentWindow = window.opener;
        if (parentWindow) {
            try {
                switch (parentWindow.location.pathname.split('/').getLast().toLowerCase()) {
                    case 'searchresults.aspx':
                    case 'imagegroups.aspx':
                    case 'mylightboxes.aspx':
                    case 'mediasetsearch.aspx':
                        window.opener.CorbisUI.Handlers.Quickpic.refreshItemAdded(corbisId);
                        break;
                    case 'enlargement.aspx':
                        window.opener.addToQuickPicSuccess(corbisId, context, methodName);
                        break;
                }
            }
            catch (e) { };
        }
    }
}

function methodFailed(results, context, methodName) {
    if (methodName == 'GetRelatedImages') relatedImagesLoaded = false;
}

function showDisclaimer(link) {

    LogOmnitureEvent("event33");
    
    new CorbisUI.Popup('disclaimerPopup', {
        showModalBackground: false,
        centerOverElement: 'disclaimerLinkDiv',
        closeOnLoseFocus: true,
        positionVert: 'top',
        positionHoriz: -2
    });
}

function clearKeywordsCheckBox() {
    var checks = $('corbisKeywordsContent').getElements('div.imageCheckbox');

    checks.each(function(cb) {
        setCheckedState(cb, false)
    });

}

function enableOrDisableSearchClearOnCheck() {
    var checks = $('corbisKeywordsContent').getElements('div.imageCheckbox');
    var hasOneChecked = false;

    checks.each(function(cb) {
        if (getCheckedState(cb)) {
            hasOneChecked = true;
        }
    });

    var searchNow = $('corbisKeywordsContent').getElement('.GlassButton');
    var clearAll = $('clearKeywords').getElement('span.textIconButtonContainer');

    if (hasOneChecked) {
        setGlassButtonDisabled(searchNow, false);
        setTextIconButtonDisabled(clearAll, false);
    }
    else {
        setGlassButtonDisabled(searchNow, true);
        setTextIconButtonDisabled(clearAll, true);
    }

}

function reloadparentCloseChild(qKey, qVal, recentImageId, searchFilterQuerystring) {
    if(qKey == "pg")
    {
        LogOmnitureEvent("event32"); 
    }
    else if(qKey == "lc")
    {
        LogOmnitureEvent("event31"); 
    }

    var url = "../Search/SearchResults.aspx?" + qKey + "=" + UrlEncode(qVal) + "&ri=" + recentImageId + "&options=true" + searchFilterQuerystring;
    redirectparentCloseChild(url);
}

function searchKeywordsNowAction(isThirdParty, searchFilterQuerystring) {
    var isFromThirdParty = isThirdParty;
    var keywordWrap = $('corbisKeywordsContent').getElement('.keywordsDisplay');
    var items = keywordWrap.getElements('input[type=checkbox]');
    var searchKeywords = [];
    var keywordString = '';

    items.each(function(el) {
        if (el.checked) {
            var keyword = el.getNext('label').get('text').trim();
            if (keyword.test(' ') || keyword.test('&') || keyword.test('-')) {
                if (keyword.test('&')) {
                    keyword = keyword.replace("&", "%26");
                }
                keyword = '"' + keyword + '"';
            }
            searchKeywords.push(keyword);
        }
    });

    if (searchKeywords.length > 0) {
        searchKeywords.each(function(el) {
            keywordString = keywordString + ' ' + el;
        });
        keywordString.trim();
        var url = '../Search/SearchResults.aspx?q=' + keywordString + searchFilterQuerystring;

        if (isFromThirdParty == true) {
            location.href = url;
        }
        else if (window.opener && !window.opener.closed) {
            try {
                window.opener.CorbisUI.ExtendedSearch.ShowSearchProgIndicator();
                window.opener.location.href = url;
                self.close();
            }
            catch (e) {
            }
        }
        else {
            window.open(url);
            self.close();
        }
    }
    return false;
}

function refreshLightbox(lightboxId, corbisId, newlightboxName) {
    var lightboxButton = $('controlsDiv').getElement('a.AddToLightbox')
    if (lightboxButton) {
        lightboxButton.removeClass('AddToLightbox');
        lightboxButton.addClass('AddToLightboxSelected');
    }
    
    var parentWindow = window.opener;
    if (parentWindow) {
        try {
            switch (parentWindow.location.pathname.split('/').getLast().toLowerCase()) {
                case 'searchresults.aspx':
                case 'imagegroups.aspx':
                case 'mediasetsearch.aspx':
                    window.opener.CorbisUI.Handlers.Lightbox.refreshItemAdded(corbisId, lightboxId, newlightboxName);
                    break;
                case 'mylightboxes.aspx':
                    if (newlightboxName != "") {
                        window.opener.location = window.opener.location;
                    }
                    else {
                        window.opener.GetLB(lightboxId);
                    }
                    break;
                case 'enlargement.aspx':
                    window.opener.refreshLightbox(lightboxId, corbisId, newlightboxName);
                    break;
            }
        }
        catch (e) { };
    }
}

function refreshCartItem(results) {
    //disable button
    var addToCartIcon = $('controlsDiv').getElement('a.AddToCart');
    if (addToCartIcon) {
        addToCartIcon.removeClass('AddToCart');
        if (!addToCartIcon.hasClass('AddToCartSelected')) addToCartIcon.addClass('AddToCartSelected');
    }

    var parentWindow = window.opener;
    if (parentWindow) {
        try {
            var parentDocument = $(parentWindow.document);
            var offeringUid = $('MainContent').getElement('input.mediaUid').value;
            var corbisId = $('corbisId').value;

            switch (parentDocument.location.pathname.split('/').getLast().toLowerCase()) {
                case 'searchresults.aspx':
                case 'imagegroups.aspx':
                case 'mediasetsearch.aspx':
                    window.opener.CorbisUI.Handlers.Cart.refreshItemAdded(offeringUid, corbisId, results);
                    break;
                case 'mylightboxes.aspx':
                    window.opener.CorbisUI.Lightbox.Handler.refreshCartItem(corbisId, results);
                    break;
                case 'cart.aspx':
                    window.opener.location = window.opener.location;
                    window.location = window.location;
                    break;
                case 'enlargement.aspx':
                    window.opener.refreshCartItem(results);
                    break;
                default:
                    //update cart count
                    window.opener.UpdateCartCount(results);
                    break;
            }
        }
        catch (e) {
        }
    }
}

function refreshEnlargementPage(pageAction) {
    //to take into account the fact the user might have move through a number of images.
    //we have to do a form repost because of all the variable we keep at a form level.
    $$('input.pageAction')[0].value = pageAction;
    __doPostBack('refreshEnlargementPage', '');
}

function ExecutePageAction(pageAction) {
    var pageActionIcon = $('controlsDiv').getElement('a.' + pageAction);
    //sometimes the action might not be available after a user is signed on, eg.adding to cart when image is already in cart.
    if (pageActionIcon)
    {
        pageActionIcon.click();
        
        
    }
}

function GetImageGroupLink(group) {
    return String.format('<a href="javascript:void(0)" onclick="javascript:redirectparentCloseChild(\'/imagegroups/imagegroups.aspx?typ={0}&id={1}&ri={2}\'); return false;">{3}</a>', group.MediaType, group.MediaSetId, $('corbisId').value, Corbis.Image.Contracts.V1.ImageMediaSetType.toLocalizedString(group.MediaType));
}

function redirectparentCloseChild(url) {
    if (window.opener) {
        try {
            if (window.opener.redirectparentCloseChild) {
                window.opener.redirectparentCloseChild(url);
            } else {
                window.opener.CorbisUI.ExtendedSearch.ShowSearchProgIndicator();
                window.opener.focus();
                window.opener.location.href = url;
            }
        } catch (e) { }
    } else {
        window.open(url);
    }
    self.close();
}

window.addEvent('domready', checkTopNavTabsLength);

function checkTopNavTabsLength() {
    if (CorbisUI.GlobalVars.Enlargement.isAnonymous) {
        var aItem = $('topNavDiv').getElements('a');
        aItem.each(function(link) {
            if (aItem.length >= 4) {
                $('signIn').setStyles({
                    'marginTop': '7px',
                    'marginBottom': '5px',
                    'marginLeft': '2px',
                    'float': 'left',
                    'fontSize': '12px',
                    'textAlign': 'left',
                    'width': '176px',
                    'whiteSpace': 'normal',
                    'lineHeight': '13px',
                    'height': '20px'
                });
            }
            if (aItem.length <= 3) {
                $('signIn').setStyles({
                    'marginTop': '7px',
                    'marginBottom': '5px',
                    'marginLeft': '5px',
                    'float': 'left',
                    'fontSize': '12px',
                    'textAlign': 'left',
                    'width': '340px',
                    'whiteSpace': 'normal',
                    'lineHeight': '13px'
                });
            }
        });
    }
    if (!CorbisUI.GlobalVars.Enlargement.isAnonymous) {
        $('signIn').setStyle('display', 'none');
    }
}
