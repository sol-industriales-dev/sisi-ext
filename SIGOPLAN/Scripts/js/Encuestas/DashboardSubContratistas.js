(function () {

    $.namespace('EncuestaSubContratista.Dashboard');

    Dashboard = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        },

            _GraficaGlobal = null,
            cboSubContratistas = $("#cboSubContratistas"),
            cboSubContratistasEstrellas = $("#cboSubContratistasEstrellas"),
            ireport = $("#report"),
            LineWithLine1 = $("#LineWithLine1"),
            LineWithLineEstrellas = $("#LineWithLineEstrellas"),
            txtFechaInicioGrafica = $("#txtFechaInicioGrafica"),
            txtFechaFinGrafica = $("#txtFechaFinGrafica"),
            btnBuscarGrafica = $("#btnBuscarGrafica"),

            txtFechaInicioGraficaEstrellas = $("#txtFechaInicioGraficaEstrellas"),
            txtFechaFinGraficaEstrellas = $("#txtFechaFinGraficaEstrellas"),
            btnBuscarGraficaEstrellas = $("#btnBuscarGraficaEstrellas"),

            tabResponder = $("#tabResponder"),
            tabGeneral = $("#tabGeneral"),
            aTabGeneral = $("#aTabGeneral"),
            aTabResponder = $("#aTabResponder"),
            _encuestaID = 0,
            _convenio = 0,
            cboTipoEncuesta = $("#cboTipoEncuesta"),
            cboEncuestas = $("#cboEncuestas"),
            txtFechaInicio = $("#txtFechaInicio"),
            txtFechaFin = $("#txtFechaFin"),
            tblContinuaProveedor = $("#tblContinuaProveedor"),
            btnExportar = $("#btnExportar"),
            btnExportarEstrella = $('#btnExportarEstrella'),
            btnBuscar = $("#btnBuscar"),
            Preguntas = $(".Preguntas"),

            /*Respuesta de evaluaciones*/
            btnBuscarRespuesta = $("#btnBuscarRespuesta"),
            cboTipoEncuestaRespuesta = $("#cboTipoEncuestaRespuesta"),
            cboEncuestasRespuesta = $("#cboEncuestasRespuesta"),
            tbSubContratistaFiltroResponder = $("#tbSubContratistaFiltroResponder"),
            tbDatosSubContratista = $("#tbDatosSubContratista"),
            tbSubContratistaFiltroRespuesta = $("#tbSubContratistaFiltroRespuesta"),
            cboCC = $("#cboCC"),
            cboCCEstrellas = $("#cboCCEstrellas"),
            btnExportarGrafica = $("#btnExportarGrafica"),
            btnExportarGraficaEstrella = $("#btnExportarGraficaEstrella"),
            myChart1 = null;

        const btnExcel = $("#btnExcel");

        function init() {
            btnExcel.attr("disabled", "true");
            btnExcel.click(function (e) {
                //$.blockUI({ message: "Preparando archivo a descargar" });
                try {
                    location.href =
                        '/encuestas/EncuestasSubContratistas/getExcel?fechaInicio=' +
                        moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true) + '&fechaFin=' + moment(txtFechaFin.val(), 'DD/MM/YYYY').toISOString(true);
                } catch (error) {
                }
                //$.unblockUI();
            });

            cboSubContratistas.fillCombo('/encuestas/EncuestasSubContratistas/SetSubContratistas', null, false, "--Seleccione--");

            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");

            cboCCEstrellas.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCCEstrellas");

            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());

            txtFechaInicioGrafica.datepicker().datepicker("setDate", new Date());
            txtFechaFinGrafica.datepicker().datepicker("setDate", new Date());
            txtFechaInicioGraficaEstrellas.datepicker().datepicker("setDate", new Date());
            txtFechaFinGraficaEstrellas.datepicker().datepicker("setDate", new Date());

            cboEncuestas.fillCombo('/EncuestasSubContratistas/cboEncuestas', { tipoEncuesta: cboTipoEncuesta.val() }, false);

            btnBuscar.click(fnBuscar);
            tblContinuaProveedorGrid = $("#tblContinuaProveedor").DataTable({});
            btnBuscarRespuesta.click(fnBuscarResponder);
            btnBuscarGrafica.click(fnGetGraficas);
            btnBuscarGraficaEstrellas.click(fnGetGraficasEstrellas);
            btnExportar.click(Exportar);
            btnExportarEstrella.click(ExportarEstrellas);
            btnExportarGrafica.click(saveCanvas1);
            btnExportarGraficaEstrella.click(saveCanvasEstrellas)

            cboSubContratistas.change(LoadGraficaSubContratistaCC);
            cboSubContratistasEstrellas.change(LoadGraficaSubContratistaCC);
        }

        $('a[data-toggle="tab"]').on("shown.bs.tab", function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function LoadGraficaSubContratistaCC() {
            if ($(this).val() != "Todos") {
                url = '/encuestas/EncuestasSubContratistas/GetGraficaSubContratistaCC';
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: { fechaInicio: txtFechaInicioGrafica.val(), fechaFin: txtFechaFinGrafica.val(), cc: getValoresMultiples("#cboCC"), subContratista: cboSubContratistas.val() },
                    success: function (response) {

                        var Data = response.dtSubCC;
                        setGraficaSubContratista(Data);
                    },
                    error: function (response) {
                        alert(response.message);
                    }
                });

            }
            else {
                setGrafica(_GraficaGlobal);
            }
        }

        function LoadGraficaSubContratistaCCEstrellas() {
            if ($(this).val() != "Todos") {
                url = '/encuestas/EncuestasSubContratistas/GetGraficaSubContratistaCCEstrellas';
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: { fechaInicio: txtFechaInicioGraficaEstrellas.val(), fechaFin: txtFechaFinGraficaEstrellas.val(), cc: getValoresMultiples("#cboCCEstrellas"), subContratista: cboSubContratistasEstrellas.val() },
                    success: function (response) {

                        var Data = response.dtSubCC;
                        setGraficaSubContratistaEstrellas(Data);
                    },
                    error: function (response) {
                        alert(response.message);
                    }
                });

            }
            else {
                setGrafica(_GraficaGlobal);
            }
        }

        function setGraficaSubContratista(dt) {
            if (myChart1 != null) {
                myChart1.destroy();
            }
            var dtLine = new Array();

            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < dt.length; i++) {
                dtLabel.push(dt[i].CC);
                dtLine.push(dt[i].value);
            }

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: $("#cboSubContratistas option:selected").text(),
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtLine,
                    fill: false
                }]
            };

            var ctx = document.getElementById("LineWithLine1").getContext("2d");

            myChart1 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;


                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                if (dataset.type != 'line') {
                                    meta.data.forEach(function (bar, index) {

                                        var dato = Number(dataset.data[index]);
                                        data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                    });
                                }

                            });

                        }
                    }
                }
            });

        }

        function setGraficaSubContratistaEstrellas(dt) {
            if (myChart1 != null) {
                myChart1.destroy();
            }
            var dtLine = new Array();

            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < dt.length; i++) {
                dtLabel.push(dt[i].CC);
                dtLine.push(dt[i].value);
            }

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: $("#cboSubContratistasEstrellas option:selected").text(),
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtLine,
                    fill: false
                }]
            };

            var ctx = document.getElementById("LineWithLineEstrellas").getContext("2d");

            myChart1 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;


                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                if (dataset.type != 'line') {
                                    meta.data.forEach(function (bar, index) {

                                        var dato = Number(dataset.data[index]);
                                        data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                    });
                                }

                            });

                        }
                    }
                }
            });

        }

        function GetColor() {
            var posarray;
            hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
            color_aleatorio = "#";
            for (i = 0; i < 6; i++) {
                posarray = aleatorio(0, hexadecimal.length)
                color_aleatorio += hexadecimal[posarray]
            }
        }

        function Exportar() {
            var blob = new Blob([document.getElementById("DivTable").innerHTML], {
                type: "text/plain;charset=utf-8;"
            });
            saveAs(blob, "Report.xls");

        }

        function ExportarEstrellas() {
            var blob = new Blob([document.getElementById("DivTableEstrellas").innerHTML], {
                type: "text/plain;charset=utf-8;"
            });
            saveAs(blob, "Report.xls");

        }

        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            iframe = document.getElementById('iframeDownload');
            iframe.src = url;

            var timer = setInterval(function () {

                var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                // Check if loading is complete
                if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                    setTimeout(function () {
                        $.unblockUI();
                    }, 5000);

                    clearInterval(timer);
                    return;
                }
            }, 1000);
        }

        function saveCanvas1() {
            var canvas = $('#LineWithLine1').get(0);
            canvas.toBlob(function (blob) {
                saveAs(blob, "EvaluacionSubContratistas.png");
            });

        }

        function saveCanvasEstrellas() {
            var canvas = $('#LineWithLineEstrellas').get(0);
            canvas.toBlob(function (blob) {
                saveAs(blob, "EvaluacionSubContratistas.png");
            });

        }

        function aleatorio(inferior, superior) {
            numPosibilidades = superior - inferior
            aleat = Math.random() * numPosibilidades
            aleat = Math.floor(aleat)
            return parseInt(inferior) + aleat
        }

        function setGrafica(dt) {
            if (myChart1 != null) {
                myChart1.destroy();
            }
            var dtLine = new Array();

            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < dt.length; i++) {
                dtLabel.push(dt[i].nombreContratista);
                dtLine.push(dt[i].value);
            }

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Calificacion",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtLine,
                    fill: false
                }]
            };

            var ctx = document.getElementById("LineWithLine1").getContext("2d");

            myChart1 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;


                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                if (dataset.type != 'line') {
                                    meta.data.forEach(function (bar, index) {

                                        var dato = Number(dataset.data[index]);
                                        data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                    });
                                }

                            });

                        }
                    }
                }
            });

        }

        function setGraficaEstrellas(dt) {
            if (myChart1 != null) {
                myChart1.destroy();
            }
            var dtLine = new Array();

            var dtLabel = new Array();
            var dtItemArreglo = new Array();
            for (var i = 0; i < dt.length; i++) {
                dtLabel.push(dt[i].nombreContratista);
                dtLine.push(dt[i].value);
            }

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Calificacion",
                    backgroundColor: "rgba(255, 159, 64, 0.5)",
                    borderColor: "rgb(255, 159, 64)",
                    borderWidth: 1,
                    data: dtLine,
                    fill: false
                }]
            };

            var ctx = document.getElementById("LineWithLineEstrellas").getContext("2d");

            myChart1 = new Chart(ctx, {
                type: 'line',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;


                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                if (dataset.type != 'line') {
                                    meta.data.forEach(function (bar, index) {

                                        var dato = Number(dataset.data[index]);
                                        data = dato.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                    });
                                }

                            });

                        }
                    }
                }
            });

        }

        var backgroundColor = 'white';
        Chart.plugins.register({
            beforeDraw: function (c) {
                var ctx = c.chart.ctx;
                ctx.fillStyle = backgroundColor;
                ctx.fillRect(0, 0, c.chart.width, c.chart.height);
            }
        });

        function SelectProveedor(event, ui) {
            tbSubContratistaFiltroRespuesta.text(ui.item.value);
            tbSubContratistaFiltroRespuesta.attr('data-NumProveedor', ui.item.id)
        }

        function fnGetGraficas() {
            url = '/encuestas/EncuestasSubContratistas/EstadisticasEvaluacionSubContrtaos';
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { fechaInicio: txtFechaInicioGrafica.val(), fechaFin: txtFechaFinGrafica.val(), cc: getValoresMultiples("#cboCC") },
                success: function (response) {

                    _GraficaGlobal = response.dtset;

                    $("#example").children().remove();
                    buildHtmlTable($("#example"), JSON.parse(response.dtSetGrafica), response.listaCondicionados, response.listTotales, response.listaReclasificacion, false);
                    setGrafica(response.dtset);
                    btnExportar.removeClass('hide');
                    cboSubContratistas.fillCombo('/encuestas/EncuestasSubContratistas/SetSubContratistas', null, false, "Todos");


                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        function fnGetGraficasEstrellas() {
            url = '/encuestas/EncuestasSubContratistas/EstadisticasEvaluacionSubContratosEstrellas';
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { fechaInicio: txtFechaInicioGraficaEstrellas.val(), fechaFin: txtFechaFinGraficaEstrellas.val(), cc: getValoresMultiples("#cboCCEstrellas") },
                success: function (response) {

                    _GraficaGlobal = response.dtset;

                    $("#graficaEstrellas").children().remove();
                    buildHtmlTable($("#graficaEstrellas"), JSON.parse(response.dtSetGrafica), response.listaCondicionados, response.listTotales, response.listaReclasificacion, true);
                    setGraficaEstrellas(response.dtset);
                    btnExportarEstrella.removeClass('hide');
                    cboSubContratistasEstrellas.fillCombo('/encuestas/EncuestasSubContratistas/SetSubContratistas', null, false, "Todos");
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        function buildHtmlTable(selector, myList, listaCondicionados, Totales, ReClasificacion, isEstrellas) {


            var columns = addAllColumnHeaders(myList, selector, listaCondicionados);

            $(selector).append($('<tbody/>'));
            for (var i = 0; i < myList.length; i++) {
                var row$ = $('<tr/>');
                for (var colIndex = 0; colIndex < columns.length; colIndex++) {
                    var cellValue = myList[i][columns[colIndex]];
                    if (cellValue == null) cellValue = "";
                    if (colIndex != 1)
                        row$.append($('<td class="text-center"/>').html(cellValue));
                    else
                        row$.append($('<td/>').html(cellValue));
                }
                $(selector).append(row$);
            }

            for (var i = 0; i < 1; i++) {
                var row$ = $('<tr/>');
                for (var colIndex = 0; colIndex < columns.length; colIndex++) {
                    var cellValue = Totales[colIndex];
                    if (cellValue == null) cellValue = "";
                    if (colIndex == 1) {
                        row$.append($('<td class="text-center" colspan="2"/>').html("REEVALUACIÓN ANUAL"));
                    } else if (colIndex > 1) {
                        row$.append($('<td class="text-center"/>').html(cellValue));
                    }

                }
                $(selector).append(row$);
            }
            for (var i = 0; i < 1; i++) {
                var row$ = $('<tr style="background-color: cornflowerblue;"/>');
                for (var colIndex = 0; colIndex < columns.length; colIndex++) {
                    var cellValue = ReClasificacion[colIndex];
                    if (cellValue == null) cellValue = "";
                    if (colIndex == 1) {
                        if (isEstrellas) {
                            row$.append($('<td class="text-center" colspan="2" style="COLOR: WHITE;"/>').html("RECLASIFICACIONES (PÉSIMO, MALO, REGULAR, ACEPTABLE, ESTUPENDO)	"));

                        } else {
                            row$.append($('<td class="text-center" colspan="2" style="COLOR: WHITE;"/>').html("RECLASIFICACIONES (CONDICIONADO, SATISFACTORIO, PREFERIDO)	"));
                        }
                    } else if (colIndex > 1) {
                        row$.append($('<td class="text-center" style="COLOR: WHITE;"/>').html(cellValue));
                    }
                }
                $(selector).append(row$);
            }
        }

        function addAllColumnHeaders(myList, selector, listaCondicionados) {
            var columnSet = [];

            var headerTr$ = $('<thead class="bg-table-header"/>');
            headerTr$.append($('<tr />'));
            headerTr$.append($('<tr style="background-color: cornflowerblue;"/>'));


            for (var i = 0; i < myList.length; i++) {
                var rowHash = myList[i];
                for (var key in rowHash) {
                    if ($.inArray(key, columnSet) == -1) {
                        columnSet.push(key);
                        headerTr$.children('tr').first().append($('<th class="text-center"/>').html(key));
                    }

                    if (i == 0) {
                        if (columnSet.length > 1) {
                            headerTr$.children('tr').last().append($('<th class="text-center" />').html(listaCondicionados[columnSet.length - 1]));
                        }
                        else {
                            headerTr$.children('tr').last().append($('<th class="text-center" />').html(""));
                        }
                    }
                }

            }
            $(selector).append(headerTr$);

            return columnSet;
        }

        function fnBuscarResponder() {

            url = '/encuestas/EncuestasSubContratistas/LoadSubcontratistaEvaluacion';
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { contratistaID: tbSubContratistaFiltroRespuesta.attr('data-NumProveedor') },
                success: function (response) {
                    loadDataTablaDashboardEvaluacion(response.dataDashboard);
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }

        function fnBuscar() {
            if (cboEncuestas.val() > 0) {
                url = '/Encuestas/EncuestasSubContratistas/LoadEncuestasSubContratistas';
                $.blockUI({ message: 'Cargando encuestas...' });
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: { FechaInicio: txtFechaInicio.val(), FechaFin: txtFechaFin.val(), encuestaID: cboEncuestas.val() },
                    success: function (response) {
                        if (response) {
                            btnExcel.removeAttr("disabled");
                            loadDataTablaDashboard(response.dataDashboard)
                            if (response.dataDashboard != "") {
                                btnExcel.removeAttr("disabled");
                            } else {
                                btnExcel.attr("disabled");
                            }
                        }
                    },
                    error: function (response) {
                        alert(response.message);
                    }
                }).always($.unblockUI);
            } else {
                ////OMAROMAR
            }
        }

        //Se encarga de cargar los datos de dashboard
        function loadDataTablaDashboard(dataSet) {
            tbResEncuestasGrid = $("#tbResEncuestas").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: true,
                "bFilter": true,
                "ordering": true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                //dom: 'Bfrtip',
                /*buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7]
                        }
                    },
                ],*/
                columns: [
                    {
                        data: "centroCostos", "width": "3%",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');
                        }
                    },
                    {
                        data: "fechaInicio", "width": "13%",
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        },
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');
                        }
                    },
                    {
                        data: "fechaFin", "width": "13%",
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        },
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');
                        }
                    },
                    {
                        data: "nombreProyecto", "width": "300px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');
                        }
                    },
                    { data: "nombreSubContratista", "width": "200px" },
                    { data: "evaluador", visible: false },
                    { data: "fechaEvaluacion", visible: false },
                    { data: "calificacion", visible: false },
                    {
                        data: "estatus", "width": "50px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html('');
                            if (rowData.estatus) {
                                $(td).append('<button type="button" class="btn btn-success btn-block btn-sm" onclick="ResponderEncuesta(\'' +
                                    rowData.centroCostos + '\',' +
                                    rowData.numProveedor + ',' +
                                    rowData.convenio +
                                    ')" >Responder</button>');

                                if (rowData.id[0] != 0) {
                                    $.each(rowData.id, function (i, e) {
                                        $(td).append('<button type="button" class="btn btn-primary btn-block btn-sm" onclick="verReporte(' +
                                            rowData.id[i] + ')" >Ver encuesta ' + (i + 1) +
                                            '</button>');
                                    });
                                }
                            }
                            else {
                                if (rowData.id.length > 1) {
                                    let impresion = '' +
                                        '<div class="btn-group">' +
                                        '<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                                        'Imprimir <span class="caret"></span>' +
                                        '</button>' +
                                        '<ul class="dropdown-menu">';
                                    $.each(rowData.id, function (i, e) {
                                        impresion += '<li>' +
                                            '<a href="#" onclick="verReporte(' + rowData.id[i] + ')">Ver encuesta ' + (i + 1) + '</a>' +
                                            '</li>';
                                    });
                                    impresion += '</ul></div>';
                                    $(td).append(impresion);
                                }
                                else {
                                    $(td).append('<button type="button" class="btn btn-primary btn-block btn-sm" onclick="verReporte(' + rowData.id[0] + ')" >Imprimir</button>');
                                }
                            }
                        }
                    }
                ],
                "paging": true,
                "info": false,
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
            });
        }

        function loadDataTablaDashboardEvaluacion(dataSet) {
            tbDatosSubContratistaGrid = $("#tbDatosSubContratista").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                responsive: true,
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        data: "centroCostosNombre"
                    },
                    {
                        data: "nombreSubContratista"
                    },
                    {
                        data: "nombreProyecto"
                    },
                    {
                        data: "Comentarios"
                    },
                    {
                        data: "btn",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.estatus) {
                                $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="ResponderEncuesta(' + rowData.centroCostos + ',' + rowData.numProveedor + ')" >Responder</button>');

                            }
                        }
                    }
                ],
                "paging": true,
                "info": false

            });
        }
        init();
    };

    $(document).ready(function () {

        EncuestaSubContratista.Dashboard = new Dashboard();
    });

})();



