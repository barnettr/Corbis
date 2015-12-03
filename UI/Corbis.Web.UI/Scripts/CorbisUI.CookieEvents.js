/****************************************************
    Corbis UI Cookie Events
***************************************************/

/****************************************************
COOKIE EVENTS
            
Utility to store javascript in a cookie. 
This was created to address problems of anonymous
users attempting to do something, who are prompted
to login. The page refreshes, an the original
event is lost. This way we can programmatically
leapfrog the intended event over the page refresh.
        
NOTE: uses CorbisUI.JSSerializer to serialize
javascript objects into XML string to be stored
in cookie.
        
See Chris for more info/examples.
            
****************************************************/

CorbisUI.CookieEvents = new Class({
    cookie: null,
    cookieData: [],

    cookieOptions: {
        domain: ".corbis.com",
        path: "/"
    },

    initialize: function() {
        //console.profile();
        this.cookieOptions.domain = (Browser.getHost().contains('corbis.com')) ? ".corbis.com" : ".corbis.pre";

        // this next line is for corbisbeta only
        if (Browser.getHost().contains('corbisbeta.com')) this.cookieOptions.domain = ".corbisbeta.com";

        this.cookieOptions.path = (Browser.getCookiePath().toLowerCase() == "default.aspx") ? "/" : "/" + Browser.getCookiePath();

        this.cookie = new Cookie('CorbisCJ', this.cookieOptions);
        //this.cookieData = [];



        if (this.cookie.read() == null) {
            this.cookie.write(CorbisUI.JSSerializer.serialize({ "data": [] }, 'CookieEvents'));
        } else {
            var cdata = CorbisUI.JSSerializer.deserialize(this.cookie.read());
            //console.log(cdata);
            if (!$type(cdata.data)) cdata.data = [];
            this.cookieData.combine(cdata.data);
        }
        //console.profileEnd();
    },
    addCookieEvent: function(func, funcVars) {
        var cdata = CorbisUI.JSSerializer.deserialize(this.cookie.read());
        //console.log(cdata);
        this.cookieData.combine(cdata.data);

        var temp = {};

        temp.runMe = func;
        if (funcVars) temp.vars = funcVars;

        this.cookieData.include(temp);
        this.cookie.write(CorbisUI.JSSerializer.serialize({ "data": this.cookieData }, 'CookieEvents'));
    },

    runCookieEvents: function() {
        if (this.cookieData.length > 0) {
            //console.log('RUNNING COOKIE EVENTS');
            //console.log(this.cookieData.getKeys());
            this.cookieData.each(function(item, key) {
                //console.log('--<' + key + '>');
                if (item.runMe) item.runMe();
            });
            this.cookieData.empty();
        }
        this.cookie.write(CorbisUI.JSSerializer.serialize({ "data": [] }, 'CookieEvents'));
    },

    addCookieEvent_altPath: function(path, func, funcVars) {

        var tmpOptions = this.cookieOptions;
        tmpOptions.path = path;

        var cookie = new Cookie('CorbisCJ', tmpOptions);

        var cdata = CorbisUI.JSSerializer.deserialize(cookie.read());
        //console.log(cdata);
        this.cookieData.combine(cdata.data);

        var temp = {};

        temp.runMe = func;
        if (funcVars) temp.vars = funcVars;

        var cookieData = [];

        if (cookie.read() == null) {
            cookie.write(CorbisUI.JSSerializer.serialize({ "data": [] }, 'CookieEvents'));
        } else {
            var cdata = CorbisUI.JSSerializer.deserialize(cookie.read());
            //console.log(cdata);
            cookieData.combine(cdata.data);
        }

        cookieData.include(temp);
        cookie.write(CorbisUI.JSSerializer.serialize({ "data": cookieData }, 'CookieEvents'));
    }

});
