if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Filmstrip = {};

CorbisUI.Filmstrip.setScrollControls = function(filmstrip)
{
	var filmstripObject = filmstrip.retrieve('filmstripObject');
	
	if (!filmstripObject)
	{
		filmstripObject = new CorbisUI.Filmstrip.filmstripObject(filmstrip);
		filmstrip.store('filmstripObject', filmstripObject);
	}
	
	filmstripObject.setScrollControls(true);
};

CorbisUI.Filmstrip.scrollFilmstrip = function(scrollControl, scrollPrevious)
{
	if (!scrollControl.hasClass('disabled'))
	{
		var filmstrip = $(scrollControl.parentNode);
		var filmstripObject = filmstrip.retrieve('filmstripObject');
		
		if (!filmstripObject)
		{
			filmstripObject = new CorbisUI.Filmstrip.filmstripObject(filmstrip);
			filmstrip.store('filmstripObject', filmstripObject);
		}
		
		filmstripObject.scrollFilmstrip(scrollPrevious);
	}
};

CorbisUI.Filmstrip.filmstripObject = new Class({
    Implements: [Options,Events],
    
    filmstripPrevious: null,
    filmstripNext: null,
    filmstripItems: null,
    filmstripItemsCol: null,
    filmstripItemWidth: 100,
    scrollItemAmount: 5,
    
    initialize: function(filmstrip){
		filmstrip = $(filmstrip);
		this.filmstripPrevious = filmstrip.getElement('img.filmstripPrevious');
		this.filmstripNext = filmstrip.getElement('img.filmstripNext');
		this.filmstripItems = filmstrip.getElement('ul');
		this.filmstripItemsCol = filmstrip.getElements('li');
		if (this.filmstripItemsCol.length >= 2) {
			this.filmstripItemWidth = (this.filmstripItemsCol[1].offsetLeft + this.filmstripItemsCol[1].clientLeft)  - (this.filmstripItemsCol[0].offsetLeft + this.filmstripItemsCol[0].clientLeft);
			this.scrollItemAmount = Math.floor(this.filmstripItems.parentNode.clientWidth / this.filmstripItemWidth);
		}
    },
    
    setScrollControls: function(updateCollection){
		if (updateCollection) this.filmstripItemsCol = this.filmstripItems.getElements('li');
		this.filmstripPrevious.removeClass('disabled');
		this.filmstripNext.removeClass('disabled');

		if (this.filmstripItemsCol.length <= this.scrollItemAmount)
		{
			if (!this.filmstripPrevious.hasClass('disabled')) this.filmstripPrevious.addClass('disabled');
			if (!this.filmstripNext.hasClass('disabled')) this.filmstripNext.addClass('disabled');
			this.filmstripItems.setStyle('left', 0);
		}
		else
		{	
			if (this.filmstripItems.offsetLeft >= 0)
			{
				if (!this.filmstripPrevious.hasClass('disabled')) this.filmstripPrevious.addClass('disabled');
			}
			else
			{
				if (this.filmstripPrevious.hasClass('disabled')) this.filmstripPrevious.removeClass('disabled');
			}

			if (this.filmstripItems.offsetLeft <= ((this.scrollItemAmount - this.filmstripItemsCol.length) * this.filmstripItemWidth))
			{
				if (!this.filmstripNext.hasClass('disabled')) this.filmstripNext.addClass("disabled");
			}
			else
			{
				if (this.filmstripNext.hasClass('disabled')) this.filmstripNext.removeClass("disabled");
			}
		}
	},
	
	scrollFilmstrip: function(scrollPrevious)
	{
		var scrollAmount = null;

		scrollAmount = this.filmstripItemWidth * this.scrollItemAmount;
		if(!scrollPrevious) scrollAmount *= -1;
		
		if (scrollAmount != null)
		{
			this.filmstripItems.setStyle('left', (this.filmstripItems.offsetLeft + scrollAmount)); 
			this.setScrollControls(false);
		}
	}
});