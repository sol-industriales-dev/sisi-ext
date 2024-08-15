
(function () {
    $.namespace('maquinaria.overhaul.reporteFallas');
    ReporteFallas = function () {
        id = $.urlParam('id');
        idRel = 0;
        contador = 0;
        archivosEvidencia = [];
        var gridEvidencias = null;
        const divTiposRefacciones = $("#divTiposRefacciones");
        const cboRefacciones = $("#cboRefacciones");
        const divTiposFallaComponentes = $("#divTiposFallaComponentes");
        const cboCC = $("#cboCC");
        const tbFechaReporte = $("#tbFechaReporte");
        const tbFechaParo = $("#tbFechaParo");
        const cboEconomico = $("#cboEconomico");
        const tbDescripcion = $("#tbDescripcion");
        const tbMarca = $("#tbMarca");
        const tbModelo = $("#tbModelo");
        const tbSerie = $("#tbSerie");
        const tbHorometros = $("#tbHorometros");
        const tbDesFalla = $("#tbDesFalla");
        const cboConjuntoAveriado = $("#cboConjuntoAveriado");
        const cboSubConjuntoAveriado = $("#cboSubConjuntoAveriado");
        const tbTipoReparacion = $("#tbTipoReparacion");
        const tbReparacionDescripcion = $("#tbReparacionDescripcion");
        const btnGuardarReporte = $("#btnGuardarReporte");
        const ckAplica = $("#ckAplica");
        const cboTipoRefacciones = $("#cboTipoRefacciones");
        const cboGrupoRefacciones = $("#cboGrupoRefacciones");
        const tbcausaFalla = $("#tbcausaFalla");
        const tbDiagnosticosAplicados = $("#tbDiagnosticosAplicados");
        const cboDestino = $("#cboDestino");
        const tbfechaRecepcion = $("#tbfechaRecepcion");
        const cboComponenteAveriado = $("#cboComponenteAveriado");
        const cboMotivoFalla = $("#cboMotivoFalla");
        const tbfechaInstalacionComponente = $("#tbfechaInstalacionComponente");
        const tbHorasComponente = $("#tbHorasComponente");
        const tbNumParteComponente = $("#tbNumParteComponente");
        const cboVistoBueno = $("#cboVistoBueno");
        const btnCargarImgEvidencia = $("#btnCargarImgEvidencia");
        const inCargarImgEvidencia = $("#inCargarImgEvidencia");
        const imgEvidencia = $("#imgEvidencia");
        const tblEvidencia = $('#tblEvidencia');
        const cboTipoArchivo = $('#cboTipoArchivo');
        const divImagen = $('#divImagen');
        const GetReporteFalla = new URL(window.location.origin + '/ReporteFalla/GetReporteFalla');
        const GetDataEconomico = new URL(window.location.origin + '/ReporteFalla/GetDataEconomico');
        const CargarDatosComponente = new URL(window.location.origin + '/ReporteFalla/CargarDatosComponente');
        const GuardarReporteFallaComponente = new URL(window.location.origin + '/ReporteFalla/GuardarReporteFallaComponente');
        const GuardarReporteFallaReparacion = new URL(window.location.origin + '/ReporteFalla/GuardarReporteFallaReparacion');
        function init() {
            initGridArchivos();
            $.fn.select2.defaults.set('language', 'es');
            $('.select2').select2();
            cboCC.fillCombo('/Overhaul/FillCbo_CentroCostos');
            cboDestino.fillCombo('/Administrador/Usuarios/getLstEmpresasActivas', null, false, null);
            cboTipoRefacciones.fillCombo('/Administrativo/RepTraspaso/fillCboTipoInsumos', { sistema: 2 });
            cboTipoRefacciones.select2();
            cboTipoArchivo.fillCombo('/ReporteFalla//FillCboRptFallaTipoArchivo', null, true, null);
            tbFechaReporte.datepicker().datepicker("setDate", new Date());
            tbFechaParo.datepicker().datepicker("setDate", new Date());
            cboMotivoFalla.select2();
            cboCC.change(fillCboEconomicos);
            cboEconomico.change(function (e) {
                fillDataMaquinaria(false);
            });
            cboConjuntoAveriado.change(cargarSubconjuntos);
            cboSubConjuntoAveriado.change(cargarComponentes);
            cboComponenteAveriado.change(setDatosComponente);
            cboTipoRefacciones.change(cargarGruposRefacciones);
            cboGrupoRefacciones.change(cargarRefacciones);
            btnGuardarReporte.click(ValidarReporte);
            cboMotivoFalla.change(habilitarTipoFalla);
            inCargarImgEvidencia.change(function () {
                leerURL(this, imgEvidencia);
            });
            btnCargarImgEvidencia.click(function () {
                let tipoArchivo = +cboTipoArchivo.val();
                inCargarImgEvidencia.prop("accept", tipoArchivo === 3 ? 'image/*' : 'application/pdf');
                inCargarImgEvidencia.click();
            });
            cboConjuntoAveriado.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cargarDatos();
        }

        async function cargarDatos() {
            if (id == null) {
                id = 0;
                idRel = 0;
            }
            if (id != 0) {
                response = await ejectFetchJson(GetReporteFalla, { idReporte: id });
                if (response.success) {
                    tbFechaReporte.val(response.fechaReporte);
                    tbFechaParo.val(response.fechaParo);
                    cboCC.val(response.reporte.cc).change();
                    fillCboEconomicos();
                    cboEconomico.val(response.reporte.maquinaID);
                    fillDataMaquinaria(true);
                    tbDesFalla.val(response.reporte.descripcionFalla);
                    cboMotivoFalla.val(response.reporte.fallaComponente);
                    habilitarTipoFalla();
                    if (response.reporte.fallaComponente == 1) {
                        idRel = response.componente.id;
                        if (idRel !== 0) {
                            //cboConjuntoAveriado.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: response.maquina });
                            cboConjuntoAveriado.val(response.componente.Conjunto);
                            cboConjuntoAveriado.trigger('change');
                            cboSubConjuntoAveriado.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idModelo: response.maquina, idConjunto: response.componente.Conjunto });
                            cboSubConjuntoAveriado.val(response.componente.Subconjunto);
                            cboSubConjuntoAveriado.trigger('change');
                            cboComponenteAveriado.fillCombo('/ReporteFalla/FillCboComponentes', { idMaquina: response.reporte.maquinaID, idSubconjunto: response.componente.Subconjunto });
                            cboComponenteAveriado.val(response.componente.Componente);
                            tbfechaInstalacionComponente.val($.toDate(response.componente.Fecha));
                            tbHorasComponente.val(response.componente.Horas);
                            tbNumParteComponente.val(response.componente.Parte);
                            response.componente.AplicaOH ? ckAplica.click() : true;
                        }
                    }
                    else {
                        idRel = response.reparacion.id;
                        if (idRel !== 0) {
                            cboTipoRefacciones.val(response.reparacion.Tipo);
                            cargarGruposRefacciones();
                            cboGrupoRefacciones.val(response.reparacion.Grupo);
                            cargarRefacciones();
                            cboRefacciones.val(response.reparacion.Insumo);
                            tbReparacionDescripcion.val(response.reparacion.Bitacora);
                        }
                    }
                    gridEvidencias.clear().draw();
                    gridEvidencias.rows.add(response.archivosEvidencia.map(a =>
                    ({
                        id: a.id
                        , nombre: `Evidencia ${++contador}`
                        , tipo: a.tipo
                        , source: a.imagen
                    }))).draw();
                    response.archivosEvidencia.forEach(a => {
                        let pos = a.imagen.indexOf(';base64,')
                            , type = a.imagen.substring(5, pos);
                        archivosEvidencia.push(new File([new Blob([a.imagen], { type })], a.nombre, { type }));
                        if (a.tipo !== 3) {
                            cboTipoArchivo.find(`option[value="${a.tipo}"]`).prop("disabled", true);
                        }
                    });
                    tbcausaFalla.text(response.reporte.causaFalla);
                    tbDiagnosticosAplicados.val(response.reporte.diagnosticosAplicados);
                    tbTipoReparacion.val(response.reporte.tipoReparacion);
                    cboDestino.val(response.reporte.destino);
                    realiza: -1;
                    cboVistoBueno.val(response.reporte.revisa);
                    tbfechaRecepcion.val(response.fechaAlta);
                    tbHorometros.val(response.reporte.horometroReporte);
                }
                else {
                    AlertaGeneral('Alerta', 'No se encontró información');
                }
            }
        }

        function fillCboEconomicos() {
            cboEconomico.clearCombo();
            cboEconomico.fillCombo('/ReporteFalla/fillCboEconomicos', { obj: cboCC.val() == "" ? 0 : cboCC.val() });
            cboEconomico.change();
            cboVistoBueno.clearCombo();
            cboVistoBueno.fillCombo('/ReporteFalla/FillCboVistoBueno', { centroCostos: cboCC.val() == "" ? 0 : cboCC.val() });
            cboVistoBueno.change();
        }

        async function fillDataMaquinaria(datosGuardados) {
            let valor = cboEconomico.val();
            if (valor == null || valor == "") {
                tbDescripcion.val("");
                tbMarca.val("");
                tbModelo.val("");
                //cboConjuntoAveriado.clearCombo();
                tbSerie.val("");
                tbHorometros.val("");
            }
            else {
                response = await ejectFetchJson(GetDataEconomico, { obj: valor });
                if (response.success) {
                    tbDescripcion.val(response.Descripcion);
                    tbMarca.val(response.Marca);
                    tbModelo.val(response.Modelo);
                    tbModelo.attr("data-id", response.modeloID)
                    //cboConjuntoAveriado.clearCombo();
                    //cboConjuntoAveriado.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: response.modeloID });
                    tbSerie.val(response.Serie);
                    tbfechaRecepcion.val(response.fechaAlta);
                    if (!datosGuardados) tbHorometros.val(response.horometroAlta);
                }
                else {
                    AlertaGeneral('Alerta', 'No se encontró información');
                }
            }
        }

        function cargarSubconjuntos() {
            if (cboConjuntoAveriado.val() == "") {
                cboSubConjuntoAveriado.clearCombo();
                cboSubConjuntoAveriado.prop("disabled", true);
            }
            else {
                cboSubConjuntoAveriado.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idModelo: tbModelo.attr("data-id"), idConjunto: cboConjuntoAveriado.val() });
                cboSubConjuntoAveriado.prop("disabled", false);
            }
        }
        function cargarComponentes() {
            if (cboSubConjuntoAveriado.val() == "") {
                cboComponenteAveriado.clearCombo();
                cboComponenteAveriado.prop("disabled", true);
            }
            else {
                cboComponenteAveriado.fillCombo('/ReporteFalla/FillCboComponentes', { idMaquina: cboEconomico.val(), idSubconjunto: cboSubConjuntoAveriado.val() });
                cboComponenteAveriado.prop("disabled", false);
            }
        }

        function habilitarTipoFalla() {
            if (cboMotivoFalla.val() === "0") {
                divTiposFallaComponentes.css("display", "none");
                divTiposRefacciones.css("display", "block");
                cboSubConjuntoAveriado.clearCombo();
                cboSubConjuntoAveriado.prop("disabled", true);
                //cboConjuntoAveriado.val("");

                $("#divDiagComponente").css("display", "none");
                $("#divReparacionesAnt").css("display", "block");

                tbfechaInstalacionComponente.val("");
                tbHorasComponente.val("");
                tbNumParteComponente.val("");
                $("#blockAplica").css("display", "none");
            }
            else {
                divTiposFallaComponentes.css("display", "block");
                divTiposRefacciones.css("display", "none");
                cboGrupoRefacciones.clearCombo();
                cboGrupoRefacciones.prop("disabled", true);
                cboRefacciones.clearCombo();
                cboRefacciones.prop("disabled", true);
                cboTipoRefacciones.val("");

                $("#divDiagComponente").css("display", "block");
                $("#divReparacionesAnt").css("display", "none");

                tbfechaInstalacionComponente.val("");
                tbHorasComponente.val("");
                tbNumParteComponente.val("");
                $("#blockAplica").css("display", "block");
            }
        }

        function cargarGruposRefacciones() {
            if (cboTipoRefacciones.val() == "") {
                cboGrupoRefacciones.clearCombo();
                cboGrupoRefacciones.prop("disabled", true);
            }
            else {
                cboGrupoRefacciones.fillCombo('/Administrativo/RepTraspaso/fillCboGrupoInsumos', { tipo: cboTipoRefacciones.val(), sistema: 2 })
                cboGrupoRefacciones.prop("disabled", false);
                cboGrupoRefacciones.select2();
            }
        }

        function cargarRefacciones() {
            if (cboGrupoRefacciones.val() == "") {
                cboRefacciones.clearCombo();
                cboRefacciones.prop("disabled", true);
            }
            else {
                cboRefacciones.fillCombo('/Overhaul/FillCboInsumos', { tipo: cboTipoRefacciones.val(), grupo: cboGrupoRefacciones.val(), sistema: 2 });
                cboRefacciones.prop("disabled", false);
                cboRefacciones.select2();
            }
        }

        function guardarReporte() {
            let formData = new FormData()
                , request = new XMLHttpRequest()
                , reporte = getElementosReporte()
                , urlGuardar = reporte.reporte.fallaComponente == "1" ? GuardarReporteFallaComponente : GuardarReporteFallaReparacion;
            formData.append("idReporte", id);
            formData.append("reportes", JSON.stringify(reporte));
            gridEvidencias.rows().data().each((i, j) => {
                formData.append("files[]", archivosEvidencia[j]);
            });
            request.open("POST", urlGuardar);
            request.send(formData);
            request.onload = response => {
                if (request.status == 200) {
                    var jsonResponse = JSON.parse(request.responseText);
                    if (jsonResponse.success) {
                        window.location.href = "/ReporteFalla/AdministradorReporteFalla";
                    } else {
                        AlertaGeneral("Aviso", jsonResponse.message);
                    }
                }
            };
        }

        function getElementosReporte() {
            let reporte = {
                id: id,
                maquinaID: cboEconomico.val(),
                cc: cboCC.val(),
                descripcionFalla: tbDesFalla.val(),
                fechaReporte: tbFechaReporte.val(),
                fechaParo: tbFechaParo.val(),
                fallaComponente: cboMotivoFalla.val(),
                causaFalla: tbcausaFalla.val(),
                diagnosticosAplicados: tbDiagnosticosAplicados.val(),
                tipoReparacion: tbTipoReparacion.val(),
                reparaciones: tbReparacionDescripcion.val(),
                destino: cboDestino.val(),
                realiza: -1,
                revisa: cboVistoBueno.val(),
                fechaAlta: tbfechaRecepcion.val(),
                horometroReporte: tbHorometros.val(),
                estatus: 1,
                lstArchivos: gridEvidencias.rows().data().map(i => ({
                    idReporteFalla: id
                    , nombre: i.nombre
                    , tipo: i.tipo
                    , id: i.id
                })).toArray()
            };
            if (cboMotivoFalla.val() == "1") {
                let componente = {
                    id: idRel
                    , idReporteFalla: id
                    , Conjunto: cboConjuntoAveriado.val()
                    , Subconjunto: cboSubConjuntoAveriado.val()
                    , Componente: cboComponenteAveriado.val()
                    , AplicaOH: ckAplica.is(":checked")
                    , Fecha: tbfechaInstalacionComponente.val()
                    , Horas: tbHorasComponente.val()
                    , Parte: tbNumParteComponente.val()
                    , reporte
                };
                return componente;
            } else {
                let reparacion = {
                    id: idRel
                    , idReporteFalla: id
                    , Tipo: cboTipoRefacciones.val()
                    , Grupo: cboGrupoRefacciones.val()
                    , Insumo: cboRefacciones.val()
                    , Bitacora: tbReparacionDescripcion.val()
                    , reporte
                };
                return reparacion;
            }
        }

        function ValidarReporte() {
            let estado = true
                , lstNoValido = [null, ""];
            switch (true) {
                case lstNoValido.includes(cboCC.val()):
                case lstNoValido.includes(tbFechaParo.val()):
                case lstNoValido.includes(tbHorometros.val()):
                case lstNoValido.includes(tbDesFalla.val()):
                case lstNoValido.includes(tbcausaFalla.val()):
                case lstNoValido.includes(tbDiagnosticosAplicados.val()):
                case lstNoValido.includes(tbTipoReparacion.val()):
                case lstNoValido.includes(cboDestino.val()):
                case lstNoValido.includes(cboVistoBueno.val()):
                    estado = false;
                    break;
                case cboMotivoFalla.val() == "0":
                    switch (true) {
                        case lstNoValido.includes(cboTipoRefacciones.val()):
                        case lstNoValido.includes(cboGrupoRefacciones.val()):
                        case lstNoValido.includes(cboRefacciones.val()):
                        case lstNoValido.includes(tbReparacionDescripcion.val()):
                            estado = false;
                            break;
                    }
                    break;
                case cboMotivoFalla.val() != "0":
                    switch (true) {
                        case lstNoValido.includes(cboConjuntoAveriado.val()):
                        case lstNoValido.includes(cboSubConjuntoAveriado.val()):
                        case lstNoValido.includes(cboComponenteAveriado.val()):
                            estado = false;
                            break;
                    }
                    break;
            }
            if (estado) {
                guardarReporte();
            }
        }

        async function setDatosComponente() {
            if (cboComponenteAveriado.val() != null && cboComponenteAveriado.val() != "") {
                response = await ejectFetchJson(CargarDatosComponente, { idComponente: cboComponenteAveriado.val() });
                if (response.success) {
                    tbfechaInstalacionComponente.val(response.fechaInstalacion);
                    tbHorasComponente.val(response.horasUso);
                    tbNumParteComponente.val(response.numParte);
                }
            }
            else {
                tbfechaInstalacionComponente.val("");
                tbHorasComponente.val("");
                tbNumParteComponente.val("");
            }
        }

        function leerURL(input, imagen) {
            let tipoArchivo = +cboTipoArchivo.val();
            if (input.files && input.files[0]) {
                if (!validaCboTipoArchivo()) return;
                contador++;

                //ImageTools.resize(
                //    input.files[0],
                //    { width: 500, height: 500 },
                //    function (blob, didItResize) {
                //        toDataUrl(window.URL.createObjectURL(blob), function (myBase64) {
                //            //imagen.attr('src', myBase64);
                //            var reader = new FileReader();
                //            reader.onload = function (e) {
                //                let JSONINFO = { "id": contador, 'nombre': "Evidencia " + contador, "source": e.target.result, "tipo": tipoArchivo };
                //                gridEvidencias.row.add(JSONINFO).draw();
                //                archivosEvidencia.push(myBase64);
                //                cboTipoArchivo.val(3);
                //            }
                //            reader.readAsDataURL(myBase64);

                //        });
                //    }
                //);
                var reader = new FileReader();
                reader.onload = function (e) {
                    let JSONINFO = { "id": contador, 'nombre': "Evidencia " + contador, "source": e.target.result, "tipo": tipoArchivo };
                    gridEvidencias.row.add(JSONINFO).draw();
                    archivosEvidencia.push(input.files[0]);
                    cboTipoArchivo.val(3);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function validaCboTipoArchivo() {
            let valido = false
                , option = cboTipoArchivo.find(`option:selected`);
            if (+option.val() == 3) {
                valido = true;
            } else {
                if (option.prop('disabled')) {
                    valido = false;
                } else {
                    option.prop('disabled', true);
                    valido = true;
                }
            }
            return valido;
        }
        function visualizarImagen({ id, source }) {
            let image = document.createElement("img");
            image.src = source;
            divImagen.data().id = id;
            divImagen.empty().append(image);
        }
        function initGridArchivos() {
            gridEvidencias = tblEvidencia.DataTable({
                destroy: true
                , ordering: false
                , paging: false
                , ordering: false
                , searching: false
                , bFilter: true
                , info: false
                , language: dtDicEsp
                , columns: [
                    { data: 'nombre' }
                    , { data: 'tipo', render: data => `${cboTipoArchivo.find(`option[value="${data}"]`).text()}` }
                    , { data: 'id', width: '3%', render: data => `<button type='button' data-id="${data}" class='btn btn-danger eliminar'><i class="fas fa-minus"></i></button>` }
                    , { data: 'tipo', width: '3%', render: data => data !== 3 ? '' : `<button type='button' class='btn btn-primary ver'><i class="fas fa-images"></i></button>` }
                ]
                , initComplete: function (settings, json) {
                    tblEvidencia.on("click", ".eliminar", function (e) {
                        let row = $(this).closest('tr'), data = gridEvidencias.row(row).data();
                        gridEvidencias.row(row).remove().draw();
                        archivosEvidencia.splice(archivosEvidencia.indexOf("contador", data.id), 1);
                        cboTipoArchivo.find(`option[value="${data.tipo}"]`).prop(`disabled`, false);
                        if (data.id === divImagen.data().id) {
                            divImagen.empty();
                        }
                    });
                    tblEvidencia.on("click", ".ver", function (e) {
                        let row = $(this).closest('tr')
                            , data = gridEvidencias.row(row).data();
                        visualizarImagen(data);
                    });
                }
            });
        }
        init();
    };
    $(document).ready(function () {
        maquinaria.overhaul.reporteFallas = new ReporteFallas();
    });
})();

