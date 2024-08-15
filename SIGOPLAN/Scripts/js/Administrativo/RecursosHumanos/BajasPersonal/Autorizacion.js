(() => {
    $.namespace('BajaPersonal.Autorizacion');
    Autorizacion = function () {
        //#region Selectores
        const selectCC = $('#selectCC');
        const botonBuscar = $('#botonBuscar');
        const botonGuardar = $('#botonGuardar');
        const tablaRegistros = $('#tablaRegistros');
        const mdlComentario = $('#mdlComentario');
        const txtMdlComentario = $('#txtMdlComentario');
        //#endregion

        let dtRegistros;
        const empresaActual = $('#empresaActual');
        _empresaActual = empresaActual.val();

        (function init() {
            initTablaRegistros();

            //#region CONTROL EMPRESAS

            if (_empresaActual == "6" || _empresaActual == "3") {
                dtRegistros.column(7).visible(false); //
                dtRegistros.column(8).visible(false); //
            }

            //#endregion

            selectCC.fillCombo('/BajasPersonal/FillCboCCByBajasPermiso', null, false, 'Todos');
            // selectCC.fillCombo('/Reclutamientos/FillCboCC', null, false, 'Todos');
            convertToMultiselect('#selectCC');

            botonBuscar.click(buscarRegistros);
            // botonGuardar.click(guardarAutorizacion);
        })();

        function initTablaRegistros() {
            dtRegistros = tablaRegistros.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tablaRegistros.on('click', '.radioBtn a', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('autorizada');

                        div.find(`a[data-toggle="radioAutorizada${rowData.id}"]`).not(`[autorizada="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioAutorizada${rowData.id}"][autorizada="${seleccion}"]`).removeClass('notActive').addClass('active');
                    });

                    tablaRegistros.on('click', '.autorizarBaja', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();

                        let fechaBaja = moment(rowData.fechaBaja).format("DD/MM/YYYY");

                        if (rowData.esAnticipada) {
                            Swal.fire({
                                position: "center",
                                icon: "warning",
                                title: "Autorizar Bajas Anticipadas",
                                input: 'textarea',
                                width: '50%',
                                showCancelButton: true,
                                html: "<h3>Esta baja anticipada sera aplicada el dia: " + fechaBaja + "<br>¿Desea autorizar la baja seleccionada?<br>Indicar el motivo:</h3>",
                                confirmButtonText: "Aceptar",
                                confirmButtonColor: "#5cb85c",
                                cancelButtonText: "Cancelar",
                                cancelButtonColor: "#5c636a",
                                showCloseButton: true
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    guardarAutorizacion(rowData.id, "1", $('.swal2-textarea').val());
                                }
                            });
                        } else {
                            Swal.fire({
                                position: "center",
                                icon: "warning",
                                title: "Autorizar Bajas",
                                input: 'textarea',
                                width: '50%',
                                showCancelButton: true,
                                html: "<h3>¿Desea autorizar la baja seleccionada?<br>Indicar el motivo:</h3>",
                                confirmButtonText: "Aceptar",
                                confirmButtonColor: "#5cb85c",
                                cancelButtonText: "Cancelar",
                                cancelButtonColor: "#5c636a",
                                showCloseButton: true
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    guardarAutorizacion(rowData.id, "1", $('.swal2-textarea').val());
                                }
                            });
                        }
                    });

                    tablaRegistros.on('click', '.rechazarBaja', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar Bajas",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar la baja seleccionada?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#d9534f",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                guardarAutorizacion(rowData.id, "2", $('.swal2-textarea').val());
                            }
                        });
                    });

                    tablaRegistros.on('click', '.verComentario', function () {
                        let rowData = dtRegistros.row($(this).closest('tr')).data();
                        txtMdlComentario.text(rowData.comentariosAutorizacion);
                        mdlComentario.modal("show");
                    });
                },
                columns: [
                    { data: 'numeroEmpleado', title: '#' },
                    { data: 'nombre', title: 'Nombre' },
                    {
                        title: 'CC', render: function (data, type, row, meta) {
                            return '[' + row.cc + '] ' + row.descripcionCC;
                        }
                    },
                    { data: 'usuarioCreacion_Nombre', title: 'Capturo' },
                    { data: 'telPersonal', title: 'Celular' },
                    { data: 'motivoBajaDeSistema', title: 'Motivo baja' },
                    {
                        data: 'curp', title: _empresaActual == 6 ? 'DNI' : _empresaActual == 3 ? 'CEDULA CIUD.' : 'CURP', render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return row.dni;
                            } else {
                                if (_empresaActual == 3) {

                                    return row.cedula_ciudadania;
                                } else {

                                    return data;
                                }
                            }
                        }
                    },
                    { data: 'rfc', title: 'RFC' },
                    { data: 'nss', title: 'NSS' },
                    {
                        data: 'fechaIngreso', title: 'FECHA ALTA',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'fechaBaja', title: 'Fecha baja',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: "esContratable", title: "Contratable", visible: true,
                        render: function (data, type, row) {
                            if (data) {
                                return "Si";
                            } else {
                                return "No";
                            }
                        }
                    },
                    {
                        data: "esAnticipada", title: "Anticipada", visible: true,
                        render: function (data, type, row) {
                            if (data) {
                                return "Si";
                            } else {
                                return "No";
                            }
                        }
                    },
                    {
                        data: 'est_baja', title: 'Baja', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='1'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            // return `
                            //     <div class="radioBtn btn-group">
                            //         <a class="btn btn-success notActive" data-toggle="radioAutorizada${row.id}" autorizada="1"><i class="fa fa-check"></i>&nbsp;AUTORIZAR</a>
                            //         <a class="btn btn-danger notActive" data-toggle="radioAutorizada${row.id}" autorizada="2"><i class="fa fa-times"></i>&nbsp;RECHAZAR</a>
                            //         <a class="btn btn-primary active" data-toggle="radioAutorizada${row.id}" autorizada="0"><i class="fa fa-square"></i>&nbsp;PENDIENTE</a>
                            //     </div>`;
                            if (row.autorizada == 0) {

                                // if (row.esAnticipada) {
                                //     if (row.est_inventario == 'A' && row.est_contabilidad == 'A' && row.est_compras == 'A') {
                                //         return `
                                //             <button title="Autorizar" class="btn btn-xs btn-success autorizarBaja"><i class="fa fa-check"></i></button>&nbsp
                                //             <button title="Rechazar" class="btn btn-xs btn-danger rechazarBaja"><i class="fa fa-times"></i></button>
                                //         `;
                                //     } else {
                                //         return `
                                //             <button title="Ver comentario" class="btn btn-xs btn-primary verComentario"><i class="far fa-comments"></i></button>
                                //         `;
                                //     }
                                // } else {
                                //     return `
                                //         <button title="Autorizar" class="btn btn-xs btn-success autorizarBaja"><i class="fa fa-check"></i></button>&nbsp
                                //         <button title="Rechazar" class="btn btn-xs btn-danger rechazarBaja"><i class="fa fa-times"></i></button>
                                //     `;
                                // }

                                return `
                                    <button title="Autorizar" class="btn btn-xs btn-success autorizarBaja"><i class="fa fa-check"></i></button>&nbsp
                                    <button title="Rechazar" class="btn btn-xs btn-danger rechazarBaja"><i class="fa fa-times"></i></button>
                                `;

                            } else {
                                return `
                                    <button title="Ver comentario" class="btn btn-xs btn-primary verComentario"><i class="far fa-comments"></i></button>
                                `;
                            }

                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { width: '20%', targets: 14 }
                ]
            });
        }

        function buscarRegistros() {
            axios.post('GetBajasPersonalAutorizacion', { listaCC: getValoresMultiples('#selectCC') })
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        AddRows(tablaRegistros, response.data.lstBajas);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarAutorizacion(idBaja, estado, comentario) {
            let listaAutorizacion = [];

            // tablaRegistros.find('tbody tr').each(function (index, row) {
            //     let rowData = dtRegistros.row(row).data();
            //     let autorizada = +($(row).find(`.radioBtn a.active[data-toggle=radioAutorizada${rowData.id}]`).attr('autorizada'));

            //     if (autorizada > 0) {
            //         listaAutorizacion.push({
            //             baja_id: rowData.id,
            //             autorizada
            //         });
            //     }
            // });

            let obj = {
                baja_id: idBaja,
                autorizada: estado,
                comentariosAutorizacion: comentario,
            }

            axios.post('GuardarAutorizacionBajas', obj)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        buscarRegistros();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => BajaPersonal.Autorizacion = new Autorizacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();