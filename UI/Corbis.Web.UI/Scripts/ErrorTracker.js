// this is a lite version of the validation.js
// do not use both scripts on a modal. They will collide
// this is for custom error tracking.

CorbisUI.FormUtilities.ErrorTracker = new Class({

    Implements: [Options],

    options: {
        container: false,
        hiliteErrors: true,
        customResize: false
    },
    errorDiv: false,
    errorTarget: false,

    initialize: function(options) {
        if (options)
            this.setOptions(options);
        this.errorDiv = $(this.options.container).getElement('div.ValidationSummary');
        this.errorTarget = this.errorDiv.getElement('ul');
    },
    addError: function(elementId, message, noHilite) {
        if (this.errorTarget.getElement(String.format('li[elementId={0}]', elementId)))
            return;
        var errorItem = String.format('<li elementId={0}>{1}</li>', elementId, message);
        this.errorTarget.set('html', this.errorTarget.get('html') + errorItem);
        if (!noHilite) {
            this.highlightRow(true, $(elementId));
        }
        this.resizeContainer();
    },
    removeError: function(elementId) {
        try {
            this.errorTarget.getElement('li[elementId=' + elementId + ']').dispose();
            this.highlightRow(false, $(elementId));
            this.resizeContainer();
        } catch (Error) { }
    },
    reset: function() {
        this.errorTarget.getElements('li').each(function(li) {
            this.highlightRow(false, $(li.getProperty('elementId')));
            li.dispose();
        }, this);
        this.resizeContainer();
    },
    resizeContainer: function() {
        if (this.errorTarget.getElements('li').length == 0)
            this.errorDiv.addClass('displayNone');
        else
            this.errorDiv.removeClass('displayNone');
        if (this.options.customResize)
            eval(this.options.customResize);
        else
            ResizeModal(this.options.container);
    },
    highlightRow: function(hilite, element) {
        var parentRow;
        var pattern = new Array('tr.FormRow', 'tr.Error', 'div.FormRow', 'div.Error');
        for (var i = 0; i < pattern.length; i++)
            if (element.getParent(pattern[i]))
            parentRow = element.getParent(pattern[i]);
        if (hilite)
            parentRow.addClass('ErrorHighlight');
        else
            parentRow.removeClass('ErrorHighlight');

    }
});
