
(function () {

    $.namespace('maquinaria.inventario.AsignacionResguardo');

    AsignacionResguardo = function () {
        const report = $('#report');

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { LoadResguardoFirmado(parametros.id) } },
                { text: `<i class="fa fa-file"></i> Visualizar`, action: "visor", fn: parametros => { visualizarResguardo(parametros.ruta) } }
            ]
        }

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        let tarjetaExiste = false;
        let polizaExiste = false;
        let permisoCarga = false;
        var idResguardo = 0;
        var PermisoAuditor = false;
        cboTipoResguardo = $("#cboTipoResguardo"),
            txtCentroCostos = $("#txtCentroCostos"),
            divLiberacion = $("#divLiberacion"),
            divNuevo = $("#divNuevo"),
            fcheckLiberacion = $("#fcheckLiberacion"),
            cboTipoAsignacion = $("#cboTipoAsignacion"),
            BntRegresar = $("#BntRegresar"),
            btnGuardarSubidaArchivo = $("#btnGuardarSubidaArchivo"),
            modalSubidaArchivos = $("#modalSubidaArchivos"),
            divInfoNuevoResguardo = $("#divInfoNuevoResguardo"),
            divListaResguardo = $("#divListaResguardo"),
            tblListaAsignados = $("#tblListaAsignados"),
            btnGuardarAsignacion = $("#btnGuardarAsignacion"),
            modalVistaAsignacionResguardos = $("#modalVistaAsignacionResguardos"),
            btnCheckList = $("#btnCheckList"),
            btnVerResguardo = $("#btnVerResguardo"),
            VencimientoPoliza = $("#VencimientoPoliza"),
            radioPrioridad3 = $("#radioPrioridad3"),
            radioPrioridad2 = $("#radioPrioridad2"),
            radioPrioridad1 = $("#radioPrioridad1"),
            tbEncierro = $("#tbEncierro"),
            Acordeon = $("#Acordeon"),
            tbNombreEmpleado = $("#tbNombreEmpleado"),
            tbPuestoEmpleado = $("#tbPuestoEmpleado"),
            tbCentroCostosEmpleado = $("#tbCentroCostosEmpleado"),
            cboEconomico = $("#cboEconomico"),
            tbDescripcionEconomico = $("#tbDescripcionEconomico"),
            tbMarcaEconomico = $("#tbMarcaEconomico"),
            tbModeloEconomico = $("#tbModeloEconomico"),
            tbKilometraje = $("#tbKilometraje"),
            tbNoSerie = $("#tbNoSerie"),
            tbPlacas = $("#tbPlacas"),
            VencimientoLicencia = $("#VencimientoLicencia"),
            fLicenciaConducir = $("#fLicenciaConducir"),
            fTarjetaCirculacion = $("#fTarjetaCirculacion"),
            fPolizaSeguro = $("#fPolizaSeguro"),
            fcheckAsignacion = $("#fcheckAsignacion"),
            fFormatoMMtoPreventivo = $("#fFormatoMMtoPreventivo"),
            fPermisoCarga = $("#fPermisoCarga"),
            tblCondicionesInterior = $("#tblCondicionesInterior"),
            tblCondicionesExterior = $("#tblCondicionesExterior"),
            tblDocumentos = $("#tblDocumentos"),
            ireport = $("#report"),
            modalEditFecha = $("#modalEditFechaLicencia"),
            modalEditFechaPoliza = $("#modalEditFechaPoliza"),
            checkboxActualizarLicencia = $('#checkboxActualizarLicencia'),
            checkboxActualizarPoliza = $('#checkboxActualizarPoliza'),
            checkboxActualizarDefensiva = $('#checkboxActualizarDefensiva'),
            divCamposLicencia = $('#divCamposLicencia'),
            divCamposPoliza = $('#divCamposPoliza'),
            divCamposDefensiva = $('#divCamposDefensiva'),
            fileLicenciaUpdate = $('#fileLicenciaUpdate'),
            filePolizaUpdate = $('#filePolizaUpdate'),
            fileDefensivaUpdate = $('#fileDefensivaUpdate'),
            modalEditlicencia = $("#modalEditlicencia"),
            btnGuardarEditLicencia = $("#btnGuardarEditLicencia"),
            tbObservaciones = $("#tbObservaciones"),
            btnBuscar = $("#btnBuscar"),
            txtCentroCostos2 = $("#txtCentroCostos2"),
            tblSinReguardo = $("#tblSinReguardo"),
            btnReporte = $("#btnReporte");

        const inputFechaVigencia = $('#inputFechaVigencia');

        mensajes = {
            NOMBRE: 'Asignacion de Equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        // menuConfig = {
        //     lstOptions: [
        //         { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.id) } }
        //         , { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.id) } }
        //     ]
        // }
        const strGetData = new URL(window.location.origin + '/ResguardoEquipo/getEquiposSinResguardo');
        function init() {

            inputFechaVigencia.datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'slide' });

            divCamposLicencia.hide();
            divCamposPoliza.hide();
            divCamposDefensiva.hide();

            ValidarPermisoAuditor();
            $("#Acordeon1").on({
                click: function () {
                    //      $(this).find(".apnl")[0].click();
                }
            }, ".pnl");
            iniciarGrid();
            datePicker();
            tblCondicionesInteriorGrid = $('#tblCondicionesInterior').DataTable({});
            tblCondicionesExteriorGrid = $('#tblCondicionesExterior').DataTable({});

            cboEconomico.change(InfoEconomico);
            tbNombreEmpleado.getAutocomplete(SelectEmpleado, null, '/ResguardoEquipo/getEmpleados');
            VencimientoPoliza.datepicker().datepicker("setDate", new Date());
            VencimientoLicencia.datepicker().datepicker("setDate", new Date());
            LoadPreguntas();
            btnGuardarAsignacion.click(TipoGuardado);
            loadTabla();

            btnGuardarSubidaArchivo.click(SubirArchivos);
            cboTipoAsignacion.prop('disabled', true);
            $("#tabTitle1").click(function () {
                $("#tabTitle2").html("Formulario Asignación");
            });
            cboEconomico.fillCombo('/ResguardoEquipo/cboEconomicos', { obj: "" });
            txtCentroCostos2.fillCombo('/MovimientoMaquinaria/cboGetCentroCostos');
            txtCentroCostos.change(loadTabla).fillCombo('/MovimientoMaquinaria/cboGetCentroCostos');
            cboTipoResguardo.change(loadTabla);
            btnGuardarEditLicencia.click(GuardarLicenciaActualizada);
            btnBuscar.click(fnLoadSinReguardo);
            //raguilar boton generar reporte gral 
            $(".actionBar").append("<button class='btn btn-info pull-left' id='btnReporte' style='margin-bottom:0px;'> <i class='fa fa-file-text-o' aria-hidden='true'></i>  Imprimir</button>'");
            //raguilar remove class container fluid
            $("#tblListaAsignados-header").removeClass("container-fluid");
            //botonreporte gral raguilar 
            $("#btnReporte").click(function () {
                verReporte();
            });

            checkboxActualizarLicencia.click(e => {
                if (e.currentTarget.checked) {
                    divCamposLicencia.show(200);
                } else {
                    divCamposLicencia.hide(200);
                    fileLicenciaUpdate.val('');
                    modalEditFecha.datepicker().datepicker("setDate", new Date());
                }
            });

            checkboxActualizarPoliza.click(e => {
                if (e.currentTarget.checked) {
                    divCamposPoliza.show(200);
                } else {
                    divCamposPoliza.hide(200);
                    filePolizaUpdate.val('');
                    modalEditFechaPoliza.datepicker().datepicker("setDate", new Date());
                }
            });

            checkboxActualizarDefensiva.click(e => {
                if (e.currentTarget.checked) {
                    divCamposDefensiva.show(200);
                } else {
                    divCamposDefensiva.hide(200);
                    fileDefensivaUpdate.val('');
                }
            })

            modalEditlicencia.on("hide.bs.modal", () => {

                checkboxActualizarLicencia[0].checked = false;
                divCamposLicencia.hide(200);
                fileLicenciaUpdate.val('');
                modalEditFecha.datepicker().datepicker("setDate", new Date());

                checkboxActualizarPoliza[0].checked = false;
                divCamposPoliza.hide(200);
                filePolizaUpdate.val('');
                modalEditFechaPoliza.datepicker().datepicker("setDate", new Date());

                checkboxActualizarDefensiva[0].checked = false;
                divCamposDefensiva.hide(200);
                fileDefensivaUpdate.val('');

                btnGuardarEditLicencia.removeAttr('data-idResguardo');
            });
            InitTblData();
        }

        async function fnLoadSinReguardo() {
            try {
                response = await ejectFetchJson(strGetData, { noAC: txtCentroCostos.val() });
                dtDataSinResguardo.clear().draw();
                dtDataSinResguardo.rows.add(response.data).draw();
            } catch (o_O) {
                AlertaGeneral("Aviso", o_O.message)
            }
        }
        function InitTblData() {

            dtDataSinResguardo = tblSinReguardo.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title: 'Economico', data: 'economico' },
                    { title: 'Grupo', data: 'grupo' },
                    { title: 'Modelo', data: 'modelo' },
                    { title: 'AC', data: 'ccActual' },
                    {
                        title: '', data: 'id', width: '70px', createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<input type='button' data-id='" + rowData.id + "' value='Resguardar'/>");
                            $(td).find(`input[type="button"]`).click(function () {
                                var id = $(this).data('id');
                                $("a[href$='#nuevoResguardo']").click();
                                cboEconomico.val(id);
                                cboEconomico.change();
                            });
                        }
                    }
                ]
                , initComplete: function (settings, json) {

                }
            });
        }
        //raguilar generacion reporte gral raguilar  11/04/18 
        function verReporte() {
            var CC = txtCentroCostos.val();
            var tipoResguardo = cboTipoResguardo.val();
            var CCTxt = $("#txtCentroCostos option:selected").text();
            var tipoResguardoTxt = $("#cboTipoResguardo option:selected").text();


            var flag = true;
            if (CC == null || tipoResguardo == null) {
                flag = false;
            }
            if (flag) {
                var idReporte = "66";
                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&tipoResguardo=" + tipoResguardo + "&CCTxt=" + CCTxt + "&tipoResguardoTxt=" + tipoResguardoTxt;
                ireport.attr("src", path);
                $.blockUI({ message: mensajes.PROCESANDO });
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
                e.preventDefault();
            }
            else {
                $.unblockUI();
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
            }
        }

        function ValidarPermisoAuditor() {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/administrador/usuarios/getUsuariosPermisos',
                success: function (response) {
                    PermisoAuditor = response.Autorizador;

                    if (PermisoAuditor) {
                        $("#tabTitle2").addClass('hide');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function TipoGuardado() {

            if (cboTipoAsignacion.val() == "1") {
                GuardarOrUpdateResguardo();
            }
            else {
                GuardarLiberacion();
            }
        }

        function GuardarLiberacion() {

            SendEspecial(null, GetInformacion());
        }

        $('.panel-heading a').on('click', function (e) {
            if ($(this).parents('.panel').children('.panel-collapse').hasClass('in')) {
                e.stopPropagation();
            }
        });

        $("a[href$='#nuevoResguardo']").on('click', function (e) {
            idResguardo = 0;
            $(".apnl1")[0].click();
            LimpiarCampos();
        });


        $('input:radio[name=radioInline1]').change(function () {

            if ($('input[name=radioInline1]:checked').val() == '1') {
                tbEncierro.val('A');

            } else if ($('input[name=radioInline1]:checked').val() == '2') {
                tbEncierro.val('B');
            }
            else if ($('input[name=radioInline1]:checked').val() == '3') {
                tbEncierro.val('C');
            }
        });

        function loadTabla() {
            switch (+cboTipoResguardo.val()) {
                case 4:
                    $('#divTabla1').css('display', 'none');
                    $('#divTabla2').css('display', 'block');
                    fnLoadSinReguardo();
                    break;
                default:
                    $('#divTabla1').css('display', 'block');
                    $('#divTabla2').css('display', 'none');
                    bootG('/ResguardoEquipo/GetListaAutorizacionesPendientes');
                    break;
            }
        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { cc: txtCentroCostos.val(), tipoDocumento: cboTipoResguardo.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.listAutorizaciones;
                    tblListaAsignados.bootgrid("clear");
                    tblListaAsignados.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function iniciarGrid() {
            tblListaAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: "TEST"
                },
                formatters: {
                    "UsuarioSolicitud": function (column, row) {
                        let usuarioAsignado = row.UsuarioSolicitud;
                        let usuarioSplit = usuarioAsignado.split(" ");
                        let usuarioConcat = "";
                        let primerNombre = "";
                        usuarioSplit.forEach(element => {
                            usuarioConcat += ` ${element}`;
                        });
                        if (usuarioSplit.length > 1) {
                            primerNombre = usuarioSplit[0];
                        }
                        return `<text title="${usuarioAsignado}">${primerNombre} ...</text>`;
                    },
                    "imprimirChecklist": function (column, row) {
                        return "<button type='button' class='btn btn-primary imprimirChecklist' data-id='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "imprimirResguardo": function (column, row) {
                        return "<button type='button' class='btn btn-primary imprimirResguardo' data-id='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },//habilitando evidencia adjunto raguilar 11/04/18
                    "ImprimirEvidencia": function (column, row) {
                        return "<button type='button' class='btn btn-primary ImprimirEvidencia' data-id='" + row.id + "' >" +
                            "<span class='fa fa-car'></span> " +
                            " </button>";
                    },
                    "Editresguardo": function (column, row) {
                        return "<button type='button' class='btn btn-warning Editresguardo' data-id='" + row.id + "' " + (cboTipoResguardo.val() == "3" ? "disabled" : "") + ">" +
                            "<span class='glyphicon glyphicon-edit'></span> " +
                            " </button>";
                    },
                    "SubirArchivo": function (column, row) {
                        return "<button type='button' class='btn btn-info SubirArchivo' data-id='" + row.id + "' " + (cboTipoResguardo.val() == "3" ? "disabled" : "") + ">" +
                            "<span class='glyphicon glyphicon-upload'></span> " +
                            " </button>";
                    },
                    "ModificarLicencia": function (column, row) {
                        return "<button type='button' class='btn btn-warning editLicencia' data-id='" + row.id + "' " + (cboTipoResguardo.val() == "3" ? "disabled" : "") + ">" +
                            "<span class='glyphicon glyphicon-edit'></span> " +
                            " </button>";
                    },
                    "ResguardoCargado": function (column, row) {
                        if (row.tieneResguardo) {
                            return `<i class="far fa-check-circle far-2xl verResguardoFirmado" data-id_resguardo_firmado="${row.resguardoFirmadoId}" data-nombre_ruta="${row.resguardoFirmadoUrl}" style="color: green; font-size: 30px; cursor: pointer;" title="Resguardo cargado."></i>`;
                        } else {
                            return `<i class="far fa-times-circle far-2xl" style="color: red; font-size: 30px;" title="Resguardo no cargado."></i>`;
                        }
                        // if (row.tieneResguardo) {
                        //     return `<i class="far fa-check-circle far-2xl verResguardoFirmado" data-id_resguardo_firmado=${row.resguardoFirmadoId} style="color: green; font-size: 30px; cursor: pointer;" title="Resguardo cargado."></i>`;
                        // } else {
                        //     return `<i class="far fa-times-circle far-2xl" style="color: red; font-size: 30px;" title="Resguardo no cargado."></i>`;
                        // }
                    }
                    //"documentos": function (column, row) {
                    //    let html = "";
                    //    for (let index = 0; index < row.documentos.length; index++) {
                    //        let obj = row.documentos[index];
                    //        html += "<button type='button' class='btn btn-primary bajarDocumento' data-tipo='" + obj.tipoDocumento + "' data-idDocumento='" + obj.idDocumento + "' title=" + obtenerInfo(obj.tipoDocumento) + " >" +
                    //            "<i class='fa fa-arrow-circle-down' aria-hidden='true'></i> </button>";
                    //    }
                    //    return html;
                    //},
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                if (PermisoAuditor) {
                    $(".Editresguardo").prop('disabled', 'disabled');
                    $(".SubirArchivo").prop('disabled', 'disabled');
                }

                //habilitando boton reporte evidencia curso manejo raguilar 11/04/18
                tblListaAsignados.find(".ImprimirEvidencia").on("click", function (e) {
                    idEvidencia = Number($(this).attr('data-id'));
                    AutorizarAdjuntoDocto(idEvidencia);
                });
                tblListaAsignados.find(".imprimirChecklist").on("click", function (e) {
                    $.blockUI({ message: mensajes.PROCESANDO });
                    var path = "/Reportes/Vista.aspx?idReporte=44&idReguardo=" + Number($(this).attr('data-id'));
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                });
                tblListaAsignados.find(".imprimirResguardo").on("click", function (e) {
                    idResguardo = Number($(this).attr('data-id'));

                    LoadReporte(idResguardo)
                });
                tblListaAsignados.find('.verResguardoFirmado').on('click', function (e) {
                    let idResguardoFirmado = Number($(this).data('id_resguardo_firmado'));
                    let nombreRuta = $(this).data("nombre_ruta");

                    // LoadResguardoFirmado(idResguardoFirmado);
                    menuConfig.parametros = {
                        id: idResguardoFirmado,
                        ruta: nombreRuta
                    }
                    mostrarMenu();
                });
                tblListaAsignados.find(".SubirArchivo").on("click", function (e) {
                    idResguardo = Number($(this).attr('data-id'));
                    SubirArchivo(idResguardo);
                });

                tblListaAsignados.find(".Editresguardo").on("click", function (e) {
                    idResguardo = Number($(this).attr('data-id'));
                    $('a[href$="#nuevoResguardo"]').tab('show');
                    SetDisabled();
                    loadDataResguardo(idResguardo);
                    $("#tabTitle2").html("Formulario Liberación");
                });
                tblListaAsignados.find(".editLicencia").on("click", function (e) {
                    idResguardo = Number($(this).attr('data-id'));
                    modalEditFecha.datepicker().datepicker("setDate", new Date());
                    modalEditFechaPoliza.datepicker().datepicker("setDate", new Date());

                    btnGuardarEditLicencia.removeAttr('data-idResguardo');
                    btnGuardarEditLicencia.attr('data-idResguardo', idResguardo);
                    modalEditlicencia.modal('show');
                });

                // tblListaAsignados.find(".bajarDocumento").on("click", function (e) {
                //     idDocumento = Number($(this).attr('data-idDocumento'));
                //     tipo = Number($(this).attr('data-tipo'));
                //     clickAnexo(tipo, idDocumento, 0);
                // });
            });
        }

        // function clickAnexo(tipoArchivo, id, idEconomico) {

        //     if (id == 0) {
        //         openModalAlta(tipoArchivo);
        //     }
        //     else {
        //         menuConfig.parametros = {
        //             id
        //         };
        //         mostrarMenu();
        //         // downloadURI(id);
        //     }
        // }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/CatInventario/getFileDownload?id=' + elemento;
            link.href = '/CatInventario/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/CatInventario/getFileDownload?id=' + elemento;
        }

        const getFileRuta = new URL(window.location.origin + '/CatInventario/getFileRuta');
        async function setVisor(id) {
            try {
                response = await ejectFetchJson(getFileRuta, { id });
                if (response.success) {
                    $('#myModal').data().ruta = response.ruta;
                    $('#myModal').modal('show');
                }
            } catch (o_O) { }
        }

        function obtenerInfo(tipoID) {

            switch (tipoID) {
                case 3: return "Poliza de Seguro";
                case 4: return "Tarjeta de Circulación";
                case 5: return "Programa Mantenimiento";
                default:
                    break;
            }

        }

        function AutorizarAdjuntoDocto(idEvidencia) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetEvidenciaByID',
                type: "POST",
                datatype: "json",
                data: { idEvidencia: idEvidencia },
                success: function (response) {
                    if (response.idDocumento != null) {
                        window.location.href = '/ResguardoEquipo/getFileDownload?id=' + idEvidencia;
                        $.unblockUI();
                    } else {
                        AlertaGeneral('Alerta', 'No Cuenta con Evidencia');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function GuardarLicenciaActualizada() {

            let fechaVencimientoLicencia = null;
            let fechaVencimientoPoliza = null;
            let archivoLicencia = null;
            let archivoPoliza = null;
            let archivoCurso = null;
            const actualizaLicencia = checkboxActualizarLicencia.prop('checked');
            const actualizaPoliza = checkboxActualizarPoliza.prop('checked');
            const actualizaCurso = checkboxActualizarDefensiva.prop('checked');

            if (actualizaLicencia) {
                fechaVencimientoLicencia = modalEditFecha.val();
                archivoLicencia = fileLicenciaUpdate[0].files[0];
                if (archivoLicencia == null) {
                    AlertaGeneral('Alerta', 'Debe seleccionar un archivo de licencia.');
                    return;
                }
            }

            if (actualizaPoliza) {
                fechaVencimientoPoliza = modalEditFechaPoliza.val();
                archivoPoliza = filePolizaUpdate[0].files[0];
                if (archivoPoliza == null) {
                    AlertaGeneral('Alerta', 'Debe seleccionar un archivo de póliza.');
                    return;
                }
            }

            if (actualizaCurso) {
                archivoCurso = fileDefensivaUpdate[0].files[0];
                if (archivoCurso == null) {
                    AlertaGeneral('Alerta', 'Debe seleccionar un archivo de curso.');
                    return;
                }
            }

            const resguardoID = btnGuardarEditLicencia.attr('data-idResguardo');
            debugger;

            modalEditlicencia.modal('hide');
            $.blockUI({ message: 'Actualizando datos...' });

            const data = new FormData();
            data.append("actualizaLicencia", actualizaLicencia);
            data.append("actualizaPoliza", actualizaPoliza);
            data.append("actualizaCurso", actualizaCurso);
            data.append("resguardoID", resguardoID);
            data.append("fechaVencimientoLicencia", fechaVencimientoLicencia);
            data.append("archivoLicencia", archivoLicencia);
            data.append("fechaVencimientoPoliza", fechaVencimientoPoliza);
            data.append("archivoPoliza", archivoPoliza);
            data.append("archivoCurso", archivoCurso);
            data.append("fechaVigencia", inputFechaVigencia.val() != '' ? JSON.stringify(inputFechaVigencia.val()) : '');

            $.ajax({
                type: "POST",
                url: '/ResguardoEquipo/EditarLicencia',
                data: data,
                dataType: 'json',
                contentType: false,
                processData: false,
            })
                .then(response => {
                    if (response.success) {
                        AlertaGeneral('Alerta', 'El registro fue actualizado correctamente.');
                        loadTabla();
                    } else {
                        AlertaGeneral('Alerta', 'Ocurrió un error. No se completó la operación.');
                    }
                }, () => AlertaGeneral('Alerta', 'Ocurrió un error. No se completó la operación.'))
                .then($.unblockUI);
        }

        function SetDisabled() {
            tbNombreEmpleado.prop('disabled', true);
            cboEconomico.prop('disabled', true);
            tbPlacas.prop('disabled', true);
            tbKilometraje.prop('disabled', true);
            cboTipoAsignacion.prop('disabled', true);
            cboTipoAsignacion.val("2");
            divNuevo.addClass('hide');
            divLiberacion.removeClass('hide');
            LoadPreguntas();
        }

        function SubirArchivo(idResguardo) {
            btnGuardarSubidaArchivo.attr('data-idResguardo', idResguardo);
            modalSubidaArchivos.modal('show');

        }

        function SubirArchivos() {

            var formData = new FormData();
            var file1 = document.getElementById("fResguardoFirmado").files[0];
            var file2 = document.getElementById("fAnexos").files[0];
            var idResguardo = btnGuardarSubidaArchivo.attr('data-idResguardo');

            formData.append("fResguardoFirmado", file1);
            formData.append("fAnexos", file2);


            formData.append("idResguardo", JSON.stringify(idResguardo));

            if (file1 != undefined) {
                $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });

                $.ajax({
                    type: "POST",
                    url: '/ResguardoEquipo/SubirArchivoResguardo',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        idResguardo = response.idResguardo;
                        LimpiarCampos();

                        ConfirmacionGeneralAccion('Confirmacion', 'Se Actualizaron los archivos Correctamente.');

                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            }
        }

        function LoadReporte(idResguardo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ResguardoEquipo/GetReporte',
                type: "POST",
                datatype: "json",
                data: { obj: idResguardo },
                success: function (response) {

                    var path = "/Reportes/Vista.aspx?idReporte=42&size=2&tipoResguardo=" + cboTipoResguardo.val();
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadResguardoFirmado(id) {
            window.location.href = '/ResguardoEquipo/getFileDownloadGeneral?id=' + id;
        }

        function visualizarResguardo(ruta) {
            $('#myModal').data().ruta = ruta;
            $('#myModal').modal('show');
        }

        function GuardarOrUpdateResguardo() {


            var objValida = ValidateInfo();

            if (objValida.estado) {
                SendEspecial(null, GetInformacion())
            }
            else {

                AlertaGeneral('Alerta', "Favor de introducir la información correspondiente marcada en rojo");
            }

        }

        function ValidateInfo() {
            var state = {};
            state.estado = true;
            var coun = 0;
            if (!validarCampo(tbPuestoEmpleado)) { state.estado = false; state.tipo = coun += 1; }
            if (!validarCampo(tbNombreEmpleado)) { state.estado = false; state.tipo = coun += 1; }
            if (!validarCampo(cboEconomico)) { state.estado = false; state.tipo = coun += 1; }
            if (!validarCampo(fLicenciaConducir)) { state.estado = false; state.tipo = coun += 3; }
            if (!tarjetaExiste)
                if (!validarCampo(fTarjetaCirculacion)) { state.estado = false; state.tipo = coun += 3; }
            if (!polizaExiste)
                if (!validarCampo(fPolizaSeguro)) { state.estado = false; state.tipo = coun += 3; }
            if (!validarCampo(tbKilometraje)) { state.estado = false; state.tipo = coun += 1; }

            return state;
        }


        function SetRequiered() {
            tbPuestoEmpleado.addClass('required')
            fLicenciaConducir.addClass('required');
            fTarjetaCirculacion.addClass('required');
            fPolizaSeguro.addClass('required');
            tbNombreEmpleado.addClass('required');
            cboEconomico.addClass('required');
            tbKilometraje.addClass('required');
        }

        function GetInformacion() {
            return {
                id: idResguardo,
                noEmpleado: tbNombreEmpleado.attr('data-numempleado'),
                nombEmpleado: tbNombreEmpleado.val(),
                Puesto: tbPuestoEmpleado.val(),
                Obra: tbCentroCostosEmpleado.attr('data-cc'),
                MaquinariaID: cboEconomico.val(),
                Kilometraje: tbKilometraje.val(),
                TipoEncierro: $('input[name="radioInline1"]:checked').val(),
                fechaVencimiento: VencimientoLicencia.val(),
                fechaVencimientoPoliza: VencimientoPoliza.val(),
                Comentario: tbObservaciones.val(),
                Placas: tbPlacas.val(),


            };
        }


        $(document).on('click', ".SelecccionInterior", function () {
            ChangeActivo($(this), tblCondicionesInteriorGrid);

            LoadTablaInterior(tblCondicionesInteriorGrid.data());
        });

        $(document).on('click', ".SeleccionExterior", function () {
            ChangeActivo($(this), tblCondicionesExteriorGrid);

            LoadTablaExterior(tblCondicionesExteriorGrid.data());
        });

        $(document).on('change', ".comentario1", function () {
            ChangeText($(this), tblCondicionesInteriorGrid);

            LoadTablaInterior(tblCondicionesInteriorGrid.data());
        });

        $(document).on('change', ".comentario2", function () {
            ChangeText($(this), tblCondicionesExteriorGrid);

            LoadTablaExterior(tblCondicionesExteriorGrid.data());
        });

        function LoadPreguntas() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetTablesPreguntas',
                async: false,
                success: function (response) {
                    var Interiores = response.Interiores;
                    var Exterior = response.Exterior;
                    var Documentos = response.Documentos;
                    LoadTablaInterior(Interiores);
                    LoadTablaExterior(Exterior);
                    // LoadTablaDocumentos(Documentos);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadTablaInterior(dataSet) {
            tblCondicionesInteriorGrid = $("#tblCondicionesInterior").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: true,
                "bFilter": false,
                "order": false,
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        data: "Concepto",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                        }
                    },
                    {
                        data: "Bueno",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'bueno');
                            $(td).addClass('SelecccionInterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {

                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Regular",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'regular');
                            $(td).addClass('SelecccionInterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Malo",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'malo');
                            $(td).addClass('SelecccionInterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "NA",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).addClass('SelecccionInterior');
                            $(td).attr('data-Select', 'na');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Observaciones",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).text('');
                            //$(td).append('<input type="text" class="comentario1 form-control value="' + cellData + ' " />');
                            $(td).append("<input type='text' class='comentario1 form-control' value='" + cellData + "' >")
                        }
                    }
                ],
                "paging": false,
                "info": false
            });
        }

        function LoadTablaExterior(dataSet) {
            tblCondicionesExteriorGrid = $("#tblCondicionesExterior").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                "bFilter": false,
                "order": false,
                destroy: true,
                data: dataSet,
                columns: [
                    {
                        data: "Concepto",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                        }
                    },
                    {
                        data: "Bueno",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'bueno');
                            $(td).addClass('SeleccionExterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {

                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Regular",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'regular');
                            $(td).addClass('SeleccionExterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Malo",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).attr('data-Select', 'malo');
                            $(td).addClass('SeleccionExterior');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "NA",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).addClass('SeleccionExterior');
                            $(td).attr('data-Select', 'na');
                            $(td).text('');
                            if (cellData == "1") {
                                $(td).addClass('Activo');
                            } else {
                                $(td).removeClass('Activo');
                            }
                        }
                    },
                    {
                        data: "Observaciones",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-id', rowData.id);
                            $(td).text('');
                            $(td).append("<input type='text' value='" + cellData + "' class='comentario2 form-control'>")
                        }
                    }
                ],
                "paging": false,
                "info": false
            });
        }

        function ChangeActivo(activo, tablaGrid) {
            if (!$(activo).hasClass('Activo')) {

                tdActivo = $(activo).parent('tr').children('td.Activo');
                $(tdActivo).removeClass('Activo');
                oldActivo = $(tdActivo).attr('data-Select');
                newActivo = $(activo).attr('data-Select');
                $(activo).addClass('Activo');

                var ObjetoTable = tablaGrid.row(activo).data();
                switch (oldActivo) {
                    case "bueno":
                        tablaGrid.row(activo).data().Bueno = 0;
                        break;
                    case "malo":
                        tablaGrid.row(activo).data().Malo = 0;
                        break;
                    case "na":
                        tablaGrid.row(activo).data().NA = 0;
                        break;
                    case "regular":
                        tablaGrid.row(activo).data().Regular = 0;
                        break;
                    default:
                }
                switch (newActivo) {
                    case "bueno":
                        tablaGrid.row(activo).data().Bueno = 1;
                        break;
                    case "malo":
                        tablaGrid.row(activo).data().Malo = 1;
                        break;
                    case "na":
                        tablaGrid.row(activo).data().NA = 1;
                        break;
                    case "regular":
                        tablaGrid.row(activo).data().Regular = 1;
                        break;
                    default:
                }

                tablaGrid.row(activo).data().Observaciones = activo.parent().children().find('input').val();

            }
        }

        function ChangeText(Comentario, tablaGrid) {
            activo = $(Comentario).parent('td');
            tablaGrid.row(activo).data().Observaciones = $(Comentario).val();
        }

        function SelectEmpleado(event, ui) {
            tbNombreEmpleado.text(ui.item.value);
            tbNombreEmpleado.attr('data-NumEmpleado', ui.item.id)
            SetInfoEmpleado(ui.item.id);
        }

        function SetInfoEmpleado(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                async: false,
                success: function (response) {
                    if (response.success) {
                        objEmpleado = response.items;
                        tbPuestoEmpleado.val(response.Puesto.toLowerCase());
                        tbCentroCostosEmpleado.val(response.Centro_Costos.toLowerCase())
                        tbCentroCostosEmpleado.attr('data-CC', response.CCEmpleado);

                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SendEspecial(e, objeto) {
            if (true) {

                var formData = new FormData();

                var file1 = document.getElementById("fLicenciaConducir").files[0];
                var file2 = document.getElementById("fTarjetaCirculacion").files[0];
                var file3 = document.getElementById("fPolizaSeguro").files[0];
                var file4 = document.getElementById("fcheckAsignacion").files[0];
                var file5 = document.getElementById("fFormatoMMtoPreventivo").files[0];
                var file6 = document.getElementById("fPermisoCarga").files[0];
                var file7 = document.getElementById("fcheckLiberacion").files[0];

                formData.append("fLicenciaConducir", file1);
                formData.append("fTarjetaCirculacion", file2);
                formData.append("fPolizaSeguro", file3);
                formData.append("fcheckAsignacion", file4);
                formData.append("fFormatoMMtoPreventivo", file5);
                formData.append("fPermisoCarga", file6);
                formData.append("fcheckLiberacion", file7);


                formData.append("TipoResguardo", cboTipoAsignacion.val());


                formData.append("datacheckList", JSON.stringify(GetInfoTablas()));
                formData.append("obj", JSON.stringify(objeto));

                if (file1 != undefined && file2 != undefined && file3 != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/ResguardoEquipo/SaveSendEspecial',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        idResguardo = response.idResguardo;

                        ConfirmacionGeneralAccion('Confirmacion', 'Se agrego un nuevo resguardo.');

                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function GetInfoTablas() {

            var InfoCompleta = [];

            for (var i = 0; i < tblCondicionesInteriorGrid.data().length; i++) {
                var objAdd = {};
                objAdd.GrupoID = 1;
                objAdd.Bueno = tblCondicionesInteriorGrid.data()[i].Bueno;
                objAdd.Malo = tblCondicionesInteriorGrid.data()[i].Malo;
                objAdd.NA = tblCondicionesInteriorGrid.data()[i].NA;
                objAdd.Regular = tblCondicionesInteriorGrid.data()[i].Regular;
                objAdd.Observaciones = tblCondicionesInteriorGrid.data()[i].Observaciones;
                objAdd.RespuestaID = tblCondicionesInteriorGrid.data()[i].id;
                objAdd.TipoResguardo = Number(cboTipoAsignacion.val());
                InfoCompleta.push(objAdd);
            }

            for (var i = 0; i < tblCondicionesExteriorGrid.data().length; i++) {

                var objAdd = {};
                objAdd.GrupoID = 2;
                objAdd.Bueno = tblCondicionesExteriorGrid.data()[i].Bueno;
                objAdd.Malo = tblCondicionesExteriorGrid.data()[i].Malo;
                objAdd.NA = tblCondicionesExteriorGrid.data()[i].NA;
                objAdd.Regular = tblCondicionesExteriorGrid.data()[i].Regular;
                objAdd.Observaciones = tblCondicionesExteriorGrid.data()[i].Observaciones;
                objAdd.RespuestaID = tblCondicionesExteriorGrid.data()[i].id;
                objAdd.TipoResguardo = Number(cboTipoAsignacion.val());
                InfoCompleta.push(objAdd);
            }
            return InfoCompleta;
        }

        function InfoEconomico() {
            polizaExiste = false;
            tarjetaExiste = false;
            permisoCarga = false;
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ResguardoEquipo/GetInfoEconomico',
                type: "POST",
                datatype: "json",
                data: { idEconomico: cboEconomico.val() },
                success: function (response) {
                    var objEconomico = response.dataEconomico;
                    let infoArchivos = response.documentos;

                    infoArchivos.forEach(archivo => {
                        if (archivo.tipoArchivo == 3)
                            polizaExiste = true;
                        if (archivo.tipoArchivo == 4)
                            tarjetaExiste = true;
                        if (archivo.tipoArchivo == 5)
                            permisoCarga = true;
                    });
                    if (polizaExiste) {
                        $("#spPoliza").html("(Ya se cuenta con este documento, no es obligatorio)");
                    }
                    else {
                        $("#spPoliza").html("");
                    }
                    if (tarjetaExiste) {
                        $("#spCirculacion").html("(Ya se cuenta con este documento, no es obligatorio)");
                    }
                    else {
                        $("#spCirculacion").html("");
                    }
                    if (permisoCarga) {
                        $("#spCarga").html("(Ya se cuenta con este documento, no es obligatorio)");
                    }
                    else {
                        $("#spCarga").html("");
                    }
                    tbDescripcionEconomico.val(objEconomico.Descripcion);
                    tbMarcaEconomico.val(objEconomico.Marca);
                    tbModeloEconomico.val(objEconomico.Modelo);
                    tbNoSerie.val(objEconomico.Serie);
                    tbPlacas.val(objEconomico.Placas);

                    switch (objEconomico.TipoEncierro) {
                        case 1:
                            {
                                radioPrioridad3.prop("checked", true);
                                tbEncierro.val('A');
                                break;
                            }
                        case 2:
                            {
                                radioPrioridad2.prop("checked", true);
                                tbEncierro.val('B');
                            }
                        case 3:
                            {
                                radioPrioridad1.prop("checked", true);
                                tbEncierro.val('C');
                                break;
                            }
                        default:

                    }

                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LimpiarCampos() {
            tbDescripcionEconomico.val('').prop('disabled', true);
            tbMarcaEconomico.val('').prop('disabled', true);
            tbModeloEconomico.val('').prop('disabled', true);
            tbNoSerie.val('').prop('disabled', true);
            tbPlacas.val('').prop('disabled', false);
            divNuevo.removeClass('hide');
            divLiberacion.addClass('hide');
            cboTipoAsignacion.val('1');
            cboTipoAsignacion.prop('disabled', true);
            tbNombreEmpleado.val('').prop('disabled', false);
            cboEconomico.val('').prop('disabled', false);
            tbKilometraje.val('').prop('disabled', false);

        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            mes = now.getMonth();
            day = now.getDate();
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#VencimientoLicencia")
                    .datepicker({

                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        //minDate: new Date(year, mes, day + 30),
                        onSelect: function () {
                            $(this).trigger('change');
                        }
                    })
                    .on("change", function () {
                        $(this).blur();

                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        function loadDataResguardo(resguardoID) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/LoadInfoResguardo',
                data: { resguardoID: resguardoID },
                async: false,
                success: function (response) {
                    if (response.success) {
                        objResguardo = response.objResguardo;
                        tbNombreEmpleado.val(objResguardo.nombEmpleado);
                        tbPuestoEmpleado.val(objResguardo.Puesto);
                        tbCentroCostosEmpleado.val(response.nameCC);
                        cboEconomico.fillCombo('/ResguardoEquipo/cboEconomicosSinFiltro', { obj: "" });
                        cboEconomico.val(objResguardo.MaquinariaID);
                        cboEconomico.trigger('change');
                        tbKilometraje.val(objResguardo.Kilometraje);

                        switch (objResguardo.TipoEncierro) {
                            case 1:
                                {
                                    radioPrioridad3.prop("checked", true);
                                    tbEncierro.val('A');
                                    break;
                                }
                            case 2:
                                {
                                    radioPrioridad2.prop("checked", true);
                                    tbEncierro.val('B');
                                }
                            case 3:
                                {
                                    radioPrioridad1.prop("checked", true);
                                    tbEncierro.val('C');
                                    break;
                                }
                            default:

                        }

                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        init();
    };

    $(document).ready(function () {

        maquinaria.inventario.AsignacionResguardo = new AsignacionResguardo();
    });
})();

