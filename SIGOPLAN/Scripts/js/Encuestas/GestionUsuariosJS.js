
(function () {
    $.namespace('encuestas.GestionUsuarios');

    GestionUsuarios = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Gestion de Usuarios',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        idComboFiltro = $("#idComboFiltro"),
            modalAsignacionPermisos = $("#modalAsignacionPermisos");
        modalPrintRpt = $("#modalPrintRpt"),
        btnCargar = $("#btnCargar");
        btnImprimir = $("#btnImprimir"),
        btnViewRpt = $("#btnViewRpt"),
        btnConsultar = $("#btnConsultar"),
        cboEncuestas = $("#cboEncuestas"),

        cboUsuarios = $("#cboUsuarios");

        const tblUsuarios = $('#tblUsuarios');
        const tblPermisos = $('#tblPermisos');
        const btnGuardarPermisos = $('#btnGuardarPermisos');
        const tblPermisosReporte = $('#tblPermisosReporte');



        function init() {
            initTablaUsuarios();
            initTablaPermisos();
            initTablaPermisosrpt();
            btnCargar.click(cargarUsuarios);
            btnGuardarPermisos.click(guardarPermisos);
            loadCboEncuestasRpt();
            btnViewRpt.click(viewRptUsuario);
            btnConsultar.click(cargarRptPermisos);
            btnViewRpt.prop('disabled',true);
        }

        const getUsuarios = () => $.post('/Encuestas/Encuesta/getUsuariosEncuestas', { tipoCliente: idComboFiltro.val() });
        const getPermisos = (idUsuario) => $.post('/Encuestas/Encuesta/getPermisos', { idUsuario: idUsuario });
        const guardarPermisosCheck = (lstPermisos) => $.post('/Encuestas/Encuesta/GuardarPermisosCheck', { lstPermisos: lstPermisos });
        const getRptPermisos = () => $.post('/Encuestas/Encuesta/getRptPermisosUsuarios', { encuestasID: getValoresMultiples("#cboEncuestas") ,usuariosID : getValoresMultiples("#cboUsuarios")});

        modalAsignacionPermisos.on('shown.bs.modal', function () {
            tblPermisos.DataTable().columns.adjust();
        });

        function viewRptUsuario() {
            modalPrintRpt.modal('show');
            tblPermisosReporte.DataTable().clear();
            tblPermisosReporte.draw();
            
        }

        function guardarPermisos() {
            let lstPermisos = [];

            tblPermisos.find('tbody tr').each(function (idx, row) {
                let rowData = tblPermisos.DataTable().row($(this)).data();

                rowData.consultar = $(this).find('.btn-consultar').attr('data-check') == 'true' ? true : false;
                rowData.editar = $(this).find('.btn-editar').attr('data-check') == 'true' ? true : false;
                rowData.enviar = $(this).find('.btn-enviar').attr('data-check') == 'true' ? true : false;
                rowData.contestaTelefonica = $(this).find('.btn-telefonica').attr('data-check') == 'true' ? true : false;
                rowData.recibeNotificacion = $(this).find('.btn-notificacion').attr('data-check') == 'true' ? true : false;
                rowData.contestaPapel = $(this).find('.btn-papel').attr('data-check') == 'true' ? true : false;

                lstPermisos.push(rowData);
            });


            guardarPermisosCheck(lstPermisos).done(function (response) {
                if (response.success) {
                    AlertaGeneral('Alerta', 'Se ha guardado la información.');
                } else {
                    AlertaGeneral('Alerta', 'Error al guardar la información.');
                }
                modalAsignacionPermisos.modal('hide');
            });
        }

        function cargarUsuarios() {
            getUsuarios().done(response => {
                if (response.success) {
                cboUsuarios.fillCombo('/Encuestas/Encuesta/fillCboClienteInterno', null, false, "Todos");
            convertToMultiselect("#cboUsuarios");
            AddRows(tblUsuarios, response.dataSet);

            if(idComboFiltro.val()!="2")
            {
                btnViewRpt.prop('disabled',false);
            }
            else{
                btnViewRpt.prop('disabled',true);
            }
                    
        }
    });
}

function cargarPermisos(idUsuario) {
    getPermisos(idUsuario).done(response => {
        if (response.success) {
            modalAsignacionPermisos.modal('show');
    AddRows(tblPermisos, response.dataSet);
             
} else {
                         AlertaGeneral('Alerta', 'Error al consultar la información.');
}
});
}

function cargarRptPermisos() {
    getRptPermisos().done(response => {
        if (response.success) {
                AddRows(tblPermisosReporte, response.dataSet);

} else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
}
});
}


function loadCboEncuestasRpt() {
    $.ajax({
        datatype: "json",
        type: "POST",
        // url: "/Encuestas/Encuesta/FillEncuestasByDepto",
        url: "/Encuestas/Encuesta/FillEncuestasPorPermisosCheck",
        success: function (response) {
            var o = response.items;
            var g = response.Group;

            if (g.length > 1) {
                var todos = '<option value="todos">Todos</option>';
                cboEncuestas.append(todos);
                $.each(g, function (j, f) {
                    var html = "";
                    html += '<optgroup label="' + f.Text + '">';
                    $.each(o, function (i, e) {
                        if (f.Value == e.deptId) {
                            html += '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                        }
                    });
                    html += '</optgroup>';
                    var count = $("#cboEncuestas optgroup[label='" + f.Text + "']");
                    if (count.length == 0)
                        cboEncuestas.append(html);
                });
            }
            else {
                $.each(o, function (i, e) {
                    var html = "";
                    html = '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                    cboEncuestas.append(html);
                });
            }
            convertToMultiselect('#cboEncuestas');
        },
        error: function (error) { }
    });
}
function initTablaUsuarios() {
    tblUsuarios.DataTable({
        destroy: true,
        scrollCollapse: true,
        bFilter: true,
        paging: false,
        info: false,
        scrollY: '50vh',
        dom: 'Bfrtip',
        buttons: parametrosImpresion("Reporte de Permisos", "<center><h3>Reporte de Permisos </h3></center>"),
        buttons: [
                       {
                           extend: 'excel',
                           exportOptions: {
                               columns: [ 0, 1, 2, 3, 5 ] //Your Colume value those you want
                           }
                       },
        ],
        initComplete: function (settings, json) {
            tblUsuarios.on('click', '.btn-permisos', function () {
                let rowData = tblUsuarios.DataTable().row($(this).closest('tr')).data();

                cargarPermisos(rowData.id);
            });

            tblUsuarios.on('click', '.btn-crearEncuesta', function () {
                var empID= $(this).data("id");
                var crearID = 2080;
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-enviar-crearEncuesta btn-success');
                    $(this).addClass('btn-crearEncuesta-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Encuestas/Encuesta/setCrearEncuesta",
                        data: { empID: empID, crearID:crearID,crear:false},
                        asyn: false,
                        success: function (response) {
                        },
                        error: function () {
                        }
                    });
                } else {
                    $(this).removeClass('btn-crearEncuesta-red');
                    $(this).addClass('btn-crearEncuesta-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Encuestas/Encuesta/setCrearEncuesta",
                        data: { empID: empID, crearID:crearID,crear:true},
                        asyn: false,
                        success: function (response) {
                        },
                        error: function () {
                        }
                    });
                }
            });
        },
        columns: [
            { data: "Nombre", title: "Nombre" },
            { data: "Cliente" },
            { data: "Empresa", title: "Empresa" },
            { data: "CC", title: "Obra" },
            {
                data: "accion", createdCell: function (td, cellData, rowData, row, col) {

                    $(td).text('');

                    if (!rowData.cliente) {
                        $(td).append('<button type="button" class="btn btn-default btn-block btn-sm btn-permisos">Permisos</button>');
                    }
                }
            },
            {
                data: "crearEncuesta", createdCell: function (td, cellData, rowData, row, col) {

                    $(td).text('');

                    if (rowData.crearEncuesta) {
                        $(td).append('<button data-id="' + rowData.id + '" class="btn-crearEncuesta btn-crearEncuesta-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true">Sí</button>');
                    } else {
                        $(td).append('<button data-id="' + rowData.id + '" class="btn-crearEncuesta btn-crearEncuesta-red btn btn-sm" type="button" value="' + row.id + '" data-check="false">No</button>');
                    }
                    
                }
            }
        ],
        columnDefs: [
            { className: "dt-center", targets: "_all" }
        ]
    });
}

