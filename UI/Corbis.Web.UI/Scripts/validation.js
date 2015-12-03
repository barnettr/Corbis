/*
Class: FormCheck
Performs different tests on forms and indicates errors.
		
Usage:
Works with these types of fields :
- input (text, radio, checkbox)
- textarea
- select
		
You just need to add a specific class to each fields you want to check. 
For example, if you add the class
(code)
validate['required','length[4, -1]','differs[email]','digit']
(end code)
the value's field must be set (required) with a minimum length of four chars (4, -1), 
must differs of the input named email (differs[email]), and must be digit. 
		
You can perform check during the datas entry or on the submit action, shows errors as tips or in a div before or after the field, 
show errors one by one or all together, show a list of all errors at the top of the form, localize error messages, add new regex check, ...
		
The layout is design only with css. Now I added a hack to use transparent png with IE6, so you can use png images in formcheck.css (works only for theme, so the file must be named formcheck.css). It can also works with multiple forms on a single html page.
The class supports now internationalization. To use it, simply specify a new <script> element in your html head, like this : <script type="text/javascript" src="formcheck/lang/fr.js"></script>.

If you add the class
(code)
validate['submit']
(end code)
to an element like an anchor (or anything else), this element will act as a submit button.
		
N.B. : you must load the language script before the formcheck and this method overpass the old way. You can create new languages following existing ones. You can otherwise still specifiy the alerts' strings when you initialize the Class, with options.
If you don't use a language script, the alert will be displayed in english.
	
Test type:
You can perform various test on fields by addind them to the validate class. Be careful to *not use space chars*. Here is the list of them.
			
required 					- The field becomes required. This is a regex, you can change it with class options.
alpha 						- The value is restricted to alphabetic chars. This is a regex, you can change it with class options.
alphanum 					- The value is restricted to alphanumeric characters only. This is a regex, you can change it with class options.
nodigit 					- The field doesn't accept digit chars. This is a regex, you can change it with class options.
digit 						- The value is restricted to digit (no floating point number) chars, you can pass two arguments (f.e. digit[21,65]) to limit the number between them. Use -1 as second argument to not set a maximum.
number 						- The value is restricted to number, including floating point number. This is a regex, you can change it with class options.
email 						- The value is restricted to valid email. This is a regex, you can change it with class options.
phone 						- The value is restricted to phone chars. This is a regex, you can change it with class options.
url: 						- The value is restricted to url. This is a regex, you can change it with class options.
confirm 					- The value has to be the same as the one passed in argument. f.e. confirm[password].
differs 					- The value has to be diferent as the one passed in argument. f.e. differs[user].
length 						- The value length is restricted by argument (f.e. length[6,10]). Use -1 as second argument to not set a maximum.
		
Parameters:
When you initialize the class with addEvent, you can set some options. If you want to modify regex, you must do it in a hash, like for display or alert. You can also add new regex check method by adding the regex and an alert with the same name.
		
Required:
			
form_id - The id of the formular. This is required.
			
Optional:
			
submitByAjax 			- you can set this to true if you want to submit your form with ajax. You should use provided events to handle the ajax request (see below). By default it is false.
ajaxResponseDiv 		- id of element to inject ajax response into (can also use onAjaxSuccess). By default it is false.
ajaxEvalScripts 		- use evalScripts in the Request response. Can be true or false, by default it is false.
onAjaxRequest 			- Function to fire when the Request event starts.
onAjaxSuccess 			- Function to fire when the Request receives .  Args: response [the request response] - see Mootools docs for Request.onSuccess.
onAjaxFailure 			- Function to fire if the Request fails.
			
tipsClass 				- The class to apply to tipboxes' errors. By default it is 'fc-tbx'.
errorClass 				- The class to apply to alertbox (not tips). By default it is 'fc-error'.
fieldErrorClass 		- The class to apply to fields with errors, except for radios. You should also turn on  options.addClassErrorToField. By default it is 'fc-field-error'
		
Display:
This is a hash of display settings. in here you can modify.
			
showErrors 				- 0 : onSubmit, 1 : onSubmit & onBlur, by default it is 1.
errorsLocation 			- 1 : tips, 2 : before, 3 : after, by default it is 1.
indicateErrors 			- 0 : none, 1 : one by one, 2 : all, by default it is 1.
keepFocusOnError 		- 0 : normal behaviour, 1 : the current field keep the focus as it remain errors. By default it is 0.
checkValueIfEmpty 		- 0 : When you leave a field and you have set the showErrors option to 1, the value is tested only if a value has been set. 1 : The value is tested  in any case.  By default it is 1.
addClassErrorToField 	- 0 : no class is added to the field, 1 : the options.fieldErrorClass is added to the field with an error (except for radio). By default it is 0.

fixPngForIe 			- 0 : do nothing, 1 : fix png alpha for IE6 in formcheck.css. By default it is 1.
replaceTipsEffect 		- 0 : No effect on tips replace when we resize the broswer, 1: tween transition on browser resize;
closeTipsButton 		- 0 : the close button of the tipbox is hidden, 1 : the close button of the tipbox is visible. By default it is 1.
flashTips 				- 0 : normal behaviour, 1 : the tipbox "flash" (disappear and reappear) if errors remain when the form is submitted. By default it is 0.
tipsPosition 			- 'right' : the tips box is placed on the right part of the field, 'left' to place it on the left part. By default it is 'right'.
tipsOffsetX 			- Horizontal position of the tips box (margin-left), , by default it is 100 (px).
tipsOffsetY				- Vertical position of the tips box (margin-bottom), , by default it is -10 (px).
			
listErrorsAtTop 		- List all errors at the top of the form, , by default it is false.
scrollToFirst 			- Smooth scroll the page to first error and focus on it, by default it is true.
fadeDuration 			- Transition duration (in ms), by default it is 300.
		
Alerts:
This is a hash of alerts settings. in here you can modify strings to localize or wathever else. %0 and %1 represent the argument.
			
required 				- "This field is required."
alpha 					- "This field accepts alphabetic characters only."
alphanum 				- "This field accepts alphanumeric characters only."
nodigit 				- "No digits are accepted."
digit 					- "Please enter a valid integer."
digitmin 				- "The number must be at least %0"
digitltd 				- "The value must be between %0 and %1"
number 					- "Please enter a valid number."
email 					- "Please enter a valid email: <br /><span>E.g. yourname@domain.com</span>"
phone 					- "Please enter a valid phone."
url 					- "Please enter a valid url: <br /><span>E.g. http://www.domain.com</span>"
confirm 				- "This field is different from %0"
differs 				- "This value must be different of %0"
length_str 				- "The length is incorrect, it must be between %0 and %1"
length_fix 				- "The length is incorrect, it must be exactly %0 characters"
lengthmax 				- "The length is incorrect, it must be at max %0"
lengthmin 				- "The length is incorrect, it must be at least %0"
checkbox 				- "Please check the box"
radios 					- "Please select a radio"
select 					- "Please choose a value"
		
Example:
You can initialize a formcheck (no scroll, custom classes and alert) by adding for example this in your html head this code :
		
(code)
<script type="text/javascript">
window.addEvent('domready', function() {
var myCheck = new CorbisFormValidator('form_id', {
tipsClass : 'tips_box',
display : {
scrollToFirst : false
},
alerts : {
required : 'This field is ablolutely required! Please enter a value'
}
})
});
</script>
(end code)
	
About:
formcheck.js v.1.4rc5 for mootools v1.2 - 09 / 2008
		
by Floor SA (http://www.floor.ch) MIT-style license
		
Created by Luca Pillonel, last modified by Luca Pillonel
		
Credits:
This class was inspired by fValidator by Fabio Zendhi Nagao (http://zend.lojcomm.com.br)	
*/


