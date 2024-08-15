(() => {
    $.namespace('RecursosHumanos.Desempeno.Calendario');
    
    Calendario = function () {
        const divCalendario = document.getElementById('divCalendario');
        const listaProximosEventos = $('#listaProximosEventos');

        let fechaActualServidor = '2020/06/19';

        function initCalendario(eventos) {
            var calendar = new FullCalendar.Calendar(divCalendario, {
                plugins: ['dayGrid', 'moment', 'interaction'],
                titleFormat: 'D MMMM YYYY',
                locale: 'es',
                defaultDate: fechaActualServidor,
                height: 650,

                events: eventos,

                // dateClick: function(info) {
                //     alert('a day has been clicked! ' + info.dateStr);
                //     calendar.next();
                // }
            });
            calendar.render();
        }

        function setProximosEventos(eventos) {
            let contador = 0;
            eventos.forEach(function callback(currentValue, index, array) {
                if(contador == 5) { return }

                if(moment(currentValue.start).isAfter(moment(fechaActualServidor)) && currentValue.inicio == true) {
                    listaProximosEventos.append('<li>' + currentValue.title + '</li>');
                    contador++;
                }
            });
        }

        function getEvaluaciones() {
            $.get('/Desempeno/GetEvaluaciones', {
                idUsuarioVerComo: 0
            }).always().then(response => {
                if(response.Success) {
                    setProximosEventos(response.Value);
                    initCalendario(response.Value);
                }
                else {
                    alert(response.Message);
                }
            }, error => {
                AlertaGeneral('Alerta:', null);
            });
        }

        function getFechaActual() {
            $.get('/Desempeno/FechaActual').always().then(response => {
                fechaActualServidor = moment(response).format('YYYY-MM-DD');
                getEvaluaciones();
            }, error => {
                AlertaGeneral('Alerta:', null);
            });
        }

        let init = () => {
            getFechaActual();
        }

        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno.Calendario = new Calendario();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...'}); })
    .ajaxStop(() => { $.unblockUI(); });
})();