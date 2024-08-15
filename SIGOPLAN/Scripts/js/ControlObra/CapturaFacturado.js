(function () {
    $.namespace('controlObra.CapturaFacturado');

    CapturaFacturado = function () {
        const hoy = new Date();
        dpFechaInicio = $('#dpFechaInicio');
        btnGuardar = $('#btnGuardar');

        //tooltip actividad
        $(document).on('mouseenter', ".iffyTip", function () {
            var $this = $(this);
            if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
                $this.tooltip({
                    title: $this.text(),
                    placement: "bottom"
                });
                $this.tooltip('show');
            }
        });

        //Pintar renglon
        // $(document).on("mouseenter", "tbody tr", function () {
        //     paintCells($(this).attr('data-actividadid'));
        // });
        // $(document).on("mouseleave", "tbody tr", function () {
        //     unpaintCells($(this).attr('data-actividadid'));
        // });

        //Calcular importe
        $(document).on('change', 'input.inputVolumen', function () {
            debugger;
            const row = $(this).closest('tr');
            const pu = unmaskNumero(row.find('.PU').text()); //parseFloat(row.find('.PU').text().substring(2, row.find('.PU').text().lenght).replace(/,/g, ''));
            const volumen = $(this).val();

            row.find('.inputImporte').text(maskNumero(pu * volumen));
        });

        function init() {
            initCbo();
            $("#SelectCapitulosFiltro").change(cargarPresupuesto);
            btnGuardar.click(crearObjectoFacturado);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function crearObjectoFacturado() {
            let facturado = new Object();
            facturado = {
                capitulo_id: $('#SelectCapitulosFiltro').val(),
                fecha: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                autorizado: false,
                estatus: true
            };
            let facturadoDetalle = [];
            let detalle;
            let total = 0;

            $('#tblPresupuesto').find('tbody tr[data-actividadid]').each(function (index) {
                debugger;
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const volumen = unmaskNumero(row.find('.inputVolumen').val());
                const importe = unmaskNumero(row.find('.inputImporte').text());
                total += importe;

                if (actividad_id > 0) {
                    detalle = new Object();
                    detalle.actividad_id = actividad_id;
                    detalle.volumen = volumen;
                    detalle.importe = importe;
                    facturadoDetalle.push(detalle);
                }
            });

            if (total > 0) {
                let objFacturado = new FormData();
                objFacturado.append("facturado", JSON.stringify(facturado));
                objFacturado.append("facturadoDetalle", JSON.stringify(facturadoDetalle));

                guardarAvance(objFacturado);
            }
            else {
                AlertaGeneral('Aviso', 'Debe ingresar una cantidad en volumen facturado');
            }
        }

        function guardarAvance(objFacturado) {
            $.blockUI({ message: "Preparando información" });

            $.ajax({
                url: '/ControlObra/ControlObra/GuardarFacturadoActividad',
                data: objFacturado,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        AlertaGeneral("Aviso", "Avance guardado correctamente.");
                        recargar();
                        $.unblockUI();
                    } else {
                        AlertaGeneral("Aviso", "Error");
                        $.unblockUI();
                    }
                }
            });
        }

        function cargarPresupuesto() {
            const capitulo_id = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            $.blockUI({ message: "Preparando información" });

            $.ajax({
                type: "GET",
                async: false,
                data: {
                    listCapitulosID: capitulo_id
                },
                url: "GetSubcapitulosN1Catalogo",
                success: function (data) {
                    if (data.success) fnFillTable(data.items);
                    else AlertaGeneral('Aviso', 'Ha ocurrido un error al cargar la información');
                    $.unblockUI();
                },
                error: function (objXMLHttpRequest) {
                    limpiarTabla();
                    $.unblockUI();
                }
            });
        }

        function fnFillTable(subCapitulosN1) {
            limpiarTabla();
            const fechaInicio = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
            const subcapituloNI = subCapitulosN1;

            //TABLA
            let table = makeTabla();
            let body = document.createElement('tbody');

            if (subcapituloNI != undefined) {
                subcapituloNI.forEach(function (subcapituloNI) {

                    const actividadesN1 = obtenerActividades(subcapituloNI.id, -1, -1);
                    const subCapitulosN2 = obtenerSubcapitulosNivel2(subcapituloNI.id);

                    //SUBCAPITULO I
                    body.append(crearCapitulosHeader(subcapituloNI, 1, 8));

                    //ACTIVIDADES I
                    if (actividadesN1 != undefined) {
                        actividadesN1.forEach(function (actividad) {
                            body.append(crearActividadesRow(actividad));
                        });
                    }

                    //SUBCAPITLO II
                    if (subCapitulosN2 != undefined) {
                        subCapitulosN2.forEach(function (subCapitulosN2) {
                            const actividadesN2 = obtenerActividades(-1, subCapitulosN2.id, -1);
                            const subCapitulosN3 = obtenerSubcapitulosNivel3(subCapitulosN2.id);

                            body.append(crearCapitulosHeader(subCapitulosN2, 2, 8));

                            //ACITIVDADES II
                            if (actividadesN2 != undefined) {
                                actividadesN2.forEach(function (actividad) {
                                    body.append(crearActividadesRow(actividad));
                                });
                            }

                            //SUBCAPITLO III
                            if (subCapitulosN3 != undefined) {
                                subCapitulosN3.forEach(function (subCapitulosN3) {
                                    const actividadesN3 = obtenerActividades(-1, -1, subCapitulosN3.id);

                                    body.append(crearCapitulosHeader(subCapitulosN3, 3, 8));

                                    //ACITIVDADES III
                                    if (actividadesN3 != undefined) {
                                        actividadesN3.forEach(function (actividad) {
                                            body.append(crearActividadesRow(actividad));
                                        });
                                    }
                                });
                            }
                        });
                    }
                });
            }

            $(table).append(body);
        }

        function makeTabla() {
            let table = $('#tblPresupuesto');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let th = document.createElement('th');

            let thActividad = document.createElement('th');
            thActividad.textContent = 'Actividad';

            let thUnidad = document.createElement('th');
            thUnidad.textContent = 'Unidad';

            let thCantidad = document.createElement('th');
            thCantidad.classList.add('text-center');
            thCantidad.textContent = 'Cantidad';

            let thPrecioUnitario = document.createElement('th');
            thPrecioUnitario.classList.add('text-center');
            thPrecioUnitario.textContent = 'P.U';

            let thImporteContratado = document.createElement('th');
            thImporteContratado.classList.add('text-center');
            thImporteContratado.textContent = 'Importe';

            let thVolumenFacturado = document.createElement('th');
            thVolumenFacturado.classList.add('text-center');
            thVolumenFacturado.textContent = 'Volumen';

            let thImporteFacturado = document.createElement('th');
            thImporteFacturado.classList.add('text-center');
            thImporteFacturado.textContent = 'Facturado';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thPrecioUnitario);
            $(tr).append(thImporteContratado);
            $(tr).append(thVolumenFacturado);
            $(tr).append(thImporteFacturado);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function crearCapitulosHeader(subcapitulo, subcapituloTipo, colSpan) {
            let tr = document.createElement('tr');
            tr.setAttribute('id', subcapitulo.id);
            tr.setAttribute('style', 'background-color:' + colorROW(subcapituloTipo));

            let tdSubcapitulo = document.createElement('td');
            tdSubcapitulo.setAttribute('style', 'border-right: none');
            tdSubcapitulo.setAttribute('style', 'border-left: none');
            tdSubcapitulo.setAttribute('colspan', colSpan);

            if (colSpan < 6) {
                tdSubcapitulo.setAttribute('style', 'height: 21px');
            } else {
                tdSubcapitulo.textContent = subcapitulo.subcapitulo;
            }

            $(tr).append(tdSubcapitulo);

            return tr;
        }

        function crearActividadesRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.id);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '2%');

            let tdActividad = document.createElement('td');
            let divActividad = document.createElement('div');
            tdActividad.setAttribute('width', '5%');
            tdActividad.setAttribute('style', 'border-left: none');
            divActividad.append(actividad.actividad);
            divActividad.classList.add('iffyTip');
            divActividad.classList.add('wd100');
            $(tdActividad).append(divActividad);

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '5.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.setAttribute('width', '5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdPrecioUnitario = document.createElement('td');
            tdPrecioUnitario.classList.add('PU');
            tdPrecioUnitario.setAttribute('width', '7%');
            tdPrecioUnitario.setAttribute('align', 'right');
            tdPrecioUnitario.textContent = maskNumero(actividad.precioUnitario);

            let tdImporteContratado = document.createElement('td');
            tdImporteContratado.setAttribute('width', '7%');
            tdImporteContratado.setAttribute('align', 'right');
            tdImporteContratado.textContent = maskNumero(actividad.importeContratado);

            let tdVolumenFacturado = document.createElement('td');
            let inputVolumen = document.createElement('input');
            inputVolumen.classList.add('inputVolumen');
            inputVolumen.setAttribute('style', 'background-color: #ccffcc; text-align: right;');
            inputVolumen.setAttribute('value', formatValue(0));
            tdVolumenFacturado.setAttribute('width', '7%');
            $(tdVolumenFacturado).append(inputVolumen);

            let tdImporteFacturado = document.createElement('td');
            tdImporteFacturado.setAttribute('width', '7%');
            tdImporteFacturado.setAttribute('style', 'background-color: #ccffcc;  text-align: right;');
            tdImporteFacturado.classList.add('inputImporte');
            tdImporteFacturado.textContent = maskNumero(0);
            //let inputImporte = document.createElement('input');
            // inputImporte.classList.add('inputImporte');
            // inputImporte.setAttribute('style', 'background-color: #ccffcc;  text-align: right;');
            // inputImporte.setAttribute('value', formatValue(0));
            //$(tdImporteFacturado).append(inputImporte);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdPrecioUnitario);
            $(tr).append(tdImporteContratado);
            $(tr).append(tdVolumenFacturado);
            $(tr).append(tdImporteFacturado);

            return tr;
        }

        function obtenerSubcapitulosNivel2(subcapituloN1_id) {
            let nivel2 = [];
            $.ajax({
                type: "GET",
                async: false, //Esta linea tendrias que poner
                data: {
                    subcapituloN1_id: subcapituloN1_id
                },
                url: "GetSubcapitulosN2Catalogo",
                success: function (data) {
                    nivel2 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel2;
        }

        function obtenerSubcapitulosNivel3(subcapituloN2_id) {
            let nivel3 = [];

            $.ajax({
                type: "GET",
                async: false, //Esta linea tendrias que poner
                data: {
                    subcapituloN2_id: subcapituloN2_id
                },
                url: "GetSubcapitulosN3Catalogo",
                success: function (data) {
                    nivel3 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel3;
        }

        function obtenerActividades(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id) {
            let actividades = [];
            $.ajax({
                type: "GET",
                async: false,
                data: {
                    subcapitulosN1_id: subcapitulosN1_id,
                    subcapitulosN2_id: subcapitulosN2_id,
                    subcapitulosN3_id: subcapitulosN3_id
                },
                url: "GetActividadesCatalogo",
                success: function (data) {
                    actividades = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return actividades;
        }

        function colorROW(subcapituloTipo) {
            let color = '';
            switch (subcapituloTipo) {
                case 1:
                    color = '#f8cbad'
                    break;
                case 2:
                    color = '#ffff99'
                    break;
                case 3:
                    color = '#9bc2e6'
                    break;
                default:
                    color = 'white'
                    break;
            }

            return color;
        }

        function paintCells(actividad_id) {
            if (actividad_id !== undefined && actividad_id.length > 0) {
                $(document).find(`tbody tr[data-actividadid=${actividad_id}]`).addClass("pintarGrupo");
            }
        }

        function unpaintCells(actividad_id, tr) {
            if (actividad_id !== undefined && actividad_id.length > 0) {
                $(document).find(`tbody tr[data-actividadid=${actividad_id}]`).removeClass("pintarGrupo");
            }
        }

        function limpiarTabla() {
            $('#tblPresupuesto thead tr').remove();
            $('#tblPresupuesto thead').remove();
            $('#tblPresupuesto tbody tr').remove();
            $('#tblPresupuesto tbody').remove();
        }

        function recargar() {
            cargarPresupuesto()
        }


        init();
    };

    $(document).ready(function () {
        controlObra.CapturaFacturado = new CapturaFacturado();
    });
})();