(function () {

    $.namespace('Maquinaria.Rentabilidad.CorteKubrixCplan');

    CorteKubrixCplan = function () {
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        // --> ELEMENTOS
        const chkEjercicio = $("#chkEjercicio");
        const cbTipoCorte = $("#cbTipoCorte");
        const inputCorte = $("#inputCorte");
        const cbDivision = $("#cbDivision");
        const cbResponsable = $("#cbResponsable");
        const cbCC = $("#cbCC");
        const btnBuscar = $("#btnBuscar");
        
        let availableDates = [];
        
        function init() {
            // --> INICIALIZADORES
            cbCC.select2();
            initDatePickers();              

            // --> CARGAR COMBOS
            cbDivision.fillCombo('/Rentabilidad/fillComboDivision', {}, false, "TODAS");
            cbResponsable.fillCombo('/Rentabilidad/fillComboResponsable', {}, false, "TODOS");
            setResponsable();
            cargarObra();

            // --> LISTENERS
            cbTipoCorte.change(updateDatePicker);
            cbDivision.change(cargarObra);
            cbResponsable.change(cargarObra);
            
        }

        function setLstFechaCortes() {
            $.get('/Rentabilidad/getLstFechasCortes', { tipoCorte: cbTipoCorte.val() })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        availableDates = [];
                        for(let i = 0; i < response.fechas.length; i++) { availableDates.push(new Date(parseInt(response.fechas[i].substr(6)))); }
                        if(availableDates.length > 0) { inputCorte.datepicker("setDate", availableDates[availableDates.length - 1] ); }
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
        }

        function initDatePickers()
        {            
            let yearActual = new Date().getFullYear();
            setLstFechaCortes();

            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                },
                beforeShowDay: function(date)
                {
                    var auxArr = false;
                    $.each(availableDates, function( index, value ) {
                        if(date.getDate() == value.getDate() && date.getMonth() == value.getMonth() &&  date.getYear() == value.getYear()) { 
                            auxArr = true; 
                        }
                    });
                    return [auxArr, ""];
                }
            });
        }

        function updateDatePicker()
        {
            setLstFechaCortes();
            inputCorte.datepicker("destroy");
            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                },
                beforeShowDay: function(date)
                {
                    var auxArr = false;
                    $.each(availableDates, function( index, value ) {
                        if(date.getDate() == value.getDate() && date.getMonth() == value.getMonth() &&  date.getYear() == value.getYear()) { 
                            auxArr = true; 
                        }
                    });
                    return [auxArr, ""];
                }
            });
            inputCorte.val("");
        }

        function setResponsable()
        {
            $.post('/Rentabilidad/checkResponsable', { responsableID: cbResponsable.val() })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        responsable = response.responsable;
                    } else {
                        // Operación no completada.
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                }
            );
        }

        function cargarObra()
        {
            setResponsable();
            cbCC.fillCombo('cboObraKubrix', {divisionID: cbDivision.val(), responsableID: cbResponsable.val() }, true);
            cbCC.find('option').get(0).remove();
        }  

        init();
    };

    $(document).ready(function () {

    Maquinaria.Rentabilidad.CorteKubrixCplan = new CorteKubrixCplan();
    }).ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
      .ajaxStop(() => { $.unblockUI(); });
})();