// TODO: make this part of the CorbisUI namespace. On request price it throws errors though.
var CorbisFormValidator = new Class({

    Implements: [Options, Events],
    container: false,
    errorDiv: false,
    errorTarget: false,
    options: {
        tipsClass: 'displayNone',
        fieldErrorClass: 'Error',
        fieldErrorClass: 'fc-field-error', //error class for elements
        submitForm: false, // button to click on submit
        containerID: 'ModalPopupContent',
        submitByAjax: false, 			//false : standard submit way, true : submit by ajax
        ajaxResponseDiv: false, 		//element to inject ajax response into (can also use onAjaxSuccess) [cronix]
        ajaxEvalScripts: false, 		//use evalScripts in the Request response [cronix]
        ajaxUrl: false,
        ajaxAsync: false,
        ajaxData: null,
        onAjaxRequest: $empty, 			//Function to fire when the Request event starts 
        onAjaxSuccess: $empty, 			//Function to fire when the Request receives .  Args: response [the request response] - see Mootools docs for Request.onSuccess
        onAjaxFailure: $empty, 			//Function to fire if the Request fails
        useStandardAjaxBehavior: false, // useStandardAjaxBehavior makes it so that we return "success" if the operation completed
        // and this script will auto-close. Otherwise, it assumes the response is a localized error
        // string that it will display in the error section
        successScript: false, // script string that closes the modal.
        failScript: false,
        resizeScript: false,
        useSubmitBehavior: false,
        highlightField: false,

        display: {
            indicateErrors: 2,
            listErrorsAtTop: true,
            scrollToFirst: false,
            addClassErrorToField: 2,
            showErrors: 1,
            errorsLocation: 1,
            keepFocusOnError: 0,
            checkValueIfEmpty: 1,
            fixPngForIe: 1,
            replaceTipsEffect: 1,
            flashTips: 0,
            closeTipsButton: 1,
            tipsPosition: "right",
            tipsOffsetX: -45,
            tipsOffsetY: 0,
            fadeDuration: 300,
            rowHiliteClass: 'ErrorHighlight'
        },

        alerts: {
            required: "This field is required.",
            alpha: "This field accepts alphabetic characters only.",
            alphanum: "This field accepts alphanumeric characters only.",
            cvv: "Please enter a valid credit card verification Number.",
            nodigit: "No digits are accepted.",
            digit: "Please enter a valid integer.",
            digitltd: "The value must be between %0 and %1",
            number: "Please enter a valid number.",
            email: "Please enter a valid email.",
            phone: "Please enter a valid phone.",
            url: "Please enter a valid url.",

            confirm: "This field is different from %0",
            differs: "This value must be different of %0",
            length_str: "The length is incorrect, it must be between %0 and %1",
            length_fix: "The length is incorrect, it must be exactly %0 characters",
            lengthmax: "The length is incorrect, it must be at max %0",
            lengthmin: "The length is incorrect, it must be at least %0",
            checkbox: "Please check the box",
            radios: "Please select a radio",
            select: "Please choose a value",
            wordcount: "Please enter more than one and up to %0 words"
        },
        regexp: {
            required: /[^.*]/,
            alpha: /^[a-z ._-]+$/i,
            alphanum: /^[a-z0-9 ._-]+$/i,
            cvv: /^\d{3,4}$/,
            digit: /^[-+]?[0-9]+$/,
            nodigit: /^[^0-9]+$/,
            number: /^[-+]?\d*\.?\d+$/,
            email: /^[a-z0-9._%-]+@[a-z0-9.-]+\.[a-z]{2,4}$/i, //\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)* is what our backend uses
            phone: /^[\d\-\.\+\(\)\s]{6,24}$/,
            url: /^(http|https|ftp)\:\/\/[a-z0-9\-\.]+\.[a-z]{2,3}(:[a-z0-9]*)?\/?([a-z0-9\-\._\?\,\'\/\\\+&amp;%\$#\=~])*$/i
        }
    },

    /*
    Constructor: initialize
    Constructor
	
		Add event on formular and perform some stuff, you now, like settings, ...
    */
    initialize: function(form, options) {
        //console.log('CALLING: CorbisFormValidator.initialize');
        if (this.form = $(form)) {
            this.form.isValid = true;
            this.regex = ['length'];
            this.setOptions(options);
            this.container = $(this.options.containerID);
            this.errorDiv = $(this.options.containerID).getElement('div.ValidationSummary');
            this.errorTarget = $(this.options.containerID).getElement('div.ValidationSummary ul');
            //internalization
            if (typeof (formcheckLanguage) != 'undefined') this.options.alerts = formcheckLanguage;

            this.validations = [];
            this.alreadyIndicated = false;
            this.firstError = false;

            var regex = new Hash(this.options.regexp);
            regex.each(function(el, key) {
                this.regex.push(key);
            }, this);
            var elementsToValidate = this.container.getElements("*[validate*=]");

            elementsToValidate.each(function(el) {
                //console.log('reg: ' + el.id + ', ' + this.container.id);
                el.validation = [];
                var validators = el.getProperty("validate").split(';');
                for (var i = 0; i < validators.length; i++) {
                    el.validation.push(validators[i].replace(/\s/g, ''));
                }
                this.register(el);
            }, this);

            if (this.options.useSubmitBehavior)
                this.form.addEvents({
                    "submit": this.validateAll.bind(this)
                });

            if (this.options.display.fixPngForIe) this.fixIeStuffs();
            document.addEvent('mousewheel', function() {
                this.isScrolling = false;
            } .bind(this));


            var enterKey = function(e) {
                e = e || window.event;
                var code = e.keyCode || e.which || e.code;
                if (code == 13) {
                    e.stop();
                    _isEnterKey = true;
                    setTimeout('_isEnterKey = false', 200);
                    this.validateAll(e);
                    e.returnValue = false;
                    return false;
                }
            };
            var enterKeyHandler = enterKey.bindWithEvent(this);
            $(this.options.containerID).getElements('input[type=text]').addEvents({ 'keydown': enterKeyHandler });

            try {
                elementsToValidate[0].focus();
            } catch (Error) {
                return;
            }

        }
    },

    /*
    Function: register
    Private method
		
		Add listener on fields
    */
    register: function(el) {
        //console.log('CALLING: CorbisFormValidator.register');
        this.validations.push(el);
        //console.log(String.format("register: {0}",el));
        el.errors = [];
        if (el.validation[0] == 'submit') {
            el.addEvent('click', function(e) {
                this.validateAll(e);
            } .bind(this));
            return true;
        }

        //errors onblur code
        //        if (!this.isChildType(el)) el.addEvent('blur', function(e) {
        //            if ((el.element || this.options.display.showErrors == 1) && (this.options.display.checkValueIfEmpty || el.value)) this.manageError(el, 'blur');
        //        } .bind(this))
        //We manage errors on radio
        else if (this.isChildType(el)) {
            //We get all radio from the same group and add a blur option
            var nlButtonGroup = this.container.getElements('input[name="' + el.getProperty("name") + '"]');
            nlButtonGroup.each(function(radio) {
                radio.addEvent('blur', function() {
                    if ((el.element || this.options.display.showErrors == 1) && (this.options.display.checkValueIfEmpty || el.value)) this.manageError(el, 'click');
                } .bind(this))
            }, this);
        }
    },

    /*
    Function: validate
    Private method
		
		Dispatch check to other methods
    */
    validate: function(el) {
        //console.log('CALLING: CorbisFormValidator.validate');
        el.errors = [];
        el.isOk = true;
        //On valide l'lment qui n'est pas un radio ni checkbox
        el.validation.each(function(rule) {
            if(!el.isOk)
            {
                return;
            }
            if (this.isChildType(el)) {
                if (!this.validateGroup(el)) {
                    el.isOk = false;
                }
            } else {
                var ruleArgs = [];

                if (rule.match(/^.+\[/)) {
                    var ruleMethod = rule.split('[')[0];
                    ruleArgs = eval(rule.match(/^.+(\[.+\])$/)[1].replace(/([A-Z\.]+)/i, "'$1'"));
                } else var ruleMethod = rule;

                if (this.regex.contains(ruleMethod) && el.get('tag') != "select") {
                    if (!this.validateRegex(el, ruleMethod, ruleArgs)) {
                        el.isOk = false;
                    }
                }
                if (ruleMethod == 'email-m') {
                    if (!this.validateMultipleEmails(el, ruleArgs)) {
                        el.isOk = false;
                    }
                }
                //console.log('-----> ' + ruleMethod);
                if (ruleMethod == 'wordcount' && el.get('tag') == "textarea") {
                    if (!this.validateWordCount(el, ruleArgs)) {
                        el.isOk = false;
                    }
                }
                if (ruleMethod.indexOf('custom') != -1) {
                    if (!this.validateCustom(el, ruleArgs, ruleMethod)) {
                        el.isOk = false;
                    }
                }
                if (ruleMethod == 'confirm') {
                    if (!this.validateConfirm(el, ruleArgs)) {
                        el.isOk = false;
                    }
                }
                if (ruleMethod == 'differs') {
                    if (!this.validateDiffers(el, ruleArgs)) {
                        el.isOk = false;
                    }
                }
                if ((el.get('tag') == "select" && ruleMethod.indexOf('custom') == -1) || (el.type == "checkbox" && ruleMethod == 'required')) {
                    if (!this.simpleValidate(el)) {
                        el.isOk = false;
                    }
                }
            }

        }, this);

        if (el.isOk) return true;
        else return false;
    },

    /*
    Function: simpleValidate
    Private method
		
		Perform simple check for select fields and checkboxes
    */
    simpleValidate: function(el) {
        //console.log('CALLING: CorbisFormValidator.simpleValidate');
        if (el.get('tag') == 'select' && el.selectedIndex != -1 && el.options[el.selectedIndex].text == el.options[0].text) {
            msg = el.getProperty('select_message') ?
                el.getProperty('id') + '|select|' +
                el.getProperty('select_message') :
                this.options.alerts.select;
            el.errors.push(msg);
            return false;
        }
        else if (el.type == "checkbox" && !el.checked) {
            msg = el.getProperty('checkbox_message') ?
                    el.getProperty('id') + '|checkbox|' +
                    el.getProperty('checkbox_message') :
                    this.options.alerts.checkbox;
            el.errors.push(msg);
            return false;
        }
        return true;
    },

    /*
    Function: validateRegex
    Private method
		
		Perform regex validations
    */
    validateRegex: function(el, ruleMethod, ruleArgs) {
        //console.log('CALLING: CorbisFormValidator.validateRegex');
        //console.log('el');
        //console.log(el);
        //console.log('ruleMethod: ' + ruleMethod);
        //console.log('ruleArgs: ' + ruleArgs.toSource());
        var msg = "";
        if (ruleArgs[1] && ruleMethod == 'length') {
            if (ruleArgs[1] == -1) {
                this.options.regexp.length = new RegExp("^[\\s\\S]{" + ruleArgs[0] + ",}$");
                msg = el.getProperty('lengthmin_message') ?
                    el.getProperty('id') + '|lengthmin|' +
                    el.getProperty('lengthmin_message') :
                    this.options.alerts.lengthmin.replace("%0", ruleArgs[0]);
            } else if (ruleArgs[0] == ruleArgs[1]) {
                this.options.regexp.length = new RegExp("^[\\s\\S]{" + ruleArgs[0] + "}$");
                msg = el.getProperty('length_fix_message') ? el.getProperty('length_fix_message') :
                    this.options.alerts.length_fix.replace("%0", ruleArgs[0]);
            } else {
                this.options.regexp.length = new RegExp("^[\\s\\S]{" + ruleArgs[0] + "," + ruleArgs[1] + "}$");
                msg = el.getProperty('length_str_message') ?
                    el.getProperty('id') + '|length_str|' +
                    el.getProperty('length_str_message') :
                    this.options.alerts.length_str.replace("%0", ruleArgs[0]).replace("%1", ruleArgs[1]);
            }
        } else if (ruleArgs[0] && ruleMethod == 'length') {
            this.options.regexp.length = new RegExp("^.{0," + ruleArgs[0] + "}$");
            msg = el.getProperty('lengthmax_message') ? el.getProperty('lengthmax_message') :
                    this.options.alerts.lengthmax.replace("%0", ruleArgs[0]);
        } else {
            msg = el.getProperty(ruleMethod + '_message') ?
                el.getProperty('id') + '|' + ruleMethod + '|' +
                el.getProperty(ruleMethod + '_message') :
            this.options.alerts[ruleMethod];
        }
        if (ruleArgs[1] && ruleMethod == 'digit') {
            var regres = true;
            if (!this.options.regexp.digit.test(el.value)) {
                el.errors.push(this.options.alerts[ruleMethod]);
                regres = false;
            }
            if (ruleArgs[1] == -1) {
                if (el.value >= ruleArgs[0]) var valueres = true; else var valueres = false;
                msg = el.getProperty('digitmin_message') ? el.getProperty('digitmin_message') :
                    this.options.alerts.digitmin.replace("%0", ruleArgs[0]);
            } else {
                if (el.value >= ruleArgs[0] && el.value <= ruleArgs[1]) var valueres = true; else var valueres = false;
                msg = el.getProperty('digitltd_message') ? el.getProperty('digitltd_message') :
                    this.options.alerts.digitltd.replace("%0", ruleArgs[0]).replace("%1", ruleArgs[1]);
            }
            if (!regres || !valueres) {
                el.errors.push(msg);
                return false;
            }
        } else if (!this.options.regexp[ruleMethod].test(el.value)) {
            console.log('tested value' + el.value);
            el.errors.push(msg);
            return false;
        }
        return true;
    },

    validateMultipleEmails: function(el, ruleArgs) {
        //console.log('CALLING: CorbisFormValidator.validateMultipleEmails');
        var val = el.value;
        var msg = el.getProperty('email_message') ?
                el.getProperty('id') + '|email|' +
                el.getProperty('email_message') :
                this.options.alerts['email'];

        // maybe multiples if semicolon
        if (val.contains(';')) {
            //console.log('--[MULTIPLE EMAIL?]');
            var items = val.split(';');

            // trim all values
            items.each(function(item, index) {
                items[index] = item.trim();
            });

            // erase empty items in the array
            // ie, someone enters 'a@c.com;;;;'
            // after erase(), you will have only one item
            items.erase('');

            // if after cleaning up array you have 0 items
            // send required error message
            if (items.length == 0) {
                msg = el.getProperty('required_message') ?
                    el.getProperty('id') + '|email|' +
                    el.getProperty('required_message') :
                    this.options.alerts['required'];
                if (el.errors.length == 0) el.errors.push(msg);
                //console.log(el.errors);
                return false;
            } else {
                var goodToGo = true;
                items.each(function(item, index) {
                    // if one of the values does not pass,
                    // then mark goodToGo as false
                    // so we know to fire off error
                    if (!this.options.regexp['email'].test(item) && goodToGo) {
                        goodToGo = false;
                    }
                }, this);
                if (!goodToGo) {
                    if (el.errors.length == 0) el.errors.push(msg);
                    //console.log(el.errors);
                    return false;
                }
            }
            // single or empty
        } else {
            //console.log('--[SINGLE EMAIL?]');
            val = val.trim();
            if (!this.options.regexp['email'].test(val)) {
                //console.log('-->last');
                if (el.errors.length == 0) el.errors.push(msg);
                //console.log(el.errors);
                return false;
            }
        }

        return true;
    },

    /*
    Function: validateConfirm
    Private method
		
		Perform confirm validations
    */
    validateConfirm: function(el, ruleArgs) {
        //console.log('CALLING: CorbisFormValidator.validateConfirm');
        if (!el.validation.contains('required')) {
            el.validation.push('required');
        }
        var confirm = ruleArgs[0];
        if (el.value != this.form[confirm].value) {
            msg = el.getProperty('confirm_message') ? el.getProperty('confirm_message') :
                this.options.alerts.confirm.replace("%0", ruleArgs[0]);
            el.errors.push(msg);
            return false;
        }
        return true;
    },

    /*
    Function: validateDiffers
    Private method
		
		Perform differs validations
    */
    validateDiffers: function(el, ruleArgs) {
        //console.log('CALLING: CorbisFormValidator.validateDiffers');
        var confirm = ruleArgs[0];
        if (el.value == this.form[confirm].value) {
            msg = el.getProperty('differs_message') ? el.getProperty('differs_message') :
                    this.options.alerts.differs.replace("%0", ruleArgs[0]);
            el.errors.push(msg);
            return false;
        }
        return true;
    },
    /*
    Function: validateWordCount
    Private method
		
		Perform word count validations
    */
    validateWordCount: function(el, ruleArgs) {
        //console.log('CALLING: CorbisFormValidator.validateWordCount');
        var msg = "";

        if (!el.validation.contains('required')) {
            el.validation.push('required');
        }
        var tmpWordcount = ruleArgs[0];
        var elWordcount = el.value.split(/\b[\s,\.-:;]*/).length;


        if (elWordcount < 2) { //The Regex counts a blank space as 1 so I am setting to 2
            msg = el.getProperty('wordcount_message') ? el.getProperty('wordcount_message') :
                    this.options.alerts.wordcount.replace("%0", tmpWordcount);
            el.errors.push(msg);
            return false;
        }
        if (elWordcount > tmpWordcount) {
            msg = this.options.alerts.wordcount.replace("%0", tmpWordcount);
            el.errors.push(msg);
            return false;
        }

        return true;

    },
    /*
    Function: isChildType
    Private method
		
		Determine if the field is a group of radio or not.
    */
    isChildType: function(el) {
        //console.log('CALLING: CorbisFormValidator.isChildType');
        if (el.get('type')) {
            var elType = el.get('type').toLowerCase();
            if ((elType == "radio")) return true;
        }
        return false;
    },

    validateCustom: function(el, ruleArgs, customString) {
        //console.log('CALLING: CorbisFormValidator.validateCustom');
        if (!eval(el.getProperty(customString))) {
            el.errors = [el.getProperty('id') + '|' + customString + '|' + el.getProperty(customString + '_message')];
            //alert(el.getProperty('id') + '|' + customString + '|' + el.getProperty(customString + '_message'));
            return false;
        }
        else return true;
    },

    /*
    Function: validateGroup
    Private method
		
		Perform radios validations
    */
    validateGroup: function(el) {
        //console.log('CALLING: CorbisFormValidator.validateGroup');
        el.errors = [];
        var nlButtonGroup = this.form[el.getProperty("name")];
        el.group = nlButtonGroup;
        var cbCheckeds = false;

        for (var i = 0; i < nlButtonGroup.length; i++) {
            if (nlButtonGroup[i].checked) {
                cbCheckeds = true;
            }
        }
        if (!cbCheckeds) {
            el.errors.push(this.options.alerts.radios);
            return false;
        } else {
            return true;
        }
    },

    /*
    Function: listErrorsAtTop
    Private method
		
		Display errors
    */
    listErrorsAtTop: function(obj) {
        //console.log('CALLING: CorbisFormValidator.listErrorsAtTop');
        if (((obj.validation.contains('required') || obj.validation[0].indexOf('custom') == 0) && obj.errors.length > 0) ||
            (obj.errors.length > 0 && obj.value && !obj.validation.contains('required'))) {
            var erstring = '';
            obj.errors.each(function(error) {
                //console.log(error);
                errorObj = error.split('|');
                //console.log(errorObj[0] + ' : ' + errorObj[1] + ' : ' + errorObj[2]);
                if (erstring != errorObj[2]) {
                    erstring = errorObj[2];

                    var errorItem = this.errorTarget.get('html') + '<li elId=' + errorObj[0] + ' erType=' + errorObj[1] + '>' + errorObj[2] + '</li>';

                    try {
                        if (errorObj[2] && typeof (errorObj[2]) != undefined) {
                            this.errorTarget.set('html', errorItem);
                            if (this.options.display.addClassErrorToField && !this.isChildType(obj)) {
                                this.highlightRow(true, obj);
                            }
                        }
                    } catch (error) { }
                } //new Element('p').set('html', "<span>" + obj.name + " : </span>" + error).injectInside(this.form.element);
            }, this);

            this.errorDiv.removeClass('displayNone');
            if (this.options.resizeScript)
                eval(this.options.resizeScript);
        }

    },

    /*
    Function: manageError
    Private method
		
		Manage display of errors boxes
    */
    manageError: function(el, method) {
        //console.log('CALLING: CorbisFormValidator.manageError');
        var isValid = this.validate(el);
        //console.log('el, isValid, validation:' + (el.getProperty('id')) + ', ' + isValid + ', ' + el.validation);
        //console.log(el.validation.indexOf('custom'));
        this.elimiSpace(el);
        if (
            (!isValid && el.validation.contains('required')) ||
            (!el.validation.contains('required') && el.value && !isValid) ||
            (!isValid && el.validation[0].indexOf('custom') == 0)
            ) {
            if (this.options.display.listErrorsAtTop) {
                this.listErrorsAtTop(el);
                //console.log('list er: ' + el);
            }
            if (this.options.display.indicateErrors == 2 || !this.alreadyIndicated || el.name == this.alreadyIndicated.name) {
                if (!this.firstError) this.firstError = el;
                this.alreadyIndicated = el;
                if (this.options.display.keepFocusOnError && el.name == this.firstError.name) (function() { el.focus() }).delay(20);
                this.addError(el);
                //console.log('false ' + this.options.containerID);
                return false;
            }
        }
        else if ((isValid || (!el.validation.contains('required') && !el.value)) && el.element) {
            this.removeError(el);
            //console.log('true 1 ' + this.options.containerID);
            return true;
        }
        //console.log('true 2 ' + this.options.containerID);
        return true;
    },

    /*
    Function: addError
    Private method
		
		Add error message
    */
    addError: function(obj) {
        //console.log('CALLING: CorbisFormValidator.addError');
        if (!obj.element && this.options.display.indicateErrors != 0) {
            if (this.options.display.errorsLocation == 1) {
                var pos = (this.options.display.tipsPosition == 'left') ? obj.getCoordinates().left : obj.getCoordinates().right;
                var options = {
                    'opacity': 0,
                    'position': 'absolute',
                    'float': 'left',
                    'left': pos + this.options.display.tipsOffsetX,
                    'top': pos + this.options.display.tipsOffsetY
                }
                obj.element = new Element('div', { 'class': this.options.tipsClass, 'styles': options }).injectInside(this.options.containerID);
                this.addPositionEvent(obj);


            } else if (this.options.display.errorsLocation == 2) {
                obj.element = new Element('div', { 'class': this.options.errorClass, 'styles': { 'opacity': 0} }).injectBefore(obj);
            } else if (this.options.display.errorsLocation == 3) {
                obj.element = new Element('div', { 'class': this.options.errorClass, 'styles': { 'opacity': 0} });
                if ($type(obj.group) == 'object' || $type(obj.group) == 'collection')
                    obj.element.injectAfter(obj.group[obj.group.length - 1]);
                else
                    obj.element.injectAfter(obj);
            }
        }
        if (obj.element) {
            obj.element.empty();
            if (this.options.display.errorsLocation == 1) {
                var errors = [];
                obj.errors.each(function(error) {
                    errors.push(new Element('p').set('html', error));
                });
                var tips = this.makeTips(errors).injectInside(obj.element);
                if (this.options.display.closeTipsButton) {
                    tips.getElements('a.close').addEvent('mouseup', function() {
                        this.removeError(obj);
                    } .bind(this));
                }
                if (obj.get('tag') == 'textarea') {

                    obj.element.setStyle('top', this.options.display.tipsOffsetY + tips.getCoordinates().height - 15);
                } else {

                    obj.element.setStyle('top', obj.getCoordinates().top - tips.getCoordinates().height + this.options.display.tipsOffsetY);
                }


            } else {
                obj.errors.each(function(error) {
                    new Element('p').set('html', error).injectInside(obj.element);
                });
            }

            if (!Browser.Engine.trident5 && obj.element.getStyle('opacity') == 0)
                new Fx.Morph(obj.element, { 'duration': this.options.display.fadeDuration }).start({ 'opacity': [1] });
            else
                obj.element.setStyle('opacity', 1);
        }

        //if (this.options.display.addClassErrorToField && !this.isChildType(obj)) {
        //this.elimiSpace(obj);
        //            //            this.highlightRow(true, obj);
        //            //obj.addClass(this.options.fieldErrorClass);
        //}
    },

    /*
    Function: addPositionEvent
		
		Update tips position after a browser resize
    */
    addPositionEvent: function(obj) {
        //console.log('CALLING: CorbisFormValidator.addPositionEvent');
        if (this.options.display.replaceTipsEffect) {
            obj.event = function() {
                new Fx.Morph(obj.element, {
                    'duration': this.options.display.fadeDuration
                }).start({
                    'left': [obj.element.getStyle('left'), obj.getCoordinates().right + this.options.display.tipsOffsetX],
                    'top': [obj.element.getStyle('top'), obj.getCoordinates().top - obj.element.getCoordinates().height + this.options.display.tipsOffsetY]
                });
            } .bind(this);

        } else {
            obj.event = function() {
                obj.element.setStyles({
                    'left': obj.getCoordinates().right + this.options.display.tipsOffsetX,
                    'top': obj.getCoordinates().top - obj.element.getCoordinates().height + this.options.display.tipsOffsetY
                });
            } .bind(this)
        }
        window.addEvent('resize', obj.event);
    },

    /*
    Function: removeError
    Private method
		
		Remove the error display
    */
    removeError: function(obj) {
        //console.log('CALLING: CorbisFormValidator.removeError');
        this.firstError = false;
        this.alreadyIndicated = false;
        obj.errors = [];
        obj.isOK = true;
        window.removeEvent('resize', obj.event);
        if (this.options.display.errorsLocation == 2)
            new Fx.Morph(obj.element, { 'duration': this.options.display.fadeDuration }).start({ 'height': [0] });
        if (!Browser.Engine.trident5) {
            new Fx.Morph(obj.element, {
                'duration': this.options.display.fadeDuration,
                'onComplete': function() {
                    if (obj.element) {
                        obj.element.destroy();
                        obj.element = false;
                    }
                } .bind(this)
            }).start({ 'opacity': [1, 0] });
        } else {
            obj.element.destroy();
            obj.element = false;
        }
        if (this.options.display.addClassErrorToField && !this.isChildType(obj)) {

            this.highlightRow(false, obj);
            this.errorTarget.getElements('li[elId=' + obj.getProperty('id') + ']').each(function(li) {
                li.dispose();

            });
            if (this.errorTarget.getElements('li').length == 0) {
                this.errorDiv.addClass('displayNone');

            }
            if (this.options.resizeScript)
                eval(this.options.resizeScript);
        }
    },

    /*
    Function: focusOnError
    Private method
		
		Create set the focus to the first field with an error if needed
    */
    focusOnError: function(obj) {
        //console.log('CALLING: CorbisFormValidator.focusOnError');
        if (this.options.display.scrollToFirst && !this.alreadyFocused && !this.isScrolling) {
            if (this.alreadyIndicated.element) {
                switch (this.options.display.errorsLocation) {
                    case 1:
                        var dest = obj.element.getCoordinates().top;
                        break;
                    case 2:
                        var dest = obj.element.getCoordinates().top - 30;
                        break;
                    case 3:
                        var dest = obj.getCoordinates().top - 30;
                        break;
                }
                this.isScrolling = true;
            } else if (!this.options.display.indicateErrors) {
                var dest = obj.getCoordinates().top - 30;
            }
            if (window.getScroll.y != dest) {
                new Fx.Scroll(window, {
                    onComplete: function() {
                        this.isScrolling = false;
                        obj.focus();
                    } .bind(this)
                }).start(0, dest);
            } else {
                this.isScrolling = false;
                obj.focus();
            }
            this.alreadyFocused = true;
        }
    },

    /*
    Function: fixIeStuffs
    Private method
		
		Fix png for IE6
    */
    fixIeStuffs: function() {
        //console.log('CALLING: CorbisFormValidator.fixIeStuffs');
        if (Browser.Engine.trident4) {
            //We fix png stuffs
            var rpng = new RegExp('url\\(([\.a-zA-Z0-9_/:-]+\.png)\\)');
            var search = new RegExp('(.+)formcheck\.css');
            for (var i = 0; i < document.styleSheets.length; i++) {
                if (document.styleSheets[i].href.match(/formcheck\.css$/)) {
                    var root = document.styleSheets[i].href.replace(search, '$1');
                    var count = document.styleSheets[i].rules.length;
                    for (var j = 0; j < count; j++) {
                        var cssstyle = document.styleSheets[i].rules[j].style;
                        var bgimage = root + cssstyle.backgroundImage.replace(rpng, '$1');
                        if (bgimage && bgimage.match(/\.png/i)) {
                            var scale = (cssstyle.backgroundRepeat == 'no-repeat') ? 'crop' : 'scale';
                            cssstyle.filter = 'progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, src=\'' + bgimage + '\', sizingMethod=\'' + scale + '\')';
                            cssstyle.backgroundImage = "none";
                        }
                    }
                }
            }
        }
    },

    /*
    Function: makeTips
    Private method
		
		Create tips boxes
    */
    makeTips: function(txt) {
        //console.log('CALLING: CorbisFormValidator.makeTips');
        var table = new Element('table');
        table.cellPadding = '0';
        table.cellSpacing = '0';
        table.border = '0';

        var tbody = new Element('tbody').injectInside(table);
        var tr1 = new Element('tr').injectInside(tbody);
        new Element('td', { 'class': 'tl' }).injectInside(tr1);
        new Element('td', { 'class': 't' }).injectInside(tr1);
        new Element('td', { 'class': 'tr' }).injectInside(tr1);
        var tr2 = new Element('tr').injectInside(tbody);
        new Element('td', { 'class': 'l' }).injectInside(tr2);
        var cont = new Element('td', { 'class': 'c' }).injectInside(tr2);
        var errors = new Element('div', { 'class': 'err' }).injectInside(cont);
        txt.each(function(error) {
            error.injectInside(errors);
        });
        if (this.options.display.closeTipsButton) new Element('a', { 'class': 'close' }).injectInside(cont);
        //	new Element('div', {'style' : "clear:both"}).injectInside(cont);
        new Element('td', { 'class': 'r' }).injectInside(tr2);
        var tr3 = new Element('tr').injectInside(tbody);
        new Element('td', { 'class': 'bl' }).injectInside(tr3);
        new Element('td', { 'class': 'b' }).injectInside(tr3);
        new Element('td', { 'class': 'br' }).injectInside(tr3);
        return table;
    },

    /*
    Function: reinitialize
    Private method		
		
		Reinitialize form before submit check
    */
    reinitialize: function() {
        //console.log('CALLING: CorbisFormValidator.reinitialize');
        this.validations.each(function(el) {
            if (el.element) {
                el.errors = [];
                el.isOK = true;
                if (this.options.display.flashTips == 1) {
                    el.element.destroy();
                    el.element = false;
                }
            }
        }, this);
        if (this.form.element) this.form.element.empty();
        this.alreadyFocused = false;
        this.firstError = false;
        this.alreadyIndicated = false;
        this.form.isValid = true;
        try {
            this.container = $(this.options.containerID);
            this.errorDiv = this.container.getElement('div.ValidationSummary');
            this.errorTarget = this.errorDiv.getElement('ul');
        } catch (Error) {
            //console.log('error binding errordiv/target ' + this.options.containerID);
        }
        this.errorTarget.getElements('li[elId]').each(function(li) {
            li.dispose();
        });
    },

    /*
    Function: reset
    Public method
    
    Use when reopening a form
    */
    reset: function() {
        //console.log('CALLING: CorbisFormValidator.reset');
        this.reinitialize();
        this.errorDiv.addClass('displayNone');
        var hClass = this.options.display.rowHiliteClass;
        $(this.options.containerID).getElements('*.' + hClass).each(function(e) {
            e.removeClass(hClass);
        });
        eval(this.options.resizeScript);
    },
    /*
    Function: submitByAjax
    Private method		
		
		Send the form by ajax, and replace the form with response
    */

    submitByAjax: function() {

        //console.log('CALLING: CorbisFormValidator.submitByAjax');
        this.reset();
        this.fireEvent('ajaxRequest');
        //var contextToBind = this;
        new Request.HTML({
            url: this.options.ajaxUrl,
            method: 'post',
            async: this.options.ajaxAsync,
            data: this.options.ajaxData,
            evalScripts: this.options.ajaxEvalScripts,
            onFailure: function(instance) {
                this.fireEvent('ajaxFailure', instance);
            } .bind(this),

            onSuccess: function(responseTree, responseElements, responseHtml) {
                if (window.location.href.test('SignInInformationRequest.aspx')) {
                    this.fireEvent('ajaxSuccess', responseHtml.toString());
                } else {
                    this.fireEvent('ajaxSuccess', responseTree);

                }

                if (this.options.useStandardAjaxBehavior) {
                    //console.log(responseElements.getElements('ScriptServiceValidationError').length);
                    //var result = responseElements.filter('scriptservicevalidationerror');
                    var errorIndex = responseHtml.indexOf('<ScriptServiceValidationError');

                    //                    var root = '<root>' + responseHtml + '</root>', doc;
                    //                    alert('root=' + root);
                    //                    doc = new DOMParser().parseFromString(root, 'text/xml');
                    //                    alert('doc=' + doc);
                    //                    root = doc.getElementsByTagName('ScriptServiceValidationError')[0];
                    //                    alert('root=' + root);


                    if (errorIndex == -1)
                        eval(this.options.successScript);
                    else {
                        var result;
                        if (Browser.Engine.trident) {
                            result = responseElements.filter('scriptservicevalidationerror');

                        } else {
                            var test = responseHtml.split('?>');
                            test.shift();
                            test = (test.length == 1) ? test[0] : test.join('?>');
                            //alert($type(test));
                            var ser = new CorbisUI.JSSerializer();
                            test = ser.getDom(test);

                            result = $(test).getElements('ScriptServiceValidationError');
                        }


                        result.each(function(item) {
                            if (item.getElement('ClientId')) {
                                var elId = item.getElement('ClientId').get('text');
                                var doHL = item.getElement('HighlightRow').get('text') == 'true';
                                this.highlightRow(doHL, $(elId));
                                if (item.getElement('ShowInSummary').get('text') == 'true') {
                                    var errorItem = this.errorTarget.get('html') +
                                    '<li elId="' + elId + '" erType="ajax">' +
                                    item.getElement('ErrorMessage').get('text') + '</li>';
                                    this.errorTarget.set('html', errorItem);
                                    this.errorDiv.removeClass('displayNone');
                                }
                            }
                        }, this);

                        eval(this.options.resizeScript);
                    }
                }
                // useOldAjaxBehavior makes it so that we return "success" if the operation completed
                // and this script will auto-close. Otherwise, it assumes the response is a localized error
                // string that it will display in the error section
                else if (this.options.useOldAjaxBehavior) {

                    var responseText = responseHtml.toString();
                    responseText = responseText.replace(/(<([^>]+)>)/ig, "");
                    if (responseText.replace(/\s/g, '').toLowerCase() == 'success') {
                        eval(this.options.successScript);
                    }
                    else {
                        this.errorDiv.removeClass('displayNone');
                        this.errorTarget.set('html', '<li elId=wsdl>' + responseText + '</li>');
                        eval(this.options.resizeScript);
                    }
                }
                if (this.options.ajaxResponseDiv) $(this.options.ajaxResponseDiv).set('html', result);
            } .bind(this)
        }).send();
    },


    /*
    Function: validateAll
    Private method		
		
		Perform check on submit action
    */
    validateAll: function(event) {
        //console.log('CALLING: CorbisFormValidator.validateAll');
        this.container.getElements('input[type=text]').each(this.removeBraces, this);
        this.container.getElements('textarea').each(this.removeBraces, this);
        if (event) Event(event).stop();
        this.reset();
        this.errorTarget.set('html', '');
        //this.reinitialize();

        //console.log('val length: ' + this.validations.length);

        this.validations.each(function(el) {
            //console.log('about to manage : ' + el.id);
            if (!this.manageError(el, 'submit')) this.form.isValid = false;
        }, this);
        //console.log('true 2 ' + this.options.containerID);

        if (this.form.isValid) {
            if (this.options.submitByAjax) {

                this.submitByAjax();
            }
            else if (this.options.successScript) {

                eval(this.options.successScript)
            }
            else if (this.options.submitForm) {

                __doPostBack(this.container.getElement('a.ValidateClickLB').id.replace(/_/g, '$'), '');
                //            //console.log(this.container.getElement('a.ValidateClickLB').id.replace(/_/g, '$'), '');
            }
            // set either submitForm or success script
        }
        else {
            this.form.isValid = false;
            this.focusOnError(this.firstError);
            //console.log("run failscript:" + this.options.failScript);
            setTimeout('eval("' + this.options.failScript + '")', 500);
        }
        //console.log(this.container.getElement('a.ValidateClickLB').id);
    },
    highlightRow: function(hilite, element) {
        //console.log('CALLING: CorbisFormValidator.highlightRow');
        try {
            var parentRow;
            var pattern = new Array('tr.FormRow', 'tr.Error', 'div.FormRow', 'div.Error', 'div.dataRowNoBorder');

            element = $(element.get('id'));
            if (this.options.highlightField) {
                if (hilite)
                    element.addClass('ErrorHighlight');
                else
                    element.removeClass('ErrorHighlight');
            }
            else {
                for (var i = 0; i < pattern.length; i++) {
                    //console.log('PAT: ' + pattern[i]);
                    //console.log(element.getParent(pattern[i]));
                    if (element.getParent(pattern[i])) {
                        parentRow = element.getParent(pattern[i]);
                    }
                }
                //console.log(parentRow.get('html'));
                if (parentRow) {
                    if (hilite)
                        parentRow.addClass('ErrorHighlight');
                    else
                        parentRow.removeClass('ErrorHighlight');
                }
            }
        } catch (Error) { }
    },
    elimiSpace: function(element) {
        //console.log('CALLING: CorbisFormValidator.elimiSpace');
        try {
            if ($(element).value) $(element).value = $(element).value.trim();
            //            if (element.getProperty('value').replace(/\s/g, '') == '') {
            //                element.setProperty('value', '');
            //            }
            //
        } catch (Error) {
            //console.log(element);
        }
    },
    removeBraces: function(element) {
        //console.log('CALLING: CorbisFormValidator.removeBraces');
        //element.value = element.value.replace(/>/g, '&gt;').replace(/</g, '&lt;');
        this.elimiSpace(element);
    }

});
var _isEnterKey = false;
