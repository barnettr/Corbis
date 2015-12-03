/****************************************************
    Corbis UI Queueing Classes
    
    This file doesn't quite follow the pattern
    of the other CorbisUI.js files since it has 
    multiple classes with a different namespaces
    than the filename. This is ok. Just wanted
    one file for this to put all Queueing classes.
    Rob Barnett
***************************************************/


/****************************************************
QUEUE CLASSES
- QueueManagerClass
- QueueListClass
	
Basic mechanism to create queues to run in 
sequence. Useful for DomReady or page OnLoad
events. But can be useful to setup basic macros
that can be rerun if a QueuList has been setup 
as being able to rerun. Othewise the queue will
run once and destroy itself.
	
See Rob for more info.
****************************************************/

/****************************************************
QUEUE MANAGER CLASS
    
Class that manages ALL queues.
****************************************************/
CorbisUI.QueuingManagerClass = new Class({
    queueList: null,

    hashMerge: 'each get has set getKeys getLength',

    initialize: function() {
        this.queueList = new Hash({});

        // lets merge some of the Hash's methods onto 'this'
        // and bind it to the queueList hash
        this.hashMerge.split(' ').each(function(item) {
            this[item] = this.queueList[item].bind(this.queueList);
        }, this);
    },

    // UTILITY: CHECK IF A QUEUE EXISTS
    //
    // type: (string) the name of your queue list
    //
    queueExists: function(type) {
        if (!this.has(type) && !this[type]) return false;
        return true;
    },

    // CREATE A NEW QUEUE LIST
    //
    //    type: (string) the name of your queue list
    // options: (object) see QueueListClass.options for possible variables
    //
    addQueue: function(type, options) {

        if (!this.queueExists(type)) {
            // create a rudimentary getter
            // example
            // if you create a new queue like:
            //      CorbisUI.QueueManager.addQueue('bob');
            // you can access it like this:
            //      CorbisUI.QueueManager.bob.<QueueListClass Methods>
            //
            this.set(type, function(el) { return this[el]; } .pass(type, this));
            this[type] = new CorbisUI.QueueListClass(type, this, options);
            //this[type].parentObj.bind(this);
        }
        return this[type];
    },

    // REMOVE A QUEUE LIST - destroys a particular queue
    //
    // type: (string) name of the queue list
    //
    removeQueue: function(type) {
        if (this.queueExists(type)) {
            this[type] = false;
            this.queueList.erase(type);
        }
    },

    /* =======================================
    SHORTCUTS TO QueueListClass methods 
    ======================================= 
    */

    // ADD A QUEUE LIST ITEM - shortcut to QueueListClass.set
    //
    //            type: (string) name of the queue list
    //             key: (string) name of the queue list item
    //            func: (function) function mapped to queue list item
    // overrideIfFound: (boolean) if true, it will reset the function
    //
    addQueueItem: function(type, key, func, overrideIfFound) {
        if (this.queueExists(type)) this[type].set(key, func, overrideIfFound);
    },

    // REMOVE A QUEUE LIST ITEM - shortcut to QueueListClass.erase
    //
    // type: (string) name of the queue list
    //  key: (string) name of the queue list item
    //
    removeQueueItem: function(type, key) {
        if (this.queueExists(type)) this[type].erase(key);
    },

    // REMOVE A QUEUE LIST ITEM - shortcut to QueueListClass.erase
    //
    //     type: (string) name of the queue list
    //  options: (object) object options { delay: true/false, delayTime: (int)}
    //           if delay is true, and no delayTime set, it defaults to 100ms
    //
    runQueue: function(type, options) {
        if (this.queueExists(type)) this[type].run(options);
    },

    // RUN SINGLE QUEUE ITEM
    // run a single item in a queue list
    // this is used only for queues that are rerunQueues 
    //
    //     queue: (string) name of the queue list
    // queueItem: (string) name of the queue list item
    runQueueItem: function(queue, queueItem) {
        if (this.queueExists(type)) this[type].runItem(queueItem);
    }

});

