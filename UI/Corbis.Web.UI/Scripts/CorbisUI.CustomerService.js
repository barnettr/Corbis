/****************************************************
    Corbis UI Customer Service
***************************************************/

CorbisUI.CustomerService = new Class({

    vars: {
        popupAnchor: null
    },

    init: function() {
        this.vars.popupAnchor = $('getThankYouWindow');
    },



    clearGetInTouchForm: function() {
        var control = $('formTable').getElements('input');

        // Clear out text inputs
        control.each(function(el) {
            if (el.type == "text") {
                el.value = '';
            }
        }, this);

        // Clear select boxes - set index to 0
        control = $('formTable').getElements('select');
        control.each(function(el) {
            el.selectedIndex = 0;
        }, this);

        // Set textarea to default text
        control = $('formTable').getElements('textarea');
        control.each(function(el) {
            el.value = '';
        }, this);
    },
    OpenThankYouPopup: function() {
        new CorbisUI.Popup('getThankYou', {
            showModalBackground: false,
            closeOnLoseFocus: true,
            positionVert: 'middle',
            positionHoriz: 'bottom'
        });
        this.init();
        this.vars.popupAnchor.setStyles({
            top: 650,
            left: 660
        });
        this.vars.popupAnchor.getElement('.FormButtons').setStyle('margin-top', 5);
        this.vars.popupAnchor.getElement('div .GlassButton').setStyles({
            marginTop: 8,
            marginRight: -10
        });
    }
});
CorbisUI.CustomerService = new CorbisUI.CustomerService();
