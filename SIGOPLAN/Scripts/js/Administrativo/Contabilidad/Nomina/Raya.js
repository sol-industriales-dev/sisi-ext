Dropzone.autoDiscover = false;

(() => {
    $.namespace('Administrativo.Contabilidad.Raya');
    Raya = function () {
        //#region selectores
        const panelGeneral = $('#panelGeneral');
        const cboxPeriodoFiltro = $('#cboxPeriodoFiltro');
        const cboxTipoRayaFiltro = $('#cboxTipoRayaFiltro');
        const cboxClasificacionCCFiltro = $('#cboxClasificacionCCFiltro');
        const btnConsultarRaya = $('#btnConsultarRaya');
        const btnCargarRaya = $('#btnCargarRaya');
        const inputArchivoRaya = $('#inputArchivoRaya');
        const cboTipoNomina = $('#cboTipoNomina');
        //const dzRaya = $('#dzRaya');

        const tblRayaTotales = $('#tblRayaTotales');
        const tblRaya = $('#tblRaya');
        //#endregion

        //#region variables
        //let _dzRaya;
        let _periodo = 0;
        let _tipoPeriodo = 0;
        let _year = 0;

        let _idNomina = 0;
        //#endregion

        //#region eventos
        cboxPeriodoFiltro.on('change', function() {
            const dataPeriodo = cboxPeriodoFiltro.find('option:selected').data('prefijo').split('-');
            _periodo = cboxPeriodoFiltro.val();
            _tipoPeriodo = dataPeriodo[2];
            _year = dataPeriodo[3];
        });

        btnCargarRaya.on('click', function() {
            if (cboxPeriodoFiltro.val() && cboxTipoRayaFiltro.val() && inputArchivoRaya.val()) {
                //_dzRaya.processQueue();

                const archivoRaya = inputArchivoRaya.get(0).files[0];

                let formData = new FormData();

                formData.append('periodo', _periodo);
                formData.append('tipoPeriodo', _tipoPeriodo);
                formData.append('year', _year);
                formData.append('tipoRaya', cboxTipoRayaFiltro.val());
                formData.append('raya', archivoRaya);

                cargarRaya(formData);
            }
            else {
                swal('Alerta!', 'Debe seleccionar un período, tipo de raya y clasificación cc', 'warning');
            }
        });

        btnConsultarRaya.on('click', function() {
            if (cboxPeriodoFiltro.val() && cboxTipoRayaFiltro.val() && cboxClasificacionCCFiltro.val()) {
                getRayaCargada(_periodo, _tipoPeriodo, _year, cboxTipoRayaFiltro.val(), cboxClasificacionCCFiltro.val());
            }
            else {
                swal('Alerta!', 'Debe seleccionar un período, tipo de raya y clasificación cc', 'warning');
            }
        });
        //#endregion

        //#region tablas
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTbls() {
            tblRayaTotales.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                fixedColumns: {
                    leftColumns: 3
                },

                columns: [
                    { data: 'cc', title: 'C.C.', className: 'text-center' },
                    { data: 'ccDescripcion', title: 'Nombre C.C.' },
                    { data: 'empleados', title: 'Empleados', className: 'text-center' },
                    { data: 'totales.sueldoUnidades', title: 'Sueldo unidades', className: 'text-center' },
                    { data: 'totales.sueldoImporte', title: 'Sueldo importe', className: 'text-right' },
                    { data: 'totales.septimoDiaUnidades', title: 'Septimo día unidades', className: 'text-right', visible: false },
                    { data: 'totales.septimoDiaImporte', title: 'Septimo día', className: 'text-right' },
                    { data: 'totales.horasExtraDoblesUnidades', title: 'Horas extras dobles unidades', className: 'text-right', visible: false },
                    { data: 'totales.horasExtraDoblesImporte', title: 'Hrs extras dobles', className: 'text-right' },
                    { data: 'totales.faltaInjustificadaUnidades', title: 'Falta injustificada unidades', className: 'text-right', visible: false },
                    { data: 'totales.premiosAsistenciaUnidades', title: 'Premios asistencia unidades', className: 'text-right', visible: false },
                    { data: 'totales.premiosAsistenciaImporte', title: 'Premios asistencia', className: 'text-right' },
                    { data: 'totales.bonoPuntualidadUnidades', title: 'Bono puntualidad unidades', className: 'text-right', visible: false },
                    { data: 'totales.bonoPuntualidadImporte', title: 'Bono puntualidad', className: 'text-right' },
                    { data: 'totales.fondoAhorroEmpresaImporte', title: 'F.A. empresa', className: 'text-right' },
                    { data: 'totales.primaVacacionalUnidades', title: 'Prima vacacional unidades', className: 'text-right', visible: false },
                    { data: 'totales.primaVacacionalImporte', title: 'Prima vacacional', className: 'text-right' },
                    { data: 'totales.primaVacacional2Unidades', title: 'Prima vacacional unidades', className: 'text-right', visible: false },
                    { data: 'totales.primaVacacional2Importe', title: 'Prima vacacional', className: 'text-right' },
                    { data: 'totales.bonoProduccionUnidades', title: 'Bono de producción unidades', className: 'text-right', visible: false },
                    { data: 'totales.bonoProduccionImporte', title: 'Bono de producción', className: 'text-right' },
                    { data: 'totales.despensaImporte', title: 'Despensa', className: 'text-right' },
                    { data: 'totales.previsionSocialImporte', title: 'Prev social', className: 'text-right' },
                    { data: 'totales.altoCostoVidaImporte', title: 'Alto costo vida', className: 'text-right' },
                    { data: 'totales.totalPercepciones', title: 'Total percepciones', className: 'text-right' },
                    { data: 'totales.isrImporte', title: 'I.S.R.', className: 'text-right' },
                    { data: 'totales.imssImporte', title: 'I.M.S.S.', className: 'text-right' },
                    { data: 'totales.cuotaSindicalImporte', title: 'Cuota sindical', className: 'text-right' },
                    { data: 'totales.infonavitImporte', title: 'INFONAVIT', className: 'text-right' },
                    { data: 'totales.fonacotImporte', title: 'FONACOT', className: 'text-right' },
                    { data: 'totales.pensionAlimenticiaImporte', title: 'Pensión alimenticia', className: 'text-right' },
                    { data: 'totales.vejezSarImporte', title: '1.125% C. y vejez SAR', className: 'text-right' },
                    { data: 'totales.descuentosAlimentosImporte', title: 'Descuentos alimentos', className: 'text-right' },
                    { data: 'totales.fondoAhorroEmpleado2Importe', title: 'F.A. empleado', className: 'text-right' },
                    { data: 'totales.fondoAhorroEmpresa3Importe', title: 'F.A. empresa', className: 'text-right' },
                    { data: 'totales.descuentosImporte', title: 'Descuentos', className: 'text-right' },
                    { data: 'totales.prestamosImporte', title: 'Prestamos', className: 'text-right' },
                    { data: 'totales.axaImporte', title: 'AXA', className: 'text-right' },
                    { data: 'totales.famsaImporte', title: 'FAMSA', className: 'text-right' },
                    { data: 'totales.totalDeducciones', title: 'Total deducciones', className: 'text-right' },
                    { data: 'totales.netoPagar', title: 'Neto a pagar', className: 'text-right' },
                    { data: 'totales.prevSocialGravableUnidades', title: 'Prev. social gravable unidades', className: 'text-right', visible: false },
                    { data: 'totales.prevSocialGravableImporte', title: 'Prev. social gravable', className: 'text-right' },
                    { data: 'totales.vacacionesUnidades', title: 'Vacaciones unidades', className: 'text-right', visible: false },
                    { data: 'totales.vacacionesImporte', title: 'Vacaciones', className: 'text-right' },
                    { data: 'totales.subsidioEmpleoImporte', title: 'Subsidio al empleado', className: 'text-right' },
                    { data: 'totales.parteExentaUnidades', title: 'Parte exenta horas extras unidades', className: 'text-right', visible: false },
                    { data: 'totales.parteExentaImporte', title: 'Parte exenta horas extras', className: 'text-right' },
                    { data: 'totales.netoPagar2Importe', title: 'Neto a pagar', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: [4, 6, 8, 11, 13, 14, 16, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 42, 44, 45, 47, 48],
                        render: function(data, row, type) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [0, 1],
                        searchable: true
                    },
                    {
                        targets: '_all',
                        searchable: false
                    }
                ],
                
                initComplete: function(settings, json) {
                    tblRayaTotales.on('click', 'tr', function() {
                        const rowData = tblRayaTotales.DataTable().row($(this)).data();

                        if (_idNomina != rowData.idNomina) {
                            getRayaDetalleCargada(rowData.idNomina);
                            _idNomina = rowData.idNomina;
                        }
                    });
                },

                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {
                    tblRayaTotales.DataTable().columns().every(function(colIdx, tableLoop, colLoop){
                        if (colIdx > 1) {
                            let total = 0;

                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            this.column(colIdx).visible(total != 0);
                        }
                    });

                    let total = 0;
                    let invisibles = 0;

                    tblRayaTotales.DataTable().columns().every(function(colIdx, tableLoop, colLoop){
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            if (this.column(colIdx).visible()) {
                                if (colIdx < 4) {
                                    $(this.footer()).html(total);
                                }
                                else {
                                    $(this.footer()).html(maskNumero(total));                                    
                                }
                            }
                            else {
                                invisibles++;
                            }
                            total = 0;
                        }
                    });
                },

                headerCallback: function(thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function(tfoot, data, start, end, display) {}
            });

            tblRaya.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                fixedColumns: {
                    leftColumns: 3
                },

                columns: [
                    { data: 'numeroEmpleado', title: 'Clave empleado', width: '50px', className: 'text-center' },
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'cc', title: 'C.C.' },
                    { data: 'propiedadesRaya.sueldoUnidades', title: 'Sueldo unidades', className: 'text-right' },
                    { data: 'propiedadesRaya.sueldoImporte', title: 'Sueldo importe', className: 'text-right' },
                    { data: 'propiedadesRaya.septimoDiaUnidades', title: 'Septimo día unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.septimoDiaImporte', title: 'Septimo día', className: 'text-right' },
                    { data: 'propiedadesRaya.horasExtraDoblesUnidades', title: 'Horas extras dobles unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.horasExtraDoblesImporte', title: 'Hrs extras dobles', className: 'text-right' },
                    { data: 'propiedadesRaya.faltaInjustificadaUnidades', title: 'Falta injustificada unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.premiosAsistenciaUnidades', title: 'Premios asistencia unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.premiosAsistenciaImporte', title: 'Premios asistencia', className: 'text-right' },
                    { data: 'propiedadesRaya.bonoPuntualidadUnidades', title: 'Bono puntualidad unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.bonoPuntualidadImporte', title: 'Bono puntualidad', className: 'text-right' },
                    { data: 'propiedadesRaya.fondoAhorroEmpresaImporte', title: 'F.A. empresa', className: 'text-right' },
                    { data: 'propiedadesRaya.primaVacacionalUnidades', title: 'Prima vacacional unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.primaVacacionalImporte', title: 'Prima vacacional', className: 'text-right' },
                    { data: 'propiedadesRaya.primaVacacional2Unidades', title: 'Prima vacacional unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.primaVacacional2Importe', title: 'Prima vacacional', className: 'text-right' },
                    { data: 'propiedadesRaya.bonoProduccionUnidades', title: 'Bono de producción unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.bonoProduccionImporte', title: 'Bono de producción', className: 'text-right' },
                    { data: 'propiedadesRaya.despensaImporte', title: 'Despensa', className: 'text-right' },
                    { data: 'propiedadesRaya.previsionSocialImporte', title: 'Prev. social', className: 'text-right' },
                    { data: 'propiedadesRaya.altoCostoVidaImporte', title: 'Alto costo vida', className: 'text-right' },
                    { data: 'propiedadesRaya.totalPercepciones', title: 'Total percepciones', className: 'text-right' },
                    { data: 'propiedadesRaya.isrImporte', title: 'I.S.R.', className: 'text-right' },
                    { data: 'propiedadesRaya.imssImporte', title: 'I.M.S.S.', className: 'text-right' },
                    { data: 'propiedadesRaya.cuotaSindicalImporte', title: 'Cuota sindical', className: 'text-right' },
                    { data: 'propiedadesRaya.infonavitImporte', title: 'INFONAVIT', className: 'text-right' },
                    { data: 'propiedadesRaya.fonacotImporte', title: 'FONACOT', className: 'text-right' },
                    { data: 'propiedadesRaya.pensionAlimenticiaImporte', title: 'Pensión alimenticia', className: 'text-right' },
                    { data: 'propiedadesRaya.vejezSarImporte', title: '1.125% C. y vejez SAR', className: 'text-right' },
                    { data: 'propiedadesRaya.descuentosAlimentosImporte', title: 'Descuentos alimentos', className: 'text-right' },
                    { data: 'propiedadesRaya.fondoAhorroEmpleado2Importe', title: 'F.A. empleado', className: 'text-right' },
                    { data: 'propiedadesRaya.fondoAhorroEmpresa3Importe', title: 'F.A. empresa', className: 'text-right' },
                    { data: 'propiedadesRaya.descuentosImporte', title: 'Descuentos ', className: 'text-right' },
                    { data: 'propiedadesRaya.prestamosImporte', title: 'Prestamos prestados', className: 'text-right' },
                    { data: 'propiedadesRaya.axaImporte', title: 'AXA', className: 'text-right' },
                    { data: 'propiedadesRaya.famsaImporte', title: 'FAMSA', className: 'text-right' },
                    { data: 'propiedadesRaya.totalDeducciones', title: 'Total deducciones', className: 'text-right' },
                    { data: 'propiedadesRaya.netoPagar', title: 'Neto a pagar', className: 'text-right' },
                    { data: 'propiedadesRaya.prevSocialGravableUnidades', title: 'Prev. social gravable unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.prevSocialGravableImporte', title: 'Prev. social gravable', className: 'text-right' },
                    { data: 'propiedadesRaya.vacacionesUnidades', title: 'Vacaciones unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.vacacionesImporte', title: 'Vacaciones', className: 'text-right' },
                    { data: 'propiedadesRaya.subsidioEmpleoImporte', title: 'Subsidio al empleado', className: 'text-right' },
                    { data: 'propiedadesRaya.parteExentaUnidades', title: 'Parte exenta horas extras unidades', className: 'text-right', visible: false },
                    { data: 'propiedadesRaya.parteExentaImporte', title: 'Parte exenta horas extras', className: 'text-right' },
                    { data: 'propiedadesRaya.netoPagar2Importe', title: 'Neto a pagar', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: [4, 6, 8, 11, 13, 14, 16, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 42, 44, 45, 47, 48],
                        render: function(data, row, type) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [0, 1, 2],
                        searchable: true
                    },
                    {
                        targets: '_all',
                        searchable: false
                    }
                ],
                
                initComplete: function(settings, json) {},
                createdRow: function(row, data, dataIndex) {},

                drawCallback: function(settings) {
                    tblRaya.DataTable().columns().every(function(colIdx, tableLoop, colLoop){
                        if (colIdx > 2) {
                            let total = 0;

                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            this.column(colIdx).visible(total != 0);
                        }
                    });
                },

                headerCallback: function(thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },

                footerCallback: function(tfoot, data, start, end, display) {}
            });
        }
        //#endregion

        //#region backend
        function getRayaDetalleCargada(nominaId){
            $.get('/Nomina/GetRayaDetalleCargada',
            {
                nominaId
            }).then(response => {
                if (response.success) {
                    addRows(tblRaya, response.items);
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function getRayaCargada(periodo, tipoPeriodo, year, tipoRaya, clasificacionCC) {
            $.get('/Nomina/GetRayaCargada',
            {
                periodo,
                tipoPeriodo,
                year,
                tipoRaya,
                clasificacionCC
            }).then(response => {
                if (response.success) {
                    addRows(tblRayaTotales, response.items);
                } else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function cargarRaya(objeto) {
            $.ajax({
                type: 'POST',
                url: '/Nomina/CargarRaya',
                data: objeto,
                contentType: false,
                processData: false,
                cache: false,
                success: function(response) {
                    if (response.success) {
                        addRows(tblRayaTotales, response.items);
                        swal('Confirmación!', 'Se guardó correctamente la raya', 'success');
                    }
                    else {
                        swal('Alerta!', `Ocurrio un error al cargar la raya: ${ response.message }`, 'warning');
                    }
                },
                error: function(xhr, status, error) {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                }
            })
        }

        //#region dropzone
        // function initDropZone() {
        //     dzRaya.dropzone({
        //         url: '/Nomina/CargarRaya',
        //         paramName: 'raya',
        //         maxFiles: 1,
        //         addRemoveLinks: false,
        //         autoProcessQueue: false,
        //         init: function() {
        //             _dzRaya = this;

        //             this.on('sending', function(file, xhr, formData) {
        //                 $.blockUI({
        //                     message: 'Procesando...'
        //                 });

        //                 formData.append('periodo', _periodo);
        //                 formData.append('tipoPeriodo', _tipoPeriodo);
        //                 formData.append('year', _year);
        //                 formData.append('tipoRaya', cboxTipoRayaFiltro.val());
        //             });

        //             this.on('complete', function() {
        //                 this.removeAllFiles( true );
        //                 $.unblockUI();
        //             });

        //             this.on('success', function(file, response) {
        //                 if (response.success) {
        //                     addRows(tblRayaTotales, response.items);
        //                     swal('Confirmación!', 'Se guardó correctamente la raya', 'success');
        //                 }
        //                 else {
        //                     swal('Alerta!', `Ocurrio un error al cargar la raya: ${ response.message }`, 'warning');
        //                 }
        //             });

        //             this.on('error', function(file, errorMessage, xhrObject){
                        
        //             });
        //         }
        //     });
        // }
        //#endregion

        function llenarCombos() {
            cboxPeriodoFiltro.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, null, () => {
                cboxPeriodoFiltro.prop('selectedIndex', 0);
                cboxPeriodoFiltro.trigger('change');
            });

            cboxTipoRayaFiltro.fillCombo('/Nomina/GetTipoRaya', null, false, null, () => {
                cboxTipoRayaFiltro.val(1);
                cboxTipoRayaFiltro.trigger('change');
            });
            cboxClasificacionCCFiltro.fillCombo('/Nomina/GetClasificacionCC', null, false, null, null);
        }
        //#endregion

        (function init() {
            moment.locale('es');

            initTbls();
            llenarCombos();
            cboTipoNomina.change (function (e) {
                cboxPeriodoFiltro.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, null, () => {
                    cboxPeriodoFiltro.prop('selectedIndex', 0);
                cboxPeriodoFiltro.trigger('change');
            });
            });
            //initDropZone();
        })();
    }
    $(document).ready(() => Administrativo.Contabilidad.Raya = new Raya())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();