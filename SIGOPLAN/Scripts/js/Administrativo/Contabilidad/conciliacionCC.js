(() => {
    $.namespace('ConciliacionCC.conciliacionCC');
    conciliacionCC = function () {

        // const idEmpresa = $("#idEmpresa");
        const cboCCPal = $("#cboCCPal");
        const cboCCSec = $("#cboCCSec");
        const CCPrincipal = $("#CCPrincipal");
        const CCSecundario = $("#CCSecundario");
        const tblConciliacionCC = $("#tblConciliacionCC");
        const botonBuscar = $("#botonBuscar");
        const botonGuardar = $("#botonGuardar");
        const botonNuevo = $("#botonNuevo");
        const modalRelacionCC = $("#modalRelacionCC");
        let catCC = [], optCCPal = [], optCCSec = [], busq, btnNuevo, dtConciliacionCC;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            fncListeners();
        })();


        function fncListeners() {
            initTblConciliacionCC();
            CargarCombosCC();
            GetBuscarConciliacionCC();

            botonNuevo.click(() => {
                limpiarModal();
                CargarCombosCC();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                modalRelacionCC.modal('show');
            });
            botonGuardar.click(guardarRelacionCC);
        }

        function CargarCombosCC() {
            cboCCPal.fillCombo('/Administrativo/ConciliacionCC/FillCCPrincipal', { est: true }, false, "Todos");
            convertToMultiselect("#cboCCPal");
            cboCCSec.fillCombo('/Administrativo/ConciliacionCC/FillCCSecundario', { est: true }, false, "Todos");
            convertToMultiselect("#cboCCSec");
            CCPrincipal.fillCombo('/Administrativo/ConciliacionCC/FillCCPrincipal', { est: true }, false, null);
            // convertToMultiselect("#CCPrincipal");
            CCSecundario.fillCombo('/Administrativo/ConciliacionCC/FillCCSecundario', { est: true }, false, null);
            // convertToMultiselect("#CCSecundario");

        }
        botonBuscar.click(function () {
            GetBuscarConciliacionCC(cboCCPal.val());
        })

        function limpiarModal() {
            CCPrincipal.val('');
            CCSecundario.val('');


        }

        function initTblConciliacionCC() {
            dtConciliacionCC = tblConciliacionCC.DataTable({
                destroy: true,
                language: dtDicEsp,
                dom: 'f<"toolbar">rtip',
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'ccPrincipal', title: 'PERÚ', visible: false },
                    { data: 'descripcionCCPrincipal', title: 'PERÚ' },
                    { data: 'ccSecundario', title: 'CONSTRUPLAN', visible: false },
                    { data: 'descripcionCCSecundario', title: 'CONSTRUPLAN' },
                    {
                        data: 'propiedad', title: 'Opciones ',
                        render: (data, type, row, meta) => {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {

                    tblConciliacionCC.on('click', '.editarRegistro', function () {
                        let rowData = dtConciliacionCC.row($(this).closest('tr')).data();
                        limpiarModal();
                        console.log(rowData);
                        // llenarCamposModal(rowData);
                        CCPrincipal.val(rowData.ccPrincipal);
                        CCPrincipal.trigger("change");
                        CCSecundario.val(rowData.ccSecundario);
                        CCSecundario.trigger("change");
                        botonGuardar.data().estatus = ESTATUS.NUEVO;
                        botonGuardar.data().id = rowData.id;
                        modalRelacionCC.modal('show');
                    });
                    tblConciliacionCC.on('click', '.eliminarRegistro', function () {
                        let rowData = dtConciliacionCC.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarConciliacionCC(rowData.id));
                    });
                },
            });
        }
        // function llenarCamposModal(data) {
        //     CCPrincipal.val(data.ccPrincipal);
        //     CCPrincipal.text(data.descripcionCCPrincipal);
        //     CCSecundario.val(data.ccSecundario);
        //     CCSecundario.text(data.descripcionCCSecundario);
        // }
        function guardarRelacionCC() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    fncGuardarEditarConciliacionCC();
                    break;
                case ESTATUS.EDITAR:
                    fncGuardarEditarConciliacionCC();
                    break;
            }
        }


        function fncGuardarEditarConciliacionCC() {
            let relacionCC = getInformacionRelacionCC();
            $.ajax({
                type: "POST",
                url: '/Administrativo/ConciliacionCC/GuardarEditarConciliacionCC',
                data: relacionCC,
                success: function (success) {
                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalRelacionCC.modal("hide");
                        GetBuscarConciliacionCC();
                    } else {
                        AlertaGeneral(`Alerta`, error.message);
                        modalRelacionCC.modal("hide");
                    }
                },
                error: function () {
                    alert('Failed');
                }
            })
                .catch(error => Alert2Error(error.message));
        }
        function GetBuscarConciliacionCC() {
            axios.post('GetBuscarConciliacionCC', { cboCCPal: getValoresMultiples("#cboCCPal") }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtConciliacionCC.clear();
                    dtConciliacionCC.rows.add(items);
                    dtConciliacionCC.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        function eliminarConciliacionCC(id) {
            axios.post('EliminarConciliacionCC', { id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la información.');
                        GetBuscarConciliacionCC();

                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function getInformacionRelacionCC() {
            return {
                id: botonGuardar.data().id,
                // descripcionCCPrincipal: CCPrincipal.val(),
                // descripcionCCSecundario: CCSecundario.val(),
                ccPrincipal: $("#CCPrincipal option:selected").val(),
                descripcionCCPrincipal: $("#CCPrincipal option:selected").text(),
                ccSecundario: $("#CCSecundario option:selected").val(),
                descripcionCCSecundario: $("#CCSecundario option:selected ").text(),

            };
        }

    }
    $(document).ready(() => {
        ConciliacionCC.conciliacionCC = new conciliacionCC();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();