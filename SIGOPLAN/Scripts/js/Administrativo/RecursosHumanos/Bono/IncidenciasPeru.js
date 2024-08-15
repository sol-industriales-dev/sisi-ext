(() => {
    $.namespace('Administrativo.RecursosHumanos.Bono.Incidencia');
    Incidencia = function () {
        let _incidenciaObj = null;
        let _evaluacion_pendiente = false;
        let itemBonIncConp = [], dtBonIncidencias;
        let periodosData = [];
        let arrayObservaciones = [];
        let arrayObservacionesBono = [];
        let clave_empleado_actual = 0;
        let botonObsActual = null;
        let botonObsBonoActual = null;
        let readyToSave = false;
        let permiso_bono_sinlimite = false;
        let ccAplicaPrimaDominical = ['007', '183', '189'];
        let _incidenciaObjPeru = null;

        //EMPRESA ACTUAL
        const txtEmpresa = $('#txtEmpresa');
        _EMPRESA_ACTUAL = txtEmpresa.val();

        const btnModalDesautorizar = $("#btnModalDesautorizar");
        const tblAuth = $("#tblAuth");
        const btnDesAuth = $("#btnDesAuth");
        const modalAuthIncidencia = $("#modalAuthIncidencia");
        const modalDesAuthIncidencia = $("#modalDesAuthIncidencia");
        const btnAuth = $("#btnAuth");
        const modalAuthIncidencia_evPendiente = $("#modalAuthIncidencia_evPendiente");
        const btnAuth_evPendiente = $("#btnAuth_evPendiente");
        const btnGuardar = $("#btnGuardar");
        const selBonIncCc = $('#selBonIncCc');
        const inpBonIncAnio = $('#inpBonIncAnio');
        const btnBonIncAuth = $('#btnBonIncAuth');
        const selBonIncDepto = $('#selBonIncDepto');
        const inpBonIncEmpMod = $('#inpBonIncEmpMod');
        const btnBonIncBuscar = $('#btnBonIncBuscar');
        const inpBonIncPeriodo = $('#inpBonIncPeriodo');
        const inpBonIncEstatus = $('#inpBonIncEstatus');
        const tblBonIncidencias = $('#tblBonIncidencias');
        const inpBonIncFechaMod = $('#inpBonIncFechaMod');
        const selBonIncTipoNomina = $('#selBonIncTipoNomina');
        const spnBonIncPeriodoFechas = $('#spnBonIncPeriodoFechas');

        const modalObservaciones = $('#modalObservaciones');
        const txtObservacion = $('#txtObservacion');
        const btnSetObervacion = $('#btnSetObervacion');

        const modalObservacionesBono = $('#modalObservacionesBono');
        const txtObservacionBono = $('#txtObservacionBono');
        const btnSetObervacionBono = $('#btnSetObervacionBono');

        const getPeriodo = new URL(window.location.origin + '/Administrativo/Bono/getPeriodo');
        const getLstIncidencias = new URL(window.location.origin + '/Administrativo/Bono/getLstIncidencias');
        const getLstIncidenciasEnkontrol = new URL(window.location.origin + '/Administrativo/Bono/getLstIncidenciasEnkontrol');
        const getCboIncidecnciaConcepto = new URL(window.location.origin + '/Administrativo/Bono/getCboIncidecnciaConcepto');
        const GuardarIncidencia = new URL(window.location.origin + '/Administrativo/Bono/GuardarIncidencia');
        const GuardarIncidenciaSincronizar = new URL(window.location.origin + '/Administrativo/Bono/GuardarIncidenciaSincronizar');
        const IncidenciaAuthDet = new URL(window.location.origin + '/Administrativo/Bono/getIncidenciaAuth');
        const authIncidencia = new URL(window.location.origin + '/Administrativo/Bono/authIncidencia');
        const desAuthIncidencia = new URL(window.location.origin + '/Administrativo/Bono/desAuthIncidencia');

        const mdlExtraTemp = $('#mdlExtraTemp');
        const tblExtraTemp = $('#tblExtraTemp');
        const mdlEvidencia = $('#mdlEvidencia');
        const inputArchivo = $('#inputArchivo');
        const btnGuardarArchivoEvidencia = $('#btnGuardarArchivoEvidencia');
        const txtArchivoEvidencia = $('#txtArchivoEvidencia');
        const txtArchivoEvidenciaGlobal = $('#txtArchivoEvidenciaGlobal');
        let dtExtraTemp;

        //#region CONST FUNCIONALIDAD AUTOFILL INCIDENCIAS
        const btnAbrirMdlAutoFillIncidencias = $('#btnAbrirMdlAutoFillIncidencias');
        const mdlAutoFillIncidencias = $("#mdlAutoFillIncidencias");
        const cboFillLunes = $("#cboFillLunes");
        const cboFillMartes = $("#cboFillMartes");
        const cboFillMiercoles = $("#cboFillMiercoles");
        const cboFillJueves = $("#cboFillJueves");
        const cboFillViernes = $("#cboFillViernes");
        const cboFillSabado = $("#cboFillSabado");
        const cboFillDomingo = $("#cboFillDomingo");
        const btnAutoFillIncidencias = $('#btnAutoFillIncidencias');
        //#endregion

        const data = new FormData();
        let strClaveEmpleados = "";

        var buttonCommon = {
            exportOptions: {
                format: {
                    body: function (data, row, column, node) {
                        if ($(node.children).hasClass("_empleado"))
                            return $(node.children).find('p').text();
                        else if ($($(node.children).find("input")[0]).hasClass("totalExtra"))
                            return $($(node.children).find("option:selected")).text();
                        else if ($($(node.children).find("input")[0]).hasClass("_totalDias"))
                            return $($(node.children).find("input")[0]).val();
                        else if ($($(node.children).find("input")[0]).hasClass("_totalHoras"))
                            return $($(node.children).find("input")[0]).val();
                        else if ($($(node.children).find("input")[0]).hasClass("_primaDominical"))
                            return data == true ? "SI" : "NO";
                        else
                            return data;
                    }
                }
            }
        };

        let init = () => {
            initForm();
            selBonIncCc.select2();
            selBonIncCc.change(setSelDepto);
            inpBonIncAnio.change(setPeriodo);
            inpBonIncPeriodo.change(setPeriodo);
            btnBonIncBuscar.click(setLstIncidencias);
            selBonIncTipoNomina.change(setSpnPeriodo);

            btnGuardar.click(fnGuardar);
            btnSetObervacion.click(fnSetObservacion);
            btnSetObervacionBono.click(fnSetObservacionBono);
            tblBonIncidencias.on('change', '.bonoDP', function (e) {

                var _this = $(this);
                var row = _this.parent().parent().parent().parent();
                var cveEmp = _this.data('clave_empleado');
                var btn = $(".obsBono" + cveEmp);


                if (_this.getVal(2) == 0) {
                    $(btn).removeClass("btn-success");
                    $(btn).removeClass("btn-danger");
                    $(btn).removeClass("btn-primary");
                    $(btn).addClass("btn-light");
                    $(btn).prop('disabled', true);
                    var observacionesBonoTemp = arrayObservacionesBono.find(x => x.clave_empleado == cveEmp);
                    if (arrayObservacionesBono != undefined) {
                        $.each(arrayObservacionesBono, function (i, el) {
                            if (this.clave_empleado == cveEmp) {
                                arrayObservacionesBono.splice(i, 1);
                            }
                        });
                    }
                }
                else {
                    $(btn).prop('disabled', false);
                    $(btn).removeClass("btn-light");
                    $(btn).addClass("btn-danger");

                }
                var parent = _this.closest('tr');
                var rowData = dtBonIncidencias.row(parent).data();
                rowData.bono = _this.getVal(2);
            });
            tblBonIncidencias.on('change', '._totalHoras', function (e) {
                var _this = $(this);
                var parent = _this.closest('tr');
                var rowData = dtBonIncidencias.row(parent).data();
                rowData.totalo_Horas = _this.getVal(2);
            });
            tblBonIncidencias.on('change', '._primaDominical', function (e) {
                var _this = $(this);
                var parent = _this.closest('tr');
                var rowData = dtBonIncidencias.row(parent).data();
                rowData.primaDominical = _this.prop("checked");
            });
            initDataTblExcluidos();
            btnBonIncAuth.click(fnAutorizaIncidencia);
            $('#modalAuthIncidencia').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
            btnAuth.click(fnAuthIncidencia);
            btnAuth_evPendiente.click(fnAuthIncidencia_evPendiente);
            btnModalDesautorizar.click(openDesautorizar);
            btnDesAuth.click(fnDesautorizar);
            btnGuardarArchivoEvidencia.click(fncGetArchivos);

            //#region CONST FUNCIONALIDAD AUTOFILL INCIDENCIAS
            btnAbrirMdlAutoFillIncidencias.click(function () {
                mdlAutoFillIncidencias.modal("show");
            });

            btnAutoFillIncidencias.click(function () {
                $("._dia_1").val(cboFillLunes.val());
                $("._dia_2").val(cboFillMartes.val());
                $("._dia_3").val(cboFillMiercoles.val());
                $("._dia_4").val(cboFillJueves.val());
                $("._dia_5").val(cboFillViernes.val());
                $("._dia_6").val(cboFillSabado.val());
                $("._dia_7").val(cboFillDomingo.val());

                $("._dia_1").trigger("change");
                $("._dia_2").trigger("change");
                $("._dia_3").trigger("change");
                $("._dia_4").trigger("change");
                $("._dia_5").trigger("change");
                $("._dia_6").trigger("change");
                $("._dia_7").trigger("change");
                mdlAutoFillIncidencias.modal("hide");
            });
            //#endregion
        }

        function fncGetArchivos() {
            $.each(document.getElementById("txtArchivoEvidencia").files, function (i, file) {
                // strClaveEmpleados.push(btnGuardarArchivoEvidencia.data().clave_empleado);
                if (strClaveEmpleados == "") {
                    strClaveEmpleados = btnGuardarArchivoEvidencia.data().clave_empleado;
                } else {
                    strClaveEmpleados += `, ${btnGuardarArchivoEvidencia.data().clave_empleado}`;
                }
                data.append("archivos", file);
            });
            mdlEvidencia.modal("hide");
        }

        function openDesautorizar() {
            var obj = {};
            obj.id = _incidenciaObj.id;
            //obj.id_incidencia = _incidenciaObj.id_incidencia;

            axios.post("RevisarFechaCierre", { lst: [obj] }).then(response => {
                let { success, items, message } = response.data;
                switch (response.data.estadoIncidencias) {
                    case 0:
                        AlertaGeneral('Alerta', 'Algunas incidencias que intenta desautorizar corresponden a perdiodos cerrados.');
                        break;
                    case 1:
                        modalDesAuthIncidencia.modal("show");
                        break;
                    default:
                        AlertaGeneral('Alerta', 'Ocurrio un error al consultar la información.');
                        break;
                }
            }).catch(error => Alert2Error(error.message));
        }
        async function fnDesautorizar() {
            var obj = {};
            obj.id = _incidenciaObj.id;
            //obj.id_incidencia = _incidenciaObj.id_incidencia;
            response = await ejectFetchJson(desAuthIncidencia, { obj });
            if (response.success) {
                modalDesAuthIncidencia.modal("hide");
                btnBonIncBuscar.click();
            }
        }
        async function fnAuthIncidencia() {
            if (_evaluacion_pendiente) {
                modalAuthIncidencia_evPendiente.modal("show");
            }
            else {
                var obj = {};
                obj.id = _incidenciaObj.id;
                //obj.id_incidencia = _incidenciaObj.id_incidencia;
                response = await ejectFetchJson(authIncidencia, { obj });
                if (response.success) {
                    dtAuth.clear().draw();
                    modalAuthIncidencia.modal("hide");
                    btnBonIncBuscar.click();
                }
            }
        }
        async function fnAuthIncidencia_evPendiente() {
            var obj = {};
            obj.id = _incidenciaObj.id;
            //obj.id_incidencia = _incidenciaObj.id_incidencia;
            response = await ejectFetchJson(authIncidencia, { obj });
            if (response.success) {
                dtAuth.clear().draw();
                modalAuthIncidencia_evPendiente.modal("hide");
                modalAuthIncidencia.modal("hide");
                btnBonIncBuscar.click();
            }
        }
        async function fnAutorizaIncidencia() {

            var obj = {};
            obj.id = _incidenciaObj.id;
            let _anio = _incidenciaObj.id;
            let _periodo = _incidenciaObj.periodo;
            let _tipo_nomina = _incidenciaObj.tipo_nomina;
            let _cc = _incidenciaObj.cc;

            //obj.id_incidencia = _incidenciaObj.id_incidencia;

            axios.post("RevisarFechaCierre", { lst: [obj] }).then(response => {
                let { success, items, message } = response.data;
                switch (response.data.estadoIncidencias) {
                    case 0:
                        AlertaGeneral('Alerta', 'Algunas incidencias que intenta desautorizar corresponden a perdiodos cerrados.');
                        break;
                    case 1:
                        axios.post("getIncidenciaAuth", { incidenciaID: _incidenciaObj.id, anio: _anio, periodo: _periodo, tipo_nomina: _tipo_nomina, cc: _cc }).then(response => {
                            if (response.data.success) {
                                _evaluacion_pendiente = response.data.evaluacion_pendiente;
                                dtAuth.clear().draw();
                                dtAuth.rows.add(response.data.datos).draw();
                                if (!response.data.completa) {
                                    btnAuth.show();
                                }
                                else {
                                    btnAuth.hide();
                                }
                            }
                        }).catch(error => Alert2Error(error.message));
                        modalAuthIncidencia.modal("show");
                        break;
                    case 3:
                        AlertaGeneral('Alerta', 'Se detectaron empleados sin captura registrada para el centro de costo que desea autorizar.');
                        break;
                    default:
                        AlertaGeneral('Alerta', 'Ocurrio un error al consultar la información.');
                        break;
                }
            }).catch(error => Alert2Error(error.message));
        }
        function initDataTblExcluidos() {
            dtAuth = tblAuth.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Lista de Incidencia", "<center><h3>Lista de incidencia</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    }
                ],
                columns: [
                    { title: 'Cve', data: 'clave_empleado' }
                    , { title: 'Nombre', data: 'nombre' }
                    , { title: 'A. Paterno', data: 'ape_paterno' }
                    , { title: 'A. Materno', data: 'ape_materno' }
                    , { title: 'Cve puesto', data: 'puesto' }
                    , { title: 'Puesto', data: 'puestoDesc' }
                    , { title: 'Departamento', data: 'deptoDesc' }
                    , { title: 'Bono Desempeño Personal', data: 'bono' }
                    , { title: 'Bono Desempeño Mensual', data: 'bonoDM' }
                    , { title: 'Horas Extra', data: 'totalo_Horas' }
                    , { title: 'Dias Extras', data: 'dias_extras' }
                    , { title: 'Total Dias', data: 'total_Dias' }
                    , {
                        title: 'Estatus captura', data: 'estatus', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(rowData.estatus ? "CAPTURADA" : "PENDIENTE");
                            if (!rowData.estatus) {
                                $(td).addClass("capturaPendiente");
                            }
                        }
                    }
                ]
            });
        }
        function fnSetObservacion() {
            var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == clave_empleado_actual);
            if (txtObservacion.val() != '') {
                botonObsActual.removeClass("btn-success");
                botonObsActual.addClass("btn-success");

                if (observacionesTemp != undefined) {
                    observacionesTemp.observaciones = txtObservacion.val();
                }
                else {
                    var o = [];
                    o.clave_empleado = clave_empleado_actual;
                    o.observaciones = txtObservacion.val();
                    arrayObservaciones.push(o);
                }
            }
            else {
                if (arrayObservaciones != undefined) {
                    $.each(arrayObservaciones, function (i, el) {
                        if (this.clave_empleado == clave_empleado_actual) {
                            arrayObservaciones.splice(i, 1);
                        }
                    });
                }

                botonObsActual.removeClass("btn-success");
            }
            $('#modalObservaciones .close').click();
        }
        function fnSetObservacionBono() {
            var observacionesBonoTemp = arrayObservacionesBono.find(x => x.clave_empleado == clave_empleado_actual);
            if (txtObservacionBono.val() != '') {
                botonObsBonoActual.removeClass("btn-success");
                botonObsBonoActual.removeClass("btn-danger");
                botonObsBonoActual.addClass("btn-success");

                if (observacionesBonoTemp != undefined) {
                    observacionesBonoTemp.observacionesBono = txtObservacionBono.val();
                }
                else {
                    var o = [];
                    o.clave_empleado = clave_empleado_actual;
                    o.observacionesBono = txtObservacionBono.val();
                    arrayObservacionesBono.push(o);
                }
            }
            else {
                if (arrayObservacionesBono != undefined) {
                    $.each(arrayObservacionesBono, function (i, el) {
                        if (this.clave_empleado == clave_empleado_actual) {
                            arrayObservacionesBono.splice(i, 1);
                        }
                    });
                }

                botonObsBonoActual.removeClass("btn-success");
                botonObsBonoActual.addClass("btn-danger");
            }
            $('#modalObservacionesBono .close').click();
        }
        //function fnGuardar(){
        //    if(readyToSave)
        //    {
        //        //GuardarIncidencia
        //        AlertaGeneral("Confirmación","Información guardada correctamente!");
        //    }
        //    else{
        //        AlertaGeneral("Alerta","Primero debe capturar o actualizar una incidencia para continuar!");
        //    }
        //}
        async function fnGuardar() {
            try {
                if (readyToSave) {
                    // datos: JSON.stringify(getDataToSave()),
                    // strClaveEmpleados: strClaveEmpleados,
                    // response = await ejectFetchJson(GuardarIncidencia, data, { headers: { 'Content-Type': 'multipart/form-data' } });
                    data.append('datos', JSON.stringify(getDataToSave()));
                    data.append('strClaveEmpleados', strClaveEmpleados);

                    axios.post("GuardarIncidencia", data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha registrado con éxito.");
                            btnBonIncBuscar.click();
                            data.forEach(function (val, key, fD) {
                                data.delete(key)
                            });
                        } else {
                            AlertaGeneral("Alerta", "Ocurrio un error, favor de contactar al depto de TI!");
                        }
                    }).catch(error => Alert2Error(error.message));

                    // if (response.success) {
                    //     AlertaGeneral("Confirmación", "Información guardada correctamente!");
                    //     btnBonIncBuscar.click();
                    // } else {
                    //     AlertaGeneral("Alerta", "Ocurrio un error, favor de contactar al depto de TI!");
                    // }
                }
                else {
                    AlertaGeneral("Alerta", "Primero debe capturar o actualizar una incidencia para continuar!");
                }

            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }

        function getDataToSave() {
            var paquete = {};
            var obj = {};
            obj.id = _incidenciaObj.id;
            //obj.id_incidencia = _incidenciaObj.id_incidencia;
            var det = [];
            var detPeru = [];
            dtBonIncidencias.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let node = $(this.node());
                let data = this.data();
                var observacionesBonoTemp = arrayObservacionesBono.find(x => x.clave_empleado == data.clave_empleado);
                var observacionesBono = observacionesBonoTemp == undefined ? "" : observacionesBonoTemp.observacionesBono;

                var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == data.clave_empleado);
                var observaciones = observacionesTemp == undefined ? "" : observacionesTemp.observaciones;

                var o = {};
                var p = {};
                o.id = data.id;
                o.nombre = data.nombre;
                o.ape_paterno = data.ape_paterno;
                o.ape_materno = data.ape_materno;
                o.puesto = data.puesto;
                o.puestoDesc = data.puestoDesc;
                o.incidenciaID = data.incidenciaID;
                //o.id_incidencia = data.id_incidencia;
                o.clave_empleado = data.clave_empleado;
                o.clave_depto = data.clave_depto;
                o.deptoDesc = data.deptoDesc;
                o.dia1 = node.find('._dia_1').val();
                o.dia2 = node.find('._dia_2').val();
                o.dia3 = node.find('._dia_3').val();
                o.dia4 = node.find('._dia_4').val();
                o.dia5 = node.find('._dia_5').val();
                o.dia6 = node.find('._dia_6').val();
                o.dia7 = node.find('._dia_7').val();
                o.dia8 = $('._dia_8').length > 0 ? node.find('._dia_8').val() : 0;
                o.dia9 = $('._dia_9').length > 0 ? node.find('._dia_9').val() : 0;
                o.dia10 = $('._dia_10').length > 0 ? node.find('._dia_10').val() : 0;
                o.dia11 = $('._dia_11').length > 0 ? node.find('._dia_11').val() : 0;
                o.dia12 = $('._dia_12').length > 0 ? node.find('._dia_12').val() : 0;
                o.dia13 = $('._dia_13').length > 0 ? node.find('._dia_13').val() : 0;
                o.dia14 = $('._dia_14').length > 0 ? node.find('._dia_14').val() : 0;
                o.dia15 = $('._dia_15').length > 0 ? node.find('._dia_15').val() : 0;
                o.dia16 = $('._dia_16').length > 0 ? node.find('._dia_16').val() : 0;
                o.he_dia1 = data.totalo_Horas;
                o.he_dia2 = 0;
                o.he_dia3 = 0;
                o.he_dia4 = 0;
                o.he_dia5 = 0;
                o.he_dia6 = 0;
                o.he_dia7 = 0;
                o.he_dia8 = 0;
                o.he_dia9 = 0;
                o.he_dia10 = 0;
                o.he_dia11 = 0;
                o.he_dia12 = 0;
                o.he_dia13 = 0;
                o.he_dia14 = 0;
                o.he_dia15 = 0;
                o.he_dia16 = 0;
                o.bono = data.bono;
                o.observaciones = observaciones;
                o.archivo_enviado = 0;
                o.dias_extras = node.find('._diaExtra').getVal(0);
                o.dias_extra_concepto = node.find('._diaExtra_Concepto').getVal(0);
                o.prima_dominical = 0;
                o.bonoDM = node.find('._bonoDM').getVal(2);
                o.bonoDM_Obs = null;
                //o.bonoU = node.find('._bonoUnico').getVal(2);
                o.total_Dias = node.find('._totalDias').getVal(0);
                o.totalo_Horas = data.totalo_Horas;
                o.horas_extras = data.totalo_Horas;
                o.bono_Obs = observacionesBono;
                o.estatus = true;
                o.primaDominical = data.primaDominical;

                switch (_EMPRESA_ACTUAL) {
                    case "6":
                        o.horas_extra_25 = node.find('._horasExtra25').getVal(0);
                        o.horas_extra_35 = node.find('._horasExtra35').getVal(0);
                        o.horas_extra_60 = node.find('._horasExtra60').getVal(0);
                        o.horas_extra_100 = node.find('._horasExtra100').getVal(0);
                        o.quinta_externa = node.find('._quintaExt').getVal(0);
                        o.minutos_tardanza = node.find('._minTardanza').getVal(0);
                        o.horas_lactancia = node.find('._horasLactancia').getVal(0);
                        o.horas_nocturnas = node.find("._horas_nocturnas").getVal(0);
                        break;

                    default:
                        o.horas_extra_25 = 0;
                        o.horas_extra_35 = 0;
                        o.horas_extra_60 = 0;
                        o.horas_extra_100 = 0;
                        o.quinta_externa = 0;
                        o.minutos_tardanza = 0;
                        o.horas_lactancia = 0;
                        break;
                }

                p.dia1 = node.find('._dia_1_ht').val();
                p.dia2 = node.find('._dia_2_ht').val();
                p.dia3 = node.find('._dia_3_ht').val();
                p.dia4 = node.find('._dia_4_ht').val();
                p.dia5 = node.find('._dia_5_ht').val();
                p.dia6 = node.find('._dia_6_ht').val();
                p.dia7 = node.find('._dia_7_ht').val();
                p.dia8 = $('._dia_8_ht').length > 0 ? node.find('._dia_8_ht').val() : 0;
                p.dia9 = $('._dia_9_ht').length > 0 ? node.find('._dia_9_ht').val() : 0;
                p.dia10 = $('._dia_10_ht').length > 0 ? node.find('._dia_10_ht').val() : 0;
                p.dia11 = $('._dia_11_ht').length > 0 ? node.find('._dia_11_ht').val() : 0;
                p.dia12 = $('._dia_12_ht').length > 0 ? node.find('._dia_12_ht').val() : 0;
                p.dia13 = $('._dia_13_ht').length > 0 ? node.find('._dia_13_ht').val() : 0;
                p.dia14 = $('._dia_14_ht').length > 0 ? node.find('._dia_14_ht').val() : 0;
                p.dia15 = $('._dia_15_ht').length > 0 ? node.find('._dia_15_ht').val() : 0;
                p.dia16 = $('._dia_16_ht').length > 0 ? node.find('._dia_16_ht').val() : 0;
                p.clave_empleado = data.clave_empleado;
                p.incidenciaID = data.incidenciaID;
                p.incidencia_detID = data.id;

                det.push(o);
                detPeru.push(p);
            });
            let busq = getFormNoDates();

            paquete.busq = busq;
            paquete.incidencia = obj;
            paquete.incidencia_det = det;
            paquete.incidencia_det_Peru = detPeru;

            return paquete;
        }
        async function setLstIncidencias() {
            //try {

            dtBonIncidencias.clear().draw();

            let busq = getForm();
            response = await ejectFetchJson(getLstIncidencias, busq);
            if (response.success) {
                arrayObservaciones = [];
                arrayObservacionesBono = [];
                var data = JSON.parse(response.lst);
                permiso_bono_sinlimite = data.permiso_bono_sinlimite;
                _incidenciaObj = data.incidencia;
                _incidenciaObjPeru = data.incidencia_det_Peru;
                //setFormInfo(data.datos[0]);
                setFormInfo(data.incidencia);
                dtBonIncidencias.destroy();
                initDataTblBonIncidencias();
                //dtBonIncidencias.rows.add(data.datos).draw();
                dtBonIncidencias.rows.add(data.incidencia_det).draw();
                setTotalTotal();
                var tabindex = 1;
                var rowsLength = $($("#tblBonIncidencias").find('tr')).length
                var colsLength = $($("#tblBonIncidencias tr:first-child").find('.inputtab')).length
                var rowsLengthReal = rowsLength - 1;

                for (var i = 0; i < colsLength; i++) {
                    for (var j = 1; j < rowsLength; j++) {
                        var c = $($($("#tblBonIncidencias tr").eq(j)[0]).find('.inputtab')[i]);
                        c.attr('tabindex', tabindex++);
                    }
                }
                //arrayObservaciones = 
                //arrayObservacionesBono.push(o);
                //if (_incidenciaObj.id == 0) {
                //    btnGuardar.hide();
                //    btnModalDesautorizar.hide();
                //    btnBonIncAuth.hide();
                //    AlertaGeneral("Alerta", "Esta Incidencia fue capturada desde enkontrol por tal no se puede modificar en SIGOPLAN");
                //}
                //else {
                //    btnModalDesautorizar.hide();
                //    btnGuardar.show();
                //}

                if (_incidenciaObj.estatus == 'A' || _incidenciaObj.estatus == 'C') {
                    btnBonIncAuth.hide();
                    btnGuardar.hide();
                    if (data.isDesauth) {
                        btnModalDesautorizar.show();
                    }
                    else {
                        btnModalDesautorizar.hide();
                    }
                }
                else if (data.isAuth) {
                    btnBonIncAuth.show();
                    btnGuardar.show();
                    btnModalDesautorizar.hide();
                }
                else {
                    btnBonIncAuth.hide();
                    btnModalDesautorizar.hide();
                    btnGuardar.show();
                }


                readyToSave = true;
            }
            //} catch (o_O) {
            //    readyToSave = false;
            //    //AlertaGeneral("",o_O.message) 
            //}
        }
        async function setLstIncidenciasEnkontrol() {
            try {

                dtBonIncidencias.clear().draw();

                let busq = getForm();
                response = await ejectFetchJson(getLstIncidenciasEnkontrol, busq);
                if (response.success) {
                    var data = response.lst;
                    _incidenciaObj = data.incidencia;
                    _incidenciaObjPeru = data.incidencia_det_Peru;
                    //setFormInfo(data.datos[0]);
                    setFormInfo(data.incidencia);
                    dtBonIncidencias.destroy();
                    initDataTblBonIncidencias();
                    //dtBonIncidencias.rows.add(data.datos).draw();
                    dtBonIncidencias.rows.add(data.incidencia_det).draw();
                    setTotalTotal();
                    readyToSave = true;
                }
            } catch (o_O) {
                readyToSave = false;
                //AlertaGeneral("",o_O.message) 
            }
        }
        function initDataTblBonIncidencias() {
            dtBonIncidencias = tblBonIncidencias.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: _EMPRESA_ACTUAL == 6 ? createCellBonIncidenciasPERU() : createCellBonIncidencias(),
                initComplete: function (settings, json) {
                    tblBonIncidencias.on('change', '.totalAsist', function (event) {
                        setTotalAsist($(this));
                    });
                    tblBonIncidencias.on('click', '._btnDesglose', function (event) {
                        let rowData = dtBonIncidencias.row($(this).closest('tr')).data();
                        mdlExtraTemp.modal("show");

                        dtExtraTemp.clear();
                        dtExtraTemp.rows.add(rowData.lstFechasExtratemporaneas);
                        dtExtraTemp.draw();
                    });
                    tblBonIncidencias.on('click', '._btnEvidencia', function (event) {
                        let rowData = dtBonIncidencias.row($(this).closest('tr')).data();
                        txtArchivoEvidencia.val("");
                        btnGuardarArchivoEvidencia.data().clave_empleado = rowData.clave_empleado;
                        mdlEvidencia.modal("show");
                    });
                },
                fixedColumns: {
                    leftColumns: 1
                },
                buttons: [
                    $.extend(true, {}, buttonCommon, {
                        extend: 'excelHtml5'
                    }),
                    $.extend(true, {}, buttonCommon, {
                        extend: 'pdfHtml5',
                        orientation: 'landscape',
                        customize: function (doc) {
                            doc.defaultStyle.fontSize = 7; //<-- set fontsize to 16 instead of 10 
                            doc.styles.tableHeader.fontSize = 7;
                        }
                    })
                ],
                scrollY: '50vh',
                scrollCollapse: true,
                scrollX: true,
                "drawCallback": function (settings) {
                    $(".dext").change();
                }
            });
        }
        function createCellBonIncidencias() {

            let lstDia = []
                , tipoNomina = +selBonIncTipoNomina.val()
                , periodo = periodosData.find(per => per.tipo_nomina === tipoNomina)
                , fecha = periodo.fecha_inicial;

            lstDia.push({ data: 'clave_empleado', title: 'Empleado', createdCell: (td, data, rowData, row, col) => $(td).html(`<div class='_empleado ${rowData.estatus ? "" : "capturaPendiente"}' data-cve='${data}'><p>${data} - ${rowData.ape_paterno} ${rowData.ape_materno} ${rowData.nombre}</p><p>${rowData.puestoDesc}</p></div>`) });
            //lstDia.push({ title:'Clasif.' ,render: td => `<div><p>Asist.</p><p>Hrs.Extra</p></div>` });
            var newFecha = new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());
            var estatusIncidencia = (inpBonIncEstatus.val() == 'ACEPTADA' || inpBonIncEstatus.val() == 'CANCELADA') ? true : false;
            if (estatusIncidencia) {
                btnSetObervacion.hide();
                btnSetObervacionBono.hide();
                //btnBonIncAuth.hide();
                //btnGuardar.hide();
            }
            else {
                btnSetObervacion.show();
                btnSetObervacionBono.show();
                //btnBonIncAuth.show();
                //btnGuardar.show();
            }
            for (let i = 1; i < 17; i++) {
                var dia = `${primeraLetraMayuscula(newFecha.toLocaleDateString("es-ES", { weekday: 'long' }).substr(0, 3))}`;
                let strFecha = `${primeraLetraMayuscula(newFecha.toLocaleDateString("es-ES", { weekday: 'long' }).substr(0, 3))}<br>${newFecha.getDate()}`;

                let esVisible = periodo.fecha_final >= newFecha;
                lstDia.push({
                    data: `he_dia${i}`, class: `${dia}`, title: `${strFecha}`, visible: esVisible, createdCell: (td, data, rowData, row, col) => {
                        $(td).html(`<div><p><select data-empleado='${rowData.clave_empleado}'></p><p><input></p><p><input data-empleado='${rowData.clave_empleado}'></p></div>`);
                        $(td).find('select').addClass(`form-control inputtab totalAsist _dia_${i}`);
                        $(td).find('input')[0].addClass(`form-control totalExtra hidden`);
                        $(td).find('input')[1].addClass(`form-control _horasTrabajadas`);

                        var tipo = rowData[`dia${i}`];
                        if (td.className == ' Dom' || td.className == ' Sáb') {
                            tipo = (tipo == 0 ? 16 : tipo);
                        }
                        else {
                            tipo = (tipo == 0 ? 1 : tipo);
                        }
                        $(td).find('select').fillComboItems(itemBonIncConp, null, tipo, true);

                        if (estatusIncidencia || tipo == 19 || tipo == 20) {
                            $(td).find('select').prop('disabled', true);
                        }

                    }
                });
                newFecha.setDate(newFecha.getDate() + 1);

            }
            lstDia.push({
                data: 'numDiasExtratemporales', title: 'D. Extratemporales', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalAsist dext _numDiasExtratemporales`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    $(td).find('input').prop('readonly', true);
                }
            });
            lstDia.push({
                data: 'dias_extras', title: 'D.Extras', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalAsist dext _diaExtra`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'dias_extra_concepto', title: 'D.Concepto', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}' readonly></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalExtraConcepto dext _diaExtra_Concepto`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            //lstDia.push({
            //    data:'prima_dominical' ,title: 'P.Dominical' ,createdCell: function (td, data, rowData, row, col) {
            //        $(td).html(`<input>`);
            //        $(td).find(`input`).addClass(`form-control`);
            //        $(td).find(`input`).val(maskNumero2D(0));
            //    }
            //});
            lstDia.push({
                data: 'primaDominical', title: 'P.Dominical', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<input type="checkbox">`);
                    $(td).find(`input`).addClass(`form-control _primaDominical`);
                    if (data) $(td).find(`input`).prop("checked", true);
                    else $(td).find(`input`).prop("checked", false);
                    if (estatusIncidencia || $.inArray(selBonIncCc.val(), ccAplicaPrimaDominical) < 0) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'bono', title: 'Bono<br/>Desempeño<br/>Personal', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab bonoDP _bonoDP _bonoPersonal`);
                    //$(td).find(`input`).DecimalFixPs(2);

                    $(td).find(`input`).val(data);

                    $(td).find(`input`).data('clave_empleado', rowData.clave_empleado);

                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);

                    }
                    else if (permiso_bono_sinlimite) {
                        $(td).find('input').prop('disabled', false);
                    }
                    else if (rowData.countBonosPersonales == 2 && $.inArray(selBonIncCc.val(), ccAplicaPrimaDominical) < 0) {
                        $(td).find('input').prop('disabled', true);
                        $(td).find('input').css('backgroundColor', 'red');
                        $(td).find('input').css('color', 'white');
                        $(td).find('input').prop('title', 'No se puede otorgar bono en 3 periodos seguidos, el empleado ya tiene bono otorgado en los 2 periodos anteriores');

                    }
                }
            });
            lstDia.push({
                data: 'bono_Obs', title: 'Obs<br/>Bono<br/>Personal', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    var claseBono = 'obsBono' + rowData.clave_empleado;
                    $(td).find(`button`).addClass(`btn btn-sm btn-primary btnObsBono ${claseBono} _obsBonoDP`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);
                    $(td).find(`button`).click(function () {
                        _this = $(this);
                        clave_empleado_actual = _this.data('clave_empleado');
                        botonObsBonoActual = _this;
                        var observacionesBonoTemp = arrayObservacionesBono.find(x => x.clave_empleado == clave_empleado_actual);
                        var observacionesBono = observacionesBonoTemp == undefined ? "" : observacionesBonoTemp.observacionesBono;
                        txtObservacionBono.val(observacionesBono);
                        if (estatusIncidencia) {
                            txtObservacionBono.prop('disabled', true);
                        }
                        modalObservacionesBono.modal("show");
                    });
                    if (rowData.bono_Obs != undefined && rowData.bono_Obs != '') {
                        var o = [];
                        o.clave_empleado = rowData.clave_empleado;
                        o.observacionesBono = rowData.bono_Obs;
                        arrayObservacionesBono.push(o);
                        $(td).find(`button`).addClass(`btn-success`);
                    }
                    else {
                        $(td).find(`button`).prop('disabled', true);
                        $(td).find(`button`).removeClass("btn-primary");
                        $(td).find(`button`).addClass("btn-light");
                    }

                }
            });
            lstDia.push({
                data: 'bonoDM', title: 'Bono<br/>Desempeño<br/>Mensual', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input readonly></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab bonoDM _bonoDM`);
                    $(td).find(`input`).DecimalFixPs(2);
                    $(td).find(`input`).setVal(data);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            //lstDia.push({
            //    data: 'bonoU', title: 'Bono<br/>Unico', createdCell: function (td, data, rowData, row, col) {
            //        $(td).html(`<div><p><input readonly></p></div>`);
            //        $(td).find(`input`).addClass(`form-control inputtab bonoPE _bonoUnico`);
            //        $(td).find(`input`).DecimalFixPs(2);
            //        $(td).find(`input`).setVal(data);
            //        if (estatusIncidencia) {
            //            $(td).find('input').prop('disabled', true);
            //        }
            //    }
            //});
            lstDia.push({
                data: 'archivo_enviado', title: 'Total<br/>Dias', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab _totalDias`);
                    $($(td).find(`input`)).addClass(`totalAsistTotal`);

                    $(td).find(`input`).prop('disabled', true);
                    $(td).find(`input`).DecimalFixNS(0);
                    $(td).find(`input`).setVal(0);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'totalo_Horas', title: 'Total<br/>Horas', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab _totalHoras`);
                    $($(td).find(`input`)).addClass(`totalHrsExtraTotal`)
                    $(td).find(`input`).prop('disabled', false);
                    $(td).find(`input`).DecimalFixNS(2);
                    $(td).find(`input`).setVal(data);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                    if (tipoNomina != 1) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'observaciones', title: 'Obs<br/>Incidencia', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    $(td).find(`button`).addClass(`btn btn-sm btn-primary btnObs _obsIncidencia`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);

                    if (estatusIncidencia) {
                        if (rowData.observaciones != undefined && rowData.observaciones != '') {

                            var o = [];
                            o.clave_empleado = rowData.clave_empleado;
                            o.observaciones = rowData.observaciones;
                            arrayObservaciones.push(o);
                            $(td).find(`button`).addClass(`btn-success`);
                            $(td).find(`button`).click(function () {
                                _this = $(this);
                                clave_empleado_actual = _this.data('clave_empleado');
                                botonObsActual = _this;
                                var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == clave_empleado_actual);
                                var observaciones = observacionesTemp == undefined ? "" : observacionesTemp.observaciones;
                                txtObservacion.val(observaciones);
                                if (estatusIncidencia) {
                                    txtObservacion.prop('disabled', true);
                                }
                                modalObservaciones.modal("show");
                            });
                        }
                        else {
                            $(td).find(`button`).prop('disabled', true);
                            $(td).find(`button`).removeClass("btn-primary");
                            $(td).find(`button`).addClass("btn-light");
                        }
                        $(td).find('input').prop('disabled', true);
                    }
                    else {
                        if (rowData.observaciones != undefined && rowData.observaciones != '') {

                            var o = [];
                            o.clave_empleado = rowData.clave_empleado;
                            o.observaciones = rowData.observaciones;
                            arrayObservaciones.push(o);
                            $(td).find(`button`).addClass(`btn-success`);
                        }

                        $(td).find(`button`).click(function () {
                            _this = $(this);
                            clave_empleado_actual = _this.data('clave_empleado');
                            botonObsActual = _this;
                            var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == clave_empleado_actual);
                            var observaciones = observacionesTemp == undefined ? "" : observacionesTemp.observaciones;
                            txtObservacion.val(observaciones);
                            if (estatusIncidencia) {
                                txtObservacion.prop('disabled', true);
                            }
                            modalObservaciones.modal("show");
                        });
                    }
                }
            });
            lstDia.push({
                data: 'lstFechasExtratemporaneas', title: 'Resumen ExtraTemp', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    $(td).find(`button`).addClass(`btn btn-sm btn-default _btnDesglose`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);
                }
            });
            return lstDia;
        }

        function createCellBonIncidenciasPERU() {
            //#region PERU
            let visible = +selBonIncTipoNomina.val() == 27 ? false : true;

            let lstDia = [],
                tipoNomina = +selBonIncTipoNomina.val(),
                periodo = periodosData.find(per => per.tipo_nomina === tipoNomina),
                fecha = periodo.fecha_inicial;

            let nombreCompleto = "";
            lstDia.push({
                data: 'clave_empleado', title: 'Empleado',
                createdCell: (td, data, rowData, row, col) => {
                    $(td).html(`<div class='_empleado ${rowData.estatus ? "" : "capturaPendiente"}' data-cve='${data}'><p>${data} - ${rowData.ape_paterno} ${rowData.ape_materno} ${rowData.nombre}</p><p>${rowData.puestoDesc}</p></div>`)
                    nombreCompleto = `${rowData.ape_paterno} ${rowData.ape_materno} ${rowData.nombre}`;
                }
            });

            var newFecha = new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());
            var estatusIncidencia = (inpBonIncEstatus.val() == 'ACEPTADA' || inpBonIncEstatus.val() == 'CANCELADA') ? true : false;
            if (estatusIncidencia) {
                btnSetObervacion.hide();
                btnSetObervacionBono.hide();
            }
            else {
                btnSetObervacion.show();
                btnSetObervacionBono.show();
            }

            for (let i = 1; i < 17; i++) {
                var dia = `${primeraLetraMayuscula(newFecha.toLocaleDateString("es-ES", { weekday: 'long' }))}`;
                let strFecha = `${primeraLetraMayuscula(newFecha.toLocaleDateString("es-ES", { weekday: 'long' }))}&nbsp;${newFecha.getDate()}`;

                let esVisible = periodo.fecha_final >= newFecha;


                lstDia.push({
                    data: `he_dia${i}`, class: `${dia}`, title: `${strFecha}`, visible: esVisible, createdCell: (td, data, rowData, row, col) => {
                        let horasTrabajadasPeru = null;
                        if (_incidenciaObjPeru != null) horasTrabajadasPeru = _incidenciaObjPeru.find(x => x.clave_empleado === rowData.clave_empleado);
                        var horasTrabajadasDiaActual = 8;
                        switch (i) {
                            case 1: horasTrabajadasDiaActual = horasTrabajadasPeru.dia1; break;
                            case 2: horasTrabajadasDiaActual = horasTrabajadasPeru.dia2; break;
                            case 3: horasTrabajadasDiaActual = horasTrabajadasPeru.dia3; break;
                            case 4: horasTrabajadasDiaActual = horasTrabajadasPeru.dia4; break;
                            case 5: horasTrabajadasDiaActual = horasTrabajadasPeru.dia5; break;
                            case 6: horasTrabajadasDiaActual = horasTrabajadasPeru.dia6; break;
                            case 7: horasTrabajadasDiaActual = horasTrabajadasPeru.dia7; break;
                            case 8: horasTrabajadasDiaActual = horasTrabajadasPeru.dia8; break;
                            case 9: horasTrabajadasDiaActual = horasTrabajadasPeru.dia9; break;
                            case 10: horasTrabajadasDiaActual = horasTrabajadasPeru.dia10; break;
                            case 11: horasTrabajadasDiaActual = horasTrabajadasPeru.dia11; break;
                            case 12: horasTrabajadasDiaActual = horasTrabajadasPeru.dia12; break;
                            case 13: horasTrabajadasDiaActual = horasTrabajadasPeru.dia13; break;
                            case 14: horasTrabajadasDiaActual = horasTrabajadasPeru.dia14; break;
                            case 15: horasTrabajadasDiaActual = horasTrabajadasPeru.dia15; break;
                            case 16: horasTrabajadasDiaActual = horasTrabajadasPeru.dia16; break;
                        }
                        $(td).html(`<div style='white-space: nowrap;'><p style='white-space: nowrap; display: inline-block;'><select style='white-space: nowrap; display: inline-block;' data-empleado='${rowData.clave_empleado}'><input style='white-space: nowrap; display: inline-block;' type='number' data-empleado='${rowData.clave_empleado}' value='${horasTrabajadasDiaActual}'></p><p><input></p></div>`);
                        $(td).find('select').addClass(`form-control inputtab totalAsist _dia_${i}`);
                        $($(td).find('input')[0]).addClass(`form-control _horasTrabajadas _dia_${i}_ht`);
                        $($(td).find('input')[1]).addClass(`form-control totalExtra hidden`);

                        var tipo = rowData[`dia${i}`];
                        if (td.className == ' Dom' || td.className == ' Sáb') {
                            tipo = (tipo == 0 ? 16 : tipo);
                        }
                        else {
                            tipo = (tipo == 0 ? 1 : tipo);
                        }

                        $(td).find('select').fillComboItems(itemBonIncConp, null, tipo, true); // TODO
                        cboFillLunes.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillMartes.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillMiercoles.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillJueves.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillViernes.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillSabado.fillComboItems(itemBonIncConp, null, tipo, true);
                        cboFillDomingo.fillComboItems(itemBonIncConp, null, tipo, true);
                        $(td).find('select').find('option').each(function () {
                            let title = "";
                            switch ($(this).text()) {
                                case "A": title = "Asistencia"; break;
                                case "F": title = "Falta"; break;
                                case "PS": title = "Permiso sin goce"; break;
                                case "PGS": title = "Permiso con goce de sueldo"; break;
                                case "VAC": title = "Vacaciones"; break;
                                case "SUSP": title = "Suspendido"; break;
                                case "HE": title = "Horas extra"; break;
                                case "PB": title = "Pagar bono"; break;
                                case "DF": title = "Día festivo"; break;
                                case "INC": title = "Incapacidad"; break;
                                case "NODES": title = "Descanso (NO USAR)"; break;
                                case "CT": title = "Comisión de trabajo"; break;
                                case "N/A": title = "No aplica"; break;
                                case "DP": title = "Descanso pagado"; break;
                                case "DL": title = "Descanso laborado"; break;
                                case "B": title = "Baja"; break;
                                case "DM": title = "Descanso medico"; break;
                                case "SUB": title = "Subsidio"; break;
                            }
                            $(this).attr("title", title);

                            // let diaColumna = "";
                            // switch (td.className.trim()) {
                            //     case "Lunes": diaColumna = "diaLunes"; break;
                            //     case "Martes": diaColumna = "diaMartes"; break;
                            //     case "Miércoles": diaColumna = "diaMiercoles"; break;
                            //     case "Jueves": diaColumna = "diaJueves"; break;
                            //     case "Viernes": diaColumna = "diaViernes"; break;
                            //     case "Sábado": diaColumna = "diaSabado"; break;
                            //     case "Domingo": diaColumna = "diaDomingo"; break;
                            // }
                            // $(this).addClass(diaColumna);
                        });

                        if (estatusIncidencia) {
                            $(td).find('select').prop('disabled', true);
                        }
                        if (estatusIncidencia) {
                            $(td).find('input').prop('disabled', true);
                        }

                    }
                });
                newFecha.setDate(newFecha.getDate() + 1);

            }

            lstDia.push({
                data: 'horas_extra_25', title: 'Horas Extra (25%)', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horasExtra25`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'horas_extra_35', title: 'Horas Extra (35%)', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horasExtra35`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'horas_extra_60', title: 'Horas Extras (60%)', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horasExtra60`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'horas_extra_100', title: 'Horas Extra (100%)', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horasExtra100`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'horas_nocturnas', title: 'Horas nocturnas', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horas_nocturnas`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'quinta_externa', title: 'Quinta Ext.', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _quintaExt`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'minutos_tardanza', title: 'Minutos Tardanza', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _minTardanza`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'horas_lactancia', title: 'Horas Lactan.', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab inputPeru _horasLactancia`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'numDiasExtratemporales', title: 'Dias Extra temp.', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalAsist dext _numDiasExtratemporales`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    $(td).find('input').prop('readonly', true);
                }
            });
            lstDia.push({
                data: 'dias_extras', title: 'Dias Extra', visible: visible, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalAsist dext _diaExtra`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'dias_extra_concepto', title: 'Dias Concepto', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}' readonly></p></div> `);
                    $(td).find(`input`).addClass(`form-control inputtab totalExtraConcepto dext _diaExtra_Concepto`);
                    $(td).find(`input`).val(maskNumero2D(data));
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            //lstDia.push({
            //    data:'prima_dominical' ,title: 'P.Dominical' ,createdCell: function (td, data, rowData, row, col) {
            //        $(td).html(`<input>`);
            //        $(td).find(`input`).addClass(`form-control`);
            //        $(td).find(`input`).val(maskNumero2D(0));
            //    }
            //});
            lstDia.push({
                data: 'primaDominical', title: 'Prima Dominical', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<input type="checkbox">`);
                    $(td).find(`input`).addClass(`form-control _primaDominical`);
                    if (data) $(td).find(`input`).prop("checked", true);
                    else $(td).find(`input`).prop("checked", false);
                    if (estatusIncidencia || $.inArray(selBonIncCc.val(), ccAplicaPrimaDominical) < 0) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'bono', title: 'Bono Desem. Personal', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab bonoDP _bonoDP _bonoPersonal`);
                    //$(td).find(`input`).DecimalFixPs(2);

                    $(td).find(`input`).val(data);

                    $(td).find(`input`).data('clave_empleado', rowData.clave_empleado);

                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);

                    }
                    else if (permiso_bono_sinlimite) {
                        $(td).find('input').prop('disabled', false);
                    }
                    else if (rowData.countBonosPersonales == 2 && $.inArray(selBonIncCc.val(), ccAplicaPrimaDominical) < 0) {
                        $(td).find('input').prop('disabled', true);
                        $(td).find('input').css('backgroundColor', 'red');
                        $(td).find('input').css('color', 'white');
                        $(td).find('input').prop('title', 'No se puede otorgar bono en 3 periodos seguidos, el empleado ya tiene bono otorgado en los 2 periodos anteriores');
                    }
                }
            });
            lstDia.push({
                data: 'bono_Obs', title: 'Obs. Bono Personal', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    var claseBono = 'obsBono' + rowData.clave_empleado;
                    $(td).find(`button`).addClass(`btn btn-sm btn-primary btnObsBono ${claseBono} _obsBonoDP`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);
                    $(td).find(`button`).click(function () {
                        _this = $(this);
                        clave_empleado_actual = _this.data('clave_empleado');
                        botonObsBonoActual = _this;
                        var observacionesBonoTemp = arrayObservacionesBono.find(x => x.clave_empleado == clave_empleado_actual);
                        var observacionesBono = observacionesBonoTemp == undefined ? "" : observacionesBonoTemp.observacionesBono;
                        txtObservacionBono.val(observacionesBono);
                        if (estatusIncidencia) {
                            txtObservacionBono.prop('disabled', true);
                        }
                        modalObservacionesBono.modal("show");
                    });
                    if (rowData.bono_Obs != undefined && rowData.bono_Obs != '') {
                        var o = [];
                        o.clave_empleado = rowData.clave_empleado;
                        o.observacionesBono = rowData.bono_Obs;
                        arrayObservacionesBono.push(o);
                        $(td).find(`button`).addClass(`btn-success`);
                    }
                    else {
                        $(td).find(`button`).prop('disabled', true);
                        $(td).find(`button`).removeClass("btn-primary");
                        $(td).find(`button`).addClass("btn-light");
                    }

                }
            });
            lstDia.push({
                data: 'bonoDM', title: 'Bono Desem. Mensual', visible: false, createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input readonly></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab bonoDM _bonoDM`);
                    $(td).find(`input`).DecimalFixPs(2);
                    $(td).find(`input`).setVal(data);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            //lstDia.push({
            //    data: 'bonoU', title: 'Bono<br/>Unico', createdCell: function (td, data, rowData, row, col) {
            //        $(td).html(`<div><p><input readonly></p></div>`);
            //        $(td).find(`input`).addClass(`form-control inputtab bonoPE _bonoUnico`);
            //        $(td).find(`input`).DecimalFixPs(2);
            //        $(td).find(`input`).setVal(data);
            //        if (estatusIncidencia) {
            //            $(td).find('input').prop('disabled', true);
            //        }
            //    }
            //});
            lstDia.push({
                data: 'archivo_enviado', title: 'Total Dias', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input data-empleado='${rowData.clave_empleado}'></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab _totalDias`);
                    $($(td).find(`input`)).addClass(`totalAsistTotal`);

                    $(td).find(`input`).prop('disabled', true);
                    $(td).find(`input`).DecimalFixNS(0);
                    $(td).find(`input`).setVal(0);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });
            lstDia.push({
                data: 'totalo_Horas', title: 'Total Horas', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><input></p></div>`);
                    $(td).find(`input`).addClass(`form-control inputtab _totalHoras`);
                    $($(td).find(`input`)).addClass(`totalHrsExtraTotal`)
                    $(td).find(`input`).prop('disabled', false);
                    $(td).find(`input`).DecimalFixNS(2);
                    $(td).find(`input`).setVal(data);
                    if (estatusIncidencia) {
                        $(td).find('input').prop('disabled', true);
                    }
                    if (tipoNomina != 1) {
                        $(td).find('input').prop('disabled', true);
                    }
                }
            });

            lstDia.push({
                data: 'observaciones', title: 'Obs Incidencia', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    $(td).find(`button`).addClass(`btn btn-sm btn-primary btnObs _obsIncidencia`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);

                    if (estatusIncidencia) {
                        if (rowData.observaciones != undefined && rowData.observaciones != '') {

                            var o = [];
                            o.clave_empleado = rowData.clave_empleado;
                            o.observaciones = rowData.observaciones;
                            arrayObservaciones.push(o);
                            $(td).find(`button`).addClass(`btn-success`);
                            $(td).find(`button`).click(function () {
                                _this = $(this);
                                clave_empleado_actual = _this.data('clave_empleado');
                                botonObsActual = _this;
                                var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == clave_empleado_actual);
                                var observaciones = observacionesTemp == undefined ? "" : observacionesTemp.observaciones;
                                txtObservacion.val(observaciones);
                                if (estatusIncidencia) {
                                    txtObservacion.prop('disabled', true);
                                }
                                modalObservaciones.modal("show");
                            });
                        }
                        else {
                            $(td).find(`button`).prop('disabled', true);
                            $(td).find(`button`).removeClass("btn-primary");
                            $(td).find(`button`).addClass("btn-light");
                        }
                        $(td).find('input').prop('disabled', true);
                    }
                    else {
                        if (rowData.observaciones != undefined && rowData.observaciones != '') {

                            var o = [];
                            o.clave_empleado = rowData.clave_empleado;
                            o.observaciones = rowData.observaciones;
                            arrayObservaciones.push(o);
                            $(td).find(`button`).addClass(`btn-success`);
                        }

                        $(td).find(`button`).click(function () {
                            _this = $(this);
                            clave_empleado_actual = _this.data('clave_empleado');
                            botonObsActual = _this;
                            var observacionesTemp = arrayObservaciones.find(x => x.clave_empleado == clave_empleado_actual);
                            var observaciones = observacionesTemp == undefined ? "" : observacionesTemp.observaciones;
                            txtObservacion.val(observaciones);
                            if (estatusIncidencia) {
                                txtObservacion.prop('disabled', true);
                            }
                            modalObservaciones.modal("show");
                        });
                    }


                }
            });
            lstDia.push({
                data: 'observaciones', title: 'Evidencia Incidencia', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-file'></i></button></p></div>`);
                    $(td).find(`button`).addClass(`btn btn-sm btn-default _btnEvidencia`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);
                }
            });
            lstDia.push({
                data: 'lstFechasExtratemporaneas', title: 'Resumen Extra Temp', createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<div><p><button><i class='fa fa-comments'></i></button></p></div>`);
                    $(td).find(`button`).addClass(`btn btn-sm btn-default _btnDesglose`);
                    $(td).find(`button`).data('clave_empleado', rowData.clave_empleado);
                }
            });
            lstDia.push({
                data: "nombre", title: "nombreCompleto", visible: false,
                createdCell: function (td, data, rowData, row, col) {
                    $(td).html(`<p>${rowData.nombre}</p>`)
                }
            })
            return lstDia;
            //#endregion

        }

        function setFormInfo({ empleado_modifica, nombreEmpMod, estatusDesc, nombreAutoriza }) {
            //inpBonIncId.val(id_incidencia);
            inpBonIncEmpMod.val(`${empleado_modifica} - ${nombreAutoriza}`);
            inpBonIncEstatus.val(estatusDesc);
        }
        async function setPeriodo() {
            try {
                var auxAnio = inpBonIncPeriodo.val() == null ? inpBonIncAnio.val() : $("#inpBonIncPeriodo option:selected").text().substr($("#inpBonIncPeriodo option:selected").text().length - 4);
                response = await ejectFetchJson(getPeriodo, {
                    anio: +auxAnio
                    , periodo: +inpBonIncPeriodo.val()
                });
                if (response.success) {
                    response.periodo.forEach(periodo => {
                        periodo.fecha_inicial = strToDate(periodo.fecha_inicial);
                        periodo.fecha_final = strToDate(periodo.fecha_final);
                        periodo.fecha_pago = strToDate(periodo.fecha_pago);
                    });
                    periodosData = response.periodo;
                    setSpnPeriodo();
                } else {
                    AlertaGeneral("Aviso", "No existe el periodo");
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function setCboIncidecnciaConcepto() {
            try {
                response = await ejectFetchJson(getCboIncidecnciaConcepto);
                if (response.success) {
                    itemBonIncConp = response.items;
                    getAsistenciaValue = id => +JSON.parse(itemBonIncConp.find(conc => id === conc.Value).Prefijo).asistencia;
                    getExtraValue = id => +(JSON.parse(itemBonIncConp.find(conc => id === conc.Value).Prefijo).asistencia == 2 ? id == 9 ? 2 : 1 : 0);
                }
            } catch (o_O) { }
        }

        function setSpnPeriodo() {
            let tipoNomina = +selBonIncTipoNomina.val()
                , periodo = periodosData.find(per => per.tipo_nomina === tipoNomina);
            inpBonIncPeriodo.fillCombo('/Administrativo/Bono/getPeriodos', { anio: inpBonIncAnio.val(), tipoNomina: selBonIncTipoNomina.val() }, true, null);
            if (periodo !== undefined) {
                inpBonIncPeriodo.val(periodo.periodo);
                spnBonIncPeriodoFechas.text(`Del ${periodo.fecha_inicialStr} al ${periodo.fecha_finalStr}`);
            }
            else {
                AlertaGeneral("Aviso", "No existe el periodo");
            }
        }
        function setSelDepto() {
            let valor = this.value;
            selBonIncDepto.html("");
            if (valor.length >= 3) {
                let items = selBonIncCc.find(`option[value="${valor}"]`).data().prefijo.cboDepto;
                selBonIncDepto.fillComboItems(items, null, items[0].Value);
            }
            setPeriodo();
            readyToSave = false;
        }
        function strToDate(str) {
            let lstFecha = $.toDate(str).split("/")
                , fecha = new Date(+lstFecha[2], +lstFecha[1] - 1, +lstFecha[0]);
            return fecha
        }
        function getForm() {
            var auxAnio = inpBonIncPeriodo.val() == null ? inpBonIncAnio.val() : $("#inpBonIncPeriodo option:selected").text().substr($("#inpBonIncPeriodo option:selected").text().length - 4);
            return {
                cc: selBonIncCc.val()
                , tipoNomina: +selBonIncTipoNomina.val()
                , anio: +auxAnio
                , periodo: +inpBonIncPeriodo.val()
                , depto: +selBonIncDepto.val()
                , fechaInicio: $("#inpBonIncPeriodo option:selected").data("comboid")
                , fechaFin: $("#inpBonIncPeriodo option:selected").data("prefijo")
                , stfechaInicio: $("#inpBonIncPeriodo option:selected").data("comboid")
                , stfechaFin: $("#inpBonIncPeriodo option:selected").data("prefijo")
            };
        }
        function getFormNoDates() {
            var auxAnio = inpBonIncPeriodo.val() == null ? inpBonIncAnio.val() : $("#inpBonIncPeriodo option:selected").text().substr($("#inpBonIncPeriodo option:selected").text().length - 4);
            return {
                cc: selBonIncCc.val()
                , tipoNomina: +selBonIncTipoNomina.val()
                , anio: +auxAnio
                , periodo: +inpBonIncPeriodo.val()
                , depto: +selBonIncDepto.val()
                , stfechaInicio: $("#inpBonIncPeriodo option:selected").data("comboid")
                , stfechaFin: $("#inpBonIncPeriodo option:selected").data("prefijo")
            };
        }
        function setTotalAsist(element) {
            var clave_empleado = $(element[0]).data("empleado");
            if (element.val() == "1") {
                element.next('input').prop("disabled", false);
                element.next('input').val("8");
            }
            else {
                element.next('input').prop("disabled", true);
                if (element.val() == "13") {
                    element.next('input').val("0");
                } else {
                    element.next('input').val("8");
                }
            }
            let row = element.closest('tr')
                , lstInp = row.find('select.totalAsist').toArray();
            //,total = lstInp.reduce((current ,inpt) => current + getAsistenciaValue(inpt.value), 0);

            var asistencias = 0;
            var extra_concepto = 0;
            $.each(lstInp, function (i, e) {
                if ($(e).hasClass('_dia_16')) {
                    asistencias += getAsistenciaValue(e.value) == 0 ? -1 : 0;
                }
                else {
                    asistencias += getAsistenciaValue(e.value);
                }

                extra_concepto += getExtraValue(e.value);

                if (e.value == "9") {
                    asistencias += 1;
                }
            })

            let numExtras = 0;
            let lstInpNumeros = row.find('input.totalAsist').toArray();

            for (const item of lstInpNumeros) {
                numExtras += +$(item).val();
            }

            asistencias += numExtras

            asistencias = asistencias > 0 ? asistencias : 0;
            $(`.totalExtraConcepto[data-empleado=${clave_empleado}]`).setVal(extra_concepto);
            $(`.totalAsistTotal[data-empleado=${clave_empleado}]`).setVal(asistencias);
        }

        function setTotalExtra(element) {
            let row = element.closest('tr')
                , lstInp = row.find('.totalExtra').toArray()
                , total = lstInp.reduce((current, inpt) => current + +inpt.value, 0);
            row.find('.totalExtraTotal').val(maskNumero2D(total));
        }
        function esBonoObervado(element) {
            let row = element.closest('tr')
                , inpbono = row.find('input.bono')
                , inpobs = row.find('textarea');
            inpbono.removeClass('errorClass');
            inpobs.removeClass('errorClass');
            if (+inpbono.val() > 0) {
                if (!(inpobs.val().length > 5)) {
                    inpbono.addClass('errorClass');
                    inpobs.addClass('errorClass');
                }
            }
        }
        function setTotalTotal() {
            tblBonIncidencias.find('tbody tr td').each((i, td) => {
                setTotalAsist($(td));
                setTotalExtra($(td));
            });
        }
        const primeraLetraMayuscula = (cadena) => cadena.charAt(0).toUpperCase().concat(cadena.substring(1, cadena.length));

        //#region EXTRATEMPORALES

        function initTblExtraTemp() {
            dtExtraTemp = tblExtraTemp.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'descTipoVacaciones', title: 'TIPO DE AUSENCIA' },
                    {
                        data: 'fecha', title: 'FECHA',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblExtraTemp.on('click', '.classBtn', function () {
                        let rowData = dtExtraTemp.row($(this).closest('tr')).data();
                    });
                    tblExtraTemp.on('click', '.classBtn', function () {
                        let rowData = dtExtraTemp.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        //#endregion

        async function initForm() {
            selBonIncCc.fillCombo('/Administrativo/Bono/getcboCcDepto', null, false, null);
            //selBonIncTipoNomina.fillCombo('/Administrativo/Bono/getcboTipoNomina', null, true, null);
            inpBonIncPeriodo.fillCombo('/Administrativo/Bono/getPeriodos', { anio: inpBonIncAnio.val(), tipoNomina: selBonIncTipoNomina.val() }, true, null);
            await setPeriodo();
            inpBonIncAnio.val(new Date().getFullYear());
            inpBonIncFechaMod.val(new Date().toLocaleString());
            setCboIncidecnciaConcepto();
            initDataTblBonIncidencias();
            initTblExtraTemp();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.Bono.Incidencia = new Incidencia();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
