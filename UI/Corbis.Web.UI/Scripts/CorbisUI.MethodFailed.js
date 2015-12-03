/****************************************************
    Corbis UI Method failed
***************************************************/

/* GENERIC METHOD FAILED FUNCTION 
*
* This arose out of a need to see more
* error details. You can use this or your
* custom methodFailed function.
*
* Example of usage:
 
function AddToCart(corbisId) {
var cartItem = $(parent.document).getElement('span.corbisID[text=' + corbisId + ']').getParent().getParent().retrieve('objectReference');
        
// args to pass is optional. This is additional info to what the service failure provides.
// args within methodArgs have to be preceeded with underscore '_'
var argsToPass = {
methodArgs: {
_script: 'Cart.js',
_method: 'CorbisUI.Cart.Handler.moveItemToPriced',
_corbisId: corbisId
}
};
// if not using argsToPass, then don't need next line
CorbisUI.MethodFailed.bind(argsToPass);
Corbis.Web.UI.Checkout.CartScriptService.GetPricedPricingDisplay(cartItem.productUID, CorbisUI.Cart.Handler.GetPricedPricingDisplayCallback, CorbisUI.MethodFailed, cartItem);
}
    
*
**********************************************************/

CorbisUI.MethodFailed = function(results, context, methodName) {
    if (Browser.getHost().contains('.pre') && CorbisUI.debug) {

        //console.log(context.toSource());
        //console.log(this.methodArgs);

        var BGColor1 = '#eeeeee';
        var BGColor2 = '#fafafa';
        var FONTcolor1 = 'brown';
        var FONTcolor2 = 'green';
        var alternateBG;
        var alternateFont;

        var title = (self.name == '') ? 'METHOD FAILED: Console' : 'METHOD FAILED: Console_' + self.name;

        var _debug_console = window.open("", "METHOD FAILED", "width=680,height=600,resizable,scrollbars=yes");

        _debug_console.document.write("<HTML><TITLE>" + title + "</TITLE><BODY bgcolor=#ffffff>");

        _debug_console.document.write("<table border=0 width=100%>");
        _debug_console.document.write("<tr bgcolor=#cccccc><th colspan=2>MONARCH Method Failure Debug Console</th></tr>");
        _debug_console.document.write("<tr bgcolor=#cccccc><td colspan=2><b>Method Name:</b></td></tr>");
        _debug_console.document.write("<tr bgcolor=" + BGColor1 + "><td colspan=2><font color='" + FONTcolor1 + "'>" + methodName + "</font></td></tr>");

        if (this.methodArgs) {
            _debug_console.document.write("<tr bgcolor=#cccccc><td colspan=2><b>Method Args:</b></td></tr>");

            for (name in this.methodArgs) {

                if (name != '_stackTrace' && name.substr(0, 1) == "_") {
                    alternateBG = (alternateBG == BGColor1) ? BGColor2 : BGColor1;
                    alternateFont = (alternateFont == FONTcolor1) ? FONTcolor2 : FONTcolor1;
                    _debug_console.document.write("<tr bgcolor='" + alternateBG + "'>");
                    _debug_console.document.write("<td align='right' width='15%' style='color:" + alternateFont + ";'>");
                    _debug_console.document.write("<b style=\"font-weight: bold;\">" + name + "</b>");
                    _debug_console.document.write("</td>");
                    _debug_console.document.write("<td align='left' style='padding-left: 10px; color:" + alternateFont + ";'>");
                    _debug_console.document.write(this.methodArgs[name]);
                    _debug_console.document.write("</td>");
                    _debug_console.document.write("</tr>");
                }
            }
            alternateBG = BGColor1;
        }

        _debug_console.document.write("<tr bgcolor=#cccccc><td colspan=2><b>Result Data:</b></td></tr>");

        for (name in results) {

            if (name != '_stackTrace' && name.substr(0, 1) == "_") {
                alternateBG = (alternateBG == BGColor1) ? BGColor2 : BGColor1;
                alternateFont = (alternateFont == FONTcolor1) ? FONTcolor2 : FONTcolor1;
                _debug_console.document.write("<tr bgcolor='" + alternateBG + "'>");
                _debug_console.document.write("<td align='right' width='15%' style='color:" + alternateFont + ";'>");
                _debug_console.document.write("<b style=\"font-weight: bold;\">" + name + "</b>");
                _debug_console.document.write("</td>");
                _debug_console.document.write("<td align='left' style='padding-left: 10px; color:" + alternateFont + ";'>");
                _debug_console.document.write(results[name]);
                _debug_console.document.write("</td>");
                _debug_console.document.write("</tr>");
            }
        }

        if (results._stackTrace) {
            _debug_console.document.write("<tr bgcolor=#cccccc><th colspan=2>STACKTRACE</th></tr>");
            _debug_console.document.write("<tr bgcolor=#cccccc><td colspan=2><textarea style='width: 100%; height: 350px;'>" + results._stackTrace + "</textarea></td></tr>");
        }

        _debug_console.document.write("</table>");
        _debug_console.document.write("</BODY></HTML>");
        _debug_console.document.close();
    } else {
        alert('ERROR [' + methodName + '] : ' + results._message);
    }
}
// alias methodFailed to CorbisUI.MethodFailed
methodFailed = CorbisUI.MethodFailed;

