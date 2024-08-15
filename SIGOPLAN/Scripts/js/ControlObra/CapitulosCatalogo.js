(function () {
    $.namespace('controlObra.CapitulosCatalogo');

    CapitulosCatalogo = function () {
        tblCapitulos = $("#tblCapitulos");
        btnActualizarObra = $('#btnActualizarObra');
        btnUpload = $('#upload');

        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');

        editarModal = $('#editarModal');

        let isEdit = false;
        const hoy = new Date();

        $('.select2').select2();

        function init() {
            initTableCapitulos();
            intcbo();

            btnUpload.click(fnGuardar);
            btnActualizarObra.click(fnActualizar);
            $("#selectCC").change(fnChangeCC);
            $("#selectCCEdit").change(fnChangeCCEdit);
            $("#selectCapitulo").change(fnCapituloNuevo);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function intcbo() {
            $("#selectCapitulo").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false, '--Nuevo Proyecto--');
            $("#selectCC").fillCombo('/ControlObra/ControlObra/LlenarComboCC', null, false);
            $("#selectCCEdit").fillCombo('/ControlObra/ControlObra/LlenarComboCC', null, false);
            $("#selectFacturacion").fillCombo('/ControlObra/ControlObra/GetPeriodoFacturacion', null, false, null);
        }

        function fnChangeCC() {
            const prefijo = $("#selectCC").find(':selected').attr('data-prefijo');
            $("#selectAutorizante").fillCombo('/ControlObra/ControlObra/LlenarComboAutorizante', { cc: prefijo }, false);
        }

        function fnChangeCCEdit() {
            const prefijo = $("#selectCCEdit").find(':selected').attr('data-prefijo');
            $("#selectAutorizanteEdit").fillCombo('/ControlObra/ControlObra/LlenarComboAutorizante', { cc: prefijo }, false);
        }

        function fnGuardar() {
            //Si tiene seleciconado un proyecto, elimina todo y lo vuelve a generar. Si no, se agregara
            if ($("#selectCapitulo").val() > 0) {

            }
            else {

                if (validarGuardarNuevo())
                    fnGuardarProyecto()
                else
                    AlertaGeneral('Aviso', 'Debe ingresar el nombre del proyecto');
            }
        }

        function fnGuardarProyecto() {
            const data = getDataArchivos();

            $.blockUI({ message: "Preparando información" });
            $.ajax({
                url: '/ControlObra/ControlObra/GuardarArchivos',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (response) {
                    if (response.success) AlertaGeneral('Aviso', 'Se ha cargado la información del proyecto');
                    else AlertaGeneral('Aviso', response.error);

                    recargarTodo();
                    $.unblockUI();
                }
            });
        }

        function getDataArchivos() {
            let formData = new FormData();
            formData.append("nombreObra", JSON.stringify($('#txtProyecto').val()));
            formData.append("periodoFacturacion", JSON.stringify($('#selectFacturacion').val() > 0 ? $('#selectFacturacion').val() : null));
            formData.append("cc_id", JSON.stringify($('#selectCC').val() > 0 ? $('#selectCC').val() : null));
            formData.append("autorizande_id", JSON.stringify($('#selectAutorizante').val() > 0 ? $('#selectAutorizante').val() : null));
            $.each(document.getElementById("file").files, function (i, file) {
                formData.append('file-' + i, file);
            });
            return formData;
        }

        function fnCapituloNuevo() {
            if ($("#selectCapitulo").val() == '--Nuevo Proyecto--')
                $("#txtProyecto").show();
            else
                $("#txtProyecto").hide();
            cargarDatosCapitulo($("#selectCapitulo").val())
        }

        function cargarDatosCapitulo(capidulo_id) {
            $.ajax({
                url: '/ControlObra/ControlObra/GetCapitulo',
                data: { capituloID: capidulo_id },
                success: function (response) {
                    const data = response.capitulo;
                    isEdit = true;

                    $("#selectCC").fillCombo('/ControlObra/ControlObra/LlenarComboCC', null, false);
                    $("#selectCC").val(data.cc_id);
                    fnChangeCC();
                    $("#selectAutorizante").val(data.autorizante_id);
                    $("#selectFacturacion").val(data.periodoFacturacion);
                }
            });
        }

        function validarGuardarNuevo() {
            let valido = false;

            if ($("#txtCapitulo").val() != "")
                valido = true;
            else
                valido = false;

            return valido;
        }

        function fnActualizar() {
            if (validarActualizar())
                editarCapitulo();
            else
                AlertaGeneral('Aviso', 'Debe ingresar datos del proyecto');
        }

        function validarActualizar() {
            let valido = false;
            if ($("#txtCapitulo").val() != "" || dpFechaInicio.val() != "" || dpFechaFin.val() != "")
                valido = true;
            else
                valido = false;
            return valido;
        }

        function editarCapitulo() {
            const url = 'UpdateCapitulo';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    capituloID: $("#btnActualizarObra").val(),
                    capitulo: $("#txtProyectoEdit").val(),
                    fechaInicio: dpFechaInicio.val(),
                    fechaFin: dpFechaFin.val(),
                    cc_id: $("#selectCCEdit").val() > 0 ? $("#selectCCEdit").val() : null,
                    autorizante_id: $("#selectAutorizanteEdit").val() > 0 ? $("#selectAutorizanteEdit").val() : null,
                    periodoFacturacion: $("#selectFacturacionEdit").val() > 0 ? $("#selectFacturacionEdit").val() : null
                },
                success: function (data) {
                    if (data.success)
                        AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    else
                        alert('Aviso', 'Ocurrio un error al guardar')

                    recargarTodo();
                }
            });
        }

        function initTableCapitulos() {
            tblCapitulos = $("#tblCapitulos").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/ControlObra/ControlObra/GetCapitulosCatalogo',
                    dataSrc: function (response) {
                        if (response.EMPTY) {
                            return [];
                        } else {
                            return response.items;
                        }
                    }
                },
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                    tblCapitulos.on('click', '.btn-editar-capitulo', function () {
                        var rowData = tblCapitulos.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetCapitulo',
                            data: { capituloID: rowData["id"] },
                            success: function (response) {
                                const data = response.capitulo;
                                const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                                const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());
                                isEdit = true;
                                $("#btnActualizarObra").val(rowData["id"]);
                                $("#txtProyectoEdit").val(data.capitulo);
                                $("#selectCCEdit").fillCombo('/ControlObra/ControlObra/LlenarComboCC', null, false);
                                $("#selectCCEdit").val(data.cc_id);
                                $("#selectAutorizanteEdit").fillCombo('/ControlObra/ControlObra/LlenarComboAutorizante', { cc: data.cc }, false);
                                $("#selectAutorizanteEdit").val(data.autorizante_id);
                                $("#selectFacturacionEdit").fillCombo('/ControlObra/ControlObra/GetPeriodoFacturacion', null, false, null);
                                $("#selectFacturacionEdit").val(data.periodoFacturacion);
                                dpFechaInicio.datepicker("setDate", fechaInicio);
                                dpFechaFin.datepicker("setDate", fechaFin);
                                editarModal.modal('show');
                            }
                        });
                    });

                    tblCapitulos.on('click', '.btn-eliminar-capitulo', function () {
                        var rowData = tblCapitulos.row($(this).closest('tr')).data();
                        $("#dialogEliminarCapitulo").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarCapitulo").text("¿Está seguro que desea eliminar el capitulo '" + rowData["capitulo"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveCapitulo',
                                        data: { capituloID: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarCapitulo").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarCapitulo").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'capitulo', title: 'Capitulo' },
                    { data: 'fechaInicio', title: 'Fecha Inicio' },
                    { data: 'fechaFin', title: 'Fecha Fin' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-capitulo btn btn-sm btn-warning" type="button" value="' + row.id + '" style=""><i class="fas fa-pencil-alt"></i></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-capitulo btn btn-sm btn-danger" type="button" value="' + row.id + '" style=""><i class="fas fa-trash-alt"></i></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { width: 100, targets: 1 },
                    { width: 100, targets: 2 },
                    { width: 5, targets: 3 },
                    { width: 5, targets: 4 }
                ],
            });
        }

        function recargarTodo() {
            tblCapitulos.ajax.reload(null, false);
            $('#txtProyecto').val('');
            $('#file').val('');
            $('#uploadModal').modal('hide');
            editarModal.modal('hide');
            intcbo();
        }

        init();
    };

    $(document).ready(function () {
        controlObra.CapitulosCatalogo = new CapitulosCatalogo();
    });
})();