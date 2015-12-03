function radioClicked(parent)
{
    parent =  $(parent);
    var rb = parent.getElement('input[type=radio]');
    $('moreSearchOptions').getElements('input[name=' + rb.getProperty('name') + ']').each(function(r){
                r.checked = false;
                r.getParent().getParent().getElement('img').setProperty('src', '/images/radio_off.png');
            });
    var img = parent.getElement('img');
    rb.checked = true;
    img.setProperty('src', '/images/radio_on.png');
}

function setRadioNoEvent(parent, checked)
{
    parent = $(parent);
    var rb = parent.getElement('input');
    var img = parent.getElement('img');
    rb.checked = checked;
    img.setProperty('src', checked ? '/images/radio_on.png' : '/images/radio_off.png');
}

function getRadioState(parent)
{
    parent = $(parent);
    var rb = parent.getElement('input');
    return rb.checked;
}