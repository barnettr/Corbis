
/****************************************************
	Corbis Javascript namespace
	
	Search Buddy
	
	=============================================
		Chris Esler, 2008-04-25
	=============================================
****************************************************/

/***************************
    GLOBAL VARIABLES
****************************/
var Floater;

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}

/***************************
    SEARCH BUDDY
****************************/
CorbisUI.SearchBuddy = {
    init: new Class({

        tabs: null,
        buddy: null,

        activeWindow: null,
        activeTab: null,

        initialize: function() {
            //console.log('CorbisUI.SearchBuddy INIT');
            this.buddy = CorbisUI.DomCache.get('SearchBuddy');
            this.tabs = new Hash({});


            var tabItems = this.buddy.getElement('ul.SB_tabs').getChildren();
            //console.log($type(this.tabs));

            tabItems.each(function(el) {
                var id = el.get('id');
                id = id.substr(id.indexOf('SBT_'));
                id = id.replace('SBT_', '');
                //console.log($type(this.tabs));
                switch (id) {
                    case "filters":
                    case "lightboxes":
                    case "quickpic":
                        this.tabs.set(id, new CorbisUI.SearchBuddy.tab(el, this));
                        break;
                }
            }, this);

            this.buddy.onkeypress = function(e) {
                if (!e) var e = window.event;
                e = e || window.event;
                var code = e.keyCode || e.which;
                if (code == 13) {
                    CorbisUI.ExtendedSearch.combineSearchBuddy();
                    e.returnValue = false;
                    return false;
                }
            }

            CorbisUI.SearchBuddy.activateTab = this.activateTab.bind(this);

        },
        // we can active any tab here
        activateTab: function(tabName) {
            if (this.activeTab != tabName) {
                this.tabs.each(function(item) {
                    if (item.name == tabName) item.el.fireEvent('click');
                });
            }
        }

    }),

    // placeholder for alias
    activateTab: null,

    fireLightboxEvent: function(ele) {
        var el = $(ele).getParent().getParent().getParent();

        var lbId = el.get('id').replace('cartBlock_', 'LBX_');
        //el.setStyle('border','1px solid red');

        // check to see if lbx item exists already
        if (!$(lbId)) {
            var lbItem = el.getElement('div.thumbWrap').getFirst().getFirst().getFirst().clone().removeProperties('title', 'class');
            lbItem.setProperty('id', lbId).setStyles({
                'float': 'left',
                'clear': 'both',
                'margin': 4
            })

            var lb = CorbisUI.SearchBuddy.init.tabs.get('lightboxes');

            lb.el.fireEvent('click');
            lb.panel.getElement('div.LBXContainer').grab(lbItem, 'top');
        }
    },

    tab: new Class({
        name: null,
        el: null,
        buddy: null,
        panel: null,
        dataLoaded: false,
        initialize: function(el, buddy) {
            this.el = el;
            this.name = el.get('id').substr(el.get('id').indexOf('SBT_') + 4);
            this.buddy = buddy;
            this.panel = CorbisUI.DomCache.get('SBBX_' + this.name);
            this.el.addEvent('click', this.clickEvent.bindWithEvent(this));
            if (this.el.hasClass('ON')) {
                this.buddy.activeTab = this.name;
            }
        },

        clickEvent: function() {
            //comment out "if" block for bug 15955, based on the spec        
            //            if (CorbisUI.Auth.GetSignInLevel() < 1 && this.name == 'lightboxes') {

            //                //CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, "CorbisUI.CookieEvents.addCookieEvent(function(){CorbisUI.SearchBuddy.activateTab('lightboxes');}); window.location.reload();");
            //            }
            //            else 
            if (this.buddy.activeTab != this.name) {
                var active = this.buddy.tabs.get(this.buddy.activeTab);
                active.el.removeClass('ON');
                active.panel.addClass('hdn');
                this.buddy.activeTab = this.name;
                this.el.addClass('ON');
                this.panel.removeClass('hdn');
                if (this.name == 'lightboxes' && !this.dataLoaded) {
                    if (CorbisUI.GlobalVars.SearchResults.isAnonymous) {
                        $('lightboxProgress').setStyle('display', 'none');
                        CorbisUI.DomCache.get('LBXContainer').getElement('.centerMe').setStyle('display', 'block');

                    }
                    else {
                        //console.log('CALLING: CorbisUI.SearchBuddy.tab.clickEvent IS LIGHTBOX');
                        // set dataLoaded first to block the second call 
                        this.dataLoaded = true;
                        //handle data load
                        // 1 grab lightboxId
                        //var lightboxDropdown = this.getElement('select[name$=lightboxList]');
                        // 2 call web service
                        //var lightboxId = lightboxDropdown.getSelected();

                        var selectedList = CorbisUI.DomCache.get('SBBX_lightboxes', true).getElement('select.lightboxList');
                        if (selectedList != null && selectedList.getSelected()[0] != null) {

                            var selectedIndex = selectedList.selectedIndex;
                            var lightboxId = selectedList.options[selectedIndex].value;

                            //console.log(lightboxId);
                            // 3 return handling
                            //Corbis.Web.UI.Search.SearchScriptService.GetLightBoxItems(lightboxId, this.getLightboxItemsCallback.bind(this));
                            // 4 lightbox items display

                            // LAUNCH the getLightboxItems handler
                            CorbisUI.Handlers.Lightbox.getLightboxItems(lightboxId);
                        }
                    }
                }
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

        initialize: function(el, wrap, footer, progressContainer) {
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

            // add window events
            window.addEvent('scroll', this.windowScroll.bindWithEvent(this));
            window.addEvent('resize', this.windowScroll.bindWithEvent(this));

            // run the Scroll for the first time
            // fixes problem of coming back to a page
            // and the buddy is messed up
            this.windowScroll();
        },

        windowScroll: function() {
            //Close create lightbox popup so we don't have to reposition it.
            HideModal('createLightboxModalPopup');

            // WINDOW SCROLL
            var wScroll = window.getScroll();
            //console.log('WINDOW SCROLL: '+wScroll.toSource());

            // WRAP POSITION
            var pos = this.wrap.getPosition();

            // WRAP COORDS
            var wrapC = this.wrap.getCoordinates();

            // FOOTER COORDINATES
            var fC = this.footer.getCoordinates();

            // WINDOW COORDINATES
            var wiC = window.getCoordinates();

            // Magical powers detect the SCROLL-X
            (wScroll.x > 0) ? this.box.setStyle('left', '-' + wScroll.x + 'px') : this.box.setStyle('left', 0);

            // Magical powers detect the SCROLL-Y

            // if global nav still visible
            if (wScroll.y < this.wC.top) {
                this.box.setStyle('top', (pos.y - wScroll.y) + 10);
            } else {
                // if footer is visible
                if ((wScroll.y + wiC.height) > fC.top) {
                    this.box.setStyle('top', (fC.top - wScroll.y) - wiC.height);
                    // if footer is not visible
                } else {
                    if (this.box.getStyle('top').toInt() != 10) this.box.setStyle('top', 10);
                }
            }

            // detect distance from top of footer

            // get value for footer distance check
            var fCheck = fC.top - wiC.height;

            // if footer visible
            if (wScroll.y > fCheck) {
                this.box.setStyle('bottom', (wiC.height - (fC.top - wScroll.y)) + 10);
                // if footer is not visible
            } else {
                if (this.box.getStyle('bottom').toInt() != -50) this.box.setStyle('bottom', -10);
            }

            var browser = navigator.appName; // detect IE6 & IE7
            var b_version = navigator.appVersion;
            var version = parseFloat(b_version);

            if ((browser == "Microsoft Internet Explorer") && (version <= 7)) {
                var lbxContainerHeight;  // this is for IE only
                var distance = 160;
                var fixMe = 0;
                if (wScroll.y > fCheck) distance = 155;

                lbxContainerHeight = this.box.getStyle('height').toInt() - distance;
                CorbisUI.DomCache.get('LBXContainer').setStyle('height', lbxContainerHeight);
            }


        }

    }),

    getValidateAlertPosition: function(el) {
        var coords = $(el).getCoordinates();
        var filterErrorHeight = CorbisUI.DomCache.get('filterErrorWindow').getCoordinates().height;
        var scroll = $(document.body).getScroll();
        var position = coords.top + scroll.y - filterErrorHeight + 75;

        if (Browser.Engine.webkit) position = position - 182;  //bug 13674, safari bug


        if (position < scroll.y) position = scroll.y + 5;

        return position;
    },


    validateCheckBoxes: function() {

        if ((CorbisUI.GlobalVars.Search.sb_KeywordSearch.value.trim() == '' || CorbisUI.GlobalVars.Search.sb_KeywordSearch.value.trim() == CorbisUI.GlobalVars.Search.sb_SearchImages)) {
            try {
                if ($type($(CorbisUI.GlobalVars.Search.sb_ImageNumbers).value) == 'string') {

                } else {
                    alert(CorbisUI.GlobalVars.Search.sb_EmptySearchStringAlert); return false;
                }
            } catch (e) {
                alert(CorbisUI.GlobalVars.Search.sb_EmptySearchStringAlert); return false;
            }
        }

        CorbisUI.DomCache.get('filterErrorCategories').setStyle('display', 'none');
        CorbisUI.DomCache.get('filterErrorColor').setStyle('display', 'none');
        CorbisUI.DomCache.get('filterErrorPhotography').setStyle('display', 'none');
        CorbisUI.DomCache.get('filterErrorRMRF').setStyle('display', 'none');

        // Validate checkboxes to see what to return
        // Case 1: No Creative, no editorial boxes checked
        if (!getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.creative)) && !getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.editorial))) {
            CorbisUI.DomCache.get('filterErrorCategories').setStyle('display', 'block');
            new CorbisUI.Popup('filterError', { showModalBackground: false, closeOnLoseFocus: true });
            ResizeModal('filterError');
            CorbisUI.DomCache.get('filterErrorWindow').setStyles({
                "left": 185,
                "top": this.getValidateAlertPosition(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.creative)
            });
            return false;
        }
        if (!getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.rmLicense)) && !getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.rfLicense))) {
            CorbisUI.DomCache.get('filterErrorRMRF').setStyle('display', 'block');
            new CorbisUI.Popup('filterError', { showModalBackground: false, closeOnLoseFocus: true });
            ResizeModal('filterError');
            CorbisUI.DomCache.get('filterErrorWindow').setStyles({
                "left": 185,
                "top": this.getValidateAlertPosition(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.rmLicense)
            });
            return false;
        }
        if (!getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.photography)) && !getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.illustration))) {
            CorbisUI.DomCache.get('filterErrorPhotography').setStyle('display', 'block');
            new CorbisUI.Popup('filterError', { showModalBackground: false, closeOnLoseFocus: true });
            ResizeModal('filterError');
            CorbisUI.DomCache.get('filterErrorWindow').setStyles({
                "left": 185,
                "top": this.getValidateAlertPosition(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.photography)
            });
            return false;
        }
        if (!getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.color)) && !getCheckedState($(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.blackWhite))) {
            CorbisUI.DomCache.get('filterErrorColor').setStyle('display', 'block');
            new CorbisUI.Popup('filterError', { showModalBackground: false, closeOnLoseFocus: true });
            ResizeModal('filterError');
            CorbisUI.DomCache.get('filterErrorWindow').setStyles({
                "left": 185,
                "top": this.getValidateAlertPosition(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.color)
            });
            return false;
        }
        return true;
    },

    lightboxLogin: function() {
        CorbisUI.Auth.Check(1, 'execute', 'CorbisUI.SearchBuddy.lightboxLogin_cookieEvent()');
    },

    lightboxLogin_cookieEvent: function() {
        CorbisUI.CookieEvents.addCookieEvent(function() {
            if (Browser.Engine.trident) {
                setTimeout("CorbisUI.SearchBuddy.activateTab('lightboxes')", 500);
            }
        }, {});
        window.location = window.location;
    }

};
	
/***************************
    WINDOW DOM READY
****************************/

CorbisUI.addQueueItem('domReady', 'searchBuddySetup', function() {
    // setup search buddy
    CorbisUI.SearchBuddy.init = new CorbisUI.SearchBuddy.init();

    // initialize search buddy
    Floater = new CorbisUI.SearchBuddy.floater(CorbisUI.DomCache.get('SearchBuddy'), CorbisUI.DomCache.get('contentBar'), CorbisUI.DomCache.get('FooterContent'));
});

