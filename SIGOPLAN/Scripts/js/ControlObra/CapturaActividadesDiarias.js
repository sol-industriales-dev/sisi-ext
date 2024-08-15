(function () {
    $.namespace('controlObra.CapturaActividadesDiarias');

    CapturaActividadesDiarias = function () {
        btnGuardar = $("#btnGuardar");

        function init() {
            initCbo();
            btnGuardar.click(crearObjectoActividad);
            $("#SelectCapitulosFiltro").change(CargarTabla);
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function CargarTabla() {
            const capitulo_id = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            obtenerSubcapitulosNivel1(capitulo_id);
        }

        function obtenerSubcapitulosNivel1(capitulo_id) {

            $.blockUI({ message: "Preparando información" });

            $.ajax({
                type: "Post",
                //async: false,
                data: {
                    listCapitulosID: capitulo_id
                },
                url: "GetSubcapitulosN1Catalogo",
                success: function (data) {
                    if (data.success) fnFillTables(data.items, new Date(), new Date());
                    else AlertaGeneral('Aviso', 'Ha ocurrido un error al cargar la información');
                    $.unblockUI();
                },
                error: function (objXMLHttpRequest) {
                    limpiarTabla();
                    $.unblockUI();
                }
            });
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
                url: "getActividadesCatalogo",
                success: function (data) {
                    actividades = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return actividades;
        }

        function fnFillTables(subCapitulosN1) {
            limpiarTabla();
            const subcapituloNI = subCapitulosN1;

            //TABLAS
            let table = makeTabla();
            let body = document.createElement('tbody');

            if (subcapituloNI != undefined) {
                subcapituloNI.forEach(function (subcapituloNI) {

                    const actividadesN1 = obtenerActividades(subcapituloNI.id, -1, -1);
                    const subCapitulosN2 = obtenerSubcapitulosNivel2(subcapituloNI.id);

                    //SUBCAPITULO I
                    body.append(crearCapitulosHeader(subcapituloNI, 1, 7));

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

                            body.append(crearCapitulosHeader(subCapitulosN2, 2, 7));

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

                                    body.append(crearCapitulosHeader(subCapitulosN3, 3, 7));

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
            thImporteContratado.textContent = 'Importe Contratado';

            let thPeriodo = document.createElement('th');
            thPeriodo.classList.add('text-right');
            thPeriodo.textContent = 'Volumen Diario';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thPrecioUnitario);
            $(tr).append(thImporteContratado);
            $(tr).append(thPeriodo);

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
            tdActividad.setAttribute('width', '10%');
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
            tdPrecioUnitario.setAttribute('width', '7%');
            tdPrecioUnitario.setAttribute('align', 'right');
            tdPrecioUnitario.textContent = '$ ' + formatValue(actividad.precioUnitario);

            let tdImporteContratado = document.createElement('td');
            tdImporteContratado.setAttribute('width', '7%');
            tdImporteContratado.setAttribute('align', 'right');
            tdImporteContratado.textContent = '$ ' + formatValue(actividad.importeContratado);

            let tdPeriodo = document.createElement('td');
            tdPeriodo.classList.add('text-center')
            tdPeriodo.setAttribute('width', '7%');

            let labelCheck = document.createElement('label');
            let inputCheck = document.createElement('input');
            let divInput = document.createElement('div');

            labelCheck.classList.add('switch-wrap')
            inputCheck.type = 'checkbox';
            inputCheck.id = 'checkActividad';
            inputCheck.classList.add('valorCheck');
            inputCheck.checked = actividad.tipoPeriodoAvance == 1 ? true : false;
            divInput.classList.add('switch')

            $(labelCheck).append(inputCheck);
            $(labelCheck).append(divInput);
            $(tdPeriodo).append(labelCheck);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdPrecioUnitario);
            $(tr).append(tdImporteContratado);
            $(tr).append(tdPeriodo);

            return tr;
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

        function crearObjectoActividad() {
            if ($('#SelectCapitulosFiltro').val() > 0) {
                let actividad = [];

                $('#tblPresupuesto').find('tbody tr[data-actividadid]').each(function (index) {
                    debugger;
                    const row = $(this).closest('tr');
                    const actividad_id = parseInt(row.attr('data-actividadid'));
                    const tipoPeriodo = row.find('.valorCheck').is(':checked') ? 1 : 2

                    if (actividad_id > 0) {
                        periodo = new Object();
                        periodo.actividad_id = actividad_id;
                        periodo.tipoPeriodo = tipoPeriodo;
                        actividad.push(periodo);
                    }
                });

                let actividadObj = new FormData();
                actividadObj.append("actividad", JSON.stringify(actividad));

                guardarActividad(actividadObj);
            }
            else {
                AlertaGeneral('Aviso', 'Debe seleccionar Obra');
            }
        }

        function guardarActividad(actividadObj) {
            $.blockUI({ message: "Preparando información" });
            $.ajax({
                url: '/ControlObra/ControlObra/UpdateActividadPeriodoValor',
                data: actividadObj,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        AlertaGeneral("Aviso", "Guardado correctamente.");
                        $.unblockUI();
                        recargar();
                    } else {
                        AlertaGeneral("Aviso", "Ocurrió un error al guardar el.");
                        $.unblockUI();
                    }
                }
            });
        }

        function recargar() {
            CargarTabla();
        }


        function limpiarTabla() {
            $('#tblPresupuesto thead tr').remove();
            $('#tblPresupuesto thead').remove();
            $('#tblPresupuesto tbody tr').remove();
            $('#tblPresupuesto tbody').remove();
        }


        init();
    }

    $(document).ready(function () {
        controlObra.CapturaActividadesDiarias = new CapturaActividadesDiarias();
    });
})();
