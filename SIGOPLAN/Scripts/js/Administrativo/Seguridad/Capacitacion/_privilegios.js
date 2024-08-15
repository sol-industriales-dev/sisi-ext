(() => {
    $.namespace('Administrativo.Seguridad.Capacitacion._privilegios');
    _privilegios = function () {
        let itemPrivilegios;
        const btnPrivBuscar = $('#btnPrivBuscar');
        const selPrivCcCplan = $('#selPrivCcCplan');
        const btnPrivGuardar = $('#btnPrivGuardar');
        const tblPrivilegios = $('#tblPrivilegios');
        const selPrivPrivilegio = $('#selPrivPrivilegio');
        const getCboCC = new URL(window.location.origin + '/Administrativo/Capacitacion/ObtenerComboCC');
        const getCboPrivilegios = new URL(window.location.origin + '/Administrativo/Capacitacion/getCboPrivilegios');
        const guardarEmpleadosPrivilegios = new URL(window.location.origin + '/Administrativo/Capacitacion/guardarEmpleadosPrivilegios');
        const ObtenerEmpleadosPrivilegios = new URL(window.location.origin + '/Administrativo/Capacitacion/ObtenerEmpleadosPrivilegios');

        let init = () => {
            initForm();
            btnPrivBuscar.click(setEmpleadosPrivilegios);
            btnPrivGuardar.click(setGuardarEmpleadosPrivilegios);
        }

        async function setGuardarEmpleadosPrivilegios() {
            try {
                let lst = getLstPrivilegio();
                if (esLstPrivilegioValido(lst)) {
                    response = await ejectFetchJson(guardarEmpleadosPrivilegios, lst);
                    if (response.success) {
                        AlertaGeneral("Aviso", `Privilegios actualizados con éxito.`)
                    }
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }

        async function setEmpleadosPrivilegios() {
            try {
                let busq = getFormBusq();
                dtPrivilegios.clear().draw();
                if (esFormBusqValido(busq)) {
                    response = await ejectFetchJson(ObtenerEmpleadosPrivilegios, busq);
                    if (response.success) {
                        dtPrivilegios.rows.add(response.lst).draw();
                    }
                } else {
                    AlertaGeneral(`Aviso`, `Debe seleccionar un centro de costo y un tipo de privilegio.`);
                    return;
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message); }
        }
        async function setItemsPrivilegios() {
            try {
                response = await ejectFetchJson(getCboPrivilegios);
                if (response.success) {
                    itemPrivilegios = response.items;
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }
        function getLstPrivilegio() {
            let lst = [];
            dtPrivilegios.rows().every(function () {
                let node = this.node()
                    , idPrivilegio = +$(node).find(`.privilegio`).val();
                let data = this.data();
                data.idPrivilegio = idPrivilegio;
                lst.push(data);
            })
            return lst;
        }
        function esLstPrivilegioValido(lst) {
            let esValido = true;
            lst.forEach(user => {
                switch (true) {
                    case user.idUsuario == 0:
                        esValido = false;
                        break;
                    default:
                        break;
                }
            });
            return esValido;
        }
        function getFormBusq() {
            let lstCc = getValoresMultiples('#selPrivCcCplan');
            const privilegios = getValoresMultiples('#selPrivPrivilegio');
            return { lstCc, privilegios };
        }
        function esFormBusqValido({ lstCc, privilegios }) {
            return lstCc.length > 0 && privilegios.length > 0;
        }
        function initForm() {
            let todos = 'Todos';
            setItemsPrivilegios();
            selPrivPrivilegio.fillCombo(getCboPrivilegios, null, false, todos);

            // selPrivCcCplan.fillCombo(getCboCC, null, false, todos);

            $.get('/Administrativo/Capacitacion/ObtenerComboCC')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        selPrivCcCplan.append(`<option value="Todos">Todos</option>`);
                        selPrivCcCplan.append(response.items.map(item => `<option value=${item.Value} >${item.Text}</option>`).join(''));
                        convertToMultiselect(`#selPrivCcCplan`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

            convertToMultiselect(`#selPrivPrivilegio`);
            initDataTblPrivilegios();
        }
        function initDataTblPrivilegios() {
            dtPrivilegios = tblPrivilegios.DataTable({
                destroy: true,
                order: [[2, "asc"]],
                language: dtDicEsp,
                columns: [
                    { data: 'nombre', title: 'Nombre' }
                    , { data: 'ccDesc', title: 'Centro Costos' }
                    , {
                        data: 'idPrivilegio', title: 'Privilegio', createdCell: function (td, data, rowData, row, col) {
                            let cbo = $(`<select>`)
                            cbo.addClass(`form-control privilegio`);
                            cbo.fillComboItems(itemPrivilegios, true, data);
                            $(td).html(cbo);
                        }
                    }
                ]
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Seguridad.Capacitacion._privilegios = new _privilegios();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();