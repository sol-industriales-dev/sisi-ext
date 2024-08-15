(function () {
    $.namespace('ActualizacionDeUbicaciones.Almacen');

    const inputAlmacenNum = $('#inputAlmacenNum');
    const inputAlmacenDescripcion = $('#inputAlmacenDescripcion');
    const btnGuardar = $("#btnGuardar");

    const tblActualizacion = $('#tblActualizacion');
    const tblAlmacen = $('#tblAlmacen');
    const tblInsumo = $('#tblInsumo');

    let dtInsumo;
    let dtAlmacen;
    let dtActualizacion;

    const inputInsumo = $('#inputInsumo');
    const inputInsumoDescripcion = $('#inputInsumoDescripcion');
    const btnBuscarInsumo = $('#btnBuscarInsumo');

    let pagina = 1;

    Almacen = function () {
        let init = function () {
            initTablaAlmacen();
            getObtenerAlmacenes();
            botones();
            inittblActualizacion();
            initInsumo();
        }
        init();
    }

    function botones() {
        inputAlmacenNum.change(function () {
            obtenerAlmacenxID();
            CargarExistenciasAlmacen();
        })
        inputInsumo.change(function () {
            consultaInicioInsumo(inputInsumo.val());
            CargarExistenciasAlmacen();
        })
        btnBuscarInsumo.click(function () {
            obtenerInsumos(1);
        })
        btnGuardar.click(function () {
            ActualizarUbicacion();
        })
        renderbutton();
    }


    function inittblActualizacion() {
        dtActualizacion = tblActualizacion.DataTable({
            retrieve: true,
            paging: false,
            language: dtDicEsp,
            dom: 't',
            columns: [
                { data: 'almacen', title: 'Almacén' },
                { data: 'insumo', title: 'Insumo' },
                {
                    data: 'areaAlmacen',
                    title: 'Área Almacén',
                    //render: function(data, type, row, meta) { 
                    //    return parseFloat(data, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "1,");
                    //} 
                },
                { data: 'ladoAlmacen', title: 'Lado Almacén' },
                { data: 'estanteAlmacen', title: 'Estante Almacén' },
                { data: 'nivelAlmacen', title: 'Nivel Almacén' },
                {
                    data: 'cantidad',
                    title: 'Cantidad Ubicación',
                    render: function (data, type, row, meta) {
                        return parseFloat(data).toFixed(6).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1");
                    }
                },
                { data: 'unidad', title: 'Unidad' },
                {
                    title: 'Valida',
                    render: function (data, type, row, meta) {
                        return html = row.cantidad > 0 ?
                            '<input class="valida" type="checkbox" data-toggle="toggle" data-onstyle="success" data-offstyle="danger" data-size="small" data-on="&#x2714;" data-off="<b>X</b>">'
                            : '';
                    }
                },
                {
                    title: 'Cantidad en Destino',
                    render: function (data, type, row, meta) {
                        return html = '<input type="number" disabled />'
                    }
                },
                {
                    title: 'Área',
                    render: function (data, type, row, meta) {
                        return html = '<input type="text" disabled />'
                    }
                },
                {
                    title: 'Lado',
                    render: function (data, type, row, meta) {
                        return html = '<input type="text" disabled />'
                    }
                },
                {
                    title: 'Estante',
                    render: function (data, type, row, meta) {
                        return html = '<input type="text" disabled />'
                    }
                },
                {
                    title: 'Nivel',
                    render: function (data, type, row, meta) {
                        return html = '<input type="text" disabled />'
                    }
                }
            ],
            initComplete: function (settings, json) {
            }
        });
    }

    function initInsumo() {
        dtInsumo = tblInsumo.DataTable({
            destroy: true,
            language: dtDicEsp,
            paging: true,
            ordering: false,
            searching: true,
            bFilter: false,
            info: false,
            columns: [
                { data: 'insumoNumero', title: 'Insumo' },
                { data: 'insumoDescripcion', title: 'Descripción' },
                {
                    title: 'Seleccionar',
                    render: function (data, type, row, meta) {
                        return html = '<button class="btn btn-sm btn-primary seleccionar"><i class="fas fa-arrow-right"></i></button>';
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblInsumo.on('click', '.seleccionar', function () {
                    const rowData = dtInsumo.row($(this).closest("tr")).data();
                    $('#inputInsumo').val(rowData.insumoNumero);
                    $('#inputInsumoDescripcion').val(rowData.insumoDescripcion);
                    $('#inputInsumo').change();
                    $('#mdlObtenerInsumo').modal('hide');
                });
            }
        });
        $('#tblInsumo_length').css('display', 'none');
    }

    function initTablaAlmacen() {
        dtAlmacen = tblAlmacen.DataTable({
            destroy: true,
            language: dtDicEsp,
            paging: true,
            ordering: false,
            searching: true,
            bFilter: false,
            info: false,
            columns: [
                { data: 'almacen', title: 'Almacén' },
                { data: 'descripcion', title: 'Descripción' },
                {
                    title: 'Seleccionar',
                    render: function (data, type, row, meta) {
                        return html = '<button class="btn btn-xs btn-primary seleccionar"><i class="fas fa-arrow-right"></i></button>';
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblAlmacen.on('click', '.seleccionar', function () {
                    const rowData = dtAlmacen.row($(this).closest("tr")).data();
                    $('#inputAlmacenNum').val(rowData.almacen);
                    $('#inputAlmacenDescripcion').val(rowData.descripcion);
                    $('#mdlObtenerAlmacen').modal('hide');
                });
            }
        });
    }

    function getObtenerAlmacenes() {
        axios.post('obtenerAlmacenes', {})
            .catch(function (o_O) { Alert2Error(o_O.message) })
            .then(function (response) {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblAlmacen, items);
                }
            }
            );
    }

    function obtenerAlmacenxID() {
        axios.post('ObtenerAlmacenID', { almacen: inputAlmacenNum.val() })
            .catch(function (o_O) { Alert2Error(o_O.message) })
            .then(function (response) {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        inputAlmacenDescripcion.val(items.descripcion)
                    }
                    else {
                        inputAlmacenDescripcion.val('')
                    }
                }
                else {
                    inputAlmacenDescripcion.val('')
                }
            }
            );
    }

    function consultaInicioInsumo(insumo) {
        axios.post('consultarPrimerInsumo', { insumo: insumo })
            .catch(function (o_O) { Alert2Error(o_O.message) })
            .then(function (response) {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        inputInsumo.val(items.insumoNumero);
                        inputInsumoDescripcion.val(items.insumoDescripcion);
                    }
                    else {
                        inputInsumoDescripcion.val('');
                    }
                }
                else {
                    inputInsumoDescripcion.val('');
                }
            }
            );
    }

    function renderbutton() {
        $('#cboRegistros').change(function () {
            pagina = 1;
            obtenerInsumos(1);
        })
    }
    function obtenerInsumos(Almacen) {
        let registros = 100000;
        axios.post('Obtenerinsumos', { Almacen: Almacen, pagina: pagina, registros: registros })
            .catch(function (o_O) { Alert2Error(o_O.message) })
            .then(function (response) {
                let data = response.data;
                AddRows(tblInsumo, data);
            }
            );
    }

    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.draw();
        dt.rows.add(lst).draw(false);
    }

    function CargarExistenciasAlmacen() {
        var almacen = inputAlmacenNum.val();
        var insumo = inputInsumo.val();
        if (almacen.length > 0 && insumo.length > 0) {
            axios.post('CargarExistenciasAlmacenPorInsumo', { almacen: almacen, insumo: insumo, existentes: false })
                .then(function (response) {
                    let { success, datos, message } = response.data;
                    if (success) {
                        AddRows(tblActualizacion, datos);
                        $(".valida").bootstrapToggle();
                        $(".valida").change(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            if ($(this).is(":checked")) $(this).parent().parent().parent().find('input[type="text"], input[type="number"]').prop("disabled", false);
                            else $(this).parent().parent().parent().find('input[type="text"], input[type="number"]').prop("disabled", true);
                            if ($('.valida:checked').length > 0) btnGuardar.prop("disabled", false);
                            else btnGuardar.prop("disabled", true);
                        });
                        inputAlmacenNum.attr("data-index", inputAlmacenNum.val());
                        inputInsumo.attr("data-index", inputInsumo.val());
                    }
                    else {
                        Alert2Error(message);
                    }
                }
                ).catch(function (error) { Alert2Error(error.message) });
        }
    }

    function ActualizarUbicacion() {
        var error = false;
        var cc = '998';
        var almacen = inputAlmacenNum.attr("data-index");
        var objectoPrincipal = GetObjetoPrincipal(almacen);
        if (objectoPrincipal == null) error = true;
        var lstDetallesSalida = [];
        var lstDetallesEntrada = [];

        var insumo = inputInsumo.attr("data-index");
        var lineasValidadas = $('.valida:checked');

        for (var i = 0; i < lineasValidadas.length; i++) {
            var padre = $(lineasValidadas[i]).parent().parent().parent();
            var detalleSalida = GetObjectoDetalleSalida(padre, almacen, insumo);
            var detalleEntrada = GetObjectoDetalleEntrada(padre, almacen, insumo);
            if (detalleSalida == null || detalleEntrada == null) error = true;
            lstDetallesSalida.push(detalleSalida);
            lstDetallesEntrada.push(detalleEntrada);
        }

        if (!error && almacen.length > 0 && insumo.length > 0) {
            axios
                .post(
                    'ActualizacionUbicacionInsumo',
                    {
                        almacen: almacen,
                        insumo: insumo,
                        cc: 998,
                        movimiento: objectoPrincipal,
                        detallesMovimientoSalida: lstDetallesSalida,
                        detallesMovimientoEntrada: lstDetallesEntrada
                    })
                .then(function (response) {
                    let { success, message } = response.data;
                    if (success) {
                        CargarExistenciasAlmacen();
                        Alert2Exito('Se realizo la actualización de ubicaciones correctamente');
                    }
                    else {
                        Alert2Error(message);
                    }
                }
                ).catch(function (error) { Alert2Error(error.message) });
        }
        else {
            Alert2Error('Error de Captura. Algunos datos son insuficientes o incorrectos.')
        }
    }

    function GetObjetoPrincipal(almacen) {
        var objeto = {
            almacen: almacen,
            tipo_mov: 0,
            //numero: 0,
            cc: '998',
            compania: 1,
            //periodo = DateTime.Now.Month,
            //ano = DateTime.Now.Year,
            //orden_ct = folioTraspaso,
            frente: 0,
            //fecha = DateTime.Now.Date,
            proveedor: 0,
            //total = total,
            estatus: "A",
            transferida: "N",
            alm_destino: almacen,
            cc_destino: '998',
            comentarios: '', //Se escoge el primer rengl�n ya que se juntaron para que nom�s generen un movimiento.
            tipo_trasp: "0",
            tipo_cambio: 1,
            //requisicion: null,
            estatusHabilitado: true
        };
        return objeto
    }

    function GetObjectoDetalleSalida(padre, almacen, insumo) {
        var errorDetalle = false;

        var area_alm = $(padre.find('td')[2]).text();
        var lado_alm = $(padre.find('td')[3]).text();
        var estante_alm = $(padre.find('td')[4]).text();
        var nivel_alm = $(padre.find('td')[5]).text();

        var cantidad = padre.find('input[type="number"]');
        var cantidadInicial = +$(padre.find('td')[6]).text();
        $(cantidad[0]).removeClass('invalid');
        if (cantidad[0] == null || +$(cantidad[0]).val() <= 0 || +$(cantidad[0]).val() > cantidadInicial) {
            $(cantidad[0]).addClass('invalid');
            errorDetalle = true;
        }

        if (!errorDetalle) {
            var objeto = {
                almacen: almacen,
                tipo_mov: 52,
                //numero: 0,
                partida: 1,
                insumo: insumo,
                //comentarios: null,
                area: 99,
                cuenta: 99,
                cantidad: +$(cantidad[0]).val(),
                //precio = costoPromedio, //precio = existencias > 0 ? costoPromedio : precioEntrada,
                //importe = ubi.cantidadMovimiento * costoPromedio, //importe = ubi.cantidadMovimiento * (existencias > 0 ? costoPromedio : precioEntrada),
                id_resguardo: 0,
                area_alm: area_alm.trim() == '' ? '' : area_alm.trim().toUpperCase(),
                lado_alm: lado_alm.trim() == '' ? '' : lado_alm.trim().toUpperCase(),
                estante_alm: estante_alm.trim() == '' ? '' : estante_alm.trim().toUpperCase(),
                nivel_alm: nivel_alm.trim() == '' ? '' : nivel_alm.trim().toUpperCase(),
                transporte: "",
                estatusHabilitado: true
            };
            return objeto;
        }
        else
            return null;
    }

    function GetObjectoDetalleEntrada(padre, almacen, insumo) {
        var errorDetalle = false;
        var cantidad = padre.find('input[type="number"]');
        var cantidadInicial = +$(padre.find('td')[6]).text();
        $(cantidad[0]).removeClass('invalid');
        if (cantidad[0] == null || +$(cantidad[0]).val() <= 0 || +$(cantidad[0]).val() > cantidadInicial) {
            $(cantidad[0]).addClass('invalid');
            errorDetalle = true;
        }
        var inputs = padre.find('input[type="text"]');
        for (var i = 0; i < inputs.length; i++) {
            $(inputs[i]).removeClass('invalid');
            if (inputs[i] == null || $(inputs[i]).val().trim() == '') {
                $(inputs[i]).addClass('invalid');
                errorDetalle = true;
            }
        }

        if (!errorDetalle) {
            var objeto = {
                almacen: almacen,
                tipo_mov: 52,
                //numero: 0,
                partida: 1,
                insumo: insumo,
                //comentarios: null,
                area: 99,
                cuenta: 99,
                cantidad: +$(cantidad[0]).val(),
                //precio = costoPromedio, //precio = existencias > 0 ? costoPromedio : precioEntrada,
                //importe = ubi.cantidadMovimiento * costoPromedio, //importe = ubi.cantidadMovimiento * (existencias > 0 ? costoPromedio : precioEntrada),
                id_resguardo: 0,
                area_alm: $(inputs[0]).val().trim().toUpperCase(),
                lado_alm: $(inputs[1]).val().trim().toUpperCase(),
                estante_alm: $(inputs[2]).val().trim().toUpperCase(),
                nivel_alm: $(inputs[3]).val().trim().toUpperCase(),
                transporte: "",
                estatusHabilitado: true
            };
            return objeto;
        }
        else
            return null;
    }


    $(document).ready(function () {
        ActualizacionDeUbicaciones.Almacen = new Almacen();
    })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();