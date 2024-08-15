$(function () {
    $.namespace('MantenimientoPM.ResetLubricantes');
    ResetLubricantes = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        _idProyectado = 0;
        lstDicEs = {
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
        };
        _horometro = 0;
        _idMantto = 0;
        tbHorometrosDt = $("#tbHorometrosDt"),
        tblHistLubEquipos = $("#tblHistLubEquipos"),
        divEconomicos = $("#divEconomicos"),
        divHistorial = $("#divHistorial"),
        modalResetLubricantes = $("#modalResetLubricantes"),
        tblresetLubricante = $("#tblresetLubricante");
        btnResetLubs = $("#btnResetLubs");
        cboObrasRL = $("#cboObras"),
         btnApliarReset = $("#btnApliarReset");

        var _dataSetGridLubProx;

        function init() {
            btnResetLubs.click(desplegarModal);
            tbHorometrosDt.change(fnCambiarInfor);
            btnApliarReset.click(fnAplicarReset);
        }

        function fnAplicarReset() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/setResetInformacion',
                data: { objs: getTblData(), idMantenimiento: _idMantto },
                async: false,
                success: function (response) {
                    $.unblockUI();
                    getActividadesLubricantes(0, _idMantto, _horometro);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getTblData() {
            array = [];
            for (var i = 0; i < tblHistLubEquipostbl.data().length; i++) {
                obj = {};
                obj = tblHistLubEquipostbl.data()[i];
                if (obj.reset)
                {
                    obj.hrsAplico = tblHistLubEquipos.find('.tbHorometroActual').eq(i).val();
                }
                array.push(obj);
            }
            return array;
        }

        function fnCambiarInfor() {
            fnSetTableTlbGridLub(_dataSetGridLubProx, $(this).val());
        }

        function getActividadesLubricantes(modeloID, idmantenimiento, horometro) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getLubricantesRest',
                data: { modeloEquipoID: modeloID, idmantenimiento: idmantenimiento },
                async: false,
                success: function (response) {
                    $.unblockUI();
                    _dataSetGridLubProx = response.dataSet;
                    fnSetTableTlbGridLub(response.dataSet, horometro); //Carga de tabla de Datatable.r

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function fnSetTableTlbGridLub(dataSet, horometro) {

            tblHistLubEquipostbl = $("#tblHistLubEquipos").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "400px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: getColumnsData(horometro),
                initComplete: function (settings, json) {

                },
                "paging": true,
                "info": false
            });
            tblHistLubEquipostbl.draw(false);
        }

        $("#tblHistLubEquipos").on('change', '.tbHorasAplicacion', function () {

            hrsAplico = $(this).parents('tr').find('.tbHorasAplicacion').val();
            componenteID = $(this).parents('tr').find('.componenteCLS').attr('data-componenteID');
            reset = $(this).parents('tr').find('.ckAplicarReset').is(":checked");
            setInfoData(hrsAplico, componenteID, reset);
            tbHorometrosDt.trigger('change');
        });

        $("#tblHistLubEquipos").on('change', '.ckAplicarReset', function () {

            hrsAplico = $(this).parents('tr').find('.tbHorasAplicacion').val();
            componenteID = $(this).parents('tr').find('.componenteCLS').attr('data-componenteID');
            reset = $(this).parents('tr').find('.ckAplicarReset').is(":checked");
            setInfoData(hrsAplico, componenteID, reset);

            tbHorometrosDt.trigger('change');

        });

        function setInfoData(hrsAplico, componenteID, reset) {

            var array = new Array();
            for (var i = 0; i < tblHistLubEquipostbl.data().length; i++) {
                obj = {};
                obj = tblHistLubEquipostbl.data()[i];
                if (tblHistLubEquipostbl.data()[i].componenteID == componenteID) {

                    obj.hrsAplico = hrsAplico;
                    obj.reset = reset;
                    array.push(obj);
                }
                else {
                    array.push(obj);
                }
            }
            fnSetTableTlbGridLub(array, _horometro);
        }

        function getColumnsData(horometro) {

            var columns = [
             {
                 data: "componente", width: '35px', createdCell: function (td, cellData, rowData, row, col) {
                     $(td).text('');
                     html = '';
                     html = "<input type='text' class='form-control bfh-number componenteCLS' data-componenteID=" + rowData.componenteID + " value='" + cellData + "' disabled>";
                     $(td).append(html);

                 }

             },
             {
                 data: "Suministros", width: '75px',
                 createdCell: function (td, cellData, rowData, row, col) {
                     $(td).text('');
                     html = '';
                     html += '<div class="SelectSum">';
                     html += '<select class="form-control cboAceiteVin" style="margin-top:4px;" title="Seleccione:">'
                     $.each(cellData, function (i, e) {
                         if (e.edadSuministro.length != 0) {
                             html += '<option  id-comp="' + e.componenteID + '"  data-edad="' + e.edadAceite + '"  value="' + e.lubricanteID + '">' + e.nomeclatura + '</option>'
                         }
                     });
                     html += '</select>'
                     html += '</div>';
                     $(td).append(html);

                 }
             },
             {
                 data: "VidaUtil", width: '40px', createdCell: function (td, cellData, rowData, row, col) {
                     $(td).text('');
                     html = '';
                     html = "<input type='number' class='form-control bfh-number InputVida'value='" + cellData.toFixed(2) + "' disabled>";
                     $(td).append(html);

                 }
             },
             {
                 data: "hrsAplico", width: '40px', createdCell: function (td, cellData, rowData, row, col) {
                     $(td).text('');
                     html = '';
                     html = "<input type='number' class='form-control bfh-number tbHorasAplicacion'value='" + cellData + "' >";
                     $(td).append(html);
                 }
             },
            {
                data: "hrsAplico", width: '40px', createdCell: function (td, cellData, rowData, row, col) {
                    //Horometro Actual Aplicar
                    $(td).text('');
                    html = '';
                    html = "<input type='number' class='form-control bfh-number tbHorometroActual'value='" + horometro + "' disabled>";
                    $(td).append(html);
                }
            },
            {
                data: "hrsAplico", width: '40px',
                createdCell: function (td, cellData, rowData, row, col) {
                    //Vida Consumida 
                    VidaConsumida = horometro - rowData.hrsAplico;
                    $(td).text('');
                    html = '';
                    html = "<input type='number' class='form-control bfh-number tbVidaRestante'value='" + VidaConsumida.toFixed(2) + "' disabled>";
                    $(td).append(html);

                }
            },
            {
                data: "VidaRestante", width: '40px',
                createdCell: function (td, cellData, rowData, row, col) {
                    //Vida VidaRestante 
                    $(td).text('');
                    vidaUtil = rowData.VidaUtil;
                    hrsAplico = rowData.hrsAplico;
                    $(td).attr('data-aplico', hrsAplico);
                    Estatus = setPorcentajesVida(vidaUtil, hrsAplico, horometro).Estatus;
                    Barra = setPorcentajesVida(vidaUtil, hrsAplico, horometro).Barra;
                    $(td).append(Estatus);
                    $(td).append(Barra);

                }
            },
            {
                data: "reset", width: '40px',
                createdCell: function (td, cellData, rowData, row, col) {
                    //Vida VidaRestante 
                    $(td).text('');

                    checked = (cellData ? 'checked' : "");
                    html = '';
                    html = "<label  class='btn chkPrueba' >" +
                            "<input type='checkbox'  class='form-control  ckAplicarReset' style='margin-left:20px;' " + checked + ">" +
                        " </label>";
                    $(td).append(html);

                }
            }
            ];

            return columns;

        }

        $('#modalResetLubricantes').on('shown.bs.modal', function () {
            gridResetLubricante.draw();
        });

        function desplegarModal() {
            divEconomicos.removeClass('hide');
            divHistorial.addClass('hide');
            modalResetLubricantes.modal('show');
            loadTblEconomicosPMs();
        }

        function loadTblEconomicosPMs() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/loadTblEconomicosPMs',
                data: { areaCuenta: cboObrasRL.val() },
                success: function (response) {
                    $.unblockUI();
                    setGridEconomicosPMs(response.dataSet);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setGridEconomicosPMs(dataSet) {
            gridResetLubricante = tblresetLubricante.DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: [
                        {
                            data: "Economico", width: '120px',
                        },
                        {
                            data: "proximoPM", width: '40px',
                        },
                        {
                            data: "horometroPM", width: '40px',
                        },
                        {
                            data: "btnViewLub", width: '40px',
                            createdCell: function (td, cellData, rowData, row, col) {
                                html = '';
                                html = '<button class="btn btn-info fnViewHistLub" style="margin-bottom:0px;" data-idMantto = "' + rowData.id + '" data-horometro="' + rowData.horometro.toFixed(2) + '">Ver</button>';
                                $(td).append(html);
                            }
                        },

                ],
                "paging": true,
                "info": false,
                initComplete: function (settings, json) {

                }
            });
        }

        $("#tblresetLubricante").on('click', '.fnViewHistLub', function () {
            divHistorial.removeClass('hide');
            divEconomicos.addClass('hide');
            id = $(this).attr('data-idMantto');
            _horometro = $(this).attr('data-horometro');
            tbHorometrosDt.val(_horometro);

            _idMantto = id;
            getActividadesLubricantes(0, id, $(this).attr('data-horometro'));

        });

        ///Se encarga de renderizar la vida consumida y la vida restante de de las tablas.
        function setPorcentajesVida(vidaUtil, hrsAplico, horometroActual) {

            Estatus = "";
            Barra = "";

            VidaAceite = horometroActual - hrsAplico;
            VidaRestante2 = vidaUtil - VidaAceite;
            VidaRestante = vidaUtil - VidaAceite;
            Vida = (VidaAceite * 100) / vidaUtil;
            Patron = VidaRestante2;
            Estatus = "";

            if (Patron >= 250) {
                Estatus = "<button type='button'  style='color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                 "<div class='progress-bar progress-bar-success' style='width:" + Vida + "%;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 250 && Patron > 0) {
                Estatus = "<button type='button'  style='color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                 "<div class='progress-bar progress-bar-warning' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 0) {
                Estatus = "<button type='button'  style='color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                 "<div class='progress-bar progress-bar-danger' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            }
            return objReturn = {

                Estatus: Estatus,
                Barra: Barra
            };

        }



        init();
    }
    $(document).ready(function () {
        MantenimientoPM.ResetLubricantes = new ResetLubricantes();
    });
});

