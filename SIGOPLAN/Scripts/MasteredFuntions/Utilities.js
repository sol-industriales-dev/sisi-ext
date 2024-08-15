(
    function ($) {
        $.sm_SplittedSave = async function (url, myArray, scheme, chunk_size, fnResponse) {
            setBarProgress(0);
            var response = {};
            response.success = new Array();
            response.error = new Array();
            response.status = 0;
            response.errorJson = "";
            var count = 0;
            let total = myArray.length;
            let acum = 0;
            while (myArray.length) {
                let porcentaje = (100 * acum / total).toFixed(2);
                setBarProgress(porcentaje);
                var data = myArray.splice(0, chunk_size);
                scheme.lst = new Array();
                scheme.lst = data;
                acum += data.length;
                var obj = {};
                obj.loop = count++;
                obj.success = await _sp_saveMethod(url, scheme);
                obj.dataRequest = scheme;
                var schemeJson = JSON.stringify(obj);
                var schemeObj = JSON.parse(schemeJson);
                if (obj.success) {
                    response.success.push(schemeObj);
                }
                else {
                    response.error.push(schemeObj);
                }
            }
            response.errorJson = JSON.stringify(response.error);

            var success = response.success.length;
            var error = response.error.length;

            if (success > 0 && error == 0) {
                response.status = 1;
            }
            else if (success > 0 && error > 0) {
                response.status = 2;
            }
            else {
                response.status = 3;
            }
            $.unblockUI();
            _sp_responseMethod(response, fnResponse);
            return response;
        };
        $.sm_SplittedSaveNoMessage = async function (url, myArray, scheme, chunk_size, fnResponse) {
            setBarProgress(0);
            var response = {};
            response.success = new Array();
            response.error = new Array();
            response.status = 0;
            response.errorJson = "";
            var count = 0;
            let total = myArray.length;
            let acum = 0;
            while (myArray.length) {
                let porcentaje = (100 * acum / total).toFixed(2);
                setBarProgress(porcentaje);
                var data = myArray.splice(0, chunk_size);
                scheme.lst = new Array();
                scheme.lst = data;
                acum += data.length;
                var obj = {};
                obj.loop = count++;
                obj.success = await _sp_saveMethod(url, scheme);
                obj.dataRequest = scheme;
                var schemeJson = JSON.stringify(obj);
                var schemeObj = JSON.parse(schemeJson);
                if (obj.success) {
                    response.success.push(schemeObj);
                }
                else {
                    response.error.push(schemeObj);
                }
            }
            response.errorJson = JSON.stringify(response.error);

            var success = response.success.length;
            var error = response.error.length;

            if (success > 0 && error == 0) {
                response.status = 1;
            }
            else if (success > 0 && error > 0) {
                response.status = 2;
            }
            else {
                response.status = 3;
            }
            $.unblockUI();
            _sp_responseMethod_NoMessage(response, fnResponse);
            return response;
        };
        $.LoadInMemoryThenSave = async function (urlSession, urlSave, myArray, scheme, chunk_size, fnResponse) {
            setBarProgress(0);
            var response = {
                success: new Array(),
                error: new Array(),
                status: 0,
                errorJson: ""
            };
            var count = 0;
            let total = myArray.length;
            let acum = 0;
            if (scheme == null || scheme == undefined) {
                scheme = {};
            }
            while (myArray.length) {
                let porcentaje = (100 * acum / total).toFixed(2);
                setBarProgress(porcentaje);
                var lstSplited = myArray.splice(0, chunk_size);
                // scheme.lst = new Array();
                // scheme.lst = lstSplited;
                acum += lstSplited.length;
                var obj = {};
                obj.loop = count++;
                obj.success = true;
                await axios.post(urlSession, lstSplited)
                    .then(responseSession => {
                        let { success } = responseSession.data;
                        if (!success) {
                            obj.success = success;
                        }
                    }).catch(o_O => {
                        $.unblockUI();
                        obj.success = false;
                        AlertaGeneral(o_O.message);
                    });
                obj.dataRequest = scheme;
                if (obj.success) {
                    response.success.push(obj);
                }
                else {
                    response.error.push(obj);
                }
            }
            response.errorJson = JSON.stringify(response.error);
            var success = response.success.length;
            var error = response.error.length;
            switch (true) {
                case success > 0 && error == 0:
                    await axios.post(urlSave, scheme)
                        .then(responseSave => {
                            if (responseSave.data.success) {
                                response.status = 1;
                            }
                        }).catch(o_O => {
                            $.unblockUI();
                            AlertaGeneral(o_O.message);
                        });
                    break;
                case success > 0 && error > 0:
                    response.status = 2;
                    break; 6
                default:
                    response.status = 3;
                    break;
            }
            $.unblockUI();
            _sp_responseMethod(response, fnResponse);
            return response;
        };
    }
)(jQuery);

async function _sp_saveMethod(url, data) {
    try {
        response = await ejectFetchJsonGlobal(url, data);
        return response.success;
    } catch (o_O) {
        return false;
    }
}
function _sp_responseMethod(data, fnResponse) {
    if (data.status == 1) {
        fnResponse();
        AlertaGeneral("Aviso", "Datos guardados correctamente.");
    }
    else if (data.status == 2) {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "¡No todos los datos se guardaron!, no cierre este mensaje y hable al personal de TI.");
    }
    else {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "Ocurrio un error al guardar, no cierre este mensaje y hable al personal de TI.");
    }
}
function _sp_responseMethod_NoMessage(data, fnResponse) {
    if (data.status == 1) {
        fnResponse();
    }
    else if (data.status == 2) {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "¡No todos los datos se guardaron!, no cierre este mensaje y hable al personal de TI.");
    }
    else {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "Ocurrio un error al guardar, no cierre este mensaje y hable al personal de TI.");
    }
}
function ejectFetchJsonGlobal(url, param) {
    let header = headerFetchJson();
    if (param !== null) {
        header.body = JSON.stringify(param);
    }
    let response = fetch(url, header)
        .then(response => {
            return response.json();
        }).catch(o_O => { $.unblockUI(); });
    return response;
}
function setBarProgress(porcentaje) {
    let barra = `<div class="progress progress-striped active">
                        <div class="progress-bar progress-bar-success" role="progressbar"
                            aria-valuenow="${porcentaje}" aria-valuemin="0" aria-valuemax="100"
                            style="width: ${porcentaje}%">
                         <span class="sr-only"></span>
                       </div>
                     </div>
                ${porcentaje}% guardando`,
        block = $(".blockUI.blockMsg.blockPage");
    if (block.length === 0) {
        $.blockUI({
            message: barra,
            baseZ: 9999,
            theme: false,
        });
    }
    block.html(barra);
}
var axiosClean = axios.create({
    timeout: 100000,
});
axios.interceptors.request.use(function (config) {
    $.blockUI({ message: 'Procesando...', baseZ: 9999 });
    return config;
}, function (error) {
    $.unblockUI();
    return Promise.reject(error);
});
axios.interceptors.response.use(function (response) {
    $.unblockUI();
    return response;
}, function (error) {
    $.unblockUI();
    return Promise.reject(error);
});
//$(document)
//    .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
//    .ajaxStop(() => $.unblockUI())