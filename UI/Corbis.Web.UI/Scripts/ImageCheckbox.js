
function toggleCheckImage(checkbox) {
    img = $(checkbox).getParent().getPrevious();
    var parentDiv = img.getParent('div.imageCheckbox');
    if (!parentDiv.hasClass('disabled') && !parentDiv.getProperty('disabled')) {
        img.setProperty('src', checkbox.checked ? '/images/checked.png' : '/images/unchecked.png');
	}
}

function toggleCheckedState(parent)
{
    parent = $(parent);
    if (!parent.hasClass('disabled') && !parent.getProperty('disabled')) {
		var cb = parent.getElement('input');
		cb.checked = !cb.checked;
		cb.onclick();
	}
}

function toggleCheckedStateNoEvent(parent)
{
    parent = $(parent);
    var cb = parent.getElement('input');
    var img = parent.getElement('img');
    cb.checked = !cb.checked;
    img.setProperty('src', cb.checked ? '/images/checked.png' : '/images/unchecked.png');
}

function setCheckedState(parent, checked)
{
    parent = $(parent);
    var cb = parent.getElement('input');
    cb.checked = checked;
    cb.onclick();
}

function setCheckedNoEvent(parent, checked)
{
    parent = $(parent);
    var cb = parent.getElement('input');
    var img = parent.getElement('img');
    cb.checked = checked;
    img.setProperty('src', checked ? '/images/checked.png' : '/images/unchecked.png');
}

function getCheckedState(parent)
{
    parent = $(parent);
    var cb = parent.getElement('input');
    return cb.checked;
}

function getCheckboxValue(parent) {
    return $(parent).getElement('input[type=checkbox]').value;
}

function setCheckboxDisabled()
{
    //tba
}
