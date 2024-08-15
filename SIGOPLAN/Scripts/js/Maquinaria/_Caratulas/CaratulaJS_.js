(() => {
    $.namespace("Maquinaria.Caratula.CapturaCaratula");

    Caratula = function () {
        const cboObra = $("#cboObra");
        const cboModelo2 = $('#cboModelo2');
        const cboGrupo2 = $('#cboGrupo2');
        const DireccionTecnica = $('#DireccionTecnica');
        const SubdireccionMaquinaria = $('#SubdireccionMaquinaria');
        const tblCaratula = $('#tblCaratula');
        const btnNuevo2 = $('#btnNuevo2');       
        const btnMostrar = $('#btnMostrar');
        const mdlNuevoModelo = $('#mdlNuevoModelo');
        const txtDepreciacionDLLS = $('#txtDepreciacionDLLS');
        const txtInversionDLLS = $('#txtInversionDLLS');
        const txtSeguroDLLS = $('#txtSeguroDLLS');
        const txtFiltroDLLS = $('#txtFiltroDLLS');
        const txtMantenimientoDLLS = $('#txtMantenimientoDLLS');
        const txtManoObraMXN = $('#txtManoObraMXN');
        const txtAuxiliarDLLS = $('#txtAuxiliarDLLS');
        const txtIndirectosDLLS = $('#txtIndirectosDLLS');
        const txtDepreciacionOHDLLS = $('#txtDepreciacionOHDLLS');
        const txtAceiteDLLS = $('#txtAceiteDLLS');
        const txtCarilleriaDLLS = $('#txtCarilleriaDLLS');
        const txtAnsulDLLS = $('#txtAnsulDLLS');
        const txtUtilidadDLLS = $('#txtUtilidadDLLS');    
        const txtTotalDLLS = $('#txtTotalDLLS');
        const txtDepreciacionMXN = $('#txtDepreciacionMXN');
        const txtInversionMXN = $('#txtInversionMXN');
        const txtSeguroMXN = $('#txtSeguroMXN');
        const txtFiltroMXN = $('#txtFiltroMXN');
        const txtMantenimientoMXN = $('#txtMantenimientoMXN');
        const txtManoObraDLLS = $('#txtManoObraDLLS');
        const txtAuxiliarMXN = $('#txtAuxiliarMXN');
        const txtIndirectosMXN = $('#txtIndirectosMXN');
        const txtDepreciacionOHMXN = $('#txtDepreciacionOHMXN');
        const txtAceiteMXN = $('#txtAceiteMXN');
        const txtCarilleriaMXN = $('#txtCarilleriaMXN');
        const txtAnsulMXN = $('#txtAnsulMXN');
        const txtUtilidadMXN = $('#txtUtilidadMXN');
        const txtTotalMXN = $('#txtTotalMXN');
        const tipoDeCambio = $('#tipoDeCambio');
        const tipoDeCambioModal = $("#tipoDeCambioModal");

        const btnSacarSuma = $('#btnSacarSuma');
        const btnGuardarNuevoModelo = $('#btnGuardarNuevoModelo');
        const TipoCambio = $('#TipoCambio');
        const btnGuardarCaratula = $('#btnGuardarCaratula');        
        ireporteTiemposCRC = $("#reporteCaratula > #reportViewerModal > #report");
        reporteCaratula = $('#reporteCaratula');
        const btnReporte = $('#btnReporte');
        const btnSumar = $('#btnSumar');
        const cboHoraDia = $('#cboHoraDia');

        let DatosCaratula = [];
        const cboAgrupaciones = $('#cboAgrupaciones');
        const cboCostoPor = $('#cboCostoPor');
        const btnBuscar = $('#btnBuscar');
        const contenidoNuevo = $('#contenidoNuevo');


        let conceptosMoneda = [];

        (function init() {          
            fncFillCombos();
            fncFillcomboGrupo();
            Autorizantes();
            initCapturaCaratula(); 
            hablitar();
            eventchanges();
            txtTotalMXN.attr("disabled", true);
            txtTotalDLLS.attr("disabled", true);
            txtManoObraMXN.attr("disabled", true);
            txtManoObraDLLS.attr("disabled", true);
            txtDepreciacionMXN.attr("disabled", true);
            txtInversionMXN.attr("disabled", true);
            txtSeguroMXN.attr("disabled", true);
            txtFiltroMXN.attr("disabled", true);
            txtMantenimientoMXN.attr("disabled", true);
            txtAuxiliarMXN.attr("disabled", true);
            txtIndirectosMXN.attr("disabled", true);
            txtDepreciacionOHMXN.attr("disabled", true);
            txtAceiteMXN.attr("disabled", true);
            txtCarilleriaMXN.attr("disabled", true);
            txtAnsulMXN.attr("disabled", true);
            txtUtilidadMXN.attr("disabled", true); 

            convertToMultiselectSelectAll(cboCostoPor);
            cboCostoPor.multiselect('enable');

            CargarCaratulaActiva();

            $('input.nuevoModelo').focus(function(e) {
                $(this).val(Number($(this).val().replace(/[^0-9.-]+/g,"")));
            });
            $('input.nuevoModelo').blur(function(e) {
                if($(this).val() == '') $(this).val("0.00");
                else {
                    let auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                    $(this).val('$' + auxValor.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
            });
            TipoCambio.change(function (e) {
                let inputActualizar = $('input.cargaMonto:disabled');
                inputActualizar.each(function (e) {
                    if($(this).hasClass('dolares'))
                    {
                        let pesos = Number($($(this).parent().children('.pesos')[0]).val().replace(/[^0-9.-]+/g,""));
                        let dolares = ConvertirADLLS(pesos);
                        $(this).val('$' + dolares.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }
                    else
                    {
                        let dolares = Number($($(this).parent().children('.dolares')[0]).val().replace(/[^0-9.-]+/g,""));
                        let pesos = ConvertirAMXN(dolares);
                        $(this).val('$' + pesos.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }
                });
            });
            limpiarInputs();
            
        })();

        function eventchanges() {

            cboGrupo2.change(function () {
                if (cboGrupo2.val()!='') {
                    fncFillcomboModelo();
                }
            });
            cboAgrupaciones.change(function () {
                if (cboAgrupaciones.find('option:selected').text() == '--Seleccione--') {
                    contenidoNuevo.css('display','block');
                    cboGrupo2.attr('disabled',false);
                } else{
                    contenidoNuevo.css('display','none');
                    cboGrupo2.attr('disabled',true);
                    cboGrupo2.val(cboAgrupaciones.find('option:selected').val());
                    cboGrupo2.trigger("change");
                    fncFillcomboModelo();    

                }
            });
        }
        function hablitar(){
            // let total = txtTotalDLLS.val();
            // if (total != "") {
            //      btnGuardarNuevoModelo.attr("disabled", false);                
            // }
            // else{
            //     btnGuardarNuevoModelo.attr("disabled", true);                
            // }
        }
        $("#archivoxls").change(function() { 
            subiendoArchivo(); 
        });

        btnNuevo2.click(function(){
            var auxTipoCambio = +TipoCambio.val();
            // console.log(auxTipoCambio);
            limpiarInputs();
            mdlNuevoModelo.modal("show");
       });       

        btnMostrar.click(function(e){
            $("#archivoxls").click();
        });

        btnBuscar.on("click", function (e) {
            CargarCaratulaActiva();
        });

        function fncFillCombos() {
            cboAgrupaciones.fillCombo('ObtenerAgrupaciones',{},false);
            cboAgrupaciones.select2({
                width: "resolve"
            });
            cboObra.fillCombo("FillAreasCuentas", {}, false);
            cboObra.select2({
                width: "resolve"
            });
         }

         function fncFillcomboModelo(){
            cboModelo2.fillCombo("obtenerModelos?idGrupo="+cboGrupo2.val(), {}, false);
            // cboModelo2.fillCombo("FillCboModelo", {},false);
            cboModelo2.select2({
                width: "resolve"
            });
         }

         function fncFillcomboGrupo(){
             cboGrupo2.fillCombo("FillcboGrupo", {},false);
             cboGrupo2.select2({
                width: "resolve"
            });
         }

         function fnSelRevisa(event, ui) {
            $(this).data("id", ui.item.id);
            $(this).data("nombre", ui.item.value);
        }
        
        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val() != '') {
                $(this).val("");
                $(this).data("id", "");
                $(this).data("nombre", "");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }

         function Autorizantes(){
            //DireccionTecnica.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
             //SubdireccionMaquinaria.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
             DireccionTecnica.val('JUAN PABLO CECCO VAZQUEZDEMERCADO');
             SubdireccionMaquinaria.val('OSCAR MANUEL ROMAN RUIZ');
         }

        $("#depreciacionMXN").change(function(){
            // var dolar = TipoCambio.val();
            // var total = 0;
            // total = $("#depreciacionMXN").val() / parseFloat(dolar);
            // var con = total.toFixed(2);
            // $("#depreciacionId").val(con);
        });

       
          
        function ConvertirAMXN(dlls){           
            var dolar = +TipoCambio.val();
            var total = 0;
            total = parseFloat(dlls) * parseFloat(dolar);
            var con = total.toFixed(2);
            return con;
        }

        function ConvertirADLLS(mxn){           
            var pesos = +TipoCambio.val();
            var total = 0;
            total = parseFloat(mxn) / parseFloat(pesos);
            var con = total.toFixed(2);
            return con;
        }

       
           
        function initCapturaCaratula() {
            dtCaratula = tblCaratula.DataTable({
                language: dtDicEsp,
                destroy: false,
                scrollX: false,                
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data:"id",title:"id", visible:false },
                    {
                        data:"lstCatGrupo",
                        title:"Grupo",
                        render: function (data, type, full, meta)
                        {
                            return data.descripcion;
                        }
                    },    
                    {
                        data:"lstCatModelo",
                        title:"Modelo",
                        render: function (data, type, full, meta)
                        {
                            if (full.Agrupacion == '-') {
                                return data.descripcion;
                            }else{
                                return '-'
                            }
                        }
                    },     
                    {
                        data:"Agrupacion",
                        title:"Agrupacion",
                        render: function (data, type, full, meta)
                        {
                            return `${data}`;
                        }
                    },   
                    {title:"Moneda", render: function (data, type, full, meta){
                            let moneda = '';
                            moneda+='<h6>USD</h6>';
                            moneda+='<h6>MXN</h6>';
                            return moneda;
                        }
                    },       
                    { data:"tipoHoraDia", title:"Tipo Costo", render: function (data, type, full, meta){
                            let moneda = '';
                            if (data == 1) {
                                moneda+='<h6>HORA</h6>';
                            }else{
                                moneda+='<h6>DIA</h6>';
                            }

                        //     <select  id="cboHora" class="form-control" style="
                        //     width: 100px;
                        // ">
                        //         `;
                        //         if (data == 1) {
                        //             moneda += `<option value='1' selected>HORA</option>
                        //              <option value='2'>DIA</option>`;

                        //         }else{
                        //             moneda += `<option value='1'>HORA</option>
                        //             <option value='2' selected>DIA</option>`;
                        //         }
                        //          moneda += `</select>`;


                            return moneda;
                        }
                    },                                                          
                    {
                        data:"depreciacionDLLS", 
                        title:"Depreciación", 
                        render: function (data, type, full, meta)
                        {
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.depreciacionMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto depreciacionDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto depreciacionMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }
                    },
                    {data:"inversionDLLS",title:"Inversión", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.inversionMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto inversionDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto inversionMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }
                     },
                    {data:"seguroDLLS",title:"Seguro", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.seguroMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto seguroDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto seguroMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }
                     },
                    {data:"filtroDLLS",title:"Filtros", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.filtroMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto filtrosDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto filtrosMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }
                    },
                    {data:"mantenimientoDLLS",title:"Mant. Correctivo", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.mantenimientoMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto mantenimientoDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto mantenimientoMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }
                     },
                    {data:"manoObraDLLS",title:"Mano de<br>obra", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.manoObraMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto manoObraDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto manoObraMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"auxiliarDLLS",title:"Eq. Auxiliar<br>y otros", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.auxiliarMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto auxiliarDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto auxiliarMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"indirectosDLLS",title:"Indirectos<br>matriz", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.indirectosMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto indirectosDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto indirectosMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"depreciacionOHDLLS",title:"Depreciacion<br>OverHaul", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.depreciacionOHMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto depreciacionOHDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto depreciacionOHMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"aceiteDLLS",title:"Aceite", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.aceiteMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto aceiteDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto aceiteMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"carilleriaDLLS",title:"Carilleria o<br>pieza especial", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.carilleriaMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto carilleriaDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto carilleriaMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"ansulDLLS",title:"ANSUL", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.ansulMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto ansulDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto ansulMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"utilidadDLLS",title:"Utilidad", render: function (data, type, full, meta){
                            let desabilitarPesos = conceptosMoneda[meta.col - 6].tipoMoneda == 'dlls';

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.utilidadMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto utilidadDLLS dolares" value=' + valorDLLS + ' style="width: 100px;" ' + (!desabilitarPesos ? 'disabled' : '') + ' /><br>'; 
                            input += '<input class="cargaMonto utilidadMXN pesos" value=' + valorMXN + ' style="width: 100px;" ' + (desabilitarPesos ? 'disabled' : '') + ' /><br>';  
                            return input;
                        }  
                    },
                    {data:"costoDLLS",title:"Costo Renta", render: function (data, type, full, meta){
                            desabilitarPesos = true;

                            let valorDLLS = '$' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            let valorMXN = '$' + parseFloat(full.costoMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                            input = '<input class="cargaMonto totalDLLS" value=' + valorDLLS + ' style="width: 100px;" disabled /><br>'; 
                            input += '<input class="cargaMonto totalMXN" value=' + valorMXN + ' style="width: 100px;" disabled /><br>';  
                            return input;
                        }  
                    },

                ],
                initComplete: function (settings, json) {
                    tblCaratula.on('click','.classBtn', function () {
                        let rowData = dtCaratula.row($(this).closest('tr')).data();                       
                    });
                    tblCaratula.on('click','.classBtn', function () {
                        let rowData = dtnombre.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                drawCallback: function( settings ) { 
                    tblCaratula.find('input.cargaMonto').focus(function(e) {
                        $(this).val(Number($(this).val().replace(/[^0-9.-]+/g,"")));
                    });
                    tblCaratula.find('input.cargaMonto').blur(function(e) {
                        if($(this).val() == '') $(this).val("0.00");
                        else {
                            let auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                            $(this).val('$' + auxValor.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        }

                        if($(this).val() != "0.00") $(this).css("background-color", "#B4C6E7");
                        else $(this).css("background-color", "-internal-light-dark(rgb(255, 255, 255), rgb(59, 59, 59));");
                    });
                    tblCaratula.find('input.dolares').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        var dolares = Number($(this).val().replace(/[^0-9.-]+/g,""));
                        var pesos = ConvertirAMXN(dolares);
                        $(this).parent().find("input.pesos").val('$' + parseFloat(pesos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        var totalDLLS = 0;
                        $(this).parent().parent().children().find('input.dolares').each(function(x, data){ totalDLLS += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                        $(this).parent().parent().children().find("input.totalDLLS").val('$' + parseFloat(totalDLLS).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        var totalMXN = 0;
                        $(this).parent().parent().children().find('input.pesos').each(function(x, data){ totalMXN += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                        $(this).parent().parent().children().find("input.totalMXN").val('$' + parseFloat(totalMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        dtCaratula.row(fila).data().costoDLLS = totalDLLS;
                        dtCaratula.row(fila).data().costoMXN = totalMXN;
                    });
                    tblCaratula.find('input.pesos').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        var pesos = Number($(this).val().replace(/[^0-9.-]+/g,""));
                        var dolares = ConvertirADLLS(pesos);
                        $(this).parent().find("input.dolares").val('$' + parseFloat(dolares).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        var totalDLLS = 0;
                        $(this).parent().parent().children().find('input.dolares').each(function(x, data){ totalDLLS += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                        $(this).parent().parent().children().find("input.totalDLLS").val('$' + parseFloat(totalDLLS).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        var totalMXN = 0;
                        $(this).parent().parent().children().find('input.pesos').each(function(x, data){ totalMXN += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                        $(this).parent().parent().children().find("input.totalMXN").val('$' + parseFloat(totalMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        dtCaratula.row(fila).data().costoDLLS = totalDLLS;
                        dtCaratula.row(fila).data().costoMXN = totalMXN;
                    });
                    tblCaratula.find('input.depreciacionDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().depreciacionDLLS = auxValor;
                        dtCaratula.row(fila).data().depreciacionMXN = auxValor2;
                    });
                    tblCaratula.find('input.inversionDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().inversionDLLS = auxValor;
                        dtCaratula.row(fila).data().inversionMXN = auxValor2;
                    });
                    tblCaratula.find('input.seguroDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().seguroDLLS = auxValor;
                        dtCaratula.row(fila).data().seguroMXN = auxValor2;
                    });
                    tblCaratula.find('input.filtroDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().filtroDLLS = auxValor;
                        dtCaratula.row(fila).data().filtroMXN = auxValor2;
                    });
                    tblCaratula.find('input.mantenimientoDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().mantenimientoDLLS = auxValor;
                        dtCaratula.row(fila).data().mantenimientoMXN = auxValor2;
                    });
                    tblCaratula.find('input.manoObraDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().manoObraDLLS = auxValor;
                        dtCaratula.row(fila).data().manoObraMXN = auxValor2;
                    });
                    tblCaratula.find('input.auxiliarDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().auxiliarDLLS = auxValor;
                        dtCaratula.row(fila).data().auxiliarMXN = auxValor2;
                    });
                    tblCaratula.find('input.indirectosDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().indirectosDLLS = auxValor;
                        dtCaratula.row(fila).data().indirectosMXN = auxValor2;
                    });
                    tblCaratula.find('input.depreciacionOHDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().depreciacionOHDLLS = auxValor;
                        dtCaratula.row(fila).data().depreciacionOHMXN = auxValor2;
                    });
                    tblCaratula.find('input.aceiteDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().aceiteDLLS = auxValor;
                        dtCaratula.row(fila).data().aceiteMXN = auxValor2;
                    });
                    tblCaratula.find('input.carilleriaDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().carilleriaDLLS = auxValor;
                        dtCaratula.row(fila).data().carilleriaMXN = auxValor2;
                    });
                    tblCaratula.find('input.ansulDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().ansulDLLS = auxValor;
                        dtCaratula.row(fila).data().ansulMXN = auxValor2;
                    });
                    tblCaratula.find('input.utilidadDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().utilidadDLLS = auxValor;
                        dtCaratula.row(fila).data().utilidadMXN = auxValor2;
                    });
                    tblCaratula.find('input.costoDLLS').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.pesos");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().costoDLLS = auxValor;
                        dtCaratula.row(fila).data().costoMXN = auxValor2;
                    });

                    tblCaratula.find('input.depreciacionMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().depreciacionMXN = auxValor;
                        dtCaratula.row(fila).data().depreciacionDLLS = auxValor2;
                    });
                    tblCaratula.find('input.inversionMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().inversionMXN = auxValor;
                        dtCaratula.row(fila).data().inversionDLLS = auxValor2;
                    });
                    tblCaratula.find('input.seguroMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().seguroMXN = auxValor;
                        dtCaratula.row(fila).data().seguroDLLS = auxValor2;
                    });
                    tblCaratula.find('input.filtroMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().filtroMXN = auxValor;
                        dtCaratula.row(fila).data().filtroDLLS = auxValor2;
                    });
                    tblCaratula.find('input.mantenimientoMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().mantenimientoMXN = auxValor;
                        dtCaratula.row(fila).data().mantenimientoDLLS = auxValor2;
                    });
                    tblCaratula.find('input.manoObraMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().manoObraMXN = auxValor;
                        dtCaratula.row(fila).data().manoObraDLLS = auxValor2;
                    });
                    tblCaratula.find('input.auxiliarMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().auxiliarMXN = auxValor;
                        dtCaratula.row(fila).data().auxiliarDLLS = auxValor2;
                    });
                    tblCaratula.find('input.indirectosMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().indirectosMXN = auxValor;
                        dtCaratula.row(fila).data().indirectosDLLS = auxValor2;
                    });
                    tblCaratula.find('input.depreciacionOHMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().depreciacionOHMXN = auxValor;
                        dtCaratula.row(fila).data().depreciacionOHDLLS = auxValor2;
                    });
                    tblCaratula.find('input.aceiteMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().aceiteMXN = auxValor;
                        dtCaratula.row(fila).data().aceiteDLLS = auxValor2;
                    });
                    tblCaratula.find('input.carilleriaMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().carilleriaMXN = auxValor;
                        dtCaratula.row(fila).data().carilleriaDLLS = auxValor2;
                    });
                    tblCaratula.find('input.ansulMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().ansulMXN = auxValor;
                        dtCaratula.row(fila).data().ansulDLLS = auxValor2;
                    });
                    tblCaratula.find('input.utilidadMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().utilidadMXN = auxValor;
                        dtCaratula.row(fila).data().utilidadDLLS = auxValor2;
                    });
                    tblCaratula.find('input.costoMXN').blur(function(e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let inputConversion = $(this).parent().find("input.dolares");
                        let auxValor = 0, auxValor2 = 0;
                        if($(this).val() != '') auxValor = parseFloat($(this).val().replace(/[^0-9.-]+/g,"")) || 0;
                        if(inputConversion.val() != '') auxValor2 = parseFloat(inputConversion.val().replace(/[^0-9.-]+/g,"")) || 0;
                        dtCaratula.row(fila).data().costoMXN = auxValor;
                        dtCaratula.row(fila).data().costoDLLS = auxValor2;
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'},
                    { width: '105px', targets: [4,5,6,7,8,9,10,11,12,13,14,15,16,17] }, 
                ],
            });
        }

        function CargarArchivo() {
            let data = new FormData();           
            $.each(document.getElementById("archivoxls").files, function (i, file) {
                data.append("archivo", file);
                data.append("tipoCambio",TipoCambio.val());
            });
            return data;
        }

        const subiendoArchivo = function () {
            var data = CargarArchivo();   
            // console.log(data)
            var tipoCambio = TipoCambio.val();   
            // console.log(tipoCambio)
            if (tipoCambio == '') {
                Alert2Warning('no tiene asignado un tipo de cambio');
                return;
            }      
            axios.post('/Caratulas/MostrarArchivo', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        dtCaratula.clear().draw();
                        // console.log(response.data.lstReturnExcel)
                        conceptosMoneda = response.data.conceptosMoneda;
                        dtCaratula.rows.add(response.data.lstReturnExcel).draw();

                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        btnGuardarNuevoModelo.click(function() {
            fncGuadarModelo();
            // limpiarInputs(); 
        });

        // guardar modelos
        function fncGuadarModelo() {
            let existe = ExisteAgrupacion();

            if (existe == 2) {
                // console.log(existe)
            }else if(existe == 1){
                // console.log(existe)
            }else{
                let parametros = objFiltros();
                if (fncCamposVacios()) {
                    Alert2Warning("Favor de llenar todos los controles");
                } else {
                    axios.post('/Caratulas/GuardarModelo', parametros).then(response => {
                        let { success, items, message } = response.data;                    
                        if (success) {                                   
                            fncGetCaratula();
                            // console.log(DatosCaratula)
                            Alert2Exito("se registro con exito");
                            mdlNuevoModelo.modal("hide");
                            limpiarInputs();    
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }
   
            
      
            
        }
        function ExisteAgrupacion() {
            let SiExiste = 0;            
            for (let i = 0; i < DatosCaratula.length; i++) {
               let existe = DatosCaratula[i].Agrupacion.indexOf(cboAgrupaciones.find('option:selected').text());
                if (existe == '0') {
                    // console.log(DatosCaratula[i].idModelo.indexOf(parseInt(cboModelo2.find('option:selected').val())))
                        if (DatosCaratula[i].idModelo.indexOf(parseInt(cboModelo2.find('option:selected').val())) == '2') {
                            Alert2Warning('El modelo '+cboModelo2.find('option:selected').text()+' ya se encuentra seleccionado');
                            SiExiste = 1;
                            break;
                        }else{
                            DatosCaratula[i].idModelo.push(parseInt(cboModelo2.find('option:selected').val()))
                            SiExiste = 2;
                            // console.log(DatosCaratula)
                            Alert2Exito("se registro con exito");
                            mdlNuevoModelo.modal("hide");
                            limpiarInputs();    
                            break;
                        }
                        
                }
            }
            // console.log(SiExiste)
            return SiExiste;
        }


            
        function fncCamposVacios() {
            let vacio = false;
            cboGrupo2.val() == "--Seleccione--" ? vacio = true : vacio = false;
            cboModelo2.val() == "--Seleccione--" ? vacio = true : vacio = false;
            txtDepreciacionDLLS.val() == "" ? vacio = true : vacio = false;
            txtInversionDLLS.val() == "" ? vacio = true : vacio = false;     
            txtSeguroDLLS.val() == "" ? vacio = true : vacio = false;     
            txtFiltroDLLS.val() == "" ? vacio = true : vacio = false;     
            txtMantenimientoDLLS.val() == "" ? vacio = true : vacio = false;     
            txtAuxiliarDLLS.val() == "" ? vacio = true : vacio = false;     
            txtIndirectosDLLS.val() == "" ? vacio = true : vacio = false;     
            txtDepreciacionOHDLLS.val() == "" ? vacio = true : vacio = false;     
            txtAceiteDLLS.val() == "" ? vacio = true : vacio = false;     
            txtUtilidadDLLS.val() == "" ? vacio = true : vacio = false;    
            txtCarilleriaDLLS.val() == "" ? vacio = true : vacio = false;    
            txtAnsulDLLS.val() == "" ? vacio = true : vacio = false;        
            tipoDeCambioModal.val(TipoCambio.val());
            return vacio;
        }  

        function objFiltros(){
            let obj = new Object();
            obj={                
                idGrupo : cboGrupo2.val(),
                idModelo : cboModelo2.val(),
                Agrupacion : '-',
                depreciacion : Number(txtDepreciacionDLLS.val().replace(/[^0-9.-]+/g,"")),
                inversion : Number(txtInversionDLLS.val().replace(/[^0-9.-]+/g,"")),
                seguro : Number(txtSeguroDLLS.val().replace(/[^0-9.-]+/g,"")),
                filtros : Number(txtFiltroDLLS.val().replace(/[^0-9.-]+/g,"")),
                mantenimientoCo : Number(txtMantenimientoDLLS.val().replace(/[^0-9.-]+/g,"")),
                manoObra : Number(txtManoObraMXN.val().replace(/[^0-9.-]+/g,"")),
                auxiliar : Number(txtAuxiliarDLLS.val().replace(/[^0-9.-]+/g,"")),
                indirectosMatriz : Number(txtIndirectosDLLS.val().replace(/[^0-9.-]+/g,"")),
                depreciacionOH : Number(txtDepreciacionOHDLLS.val().replace(/[^0-9.-]+/g,"")),
                aceite : Number(txtAceiteDLLS.val().replace(/[^0-9.-]+/g,"")),
                carilleria : Number(txtCarilleriaDLLS.val().replace(/[^0-9.-]+/g,"")),
                ansul : Number(txtAnsulDLLS.val().replace(/[^0-9.-]+/g,"")),
                utilidad : Number(txtUtilidadDLLS.val().replace(/[^0-9.-]+/g,"")),
                costoTotal : Number(txtTotalDLLS.val().replace(/[^0-9.-]+/g,"")),
                tipoHoraDia : cboHoraDia.val() == '--SELECIONE--' ? 1: cboHoraDia.val(),
            }
            return obj;
        }

        txtDepreciacionDLLS.change(function(){
            var dlls = Number(txtDepreciacionDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtDepreciacionMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));            
            RecalcularTotalModal();   
        });

        txtInversionDLLS.change(function(){
            var dlls = Number(txtInversionDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtInversionMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));        
            RecalcularTotalModal();   
        });
       
        txtSeguroDLLS.change(function(){
            var dlls = Number(txtSeguroDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtSeguroMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));        
            RecalcularTotalModal();   
        });

        txtFiltroDLLS.change(function(){
            var dlls = Number(txtFiltroDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtFiltroMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));       
            RecalcularTotalModal();   
        });

        txtMantenimientoDLLS.change(function(){
            var dlls = Number(txtMantenimientoDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtMantenimientoMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));          
            RecalcularTotalModal();   
        });

        txtAuxiliarDLLS.change(function(){
            var dlls = Number(txtAuxiliarDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtAuxiliarMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));    
            RecalcularTotalModal();   
        });

        txtIndirectosDLLS.change(function(){
            var dlls = Number(txtIndirectosDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtIndirectosMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));     
            RecalcularTotalModal();   
        });

        txtDepreciacionOHDLLS.change(function(){
            var dlls = Number(txtDepreciacionOHDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtDepreciacionOHMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));       
            RecalcularTotalModal();   
        });

        txtAceiteDLLS.change(function(){
            var dlls = Number(txtAceiteDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtAceiteMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));      
            RecalcularTotalModal();   
        });

        txtCarilleriaDLLS.change(function(){
            var dlls = Number(txtCarilleriaDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtCarilleriaMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));      
            RecalcularTotalModal();   
        });

        txtAnsulDLLS.change(function(){
            var dlls = Number(txtAnsulDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            txtAnsulMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));     
            RecalcularTotalModal();   
        });

        txtUtilidadDLLS.change(function(){
            var dlls = Number(txtUtilidadDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();     
            
            total = parseFloat(dlls) * parseFloat(tipo);
            xtUtilidadMXN.val('$' + total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));  
            RecalcularTotalModal();             
        });     

        function RecalcularTotalModal()
        {
            let totalCosto = 0;

            var depreciacion = Number(txtDepreciacionDLLS.val().replace(/[^0-9.-]+/g,""));
            var inversion = Number(txtInversionDLLS.val().replace(/[^0-9.-]+/g,""));
            var seguro = Number(txtSeguroDLLS.val().replace(/[^0-9.-]+/g,""));
            var filtro = Number(txtFiltroDLLS.val().replace(/[^0-9.-]+/g,""));
            var mantenimientoCo = Number(txtMantenimientoDLLS.val().replace(/[^0-9.-]+/g,""));
            var manoObra = Number(txtManoObraMXN.val().replace(/[^0-9.-]+/g,""));
            var auxiliar = Number(txtAuxiliarDLLS.val().replace(/[^0-9.-]+/g,""));
            var indirectos = Number(txtIndirectosDLLS.val().replace(/[^0-9.-]+/g,""));
            var depreciacionOH = Number(txtDepreciacionOHDLLS.val().replace(/[^0-9.-]+/g,""));
            var aceite = Number(txtAceiteDLLS.val().replace(/[^0-9.-]+/g,""));
            var ansul = Number(txtAnsulDLLS.val().replace(/[^0-9.-]+/g,""));
            var carilleria = Number(txtCarilleriaDLLS.val().replace(/[^0-9.-]+/g,""));
            var utilidad = Number(txtUtilidadDLLS.val().replace(/[^0-9.-]+/g,""));
                
            totalCosto =  parseFloat(depreciacion) + parseFloat(inversion) + parseFloat(seguro) + parseFloat(filtro) +
            parseFloat(mantenimientoCo) + parseFloat(manoObra) + parseFloat(auxiliar) + parseFloat(indirectos) + parseFloat(depreciacionOH) +  
            parseFloat(aceite) + parseFloat(ansul) + parseFloat(carilleria) + parseFloat(utilidad);

            txtTotalDLLS.val('$' + totalCosto.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")); 

            var dlls = Number(txtTotalDLLS.val().replace(/[^0-9.-]+/g,""));
            var tipo = tipoDeCambioModal.val();          
             
            
            totalMxn = parseFloat(dlls) * parseFloat(tipo);
            txtTotalMXN.val('$' + totalMxn.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));           
            hablitar();  
        }

        btnSacarSuma.click(function(){
            suma();
        });

        function suma(){
            if (fncCamposVacios()) {
                Alert2Warning("Favor de llenar todos los controles");
            }
            else{
                let total = 0;

                var depreciacion = txtDepreciacionDLLS.val();
                var inversion = txtInversionDLLS.val();
                var seguro = txtSeguroDLLS.val();
                var filtro = txtFiltroDLLS.val();
                var mantenimientoCo = txtMantenimientoDLLS.val();
                var manoObra = txtManoObraMXN.val();
                var auxiliar = txtAuxiliarDLLS.val();
                var indirectos = txtIndirectosDLLS.val();
                var depreciacionOH = txtDepreciacionOHDLLS.val();
                var aceite = txtAceiteDLLS.val();
                var ansul = txtAnsulDLLS.val();
                var carilleria = txtCarilleriaDLLS.val();
                var utilidad = txtUtilidadDLLS.val();
                
                total =  parseFloat(depreciacion) + parseFloat(inversion) + parseFloat(seguro) + parseFloat(filtro) +
                parseFloat(mantenimientoCo) + parseFloat(manoObra) + parseFloat(auxiliar) + parseFloat(indirectos) + parseFloat(depreciacionOH) +  
                parseFloat(aceite) + parseFloat(ansul) + parseFloat(carilleria) + parseFloat(utilidad);

                txtTotalDLLS.val(total); 
                hablitar();                
            }
            
        }

        function limpiarInputs() {
            cboGrupo2.val("");
            cboGrupo2.trigger("change");
            cboModelo2.val("");
            cboModelo2.trigger("change");
            cboAgrupaciones.val('');
            cboAgrupaciones.trigger("change");
            txtDepreciacionDLLS.val("$0.00");
            txtInversionDLLS.val("$0.00");
            txtSeguroDLLS.val("$0.00");
            txtFiltroDLLS.val("$0.00");
            txtMantenimientoDLLS.val("$0.00");
            txtAuxiliarDLLS.val("$0.00");
            txtIndirectosDLLS.val("$0.00");
            txtDepreciacionOHDLLS.val("$0.00");
            txtAceiteDLLS.val("$0.00");
            txtAnsulDLLS.val("$0.00");
            txtCarilleriaDLLS.val("$0.00");
            txtUtilidadDLLS.val("$0.00");
            txtTotalDLLS.val("$0.00");
            txtDepreciacionMXN.val("$0.00");
            txtInversionMXN.val("$0.00");
            txtSeguroMXN.val("$0.00");
            txtFiltroMXN.val("$0.00");
            txtMantenimientoMXN.val("$0.00");
            txtAuxiliarMXN.val("$0.00");
            txtIndirectosMXN.val("$0.00");
            txtDepreciacionOHMXN.val("$0.00");
            txtAceiteMXN.val("$0.00");
            txtAnsulMXN.val("$0.00");
            txtCarilleriaMXN.val("$0.00");
            txtUtilidadMXN.val("$0.00");
            txtTotalMXN.val("$0.00");
            txtManoObraMXN.val("$0.00");
            txtManoObraDLLS.val("$0.00");
            tipoDeCambioModal.val(TipoCambio.val());
        }

        function fncGetCaratula() {          
            axios.post("GetCaratula").then(response => {
                let { success, items, message } = response.data;
                if (success) {                    
                    dtCaratula.rows.add(response.data.lstCaratula).draw();                    
                }
                else {
                    Alert2Error(message)
                }
    
            }).catch(error => Alert2Error(error.message));
        }

        btnGuardarCaratula.click(function(){
            fncGuadarCaratula();
        });

        function fncGuadarCaratula() {
            let listaCaratula = dtCaratula.data().toArray();
            
            if(TipoCambio.val() == "")
            {
                Alert2Error("Se requiere especificar el tipo de cambio correspondiente.");
                return;
            }
            //if(DireccionTecnica.data("id") == null || SubdireccionMaquinaria.data("id") == null)
            //{
            //    Alert2Error("Se requiere especificar a todos los autorizantes correspondientes.");
            //    return;
            //}
            axios.post('/Caratulas/GuardarCaratula', 
                { 
                    listaCaratulaStr: JSON.stringify(listaCaratula), 
                    tipoCambio: +TipoCambio.val(), 
                    //idTecnico:DireccionTecnica.data("id"),
                    //idSubdireccionMaquinaria:SubdireccionMaquinaria.data("id")
                    idTecnico:1170,
                    idSubdireccionMaquinaria:3314
                })
                .then(response => {
                let { success, items, message } = response.data;                    
                if (success) {     
                    enviarCorreoGuardarCaratula();
                    Alert2Exito("La caratula se guardo correctamente");  
                }
                else{
                    Alert2Error("No se puede agregar el mismo autorizante.");
                }
            }).catch(error => Alert2Error(error.message));
            
        }
        function CargarCaratulaActiva()
        {
            axios.post('/Caratulas/CargarCaratulaActiva', { lstTipoHoraDia: cboCostoPor.val() })
            .then(response => {
                if (response.data.success) {
                    TipoCambio.val(response.data.tipoCambio);
                    tipoDeCambioModal.val(response.data.tipoCambio);
                    conceptosMoneda = response.data.conceptosMoneda;
                    // console.log(response.data);
                    DatosCaratula = [];
                    DatosCaratula = response.data.data;
                    dtCaratula.clear().draw();
                    dtCaratula.rows.add(DatosCaratula).draw();      
                    

                    let lstString =[]
                    for (let index = 0; index < DatosCaratula.length; index++) {
                        let item ={};
                        let nombreModelo = "";
                        for (let b = 0; b < DatosCaratula[index].stringModelo.length; b++) {
                            nombreModelo += 'Modelo : ' + DatosCaratula[index].stringModelo[b] + ', ' ;
                        }
                        item = {
                            title : nombreModelo
                        };
                        lstString.push(item);
                    }


                    var tr = $('#tblCaratula').find('tbody').find('tr')
                    for (let i = 0; i < tr.length; i++) {
                        $(tr[i]).attr('data-toggle','toltips');
                        $(tr[i]).attr('title',lstString[i].title);
                    }
                    $('[data-toggle="tooltip"]').tooltip();

                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function enviarCorreoGuardarCaratula() {
            $.post('EnviarCorreoGuardarCaratula')
                .done(respuesta => {
                    if (respuesta.success) {        
                        Alert2AccionConfirmar('Caratula','Se han enviado correo a los autorizantes.','Confirmar','Salir');                        
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

    $(document).ready(() => Caratula = new Caratula())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();