function initTablaPermisos() {
    tblPermisos.DataTable({
        retrieve: true,
        paging: false,
        searching: false,
        language: dtDicEsp,
        scrollCollapse: true,
        bFilter: false,
        info: false,
        scrollY: '50vh',
        initComplete: function (settings, json) {
            tblPermisos.on('click', '.btn-consultar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-consultar-green btn-success');
                    $(this).addClass('btn-consultar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-consultar-red');
                    $(this).addClass('btn-consultar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisos.on('click', '.btn-editar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-editar-green btn-success');
                    $(this).addClass('btn-editar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-editar-red');
                    $(this).addClass('btn-editar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisos.on('click', '.btn-enviar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-enviar-green btn-success');
                    $(this).addClass('btn-enviar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-enviar-red');
                    $(this).addClass('btn-enviar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisos.on('click', '.btn-telefonica', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-telefonica-green btn-success');
                    $(this).addClass('btn-telefonica-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-telefonica-red');
                    $(this).addClass('btn-telefonica-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisos.on('click', '.btn-notificacion', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-notificacion-green btn-success');
                    $(this).addClass('btn-notificacion-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-notificacion-red');
                    $(this).addClass('btn-notificacion-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisos.on('click', '.btn-papel', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-papel-green btn-success');
                    $(this).addClass('btn-papel-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-papel-red');
                    $(this).addClass('btn-papel-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });
        },
        destroy: true,
        scrollCollapse: true,
        columns: [
            { data: 'titulo', title: "Encuesta" },
            { data: 'descripcion', title: "Descripción" },
            { data: 'departamentoDesc', title: "Departamento" },
            {
                data: 'soloLectura', title: "Consultar", render: function (data, type, row, meta) {
                    if (row.consultar) {
                        return '<button class="btn-consultar btn-consultar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-consultar btn-consultar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'soloLectura', title: "Editar", render: function (data, type, row, meta) {
                    if (row.editar) {
                        return '<button class="btn-editar btn-editar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-editar btn-editar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'soloLectura', title: "Enviar", render: function (data, type, row, meta) {
                    if (row.enviar) {
                        return '<button class="btn-enviar btn-enviar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-enviar btn-enviar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'contestaTelefonica', title: "Telefónica", render: function (data, type, row, meta) {
                    if (row.contestaTelefonica) {
                        return '<button class="btn-telefonica btn-telefonica-green btn btn-sm btn-success" type="button" value="' + row.id + '" ' +
                            (row.soloLectura || !row.encuestaTelefonica ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-telefonica btn-telefonica-red btn btn-sm" type="button" value="' + row.id + '" ' +
                            (row.soloLectura || !row.encuestaTelefonica ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'recibeNotificacion', title: "Notificación", render: function (data, type, row, meta) {
                    if (row.recibeNotificacion) {
                        return '<button class="btn-notificacion btn-notificacion-green btn btn-sm btn-success" type="button" value="' + row.id + '" ' +
                            (!row.encuestaNotificacion ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-notificacion btn-notificacion-red btn btn-sm" type="button" value="' + row.id + '" ' +
                            (!row.encuestaNotificacion ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'contestaPapel', title: "Papel", render: function (data, type, row, meta) {
                    if (row.contestaPapel) {
                        return '<button class="btn-papel btn-papel-green btn btn-sm btn-success" type="button" value="' + row.id + '" ' +
                            (!row.encuestaPapel ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-papel btn-papel-red btn btn-sm" type="button" value="' + row.id + '" ' +
                            (!row.encuestaPapel ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            }
        ],
        columnDefs: [
            { className: "dt-center", targets: "_all" }
        ]
    });
}
var groupColumn = 0;
function initTablaPermisosrpt() {
    tblPermisosReporte.DataTable({
        retrieve: true,
        paging: false,
        searching: false,
        language: dtDicEsp,
        scrollCollapse: true,
        bFilter: false,
        info: false,
        scrollY: '50vh',
        dom: 'Bfrtip',
        buttons: parametrosImpresion("Reporte de Permisos", "<center><h3>Reporte de Permisos </h3></center>"),
        initComplete: function (settings, json) {
            tblPermisosReporte.on('click', '.btn-consultar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-consultar-green btn-success');
                    $(this).addClass('btn-consultar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-consultar-red');
                    $(this).addClass('btn-consultar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisosReporte.on('click', '.btn-editar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-editar-green btn-success');
                    $(this).addClass('btn-editar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-editar-red');
                    $(this).addClass('btn-editar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisosReporte.on('click', '.btn-enviar', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-enviar-green btn-success');
                    $(this).addClass('btn-enviar-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-enviar-red');
                    $(this).addClass('btn-enviar-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisosReporte.on('click', '.btn-telefonica', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-telefonica-green btn-success');
                    $(this).addClass('btn-telefonica-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-telefonica-red');
                    $(this).addClass('btn-telefonica-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisosReporte.on('click', '.btn-notificacion', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-notificacion-green btn-success');
                    $(this).addClass('btn-notificacion-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-notificacion-red');
                    $(this).addClass('btn-notificacion-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });

            tblPermisosReporte.on('click', '.btn-papel', function () {
                if ($(this).attr('data-check') == 'true') {
                    $(this).removeClass('btn-papel-green btn-success');
                    $(this).addClass('btn-papel-red');
                    $(this).text('No');
                    $(this).attr('data-check', 'false');
                } else {
                    $(this).removeClass('btn-papel-red');
                    $(this).addClass('btn-papel-green btn-success');
                    $(this).text('Sí');
                    $(this).attr('data-check', 'true');
                }
            });
        },
        destroy: true,
        scrollCollapse: true,
        columns: [
            {data: 'nombreUsuario',title:'Nombre'},
            { data: 'titulo', title: "Encuesta" },
            { data: 'descripcion', title: "Descripción" },
            { data: 'departamentoDesc', title: "Departamento" },
            {
                data: 'soloLectura', title: "Consultar", render: function (data, type, row, meta) {
                    if (row.consultar) {
                        return '<button class="btn-consultar  btn-consultar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true" disabled>Sí</button>';
                    } else {
                        return '<button class="btn-consultar btn-consultar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false" disabled>No</button>';
                    }
                }
            },
            {
                data: 'soloLectura', title: "Editar", render: function (data, type, row, meta) {
                    if (row.editar) {
                        return '<button class="btn-editar btn-editar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true" disabled >Sí</button>';
                    } else {
                        return '<button class="btn-editar btn-editar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false" disabled>No</button>';
                    }
                }
            },
            {
                data: 'soloLectura', title: "Enviar", render: function (data, type, row, meta) {
                    if (row.enviar) {
                        return '<button class="btn-enviar btn-enviar-green btn btn-sm btn-success" type="button" value="' + row.id + '" data-check="true" disabled>Sí</button>';
                    } else {
                        return '<button class="btn-enviar btn-enviar-red btn btn-sm" type="button" value="' + row.id + '" data-check="false" disabled>No</button>';
                    }
                }
            },
            {
                data: 'contestaTelefonica', title: "Telefónica", render: function (data, type, row, meta) {
                    if (row.contestaTelefonica) {
                        return '<button class="btn-telefonica btn-telefonica-green btn btn-sm btn-success" disabled type="button" value="' + row.id + '" ' +
                            (row.soloLectura || !row.encuestaTelefonica ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-telefonica btn-telefonica-red btn btn-sm" type="button" disabled  value="' + row.id + '" ' +
                            (row.soloLectura || !row.encuestaTelefonica ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'recibeNotificacion', title: "Notificación", render: function (data, type, row, meta) {
                    if (row.recibeNotificacion) {
                        return '<button class="btn-notificacion btn-notificacion-green btn btn-sm btn-success" disabled  type="button" value="' + row.id + '" ' +
                            (!row.encuestaNotificacion ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-notificacion btn-notificacion-red btn btn-sm" type="button" disabled value="' + row.id + '" ' +
                            (!row.encuestaNotificacion ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            },
            {
                data: 'contestaPapel', title: "Papel", render: function (data, type, row, meta) {
                    if (row.contestaPapel) {
                        return '<button class="btn-papel btn-papel-green btn btn-sm btn-success" type="button" disabled value="' + row.id + '" ' +
                            (!row.encuestaPapel ? 'disabled' : '') + ' data-check="true">Sí</button>';
                    } else {
                        return '<button class="btn-papel btn-papel-red btn btn-sm" type="button" disabled value="' + row.id + '" ' +
                            (!row.encuestaPapel ? 'disabled' : '') + ' data-check="false">No</button>';
                    }
                }
            }
        ],
        columnDefs: [
            { className: "dt-center", targets: "_all" },
            { "visible": false, "targets": groupColumn }
        ],
        "drawCallback":function(settings)
        {
            var api = this.api();
            var rows = api.rows( {page:'current'} ).nodes();
            var last=null;
 
            api.column(groupColumn, {page:'current'} ).data().each( function ( group, i ) {
                if ( last !== group ) {
                    $(rows).eq( i ).before(
                        '<tr class="group"><td colspan="9" style="background: orange;"><label> '+group+'</label></td></tr>'
                    );  
 
                    last = group;
                }
            } );
        }
    });
}

function AddRows(tbl, lst) {
    dt = tbl.DataTable();
    dt.clear().draw();

    dt.rows.add(lst).draw(false);
    var column = dt.column( 3);
    if(idComboFiltro.val()==0){
        column.visible( true );
    }
    else if(idComboFiltro.val()==1){
        column.visible( true );
    }
    else{
        column.visible( false );
    }
}

init();
};

$(document).ready(function () {
    encuestas.GestionUsuarios = new GestionUsuarios();
}).ajaxStart(function () {
    $.blockUI({ message: 'Procesando...' });
}).ajaxStop(function () {
    $.unblockUI();
});
})();