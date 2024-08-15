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
        const chkEsContratista = $('#chkEsContratista');
        const lblContratista = $('#lblContratista');
        //#endregion

        let arrFechas = [];

        //#region PETICIONES
        const getInformacion = (idAgrupacion, fechaInicio, fechaFin, idEmpresa) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInformacionColaboradores', { idAgrupacion, fechaInicio, fechaFin, idEmpresa }) };
        const guardarRegistro = (registroInformacion, lstDetalle, listaClasificacion) => {

            //#region SE REEMPLAZA "c_" Y "a_" EN CASO DE SELECCIONAR A CONTRATISTAS.
            let idEmpresa = registroInformacion["idEmpresa"];
            let strAgrupacion = registroInformacion["idAgrupacion"];
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }
            registroInformacion["idAgrupacion"] = idAgrupacion;

            listaClasificacion.forEach(element => {
                element.idAgrupacion = idAgrupacion;
            });
            //#endregion

            return $.post('/Administrativo/IndicadoresSeguridad/GuardarRegistroInformacion', { registroInformacion, lstDetalle, listaClasificacion });
        };
        const actualizarRegistro = (registroInformacion, lstDetalle, listaClasificacion) => {
            return $.post('/Administrativo/IndicadoresSeguridad/UpdateRegistroInformacion', { registroInformacion, lstDetalle, listaClasificacion });
        };

        //raguilar
        const getInfoEmpleado = (claveEmpleado, esContratista, idEmpresaContratista) => {
            return $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleado', { claveEmpleado, esContratista, idEmpresaContratista })
        };
        //#endregion

        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        const selectCCFiltros = $('#selectCCFiltros');

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

            // $("#chkEsContratista").bootstrapToggle();

            fncValidarAccesoContratista();

            chkEsContratista.change(function (e) {
                if ($(".claveEmpleado").val() != "") { $(".claveEmpleado").trigger("change"); }
            });

            selectCCRegistro.change(function (e) {
                $(".claveEmpleado").val("");
                $(".txtNombreEmp").val("");
                $(".txtLost").val("");
                $(".selectClasificacion").selectedIndex = 0;
                chkEsContratista.prop("checked", false);
                $("#tablaPadre tbody").empty();

                let attrEmpresaOption = $(`select[id="selectCCRegistro"] option:selected`).attr("empresa");
                if (attrEmpresaOption == 1000 || attrEmpresaOption == 2000) {
                    btnGetDatos.attr("disabled", true);
                } else {
                    btnGetDatos.attr("disabled", false);
                }
            });

            //#region SE OBTIENE EL VALOR DEL ATRIBUTO data-select2-id (ES DATO UNICO), PARA PODER OBTENER LOS DEMAS ATRIBUTOS.
            // selectCCRegistro.change(function (e) {

            // fncGetDataSelect2(cc, idEmpresa);

            // let dataSelect2ID = $('option:selected', this).attr('data-select2-id');
            // let optionSelected = $(this).find(`option[data-select2-id="${dataSelect2ID}"]`);
            // fncGetDataSelect2(optionSelected);
            // });
            //#endregion
        })();

        $(document).on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        //#region CARGAS INICIALES
        function setDatosInicio() {
            selectCCFiltros.fillComboSeguridadSinContratistas(false);
            selectCCRegistro.fillComboSeguridadSinContratistas(false);
            $('.select2').select2();
            esEdit = false
        }

        function fncValidarAccesoContratista() {
            axios.post("ValidarAccesoContratista").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    btnGetDatos.css("display", "none");
                    chkEsContratista.css("display", "none");
                    chkEsContratista.attr("esOculto", "1");
                    lblContratista.css("display", "none");
                    btnGetDatos.attr("data-esContratista", true);
                } else {
                    btnGetDatos.attr("data-esContratista", false);
                    chkEsContratista.attr("esOculto", "0");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTable() {
            tblcolaboradores = $("#tblcolaboradores").DataTable({
                retrieve: true,
                paging: false,
                // ajax: {
                //     url: 'GetInformacionColaboradores',
                //     dataSrc: function (response) {
                //         if (response.EMPTY) {
                //             return [];
                //         } else {
                //             return response.items;
                //         }
                //     }
                // },
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
                                    //const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());
                                    const fechaFin = new Date(moment(data.fechaFinStr, "DD-MM-YYYY").format());
                                    esEdit = true;

                                    $("#btnGuardarRegistro").val(rowData["id"]);
                                    $("#txtHorasHombre").val(data.horasHombre);
                                    $("#txtLostDay").val(data.lostDay);
                                    // $("#selectCCRegistro").fillCombo('LlenarComboCC', null, false);

                                    let idEmpresa = data.idEmpresa;
                                    let strAgrupacion = data.idAgrupacion;
                                    let idAgrupacion;
                                    if (idEmpresa == 1000) {
                                        let contratista = "c_";
                                        idAgrupacion = contratista.concat(strAgrupacion);
                                    } else if (idEmpresa == 2000) {
                                        let agrupacionContratista = "a_";
                                        idAgrupacion = agrupacionContratista.concat(strAgrupacion);
                                    } else {
                                        idAgrupacion = strAgrupacion;
                                    }
                                    $("#selectCCRegistro").val(idAgrupacion).trigger('change.select2');
                                    $("#selectCCRegistro").attr('disabled', true)

                                    $('#txtFechaInicio').datepicker("setDate", fechaInicio);
                                    $('#txtFechaFin').datepicker("setDate", fechaFin);
                                    $('#modalRegistro').modal('show');
                                    rowTablaHHT.css('display', 'none');
                                    agregarListaVacia();

                                    if (response.EMPTYDETALLE) {
                                        //raguilar 24/12/19 
                                        // AlertaGeneral('Aviso', 'No se ha capturado la plantilla de colaboradores.');
                                    } else {
                                        //llenar tabla con los valores
                                        for (var i = 0; i < response.informacionDetalle.length; i++) {
                                            $("#tblPlantilla").append(plantillaPuesto);
                                            $('#tablaPadre').find("tr td .claveEmpleado").eq(i).attr('idDetalle', response.informacionDetalle[i].id);
                                            //$('#tablaPadre').find("tr.rowPuesto").attr('idDetalle')=response.informacionDetalle[i].id;
                                            //$('#tablaPadre').find("tr").attr('idDetalle', response.informacionDetalle[i].id);
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
            // $('#selectCCRegistro').fillCombo('LlenarComboCC', null, false);
            $("#selectCCRegistro").attr('disabled', false);

            $('#modalRegistro').modal('show');
            rowTablaHHT.css('display', 'none');
            agregarListaVacia();

            $("#selectCCRegistro").focus();
        }

        //raguilar agregar personal append 20/01/19
        function agregarPersonal() {
            $("#tblPlantilla").append(plantillaPuesto);
            $('#tblPlantilla').find('tr:last td .selectClasificacion').fillCombo('GetClasificacionHHTCombo', null, false, null);
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
                <td><input class='form-control claveEmpleado' onclick="$(this).select();" type='number' min='1' max='5'></td>
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

        $('#tablaPadre').on('change', 'tr td .claveEmpleado', function (evt) {
            if ($(this).val() == null || $(this).val() == "") {
                return;
            }
            var myCol = $(this).index();
            var $tr = $(this).closest('tr');
            var myRow = $tr.index();
            flagRepetido = false;
            valorIngresado = $(this).val();
            contador = 0;
            var valorLostEmp = 0;
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
                let attrEsContratista = btnGetDatos.attr("data-esContratista");
                idEmpresa = selectCCRegistro.val() != "" ? selectCCRegistro.val() : 0;
                let esContratista = false;
                if (attrEsContratista == "true") {
                    esContratista = true;
                } else {
                    if (chkEsContratista.prop('checked')) {
                        esContratista = true;
                    } else {
                        esContratista = false;
                    }
                }

                if (chkEsContratista.attr("esOculto") == 1 && selectCCRegistro.val() == "") {
                    Alert2Warning("Es necesario seleccionar una empresa");
                } else {
                    getInfoEmpleado($(this).val(), esContratista, idEmpresa).done(function (response) {
                        if (response.success) {
                            $('#tablaPadre').find("tr td .txtNombreEmp").eq(myRow).val(response.empleadoInfo.nombreEmpleado);
                        } else {
                            clearInfoEmpleado(myRow)
                        }
                    });
                }
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
            let idEmpresa = $('select[id="selectCCFiltros"] option:selected').attr("empresa"); //TODO
            let arrObjFiltro = [];
            let objFiltro = new Object();
            let strAgrupacionID = $('select[id="selectCCFiltros"] option:selected').val();
            let idAgrupacion = 0;

            if (idEmpresa) {
                if (idEmpresa == 1000) {
                    idAgrupacion = strAgrupacionID.replace("c_", "");
                } else if (idEmpresa == 2000) {
                    idAgrupacion = strAgrupacionID.replace("a_", "");
                } else {
                    idAgrupacion = strAgrupacionID;
                }
            } else {
                idEmpresa = -1;
                idAgrupacion = -1;
            }

            objFiltro = {
                idEmpresa: idEmpresa,
                idAgrupacion: parseFloat(idAgrupacion)
            };
            arrObjFiltro.push(objFiltro);
            console.log(arrObjFiltro);

            // getInformacion(selectCCFiltros.getAgrupador(), $('#txtFechaInicio').val(), $('#txtFechaFin').val(), idEmpresa).done(function (response) {
            getInformacion(parseFloat(idAgrupacion), $('#txtFechaInicio').val(), $('#txtFechaFin').val(), idEmpresa).done(function (response) {
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
            rowTablaHHT.css('display', 'none'); //TODO
            agregarListaVacia();

            let idAgrupacion = 0;
            let strAgrupacion = $(selectCCRegistro).getAgrupador();
            let idEmpresa = $(selectCCRegistro).getEmpresa();
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            }
            else {
                idAgrupacion = strAgrupacion;
            }

            $.ajax({
                url: 'GetFechasUltimoCorte',
                datatype: "json",
                type: "GET",
                data: {
                    idEmpresa: idEmpresa,
                    idAgrupacion: idAgrupacion
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
            // if ($("#txtHorasHombre").val() == "" || $("#txtLostDay").val() == "") {
            //     countInvalidos++;
            // }
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
            let idEmpresa = $("option:selected", selectCCRegistro).attr("empresa");
            const registroInformacion = {
                horasHombre: +(labelTotalHoras.data('totalHoras')),
                lostDay: $("#txtLostDay").val(),
                cc: '',
                idAgrupacion: $(selectCCRegistro).getAgrupador(),
                idEmpresa: $(selectCCRegistro).getEmpresa(),
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
                let horasContratistas = +($(row).find('.inputContratistas').val());

                if (esEdit) {
                    listaClasificacion.push({
                        id: rowData.id,
                        cc: '',
                        idAgrupacion: $(selectCCRegistro).getAgrupador(),
                        idEmpresa: $(selectCCRegistro).getEmpresa(),
                        fecha: rowData.fechaString,
                        horasMantenimiento: horasMantenimiento,
                        horasOperativo: horasOperativo,
                        horasAdministrativo: horasAdministrativo,
                        horasContratistas: horasContratistas,
                        informacionColaboradoresID: rowData.informacionColaboradoresID,
                        estatus: true
                    });
                } else {
                    listaClasificacion.push({
                        id: 0,
                        cc: '',
                        idAgrupacion: $(selectCCRegistro).getAgrupador(),
                        idEmpresa: $(selectCCRegistro).getEmpresa(),
                        fecha: rowData.fechaString,
                        horasMantenimiento: horasMantenimiento,
                        horasOperativo: horasOperativo,
                        horasAdministrativo: horasAdministrativo,
                        horasContratistas: horasContratistas,
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
                            // tblcolaboradores.ajax.reload(null, false);
                            setDatosInicio();
                            $('#selectCCFiltros').val(listaClasificacion[0].idAgrupacion);
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
                            // tblcolaboradores.ajax.reload(null, false);
                            setDatosInicio();
                            $('#selectCCFiltros').val(listaClasificacion[0].idAgrupacion);
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


        $('#selectCCRegistro').change(getUltimoRegistro)
        //#endregion

        let contMantenimiento;
        let contOperativo;
        let contAdministrativo;
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
                    },
                    {
                        data: 'horasContratistas', title: 'Contratistas',
                        render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center horasClasificacion inputContratistas" value="${data}">`;
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
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
                { fecha: '', fechaString: '', horasMantenimiento: 0, horasOperativo: 0, horasAdministrativo: 0, horasContratistas: 0 },
            );
            AddRows(tablaHHT, listaDiasVacia);
        }

        function calcularTotalHorasClasificacion() {
            let totalHoras = 0;

            tablaHHT.find('tbody tr').each(function (idx, row) {
                let horasMantenimiento = +($(row).find('.inputMantenimiento').val());
                let horasOperativo = +($(row).find('.inputOperativo').val());
                let horasAdministrativo = +($(row).find('.inputAdministrativo').val());
                let horasContratistas = +($(row).find('.inputContratistas').val());

                totalHoras += horasMantenimiento + horasOperativo + horasAdministrativo + horasContratistas;
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

        function getDates(startDate, stopDate) {
            var dateArray = new Array();
            var currentDate = startDate;
            while (currentDate <= stopDate) {
                dateArray.push(new Date(currentDate));
                currentDate = currentDate.addDays(1);
            }
            return dateArray;
        }

        // let objSelected;
        // function fncGetDataSelect2(optionSelected){
        //     let cc = optionSelected.val();
        //     let empresa = optionSelected.attr("empresa");
        //     objSelected = new Object();
        //     objSelected = {
        //         cc: cc,
        //         idEmpresa: empresa
        //     };
        // }

        //#region CALCULO DE HORAS TRABAJADAS - HORAS HOMBRE
        function GetDatos() {
            //#region SE OBTIENEN LAS FECHAS
            let fechas = "";
            for (let i = 0; i < 7; i++) {
                if (i == 0)
                    fechas += arrFechas[i];
                else
                    fechas += "|" + arrFechas[i];
            }
            //#endregion

            //#region SE CONSTUYE EL OBJETO
            let idAgrupacion = $("option:selected", selectCCRegistro).val();
            let idEmpresa = $("option:selected", selectCCRegistro).attr("empresa");

            let objDatos = new Object();
            objDatos = {
                idAgrupacion: $(selectCCRegistro).getAgrupador(),
                idEmpresa: $(selectCCRegistro).getEmpresa(),
                fechas: fechas
            };
            //#endregion

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

                        calcularTotalHorasClasificacion();

                        $.unblockUI();
                        Alert2Exito("Se ha calculado las horas con éxito");
                    } else {
                        Alert2Warning(response.message);
                        $.unblockUI();
                    }
                }
            });
        }

        //#endregion
    }

    $(document).ready(() => CapturaColaboradores.Seguridad = new Seguridad())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop($.unblockUI);
})();