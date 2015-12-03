//var myValidator = new fValidator('startDateBlock');
var _originalDate;
var newDate;
var pricedByAEDate;

var enableButtons = function(){
    var buttons = $('startDateButtons').getElements('span');
    
    buttons.each(function(btn){
        btn.setStyle('display', 'inline-block');
    });
}

var toggleEditMode = function(isEditMode, isSave) {
    var AEO = $('aeOutLine');
    var defaultItems = AEO.getElements('.startDateDefaultMode');
    var editItems = AEO.getElements('.startDateEditMode');
    var hideItems = $('startDateButtons').getElements('span');
    //editItems.merge($('startDateButtons').getElements('div'));

    if (isEditMode) {
        enableButtons();
        _originalDate = $('startDateMain').getElement('span').textContent;
        var calendarBehavior1 = $find("Calendar1");
        pricedByAEDate = calendarBehavior1._selectedDate;
    }


    defaultItems.each(function(item) {
        showItem(item, !isEditMode);
    });
    editItems.each(function(item) {
        showItem(item, isEditMode);
    });
    hideItems.each(function(item) {
        showItem(item, isEditMode);
    });
    return false;
}

var showItem = function(item, showMode){
    var mode = (showMode)? 'inline-block' : 'none';
    item.setStyle('display', mode);
    /*if (showMode)
    {
        item.setStyle('display', 'inline-block');
    }
    else
    {
        item.setStyle('display', 'none');
    }*/
}
var setWarningMode = function(warningMode) {
    if (warningMode) {
        $('errorBlock').setStyle('visibility', ''); ;
        $('startDateMain').addClass('WarningMode');
        $('priceByAETitle').setStyle('color', '#1A1A1A');
    }
    else {
        $('errorBlock').setStyle('visibility', 'hidden');
        $('startDateMain').removeClass('WarningMode');
        $('priceByAETitle').setStyle('color', '#e8e8e8');
    }

}

function checkStartDate(sender, args) {

    /*if (sender._selectedDate < new Date()) 
    {
        setWarningMode(true);
        //sender._selectedDate = new Date(); 
        // set the date back to the current date
        //sender._textbox.set_Value(sender._selectedDate.format(sender._format))
    }
    else
    {
        setWarningMode(false);
    }*/
    setWarningMode((sender._selectedDate < new Date().setHours(0, 0, 0, 0)));
    if (sender._selectedDate < new Date().setHours(0, 0, 0, 0)) {
        disableCartAndLightBoxButtons();
    } else {
        enableCartAndLightBoxButtons();
    }
}

function checkStartDateOnManualEdit() {
    //newDate = $('startDateMain').getElement('input').value;
    var calendarBehavior1 = $find("Calendar1");
    //calendarBehavior1._textbox.set_Value(newDate);

    if (calendarBehavior1._selectedDate < new Date().setHours(0, 0, 0, 0)) {
        disableCartAndLightBoxButtons();
    } else {
        enableCartAndLightBoxButtons();
    }
}





var updateResult = function(isSave, updateProduct) {

    if (isSave) {
        //newDate = $('startDateMain').getElement('input').value;
        var calendarBehavior1 = $find("Calendar1");
        //calendarBehavior1._textbox.set_Value(newDate);
        if (calendarBehavior1._selectedDate < new Date().setHours(0, 0, 0, 0)) {

            setWarningMode(true);
            //$('startDateMain').setStyle('color', '#333333');
            return false;
        }
        setWarningMode(false);

        if (updateProduct) {

            var cultureName = $(document.body).getElement('input[id$=hdnCultureName]').value;
            //now we update the data to the server
            PageMethods.changeStartDate(calendarBehavior1._selectedDate.localeFormat('d'), cultureName, changeStartDateCompleted);

        }
        else {
            changeStartDateCompleted();
        }
    }
    else {

        var calendarBehavior2 = $find("Calendar1");
        calendarBehavior2._selectedDate = pricedByAEDate;
        $('startDateMain').getElement('input').value = calendarBehavior2._selectedDate.localeFormat('d');
        checkStartDateOnManualEdit();

        setWarningMode(false);
        toggleEditMode(false, isSave);
        $('startDateMain').setStyle('color', 'white');
    }
} 

function changeStartDateCompleted(results, context, methodName) {
    var calendarBehavior1 = $find("Calendar1");
    pricedByAEDate = calendarBehavior1._selectedDate;
    //now update the content on the client
    $('startDateMain').getElement('span.startDateDefaultMode').set('text', calendarBehavior1._selectedDate.localeFormat('d'));
    $('licenseStartDateDetails').set('text', calendarBehavior1._selectedDate.localeFormat('d'));
    toggleEditMode(false, true);
    $('startDateMain').setStyle('color', 'white');
}