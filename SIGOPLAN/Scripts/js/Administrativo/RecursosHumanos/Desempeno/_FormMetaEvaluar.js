(() => {
    $.namespace('RecursosHumanos.Desempeno._FormMetaEvaluar');
    _FormMetaEvaluar = function () {
        var idObservacion = 0, idMeta = 0, idEvaluacion = 0, idProceso = 0, idUsuario = 0, lstSemaforo = [], evaMetaContador = 0, lstEvaMetaEvidencias = [], esAutoEvaluacion;
        const divEvaMetaForm = $('.divEvaMetaForm');
        const lblEvaMetaTipo = $('#lblEvaMetaTipo');
        const btnEvaMetaGuardar = $('#btnEvaMetaGuardar');
        const txtVerComoActivado = $('#txtVerComoActivado');
        const divEvaMetaSemaforo = $('#divEvaMetaSemaforo');
        const txtEvaMetaResultado = $('#txtEvaMetaResultado');
        const tblEvaMetaEvidencia = $('#tblEvaMetaEvidencia');
        const btnEvaMetaEvidencia = $('#btnEvaMetaEvidencia');
        const cboEvaMetaEvaluacion = $('#cboEvaMetaEvaluacion');
        const fileEvaMetaEvidencia = $('#fileEvaMetaEvidencia');
        const lblEvaMetaDescripcion = $('#lblEvaMetaDescripcion');
        const divEvaMetaAutoIndicador = $('#divEvaMetaAutoIndicador');
        const divEvaMetaJefeIndicador = $('#divEvaMetaJefeIndicador');
        const txtEvaMetaJefeEvaluacion = $('#txtEvaMetaJefeEvaluacion');
        const txtEvaMetaAutoEvaluacion = $('#txtEvaMetaAutoEvaluacion');
        const txtEvaMetaJefeObservacion = $('#txtEvaMetaJefeObservacion');
        const divEvaluacionJefe = $('.divEvaluacionJefe');
        const divEvaluacionUsuario = $('.divEvaluacionUsuario');
        const txtEvaMetaAutoObservacion = $('#txtEvaMetaAutoObservacion');
        const infoAutoevaluacion = $('#infoAutoevaluacion');
        const colorAutoevaluacion = $('#colorAutoevaluacion');
        const resultadoEvaluacion = $('#resultadoEvaluacion');
        const resultadoEvaluacionSeguimiento = $('#resultadoEvaluacionSeguimiento');
        const comentarioEvaluacion = $('#comentarioEvaluacion');
        const getObservacion = new URL(window.location.origin + '/Administrativo/Desempeno/getObservacion');
        const eliminarEvidencia = new URL(window.location.origin + '/Administrativo/Desempeno/eliminarEvidencia');
        const metaEvidenciaGuardar = new URL(window.location.origin + '/Administrativo/Desempeno/metaEvidenciaGuardar');
        const getCboEvaluacionPorProceso = new URL(window.location.origin + '/Administrativo/Desempeno/getCboEvaluacionPorProceso');
        const resultadoSeguimiento = $('#resultadoSeguimiento');
        const progressBarAutoSeguimiento = $('#progressBarAutoSeguimiento');
        const progressBarJefeSeguimiento = $('#progressBarJefeSeguimiento');
        const descripcionEmpleado = $('#descripcionEmpleado');
        const descripcionJefe = $('#descripcionJefe');
        const btnDescargarEvidenciasMetaFormMetaEvaluar = $("#btnDescargarEvidenciasMetaFormMetaEvaluar");
        const btnDescargarEvidenciasMetaEscritorio = $("#btnDescargarEvidenciasMetaEscritorio");
        const modalDescargarEvidencias = $("#modalDescargarEvidencias");
        const btnModalDescargarEvidencia = $("#btnModalDescargarEvidencia");
        const btnModalCancelarDescarga = $("#btnModalCancelarDescarga");
        const modalEliminarEvidencias = $("#modalEliminarEvidencias");
        const btnModalEliminarEvidencia = $("#btnModalEliminarEvidencia");
        const btnModalCancelarEliminar = $("#btnModalCancelarEliminar");
        let idEvidencia = 0;
        let colorProgressBar = "";

        let init = () => {
            cboEvaMetaEvaluacion.select2();
            initDataTblEvaMetaEvidencia();
            //txtEvaMetaResultado.change(setEvaMetaResultado);
            txtEvaMetaResultado.on("keypress", function (e) {
                aceptaSoloNumeroXD($(this), e, 2);
                txtEvaMetaResultado.change(setEvaMetaResultado);
            });

            txtEvaMetaResultado.click(function(e){
                $(this).select();
            });

            txtEvaMetaResultado.attr("autocomplete", "off");
            txtEvaMetaAutoEvaluacion.attr("autocomplete", "off");
            txtEvaMetaJefeEvaluacion.attr("autocomplete", "off");

            txtEvaMetaAutoEvaluacion.click(function(e){
               $(this).select(); 
            })

            //txtEvaMetaAutoEvaluacion.change(setEvaMetaAutoIndicador);
            txtEvaMetaAutoEvaluacion.on("keypress", function (e) {
                aceptaSoloNumeroXD($(this), e, 2);
                txtEvaMetaAutoEvaluacion.change(setEvaMetaAutoIndicador);
            });

            //txtEvaMetaJefeEvaluacion.change(setEvaMetaJefeIndicador)
            txtEvaMetaJefeEvaluacion.on("keypress", function (e) {
                aceptaSoloNumeroXD($(this), e, 2);
                txtEvaMetaJefeEvaluacion.change(setEvaMetaJefeIndicador);
            });

            txtEvaMetaJefeEvaluacion.click(function(e){
                $(this).select();
            });

            btnDescargarEvidenciasMetaFormMetaEvaluar.click(function (e) { 
                //modalDescargarEvidencias.modal("show"); 
                Swal.fire({
                    position: "center",
                    icon: "info",
                    title: "Descarga de archivos",
                    width: '35%',
                    showCancelButton: true,
                    html: "<h3>¿Desea descargar los archivos adjuntos?</h3>",
                    confirmButtonText: "Confirmar",
                    confirmButtonColor: "#5cb85c",
                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#d9534f",
                    showCloseButton: true
                }).then((result) => {
                    if (result.value) {
                        DescargarEvidenciasMeta(false);
                    }
                });
            });

            btnDescargarEvidenciasMetaEscritorio.click(function (e) {
                //modalDescargarEvidencias.modal("show"); 
                Swal.fire({
                    position: "center",
                    icon: "info",
                    title: "Descarga de archivos",
                    width: '35%',
                    showCancelButton: true,
                    html: "<h3>¿Desea descargar los archivos adjuntos?</h3>",
                    confirmButtonText: "Confirmar",
                    confirmButtonColor: "#5cb85c",
                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#d9534f",
                    showCloseButton: true
                }).then((result) => {
                    if (result.value) {
                        DescargarEvidenciasMeta(true);
                    }
                });
            });

            btnModalDescargarEvidencia.click(function (e) { DescargarEvidenciasMeta(); });
            btnModalCancelarDescarga.click(function (e) { modalEliminarEvidencias.modal("hide"); });

            btnModalEliminarEvidencia.click(function (e) { fncEliminarEvidencia(); });
            btnModalCancelarEliminar.click(function (e) { modalEliminarEvidencias.modal("hide"); });

            cboEvaMetaEvaluacion.change(setEvaMetaObs);
            btnEvaMetaGuardar.click(setEvaMetaEvidenciaGuardar);
            btnEvaMetaEvidencia.click(setEvaMetaInputEvidencia);
            fileEvaMetaEvidencia.change(function () { setEvaMetaEvidencia(this); });

            btnEvaMetaGuardar.removeClass("hidden");
        }
        function setEvaMetaObs() {
            setEvaMetaObservacion(cboEvaMetaEvaluacion);
        };
        setEvaMetaObservacion = async (cboEva) => {
            try {
                idEvaluacion = $(cboEva).val();
                response = await ejectFetchJson(getObservacion, {
                    idMeta,
                    idEvaluacion,
                    idUsuarioCalificar: idUsuario
                });
                let { success, observacion } = response;
                if (success) {
                    //console.log("t2: " + response.observacion.id); SE OBTIENE idObservacion //TODO
                    btnDescargarEvidenciasMetaFormMetaEvaluar.attr("data-id", response.observacion.id);
                    btnDescargarEvidenciasMetaEscritorio.attr("data-id", response.observacion.id);
                    setFormEvaluacion(observacion);
                }else{
                    Alert2Warning(response.message);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setEliminarEvidencia = async id => {
            try {
                response = await ejectFetchJson(eliminarEvidencia, { id });
                if (response.success) {
                    NotificacionGeneral("Aviso", "Evidencia eliminada con éxito.");
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setEvaMetaEvidenciaGuardar = () => {
            try {
                btnEvaMetaGuardar.attr("disabled", true);
                let formData = getFormEvaMetaEvaluacion()
                    , request = new XMLHttpRequest();
                request.open("POST", metaEvidenciaGuardar);
                request.send(formData);
                request.onload = response => {
                    if (request.status == 200) {
                        var jsonResponse = JSON.parse(request.responseText);
                        if (jsonResponse.success) {    
                            btnEvaMetaGuardar.attr("disabled", false);

                            Alert2Exito("¡Se registró correctamente la evaluación!")
                            dtEvaMetaEvidencia.columns.adjust().draw();
                            // setMetaIndividual(idMeta);
                            // setEmpleadosJefe(btnEvaMetaGuardar.data().puedeJefeEvaluar ? $('#divEscMetaEmpleado').find("ul li.active").index() : -1);
                            setLstMetaPorProceso();
                            $('#modalEvaluacion').modal('hide');
                            fileEvaMetaEvidencia.val('');

                            // lstEvaMetaEvidencias = [];
                            // for (let i = 0; i < lstEvaMetaEvidencias.length; i++) {
                            //     console.log(lstEvaMetaEvidencias[i]);
                            // }
                        } else {
                            //AlertaGeneral("Aviso", "El archivo: " + `${jsonResponse.message}`);
                            //Alert2Error("El archivo: " + `${jsonResponse.message}`);
                            btnEvaMetaGuardar.attr("disabled", false);
                            Alert2Error(`Aviso: ${jsonResponse.message}`);
                        }
                    }
                };
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error("Aviso: " + o_O.message);
            }
        }
        setFormEvaluacion = ({ lstEvidencia, lstArchivo, meta, lstSemaforo, id, idUsuario, idJefe, resultado, autoEvaluacion, autoObservacion, 
                                jefeEvaluacion, jefeObservacion, puedeAutoEvaluar, puedeJefeEvaluar, esJefeEvaluado }) => {
            if(lstEvidencia.length == 0){
                btnDescargarEvidenciasMetaFormMetaEvaluar.attr("disabled", true);
                btnDescargarEvidenciasMetaEscritorio.attr("disabled", true);
            } else {
                btnDescargarEvidenciasMetaFormMetaEvaluar.attr("disabled", false);
                btnDescargarEvidenciasMetaEscritorio.attr("disabled", false);
            }
            setMetaSemaforoForm(lstSemaforo);
            dtEvaMetaEvidencia.clear().draw();
            esAutoEvaluacion = puedeAutoEvaluar;
            dtEvaMetaEvidencia.rows.add(lstEvidencia).draw();
            lblEvaMetaDescripcion.text(meta.descripcion);
            lblEvaMetaTipo.text(meta.tipo);
            txtEvaMetaResultado.val(resultado);
            txtEvaMetaAutoEvaluacion.val(autoEvaluacion);
            txtEvaMetaAutoObservacion.val(autoObservacion);
            txtEvaMetaJefeEvaluacion.val(jefeEvaluacion);
            txtEvaMetaJefeObservacion.val(jefeObservacion);
            setEvaMetaResultado();
            setEvaMetaAutoIndicador(true);
            setEvaMetaJefeIndicador();
            // txtEvaMetaResultado.prop("disabled", !puedeAutoEvaluar);
            // txtEvaMetaAutoEvaluacion.prop("disabled", !puedeAutoEvaluar);
            // txtEvaMetaAutoObservacion.prop("disabled", !puedeAutoEvaluar);
            
            // console.log(puedeJefeEvaluar);
            // console.log("B: " + puedeJefeEvaluar);
            if (!puedeJefeEvaluar) { 
                // console.log("AUTOEVALUACION");
                divEvaluacionJefe.hide(); 
                divEvaluacionUsuario.show();
            } else { 
                // console.log("PUEDE EVALUAR JEFE");
                divEvaluacionJefe.show(); 
                divEvaluacionUsuario.hide(); 
            }

            txtEvaMetaJefeEvaluacion.prop("disabled", !puedeJefeEvaluar);
            txtEvaMetaJefeObservacion.prop("disabled", !puedeJefeEvaluar);
            //if(puedeJefeEvaluar) { btnEvaMetaEvidencia.hide(); } else { btnEvaMetaEvidencia.show(); }

            // btnDescargarEvidenciasMetaFormMetaEvaluar.attr("data-index", lstEvidencia[0].id); //TODO

            // infoAutoevaluacion

            resultadoSeguimiento.text('0.00%');
            resultadoSeguimiento.text(maskNumero2D(esJefeEvaluado ? jefeEvaluacion : autoEvaluacion) + '%');

            setPorcentajeAutoEvaluacion(autoEvaluacion);
            setPorcentajeJefeSeguimiento(jefeEvaluacion);

            descripcionEmpleado.text('');
            descripcionEmpleado.text(autoObservacion);

            descripcionJefe.text('');
            descripcionJefe.text(jefeObservacion);

            
            colorAutoevaluacion.next('div').html('&nbsp;' + autoEvaluacion + '%');
            resultadoEvaluacion.text(resultado);
            resultadoEvaluacionSeguimiento.text(resultado);
            comentarioEvaluacion.text(autoObservacion == '' || autoObservacion == null ? '-' : autoObservacion);

            btnEvaMetaGuardar.data({
                id,
                idUsuario,
                idJefe,
                idMeta: meta.id,
                idProceso: meta.idProceso,
                puedeAutoEvaluar,
                puedeJefeEvaluar
            });
            btnEvaMetaGuardar.removeClass("hidden");
            // btnEvaMetaGuardar.addClass("hidden");
            // if (puedeAutoEvaluar || puedeJefeEvaluar) {
            //     btnEvaMetaGuardar.removeClass("hidden");
            // }
            lstEvaMetaEvidencias = [];
            lstEvidencia.forEach(a => {
                let pos = a.source.indexOf(';base64,')
                    , type = a.source.substring(5, pos);
                lstEvaMetaEvidencias.push(new File([new Blob([a.source], { type })], a.nombre, { type }));
            });
        }
        setEvaMetaResultado = ( ) => {
            /*let resultado =+ txtEvaMetaResultado.val();
            txtEvaMetaResultado.val(maskNumero2D(resultado));*/
            let resultado = + txtEvaMetaResultado.val();
            txtEvaMetaResultado.val(parseFloat(resultado).toFixed(2));
        }
        setEvaMetaAutoIndicador = (txt) => {
            let calificacion = +txtEvaMetaAutoEvaluacion.val(),
                min = lstSemaforo.reduce((prev, curr) => prev.minimo < curr.minimo ? prev : curr).minimo,
                max = lstSemaforo.reduce((prev, curr) => prev.maximo > curr.maximo ? prev : curr).maximo;
            if (calificacion < min) {
                calificacion = min;
            }
            if (calificacion > max) {
                calificacion = max;
            }
            let indicador = lstSemaforo.find(semaforo => semaforo.minimo <= calificacion && semaforo.maximo >= calificacion);
            divEvaMetaAutoIndicador.css("background", `#${indicador.color}`);
            txtEvaMetaAutoEvaluacion.val(maskNumero2D(calificacion)); 

            if(txt === true) {
                colorAutoevaluacion.css('background', `#${indicador.color}`);
            }
        }
        setEvaMetaJefeIndicador = () => {
            let calificacion = +txtEvaMetaJefeEvaluacion.val(),
                min = lstSemaforo.reduce((prev, curr) => prev.minimo < curr.minimo ? prev : curr).minimo,
                max = lstSemaforo.reduce((prev, curr) => prev.maximo > curr.maximo ? prev : curr).maximo;
            if (calificacion < min) {
                calificacion = min;
            }
            if (calificacion > max) {
                calificacion = max;
            }
            let indicador = lstSemaforo.find(semaforo => semaforo.minimo <= calificacion && semaforo.maximo >= calificacion);
            divEvaMetaJefeIndicador.css("background", `#${indicador.color}`);
            txtEvaMetaJefeEvaluacion.val(maskNumero2D(calificacion));
        }
        setMetaSemaforoForm = lst => {
            divEvaMetaSemaforo.html("");
            lstSemaforo = lst;
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
                divEvaMetaSemaforo.append(divContenedor);
            });
        }
        setEvaMetaEvidencia = input => {
            if (input.files && input.files[0]) {
                evaMetaContador++;
                var reader = new FileReader();
                reader.onload = function (e) {
                    let nombreLimit = input.files[0].name.split('.').length - 1,
                        extension = input.files[0].name.split('.')[nombreLimit];
                    //input.files[0].name = `${idProceso} - Evidencia ${evaMetaContador}.${extension}`;
                    
                    let ext = extension.toUpperCase();
                    console.log(ext);
                    if (ext == "PDF" || ext == "JPEG" || ext == "JPG" || ext == "PNG" || ext == "RAR" || ext == "ZIP" ||
                        ext == "XLSX" || ext == "XLS" || ext == "DOCX" || ext == "DOC" || ext == "PPTX" || ext == "PPT"){
                        dtEvaMetaEvidencia.row.add({
                            id: 0,
                            contador: evaMetaContador,
                            //nombre: `Evidencia ${evaMetaContador}.${extension}`,
                            nombre: input.files[0].name,
                            extension: `.${extension}`,
                            source: input.files[0],
                            url: false,
                            btnMostrar: true
                        }).draw();
                        lstEvaMetaEvidencias.push(input.files[0]);
                        btnDescargarEvidenciasMetaFormMetaEvaluar.attr("disabled", true);
                        btnDescargarEvidenciasMetaEscritorio.attr("disabled", true);
                    } else {
                        //AlertaGeneral('Aviso', "Solamente se permite archivos con extensión: pdf, jpeg, jpg, png, rar y zip.");
                        Alert2Warning("Solamente se permite archivos con extensión: pdf, jpeg, jpg, png, rar y zip.");
                    }
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
        getFormEvaMetaEvaluacion = () => {
            let formData = new FormData(),
                lstEvidencia = dtEvaMetaEvidencia.rows().data().toArray(),
                data = btnEvaMetaGuardar.data(),
                obs = {
                    id: data.id,
                    idEvaluacion: data.idEvaluacion,
                    idMeta: data.idMeta,
                    idUsuario: data.idUsuario,
                    idJefe: data.idJefe,
                    idEvaluacion: +cboEvaMetaEvaluacion.val(),
                    resultado: +txtEvaMetaResultado.val(),
                    autoEvaluacion: +txtEvaMetaAutoEvaluacion.val(),
                    autoObservacion: txtEvaMetaAutoObservacion.val(),
                    jefeEvaluacion: +txtEvaMetaJefeEvaluacion.val(),
                    jefeObservacion: txtEvaMetaJefeObservacion.val(),
                    esActivo: true,
                    esAutoEvaluado: true,
                    esJefeEvaluado: data.puedeJefeEvaluar,
                    lstEvidencia: lstEvidencia.map(evidencia => ({
                        id: evidencia.id,
                        idObservacion: evidencia.idObservacion,
                        nombre: evidencia.nombre,
                    })),
                };
            formData.append("obs", JSON.stringify(obs));
            dtEvaMetaEvidencia.rows().data().each((i, j) => {
                formData.append("files[]", lstEvaMetaEvidencias[j]);
            });
            return formData;
        }
        setEvaMetaInputEvidencia = () => {
            fileEvaMetaEvidencia.val('');
            fileEvaMetaEvidencia.click();
        }
        showFormMetaEvaluar = (proceso, meta, permiteEliminarEvidencia, esUsuario) => {
            var column = dtEvaMetaEvidencia.column(2);
            column.visible(permiteEliminarEvidencia);
            column.visible(permiteEliminarEvidencia && esUsuario);

            idProceso = proceso.id;
            idMeta = meta.id;
            idEvaluacion = $('#cboEscEvaluacion').val();
            cboEvaMetaEvaluacion.fillCombo(getCboEvaluacionPorProceso, { idProceso }, true, null);
            cboEvaMetaEvaluacion.val(idEvaluacion);
            divEvaMetaForm.addClass("hidden");
            if (meta.esVobo) {
                idUsuario = meta.idUsuario;
                setEvaMetaObservacion($('#cboEscEvaluacion'));
                divEvaMetaForm.removeClass("hidden");
            }
        }
        setPorcentajeAutoEvaluacion = (autoEvaluacion) => {
            getColorProgressBar(autoEvaluacion);
            if (autoEvaluacion > -1) {
                progressBarAutoSeguimiento.attr("style", "width:" + autoEvaluacion + "%; background-color: " + colorProgressBar + " !important; min-width:10%;");
                progressBarAutoSeguimiento.text(parseFloat(autoEvaluacion).toFixed(2) + "%");
            } else {
                progressBarAutoSeguimiento.attr("style", "width:0%; background-color: #ff3939 !important; min-width:10%;");
                progressBarAutoSeguimiento.text("0.00%");
            }
        }
        setPorcentajeJefeSeguimiento = (jefeEvaluacion) => {
            getColorProgressBar(jefeEvaluacion);
            if (jefeEvaluacion > -1) {
                progressBarJefeSeguimiento.attr("style", "width:" + jefeEvaluacion + "%; background-color: " + colorProgressBar + " !important; min-width:10%;");
                progressBarJefeSeguimiento.text(parseFloat(jefeEvaluacion).toFixed(2) + "%");
            } else {
                progressBarJefeSeguimiento.attr("style", "width:0%; background-color: #ff3939 !important; min-width:10%;");
                progressBarJefeSeguimiento.text("0.00%");
            }
        }
        getColorProgressBar = (puntuacion) => {
            colorProgressBar = "";
            if (puntuacion >= 0.00 && puntuacion <= 70.00)
                colorProgressBar = "#ff3939";
            if (puntuacion >= 70.01 && puntuacion <= 80.00)
                colorProgressBar = "#ff6a00";
            if (puntuacion >= 80.01 && puntuacion <= 85.00)
                colorProgressBar = "#ffd93b";
            if (puntuacion >= 85.01 && puntuacion <= 90.00)
                colorProgressBar = "#bdd262";
            if (puntuacion >= 90.01 && puntuacion <= 100.00)
                colorProgressBar = "#36bcc2";
        }
        function initDataTblEvaMetaEvidencia() {
            dtEvaMetaEvidencia = tblEvaMetaEvidencia.DataTable({
                destroy: true,
                paging: false,
                searching: false,
                info: false,
                ordering: false,
                language: dtDicEsp, 
                columns: [
                    { data: 'nombre', title: 'Evidencia' },
                    {
                        data: 'contador', width: '2%', title: 'Mostrar', visible: true, createdCell: function (td, data, rowData, row, col) {
                            let archivo = rowData.nombre;
                            let esRar = archivo.includes(".rar");
                            let esZip = archivo.includes(".zip");
                            let esDoc = archivo.includes(".doc");
                            let esDocx = archivo.includes(".docx");
                            let esPptx = archivo.includes(".pptx");
                            let esPpt = archivo.includes(".ppt");
                            let esXls = archivo.includes(".xls");
                            let esXlsx = archivo.includes(".xlsx");

                            let btn = "";
                            if (esRar || esZip || esDoc || esDocx || esPptx || esPpt || esXls || esXlsx) {
                                $(td).html(btn);
                            } else {
                                btn = $("<button>", {
                                    class: `btn btn-success descargar`,
                                    html: `<i class='fa fa-file'></i>`
                                });
                                $(td).html(btn);
                            }
                        }
                    },
                    {
                        data: 'contador', title: 'Eliminar', width: '2%', visible: !txtVerComoActivado.prop('checked'), createdCell: function (td, data, rowData, row, col) {
                            let btn = $("<button>", {
                                class: `btn btn-danger eliminar`,
                                html: `<i class='fa fa-trash'></i>`
                            });
                            if(esAutoEvaluacion) { $(td).html(btn); } else { $(td).html(''); }
                        }
                    }
                ], initComplete: function (settings, json) {
                    tblEvaMetaEvidencia.on('click', '.eliminar', function (event) {
                        //v1                        
                        //let row = $(this).closest('tr');
                        //data = dtEvaMetaEvidencia.row(row).data();
                        //dtEvaMetaEvidencia.row(row).remove().draw();
                        //lstEvaMetaEvidencias.splice(row.index(), 1);

                        //v2
                        /*modalEliminarEvidencias.modal("show");
                        idEvidencia = $(this).closest("tr");
                        data = dtEvaMetaEvidencia.row(idEvidencia).data();*/
                        //dtEvaMetaEvidencia.row(idEvidencia).remove().draw();
                        //btnModalEliminarEvidencia.attr("data-id", Object.values(row));

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "¡Cuidado!",
                            width: '35%',
                            showCancelButton: true,
                            html: "<h3>¿Desea eliminar la evidencia seleccionada?</h3>",
                            confirmButtonText: "Confirmar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#d9534f",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.value) {
                                let row = $(this).closest('tr');
                                data = dtEvaMetaEvidencia.row(row).data();
                                dtEvaMetaEvidencia.row(row).remove().draw();
                                lstEvaMetaEvidencias.splice(row.index(), 1);
                                Alert2Exito("¡Se eliminó correctamente el archivo adjunto!")
                            }
                        });
                    });

                    tblEvaMetaEvidencia.on('click', '.descargar', function (event) {
                        let row = $(this).closest('tr'),
                            data = dtEvaMetaEvidencia.row(row).data();

                            // console.log(data);
                            // console.log(lstEvaMetaEvidencias);

                            if (data.url) {
                                //setVisorFile(lstEvaMetaEvidencias[row.index()], data.extencion);
                                $('#myModal').data().ruta = data.source;
                                $('#myModal').modal('show');
                            } else {
                                setVisorFile(data.source, data.extencion);
                            }
                    });
                }
            });
        }
        
        function fncEliminarEvidencia() {
            dtEvaMetaEvidencia.row(idEvidencia).remove().draw();
            lstEvaMetaEvidencias.splice(idEvidencia.index(), 1);
        }

        function DescargarEvidenciasMeta(esEscritorio) {
            // const idObservacion = btnDescargarEvidenciasMetaFormMetaEvaluar.attr("data-id");
            let idObservacion = "";
            if (esEscritorio){
                idObservacion = btnDescargarEvidenciasMetaEscritorio.attr("data-id");
            }else{
                idObservacion = btnDescargarEvidenciasMetaFormMetaEvaluar.attr("data-id");
            }
            if (idObservacion && idObservacion > 0) {
                location.href = `DescargarEvidenciasMeta?idObservacion=${idObservacion}`;
                Alert2Exito("¡Se descargó con éxito los archivos!")
            }
        }

        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno._FormMetaEvaluar = new _FormMetaEvaluar();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();