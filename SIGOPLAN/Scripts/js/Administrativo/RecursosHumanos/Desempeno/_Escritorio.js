(() => {
    $.namespace('RecursosHumanos.Desempeno._Escritorio');
    _Escritorio = function () {
        var idProceso = 0;
        const mdlFormPeso = $('#mdlFormPeso');
        const mdlFormMeta = $('#mdlFormMeta');
        const divEscMetas = $('#divEscMetas');
        const btnEscAddMeta = $('#btnEscAddMeta');
        const cboEscProceso = $('#cboEscProceso');
        const lblEscProceso = $('#lblEscProceso');
        const btnEscEditPeso = $('#btnEscEditPeso');
        const lblEscMetaTotal = $('#lblEscMetaTotal');
        const lblEscEvaluacion = $('#lblEscEvaluacion');
        const cboEscEvaluacion = $('#cboEscEvaluacion');
        const btnEscMetaIndVobo = $('#btnEscMetaIndVobo');
        const divEscMetaEmpleado = $('#divEscMetaEmpleado');
        const mdlEscMetaIndividual = $('#mdlEscMetaIndividual');
        const divEscMetaIndMetaVobo = $('#divEscMetaIndMetaVobo');
        const divEscMetaIndMetaLabels = $('#divEscMetaIndMetaLabels');
        const lblEscMetaIndNombreMeta = $('#lblEscMetaIndNombreMeta');
        const divEscMetaIndMetaGraphs = $('#divEscMetaIndMetaGraphs');
        const divEscMetaIndMetaSemaforo = $('#divEscMetaIndMetaSemaforo');
        const divEscMetaIndMetaSemaforoEvaluacion = $('#divEscMetaIndMetaSemaforoEvaluacion');
        const darVoboMeta = new URL(window.location.origin + '/Administrativo/Desempeno/darVoboMeta');
        const getCboProceso = new URL(window.location.origin + '/Administrativo/Desempeno/getCboProceso');
        const getEmpleadosJefe = new URL(window.location.origin + '/Administrativo/Desempeno/getEmpleadosJefe');
        const getMetaIndividual = new URL(window.location.origin + '/Administrativo/Desempeno/getMetaIndividual');
        const getLstMetaPorProceso = new URL(window.location.origin + '/Administrativo/Desempeno/getLstMetaPorProceso');
        const getCboEvaluacionPorProceso = new URL(window.location.origin + '/Administrativo/Desempeno/getCboEvaluacionPorProceso');
        const tituloFechaSeguimiento = $('#tituloFechaSeguimiento');
        const diasFechaSeguimiento = $('#diasFechaSeguimiento');
        const mensajeFinalFechaSeguimiento = $('#mensajeFinalFechaSeguimiento');
        const indicadorDiasSeguimiento = $('#indicadorDiasSeguimiento');
        const btnNotificar = $('#btnNotificar');
        const modalEvaluacion = $('#modalEvaluacion');
        const mdlEvaluaciondescripcionPuesto = $('#mdlEvaluaciondescripcionPuesto');
        const mdlDescripcionMeta = $('#mdlDescripcionMeta');
        const mdlMetaEstrategica = $('#mdlMetaEstrategica');
        const btnNotificarMetas = $('#btnNotificarMetas');
        const btnVoBo = $('#btnVoBo');
        const lblPromedioGeneral = $("#lblPromedioGeneral");
        const btnDescargarEvidenciasMetaEscritorio = $("#btnDescargarEvidenciasMetaEscritorio");
        const modalNotificarEvaluacion = $("#modalNotificarEvaluacion");
        const btnModalNotificarEvaluacion = $("#btnModalNotificarEvaluacion");
        const btnModalCancelarNotificarEvaluacion = $("#btnModalCancelarNotificarEvaluacion");
        const indicadorJefeInmediato = $("#indicadorJefeInmediato");

        let clicEmpleado = 0;

        let init = () => {
            initFormEsc();
            btnVoBo.attr("disabled", true);
            cboEscProceso.change(setEscEvaluacion);
            btnEscEditPeso.click(showEscMetaPesos);
            btnEscMetaIndVobo.click(setDarVoboMeta);
            cboEscEvaluacion.change(setLstMetaPorProceso);
            $('.box-metas').on('click', function () {
                $('.box-metas').removeClass('clicked');
                $(this).toggleClass('clicked');
            });
            btnEscAddMeta.on('click', function () {
                if (cboEscEvaluacion.val() == null || cboEscEvaluacion.val() == undefined || cboEscEvaluacion.find('option').length == 0) {
                    //AlertaGeneral('Alerta', 'Primero seleccione una evaluación');
                    Alert2Warning("Primero seleccione una evaluación");
                }
                else {
                    mdlFormMeta.find('.modal-title').text('Agregar meta');
                    mdlFormMeta.find('#btnMetaGuardar').data('idusuario', $(this).data('idusuario'));
                    mdlFormMeta.modal('show');
                }
            });
            
            btnNotificar.click(function (e) { 
                //modalNotificarEvaluacion.modal("show");  
                Swal.fire({
                    position: "center",
                    icon: "info",
                    title: "Notificar",
                    width: '35%',
                    showCancelButton: true,
                    html: "<h3>¿Desea notificar la evaluación?</h3>",
                    confirmButtonText: "Confirmar",
                    confirmButtonColor: "#5cb85c",
                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#d9534f",
                    showCloseButton: true
                }).then((result) => {
                    if (result.value) {
                        Notificar($("#btnNotificar").data('empleadoid'), $("#btnNotificar").data('evaluacionid'), cboEscProceso.val());
                        Alert2Exito("¡Se notificó correctamente la evaluación!");
                    }
                });
            });
            btnModalNotificarEvaluacion.click(function (e) {
                
            });
            btnModalCancelarNotificarEvaluacion.click(function (e) { modalNotificarEvaluacion.modal("hide"); });

            btnNotificarMetas.on('click', function() {
                NotificarMetas(0, cboEscProceso.val());
            });

            mdlFormMeta.on('hide.bs.modal', function() {
                limpiarMetaForm();
            });

            btnVoBo.on('click', function() {
                Swal.fire({
                    position: "center",
                    icon: "warning",
                    title: "Notificar",
                    width: '35%',
                    showCancelButton: true,
                    html: "<h3>¿Desea notificar al usuario para que realice su evaluación?</h3>",
                    confirmButtonText: "Confirmar",
                    confirmButtonColor: "#5cb85c",
                    cancelButtonText: "Cancelar",
                    cancelButtonColor: "#d9534f",
                    showCloseButton: true
                }).then((result) => {
                    if (result.value) {
                        setDarVoboMetas();
                    }
                });
            });
            // fncFirstChildCboEvaluacion();
        };
        function fncFirstChildCboEvaluacion(idEvaluacionFocus){
            let evaluacionesLength = $('#cboEscEvaluacion option').length; // TODO
            console.log(evaluacionesLength);
            if (evaluacionesLength > 0){
                // cboEscEvaluacion[0].selectedIndex = 0;
                // cboEscEvaluacion.trigger("change");
            }
        }
        async function setDarVoboMeta() {
            try {
                let idMeta = btnEscMetaIndVobo.data().id;
                response = await ejectFetchJson(darVoboMeta, { idMeta });
                if (response.success) {
                    ConfirmacionGeneral("Confirmación", `Meta ${lblEscMetaIndNombreMeta.text()} disponible para evaluar.`);
                    mdlEscMetaIndividual.modal('hide');
                    setEscComboProceso();
                } else {
                    //AlertaGeneral("Aviso", response.message);
                    Alert2Error(response.message);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        async function setDarVoboMetas() {
            try {
                let empleado = getDataUsuarioSeleccionado();
                let procesoIdVoBo = cboEscProceso.val();
                let usuarioIdVoBo = empleado.idUsuario;
                let evaluacionIdVoBo = cboEscEvaluacion.val();

                response = await ejectFetchJson('/Desempeno/VoBoMetas', {
                    idProceso: procesoIdVoBo,
                    idEmpleado: usuarioIdVoBo,
                    idEvaluacion: evaluacionIdVoBo
                });
                if (response.Success) {
                    setLstMetaPorProceso();
                    //ConfirmacionGeneral('Confirmación', 'Se envió alerta al usuario para que realice la evaluación');
                    Alert2Exito("¡Se notificó correctamente al usuario!");
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }
            catch(o_O) {
                //AlertaGeneral('Aviso', o_O.message);
                Alert2Error(o_O.message);
            }
        }
        setEmpleadosJefe = async posicion => {
            try {
                response = await ejectFetchJson(getEmpleadosJefe);
                if (response.success) {
                    setEscPanelEmpleados(response.lst, posicion);
                    setEscComboProceso();
                    indicadorJefeInmediato.text(response.jefe);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message)
                Alert2Error(o_O.message);
            }
        };
        setLstMetaPorProceso = async () => { //ERROR VOBO
            // try {
                let idEvaluacion = +cboEscEvaluacion.val(),
                    usuario = getDataUsuarioSeleccionado();
                setEscMeta();
                response = await ejectFetchJson(getLstMetaPorProceso, {
                    idProceso,
                    idUsuario: usuario.idUsuario,
                    idEvaluacion,
                    esVobo: usuario.esVobo,
                    esCalificar: usuario.esCalificar,
                    esJefe: usuario.esJefe,
                    esRevisados: usuario.esRevisados
                });
                if (response.success) {
                    //Eliminar restricciones del usuario
                    //-->
                    response.notificado = false;
                    //<--
                    if((!usuario.esVobo || !response.notificarMetas)) { btnEscEditPeso.hide(); }
                    setEscPanelEmpleadosIndicadores(response.lstEmpleadoIndicadores);
                    // console.log(response.lst);
                    setEscPanelMetas(response.lst, response.lstSemaforo, response.notificado, response.notificarMetas, response.editarMetas, response.puedeDarVoBo, response.esPeriodoActual, response.esProcesoActual);
                    //comentar la restriccion para no poder agregar mas metas
                    //-->
                    if(response.permiteAgregarMasMetas /*|| !response.editarMetas*/) {
                        btnEscAddMeta.show();
                    } else {
                        btnEscAddMeta.hide();
                    }
                    if(response.esPeriodoActual /*|| !response.editarMetas*/) {
                        // btnEscAddMeta.show();
                        // btnEscEditPeso.show();
                        // btnNotificar.show();
                        // btnNotificarMetas.show();
                    } else {
                        //btnEscAddMeta.hide();
                        //btnEscEditPeso.hide();
                        btnNotificar.hide();
                        //btnNotificarMetas.hide();
                    }
                    if(!response.esProcesoActual) {
                        btnEscAddMeta.hide();
                        btnNotificarMetas.hide();
                    }
                    //<--
                    lblEscMetaTotal.text('Peso total: ' + response.totalPesoMetas);
                }

                if(!usuario.esVobo) {
                    btnVoBo.hide();
                }
                else {
                    btnVoBo.attr('disabled', true);
                }
            // } catch (o_O) { divEscMetas
            //     //AlertaGeneral('Aviso', o_O.message)
            //     Alert2Error("3: " + o_O.message);
            // }
        };
        setMetaIndividual = async (idMeta, permiteEliminarEvi, esUsuario) => {
            try {
                divEscMetaIndMetaGraphs.html("");
                response = await ejectFetchJson(getMetaIndividual, { idMeta });
                let { proceso, success, meta, lstSemaforo, lstSeguimientos, lstEvaluaciones } = response;
                if (success) {
                    
                    if(!permiteEliminarEvi){
                        //console.log('falsefalsefalsefalse');
                        $('#tblEvaMetaEvidencia').appendTo('#divTblSeguimiento');
                    }else{
                        //console.log('truetruetruetruetrue');
                        $('#tblEvaMetaEvidencia').appendTo('#divTblEvaluacionEvidencia');
                    }

                    setFormMetaIndividual(meta);
                    setFormMetaIndvSemaforo(lstSemaforo);
                    setFormMetaIndGraphs(lstSeguimientos, lstEvaluaciones);
                    showFormMetaEvaluar(proceso, meta, permiteEliminarEvi, esUsuario);
                }
            } catch (o_O) { 
                //AlertaGeneral('Aviso', o_O.message)
                Alert2Error(o_O.message);
            }
        };
        setFormMetaIndividual = ({ id, nombre, descripcion, estrategia, esVobo, puedeDarVobo, peso, notificado }) => {
            lblEscMetaIndNombreMeta.text(nombre);
            divEscMetaIndMetaLabels.html(`
            <label>Descripción</label>
            <p>${descripcion}</p>
            <label>Estrategia</label>
            <p>${estrategia}</p>
            <label>Peso</label>
            <p>${peso}</p>`);
            if(!divEscMetaIndMetaVobo.hasClass('hidden')) {
                divEscMetaIndMetaVobo.addClass("hidden");
            }
            divEscMetaIndMetaGraphs.addClass("hidden");
            if (esVobo) {
                divEscMetaIndMetaGraphs.removeClass("hidden");
            } else {
                if (puedeDarVobo && notificado) {
                    divEscMetaIndMetaVobo.removeClass("hidden");
                }
            }
            btnEscMetaIndVobo.data().id = id;
        };
        setFormMetaIndvSemaforo = (lst) => {
            divEscMetaIndMetaSemaforo.html("<label>Semáforo</label>");
            divEscMetaIndMetaSemaforoEvaluacion.html("<label>Semáforo</label>");
            
            lst.forEach(semaforo => {
                let divContenedor = $('<div>', {
                    class: 'input-group'
                }),
                    divColor = $('<span>', {
                        class: 'dot',
                        style: `background: #${semaforo.color}`
                    }),
                    lblPorcentaje = $('<label>', {
                        html: `&nbsp;${maskNumero2D(semaforo.minimo)}% - ${maskNumero2D(semaforo.maximo)}%`,
                    });

                let color = $("<span />", { class: "dot", style: `background: #${semaforo.color}` });
                let porcentaje = $("<label />", { html: `&nbsp;${maskNumero2D(semaforo.minimo)}% - ${maskNumero2D(semaforo.maximo)}%` });

                divEscMetaIndMetaSemaforo.append("<br>");
                divEscMetaIndMetaSemaforo.append(color);
                divEscMetaIndMetaSemaforo.append(porcentaje);

                divEscMetaIndMetaSemaforoEvaluacion.append("<br>");
                divEscMetaIndMetaSemaforoEvaluacion.append(divColor);
                divEscMetaIndMetaSemaforoEvaluacion.append(lblPorcentaje);
            });
        }
        setFormMetaIndGraphs = (lstSeguimientos, lstEvaluaciones) => {
            try {
                mixedChartEscMetaIndGraphs = Highcharts.chart('divEscMetaIndMetaGraphs',
                    {
                        title: { text: '' },
                        xAxis: { categories: lstSeguimientos },
                        yAxis: {
                            title: { text: '' },
                            min: 0,
                            max: 100,
                            startOnTick: false,
                            endOnTick: false
                        },
                        plotOptions: {
                            series: {
                                label: {
                                    connectorAllowed: false
                                }
                            }
                        },
                        series: lstEvaluaciones,
                        responsive: {
                            rules: [{
                                condition: { maxWidth: 93 },
                                chartOptions: {
                                    legend: {
                                        layout: 'horizontal',
                                        align: 'center',
                                        verticalAlign: 'bottom'
                                    },

                                }
                            }]
                        },
                        credits: {
                            enabled: false
                        }
                    });
                divEscMetaIndMetaGraphs.css("overflow", "");
            } catch (error) {
            }
        };

        setEscPanelEmpleados = (lst, posicion) => {
            //
            const parametrosUrl = new URLSearchParams(window.location.search);
            //
            divEscMetaEmpleado.html("");
            let ul = $('<ul>', { class: 'list-group' });
            let contador = 0;
            lst.forEach(empleado => {
                if(contador == 1 && lst.length > 1) {
                    let li = $('<li>', { id: 'tituloEmpleados', class: 'list-group-item'});
                    let tituloEmpleados = '<div><h4>Mis empleados</h4></div>';
                    li.css('padding-left', '0px');
                    li.css('padding-right', '0px');
                    li.css('border', '0px');
                    li.css('margin-top', '10px');
                    li.css('padding-bottom', '2px');
                    li.append(tituloEmpleados);
                    ul.append(li);
                }
                let active = parametrosUrl.has('info') && posicion == -1 ? empleado.idUsuario == parametrosUrl.get('info') ? ' active' : '' : '';
                let li = $('<li>', { class: 'list-group-item col-sm-12 empleado ' +  active });
                empleado.esVobo = true;
                empleado.esCalificar = true;
                empleado.esJefe = true;
                empleado.esRevisados = true;
                li.data(empleado);
                li.append(`
                <div class="col-sm-2"><i class="far fa-user-circle fa-4x"></i></div>
                    <div class="col-sm-6">
                        <label>${empleado.nombre}</label>
                    </div>
                </div>`);
                ul.append(li);
                contador++;
            });
            divEscMetaEmpleado.append(ul);
            if(!parametrosUrl.has('info') && posicion == -1) {
                divEscMetaEmpleado.find('li:first').addClass('active');
            }
            if(posicion != -1) {
                let posActual = divEscMetaEmpleado.find('ul li.active').index();
                divEscMetaEmpleado.find('ul li.active').removeClass('active');
                divEscMetaEmpleado.find(`li:eq(${posicion})`).addClass('active');
                posActual = divEscMetaEmpleado.find('ul li.active').index();
            }
            // divEscMetaEmpleado.find('li:first').addClass('active');
        };

        setEscPanelEmpleadosIndicadores = lst => {
            // try {
                let posicionSeleccionado = divEscMetaEmpleado.find("ul li.active").index(),
                    lstData = getLstDataEmpleado();
                if (posicionSeleccionado == 1) { posicionSeleccionado += 1; }
                divEscMetaEmpleado.html("");
                let ul = $('<ul>', { class: 'list-group' });
                let contador = 0;
                let esActivo = lst[0].estatus;
                if (esActivo) {
                    lst.forEach(empleado => {
                        let li = null;
                        if (contador == 1 && lst.length > 1) {
                            li = $('<li>', { id: 'tituloEmpleados', class: 'list-group-item' });
                            let tituloEmpleados = '<div><h4>Mis empleados</h4></div>';
                            li.css('padding-left', '0px');
                            li.css('padding-right', '0px');
                            li.css('border', '0px');
                            li.css('margin-top', '10px');
                            li.css('padding-bottom', '2px');
                            li.append(tituloEmpleados);
                            ul.append(li);
                        }
                        li = $('<li>', { class: 'list-group-item empleado' }),
                        data = lstData.find(data => data.idUsuario == empleado.idUsuario);
                        if (data == null){
                            data = {
                                esVobo: false,
                                esCalificar: false,
                                esJefe: false,
                                esRevisados: false
                            };
                            // console.log("Data es null");
                        }
                        // esJefe = empleado.esJefe;
                        empleado.esVobo = data.esVobo;
                        // console.log(data.esVobo);
                        empleado.esCalificar = data.esCalificar;
                        // empleado.esJefe = data.esJefe;
                        empleado.esRevisados = data.esRevisados;
                        empleado.esJefe = data.esJefe;
                        li.data(empleado);
                        li.css('padding-left', '0px');
                        li.css('padding-right', '0px');
                        let panel = `<div class="row vertical-align"><div class="col-sm-2">
                    <i class="far fa-user-circle fa-3x"></i>
                </div>
                <div class="col-sm-6">
                    <h5>${empleado.nombre}</label>
                </div><div class="col-sm-4 text-right">`;
                        let notificaciones = 0;
                        notificaciones += empleado.noVobo > 0 ? 1 : 0;
                        notificaciones += empleado.noCalificar > 0 ? 1 : 0;
                        notificaciones += empleado.noJefe > 0 ? 1 : 0;
                        notificaciones += empleado.noRevisados > 0 ? 1 : 0;
                        panel += setBurbujaIcono("far fa-flag", empleado.noVobo, "Vobos faltantes", data.esVobo);
                        panel += setBurbujaIcono("fas fa-tasks", empleado.noCalificar, "Autoevaluaciones faltantes", data.esCalificar);
                        panel += setBurbujaIcono("fas fa-toolbox", empleado.noJefe, "Evaluaciones del jefe faltantes", data.esJefe);
                        panel += setBurbujaIcono("fas fa-check", empleado.noRevisados, "Evaluaciones completas", data.esRevisados);
                        panel += '</div></div>';
                        li.append(panel);
                        ul.append(li);
                        contador++;
                    });
                } else {
                    $("#menuCalendario").css("display", "none");
                }
                divEscMetaEmpleado.append(ul);
                divEscMetaEmpleado.find(`li:eq(${posicionSeleccionado})`).addClass('active');
                divEscMetaEmpleado.find('.empleado').on('click', function () {
                    clicEmpleado = 1;
                    let liEmpleado = divEscMetaEmpleado.find('.empleado.active').index();
                    divEscMetaEmpleado.find('.empleado').removeClass('active');
                    $(this).addClass('active');

                    if (liEmpleado == $(this).index()) {
                        setLstMetaPorProceso();
                    }
                    else {
                        setEscComboProceso();
                    }
                    // fncFirstChildCboEvaluacion();
                });
                divEscMetaEmpleado.find('.burbuja').on('click', function () {
                    $(this).toggleClass('checked');
                    let panel = $(this).closest('.empleado')
                    let data = panel.data();

                    // let flag = panel.find(".fa-flag").closest(".burbuja").hasClass("checked");
                    // let tasks = panel.find(".fa-tasks").closest(".burbuja").hasClass("checked");
                    // let toolbox = panel.find(".fa-toolbox").closest(".burbuja").hasClass("checked");
                    // let check = panel.find(".fa-check").closest(".burbuja").hasClass("checked");// true;

                    //
                    let flag = panel.find(".fa-flag").closest(".burbuja");
                    let tasks = panel.find(".fa-tasks").closest(".burbuja");
                    let toolbox = panel.find(".fa-toolbox").closest(".burbuja");
                    let check = panel.find(".fa-check").closest(".burbuja");

                    console.log(flag);
                    console.log(tasks);
                    console.log(toolbox);
                    console.log(check);

                    flag = flag.length <= 0 ? true : flag.hasClass("checked");
                    tasks = tasks.length <= 0 ? true : tasks.hasClass("checked");
                    toolbox = toolbox.length <= 0 ? true : toolbox.hasClass("checked");
                    check = check.length <= 0 ? true : check.hasClass("checked");// true;
                    //

                    // if (flag.length <= 0) { flag = true; }
                    // if (tasks.length <= 0) { tasks = true; }
                    // if (toolbox.length <= 0) { toolbox = true; }
                    // if (check.length <= 0) { check = true; }
                    
                    data.esVobo = flag;
                    data.esCalificar = tasks;
                    data.esJefe = toolbox;
                    data.esRevisados = check;
                });
            // } catch (ex) {
            //     Alert2Error("2: " + ex.message);
            // }
        };
        setBurbujaIcono = (iconoClass, contador, title, esChecked /*,colorClass, burbujaClass*/) => {
            /*<i class="fa fa-circle fa-stack-2x ${colorClass}"></i>*/
            if(contador > 0) {
                return `<span class="burbuja ${esChecked ? 'checked' : ''} " data-toggle="tooltip" title="${title}" >
                            <span class="fa-stack fa-5x has-badge" data-count="${contador}">
                                
                                <i class="fa fa-circle fa-stack-2x" style="color:black"></i>
                                <i class="${iconoClass} fa-stack-1x fa-inverse"></i>
                            </span>
                        </span>`;
            } return '';
        }
        getLstDataEmpleado = () => {
            let lst = [];
            divEscMetaEmpleado.find('.empleado').each((i, empleado) => {
                lst.push($(empleado).data());
            });
            return lst;
        }
        getDataUsuarioSeleccionado = () => {
            let tarjeta = divEscMetaEmpleado.find('ul li.empleado.active');
            if (tarjeta.length == 0) {
                return {
                    idUsuario: 0
                }
            } else {
                data = tarjeta.data();
                return data;
            }
        }
        setEscPanelMetas = (lst, lstSemaforo, notificado, notificarMetas, editarMetas, puedeDarVoBo, esPeriodoActual, esProcesoActual) => {
            // try {
                let PromedioGeneral = 0;
                let SumaPorcentajes = 0;
                let contador = 0;
                let activarBtnNotificar = notificado;
                let ul = $('<ul>', { class: 'list-group' });
                divEscMetas.html("");
                if (lst.length == 0) {

                    let li = '' +
                        '<li class="list-group-item">' +
                        '<p>No hay metas para mostrar.</p>' +
                        //  '<div class="progress">' +
                        //  '<div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width:0%; background-color:#ff3939 !important; min-width:5%;">0%</div>' +
                        //  '</div>' +
                        '</li>';
                    ul.append(li);
                    btnNotificarMetas.hide();
                }
                lst.forEach(meta => { //TODO
                    // console.log(meta);
                    btnNotificar.data('empleadoid', meta.esUsuario ? 0 : meta.idUsuario);
                    let dias = $("#diasFechaSeguimiento").text();
                    let disabled = "";
                    //dias <= 0 ? disabled = "disabled" : disabled = "";

                    let btnVisible = "";
                    if (!puedeDarVoBo) {
                        btnVisible = "style='display:none;'";
                    } else {
                        btnVisible = "style='display:inline;'";
                    }

                    let btnEliminarMetaDisabled = "";
                    let cantMetas = 0;
                    cantMetas = lst.length;
                    //if (cantMetas == 1)
                        //btnEliminarMetaDisabled = "disabled";

                    let li = $('<li>', { class: 'list-group-item' }),
                        semaforo = lstSemaforo.find(semaforo => semaforo.minimo <= meta.evaluacion && semaforo.maximo >= meta.evaluacion);
                    li.data(meta);
                    let botonera = !esProcesoActual ? '' : (!meta.esVobo && editarMetas && ((meta.esUsuario) || (!meta.esUsuario))) || (meta.esVobo && !meta.esUsuario) ? `<p class="pull-right">
                    <button type='button' class='btn btn-warning edit' data-toggle='modal' data-target='#mdlFormMeta' data-idusuario="${meta.idUsuario}"> 
                        <i class='fa fa-edit'></i> Editar
                    </button>
                    <button type='button' class='btn btn-danger delete' ${btnEliminarMetaDisabled}>
                        <i class='fa fa-trash'></i> Eliminar
                    </button>

                </p> ${/*condicion para que el jefe pueda eliminar metas y evaluarlas*/meta.esVobo && !meta.esUsuario ? !esPeriodoActual ? '' : (!meta.esUsuario && meta.jefePuedeEvaluar) || (!meta.esUsuario && meta.puedeEvaluar) || (meta.esUsuario && meta.esVobo/*!meta.evaluadaPorJefe*/ && meta.puedeEvaluar /*!notificado*/) ?
                (!meta.esUsuario && meta.evaluadaPorJefe) ?// && meta.jefePuedeEvaluar && meta.evaluadaPorJefe) ?
                    '<p class="pull-right" style="margin-right:4px;"><button type="button" class="btn btn-success evaluar" title="Evaluar" ' + disabled + '><i class="far fa-edit"></i> Editar evaluación</button></p><p class="pull-right"></p>' :
                    '<p class="pull-right" style="margin-right:4px;"><button type="button" class="btn btn-success evaluar" title="Evaluar" ' + disabled + '>' + (meta.tieneAutoEvaluacion && meta.esUsuario ? '<i class="far fa-edit"></i> ' :
                        // '<i class="fas fa-check"></i> ') + (meta.evaluacion != 0 && meta.esUsuario ?
                        '<i class="fas fa-check"></i> ') + (meta.tieneAutoEvaluacion && meta.esUsuario ?
                            'Editar evaluación' : 'Evaluar') + '</button></p>' :
                (meta.esUsuario && meta.evaluadaPorJefe) || (!meta.esUsuario && !meta.puedeJefeEvaluar) ? '<p class="pull-right" style="margin-right:4px;"><i class="fas fa-check-double"></i> Evaluado</p>' :
                    '' : ''}` : !esPeriodoActual ? '' : (!meta.esUsuario && meta.jefePuedeEvaluar) || (!meta.esUsuario && meta.puedeEvaluar) || (meta.esUsuario && meta.esVobo/*!meta.evaluadaPorJefe*/ && meta.puedeEvaluar /*!notificado*/) ?
                            (!meta.esUsuario && meta.evaluadaPorJefe) ?// && meta.jefePuedeEvaluar && meta.evaluadaPorJefe) ?
                                '<p class="pull-right"><button type="button" class="btn btn-success evaluar" title="Evaluar" ' + disabled + '><i class="far fa-edit"></i> Editar evaluación</button></p><p class="pull-right"></p>' :
                                '<p class="pull-right"><button type="button" class="btn btn-success evaluar" title="Evaluar" ' + disabled + '>' + (meta.tieneAutoEvaluacion && meta.esUsuario ? '<i class="far fa-edit"></i> ' :
                                    // '<i class="fas fa-check"></i> ') + (meta.evaluacion != 0 && meta.esUsuario ?
                                    '<i class="fas fa-check"></i> ') + (meta.tieneAutoEvaluacion && meta.esUsuario ?
                                        'Editar evaluación' : 'Evaluar') + '</button></p>' :
                            (meta.esUsuario && meta.evaluadaPorJefe) || (!meta.esUsuario && !meta.puedeJefeEvaluar) ? '<p class="pull-right"><i class="fas fa-check-double"></i> Evaluado</p>' :
                                '';

                    li.append(`<div class="infoMeta">${botonera}
            <input type="checkbox" class="chkVoBo" value="${contador}" ${btnVisible}> <label class="meta" > ${meta.nombre}</label >
            <p><label>Peso:</label> ${meta.peso}</p></div>
            <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width:${meta.evaluacion}%; background-color: #${semaforo.color == 0 ? 'f5f5f5' : semaforo.color} !important; min-width:5%;">${meta.evaluacion}%</div>
            </div>`);

                    ul.append(li);
                    SumaPorcentajes += ((meta.evaluacion*meta.peso)/100);
                    contador++;
                });
                btnVoBo.attr("cantidad-metas", contador);
                //PromedioGeneral = SumaPorcentajes / contador;
                setPromedioGeneral(SumaPorcentajes);

                if (!activarBtnNotificar) {
                    btnNotificar.data('evaluacionid', cboEscEvaluacion.val());
                    btnNotificar.show();
                }
                else {
                    btnNotificar.hide();
                }
                if (notificarMetas) { btnNotificarMetas.show(); } else { btnNotificarMetas.hide(); }
                if (editarMetas) { btnEscEditPeso.show(); } else { btnEscEditPeso.hide(); }


                if (puedeDarVoBo) { btnVoBo.show(); } else { btnVoBo.hide(); }
                divEscMetas.append(ul);
                divEscMetas.find('.meta').on('click', function (event) {
                    let classElemento = $(event.target).attr("class").split(" ");
                    if (!["edit", "delete", "fa"].some(str => classElemento.includes(str))) {
                        let data = $(this).closest('li').data();
                        //console.log('data-meta');
                        //console.log(data);
                        setMetaIndividual(data.id, false, data.esUsuario);
                        mdlEscMetaIndividual.modal('show');
                    }
                });

                divEscMetas.on('click', '.chkVoBo', function () {
                    var checked = $(".chkVoBo:checked").length;
                    let cantidadMetas = btnVoBo.attr("cantidad-metas");
                    if (checked == cantidadMetas && puedeDarVoBo) {
                        btnVoBo.attr("disabled", false);
                    } else {
                        btnVoBo.attr("disabled", true);
                    }
                });

                divEscMetas.find('.edit').on('click', function () {
                    let data = $(this).closest('li').data();
                    mdlFormMeta.find('#btnMetaGuardar').data('idusuario', $(this).data('idusuario'));
                    setFormMeta(data);
                });
                divEscMetas.find('.delete').on('click', function () {
                    let data = $(this).closest('li').data();
                    setMetaId(data.id);
                    //AlertaAceptarRechazarNormal("Aviso", `La meta ${data.nombre} será eliminada.¿Deseas continuar ? `, setEliminarMeta, null)
                    Alert2AccionConfirmar("¡Cuidado!",
                        `La meta ${data.nombre} será eliminada. ¿Deseas continuar?`,
                        "Confirmar",
                        "Cancelar",
                        setEliminarMeta);
                });
                divEscMetas.find('.evaluar').on('click', function () {
                    
                    // $(".divEvaluacionJefe").hide();
                    // $(".divEvaluacionUsuario").hide(); 

                    let data = $(this).closest('li').data();
                    let empleado = getDataUsuarioSeleccionado();
                    setMetaIndividual(data.id, true, data.esUsuario);
                    mdlDescripcionMeta.text(data.descripcion);
                    mdlMetaEstrategica.text(data.estrategia);
                    modalEvaluacion.find('.modal-subtitle').text('Seguimiento: ' + cboEscEvaluacion.find('option:selected').text());
                    mdlEvaluaciondescripcionPuesto.text(empleado.nombre + (data.descripcionPuesto != '' && data.descripcionPuesto != null ? " - " + data.descripcionPuesto : ''));
                    btnDescargarEvidenciasMetaEscritorio.attr("disabled", false);
                    modalEvaluacion.modal('show');
                });
            // } catch (ex) {
            //     Alert2Error("1: " + ex.message);
            // }
        }
        showEscMetaPesos = () => {
            let empleado = getDataUsuarioSeleccionado();
            setLstMetaPesos(idProceso, empleado.idUsuario);
            mdlFormPeso.modal('show');
        }
        setEscEvaluacion = () => {
            let idEvaluacion = +cboEscEvaluacion.val();
            
            const parametrosUrl = new URLSearchParams(window.location.search);
            if (parametrosUrl.has('eva')) {
                idEvaluacion = parametrosUrl.get('eva');
            }

            var clean_uri = location.protocol + "//" + location.host + location.pathname;
            window.history.replaceState({}, document.title, clean_uri);

            idProceso = +cboEscProceso.val();
            cboEscEvaluacion.fillCombo(getCboEvaluacionPorProceso, { idProceso }, true, null);
            cboEscEvaluacion.val(idEvaluacion);
            if (cboEscEvaluacion.val() === null) {
                // idEvaluacion = cboEscEvaluacion.find("option:eq(0)").val();
                idEvaluacion = cboEscEvaluacion.find('option[data-prefijo="seleccionadoPorFecha"]').val();
                cboEscEvaluacion.val(idEvaluacion);
            }
            if (cboEscEvaluacion.val() === null) {
                idEvaluacion = cboEscEvaluacion.find("option:eq(0)").val();
                cboEscEvaluacion.val(idEvaluacion);
            }
            lblEscProceso.text(cboEscProceso.find('option:selected').text());
            cboEscEvaluacion.select2();
            setLstMetaPorProceso();
            // fncFirstChildCboEvaluacion();
            //fncFocusEvaluacionVigente(idProceso);
        }
        function fncFocusEvaluacionVigente(_idProceso){
            let idProceso = _idProceso
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Administrativo/Desempeno/getEvaluacionVigenteID",
                data: {idProceso: idProceso},
                success: function (response) {
                    if (!response.success) {
                        console.log(response.message);
                    }
                    else {
                        fncFirstChildCboEvaluacion(response.idEvaluacionFocus);
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
        setEscMeta = () => {
            lblEscEvaluacion.text(cboEscEvaluacion.find('option:selected').text());
            getFechaEvaluacion(cboEscProceso.val());
        }
        initFormEsc = () => {
            setEmpleadosJefe(-1);
        }
        setEscComboProceso = () => {
            let empleado = getDataUsuarioSeleccionado();
            btnEscAddMeta.data('idusuario', empleado.idUsuario);
            cboEscProceso.fillCombo(getCboProceso, { idEmpleado: empleado.idUsuario }, true, null);
            const parametrosUrl = new URLSearchParams(window.location.search);
            if (parametrosUrl.has('pro')) {
                cboEscProceso.val(parametrosUrl.get('pro'));
            }
            cboEscProceso.select2();

            setEscEvaluacion();
        }
        getFechaEvaluacion = (idProceso) => {
            if (idProceso == null) { idProceso = 0; }
            $.get('/Desempeno/DiasSeguimiento', {
                idProceso: idProceso
            }).always().then(response => {
                if (response.Success) {
                    if(response.Value.MensajeFinal == '' && response.Value.Dias == 0) {
                        tituloFechaSeguimiento.text('Sin información que mostrar');
                        diasFechaSeguimiento.text('');
                    }
                    else {
                        tituloFechaSeguimiento.text(response.Value.Titulo);
                        diasFechaSeguimiento.text(response.Value.Dias);

                        // if (response.Value.Dias <= 0){
                        //     $("#btnEscAddMeta").attr("disabled", true);
                        //     $("#txtEvaMetaResultado").attr("disabled", true);
                        //     $("#txtEvaMetaAutoEvaluacion").attr("disabled", true);
                        //     $("#txtEvaMetaAutoObservacion").attr("disabled", true);
                        //     $("#btnEvaMetaEvidencia").attr("disabled", true);
                        //     $("#btnDescargarEvidenciasMetaEscritorio").attr("disabled", true);
                        //     $("#btnEscEditPeso").attr("disabled", true);
                        //     $("#txtMetaNombre").attr("disabled", true);
                        //     $("#cboMetaEstrategia").attr("disabled", true);
                        //     $("#txtMetaDescripcion").attr("disabled", true);
                        //     $("#txtMetaPeso").attr("disabled", true);
                        //     $("#btnMetaLimpiar").attr("disabled", true);
                        //     //$("#btnMetaGuardar").attr("disabled", true);
                        //     $("#btnNotificarMetas").attr("disabled", true);
                        //     //$("#mdlbtnAceptar").attr("disabled", true);
                        //     $("#btnNotificar").attr("disabled", true);
                        // } else {
                        //     $("#btnEscAddMeta").attr("disabled", false);
                        //     $("#txtEvaMetaResultado").attr("disabled", false);
                        //     $("#txtEvaMetaAutoEvaluacion").attr("disabled", false);
                        //     $("#txtEvaMetaAutoObservacion").attr("disabled", false);
                        //     $("#btnEvaMetaEvidencia").attr("disabled", false);
                        //     $("#btnDescargarEvidenciasMetaEscritorio").attr("disabled", false);
                        //     $("#btnEscEditPeso").attr("disabled", false);
                        //     $("#txtMetaNombre").attr("disabled", false);
                        //     $("#cboMetaEstrategia").attr("disabled", false);
                        //     $("#txtMetaDescripcion").attr("disabled", false);
                        //     $("#txtMetaPeso").attr("disabled", false);
                        //     $("#btnMetaLimpiar").attr("disabled", false);
                        //     //$("#btnMetaGuardar").attr("disabled", false);
                        //     $("#btnNotificarMetas").attr("disabled", false);
                        //     //$("#mdlbtnAceptar").attr("disabled", false);
                        //     $("#btnNotificar").attr("disabled", false);
                        // }
                        
                        mensajeFinalFechaSeguimiento.text(response.Value.MensajeFinal);
                    }
                    if (response.Value.Estatus == 'verde') {
                        indicadorDiasSeguimiento.css('background-color', '#1abc9c');
                    }
                    if (response.Value.Estatus == 'amarillo') {
                        indicadorDiasSeguimiento.css('background-color', '#df5e35');
                    }
                    if (response.Value.Estatus == 'rojo') {
                        indicadorDiasSeguimiento.css('background-color', '#df5e35');
                    }
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }
        function Notificar(idEmpleado, idEvaluacion) {
            $.post('/Desempeno/Notificar', {
                idEmpleado,
                idEvaluacion,
                idProceso
            }).always().then(response => {
                if (response.Success) {
                    btnNotificar.hide();
                    setLstMetaPorProceso();
                }
                else {
                    //AlertaGeneral('Alerta', response.Message);
                    Alert2Error(response.Message);
                }
            }, error => {
                //AlertaGeneral('Error', null);
                Alert2Error("Error");
            });
        }
        function NotificarMetas(idEmpleado, idProceso) {
            let strText = "¿Desea noticiar a su jefe?"
            Swal.fire({
                position: "center",
                icon: "info",
                title: "Notificar",
                width: '35%',
                showCancelButton: true,
                html: "<h3>" + strText + "</h3>",
                confirmButtonText: "Confirmar",
                confirmButtonColor: "#5cb85c",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#d9534f",
                showCloseButton: true
            }).then((result) => {
                if (result.value) {
                    $.post('/Desempeno/NotificarMetas', {
                        idEmpleado,
                        idProceso
                    }).always().then(response => {
                        if (response.Success) {
                            btnNotificarMetas.hide();
                            btnEscEditPeso.hide();
                            setLstMetaPorProceso();
                            //ConfirmacionGeneral('Confirmación', 'Se envió la notificación a tu jefe');
                            Alert2Exito("¡Se notificó correctamente a tu jefe!");
                        }
                        else {
                            //AlertaGeneral('Alerta', response.Message);
                            Alert2Error(response.Message);
                        }
                    }, error => {
                        //AlertaGeneral('Error', null);
                        Alert2Error("Error");
                    });
                }
            });
        }

        function setPromedioGeneral(PromedioGeneral) {
            let color = "";
            if (PromedioGeneral >= 0.00 && PromedioGeneral <= 70.00){
                color = "#ff3939";
            }
            if (PromedioGeneral >= 70.01 && PromedioGeneral <= 80.00) {
                color = "#ff6a00";
            }
            if (PromedioGeneral >= 80.01 && PromedioGeneral <= 85.00) {
                color = "#ffd93b";
            }
            if (PromedioGeneral >= 85.01 && PromedioGeneral <= 90.00) {
                color = "#bdd262";
            }
            if (PromedioGeneral >= 90.01 && PromedioGeneral <= 100.00) {
                color = "#36bcc2";
            }
            if (PromedioGeneral > -1){
                lblPromedioGeneral.attr("style", "width:" + PromedioGeneral + "%; background-color: " + color + " !important; min-width:5%;");
                lblPromedioGeneral.text(parseFloat(PromedioGeneral).toFixed(2) + "%");
            }else{
                lblPromedioGeneral.attr("style", "width:0%; background-color: #ff3939 !important; min-width:5%;");
                lblPromedioGeneral.text("0.00%");
            }
        }

        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno._Escritorio = new _Escritorio();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();