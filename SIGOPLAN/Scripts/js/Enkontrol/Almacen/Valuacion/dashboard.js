/**
 * @fileoverview Genera gráficos consultando las entradas de almacen del año en turno
 * @version v1.0
 * @author Aaron Romero <aaron.romero@construplan.com.mx>
 * @copyright Construplan S.A de C.V.
 * History
 * v1.0 – Primer versión 12/11/2018
 * ----
 * La primera versión de dashboard de entradas de almacen fue escrita por Aaron Romero */
(() => {
    $.namespace('Enkontrol.Almacen.Valuacion.Dashboard');
    Dashboard = function () {
        chAlmacen = document.getElementById("chAlmacen").getContext('2d');
        chGrupo = document.getElementById("chGrupo").getContext('2d');
        chCompania = document.getElementById("chCompania").getContext('2d');
        const lblFecha = $('#lblFecha');
        const lblTotal = $('#lblTotal');
        const divGrupo = $('#divGrupo');
        const chkGrupo = $('#chkGrupo');
        const chkAlmacen = $('#chkAlmacen');
        const chkCompania = $('#chkCompania');
        const divAlmacen = $('#divAlmacen');
        const divCompania = $('#divCompania');
        const btnExportar = $('#btnExportar');
        gAlmacen = undefined;
        gGrupo = undefined;
        gCompania = undefined;
        let init = () => {
            btnExportar.click(getExcel);
            chkGrupo.change(esTodoGrupo);
            chkAlmacen.change(esTodoAlamcen);
            chkCompania.change(esTodoCompania)
            divCompania.change(setAlmacenes);
            divAlmacen.change(setGrupos);
            divGrupo.change(setValores);
            initForm();
        }
        /**
         * Consulta los almacenes de entrada de las compañías
         * @returns Alamacenes
         */
        const getAlamcenes = () => $.post('/Enkontrol/Valuacion/getAlamcenes', { lstCompania: getChkValue(divCompania) });
        /**
         * Consulta los grupos de insumos de los almacences
         * @returns grupos
         */
        const getGruposInsumos = () => $.post('/Enkontrol/Valuacion/getGruposInsumos', { lstAlmacenes: getChkDTO(divAlmacen) });
        /**
         * Consulta los insumos de entrada
         * @returns Los grupos de importes
         */
        const getValuacion = () => $.post('/Enkontrol/Valuacion/getValuacion', { lstGrupo: getChkDTO(divGrupo) });
        /**
         * Envía los grupos seleccionados para filtrar los insumos
         * @param {Array} lstGrupo Grupos Seleccionado
         * @returns Excel con los importe de los insumos
         */
        const getBusqExpors = (lstGrupo) => $.post('/Enkontrol/Valuacion/getBusqExpors', { lstGrupo: lstGrupo });
        /**
          * Evento botón de excel
          */
        function getExcel() {
            getBusqExpors(getChkDTO(divGrupo))
                .done(response => {
                    if (response.success)
                        download(`/Enkontrol/Valuacion/setExport`);
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
         * Evento change() de Almacenes
         */
        function setAlmacenes() {
            getAlamcenes().done(response => {
                setChkBox(divAlmacen, response.lstAlmacen);
                selTodoAlmacen();
                setGrupos();
            }).catch(o_O => { });
        }
        /**
         * Evento change() de grupos
         */
        function setGrupos() {
            getGruposInsumos().done(response => {
                setChkBox(divGrupo, response.lstGrupo);
                selTodoGrupo();
                setValores();
            }).catch(o_O => { });
        }
        /**
         * Graficador de valuaciones
         */
        function setValores() {
            getValuacion().done(response => {
                initTotal(response.total);
                initAlmacen(response.almacen);
                initGrupo(response.grupo);
                initCompania(response.compania);
            });
        }
        /**
         * Geneara contenedor de checkbox
         * @param {element} div Contenedor
         * @param {[]} lst checkbox
         */
        function setChkBox(div, lst) {
            div.empty();
            if (lst.length > 0)
                for (let i in lst)
                    if (lst.hasOwnProperty(i)) {
                        let chk = $(`<label>`).addClass("chBox"),
                            inp = $(`<input>`)
                        sp1 = $(`<span>`)
                        sp2 = $(`<span>`)
                        inp.val(lst[i].Value).data({
                            almacen: lst[i].almacen,
                            grupo: lst[i].grupo,
                            tipo: lst[i].tipo
                        });
                        inp.get(0).type = "checkbox";
                        sp1.addClass(`y`).text(lst[i].Text);
                        sp2.addClass(`n`).text(lst[i].Text);
                        div.append(chk.append(inp, sp1, sp2));
                    }
        }
        /**
         * Iniciador de elementos
         */
        function initForm() {
            let hoy = new Date().toLocaleDateString();
            lblFecha.text(`Valuación a ${hoy}`);
            divCompania.find('input').each(function () {
                this.checked = true;
            });
            chkGrupo.prop("checked", true);
            chkAlmacen.prop("checked", true);
            chkCompania.prop("checked", true);
            getAlamcenes()
                .done(response => {
                    setChkBox(divAlmacen, response.lstAlmacen);
                    selTodoAlmacen();
                })
                .done(response => {
                    getGruposInsumos()
                        .done(response => {
                            setChkBox(divGrupo, response.lstGrupo);
                            selTodoGrupo();
                        }).done(response => {
                            setValores();
                        }).catch(o_O => { });
                });
        }
        /**
         * Selecciona/deseleciona los checkbox de Almancen
         */
        function esTodoAlamcen() {
            let esTodo = $(this).is(":checked");
            divAlmacen.find('input').each(function () {
                this.checked = esTodo;
            });
            setGrupos();
        }
        /**
         * Selecciona/deseleciona los checkbox de grupo
         */
        function esTodoGrupo() {
            let esTodo = $(this).is(":checked");
            divGrupo.find('input').each(function () {
                this.checked = esTodo;
            });
            setValores();
        }
        /**
         * Selecciona/deseleciona los checkbox de Compañía
         */
        function esTodoCompania() {
            let esTodo = $(this).is(":checked");
            divCompania.find('input').each(function () {
                this.checked = esTodo;
            });
            setAlmacenes();
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
         * Selecciona los checkbox de Grupo
         */
        function selTodoGrupo() {
            divGrupo.find('input').each(function () {
                this.checked = true;
            });
        }
        /**
         * Obtiene los valores selecionados del contenedor
         * @param {elemento} div Contenedor
         * @returns Valores del contenedor
         */
        function getChkValue(div) {
            let selected = [];
            div.find('input:checked').each(function () {
                selected.push(+(this.value));
            });
            return selected;
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
         * Obtiene la data seleccionada
         * @param {elemto} div contenedor
         * @returns Data de todos los contenedores
         */
        function getChkDTO(div) {
            let selected = [],
                id = div[0].id,
                compania = getChkValue(divCompania),
                almacen = getChkData(divAlmacen).map(f => f.almacen),
                grupo = getChkData(divGrupo).map(f => f.grupo);
            div.find('input:checked').each(function (e, i) {
                let data = $(this).data();
                selected.push({
                    almacen: data.almacen,
                    grupo: data.grupo,
                    tipo: data.tipo
                });
            });
            selected.filter(s => s.grupo.filter(g => compania.includes(g.compania)))
            selected.filter(s => s.grupo.length > 0);
            if (id == "divAlmacen") {
                selected.filter(s => almacen.includes(s.almacen));
                selected.filter(s => s.almacen.length > 0);
            }
            if (id == "divGrupo") {
                selected.filter(s => s.grupo.filter(g => grupo.includes(g.grupo)));
                selected.filter(s => s.grupo.length > 0);
            }
            return selected;
        }
        /**
         * Imprime el total en #lblTotal
         * @param {Number} total Total de compañia
         */
        function initTotal(total) {
            lblTotal.text(`TOTAL ${maskNumero(total)}`);
        }
        /**
         * Configuración del chart de alamcen
         * @param {data} almacen alamcen
         */
        function initAlmacen(almacen) {
            if (gAlmacen != undefined)
                gAlmacen.destroy();
            options.title.text = 'ALMACEN';
            let gradient = chAlmacen.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(250,174,50,2)');
            gradient.addColorStop(1, 'rgba(250,174,90,0)');
            gAlmacen = new Chart(chAlmacen, {
                type: 'bar',
                data: {
                    labels: almacen.map(a => { return a.label; }),
                    datasets: [{
                        label: '',
                        backgroundColor: gradient,
                        data: almacen.map(a => { return a.total; })
                    }]
                },
                options: options
            });
        }
        /**
         * Configuración del chart de grupo
         * @param {data} grupo grupo
         */
        function initGrupo(grupo) {
            if (gGrupo != undefined)
                gGrupo.destroy();
            options.title.text = 'GRUPO';
            let gradient = chGrupo.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(250,174,50,2)');
            gradient.addColorStop(1, 'rgba(250,174,90,0)');
            gGrupo = new Chart(chGrupo, {
                type: 'bar',
                data: {
                    labels: grupo.map(a => { return a.label; }),
                    datasets: [{
                        label: '',
                        backgroundColor: gradient,
                        data: grupo.map(a => { return a.total; })
                    }]
                },
                options: options
            });
        }
        /**
         * Configuración del chart de Compañías
         * @param {data} compania compañía
         */
        function initCompania(compania) {
            if (gCompania != undefined)
                gCompania.destroy();
            options.title.text = 'COMPAÑÍA';
            let gradient = chCompania.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, 'rgba(250,174,50,2)');
            gradient.addColorStop(1, 'rgba(250,174,90,0)');
            gCompania = new Chart(chCompania, {
                type: 'bar',
                data: {
                    labels: compania.map(a => { return a.label; }),
                    datasets: [{
                        backgroundColor: gradient,
                        label: compania.map(a => { return a.label; }),
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
                    sourceCtx.font = Chart.helpers.fontString(10, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                    sourceCtx.fillStyle = '#252121';
                    sourceCtx.textAlign = '';
                    sourceCtx.textBaseline = '';
                    sourceCtx.imageSmoothingQuality = 'high';
                    myLiveChart.data.datasets.forEach(function (dataset, i) {
                        var meta = sorceChart.controller.getDatasetMeta(i);
                        meta.data.forEach((bar, index) => {
                            var data = dataset.data[index];
                            sourceCtx.fillText(maskNumero(data), bar._model.x, bar._model.y - 8);
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
                display: true,
                fontSize: 24,
            },
            tooltips: {
                enable: true,
                callbacks: {
                    label: function (t, d) {
                        return maskNumero(d.datasets[t.datasetIndex].data[t.index]);
                    }
                }
            },
            scales: {
                yAxes: [{
                    display: true,
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        display: true,
                        beginAtZero: true,
                        fontColor: "black",
                        callback: function (value, index, values) {
                            return maskNumero(value);
                        }
                    },
                }],
                xAxes: [{
                    categoryPercentage: 0.99,
                    barPercentage: 1.0,
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
        Enkontrol.Almacen.Valuacion.Dashboard = new Dashboard();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Generando gráfica...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();