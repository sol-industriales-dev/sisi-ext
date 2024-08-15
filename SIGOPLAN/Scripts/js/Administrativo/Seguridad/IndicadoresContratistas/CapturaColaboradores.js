(() => {
    $.namespace('CapturaColaboradores.Seguridad');
    Seguridad = function () {

        //#region VARIABLES
        let esEdit = false
        tblcolaboradores = $("#tblcolaboradores");
        const tablaHHT = $('#tablaHHT');
        const rowTablaHHT = $('#rowTablaHHT');
        const txtHorasHombre = $('#txtHorasHombre');
        const labelTotalHoras = $('#labelTotalHoras');
        const selectCCRegistro = $('#selectCCRegistro');
        const btnGetDatos = $('#btnGetDatos');
        //#endregion

        let arrFechas = [];

        //#region PETICIONES
        const getInformacion = (cc, fechaInicio, fechaFin) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInformacionColaboradores', { cc, fechaInicio, fechaFin }) };
        const guardarRegistro = (registroInformacion, lstDetalle, listaClasificacion) => {
            return $.post('/Administrativo/IndicadoresSeguridad/GuardarRegistroInformacion', { registroInformacion, lstDetalle, listaClasificacion });
        };
        const actualizarRegistro = (registroInformacion, lstDetalle, listaClasificacion) => {
            return $.post('/Administrativo/IndicadoresSeguridad/UpdateRegistroInformacion', { registroInformacion, lstDetalle, listaClasificacion });
        };

        //raguilar
        const getInfoEmpleado = (claveEmpleado) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleado', { claveEmpleado }) };
        //#endregion

        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            $('#txtFechaInicio').datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaInicioAnio);
            $('#txtFechaFin').datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaActual);

            setDatosInicio();
            initTable();
            initTablaHHT();
            $('#btnGuardarRegistro').click(guardar);
            $('#btnBuscar').click(buscar);
            $('#btnRegistrar').click(setModalAgregar);

            //raguilar
            $("#btnAddPersonal").click(agregarPersonal);
            $("#txtLostDay").prop("disabled", true);

            btnGetDatos.click(function (e) {
                GetDatos();
            });
            fillCboCCEnKontrol();

            //#region SE OBTIENE EL VALOR DEL ATRIBUTO data-select2-id (ES DATO UNICO), PARA PODER OBTENER LOS DEMAS ATRIBUTOS.
            selectCCRegistro.change(function (e) {
                let dataSelect2ID = $('option:selected', this).attr('data-select2-id');
                let optionSelected = $(this).find(`option[data-select2-id="${dataSelect2ID}"]`);
                fncGetDataSelect2(optionSelected);
            });
            //#endregion
        })();

        $(document).on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        //#region CARGAS INICIALES
        function setDatosInicio() {
            esEdit = false
        }

        function initTable() {
            tblcolaboradores = $("#tblcolaboradores").DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                "aaSorting": [4, 'desc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                    tblcolaboradores.on('click', '.btn-editar-informacion', function () {
                        var rowData = tblcolaboradores.row($(this).closest('tr')).data();

                        $.ajax({
                            url: 'GetInformacionColaboradoresByID',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                if (response.success) {
                                    const data = response.informacion;

                                    const fechaInicio = new Date(moment(data.fechaInicioStr, "DD-MM-YYYY").format());
                                    const fechaFin = new Date(moment(data.fechaFinStr, "DD-MM-YYYY").format());
                                    esEdit = true;

                                    $("#btnGuardarRegistro").val(rowData["id"]);
                                    $("#txtHorasHombre").val(data.horasHombre);
                                    $("#txtLostDay").val(data.lostDay);
                                    $("#selectCCRegistro").val(data.cc);
                                    $("#selectCCRegistro").attr('disabled', true)

                                    $('#txtFechaInicio').datepicker("setDate", fechaInicio);
                                    $('#txtFechaFin').datepicker("setDate", fechaFin);
                                    $('#modalRegistro').modal('show');
                                    rowTablaHHT.css('display', 'none');
                                    agregarListaVacia();

                                    if (!response.EMPTYDETALLE) {
                                        //llenar tabla con los valores
                                        for (var i = 0; i < response.informacionDetalle.length; i++) {
                                            $("#tblPlantilla").append(plantillaPuesto);
                                            $('#tablaPadre').find("tr td .claveEmpleado").eq(i).attr('idDetalle', response.informacionDetalle[i].id);
                                            $('#tablaPadre').find("tr td .claveEmpleado").eq(i).val(response.informacionDetalle[i].cveEmpleado);
                                            $('#tablaPadre').find("tr td .txtNombreEmp").eq(i).val(response.informacionDetalle[i].nombre);
                                            $('#tablaPadre').find("tr td .txtLost").eq(i).val(response.informacionDetalle[i].lostDayEmpleado);

                                            $('#tablaPadre').find('tr td .selectClasificacion').eq(i).fillCombo('GetClasificacionHHTCombo', null, false, null);
                                            $('#tablaPadre').find('tr td .selectClasificacion').eq(i).val(response.informacionDetalle[i].clasificacion);
                                        }
                                    }

                                    rowTablaHHT.css('display', 'block');

                                    if (response.listaClasificacion != null) {
                                        AddRows(tablaHHT, response.listaClasificacion);
                                    } else {
                                        agregarListaVacia();

                                        for (i = 0; i < response.listaFechas.length; i++) {
                                            let row = tablaHHT.find(`tbody tr:eq(${i})`);
                                            let rowData = tablaHHT.DataTable().row(row).data();

                                            rowData.fecha = response.listaFechas[i];
                                            rowData.fechaString = $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(response.listaFechas[i].substr(6))));

                                            tablaHHT.DataTable().row(row).data(rowData).draw();
                                        }
                                    }

                                    calcularTotalHorasClasificacion();
                                } else {
                                    AlertaGeneral('Aviso', 'Ocurrió un error al cargar la información.');
                                }
                            }
                        });
                    });

                    tblcolaboradores.on('click', '.btn-eliminar-informacion', e => {

                        var rowData = tblcolaboradores.row($(e.currentTarget).closest('tr')).data();
                        if (rowData == null || rowData.id <= 0) {
                            return;
                        }
                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            `¿Está seguro de eliminar este registro?`,
                            () => eliminarHHT(rowData.id))
                    });

                },
                columns: [
                    { data: 'cc', title: 'Proyecto' },
                    { data: 'horasHombre', title: 'Horas Hombre' },
                    { data: 'lostDay', title: 'Lost Day' },
                    { data: 'fechaInicioStr', title: 'Fecha Inicio' },
                    { data: 'fechaFinStr', title: 'Fecha Fin' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-informacion btn btn-sm btn-warning" type="button" value="' + row.id + '" style=""><i class="fas fa-pencil-alt"></i></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        data: 'puedeSerEliminado',
                        title: '',
                        sortable: false,
                        render: puedeSerEliminado => puedeSerEliminado ? `<button class="btn-eliminar-informacion btn btn-sm btn-danger"><i class="fas fa-trash"></i></button>` : ''
                    },
                ]
            });
        }

        function eliminarHHT(id) {

            if (id == null || id <= 0) {
                return;
            }

            $.post('/Administrativo/IndicadoresSeguridad/EliminarHHT', { id })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Registro eliminado correctamente.`);
                        $('#btnBuscar').click();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setModalAgregar() {
            esEdit = false;
            $("#txtHorasHombre").val("");
            $("#txtLostDay").val("0");
            $("#txtFechaInicio").val("");
            $("#txtFechaFin").val("");
            $("#selectCCRegistro").attr('disabled', false);
            $('#modalRegistro').modal('show');
            rowTablaHHT.css('display', 'none');
            agregarListaVacia();

            $("#selectCCRegistro").focus();
        }

        //raguilar agregar personal append 20/01/19
        function agregarPersonal() {
            $("#tblPlantilla").append(plantillaPuesto);
            $('#tblPlantilla').find('tr:last td .selectClasificacion').fillCombo('/Administrativo/IndicadoresSeguridad/GetClasificacionHHTCombo', null, false, null);
        }

        $('#chkLost').change(function () {
            if (this.checked) {
                $("#txtLostDay").prop("disabled", false);
            } else {
                $("#txtLostDay").prop("disabled", true);
            }
        });
        //agregarPersonal
        //agregar a plantilla
        var plantillaPuesto = `
        <tr class='rowPuesto'>
            <td><input class='form-control claveEmpleado' type='number' min='1' max='5'></td>
            <td><input class='form-control txtNombreEmp' readonly type='text'></td>
            <td><input class='form-control txtLost' type='number' min='1' max='5'></td>
            <td><select class='form-control selectClasificacion'></select></td>
            <td><button class='btndrop' type='button'> <span class='glyphicon glyphicon-trash'></span></button></td>
        </tr>`;

        $('#tablaPadre').on('click', 'tr td .btndrop', function (evt) {
            $(this).closest("tr").remove();
        });

        $('#tablaPadre').on('blur', 'tr td .txtLost', function (evt) {
            sumaLost = 0;
            $('#tablaPadre tr').each(function (index, tr) {
                valorLostEmp = parseInt($('#tablaPadre').find("tr td .txtLost").eq(index).val());
                if (!isNaN(valorLostEmp)) {
                    sumaLost = valorLostEmp + sumaLost;
                    $("#txtLostDay").val(sumaLost);
                }
            });
        });

        $('#tablaPadre').on('blur', 'tr td .claveEmpleado', function (evt) {
            if ($(this).val() == null || $(this).val() == "") {
                return;
            }
            var $tr = $(this).closest('tr');
            var myRow = $tr.index();
            flagRepetido = false;
            valorIngresado = $(this).val();
            contador = 0;
            $('#tablaPadre tr').each(function (index, tr) {
                var cveEmp = $('#tablaPadre').find("tr td .claveEmpleado").eq(index).val();
                if (cveEmp == valorIngresado && contador <= 1) {
                    if (contador == 1) {
                        flagRepetido = true;
                        clearInfoEmpleado(myRow)
                    }
                    contador = contador + 1;
                }
            });
            if (flagRepetido == false) {
                getInfoEmpleado($(this).val()).done(function (response) {
                    if (response.success) {
                        $('#tablaPadre').find("tr td .txtNombreEmp").eq(myRow).val(response.empleadoInfo.nombreEmpleado);

                    } else {
                        clearInfoEmpleado(myRow)
                    }
                })
            }
        });

        function clearInfoEmpleado(myRow) {
            $('#tablaPadre').find("tr td .txtNombreEmp").eq(myRow).val('');
            $('#tablaPadre').find("tr td .txtLost").eq(myRow).val('');
            $('#tablaPadre').find("tr td .claveEmpleado").eq(myRow).val('');
        }
        //#endregion

        //#region METODOS GENERALES
        function buscar() {
            getInformacion($("#selectCCFiltros").val(), $('#txtFechaInicio').val(), $('#txtFechaFin').val()).done(function (response) {
                if (response.success) {
                    tblcolaboradores.clear();
                    tblcolaboradores.rows.add(response.items);
                    tblcolaboradores.draw();
                } else {
                    tblcolaboradores.clear();
                    tblcolaboradores.draw();
                }
            })
        }

        function getUltimoRegistro() {
            rowTablaHHT.css('display', 'none');
            agregarListaVacia();

            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/GetFechasUltimoCorte',
                datatype: "json",
                type: "GET",
                data: {
                    cc: $("#selectCCRegistro").val()
                },
                success: function (response) {
                    if (response.success) {
                        rowTablaHHT.css('display', 'block');
                        arrFechas = [];

                        const ultimoRegistro = response.ultimoRegistro;

                        if (ultimoRegistro == null) {
                            const fecha = moment().startOf('month').startOf('isoweek').format();
                            const fechaFin = moment().startOf('month').endOf('isoweek').format();

                            $('#txtFechaInicio').datepicker("setDate", new Date(fecha));
                            $('#txtFechaFin').datepicker("setDate", new Date(fechaFin));

                            for (i = 0; i < response.listaFechas.length; i++) {
                                let row = tablaHHT.find(`tbody tr:eq(${i})`);
                                let rowData = tablaHHT.DataTable().row(row).data();

                                rowData.fecha = response.listaFechas[i];
                                rowData.fechaString = $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(response.listaFechas[i].substr(6))));
                                tablaHHT.DataTable().row(row).data(rowData).draw();
                            }
                            for (let i = 0; i < response.listaFechas.length; i++) {
                                arrFechas.push($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(response.listaFechas[i].substr(6)))));
                            }
                        } else {
                            const fecha = moment(ultimoRegistro.fechaFinStr, "DD-MM-YYYY").add(1, 'days').format();
                            const fechaFin = moment(ultimoRegistro.fechaFinStr, "DD-MM-YYYY").add(7, 'days').format()

                            $('#txtFechaInicio').datepicker("setDate", new Date(fecha));
                            $('#txtFechaFin').datepicker("setDate", new Date(fechaFin));

                            for (let i = 0; i < response.listaFechas.length; i++) {
                                let row = tablaHHT.find(`tbody tr:eq(${i})`);
                                let rowData = tablaHHT.DataTable().row(row).data();

                                rowData.fecha = response.listaFechas[i];
                                rowData.fechaString = $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(response.listaFechas[i].substr(6))));

                                tablaHHT.DataTable().row(row).data(rowData).draw();
                            }
                            for (let i = 0; i < response.listaFechas.length; i++) {
                                arrFechas.push($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(response.listaFechas[i].substr(6)))));
                            }
                        }

                        calcularTotalHorasClasificacion();
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al cargar el último corte.`);
                        $('#modalRegistro').modal('hide');
                    }
                }
            });
        }

        function validarGuardar() {
            let countInvalidos = 0
            fechaInicio = $('#txtFechaInicio').val();
            fechaFin = $('#txtFechaFin').val();
            if ($("#selectCCRegistro").val() == "") {
                countInvalidos++;
            }
            if (fechaInicio == "" || fechaFin == "") {
                countInvalidos++;
            }
            return countInvalidos;
        }

        function getValPlantilla() {
            //regla de negocio validacion personal
            var f1 = new Date(moment($('#txtFechaInicio').val(), "DD-MM-YYYY").format());
            var f2 = new Date(moment($('#txtFechaFin').val(), "DD-MM-YYYY").format());
            var fecha1 = moment(f1);
            var fecha2 = moment(f2);
            var longitud = $("#tablaPadre >tbody >tr").length
            var diferencia = fecha2.diff(fecha1, 'hours') / 24;
            if (diferencia >= 7 && longitud == 0) {
                return false;
            }
        }

        function getObjRegistro() {
            const registroInformacion = {
                horasHombre: +(labelTotalHoras.data('totalHoras')),
                lostDay: $("#txtLostDay").val(),
                cc: $("#selectCCRegistro").val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val()
            }
            return registroInformacion
        }

        $('#modalRegistro').on('hidden.bs.modal', function () {
            $("#tablaPadre >tbody >tr").remove()
        })

        function getObjRegistroDetalle() {
            let lstDetalle = []
            let flagCorrecto = true;

            $('#tablaPadre tbody tr').each(function (index, tr) {
                let idDetalle = $('#tablaPadre').find("tr td .claveEmpleado").eq(index).attr('idDetalle');
                let cveEmpleado = $('#tablaPadre').find("tr td .claveEmpleado").eq(index).val();
                let nombre = $('#tablaPadre').find("tr td .txtNombreEmp").eq(index).val();
                let lostDayEmpleado = $('#tablaPadre').find("tr td .txtLost").eq(index).val();
                let clasificacion = +($('#tablaPadre').find("tr td .selectClasificacion").eq(index).val());

                if (cveEmpleado == "" || nombre == "" || lostDayEmpleado == "") {
                    AlertaGeneral('Aviso', 'Debe ingresar todos los datos');
                    flagCorrecto = false;
                } else {
                    lstDetalle.push({
                        id: idDetalle,
                        cveEmpleado: cveEmpleado,
                        nombre: nombre,
                        lostDayEmpleado: lostDayEmpleado,
                        clasificacion: clasificacion
                    })
                }
            });

            if (flagCorrecto != false && lstDetalle.length > 0) {//si no hay errores y no hay detalle
                return lstDetalle;
            } else if (lstDetalle.length == 0 && flagCorrecto != false) {//si no hay errores pero tampoco detalle
                return true;
            } else {
                return false;//si hay errores
            }
        }

        function getListaClasificacion(esEdit) {
            let listaClasificacion = [];

            tablaHHT.find('tbody tr').each(function (idx, row) {
                let rowData = tablaHHT.DataTable().row(row).data();
                let horasMantenimiento = +($(row).find('.inputMantenimiento').val());
                let horasOperativo = +($(row).find('.inputOperativo').val());
                let horasAdministrativo = +($(row).find('.inputAdministrativo').val());

                if (esEdit) {
                    listaClasificacion.push({
                        id: rowData.id,
                        cc: selectCCRegistro.val(),
                        fecha: rowData.fechaString,
                        horasMantenimiento: horasMantenimiento,
                        horasOperativo: horasOperativo,
                        horasAdministrativo: horasAdministrativo,
                        informacionColaboradoresID: rowData.informacionColaboradoresID,
                        estatus: true
                    });
                } else {
                    listaClasificacion.push({
                        id: 0,
                        cc: selectCCRegistro.val(),
                        fecha: rowData.fechaString,
                        horasMantenimiento: horasMantenimiento,
                        horasOperativo: horasOperativo,
                        horasAdministrativo: horasAdministrativo,
                        informacionColaboradoresID: 0,
                        estatus: true
                    });
                }
            });

            return listaClasificacion;
        }

        function getObjRegistroEdit() {
            const registroInformacion = {
                id: $("#btnGuardarRegistro").val(),
                horasHombre: +(labelTotalHoras.data('totalHoras')),
                lostDay: $("#txtLostDay").val(),
            }

            return registroInformacion
        }

        function guardar() {
            if (validarGuardar() > 0) {
                AlertaGeneral('Aviso', 'Debe ingresar todos los datos');
            } else if (getValPlantilla() == false) {
                AlertaGeneral('Aviso', 'Debe ingresar plantilla de personal, debido al rango de fechas');
            } else {
                let lstDetalle = getObjRegistroDetalle();
                let listaClasificacion = getListaClasificacion(esEdit);

                if (esEdit && lstDetalle != false) {
                    actualizarRegistro(getObjRegistroEdit(), lstDetalle, listaClasificacion).done(function (response) {
                        if (response.success) {
                            AlertaGeneral('Aviso', 'Se guardo con exito');
                            $('#modalRegistro').modal('hide');
                            setDatosInicio();
                            $('#selectCCFiltros').val(listaClasificacion[0].cc);
                            $('#selectCCFiltros').trigger("change.select2");
                            buscar();
                            esEdit = false;
                        } else {
                            AlertaGeneral('Aviso', response.error);
                        }
                    });
                } else if (!esEdit) {
                    guardarRegistro(getObjRegistro(), lstDetalle, listaClasificacion).done(function (response) {
                        if (response.success) {
                            AlertaGeneral('Aviso', 'Se guardo con exito')
                            $('#modalRegistro').modal('hide');
                            setDatosInicio();
                            $('#selectCCFiltros').val(listaClasificacion[0].cc);
                            $('#selectCCFiltros').trigger("change.select2");
                            buscar();
                        } else {
                            AlertaGeneral('Aviso', response.error);
                        }
                    });
                }
            }
        }
        //#endregion

        //#region EVENT'S CHANGE, CLICKS
        $('.select2').select2();

        $('#selectCCRegistro').change(getUltimoRegistro)
        //#endregion
        function initTablaHHT() {
            tablaHHT.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                bLengthChange: false,
                searching: false,
                paging: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tablaHHT.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tablaHHT.on('change', '.horasClasificacion', function () {
                        calcularTotalHorasClasificacion();
                    });
                },
                columns: [
                    { data: 'fechaString', title: 'Fecha' },
                    {
                        data: 'horasMantenimiento', title: 'Mantenimiento',
                        render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center horasClasificacion inputMantenimiento" value="${data}">`;
                        }
                    },
                    {
                        data: 'horasOperativo', title: 'Operativo',
                        render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center horasClasificacion inputOperativo" value="${data}">`;
                        }
                    },
                    {
                        data: 'horasAdministrativo', title: 'Administrativo',
                        render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center horasClasificacion inputAdministrativo" value="${data}">`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            agregarListaVacia();
        }

        function agregarListaVacia() {
            //Se agregaron siempre 7 registros para cada día de la semana
            let listaDiasVacia = [];

            listaDiasVacia.push(
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0 },
            );
            AddRows(tablaHHT, listaDiasVacia);
        }

        function calcularTotalHorasClasificacion() {
            let totalHoras = 0;

            tablaHHT.find('tbody tr').each(function (idx, row) {
                let horasMantenimiento = +($(row).find('.inputMantenimiento').val());
                let horasOperativo = +($(row).find('.inputOperativo').val());
                let horasAdministrativo = +($(row).find('.inputAdministrativo').val());

                totalHoras += horasMantenimiento + horasOperativo + horasAdministrativo;
            });

            labelTotalHoras.text('Total Horas: ' + totalHoras);
            labelTotalHoras.data('totalHoras', totalHoras);
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }

        let objSelected;
        function fncGetDataSelect2(optionSelected) {
            let cc = optionSelected.val();
            let empresa = optionSelected.attr("empresa");
            objSelected = new Object();
            objSelected = {
                cc: cc,
                idEmpresa: empresa
            };
        }

        //#region CALCULO DE HORAS TRABAJADAS - HORAS HOMBRE
        function GetDatos() {
            let fechas = "";
            for (let i = 0; i < 7; i++) {
                if (i == 0)
                    fechas += arrFechas[i];
                else
                    fechas += "|" + arrFechas[i];
            }

            let cc = objSelected.cc;
            let idEmpresa = objSelected.idEmpresa;
            let objDatos = new Object();
            objDatos = {
                cc: cc,
                idEmpresa: idEmpresa,
                fechas: fechas
            };

            $.ajax({
                url: 'GetDatos',
                datatype: "json",
                type: "GET",
                data: objDatos,
                success: function (response) {
                    if (response.success) {

                        var table = tablaHHT.DataTable();
                        // #region CELDAS DE LA COLUMNA MANTENIMIENTO
                        table.cell(0, 1).data(response.lstHorasLaboradasSemanalMantenimiento[0] + response.lstHorasLaboradasQuincenalMantenimiento[0]).draw();
                        table.cell(1, 1).data(response.lstHorasLaboradasSemanalMantenimiento[1] + response.lstHorasLaboradasQuincenalMantenimiento[1]).draw();
                        table.cell(2, 1).data(response.lstHorasLaboradasSemanalMantenimiento[2] + response.lstHorasLaboradasQuincenalMantenimiento[2]).draw();
                        table.cell(3, 1).data(response.lstHorasLaboradasSemanalMantenimiento[3] + response.lstHorasLaboradasQuincenalMantenimiento[3]).draw();
                        table.cell(4, 1).data(response.lstHorasLaboradasSemanalMantenimiento[4] + response.lstHorasLaboradasQuincenalMantenimiento[4]).draw();
                        table.cell(5, 1).data(response.lstHorasLaboradasSemanalMantenimiento[5] + response.lstHorasLaboradasQuincenalMantenimiento[5]).draw();
                        table.cell(6, 1).data(response.lstHorasLaboradasSemanalMantenimiento[6] + response.lstHorasLaboradasQuincenalMantenimiento[6]).draw();
                        // #endregion

                        // #region CELDAS DE LA COLUMNA OPERATIVO
                        table.cell(0, 2).data(response.lstHorasLaboradasSemanalOperativo[0] + response.lstHorasLaboradasQuincenalOperativo[0]).draw();
                        table.cell(1, 2).data(response.lstHorasLaboradasSemanalOperativo[1] + response.lstHorasLaboradasQuincenalOperativo[1]).draw();
                        table.cell(2, 2).data(response.lstHorasLaboradasSemanalOperativo[2] + response.lstHorasLaboradasQuincenalOperativo[2]).draw();
                        table.cell(3, 2).data(response.lstHorasLaboradasSemanalOperativo[3] + response.lstHorasLaboradasQuincenalOperativo[3]).draw();
                        table.cell(4, 2).data(response.lstHorasLaboradasSemanalOperativo[4] + response.lstHorasLaboradasQuincenalOperativo[4]).draw();
                        table.cell(5, 2).data(response.lstHorasLaboradasSemanalOperativo[5] + response.lstHorasLaboradasQuincenalOperativo[5]).draw();
                        table.cell(6, 2).data(response.lstHorasLaboradasSemanalOperativo[6] + response.lstHorasLaboradasQuincenalOperativo[6]).draw();
                        // #endregion

                        // #region CELDAS DE LA COLUMNA ADMINISTRATIVO
                        table.cell(0, 3).data(response.lstHorasLaboradasSemanalAdministrativo[0] + response.lstHorasLaboradasQuincenalAdministrativo[0]).draw();
                        table.cell(1, 3).data(response.lstHorasLaboradasSemanalAdministrativo[1] + response.lstHorasLaboradasQuincenalAdministrativo[1]).draw();
                        table.cell(2, 3).data(response.lstHorasLaboradasSemanalAdministrativo[2] + response.lstHorasLaboradasQuincenalAdministrativo[2]).draw();
                        table.cell(3, 3).data(response.lstHorasLaboradasSemanalAdministrativo[3] + response.lstHorasLaboradasQuincenalAdministrativo[3]).draw();
                        table.cell(4, 3).data(response.lstHorasLaboradasSemanalAdministrativo[4] + response.lstHorasLaboradasQuincenalAdministrativo[4]).draw();
                        table.cell(5, 3).data(response.lstHorasLaboradasSemanalAdministrativo[5] + response.lstHorasLaboradasQuincenalAdministrativo[5]).draw();
                        table.cell(6, 3).data(response.lstHorasLaboradasSemanalAdministrativo[6] + response.lstHorasLaboradasQuincenalAdministrativo[6]).draw();
                        // #endregion

                        // #region SE OBTIENE EL TOTAL DE HORAS
                        let totalHoras = response.lstHorasLaboradasSemanalMantenimiento[0] +
                            response.lstHorasLaboradasSemanalMantenimiento[1] +
                            response.lstHorasLaboradasSemanalMantenimiento[2] +
                            response.lstHorasLaboradasSemanalMantenimiento[3] +
                            response.lstHorasLaboradasSemanalMantenimiento[4] +
                            response.lstHorasLaboradasSemanalMantenimiento[5] +
                            response.lstHorasLaboradasSemanalMantenimiento[6] +
                            response.lstHorasLaboradasSemanalOperativo[0] +
                            response.lstHorasLaboradasSemanalOperativo[1] +
                            response.lstHorasLaboradasSemanalOperativo[2] +
                            response.lstHorasLaboradasSemanalOperativo[3] +
                            response.lstHorasLaboradasSemanalOperativo[4] +
                            response.lstHorasLaboradasSemanalOperativo[5] +
                            response.lstHorasLaboradasSemanalOperativo[6] +
                            response.lstHorasLaboradasSemanalAdministrativo[0] +
                            response.lstHorasLaboradasSemanalAdministrativo[1] +
                            response.lstHorasLaboradasSemanalAdministrativo[2] +
                            response.lstHorasLaboradasSemanalAdministrativo[3] +
                            response.lstHorasLaboradasSemanalAdministrativo[4] +
                            response.lstHorasLaboradasSemanalAdministrativo[5] +
                            response.lstHorasLaboradasSemanalAdministrativo[6] +
                            response.lstHorasLaboradasQuincenalMantenimiento[0] +
                            response.lstHorasLaboradasQuincenalMantenimiento[1] +
                            response.lstHorasLaboradasQuincenalMantenimiento[2] +
                            response.lstHorasLaboradasQuincenalMantenimiento[3] +
                            response.lstHorasLaboradasQuincenalMantenimiento[4] +
                            response.lstHorasLaboradasQuincenalMantenimiento[5] +
                            response.lstHorasLaboradasQuincenalMantenimiento[6] +
                            response.lstHorasLaboradasQuincenalOperativo[0] +
                            response.lstHorasLaboradasQuincenalOperativo[1] +
                            response.lstHorasLaboradasQuincenalOperativo[2] +
                            response.lstHorasLaboradasQuincenalOperativo[3] +
                            response.lstHorasLaboradasQuincenalOperativo[4] +
                            response.lstHorasLaboradasQuincenalOperativo[5] +
                            response.lstHorasLaboradasQuincenalOperativo[6] +
                            response.lstHorasLaboradasQuincenalAdministrativo[0] +
                            response.lstHorasLaboradasQuincenalAdministrativo[1] +
                            response.lstHorasLaboradasQuincenalAdministrativo[2] +
                            response.lstHorasLaboradasQuincenalAdministrativo[3] +
                            response.lstHorasLaboradasQuincenalAdministrativo[4] +
                            response.lstHorasLaboradasQuincenalAdministrativo[5] +
                            response.lstHorasLaboradasQuincenalAdministrativo[6];
                        labelTotalHoras.text("Total horas: " + new Intl.NumberFormat().format(totalHoras));
                        // #endregion

                        $.unblockUI();
                        Alert2Exito("Se ha calculado las horas con éxito");
                    } else {
                        Alert2Warning(response.message);
                        $.unblockUI();
                    }
                }
            });
        }

        function fillCboCCEnKontrol() {
            axios.get('/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresas')
                .then(response => {
                    let { success, items, message } = response.data;

                    if (success) {
                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 3 : 0}">${y.Text}</option>`;
                            });
                            selectCCRegistro.append('<option value="">--Seleccione--</option>');
                            selectCCRegistro.append(groupOption);

                            $("#selectCCFiltros").append('<option value="">--Seleccione--</option>');
                            $("#selectCCFiltros").append(groupOption);
                        });
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        //#endregion
    }

    $(document).ready(() => CapturaColaboradores.Seguridad = new Seguridad())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop($.unblockUI);
})();