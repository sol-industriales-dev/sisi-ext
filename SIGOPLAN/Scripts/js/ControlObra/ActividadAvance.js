(function () {
    $.namespace('controlObra.ActividadAvance');

    ActividadAvance = function () {
        btnGuardarAvance = $("#btnGuardarAvance");
        dpFechaInicio = $('#dpFechaInicio');
        const hoy = new Date();

        function init() {
            initCbo();

            $("#SelectCapitulosFiltro").change(cargarPresupuesto);
            btnGuardarAvance.click(crearObjectoAvance);
            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' });
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function cargarPresupuesto() {
            const capitulo_id = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            $.blockUI({ message: "Preparando información" });

            $.ajax({
                type: "GET",
                //async: false,
                data: {
                    listCapitulosID: capitulo_id
                },
                url: "GetSubcapitulosN1Catalogo",
                success: function (data) {
                    if (data.success) {
                        fnFillTable(data.items);
                        getUltimoFechaUltimoAvance(capitulo_id);
                    }
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
                    body.append(crearCapitulosHeader(subcapituloNI, 1, 5));

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

                            body.append(crearCapitulosHeader(subCapitulosN2, 2, 5));

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

                                    body.append(crearCapitulosHeader(subCapitulosN3, 3, 5));

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

            let thAvancePeriodo = document.createElement('th');
            thAvancePeriodo.classList.add('text-center');
            thAvancePeriodo.textContent = 'Avance Periodo';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thAvancePeriodo);

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

            if (colSpan < 5) {
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

            let tdVolumenAvanzado = document.createElement('td');
            let inputVolumen = document.createElement('input');
            inputVolumen.classList.add('inputVolumen');
            inputVolumen.setAttribute('style', 'background-color: #ccffcc; text-align: right;');
            inputVolumen.setAttribute('value', formatValue(0));
            tdVolumenAvanzado.setAttribute('width', '7%');
            $(tdVolumenAvanzado).append(inputVolumen);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdVolumenAvanzado);

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

        function getUltimoFechaUltimoAvance(capituloID) {
            $.ajax({
                url: '/ControlObra/ControlObra/GetFechasUltimoAvance',
                datatype: "json",
                type: "GET",
                data: {
                    capituloID: capituloID
                },
                success: function (response) {
                    const actividadAvance = response.actividadAvance;
                    const capitulo = response.capitulo;

                    if (actividadAvance == null) {
                        const fechaFin = moment(capitulo.fechaInicio, "DD-MM-YYYY").add(7, 'days').format();

                        dpFechaInicio.datepicker("setDate", capitulo.fechaInicio);
                        // dpFechaFin.datepicker("setDate", new Date(fechaFin));
                    } else {
                        const fechaInicio = moment(actividadAvance.fechaFin, "DD-MM-YYYY").add(1, 'days').format();
                        const fechaFin = moment(actividadAvance.fechaFin, "DD-MM-YYYY").add(8, 'days').format();

                        dpFechaInicio.datepicker("setDate", new Date(fechaInicio));
                        // dpFechaFin.datepicker("setDate", new Date(fechaFin));
                    }
                }
            });
        }

        function crearObjectoAvance() {
            let avance = new Object();
            avance = {
                capitulo_id: $('#SelectCapitulosFiltro').val(),
                fechaI: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaF: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                autorizado: false,
                estatus: true
            };
            let avanceDetalle = [];
            let detalle;
            let total = 0;

            $('#tblPresupuesto').find('tbody tr[data-actividadid]').each(function (index) {
                debugger;
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const volumen = unmaskNumero(row.find('.inputVolumen').val());
                total += volumen;

                if (actividad_id > 0) {
                    detalle = new Object();
                    detalle.actividad_id = actividad_id;
                    detalle.cantidadAvance = volumen;
                    detalle.fechaI = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
                    detalle.fechaF = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
                    avanceDetalle.push(detalle);
                }
            });

            if (total > 0) {
                let avanceObj = new FormData();
                avanceObj.append("avance", JSON.stringify(avance));
                avanceObj.append("avanceDetalle", JSON.stringify(avanceDetalle));

                guardarAvance(avanceObj);
            } else {
                AlertaGeneral('Aviso', 'Debe ingresar una cantidad en volumen avanzado');
            }


        }

        function guardarAvance(avanceObj) {
            $.blockUI({ message: "Preparando información" });
            $.ajax({
                url: '/ControlObra/ControlObra/GuardarAvanceActividad',
                data: avanceObj,
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
                        AlertaGeneral("Aviso", "Ocurrió un error al guardar el avance.");
                        $.unblockUI();
                    }
                }
            });
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
        controlObra.ActividadAvance = new ActividadAvance();
    });
})();