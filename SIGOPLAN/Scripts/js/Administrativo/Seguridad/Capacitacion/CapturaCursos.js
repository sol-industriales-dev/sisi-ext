(function () {

    $.namespace('administrativo.seguridad.CapturaCursos');

    CapturaCursos = function () {

        //#region VARIABLES
        const hoy = new Date();
        let esEdit = false
        tblCursos = $("#tblCursos");
        //#endregion

        //#region PETICIONES
        const getCursos = (clasificaciones, puestos, estatus) => { return $.post('/Administrativo/Capacitacion/GetCursos', { clasificaciones, puestos, estatus }) };
        const getTipoExamen = (curso_id) => { return $.post('/Administrativo/Capacitacion/GetTipoExamen', { curso_id }) };
        const GuardarCurso = (curso) => { return $.post('/Administrativo/Capacitacion/GuardarCurso', { curso }) };
        const ActualizarCurso = (curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto) => {
            return $.post('/Administrativo/Capacitacion/ActualizarCurso', { curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto })
        };
        const GuardarExamenes = (examenes) => { return $.post('/Administrativo/Capacitacion/GuardarExamenes', { examenes }) };
        const DescargarArchivo = (examen_id) => { return $.post('/Administrativo/Capacitacion/DescargarArchivo', { examen_id }) };
        const EliminarExamen = (examen_id) => { return $.post('/Administrativo/Capacitacion/EliminarExamen', { examen_id }) };
        //const getClasificacion = () => { return $.post('/Administrativo/Capacitacion/GetClasificacionCursos') };
        //#endregion

        const esCreador = $('#inputEsCreador').val() == "True";
        const puedeEliminar = $('#inputPuedeEliminarCursos').val() == "True";

        const checkboxTodosPuestos = $('#checkboxTodosPuestos');
        const checkboxCapacitacionUnica = $('#checkboxCapacitacionUnica');
        const botonModalCargaMasiva = $('#botonModalCargaMasiva');
        const modalCargaMasiva = $('#modalCargaMasiva');
        const botonGuardarCargaMasiva = $('#botonGuardarCargaMasiva');
        const checkboxTodosPuestosMandos = $('#checkboxTodosPuestosMandos');

        _privilegioUsuario = 0;

        (function init() {
            revisarPrivilegio();
            setDatosInicio();
            initTable();
            agregarListeners();
        })();

        $('#selectMando').on('change', function () {
            let listaMandos = getValoresMultiples('#selectMando').filter(function (x) { return parseInt(x) });

            axios.post('GetPuestosMandos', { mandos: listaMandos })
                .then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        $("#selectPuestoCurso").fillCombo(response.data, null, false, 'Todos');
                        $("#selectPuestoCurso").find('option[value="Todos"]').remove();
                        $('#selectPuestoCurso').select2(); // convertToMultiselect('#selectPuestoCurso');

                        $("#selectPuestoAutorizacionCurso").fillCombo(response.data, null, false, 'Todos');
                        convertToMultiselect('#selectPuestoAutorizacionCurso');
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        });

        checkboxTodosPuestosMandos.on('click', function () {
            if (checkboxTodosPuestosMandos.is(':checked')) {
                $('#selectPuestoCurso').find('option').prop("selected", true);
                $('#selectPuestoCurso').trigger("change");
            } else {
                $('#selectPuestoCurso').find('option').prop("selected", false);
                $('#selectPuestoCurso').trigger("change");
            }
        });

        function agregarListeners() {
            $('#btnBuscar').click(buscarCursos)
            $('#btnRegistrarCurso').click(setModalAgregar)
            $('#btnGuardarCurso').click(guardarCurso)
            $('#btnGuardarExamenes').click(guardarExamen)

            $('#modalRegistroCurso').on('hide.bs.modal', () => {
                esEdit = false;

                $('#selectPuestoCurso').val(null).select2().trigger('change'); // limpiarMultiselect("#selectPuestoCurso");
                limpiarMultiselect('#selectPuestoAutorizacionCurso');

                $('#modalRegistroCurso button.multiselect.dropdown-toggle.btn.btn-default').attr('disabled', false);


                $("#selectClasificacionCurso").attr('disabled', false);
                checkboxTodosPuestos.attr('disabled', false);
                checkboxTodosPuestos[0].checked = false;
                checkboxCapacitacionUnica[0].checked = false;
            }
            );

            $('#btnImprimir').on('click', function () {
                $.blockUI({ message: 'Procesando...' });

                $(this).download = '/Administrativo/Capacitacion/CrearExcelRelacionCursosPuestos';
                $(this).href = '/Administrativo/Capacitacion/CrearExcelRelacionCursosPuestos';

                location.href = '/Administrativo/Capacitacion/CrearExcelRelacionCursosPuestos';

                $.unblockUI();
            });

            checkboxTodosPuestos.click(alternarComboPuestos);
            botonModalCargaMasiva.click(() => modalCargaMasiva.modal('show'));
            botonGuardarCargaMasiva.click(guardarCargaMasiva);
        }

        function alternarComboPuestos() {
            $('#modalRegistroCurso button.multiselect.dropdown-toggle.btn.btn-default').attr('disabled', this.checked);

            if (this.checked) {
                $('#selectPuestoCurso').val(null).select2().trigger('change') // limpiarMultiselect("#selectPuestoCurso");
            }
        }

        //#region CARGAS INICIALES
        function setDatosInicio() {
            $('#selectClasificacionCurso').fillCombo('GetClasificacionCursos', null, false);
            $('#selectClasificacion').fillCombo('GetClasificacionCursos', null, false);
            convertToMultiselect('#selectClasificacion');

            $('#selectEstatus').fillCombo('GetEstatusCursos', null, false, 'Todos');

            $("#selectMando").fillCombo('GetMandosEnum', null, false, 'Todos');
            convertToMultiselect('#selectMando');

            $("#selectPuesto").fillCombo('GetPuestos', null, false, 'Todos');
            convertToMultiselect('#selectPuesto');

            $("#selectPuestoCurso").fillCombo('GetPuestos', null, false, 'Todos');
            $("#selectPuestoCurso").find('option[value="Todos"]').remove();
            $('#selectPuestoCurso').select2(); // convertToMultiselect('#selectPuestoCurso');

            // $("#selectCentroCosto").fillCombo('ObtenerComboCCAmbasEmpresas', null, false, 'Todos');
            // convertToMultiselect('#selectCentroCosto');

            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;

                        x.options.forEach(y => {
                            let empresa = x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0;

                            groupOption += `<option value="${y.Value}_${empresa}" empresa="${empresa}">${y.Text}</option>`;
                        });

                        $("#selectCentroCosto").append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            $("#selectPuestoAutorizacionCurso").fillCombo('GetPuestos', null, false, 'Todos');
            convertToMultiselect('#selectPuestoAutorizacionCurso');

            esEdit = false;
        }

        function initTable() {
            tblCursos = $("#tblCursos").DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                dom: 't',
                scrollY: "500px",
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblCursos.on('click', '.btn-editar-informacion', function () {
                        var rowData = tblCursos.row($(this).closest('tr')).data();

                        $.ajax({
                            url: 'GetCursoById',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                const data = response.informacion;
                                esEdit = true;

                                checkboxTodosPuestosMandos.prop('checked', false);
                                $("#btnGuardarCurso").val(rowData["id"]);
                                $("#claveCurso").val(data.claveCurso);
                                $("#nombreCurso").val(data.nombre);
                                $("#duracionCurso").val(data.duracion);
                                $("#selectClasificacionCurso").val(data.clasificacion).change();

                                $("#selectMando").val(data.mandos.map(x => x.mando));
                                $("#selectMando").multiselect("refresh");
                                $("#selectPuestoCurso").val(data.puestos.map(x => x.puesto_id));
                                $('#selectPuestoCurso').select2().trigger('change'); // $("#selectPuestoCurso").multiselect("refresh");
                                $("#selectPuestoAutorizacionCurso").val(data.puestosAutorizacion.map(x => x.puesto_id));
                                $("#selectPuestoAutorizacionCurso").multiselect("refresh");

                                $("#selectCentroCosto").multiselect('deselectAll', false);

                                data.centrosCosto.forEach(x => {
                                    $('#selectCentroCosto').find(`option[value="${x.cc}_${x.empresa}"]`).prop('selected', true);
                                });

                                $("#selectCentroCosto").multiselect("refresh");

                                // $("#selectCentroCosto").val(data.centrosCosto.map(x => x.cc));


                                $("input[name=cursoGeneral][value='" + data.esGeneral + "']").prop('checked', true)
                                $("#objetivoCurso").val(data.objetivo);
                                $("#temaCurso").val(data.temasPrincipales);
                                $("#referenciasNormativas").val(data.referenciasNormativas);
                                $("#notaCurso").val(data.nota);

                                $("#selectClasificacionCurso").attr('disabled', true);
                                checkboxTodosPuestos.attr('disabled', true);
                                checkboxTodosPuestos[0].checked = data.aplicaTodosPuestos;
                                checkboxCapacitacionUnica[0].checked = data.capacitacionUnica;

                                $('#modalRegistroCurso').modal('show');
                            }
                        });
                    });

                    tblCursos.on('click', '.btn-alta-examen', function () {
                        var rowData = tblCursos.row($(this).closest('tr')).data();

                        $.ajax({
                            url: 'GetExamenesCursoById',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                const data = response.informacion;

                                $("#titleCurso").empty();
                                $("#titleCurso").append('Examenes Curso - ' + rowData["nombre"]);
                                $("#btnAgregarExamen").val(rowData["id"]);
                                $("#btnGuardarExamenes").val(rowData["id"]);
                                $("#btnGuardarExamenes").attr('data-nombre', rowData["nombre"]);
                                $('#tblExamenes tbody tr').remove()

                                if (response.success) {
                                    data.forEach(function (element, index) {
                                        $('#tblExamenes tbody').append(setRowsTableExamenesCurso(element));
                                    });
                                } else {
                                    $('#tblExamenes tbody').append(setNewRowTableExamenes(''));
                                }

                                $('#modalAltaExamen').modal('show');
                            },
                            error: function (response) {
                                //$('#tblExamenes tbody').append(setRowsTableExamenes());
                            }
                        });
                    });

                    tblCursos.on('click', '.btn-eliminar', function () {
                        var rowData = tblCursos.row($(this).closest('tr')).data();

                        if (rowData == null || rowData.id <= 0) {
                            return;
                        }

                        AlertaAceptarRechazarNormal(
                            'Confirmar eliminación',
                            `¿Está seguro de eliminar este curso? Eliminar este curso provocará que todas las capacitaciones vinculadas a este curso también se eliminen.`,
                            () => eliminarCurso(rowData.id))
                    });

                },
                columns: [
                    { data: 'claveCurso', title: 'Clave Curso' },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'estatus', title: 'Estatus' },
                    { data: 'duracion', title: 'Duración (Horas)' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    {
                        sortable: false,
                        render: function (data, type, row, meta) {
                            return _privilegioUsuario != 3 ? `
                                <button title="Editar" class="btn-editar-informacion btn btn-xs btn-warning" value="${row.id}"><i class="fas fa-pencil-alt"></i></button>&nbsp;
                                ${(row.clasificacion != 3 && row.clasificacion != 4) ? `<button title="Gestionar exámenes" class="btn-alta-examen btn btn-xs btn-info" value="${row.id}"><i class="far fa-file-word"></i></button>&nbsp;` : ``}
                                ${puedeEliminar ? `<button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger" value="${row.id}"><i class="fas fa-trash"></i></button>` : ``}
                            ` : ``;
                        },
                        title: "Acciones"
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [0, 2, 3, 4, 5] },
                    { "width": "10%", "targets": 0 },
                    { "width": "40%", "targets": 1 },
                    { "width": "15%", "targets": 2 },
                    { "width": "10%", "targets": 3 },
                    { "width": "15%", "targets": 4 }
                ]
            });
        }

        function eliminarCurso(cursoID) {

            $.post('/Administrativo/Capacitacion/EliminarCurso', { cursoID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Curso eliminado correctamente.`);
                        buscarCursos();
                        esEdit = false;
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        function setModalAgregar() {
            checkboxTodosPuestosMandos.prop('checked', false);
            $("#claveCurso").val("");
            $("#nombreCurso").val("");
            $("#duracionCurso").val("");
            $("#objetivoCurso").val("");
            $("#temaCurso").val("");
            $("#referenciasNormativas").val("");
            $("#notaCurso").val("");
            $(".nombreExamen").val("");

            $('#modalRegistroCurso').modal('show');

            $("#claveCurso").focus();

            $('#selectClasificacionCurso').val('');
            $('#selectClasificacionCurso').select2().change();

            $("#selectMando").multiselect('deselectAll', false);
            $('#selectMando').multiselect('refresh');
            $("#selectCentroCosto").multiselect('deselectAll', false);
            $('#selectCentroCosto').multiselect('refresh');
        }
        //#endregion

        //#region METODOS GENERALES
        function buscarCursos() {
            const clasificaciones = getValoresMultiples('#selectClasificacion');
            const puestos = getValoresMultiples('#selectPuesto');
            let estatus = $('#selectEstatus').val();

            if (estatus == "Todos") {
                estatus = 0;
            }

            $.blockUI({ message: 'Cargando cursos...' });
            getCursos(clasificaciones, puestos, estatus)
                .always($.unblockUI)
                .done(function (response) {
                    if (response.success) {
                        tblCursos.clear().rows.add(response.items).draw();
                    } else {
                        tblCursos.clear().draw();
                    }
                });
        }

        function setNewRowTableExamenes() {
            const tr = `
                        <tr>
                            <td></td>
                            <td></td>
                            <td>                                                
                                <input type="file" class="inputfile nuevoArchivo" />
                                <button class="btn btn-sm btn-danger eliminarArchivo" data-id="0">X</button>
                            </td>
                        </tr>
                     `;

            return tr;
        }

        function setRowsTableExamenesCurso(data) {
            const tr = `
                <tr>
                    <td>${data.nombreExamen}</td>
                    <td>${data.tipoExamen}</td>
                    <td>
                        <button class="btn btn-sm btn-primary descargarArchivo" data-id="${data.id}">Descargar Archivo</button>
                        ${esCreador ? `<button class="btn btn-sm btn-danger eliminarArchivo" data-id="${data.id}">X</button>` : ''}
                    </td>
                    
                </tr>
            `;

            return tr;
        }

        function paintCells(tr) {
            tr.addClass("active-row")
        }

        function unpaintCells(tr) {
            tr.removeClass("active-row");
        }
        //#endregion

        //#region GUARDAR
        function validarGuardarCurso() {
            let countInvalidos = 0

            if ($("#claveCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#selectClasificacionCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#nombreCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#duracionCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#objetivoCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#temaCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#referenciasNormativas").val().trim() == "") {
                countInvalidos++;
            }

            if ($("#notaCurso").val().trim() == "") {
                countInvalidos++;
            }

            if ($("input.validar").val().trim() == "" || $("textarea").val().trim() == "") {
                countInvalidos++;
            }

            if ($('#selectClasificacionCurso').val() != "3" && $('#selectClasificacionCurso').val() != "4") {
                if (checkboxTodosPuestos[0].checked == false && getObjPuestos().length == 0) {
                    countInvalidos++;
                }
            }

            return countInvalidos;
        }

        function getObjCurso() {

            const capacitacionCurso = {
                clasificacion: $('#selectClasificacionCurso').val(),
                claveCurso: $("#claveCurso").val().trim(),
                nombre: $("#nombreCurso").val().trim(),
                duracion: $('#duracionCurso').val(),
                objetivo: $('#objetivoCurso').val().trim(),
                temasPrincipales: $('#temaCurso').val().trim(),
                referenciasNormativas: $('#referenciasNormativas').val().trim(),
                esGeneral: $('#selectClasificacionCurso').val() == "3" || $('#selectClasificacionCurso').val() == "4" ? true : false,
                nota: $('#notaCurso').val().trim(),
                capacitacionUnica: checkboxCapacitacionUnica[0].checked,
                isActivo: true,
                fechaCreacion: hoy,
                aplicaTodosPuestos: checkboxTodosPuestos[0].checked,
                Puestos: getObjPuestos(),
                PuestosAutorizacion: getObjPuestosAutorizacion(),
                Mandos: getObjMandos(),
                CentrosCosto: getObjCentrosCosto()
            }
            return capacitacionCurso;
        }

        function getObjPuestos() {

            let puestos = [];

            getValoresMultiples('#selectPuestoCurso').forEach(currentValue => {
                puestos.push({
                    puesto_id: currentValue,
                    estatus: true
                })
            });

            return puestos;

        }

        function getObjPuestosAutorizacion() {

            let puestos = [];

            getValoresMultiples('#selectPuestoAutorizacionCurso').forEach(currentValue => {
                puestos.push({
                    puesto_id: currentValue,
                    estatus: true
                })
            });

            return puestos;

        }

        function getObjMandos() {
            let mandos = [];

            getValoresMultiples('#selectMando').forEach(currentValue => {
                mandos.push({
                    mando: currentValue,
                    estatus: true
                })
            });

            return mandos;
        }

        function getObjCentrosCosto() {
            let centrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                centrosCosto.push({
                    cc: x.value.split('_')[0],
                    empresa: +(x.empresa),
                    estatus: true
                });
            });

            return centrosCosto;
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }

        function getObjCursoEdit() {

            const capacitacionCurso = {
                id: $("#btnGuardarCurso").val(),
                clasificacion: $('#selectClasificacionCurso').val(),
                claveCurso: $("#claveCurso").val(),
                nombre: $("#nombreCurso").val(),
                duracion: $('#duracionCurso').val(),
                objetivo: $('#objetivoCurso').val(),
                temasPrincipales: $('#temaCurso').val(),
                referenciasNormativas: $('#referenciasNormativas').val(),
                esGeneral: $('#selectClasificacionCurso').val() == "3" || $('#selectClasificacionCurso').val() == "4" ? true : false,
                nota: $('#notaCurso').val(),
                capacitacionUnica: checkboxCapacitacionUnica[0].checked,
                fechaEdicion: hoy,
                aplicaTodosPuestos: checkboxTodosPuestos[0].checked,
                isActivo: true
            }


            return capacitacionCurso
        }

        function getObjPuestosEdit() {
            let puestos = [];

            getValoresMultiples('#selectPuestoCurso').forEach(currentValue => {
                puestos.push({
                    curso_id: $("#btnGuardarCurso").val(),
                    puesto_id: currentValue,
                    estatus: true
                });
            });

            return puestos;
        }

        function getObjPuestosAutorizacionEdit() {
            let puestos = [];

            getValoresMultiples('#selectPuestoAutorizacionCurso').forEach(currentValue => {
                puestos.push({
                    curso_id: $("#btnGuardarCurso").val(),
                    puesto_id: currentValue,
                    estatus: true
                });
            });

            return puestos;
        }

        function getObjMandosEdit() {
            let mandos = [];

            getValoresMultiples('#selectMando').forEach(currentValue => {
                mandos.push({
                    curso_id: $("#btnGuardarCurso").val(),
                    mando: currentValue,
                    estatus: true
                })
            });

            return mandos;
        }

        function getObjCentrosCostoEdit() {
            let centrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                centrosCosto.push({
                    curso_id: $("#btnGuardarCurso").val(),
                    cc: x.value.split('_')[0],
                    empresa: +(x.empresa),
                    estatus: true
                })
            });

            return centrosCosto;
        }

        function getFilesExamenes() {
            let filesExamen = new FormData();

            filesExamen.append("examenes", JSON.stringify(getObjExamenes()));

            $('#tblExamenes tbody tr').each(function (index, value) {
                if ($(this).find('.nuevoArchivo').length > 0) {
                    filesExamen.append($(this).find('.nuevoArchivo').val().split(' ').join('') + 'File', $(this).find('.nuevoArchivo')[0].files[0]);
                }
            })

            return filesExamen;
        }

        function getObjExamenes() {
            let examenes = [];

            $('#tblExamenes tbody tr').each(function () {
                if ($(this).find('.nuevoArchivo').length > 0) {
                    examenes.push({
                        curso_id: $("#btnGuardarExamenes").val(),
                        // nombreExamen: $(this).find('.nombreExamen').val(),
                        //pathExamen: $("#btnGuardarExamenes").attr('data-nombre') + "\\" + $(this).find('.nombreExamen').val(),
                        isActivo: true
                    })
                }
            })

            return examenes
        }

        function guardarCurso() {
            if (validarGuardarCurso() > 0) {
                AlertaGeneral('Aviso', 'Debe ingresar todos los datos')
            } else {
                if (esEdit) {
                    ActualizarCurso(getObjCursoEdit(), getObjMandosEdit(), getObjPuestosEdit(), getObjPuestosAutorizacionEdit(), getObjCentrosCostoEdit()).done(function (response) {
                        if (response.success) {
                            AlertaGeneral('Aviso', 'Se actualizó con exito')
                            $('#modalRegistroCurso').modal('hide');
                            buscarCursos();
                            esEdit = false;
                        } else {
                            AlertaGeneral('Aviso', response.error)
                        }
                    })
                } else {
                    GuardarCurso(getObjCurso()).done(function (response) {
                        if (response.success) {
                            AlertaGeneral('Aviso', 'Se guardo con exito')
                            $('#modalRegistroCurso').modal('hide');
                            buscarCursos();
                            esEdit = false;
                        } else {
                            AlertaGeneral('Aviso', response.error)
                        }
                    })
                }
            }
        }

        function guardarExamen() {
            const data = getFilesExamenes();

            $.ajax({
                url: 'GuardarExamenes',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral('Aviso', 'Se ha cargado la información de los archivos');
                        $('#modalAltaExamen').modal('hide');
                        buscarCursos();
                    } else {
                        AlertaGeneral('Aviso', response.error);
                    }
                }
            });
        }
        //#endregion 

        //#region EVENT'S CHANGE, CLICKS
        $('.select2').select2();

        $('.select2Multi').select2();

        $('#tblCursos').on("mouseenter", "tbody td", function () {

            paintCells($(this).parent());
        });

        $('#tblCursos').on("mouseleave", "tbody td", function () {
            unpaintCells($(this).parent());
        });

        $(`#selectClasificacionCurso`).change(function () {
            if (this.value == 3 || this.value == 4) {

                $('#selectPuestoCurso').val(null).select2().trigger('change'); // limpiarMultiselect("#selectPuestoCurso");
                // $('#modalRegistroCurso button.multiselect.dropdown-toggle.btn.btn-default').attr('disabled', true);


                checkboxTodosPuestos[0].checked = true;
                // checkboxTodosPuestos.attr('disabled', true);

            } else {
                if (checkboxTodosPuestos[0].checked) {

                    $('#selectPuestoCurso').val(null).select2().trigger('change'); // limpiarMultiselect("#selectPuestoCurso");
                    // $('#modalRegistroCurso button.multiselect.dropdown-toggle.btn.btn-default').attr('disabled', true);

                } else {
                    $('#modalRegistroCurso button.multiselect.dropdown-toggle.btn.btn-default').attr('disabled', false);
                }
                checkboxTodosPuestos.attr('disabled', false);
            }
        });

        $(document).on('click', '.descargarArchivo', function () {
            location.href = `DescargarArchivo?examen_id=${$(this).attr('data-id')} `;
        })

        $('#searchbtn').keyup(function () {
            tblCursos.search($(this).val()).draw();
        })

        $('#btnAgregarExamen').on('click', function () {

            $('#tblExamenes tbody').append(setNewRowTableExamenes());

            // getTipoExamen(this.value).done(function (response) {
            //     if (response.success) {

            //     }
            // })

        })

        $(document).on('click', '.eliminarArchivo', function () {
            if ($(this).attr('data-id') > 0) {
                const element = $(this)

                EliminarExamen($(this).attr('data-id')).done(function (response) {
                    if (response.success) {
                        element.parent().parent().remove();
                        buscarCursos();
                        AlertaGeneral(`Éxito`, `Examen eliminado correctamente.`);
                    } else {
                        // alert(response.error)
                        AlertaGeneral(`Error`, `Ocurrió un error al intentar eliminar el examen.`);
                    }
                })
            } else {
                $(this).parent().parent().remove()
            }
        });

        //#endregion

        function guardarCargaMasiva() {
            var request = new XMLHttpRequest();

            request.open("POST", "/Administrativo/Capacitacion/GuardarCargaMasivaRelacionCursosPuestosAutorizacion");
            request.send(formDataCargaMasiva());

            request.onload = function (response) {
                if (request.status == 200) {
                    let respuesta = JSON.parse(request.response);

                    modalCargaMasiva.modal('hide');
                    $('#inputFile').val('');

                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, `${respuesta.message}`);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            };
        }

        function formDataCargaMasiva() {
            let formData = new FormData();

            $.each(document.getElementById("inputFile").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
    };

    $(document).ready(function () {
        administrativo.seguridad.CapturaCursos = new CapturaCursos();
    });
})();

