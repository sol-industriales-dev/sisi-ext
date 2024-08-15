(function () {
    $.namespace('controlObra.PlantillaInforme');
    modalGuardar = $('#modalGuardar');
    let divisionSeleccionada = '';

    PlantillaInforme = function () {

        const getDivisiones = () => $.post('/ControlObra/ControlObra/ObtenerDivisiones');
        const guardarPlantilla = (plantilla, plantilla_detalle) => { return $.post('/ControlObra/ControlObra/GuardarPlantilla', { plantilla, plantilla_detalle }) };


        //#region RADIO CHECK
        $(document).on('click', '[data-toggle="wizard-radio"]', function () {
            wizard = $(this).closest('.wizard-card');
            wizard.find('[data-toggle="wizard-radio"]').removeClass('active');
            $(this).addClass('active');
            $(wizard).find('[type="radio"]').removeAttr('checked');
            $(this).find('[type="radio"]').attr('checked', 'true');
            divisionSeleccionada = $("input[type='radio']:checked").val();
            $('#btn_crearPlantilla').attr('data-divisionID', divisionSeleccionada);

            $('#rowInformes').css("display", "block");
            mostrarInformes();
            getPlantillaDivision(divisionSeleccionada);
        });
        //#endregion

        //#region CLICK EVENTS
        $('#btn_crearPlantilla').on("click", function () {
            $('#modalPlantilla').modal('show');
        });

        $('.addDiapositiva').on("click", function () {
            $('#modalPlantilla .modal-body').append(createInputTituloDiapositiva())
        })

        $('.removeDiapositiva').on("click", function () {
            $('div.tituloDiapositiva').last().remove()
        })

        $('#btnGuardarDiapositiva').on("click", function () {
            guardar();
        })

        $(document).on('input', 'input.tituloDiapositiva', function () {
            validarGuardar()
        })
        //#endregion

        //#region METODOS
        function mostrarInformes() {
            $('#tbodyInformes tr').remove();

            let division_id = parseInt(divisionSeleccionada)
            $.post('/ControlObra/ControlObra/GetInformesSemanal', { division_id })
                .done(response => {
                    if (response.success) {
                        response.items.forEach(element => {
                            $('#tbodyInformes').append(createRowInformes(element));
                        });
                    }
                });
        }

        function createRowInformes(element) {
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlInforme = baseUrl + `/ControlObra/ControlObra/InformeSemanal?informePresentacion=${element.id}`;

            const tr = `
                <tr>
                    <td width="5%"><a href="${urlInforme}" class="waves-effect waves-block">${element.cc}</a></td>
                    <td width="30%"><a href="${urlInforme}" class="waves-effect waves-block">${element.cc_desc}</a></td>
                    <td width="5%"><a href="${urlInforme}" class="waves-effect waves-block">${element.fecha_st}</a></td>
                </tr>
            `;

            return tr;
        }

        function getPlantillaDivision(divisionSeleccionada) {
            let divisionID = parseInt(divisionSeleccionada);

            $('#ulOpcionesPlantilla .editPlantilla').remove()
            $('#titleInformes').text('')

            $.post('/ControlObra/ControlObra/GetPlantillaDivision', { divisionID })
                .done(response => {
                    if (response.success) {
                        $('#ulOpcionesPlantilla').append(createButtonPlantillaEdit(response.plantilla.id))
                        $('#titleInformes').append(response.division.division);
                    } else {
                        $('#titleInformes').append(response.division.division);
                    }
                })

        }

        function createButtonPlantillaEdit(plantilla_id) {
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlPlantilla = baseUrl + `/ControlObra/ControlObra/PlantillaInformeEditor?plantilla=${plantilla_id}`;

            const li = ` <li><a href="${urlPlantilla}" class="waves-effect waves-block editPlantilla">Editar Plantilla</a></li>`;

            return li;
        }

        function createDivisionesCheck(division) {
            //let disabled = division.ccValido ? "enabled" : "disabled"

            let div = `
                        <div class="col-sm-3">
                            <div class="choice" data-toggle="wizard-radio" usuario_cc={${division.usuario_cc}}>
                                <input type="radio" name="job" value=${division.id}>
                                <div class="icon">
                                    <i class="${setIconDivision(division.id)}"></i>
                                </div>
                                <h6>${division.division}</h6>
                            </div>
                        </div>
                    `;
            return div;
        }

        function setIconDivision(id) {
            let clase = "";
            switch (id) {
                case 1:
                    clase = "fas fa-truck-monster"
                    break;
                case 2:
                    clase = "fas fa-snowplow"
                    break;
                case 3:
                    clase = "fas fa-building"
                    break;
                case 4:
                    clase = "fas fa-road"
                    break;
                case 5:
                    clase = "fas fa-charging-station"
                    break;
                case 6:
                    clase = "fas fa-apple-alt"
                    break;
                case 7:
                    clase = "fas fa-car"
                    break;
                case 8:
                    clase = "fas fa-car"
                    break;
                default:
                    clase = ""
                    break;
            }
            return clase;
        }

        function createInputTituloDiapositiva() {
            const form = `
                        <div class="form-group tituloDiapositiva">
                            <div class="form-line">
                    	        <input type="text" class="form-control tituloDiapositiva" placeholder="Titulo diapositiva">
                            </div>
                        </div>
                    `;
            return form;
        }

        function validarGuardar() {
            let invalidos = 0;
            $("input.tituloDiapositiva").each(function () {
                if ($(this).val() == "") {
                    invalidos++;
                    $(this).parents('.form-line').addClass('error');
                }
            })

            $('#btnGuardarDiapositiva').prop('disabled', invalidos > 0)
        }

        function guardar() {

            const plantilla = {
                division_id: divisionSeleccionada,
                cantidadDiapositivas: $('div.tituloDiapositiva').length,
                estatus: true
            }

            let plantilla_detalle = [];

            $("input.tituloDiapositiva").each(function (index, value) {
                plantilla_detalle.push({ ordenDiapositiva: index + 1, tituloDiapositiva: value.value })
            })

            $.blockUI({ message: "Preparando información" });
            guardarPlantilla(plantilla, plantilla_detalle).done(function (response) {
                if (response.success) {
                    $('#modalGuardar').modal('hide');
                    limpiar();
                    $.unblockUI();

                    Swal.fire({
                        title: 'La plantilla ha sido guardada, Se redirigirá a la edición de la plantilla',
                        type: 'success',
                        showConfirmButton: true,
                        confirmButtonText: 'Aceptar',
                        width: 700
                    }).then((result) => {
                        if (result.value) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlPlantilla = baseUrl + `/ControlObra/ControlObra/PlantillaInformeEditor?plantilla=${response.plantilla}`;

                            window.location.href = urlPlantilla;
                        }
                    })
                } else {
                    Swal.fire({
                        type: 'error',
                        title: 'Oops...',
                        text: response.error
                        // footer: '<a href>Why do I have this issue?</a>'
                    });
                    $.unblockUI();
                }
            });

        }

        /**
         * Function to sort alphabetically an array of objects by some specific key.
         * 
         * @param {String} property Key of the object to sort.
        */
        function dynamicSort(property) {
            var sortOrder = 1;

            if (property[0] === "-") {
                sortOrder = -1;
                property = property.substr(1);
            }

            return function (a, b) {
                if (sortOrder == -1) {
                    return b[property].localeCompare(a[property]);
                } else {
                    return a[property].localeCompare(b[property]);
                }
            }
        }

        function limpiar() {
            $('#modalPlantilla').modal('hide');
            $('input.tituloDiapositiva').val('').trigger('input');
            $('div').find('[data-toggle="wizard-radio"]').removeClass('active');
            $('div').find('[type="radio"]').removeAttr('checked');
        }
        //#endregion

        function init() {
            $('#divNumDiapositivas').hide();
            $('#btnGuardar').hide();

            $.blockUI({ message: "Preparando información" });
            getDivisiones().done(function (response) {
                if (response.success) {

                    $.each(response.items, function (index, value) {
                        $('#divisiones').append(createDivisionesCheck(value));
                    });
                    $.unblockUI();
                } else {
                    $.unblockUI();
                }
            });
        }

        init();
    }

    $(document).ready(function () {
        controlObra.PlantillaInforme = new PlantillaInforme();
    });
})();