function ResponderEncuesta(cc, numContrato, convenio) {

    if ($("#cboEncuestas").val() != "") {
        encuestaID = $("#cboEncuestas").val();

        $("#divRespuestas").removeClass('hide');
        $("#divBusquedaSubContratistas").addClass('hide');
        cargarEncuesta(cc, numContrato, convenio)
    }
    else {
        AlertaGeneral("Alerta", "Se debe seleccionar una encuesta antes de poder continuar.");
    }
}

function cargarEncuesta(centrocostos, numContrato, convenio) {
    _convenio = 0;
    $('.Preguntas:eq(1)').empty();

    var id = $("#cboEncuestas").val();
    if (id != null) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Encuestas/EncuestasSubContratistas/CargarEncuestaResponder",
            data: { idEncuesta: id, numContrato: numContrato, CC: centrocostos, convenio: convenio },
            asyn: false,
            success: function (response) {
                var obj = response.obj;
                var blob = $.urlParam('encuesta');

                if (response.RespuestaEncuesta) {
                    ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                    setInterval(function () {
                        AbandonarSession(this);
                    }, 2000);
                }
                else {

                    aTabResponder.trigger('click');

                    moment.locale();
                    var proveedoresData = response.getDatosSubcontratista;
                    _encuestaID = response.id;
                    _convenio = response.convenioID;

                    var FechaActual = new Date();
                    $("#tbFechaEvaluacion").val(moment(FechaActual).format('DD/MM/YYYY'));
                    $("#tbNombreSubContratista").val(proveedoresData.nombreSubContratista);
                    $("#tbNombreSubContratista").attr('data-numProveedor', proveedoresData.numProveedor);
                    $("#tbNombreSubContratista").attr('data-centroCostos', proveedoresData.centroCostos);
                    $("#tbNombreSubContratista").attr('data-centroCostosNombre', proveedoresData.centroCostosNombre);
                    $("#tbEvaluador").val(response.evaluador);
                    $("#txtTitulo").val(response.titulo);
                    $("#txtDescripcion").val(response.descripcion);
                    $("#txtAsunto").val(response.tipoEncuesta);
                    $("#tbServicioContratado").val(proveedoresData.nombreProyecto);
                    $("#tbNombreProyecto").val(response.CentroCostos);

                    $.ajax({
                        url: "/Encuestas/Encuesta/getEstrellas",
                        asyn: false,
                        success: function (respuestaEstrellas) {
                            var estrellas = respuestaEstrellas.data;

                            $.each(response.preguntas, function (i, e) {
                                var pregunta = fnAddPregunta(e.pregunta, e.id);
                                $(pregunta).appendTo(Preguntas[1]);
                                $('.starrr').starrr({
                                    rating: 0,
                                    change: function (e, value) {
                                        var id = $(e.currentTarget).data("id");
                                        $(e.currentTarget).attr("data-calificacion", value);
                                        if (value <= 5) {
                                            $('[data-id="txtRespuesta' + id + '"]').show();
                                        }

                                        let etiqueta = $(e.currentTarget).find('label');
                                        if (value > 0) {
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == value) {
                                                    etiqueta.text(est.descripcion);
                                                }
                                            });
                                        } else {
                                            etiqueta.text('');
                                        }
                                    }
                                });

                                var etiqueta = document.createElement('label');
                                etiqueta.style.marginLeft = '10px';
                                $('.starrr:last').append(etiqueta);
                            });
                        }
                    });
                }
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
}
function fnAddPregunta(text, id) {
    var html = '<div class="row Pregunta">';
    html += '    <div class="col-lg-12">';
    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <div class="input-group">';
    html += '                    <span class="input-group-addon">Pregunta:</span>';
    html += '                    <div style="border:1px dotted gray;height: 32px;">';
    html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0"></div>';
    html += '                    </div>';
    html += '                </div>';
    html += '            </div>';
    html += '        </div>';
    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
    html += '                <textarea class="form-control txtPregunta" placeholder="Explica tu respuesta" data-respuesta="" data-id="txtRespuesta' + id + '"></textarea>';
    html += '            </div>';
    html += '        </div>';
    html += '    </div>';
    html += '</div>';
    return html;
}

