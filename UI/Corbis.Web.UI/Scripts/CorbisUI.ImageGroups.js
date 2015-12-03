/****************************************************
    Corbis UI Image Groups
***************************************************/


CorbisUI.ImageGroups = {
    vars: {
        parentElement: null,
        ele: null,
        imageEle: null,
        products: null,
        icon: null,
        capContainer: null,
        prodBlock: null,
        emptydiv: null,
        productResults: null
    },

    initialize: function() {
        this.vars.parentElement = $('SearchColumnedContent');
        this.vars.ele = this.vars.parentElement.getElement('.hideCaptionWrapper');
        this.vars.imageEle = this.vars.parentElement.getElement('.captionBtnHide');
        this.vars.products = $('ProductResults');
        this.vars.icon = $('captionIconHideDiv');
        this.vars.capContainer = $('captionContainer');
        this.vars.prodBlock = $('productBlock');
        this.vars.emptydiv = $('emptyDiv');
    },

    addToCart: function(corbisId) {
        this.initialize();

        if (CorbisUI.Auth.GetSignInLevel() < CorbisUI.Auth.SignInLevels.AutoLoggedIn) {
            CorbisUI.Auth.Check(CorbisUI.Auth.SignInLevels.AutoLoggedIn, CorbisUI.Auth.ActionTypes.Execute, "CorbisUI.ImageGroups.addToCart('" + corbisId + "')");
        } else {
            /*
            var product = this.vars.products.getElement('span[corbisid=' + corbisId + ']');
            if (product) {
            (new CorbisUI.SearchModels.ProductBlock(product)).addProductToCart();
            }
            */
            CorbisUI.Handlers.Cart.addProductToCart(corbisId);
        }
    },

    toggleCaption: function() {
        this.initialize();

        if (this.vars.ele.getStyle('visibility') == 'visible' || this.vars.ele.getStyle('display') == 'block') {

            $('HideCaptionBody').fade('out');
            var myEffect = new Fx.Morph(this.vars.ele, { duration: 'short', transition: Fx.Transitions.Sine.easeOut });

            myEffect.start({
                'height': [505, 1],
                'visibility': 'hidden' //Morphs the 'height' style from 505px to 0px.
            });

            this.vars.icon.setProperty('class', 'captionIconShow');
            this.vars.imageEle.getElement('.labelDivShow').removeClass('displayNone');
            this.vars.imageEle.getElement('.labelDivHide').addClass('displayNone');
            this.vars.ele.setStyles({ 'float': 'right', 'display': 'inline-block' });
            this.vars.products.getElement('.hideCaptionWrapper').addClass('displayNone');
            //this.vars.capContainer.setStyles({ marginTop: '0px', marginBottom: '0px' });
            if (this.vars.emptydiv != null) {
                this.vars.emptydiv.setStyle('height', 30);
            }
            this.vars.products.setStyles({
                'float': 'left',
                'marginTop': 5,
                'minHeight': 450,
                'fontSize': 10,
                'fontWeight': 'bold',
                'marginRight': 0,
                'display': 'inline-block'
            });
            if (this.vars.prodBlock != null) {
                this.vars.products.getElement('.productBlock').setStyle('padding', '3px');
            }

        }


        if ($('HideCaptionBody').getStyle('visibility') == 'hidden') {

            $('HideCaptionBody').fade('in');
            var myEffect = new Fx.Morph(this.vars.ele, { duration: 'short', transition: Fx.Transitions.Sine.easeIn });

            myEffect.start({
                'height': [505], //Morphs the 'height' style from the current to  505px.
                'visibility': 'visible'
            });

            this.vars.icon.setProperty('class', 'captionIconHide');
            this.vars.imageEle.getElement('.labelDivHide').removeClass('displayNone');
            this.vars.imageEle.getElement('.labelDivShow').addClass('displayNone');
            this.vars.products.getElement('.hideCaptionWrapper').removeClass('displayNone');
            //this.vars.capContainer.setStyles({ marginTop: '0px', marginBottom: '0px' });
            if (this.vars.emptydiv != null) {
                this.vars.emptydiv.setStyle('height', 30);
            }
            this.vars.products.setStyles({
                'float': 'left',
                'marginTop': 5,
                'minHeight': 450,
                'fontSize': 10,
                'fontWeight': 'bold',
                'marginRight': 140
            });
            if (this.vars.prodBlock != null) {
                this.vars.products.getElement('.productBlock').setStyle('padding', '20px 5px 0 0');
            }
        }
    }
}

var captionMover = new Class({
    defaultTop: null,
    initialize: function() {
        window.addEvent('resize', this.resizingEvent.bindWithEvent(this));
        window.addEvent('domready', this.resizingEvent.bindWithEvent(this));
    },

    resizingEvent: function() {
        var productBlock = $('ProductResults');
        var productBlockCoord = productBlock.getCoordinates();
        if (productBlock.getElement('.hideCaptionWrapper')) {
            var CD = productBlock.getElement('.hideCaptionWrapper');
            var CDc = CD.getCoordinates();

            var rightmargin = productBlockCoord.width % 189;
            //console.log(rightmargin);
            if (Browser.Engine.trident) {
                if (rightmargin > 168) {
                    // make an adjustment when the rightmargin calculation was between 180 and 189. why?? idunno
                    CD.setStyle('margin-right', rightmargin - 171 + 11);
                }
                else {
                    CD.setStyle('margin-right', rightmargin + 27);
                }
            }
            else {
                if (rightmargin > 180) {
                    // make an adjustment when the rightmargin calculation was between 180 and 189. why?? idunno
                    CD.setStyle('margin-right', rightmargin - 171);
                }
                else {
                    CD.setStyle('margin-right', rightmargin + 16);
                }
            }
        }
    }
});


