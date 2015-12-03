//Moved code from here to AddToLightbox.ascx

// Due to the similarity of function for search load progress, attach the progress handling function here -- MIKE

/***************************
    SCRIPT VARIABLES
****************************/
var timer, timer2, thumbTips, noLicense, noCat, noColor, noPhoto;
var _clarifyCheckCount = 0;
//var isAnonymous  = CorbisUI.GlobalVars.SearchResults.isAnonymous;

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
/////////////////////////////////////
// CONSTRUCTORS
// generic templates incorporated
// by other objects with Implement
/////////////////////////////////////
CorbisUI.SearchConstructors = {
	
    /* block constructor for lightbox and quickpic item */
    pinkyBlockConstructor : new Class({
        
        closeButton: null,
        cartButton: false,
        
        mediaUID: null,
        corbisID: null,
        
        licenseModel: null,
        
        block: null,
        
        createBlock: function(){
            //console.log(this.constructorType);
            
            var objRef = $('ProductResults')
                            .getElement('span[productuid='+this.mediaUID+']')
                            .retrieve('objectReference');
                            
            this.licenseModel = objRef.licenseModel;
                            
            var Wrap = new Element('div').addClass(this.constructorType+'Block');
            
            var closeWrap = new Element('div')
                            .addClass('hoverBtn')
                            .addClass('closeIcon')
                            .inject(Wrap);
            
            var closeInput = new Element('input').setProperties({
                'class': 'hovable',
                'type': 'submit',
                'value': ''
            }).inject(closeWrap);
            
            this.closeButton = closeInput;
            
            var thumbBlock = new Element('div').setProperties({
                'class': 'thumbWrap',
                'onclick': "EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id="+this.corbisID+"&puid="+this.mediaUID+"');return false;"
            }).setStyles({
                'cursor': 'pointer',
                'background': '#262626'  
            }).inject(Wrap);
            
            var imageRef = objRef.thumbWrap.getFirst();
                            
            var imageClone = imageRef.clone()
                                .setStyle('margin-top',0);
                                
            imageClone.src = imageClone.src.replace('/170/','/thumb/');
            
            imageClone = imageClone.clone();
            
            imageClone.inject($('hiddenWorkshop'));
            
            var coords = imageClone.getCoordinates();
            imageClone.setProperties({
                width: coords.width,
                height: coords.height
            });
            
            imageClone.inject(thumbBlock);
                              
            imageClone.setStyles(CorbisUI.SearchConstructors.helpers.ScaleImage(imageClone,{marginTop: 90, width: 90, height: 90}));
            
            var licenseBlock = new Element('div')
                                .addClass(this.licenseModel+'color infoBox')
                                .inject(Wrap);
            
            var licenseWrap = new Element('div').addClass('license').inject(licenseBlock);
            
            var licenseText = new Element('span').set('text',this.licenseModel).inject(licenseWrap);
            
            if(this.constructorType == 'lightbox'){
                // need to attach event to this
                var cartAnchor = new Element('a').inject(licenseBlock);

                var cartIcon = new Element('div').addClass("ICN_cart").inject(cartAnchor);
                this.cartButton = cartAnchor;
            }
            
            this.block = Wrap;
            
        }
        
    }),
    
    helpers: {
    
        ScaleImage : function(ele,options){

            var Base = {
                marginTop: 90, //basic dimensions of container
                width: 90, // minimum width
                height: 90 // minimum height
            };
            if(options) Base = $merge(Base,options);

            //if(!margin) margin = 128;
            
            //console.log(Base);
            
            var OrigCoords = ele.getCoordinates();
            var Coords = {width: 0, height: 0};
            Coords.width = OrigCoords.width;
            Coords.height = OrigCoords.height;
            
            var newValues = Base;
            
            for(var n in Coords){
                var m = Base[n];
                if(Coords[n] > m && Base[n]){
                    var o = (n == 'width') ? 'height' : 'width';
                    var r = m / Coords[n];
                    newValues[n] = m;
                    newValues[o] = Math.ceil(Base[o]*r);
                }
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
         
            //console.log('NEW VALUES');
            //console.log(newValues.toSource());
            
            return newValues;
        }
    
    }
};


/////////////////////////////////////
// MODELS
/////////////////////////////////////
CorbisUI.SearchModels = {
	
    // product block model
    ProductBlock : new Class({
        Implements: [Options,Events],
        
        productBlock: null,
        
        LB: null,
        QPon: null,
        QPoff: null,
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
        
        QPenabled: false,
        CTenabled: false,
        
        basketCase: false,
        
        thumbWrap: null,
        
        initialize: function(el){
            if(!el) return false;
            this.productBlock = $(el);
        
            this.licenseModel = this.productBlock.getProperty('licenseModel');
            this.productUID = this.productBlock.getProperty('productUID');
            this.corbisID = this.productBlock.getProperty('corbisID');
            
            this.thumbWrap = this.productBlock.getElement('.thumbWrap');
            
            this.isSelected = this.productBlock.hasClass('ProductSelected');
            
            this.QPenabled = (this.productBlock.getElement('.ICN_quickpic') == null)? false : true;
            this.CTenabled = (this.productBlock.getElement('.ICN_cart') == null)? false : true;
            
            this.LB = this.productBlock.getElement('.ICN_lightbox');
            
            this.activeStates.LB = this.LB.hasClass('ICN_lightbox_selected');
            
            if(this.QPenabled){
                this.QPon = this.productBlock.getElement('.QP_on');
                this.QPoff = this.productBlock.getElement('.QP_off');
                this.activeStates.QP = this.QPon.hasClass('ICN_quickpic_selected');
            }
            
            if(this.CTenabled){
                this.CT = this.productBlock.getElement('.ICN_cart');
                this.basketCase = this.CT.hasClass('ICN_cartBasket');
                
                if(this.basketCase){
                    this.activeStates.CT = this.CT.hasClass('ICN_cartBasket_selected');
                }else{
                    this.activeStates.CT = this.CT.hasClass('ICN_cart_selected');
                }
                
            }
            
            // store objecet reference on product block
            this.productBlock.store('objectReference',this);
            
            // add test click stuff
            this.productBlock.getElement('div.LT').addEvent('click',this.test.bindWithEvent(this));
            
        },
        
        test: function(event){
            console.log('+=== ITEM INFORMATION ===================================================+');
            
            console.log('     license: '+this.licenseModel);
            console.log('     corbisID: '+this.corbisID);
            console.log('     productUID: '+this.productUID);
            console.log('     isSelected: '+this.isSelected);
            console.log('     QPenabled: '+this.QPenabled);
            console.log('     CTenabled: '+this.CTenabled);
            console.log('     basketCase: '+this.basketCase);
            console.log('     activeStates: '+this.activeStates.toSource());
        },
        
        updateIcon: function(type,action){
            //console.log(type+' : '+action);
            this[action](type);
        },
        
        selectIcon: function(type){
            switch(type){
                case "LB":
                    //console.log(this.LB.hasClass('ICN_lightbox_selected'));
                    if(!this.LB.hasClass('ICN_lightbox_selected')) this.LB.toggleClass('ICN_lightbox_selected');
                    this.activeStates.LB = true;
                    break;
                case "QP":
                    if(this.QPenabled){
                        if(!this.QPon.hasClass('ICN_quickpic_selected')) this.QPon.addClass('ICN_quickpic_selected');
                        if(!this.QPoff.hasClass('ICN_quickpic_selected'))this.QPoff.addClass('ICN_quickpic_selected');
                        this.activeStates.QP = true;
                    }
                    break;
                case "CT":
                    if(this.CTenabled){
                        if(this.basketCase){
                            if(!this.CT.hasClass('ICN_cartBasket_selected')) this.CT.addClass('ICN_cartBasket_selected');
                        }else{
                            if(!this.CT.hasClass('ICN_cart_selected')) this.CT.addClass('ICN_cart_selected');
                        }
                        this.activeStates.CT = true;
                    }
                    break;
            }
            // update productblock glow
            this.highlightBlock();
        },
        
        deselectIcon: function(type){
            //console.log('deselectIcon(\''+type+'\')');
            switch(type){
                case "LB":
                    if(this.LB.hasClass('ICN_lightbox_selected')) this.LB.toggleClass('ICN_lightbox_selected');
                    this.activeStates.LB = false;
                    break;
                case "QP":
                    if(this.QPenabled){
                        if(this.QPon.hasClass('ICN_quickpic_selected')) this.QPon.toggleClass('ICN_quickpic_selected');
                        if(this.QPoff.hasClass('ICN_quickpic_selected'))this.QPoff.toggleClass('ICN_quickpic_selected');
                        this.activeStates.QP = false;
                    }
                    break;
                case "CT":
                    if(this.CTenabled){
                        if(this.basketCase){
                            if(this.CT.hasClass('ICN_cartBasket_selected')) this.CT.toggleClass('ICN_cartBasket_selected');
                        }else{
                            if(this.CT.hasClass('ICN_cart_selected')) this.CT.toggleClass('ICN_cart_selected');
                        }
                        this.activeStates.CT = false;
                    }
                    break;
            }
            // update productblock glow
            this.unhighlightBlock();
        },
        
        highlightBlock: function(){
            if(this.activeStates.LB
                || (this.QPenabled && this.activeStates.QP)
                || (this.CTenabled && this.activeStates.CT))
            {
                if(!this.productBlock.hasClass('ProductSelected')) this.productBlock.toggleClass('ProductSelected');
            }
        },
        
        unhighlightBlock: function(){
            if(!this.activeStates.LB
                && !this.activeStates.QP
                && !this.activeStates.CT)
            {
                if(this.productBlock.hasClass('ProductSelected')) this.productBlock.toggleClass('ProductSelected');
            }
        },
        
        refreshObject: function(){
            // the whole purpose of this method is
            // because update panel messes our scripts
            // up by redrawing the dom when we have 
            // JS object attached to certain dom elements
            
            var item = $('ProductResults').getElement('span[productuid='+this.productUID+']');
            if(item){
                // reattach this to the current dom object
                this.initialize(item);
                // reattach tooltips - stupid update panel
                
            } 
            //(function(){registerTooltips(false);}).delay(200);
            return this;
        },
        
        addProductToCart: function(){
	        if (!this.activeStates.CT)
	        {
		        var addToCart = new CorbisUI.Cart.AddToCart(this.productUID);
		        addToCart.context = this;
		        addToCart.onSuccess = this.refreshCartItem;
		        addToCart.addOfferingToCart(); 
	        }
        }, 
    	
        refreshCartItem: function(results){
            var thisItem = this.context;
            if (!thisItem)
            {
                thisItem = this;
            }
    		
    		thisItem.selectIcon('CT');
    		
	        //update lighbox buddy
	        var lightboxItem = $('LBXContainer').getElement('div[mediauid='+thisItem.productUID+']');
	        if (lightboxItem)
	        {
		        var refreshButton = $('SBBX_lightboxes').getElement('input.hdn'); 
		        refreshButton.value = $('SBBX_lightboxes').getElement('select.lightboxList').selectedValue; 
		        refreshButton.onclick();
	        }

	        //update cart count
	        var cartCounter = $('cartCount');
	        if (cartCounter)
	        {
		        cartCounter.setProperty('text', results);
	        }
        }
    	
    }),
    
    /* model for lightbox item */
    
    lightboxBlock :  new Class({
    
        Implements: [CorbisUI.SearchConstructors.pinkyBlockConstructor, Options, Events],
        
        constructorType: 'lightbox',
        
        options: {},

        initialize: function(mediaUID,corbisID,options){
            //$extend(this,CorbisUI.Search.pinkyBlockConstructor);
            
            if(options) this.setOptions(options);
            
            this.mediaUID = mediaUID;
            this.corbisID = corbisID;
            
            this.createBlock();
            
            // setup block here
            
            
            //inject it into the lightbox pane
            $('LBXContainer').grab(this.block,'top');
        },
        
        // events
        closeButtonEvent: function(){
        
        },
        
        cartButtonEvent: function(){
        
        }
    
    }),
    
    /* model for quickpic item */
    quickPicBlock :  new Class({
    
        Implements: [CorbisUI.SearchConstructors.pinkyBlockConstructor, Options, Events],
        
        constructorType: 'quickPic',
        
        options: {},
        
        initialize: function(mediaUID,corbisID,options){
            //$extend(this,CorbisUI.Search.pinkyBlockConstructor);
            
            if(options) this.setOptions(options);
            
            this.mediaUID = mediaUID;
            this.corbisID = corbisID;
            
            this.createBlock();
            
            // setup block here
            
            
            //inject it into the lightbox pane
            var target = $('quickPicsContainer')
                            .getFirst()
                            .getElement('.centerMe');
                            
            this.block.inject(target,'before');
        },
        
        // events
        closeButtonEvent: function(){
        
        }
    
    })

};	

/////////////////////////////////////
// MAIN SEARCH NAMESPACE OBJECT
/////////////////////////////////////

CorbisUI.Search = {

	MoveQuickPick : function(ctl, ctl2,corbisId,Url128,licenseModel,aspectRatio,title,toQuickPick)
    {
        // see if quickpic tab is active, if not then "click" it
        // this interacts with objects in SearchBuddy.js
        if(CorbisUI.SearchBuddy.init.activeTab != "quickpic"){
           var qp = CorbisUI.SearchBuddy.init.tabs.get("quickpic");
           qp.el.fireEvent('click'); 
        }
    	
        if (toQuickPick)
        {
    		
	        Corbis.Web.UI.Search.SearchScriptService.AddItemToQuickPick(corbisId,Url128,licenseModel,aspectRatio,title,updateQuickPicView, methodFailed);
            var item = $(ctl);
	        item.getParent().addClass('hdn');
	        var hideQP = item.getParent('ul').getElement('.QP_off');
	        if (hideQP)
	        {
	            hideQP.removeClass('hdn');
	        }
        }
        else
        {
	        Corbis.Web.UI.Search.SearchScriptService.RemoveItemFromQuickPick(corbisId,updateQuickPicView, methodFailed)
	        var item = $(ctl);
	        item.getParent().addClass('hdn');
	        var ulItem=item.getParent('ul');
	        if(ulItem)
	        {		
	            var showQP = ulItem.getElement('.QP_on');
	            if (showQP)
	            {
	                showQP.removeClass('hdn');
                }
	        }
        }

    }
};

/////////////////////////////////////
// HANDLER FUNCTIONS
/////////////////////////////////////


CorbisUI.Search.Handler = {
	
    selectQuickpicIcon : function(id,idType){
        (function(){
            $('ProductResults')
                .getElement('span['+idType+'='+id+']')
                .retrieve('objectReference')
                .refreshObject()
                .updateIcon('QP','selectIcon');
        }).delay(200);
    },

    deselectQuickpicIcon : function(id,idType){
        (function(){
            $('ProductResults')
                .getElement('span['+idType+'='+id+']')
                .retrieve('objectReference')
                .refreshObject()
                .updateIcon('QP','deselectIcon');
        }).delay(200);
    },

    syncLightboxToImages : function(){
        //deselect all selected images
        $$('#ProductResults span.ProductBlock').each(function(el){
            var productBlock = el.retrieve('objectReference');
            if(productBlock.activeStates.LB)
            {
	            productBlock.updateIcon('LB','deselectIcon');    
            }
        });
    	
        //Update images that's in current lightbox
        $$('#SBBX_lightboxes div.lightboxBlock').each(function(el){
            var lightboxImage = $('ProductResults').getElement('span[productuid='+el.getProperty('mediaUid')+']');
            if (lightboxImage != null)
            {
                lightboxImage.retrieve('objectReference')
                .refreshObject()
                .updateIcon('LB','selectIcon');
            }
        });
    },

    addProductToCart : function(corbisId){
        var product = $('ProductResults').getElement('span[corbisid='+corbisId+']');
        if (product)
        {
            product.retrieve('objectReference').refreshObject().addProductToCart();
        }
    },
    
    refreshCartItem: function(corbisId, cartItemCount)
    {
        var product = $('ProductResults').getElement('span[corbisid='+corbisId+']');
        if (product)
        {
            product.retrieve('objectReference').refreshObject().refreshCartItem(cartItemCount);
        }
    }
};

/////////////////////////////////////
// WINDOW DOMREADY EVENTS
// if you add new functions,
// add them in statics
/////////////////////////////////////


window.addEvent('domready', function() {
    if (window.location.href.toLowerCase().contains('searchresults.aspx')) {
        registerTooltips(false);
        setEditorialCheckedState(true);
        noPeopleChanged(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.noPeople);

        // apply product block objects to dom elements
        $('ProductResults').getElements('span.ProductBlock').each(function(el) {
            new CorbisUI.SearchModels.ProductBlock(el);
        });

        // temp...remove later
        //var testData = $('ProductResults').getFirst('span.ProductBlock');
        //if(testData) new CorbisUI.SearchModels.lightboxBlock(testData.getProperty('productuid'),testData.getProperty('corbisid'));

        //synchronize the lightbox with the images
        CorbisUI.Search.Handler.syncLightboxToImages();

        //search term clarification
        var displayClarificationPopup = $(CorbisUI.GlobalVars.SearchResults.showClarificationPopup);
        if (displayClarificationPopup.value == 'true' || displayClarificationPopup.value == 'True') {
            OpenAmbiguousModal();
        }

        // *********************************************
        // Show Options Applied Style in the Search box
        // *********************************************

        var getOptionsAppliedStyle = $(CorbisUI.GlobalVars.Search.showOptionsAppliedStyle);
        if (getOptionsAppliedStyle.value == 'true' || getOptionsAppliedStyle.value == 'True') {
            applyOptionStyles();
        }

        var getZeroSearchResults = $(CorbisUI.GlobalVars.SearchResults.zeroSearchResults).value;
        if (getZeroSearchResults == 'True') {
            applyZeroResultsStyles();
        }
    }
});


//function refreshWithKeyWords(url)
//{
//    window.location.href=url;
//}

/////////////////////////////////////
// SERVICE FUNCTIONS?
/////////////////////////////////////

function updateQuickPicView(results, context, methodName)
{  
    var corbisId = results;
    if (corbisId=="")
    {
        //TODO: localize
        alert("You've reached the 20 - image lmit for a Quick pic download. Please download your images now and then return for more.");
    }
    else
    {
        if (methodName == 'AddItemToQuickPick')
        {
    		CorbisUI.Search.Handler.selectQuickpicIcon(corbisId,'corbisid');
        }
        else
        {
    		CorbisUI.Search.Handler.deselectQuickpicIcon(corbisId,'corbisid');
        }
        var quickpicField = $('quickPicsContainer').getElement('input');  
        quickpicField.onclick();
    }
}

function methodFailed(results, context, methodName)
{
    //console.log(results);
    //TODO: use mochaUI for alert window
    alert(results.get_message());
}

function endRequestHandler(sender, args) {
    //alert(sender._postBackSettings.sourceElement.id);
    if (sender._postBackSettings.sourceElement && 
                ( sender._postBackSettings.sourceElement.id.contains('quickpicField')
                  || sender._postBackSettings.sourceElement.id.contains('addToLightboxBtn'))
       )
        return;
    //debugger;
    //console.log('sender=', sender, ' args=' + args);
    //console.log('sourceElementId=', sender._postBackSettings.sourceElement.id);
    scrollTo(0, 0);
}        

function registerTooltips(isFirstTime)
{
    //console.log('in registerTooltips, isFirstTime', isFirstTime);
    if (!showTooltip() || thumbTips) return;
    
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
    thumbTips = new Tips('#ProductResults .thumbWrap', {
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
    // debugger;
    return ($$('.previewOffSelected').length == 0);
}

function InitiateTimer()
{
	if (timer!=null)window.clearInterval(timer);
	timer = window.setInterval("window.clearInterval(timer);if (testSBCategoriesChecked()) secondTimer()", 2000);
}

function testSBCategoriesChecked()
{
    noLicense = !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.rmLicense) && 
        !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.rfLicense);
    noCat = !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.editorial) && 
        !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.creative);
    noPhoto = !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.photography) && 
        !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.illustration);
    noColor = !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.color) && 
        !getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.blackWhite);
        
    var noResults = noLicense || noCat || noColor || noPhoto;
    
    //alert(noResults);
    if (noResults)
    {
        if (!$('noSearchResultsWarningWindow'))
        {
            var sbCoords = $('SearchBuddy').getCoordinates();
            var sbScroll = $(document.body).getScroll();
            var scrollOffset = sbCoords.top + sbScroll.y;
            var vPos = noCat ? 10 : noLicense ? 98 : noPhoto ? 232 : 287;
            vPos += scrollOffset;
            new CorbisUI.Popup('noSearchResultsWarning', { 
                createFromHTML: true,
                showModalBackground: false,
                centerOverElement: 'noSearchResultsWarning',
                closeOnLoseFocus: true,
                positionVert: vPos, 
                positionHoriz: 212,
                replaceText: [ '' ]
            });
            setTimeout(setErrorOptions, 100);
        }
        return false;
    }
    else
    {
        MochaUI.CloseModal('noSearchResultsWarning');
        return true;
    }
}

