(() => {
    $.namespace('AdministracionInstructores.Promedio');

    Promedio = function () {
        _privilegioUsuario = 0;

        const listaProximosEventos = $('#listaProximosEventos');
        const cboAno = $('#cboAno');
        const divCalendario = document.getElementById('divCalendario');
        const btnBuscar = $('#btnBuscar');
        const divCalendarioEnero = document.getElementById('divCalendarioEnero');
        const divCalendarioFebrero = document.getElementById('divCalendarioFebrero');
        const divCalendarioMarzo = document.getElementById('divCalendarioMarzo');
        const divCalendarioAbril = document.getElementById('divCalendarioAbril');
        const divCalendarioMayo = document.getElementById('divCalendarioMayo');
        const divCalendarioJunio = document.getElementById('divCalendarioJunio');
        const divCalendarioJulio = document.getElementById('divCalendarioJulio');
        const divCalendarioAgosto = document.getElementById('divCalendarioAgosto');
        const divCalendarioSeptiembre = document.getElementById('divCalendarioSeptiembre');
        const divCalendarioOctubre = document.getElementById('divCalendarioOctubre');
        const divCalendarioNoviembre = document.getElementById('divCalendarioNoviembre');
        const divCalendarioDiciembre = document.getElementById('divCalendarioDiciembre');
        const cboProyecto = $('#cboProyecto');

        let fechaActualServidor = new Date;
        let fechaActualServidorOpera = new Date;
        let fechaActualr = new Date;
        let ArrAnos = [];
        let fechaActualServidorEnero = '2021-01-01';
        let fechaActualServidorFebrero = '2021-02-01';
        let fechaActualServidorMarzo = '2021-03-01';
        let fechaActualServidorAbril = '2021-04-01';
        let fechaActualServidorMayo = '2021-05-01';
        let fechaActualServidorJunio = '2021-06-01';
        let fechaActualServidorJulio = '2021-07-01';
        let fechaActualServidorAgosto = '2021-08-01';
        let fechaActualServidorSeptiembre = '2021-09-01';
        let fechaActualServidorOctubre = '2021-10-01';
        let fechaActualServidorNoviembre = '2021-11-01';
        let fechaActualServidorDiciembre = '2021-12-01';

        let calendar;
        let calendar2;
        let calendar3;
        let calendar4;
        let calendar5;
        let calendar6;
        let calendar7;
        let calendar8;
        let calendar9;
        let calendar10;
        let calendar11;
        let calendar12;
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        const tablaEvaluadores = $('#tablaEvaluadores');
        let dtEvaluadores;
        const selectGrupo = $('#selectGrupo');
        const inputFechaInicioRol = $('#inputFechaInicioRol');
        const selectCentroCosto = $('#selectCentroCosto');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const cboInstructor = $('#cboInstructor');
        let datos = {
            fechaInicio: fechaActual
        }
        const cboTematica = $('#cboTematica')
        function initCalendarioEnero(lstDatos) {
            if (calendar != null) {
                calendar.destroy();
                calendar2.destroy();
                calendar3.destroy();
                calendar4.destroy();
                calendar5.destroy();
                calendar6.destroy();
                calendar7.destroy();
                calendar8.destroy();
                calendar9.destroy();
                calendar10.destroy();
                calendar11.destroy();
                calendar12.destroy();
            }
            calendar = new FullCalendar.Calendar(divCalendarioEnero, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorEnero,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true


            });
            calendar.render();
            calendar2 = new FullCalendar.Calendar(divCalendarioFebrero, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorFebrero,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar2.render();
            calendar3 = new FullCalendar.Calendar(divCalendarioMarzo, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorMarzo,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true
            });
            calendar3.render();
            calendar4 = new FullCalendar.Calendar(divCalendarioAbril, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorAbril,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true
            });
            calendar4.render();
            calendar5 = new FullCalendar.Calendar(divCalendarioMayo, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorMayo,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar5.render();
            calendar6 = new FullCalendar.Calendar(divCalendarioJunio, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorJunio,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar6.render();
            calendar7 = new FullCalendar.Calendar(divCalendarioJulio, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorJulio,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar7.render();
            calendar8 = new FullCalendar.Calendar(divCalendarioAgosto, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },

                locale: 'es',
                defaultDate: fechaActualServidorAgosto,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar8.render();
            calendar9 = new FullCalendar.Calendar(divCalendarioSeptiembre, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorSeptiembre,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar9.render();
            calendar10 = new FullCalendar.Calendar(divCalendarioOctubre, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorOctubre,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar10.render();
            calendar11 = new FullCalendar.Calendar(divCalendarioNoviembre, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorNoviembre,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar11.render();
            calendar12 = new FullCalendar.Calendar(divCalendarioDiciembre, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'MMMM YYYY',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                locale: 'es',
                defaultDate: fechaActualServidorDiciembre,
                height: 400,
                events: lstDatos,
                showNonCurrentDates: false,
                allDayDefault: true

            });
            calendar12.render();
            $('.fc-button').remove();
            $('.fc-day-header').css('background-color', '#ec971f');


        }
        function getFechaActual() {


            fechaActualServidorEnero = cboAno.val() + '-01-01';
            fechaActualServidorFebrero = cboAno.val() + '-02-01';
            fechaActualServidorMarzo = cboAno.val() + '-03-01';
            fechaActualServidorAbril = cboAno.val() + '-04-01';
            fechaActualServidorMayo = cboAno.val() + '-05-01';
            fechaActualServidorJunio = cboAno.val() + '-06-01';
            fechaActualServidorJulio = cboAno.val() + '-07-01';
            fechaActualServidorAgosto = cboAno.val() + '-08-01';
            fechaActualServidorSeptiembre = cboAno.val() + '-09-01';
            fechaActualServidorOctubre = cboAno.val() + '-10-01';
            fechaActualServidorNoviembre = cboAno.val() + '-11-01';
            fechaActualServidorDiciembre = cboAno.val() + '-12-01';


            initCalendarioEnero();
        }
        function IniciarProcesoFechas() {
            fechaActualServidorOpera = moment(fechaActualServidor).format('YYYY');
            fechaActualServidorOpera = fechaActualServidorOpera - 10;

            for (let index = 0; index < 10; index++) {
                fechaActualServidorOpera++;
                let element = { ano: fechaActualServidorOpera };
                ArrAnos.push(element);
            }
            for (let index = 0; index < 10; index++) {
                fechaActualServidorOpera++;
                let element = { ano: fechaActualServidorOpera };
                ArrAnos.push(element);
            }



            let groupOption = ``;
            ArrAnos.forEach(y => {
                groupOption += `<option value="${y.ano}">${y.ano}</option>`;
            });
            cboAno.append(groupOption);
            fechaActualr = moment(fechaActualr).format('YYYY');
            cboAno.val(fechaActualr)
        }
        function initTablaEvaluadores() {
            dtEvaluadores = tablaEvaluadores.DataTable({
                retrieve: true,
                paging: true,
                searching: true,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                order: [],
                columns: [
                    { data: 'cveEmpleado', title: 'Clave Empleado' },
                    {
                        title: 'Nombre',
                        render: function (data, type, row) {
                            let div = row.nombreCompleto + ' ' + row.ApeidoP + ' ' + row.ApeidoM;

                            return div;
                        }
                    },
                    { data: 'nombreGrupo', title: 'Grupo' },
                    {
                        data: 'fechaInicio', title: 'Fecha',
                        render: function (data, type, row, meta) {
                            let div = moment(data).format("YYYY-MM-D");

                            return div;
                        },

                    },
                    {
                        render: function (data, type, row) {
                            let btnEliminar = "";

                            btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarInstructor' data-id="${row.id}">` +
                                `<i class="fas fa-toggle-on"></i></button>`;


                            return `<button class='btn-editar btn btn-warning editarInstructor' data-nombre="${row.nombreCompleto}" data-apeidoP="${row.ApeidoP}" data-apeidoM="${row.ApeidoM}" data-tematica="${row.tematica}" data-instructor="${row.instructor}" data-grupo="${row.grupo}" data-fecha="${row.fechaInicio}" data-cveEmpleado="${row.cveEmpleado}" data-id="${row.id}">` +
                                `<i class='fas fa-pencil-alt'></i>` +
                                `</button>&nbsp;` + btnEliminar;
                        }
                    }

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                initComplete: function (settings, json) {
                    tablaEvaluadores.on("click", ".eliminarInstructor", function () {
                        let strMensaje = "¿Desea eliminar el registro seleccionado?";

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "¡Cuidado!",
                            width: '35%',
                            showCancelButton: true,
                            html: `<h3>${strMensaje}</h3>`,
                            confirmButtonText: "Confirmar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#d9534f",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.value) {
                                fncEliminar($(this).attr("data-id"));
                            }
                        });
                    });

                    tablaEvaluadores.on("click", ".editarInstructor", function (e) {
                        $('#modalAltaAgregarEditar').modal('show');
                        $('#inputClaveEmpleado').attr('data-id', $(this).attr("data-id"));
                        $('#inputClaveEmpleado').val($(this).attr("data-cveEmpleado"));
                        cargarEmpleadoPorClave();

                        selectGrupo.val($(this).attr("data-grupo"));
                        inputFechaInicioRol.val(moment($(this).attr("data-fecha")).format("YYYY-MM-D"));
                        $('#checkBoxEvaluador').prop('checked', $(this).attr("data-instructor"))
                        $('#cboTematica').val($(this).attr("data-tematica"));
                        $('#inputClaveEmpleado').prop('disabled', true)

                    });
                }
            });
            if ($('.dataTables_length') != undefined) {
                $('.dataTables_length').remove();
            }
        }
        function fncEliminar(id) {
            let datos = { id: id };
            $.ajax({
                datatype: "json",
                type: "GET",
                url: "/Administrativo/CapacitacionSeguridad/EliminarInstructor",
                data: datos,
                success: function (response) {
                    if (!response.success) {
                        Alert2Error(response.message);
                    }
                    else {
                        let strMensaje = "";
                        strMensaje = "Se ha eliminado con éxito el registro.";
                        Alert2Exito(strMensaje);
                        cargarEmpleados();
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                }
            });
        }
        function cargarEmpleados() {
            axios.post('/Administrativo/CapacitacionSeguridad/GetInstructores')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaEvaluadores, response.data.items.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.draw();
            dt.rows.add(lst).draw(false);
        }

        function getEvaluadoresAplica() {
            let evaluadores = [];

            tablaEvaluadores.find('tbody tr').each(function (index, row) {
                let checkbox = $(row).find('.checkBoxAplica');

                if (checkbox.prop('checked')) {
                    let rowData = dtEvaluadores.row(row).data();

                    evaluadores.push({
                        id: rowData.id,
                        claveEmpleado: rowData.claveEmpleado,
                        nombre: rowData.nombre,
                        apellidoPaterno: rowData.apellidoPaterno,
                        apellidoMaterno: rowData.apellidoMaterno,
                        puestoEvaluacionID: rowData.puestoEvaluacionID,
                        evaluador: rowData.evaluador,
                        estatus: rowData.estatus,
                        aplica: true
                    });
                }
            });

            return evaluadores;
        }

        function cargarEmpleadoPorClave() {
            let cveEmpleado = +($('#inputClaveEmpleado').val());

            if (cveEmpleado > 0) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/CapacitacionSeguridad/ObtenerCCUnico', { cveEmpleado })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.items.status == 2) {
                                AlertaGeneral('Alerta', response.items.messaje);
                                limpiar();
                            } else {
                                $('#inputNombre').val(response.items.nombreCompleto);
                                $('#inputApellidoPaterno').val(response.items.ApeidoP);
                                $('#inputApellidoMaterno').val(response.items.ApeidoM);
                                $('#inputCC').val(response.items.cc + " " + response.items.descripcion);
                                $('#inputCC').attr('data-empresa', response.items.empresa);
                                $('#inputCC').attr('disabled', true);
                            }


                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function limpiar() {
            $('#inputClaveEmpleado').val('');
            $('#inputNombre').val('');
            $('#inputApellidoPaterno').val('');
            $('#inputApellidoMaterno').val('');
            $('#inputCC').val('');
        }
        function GuardarCveEmpleado() {
            if ($('#inputClaveEmpleado').val() == "") {
                AlertaGeneral('Alerta', 'Asignar una clave de empleado');
                return;
            } else {
                let parametros = GetParametros();


                let AddEdit = $('#inputClaveEmpleado').attr('data-id') != 0 ? true : false;
                axios.post('/Administrativo/CapacitacionSeguridad/PostGuardarInstructor', { parametros: parametros, AddEdit: AddEdit })
                    .catch(o_O => AlertaGeneral(o_O.message))
                    .then(response => {
                        let result = response.data.items.data;
                        if (result.status == 2) {
                            AlertaGeneral('Alerta', result.messaje)
                        } else if (result.status == 1) {
                            cargarEmpleados();
                            limpiar();
                            $('#modalAltaAgregarEditar').modal('hide');
                        } else {
                            AlertaGeneral('Alerta', result.messaje)
                        }
                    });
            }
        }
        function GetParametros() {
            let parametros = {};
            parametros = {
                id: $('#inputClaveEmpleado').attr('data-id'),
                cveEmpleado: $('#inputClaveEmpleado').val(),
                grupo: selectGrupo.val(),
                fechaInicio: inputFechaInicioRol.val(),
                instructor: $('#checkBoxEvaluador').prop('checked'),
                tematica: $('#cboTematica').val(),
                empresa: $('#inputCC').attr('data-empresa')
            };
            return parametros;
        }


        function getFechaInicio(cveEmpleado) {
            axios.post('/Administrativo/CapacitacionSeguridad/getFechaInicio', { cveEmpleado: cveEmpleado })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let result = response.data.items;
                    let ArrDatos = llenarAno(result);

                    initCalendarioEnero(ArrDatos);
                });
        }

        function llenarAno(result) {
            let calendario = [];
            let fechaInicioRol = new Date(moment(result.fechaInicio).format("YYYY-MM-D"));
            let FechaFutura = new Date('2031-12-31');
            let total = FechaFutura - fechaInicioRol;

            total = parseInt(total / (1000 * 60 * 60 * 24));

            if (!result.mixto) {
                let cantDiasTrabajados = result.cantDiasTrabajados;
                let cantDiasDescansados = result.cantDiasDescansados;
                let contadorDiasGrupo = 0;

                //#region Array de grupo normal
                let grupoNormal = [];

                grupoNormal = grupoNormal.concat(Array(cantDiasTrabajados).fill({ diaLaboral: true }));
                grupoNormal = grupoNormal.concat(Array(cantDiasDescansados).fill({ diaLaboral: false }));
                //#endregion

                for (let i = 0; i <= total; i++) {
                    if (grupoNormal[contadorDiasGrupo++].diaLaboral) {
                        calendario.push({ title: '°    ', start: fechaInicioRol.AddDays(i), color: '#257e4a', textColor: '#257e4a' });
                    } else {
                        calendario.push({ title: '°    ', start: fechaInicioRol.AddDays(i), color: '#EA461A', textColor: '#EA461A' });
                    }

                    if (contadorDiasGrupo == grupoNormal.length) {
                        contadorDiasGrupo = 0;
                    }
                }

                // let cantDiasTrabajados = result.cantDiasTrabajados;
                // let cantDiasDescansados = result.cantDiasDescansados;

                // for (let i = 0; i <= total; i++) {
                //     let datos = {};

                //     if (cantDiasTrabajados != 0) {
                //         datos = { title: '°    ', start: fechaInicioRol.AddDays(i), color: '#257e4a', textColor: '#257e4a' };

                //         cantDiasTrabajados--;

                //         if (cantDiasTrabajados < 0) {
                //             cantDiasTrabajados = 0
                //         }

                //         if (cantDiasTrabajados == 0) {
                //             cantDiasDescansados = result.cantDiasDescansados;
                //         }
                //     } else {
                //         datos = { title: '°    ', start: fechaInicioRol.AddDays(i), color: '#EA461A', textColor: '#EA461A' };

                //         cantDiasDescansados--;

                //         if (cantDiasDescansados < 0) {
                //             cantDiasDescansados = 0
                //         }

                //         if (cantDiasDescansados == 0) {
                //             cantDiasTrabajados = result.cantDiasTrabajados;
                //         }
                //     }

                //     calendario.push(datos);
                // }
            } else {
                let cantDiasTrabajados = result.cantDiasTrabajados;
                let cantDiasDescansados = result.cantDiasDescansados;
                let cantDiasTrabajados2 = result.cantDiasTrabajados2;
                let cantDiasDescansados2 = result.cantDiasDescansados2;
                let contadorDiasGrupo = 0;

                //#region Array de grupo mixto
                let grupoMixto = [];

                grupoMixto = grupoMixto.concat(Array(cantDiasTrabajados).fill({ diaLaboral: true }));
                grupoMixto = grupoMixto.concat(Array(cantDiasDescansados).fill({ diaLaboral: false }));
                grupoMixto = grupoMixto.concat(Array(cantDiasTrabajados2).fill({ diaLaboral: true }));
                grupoMixto = grupoMixto.concat(Array(cantDiasDescansados2).fill({ diaLaboral: false }));
                //#endregion

                for (let i = 0; i <= total; i++) {
                    if (grupoMixto[contadorDiasGrupo++].diaLaboral) {
                        calendario.push({ title: '°    ', start: fechaInicioRol.AddDays(i), color: '#257e4a', textColor: '#257e4a' });
                    } else {
                        calendario.push({ title: '°    ', start: fechaInicioRol.AddDays(i), color: '#EA461A', textColor: '#EA461A' });
                    }

                    if (contadorDiasGrupo == grupoMixto.length) {
                        contadorDiasGrupo = 0;
                    }
                }
            }

            return calendario;
        }

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;

                        if (response.data == 2 || response.data == 3 || response.data == 4) {
                            $('#btnAlta').attr('disabled', true);
                        }
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function diasEnUnMes(mes, año) {
            return new Date(año, mes, 0).getDate();
        }

        const CargarCombos = function () {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    cboProyecto.append('<option value="seleccione">--seleccione--</option>');
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        cboProyecto.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }


        let init = () => {
            revisarPrivilegio();
            CargarCombos();
            initTablaEvaluadores();
            IniciarProcesoFechas();
            getFechaActual();
            cboTematica.fillCombo('ComboGetEnumerables', null, false);
            btnBuscar.click(function () {
                if (cboInstructor.val() != '') {

                    getFechaInicio(cboInstructor.val());
                    getFechaActual();
                }
            });
            $('#btnAlta').click(function () {
                $('#modalAlta').modal('show');
                cargarEmpleados();
            })
            selectGrupo.fillCombo('/Administrativo/CapacitacionSeguridad/GetRolesCombo', null, false);
            inputFechaInicioRol.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaActual);
            selectCentroCosto.fillComboSeguridad(false);
            inputClaveEmpleado.change(function () {
                cargarEmpleadoPorClave();

            });
            $('#btnCancelar').click(function () {
                limpiar();
            });
            $('#btnGuardar').click(function () {
                GuardarCveEmpleado();
            });
            $('#btnCancelar').click(function () {
                $('#modalAltaAgregarEditar').modal('hide');
            });

            $('#btnGenerarModal').click(function () {
                $('#modalAltaAgregarEditar').modal('show');
                $('#inputClaveEmpleado').attr('data-id', 0);
                $('#inputClaveEmpleado').prop('disabled', false)

            });
            cboProyecto.change(function (e) {
                cboInstructor.fillCombo('/Administrativo/CapacitacionSeguridad/GetInstructoresCombo?cc=' + $('#cboProyecto').val() + '', null, false);
            });
        }

        init();
    }
    $(document).ready(() => {
        AdministracionInstructores.Promedio = new Promedio();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();