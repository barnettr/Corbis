function GeographySelectorAdd(el) {
    var leftGeoList = $(document.body).getElement('div.GeographyControlLeftList').getElement('select');
    var rightGeoList = $(document.body).getElement('div.GeographyControlRightList').getElement('select');
    var hiddenField = $(document.body).getElement('div.GeographyControlRightList').getElement('input')
    MoveItems(leftGeoList, rightGeoList, false);
    SetSelectedGeography(rightGeoList, hiddenField)
}

function GeographySelectorRemove(el) {
    var leftGeoList = $(document.body).getElement('div.GeographyControlLeftList').getElement('select');
    var rightGeoList = $(document.body).getElement('div.GeographyControlRightList').getElement('select');
    var hiddenField = $(document.body).getElement('div.GeographyControlRightList').getElement('input')
    MoveItems(rightGeoList, leftGeoList, true);
    SetSelectedGeography(rightGeoList, hiddenField);
}

function MoveItems(fromList, toList, removeSelected) {
    for (i = 0; i < fromList.options.length; i++) {
        // each one, check that it doesn't exist in target
        if (removeSelected) {
            if (fromList.options[i].selected) {
                fromList.options[i] = null;
                i--;
            }
        } else {
            if (fromList.options[i].selected) {
                var doNotAdd = false;
                if (fromList.options[i].value == '00000000-0000-0000-0000-000000000000') {
                    doNotAdd = true;
                }
                for (j = 0; j < toList.options.length; j++) {
                    if (fromList.options[i].value == toList.options[j].value) {
                        doNotAdd = true;
                        break;
                    }
                }
                if (!doNotAdd) {
                    toList.options[toList.options.length] = new Option(fromList.options[i].text, fromList.options[i].value);
                }
            }
        }
    }
}

function SetSelectedGeography(selectedList, hiddenField) {
    var selectedArray = new Array(selectedList.options.length);
    for (i = 0; i < selectedList.options.length; i++) {
        selectedArray[i] = selectedList.options[i].value;
    }

    hiddenField.value = selectedArray.join(',');
    if (hiddenField.onchange) {
        if (document.createEventObject) {
            // for IE
            hiddenField.onchange();
        }
        else {
            // for firefox + others
            var evt = document.createEvent("HTMLEvents");
            evt.initEvent('change', true, true);
            hiddenField.dispatchEvent(evt);
        }
    }
}
