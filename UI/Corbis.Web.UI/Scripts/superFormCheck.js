
/****************************************************
	Corbis Javascript namespace
	
	SFC = Super Form Check
	
	=============================================
		Chris Esler, 2008-10-28
	=============================================
****************************************************/

CorbisUI.defineClass("CorbisUI.SFC",
{
	
	// just a container
	statics : {
		FormCheck: new Class({
		    Implements: [Options, Events],
		    
		    form: null,
		    options: {},
		    
		    initialize: function(el, options){
		        if(options) this.setOptions(options);
		        this.form = $(el);
		    }
			
		}),
		
		RegexpManager: new Class({
		    Implements: Options,
		    
		    options: {},
		    
		    initialize: function(conditions, options){
		        if(options) this.setOptions(options);
		    }
		});
		
	},
	
	defer : function(statics) {
	    // cleanup memory
	    statics = $lambda(false);
	}
});
