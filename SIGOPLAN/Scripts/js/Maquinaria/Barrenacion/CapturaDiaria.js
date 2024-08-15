(() => {
    $.namespace('Barrenacion.CapturaDiaria');
    CapturaDiaria = function () {

        //#region Variables    

        let d = new Date();
        let idBanco;
        let _barrenadoraID;
        let _listaPiezas = [];

        const inputRehabilitacion = $('#inputRehabilitacion');
        const comboAC = $('#comboAC');
        const comboTurno = $('#comboTurno');
        const inputFecha = $('#inputFecha');
        const tablaEquipos = $('#tablaEquipos');
        const botonGuardar = $('#botonGuardar');

        // Modal detalles.
        const inputHorasTrabajadas = $("#inputHorasTrabajadas");
        const modalDetalles = $('#modalDetalles');
        const inputNoEconomico = $('#inputNoEconomico');
        const inputAreaCuenta = $('#inputAreaCuenta');
        const tablaDetalles = $('#tablaDetalles');

        let dtTablaDetalles;

        //Modal Detalle Operador y Ayudante.
        const inputClaveOperador = $("#inputClaveOperador");
        const inputNombreOperador = $("#inputNombreOperador");
        const inputSueldoOperador = $("#inputSueldoOperador");
        const inputHorasJornadaOperador = $("#inputHorasJornadaOperador");
        const inputFSROperador = $("#inputFSROperador");
        const inputTotalOperador = $("#inputTotalOperador");
        const inputMontoMensual = $('#inputMontoMensual');

        const inputClaveAyudante = $("#inputClaveAyudante");
        const inputNombreAyudante = $("#inputNombreAyudante");
        const inputSueldoAyudante = $("#inputSueldoAyudante");
        const inputHorasJornadaAyudante = $("#inputHorasJornadaAyudante");
        const inputFSRAyudante = $("#inputFSRAyudante");
        const inputTotalAyudante = $("#inputTotalAyudante");
        const comboAyudante = $("#comboAyudante");

        //Modal Detalle operador y ayudante fin;

        const botonAgregarDetalle = $('#botonAgregarDetalle');
        const botonAsignarDetalle = $('#botonAsignarDetalle');
        const GuardarCapturaDiaria = new URL(window.location.origin + '/Barrenacion/GuardarCapturaDiaria');
        const ObtenerBarrenadorasCaptura = new URL(window.location.origin + '/Barrenacion/ObtenerBarrenadorasCaptura');
        const ObtenerCapturaDiaria = new URL(window.location.origin + '/Barrenacion/ObtenerCapturaDiaria');
        const ObtenerEmpleadosEnKontrol = '/Barrenacion/ObtenerEmpleadosEnKontrol';
        const ObtenerSerieMartilloReparadoNoAsignado = new URL(window.location.origin + '/Barrenacion/ObtenerSerieMartilloReparadoNoAsignado');
        //#region Modal Insumos
        let listaInsumosInicial = [];
        let listaInsumosSalida = [];
        const comboOperadores = $("#comboOperadores");

        let listaInsumosTempEdit = [];
        const comboInsumosMartillo = $("#comboInsumosMartillo");
        const comboInsumosBroca = $("#comboInsumosBroca");
        const comboInsumosBarra = $("#comboInsumosBarra");
        const comboInsumosBarraSegunda = $("#comboInsumosBarraSegunda");
        const comboInsumosCulata = $('#comboInsumosCulata');
        const comboInsumosPortabit = $('#comboInsumosPortabit');
        const comboInsumosCilindro = $('#comboInsumosCilindro');
        const btnEditInsumo = $('.btnEditInsumo');
        const divMartillo = $("#divMartillo");
        const divEditForm = $('.divEditForm');
        const divCulata = $("#divCulata");
        const divCilindro = $("#divCilindro");
        const divBarra = $('#divBarra');
        const botonEditCulata = $("#botonEditCulata");
        const botonEditCilindro = $("#botonEditCilindro");
        const inputFechaPagoMensual = $('#inputFechaPagoMensual');

        const desmontarCilindro = $('#desmontarCilindro');
        const desmontarCulata = $('#desmontarCulata');
        const deshechoCilindro = $('#deshechoCilindro');
        const deshechoCulata = $('#deshechoCulata');
        const inputSerieMartillo = $("#inputSerieMartillo");
        const comboInsumosZanco = $('#comboInsumosZanco');


        const inputBordo = $("#inputBordo");
        const inputEspaciamiento = $("#inputEspaciamiento");
        const inputNoBarrenos = $("#inputNoBarrenos");
        const inputProfundidad = $("#inputProfundidad");
        const comboBanco = $("#comboBanco");
        const inputDensidadMaterial = $("#inputDensidadMaterial");
        const inputSubBarreno = $("#inputSubBarreno");
        //#endregion
        //operadores
        const sumaTotalOperadores = $(".totalOperadores");
        const sumaTotalAyudantes = $(".totalAyudantes");

        let dtTablaEquipos;
        let comboMotivoHTML = "";

        // Variables para utilizar en name de los radioButton
        let rowCounter = 0;
        //Variable para cargarCombos 
        const CargarInsumo = { Broca: true, Martillo: true, Barra: true, barraSegunda: true, Zanco: true };
        //#endregion Variables

        //agua 

        const botonGuardarPagoMensual = $('#botonGuardarPagoMensual');
        const botonGuardarAgua = $('#botonGuardarAgua');
        const inputLitros = $("#inputLitros");
        const inputFechaLitros = $('#inputFechaLitros');
        // Otros Montos
        const botonAbrirGuardarMontos = $('#botonAbrirGuardarMontos');
        const botonGuardarOtros = $('#botonGuardarOtros');
        //#region divs
        const divBroca = $('#divBroca');
        const divZanco = $("#divZanco");
        const deshechoZanco = $("#deshechoZanco");
        const inputPiezaAccionZanco = $('#inputPiezaAccionZanco');

        ////#region 

        const modalNuevoBanco = $("#modalNuevoBanco");
        const inputBanco = $("#inputBanco");
        const inputDescripcionBanco = $("#inputDescripcionBanco");
        const botonAgregarBanco = $("#botonAgregarBanco");
        const inputAreaCuentaBanco = $("#inputAreaCuentaBanco");
        const comboNuevoBanco = $("#comboNuevoBanco");
        const modalAgregarMontoOtros = $('#modalAgregarMontoOtros');
        const inputFechaOtrosGastos = $('#inputFechaOtrosGastos');
        const inputMonto = $('#inputMonto');

        ////#endregion

        //#region 
        //Captura diaria de informacion brocas
        const inputpzasModalBroca = $('#inputpzasModalBroca');
        const inputpzasModalMartillo = $('#inputpzasModalMartillo');
        const inptupzasModalBarra = $('#inptupzasModalBarra');
        const inputpzasModalCulata = $('#inputpzasModalCulata');
        const inputpzasModalCilindro = $('#inputpzasModalCilindro');
        const inputpzasModalZanco = $('#inputpzasModalZanco');
        const modalPzasCaptura = $('#modalPzasCaptura');

        const comboPiezasBroca = $('#comboPiezasBroca');
        const comboPiezasMartillo = $('#comboPiezasMartillo');
        const comboPiezasBarra = $('#comboPiezasBarra');
        const comboPiezasBarraSegunda = $('#comboPiezasBarraSegunda');
        const comboPiezasCilindro = $('#comboPiezasCilindro');
        const comboPiezasZanco = $('#comboPiezasZanco');
        const comboPiezasCulata = $("#comboPiezasCulata");

        (function init() {
            // initAutocompletes();
            agregarListeners();
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
            comboAC.select2();
            initTablaEquipos();
            initTablaDetalles();

            inputFecha.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());

            inputFechaLitros.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());

            inputFechaOtrosGastos.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
            botonGuardar.hide();

            cargarTiposCaptura();
            btnEditInsumo.click(editInsumo);
            divEditForm.addClass('hide');
            $("input:checkbox").on('click', clickCheckBox);
            initFechaMES();
        })();

        function initFechaMES() {
            $('.date-picker').datepicker(
                {
                    dateFormat: "mm/yy",
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    onClose: function (dateText, inst) {
                        function isDonePressed() {
                            return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                        }
                        if (isDonePressed()) {
                            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                            $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                            $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                        }
                    },
                    beforeShow: function (input, inst) {

                        inst.dpDiv.addClass('month_year_datepicker')

                        if ((datestr = $(this).val()).length > 0) {
                            year = datestr.substring(datestr.length - 4, datestr.length);
                            month = datestr.substring(0, 2);
                            $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                            $(this).datepicker('setDate', new Date(year, month - 1, 1));
                            $(".ui-datepicker-calendar").hide();
                        }
                    }
                })
        }



        function cargarTiposCaptura() {
            $.blockUI({ message: 'Cargando tipos de captura' });
            $.post('/Barrenacion/ObtenerTiposCapturas', { maquinaID: 1 })
                .always($.unblockUI)
                .then(response => {
                    if (response) {
                        // Operación exitosa.
                        response.reverse().pop();
                        comboMotivoHTML = response.map(item => `<option value=${item.Value} >${item.Text}</option>`).join('');
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

        async function cargarEquipos() {
            try {
                llenarCombos();
                let areaCuenta = comboAC.val()
                    , turno = comboTurno.val()
                    , fecha = inputFecha.val();
                if (areaCuenta === '') {
                    AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costos.`);
                    return;
                }
                comboBanco.fillCombo('/Barrenacion/GetComboBancos', { areaCuenta: areaCuenta }, false);
                response = await ejectFetchJson(ObtenerBarrenadorasCaptura, { areaCuenta, turno, fecha });

                if (response.success) {
                    if (response.items && response.items.length > 0) {

                        dtTablaEquipos.clear().draw();
                        dtTablaEquipos.rows.add(response.items).draw();

                        tablaEquipos.find('select.motivo').hide();
                        botonGuardar.show();
                    } else {
                        if (dtTablaEquipos != null) {
                            dtTablaEquipos.clear().draw();
                        }
                    }
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    if (dtTablaEquipos != null) {
                        dtTablaEquipos.clear().draw();
                        botonGuardar.hide(1000);
                    }
                }
            }
            catch (error) {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                botonGuardar.hide(1000);
            }
        }

        function initTablaEquipos() {
            dtTablaEquipos = tablaEquipos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'noEconomico', title: 'No. Económico' },
                    {
                        data: 'id', title: 'Horas Trabajadas', render: (data, type, row) =>
                            `<input type="text" value="0" class="inputHF form-control numerico">`
                    },
                    { data: 'horometro', title: 'Horómetro Acumulado' },
                    {
                        data: 'operador', title: 'Operador', render: (data, type, row) => {
                            const operador = row.operador;
                            if (operador) {
                                return `
                                    <div class="infoEmpleado" >
                                        <div class="col-md-2">
                                            <input type='text' disabled='disabled' class="form-control numerico inputClave claveOperador" value='${operador.claveEmpleado != null ? operador.claveEmpleado : ""}'>
                                        </div>
                                        <div class="col-md-10">
                                            <input type='text'disabled='disabled' class="form-control inputDescripcion" value='${operador.nombre != null ? operador.nombre : ""}'>
                                        </div>
                                    </div>`;
                            } else {
                                return `
                                    <div class="col-md-12 infoEmpleado">
                                        <div class="col-md-2">
                                            <input type='text' disabled='disabled' class="form-control numerico inputClave claveOperador">
                                        </div>
                                        <div class="col-md-10">
                                            <input type='text'disabled='disabled' class="form-control inputDescripcion">
                                        </div>
                                    </div>`;
                            }
                        }
                    },
                    {
                        data: 'ayudante', title: 'Ayudante', render: (data, type, row) => {
                            const ayudante = row.ayudante;
                            if (ayudante) {
                                return `
                                    <div class="col-md-12 infoEmpleado">
                                        <div class="col-md-2">
                                            <input disabled='disabled' class="text-center   form-control numerico inputClave claveAyudante" value='${ayudante.claveEmpleado != null ? ayudante.claveEmpleado : ""}'>
                                        </div>
                                        <div class="col-md-10">
                                            <input disabled='disabled' class="text-center   form-control inputDescripcion" value='${ayudante.nombre != null ? ayudante.nombre : ""}'>
                                        </div>
                                    </div>`;
                            } else {
                                return `
                                    <div class="col-md-12 infoEmpleado">
                                        <div class="col-md-2">
                                            <input class="text-center  form-control numerico inputClave claveAyudante" disabled='disabled'>
                                        </div>
                                        <div class="col-md-10">
                                            <input class="text-center form-control inputDescripcion" disabled='disabled'>
                                        </div>
                                    </div>`;
                            }
                        }
                    },

                    {
                        data: 'id', title: 'Detalles', render: (data, type, row) =>
                            `<button class="btn btn-primary botonDetalles"><i class="fas fa-th"></i></button>`
                    },
                    {
                        data: 'id', title: 'Captura Especial', render: (data, type, row) =>
                            `<input type="checkbox" class="form-control especial">`
                    },
                    {
                        data: 'id', title: 'Motivo', render: (data, type, row) =>
                            `<select disabled class="form-control motivo"></select>`
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '20%', targets: [3, 4, 7] },
                    {
                        "targets": [1],
                        "visible": false
                    }
                ],
                drawCallback: function (settings) {

                    tablaEquipos.find('.botonDetalles').click(function () {
                        const barrenadora = dtTablaEquipos.row($(this).parents('tr')).data();
                        _barrenadoraID = barrenadora.id;
                        inputNoEconomico.val(barrenadora.noEconomico);
                        ObtenerBarrenadorasOperadores(_barrenadoraID);
                        fnCargarCapturasDiarias(barrenadora.id);

                    });

                    tablaEquipos.find('input[type="checkbox"].especial').click(function () {
                        const select = $(this).parents('tr').find('select');
                        const button = $(this).parents('tr').find('.botonDetalles');
                        select.attr('disabled', !this.checked);
                        select.prop("selectedIndex", 0);
                        this.checked ? select.show(500) : select.hide(500);
                    });

                    tablaEquipos.find('select.motivo').append(comboMotivoHTML);

                    tablaEquipos.find('.inputClave').toArray().forEach(inputClave => {
                        inputClave = $(inputClave);
                        inputClave.getAutocompleteValid(setClaveEmpledoDesc, verificarClaveEmpleado, { porDesc: false }, ObtenerEmpleadosEnKontrol);
                    });

                    tablaEquipos.find('.inputDescripcion').toArray().forEach(inputDescripcion => {
                        inputDescripcion = $(inputDescripcion);
                        inputDescripcion.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, { porDesc: true }, ObtenerEmpleadosEnKontrol);
                    });
                }
            });
        }

        function fnCargarCapturasDiarias(id) {
            dtTablaDetalles.clear().draw();

            $.get('/Barrenacion/ObtenerCapturaDiaria', { barrenadoraID: id, pfechaInicio: inputFecha.val(), turno: comboTurno.val() })
                .then(response => {
                    let disabled = response.disabled;
                    if (response.success) {
                        dtTablaDetalles.rows.add(response.detalles).draw();
                    }
                    $(".botonEliminarDetalle").prop('disabled', disabled);
                    //botonAgregarDetalle.prop('disabled', disabled);
                    //botonAsignarDetalle.prop('disabled', disabled);
                    inputAreaCuenta.val(comboAC.find('option:selected').text());
                    botonAsignarDetalle.data().barrenadoraID = id;
                    obtenerOperadoresBarrenadora(id, comboTurno.val());
                    if ($("#tipoBarreno").prop("checked")) {
                        inputNoBarrenos.prop("disabled", false);
                        inputRehabilitacion.prop("disabled", true);
                        inputRehabilitacion.val("");
                    }
                    else {
                        inputNoBarrenos.prop("disabled", true);
                        inputNoBarrenos.val("");
                        inputRehabilitacion.prop("disabled", false);
                    }
                    inputDensidadMaterial.val(response.ultimaDensidad);
                    modalDetalles.modal('show');
                    tablaDetalles.find('input:first').focus();
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setClaveEmpledo(e, ui) {
            const row = $(this).closest('div.infoEmpleado');
            $(this).val(ui.item.value);
            row.find('.inputClave').val(ui.item.id);
        }

        function setClaveEmpledoDesc(e, ui) {
            const row = $(this).closest('div.infoEmpleado');
            $(this).val(ui.item.value)
            row.find('.inputDescripcion').val(ui.item.id);
        }

        function verificarClaveEmpleado(e, ui) {
            if (ui.item == null) {
                const row = $(this).closest('div.infoEmpleado');
                row.find('.inputClave').val('');
                row.find('.inputDescripcion').val('');
            }
        }

        tablaDetalles.on('click', '.btnVerPiezas', function () {
            let data = dtTablaDetalles.row($(this).parents('tr')).data();
            inputpzasModalBroca.val(data.brocaSerie);
            inputpzasModalMartillo.val(data.martilloSerie);
            inptupzasModalBarra.val(data.barraSerie);
            inputpzasModalCulata.val(data.culataSerie);
            inputpzasModalCilindro.val(data.cilindroSerie);
            inputpzasModalZanco.val(data.zancoSerie);
            modalPzasCaptura.modal('show')
        });

        function initTablaDetalles() {
            dtTablaDetalles = tablaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    {
                        data: null, title: 'pieazas',
                        createdCell: function (td, celldata, rowData, row, col) {
                            let html = `<button class="btn btn-primary btnVerPiezas"><i class="fas fa-th-list"></i></button>`;
                            $(td).text('');
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'horasTrabajadas', title: 'Horas Trabajadas',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text"  ${disabled} class="form-control inputHorasTrabajadas numerico" value=${cellData}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'bordo', title: 'Bordo',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled} class="form-control inputBordo numerico" value=${cellData}>`;
                            $(td).append(html);
                        }

                    },
                    {
                        data: 'espaciamiento', title: 'Espaciamiento',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled} class="form-control inputEspaciamiento numerico" value=${cellData}>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'barrenos', title: '# Barrenos',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit || rowData.tipoBarreno == 2) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled}  class="form-control numerico inputNB" value=${(rowData.tipoBarreno == 2 ? 0 : cellData)}>`;// `<input type="text" ${disabled} class="form-control inputEspaciamiento numerico" value=${cellData}>`;
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'profundidad', title: 'Profundidad',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled} class="form-control numerico inputPF" value=${cellData}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'banco', title: 'Banco',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled}  class="form-control inputBanco" value='${cellData}'>`;
                            $(td).append(html);
                        }

                    },
                    {
                        data: 'densidadMaterial', title: 'Densidad Material',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input type="text" ${disabled} class="form-control numerico inputDM" value=${cellData}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'tipoBarreno', title: 'Tipo de barreno',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `N  <input  ${disabled} type="checkbox" class="tipoBarrenoNormal" name="${'tipoBarreno' + rowCounter}" ${cellData == 1 ? 'checked' : ''}><br>
                            R  <input  ${disabled} type="checkbox" class="tipoBarrenoRehabilitacion" name="${'tipoBarreno' + rowCounter++}" ${cellData == 2 ? 'checked' : ''}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'subbarreno', title: 'Profundidad Sub-barreno',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit) {
                                disabled = 'disabled'
                            }
                            let html = `<input ${disabled} type="text" class="form-control numerico inputPFS" value=${cellData}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'rehabilitacion', title: 'Rehabilitacion',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let disabled = '';
                            $(td).text('');
                            if (!rowData.edit || rowData.tipoBarreno == 1) {
                                disabled = 'disabled'
                            }
                            let html = `<input ${disabled} type="text" class="form-control numerico " value=${(rowData.tipoBarreno == 1 ? 0 : cellData)}>`
                            $(td).append(html);
                        }
                    },
                    {
                        data: 'id', title: 'Eliminar',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let html = `<button class="btn btn-danger botonEliminarDetalle" data-id='${cellData}'><i class="fas fa-trash-alt"></i></button>`;
                            $(td).text('');
                            if (rowData.edit) {
                                $(td).append(html);
                            }
                        }
                    }
                ],
                columnDefs: [{ className: "dt-center", "targets": "_all" }],
                drawCallback: function () {


                }
            });
        }

        tablaDetalles.on('click', '.botonEliminarDetalle', function () {
            let data = dtTablaDetalles.row($(this).closest('tr')).data();
            if (data.id != 0) {
                fnEliminarCaptura(data.id, data.barrenadoraID);
            }
            else {
                let detalleDelete = dtTablaDetalles.data().toArray();
                detalleDelete.pop();
                for (let index = 0; index < detalleDelete.length; index++) {
                    const element = detalleDelete[index];
                    dtTablaDetalles.row.add(element).draw();
                }

            }
        });

        function fnEliminarCaptura(capturaID, barrenadoraID) {
            $.post('/Barrenacion/eliminarCaptura', { capturaID: capturaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        fnCargarCapturasDiarias(barrenadoraID);
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

        function agregarListeners() {

            tablaEquipos.on("keypress", ".numerico", function (e) {
                if (e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) //solo dígitos
                    return false;
            });

            tablaDetalles.on("keypress", ".numerico", function (e) {
                if (e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) //solo dígitos
                    return false;
            });

            comboTurno.change(cargarEquipos);
            botonAgregarDetalle.click(agregarDetalle);
            comboAC.change(cargarEquipos);
            inputFecha.change(cargarEquipos);

            botonAsignarDetalle.click(actualizarDetallesBarrenadora);
            botonGuardar.click(setGuardarCapturaDiaria);
            modalDetalles.on('hide.bs.modal', limpiarCamposModal);

            sumaTotalOperadores.change(realizarSumaOperadores);
            sumaTotalAyudantes.change(realizarSumaAyudante);
            $('.comboInsumo').change(GetInfoInsumoNuevo);
            inputClaveAyudante.change();
            inputClaveOperador.change();

            modalNuevoBanco.on("hide.bs.modal", limpiarModal);
            comboNuevoBanco.click(
                () => {
                    limpiarModal();
                    modalNuevoBanco.modal('show');
                });
            botonAgregarBanco.click(GuardarBanco);
            botonGuardarAgua.click(GuardarAgua);
            botonGuardarOtros.click(GuardarOtros);
            botonAbrirGuardarMontos.click(openModalMontos);
            botonGuardarPagoMensual.click(GuardarPagoMensual);
            $("#tipoBarreno").change(function () {
                if ($("#tipoBarreno").prop("checked")) {
                    inputNoBarrenos.prop("disabled", false);
                    inputRehabilitacion.prop("disabled", true);
                    inputRehabilitacion.val("");
                }
                else {
                    inputNoBarrenos.prop("disabled", true);
                    inputNoBarrenos.val("");
                    inputRehabilitacion.prop("disabled", false);
                }
            });
        }

        function GuardarPagoMensual() {
            $.post('/Barrenacion/guardarPagoMensual', { areaCuenta: comboAC.val(), fechaCaptura: inputFechaPagoMensual.val(), cantidad: inputMontoMensual.val(), id: 0 })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
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

        function openModalMontos() {
            modalAgregarMontoOtros.modal('show');
        }

        function GuardarAgua() {

            if (+inputLitros.val()) {
                $.post('/Barrenacion/guardarAgua', { areaCuenta: comboAC.val(), turno: comboTurno.val(), fechaCaptura: inputFechaLitros.val(), litros: inputLitros.val(), id: 0 })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral('Operacion Exitosa', 'Se realizo la captura Correctamente');
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
            else {

                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: No valido`);
            }

        }

        function GuardarOtros() {

            if (+inputMonto.val() > 0) {
                $.post('/Barrenacion/guardarOtrosPrecios', { areaCuenta: comboAC.val(), turno: comboTurno.val(), fechaCaptura: inputFechaOtrosGastos.val(), monto: inputMonto.val(), id: 0 })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral('Operacion Exitosa', 'Se realizo la captura Correctamente');
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
            else {
                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: No valido`);
            }

        }

        //#endregion 
        function GetInfoInsumoNuevo() {
            $.get('/Barrenacion/getInfoInsumo', { barrenadoraID: botonAsignarDetalle.data().barrenadoraID, insumo: $(this).val(), areaCuenta: comboAC.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let divID = $(this).parents('div.seccion').attr('id');
                        let div = $(`#${divID}`)
                        div.find('.inputNoSerie').data().piezaID = response.data.id;
                        div.find('.inputNoSerie').val(response.nuevoInsumo);
                        div.find('.comboInsumo').val(response.data.insumo);
                        div.find('.inputPrecio').val(response.data.precio);
                        div.find('.inputSerie').val(response.nuevoInsumo);
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

        function limpiarCamposModal() {
            dtTablaDetalles.clear().draw();
        }

        function agregarDetalle() {

            let horasTrabajadas = inputHorasTrabajadas.val();
            let tipoBarreno = $("#tipoBarreno").prop("checked") ? 1 : 2;
            let bordo = inputBordo.val();
            let espaciamiento = inputEspaciamiento.val();
            let barrenos = inputNoBarrenos.val();
            let profundidad = inputProfundidad.val();
            let banco = $("#comboBanco option:selected").text(); //comboBanco.val();
            let densidadMaterial = inputDensidadMaterial.val();
            let subbarreno = inputSubBarreno.val();
            let comentario = "Favor de Capturar para continuar.";
            let rehabilitacion = inputRehabilitacion.val();

            if (horasTrabajadas == 0)
                return AlertaGeneral('Alerta', 'No se capturaron las horas trabajadas ' + comentario);

            if (tipoBarreno == 1) {
                if (bordo == 0)
                    return AlertaGeneral('Alerta', 'El Bordo no puede ser 0 en un barreno normal, ' + comentario);
                if (espaciamiento == 0)
                    return AlertaGeneral('Alerta', 'El espaciamiento no puede ser 0 en un barre normal, ' + comentario);
            }
            if (barrenos == 0 && $("#tipoBarreno").prop("checked"))
                return AlertaGeneral('Alerta', 'No fue capturado el numero de barrenos, ' + comentario);

            if (rehabilitacion == 0 && !$("#tipoBarreno").prop("checked"))
                return AlertaGeneral('Alerta', 'No fue capturado el numero de rehabilitacion, ' + comentario);

            if (profundidad == 0)
                return AlertaGeneral('Alerta', 'No fue capturada la profundidad del barreno,' + comentario);

            if (comboBanco == 0)
                return AlertaGeneral('Alerta', 'No fue seleccionado un banco, Favor de Seleccionar para continuar');

            if (densidadMaterial == 0)
                return AlertaGeneral('Alerta', 'No fue capturada la densidad del material ' + comentario);

            if (subbarreno == 0)
                return AlertaGeneral('Alerta', 'No fue capturado el subarrendó ' + comentario);

            if ($('.btnAceptarInsumo').length > 0)
                return AlertaGeneral('Alerta', 'Usted tiene una pieza en modificación, favor de terminar la edición para poder continuar.');

            dtTablaDetalles.row.add({
                bordo: bordo,
                espaciamiento: espaciamiento,
                barrenos: barrenos,
                profundidad: profundidad,
                banco: banco,
                densidadMaterial: densidadMaterial,
                tipoBarreno: tipoBarreno,
                subbarreno: subbarreno,
                horasTrabajadas: horasTrabajadas,
                brocaID: getIDNumeroSeriePieza("Broca").piezaID,
                martilloID: getIDNumeroSeriePieza("Martillo").piezaID,
                barraID: getIDNumeroSeriePieza("Barra").piezaID,
                culataID: getIDNumeroSeriePieza("Culata").piezaID,
                cilindroID: getIDNumeroSeriePieza("Cilindro").piezaID,
                barraSegundaID: getIDNumeroSeriePieza("BarraSegunda").piezaID,
                zancoID: getIDNumeroSeriePieza("Zanco").piezaID,
                brocaSerie: getIDNumeroSeriePieza("Broca").NoSerie,
                martilloSerie: getIDNumeroSeriePieza("Martillo").NoSerie,
                barraSerie: getIDNumeroSeriePieza("Barra").NoSerie,
                culataSerie: getIDNumeroSeriePieza("Culata").NoSerie,
                cilindroSerie: getIDNumeroSeriePieza("Cilindro").NoSerie,
                barraSegundaSerie: getIDNumeroSeriePieza("BarraSegunda").NoSerie,
                zancoSerie: getIDNumeroSeriePieza("Zanco").NoSerie,
                rehabilitacion: rehabilitacion,
                id: 0,
                edit: true
            }).draw();

            limpiarCapturaPlantilla();
        }

        function getIDNumeroSeriePieza(tipoPieza) {
            const pieza = {};
            const div = $(`#div${tipoPieza}`);
            pieza.NoSerie = div.find('.inputNoSerie').val();
            pieza.piezaID = div.find('.inputNoSerie').data().piezaID;

            return pieza;
        }

        function limpiarCapturaPlantilla() {
            inputHorasTrabajadas.val('');
            //$('input[name="tipoBarreno"]').prop('checked', false);
            $("#tipoBarreno").prop("checked", true);
            inputBordo.val('');
            inputEspaciamiento.val('');
            inputNoBarrenos.val('');
            inputProfundidad.val('');
            comboBanco.val(0);
            inputDensidadMaterial.val('');
            inputSubBarreno.val('');
        }

        function actualizarDetallesBarrenadora() {

            let fecha = inputFecha.val();
            var tableData = dtTablaEquipos.rows().data().toArray();
            const rowData = tableData.find(row => row.id == botonAsignarDetalle.data().barrenadoraID);
            const listaDetalles = dtTablaDetalles.data().toArray()
                .map(row => {
                    return {
                        barrenadoraID: botonAsignarDetalle.data().barrenadoraID,
                        bordo: row.bordo,
                        espaciamiento: row.espaciamiento,
                        barrenos: row.barrenos,
                        profundidad: row.profundidad,
                        banco: row.banco,
                        densidadMaterial: row.densidadMaterial,
                        tipoBarreno: row.tipoBarreno,
                        subbarreno: row.subbarreno,
                        horasTrabajadas: row.horasTrabajadas,
                        claveOperador: comboOperadores.val(),
                        precioOperador: 0, //Math.round(inputSueldoOperador.val() / inputHorasJornadaOperador.val(), 2),
                        fsrOperador: 0,//inputFSROperador.val(),
                        totalOperadores: 0,//Math.round(inputSueldoOperador.val() / inputHorasJornadaOperador.val() * (1 + (inputFSROperador.val() / 100)), 2),
                        claveAyudante: comboAyudante.val(),
                        precioAyudante: 0,//Math.round(inputSueldoAyudante.val() / inputHorasJornadaAyudante.val(), 2),
                        fsrAyudante: inputFSRAyudante.val(),
                        totalAyudantes: 0,//Math.round(inputSueldoAyudante.val() / inputHorasJornadaAyudante.val() * (1 + (inputFSRAyudante.val() / 100)), 2),
                        brocaID: row.brocaID,
                        martilloID: row.martilloID,
                        barraID: row.barraID,
                        culataID: row.culataID,
                        cilindroID: row.cilindroID,
                        zancoID: row.zancoID,
                        barraSegunda: row.barraSegundaID,
                        brocaSerie: row.brocaSerie,
                        martilloSerie: row.martilloSerie,
                        barraSerie: row.barraSerie,
                        culataSerie: row.culataSerie,
                        cilindroSerie: row.cilindroSerie,
                        zancoSerie: row.zancoSerie,
                        barraSegunda: row.barraSegundaSerie,
                        horasTrabajadas: row.horasTrabajadas,
                        horometroFinal: rowData.horometro,
                        turno: comboTurno.val(),
                        tipoCaptura: 1,
                        fechaCaptura: fecha,
                        areaCuenta: comboAC.val(),
                        rehabilitacion: row.rehabilitacion,
                        id: row.id

                    };
                });
            listaInsumosSalida = listaInsumosSalida.filter(row => row != '');
            setGuardarCapturaDiariaXDetalle(listaDetalles, _listaPiezas, rowData);
        }

        function setGuardarCapturaDiariaXDetalle(listaDetalles, listaInsumosSalida) {
            $.blockUI({ message: 'Se comenzo a guardar la información.' });
            $.post('/Barrenacion/setInfoCapturaDiaria', { listaCapturaDiaria: listaDetalles, listaPiezas: listaInsumosSalida })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalDetalles.modal('hide');
                        AlertaGeneral(`Operación Exitosa`, `Se realizó correctamente el guardado de los barrenos`);

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                ).then(
                    $.get('/Barrenacion/ObtenerCapturaDiaria', { barrenadoraID: botonAsignarDetalle.data().barrenadoraID, pfechaInicio: inputFecha.val(), turno: comboTurno.val() })
                        .then(response => {
                            dtTablaDetalles.clear().draw();
                            if (response.success) {
                                dtTablaDetalles.rows.add(response.detalles).draw();
                            }
                            obtenerOperadoresBarrenadora(botonAsignarDetalle.data().barrenadoraID, comboTurno.val());

                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        )
                );
        }

        async function setGuardarCapturaDiaria() {

            try {
                let datosValidos = validarDatosCaptura();

                let fecha = inputFecha.val();

                if (datosValidos === false) {
                    AlertaGeneral(`Aviso`, `Se debe seleccionar por lo menos una captura especial para continuar con el guardado.`);
                    return;
                }

                botonGuardar.hide(10);
                response = await ejectFetchJson(GuardarCapturaDiaria, { listaCaptura: datosValidos, fecha });

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Captura guardada correctamente`);
                    dtTablaEquipos.clear().draw();
                    botonGuardar.show();
                    botonGuardar.prop('disabled', true);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }

            } catch (error) { AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`); }
        }

        function validarDatosCaptura() {

            const turno = comboTurno.val();
            const datosCaptura = tablaEquipos.find('tbody tr[role="row"]').toArray()
                .map(row => {
                    let $row = $(row);
                    const esCapturaEspecial = $row.find('input[type="checkbox"].especial')[0].checked;
                    if (esCapturaEspecial) {
                        let barrenadora = dtTablaEquipos.row(row).data();
                        const captura = {
                            horasTrabajadas: +$row.find('.inputHF').val(),
                            horometroFinal: 0,
                            barrenadoraID: barrenadora.id,
                            detalles: esCapturaEspecial ? null : barrenadora.detalles,
                            turno,
                            claveOperador: +$row.find('.claveOperador').val(),
                            claveAyudante: +$row.find('.claveAyudante').val(),
                            tipoCaptura: esCapturaEspecial ? +$row.find('select.motivo').val() : 1
                        }
                        return captura;
                    }

                }).filter(x => x != undefined);
            let datosValidos = false;
            if (datosCaptura.length > 0) {

                datosValidos = true;
                for (let index = 0; index < datosCaptura.length; index++) {
                    const captura = datosCaptura[index];

                    if (captura.tipoCaptura != 1) {
                        continue;
                    }

                    // Se validan los detalles.
                    let detallesInvalidos = false;
                    detallesInvalidos = (captura.detalles == null || captura.detalles.constructor !== Array || captura.detalles.length === 0) ||
                        (captura.detalles.filter(x =>
                        ((x.tipoBarreno === 1 && (Number.isNaN(x.bordo) || x.bordo <= 0 || Number.isNaN(x.espaciamiento) || x.espaciamiento <= 0)) ||
                            Number.isNaN(x.barrenos) || x.barrenos <= 0 ||
                            Number.isNaN(x.profundidad) || x.profundidad <= 0 ||
                            x.banco == "" ||
                            Number.isNaN(x.densidadMaterial) || x.densidadMaterial <= 0 ||
                            Number.isNaN(x.subbarreno) || x.subbarreno <= 0)).length > 0);

                    // Se validan datos principales
                    if (Number.isNaN(captura.horasTrabajadas) || captura.horasTrabajadas <= 0 ||
                        Number.isNaN(captura.claveOperador) || captura.claveOperador <= 0 ||
                        detallesInvalidos) {
                        datosValidos = false;
                        break;
                    }
                }
            }
            return datosValidos ? datosCaptura : false;
        }

        function obtenerOperadoresBarrenadora(barrenadoraID, turno) {
            // llenarCombos();
            $.blockUI({ message: 'Cargando operadores...' });
            $.get('/Barrenacion/ObtenerOperadoresBarrenadora', { barrenadoraID, turno })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cargarOperadores(response.items);
                        return barrenadoraID;
                    }
                    else
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                )
                .then(
                    $.get('/Barrenacion/ObtenerPiezasBarrenadora', { barrenadoraID: barrenadoraID, areaCuenta: comboAC.val() })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                _listaPiezas = [];
                                _barrenadoraID = barrenadoraID;//barrenadora.id;
                                fnEnable();
                                if (response.items != null) {
                                    _listaPiezas = response.items;
                                    cargarPiezasModal(response.items);
                                }
                            } else {
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        )
                );
        }

        //#region Mano de obra Operadores y Ayudantes.

        //Métodos.
        function initAutocompletes() {

            /*
             inputClaveOperador.getAutocompleteValid(setClaveEmpledoDesc, verificarClaveEmpleado, { porDesc: false }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
             inputNombreOperador.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, { porDesc: true }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
             inputClaveAyudante.getAutocompleteValid(setClaveEmpledoDesc, verificarClaveEmpleado, { porDesc: false }, '/Barrenacion/ObtenerEmpleadosEnKontrol');
             inputNombreAyudante.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, { porDesc: true }, '/Barrenacion/ObtenerEmpleadosEnKontrol');*/
        }

        function setClaveEmpledo(e, ui) {
            const row = $(this).closest('div.operador');

            $(this).val(ui.item.value);
            row.find('.inputClave').val(ui.item.id);
            row.find('.inputSueldo').val(ui.item.sueldo);
        }

        function setClaveEmpledoDesc(e, ui) {
            const row = $(this).closest('div.operador');

            $(this).val(ui.item.value)
            row.find('.inputDescripcion').val(ui.item.id);
            row.find('.inputSueldo').val(ui.item.sueldo);
        }

        function verificarClaveEmpleado(e, ui) {
            if (ui.item == null) {
                const row = $(this).closest('div.row');
                row.find('.inputClave').val('');
                row.find('.inputDescripcion').val('');
                row.find('.inputSueldo').val(ui.item.sueldo);
            }
        }
        function ObtenerBarrenadorasOperadores(_barrenadoraID) {
            $.get('/Barrenacion/ObtenerOperadoresBarrenadora', { barrenadoraID: _barrenadoraID, turno: comboTurno.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let operadores = response.items.filter(r => r.tipoOperador == 1).map(r => {
                            return { id: r.claveEmpleado, text: r.descripcion }
                        });

                        let ayudantes = response.items.filter(r => r.tipoOperador == 1).map(r => {
                            return { id: r.claveEmpleado, text: r.descripcion }
                        });
                        comboOperadores.select2({ data: operadores });
                        comboAyudante.select2({ data: ayudantes });

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
        //1147
        //Calculo del Costo Hora por Operador y Ayudante.
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

        //Obtiene la informacion de los operadores y ayudantes segun la barrenadora.
        function cargarOperadores(operadores) {

            /* Campos del operador*/
            inputClaveOperador.val('');
            inputNombreOperador.val('');
            inputSueldoOperador.val('');
            inputHorasJornadaOperador.val('');
            inputFSROperador.val('');

            /*Campos del ayudante. */
            inputClaveAyudante.val('');
            inputNombreAyudante.val('');
            inputSueldoAyudante.val('');
            inputHorasJornadaAyudante.val('');
            inputFSRAyudante.val('');

            //LLena la información con el operador.
            const operador = operadores.find(element => element.tipoOperador == 1);
            if (operador) {
                inputClaveOperador.val(operador.claveEmpleado);
                inputNombreOperador.val(operador.descripcion);
                inputSueldoOperador.val(operador.sueldo);
                inputHorasJornadaOperador.val(operador.jornada);
                inputFSROperador.val(operador.fsr);
                realizarSumaOperadores();
            }
            const ayudante = operadores.find(element => element.tipoOperador == 2);
            if (ayudante) {
                inputClaveAyudante.val(ayudante.claveEmpleado);
                inputNombreAyudante.val(ayudante.descripcion);
                inputSueldoAyudante.val(operador.sueldo);
                inputHorasJornadaAyudante.val(operador.jornada);
                inputFSRAyudante.val(operador.fsr);
                realizarSumaAyudante();
            }
        }
        //#endregion

        //#region Piezas Barrenadora
        //Carga los ComboBox de cada una de las piezas en el fieldset de los insumos.
        function cargarPiezasModal(piezas) {
            const arrayDivsInputs = ['Broca', 'Martillo', 'Barra', 'Culata', 'Cilindro', 'BarraSegunda', 'Zanco'];
            arrayDivsInputs.forEach(divID => {
                const div = $(`#div${divID}`);
                const tipoPieza = +(div.attr('tipoPieza'));
                let pieza = null;
                if (tipoPieza == 3 || tipoPieza == 7) {
                    if (divID == 'Barra') {
                        pieza = piezas.find(item => (item.tipoPieza == 3) && !item.barraSegunda);
                    } else if (divID == 'BarraSegunda') {
                        pieza = piezas.find(item => (item.tipoPieza == 7) && item.barraSegunda);
                    }
                } else {
                    pieza = piezas.find(item => item.tipoPieza == tipoPieza);
                }
                setPiezaExiste(div, pieza);
            });
        }

        function setPiezaExiste(div, pieza) {
            if (pieza) {
                div.removeClass('hide');
                div.find('.inputNoSerie').val(pieza.noSerie.trim(' '));
                div.find('.inputNoSerie').attr('disabled', true);
                div.find('.inputSerie').val(pieza.serialExcel).attr('disabled', true);
                div.find('.inputNoSerie').data().piezaID = pieza.id;
                div.find('select').attr('disabled', true).val(pieza.insumo);
                div.find('.inputPrecio').val(maskNumero(pieza.precio));
                div.find('.inputPrecio').attr('disabled', true);
                div.find('.comboPieza').val(pieza.id).attr('disabled', true);
                div.find('.spanSizeRadio').children('input').attr('disabled', true);
                div.find('.comboInsumo').val(pieza.insumo);
                div.find('.btn-xs').removeClass('btnAceptarInsumo');
                div.find('.btn-xs').addClass('btnEditInsumo');
            }
            else {
                div.find('.btn-xs').removeClass('btnAceptarInsumo');
                div.find('.btn-xs').addClass('btnEditInsumo');
                div.addClass('hide');
            }
        }

        function disablePieza(div) {
            div.find('button').removeClass('btnAceptarInsumo').addClass('btnEditInsumo').prop('disabled', true);
            $.each(div.find('input:Checkbox'), (key, value) => {
                $(value).prop('disabled', true);
                $(value).prop('checked', false);
            });
        }

        function setPieza(div, pieza) {
            if (pieza) {
                div.find('.inputNoSerie').val(pieza.noSerie);
                div.find('.inputNoSerie').attr('disabled', true);
                div.find('.inputNoSerie').data().piezaID = pieza.id;
                div.find('.btn-danger').prop('disabled', false).show();
                div.find('select').attr('disabled', true).val(pieza.insumo);
                div.find('.inputPrecio').attr('disabled', true);
                div.find('.inputPrecio').val(pieza.precio);
                div.find('.quitarPieza').data().modificar = true;
                div.find('.quitarPieza').data().piezaEditada = null;
                div.find('.quitarPieza').val(' ');
                div.find('.inputSerie').val(pieza.serialExcel);
                div.find('.inputSerie').attr('disabled', true);
            } else {
                if (pieza == undefined) {
                    div.addClass('hide');
                }

            }
        }

        function llenarCombos() {

            limpiarCombos();
            fnLimpiarComboPiezas();

            if (comboAC.val() != "") {
                $.blockUI({ message: 'Cargando insumos...' });
                $.get('/Barrenacion/ObtenerInsumosPorPiezaConPrecio', { areaCuenta: comboAC.val() })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            cargarCombosInsumos(response);
                            if (response.BrocaBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.BrocaBarrenadora, comboPiezasBroca);
                            if (response.MartilloBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.MartilloBarrenadora, comboPiezasMartillo);
                            if (response.BarraBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.BarraBarrenadora, comboPiezasBarra);
                            if (response.BarraSegunda ?? false)
                                CargarPiezasBarrenadora(response.BarraSegunda, comboPiezasBarraSegunda);
                            if (response.CulataBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.CulataBarrenadora, comboPiezasCulata);
                            if (response.CilindroBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.CilindroBarrenadora, comboPiezasCilindro);
                            if (response.ZancoBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.ZancoBarrenadora, comboPiezasZanco);
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

        }

        function CargarPiezasBarrenadora(listaPiezas, cboPiezas) {
            if (listaPiezas.length > 0) {
                listaPiezas.forEach(piezaBarrenadora => {
                    $(cboPiezas).append(`<option value="${piezaBarrenadora.id}" 
                                         data-precioPza="${piezaBarrenadora.precio}"
                                         data-horasAcum="${piezaBarrenadora.horasAcumuladas}">
                    ${piezaBarrenadora.serieAutomatica}
                    </option>`
                    );
                });
            }
        }

        function cargarCombosInsumos(listaInsumos) {
            if (listaInsumos.Broca && listaInsumos.Broca.length > 0) {
                listaInsumos.Broca.forEach(insumoPieza => {
                    comboInsumosBroca.append(`<option value="${insumoPieza.insumo}" data-precioPza="${insumoPieza.precioPieza}">${insumoPieza.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Martillo && listaInsumos.Martillo.length > 0) {
                listaInsumos.Martillo.forEach(insumo => {
                    comboInsumosMartillo.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Barra && listaInsumos.Barra.length > 0) {
                listaInsumos.Barra.forEach(insumo => {
                    comboInsumosBarra.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Barra && listaInsumos.Barra.length > 0) {
                listaInsumos.Barra.forEach(insumo => {
                    comboInsumosBarraSegunda.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Culata && listaInsumos.Culata.length > 0) {
                listaInsumos.Culata.forEach(insumo => {
                    comboInsumosCulata.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Cilindro && listaInsumos.Cilindro.length > 0) {
                listaInsumos.Cilindro.forEach(insumo => {
                    comboInsumosCilindro.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Zanco && listaInsumos.Zanco.length > 0) {
                listaInsumos.Zanco.forEach(insumo => {
                    comboInsumosZanco.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }
        }

        function limpiarCombos() {
            comboInsumosBroca.empty();
            comboInsumosBroca.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosMartillo.empty();
            comboInsumosMartillo.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosBarra.empty();
            comboInsumosBarra.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosBarraSegunda.empty();
            comboInsumosBarraSegunda.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosCulata.empty();
            comboInsumosCulata.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosCilindro.empty();
            comboInsumosCilindro.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosZanco.empty();
            comboInsumosZanco.append(`<option value="0" >Seleccione Insumo</option>`);
        }

        function fnLimpiarComboPiezas() {
            comboPiezasBroca.empty();
            comboPiezasBroca.append(`<option selected value="0">Nueva Pieza</option>`);
            comboPiezasMartillo.empty();
            comboPiezasMartillo.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasBarra.empty();
            comboPiezasBarra.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasBarraSegunda.empty();
            comboPiezasBarraSegunda.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasCulata.empty();
            comboPiezasCulata.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasCilindro.empty();
            comboPiezasCilindro.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasZanco.empty();
            comboPiezasZanco.append(`<option value="0" >Nueva Pieza</option>`);
        }
        //#endregion

        //#region Modal Agregar Banco
        function limpiarModal() {
            SetIdDefault();
            inputDescripcionBanco.val('');
            inputBanco.val('');
            inputAreaCuentaBanco.val(comboAC.val());
        }

        function GuardarBanco() {
            const nuevoBanco = ItemBanco();
            $.blockUI({ message: 'Guardando Banco...' });
            $.post('/Barrenacion/AgregarBanco', { nuevoBanco })
                .always($.unblockUI)
                .then(r => {
                    if (r.success) {
                        CheckModalShown();
                        AlertaGeneral(`Éxito`, `El banco ha sido actualizado con exito.`);
                        comboBanco.fillCombo('/Barrenacion/GetComboBancos', { areaCuenta: comboAC.val() }, false);
                    }
                    else {
                        CheckModalShown();
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    CheckModalShown();
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function ItemBanco() {
            return {
                id: idBanco,
                banco: inputBanco.val(),
                descripcion: inputDescripcionBanco.val(),
                areaCuenta: inputAreaCuentaBanco.val(),
                estatus: true,
                fechaCreacion: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                usuarioCreadorID: 0
            };
        }

        function SetIdDefault() {
            idBanco = 0;
        }

        function CheckModalShown() {
            if ((modalNuevoBanco.data('bs.modal') || {}).isShown)
                modalNuevoBanco.modal('hide');
        }
        //#endregion

        //#region Nuevo Guardado de Piezas
        function fnEnable() {
            $('.btnEditInsumo').val('').addClass('btnAceptarInsumo');
            $('.btnEditInsumo').val('').removeClass('btnEditInsumo');
            $('.comboPieza').val('');
            $('.inputNoSerie').val('').prop('disabled', false);
            $('.inputSerie').val('').prop('disabled', false);
            $('.inputPrecio').val('').prop('disabled', false);
            $('.quitarPieza').val('');
            $('.inputNoSerie').data().piezaID = 0;
            $('.spanSizeRadio').children('input').attr('disabled', false).prop('checked', false);
            $('.comboPieza').attr('disabled', false);
            $('.comboInsumo').attr('disabled', false);
        }

        function clickCheckBox() {
            var $box = $(this);
            if ($box.is(":checked")) {
                var group = "input:checkbox[name='" + $box.attr("name") + "']";
                $(group).prop("checked", false);
                $box.prop("checked", true);
                setInfoEdit($box);
            } else {
                $box.prop("checked", false);
                rollbackPieza($box);
            }
        }

        function setInfoEdit(box) {
            let activa = $(box).val();
            let divID = $(box).attr('name').split('-')[1];
            let div = $(`#div${divID}`);
            if (div.find('.quitarPieza').val() == '') {
                if (div.attr('id') == "divMartillo") {
                    addPiezaTempEdit(box, divMartillo);
                    boxCilindro = box.val() == "1" ? $(divCilindro.find('input:checkbox')[0]).prop('checked', true) : $(divCilindro.find('input:checkbox')[1]).prop('checked', true);
                    addPiezaTempEdit(boxCilindro, divCilindro);
                    boxCulata = box.val() == "1" ? $(divCulata.find('input:checkbox')[0]).prop('checked', true) : $(divCulata.find('input:checkbox')[1]).prop('checked', true);
                    addPiezaTempEdit(boxCulata, divCulata);
                }
                else {
                    addPiezaTempEdit(box, div);
                }
            }
            else {
                let piezaEdit = div.find('.quitarPieza').data().piezaEditada;
                switch (activa) {
                    case 1: //Desmontar
                        piezaEdit.activa = true;
                        piezaEdit.montada = false;
                        break;
                    case 2: //Deshecho
                        piezaEdit.activa = false;
                        piezaEdit.montada = false;
                        break;
                    case 3: //Reparacion
                        piezaEdit.activa = true;
                        piezaEdit.montada = false;
                        piezaEdit.reparando = true;
                        break;
                    default:
                        break;
                }
                div.find('.quitarPieza').data().piezaEditada = piezaEdit;
            }
        }

        function addPiezaTempEdit(box, div) {
            let activa = +$(box).val();
            let piezaID = div.find('.inputNoSerie').data().piezaID;

            if (piezaID != 0) {
                _listaPiezas.filter(function (pieza) {
                    return pieza.id == piezaID;
                }).forEach(piezaEdit => {
                    if (piezaEdit.id == piezaID) {
                        switch (activa) {
                            case 1: //Desmontar
                                piezaEdit.activa = true;
                                piezaEdit.montada = false;
                                fnSetPiezaEdit(div, piezaEdit);
                                break;
                            case 2: //Deshecho
                                piezaEdit.activa = false;
                                piezaEdit.montada = false;
                                fnSetPiezaEdit(div, piezaEdit);
                                break;
                            case 3: //Reparacion
                                piezaEdit.activa = true;
                                piezaEdit.montada = false;
                                piezaEdit.reparando = true;
                                break;
                            default:
                                break;
                        }
                    }
                });
            }
            else {
                $(box).prop('checked', false);
            }
        }

        function fnSetPiezaEdit(div, piezaEdit) {
            div.find('.quitarPieza').val(div.find('.inputNoSerie').val());
            div.find('.quitarPieza').data().piezaEditada = piezaEdit;
            div.find('.inputNoSerie').val('');
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputNoSerie').data().piezaID = 0;
            div.find('select').attr('disabled', false).val(0);
            div.find('.inputPrecio').attr('disabled', false);
            div.find('.inputPrecio').val('');
            div.find('.inputSerie').val('');
            div.find('.inputSerie').attr('disabled', false);
            div.find('.comboPieza').val('');
        }

        function rollbackPieza(box) {
            let divID = $(box).attr('name').split('-')[1];
            let div = $(`#div${divID}`);
            if (div.attr('id') == "divMartillo") {
                unSetPieza(div);
                unSetPieza(divCilindro);
                unSetPieza(divCulata);
            }
            else {
                unSetPieza(div);
            }
        }

        function unSetPieza(div) {
            let piezaTemp = div.find('.quitarPieza').data().piezaEditada;

            if (piezaTemp != undefined) {
                _listaPiezas = _listaPiezas.filter(function (pieza) {
                    if (div.find('.inputNoSerie') != pieza.noSerie) {
                        return pieza;
                    }
                });

                _listaPiezas.forEach(pieza => {
                    if (pieza.id == piezaTemp.id) {
                        pieza.activa = true;
                        pieza.montada = true;
                        pieza.reparando = false;
                    }
                });

                let objTemp = _listaPiezas.find(pieza => {
                    if (pieza.id == piezaTemp.id) {
                        return pieza;
                    }
                });
                setPieza(div, objTemp);
            }
        }

        function setPieza(div, pieza) {
            div.find('.inputNoSerie').val(pieza.noSerie);
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputNoSerie').data().piezaID = pieza.id;
            div.find('select').attr('disabled', false).val(pieza.insumo);
            div.find('.inputPrecio').attr('disabled', false);
            div.find('.inputPrecio').val(pieza.precio);
            div.find('.inputSerie').val(pieza.serialExcel);
            div.find('.inputSerie').attr('disabled', false);
            div.find('.comboPieza').val(pieza.id);
            div.find('.quitarPieza').data().piezaEditada = null;
            div.find('.quitarPieza').val('');
            div.find('input:checkbox').attr('disabled', false).prop('checked', false);
        }

        function editInsumo() {
            if ($(this).hasClass('btnEditInsumo')) {
                $(this).removeClass('btnEditInsumo').addClass('btnAceptarInsumo');
                $(this).parents('div.seccion').find('.divEditForm').removeClass('hide');
                if ($(this).parents('div.seccion').attr('id') == "divMartillo") {
                    $(divCulata).find('.divEditForm').removeClass('hide');
                    $(divCilindro).find('.divEditForm').removeClass('hide');
                    botonEditCulata.removeClass('btnEditInsumo').addClass('btnAceptarInsumo');//.prop('disabled', true);
                    botonEditCilindro.removeClass('btnEditInsumo').addClass('btnAceptarInsumo');//.prop('disabled', true);
                }
                fnDisabledEdit($(this));
            }
            else {
                if (getValidar($(this))) {
                    $(this).addClass('btnEditInsumo').removeClass('btnAceptarInsumo');
                    let divID = $(this).parents('div.seccion').attr('id');
                    disabledInputInsumo(divID);
                }
                else {
                    AlertaGeneral('Alerta:', `Es necesaria Tener la informacíon del Pieza: ${$(this).parent().find('label').text()}`);
                }
            }
        }

        function fnDisabledEdit(btnThis) {
            $btnThis = $(btnThis);
            let divID = getTipoPieza($btnThis); // Nombre de la pieza.
            let div = $(btnThis).parents('.seccion');
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputSerie').attr('disabled', false);
            div.find('select').attr('disabled', false);
            div.find('.inputPrecio').attr('disabled', false);
            div.find('.comboPieza').attr('disabled', false);
            div.find('.spanSizeRadio').children('input').attr('disabled', false);
        }

        function getTipoPieza(div) {
            const arrayDivsInputs = ['Broca', 'Martillo', 'Barra', 'Culata', 'Portabit', 'Cilindro', 'BarraSegunda', 'Zanco'];
            const tipoPieza = div.attr('tipoPieza') - 1;

            return arrayDivsInputs[tipoPieza];
        }

        function getValidar(elemento) {
            let divID = $(elemento).parents('div.seccion').attr('id');

            switch (divID) {
                case "divBarraSegunda":
                    return true;
                case "divBroca":
                case "divBarra":
                case "divZanco":
                    return validarInsumo($(`#${divID}`));
                case "divCulata":
                case "divCilindro":
                case "divMartillo":
                    if (!validarInsumo(divMartillo))
                        return false;
                    if (!validarInsumo(divCulata))
                        return false;
                    if (!validarInsumo(divCilindro))
                        return false;
                    else
                        return true;
                default:
                    break;
            }
        }

        function validarInsumo(div) {
            let inputNoSerie = $(div.find('.inputNoSerie'));
            let comboInsumo = $(div.find('comboInsumo'));
            let inputPrecio = $(div.find('inputPrecio'));

            if (inputNoSerie.val().trim(' ') == '' && inputNoSerie.val().trim(' ').length == 0)
                return false;
            if (comboInsumo.val() == 0)
                return false
            if (inputPrecio.val() == 0)
                return false
            else
                return true;
        }

        function disabledInputInsumo(divID) {

            let div = $(`#${divID}`);
            switch (divID) {
                case "divBarraSegunda":
                case "divBroca":
                case "divBarra":
                case "divZanco":
                    disabledInputs(div);
                    break;
                case "divCulata":
                case "divCilindro":
                case "divMartillo":
                    div = $("#divMartillo");
                    botonEditCulata.removeClass('btnAceptarInsumo').addClass('btnEditInsumo');
                    disabledInputs(div);
                    div = $("#divCulata");
                    botonEditCilindro.removeClass('btnAceptarInsumo').addClass('btnEditInsumo');
                    disabledInputs(div);
                    div = $("#divCilindro");
                    disabledInputs(div);
                    break;
                default:
                    break;
            }
        }

        function disabledInputs(div) {
            let tipoPieza = div.attr('tipoPieza');
            let valorInput = div.find('input:checked').val() ?? "1";

            var existe = _listaPiezas.filter(function (pieza) {
                if (pieza.noSerie == div.find('.inputNoSerie').val()) {
                    return pieza;
                }
            });
            if (existe.length == 0) {
                piezaEditada = {
                    id: +div.find('.comboPieza').val(),
                    noSerie: div.find('.inputNoSerie').val(),
                    insumo: div.find('.comboInsumo').val(),
                    tipoPieza: tipoPieza,
                    tipoBroca: tipoPieza == 3 ? 1 : tipoPieza == 7 ? 2 : 0,
                    barraSegunda: tipoPieza == 7 ? true : false,
                    horasTrabajadas: 0,
                    horasAcumuladas: 0,
                    reparando: false,
                    activa: true,
                    montada: true,
                    cilindroID: getInfoMartillo(tipoPieza).cilindroID,
                    culataID: getInfoMartillo(tipoPieza).culataID,
                    serialExcel: div.find('.inputSerie').val(),
                    precio: div.find('.inputPrecio').val(),
                    areaCuenta: comboAC.val(),
                    barrenadoraID: _barrenadoraID
                };
                _listaPiezas.push(piezaEditada);
            }
            div.find('.inputNoSerie').attr('disabled', true);
            div.find('.inputSerie').attr('disabled', true);
            div.find('select').attr('disabled', true);
            div.find('.inputPrecio').attr('disabled', true);
            div.find('.comboPieza').attr('disabled', true);
            div.find('.spanSizeRadio').children('input').attr('disabled', true);
        }

        function getInfoMartillo(tipoPieza) {
            var obj = {};
            if (tipoPieza == 2) {
                obj.cilindroID = divCilindro.find('.inputNoSerie').data().piezaID;
                obj.culataID = divCulata.find('.inputNoSerie').data().piezaID;
            }
            else {
                obj.cilindroID = 0;
                obj.culataID = 0;
            }

            return obj;
        }

        $('.comboPieza').on('change', function () {
            let piezaObj = $(this).val();
            let div = $(this).parents('div.seccion');
            let insumo = div.find('.comboInsumo').val();
            limpiarDiv(div);
            if (piezaObj == "0") {

                $.post('/Barrenacion/GetPiezaNueva', { insumo: insumo, areaCuenta: comboAC.val() }).then(response => {
                    if (response.success) {
                        div.find('.inputNoSerie').val(response.noSerie);
                        div.find('.inputPrecio').val(response.precioInsumo);

                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información de la pieza nueva.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
            else {
                $.get('/Barrenacion/GetPiezaID', { piezaID: piezaObj }).then(response => {
                    if (response.success) {
                        let pieza = response.pieza;
                        div.find('.inputNoSerie').val(pieza.noSerie.trim(' '));
                        div.find('.inputNoSerie').attr('disabled', false);
                        div.find('.inputSerie').val(pieza.serialExcel).attr('disabled', false);
                        div.find('.inputNoSerie').data().piezaID = pieza.id;
                        div.find('select').attr('disabled', false).val(pieza.insumo);
                        div.find('.inputPrecio').val(maskNumero(pieza.precio));
                        div.find('.inputPrecio').attr('disabled', false);
                        div.find('.comboPieza').val(pieza.id).attr('disabled', false);
                        div.find('.spanSizeRadio').children('input').attr('disabled', false);
                        div.find('.comboInsumo').val(pieza.insumo);

                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información de la pieza nueva.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });

            }
        });
        function limpiarDiv(div) {
            div.find('.inputNoSerie').val('');
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputSerie').val('');
            div.find('.inputNoSerie').data().piezaID = 0;
            div.find('.inputPrecio').val('');
        }
        //#endregion
    }
    $(() => Barrenacion.CapturaDiaria = new CapturaDiaria())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();