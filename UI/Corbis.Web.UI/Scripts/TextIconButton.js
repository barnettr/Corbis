function setTextIconButtonDisabled(span, disabled)
{
    span = $(span);
    var a = span.getElement('a');
    if (disabled && !span.hasClass('disabled'))
    {
        span.addClass('disabled');
        a.addClass('disabled');
    }
    else if (!disabled && span.hasClass('disabled'))
    {
        span.removeClass('disabled');
        a.removeClass('disabled');
    }
}
function sendClickToAnchor(span)
{
	span = $(span);
	var a = span.getElement('a');
	if (!span.hasClass('disabled')) {
	    var execScript =  a.getProperty('exec');
	    execScript = execScript.replace(/THIS/i, "'" + a.id + "'");
	    eval(execScript);
	}
}