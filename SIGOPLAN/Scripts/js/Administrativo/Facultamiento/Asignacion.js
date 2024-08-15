(function () {
    $.namespace('administrativo.facultamiento.Asignacion');
    Asignacion = function () {

        // Variables vista principal.
        let dataTableCatalogo;
        const tablaPaquetes = $("#tablaPaquetes")
        const comboDepartamento = $('#comboDepartamento');
        const comboEstado = $('#comboEstado');
        const comboObra = $('#comboObra');
        const btnNuevaAsignacion = $('#nuevaAsignacion');
        const btnBuscar = $('#btnBuscar');
        const btnLimpiar = $('#btnLimpiar');
        const reporte = $("#report");

        // Variables modal.
        const modal = $('#modalAsignacion');
        const comboCC = $("#comboCC");
        const primerAutorizante = $('#tablaAutorizantes input:first');
        const btnGuardar = $('#btnGuardar');

        // Variable constante - Encabezados facultamiento
        const trEncabezados = $(
            `<tr>
                <th class="text-center">Concepto</th>
                <th class="text-center">Personal</th>
                <th class="text-center">Aplica</th>
            </tr>`);

        var isAdmin = false;

        class Empleado {
            constructor(conceptoID, nombreEmpleado, claveEmpleado, empleadoID, aplica) {
                this.ConceptoID = conceptoID;
                this.NombreEmpleado = nombreEmpleado;
                this.ClaveEmpleado = claveEmpleado;
                this.EmpleadoID = empleadoID;
                this.Aplica = aplica;
            }
        }

        class Facultamiento {
            constructor(plantillaID, listaEmpleados, aplica, facultamientoID) {
                this.PlantillaID = plantillaID;
                this.ListaEmpleados = listaEmpleados;;
                this.Aplica = aplica;
                this.FacultamientoID = facultamientoID;
            }
        }

        function init() {
            llenarCombos();
            agregarListeners();
            obtenerPaquetes(0, 0, 0);
        }

        function llenarCombos() {
            comboCC.fillCombo('/Administrativo/Facultamientos/LlenarComboCC', null, false, null);
            comboDepartamento.fillCombo('/Administrativo/Facultamientos/LlenarComboDepartamentos', null, false, null);
            comboObra.fillCombo('/Administrativo/Facultamientos/LlenarComboObras', null, false, null);
            comboEstado.fillCombo('/Administrativo/Facultamientos/LlenarComboEstadoPaquetes', null, false, null);
        }

        function agregarListeners() {

            btnBuscar.click(() => {
                const departamentoID = (comboDepartamento.val() != "") ? comboDepartamento.val() : 0;
                const obraID = (comboObra.val() != "") ? comboObra.val() : 0;
                const estadoPaqueteID = (comboEstado.val() != "") ? comboEstado.val() : 0;
                obtenerPaquetes(departamentoID, obraID, estadoPaqueteID);
            });

            btnLimpiar.click(() => {
                comboDepartamento[0].selectedIndex = 0;
                comboEstado[0].selectedIndex = 0;
                comboObra[0].selectedIndex = 0;
                obtenerPaquetes(0, 0, 0);
            });

            comboCC.change(function () {
                if ($(this).val() !== "") {
                    $('#listadoFacultamientos table').remove();
                    $('#tablaAutorizantes input').toArray().map(x => {
                        $(x).data().claveEmpleado = null;
                        $(x).val('')
                    })
                    obtenerFacultamientos(comboCC.val());
                    $(btnGuardar).attr('disabled', false);
                } else {
                    limpiarCamposModal();
                }
            });

            btnNuevaAsignacion.click(() => {
                modal.modal({
                    backdrop: 'static',
                    keyboard: false
                });
            });

            $('#tablaAutorizantes input')
                .toArray()
                .map(x =>
                    $(x).getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, null, '/Administrativo/Facultamiento/getEmpleadosSigoplan'));

            modal.on("hide.bs.modal", limpiarCamposModal);

            btnGuardar.click(() => {
                if (camposValidos()) {
                    if (modal.data().paqueteID != null) {
                        actualizarPaqueteFacultamientos();
                    } else {
                        guardarPaqueteFacultamientos();
                    }
                }
            });

            modal.on('change', '.aplica', function () {
                if (this.checked) {
                    $(this).closest("tr").find('input[type="text"]').prop('disabled', false);
                    $(this).closest("tr").find('input[type="text"]').val('');
                    $(this).closest("tr").find('td:first').find('.label-danger').remove();
                } else {
                    $(this).closest("tr").find('input[type="text"]').prop('disabled', true);
                    $(this).closest("tr").find('input[type="text"]').val('');
                    $(this).closest("tr").find('input[type="text"]').data().claveEmpleado = null;
                    $(this).closest("tr").find('td:first')
                        .append(`<span class="label label-danger spanTipo pull-right">No Aplica</span>`);
                }
            });



            modal.on('change', '.aplicaFacultamiento', function () {
                const tabla = $(this).closest("table");
                if (this.checked) {
                    if (modal.data().paqueteID != null) {
                        agregarCuerpoFacultamiento(tabla, true);
                    } else {
                        agregarCuerpoFacultamiento(tabla);
                    }
                } else {
                    limpiarCuerpoFacultamiento(tabla);
                }
            });

            modal.on('click', '.aplicaTabla', (event => {
                const checkbox = $(event.currentTarget).find('.aplicaFacultamiento');
                checkbox.prop("checked", !checkbox.prop("checked"));
                checkbox.trigger("change");
            }));
        }

        function agregarCuerpoFacultamiento(tabla, esActualizacion) {
            const facultamiento = tabla.data().facultamiento;
            tabla.find('thead').append(trEncabezados);
            const tbody = $(`
            <tbody></tbody>
            `);

            let contador = 0;
            for (const puesto of (esActualizacion) ? facultamiento.ListaEmpleados : facultamiento.ListaConceptos) {
                const tr =
                    $(`<tr>
                        <td class="concepto">${puesto.Concepto}  
                            <span class="label label-primary spanTipo pull-right">${(puesto.EsAutorizante)?'Autoriza':'VoBo'}</span>
                        </td>
                        <td>
                            <input type="text" class="form-control" placeholder=${(puesto.EsAutorizante)?'Autoriza':'VoBo'}>
                        </td>
                    </tr>`);

                if (contador === 0) {
                    tr.append(`<td><input checked class="aplica" type="checkbox"></td>`);
                    contador++;
                } else {
                    const noAplicaLabel = $(`<span class="label label-danger spanTipo pull-right">No Aplica</span>`);
                    tr.find('td:first').append(noAplicaLabel);
                    tr.find('input[type="text"]').prop('disabled', true);
                    tr.append(`<td><input class="aplica" type="checkbox"></td>`);
                    contador++;
                }
                $(tr).find('input[type="text"]').getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, null, '/OT/getEmpleados');
                $(tr).find('input[type="text"]').data().conceptoID = puesto.ID;
                if (esActualizacion) {
                    $(tr).find('input[type="text"]').data().empleadoID = puesto.EmpleadoID;
                    $(tr).find('input[type="text"]').data().conceptoID = puesto.ConceptoID;
                }
                $(tbody).append(tr);
            }
            tabla.append(tbody);
        }

        function limpiarCuerpoFacultamiento(tabla) {
            tabla.find('thead').find('tr:nth-child(2)').remove();
            tabla.find('tbody').remove();
        }

        function obtenerPaquetes(departamentoID, obraID, estado) {
            $.blockUI({
                message: 'Cargando paquetes...'
            });
            $.get('/Administrativo/Facultamientos/ObtenerPaquetes', {
                    departamentoID: departamentoID,
                    obraID: obraID,
                    estado: estado
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        cargarPaquetes(respuesta.listaPaquetesFa);
                    } else {
                        if (respuesta.EMPTY == null) {
                            AlertaGeneral("Aviso", "Aviso: " + respuesta.error);
                        }
                        if (dataTableCatalogo != null) {
                            dataTableCatalogo.clear().draw();
                        }
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                })
                .always(() => $.unblockUI());
        }

        function cargarPaquetes(data) {
            if (dataTableCatalogo != null) {
                dataTableCatalogo.destroy();
            }
            dataTableCatalogo = tablaPaquetes.DataTable({
                language: dtDicEsp,
                destroy: true,
                data: data,
                order: [[0,"asc"]],
                columns: [{
                        data: 'CentroCostos'
                    },
                    {
                        data: 'Descripcion'
                    },
                    {
                        data: 'Fecha'
                    },
                    {
                        data: 'Estatus'
                    },
                    {
                        data: 'Departamento'
                    },
                    {
                        data: 'ID',
                        createdCell: function (td, cellData, rowData, row, col) {
                            const divFlex = $('<div class="flexContainer"></div>');
                            const botonEditar = $('<button class="btn btn-warning btnEditar"><i class="fa fa-edit"></i> Actualizar</button>');
                            const botonReporte = $('<button class="btn btn-primary btnReporte"><i class="fa fa-book"></i> Reporte</button>');
                            $(td).html(divFlex);
                            $(divFlex).append(botonEditar);
                            $(divFlex).append(botonReporte);
                            if (!rowData.Editable) {
                                botonEditar.prop('disabled', true);
                            }
                        }
                    }
                ],
                drawCallback: function () {

                    $('#tablaPaquetes .btnEditar').unbind().click(function () {
                        const paquete = dataTableCatalogo.row($(this).parents('tr')).data();
                        const tituloCC = paquete.CentroCostos + ' - ' + paquete.Descripcion;
                        obtenerPaqueteActualizar(paquete.ID, tituloCC);
                    });
                    $('#tablaPaquetes .btnReporte').unbind().click(function () {
                        const paquete = dataTableCatalogo.row($(this).parents('tr')).data();
                        verReporte(paquete.ID, true);
                    });
                }
            });
        }

        function verReporte(paqueteID, isReporte, ordenVoBo) {
            // CRModal = Crystal Reports Modal.
            $.blockUI({
                message: 'Generando reporte...'
            });
            reporte.attr("src", `/Reportes/Vista.aspx?idReporte=99&id=${paqueteID}&inMemory=${1}&isCRModal=${true}`);
            document.getElementById('report').onload = function () {
                if (isReporte) {
                    openCRModal();
                    $.unblockUI();
                } else {
                    enviarCorreoAut(paqueteID, ordenVoBo);
                }
            };
        }

        function enviarCorreoAut(paqueteID, ordenVoBo) {
            $.post('/Administrativo/Facultamientos/EnviarCorreoAutorizacion', {
                    paqueteID: paqueteID,
                    ordenVoBo: ordenVoBo
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Éxito", "Operación Exitosa.");
                    } else {
                        AlertaGeneral("Error", `Los facultamientos quedaron asignados pero ocurrió 
                        un error al momento de tratar de enviar el correo al primer autorizante: ${respuesta.error}`);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                })
                .always(() => $.unblockUI());
        }

        function obtenerPaqueteActualizar(paqueteID, tituloCC) {
            $.blockUI({
                message: 'Cargando el paquete seleccionado...'
            });
            $.get('/Administrativo/Facultamientos/ObtenerPaqueteActualizar', {
                    paqueteID: paqueteID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        $('#modalAsignacion').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                        agregarTablasFacultamientos(respuesta.paqueteFacultamientos.listaFacultamientos);
                        agregarAutorizantes(respuesta.paqueteFacultamientos.ListaAutorizantes);
                        $('[data-toggle="tooltip"]').tooltip();
                        $('#modalAsignacion .input-group:first').addClass('hiddenCombo');
                        $('#modalAsignacion #tituloCC').text('Obra: ' + tituloCC);
                        $(btnGuardar).attr('disabled', false);
                        modal.data().paqueteID = paqueteID;

                        if (respuesta.paqueteFacultamientos.Estado == "Editando") {
                            AlertaGeneral("Aviso",
                                "El paquete de facultamientos ha sido rechazado / modificado. Razón: " + respuesta.paqueteFacultamientos.Comentario);
                        }
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                })
                .always(() => $.unblockUI());
        }

        function agregarAutorizantes(listaAutorizantes) {
            const inputsAutorizantes = $('#tablaAutorizantes input').toArray();
            $('#tablaAutorizantes span').remove();
            const spanTDs = $('#tablaAutorizantes td:nth-child(3)').toArray();
            for (let index = 0; index < listaAutorizantes.length; index++) {
                const input = $(inputsAutorizantes[index]);
                const autorizante = listaAutorizantes[index];
                const td = $(spanTDs[index]);

                input.val(autorizante.Nombre);
                input.data().claveEmpleado = autorizante.UsuarioID;
                input.data().autorizanteID = autorizante.AutorizanteID;

                switch (autorizante.Autorizado) {
                    case null:
                        td.append('<span class="label label-default">Pendiente</span>');
                        break;
                    case false:
                        td.append('<span class="label label-danger">Rechazado</span>');
                        break;
                    case true:
                        td.append('<span class="label label-success">Autorizado</span>');
                        break;
                    default:
                        td.append('<span class="label label-default">Pendiente</span>');
                        break;
                }
            }
        }

        function obtenerFacultamientos(centroCostosID) {
            $.blockUI({
                message: 'Cargando facultamientos...'
            });
            $.get('/Administrativo/Facultamientos/CargarPlantillasCC', {
                    centroCostosID: centroCostosID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        agregarFacultamientosPlantilla(respuesta.listaPlantillas);
                        $('[data-toggle="tooltip"]').tooltip();
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                })
                .always(() => $.unblockUI());
        }

        function agregarTablasFacultamientos(facultamientos) {
            for (const facultamiento of facultamientos) {
                const tabla =
                    $(`<table class="table table-fixed table-responsive table-hover table-striped"></table>`);
                const thead = $(`
                    <thead class="bg-table-header">
                    <tr>
                    <th title='Indicar que el facultamiento "${facultamiento.Titulo}" aplica para este paquete'
                    class="text-center titulo aplicaTabla" colspan="3"><span class="pull-left">Numero: ${facultamiento.orden}</span> ${facultamiento.Titulo}
                            <input disabled ${facultamiento.Aplica?"checked":""} class="aplicaFacultamiento pull-right" type="checkbox">
                            <p class="pull-right aplicaFac">Aplica:</p>
                        </th>
                    </tr>
                </thead>`);

                tabla.append(thead).data().facultamiento = facultamiento;

                // Si el facultamiento aplica, se le agrega el cuerpo de la tabla.
                if (facultamiento.Aplica) {
                    thead.append(trEncabezados);
                    const tbody = $(`<tbody></tbody>`);
                    let contador = 0;
                    for (const empleado of facultamiento.ListaEmpleados) {
                        const tr =
                            $(`<tr>
                                <td class="concepto">${empleado.Concepto}  
                                    <span class="label label-primary spanTipo pull-right">${(empleado.EsAutorizante)?'Autoriza':'VoBo'}</span>
                                </td>
                                <td>
                                    <input type="text" class="form-control" placeholder=${(empleado.EsAutorizante)?'Autoriza':'VoBo'}>
                                </td>
                            </tr>`);

                        const inputNombre = $(tr).find('input[type="text"]');

                        //if (contador === 0) {
                        //    tr.append(`<td><input checked class="aplica" type="checkbox"></td>`);
                        //} else 
                        if (!empleado.Aplica) {
                            const noAplicaLabel = $(`<span class="label label-danger spanTipo pull-right">No Aplica</span>`);
                            tr.find('td:first').append(noAplicaLabel);
                            inputNombre.prop('disabled', true);
                            tr.append(`<td><input class="aplica" type="checkbox"></td>`);
                        } else {
                            tr.append(`<td><input checked class="aplica" type="checkbox"></td>`);
                        }
                        contador++;
                        inputNombre.getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, null, '/OT/getEmpleados');
                        inputNombre.data().conceptoID = empleado.ConceptoID;
                        inputNombre.data().empleadoID = empleado.EmpleadoID;
                        inputNombre.data().claveEmpleado = empleado.ClaveEmpleado;
                        inputNombre.val(empleado.NombreEmpleado)
                        $(tbody).append(tr);
                    }
                    tabla.append(tbody);
                }
                $("#modalAsignacion #listadoFacultamientos").append(tabla);
            }
        }

        function agregarFacultamientosPlantilla(plantillas) {
            for (const facultamiento of plantillas) {

                const tabla =
                    $(`<table class="table table-fixed table-responsive table-hover table-striped"></table>`);

                const thead = $(`
                    <thead class="bg-table-header">
                    <tr>
                        <th title='Indicar que el facultamiento "${facultamiento.Titulo}" aplica para este paquete'
                        class="text-center titulo aplicaTabla" colspan="3"><span class="pull-left">Numero: ${facultamiento.orden}</span> ${facultamiento.Titulo}
                            <input disabled class="aplicaFacultamiento pull-right" type="checkbox">
                            <p class="pull-right aplicaFac">Aplica:</p>
                        </th>
                    </tr>
                </thead>`);

                tabla.append(thead).data().facultamiento = facultamiento;
                $("#modalAsignacion #listadoFacultamientos").append(tabla);
            }
        }

        function setClaveEmpledo(e, ul) {
            $(this).data().claveEmpleado = ul.item.id;
        }

        function verificarClaveEmpleado(e, ul) {
            if (ul.item == null) {
                $(this).val('');
                $(this).data().claveEmpleado = null;
            }
        }

        function limpiarCamposModal() {
            $('#listadoFacultamientos table').remove();
            comboCC[0].selectedIndex = 0;
            $('#modalAsignacion .input-group:first').removeClass('hiddenCombo');
            $('#modalAsignacion #tituloCC').text('');
            comboCC.prop('visible', false);
            modal.data().paqueteID = null;
            $(btnGuardar).attr('disabled', true);
            $('#tablaAutorizantes input').toArray().map(x => {
                $(x).data().usuarioID = null;
                $(x).data().autorizanteID = null;
                $(x).val('');
            });
            $('#tablaAutorizantes span').remove();
        }

        function actualizarPaqueteFacultamientos() {
            const paqueteID = modal.data().paqueteID;
            const listaAutorizantes = obtenerListaAutorizantes();
            const listaFacultamientos = obtenerListaFacultamientos(true);
            let todoCompleto = verificarLlenadoDeCampos(listaFacultamientos);
            $.blockUI({
                message: 'Actualizando paquete...'
            });
            $.post('/Administrativo/Facultamientos/ActualizarFacultamientos', {
                    paqueteID: paqueteID,
                    listaFacultamientos: listaFacultamientos,
                    listaAutorizantes: listaAutorizantes,
                    todoCompleto: todoCompleto
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        if (respuesta.ordenVoBo && respuesta.ordenVoBo > 0) {
                            verReporte(respuesta.paqueteID, false, respuesta.ordenVoBo);
                        } else {
                            AlertaGeneral("Éxito", "Operación Exitosa.");
                        }
                        modal.modal("hide");
                        btnLimpiar.click();
                        comboCC.fillCombo('/Administrativo/Facultamientos/LlenarComboCC', null, false, null);
                        comboObra.fillCombo('/Administrativo/Facultamientos/LlenarComboObras', null, false, null);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                })
                .always(() => $.unblockUI());
        }

        function camposValidos() {
            let contador = 0;
            let listaValidaciones = [];

            if (comboCC[0].selectedIndex !== 0) {
                contador++;
            } else if ($('#modalAsignacion .input-group:first').hasClass('hiddenCombo')) {
                contador++;
            } else {
                listaValidaciones.push("Centro de Costos");
            }

            const facultamientosAplicables = $('#listadoFacultamientos table')
                .find('tr:first')
                .find('input')
                .toArray()
                .filter(x => $(x).is(':checked'));
            if (facultamientosAplicables.length > 0) {
                contador++
            } else {
                listaValidaciones.push("Seleccionar mínimo un facultamiento como aplicable");
            }

            // if (primerAutorizante.val() !== "") {
            //     contador++;
            // } else {
            //     listaValidaciones.push("Seleccionar empleado autorizante");
            // }

            if (contador !== 2) {
                AlertaGeneral("Campos incompletos", "Por favor complete los campos incompletos: " + listaValidaciones + ".");
                return false;
            } else {
                return true;
            }
        }

        function verificarLlenadoDeCampos(listaFacultamientos) {
            let empleadosVacios;
            const facultamientosQueAplican = listaFacultamientos
                .filter(facultamiento => facultamiento.Aplica);

            for (let index = 0; index < facultamientosQueAplican.length; index++) {

                empleadosVacios = facultamientosQueAplican[index].ListaEmpleados
                    .filter(empleado => empleado.ClaveEmpleado === null);

                if (empleadosVacios.length !== 0) {
                    const empleadosNoAplica = empleadosVacios
                        .filter(empleado => !empleado.Aplica);

                    if (empleadosVacios.length !== empleadosNoAplica.length) {
                        return false;
                    }
                }
            }

            if (primerAutorizante.val() === "") {
                return false;
            }

            if (facultamientosQueAplican.lenth === 0) {
                return false;
            }

            return true;
        }

        function guardarPaqueteFacultamientos() {
            const centroCostosID = comboCC.val();
            const listaFacultamientos = obtenerListaFacultamientos(false);
            const listaAutorizantes = obtenerListaAutorizantes();
            let todoCompleto = verificarLlenadoDeCampos(listaFacultamientos);
            $.blockUI({
                message: 'Guardando paquete de facultamientos...'
            });
            $.post('/Administrativo/Facultamientos/AsignarFacultamientos', {
                    centroCostosID: centroCostosID,
                    listaFacultamientos: listaFacultamientos,
                    listaAutorizantes: listaAutorizantes,
                    todoCompleto: todoCompleto
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        if (respuesta.ordenVoBo && respuesta.ordenVoBo > 0) {
                            verReporte(respuesta.paqueteID, false, respuesta.ordenVoBo);
                        } else {
                            AlertaGeneral("Éxito", "Operación Exitosa.");
                        }
                        modal.modal("hide");
                        btnLimpiar.click();
                        comboCC.fillCombo('/Administrativo/Facultamientos/LlenarComboCC', null, false, null);
                        comboObra.fillCombo('/Administrativo/Facultamientos/LlenarComboObras', null, false, null);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function obtenerListaFacultamientos(esActualizacion) {
            const facultamientos = [];
            const listaFacultamientos = $('#listadoFacultamientos table')
                .toArray();
            listaFacultamientos.forEach(tablaFacultamiento => {
                const checkbox = $(tablaFacultamiento).find('tr:first').find('input');

                // Si el facultamiento aplica, se obtienen los empleados ingresados
                if (checkbox.is(':checked')) {
                    facultamientos.push(new Facultamiento(
                        $(tablaFacultamiento).data().facultamiento.PlantillaID,
                        obtenerListaEmpleados($(tablaFacultamiento), esActualizacion),
                        true,
                        $(tablaFacultamiento).data().facultamiento.FacultamientoID
                    ));
                }
                // Si no, se ponen los puestos por default
                else {
                    facultamientos.push(new Facultamiento(
                        $(tablaFacultamiento).data().facultamiento.PlantillaID,
                        obtenerLisaEmpleadosDefault($(tablaFacultamiento), esActualizacion),
                        false,
                        $(tablaFacultamiento).data().facultamiento.FacultamientoID
                    ));

                }
            });
            return facultamientos;
        }

        function obtenerLisaEmpleadosDefault(tablaFacultamiento, esActualizacion) {
            const conceptos = (esActualizacion) ?
                tablaFacultamiento.data().facultamiento.ListaEmpleados :
                tablaFacultamiento.data().facultamiento.ListaConceptos;
            const empleados = [];
            if (conceptos != null) {
                conceptos.forEach(concepto => {
                    empleados.push(
                        new Empleado(
                            (esActualizacion) ? concepto.ConceptoID : concepto.ID,
                            "",
                            null,
                            (concepto.EmpleadoID) ? concepto.EmpleadoID : 0,
                            false
                        ));
                });
            }
            return empleados;
        }

        function obtenerListaEmpleados(tablaFacultamiento, esActualizacion) {
            const empleados = tablaFacultamiento.find('input[type="text"]')
                .toArray()
                .map(x => {
                    if ($(x).val() !== "") {
                        return new Empleado(
                            $(x).data().conceptoID,
                            $(x).val().trim(),
                            $(x).data().claveEmpleado,
                            ($(x).data().empleadoID) ? $(x).data().empleadoID : 0,
                            true
                        )
                    } else if ($(x).closest('tr').find('.aplica').is(':checked')) {
                        return new Empleado(
                            $(x).data().conceptoID,
                            "",
                            null,
                            ($(x).data().empleadoID) ? $(x).data().empleadoID : 0,
                            true
                        )
                    } else {
                        return new Empleado(
                            $(x).data().conceptoID,
                            "",
                            null,
                            ($(x).data().empleadoID) ? $(x).data().empleadoID : 0,
                            false
                        )
                    }
                });
            return empleados;
        }

        function obtenerListaAutorizantes() {
            const listaAutorizantes = $('#tablaAutorizantes input').toArray()
                .map(x => {
                    if ($(x).val() !== "") {
                        return new Empleado(
                            null,
                            $(x).val().trim(),
                            $(x).data().claveEmpleado,
                            ($(x).data().autorizanteID) ? $(x).data().autorizanteID : 0
                        )
                    } else {
                        return new Empleado(
                            0,
                            "",
                            null,
                            ($(x).data().autorizanteID) ? $(x).data().autorizanteID : 0
                        )
                    }
                });
            return listaAutorizantes;
        }

        init();
    }
    $(document).ready(() => administrativo.facultamiento.Asignacion = new Asignacion());
})();