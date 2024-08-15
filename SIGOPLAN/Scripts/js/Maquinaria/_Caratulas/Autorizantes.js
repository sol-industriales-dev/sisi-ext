
(() => {
    $.namespace("Caratula.Maquinaria");

    Maquinaria = function () {   
        const divAutorizantes = $('#divAutorizantes');           
        ireporteTiemposCRC = $("#reporteCaratula > #reportViewerModal > #report");
        reporteCaratula = $('#reporteCaratula');
        const fieldsetAutorizacion = $('#fieldsetAutorizacion');
        const fieldsetReporte = $('#fieldsetReporte');
        const divFiltros = $('#divFiltros');
        const divPrenominas = $('#divPrenominas');
        const report = $('#report');
        const modalAutorizar = $('#modalAutorizar');
        const modalRechazar = $('#modalRechazar');
        const tablaCaratula = $('#tablaCaratula');
        const cboCaratulas = $('#cboCaratulas');
        const btnBuscarReporte = $('#btnBuscarReporte');
        const modalAutorizarbtnAutorizar = $('#modalAutorizarbtnAutorizar');
        const textAreaRechazo = $('#textAreaRechazo');
        const modalAutorizarbtnRechazar = $('#modalAutorizarbtnRechazar');
        
        const btnBuscar = $('#btnBuscar');
        const cboEstatus = $('#cboEstatus');
        const tblHistorial = $('#tblHistorial');
        let dtHistorial;

        let lstEstatus = [
            {val: 0,text: 'En espera'},
            {val: 1,text: 'Autorizado'},
            {val: 2,text: 'Rechazado'},
        ];


        (function init() {   

            initDatatblHistorial();
            obtenerEstatus();
            //fncGetCaratula();
            // initDataTblCaratula();
            fncFillCaratulas();
            iniciarModal();
            // obtenerListaAutorizantes();
            // divPrenominas.hide(500);
        })();   
        function iniciarModal() {
            $("#dlgFormulario").removeClass('hide');
            dlgFormulario = $("#dlgFormulario").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });
        }
        function fncFillCaratulas(){
            cboCaratulas.fillCombo("FillCaratulas", {},false);
            cboCaratulas.select2({
                width: "resolve"
            });
            let item = ``; 
            lstEstatus.forEach( x => {
                item += `<option value="${x.val}"  >${x.text}</option>`;
            });
            cboEstatus.append(item);
            btnBuscar.click(function () {
                
            })
            // btnBuscarReporte.click(function(){
            //     obtenerListaAutorizantes();
            // });

            modalAutorizarbtnAutorizar.click(function(){
                autorizar();
            });

            modalAutorizarbtnRechazar.click(function(){
                    rechazar();
            });
            cboEstatus.change(function () {
                obtenerEstatus();
            })
        }
        function obtenerEstatus() {
            axios.post('/Caratulas/obtenerHistorialCaratulas', {estatus: cboEstatus.val() == null ? 0 :cboEstatus.val() })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items} = response.data;
                    if (success) {
                        AddRows(tblHistorial,items.items);
                    }
                });
        }
        function initDatatblHistorial() {
            dtHistorial = tblHistorial.DataTable({
                destroy: true
                ,language: dtDicEsp
                ,paging: false
                ,ordering:false
                ,searching: false
                ,bFilter: true
                ,info: false
                ,columns: [
                    { data: 'id', title: 'id' ,visible:false },
                    { data: 'NombreCaratula', title: 'NombreCaratula'},
                    { data: 'estatus', title: 'estatus' },
                    { data: 'tipodeCambio', title: 'tipodeCambio' },
                    { title: 'Opciones' ,render: (data, type, row, meta) => {
                        let item ='';
                        return item = `<button class='btn btn-primary verReporte'><i class='fas fa-file'></i></button>`;
                        }
                    },
                ],
                columnDefs: [
                    { className: 'dt-center','targets': '_all'},
                    { width: '105px', targets: [2,3] }, 
                ]
                ,initComplete: function (settings, json) {
                    tblHistorial.on('click','.verReporte', function () {
                        let rowData = dtHistorial.row($(this).closest('tr')).data();
                        console.log(rowData)
                        dlgFormulario.dialog("open");
                        $('#dlgFormulario').css('min-height', '850px');
                        $('#dlgFormulario').css('height', '100%');
                        getReporteCaratula(rowData.id)
                    });
                }
            });
        }
         
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.draw();
            dt.rows.add(lst).draw(false);
        }
        function initDataTblCaratula() {
            dtCaratula = tablaCaratula.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {data:"id",title:"hola"},
                    {data:"usuario",title:"hola"}
                ],
                initComplete: function (settings, json) {
                    tablaCaratula.on('click','.classBtn', function () {
                        let rowData = dtCaratula.row($(this).closest('tr')).data();
                    });
                    tablaCaratula.on('click','.classBtn', function () {
                        let rowData = dtCaratula.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }


         function ocultarPaneles() {

            // fieldsetAutorizacion.hide(500);
            divPrenominas.hide(500);

            fieldsetAutorizacion.data().idCaratula = null;
            // fieldsetReporte.hide(500);
            divAutorizantes.empty();

            //dtTablaPlantillaActual.clear().draw();
            //dtTablaPlantillaNueva.clear().draw();

            // divPrenominas.prop('hidden', false);
            // divPrenominas.show(500);

            divFiltros.prop('hidden', false);
            divFiltros.show(500);
        }

        

         function getReporteCaratula(idCaratula){
            axios.post("/Caratulas/GetReporteCaratula",{idCaratula:idCaratula}).then(response => {
                let { success, items, message } = response.data;
                console.log(response);
                if (success) {           
                    fncGetCaratula(idCaratula);
                }
                else {
                    Alert2Error(message)
                }
    
            }).catch(error => Alert2Error(error.message));
         }

        function fncGetCaratula(idCaratula) { 
            axios.post("ListaAutorizantes",{idCaratula:idCaratula}).then(response => {
                let { success, items, message } = response.data;
                console.log(response);
                if (success) {           
                    // dtCaratula.clear().draw();
                    // dtCaratula.rows.add(response.data.lst).draw();
                    console.log(response.data.autorizantes)    
                    establecerPaneles(response.data.autorizantes,idCaratula);  
                }
                else {
                    Alert2Error(message)
                }
    
            }).catch(error => Alert2Error(error.message));
        }

        // function obtenerListaAutorizantes(idCaratula) {            
        //     $.blockUI({ message: 'Cargando datos...' });
        //     $.get('/Caratulas/ListaAutorizantes?idCaratula='+idCaratula)
        //         .always($.unblockUI)
        //         .then(response => {
        //             if (response.success) {
        //                 if (response.autorizantes.length > 0) {
        //                     CargarRptCaratula(idCaratula);
        //                     establecerPaneles(response.autorizantes);
        //                 } else {
        //                     Alert2AccionConfirmar('No hay caratulas por autorizar','Sera redirigido a caratulas','Confirmar','Cancelar', () => fncRedireccionar());
        //                 }
        //             } else {                  
        //                 AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
        //             }
        //         }, error => {               
        //             AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
        //         }
        //         );
        // }

        function fncRedireccionar() {
            document.location.href = '/Caratulas/viewCaratulas';
        }
   
        function CargarRptCaratula(idCaratula) {            
            $.ajax({
                url: '/Caratulas/GetReporteCaratula?idCaratula='+idCaratula,
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                success: function (response) {
                    $.blockUI({ message: 'Cargando reporte...' });
                    $.unblockUI();                     
                    report.attr(`src`, `/Reportes/Vista.aspx?idReporte=222&isCRModal=${false}`+"&inMemory=1"); 
                    report[0].onload = () => {
                        $.unblockUI();
                    }                   
                },
                error: function (response) {             
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function establecerPaneles(listaAutorizantes, idCaratula) {
            console.log(listaAutorizantes);
            divPrenominas.hide(500);
            divFiltros.hide(500);
            fieldsetReporte.show(500);
            fieldsetAutorizacion.show(500);

            //divAutorizacion.show(500);

            

            listaAutorizantes.forEach((autorizante, index) => {
                console.log(`N: ${autorizante.nombre} A: ${autorizante.authEstado}`);
                const divPanel = $(`
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="panel panel-default text-center">
                            <div class="panel-heading"><label>${autorizante.nombre}</label></div>                           
                            <div class="panel-body ">
                                <p>${autorizante.descripcion}</p>
                                <button ${autorizante.authEstado == 3 ? "id='botonAutorizar'" : ''} class='${autorizante.authEstado == 3 ? '' : 'hidden'}  btn btn-success btnPanel'><i class='fa fa-check'></i> Autorizar</button>
                                <button ${autorizante.authEstado == 3 ? "id='botonRechazar'" : ''} class='${autorizante.authEstado == 3 ? '' : 'hidden'}  btn btn-danger btnPanel'><i class='fa fa-ban'></i> Rechazar</button>
                            </div>
                            <div class="panel-footer ${autorizante.authEstado == 1 ? "panelAutorizado" : autorizante.authEstado == 2 ? "panelRechazado" : "panelPendiente"}">
                                <p>${autorizante.authEstado == 1 || autorizante.estatus == 1 ? 'Autorizado' : autorizante.authEstado == 2 ? "Rechazado" : "Pendiente"}</p>
                                <p>${autorizante.firma ? autorizante.firma : "S/F"}</p>
                            </div>
                        </div>
                    </div>
                </div>
                `);
                
                divPanel.data().idRegistro = autorizante.idRegistro;

                divAutorizantes.append(divPanel);
            });

            $('#botonAutorizar').click(e => {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                console.log('hola');
                modalAutorizar.modal('show');
                const registroID = $(e.currentTarget).parents('div.row').data().idRegistro;
                fieldsetAutorizacion.data().idRegistro = registroID;
            });

            $('#botonRechazar').click(e => {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                modalRechazar.modal('show');

                const registroID = $(e.currentTarget).parents('div.row').data().idRegistro;
                fieldsetAutorizacion.data().idRegistro = registroID;

                textAreaRechazo.change(() =>
                    textAreaRechazo.val(sanitizeString(textAreaRechazo.val()))
                );
            });
            CargarRptCaratula(idCaratula);
        }


        function sanitizeString(str) {
            str = str.replace(/[^a-z0-9áéíóúñü \.,]/gim, "");
            return str.trim();
        }

        function autorizar() {
            
            const id = fieldsetAutorizacion.data().idRegistro;

            const caratulas = {               
                idRegistro: id
            };

          

                modalAutorizar.modal('hide');

                $.blockUI({ message: 'Autorizando...' });
                $.post('Autorizar', { caratulas })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) { 
                            dlgFormulario.dialog("close");
                            enviarCorreoAut();
                            ocultarPaneles();                            
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, "Espera ha que el usuario termine de autorizar");
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            
        }

        function rechazar() {

            const comentario = textAreaRechazo.val().trim();

            if (comentario == null || comentario.trim().length <= 10) {
                AlertaGeneral("Aviso", "El mensaje de rechazo debe tener un mínimo de 10 caracteres.");
                return;
            }

            
            const id = fieldsetAutorizacion.data().idRegistro;

            const Rechazar = {
                
                idRegistro: id,
                comentario
            };

            modalRechazar.modal('hide');
            $.blockUI({ message: 'Rechazando caratula...' });
            $.post('Rechazar', { Rechazar })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        dlgFormulario.dialog("close");
                        AlertaGeneral(`Aviso`, `caratula rechazada.`);
                        enviarCorreoAut();
                        ocultarPaneles();                        
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al intentar rechazar la caratula`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function enviarCorreoAut() {
            $.post('/Caratulas/EnviarCorreoAutorizacion')
                .done(respuesta => {
                    if (respuesta.success) {        
                        Alert2AccionConfirmar('Autorización','Se han enviado correo a los autorizantes correctamente.','Confirmar','Salir');               
                        //AlertaGeneral("Éxito", "Se han enviado correo a los autorizantes correctamente.");
                    } else {
                        AlertaGeneral("Error", `La autorización fue exitosa pero
                        no se pudo enviar correo al siguiente autorizante: ${respuesta.error}`);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }
    
           
           
        


      
}
    $(document).ready(() => Caratula.Maquinaria = new Maquinaria())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();