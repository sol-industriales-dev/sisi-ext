(() => {
    $.namespace('Barrenacion.ManoOBra');
    ManoOBra = function () {

        // Variables.
        const comboAC = $('#comboAC');
        const comboEstatus = $('#comboEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaEquipos = $('#tablaEquipos');
        let dtTablaEquipos;

        // Variables modal.
        const modalGestion = $('#modalGestion');
        const inputNoEconomico = $('#inputNoEconomico');
        const inputDescripcion = $('#inputDescripcion');
        const comboTurno = $('#comboTurno');

        const inputOperador = $('#inputOperador');
        const inputClaveOperador = $('#inputClaveOperador');

        const inputAyudante = $('#inputAyudante');
        const inputClaveAyudante = $('#inputClaveAyudante');

        const inputSueldoOperador = $("#inputSueldoOperador");
        const inputFSROperador = $("#inputFSROperador");
        const inputHorasJornadaOperador = $("#inputHorasJornadaOperador");

        const inputSueldoAyudante = $("#inputSueldoAyudante");
        const inputFSRAyudante = $("#inputFSRAyudante");
        const inputHorasJornadaAyudante = $("#inputHorasJornadaAyudante");

        const botonAsignarOperadores = $('#botonAsignarOperadores');

        const inputTotalAyudante = $("#inputTotalAyudante");
        const inputTotalOperador = $("#inputTotalOperador");

        //operadores
        const sumaTotalOperadores = $(".totalOperadores");
        const sumaTotalAyudantes = $(".totalAyudantes");

        // Lógica de inicialización.
        (function init() {

            initAutocompletes();
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false, null);
            agregarListeners();
            cargarEquipos();
        })();

        //Métodos.
        function initAutocompletes() {
            inputClaveOperador.getAutocompleteValid(setClaveEmpledoDesc, verificarClaveEmpleado, { porDesc: false }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
            inputOperador.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, { porDesc: true }, '/Barrenacion/ObtenerEmpleadosEnKontrol');

            inputClaveAyudante.getAutocompleteValid(setClaveEmpledoDesc, verificarClaveEmpleado, { porDesc: false }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
            inputAyudante.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, { porDesc: true }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
        }

        function agregarListeners() {
            botonBuscar.click(cargarEquipos);
            botonAsignarOperadores.click(GuardarOperadoresBarrenadora);
            modalGestion.on("hide.bs.modal", limpiarCamposModal);
            comboTurno.change(() => {
                const barrenadoraID = botonAsignarOperadores.data().barrenadoraID;
                const turno = comboTurno.val();
                obtenerOperadoresBarrenadora(barrenadoraID, turno);

            });

            sumaTotalOperadores.change(realizarSumaOperadores);
            sumaTotalAyudantes.change(realizarSumaAyudante);
        }

        function realizarSumaOperadores() {
            if (inputSueldoOperador.val() != 0 && inputHorasJornadaOperador.val() != 0 && inputFSRAyudante != 0) {
                let costoHora = (inputSueldoOperador.val() / inputHorasJornadaOperador.val());
                let sumaTotal = costoHora * (1 + (inputFSROperador.val() / 100));
                inputTotalOperador.val(sumaTotal.toFixed(2));
            }
        }

        function realizarSumaAyudante() {
            if (inputSueldoAyudante.val() != 0 && inputHorasJornadaAyudante.val() != 0 && inputFSRAyudante.val() != 0) {
                let costoHora = (inputSueldoAyudante.val() / inputHorasJornadaAyudante.val());
                let sumaTotal = costoHora * (1 + (inputFSRAyudante.val() / 100));
                inputTotalAyudante.val(sumaTotal.toFixed(2));
            }
        }

        function limpiarCamposModal() {
            comboTurno.val(1);
            inputClaveOperador.val('');
            inputOperador.val('');
            inputSueldoOperador.val('');
            inputHorasJornadaOperador.val('');
            inputFSROperador.val('');
            inputTotalOperador.val('');


            inputClaveAyudante.val('');
            inputAyudante.val('');
            inputSueldoAyudante.val('');
            inputHorasJornadaAyudante.val('');
            inputFSRAyudante.val('');
            inputTotalAyudante.val(' ');

        }

        function setClaveEmpledo(e, ui) {
            const row = $(this).closest('div.row');

            $(this).val(ui.item.value);
            row.find('.inputClave').val(ui.item.id);
            row.find('.inputSueldo').val(ui.item.sueldo);

            realizarSumaOperadores();
            realizarSumaAyudante();
        }

        function setClaveEmpledoDesc(e, ui) {
            const row = $(this).closest('div.row');

            $(this).val(ui.item.value)
            row.find('.inputDescripcion').val(ui.item.id);
            row.find('.inputSueldo').val(ui.item.sueldo);
            realizarSumaOperadores();
            realizarSumaAyudante();

        }

        function verificarClaveEmpleado(e, ui) {
            if (ui.item == null) {
                const row = $(this).closest('div.row');
                row.find('.inputClave').val('');
                row.find('.inputDescripcion').val('');
                row.find('.inputSueldo').val(ui.item.sueldo);

                realizarSumaOperadores();
                realizarSumaAyudante();
            }
        }

        function cargarTablaEquipos(data) {
            dtTablaEquipos = tablaEquipos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                data,
                columns: [
                    { data: 'noEconomico', title: 'No. Económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'noSerie', title: 'No. Serie' },
                    { data: 'estatus', title: 'Estatus' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-primary botonGestionar"><i class="fas fa-user-friends"></i> Gestionar</button>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tablaEquipos.find('.botonGestionar').click(function () {
                        const barrenadora = dtTablaEquipos.row($(this).parents('tr')).data();
                        inputNoEconomico.val(barrenadora.noEconomico);
                        inputDescripcion.val(barrenadora.descripcion);
                        botonAsignarOperadores.data().barrenadoraID = barrenadora.id;
                        obtenerOperadoresBarrenadora(barrenadora.id, 1);
                        modalGestion.modal('show');
                    });
                }
            });
        }

        function obtenerOperadoresBarrenadora(barrenadoraID, turno) {

            $.blockUI({ message: 'Cargando operadores...' });
            $.get('/Barrenacion/ObtenerOperadoresBarrenadora', { barrenadoraID, turno })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cargarOperadoresModal(response.items);
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

        function cargarOperadoresModal(operadores) {

            /* Campos del operador*/
            inputClaveOperador.val('');
            inputOperador.val('');
            inputSueldoOperador.val('');
            inputHorasJornadaOperador.val('');
            inputFSROperador.val('');
            inputTotalOperador.val('');

            /*Campos del ayudante. */
            inputClaveAyudante.val('');
            inputAyudante.val('');
            inputSueldoAyudante.val('');
            inputHorasJornadaAyudante.val('');
            inputFSRAyudante.val('');
            inputTotalAyudante.val('');

            //LLena la información con el operador.
            const operador = operadores.find(element => element.tipoOperador == 1);
            if (operador) {
                inputClaveOperador.val(operador.claveEmpleado);
                inputOperador.val(operador.descripcion);
                inputSueldoOperador.val(operador.sueldo);
                inputHorasJornadaOperador.val(operador.jornada);
                inputFSROperador.val(operador.fsr);
                realizarSumaOperadores();

            }

            const ayudante = operadores.find(element => element.tipoOperador == 2);
            if (ayudante) {
                inputClaveAyudante.val(ayudante.claveEmpleado);
                inputAyudante.val(ayudante.descripcion);
                inputSueldoAyudante.val(operador.sueldo);
                inputHorasJornadaAyudante.val(operador.jornada);
                inputFSRAyudante.val(operador.fsr);
                realizarSumaAyudante();
            }

        }

        function cargarEquipos() {

            const areaCuenta = comboAC.val();
            const estatusOperadores = comboEstatus.val();

            $.blockUI({ message: 'Cargando equipos...' });
            $.get('/Barrenacion/ObtenerBarrenadorasOperadores', { areaCuenta, estatusOperadores })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cargarTablaEquipos(response.listaBarrenadoras);
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

        function GuardarOperadoresBarrenadora() {

            validarDatos();
            const listaOperadores = obtenerOperadoresAsignacion();

            $.blockUI({ message: 'Asignando operadores...' });
            $.post('/Barrenacion/GuardarOperadoresBarrenadora', { listaOperadores })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Operadores asignados correctamente.`);
                        modalGestion.modal('hide');
                        cargarEquipos();
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

        function validarDatos() {
            if (inputClaveOperador.val().trim('').length == 0)
                return AlertaGeneral('Alerta', 'No Capturo la Clave del operador, favor de capturar para poder continuar');

            if (inputSueldoOperador.val() == 0)
                return AlertaGeneral('Alerta', 'No fue capturado el sueldo del operador, favor de capturar para poder continuar');
            if (inputHorasJornadaOperador.val() == 0)
                return AlertaGeneral('Alerta', 'No fueron capturadas las horas de jornada del operador, favor de capturar para poder continuar');
            if (inputHorasJornadaOperador.val() == 0)
                return AlertaGeneral('Alerta', 'No fueron capturadas las horas de jornada del operador, favor de capturar para poder continuar');
            if (inputFSROperador.val() == 0)
                return AlertaGeneral('Alerta', 'No fue Capturado el FSR del Operador, favor de capturar para poder continuar');
            if (inputClaveAyudante.val().trim('').length > 0) {
                if (inputSueldoAyudante.val() == 0)
                    return AlertaGeneral('Alerta', 'No fue capturado el sueldo del ayudante, favor de capturar para poder continuar');
                if (inputHorasJornadaAyudante.val() == 0)
                    return AlertaGeneral('Alerta', 'No fueron capturadas las horas de jornada del ayudante, favor de capturar para poder continuar');
                if (inputFSRAyudante.val() == 0)
                    return AlertaGeneral('Alerta', 'No fue Capturado el FSR del ayudante, favor de capturar para poder continuar');
            }

        }

        function obtenerOperadoresAsignacion() {

            const operadores = [];
            operadores.push({
                claveEmpleado: inputClaveOperador.val(),
                tipoOperador: 1,
                turno: comboTurno.val(),
                barrenadoraID: botonAsignarOperadores.data().barrenadoraID,
                sueldo: inputSueldoOperador.val(),
                jornada: inputHorasJornadaOperador.val(),
                fsr: inputFSROperador.val()
            });

            operadores.push({
                claveEmpleado: inputClaveAyudante.val(),
                tipoOperador: 2,
                turno: comboTurno.val(),
                barrenadoraID: botonAsignarOperadores.data().barrenadoraID,
                sueldo: inputSueldoAyudante.val(),
                jornada: inputHorasJornadaAyudante.val(),
                fsr: inputFSRAyudante.val()
            });

            return operadores;
        }

    }
    $(document).ready(() => {
        Barrenacion.ManoOBra = new ManoOBra();
    });
})();