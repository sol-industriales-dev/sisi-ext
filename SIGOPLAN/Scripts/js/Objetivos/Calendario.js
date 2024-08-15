(() => {
    $.namespace('Objetivos.Calendario');
    Calendario = function () {
        //#region Selectores
        const calendario = $('#calendario');
        //#endregion

        (function init() {
            agregarListeners();
            initCalendario();
        })();

        function agregarListeners() {

        }

        function initCalendario() {
            calendario.fullCalendar({
                header: { left: 'prev,next today miboton,', center: 'title', right: 'listYear,month,agendaWeek' },
                timeFormat: 'H(:mm)',
                eventLimit: true,
                timezone: 'local',
                buttonText: { today: 'Hoy', month: 'Mes', week: 'Semana', day: 'Dia', list: 'Agenda' },
                locate: 'ISO',
                displayEventTime: true,
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                editable: true,
                disableDragging: true,
                events: [],
                contentHeight: 'auto',
                eventClick: function (calEvent, jsEvent, view) {

                },
                eventDrop: function (event, delta, revertFunc) {

                },
                eventResize: function (event, delta, revertFunc) {

                },
                eventRender: function (event, element) {

                }
            });
        }
    }
    $(document).ready(() => Objetivos.Calendario = new Calendario())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();