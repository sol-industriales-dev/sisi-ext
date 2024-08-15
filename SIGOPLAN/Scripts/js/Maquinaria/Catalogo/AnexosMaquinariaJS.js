
(function () {

    $.namespace('maquinaria.Catalogo.AnexosMaquinaria');

    AnexosMaquinaria = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Visor de solicitudes.',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        var formData = new FormData();
        btnAgregar = $("#btnAgregar"),
            modalAnexoMultiple = $("#modalAnexoMultiple"),
            btnGuardarData = $("#btnGuardarData"),
            updFactura = $("#updFactura"),
            updPedimento = $("#updPedimento"),
            updPoliza = $("#updPoliza"),
            updTarjetaCirculacion = $("#updTarjetaCirculacion"),
            updPermisoCarga = $("#updPermisoCarga"),
            updCertificacion = $("#updCertificacion"),
            updContratos = $("#updContratos"),
            updCuadroComparativo = $("#updCuadroComparativo"),
            updAnsul = $("#updAnsul"),

            cboEconomico = $("#cboEconomico");

        cboTipoMaquinaria = $("#cboTipoMaquinaria");

        peconomicoID = 0;
        BntRegresar = $("#BntRegresar"),
            ireport = $("#report"),
            tblSolicitudesDetalle = $("#tblSolicitudesDetalle"),
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas"),
            btnBuscar = $("#btnBuscar"),
            cboGrupo = $("#cboGrupo"),
            cboEconomicoFiltro = $("#cboEconomicoFiltro"),
            cboFiltroCentroCostos = $("#cboFiltroCentroCostos"),
            cboTipoDocumentoSingle = $("#cboTipoDocumentoSingle"),
            modalAnexoSimple = $("#modalAnexoSimple"),
            updSingleAnexo = $("#updSingleAnexo"),
            btnAddSingle = $("#btnAddSingle");

        const botonActualizarArchivo = $('#botonActualizarArchivo');

        function init() {
            verificarPermisoEliminar();

            cboTipoMaquinaria.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true });
            cboGrupo.fillCombo('/CatMaquina/FillCboGrupos');
            cboGrupo.change(FillEconomicosByGrupo);
            $("#cboTipoDocumentoSingle").fillCombo('/CatMaquina/FillCboTiposArchivos');
            cboFiltroCentroCostos.fillCombo('/CatInventario/FillComboCC', { est: true }, false, "Todos");

            convertToMultiselect("#cboFiltroCentroCostos");
            // cboFiltroCentroCostos.change(loadEconomicos);
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({
                language: dtDicEsp,
                columnDefs: [
                    { targets: 1, "visible": false },
                    { targets: 2, "visible": false },
                    { targets: 3, "visible": false },
                    { targets: 4, "visible": false },
                    { targets: 5, "visible": false },
                    { targets: 6, "visible": false },
                    { targets: 7, "visible": false },
                    { targets: 8, "visible": false }
                ]
            });
            BntRegresar.click(regresar);
            btnBuscar.click(loadTabla);

            btnAddSingle.click(subirArchivos);
            botonActualizarArchivo.click(actualizarArchivo);
            btnAgregar.click(openModalData);
            btnGuardarData.click(GuardarInformacion);

            loadEconomicos();
        }
        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.id) } },
                { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.id) } },
                { text: `<i class="fa fa-edit"></i> Actualizar`, action: "actualizar", fn: parametros => { openModalActualizar(parametros.tipoArchivo); } }
            ]
        }
        function FillEconomicosByGrupo() {
            loadEconomicos();

        }
        function openModalData() {
            cboEconomico.fillCombo('/CatInventario/FillCboEconomicos', { ccs: getValoresMultiples("#cboFiltroCentroCostos"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0 });
            VaciarNuevaCarga();
            modalAnexoMultiple.modal('show');
        }

        function loadEconomicos() {
            cboEconomicoFiltro.fillCombo('/CatInventario/FillCboEconomicos', { ccs: getValoresMultiples("#cboFiltroCentroCostos"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0 });
        }

        function subirArchivos() {

            var formData = new FormData();
            var file1 = document.getElementById("updSingleAnexo").files[0];


            formData.append("pNoEconomicoID", JSON.stringify(btnAddSingle.attr('data-idEconomico')));
            formData.append("pTipoArchivo", JSON.stringify(cboTipoDocumentoSingle.val()));
            formData.append("fSingleAnexo", file1);

            if (file1 != undefined) {
                $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });

                $.ajax({
                    type: "POST",
                    url: '/CatInventario/SubirArchivoSingle',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {

                        document.getElementById("updSingleAnexo").value = "";
                        cboTipoDocumentoSingle.val('');
                        modalAnexoSimple.modal('hide');

                        $.unblockUI();

                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            }

        }

        function actualizarArchivo() {
            var formData = new FormData();
            var file1 = document.getElementById("inputActualizarArchivo").files[0];

            formData.append("pNoEconomicoID", JSON.stringify(botonActualizarArchivo.attr('data-idEconomico')));
            formData.append("idDocumento", JSON.stringify(botonActualizarArchivo.attr('data-id-documento')));
            formData.append("pTipoArchivo", JSON.stringify($('#selectTipoDocumentoActualizar').val()));
            formData.append("fSingleAnexo", file1);

            if (file1 != undefined) {
                $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                $.ajax({
                    type: "POST",
                    url: '/CatInventario/ActualizarArchivoEconomico',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        document.getElementById("inputActualizarArchivo").value = "";
                        $('#selectTipoDocumentoActualizar').val('');
                        $('#modalActualizarArchivo').modal('hide');

                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            }
        }

        function GuardarInformacion() {

            var formData = new FormData();
            var flFactura = document.getElementById("updFactura").files[0];
            var flPedimento = document.getElementById("updPedimento").files[0];
            var flPoliza = document.getElementById("updPoliza").files[0];
            var flTarjetaCirculacion = document.getElementById("updTarjetaCirculacion").files[0];
            var flPermisoCarga = document.getElementById("updPermisoCarga").files[0];
            var flCertificacion = document.getElementById("updCertificacion").files[0];
            var flCuadroComparativo = document.getElementById("updCuadroComparativo").files[0];
            var flContratos = document.getElementById("updContratos").files[0];
            var flAnsul = document.getElementById("updAnsul").files[0];

            if (cboEconomico.val() == "") {
                AlertaGeneral('Alerta', 'Se debe tener un Economico seleccionado para poder aplicar la operación')
                return false;
            }


            formData.append("pNoEconomicoID", JSON.stringify(cboEconomico.val()));
            formData.append("pTipoFactura", JSON.stringify(updFactura.attr('data-tipo')));
            formData.append("pTipoPedimento", JSON.stringify(updPedimento.attr('data-tipo')));
            formData.append("pTipoPoliza", JSON.stringify(updPoliza.attr('data-tipo')));
            formData.append("pTipoTCirculacion", JSON.stringify(updTarjetaCirculacion.attr('data-tipo')));
            formData.append("pTipoPermisoCarga", JSON.stringify(updPermisoCarga.attr('data-tipo')));
            formData.append("pTipoCertificacion", JSON.stringify(updCertificacion.attr('data-tipo')));
            formData.append("pTipoCuadrpoComparativo", JSON.stringify(updCuadroComparativo.attr('data-tipo')));
            formData.append("pTipoContratos", JSON.stringify(updContratos.attr('data-tipo')));
            formData.append("pTipoAnsul", JSON.stringify(updAnsul.attr('data-tipo')));



            formData.append("fFactura", flFactura);
            formData.append("fPedimento", flPedimento);
            formData.append("fPoliza", flPoliza);
            formData.append("fTarjetaCirculacion", flTarjetaCirculacion);
            formData.append("fPermisoCarga", flPermisoCarga);
            formData.append("fCertificacion", flCertificacion);
            formData.append("flCuadroComparativo", flCuadroComparativo);
            formData.append("flContratos", flContratos);
            formData.append("flAnsul", flAnsul);


            var Bandera = ValidarEnvio();

            if (Bandera) {
                $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });

                $.ajax({
                    type: "POST",
                    url: '/CatInventario/EnviarInformacion',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        VaciarNuevaCarga();

                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            }
            else {
                AlertaGeneral('Alerta', 'por lo menos debe tener un archivo para continuar')
            }

        }

        function VaciarNuevaCarga() {


            document.getElementById("updFactura").value = "";
            document.getElementById("updPedimento").value = "";
            document.getElementById("updPoliza").value = "";
            document.getElementById("updTarjetaCirculacion").value = "";
            document.getElementById("updPermisoCarga").value = "";
            document.getElementById("updCertificacion").value = "";
            document.getElementById("updCuadroComparativo").value = "";
            document.getElementById("updContratos").value = "";
            document.getElementById("updAnsul").value = "";
            cboEconomico.val('');
            $.unblockUI();
        }
        function ValidarEnvio() {

            var flFactura = document.getElementById("updFactura").files[0];
            var flPedimento = document.getElementById("updPedimento").files[0];
            var flPoliza = document.getElementById("updPoliza").files[0];
            var flTarjetaCirculacion = document.getElementById("updTarjetaCirculacion").files[0];
            var flPermisoCarga = document.getElementById("updPermisoCarga").files[0];
            var flCertificacion = document.getElementById("updCertificacion").files[0];
            var flContratos = document.getElementById("updContratos").files[0];
            var flCuadroComparativo = document.getElementById("updCuadroComparativo").files[0];
            var flAnsul = document.getElementById("updAnsul").files[0];

            if (flFactura != undefined)
                return true;
            if (flPedimento != undefined)
                return true;
            if (flPoliza != undefined)
                return true;
            if (flTarjetaCirculacion != undefined)
                return true;
            if (flPermisoCarga != undefined)
                return true;
            if (flCertificacion != undefined) {
                return true;
            }
            if (flContratos != undefined) {
                return true;
            }
            if (flCuadroComparativo != undefined) {
                return true;
            }
            if (flAnsul != undefined) {
                return true;
            }
            else {

                return false;
            }

        }

        function regresar() {

            $("#divtblDetalle").addClass('hide');
            $("#divtblGeneral").removeClass('hide');

        }

        function loadTabla() {
            regresar();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/CatInventario/LoadTablaAnexosMaquinaria",
                type: "POST",
                datatype: "json",
                data: { ccs: getValoresMultiples("#cboFiltroCentroCostos"), grupo: cboGrupo.val() != "" ? cboGrupo.val() : 0, Economico: cboEconomicoFiltro.val() != "" ? cboEconomicoFiltro.val() : 0, tipo: cboTipoMaquinaria.val() != "" ? cboTipoMaquinaria.val() : 0 },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataSend;
                    SetDataInTables(data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetDataInTables(dataSet) {
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
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
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                    {
                        data: "noEconomico"
                    },
                    {
                        data: "vFactura"
                    },
                    {
                        data: "vPedimento"
                    },
                    {
                        data: "vPoliza"
                    },
                    {
                        data: "vTarCirculacion"
                    },
                    {
                        data: "vPerEspecialCarga"
                    },
                    {
                        data: "vCertificacion"
                    },
                    {
                        data: "vContratos"
                    },
                    {
                        data: "vCuadroComparativo"
                    }, {
                        data: "Factura",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            var Clase = "";
                            if (rowData.vFactura == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Factura.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            // $(td).append('<div class="' + Clase + '" clickAnexo("' + rowData.Factura.tipo + '","' + rowData.Factura.id + ',' + rowData.noEconomicoID + ')"></div>');
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 1 + ',' + rowData.Factura.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Factura.id);
                        }
                    },
                    {
                        data: "Pedimento",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            var Clase = "";
                            if (rowData.vPedimento == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Pedimento.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            //   $(td).append('<div class="' + Clase + '" clickAnexo("' + rowData.Pedimento.tipo + '","' + rowData.Pedimento.id + ',' + rowData.noEconomicoID + ')"></div>');
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 2 + ',' + rowData.Pedimento.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Pedimento.id);

                        }
                    },
                    {
                        data: "Poliza",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            var Clase = "";
                            if (rowData.vPoliza == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Poliza.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            // $(td).append('<div class="' + Clase + '" clickAnexo("' + rowData.Poliza.tipo + '","' + rowData.Poliza.id + ',' + rowData.noEconomicoID + ')"></div>');
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 3 + ',' + rowData.Poliza.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Poliza.id);
                        }
                    },
                    {
                        data: "TarCirculacion",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            var Clase = "";
                            if (rowData.vTarCirculacion == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.TarCirculacion.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            //    $(td).append('<div class="' + Clase + '" clickAnexo("' + rowData.TarCirculacion.tipo + '","' + rowData.TarCirculacion.id + ',' + rowData.noEconomicoID + ')"></div>');
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 4 + ',' + rowData.TarCirculacion.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.TarCirculacion.id);
                        }
                    },
                    {
                        data: "PerEspecialCarga",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var Clase = "";
                            if (rowData.vPerEspecialCarga == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.PerEspecialCarga.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            //  $(td).append('<div class="' + Clase + '" clickAnexo("' + rowData.PerEspecialCarga.tipo + '","' + rowData.PerEspecialCarga.id + ',' + rowData.noEconomicoID + ')"></div>');
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 5 + ',' + rowData.PerEspecialCarga.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.PerEspecialCarga.id);
                        }
                    },
                    {
                        data: "Certificacion",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var Clase = "";
                            if (rowData.vCertificacion == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Certificacion.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }

                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 6 + ',' + rowData.Certificacion.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Certificacion.id);

                        }
                    },
                    {
                        data: "CuadroComparativo",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var Clase = "";
                            if (rowData.vCuadroComparativo == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.CuadroComparativo.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }

                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 7 + ',' + rowData.CuadroComparativo.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.CuadroComparativo.id);

                        }
                    },
                    {
                        data: "Contratos",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var Clase = "";
                            if (rowData.vContratos == 'N/A') {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Contratos.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    //Clase = 'Pendiente';
                                    Clase = 'NoAplica';
                                }
                            }

                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 8 + ',' + rowData.Contratos.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Contratos.id);

                        }
                    },
                    {
                        data: "Ansul",
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var Clase = "";
                            if (rowData.PuedeAnsul == false) {
                                Clase = 'NoAplica';
                            }
                            else {
                                if (rowData.Ansul.id != 0) {
                                    Clase = 'Activo';
                                }
                                else {
                                    Clase = 'Pendiente';
                                }
                            }
                            $(td).append('<div class="' + Clase + '" onclick="clickAnexo(' + 9 + ',' + rowData.Ansul.id + ',' + rowData.noEconomicoID + ',this)"></div>');
                            $(td).attr('data-id', rowData.Ansul.id);
                            // if (rowData.puedeAnsul) {

                            // } else {
                            //     $(td).append('<div class="' + Clase + '"></div>');
                            //     $(td).attr('data-id', rowData.Ansul.id);
                            // }
                        }
                    }
                ],
                "paging": true,
                "info": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Documentos", "<center><h3>Reporte de Documentos </h3></center>"),
                buttons: [
                    {
                        extend: 'excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] //Your Colume value those you want
                        }
                    },
                ],
                columnDefs: [
                    { targets: 1, "visible": false },
                    { targets: 2, "visible": false },
                    { targets: 3, "visible": false },
                    { targets: 4, "visible": false },
                    { targets: 5, "visible": false },
                    { targets: 6, "visible": false },
                    { targets: 7, "visible": false },
                    { targets: 8, "visible": false }
                ]

            });
        }

        function verificarPermisoEliminar() {
            axios.get('/CatInventario/VerificarPermisoEliminarDocumentoEconomico')
                .then(response => {
                    if (response.data) {
                        //Se agrega la cuarta opción de "eliminar".
                        menuConfig.lstOptions.push(
                            {
                                text: `<i class="fa fa-times botonEliminar"></i> Eliminar`, action: "eliminar", fn: () => {
                                    AlertaAceptarRechazarNormal(
                                        'Confirmar Eliminación',
                                        `¿Está seguro de eliminar el archivo?`,
                                        () => eliminarArchivo())
                                }
                            }
                        );
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarArchivo() {
            let idDocumento = +(botonActualizarArchivo.attr('data-id-documento'));

            axios.post('/CatInventario/EliminarArchivoEconomico', { idDocumento })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        loadTabla();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        init();

    };



    $(document).ready(function () {

        maquinaria.Catalogo.AnexosMaquinaria = new AnexosMaquinaria();
    });
})();

function clickAnexo(tipoArchivo, id, idEconomico, clase) {
    var _this = $(this);
    if (!$(clase).hasClass('NoAplica')) {
        btnAddSingle.attr('data-idEconomico', idEconomico);
        $('#botonActualizarArchivo').attr('data-idEconomico', idEconomico);
        $('#botonActualizarArchivo').attr('data-id-documento', id);
        if (id == 0) {
            openModalAlta(tipoArchivo);
        } else {
            if (tipoArchivo == 8) {
                menuConfig = {
                    lstOptions: [
                        { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.id) } },
                        { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.id) } }
                    ]
                }
            } else {
                menuConfig = {
                    lstOptions: [
                        { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.id) } },
                        { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.id) } },
                        { text: `<i class="fa fa-edit"></i> Actualizar`, action: "actualizar", fn: parametros => { openModalActualizar(parametros.tipoArchivo); } }
                    ]
                }
            }
            menuConfig.parametros = { tipoArchivo, id };
            mostrarMenu();
            // downloadURI(id);
        }
    }
}

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

function openModalAlta(tipoArchivo) {

    $("#modalAnexoSimple").modal('show');
    $("#cboTipoDocumentoSingle").fillCombo('/CatMaquina/FillCboTiposArchivos');
    $("#cboTipoDocumentoSingle").val(tipoArchivo);

}

function openModalActualizar(tipoArchivo) {
    $("#modalActualizarArchivo").modal('show');
    $("#selectTipoDocumentoActualizar").fillCombo('/CatMaquina/FillCboTiposArchivos');
    $("#selectTipoDocumentoActualizar").val(tipoArchivo);
}
