(function () {
    $.namespace('administrativo.cotizaciones.capturacotizacion');
    capturacotizacion = function () {
        mensajes = {
            NOMBRE: 'Reporte Comparativa de Tipos',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        tbFechaProbableF = $("#tbFechaProbableF"),
        tbContacto = $("#tbContacto"),
        txtFechaI = $("#txtFechaI"),
        txtFechaF = $("#txtFechaF"),
        registroActual = 0,
        cboTipoMoneda = $("#cboTipoMoneda"),
        tbMargen = $("#tbMargen"),
        tbFechaStatus = $("#tbFechaStatus"),
        txtRevision = $("#txtRevision"),
        txtBFolio = $("#txtBFolio"),
        txtBCliente = $("#txtBCliente"),
        txtBProyecto = $("#txtBProyecto"),
        cboBEstatus = $("#cboBEstatus"),
        txtFolio = $("#txtFolio"),
        txtCliente = $("#txtCliente"),
        txtProyecto = $("#txtProyecto"),
        txtMonto = $("#txtMonto"),
        cboEstatus = $("#cboEstatus"),
        btnGuardarObj = $("#btnGuardarObj"),
        btnBuscar = $("#btnBuscar"),
        modal = $("#modal"),
        tblData = $("#tblData"),
        btnEliminar = $("#btnEliminar"),
        btnImprimir = $("#btnImprimir"),
        btnEliminarObj = $("#btnEliminarObj"),
        btnAgregar = $("#btnAgregar"),
        fupAdjunto = $("#fupAdjunto"),
        divVerComentario = $("#divVerComentario"),
        ulComentarios = $("#ulComentarios"),
        btnAddComentario = $("#btnAddComentario"),
        txtComentarios = $("#txtComentarios"),
        cboBCC = $("#cboBCC"),
        cboCC = $("#cboCC");


        function init() {

            var now = new Date(),
            year = now.getYear() + 1900;
            txtFechaI.datepicker().datepicker("setDate", "01/01/" + year);
            txtFechaF.datepicker().datepicker("setDate", new Date());


            initTable();
            tbMargen.DecimalFixPr(2);

            cboBCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            cboCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            cboBEstatus.fillCombo('/Administrativo/Cotizaciones/FillCboStatus', null, false);
            if (_GEditar == 1) {
                btnAgregar.hide();
            }
            else {
                btnAgregar.click(clickAgregar);
            }
            btnImprimir.click(fnImprimir);
            btnGuardarObj.click(fnGuardar);
            btnEliminar.click(fnOpenEliminar);
            txtMonto.DecimalFixPs(2);
            btnEliminarObj.click(fnEliminar);
            btnBuscar.click(fnBuscar);
            btnAddComentario.click(insertCommentary);
            cboCC.change(fnGetFolio);

            tbFechaStatus.datepicker().datepicker("setDate", new Date());
            tbFechaProbableF.datepicker().datepicker("setDate", new Date());
        }


        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#txtFechaI")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }

                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#txtFechaF").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(year, 11, 31),
                  onChangeMonthYear: function (y, m, i) {
                      var d = i.selectedDay;
                      $(this).datepicker('setDate', new Date(y, m - 1, d));
                      $(this).trigger('change');
                  }
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
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

        function fnGetFolio() {
            $.ajax({
                url: '/Administrativo/Cotizaciones/GetFolioCotizacion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ CC: cboCC.val() }),
                success: function (response) {
                    txtFolio.val(response.data).prop('disabled', true);
                },
                error: function (response) {
                    AlertaGeneral("Error", response.message);
                }
            });
        }

        function fnImprimir(e) {
            verReporte(57, "", "H");
            e.preventDefault();
        }
        function verReporte(idReporte, parametros, orientacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function initTable() {
            tblData = $("#tblData").DataTable({
                ajax: {
                    url: '/Administrativo/Cotizaciones/obtenerCotizacion',
                    dataSrc: 'dataMain',
                    type: 'POST',
                    data: function (d) {
                        d.folio = $("#txtBFolio").val(),
                        d.cc = $("#cboBCC").val(),
                        d.cliente = $("#txtBCliente").val(),
                        d.proyecto = $("#txtBProyecto").val(),
                        d.estatus = Number($("#cboBEstatus").val())
                        d.fechaI = $("#txtFechaI").val(),
                        d.fechaF = $("#txtFechaF").val()
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
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
                columns: [
                    { data: 'id' },
                    { data: 'folio' },
                    { data: 'cc' },
                    { data: 'cliente' },
                    { data: 'proyecto' },
                    { data: 'monto' },
                    { data: 'Margen' },
                    { data: 'fechaStatus' },
                    { data: 'fechaProbableF' },
                    { data: 'contacto' },
                    { data: 'estatus' },
                    { data: 'tipoMoneda' },
                    { data: 'btnSeguimiento', width: "25px" },
                    { data: 'btnEditar', width: "25px" }
                ],
                columnDefs: [
                    { targets: 0, "visible": false },
                    { targets: 13, "visible": (_GEditar==1?false:true) }
                ]
            });

        }
        function fnOpenEliminar() {
            if ($("#tblData").find("input:checked").length > 0) {
                dialogEliminarObj.modal("show");
            }
            else {
                AlertaGeneral("Alerta", "¡Debe seleccionar almenos un registro!");
            }
        }
        function fnEliminar() {
            var lista = [];

            $("#tblData").find("input:checked").each(function (i, ob) {
                lista.push($(ob).data("id"));
            });
            if (lista.length > 0) {
                $.ajax({
                    url: '/Administrativo/Cotizaciones/eliminarCotizacion',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ lista: lista }),
                    success: function (response) {
                        tblData.ajax.reload(null, false);
                        AlertaGeneral("Confirmación", "¡Registros eliminados correctamente!");
                    },
                    error: function (response) {
                        AlertaGeneral("Error", response.message);
                    }
                });
                tblData.ajax.reload(null, false);
            }
            else {
                AlertaGeneral("Alerta", "¡Debe seleccionar almenos 1 registro para eliminar!");
            }
        }
        function clickAgregar() {
            limpiarCampos();
            btnGuardarObj.val("Guardar");
            modal.modal("show");
        }
        function fnGuardar() {
            if (validateModal()) {
                var obj = {};
                obj.id = registroActual;
                obj.folio = txtFolio.val();
                obj.cc = cboCC.val();
                obj.cliente = txtCliente.val();
                obj.proyecto = txtProyecto.val();
                obj.monto = txtMonto.getVal(2);
                obj.Estatus = cboEstatus.val();
                obj.activo = true;
                obj.comentariosCount = 0;
                obj.revision = txtRevision.val();
                obj.Margen = tbMargen.getVal(2);
                obj.tipoMoneda = cboTipoMoneda.val();
                obj.contacto = tbContacto.val();
                obj.fechaProbableF = tbFechaProbableF.val();

                obj.fechaStatus = tbFechaStatus.val();
                $.ajax({
                    type: "POST",
                    url: "/Administrativo/Cotizaciones/guardarCotizacion",
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        if (response.success === true) {
                            tblData.ajax.reload(null, false);
                            AlertaGeneral("Confirmación", "¡Registro guardado correctamente!");
                            $("#modal .close").click();
                        }
                        else {
                            AlertaGeneral("Error", "¡Ocurrio un error al guardar intente de nuevo!");
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }
        function fnBuscar() {
            btnImprimir.show();
            tblData.ajax.reload(null, false);
        }
        function validateModal() {
            var state = true;
            if (!validarCampo(txtFolio)) { state = false; }
            if (!validarCampo(cboCC)) { state = false; }
            if (!validarCampo(txtCliente)) { state = false; }
            if (!validarCampo(txtProyecto)) { state = false; }
            if (!validarCampo(txtMonto)) { state = false; }
            if (!validarCampo(cboEstatus)) { state = false; }

            return state;
        }
        function limpiarCampos() {
            registroActual = 0;

            txtFolio.val("").prop('disabled', true);
            txtCliente.val("");
            txtProyecto.val("");
            txtMonto.setVal(0);
            cboEstatus.val(1);
            cboCC.val("").prop('disabled', false);
            txtRevision.val("0");
            tbMargen.setVal(0);
            cboTipoMoneda.val("1");
            tbContacto.val('');
            tbFechaProbableF.datepicker().datepicker("setDate", new Date());
        }
        function insertCommentary(e) {
            if (validateComentario()) {
                var obj = getNewCommentary();

                var formData = new FormData();
                //var filesVisor = document.getElementById("fupAdjunto").files.length;
                var file = document.getElementById("fupAdjunto").files[0];

                formData.append("fupAdjunto", file);
                formData.append("obj", JSON.stringify(obj));

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: "/Administrativo/Cotizaciones/guardarComentario",
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        fupAdjunto.val("");
                        $.unblockUI();
                        var data = response.data;
                        setComentarios(data);
                        txtComentarios.val("");
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }
        function setComentarios(data) {
            ulComentarios.empty();
            ulComentarios.html("");
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                if (e.adjuntoNombre != null && e.adjuntoNombre != "") {
                    htmlComentario += "        <div class='timeline-footer'>";
                    htmlComentario += "             <a href='/Administrativo/Cotizaciones/getComentarioArchivoAdjunto/?id=" + e.id + "' class='openComentarios'></span>Descargar: " + e.adjuntoNombre + "</a>";
                    htmlComentario += "        </div>";
                }
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }
        function getNewCommentary() {
            var r = {};
            r.id = 0;
            r.cotizacionID = registroActual;
            r.comentario = txtComentarios.val();
            r.usuarioNombre = "";
            r.usuarioID = 0;
            r.fecha = "";
            r.tipo = 'new';
            r.adjuntoNombre = "";
            return r;
        }
        function validateComentario() {
            var state = true;
            if (!validarCampo(txtComentarios)) { state = false; }
            return state;
        }
        init();
    };
    $(document).ready(function () {
        administrativo.cotizaciones.capturacotizacion = new capturacotizacion();
    });
})();

function clickActualizar(id, folio, cliente, proyecto, monto, utilidad, fechaStatus, estatus, cc, revision, tipoMoneda, fechap, contacto) {
    registroActual = id;

    tbFechaStatus.val(fechaStatus);
    tbMargen.setVal(utilidad);
    txtFolio.val(folio);
    cboCC.val(cc).prop('disabled', true);
    txtCliente.val(cliente);
    txtProyecto.val(proyecto);
    txtMonto.setVal(monto);
    cboEstatus.val(estatus);
    txtRevision.val(revision);
    btnGuardarObj.val("Actualizar");
    cboTipoMoneda.val(tipoMoneda);
    tbFechaProbableF.val(fechap);
    tbContacto.val(contacto);

    modal.modal("show");
}
function clickComentarior(id) {
    registroActual = id;
    $.ajax({
        url: '/Administrativo/Cotizaciones/obtenerComentarios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ id: registroActual }),
        success: function (response) {
            var data = response.data;
            setComentarios(data);
            divVerComentario.modal("show");
        },
        error: function (response) {
            AlertaGeneral("Error", response.message);
        }
    });

}
function setComentarios(data) {
    ulComentarios.empty();
    ulComentarios.html("");
    var htmlComentario = "";
    $.each(data, function (i, e) {
        htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
        htmlComentario += "    <div class='timeline-item'>";
        htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
        htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
        htmlComentario += "        <div class='timeline-body'>";
        htmlComentario += "             " + e.comentario;
        htmlComentario += "        </div>";
        if (e.adjuntoNombre != null && e.adjuntoNombre != "") {
            htmlComentario += "        <div class='timeline-footer'>";
            htmlComentario += "             <a href='/Administrativo/Cotizaciones/getComentarioArchivoAdjunto/?id=" + e.id + "' class='openComentarios'></span>Descargar: " + e.adjuntoNombre + "</a>";
            htmlComentario += "        </div>";
        }
        htmlComentario += "    </div>";
        htmlComentario += "</li>";
    });
    ulComentarios.html(htmlComentario);
}