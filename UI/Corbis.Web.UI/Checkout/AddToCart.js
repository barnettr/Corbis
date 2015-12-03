/******************************
    CORBIS ADD TO CART
*******************************/

/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
	CorbisUI = {};
}
CorbisUI.Cart = {};
var addToCartVar = null;
var addingRFCD = false;
CorbisUI.Cart.AddToCartInst = function() {
    if (!addToCartVar) {
        addToCartVar = new CorbisUI.Cart.AddToCart();
    }
    return addToCartVar;
}
CorbisUI.Cart.AddToCart = new Class({
    context: null,
    offeringUid: null,
    onSuccess: $empty,

    initialize: function(offeringUid) {
        this.offeringUid = offeringUid;
        Corbis.Web.UI.Checkout.CartScriptService.set_defaultUserContext(this);
    },
    addRFCDToCart: function(el) {
        
        if (!CorbisUI.Pricing.IsAuthenticated(el.id)) {
            return false;
        }
        addingRFCD = true;
        return true;
    },
    addRFCDToLightbox: function() {
        addingRFCD = false;
    },
    addOfferingToCart: function() {
        addingRFCD = false;
        LogOmnitureEvent("scAdd");
        Corbis.Web.UI.Checkout.CartScriptService.AddOfferingToCart(this.offeringUid, this.addOfferingToCartSuccess, this.methodFailed);
    },

    addProductToCart: function(corbisId, productUid) {
        addingRFCD = false;
        LogOmnitureEvent("scAdd");
        Corbis.Web.UI.Checkout.CartScriptService.AddProductToCart(corbisId, productUid, this.addOfferingToCartSuccess, this.methodFailed);
    },
    displaySuccessDialog: function(context) {
        var options = {
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            positionVert: 'top',
            positionHoriz: -250
        };

        if (context && $type(context.CT) == 'element') {
            options.centerOverElement = context.CT;
        }

        new CorbisUI.Popup('addToCartConfirm', options);
        
        $('addToCartConfirmWindow').getElement('input.Close').setStyle('visibility', 'visible');
    },
    addOfferingToCartSuccess: function(results, context, methodName) {
        if (results > 0) {
            context.onSuccess(results);
            //CorbisUI.Cart.AddToCartInst().displaySuccessDialog(context.context); for bug 15191
        }
        else {
            //TODO: Open login popup.
        }
    },

    methodFailed: function(results, context, methodName) {
        //console.log(results);
        //TODO: use mochaUI for alert window
        alert(results.get_message());
    }
});    

