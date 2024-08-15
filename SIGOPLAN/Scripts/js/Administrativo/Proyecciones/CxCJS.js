
(function () {

    $.namespace('administrativo.proyecciones.CxC');

    CxC = function () {
        var idGlobalRegistro = 0;
        var ElementoTR;
        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
        lblDecripcionObraCxC = $("#lblDecripcionObraCxC"),
        btnConfirmacionEliminarModal = $("#btnConfirmacionEliminarModal");
        modalConfirmacionDelete = $("#modalConfirmacionDelete"),
        cboPeriodo = $("#cboPeriodo"),
        tbMesesInicio = $("#tbMesesInicio"),
        tblCxC = $("#tblCxC");
        var dialog;
        modalEditObraCxC = $("#modalEditObraCxC"),
        BtnAddNewRow = $("#BtnAddNewRow");
        //Modal de Registro Datos
        modalNewObra = $("#modalNewObra"),
        ModalObraCxC = $("#ModalObraCxC"),
        ModalProbabildiadCxC = $("#ModalProbabildiadCxC"),
        ModalCostoYaAplicadoCxC = $("#ModalCostoYaAplicadoCxC"),
        ModalImporteCxC = $("#ModalImporteCxC"),
        btnAddNewRegistro = $("#btnAddNewRegistro");
        btnGuardarCxC = $("#btnGuardarCxC");
        tblTotales = $("#tblTotales"),
        /*Selectores de */
        cboPeriodo = $("#cboPeriodo"),
        tbMesesInicio = $("#tbMesesInicio"),
        cboEscenario = $("#cboEscenario"),
        MensajeConfirmacion = $("#MensajeConfirmacion"),
        TituloConfirmacion = $("#TituloConfirmacion"),
        MensajeConfirmacionModal = $("#MensajeConfirmacionModal");

        ModalUploadFile = $("#ModalUploadFile"),
        btnUploadInfo = $("#btnUploadInfo");

        btnCargarInfo = $("#btnCargarInfo");
        btnGuardarEdit = $("#btnGuardarEdit");

        var Seleccionado;
        function int() {
            //  tblModalEstRecuperacionGrid = $('#tblModalEstRecuperacion').DataTable({});
            // tblModalPorAplicacionDeCostoGrid = $('#tblModalPorAplicacionDeCosto').DataTable({});
            $(document).off('.editRow');
            tblCxC = $('#tblCxC').DataTable({});
            tblTotales = $('#tblTotales').DataTable({});
            LoadTableCxC();
            InitModal();
            BtnAddNewRow.click(OpenModal);
            btnCargarInfo.click(LoadTableCxC);
            btnGuardarCxC.click(getAllData);
            btnGuardarEdit.click(SaveTemp);
            btnConfirmacionEliminarModal.click(DeleteRow);
            btnUploadInfo.click(OpenLoadFile);
            btnMenuPrincipal.click(LoadTableCxC);
        }

        function OpenLoadFile() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/setTipoDocumento',
                type: 'POST',
                dataType: 'json',
                data: { id: 1, mes: tbMesesInicio.val(), anio: cboPeriodo.val(), idGlobal: idGlobalRegistro },
                success: function (response) {
                    ModalUploadFile.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SetFillTable(dataSet) {


            var tamañoY = '60vh';
            var clase = btnMenuPrincipal.attr('class');
            if (clase == "collapsed") {
                var Tamaño = ($(window).width() * 60) / 1366;
                tamañoY = Tamaño + 'vh';
            }
            else {
                var Tamaño = ($(window).width() * 43) / 1366;
                tamañoY = Tamaño + 'vh';
            }

            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            var MesSelected = tbMesesInicio.val();
            tblCxC = $('#tblCxC').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
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
                scrollY: tamañoY,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                columns: [
                 {
                     data: "id", "width": "30px"
                 },
                 {
                     data: "Obra", "width": "440px",
                     createdCell: function (td, cellData, rowData, row, col) {

                         $(td).attr('data-id', rowData.id);
                         $(td).text('');
                         $(td).append('<input type="text" class="CDescripcionObraCxC form-control CxCTamaño" value="' + cellData + '">');
                     }

                 },
                 {
                     data: "Probabilidad", "width": "60px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CProbabilidadCxC text-center CxCTamaño", cellData, 1));
                     }
                 },
                 {
                     data: "CostoYaAplicado", "width": "60px",
                     createdCell: function (td, cellData, rowData, row, col) {

                         var Color = "";
                         if (cellData < 100) {
                             Color = 'SetColorRojo';
                         }

                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CCostoAplicadoCxC text-center CxCTamaño" + Color, cellData, 1));
                     }
                 },
                 {
                     data: "ImporteCxC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CImporteCxC text-center CxCTamaño", cellData, 2));
                     }
                 },
                {
                    data: "accion", "width": "85px",
                    createdCell: function (td, cellData, rowData, row, col) {

                    }
                }

                ],
                "paging": false,
                "info": false,
                "footerCallback": function (row, data, start, end, display) {
                    var nf = new Intl.NumberFormat("es-MX");
                    var api = this.api(), data;


                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {

                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    // Total over all pages
                    total = api
                        .column(4)
                        .data()
                        .reduce(function (a, b) {

                            return intVal(a) + intVal(b);
                        }, 0);
                    // Total over this page

                    // Update footer
                    $(api.column(4).footer()).html(
                       'Total: ' + '$' + nf.format(total)
                    );
                }
            });

            $('.editRow').on('click', function () {
                EditObra($(this));

                Seleccionado;
                Seleccionado = $(this).parents('td');
            });

            $(".removeRow").on('click', function () {
                ElementoTR = $(this).parents('td');
                modalConfirmacionDelete.modal('show');
            });
        }

        $('#btnAddNewRegistro').on('click', function () {

            idRow = GetMaxId();
            idRow += 1;

            tblCxC.row.add({

                "id": idRow,
                "Obra": ModalObraCxC.val(),
                "Probabilidad": ModalProbabildiadCxC.val(),
                "CostoYaAplicado": ModalCostoYaAplicadoCxC.val(),
                "ImporteCxC": ModalImporteCxC.val(),
                "accion": getAccion(idRow),
                "MESV1": 0,
                "MESV2": 0,
                "MESV3": 0,
                "MESV4": 0,
                "MESV5": 0,
                "MESV6": 0,
                "MESV7": 0,
                "MESV8": 0,
                "MESV9": 0,
                "MESV10": 0,
                "MESV11": 0,
                "MESV12": 0,

                "MESP1": 0,
                "MESP2": 0,
                "MESP3": 0,
                "MESP4": 0,
                "MESP5": 0,
                "MESP6": 0,
                "MESP7": 0,
                "MESP8": 0,
                "MESP9": 0,
                "MESP10": 0,
                "MESP11": 0,
                "MESP12": 0,
                "TotalRecuperacion": 0,

                "CxCMargen": 0,
                "CostoXAplicar": 0,
                "MES1AC": 0,
                "MES2AC": 0,
                "MES3AC": 0,
                "MES4AC": 0,
                "MES5AC": 0,
                "MES6AC": 0,
                "MES7AC": 0,
                "MES8AC": 0,
                "MES9AC": 0,
                "MES10AC": 0,
                "MES11AC": 0,
                "MES12AC": 0,
                "MES1PORAC": 0,
                "MES2PORAC": 0,
                "MES3PORAC": 0,
                "MES4PORAC": 0,
                "MES5PORAC": 0,
                "MES6PORAC": 0,
                "MES7PORAC": 0,
                "MES8PORAC": 0,
                "MES9PORAC": 0,
                "MES10PORAC": 0,
                "MES11PORAC": 0,
                "MES12PORAC": 0,
                "TotalAC": 0

            }).draw(false);
        });


        function DeleteRow() {

            modalConfirmacionDelete.modal('hide');
            tblCxC.row(ElementoTR).remove().draw()
            getAllDataDelete();
        }


        function GuardarDataDelete(Array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/GuardarInfoTabla',
                type: 'POST',
                dataType: 'json',
                data: { obj: Array, obj1: cboEscenario.val(), obj2: tbMesesInicio.val(), obj3: cboPeriodo.val(), id: idGlobalRegistro },
                //data: { obj: Array, obj1: "1", obj2: "2", obj3: "2016", id: 0 },//cboPeriodo.val() },
                success: function (response) {
                    $.unblockUI();
                    AlertaGeneral('Confirmación', 'El Registro Fue eliminado correctamente');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getAllDataDelete() {

            var Array = [];
            $.each(tblCxC.data(), function (index, value) {
                var JsonData = {};

                if (value.length != 0) {
                    JsonData.id = value.id;
                    JsonData.Obra = value.Obra;
                    JsonData.Probabilidad = value.Probabilidad;
                    JsonData.CostoYaAplicado = value.CostoYaAplicado;
                    JsonData.ImporteCxC = value.ImporteCxC;
                    JsonData.MESV1 = value.MESV1;
                    JsonData.MESV2 = value.MESV2;
                    JsonData.MESV3 = value.MESV3;
                    JsonData.MESV4 = value.MESV4;
                    JsonData.MESV5 = value.MESV5;
                    JsonData.MESV6 = value.MESV6;
                    JsonData.MESV7 = value.MESV7;
                    JsonData.MESV8 = value.MESV8;
                    JsonData.MESV9 = value.MESV9;
                    JsonData.MESV10 = value.MESV10;
                    JsonData.MESV11 = value.MESV11;
                    JsonData.MESV12 = value.MESV12;
                    JsonData.MESP1 = value.MESP1;
                    JsonData.MESP2 = value.MESP2;
                    JsonData.MESP3 = value.MESP3;
                    JsonData.MESP4 = value.MESP4;
                    JsonData.MESP5 = value.MESP5;
                    JsonData.MESP6 = value.MESP6;
                    JsonData.MESP7 = value.MESP7;
                    JsonData.MESP8 = value.MESP8;
                    JsonData.MESP9 = value.MESP9;
                    JsonData.MESP10 = value.MESP10;
                    JsonData.MESP11 = value.MESP11;
                    JsonData.MESP12 = value.MESP12;
                    JsonData.TotalRecuperacion = value.TotalRecuperacion;
                    JsonData.CxCMargen = value.CxCMargen;
                    JsonData.CostoXAplicar = value.CostoXAplicar;
                    JsonData.MES1AC = value.MES1AC;
                    JsonData.MES2AC = value.MES2AC;
                    JsonData.MES3AC = value.MES3AC;
                    JsonData.MES4AC = value.MES1AC;
                    JsonData.MES5AC = value.MES2AC;
                    JsonData.MES6AC = value.MES3AC;
                    JsonData.MES7AC = value.MES1AC;
                    JsonData.MES8AC = value.MES2AC;
                    JsonData.MES9AC = value.MES3AC;
                    JsonData.MES10AC = value.MES1AC;
                    JsonData.MES11AC = value.MES2AC;
                    JsonData.MES12AC = value.MES3AC;


                    JsonData.MES1PORAC = value.MES1PORAC;
                    JsonData.MES2PORAC = value.MES2PORAC;
                    JsonData.MES3PORAC = value.MES3PORAC;
                    JsonData.MES4PORAC = value.MES4PORAC;
                    JsonData.MES5PORAC = value.MES5PORAC;
                    JsonData.MES6PORAC = value.MES6PORAC;
                    JsonData.MES7PORAC = value.MES7PORAC;
                    JsonData.MES8PORAC = value.MES8PORAC;
                    JsonData.MES9PORAC = value.MES9PORAC;
                    JsonData.MES10PORAC = value.MES10PORAC;
                    JsonData.MES11PORAC = value.MES11PORAC;
                    JsonData.MES12PORAC = value.MES12PORAC;

                    JsonData.TotalAC = value.TotalAC;
                    Array.push(JsonData);
                }
            });


            GuardarDataDelete(Array);
        }

        function GetMaxId() {
            var idRow = $('.editRow[data-idrow]').get().map(function (value) {
                return Number($(value).attr('data-idrow'));
            });
            Array.prototype.max = function () {
                return Math.max.apply(null, this);
            };
            return idRow.max();
        }
        function getAccion(id) {

            return "<div> <button class='btn btn-warning editRow' data-idrow='" + id + "' type='button' style='margin: 2px;'> " +
                                 "<span class='glyphicon glyphicon-edit'></span></button>" +
                                 "<button class='btn btn-danger removeRow'data-idrow='" + id + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-remove'></span></button> " +
                "</div>";
        }

        function SetModalDataTable(dataSet) {
            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            tblModalEstRecuperacionGrid = $('#tblModalEstRecuperacion').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
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
                destroy: true,
                data: dataSet,
                columns: [
                        {
                            "title": tituloMeses[0], data: "MESV1", "width": "85px",
                            createdCell: function (td, cellData, rowData, row, col) {
                                $(td).text('');
                                $(td).attr('data-id', rowData.id);
                                $(td).append(GetInput("P1 CPRecupeacionesMESCxC", cellData, 1));
                            }
                        },
                 {
                     "title": tituloMeses[1], data: "MESV2", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P2 CPRecupeacionesMESCxC", cellData, 1));
                     }
                 },
                 {
                     "title": tituloMeses[2], data: "MESV3", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P3 CPRecupeacionesMESCxC", cellData, 1));


                     }
                 },
                 {
                     "title": tituloMeses[3], data: "MESV4", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P4 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[4], data: "MESV5", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P5 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[5], data: "MESV6", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P6 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[6], data: "MESV7", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P7 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[7], data: "MESV8", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P8 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[8], data: "MESV9", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P9 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[9], data: "MESV10", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P10 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 },
                 {
                     "title": tituloMeses[10], data: "MESV11", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P11 CPRecupeacionesMESCxC", cellData, 1));
                     }
                 },
                 {
                     "title": tituloMeses[11], data: "MESV12", "width": "85px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("P12 CPRecupeacionesMESCxC", cellData, 1));

                     }
                 }
                ],
                "paging": false,
                "info": false
            });
        }

        function SetDataImporteRecuperacion(dataSet) {
            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            tblModalImporteRecuperacion = $('#tblModalImporteRecuperacion').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
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
                destroy: true,
                data: dataSet,
                columns: [
                      {
                          "title": tituloMeses[0], data: "MESP1", "width": "10%",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');
                              $(td).attr('data-id', rowData.id);
                              $(td).append(GetInput("MESP1 CVRecupeacionesMESCxC", cellData / 1000, 2));

                          }
                      },
                 {
                     "title": tituloMeses[1], data: "MESP2", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP2 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[2], data: "MESP3", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP3 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[3], data: "MESP4", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP4 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[4], data: "MESP5", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP5 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[5], data: "MESP6", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP6 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[6], data: "MESP7", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP7 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     "title": tituloMeses[7], data: "MESP8", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);

                         $(td).append(GetInput("MESP8 CVRecupeacionesMESCxC", cellData / 1000, 2));
                     }
                 },
                 {
                     "title": tituloMeses[8], data: "MESP9", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);

                         $(td).append(GetInput("MESP9 CVRecupeacionesMESCxC", cellData / 1000, 2));
                     }
                 },
                 {
                     "title": tituloMeses[9], data: "MESP10", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP10 CVRecupeacionesMESCxC", cellData / 1000, 2));
                     }
                 },
                 {
                     "title": tituloMeses[10], data: "MESP11", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP11 CVRecupeacionesMESCxC", cellData / 1000, 2));
                     }
                 },
                 {
                     "title": tituloMeses[11], data: "MESP12", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MESP12 CVRecupeacionesMESCxC", cellData / 1000, 2));

                     }
                 },
                 {
                     data: "TotalRecuperacion", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CTotalRecuperacionCxC", cellData / 1000, 2));

                     }
                 }
                ],
                "paging": false,
                "info": false
            });

            $(".CVRecupeacionesMESCxC").prop('disabled', true);
            $('.CTotalRecuperacionCxC').prop('disabled', true);
        }

        function SetDataPorAplicarDeCostos(dataSet) {
            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            tblModalPorAplicacionDeCostoGrid = $('#tblModalPorAplicacionDeCosto').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
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
                destroy: true,
                data: dataSet,
                columns: [
                     {
                         data: "CxCMargen", "width": "110px", createdCell: function (td, cellData, rowData, row, col) {
                             $(td).text('');
                             $(td).attr('data-id', rowData.id);
                             $(td).append(GetInput("CxCXMargen", cellData / 1000, 2));

                         }
                     },
                 {
                     data: "CostoXAplicar", "width": "110px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CCostoXAplicarCxC", cellData, 2));

                     }
                 },
                      {
                          "title": tituloMeses[0], data: "MES1PORAC", "width": "120px",
                          createdCell: function (td, cellData, rowData, row, col) {
                              $(td).text('');
                              $(td).attr('data-id', rowData.id);
                              $(td).append(GetInput("MES1PORAC CpAplicacionDeCostosCxC", cellData, 2));

                          }
                      },
                 {
                     "title": tituloMeses[1], data: "MES2PORAC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES2PORAC CpAplicacionDeCostosCxC", cellData, 2));
                     }
                 },
                 {
                     "title": tituloMeses[2], data: "MES3PORAC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES3PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 },
                 {
                     "title": tituloMeses[3], data: "MES4PORAC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES4PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 },
                 {
                     "title": tituloMeses[4], data: "MES5PORAC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES5PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 },
                 {
                     "title": tituloMeses[5], data: "MES6PORAC", "width": "120px",
                     createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES6PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 },
                 {
                     "title": tituloMeses[6], data: "MES7PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES7PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 },
                 {
                     "title": tituloMeses[7], data: "MES8PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);

                         $(td).append(GetInput("MES8PORAC CpAplicacionDeCostosCxC", cellData, 2));
                     }
                 },
                 {
                     "title": tituloMeses[8], data: "MES9PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);

                         $(td).append(GetInput("MES9PORAC CpAplicacionDeCostosCxC", cellData, 2));
                     }
                 },
                 {
                     "title": tituloMeses[9], data: "MES10PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES10PORAC CpAplicacionDeCostosCxC", cellData, 2));
                     }
                 },
                 {
                     "title": tituloMeses[10], data: "MES11PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES11PORAC CpAplicacionDeCostosCxC", cellData, 2));
                     }
                 },
                 {
                     "title": tituloMeses[11], data: "MES12PORAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("MES12PORAC CpAplicacionDeCostosCxC", cellData, 2));

                     }
                 }
                ],
                "paging": false,
                "info": false
            });

            if (tblModalEstRecuperacionGrid.data()[0].CostoYaAplicado != 100) {
                $('.CpAplicacionDeCostosCxC').prop('disabled', false)

            }
            else {
                $('.CpAplicacionDeCostosCxC').prop('disabled', true)
            }

        }

        function SetDataMontoAplicarDeCostos(dataSet) {
            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            tblModalValorAplicaciondeCostoGrid = $('#tblModalValorAplicaciondeCosto').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
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
                destroy: true,
                data: dataSet,
                columns: [
                 {
                     "title": tituloMeses[0], data: "MES1AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES1", cellData / 1000, 2));
                     }
                 },
                 {
                     "title": tituloMeses[1], data: "MES2AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES2", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[2], data: "MES3AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES3", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[3], data: "MES4AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES4", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                    {
                        "title": tituloMeses[4], data: "MES5AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).attr('data-id', rowData.id);
                            $(td).append(GetInput("CPAplicacionCostoMESCxC MES5", cellData / 1000, 2));
                        }
                    },
                 {
                     "title": tituloMeses[5], data: "MES6AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES6", cellData / 1000, 2));

                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[6], data: "MES7AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES7", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[7], data: "MES8AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES8", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[8], data: "MES9AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES9", cellData / 1000, 2));

                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[9], data: "MES10AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES10", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                 {
                     "title": tituloMeses[10], data: "MES11AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPAplicacionCostoMESCxC MES12", cellData / 1000, 2));
                     },
                     "orderable": false
                 },
                {
                    "title": tituloMeses[11], data: "MES12AC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');
                        $(td).attr('data-id', rowData.id);
                        $(td).append(GetInput("CPAplicacionCostoMESCxC MES12", cellData / 1000, 2));
                    },
                    "orderable": false
                },
                 {
                     data: "TotalAC", "width": "120px", createdCell: function (td, cellData, rowData, row, col) {
                         $(td).text('');
                         $(td).attr('data-id', rowData.id);
                         $(td).append(GetInput("CPTotalCxC", cellData / 1000, 1));
                     }
                 }
                ],
                "paging": false,
                "info": false
            });
            $(".CPAplicacionCostoMESCxC").prop('disabled', true);
            $(".CPTotalCxC").prop('disabled', true);

        }

        function removeCommas(str) {

            while (str.search(",") >= 0) {
                str = (str + "").replace(',', '');
            }
            return str;
        };

        function getAllData() {
            var Array = [];
            $.each(tblCxC.data(), function (index, value) {
                var JsonData = {};

                if (value.length != 0) {
                    JsonData.id = value.id;
                    JsonData.Obra = value.Obra;
                    JsonData.Probabilidad = value.Probabilidad;
                    JsonData.CostoYaAplicado = value.CostoYaAplicado;
                    JsonData.ImporteCxC = value.ImporteCxC;
                    JsonData.MESV1 = value.MESV1;
                    JsonData.MESV2 = value.MESV2;
                    JsonData.MESV3 = value.MESV3;
                    JsonData.MESV4 = value.MESV4;
                    JsonData.MESV5 = value.MESV5;
                    JsonData.MESV6 = value.MESV6;
                    JsonData.MESV7 = value.MESV7;
                    JsonData.MESV8 = value.MESV8;
                    JsonData.MESV9 = value.MESV9;
                    JsonData.MESV10 = value.MESV10;
                    JsonData.MESV11 = value.MESV11;
                    JsonData.MESV12 = value.MESV12;
                    JsonData.MESP1 = value.MESP1;
                    JsonData.MESP2 = value.MESP2;
                    JsonData.MESP3 = value.MESP3;
                    JsonData.MESP4 = value.MESP4;
                    JsonData.MESP5 = value.MESP5;
                    JsonData.MESP6 = value.MESP6;
                    JsonData.MESP7 = value.MESP7;
                    JsonData.MESP8 = value.MESP8;
                    JsonData.MESP9 = value.MESP9;
                    JsonData.MESP10 = value.MESP10;
                    JsonData.MESP11 = value.MESP11;
                    JsonData.MESP12 = value.MESP12;
                    JsonData.TotalRecuperacion = value.TotalRecuperacion;
                    JsonData.CxCMargen = value.CxCMargen;
                    JsonData.CostoXAplicar = value.CostoXAplicar;
                    JsonData.MES1AC = value.MES1AC;
                    JsonData.MES2AC = value.MES2AC;
                    JsonData.MES3AC = value.MES3AC;
                    JsonData.MES4AC = value.MES1AC;
                    JsonData.MES5AC = value.MES2AC;
                    JsonData.MES6AC = value.MES3AC;
                    JsonData.MES7AC = value.MES1AC;
                    JsonData.MES8AC = value.MES2AC;
                    JsonData.MES9AC = value.MES3AC;
                    JsonData.MES10AC = value.MES1AC;
                    JsonData.MES11AC = value.MES2AC;
                    JsonData.MES12AC = value.MES3AC;


                    JsonData.MES1PORAC = value.MES1PORAC;
                    JsonData.MES2PORAC = value.MES2PORAC;
                    JsonData.MES3PORAC = value.MES3PORAC;
                    JsonData.MES4PORAC = value.MES4PORAC;
                    JsonData.MES5PORAC = value.MES5PORAC;
                    JsonData.MES6PORAC = value.MES6PORAC;
                    JsonData.MES7PORAC = value.MES7PORAC;
                    JsonData.MES8PORAC = value.MES8PORAC;
                    JsonData.MES9PORAC = value.MES9PORAC;
                    JsonData.MES10PORAC = value.MES10PORAC;
                    JsonData.MES11PORAC = value.MES11PORAC;
                    JsonData.MES12PORAC = value.MES12PORAC;

                    JsonData.TotalAC = value.TotalAC;
                    Array.push(JsonData);
                }
            });
            GuardarData(Array);
        }

        function GetValueHtml(tipo, elemento) {
            html = $.parseHTML(elemento);

            switch (tipo) {
                case 1:
                    return $(html).children('input').val();
                case 2:
                    return $(html).children('input').val();
                case 3:
                    return $(html).children('input').val();
                default:
            }
        }

        function getObjetoRow(elemento) {
            var nf = new Intl.NumberFormat();

            var value = $(elemento).parents('tr').children();
            var JsonData = {};
            JsonData.id = 0;
            JsonData.Obra = $(value[1]).children().children('input').val();///GetValueHtml(1, value.Obra);
            JsonData.Probabilidad = Number(removeCommas($(value[2]).children().children('input').val()));
            JsonData.CostoYaAplicado = Number(removeCommas($(value[3]).children().children('input').val()));
            JsonData.ImporteCxC = Number(removeCommas($(value[4]).children().children('input').val()));

            JsonData.MESV1 = Number(removeCommas($(value[5]).children().children('input').val()));
            JsonData.MESV2 = Number(removeCommas($(value[6]).children().children('input').val()));
            JsonData.MESV3 = Number(removeCommas($(value[7]).children().children('input').val()));
            JsonData.MESV4 = Number(removeCommas($(value[8]).children().children('input').val()));
            JsonData.MESV5 = Number(removeCommas($(value[9]).children().children('input').val()));
            JsonData.MESV6 = Number(removeCommas($(value[10]).children().children('input').val()));
            JsonData.MESV7 = Number(removeCommas($(value[11]).children().children('input').val()));
            JsonData.MESV8 = Number(removeCommas($(value[12]).children().children('input').val()));
            JsonData.MESV9 = Number(removeCommas($(value[13]).children().children('input').val()));
            JsonData.MESV10 = Number(removeCommas($(value[14]).children().children('input').val()));
            JsonData.MESV11 = Number(removeCommas($(value[15]).children().children('input').val()));
            JsonData.MESV12 = Number(removeCommas($(value[16]).children().children('input').val()));

            JsonData.MESP1 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV1);

            JsonData.MESP2 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV2);
            JsonData.MESP3 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV3);
            JsonData.MESP4 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV4);
            JsonData.MESP5 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV5);
            JsonData.MESP6 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV6);
            JsonData.MESP7 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV7);
            JsonData.MESP8 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV8);
            JsonData.MESP9 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV9);
            JsonData.MESP10 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV10);
            JsonData.MESP11 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV11);
            JsonData.MESP12 = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.MESV12);
            JsonData.TotalRecuperacion = JsonData.MESP1 +
                                         JsonData.MESP2 +
                                         JsonData.MESP3 +
                                         JsonData.MESP4 +
                                         JsonData.MESP5 +
                                         JsonData.MESP6 +
                                         JsonData.MESP7 +
                                         JsonData.MESP8 +
                                         JsonData.MESP9 +
                                         JsonData.MESP10 +
                                         JsonData.MESP11 +
                                         JsonData.MESP12;



            JsonData.CxCMargen = OpeXPorcentaje(JsonData.ImporteCxC, JsonData.CostoYaAplicado);
            JsonData.CostoXAplicar = JsonData.TotalRecuperacion - JsonData.CxCMargen;
            JsonData.MES1AC = OpeXPorcentaje(JsonData.CostoXAplicar, 0);// $("#mes1").val());
            JsonData.MES2AC = OpeXPorcentaje(JsonData.CostoXAplicar, 0); //$("#mes1").val());
            JsonData.MES3AC = OpeXPorcentaje(JsonData.CostoXAplicar, 0); //$("#mes1").val());
            JsonData.MES4AC = OpeXPorcentaje(JsonData.CostoXAplicar, 0); //$("#mes1").val());
            JsonData.TotalAC = JsonData.MES1AC + JsonData.MES2AC + JsonData.MES3AC + JsonData.MES4AC;

            $(value[17]).children().children('input').val(nf.format(JsonData.MESP1));
            $(value[18]).children().children('input').val(nf.format(JsonData.MESP2));
            $(value[19]).children().children('input').val(nf.format(JsonData.MESP3));
            $(value[20]).children().children('input').val(nf.format(JsonData.MESP4));
            $(value[21]).children().children('input').val(nf.format(JsonData.MESP5));
            $(value[22]).children().children('input').val(nf.format(JsonData.MESP6));
            $(value[23]).children().children('input').val(nf.format(JsonData.MESP7));
            $(value[24]).children().children('input').val(nf.format(JsonData.MESP8));
            $(value[25]).children().children('input').val(nf.format(JsonData.MESP9));
            $(value[26]).children().children('input').val(nf.format(JsonData.MESP10));
            $(value[27]).children().children('input').val(nf.format(JsonData.MESP11));
            $(value[28]).children().children('input').val(nf.format(JsonData.MESP12));
            $(value[29]).children().children('input').val(nf.format(JsonData.TotalRecuperacion));
            $(value[30]).children().children('input').val(nf.format(JsonData.CxCMargen));
            $(value[31]).children().children('input').val(nf.format(JsonData.CostoXAplicar));

            return JsonData;
        }

        function OpeXPorcentaje(importe, porcentaje) {

            var temp = Number(importe) * (Number(porcentaje) / 100);
            var resultado = Math.round(temp);

            return resultado;
        }

        function GuardarData(Array) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/GuardarInfoCxC',
                type: 'POST',
                dataType: 'json',
                data: { obj: Array, mes: tbMesesInicio.val(), anio: cboPeriodo.val(), id: idGlobalRegistro },
                //data: { obj: Array, mes: 6, anio: 2017, id: idGlobalRegistro },
                success: function (response) {
                    LoadTableCxC();
                    AlertaGeneral("Confirmación", 'El Registro se actualizo correctamente');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetInput(Classe, valor, tipo) {
            var nf = new Intl.NumberFormat();

            switch (tipo) {
                case 1: // Porcentajes 
                    {
                        return "<input type='number' class='form-control " + Classe + " DecimalSet ' value='" + Math.round(valor) + "' max='100'>";
                    }
                case 2://Totales E importes
                    {
                        return "<input type='text' class='form-control " + Classe + " ' value='" + nf.format(Math.round(valor)) + "'>";
                    }
                case 3://Descripciones
                    {
                        return "<input type='text' class='form-control " + Classe + " ' value='" + valor + "' >";
                    }
            }
        }

        function InitModal() {
            dialog = $("#modalNewObra").dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
        }

        function SaveTemp() {
            var ObjRow = {};
            ObjRow = tblModalEstRecuperacionGrid.data()[0];
            ChangeAllinfo(Seleccionado, ObjRow);

            modalEditObraCxC.modal('hide');
        }

        $(document).on('change', ".CPRecupeacionesMESCxC", function () {
            ChangeText($(this), tblModalEstRecuperacionGrid, 1);
            var tempCxC = {};
            tempCxC = tblModalEstRecuperacionGrid.data()[0];
            var dataSet = {};
            dataSet = GetInfoCxC(tempCxC);
            Arreglo = [];
            Arreglo.push(dataSet);
            SetModalDataTable(Arreglo);
            var ObjXEstimacionRecuperacion = tblModalEstRecuperacionGrid.data()[0];
            SetChangeData(ObjXEstimacionRecuperacion);
        });



        $(document).on('change', '.CpAplicacionDeCostosCxC', function () {
            ChangeText($(this), tblModalEstRecuperacionGrid, 2);
            var tempCxC = {};
            tempCxC = tblModalEstRecuperacionGrid.data()[0];
            var dataSet = {};
            dataSet = GetInfoCxC(tempCxC);
            Arreglo = [];
            Arreglo.push(dataSet);
            SetModalDataTable(Arreglo);
            var ObjXEstimacionRecuperacion = tblModalEstRecuperacionGrid.data()[0];
            SetChangeData(ObjXEstimacionRecuperacion);
        });

        $(document).on('change', ".CCostoAplicadoCxC", function () {
            ChangeText($(this), tblCxC, 3);
            if ($(this).val() < 100) {
                $(this).addClass('SetColorRojo');
            } else {
                $(this).removeClass('SetColorRojo');
            }
        });

        $(document).on('change', ".CImporteCxC", function () {
            ChangeText($(this), tblCxC, 4);
        });



        function EditObra(_this) {
            activo = $(_this).parent().parent('td');
            var tempCxC = {};
            tempCxC = tblCxC.row(activo).data();

            lblDecripcionObraCxC.text(tempCxC.Obra);
            var dataSet = {};
            dataSet = GetInfoCxC(tempCxC);
            Arreglo = [];
            Arreglo.push(dataSet);
            SetModalDataTable(Arreglo);
            var ObjXEstimacionRecuperacion = tblModalEstRecuperacionGrid.data()[0];
            SetChangeData(ObjXEstimacionRecuperacion);
            modalEditObraCxC.modal('show');
        }

        function GetInfoCxC(tempCxC) {
            var dataSet = {};
            dataSet.CostoXAplicar = tempCxC.CostoXAplicar;
            dataSet.CostoYaAplicado = tempCxC.CostoYaAplicado;
            dataSet.CxCMargen = tempCxC.CxCMargen;
            dataSet.ImporteCxC = tempCxC.ImporteCxC;
            dataSet.MES1AC = tempCxC.MES1AC;
            dataSet.MES1PORAC = tempCxC.MES1PORAC;
            dataSet.MES2AC = tempCxC.MES2AC;
            dataSet.MES2PORAC = tempCxC.MES2PORAC;
            dataSet.MES3AC = tempCxC.MES3AC;
            dataSet.MES3PORAC = tempCxC.MES3PORAC;
            dataSet.MES4AC = tempCxC.MES4AC;
            dataSet.MES4PORAC = tempCxC.MES4PORAC;
            dataSet.MES5AC = tempCxC.MES5AC;
            dataSet.MES5PORAC = tempCxC.MES5PORAC;
            dataSet.MES6AC = tempCxC.MES6AC;
            dataSet.MES6PORAC = tempCxC.MES6PORAC;
            dataSet.MES7AC = tempCxC.MES7AC;
            dataSet.MES7PORAC = tempCxC.MES7PORAC;
            dataSet.MES8AC = tempCxC.MES8AC;
            dataSet.MES8PORAC = tempCxC.MES8PORAC;
            dataSet.MES9AC = tempCxC.MES9AC;
            dataSet.MES9PORAC = tempCxC.MES9PORAC;
            dataSet.MES10AC = tempCxC.MES10AC;
            dataSet.MES10PORAC = tempCxC.MES10PORAC;
            dataSet.MES11AC = tempCxC.MES11AC;
            dataSet.MES11PORAC = tempCxC.MES11PORAC;
            dataSet.MES12AC = tempCxC.MES12AC;
            dataSet.MES12PORAC = tempCxC.MES12PORAC;
            dataSet.MESP1 = tempCxC.MESP1;
            dataSet.MESP2 = tempCxC.MESP2;
            dataSet.MESP3 = tempCxC.MESP3;
            dataSet.MESP4 = tempCxC.MESP4;
            dataSet.MESP5 = tempCxC.MESP5;
            dataSet.MESP6 = tempCxC.MESP6;
            dataSet.MESP7 = tempCxC.MESP7;
            dataSet.MESP8 = tempCxC.MESP8;
            dataSet.MESP9 = tempCxC.MESP9;
            dataSet.MESP10 = tempCxC.MESP10;
            dataSet.MESP11 = tempCxC.MESP11;
            dataSet.MESP12 = tempCxC.MESP12;
            dataSet.MESV1 = tempCxC.MESV1;
            dataSet.MESV2 = tempCxC.MESV2;
            dataSet.MESV3 = tempCxC.MESV3;
            dataSet.MESV4 = tempCxC.MESV4;
            dataSet.MESV5 = tempCxC.MESV5;
            dataSet.MESV6 = tempCxC.MESV6;
            dataSet.MESV7 = tempCxC.MESV7;
            dataSet.MESV8 = tempCxC.MESV8;
            dataSet.MESV9 = tempCxC.MESV9;
            dataSet.MESV10 = tempCxC.MESV10;
            dataSet.MESV11 = tempCxC.MESV11;
            dataSet.MESV12 = tempCxC.MESV12;
            dataSet.Obra = tempCxC.Obra;
            dataSet.Probabilidad = tempCxC.Probabilidad;
            dataSet.TotalAC = tempCxC.TotalAC;
            dataSet.TotalRecuperacion = tempCxC.TotalRecuperacion;

            return dataSet;
        }

        function SetChangeData(ObjXEstimacionRecuperacion) {
            Arreglo2 = [];
            var DataSetImpRecuperacion = {};
            DataSetImpRecuperacion.MESP1 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV1) / 100;
            DataSetImpRecuperacion.MESP2 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV2) / 100;
            DataSetImpRecuperacion.MESP3 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV3) / 100;
            DataSetImpRecuperacion.MESP4 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV4) / 100;
            DataSetImpRecuperacion.MESP5 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV5) / 100;
            DataSetImpRecuperacion.MESP6 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV6) / 100;
            DataSetImpRecuperacion.MESP7 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV7) / 100;
            DataSetImpRecuperacion.MESP8 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV8) / 100;
            DataSetImpRecuperacion.MESP9 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV9) / 100;
            DataSetImpRecuperacion.MESP10 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV10) / 100;
            DataSetImpRecuperacion.MESP11 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV11) / 100;
            DataSetImpRecuperacion.MESP12 = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.MESV12) / 100;
            DataSetImpRecuperacion.TotalRecuperacion = SumaDatos(DataSetImpRecuperacion);

            DataSetImpRecuperacion.CxCMargen = (ObjXEstimacionRecuperacion.ImporteCxC * ObjXEstimacionRecuperacion.CostoYaAplicado) / 100;
            DataSetImpRecuperacion.CostoXAplicar = ObjXEstimacionRecuperacion.ImporteCxC - DataSetImpRecuperacion.CxCMargen;
            DataSetImpRecuperacion.MES1PORAC = ObjXEstimacionRecuperacion.MES1PORAC;
            DataSetImpRecuperacion.MES2PORAC = ObjXEstimacionRecuperacion.MES2PORAC;
            DataSetImpRecuperacion.MES3PORAC = ObjXEstimacionRecuperacion.MES3PORAC;
            DataSetImpRecuperacion.MES4PORAC = ObjXEstimacionRecuperacion.MES4PORAC;
            DataSetImpRecuperacion.MES5PORAC = ObjXEstimacionRecuperacion.MES5PORAC;
            DataSetImpRecuperacion.MES6PORAC = ObjXEstimacionRecuperacion.MES6PORAC;
            DataSetImpRecuperacion.MES7PORAC = ObjXEstimacionRecuperacion.MES7PORAC;
            DataSetImpRecuperacion.MES8PORAC = ObjXEstimacionRecuperacion.MES8PORAC;
            DataSetImpRecuperacion.MES9PORAC = ObjXEstimacionRecuperacion.MES9PORAC;
            DataSetImpRecuperacion.MES10PORAC = ObjXEstimacionRecuperacion.MES10PORAC;
            DataSetImpRecuperacion.MES11PORAC = ObjXEstimacionRecuperacion.MES11PORAC;
            DataSetImpRecuperacion.MES12PORAC = ObjXEstimacionRecuperacion.MES12PORAC;

            DataSetImpRecuperacion.MES1AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES1PORAC / 100;
            DataSetImpRecuperacion.MES2AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES2PORAC / 100;
            DataSetImpRecuperacion.MES3AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES3PORAC / 100;
            DataSetImpRecuperacion.MES4AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES4PORAC / 100;
            DataSetImpRecuperacion.MES5AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES5PORAC / 100;
            DataSetImpRecuperacion.MES6AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES6PORAC / 100;
            DataSetImpRecuperacion.MES7AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES7PORAC / 100;
            DataSetImpRecuperacion.MES8AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES8PORAC / 100;
            DataSetImpRecuperacion.MES9AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES9PORAC / 100;
            DataSetImpRecuperacion.MES10AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES10PORAC / 100;
            DataSetImpRecuperacion.MES11AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES11PORAC / 100;
            DataSetImpRecuperacion.MES12AC = ObjXEstimacionRecuperacion.CostoXAplicar * ObjXEstimacionRecuperacion.MES12PORAC / 100;

            DataSetImpRecuperacion.TotalAC = SumaAC(DataSetImpRecuperacion);
            DataSetImpRecuperacion.CostoXAplicar = ObjXEstimacionRecuperacion.CostoXAplicar;

            Arreglo2.push(DataSetImpRecuperacion);
            SetDataImporteRecuperacion(Arreglo2);
            SetDataPorAplicarDeCostos(Arreglo2);
            SetDataMontoAplicarDeCostos(Arreglo2);
        }

        function ChangeText(_this, tablaGrid, col) {
            switch (col) {
                case 1:
                    {
                        ObjetoRow = tblModalEstRecuperacionGrid.data()[0];
                        var porcentaje = Number($(_this).val());
                        var importe = ObjetoRow.ImporteCxC;
                        var Mes = $(_this).attr('class').split(' ')[1];
                        var Result = OpeXPorcentaje(importe, porcentaje);
                        SetValue(tblModalEstRecuperacionGrid, Mes, Result, porcentaje);
                        break;
                    }
                case 2:
                    {
                        ObjetoRow = tblModalEstRecuperacionGrid.data()[0];
                        var porcentaje = Number($(_this).val());
                        var importe = ObjetoRow.CostoXAplicar;
                        var Mes = $(_this).attr('class').split(' ')[1];
                        var Result = OpeXPorcentaje(importe, porcentaje);
                        SetValue(tblModalEstRecuperacionGrid, Mes, Result, porcentaje);
                        break;
                    }
                case 3:
                    {
                        activo = $(_this).parents('td');
                        tablaGrid.row(activo).data().CostoYaAplicado = Number($(_this).val());
                        var ObjRow = tablaGrid.row(activo).data();
                        tablaGrid.row(activo).data().CxCMargen = (ObjRow.CostoYaAplicado * ObjRow.ImporteCxC) / 100;
                        tablaGrid.row(activo).data().CostoXAplicar = ObjRow.ImporteCxC - tablaGrid.row(activo).data().CxCMargen
                        break;
                    }
                case 4:
                    {
                        activo = $(_this).parents('td');
                        tablaGrid.row(activo).data().ImporteCxC = Number(removeCommas($(_this).val()));
                        var ObjRow = {};
                        ObjRow = tblCxC.row(activo).data();
                        ChangeAllinfo(activo, ObjRow);

                        break;
                    }
                default:

            }
        }

        function ChangeAllinfo(activo, ObjRow) {

            tblCxC.row(activo).data().MESP1 = (ObjRow.MESV1 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP2 = (ObjRow.MESV2 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP3 = (ObjRow.MESV3 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP4 = (ObjRow.MESV4 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP5 = (ObjRow.MESV5 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP6 = (ObjRow.MESV6 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP7 = (ObjRow.MESV7 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP8 = (ObjRow.MESV8 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP9 = (ObjRow.MESV9 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP10 = (ObjRow.MESV10 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP11 = (ObjRow.MESV11 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().MESP12 = (ObjRow.MESV12 * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().TotalRecuperacion = SumaDatos(ObjRow);
            tblCxC.row(activo).data().CxCMargen = (ObjRow.CostoYaAplicado * ObjRow.ImporteCxC) / 100;
            tblCxC.row(activo).data().CostoXAplicar = (ObjRow.ImporteCxC - ObjRow.CxCMargen);

            tblCxC.row(activo).data().MESV1 = ObjRow.MESV1;
            tblCxC.row(activo).data().MESV2 = ObjRow.MESV2;
            tblCxC.row(activo).data().MESV3 = ObjRow.MESV3;
            tblCxC.row(activo).data().MESV4 = ObjRow.MESV4;
            tblCxC.row(activo).data().MESV5 = ObjRow.MESV5;
            tblCxC.row(activo).data().MESV6 = ObjRow.MESV6;
            tblCxC.row(activo).data().MESV7 = ObjRow.MESV7;
            tblCxC.row(activo).data().MESV8 = ObjRow.MESV8;
            tblCxC.row(activo).data().MESV9 = ObjRow.MESV9;
            tblCxC.row(activo).data().MESV10 = ObjRow.MESV10;
            tblCxC.row(activo).data().MESV11 = ObjRow.MESV11;
            tblCxC.row(activo).data().MESV12 = ObjRow.MESV12;



            tblCxC.row(activo).data().MES1PORAC = ObjRow.MES1PORAC;
            tblCxC.row(activo).data().MES2PORAC = ObjRow.MES2PORAC;
            tblCxC.row(activo).data().MES3PORAC = ObjRow.MES3PORAC;
            tblCxC.row(activo).data().MES4PORAC = ObjRow.MES4PORAC;
            tblCxC.row(activo).data().MES5PORAC = ObjRow.MES5PORAC;
            tblCxC.row(activo).data().MES6PORAC = ObjRow.MES6PORAC;
            tblCxC.row(activo).data().MES7PORAC = ObjRow.MES7PORAC;
            tblCxC.row(activo).data().MES8PORAC = ObjRow.MES8PORAC;
            tblCxC.row(activo).data().MES9PORAC = ObjRow.MES9PORAC;
            tblCxC.row(activo).data().MES10PORAC = ObjRow.MES10PORAC;
            tblCxC.row(activo).data().MES11PORAC = ObjRow.MES11PORAC;
            tblCxC.row(activo).data().MES12PORAC = ObjRow.MES12PORAC;


            tblCxC.row(activo).data().MES1AC = (ObjRow.MES1PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES2AC = (ObjRow.MES2PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES3AC = (ObjRow.MES3PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES4AC = (ObjRow.MES4PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES5AC = (ObjRow.MES5PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES6AC = (ObjRow.MES6PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES7AC = (ObjRow.MES7PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES8AC = (ObjRow.MES8PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES9AC = (ObjRow.MES9PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES10AC = (ObjRow.MES10PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES11AC = (ObjRow.MES11PORAC * ObjRow.CostoXAplicar) / 100;
            tblCxC.row(activo).data().MES12AC = (ObjRow.MES12PORAC * ObjRow.CostoXAplicar) / 100;

            ObjetoRow = {};
            ObjetoRow = tblCxC.row(activo).data();

            tblCxC.row(activo).data().TotalAC = SumaAC(ObjetoRow);

        }

        function SetInfoChange(SelectorActivo, tablaGrid) {

            activo = $(SelectorActivo).parents('td');
            var ObjRow = tablaGrid.row(activo).data();
            var id = $(SelectorActivo).parent().parent().attr('data-id');
            var CxCXMargen = $("td[data-id='" + id + "']").children().children('.CxCXMargen');
            var listaInputPorcentaje = $("td[data-id='" + id + "']").children().children('.CPRecupeacionesMESCxC');
            var listaInputValores = $("td[data-id='" + id + "']").children().children('.CVRecupeacionesMESCxC');
            var TotalRecuperacion = $("td[data-id='" + id + "']").children().children('.CTotalRecuperacionCxC');

            tablaGrid.row(activo).data().MESP1 = (ObjRow.MESV1 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP2 = (ObjRow.MESV2 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP3 = (ObjRow.MESV3 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP4 = (ObjRow.MESV4 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP5 = (ObjRow.MESV5 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP6 = (ObjRow.MESV6 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP7 = (ObjRow.MESV7 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP8 = (ObjRow.MESV8 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP9 = (ObjRow.MESV9 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP10 = (ObjRow.MESV10 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP11 = (ObjRow.MESV11 * ObjRow.ImporteCxC) / 100;
            tablaGrid.row(activo).data().MESP12 = (ObjRow.MESV12 * ObjRow.ImporteCxC) / 100;

            $(listaInputValores[0]).val(tablaGrid.row(activo).data().MESP1);
            $(listaInputValores[1]).val(tablaGrid.row(activo).data().MESP2);
            $(listaInputValores[2]).val(tablaGrid.row(activo).data().MESP3);
            $(listaInputValores[3]).val(tablaGrid.row(activo).data().MESP4);
            $(listaInputValores[4]).val(tablaGrid.row(activo).data().MESP5);
            $(listaInputValores[5]).val(tablaGrid.row(activo).data().MESP6);
            $(listaInputValores[6]).val(tablaGrid.row(activo).data().MESP7);
            $(listaInputValores[7]).val(tablaGrid.row(activo).data().MESP8);
            $(listaInputValores[8]).val(tablaGrid.row(activo).data().MESP9);
            $(listaInputValores[9]).val(tablaGrid.row(activo).data().MESP10);
            $(listaInputValores[10]).val(tablaGrid.row(activo).data().MESP11);
            $(listaInputValores[11]).val(tablaGrid.row(activo).data().MESP12);


            tablaGrid.row(activo).data().CxCMargen = (ObjRow.CostoYaAplicado * ObjRow.ImporteCxC) / 100;


            $(TotalRecuperacion).val(SumaDatos(ObjRow));
            $(CxCXMargen).val(tablaGrid.row(activo).data().CxCMargen);
            tablaGrid.row(activo).data().TotalRecuperacion = SumaDatos(ObjRow);
        }

        function SumaDatos(ObjetoRow) {
            return ObjetoRow.MESP1 +
                    ObjetoRow.MESP2 +
                    ObjetoRow.MESP3 +
                    ObjetoRow.MESP4 +
                    ObjetoRow.MESP5 +
                    ObjetoRow.MESP6 +
                    ObjetoRow.MESP7 +
                    ObjetoRow.MESP8 +
                    ObjetoRow.MESP9 +
                    ObjetoRow.MESP10 +
                    ObjetoRow.MESP11 +
                    ObjetoRow.MESP12;
        }

        function SumaAC(ObjetoRow) {
            return ObjetoRow.MES1AC +
                    ObjetoRow.MES2AC +
                    ObjetoRow.MES3AC +
                    ObjetoRow.MES4AC +
                    ObjetoRow.MES5AC +
                    ObjetoRow.MES6AC +
                    ObjetoRow.MES7AC +
                    ObjetoRow.MES8AC +
                    ObjetoRow.MES9AC +
                    ObjetoRow.MES10AC +
                    ObjetoRow.MES11AC +
                    ObjetoRow.MES12AC;
        }

        function SetValue(tablaGrid, posicion, valor, porcentaje) {
            switch (posicion) {
                case "P1": {
                    tablaGrid.data()[0].MESP1 = valor;
                    tablaGrid.data()[0].MESV1 = porcentaje;
                    break;
                }
                case "P2": {
                    tablaGrid.data()[0].MESP2 = valor;
                    tablaGrid.data()[0].MESV2 = porcentaje;
                    break;
                }
                case "P3": {
                    tablaGrid.data()[0].MESP3 = valor;
                    tablaGrid.data()[0].MESV3 = porcentaje;
                    break;
                }
                case "P4": {
                    tablaGrid.data()[0].MESP4 = valor;
                    tablaGrid.data()[0].MESV4 = porcentaje;
                    break;
                }
                case "P5": {
                    tablaGrid.data()[0].MESP5 = valor;
                    tablaGrid.data()[0].MESV5 = porcentaje;
                    break;
                }
                case "P6": {
                    tablaGrid.data()[0].MESP6 = valor;
                    tablaGrid.data()[0].MESV6 = porcentaje;
                    break;
                }
                case "P7": {
                    tablaGrid.data()[0].MESP7 = valor;
                    tablaGrid.data()[0].MESV7 = porcentaje;
                    break;
                }
                case "P8": {
                    tablaGrid.data()[0].MESP8 = valor;
                    tablaGrid.data()[0].MESV8 = porcentaje;
                    break;
                }
                case "P9": {
                    tablaGrid.data()[0].MESP9 = valor;
                    tablaGrid.data()[0].MESV9 = porcentaje;
                    break;
                }
                case "P10": {
                    tablaGrid.data()[0].MESP10 = valor;
                    tablaGrid.data()[0].MESV10 = porcentaje;
                    break;
                }
                case "P11": {
                    tablaGrid.data()[0].MESP11 = valor;
                    tablaGrid.data()[0].MESV11 = porcentaje;
                    break;
                }
                case "P12": {
                    tablaGrid.data()[0].MESP12 = valor;
                    tablaGrid.data()[0].MESV12 = porcentaje;
                    break;
                }
                case "MES1PORAC": {
                    tablaGrid.data()[0].MES1AC = valor;
                    tablaGrid.data()[0].MES1PORAC = porcentaje;
                    break;
                }
                case "MES2PORAC": {
                    tablaGrid.data()[0].MES2AC = valor;
                    tablaGrid.data()[0].MES2PORAC = porcentaje;
                    break;
                }
                case "MES3PORAC": {
                    tablaGrid.data()[0].MES3AC = valor;
                    tablaGrid.data()[0].MES3PORAC = porcentaje;
                    break;
                }
                case "MES4PORAC": {
                    tablaGrid.data()[0].MES4AC = valor;
                    tablaGrid.data()[0].MES4PORAC = porcentaje;
                    break;
                }
                case "MES5PORAC": {
                    tablaGrid.data()[0].MES5AC = valor;
                    tablaGrid.data()[0].MES5PORAC = porcentaje;
                    break;
                }
                case "MES6PORAC": {
                    tablaGrid.data()[0].MES6AC = valor;
                    tablaGrid.data()[0].MES6PORAC = porcentaje;
                    break;
                }
                case "MES7PORAC": {
                    tablaGrid.data()[0].MES7AC = valor;
                    tablaGrid.data()[0].MES7PORAC = porcentaje;
                    break;
                }
                case "MES8PORAC": {
                    tablaGrid.data()[0].MES8AC = valor;
                    tablaGrid.data()[0].MES8PORAC = porcentaje;
                    break;
                }
                case "MES9PORAC": {
                    tablaGrid.data()[0].MES9AC = valor;
                    tablaGrid.data()[0].MES9PORAC = porcentaje;
                    break;
                }
                case "MES10PORAC": {
                    tablaGrid.data()[0].MES10AC = valor;
                    tablaGrid.data()[0].MES10PORAC = porcentaje;
                    break;
                }
                case "MES11PORAC": {
                    tablaGrid.data()[0].MES11AC = valor;
                    tablaGrid.data()[0].MES11PORAC = porcentaje;
                    break;
                }
                case "MES12PORAC": {
                    tablaGrid.data()[0].MES12AC = valor;
                    tablaGrid.data()[0].MES12PORAC = porcentaje;
                    break;
                }
                default:
                    {
                        var objGrid = tablaGrid.row(activo).data();
                        tablaGrid.data()[0].TotalRecuperacion = SumaDatos(objGrid);
                    }


            }
        }

        function LoadTableCxC() {
            var mes = GetPeriodoMeses();
            // idTituloContainer.text('CxC ' + mes[0]);
            tblCxC.clear().draw();
            tblTotales.clear().draw();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GetFillTableCxC',
                type: 'POST',
                dataType: 'json',
                data: { escenario: cboEscenario.val(), meses: tbMesesInicio.val(), anio: cboPeriodo.val() },
                //data: { escenario: 1, meses: 6, anio: 2017 },
                success: function (response) {
                    if (response.EstadoRegreso > 0) {
                        var dataRes = response.GetData;
                        idGlobalRegistro = response.id;
                        SetFillTable(dataRes);
                        if (response.EstadoRegreso == 1) {
                            btnGuardarCxC.removeClass('hide');
                            btnUploadInfo.removeClass('hide');
                        } else {
                            btnGuardarCxC.addClass('hide');
                            btnUploadInfo.addClass('hide');
                        }
                    } else {
                        SetFillTable(null);
                        btnGuardarCxC.addClass('hide');
                        btnUploadInfo.addClass('hide');
                    }

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetPeriodoMeses() {

            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];

            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;

                tituloMeses.push(months[i] + " " + periodo);
            }

            for (var i = 0 ; i < MesInicio; i++) {

                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
            }
            return tituloMeses;

        }

        function OpenModal() {
            dialog.dialog("open");
        }
        int();
    };

    $(document).ready(function () {

        administrativo.proyecciones.CxC = new CxC();
    });
})();

