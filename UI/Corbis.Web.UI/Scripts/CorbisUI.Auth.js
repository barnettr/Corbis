/****************************************************
    Corbis UI Auth
***************************************************/

// * CorbisUI.Auth Namespace
// * Provides authentication methods for site
// * TravisO

CorbisUI.Auth = {
    ActionTypes: {
        Reload: 'reload',
        ReturnUrl: 'returnUrl',
        Execute: 'execute',
        None: 'none'
    },

    SignInLevels: {
        None: -1,
        Anonymous: 0,
        AutoLoggedIn: 1,
        LoggedIn: 2
    },

    currentSignInLevel: null,

    OpenSSLSignIn: function(actionType, actionArg) {
        var signIn = HttpsUrl + '/Registration/SignIn.aspx?' + actionType + "=" + escape(actionArg);
        if (window.location.protocol == 'https:') {
            signIn += '&protocol=https';
        }
        var dlgWidth = 350;
        var g_firefox = document.getElementById && !document.all;
        if (g_firefox) {
            OpenNewIModal(signIn, dlgWidth, 140, 'secureSignIn');
            if (window.location.protocol != 'https:') {
                // Firefox bug:  Sometimes the iFrame doesn't load.  Attach an onload event to this to check; wait 5 seconds for load
                setTimeout('CorbisUI.Auth.FirefoxLoadCheck()', 3000);
            }
        } else {
            OpenNewIModal(signIn, dlgWidth, 140, 'secureSignIn');
        }
    },

    GetSignInLevel: function() {
        var returnValue = 0;
        var req = new Request({
            async: false,
            method: 'post',
            url: "/Registration/SignInStatus.asmx/SignInState",
            onSuccess: function(responseText, responseXml) { returnValue = parseInt(responseText.replace(/(<([^>]+)>)/ig, ""), 10); }
        }).send();
        CorbisUI.Auth.currentSignInLevel = returnValue;
        return CorbisUI.Auth.currentSignInLevel;
    },

    Check: function(signInLevel, actionType, actionArg) {
        if (signInLevel == null) { signInLevel = this.SignInLevels.LoggedIn; }
        if (actionType == null) { actionType = this.ActionTypes.Reload; }
        var currentSignInLevel = this.GetSignInLevel();
        if (currentSignInLevel < signInLevel) {
            this.OpenSSLSignIn(actionType, actionArg);
        } else {
            switch (actionType) {
                case this.ActionTypes.Reload:
                    window.location = window.location;
                    break;
                case this.ActionTypes.ReturnUrl:
                    window.location = actionArg;
                    break;
                case this.ActionTypes.Execute:
                    eval(actionArg);
                    break;
            }
        }
    },

    Go: function(url) {
        if (window.opener) {
            window.opener.location = url;
            window.close();
        } else {
            parent.window.location = url;
            window.location = url;
        }
    },

    FirefoxLoadCheck: function() {
        try {
            // if loaded, this code will return an error since it can't get to the document in an SSL iFrame
            var oIframe = document.getElementById("secureSignInWindow_iframe");
            var oDoc = oIframe.contentWindow || oIframe.contentDocument;
            if (oDoc.document) {
                oDoc = oDoc.document;
            }
            oDoc.body.style.backgroundColor = oDoc.body.style.backgroundColor;
            oIframe.src = oIframe.src + "#";
            // if opening page is loading under https, this won't throw an error.
            if (window.location.protocol != 'https:') {
                setTimeout('CorbisUI.Auth.FirefoxLoadCheck()', 3000);
            }
        } catch (e)
        { }
    },

    SetErrorStyle: function(height) {
        var windowId = 'secureSignIn';
        ResizeIModal(windowId, height);
    },
    SetErrorStyleXY: function(w, h) {
        var windowId = 'secureSignIn';
        ResizeIModalXY(windowId, w, h);
    }
}

CorbisUI.Auth.currentSignInLevel = CorbisUI.Auth.SignInLevels.None;