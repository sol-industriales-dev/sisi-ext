(() => {
    $.namespace('Administrativo.Evaluacion.AgendaActividades');
    AgendaActividades = function () {
        //#region Selectores
        const inputMes = $('#inputMes');
        const botonResultados = $('#botonResultados');
        const graficaGantt = $('#graficaGantt');
        //#endregion

        const fechaActual = new Date();

        (function init() {
            agregarListeners();
            initInputMes();
            initGantt();
        })();

        function agregarListeners() {
            botonResultados.click(getAgendaActividades);
            // inputMes.change(getAgendaActividades);
        }

        function initInputMes() {
            $('.date-picker').datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                closeText: 'Aceptar',
                currentText: 'Mes Actual',
                maxDate: fechaActual,
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();

                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');
                        $('.date-picker').focusout(); //Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker('setDate', new Date(fechaActual.getFullYear(), fechaActual.getMonth(), fechaActual.getUTCDay()));
        }

        function initGantt() {
            gantt.config.readonly = true;
            gantt.config.drag_move = false; //disables the possibility to move tasks by dnd
            gantt.config.drag_links = false; //disables the possibility to create links by dnd
            gantt.config.drag_progress = false; //disables the possibility to change the task //progress by dragging the progress knob
            gantt.config.drag_resize = false; //disables the possibility to resize tasks by dnd
            gantt.config.grid_width = 480;
            gantt.config.add_column = false;

            gantt.config.columns = [
                { name: 'text', label: 'Periodos', tree: true, width: '*' },
                // { name: 'duration', label: 'Periodicidad', tree: true, width: '*' },
                // {
                //     name: 'progress', label: 'Progreso', width: 50, align: 'center',
                //     template: function (item) {
                //         return Math.round(item.progress * 100) + '%';
                //     }
                // }
            ];

            // gantt.templates.progress_text = function (start, end, task) {
            //     return '<span style="text-align:left;"></span>';
            // };

            gantt.init('graficaGantt');
        }

        function getAgendaActividades() {
            let mes = inputMes.val();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetAgendaActividades', { mes })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        cargarGantt(response.dataPeriodos);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarGantt(actividades) {
            let tasks = { data: new Array() };
            let contador = 1;

            $.each(actividades, function (index, actividad) {
                tasks.data.push(actividad);
            });

            // tasks.data.push(actividades);

            gantt.clearAll();
            gantt.parse(tasks);
        }

        function restaFechas(f1, f2) {
            let aFecha1 = f1.split('/');
            let aFecha2 = f2.split('/');
            let fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
            let fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
            let dif = fFecha2 - fFecha1;
            let dias = Math.floor(dif / (1000 * 60 * 60 * 24));

            return Number(dias) + 1;
        }

        function getPorcentajeAvance(o) {
            return Number(o) / 100;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Evaluacion.AgendaActividades = new AgendaActividades())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();