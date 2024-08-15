/**
 * @fileoverview Genera gráficos consultando las Salidas de almacen del año en turno
 * @version v1.0
 * @author Aaron Romero <aaron.romero@construplan.com.mx>
 * @copyright Construplan S.A de C.V.
 * History
 * v1.0 – Primer versión 20/11/2018
 * ----
 * La primera versión de dashboard de salidas de almacen fue escrita por Aaron Romero */
(() => {
    $.namespace('Enkontrol.Almacen.Valuacion.Salidas');
    Salidas = function () {
        chAlmacen = document.getElementById("chAlmacen").getContext('2d');
        chPeriodo = document.getElementById("chPeriodo").getContext('2d');
        const divPeriodo = $('#divPeriodo');
        const divAlmacen = $('#divAlmacen');
        const chkPeriodo = $('#chkPeriodo');
        const chkAlmacen = $('#chkAlmacen');
        const btnExportar = $('#btnExportar');
        const lblFecha = $('#lblFecha');
        const datePickerFecha = $('#datePickerFecha');
        gAlmacen = undefined;
        gPeriodo = undefined;
        let init = () => {
            btnExportar.click(getExcel);
            chkPeriodo.change(esTodoPeriodo);
            chkAlmacen.change(esTodoAlamcen);
            divPeriodo.change(setValores);
            divAlmacen.change(setValores);
            // datePickerFecha.change(setValores);
            datePickerFecha.change(function () {
                getPeriodos().done(response => {
                    setChkBox(divPeriodo, response.lstPeridos);
                    selTodoPeriodo();
                    getAlmacenesSalida().done(response => {
                        setChkBox(divAlmacen, response.lstAlmacenes);
                        selTodoAlmacen();
                        setValores();
                    });
                });
            });
            initForm();
        }
        /**
         * Consulta de valuación
         */
        const getValuacion = () => $.post('/Enkontrol/Valuacion/getValuacionSalida', {
            almacen: getChkData(divAlmacen).map(f => f.almacen),
            periodo: getChkData(divPeriodo).map(f => f.periodo),
            fecha: datePickerFecha.val()
        });
        /**
         * Consulta de Alamcenes
         */
        const getAlmacenesSalida = () => $.post('/Enkontrol/Valuacion/getAlmacenesSalida', { lstFront: getChkData(divAlmacen), fecha: datePickerFecha.val() });
        /**
         * Consulta de periodos
         */
        const getPeriodos = () => $.post('/Enkontrol/Valuacion/getPeriodos', { lstFront: getChkData(divPeriodo), fecha: datePickerFecha.val() });
        /**
         * Manda filtros para exportar
         * @param {data} almacen alamcenes
         * @param {data} periodo periodos
         */
        const getBusqSalidaExpors = (almacen, periodo) => $.post('/Enkontrol/Valuacion/getBusqSalidaExpors', {
            almacen: almacen,
            periodo: periodo
        });
        /**
         * Evento botón de excel
         */
        function getExcel() {
            getBusqSalidaExpors(
                getChkData(divAlmacen).map(f => f.almacen),
                getChkData(divPeriodo).map(f => f.periodo)
            ).done(response => {
                if (response.success)
                    download(`/Enkontrol/Valuacion/setSalidaExport`);
            });
        }
        /**
          *Petición de excel 
          * @param {post} url post para pedir archivo
          * @returns Excel que desgloza el importe por insumo
         */
        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            var link = document.createElement("button");
            link.download = url;
            link.href = url;
            $(link).unbind("click");
            location.href = url;
            $.unblockUI();
        }
        /**
         * Inicializa elementos
         */
        function initForm() {
            let hoy = new Date().toLocaleDateString();
            lblFecha.text(`Fecha ${hoy}`);
            chkAlmacen.prop("checked", true);
            chkPeriodo.prop("checked", true);

            let fechaHoy = new Date();

            datePickerFecha.datepicker().datepicker("setDate", new Date(fechaHoy.getFullYear(), fechaHoy.getMonth(), fechaHoy.getDate()));

            getPeriodos()
                .done(response => {
                    setChkBox(divPeriodo, response.lstPeridos);
                    selTodoPeriodo();
                    getAlmacenesSalida().done(response => {
                        setChkBox(divAlmacen, response.lstAlmacenes);
                        selTodoAlmacen();
                        setValores();
                    });
                });
        }
        /**
         * Evento change() de Almacenes
         */
        function esTodoAlamcen() {
            let esTodo = $(this).is(":checked");
            divAlmacen.find('input').each(function () {
                this.checked = esTodo;
            });
            setValores();
        }
        /**
         * Evento change() de Periodos
         */
        function esTodoPeriodo() {
            let esTodo = $(this).is(":checked");
            divPeriodo.find('input').each(function () {
                this.checked = esTodo;
            });
            setValores();
        }
        /**
         * Obtiene la data selecionada del contenedor
         * @param {element} div contenedor
         * @returns  Data del contenedor
         */
        function getChkData(div) {
            let selected = [];
            div.find('input:checked').each(function () {
                selected.push($(this).data());
            });
            return selected;
        }
        /**
         * Selecciona los checkbox de Almancen
         */
        function selTodoAlmacen() {
            divAlmacen.find('input').each(function () {
                this.checked = true;
            });
        }
        /**
         * Selecciona los checkbox de Periodos
         */
        function selTodoPeriodo() {
            divPeriodo.find('input').each(function () {
                this.checked = true;
            });
        }
        /**
         * Geneara contenedor de checkbox
         * @param {element} div Contenedor
         * @param {[]} lst checkbox
         */
        function setChkBox(div, lst) {
            div.empty();
            if (lst != null || lst.length > 0)
                for (let i in lst)
                    if (lst.hasOwnProperty(i)) {
                        let chk = $(`<label>`).addClass("chBox"),
                            inp = $(`<input>`)
                        sp1 = $(`<span>`)
                        sp2 = $(`<span>`)
                        inp.val(lst[i].Value).data({
                            almacen: lst[i].almacen,
                            periodo: lst[i].periodo,
                        });
                        inp.get(0).type = "checkbox";
                        sp1.addClass(`y`).text(lst[i].Text);
                        sp2.addClass(`n`).text(lst[i].Text);
                        div.append(chk.append(inp, sp1, sp2));
                    }
        }
        /**
         * Graficador de valuaciones
         */
        function setValores() {
            getValuacion().done(response => {
                initAlmacen(response.lstAlmacenes);
                initPeriodo(response.lstPeriodo);
            });
        }
        /**
         * Configuración del chart de alamcen
         * @param {data} almacen alamcen
         */
        function initAlmacen(compania) {
            if (gAlmacen != undefined)
                gAlmacen.destroy();
            let gradient = chAlmacen.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(250,174,50,2)');
            gradient.addColorStop(1, 'rgba(250,174,90,0)');
            gAlmacen = new Chart(chAlmacen, {
                type: 'bar',
                data: {
                    labels: compania.map(a => { return a.label; }),
                    datasets: [{
                        label: compania.map(a => { return a.label; }),
                        backgroundColor: gradient,
                        data: compania.map(a => { return a.total; })
                    }]
                },
                options: options
            });
        }
        /**
         * Configuración del chart de alamcen
         * @param {data} compania alamcen
         */
        function initPeriodo(compania) {
            if (gPeriodo != undefined)
                gPeriodo.destroy();
            let gradient = chAlmacen.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(250,174,50,2)');
            gradient.addColorStop(1, 'rgba(250,174,90,0)');
            gPeriodo = new Chart(chPeriodo, {
                type: 'bar',
                data: {
                    labels: compania.map(a => { return a.label; }),
                    datasets: [{
                        label: compania.map(a => { return a.label; }),
                        backgroundColor: gradient,
                        data: compania.map(a => { return a.total; })
                    }]
                },
                options: options
            });
        }
        options = {
            animation: {
                duration: 1,
                onComplete: function (animation) {
                    var myLiveChart = this;
                    var sorceChart = myLiveChart.chart;
                    var sourceCtx = sorceChart.ctx;
                    sourceCtx.font = Chart.helpers.fontString(14, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                    sourceCtx.textAlign = 'center';
                    sourceCtx.textBaseline = 'bottom';
                    sourceCtx.imageSmoothingQuality = 'high';
                    myLiveChart.data.datasets.forEach(function (dataset, i) {
                        var meta = sorceChart.controller.getDatasetMeta(i);
                        meta.data.forEach((bar, index) => {
                            var data = dataset.data[index];
                            sourceCtx.fillText(data.toFixed(2) + '%', bar._model.x, bar._model.y);
                        });
                    });
                }
            },
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            title: {
                display: false,
            },
            tooltips: {
                enable: true,
                callbacks: {
                    label: function (t, d) {
                        return (d.datasets[t.datasetIndex].data[t.index]).toFixed(2) + '%';
                    }
                }
            },
            scales: {
                yAxes: [{
                    display: true,
                    gridLines: {
                        display: true
                    },
                    ticks: {
                        display: true,
                        beginAtZero: true,
                        fontColor: "black",
                        max: 120,
                    },
                }],
                xAxes: [{
                    barPercentage: 1,
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        beginAtZero: true,
                        maxRotation: 85,
                        minRotation: 80,
                    }
                }]
            }
        }
        init();
    }
    $(document).ready(() => {
        Enkontrol.Almacen.Valuacion.Salidas = new Salidas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();