function verReporte(pREncuestaID) {
    $.blockUI({ message: mensajes.PROCESANDO });

    var path = "/Reportes/Vista.aspx?idReporte=" + 76 + "&rEncuestaID=" + pREncuestaID;
    $("#report").attr("src", path);
    document.getElementById('report').onload = function () {
        $.unblockUI();
        openCRModal();
    };

}

function fnEnviar() {
    var preguntas = $(".starrr");
    var preguntasList = new Array();
    var validacion = false;
    $.each(preguntas, function (i, e) {
        var o = {};
        o.encuestaID = _encuestaID;
        o.preguntaID = $(e).attr('data-id');
        o.calificacion = $(e).attr('data-calificacion');
        o.respuesta = $('[data-id="txtRespuesta' + o.preguntaID + '"]').val();
        preguntasList.push(o);
    });


    //if ($("#tbNoContrato").val() == "") {
    //    validacion = true;
    //    ConfirmacionGeneral("Confirmación", "¡Se debe agregar el número de contrato para poder continuar.");
    //}

    if (validacion == false) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Encuestas/EncuestasSubContratistas/saveEncuestaResult",
            data: { obj: preguntasList, objSingle: getObjDetalle(), encuestaID: _encuestaID, comentario: $("#txtComentarioSub").val().trim() },
            asyn: false,
            success: function (response) {
                $('.nav-tabs a[href="#tabGeneral"]').tab('show');
                $("#divRespuestas").addClass('hide');

                btnBuscar.click();
                ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                // setInterval(function () {
                //     var blob = $.urlParam('blob');
                //     if (blob == null) {
                //         window.location.href = "/Encuestas/EncuestasSubContratistas/Dashboard";
                //     }
                //     else {
                //         window.location.href = "/Encuestas/EncuestasSubContratistas/Dashboard";
                //     }

                // }, 2000);
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
}

function getObjDetalle() {
    return {
        numSubContratista: $("#tbNombreSubContratista").attr('data-numproveedor'),
        nombreSubContratista: $("#tbNombreSubContratista").val(),
        noContrato: $("#tbNoContrato").val(),
        descripcionServicio: $("#tbServicioContratado").val(),
        nombreProyecto: $("#tbNombreProyecto").val(),
        evaluador: $("#tbEvaluador").attr('data-id'),
        centroCostos: $("#tbNombreSubContratista").attr('data-centrocostos'),
        comentarios: $("#txtComentarioSub").val().trim(),
        estadoEncuesta: true,
        encuestaID: _encuestaID,
        convenioID: _convenio
    };
}