(() => {
    $.namespace('CatDepartamentos.Seguridad');

    // CONTROLES FILTROS
    const cboCC = $('#cboCC');
    const cboClaveDepto = $('#cboClaveDepto');
    const cboAreaOperativa = $('#cboAreaOperativa');
    const btnLimpiar = $('#btnLimpiar');
    const btnNuevo = $('#btnNuevo');
    const btnBuscar = $('#btnBuscar');
    const idEmpresa = $('#idEmpresa');

    // CONTROLES PARA CREAR/EDITAR UN REGISTRO

    const txtcboNuevoCC = $('#txtcboNuevoCC');
    const txtNuevoClaveDepto = $('#txtNuevoClaveDepto');
    const cboNuevoCC = $('#cboNuevoCC');
    const cboNuevoClaveDepto = $('#cboNuevoClaveDepto');
    const cboNuevoAreaOperativa = $('#cboNuevoAreaOperativa');
    const txtNuevoDescripcion = $('#txtNuevoDescripcion');
    const btnNuevoLimpiar = $('#btnNuevoLimpiar');
    const btnCrearEditarHorasHombre = $('#btnCrearEditarHorasHombre');
    const mdlCrearEditarDepartamento = $('#mdlCrearEditarDepartamento');
    const lblCrearEditarDepartamento = $('#lblCrearEditarDepartamento');
    let prefijo = '';
    // TABLA
    const tblS_CatDepartamentos = $('#tblS_CatDepartamentos');
    let dtCatDepartamentos;
    let objSelect = new Object();
    let obtenerCC;

    // CONTROLES MODAL CREAR/EDITAR DEPARTAMENTO

    Seguridad = function () {
        (function init() {
            Listeners();
            initDataTblS_CatDepartamentos();
            // fillComboClaveDepto();
            fillCboCCEnKontrol();
            fillCboCCEnKontrolDepto();
            $('#showEditar').css("display", "none");
            fillcomboNuevoCC();

        })();
    }




    function Listeners() {
        fncDeshabilitarFiltros();
        fncDeshabilitarCtrlsCrearEditarRegistro();
        fncFillCboCC();
        fncCbosSelect2();
        cboNuevoCC.attr('disabled', 'disabled');

        // SE LLENAN LOS SELECT DEL FILTRO
        // fncFillCboClaveDepto();
        fncFillCboAreaOperativa();
        // END: SE LLENAN LOS SELECT DEL FILTRO

        txtNuevoDescripcion.attr("autocomplete", "off");


        cboNuevoCC.on("select2:select", function (e) {
            let selCC = $(this).find(`option[value="${$(this).val()}"]`);
            let cc = selCC.attr("Value");
            obtenerCC = cc;
            let idEmpresa = selCC.attr("empresa");
            fillComboClaveDepto(cc, idEmpresa);
            cboNuevoClaveDepto.attr("disabled", false);


        });

        cboNuevoClaveDepto.on("select2:select", function (e) {
            console.log('hola')
            let data = $('option:selected', this).attr('data-id');
            let optionSelected = $(this).find(`option[data-id="${data}"]`);
            fncGetDataSelect2(optionSelected);
            if (objSelect.idEmpresa != undefined) {
                console.log(objSelect)
            }

        });
        cboNuevoClaveDepto.change(function (e) {
            console.log('hola')
            let data = $('option:selected', this).attr('data-id');
            let optionSelected = $(this).find(`option[data-id="${data}"]`);
            fncGetDataSelect2(optionSelected);
            if (objSelect.idEmpresa != undefined) {
                console.log(objSelect)
            }
        });


        cboCC.on("select2:select", function (e) {
            let selCC = $(this).find(`option[value="${$(this).val()}"]`);
            let cc = selCC.attr("empresa");

        });

        cboCC.change(function (e) {
            let selCC = $(this).find(`option[value="${$(this).val()}"]`);
            let cc = selCC.attr("Value");
            let idEmpresa = selCC.attr("empresa");
            console.log(cc)
            console.log(idEmpresa)
        });

        cboNuevoCC.change(function (e) {
        });

        btnLimpiar.click(function (e) {
            cboCC[0].selectedIndex = 0;
            cboCC.trigger("change");
            // cboClaveDepto.empty();
            // cboClaveDepto.attr("disabled", true);
            // cboAreaOperativa.empty();
            // cboAreaOperativa.attr("disabled", true);
            cboClaveDepto[0].selectedIndex = 0;
            cboClaveDepto.trigger("change");
            cboAreaOperativa.selectedIndex = 0;
            cboAreaOperativa.trigger("change");
            cboAreaOperativa[0].selectedIndex = 0;
            cboAreaOperativa.trigger("change");
        });

        btnNuevoLimpiar.click(function (e) {
            fncLimpiarModalCrearEditar();
        });

        btnNuevo.click(function (e) {
            fncLimpiarModalCrearEditar();
        });

        btnCrearEditarHorasHombre.click(function (e) {
            let id;
            btnCrearEditarHorasHombre.attr("data-id") > 0 ? id = btnCrearEditarHorasHombre.attr("data-id") : id = 0;
            fncCrearEditarDepartamento(id);
            fncGetCatDepartamentos();
        });

        btnBuscar.click(function (e) {
            fncGetCatDepartamentos();
        });
    }

    function fncLimpiarModalCrearEditar() {
        cboNuevoClaveDepto.empty();
        txtNuevoDescripcion.val('');
        cboNuevoCC[0].selectedIndex = 0;
        cboNuevoCC.trigger("change");
        cboNuevoClaveDepto[0].selectedIndex = 0;
        cboNuevoClaveDepto.trigger("change");
        cboNuevoAreaOperativa[0].selectedIndex = 0;
        cboNuevoAreaOperativa.trigger("change");
        txtNuevoDescripcion.attr("value", " ");
        btnCrearEditarHorasHombre.attr("data-id", 0);
        lblCrearEditarDepartamento.val("Test");
        $('#ocultarEditar').css("display", "block");
        $('#showEditar').css("display", "none");
        cboNuevoClaveDepto.attr("disabled", true);
        cboNuevoCC.attr("disabled", false);

    }

    function fncFillCboAreaOperativa(esActualizar) {
        cboAreaOperativa.empty();
        cboAreaOperativa.fillCombo('/CatDepartamentos/getAreaOperativa', null, null);
        cboAreaOperativa[0].selectedIndex = 0;
        cboNuevoAreaOperativa.empty();
        cboNuevoAreaOperativa.fillCombo('/CatDepartamentos/getAreaOperativa', null, null);
        cboNuevoAreaOperativa[0].selectedIndex = 0;
    }

    function fncDeshabilitarFiltros() {
        // cboClaveDepto.attr("disabled", true);
        // cboAreaOperativa.attr("disabled", true);
    }

    function fncDeshabilitarCtrlsCrearEditarRegistro() {
        // cboNuevoClaveDepto.attr("disabled", true);
        // cboNuevoAreaOperativa.attr("disabled", true);
    }

    function fncCbosSelect2() {
        cboCC.select2();
        cboClaveDepto.select2();
        cboAreaOperativa.select2();
        cboNuevoCC.select2();
        cboNuevoClaveDepto.select2();
        cboNuevoAreaOperativa.select2();
    }

    function fillCboCCEnKontrol() {
        axios.get('/Administrativo/CatDepartamentos/ObtenerComboCCAmbasEmpresas')
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    cboCC.append('<option value="">--Seleccione--</option>');
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : x.label == 'COLOMBIA' ? 3 : x.label == 'PERU' ? 6 : 0}">${y.Text}</option>`;
                        });

                        cboCC.append(groupOption);


                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }


    function fillcomboNuevoCC() {
        axios.get('/Administrativo/CatDepartamentos/ObtenerComboCCAmbasEmpresas')
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    console.log(items)
                    cboNuevoCC.append('<option value="">--Seleccione--</option>');
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : x.label == 'COLOMBIA' ? 3 : x.label == 'PERU' ? 6 : 0}">${y.Text}</option>`;
                        });
                        cboNuevoCC.append(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }


    function fillComboClaveDepto(cc, idEmpresa) {
        var con = 0;
        axios.get(`/Administrativo/CatDepartamentos/ObtenerCCporDepartamento?cc=${cc}&idEmpresa=${idEmpresa}`)
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    console.log(items)
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : x.label == 'COLOMBIA' ? 3 : x.label == 'PERU' ? 6 : 0}" data-id="${con++}">${y.Text}</option>`;
                        });
                        cboNuevoClaveDepto.empty();
                        cboNuevoClaveDepto.append('<option value="">--Seleccione--</option>');
                        cboNuevoClaveDepto.append(groupOption);
                        cboNuevoCC[0].selectedIndex = 1;


                    });
                } else {
                    AlertaGeneral(`Alerta`, 'no se encuenta clave departamento para este centro de costo');
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function GetFillTxt(claveDepto, idEmpresa) {
        axios.get(`/Administrativo/CatDepartamentos/ObtenerCCporDepartamentoEditar?claveDepto=${claveDepto}&idEmpresa=${idEmpresa}`)
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    items.forEach(x => {
                        let groupOption = "";
                        x.options.forEach(y => {
                            groupOption += `${y.Text}`;
                        });
                        txtcboNuevoCC.val(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function GetFillTxtCC(cc, idEmpresa) {
        axios.post(`/Administrativo/CatHorasHombre/ObtenerAreasPorCC`, {
            ccsCplan: cc,
            idEmpresa: idEmpresa
        })
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    items.forEach(x => {
                        let groupOption = "";
                        x.options.forEach(y => {
                            groupOption += `${y.Value} - ${y.Text}`;
                        });
                        txtNuevoClaveDepto.val(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function fncFillCboCC() {
        var a = $('#cboNuevoClaveDepto').find(`option[value="${prefijo}"]`);
        let d = a.val("value");
        cboNuevoCC[0].selectedIndex = 0;

    }


    function fillCboCCEnKontrolDepto() {
        let con = 0;
        axios.get('/Administrativo/CatDepartamentos/getClaveDepto')
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    cboClaveDepto.append('<option value="">--Seleccione--</option>');
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" Prefijo="${y.Prefijo}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : x.label == 'COLOMBIA' ? 3 : x.label == 'PERU' ? 6 : 0}" data-id="${con++}">${y.Text}</option>`;
                        });

                        cboClaveDepto.append(groupOption);


                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }


    function fncFillCboClaveDepto(cc, esActualizar) {
        cboClaveDepto.empty();
        cboClaveDepto.fillCombo('/CatDepartamentos/getClaveDepto', null, null);
        cboClaveDepto[0].selectedIndex = 0;
        cboNuevoClaveDepto.empty();
        cboNuevoClaveDepto.fillCombo('/CatDepartamentos/getClaveDepto', null, null);
        cboNuevoClaveDepto[0].selectedIndex = 0;
    }

    function fncCrearEditarDepartamento(id) {
        let esEditar = false;
        btnCrearEditarHorasHombre.attr("data-id") > 0 ? esEditar = true : esEditar = false
        let objData = fncGetFormulario(esEditar);
        console.log(objData)
        if (cboNuevoCC.val() != '' && cboNuevoClaveDepto.val() > 0 && cboNuevoAreaOperativa.val() > 0 || esEditar) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrativo/CatDepartamentos/CrearEditarDepartamento",
                data: objData,
                success: function (response) {
                    if (!response.success) {
                        Alert2Error(response.message);
                    }
                    else {
                        id > 0 ?
                            Alert2Exito("Se actualizó correctamente.") :
                            Alert2Exito("Se registró correctamente.");
                        mdlCrearEditarDepartamento.modal("hide");
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    }

    function fncGetFormulario(esEditar) {
        let id, clave_depto, descripcion, idAreaOperativa;

        btnCrearEditarHorasHombre.attr("data-id") > 0 ? id = btnCrearEditarHorasHombre.attr("data-id") : id = 0;

        if (!esEditar) {
            cboNuevoClaveDepto.val() > 0 && cboNuevoClaveDepto != null ?
                clave_depto = cboNuevoClaveDepto.val() :
                Alert2Warning("Es necesario seleccionar un departamento.");
        }

        // let selCC = cboNuevoCC.find(`option[value="${cboNuevoCC.val()}"]`);
        // let cc = selCC.attr("data-prefijo");
        let cc
        if (esEditar) {
            cc = txtcboNuevoCC.val();
        } else {
            cc = cboNuevoCC.val();
        }

        cc != '' && cc != null ?
            cc = cc :
            Alert2Warning("Es necesario seleccionar un CC.");

        cboNuevoAreaOperativa.val() > 0 && cboNuevoAreaOperativa.val() != null ?
            idAreaOperativa = cboNuevoAreaOperativa.val() :
            Alert2Warning("Es necesario seleccionar una Área operativa.");

        txtNuevoDescripcion.val() != "" ? descripcion = txtNuevoDescripcion.val() : descripcion = "";
        console.log(objSelect)
        let objData = {};
        objData = {
            id: id,
            clave_depto: objSelect.cc,
            descripcion: descripcion,
            cc: obtenerCC,
            idAreaOperativa: idAreaOperativa,
            idEmpresa: objSelect.idEmpresa
        };
        return objData;
    }

    function fncGetCatDepartamentos() {
        let objFiltro = fncGetFiltros();
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/CatDepartamentos/GetCatDepartamentos",
            data: objFiltro,
            success: function (response) {
                if (response.success) {
                    dtCatDepartamentos.clear();
                    dtCatDepartamentos.rows.add(response.lstCatDepartamentos);
                    dtCatDepartamentos.draw();
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }






    function fncGetFiltros() {
        let clave_depto, idAreaOperativa;

        let selCC = cboCC.find(`option[value="${cboCC.val()}"]`);
        let cc = selCC.attr("data-prefijo");
        cboCC.val() > 0 ? cc = cboCC.val() : cc = "";
        cboClaveDepto.val() > 0 ? clave_depto = cboClaveDepto.val() : clave_depto = 0;
        cboAreaOperativa.val() > 0 ? idAreaOperativa = cboAreaOperativa.val() : idAreaOperativa = 0;

        let objData = {};
        objData = {
            cc: cc,
            clave_depto: clave_depto,
            idAreaOperativa: idAreaOperativa
        };

        return objData;
    }
    var descDep = "";
    if (idEmpresa.val() == 6) {
        descDep = "descripcion";
    } else {
        descDep = "departamento";
    }
    function initDataTblS_CatDepartamentos() {
        dtCatDepartamentos = tblS_CatDepartamentos.DataTable({
            language: dtDicEsp,
            ordering: true,
            paging: true,
            searching: true,
            bFilter: true,
            info: false,
            columns: [
                { data: "cc", title: "CC" },
                // { data: 'departamento', title: 'Departamento' },
                { data: descDep, title: 'Departamento' },
                { data: 'NombreAreaOperativa', title: 'Área operativa' },
                {
                    title: "Estatus",
                    render: function (data, type, row) {
                        let activo;
                        row.esActivo ? activo = "Activo" : activo = "Desactivado";
                        return activo;
                    }
                },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                        row.esActivo ?
                            btnEliminar = `<button class='btn-eliminar btn btn-success elimininarDepartamento' data-esActivo="1" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>` :
                            btnEliminar = `<button class='btn-eliminar btn btn-danger elimininarDepartamento' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-off"></i></button>`;

                        return `<button class='btn-editar btn btn-warning editarDepartamento' data-id="${row.id}">` +
                            `<i class='fas fa-pencil-alt'></i>` +
                            `</button>&nbsp;` + btnEliminar;
                    }
                },
                { data: 'id', title: 'id', visible: false },
                { data: 'clave_depto', title: 'clave_depto', visible: false },
                { data: 'idAreaOperativa', title: 'idAreaOperativa', visible: false },
                { data: 'descripcion', title: 'descripcion', visible: false }
            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "13%", "targets": [0, 2, 3] }
            ],
            initComplete: function (settings, json) {
                tblS_CatDepartamentos.on("click", ".elimininarDepartamento", function () {
                    let esActivo = $(this).attr("data-esActivo");
                    let strMensaje = "";
                    esActivo == 1 ?
                        strMensaje = "¿Desea desactivar el registro seleccionado?" :
                        strMensaje = "¿Desea activar el registro seleccionado?";

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
                            fncActivarDesactivarDepartamento($(this).attr("data-id"), esActivo);
                        }
                    });
                });

                tblS_CatDepartamentos.on("click", ".editarDepartamento", function () {
                    const rowData = dtCatDepartamentos.row($(this).closest("tr")).data();
                    // let optionID = cboNuevoCC.find(`option[data-prefijo="${rowData.cc}"]`);
                    // let valCC = optionID.val();



                    $('#ocultarEditar').css("display", "none");
                    $('#showEditar').css("display", "block");

                    cboNuevoCC.attr("disabled", false);
                    cboNuevoCC.trigger("change");

                    txtNuevoClaveDepto.attr("disabled", true);
                    txtcboNuevoCC.attr("disabled", true);
                    txtNuevoClaveDepto.val(rowData.departamento[0])
                    GetFillTxt(rowData.cc, rowData.idEmpresa);
                    // GetFillTxtCC(rowData.cc,rowData.idEmpresa);
                    cboNuevoClaveDepto.attr("disabled", false);
                    // cboNuevoClaveDepto[0].selectedIndex = 1;
                    cboNuevoClaveDepto.trigger("change");

                    cboNuevoAreaOperativa.val(rowData.idAreaOperativa);
                    // cboNuevoAreaOperativa[0].selectedIndex = 1;
                    cboNuevoAreaOperativa.trigger("change");

                    txtNuevoDescripcion.val(rowData.descripcion);
                    btnCrearEditarHorasHombre.attr("data-id", rowData.id);

                    mdlCrearEditarDepartamento.modal("show");

                });
            }
        });
    }

    function fncActivarDesactivarDepartamento(id, esActivo) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrativo/CatDepartamentos/ActivarDesactivarDepartamento",
            data: {
                id: id
            },
            success: function (response) {
                if (!response.success) {
                    Alert2Error(response.message);
                }
                else {
                    let strMensaje = "";
                    esActivo == 1 ?
                        strMensaje = "Se ha desactivado con éxito el registro." :
                        strMensaje = "Se ha activado con éxito el registro.";
                    Alert2Exito(strMensaje);
                    btnBuscar.click();
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            }
            // ,
            // complete: function () {
            //     btnBuscar.click();
            //     $.unblockUI();
            // }
        });
    }

    function fncGetDataSelect2(optionSelected) {
        console.log(optionSelected)
        console.log(optionSelected.val())
        let cc = optionSelected.val();
        let empresa = optionSelected.attr("empresa");
        objSelect = new Object();
        objSelect = {
            cc: cc,
            idEmpresa: empresa
        };
        console.log(objSelect)
    }







    $(document).ready(() => {
        CatDepartamentos.Seguridad = new Seguridad();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();