function setErrorOptions()
{
    var pop = $('noSearchResultsWarningWindow');
    if (!pop) return false;
    
    var items = {
        license : pop.getElement('p[textKey=License]'),
        category : pop.getElement('p[textKey=Category]'),
        color : pop.getElement('p[textKey=Color]'),
        photo : pop.getElement('p[textKey=Photo]')
    }
    
    items.license.setStyle('display', 'none');
    items.category.setStyle('display', 'none');
    items.color.setStyle('display', 'none');
    items.photo.setStyle('display', 'none');
    
    if (noCat)
        items.category.setStyle('display', 'block');
    else if (noLicense)
        items.license.setStyle('display', 'block');
    else if (noColor)
        items.color.setStyle('display', 'block');
    else if (noPhoto)
        items.photo.setStyle('display', 'block');
    
    $('modalOverlay').addEvent('click', function(){
        //$('modalOverlay').setStyle('display', 'none');
        $(this).setStyle('display', 'none');
    });
    $('searchProgIndicator').setStyle('display', 'none');
    
}

function secondTimer()
{
    if (timer2 != null) window.clearInterval(timer);
	timer2 = window.setInterval("window.clearInterval(timer2);invokeSearch(true)", 1500);
	$('searchProgIndicator')
	    .setStyles({
	        'display': 'block',
	        'height': document.body.scrollHeight + 'px',
	        'left': '206px'
	    });
	$('processingFilters').setStyle('display', 'block');
}

