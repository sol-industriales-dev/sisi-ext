(() => {
$.namespace('MatrizDeRiesgo.ControlObra');
    
    const cboProyecto = $('#cboProyecto');
    const tblMatrizDeRiesgo = $('#tblMatrizDeRiesgo');
    let dttblMatrizDeRiesgo;
    const mdlMatrizDeRiesgo = $('#mdlMatrizDeRiesgo');
    const btnNuevo = $('#btnNuevo');
    const btnSave = $('#btnSave');
    const btnBuscar = $('#btnBuscar');
    const report = $('#report');
    const btnRegresar = $('#btnRegresar');

    const cboContratos = $('#cboContratos');
    const cboCC = $('#cboCC');
    const inpElaboro = $('#inpElaboro');
    const inpFaseDeProyecto = $('#inpFaseDeProyecto');
    const inpNoProyecto = $('#inpNoProyecto');
    const dtFecha = $('#dtFecha');
    const inpBaja = $('#inpBaja');
    const inpBajaFin = $('#inpBajaFin');
    const inpMedia = $('#inpMedia');
    const inpMediaFin = $('#inpMediaFin');
    const inpAlta = $('#inpAlta');
    const inpAltaFin = $('#inpAltaFin');
    const fechaActual = new Date();

    const tblEscalaDeSeveridad = $('#tblEscalaDeSeveridad');
    const tblEscalaDeSeveridad2 = $('#tblEscalaDeSeveridad2');
    const contenedorTablaTipos = $('#contenedorTablaTipos');
    const conteniendoDeTabla = $('#conteniendoDeTabla');

    const inpTiempoBaja = $('#inpTiempoBaja');
    const inpCostoBaja = $('#inpCostoBaja');
    const inpCalidadBaja = $('#inpCalidadBaja');
    const inpTiempoMedia = $('#inpTiempoMedia');
    const inpCostoMedia = $('#inpCostoMedia');
    const inpCalidadMedia = $('#inpCalidadMedia');
    const inpTiempoAlta = $('#inpTiempoAlta');
    const inpCostoAlta = $('#inpCostoAlta');
    const inpCalidadAlta = $('#inpCalidadAlta');

    const inpTiempoBaja2 = $('#inpTiempoBaja2');
    const inpCostoBaja2 = $('#inpCostoBaja2');
    const inpCalidadBaja2 = $('#inpCalidadBaja2');
    const inpBaja2 = $('#inpBaja2');
    const inpBajaFin2 = $('#inpBajaFin2');
    const inpTiempoMedia2 = $('#inpTiempoMedia2');
    const inpCostoMedia2 = $('#inpCostoMedia2');
    const inpCalidadMedia2 = $('#inpCalidadMedia2');
    const inpMedia2 = $('#inpMedia2');
    const inpMediaFin2 = $('#inpMediaFin2');
    const inpTiempoAlta2 = $('#inpTiempoAlta2');
    const inpCostoAlta2 = $('#inpCostoAlta2');
    const inpCalidadAlta2 = $('#inpCalidadAlta2');
    const inpAlta2 = $('#inpAlta2');
    const inpAltaFin2 = $('#inpAltaFin2');

    const btnReporte = $('#btnReporte');


    const tblMatrizDetallado = $('#tblMatrizDetallado');
    let dtMatrizDetallado;
    const contenedorTabla = $('#contenedorTabla');
    // const mdlDetalleMatrizDeRiesgo = $('#mdlDetalleMatrizDeRiesgo');
    const mdlAgregarDetalle = $('#mdlAgregarDetalle');
    const btnGenerarDetalle = $('#btnGenerarDetalle');

    const btnSaveDetalle = $('#btnSaveDetalle');

    const inpAmenazaOportunidad =$('#inpAmenazaOportunidad');
    const inpCategoriaDelRiesgo =$('#inpCategoriaDelRiesgo');
    const inpCausaBasica =$('#inpCausaBasica');
    const inpAreaDelProyecto =$('#inpAreaDelProyecto');
    const cmbImpacto =$('#cmbImpacto');
    const inpProbabilidad =$('#inpProbabilidad');
    const inpImpacto =$('#inpImpacto');
    const inpSeveridadInicial =$('#inpSeveridadInicial');
    const inpSeveridadActual =$('#inpSeveridadActual');
    const cboDueñoDelRiesgo =$('#cboDueñoDelRiesgo');
    const dtFechaCompromiso =$('#dtFechaCompromiso');
    const cboTipoRespuesta =$('#cboTipoRespuesta');
    const txtMedidasATomar =$('#txtMedidasATomar');
    const cboAbiertoCerrado = $('#cboAbiertoCerrado');
    const contenidoInformativo = $('#contenidoInformativo');

    const btnTiposRespuestaRiesgo = $('#btnTiposRespuestaRiesgo');
    const btnImpactoSobreProyecto = $('#btnImpactoSobreProyecto');
    const btnGrafica = $('#btnGrafica');

    const contenedoEscala = $('#contenedoEscala');
    const contenedorEscalaSev = $('#contenedorEscalaSev');

    const textolblMtz = $('#textolblMtz');
        var lstDetalle = [];

    const cboEstatus = $('#cboEstatus');

    //#region Modal filtrar reporte

    const mdlFilterEstatus = $('#mdlFilterEstatus');
    const cboEstatusRerpote = $('#cboEstatusRerpote');
    const btnFilterReportSI = $('#btnFilterReportSI');
    const btnFilterReportNO = $('#btnFilterReportNO');

    //#endregion
    
    ControlObra = function (){
        let init = () => {
            inpBaja.val(1);
            inpBajaFin.val(6);
            inpMedia.val(7);
            inpMediaFin.val(12);
            inpAlta.val(13);
            inpAltaFin.val(25);
            initDatatblMatrizDeRiesgo();
            initTblMatrizDetallado();
            eventListeners();
            MatrizTexteo();
            obtenerChangesMatrices(); 
        }
        init();
    }
    function obtenerChangesMatrices() {
        inpBaja.change(function () {
            MatrizTexteo();
        })
        inpBajaFin.change(function () {
            MatrizTexteo();
        })
        inpMedia.change(function () {
            MatrizTexteo();
        })
        inpMediaFin.change(function () {
            MatrizTexteo();
        })
        inpAlta.change(function () {
            MatrizTexteo();
        })
        inpAltaFin.change(function () {
            MatrizTexteo();
        })
    }
    function MatrizTexteo() {
        let tr = tblEscalaDeSeveridad.find('tr');
        let th0 = $(tr[0]).find('th');
        for (let a = 1; a < tr.length; a++) {
            let th = $(tr[a]).find('th');
            for (let b = 0; b < th.length -1; b++) {
                let suma = ( $(th0[b]).text() * $(th[5]).text() ); 
                $(th[b]).text('')
                $(th[b]).text(suma)
                if (suma >= inpBaja.val() && suma <= inpBajaFin.val() ) {
                    $(th[b]).css('background-color','#00b050');
                }else if (suma >= inpMedia.val() && suma <= inpMediaFin.val() ) {
                    $(th[b]).css('background-color','#ffc000');
                }else if (suma >= inpAlta.val() && suma <= inpAltaFin.val() ) {
                    $(th[b]).css('background-color','#ed2937');
                }else{
                    $(th[b]).css('background-color','#fff');
                }
            }            
        }
        let tr2 = tblEscalaDeSeveridad2.find('tr');
        let th02 = $(tr2[0]).find('th');
        for (let a = 1; a < tr2.length; a++) {
            let th = $(tr2[a]).find('th');
            for (let b = 0; b < th.length -1; b++) {
                let suma = ( $(th02[b]).text() * $(th[5]).text() ); 
                $(th[b]).text('')
                $(th[b]).text(suma)
                if (suma >= inpBaja.val() && suma <= inpBajaFin.val() ) {
                    $(th[b]).css('background-color','#00b050');
                }else if (suma >= inpMedia.val() && suma <= inpMediaFin.val() ) {
                    $(th[b]).css('background-color','#ffc000');
                }else if (suma >= inpAlta.val() && suma <= inpAltaFin.val() ) {
                    $(th[b]).css('background-color','#ed2937');
                }else{
                    $(th[b]).css('background-color','#fff');
                }
            }            
        }
    }
    function agregarDetallado(){
        console.log(moment(fechaActual).format("DD/MM/YYYY"))
        console.log(lstDetalle)
        let no = (lstDetalle === null ? 0 : lstDetalle.length) + 1;
        if (lstDetalle === null) {
            lstDetalle = [];
        }
        console.log(no)
        let item = {
            id : 0,
            idMatrizDeRiesgo : 0,
            Historial : 0,
            No : no,
            chAmenzaOportunidad : false,
            amenazaOportunidad : '',
            categoriaDelRiesgo : '',
            descategoriaDelRiesgo : '',
            causaBasica : '',
            areaDelProyecto : '',
            costoTiempoCalidad : 1,
            probabilidad : 1,
            impacto : 1,
            severidadInicial : 1,
            severidadActual : 1,
            tipoDeRespuesta : '',
            desctipoDeRespuesta : '',
            medidasATomar :'',
            dueñoDelRiesgo : '',
            fechaDeCompromiso : moment(fechaActual).format("DD/MM/YYYY"),
            abiertoProcesoCerrado : 1,
        };
        
        lstDetalle.push(item);
        dtMatrizDetallado.clear();
        dtMatrizDetallado.rows.add(lstDetalle);
        dtMatrizDetallado.draw();
    }
    function eventListeners() {
        btnTiposRespuestaRiesgo.click(function () {
            mdlAgregarDetalle.modal('show');
            contenedoEscala.css('display','none');
            contenedorEscalaSev.css('display','none');
            contenedorTablaTipos.css('display','block');
            llenarTablaD();
        });
        btnImpactoSobreProyecto.click(function () {
            mdlAgregarDetalle.modal('show');
            contenedoEscala.css('display','block');
            contenedorEscalaSev.css('display','none');
            contenedorTablaTipos.css('display','none');
        });
        btnGrafica.click(function () {
            mdlAgregarDetalle.modal('show');
            contenedoEscala.css('display','none');
            contenedorEscalaSev.css('display','block');
            contenedorTablaTipos.css('display','none');
        });
        btnRegresar.click(function () {
            mdlAgregarDetalle.modal('hide');
        })
        dtFecha.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaActual);
        dtFechaCompromiso.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaActual);
        
        btnNuevo.click(function () {
            limpiarArchivos();
            btnSave.attr('data-id',0);
            mdlMatrizDeRiesgo.modal('show');
            textolblMtz.text('Nueva Matriz de riesgo');
                    btnGenerarDetalle.css('display','none');
                    contenedorTabla.css('display','none');
                    contenidoInformativo.css('display','block');
        });
        btnGenerarDetalle.click(function () {
            // mdlAgregarDetalle.modal('show');
            agregarDetallado();
        })
        btnReporte.click(function(){
            fncReporte(btnReporte.attr('data-id'),cboEstatus.val());
        });
        cboProyecto.fillCombo('/ControlObra/ControlObra/getProyecto', null, false, null); 
        cboCC.fillCombo('/ControlObra/ControlObra/TraermeTodosLosCC', null, false, null); 
        // inpCategoriaDelRiesgo.fillCombo('/ControlObra/ControlObra/cbolstMrCategorias', null, false, null);
        // inpCategoriaDelRiesgo.select2({dropdownParente: $(mdlAgregarDetalle) });
        // cboTipoRespuesta.fillCombo('/ControlObra/ControlObra/cboTiposDeRespuestas', null, false, null);
        // cboTipoRespuesta.select2({dropdownParente: $(mdlAgregarDetalle) });
        // cboDueñoDelRiesgo.fillCombo('/ControlObra/ControlObra/cboResponsables', null, false, null);
        // cboDueñoDelRiesgo.select2({dropdownParente: $(mdlAgregarDetalle) });
        
        // cboContratos.fillCombo('/ControlObra/ControlObra/obtenerContratos', null, false, null); 
        traermeElaboro();
        // cboContratos.change(function () {
        //     let cc = cboContratos.find('option:selected').text().split('-')[0];
        //     cboCC.val(cc.trim());
        //     cboCC.trigger("change");
        // })

        cboProyecto.change(function () {
            var table = tblMatrizDeRiesgo.DataTable();
            if (cboProyecto.val()!='') {
                table.columns( [0] ).visible( false, false );
            }else{
                table.columns( [0] ).visible( true, true );
            }
            obtenerMatrizDeRiesgo();
        });
        btnNuevo.click(function () {
            btnSave.attr('data-editar',false);
            btnTiposRespuestaRiesgo.css('display','none');
            btnImpactoSobreProyecto.css('display','none');
            btnGrafica.css('display','none');
            mdlMatrizDeRiesgo.modal('show');
        });
        btnSave.click(function () {
            GuardarEditarMatriz();
            // console.log(dtMatrizDetallado.data().toArray());
            
        });
        btnBuscar.click(function () {
            obtenerMatrizDeRiesgo();
        });
        btnSaveDetalle.click(function () {
            AgregarAlDetalle();
        });
        inpImpacto.change(function () {
            sumatoria(); 
        });
        inpProbabilidad.change(function () {
            sumatoria(); 
        });

        cboEstatus.attr('multiple', true);
        convertToMultiselect('#cboEstatus');

        cboEstatus.on("change", function(){

            // let lst = $.grep(lstDetalle, function(n,index){ return cboEstatus.val().includes(n.abiertoProcesoCerrado == 0 ? 'Cerrado' : 
            //     n.abiertoProcesoCerrado == 1 ? 'Abierto':'Proceso') ; });
            let lst = $.grep(lstDetalle, function(n,index){ if (cboEstatus.val().includes(`${n.abiertoProcesoCerrado}`)) {return n} });    
            
            dtMatrizDetallado.clear();
            dtMatrizDetallado.rows.add(lst);
            dtMatrizDetallado.draw();
            
        });
        
    }
    function limpiarArchivos() {
        cboEstatus.multiselect('selectAll', false);
        cboEstatus.multiselect('refresh');
        cboEstatus.multiselect('deselect', 'Todos');
        inpBaja.val(1);
        inpBajaFin.val(6);
        inpMedia.val(7);
        inpMediaFin.val(12);
        inpAlta.val(13);
        inpAltaFin.val(25);
        inpTiempoBaja.val('');
        inpCostoBaja.val('');
        inpCalidadBaja.val('');
        inpTiempoMedia.val('');
        inpCostoMedia.val('');
        inpCalidadMedia.val('');
        inpTiempoAlta.val('');
        inpCostoAlta.val('');
        inpCalidadAlta.val('');
        inpElaboro.val('');
        inpFaseDeProyecto.val('');
        inpNoProyecto.val('');
    }
    function traermeElaboro() {
        axios.post('QuienElaboro').then(response => {
            let { success, items, message } = response.data;
                    if (success) {
                    inpElaboro.val(items[0].Text);
                    inpElaboro.attr('data-id',items[0].Value);
                }
        }).catch(error => Alert2Error(error.message));
    }
    function sumatoria() {
        let valor1 = inpProbabilidad.val()==''?0:inpProbabilidad.val();
        let valor2 = inpImpacto.val()==''?0:inpImpacto.val();
        let suma = 0;

        suma = ( parseInt(valor1)*parseInt(valor2))

        inpSeveridadInicial.val(suma)
    }
    function initDatatblMatrizDeRiesgo() {
        dttblMatrizDeRiesgo = tblMatrizDeRiesgo.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,paging: true
            ,ordering:true
            ,searching: true
            ,bFilter: true
            ,info: true
            ,columns: [
                { data: 'cc', title: 'Centro de costo' },
                { data: 'nombreDelProyecto', title: 'Nombre proyecto' },
                { data: 'personalElaboro', title: 'Persona elaboro' },
                { data: 'faseDelProyecto', title: 'Fase del proyecto' },
                { data: 'fechaElaboracion', title: 'Fecha elaboracion' ,render: (data, type, row, meta) => {
                    let html = moment(data).format("DD/MM/YYYY");
                    return html;
                    }
                },
                {  title: 'Acciones' ,render: (data, type, row, meta) => {
                    let html = `<button class='btn btn-primary btn-sm editarMatriz'><i class='glyphicon glyphicon-pencil'></i></button>
                    `;
                    // <button class='btn btn-warning btn-sm reporte'><i class='glyphicon glyphicon-pencil'></i></button>
                    return html;
                    }
                },
            ]
            ,initComplete: function (settings, json) {
                
                tblMatrizDeRiesgo.on("click", ".editarMatriz", function () {
                    const rowData = dttblMatrizDeRiesgo.row($(this).closest("tr")).data(); 
                    cboEstatus.multiselect('selectAll', false);
                    cboEstatus.multiselect('refresh');
                    cboEstatus.multiselect('deselect', 'Todos');
                    btnSave.attr('data-editar',true);
                    mdlMatrizDeRiesgo.modal('show');
                    contenedorTabla.css('display','block');
                    btnGenerarDetalle.css('display','block');
                    btnSave.attr('data-id',rowData.id);
                    contenidoInformativo.css('display','none');
                    textolblMtz.text('Editar Matriz de riesgo');
                    btnTiposRespuestaRiesgo.css('display','block');
                    btnImpactoSobreProyecto.css('display','block');
                    btnGrafica.css('display','block');
                    btnReporte.attr('data-id',rowData.id);
                    obtenerDetalleDatos(rowData); 
                });
                tblMatrizDeRiesgo.on("click", ".reporte", function () {
                    const rowData = dttblMatrizDeRiesgo.row($(this).closest("tr")).data();
                    btnReporte.attr('data-id',rowData.id);
                    //fncReporte(rowData.id);
                });
            }
        });
    }
    function fncReporte(idMatrizDeRiesgo,lstFiltro) {
        if(lstFiltro != undefined && lstFiltro != null){
            report.attr(`src`,`/Reportes/Vista.aspx?idReporte=247&idMatrizDeRiesgo=${idMatrizDeRiesgo}&isCRModal=${true}&filterCerrado=${lstFiltro.includes("0")}&filterAbierto=${lstFiltro.includes("1")}&filterProceso=${lstFiltro.includes("2")}`); 

        }else{
            report.attr(`src`,`/Reportes/Vista.aspx?idReporte=247&idMatrizDeRiesgo=${idMatrizDeRiesgo}&isCRModal=${true}`);
        }
        //report.attr(`src`,`/Reportes/Vista.aspx?idReporte=247&idMatrizDeRiesgo=${idMatrizDeRiesgo}&isCRModal=${true}&filterCerrado=${lstFiltro.includes("0")}&filterAbierto=${lstFiltro.includes("1")}&filterProceso=${lstFiltro.includes("2")}`); 

        // &filterCerrado=${lstFiltro.includes("0")}&filterAbierto=${lstFiltro.includes("1")}&filterProceso=${lstFiltro.includes("2")}
        document.getElementById('report').onload = function () {
        $.unblockUI();
        openCRModal();
        }
    }

    function obtenerDetalleDatos(rowData) {

        inpNoProyecto.val(rowData.nombreDelProyecto);
        inpFaseDeProyecto.val(rowData.faseDelProyecto);
        inpElaboro.val(rowData.personalElaboro)
        cboCC.val(rowData.cc.split('-')[0].trim());
        cboCC.trigger("change");
        dtFecha.val(moment(rowData.fechaElaboracion).format("DD/MM/YYYY"));
        if (rowData.lstImpacto != null) {
            
            if (rowData.lstImpacto[0].tiempo != null) {inpTiempoBaja.val(rowData.lstImpacto[0].tiempo);}
            if (rowData.lstImpacto[0].costo != null) {inpCostoBaja.val(rowData.lstImpacto[0].costo);}
            if (rowData.lstImpacto[0].calidad != null) {inpCalidadBaja.val(rowData.lstImpacto[0].calidad);}
            if (rowData.lstImpacto[0].baja != null) {inpBaja.val(rowData.lstImpacto[0].baja);}
            if (rowData.lstImpacto[0].bajaFin != null) {inpBajaFin.val(rowData.lstImpacto[0].bajaFin);}

            if (rowData.lstImpacto[1].tiempo != null) {inpTiempoMedia.val(rowData.lstImpacto[1].tiempo);}
            if (rowData.lstImpacto[1].costo != null) {inpCostoMedia.val(rowData.lstImpacto[1].costo);}
            if (rowData.lstImpacto[1].calidad != null) {inpCalidadMedia.val(rowData.lstImpacto[1].calidad);}
            if (rowData.lstImpacto[1].baja != null) {inpMedia.val(rowData.lstImpacto[1].baja);}
            if (rowData.lstImpacto[1].bajaFin != null) {inpMediaFin.val(rowData.lstImpacto[1].bajaFin);}
        
            if (rowData.lstImpacto[2].tiempo != null) {inpTiempoAlta.val(rowData.lstImpacto[2].tiempo);}
            if (rowData.lstImpacto[2].costo != null) {inpCostoAlta.val(rowData.lstImpacto[2].costo);}
            if (rowData.lstImpacto[2].calidad != null) {inpCalidadAlta.val(rowData.lstImpacto[2].calidad);}
            if (rowData.lstImpacto[2].baja != null) {inpAlta.val(rowData.lstImpacto[2].baja);}
            if (rowData.lstImpacto[2].bajaFin != null) {inpAltaFin.val(rowData.lstImpacto[2].bajaFin);}

            if (rowData.lstImpacto[0].tiempo != null) {inpTiempoBaja2.val(rowData.lstImpacto[0].tiempo);}
            if (rowData.lstImpacto[0].costo != null) {inpCostoBaja2.val(rowData.lstImpacto[0].costo);}
            if (rowData.lstImpacto[0].calidad != null) {inpCalidadBaja2.val(rowData.lstImpacto[0].calidad);}
            if (rowData.lstImpacto[0].baja != null) {inpBaja2.val(rowData.lstImpacto[0].baja);}
            if (rowData.lstImpacto[0].bajaFin != null) {inpBajaFin2.val(rowData.lstImpacto[0].bajaFin);}

            if (rowData.lstImpacto[1].tiempo != null) {inpTiempoMedia2.val(rowData.lstImpacto[1].tiempo);}
            if (rowData.lstImpacto[1].costo != null) {inpCostoMedia2.val(rowData.lstImpacto[1].costo);}
            if (rowData.lstImpacto[1].calidad != null) {inpCalidadMedia2.val(rowData.lstImpacto[1].calidad);}
            if (rowData.lstImpacto[1].baja != null) {inpMedia2.val(rowData.lstImpacto[1].baja);}
            if (rowData.lstImpacto[1].bajaFin != null) {inpMediaFin2.val(rowData.lstImpacto[1].bajaFin);}
        
            if (rowData.lstImpacto[2].tiempo != null) {inpTiempoAlta2.val(rowData.lstImpacto[2].tiempo);}
            if (rowData.lstImpacto[2].costo != null) {inpCostoAlta2.val(rowData.lstImpacto[2].costo);}
            if (rowData.lstImpacto[2].calidad != null) {inpCalidadAlta2.val(rowData.lstImpacto[2].calidad);}
            if (rowData.lstImpacto[2].baja != null) {inpAlta2.val(rowData.lstImpacto[2].baja);}
            if (rowData.lstImpacto[2].bajaFin != null) {inpAltaFin2.val(rowData.lstImpacto[2].bajaFin);}
            
        }


        lstDetalle = rowData.lstMatrizDeRiesgo;
        if (lstDetalle != null ) {
            dtMatrizDetallado.clear();
            dtMatrizDetallado.rows.add(lstDetalle);
            dtMatrizDetallado.draw();
        }else{
            dtMatrizDetallado.clear();
            dtMatrizDetallado.draw();            
        }
        MatrizTexteo();
    }
    function obtenerMatrizDeRiesgo() {
        axios.post('obtenerMatrizesDeRiesgo', {variable:cboProyecto.val()})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    AddRows(tblMatrizDeRiesgo,items.items)
                }
            });
    }
    function getParameters() {
        let lstDetalle = dtMatrizDetallado.data().toArray();

        lstDetalle.forEach(x => {
            if (x.fechaDeCompromiso.split('/')[1].split('(')[0] == 'Date') {
                x.fechaDeCompromiso = moment(x.fechaDeCompromiso).format("DD/MM/YYYY");
            }else{
                x.fechaDeCompromiso = x.fechaDeCompromiso;
            }
        });


        let parametros = {
            id: btnSave.attr('data-id'),
            idContrato: cboProyecto.val(),
            cc: cboCC.find('option:selected').text().trim(),
            nombreDelProyecto: inpNoProyecto.val(),
            faseDelProyecto: inpFaseDeProyecto.val(),
            personajeElaboro: inpElaboro.val(),
            fechaElaboracion: dtFecha.val(),
            baja: inpBaja.val(),
            bajaFin: inpBajaFin.val(),
            media: inpMedia.val(),
            mediaFin: inpMediaFin.val(),
            alta: inpAlta.val(),
            altaFin: inpAltaFin.val(),
            tiempoBaja : inpTiempoBaja.val(),
            costoBaja : inpCostoBaja.val(),
            calidadBaja : inpCalidadBaja.val(),
            tiempoMedia : inpTiempoMedia.val(),
            costoMedia : inpCostoMedia.val(),
            calidadMedia : inpCalidadMedia.val(),
            tiempoAlta : inpTiempoAlta.val(),
            costoAlta : inpCostoAlta.val(),
            calidadAlta : inpCalidadAlta.val(),
            lstDetalleGuardado : lstDetalle,
        }
        return parametros;
    }
    function initTblMatrizDetallado() {
        dtMatrizDetallado = tblMatrizDetallado.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            scrollY: true,
            scrollX: true,
            drawCallback: function( settings ) {                 
                tblMatrizDetallado.find('input.chAmenzaOportunidad').bootstrapToggle();
                tblMatrizDetallado.on( function () {
                
                });
                tblMatrizDetallado.find('select.inpCategoriaDelRiesgo').fillCombo('/ControlObra/ControlObra/cbolstMrCategorias', null, false, null);
                tblMatrizDetallado.find('select.inpCategoriaDelRiesgo').select2();

                tblMatrizDetallado.find('select.dueñoDelRiesgo').fillCombo('/ControlObra/ControlObra/cboResponsables', null, false, null);
                tblMatrizDetallado.find('select.dueñoDelRiesgo').select2();
                var api = this.api();
                let tm = api.rows( {page:'current'} ).data();
                for (let index = 0; index < tm.length; index++) {
                   
                    let selectCateg = tblMatrizDetallado.find('select.inpCategoriaDelRiesgo')[index]
                    $(selectCateg).val(tm[index].categoriaDelRiesgo);
                    $(selectCateg).trigger("change");

            
                    let selectDueñoDel = tblMatrizDetallado.find('select.dueñoDelRiesgo')[index]
                    $(selectDueñoDel).val(tm[index].dueñoDelRiesgo);
                    $(selectDueñoDel).trigger("change");

                   

                    let selectDesc = tblMatrizDetallado.find('input.chAmenzaOportunidad')[index];
                    console.log(selectDesc)
                    let selectTipo = tblMatrizDetallado.find('select.desctipoDeRespuesta')[index]
                    console.log($(selectDesc).prop('checked'))
                        $(selectTipo).fillCombo('/ControlObra/ControlObra/cboTiposDeRespuestas', {idTipo: $(selectDesc).prop('checked') == false ? 1: 2}, false, null);
                        $(selectTipo).val(tm[index].tipoDeRespuesta);
                        $(selectTipo).trigger("change");  
                    $(selectDesc).change(function () {
                        $(selectTipo).fillCombo('/ControlObra/ControlObra/cboTiposDeRespuestas', {idTipo: $(selectDesc).prop('checked') == false ? 1: 2}, false, null);
                        console.log($(selectDesc).prop('checked'))
                        $(selectTipo).val(tm[index].tipoDeRespuesta);
                        $(selectTipo).trigger("change");  
                    })

                    // $(selectDesc).change(function (e) { 
                    //     tblMatrizDetallado.find('select.desctipoDeRespuesta').fillCombo('/ControlObra/ControlObra/cboTiposDeRespuestas', {idTipo: $(selectDesc).prop('checked') == false ? 1: 2}, false, null);
                    //     tblMatrizDetallado.find('select.desctipoDeRespuesta').select2();
                    // });

              

                }
                tblMatrizDetallado.find('select.desctipoDeRespuesta').change(function(e) {
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.desctipoDeRespuesta").val();
                    let select = $(this).parent().find("input.chAmenzaOportunidad").prop('checked');
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().tipoDeRespuesta = input;                
                    dtMatrizDetallado.row(fila).data().chAmenzaOportunidad = select == false?1:2;               
                });
                tblMatrizDetallado.find('input.amenazaOportunidad').blur(function(e) {
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("input.amenazaOportunidad").val();
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().amenazaOportunidad = input;
                });
                tblMatrizDetallado.find('input.severidadInicial').change(function(e) {
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("input.severidadInicial").val();
                    if (input > 25) {
                        input = 25
                    }else if (input < 1) {
                        input = 1 
                    }
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().severidadInicial = parseInt(input);
                    dtMatrizDetallado.clear();
                    dtMatrizDetallado.rows.add(lstDetalle);
                    dtMatrizDetallado.draw();
                });
                tblMatrizDetallado.find('select.probabilidad').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.probabilidad").val();
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().probabilidad = parseInt(input);
                    let total = parseInt(dtMatrizDetallado.row(fila).data().probabilidad) * parseInt(dtMatrizDetallado.row(fila).data().impacto); 
                    
                    console.log(dtMatrizDetallado.row(fila).data())
                    console.log(parseInt(dtMatrizDetallado.row(fila).data().probabilidad))
                    console.log(parseInt(dtMatrizDetallado.row(fila).data().impacto))
                    console.log(total)

                    dtMatrizDetallado.row(fila).data().severidadActual = total;
                    dtMatrizDetallado.clear();
                    dtMatrizDetallado.rows.add(lstDetalle);
                    dtMatrizDetallado.draw();
                });
                tblMatrizDetallado.find('select.impacto').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.impacto").val();
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().impacto = parseInt(input);
                    let total = parseInt(dtMatrizDetallado.row(fila).data().probabilidad) * parseInt(dtMatrizDetallado.row(fila).data().impacto); 
                    
                    console.log(parseInt(dtMatrizDetallado.row(fila).data().probabilidad))
                    console.log(parseInt(dtMatrizDetallado.row(fila).data().impacto))
                    console.log(total)

                    dtMatrizDetallado.row(fila).data().severidadActual = total;
                    dtMatrizDetallado.clear();
                    dtMatrizDetallado.rows.add(lstDetalle);
                    dtMatrizDetallado.draw();
                });
                MergeGridCells();                
                tblMatrizDetallado.find('select.cboAbiertoCerrado').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.cboAbiertoCerrado").val();
                    console.log(input)
                    dtMatrizDetallado.row(fila).data().abiertoProcesoCerrado = parseInt(input);
                });

                tblMatrizDetallado.find('select.inpCategoriaDelRiesgo').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.inpCategoriaDelRiesgo").val();
                    dtMatrizDetallado.row(fila).data().categoriaDelRiesgo = parseInt(input);
                });
                tblMatrizDetallado.find('input.causaBasica').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("input.causaBasica").val();
                    dtMatrizDetallado.row(fila).data().causaBasica = input;
                });
                tblMatrizDetallado.find('input.areaDelProyecto').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("input.areaDelProyecto").val();
                    dtMatrizDetallado.row(fila).data().areaDelProyecto = input;
                });
                tblMatrizDetallado.find('select.costoTiempoCalidad').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.costoTiempoCalidad").val();
                    dtMatrizDetallado.row(fila).data().costoTiempoCalidad = parseInt(input);
                });
                tblMatrizDetallado.find('input.medidasATomar').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("input.medidasATomar").val();
                    dtMatrizDetallado.row(fila).data().medidasATomar = input;
                });
                tblMatrizDetallado.find('select.dueñoDelRiesgo').change(function (e) { 
                    let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                    let input = $(this).parent().find("select.dueñoDelRiesgo").val();
                    dtMatrizDetallado.row(fila).data().dueñoDelRiesgo = parseInt(input);
                });
            },
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'idMatrizDeRiesgo', title: 'idMatrizDeRiesgo', visible: false },
                { data: 'Historial', title: 'Historial', visible: false },
                { data: 'No', title: 'No', visible: false },
                { data: 'chAmenzaOportunidad', title: 'Amenaza/Oportunidad' ,render: (data, type, row, meta) => {
                     let html = ``;
                     html += '<input  id="chAmenzaOportunidad" class="form-control chAmenzaOportunidad" type="checkbox" ' + (data ? 'checked' : '') + ' data-toggle="toggle" data-on="Amenaza" data-off="Oportunidad" >';
                     return html;
                }},
                { data: 'amenazaOportunidad', title: 'Descripcion del Riesgo' ,render: (data, type, row, meta) => {
                    let html = `<input id='amenazaOportunidad' class='form-control amenazaOportunidad' value='${data}'>`;
                    return html; 
                }},
                { data: 'categoriaDelRiesgo', title: 'Categoria Del Riesgo', visible:false },
                { data: 'descategoriaDelRiesgo', title: 'Categoria Del Riesgo' ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='inpCategoriaDelRiesgo' class='form-control inpCategoriaDelRiesgo' value='${data}'></select>
                    `;
                    return html; 
                }},
                { data: 'causaBasica', title: 'Causa Basica',render: (data, type, row, meta) => {
                    let html = `<input id='causaBasica' class='form-control causaBasica' value='${data}'>`;
                    return html; 
                }},
                { data: 'areaDelProyecto', title: 'Area Del Proyecto' ,render: (data, type, row, meta) => {
                    let html = `<input id='areaDelProyecto' class='form-control areaDelProyecto' value='${data}'>`;
                    return html; 
                }},
                { data: 'costoTiempoCalidad', title: 'Costo/Tiempo/Calidad' , visible:false},
                { data: 'costoTiempoCalidad', title: 'Costo/Tiempo/Calidad'  ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='cmbImpacto' class='form-control costoTiempoCalidad'>
                        `;
                        
                        if (data == 1) {
                            html += `
                            <option value="1" selected>Costo</option>
                            <option value="2">Tiempo</option>
                            <option value="3">Calidad</option>
                            `;
                        }else if (data == 2) {
                            html += `
                            <option value="1">Costo</option>
                            <option value="2" selected>Tiempo</option>
                            <option value="3">Calidad</option>
                            `;
                        }else if (data == 3) {
                            html += `
                            <option value="1">Costo</option>
                            <option value="2">Tiempo</option>
                            <option value="3" selected>Calidad</option>
                            `;
                        }

                    html +=`
                    </select>
                    `;
                  return html; 

                }},
                { data: 'probabilidad', title: 'Probabilidad' ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='probabilidad' class='form-control probabilidad'>
                     `;
                    if (data == 1) {
                        html += `
                        <option value="1" selected>1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 2) {
                        html += `
                        <option value="1">1</option>
                        <option value="2" selected>2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 3) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3" selected>3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 4) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4" selected>4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 5) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5" selected>5</option>
                        `;
                    }

                    html +=`
                    </select>
                    `;
                    return html; 
                }},
                { data: 'impacto', title: 'Impacto' ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='impacto' class='form-control impacto'>
                    `;
                    if (data == 1) {
                        html += `
                        <option value="1" selected>1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 2) {
                        html += `
                        <option value="1">1</option>
                        <option value="2" selected>2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 3) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3" selected>3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 4) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4" selected>4</option>
                        <option value="5">5</option>
                        `;
                    }else if (data == 5) {
                        html += `
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5" selected>5</option>
                        `;
                    }

                    html +=`
                    </select>
                    `;
                    return html; 
                }},
                { data: 'severidadInicial', title: 'Sev Ini' ,render: (data,type,row,meta) => {
                    let html = '';
                    // if(data>25){
                    //     data = 25;
                    // }else if(data<1){
                    //     data = 1
                    // }
                    html = `<input id='severidadInicial' class='form-control severidadInicial' value='${data}'>`;
                    return html;
                }},
                { data: 'severidadActual', title: 'Sev Act' ,render: (data, type, row, meta) => {
                    let html = ``;
                    html = data;
                    return html; 
                }},
                { data: 'tipoDeRespuesta', title: 'tipoDeRespuesta', visible:false },
                { data: 'desctipoDeRespuesta', title: 'Tipo Respuesta' ,render: (data, type, row, meta) => {
                    let html = `
                        <select type='text' id='desctipoDeRespuesta' class='form-control desctipoDeRespuesta'>
                        </select>
                    `;
                    return html; 
                }},
                { data: 'medidasATomar', title: 'Medidas A Tomar' ,render: (data, type, row, meta) => {
                    let html = `<input id='medidasATomar' class='form-control medidasATomar' value='${data}'>`;
                    return html; 
                }},
                { data: 'dueñoDelRiesgo', title: 'Dueño Del Riesgo' ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='dueñoDelRiesgo' class='form-control dueñoDelRiesgo'>
                    </select>
                    `;
                    return html; 
                }},
                { data: 'fechaDeCompromiso', title: 'Fecha De Compromiso'  ,render: (data, type, row, meta) => {
                    let html = ``;

                    if (data != '') {
                        if (data.split('/')[1].split('(')[0] == 'Date') {
                            html =  `<input type='text' id='dtFechaCompromiso' class='form-control' value='${moment(data).format("DD/MM/YYYY")}'>`;
                        }else{
                            html =  `<input type='text' id='dtFechaCompromiso' class='form-control' value='${data}'>`;
                        }
                    }
                    return html; 

                }},
                { data: 'abiertoProcesoCerrado', title: 'Proceso' ,render: (data, type, row, meta) => {
                    let html = `
                    <select type='text' id='cboAbiertoCerrado' class='form-control cboAbiertoCerrado'>
                       `;
                        if (data == 1) {
                            html += `
                            <option value="1" selected>Abierto</option>
                            <option value="2">Proceso</option>
                            <option value="0">Cerrado</option>
                            `;
                        }else if (data == 2) {
                            html += `
                            <option value="1">Abierto</option>
                            <option value="2" selected>Proceso</option>
                            <option value="0">Cerrado</option>
                            `;
                        }else if (data == 0) {
                            html += `
                            <option value="1">Abierto</option>
                            <option value="2">Proceso</option>
                            <option value="0" selected>Cerrado</option>
                            `;
                        }

                       html += `
                    </select>
                    `;
                    return html; 
                }},
              
                
                //render: function (data, type, row) { }
            ],
            initComplete: function (settings, json) {
                tblMatrizDetallado.on('click','.eliminarMatriz', function () {
                    let rowData = dtMatrizDetallado.row($(this).closest('tr')).data();
                });
                tblMatrizDetallado.on('click','.classBtn', function () {
                    let rowData = dtMatrizDetallado.row($(this).closest('tr')).data();
                    //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                });                
            },
            rowCallback: function (row, data, index) {
                
                $('td', row).css('text-align', 'center');
                
                if (data.severidadInicial >= inpBaja.val() && data.severidadInicial <= inpBajaFin.val() ) {
                    $('td', row).eq(8).css('background-color', '#00b050');
                    $('td', row).eq(8).css('color', 'white');
                    $('td', row).eq(8).find('input').css('background-color', '#00b050');
                    $('td', row).eq(8).find('input').css('color', 'white');
                }else if (data.severidadInicial >= inpMedia.val() && data.severidadInicial <= inpMediaFin.val() ) {
                    $('td', row).eq(8).css('background-color', '#ffc000');
                    $('td', row).eq(8).css('color', 'white');
                    $('td', row).eq(8).find('input').css('background-color', '#ffc000');
                    $('td', row).eq(8).find('input').css('color', 'white');
                }else if (data.severidadInicial >= inpAlta.val() && data.severidadInicial <= inpAltaFin.val() ) {
                    $('td', row).eq(8).css('background-color', '#ed2937');
                    $('td', row).eq(8).css('color', 'white');
                    $('td', row).eq(8).find('input').css('background-color', '#ed2937');
                    $('td', row).eq(8).find('input').css('color', 'white');
                }
                if (data.severidadActual >= inpBaja.val() && data.severidadActual <= inpBajaFin.val() ) {
                    $('td', row).eq(9).css('background-color', '#00b050');
                    $('td', row).eq(9).css('color', 'white');
                 
                }else if (data.severidadActual >= inpMedia.val() && data.severidadActual <= inpMediaFin.val() ) {
                    $('td', row).eq(9).css('background-color', '#ffc000');
                    $('td', row).eq(9).css('color', 'white');
              
                }else if (data.severidadActual >= inpAlta.val() && data.severidadActual <= inpAltaFin.val() ) {
                    $('td', row).eq(9).css('background-color', '#ed2937');
                    $('td', row).eq(9).css('color', 'white');
                    
                }
                
                //         $('td', row).eq(0).css('background-color', '#5cb85c');
                //         $('td', row).eq(0).css('color', 'white');
                //         break;
                //     case 1:
                //         $('td', row).eq(0).css('background-color', '#204d74');
                //         $('td', row).eq(0).css('color', 'white');
                //         break;
                //     case 3:
                //         $('td', row).eq(0).css('background-color', '#ff1919');
                //         $('td', row).eq(0).css('color', 'white');
                //         break;
                // }
            },
           
            columnDefs: [
                { className: 'dt-center','targets': '_all'},
                { "width": "20%", "targets": 0 } 
            ],
        });
        dtMatrizDetallado.columns.adjust().draw();
    }
    function GuardarEditarMatriz() {
        let parametros = getParameters();
        axios.post('GuardarEditarMatriz', {parametros:parametros,editar:btnSave.attr('data-editar')})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    obtenerMatrizDeRiesgo();
                    Alert2Exito('Guardado con exito');
                    mdlMatrizDeRiesgo.modal('hide');
                }
            });
    }
    function AddRows(tbl, lst) {
        dttblMatrizDeRiesgo = tbl.DataTable();
        dttblMatrizDeRiesgo.clear().draw();
        dttblMatrizDeRiesgo.rows.add(lst).draw(false);
    }
    function AgregarAlDetalle() {
        if (lstDetalle == null) {
            lstDetalle = [];
        }
        let item = {
            id : 0,
            idMatrizDeRiesgo : 0,
            Historial : 0,
            No : inpNoProyecto.val() == null ? '':inpNoProyecto.val(),
            amenazaOportunidad : inpAmenazaOportunidad.val() == null ? '':inpAmenazaOportunidad.val(),
            categoriaDelRiesgo : inpCategoriaDelRiesgo.val() == null ? '':inpCategoriaDelRiesgo.val(),
            descategoriaDelRiesgo : inpCategoriaDelRiesgo.val() == null ? '':inpCategoriaDelRiesgo.find('option:selected').text(),
            
            causaBasica : inpCausaBasica.val() == null ? '':inpCausaBasica.val(),
            areaDelProyecto : inpAreaDelProyecto.val() == null ? '':inpAreaDelProyecto.val(),
            costoTiempoCalidad : cmbImpacto.val() == null ? '':cmbImpacto.val(),
            probabilidad : inpProbabilidad.val() == null ? '':inpProbabilidad.val(),
            impacto : inpImpacto.val() == null ? '':inpImpacto.val(),
            severidadInicial : inpSeveridadInicial.val() == null ? '':inpSeveridadInicial.val(),
            severidadActual : inpSeveridadActual.val() == null ? '':inpSeveridadActual.val(),
            tipoDeRespuesta : cboTipoRespuesta.val() == null ? '':cboTipoRespuesta.val(),
            desctipoDeRespuesta : cboTipoRespuesta.val() == null ? '':cboTipoRespuesta.find('option:selected').text(),
            
            medidasATomar : txtMedidasATomar.val()==null?'':txtMedidasATomar.val(),
            dueñoDelRiesgo : cboDueñoDelRiesgo.val() == null ? '':cboDueñoDelRiesgo.val(),
            fechaDeCompromiso : dtFechaCompromiso.val() == null ? '':dtFechaCompromiso.val(),
            abiertoProcesoCerrado : cboAbiertoCerrado.val() == null ? '':cboAbiertoCerrado.val(),
        };
        lstDetalle.push(item);
        if (lstDetalle != null ) {
            dtMatrizDetallado.clear();
            dtMatrizDetallado.rows.add(lstDetalle);
            dtMatrizDetallado.draw();
        } 
        mdlAgregarDetalle.modal('hide');
        
    }

    function llenarTablaD() {
        axios.post('lstMrTiposDeRespuestas').then(response => {
            let { success, items, message } = response.data;
            if (success) {

                console.log(items)
                let Amenaza = $.grep(items, function(n,index){ return n.tipoRespuesta == 'Amenaza' ; });
                let Oportunidad = $.grep(items, function(n,index){ return n.tipoRespuesta == 'Oportunidad' ; });

                console.log(Amenaza)
                console.log(Oportunidad)
                let numero = numeroMayo(Amenaza.length,Oportunidad.length);
                let html = ``;
                for (let i = 0; i < numero; i++) {
                    html += `<tr>`
                    html += `<td>${Amenaza[i]==undefined?'':Amenaza[i].descripcion}</td>
                             <td>${Amenaza[i]==undefined?'':Amenaza[i].respuestaDesc}</td>
                             <td>${Oportunidad[i]==undefined?'':Oportunidad[i].descripcion}</td>
                             <td>${Oportunidad[i]==undefined?'':Oportunidad[i].respuestaDesc}</td>`
                    html += `</tr>`
                }
                conteniendoDeTabla.find('tr').remove();
                conteniendoDeTabla.append(html);
            }
        }).catch(error => Alert2Error(error.message));
    }

    function numeroMayo(a,b) {
        if (a > b) {
            return a;
        }else{
            return b;
        }
    }

    function MergeGridCells() {
        var dimension_cells = new Array();
        var dimension_col = 2;
        var first_instance = null;
        var rowspan = 1;
        $("#tblMatrizDetallado").find('tr').each(function () {
            var dimension_td = $(this).find('td:nth-child(' + dimension_col + ')');
            if (first_instance == null) {
                first_instance = dimension_td;
            } else if (dimension_td.html() == first_instance.html()) {
                dimension_td.remove();
                ++rowspan;
                first_instance.attr('rowspan', rowspan);
            } else {
                first_instance = dimension_td;
                rowspan = 1;
            }
        });        
    }

    $(document).ready(() => {
        MatrizDeRiesgo.ControlObra = new ControlObra();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();