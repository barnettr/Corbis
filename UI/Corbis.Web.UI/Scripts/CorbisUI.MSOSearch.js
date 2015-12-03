/****************************************************
    Corbis UI MSO Search
    (More Search Options)
***************************************************/

CorbisUI.MSOSearch = {
    vars: {
        imageBase: null,
        imageNumberText: null,
        searchBuddyBase: null,
        noPeopleCheckBox: null,
        numberOfPeopleSelect: null,
        numberOfPeopleValue: null,
        dateContainer: null,
        betweenBtn: null,
        startDate: null,
        endDate: null,
        radioBetweenBase: null,
        radioBetweenButton: null,
        radioDaysBase: null,
        radioDaysTextBox: null
    },

    initialize: function() {
        // No bColumn, MSO is not enabled
        if ($('bColumn')) {
            this.vars.imageBase = $('bColumn');
            this.vars.imageNumberText = this.vars.imageBase.getElement('.imageNumbers');
            this.vars.numberOfPeopleSelect = this.vars.imageBase.getElement('div.numberOfPeople').getElement('select');

            this.vars.searchBuddyBase = $('SearchBuddy');
            if (this.vars.searchBuddyBase != null) {
                if (this.vars.searchBuddyBase.getElement('.NoPeople') != null) {
                    this.vars.noPeopleCheckBox = this.vars.searchBuddyBase.getElement('.NoPeople').getElement('.imageCheckbox');
                }
            }

            this.vars.dateContainer = $('betweenDataContainer');
            this.vars.betweenBtn = this.vars.dateContainer.getElement('input');
            this.vars.startDate = this.vars.dateContainer.getElement('.beginDate').value;
            this.vars.endDate = this.vars.dateContainer.getElement('.endDate').value;
            this.vars.radioBetweenBase = $('radioBetweenDiv');
            this.vars.radioBetweenButton = this.vars.radioBetweenBase.getElement('input[type=radio]'); ;
            this.vars.radioDaysBase = $('radioDaysDiv');
            this.vars.radioDaysTextBox = this.vars.radioDaysBase.getNext('input[type=text]');
        }
    },

    clearText: function() {
        this.initialize();
        if (this.vars.imageNumberText) {
            if (this.vars.imageNumberText.value == CorbisUI.ExtendedSearch.vars.ImageNumbersText) {
                this.vars.imageNumberText.value = "";
            }
        }
    },

    resetText: function() {
        this.initialize();
        if (this.vars.imageNumberText) {
            if (this.vars.imageNumberText.value == "") {
                this.vars.imageNumberText.value = CorbisUI.ExtendedSearch.vars.ImageNumbersText;
            }
        }
    },

    // Called when a selection is made in the MSO Number Of People filter.
    // If the No People checkbox is checked, then uncheck it
    verifyNoPeopleChecked: function() {
        this.initialize();

        this.vars.numberOfPeopleValue = this.vars.numberOfPeopleSelect.value;
        if (this.vars.noPeopleCheckBox != null) {
            setCheckedState(this.vars.noPeopleCheckBox, false);
        }
    },

    // Called to initialize the MSO Number Of People filter when it loads the first time
    setNumberOfPeopleControlValue: function() {
        this.initialize();
        if (this.vars.numberOfPeopleSelect != null && this.vars.numberOfPeopleValue != null) {
            if (this.vars.numberOfPeopleValue == 5 || this.vars.numberOfPeopleValue == "5" ) {
                this.vars.numberOfPeopleSelect[0].selected = true;
            } else {
                this.vars.numberOfPeopleSelect[this.vars.numberOfPeopleValue].selected = true;
            }
        }
    },

    // Called when the No People filter is checked. Sets the MSO Number
    // of People filter to 'Any'.
    verifyNumberOfPeopleChecked: function() {
        this.initialize();

        // Set global var to the 'Any' value 
        this.vars.numberOfPeopleValue = "5";

        if (this.vars.noPeopleCheckBox) {
            if (getCheckedState(this.vars.noPeopleCheckBox)) {
                if (this.vars.numberOfPeopleSelect.selectedIndex > 0) {
                    this.vars.numberOfPeopleSelect[0].selected = true;
                }
            }
        }
    },

    isDate: function(dateString) {
        try {
            var dateParts = CorbisUI.ExtendedSearch.vars.DateTimeFormat.split(CorbisUI.ExtendedSearch.vars.DateSeparator);
            var dateValues = dateString.split(CorbisUI.ExtendedSearch.vars.DateSeparator);
            var mm, dd, yyyy;
            for (var i = 0; i < dateParts.length; i++) {
                if (dateParts[i].indexOf('M') > -1) {
                    mm = dateValues[i];
                }
                if (dateParts[i].indexOf('y') > -1) {
                    yyyy = dateValues[i];
                }
                if (dateParts[i].indexOf('d') > -1) {
                    dd = dateValues[i];
                }
            }
            var d = new Date(mm + "/" + dd + "/" + yyyy);
            return d.getMonth() + 1 == mm && d.getDate() == dd && d.getFullYear() == yyyy;
        } catch (ex) {
            return false;
        }
    },

    getLocalDate: function(dateString) {
        var dateParts = CorbisUI.ExtendedSearch.vars.DateTimeFormat.split(CorbisUI.ExtendedSearch.vars.DateSeparator);
        var dateValues = dateString.split(CorbisUI.ExtendedSearch.vars.DateSeparator);
        var mm, dd, yyyy;
        for (var i = 0; i < dateParts.length; i++) {
            if (dateParts[i].indexOf('M') > -1) {
                mm = dateValues[i];
            }
            if (dateParts[i].indexOf('y') > -1) {
                yyyy = dateValues[i];
            }
            if (dateParts[i].indexOf('d') > -1) {
                dd = dateValues[i];
            }
        }
        var d = new Date(mm + "/" + dd + "/" + yyyy);
        return d;
    },

    checkDateValues: function() {
        this.initialize();
        if (this.vars.betweenBtn) {
            if (this.vars.betweenBtn.checked) {
                if (!this.isDate(this.vars.startDate) || !this.isDate(this.vars.endDate)) {
                    alert(this.vars.notDateErr);
                    return false;
                }
                else if (this.isDate(this.vars.startDate) && this.isDate(this.vars.endDate)) {
                    var date1 = this.getLocalDate(this.vars.startDate);
                    var date2 = this.getLocalDate(this.vars.endDate);
                    if (Date.parse(date1) > Date.parse(date2)) {
                        alert(this.vars.AlertDateText);
                        return false;
                    } else {
                        CorbisUI.ExtendedSearch.vars.noSearchMsg = false;
                    }
                } else {
                    alert(this.vars.AlertDateText);
                    return false;
                }
            }
        }
        return true;
    },

    clearRadioDaysText: function() {
        this.initialize();
        if (this.vars.radioBetweenButton.checked) {
            this.vars.radioDaysTextBox.value = '';
        }
    }

}
