
(function () {

    $.namespace('Agendas.Agenda.Vehiculos');

    Vehiculos = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Agenda de Vehiculos',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            loadCalendario();
        }



        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl, {
                plugins: ['interaction', 'dayGrid', 'timeGrid'],
                timeZone: 'UTC',
                defaultView: 'dayGridMonth',
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                editable: true,

                // JSON FEED INSTRUCTIONS
                //
                // 1. Open a new browser tab. Go to codepen.io
                //
                // 2. Create a new pen (Create > New Pen)
                //
                // 3. Paste your JSON into the JS pane
                //
                // 4. Hit the "Save" button
                //
                // 5. The page's URL will change. It will look like this:
                //    https://codepen.io/anon/pen/eWPBOx
                //
                // 6. Append ".js" to it. Will become like this:
                //    https://codepen.io/anon/pen/eWPBOx.js
                //
                // 7. Paste this URL below.
                //
                events: 'https://fullcalendar.io/demo-events.json'

                // 8. Then, enter a date for defaultDate that best displays your events.
                //
                // defaultDate: 'XXXX-XX-XX'
            });

            calendar.render();
        });
    }
    $(document).ready(function () {

        Agendas.Agenda.Vehiculos = new Vehiculos();
    });
})();

