(function () {
    $.namespace('controlObra.InformeSemanal');

    InformeSemanal = function () {
        const btnAddAspecto = $('#btnAddAspecto');
        const btnDeleteAspecto = $('#btnDeleteAspecto');
        const aspecto = $('#aspecto');
        const btnreporte = $('#btnreporte');
        let ulAspectos = $('#ulAspectos');
        let listAspectos = [];
        let listOrdenesCambio = [];
        let nombreObra = '';

        const getDivisionCC = () => $.post('/ControlObra/ControlObra/ObtenerDivisionCC');


        //#region ORDENES DE CAMBIO
        $('[data-toggle="tooltip"]').tooltip();
        const actions = getActionsButtonsOrdenCambio(); //$("table td:last-child").html();
        // Append table with add row form on add new button click
        $(".ordenes").click(function () {
            $(this).attr("disabled", "disabled");
            var index = $("#tblOrdenes tbody tr:last-child").index();
            var row = '<tr>' +
                '<td><input type="text" class="form-control" name="numeroAdicional" id="numeroAdicional"></td>' +
                '<td><input type="date" class="form-control" name="fechaSolicitud" id="fechaSolicitud"></td>' +
                '<td><input type="text" class="form-control" name="tipoAdicional" id="tipoAdicional"></td>' +
                '<td><input type="date" class="form-control" name="fechaCompromiso" id="fechaCompromiso"></td>' +
                '<td><input type="text" class="form-control" name="responsable" id="responsable"></td>' +
                //'<td><input type="text" class="form-control" name="observaciones" id="observaciones"></td>' +
                '<td><textarea class="form-control" name="observaciones" id="observaciones"></textarea></td>' +
                '<td>' + actions + '</td>' +
                '</tr>';
            $("#tblOrdenes").append(row);
            $("#tblOrdenes tbody tr").eq(index + 1).find(".addOrdenCambio, .editOrdenCambio").toggle();
            $('[data-toggle="tooltip"]').tooltip();
        });
        // Add row on add button click
        $(document).on("click", ".addOrdenCambio", function () {
            var empty = false;
            var input = $(this).parents("tr").find('input[type="text"]');
            var dates = $(this).parents("tr").find('input[type="date"]');

            input.each(function () {
                if (!$(this).val()) {
                    $(this).addClass("error");
                    empty = true;
                } else {
                    $(this).removeClass("error");
                }
            });
            $(this).parents("tr").find(".error").first().focus();
            if (!empty) {
                input.each(function () {
                    $(this).parent("td").html($(this).val());
                });

                dates.each(function () {
                    // $(this).parent("td").html($(this).val());
                    $(this).attr('disabled', true)
                });
                $(this).parents("tr").find(".addOrdenCambio, .editOrdenCambio").toggle();
                $(".ordenes").removeAttr("disabled");
            }
        });
        // Edit row on edit button click
        $(document).on("click", ".editOrdenCambio", function () {
            $(this).parents("tr").find("td:not(:last-child)").each(function () {
                if ($(this).children('input').length > 0) {
                    $(this).attr('disabled', false)
                    $(this).html('<input type="date" class="form-control" value="' + $(this).children().val() + '">');
                }
                else if ($(this).children('textarea').length > 0) {
                    $(this).html('<textarea class="form-control">' + $(this).children().val() + '</textarea>');
                } else {
                    $(this).html('<input type="text" class="form-control" value="' + $(this).text() + '">');
                }
            });
            $(this).parents("tr").find(".addOrdenCambio, .editOrdenCambio").toggle();
            $(".ordenes").attr("disabled", "disabled");
        });
        // Delete row on delete button click
        $(document).on("click", ".deleteOrdenCambio", function () {
            debugger;
            $(this).parents("tr").remove();
            $(".ordenes").removeAttr("disabled");
        });
        //#endregion

        //#region RIESGOS
        const actionsRiesgos = getActionsButtonsRiesgo();

        $(".riesgos").click(function () {
            $(this).attr("disabled", "disabled");
            var index = $("#tblRiesgos tbody tr:last-child").index();
            var row = '<tr>' +
                '<td><input type="text" class="form-control" name="numero" id="numero"></td>' +
                '<td><input type="text" class="form-control" name="descripcionRiesgo" id="descripcionRiesgo"></td>' +
                '<td><textarea class="form-control" name="planAccion" id="planAccion"></textarea></td>' +
                '<td><input type="text" class="form-control" name="responsable" id="responsable"></td>' +
                '<td><input type="date" class="form-control" name="responsable" id="fechaCompromiso"></td>' +
                '<td>' + actionsRiesgos + '</td>' +
                '</tr>';
            $("#tblRiesgos").append(row);
            $("#tblRiesgos tbody tr").eq(index + 1).find(".addRiesgos, .editRiesgos").toggle();
            $('[data-toggle="tooltip"]').tooltip();
        });

        $(document).on("click", ".addRiesgos", function () {
            var empty = false;
            var input = $(this).parents("tr").find('input[type="text"]');
            var dates = $(this).parents("tr").find('input[type="date"]');

            input.each(function () {
                if (!$(this).val()) {
                    $(this).addClass("error");
                    empty = true;
                } else {
                    $(this).removeClass("error");
                }
            });
            $(this).parents("tr").find(".error").first().focus();
            if (!empty) {
                input.each(function () {
                    $(this).parent("td").html($(this).val());
                });

                dates.each(function () {
                    $(this).attr('disabled', true)
                });

                $(this).parents("tr").find(".addRiesgos, .editRiesgos").toggle();
                $(".riesgos").removeAttr("disabled");
            }
        });

        $(document).on("click", ".editRiesgos", function () {
            $(this).parents("tr").find("td:not(:last-child)").each(function () {
                if ($(this).children('input').length > 0) {
                    $(this).attr('disabled', false)
                    $(this).html('<input type="date" class="form-control" value="' + $(this).children().val() + '">');
                }
                else if ($(this).children('textarea').length > 0) {
                    debugger;
                    $(this).html('<textarea class="form-control">' + $(this).children().val() + '</textarea>');
                } else {
                    $(this).html('<input type="text" class="form-control" value="' + $(this).text() + '">');
                }
            });
            $(this).parents("tr").find(".addRiesgos, .editRiesgos").toggle();
            $(".riesgos").attr("disabled", "disabled");
        });

        $(document).on("click", ".deleteRiesgos", function () {
            $(this).parents("tr").remove();
            $(".riesgos").removeAttr("disabled");
        });
        //#endregion

        function init() {
            //#region FUNCIONES WIZARD 
            $('.nav-tabs > li a[title]').tooltip();
            $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {

                var $target = $(e.target);

                if ($target.parent().hasClass('disabled')) {
                    return false;
                }
            });
            $(".next-step").click(function (e) {

                var $active = $('.wizard .nav-tabs li.active');
                $active.next().removeClass('disabled');
                nextTab($active);

            });
            $(".prev-step").click(function (e) {

                var $active = $('.wizard .nav-tabs li.active');
                prevTab($active);

            });
            //#endregion

            //#region CARGAR CC / DIVISION PLANTILLA
            getDivisionCC().done(function (response) {
                if (response.success) {
                    //nombreObra = response.divisionCC.division
                    nombreObra = response.divisionCC.descripcion_cc;
                }
            });
            //#endregion

            //#region FUNCIONES BUTTONS
            btnAddAspecto.click(addAspectoList);
            btnDeleteAspecto.click(deleteAspectoList);
            //#endregion
        }

        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }

        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }

        function getActionsButtonsOrdenCambio() {
            let td = `                            
                    <a class="addOrdenCambio" title="Agregar" data-toggle="tooltip"><i class="fas fa-plus"></i></a>
                    <a class="editOrdenCambio" title="Editar" data-toggle="tooltip"><i class="fas fa-pencil-alt"></i></a>
                    <a class="deleteOrdenCambio" title="Eliminar" data-toggle="tooltip"><i class="fas fa-trash"></i></a>
                        `;
            return td;
        }

        function getActionsButtonsRiesgo() {
            let td = `                            
                    <a class="addRiesgos" title="Agregar" data-toggle="tooltip"><i class="fas fa-plus"></i></a>
                    <a class="editRiesgos" title="Editar" data-toggle="tooltip"><i class="fas fa-pencil-alt"></i></a>
                    <a class="deleteRiesgos" title="Eliminar" data-toggle="tooltip"><i class="fas fa-trash"></i></a>
                        `;
            return td;
        }

        function addAspectoList() {
            if (aspecto.val().trim() != '') {
                addRowAspectoList(ulAspectos, aspecto.val());
            }
            aspecto.val("");
            aspecto.focus();
        }

        function deleteAspectoList() {
            listAspectos.pop();

            $("p").remove();
            ulAspectos.find('li:last-child').remove();
            desactivarBotonEliminar();
        }

        function addRowAspectoList(ul, aspecto) {
            listAspectos.push(aspecto);

            let li = `
                        <li><i class="fa-li far fa-square"></i>${aspecto}</li>
                    `;

            ul.append(li);

            desactivarBotonEliminar();
        }

        function desactivarBotonEliminar() {
            if (listAspectos.length == 0)
                btnDeleteAspecto.prop('disabled', true)
            else
                btnDeleteAspecto.prop('disabled', false)
        }



        init();
    };

    $(document).ready(function () {
        controlObra.InformeSemanal = new InformeSemanal();
    });
})();