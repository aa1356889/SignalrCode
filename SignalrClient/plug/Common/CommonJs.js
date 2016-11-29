var myCommon = {
    Ajax:{ },
    Button: {},

};
myCommon.Ajax.ResponseReulstProcess = function (ajaxObj, successCallback, failCallback) {
    switch (ajaxObj.State) {
        case 0://操作成功
            successCallback();
            break;
        case 1://操作失败
            this.Button.Seucess(title, ajaxObj.Message);
            if(failCallback)
            failCallback();
            break;
    }
}

myCommon.Button.Seucess=function(title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr["success"](msg, title);
}