/****************************************************
    Corbis UI Form Utilities
****************************************************/

CorbisUI.FormUtilities = {};

// This class binds a country dropdown to its dependant province dropdown.
// To maintain the form state on the server, the province hidden fields are
// used to pass values back to the server. Everything but the disabledOptionText is required
// Author: Ron Gilchrist
CorbisUI.FormUtilities.ProvinceBehavior = new Class({
    Implements: [Options],
    options: {
        countriesDropdownId: false,
        provinceDropdownId: false,
        provinceNameHdnId: false,
        provinceCodeHdnId: false,
        ajaxUrl: false,
        validationClass: $empty,
        disabledOptionText: ' - - -'
    },
    countryEl: $empty,
    provinceEl: $empty,
    hdnNameEl: $empty,
    hdnCodeEl: $empty,
    initialize: function(options) {
        this.setOptions(options);
        this.countryEl = $(this.options.countriesDropdownId);
        this.provinceEl = $(this.options.provinceDropdownId);
        this.hdnNameEl = $(this.options.provinceNameHdnId);
        this.hdnCodeEl = $(this.options.provinceCodeHdnId);
        var countryChange = function(e) {
            var req = new Request.HTML({
                method: 'post',
                url: this.options.ajaxUrl,
                data: { country: this.countryEl.value },
                onRequest: function() { },
                onComplete: function(responseTree, responseElements, responseHtml) {
                    this.provinceEl.empty();
                    var Data = parseXMLDoc(responseHtml);
                    if (Data && Data.getElementsByTagName('ContentItem').length > 0) {

                        var ContentItemArray = Data.getElementsByTagName('ContentItem');
                        var ContentItemObject;
                        var KeyObject;
                        var ValueObject;

                        this.provinceEl.removeProperty('disabled');
                        for (var i = 0; i < ContentItemArray.length; i++) {
                            ContentItemObject = ContentItemArray[i];
                            KeyObject = ContentItemObject.getElementsByTagName('Key')[0].childNodes[0].nodeValue;
                            ValueObject = ContentItemObject.getElementsByTagName('ContentValue')[0].childNodes[0].nodeValue;
                            var opt = new Element('option', { value: KeyObject });
                            opt.set('text', ValueObject);
                            opt.inject(this.provinceEl);
                        }



                        if (Data.getElementsByTagName('ContentItem').length == 1) {
                            this.provinceEl.setProperty('disabled', 'disabled');
                            this.hdnNameEl.value = '';
                            this.hdnCodeEl.value = '';
                        }
                    }

                } .bind(this),
                onFailure: function() { }
            }).send();
        }
        var countryChangeHandler = countryChange.bindWithEvent(this);
        var stateChange = function(e) {
            var opt = this.provinceEl.getElements('option')[this.provinceEl.selectedIndex];
            var isEmpty = this.provinceEl.getElements('option').length == 1;
            this.hdnNameEl.value = isEmpty ? '' : opt.get('text');
            this.hdnCodeEl.value = isEmpty ? '' : this.provinceEl.value;
        }
        var stateChangeHandler = stateChange.bindWithEvent(this);
        this.countryEl.addEvent('change', countryChangeHandler);
        this.provinceEl.addEvent('change', stateChangeHandler);
    },
    validateProvince: function(doHilite) {
        var expression = this.provinceEl.getElements('option').length == 1 || this.provinceEl.selectedIndex > 0;
        if (doHilite) this.options.validationClass.highlightRow(!expression, this.provinceEl);
        return expression;
    },
    validateCountry: function(doHilite) {
        var expression = this.countryEl.selectedIndex != 0;
        if (doHilite) this.options.validationClass.highlightRow(!expression, this.countryEl);
        return expression;
    }
});