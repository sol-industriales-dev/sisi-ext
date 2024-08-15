(() => {
    $.namespace('CH.CatTabuladores');

    //#region CONSTS
    const cboFiltroCC = $('#cboFiltroCC');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const btnFiltroExportar = $('#btnFiltroExportar')
    const tblTabuladores = $('#tblTabuladores');
    let dtTabuladores;
    //#endregion

    //#region CE TABULADOR
    const mdlCETabulador = $('#mdlCETabulador');
    const cboCETabPuesto = $('#cboCETabPuesto');
    const txtCETabSueldoBase = $('#txtCETabSueldoBase');
    const txtCETabComplemento = $('#txtCETabComplemento');
    const txtCETabBono = $('#txtCETabBono');
    const btnCETabulador = $('#btnCETabulador');
    //#endregion

    CatTabuladores = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            initTblTabuladores();
            $(".ui .button .buttons-excel .buttons-html5").css("display: none")

            cboFiltroCC.fillCombo('FillCboCC', {}, false);
            cboCETabPuesto.fillCombo("GetAllPuestos", {}, false);

            cboFiltroCC.select2({ width: '100%' });
            cboCETabPuesto.select2({ width: '100%' });

            cboFiltroCC.on("change", function () {
                fncGetPuestosTabuladores();
            });

            btnFiltroBuscar.on("click", function () {
                fncGetPuestosTabuladores();
            });

            btnFiltroNuevo.on("click", function () {
                mdlCETabulador.modal("show");
                btnCETabulador.data("id", 0);
                btnCETabulador.text("Añadir");

                fncDefaultModalTab();
                fncDefaultBorder();
            });

            btnCETabulador.on("click", function () {
                fncCrearEditarTabuladoresPuesto();
            });

            txtCETabSueldoBase.on("change", function () {
                $(this).val(maskNumero(unmaskNumero($(this).val())));
            });

            txtCETabComplemento.on("change", function () {
                $(this).val(maskNumero(unmaskNumero($(this).val())));
            });

            txtCETabBono.on("change", function () {
                $(this).val(maskNumero(unmaskNumero($(this).val())));
            });

            btnFiltroExportar.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea generar el reporte?', 'Confirmar', 'Cancelar', () => fncGenerarReporteTabuladores());
            })

            //#region KEY UPS
            cboCETabPuesto.on("change", function () {
                if ($(this).val() == "") { $("#select2-cboCETabPuesto-container").css('border', '2px solid red'); } else { $("#select2-cboCETabPuesto-container").css('border', '1px solid #CCC'); }
            });

            txtCETabSueldoBase.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }

            });

            txtCETabComplemento.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }

            });

            txtCETabBono.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }

            });
            //#endregion
        }

        function initTblTabuladores() {
            dtTabuladores = tblTabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                "bLengthChange": false,
                "autoWidth": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            // columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    { data: "tabulador", title: "TABULADOR", },
                    { data: "puesto_desc", title: "PUESTO", },
                    {
                        data: "salario_base", title: "SALARIO BASE",
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: "complemento", title: "COMPLEMENTO",
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: "bono_de_zona", title: "BONO EN SISTEMA",
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    { data: "year", title: "AÑO", },
                    {
                        render: (data, type, row, meta) => {
                            return `<button title='Actualizar datos del tabulador.' class="btn btn-warning actualizarTabulador btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblTabuladores.on('click', '.actualizarTabulador', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();

                        cboCETabPuesto.val(rowData.puesto);
                        cboCETabPuesto.trigger("change");
                        cboCETabPuesto.prop("disabled", true);


                        txtCETabSueldoBase.val(maskNumero(rowData.salario_base));
                        txtCETabComplemento.val(maskNumero(rowData.complemento));
                        txtCETabBono.val(maskNumero(rowData.bono_de_zona));

                        mdlCETabulador.modal("show");
                        btnCETabulador.data("id", rowData.id);
                        btnCETabulador.text("Actualizar");
                        fncDefaultBorder();
                    });
                    tblTabuladores.on('click', '.classBtn', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetPuestosTabuladores() {
            let obj = {
                cc: cboFiltroCC.val(),
            }
            axios.post("GetPuestosTabuladores", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(items);
                    dtTabuladores.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarTabuladoresPuesto() {
            let obj = fncGetObjCETabuladoresPuesto();
            axios.post("CrearEditarTabuladorPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetPuestosTabuladores();

                    if (obj.id > 0) {
                        Alert2Exito("Tabulador actualizado con exito");

                    } else {
                        Alert2Exito("Tabuador creado con exito");

                    }

                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetObjCETabuladoresPuesto() {
            let strMensajeError = "";

            if (cboCETabPuesto.val() == "") { $("#select2-cboCETabPuesto-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            // if (txtCETabSueldoBase.val() == "") { txtCETabSueldoBase.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (txtCETabComplemento.val() == "") { txtCETabComplemento.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (txtCETabBono.val() == "") { txtCETabBono.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    objTabulador: {
                        id: btnCETabulador.data("id"),
                        // tabulador: ,
                        puesto: cboCETabPuesto.val(),
                        salario_base: unmaskNumero(txtCETabSueldoBase.val()),
                        complemento: unmaskNumero(txtCETabComplemento.val()),
                        bono_de_zona: unmaskNumero(txtCETabBono.val()),
                    }

                }
                return obj;
            }
        }

        function fncDefaultModalTab() {
            cboCETabPuesto.val("");
            cboCETabPuesto.trigger("change");
            cboCETabPuesto.prop("disabled", false);

            txtCETabSueldoBase.val("");
            txtCETabComplemento.val("");
            txtCETabBono.val("");
        }

        function fncDefaultBorder() {
            $("#select2-cboCETabPuesto-container").css('border', '1px solid #CCC');
            txtCETabSueldoBase.css('border', '1px solid #CCC');
            txtCETabComplemento.css('border', '1px solid #CCC');
            txtCETabBono.css('border', '1px solid #CCC');
        }

        function fncGenerarReporteTabuladores() {
            if (cboFiltroCC.val() > 0) {
                var path = `/Reportes/Vista.aspx?idReporte=280&cc=${cboFiltroCC.val()}`;
                $("#report").attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            } else {
                Alert2Warning("Es necesario seleccionar un CC.")
            }
        }
    }

    $(document).ready(() => {
        CH.CatTabuladores = new CatTabuladores();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();