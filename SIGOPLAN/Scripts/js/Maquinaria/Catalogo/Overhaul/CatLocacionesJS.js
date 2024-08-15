
(function () {

    $.namespace('maquinaria.catalogo.overhaul.CatLocaciones');

    CatLocaciones = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Catalogó de Locaciones',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tbLocacionFiltro = $("#tbLocacionFiltro"),
        cboEstatusFiltro = $("#cboEstatusFiltro"),
        btnBuscar = $("#btnBuscar"),
        btnNuevo = $("#btnNuevo"),
        tblLocacion = $("#tblLocacion"),
        locacionModal = $("#locacionModal"),
        txtTituloModal = $("#txtTituloModal"),
        frmLocacion = $("#frmLocacion"),
        tbLocacionModal = $("#tbLocacionModal"),
        cboEstatusModal = $("#cboEstatusModal"),
        btnModalGuardar = $("#btnModalGuardar"),
        btnModalCancelar = $("#btnModalCancelar");
        accion = 1;

        function init() {
            LoadTabla();
            btnBuscar.click(LoadTabla);
            btnNuevo.click(openModal);

        }

        function openModal() {
            VaciarCampos();
            locacionModal.modal('show');
        }

        function VaciarCampos() {

            tbLocacionModal.val('');
            cboEstatusModal.val("1").prop('disabled', true);
        }

        function LoadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatLocaciones/getDataTable',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ locacion: tbLocacionFiltro.val(), estatus: (cboEstatusFiltro.val() == "1" ? true : false) }),
                success: function (response) {

                    var TablaLocacion = response.tblRes;
                    tblLocacion.bootgrid("clear");
                    //tblLocacion.bootgrid("append", TablaLocacion);
                    tblLocacion.bootgrid('reload');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initGrid() {
            tblLocacion.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'>" +
                                        "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-index='" + row.id + "'>" +
                                       "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblLocacion.find(".modificar").on("click", function (e) {
                    accion = 2;
                    idObj = $(this).attr("data-index");
                    LoadData(idObj, accion);
                });

                tblLocacion.find(".eliminar").on("click", function (e) {
                    accion = 3;
                    idObj = $(this).attr("data-index");
                    LoadData(idObj, accion);
                });

            });
        }

        function LoadData(idObj, accion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatLocaciones/getDataRegistro',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: idObj }),
                success: function (response) {
                    if (response.success) {
                        SetInfo(response.res);
                    }
                    else {
                        AlertaGeneral("Alerta", "No se encontro la información solicitada.")
                    }

                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }

        function SetInfo(obj) {

            VaciarCampos();
            tbLocacionModal.val(obj.descripcion);
            cboEstatusModal.val(obj.estatus ? "1" : "0").prop('disabled', false);

        }

        init();
    };

    $(document).ready(function () {

        maquinaria.catalogo.overhaul.CatLocaciones = new CatLocaciones();
    });
})();

