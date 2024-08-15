(() => {
    $.namespace('maquinaria.KPI.CatalogoCodigoParo');

    CatalogoCodigoParo = function () {

        // Variables.
        let dtTablaCodigosParo;
        let idCodigo = 0;

        //Selectores Pantalla Principal
        const comboACFiltro = $('#comboACFiltro');
        const inputCodigoFiltro = $('#inputCodigoFiltro');
        const btnBuscar = $('#btnBuscar');
        const btnNuevo = $('#btnNuevo');
        const tablaCodigosParo = $('#tablaCodigosParo');

        //Selectores Modal Código Paro
        const comboAC = $('#comboAC');
        const modalCodigoParo = $('#modalCodigoParo');
        const lblTituloAccionCatalogoParo = $('#lblTituloAccionCatalogoParo');
        const inputCodigoModal = $('#inputCodigoModal');
        const inputDescripcionModal = $('#inputDescripcionModal');
        const cboTipoParoModal = $('#cboTipoParoModal');
        const cboEstatusModal = $('#cboEstatusModal');
        const btnGuardar = $('#btnGuardar');

        //Selectes Modal Eliminar Código Paro
        const modalElimiarCodigoParo = $('#modalEliminarCodigoParo');
        const lblCodigoSeleccionado = $('#lblCodigoSeleccionado');
        const btnEliminarCodigoParo = $('#btnEliminarCodigoParo');

        (function init() {

            // Lógica de inicialización.
            initTablaCodigoParo();
            initEventListener();
            fnCargarTablaCodigosParo();
            initCombos();

        })();

        // Métodos.
        function initCombos() {
            cboTipoParoModal.fillCombo('/KPI/getCboTiposParo', null, false);
            comboACFiltro.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            comboAC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
        }

        function initEventListener() {
            btnNuevo.click(fnNuevoCodigo);
            btnGuardar.click(fnGuardarCodigo);
            btnBuscar.click(fnCargarTablaCodigosParo);
            btnEliminarCodigoParo.click(fnEliminarCodigo);
        }

        //#region Modal Código de Paros
        function fnNuevoCodigo() {
            lblTituloAccionCatalogoParo.text('Nuevo Código de Paro');
            modalCodigoParo.modal('show');
        }

        function fnGuardarCodigo() {
            let codigoParo = setObjCodigoParo();
            if (codigoParo.valido) {
                fnAjaxCodigoParo(codigoParo, modalCodigoParo);
            }
        }

        function fnEliminarCodigo() {
            let data = dtTablaBancos.data().toArray().filter(r => r.codigo == idCodigo);
            fnAjaxCodigoParo(data, modalElimiarCodigoParo);
        }

        function fnAjaxCodigoParo(codigoParo, modal) {
            $.post('/KPI/GuardarCodigoParo', { codigoParo })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Operacion Exitosa`, `La operacion se realizó con exito.`);
                        modal.modal('hide');
                        fnLimpiarData();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function fnLimpiarData() {
            cboEstatusModal.val('true');
            inputCodigoModal.val('');
            inputDescripcionModal.val('');
            cboTipoParoModal.val('');
            idCodigo = 0;
        }

        function setObjCodigoParo() {

            let objCodigoParo = {};
            objCodigoParo.valido = true;
            if (inputCodigoModal.val() != "") {
                objCodigoParo.codigo = inputCodigoModal.val();
            }
            else {
                objCodigoParo.valido = false;
                return AlertaGeneral('Error', 'El campo de Código no puede estar vacío.');
            }

            if (inputDescripcionModal.val() != "") {
                objCodigoParo.descripcion = inputDescripcionModal.val();
            }
            else {
                return AlertaGeneral('Error', 'El campo de descripción no puede estar vacío.');
            }

            if (cboTipoParoModal.val() != "") {
                objCodigoParo.tipoParo = cboTipoParoModal.val();
            }
            else {
                return AlertaGeneral('Error', 'Se debe seleccionar un tipo de código.');
            }
            objCodigoParo.activo = cboEstatusModal.val();
            objCodigoParo.areaCuenta = comboAC.val();
            return objCodigoParo;
        }

        function fnCargarTablaCodigosParo() {
            $.get('/KPI/tlbCodigoParo', { codigoParo: inputCodigoFiltro.val() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        let dataCodigosParo = response.dataCodigosParo;
                        dtTablaCodigosParo.clear().draw();
                        if (dataCodigosParo != null) {
                            dtTablaCodigosParo.rows.add(dataCodigosParo).draw();
                        }
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
        //#endregion

        //#region Metodos Tabla Código Paro.
        function initTablaCodigoParo() {
            dtTablaCodigosParo = tablaCodigosParo.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: false,
                columns: [
                    { data: 'codigo', title: 'Código' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'tipoParo', title: 'Tipo Paro' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar"><i class="fas fa-tools"></i> editar</button>` },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-danger eliminar"><i class="fas fa-trash"></i> eliminar</button>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        tablaCodigosParo.on('click', '.editar', function () {
            let data = dtTablaCodigosParo.row($(this).parents('tr')).data();
            idCodigo = data.id;
            inputCodigoModal.val(data.banco);
            inputDescripcionModal.val(data.descripcion);
            cboTipoParoModal.val(data.tipoParo);
            cboEstatusModal.val(data.activo);
            inputCodigoModal.val(data.codigo)
            lblTituloAccionCatalogoParo.text(`Editar Código de Paro: ${data.codigo}`);
            modalCodigoParo.modal('show');
        });

        tablaCodigosParo.find('.eliminar').click(function () {
            let data = dtTablaCodigosParo.row($(this).parents('tr')).data();
            idCodigo = data.id;
            lblCodigoSeleccionado.text(data.codigo);
            modalElimiarCodigoParo.modal('show');
        });
    }

    $(() => maquinaria.KPI.CatalogoCodigoParo = new CatalogoCodigoParo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();