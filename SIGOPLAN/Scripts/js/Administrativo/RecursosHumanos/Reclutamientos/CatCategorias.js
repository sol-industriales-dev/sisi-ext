(() => {
    $.namespace('CH.CatCategorias');

    //#region CONSTS
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroPuesto = $('#cboFiltroPuesto');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnCrearEmpleado = $('#btnCrearEmpleado');
    const titleMovimientos = $('#titleMovimientos');
    const btnEditar = $('#btnEditar');
    const btnFiltroAñadir = $('#btnFiltroAñadir');

    const tblPuestos = $('#tblPuestos');
    let dtPuestos;

    const tblPuestoDetalle = $('#tblPuestoDetalle');
    let dtPuestoDetalle;

    let numMovimientos = [];
    let lstPuestos;
    //#endregion

    //#region 
    const mdlAddPuesto = $('#mdlAddPuesto');
    const spanAddPuestoCC = $('#spanAddPuestoCC');
    const spanAddPuestoNombre = $('#spanAddPuestoNombre');
    const cboAddPuesto = $('#cboAddPuesto');
    const btnAddPuesto = $('#btnAddPuesto');
    //#endregion

    CatCategorias = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblPuestos();
            initTblPuestoDetalle();
            fncGetPuestoDetalle();

            //#region FILL COMBO
            cboFiltroCC.fillCombo('FillCboCC', {}, false);
            cboAddPuesto.fillCombo("GetAllPuestos", {}, false);

            cboFiltroCC.select2({ width: '100%' });
            cboAddPuesto.select2({ width: '100%' });

            cboFiltroCC.on("change", function () {

                if (cboFiltroCC.val() != "") {
                    cboFiltroPuesto.fillCombo("/Reclutamientos/FillComboPuestos", { cC: $(this).val() }, false);
                    cboFiltroPuesto.select2({ width: '100%' });

                    fncGetPuestoDetalle();
                }
                fncFocus("cboFiltroCC");
            });
            //#endregion

            btnFiltroBuscar.on("click", function () {
                fncGetPuestosCategorias();
                fncGetPuestoDetalle();
                numMovimientos = [];
                titleMovimientos.text("0");
            });

            cboFiltroPuesto.on("change", function () {
                // let optionPuestoNom = cboFiltroPuesto.find(`option[value="${cboFiltroPuesto.val()}"]`);
                // let prefijoNoEconomico = optionPuestoNom.attr("data-prefijo");
                fncGetPuestosCategorias();
                fncGetPuestoDetalle();
                numMovimientos = [];
                titleMovimientos.text("0");

                fncFocus("cboFiltroPuesto");
            });

            btnEditar.on("click", function () {

                if (cboFiltroCC.val() == "" || cboFiltroPuesto.val() == "") {
                    Alert2Warning("No hay cambios para guardar");
                } else {
                    fncEditarPlantilla();
                }
            });

            btnFiltroAñadir.on("click", function () {
                if (cboFiltroCC.val() != "" && cboFiltroPuesto.val() != "") {
                    mdlAddPuesto.modal("show");
                    spanAddPuestoCC.text($('select[id="cboFiltroCC"] option:selected').text());
                    spanAddPuestoNombre.text($('select[id="cboFiltroPuesto"] option:selected').text())
                } else {
                    Alert2Warning("Seleccione un cc y un puesto");
                    return "";
                }
            });

            btnAddPuesto.on("click", function () {
                if (cboAddPuesto.val() != "") {
                    fncAddPlantillaPuesto();

                } else {
                    Alert2Warning("Seleccione el puesto a añadir");
                    return "";
                }
            })
        }

        function initTblPuestos() {
            dtPuestos = tblPuestos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'id_puesto', title: 'ID' },
                    { data: 'puesto', title: 'PUESTO' },
                    { data: 'cantidad', title: 'SOLICITADOS' },
                    {
                        title: 'VACANTES',
                        render: function (data, type, row) {
                            return ((row.cantidad - row.altas))
                        }
                    },
                    {
                        data: 'propiedad',
                        render: (data, type, row, meta) => {
                            return `
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-1">                            
                                <div class="pull-right">
                                    <button id="btnSub${row.id_puesto ?? ""}" type="button" class="btn btn-primary substractBtn"><span style="font-size: 14px">-<span></button>
                                </div>
                            </div>
                            <div class="col-sm-2">                            
                                <input type="number" id="row${row.id_puesto ?? ""}" class="form-control" value=${row.cantidad}>
                            </div>
                            <div class="col-sm-1">                            
                                <button id="btnSum${row.id_puesto ?? ""}" type="button" class="btn btn-primary addBtn"><span style="font-size: 14px">+</span></button>
                            </div>
                            <div class="col-sm-4">
                            </div>
                            `
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPuestos.on('click', '.classBtn', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();
                    });
                    tblPuestos.on('click', '.classBtn', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                    tblPuestos.on('click', '.substractBtn', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();

                        let idPares = [];
                        let paresModificados = 0;

                        // numMovimientos[rowData.id_puesto] = (numMovimientos[rowData.id_puesto] ?? 0) + 1;
                        //Actulizar header
                        titleMovimientos.text(Number(titleMovimientos.text()) - 1);

                        //Añadir registro en un arreglo de objetos con el id de puesto y valor de puestos
                        let keyArr = rowData.id_puesto;
                        let valArr = (numMovimientos[rowData.id_puesto] != undefined ? numMovimientos[rowData.id_puesto].cantidad : rowData.cantidad) - 1;
                        let movsArr = (numMovimientos[rowData.id_puesto] != undefined ? numMovimientos[rowData.id_puesto].cambios : 0) - 1;

                        numMovimientos[keyArr] = { "id_puesto": keyArr, "cantidad": valArr, "cambios": movsArr };

                        numMovimientos.forEach(e => {
                            if (e.id_puesto != undefined) {
                                paresModificados++;
                                idPares.push(e.id_puesto);
                            }
                        });

                        if (paresModificados == 2) {
                            lstPuestos.forEach(e => {
                                if (!idPares.includes(e.id_puesto)) {
                                    $("#btnSub" + e.id_puesto).prop("disabled", true);
                                    $("#btnSum" + e.id_puesto).prop("disabled", true);
                                }
                            });
                        }
                        console.log(numMovimientos);
                        // let temp = $("#row" + rowData.id_puesto).val();
                        $("#row" + rowData.id_puesto).val(Number($("#row" + rowData.id_puesto).val()) - 1);

                        // if ($("#row" + rowData.id_puesto).val() == 0) {
                        //     $("#row" + rowData.id_puesto).val(Number($("#row" + rowData.id_puesto).val()) - 1);
                        // }


                    });
                    tblPuestos.on('click', '.addBtn', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();

                        //ACTUALIZAR HEADER
                        titleMovimientos.text(Number(titleMovimientos.text()) + 1);

                        let idPares = [];
                        let paresModificados = 0;

                        // numMovimientos[rowData.id_puesto] = (numMovimientos[rowData.id_puesto] ?? 0) + 1;

                        //Añadir registro en un arreglo de objetos con el id de puesto y valor de puestos
                        let keyArr = rowData.id_puesto;
                        let valArr = (numMovimientos[rowData.id_puesto] != undefined ? numMovimientos[rowData.id_puesto].cantidad : rowData.cantidad) + 1;
                        let movsArr = (numMovimientos[rowData.id_puesto] != undefined ? numMovimientos[rowData.id_puesto].cambios : 0) + 1;

                        numMovimientos[keyArr] = { "id_puesto": keyArr, "cantidad": valArr, "cambios": movsArr };

                        numMovimientos.forEach(e => {
                            if (e.id_puesto != undefined) {
                                paresModificados++;
                                idPares.push(e.id_puesto);
                            }
                        });

                        if (paresModificados == 2) {
                            lstPuestos.forEach(e => {
                                if (!idPares.includes(e.id_puesto)) {
                                    $("#btnSub" + e.id_puesto).prop("disabled", true);
                                    $("#btnSum" + e.id_puesto).prop("disabled", true);
                                }
                            });
                        }
                        console.log(numMovimientos);

                        // let temp = $("#row" + rowData.id_puesto).val();
                        $("#row" + rowData.id_puesto).val(Number($("#row" + rowData.id_puesto).val()) + 1);

                        // if ($("#row" + rowData.id_puesto).val() == 0) {
                        //     $("#row" + rowData.id_puesto).val(Number($("#row" + rowData.id_puesto).val()) + 1);
                        // }
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: "10%", targets: 0 },
                    { width: "40%", targets: 1 },
                    { width: "10%", targets: 2 },
                    { width: "10%", targets: 3 },
                    { width: "30%", targets: 4 },
                ],

            });
        }

        function initTblPuestoDetalle() {
            dtPuestoDetalle = tblPuestoDetalle.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                // "order": [[0, "ASC"]],
                columns: [
                    { data: 'orden', title: '#' },
                    { data: 'puesto', title: 'PUESTO' },
                    { data: 'tipoAditiva', title: 'TIPO ADITIVA' },
                    { data: 'cantidad', title: 'CANTIDAD' },
                    {
                        data: 'fecha_solicita', title: 'FECHA SOLICITUD',
                        render: function (data, type, row, meta) {
                            return moment(data).format("DD/MM/YYYY");
                        },
                    },
                    { data: 'observaciones', title: 'OBSERVACIONES' },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '3%', targets: [0] },
                    { width: '30%', targets: [1] },
                    { width: '15%', targets: [4] },
                    { width: '10%', targets: [2, 3] }
                ],
            });
        }

        function fncGetPuestoDetalle() {
            let obj = new Object();
            obj.cc = cboFiltroCC.val();
            obj.idPuesto = cboFiltroPuesto.val();
            axios.post('GetPuestoDetalle', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPuestoDetalle.clear();
                    dtPuestoDetalle.rows.add(response.data.lstPuestoDetalle);
                    dtPuestoDetalle.draw();
                    //#endregion
                } else {
                    // Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetPuestosCategorias() {
            let obj = new Object();

            if (cboFiltroPuesto.val() == "") {
                return "";
            }

            let optionPuestoNom = cboFiltroPuesto.find(`option[value="${cboFiltroPuesto.val()}"]`);
            let nomPuesto = optionPuestoNom.attr("data-prefijo");

            obj = {
                _cc: cboFiltroCC.val(),
                _strPuesto: nomPuesto,
                idPuesto: cboFiltroPuesto.val(),
            }

            axios.post("GetPuestosCategoriasRelPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // let strCategorias = "";
                    // let input = ``;
                    // for (let i = 0; i < response.data.lstCategoriasRelPuesto.length; i++) {
                    //     strCategorias += " " + response.data.lstCategoriasRelPuesto[i] + " ";
                    //     input += `<div class="col-lg-1"><input type="text" class="form-control" disabled="disabled" value="${response.data.lstCategoriasRelPuesto[i]}"></div>`;
                    // }

                    // // if (response.data.lstCategoriasRelPuesto.length > 0) {
                    // //     lblPuestosCategorias.html(`Categorías: ${strCategorias}`);
                    // //     txtCrearEditarCategoriasPuesto.val(strCategorias);
                    // // } else {
                    // //     lblPuestosCategorias.html("");
                    // //     txtCrearEditarCategoriasPuesto.val("");
                    // // }
                    // lblPuestosCategorias.html(input);
                    dtPuestos.clear();
                    dtPuestos.rows.add(response.data.lstPuestosRelPuesto);
                    dtPuestos.draw();
                    lstPuestos = response.data.lstPuestosRelPuesto;
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEditarPlantilla() {

            // if (numMovimientos[0] != undefined) {
            //     if (numMovimientos[0].id_puesto == undefined) {
            //         numMovimientos.shift();
            //     }
            // }


            let input = [];

            numMovimientos.forEach(e => {
                if (e.id_puesto != undefined) {
                    input.push(e);
                }
            });

            let optionPuestoNom = cboFiltroPuesto.find(`option[value="${cboFiltroPuesto.val()}"]`);
            let nomPuesto = optionPuestoNom.attr("data-prefijo");

            obj = {
                lstCambios: input,
                cc: cboFiltroCC.val(),
                puesto: cboFiltroPuesto.val(),
                descPuesto: nomPuesto
            }
            axios.post("/Reclutamientos/EditarPlantilla", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    fncGetPuestosCategorias();
                    Alert2Exito("Plantilla guardada con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAddPlantillaPuesto() {

            let optionPuestoNom = cboFiltroPuesto.find(`option[value="${cboFiltroPuesto.val()}"]`);
            let nomPuesto = optionPuestoNom.attr("data-prefijo");

            let optionPuestoNuevoNom = cboAddPuesto.find(`option[value="${cboAddPuesto.val()}"]`);
            let nomPuestoNuevo = optionPuestoNom.attr("data-prefijo");

            let obj = {
                _cc: cboFiltroCC.val(),
                _strPuesto: nomPuesto,
                idPuesto: cboFiltroPuesto.val(),
                _strNuevoPuesto: $('select[id="cboAddPuesto"] option:selected').text(),
                nuevoPuesto: cboAddPuesto.val(),
            }
            axios.post("AddPlantillaPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetPuestosCategorias();
                    Alert2Exito("Puesto agregado con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFocus(obj) {
            if (obj != "") {
                setTimeout(() => $(`#${obj}`).focus(), 50);
            }
        }
    }

    $(document).ready(() => {
        CH.CatCategorias = new CatCategorias();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();