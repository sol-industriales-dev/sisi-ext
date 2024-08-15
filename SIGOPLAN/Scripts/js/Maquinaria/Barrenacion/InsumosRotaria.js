(() => {
    $.namespace('Barrenacion.InsumosRotaria');
    InsumosRotaria = function () {

        // Variables.
        const tablaEquipos = $('#tablaEquipos');
        let dtTablaEquipos;

        // Modal.
        const modalPiezas = $('#modalPiezas');
        const inputNoEconomico = $('#inputNoEconomico');
        const inputNombreEquipo = $('#inputNombreEquipo');

        // Lógica de inicialización.
        (function init() {
            cargarEquipos();
            agregarListeners();
        })();

        // init();

        // Métodos
        function cargarEquipos() {
            $.blockUI({ message: 'Cargando...' });
            $.get('/Barrenacion/ObtenerBarrenadoras').done(response => {
                if (response.success) {
                    cargarTablaEquipos(response.listaEquipos);
                } else {
                    AlertaGeneral(`Error`, `Ocurrió un error al gargar la lista de equipos.`);
                    // alert('Bad boy');
                }
            }).always($.unblockUI);
        }

        function cargarTablaEquipos(data) {
            dtTablaEquipos = tablaEquipos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                data,
                columns: [
                    { data: 'noEconomico', title: 'No. Económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'noSerie', title: 'No. Serie' },
                    { data: 'placas', title: 'Placas' },
                    { data: 'anio', title: 'Año' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-primary botonPiezas"><i class="fas fa-tools"></i> Piezas</button>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaEquipos.find('.botonPiezas').click(function () {
                        const equipo = dtTablaEquipos.row($(this).parents('tr')).data();
                        inputNoEconomico.val(equipo.noEconomico);
                        inputNombreEquipo.val(equipo.descripcion);
                        modalPiezas.modal('show');
                    });


                }
            });
        }

        function agregarListeners() {

            // Solo introducir números en el campo insumo.
            modalPiezas.on("keypress", "[id^='inputNum']", function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) //solo dígitos
                    return false;
            });

            // Autocomplete número de Insumo
            $("#modalPiezas [id^='inputNum']").toArray().forEach(input =>
                $(input).getAutocompleteValid(setInsumoDesc, validarInsumo, { porDesc: false }, '/Barrenacion/getInsumo'));

            // Autocomplete descripción de insumo
            $("#modalPiezas [id^='inputDesc']").toArray().forEach(input =>
                $(input).getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { porDesc: true }, '/Barrenacion/getInsumo'));
        }


        function setInsumoDesc(e, ui) {
            const row = $(this).closest('div.row').parent();

            row.find('.inputInsumo').val(ui.item.value);
            row.find('.inputDescripcion').val(ui.item.id);
        }

        function setInsumoBusqPorDesc(e, ui) {

            const row = $(this).closest('div.row').parent();

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputDescripcion').val(ui.item.value);
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                const row = $(this).closest('div.row').parent();
                row.find('.inputInsumo').val('');
                row.find('.inputDescripcion').val('');
            }
        }
    }
    $(document).ready(() => {
        Barrenacion.InsumosRotaria = new InsumosRotaria();
    });
})();