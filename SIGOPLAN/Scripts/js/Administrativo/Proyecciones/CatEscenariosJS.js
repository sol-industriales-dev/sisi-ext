
(function () {

    $.namespace('administracion.proyecciones.CatEscenarios');

    CatEscenarios = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Catalogó de Escenarios',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        globalID = 0;
        tblEscenarios = $("#tblEscenarios");
        btnBuscar = $("#btnBuscar"),
        btnNuevo = $("#btnNuevo"),
        modalNewEscenario = $("#modalNewEscenario"),
        modaltitle = $("#modaltitle"),
        chkEscenarioPadre = $("#chkEscenarioPadre"),
        cboEscenariosPrincipalesFiltro = $("#cboEscenariosPrincipalesFiltro"),
        cboEscenariosPrincipales = $("#cboEscenariosPrincipales"),
        tbFiltroDescripcion = $("#tbFiltroDescripcion"),
        btnGuardarNuevoEscenario = $("#btnGuardarNuevoEscenario"),
        tbModalDescEscenario = $("#tbModalDescEscenario");


        function init() {
            cboEscenariosPrincipales.prop('disabled', true);
            cboEscenariosPrincipales.fillCombo('/Proyecciones/fillCboEscenariosPadre');
            chkEscenarioPadre.change(VerEscenarios);
            cboEscenariosPrincipalesFiltro.fillCombo('/Proyecciones/fillCboEscenariosPadre');

            LoadTable();
            btnNuevo.click(openModal);
            btnGuardarNuevoEscenario.click(guardarNuevoEscenario);
            btnBuscar.click(LoadTable);

            globalID = 0;
        }

        function guardarNuevoEscenario() {
            $.ajax({
                url: '/Proyecciones/guardarEscenario',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: GetData() }),
                success: function (response) {
                    fnLimpiarDataEscenario();
                    AlertaGeneral("Confirmación", "Fue Agregado un nuevo Escenario");
                },
                error: function (response) {
                    AlertaGeneral("Error", response.message);
                }
            });
        }

        function fnLimpiarDataEscenario() {
            cboEscenariosPrincipales.val('');
            tbModalDescEscenario.val('');

        }

        function GetData() {
            return {
                id: globalID,
                PadreID: cboEscenariosPrincipales.val(),
                descripcion: tbModalDescEscenario.val(),

                estatus: 1
            }
        }


        function VerEscenarios() {
            if (chkEscenarioPadre.is(':checked')) {
                cboEscenariosPrincipales.prop('disabled', false);
                cboEscenariosPrincipales.fillCombo('/Proyecciones/fillCboEscenariosPadre', null, false);
            }
            else {
                cboEscenariosPrincipales.prop('disabled', true);

                cboEscenariosPrincipales.clearCombo();
                cboEscenariosPrincipales.val('');
            }
        }

        function openModal() {
            globalID = 0;
            fnLimpiarDataEscenario();
            modalNewEscenario.modal('show');
        }

        function LoadTable() {
            $.blockUI({ message: 'Cargando información...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Proyecciones/fillTblEscenarios',
                async: false,
                data: { escenario: cboEscenariosPrincipalesFiltro.val() == "" ? 0 : cboEscenariosPrincipalesFiltro.val(), descripcion: tbFiltroDescripcion.val() },
                success: function (response) {

                    var tblEscenariosData = response.tblEscenariosData;
                    tblEscenarios.bootgrid("clear");
                    tblEscenarios.bootgrid("append", tblEscenariosData);
                    tblEscenarios.bootgrid('reload');

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function initGrid() {
            tblEscenarios.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "editar": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-id=" + row.id + ">" + //  "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'>" +
                                        "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "baja": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index=" + row.id + ">" +//+ "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'>" +
                                       "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEscenarios.find(".modificar").on("click", function (e) {
                    Actualizacion = 2;
                    idEscenario = $(this).attr("data-id");
                    globalID = Number(idEscenario);
                    LoadData(idEscenario);

                });
            });
        }

        function LoadData(idEscenario) {
            $.blockUI({ message: 'Cargando información...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Proyecciones/loadDataEscenario',
                async: false,
                data: { idObj: idEscenario },
                success: function (response) {
                    modalNewEscenario.modal('show');
                    if (response.success) {
                        var DataInfo = response.obj;

                        if (DataInfo.PadreID != 0) {
                            cboEscenariosPrincipales.prop('disabled', false);
                            cboEscenariosPrincipales.fillCombo('/Proyecciones/fillCboEscenariosPadre', null, false);
                            cboEscenariosPrincipales.val(DataInfo.PadreID);
                        }
                        else {
                            cboEscenariosPrincipales.prop('disabled', true);

                            cboEscenariosPrincipales.clearCombo();
                            cboEscenariosPrincipales.val('');
                        }
                        tbModalDescEscenario.val(DataInfo.descripcion);


                    }
                    else {
                        AlertaGeneral("Alerta", "No se encontró información");
                    }


                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        initGrid();
        init();
    };

    $(document).ready(function () {

        administracion.Proyecciones.CatEscenarios = new CatEscenarios();
    });
})();

