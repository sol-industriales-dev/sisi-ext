(function () {
    $.namespace('administrativo.facultamiento.Catalogo');
    Catalogo = function () {

        // Clase base para crear un concepto.
        class Concepto {
            constructor(concepto, EsAutorizante, id) {
                this.concepto = concepto;
                this.EsAutorizante = EsAutorizante;
                this.ID = id;
            }
        }
        let currentPuesto = 0;
        let currentRow = null;
        // Variables Catálogo.
        let dataTableCatalogo;
        const tablaFacultamientos = $("#tablaFacultamientos");
        const cboDepartamentoFiltro = $("#cboDepartamentoFiltro");
        const btnNuevoFacultamiento = $("#btnNuevoFacultamiento");

        // Variables Modal.
        const titutloModal = $(".modal-title");
        const inputFacultamiento = $("#inputFacultamiento");
        const cboDepartamento = $("#cboDepartamento");
        const btnNuevaPersona = $("#btnNuevaPersona");
        const btnGuardar = $("#btnGuardar");
        const modalEliminarPuesto = $("#modalEliminarPuesto");
        const btnEliminarPuesto = $("#btnEliminarPuesto");

        // Otras.
        let esAuditor = false;
        let plantillaEditarID;

        function llenarCombos() {
            cboDepartamento.fillCombo('/Administrativo/Facultamientos/LlenarComboDepartamentos', null, false, "Todos");
            convertToMultiselect("#cboDepartamento");
            cboDepartamentoFiltro.fillCombo('/Administrativo/Facultamientos/LlenarComboDepartamentos', null, false, "Todos");
        }

        function obtenerCatalogo(departamentoID) {
            $.get('/Administrativo/Facultamientos/ObtenerCatalogo', {
                departamentoID: departamentoID
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        cargarFacultamientos(respuesta.listaFacultamientos);
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
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function cargarFacultamientos(data) {
            if (dataTableCatalogo != null) {
                dataTableCatalogo.destroy();
            }
            dataTableCatalogo = tablaFacultamientos.DataTable({
                language: dtDicEsp,
                destroy: true,
                data: data,
                order: [[0, 'asc']],
                columns:
                    [
                        {
                            data: 'orden'
                        },
                        {
                            data: 'Titulo'
                        },
                        {
                            data: 'Departamento'
                        },
                        {
                            data: 'Fecha'
                        },
                        {
                            data: 'PlantillaID',
                            createdCell: function (td, cellData, rowData, row, col) {
                                const divFlex = $('<div class="flexContainer"></div>');
                                const botonEditar = $('<button class="btn btn-warning btnEditar"><i class="fa fa-edit"></i> Actualizar</button>');
                                // const botonEliminar = $('<button disabled class="btn btn-danger"><i class="fa fa-times"></i> Eliminar</button>');
                                $(td).html(divFlex);
                                $(divFlex).append(botonEditar);
                                // $(divFlex).append(botonEliminar);
                            }
                        }
                    ],
                drawCallback: function (settings) {
                    $('#tablaFacultamientos .btnEditar').unbind().click(function () {
                        cargarPlantilla(dataTableCatalogo.row($(this).parents('tr')).data().PlantillaID);
                    });
                }
            });
        }

        function cargarPlantilla(plantillaID) {
            $.get('/Administrativo/Facultamientos/ObtenerPlantilla', {
                plantillaID: plantillaID
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        editarPlantilla(respuesta.plantilla, plantillaID);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                        return;
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                    return;
                });
        }

        function editarPlantilla(plantilla, plantillaID) {
            $(titutloModal).html('<i class="fa fa-pencil"></i> Actualizar Plantilla de Facultamiento');
            $(btnGuardar).html('<i class="fa fa-save"></i> Guardar');
            plantillaEditarID = plantillaID;
            $("#modalFacultamiento").modal("show");
            inputFacultamiento.val(plantilla.Titulo);
            seleccionarDepartamentos(plantilla.ListaDepartamentos);
            cargarConceptos(plantilla.ListaConceptos);
        }

        function seleccionarDepartamentos(listaDepartamentos) {
            resetMultiselect();
            for (const departamentoID of listaDepartamentos) {
                let boton = $(`#modalFacultamiento span.multiselect-native-select > span > div > ul > li > a > label`)
                    .toArray().filter(x => $(x).find('input[type="checkbox"]').val() == departamentoID);
                $(boton).click();
                let li = $(`#modalFacultamiento span.multiselect-native-select > span > div > ul > li`)
                    .toArray().filter(x => $(x).find('input[type="checkbox"]').val() == departamentoID);
                //$(li).addClass('disabled');
            }
        }

        function cargarConceptos(listaConceptos) {
            //$("#tablaAutorizacion tbody tr.first input[type='text']").val(listaConceptos[0].Concepto);
            //$("#tablaAutorizacion tbody tr.first input[type='text']").data().conceptoID = listaConceptos[0].ID;
            //listaConceptos.splice(0, 1);
            listaConceptos.forEach(element => {
                agregarFilaAutorizacion(element, true);
            });
        }

        function agregarFilaAutorizacion(concepto, esActualizacion) {

            const row = $("<tr></tr>");

            const tdConcepto = $("<td></td>");
            const input = $('<input type="text" class="form-control inputPersona letras"/></td>');
            if (!esActualizacion) {
                const button = $('<button class="btn btn-danger btnEliminar"><i class="fa fa-times"></i></button>');
                tdConcepto.append(button);
            }
            else {
                const button = $('<button class="btn btn-danger btnEliminarCompleto" data-id="' + concepto.ID + '"><i class="fa fa-times"></i></button>');
                tdConcepto.append(button);
            }
            tdConcepto.append(input);

            let renglon = $("#tablaAutorizacion tbody tr").last().find("input").last().prop("name") ?? 0
            renglon++;

            const tdTipo = $("<td></td>");
            const radioButtonAut = $(`
            <input type="radio" name="${renglon}"><label class="labelRadio">Autoriza</label>
            `);
            const radioButtonVoBo = $(`
            <input type="radio" name="${renglon}"><label class="labelRadio">VoBo</label>
            `);

            if (concepto && concepto.Concepto) {
                $(input).val(concepto.Concepto.trim());
                (concepto.EsAutorizante) ? $(radioButtonAut).prop('checked', true) : $(radioButtonVoBo).prop('checked', true);
                $(input).data().conceptoID = concepto.ID;
            }

            tdTipo.append(radioButtonAut).append(radioButtonVoBo);
            row.append(tdConcepto).append(tdTipo);

            $("#tablaAutorizacion tbody").append(row);

            $(".btnEliminar").click(function () {
                $(this).parents("tr").remove();
            });
            $(".btnEliminarCompleto").click(function () {
                var id = $(this).data("id");
                currentRow = $(this).parents("tr");
                currentPuesto = id;
                modalEliminarPuesto.modal("show");
            });

        }

        function camposValidos() {
            let contador = 0;
            let listaValidaciones = [];
            if (inputFacultamiento.val().trim() !== "") {
                contador++;
            } else {
                listaValidaciones.push("título")
            }
            if (getValoresMultiples("#cboDepartamento")[0] != null) {
                contador++;
            } else {
                listaValidaciones.push("seleccionar departamento")
            }
            const conceptos = $("#tablaAutorizacion > tbody td input[type='text']").toArray();
            const conceptosDefinidos = conceptos.filter(x => $(x).val().trim() !== "");
            if (conceptosDefinidos.length === conceptos.length) {
                contador++;
            } else {
                listaValidaciones.push("conceptos")
            }
            const radiosBtn = $("#tablaAutorizacion > tbody > tr > td:nth-child(2) input[type='radio']").toArray();
            const radioSelected = radiosBtn.filter(x => $(x).is(':checked'));
            if (radioSelected.length === (radiosBtn.length / 2)) {
                contador++;
            } else {
                listaValidaciones.push("tipo de autorización")
            }
            if (contador !== 4) {
                AlertaGeneral("Campos incompletos", "Por favor complete los campos incompletos: " + listaValidaciones + ".");
                return false;
            } else {
                return true;
            }
        }

        function guardarPlantilla() {
            const nombreFacultamiento = inputFacultamiento.val().trim();
            const departamentos = getValoresMultiples("#cboDepartamento");
            const conceptos = obtenerConceptos();

            $.post('/Administrativo/Facultamientos/GuardarPlantilla', {
                titulo: nombreFacultamiento,
                listaDepartamentos: departamentos,
                listaConceptos: conceptos
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        $("#modalFacultamiento").modal("hide");
                        cboDepartamentoFiltro[0].selectedIndex = 0;
                        AlertaGeneral("Éxito", "Plantilla guardada correctamente.")
                        obtenerCatalogo(0);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }

        function actualizarPlantilla(plantillaID) {
            const nuevoTitulo = inputFacultamiento.val().trim();
            const nuevosDepartamentos = obtenerNuevosDepartamentos();
            const nuevosConceptos = obtenerConceptos();

            $.post('/Administrativo/Facultamientos/ActualizarPlantilla', {
                nuevoTitulo: nuevoTitulo,
                nuevosDepartamentos: nuevosDepartamentos,
                nuevosConceptos: nuevosConceptos,
                plantillaID: plantillaID,
                esActualizar: nuevoTitulo != "" ? true : false
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        $("#modalFacultamiento").modal("hide");
                        // AlertaGeneral("Éxito", "Plantilla actualizada correctamente.");
                        Alert2Exito("Se ha actualizado con éxito.");
                        cboDepartamentoFiltro[0].selectedIndex = 0;
                        obtenerCatalogo(0);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }

        function obtenerNuevosDepartamentos() {
            return nuevosDepartamentos =
                $(`#modalFacultamiento span.multiselect-native-select li:not(.disabled) label input[type="checkbox"]`)
                    .toArray()
                    .filter(x => $(x).is(':checked'))
                    .map(x => $(x).val());
        }

        function obtenerConceptos() {

            const conceptos = $("#tablaAutorizacion > tbody td input[type='text']")
                .toArray();

            const autorizantes = $("#tablaAutorizacion > tbody > tr > td:nth-child(2) input[type='radio']:nth-child(1)")
                .toArray()
                .map(input => $(input).is(':checked'));

            const listaConceptos = [];
            for (let index = 0; index < conceptos.length; index++) {
                listaConceptos.push(new Concepto(
                    $(conceptos[index]).val().trim(),
                    autorizantes[0],
                    (($(conceptos[index]).data()) && $(conceptos[index]).data().conceptoID) ?
                        $(conceptos[index]).data().conceptoID :
                        null
                ));
                autorizantes.splice(0, 1);
            }
            return listaConceptos;
        }

        function limpiarCamposModal() {
            $(titutloModal).html('<i class="fa fa-plus"></i> Añadir Plantilla de Facultamiento');
            $(btnGuardar).html('<i class="fa fa-save"></i> Guardar');
            plantillaEditarID = null;
            inputFacultamiento.val('');
            $("#tablaAutorizacion tbody tr:not(.first)").remove();
            $("#tablaAutorizacion tbody tr.first").find("input").first().val('');
            resetMultiselect();
        }

        function resetMultiselect() {
            let li = $(`#modalFacultamiento span.multiselect-native-select > span > div > ul > li`);
            $(li).removeClass('disabled');
            limpiarMultiselect("#cboDepartamento");
        }

        function sanitizeString(str) {
            str = str.replace(/[^a-z0-9áéíóúñü()\/ .]/gim, "");
            return str.trim();
        }
        function eliminarPuesto() {
            $.post('/Administrativo/Facultamientos/delPuesto', {
                id: currentPuesto
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        currentRow.remove();
                        modalEliminarPuesto.modal("hide");

                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }
        function agregarListeners() {
            cboDepartamentoFiltro.change(function () {
                if ($(this).val() != "Todos") {
                    obtenerCatalogo(cboDepartamentoFiltro.val());
                } else {
                    obtenerCatalogo(0);
                }
            });

            btnNuevoFacultamiento.click(() => {
                $("#modalFacultamiento").modal("show");
            });

            $('#modalFacultamiento').on('change', '.letras', function () {
                this.value = sanitizeString(this.value);
            });

            $("#modalFacultamiento").on("hide.bs.modal", limpiarCamposModal);

            btnNuevaPersona.click(() => {
                if (plantillaEditarID) {
                    agregarFilaAutorizacion(false, true);
                } else {
                    agregarFilaAutorizacion();
                }
            });

            btnGuardar.click(() => {
                if (camposValidos()) {
                    if (plantillaEditarID) {
                        actualizarPlantilla(plantillaEditarID);
                    } else {
                        guardarPlantilla();
                    }
                }
            });
            btnEliminarPuesto.click(() => {
                eliminarPuesto();
            });
        }

        function init() {
            llenarCombos();
            obtenerCatalogo(0);
            agregarListeners();
        }

        init();
    }
    $(document).ready(() => administrativo.facultamiento.Catalogo = new Catalogo())
        .ajaxStart((() => $.blockUI({
            message: 'Procesando...'
        })))
        .ajaxStop(() => $.unblockUI());
})();