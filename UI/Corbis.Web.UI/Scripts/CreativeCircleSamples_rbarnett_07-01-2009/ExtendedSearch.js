if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}

CorbisUI.ExtendedSearch = {

    vars: {
        SearchUri: null,
        HiddenMSOTrigger: null,
        HiddenMSOValue: null,
        KeywordSearch: null,
        ImageNumbers: null,
        SearchImages: null,
        EmptySearchStringAlert: null,
        ImageNumbersText: null,
        DaysButton: null,
        BetweenButton: null,
        DaysText: null,
        MSO: null,
        MSOStartDateExtend: null,
        MSOEndDateExtend: null,
        noLicense: false,
        noCat: false,
        SearchClientId: null,
        DeleteMSO: null,
        DateCreated: null,
        Location: null,
        Photographer: null,
        Provider: null,
        Horizontal: null,
        Vertical: null,
        Panoramic: null,
        PointOfView: null,
        NumberOfPeople: null,
        ImmediateAvailablility: null,
        StartDate: null,
        EndDate: null,
        DefaultStartDate: null,
        DefaultEndDate: null,
        _isResultsPage: location.href.toLowerCase().indexOf('searchresults.aspx') != -1,
        _anyEditorialChecked: false,
        _isSearching: false,
        _isSearchFlyoutMousedown: false,
        _msoStartDate: null,
        _msoEndDate: new Date(),
        _isAddLightBox: null,
        _isOptionsAppliedWindowMousedown: false,
        _MSOErrorString: null,
        MSODeleted: false,
        noSearchMsg: true,
        currentPage: null,
        totalPages: null,
        checkBoxIDS: {
            creative: null,
            editorial: null,
            documentary: null,
            fineArt: null,
            archival: null,
            currentEvents: null,
            entertainment: null,
            outline: null,
            noPeople: null,
            rmLicense: null,
            rfLicense: null,
            photography: null,
            illustration: null,
            color: null,
            blackWhite: null,
            onlyModelReleased: null,
            IsManualSearch: true,
            LastMSOFilters: null
        }
    },

    flyoutKeyPress: function() {
        $('searchFlyout').onkeypress = function(e) {
            if (!e) var e = window.event;
            e = e || window.event;
            var code = e.keyCode || e.which;
            if (code == 13) {
                CorbisUI.ExtendedSearch.combineSearchBuddy();
                e.returnValue = false;
                return false;
            }
        }
    },

    getSearchTipsWindow: function() {
        tipsWindow = window.open("/Creative/SearchTips/Page.aspx", "tipsWindow", "left=0,top=0,width=1009,height=633,toolbar=0,resizable=0,scrollbars=0,menubar=0");
        tipsWindow.moveTo(0, 0);
    },


    testCategoriesChecked: function(isFlyoutInvoked) {
        if ($('searchFlyout').getStyle('display') == 'none' && !CorbisUI.ExtendedSearch.vars._isResultsPage) {
            return true;
        }

        CorbisUI.ExtendedSearch.vars.noLicense = !getCheckedState(this.vars.checkBoxIDS.rmLicense) && !getCheckedState(this.vars.checkBoxIDS.rfLicense);
        CorbisUI.ExtendedSearch.vars.noCat = !getCheckedState(this.vars.checkBoxIDS.editorial) && !getCheckedState(this.vars.checkBoxIDS.creative);

        var noResults = CorbisUI.ExtendedSearch.vars.noLicense || CorbisUI.ExtendedSearch.vars.noCat;

        if (noResults && !isFlyoutInvoked) {
            if (!$('noSearchResultsWarningWindow')) {
                new CorbisUI.Popup('noSearchResultsWarning', {
                    createFromHTML: true,
                    showModalBackground: false,
                    centerOverElement: 'noSearchResultsWarning',
                    closeOnLoseFocus: true,
                    positionVert: 180,
                    positionHoriz: 212,
                    replaceText: ['']
                });

                setTimeout('CorbisUI.ExtendedSearch.setNoResultsOptions()', 100);
            }


            return false;
        }
        else if (!noResults) {
            MochaUI.CloseModal('noSearchResultsWarning');
            this.flyoutKeyPress();
            return true;
        }
        else return !noResults;
    },

    MoveToEnd: function() {
        var keywordSearchTextbox = CorbisUI.ExtendedSearch.vars.KeywordSearch;
        var keywordSearchValue = keywordSearchTextbox.getProperty('value');

        if (navigator.userAgent.indexOf('MSIE') != -1) {
            keywordSearchTextbox.focus();
            keywordSearchTextbox.select();
            document.selection.clear();
            keywordSearchTextbox.setProperty('value', keywordSearchValue);
        }
        else {
            var keywordSearchValue = keywordSearchTextbox.getProperty('value');
            keywordSearchTextbox.setProperty('value', '');
            keywordSearchTextbox.setProperty('value', keywordSearchValue);
        }
        checkOptionsApplied();
    },

    ShowSearchProgIndicator: function() {
        if ($type($('searchProgIndicator')) == 'element') {
            if (Browser.Engine.trident) {
                setTimeout("$('searchProgIndicator').setStyle('display', 'block');", 0);
            } else {
                $('searchProgIndicator').setStyle('display', 'block');
            }
        }
    },

    HideSearchProgIndicator: function() {
        if ($type($('searchProgIndicator')) == 'element') {
            $('searchProgIndicator').setStyle('display', 'none');
        }
    },

    setEditorialCheckedState_flyout: function(checked) {
        var editorialCB = this.vars.checkBoxIDS.editorial;
        var allUnchecked = true;
        var checks = $('EditorialChildrenDiv').getElements('div.imageCheckbox');

        checks.each(function(cb) {
            if (getCheckedState(cb)) {
                allUnchecked = false;
            }
        });
        if (allUnchecked)
            setCheckedNoEvent(editorialCB, false);
        else
            setCheckedNoEvent(editorialCB, true);

        CorbisUI.ExtendedSearch.vars._anyEditorialChecked = !allUnchecked;
    },

    OpenMoreSearchOptions: function() {
        if (CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value == "UnOpened") {
            CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value = "Opening";
            CorbisUI.ExtendedSearch.vars.HiddenMSOTrigger.click();
        }
        if (CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value == "Reset") {
            CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value = "Opening";
            CorbisUI.ExtendedSearch.vars.HiddenMSOTrigger.click();
        }
        var noFlyout = false;
        if ($('moreSearchOptionsWindow')) {
            if ($('moreSearchOptionsWindow').getStyle('display') == 'none') {
                $('moreSearchOptionsWindow').setStyle('display', 'block');
            }
            else {
                CorbisUI.ExtendedSearch.hideMSOWindow(true);
                CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown = false;
                CorbisUI.ExtendedSearch.hideSearchFlyout();
                noFlyout = true;
            }
        }
        else {
            var el = $('moreSearchOptions');
            el.setStyle('display', 'block');
            var elDimensions = el.getCoordinates();
            var properties = {
                title: '',
                collapsible: false,
                minimizable: false,
                contentBgColor: '#363636',
                headerStartColor: [74, 74, 74],
                headerStopColor: [65, 65, 65],
                bodyBgColor: [54, 54, 54],
                useCanvasControls: false,
                cornerRadius: 4,
                headerHeight: 14,
                footerHeight: 4,
                padding: 0,
                scrollbars: false,
                closable: false,
                type: 'window',
                id: el.getProperty('id') + "Window",
                height: elDimensions.height,
                width: elDimensions.width,
                //		        x: elDimensions.left,
                //		        y: elDimensions.top,
                x: 220,
                y: 180,
                content: '',
                draggable: false,
                resizable: false
            };
            MochaUI.NewWindowFromDiv(el, properties);
            CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown = false;
            $(document.body).addEvent('mousedown', function() {
                setTimeout("CorbisUI.ExtendedSearch.hideMSOWindow(true);", 10);
            });
        }
        if (CorbisUI.ExtendedSearch.vars._isResultsPage && !noFlyout) {
            CorbisUI.ExtendedSearch.ShowSearchProgIndicator();
        }
        else if (!noFlyout) {
            CorbisUI.ExtendedSearch.showSearchFlyout();
        }
        return false;
    },

    restoreDefaultMSO: function() {
        var control = $('moreSearchOptions').getElements('input');
        control.each(function(el) {
            // Restore checkboxes to checked
            if (el.type == "checkbox") {
                if (!el.checked) {
                    toggleCheckedState(el.getParent().getParent().id);
                }
            }
            // Clear out text inputs
            if (el.type == "text") {
                // DON'T DO ANYTHING TO HIDDEN MSO VALUE
                if (!el.id.contains('hiddenMSOValue')) el.value = '';
            }
        }, this);

        radioClicked($(CorbisUI.ExtendedSearch.vars.DaysButton).getParent().getParent().id);

        $(this.vars.StartDate).value = this.vars.DefaultStartDate;
        $(this.vars.EndDate).value = this.vars.DefaultEndDate;

        // Clear select boxes - set index to 0
        control = $('moreSearchOptions').getElements('select');
        control.each(function(el) {
            el.selectedIndex = 0;
        }, this);

        // Set textarea to default text
        control = $('moreSearchOptions').getElements('textarea');
        control.each(function(el) {
            el.value = CorbisUI.ExtendedSearch.vars.ImageNumbersText;
        }, this);

        $('moreSearchOptions').getElements('div.MSO_toggler').each(function(el) {
            var elementId = el.id.toString();
            CorbisUI.ExtendedSearch.vars.MSO.updateStatCounts(elementId);
        });
        this.vars.MSODeleted = true;
    },

    cancelMSOChanges: function() {
        if (this.vars.MSODeleted) {
            this.restoreDefaultMSO();
        }
        else {
            var control = $('moreSearchOptions').getElements('input');
            control.each(function(el) {
                if (el.type == "checkbox") {
                    if (el.checked != el.defaultChecked) {
                        toggleCheckedState(el.getParent().getParent().id);
                    }
                }
                else if (el.type == "radio") {
                    if (el.defaultChecked) {
                        radioClicked(el.getParent().getParent().id);
                    }
                }
                else {
                    el.value = el.defaultValue;
                }
            }, this);

            control = $('moreSearchOptions').getElements('select');
            control.each(function(el) {
                el.selectedIndex = 0;
            }, this);

            control = $('moreSearchOptions').getElements('textarea');
            control.each(function(el) {
                el.value = el.defaultValue;
            }, this);

            $('moreSearchOptions').getElements('div.MSO_toggler').each(function(el) {
                var elementId = el.id.toString();
                CorbisUI.ExtendedSearch.vars.MSO.updateStatCounts(elementId);
            });
        }
    },


    deleteSearchOptions: function() {
        var keywordSearchTextbox = CorbisUI.ExtendedSearch.vars.KeywordSearch;

        if (this.vars.HiddenMSOValue.value == "UnOpened") {
            this.vars.HiddenMSOValue.value = "Reset";
        }
        else {
            // try to clear fields
            this.restoreDefaultMSO();
        }
        $('optionsAppliedClose').setStyle('display', 'none');
        $("Keywords").removeClass("optionsApplied");
        $("optionsAppliedDiv").removeClass("optionsAppliedDiv").setStyle("display", "none");
        keywordSearchTextbox.setStyle('background-color', '#ffffff');

        if ($('optionsAppliedClose').getStyle('display') == 'none') {
            keywordSearchTextbox.removeClass("optionsAppliedInput");
        }

        var req = new Request({
            method: 'post',
            url: "/search/searchscriptservice.asmx/DeleteMoreSearchOptions"
        }).send();
    },

    hideMSOWindow: function(force) {
        if (CorbisUI.ExtendedSearch.vars._isSearching) {
            return;
        }
        if (force || (!CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown && $('moreSearchOptionsWindow') != null)) {
            try {
                $('moreSearchOptionsWindow').setStyle('display', 'none');
            }
            catch (er) { }
        }
        CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown = false;
        if (CorbisUI.ExtendedSearch.vars._isResultsPage && !CorbisUI.ExtendedSearch.vars._isSearching) {
            CorbisUI.ExtendedSearch.HideSearchProgIndicator();
            //$('searchProgContents').setStyle('display', 'block');
        }
    },

    showSearchFlyout: function() {
        if (CorbisUI.ExtendedSearch.vars._isResultsPage) {
            return;
        }
        if ($('searchFlyoutWindow')) {
            $('searchFlyoutWindow').setStyle('display', 'block');
        }
        else {
            var el = $('searchFlyout');
            el.setStyle('display', 'block');
            var elDimensions = el.getCoordinates();
            var properties = {
                title: '',
                indexLevel: 11111,
                collapsible: false,
                minimizable: false,
                contentBgColor: [102, 102, 102],
                headerStartColor: [117, 117, 117],
                headerStopColor: [110, 110, 110],
                bodyBgColor: [102, 102, 102],
                useCanvasControls: false,
                cornerRadius: 4,
                headerHeight: 15,
                footerHeight: 4,
                padding: 10,
                scrollbars: false,
                closable: true,
                type: 'window',
                id: el.getProperty('id') + "Window",
                height: elDimensions.height,
                width: elDimensions.width,
                x: elDimensions.left,
                y: elDimensions.top,
                content: '',
                draggable: false,
                resizable: false
            };

            el.setStyle("left", "0");
            el.setStyle("top", "0");
            MochaUI.NewWindowFromDiv(el, properties);
            $(document.body).addEvent('mousedown', function() {
                setTimeout('CorbisUI.ExtendedSearch.hideSearchFlyout()', 10);
            });
            $('searchFlyoutWindow_titleBar').setStyle('background', 'transparent');
            $('searchFlyoutWindow_contentWrapper').setStyle('background', 'transparent');
            CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown = false;
        }
    },

    hideSearchFlyout: function() {
        if (CorbisUI.ExtendedSearch.vars._isResultsPage) return;
        if (!CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown)
            try { $('searchFlyoutWindow').setStyle('display', 'none'); } catch (er) { }
        CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown = false;
        MochaUI.CloseModal('noSearchResultsWarning');
    },


    EditorialChanged_flyout: function(checked) {
        var checks = $('EditorialChildrenDiv').getElements('div.imageCheckbox');
        checks.each(function(cb) {
            setCheckedNoEvent(cb, checked)
        });
        CorbisUI.ExtendedSearch.vars._anyEditorialChecked = checked;
    },

    setNoResultsOptions: function() {

        if (CorbisUI.ExtendedSearch.vars._isResultsPage) return;
        var pop = $('noSearchResultsWarningWindow');
        pop.getElement('p[textKey=License]').setStyle('display', 'none');
        pop.getElement('p[textKey=Category]').setStyle('display', 'none');
        //alert(pop.getElement('p[textKey=License]'));
        if (CorbisUI.ExtendedSearch.vars.noLicense)
            pop.getElement('p[textKey=License]').setStyle('display', 'block');
        else if (CorbisUI.ExtendedSearch.vars.noCat)
            pop.getElement('p[textKey=Category]').setStyle('display', 'block');

        $('searchFlyoutWindow').setStyle('z-index', '99999');
        $('searchFlyoutWindow').addEvent('mousedown', function() {
            $('searchFlyoutWindow').setStyle('z-index', '99999');
        });
        $('modalOverlay').addEvent('click', function() {
            $('modalOverlay').setStyle('display', 'none');
        });
        if (!$('searchFlyoutWindow') || $('searchFlyoutWindow').getStyle('display', 'none'))
            CorbisUI.ExtendedSearch.showSearchFlyout();
    },

    IMA_disabler: function(e, ele) {
        // get the parent wrapper
        var parent = this.getParent('.roundMe');
        // fire the click event
        parent.getElement('input').setProperty('checked', 'true');
    },


    // Calendar controls on MSO pane

    checkStartDate: function(sender, args) {
        CorbisUI.ExtendedSearch.vars._msoStartDate = sender._selectedDate;
        var dt = new Date();
        if (sender._selectedDate > dt) {
            sender._textbox.set_Value(dt.format(sender._format));
            CorbisUI.ExtendedSearch.vars._msoStartDate = dt;
        }
        if (CorbisUI.ExtendedSearch.vars._msoStartDate > CorbisUI.ExtendedSearch.vars._msoEndDate) {
            sender._textbox.set_Value(CorbisUI.ExtendedSearch.vars._msoEndDate.format(sender._format));
            CorbisUI.ExtendedSearch.vars._msoStartDate = CorbisUI.ExtendedSearch.vars._msoEndDate;
        }
    },

    checkEndDate: function(sender, args) {
        CorbisUI.ExtendedSearch.vars._msoEndDate = sender._selectedDate;
        var dt = new Date();
        if (sender._selectedDate > dt) {
            sender._textbox.set_Value(dt.format(CorbisUI.ExtendedSearch.vars.DateTimeFormat));
        }
        if (CorbisUI.ExtendedSearch.vars._msoStartDate > CorbisUI.ExtendedSearch.vars._msoEndDate) {
            sender._textbox.set_Value(CorbisUI.ExtendedSearch.vars._msoStartDate.format(CorbisUI.ExtendedSearch.vars.DateTimeFormat));
            CorbisUI.ExtendedSearch.vars._msoEndDate = CorbisUI.ExtendedSearch.vars._msoStartDate;
        }
    },

    // *******************************
    // Advanced Search Option
    // *******************************
    OpenOptionsAppliedModal: function() {
        if ($('optionsAppliedModalWindow')) {
            $('optionsAppliedModalWindow').setStyle('display', 'block');
        } else {
            var el = $('optionsAppliedModal');
            el.setStyle('display', 'block');
            var elDimensions = el.getCoordinates();
            var properties = {
                title: '',
                collapsible: false,
                minimizable: false,
                contentBgColor: '#e8e8e8',
                headerStartColor: [219, 219, 219],
                headerStopColor: [219, 219, 219],
                bodyBgColor: [232, 232, 232],
                useCanvasControls: false,
                cornerRadius: 4,
                headerHeight: 42,
                footerHeight: 4,
                padding: 0,
                shadowBlur: 9,
                scrollbars: false,
                closable: false,
                type: 'window',
                id: el.getProperty('id') + "Window",
                height: elDimensions.height,
                width: elDimensions.width,
                //		        x: elDimensions.left,
                //		        y: elDimensions.top,
                x: 340,
                y: 180,
                content: '',
                draggable: false,
                resizable: false
            };
            MochaUI.NewWindowFromDiv(el, properties);

            $(document.body).addEvent('mousedown', CorbisUI.ExtendedSearch.detectOptionsAppliedModalClick.bindWithEvent($('optionsAppliedModalWindow')));
        }
    },

    hideAppliedOptionsWindow: function(e) {
        if ($('optionsAppliedModalWindow')) {
            $('optionsAppliedModalWindow').setStyle('display', 'none');

            CorbisUI.ExtendedSearch.cancelMSOChanges();
            e.cancelBubble = true;
        } else {
            var el = $('optionsAppliedModal');
            el.setStyle('display', 'none');
            CorbisUI.ExtendedSearch.hideMSOWindow(true);
        }
    },

    detectOptionsAppliedModalClick: function(ev) {

        var coord = this.getCoordinates();
        if (
            ((ev.page.y < coord.top) || (ev.page.y > (coord.top + coord.height)))
            ||
            (ev.page.x < coord.left) || (ev.page.x > (coord.left + coord.width))
        ) {
            (function() { CorbisUI.ExtendedSearch.hideTimerWindow() }).delay(10);
        }
    },

    hideTimerWindow: function() {
        if (!CorbisUI.ExtendedSearch.vars._isOptionsAppliedWindowMousedown)
            try { $('optionsAppliedModalWindow').setStyle('display', 'none'); } catch (e) { }
        CorbisUI.ExtendedSearch.vars._isOptionsAppliedWindowMousedown = false;
    },

    setupMSO: function() {
        // startup accordion for more search options
        CorbisUI.ExtendedSearch.vars.MSO = new MSOaccordion();

        // get the more search options wrap
        var MSOwrap = $('moreSearchOptions');

        // get input elements
        var IMA_last_items = MSOwrap.getElement('div.IMA_lastDays').getElements('input[type=text]');
        var IMA_between_items = MSOwrap.getElement('div.IMA_between').getElements('input[type=text]');

        // combine the element arrays
        IMA_between_items.combine(IMA_last_items);

        // loopty do
        IMA_between_items.each(function(item) {
            item.addEvent('focus', CorbisUI.ExtendedSearch.IMA_disabler.bindWithEvent(item))
        });

        $('MSOIndicator').setStyle('display', 'none');

        MSOwrap.onkeypress = function(e) {
            if (!e) var e = window.event;
            e = e || window.event;
            var target = window.event ? window.event.srcElement : e ? e.target : null;
            var code = e.keyCode || e.which;
            if (target.id != CorbisUI.ExtendedSearch.vars.ImageNumbers) {
                if (code == 13 && $('moreSearchOptionsWindow') && $('moreSearchOptionsWindow').getStyle('display') == 'block') {
                    CorbisUI.ExtendedSearch.validateSearch();
                    e.returnValue = false;
                    return false;
                }
            } else {
                e.returnValue = true;
                return true;
            }
        }
    },

    invokeSearch: function(isFromFilters, resetOptions) {

        LogOmnitureEvent("event5");
        CorbisUI.ExtendedSearch.hideMSOWindow(true);
        CorbisUI.ExtendedSearch.hideSearchFlyout();
        try {
            var pop = $('noSearchResultsWarningWindow');
            if (pop && pop.getStyle('display') == 'block') {
                if (CorbisUI.ExtendedSearch.vars._isResultsPage)
                    $('searchBuddyMask').setStyle('display', 'none');
                return false;
            }
        } catch (er) { }
        if (isFromFilters) {
            $('processingFilters').setStyle('display', 'block');
        }
        CorbisUI.ExtendedSearch.ShowSearchProgIndicator();
        CorbisUI.ExtendedSearch.vars._isSearching = true;
        this.executeSearchQuery();
    },

    executeSearchQuery: function() {
        var query = "";
        if ($(this.vars.KeywordSearch).value != this.vars.SearchImages && $(this.vars.KeywordSearch).value != "")
            query += "&q=" + UrlEncode($(this.vars.KeywordSearch).value);

        if ($("moreSearchOptionsWindow")) {

            if ($(this.vars.DateCreated).value)
                query += "&dr=" + UrlEncode($(this.vars.DateCreated).value.trim());

            if ($(this.vars.DaysButton).checked && $(this.vars.DaysText).value.trim())
                query += "&ma=" + UrlEncode($(this.vars.DaysText).value.trim());
            else if ($(this.vars.BetweenButton).checked)
                query += "&bd=" + UrlEncode($(this.vars.StartDate).value.trim()) + "," + UrlEncode($(this.vars.EndDate).value.trim());

            if ($(this.vars.Location).value)
                query += "&lc=" + UrlEncode($(this.vars.Location).value.trim());

            if ($(this.vars.Photographer).value)
                query += "&pg=" + UrlEncode($(this.vars.Photographer).value.trim());

            if ($(this.vars.Provider).value)
                query += "&pr=" + UrlEncode($(this.vars.Provider).value.trim());

            if ($(this.vars.ImageNumbers).value && $(this.vars.ImageNumbers).value != this.vars.ImageNumbersText)
                query += "&in=" + UrlEncode($(this.vars.ImageNumbers).value.trim());

            query += this.getOrientationQuery();

            if ($(this.vars.PointOfView).value && $(this.vars.PointOfView).value != "0")
                query += "&pv=" + $(this.vars.PointOfView).value;

            if ($(this.vars.NumberOfPeople).value && $(this.vars.NumberOfPeople).value != "5")
                query += "&np=" + $(this.vars.NumberOfPeople).value;

            if ($(this.vars.ImmediateAvailablility).value && $(this.vars.ImmediateAvailablility).value != "1")
                query += "&ia=" + $(this.vars.ImmediateAvailablility).value;

            query += this.getMarketingCollectionQuery();
        }

        query += this.getCategoryQuery();
        query += this.getLicenseQuery();
        query += this.getNoPeopleQuery();
        query += this.getMediaTypeQuery();
        query += this.getColorFormatQuery();
        query += this.getModelReleaseQuery();

        if (!$("moreSearchOptionsWindow")) {
            query += "&" + CorbisUI.ExtendedSearch.vars.LastMSOFilters;
        }

        var clarificationQuery = CorbisUI.ExtendedSearch.getClarifactionChecked();
        if (clarificationQuery && clarificationQuery != ',') {
            query += "&cl=" + clarificationQuery;
        }
        CorbisUI.ExtendedSearch.SetSearchCookie(UrlEncode(query.substring(1)));

        if (query != "") {
            location.href = this.vars.SearchUri + '?' + query.substr(1);
        }
        return false;
    },


    SetSearchCookie: function(value) {
        try {
            var name = "UserSearchOptions";
            var myURI = (location.hostname).toString();
            var domain = '';
            domain = (myURI.contains('corbis.com')) ? ".corbis.com" : ".corbis.pre";
            // this next line is for corbisbeta only
            if (myURI.contains('corbisbeta.com')) domain = ".corbisbeta.com";
            Cookie.write(name, value, {
                path: "/",
                domain: domain,
                secure: false
            });
        } catch (e) { }
    },

    getClarifactionChecked: function(method) {

        var base = $('ambiguousModal');
        if (base) {
            var clarificationGroups = base.getElements('.Clarification');
            var clarificationString = '';

            clarificationGroups.each(function(el) {
                clarificationString += ',';

                var isOneClarificationSelected = false;

                var checkboxWrap = el.getElements('.checkboxWrap');
                for (i = 0; i < checkboxWrap.length; i++) {
                    var cbx = checkboxWrap[i].getElement('input[type=checkbox]');
                    if (cbx.checked) {
                        isOneClarificationSelected = true;
                        break;
                    }
                }

                if (isOneClarificationSelected) {
                    el.getElements('.checkboxWrap').each(function(itemEl) {
                        var cbx = itemEl.getElement('input[type=checkbox]');
                        (cbx.checked) ? cbx.value = 1 : cbx.value = 0;
                        clarificationString += cbx.value;
                    });
                }
            });

            clarificationString = clarificationString.substring(1);
            if (clarificationString.endsWith(",")) {
                clarificationString = clarificationString.substring(0, clarificationString.length - 1);
            }
            var queryFlags = $(CorbisUI.GlobalVars.SearchResults.clarificationQueryFlags);
            queryFlags.value = clarificationString;
            if (method == "cancel") {
                HideModal('ambiguousModal');
            }
            else {
                MochaUI.CloseModal('ambiguousModal');
                return clarificationString;
            }
        }
    },

    getOrientationQuery: function() {
        var horz = getCheckedState(this.vars.Horizontal);
        var vert = getCheckedState(this.vars.Vertical);
        var pano = getCheckedState(this.vars.Panoramic);
        var str = "";
        if (!(horz & vert & pano) && !(!horz & !vert & !pano)) {
            str += "&or=";
            var sep = "";
            if (vert) { str += sep + "1"; sep = ","; }
            if (horz) { str += sep + "2"; sep = ","; }
            if (pano) { str += sep + "3"; sep = ","; }
        }
        return str;
    },

    getCategoryQuery: function() {
        var cats = new Array();
        var cbControls = this.vars.checkBoxIDS;
        var isOutline = $(cbControls.outline) != null;
        var query = "";

        if (this.vars._isResultsPage)
            cbControls = CorbisUI.GlobalVars.SearchResults.checkBoxIDS;

        if (getCheckedState(cbControls.creative))
            cats.push(getCheckboxValue(cbControls.creative));

        if (getCheckedState(cbControls.documentary))
            cats.push(getCheckboxValue(cbControls.documentary));

        if (getCheckedState(cbControls.fineArt))
            cats.push(getCheckboxValue(cbControls.fineArt));

        if (getCheckedState(cbControls.archival))
            cats.push(getCheckboxValue(cbControls.archival));

        if (getCheckedState(cbControls.currentEvents))
            cats.push(getCheckboxValue(cbControls.currentEvents));

        if (getCheckedState(cbControls.entertainment))
            cats.push(getCheckboxValue(cbControls.entertainment));

        if (isOutline && getCheckedState(cbControls.outline))
            cats.push(getCheckboxValue(cbControls.outline));

        if (((isOutline && cats.length < 7) || (!isOutline && cats.length < 6)) && cats.length > 0) {
            query = "&cat=" + cats.join(',');
        }
        return query;
    },

    getLicenseQuery: function() {
        var filters = new Array();
        var cbControls = this.vars.checkBoxIDS;
        var query = "";

        if (this.vars._isResultsPage)
            cbControls = CorbisUI.GlobalVars.SearchResults.checkBoxIDS;

        if (getCheckedState(cbControls.rmLicense))
            filters.push(getCheckboxValue(cbControls.rmLicense));

        if (getCheckedState(cbControls.rfLicense))
            filters.push(getCheckboxValue(cbControls.rfLicense));

        // I'm doing the logic this way to make it easier to add another license model later -jf
        if (filters.length < 2 && filters.length > 0)
            query = "&lic=" + filters.join(',');
        return query;
    },

    getNoPeopleQuery: function() {
        var query = "";
        if (this.vars._isResultsPage && getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.noPeople))
            query = "&np=" + getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.noPeople);
        return query;
    },

    getMediaTypeQuery: function() {
        var filters = new Array();
        var query = "";
        if (this.vars._isResultsPage) {
            if (getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.photography))
                filters.push(getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.photography))

            if (getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.illustration))
                filters.push(getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.illustration))

            if (filters.length == 1)
                query = "&mt=" + filters.join(',');
        }
        return query;
    },

    getColorFormatQuery: function() {
        var filters = new Array();
        var query = "";
        if (this.vars._isResultsPage) {
            if (getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.color))
                filters.push(getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.color))

            if (getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.blackWhite))
                filters.push(getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.blackWhite))

            if (filters.length == 1)
                query = "&cf=" + filters.join(',');
        }
        return query;
    },

    getModelReleaseQuery: function() {
        var query = "";
        if (this.vars._isResultsPage && getCheckedState(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.onlyModelReleased))
            query = "&mr=" + getCheckboxValue(CorbisUI.GlobalVars.SearchResults.checkBoxIDS.onlyModelReleased);
        return query;
    },

    getMarketingCollectionQuery: function() {
        var str = "";
        var sep = "";
        var isAllChecked = true;
        var items = $('MSO_accordion').getElements('input[type=checkbox]')
        items.each(function(el) {
            if (el.checked) {
                str += sep + el.value;
                sep = ",";
            }
            else {
                isAllChecked = false;
            }
        });
        return (isAllChecked || str == "") ? "" : "&mrc=" + str;
    },

    combineSearchBuddy: function() {
        if (this.vars._isResultsPage) {
            if (CorbisUI.SearchBuddy.validateCheckBoxes()) {
                CorbisUI.ExtendedSearch.validateSearch();
            }
        }
        else {
            CorbisUI.ExtendedSearch.validateSearch();
        }
        return false;
    },

    validateSearch: function() {
        if (CorbisUI.ExtendedSearch.vars._isAddLightBox) return;

        this.validateSearchString();
        if (CorbisUI.ExtendedSearch.validateMSO()) {
            if (this.vars._isResultsPage || this.testCategoriesChecked()) {
                if (this.vars.noSearchMsg) {
                    alert(CorbisUI.ExtendedSearch.vars.EmptySearchStringAlert); return false;
                }
                CorbisUI.ExtendedSearch.invokeSearch(false);
            }
        }
    },

    // Done
    validateSearchString: function() {
        try { if (timer != null) window.clearInterval(timer); } catch (er) { }
        var imageNumbersValue = '';
        if ($(CorbisUI.ExtendedSearch.vars.ImageNumbers)) {
            imageNumbersValue = $(CorbisUI.ExtendedSearch.vars.ImageNumbers).value.trim();
            if (imageNumbersValue == this.vars.ImageNumbersText) {
                imageNumbersValue = '';
            }
        }

        var keywordValue = CorbisUI.ExtendedSearch.vars.KeywordSearch.value.trim();
        if (keywordValue == CorbisUI.ExtendedSearch.vars.SearchImages) {
            keywordValue = '';
        }

        if (imageNumbersValue != '' || keywordValue != '') {
            this.vars.noSearchMsg = false;
        }
    },

    validateMarketingCollections: function() {
        $$('div.MSO_toggler').each(function(el) {
            var elementId = el.id.toString();
            var items = CorbisUI.ExtendedSearch.vars.MSO.collections[elementId].domObj.getNext('div.MSO_element').getElements('input[type=checkbox]');
            var numTotal = 0;
            var numChecked = 0;
            items.each(function(item) {
                numTotal++;
                if (item.checked) {
                    numChecked++;
                }
            });
            if (numTotal > numChecked) {
                CorbisUI.ExtendedSearch.vars.noSearchMsg = false;
            }
        });
    },

    validateMSO: function() {
        // if MSO is null, either the user is looking at a previously-validated search result
        // or they are using the default settings.
        if ($('moreSearchOptionsWindow') == null) {
            return true;
        }
        // Check Categories and License types
        CorbisUI.ExtendedSearch.vars._MSOErrorString = '';
        var pop = $('noSearchResultsWarning');
        if (!this.testCategoriesChecked(true)) {
            if (CorbisUI.ExtendedSearch.vars.noLicense)
                CorbisUI.ExtendedSearch.appendEr(pop.getElement('p[textKey=License]').get('text'));
            if (CorbisUI.ExtendedSearch.vars.noCat)
                CorbisUI.ExtendedSearch.appendEr(pop.getElement('p[textKey=Category]').get('text'));
            alert(CorbisUI.ExtendedSearch.vars._MSOErrorString);
            return false;
        }
        // Check for "ITEMS IN THE LAST X DAYS"
        var IMA_last_items = $('moreSearchOptions').getElement('div.IMA_lastDays').getElements('input[type=text]').get('value');
        //check that number of days is indeed a number
        var daysRadio = $(CorbisUI.ExtendedSearch.vars.DaysButton);
        if (daysRadio.checked) {
            if (isNaN(IMA_last_items)) {
                CorbisUI.ExtendedSearch.appendEr(pop.getElement('p[textKey=NotNumeric]').get('text'));
                alert(CorbisUI.ExtendedSearch.vars._MSOErrorString);
                var theField = $('moreSearchOptions').getElement('div.IMA_lastDays').getElements('input[type=text]');
                theField[0].focus();
                return false;
            } else {
                try {
                    if (!isNaN(parseInt(IMA_last_items, 10))) {
                        this.vars.noSearchMsg = false;
                    }
                } catch (ex)
                { }
            }
        }

        if (!CorbisUI.MSOSearch.checkDateValues()) {
            return false;
        }

        // Check Marketing Collections
        this.validateMarketingCollections();
        // Check for Date Created
        if ($(this.vars.DateCreated).value.trim() != '') {
            this.vars.noSearchMsg = false;
        }

        // Check for Location
        if ($(this.vars.Location).value.trim() != '') {
            this.vars.noSearchMsg = false;
        }

        // Check for Photographer
        if ($(this.vars.Photographer).value.trim() != '') {
            this.vars.noSearchMsg = false;
        }

        // Check for Provider
        if ($(this.vars.Provider).value.trim() != '') {
            this.vars.noSearchMsg = false;
        }

        // Check for Point of View
        if ($(this.vars.PointOfView).selectedIndex > 0) {
            this.vars.noSearchMsg = false;
        }

        // Check for ImmediateAvailablility
        if ($(this.vars.ImmediateAvailablility).selectedIndex > 0) {
            this.vars.noSearchMsg = false;
        }

        return true;
    },

    appendEr: function(sErr) {
        CorbisUI.ExtendedSearch.vars._MSOErrorString += sErr;
    },

    showCalendar1: function() {
        $find(CorbisUI.ExtendedSearch.vars.MSOStartDateExtend).show();
        radioClicked('radioBetweenDiv');
        $find(CorbisUI.ExtendedSearch.vars.MSOStartDateExtend).onfocus = CorbisUI.ExtendedSearch.showCalendar1;
    },

    showCalendar2: function() {
        $find(CorbisUI.ExtendedSearch.vars.MSOEndDateExtend).show();
        radioClicked('radioBetweenDiv');
        $find(CorbisUI.ExtendedSearch.vars.MSOEndDateExtend).onfocus = CorbisUI.ExtendedSearch.showCalendar2;
    },
    hideCalendar1: function() {
        $find(CorbisUI.ExtendedSearch.vars.MSOStartDateExtend).hide();
    },

    hideCalendar2: function() {
        $find(CorbisUI.ExtendedSearch.vars.MSOEndDateExtend).hide();
    },

    hijackTheEnter: function(e) {
        var code;
        if (!e) var e = window.event;
        e = e || window.event;
        var code = e.keyCode || e.which;
        if (code == 13) {
            CorbisUI.ExtendedSearch.combineSearchBuddy();

            //Stop any further attempts by IE to submit everything on the internet with one key
            e.returnValue = false;
            return false;
        }
    },

    // Determines if we should show the option applied notice on the search box
    showOptionAppliedStyle: function() {
        var getOptionsAppliedStyle = $(CorbisUI.GlobalVars.Search.showOptionsAppliedStyle);
        if (getOptionsAppliedStyle.value == 'true' || getOptionsAppliedStyle.value == 'True') {
            this.applyOptionStyles();
        }
    },

    // Applies the options applied notice on the search box
    applyOptionStyles: function() {
        try {
            CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value = "UnOpened";
        }
        catch (e) { }
        $("Keywords").addClass("optionsApplied");
        if ($type($("Keywords").getElement('input[type=text]'))) {
            $("Keywords").getElement('input[type=text]').setStyle('background-color', '#ffffcc');
        }
        if ($type($('optionsAppliedDiv')) == 'element') {
            $("optionsAppliedDiv").addClass("optionsAppliedDiv").setStyle("display", "block");
        }
        if ($('optionsAppliedDiv') != null) {
            $$('#Search .optionsApplied input').addClass("optionsAppliedInput");
        }
    },

    nextPage: function() {
        if (this.vars.currentPage && this.vars.currentPage < this.vars.totalPages)
            this.gotoPage(this.vars.currentPage + 1);
    },

    previousPage: function() {
        if (this.vars.currentPage && this.vars.currentPage > 1)
            this.gotoPage(this.vars.currentPage - 1);
    },

    pageNumberKeypress: function(event, element) {
        // on CR redirect to new page number
        if (event.keyCode == 13) {
            if (element.value != '' && !isNaN(element.value) && this.vars.totalPages && element.value <= this.vars.totalPages && element.value >= 1)
                this.gotoPage(element.value);
            return false;
        }
        //allow numbers and control characters only for fireefox
        else if (Browser.Engine.gecko) {
            if ((event.charCode < 48 || event.charCode > 57) && (event.keyCode == 0) && !event.ctrlKey && !event.altKey)
                return false;
        }
        //allow numbers and control characters only for others
        else if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode > 31 && event.keyCode != 127 && !event.ctrlKey && !event.altKey)
            return false;

        return (WebForm_TextBoxKeyHandler(event));
    },

    // redirects to new page
    gotoPage: function(pageNo) {
        if (pageNo <= this.vars.totalPages && pageNo >= 1) {
            this.ShowSearchProgIndicator();
            var newLoc = location.search;
            if (newLoc.match(/\&p\=/)) {
                newLoc = newLoc.replace(/\&p\=[0-9]+/, "&p=" + pageNo);
            }
            else {
                newLoc += "&p=" + pageNo;
            }

            // only save the page turn cookie on results from a manual search.
            if (this.vars.IsManualSearch) {
                this.SetSearchCookie(UrlEncode(newLoc.substr(1)));
            }

            location.search = newLoc;
        }
    }
}


