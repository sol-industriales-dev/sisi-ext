(() => {
$.namespace('Encuestas.Asignacion');
    Asignacion = function (){
        let _encuestaID = 0;
        const lblTitulo = $('#lblTitulo');
        const tblEncAsignar = $('#tblEncAsignar');
        const inpEncAsigUsuario = $('#inpEncAsigUsuario');
        const mdlEncAsigUsuario = $('#mdlEncAsigUsuario');
        const btnEncAsigUsuario = $('#btnEncAsigUsuario');
        const tblEncAsigUsuario = $('#tblEncAsigUsuario');
        const btnEncAsigAddUsuario = $('#btnEncAsigAddUsuario');
        const eliminaEncAsigUsuario = new URL(window.location.origin + '/Encuestas/Encuesta/eliminaEncAsigUsuario');
        const GuardarEncAsigUsuario = new URL(window.location.origin + '/Encuestas/Encuesta/GuardarEncAsigUsuario');
        const getEncuestaAsignaUsuario = new URL(window.location.origin + '/Encuestas/Encuesta/getEncuestaAsignaUsuario');
        let init = () => {
            initForm();
            btnEncAsigUsuario.click(setGuardarEncAsigUsuario);
            btnEncAsigAddUsuario.click(addUsuarioTblEncAsigUsuario);
            mdlEncAsigUsuario.on('hidden.bs.modal', () => { 
                _encuestaID = 0; 
                lblTitulo.text("");
            });
        }
        async function setGuardarEncAsigUsuario() {
            try {
                let lst = dtEncAsigUsuario.data().map(us => ({ 
                    usuarioID: us.id
                  ,encuestaID: _encuestaID 
                })).toArray();
                if (_encuestaID > 0 && lst.length > 0) {
                    response = await ejectFetchJson(GuardarEncAsigUsuario, {lst});
                    if (response.success) {
                        setEncuestaAsignaUsuario();
                        mdlEncAsigUsuario.modal("hide");
                        AlertaGeneral("Aviso","Usuarios guardado con éxito");
                        dtEncAsigUsuario.clear().draw();
                    }   
                }
            } catch (o_O) { }
        }
        async function seteliminaEncAsigUsuario(obj) {
            try {
                response = await ejectFetchJson(eliminaEncAsigUsuario, obj);
                return response.success;
            } catch (o_O) { }
        }
        async function setEncuestaAsignaUsuario() {
            try {
                dtEncAsignar.clear().draw();
                response = await ejectFetchJson(getEncuestaAsignaUsuario);
                if (response.success) {
                    dtEncAsignar.rows.add(response.lst).draw();
                }
            } catch (o_O) { }
        }
        function addUsuarioTblEncAsigUsuario() {
            let data = inpEncAsigUsuario.data().empleado;
            if (data !== null) {
                let empleado = {
                    id:data.id
                   ,nombre: data.label
                   ,puestoDescripcion: data.puestoDescripcion
                   ,correo: data.correo
                   };
                dtEncAsigUsuario.row.add(empleado).draw();
                inpEncAsigUsuario.val("");
                inpEncAsigUsuario.data().empleado = null;   
            }
        }
        function initDataTblEncAsignar() {
            dtEncAsignar = tblEncAsignar.DataTable({
                destroy: true,
                language: dtDicEsp,
                order: [2, 'asc'],
                columnDefs: [ { className: "dt-center", "targets": [0, 1, 2, 3] } ],
                columns: [
                    { title:'Título' ,data: 'titulo'}
                   ,{ data: 'descripcion', title: "Descripción" }
                   ,{ title:'Departamento' ,data: 'departamento'}
                   ,{ title:'Usuarios' ,data:'usuarios' ,createdCell: (td ,data, rowData) => {
                        let tieneUsuario = data.length > 0 ? "btn-success" : "btn-default";
                        $(td).html(`<button>`);
                        $(td).find(`button`).addClass(`btn ${tieneUsuario} asingUsuario`);
                        $(td).find(`button`).html(`<i class='fa fa-user'></i> Asignar`);
                       }
                   }
                ],
                initComplete: function (settings, json) {
                    tblEncAsignar.on('click','.asingUsuario', function () {
                        let data = dtEncAsignar.row($(this).closest('td')).data()
                           ,lstUsuario = data.usuarios;
                        lblTitulo.text(data.titulo);
                        _encuestaID = data.id;
                        dtEncAsigUsuario.clear().draw();
                        dtEncAsigUsuario.rows.add(lstUsuario).draw();
                        inpEncAsigUsuario.val("");
                        inpEncAsigUsuario.data().empleado = null;
                        mdlEncAsigUsuario.modal('show');
                    });
                }
            });
        }
        function initDataTblEncAsigUsuario() {
            dtEncAsigUsuario = tblEncAsigUsuario.DataTable({
                destroy: true
                ,language: dtDicEsp
                ,columns:[
                    { title:'Nombre' ,data: 'nombre'}
                   ,{ title:'Puesto' ,data: 'puestoDescripcion'}
                   ,{ title:'Correo' ,data: 'correo'}
                   ,{ title:'' ,data: 'id' ,createdCell: function (td, data, rowData, row, col) {
                           let btn = $(`<button>`)
                           btn.addClass(`btn btn-danger glyphicon glyphicon-remove elimina`);
                           btn.html(``);
                           $(td).html(btn);
                       }
                   }
                ]
                ,initComplete: function (settings, json) {
                    tblEncAsigUsuario.on('click','.elimina', function () {
                        let tr = $(this).closest('td')
                           ,data = dtEncAsigUsuario.row(tr).data()
                           ,obj = {
                                usuarioID: data.id
                               ,encuestaID: _encuestaID
                           }
                           ,esEliminado = seteliminaEncAsigUsuario(obj);
                        if (esEliminado) {
                            setEncuestaAsignaUsuario();
                            dtEncAsigUsuario.row(tr).remove().draw();
                        }
                    });
                }
            });
        }
        setIdEmpleado = (event, ui) => inpEncAsigUsuario.data({empleado: ui.item});
        function initForm() {
            initDataTblEncAsignar();
            initDataTblEncAsigUsuario();
            setEncuestaAsignaUsuario();
            inpEncAsigUsuario.getAutocomplete(setIdEmpleado, null, '/Encuestas/Encuesta/getUsuarios');
        }
        init();
    }
    $(document).ready(() => {
        Encuestas.Asignacion = new Asignacion();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();