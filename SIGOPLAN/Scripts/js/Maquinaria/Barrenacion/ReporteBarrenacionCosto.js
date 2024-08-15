(() => {
    $.namespace('Barrenacion.ReporteBarrenacionCosto');

ReporteBarrenacionCosto = function () {

        //#region PETICIONES
    const guardarBarrenacion = (registroInformacion, lstPiezaDetalle, lstPiezaOtroDetalle) => { return $.post('/Barrenacion/GuardarBarrenacionCosto', { registroInformacion, lstPiezaDetalle, lstOtroDetalle }) };
        //const guardarBarrenacion = (registroInformacion) => { return $.post('/Barrenacion/GuardarBarrenacionCosto', { registroInformacion}) };
        const getBarrenacionCosto = () => { return $.post('/Barrenacion/GetBarrenacionCosto', { }) };
        // Filtros
        const comboAC = $('#comboAC');
        //const comboTipoPieza = $('#comboTipoPieza');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const inputFechaCosto = $('#inputFechaCosto');
        const botonBuscar = $('#botonBuscar');
        //raguilar
        const botonAgregar = $('#botonAgregar');
        const modalAgregar = $('#modalRegistroBarrenacionCosto');
        const btnAddPieza = $('#btnAddPieza');
        const btnAddOtro = $('#btnAddOtro');
        const btnGuardar =$('#btnGuardar'); 
        const txtMano = $("#txtMano");
        const txtCostoRenta = $("#txtCostoRenta");
        const txtDiesel =  $("#txtDiesel");
        const txtTotalCostoBarrenacion=  $("#txtTotalCostoBarrenacion");
        const txtResumenPiezaTotal = $("#txtResumenPiezaTotal")
        const txtResumenOtroTotal = $("#txtResumenOtroTotal")
        // Tabla capturas 
        const tablaPiezas = $('#tablaPiezas');
        let dtTablaPiezas;


        // Reporte
        const report = $("#report");


    (function init() {
        // Lógica de inicialización.
        comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
        //GetBarrenacionCosto();
        inputFechaCosto.datepicker({
            "dateFormat": "MM",
            "changeMonth": true,
            "changeYear": true,
            "changeDay": false,
            "showButtonPanel": true,
            "maxDate": new Date()
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", new Date())

        inputFechaInicio.datepicker({
            //"dateFormat": "dd/mm/yy",
            "dateFormat": "dd/mm/yy",
            "maxDate": new Date()
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", new Date())


        inputFechaFin.datepicker({
            "dateFormat": "dd/mm/yy",
            "maxDate": new Date()
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", new Date());
        initTablaPiezas();
    })();
        
        //eventos
        botonAgregar.click(function () {
            modalAgregar.modal('show');
        });
        btnAddOtro.click(function () {
            $("#tblOtro").append(plantillaOtro);
        });
        btnAddPieza.click(function () {
            $("#tblPieza").append(plantillaPieza);
        });
        btnGuardar.click(function () {
            lstPiezaDetalle = getObjRegistroPiezaDetalle();
            lstOtroDetalle = getObjRegistroOtroDetalle();
            var a = 5;
            guardarBarrenacion(getObjRegistro(), lstPiezaDetalle,lstOtroDetalle).done(function (response) {
                if (response.success) {
                    AlertaGeneral('Aviso', 'Se guardo con exito')
                    $('#modalRegistro').modal('hide');
                    tblcolaboradores.ajax.reload(null, false);
                    setDatosInicio()
                } else {
                    AlertaGeneral('Aviso', response.error)
                }
            })
        });

    //eventos plantilla
    
        $('#tblPieza').on('click', 'tr td .btndrop', function (evt) {
            $(this).closest("tr").remove();
        });
        $('#tblOtro').on('click', 'tr td .btndrop', function (evt) {
            $(this).closest("tr").remove();
        });

        //evento pieza
        $('#tblPieza').on('blur', 'tr td .txtPrecioPieza', function (evt) {
            //sumaTotal = 0;
            //$('#tblPieza tr').each(function (index, tr) {
            //    valorPrecioPieza = $('#tblPieza').find("tr td .txtPrecioPieza").eq(index).val();
            //    valorPrecioCantidad = $('#tblPieza').find("tr td .txtCantidadPieza").eq(index).val();
            //    valorTotalPieza = (valorPrecioPieza*valorPrecioCantidad);
            //    //valorLostEmp = parseInt($('#tablaPadre').find("tr td .txtTotalPieza").eq(index).val());
            //    if (!isNaN(valorTotalPieza)) {
            //        $('#tblPieza').find("tr td .txtTotalPieza").eq(index).val(valorTotalPieza.toFixed(2))
            //        sumaTotal = valorTotalPieza+ sumaTotal;
            //    }
            //});
            //txtResumenPiezaTotal.val(sumaTotal.toFixed(2));
            calculoTotalPrincipal()
        });
        //evento Otro
        $('#tblOtro').on('blur', 'tr td .txtPrecioOtro', function (evt) {
            //sumaTotal = 0;
            //$('#tblOtro tr').each(function (index, tr) {
            //    valorPrecioPieza = $('#tblOtro').find("tr td .txtPrecioOtro").eq(index).val();
            //    valorPrecioCantidad = $('#tblOtro').find("tr td .txtCantidadOtro").eq(index).val();
            //    valorTotalPieza = (valorPrecioPieza*valorPrecioCantidad);
            //    //valorLostEmp = parseInt($('#tablaPadre').find("tr td .txtTotalPieza").eq(index).val());
            //    if (!isNaN(valorTotalPieza)) {
            //        $('#tblOtro').find("tr td .txtTotalOtro").eq(index).val(valorTotalPieza.toFixed(2))
            //        sumaTotal = valorTotalPieza+ sumaTotal;
            //    }
            //});
            //txtResumenOtroTotal.val(sumaTotal.toFixed(2));
            calculoTotalPrincipal()
        });
        $('#tblOtro').on('blur', 'tr td .txtCantidadOtro', function (evt) {
            calculoTotalPrincipal()
        });
        $('#tblPieza').on('blur', 'tr td .txtCantidadPieza', function (evt) {
            calculoTotalPrincipal()
        });
        function calculoTotalPrincipal()   {
            sumaTotal = 0;
            $('#tblOtro tr').each(function (index, tr) {
                valorPrecioPieza = $('#tblOtro').find("tr td .txtPrecioOtro").eq(index).val();
                valorPrecioCantidad = $('#tblOtro').find("tr td .txtCantidadOtro").eq(index).val();
                valorTotalPieza = (valorPrecioPieza*valorPrecioCantidad);
                //valorLostEmp = parseInt($('#tablaPadre').find("tr td .txtTotalPieza").eq(index).val());
                if (!isNaN(valorTotalPieza)) {
                    $('#tblOtro').find("tr td .txtTotalOtro").eq(index).val(valorTotalPieza.toFixed(2))
                    sumaTotal = valorTotalPieza+ sumaTotal;
                }
                if ($('#tblOtro tr').length== (index+1)) {
                    txtResumenOtroTotal.val(sumaTotal.toFixed(2));
                }
            });

            $('#tblPieza tr').each(function (index, tr) {
                valorPrecioPieza = $('#tblPieza').find("tr td .txtPrecioPieza").eq(index).val();
                valorPrecioCantidad = $('#tblPieza').find("tr td .txtCantidadPieza").eq(index).val();
                valorTotalPieza = (valorPrecioPieza*valorPrecioCantidad);
                //valorLostEmp = parseInt($('#tablaPadre').find("tr td .txtTotalPieza").eq(index).val());
                if (!isNaN(valorTotalPieza)) {
                    $('#tblPieza').find("tr td .txtTotalPieza").eq(index).val(valorTotalPieza.toFixed(2))
                    sumaTotal = valorTotalPieza+ sumaTotal;
                }
                if ($('#tblPieza tr').length== (index+1)) {
                    txtResumenPiezaTotal.val(sumaTotal.toFixed(2));
                }
            });
            txtTotalCostoBarrenacion.val(sumaTotal.toFixed(2));
        }


        //plantillas append
        //agregar a plantilla
        var plantillaPieza = "<tr class='rowPuesto' >"
            + "<td>"
            //+ "<input class='form-control claveEmpleado' type='number' min='1' max='5'>"
            +'<select id="cboPieza" class="browser-default custom-select" style="width:100%">'
                  //+'<option selected>Selecciona La Pieza</option>'
                  +'<option hidden >Selecciona la Pieza</option>'
                  +'<option value="1">Broca</option>'
                  +'<option value="2">Martillo</option>'
                  +'<option value="3">Barra</option>'
                  +'<option value="4">Culata</option>'
                  +'<option value="5">Portabit</option>'
                  +'<option value="6">Cilindro</option>'
            +'</select>'
            + "</td>"
            + "<td>"
           + "<input class='form-control txtCantidadPieza' type='number' min='1' max='5'>"
            + "</td>"
            + "<td>"
            + "<input class='form-control txtPrecioPieza' type='number' min='1' max='5'>"
            + "</td>"
            + "<td>"
                 + "<input class='form-control txtTotalPieza' type='number' min='1' max='5' readonly>"
            + "</td>"
            + "<td>"
            + "<button class='btndrop' type='button'> <span class='glyphicon glyphicon-trash'></span></button>"
            + "</td>"
            + "</tr>";
        //agregar a plantilla
        var plantillaOtro = "<tr class='rowPuesto' >"
            + "<td>"
            + "<input class='form-control txtConceptoOtro' type='text' min='1' max='5'>"
            + "</td>"
            + "</td>"
            + "<td>"
           + "<input class='form-control txtCantidadOtro' type='number' min='1' max='5'>"
            + "</td>"
            + "<td>"
            + "<input class='form-control txtPrecioOtro' type='number' min='1' max='5'>"
            + "</td>"
            + "<td>"
                 + "<input class='form-control txtTotalOtro' type='number' readonly>"
            + "</td>"
            + "<td>"
            + "<button class='btndrop' type='button'> <span class='glyphicon glyphicon-trash'></span></button>"
            + "</td>"
            + "</tr>";

    //metodos
        function GetBarrenacionCosto(){
            getBarrenacionCosto().done(function (response) {
                if (response.success) {
                    //AlertaGeneral('Aviso', 'Se guardo con exito')
                    $('#modalRegistro').modal('hide');
                    tblcolaboradores.ajax.reload(null, false);
                    setDatosInicio()
                } else {
                    AlertaGeneral('Aviso', response.error)
                }
            })
        }

        function getObjRegistro() {
            const registroInformacion = {
                manoObra:txtMano.val(),
                costoRenta: txtCostoRenta.val(),
                diesel: txtDiesel.val(),
                totalCosto: txtTotalCostoBarrenacion.val()
                //public int id { get; set; }
                //public decimal  manoObra { get; set; }
                //public decimal  costoRenta { get; set; }
                //public decimal  diesel { get; set; }
                //public decimal  totalCosto { get; set; }
                //public decimal  activa { get; set; }
                //public DateTime  fechaCreacion { get; set; }
                //public int usuarioCreadorID { get; set; }    
            }
            return registroInformacion
        }
        function getObjRegistroPiezaDetalle() {
            lstPiezaDetalle = []
            flagCorrecto = true;
            var longitud = $("#tblPieza >tbody >tr").length
            $('#tablaPadrePieza tbody tr').each(function (index, tr) {
                //cboPieza
                var idPieza = $('#tblPieza').find("tr td #cboPieza").eq(index).val();
                var cantidadPieza = $('#tblPieza').find("tr td .txtCantidadPieza").eq(index).val();
                var precioUnitarioPieza = $('#tblPieza').find("tr td .txtPrecioPieza").eq(index).val();
                var totalPieza = $('#tblPieza').find("tr td .txtTotalPieza").eq(index).val();
                //if (cveEmpleado == "" || nombre == "" || lostDayEmpleado == "") {
                //    AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
                //    flagCorrecto = false;
                //} else {
                const objDetalle = {
                        idPieza:idPieza,    
                        cantidadPieza: cantidadPieza,
                        precioUnitarioPieza: precioUnitarioPieza,
                        totalPieza: totalPieza
                    }
                    const obj = Object.create(objDetalle);
                    lstPiezaDetalle.push(objDetalle)
                //}
            });
            //if (flagCorrecto != false && lstPiezaDetalle.length > 0) {//si no hay errores y no hay detalle
            return lstPiezaDetalle;
            //} else if (lstPiezaDetalle.length == 0 && flagCorrecto != false) {//si no hay errores pero tampoco detalle
            //    return true;
            //} else {
            //    return false;//si hay errores
            //}
        }
        function getObjRegistroOtroDetalle() {
            lstOtroDetalle = []
            flagCorrecto = true;
            var longitud = $("#tblOtro >tbody >tr").length
            $('#tablaPadreOtro tbody tr').each(function (index, tr) {
                var conceptoOtro = $('#tblOtro').find("tr td .txtConceptoOtro").eq(index).val();
                var cantidadOtro = $('#tblOtro').find("tr td .txtCantidadOtro").eq(index).val();
                var precioUnitarioOtro = $('#tblOtro').find("tr td .txtPrecioOtro").eq(index).val();
                var totalOtro = $('#tblOtro').find("tr td .txtTotalOtro").eq(index).val();
                //if (cveEmpleado == "" || nombre == "" || lostDayEmpleado == "") {
                //    AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
                //    flagCorrecto = false;
                //} else {
                const objDetalle = {
                    conceptoOtro : conceptoOtro,
                    cantidadOtro: cantidadOtro,
                    precioUnitarioOtro: precioUnitarioOtro,
                    totalOtro: totalOtro
                }
                const obj = Object.create(objDetalle);
                lstOtroDetalle.push(objDetalle)
                //}
            });
            //if (flagCorrecto != false && lstOtroDetalle.length > 0) {//si no hay errores y no hay detalle
                return lstOtroDetalle;
            //} else if (lstOtroDetalle.length == 0 && flagCorrecto != false) {//si no hay errores pero tampoco detalle
            //    return true;
            //} else {
            //    return false;//si hay errores
            //}
        }

        function initTablaPiezas() {
            dtTablaPiezas = tablaPiezas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                ajax: {
                    url: 'getBarrenacionCosto',
                    dataSrc: function (response) {
                        if (response.EMPTY) {
                            return [];
                        } else {
                            return response.items;
                        }
                    }
                },language: dtDicEsp,
                "aaSorting": [4, 'desc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                },
                columns: [
                    {
                        data: 'id', title: 'id'
                    },
                    {
                        data: 'manoObra', title: 'Mano de Obra'
                    },
                    {
                        data: 'costoRenta', title: 'Costo Renta'
                    },
                    {
                        data: 'diesel', title: 'Diesel'
                    },
                    {
                        data: 'fechaCosto', title: 'fecha', render: (data, type,row) => 
                        $.toDate(row.fechaCosto)
                    },
                    {
                        data: 'id', title: 'Piezas', render: (data, type, row) => row.totalPieza== null?
                      '<button class="btn btn-default reporte" id="0"><i class="fas fa-tools"></i>  '+0+'</button>':
                      '<button class="btn btn-warning reporte" id="'+row.id+'"><i class="fas fa-tools"></i>  '+row.totalPieza+'</button>'
                    },
                    {
                        data: 'id', title: 'Otros', render: (data, type, row) => row.totalOtro== null?
                      //'<button class="btn btn-primary reporte"><i class="fas fa-print"></i></button>'
                      '<button class="btn btn-default reporte" id="0"><i class="fas fa-dollar-sign"></i>  '+0+'</button>':
                      '<button class="btn btn-warning  reporte" id="'+row.id+'"><i class="fas fa-dollar-sign"></i>  '+row.totalOtro+'</button>'
                        //<i class="fas fa-dollar-sign"></i>
                    },
                    {
                        data: 'totalCosto', title: 'totalCosto', render: (data, type,row) => 
                            '<h3>'+row.totalCosto+'</h3>'
                    },
                    {
                        data: 'id', title: 'Reporte', render: (data, type, row) =>
                            '<button class="btn btn-primary reporte"><i class="fas fa-print"></i></button>'
                    }
                ],
                columnDefs: [{ className: "dt-center", "targets": "_all" }],
                drawCallback: function () {
                    tablaPiezas.find('.reporte').unbind().click(function () {
                        const piezaID = dtTablaPiezas.row($(this).parents('tr')).data().piezaID;
                        const fechaInicio = inputFechaInicio.val();
                        const fechaFin = inputFechaFin.val();
                        verReporte(piezaID, fechaInicio, fechaFin);
                    });
                }
            });
        }
        //function verReporte(piezaID, fechaInicio, fechaFin) {
        //    var path = `/Reportes/Vista.aspx?idReporte=166&piezaID=${piezaID}&isCRModal=${true}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
        //    report.attr("src", path);
        //    document.getElementById('report').onload = function () {
        //        openCRModal();
        //    };
        //}

    }

$(() => Barrenacion.ReporteBarrenacionCosto = new ReporteBarrenacionCosto())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();