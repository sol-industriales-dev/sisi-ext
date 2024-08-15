(function () {
    $.namespace('administrativo.facultamiento.Autorizacion');
    Autorizacion = function () {
        txtMotivoRechazo = $('#txtMotivoRechazo');
        btnRechazar = $('#btnRechazar');
        mdlRechazo = $('#mdlRechazo');
        tblCuadro = $("#tblCuadro");
        fsCuadro = $("#fsCuadro");
        fsReport = $("#fsReport");
        fsAuth = $("#fsAuth");
        fsPuestoAnt = $("#fsPuestoAnt");
        fsPuestoActual = $("#fsPuestoActual");
        lgHome = $("#lgHome");
        report = $("#report");
        menuTab = $("#menuTab");
        tabComparacion = $("#Comparacion");
        btnReporte = $("#btnReporte");
        btnComparacion = $("#btnComparacion");
        tblRefacVer = $("#tblRefacVer");
        tblRefacActual = $("#tblRefacActual");
        tblMatVer = $("#tblMatVer");
        tblMatActual = $("#tblMatActual");
        fsMaterialesAnt = $("#fsMaterialesAnt");
        fsMaterialesAct = $("#fsMaterialesAct");
        lbllegendAnt = $("#lbllegendAnt");
        lbllegendAct = $("#lbllegendAct");
        tblPuestoVer = $("#tblPuestoVer");
        tblPuestoAct = $("#tblPuestoAct");
        idAuth = 0;
        idFacul = 0;
        MontoAnterior = 0;
        MontoActual = 0;
        MatAnterior = 0;
        MatActual = 0;
        PuestoAnterior = 0;
        PuestoActual = 0;
        function init() {
            initForm();
            initRefacAnterior();
            initMatAnterior();
            initPuestoAnterior();
            initRefacActual();
            initMatActual();
            initPuestoActual();
            lgHome.click(regresar);
            btnRechazar.click(setRechazo);
        }
        btnReporte.click(function (e) {
            openTab(e, 'Reporte');
        });
        btnComparacion.click(function (e) {
            openTab(e, 'Comparacion');
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function getLstGestion() {
            let res = $.post("/Administrativo/facultamiento/getLstGestionNoAuth");
            res.done(function (response) {
                if (response.success) {

                    AddRows(tblCuadro, response.lstGestion);
                }
            });
        }
        function sendCorreo(vobo) {
            $.post("/Administrativo/facultamiento/sendCorreo", { vobo: vobo });
        }
        function limpiarReporte(isReporte) {
            if (isReporte) {
                fsCuadro.prop("hidden", true);
                menuTab.prop("hidden", false);
                fsReport.prop("hidden", false);
                fsAuth.prop("hidden", false);
                tabComparacion.prop("hidden", false);
            }
        }

        function alinearheader() {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            //$.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }

        function verReporte(id, isReporte, vobo) {
            if (isReporte) {
                fsCuadro.prop("hidden", true);
                menuTab.prop("hidden", false);
                fsReport.prop("hidden", false);
                fsAuth.prop("hidden", false);
                tabComparacion.prop("hidden", false);
            }
            report.attr("src", `/Reportes/Vista.aspx?idReporte=${78}&id=${id}&inMemory=${1}&isCRModal=${false}`);
            document.getElementById('report').onload = function () {
                if (!isReporte)
                    sendCorreo(vobo);
                $(".auth").click(setAuth);
            };

        }

        function getLstAutorizacion(id) {
            let res = $.post("/Administrativo/facultamiento/getLstAutorizacion", { id: id });
            res.done(function (response) {
                if (response.success) {
                    setPanel(response.lstAuth);
                }
            });
        }
        function getFacAnterior(cc, fecha, idActual) {

            let res = $.post("/Administrativo/facultamiento/getFacAnterior", { cc: cc, fecha: fecha, idActual: idActual });
            res.done(function (response) {
                if (response.success) {
                    const anterior = response.objAnterior;
                    const actual = response.objActual;
                    const isAdmin = response.isAdmin;
                    const esArrendadora = response.esArrendadora;

                    fsMaterialesAnt.prop("hidden", isAdmin);
                    fsMaterialesAct.prop("hidden", isAdmin);
                    lbllegendAnt.text(isAdmin ? "Administrativo" : "Refacciones");
                    lbllegendAct.text(isAdmin ? "Administrativo" : "Refacciones");

                    $("#txtFechaVer").val(anterior.fecha);
                    $("#txtCCVer").val(anterior.cc + '-' + anterior.ccNombre);
                    $("#txtObraVer").val(anterior.obra);


                    $("#txtFechaActual").val(actual.fecha);
                    $("#txtCCActual").val(actual.cc + '-' + anterior.ccNombre);
                    $("#txtObraActual").val(actual.obra);
                    $("#lblUsuarioUpdate").text(actual.usuario);

                    if (anterior.fecha != actual.fecha)
                        $("#txtFechaActual").css("backgroundColor", "yellow");

                    if (esArrendadora) {
                        fsPuestoActual.prop('hidden', true);
                        fsPuestoAnt.prop('hidden', true);
                    } else {
                        fsPuestoActual.prop('hidden', false);
                        fsPuestoAnt.prop('hidden', false);
                    }

                    if (isAdmin) {
                        MontoAnterior = response.lstAdmin;
                        MontoActual = response.lstAdminActual;

                        AddRows(tblRefacVer, response.lstAdmin);
                        AddRows(tblRefacActual, response.lstAdminActual);

                        agregarEliminados(MontoAnterior, MontoActual, tblRefacActual);


                    }
                    else {
                        MontoAnterior = response.lstRefactAnt;
                        MontoActual = response.lstRefactAct;

                        MatAnterior = response.lstMatAnt;
                        MatActual = response.lstMatAct;

                        AddRows(tblRefacVer, response.lstRefactAnt);
                        AddRows(tblMatVer, response.lstMatAnt);

                        AddRows(tblRefacActual, response.lstRefactAct);
                        AddRows(tblMatActual, response.lstMatAct);

                        agregarEliminados(MontoAnterior, MontoActual, tblRefacActual);
                        agregarEliminados(MatAnterior, MatActual, tblMatActual);
                    }
                    PuestoAnterior = response.objPuestoAnt;
                    PuestoActual = response.objPuestoAct;

                    AddRows(tblPuestoVer, response.objPuestoAnt);
                    AddRows(tblPuestoAct, response.objPuestoAct);


                    btnComparacion.show();
                    btnComparacion.click();
                } else {
                    btnComparacion.hide();
                    btnReporte.click();
                }
            });
        }
        function initRefacAnterior() {
            tblRefacVer.DataTable({
                sScrollX: "100%",
                sScrollXInner: "110%",
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                drawCallback: function () {
                    tblRefacVer.DataTable().columns.adjust();
                },
                columns: [
                    {
                        data: 'monto.min',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let valor = cellData == 0 ? "En adelante" : maskDinero(cellData)
                            $(td).html(valor);
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
            });
        }
        function initMatAnterior() {
            tblMatVer.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                columns: [
                    {
                        data: 'monto.min',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let valor = cellData == 0 ? "En adelante" : maskDinero(cellData);
                            $(td).html(valor);
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
            });
        }
        function initPuestoAnterior() {
            dtPuesto = tblPuestoVer.DataTable({
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
                            $(td).html("<input disabled></input>");
                            $(td).find("input").addClass("form-control puesto");
                            $(td).find("input").val(cellData);
                        }
                    },
                    { data: 'auth', sortable: false, },
                    { data: 'tipo', sortable: false, visible: false, targets: 2 },
                    { data: 'idTabla', sortable: false, visible: false, targets: 2 },
                ],
                initComplete: function (settings, json) {
                    tblPuestoVer.DataTable().order([3, 'asc']).draw();
                }
            });
        }
        function initRefacActual() {
            tblRefacActual.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                scrollX: true,
                language: dtDicEsp,
                iDisplayLength: -1,
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (MontoAnterior[iDisplayIndex] == void 0) { /* void 0 === undefined */  // MontoAnterior[iDisplayIndex]["lstAuto"][iDisplayIndex] == void 0 
                        $('td', nRow).css('background-color', 'yellow');
                    }
                    else {
                        if (aData["monto"]["min"] != MontoAnterior[iDisplayIndex]["monto"]["min"])
                            $(nRow.childNodes[0]).css('background-color', 'yellow');
                        if (aData["monto"]["max"] != MontoAnterior[iDisplayIndex]["monto"]["max"])
                            $(nRow.childNodes[1]).css('background-color', 'yellow');

                        for (let i = 0; i < MontoAnterior[iDisplayIndex]["lstAuto"].length; i++)
                            if (aData["lstAuto"][i] == void 0) {
                                switch (MontoAnterior[iDisplayIndex]["lstAuto"][i]["idTipoAutorizacion"]) {
                                    case 1:
                                        $(nRow.childNodes[2]).css('background-color', 'yellow');
                                        break;
                                    case 2:
                                        $(nRow.childNodes[3]).css('background-color', 'yellow');
                                        break;
                                    case 3:
                                        $(nRow.childNodes[4]).css('background-color', 'yellow');
                                        break;
                                }

                            }
                            else {
                                if (aData["lstAuto"][i]["renglon"] == MontoAnterior[iDisplayIndex]["lstAuto"][i]["renglon"]) {
                                    if (aData["lstAuto"][i]["idTipoAutorizacion"] == MontoAnterior[iDisplayIndex]["lstAuto"][i]["idTipoAutorizacion"]) {

                                        if (aData["lstAuto"][i]["nombre"] != MontoAnterior[iDisplayIndex]["lstAuto"][i]["nombre"]) {
                                            switch (aData["lstAuto"][i]["idTipoAutorizacion"]) {
                                                case 1:
                                                    $(nRow.childNodes[2]).css('background-color', 'yellow');
                                                    break;
                                                case 2:
                                                    $(nRow.childNodes[3]).css('background-color', 'yellow');
                                                    break;
                                                case 3:
                                                    $(nRow.childNodes[4]).css('background-color', 'yellow');
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }

                    }
                },
                columns: [
                    {
                        data: 'monto.min',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let valor = cellData == 0 ? "En adelante" : maskDinero(cellData)
                            $(td).html(valor);
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
            });
        }
        function initMatActual() {
            tblMatActual.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (MatAnterior[iDisplayIndex] == void 0) { /* void 0 === undefined */ //|| MatAnterior[iDisplayIndex]["lstAuto"][iDisplayIndex] == void 0
                        $('td', nRow).css('background-color', 'yellow');
                    }
                    else {
                        if (aData["monto"]["min"] != MatAnterior[iDisplayIndex]["monto"]["min"])
                            $(nRow.childNodes[0]).css('background-color', 'yellow');
                        if (aData["monto"]["max"] != MatAnterior[iDisplayIndex]["monto"]["max"])
                            $(nRow.childNodes[1]).css('background-color', 'yellow');


                        for (let i = 0; i < aData["lstAuto"].length; i++)
                            if (MatAnterior[iDisplayIndex]["lstAuto"][i] == void 0) {

                                switch (aData["lstAuto"][i]["idTipoAutorizacion"]) {
                                    case 1:
                                        $(nRow.childNodes[2]).css('background-color', 'yellow');
                                        break;
                                    case 2:
                                        $(nRow.childNodes[3]).css('background-color', 'yellow');
                                        break;
                                    case 3:
                                        $(nRow.childNodes[4]).css('background-color', 'yellow');
                                        break;
                                }


                            } else {
                                if (aData["lstAuto"][i]["renglon"] == MatAnterior[iDisplayIndex]["lstAuto"][i]["renglon"]) {
                                    if (aData["lstAuto"][i]["idTipoAutorizacion"] == MatAnterior[iDisplayIndex]["lstAuto"][i]["idTipoAutorizacion"]) {

                                        if (aData["lstAuto"][i]["nombre"] != MatAnterior[iDisplayIndex]["lstAuto"][i]["nombre"]) {
                                            switch (aData["lstAuto"][i]["idTipoAutorizacion"]) {
                                                case 1:
                                                    $(nRow.childNodes[2]).css('background-color', 'yellow');
                                                    break;
                                                case 2:
                                                    $(nRow.childNodes[3]).css('background-color', 'yellow');
                                                    break;
                                                case 3:
                                                    $(nRow.childNodes[4]).css('background-color', 'yellow');
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                    }
                },
                columns: [
                    {
                        data: 'monto.min',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'monto.max',
                        createdCell: function (td, cellData, rowData, row, col) {
                            let valor = cellData == 0 ? "En adelante" : maskDinero(cellData)
                            $(td).html(valor);
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
            });
        }
        function initPuestoActual() {
            dtPuesto = tblPuestoAct.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (PuestoAnterior[iDisplayIndex] == void 0) { /* void 0 === undefined */  // MontoAnterior[iDisplayIndex]["lstAuto"][iDisplayIndex] == void 0 
                        $('td', nRow).css('background-color', 'yellow');
                    }
                    else {
                        if (aData["idTabla"] == PuestoAnterior[iDisplayIndex]["idTabla"])
                            if (aData["orden"] == PuestoAnterior[iDisplayIndex]["orden"])
                                if (PuestoAnterior[iDisplayIndex]["puesto"] != null && aData["puesto"] != PuestoAnterior[iDisplayIndex]["puesto"])
                                    $(nRow.childNodes[0]).css('background-color', 'yellow');
                    }
                },
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
                            $(td).html("<input disabled></input>");
                            $(td).find("input").addClass("form-control puesto");
                            $(td).find("input").val(cellData);
                        }
                    },
                    { data: 'auth', sortable: false, },
                    { data: 'tipo', sortable: false, visible: false, targets: 2 },
                    { data: 'idTabla', sortable: false, visible: false, targets: 2 },
                ],
                initComplete: function (settings, json) {
                    tblPuestoAct.DataTable().order([3, 'asc']).draw();
                }
            });
        }
        function createTdEmpleado(lst, autorizacion) {
            let div = $('<div>'), ban = false;

            $.each(lst, (idx, x) => {
                if (x.idTipoAutorizacion == autorizacion && x.nombre != null && x.nombre.length > 0) {
                    let elemnt = $('<div>');
                    let lbl = $("<span></span>")
                    lbl.text(x.nombre);
                    elemnt.prepend(lbl);
                    div.append(elemnt);
                    ban = true;
                }
            });
            return div;
        }
        function setAuth() {
            let st = this.value;
            if (st == 2) {
                let res = $.post("/Administrativo/facultamiento/setAutorizacion", { id: idAuth });
                res.done(function (response) {
                    if (response.success) {
                        $.when(
                            verReporte(idFacul, response.vobo == "", response.vobo)
                            , getLstAutorizacion(idFacul)
                            , getLstGestion()
                        );
                    }
                });
            }
            else {
                txtMotivoRechazo.val("");
                mdlRechazo.modal("show");
            }
        }
        function openTab(evt, tabName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(tabName).style.display = "block";
            evt.currentTarget.className += " active";

            if (tabName == 'Reporte') verReporte(idFacul, true, null);
        }
        function agregarEliminados(listaAnt, listaAct, table) {
            let mainRow = 0;
            for (let i = 0; i < listaAnt.length; i++)
                if (listaAct[i] == void 0) {
                    let min = maskDinero(listaAnt[i]["monto"]["min"]);
                    let max = listaAnt[i]["monto"]["max"] == 0 ? "En adelante" : maskDinero(listaAnt[i]["monto"]["max"]);
                    let td = $('<td>')
                    let auth = '';
                    let vobo1 = '';
                    let vobo2 = '';
                    let markup = '';

                    if (listaAnt[i]["lstAuto"].length > 0) {
                        for (let j = 0; j < listaAnt[i]["lstAuto"].length; j++) {
                            switch (listaAnt[i]["lstAuto"][j]["idTipoAutorizacion"]) {
                                case 1:
                                    auth = $("<td>").html(createTdEmpleado(listaAnt[i]["lstAuto"], 1));
                                    break;
                                case 2:
                                    vobo1 = $("<td>").html(createTdEmpleado(listaAnt[i]["lstAuto"], 2));
                                    break;
                                case 3:
                                    vobo2 = $("<td>").html(createTdEmpleado(listaAnt[i]["lstAuto"], 3));
                                    break;
                            }
                        }
                    }
                    else {
                        let div = $('<div>')
                        let elemnt = $('<div>');
                        let lbl = $("<span></span>")
                        lbl.text(" ");
                        elemnt.prepend(lbl);
                        td.append(elemnt);

                        auth = td;
                        vobo1 = td;
                        vobo2 = td;
                    }


                    markup = "<tr style='background-color: red !important;'> <td>" + min + "</td> <td>" + max + "</td>" + auth[0].outerHTML + vobo1[0].outerHTML + vobo2[0].outerHTML + "</tr>";
                    table.append(markup);
                    mainRow++;
                }
            if (mainRow > 0) table.removeClass("table-striped > tbody > tr:nth-child(odd) > td, .table-striped > tbody > tr:nth-child(odd) > th");
            else table.addClass("table-striped > tbody > tr:nth-child(odd) > td, .table-striped > tbody > tr:nth-child(odd) > th");
        }
        function setRechazo() {
            let motivo = txtMotivoRechazo.val();
            if (motivo && motivo.length >= 10) {
                let res = $.post("/Administrativo/facultamiento/setRechazo", { id: idAuth, motivo: motivo });
                res.done(function (response) {
                    if (response.success) {
                        $.when(
                            getLstAutorizacion(idFacul)
                            , getLstGestion()
                        );
                        mdlRechazo.modal("hide");
                        txtMotivoRechazo.removeClass("danger");
                    }
                });
            } else {
                txtMotivoRechazo.addClass("danger");
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }
        }
        function initCuadro() {
            dtCuadro = tblCuadro.DataTable({
                language: dtDicEsp,
                columns: [
                    { data: 'cc' },
                    { data: 'obra' },
                    { data: 'fecha' },
                    {
                        data: 'estatus',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<button><i></i> ${cellData ? "Rechazado" : "En espera"}</button>`);
                            $(td).find("button").addClass(`btn ${cellData ? "btn-danger" : "btn-primary"} auto`);
                            $(td).find("i").addClass(`fa ${cellData ? 'fa-ban' : 'fa-user'}`);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCuadro.on('click', '.auto', function () {
                        let id = dtCuadro.row($(this).parents('tr')).data().id;
                        let cc = dtCuadro.row($(this).parents('tr')).data().cc;
                        let fecha = dtCuadro.row($(this).parents('tr')).data().fecha;
                        $.when(
                            idFacul = id
                            , getLstAutorizacion(id)
                            , getFacAnterior(cc, fecha, id)
                            , limpiarReporte(true)

                        )
                    });
                }
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        function regresar() {
            fsCuadro.prop("hidden", false);
            fsAuth.prop("hidden", true);
            fsAuth.removeClass(`flex-container`);
            fsReport.prop("hidden", true);
            menuTab.prop("hidden", true);
            tabComparacion.prop("hidden", true);
        }
        function setPanel(lst) {
            fsAuth.html();
            let div = `<legend class="legend-custm"><i class="fa fa-user"></i> Autorizantes</legend>
                    <div class="flex-container text-center">`;
            $.each(lst, (idx, x) => {
                div += `<div class="panel ${x.auth == "U" ? "panel-primary" : x.auth == "E" ? "panel-default" : x.auth == "A" ? "panel-success" : "panel-danger"} flex-item">
                   <div class="panel-heading">
                       ${x.orden == 0 ? "Autorizante" : "Vobo " + x.orden} ${x.auth == "A" ? "Autorizó" : x.auth == "E" ? "En espera" : x.auth == "R" ? "Rechazado" : "<button class='btn btn-danger btn-xs pull-left auth' value='3'><i class='fas fa-times'></i> </button><button class='btn btn-success btn-xs pull-right auth' value='2'><i class='fa fa-check'></i></button>"}
                        </div>
                     <div class="panel-body"><label>${x.nombre}</label></div>
                    </div>`;
                if (x.auth == "U")
                    idAuth = x.id;
            });
            div += `</div>`;
            fsAuth.html(div);
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function initForm() {
            initCuadro();
            fsCuadro.prop("hidden", false);
            fsAuth.prop("hidden", true);
            fsAuth.removeClass(`flex-container`);
            fsReport.prop("hidden", true);
            menuTab.prop("hidden", true);
            tabComparacion.prop("hidden", true);
            getLstGestion();
        }
        init();
    }
    $(document).ready(function () {
        administrativo.facultamiento.Autorizacion = new Autorizacion();
    })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();