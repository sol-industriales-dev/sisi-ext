(function () {

    $.namespace('maquinaria.reporte.repGanttMaquinaria');

    repGanttMaquinaria = function () {

        cboCC = $("#cboCC");
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        cboEscala = $("#cboEscala"),
        cboFiltroTipo = $("#cboFiltroTipo");
        cboFiltroGrupo = $("#cboFiltroGrupo");

        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        
        function init() {

            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            cboFiltroTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });  //.fillCombo('/RepGastosMaquinaria/fillCboTipoMaquina');
            cboFiltroTipo.change(FillCboGrupo);

            convertToMultiselect("#cboFiltroGrupo");
            btnBuscar.click(BuscarMaquinas);
            cboEscala.val(2);
            cboEscala.change(zoomGantt);

            // Gantt

            gantt.config.readonly = true;
            gantt.config.drag_move = false; //disables the possibility to move tasks by dnd
            gantt.config.drag_links = false; //disables the possibility to create links by dnd
            gantt.config.drag_progress = false; //disables the possibility to change the task //progress by dragging the progress knob
            gantt.config.drag_resize = false; //disables the possibility to resize tasks by dnd
            gantt.config.grid_width = 600;
            gantt.config.add_column = false;
            gantt.config.columns = [
                { name: "text", label: "Maquina", tree: true, width:"*"},
                { name: "inicio", label: "Inicio", width: 75 },
                { name: "fin", label: "Fin", width: 75 },
                { name: "duration", label: "Dias", width: 75 },
                { name: "avance", label: "Avance", width: 75 },

            ];
            gantt.templates.progress_text = function (start, end, task) {
                return "<span style='text-align:left;'></span>";
            };
            gantt.config.scale_unit = "week";
            gantt.config.min_column_width = 150;
            gantt.config.step = 1;
            gantt.config.date_scale = "Semana #%W - %Y";
            gantt.templates.scale_cell_class = function (date) {
                if (date.getMonth() % 2 == 0) { return "par"; }
                else { return "impar"; }
            };

            gantt.config.subscales = [
		        { unit: "week", step: 1, template: weekScaleTemplate }
            ];

            gantt.init("gantt_here");

            //Gantt

            btnImprimir.click(imprimirGantt);

        }

        function imprimirGantt() {
            gantt.exportToPDF({
                name: "mygantt.pdf",
                header: "<h1 style=' text-align: center;'>Grupo Construcciones Planificadas S.A. de C.V.</h1>"
                        + "<h3 style=' text-align: center;'>Maquinaria</h3>"
                        + "<h3 style=' text-align: center;'>Diagrama de Gantt por Centro de Costo</h3>"
                        + "Centro de Costo: " + getValoresMultiples("#cboCC"),
                locale: "en",
                skin: 'meadow',
            });
        }

        var weekScaleTemplate = function (date) {
            var dateToStr = gantt.date.date_to_str("%M %d");
            var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
            return dateToStr(date) + " - " + dateToStr(endDate);
        };

        function BuscarMaquinas() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/RepGanttMaquinaria/getMaquinas",
                data: { ccs: getValoresMultiples("#cboCC"), grupos: getValoresMultiples("#cboFiltroGrupo") },
                success: function (response) {

                    btnImprimir.show();

                    loadGantt(response);

                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }        

        function loadGantt(data) {

            gantt.clearAll();

            
            var tasks = {};
            tasks.data = new Array();
            var cont = 1;
            $.each(data, function (i, e) {
                var obj = {};
                obj.id = e.id;
                obj.text = e.text;
                obj.inicio = e.inicio,
                obj.avance = e.avance * 100 + "%",
                obj.fin = e.fin,
                obj.start_date = e.start_date;
                obj.duration = "" + restaFechas(e.inicio, e.fin);
                obj.parent = e.parent;
                obj.grupoID = e.grupoID,
                obj.progress = e.progress,
                obj.cc = e.cc;
                obj.open = e.open;
                obj.color = e.color;
                obj.textColor = e.textColor;
                obj.progressColor = e.progressColor;
                tasks.data.push(obj);
            });
            gantt.parse(tasks);
        }

        function zoomGantt() {

            switch (cboEscala.val()) {
                case '1':
                    gantt.config.scale_unit = "week";
                    gantt.config.min_column_width = 150;
                    gantt.config.step = 1;
                    gantt.config.date_scale = "Semana #%W - %Y";
                    gantt.config.subscales = [
		                { unit: "week", step: 1, template: weekScaleTemplate }
                        ];
                    break;
                case '2':
                    gantt.config.scale_unit = "month";
                    gantt.config.min_column_width = 150;
                    gantt.config.step = 1;
                    gantt.config.date_scale = "%M - %Y";
                    gantt.config.subscales = [];
                    break;
                case '3':
                    gantt.config.scale_unit = "year";
                    gantt.config.min_column_width = 150;
                    gantt.config.step = 1;
                    gantt.config.date_scale = "%Y";
                    gantt.config.subscales = [];
                    break;
            }

            gantt.render();
        }
        function FillCboGrupo() {
            clearCbo();
            if (cboFiltroTipo.val() != "") {
                cboFiltroGrupo.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboFiltroTipo.val() }, false, "Todos");//.fillCombo('/RepGastosMaquinaria/fillCboGrupoMaquina', { idTipo: cboFiltroTipo.val() });
                convertToMultiselect("#cboFiltroGrupo");
            }
            else {
                cboFiltroGrupo.val("");
                cboFiltroGrupo.attr('disabled', true);
            }
        }
        function clearCbo() {
            cboFiltroGrupo.attr('disabled', false);
        }
        init();

    };

    function restaFechas(f1, f2) {
        var aFecha1 = f1.split('/');
        var aFecha2 = f2.split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        return Number(dias) + 1;
    }

    $(document).ready(function () {

        maquinaria.reporte.repGanttMaquinaria = new repGanttMaquinaria();
    });
})();
