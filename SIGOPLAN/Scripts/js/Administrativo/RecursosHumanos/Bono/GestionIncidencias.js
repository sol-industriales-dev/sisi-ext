(() => {
    $.namespace('Administrativo.RecursosHumanos.MontoAdministrativo');
    MontoAdministrativo = function () {

        const btnModalAutorizar = $('#btnModalAutorizar');
        const buscar = $('.buscar');
        const tblData = $('#tblData');
        const tblAuth = $('#tblAuth');
        const modalDetalle = $('#modalDetalle');
        const modalAuth = $('#modalAuth');
        const btnAuth = $('#btnAuth');
        const getIncidencias = new URL(window.location.origin + '/Administrativo/Bono/getIncidenciasPendiente');
        const IncidenciaAuthDet = new URL(window.location.origin + '/Administrativo/Bono/getIncidenciaAuth');
        const authIncidencia = new URL(window.location.origin + '/Administrativo/Bono/authListIncidencia');
        const revisarFechaCierre = new URL(window.location.origin + '/Administrativo/Bono/RevisarFechaCierre');

        modalDetalle.on('shown.bs.modal', function (e) {
            dtAuth.columns.adjust().draw();
        })

        let init = () => {

            initForm();
        }
        async function fnVerDetalle(id, anio, periodo, tipo_nomina, cc) {
            var _this = $(this);
            response = await ejectFetchJson(IncidenciaAuthDet, { incidenciaID: id, anio: anio, periodo: periodo, tipo_nomina: tipo_nomina, cc: cc });
            if (response.success) {
                dtAuth.clear().draw();
                dtAuth.rows.add(response.datos).draw();                
            }
            modalDetalle.modal("show");
            
        }
        function fnOpenAuth() {
            var seleccionados = false;
            var checks = $('.clsSeleccionado');

            $.each(checks, function (i, e) {
                if ($(e).is(':checked')) {
                    seleccionados = true;
                }

            });
            if (seleccionados) {
                var lst = [];
                var checks = $('.clsSeleccionado');

                $.each(checks, function (i, e) {
                    if ($(e).is(':checked')) {
                        var obj = {};
                        obj.id = $(e).data('id');
                        obj.id_incidencia = 0;
                        lst.push(obj);
                    }
                });

                axios.post("RevisarFechaCierre", { lst }).then(response => {
                    let { success, items, message } = response.data;
                    switch (response.data.estadoIncidencias) {
                        case 0:
                            AlertaGeneral('Alerta', 'Algunas incidencias que intenta autorizar corresponden a perdiodos cerrados.');
                            break;
                        case 1:
                            modalAuth.modal('show');
                            break;
                        case 3:
                            AlertaGeneral('Alerta', 'Se detectaron empleados sin captura registrada para alguno de los centros de costo que desea autorizar.');
                            break;
                        default:
                            AlertaGeneral('Alerta', 'Ocurrio un error al consultar la información.');
                            break;
                    }
                }).catch(error => Alert2Error(error.message));

            }
            else {
                AlertaGeneral('Alerta', 'Debe seleccionar almenos una incidencia!');
            }
        }
        async function fnAuthIncidencia_evPendiente() {
            var lst = [];
            var checks = $('.clsSeleccionado');

            $.each(checks, function (i, e) {
                if ($(e).is(':checked')) {
                    let row = $(e).closest('tr');
                    let rowData = dtData.row(row).data();

                    lst.push({
                        id: $(e).data('id'),
                        id_incidencia: 0,
                        cc: rowData.cc,
                        tipo_nomina: rowData.tipo_nomina,
                        periodo: rowData.periodo,
                        anio: rowData.anio
                    });
                }
            });

            response = await ejectFetchJson(authIncidencia, { lst });
            if (response.success) {

                modalAuth.modal("hide");
                setLstGestionIncidencias();
            }
        }
        function checkFecha(seleccionados) {
            var lst = [];
            var checks = $('.clsSeleccionado');

            $.each(checks, function (i, e) {
                if ($(e).is(':checked')) {
                    var obj = {};
                    obj.id = $(e).data('id');
                    obj.id_incidencia = 0;
                    lst.push(obj);
                }
            });

            axios.post("RevisarFechaCierre", { lst }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    return response.estadoIncidencias;
                } else {
                    return 2;
                }
            }).catch(error => Alert2Error(error.message));
        }
        function initDataTblPrincipal() {
            dtData = tblData.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                //"sScrollX": "100%",
                //"sScrollXInner": "100%",
                "bScrollCollapse": true,
                //scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                initComplete: function (settings, json) {
                    tblData.on('click', '.clsVer', function (e) {
                        let id = $(this).data('id');
                        let anio = $(this).data('anio');
                        let periodo = $(this).data('periodo');
                        let tipo_nomina = $(this).data('tipo_nomina');
                        let cc = $(this).data('cc');
                        fnVerDetalle(id, anio, periodo, tipo_nomina, cc);
                    });
                    $('.clsSelecTodo').change(function(e){
                        if($('.clsSeleccionado:checked').length == $('.clsSeleccionado').length) $('.clsSeleccionado').prop('checked', false);
                        else $('.clsSeleccionado').prop('checked', true);
                    });               
                },
                columns: [
                    { title: 'ID Enkontrol', data: 'id_incidencia', visible: false }
                    , { title: 'CC', data: 'cc' }
                    , {
                        title: 'Nomina', data: 'tipo_nomina', createdCell: function (td, data, rowData, row, col) {
                            var html = "";
                            switch(data)
                            {
                                case 1: html = "SEMANAL"; break;
                                case 4: html = "QUINCENAL"; break;
                                case 20: html = "OBRERO"; break;
                                case 21: html = "EMPLEADO"; break;
                                case 27: html = "CONSTRUCCIÓN CIVIL"; break;
                            }
                            $(td).html(html);
                        }
                    }
                    , { title: 'Año', data: 'anio' }
                    , { title: 'Periodo', data: 'periodo' }                    
                    , { title: 'Fechas', data: 'fechas' }
                    , {
                        title: 'Guardado<br>Completo', data: 'cambio_pendiente', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(rowData.cambio_pendiente ? "<i class='fas fa-times' style='color:red;'></i>" : "<i class='fas fa-check'style='color:green;'></i>");
                            //if (rowData.cambio_pendiente) {
                            //    $(td).addClass("capturaPendiente");
                            //}
                            $(td).css("text-align", "center");
                        }
                    }
                    , {
                        title: 'Evaluacion', data: 'evaluacion_pendiente', createdCell: function (td, data, rowData, row, col) {
                            switch(rowData.evaluacion_pendiente)
                            {
                                case 1: $(td).html("<i class='fas fa-times' style='color:red;'></i>"); break;
                                case 2: $(td).html("<i class='fas fa-check'style='color:green;'></i>"); break;
                                default: $(td).html("N/A"); break;
                            }
                            $(td).css("text-align", "center");
                        }
                    }
                    , {
                        title: 'Detalle', data: 'id', createdCell: function (td, data, rowData, row, col) {
                            var html = "";
                            //if (!rowData.cambio_pendiente) 
                            //{
                            var html = "<button class='btn btn-primary clsVer' data-id='" + rowData.id + "' data-anio='" + rowData.anio + "' data-periodo='" + rowData.periodo + "' data-tipo_nomina='" + rowData.tipo_nomina + "' data-cc='" + rowData.cc + "' style='height: 30px;'><i class='fas fa-eye'></i></button>";
                            //}
                            $(td).html(html);
                            $(td).css("padding-top", "1px");
                            $(td).css("padding-bottom", "1px");
                            $(td).css("text-align", "center");
                        }
                    }
                    , {
                        title: "<input class='form-control clsSelecTodo' style='height: 30px;' type='checkbox' />", data: 'id', createdCell: function (td, data, rowData, row, col) {
                            var html = "<input class='form-control clsSeleccionado' data-id='" + rowData.id + "' style='height: 30px;' type='checkbox' />";
                            if (rowData.cambio_pendiente) {
                                $(td).html("");
                            }
                            else {
                                $(td).html(html);
                                $(td).css("padding", "0");
                                $(td).css("text-align", "center");
                            }
                        }
                    }
                ]
            });
        }

        function initDataTblDetalle() {
            dtAuth = tblAuth.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                //"sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                //scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": true,
                columns: [
                    { title: 'Clave', data: 'clave_empleado' }                    
                    , { title: 'Apellido<br>Paterno', data: 'ape_paterno' }
                    , { title: 'Apellido<br>Materno', data: 'ape_materno' }
                    , { title: 'Nombre', data: 'nombre' }
                    , { title: 'Clave<br>Puesto', data: 'puesto' }
                    , { title: 'Puesto', data: 'puestoDesc' }
                    , { title: 'Departamento', data: 'deptoDesc' }
                    , { title: 'Bono<br>Desempeño<br>Personal', data: 'bono' }
                    , { title: 'Bono<br>Desempeño<br>Mensual', data: 'bonoDM' }
                    , { title: 'Horas<br>Extra', data: 'totalo_Horas' }
                    , { title: 'Dias<br>Extras', data: 'dias_extras' }
                    , { title: 'Total<br>Dias', data: 'total_Dias' }
                    , {
                        title: 'Estatus<br>captura', data: 'estatus', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(rowData.estatus ? "CAPTURADO" : "PENDIENTE");
                            if (!rowData.estatus) {
                                $(td).addClass("capturaPendiente");
                            }
                        }
                    }
                ]
            });
        }
        async function setLstGestionIncidencias() {
            try {
                dtData.clear().draw();
                response = await ejectFetchJson(getIncidencias, {});
                if (response.success) {
                    dtData.rows.add(response.data).draw();                    
                } else {
                    AlertaGeneral(`Erro`, `No se encontraron incidencias pendientes de autorización.`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }

        function initForm() {
            btnModalAutorizar.click(fnOpenAuth);
            btnAuth.click(fnAuthIncidencia_evPendiente);
            initDataTblPrincipal();
            initDataTblDetalle();
            setLstGestionIncidencias();
            $('#modalDetalle').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.MontoAdministrativo = new MontoAdministrativo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
