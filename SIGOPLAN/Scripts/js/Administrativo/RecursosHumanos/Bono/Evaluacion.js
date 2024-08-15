(() => {
$.namespace('Administrativo.RecursosHumanos.Evaluacion');
Evaluacion = function (){
    let _currentObj = 0;
    var G_PlantillaID =0;
        let itemsPorcentaje;
        const cboSetTodoSel = $("#cboSetTodoSel");
        const cboSetTodo = $("#cboSetTodo");
        const cboTipoNomina = $("#cboTipoNomina");
        const selCC = $('#selCC');
        const tblBono = $('#tblBono');
        const mpPeriodo = $('#mpPeriodo');
        const btnGuardar = $('#btnGuardar');
        const autoInput = $('.ui-autocomplete-input');
        const btnCargar = $('#btnCargar');

        const btnExcluidos = $("#btnExcluidos");
        const modalExcluidos = $('#modalExcluidos');
        const tblExcluidos = $('#tblExcluidos');

        var anio = 0;
        const getItemPorcentaje = new URL(window.location.origin + '/Administrativo/Bono/FillCboPorcentaje');
        const getEmpleados = new URL(window.location.origin + '/Administrativo/Bono/getEmpleadosEvaluar');
        const GuardarEvaluacion = new URL(window.location.origin + '/Administrativo/Bono/guardarEvaluacion');
        const ActualizarEvaluacion = new URL(window.location.origin + '/Administrativo/Bono/actualizarEvaluacion');
        const GuardarEvaluacionDet = new URL(window.location.origin + '/Administrativo/Bono/guardarEvaluacionDet');
        var lstExcluidos = null;
        let init = () => {
            var d = new Date();
            anio = d.getFullYear();
            
            initForm();
            
            $('#modalExcluidos').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
        }
        function loadExcluidos(){
            dtExcluidos.clear().draw();
            dtExcluidos.rows.add(lstExcluidos).draw();  
            modalExcluidos.modal("show");
        }
        function initDataTblExcluidos() {
            dtExcluidos = tblExcluidos.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Lista de excluidos", "<center><h3>Lista de empleados excluidos para bono desempeño mensual</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [0, 1, 2,3,4,5,6,7,8,9,10]
                        }
                    }
                ],
                columns: [
                    { title:'Cve' ,data: 'cve_Emp'}
                   ,{ title:'Nombre' ,data: 'nombre_Emp'}
                   ,{ title:'Cve puesto' ,data: 'puestoCve_Emp'}
                   ,{ title:'Puesto' ,data: 'puesto_Emp'}
                   ,{ title:'Fecha alta' ,data: 'fechaAlta'}
                   ,{ title:'Antiguedad (dias)' ,data: 'diasAntiguedad'}
                   ,{ title:'Antiguedad requerida (dias)' ,data: 'diasOBraParaBono'}
                   ,{ title:'Bono sistema' ,data: 'isBonoSistema'}
                   ,{ title:'Lista negra' ,data: 'isLstNegra'}
                   ,{ title:'Antiguedad' ,data: 'isAntiguedad'}
                   ,{ title:'Bono plantilla' ,data: 'isPlantilla'}
                ]
            });
        }
        async function setItemPorcentaje() {
            try {
                response = await ejectFetchJson(getItemPorcentaje);
                if (response.success) {
                    itemsPorcentaje = response.items;
                }
            } catch (o_O) { }
        }
        function fnSetAll(){
            var _this = $(this);
            $(".porcentaje_Asig").val(_this.val());
            $(".porcentaje_Asig").change();
        }
        function initDataTblBono() {
            dtBono = tblBono.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columns: [
                    { title:'Cve' ,data: 'cve_Emp'}
                   ,{ title:'Nombre' ,data: 'nombre_Emp'}
                   ,{ title:'Puesto' ,data: 'puesto_Emp'}
                   ,{
                       title:'T.Nom' ,data: 'tipo_Nom' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control tipo_Nom disabled`);
                           $(td).find(`input`).val(rowData.tipo_Nom);
                       }
                   }
                   ,{
                       title:'Base' ,data: 'base_Emp' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control Base disabled`);
                           $(td).find(`input`).val(maskNumero(rowData.base_Emp));
                       }
                   }
                   ,{
                       title:'Comp' ,data: 'complemento_Emp' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control Complemento disabled`);
                           $(td).find(`input`).val(maskNumero(rowData.complemento_Emp));
                       }
                   }
                   ,{
                       title:'Total Nom' ,data: 'total_Nom' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control total_Nom disabled`);
                           if(rowData.evaluacionID==0){
                               $(td).find(`input`).val(maskNumero(rowData.base_Emp + rowData.complemento_Emp + rowData.monto_Asig));
                           }
                           else{
                               $(td).find(`input`).val(maskNumero(rowData.total_Nom));
                           }
                       }
                   }
                   ,{
                       title:'Total Mensual' ,data: 'total_Mensual' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control total_Mensual disabled`);
                           
                           if(rowData.evaluacionID==0){
                               $(td).find(`input`).val(maskNumero(fnTotalMensual(rowData.tipoCve_Nom,rowData.periodicidadCve,rowData.bono_Emp,rowData.porcentaje_Asig,(rowData.base_Emp + rowData.complemento_Emp + rowData.monto_Asig))));
                           }
                           else{
                               $(td).find(`input`).val(maskNumero(rowData.total_Mensual));
                           }
                       }
                   }
                   ,{
                       title:'Bono Sistema' ,data: 'bono_FC' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control bono_FC disabled`);
                           $(td).find(`input`).val(maskNumero(rowData.bono_FC));
                       }
                   }
                   
                   ,{
                       title:'Bono Admin' ,data: 'bono_Emp' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control bono_Emp disabled`);
                           $(td).find(`input`).val(maskNumero(rowData.bono_Emp));
                       }
                   }
                   
                   ,{
                       title:'% asig' ,data: 'porcentaje_Asig' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<div class="select-editable" title="Selecciona o escribe un % para aplicarlo unicamente este empleado" ><select onchange="this.nextElementSibling.value=this.value" class="form-control"></select><input type="text" name="format" value="" class="form-control"/></div>`);
                           $(td).find(`input`).addClass(`porcentaje_Asig`);
                           $(td).find(`select`).fillCombo('/Administrativo/Bono/FillCboPorcentaje',{},true);
                           $(td).find(`select`).val(rowData.porcentaje_Asig);
                           
                           
                           $(td).find(`select`).change(function(){
                               $(this).siblings().change();
                           });
                           if(rowData.evaluacionID>0){
                               $(td).find(`input`).val(rowData.porcentaje_Asig);
                           }
                           else{
                               $(td).find(`input`).val(0);
                           }
                           
                       }
                   }
                   ,{
                       title:'$ asignado' ,data: 'monto_Asig' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control monto_Asig disabled`);
                           
                           if(rowData.evaluacionID==0){
                               $(td).find(`input`).val(maskNumero((data / 100) * rowData.bono_Emp));
                           }
                           else{
                               $(td).find(`input`).val(maskNumero(rowData.monto_Asig));
                           }
                       }
                   },{
                       title:'Con Bono' ,data: 'con_Bono' ,createdCell: function (td, data, rowData, row, col) {
                           $(td).html(`<input disabled>`);
                           $(td).find(`input`).addClass(`form-control con_bono disabled`);
                           
                           if(rowData.evaluacionID==0){
                               $(td).find(`input`).val(maskNumero(fnTotalMensual(rowData.tipoCve_Nom,rowData.periodicidadCve,rowData.bono_Emp,rowData.porcentaje_Asig,(rowData.base_Emp + rowData.complemento_Emp + rowData.monto_Asig))));
                           }
                           else{
                               $(td).find(`input`).val(maskNumero(rowData.con_Bono));
                           }
                       }
                   }
                   ,{ title:'Periodicidad' ,data: 'periodicidad'}
                   
                   
                ]
                ,
columnDefs: [
            { width: 50, targets: 10 }
]
                ,initComplete: function (settings, json) {
                    tblBono.on(`change`, `.porcentaje_Asig`, function () {
                        let _this = $(this);
                        let row = _this.parent().parent().parent();
                        var data = dtBono.row(row).data();
                        var tipoNomina = data.tipoCve_Nom;
                        var bonomul = data.periodicidadCve==1?4:data.periodicidadCve==4?2:1;
                        var base = data.base_Emp;
                        var complemento = data.complemento_Emp;
                        var pa = _this.val();
                        var bonoMax = data.bono_Emp;
                        var bonoAsig = bonoMax * (pa/100);
                        var tn = base+complemento;
                        var tm = 0;
                        if(tipoNomina==1)
                        {
                            tm = ((tn / 7) * 30.4); 
                        }
                        else if(tipoNomina==4)
                        {
                            tm = (tn * 2);
                        }
                        else if(tipoNomina==5)
                        {
                            tm = tn + (bonoAsig * bonomul);
                        }
                        else{
                            tm = tn + (bonoAsig * bonomul);
                        }
                        
                        row.find(".total_Nom").val(maskNumero(tn));
                        row.find(".total_Mensual").val(maskNumero(tm));
                        row.find(".monto_Asig").val(maskNumero(bonoAsig));
                        row.find(".con_bono").val(maskNumero(tm+bonoAsig));
                  
                    });
                }
            });
        }
        function fnTotalMensual(tipo,periodicidad,fnbono,fnPorcentaje,fnnomina)
        {
            var bono = (fnPorcentaje/100)*unmaskNumero(''+fnbono);
            var nomina = unmaskNumero(''+fnnomina);
            var total =0;
            var bonomul = periodicidad==1?4:periodicidad==4?2:1;

            if(tipo==1)
            {
                total = ((nomina / 7) * 30.4); 
            }
            else if(tipo==4)
            {
                total = (nomina * 2) ;
            }
            else if(tipo==5)
            {
                total = nomina;
            }
            else{
                total = nomina ;
            }
            return total;
        }
        function setMes(inp, fecha) {
            let currentMonth = new Date().getMonth()
               ,currentYear = new Date().getFullYear()
               ,mes = fecha.getMonth()
               ,anio = fecha.getFullYear();
            inp.MonthPicker({ SelectedMonth: (mes - currentMonth - 1), SelectedYear: (anio - currentYear) });
        }
        async function fnCargarEmpleados() {
            try {
                let cc = selCC.val(), tipoNomina = cboTipoNomina.val(), periodo = mpPeriodo.val();
                let fechaPeriodo = $("#mpPeriodo option:selected").data("comboid");
                dtBono.clear().draw();
                if (cc.length > 0) {
                    response = await ejectFetchJson(getEmpleados, { cc , periodo , tipoNomina, fechaPeriodo});
                    if(response.lst != undefined){
                        var estatus = response.estatus;
                        _currentObj = response.evaluacionID;
                        G_PlantillaID = response.lst[0].plantillaID;
                        
                        
                        //Autorizado
                        if(estatus == 1){
                            $("#divAceptacion").hide();
                            cboSetTodo.val(0);
                            btnGuardar.hide();
                            btnExcluidos.hide();
                            $('.dataTables_scrollHeadInner th').each( function (i,e) {
                                if(i==10){
                                    $("#cboSetTodoSel").unbind();
                                    $("#cboSetTodo").unbind();
                                }
                            });   
                            AlertaGeneral("Aviso", "Esta evaluación ya fue generada y Autorizada por lo que no se puede modificar");
                        }
                        //Rechazado
                        else if(estatus == 2){
                            $("#divAceptacion").hide();
                            cboSetTodo.val(0);
                            btnGuardar.hide();
                            btnExcluidos.hide();
                            $('.dataTables_scrollHeadInner th').each( function (i,e) {
                                if(i==10){
                                    $("#cboSetTodoSel").unbind();
                                    $("#cboSetTodo").unbind();
                                }
                            });   
                            $('.dataTables_scrollHeadInner th').each( function (i,e) {
                                if(i==10){
                                    $("#cboSetTodoSel").unbind();
                                    $("#cboSetTodo").unbind();
                                }
                            }); 
                            AlertaGeneral("Aviso", "Esta evaluación ya fue generada y Rechazada por lo que no se puede modificar");
                        }
                        //Autorizando
                        else if(estatus == 3){
                            $("#divAceptacion").hide();
                            cboSetTodo.prop("disabled",false);
                            cboSetTodo.val(0);
                            
                            btnGuardar.html("Actualizar");
                            btnGuardar.show();
                            btnExcluidos.hide();
                            $('.dataTables_scrollHeadInner th').each( function (i,e) {
                                if(i==10){
                                    $("#cboSetTodoSel").unbind();
                                    $("#cboSetTodo").unbind();
                                    $(e).html( '<b>% Asig</b><div title="Selecciona o escribe un % para aplicarlos a todos los empleados" class="select-editable clsOne"><select id="cboSetTodoSel" onchange="this.nextElementSibling.value=this.value" class="form-control"></select><input id="cboSetTodo" type="text" name="format" value="" class="form-control"/></div>' );
                                    $("#cboSetTodoSel").fillCombo('/Administrativo/Bono/FillCboPorcentaje',{},true);
                                    $("#cboSetTodo").change(fnSetAll);
                                    $("#cboSetTodo").val(0);
                                    $("#cboSetTodo").change();
                                    $("#cboSetTodoSel").change(fnSetAll);
                                }
                            });   
                            AlertaGeneral("Aviso", "Esta evaluación ya fue generada y esta en proceso de autorización, aun se puede actualizar");
                        }
                        //No existe
                        else{
                            $("#divAceptacion").show();
                            cboSetTodo.prop("disabled",false);
                            cboSetTodo.val(0);
                            cboSetTodo.change();
                            btnGuardar.html("Guardar");
                            btnGuardar.show();
                            btnExcluidos.show();
                            $('.dataTables_scrollHeadInner th').each( function (i,e) {
                                if(i==10){
                                    $("#cboSetTodoSel").unbind();
                                    $("#cboSetTodo").unbind();
                                    $(e).html( '<b>% Asig</b><div title="Selecciona o escribe un % para aplicarlos a todos los empleados" class="select-editable clsOne"><select id="cboSetTodoSel" onchange="this.nextElementSibling.value=this.value" class="form-control"></select><input id="cboSetTodo" type="text" name="format" value="" class="form-control"/></div>' );
                                    $("#cboSetTodoSel").fillCombo('/Administrativo/Bono/FillCboPorcentaje',{},true);
                                    $("#cboSetTodo").change(fnSetAll);
                                    $("#cboSetTodo").val(0);
                                    $("#cboSetTodo").change();
                                    $("#cboSetTodoSel").change(fnSetAll);
                                }
                            });   
                        }
                        dtBono.rows.add(response.lst).draw();    
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().responsive.recalc();
                        $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().fixedColumns().relayout();
                        lstExcluidos = null;
                        lstExcluidos = response.lstnovalidos;
                    }
                    else{
                        G_PlantillaID = 0;
                        AlertaGeneral("Aviso", "El CC no tiene una plantilla de bonos autorizada o ninguno de los empleados es apto para el bono");
                    }
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function setGuardarBono() {
            if(_currentObj>0)
            {
                try {
                    var dataExist = false;
                    var authExist = true;
                    var obj = {};
                    var det = [];
                    obj.id = _currentObj;
                    obj.periodo=mpPeriodo.val();
                    obj.fechaInicio=$("#mpPeriodo option:selected").data("comboid");
                    obj.fechaFin=$("#mpPeriodo option:selected").data("prefijo");

                    dtBono.rows().every(function (rowIdx, tableLoop, rowLoop ) {
                        let node = $(this.node());
                        let data = this.data();
                        //if(node.find('.porcentaje_Asig').val()!=0){
                            var o = {};
                            o.id = data.id;
                            o.porcentaje_Asig = unmaskNumero(node.find('.porcentaje_Asig').val());
                            o.monto_Asig = unmaskNumero(node.find('.monto_Asig').val());
                            o.con_Bono = unmaskNumero(node.find('.con_bono').val());
                            det.push(o);
                        //}
                    });
                    
                    response = await ejectFetchJson(ActualizarEvaluacion, { obj:obj,det:det});
                    if (response.success) {
                        callBackEvaluacionUpdate();
                    } else {
                        AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la evaluación.`);
                    }

                } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
            }
            else{
                try {
                    var dataExist = false;
                    var authExist = true;
                    var obj = {};
                    obj.id = 0;
                    obj.plantillaID = G_PlantillaID;
                    obj.estatus = 0;
                    obj.cc=selCC.val();
                    obj.tipoNomina=cboTipoNomina.val();
                    obj.anio=0;
                    obj.periodo=mpPeriodo.val();
                    obj.fechaInicio=$("#mpPeriodo option:selected").data("comboid");
                    obj.fechaFin=$("#mpPeriodo option:selected").data("prefijo");
                    obj.aplicado = false;
                    dtBono.rows().every(function (rowIdx, tableLoop, rowLoop ) {
                        let node = $(this.node());
                        let data = this.data();
                        if(node.find('.porcentaje_Asig').val()!=0){
                            var o = {};
                            o.id = evaluacionID;
                            o.plantillaID = data.plantillaID;
                            o.plantillaDetID = data.plantillaDetID ;
                            o.evaluacionID = data.evaluacionID;
                            o.cve_Emp = data.cve_Emp;
                            o.nombre_Emp = data.nombre_Emp;
                            o.puestoCve_Emp = data.puestoCve_Emp;
                            o.puesto_Emp = data.puesto_Emp;
                            o.tipoCve_Nom = data.tipoCve_Nom;
                            o.base_Emp = data.base_Emp;
                            o.complemento_Emp = data.complemento_Emp;
                            o.tipo_Nom = $("#cboTipoNomina option:selected").text();
                            o.total_Mensual = unmaskNumero(node.find('.total_Mensual').val());
                            o.bono_FC = data.bono_FC;
                            o.bono_Emp = data.bono_Emp;
                            o.porcentaje_Asig = unmaskNumero(node.find('.porcentaje_Asig').val());
                            o.monto_Asig = unmaskNumero(node.find('.monto_Asig').val());
                            o.total_Nom = unmaskNumero(node.find('.total_Nom').val());
                            o.con_Bono = unmaskNumero(node.find('.con_bono').val());
                            o.periodicidadCve = data.periodicidadCve;
                            dataExist = true;
                            return;
                        }

                    });
                    var aut = [];
                    $.each($(".autoriza"),function(i,e){
                    
                        var a = {};
                        if($(e).val()!=''){
                            a.evaluacionID = 0;
                            a.aprobadorClave = $(e).data().idUsuario;
                            a.aprobadorNombre = $(e).val();
                            a.aprobadorPuesto = $(e).data().puesto;
                            a.tipo = $(e).data().tipo;
                            a.estatus = 0;
                            a.firma = "S/F";
                            a.autorizando = $(e).data().orden==1?1:0;
                            a.orden = $(e).data().orden;
                            a.comentario = "";
                            aut.push(a);
                        }
                        else{
                            if($(e).data().requerido==true){
                                authExist = false;
                            }
                        }
                    });
                
                    if(dataExist){
                        if(authExist){
                            response = await ejectFetchJson(GuardarEvaluacion, { obj:obj , aut:aut});
                            if (response.success) {
                                _currentObj = response.evaluacionID;
                                var evaluacionID = response.evaluacionID;
                                var det = [];
                                dtBono.rows().every(function (rowIdx, tableLoop, rowLoop ) {
                                    let node = $(this.node());
                                    let data = this.data();
                                    //if(node.find('.porcentaje_Asig').val()!=0){
                                        var o = {};
                                        o.id = 0;
                                        o.plantillaID = data.plantillaID;
                                        o.plantillaDetID = data.plantillaDetID ;
                                        o.evaluacionID = evaluacionID;
                                        o.cve_Emp = data.cve_Emp;
                                        o.nombre_Emp = data.nombre_Emp;
                                        o.puestoCve_Emp = data.puestoCve_Emp;
                                        o.puesto_Emp = data.puesto_Emp;
                                        o.tipoCve_Nom = data.tipoCve_Nom;
                                        o.base_Emp = data.base_Emp;
                                        o.complemento_Emp = data.complemento_Emp;
                                        o.tipo_Nom = $("#cboTipoNomina option:selected").text();
                                        o.total_Mensual = unmaskNumero(node.find('.total_Mensual').val());
                                        o.bono_FC = data.bono_FC;
                                        o.bono_Emp = data.bono_Emp;
                                        o.porcentaje_Asig = unmaskNumero(node.find('.porcentaje_Asig').val());
                                        o.monto_Asig = unmaskNumero(node.find('.monto_Asig').val());
                                        o.total_Nom = unmaskNumero(node.find('.total_Nom').val());
                                        o.con_Bono = unmaskNumero(node.find('.con_bono').val());
                                        o.periodicidadCve = data.periodicidadCve;
                                        det.push(o);
                                        dataExist = true;
                                    //}

                                });
                                var lst = [];
                                lst.data = det;
                                lst.valid = dataExist;
                                var scheme = {lst : new Array()};
                                $.sm_SplittedSave(GuardarEvaluacionDet,lst.data,scheme,10,callBackEvaluacion);
                            
                            } else {
                                AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la evaluación.`);
                            }
                        }
                        else{
                            AlertaGeneral(`Error`, `Todos los autorizantes indicados como (Requerido) son obligatorios`);
                        }
                    }
                    else{
                        AlertaGeneral(`Error`, `Debe asignar minimo un porcentaje de bono para poder continuar.`);
                    }
                } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
            }
        }
        async function callBackEvaluacionUpdate() {
            try {
                $.blockUI({ message: 'Evaluación guardanda con exito, Enviando correo...' });
                var path = "/Reportes/Vista.aspx?idReporte=216&fId=" + _currentObj + "&inMemory=1&actualizacion=true";
                $("#report").attr("src", path);
                document.getElementById('report').onload = function () {
                    //AlertaGeneral("Aviso", `Evaluación guardada con éxito!.`);
                    $.unblockUI();
                    location.reload();
                };

                
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message);}
        }
        async function callBackEvaluacion() {
            try {
                $.blockUI({ message: 'Evaluación guardanda con exito, Enviando correo...' });
                var path = "/Reportes/Vista.aspx?idReporte=216&fId=" + _currentObj + "&inMemory=1";
                $("#report").attr("src", path);
                document.getElementById('report').onload = function () {
                    //AlertaGeneral("Aviso", `Evaluación guardada con éxito!.`);
                    $.unblockUI();
                    location.reload();
                };

                
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message);}
        }
        function eventAutoAuth(event, ui) {
            let inp = $(this);
            inp.val(ui.item.label);
            inp.data().idUsuario = ui.item.id;
        }
        async function fnCargaPeriodos()
        {
            mpPeriodo.fillCombo('/Administrativo/Bono/getPeriodos', { anio : anio , tipoNomina : cboTipoNomina.val() }, false);
            try {
                response = await ejectFetchJson('/Administrativo/Bono/getPeriodoActual', { tipoNomina : cboTipoNomina.val() });
                if (response.success) {
                    mpPeriodo.val(response.periodo.periodo);
                } else {
                    AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la plantilla.`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
       async function initForm () {

           mpPeriodo.fillCombo('/Administrativo/Bono/getPeriodosRestantes', { anio : anio , tipoNomina : cboTipoNomina.val() }, false);
            selCC.fillCombo('/Administrativo/Bono/getTblP_CCconPlantilla', null, false, null);
            setItemPorcentaje();
            initDataTblBono();
            initDataTblExcluidos();
            btnExcluidos.click(loadExcluidos);
            btnCargar.click(fnCargarEmpleados);
            autoInput.getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            btnGuardar.click(setGuardarBono);
            cboTipoNomina.change(fnCargaPeriodos);
            
            var cc = localStorage.getItem('cc');
            var nomina = localStorage.getItem('nomina');
            var periodo = localStorage.getItem('periodo');
            if(cc!=null)
            {
                selCC.val(cc);
                cboTipoNomina.val(nomina);
                cboTipoNomina.change();
                mpPeriodo.val(periodo);

                localStorage.removeItem('cc');
                localStorage.removeItem('nomina');
                localStorage.removeItem('periodo');
                btnCargar.click();

            }
            else{
                try {
                    cboTipoNomina.change();
                    response = await ejectFetchJson('/Administrativo/Bono/getPeriodoActual', { tipoNomina : cboTipoNomina.val() });
                    if (response.success) {
                        mpPeriodo.val(response.periodo.periodo);
                    } else {
                        AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la plantilla.`);
                    }
                } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
            }
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.Evaluacion = new Evaluacion();
    })
    //.ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    //.ajaxStop(() => { $.unblockUI(); });
})();