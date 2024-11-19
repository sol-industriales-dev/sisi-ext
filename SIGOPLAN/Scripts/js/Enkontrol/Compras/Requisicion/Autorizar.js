(() => {
    $.namespace('Enkontrol.Compras.Requisicion.Autorizar');
    Autorizar = function () {
        //#region Selectores
        const selCC = $('#selCC');
        const selCCMulti = $('#selCCMulti');
        const tblAuth = $('#tblAuth');
        const btnAutorizar = $('#btnAutorizar');
        const radioBtn = $('.radioBtn a');
        const headerAut = $('#headerAut');
        const report = $("#report");
        //#endregion

        _counReq = 0;

        (function init() {
            $('.select2').select2();
            initAutorizacion();

            btnAutorizar.click(autorizar);
            selCCMulti.change(loadTblAuth);
            radioBtn.click(clickRadios);

            // seleccionarTodosMultiselect('#selCCMulti');
            loadTblAuth();
        })();

        $('#tblInsumos').on('click', '.btn-quitar-insumo', function () {
            let campoCantidad = $(this).closest('tr').find('.cantidad');

            if ($(this).attr('data-quitar') == 'true') {
                $(this).removeClass('btn-danger').addClass('btn-default');
                $(this).attr('data-quitar', 'false');

                campoCantidad.removeClass('campoInvalido');
                campoCantidad.val(campoCantidad.attr('data-valor-anterior')).change();
            } else {
                $(this).removeClass('btn-default').addClass('btn-danger');
                $(this).attr('data-quitar', 'true');

                campoCantidad.attr('data-valor-anterior', campoCantidad.val());
                campoCantidad.addClass('campoInvalido');
                campoCantidad.val(0).change();
            }
        });

        function autorizar() {
            let lst = getLstReq();

            if (lst.length > 0) {
                $.post('/Enkontrol/Requisicion/setAuth', { lst }).then(response => {
                    if (response.success) {
                        AlertaGeneral("Aviso", `${getRadioValue("radAuth") ? "Desautorización" : "Autorización"} realizada.`);

                        loadTblAuth();

                        selCCMulti.fillCombo('/Enkontrol/Requisicion/FillComboCcReq', { isAuth: getRadioValue("radAuth") }, false, 'Todos');
                        // convertToMultiselect('#selCCMulti');
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                AlertaGeneral("Aviso", "No has seleccionado ninguna requisición.");
            }
        }

        function loadTblAuth() {
            if (getValoresMultiples('#selCCMulti').length > 0) {
                $.post('/Enkontrol/Requisicion/getEncReq', {
                    cc: getValoresMultiples('#selCCMulti'),
                    isAuth: $(`a.active[data-toggle=radAuth]`).data('title')
                }).always($.unblockUI).then(response => {
                    if (response.success) {
                        AddRows(tblAuth, response.lstReq);

                        _counReq = 0;
                        btnAutorizar.addClass("disabled");
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                tblAuth.DataTable().clear().draw();
            }
        }

        function clickRadios() {
            let isAuth = !getRadioValue($(this).data('toggle'));

            aClick(this);

            headerAut.text(isAuth ? 'Autorizar' : 'Desautorizar');

            selCCMulti.fillCombo('/Enkontrol/Requisicion/FillComboCcReq', { isAuth: !isAuth }, false, 'Todos');
            // convertToMultiselect('#selCCMulti');

            tblAuth.DataTable().clear().draw();

            // loadTblAuth();
        }

        function aClick(esto) {
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');

            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }

        function getLstReq() {
            let listaSeleccionados = tblAuth.DataTable().rows().data()
                .filter(fila => fila.flagCheckBox).toArray()
                .map(x => {
                    return {
                        cc: x.cc,
                        numero: x.numero,
                        stAutoriza: x.isAuth,
                        PERU_tipoRequisicion: x.PERU_tipoRequisicion ?? ""
                    }
                });

            return listaSeleccionados;
        }

        function initAutorizacion() {
            selCCMulti.fillCombo('/Enkontrol/Requisicion/FillComboCcReq', { isAuth: false }, false, 'Todos');
            // convertToMultiselect('#selCCMulti');
            selCCMulti.find("option[value='Todos']").prop("selected", "value");
            selCCMulti.trigger("change.select2");
            dtAuth = tblAuth.DataTable({
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                columns: [
                    { data: 'ccNom', title: 'CC' },
                    { data: 'numero', title: 'Número' },
                    { data: 'otFolio', title: 'OT' },
                    { data: 'economico', title: 'No. Economico' },
                    { data: 'solNom', title: 'Solicitante' },
                    {
                        data: 'fecha', title: 'Fecha',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data.parseDate().toLocaleDateString());
                        }
                    },
                    { data: 'cantidadTotal', title: 'Cantidad' },
                    {
                        data: 'numero', title: 'Seleccionar',
                        createdCell: function (td, data, rowData, row, col) {
                            let checkbox = document.createElement('input');
                            checkbox.id = 'checkboxAut_' + row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');

                            if (getRadioValue("radAuth")) {
                                checkbox.classList.add('regular-checkboxdanger');
                            } else {
                                checkbox.classList.add('regular-checkbox');
                            }

                            checkbox.style.height = '25px';

                            let label = document.createElement('label');
                            label.setAttribute('for', checkbox.id);

                            $(td).html(checkbox);
                            $(td).append(label);
                            $(td).addClass('centradoVertical');
                        }
                    },
                    {
                        sortable: false, title: 'Detalle',
                        render: function (data, type, row, meta) {
                            let divBotones = document.createElement('div');
                            divBotones.style.display = 'inline-flex';

                            let buttonDetalle = document.createElement('button');
                            let icono = document.createElement('i');

                            $(buttonDetalle).addClass('btn btn-xs btn-default btn-detalle');
                            $(icono).addClass('far fa-eye');
                            $(buttonDetalle).append(icono);
                            $(buttonDetalle).append(' Detalle');

                            let buttonImprimir = document.createElement('button');
                            let iconoImprimir = document.createElement('i');

                            $(buttonImprimir).addClass('btn btn-xs btn-default btn-imprimir');
                            $(iconoImprimir).addClass('far fa-file');
                            $(buttonImprimir).append(iconoImprimir);
                            $(buttonImprimir).append(' Imprimir');
                            $(divBotones).append(buttonDetalle);
                            $(divBotones).append(buttonImprimir);

                            return divBotones.outerHTML;
                        }
                    },
                ],
                columnDefs: [
                    // { "className": "dt-center centradoVertical", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
                    { 'width': '100px', 'targets': [2, 3, 4, 5,8] },
                    { 'width': '50px', 'targets': [1,6,7] }
                ],
                initComplete: function (settings, json) {
                    tblAuth.on('click', 'input[type="checkbox"]', function () {
                        if ($(this).prop('checked')) {
                            tblAuth.DataTable().row($(this).closest('tr')).data().flagCheckBox = true;
                            tblAuth.DataTable().row($(this).closest('tr')).data().isAuth = !getRadioValue("radAuth");
                            _counReq++;
                        } else {
                            tblAuth.DataTable().row($(this).closest('tr')).data().flagCheckBox = false;
                            tblAuth.DataTable().row($(this).closest('tr')).data().isAuth = !getRadioValue("radAuth");
                            _counReq--;
                        }

                        _counReq > 0 ? btnAutorizar.removeClass("disabled") : btnAutorizar.addClass("disabled");
                    });

                    tblAuth.on('click', '.btn-detalle', function () {
                        let req = dtAuth.row($(this).parents('tr')).data();

                        localStorage.setItem("cc", req.cc);
                        localStorage.setItem("num", req.numero);
                        localStorage.setItem("esServicio", req.PERU_tipoRequisicion);

                        setFromStorage();
                    });

                    tblAuth.on('click', '.btn-imprimir', function () {
                        let rowData = dtAuth.row($(this).closest('tr')).data();

                        verReporteRequisicion(rowData.cc, rowData.numero, rowData.PERU_tipoRequisicion);
                    });
                },
                rowCallback: function (row, data) {
                    if (data.contieneCancelado) {
                        $(row).addClass('requiContieneCancelado');
                    }
                },
            });
        }

        function setFromStorage() {
            let cc = localStorage.getItem("cc");
            let num = localStorage.getItem("num");
            let esServicio = localStorage.getItem("esServicio");

            localStorage.removeItem("cc");
            localStorage.removeItem("num");
            localStorage.removeItem("esServicio");

            if (cc != null) {
                selCC.val(cc);
                $('#selCCDetReq').val(cc);
                $('#inputTipoRequisicionPERU').val(esServicio);
                txtNum.val(num).change();
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }

        function verReporteRequisicion(cc, numero, PERU_tipoRequisicion) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=112' + '&cc=' + cc + '&numero=' + numero + '&PERU_tipoRequisicion=' + PERU_tipoRequisicion);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        String.prototype.parseDate = function () { return new Date(parseInt(this.replace('/Date(', ''))); }
        Date.prototype.parseDate = function () { return this; }
    }

    $(document).ready(() => Enkontrol.Compras.Requisicion.Autorizar = new Autorizar())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 2000 }))
        .ajaxStop($.unblockUI);
})();