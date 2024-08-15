(function () {

    $.namespace('Maquinaria.Rentabilidad.GuardarCorteKubrixConstruplan');

    GuardarCorteKubrixConstruplan = function () {
        const cboTipoCorte = $('#cboTipoCorte');
        const botonBuscar = $('#botonBuscar');
        const botonGuardarCorte = $('#botonGuardarCorte');
        const tablaBalanza = $('#tablaBalanza');
        const botonEnviarArchivos1 = $('#botonEnviarArchivos1');

        let dtTablaBalanza;
        let tipoCorte = "0";

        let collapsedGroups = {};

        const CargarBalanza = new URL(window.location.origin + '/Rentabilidad/CargarBalanza');
        const GuardarCorte = new URL(window.location.origin + '/Rentabilidad/GuardarCorte');
        const EnviarCorreoSemanal = new URL(window.location.origin + '/Rentabilidad/EnviarCorreoSemanal');


        function init() {
            initTablaBalanza();
            addListeners();
        }

        function addListeners() {
            botonBuscar.click(CargarTablaBalanza);
            botonGuardarCorte.click(function (e) {
                var fechaCorte = new Date();
                fechaCorte.setDate(fechaCorte.getDate() + 1);
                if (tipoCorte == "0") {
                    var miercoles = 3 - fechaCorte.getDay();
                    if (miercoles > 0) { miercoles -= 7; }
                    fechaCorte.setDate(fechaCorte.getDate() + miercoles);
                }
                else {
                    fechaCorte = new Date(fechaCorte.getFullYear(), fechaCorte.getMonth(), 0);
                }
                $("#mensajeModalCorte").text("¿Está seguro que desea guardar el corte del dia " + fechaCorte.toLocaleDateString('en-GB').Capitalize() + "?")
                $("#modalConfirmacionCorte").modal("show");
            });
            $("#btnConfirmacionGuardar").click(function (e) {
                setGuardarCorte(e);
            });
            botonEnviarArchivos1.click(enviarCorreoSemanal);
        }

        function initTablaBalanza() {
            dtTablaBalanza = tablaBalanza.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                autoWidth: false,
                searching: true,
                dom: '<<"col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center divLblFiltros">f<t>>',
                columns: [
                    { data: 'cta', title: 'Cta' },
                    { data: 'scta', title: 'SCta' },
                    { data: 'sscta', title: 'SSCta' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'saldoInicial', title: 'Saldo Inicial', render: function (data, type, row) { return '<span class="' + (row.saldoInicial < 0 ? 'negativo' : (row.saldoInicial > 0 ? 'positivo' : '')) + '">$' + parseFloat(row.saldoInicial).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</span>' } },
                    { data: 'cargos', title: 'Cargos', render: function (data, type, row) { return '<span class="' + (row.cargos < 0 ? 'negativo' : (row.cargos > 0 ? 'positivo' : '')) + '">$' + parseFloat(row.cargos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</span>' } },
                    { data: 'abonos', title: 'Abonos', render: function (data, type, row) { return '<span class="' + (row.abonos < 0 ? 'negativo' : (row.abonos > 0 ? 'positivo' : '')) + '">$' + parseFloat(row.abonos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</span>' } },
                    { data: 'saldoActual', title: 'Saldo Actual', render: function (data, type, row) { return '<span class="' + (row.saldoActual < 0 ? 'negativo' : (row.saldoActual > 0 ? 'positivo' : '')) + '">$' + parseFloat(row.saldoActual).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</span>' } },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [0, 1, 2] },
                    { className: "dt-right", "targets": [4, 5, 6, 7] },
                    { width: "5%", "targets": [0, 1, 2] },
                    { width: "35%", "targets": [3] },
                    { width: "12.5%", "targets": [4, 5, 6, 7] }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrix_filter input').addClass("form-control input-sm");
                },
                createdRow: function (row, data, dataIndex) {
                    if (data.cta == 0) $(row).addClass('cuentaPrincipal');
                    else {
                        if (data.scta == 0) $(row).addClass('sCuentaPrincipal');
                        else {
                            if (data.sscta == 0) $(row).addClass('ssCuentaPrincipal');
                        }
                    }
                    if (data['scta'] == 0) {
                        $(row).hide();
                    }
                },
                rowGroup: {
                    dataSrc: ['cta'],
                    startRender: function (rows, group) {
                        var collapsed = !collapsedGroups[group];

                        rows.nodes().each(function (r) {
                            r.style.display = collapsed ? 'none' : ($(r).hasClass("sCuentaPrincipal") || $(r).hasClass("cuentaPrincipal") ? 'none' : '');
                            var rowActual = $(r);
                        });
                        var data = rows.data();

                        var cta = group;
                        var scta = 0;
                        var sscta = 0;
                        var rowData = $.grep(data, function (e) { return e.cta == group && e.scta == 0 && e.sscta == 0; })[0];
                        var descripcion = rowData.descripcion;
                        var saldoInicial = rowData.saldoInicial;
                        var cargos = rowData.cargos;
                        var abonos = rowData.abonos;
                        var saldoActual = rowData.saldoActual;
                        // Add category name to the <tr>. NOTE: Hardcoded colspan
                        return $('<tr/>')
                            .append('<td class="text-center">' + cta + '</td>' +
                            '<td class="text-center">' + scta + '</td>' +
                            '<td class="text-center">' + sscta + '</td>' +
                            '<td class="text-center">' + descripcion + '</td>' +
                            '<td class="text-center ' + (saldoInicial < 0 ? 'negativo' : (saldoInicial > 0 ? 'positivo' : '')) + '">' + '$' + parseFloat(saldoInicial).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</td>' +

                            '<td class="text-center ' + (cargos < 0 ? 'negativo' : (cargos > 0 ? 'positivo' : '')) + '">' + '$' + parseFloat(cargos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</td>' +
                            '<td class="text-center ' + (abonos < 0 ? 'negativo' : (abonos > 0 ? 'positivo' : '')) + '">' + '$' + parseFloat(abonos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</td>' +
                            '<td class="text-center ' + (saldoActual < 0 ? 'negativo' : (saldoActual > 0 ? 'positivo' : '')) + '">' + '$' + parseFloat(saldoActual).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '</td>')
                            .attr('data-name', group)
                            .toggleClass('collapsed', collapsed);
                    }
                },
                drawCallback: function (settings) {
                    $('#tablaBalanza tbody').on('click', 'tr.dtrg-start', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var name = $(this).data('name');
                        collapsedGroups[name] = !collapsedGroups[name];
                        dtTablaBalanza.draw(false);
                    });
                },
                rowCallback: function (row, data, index) {
                    if (data['scta'] == 0) {
                        $(row).hide();
                    }
                },
            });
        }

        function CargarTablaBalanza() {
            $.post(CargarBalanza, { tipo: cboTipoCorte.val(), empresa: 1 })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        dtTablaBalanza.clear().rows.add(response.balanza).draw();
                        tipoCorte = cboTipoCorte.val();
                        //var fechaCorte = new Date();
                        //if (cboTipoCorte.val() == "0") {
                        //    var miercoles = 3 - (new Date()).getDay();
                        //    if (miercoles > 0) { miercoles -= 7; }
                        //    fechaCorte.setDate(fechaCorte.getDate() + miercoles);
                        //}
                        //else { fechaCorte = new Date(fechaCorte.getFullYear(), fechaCorte.getMonth(), 0); }
                        $("div.divLblFiltros")
                            .html("<b>Reporte de balanza para el " + response.fechaCorte + "</b>");
                        $('div.divLblFiltros').css("font-size", "20px");
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function setGuardarCorte(e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            $.post(GuardarCorte, { tipo: tipoCorte, empresa: 1 })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        $("#modalConfirmacionCorte").modal("hide");
                        AlertaGeneral('Operación exitosa', 'Se guardó el corte');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function enviarCorreoSemanal()
        {
            $.post(EnviarCorreoSemanal, { tipo: tipoCorte, empresa: 1 })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        $("#modalConfirmacionCorte").modal("hide");
                        AlertaGeneral('Operación exitosa', 'Se envío el correo correctamente');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        init();
    };

    $(document).ready(function () {
        Maquinaria.Rentabilidad.GuardarCorteKubrixConstruplan = new GuardarCorteKubrixConstruplan();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();


