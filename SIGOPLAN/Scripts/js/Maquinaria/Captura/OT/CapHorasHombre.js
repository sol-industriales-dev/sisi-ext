(function () {
    $.namespace('maquinaria.HorasHombre.CapHorasHombre');

    CapHorasHombre = function () {
        const btnGuardar = $("#btnGuardar");
        const btnBuscar = $('#btnBuscar');
        const cboCC = $("#cboCC");
        const txtFecha = $("#txtFecha");
        const cboTurno = $("#cboTurno");
        const cboPuesto = $("#cboPuesto");
        const tbNombreEmpleado = $("#tbNombreEmpleado");
        const tblCapturaHorasHombre = $("#tblCapturaHorasHombre");
        const selectDepartamento = $('#selectDepartamento');

        const mensajes = {
            NOMBRE: 'Captura de Horas Hombre.',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            $('#tbNumEmpleado').change(function () {
                axios.post('/HorasHombre/searchNumEmpleado?term=""&puesto=0&numEmpleado=' + $('#tbNumEmpleado').val()).catch(o_O => AlertaGeneral(o_O.message))
                    .then(response => {
                        let { success, items } = response.data;

                        tbNombreEmpleado.attr('data-numempleado', response.data.id)
                        tbNombreEmpleado.text(response.data.label)
                        tbNombreEmpleado.val(response.data.label)
                    });
            })
            txtFecha.datepicker().datepicker("setDate", new Date());
            cboCC.fillCombo('/HorasHombre/fillCboCC', null, false, "--Seleccione--");
            cboPuesto.fillCombo('/HorasHombre/fillCboPuestos', null, false);
            cboPuesto.change(cleanTabla);
            tbNombreEmpleado.getAutocomplete(SelectEmpleado, getFiltrosEmpleados(), '/HorasHombre/searchEmpleado');
            tbNombreEmpleado.change(cleanNombreEmpleado);

            btnBuscar.on('click', function () {
                tbNombreEmpleado.trigger('change');
                loadDataTable();
            });
            cboCC.change(cleanTabla);
            cboTurno.change(cleanTabla);
            txtFecha.change(cleanTabla);
            btnGuardar.click(fnGuardarCaptura);
            tblCapturaHorasHombreGrid = $("#tblCapturaHorasHombre").DataTable({});
        }

        cboCC.on('change', function () {
            if (cboCC.val() != '--Seleccione--') {
                selectDepartamento.fillCombo('/HorasHombre/FillComboDepartamentos', { cc: cboCC.val() }, false, null);
            } else {
                selectDepartamento.empty();
                selectDepartamento.append(`<option value="">--Seleccione--</option>`);
            }
        });

        function cleanNombreEmpleado() {
            cleanTabla();
            if (tbNombreEmpleado.val() == null || tbNombreEmpleado.val() == '') {
                tbNombreEmpleado.attr('data-numempleado', 0);
            }
        }

        function getFiltrosEmpleados() {
            var obj = {};
            obj.puesto = cboPuesto.val() == null || cboPuesto.val() == "" ? 0 : cboPuesto.val();

            return obj;
        }

        function SelectEmpleado(event, ui) {
            tbNombreEmpleado.text(ui.item.value);
            tbNombreEmpleado.attr('data-numempleado', ui.item.id);
        }

        function cleanTabla() {
            tblCapturaHorasHombreGrid.clear().draw();
        }

        function setValueTabla(elemento) {
            var idRow = Number($(elemento).attr('data-index'));
            var Value = Number($(elemento).val());

            var index = $(elemento).parent().parent().index();



            if ($(elemento).hasClass('valueTrabajosInstalaciones')) {
                var count = $(elemento).parent().index()
                tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion[count].tiempo = Value;

            } else if ($(elemento).hasClass('subCatTrabajosInstalaciones')) {
                var count = $(elemento).parent().parent().index();
                tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion[count].subCategoria = Value;

            } else if ($(elemento).hasClass('valueLimpieza')) {
                var count = $(elemento).parent().index();
                tblCapturaHorasHombreGrid.data()[idRow].limpieza[count].tiempo = Value;
            } else if ($(elemento).hasClass('subCatLimpieza')) {

                var count = $(elemento).parent().parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].limpieza[count].subCategoria = Value;


            } else if ($(elemento).hasClass('valueConsultaInformacion')) {

                var count = $(elemento).parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion[count].tiempo = Value;

            } else if ($(elemento).hasClass('subCatConsultaInformacion')) {

                var count = $(elemento).parent().parent().index();
                tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion[count].subCategoria = Value;


            } else if ($(elemento).hasClass('valueTiempoDescanso')) {

                var count = $(elemento).parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso[count].tiempo = Value;


            } else if ($(elemento).hasClass('subCatTiempoDescanso')) {
                var count = $(elemento).parent().parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso[count].subCategoria = Value;

            } else if ($(elemento).hasClass('valueCursosCapacitaciones')) {

                var count = $(elemento).parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion[count].tiempo = Value;


            } else if ($(elemento).hasClass('subCatCursosCapacitaciones')) {

                var count = $(elemento).parent().parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion[count].subCategoria = Value;

            } else if ($(elemento).hasClass('valueMonitoreoDiario')) {

                var count = $(elemento).parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario[count].tiempo = Value;


            } else if ($(elemento).hasClass('subCatMonitoreoDiario')) {

                var count = $(elemento).parent().parent().index();

                tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario[count].subCategoria = Value;

            }

            loadSuma(idRow);
        }

        function loadSuma(idRow) {

            var Total = 0;


            $.each(tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion, function (index, a) {

                Total += a.tiempo;
            });
            $.each(tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion, function (index, b) {
                Total += b.tiempo;
            });
            $.each(tblCapturaHorasHombreGrid.data()[idRow].limpieza, function (index, c) {
                Total += c.tiempo;
            });
            $.each(tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario, function (index, c) {
                Total += c.tiempo;
            });
            $.each(tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso, function (index, c) {
                Total += c.tiempo;
            });
            $.each(tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion, function (index, c) {
                Total += c.tiempo;
            });

            $("label[data-index='" + idRow + "']").text(Total);
        }

        function loadDataTable() {
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/HorasHombre/loadCargaHorasHombre',
                data: {
                    cc: cboCC.val(),
                    clave_depto: +selectDepartamento.val(),
                    fecha: txtFecha.val(),
                    turno: cboTurno.val(),
                    puesto: cboPuesto.val() != "" ? cboPuesto.val() : 0,
                    empleado: tbNombreEmpleado.attr('data-numEmpleado') != undefined ? tbNombreEmpleado.attr('data-numEmpleado') : 0
                },
                async: true,
                success: function (response) {
                    if (response.success) {
                        var dt1 = response.catTrabajosInstalacionesLista;
                        var dt2 = response.catLimpiezaLista;
                        var dt3 = response.catConsultaInformacionLista;
                        var dt4 = response.catTiempoDescansoLista;
                        var dt5 = response.catCursosCapacitacionesLista;
                        var dt6 = response.catMonitoreoDiario;

                        if (response.dtSet != undefined) {
                            setTable(response.dtSet, dt1, dt2, dt3, dt4, dt5, dt6);
                        } else {
                            cleanTabla();
                        }

                        $.unblockUI();
                    }
                    else {
                        $.unblockUI();
                        tblCapturaHorasHombreGrid.clear().draw();
                        AlertaGeneral('Alerta', response.message);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setTable(dataSet, dt1, dt2, dt3, dt4, dt5, dt6) {
            tblCapturaHorasHombreGrid = $("#tblCapturaHorasHombre").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                "bFilter": false,
                "paging": true,
                "info": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: [
                    {
                        data: "nombreEmpleado", width: '120px',
                    },
                    {
                        data: "trabajosInstalacion", width: '40px', // Value
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div> <input data-index="' + row + '" type="text" style="width: 100px;" class="changeEvent valueTrabajosInstalaciones form-control" value="' + 0 + '"> </div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }


                        }
                    },
                    {
                        data: "trabajosInstalacion", width: '195px', //SubCategoria
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt1, row, "subCatTrabajosInstalaciones", 2);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt1, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "limpieza", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div><input type="text" data-index="' + row + '" style="width: 100px;" class="valueLimpieza form-control changeEvent" value="' + 0 + '"></div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "limpieza", width: '195px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt2, row, "subCatLimpieza", 3);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt2, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "cursoCapacitacion", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div><input type="text" data-index="' + row + '" style="width: 100px;" class="valueCursosCapacitaciones form-control changeEvent" value="' + 0 + '"></div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "cursoCapacitacion", width: '195px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt5, row, "subCatCursosCapacitaciones", 6);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt5, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "tiempoDescanso", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div><input type="text" data-index="' + row + '" style="width: 100px;" class="valueTiempoDescanso form-control changeEvent" value="' + 0 + '"></div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "tiempoDescanso", width: '195px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt4, row, "subCatTiempoDescanso", 5);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt4, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "consultaInformacion", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div><input type="text" data-index="' + row + '" style="width: 100px;" class="valueConsultaInformacion form-control changeEvent" value="' + 0 + '"></div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "consultaInformacion", width: '195px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt3, row, "subCatConsultaInformacion", 4);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt3, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "monitoreoDiario", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            for (var i = 0; i < cellData.length; i++) {

                                if (cellData[i].id == 0) {
                                    $(td).append('<div><input data-index="' + row + '" type="text" style="width: 100px;" class="valueMonitoreoDiario form-control changeEvent" value="' + 0 + '"></div>');
                                }
                                else {
                                    $(td).append('<div class="row text-center" ><label>' + cellData[i].tiempo + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "monitoreoDiario", width: '195px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            for (var i = 0; i < cellData.length; i++) {
                                if (cellData[i].id == 0) {
                                    var select = setSubCategoria(dt6, row, "subCatMonitoreoDiario", 7);
                                    $(td).append(select);
                                }
                                else {

                                    arr = jQuery.grep(dt6, function (a) {
                                        return a.Value == cellData[i].subCategoria;
                                    });

                                    $(td).append('<div class="row text-center"> <label>' + arr[0].Text + '</label></div>');
                                }
                            }
                        }
                    },
                    {
                        data: "total", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).append("<div><label class='totalData' data-index='" + row + "'>" + cellData + "</label></div>");
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    { className: 'dt-body-right', targets: [13] },
                    // { width: '5%', targets: [0] }
                ]
            });
        }

        function setSubCategoria(dtSubCat, index, clase, categoria) {
            var select = "<div class='row'>"
            select += "<div class='col-lg-10'>"
            select += "<select class='form-control " + clase + " changeEvent' data-index='" + index + "'>";
            select += "<option value='0'>Seleccione:</option>";
            for (var i = 0; i < dtSubCat.length; i++) {
                select += "<option data-categoria='" + dtSubCat[i].categoria + "' value='" + dtSubCat[i].Value + "'>" + dtSubCat[i].Text + "</option>";
            }
            select += "</select>";
            select += "</div>"
            select += "<div class='col-lg-1' style='padding-left: 0;'>"
            select += "<button type='button' class='btnAddNew btn btn-primary btn-sm' data-categoria='" + categoria + "' data-index='" + index + "'><span class='glyphicon glyphicon-plus'></span></button>";
            select += "</div>"
            select += "</div>"

            return select;
        }

        $(document).on('click', '.btnAddNew', function () {
            setNewSubCat($(this));
        });

        $(document).on('change', '.changeEvent', function () {
            setValueTabla($(this));
        });

        $(document).on('click', '.btnRemove', function () {
            eliminarRegistro($(this));
        });

        function eliminarRegistro(elemento) {

            var indexTD = $(elemento).parent().parent().index();
            var select = $(elemento).parents('td').find('div.row')[indexTD];
            var idRow = $(select).find('select').attr('data-index');
            var value = Number($(select).find('select').val());
            var categoria = Number($(elemento).attr('data-categoria'));

            switch (categoria) {
                case 2:
                    {

                        if (tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion = $.grep(tblCapturaHorasHombreGrid.data()[idRow].trabajosInstalacion,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }
                    }
                    break;
                case 3:
                    {
                        if (tblCapturaHorasHombreGrid.data()[idRow].limpieza.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].limpieza.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].limpieza.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].limpieza = $.grep(tblCapturaHorasHombreGrid.data()[idRow].limpieza,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }

                    }
                    break;
                case 4:
                    {
                        if (tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion = $.grep(tblCapturaHorasHombreGrid.data()[idRow].consultaInformacion,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }
                    }
                    break;
                case 5:
                    {
                        if (tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso = $.grep(tblCapturaHorasHombreGrid.data()[idRow].tiempoDescanso,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }

                    }
                    break;
                case 6:
                    {
                        if (tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion = $.grep(tblCapturaHorasHombreGrid.data()[idRow].cursoCapacitacion,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }

                    }
                    break;
                case 7:
                    {
                        if (tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario.length == 1) {
                            tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario.subCategoria = 0;
                            tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario.tiempo = 0;

                        }
                        else {
                            tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario = $.grep(tblCapturaHorasHombreGrid.data()[idRow].monitoreoDiario,
                                function (o, i) { return o.subCategoria == Number(value); },
                                true);
                        }


                    }
                    break;
                default:

            }
            $($(elemento).parents('td').prev().children()[indexTD]).remove();
            $($(elemento).parents('td').find('div.row')[indexTD]).remove();

            loadSuma(idRow);
        }

        function setNewSubCat(elemento) {

            if (fnHabilitarBoton(elemento)) {

                var index = $(elemento).attr('data-index');

                var obj = {};
                obj.categoriaTrabajo = Number($(elemento).attr('data-categoria'));
                obj.centroCostos = cboCC.val();
                obj.id = 0;
                obj.nombreEmpleado = tblCapturaHorasHombreGrid.data()[index].nombreEmpleado;
                obj.numEmpleado = tblCapturaHorasHombreGrid.data()[index].numEmpleado;
                obj.puestoID = tblCapturaHorasHombreGrid.data()[index].puestoID;
                obj.subCategoria = 0;
                obj.tiempo = 0;
                obj.turno = cboTurno.val();
                obj.usuarioCapturaID = 0;

                var newDiv = $(elemento).parents('td').children().last();
                var newInput = $(elemento).parents('td').prev().children().last();
                var newInputHtml = "<div>" + newInput[0].innerHTML + "</div>"

                var newDivHtml = "<div class='row'>" + newDiv.last().html() + "</div>";
                $(elemento).parents('td').prev().append(newInputHtml);
                $(elemento).parents('td').append(newDivHtml);
                $(elemento).removeClass('btnAddNew');
                $(elemento).addClass('btnRemove');
                $(elemento).children().removeClass('glyphicon glyphicon-plus');
                $(elemento).children().addClass('glyphicon glyphicon-minus');
                $(elemento).removeClass('btn-primary');
                $(elemento).addClass('btn-danger');

                switch (obj.categoriaTrabajo) {
                    case 2:
                        tblCapturaHorasHombreGrid.data()[index].trabajosInstalacion.push(obj);
                        break;
                    case 3:
                        tblCapturaHorasHombreGrid.data()[index].limpieza.push(obj);
                        break;
                    case 4:
                        tblCapturaHorasHombreGrid.data()[index].consultaInformacion.push(obj);
                        break;
                    case 5:
                        tblCapturaHorasHombreGrid.data()[index].tiempoDescanso.push(obj);
                        break;
                    case 6:
                        tblCapturaHorasHombreGrid.data()[index].cursoCapacitacion.push(obj);
                        break;
                    case 7:
                        tblCapturaHorasHombreGrid.data()[index].monitoreoDiario.push(obj);
                        break;


                    default:

                }
                loadSuma(index);

            }
            else {
                AlertaGeneral('Alerta', 'Es necesario tener valores capturados para poder agregar una nueva subcategoria.')
            }
        }

        function fnHabilitarBoton(elemento) {
            var valida = false;

            var valueSelect = Number($(elemento).parents('td').children().last().find('select ').val());
            var valueInput = Number($(elemento).parents('td').prev().children().last().find('input').val());

            if (valueSelect > 0) {
                if (valueInput > 0) {
                    valida = true;
                }
            }

            return valida;
        }

        function fnGuardarCaptura() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/HorasHombre/saveorUpdateInformacion',
                data: { obj: getInfoObject(), cc: cboCC.val(), turno: cboTurno.val(), fechaCaptura: txtFecha.val() },
                async: true,
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral('Alerta', 'La informacion se guardo correctamente.')
                        loadDataTable();
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getInfoObject() {
            var Array = [];

            $.each(tblCapturaHorasHombreGrid.data(), function (index, value) {
                var JsonData = {};
                var Total = value.valueConsultaInformacion + value.valueCursosCapacitaciones + value.valueLimpieza + value.valueTiempoDescanso + value.valueTotalHorashombre + value.valueTrabajosInstalaciones + value.valueMonitoreoDiario;

                $.each(value.consultaInformacion, function (index, a) {

                    Array.push(a);
                });
                $.each(value.cursoCapacitacion, function (index, b) {
                    Array.push(b);
                });
                $.each(value.limpieza, function (index, c) {
                    Array.push(c);
                });
                $.each(value.monitoreoDiario, function (index, c) {
                    Array.push(c);
                });
                $.each(value.tiempoDescanso, function (index, c) {
                    Array.push(c);
                });
                $.each(value.trabajosInstalacion, function (index, c) {
                    Array.push(c);
                });
            });

            return Array;
        }

        init();
    }

    $(document).ready(function () {
        maquinaria.HorasHombre.CapHorasHombre = new CapHorasHombre();
    });
})();