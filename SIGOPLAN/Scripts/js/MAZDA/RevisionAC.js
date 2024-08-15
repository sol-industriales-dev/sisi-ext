(function () {
    $.namespace('planActividades.capturaActividad');
    capturaActividad = function () {
        inputObs = $("#inputObs");
        divGaleria = $("#divGaleria");
        btnShowMdlAC = $("#btnShowMdlAC");
        btnGuardar = $('#btnGuardar');
        btnLimpiar = $("#btnLimpiar");
        selectArea = $("#selectArea");
        selTecnico = $("#selTecnico");
        selAyudante = $("#selAyudante");
        mdlEvidencia = $("#mdlEvidencia");
        selectEquipo = $("#selectEquipo");
        selectPeriodo = $("#selectPeriodo");
        inputTonelaje = $("#inputTonelaje");
        tblEvaporador = $("#tblEvaporador");
        tblCondensador = $("#tblCondensador");
        inputEvidencias = $("#inputEvidencias");
        function init() {
            initForm();
            btnLimpiar.click(setDefault);
            btnShowMdlAC.click(showModal);
            btnGuardar.click(GuardarRevision);
            inputEvidencias.change(setGalery);
            selectEquipo.change(setEquipoAC);
            fillTablas();
        }
        tblCondensador.on('click', 'a', function () {
            if ($(this).hasClass('notActive')) {
                $(this).removeClass('notActive').addClass('active');
                $(this).siblings().removeClass('active').addClass('notActive');
            }
        });
        tblEvaporador.on('click', 'a', function () {
            if ($(this).hasClass('notActive')) {
                $(this).removeClass('notActive').addClass('active');
                $(this).siblings().removeClass('active').addClass('notActive');
            }
        });
        function setEquipoAC() {
            let equipoID = $(this).val();
            if (equipoID.length != 0) {
                GetEquipoAC(equipoID).done(function (response) {
                    if (response.success) {
                        let data = response.data;
                        let areaID = selectArea.find('option[name="' + data.area + '"]').val();
                        inputTonelaje.val(data.tonelaje);
                        selectArea.val(areaID);
                        selectPeriodo.val(data.periodo);
                    }
                });
            } else {
                inputTonelaje.val(0);
                selectArea.val("");
                selectPeriodo.val("");
            }
        }
        function initCbo() {
            selectEquipo.fillCombo('/MAZDA/PlanActividades/GetEquiposList', null, false);
            selectArea.fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: 1 }, false);
            selectPeriodo.fillCombo('/MAZDA/PlanActividades/GetPeriodosList', null, false);
            selTecnico.fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', null, false);
            selAyudante.fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', null, false);
        }
        function GuardarRevision() {
            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/GuardarRevision");
            request.send(formData());
            request.onload = function (response) {
                if (request.status == 200)
                    AlertaGeneral("Aviso", "Revision guardada correctamente.");
            };
        }
        function GetAllActividadesAC() {
            return $.post("/MAZDA/PlanActividades/GetAllActividadesAC");
        }
        function GetEquipoAC(equipoID) {
            return $.post("/MAZDA/PlanActividades/GetEquipoAC", { equipoID: equipoID });
        }
        function fillTablas() {
            GetAllActividadesAC().done(function (response) {
                if (response.success) {
                    setRows("tblCondensador", response.lstCondensador);
                    setRows("tblEvaporador", response.lstEvaporador);
                }
            });
        }
        function validaGuardado() {
            let ban = true;
            $.each($("#fieldBusquedaCuadrillaAC > input, select"), function (i, e) {
                if (this.value.length == 0 || this.value == 0)
                    ban = false;
            });
            return ban;
        }
        function showModal() {
            if (validaGuardado())
                mdlEvidencia.modal("show");
        }
        function formData() {
            let formData = new FormData();
            formData.append("rev", JSON.stringify(getRevision()));
            formData.append("det[]", getDetalles());
            $.each(document.getElementById("inputEvidencias").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        function getDetalles() {
            let det = [];
            tblCondensador.find("tbody tr").each(function (idx, row) {
                let obs = $(this).find('input').val();
                obs = obs.length == 0 ? " " : obs;
                det.push(JSON.stringify({
                    id: 0,
                    revisionID: 0,
                    tipo: 1,
                    actividadID: $(this).data().idActividad,
                    realizo: false,
                    observaciones: 0,
                    estatus: getRadioValue('radRealizo' + idx)
                }));
            });
            tblEvaporador.find("tbody tr").each(function (idx, row) {
                let obs = $(this).find('input').val();
                obs = obs.length == 0 ? " " : obs;
                det.push(JSON.stringify({
                    id: 0,
                    revisionID: 0,
                    tipo: 2,
                    actividadID: $(this).data().idActividad,
                    realizo: false,
                    observaciones: 0,
                    estatus: getRadioValue('radRealizo' + idx)
                }));
            });
            return det;
        }
        function getRevision() {
            return {
                equipoID: selectEquipo.val(),
                tonelaje: inputTonelaje.val(),
                area: selectArea.val(),
                periodo: selectPeriodo.val(),
                tecnico: selTecnico.val(),
                ayudantes: selAyudante.val(),
                observaciones: inputObs.val(),
                estatus: true
            };
        }
        function setDefault() {
            $.each($("input, select"), function (i, e) {
                this.value = "";
            });
            inputTonelaje.val(0);
        }
        function initForm() {
            initCbo();
            setDefault();
        }
        function getRadioValue(tog) {
            return $('a[data-toggle="' + tog + '"]').data('title');
        }
        function setGalery() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURL(this);
            });
        }
        var readURL = function (input) {
            var reader = new FileReader();
            let item = $(document.createElement('div'));
            reader.onload = function (e) {
                item.addClass("mkr_SldItem");
                item.append(document.createElement('div'));
                item.find("div").addClass("thumbHolder");
                item.find(".thumbHolder").append(document.createElement("img"));
                item.find("img").attr("src", e.target.result);
                item.find("img").attr("width", "125px");
            }
            reader.readAsDataURL(input);
            divGaleria.append(item);
        }
        function setRows(tbl, data) {
            for (i = 0; i < data.length; i++) {
                var row = document.createElement('tr');
                var celdaDesc = document.createElement('td');
                celdaDesc.textContent = data[i].descripcion;
                var celdaRealizo = document.createElement('td');
                celdaRealizo.classList.add('text-center');
                var divRadio = document.createElement('div');
                divRadio.classList.add('radioBtn');
                divRadio.classList.add('btn-group');
                $(celdaRealizo).append(divRadio);
                var a1 = document.createElement('a');
                a1.classList.add('btn');
                a1.classList.add('btn-success');
                a1.classList.add('notActive');
                a1.setAttribute('data-toggle', 'radRealizo' + i);
                a1.setAttribute('data-title', 'true');
                var i1 = document.createElement('i');
                i1.classList.add('fa');
                i1.classList.add('fa-check');
                $(a1).append(i1);
                $(divRadio).append(a1);
                var a2 = document.createElement('a');
                a2.classList.add('btn');
                a2.classList.add('btn-danger');
                a2.classList.add('active');
                a2.setAttribute('data-toggle', 'radRealizo' + i);
                a2.setAttribute('data-title', 'false');
                var i2 = document.createElement('i');
                i2.classList.add('fa');
                i2.classList.add('fa-close');
                $(a2).append(i2);
                $(divRadio).append(a2);
                var celdaObs = document.createElement('td');
                var inputObs = document.createElement('input');
                inputObs.classList.add('form-control');
                $(celdaObs).append(inputObs);
                $(row).data().idActividad = data[i].id;
                $(row).append(celdaDesc);
                $(row).append(celdaRealizo);
                $(row).append(celdaObs);
                $('#' + tbl + ' tbody').append(row);
            }
        }
        init();
    };

    $(document).ready(function () {
        planActividades.capturaActividad = new capturaActividad();
    })
    .ajaxStart(function () {
        $.blockUI({
            message: 'Procesando...',
            baseZ: 2000
        });
    })
    .ajaxStop(function () { $.unblockUI(); });
})();