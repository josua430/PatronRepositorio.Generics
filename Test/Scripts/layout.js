
function validateEmail(emailField) {
    //var reg = /^([A-Za-z0-9_\-\.])+\@@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var reg= /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var res = emailField.split(";");
    for (i in res) {
        if (reg.test(res[i]) === false) {
            //alert('Email no válido');
            return res[i];
        }
    }
    return "";
}

    function ShowMessageNotification(message) {
        var m = message;

        var notification = $("#popupNotification").kendoNotification({
            position: {
                pinned: true,
                top: 30,
                right: 30
            },
            autoHideAfter: 3000,
            stacking: "down"
        }).data("kendoNotification");

        notification.show(m);
    }

    function ShowMessageSuccess(message) {
        var m = message;

        var notification = $("#popupNotification").kendoNotification({
            position: {
                pinned: true,
                top: 30,
                right: 30
            },
            autoHideAfter: 3000,
            stacking: "down"
        }).data("kendoNotification");

        notification.success(m);
    }

    function ShowMessageError(message) {
        var m = message;

        var notification = $("#popupNotification").kendoNotification({
            position: {
                pinned: true,
                top: 30,
                right: 30
            },
            autoHideAfter: 3000,
            stacking: "down"
        }).data("kendoNotification");

        notification.error(m);
    }

    function ShowMessageWarning(message) {
        var m = message;

        var notification = $("#popupNotification").kendoNotification({
            position: {
                pinned: true,
                top: 30,
                right: 30
            },
            autoHideAfter: 6000,
            stacking: "down"
        }).data("kendoNotification");

        notification.warning(m);
    }

    function ShowMessageErrorAdmin(message) {
        var m = message;

        var notification = $("#popupNotification").kendoNotification({
            position: {
                pinned: true,
                top: 30,
                right: 30
            },
            autoHideAfter: 10000,
            stacking: "down"
        }).data("kendoNotification");

        notification.error(m);
    }

$(document).ready(function () {

    $.blockUI.defaults = {
        // message displayed when blocking (use null for no message)
        message: '',

        title: null,        // title string; only used when theme == true
        draggable: true,    // only used when theme == true (requires jquery-ui.js to be loaded)

        theme: false, // set to true to use with jQuery UI themes

        // styles for the message when blocking; if you wish to disable
        // these and use an external stylesheet then do this in your code:
        // $.blockUI.defaults.css = {};
        css: {
            padding: 0,
            margin: 0,
            width: '30%',
            top: '40%',
            left: '35%',
            textAlign: 'center',
            color: '#fff',
            cursor: 'wait'
        },

        // minimal style set used when themes are used
        themedCSS: {
            width: '30%',
            top: '40%',
            left: '35%'
        },

        // styles for the overlay
        overlayCSS: {
            backgroundColor: '#000',
            opacity: 0.8,
            cursor: 'wait'
        },

        // style to replace wait cursor before unblocking to correct issue
        // of lingering wait cursor
        cursorReset: 'default',

        // styles applied when using $.growlUI
        growlCSS: {
            width: '350px',
            top: '10px',
            left: '',
            right: '10px',
            border: 'none',
            padding: '5px',
            opacity: 0.6,
            cursor: null,
            color: '#fff',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px'
        },

        iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',

        forceIframe: false,

        baseZ: 1000,

        centerX: true, // <-- only effects element blocking (page block controlled via css above)
        centerY: true,

        // allow body element to be stetched in ie6; this makes blocking look better
        // on "short" pages.  disable if you wish to prevent changes to the body height
        allowBodyStretch: true,

        // enable if you want key and mouse events to be disabled for content that is blocked
        bindEvents: true,

        // be default blockUI will supress tab navigation from leaving blocking content
        // (if bindEvents is true)
        constrainTabKey: true,

        // fadeIn time in millis; set to 0 to disable fadeIn on block
        fadeIn: 200,

        // fadeOut time in millis; set to 0 to disable fadeOut on unblock
        fadeOut: 400,

        // time in millis to wait before auto-unblocking; set to 0 to disable auto-unblock
        timeout: 0,

        // disable if you don't want to show the overlay
        showOverlay: true,

        // if true, focus will be placed in the first available input field when
        // page blocking
        focusInput: true,
        onBlock: null,
        onUnblock: null,
        quirksmodeOffsetHack: 4,
        blockMsgClass: 'blockMsg',
        ignoreIfBlocked: false
    };

    $('form').submit(function () {

        $.blockUI();

    });


});

$(document).ajaxStart(function () {
    $.blockUI();

});

$(document).ajaxStop(function () {
    $.unblockUI();
});