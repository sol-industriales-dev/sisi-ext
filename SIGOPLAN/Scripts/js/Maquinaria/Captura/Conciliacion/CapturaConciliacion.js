(function () {
    $.namespace('SIGOPLAN.Controllers.Maquinaria.Capturas');
    Capturas = function () {
        urlCboCC = '/Conciliacion/getCboCC';
        mensajes = {
            NOMBRE: 'Captura de Horas Hombre.',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        btnCargar = $("#btnCargar"),
            btnGuardar = $("#btnGuardar"),
            dtBusqInicial = $("#dtBusqInicial");
        dtBusqFinal = $("#dtBusqFinal");
        selBusqCC = $("#selBusqCC");
        selSemanas = $("#selSemanas");
        btnBuscar = $("#btnBuscar");
        radioBtn = $('.radioBtn a');
        tblConciliacionHorometros = $("#tblConciliacionHorometros");
        function init() {
            initForm();
            selBusqCC.change(fnFechas);
            //selSemanas.change(fnLoadCCData);
            radioBtn.click(aClick);
            btnCargar.click(fnLoadCCData);
            btnGuardar.click(fnsaveOrUpdateConciliacion);
        }
        function initForm() {
            selBusqCC.fillCombo(urlCboCC, null, false, null);
            selSemanas.fillCombo('/Conciliacion/FillCboQuincenasVariables', { ccID: selBusqCC.val() }, false, null);
        }
        function fnFechas() {
            selSemanas.fillCombo('/Conciliacion/FillCboQuincenasVariables', {ccID:selBusqCC.val()}, false, null);
        }
        function fnLoadCCData() {
            btnGuardar.prop('disabled', false);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/loadTablaConciliacionHorometros',
                type: "POST",
                datatype: "json",
                data: {
                    enc: {
                        ccID: selBusqCC.val(),
                        moneda: getRadioValue("radMoneda")
                    },
                    fechaInicio: GetFecha(0),
                    fechaFinal: GetFecha(1),
                    fechaID: selSemanas.val()
                },
                success: function (response) {
                    btnGuardar.prop('disabled', false);
                    fnSetInfoTbl(response.items);

                    if (response.getValida == 1) {
                        btnGuardar.prop('disabled', true);
                        AlertaGeneral('Alerta', 'Ya fue Generada esa conciliacion de horometros.');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function fnsaveOrUpdateConciliacion() {
            //if (getDataTbl().length > 0) {
                btnGuardar.prop('disabled', true);
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/conciliacion/saveOrUpdateConciliacion',
                    type: "POST",
                    datatype: "json",
                    data: { centroCostosID: selBusqCC.val(), fechaID: selSemanas.val(), fechaInicio: GetFecha(0), fechaFin: GetFecha(1), obj: getDataTbl() },
                    success: function (response) {
                        AlertaGeneral('Alerta', 'Se guardo Correctamente.');
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            //}
            //else {
            //    AlertaGeneral('Alerta', 'Verifique la información antes de guardar.');
            //}
        }
        function getDataTbl() {
            var Array = [];
            $.each(tblConciliacionHorometrosGrid.data(), function (index, value) {
                var count = index == 0 ? 1 : count;
                var JsonData = {};
                JsonData.id = value.id;
                JsonData.numero = index;
                JsonData.noEconomicoID = value.idMaquinaria;
                JsonData.economico = value.economico;
                JsonData.descripcion = value.descripcion;
                JsonData.modelo = value.modelo;
                JsonData.horometroInicial = value.HI;
                JsonData.horometroFinal = value.HF;
                JsonData.horometroEfectivo = value.HE;
                JsonData.unidad = value.unidad;
                JsonData.costo = value.costo;
                JsonData.total = setCostoCero(value.comentario,value.costoTotal);
                JsonData.idEmpresa = value.cargo;
                JsonData.idEncCaratula = value.idEncCaratula;
                JsonData.idCapCaratula = value.idCapCaratula;
                JsonData.observaciones = value.comentario;
                JsonData.moneda = value.moneda;
                JsonData.overhaul = value.overhaul;

                Array.push(JsonData);
                count++;
            });
            return Array;
        }
        function setCostoCero(comentario, costoTotal) {

            switch (comentario) {
                case 'Depreciación':
                case 'standBy':
                    return 0;
                default:
                    return costoTotal;
            }
        }

        function GetFecha(index) {
            var FechaRaw = $("#selSemanas :selected").text();
            var arrayFechas = FechaRaw.split("-");
            return arrayFechas[index];
        }
        Number.prototype.format = function (n, x) {
            var re = '(\\d)(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
            return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$1,');
        };
        function fnSetInfoTbl(dataSet) {
            tblConciliacionHorometrosGrid = tblConciliacionHorometros.DataTable({
                info: false,
                paging: false,
                searching: false,
                language: dtDicEsp,
                destroy: true,
                scrollY: "300px",
                scrollX: true,
                data: dataSet,
                columns: [
                    { data: 'economico', sortable: false, },
                    { data: 'descripcion', sortable: false, },
                    { data: 'modelo', sortable: false, },
                    { data: 'HI', sortable: false, },
                    { data: 'HF', sortable: false, },
                    { data: 'HE', sortable: false, },
                    {
                        data: 'unidad', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            switch (cellData) {
                                case 1:
                                    $(td).text('HORAS');
                                    break;
                                case 2:
                                    $(td).text('DIA');
                                    break;
                                default:

                            }

                        }
                    },
                    {
                        data: 'costo', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            varTipoMoneda = rowData.moneda == 1 ? "MXN" : "USD";
                            $(td).addClass('text-right');
                            $(td).text('');
                            $(td).text("$   " + Number(cellData).format(2) + " " + varTipoMoneda);
                        }
                    },
                    {
                        data: 'costoTotal', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('costoTotal');
                            $(td).addClass('text-right');
                            $(td).text('');

                            varTipoMoneda = rowData.moneda == 1 ? "MXN" : "USD";

                            $(td).text("$   " + Number(cellData).format(2) + " " + varTipoMoneda);
                        }
                    },
                    {
                        data: 'cargo', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            //  select = setTipoDeCargo(row);

                            $(td).text(cellData == 1 ? "CONSTRUPLAN" : "ARRENDADORA");
                            //                            $(td).append(select);
                        }
                    },
                    {
                        data: 'comentario', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            // $(td).append('<div><input data-index="' + row + '" type="text" style="width: 100px;" class="comentario form-control changeEvent" value=" "></div>');
                            select = setTipoComentario(row);
                            $(td).append(select);
                        }


                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };

                    // Total over all pages
                    Total = api
                        .column(8)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(7).footer()).html(
                        "$ " + Number(Total).format(2)
                    );
                }, drawCallback: function (settings) {

                    tblConciliacionHorometros.find('.changeEvent').change(function () {

                        let data = tblConciliacionHorometrosGrid.row($(this).parents('tr')).data();
                        let comentario = $(this).val();
                        let varTipoMoneda = data.moneda == 1 ? "MXN" : "USD";
                        setValueTabla($(this));
                        $(this).parents('tr').find('.costoTotal').text("$   " + Number(setCostoCero(comentario, data.costoTotal)).format(2) + " " + varTipoMoneda);

                    });
                }

            });
            tblConciliacionHorometrosGrid.draw();
        }

        function setValueTabla(elemento) {
            var idRow = Number($(elemento).attr('data-index'));
            var Value = Number($(elemento).val());
            var index = $(elemento).parent().parent().index();
            if ($(elemento).hasClass('comentario')) {
                var Value = $(elemento).val();
                tblConciliacionHorometrosGrid.data()[idRow].comentario = Value;
            }
        }

        function setTipoComentario(index) {
            var select = "<div class='row'>"
            select += "<div class='col-lg-12'>"
            select += "<select class='form-control comentario changeEvent' data-index='" + index + "'>";
            select += "<option value=''>NA</option>";
            select += "<option value='Disponible para envió'>A: Disponible para envió </option>";
            select += "<option value='Reparación más de 3 días'>B: Reparación más de 3 días  </option>";
            select += "<option value='Falta de tramo'>C: Falta de tramo </option>";
            select += "<option value='Reparación por daño en obra anterior'>D: Reparación por daño en obra anterior</option>";
            select += "<option value='Depreciación'>E:Depreciación</option>";
            select += "<option value='standBy'>F:Stand By</option>";
            select += "</div>";
            return select;
        }
        //#region aRadio
        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function aClick() {
            let sel = $(this).data('title');
            let tog = $(this).data('toggle');
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
            fnLoadCCData();
        }
        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }
        //#endregion
        init();
    }
    $(document).ready(function () {
        SIGOPLAN.Controllers.Maquinaria.Capturas = new Capturas();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000000
            });
        })
        .ajaxStop(function () {
            $.unblockUI();
        });
})();