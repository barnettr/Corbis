/****************************************************
    Corbis UI Global Nav
***************************************************/

CorbisUI.GlobalNav = {
    vars: {
        _isBrowseFlyoutMousedown: null
    },
    ShowLanguages: function(show) {
        var div = $('languageSelectorMenu');
        var divBottom = $$('#languageSelectorMenu .LanguageMenuBottom');
        var divDropShadow = $('languageSelectorMenuDropShadow');

        if (show) {
            if (div) {
                div.setStyle('display', 'block');
                var coords = divBottom[0].getCoordinates();
                if (divDropShadow) {
                    divDropShadow.setStyle('display', 'block');
                    divDropShadow.setStyle('position', 'absolute');
                    divDropShadow.setStyle('top', coords.top + 4);
                    divDropShadow.setStyle('left', coords.left + 4);
                    divDropShadow.setStyle('width', coords.width);
                    divDropShadow.setStyle('height', coords.height);
                }
            }
        } else {
            div.setStyle('display', 'none');
            divDropShadow.setStyle('display', 'none');
        }
    },
    showBrowseMenu: function(show) {
        //Change chevron graphic to point down
        $$('#BrowseMenu .BrowseImages .Icon img').setStyle('background-position', '0px -8px');

        if ($('dropDownMenuDivWindow')) {
            $('dropDownMenuDivWindow').setStyle('display', 'block');
        } else {
            var el = $('dropDownMenuDiv');

            el.setStyle('display', 'block');
            var elDimensions = el.getCoordinates();
            var properties = {
                title: '',
                indexLevel: 11111,
                collapsible: false,
                minimizable: false,
                useCanvasControls: false,
                cornerRadius: 0,
                headerHeight: 0,
                footerHeight: 0,
                padding: 0,
                scrollbars: false,
                closable: true,
                type: 'window',
                id: el.getProperty('id') + "Window",
                height: elDimensions.height,
                width: elDimensions.width,
                x: elDimensions.left + 20,
                y: elDimensions.top + 30,
                content: '',
                draggable: false,
                resizable: false
            };
            if (!Browser.Engine.trident) {
                el.setStyle("left", "20");
                el.setStyle("top", "30");
            }
            MochaUI.NewWindowFromDiv(el, properties);
            $('dropDownMenuDivWindow_titleBar').setStyle('background', 'transparent');
            $('dropDownMenuDivWindow_contentWrapper').setStyle('background', 'transparent');



        }
        $(document.body).addEvent('mousedown', function() {
            setTimeout('CorbisUI.GlobalNav.hideBrowseMenu()', 10);
        });
    },

    hideBrowseMenu: function() {

        if (!CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown) {
            //Change chevron graphic to point right
            $$('#BrowseMenu .BrowseImages .Icon img').setStyle('background-position', '0px -18px')
            try { $('dropDownMenuDivWindow').setStyle('display', 'none'); } catch (er) { }
        }

    },
    SetupBrowseNav: function() {


        //Find the 'Motion' link in dropdown and turn it white
        var spanList = $('dropDownMenuDiv').getElements('span');
        spanList.each(function(el) {

            if (el.id.indexOf('Creative') >= 0) {
                var theLink = el.getParent().getParent();
                theLink.href = "javascript:void(0);";
                theLink.setStyle('cursor', 'default');
                theLink.addEvents('click', function() { return false; });
            }
            if (el.id.indexOf('Editorial') >= 0) {
                var theLink = el.getParent().getParent();
                theLink.href = "javascript:void(0);";
                theLink.setStyle('cursor', 'default');
                theLink.addEvents('click', function() { return false; });
            }
            if (el.id.indexOf('Motion') >= 0) {
				var theLink = el.getParent().getParent();
				theLink.setProperty('target', '_blank');
				theLink.href = "http://www.corbismotion.com/";
                el.setStyle('color', 'white');
                el.getParent().getParent().addEvent('mouseover', function() {
                    el.getParent().getParent().getParent().addClass('MotionHover');
                });
                el.getParent().getParent().addEvent('mouseout', function() {
                    el.getParent().getParent().getParent().removeClass('MotionHover');
                });
            }

        });
    },
    ChangeMotionLinkColor: function() {

    },
    RefreshSignInStatus: function() {
        var SignInStatusReq = new Request({
            method: 'post',
            url: '/src/Authentication/SignInStatusRefresh.aspx',
            onSuccess: function(response) {
                if ($('SignInStatus')) {
                    $('SignInStatus').set('html', response);
                }
                if (window.opener) {
                    try {
                        var openerDocument = $(window.opener.document);
                        var signInStatus = openerDocument.getElement('#SignInStatus');
                        if (signInStatus) {
                            signInStatus.set('html', response);
                        }
                    }
                    catch (e) { }
                }
            }
        }).send();
    },
    RefreshCartStatus: function(gotoCart) {
        var CheckoutWidgetReq = new Request({
            method: 'post',
            url: '/src/Navigation/CheckoutControlRefresh.aspx'
        });
        if (gotoCart) {
            CheckoutWidgetReq.onSuccess = function(response) {
                if ($('CheckoutWidget')) {
                    $('CheckoutWidget').set('html', response);
                }
                GoToCart();
            };
        } else {
            CheckoutWidgetReq.onSuccess = function(response) {
                if ($('CheckoutWidget')) {
                    $('CheckoutWidget').set('html', response);
                }
                if (window.opener) {
                    try {
                        var openerDocument = $(window.opener.document);
                        var checkoutWidget = openerDocument.getElement('#CheckoutWidget');
                        if (checkoutWidget) {
                            checkoutWidget.set('html', response);
                        }
                    }
                    catch (e) { }
                }
            }
        }
        CheckoutWidgetReq.send();
    },
    RefreshGlobalNav: function() {
        this.RefreshSignInStatus();
        this.RefreshCartStatus();
        try {
            // Bubble up to any parent windows
            if (parent.window.location != window.location) {
                parent.CorbisUI.GlobalNav.RefreshGlobalNav();
            }
        } catch (ex) { }
    },
    SessionTimedOut: function() {
        MochaUI.Windows.instances.each(function(instance) {
            MochaUI.CloseModal(instance.options.id.replace("Window", ""));
        });
        OpenModal('sessionTimeout');
    }
}