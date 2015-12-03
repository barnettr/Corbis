// NEW WINDOW FROM DIV
MochaUI.extend({
    NewWindowFromDiv: function(el, props) {
        if (!$('windowUnderlay')) {
            MochaUI.underlayInitialize();
        }
        // Create window
        new MochaUI.Window(props);
        el.injectInside($(el.getProperty('id') + "Window_content"));
        $(el.getProperty('id') + "Window").injectInside(document.forms[0]);
    },

    NewWindowFromDivHTML: function(el) {
        var title = el.getElement('h3.mochaTitle');
        var properties = {
            collapsible: false,
            minimizable: false,
            contentBgColor: '#E8E8E8',
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
            closable: true,
            type: 'window',
            id: el.getProperty('id') + "Window"

        };

        var elInnerHTML = el.innerHTML;

        if (arguments.length > 1) {
            for (var i = 1; i < arguments.length; i++) {
                var regex = new RegExp('\\{' + (i - 1) + '\\}', 'g');
                elInnerHTML = elInnerHTML.replace(regex, arguments[i]);
            }
        }

        properties.content = elInnerHTML;

        if (title) {
            properties.title = title.innerHTML;
        }

        // Create window
        new MochaUI.Window(properties);
        MochaUI.dynamicResize($(el.getProperty('id') + "Window"));
    },

    // NEW MODAL FROM DIV

    NewModalFromDiv: function(el, options) {
        el.setStyle('position', 'absolute');
        el.setStyle('top', '-1000px');
        el.setStyle('display', 'block');
        var title = el.getElement('h3.mochaTitle');
        var elDimensions = el.getSize();
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
            id: el.getProperty('id') + "Window",
            height: elDimensions.y,
            width: elDimensions.x,
            content: ''
        };

        if (options) properties = $merge(properties, options);
        // Create window
        new MochaUI.Window(properties);
        el.setStyle('top', '');
        el.injectInside($(el.getProperty('id') + "Window_content"));
        $(el.getProperty('id') + "Window").injectInside(document.forms[0]);
    },

    HideModal: function(el) {
        if ($(el + "Window")) {
            $(el + "Window").setStyle('display', 'none');
            /*if (OpenModalCount == 0) {
            $('modalOverlay').setStyle('opacity', 0);
            if (Browser.Engine.trident4) {
            $('modalFix').setStyle('display', 'none');
            }
            }*/

            /* 
            NEW - Chris
            better way to detect if we should hide
            the modalOverlay, although slightly slower
            */
            var items = $$('div.mochaOverlay');
            var modalCount = 0;
            items.each(function(el) {
                var parent = el.getParent('div.mocha');
                // Z-index set to 11000 for modals
                if (parent.getStyle('display') != 'none' && parent.getStyle('z-index') == '11000') {
                    modalCount++;
                }
            });
            if (modalCount == 0) {
                if ($('modalOverlay')) {
                    $('modalOverlay').setStyle('opacity', 0);
                }

                if (Browser.Engine.trident4) {
                    $('modalFix').setStyle('display', 'none');
                }
            }

        }
    },

    ModalExists: function(el) {
        if ($(el + "Window")) {
            return true;
        } else {
            return false;
        }
    },

    ShowModal: function(el) {
        if ($(el + "Window")) {
            $(el + "Window").setStyle('display', 'block');
            if (!$('modalOverlay')) {
                MochaUI.Modal = new MochaUI.Modal();
            }
            $('modalOverlay').setStyle('opacity', 0.4);
            if (Browser.Engine.trident4) {
                $('modalFix').setStyle('display', 'block');
            }
        }
    },

    CloseModal: function(el) {
        var windowEl = $(el + 'Window')
        if (windowEl) {
            // Only hide the modal layer if this is the last window
            var items = $$('div.mochaOverlay');
            var modalCount = 0;
            items.each(function(el) {
                var parent = el.getParent('div.mocha');
                // Z-index set to 11000 for modals
                if (parent.getStyle('display') != 'none' && parent.getStyle('z-index') == '11000') {
                    modalCount++;
                }
            });
            if (modalCount <= 1) {
                if ($('modalOverlay')) {
                    $('modalOverlay').setStyle('opacity', 0);
                }

                if (Browser.Engine.trident4) {
                    $('modalFix').setStyle('display', 'none');
                }
            }

            var instances = MochaUI.Windows.instances;
            var currentInstance = instances.get(windowEl.id);
            if (windowEl != $(windowEl) || currentInstance.isClosing) return;
            currentInstance.isClosing = true;
            currentInstance.fireEvent('onClose', windowEl);
            if (currentInstance.check) currentInstance.check.destroy();
            windowEl.removeEvents();
            windowEl.setStyle('visibility', 'hidden');

            //work around for IE issue where all textboxes gets locked up when user open modal in a modal
            if (Browser.Engine.trident) {
                try {
                    var newTextBox = new Element('input', {
                        'id': 'dummyInput',
                        'styles': {
                            'position': 'absolute',
                            'top': parent.window.document.documentElement.scrollTop
                        }
                    });
                    newTextBox.inject($(parent.document).getElement('body')).focus();
                    $(parent.document).getElement('body').focus();
                    newTextBox.dispose();
                }
                catch (e) { };
            }

            // Why set timeout?  Because MS.AJAX has something connected to the iframe that breaks in IE
            setTimeout("$('" + windowEl.id + "').destroy()", 20);
        }
    },

    // NEW MODAL FROM DIV HTML

    NewModalFromDivHTML: function(el) {
        var title = el.getElement('h3.mochaTitle');
        var properties = {
            collapsible: false,
            minimizable: false,
            contentBgColor: '#E8E8E8',
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
            closable: true,
            type: 'modal',
            id: el.getProperty('id') + "Window"

        };
        var elInnerHTML = el.innerHTML;
        var width = el.getStyle('width');
        if (!isNaN(parseInt(width, 10))) {
            properties.width = parseInt(width, 10);
        }

        if (arguments.length > 1) {
            var replaceText = arguments[1];
            for (var i = 0; i < replaceText.length; i++) {
                var regex = new RegExp('\\{' + (i) + '\\}', 'g');
                elInnerHTML = elInnerHTML.replace(regex, replaceText[i]);
            }
        }

        properties.content = elInnerHTML;

        if (title) {
            properties.title = title.innerHTML;
        }

        // Create window
        new MochaUI.Window(properties);
        ResizeModal(el.getProperty('id'));
    },

    // RECENTER A MOCHA UI WINDOW

    recenterWindow: function(windowEl) {

        if (!windowEl) {
            MochaUI.Windows.instances.each(function(instance) {
                if (instance.isFocused == true) {
                    windowEl = instance.windowEl;
                }
            });
        }

        var currentInstance = MochaUI.Windows.instances.get(windowEl.id);
        var options = currentInstance.options;
        var dimensions = options.container.getCoordinates();
        var scrollPos = document.getScroll();
        var windowPosTop = (dimensions.height * .5) - ((options.height + currentInstance.headerFooterShadow) * .5) + scrollPos.y;
        var windowPosLeft = (dimensions.width * .5) - (options.width * .5);

        if (MochaUI.options.useEffects == true) {
            currentInstance.morph.start({
                'top': windowPosTop,
                'left': windowPosLeft
            });
        }
        else {
            windowEl.setStyles({
                'top': windowPosTop,
                'left': windowPosLeft
            });
        }
    }
});