function noPeopleChanged(chk)
{
    var checked = getCheckedState(chk);
    $('divNoPeopleIcon')
        .setStyle('background-position', checked ? 'bottom right' : 'top right');

}
function EditorialChanged(checked)
{
    var div = $('EditorialChildren');
    var checks = div.getElements('div.imageCheckbox');

    checks.each(function(cb){
        setCheckedNoEvent(cb, checked)
    });
    InitiateTimer();
}

function setEditorialCheckedState(init)
{
    var editorialCB = $(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.editorial)
    var allUnchecked = true;
    var div = $('EditorialChildren');
    var checks = div.getElements('div.imageCheckbox');

    checks.each(function(cb){
        if (getCheckedState(cb))
            allUnchecked = false
    });
    if (allUnchecked)
        setCheckedNoEvent(editorialCB, false);
    else
        setCheckedNoEvent(editorialCB, true);
    if (typeof(init) == 'undefined') InitiateTimer();
}
//var showTooltip = $$('previewOffSelected');
function openCreateLightbox()
{
	if (!CorbisUI.GlobalVars.SearchResults.isAnonymous)
	{
		new CorbisUI.Popup('createLightboxModalPopup', { 
			showModalBackground: false,
			centerOverElement: 'SearchBuddy',
			closeOnLoseFocus: true,
			positionVert: '60', 
			positionHoriz: '5'
		});    

		//Reposition because getCoordinate() does not work so well for Safari.
		var SB = $('SearchBuddy');
		$('createLightboxModalPopupWindow').setStyles({
			top: SB.offsetTop + window.getScroll().y + 60,
			left: SB.offsetLeft + window.getScroll().x + 5
		});
		
	}
}

