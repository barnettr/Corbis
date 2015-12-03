
/****************************************************
Based in part on QooxDoo bootstrap
http://www.qooxdoo.org
	
- modified heavily to to workwith mootools
- in both browser and server-side enviroments
	
=============================================
Chris Esler, 2008-01-01
=============================================
****************************************************/

bootstrap = new Class({

    Implements: [Options, Chain],

    options: {
        environment: 'browser'
    },

    initialize: function(options) {
        this.setOptions(options);

        $extend(this, CorbisUI);

        CorbisUI = this;

        //setup bootstrap core
        this.defineClass("CorbisUI.app", {
            statics: {
                LOADSTART: new Date,

                time: function() {
                    return new Date().getTime();
                },

                since: function() {
                    return this.time() - this.LOADSTART;
                }

            }
        });

    },

    // quick way to fetch object from registry
    getObject: function(name) {
        return this.registry.Class[name];
    },

    // method to create namespace and bind it onto the object
    // see defineClass for structure example
    createNamespace: function(name, object) {
        var splits = name.split(".");
        var parent = this;
        var part = splits[0];

        for (var i = 0, len = splits.length - 1; i < len; i++, part = splits[i]) {
            if (i == 0) continue;
            if (!parent[part]) {
                parent = parent[part] = {};
            } else {
                parent = parent[part];
            }
        }

        // get general parent name
        var PNsplits = name.split(".");
        PNsplits.pop();
        var parentName = PNsplits.join(".");

        // add some utility values to object
        var helpers = {
            identifier: {
                'classname': name,
                'basename': part,
                'parentname': parentName
            }
        }

        // extend object with utilities
        if ($type(object.prototype) == "class") {
            object.implement(helpers);
        } else {
            $extend(object, helpers);
        }

        // bridge to make qooxdoo-like
        // object assignment work with mootools
        var tempObject = {};
        tempObject[part] = object;

        // store object
        $extend(parent, tempObject);

        // return last part name (e.g. classname)
        return part;

    },

    // method to dynamically add item to namespace (qooxdoo style)
    //
    // example: assuming CorbisUI is main namespace
    //   
    //    CorbisUI.defineClass("CorbisUI.TEST",
    //    {
    //    	
    //	    // just a container
    //	    // can be function, obect, class, or variable (string/int)
    //	    statics : function(){
    //	        alert('test');
    //	    },
    //    	
    //	    defer : function(statics) {
    //	        // can add domready stuff here
    //	        // cleanup memory
    //	        statics = $lambda(false);
    //	    }
    //    });
    defineClass: function(name, config) {

        var parent = this;

        if (!config) {
            config = { statics: {} };
        };

        if (!config.statics) {
            config.statics = {};
        };

        // create the namespace
        this.createNamespace(name, config.statics);

        // deferring is to run things after namespace is 
        // created. kind of like deferred initialization.
        if (config.defer) {
            config.defer(config.statics);
        }

        // clear up memory
        config = $lambda(false);

    }

});

// startup bootstrap
new bootstrap(CorbisUI.options);

// init CorbisUI namespace engine
CorbisUI.init();

if (!CorbisUI.debug) console.log = function() { };