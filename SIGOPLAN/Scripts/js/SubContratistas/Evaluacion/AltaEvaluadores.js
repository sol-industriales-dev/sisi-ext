(() => {
    $.namespace('evaluadorxProyectos.EvaluacionSubcontratista');

    //#region CONST
    const tblTablaEvaluadorxCC = $('#tblTablaEvaluadorxCC');
    const mdlAgregarEditar = $('#mdlAgregarEditar');
    const cboFiltroEvaluadores = $('#cboFiltroEvaluadores');
    const cboSubContratista = $('#cboSubContratista');
    const btnNuevo = $('#btnNuevo');
    const cboElemento = $('#cboElemento');
    const btnGuardarPreguntarPrimero = $('#btnGuardarPreguntarPrimero');
    const btnBuscar = $('#btnBuscar');
    const cboProyecto = $('#cboProyecto');
    const cboProyecto2 = $('#cboProyecto2');
    const cboEvaluadores = $('#cboEvaluadores');
    const cboElementos = $('#cboElementos');
    let dtTablaEvaluadorxCC
    //#endregion

    EvaluacionSubcontratista = function () {
        let init = () => {
            initTablaEvaluadorxCC();
            getEvaluadoresxCC();
            llenarCombos();
        }
        init();
    }

    function llenarCombos() {
        btnBuscar.click(function () {
            getEvaluadoresxCC();
        });

        btnNuevo.click(function () {
            $('.modal-title').text('NUEVO EVALUADOR');
            btnGuardarPreguntarPrimero.attr('data-id', 0);
            cboSubContratista.val('')
            cboSubContratista.trigger('change');
            cboSubContratista.prop('disabled', false);
            cboProyecto.val('')
            cboProyecto.trigger('change');
            cboElemento.val('')
            cboElemento.trigger('change');
        });

        cboFiltroEvaluadores.fillCombo('FillCboFiltroEvaluadores', null, null);
        cboFiltroEvaluadores.select2();

        cboProyecto.fillCombo('getProyectoRestantes?Agregar=' + false, null, null);
        cboProyecto.select2({ dropdownParent: $(mdlAgregarEditar) });

        cboSubContratista.fillCombo('FillCboCEEvaluadores', null, null);
        cboSubContratista.select2();

        cboProyecto2.fillCombo('cboProyecto', null, null);
        cboProyecto2.select2();
        cboEvaluadores.fillCombo('cboEvaluador', null, null);
        cboEvaluadores.select2();
        cboElementos.fillCombo('cboElementos', null, null);
        cboElementos.select2();

        btnGuardarPreguntarPrimero.click(function () {
            guardarEditar();
        });

        cboElemento.fillCombo('obtenerTodolosElementos', null, null);
        cboElemento.select2();
    }

    function getParameters() {
        let arrae = getValoresMultiples('#cboProyecto');
        let lstElementos = getValoresMultiples('#cboElemento');

        // console.log(arrae)
        let arrCC = '';
        arrae.forEach(element => {
            if (arrCC == "") {
                arrCC = element;
            } else {
                arrCC += `, ${element}`;
            }
        });

        let arrElementos = '';
        lstElementos.forEach(element => {
            if (arrElementos == "") {
                arrElementos = element;
            } else {
                arrElementos += `, ${element}`;
            }
        });

        let parametros = {
            cc: arrCC,
            lstElem: arrElementos,
            evaluador: cboSubContratista.val(),
            id: btnGuardarPreguntarPrimero.attr('data-id')
        }
        return parametros;
    }
    function guardarEditar() {
        let parametros = getParameters();
        if (cboSubContratista.val() != "" && cboProyecto.val() != '') {
            axios.post('AgregarEditarEvaluadores', { parametros: parametros })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        mdlAgregarEditar.modal('hide');
                        getEvaluadoresxCC();
                    }
                });
        } else {
            let strMensajeError = "";
            cboSubContratista.val() == "" ? strMensajeError += "Es necesario indicar a un evaluador." : "";
            cboProyecto.val() == "" ? strMensajeError += "<br>Es necesario indicar al menos un proyecto" : "";
            Alert2Warning(strMensajeError);
        }
    }
    function getEvaluadoresxCC() {

        let cc = cboProyecto2.val() == null ? "" : cboProyecto2.val().toString();
        let elemento = cboElementos.val() == null ? "" : cboElementos.val().toString();
        let evaluadores = (cboEvaluadores.val() == null || cboEvaluadores.val() == "" ? 0 : cboEvaluadores.val());

        // console.log(cc);
        // console.log(elemento);
        // console.log(evaluadores);

        axios.post('getEvaluadoresxCC', { cc: cc, elemento: elemento, evaluadores: evaluadores })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblTablaEvaluadorxCC, items);
                }
            });
    }
    function initTablaEvaluadorxCC() {
        dtTablaEvaluadorxCC = tblTablaEvaluadorxCC.DataTable({
            destroy: true,
            language: dtDicEsp,
            paging: true,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            scrollX: true,
            scrollY: '45vh',
            scrollCollapse: true,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'evaluador', title: 'evaluador', visible: false },
                { data: 'nombreEvaluador', title: 'EVALUADOR' },
                { data: 'cc', title: 'cc', visible: false },
                {
                    data: 'lstCC', title: 'PROYECTOS', render: (data, type, row, meta) => {
                        let cont = 0;
                        let html = ``;
                        if (data[0] == "") {
                            html += '';
                        } else {
                            data.forEach(x => {
                                cont++;
                                if (x != " - ") {
                                    html += `<span class=' btn-primary displayCC'>&nbsp;<i class='fab fa-creative-commons-nd'></i>${x}</span>&nbsp; `;
                                    html += '</br>';
                                }
                            });
                        }
                        return html;
                    }
                },
                { data: 'lstElem', title: 'cc', visible: false },
                {
                    data: 'lstElementos', title: 'ELEMENTOS', render: (data, type, row, meta) => {
                        let cont = 0;
                        let html = ``;
                        if (data[0] == "") {
                            html += '';
                        } else {
                            data.forEach(x => {
                                cont++;
                                if (x != " - ") {
                                    html += `<span class=' btn-primary displayCC'>&nbsp;<i class='fab fa-creative-commons-nd'></i>${x}</span>&nbsp; `;
                                    html += '</br>';
                                }
                            });
                        }
                        return html;
                    }
                },
                { data: 'esActivo', title: 'esActivo', visible: false },
                {
                    render: (data, type, row, meta) => {
                        let html = '';
                        html += `<button class='btn btn-warning btn-xs EditarAgrupacion' data-esActivo="1" data-id="${row.id}" title="Actualizar evaluador."><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                        html += `<button class='btn btn-danger btn-xs EliminarAgrupacion' data-esActivo="0" data-id="${row.id}" title="Eliminar evaluador."><i class="fas fa-trash"></i></button>`;
                        return html;
                    }
                },
            ], initComplete: function (settings, json) {
                tblTablaEvaluadorxCC.on("click", ".EliminarAgrupacion", function () {
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

                tblTablaEvaluadorxCC.on("click", ".EditarAgrupacion", function () {
                    const rowData = dtTablaEvaluadorxCC.row($(this).closest("tr")).data();
                    mdlAgregarEditar.modal('show');
                    btnGuardarPreguntarPrimero.attr('data-id', rowData.id)
                    cboSubContratista.val(rowData.evaluador);
                    cboSubContratista.trigger('change');
                    let cc = [];
                    let arreglo = rowData.cc.split(',');
                    arreglo.forEach(element => {
                        if (element != "") {
                            cc.push(element);
                        }
                    });
                    let lstElemen = [];

                    let arreglo2 = rowData.lstElem.split(',');
                    arreglo2.forEach(element => {
                        if (element != "") {
                            lstElemen.push(element);
                        }
                    });
                    cboSubContratista.text(rowData.nombreEvaluador);
                    // $("#cboSubContratista").prop('disabled', 'enabled');
                    // console.log(cc)
                    cboProyecto.fillCombo('getProyectoRestantes?Agregar=' + false, null, false, null);
                    cboProyecto.val(cc);
                    cboProyecto.trigger('change');
                    cboElemento.val(lstElemen);
                    cboElemento.trigger('change');
                    $('.modal-title').text('MODIFICAR EVALUADORES');
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                //{ className: 'dt-body-center', targets: [0] },
                //{ className: 'dt-body-right', targets: [0] },
                //{ width: '5%', targets: [0] }
            ],
        });
    }

    var fncEliminar = function (id) {
        axios.post('ActivarDesactivarEvaluadores', { id: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.estatus == 1) {
                        Alert2Exito(items.mensaje);
                        getEvaluadoresxCC();
                    } else {
                        Alert2Warning(item.mensaje)
                    }
                }
            });
    }
    function AddRows(tbl, lst) {
        dtTablaEvaluadorxCC = tbl.DataTable();
        dtTablaEvaluadorxCC.clear().draw();
        dtTablaEvaluadorxCC.rows.add(lst).draw(false);
    }











    $(document).ready(() => {
        evaluadorxProyectos.EvaluacionSubcontratista = new EvaluacionSubcontratista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();