/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}

//TODO: consolidate scale image from cart, lightbox and search
CorbisUI.Image = {
	ScaleImage: function(ele,options){
		var Base = {
			marginTop: 128, //basic dimensions of container
			width: 128, // minimum width
			height: 128 // minimum height
		};
		if(options) Base = $merge(Base,options);

		var OrigCoords = ele.getCoordinates();
		var Coords = {width: 0, height: 0};
		Coords.width = OrigCoords.width;
		Coords.height = OrigCoords.height;
	    
		var newValues = Base;
	    
		if (Coords.height > Coords.width)
		{
			newValues.width = newValues.width * (Coords.width / Coords.height);    
		}
		else
		{
			newValues.height = newValues.height * (Coords.height / Coords.width); 
		}
	    
		var wrap, img, m;
	    
		if(newValues.height <= Base.marginTop){
			wrap = (Base.marginTop / 2); //half of wrapper
			img = (newValues.height / 2); //half of image
			newValues.marginTop = (wrap - img);
		}
	    
		if(Coords.height < options.marginTop){
		   newValues.width = 90;
	   }
	 
		return newValues;
	}
};

