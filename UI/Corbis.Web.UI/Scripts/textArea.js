// Keep user from entering more than maxLength characters
function doKeypress(control, e) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    //debugger
    if (maxLength && value.length > maxLength - 1) {
        if (window.event) {
            e.returnValue = false;
        }
        else{ //firefox
            //backspace and delete, left, right, up, down
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                e.preventDefault();         
        }
        maxLength = parseInt(maxLength);
    }
}
// Cancel default behavior
function doBeforePaste(control, e) {
    maxLength = control.attributes["maxLength"].value;
    if (maxLength) {
        var getEvent = (window.event) ? event : control;
        getEvent.returnValue = false;

    }
}

var curTa;
function chop() {
    if (curTa) {
        var maxLength = curTa.attributes["maxLength"].value;
        maxLength = parseInt(maxLength);
        if (curTa.value.length > maxLength)
            curTa.value = curTa.value.substr(0, maxLength);
    }
}

// Cancel default behavior and create a new paste routine
function doPaste(control, e) {
    curTa = control;
    var maxLength = control.attributes["maxLength"].value;
    maxLength = parseInt(maxLength);
    if (maxLength) {
        if (window.event && !Browser.Engine.webkit) {            
            window.event.returnValue = false;
            var oTR = control.document.selection.createRange();
            var iInsertLength = maxLength - control.value.length;  //+ oTR.text.length;
            var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
            oTR.text = sData;
        }
        else {
            window.setTimeout('chop();', 10);      //firefox and safari , becasue clipboardData is not work for FF and safari      
        }       
    }
}
