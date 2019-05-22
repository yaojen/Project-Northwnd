var ajaxSend = function (pType, pUrl, pData, pAsync, pBeforeSend, pSuccess, pError, pComplete) {
    if (pType == undefined) {
        pType = 'post';
    }

    if (pAsync == undefined) {
        pAsync = false;
    }

    $.ajax({
        type: pType,
        url: pUrl,
        data: pData,
        async: pAsync,
        dataType: 'json',
        beforeSend: function () {
            if (pBeforeSend != undefined) {
                pBeforeSend();
            }
        },
        success: function (data) {
            var objData = JSON.parse(data);
            if (pSuccess != undefined){
                pSuccess(objData);
            }
        },
        error: function (e) {
            if (pError != undefined) {
                pError(e);
            }
        },
        complete: function (e) {
            if (pComplete != undefined) {
                pComplete();
            }
        }
    });
}