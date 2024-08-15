(() => {
    $.namespace('Administrativo.Requerimientos.Auditoria');
    Auditoria = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectClasificacion = $('#selectClasificacion');
        const botonBuscar = $('#botonBuscar');
        const botonGuardar = $('#botonGuardar');
        const tablaCapturas = $('#tablaCapturas');
        //#endregion

        let dtCapturas;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        (function init() {
            agregarListeners();
            initTablaCapturas();
            $('.select2').select2();

            selectCentroCosto.fillComboSeguridad(false);
            selectClasificacion.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, null);
        })();

        function agregarListeners() {
            botonBuscar.click(cargarAsignacionCaptura);
            botonGuardar.click(guardarEvidencia);
        }

        function guardarEvidencia() {
            let captura = getInformacionEvidencia();
            let datosCaptura = JSON.parse(captura.get('captura'));

            let flagPorcentajeIncorrecto = datosCaptura.some(function (x) {
                return x.calificacion < 0 || x.calificacion > 100;
            });

            if (flagPorcentajeIncorrecto) {
                AlertaGeneral(`Alerta`, `La calificación no puede ser menor a cero ni mayor a cien.`);
                return;
            }

            $.ajax({
                url: '/Administrativo/Requerimientos/GuardarEvidenciaAuditoria',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                cargarAsignacionCaptura();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                cargarAsignacionCaptura();
            }
            );
        }

        function getInformacionEvidencia() {
            const data = new FormData();
            let captura = [];

            tablaCapturas.find('tbody tr').each(function (index, row) {
                let rowData = dtCapturas.row(row).data();
                let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.asignacionID}`);
                let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;
                // let aprobado = $(row).find(`.radioBtn a.active[data-toggle=radioAprobado${rowData.id}]`).attr('aprobado') == 'true';
                let calificacion = +($(row).find('.inputCalificacion').val());

                if (evidencia != null) {
                    captura.push({
                        cc: rowData.cc,
                        requerimientoID: rowData.requerimientoID,
                        puntoID: rowData.id,
                        asignacionID: rowData.asignacionID,
                        aprobado: true,
                        calificacion: calificacion
                    });

                    data.append('evidencias', evidencia);
                }
            });

            data.append('captura', JSON.stringify(captura));

            return data;
        }

        function initTablaCapturas() {
            dtCapturas = tablaCapturas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaCapturas.on('click', '.btn-descargar-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        descargarArchivo(rowData.id);
                    });

                    tablaCapturas.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtCapturas.row(row).data();
                        let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.asignacionID}`);
                        let botonEvidencia = $(row).find(`#botonEvidencia_${rowData.asignacionID}`);
                        let iconoBoton = botonEvidencia.find('i');
                        let labelArchivo = $(row).find(`#labelArchivoEvidencia_${rowData.asignacionID}`);

                        if (inputEvidencia[0].files.length > 0) {
                            let textoLabel = inputEvidencia[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonEvidencia.addClass('custom-file-upload-subido');
                            botonEvidencia.removeClass('custom-file-upload');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-file-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('custom-file-upload');
                            botonEvidencia.removeClass('custom-file-upload-subido');
                            iconoBoton.addClass('fa-file-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    // tablaCapturas.on('click', '.radioBtn a', function () {
                    //     let rowData = dtCapturas.row($(this).closest('tr')).data();
                    //     let div = $(this).closest('div');
                    //     let seleccion = $(this).attr('aprobado');

                    //     div.find(`a[data-toggle="radioAprobado${rowData.id}"]`).not(`[aprobado="${seleccion}"]`).removeClass('active').addClass('notActive');
                    //     div.find(`a[data-toggle="radioAprobado${rowData.id}"][aprobado="${seleccion}"]`).removeClass('notActive').addClass('active');
                    // });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'requerimientoDesc', title: 'Requerimiento' },
                    { data: 'puntoDesc', title: 'Punto' },
                    { data: 'codigo', title: 'Código' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return `<div class="text-center"><label id="botonEvidencia_${row.asignacionID}"for="inputEvidencia_${row.asignacionID}" class="custom-file-upload"><i class="fa fa-file-upload"></i></label>
                                    <label id="labelArchivoEvidencia_${row.asignacionID}" class="labelArchivo"></label>
                                    <input id="inputEvidencia_${row.asignacionID}" type="file" class="inputEvidencia_${row.asignacionID}" accept="application/pdf, image/*"></div>`;
                        }
                    },
                    // {
                    //     title: 'Aprobado', render: function (data, type, row, meta) {
                    //         let html = `
                    //         <div class="radioBtn btn-group">
                    //             <a class="btn btn-success notActive" data-toggle="radioAprobado${row.id}" aprobado="true" style="width: 50%;"><i class="fa fa-check"></i>&nbsp;SÍ</a>
                    //             <a class="btn btn-danger active" data-toggle="radioAprobado${row.id}" aprobado="false" style="width: 50%;"><i class="fa fa-times"></i>&nbsp;NO</a>
                    //         </div>`;

                    //         return html;
                    //     }
                    // }
                    {
                        title: 'Calificación', render: function (data, type, row, meta) {
                            let html = `<input type="number" class="form-control text-center inputCalificacion">`;

                            return html;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [4] }
                ]
            });
        }

        function cargarAsignacionCaptura() {
            let clasificacion = +(selectClasificacion.val());
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000){
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetAsignacionCapturaAuditoria', { idEmpresa, idAgrupacion, clasificacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaCapturas, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function descargarArchivo(evidenciaID) {
            location.href = `/Administrativo/Requerimientos/DescargarArchivoEvidencia?evidenciaID=${evidenciaID}`;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Requerimientos.Auditoria = new Auditoria())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();