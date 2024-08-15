(() => {
    $.namespace('subContratistas.ControlObra');

    const cboProyecto = $('#cboProyecto');
    const cboSubContratista = $('#cboSubContratista');
    const contenido = $('#contenido');
    const contenido23 = $('#contenido23');
    const tblAutorizacion = $('#tblAutorizacion');
    let dtAutorizacion;
    const btnBuscar = $('#btnBuscar');
    const mdlComentario = $('#mdlComentario');
    const btnGuardar = $('#btnGuardar');

    const inpComentario = $('#inpComentario');
    const inpEvaluacion = $('#inpEvaluacion');
    const inpPlanDeAccion = $('#inpPlanDeAccion');
    const inpResponsable = $('#inpResponsable');
    const inpFechaCompromiso = $('#inpFechaCompromiso');

    const mdlPreguntarPrimero = $('#mdlPreguntarPrimero');
    const containerPreguntarPrimero = $('#containerPreguntarPrimero');
    const btnGuardarPreguntarPrimero = $('#btnGuardarPreguntarPrimero');
    const cboEstatus = $('#cboEstatus');
    const mdlFormulario = $('#mdlFormulario');

    const btnGuardarModal = $('#btnGuardarModal');
    const txtUsuario = $('#txtUsuario');
    const txtPeriodoDeFechas = $('#txtPeriodoDeFechas');
    const txtTituloClick = $('#txtTituloClick');
    const mdlGraficasPorSubcontratista = $('#mdlGraficasPorSubcontratista');
    const mdlPlantillas = $('#mdlPlantillas');
    var TipoUSuario = 0;
    let datosAGuardar;
    let dtPromedio;
    let dtAsginacion;
    var srowData = {};
    let Estatus = [
        { val: 0, text: 'Pendientes por Asignar' },
        { val: 2, text: 'Pendientes por Autorizar' },
        { val: 3, text: 'Autorizadas' },
        { val: 4, text: 'Historial' }
    ]

    const contenedorEstatus = $('#contenedorEstatus');


    ControlObra = function () {
        let init = () => {
            inpEvaluacion.css('font-weight', 'bold');
            fillCombos();
            fncButtons();
            cargarcboEstatus();
            cargarTipoUsuario();

            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();

            // ObtenerGraficaDeEvaluacionPorCentroDeCosto();
        }
        init();
    }
    function getParametrosGrafica() {
        let parametros = {

        };
        return parametros;
    }
    function ObtenerGraficaDeEvaluacionPorCentroDeCosto(params) {
        let parametros = getParametrosGrafica();
        axios.post('ObtenerGraficaDeEvaluacionPorCentroDeCosto', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {

                }
            });
    }
    function cargarcboEstatus() {
        let elemento = ``;
        Estatus.forEach(x => {
            elemento = `<option value="${x.val}">${x.text}</option>`;
            cboEstatus.append(elemento);
        });
    }
    function cargarTipoUsuario() {
        // axios.post('cargarTipoUsuarios').then(response => {
        //     let { success, items } = response.data
        //     if (success) {
        //         if(items == 10){
        TipoUSuario = 10;
        contenedorEstatus.css('display', 'none');
        cboEstatus.val(2);
        cboEstatus.trigger('change');
        //         }else{
        // TipoUSuario = 10;
        // contenedorEstatus.css('display','block');
        // cboEstatus.val(0);
        // cboEstatus.trigger('change');
        //         }
        //     }
        // }).catch(error => Alert2Error(error.message));
    }
    function fillCombos() {
        cboProyecto.fillCombo('getProyecto', null, false, null);
        cboSubContratista.fillCombo('getSubContratistas?AreaCuenta=' + cboProyecto.val(), null, false, null);
    }
    function fncButtons() {
        btnBuscar.click(function () {
            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();
            CargarConfiguracionVista();
        })
        btnGuardar.click(function () {
            GuardarEvaluacion($(this));
        })
        cboProyecto.change(function (e) {
            cboSubContratista.fillCombo('getSubContratistas?AreaCuenta=' + cboProyecto.val(), null, false, null);
        })
        btnGuardarPreguntarPrimero.click(function () {
            fncGuardarPreguntarPrimero();
        })
        inpEvaluacion.change(function () {
            coloresIo();
        })
        cboEstatus.change(function () {
            $(`#ContenidoDiviciones`).remove();
            $(`#ConenidoArchivos`).remove();

            CargarConfiguracionVista();
        })
    }
    function coloresIo() {
        // if (inpEvaluacion.val() == 25) {
        //     inpEvaluacion.css('background-color','#FA0101');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 50){
        //     inpEvaluacion.css('background-color','#FA8001');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 70){
        //     inpEvaluacion.css('background-color','#FAFF01');
        //     inpEvaluacion.css('color','#000');
        // }else if(inpEvaluacion.val() == 90){
        //     inpEvaluacion.css('background-color','#018001');
        //     inpEvaluacion.css('color','#fff');
        // }else if(inpEvaluacion.val() == 100){
        //     inpEvaluacion.css('background-color','#0180FF');
        //     inpEvaluacion.css('color','#fff');
        // }
    }
    function CargarConfiguracionVista() {
        axios.post('obtenerDivicionesEvaluador', {})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    fncGenerandoDivicion(items);
                }
            });
    }
    function fncGenerandoDivicion(lstDatos) {
        let html = `   <div class="col-lg-12 col-md-12 col-sm-12" id='ContenidoDiviciones'>
                            <div class=" row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 0;display:none;">
                                    <fieldset class="fieldset-custm">
                                        <legend class="legend-custm">Menu : </legend>`;
        for (let index = 0; index < lstDatos.length; index++) {
            html += `
                                    <div class='col-md-3'>     
                                        <button class="btn btn-block btn-social btn-primary-menu boton-menu" id="${lstDatos[index].idbutton}" data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}">
                                            <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                                        </button>
                                    </div>    
                                        `;
        }

        html += ` 
                            </fieldset >
                            
                            </div>
                        </div>
                    </div>`;
        html += `<div class="col-lg-12 col-md-12 col-sm-12" id='ConenidoArchivos'>
                    <div class='row'>
                    `;
        for (let index = 0; index < lstDatos.length; index++) {

            html += `
                    
                        <div id="${lstDatos[index].idsection}"  class="col-md-12" style='padding: 0;'>
                            <fieldset class="fieldset-custm">
                                <legend class="legend-custm">${lstDatos[index].descripcion} : <span id="lblErrorProyecto"></span></legend>
                                <div id="${lstDatos[index].idsection + lstDatos[index].id}" ></div>
                            </fieldset>
                        </div>
                   
                    
                    `;

        }
        html += `
                    </div>
                
                </div>`;
        contenido.append(html);

        for (let index = 0; index < lstDatos.length; index++) {
            $(`#${lstDatos[index].idsection}`).css('display', 'none');
            $(`#${lstDatos[index].idbutton}`).click(function () {

                $('#contenidoPromedio').remove();
                $('#contenidoPromedio2').remove();
                for (let i = 0; i < lstDatos.length; i++) {
                    if (lstDatos[index].idbutton == lstDatos[i].idbutton) {
                        $(`#${lstDatos[i].idsection}`).css('display', 'block');
                        btnBuscar.attr('data-id', lstDatos[i].id);
                        obtenerInputs(lstDatos[index].id, lstDatos[index].idsection + lstDatos[index].id);
                    } else {
                        $(`#${lstDatos[i].idsection}`).css('display', 'none');
                    }

                }
            })
        }
        //AQUI ANDO
        if (lstDatos.length != 0) {
            if (TipoUSuario == 10) {
                $(`#${lstDatos[0].idsection}`).css('display', 'block');
                btnBuscar.attr('data-id', lstDatos[0].id);
                obtenerInputs(lstDatos[0].id, lstDatos[0].idsection + lstDatos[0].id);
                $('#btnEvaluacion2').css('display', 'none');
            } else {
                $(`#${lstDatos[0].idsection}`).css('display', 'block');
                btnBuscar.attr('data-id', lstDatos[0].id);
                obtenerInputs(lstDatos[0].id, lstDatos[0].idsection + lstDatos[0].id);
            }
        }

    }
    function obtenerInputs(id, idsection) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    generarInputs(items, idsection);
                }
            });
    }
    function generarInputs(lstDatos, idsection) {
        $(`#${idsection}`).find('div').remove();
        if (idsection == 'sectionDiv22') {
            let html = `
                <div class='row'>
                    <div class="col-md-12">
                        <table id='tblPromedios' class="table table-hover table-bordered table-striped compact"></table>
                    </div>
                </div>

                <div class="row margin-top">
                    <div class="col-xs-12 col-md-12">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title text-center">
                                        <a data-toggle="collapse" href="#panel2">
                                            Grafica de evaluaciones pendientes realizadas por centro de costo
                                        </a>
                                    </h4>
                                </div>
                                <div id="panel2" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <div class="col-lg-12">
                                            <div style="margin-left: auto; margin-right: auto">
                                                <figure class="highcharts-figure">
                                                    <div id="gpxporCentrosDeCosto" style="margin-left:auto; margin-right:auto;"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row margin-top">
                    <div class="col-xs-12 col-md-12">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title text-center">
                                        <a data-toggle="collapse" href="#panel1">
                                            Grafica de subcontratistas por indicadores
                                        </a>
                                    </h4>
                                </div>
                                <div id="panel1" class="panel-collapse collapse in">
                                    <div class="panel-body">
                                        <div class="col-lg-12">
                                            <div style="margin-left: auto; margin-right: auto">
                                                <figure class="highcharts-figure">
                                                    <div id="GraficaReportesBarras" style="margin-left:auto; margin-right:auto;"></div>
                                                </figure>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;

            $(`#${idsection}`).append(html);
            obtenerGraficaPastel();
            initDataTblPromedio();
            obtenerPromediosxSubcontratista();

        } else {
            let html = `
                <div class='row'>
                    <div class="col-md-12">
                        <table id='tblAsignacionEvaluacion' class="table table-hover table-bordered table-striped compact" style="font-size: 12px;">
                            <thead>
                                <tr style='background-color: #000; color: #FFF; font-size: 12px;'>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th colspan="3">EVALUACIONES</th>
                                </tr>
                                <tr style='font-size: 12px;'>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th id='txtTexto'></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
              `;
            $(`#${idsection}`).append(html);

            inittblAsignacionEvaluacion();
            ObtenerContratistas();
        }


    }
    function GenerarInputModal(lstDatos, idsection, rowData, dn, btnEvaluacion) {
        $(`#${idsection}`).find('div').remove();
        // CargarArchivosXSubcontratista(lstDatos);

        if (lstDatos.length != 0) {
            let html = ` <div class='row'>`;
            html += `
                        <div class='container-fluid'>
                            <div class='row' style='overflow-y: scroll;'>
                                <div class='col-md-12'>
                             
                                    `;
            for (let index = 0; index < lstDatos.length; index++) {
                html += `
                                    <div class="col-sm-6">
                                        <div class="panel-group">
                                            <div class="panel panel-primary">
                                                <div class="panel-heading" style="padding: 5px;">
                                                    <div class="panel-title" style="font-size: 11px;">
                                                        ${lstDatos[index].texto.substr(0, 75)}
                                                    </div>
                                                </div>
                                                <div class="panel-collapse collapse in" aria-expanded="true">
                                                    <div class="panel-body p-primary" style="padding: 5px;">
                                                        <div class="row text-center">
                                                            <label for="${lstDatos[index].inputFile}" id="${lstDatos[index].inputFile}"  class="inputs pointer btn" style="margin-top: 10px; border-radius: 20px;">
                                                                <i class="fa fa-file-alt fa-3x"></i>
                                                            </label>
                                                            <br>
                                                            <label class="label labelNombre" id="${lstDatos[index].lblInput}" class="botonDocumento" data-idevaluacion='${lstDatos[index].id}' style='color:black;'> Ningún Archivo Seleccionado</label>
                                                        </div>
                                                    </div>
                                                </div>
                                                    <button id="btnbutton${lstDatos[index].id}" data-id='${lstDatos[index].id}' class="btn btn-primary" style='width:100%;'><i class="fa fa-list"></i> Calificar</button>
                                            
                                            </div>
                                        </div>
                                    </div>
                                        `;
            }
            html += `
                                    </div>
                                </div>
                            </div>
                            `;

            html += `
                        </div>
                        <div class='row'>
                        <div class="col-xs-12 col-md-6" id='contenidoPromedio'>
                            <div class="input-group">
                                <span class="input-group-addon">Promedio Evaluacion : </span>
                                <input id="inpPromedio" class="form-control" disabled='true'/>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6" id='contenidoPromedio2' style='    text-align-last: end;'>
                           <button id="btnEvaluado" class="btn btn-primary"><i class="fa fa-list"></i> Notificar subcontratista</button>
                        </div>
                        </div>
                        `;

            //     <table  class='table table-hover table-fixed table-bordered compact dataTable no-footer' style='width: 100% !important;  '>
            //     <thead>
            //         <tr style='background-color:#000;color:#FFF;text-align-last: center;'>
            //             <th>DESCRIPCION</th>
            //             <th>ARCHIVOS</th>
            //             <th>EVALUACION</th>
            //         </tr>
            //     </thead>
            // <tbody>
            //     `;
            // for (let index = 0; index < lstDatos.length; index++) {
            //     html += `
            //             <tr>
            //                 <td>${lstDatos[index].texto}</td>
            //                 <td>
            //                 <div class="panel-collapse collapse in" aria-expanded="true">
            //                     <div class="panel-body p-primary" style="padding: 5px;">
            //                         <div class="row text-center">
            //                             <label for="${lstDatos[index].inputFile}" id="${lstDatos[index].inputFile}"  class="inputs pointer btn" style="margin-top: 10px; border-radius: 20px;">
            //                                 <i class="fa fa-file-alt fa-3x"></i>
            //                             </label>
            //                             <br>
            //                             <label class="label labelNombre" id="${lstDatos[index].lblInput}" class="botonDocumento" data-idevaluacion='${lstDatos[index].id}' style='color:black;'> Ningún Archivo Seleccionado</label>
            //                         </div>
            //                     </div>
            //                 </div>
            //                 </td>
            //                 <td><button id="btnbutton${lstDatos[index].id}" data-id='${lstDatos[index].id}' class="btn btn-primary" style='width:100%;'><i class="fa fa-list"></i> Evaluacion</button></td>
            //             </tr>
            //         `;
            // }
            // html += `
            //         </tbody>
            //     </table>



            // <div class="col-sm-6">
            //     <div class="panel-group">
            //         <div class="panel panel-primary">
            //             <div class="panel-heading" style="padding: 5px;">
            //                 <div class="panel-title" style="font-size: 11px;">
            //                     ${lstDatos[index].texto}
            //                 </div>
            //             </div>
            //             <div class="panel-collapse collapse in" aria-expanded="true">
            //                 <div class="panel-body p-primary" style="padding: 5px;">
            //                     <div class="row text-center">
            //                         <label for="${lstDatos[index].inputFile}" id="${lstDatos[index].inputFile}"  class="inputs pointer btn" style="margin-top: 10px; border-radius: 20px;">
            //                             <i class="fa fa-file-alt fa-3x"></i>
            //                         </label>
            //                         <br>
            //                         <label class="label labelNombre" id="${lstDatos[index].lblInput}" class="botonDocumento" data-idevaluacion='${lstDatos[index].id}' style='color:black;'> Ningún Archivo Seleccionado</label>
            //                     </div>
            //                 </div>
            //             </div>
            //                 <button id="btnbutton${lstDatos[index].id}" data-id='${lstDatos[index].id}' class="btn btn-primary" style='width:100%;'><i class="fa fa-list"></i> Evaluacion</button>

            //         </div>
            //     </div>
            // </div>








            //     <div class="col-md-12">
            //     <div class="col-md-5">
            //         <br>
            //         <label for="">${lstDatos[index].texto} </label>
            //     </div>
            //     <div class="col-md-5">

            //         <button id='${lstDatos[index].inputFile}' class="btn btn-success" ><i class="fa fa-download"></i>Descgargar</button>
            //         <label id="${lstDatos[index].lblInput}">Ningún archivo seleccionado</label>
            //     </div>
            //     <div class="col-md-2">
            //         <button id="btnbutton${lstDatos[index].id}" class="btn btn-primary"><i class="fa fa-list"></i> Evaluacion</button>
            //     </div>

            // </div>

            $(`#${idsection}`).append(html);
        }
        for (let index = 0; index < lstDatos.length; index++) {
            $(`#${lstDatos[index].inputFile}`).change(function () {
                $(`#${lstDatos[index].lblInput}`).text($(this)[0].files[0].name);

            });
            $(`#btnbutton${lstDatos[index].id}`).click(function () {
                btnGuardar.attr('data-idBtnRequerimiento', 0);
                mdlComentario.modal('show');
                $('#estrellasCalificacion').css('display', 'none');
                txtTituloClick.text(`${lstDatos[index].texto}`);
                btnBuscar.attr('data-id', $(`#btnbutton${lstDatos[index].id}`).attr('data-id'));
                btnGuardar.attr('data-idevaluacion', $(`#${lstDatos[index].lblInput}`).attr('data-idevaluacion'))
                btnGuardar.attr('data-idBtnRequerimiento', $(this).attr('id'));
                obtenerEvaluacionxReq();
            })
            $(`#${lstDatos[index].inputFile}`).click(function () {
                DescargarArchivos($(`#${lstDatos[index].lblInput}`).attr('data-idevaluacion'));
            })
        }
        cargandoModal(lstDatos, idsection, rowData);
        $('#btnEvaluado').attr('data-idevaluacion', rowData.id);
        $('#btnEvaluado').removeData('idbtnevaluacion');
        $('#btnEvaluado').attr('data-idbtnevaluacion', btnEvaluacion.attr('id'));
        $('#btnEvaluado').click(function () {
            Evaluado(lstDatos, rowData);
        });
    }
    function obtenerFuncion() {
        $('input.botonDocumento').change(function () {
            $(this).attr('archivosCambiados', 1);

            if ($(this)[0].files.length > 0) {
                visualizarArchivoCargado(this);

                if ($(this)[0].files.length > 1) {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files.length + ' archivos cargados.');
                } else {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files[0].name);
                }
            } else {
                visualizarArchivoNoCargado(this);

                $(this).closest('.panel-body').find('.labelNombre').text('Ningún Archivo Seleccionado');
            }
        });
    }
    function visualizarArchivoCargado(elemento) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoNoCargado(elemento) {
        $(elemento).closest('.panel-body').addClass('p-primary');
        $(elemento).closest('.panel-body').removeClass('p-success');
        $(elemento).closest('.panel-body').find('i').addClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoGuardado(elemento, archivos, tipoAnexo) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        if (archivos.length > 1) {
            $(elemento).closest('.panel-body').find('.labelNombre').text(archivos);
        } else {
            let rutaSeccionada = archivos[0].rutaArchivo.split('\\');

            $(elemento).closest('.panel-body').find('.labelNombre').text(rutaSeccionada[rutaSeccionada.length - 1]);
        }

        // $(elemento).closest('.panel').find('.panel-title').append(`
        //     <button class="btn btn-xs btn-warning botonVerArchivos" tipoanexo="${tipoAnexo}"><i class="fa fa-align-justify"></i></button>
        // `);
    }
    function getparamsEvaluar(lstDatos) {
        let params = []
        for (let i = 0; i < lstDatos.length; i++) {
            let item = {
                id: $(`#${lstDatos[i].lblInput}`).attr('data-idevaluacion'),
            };
            params.push(item)
        }
        return params;
    }
    function Evaluado(lstDatos, rowData) {
        let parametros = getparamsEvaluar(lstDatos);

        let textoElemento = $(`#${$('#btnEvaluado').attr('data-idbtnevaluacion')}`).text().trim();

        swal({
            title: 'Alerta!',
            text: `Se notificará al cliente que el elemento: ${textoElemento} fue evaluado con un promedio de ${$('#inpPromedio').val()} ¿Desea continuar?`,
            icon: 'warning',
            buttons: true,
            dangerMode: true,
            buttons: ['Cancelar', 'Notificar']
        })
            .then((aceptar) => {
                if (aceptar) {
                    axios.post('EvaluarDetalle', { id: $('#btnEvaluado').attr('data-idevaluacion'), parametros: parametros })
                        .catch(o_O => AlertaGeneral(o_O.message))
                        .then(response => {
                            let { success, items } = response.data;
                            if (success) {
                                if (items.status == 2) {
                                    // let btnElemento = $(`#${$('#btnEvaluado').attr('data-idbtnevaluacion')}`);
                                    // if (!btnElemento.hasClass('p-success')) {
                                    //     btnElemento.addClass('p-success');
                                    // }
                                    CargarColorVerdeDeNotificado(rowData);

                                }
                                Alert2Exito('Guardado Exitoso');
                            }
                        });
                }
            });
    }
    function DescargarArchivos(idDet) {
        location.href = `DescargarArchivo?idDet=${idDet}`;
    }
    function getParameters(rowData) {
        let item = {
            cc: rowData.cc,
            idSubContratista: rowData.idSubContratista,
            tipoEvaluacion: btnBuscar.attr('data-idTipoEvaluacion'),
        }
        return item;
    }
    function CargarArchivosXSubcontratista(lstDatos, rowData) {
        let parametros = getParameters(rowData);
        axios.post('CargarArchivosXSubcontratista', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    obtenerPromegioEvaluacion(rowData)

                    for (let i = 0; i < items.length; i++) {
                        $(`#${lstDatos[i].lblInput}`).text(items[i].rutaArchivo);
                        items[i].rutaArchivo != "Ningún archivo seleccionado" ? visualizarArchivoGuardado($(`#${lstDatos[i].lblInput}`), items[i].rutaArchivo, i) : visualizarArchivoNoCargado($(`#${lstDatos[i].lblInput}`));
                        $(`#${lstDatos[i].lblInput}`).attr('data-idEvaluacion', items[i].id);

                        if (items[i].estaEvaluado) {
                            // let btnElemento = $(`#${items[i].btnElemento}`);
                            // if (!btnElemento.hasClass('p-success')) {
                            //     btnElemento.addClass('p-success');
                            // }

                            let tieneSuccess = $(`#btnbutton${lstDatos[i].id}`).hasClass('p-success');
                            if (!tieneSuccess) {
                                $(`#btnbutton${lstDatos[i].id}`).addClass('p-success');
                            }
                        }
                    }
                }
            });
    }
    function obtenerValuesInputs(id) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    fncGuardar(items);
                }
            });
    }
    function CrearFormData(lstDatos) {
        let formData = new FormData();
        let idRow = 0;
        for (let index = 0; index < lstDatos.length; index++) {
            idRow++;
            formData.append("parametros[" + index + "][idEvaluacion]", $(`#${lstDatos[index].lblInput}`).attr('data-idEvaluacion'));
            formData.append("parametros[" + index + "][idRow]", idRow);
            formData.append("parametros[" + index + "][tipoEvaluacion]", btnBuscar.attr('data-id'));
            if (document.getElementById(`${lstDatos[index].inputFile}`).files[0] != null) {
                formData.append("parametros[" + index + "][Archivo]", document.getElementById(`${lstDatos[index].inputFile}`).files[0].name);
                formData.append("Archivo", document.getElementById(`${lstDatos[index].inputFile}`).files[0]);
            }

        }

        return formData;
    }
    function fncGuardar(lstDatos) {
        let parametros = CrearFormData(lstDatos);
        axios.post('addEditSubContratista', parametros, { headers: { 'Content-Type': 'multipart/form-data' } })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
            });
    }
    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }
    function parametrosGuardar() {
        let item = {
            id: btnGuardar.attr('data-idevaluacion'),
            Comentario: inpComentario.val(),
            Calificacion: obtenerCalificacion($('.starrr').attr('data-calificacion')),
        };
        return item;
    }
    function obtenerCalificacion(item) {
        let numero = 0;
        if (item == 1) {
            return numero = 25;
        }
        if (item == 2) {
            return numero = 50;
        }
        if (item == 3) {
            return numero = 70;
        }
        if (item == 4) {
            return numero = 90;
        }
        if (item == 5) {
            return numero = 100;
        }
    }
    function obtenerEvaluacionxReq() {
        let parametros = parametrosGuardar()
        axios.post('obtenerEvaluacionxReq', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        let item = items;
                        inpComentario.val(item.Comentario);
                        inpEvaluacion.val(item.Calificacion);
                        inpPlanDeAccion.val(item.planesDeAccion);
                        inpPlanDeAccion.attr('disabled', 'true');
                        inpResponsable.val(item.responsable);
                        inpResponsable.attr('disabled', 'true');
                        inpFechaCompromiso.val(item.fechaCompromiso);
                        inpFechaCompromiso.attr('disabled', 'true');

                        axios.post('getEstrellas', {})
                            .catch(o_O => AlertaGeneral(o_O.message))
                            .then(response => {
                                let { success, items } = response.data;
                                if (success) {
                                    let estrellas = items;

                                    $('.starrr').starrr({
                                        rating: 0,
                                        change: function (e, value) {


                                            var id = $(e.currentTarget).data("id");
                                            $(e.currentTarget).attr("data-calificacion", value);
                                            if (value <= 2) {
                                                $('[data-id="txtRespuesta' + id + '"]').show();
                                            }
                                            if (value > 2) {
                                                $('[data-id="txtRespuesta' + id + '"]').hide();

                                            }
                                            $(e.currentTarget).find('label').text('');
                                            let etiqueta = $(e.currentTarget).find('label');
                                            etiqueta.text('');
                                            if (value > 0) {
                                                $.each(estrellas, function (index, est) {
                                                    if (est.estrellas == value) {
                                                        etiqueta.text(est.descripcion);
                                                    }
                                                });
                                            } else {
                                                etiqueta.text('');
                                            }
                                        }
                                    });
                                    $('.starrr').find('label').remove();
                                    var etiqueta = document.createElement('label');
                                    etiqueta.style.marginLeft = '10px';
                                    $('.starrr:last').append(etiqueta);
                                    let estrella = $('.starrr').find('a')

                                    switch (item.Calificacion) {
                                        case 100:
                                            $('.starrr').attr('data-calificacion', 5)

                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 5) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-azul');
                                                    $(estrella[1]).addClass('fas fa-star star-azul');
                                                    $(estrella[2]).addClass('fas fa-star star-azul');
                                                    $(estrella[3]).addClass('fas fa-star star-azul');
                                                    $(estrella[4]).addClass('fas fa-star star-azul');
                                                    $('.starrr').find('label').text(est.descripcion);

                                                }
                                            });
                                            break;
                                        case 90:
                                            $('.starrr').attr('data-calificacion', 4)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 4) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-verde');
                                                    $(estrella[1]).addClass('fas fa-star star-verde');
                                                    $(estrella[2]).addClass('fas fa-star star-verde');
                                                    $(estrella[3]).addClass('fas fa-star star-verde');
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[4]).addClass('far fa-star default');
                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 70:
                                            $('.starrr').attr('data-calificacion', 3)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 3) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[1]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[2]).addClass('fas fa-star star-amarillo');
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 50:
                                            $('.starrr').attr('data-calificacion', 2)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 2) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-naranja');
                                                    $(estrella[1]).addClass('fas fa-star star-naranja');
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[2]).addClass('far fa-star default');
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                        case 25:
                                            $('.starrr').attr('data-calificacion', 1)
                                            $.each(estrellas, function (index, est) {
                                                if (est.estrellas == 1) {
                                                    $(estrella[0]).removeClass();
                                                    $(estrella[0]).addClass('fas fa-star star-rojo');
                                                    $(estrella[1]).removeClass();
                                                    $(estrella[2]).removeClass();
                                                    $(estrella[3]).removeClass();
                                                    $(estrella[4]).removeClass();
                                                    $(estrella[1]).addClass('far fa-star default');
                                                    $(estrella[2]).addClass('far fa-star default');
                                                    $(estrella[3]).addClass('far fa-star default');
                                                    $(estrella[4]).addClass('far fa-star default');

                                                    $('.starrr').find('label').text(est.descripcion);
                                                }
                                            });
                                            break;
                                    }
                                }
                            });


                    }
                }
            });
    }
    function GuardarEvaluacion(btnRequerimiento) {
        let parametros = parametrosGuardar()
        axios.post('GuardarEvaluacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.status == 1) {
                        let botonVerdeRequerimiento = $(`#${btnRequerimiento.attr('data-idbtnrequerimiento')}`);
                        if (!botonVerdeRequerimiento.hasClass('p-success')) {
                            botonVerdeRequerimiento.addClass('p-success');
                        }

                        // let botonElemento = $(`#${btnRequerimiento.data('elementoid')}`);
                        // if (!botonElemento.hasClass('p-success')) {
                        //     botonElemento.addClass('p-success');
                        // }

                        Alert2Exito(items.mensaje);
                        $('#mdlComentario').modal('hide');
                        obtenerPromegioEvaluacion(srowData);
                    } else if (items.status == 2) {
                        AlertaGeneral('Hubo un error', items.mensaje);
                    } else {
                        AlertaGeneral('Alerta', items.mensaje);
                    }
                }
            });
    }

    function ponerVerdeRequerimiento(seccion) {

    }

    function parametrosBusquedaPromedio(rowData) {
        let item = {
            cc: rowData.cc,
            tipoEvaluacion: btnBuscar.attr('data-idTipoEvaluacion'),
            idSubContratista: rowData.idSubContratista,
        };
        return item;
    }
    function obtenerPromegioEvaluacion(rowData) {
        let parametros = parametrosBusquedaPromedio(rowData);
        axios.post('obtenerPromegioEvaluacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    srowData = rowData;
                    $('#inpPromedio').val('')
                    $('#inpPromedio').val(items.promedio)
                }
            });
    }
    function obtenerGraficaPastel() {
        let parametros = parametrosGuardar();


        axios.post('ObtenerGraficaDeBarras', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                // CrearGraficasPastel('GraficaReportesPastel', response.data, null, null);
                CrearGraficasPastel('GraficaReportesBarras', response.data, null, null);

            });
        axios.post('ObtenerGraficaDeEvaluacionPorCentroDeCosto', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                // CrearGraficasPastel('GraficaReportesPastel', response.data, null, null);
                gxpGraficaTotal('gpxporCentrosDeCosto', response.data, null, null);

            });
    }
    function obtenerGraficaBarras() {

    }
    function CrearGraficasPastel(gpxGraficaPastel, datos, callback, items) {
        Highcharts.chart(gpxGraficaPastel, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Total'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        // format: '{point.y:.1f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGraficaDeBarras

        });
    }
    function gxpGraficaTotal(grafica, datos, callback, items) {
        Highcharts.chart(grafica, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                min: 0,
                max: datos.numMaximo,
                title: {
                    text: ''
                },
                labels: {
                    format: '{value}'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y %}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b><br/>'
            },
            series: datos.gpxGrafica

        });
    }
    function initDataTblPromedio() {
        dtnombre = $('#tblPromedios').DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: true
            , info: false
            , language: dtDicEsp
            , columns: [

                { data: 'cc', title: 'Centro de costo' },
                { data: 'responsable', title: 'Nombre del subcontratista' },
                {
                    data: 'promedio', title: 'Evaluacion'
                    , render: (data, type, row, meta) => {
                        let html = `
                                <center>${Math.round(data)}</center>
                            `;
                        return html;
                    }
                },
                {
                    data: 'Indicador', title: 'Indicador', render: (data, type, row, meta) => {
                        let html = ``;
                        if (data == 1) {
                            html += `<center><i class="fas fa-star" style='color: #FA0101;'></i></center>`;
                        }
                        if (data == 2) {
                            html += `<center><i class="fas fa-star" style='color: #FA8001;'></i><i class="fas fa-star" style='color: #FA8001;'></i></center>`;
                        }
                        if (data == 3) {
                            html += `<center><i class="fas fa-star" style='color: #FAFF01;'></i><i class="fas fa-star" style='color: #FAFF01;'></i><i class="fas fa-star" style='color: #FAFF01;'></i></center>`;
                        }
                        if (data == 4) {
                            html += `<center><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i><i class="fas fa-star" style='color: #018001;'></i></center>`;
                        }
                        if (data == 5) {
                            html += `<center><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i><i class="fas fa-star" style='color: #0180FF;'></i></center>`;
                        }
                        return html;
                    }
                },
                {
                    title: 'Acciones', render: (data, type, row, meta) => {
                        return `<button class='btn btn-primary btnGrafica'><i class="fas fa-chart-bar"></i></button>`
                    }
                },
            ]
            , initComplete: function (settings, json) {
                $('#tblPromedios').on("click", ".btnGrafica", function () {
                    let rowData = dtnombre.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlGraficasPorSubcontratista.modal('show');
                    ObtenerGraficaDeEvaluacionPorDivisionElemento(rowData.id);
                    ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(rowData.centroCostos, rowData.idSubContratista);
                });
            }
        });
    }
    function obtenerPromediosxSubcontratista() {
        let parametros = parametrosGuardar();
        axios.post('obtenerPromediosxSubcontratista', parametros)
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows($('#tblPromedios'), items);
                }
            });
    }
    function inittblAsignacionEvaluacion() {
        dtAsginacion = $('#tblAsignacionEvaluacion').DataTable({
            destroy: true,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            scrollY: true,
            scrollX: true,
            language: dtDicEsp,
            columns: [
                {
                    data: 'id', title: 'GRÁFICA',
                    render: (data, type, row, meta) => {
                        return `<button class='btn btn-primary btnGrafica'><i class="fas fa-chart-bar"></i></button>`
                    }
                },
                { data: 'id', title: 'id', visible: false },
                { data: 'numeroContrato', title: 'NO. CONTRATO' },
                { data: 'descripcioncc', title: 'CC' },
                { data: 'nombre', title: 'NOMBRE' },
                { data: 'direccion', title: 'DIRECCIÓN', visible: false },
                {
                    data: 'evaluacionAnteriorid', title: 'ANTERIOR',
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (row.status == 3) {
                            html = moment(row.fechaCreacion).format("YYYY-MM-D");
                        } else {
                            if (data != 0) {
                                html = `<button class="btn btn-danger descargarEvaluacionActualPdfAnterior"> <i class="fas fa-file-pdf"></i></button>`;
                            } else {
                                html = ``;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'evaluacionActual', title: 'ACTUAL',
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (data != 0) {
                            html = `<button class="btn btn-danger descargarEvaluacionActualPdf"> <i class="fas fa-file-pdf"></i></button>`;
                        } else {
                            html = ``;
                        }
                        return html;
                    }
                },
                {
                    data: 'estatusAutorizacion', title: ' ',
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (data == 0) {
                            if (row.statusVobo == false) {
                                html = `<button class="btn btn-primary GuardarAsignacion"> <i class="fa fa-save"></i></button>`;
                            } else {
                                html = `<button class="btn btn-success verPlantilla"> <i class="far fa-file"></i></i></button>
                                        <button class="btn btn-warning AutorizarAsignacion"> <i class="fa fa-check"></i></button>`;
                            }

                        } else if (data == 2) {
                            html += `<button class="btn btn-primary Evaluar"> <i class="far fa-edit"></i></button>`;
                            if (row.tipoUsuario != 10) {
                                html += `<button class="btn btn-danger descargarEvaluacionActualPdf"> <i class="fas fa-file-pdf"></i></button>
                                        <button class="btn btn-success autorizarEvaluacion"> <i class="fa fa-check"></i></button>`;
                            }
                        } else {
                            html = `<button class="btn btn-primary descargarEvaluacionActual"> <i class="fa fa-file"></i></button>`;
                        }
                        return html;
                    }
                },
            ], initComplete: function (settings, json) {


                $('#tblAsignacionEvaluacion').on("click", ".verPlantilla", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlPlantillas.modal('show');
                });
                $('#tblAsignacionEvaluacion').on("click", ".btnGrafica", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    //INIT COMPLETE
                    mdlGraficasPorSubcontratista.modal('show');
                    ObtenerGraficaDeEvaluacionPorDivisionElemento(rowData.id);
                    ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(rowData.centroCostos, rowData.idSubContratista);
                });
                $('#tblAsignacionEvaluacion').on("click", ".Evaluar", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    mdlFormulario.modal('show');

                    txtUsuario.text('subcontratista a evaluar : ' + rowData.nombre);
                    txtPeriodoDeFechas.text(rowData.perdiodoFechas);
                    $(`#ContenidoDivicionesModal`).remove();
                    $(`#ConenidoArchivosModal`).remove();
                    CargarConfiguracionVistaModal(rowData);
                    CargarColorVerdeDeNotificado(rowData);
                });
                $('#tblAsignacionEvaluacion').on("click", ".GuardarAsignacion", function () {
                    let strMensaje = "¡Asignar evaluacion!";
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    // Swal.fire({
                    //     position: "center",
                    //     icon: "warning",
                    //     title: "¡Cuidado!",
                    //     width: '35%',
                    //     showCancelButton: true,
                    //     html: `<h3>${strMensaje}</h3>`,
                    //     confirmButtonText: "Confirmar",
                    //     confirmButtonColor: "#5cb85c",
                    //     cancelButtonText: "Cancelar",
                    //     cancelButtonColor: "#d9534f",
                    //     showCloseButton: true
                    // }).then((result) => {
                    //     if (result.value) {
                    //         GuardarAsignacion(rowData);
                    //     }
                    // });
                    Swal.fire({
                        title: strMensaje,
                        icon: "warning",
                        type: 'question',
                        width: '35%',
                        html: `
                                        <label>Fecha Inicial : </label><input id="datepickerInicial" readonly class="form-control" style='background:white;'>
                                        <br>
                                        <label>Fecha Final : </label><input id="datepickerFinal" readonly class="form-control" style='background:white;'>
                                        <br>
                                        <label> Frecuencia de Evaluacion : </label><input type="number" min = "1" max = "50" id="inFreqNum" class="form-control" style='background:white;'>
                                    `,
                        customClass: 'swal2-overflow',
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCancelButton: true,
                        input: "text",

                        onOpen: function () {
                            $('#datepickerInicial').datepicker({
                                dateFormat: 'yy/mm/dd'
                            });
                            $('#datepickerInicial').change(function () {
                                if ($('#datepickerInicial').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                                if ($('#datepickerFinal').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                            })
                            $('#datepickerFinal').datepicker({
                                dateFormat: 'yy/mm/dd'
                            });
                            $('#datepickerFinal').change(function () {
                                if ($('#datepickerFinal').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                                if ($('#datepickerInicial').val() == "") {
                                    $('.swal2-input').val('');
                                } else {
                                    $('.swal2-input').val('fechaInicial');
                                }
                            })
                        },
                        inputValidator: nombre => {
                            if (!nombre) {
                                return "Por favor seleccionar una fecha";
                            } else {
                                return undefined;
                            }
                        },
                    }).then(function (result) {
                        if (result.isConfirmed == true) {
                            GuardarAsignacion(rowData, $('#datepickerInicial').val(), $('#datepickerFinal').val(), $('#inFreqNum').val(), rowData.idPlantilla);
                        } else {
                        }
                    });
                    $('.swal2-input').css('display', 'none');
                });

                $('#tblAsignacionEvaluacion').on("click", ".autorizarEvaluacion", function () {
                    let strMensaje = "¿Desea Autorizar la evaluacion?";
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            AutorizarEvaluacion(rowData);
                        }
                    });
                });

                $('#tblAsignacionEvaluacion').on("click", ".AutorizarAsignacion", function () {
                    let strMensaje = "¿Desea dar Vobo 1 la evaluacion?";
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            AutorizarAsignacion(rowData);
                        }
                    });
                });
                // $('#tblAsignacionEvaluacion').on("click", ".preguntarPrimero", function () {
                //     let rowData = dtAsginacion.row($(this).closest('tr')).data();
                //     abrirModalPreguntarPrimero(rowData);
                // });
                $('#tblAsignacionEvaluacion').on("click", ".descargarEvaluacionActualPdfAnterior", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    var path = `/Reportes/Vista.aspx?idReporte=249&idAsignacion=${rowData.evaluacionAnteriorid}`;
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                });
                $('#tblAsignacionEvaluacion').on("click", ".descargarEvaluacionActualPdf", function () {
                    let rowData = dtAsginacion.row($(this).closest('tr')).data();
                    var path = `/Reportes/Vista.aspx?idReporte=249&idAsignacion=${rowData.idAsignacion}`;
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                });

            }
        });
        $('.dataTables_scrollBody').css('height', '330px')
    }

    function CargarColorVerdeDeNotificado(rowData) {
        axios.post('cambiarDeColor', { idPlantilla: rowData.idPlantilla, idAsignacion: rowData.idAsignacion }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                console.log(items)
                items.forEach(element => {
                    // $('#'+element.idButton).removeClass('p-success');  
                    $('#' + element.idbutton).addClass(element.classe);
                });
            }
        }).catch(error => Alert2Error(error.message));
    }
    function ObtenerContratistas() {
        axios.post('obtenerContratistasConContrato', { AreaCuenta: cboProyecto.val(), subcontratista: cboSubContratista.val() == "" ? 0 : cboSubContratista.val(), Estatus: cboEstatus.val() })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows($('#tblAsignacionEvaluacion'), items);
                    if (cboEstatus.val() === '0') {
                        $('#txtTexto').text('Periodo Evaluacion')
                    } else if (cboEstatus.val() === '2') {
                        $('#txtTexto').text('Editar/Autorizar')
                    }
                }
            });
    }
    function getParametersSubContrat(rowData, fechaInicial, fechaFinal, freqNum, idPlantilla) {
        let item = {
            cc: rowData.cc,
            idSubContratista: rowData.idSubContratista,
            idAsignacion: rowData.idAsignacion,
            lstRelacion: getParametersPreguntar(),
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal,
            freqEval: Number(freqNum),
            idPlantilla: idPlantilla

        };
        return item;
    }
    function GuardarAsignacion(rowData, fechaInicial, fechaFinal, freqNum, idPlantilla) {
        let parametros = getParametersSubContrat(rowData, fechaInicial, fechaFinal, freqNum, idPlantilla);
        axios.post('GuardarAsignacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ObtenerContratistas();
                    mdlPreguntarPrimero.modal('hide');
                }
            });
    }
    function getAutorizarEvaluacion(rowData) {
        let item = {
            id: rowData.idAsignacion
        };
        return item;
    }
    function AutorizarEvaluacion(rowData) {
        let parametros = getAutorizarEvaluacion(rowData);
        axios.post('AutorizarEvaluacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ObtenerContratistas();
                }
            });
    }

    function AutorizarAsignacion(rowData) {
        let parametros = getAutorizarEvaluacion(rowData);
        axios.post('AutorizarAsignacion', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ObtenerContratistas();
                }
            });
    }

    function abrirModalPreguntarPrimero(rowData) {
        mdlPreguntarPrimero.modal('show');
        axios.post('obtenerTodosLosElementosConSuRequerimiento')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    ImprimirTodoPreguntarPrimero(items, rowData);
                }
            });
    }
    function ImprimirTodoPreguntarPrimero(items, rowData) {
        datosAGuardar = {};
        datosAGuardar = rowData;
        containerPreguntarPrimero.find('div').remove();
        let html = '<div id="ContenidoPreguntaPrim">';
        items.forEach(x => {
            if (x.Aparece == true) {
                html += x.descripcion + `<br>`
                x.lstRequerimientos.forEach(y => {
                    html += `&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<input data-idreq='${y.id}' type='checkbox'/>&nbsp&nbsp` + y.texto + `<br>`
                });
            }
        });
        html += '</div>'
        containerPreguntarPrimero.append(html);
    }
    function getParametersPreguntar() {
        let paramet = [];
        let input = $('#ContenidoPreguntaPrim').find('input');
        for (let index = 0; index < input.length; index++) {
            let items = {
                idSubContratista: datosAGuardar.id,
                Preguntar: $(input[index]).prop('checked'),
                idReq: $(input[index]).attr('data-idreq'),
            }
            paramet.push(items);
        }
        return paramet;
    }
    function fncGuardarPreguntarPrimero() {
        GuardarAsignacion(datosAGuardar);
    }
    function cargandoModal(lstDatos, idsection, rowData) {
        CargarArchivosXSubcontratista(lstDatos, rowData)
        obtenerFuncion();
    }
    function CargarConfiguracionVistaModal(rowData) {
        axios.post('obtenerDivicionesEvaluadorArchivos', { idPlantilla: rowData.idPlantilla, idAsignacion: rowData.id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                console.log(items)
                if (success) {
                    obtenerVisibles(items, rowData)
                }
            });
    }
    function obtenerVisibles(items2, rowData) {
        let dn = [];
        axios.post('obtenerElementosEvaluar', { idPlantilla: rowData.idPlantilla })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                console.log(items.items)
                if (success) {
                    if (items.items != "" && items.items != undefined) {
                        let item = items.items.split(',');
                        item.forEach(x => {
                            if (x != "") {
                                dn.push(x);
                            }
                        });
                    }
                    fncGenerandoDivicionVistaModal(items2, rowData, dn);
                }
            });
    }
    function fncGenerandoDivicionVistaModal(lstDatos, rowData, dn) {
        let elementoInicial = null;

        let html = `   <div class="col-lg-2 col-md-2 col-sm-2" style="margin-top:15px;" id='ContenidoDivicionesModal'>
                            <div class=" row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 0;">
                                    <fieldset class="fieldset-custm">
                                        <legend class="legend-custm">Elementos : </legend>`;
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {

                if (lstDatos[index].id == dn[v]) {
                    if (elementoInicial == null) {
                        elementoInicial = lstDatos[index].idbutton;
                    }
                    html += `<button class="btn btn-block btn-social btn-primary-menu boton-menu" id="${lstDatos[index].idbutton}" data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}">
                                                <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                                            </button>`;
                }

            }
        }

        html += ` 
                <fieldset class="fieldset-custm" >
                    <legend class="legend-custm">Indicadores : </legend>
                    <button class="btn btn-block boton-estatus "> 0 - 25
                     Pesimo<br> <i class="fa fa-star color-pesimo"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 26 - 50
                        Malo<br> <i class="fa fa-star color-malo"></i><i class="fa fa-star color-malo"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 51 - 75
                        Regular<br> <i class="fa fa-star color-regular"></i><i class="fa fa-star color-regular"></i><i class="fa fa-star color-regular"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 76 - 90
                        Aceptable<br> <i class="fa fa-star color-aceptable"></i><i class="fa fa-star color-aceptable"></i><i class="fa fa-star color-aceptable"></i><i class="fa fa-star color-aceptable"></i>
                    </button>
                    <button class="btn btn-block boton-estatus"> 91 - 100
                        Excediendo las expectativas <br><i class="fa fa-star color-excediendo"></i><i class="fa fa-star color-excediendo"></i><i class="fa fa-star color-excediendo"></i><i class="fa fa-star color-excediendo"></i><i class="fa fa-star color-excediendo"></i>
                    </button>
                </fieldset>
                            </div>
                        </div>
                    </div>`;
        html += `<div class="col-lg-10 col-md-10 col-sm-10" id='ConenidoArchivosModal'>`;
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {

                if (lstDatos[index].id == dn[v]) {
                    html += `
                                <div id="${lstDatos[index].idsection}"  class="col-md-12">
                                    <fieldset class="fieldset-custm">
                                        <legend class="legend-custm">${lstDatos[index].descripcion} : <span id="lblErrorProyecto"></span></legend>
                                        <div id="${lstDatos[index].idsection + lstDatos[index].id}" ></div>
                                    </fieldset>
                                </div>`;
                }
            }

        }
        html += ` 
                <div class="row">
                    <div class='col-md-6 col-lg-6'>
                        <div class='input-group'>
                            <span class='input-group-addon'>Calificacion Global</span>
                            <input type='text' id='inpCalificacionGlobal' class='form-control'>
                        </div>
                    </div>
                </div>
                
                </div>`;
        contenido23.append(html);

        let lstARr = [];
        for (let index = 0; index < lstDatos.length; index++) {
            for (let v = 0; v < dn.length; v++) {
                if (lstDatos[index].id == dn[v]) {
                    lstARr.push(lstDatos[index])
                }
            }
        }
        obtenerPromedioGeneral(rowData.idAsignacion);

        for (let index = 0; index < lstARr.length; index++) {
            $(`#${lstARr[index].idsection}`).css('display', 'none');
            $(`#${lstARr[index].idbutton}`).click(function () {
                $('#contenidoPromedio').remove();
                $('#contenidoPromedio2').remove();
                CargarColorVerdeDeNotificado(rowData);
                btnGuardar.data('elementoid', lstARr[index].idbutton);
                for (let i = 0; i < lstARr.length; i++) {
                    if (lstARr[index].idbutton == lstARr[i].idbutton) {
                        $(`#${lstARr[i].idsection}`).css('display', 'block');
                        btnBuscar.attr('data-idTipoEvaluacion', lstARr[i].id);
                        ObtenerReqModal(lstARr[index].id, lstARr[index].idsection + lstARr[index].id, rowData, dn, $(this));
                    } else {
                        $(`#${lstARr[i].idsection}`).css('display', 'none');
                    }

                }
            })
        }
        if (lstARr.length != 0) {
            $(`#${lstARr[0].idsection}`).css('display', 'block');
            btnBuscar.attr('data-id', lstARr[0].id);
            btnBuscar.attr('data-idTipoEvaluacion', lstARr[0].id);
            ObtenerReqModal(lstARr[0].id, lstARr[0].idsection + lstARr[0].id, rowData, dn, $(`#${elementoInicial}`));
        }

        if (lstARr.length > 0) {
            btnGuardar.data('elementoid', lstARr[0].idbutton);
        }
    }
    function obtenerPromedioGeneral(id) {
        axios.post('obtenerPromedioGeneral', { id: id }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                $('#inpCalificacionGlobal').val(items);
            }
        }).catch(error => Alert2Error(error.message));
    }
    function ObtenerReqModal(id, idsection, rowData, dn, btnEvaluacion) {
        axios.post('obtenerRequerimientos', { idDiv: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    console.log(items)
                    GenerarInputModal(items, idsection, rowData, dn, btnEvaluacion);
                }
            });
    }
    function ObtenerGraficaDeEvaluacionPorDivisionElemento(id) {
        let parametros = {
            id: id
        };
        axios.post('ObtenerGraficaDeEvaluacionPorDivisionElemento', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    gpxGraficaSubxCa('gpxGraficaSubxCalificacion', response.data, null, null);
                }
            });
    }
    function gpxGraficaSubxCa(gpxGraficaSubxCalificacion, datos, callback, items) {
        Highcharts.chart(gpxGraficaSubxCalificacion, {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Total'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGrafica

        });
    }
    function ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(cc, idSubContratista) {
        let parametros = {
            cc: cc,
            idSubContratista: idSubContratista,
        }
        axios.post('ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    gpxObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones('gpxSubPorCalificacionMensual', response.data, null, null);
                }
            });
    }
    function gpxObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(gpxSubPorCalificacionMensual, datos, callback, items) {
        let categorias = [];
        datos.gpxGrafica[0].data.forEach(x => {
            categorias.push(x.name)
        });

        Highcharts.chart(gpxSubPorCalificacionMensual, {

            title: {
                text: ''
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                categories: categorias,
                type: 'Meses'
            },
            yAxis: {
                min: 0,
                max: 100,
                title: {
                    text: 'Promedio de evaluaciones'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },
            series: datos.gpxGrafica

        });
    }



    $(document).ready(() => {
        subContratistas.ControlObra = new ControlObra();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();