if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Popup = new Class({

    Implements: [Options],

    //options  
    options: {
        showModalBackground: true,
        centerOverElement: null,
        positionVert: 'top',
        positionHoriz: 'middle',
        closeOnLoseFocus: false,
        onHide: null,
        createFromHTML: false,
        replaceText: [], 
        //mike:this is for popup on another popup
        backgroundElement:null
    },

    setCloseOnLoseFocus: function(elementId) {
        if (this.options.closeOnLoseFocus) {
            if (this.options.onHide) {
                $('modalOverlay').addEvent('click', this.options.onHide);
            }
            if (this.options.createFromHTML) {
                $('modalOverlay').addEvent('click', function() { MochaUI.CloseModal(elementId); });
            } else {
                $('modalOverlay').addEvent('click', function() { MochaUI.HideModal(elementId); });
            }
        }
    },

    setPosition: function(elementId) {
        var el = $(elementId + "Window");
        var scrollPos = document.getScroll();
        
        var backgroundEle = $(this.options.backgroundElement);
        if (backgroundEle)
        {
            el.setStyle("zIndex", backgroundEle.getStyle("zIndex") + 2);
        }

        if (this.options.centerOverElement != null) {
            var openerElement = $(this.options.centerOverElement);
            if (openerElement) {
                var openerPos = openerElement.getCoordinates();
                el.setStyle("position", "absolute");

                switch (this.options.positionVert) {
                    case "top":
                        el.setStyle("top", openerPos.top - el.getCoordinates().height + 5);
                        break;
                    case "middle":
                        el.setStyle("top", openerPos.top + (openerPos.height / 2) - (el.getCoordinates().height / 2));
                        break;
                    case "bottom":
                        el.setStyle("top", openerPos.top);
                        break;
                    default:
                        if (!isNaN(parseInt(this.options.positionVert, 10))) {
                            el.setStyle("top", openerPos.top + parseInt(this.options.positionVert, 10));
                        }
                        break;
                }

                switch (this.options.positionHoriz) {
                    case "left":
                        el.setStyle("left", openerPos.left + openerPos.width - el.getCoordinates().width);
                        break;
                    case "middle":
                        el.setStyle("left", openerPos.left + (openerPos.width / 2) - (el.getCoordinates().width / 2));
                        break;
                    case "right":
                        el.setStyle("left", openerPos.left);
                        break;
                    default:
                        if (!isNaN(parseInt(this.options.positionHoriz, 10))) {
                            el.setStyle("left", openerPos.left + parseInt(this.options.positionHoriz, 10));
                        }
                        break;
                }

            }
        } else {
            var styleTop = scrollPos.y + (window.getSize().y / 2) - (el.getCoordinates().height / 2);
            var styleLeft = (window.getSize().x / 2) - (el.getCoordinates().width / 2);
            if (styleTop < 10) { styleTop = 10; }
            el.setStyle("top", styleTop);
            el.setStyle("left", styleLeft);
        }

    },

    initialize: function(elementId, options) {
        this.setOptions(options);

        if (!$('modalOverlay')) {
            MochaUI.Modal = new MochaUI.Modal();
        }
        if (!$('windowUnderlay')) {
            MochaUI.underlayInitialize();
        }

        this.setCloseOnLoseFocus(elementId);

        if (!this.options.createFromHTML) {
            if ($(elementId + "Window")) {
                $(elementId + "Window").setStyle('display', 'block');
                if (Browser.Engine.trident4) {
                    $('modalFix').setStyle('display', 'block');
                }
            } else {
                MochaUI.NewModalFromDiv($(elementId), options);
            }
        } else {
            MochaUI.NewModalFromDivHTML($(elementId), this.options.replaceText);
        }

        this.setPosition(elementId);

        $('modalOverlay').setStyle('display', 'block');
        if (!this.options.showModalBackground) {
            $('modalOverlay').setStyle('opacity', .01);
        } else {
            $('modalOverlay').setStyle('opacity', .4);
        }
    }
});    