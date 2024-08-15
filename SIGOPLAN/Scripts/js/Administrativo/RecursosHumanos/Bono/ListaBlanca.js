(() => {
    $.namespace('Administrativo.RecursosHumanos.ListaBlanca');
    ListaBlanca = function (){
            let itemsPorcentaje;
            const txtCve = $('#txtCve');
            const txtNombre = $('#txtNombre');
            const txtPuesto = $('#txtPuesto');
            let tblData = $('#tblData');
            const btnGuardar = $('#btnGuardar');
    
            let dtTabla;
    
            const empleado = {
                cve_Emp: 0,
                nombre_Emp: '',
                puesto_Emp: 0,
                puestoCve_Emp: '',
                cc: '',
            }
    
            btnGuardar.on('click', function(){
                let newEmpleadoLN = Object.create(empleado);
                newEmpleadoLN.cve_Emp = txtNombre.data('cve');
                newEmpleadoLN.nombre_Emp = txtNombre.val();
                newEmpleadoLN.puesto_Emp = txtNombre.data('cve_puesto');
                newEmpleadoLN.puestoCve_Emp = txtPuesto.val();
                newEmpleadoLN.cc = txtNombre.data('cc');
    
                guardarEmpleado(newEmpleadoLN);
            });
    
            let init = () => {
                initForm();
            }
            const getItemPorcentaje = new URL(window.location.origin + '/Administrativo/Bono/setItemPorcentaje');
            async function setItemPorcentaje() {
                try {
                    response = await ejectFetchJson(getItemPorcentaje);
                    if (response.success) {
                        itemsPorcentaje = response.items;
                    }
                } catch (o_O) { }
            }
    
            function initDataTblBono() {
                dtTabla = tblData.DataTable({
                    destroy: true,
                    language: dtDicEsp,
                    dom: 'Bfrtip',
                    buttons: parametrosImpresion("Lista Blanca", "<center><h3>Lista blanca para bono desempeño mensual</h3></center>"),
                    buttons: [
                        {
                            extend: 'excelHtml5', footer: true,
                            exportOptions: {
                                // columns: [':visible', 21]
                                columns: [0, 1, 2]
                            }
                        }
                    ],
                    columns: [
                        { title:'CVE' ,data: 'cve', className: 'text-center', width: '10%'},
                        { title:'NOMBRE' ,data: 'nombre'},
                        { title:'CC' ,data: 'cc', className: 'text-center', width: '10%'},
                        {
                            title: "ELIMINAR" ,data: null ,width:"5%" , className: 'text-center', createdCell: function (td, data, rowData, row, col) {
                                let btn = $("<button data-cve='"+rowData.cve+"'>")
                                   ,ico = $('<i class="far fa-trash-alt"></i>');
                                btn.addClass('btn btn-danger eliminar');
                                ico.addClass("fa fa-remove");
                                btn.html(ico);
                                $(td).html(btn);
                            }
                        }
                    ]
                    ,initComplete: function (settings, json) {
                        tblData.on(`click` ,`.eliminar` , function () {
                            var cve=$(this).data("cve");
                            eliminarDeListaBlanca(cve, $(this).closest('tr'));
                        });
                    }
                });
            }
    
            function addRow(tbl, lst) {
                dt = tbl.DataTable();
                dt.row.add(lst).draw();
            }
    
            function addRows(tbl, lst) {
                dt = tbl.DataTable();
                dt.clear().draw();
                dt.rows.add(lst).draw();
            }
    
            function SelectEmpleado(event, ui) {
                txtNombre.val(ui.item.value);
                txtNombre.data('cve', ui.item.id);
                txtCve.val(ui.item.id);
                SetInfoEmpleado(ui.item.id)
            }
            function SetInfoEmpleado(idEmplado) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/ResguardoEquipo/GetSingleUsuario',
                    data: { id: idEmplado },
                    async: false,
                    success: function (response) {
                        if (response.success) {
                            txtPuesto.val(response.Puesto.toLowerCase());
                            txtNombre.data('cc', response.CCEmpleado);
                            txtNombre.data('cve_puesto', response.CvePuesto);
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
    
            function guardarEmpleado(empleado) {
                $.post('/Administrativo/Bono/GuardarEnListaBlanca',
                {
                    empleado
                }).then(response => {
                    if (response.Success) {
                        let datosEmpleado = {
                            cve: empleado.cve_Emp,
                            nombre: empleado.nombre_Emp,
                            cc: empleado.cc
                        }
                        
                        addRow(tblData, datosEmpleado);
    
                        txtNombre.val('');
                        txtNombre.data('cve', null);
                        txtNombre.data('cve_puest', null);
                        txtNombre.data('cc', null);
                        txtCve.val('');
                        txtPuesto.val('');
                    } else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
    
            function empleadosListaBlanca(){
                $.get('/Administrativo/Bono/EmpleadosListaBlanca').then(response => {
                    if (response.Success) {
                        addRows(tblData, response.Value);
                    } else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
    
            function eliminarDeListaBlanca(claveEmpleado, row) {
                $.post('/Administrativo/Bono/EliminarDeListaBlanca',
                {
                    claveEmpleado
                }).then(response => {
                    if (response.Success) {
                        dtTabla.row(row).remove().draw('false');
                    } else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
    
            function initForm () {
                txtNombre.getAutocomplete(SelectEmpleado, null, '/OT/getEmpleados');
                initDataTblBono();
                empleadosListaBlanca();
            }
            init();
        }
        $(document).ready(() => {
            Administrativo.RecursosHumanos.ListaBlanca = new ListaBlanca();
        })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
    })();