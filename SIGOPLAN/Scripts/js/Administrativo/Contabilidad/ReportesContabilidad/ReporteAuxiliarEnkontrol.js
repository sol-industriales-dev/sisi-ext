(function () {

    $.namespace('Administrativo.Contabilidad.ReporteAuxiliarEnkontrol');

    ReporteAuxiliarEnkontrol = function () {
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        // --> ELEMENTOS
        const btnBuscar = $("#btnBuscar");
        const inputFechaInicio = $("#inputFechaInicio");
        const inputFechaFin = $("#inputFechaFin");
        const comboCtaInicio = $("#comboCtaInicio");
        const comboCtaFin = $("#comboCtaFin");
        const comboAC = $("#comboAC");
        const inputTabla = $("#inputTabla");
        const tblAuxiliar = $("#tblAuxiliar");
        const botonExcel = $("#botonExcel");

        let dtTblAuxiliar;

        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        
        function init() {
            // --> INICIALIZADORES
            initTblAuxiliar();

            // --> CARGAR COMBOS
            comboAC.fillCombo('/ReportesContabilidad/ReportesContabilidad/getListaAC');
            comboAC.select2();

            comboCtaInicio.fillCombo('/ReportesContabilidad/ReportesContabilidad/GetCuentas');
            comboCtaInicio.select2();  
            //comboCtaInicio.select2({
            //    minimumInputLength: 5,
            //    tags: [],
            //    ajax: {
            //        url: '/ReportesContabilidad/ReportesContabilidad/GetCuentas',
            //        dataType: 'json',
            //        type: "GET",
            //        results: function (data) {
            //            return {
            //                results: $.map(data.items, function (item) {
            //                    return {
            //                        text: item.Text,
            //                        id: item.Value
            //                    }
            //                })
            //            };
            //        }
            //    }
            //});            

            comboCtaFin.fillCombo('/ReportesContabilidad/ReportesContabilidad/GetCuentas');
            comboCtaFin.select2();     

            inputFechaInicio.datepicker().datepicker('setDate', new Date());
            inputFechaFin.datepicker().datepicker('setDate', new Date());

            // --> LISTENERS
            btnBuscar.click(cargarTabla);
            botonExcel.click(function() {
                $(".btn-tblAuxiliar-excel").click();
            });  
            inputTabla.keyup(function() {
                $("#tblAuxiliar_filter input").val(inputTabla.val());
                $("#tblAuxiliar_filter input").keyup();
            });

        }

        function initTblAuxiliar()
        {
            dtTblAuxiliar = tblAuxiliar.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                //dom: 't',
                ordering: false,
                columnDefs: [
                    { className: "dt-center", targets: [ 0, 1, 2, 3, 4, 5, 6, 7, 13, 15 ] },
                    { className: "dt-left", targets: [ 8, 9, 10, 14 ] },
                    { className: "dt-right", targets: [ 11, 12 ] },

                    { className: "dt-right", targets: [ 11, 12 ] },

                    { width: '10px', targets: [ 0, 1, 2, 3, 4, 5, 6, 7, 9, 13, 14 ] },
                    { width: '20px', targets: [ 11, 12, 15 ] },
                    { width: '40px', targets: [ 8 ] },
                ],
                dom: 'Bf<t>',
                buttons: [
                    {
                        extend: 'excelHtml5',// footer: true,
                        className: "btn btn-excel-ocultar btn-tblAuxiliar-excel",
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        }
                    },
                ],
                columns: [
                    { title: "Año", data: 'year' }
                    , { title: "Mes", data: 'mes' }
                    , { title: "Póliza", data: 'poliza' }
                    , { title: "TP", data: 'tp' }
                    , { title: "Linea", data: 'linea' }
                    , { title: "cta", data: 'cta' }
                    , { title: "scta", data: 'scta' }
                    , { title: "sscta", data: 'sscta' }
                    , { title: "Ref.", data: 'referencia' }
                    , { title: "C.C.", data: 'cc' }
                    , { title: "Concepto", data: 'concepto' }                    
                    , { 
                        title: "Cargo", data: 'cargo', render: function (data, type, row)
                        {
                            return '$' + parseFloat(data, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
                        } 
                    }
                    , { 
                        title: "Abono", data: 'abono', render: function (data, type, row)
                        {
                            return '$' + parseFloat(data, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
                        } 
                    }
                    , { title: "itm", data: 'itm' }
                    , { title: "A.C.", data: 'areaCuenta' }
                    , { title: "Fecha", data: 'fechapolStr' }
                   
                ]
                , initComplete: function (settings, json) {
                    $("#tblAuxiliar_filter").addClass("input-filter-ocultar");
                    $("#tblAuxiliar_filter input").addClass("input-filter-keydown");
                }
                , footerCallback: function (row, data, start, end, display) {
                    let api = this.api(); 
                    let intVal = function (i) {
                        return typeof i === 'string'
                            ? i.replace(/[\$,]/g, '') * 1
                            : typeof i === 'number'
                            ? i
                            : 0;
                    };
                    totalCargo = api.column(11).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                    totalAbono = api.column(12).data().reduce((a, b) => intVal(a) + intVal(b), 0);
 
                    api.column(11).footer().innerHTML = '$' + totalCargo.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
                    api.column(12).footer().innerHTML = '$' + totalAbono.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
                }
            });
        }

        function cargarTabla()
        {            
            $.get('/ReportesContabilidad/ReportesContabilidad/cargarAuxiliarEnkontrol', { fechaInicio: JSON.stringify(inputFechaInicio.val()), fechaFin: JSON.stringify(inputFechaFin.val()), ctaInicio: comboCtaInicio.val(), ctaFin: comboCtaFin.val(), areaCuenta: JSON.stringify(comboAC.val()) })
                .then(response => {
                    if (response.success) {
                        dtTblAuxiliar.clear().draw();
                        dtTblAuxiliar.rows.add(response.items).draw();
                    } 
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                }
            );
        }

        init();
    };

    $(document).ready(function () {

        Administrativo.Contabilidad.ReporteAuxiliarEnkontrol = new ReporteAuxiliarEnkontrol();
    }).ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
      .ajaxStop(() => { $.unblockUI(); });
})();