(() => {
    $.namespace('Capacitacion.FirmasInstructores');

    //#region CONST FILTROS
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    const btnFiltroNuevo = $("#btnFiltroNuevo")
    //#endregion

    //#region CONST FIRMAS INSTRUCTORES
    let dtFirmasInstructores
    const tblFirmasInstructores = $('#tblFirmasInstructores')
    const mdlCEFirmaInstructor = $('#mdlCEFirmaInstructor')
    const cboCE_Usuario = $('#cboCE_Usuario')
    const txtCE_Firma = $('#txtCE_Firma')
    const btnCE_FirmaInstructor = $('#btnCE_FirmaInstructor')
    //#endregion

    FirmasInstructores = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblFirmasInstructores()
            cboCE_Usuario.fillCombo('FillCboUsuarios', null, false, null);
            $(".select2").select2()
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncGetFirmasInstructores()
            })

            btnFiltroNuevo.click(function () {
                fncLimpiarCEFirmaInstructor()
                btnCE_FirmaInstructor.html(`<i class='fas fa-save'></i> Guardar`)
                btnCE_FirmaInstructor.data().id = 0
                mdlCEFirmaInstructor.modal("show")
            })
            //#endregion

            //#region FIRMAS INSTRUCTORES
            btnCE_FirmaInstructor.click(function () {
                fncCEFirmaInstructor()
            })
            //#endregion
        }

        //#region CATALOGO FIRMAS INSTRUCTORES
        function initTblFirmasInstructores() {
            dtFirmasInstructores = tblFirmasInstructores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreCompleto', title: 'Instructor' },
                    {
                        title: "Opciones",
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblFirmasInstructores.on('click', '.editarRegistro', function () {
                        let rowData = dtFirmasInstructores.row($(this).closest('tr')).data();
                        fncGetDatosActualizarInstructor(rowData.id);
                    });
                    tblFirmasInstructores.on('click', '.eliminarRegistro', function () {
                        let rowData = dtFirmasInstructores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFirmaInstructor(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '7%', targets: [1] }
                ],
            });
        }

        function fncGetFirmasInstructores() {
            axios.post('GetFirmasInstructores').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtFirmasInstructores.clear();
                    dtFirmasInstructores.rows.add(response.data.lstFirmasInstructores);
                    dtFirmasInstructores.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEFirmaInstructor() {
            if (cboCE_Usuario.val() > 0) {
                var data = fncGetEvidenciaParaGuardar();
                let obj = new Object();
                obj.FK_Usuario = cboCE_Usuario.val()
                axios.post('CEFirmaInstructor', data, { params: FK_Usuario = obj }, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        btnFiltroBuscar.trigger("click")
                        Alert2Exito(message)
                        mdlCEFirmaInstructor.modal("hide")
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                if (cboCE_Usuario.val() <= 0) {
                    Alert2Warning("Es necesario seleccionar un instructor.")
                }
            }
        }

        function fncEliminarFirmaInstructor(idFirma) {
            if (idFirma > 0) {
                let obj = {}
                obj.id = idFirma
                axios.post('EliminarFirmaInstructor', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnFiltroBuscar.trigger("click")
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (idFirma <= 0) {
                    Alert2Warning("Ocurrió un error al eliminar la firma del instructor.")
                }
            }
        }

        function fncGetEvidenciaParaGuardar() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("txtCE_Firma").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            return data;
        }

        function fncLimpiarCEFirmaInstructor() {
            cboCE_Usuario[0].selectedIndex = 0
            cboCE_Usuario.trigger("change")
            txtCE_Firma.val("")
            btnCE_FirmaInstructor.data().id = 0
        }
        //#endregion
    }

    $(document).ready(() => {
        Capacitacion.FirmasInstructores = new FirmasInstructores();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();