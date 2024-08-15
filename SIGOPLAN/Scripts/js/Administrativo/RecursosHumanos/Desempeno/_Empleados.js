(function () {
    $.namespace('RecursosHumanos.Desempeno._Empleados');
    _Empleados = function () {
        const buscarEmpleado = $("#buscarEmpleado");
        const cbEstatus = $("#cbEstatus");
        const tblEmpleados = $("#tblEmpleados");
        let dttblEmpleados;
        const agregarEmpleado = $("#agregarEmpleado");
        const modalAltaEmpleado = $("#modalAltaEmpleado");
        
        const txtEmpleado = $("#txtEmpleado");
        const txtJefe = $("#txtJefe");
        const cbTipo = $("#cbTipo");
        const guardarEmpleado = $("#guardarEmpleado");
        let todosLosProcesos;

        const btnAgregarProceso =$('#btnAgregarProceso');
        const tblProcesos = $('#tblProcesos');
        let dtTblProcesos;
        const modalAgregarProceso = $('#modalAgregarProceso');
        const txtProceso = $('#txtProceso');
        const txtFechaInicioProceso = $('#txtFechaInicioProceso');
        const txtFechaFinProceso = $('#txtFechaFinProceso');
        const btnRegistrarProceso = $('#btnRegistrarProceso');
        const btnActualizarProceso =$('#btnActualizarProceso');

        const modalEvaluacionesPorProceso = $('#modalEvaluacionesPorProceso');
        const tblEvaluacionesPorProceso = $('#tblEvaluacionesPorProceso');
        let dtTblEvaluacionesPorProceso;
        const btnAgregarEvaluacionPorProceso = $('#btnAgregarEvaluacionPorProceso');
        const divAgregarEvaluacion = $('#divAgregarEvaluacion');
        const txtNombreEvaluacion = $('#txtNombreEvaluacion');
        const txtFechaInicioEvaluacion = $('#txtFechaInicioEvaluacion');
        const txtFechaFinEvaluacion = $('#txtFechaFinEvaluacion');
        const btnGuardarEvaluacion = $('#btnGuardarEvaluacion');
        const btnCancelarRegistroEvaluacion = $('#btnCancelarRegistroEvaluacion');

        const fechaActual = new Date();
        const añoActual = fechaActual.getFullYear();
        const diaActual = fechaActual.getDate();
        const mesActual = fechaActual.getMonth();
        const ultimoDiaDelMes = moment(new Date(añoActual, mesActual, 1)).endOf('month').format('DD');

        txtFechaFinEvaluacion.on('change', function () {
            if (moment(txtFechaFinEvaluacion.val(), 'DD/MM/YYYY').isBefore(moment(txtFechaInicioEvaluacion.val(), 'DD/MM/YYYY'))) {
                txtFechaFinEvaluacion.val('');
                //AlertaGeneral('Alerta', 'La fecha de finalización no debe ser menor a la fecha de inicio');
                Alert2Warning("La fecha de finalización no debe ser menor a la fecha de inicio 2");
            }
        });

        txtFechaInicioEvaluacion.on('change', function () {
            if (moment(txtFechaInicioEvaluacion.val(), 'DD/MM/YYYY').isValid() && txtFechaFinEvaluacion.val() == ''){
                txtFechaFinEvaluacion.val(moment(txtFechaInicioEvaluacion.val(), 'DD/MM/YYYY').endOf('month').format('DD/MM/YYYY'));
            }
        }); 

        btnGuardarEvaluacion.on('click', function() {
            if (validarRegEvaluacion()) {
                CUDEvaluacion(crearObjetoEvaluacion($(this).data('evaluacionid'), $(this).data('procesoid'), false));
            }else{
                Alert2Error("Se requiere ingresar todos los datos");
            }
        });

        btnAgregarEvaluacionPorProceso.on('click', function(e) {
            txtNombreEvaluacion.val('');
            txtFechaInicioEvaluacion.val(moment(fechaActual).format('DD/MM/YYYY'));
            txtFechaFinEvaluacion.val(moment(new Date(añoActual, mesActual, ultimoDiaDelMes)).format('DD/MM/YYYY'));
            btnGuardarEvaluacion.data('evaluacionid', '');
        });

        btnCancelarRegistroEvaluacion.on('click', function() {
            txtNombreEvaluacion.val('');
            txtFechaInicioEvaluacion.val('');
            txtFechaFinEvaluacion.val('');
            btnGuardarEvaluacion.data('evaluacionid', '');
        });

        btnAgregarProceso.on('click', function () {
            modalAgregarProceso.find('modal-title').text('Agregar proceso');
            btnRegistrarProceso.show();
            btnActualizarProceso.hide();

            txtProceso.val("");
            txtFechaInicioProceso.val("");
            txtFechaFinProceso.val("");
            modalAgregarProceso.find(".errorClass").removeClass("errorClass");

            txtFechaInicioProceso.val(moment(fechaActual).format('DD/MM/YYYY')); 
            txtFechaFinProceso.val(moment(new Date(añoActual, mesActual, ultimoDiaDelMes)).format('DD/MM/YYYY'));
            modalAgregarProceso.modal('show');
        });

        btnRegistrarProceso.on('click', function () {
            if (validarRegProceso()) {
                CRUDProceso(crearObjetoProceso(null, false));
            }else{
                Alert2Error("Se requiere ingresar todos los datos");
            }
        });

        btnActualizarProceso.on('click', function () {
            if (validarRegProceso()) {
                CRUDProceso(crearObjetoProceso($(this).data('id'), false));
            }else{
                Alert2Error("Se requiere ingresar todos los datos");
            }
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function(e) {
            e.target // newly activated tab
            e.relatedTarget // previous active tab
            
            dtTblProcesos.columns.adjust();
        });

        modalAgregarProceso.on('hide.bs.modal', function () {
            /*txtProceso.val('');
            txtFechaInicioProceso.val('');
            txtFechaFinProceso.val('');
            modalAgregarProceso.find('.errorClass').removeClass('errorClass');*/
        });

        modalEvaluacionesPorProceso.on('shown.bs.modal', function() {
            dtTblEvaluacionesPorProceso.columns.adjust();
        });

        modalEvaluacionesPorProceso.on('hidden.bs.modal', function() {
            btnCancelarRegistroEvaluacion.trigger('click');
        });

        function init() {
            AutoCompleteOff();
            setTodosLosProcesos();
            agregarEmpleado.click(function (e) {
                AbrirModalEmpleado();
            });
            buscarEmpleado.click(CargarTblEmpleados);
            guardarEmpleado.click(GuardarEmpleado);
            txtEmpleado.getAutocomplete(SelectEmpleado, null, '/Desempeno/getEmpleados');
            txtJefe.getAutocomplete(SelectEmpleadoDesempeno, null, '/Desempeno/getEmpleadosDesempeno');

            /*$.fn.datetimepicker.dates['es'] = {
                days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"],
                daysShort: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sab", "Dom"],
                daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa", "Do"],
                months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dec"],
                today: "Hoy"
            };*/

            txtFechaInicioProceso.datetimepicker({
                format: 'dd/mm/yyyy', 
                minView: 2, 
                maxView: 4,
                autoclose: true,
                language: 'es'
            });

            txtFechaFinProceso.datetimepicker({
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true,
                language: 'es'
            });

            txtFechaInicioEvaluacion.datetimepicker({
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true,
                language: 'es'
            });

            txtFechaFinEvaluacion.datetimepicker({
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true,
                language: 'es'
            });

            //txtFechaInicioProceso.datepicker().datepicker('setDate', new Date()); //TODO
            //txtFechaFinProceso.datepicker().datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
            //txtFechaInicioEvaluacion.datepicker().datepicker('setDate', new Date());
            //txtFechaFinEvaluacion.datepicker().datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));

            initTblProcesos();
            initTblEvaluacionesPorProcesos();
            obtenerTodosLosProcesos();
        }

        function initTblEmpleados() {
            dttblEmpleados = tblEmpleados.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'id', title: 'empleadoID', visible: false },
                    { data: 'contador', title: '#', visible: false },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'jefe', title: 'Jefe Inmediato' },
                    {
                        data: null,
                        title: 'Procesos',
                        render: function(data, type, row) {
                            let selectProcesos = '<select class="form-control cboProcesosEmpleado" value=""></select>'
                            return selectProcesos;
                        }
                    },
                    {
                        data: null,
                        title: 'Opciones',
                        render: function(data, type, row) {
                            var htmlEditar = '<button data-index="' + row.id + '" title="Editar empleado" class="btn btn-warning btn-sm editar"><i class="fa fa-edit" aria-hidden="true"></i></i></button>';
                            var htmlDesactivar = '<button data-index="' + row.id + '" title="Desactivar empleado" class="btn btn-danger btn-sm desactivar"><i class="fas fa-toggle-off" aria-hidden="true"></i></button>';
                            var htmlActivar = '<button data-index="' + row.id + '" class="btn btn-success btn-sm editar"><i class="fas fa-toggle-on" aria-hidden="true"></i></button>';
                            var htmlVerComo = '<button data-index="' + row.id + '" class="btn btn-primary btn-sm btnVerComo"><i class="far fa-eye"></i></button>';
                            return (row.estatus ? htmlEditar : ' ') + ' ' + (!row.estatus ? htmlActivar : htmlDesactivar) + ' ' + htmlVerComo;
                        }
                    },
                    { data: 'tipo', visible: false }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [1, 5, ] }
                    //{ "width": "20%", "targets": 0 }
                ],
                order: [[0, 'asc'], [1, 'asc']],
                createdRow: function(row, data, dataIndex) {
                    $(row).find('.cboProcesosEmpleado').attr('multiple', true);
                    $(row).find('.cboProcesosEmpleado').fillComboItems(todosLosProcesos, true, null);

                    $(row).find('.cboProcesosEmpleado').val(data.procesos);
                    convertToMultiselectSelectAll($(row).find('.cboProcesosEmpleado'));
                },
                drawCallback: function () {
                    tblEmpleados.find('button.desactivar').click(function (e) {
                        let dataRow = dttblEmpleados.row($(this).closest('tr')).data();
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                        let tipo = dataRow.tipo;
                        let cantEmpleadosTipo1 = 0;
                        let arrayEmpleados = dttblEmpleados.data().toArray();
                        arrayEmpleados.forEach(item => {
                            if (item.tipo == 1){
                                cantEmpleadosTipo1++;
                            }
                        });

                        if (cantEmpleadosTipo1 == 1 && tipo == 1){
                            Alert2Warning("No se puede desactivar al único administrador.");
                        }else{
                            Alert2AccionConfirmar("¡Cuidado!",
                                "¿Desea desactivar al empleado: " + dataRow.nombre + "?",
                                "Confirmar",
                                "Cancelar",
                                () => EliminarEmpleado(dataRow.id));
                        }
                    });

                    tblEmpleados.find('button.editar').click(function (e) {
                        let dataRow = dttblEmpleados.row($(this).closest('tr')).data();
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        modalAltaEmpleado.find('.modal-header .titleHeader').html('Editar empleado');
                        guardarEmpleado.removeAttr("data-accion");
                        txtEmpleado.val(dataRow.nombre);
                        txtJefe.val(dataRow.jefe);
                        txtEmpleado.attr("data-index", dataRow.idEmpleado);
                        txtJefe.attr("data-index", dataRow.idJefe);
                        cbTipo.val(dataRow.tipo);
                        txtEmpleado.prop('disabled', true);
                        modalAltaEmpleado.modal('show');
                    });
                },
                initComplete: function(settings, json) {
                    tblEmpleados.on('click', '.btnVerComo', function() {
                        let dataRow = dttblEmpleados.row($(this).closest('tr')).data();

                        /*AlertaAceptarRechazarNormal(
                            'Confirmación',
                            'Ingresará al sistema como el usuario: <b>' + dataRow.nombre + '</b> ¿Desea continuar?',
                            () => VerComo(dataRow.id)
                        );*/

                        Swal.fire({
                            position: "center",
                            icon: "info",
                            title: "¡Cuidado!",
                            width: '35%',
                            showCancelButton: true,
                            html: "<h3>Ingresará al sistema como el usuario: " + dataRow.nombre + " <br>¿Desea continuar?</h3>",
                            confirmButtonText: "Confirmar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#d9534f",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.value) {
                                VerComo(dataRow.id)
                            }
                        });

                    });

                    tblEmpleados.on('change', '.cboProcesosEmpleado', function() {
                        let dataRow = dttblEmpleados.row($(this).closest('tr')).data();
                        let procesosOriginales = new Array();
                        dataRow.procesos.forEach(function(value, index, array){
                            procesosOriginales.push(value.toString());
                        });
                        let procesosNuevos = $(this).val();
                        let difference = procesosOriginales
                            .filter(x => !procesosNuevos.includes(x))
                            .concat(procesosNuevos.filter(x => !procesosOriginales.includes(x)));
                        if(difference.length > 0) {
                            ModificarProcesoEmpleado(difference, dataRow.id);
                        }
                        dataRow.procesos = procesosNuevos;
                    });
                }
            });

            CargarTblEmpleados();
        }

        function initTblProcesos() {
            dtTblProcesos = tblProcesos.DataTable({
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,

                columns: [
                    { data: null, title: '#' },
                    { data: 'Proceso', title: 'Nombre' },
                    { data: 'FechaInicio', title: 'Fecha de inicio' },
                    { data: 'FechaFin', title: 'Fecha de finalización' },
                    { data: null, title: 'Opciones' }
                ],

                columnDefs: [
                    {
                        targets: [0, 2, 3, 4],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [2, 3],
                        render: function(data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [4],
                        render: function(data, type, row) {
                            let evaluaciones = '' +
                                '<button class="btn btn-success btnEvaluaciones">' +
                                    '<i class="far fa-calendar-check"></i>' +
                                '</button>';
                            let editar = '' +
                                '<button class="btn btn-warning btnEditarProceso">' +
                                    '<i class="far fa-edit"></i>' +
                                '</button>';
                            let eliminar = '' +
                                '<button class="btn btn-danger btnEliminarProceso">' +
                                    '<i class="far fa-trash-alt"></i>'
                                '</button>';
                            return evaluaciones + ' ' + editar + ' ' + eliminar;
                        }
                    }
                ],

                createdRow: function(row, data, dataIndex) {
                    $(row).find('td').eq(0).text(++dataIndex);
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');
                },

                initComplete: function(settings, json) {
                    tblProcesos.on('click', '.btnEditarProceso', function () {
                        let dataRow = dtTblProcesos.row($(this).closest('tr')).data();

                        modalAgregarProceso.find('.modal-title').text('Editar proceso');
                        btnActualizarProceso.data('id', dataRow.IdProceso);
                        btnActualizarProceso.show();
                        btnRegistrarProceso.hide();
                        
                        crearVistaEdicionProceso(dataRow);

                        modalAgregarProceso.modal('show');
                    });

                    tblProcesos.on('click', '.btnEliminarProceso', function () { //ELIMINAR PROCESO
                        let dataRow = dtTblProcesos.row($(this).closest('tr')).data();
                        /*AlertaAceptarRechazarNormal(
                            'Confirmación',
                            '¿Desea eliminar el proceso: <b>' + dataRow.Proceso + '</b>?',
                            () => CRUDProceso(crearObjetoProceso(dataRow.IdProceso, true))
                        );*/

                        Alert2AccionConfirmar("¡Cuidado!", 
                                                "¿Desea eliminar el proceso: " + dataRow.Proceso + "?", 
                                                "Confirmar", 
                                                "Cancelar", 
                                                () => CRUDProceso(crearObjetoProceso(dataRow.IdProceso, true))
                        );
                    });

                    tblProcesos.on('click', '.btnEvaluaciones', function() {
                        let dataRow = dtTblProcesos.row($(this).closest('tr')).data();

                        modalEvaluacionesPorProceso.find('.modal-title').text('Proceso: ' + dataRow.Proceso);
                        getEvaluacionesPorProceso(dataRow.IdProceso);
                    });
                }
            });
        }

        function initTblEvaluacionesPorProcesos() {
            dtTblEvaluacionesPorProceso = tblEvaluacionesPorProceso.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,

                columns: [
                    { data: null, title: '#' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'FechaInicio', title: 'Fecha de inicio' },
                    { data: 'FechaFin', title: 'Fecha de finalización' },
                    { data: null, title: 'Opciones' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-body-center'
                    },
                    {
                        targets: [2, 3],
                        render: function(data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [4],
                        render: function(data, type, row) {
                            let editar = '' +
                                '<button class="btn btn-warning btnEditarEvaluacion">' + 
                                    '<i class="far fa-edit"></i>' +
                                '</button>';
                            let eliminar = '' +
                                '<button class="btn btn-danger btnEliminarEvaluacion">' +
                                    '<i class="far fa-trash-alt"></i>'
                                '</button>';
                            return editar + ' ' + eliminar;
                        }
                    }
                ],

                createdRow: function(row, data, dataIndex) {
                    $(row).find('td').eq(0).text(++dataIndex);
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).find('th').addClass('text-center');
                },

                initComplete: function(settings, json) {
                    tblEvaluacionesPorProceso.on('click', '.btnEditarEvaluacion', function () {
                        let dataRow = dtTblEvaluacionesPorProceso.row($(this).closest('tr')).data();

                        if(!divAgregarEvaluacion.hasClass('in')) {
                            btnAgregarEvaluacionPorProceso.trigger('click');
                        }

                        txtNombreEvaluacion.val(dataRow.Descripcion);
                        txtFechaInicioEvaluacion.val(moment(dataRow.FechaInicio).format('DD/MM/YYYY'));
                        txtFechaFinEvaluacion.val(moment(dataRow.FechaFin).format('DD/MM/YYYY'));

                        btnGuardarEvaluacion.data('evaluacionid', dataRow.Id);
                    });

                    tblEvaluacionesPorProceso.on('click', '.btnEliminarEvaluacion', function () { //ELIMINAR EVALUACION
                        let dataRow = dtTblEvaluacionesPorProceso.row($(this).closest('tr')).data();
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: '¿Desea eliminar la evaluación:&nbsp;<b>' + dataRow.Descripcion + '</b>?',
                            customClass: 'swal-wide',
                            confirmButtonText: "Confirmar",
                            showCancelButton: true,
                            cancelButtonText: "Cancelar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonColor: "#d9534f",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.value) {
                                CUDEvaluacion(crearObjetoEvaluacion(dataRow.Id, dataRow.ProcesoId, true))
                            }
                        });
                    });
                }
            });
        }

        function CargarTblEmpleados() {
            $.ajax({
                url: '/Desempeno/CargarTblEmpleados',
                datatype: "json",
                type: "POST",
                data: { estatus: cbEstatus.val() == '1' },
                success: function (response) {
                    if (response.success) {
                        let cantEmpleados = 0;
                        let cantEmpleadosTipo1 = 0; //NIVEL ADMINISTRADOR
                        response.lst.length > 0 ? cantEmpleados = response.lst.length : cantEmpleados = 0;
                        if (cantEmpleados > 0)
                        {
                            for (let i = 0; i < response.lst.length; i++) {
                                if (response.lst[i].tipo == 1){
                                    cantEmpleadosTipo1++;
                                }
                            }
                        }

                        dttblEmpleados.clear();
                        dttblEmpleados.rows.add(response.lst);
                        dttblEmpleados.draw(false);
                        dttblEmpleados.columns.adjust();
                    }
                }
            });
        }

        function AbrirModalEmpleado() {
            LimpiarModalEmpleado();
            modalAltaEmpleado.find('.modal-header .titleHeader').html('Alta de empleado');
            txtEmpleado.prop('disabled', false);
            modalAltaEmpleado.modal("show");
            guardarEmpleado.attr("data-accion", "nuevoEmpleado");
            txtEmpleado.removeAttr("data-index");
        }

        function LimpiarModalEmpleado() {
            txtEmpleado.val("");
            txtJefe.val("");
            cbTipo.val("3");
        }

        function SelectEmpleado(event, ui) {
            $("#txtEmpleado").text(ui.item.label);
            $("#txtEmpleado").attr("data-index", ui.item.id);
        }
        function SelectEmpleadoDesempeno(event, ui) {
            $("#txtJefe").text(ui.item.label);
            $("#txtJefe").attr("data-index", ui.item.id);
        }

        function ValidarEmpleado()
        {
            let estado = true;
            if ($("#txtEmpleado").attr("data-index") == null || $("#txtEmpleado").attr("data-index") == "") estado = false;
            if(txtJefe.val() == ''){ $('#txtJefe').attr('data-index', ''); }
            return estado;
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function validarRegProceso() {
            let valido = true;

            if (!validarCampo(txtProceso)) { valido = false; }
            if (!validarCampo(txtFechaInicioProceso)) { valido = false; }
            if (!validarCampo(txtFechaFinProceso)) { valido = false; }

            return valido;
        }

        function validarRegEvaluacion() {
            let valido = true;

            if(!validarCampo(txtNombreEvaluacion)) { valido = false; }
            if(!validarCampo(txtFechaInicioEvaluacion)) { valido = false; }
            if(!validarCampo(txtFechaFinEvaluacion)) { valido = false; }

            return valido;
        }

        function crearVistaEdicionProceso(proceso) {
            txtProceso.val(proceso.Proceso);
            txtFechaInicioProceso.val(moment(proceso.FechaInicio).format('DD/MM/YYYY'));
            txtFechaFinProceso.val(moment(proceso.FechaFin).format('DD/MM/YYYY'));
        }

        function crearObjetoProceso(idProceso, esEliminacion) {
            let objProceso = {
                IdProceso: idProceso,
                Proceso: txtProceso.val(),
                FechaInicio: moment(txtFechaInicioProceso.val(), 'DD/MM/YYYY').toISOString(true),
                FechaFin: moment(txtFechaFinProceso.val(), 'DD/MM/YYYY').toISOString(true),
                EsEliminacion: esEliminacion
            }
            return objProceso;
        }

        function crearObjetoEvaluacion(idEvaluacion, idProceso, esEliminacion) {
            let objEvaluacion = {
                Id: idEvaluacion,
                ProcesoId: idProceso,
                Descripcion: txtNombreEvaluacion.val(),
                FechaInicio: moment(txtFechaInicioEvaluacion.val(), 'DD/MM/YYYY').toISOString(true),
                FechaFin: moment(txtFechaFinEvaluacion.val(), 'DD/MM/YYYY').toISOString(true),
                EsEliminacion: esEliminacion,
            }
            return objEvaluacion;
        }

        function GuardarEmpleado() {
            if (ValidarEmpleado()) {
                let empleadoID = $("#txtEmpleado").attr("data-index");
                let jefeID = $("#txtJefe").attr("data-index");
                let tipo = $("#cbTipo").val();

                //console.log("")

                $.ajax({
                    url: '/Desempeno/GuardarEmpleado',
                    datatype: "json",
                    type: "POST",
                    data: { empleadoID: empleadoID, jefeID: jefeID, tipo: tipo },
                    success: function (response) {
                        if (response.success) {
                            if (response.exito) {
                                let strMensaje = "";
                                let accion = guardarEmpleado.attr("data-accion");
                                accion == "nuevoEmpleado" ? 
                                    strMensaje = "Se registró correctamente el usuario" : 
                                    strMensaje = "Se actualizó correctamente el usuario";
                                //console.log("accion: " + accion);
                                Alert2Exito(strMensaje);

                                $("#modalAltaEmpleado").modal("hide");
                                if(response.recargar) {
                                    window.location.href = "/Administrativo/Desempeno/SalirVerComo";
                                }
                                setTodosLosProcesos();
                            }
                            else {
                                Alert2Error(response.Message);
                            }
                        }
                        else {
                            Alert2Error("Ocurrió un error al intentar guardar el empleado");
                        }
                    },
                    error: function (error) {
                        // $.unblockUI
                        // Error al lanzar la petición.
                        //AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
                        let strMensaje = "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".";
                        Alert2Error(strMensaje);
                    }
                });
            }
            else {
                Alert2Error("Se requiere ingresar nombre del empleado");
            }
        }

        function VerComo(idUsuario) {
            $.get('/Desempeno/VerComo', {
                idUsuario: idUsuario
            }).always().then(response => {
                if(response.Success) {
                    location.reload();
                }
                else {
                    Alert2Error(response.Message);
                    //AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                Alert2Error("Error");
                //AlertaGeneral('Error', null);
            });
        }

        setTodosLosProcesos = async () => {
            try {
                response = await ejectFetchJson('/Desempeno/getCboTodosLosProcesos');
                if(response.success) {
                    todosLosProcesos = response.items;
                    initTblEmpleados();
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message); 
                Alert2Error(o_O.message);
            }
        }

        function ModificarProcesoEmpleado(idProceso, idEmpleado) {
            $.post('/Desempeno/ModificarProcesoEmpleado', {
                idProceso: idProceso,
                idEmpleado: idEmpleado
            }).always().then(response => {
                if(response.Success) {

                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }

        function EliminarEmpleado(idEmpleado) {
            $.post('/Desempeno/EliminarEmpleado', {
                idEmpleado: idEmpleado
            }).always().then(response => {
                if(response.Success) {
                    //ConfirmacionGeneral('Confirmación', '!Se eliminó correctamente el empleado!');
                    Alert2Exito("¡Se eliminó correctamente el empleado!") 
                    if (response.Message == "recargar") {
                        window.location.href = "/Administrativo/Desempeno/SalirVerComo";
                    }
                    setTodosLosProcesos();
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }

        function obtenerTodosLosProcesos() {
            $.get('/Desempeno/ObtenerTodosLosProcesos').always().then(response => {
                if(response.Success) {
                    addRows(tblProcesos, response.Value);
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }

        function CRUDProceso(objProceso) {
            $.post('/Desempeno/CRUDProceso', {
                objProceso: objProceso
            }).always().then(response => {
                if (response.Success) {
                    let strMensaje = "";
                    let = mensaje = objProceso.IdProceso == null ? 'registró' : objProceso.EsEliminacion ? 'eliminó' : 'actualizó';
                    modalAgregarProceso.modal('hide');
                    obtenerTodosLosProcesos();
                    setTodosLosProcesos();

                    strMensaje = "¡Se " + mensaje + " correctamente el proceso!";
                    Alert2Exito(strMensaje);
                }
                else {
                    Alert2Error(response.Message);
                }
            }, error => {
                Alert2Error("Error");
            });
        }

        function getEvaluacionesPorProceso(idProceso) {
            $.get('/Desempeno/GetEvaluacionesPorProceso', {
                idProceso: idProceso
            }).always().then(response => {
                if(response.Success) {
                    addRows(tblEvaluacionesPorProceso, response.Value);
                    btnGuardarEvaluacion.data('procesoid', idProceso);
                    modalEvaluacionesPorProceso.modal('show');
                }
                else {
                    Alert2Error(response.Message);
                }
            }, error => {
                Alert2Error("Error");
            });
        }

        function CUDEvaluacion(objEvaluacion) {
            $.post('/Desempeno/CUDEvaluacion', {
                objEvaluacion: objEvaluacion
            }).always().then(response => {
                if(response.Success) {
                    let strMensaje = "";
                    if (objEvaluacion.EsEliminacion){
                        strMensaje = "¡Se eliminó correctamente la evaluación!";
                    }else{
                        btnGuardarEvaluacion.data('evaluacionid') > 0 ? 
                            strMensaje = "¡Se actualizó correctamente la evaluación!" :
                            strMensaje = "¡Se registró correctamente la evaluación!";
                    }
                    Alert2Exito(strMensaje);

                    txtNombreEvaluacion.val('');
                    txtFechaInicioEvaluacion.val('');
                    txtFechaFinEvaluacion.val('');
                    btnGuardarEvaluacion.data('evaluacionid', '');
                    getEvaluacionesPorProceso(objEvaluacion.ProcesoId);
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }

        function AutoCompleteOff(){
            txtProceso.attr("autocomplete", "off");
            txtFechaInicioProceso.attr("autocomplete", "off");
            txtFechaFinProceso.attr("autocomplete", "off");
            txtNombreEvaluacion.attr("autocomplete", "off");
            txtFechaInicioEvaluacion.attr("autocomplete", "off");
            txtFechaFinEvaluacion.attr("autocomplete", "off");
        } 

        init();
    };

    $(document).ready(function () {
        RecursosHumanos.Desempeno._Empleados = new _Empleados();
    })
    .ajaxStart(() => { /*$.blockUI({ message: 'Procesando...' });*/ })
    .ajaxStop(() => { $.unblockUI(); });
})();