/****************************************************
QUEUE LIST CLASS
        
Class that manages runable items in a Queue.
This gets initiated from the QueueManagerClass
via the method addQueue.
    
Options available
- canRerun : (boolean) Whether a queue can be rerun.
Queue will always exist until told to
destroy itself explicitly. 
Defaults to false
- delay : (boolean) Whether a queue should delay
between each queue function when running.
Defaults to false.
- delayTime : (int) Time in milliseconds for delay.
Defaults to 100ms if delay=true and
delayTime is not set.
-runOnDomReady : (boolean) Flag as to whether 
queue should run on dom ready.
Defaults to false.
****************************************************/
CorbisUI.QueueListClass = new Class({
    Implements: [Options],
    options: {
        canRerun: false,
        delay: false,
        delayTime: 0,
        runOnDomReady: false
    },

    hashMerge: 'each has keyOf hasValue extend combine erase get empty include map filter every some getClean getKeys getValues getLength',

    queueItems: null, // placeholder for items hash
    listName: null, // name of list

    parentObj: null, // reference to the queuemanager instance that called it
    // this gets bound in the queuemanager after QueueListClass instance created

    // alias placeholders
    addItem: null,
    destroy: null,

    // INITIALIZE QUEUE LIST
    initialize: function(name, parentObj, options) {
        if (options) this.setOptions(options);
        if (this.options.delay && this.options.delayTime == 0) this.options.delayTime = 100;

        this.listName = name;
        this.queueItems = new Hash({});
        this.parentObj = parentObj;

        // alias destroy method - see QueueManagerClass.removeQueue
        this.destroy = parentObj.removeQueue.bind(parentObj).pass(this.listName);

        // lets merge some of the Hash's methods onto 'this'
        // and bind it to the queueItems hash
        this.hashMerge.split(' ').each(function(item) {
            this[item] = this.queueItems[item].bind(this.queueItems);
        }, this);

        // setup domready if the option is on
        if (this.options.runOnDomReady) this.setupDomReady();

        // setup some aliases
        this.addItem = this.set;

    },

    // ADD A QUEUE LIST ITEM
    //
    //             key: (string) name of the queue list item
    //            func: (function) function mapped to queue list item
    // overrideIfFound: (boolean) if true, it will reset the function
    //
    set: function(key, func, overrideIfFound) {
        if (this.has(key) && !overrideIfFound) {
            throw "QueueListClass ERROR: the '" + this.listName + "' queue has an items called " + key + " already. Choose another key please.";
            return false;
        }
        if ($type(func) != "function") {
            throw "QueueListClass ERROR: the queue item is not a function. Fixit please.";
            return false;
        }
        this.queueItems.set(key, func);
        return this;
    },

    // RUN QUEUE LIST - runs through entire queue list
    //
    //  options: (object) object options { delay: true/false, delayTime: (int)}
    //           if delay is true, and no delayTime set, it defaults to 100ms
    //
    run: function(options) {

        var tempOptions = this.options;
        if (options) $merge(tempOptions, options);
        if (tempOptions.delay && tempOptions.delayTime == 0) tempOptions.delayTime = 100;

        if (this.getLength() > 0) {
            this.each(function(value, key) { (function() { value.run() }).delay(tempOptions.delayTime); });
        }

        // if this queue is not rerunnable, then destroy it
        if (!this.options.canRerun) this.destroy();
    },

    // RUN SINGLE QUEUE ITEM
    // run a single item in a queue list
    // this is used only for queues that are rerunQueues 
    //
    // queueItem: (string) name of the queue list item
    runItem: function(queueItem) {
        var runItem = this.queueItems.get(queueItem);
        if (runItem != null) runItem.run();
        return this;
    },

    // RUN A SEQUENCE
    // run queue items in the sequence entered
    //
    // arguments: name of the queue list items in the order you want them run
    // example - INSTANCE.runSequence('bob','fred','tony');
    //
    runSequence: function() {
        for (var i = 0; i < arguments.length; i++) {
            var runItem = this.queueItems.get(arguments[i]);
            if (runItem != null) {
                (function(value) { value.run() }).pass(runItem).delay(this.options.delayTime);
            }
        }
        return this;
    },

    // DESTROY QUEUE LIST
    //
    // This will destroy this instance
    //
    //        destroy: function() {
    //            this.parentObj.removeQueue(this.listName); // say goodbye to your little friend
    //        },

    // SET THE DELAY
    //
    // If you forgot to set your delay, you can do it here
    //
    setDelay: function(delayTime) {
        this.options.delay = (delayTime > 0) ? true : false;
        this.options.delayTime = (this.options.delay) ? delayTime : 0;
        return this;
    },

    // SET IF IT CAN RERUN
    //
    // If you forgot to set whether the queue can rerun, you can do it here
    //
    setRerun: function(canI) {
        this.options.canRerun = canI;
        return this;
    },

    runOnDomReady: function() {
        this.options.runOnDomReady = true;
        return this;
    },

    setupDomReady: function() {
        window.addEvent('domready', this.run.bind(this));
    }

});


// MAKING ALIASES TO WORK WITH NEW CLASS
// instead of calling CorbisUI.QueueManager.<method>
// you can call CorbisUI.<method>
// more or less to deal with pre-QueueManagerClass calls

CorbisUI.addQueueItem = function(type, key, func) {
    CorbisUI.QueueManager.addQueueItem(type, key, func);
};

CorbisUI.removeQueueItem = function(type, key) {
    CorbisUI.QueueManager.removeQueueItem(type, key);
};

CorbisUI.addQueue = function(type, canRerun) {
    var item = CorbisUI.QueueManager.addQueue(type);
    if (canRerun) item.setRerun(canRerun);
};

CorbisUI.runQueue = function(type, delay, delayTime) {
    var tempOptions = { delay: false, delayTime: 0 };
    if (delay) tempOptions.delay = true;
    if (delayTime > 0) tempOptions.delay = delayTime;
    CorbisUI.QueueManager.runQueue(type, tempOptions);
};

CorbisUI.runQueueItem = function(queue, queueItem) {
    CorbisUI.QueueManager.runQueueItem(type, queueItem);
}
// END QUEUE ALIASES