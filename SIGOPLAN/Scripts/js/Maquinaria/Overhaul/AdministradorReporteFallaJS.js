(() => {
    $.namespace('maquinaria.overhaul.Administradorreportefalla');

    Administradorreportefalla = function () {
        var idReporte = 0;
        var bandera = false;
        const cboCC = $("#cboCC");
        const ireport = $("#report");
        const btnBuscar = $("#btnBuscar");
        const gridReportes = $("#gridReportes");
        const cboEstatusReporte = $("#cboEstatusReporte");
        const txtFiltroEconomico = $("#txtFiltroEconomico");
        const rptComponente = new URL(window.location.origin + '/Reportes/Vista.aspx?idReporte=152');
        const GetReporteFalla = new URL(window.location.origin + '/ReporteFalla/GetReporteFalla');
        const CargarReportesFalla = new URL(window.location.origin + '/ReporteFalla/CargarReportesFalla');
        const aprobarReporteFalla = new URL(window.location.origin + '/ReporteFalla/aprobarReporteFalla');
        const EliminarReporteFalla = new URL(window.location.origin + '/ReporteFalla/EliminarReporteFalla');
        const tblM_ReporteFalla_Archivo = $('#tblM_ReporteFalla_Archivo');
        const mdlListadoArchivos = $('#mdlListadoArchivos');
        let dtArchivos;

        function init() {
            initForm();
            initTblArchivos();
            btnBuscar.click(cargarGrid);
            cboEstatusReporte.change(cargarGrid);
            $(document).on('click', "#btnModalEliminar", () => {
                if (bandera) {
                    aprobarReporte();
                } else {
                    eliminarReporte();
                }
                bandera = false;
                cargarGrid();
            });
        }

        function initForm() {
            initGrid();
            cboCC.fillCombo('/CatComponentes/FillCbo_CentroCostos');
            cargarGrid();
        }

        function initGrid() {
            dtReportes = gridReportes.DataTable({
                destroy: true
                , language: dtDicEsp
                , class: "text-center"
                , columns: [
                    { data: 'noEconomico', title: `Economico` }, 
                    { data: 'cc', title: `Area Cuenta` }, 
                    { data: 'fechaParo', title: `Fecha De Paro` }, 
                    { data: 'fechaReporte', title: `Fecha De Reporte` }, 
                    { data: 'falla', title: `Falla Por` }, 
                    { data: 'componenteInsumo', title: `Componente<br>Insumo` }, 
                    {
                        data: 'estatus', title: `Estatus`, createdCell: function (td, data) {
                            let estatus = "";
                            switch (data) {
                                case 0: estatus = `INCOMPLETO`; break;
                                case 1: estatus = `COMPLETADO`; break;
                                case 2: estatus = `APROBADO`; break;
                                default:
                                    break;
                            }
                            $(td).html(`<span>${estatus}</span>`);
                        }
                    }, 
                    { data: 'estatus', title: `Detalles`, width: `3%`, render: data => `<button type='button' class='btn btn-default ver'><i class="fa fa-table"></i></button>` }, 
                    { data: 'estatus', title: `Reporte`, width: `3%`, render: data => `<button type='button' class='btn btn-primary print' ${data === 2 ? "" : "disabled"}><i class="fa fa-print"></i></button>` }, 
                    { data: 'estatus', title: `Aprobar`, width: `3%`, render: data => `<button type='button' class='btn btn-success aprobar' ${data === 1 ? "" : "disabled"}><i class="fa fa-check"></i></button>` }, 
                    { data: 'estatus', title: `Eliminar`, width: `3%`, render: data => `<button type='button' class='btn btn-danger eliminar' ><i class="fa fa-minus"></i></button>` },
                    { title: `Archivos`, width: `3%`, render: data => `<button type='button' class='btn btn-primary mdlArchivosDescargar'><i class="fas fa-folder-open"></i></button>` }
                ], 
                initComplete: function (settings, json) {
                    gridReportes.on("click", ".ver", function () {
                        setIdFromBtnClick(this);
                        window.location.href = `/Overhaul/ReporteFallaVista?id=${idReporte}`;
                    });
                    gridReportes.on("click", ".print", function () {
                        setIdFromBtnClick(this);
                        abrirReporte();
                    });
                    gridReportes.on("click", ".aprobar", function () {
                        bandera = true;
                        setIdFromBtnClick(this);
                        noEconomico = getNoEconomicoFromBtnClick(this);
                        ConfirmacionEliminacion("Aprobar Reporte de Falla", `Se aprobará el reporte de falla para el equipo "${noEconomico}", ¿Desea continuar?`);
                    });
                    gridReportes.on("click", ".eliminar", function () {
                        bandera = false;
                        setIdFromBtnClick(this);
                        noEconomico = getNoEconomicoFromBtnClick(this);
                        ConfirmacionEliminacion("Eliminar Reporte de Falla", `Se eliminará el reporte de falla para el equipo "${noEconomico}", ¿Desea continuar?`);
                    });
                    gridReportes.on("click", ".mdlArchivosDescargar", function () {
                        setIdFromBtnClick(this);
                        fncGetArchivosReporteFalla(idReporte);
                        mdlListadoArchivos.modal("show");
                    });
                }
            });
        }

        function setIdFromBtnClick(btn) {
            let row = $(btn).closest("tr");
            idReporte = dtReportes.row(row).data().id;
        }

        function getNoEconomicoFromBtnClick(btn) {
            let row = $(btn).closest("tr")
                , noEconomico = dtReportes.row(row).data().noEconomico;
            return noEconomico;
        }

        async function cargarGrid() {
            try {
                let estatus = cboEstatusReporte.val();
                dtReportes.clear().draw();
                response = await ejectFetchJson(CargarReportesFalla, {
                    estatus: estatus,
                    noEconomico: txtFiltroEconomico.val() == "" ? -1 : txtFiltroEconomico.val(),
                    cc: cboCC.val() == "" ? -1 : cboCC.val(),
                });
                if (response.success) {
                    dtReportes.rows.add(response.reportes);
                    switch (estatus) {
                        case "0":
                            dtReportes.columns(8).visible(false);
                            dtReportes.columns(9).visible(true);
                            break;
                        case "1":
                            dtReportes.columns(8).visible(true);
                            dtReportes.columns(9).visible(false);
                            break;
                        default:
                            break;
                    }
                    dtReportes.draw();
                }
                else {
                    AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                }
            } catch (o_O) { AlertaGeneral("Alerta", o_O.message); }
        }

        async function eliminarReporte() {
            try {
                response = await ejectFetchJson(EliminarReporteFalla, { id: idReporte });
                if (response.success) {
                    AlertaGeneral("Alerta", "El reporte se eliminó correctamente");
                }
            } catch (o_O) { AlertaGeneral('Alerta', o_O.message) }
        }

        async function abrirReporte() {
            try {
                $(`body`).block({ message: "Generando Reporte..." });
                response = await ejectFetchJson(GetReporteFalla, { idReporte: idReporte });
                if (response.success) {
                    ireport.attr("src", rptComponente);
                    document.getElementById('report').onload = () => {
                        openCRModal();
                        $(`body`).unblock();
                    }
                }
            } catch (o_O) { AlertaGeneral('Alerta', o_O.message) }
        }

        async function aprobarReporte() {
            try {
                response = await ejectFetchJson(aprobarReporteFalla, { idReporte: idReporte });
            } catch (o_O) { AlertaGeneral('Alerta', o_O.message) }
        }

        function initTblArchivos() {
            dtArchivos = tblM_ReporteFalla_Archivo.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombre', title: 'Archivo' },
                    { data: 'strTipo', title: 'Tipo' },
                    {
                        title: "Descargar",
                        render: function (data, type, row) {
                            return `<button class="btn btn-xs btn-primary descargarArchivo" title="Descargar archivo."><i class="fas fa-download"></i></button>`;
                        },
                    },
                    { data: 'tipo', visible: false },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblM_ReporteFalla_Archivo.on('click','.descargarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Descarga', '¿Desea descargar el archivo seleccionado?', 'Confirmar', 'Cancelar', () => fncDescargarArchivoReporteFalla(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }

        function fncGetArchivosReporteFalla(_idReportefalla) {
            if (_idReportefalla > 0) {
                let obj = new Object();
                obj = {
                    _idReportefalla: _idReportefalla
                }
                axios.post("GetArchivosReporteFalla", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtArchivos.clear();
                        dtArchivos.rows.add(response.data.data);
                        dtArchivos.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al consultar los archivos.");
            }
        }

        function fncDescargarArchivoReporteFalla(_idArchivo) {
            // const filtro = JSON.stringify(_idArchivo);

            const data = new FormData();
            data.append('filtro', _idArchivo);

            var xhr = new XMLHttpRequest();
            xhr.responseType = 'blob';

            xhr.onloadstart = () => {
                $.blockUI({
                    message: 'Procesando...',
                    baseZ: 2000
                });
            }

            xhr.onload = () => {
                if (xhr.status == 200) {
                    var blob = xhr.response;

                    if (blob.size > 0) {
                        var fileName = xhr.getResponseHeader('content-disposition');
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = fileName.split('filename=')[1];
                        link.click();
                        Alert2Exito("Se ha descargado con éxito el archivo.");
                    } else {
                        Alert2Error('Ocurrió un error al descargar el archivo.');
                    }
                } else {
                    Alert2Error(`Ocurrió un error al lanzar la petición al servidor. ${xhr.status} ${xhr.statusText}`);
                }
            }

            xhr.onerror = () => {
                Alert2Error(`Ocurrió un error al lanzar la petición al servidor.`);
            }

            xhr.addEventListener('loadend', () => {
                $.unblockUI();
            });

            xhr.open('POST', 'DescargarArchivoReporteFalla');
            xhr.send(data);
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.Administradorreportefalla = new Administradorreportefalla();
    });
})();