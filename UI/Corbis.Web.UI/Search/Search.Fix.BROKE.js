
/* THIS IS HERE TEMPORARILY
    WHILE I WORK ON THE SEARCH BUDDY
    WEIRDNESS IN SMALLER WINDOW SIZES
    
    PLEASE DO NOT DELETE
    
    --Chris
*/

//Moved code from here to AddToLightbox.ascx

// SHOULD NOT HAVE MOVED SCRIPTS -- CHRIS

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Search = {};

/* SEARCH BUDDY FLOATER */
CorbisUI.Search.Floater = new Class({
	
	box: null,
	wrap: null,
	footer: null,
	
	initialize: function(el,wrap,footer){
	    //console.log('INITIALIZING BUDDY FLOATER');
		this.box = el;
		this.wrap = wrap;
		this.footer = footer;
		var wC = this.wrap.getCoordinates();
		/*this.box.setStyles({
			'top': wC.top+10 
		});*/
		
		//console.log(wC.toSource());
		
		this.wC = {
			top: wC.top,
			bottom: wC.top+wC.height
		};
		
		// left column magic
		this.rightCol = $('SearchColumnedContent').getElement('.rightColumn');
		//console.log('RCOL: '+rightCol.getCoordinates().toSource());
		this.leftCol = $('SearchColumnedContent').getElement('.leftColumn');
		this.leftCol.setStyle('height',this.rightCol.getCoordinates().height);
		
		
		//this.windowScroll.bind
		window.addEvent('scroll',this.windowScroll.bindWithEvent(this));
		window.addEvent('resize',this.windowScroll.bindWithEvent(this));
		
		// run the Scroll for the first time
		// fixes problem of coming back to a page
		// and the buddy is messed up
		this.windowScroll();
	},
	
	windowScroll: function(){
	    this.leftCol.setStyle('height',this.rightCol.getCoordinates().height);
	    //console.log('scrolling....');
		// WINDOW SCROLL
		var wScroll = window.getScroll();
		//console.log('WINDOW SCROLL: '+wScroll.toSource());

		// WRAP POSITION
		var pos = this.wrap.getPosition();
		
		// WRAP COORDS
		var wrapC = this.wrap.getCoordinates();
		//console.log('WRAP POS: '+pos.toSource());
		
		// FOOTER COORDINATES
		var fC = this.footer.getCoordinates();
		//console.log('FOOTER COORDS: '+fC.toSource());
		
		// WINDOW COORDINATES
		var wiC = window.getCoordinates();
		//console.log('WINDOW COORDS: '+wiC.toSource());

		var newTop,newBottom;
		
		var originalCoords = wrapC;
		
		// EASY
		// detect distance from top
		
		//if(wScroll.y < this.wC.top){
		if(wScroll.y > wrapC.top){
			//this.box.setStyle('top',(pos.y - wScroll.y)+10);
			newTop = (wScroll.y-pos.y)+10; 
			this.box.setStyle('top',newTop);
		}else{
			if(this.box.getStyle('top').toInt() != '10') { this.box.setStyle('top',10); }
		}
		
		// NOT SO EASY
		// detect distance from top of footer
		
		// get value for footer distance check
		var fCheck = fC.top - wiC.height;

		if(wScroll.y > fCheck){

			//this.box.setStyle('bottom', (wiC.height-(fC.top-wScroll.y))+20);
            newBottom = (wiC.height-(fC.top-wScroll.y))+10;
            
		}else{
			//if(this.box.getStyle('bottom').toInt() != '10') this.box.setStyle('bottom',10);
			newBottom = 10;
		}
		
		if(Browser.Engine.gecko){
		/*console.log('======[ COORD VALUES ]======');
		console.log('Footer Check: '+fCheck);
		console.log('Wrap Pos: '+pos.toSource());
		console.log('Wrap Coords: '+wrapC.toSource());
        console.log('Window Scroll: '+wScroll.toSource());
        console.log('Footer Coords: '+fC.toSource());
        console.log('Window Coords: '+wiC.toSource());
        console.log('newTop: '+newTop);
        console.log('newBottom: '+newBottom);
        console.log('DIFF: '+(fC.top-wScroll.y));
        console.log(this.box.getCoordinates().toSource());*/
		}
		// SET THE STYLES
		//this.box.setStyle('top',newTop);
		
		/*var DiffCheck = fC.top-wScroll.y;
		if(DiffCheck > 550){
		    newBottom = 550-DiffCheck;
		    console.log('NEW DIFF: '+newBottom);
		    this.box.setStyle('bottom', newBottom);
		}else{
		    this.box.setStyle('bottom', newBottom);
		}
		*/
		
		
	}
	
});
var Floater;
window.addEvent('domready',function(){
    Floater = new CorbisUI.Search.Floater($('SearchBuddy'),$('MainContent'),$('FooterContent'));

});