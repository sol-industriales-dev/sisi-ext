(() => {
    $.namespace('Capacitacion.HorasHombreCapacitacion');

    HorasHombreCapacitacion = function () {
        //#region Selectores
        const tblHorasHombreCapacitacion = $('#tblHorasHombreCapacitacion');
        const btnBuscar = $('#btnBuscar');
        const cboProyecto = $('#cboProyecto');
        const dtAño = $('#dtAño');
        const btnDescargar = $('#btnDescargar');
        const report = $('#report');
        const btnReporte = $('#btnReporte');
        //#endregion

        let fechaActualServidor = new Date;
        let fechaActualServidorOpera = new Date;
        let fechaActualr = new Date;
        let ArrAnos = [];
        let dtHorasHombres;

        (function init() {
            initTablaHoras();
            cargarCombos();
            iniciarProcesoFechas();
        })();

        btnBuscar.click(cargarInformacion);

        btnDescargar.click(function () {
            $.blockUI({ message: 'Procesando...' });
            let parametros = getComboCCPos();

            axios.post('creaExcelito', { parametros: parametros }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
                location.href = `crearExcelHorasHombreCapacitacion`;
            });

            $.unblockUI();
        });

        btnReporte.click(verReporte);

        function iniciarProcesoFechas() {
            fechaActualServidorOpera = moment(fechaActualServidor).format('YYYY');
            fechaActualServidorOpera = fechaActualServidorOpera - 10;

            for (let index = 0; index < 10; index++) {
                fechaActualServidorOpera++;
                let element = { ano: fechaActualServidorOpera };
                ArrAnos.push(element);
            }

            for (let index = 0; index < 10; index++) {
                fechaActualServidorOpera++;
                let element = { ano: fechaActualServidorOpera };
                ArrAnos.push(element);
            }

            let groupOption = ``;

            ArrAnos.forEach(y => {
                groupOption += `<option value="${y.ano}">${y.ano}</option>`;
            });

            dtAño.append(groupOption);
            fechaActualr = moment(fechaActualr).format('YYYY');
            dtAño.val(fechaActualr);
        }

        function initTablaHoras() {
            dtHorasHombres = tblHorasHombreCapacitacion.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                searching: false,
                paging: false,
                dom: 't',
                columns: [
                    {
                        data: 'centrocosto', width: '6%',
                        render: function (data, type, row) {
                            let div = ``;
                            if (data == "CC") {
                                div = `<span class="badge badge-dark" id="${data}">${data}</span>`;
                            } else {
                                div = data;
                            }
                            return div;
                        }
                    },
                    {
                        data: 'descripcion', width: '7%',
                        render: function (data, type, row) {
                            let div = ``;
                            if (data == "DESCRIPCION") {
                                div = `<span class="badge badge-dark" id="${data}" >${data}</span>`;
                            } else {
                                div = data;
                            }
                            return div;
                        }
                    },
                    {
                        data: 'totalPersonal', width: '7%',
                        render: function (data, type, row) {
                            let div = ``;
                            if (data == "TOTAL PERSONAL OPERATIVO") {
                                div = `<span class="badge badge-dark" id="TOTALPERSONALOPERATIVO" >${data}</span>`;
                            } else {
                                div = data;
                            }
                            return div;
                        }
                    },
                    {
                        data: 'lstGlobal', width: '300px',
                        render: function (data, type, row) {
                            let div = ``;
                            if (data.descripcion == "GLOBAL") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff"> ${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstEnero', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "ENERO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstFebrero', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "FEBRERO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstMarzo', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "MARZO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstAbril', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "ABRIL") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstMayo', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "MAYO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstJunio', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "JUNIO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstJulio', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "JULIO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstAgosto', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "AGOSTO") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstSeptiembre', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "SEPTIEMBRE") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstOctubre', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "OCTUBRE") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstNoviembre', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "NOVIEMBRE") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    },
                    {
                        data: 'lstDiciembre', width: '300px',
                        render: function (data, type, row) {

                            let div = ``;
                            if (data.descripcion == "DICIEMBRE") {
                                div = ` <table class="table-bordered stripe order-column" style="width: 100%;"><tr><th colspan="2">${data.descripcion}</th></tr><tr><th> <span class="badge badge-dark" style="width:100px;">${data.HrsCap}  </span>   </th><th> <span class="badge badge-dark" style="width:100px;">${data.HrsTrab}   </span>  </th><tr></table>`;
                            } else {
                                div = `<table class="CuerpoTabla" style="width:100%"><tr><td><span class="badge badge-dark" style="width:100px;background-color:#fff;color:#000;">${data.HrsCap}</span>  </td> <td><span class="badge badge-dark" style="width:100px;color:#000;background-color:#fff">${data.HrsTrab}</span>   </td></tr></table>`
                            }

                            return div;
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    if (data.centrocosto == "GLOBAL PERSONAL") {
                        $('td:eq(0)', row).attr('colspan', 2);
                        $('td:eq(1)', row).css('display', 'none');
                    }
                }
            });

            $('.sorting_disabled').remove();
        }

        function cargarCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    cboProyecto.append('<option value="Todos">Todos</option>');

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        cboProyecto.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#cboProyecto');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarInformacion() {
            let parametros = getComboCCPos();

            axios.post('CargarInformacionHorasHombre', parametros).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    $('#HrsCap').val(response.data.promedioHRSCapacitaciones);
                    $('#HrsTrab').val(response.data.promedioHRSTrabajadas);

                    AddRows(tblHorasHombreCapacitacion, response.data.listaHoras);

                    $('.order-column').parent().css('padding', 0);
                    $('.CuerpoTabla').find('tbody').css('border', 'none');
                    $('.CuerpoTabla').find('tbody').find('td').css('border', 'none');
                    $('.table-bordered').find('th').css('background-color', '#404040');
                    $('.table-bordered').find('th').css('color', '#fff');
                    $('.table-bordered').find('th').css('text-align', 'center');
                    $('.table-bordered').find('th').find('span').css('background-color', '#404040');
                    $('#CC').css('background-color', '#404040');
                    $('#DESCRIPCION').css('background-color', '#404040');
                    $('#DESCRIPCION').css('width', '180px');
                    $('#TOTALPERSONALOPERATIVO').css('background-color', '#404040');
                    $('#CC').parent().css('background-color', '#404040');
                    $('#DESCRIPCION').parent().css('background-color', '#404040');
                    $('#TOTALPERSONALOPERATIVO').parent().css('background-color', '#404040');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getComboCCPos() {
            return { año: dtAño.val(), comboCC: getComboCC() };
        }

        function getComboCC() {
            let returitem = [];
            for (let i = 0; i < $('#cboProyecto').val().length; i++) {
                selCC = '';
                Prefijo = '';
                selCC = $('#cboProyecto').find("option[value=" + $('#cboProyecto').val()[i] + "]");
                Prefijo = selCC.attr("empresa");

                const element = {
                    cc: $('#cboProyecto').val()[i],
                    empresa: Prefijo
                };
                returitem.push(element);
            }

            return returitem;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.draw();
            dt.rows.add(lst).draw(false);
        }

        function verReporte() {
            $.blockUI({ message: 'Generando imprimible...' });
            let parametros = getComboCCPos();
            axios.post('CrearReporteHorasHombres', { parametros: parametros }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
                let { success } = response.data;
                if (success) {
                    var path = `/Reportes/Vista.aspx?idReporte=214`;
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                }
            });
        }
    }

    $(document).ready(() => Capacitacion.HorasHombreCapacitacion = new HorasHombreCapacitacion()).ajaxStart(() => $.blockUI({ message: 'Procesando...' })).ajaxStop($.unblockUI);
})();