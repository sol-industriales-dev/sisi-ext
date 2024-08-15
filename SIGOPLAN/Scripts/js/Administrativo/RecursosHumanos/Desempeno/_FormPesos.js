(() => {
    $.namespace('RecursosHumanos.Desempeno._FormPesos');
    _FormPesos = function () {
        var suma = 0;
        const tblMetaPeso = $('#tblMetaPeso');
        const lblMetaPesoTotal = $('#lblMetaPesoTotal');
        const btnMetaPesoGuardar = $('#btnMetaPesoGuardar');
        const guardarMetasPesos = new URL(window.location.origin + '/Administrativo/Desempeno/guardarMetasPesos');
        const getLstMetaPorPesos = new URL(window.location.origin + '/Administrativo/Desempeno/getLstMetaPorPesos');
        let init = () => {
            initDataTblMetaPeso();
            btnMetaPesoGuardar.click(setGuardarMetaPesos);
        }
        setLstMetaPesos = async (idProceso, idUsuario) => {
            try {
                dtMetaPeso.clear().draw();
                response = await ejectFetchJson(getLstMetaPorPesos, { idProceso, idUsuario });
                if (response.success) {
                    dtMetaPeso.rows.add(response.lst).draw();
                    setSumaMetaPesos();
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setGuardarMetaPesos = async () => {
            try {
                let peso = lblMetaPesoTotal.attr("data-peso");
                // console.log("Peso: " + peso);

                if (lblMetaPesoTotal.attr("data-peso") > 0){
                    response = await ejectFetchJson(guardarMetasPesos, getLstMetasPesos());
                    if (response.success) {
                        $('#divEscMetaEmpleado').find('.active').trigger('click');
                        // divEscMetaEmpleado.find('.empleado').on('click', function () {
                        //     divEscMetaEmpleado.find('.empleado').removeClass('active');
                        //     $(this).closest('li').addClass('active');
                        //     setEscComboProceso();
                        // });
                        $('#mdlFormPeso').modal('hide');

                        Alert2Exito("¡Se actualizó correctamente el peso!");
                        // ConfirmacionGeneral("Confirmación", "Pesos actualizados con éxito.");
                    }
                }else{
                    Alert2Warning("Es necesario que haya mínimo como peso: 0.01%");
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setSumaMetaPesos = () => {
            suma = 0;
            dtMetaPeso.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = this.node(),
                    peso = +$(row).find('.peso').val();
                suma += peso;
            });

            let textoPesoTotal = '';
            if(suma < 25 ) { textoPesoTotal = '<div class="alert alert-success" role="alert">Peso total: ' + suma + '%</div>'; }
            if(suma >= 25 && suma < 50) { textoPesoTotal = '<div class="alert alert-warning" role="alert">Peso total: ' + suma + '%</div>'; }
            if(suma >= 50 && suma < 100) { textoPesoTotal = '<div class="alert alert-info" role="alert">Peso total: ' + suma + '%</div>'; }
            if(suma == 100) { textoPesoTotal = '<div class="alert alert-success" role="alert">Peso total: ' + suma + '%</div>'; }

            lblMetaPesoTotal.html(textoPesoTotal);
            lblMetaPesoTotal.attr("data-peso", suma);
        }
        getLstMetasPesos = () => {
            let lst = [];
            dtMetaPeso.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = this.node(),
                    data = this.data();
                data.peso = +$(row).find('.peso').val();
                lst.push(data);
            });
            return lst;
        }
        initDataTblMetaPeso = () => {
            dtMetaPeso = tblMetaPeso.DataTable({
                destroy: true,
                paging: false,
                searching: false,
                info: false,
                ordering: false, 
                language: dtDicEsp, 
                columns: [
                    { data: 'nombre', title: 'META', width: '70%' }, 
                    {
                        data: 'peso', title: 'PESO', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input onclick="$(this).select();">`);
                            $(td).find(`input`).addClass(`form-control text-right peso`);
                            // $(td).find(`input`).prop("disabled", rowData.esVobo);
                            $(td).find(`input`).val(maskNumero2D(data));
                        }
                    }
                ], 
                initComplete: function (settings, json) {
                    tblMetaPeso.on('keyup', '.peso', function (e) { //TODO
                        // console.log("ENTRE");
                        aceptaSoloNumeroXD($(this), e, 2);

                        let peso = this.value,
                            row = $(this).closest('tr'),
                            data = dtMetaPeso.row(row).data();
                        if (peso < 0) {
                            peso *= -1;
                        }
                        setSumaMetaPesos();
                        if (suma > 100) {
                            this.value = parseFloat(data.peso).toFixed(2);
                            setSumaMetaPesos();
                        } else {
                            data.peso = peso;
                        }
                    });

                    /*tblMetaPeso.on('change', '.peso', function (event) { //TODO
                        console.log("ENTRE2");
                        aceptaSoloNumeroXD($(this), e, 2);

                        let peso = this.value,
                            row = $(this).closest('tr'),
                            data = dtMetaPeso.row(row).data();
                        if (peso < 0) {
                            peso *= -1;
                        }
                        setSumaMetaPesos();
                        if (suma > 100) {
                            this.value = maskNumero2D(data.peso);
                            setSumaMetaPesos();
                        } else {
                            data.peso = peso;
                        }
                    });*/
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno._FormPesos = new _FormPesos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();