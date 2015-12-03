/****************************
NAMESPACES
****************************/
if (typeof (CorbisUI) == 'undefined') {
    CorbisUI = {};
}

/////////////////////////////////////
// CONSTRUCTORS
// generic templates incorporated
// by other objects with Implement
/////////////////////////////////////
CorbisUI.SearchConstructors = {

    licenseMapping: ['Unknown', 'RM', 'RF', 'RS'],

    /* block constructor for lightbox and quickpic item */
    pinkyBlockConstructor: new Class({

        closeButton: null,
        cartButton: false,
        quickCheckoutButton: false,

        mediaUID: null,
        corbisID: null,
        realProductUID: null,
        lightboxID: 0,
        licenseModel: null,

        isOutline: false,
        isRfcd: false,

        injectWhere: null,

        block: null,

        isInCart: function() {
            return CorbisUI.cartMediaUidList.has(this.mediaUID);
        },

        highlightIcon: function(iconType) {
            if (iconType == 'cart') this.block.addClass('inCart');
        },

        createBlock: function(newItem, caller) {

            var objRef, imageRef, imageClone, title, coords, clickContent, clickLink;

            var coordsBase = {
                marginTop: 90,  //basic dimensions of container
                width: 90,      // minimum width
                height: 90      // minimum height
            };

            objRef = (newItem) ? this.tempObject : CorbisUI.ProductCache.get(this.mediaUID);

            //console.log('TEMPOBJECT');
            //console.log(this.tempObject);
            this.isOutline = (newItem) ? this.tempObject.IsOutLine : CorbisUI.ProductCache.get(this.mediaUID).isOutline;
            this.isRfcd = (newItem) ? this.tempObject.IsRfcd : CorbisUI.ProductCache.get(this.mediaUID).isRfcd;
            //this.isOutline = this.tempObject.IsOutline;
            //console.log('ISOUTLINE: ' + this.isOutline);

            if (!newItem) objRef.setupObject();

            this.injectWhere = (newItem) ? 'bottom' : 'top';

            var objRefLicenseModal = (newItem) ? CorbisUI.SearchConstructors.licenseMapping[objRef.LicenseModel] : objRef.licenseModel;
            this.licenseModel = Corbis.CommonSchema.Contracts.V1.LicenseModel.toLocalizedString(Corbis.CommonSchema.Contracts.V1.LicenseModel[objRefLicenseModal]);
            //this.licenseModel = (newItem) ? CorbisUI.SearchConstructors.licenseMapping[objRef.LicenseModel] : objRef.licenseModel;

            var Wrap = new Element('div', {
                'class': this.constructorType + 'Block',
                'corbisid': this.corbisID
            });

            var closeWrap = new Element('div')
                                .addClass('hoverBtn')
                                .addClass('closeIcon')
                                .inject(Wrap);

            var closeInput = new Element('input').setProperties({
                'class': 'hovable',
                'value': '',
                'title': CorbisUI.GlobalVars.SearchResults.text.deleteBtnAlt,
                'alt': CorbisUI.GlobalVars.SearchResults.text.deleteBtnAlt
            }).addEvent('click', this.deleteButtonEvent.bindWithEvent(this))
                .inject(closeWrap);

            this.closeButton = closeInput;

            //note: if newitem is null, IsRfcd also available?
            if (objRef.IsRfcd == true) {
                clickContent = '../imagegroups/imagegroups.aspx?typ=' + Corbis.Image.Contracts.V1.ImageMediaSetType.RFCD + '&id=' + objRef.CorbisId;
                clickLink = function() { window.location.href = clickContent; return false; };
            }
            else {
                if (newItem) {
                    clickContent = '../Enlargement/Enlargement.aspx?id=' + objRef.CorbisId + '&puid=' + objRef.ProductUid + '&caller=' + caller;
                    clickLink = function() { EnlargeImagePopUp(clickContent); return false; };
                }
            }
            var thumbBlock = new Element('div').setProperties({
                'class': 'thumbWrap'
            }).setStyles({
                'cursor': 'pointer',
                'background': '#262626'
            });

            if (clickLink) {
                thumbBlock.addEvent('click', clickLink);
            } else {
                if (Browser.Engine.trident) {
                    clickContent = '../Enlargement/Enlargement.aspx?id=' + this.corbisID + '&puid=' + this.mediaUID + '&caller=' + caller;
                    clickLink = function() { EnlargeImagePopUp(clickContent); return false; };
                    thumbBlock.addEvent('click', clickLink);
                }
            }
            thumbBlock.inject(Wrap);

            if (!newItem) {
                imageRef = objRef.thumbWrap.getFirst();
                imageClone = imageRef.clone();
                coords = CorbisUI.SearchConstructors.helpers.ScaleImage2(objRef.thumbWrap.getFirst(), coordsBase);
                imageClone.setStyles(coords);

                //need to set the caller for enlargement to lightbox for the lightbox buddy.
                if (imageClone.getProperty('onclick')) imageClone.setProperty('onclick', imageClone.getProperty('onclick').replace(/caller=[^&\']*/i, 'caller=' + caller));

                title = imageRef.getProperty('title');

            } else {

                this.realProductUID = objRef.ProductUid;

                imageClone = new Element('img', { 'src': objRef.Url128 });

                if (objRef.AspectRatio < 1) {
                    coords = {
                        width: 90 * objRef.AspectRatio,
                        height: 90
                    };
                } else {
                    coords = {
                        width: 90,
                        height: 90 / objRef.AspectRatio,
                        marginTop: (90 - (90 / objRef.AspectRatio)) / 2
                    };
                }

                imageClone.setStyles(coords);

                title = objRef.CorbisId + ' - ' + objRef.Title;

            }

            imageClone.setProperties({
                'title': title,
                'alt': title
            });

            imageClone.inject(thumbBlock);

            this.licenseBlock = new Element('div')
                                    .addClass(this.licenseModel + 'color infoBox')
                                    .inject(Wrap);

            var licenseWrap = new Element('div').addClass('license').inject(this.licenseBlock);

            var LT = new Element('span').set('text', this.licenseModel).inject(licenseWrap);

            this.block = Wrap;

            if (newItem) this.tempObject = null;

            //LT.addEvent('click', this.test.bindWithEvent(this));
        }
//        ,

//        test: function(event) {

//            console.log('+=== SB [ ' + this.licenseModel + ' ] ITEM INFORMATION ===================================================+');

//            console.log('     license: ' + this.licenseModel);
//            console.log('     corbisID: ' + this.corbisID);
//            console.log('     mediaUID: ' + this.mediaUID);
//            console.log('     productUID: ' + this.realProductUID);
//            console.log('     lightboxID: ' + this.lightboxID);
//            console.log('     isOutline: ' + this.isOutline);
//            console.log('     isRfcd: ' + this.isRfcd);

//        }

    }),



    helpers: {

        ScaleImage: function(ele, options) {

            var Base = {
                marginTop: 90, //basic dimensions of container
                width: 90, // minimum width
                height: 90 // minimum height
            };
            if (options) Base = $merge(Base, options);

            //if(!margin) margin = 128;

            //console.log(Base);

            var OrigCoords = ele.getCoordinates();
            var Coords = { width: 0, height: 0 };
            Coords.width = OrigCoords.width;
            Coords.height = OrigCoords.height;

            var newValues = Base;

            for (var n in Coords) {
                var m = Base[n];
                if (Coords[n] > m && Base[n]) {
                    var o = (n == 'width') ? 'height' : 'width';
                    var r = m / Coords[n];
                    newValues[n] = m;
                    newValues[o] = Math.ceil(Base[o] * r);
                }
            }

            var wrap, img, m;



            if (newValues.height <= Base.marginTop) {
                wrap = (Base.marginTop / 2); //half of wrapper
                img = (newValues.height / 2); //half of image
                newValues.marginTop = (wrap - img);
            }

            if (Coords.height < options.marginTop) {
                newValues.width = 90;
            }

            //console.log('NEW VALUES');
            //console.log(newValues.toSource());

            return newValues;
        },

        ScaleImage2: function(ele, options) {
            var Base = {
                marginTop: 90, //basic dimensions of container
                width: 90, // minimum width
                height: 90 // minimum height
            };
            if (options) Base = $merge(Base, options);

            //if(!margin) margin = 128;

            //console.log(Base);

            var OrigCoords = ele.getCoordinates();
            var Coords = { width: 0, height: 0 };
            Coords.width = OrigCoords.width;
            Coords.height = OrigCoords.height;

            var newValues = Base;

            if (Coords.height > Coords.width) {
                newValues.width = newValues.width * (Coords.width / Coords.height);
            }
            else {
                newValues.height = newValues.height * (Coords.height / Coords.width);
            }

            var wrap, img, m;

            if (newValues.height <= Base.marginTop) {
                wrap = (Base.marginTop / 2); //half of wrapper
                img = (newValues.height / 2); //half of image
                newValues.marginTop = (wrap - img);
            }

            if (Coords.height < options.marginTop) {
                newValues.width = 90;
            }

            newValues.width = newValues.width.round();
            newValues.height = newValues.height.round();
            newValues.marginTop = newValues.marginTop.round();

            return newValues;
        }

    }
};