MSOaccordion = new Class({
    Implements: [Options, Events],
    options: {
        toggler: 'div.MSO_toggler',
        elements: 'div.MSO_element'
    },
    // object references
    accordion: null, // accordian wrapper dom object
    collections: {},

    initialize: function(options) {
        if (options) this.setOptions(options);
        this.accordion = $('MSO_accordion');

        // grab needed dom objects

        var sa, dsa;
        var collectionGroups = this;

        this.accordion.getElements('div.MSO_toggler').chunk(function(el) {
            var elementId = el.id.toString();
            collectionGroups.collections[elementId] = { domObj: null, span: null, total: 0, checked: 0 };
            collectionGroups.collections[elementId].domObj = el;
            collectionGroups.collections[elementId].span = el.getElement('span.counts');
            collectionGroups.updateStatCounts(elementId, true);
            collectionGroups.setupMultiselectors(elementId);
            el.addEvents({
                'mouseover': function() {
                    if (!this.hasClass('MSO_togglerHover')) this.addClass('MSO_togglerHover');
                },
                'mouseout': function() {
                    if (this.hasClass('MSO_togglerHover')) this.removeClass('MSO_togglerHover');
                }
            });
        },1);

        // setup accordion  
        this.setupAccordion();
    },

    setupMultiselectors: function(section) {
        // sa = select all link
        // dsa = deselect all link
        var sa, dsa;
        sa = this.collections[section].domObj
            .getNext('div.MSO_element')
            .getElement('a.selectAllLink');
        sa.store('section', section)
            .addEvent('click', this.selectAll.bindWithEvent(this, sa));

        dsa = this.collections[section].domObj
            .getNext('div.MSO_element')
            .getElement('a.deselectAllLink');
        dsa.store('section', section)
            .addEvent('click', this.deselectAll.bindWithEvent(this, dsa));
    },

    // select all
    selectAll: function(e, ele) {
        var section = ele.retrieve('section');
        var items = this.collections[section].domObj
                         .getNext('div.MSO_element')
                         .getElements('input[type=checkbox]');

        items.each(function(item) {
            if (!item.checked) {
                var decoyCheckbox = item.getParent().getPrevious('img.checkbox');
                decoyCheckbox.onclick();
            }
        }, this);

        this.updateStatCounts(section);
    },

    // deselect all
    deselectAll: function(e, ele) {
        var section = ele.retrieve('section');
        var items = this.collections[section].domObj
                         .getNext('div.MSO_element')
                         .getElements('input[type=checkbox]');

        items.each(function(item) {
            if (item.checked) {
                var decoyCheckbox = item.getParent().getPrevious('img.checkbox');
                decoyCheckbox.onclick();
            }
        }, this);

        this.updateStatCounts(section);

    },

    // checkbox click event

    checkboxClickEvent: function(e, ele) {
        this.updateStatCounts(ele.retrieve('section'));
    },

    updateStatCounts: function(section, setup) {
        var items = this.collections[section].domObj.getNext('div.MSO_element').getElements('input[type=checkbox]');
        this.collections[section].total = 0;
        this.collections[section].checked = 0;
        items.each(
                function(item) {
                    this.collections[section].total = this.collections[section].total + 1;
                    if (item.checked) {
                        this.collections[section].checked = this.collections[section].checked + 1;
                    }
                    if (setup) {
                        var decoyCheckbox = item.getParent().getPrevious('img.checkbox');
                        decoyCheckbox.store('section', section);
                        item.store('section', section);
                        decoyCheckbox.addEvent('click', this.checkboxClickEvent.bindWithEvent(this, decoyCheckbox));
                    }
                },
                this
            );
        var formatCountObject = { "0": this.collections[section].checked, "1": this.collections[section].total };
        this.collections[section].span.set('text', this.collections[section].checked + '/' + this.collections[section].total);
        this.collections[section].span.set('title', CorbisUI.MSOSearch.vars.CollectionCountStringFormat.substitute(formatCountObject));
    },

    setupAccordion: function() {
        new Accordion(this.accordion, this.options.toggler, this.options.elements, {
            opacity: true,
            height: true,
            fixedHeight: 210,
            onActive: function(toggler, element) {
                toggler.addClass('MSO_togglerOn');
                var sibling = toggler.getNext();
                sibling.getElement('.collectionOptionsWrap').setStyle('overflow', 'hidden');
            },
            onBackground: function(toggler, element) {
                toggler.removeClass('MSO_togglerOn');
                var sibling = toggler.getNext();
                sibling.getElement('.collectionOptionsWrap').setStyle('overflow', 'hidden');
            },
            onComplete: function(toggler, element) {
                var sibling = toggler.getNext();
                $('MSO_accordion').getElements('.MSO_element').each(
                    function(el) {
                        if (el.getStyle('height') != '0px')
                            el.getElement('.collectionOptionsWrap').setStyle('overflow', 'auto');
                    });
            }
        });
    }
});

function checkOptionsApplied() {
    if ($type($('optionsAppliedDiv')) == 'element') {
        if (CorbisUI.GlobalVars.Search.sb_KeywordSearch.value.trim() == CorbisUI.GlobalVars.Search.sb_SearchImages) {
            var keywordSearchTextbox = CorbisUI.GlobalVars.Search.sb_KeywordSearch;
            if ($('Keywords').hasClass('optionsApplied')) {
                keywordSearchTextbox.setStyle('background-color', '#ffffcc');
            }
            if (!$('Keywords').hasClass('optionsApplied')) {
                keywordSearchTextbox.setStyle('background-color', '#ffffff');
            }
        }
    }
}
String.prototype.endsWith = function(str)
{return (this.match(str+"$")==str)}
