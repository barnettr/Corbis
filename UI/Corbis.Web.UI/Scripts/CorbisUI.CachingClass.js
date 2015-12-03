/****************************************************
    Corbis UI Caching Class
***************************************************/

/****************************************************
CACHING CLASS
        
utility class to cache dom elements that are accessed a lot
        
NOTE: the bulk add is really only for items with an ID,
but there is a method for storing collections/objects
as key,value(the collection or object) which can be
added individually.
        
CorbisUI.DomCache uses this. But you can create
your own caches if that makes it easier for
you to manage.
            
****************************************************/
CorbisUI.CachingClass = new Class({
    cache: null,

    initialize: function() {
        this.cache = new Hash({});
    },

    // this is for adding items that have ID's
    add: function(items) {
        if ($type(items) == 'string') {
            this.cache.set(items, $(items));
        }
        else if ($type(items) == 'array') {
            items.each(function(item) {
                if ($type(item) == 'string') this.cache.set(item, $(item));
            }, this);
        }
    },

    // want to cache an object?
    //    key: (string) name of the object
    // object: (object) the object to be stored
    addObject: function(key, object) {
        this.cache.set(key, object);
    },

    // key: accessor name for cache
    // value: the collection
    // TODO: right now you can add collections one at at time. 
    // need to make it so you can batch add
    addCollection: function(key, collection) {
        this.cache.set(key, collection);
    },

    // items: (string/array) items to fetch
    // reset: (boolean) will force an item to be recached with $()
    get: function(items, reset) {

        if ($type(items) == 'string') {
            if (!this.cache.has(items) || reset) this.add(items);
            return this.cache.get(items);
        }
        else if ($type(items) == 'array') {
            var returnItems = {};
            items.each(function(item) {
                if (!this.cache.has(item) || reset) this.add(item);
                returnItems[item] = this.cache.get(item);
            }, this);
            return returnItems;
        }
    },

    remove: function(items) {
        if ($type(items) == 'string') {
            if (this.cache.has(item)) this.cache.erase(items);
        }
        else if ($type(items) == 'array') {
            items.each(function(item) {
                if (this.cache.has(item)) this.cache.erase(item);
            }, this);

        }
        return;
    },

    has: function(key) {
        return this.cache.has(key) && this.cache[key] != null;
    },

    emptyCache: function() {
        this.cache.empty();
    }

});