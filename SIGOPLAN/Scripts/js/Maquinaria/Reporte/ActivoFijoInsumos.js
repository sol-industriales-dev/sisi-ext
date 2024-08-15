(() => {
    $.namespace('Maquinaria.ActivoFijo.Insumos');
    Insumos = function() {
        //Objetos
        const tblDepInsumos = $('#tblDepInsumos');
        const btnActualizarInsumos = $('#btnActualizarInsumos');
        const btnMdlNuevoInsumo = $('#btnMdlNuevoInsumo');
        const btnCargaAutomatica = $('#btnCargaAutomatica');

        const mdlNuevoInsumo = $('#mdlNuevoInsumo');
        const txtNuevoInsumo = $('#txtNuevoInsumo');
        const txtNuevoDescripcion = $('#txtNuevoDescripcion');
        const txtNuevoPorcentaje = $('#txtNuevoPorcentaje');
        const txtNuevoMeses = $('#txtNuevoMeses');
        const btnGuardarInsumo = $('#btnGuardarInsumo');

        var dtInsumos;

        //Eventos
        // btnModificarDepCuentas.on('click', function() {
        //     var cuentas = $(this).data('cuentas');
        //     objDepCuentas = new Array();
        //     for (let index = 0; index < cuentas; index++) {
        //         objDepCuenta = {
        //             Id: $('#idDepCuenta' + index).data('id'),
        //             Cuenta: '',
        //             Descripcion: '',
        //             PorcentajeDepreciacion: $('#txtPorcentajeDepreciacion' + $('#idDepCuenta' + index).data('id')).val(),
        //             MesesDeDepreciacion: $('#txtMesesDepreciacion' + $('#idDepCuenta' + index).data('id')).val()
        //         }
        //         objDepCuentas.push(objDepCuenta);
        //     }
        //     ModificarDepreciacionCuenta(objDepCuentas);
        // });

        // $(document).on('keypress', '.txtPorcentajeDepreciacion', function(event){
        //     aceptaSoloNumeroXDIntMax($(this), event, 2);
        // });

        // $(document).on('paste', '.txtPorcentajeDepreciacion', function(event){
        //     permitePegarSoloNumeroXDIntMax($(this), event, 2);
        // });

        // $(document).on('keypress', '.txtMesesDepreciacion', function(event){
        //     aceptaSoloNumeroXDIntMax($(this), event, 3);
        // });

        // $(document).on('paste', '.txtMesesDepreciacion', function(event){
        //     permitePegarSoloNumeroXDIntMax($(this), event, 3);
        // });

        btnActualizarInsumos.on('click', function(){
            updateInsumos(armarInsumos());
        });

        btnMdlNuevoInsumo.on('click', function() {
            mdlNuevoInsumo.modal('show');
        });

        btnCargaAutomatica.on('click', function() {
            AgregarInsumosAutomaticamente();
        })

        btnGuardarInsumo.on('click', function() {
            addInsumo(crearInsumoNuevo());
        })

        function crearInsumoNuevo() {
            var objInsumo = {
                Insumo: txtNuevoInsumo.val().trim(),
                Descripcion: txtNuevoDescripcion.val().trim(),
                Porcentaje: txtNuevoPorcentaje.val() / 100,
                Meses: txtNuevoMeses.val()
            }

            return objInsumo;
        }

        function armarInsumos() {
            var objInsumos = new Array();

            var datosDT = $('#tblDepInsumos').DataTable().rows().data();

            datosDT.each(function(element, index) {
                var objInsumo = {
                    Id: element.id,
                    Insumo: element.insumo.trim(),
                    Descripcion: element.descripcion.trim(),
                    Porcentaje: element.porcentaje / 100,
                    Meses: element.meses,
                    Estatus: element.estatus
                }

                objInsumos.push(objInsumo);
            });

            return objInsumos;
        }

        function initTableInsumos() {
            dtInsumos = tblDepInsumos.DataTable({
                order: [[0, "asc"]],
                searching: true,
                paging: false,
                ordering: true,
                info: false,
                language: dtDicEsp,
                scrollY: "400px",
                scrollCollapse: true,
                columns: [
                    { data: 'id', visible: false },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'porcentaje', title: 'Porcentaje Dep' },
                    { data: 'meses', title: 'Meses a Dep' },
                    { data: 'estatus', title: 'Estatus' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap text-center'
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            if (type == 'display') {
                                var html = '<input type="text" class="txtInsumo form-control" value="' + data + '" />';
                                return html;
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            if (type == 'display') {
                                var html = '<input type="text" class="txtDescripcion form-control" value="' + data + '" />';
                                return html;
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            var html = '<input type="text" class="txtPorcentaje form-control" value="' + (data != null ? data : "") + '" placeholder="%" />';
                            return html;
                        }
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            var html = '<input type="text" class="txtMeses form-control" value="' + data + '" />';
                            return html;
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            var html = '<input type="checkbox" class="cboxEstatus form-control" ' + (data ? "checked" : "") + ' />';
                            return html;
                        }
                    }
                ],
                initComplete: function () {
                    tblDepInsumos.on('change', '.txtInsumo', function() {
                        dtInsumos.row($(this).closest('tr')).data().insumo = $(this).val();
                    });
                    tblDepInsumos.on('change', '.txtDescripcion', function() {
                        dtInsumos.row($(this).closest('tr')).data().descripcion = $(this).val();
                    });
                    tblDepInsumos.on('change', '.txtPorcentaje', function() {
                        dtInsumos.row($(this).closest('tr')).data().porcentaje = $(this).val();
                    });
                    tblDepInsumos.on('change', '.txtMeses', function() {
                        dtInsumos.row($(this).closest('tr')).data().meses = $(this).val();
                    });
                    tblDepInsumos.on('change', '.cboxEstatus', function() {
                        dtInsumos.row($(this).closest('tr')).data().estatus = $(this).is(':checked');
                    });
                },
                createdRow: function (row, data, dataIndex) {
                },
                drawCallback: function (settings) {
                },
                headerCallback: function (thead, data, start, end, display) {
                },
                footerCallback: function (tfoot, data, start, end, display) {
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function AddRow(tbl, data) {
            dt = tbl.DataTable();
            dt.row.add(data).draw();
        }

        let init = () => {
            initTableInsumos();
            getInsumos();
        }

        //Inits

        //LLamadas al servidor
        function getInsumos() {
            $.get('/ActivoFijo/GetInsumos', {
            }).always().then(response => {
                if (response.Success) {
                    AddRows(tblDepInsumos, response.Value);
                }
                else {
                    alert('EROR: ' + response.Message)
                }
            }, error => {
                alert('SERVER ERROR: ' + error.statusText);
            });
        }

        function AgregarInsumosAutomaticamente() {
            $.post('/ActivoFijo/AgregarInsumosAutomaticamente', {  }).then(response => {
                if (response.Success) {
                    AddRows(tblDepInsumos, response.Value);
                } else {
                    AlertaGeneral(`Alerta`, response.Message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function updateInsumos(objInsumos) {
            $.post('/ActivoFijo/UpdateInsumos', {
                insumos: objInsumos
            }).always().then(response => {
                if (response.Success) {
                    ConfirmacionGeneral('Confirmación', '¡Se actualizaron los datos!');
                }
                else {
                    alert('ERROR: ' + response.Message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + error.statusText);
            });
        }

        function addInsumo(objInsumo) {
            $.post('/ActivoFijo/AddInsumo', {
                insumo: objInsumo
            }).then(response => {
                if (response.Success) {
                    mdlNuevoInsumo.modal('hide');
                    getInsumos();
                }
                else {
                    alert('ERROR: ', response.Message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + error.statusText);
            });
        }

        init();
    }

    $(document).ready(() => {
        Maquinaria.ActivoFijo.Insumos = new Insumos();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();