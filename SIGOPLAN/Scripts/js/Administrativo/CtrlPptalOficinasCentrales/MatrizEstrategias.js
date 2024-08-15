(() => {
	$.namespace('CtrlPresupuestalOfCE.MatrizEstrategias');

	//#region CONST
	const btnFiltroBuscar = $('#btnFiltroBuscar');
	const btnFiltroNuevoMatrizEstrategia = $('#btnFiltroNuevoMatrizEstrategia');
	const tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas = $('#tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas');
	let dtMatrizEstrategia;
	//#endregion

	//#region CONST MODAL CREAR/EDITAR MATRIZ ESTRATEGIAS
	const mdlCEMatrizEstrategia = $('#mdlCEMatrizEstrategia');
	const lblTitleCEMatrizEstrategia = $('#lblTitleCEMatrizEstrategia');
	const txtCuenta = $("#txtCuenta");
	const txtActividades = $("#txtActividades");
	const txtCantidad = $("#txtCantidad");
	const txtFechaReal = $("#txtFechaReal");
	const cboResponsable = $('#cboResponsable');
	const btnCEMatrizEstrategia = $('#btnCEMatrizEstrategia');
	const lblTitleBtnCEMatrizEstrategia = $('#lblTitleBtnCEMatrizEstrategia');
	//#endregion

	MatrizEstrategias = function () {
		(function init() {
			fncListeners();
		})();

		function fncListeners() {
			//#region INIT DATATABLES
			initTblMatrizEstrategias();
			//#endregion

			//#region 
			//TODO: GET REGISTROS

			btnFiltroNuevoMatrizEstrategia.on("click", function () {
                fncLimpiarMdlCE();
                fncTitleMdlCEMatrizEstrategia(true);
                mdlCEMatrizEstrategia.modal("show");
            });
			//#endregion

			//#region FILL COMBOS
			$(".select2").select2();
			cboResponsable.fillCombo("FillResponsablesCuentasLider", {}, false);
			cboResponsable.select2({ width: "100%" });
			//#endregion
		}

		//#region CRUD MATRIZ ESTRATEGIAS
		function initTblMatrizEstrategias() {
            dtMatrizEstrategia = tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'consecutivo', title: '#' },
                    { data: 'cuenta', title: 'Cuenta' },
                    { data: 'actividades', title: 'Actividades pendientes<br>o por realizar' },
                    { data: 'cantidad', title: '$' },
                    { data: 'responsable', title: 'Responsables' },
                    { 
                        data: 'fechaReal', title: 'Fecha real',
                        render: function (data, type, row) {
                            if (data != null) {
                                return moment(data).format("DD/MM/YYYY");
                            } else {
                                return "";
                            }
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-warning editarRegistro" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarRegistro" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas.on('click','.editarRegistro', function () {
                        let rowData = dtMatrizEstrategia.row($(this).closest('tr')).data();
                        fncGetDatosActualizarMatrizEstrategia(rowData.id);
                    });
                    tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas.on('click','.eliminarRegistro', function () {
                        let rowData = dtMatrizEstrategia.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarMatrizEstrategia(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }

		function fncTitleMdlCEMatrizEstrategia(esCrear) {
            if (esCrear) {
                lblTitleCEMatrizEstrategia.html("Nuevo registro");
                lblTitleBtnCEMatrizEstrategia.html("Guardar");
                btnCEMatrizEstrategia.attr("data-id", 0);
            } else {
                lblTitleCEMatrizEstrategia.html("Actualizar registro");
                lblTitleBtnCEMatrizEstrategia.html("Actualizar");
            }
        }
		//#endregion

		//#region FUNCIONES GENERALES
        function fncLimpiarMdlCE() {
            $('input[type="text"]').val("");
			txtActividades.val("");
        }

        function fncBorderDefault() {
            // $("#select2-cboResponsableLiderCuenta-container").css("border", "1px solid #CCC");
        }
        //#endregion
	}

	$(document).ready(() => {
		CtrlPresupuestalOfCE.MatrizEstrategias = new MatrizEstrategias();
	})
		.ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
		.ajaxStop(() => { $.unblockUI(); });
})();