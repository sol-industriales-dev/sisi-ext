(function () {

    $.namespace('Administrativo.Contabilidad.AlertaAutorizacion');

    AlertaAutorizacion = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        const buscar = $('.buscar');

        const comboNomina = $("#comboNomina");
        const comboCC = $("#comboCC");
        //const botonBuscar = $("#botonBuscar");
        const botonReporte = $("#botonReporte");
        const botonCorreo = $("#botonCorreo");
        const botonCorreoDespacho = $("#botonCorreoDespacho");
        const cboTipoNomina = $('#cboTipoNomina');

        const tablaAutorizacion = $("#tablaAutorizacion");
        let dttablaAutorizacion;

        const GetCbotPeriodoNomina = originURL('/Administrativo/Nomina/GetCbotPeriodoNomina');
        const FillCboCC = originURL('/Administrativo/Nomina/GetCCsIncidencias');

        let tipoPeriodo = -1;
        let periodo = -1;
        let anio = -1;
        let cc = '-1'

        function init() {
            AgregarListener();
            Inicializadores();
            LlenarCombos();
            comboNomina.change();
        }

        function AgregarListener() {
            //botonBuscar.click(CargarPrenomina);
            botonReporte.click(DescargarReporte);
            buscar.change(CargarPrenomina);
            botonCorreo.click(EnviarCorreoAutorizacion);
            botonCorreoDespacho.click(VerificarCorreoDespacho);
            cboTipoNomina.change(function (e)
            {
                comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                    //comboNomina.prop("selectedIndex", 1);
                    comboNomina.change();
                });
            });
        }

        function Inicializadores() {
            InittablaAutorizacion();
            comboCC.select2({
                templateResult: function (data, container) {
                    if (data.element) {
                        $(container).addClass($(data.element).attr("class"));
                    }
                    return data.text;
                }
            });
            cboTipoNomina.select2();
        }

        function LlenarCombos()
        {
            comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function() {
                comboNomina.change();
            });

            comboNomina.change(function (e) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                tipoPeriodo = dataPeriodo[2];
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                comboCC.fillComboGroupSelectable(FillCboCC, { periodo: periodo, tipoNomina: tipoPeriodo, anio: anio }, false, "--Seleccione--");
            });
            comboCC.change(function (e) {
                $("#select2-comboCC-container").removeClass('validada');
                $("#select2-comboCC-container").removeClass('no-validada');
                $("#select2-comboCC-container").addClass($("#comboCC option:selected")[0].className);
                $(".select2-selection--single").removeClass('validada');
                $(".select2-selection--single").removeClass('no-validada');
                $(".select2-selection--single").addClass($("#comboCC option:selected")[0].className);                
            });
        }
        
        function InittablaAutorizacion()
        {
            dttablaAutorizacion = tablaAutorizacion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                fixedHeader: true,
                drawCallback: function( settings ) { 
                    tablaAutorizacion.find('button.descargarExcel').click(function(e) {
                        var index = $(this).attr("data-index");
                        DescargarReporte(index);
                    });

                },
                columns: [
                    //{
                    //    title: '<input type="checkbox" class="select-all" style="width: 25px; height: 25px;">',
                    //    orderable: false,
                    //    render: function ( data, type, row ) {
                    //        var html = ''
                    //        if(!row.notificadoOficina)
                    //            html = '<input type="checkbox" class="editor-active seleccionar" style="width: 25px; height: 25px;">';
                    //        return html;
                    //    },
                    //    targets:   0
                    //},
                    { data: 'id', title: 'id', visible: false },
                    { data: 'CC', title: 'CC', visible: false },
                    { data: 'nombreCC', title: 'Centro<br>Costo' },
                    { data: 'usuarioCaptura', title: 'Capturó' },
                    { data: 'usuarioValida', title: 'Validó' },
                    { data: 'fechaValidacion', title: 'Fecha<br>Validó' },
                    { 
                        data: 'autorizadoObra', 
                        title: 'Autorizó<br>Obra', 
                        render: function ( data, type, row ) {
                            if(data)
                                return '<i class="fas fa-check" style="font-size:25px;color:green;"></i>';
                            else
                                return '<i class="fas fa-times" style="font-size:25px;color:red;"></i>';
                        }
                    },
                    { data: 'fechaAutorizacionObra', title: 'Fecha<br>Autorizó<br>Obra' },
                    { 
                        data: 'autorizado', 
                        title: 'Autorizado', 
                        render: function ( data, type, row ) {
                            if(data)
                                return '<i class="fas fa-check" style="font-size:25px;color:green;"></i>';
                            else
                                return '<i class="fas fa-times" style="font-size:25px;color:red;"></i>';
                        }
                    },
                    { data: 'fechaAutorizacion', title: 'Fecha<br>Autorizado' },
                    { 
                        title: 'Descargar<br>Reporte',
                        render: function ( data, type, row ) {
                            return '<button class="btn btn-sm btn-success descargarExcel" data-index=' + row.id + '><i class="fas fa-file-excel"></i></button>';
                        }
                    },
                    { 
                        data: 'notificadoOficina', 
                        title: 'Notificado', 
                        render: function ( data, type, row ) {
                            if(data)
                                return '<i class="fas fa-check" style="font-size:25px;color:green;"></i>';
                            else
                                return '<i class="fas fa-times" style="font-size:25px;color:red;"></i>';
                        }
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] },
                    { width: '20px', targets: [5, 6, 7, 8, 9, 10, 11] },  
                ],    
                select: {
                    style: 'multi'
                },
            });
            tablaAutorizacion.on('click', 'tbody tr', function (e) {
                if (dttablaAutorizacion.row(this).data().notificadoOficina) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    dttablaAutorizacion.row(this).deselect();
                }
            });
        }

        function fnSelRevisa(event, ui) {
            $(this).data("id", ui.item.id);
            $(this).data("nombre", ui.item.value);
        }

        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val() != '') {
                $(this).val("");
                $(this).data("id", "");
                $(this).data("nombre", "");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }

        function CargarPrenomina() {
            if (comboNomina.val()) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                tipoPeriodo = dataPeriodo[2];
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                cc = comboCC.val()
                $.blockUI({ message: 'Procesando...' }); 
                $.get('/Nomina/CargarPrenominasValidadas', { CC: cc, periodo: periodo, tipoNomina: tipoPeriodo, anio: anio })
                .then(
                    function(response) {
                        $.unblockUI();
                        if (response.success) {                            
                            AddRows(tablaAutorizacion, response.listaPrenominas);
                        } else {
                            swal('Alerta!', response.message, 'warning');
                        }
                    }, 
                    function(error) {
                        swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                        $.unblockUI();
                    }
                );
            }
            else {
                swal('Alerta!', 'Debe seleccionar todos los filtros', 'warning');
                $.unblockUI();
            }
        }

        function DescargarReporte(prenominaID)
        {
            //var prenominaID = botonReporte.attr("prenominaID");
            if(prenominaID > 0) {
                $(this).download = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                $(this).href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                location.href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;   
            } 
            else
            {
                swal('Alerta!', "No se ha cargado prenomina", 'warning');
            }
        }

        function EnviarCorreoAutorizacion()
        {
            let prenominasIDs = dttablaAutorizacion.rows('.selected').data().toArray().map(a => a.id);
            $.blockUI({ message: 'Procesando...' }); 
            $.post('/Nomina/EnviarCorreoAutorizantesOficina', { prenominasIDs: prenominasIDs, periodo: periodo, tipoPrenomina: tipoPeriodo, anio: anio })
            .then(
                function(response) {
                    $.unblockUI();
                    if (response.success) {                            
                        AddRows(tablaAutorizacion, response.listaPrenominas);
                        CargarPrenomina();
                        Alert2Exito("Se han enviado las notificaciones señaladas");
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, 
                function(error) {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                    $.unblockUI();
                }
            );
        }

        function VerificarCorreoDespacho()
        {
            $.blockUI({ message: 'Procesando...' }); 
            $.post('/Nomina/VerificarCorreoDespacho', { periodo: periodo, tipoNomina: tipoPeriodo, anio: anio })
            .then(
                function(response) {
                    $.unblockUI();
                    if (response.success) {                            
                        if(response.faltantes.length > 0)
                        {
                            let falantesStr = "";
                            for(var i = 0; i < response.faltantes.length; i++)
                            {
                                falantesStr += "[" + response.faltantes[i].cc + "] " + response.faltantes[i].ccDescripcion + "<br>"; 
                            }
                            Alert2AccionConfirmar("Confirmación", "Esta a punto de enviar la documentación a despacho, con algunos CC sin validar: <br>" + falantesStr + "¿Desea continuar?", "Confirmar", "Cancelar", () => EnviarCorreoDespacho());
                        }
                        else
                        {
                            EnviarCorreoDespacho();
                        }
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, 
                function(error) {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                    $.unblockUI();
                }
            );
        }

        function EnviarCorreoDespacho()
        {
            $.blockUI({ message: 'Procesando...' }); 
            $.post('/Nomina/EnviarCorreoDespacho', { periodo: periodo, tipoNomina: tipoPeriodo, anio: anio })
            .then(
                function(response) {
                    $.unblockUI();
                    if (response.success) {                            
                        Alert2Exito("Se ha enviado la documentación a despacho satisfactoriamente.");
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, 
                function(error) {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                    $.unblockUI();
                }
            );
        }
                
        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        

        init();
    };

    $(document).ready(function () {
        Administrativo.Contabilidad.AlertaAutorizacion = new AlertaAutorizacion();
    });
})();