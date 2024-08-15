$(function () {

    $.namespace('recursoshumanos.Layout.AltasRH');

    AltasRH = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        selected = false;
        tblEmpleadosNomina = $("#tblEmpleadosNomina"),
            btnBuscar = $("#btnBuscar"),
            btnExportar = $("#btnExportar"),
            fechaIni = $("#fechaIni"),
            fechaFin = $("#fechaFin"),
            btnSeleccionar = $("#btnSeleccionar"),
            comboAC = $("#comboAC"),
            btnLimpiarCC = $("#btnLimpiarCC")

        function init() {
            comboAC.select2({ closeOnSelect: false });
            comboAC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "TODOS");
            comboAC.find('option').get(0).remove();

            $("#spanComboAC").click(function (e) {
                comboAC.next(".select2-container").css("display", "block");
                comboAC.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboAC.on('select2:close', function (e) {
                comboAC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
                else {
                    if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
                }
            });
            comboAC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });

            // btnLimpiarCC.on("click", function () {
            //     comboAC.val("");
            //     comboAC.trigger("change");
            //     spanComboAC.trigger("change");
            // });

            fechaIni.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());

            btnBuscar.click(loadTabla);
            btnExportar.click(exportData);
            btnSeleccionar.click(selectAll);

            // $("#btnLimpiar").on("click", function () {
            //     cboCC.val("");
            //     cboCC.trigger("change");
            // });

            // cboCC.on("change", function () {

            //     if ($(this).length > 0) {
            //         let arrCantCC = new Array();

            //         let todosSeleccionados = $("#cboCC")[0].selectedIndex;
            //         if (todosSeleccionados == 0) {
            //             $("#cboCC > option").prop("selected", "selected");
            //         } else if (todosSeleccionados == 1) {
            //             let cantCC_Disponibles = $("#cboCC > option").length;
            //             let cantCC_Seleccionados = 0;
            //             $(this).val().forEach(element => {
            //                 cantCC_Seleccionados += 1;
            //             });
            //             cantCC_Seleccionados += 1;

            //             if (cantCC_Disponibles == cantCC_Seleccionados) {
            //                 $(this).val("");
            //             }
            //         }

            //         $(this).val().forEach(element => {
            //             arrCantCC.push(element);
            //         });

            //         let cant = `Seleccionados: ${arrCantCC.length}`;
            //         $("#lblCC").html(`Centro Costos: ${cant}`);

            //         if (arrCantCC.length == 0 && $(this).length <= 0) {
            //             cboCC.val("");
            //             cboCC.trigger("change");
            //         }
            //         // $(".select2-selection__choice").css("display", "none");
            //     }
            // });

            $("#spanComboAC").trigger("click");
            $("#spanComboAC").trigger("click");
        }

        function getFiltrosObject() {
            let arrCC = new Array;
            if (comboAC.val() == "") {
                $("#comboAC > option").prop("selected", "selected");
                comboAC.val().forEach(element => {
                    arrCC.push(element);
                });
            } else {
                comboAC.val().forEach(element => {
                    arrCC.push(element);
                });
            }

            return {
                // cc: getValoresMultiples("#cboCC"),
                cc: arrCC,
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val()
            }
        }


        function selectAll() {

            if (!selected) {
                selected = true;
                $("input:checkbox[name=cckEmpleados]").prop('checked', true);
            }
            else {
                selected = false;
                $("input:checkbox[name=cckEmpleados]").prop('checked', false);
            }

        }

        function loadTabla() {
            loadGrid(getFiltrosObject(), "/Administrativo/LayoutRH/fillTableLayoutAltasRH", tblEmpleadosNomina);
        }

        function exportData() {

            var array = [];
            $("input:checkbox[name=cckEmpleados]:checked").each(function () {
                array.push($(this).attr('data-cveEmpleado'));
            });

            if (array.length > 0) {
                getFileDownload(array);
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar almenos un registro para exportar!");
            }
            // window.location = '/Administrativo/LayoutRH/getFileDownload';
        }

        function getFileDownload(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/LayoutRH/ExportInformacion',
                type: "POST",
                datatype: "json",
                data: { empleados: array },
                success: function (response) {
                    $.unblockUI();
                    window.location = '/Administrativo/LayoutRH/getFileDownloadAltas';
                    AlertaGeneral("Confirmación", "¡Archivo Generado Correctamente!");
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function initGrid() {

            tblEmpleadosNomina.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "seleccion": function (column, row) {
                        return "<input type='checkbox' style=\"width: 15px;height: 15px;\" name='cckEmpleados' data-cveEmpleado='" + row.EMP_TRAB + "'>";
                    }

                }
            });
        }
        initGrid();
        init();
    }

    $(document).ready(function () {
        recursoshumanos.Layout.AltasRH = new AltasRH();
    });
});