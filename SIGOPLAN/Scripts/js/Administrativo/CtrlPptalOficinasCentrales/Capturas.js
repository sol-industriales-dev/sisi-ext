(() => {
    $.namespace('CtrlPptalOfCE.Capturas');

    //#region CONST
    const btnCrearComentario = $('#btnCrearComentario');
    const txtComentarios = $('#txtComentarios');
    const ulComentarios = $('#ulComentarios');
    const divVerComentario = $('#divVerComentario');
    const mdlVerActividad = $('#mdlVerActividad');
    const txtVerActividad = $('#txtVerActividad');
    const lblMatch = $('#lblMatch');
    const mdlMatch = $('#mdlMatch');
    const btnFiltroMatch = $('#btnFiltroMatch');
    const lblTitleMatch = $('#lblTitleMatch');
    const lblTitleCatalogo = $("#lblTitleCatalogo");
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroAgrupacion = $("#cboFiltroAgrupacion");
    const cboFiltroAnio = $('#cboFiltroAnio');
    const btnFiltroBuscar = $("#btnFiltroBuscar");
    const btnFiltroNuevaCaptura = $("#btnFiltroNuevaCaptura");
    const tblAF_CtrlPptalOfCe_CapPptos = $("#tblAF_CtrlPptalOfCe_CapPptos");
    const mdlCECaptura = $('#mdlCECaptura');
    const lblTitleCECaptura = $('#lblTitleCECaptura');
    const cboCC = $('#cboCC');
    const cboAgrupacion = $("#cboAgrupacion");
    const cboConcepto = $("#cboConcepto");
    const cboResponsable = $("#cboResponsable");
    const cboAnio = $('#cboAnio');
    const txtImporteEnero = $("#txtImporteEnero");
    const txtImporteFebrero = $("#txtImporteFebrero");
    const txtImporteMarzo = $("#txtImporteMarzo");
    const txtImporteAbril = $("#txtImporteAbril");
    const txtImporteMayo = $("#txtImporteMayo");
    const txtImporteJunio = $("#txtImporteJunio");
    const txtImporteJulio = $("#txtImporteJulio");
    const txtImporteAgosto = $("#txtImporteAgosto");
    const txtImporteSeptiembre = $("#txtImporteSeptiembre");
    const txtImporteOctubre = $("#txtImporteOctubre");
    const txtImporteNoviembre = $("#txtImporteNoviembre");
    const txtImporteDiciembre = $("#txtImporteDiciembre");
    const btnCECaptura = $('#btnCECaptura');
    const lblTitleBtnCECaptura = $('#lblTitleBtnCECaptura');
    const txtInsumo = $('#txtInsumo');
    const txtCuentaDescripcion = $('#txtCuentaDescripcion');
    const txtActividad = $('#txtActividad');
    const btnFiltroNotificar = $('#btnFiltroNotificar');
    let dtCapturas;
    const mdlAditiva = $('#mdlAditiva');
    const btnAgregarAditiva = $('#btnAgregarAditiva');
    const lblTitleAditiva = $('#lblTitleAditiva');
    const tblAditiva = $('#tblAditiva');
    const btnAditivaGuardar = $('#btnAditivaGuardar');
    const divComentarioAditiva = $('#divComentarioAditiva');
    const comentarioAditiva = $('#comentarioAditiva');

    const TipoPresupuestoEnum = {
        original: 0,
        aditivaDeductivaAutorizada: 1,
        total: 2,
        aditivaDeductivaPendiente: 3,
        aditivaDeductivaNueva: 4,
        aditivaDeductivaRechazada: 5
    };
    let pptoNotificado = false;
    let _idConcepto = 0;
    let _CC = "";
    //#endregion

    Capturas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblCapturas();
            initTablaAdivitas();
            //#endregion

            //#region EVENTOS CONTROL IMPACTOS
            btnFiltroNotificar.attr("disabled", "disabled");

            btnFiltroNuevaCaptura.on("click", function () {
                if (cboFiltroAgrupacion.val() == "" || cboFiltroCC.val() == "" || cboFiltroAnio.val() == "") {
                    lblTitleCatalogo.html("");
                    let strMensajeError = "";
                    cboFiltroCC.val() <= 0 ? strMensajeError += "Es necesario indicar un CC." : "";
                    cboFiltroAgrupacion.val() <= 0 ? strMensajeError += "<br>Es necesario indicar una agrupación." : "";
                    cboFiltroAnio.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un año." : "";
                    Alert2Warning(strMensajeError);
                } else {
                    fncLimpiarMdlCECapturas();
                    fncTitleMdlCECaptura(true);
                    cboCC.val(cboFiltroCC.val());
                    cboCC.trigger("change");
                    cboAgrupacion.val(cboFiltroAgrupacion.val());
                    cboAgrupacion.trigger("change");
                    cboAnio.val(cboFiltroAnio.val());
                    cboAnio.trigger("change");

                    if (cboAgrupacion.val() != "") {
                        // cboConcepto.fillCombo("FillConceptos", { idAgrupacion: cboAgrupacion.val() }, false); v1
                        cboConcepto.fillCombo("FillConceptosDeCapturas", { idCC: cboFiltroCC.val(), idAgrupacion: cboAgrupacion.val() }, false);
                        cboConcepto.select2({ width: "100%" });
                    }

                    axios.post('VerificarPlanMaestroCapturado', { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() })
                        .then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                if (response.data.existePlan) {
                                    mdlCECaptura.modal("show");
                                } else {
                                    Alert2Warning(`No existe un plan maestro para el cc "${cboFiltroCC.find('option:selected').text().trim()}" en el año ${cboFiltroAnio.val()}. No se puede proceder con la captura.`);
                                }
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            });

            btnCECaptura.on("click", function () {
                fncCECapturas();
            });

            $(".importePpto").on("click", function () {
                $(this).select();
            });

            btnFiltroBuscar.on("click", function () {
                fncVerificarPptoNotificado();
            });

            cboFiltroAgrupacion.on("change", function () {
                if ($(this).val() > 0) {
                    lblTitleCatalogo.html(` - ${$(this).find('option:selected').text()}`);
                } else {
                    lblTitleCatalogo.html("");
                }

                fncVerificarPptoNotificado();
            });

            cboConcepto.on("change", function () {
                if ($(this).val() > 0) {
                    fncGetInsumoCuenta();
                }
            });

            btnFiltroNotificar.on("click", function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea notificar el presupuesto capturado?', 'Confirmar', 'Cancelar', () => fncNotificarPptoCapturado());
            });

            btnFiltroMatch.on("click", function () {
                fncMatchPptoVsPlanMaestro();
            });

            btnCrearComentario.on("click", function () {
                fncCrearComentario();
            });

            $(".importePpto").on("change", function () {
                let importe = unmaskNumero($(this).val());
                let mask = maskNumero_NoDecimal(importe);
                $(this).val(mask);
            });
            //#endregion

            //#region EVENTOS ADITIVAS
            mdlAditiva.on('shown.bs.modal', function () {
                comentarioAditiva.css('width', '100%');
                tblAditiva.DataTable().columns.adjust().draw();
            });

            btnAgregarAditiva.on('click', function () {
                $(this).hide();
                divComentarioAditiva.show();

                const aditiva = {
                    id: 0,
                    indicador: TipoPresupuestoEnum.aditivaDeductivaNueva,
                    descripcion: "NUEVA",
                    importeEnero: 0,
                    importeFebrero: 0,
                    importeMarzo: 0,
                    importeAbril: 0,
                    importeMayo: 0,
                    importeJunio: 0,
                    importeJulio: 0,
                    importeAgosto: 0,
                    importeSeptiembre: 0,
                    importeOctubre: 0,
                    importeNoviembre: 0,
                    importeDiciembre: 0,
                    importeTotal: 0
                }

                tblAditiva.DataTable().row.add(aditiva).draw();
                comentarioAditiva.val("");
            });

            btnAditivaGuardar.on('click', function () {
                let aditivasNuevas = new Array();
                let mensajeError = "";

                tblAditiva.find('tbody tr').each(function (idx, row) {
                    let rowData = tblAditiva.DataTable().row($(row)).data();

                    if (rowData.indicador == TipoPresupuestoEnum.aditivaDeductivaNueva) {

                        let importeEnero = $(row).find('td:eq(1)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(1)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(1)').find('.inputImporteAditiva').val();
                        let importeFebrero = $(row).find('td:eq(2)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(2)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(2)').find('.inputImporteAditiva').val();
                        let importeMarzo = $(row).find('td:eq(3)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(3)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(3)').find('.inputImporteAditiva').val();
                        let importeAbril = $(row).find('td:eq(4)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(4)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(4)').find('.inputImporteAditiva').val();
                        let importeMayo = $(row).find('td:eq(5)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(5)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(5)').find('.inputImporteAditiva').val();
                        let importeJunio = $(row).find('td:eq(6)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(6)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(6)').find('.inputImporteAditiva').val();
                        let importeJulio = $(row).find('td:eq(7)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(7)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(7)').find('.inputImporteAditiva').val();
                        let importeAgosto = $(row).find('td:eq(8)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(8)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(8)').find('.inputImporteAditiva').val();
                        let importeSeptiembre = $(row).find('td:eq(9)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(9)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(9)').find('.inputImporteAditiva').val();
                        let importeOctubre = $(row).find('td:eq(10)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(10)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(10)').find('.inputImporteAditiva').val();
                        let importeNoviembre = $(row).find('td:eq(11)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(11)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(11)').find('.inputImporteAditiva').val();
                        let importeDiciembre = $(row).find('td:eq(12)').find('.inputImporteAditiva').val().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(12)').find('.inputImporteAditiva').val()) : $(row).find('td:eq(12)').find('.inputImporteAditiva').val();

                        //#region SE VERIFICA SI HAY MODIFICACIONES EN MESES PASADOS Y VERIFICAR QUE SEAN SOLO DEDUCTIVAS
                        if ($(row).find('td:eq(1)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeEnero > 0) {
                                mensajeError = "En el mes de enero solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(2)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeFebrero > 0) {
                                mensajeError = "En el mes de febrero solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(3)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeMarzo > 0) {
                                mensajeError = "En el mes de marzo solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(4)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeAbril > 0) {
                                mensajeError = "En el mes de abril solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(5)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeMayo > 0) {
                                mensajeError = "En el mes de mayo solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(6)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeJunio > 0) {
                                mensajeError = "En el mes de junio solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(7)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeJulio > 0) {
                                mensajeError = "En el mes de julio solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if (cboFiltroCC.val() != '2195') { // ELIMINAR ESTA CONDICIÓN AL INICIAR NOVIEMBRE.
                            if ($(row).find('td:eq(8)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                                if (importeAgosto > 0) {
                                    mensajeError = "En el mes de agosto solo se aceptan deductivas en caso de ser necesario.";
                                }
                            }
                        }

                        if ($(row).find('td:eq(9)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeSeptiembre > 0) {
                                mensajeError = "En el mes de septiembre solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(10)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeOctubre > 0) {
                                mensajeError = "En el mes de octubre solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(11)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeNoviembre > 0) {
                                mensajeError = "En el mes de noviembre solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }

                        if ($(row).find('td:eq(12)').find('.inputImporteAditiva').attr("soloDeductiva") != undefined) {
                            if (importeDiciembre > 0) {
                                mensajeError = "En el mes de diciembre solo se aceptan deductivas en caso de ser necesario.";
                            }
                        }
                        //#endregion

                        if (mensajeError != "") {
                            Alert2Warning(mensajeError);
                        } else {
                            let aditivaNueva = {
                                id: rowData.id,
                                indicador: rowData.indicador,
                                descripcion: rowData.descripcion,
                                importeEnero: importeEnero,
                                importeFebrero: importeFebrero,
                                importeMarzo: importeMarzo,
                                importeAbril: importeAbril,
                                importeMayo: importeMayo,
                                importeJunio: importeJunio,
                                importeJulio: importeJulio,
                                importeAgosto: importeAgosto,
                                importeSeptiembre: importeSeptiembre,
                                importeOctubre: importeOctubre,
                                importeNoviembre: importeNoviembre,
                                importeDiciembre: importeDiciembre,
                                importeTotal: $(row).find('td:eq(13)').text().split('$')[0] == '' ? unmaskNumero($(row).find('td:eq(13)').text()) : $(row).find('td:eq(13)').text(),
                                comentario: comentarioAditiva.val()
                            }

                            aditivasNuevas.push(aditivaNueva);
                        }
                    }
                });

                if (mensajeError == "") {
                    if (aditivasNuevas.length == 0) {
                        Alert2Error('Favor de agregar una aditiva');
                        return;
                    }

                    if (!comentarioAditiva.val()) {
                        Alert2Error('Favor de agregar un comentario para la aditiva');
                        return;
                    }

                    $.post('GuardarAditivasNuevas',
                        {
                            aditivas: aditivasNuevas,
                            idPresupuesto: btnAgregarAditiva.data('idpresupuesto')
                        }).then(response => {
                            if (response.success) {
                                Alert2Exito("Se ha registrado con éxito la aditiva.");
                                obtenerAditivas(btnAgregarAditiva.data('idpresupuesto')).done(function (response) {
                                    if (response && response.success) {
                                        addRows(tblAditiva, response.items);
                                    }
                                });
                            } else {
                                Alert2Error(response.message);
                            }
                        }, error => {
                            Alert2Error(error.message)
                        });
                }
            });
            //#endregion

            //#region FILL COMBOS
            $(".select2").select2();

            cboAnio.fillCombo("FillAnios", {}, false);
            cboAnio.select2({ width: "100%" });

            cboFiltroAnio.fillCombo("FillAnios", {}, false);
            cboFiltroAnio.select2({ width: "100%" });

            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo("FillUsuarioRelCC", { anio: $(this).val() }, false);
                    cboFiltroCC.select2({ width: "100%" });

                    cboCC.fillCombo("FillUsuarioRelCC", { anio: $(this).val() }, false);
                    cboCC.select2({ width: "100%" });
                }
                btnFiltroNotificar.attr("disabled", "disabled");
                fncLimpiarTblCapturas();
            });

            cboFiltroCC.on("change", function () {
                if (cboFiltroAnio.val() > 0 && $(this).val()) {
                    cboFiltroAgrupacion.fillCombo("FillAgrupaciones", { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() }, false);
                    cboFiltroAgrupacion.select2({ width: "100%" });

                    cboAgrupacion.fillCombo("FillAgrupaciones", { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val() }, false);
                    cboAgrupacion.select2({ width: "100%" });
                }

                pptoNotificado = true;
                btnFiltroNuevaCaptura.removeAttr("disabled");
                // btnFiltroNotificar.removeAttr("disabled");
                btnFiltroNotificar.attr("disabled", "disabled");
                fncLimpiarTblCapturas();
            });

            cboResponsable.fillCombo("FillUsuarios", {}, false);
            cboResponsable.select2({ width: "100%" });
            //#endregion
        }

        //#region CRUD CAPTURAS
        function fncLimpiarTblCapturas() {
            dtCapturas.clear();
            dtCapturas.rows.add("");
            dtCapturas.draw();
        }

        function initTblCapturas() {
            dtCapturas = tblAF_CtrlPptalOfCe_CapPptos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'concepto', title: 'Concepto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                    },
                    {
                        data: 'lenActividad', title: 'Actividad',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<label title="${row.actividad}">${data}</label>`;
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeEnero', title: 'Enero',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeEnero" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeEnero">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeFebrero', title: 'Febrero',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeFebrero" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeFebrero">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeMarzo', title: 'Marzo',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeMarzo" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeMarzo">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeAbril', title: 'Abril',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeAbril" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeAbril">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeMayo', title: 'Mayo',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeMayo" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeMayo">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeJunio', title: 'Junio',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeJunio" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeJunio">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeJulio', title: 'Julio',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeJulio" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeJulio">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeAgosto', title: 'Agosto',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeAgosto" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeAgosto">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeSeptiembre', title: 'Septiembre',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeSeptiembre" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeSeptiembre">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeOctubre', title: 'Octubre',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeOctubre" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeOctubre">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeNoviembre', title: 'Noviembre',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeNoviembre" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeNoviembre">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        data: 'importeDiciembre', title: 'Diciembre',
                        render: function (data, type, row) {
                            if (row.esTotal) {
                                return `<a class="importeDiciembre" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                            }
                            else {
                                return `<a class="importeDiciembre">${maskNumero_NoDecimal(data)}</a>`
                            }
                        }
                    },
                    {
                        data: 'totalRow', title: 'Total',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            $(td).css('background-color', '#dbdcd9');
                        },
                        render: function (data, type, row) {
                            return `<a class="importeTotalRow" style="font-weight: bold !important;">${maskNumero_NoDecimal(data)}</a>`
                        }
                    },
                    { data: 'anio', title: 'Año', visible: false },
                    {
                        data: 'responsable', title: 'Responsable',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: null,
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esTotal) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row, meta) {
                            let btnEditar = "";
                            let btnEliminar = "";

                            if (pptoNotificado) {
                                btnEditar = `<button class="btn btn-xs btn-warning editarRegistro" title="Editar registro." disabled="disabled"><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                                btnEliminar = `<button style="display: none"; class="btn btn-xs btn-danger eliminarRegistro" title="Eliminar registro." disabled="disabled"><i class="fas fa-trash"></i></button>`;
                            } else {
                                btnEditar = `<button class="btn btn-xs btn-warning editarRegistro" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                                btnEliminar = `<button style="display: none"; class="btn btn-xs btn-danger eliminarRegistro" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            }
                            let btnAditiva = `<button class="btn btn-xs btn-info aditivaRegistro" title="Aditiva Deductiva"><i class="fas fa-plus"></i></button>`;

                            if (row.autorizado) {
                                if (row.idResponsable > 0) {
                                    return btnEditar + ' ' + btnAditiva + ' ' + btnEliminar;
                                }
                            } else {
                                if (row.idResponsable > 0) {
                                    return btnEditar + ' ' + btnEliminar;
                                }
                            }
                            return ``;
                        },
                    },
                    { data: 'cc', title: 'CC', visible: false },
                    { data: 'agrupacion', title: 'Agrupación', visible: false },
                    { data: 'concepto', title: 'Concepto', visible: false },
                    { data: 'id', visible: false },
                    { data: 'esTotal', title: 'esTotal', visible: false },
                    { data: 'actividad', title: 'actividad', visible: false }
                ],
                initComplete: function (settings, json) {
                    //#region COMENTARIOS
                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeEnero", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 1);
                        fncGetComentarios(1, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeFebrero", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 2);
                        fncGetComentarios(2, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeMarzo", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 3);
                        fncGetComentarios(3, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeAbril", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 4);
                        fncGetComentarios(4, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeMayo", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 5);
                        fncGetComentarios(5, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeJunio", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 6);
                        fncGetComentarios(6, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeJulio", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 7);
                        fncGetComentarios(7, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeAgosto", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 8);
                        fncGetComentarios(8, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeSeptiembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 9);
                        fncGetComentarios(9, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeOctubre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 10);
                        fncGetComentarios(10, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeNoviembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 11);
                        fncGetComentarios(11, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on("click", ".importeDiciembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 12);
                        fncGetComentarios(12, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });
                    //#endregion

                    tblAF_CtrlPptalOfCe_CapPptos.on('click', '.editarRegistro', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on('click', '.aditivaRegistro', function () {
                        let rowData = tblAF_CtrlPptalOfCe_CapPptos.DataTable().row($(this).closest('tr')).data();

                        obtenerAditivas(rowData.id).done(function (response) {
                            if (response && response.success) {
                                btnAgregarAditiva.removeData('idpresupuesto');
                                btnAgregarAditiva.data('idpresupuesto', rowData.id);
                                addRows(tblAditiva, response.items);
                                btnAgregarAditiva.show();
                                comentarioAditiva.val('');
                                divComentarioAditiva.hide();
                                mdlAditiva.modal('show');
                            }
                        });
                    });

                    tblAF_CtrlPptalOfCe_CapPptos.on('click', '.verActividad', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        txtVerActividad.val("");
                        txtVerActividad.val(rowData.actividad);
                        mdlVerActividad.modal("show");
                    });
                },
                columnDefs: [
                    { targets: [1, 14, 15, 16, 17], className: 'dt-body-center' },
                    { targets: [1, 14, 15, 16, 17], className: 'dt-head-center' },
                    { targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-body-right' },
                    { width: "5%", targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13] },
                    { width: "5%", targets: [17] },
                    { width: "20%", targets: [16] },
                    { width: "8%", targets: [0, 1] }
                ],
            });
        }

        function fncCrearComentario() {
            let obj = new Object();
            obj = {
                anio: cboFiltroAnio.val(),
                idCC: cboFiltroCC.val(),
                comentario: txtComentarios.val(),
                idMes: btnCrearComentario.attr("data-id"),
                idConcepto: _idConcepto
            }
            axios.post("CrearComentario", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetComentarios(btnCrearComentario.attr("data-id"), _idConcepto);
                    txtComentarios.val("");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetComentarios(mes, idConcepto) {
            let objDTO = { anio: cboFiltroAnio.val(), idCC: cboFiltroCC.val(), idMes: btnCrearComentario.attr("data-id"), idConcepto };

            axios.post("GetComentarios", objDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    ulComentarios.html("");
                    let cantComentarios = response.data.lstComentarios;
                    let arr = new Array();
                    for (let i = 0; i < cantComentarios.length; i++) {
                        let data = new Object();
                        data.fecha = moment(response.data.lstComentarios[i].fechaCreacion).format("DD/MM/YYYY");
                        data.usuarioNombre = response.data.lstComentarios[i].usuarioNombre;
                        data.comentario = response.data.lstComentarios[i].comentario;
                        arr.push(data);
                    }
                    setComentarios(arr);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='100'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }

        function fncGetCapturas() {
            if (cboFiltroCC.val() > 0 && cboFiltroAgrupacion.val() > 0 && cboFiltroAnio.val() > 0) {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    idAgrupacion: cboFiltroAgrupacion.val(),
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtCapturas.clear();
                        dtCapturas.rows.add(response.data.lstCapPptos);
                        dtCapturas.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "Es necesario indicar un CC." : "";
                cboFiltroAgrupacion.val() <= 0 ? strMensajeError += "<br>Es necesario indicar una agrupación." : "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un año." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncVerificarPptoNotificado() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0 && cboFiltroAgrupacion.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val()
                }
                axios.post("VerificarPptoNotificado", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        let notificado = response.data.obj;
                        if (notificado) {
                            pptoNotificado = true;
                            btnFiltroNuevaCaptura.attr("disabled", "disabled");
                            btnFiltroNotificar.attr("disabled", "disabled");
                        } else {
                            pptoNotificado = false;
                            btnFiltroNuevaCaptura.removeAttr("disabled", "false");
                            btnFiltroNotificar.removeAttr("disabled", "false");
                        }
                        fncGetCapturas();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar el año." : "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar el CC." : "";
                cboFiltroAgrupacion.val() <= 0 ? strMensajeError += "<br>Es necesario indicar la agrupación." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncCECapturas() {
            let obj = fncObjCECaptura();
            if (obj != "") {
                axios.post("CrearEditarCaptura", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        mdlCECaptura.modal("hide");
                        Alert2Exito(response.data.message);
                        fncMatchPptoVsPlanMaestro();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCECaptura() {
            fncBorderDefault();
            let strMensajeError = "";
            if (cboConcepto.val() == "") { cboConcepto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCECaptura.attr("data-id"),
                    actividad: txtActividad.val(),
                    cc: cboCC.val(),
                    idAgrupacion: cboAgrupacion.val(),
                    idConcepto: cboConcepto.val(),
                    importeEnero: unmaskNumero(txtImporteEnero.val()),
                    importeFebrero: unmaskNumero(txtImporteFebrero.val()),
                    importeMarzo: unmaskNumero(txtImporteMarzo.val()),
                    importeAbril: unmaskNumero(txtImporteAbril.val()),
                    importeMayo: unmaskNumero(txtImporteMayo.val()),
                    importeJunio: unmaskNumero(txtImporteJunio.val()),
                    importeJulio: unmaskNumero(txtImporteJulio.val()),
                    importeAgosto: unmaskNumero(txtImporteAgosto.val()),
                    importeSeptiembre: unmaskNumero(txtImporteSeptiembre.val()),
                    importeOctubre: unmaskNumero(txtImporteOctubre.val()),
                    importeNoviembre: unmaskNumero(txtImporteNoviembre.val()),
                    importeDiciembre: unmaskNumero(txtImporteDiciembre.val()),
                    idResponsable: cboResponsable.val(),
                    anio: cboAnio.val()
                };
                return obj;
            }
        }

        function fncEliminarCaptura(idCaptura) {
            if (idCaptura > 0) {
                let obj = new Object();
                obj = {
                    idCaptura: idCaptura
                }
                axios.post("EliminarCaptura", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el registro seleccionado.");
            }
        }

        function fncGetDatosActualizarCaptura(idCaptura) {
            if (idCaptura > 0) {
                let obj = new Object();
                obj = {
                    idCaptura: idCaptura
                }
                axios.post("GetDatosActualizarCaptura", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCECaptura.attr("data-id", idCaptura);
                        txtActividad.val(response.data.objCapPpto.actividad);
                        cboCC.val(response.data.objCapPpto.cc);
                        cboCC.trigger("change");
                        cboAgrupacion.val(response.data.objCapPpto.idAgrupacion);
                        cboAgrupacion.trigger("change");
                        if (cboAgrupacion.val() != "") {
                            cboConcepto.fillCombo("FillConceptos", { idAgrupacion: cboAgrupacion.val() }, false);
                            cboConcepto.select2({ width: "100%" });
                            cboConcepto.val(response.data.objCapPpto.idConcepto);
                            cboConcepto.trigger("change");
                        }
                        txtImporteEnero.val(maskNumero_NoDecimal(response.data.objCapPpto.importeEnero));
                        txtImporteFebrero.val(maskNumero_NoDecimal(response.data.objCapPpto.importeFebrero));
                        txtImporteMarzo.val(maskNumero_NoDecimal(response.data.objCapPpto.importeMarzo));
                        txtImporteAbril.val(maskNumero_NoDecimal(response.data.objCapPpto.importeAbril));
                        txtImporteMayo.val(maskNumero_NoDecimal(response.data.objCapPpto.importeMayo));
                        txtImporteJunio.val(maskNumero_NoDecimal(response.data.objCapPpto.importeJunio));
                        txtImporteJulio.val(maskNumero_NoDecimal(response.data.objCapPpto.importeJulio));
                        txtImporteAgosto.val(maskNumero_NoDecimal(response.data.objCapPpto.importeAgosto));
                        txtImporteSeptiembre.val(maskNumero_NoDecimal(response.data.objCapPpto.importeSeptiembre));
                        txtImporteOctubre.val(maskNumero_NoDecimal(response.data.objCapPpto.importeOctubre));
                        txtImporteNoviembre.val(maskNumero_NoDecimal(response.data.objCapPpto.importeNoviembre));
                        txtImporteDiciembre.val(maskNumero_NoDecimal(response.data.objCapPpto.importeDiciembre));
                        cboResponsable.val(response.data.objCapPpto.idResponsable);
                        cboResponsable.trigger("change");
                        cboAnio.val(response.data.objCapPpto.anio);
                        cboAnio.trigger("change");
                        fncTitleMdlCECaptura(false);
                        mdlCECaptura.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información.")
            }
        }

        function fncTitleMdlCECaptura(esCrear) {
            if (esCrear) {
                lblTitleCECaptura.html(`Nuevo registro - ${cboFiltroAgrupacion.find('option:selected').text()}`);
                lblTitleBtnCECaptura.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCECaptura.attr("data-id", 0);
            } else {
                lblTitleCECaptura.html(`Actualizar registro - ${cboFiltroAgrupacion.find('option:selected').text()}`);
                lblTitleBtnCECaptura.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
            }
        }

        function fncGetInsumoCuenta() {
            let obj = new Object();
            obj = {
                idConcepto: cboConcepto.val()
            }
            axios.post("GetInsumoCuenta", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtInsumo.val(response.data.objInfo.insumoDescripcion);
                    txtCuentaDescripcion.val(response.data.objInfo.cuentaDescripcion);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncNotificarPptoCapturado() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val()
                }
                axios.post("NotificarPptoCapturado", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnFiltroBuscar.trigger("click");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar el año." : "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indiciar el CC." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncMatchPptoVsPlanMaestro() {
            if (cboFiltroAnio.val() > 0 && cboFiltroCC.val() > 0 && cboFiltroAgrupacion.val() > 0) {
                let obj = new Object();
                obj = {
                    anio: cboFiltroAnio.val(),
                    idCC: cboFiltroCC.val(),
                    idAgrupacion: cboFiltroAgrupacion.val()
                }
                axios.post("MatchPptoVsPlanMaestro", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //${maskNumero2DCompras(data)}
                        lblTitleMatch.html(`
                            <label style="font-size: 22px;">Plan maestro: ${maskNumero2DCompras(response.data.objCapPpto.totalAgrupacionPlanMaestro)}</label><br>
                            <label style="font-size: 22px;">Agrupación Captura: ${maskNumero2DCompras(response.data.objCapPpto.totalAgrupacionCaptura)}</label><br>
                            <label style="font-size: 15px;">${response.data.message}</label>`
                        );

                        if (response.data.esMatch) {
                            if (response.data.objCapPpto.totalAgrupacionPlanMaestro == 0 && response.data.objCapPpto.totalAgrupacionCaptura == 0) {
                                lblMatch.html(`<h5 style="font-size: 25px; color: red;">MATCH</h5>`);

                            } else {
                                lblMatch.html(`<h5 style="font-size: 25px; color: green;">MATCH</h5>`);

                            }
                        } else {
                            lblMatch.html(`<h5 style="font-size: 25px; color: red;">MATCH</h5>`);
                        }

                        mdlMatch.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {

            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarMdlCECapturas() {
            $('input[type="text"]').val("");
            txtActividad.val("");
            cboCC[0].selectedIndex = 0;
            cboCC.trigger("change");
            cboAgrupacion[0].selectedIndex = 0;
            cboAgrupacion.trigger("change");
            cboConcepto[0].selectedIndex = 0;
            cboConcepto.trigger("change");
            cboResponsable[0].selectedIndex = 0;
            cboResponsable.trigger("change");
            cboAnio[0].selectedIndex = 0;
            cboAnio.trigger("change");
        }

        function fncBorderDefault() {
            $("#select2-cboConcepto-container").css("border", "1px solid #CCC");
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion

        //#region ADITIVA
        function initTablaAdivitas() { //TODO
            tblAditiva.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: true,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                dom: 'frtip',

                columns: [
                    {
                        data: 'descripcion',
                        title: 'DESCRIPCIÓN',
                        className: 'dt-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeEnero',
                        title: 'ENERO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeFebrero',
                        title: 'FEBRERO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeMarzo',
                        title: 'MARZO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeAbril',
                        title: 'ABRIL',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeMayo',
                        title: 'MAYO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeJunio',
                        title: 'JUNIO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeJulio',
                        title: 'JULIO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeAgosto',
                        title: 'AGOSTO',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeSeptiembre',
                        title: 'SEPTIEMBRE',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeOctubre',
                        title: 'OCTUBRE',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeNoviembre',
                        title: 'NOVIEMBRE',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeDiciembre',
                        title: 'DICIEMBRE',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.descripcion == "TOTAL") ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeTotal',
                        title: 'TOTAL',
                        className: 'dt-body-right dt-header-center',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            $(td).css('background-color', '#dbdcd9');
                        }
                    }
                ],

                columnDefs: [
                    {
                        targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    if (data.indicador == TipoPresupuestoEnum.aditivaDeductivaNueva) {
                        for (let index = 1; index < 13; index++) {
                            $(row).find(`td:eq(${index})`).html(`<input type='text' class='inputImporteAditiva text-right' value='0' />`);
                        }
                    }
                },
                drawCallback: function (settings) {
                    var d = new Date();
                    var month = d.getMonth();
                    let inputs = $('.inputImporteAditiva')
                    for (let i = 0; i < inputs.length; i++) {
                        if (i < month) {
                            // $(inputs[i]).attr('disabled', true);
                            $(inputs[i]).attr('soloDeductiva', true);
                        }
                    }

                    tblAditiva.find('input.inputImporteAditiva').blur(function (e) {
                        let fila = ($(this).closest("tr")[0].rowIndex) - 2;
                        let importe = unmaskNumero($(this).val());
                        let mask = maskNumero_NoDecimal(importe);
                        $(this).val(mask);
                        $(this).text(mask);
                    });

                },
                initComplete: function (settings, json) {
                    tblAditiva.on('change', '.inputImporteAditiva', function () {
                        let rowData = tblAditiva.DataTable().row($(this).closest('tr')).data();

                        let totalAditiva = 0;
                        tblAditiva.find($(this).closest('tr')).find('td').each(function (idx, column) {
                            if (idx >= 1 && idx <= 12) {
                                let sumat = $(this).find('.inputImporteAditiva').val().split('$')[0] ? unmaskNumero($(this).find('.inputImporteAditiva').val()) : $(this).find('.inputImporteAditiva').val();
                                totalAditiva += isString(sumat) == true ? unmaskNumero(sumat) : sumat;
                            }
                            if (idx == 13) {
                                rowData.importeTotal = totalAditiva;
                                $(this).text(maskNumero_NoDecimal(rowData.importeTotal));
                            }
                        });
                    });

                }
            });
        }
        function isString(inputText) {
            if (typeof inputText === 'string' || inputText instanceof String) {
                //it is string
                return true;
            } else {
                //it is not string
                return false;
            }
        }
        function obtenerAditivas(idPresupuesto) {
            return $.get('GetAditivas',
                {
                    idPresupuesto
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        Alert2Error(response.message);
                    }
                }, error => {
                    Alert2Error(error.message)
                });
        }

        function agregarAditiva() {

        }
        //#endregion
    }

    $(document).ready(() => {
        CtrlPptalOfCE.Capturas = new Capturas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();