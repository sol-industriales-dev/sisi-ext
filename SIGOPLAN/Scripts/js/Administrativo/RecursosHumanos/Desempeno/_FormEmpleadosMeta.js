(() => {
    $.namespace('RecursosHumanos.Desempeno._FormEmpleadosMeta');
    _FormEmpleadosMeta = function () {
        let lstProcesos = [];
        const mdlFormPeso = $('#mdlFormPeso');
        const tblEmpleadosMeta = $('#tblEmpleadosMeta');
        const mdlFormMetaEvaluar = $('#mdlFormMetaEvaluar');
        const lblMetaEvaluarEmplNombre = $('#lblMetaEvaluarEmplNombre');
        const getCboProceso = new URL(window.location.origin + '/Administrativo/Desempeno/getCboProceso');
        const getEmpleadosJefe = new URL(window.location.origin + '/Administrativo/Desempeno/getEmpleadosJefe');
        let init = () => {
            setCboProceso();
        }
        setCboProceso = async () => {
            try {
                response = await ejectFetchJson(getCboProceso);
                if (response.success) {
                    lstProcesos = response.items;
                    initDataTblEmpleadosMeta();
                    setEmpleadosJefe();
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setEmpleadosJefe = async () => {
            try {
                dtEmpleadosMeta.clear().draw();
                response = await ejectFetchJson(getEmpleadosJefe);
                if (response.success) {
                    dtEmpleadosMeta.rows.add(response.lst).draw();
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        initDataTblEmpleadosMeta = () => {
            dtEmpleadosMeta = tblEmpleadosMeta.DataTable({
                destroy: true
                , language: dtDicEsp
                , columns: [
                    { data: 'nombre', title: "Nombre" }
                    , { data: 'puesto', title: "Puesto" }
                    , {
                        data: 'idEmpleado', title: "Pesos", createdCell: function (td, data, rowData, row, col) {
                            let lstUserProceso = lstProcesos.filter(option => option.Prefijo.includes(data)),
                                tieneProceso = lstUserProceso.length > 0,
                                div = $("<div>",
                                    {
                                        class: "input-group"
                                    }),
                                span = $("<span>", {
                                    class: "input-group-btn"
                                }),
                                cboProceso = $("<select>",
                                    {
                                        class: "form-control cboProceso"
                                    }),
                                btnProceso = $("<button>", {
                                    class: `btn btn-default btnProceso ${tieneProceso ? "" : "disabled"}`,
                                    html: '<i class="fa fa-edit"></i>'
                                });
                            cboProceso.fillComboItems(lstUserProceso, null, null);
                            span.append(btnProceso);
                            div.append(cboProceso).append(span);
                            $(td).html(div);
                        }
                    }
                    , {
                        data: 'idUsuario', title: "Calificar", width: "2%", createdCell: function (td, data, rowData, row, col) {
                            let lstUserProceso = lstProcesos.filter(option => option.Prefijo.includes(rowData.idEmpleado)),
                                tieneProceso = lstUserProceso.length > 0;
                            btnCalificar = $("<button>", {
                                class: `btn btn-default btnCalificar ${tieneProceso ? "" : "disabled"}  `,
                                html: "Evaluación "
                            });
                            $(td).html(btnCalificar);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblEmpleadosMeta.on('click', '.btnCalificar', function (event) {
                        let node = $(this).closest("tr"),
                            data = dtEmpleadosMeta.row(node).data();
                        setEvaMetaObservacion(data.idUsuario);
                        lblMetaEvaluarEmplNombre.text(data.nombre);
                        mdlFormMetaEvaluar.modal("show");
                    });
                    tblEmpleadosMeta.on('click', '.btnProceso', function (event) {
                        let node = $(this).closest("tr"),
                            data = dtEmpleadosMeta.row(node).data();
                        idProceso = +node.find(".cboProceso").val();
                        if (idProceso > 0) {
                            setLstMetaPesos(idProceso, data.idUsuario);
                            mdlFormPeso.modal("show");
                        }
                    });
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno._FormEmpleadosMeta = new _FormEmpleadosMeta();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();