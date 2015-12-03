/****************************
    NAMESPACE CHECK
****************************/
if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}

/****************************
    EVENTS WATCHER
****************************/

CorbisUI.Events = {

    // simple class for watching key events
    KeyWatch: new Class({
        Implements: [Options],
        options: {
            toDo: 'preventDefault', // preventDefault, stop, stopPropagation, custom or hash
            event: 'keydown', // keydown should be used mostly for KeyWatch. Although keypress and keyup may work
            custom: false // placeholder for SINGLE custom function
        },

        obj: null, // reference to object to watch
        keys: [], // keys to watch [ NOTE: not the key code. the actual key. You may need to play around with mootools events to see what types of values get passed. ]
        eventItem: null, // placeholder for event function


        initialize: function(keys, options) {
            if (options) this.setOptions(options);

            switch ($type(keys)) {
                case "string":
                case "array":
                    $each(keys, function(item) {
                        this.keys.include(item);
                    }, this);
                    break;
                case "hash":
                case "object":
                    if (this.options.toDo != "hash") this.options.toDo = "hash";
                    this.keys = ($type(keys) == "hash") ? keys : new Hash(keys);
                    break;
            }
            this.obj = (Browser.Engine.trident) ? $(document.body) : $(window) ;

            return this;
        },

        start: function() {
            this.eventItem = this.manager.bind(this);
            this.obj.addEvent(this.options.event, this.eventItem);
        },

        stop: function() {
            this.obj.removeEvent(this.options.event, this.eventItem);
        },

        manager: function(event) {
            //console.log('EVENTS: Manager');
            var method = ($type(this.keys) == "hash") ? "has" : "contains";

            var keyCheck = (this.isSpecialKey(event)) ? this.isSpecialKey(event) : event.key;
            //console.log(keyCheck);

            if (this.keys[method](keyCheck)) {
                switch (this.options.toDo) {
                    case "preventDefault":
                    case "stop":
                    case "stopPropagation":
                        event[this.options.toDo]();
                        break;
                    case "custom":
                        this.options.custom.pass(event).bind(this).run();
                        break;
                    case "hash":
                        this.keys.get(keyCheck).pass(event).bind(this).run();
                        break;
                }
            }
        },

        isSpecialKey: function(e) {
            //console.log(e);
            // NOTE: some of these keys are not detectable in all browsers (like IE)
            var keyHash = new Hash({
                16: "shift",
                17: "ctl",
                18: "alt",
                19: "pause",
                20: "caps",
                144: "numlock",
                145: "scrolllock"

            });
            //alert(e.code);
            var key = (keyHash.has(e.code)) ? keyHash.get(e.code) : false;
            return key;
        }

    })
};

var keyWatch;
window.addEvent('domready', function() {
    keyWatch = new CorbisUI.Events.KeyWatch(['space', 'esc'], { toDo: "stop" });
    keyWatch.start();
});
window.addEvent('load', function() {
    (function() { keyWatch.stop(); }).delay(2000);
});

/*
if (Browser.Engine.gecko) {
    var easterEgg = new CorbisUI.Events.KeyWatch({
        "esc": function(e) {
            alert('numlock');
            if (e.alt && e.control && e.shift && !Browser.isHttps()) {
                var where_to = confirm("Do you like Easter?");
                if (where_to == true) {
                    var port = (Browser.getPort()) ? ":" + Browser.getPort() : "";
                    window.location = "http://" + Browser.getHost() + port + "/Search/SearchResults.aspx?q=easter+egg";
                }
                else {
                    alert('no easter egg for you then!');
                }

            }
        }
    });
    easterEgg.start();
}
*/





