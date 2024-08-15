(() => {
    $.namespace('Adminstrativo.Seguridad.CapacitacionSeguridad.AutorizadosPersonalAutorizado');
    AutorizadosPersonalAutorizado = function () {
        //#region Selectores
        const botonEnviarCorreos = $('#botonEnviarCorreos');
        const botonImprimirLista = $('#botonImprimirLista');
        const inputFecha = $('#inputFecha');
        const selectClaveListaAutorizacion = $('#selectClaveListaAutorizacion');
        const selectCurso = $('#selectCurso');
        const inputRevision = $('#inputRevision');
        const selectRazonSocial = $('#selectRazonSocial');
        const inputRFC = $('#inputRFC');
        const selectDepartamento = $('#selectDepartamento');
        const inputObjetivo = $('#inputObjetivo');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const botonAgregarAsistente = $('#botonAgregarAsistente');
        const tablaAsistentes = $('#tablaAsistentes');
        const inputNota = $('#inputNota');
        const inputJefeDepartamento = $('#inputJefeDepartamento');
        const inputCoordinadorCSH = $('#inputCoordinadorCSH');
        const inputGerenteProyecto = $('#inputGerenteProyecto');
        const inputSecretarioCSH = $('#inputSecretarioCSH');
        const inputSeguridad = $('#inputSeguridad');
        const inputInteresados = $('#inputInteresados');
        const botonGuardar = $('#botonGuardar');
        const divInteresados = $('#divInteresados');
        const modalEnviarCorreos = $('#modalEnviarCorreos');
        const textAreaCorreos = $('#textAreaCorreos');
        const botonEnviarCorreosConfirmar = $('#botonEnviarCorreosConfirmar');
        const report = $("#report");
        //#endregion

        let dtAsistentes;

        (function init() {
            $('.select2').select2();

            initTablaAsistentes();

            inputFecha.val(getFechaCeros());

            botonAgregarAsistente.click(agregarAsistente);
            botonGuardar.click(guardarInformacion);

            inputClaveEmpleado.getAutocompleteValid(setDatosEmpleado, verificarEmpleado, { porClave: true }, '/Administrativo/CapacitacionSeguridad/GetEmpleadoEnKontrolAutocomplete');
            inputNombreEmpleado.getAutocompleteValid(setDatosEmpleado, verificarEmpleado, { porClave: false }, '/Administrativo/CapacitacionSeguridad/GetEmpleadoEnKontrolAutocomplete');

            inputJefeDepartamento.getAutocompleteValid(setDatosAutorizante, verificarAutorizante, null, 'GetAutorizanteEnkontrolAutocomplete');
            inputGerenteProyecto.getAutocompleteValid(setDatosAutorizante, verificarAutorizante, null, 'GetAutorizanteEnkontrolAutocomplete');
            inputCoordinadorCSH.getAutocompleteValid(setDatosAutorizante, verificarAutorizante, null, 'GetAutorizanteEnkontrolAutocomplete');
            inputSecretarioCSH.getAutocompleteValid(setDatosAutorizante, verificarAutorizante, null, 'GetAutorizanteEnkontrolAutocomplete');
            inputSeguridad.getAutocompleteValid(setDatosAutorizante, verificarAutorizante, null, 'GetAutorizanteEnkontrolAutocomplete');

            selectClaveListaAutorizacion.fillCombo('GetListasAutorizacionCombo', null, false, null);
            selectClaveListaAutorizacion.change(cargarListaAutorizacion);

            // axios.get('GetDepartamentosCombo')
            //     .then(response => {
            //         let { success, items, message } = response.data;

            //         if (success) {
            //             selectDepartamento.append('<option value="">--Seleccione--</option>');

            //             items.forEach(x => {
            //                 let groupOption = `<optgroup label="${x.label}"></optgroup>`;
            //                 x.options.forEach(y => {
            //                     groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
            //                 });
            //                 selectDepartamento.append(groupOption);
            //             });
            //         } else {
            //             AlertaGeneral(`Alerta`, message);
            //         }
            //     }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectCurso.fillCombo('ObtenerComboCursos', null, false, null);

            inputInteresados.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'GetAutorizanteEnkontrolAutocomplete',
                        dataType: 'json',
                        data: { term: request.term },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: interesadoSeleccionado
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");

                return $("<li>").data("item.autocomplete", item).append("<div>" + t + "</div>").appendTo(ul);
            };

            inputInteresados.click(() => {
                inputInteresados.focus();
                inputInteresados.autocomplete('search');
            });

            divInteresados.on('click', '.userDelete', (e) => { $(e.currentTarget).closest('.divUser').remove(); });
            botonImprimirLista.click(imprimirLista);
            botonEnviarCorreos.click(enviarCorreos);
            botonEnviarCorreosConfirmar.click(confirmarEnviarCorreos);
        })();

        selectCurso.on('change', function () {
            let cursoID = selectCurso.val();

            if (cursoID == '') {
                inputNota.val('');
                inputObjetivo.val('');
            } else {
                axios.get('GetCursoById', { params: { id: cursoID } })
                    .then(response => {
                        let { success, informacion, message } = response.data;

                        if (success) {
                            inputNota.val(informacion.nota);
                            inputObjetivo.val(informacion.objetivo);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        selectRazonSocial.on('change', function () {
            if (selectRazonSocial.val() != '') {
                let rfc = selectRazonSocial.find('option:selected').attr('rfc');

                inputRFC.val(rfc);
            } else {
                inputRFC.val('');
            }
        });

        function initTablaAsistentes() {
            dtAsistentes = tablaAsistentes.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                searching: false,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tablaAsistentes.on('click', '.botonQuitar', function () {
                        let row = $(this).closest('tr');

                        dtAsistentes.row(row).remove().draw();

                        let cuerpo = tablaAsistentes.find('tbody');

                        if (cuerpo.find("tr").length == 0) {
                            dtAsistentes.draw();
                        } else {
                            tablaAsistentes.find('tbody tr').each(function (idx, row) {
                                let rowData = dtAsistentes.row(row).data();

                                if (rowData != undefined) {
                                    dtAsistentes.row(row).data(rowData).draw();
                                }
                            });
                        }
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: '# Empleado' },
                    { data: 'nombreEmpleado', title: 'Nombre' },
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-danger btn-sm botonQuitar"><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function agregarAsistente() {

            if (inputNombreEmpleado.val() == '') {
                return;
            }

            let inputData = inputNombreEmpleado.data().uiAutocomplete;

            if (inputData == null) {
                return;
            }

            let empleadoData = inputData.selectedItem;

            if (empleadoData == null) {
                empleadoData = inputClaveEmpleado.data().uiAutocomplete.selectedItem;
                if (empleadoData == null) {
                    return;
                }
            }

            const { id, nombreEmpleado, cc, ccID, puestoEmpleado, } = empleadoData;

            if (tablaAsistentes.find('tbody tr').toArray().some(x => $(x).find('p').text() == id)) {
                return;
            }

            agregarRowAsistente(id, nombreEmpleado, puestoEmpleado, ccID, `${ccID} - ${cc}`);
            inputNombreEmpleado.val('');
            inputClaveEmpleado.val('');
            inputNombreEmpleado.focus();
        }

        function agregarRowAsistente(claveEmpleado, nombreEmpleado, puestoDesc, cc, ccDesc) {
            dtAsistentes.row.add({
                claveEmpleado,
                nombreEmpleado,
                puestoDesc,
                cc,
                ccDesc
            }).draw();
        }

        function setDatosEmpleado(e, ui) {
            inputClaveEmpleado.val(ui.item.id);
            inputNombreEmpleado.val(ui.item.nombreEmpleado);
        }

        function verificarEmpleado(e, ui) {
            if (ui.item == null) {
                inputClaveEmpleado.val('');
                inputNombreEmpleado.val('')
            }
        }

        function setDatosAutorizante(e, ui) {
            let input = $(e.target);

            input.data().id = ui.item.id;
            input.val(ui.item.nombreAutorizante);
        }

        function verificarAutorizante(e, ui) {
            if (ui.item == null) {
                let input = $(e.target);

                input.data().id = 0;
                input.val('');
            }
        }

        function cargarListaAutorizacion() {
            let listaAutorizacionID = selectClaveListaAutorizacion.val();

            if (listaAutorizacionID == '') {
                limpiarPantalla();
            } else {
                axios.get('GetListaAutorizacionByID', { params: { listaAutorizacionID } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            llenarPantalla(datos);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function limpiarPantalla() {
            selectClaveListaAutorizacion.val('');
            selectCurso.val('');
            selectCurso.change();
            inputRevision.val('');
            selectRazonSocial.val('');
            selectRazonSocial.empty();
            inputRFC.val('');
            selectDepartamento.val('');
            selectDepartamento.empty();
            inputObjetivo.val('');
            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');

            dtAsistentes.clear().draw();

            inputNota.val('');
            inputJefeDepartamento.data().id = 0;
            inputJefeDepartamento.val('');
            inputCoordinadorCSH.data().id = 0;
            inputCoordinadorCSH.val('');
            inputGerenteProyecto.data().id = 0;
            inputGerenteProyecto.val('');
            inputSecretarioCSH.data().id = 0;
            inputSecretarioCSH.val('');
            inputSeguridad.data().id = 0;
            inputSeguridad.val('');

            divInteresados.find('.divUser').remove();
            inputInteresados.val('');
        }

        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        function llenarPantalla(datos) {
            selectCurso.val(datos.cursoID);
            selectCurso.change();

            inputRevision.val(datos.revision);

            const optionSeleccione = `<option value="">--Seleccione--</option>`;

            //#region Razón Social y RFC
            selectRazonSocial.append(optionSeleccione);

            datos.listaRFC.forEach(x => {
                selectRazonSocial.append(`<option value="${x.id}" rfc="${x.rfc}">${x.razonSocial}</option>`);
            });

            //Se coloca el primero por default.
            selectRazonSocial.val(datos.listaRFC[0].id);
            inputRFC.val(datos.listaRFC[0].rfc);
            //#endregion

            //#region Departamento
            selectDepartamento.append(optionSeleccione);

            let departamentosConstruplan = datos.listaCC.filter((x) => x.empresa == 1);
            let departamentosArrendadora = datos.listaCC.filter((x) => x.empresa == 2);

            if (departamentosConstruplan.length > 0) {
                let listaCCUnicos = departamentosConstruplan.map((x) => x.cc).filter(onlyUnique)

                listaCCUnicos.forEach((x) => {
                    let groupOption = `<optgroup label="CONSTRUPLAN - ${x}"></optgroup>`;
                    let listaDepartamentosFiltrados = departamentosConstruplan.filter((y) => y.cc == x);

                    listaDepartamentosFiltrados.forEach(y => {
                        groupOption += `<option value="${y.departamento}" cc="${x}" empresa="${y.empresa}">${y.departamentoDesc}</option>`;
                    });

                    selectDepartamento.append(groupOption);
                });
            }

            if (departamentosArrendadora.length > 0) {
                let listaCCUnicos = departamentosArrendadora.map((x) => x.cc).filter(onlyUnique)

                listaCCUnicos.forEach((x) => {
                    let groupOption = `<optgroup label="ARRENDADORA - ${x}"></optgroup>`;
                    let listaDepartamentosFiltrados = departamentosArrendadora.filter((y) => y.cc == x);

                    listaDepartamentosFiltrados.forEach(y => {
                        groupOption += `<option value="${y.departamento}" cc="${x}" empresa="${y.empresa}">${y.departamentoDesc}</option>`;
                    });

                    selectDepartamento.append(groupOption);
                });
            }

            //Se coloca el primero por default.
            selectDepartamento.val(datos.listaCC[0].departamento);
            //#endregion

            inputClaveEmpleado.val('');
            inputNombreEmpleado.val('');

            AddRows(tablaAsistentes, datos.listaAsistentes);

            inputJefeDepartamento.data().id = datos.jefeDepartamento;
            inputJefeDepartamento.val(datos.jefeDepartamentoDesc);
            inputCoordinadorCSH.data().id = datos.coordinadorCSH;
            inputCoordinadorCSH.val(datos.coordinadorCSHDesc);
            inputGerenteProyecto.data().id = datos.gerenteProyecto;
            inputGerenteProyecto.val(datos.gerenteProyectoDesc);
            inputSecretarioCSH.data().id = datos.secretarioCSH;
            inputSecretarioCSH.val(datos.secretarioCSHDesc);
            inputSeguridad.data().id = datos.seguridad;
            inputSeguridad.val(datos.seguridadDesc);

            $(datos.listaInteresados).each(function (index, element) {
                $(`
                    <div class="divUser">
                        <div class="userContainer">
                            <span class="userFill">&nbsp;</span>
                            <span class="userComponent" data-user="${element.nombreEmpleado}" data-userid="${element.claveEmpleado}">${element.nombreEmpleado}</span>
                            <button type="button" class="userDelete">&nbsp;X</button>
                        </div>
                    </div>
                `).insertBefore(inputInteresados);
            });
        }

        function guardarInformacion() {
            let listaAutorizacion = getInformacionAutorizados();

            if (listaAutorizacion.id == '') {
                AlertaGeneral(`Alerta`, `Debe seleccionar una lista de autorización.`);
            } else {
                axios.post('GuardarInformacionAutorizados', { listaAutorizacion })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            limpiarPantalla();
                            selectClaveListaAutorizacion.val('');
                            selectClaveListaAutorizacion.change();
                            AlertaGeneral(`Alerta`, `Se guardó la información.`);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function getInformacionAutorizados() {
            let listaAsistentes = [];

            tablaAsistentes.find('tbody tr').each(function (index, row) {
                let rowData = dtAsistentes.row(row).data();

                listaAsistentes.push({
                    claveEmpleado: rowData.claveEmpleado
                });
            });

            let listaInteresados = [];

            divInteresados.find('.userComponent').each(function (index, element) {
                let claveEmpleado = +$(element).attr('data-userid');

                listaInteresados.push({ claveEmpleado });
            });

            let datos = {
                id: selectClaveListaAutorizacion.val(),
                jefeDepartamento: inputJefeDepartamento.data().id,
                gerenteProyecto: inputGerenteProyecto.data().id,
                coordinadorCSH: inputCoordinadorCSH.data().id,
                secretarioCSH: inputSecretarioCSH.data().id,
                seguridad: inputSeguridad.data().id,
                listaAsistentes: listaAsistentes,
                listaInteresados: listaInteresados
            };

            return datos;
        }

        function interesadoSeleccionado(event, ui) {
            let html = `
                <div class="divUser">
                    <div class="userContainer">
                        <span class="userFill">&nbsp;</span>
                        <span class="userComponent" data-user="${ui.item.value}" data-userid="${ui.item.id}">${ui.item.value}</span>
                        <button type="button" class="userDelete">&nbsp;X</button>
                    </div>
                </div>`;

            $(html).insertBefore(inputInteresados);
            inputInteresados.focus();
            ui.item.value = "";  // it will clear field 
            inputInteresados.autocomplete('close').val('');

            return false;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function imprimirLista() {
            if (selectRazonSocial.val() == '' || selectDepartamento.val() == '') {
                AlertaGeneral(`Alerta`, `Debe seleccionar una razón social y un departamento para imprimir la lista de autorización.`);
                return;
            }

            let listaAutorizacionID = +selectClaveListaAutorizacion.val();

            if (listaAutorizacionID == 0) {
                AlertaGeneral(`Alerta`, `Debe seleccionar una lista de autorización válida.`);
                return;
            }

            let razonSocialID = +selectRazonSocial.val();
            let departamento = +selectDepartamento.val();
            let cc = selectDepartamento.find('option:selected').attr('cc');
            let empresa = selectDepartamento.find('option:selected').attr('empresa');

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=207&listaAutorizacionID=${listaAutorizacionID}&razonSocialID=${razonSocialID}&departamento=${departamento}&cc=${cc}&empresa=${empresa}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function enviarCorreos() {
            if (selectRazonSocial.val() == '' || selectDepartamento.val() == '') {
                AlertaGeneral(`Alerta`, `Debe seleccionar una razón social y un departamento para enviar el correo.`);
                return;
            }

            let listaAutorizacionID = +selectClaveListaAutorizacion.val();

            if (listaAutorizacionID == 0) {
                AlertaGeneral(`Alerta`, `Debe seleccionar una lista de autorización.`);
                botonEnviarCorreosConfirmar.data().listaAutorizacionID = 0;
                return;
            }

            botonEnviarCorreosConfirmar.data().listaAutorizacionID = listaAutorizacionID;

            axios.get('GetCorreosListaAutorizacion', { params: { listaAutorizacionID } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        textAreaCorreos.val(response.data.correos);
                        modalEnviarCorreos.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function confirmarEnviarCorreos() {
            let listaAutorizacionID = botonEnviarCorreosConfirmar.data().listaAutorizacionID;

            if (listaAutorizacionID == 0) {
                AlertaGeneral(`Alerta`, `Error al enviar el correo.`);
                return;
            }

            let razonSocialID = +selectRazonSocial.val();
            let departamento = +selectDepartamento.val();
            let cc = selectDepartamento.find('option:selected').attr('cc');
            let empresa = selectDepartamento.find('option:selected').attr('empresa');
            let listaCorreos = textAreaCorreos.val().split(';').map((x) => { return x.trim(); });

            if (listaCorreos.length == 0) {
                AlertaGeneral(`Alerta`, `No se ha capturado un correo válido.`);
                return;
            }

            axios.post('SetCorreosListaAutorizacion', listaCorreos)
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $.ajax({
                            datatype: "json",
                            type: "POST",

                            url: `/Reportes/Vista.aspx?inMemory=1&idReporte=208&listaAutorizacionID=${listaAutorizacionID}&razonSocialID=${razonSocialID}&departamento=${departamento}&cc=${cc}&empresa=${empresa}`,
                            success: function (response) {
                                if (response != null) {
                                    AlertaGeneral("Confirmación", "Correos enviados correctamente");
                                    $.unblockUI();
                                }
                                else {
                                    AlertaGeneral("Alerta", "¡Ocurrio un problema al convertir la minuta a PDF para ser enviada!");
                                    $.unblockUI();
                                }
                            },
                            error: function () {
                                $.unblockUI();
                            },

                            error: function () {
                                $.unblockUI();
                            }
                        });
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            // $.LoadInMemoryThenSaveCustom(
            //     'SetCorreosListaAutorizacion', , listaCorreos, null, 5, null
            // );
        }

        $.LoadInMemoryThenSaveCustom = async function (urlSession, urlSave, myArray, scheme, chunk_size) {
            setBarProgress(0);
            var response = {
                success: new Array(),
                error: new Array(),
                status: 0,
                errorJson: ""
            };
            var count = 0;
            let total = myArray.length;
            let acum = 0;
            if (scheme == null || scheme == undefined) {
                scheme = {};
            }
            while (myArray.length) {
                let porcentaje = (100 * acum / total).toFixed(2);
                setBarProgress(porcentaje);
                var lstSplited = myArray.splice(0, chunk_size);
                // scheme.lst = new Array();
                // scheme.lst = lstSplited;
                acum += lstSplited.length;
                var obj = {};
                obj.loop = count++;
                obj.success = true;
                await axios.post(urlSession, lstSplited)
                    .then(responseSession => {
                        let { success } = responseSession.data;
                        if (!success) {
                            obj.success = success;
                        }
                    }).catch(o_O => {
                        $.unblockUI();
                        obj.success = false;
                        AlertaGeneral(o_O.message);
                    });
                obj.dataRequest = scheme;
                if (obj.success) {
                    response.success.push(obj);
                }
                else {
                    response.error.push(obj);
                }
            }
            response.errorJson = JSON.stringify(response.error);
            var success = response.success.length;
            var error = response.error.length;
            switch (true) {
                case success > 0 && error == 0:
                    await axios.post(urlSave, scheme)
                        .then(responseSave => {
                            if (responseSave.status == 200) {
                                response.status = 1;
                            }
                        }).catch(o_O => {
                            $.unblockUI();
                            AlertaGeneral(o_O.message);
                        });
                    break;
                case success > 0 && error > 0:
                    response.status = 2;
                    break; 6
                default:
                    response.status = 3;
                    break;
            }
            $.unblockUI();
            if (response.status == 1) {
                AlertaGeneral(`Alerta`, `Se envió el correo.`);
                modalEnviarCorreos.modal('hide');
            }
            // _sp_responseMethod(response, fnResponse);
            return response;
        };
    }
    $(document).ready(() => Adminstrativo.Seguridad.CapacitacionSeguridad.AutorizadosPersonalAutorizado = new AutorizadosPersonalAutorizado())
})();