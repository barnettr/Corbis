/* EXTENSIONS OF NATIVE JAVASCRIPT OBJECTS */

// helper enhancements to Native objects

Array.implement({
    batchRun: false,
    /*
    this is a function to help free up memory
    when looping through LARGE arrays
        
    process: function to be run
    delay: delay time (defaults to 100)
    context: if you want to bind something to process
    */
    chunk: function(process, delay, context, Complete, batch) {
        var delayTime = ($type(delay) == 'number') ? delay : 100;
        var items = this.concat();
        if (items.length == this.length && batch) {
            //console.log('going to batch!');
            this.batchBase = batch;
            this.batchCount = 0;
            this.batchDelay = delayTime;
            this.batchRun = true;
        }
        setTimeout(function() {
            if (this.batchRun) {
                //console.log('still in batch');
                //console.log(this.batchBase + ' | ' + this.batchCount);
                if (this.batchCount == this.batchBase) {
                    this.batchCount = 0;
                    delayTime = this.batchDelay;
                } else {
                    delayTime = 0;
                    this.batchCount++;
                }
                //console.log('delay: ' + delayTime);
            }
            var item = items.shift();
            if (context) process.bind(context);

            if (!this.stopChunkProcess) process.pass(item).run();

            if (items.length > 0 && !this.stopChunkProcess) {
                if (delayTime == 0) {
                    arguments.callee.bind(this).run();
                } else {
                setTimeout(arguments.callee.bind(this), delayTime);
            }
            }
            if (this.stopChunkProcess) {
                items.empty();
                this.stopChunkProcess = false;
            }
            //if (items.length == 0 && Complete) Complete.run();

        } .bind(this), delayTime);

    },

    stopChunk: function() {
        this.stopChunkProcess = true;
    },

    flip: function() {
        this.reverse();
        return this;
    }
});

String.implement({

    // *     example 1: string.empty(null);
    // *     returns 1: true
    // *     example 2: string.empty(undefined);
    // *     returns 2: true
    // *     example 3: string.empty([]);
    // *     returns 3: true
    // *     example 4: string.empty({});
    // *     returns 4: true
    isEmpty: function() {

        var key;

        if (this === ""
            || this === 0
            || this === "0"
            || this === null
            || this === false
            || this === undefined
        ) {
            return true;
        }
        if (typeof this == 'object') {
            for (key in this) {
                return false;
            }
            return true;
        }
        return false;
    },

    // *     example 1: "kevin's birthday".addslashes();
    // *     returns 1: 'kevin\'s birthday'
    addslashes: function() {
        return (this + '').replace(/([\\"'])/g, "\\$1").replace(/\0/g, "\\0");
    },

    // *     example 1: 'Kevin\'s code'.stripslashes();
    // *     returns 1: "Kevin's code"
    stripslashes: function() {
        return (this + '').replace('/\0/g', '0').replace('/\(.)/g', '$1');
    },

    // *     example 1: 'Kevin van Zonneveld'.strtoupper();
    // *     returns 1: 'KEVIN VAN ZONNEVELD'
    strtoupper: function() {
        return (this + '').toUpperCase();
    },

    // *     example 1: 'Kevin van Zonneveld'.strtolower();
    // *     returns 1: 'kevin van zonneveld'
    strtolower: function() {
        return (this + '').toLowerCase();
    },

    // *     example 1: 'Kevin van Zonneveld'.strrev();
    // *     returns 1: 'dlevennoZ nav niveK'
    strrev: function() {
        var str = this;
        var ret = '', i = 0;

        str += '';
        for (i = str.length - 1; i >= 0; i--) {
            ret += str.charAt(i);
        }

        return ret;
    },

    // *     example 1: '<p>Kevin</p> <br /><b>van</b> <i>Zonneveld</i>'.strip_tags('<i><b>');
    // *     returns 1: 'Kevin <b>van</b> <i>Zonneveld</i>'
    // *     example 2: '<p>Kevin <img src="someimage.png" onmouseover="someFunction()">van <i>Zonneveld</i></p>'.strip_tags('<p>');
    // *     returns 2: '<p>Kevin van Zonneveld</p>'
    // *     example 3: "<a href='http://kevin.vanzonneveld.net'>Kevin van Zonneveld</a>".strip_tags("<a>");
    // *     returns 3: '<a href='http://kevin.vanzonneveld.net'>Kevin van Zonneveld</a>'
    strip_tags: function(allowed_tags) {

        var str = this;
        var key = '', tag = '', allowed = false;
        var matches = allowed_array = [];
        var allowed_keys = {};

        var replacer = function(search, replace, str) {
            return str.split(search).join(replace);
        };

        // Build allowes tags associative array
        if (allowed_tags) {
            allowed_array = allowed_tags.match(/([a-zA-Z]+)/gi);
        }

        str += '';

        // Match tags
        matches = str.match(/(<\/?[^>]+>)/gi);

        // Go through all HTML tags
        for (key in matches) {
            if (isNaN(key)) {
                // IE7 Hack
                continue;
            }

            // Save HTML tag
            html = matches[key].toString();

            // Is tag not in allowed list? Remove from str!
            allowed = false;

            // Go through all allowed tags
            for (k in allowed_array) {
                // Init
                allowed_tag = allowed_array[k];
                i = -1;

                if (i != 0) { i = html.toLowerCase().indexOf('<' + allowed_tag + '>'); }
                if (i != 0) { i = html.toLowerCase().indexOf('<' + allowed_tag + ' '); }
                if (i != 0) { i = html.toLowerCase().indexOf('</' + allowed_tag); }

                // Determine
                if (i == 0) {
                    allowed = true;
                    break;
                }
            }

            if (!allowed) {
                str = replacer(html, "", str); // Custom replace. No regexing
            }
        }

        return str;
    },

    substr: function(f_start, f_length) {
        // Returns part of a string  
        // 
        // version: 810.1317
        // discuss at: http://phpjs.org/functions/substr
        // +     original by: Martijn Wieringa
        // +     bugfixed by: T.Wild
        // +      tweaked by: Onno Marsman
        // *       example 1: 'abcdef'.substr(0, -1);
        // *       returns 1: 'abcde'

        var f_string = this.concat();

        f_string += '';

        if (f_start < 0) {
            f_start += f_string.length;
        }

        if (f_length == undefined) {
            f_length = f_string.length;
        } else if (f_length < 0) {
            f_length += f_string.length;
        } else {
            f_length += f_start;
        }

        if (f_length < f_start) {
            f_length = f_start;
        }

        return f_string.substring(f_start, f_length);
    }


});

