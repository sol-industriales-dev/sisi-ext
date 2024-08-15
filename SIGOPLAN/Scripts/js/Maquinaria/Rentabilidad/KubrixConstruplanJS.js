(function () {

    $.namespace('maquinaria.rentabilidad.KubrixConstruplan');

    KubrixConstruplan = function () {
        // --> Filtros 
        const cboTipoCorte = $("#cboTipoCorte");
        const cboFechaCorte = $("#cboFechaCorte");
        //const cboDivision = $("#cboDivision");
        //const cboResponsable = $("#cboResponsable");
        const cboCC = $("#cboCC");

        const cboEjercicio = $("#cboEjercicio");
        const btnBuscar = $("#btnBuscar");
        // <--

        const tablaNivel1 = $("#tablaNivel1");
        let dtTablaNivel1;

        function init() {
            $(".tipoAgrupacion").on("change", function () {
                $(".tipoAgrupacion").not(this).prop("checked", false);
            });
            initInformacionNivel1();
            // --> Inicializar combos
            cboTipoCorte.select2();
            cboFechaCorte.select2();
            cboFechaCorte.fillCombo("/Rentabilidad/fillComboFechasConstruplan", { tipoCorte: cboTipoCorte.val() });
            cboFechaCorte.find("option").get(0).remove();
            //cboDivision.select2();
            //cboDivision.fillCombo("/Rentabilidad/fillComboDivision", {}, false, "TODAS");
            //cboResponsable.select2();
            //cboResponsable.fillCombo("/Rentabilidad/fillComboResponsable", {}, false, "TODOS");
            cboCC.select2();
            cboCC.fillCombo("/Rentabilidad/fillComboCCConstruplan", {}, true, "TODOS");

            btnBuscar.on("click", function () {
                cargarInformacionNivel1();
            });
        }

        function initInformacionNivel1()
        {
            dtTablaNivel1 = tablaNivel1.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columnDefs: [
                    //{ targets: [0, 1, 2, 3, 4, 5, 9], width: '8px' },
                ],
                columns: [
                    {
                        title: 'Descripción',
                        data: 'descripcion'
                    },
                    {
                        title: 'Semana Actual',
                        data: 'semana1'
                    },
                    {
                        title: 'Semana 2',
                        data: 'semana2'
                    },
                    {
                        title: 'Semana 3',
                        data: 'semana3'
                    },
                    {
                        title: 'Semana 4',
                        data: 'semana4'
                    },
                    {
                        title: 'Semana 5',
                        data: 'semana5'
                    },
                    {
                        title: 'Semana 6',
                        data: 'semana6'
                    },

                ],
                drawCallback: function (settings) {
                        
                },
                initComplete: function (settings, json) {
                    
                }
            });
            
        }

        function cargarInformacionNivel1()
        {
            $.get("/Rentabilidad/cargarInformacionNivel1", { corteID: cboFechaCorte.val(), listaCC: cboCC.val() })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        dtTablaNivel1.clear().draw();
                        dtTablaNivel1.rows.add(response.listaNivel1).draw();
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.rentabilidad.KubrixConstruplan = new KubrixConstruplan();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: "Procesando..." }) }).ajaxStop($.unblockUI);
})();