function confirmItemAdded(link)//icon_cart_selected
{
    new CorbisUI.Popup('addToCartConfirm', { 
        createFromHTML: true,
        showModalBackground: false,
        centerOverElement: link,
        closeOnLoseFocus: true,
        positionVert: 'top', 
        positionHoriz: 'center',
        replaceText: []
    });
    return false;
}

function OpenAmbiguousModal(element)
{
    new CorbisUI.Popup('ambiguousModal');
    setGlassButtonDisabled(CorbisUI.GlobalVars.SearchResults.clarificationUpdate, true);
      
}
function  applyOptionStyles()
{
    $("Keywords").addClass("optionsApplied");
    $("optionsAppliedDiv").addClass("optionsAppliedDiv");
    $$('#Search .Keywords input').addClass("optionsAppliedInput");
    var browserName = navigator.userAgent;
    var isIE = browserName.match(/MSIE/);
    if (isIE) {
        $("ctl00_mainContent_searchControl_dataContainerDivider").style.cssText = "display:none;";
    }
}

function applyZeroResultsStyles() {
    $('FooterContent').addClass('zeroResults');
    $('SearchBuddy').addClass('zeroResultsBuddy');
}
        
function updateClarificationCount(isChecked)
{
    if (isChecked){
        _clarifyCheckCount++;
    }
    else{ 
        _clarifyCheckCount--;
    }
    setGlassButtonDisabled(CorbisUI.GlobalVars.SearchResults.clarificationUpdate, _clarifyCheckCount < 1);
}

function getClarifactionChecked()
{
    var base = $('ambiguousModal');
    var clarificationGroups = base.getElements('.Clarification');
    var clarificationString = '';
    
    clarificationGroups.each(function(el){
       clarificationString += ',';

       el.getElements('.checkboxWrap').each(function(itemEl){
       
            var cbx = itemEl.getElement('input[type=checkbox]');
            if (cbx.checked){
                cbx.value = 1;
            }
            else{
                cbx.value = 0;
            }
            clarificationString += cbx.value;       
       });   
    });
    
    clarificationString = clarificationString.substring(1);
    $(CorbisUI.GlobalVars.SearchResults.clarificationQueryFlags).value = clarificationString;
    MochaUI.CloseModal('ambiguousModal');
    validateSearch();    
}
