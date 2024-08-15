var EmpleadosController = function () {

    //#region INPUTS
    const aEmpresas = $('#aEmpresas');
    const navTabEmpresas = $('#navTabEmpresas');
    const aEmpleados = $('#aEmpleados');
    const btnEmpresasBuscar = $('#btnEmpresasBuscar');
    const btnNuevo = $('#btnNuevo');
    const btnEditar = $('#btnEditar');
    const cboFiltroEmpresas = $('#cboFiltroEmpresas');
    const tblEmpresas = $('#tblS_Empresascontratistas');
    const cboEsActivo = $('#cboEsActivo');
    const txtNombreEmpresa = $('#txtNombreEmpresa');
    const txtEditarEmpresa = $('#txtEditarEmpresa');
    const mdlNuevoEmpresa = $('#mdlNuevoEmpresa');
    const mdlEditarEmpresa = $('#mdlEditarEmpresa');
    var dtEmpresas;
    var idEmpresa;
    const txtNOEmpleado = $('#txtNOEmpleado');
    const txtNombreEmpleado = $('#txtNombreEmpleado');
    const txtApeidoP = $('#txtApeidoP');
    const txtApeidoM = $('#txtApeidoM');
    const txtDomicilio = $('#txtDomicilio');
    const txtColonia = $('#txtColonia');
    const cboPais = $('#cboPais');
    const cboEstado = $('#cboEstado');
    const cboCiudad = $('#cboCiudad');
    const txtCodigoPostal = $('#txtCodigoPostal');
    const txtUMF = $('#txtUMF');
    const txtLugar = $('#txtLugar');
    const txtEstadoNacimiento = $('#txtEstadoNacimiento');
    const txtFechaDeNac = $('#txtFechaDeNac');
    const txtNumeroIMSS = $('#txtNumeroIMSS');
    const txtRFC = $('#txtRFC');
    const txtNumeroDeIdentificacion = $('#txtNumeroDeIdentificacion');
    const txtCURP = $('#txtCURP');
    const txtNombreCompletoPadre = $('#txtNombreCompletoPadre');
    const txtNombreCompletoMadre = $('#txtNombreCompletoMadre');
    const txtNombreCompletoEspo = $('#txtNombreCompletoEspo');
    const txtNombreBeneficiario = $('#txtNombreBeneficiario');
    const txtParentesco = $('#txtParentesco');
    const txtFechaDeNacimientoBeneficiario = $('#txtFechaDeNacimientoBeneficiario');
    const txtCalzado = $('#txtCalzado');
    const txtTallaCamisa = $('#txtTallaCamisa');
    const txtTallaPantalon = $('#txtTallaPantalon');
    const txtHijos = $('#txtHijos');
    const txtEdades = $('#txtEdades');
    const txtTipoSangre = $('#txtTipoSangre');
    const txtAlergias = $('#txtAlergias');
    const txtOveroll = $('#txtOveroll');
    const txtTelefono = $('#txtTelefono');
    const txtEstadoCivil = $('#txtEstadoCivil');
    const txtTipoVivienda = $('#txtTipoVivienda');
    const txtCelular = $('#txtCelular');
    const txtPuestoEmpleado = $('#txtPuestoEmpleado');
    const cboCC = $('#cboCC');
    const txtJefeInmediato = $('#txtJefeInmediato');
    const txtSueldoBase = $('#txtSueldoBase');
    const txtCantidadConLetra = $('#txtCantidadConLetra');
    const txtComplemento = $('#txtComplemento');
    const txtTotalSemanal = $('#txtTotalSemanal');
    const cboIdEmpresa = $('#cboIdEmpresa');
    const cboCCcc = $('#cboCCcc');
    const mdlNuevoEmpleado = $('#mdlNuevoEmpleado');
    const btnNuevoEmpleados = $('#btnNuevoEmpleados');
    const btnBuscarEmpleado = $('#btnBuscarEmpleado');
    const tblEmpleados = $('#tblS_EmpleadosContratistas');
    var dtEmpleados;
    const cboFiltroEmpresasEmpleadosExcel = $('#cboFiltroEmpresasEmpleadosExcel');
    const cboFiltroEmpresasEmpleados = $('#cboFiltroEmpresasEmpleados');
    const cboFechaAlta = $('#cboFechaAlta');
    const cboEsActivoEmpleados = $('#cboEsActivoEmpleados');
    const btnNuevoEm = $('#btnNuevoEm');
    const btnNuevoEmpleadosExcel = $('#btnNuevoEmpleadosExcel');

    const btnNuevoExcel = $('#btnNuevoExcel');
    
    const mdlRelEmpresaContratistas = $('#mdlRelEmpresaContratistas');
    const cboRelEmpresaContratista = $('#cboRelEmpresaContratista');
    const btnCancelarCrearRelEmpresaContratista = $('#btnCancelarCrearRelEmpresaContratista');
    const btnCrearRelContratista = $('#btnCrearRelContratista');
    const tblRelEmpresaContratistas = $('#tblRelEmpresaContratistas');
    let dtRelEmpresaContratista;
    let esContratista = false;
    //#endregion

    var Inicializar = function () {
        $.namespace('Contratistas.Empleados');
        Iniciar();
        fcnNuevoEmpleado();
        fncCargarCombo();
        fcnBuscarEmpleado();
        dnbbtnnuevo();
        CargarCombos();
        fcnbtnBuscar();
        fcnbtnNuevo();
        fcnbtnEditar();
        fncValidarAccesoContratista();
    }

    var Iniciar = function () {
        Empleados = function () {
            (function init() {
                initDatatblEmpleados();
                initDatatblEmpresas();
                initRelEmpresaContratistas();
                fncFillCboRelEmpresaContratista();

                txtFechaDeNacimientoBeneficiario.datepicker({
                    format: 'dd/mm/yyyy',
                    minView: 2,
                    maxView: 4,
                    autoclose: true,
                    language: 'es'
                });
                txtFechaDeNac.datepicker({
                    format: 'dd/mm/yyyy',
                    minView: 2,
                    maxView: 4,
                    autoclose: true,
                    language: 'es'
                });
                txtComplemento.change(function (e) {
                    let suma = txtTotalSemanal.val() + txtCantidadConLetra.val();
                    txtTotalSemanal.val(suma);
                });

                btnCrearRelContratista.click(function(e){
                    fncCrearRelEmpresaContratista();
                });

                btnCancelarCrearRelEmpresaContratista.click(function(e){
                    cboRelEmpresaContratista[0].selectedIndex = 0;
                    cboRelEmpresaContratista.trigger("change");
                });

                txtNOEmpleado.attr("disabled", true);
                btnNuevoExcel.click(function(){
                    console.log('click')
                    LimpiarInputsExcel();
                })
                $('#inputExcel').change(function () {
                    $('#lblTexto1').text($(this)[0].files[0].name);
                });
            })();
        }
    }

    var dnbbtnnuevo = function () {
        btnNuevoEm.click(function (e) {
            LimpiarModal();
        });
     
        btnNuevoEmpleadosExcel.click(function () {
            fncNuevoEmpleadosExcel();
        });
       
    }

    function LimpiarInputsExcel(){
        $('#lblTexto1').text('');
        $('#lblTexto1').text('Ningún archivo seleccionado');
        document.getElementById("inputExcel").value = null;
    }


    var fcnNuevoEmpleado = function () {
        btnNuevoEmpleados.click(function (e) {
            fncAgregarNuevoEmpleado();
        });
    }

    var fcnBuscarEmpleado = function () {
        btnBuscarEmpleado.click(function (e) {
            fcnBuscarlistadoDeEmpleados();
        });
    }

    var fcnBuscarlistadoDeEmpleados = function () {
        var idEmpleados = cboFiltroEmpresasEmpleados.val() == "" ? 0 : cboFiltroEmpresasEmpleados.val();
        var fechAlt = cboFechaAlta.val() == null ? "1900/01/01" : cboFechaAlta.val();
        var esActivo = cboEsActivoEmpleados.val();
        fncCargarTablaEmpleados(idEmpleados, fechAlt, esActivo);
    }

    var initDatatblEmpleados = function () {
        dtEmpleados = tblEmpleados.DataTable({
            language: dtDicEsp,
            ordering: true,
            paging: true,
            searching: true,
            bFilter: true,
            info: false,
            columns: [
                { data: 'id', title: 'Cve. empleado' },
                { data: 'nombreEmpresa', title: 'Empresa' },
                { data: 'nombreCompleto', title: 'Nombre completo' },
                { data: 'colonia', title: 'Colonia', visible: false },
                { data: 'domicilio', title: 'Domicilio', visible: false },
                {
                    title: "Estatus",
                    render: function (data, type, row) {
                        let activo;
                        row.esActivo ? activo = "Activo" : activo = "Desactivado";
                        return activo;
                    }, visible: false
                },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                        row.esActivo ?
                            btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarEmpleado' data-esActivo="1" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>` :
                            btnEliminar = `<button class='btn-eliminar btn btn-success eliminarEmpleado' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>`;

                        return `<button class='btn-editar btn btn-warning editarEmpleado' data-id="${row.id}">` +
                            `<i class='fas fa-pencil-alt'></i>` +
                            `</button>&nbsp;` + btnEliminar;
                    }
                }
            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "40%", "targets": [0, 2, 3] }
            ],
            initComplete: function (settings, json) {
                tblEmpleados.on("click", ".eliminarEmpleado", function () {
                    let esActivo = $(this).attr("data-esActivo");
                    if (esActivo == false) {
                        strMensaje = "¿Desea activar el registro seleccionado?";
                    } else {
                        strMensaje = "¿Desea desactivar el registro seleccionado?";
                    }
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            fncEliminarEmpleado($(this).attr("data-id"), esActivo);
                        }
                    });
                });

                tblEmpleados.on("click", ".editarEmpleado", function (e) {
                    const rowData = dtEmpleados.row($(this).closest("tr")).data();
                    mdlNuevoEmpleado.modal("show");
                    AbrirModalEditar(rowData);
                });
            }
        });
    }

    var fncEliminarEmpleado = function (id, esActivo) {
        var estatus = esActivo == 0 ? true : false;
        let datos = { id: id, esActivo: estatus };
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/IndicadoresSeguridad/ActivarDesactivar",
            data: datos,
            success: function (response) {
                if (!response.success) {
                    Alert2Error(response.message);
                }
                else {
                    let strMensaje = "";
                    esActivo == 1 ?
                        strMensaje = "Se ha desactivado con éxito el registro." :
                        strMensaje = "Se ha activado con éxito el registro.";
                    Alert2Exito(strMensaje);
                    fncCargarCombo();
                    fcnBuscarlistadoDeEmpleados();
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            }
        });
    }

    var fncCargarTablaEmpleados = function (idEmpresa, FechaAlta, esActivo) {
        let strMensajeError = "";
        if (esContratista){
            strMensajeError = cboFiltroEmpresasEmpleados.val() != "" ? strMensajeError = "" : strMensajeError = "Es necesario seleccionar una empresa.";
        }

        if (strMensajeError == ""){
            $.ajax({
                datatype: "json",
                type: "GET",
                url: "/Administrativo/IndicadoresSeguridad/getListadoDeEmpleados?idEmpresa=" + idEmpresa + "&FechaAlta=" + FechaAlta + "&esActivo=" + esActivo + "",
                success: function (response) {
                    if (response.success) {
                        dtEmpleados.clear();
                        dtEmpleados.rows.add(response.items);
                        dtEmpleados.draw();
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }else{
            Alert2Warning("Es necesario seleccionar una empresa.");
        }
    }

    var fncAgregarNuevoEmpleado = function () {
        let Valida = Validar();
        if (Valida == false) {
            AlertaGeneral('¡Aviso!', 'llene los datos importantes')
        } else {
            let Parametros = {
                id: txtNOEmpleado.val() == "" ? 0 : txtNOEmpleado.val(),
                idEmpresaContratista: cboIdEmpresa.val(),
                nombre: txtNombreEmpleado.val(),
                apePaterno: txtApeidoP.val(),
                apeMaterno: txtApeidoM.val(),
                domicilio: txtDomicilio.val(),
                colonia: txtColonia.val(),
                idPais: cboPais.val(),
                idEstado: cboEstado.val(),
                idCiudad: cboCiudad.val(),
                codigoPostal: txtCodigoPostal.val(),
                UMF: txtUMF.val(),
                localidadNacimiento: txtLugar.val(),
                estadoNacimiento: txtEstadoNacimiento.val(),
                fechaNacimiento: txtFechaDeNac.val(),
                numeroSeguroSocial: txtNumeroIMSS.val(),
                numeroDeIdentificacionOficial: txtNumeroDeIdentificacion.val(),
                rfc: txtRFC.val(),
                curp: txtCURP.val(),
                nombrePadre: txtNombreCompletoPadre.val(),
                nombreMadre: txtNombreCompletoMadre.val(),
                nombreEspo: txtNombreCompletoEspo.val(),
                beneficiario: txtNombreBeneficiario.val(),
                parentesco: txtParentesco.val(),
                fechaDeNacimientoParentesco: txtFechaDeNacimientoBeneficiario.val(),
                calzado: txtCalzado.val(),
                tipoSangre: txtTipoSangre.val(),
                estadoCivil: txtEstadoCivil.val(),
                tallaCamisa: txtTallaCamisa.val(),
                alergias: txtAlergias.val(),
                tipoVivienda: txtTipoVivienda.val(),
                tallaPantalon: txtTallaPantalon.val(),
                overoll: txtOveroll.val(),
                hijos: txtHijos.val(),
                edades: txtEdades.val(),
                telefono: txtTelefono.val(),
                celular: txtCelular.val(),
                puesto: txtPuestoEmpleado.val(),
                centroDeCostos: cboCC.val(),
                jefeInmediato: txtJefeInmediato.val(),
                sueldoBase: txtSueldoBase.val(),
                complento: txtComplemento.val(),
                total: txtTotalSemanal.val()
            }

            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrativo/IndicadoresSeguridad/CrearEditar",
                data: { _objEmpleados: Parametros },
                success: function (response) {
                    if (response.success) {
                        if (response.items.status == 1) {
                            mdlNuevoEmpleado.modal("hide");
                        } else {
                            AlertaGeneral('¡Aviso!', response.items.msjExito)
                        }
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    fcnBuscarlistadoDeEmpleados();
                }
            });
        }
    }

    var LimpiarModal = function (objDatos) {
        txtNOEmpleado.val('');
        txtNombreEmpleado.val('');
        txtApeidoP.val('');
        txtApeidoM.val('');
        txtDomicilio.val('');
        txtColonia.val('');
        cboPais.val('');
        cboCiudad.val('');
        txtCodigoPostal.val('');
        txtUMF.val('');
        txtLugar.val('');
        txtEstadoNacimiento.val('');
        txtFechaDeNac.val('');
        txtNumeroIMSS.val('');
        txtRFC.val('');
        txtNumeroDeIdentificacion.val('');
        txtCURP.val('');
        txtNombreCompletoPadre.val('');
        txtNombreCompletoMadre.val('');
        txtNombreCompletoEspo.val('');
        txtNombreBeneficiario.val('');
        txtParentesco.val('');
        txtFechaDeNacimientoBeneficiario.val('');
        txtCalzado.val('');
        txtTallaCamisa.val('');
        txtTallaPantalon.val('');
        txtHijos.val('');
        txtEdades.val('');
        txtTipoSangre.val('');
        txtAlergias.val('');
        txtOveroll.val('');
        txtTelefono.val('');
        txtEstadoCivil.val('');
        txtTipoVivienda.val('');
        txtCelular.val('');
        txtPuestoEmpleado.val('');
        cboCC.val('');
        txtJefeInmediato.val('');
        txtSueldoBase.val('');
        txtComplemento.val('');
        txtTotalSemanal.val('');
        cboIdEmpresa.val('');
        cboIdEmpresa.trigger("change");
    }

    var AbrirModalEditar = function (objDatos) {
        txtNOEmpleado.val(objDatos.id);
        txtNombreEmpleado.val(objDatos.nombre);
        txtApeidoP.val(objDatos.apePaterno);
        txtApeidoM.val(objDatos.apeMaterno);
        txtDomicilio.val(objDatos.domicilio);
        txtColonia.val(objDatos.colonia);
        txtCodigoPostal.val(objDatos.codigoPostal);
        txtUMF.val(objDatos.UMF);
        txtLugar.val(objDatos.localidadNacimiento);
        txtEstadoNacimiento.val(objDatos.estadoNacimiento);
        txtFechaDeNac.val(moment(objDatos.fechaNacimiento).format('DD/MM/YYYY'));
        txtNumeroIMSS.val(objDatos.numeroSeguroSocial);
        txtRFC.val(objDatos.rfc);
        txtNumeroDeIdentificacion.val(objDatos.numeroDeIdentificacionOficial);
        txtCURP.val(objDatos.curp);
        txtNombreCompletoPadre.val(objDatos.nombrePadre);
        txtNombreCompletoMadre.val(objDatos.nombreMadre);
        txtNombreCompletoEspo.val(objDatos.nombreEspo);
        txtNombreBeneficiario.val(objDatos.beneficiario);
        txtParentesco.val(objDatos.parentesco);
        txtFechaDeNacimientoBeneficiario.val(moment(objDatos.fechaDeNacimientoParentesco).format('DD/MM/YYYY'));
        txtCalzado.val(objDatos.calzado);
        txtTallaCamisa.val(objDatos.tallaCamisa);
        txtTallaPantalon.val(objDatos.tallaPantalon);
        txtHijos.val(objDatos.hijos);
        txtEdades.val(objDatos.edades);
        txtTipoSangre.val(objDatos.tipoSangre);
        txtAlergias.val(objDatos.alergias);
        txtOveroll.val(objDatos.overoll);
        txtTelefono.val(objDatos.telefono);
        txtEstadoCivil.val(objDatos.estadoCivil);
        txtTipoVivienda.val(objDatos.tipoVivienda);
        txtCelular.val(objDatos.celular);
        txtPuestoEmpleado.val(objDatos.puesto);
        txtJefeInmediato.val(objDatos.jefeInmediato);
        txtSueldoBase.val(objDatos.sueldoBase);
        txtComplemento.val(objDatos.complemento);
        txtTotalSemanal.val(objDatos.total);
        fncCargarComboMunicipio(objDatos.idEstado);

        cboIdEmpresa.val(objDatos.idEmpresaContratista);
        cboPais.val(objDatos.idPais);
        cboEstado.val(objDatos.idEstado);
        cboCiudad.val(objDatos.idCiudad);
        cboCC.val(objDatos.centroDeCostos);
        cboIdEmpresa.trigger("change");
        cboPais.trigger("change");
        cboEstado.trigger("change");
        cboCiudad.trigger("change");
        cboCC.trigger("change");
    }

    var Validar = function () {
        let Validacion = false;
        cboIdEmpresa.val() != '' ? Validacion = true : Validacion = false
        txtNombreEmpleado.val() != '' ? Validacion = true : Validacion = false
        txtDomicilio.val() != '' ? Validacion = true : Validacion = false
        txtColonia.val() != '' ? Validacion = true : Validacion = false
        txtApeidoP.val() != '' ? Validacion = true : Validacion = false
        txtApeidoM.val() != '' ? Validacion = true : Validacion = false
        return Validacion;
    }

    cboEstado.on("select2:select", function (e) {
        let selCC = $(this).find(`option[value="${$(this).val()}"]`);
        fncCargarComboMunicipio(cboEstado.val());
    });

    var fncCargarCombo = function () {
        cboFiltroEmpresasEmpleados.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
        cboIdEmpresa.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
        cboPais.fillCombo('/IndicadoresSeguridad/ObtenerPais', null, null);
        cboPais.val(1);
        fncCargarComboEstado(cboPais.val());
    }

    var fncCargarComboEstado = function (idPais) {
        cboEstado.fillCombo('/IndicadoresSeguridad/ObtenerEstado?idPais=' + idPais + '', null, null);
    }

    var fncCargarComboMunicipio = function (idEstado) {
        cboCiudad.fillCombo('/IndicadoresSeguridad/ObtenerMunicipio?idEstado=' + idEstado + '', null, null);
    }

    //#region EMPRESAS 
    var fcnbtnNuevo = function () {
        btnNuevo.click(function (e) {
            fncNuevaEmpresa();
            Limpiar();
        });
    }

    var fcnbtnBuscar = function () {
        btnEmpresasBuscar.click(function (e) {
            fncCargarTabla();
        });
    }

    var fcnbtnEditar = function () {
        btnEditar.click(function (e) {
            fcnEditarEmpresa();
        });
    }

    var fncCargarTabla = function () {
        var FiltroEmpresas = cboFiltroEmpresas.val() == "" ? 0 : cboFiltroEmpresas.val();
        fncCargarTablaIncidentes(FiltroEmpresas, cboEsActivo.val());
    }

    var Limpiar = function () {
        txtNombreEmpresa.val('');
    }

    var initDatatblEmpresas = function () {
        dtEmpresas = tblEmpresas.DataTable({
            language: dtDicEsp,
            ordering: true,
            paging: true,
            searching: true,
            bFilter: true,
            info: false,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'nombreEmpresa', title: 'Nombre Empresa' },
                {
                    title: "Estatus",
                    render: function (data, type, row) {
                        let activo;
                        row.esActivo ? activo = "Activo" : activo = "Desactivado";
                        return activo;
                    }, visible: false
                },
                {
                    title: "Empleados",
                    render: function (data, type, row) {
                        return `<button class="btn btn-success verContratistas"><i class="fas fa-list-ul"></i></button>`;
                    }
                },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                        row.esActivo ?
                            btnEliminar = `<button class='btn-eliminar btn btn-danger ActivarDesactivarEmpresas' data-esActivo="1" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>` :
                            btnEliminar = `<button class='btn-eliminar btn btn-success ActivarDesactivarEmpresas' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-toggle-on"></i></button>`;

                        return `<button class='btn-editar btn btn-warning EditarEmpresa' data-id="${row.id}">` +
                            `<i class='fas fa-pencil-alt'></i>` +
                            `</button>&nbsp;` + btnEliminar;
                    }
                }
            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "40%", "targets": [0, 2, 3] }
            ],
            initComplete: function (settings, json) {
                tblEmpresas.on("click", ".ActivarDesactivarEmpresas", function () {
                    let esActivo = $(this).attr("data-esActivo");
                    let strMensaje;
                    if (esActivo == false) {
                        strMensaje = "¿Desea Activar el registro seleccionado?";
                    } else {
                        strMensaje = "¿Desea desactivar el registro seleccionado?";
                    }

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            fncEliminar($(this).attr("data-id"), esActivo);
                        }
                    });
                });

                tblEmpresas.on("click", ".EditarEmpresa", function (e) {
                    const rowData = dtEmpresas.row($(this).closest("tr")).data();
                    txtEditarEmpresa.val(rowData.nombreEmpresa);
                    idEmpresa = rowData.id;
                    mdlEditarEmpresa.modal("show");
                });

                tblEmpresas.on("click", ".verContratistas", function(e){
                    const rowData = dtEmpresas.row($(this).closest("tr")).data();
                    btnCrearRelContratista.attr("data-id", rowData.id);
                    cboRelEmpresaContratista[0].selectedIndex = 0;
                    cboRelEmpresaContratista.trigger("change");
                    fncGetRelEmpresasContratistas(rowData.id);
                    mdlRelEmpresaContratistas.modal("show");
                })
            }
        });
    }

    var fncCargarTablaIncidentes = function (nombreEmpresa, esActivo) {
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/IndicadoresSeguridad/ObtenerEmpresas?nombreEmpresa=" + nombreEmpresa + "&esActivo=" + esActivo + "",
            success: function (response) {
                if (response.success) {
                    dtEmpresas.clear();
                    dtEmpresas.rows.add(response.items);
                    dtEmpresas.draw();
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }

    var CargarCombos = function () {
        cboFiltroEmpresas.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
        cboFiltroEmpresasEmpleadosExcel.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);

        axios.get('/IndicadoresSeguridad/GetCC').then(response => {
            let { items, message } = response.data;
            if (response.data.items.success) {
                cboCCcc.append('<option value="--Seleccione--">--Seleccione--</option>');

                items.items.forEach(x => {
                    let groupOption = `<optgroup label="${x.label}">`;

                    x.options.forEach(y => {
                        groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                    });

                    groupOption += `</optgroup>`;

                    cboCCcc.append(groupOption);
                });
            } else {
                AlertaGeneral(`Alerta`, message);
            }
        }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    var fncEliminar = function (id, esActivo) {
        var estatus = esActivo == 0 ? true : false;
        let datos = { idEmpresa: id, esActivo: estatus };
        $.ajax({
            datatype: "json",
            type: "GET",
            url: "/Administrativo/IndicadoresSeguridad/ActivarDesactivarEmpresa",
            data: datos,
            success: function (response) {
                if (!response.success) {
                    Alert2Error(response.message);
                }
                else {
                    let strMensaje = "";
                    esActivo == 1 ?
                        strMensaje = "Se ha desactivado con éxito el registro." :
                        strMensaje = "Se ha activado con éxito el registro.";
                    Alert2Exito(strMensaje);
                    CargarCombos(); //TODO
                    // fncCargarTabla();
                    btnEmpresasBuscar.trigger("click");
                    fncCargarCombo();
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            }
        });
    }

    var fncNuevaEmpresa = function () {
        if (txtNombreEmpresa.val() != ""){
            let Parametros = {
                nombreEmpresa: txtNombreEmpresa.val()
            }
            let strMensajeError = "";
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrativo/IndicadoresSeguridad/AgregarEmpresa",
                data: { _objEmpresa: Parametros },
                success: function (response) {
                    if (response.success) {
                        if (response.items.statusExito == 1) {
                            mdlNuevoEmpresa.modal("hide");
                            fncCargarCombo();
                            cboFiltroEmpresas.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
                        } else {
                            AlertaGeneral('¡Aviso!', response.items.msjExito)
                        }
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    fncCargarTabla();
                }
            });
        }else{
            Alert2Warning("Es necesario ingresar el nombre de la empresa.");
        }
    }

    var fcnEditarEmpresa = function () {
        var objEmpresa = {
            id: idEmpresa,
            nombreEmpresa: txtEditarEmpresa.val()
        }
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Administrativo/IndicadoresSeguridad/EditarEmpresa",
            data: { _objEmpresa: objEmpresa },
            success: function (response) {
                if (response.success) {
                    if (response.items.statusExito == 1) {
                        mdlEditarEmpresa.modal("hide");
                    } else {
                        AlertaGeneral('¡Aviso!', response.items.msjExito)
                    }
                } else {
                    Alert2Error(response.message);

                }

            },
            error: function () {
                // AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                fncCargarTabla();
            }
        });
    }
    //#endregion

    function formDataCargaMasiva() {
        let formData = new FormData();
        formData.append("id", 0);
        $.each(document.getElementById("inputExcel").files, function (i, file) {
            formData.append("archivo", file);
        });
        return formData;
    }

    function fncNuevoEmpleadosExcel() {
        let data = formDataCargaMasiva();
        axios.post('CargaMasivaContratistas', data, { headers: { 'Content-Type': 'multipart/form-data' } })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    $('#mdlNuevoEmpleadoExcel').modal('hide');
                    Alert2Exito(items.msjExito)
                    cboFiltroEmpresasEmpleados.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
                    cboFiltroEmpresas.fillCombo('/IndicadoresSeguridad/ObtenerEmpresasCombo', null, null);
                }
            });
    }

    function fncValidarAccesoContratista(){
        axios.post("ValidarAccesoContratista").then(response => {
            let { success, items, message } = response.data;
            if (success) {
                navTabEmpresas.hide();
                aEmpleados.trigger("click");
                esContratista = true;
            } else {
                navTabEmpresas.show();
                aEmpresas.trigger("click");
                esContratista = false;
            }
        }).catch(error => Alert2Error(error.message));
    }

    //#region CATÁLOGO RELACIÓN EMPRESA CONTRATISTA
    function fncFillCboRelEmpresaContratista(){
        cboRelEmpresaContratista.fillCombo('/IndicadoresSeguridad/FillCboContratistasSP', null, null);
    }

    function fncGetRelEmpresasContratistas(){
        let objData = new Object();
        objData = {
            idEmpresa: btnCrearRelContratista.attr("data-id")
        };
        axios.post("GetEmpresaRelContratistas", objData).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                dtRelEmpresaContratista.clear();
                dtRelEmpresaContratista.rows.add(response.data.lstContratistas).draw();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function initRelEmpresaContratistas() {
        dtRelEmpresaContratista = tblRelEmpresaContratistas.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: true,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'idContratista', title: 'Empleado', visible: false },
                { data: 'nomContratista', title: 'Empleado' },
                { 
                    render: function (data, type, row) { 
                        return `<button class="btn btn-danger eliminarRelEmpresaContratista" data-id="${row.id}"><i class="far fa-trash-alt"></i></button>`
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblRelEmpresaContratistas.on('click','.eliminarRelEmpresaContratista', function () {
                    let rowData = dtRelEmpresaContratista.row($(this).closest('tr')).data();
                    Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarRelEmpresaContratista(rowData.id));
                });
            },
            columnDefs: [
                { className: 'dt-center','targets': '_all'}
            ],
        });
    }

    function fncCrearRelEmpresaContratista(){
        let objData = fncGetDataRelEmpresaContratista();
        if (objData != undefined){
            axios.post("CrearEditarRelEmpresaContratista", objData).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se registro correctamente al empleado.");
                    fncGetRelEmpresasContratistas(btnCrearRelContratista.attr("data-id"));
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncGetDataRelEmpresaContratista(){
        let strMensajeError = "";
        let idContratista = cboRelEmpresaContratista.val() > 0 ? cboRelEmpresaContratista.val() : strMensajeError = "Es necesario seleccionar un contratista.";
        let objData = new Object();

        if (strMensajeError != ""){
            Alert2Warning(strMensajeError);
        }else{
            objData = {
                idEmpresa: btnCrearRelContratista.attr("data-id"),
                idContratista: idContratista
            };
            return objData;
        }
    }

    function fncEliminarRelEmpresaContratista(idRel){
        // axios.delete("EliminarRelEmpresaContratista", { params: { idRel: idRel }})
        let obj = new Object();
        obj = {
            idRel: idRel
        };
        axios.post("EliminarRelEmpresaContratista", obj)
        .then(response => {
            let { success, items, message } = response.data;
            if (success) {
                Alert2Exito("Se ha eliminado con éxito al empleado.");
                fncGetRelEmpresasContratistas(btnCrearRelContratista.attr("data-id"));
            } else {
                Alert2Error(message);
            }
        }).catch(error => AlertaGeneral(error.message));
    }
    //#endregion

    $(document).ready(() => {
        Contratistas.Empleados = new Empleados();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

    return {
        Inicializar: Inicializar
    }
};