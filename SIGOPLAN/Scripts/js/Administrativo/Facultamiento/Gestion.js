(function () {
    $.namespace('administrativo.facultamiento.Gestion');
    Gestion = function () {
        tblCuadro = $("#tblCuadro");
        tblRefacMonto = $("#tblRefacMonto");
        tblMatMonto = $("#tblMatMonto");
        tblAuth = $("#tblAuth");
        selCC = $("#selCC");
        dtFecha = $("#dtFecha");
        txtObra = $("#txtObra");
        mdlCaptura = $("#mdlCaptura");
        mdlVerUpdate = $("#mdlVerUpdate");
        dialogVerOCUpdate = $("#dialogVerOCUpdate");
        addRefacMonto = $("#addRefacMonto");
        addMatMonto = $("#addMatMonto");
        remRefacMonto = $("#remRefacMonto");
        remMatMonto = $("#remMatMonto");
        btnGuardar = $("#btnGuardar");
        btnPrint = $("#btnPrint");
        lblLegend = $("#lblLegend");
        fsMateriales = $("#fsMateriales");
        tblPuesto = $('#tblPuesto');
        var isAdmin = false;
        function init() {
            initCaptura();
            initCuadro();
            initRefacMonto();
            initMatMonto();
            initAuth();
            initPuesto();
            getLstGestion();
            mdlCaptura.find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' });
            mdlVerUpdate.find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' });
            addRefacMonto.click(fnAddRefacMonto);
            addMatMonto.click(fnAddMatMonto);
            remRefacMonto.click(fnRemRefacMonto);
            remMatMonto.click(fnRemMatMonto);
            btnGuardar.click(valGuardado);
            btnPrint.click(fnReporte);
            selCC.change(getCuadro);
            mdlCaptura.on("hidden.bs.modal", function () { selCC.val("").change(); });
            mdlVerUpdate.on("hidden.bs.modal", function () { selCC.val("").change(); });
        }
        function getLstGestion() {
            let res = $.post("/Administrativo/facultamiento/getLstGestion");
            res.done(function (response) {
                if (response.success) {
                    AddRows(tblCuadro, response.lstGestion);
                }
            });
        }
        function saveFacultamiento() {
            mdlCaptura.block({ message: 'Guardando...' });
            let res = $.post("/Administrativo/facultamiento/saveFacultamiento", {
                obj: getObjFacultamiento(),
                lstMonto: getLstMonto(),
                lstAut: getLstAuto(),
                lstAuth: getLstAuth(),
                lstPuesto: getLstPuesto()
            })
                .done(function (response) {
                    if (response.success)
                        verReporte(btnGuardar.val(), false, response.vobo)
                })
                .done(function (response) {
                    if (response.success)
                        getLstGestion()
                })
                .done(function (response) {
                    if (response.success)
                        AlertaGeneral("Alerta", "Facultamiento guardado correctamente")
                });
        }
        function sendCorreo(vobo) {
            $.post("/Administrativo/facultamiento/sendCorreo", { vobo: vobo });
        }
        function verReporte(id, isReporte, vobo) {
            $("#report").attr("src", `/Reportes/Vista.aspx?idReporte=78&id=${id}&inMemory=${1}&isCRModal=${true}`);
            document.getElementById('report').onload = function () {
                if (isReporte)
                    openCRModal();
                else
                    sendCorreo(vobo);
                mdlCaptura.unblock();
            };
        }
        function getCuadro() {
            let isCCEmpty = selCC.val();
            if (isCCEmpty != '') {
                mdlCaptura.block({ message: 'Procesando...' });
                mdlVerUpdate.block({ message: 'Procesando...' });
                let res = $.post("/Administrativo/facultamiento/getCuadro", { cc: selCC.val(), fecha: dtFecha.val() });
                res.done(function (response) {
                    if (response.success) {
                        isAdmin = response.isAdmin;
                        txtObra.prop("disabled", response.isEditObra).val(response.cuadro.obra);
                        fsMateriales.prop("hidden", isAdmin);
                        //dtFecha.val(response.fecha);
                        btnPrint.prop("disabled", response.cuadro.id == 0);
                        btnGuardar.prop("disabled", false)
                        btnGuardar.val(response.cuadro.id).html(`<i class='fa fa-save'></i> ${response.cuadro.id == 0 ? 'Guardar' : 'Actualizar'}`);
                        lblLegend.text(isAdmin ? "Administrativo" : "Refacciones");
                        if (isAdmin) {
                            AddRows(tblRefacMonto, response.lstAdmin);
                        }
                        else {
                            AddRows(tblRefacMonto, response.lstRefact);
                            AddRows(tblMatMonto, response.lstMat);
                        }
                        AddRows(tblPuesto, response.lstPuesto);
                        AddRows(tblAuth, response.lstAuth);
                    }
                });
                res.always(function (a) {
                    mdlCaptura.unblock();
                    mdlVerUpdate.unblock();
                });
            } else {
                txtObra.val('');
                btnGuardar.val(0);
                btnGuardar.html("<i class='fa fa-save'></i> Guardar");
                btnGuardar.prop("disabled", true)
                btnPrint.prop("disabled", true);
                dtFecha.val(new Date().toLocaleDateString());
                ClearAllRows(tblRefacMonto);
                ClearAllRows(tblMatMonto);
                $(`#tblPuesto>tbody>tr>td>.form-control ,#tblAuth>tbody>tr>td>.form-control`).val("");
                $(`#tblAuth>tbody tr td:last-child`).text("No autorizó");
                dtAuth.rows().iterator('row', function (context, index) {
                    var data = this.row(index).data();
                    data.id = 0;
                    data.idFacultamiento = 0;
                    data.idUsuario = 0;
                    data.nombre = "";
                    data.auth = false;
                    data.firma = "";
                    data.fechaFirma = "";
                    data.motivoRechazo = "";
                    data.esRechazado = false;
                });
                dtPuesto.rows().iterator('row', function (context, index) {
                    var data = this.row(index).data();
                    data.id = 0;
                    data.idFacultamiento = 0;
                    data.puesto = "";
                });
            }
            addRefacMonto.prop("disabled", !isCCEmpty);
            remRefacMonto.prop("disabled", !isCCEmpty);
            addMatMonto.prop("disabled", !isCCEmpty);
            remMatMonto.prop("disabled", !isCCEmpty);
            dtFecha.prop("disabled", !isCCEmpty);
        }
        function valGuardado() {
            let ban = true;
            tblRefacMonto.find("tbody tr").each(function (idx, row) {
                $.each($(this).find(".auto"), function (i, e) {
                    if (!validarCampo($(this)))
                        ban = false;
                });
            });
            if (!validarCampo(tblAuth.find("tbody tr:eq(0) .auth")))
                ban = false;
            if (ban)
                saveFacultamiento();
            return ban;
        }
        function validarCampo(_this) {
            var r = false;
            if (_this.val().length == 0) {
                if (!_this.hasClass("errorClass")) {
                    _this.addClass("errorClass")
                }
                r = false;
            }
            else {
                if (_this.hasClass("errorClass")) {
                    _this.removeClass("errorClass")
                }
                r = true;
            }
            return r;
        }
        function getObjFacultamiento() {
            return {
                id: btnGuardar.val(),
                obra: txtObra.val(),
                cc: selCC.val(),
                fecha: dtFecha.val()
            }
        }
        function getLstMonto() {
            let lst = [];
            let dt = tblRefacMonto.DataTable();
            tblRefacMonto.find("tbody tr").each(function (idx, row) {
                lst.push({
                    id: dt.rows().data()[idx].monto.id,
                    idFacultamiento: dt.rows().data()[idx].monto.idFacultamiento,
                    idTabla: dt.rows().data()[idx].monto.idTabla,
                    renglon: idx + 1,
                    max: unmaskDinero($(this).find(".max").val()),
                    min: unmaskDinero($(this).find(".min").val())
                });
            });
            if (!isAdmin && tblMatMonto.find('tbody tr').length > 0) {
                dt = tblMatMonto.DataTable();
                tblMatMonto.find("tbody tr").each(function (idx, row) {
                    lst.push({
                        id: dt.rows().data()[idx].monto.id,
                        idFacultamiento: dt.rows().data()[idx].monto.idFacultamiento,
                        idTabla: dt.rows().data()[idx].monto.idTabla,
                        renglon: idx + 1,
                        max: unmaskDinero($(this).find(".max").val()),
                        min: unmaskDinero($(this).find(".min").val())
                    });
                });
            }
            return lst;
        }
        function getLstAuto() {
            let lst = [];
            let dt = tblRefacMonto.DataTable();
            tblRefacMonto.find("tbody tr").each(function (idx, row) {
                $(row).find("td").each(function (idy, td) {
                    $(td).find(".form-inline").each(function (idz, div) {
                        let data = dt.rows().data()[idx];
                        lst.push({
                            id: 0,
                            idMonto: data.monto.id,
                            idTitulo: $(this).find(".titulo").val(),
                            idTipoAutorizacion: idy - 1,
                            renglon: idx + 1,
                            cve: $(this).find(".nombre").data().cve,
                            nombre: $(this).find(".nombre").val(),
                            descPuesto: ""
                        });
                    });
                });
            });
            if (!isAdmin && tblMatMonto.find('tbody tr').length > 0) {
                dt = tblMatMonto.DataTable();
                tblMatMonto.find("tbody tr").each(function (idx, row) {
                    $(row).find("td").each(function (idy, td) {
                        $(td).find(".form-inline").each(function (idz, div) {
                            let data = dt.rows().data()[idx];
                            lst.push({
                                id: 0,
                                idMonto: data.monto.id,
                                idTitulo: $(this).find(".titulo").val(),
                                idTipoAutorizacion: idy - 1,
                                renglon: idx + 1,
                                cve: $(this).find(".nombre").data().cve,
                                nombre: $(this).find(".nombre").val(),
                                descPuesto: ""
                            });
                        });
                    });
                });
            }
            return lst;
        }
        function getLstAuth() {
            let lst = [];
            tblAuth.find("tbody tr").each(function (idx, row) {
                lst.push({
                    orden: idx,
                    idUsuario: $(this).find('.auth').data().idUsuario,
                    nombre: $(this).find('.auth').val(),
                    firma: ""
                });
            });
            return lst;
        }
        function getLstPuesto() {
            let lst = [];
            dt = tblPuesto.DataTable();
            tblPuesto.find(`tbody tr:not(.success)`).each(function (idx, row) {
                let data = dt.rows().data()[idx];
                lst.push({
                    id: data.id,
                    idFacultamiento: data.idFacultamiento,
                    idTabla: data.idTabla,
                    orden: data.orden,
                    puesto: $(this).find("td > .puesto").val()
                });
            });
            return lst;
        }
        function fnReporte() {
            verReporte(btnGuardar.val(), true, null);
        }
        function fnAddRefacMonto() {
            let dt = tblRefacMonto.DataTable(),
                renglon = dt.rows().count() + 1;
            AddRow(dt, {
                monto: {
                    id: 0,
                    idFacultamiento: 0,
                    idTabla: dt.rows().data()[0].monto.idTabla,
                    renglon: renglon,
                    max: 0,
                    min: 0
                },
                lstAuto: [
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 1,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    },
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 2,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    },
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 3,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    }
                ]
            });
        }
        function fnAddMatMonto() {
            let dt = tblMatMonto.DataTable();
            renglon = dt.rows().count() + 1;
            AddRow(dt, {
                monto: {
                    id: 0,
                    idFacultamiento: 0,
                    idTabla: dt.rows().data()[0].monto.idTabla,
                    renglon: renglon,
                    max: 0,
                    min: 0
                },
                lstAuto: [
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 1,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    },
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 2,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    },
                    {
                        id: 0,
                        idMonto: 0,
                        idTitulo: 0,
                        idTipoAutorizacion: 3,
                        renglon: renglon,
                        cve: 0,
                        nombre: "",
                        descPuesto: ""
                    }
                ]
            });
        }
        function fnRemRefacMonto() {
            let dt = tblRefacMonto.DataTable();
            if (dt.rows().count() != 1) {
                dt.row(':last').remove().draw(false);
            }
        }
        function fnRemMatMonto() {
            let dt = tblMatMonto.DataTable();
            if (dt.rows().count() != 1) {
                dt.row(':last').remove().draw(false);
            }
        }
        function ClearAllRows(tbl) {
            tbl.DataTable().clear().draw();
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst) {
                if (lst.hasOwnProperty(i)) {
                    AddRow(dt, lst[i]);
                }
            }
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        function initCaptura() {
            selCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            dtFecha.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            addRefacMonto.prop("disabled", true);
            remRefacMonto.prop("disabled", true);
            addMatMonto.prop("disabled", true);
            remMatMonto.prop("disabled", true);
            txtObra.prop("disabled", true);
            dtFecha.prop("disabled", true);
            btnGuardar.prop("disabled", true)
            btnPrint.prop("disabled", true);
        }
        function initCuadro() {
            dtCuadro = tblCuadro.DataTable({
                language: dtDicEsp,
                columns: [
                    { data: 'cc' },
                    { data: 'obra' },
                    { data: 'fecha' },
                    { data: 'estatus' },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><i></i> Actualizar</button>");
                            $(td).find("button").addClass('btn btn-success editar');
                            $(td).find("i").addClass('fa fa-edit');
                        }
                    },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><i></i> Reporte</button>");
                            $(td).find("button").addClass('btn btn-primary report');
                            $(td).find("i").addClass('fa fa-print');
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCuadro.on('click', '.report', function () {
                        verReporte(dtCuadro.row($(this).parents('tr')).data().id, true, null);
                    });
                    tblCuadro.on('click', '.editar', function () {
                        selCC.val(dtCuadro.row($(this).parents('tr')).data().cc).change();
                        mdlCaptura.modal('toggle');
                    });
                }
            });
        }
        function initRefacMonto() {
            var dtRefac;
            dtRefac = tblRefacMonto.DataTable({
                info: false,
                paging: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                "ordering": false,
                columns: [
                    {
                        data: 'monto.min',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).width("140px");
                            $(td).html("<input></input>");
                            $(td).find("input").addClass('form-control text-right dinero min').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let val = cellData == 0 ? "En adelante" : maskDinero(cellData);
                            $(td).width("140px");
                            $(td).html("<input></input>");
                            $(td).find("input").addClass('form-control text-right dinero max').val(val);
                        }
                    },
                    {
                        data: 'lstAuto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 1));
                        }
                    },
                    {
                        data: 'lstAuto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 2));
                        }
                    },
                    {
                        data: 'lstAuto',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 3));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRefacMonto.on('change', '.dinero', function () {
                        this.value = maskDinero(unmaskDinero(this.value));
                    });
                    tblRefacMonto.on('change', '.max', function (row) {
                        let max = unmaskDinero(this.value);
                        if (max == 0) {
                            this.value = "En adelante"
                        } else {
                            min = $(this).closest('tr').next('tr').find('.min');
                            min.val(maskDinero(max + 1));
                        }
                    });
                    tblRefacMonto.on('click', '.btn-success', function () {
                        let div = $(this).closest('td').find('.form-inline');
                        if (div.length < 3) {
                            let idTipoAutorizacion = dtRefac.row($(this).parents('tr')).data().lstAuto[0].idTipoAutorizacion;
                            $(div[0]).append(createDivEmpleado(idTipoAutorizacion, false));
                        }
                    });
                    tblRefacMonto.on('click', '.btn-danger', function () {
                        let div = $(this).closest('td').find('.form-inline');
                        if (div.length > 1)
                            $(div[div.length - 1]).remove();
                    });
                }
            });
        }
        function initMatMonto() {
            var dtMat;
            dtMat = tblMatMonto.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                "ordering": false,
                columns: [
                    {
                        data: 'monto.min',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).width("140px");
                            $(td).html("<input></input>");
                            $(td).find("input").addClass('form-control text-right dinero min').val(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            let val = cellData == 0 ? "En adelante" : maskDinero(cellData);
                            $(td).width("140px");
                            $(td).html("<input></input>");
                            $(td).find("input").addClass('form-control text-right dinero max').val(val);
                        }
                    },
                    {
                        data: 'lstAuto',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 1));
                        }
                    },
                    {
                        data: 'lstAuto',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 2));
                        }
                    },
                    {
                        data: 'lstAuto',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createTdEmpleado(rowData.lstAuto, 3));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblMatMonto.on('change', '.dinero', function () {
                        this.value = maskDinero(unmaskDinero(this.value));
                    });
                    tblMatMonto.on('change', '.max', function (row) {
                        let max = unmaskDinero(this.value);
                        if (max == 0) {
                            this.value = "En adelante"
                        } else {
                            min = $(this).closest('tr').next('tr').find('.min');
                            min.val(maskDinero(max + 1));
                        }
                    });
                    tblMatMonto.on('click', '.btn-success', function () {
                        let div = $(this).closest('td').find('.form-inline');
                        if (div.length < 3) {
                            let idTipoAutorizacion = dtMat.row($(this).parents('tr')).data().lstAuto[0].idTipoAutorizacion;
                            $(div[0]).append(createDivEmpleado(idTipoAutorizacion, false));
                        }
                    });
                    tblMatMonto.on('click', '.btn-danger', function () {
                        let div = $(this).closest('td').find('.form-inline');
                        if (div.length > 1)
                            $(div[div.length - 1]).remove();
                    });
                }
            });
        }
        function initAuth() {
            dtAuth = tblAuth.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                columns: [
                    {
                        data: 'orden',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`${cellData == 0 ? "Autorizá" : `Vobo ${cellData}`}`);
                        }
                    },
                    {
                        data: 'nombre',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(createAuth(cellData, rowData.idUsuario, rowData.orden));
                        }
                    },
                    {
                        data: 'auth',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`${rowData.esRechazado ? "Rechazó" : cellData ? "Autorizó" : "No autorizó"}`);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblAuth.on('change', '.auth', function () {
                        let auth = dtAuth.row($(this).parents('tr')).data();
                        $(this).closest('td').next().text(`${auth.nombre == this.value ? auth.esRechazado ? "Rechazó" : auth.auth ? "Autorizó" : "No Autorizó" : "No autorizó"}`);
                    });
                }
            });
        }
        function initPuesto() {
            dtPuesto = tblPuesto.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                drawCallback: function (settings) {
                    var api = this.api(),
                        rows = api.rows({ page: 'current' }).nodes(),
                        last = null,
                        data = api.data();
                    api.column(2, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(`<tr class="group success"><td colspan="2">${group}</td></tr>`);
                            last = group;
                        }
                    });
                },
                columns: [
                    {
                        data: 'puesto',
                        sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<input></input>");
                            $(td).find("input").addClass("form-control puesto");
                            $(td).find("input").val(cellData);
                            $(td).find("input").getAutocomplete(null, null, '/Administrativo/Facultamiento/geDesctPuesto');
                        }
                    },
                    { data: 'auth', sortable: false, },
                    { data: 'tipo', sortable: false, visible: false, targets: 2 },
                    { data: 'idTabla', sortable: false, visible: false, targets: 2 },
                ],
                initComplete: function (settings, json) {
                    tblPuesto.DataTable().order([3, 'asc']).draw();
                }
            });
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function createTdEmpleado(lst, autorizacion) {
            let div = $('<div>'),
                ban = false;
            $.each(lst, (idx, x) => {
                if (x.idTipoAutorizacion == autorizacion && x.nombre != null && x.nombre.length > 0) {
                    let elemnt = $('<div>').addClass("form-inline");
                    let sel = $("<select></select>"),
                        inp = $("<input></input>"),
                        btn = $("<button/>"),
                        ico = $("<i></i>");
                    sel.fillCombo('/Administrativo/Facultamiento/fillComboTitulo', null, false, null);
                    inp.addClass(`form-control nombre ${x.idTipoAutorizacion == 1 ? "auto" : "vobo"}`).width("77%").val(x.nombre).data().cve = x.cve;
                    inp.getAutocomplete(setCve, null, '/OT/getEmpleados');
                    sel.addClass('form-control titulo').width("15%");
                    sel.find('option[value="' + x.idTitulo + '"]').attr('selected', true);
                    btn.addClass(`btn ${div[0].childNodes.length == 0 ? "btn-success" : "btn-danger"}`);
                    ico.addClass(`fa ${div[0].childNodes.length == 0 ? "fa-plus" : "fa-minus"}`);
                    elemnt.prepend(btn.prepend(ico));
                    elemnt.prepend(inp);
                    elemnt.prepend(sel);
                    div.append(elemnt);
                    ban = true;
                }
            });
            if (!ban) div.append(createDivEmpleado(autorizacion, true))
            return div;
        }
        function createDivEmpleado(idTipoAutorizacion, btnTipo) {
            let elemnt = $('<div>').addClass("form-inline"),
                sel = $("<select></select>"),
                inp = $("<input></input>"),
                btn = $("<button/>"),
                ico = $("<i></i>");
            sel.fillCombo('/Administrativo/Facultamiento/fillComboTitulo', null, false, null);
            inp.addClass(`form-control nombre ${idTipoAutorizacion == 1 ? "auto" : "vobo"}`).width("77%").data().cve = 0;
            inp.getAutocomplete(setCve, null, '/OT/getEmpleados');
            sel.addClass('form-control titulo').width("15%").val(0);
            btn.addClass(`btn ${btnTipo ? "btn-success" : "btn-danger"} `);
            ico.addClass(`fa ${btnTipo ? "fa-plus" : "fa-minus"}`);
            elemnt.prepend(btn.prepend(ico));
            elemnt.prepend(inp);
            elemnt.prepend(sel);
            return elemnt;
        }
        function createAuth(nombre, idUsuario, orden) {
            if (orden == 0 && idUsuario == 1080) {
                let inp = $("<input readonly></input>");
                inp.addClass(`form-control auth`).val(nombre).data().idUsuario = idUsuario;
                inp.getAutocomplete(setIdUsuario, null, '/Administrativo/Facultamiento/getEmpleadosSigoplan');
                return inp;
            }
            else if (orden == 0 && idUsuario != 1080) {
                let inp = $("<input></input>");
                inp.addClass(`form-control auth`).val(nombre).data().idUsuario = idUsuario;
                inp.getAutocomplete(setIdUsuario, null, '/Administrativo/Facultamiento/getEmpleadosSigoplan');
                return inp;
            }
            else {
                let inp = $("<input></input>");
                inp.addClass(`form-control auth`).val(nombre).data().idUsuario = idUsuario;
                inp.getAutocomplete(setIdUsuario, null, '/Administrativo/Facultamiento/getEmpleadosSigoplanNOAG');
                return inp;
            }
        }
        function setCve(e, ui) {
            $(this).data().cve = ui.item.id;
        }
        function setIdUsuario(e, ui) {
            $(this).data().idUsuario = ui.item.id;
        }
        init();
    }
    $(document).ready(function () {
        administrativo.facultamiento.Gestion = new Gestion();
    })
        .ajaxStart(function () {
            var height = $(window).height();
            $(".jquery-ui-dialog-overlay-element").css('height', height);
            $.blockUI({ message: 'Procesando...' });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();