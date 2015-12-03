/******************************
CORBIS ORDER HISTORY
*******************************/

/***************************
    GLOBAL VARIABLES
****************************/
var executer;

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}


CorbisUI.Order = {
    OpenProgressModal: function(url) {
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

    CancelDownload: function() {
        if (executer && executer.get_started()) {
            executer.abort();
        }

        HideModal('downloadProgress');
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
        this.ThumbTips = new Tips('.thumbWrap', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            offsets: { x: 0, y: -120 },
            className: 'TIP-license-details mochaContent',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    },
    DownloadImage: function(item) {
        LogOmnitureEvent("event19"); 
        
        var product = new CorbisUI.Order.ProductBlock($(item).getParent().getParent().getParent());

        if (product.offeringType == 'RFCD') {
            var rfcdInfo = $('rfcdDownloadTemplate').getElement('div.rfcdInfo');
            var rfcdDownloadThumb = rfcdInfo.getElement('div.rfcdImageThumb');
            rfcdDownloadThumb.empty();
            var rfcdThumb = product.productBlock.getElement('div.thumbWrap').getElement('img');
            rfcdThumb.clone().inject(rfcdDownloadThumb);
            rfcdDownloadThumb.set('title', rfcdThumb.get('title'));

            var rfcdHeaderInfo = $('licensePane').getElement('div.licenseDetailHeader');

            rfcdInfo.getElement('span.rfcdId').set('text', product.corbisId);
            rfcdInfo.getElement('span.licenseModel').set('text', rfcdHeaderInfo.getElement('span.licenseModel').get('text'));
            rfcdInfo.getElement('span.rfcdTitle').set('text', rfcdHeaderInfo.getElement('span.rfcdTitle').get('text'));
            rfcdInfo.getElement('span.rfcdImageCount').set('text', product.rfcdCount);
            $('selectedFileSize').value = product.selectedFileSize;

            Corbis.Web.UI.Checkout.CheckoutService.GetRfcdImagesByVolumeNumber(product.corbisId, CorbisUI.Order.GetRfcdImageSucceeded, CorbisUI.Order.GetRfcdImageFailed);
        }
        else {
            var order = $('CompletedOrder');
            var orderUid = order.getProperty('orderUid');
            var orderNumber = order.getProperty('orderNumber');
            var longWaitMessageDiv = $('longWaitMessageDiv');

            if (longWaitMessageDiv.hasClass('hdn')) {
                longWaitMessageDiv.addClass('hdn');
            }

            var serviceCall = Corbis.Web.UI.Checkout.CheckoutService._staticInstance.DownloadImage(orderUid, orderNumber, product.imageUid, product.selectedFileSize, product.offeringType, CorbisUI.Order.DownloadImageSucceeded, CorbisUI.Order.DownloadImageFailed, new Array(1, orderNumber));
            executer = serviceCall.get_executor();
        }
    },

    DownloadImages: function() {

        LogOmnitureEvent("event19"); 
        
        var order = $('CompletedOrder');
        var orderUid = order.getProperty('orderUid');
        var orderNumber = order.getProperty('orderNumber');
        var productBlocks = $('downloadProducts').getElements('div.productBlock');
        var imageList = new Array();
        var index = 0;
        var pendingImages = 0;
        var longWaitMessageDiv = $('longWaitMessageDiv');

        productBlocks.each(function(el) {
            var productBlock = new CorbisUI.Order.ProductBlock(el);
            if (productBlock.imageAvailable) {
                var image = new Corbis.MediaDownload.Contracts.V1.ImageDownloadFileSize();
                image.FileSize = productBlock.selectedFileSize;
                image.ImageUid = productBlock.imageUid;
                image.OfferingType = productBlock.offeringType;

                imageList[index] = image;
                index++;
            }
            else {
                //get image count of images that will not be downloaded
                if (productBlock.OfferingType == 'RFCD') {
                    pendingImages += productBlock.rfcdCount;
                }
                else {
                    pendingImages++;
                }
            }
        });

        if ((CorbisUI.GlobalVars.DownloadSummary.totalImageCount - pendingImages) > 50) {
            longWaitMessageDiv.removeClass('hdn');
        }
        else {
            if (!longWaitMessageDiv.hasClass('hdn')) {
                longWaitMessageDiv.addClass('hdn');
            }
        }

        var serviceCall = Corbis.Web.UI.Checkout.CheckoutService._staticInstance.DownloadImages(orderUid, orderNumber, imageList, CorbisUI.Order.DownloadImageSucceeded, CorbisUI.Order.DownloadImageFailed, new Array((CorbisUI.GlobalVars.DownloadSummary.totalImageCount - pendingImages), orderNumber));
        executer = serviceCall.get_executor();
    },

    DownloadRfcdImages: function() {
        var order = $('CompletedOrder');
        var orderUid = order.getProperty('orderUid');
        var orderNumber = order.getProperty('orderNumber');
        var selectedImages = $('rfcdImages').getElements('li.selected');
        var imageList = new Array(selectedImages.length);
        var index = 0;
        var selectedSize = $('selectedFileSize').value;
        var longWaitMessageDiv = $('longWaitMessageDiv');
        var offeringType = Corbis.CommonSchema.Contracts.V1.OfferingType.Stills;

        selectedImages.each(function(el) {
            var image = new Corbis.MediaDownload.Contracts.V1.ImageDownloadFileSize();
            image.FileSize = selectedSize;
            image.ImageUid = el.getProperty('imageUid');
            image.OfferingType = offeringType;

            imageList[index] = image;
            index++;
        });

        if (selectedImages.length > 50) {
            longWaitMessageDiv.removeClass('hdn');
        }
        else {
            if (!longWaitMessageDiv.hasClass('hdn')) {
                longWaitMessageDiv.addClass('hdn');
            }
        }

        HideModal('rfcdDownloadTemplate');

        var serviceCall = Corbis.Web.UI.Checkout.CheckoutService._staticInstance.DownloadImages(orderUid, orderNumber, imageList, CorbisUI.Order.DownloadImageSucceeded, CorbisUI.Order.DownloadImageFailed, new Array(selectedImages.length, orderNumber));
        executer = serviceCall.get_executor();
    },

    DownloadImageSucceeded: function(result, context) {
        $('zipFilesHeader').set('html', String.format(CorbisUI.GlobalVars.DownloadSummary.zipFilesHeaderTemplate, result.DownloadImages.length.toString()));

        var zipFilesDiv = $('zipFiles');
        zipFilesDiv.empty();

        result.DownloadImages.each(function(el) {
            (new Element('a', {
                'href': el.Value,
                'html': el.Key
            })).inject(zipFilesDiv, 'bottom');

            (new Element('br')).inject(zipFilesDiv, 'bottom');
        });

        $('imagesHeader').set('html', String.format(CorbisUI.GlobalVars.DownloadSummary.imagesHeaderTemplate, context[0] - result.FailedCount, context[1]));

        OpenModal('downloadImagesModal');
        ResizeModal('downloadImagesModal');
    },

    DownloadImageFailed: function(result) {
        if (!executer.get_aborted()) {
            OpenModal('downloadErrorModal');
        }
    },

    SetFileSizeAvailability: function(selectedItem, availableSizes) {
        var downloadButton = $(selectedItem).getParent().getElement('div.GlassButton');
        var selectedSize = selectedItem.options[selectedItem.selectedIndex].value;

        if (availableSizes.contains(selectedSize)) {
            setGlassButtonDisabled(downloadButton, false);
            downloadButton.getElement('input').setProperty('value', downLoadText);
        }
        else {
            setGlassButtonDisabled(downloadButton, true);
            downloadButton.getElement('input').setProperty('value', pendingText);
        }
    },

    ToggleAllRfcd: function(el) {
        var selectElement = $(el);
        var selectText = selectElement.getProperty('select');
        var imagesItems = $('rfcdImages').getElements('li')
        var downloadButton = $('rfcdDownloadTemplate').getElement('div.GlassButton');

        if (selectElement.get('text') == selectText) {
            imagesItems.each(function(el) {
                if (!el.hasClass('selected')) el.addClass('selected');
            });
            selectElement.set('text', selectElement.getProperty('deselect'));

            setGlassButtonDisabled(downloadButton, false);
        }
        else {
            imagesItems.each(function(el) {
                el.removeClass('selected');
            });
            selectElement.set('text', selectText);
            setGlassButtonDisabled(downloadButton, true);
        }
    },

    addProductToCart: function(uid) {
        var addToCart = new CorbisUI.Cart.AddToCart(uid);
        addToCart.context = this;
        addToCart.onSuccess = this.refreshCartItem;
        addToCart.addOfferingToCart();
    },

    refreshCartItem: function(results) {
        //update cart count
        var cartCounter = $('cartCount');
        if (cartCounter) {
            cartCounter.setProperty('text', results);
            fixIECheckoutWidgetWidth();
        }
    },
    GetRfcdImageSucceeded: function(result) {
        if (result) {
            var imageList = $('rfcdImages').getElement('ul');
            imageList.empty();

            result.each(function(image) {
                var listImage = new Element('img', {
                    'src': image.Url128,
                    'title': image.CorbisId + ' ' + image.Title
                });

                var listDiv = new Element('div', {
                    'class': 'imageWrap'
                }).addEvent('click', function(e) {
                    var thumbDiv = $(e.target);
                    //if it's the image then we need to set it to the containing div
                    if (!thumbDiv.hasClass('imageWrap')) {
                        thumbDiv = thumbDiv.getParent();
                    }

                    var listItem = thumbDiv.getParent()

                    if (e.shift && lastSelectedIndex != undefined) {
                        var listItems = listItem.getParent().getElements('li');
                        var currentIndex = listItems.indexOf(listItem);
                        var startIndex = currentIndex;
                        var endIndex = lastSelectedIndex;

                        if (startIndex > endIndex) {
                            startIndex = lastSelectedIndex;
                            endIndex = currentIndex;
                        }

                        for (var i = startIndex; i <= endIndex; i++) {
                            if (!listItems[i].hasClass('selected')) listItems[i].addClass('selected');
                        }
                    }
                    else {
                        listItem.toggleClass('selected');
                    }

                    lastSelectedIndex = listItem.getParent().getElements('li').indexOf(listItem);

                    var downloadButton = $('rfcdDownloadTemplate').getElement('div.GlassButton');
                    if (listItem.hasClass('selected')) {
                        setGlassButtonDisabled(downloadButton, false);
                    }
                    else if (listItem.getParent().getElements('li.selected').length == 0) {
                        setGlassButtonDisabled(downloadButton, true);
                    }
                });

                var newlistItem = new Element('li').setProperty('imageUid', image.MediaUid);

                listImage.inject(listDiv);
                listDiv.inject(newlistItem);
                newlistItem.inject(imageList, 'bottom');

                if (image.AspectRatio > 1) {
                    var thumbHeight = 90 / image.AspectRatio;
                    listImage.setStyles({ 'marginTop': (90 - thumbHeight) / 2, 'width': 90, 'height': thumbHeight });
                }
                else {
                    listImage.setStyles({ 'marginTop': 0, 'width': (90 * image.AspectRatio), 'height': 90 });
                }
            });
        }

        var selectAllLink = $('rfcdDownloadTemplate').getElement('a.selectAllLink');
        selectAllLink.set('text', selectAllLink.getProperty('select'));
        setGlassButtonDisabled($('rfcdDownloadTemplate').getElement('div.GlassButton'), true);

        OpenModal('rfcdDownloadTemplate');
    },

    GetRfcdImageFailed: function(result) {
        //TODO: handle error
    }
};

var lastSelectedIndex;

CorbisUI.Order.ProductBlock = new Class({
    productBlock: null,
    imageUid: null,
    corbisId: null,
    selectedFileSize: null,
    offeringType: null,
    rfcdCount: null,
    imageAvailable: false,

    initialize: function(el) {
        this.productBlock = $(el)
        this.imageUid = this.productBlock.getProperty('imageUid');
        this.corbisId = this.productBlock.getProperty('corbisId');
        var filesizeDropdown = this.productBlock.getElement('select');
        this.selectedFileSize = filesizeDropdown ? this.productBlock.getElement('select').value : null;
        this.offeringType = this.productBlock.getProperty('offeringType');
        var downloadButton1 = this.productBlock.getElement('input');
        this.imageAvailable = downloadButton1 && !downloadButton1.getProperty('disabled');
        if (this.offeringType == 'RFCD') {
            this.rfcdCount = this.productBlock.getElement('div.rfcdCount').getProperty('text').toInt();
        }
    }
})
function showFullrightAddendum(el) {
    el = $(el);
    var parent = el.getParent('div.rightAddendum');

    var lessItem = parent.getElement('div.lessAddendum');
    var lessStyle = lessItem.getStyle('display');

    var lessItemLink = parent.getElement('span.lessLink');
    var lessStyleLink = lessItemLink.getStyle('display');

    var moreItem = parent.getElement('div.moreAddendum');
    var moreStyle = moreItem.getStyle('display');

    var moreItemLink = parent.getElement('span.moreLink');
    var moreStyleLink = moreItemLink.getStyle('display');

    if (lessStyle == 'block') {
        lessItem.setStyle('display', 'none');
        moreItemLink.setStyle('display', 'none')
    }
    else {
        lessItem.setStyle('display', 'block');
        moreItemLink.setStyle('display', 'block');
    }
    if (moreStyle == 'none') {
        moreItem.setStyle('display', 'block');
        lessItemLink.setStyle('display', 'block')
    }
    else {
        lessItemLink.setStyle('display', 'none');
        moreItem.setStyle('display', 'none');
    }
}

function getOrderHistorySummaryList(result) {
	
	    var selectDropdown = $('ddlSelectOrder');
	    var selectOrder = $('selectOrder');
	    var g_firefox = document.getElementById && !document.all;

		if (result && result.length > 0)
		{
			selectDropdown.empty();
			result.each(function(item) {
			    var optionItem;
			    if (orderUid == item.Value) {
			        var checkMark = 'selectOrderOption';
			    }
			    else { 
			        var checkMark = 'selectOrderOption1';
			    }
			    
			    if(item.Value != null)
			    {
			        optionItem = new Element('a', {
			            'text': item.Key,
			            'href': '../OrderHistory/OrderHistorySummary.aspx?OrderUid=' + item.Value,
			            'class': checkMark
			        });
			    }
			    else
			    {
			        optionItem = new Element('a', {
			            'text': item.Key,
			            'href': '../OrderHistory/OrderHistory.aspx',
			            'class': checkMark
			        });
			    }
			    
			    optionItem.inject(selectDropdown);
			});
            }
        
         
       // var  ddlWidth = selectDropdown.offsetWidth +10 ;
       // selectDropdown.setStyle('width', ddlWidth);
            selectDropdown.setStyle('padding-left', '3px');
            if (navigator.userAgent.toLowerCase().indexOf("msie") > 0) {
                selectDropdown.setStyle('padding-right', '18px');
            }
            else {
                selectDropdown.setStyle('padding-right', '3px');
            }
        selectDropdown.setStyle('background-color', '#cecfce');
		if (result.length > 10)
		{
		    selectDropdown.setStyle('height', '270px');
		    
		    if (!selectDropdown.hasClass('selectContainerScrollBar'))
		        selectDropdown.addClass('selectContainerScrollBar');

	    }
		else
		{
		    if (!selectDropdown.hasClass('selectContainer'))
		        selectDropdown.addClass('selectContainer');
	    }

	}
    

    function OpenSelectOrder()
    {
        if (Browser.Engine.webkit) {
            $('selectOrder').setStyle('visibility', 'visible');
            $('selectOrder').setStyle('display', 'block');
        } else {
            $('selectOrder').setStyle('display', 'block');
        }
	     if(orderUid !=null)
            Corbis.Web.UI.OrderHistory.OrderHistoryScriptService.GetProjectList( 0, getOrderHistorySummaryList);
    }
//mouse out
    function HideddlSelectOrder() {
        try {
            if (Browser.Engine.webkit) {
                $('selectOrder').setStyle('visibility', 'hidden');
            } else {
                $('selectOrder').setStyle('display', 'none');
            }
            

        } catch (er) { }
    }
    function ShowddlSelectOrder() {
        try {
            if (Browser.Engine.webkit) {
                $('selectOrder').setStyle('display', 'block');
                $('selectOrder').setStyle('visibility', 'visible');
            } else {
                $('selectOrder').setStyle('display', 'block');
            }
        } catch (er) { }
    }
    
  
//Dom ready stuff
    window.addEvent('domready', function() {
        if (window.location.pathname.toLowerCase().contains('orderhistorysummary.aspx')) {
            $$('div.licenseDetailsImageThumb').each(
			function(el) {
			    var imageThumb = el.getElement('img');
			    imageThumb.setStyles(CorbisUI.Image.ScaleImage(imageThumb, { 'marginTop': 90, 'width': 90, 'height': 90 }));
			}
		);

            $$('.restrictionsToggler').addEvents({
                'click': function() {
                    var restrictionDiv = $(this).getParent().getPrevious();
                    if (restrictionDiv.style.height == '140px') {
                        // Always sets the duration of the tween to 1000 ms and a bouncing transition
                        // And then tweens the height of the element
                        restrictionDiv.set('tween', {
                            duration: 1000,
                            transition: Fx.Transitions.Bounce.easeOut // This could have been also 'bounce:out'
                        }).tween('height', restrictionDiv.scrollHeight);

                        this.set('text', CorbisUI.GlobalVars.DownloadSummary.lessRestrictionText);
                    } else {
                        restrictionDiv.set('tween', {
                            duration: 1000,
                            transition: Fx.Transitions.Bounce.easeOut // This could have been also 'bounce:out'
                        }).tween('height', '140');

                        this.set('text', CorbisUI.GlobalVars.DownloadSummary.moreRestrictionText);
                    }
                }
            }).each(function(el) { el.getParent().getPrevious().setStyle('height', '140px') });

            //Adjust so the left and right pane are same height
            if ($('licensePane') != null) {
                $('licensePane').getElement('table.liecenseDetails').getElement('tbody').getChildren().each(
			function(el) {
			    var restrictionDiv;
			    var height;
			    var oddDetail = el.getElement('td.oddLicenseDetail').getElement('div.licenseImageDetails');
			    var evenDetail = el.getElement('td.evenLicenseDetail').getElement('div.licenseImageDetails');

			    if (evenDetail && oddDetail) {
			        var oddRect = oddDetail.getBoundingClientRect();
			        var evenRect = evenDetail.getBoundingClientRect();
			        var heightDifference = oddRect.bottom - evenRect.bottom;

			        if (heightDifference == 0) {
			            return;
			        }
			        else if (heightDifference < 0) {
			            height = oddRect.bottom - oddRect.top + Math.abs(heightDifference) - 20;
			            restrictionDiv = oddDetail;
			        }
			        else {
			            height = evenRect.bottom - evenRect.top + heightDifference - 20;
			            restrictionDiv = evenDetail;
			        }

			        restrictionDiv.setStyle('height', height);
			    }
			}
		)
            };
        }
    });

    // Disable IE7 quirky behavior when using mousewheel
    window.addEvent('domready', function() {

    if (Browser.Engine.trident) {
       if ($('ddlSelectOrder') != null) {
                $('ddlSelectOrder').addEvent('mousewheel', function(event) {
                    return false;
                });
            }
        }
    });