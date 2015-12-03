/*

  |:::::::::::::;;::::::::::::::::::|
  |:::::::::::'~||~~~``:::::::::::::|
  |::::::::'   .':     o`:::::::::::|
  |:::::::' oo | |o  o    ::::::::::|
  |::::::: 8  .'.'    8 o  :::::::::|
  |::::::: 8  | |     8    :::::::::|
  |::::::: _._| |_,...8    :::::::::|
  |::::::'~--.   .--. `.   `::::::::|
  |:::::'     =8     ~  \ o ::::::::|
  |::::'       8._ 88.   \ o::::::::|
  |:::'   __. ,.ooo~~.    \ o`::::::|
  |:::   . -. 88`78o/:     \  `:::::|
  |::'     /. o o \ ::      \88`::::|   "Join us or die."
  |:;     o|| 8 8 |d.        `8 `:::|
  |:.       - ^ ^ -'           `-`::|
  |::.                          .:::|
  |:::::.....           ::'     ``::|
  |::::::::-'`-        88          `|
  |:::::-'.          -       ::     |
  |:-~. . .                   :     |
  | .. .   ..:   o:8      88o       |
  |. .     :::   8:P     d888. . .  |
  |.   .   :88   88      888'  . .  |
  |   o8  d88P . 88   ' d88P   ..   |
  |  88P  888   d8P   ' 888         |
  |   8  d88P.'d:8  .- dP~ o8       |   Darth Javascript (1)
  |      888   888    d~ o888    LS |
  |_________________________________|

*/

/****************************************************
Corbis Javascript namespace intialization
	
=============================================
Chris Esler, 2008-01-01
=============================================
****************************************************/



// CORBIS UI NAMESPACE SETUP
var CorbisUI = {
    version: '0.0.1',
    registry: {
        Id: [],
        deferred: [],
        includes: []
    },
    
    forKomodo: 'asDaSdalkNVa1234123lncasFioasCllk109Gh324KL',
    
    // define global variable namespace
    GlobalVars: {},

    debug: false,

    dirs: {},

    options: {
        environment: 'browser'
    },

    // Placeholder for DomCache - which uses CachingClass
    DomCache: null,

    // Placeholder for the main QueueManager instance - which uses QueueManagerClass
    QueueManager: null,

    init: function(options) {

        //console.log('calling CorbisUI.init()');
        if ($type(options) == 'object') this.options = $merge(this.options, options);



        // initialize DomCache
        CorbisUI.DomCache = new CorbisUI.CachingClass();

        // initialize QueueManager
        CorbisUI.QueueManager = new CorbisUI.QueuingManagerClass();

        // add some default queues
        CorbisUI.QueueManager.addQueue('domReady', { runOnDomReady: true });
        CorbisUI.QueueManager.addQueue('afterDomReady');


        // if in http mode, then run cookie events 
        if (Browser.getHost()) {

            // initialize js serializer
            CorbisUI.JSSerializer = new CorbisUI.JSSerializer();

            // initialize CookieEvents
            if (Browser.getHost()) CorbisUI.CookieEvents = new CorbisUI.CookieEvents();

            // run cookie events
            CorbisUI.QueueManager.afterDomReady.addItem('cookieEvents', CorbisUI.CookieEvents.runCookieEvents.bind(CorbisUI.CookieEvents));

        }

        // setup event for afterDomReady queue
        window.addEvent('load', function() {
            CorbisUI.QueueManager.runQueue('afterDomReady');
        });
    }


    
};



/***************************************************************************
    Splitting out some namespaces to make this more manageable
    
    bootstrap : /Scripts/CorbisUI-BOOTSTRAP.js
    
    CorbisUI.QueueManagerClass : /Scripts/CorbisUI.QueueingClasses.js
    CorbisUI.QueuListClass : /Scripts/CorbisUI.QueueingClasses.js
    
    CorbisUI.CachingClass : /Scripts/CorbisUI.CachingClass.js
    CorbisUI.CookieEvents : /Scripts/CorbisUI.CookieEvents.js
    CorbisUI.JSSerializer : /Scripts/CorbisUI.JSSerializer.js
    CorbisUI.MethodFailed : /Scripts/CorbisUI.MethodFailed.js
    CorbisUI.Auth : /Scripts/CorbisUI.Auth.js
    CorbisUI.GlobalNav : /Scripts/CorbisUI.GlobalNav.js
    CorbisUI.ImageGroups : /Scripts/CorbisUI.ImageGroups.js
    CorbisUI.ExpressCheckout : /Scripts/CorbisUI.ExpressCheckout.js
    CorbisUI.Legal : /Scripts/CorbisUI.Legal.js
    CorbisUI.EnlargementTimerSearch : /Scripts/CorbisUI.EnlargementTimerSearch.js
    CorbisUI.MSOSearch : /Scripts/CorbisUI.MSOSearch.js
    CorbisUI.Watermark : /Scripts/CorbisUI.Watermark.js
    CorbisUI.CustomerService : /Scripts/CorbisUI.CustomerService.js
    CorbisUI.MyProfile : /Scripts/CorbisUI.MyProfile.js
    CorbisUI.Pricing : /Scripts/CorbisUI.Pricing.js
    CorbisUI.FormUtilities : /Scripts/CorbisUI.FormUtilities.js
    
****************************************************************************/
