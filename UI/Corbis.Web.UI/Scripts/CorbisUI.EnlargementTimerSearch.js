/****************************************************
    Corbis UI Enlargement Timer Search
***************************************************/

CorbisUI.EnlargementTimerSearch = {
    vars: {
        secs: null,
        timerID: null,
        timerRunning: null,
        delay: null,
        qs: null,
        args: null,
        pair: null,
        name: null
    },

    initialize: function() {
        this.vars.timerRunning = false;
        this.vars.timerID = null;
        this.vars.delay = 1000;
    },

    DecodeQuerystring: function() {
        try {
            this.vars.qs = window.location.search.substring(1);
            this.vars.args = this.vars.qs.split('&');
            if (this.vars.args[2] != "undefined" && this.vars.args[2] != null) {
                this.vars.pair = this.vars.args[2].split("=");
                this.vars.name = decodeURIComponent(this.vars.pair[0]);
                if (this.vars.name == "options") {
                    CorbisUI.ExtendedSearch.OpenOptionsAppliedModal();
                    this.InitializeTimer();
                }
            }
        }
        catch (e) {
        }
    },

    InitializeTimer: function() {
        this.vars.secs = 10;
        this.StopTheClock();
        this.StartTheTimer();
    },

    StopTheClock: function() {
        this.initialize();

        if (this.vars.timerRunning) {
            clearTimeout(this.vars.timerID);
        }
        this.vars.timerRunning = false;
    },

    StartTheTimer: function() {
        this.initialize();

        if (this.vars.secs == 0) {
            this.StopTheClock();
            this.HideAppliedOptionsWindow();
        }
        else {
            this.vars.secs = this.vars.secs - 1;
            this.vars.timerRunning = true;
            this.vars.timerID = setTimeout("CorbisUI.EnlargementTimerSearch.StartTheTimer()", this.vars.delay);
            if (CorbisUI.GlobalVars.Search.sb_KeywordSearch.value == CorbisUI.GlobalVars.Search.sb_SearchImages) {
                $('Search').getElement('.Optional').setStyle('background-color', '#ffffcc');
            }
        }
    },

    HideAppliedOptionsWindow: function() {
        try { $('optionsAppliedModalWindow').setStyle('display', 'none'); } catch (er) { }
    }
}