function setGlassButtonDisabled(btn, disable) {
    btn = $(btn);
    //console.log("glassbutton: " + btn.id + " disabled=" + disable);
	var center = btn.getElement('input');
	var right = btn.getElement('span');
	btn.disabled = disable;
   	center.disabled = disable;
	right.disabled = disable;
	var tip = btn.getProperty('title');

	if (disable) {

	    if (tip != null) {
	        glassButtonTooltipFix(btn, tip, true); 
	    }
		btn.addClass('DisabledGlassButton');
   		center.addClass('DisabledGlassButton');
   		right.addClass('DisabledGlassButton');
	}
	else {

	   if (tip != null) {
	        glassButtonTooltipFix(btn, tip, false);
	    }
		btn.removeClass('DisabledGlassButton');
        center.removeClass('DisabledGlassButton');
        right.removeClass('DisabledGlassButton');
        if (btn.id.contains('glassBtnId')) {
            if (btn.getParent() && btn.getParent().getElement('img'))
                btn.getParent().getElement('img').addClass('hdn');
        }
    }
}
function setOutlineButtonDisabled(btn, disable)
{
	btn = $(btn);
	var center = btn.getElement('span .Center');
	var centerLink = btn.getElement('span .Center a');
	var right = btn.getElement('span');
	btn.disabled = disable;
   	center.disabled = disable;
   	centerLink.disabled = disable;
	right.disabled = disable;
	var tip = btn.getProperty('title');

	if (disable) {

		btn.addClass('DisabledGlassButton');
		centerLink.addClass('DisabledGlassButton');
   		center.addClass('DisabledGlassButton');    		
		right.addClass('DisabledGlassButton');
	}
	else {

		btn.removeClass('DisabledGlassButton');
        center.removeClass('DisabledGlassButton');
        centerLink.removeClass('DisabledGlassButton');
		right.removeClass('DisabledGlassButton');
	}
}

function passClickToChild(btn)
{
    btn = $(btn);
	var submit = btn.getElement('input');
	try
	{
	    submit.onclick;
	    //submit.click();
	}
	catch(er)
	{
		submit.click();
    }

}

function glassButtonTooltipFix(el, tip, showtip) {

    if (showtip) {
        el.addClass('relative');
        if (!el.getElement('div.disabledButtonTip')) {
            new Element('div')
                .set('text', tip)
                .addClass('disabledButtonTip')
                .setStyles({
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    right: 0,
                    bottom: 0,
                    background: 'black',
                    opacity: 0.01
                })
                .inject(el);
        }
    } else {
        if (el.getElement('div.disabledButtonTip'))
            el.getElement('div.disabledButtonTip').destroy();
    }

}

// -1: disable forever
// 0: do not disable it
function setGlassButtonDisabledWithinPeriod(btn, delayReset) {
    //display progress
    btn = $(btn).getParent().getParent();
    if (btn.getParent().getElement('img') && btn.id.contains('glassBtnId'))
        btn.getParent().getElement('img').removeClass('hdn');
    if (delayReset == 0)
        return;
    (function() { setGlassButtonDisabled(btn, true); }).delay(1);
    if (delayReset == -1) return;
    setTimeout("setGlassButtonDisabled('" + btn.id.toString() + "', false);", delayReset);
}