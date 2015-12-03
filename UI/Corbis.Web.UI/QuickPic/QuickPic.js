/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.QuickPic = {};

CorbisUI.QuickPic = {
    DeleteDownloadQuickPic: function(mydiv) {
        try {
            var quickPicDiv = $(mydiv).getParent().getParent();
            var quickPicImagesWrap = quickPicDiv.getParent();
            if (quickPicDiv) {
                var nextItem = quickPicDiv.getNext();

                quickPicDiv.dispose();

                //cycle through rest of elements because of display issues in IE7
                while (nextItem) {
                    nextItem.addClass('hdn').removeClass('hdn');
                    nextItem = nextItem.getNext();
                }
            }

            //disable delete for last item
            var quickPicImages = quickPicImagesWrap.getElements('div.DownloadQuickPic');
            if (quickPicImages.length == 1) quickPicImages[0].getElement('div.closeIcon').setStyle('display', 'none');
        }
        catch (Exception) { }
    },

    DownloadQuickPicImages: function() {
        var imageList = new Array();
        var index = 0;

        $$('div.DownloadQuickPic').each(
			function(el) {
			    var image = new Corbis.WebOrders.Contracts.V1.QuickPicOrderImage();
			    var dropDown = el.getElement('select.fileSize')
			    image.FileSize = dropDown.options[dropDown.selectedIndex].value;
			    image.ImageUid = el.getProperty('imageUid');
			    image.CorbisId = el.getProperty('corbisId');
			    imageList[index] = image;
			    index++;
			}
		);

        var serviceCall = Corbis.Web.UI.QuickPic.QuickPicScriptService._staticInstance.DownloadQuickPicImages($('selectImage').getElement('div.projectName input').value, imageList, CorbisUI.QuickPic.DownloadImageSucceeded, CorbisUI.QuickPic.DownloadImageFailed, imageList);
        executer = serviceCall.get_executor();
    },

    DownloadImageSucceeded: function(result, imageList) {

        $(parent.document).getElement('#QuickPicWindow_overlay').setStyle('background-image', 'none');
        var linksConatiner = $('download').getElement('div.downloadLinks');
        linksConatiner.empty();
       
        if (!result) {
            CorbisUI.QuickPic.DownloadImageFailed(null);
            return;
        }

        $('orderNumber').set('text', result.OrderNumber);
        $('projectName').set('text', result.ProjectName);
        $('emailedTo').set('text', result.ConfirmationEmail);

       

        if (!$('selectImage').hasClass('hdn')) $('selectImage').addClass('hdn');
        $('download').removeClass('hdn');

        if (parent.location.pathname.toLowerCase().endsWith('mylightboxes.aspx')) {
            if (result && result.PackagedCorbisIds) {
                for (var i = 0; i < result.PackagedCorbisIds.length; i++) {
                    var corbisId = result.PackagedCorbisIds[i];
                    var mediaId = parent.CorbisUI.ProductCache.get(corbisId);
                    var product = parent.CorbisUI.ProductCache.get(mediaId);

                    if (product) product.updateIcon('QP', 'deselectIcon');
                }
            }
            parent.CorbisUI.Lightbox.Handler.refreshQuickPicBuddy();
        }
        else {

            if (result && result.PackagedCorbisIds) {
                for (var i = 0; i < result.PackagedCorbisIds.length; i++) {
                    var corbisId = result.PackagedCorbisIds[i];
                    var mediaId = parent.CorbisUI.ProductCache.get(corbisId);
                    var product = parent.CorbisUI.ProductCache.get(mediaId);

                    if (product) product.updateIcon('QP', 'deselectIcon');
                }
            }
            parent.CorbisUI.Search.Handler.refreshQuickPicBuddy();
        }

        result.DownloadPackages.each(
			function(el) {
			  
			    var newLink = new Element('a', {
			        'href': el.Value,
			        'html': el.Key
			    });

			    newLink.inject(linksConatiner, 'bottom');
			    var win;
			    if (Browser.Engine.webkit) {
			        //Safari doesnt close the download window after the download process is initiated.
			        //Bug: 17179.
			        //TODO: Need to find a better solution or check in the next version of Safari. 
			        //Right now the window will close itself after 5 seconds.
			         win = window.open(el.Value, '_blank', 'height=1,width=1');
			         win.setTimeout("self.close();", 5000);
			    } else {
			        win = window.open(el.Value, '_blank', 'height=100,width=100');
			    }
			}
		)

       
    },

    DownloadImageFailed: function(result) {
        if (!executer.get_aborted()) {
            OpenModal('downloadErrorModal');
            $('modalOverlay').setStyle('opacity', 0.7);
        }
    },

    OpenProgressModal: function(url) {
        var options = {
            cornerRadius: 0,
            headerHeight: 0,
            footerHeight: 0,
            contentBgColor: 'transparent',
            useCanvas: false,
            createFromHTML: false
        };
        new CorbisUI.Popup('downloadProgress', options);
        $('modalOverlay').setStyle('opacity', 0.7);
        $(parent.document).getElement('#QuickPicWindow_overlay').setStyles({
            'background-image': 'url(../Images/QuickPicProgressBackground.gif)',
            'background-repeat': 'no-repeat'
        });
    },

    HideProgressModal: function() {
        $(parent.document).getElement('#QuickPicWindow_overlay').setStyle('background-image', 'none');
        HideModal('downloadProgress');
    },

    CancelDownload: function() {
        if (executer && executer.get_started()) {
            executer.abort();
        }

        this.HideProgressModal();
    },

    SetFileSize: function(size) {
        $('selectImage').getElements('select.fileSize').each(function(el) {
            el.selectedIndex = (size == 'smallest' ? el.options.length - 1 : 0);
        }
		);
    }
}
