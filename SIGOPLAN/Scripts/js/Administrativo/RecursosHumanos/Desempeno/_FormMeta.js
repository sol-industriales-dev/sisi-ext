(() => {
    $.namespace('RecursosHumanos.Desempeno._FormMeta');
    _FormMeta = function () {
        var idMeta = 0, idProceso = 0, idUsuario = 0;
        const txtMetaPeso = $('#txtMetaPeso');
        const txtMetaNombre = $('#txtMetaNombre');
        const btnMetaLimpiar = $('#btnMetaLimpiar');
        const btnMetaGuardar = $('#btnMetaGuardar');
        const divMetaSemaforo = $('#divMetaSemaforo');
        const cboMetaEstrategia = $('#cboMetaEstrategia');
        const txtMetaDescripcion = $('#txtMetaDescripcion');
        const guardarMeta = new URL(window.location.origin + '/Administrativo/Desempeno/guardarMeta');
        const eliminarMeta = new URL(window.location.origin + '/Administrativo/Desempeno/eliminarMeta');
        const getLstSemaforo = new URL(window.location.origin + '/Administrativo/Desempeno/getLstSemaforo');
        const getCboEstrategias = new URL(window.location.origin + '/Administrativo/Desempeno/getCboEstrategias');
        let init = () => {
            initFormMeta();
            btnMetaLimpiar.click(limpiarMetaForm);
            btnMetaGuardar.click(setGuardarMeta);
            txtMetaPeso.on("keypress", function (e) {
                aceptaSoloNumeroXD($(this), e, 2);
                txtMetaPeso.change(setMetaPeso);
            });
            Listeners();
        }
        function Listeners(){
            txtMetaPeso.click(function (e){
                $(this).select();
            });
        }
        function setMetaPeso() {
            let peso = +this.value;
            if (peso < 0) {
                peso *= -1;
            }
            this.value = peso
        }
        function setMetaSemaforoForm(lst) {
            divMetaSemaforo.html("");
            lst.forEach(semaforo => {
                let divContenedor = $('<div>', {
                    class: 'input-group'
                }),
                    divColor = $('<span>', {
                        class: 'dot',
                        style: `background: #${semaforo.color}`
                    }),
                    lblPorcentaje = $('<label>', {
                        html: `&nbsp;${maskNumero2D(semaforo.minimo)} - ${maskNumero2D(semaforo.maximo)}`,
                    });
                divContenedor.append(divColor);
                divContenedor.append(lblPorcentaje);
                divMetaSemaforo.append(divContenedor);
            });
        }
        limpiarMetaForm = () => {
            setFormMeta({
                idMeta: 0,
                idProceso: 0,
                idUsuario: 0,
                nombre: "",
                descripcion: "",
                peso: 0,
                tipo: 1
            });
        }
        setMetaId = id => {
            idMeta = id;
        }
        setFormMeta = meta => {
            idMeta = meta.id;
            idProceso = meta.idProceso;
            idUsuario = meta.idUsuario;
            txtMetaNombre.val(meta.nombre);
            txtMetaDescripcion.val(meta.descripcion);
            txtMetaPeso.val(meta.peso);
            cboMetaEstrategia.val(meta.tipo);
        }
        getFormMeta = () => {
            let meta = {
                id: idMeta,
                idProceso: $('#cboEscProceso').val(),
                idUsuario: $('#btnMetaGuardar').data('idusuario'),
                nombre: txtMetaNombre.val(),
                tipo: cboMetaEstrategia.val(),
                descripcion: txtMetaDescripcion.val(),
                peso: txtMetaPeso.val()
                // esVobo: true //TODO
            };
            return meta;
        }
        setGuardarMeta = async () => {
            try {
                let metaForm = getFormMeta();
                // console.log(getFormMeta);
                let pesosTotales = 0;
                $('#divEscMetas ul li').each(function(value, index, array) {
                    if (metaForm.id != 0 && metaForm.id == $(this).data('id')) {

                    }
                    else {
                        pesosTotales += $(this).data('peso');
                    }
                });
                if(pesosTotales + parseInt(txtMetaPeso.val()) > 100) {
                    //AlertaGeneral('Alerta', 'La suma del peso de las meta no debe ser mayor a 100');
                    Alert2Warning("La suma del peso de la meta, no debe ser mayor a 100.");
                    return;
                }
                response = await ejectFetchJson(guardarMeta, metaForm);
                if (response.success) {
                    setLstMetaPorProceso(0);
                    $('#mdlFormMeta').modal('hide');
                    // ConfirmacionGeneral("Confirmación", `La meta ${txtMetaNombre.val()} fue guardada con éxito.`);
                    Alert2Exito("¡Se registró correctamente la meta!")
                } else {
                    //AlertaGeneral("Aviso", response.message);
                    Alert2Error(response.message);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setEliminarMeta = async () => {
            try {
                response = await ejectFetchJson(eliminarMeta, { id: idMeta });
                if (response.success) {
                    limpiarMetaForm();
                    //ConfirmacionGeneral("Confirmación", "La meta se eliminó correctamente.");
                    Alert2Exito("¡Se eliminó correctamente la meta!")
                    if ($("#divEscMetas").length > 0) {
                        setLstMetaPorProceso(0);
                    }
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        fillDivMetaSemaforo = async () => {
            try {
                response = await ejectFetchJson(getLstSemaforo, { idProceso });
                if (response.success) {
                    setMetaSemaforoForm(response.lst);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        initFormMeta = () => {
            cboMetaEstrategia.fillCombo(getCboEstrategias, null, true, null);
            limpiarMetaForm();
            fillDivMetaSemaforo();
        }
        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno._FormMeta = new _FormMeta();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();