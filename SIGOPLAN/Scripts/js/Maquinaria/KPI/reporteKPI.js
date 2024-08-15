(() => {
    $.namespace('KPI.reporteKPI');

    reporteKPI = function () {
        urlCboCC = '/Conciliacion/getCboCC';
        colExcel = [];
        // Variables.
        const CargarCaptura = originURL('/KPI/CargarCapturaKPI');
        const selBusqCC = $('#selBusqCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const btnBuscar = $('#btnBuscar');
        const report = $('#report');
        (function init() {
            // Lógica de inicialización.
            GeneraArregloColumnas();
            initObjects();
            btnBuscar.click(setTabla);
        })();

        function initObjects() {

            selBusqCC.fillCombo(urlCboCC, null, false, null);
            inputFechaInicio.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
            inputFechaFin.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
        }

        function setTabla() {
            $.post('/KPI/CargarCapturaKPI', { fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val(), ac: $("#selBusqCC option:selected").attr('data-prefijo') })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        InyectarPanel(response.lst, response.maxCol, response.maxRow);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function InyectarPanel(lst, maxCol, maxRow) {
            let tbl = $(`<table>`, {
                class: "tblPanel table"
            }),
                rowSpanciones = [],
                colSpanciones = [];
            for (let iRow = 1; iRow < +maxRow + 1; iRow++) {
                let tr = $(`<tr>`);
                tr.attr('data-row', iRow);
                // let rowSpan = rowSpanciones.find(span => span.row === iRow);
                // if (rowSpan === undefined) {
                //     rowSpanciones.push({
                //         row: iRow,
                //         span: 1
                //     });
                // }
                for (let iCol = 0; iCol < colExcel.length; iCol++) {
                    const cCol = colExcel[iCol];
                    let celda = lst.find(data => data.col === cCol && data.row === iRow.toString());
                    if (celda === undefined) {
                        celda = CeldaDefault(iRow, cCol);
                    }
                    let td = $(`<td>`, {
                        text: celda.valor,
                        class: celda.clase,
                        style: `background-color: rgba(${celda.color.red}, ${celda.color.green}, ${celda.color.blue}, ${celda.color.aplha})`
                    });
                    td.attr('data-col', cCol);
                    // let celSpan = colSpanciones.find(span => span.col === cCol);
                    // if (celSpan === undefined) {
                    //     colSpanciones.push({
                    //         col: cCol,
                    //         span: 1
                    //     });
                    // }
                    // if (celSpan.span > 1) {
                    //     if (celSpan.span === celda.celSpan) {

                    //     }
                    // }
                    tr.append(td);
                    if (cCol === maxCol) {
                        break;
                    }
                }
                tbl.append(tr);
            }
            report.empty();
            report.append(tbl);
        }
        function GeneraArregloColumnas() {
            var i1, i2, i3;
            for (i1 = 0; i1 < 26; i1++) {
                colExcel.push(String.fromCharCode(65 + i1));
            }
            for (i1 = 0; i1 < 26; i1++) {
                for (i2 = 0; i2 < 26; i2++) {
                    colExcel.push(String.fromCharCode(65 + i1) + String.fromCharCode(65 + i2));
                }
            }
            for (i1 = 0; i1 < 26; i1++) {
                for (i2 = 0; i2 < 26; i2++) {
                    for (let i3 = 0; i3 < 26; i3++) {
                        colExcel.push(String.fromCharCode(65 + i1) + String.fromCharCode(65 + i2) + String.fromCharCode(65 + i3));
                    }
                }
            }
        }
        function CeldaDefault(row, col) {
            let celda = {
                valor: "",
                row: row,
                col: col,
                colSpan: 1,
                rowSpan: 1,
                clase: "",
                color: {
                    aplha: 0,
                    red: 0,
                    blue: 0,
                    green: 0
                }
            };
            return celda;
        }
        // Métodos.


    }

    $(() => KPI.reporteKPI = new reporteKPI())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();