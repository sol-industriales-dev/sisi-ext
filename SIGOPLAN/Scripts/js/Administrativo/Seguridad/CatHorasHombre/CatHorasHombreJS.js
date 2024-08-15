(() => {
    $.namespace('Cat_HorasHombre.Seguridad');
    const cboCC = $('#cboCC');
    const cboClaveDepto = $('#cboClaveDepto');
    const cboGrupo = $('#cboGrupo');
    const tblS_CatHorasHombre = $('#tblS_CatHorasHombre');
    const divDias = $('#divDias');
    const cboCCNuevo = $("#cboCCNuevo");
    const cboClaveDeptoNuevo = $('#cboClaveDeptoNuevo');
    const cboGrupoNuevo = $('#cboGrupoNuevo');
    const divDiasSeccion = $('#divDiasSeccion');
    const btnCrearEditarHorasHombre = $('#btnCrearEditarHorasHombre');
    const btnBuscar = $('#btnBuscar');
    const mdlNuevoHorasHombre = $('#mdlNuevoHorasHombre');
    const btnNuevo = $('#btnNuevo');
    const txtFechaInicio = $('#txtFechaInicio');
    const txtHoras = $('#txtHoras');
    const btnLimpiar = $('#btnLimpiar');
    let BanderaCrearEditarHorasHombre;
    const txtcboNuevoCC = $('#txtcboNuevoCC');
    const txtNuevoClaveDepto = $('#txtNuevoClaveDepto');

    let arrHorasDia = [];
    let objData = new Object();
    let ccNuevo = '';

    Seguridad = function () {
        (function init() {
            Listeners();
            initDataTblS_CatHorasHombre();
            fillCboCCEnKontrol();
        })();
    }

    function Listeners() {
        convertToSelect2();
        txtFechaInicio.datepicker({
            changeMonth: true,
            changeYear: true
        }).datepicker('setDate', new Date());
        txtFechaInicio.attr("readonly", true);

        txtHoras.val("0");
        txtHoras.attr("type", "number")
        txtHoras.attr("autocomplete", "off");

        // cboCC.fillCombo('CatHorasHombre/getCC', null, false); v1
        // cboCC.fillCombo('../IndicadoresSeguridad/LlenarComboCC', null, false);
        // $('select[id=cboCC] > option:first-child').text("--Todos--");
        // cboCCNuevo.fillCombo('CatHorasHombre/getCC', null, false);
        // cboCCNuevo.fillCombo('../IndicadoresSeguridad/LlenarComboCC', null, false);

        habilitarDeshabilitarCtrl(cboClaveDepto, true);
        habilitarDeshabilitarCtrl(cboGrupo, true);
        cboCC.change(function (e) {
            if ($(this).val() > 0) {
                // let selCC = $(this).find(`option[value="${$(this).val()}"]`);
                // let cc = selCC.attr("data-prefijo");
                let cc = $(this).val();


                let data = $('option:selected', this).attr('data-id');
                let optionSelected = $(this).find(`option[data-id="${data}"]`);
                fncGetDataSelect2(optionSelected);



                habilitarDeshabilitarCtrl(cboClaveDepto, false);
                habilitarDeshabilitarCtrl(cboGrupo, false);
                cboGrupo.fillCombo('CatHorasHombre/getRoles', null, false);
                $('select[id=cboGrupo] > option:first-child').text("--Todos--");
                getAreaOperativa(cboClaveDepto, cc, objData.idEmpresa,true);
                // cboClaveDepto.fillCombo('/Administrativo/Capacitacion/ObtenerAreasPorCC', {ccsCplan: cc});
            } else {
                habilitarDeshabilitarCtrl(cboClaveDepto, true);
                habilitarDeshabilitarCtrl(cboGrupo, true);
            }
        });

        habilitarDeshabilitarCtrl(cboClaveDeptoNuevo, true);
        habilitarDeshabilitarCtrl(cboGrupoNuevo, true);
        cboCCNuevo.change(function (e) {
            
            divDiasSeccion.html("");
            cboClaveDeptoNuevo.attr("data-id", 0);
            cboGrupoNuevo.attr("data-id", 0);
            btnCrearEditarHorasHombre.attr("data-id", 0);

            if ($(this).val() != "0" && $(this).val() != "") {
                // let selCC = $(this).find(`option[value="${$(this).val()}"]`);
                // let cc = selCC.attr("data-prefijo");
                let cc = $(this).val();
                ccNuevo = cc;
                let data = $('option:selected', this).attr('data-id');
                let optionSelected = $(this).find(`option[data-id="${data}"]`);
                fncGetDataSelect2(optionSelected);


                habilitarDeshabilitarCtrl(cboClaveDeptoNuevo, false);
                habilitarDeshabilitarCtrl(cboGrupoNuevo, false);
                cboGrupoNuevo.fillCombo('CatHorasHombre/getRoles', null, false);
                getAreaOperativa(cboClaveDeptoNuevo, cc, objData.idEmpresa);

                // cboClaveDeptoNuevoNuevo.fillCombo('/Administrativo/Capacitacion/ObtenerAreasPorCC', {ccsCplan: cc});
            } else {
                habilitarDeshabilitarCtrl(cboClaveDeptoNuevo, true);
                habilitarDeshabilitarCtrl(cboGrupoNuevo, true);
            }
        });

        cboClaveDeptoNuevo.change(function (e) {
            let optionID = $(this).val();
            //let claveDepto = cboClaveDeptoNuevo.find(`option[value="${optionID}"]`);
            $(this).attr("data-id", optionID);
        });

        cboGrupoNuevo.change(function (e) {
            //divDiasSeccion.css("display", "inline");
            let optionID = $(this).val();
            $(this).attr("data-id", optionID);
            // let cantDiasLaborales = cboGrupoNuevo.find(`option[value="${optionID}"]`);
            // let data = cantDiasLaborales.attr("data-prefijo");
            // crearPanelHorasDia(data);
        });

        btnCrearEditarHorasHombre.click(function (e) {
            ValidarDatos();
            getHorasDia();
        });

        btnBuscar.click(function (e) {
            getHorasHombre();
        });

        btnNuevo.click(function (e) {
            limpiarModalCtrls();
        });

        txtHoras.click(function (e) {
            $(this).select();
        });

        btnLimpiar.click(function (e) {
            limpiarFiltrosBusqueda();
        });
    }

    function limpiarFiltrosBusqueda() {
        cboCC.val("");
        cboCC.trigger("change");
    }

    function limpiarModalCtrls() {
        cboCCNuevo.val("");
        cboCCNuevo.trigger("change");
        divDiasSeccion.html("");
        btnCrearEditarHorasHombre.attr("data-id", 0);
        cboCCNuevo.removeAttr("disabled");
        txtFechaInicio.datepicker({
            changeMonth: true,
            changeYear: true
        }).datepicker('setDate', new Date());
        txtHoras.val("");

        $('#ocultarEditar').css("display", "block")
        $('#showEditar').css("display", "none")
    }

    function getHorasDia() {
        let i = 0;
        let Bandera = true;
        let strMensaje = "";

        arrHorasDia = [];
        $("input[type=number]").each(function (e) {
            let hrsDia = $(this).val();
            arrHorasDia.push(`${hrsDia}`);
            i++;
        });

        for (let i = 0; i < arrHorasDia.length; i++) {
            let cantHoras = arrHorasDia[i];
            if (cantHoras < 0 || cantHoras == "" || cantHoras == null || cantHoras == "-0") {
                strMensaje += "Es necesario ingresar todos los campos. <br>";
                Bandera = false;
            }
            if (cantHoras == "-0" || cantHoras < 0) {
                strMensaje = "Solamente se aceptan datos positivos.";
            }
        }

        if (Bandera) {
            let id = btnCrearEditarHorasHombre.attr("data-id");
            crearActualizarHorasHombre(id, objData.cc, objData.idEmpresa);
        } else {
            Alert2Warning(strMensaje);
        }

    }

    function habilitarDeshabilitarCtrl(input, disabled) {
        input.attr("disabled", disabled);
        input.empty();
    }

    function convertToSelect2() {
        cboCC.select2();
        cboClaveDepto.select2();
        cboGrupo.select2();
        cboCCNuevo.select2();
        cboClaveDeptoNuevo.select2();
        cboGrupoNuevo.select2();
    }

    function getAreaOperativa(cboAreaOperativaID, cc, idEmpresa) {
        $.post('/Administrativo/CatHorasHombre/ObtenerAreasPorCC', {
            ccsCplan: cc,
            idEmpresa: idEmpresa
        }).then(response => {
            if (response.success) {
                //cboAreaOperativaID.append(`<option value=>--Seleccione--</option>`);
                let contador = 0;
                response.items.forEach(x => {
                    let groupOption;
                    x.options.forEach(y => {
                        if (contador == 0) {
                            cboAreaOperativaID.attr("data-id", y.Value);
                        }
                        contador++;
                        groupOption += `<option value="${y.Value}">${y.Text}</option>`;
                    });
                    cboAreaOperativaID.append(groupOption);
                });
            } else {
                Alert2Error(`No se pudo completar la operación: ${response.message}`);
            }
        }, error => {
            Alert2Error(`Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
        });
    }

    function initDataTblS_CatHorasHombre() {
        dtHorasHombre = tblS_CatHorasHombre.DataTable({
            language: dtDicEsp,
            ordering: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            columns: [
                { data: "cc", title: "CC" },
                { data: "idEmpresa", title: "CC" },
                { data: "departamento", title: "Área operativa" },
                { data: "descripcion", title: "Grupo" },
                { 
                    data: 'horasDia', title: 'Horas',
                    render: function (data, type, row){
                        return parseFloat(data);
                    }
                },
                {
                    data: "fechaInicio", title: "Fecha inicio",
                    render: function (data, type, row) {
                        return moment(data).format('DD/MM/YYYY');
                    }

                },
                {
                    data: "esActivo", title: "Estatus",
                    render: function (data, type, row) {
                        let esActivo;
                        data ? esActivo = "Activo" : esActivo = "Desactivado";
                        return esActivo;
                    }
                },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                        row.esActivo ?
                            btnEliminar = `<button class='btn-eliminar btn btn-success eliminarHorasHombre' data-esActivo="1" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>` :
                            btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarHorasHombre' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-off"></i></button>`;

                        return `<button class='btn-editar btn btn-warning editarHorasHombre' data-id="${row.id}">` +
                            `<i class='fas fa-pencil-alt'></i>` +
                            `</button>&nbsp;` + btnEliminar;
                    }
                },
                { data: "clave_depto", visible: false },
                { data: 'idGrupo', visible: false },
                { data: 'id', visible: false },
            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "13%", "targets": [1, 2, 4, 5] },
                { "width": "5%", "targets": 3 },
                {
                    targets: [4],
                    render: function (data, type, row) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblS_CatHorasHombre.on("click", ".editarHorasHombre", function () {
                    const rowData = dtHorasHombre.row($(this).closest("tr")).data();

                    let optionID = cboCC.find(`option[data-prefijo="${rowData.idCC}"]`);
                    let valCC = optionID.val();
                    cboCCNuevo.val(valCC);
                    cboCCNuevo.trigger("change");
                    cboCCNuevo.attr("disabled", "disabled");
                    $('#ocultarEditar').css("display", "none");
                    $('#showEditar').css("display", "block");

                    let idEmpresa = rowData.idEmpresa == "ARRENDADORA" ? 2 : 1;
                    GetFillTxt(rowData.cc, idEmpresa);
                    GetFillTxtCC(rowData.cc, idEmpresa,rowData.clave_depto);
                    // cboClaveDeptoNuevo.val(rowData.departamento);
                    $('select[id=cboClaveDeptoNuevo] > option:first-child').text(rowData.departamento);
                    cboClaveDeptoNuevo.trigger("change");
                    cboClaveDeptoNuevo.attr("data-id", rowData.clave_depto);
                    cboClaveDeptoNuevo.attr("disabled", "disabled");

                    habilitarDeshabilitarCtrl(cboGrupoNuevo, false)
                    cboGrupoNuevo.fillCombo('CatHorasHombre/getRoles', null, false);
                    cboGrupoNuevo.val(rowData.idGrupo);
                    cboGrupoNuevo.trigger("change");

                    txtHoras.val(rowData.horasDia);
                    btnCrearEditarHorasHombre.attr("data-id", rowData.id);
                    mdlNuevoHorasHombre.modal("show");

                    //     let optionID = cboGrupoNuevo.val();
                    //     let cantDiasLaborales = cboGrupoNuevo.find(`option[value="${optionID}"]`);
                    //     let data = cantDiasLaborales.attr("data-prefijo");

                    //     let jsonToArrayHorasDias = [];
                    //     jsonToArrayHorasDias = JSON.parse(rowData.horasDia);

                    //     let contador = 1;
                    //     let arrHorasDias = (Object.values(jsonToArrayHorasDias)[0]);
                    //     for (let i = 0; i < data; i++) {
                    //         let txt = $(`#txt${contador}`);
                    //         txt.val(arrHorasDias[i]);
                    //         contador++;
                    //     }
                });

                tblS_CatHorasHombre.on("click", ".eliminarHorasHombre", function () {
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
                            EliminarHoraHombre($(this).attr("data-id"), esActivo);
                        }
                    });
                });
            }
        });
    }

    function EliminarHoraHombre(id, esActivo) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrativo/CatHorasHombre/EliminarHorasHombre",
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
                    getHorasHombre();
                    Alert2Exito(strMensaje);
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

    function getHorasHombre() {
        let data = getFiltrosBusqueda();
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/CatHorasHombre/GetHorasHombre",
            data: data,
            success: function (response) {
                if (response.success) {
                    dtHorasHombre.clear();
                    dtHorasHombre.rows.add(response.lstHorasHombre);
                    dtHorasHombre.draw();
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


    function fillCboCCEnKontrol() {
        let con = 0;
        axios.get('/Administrativo/CatHorasHombre/ObtenerComboCCAmbasEmpresas')
            .then(response => {
                let { success, items, message } = response.data;

                if (success) {
                        cboCC.append('<option value="">--Seleccione--</option>');
                        cboCCNuevo.append('<option value="">--Seleccione--</option>');
                        items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}" data-id="${con++}" data-prefijo="${y.Value}">${y.Text}</option>`;
                        });

                        cboCC.append(groupOption);
                        cboCCNuevo.append(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function getFiltrosBusqueda() {
        let idCC = 0,
            clave_depto = 0;
        if (cboClaveDepto.val() > 0 || cboClaveDepto.val() != "")
            clave_depto = cboClaveDepto.val();

        return {
            cc: objData.cc,
            clave_depto: clave_depto
        };
    }

    // function crearPanelHorasDia(rol) {
    //     let panelDia = "";
    //     let contador = 0;

    //     $("#divDiasSeccion").html("");
    //     for (let i = 0; i < rol; i++) {
    //         contador++;
    //         if (contador == 1){
    //             //console.log("ENTRE AL CONTADOR: " + contador);
    //             $("#divDiasSeccion").append("<div class='row'>");
    //         }

    //         panelDia =
    //         `<div class='col-lg-2'>` +
    //             `<div class='panel panel-primary'>` +
    //                 `<div class='panel-heading'>` +
    //                     `<button type='button' class='btn cardHrsDia'>${contador}</button>` +
    //                 `</div>` +
    //                 `<div class='panel-body'>` +
    //                     `<input type='number' onClick="this.select();" id='txt${contador}' value='0' style='width: 100%; text-align:center;'>` +
    //                 `</div>` +
    //             `</div>` +
    //         `</div>`;

    //         $("#divDiasSeccion").append(panelDia);
    //         //console.log(panelDia);
    //     }
    //     //contador = 0;
    //     $("#divDiasSeccion").append("</div>");
    //     // "</div>" //END: ROW
    // }

    function crearActualizarHorasHombre(id, cc, idEmpresa) {
        let esEditar = false;
        btnCrearEditarHorasHombre.attr("data-id") > 0 ? esEditar = true : esEditar = false
        ValidarDatos(esEditar);

        if (BanderaCrearEditarHorasHombre) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrativo/CatHorasHombre/CrearEditarHorasHombre",
                data: {
                    id: id,
                    cc: cc,
                    idEmpresa: idEmpresa,
                    clave_depto: cboClaveDeptoNuevo.val(),
                    idGrupo: cboGrupoNuevo.val(),
                    fechaInicio: txtFechaInicio.val(),
                    horas: txtHoras.val()
                },
                success: function (response) {
                    if (!response.success) {
                        Alert2Error(response.message);
                    }
                    else {
                        id > 0 ?
                            Alert2Exito("Se actualizó correctamente.") :
                            Alert2Exito("Se registró correctamente.");
                        getHorasHombre();
                        mdlNuevoHorasHombre.modal("hide");
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
                        txtNuevoClaveDepto.val(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function GetFillTxtCC(cc, idEmpresa,claveDepto) {
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
                            if (claveDepto==y.Value) {
                            groupOption += `${y.Value} - ${y.Text}`;
                        }
                        });
                        txtcboNuevoCC.val(groupOption);

                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }




    function ValidarDatos(esEditar) {
        let strMensaje = "";

        let selCC = cboCCNuevo.find(`option[value="${cboCCNuevo.val()}"]`);
        let idCC = selCC.attr("data-prefijo");
        if (esEditar == false) {
            if (idCC == "" || idCC == null || idCC == 0) {
                strMensaje = "Es necesario seleccionar un CC. <br>";
            }
            if (cboClaveDeptoNuevo.attr("data-id") < 1 || cboClaveDeptoNuevo.val() == null || cboClaveDeptoNuevo.val() == "") {
                strMensaje += "Es necesario seleccionar un Área operativa. <br>";
            }
            if (cboGrupoNuevo.attr("data-id") < 1 || cboGrupoNuevo.val() == null || cboGrupoNuevo.val() == "") {
                strMensaje += "Es necesario seleccionar un Grupo. <br>";
            }

        }

        if (txtHoras.val() == "") {
            strMensaje += "Es necesario ingresar una hora. <br>";
        }
        if (txtHoras.val() < 0) {
            strMensaje += "Solamente se aceptan datos positivos.";
            Alert2Error(strMensaje);
        }

        if (esEditar == false) {
            if (cboCCNuevo.val() != "" && cboClaveDeptoNuevo.val() != null && cboClaveDeptoNuevo.attr("data-id") > 0 && cboGrupoNuevo.attr("data-id") > 0) {
                BanderaCrearEditarHorasHombre = true;
            } else {
                BanderaCrearEditarHorasHombre = false
                Alert2Error(strMensaje);
            }
        } else {
            BanderaCrearEditarHorasHombre = true;
        }
    }
    function fncGetDataSelect2(optionSelected) {

        let cc = optionSelected.val();
        let empresa = optionSelected.attr("empresa");
        objData = new Object();
        objData = {
            cc: cc,
            idEmpresa: empresa
        };
    }

    $(document).ready(() => {
        Cat_HorasHombre.Seguridad = new Seguridad();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();