var AgrupacionCCController = function () {

    //#region  Variables
    const btnNuevo = $('#btnNuevo');
    const btnBuscar = $('#btnBuscar');
    const btnLimpiar = $('#btnLimpiar');
    const btnCrearEditar = $('#btnCrearEditar');
    const btnEditarEditar = $('#btnEditarEditar');

    const mdlCrearAgrupacion = $('#mdlCrearAgrupacion');
    const mdlEditarAgrupacion = $('#mdlEditarAgrupacion');

    const cboAgrupacion = $('#cboAgrupacion');
    const cboCCMultiple = $('#cboCCMultiple');
    const cboEditarCC = $('#cboEditarCC');
    const cboComboAgrupaciones = $('#cboComboAgrupaciones');

    const txtNombreAgrupacion = $('#txtNombreAgrupacion');
    const txtNombreAgrupacionEditar = $('#txtNombreAgrupacionEditar');

    const tblAgrupacion = $('#tblS_IncidenteAgrupacionCC');

    var id;
    var txtNombreAgrupa;

    var dtAgrupacion;
    let lstAgrupacion = [];
    let lstAgrupacionCC = [];
    let lstPrefijos = [];

    //#endregion
    var Inicializar = function () {
        $.namespace('AgrupacionCC.Agrupacion');

        Iniciar();

        CargarComboBusqueda();
        CargarComboCC();
        fcnbtnNuevo();
        fcnbtnLimpiar();
        fcnbtnBuscar();
        fcnbtnCrearEditar();
        fcnbtnEditar();
        // fncSelect2();
    }
    //#region TABLA 
    var Iniciar = function () {
        Agrupacion = function () {
            (function init() {
                initDataTblS_AgrupacionCC();
                cboAgrupacion.select2();
                Listener();
            })();
        }
    }

    function Listener() {
        txtNombreAgrupacion.attr("autocomplete", "off");
    }

    var initDataTblS_AgrupacionCC = function () {
        dtAgrupacion = tblAgrupacion.DataTable({
            language: dtDicEsp,
            ordering: true,
            paging: true,
            searching: true,
            bFilter: true,
            info: false,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'nombAgrupacion', title: 'Agrupación' },
                {
                    data: 'lstCC', title: 'Centro Costo', render: (data, type, row) => {

                        let html = "";
                        for (let i = 0; i < row.lstCC.length; i++) {
                            html += "<span class='btn btn-primary displayCC'><i class='fab fa-creative-commons-nd'>" + row.lstCC[i].cc + "</i></span>";
                        }
                        return html;
                    }
                },
                {
                    title: "Estatus",
                    render: function (data, type, row) {
                        let activo;
                        row.esActivo ? activo = "Activo" : activo = "Desactivado";
                        return activo;
                    }, visible: false
                },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                        row.esActivo ?
                            btnEliminar = `<button class='btn-eliminar btn btn-success eliminarAgrupacion' data-esActivo="1" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>` :
                            btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarAgrupacion' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-trash"></i></button>`;

                        return `<button class='btn-editar btn btn-warning editarAgrupacion' data-id="${row.id}">` +
                            `<i class='fas fa-pencil-alt'></i>` +
                            `</button>&nbsp;` + btnEliminar;
                    }
                }
            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "40%", "targets": [0, 2, 3] }
            ],
            initComplete: function (settings, json) {
                tblAgrupacion.on("click", ".eliminarAgrupacion", function () {
                    let esActivo = $(this).attr("data-esActivo");
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
                            fncEliminar($(this).attr("data-id"), esActivo);
                        }
                    });
                });

                tblAgrupacion.on("click", ".editarAgrupacion", function (e) {
                    const rowData = dtAgrupacion.row($(this).closest("tr")).data();


                    mdlEditarAgrupacion.modal("show");
                    AbrirModalEditar(rowData);

                });
            }
        });
    }
    var fncCargarTablaIncidentes = function () {
        let objFiltro = fncGetFiltros();
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/AgrupacionCC/GetDetalleAgrupacion",
            data: objFiltro,
            success: function (response) {
                if (response.success) {
                    dtAgrupacion.clear();
                    dtAgrupacion.rows.add(response.items);
                    dtAgrupacion.draw();
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
    //#endregion
    //#region FUNCIONES ENDPOINT
    var AbrirModalEditar = function (rowData) {
        var texto = rowData.nombAgrupacion;
        txtNombreAgrupacionEditar.val(texto);

        id = rowData.id;
        let lstDato = rowData.lstCC;
        var roots = rowData.lstCC.map(function (num) {
            return num.value.trim();
        });
        var lstAgrupa = rowData.lstCC.map(function (num) {
            return num.id;
        });


        cboComboAgrupaciones.val(rowData.id);
        cboComboAgrupaciones.trigger("change");
        lstAgrupacion = [];
        for (let i = 0; i < rowData.lstCC.length; i++) {
            let item =
            {
                id: rowData.lstCC[i].id,
                cc: rowData.lstCC[i].cc.split('-')[0].trim()
            }
            lstAgrupacion.push(item)
        }


        // var roots2 = rowData.lsKCtCC.map(num => num.cc.split('-')[0].trim());
        cboEditarCC.fillCombo('/AgrupacionCC/getCCTodos?idAgrupacionCC=' + rowData.id + '', null, null);

        cboEditarCC.val(roots);
        cboEditarCC.trigger("change");

    }

    var fncEliminar = function (id, esActivo) {
        let datos = { id: id, esActivo: esActivo };
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/AgrupacionCC/EliminarAgrupacion",
            data: datos,
            success: function (response) {
                if (!response.success) {
                    Alert2Error(response.message);
                }
                else {
                    let strMensaje = "";
                    strMensaje = "Se ha eliminado con éxito el registro.";
                    Alert2Exito(strMensaje);
                    CargarComboCC();
                    btnBuscar.click();
                    CargarComboBusqueda();
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

    var editarEditar = function (id, NuevoNombre, lstAgrupacion) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrativo/AgrupacionCC/EditarAgrupacion",
            data: {
                id: id,
                NuevoNombre: NuevoNombre,
                lstAgrupacion:
                    lstAgrupacion
            },
            success: function (response) {
                if (response.success) {
                    CargarComboCC();
                    btnBuscar.click();
                    Alert2Exito("Se ha registrado con éxito la agrupación");
                    CargarComboBusqueda();
                    mdlEditarAgrupacion.modal("hide");
                    fncCargarTablaIncidentes();
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            }
        });
    }
    var CreateEdit = function (valores) {
        let Parametros = {
            nomAgrupacion: txtNombreAgrupacion.val()
        }
        let lstAgrupaciones = fncFiltritos(valores);

        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrativo/AgrupacionCC/CrearAgrupacion",
            data: { objAgrupaciones: Parametros, lstAgrupaciones: lstAgrupaciones },
            success: function (response) {
                if (response.success) {

                    if (response.items.Exitoso == true) {
                        LimpiartFiltros();
                        CargarComboCC();
                        CargarComboBusqueda();
                        mdlCrearAgrupacion.modal("hide");
                        Alert2Exito("Se ha registrado con éxito la agrupación");
                        CargarComboBusqueda();
                    } else {
                        Alert2Warning(response.items.Mensaje);
                    }
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                fncCargarTablaIncidentes();
            }
        });
    }

    //#endregion
    //#region VALIDACIONES Y LIMPIEZA
    var ValidacionTextoVacioBuscar = function () {
        var valid = false;
        let strMensajeError = "";
        cboCCMultiple.val() == "" ? strMensajeError += "Es necesario seleccionar un cc." : false;
        txtNombreAgrupacion.val() == "" ? strMensajeError += "<br>Es necesario asignar un nombre a la agrupación." : false;
        strMensajeError != "" ? Alert2Warning(strMensajeError) : false;

        if (strMensajeError == "")
            valid = true
        else
            return valid;

        return valid;

        // if (cboCCMultiple.val() == "" || txtNombreAgrupacion.val() == "") {
        //     AlertaGeneral("Aviso", "Debe asignar alguna agrupacion o elegir centros de costos para continuar.");

        //     return valid;
        // } else {
        //     valid = true;
        // }
        // return valid;
    }
    var LimpiartFiltros = function (editar) {

        txtNombreAgrupacion.val('');
        cboCCMultiple.empty();
    }
    var fncFiltritos = function (valores) {
        let selCC;
        let Prefijo;
        let arreglo = []

        for (let i = 0; i < valores.length; i++) {
            selCC = '';
            Prefijo = '';
            selCC = $('#cboCCMultiple').find("option[value=" + valores[i] + "]");
            Prefijo = selCC.attr("data-prefijo");


            const element = {
                cc: valores[i].split('-')[1],
                idEmpresa: Prefijo
            };
            arreglo.push(element);
        }

        return arreglo;
    }
    var fncGetFiltros = function () {
        let objData = {};
        let id = cboAgrupacion.val();
        id == "" ? id = 0 : id = id;
        objData = {
            idAgrupacionCC: id,
        }
        return objData;
    }

    var ValidacionEditar = function () {
        var valid = false;
        let strMensajeError = "";
        cboEditarCC.val() == "" ? strMensajeError += "Es necesario seleccionar un cc." : false;
        txtNombreAgrupacionEditar.val() == "" ? strMensajeError += "<br>Es necesario asignar un nombre a la agrupación." : false;
        strMensajeError != "" ? Alert2Warning(strMensajeError) : false;

        if (strMensajeError == "")
            valid = true;
        else
            return valid;

        return valid;
    }
    //#endregion
    //#region BOTONES
    var fcnbtnNuevo = function () {
        btnNuevo.click(function (e) {
            txtNombreAgrupacion.val("");
            LimpiartFiltros();
            CargarComboCC();
        });
    }
    var fcnbtnLimpiar = function () {
        btnLimpiar.click(function (e) {

        });
    }

    var fcnbtnBuscar = function () {
        btnBuscar.click(function (e) {
            fncCargarTablaIncidentes();

        });
    }
    var fcnbtnCrearEditar = function () {
        btnCrearEditar.click(function (e) {

            var a = getValoresMultiples('#cboCCMultiple');

            var valid = ValidacionTextoVacioBuscar();
            if (valid == true) {
                CreateEdit(a);
            }
        });
    }

    var fcnbtnEditar = function () {
        btnEditarEditar.click(function (e) {
            txtNombreAgrupa = txtNombreAgrupacionEditar.val();
            lstAgrupacionCC = cboEditarCC.val();


            let valid = ValidacionEditar();
            if (valid == true) {
                editarEditar(id, txtNombreAgrupa, lstAgrupacionCC);
            }
        });
    }
    //#endregion
    //#region GET COMBOS
    var CargarComboBusqueda = function () {
        cboAgrupacion.fillCombo('/AgrupacionCC/ObtenerAgrupacion', null, null);
    }
    var CargarComboCC = function () {
        cboCCMultiple.fillCombo('/AgrupacionCC/getCC', null, null);
        cboComboAgrupaciones.fillCombo('/AgrupacionCC/obtenerAgrupacionCombo', null, null);
    }
    //#endregion

    //#region no sirve
    var FormatearLista = function (lst) {
        var returnLista = [];
        var returnr = [];
        var listaRegreso = [];
        var idAgrupacionCC = 0;
        var idCheck = 0;
        for (let i = 0; i < lst.length; i++) {
            let item = {
                id: lst[i].id,
                idAgrupacionCC: lst[i].idAgrupacionCC,
                cc: lst[i].cc,
            }
            returnr.push(item);
            if (idAgrupacionCC != lst[i].idAgrupacionCC) {
                idAgrupacionCC = lst[i].idAgrupacionCC;
                let item = {
                    id: lst[i].idAgrupacionCC,
                    nombAgrupacion: lst[i].nombAgrupacion
                }
                returnLista.push(item);
            }
        }
        for (let a = 0; a < returnLista.length; a++) {
            let itemArr = [];
            itemArr = [{
                id: returnLista[a].id,
                nombAgrupacion: returnLista[a].nombAgrupacion
            }]
            let item = [];
            for (let b = 0; b < returnr.length; b++) {
                if (returnLista[a].id == returnr[b].idAgrupacionCC) {
                    item.push(returnr[b])
                }
            }
            itemArr.push(item);
            item = [];
            listaRegreso.push(itemArr);
        }
        return listaRegreso;
    }
    var FormatearListastring = function (lst) {
        var returnLista = [];
        var returnr = [];
        var listaRegreso = "";
        var idAgrupacionCC = 0;
        var idCheck = 0;
        for (let i = 0; i < lst.length; i++) {
            let item = {
                id: lst[i].id,
                idAgrupacionCC: lst[i].idAgrupacionCC,
                cc: lst[i].cc,
            }
            returnr.push(item);
            if (idAgrupacionCC != lst[i].idAgrupacionCC) {
                idAgrupacionCC = lst[i].idAgrupacionCC;
                let item = {
                    id: lst[i].idAgrupacionCC,
                    nombAgrupacion: lst[i].nombAgrupacion
                }
                returnLista.push(item);
            }
        }
        for (let a = 0; a < returnLista.length; a++) {
            let itemArr = [];
            itemArr = "[lstAgrup:" + "{id:'" + returnLista[a].id + "',nombAgrupacion:'" + returnLista[a].nombAgrupacion + "'}";
            let item = "[ArrDatos:";
            for (let b = 0; b < returnr.length; b++) {
                if (returnLista[a].id == returnr[b].idAgrupacionCC) {
                    item += "{id:'" + returnr[a].id + "',idAgrupacionCC:'" + returnr[a].idAgrupacionCC + "',cc:'" + returnr[a].cc + "'},";
                }
            }
            item += "]";
            itemArr += item;
            item = "";
            listaRegreso += itemArr;
        }
        return listaRegreso;
    }
    //#endregion


    //#region ANEXOS
    var post = function (funcion, objFiltro) {
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/AgrupacionCC/" + funcion + "",
            data: objFiltro,
            success: function (response) {
                if (response.success) {

                    response.items

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
    $(document).ready(() => {
        AgrupacionCC.Agrupacion = new Agrupacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

    //#endregion
    return {
        Inicializar: Inicializar
    }
};