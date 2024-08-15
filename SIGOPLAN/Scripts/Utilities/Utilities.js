/// <reference path="../../jquery-1.9.1.js" />
/// <reference path="../../jquery-1.9.1-vsdoc.js" />


(function ($) {
    $.ShowMyErrors = function (errors) {
        var msg,
            headerMessage = "Favor de corregir todas las áreas con error como se indica.</br> Una vez que haya terminado, haga click en aceptar, guardar o continuar para completar su formulario.",
            $this = null,
            Generator = {
                myDiv: $("<div></div>"),
                built: function () {
                    $this.myDiv.dialog({
                        title: 'Información incompleta.',
                        autoOpen: true,
                        modal: true,
                        height: 'auto',
                        width: 600,
                        resizable: false,
                        draggable: true,
                        position: 'top',
                        dialogClass: 'alert',
                        closeOnEscape: false,
                        stack: false,
                        buttons: {
                            'Aceptar': $this.onOk
                        },
                        open: $this.onOpen
                    });
                },
                onOk: function () {
                    $this.myDiv.dialog("close");
                },
                onOpen: function () {
                    $this.builtMessage();
                },
                builtMessage: function () {
                    msg = "<div class='dialog-error'><p style='padding-left: 5%;' align = 'justify'>" + headerMessage + "</p></br><table>";
                    if (!$.isArray(errors)) {
                        throw "No found array";
                    }
                    for (var i in errors) {
                        msg += "<tr><td><p> * " + errors[i] + "</p></td></tr>";
                    }
                    msg += "</table></div>";
                    $this.myDiv.html(msg);
                },
                init: function () {
                    $this = this;
                    $this.built();
                }
            }
        Generator.init();
    },
        $.fn.getAutocomplete = function (funSelect, params, url) {
            control = this;
            if (params == null) {
                params = { term: "" }
            }
            control.autocomplete({
                source: function (request, response) {
                    params.term = request.term
                    axiosClean.get(url, { params: params })
                        .then(res => response(res.data))
                        .catch(o_O => AlertaGeneral(o_O));
                },
                minLength: 0,
                select: funSelect
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };

        },
        $.fn.getAutocompleteParams = function (funSelect, params, url, funParam) {
            control = this;
            if (params == null) {
                params = { term: "" }
            }
            control.autocomplete({
                source: function (request, response) {
                    params = funParam();
                    params.term = request.term
                    axiosClean.get(url, { params: params })
                        .then(res => response(res.data))
                        .catch(o_O => AlertaGeneral(o_O));
                },
                minLength: 0,
                select: funSelect
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };

        },
        $.fn.getAutocompleteValid = function (funSelect, funChange, params, url) {
            control = this;
            if (params == null) {
                params = { term: "" }
            }
            control.autocomplete({
                source: function (request, response) {
                    params.term = request.term
                    axiosClean.get(url, { params: params })
                        .then(res => response(res.data))
                        .catch(o_O => AlertaGeneral(o_O));

                },
                minLength: 0,
                change: funChange,
                select: funSelect
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };

        },
        $.fn.getAutocompleteValidSubCon = function (funSelect, funChange, params, cc, url) {
            control = this;
            if (params == null) {
                params = { term: "", cc: cc }
            }
            console.log(params)
            control.autocomplete({
                source: function (request, response) {
                    params.term = request.term
                    axiosClean.get(url, { params: params })
                        .then(res => response(res.data))
                        .catch(o_O => AlertaGeneral(o_O));

                },
                minLength: 0,
                change: funChange,
                select: funSelect
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };

        },
        //$.fn.fillCombo = function (urlData, params, multiple, callback, async) {
        //    combo = this;
        //    if (urlData.items) {
        //        combo.children().remove();
        //        if (!multiple) {
        //            combo.append("<option value=''>--Seleccione--</option>");
        //            $.each(urlData.items, function () {
        //                combo.append("<option value='" + this.Value + "'>" + this.Text + "</option>");
        //            });
        //        }
        //    } else {
        //        $.ajax({
        //            url: urlData,
        //            type: 'POST',
        //            dataType: 'json',
        //            contentType: 'application/json; charset=utf-8',
        //            async: false,
        //            data: JSON.stringify(params),
        //            global: false,
        //            success: function (response) {
        //                if (response.success != 'False') {
        //                    combo.attr('disabled', false);
        //                    combo.children().remove();
        //                    if (!multiple)
        //                        combo.append("<option value=''>--Seleccione--</option>");
        //                    $.each(response.items, function () {
        //                        combo.append("<option value='" + this.Value + "' name='" + this.Text + "' data-prefijo='" + this.Prefijo + "'>" + this.Text + "</option>");
        //                    });
        //                } else if (response.message != '') {
        //                    alertMsg(response.message);
        //                }
        //                if (callback) {
        //                    callback();
        //                }
        //            },
        //            error: function (response) {
        //            }
        //        });
        //    }
        //},
        $.fn.fillComboItemsGroup = function (items, text, value) {
            combo = this;
            combo.children().remove();
            if (text !== undefined) {
                let value = text === null ? "" : text;
                text = text === null ? "--Seleccione--" : text;
                let option = $("<option>", {
                    value,
                    text,
                    selected: "selected"
                });
                if (text !== true) {
                    combo.append(option);
                }
            }
            items.forEach(({ label, options }) => {
                grupo = $('<optgroup>');
                grupo.attr('label', label);
                options.forEach(({ Value, Text, Prefijo }) => {
                    let option = $(`<option value="${Value}">${Text}</option>`);
                    option.data().prefijo = Prefijo;
                    grupo.append(option);
                });
                combo.append(grupo);
            });
            if (value !== undefined) {
                combo.find(`option[value="${value}"]`).attr("selected", true);
                combo.val(value);
            }
        }
        , $.fn.fillComboItems = function (items, text, value) {
            combo = this;
            combo.children().remove();
            if (text !== undefined) {
                let value = text === null ? "" : text;
                text = text === null ? "--Seleccione--" : text;
                let option = $("<option>", {
                    value,
                    text,
                    selected: "selected"
                });
                if (text !== true) {
                    combo.append(option);
                }
            }
            items.forEach(({ Value, Text, Prefijo }) => {
                let option = $(`<option value="${Value}">${Text}</option>`);
                option.data().prefijo = Prefijo;
                combo.append(option);
            });
            if (value !== undefined) {
                combo.find(`option[value="${value}"]`).attr("selected", true);
                combo.val(value);
            }
        }
        , $.fn.fillComboGroup = function (urlData, params, multiple, text, callback) {
            combo = this;
            $.ajax({
                url: urlData,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify(params),
                global: false,
                success: function (response) {
                    if (response.success) {
                        combo.children().remove();
                        if (text != undefined) {
                            var option = $("<option>");
                            option.val(text === null ? "--Seleccione--" : text);
                            option.text(text === null ? "--Seleccione--" : text);
                            combo.append(option);
                        }
                        $.each(response.items, function () {
                            grupo = $('<optgroup>');
                            grupo.attr('label', this.label);
                            $.each(this.options, function () {
                                if (this.Value !== null) {
                                    var option = $("<option>");
                                    option.val(this.Value);
                                    option.text(this.Text);
                                    option.data().prefijo = this.Prefijo;
                                    if (this.addClass == "selected") option.attr("selected", "selected");
                                    grupo.append(option);
                                }
                            })
                            combo.append(grupo);
                        });
                    } else if (response.message != '') {
                        AlertaGeneral("Aviso", response.message);
                    }
                    if (callback) {
                        callback();
                    }
                }
            });
        },

        $.fn.fillComboGroupSelectable = function (urlData, params, multiple, text, callback) {
            combo = this;
            combo.children().remove();
            switch (Object.prototype.toString.call(urlData)) {
                case '[object Array]':
                    fillItemsGroupSelectable(combo, urlData, multiple, text);
                    break;
                case '[object URL]':
                case 'string':
                    $.ajax({
                        url: urlData,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        async: false,
                        data: JSON.stringify(params),
                        global: false,
                        success: function (response) {
                            if (response.success) {
                                if (!multiple) {
                                    var option = $("<option>");
                                    option.val(text === null ? "--Seleccione--" : text);
                                    option.text(text === null ? "--Seleccione--" : text);
                                    combo.append(option);
                                }
                                $.each(response.items, function (i, e) {
                                    if (e.isGroup) {
                                        grupo = this.Selectable ? $('<option class="optionGroup_Selectable">') : $("<optgroup>");
                                        if (this.Selectable) {
                                            grupo.val(this.Value);
                                            grupo.text(this.Text);
                                            grupo.data().prefijo = this.Prefijo;
                                            combo.append(grupo);
                                        }
                                        else {
                                            grupo.attr('label', this.Text);
                                        }

                                        $.each(this.options, function () {
                                            if (this.Value !== null) {
                                                var option = $("<option>");
                                                option.addClass(this.addClass);
                                                option.val(this.Value);
                                                option.text("   " + this.Text);
                                                option.data().prefijo = this.Prefijo;
                                                if (e.Selectable) {
                                                    combo.append(option);
                                                }
                                                else {
                                                    grupo.append(option);
                                                }

                                            }
                                        })
                                        if (!this.Selectable) {
                                            combo.append(grupo);
                                        }
                                    }
                                    else {
                                        var option = $("<option>");
                                        option.addClass(this.addClass);
                                        option.val(this.Value);
                                        option.text(this.Text);
                                        option.data().prefijo = this.Prefijo;
                                        combo.append(option);
                                    }
                                });
                            } else if (response.message != '') {
                                AlertaGeneral("Aviso", response.message);
                            }
                        }
                    });
                    break;
                default:
                    break;
            }
            if (callback) {
                callback();
            }
        },

        $.fn.fillComboBox = function (urlData, parametros, texto, callback) {
            let comboBox = this;

            if (!urlData) {
                comboBox.children().remove();
                comboBox.append(`<option value="">${texto}</option>`);

                if (callback) {
                    callback();
                }
            } else if (urlData.items) {

            } else {
                $.get(urlData, parametros ?? {})
                    .then(response => {
                        if (response.success) {
                            comboBox.children().remove();

                            if (texto) {
                                comboBox.append(`<option value="">${texto}</option>`);
                            }
                            $.each(response.items, function () {
                                let data = '';
                                $.each(this.datas, function () {
                                    data += ` data-${this.data}="${this.valor}"`;
                                });
                                comboBox.append(`<option value="${this.valor}" ${data}>${this.texto}</option>`);
                            })

                            if (callback) {
                                callback();
                            }
                        } else {
                            AlertaGeneral('Aviso', response.message);
                        }
                    }, error => {
                        AlertaGeneral('Aviso', error.status + ' ' + error.statuText);
                    });
            }
        },

        $.fn.fillCombo = function (urlData, params, multiple, text, callback, async) {
            combo = this;
            if (urlData.items) {
                combo.children().remove();
                if (!multiple) {
                    if (text != null)
                        combo.append("<option value='" + text + "'>" + text + "</option>");
                    else
                        combo.append("<option value=''>--Seleccione--</option>");
                    $.each(urlData.items, function () {
                        if (this.Value != null) {
                            combo.append("<option value='" + this.Value + "' name='" + this.name + "' data-prefijo='" + this.Prefijo + "' data-comboId='" + this.Id + "'>" + this.Text + "</option>");
                        }
                    });
                }
            } else {
                $.ajax({
                    url: urlData,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    async: false,
                    data: JSON.stringify(params),
                    global: false,
                    success: function (response) {
                        if (response.success != 'False') {
                            combo.children().remove();
                            if (!multiple) {
                                if (text != null)
                                    combo.append("<option value='" + text + "'>" + text + "</option>");
                                else
                                    combo.append("<option value=''>--Seleccione--</option>");
                            }
                            $.each(response.items, function () {
                                if (this.Value != null) {
                                    combo.append("<option value='" + this.Value + "' name='" + this.name + "' data-prefijo='" + this.Prefijo + "' data-comboId='" + this.Id + "'>" + this.Text + "</option>");
                                }
                            });
                        } else if (response.message != '') {
                            AlertaGeneral("Aviso", response.message);
                        }
                        if (callback) {
                            callback();
                        }
                    }
                });
            }
        },
        $.fn.fillComboSeguridad = function (incContratista, division, multiple) {
            combo = this;
            combo.children().remove();

            if (division == 0) {
                division = null;
            }

            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify({ incContratista: incContratista, division: division }),
                global: false,
                success: function (response) {
                    let { success, items, message } = response;
                    if (success) {
                        if (multiple) {
                            combo.append('<option value="Todos">TODOS</option>');
                        }
                        else {
                            combo.append('<option value="">--Seleccione--</option>');
                        }

                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            combo.append(groupOption);
                        });
                    } else {
                        Alert2Error(`Alerta`, message);
                    }
                }
            });

        },
        $.fn.fillComboSeguridadSinContratistas = function (incContratista, division, multiple) {
            combo = this;
            combo.children().remove();

            if (division == 0) {
                division = null;
            }

            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify({ incContratista: incContratista, division: division }),
                global: false,
                success: function (response) {
                    let { success, items, message } = response;
                    if (success) {
                        if (multiple) {
                            combo.append('<option value="Todos">TODOS</option>');
                        }
                        else {
                            combo.append('<option value="">--Seleccione--</option>');
                        }

                        items.splice(1, 1);

                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            combo.append(groupOption);
                        });
                    } else {
                        Alert2Error(`Alerta`, message);
                    }
                }
            });

        },
        $.fn.fillComboSeguridadDivisionLineaNegocio = function (incContratista, multiple, listaDivisiones, listaLineasNegocio) {
            combo = this;
            combo.children().remove();

            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresasDivisionesLineas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify({ incContratista: incContratista, listaDivisiones, listaLineasNegocio }),
                global: false,
                success: function (response) {
                    let { success, items, message } = response;
                    if (success) {
                        if (multiple) {
                            combo.append('<option value="Todos">TODOS</option>');
                        }
                        else {
                            combo.append('<option value="">--Seleccione--</option>');
                        }

                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            combo.append(groupOption);
                        });
                    } else {
                        Alert2Error(`Alerta`, message);
                    }
                }
            });

        },
        $.fn.ObtenerComboCCAmbasEmpresas_SoloGrupos = function (incContratista, division, multiple) {
            combo = this;
            combo.children().remove();

            if (division == 0) {
                division = null;
            }

            $.ajax({
                url: '/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresas_SoloGrupos',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify({ incContratista: incContratista, division: division }),
                global: false,
                success: function (response) {
                    let { success, items, message } = response;
                    if (success) {
                        if (multiple) {
                            combo.append('<option value="Todos">TODOS</option>');
                        }
                        else {
                            combo.append('<option value="">--Seleccione--</option>');
                        }

                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            combo.append(groupOption);
                        });
                    } else {
                        Alert2Error(`Alerta`, message);
                    }
                }
            });

        },
        $.fn.fillComboActoCondicion = function (incContratista, division, multiple) {
            combo = this;
            combo.children().remove();

            if (division == 0) {
                division = null;
            }

            $.ajax({
                url: '/Administrativo/ActoCondicion/ObtenerComboCCAmbasEmpresas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,
                data: JSON.stringify({ incContratista: incContratista, division: division }),
                global: false,
                success: function (response) {
                    let { success, items, message } = response;
                    if (success) {
                        if (multiple) {
                            combo.append('<option value="Todos">TODOS</option>');
                        }
                        else {
                            combo.append('<option value="">--Seleccione--</option>');
                        }

                        items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            combo.append(groupOption);
                        });
                    } else {
                        Alert2Error(`Alerta`, message);
                    }
                }
            });

        },
        //$.fn.getAgrupador = function () {
        //    var _this = $(this);

        //    return _this.val();
        //},
        //$.fn.getEmpresa = function () {
        //    var _this = $(this);
        //    var empresa = $("option:selected", _this).attr("empresa");
        //    return empresa;
        //},
        $.fn.fillComboAsync = function (urlData, params, multiple, text, callback) {
            combo = this;
            if (urlData.items) {
                combo.children().remove();
                if (!multiple) {
                    if (text != null)
                        combo.append("<option value='" + text + "'>" + text + "</option>");
                    else
                        combo.append("<option value=''>--Seleccione--</option>");
                    $.each(urlData.items, function () {
                        if (this.Value != null) {
                            combo.append("<option value='" + this.Value + "' name='" + this.Text + "' data-prefijo='" + this.Prefijo + "'>" + this.Text + "</option>");
                        }
                    });
                }
            } else {
                $.ajax({
                    url: urlData,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    global: false,
                    success: function (response) {
                        if (response.success != 'False') {
                            combo.children().remove();
                            if (!multiple) {
                                if (text != null)
                                    combo.append("<option value='" + text + "'>" + text + "</option>");
                                else
                                    combo.append("<option value=''>--Seleccione--</option>");
                            }
                            $.each(response.items, function () {
                                if (this.Value != null) {
                                    combo.append("<option value='" + this.Value + "' name='" + this.Text + "' data-prefijo='" + this.Prefijo + "'>" + this.Text + "</option>");
                                }
                            });
                        } else if (response.message != '') {
                            AlertaGeneral("Aviso", response.message);
                        }
                        if (callback) {
                            callback();
                        }
                    }
                });
            }
        },
        $.fn.resetCombo = function () {
            var cbo = this;
            var options = cbo.find('option');
            cbo.clearCombo();

            $.each(options, function (index) {
                if (options[index].value == -1) {
                    options.splice(index, 1);
                }
            });
            $.each(options, function (index) {
                if (options[index].value != '')
                    cbo.append($('<option>', { value: options[index].value, text: options[index].text }));
            });
            cbo.val('');
            cbo.removeClass('error');
        },
        $.fn.setInfoCombo = function (id, name) {
            var cbo = this;
            var isFound = false;
            var options = cbo.find('option');
            cbo.clearCombo();

            $.each(options, function (index) {
                if (options[index].value == -1) {
                    options.splice(index, 1);
                }
            });

            $.each(options, function (index) {
                if (options[index].value == id)
                    isFound = true;
                if (options[index].value != '')
                    cbo.append($('<option>', { value: options[index].value, text: options[index].text }));
            });
            if (!isFound) {
                cbo.append($('<option>', { value: -1, text: name != undefined && name != null ? name : 'Registro desactivado, favor de seleccionar otro' }));
                cbo.val(-1);
            }
            else {
                cbo.val(id);
            }
        },
        $.addError = function (element, msg) {
            var myField = $(element), error = $("<span class='message' title='" + msg + "'></span>");
            myField.addClass('error');
            myField.parent().find(".message").remove();
            error.insertAfter(myField);
        },
        $.setDialogResult = function (type, title) {
            var dialog;
            switch (type) {
                case '1': dialog = $('#dSuccess'); break;
                case '2': dialog = $('#dError'); break;
                default: dialog = $('dSuccess'); break;
            }

            dialog.empty().append('<span>' + title + '</span>');
            dialog.fadeIn(2000);
            dialog.css("position", "absolute");
            window_width = $(window).width(); //Get the user's window's width
            window_height = $(window).height(); //Get the user's window's height
            dialog.css("left", (window_width - dialog.width()) / 1.15);
            dialog.css("top", (window_height - dialog.height()) / 18);
            dialog.fadeOut(3000);
            $.unblockUI();
        },
        /**
        * jQuery.namespace. this function adds namespace to our desired object
        *
        * @package jQuery Toolkit
        * @author mahdi pedramrazi
        *
        * @params {Array/String...}
        *
        * @copyright � 2010 http://jQueryToolkit.com | All rights reserved.
        * @license licensing@jquerytoolkit.com
        * http://www.jquerytoolkit.com/license
        *
        *
        * @example
        *
        * $.namespace(["liteframe.actions.controller","cake.action.jump"]);
        * //$.namespace("liteframe.actions.controller","cake.action.jump");
        * liteframe.actions.controller = function(){
        *         console.log("this is it");
        * };
        * cake.action.jump = function(){
        *     console.log("this is it 2");
        * };
        * new liteframe.actions.controller();
        * new cake.action.jump();
        *
        */

        $.namespace = function () {
            var args = (arguments.length == 1 && $.isArray(arguments[0])) ?
                arguments[0] : arguments, splittedDot, i = 0, j = 0, dynamicVar;

            if (!args.length) {
                return;
            }

            for (i = 0; i < args.length; i++) {
                splittedDot = args[i].split('.');
                dynamicVar = window;
                for (j = 0; j < splittedDot.length; j++) {
                    if (!dynamicVar[splittedDot[j]]) {
                        dynamicVar[splittedDot[j]] = {};
                        dynamicVar = dynamicVar[splittedDot[j]];
                    }
                }
            }
        },
        /**
         * Hace la primer letra matuscula
         * @param {string} string Cadena a capitalizar
         * @returns {string} Cadenaa capitalizada
         */
        String.prototype.Capitalize = function () {
            return this.charAt(0).toUpperCase() + this.slice(1)
        },
        /**
         * Obtiene el número de la semana del año
         * @returns {Number} Número de la semana
         */
        Date.prototype.noSemana = function () {
            var eneroUno = new Date(this.getFullYear() - 1, 11, 29);
            var hoy = new Date(this.getFullYear(), this.getMonth(), this.getDate());
            var diaDelAnio = ((hoy - eneroUno + 86400000) / 86400000);
            return Math.ceil(diaDelAnio / 7)
        },
        /**
         * Agrega días a la fecha
        * @returns {Date} Fecha con los días
         */
        Date.prototype.AddDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }

    /**
     * Regresa la fecha de un string en formato dd/MM/yyyy
     * @returns {Date} fecha
     */
    String.prototype.toDate = function () {
        let arrFecha = this.split('/'),
            fecha = new Date(arrFecha[2], +arrFecha[1] - 1, arrFecha[0]);
        return fecha;
    }
    /**
     * Convierte cadena con formato /Date(ticks)/ en fecha
     * @param {string} string Cadena de fecha
     * @returns {Date} Fecha
     */
    $.toDate = function (date) {
        switch (Object.prototype.toString.call(date)) {
            case '[object Date]':
                dia = date.getDate(), mes = (parseInt(date.getMonth()) + 1), anio = date.getFullYear();
                return ((dia < 10 ? "0" + dia : dia) + "/" + (mes < 10 ? "0" + mes : mes) + "/" + anio);
            case '[object String]':
                var f = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10)),
                    dia = f.getDate(), mes = (parseInt(f.getMonth()) + 1), anio = f.getFullYear();
                return ((dia < 10 ? "0" + dia : dia) + "/" + (mes < 10 ? "0" + mes : mes) + "/" + anio);
            case !date:
            default:
                return ""
        }
    },
        /**
         * Da el nombre del mes
         * @returns {String} nombre del mes
         */
        Date.prototype.NombreMes = function () {
            return this.toLocaleDateString("es-ES", { month: 'long' }).Capitalize()
        },
        /**
          *Verifica si la cadena es vacía
          * @param {string} string Cadena a verificar
          * @returns {bool} Respuesta
         */
        $.esStringVacio = function (string) {
            let sanitizado = string.replace(/\s/g, ''),
                longitud = sanitizado.length,
                esVacio = longitud === 0;
            return esVacio;
        },
        $.DialogConfirm = function (headerMsg) {
            var msg,
                headerMessage = headerMsg,
                $this = null,
                Generator = {
                    myDiv: $("<div class=\"modal fade\"><div class=\"modal-dialog\">div class=\"modal-content\"><div class=\"modal-header\"><button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button></div></div></div></div>"),
                    built: function () {
                        $this.myDiv.dialog({
                            title: headerMessage,
                            autoOpen: true,
                            modal: true,
                            height: 'auto',
                            width: 600,
                            resizable: false,
                            draggable: true,
                            position: 'top',
                            dialogClass: 'alert',
                            closeOnEscape: false,
                            stack: false,
                            buttons: {
                                'Aceptar': $this.onOk
                            },
                            open: $this.onOpen
                        });
                    },
                    onOk: function () {
                        $this.myDiv.dialog("close");
                    },
                    onOpen: function () {
                        $this.builtMessage();
                    },
                    builtMessage: function () {
                    },
                    init: function () {
                        $this = this;
                        $this.built();
                    }
                }
            Generator.init();
        },
        $.fn.clearCombo = function () {
            $(this).find("option[value!=''], optgroup").remove();
            return this;
        },

        $.getMaxDateToday = function () {

            var today = new Date();

            today.setHours(23);
            today.setMinutes(59);
            today.setSeconds(59);

            return today;
        },
        $.fn.onlyNumbers = function (x) {
            jQuery(this).keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });
        },

        $.isIExplorer = function () {
            var isIE11 = navigator.userAgent.indexOf(".NET CLR") > -1;
            var isIE11orLess = isIE11 || navigator.appVersion.indexOf("MSIE") != -1;
            return isIE11orLess;
        },
        $.DateRangeValidation = function (startDate, endDate) {

            var sD = (startDate.data("date")).split("/");
            var eD = (endDate.data("date")).split("/");

            var sDD = parseInt(sD[0]);
            var sDM = parseInt(sD[1]);
            var sDY = parseInt(sD[2]);
            var eDD = parseInt(eD[0]);
            var eDM = parseInt(eD[1]);
            var eDY = parseInt(eD[2]);

            if ((eDY > sDY) || (eDY == sDY) && (eDM > sDM) || (eDY == sDY) && (eDM == sDM) && (eDD >= sDD))
                return true;
            else
                return false;

            /*
            if ((eDY >= sDY) && (eDM >= sDM))
            return true;
            else
            return false;
            */
        }


})(jQuery);
(function ($) {
    $.fn.onEnter = function (func) {
        this.bind('keypress', function (e) {
            if (e.keyCode == 13) func.apply(this, [e]);
        });
        return this;
    };
})(jQuery);

fillItemsGroupSelectable = function (combo, options, multiple, text) {
    if (!multiple && text !== undefined) {
        var option = $("<option>");
        option.val(text === null ? "--Seleccione--" : text);
        option.text(text === null ? "--Seleccione--" : text);
        combo.append(option);
    }
    $.each(options, function (i, e) {
        if (e.isGroup) {
            grupo = this.Selectable ? $('<option class="optionGroup_Selectable">') : $("<optgroup>");
            if (this.Selectable) {
                grupo.val(this.Value);
                grupo.text(this.Text);
                grupo.data().prefijo = this.Prefijo;
                combo.append(grupo);
            }
            else {
                grupo.attr('label', this.Text);
            }
            $.each(this.options, function () {
                if (this.Value !== null) {
                    var option = $("<option>");
                    option.addClass(this.addClass);
                    option.val(this.Value);
                    option.text("   " + this.Text);
                    option.data().prefijo = this.Prefijo;
                    if (e.Selectable) {
                        combo.append(option);
                    }
                    else {
                        grupo.append(option);
                    }
                }
            })
            if (!this.Selectable) {
                combo.append(grupo);
            }
        }
        else {
            var option = $("<option>");
            option.addClass(this.addClass);
            option.val(this.Value);
            option.text(this.Text);
            option.data().prefijo = this.Prefijo;
            combo.append(option);
        }
    });
}

/**
 * Ejecuta el url que genera un archivo
 * @param {string} url Controlador al archivo
 * @return {File} Archivo solicitado
 */
function exportUrlToFile(url) {
    if (url.length > 0 || url.pathname.length > 0) {
        $.when($.blockUI({ message: "Preparando archivo a descargar", baseZ: 100000 })).then(function () {
            iframe = document.getElementById('iframeDownload');
            iframe.src = url;
            var timer = setInterval(function () {
                var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                    setTimeout(function () {
                        $.unblockUI();
                    }, 1200);
                    clearInterval(timer);
                    return;
                }
            }, 1000);
        });
    }
}

function NotificacionGeneral(titulo, mensaje) {
    $('.RenderBody').block({
        title: titulo,
        message: mensaje,
        fadeIn: 700,
        fadeOut: 1000,
        timeout: 3000,
        centerY: false,
        showOverlay: false,
        theme: true,
        baseZ: 999999,
        css: {
            // width: '350px',
            top: '10px',
            left: '',
            right: '10px',
            border: 'none',
            padding: '5px',
            opacity: 0.6,
            cursor: 'default',
            color: '#fff',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            'border-radius': '10px'
        }
    });
}

function AlertaGeneral(titulo, mensaje) {
    if (mensaje == null) {
        mensaje = "Error en el resultado de la petición, favor de intentar de nuevo.";
    }

    $("#dialogalertaGeneral").removeClass('hide');
    $("#txtComentarioAlerta").html(mensaje);
    var opt = {
        title: titulo,
        autoOpen: false,
        draggable: false,
        resizable: false,
        modal: true,
        maxWidth: 600,
        minWidth: 400,
        position: {
            my: "center",
            at: "center",
            within: $(".RenderBody")
        },
        buttons: [
            {
                text: "Aceptar",
                click: function () {
                    $("#dialogalertaGeneral").addClass('hide');
                    $(this).dialog("close");
                }

                // Uncommenting the following line would hide the text,
                // resulting in the label being used as a tooltip
                //showText: false
            }
        ]

    };
    var theDialog = $("#dialogalertaGeneral").dialog(opt);
    theDialog.dialog("open");
}
function AlertaAceptarRechazar(titulo, mensaje, cbAcetar, cbRechazar) {
    const mdlAceptarRechazar = $("#mdlAceptarRechazar"),
        mdltxtTitulo = $("#mdltxtTitulo"),
        mdltxtMensaje = $("#mdltxtMensaje"),
        mdlbtnAceptar = $("#mdlbtnAceptar"),
        mdlbtnCancelar = $("#mdlbtnCancelar");
    mdltxtTitulo.text(titulo !== null ? titulo : "Aviso");
    mdltxtMensaje.html(mensaje !== null ? mensaje : "Error en el resultado de la peticion favor de intentar de nuevo");
    mdlAceptarRechazar.modal("show");
    mdlAceptarRechazar.on('hidden.bs.modal', function () {
        mdltxtTitulo.text(``);
        mdltxtMensaje.html(``);
    });
    return new Promise((cbAcetar, cbRechazar) => {
        mdlbtnAceptar.click(cbAcetar);
        mdlbtnCancelar.click(cbRechazar);
    });
};

function AlertaAceptarRechazarNormal(titulo, mensaje, cbAcetar, cbRechazar) {
    const mdlAceptarRechazar = $("#mdlAceptarRechazar"),
        mdltxtTitulo = $("#mdltxtTitulo"),
        mdltxtMensaje = $("#mdltxtMensaje"),
        mdlbtnAceptar = $("#mdlbtnAceptar"),
        mdlbtnCancelar = $("#mdlbtnCancelar");
    mdltxtTitulo.text(titulo ? titulo : "Confirmar acción");
    mdltxtMensaje.html(mensaje ? mensaje : "Por favor confirme que desea realizar esta acción.");
    mdlAceptarRechazar.modal("show");
    mdlAceptarRechazar.on('hidden.bs.modal', function () {
        mdltxtTitulo.text(``);
        mdltxtMensaje.html(``);
    });
    if (cbAcetar) {
        mdlbtnAceptar.off().click(cbAcetar);
    }
    if (cbRechazar) {
        mdlbtnCancelar.off().click(cbRechazar);
    }
};

function loadGrid(objetoCarga, controller, grid) {
    $.blockUI({ message: "Procesando..." });
    $.ajax({
        url: controller,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(objetoCarga),
        success: function (response) {
            if (response.success) {
                grid.bootgrid({
                    templates: {
                        header: ""

                    }
                });
                grid.bootgrid("clear");
                var JSONINFO = response.rows;
                grid.bootgrid("append", JSONINFO);
                grid.bootgrid('reload');

            }
            else {

                AlertaGeneral("Alerta", "no se obtuvieron registros con los filtros seleccionados")
            }
            $.unblockUI();
        },
        error: function (response) {
            $.unblockUI();
            alert(response.message);
        }
    });
}

function ConfirmacionGeneral(titulo, mensaje, color) {


    if (mensaje == null) {
        mensaje = "Error en el resultado de la peticion favor de intentar de nuevo";
    }

    $("#dialogalertaGeneral").removeClass('hide');
    $("#txtComentarioAlerta").text(mensaje);
    var opt = {
        title: titulo,
        draggable: false,
        resizable: false,
        modal: true,
        maxWidth: 600,
        minWidth: 400,
        position: {
            my: "center",
            at: "center",
            within: $(".RenderBody")
        },
        buttons: [
            {
                text: "Aceptar",
                click: function () {
                    $("#dialogalertaGeneral").addClass('hide');
                    $(this).dialog("close");
                }
            }
        ]

    };
    $("#dialogalertaGeneral").dialog(opt).dialog("open");
    //    $("#dialogalertaGeneral").trigger("click");

}

function ConfirmacionGeneralAccion(titulo, mensaje) {


    if (mensaje == null) {
        mensaje = "Error en el resultado de la peticion favor de intentar de nuevo";
    }

    $("#dialogalertaGeneral").removeClass('hide');
    $("#txtComentarioAlerta").text(mensaje);
    var opt = {
        title: titulo,
        draggable: false,
        resizable: false,
        modal: true,
        close: function (event, ui) { location.reload(); },
        maxWidth: 600,
        minWidth: 400,
        position: {
            my: "center",
            at: "center",
            within: $(".RenderBody")
        },
        buttons: [
            {
                text: "Aceptar",
                click: function () {
                    location.reload();
                    $(this).dialog("close");
                }
            }
        ]

    };
    $("#dialogalertaGeneral").dialog(opt).dialog("open");
    //    $("#dialogalertaGeneral").trigger("click");

}
function ConfirmacionEliminacionCustom(titulo, mensaje, tituloBoton1, tituloBoton2) {

    if (!$("#modalEliminacion").is(':visible')) {
        var html = '<div id="modalEliminar" class="modal fade" role="dialog">' +
            '<div class="modal-dialog modal-dialog-fix modal-md">' +
            '<div class="modal-content">' +
            '<div class="modal-header text-center modal-bg">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
            '&times;</button>' +
            '<h4  class="modal-title">' + titulo + '</h4>' +
            '</div>' +
            '<div class="modal-body ajustar-texto">' +
            '<h5 id="pMessage">' +
            '</h5>' +
            '<div class="row">' +
            '<div id="icon" class="col-md-2">' +
            '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
            '</div>' +
            '<div class="col-md-10">' +
            '<h3>  ' + mensaje + '</h3>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="modal-footer">' +
            '<a data-dismiss="modal" id="btnModalBoton1" class="btn btn-primary btn-sm">' + tituloBoton1 + '</a>' +
            '<a data-dismiss="modal" id="btnModalBoton2" class="btn btn-primary btn-sm">' + tituloBoton2 + '</a>' +
            '<a data-dismiss="modal" id="btnModalBotonCancelar" class="btn btn-primary btn-sm">Cancelar</a>' +
            '</div>' +
            '</div>' +
            '</div></div>';

        var _this = $(html);
        _this.modal("show");
    }
    //    $("#dialogalertaGeneral").trigger("click");

}
function ConfirmacionEliminacion(titulo, mensaje) {

    if (!$("#modalEliminacion").is(':visible')) {
        var html = '<div id="modalEliminar" class="modal fade" role="dialog">' +
            '<div class="modal-dialog modal-dialog-fix modal-md">' +
            '<div class="modal-content">' +
            '<div class="modal-header text-center modal-bg">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
            '&times;</button>' +
            '<h4  class="modal-title">' + titulo + '</h4>' +
            '</div>' +
            '<div class="modal-body ajustar-texto">' +
            '<h5 id="pMessage">' +
            '</h5>' +
            '<div class="row">' +
            '<div id="icon" class="col-md-2">' +
            '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
            '</div>' +
            '<div class="col-md-10">' +
            '<h3>  ' + mensaje + '</h3>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="modal-footer">' +
            '<a data-dismiss="modal" id="btnModalEliminar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
            '<a data-dismiss="modal" id="btnCancelar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
            '</div>' +
            '</div>' +
            '</div></div>';

        var _this = $(html);
        _this.modal("show");
    }
    //    $("#dialogalertaGeneral").trigger("click");

}

function ConfirmacionGuardado(titulo, mensaje) {

    if (!$("#modalEliminacion").is(':visible')) {
        var html = '<div id="modalEliminar" class="modal fade" role="dialog">' +
            '<div class="modal-dialog modal-dialog-fix modal-md">' +
            '<div class="modal-content">' +
            '<div class="modal-header text-center modal-bg">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
            '&times;</button>' +
            '<h4  class="modal-title">' + titulo + '</h4>' +
            '</div>' +
            '<div class="modal-body ajustar-texto">' +
            '<h5 id="pMessage">' +
            '</h5>' +
            '<div class="row">' +
            '<div id="icon" class="col-md-2">' +
            '<span class="glyphicon glyphicon glyphicon-floppy-save" style="font-size:40px;" aria-hidden="true"></span>' +
            '</div>' +
            '<div class="col-md-10">' +
            '<h3>  ' + mensaje + '</h3>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="modal-footer">' +
            '<a data-dismiss="modal" id="btnConfirmacionGuardar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
            '<a data-dismiss="modal" id="btnConfirmacionCancelar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
            '</div>' +
            '</div>' +
            '</div></div>';

        var _this = $(html);
        _this.modal("show");
    }
    //    $("#dialogalertaGeneral").trigger("click");

}

function AlertaLogin(titulo, mensaje) {

    if (!$("#modalEliminacion").is(':visible')) {
        var html = '<div id="modalEliminar" class="modal fade" role="dialog">' +
            '<div class="modal-dialog modal-dialog-fix modal-md">' +
            '<div class="modal-content">' +
            '<div class="modal-header text-center modal-bg">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
            '&times;</button>' +
            '<h4  class="modal-title">' + titulo + '</h4>' +
            '</div>' +
            '<div class="modal-body ajustar-texto">' +
            '<h5 id="pMessage">' +
            '</h5>' +
            '<div class="row">' +

            '<div class="col-md-12">' +
            '<div id="icon" class="col-md-2">' +
            '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;top:-20px;" aria-hidden="true"></span>' +
            '</div>' +
            '<h3>  ' + mensaje + '</h3>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="modal-footer">' +
            '<a data-dismiss="modal" class="btn btn-primary btn-sm">Aceptar</a>' +
            '</div>' +
            '</div>' +
            '</div></div>';

        var _this = $(html);
        _this.modal("show");
    }
    //    $("#dialogalertaGeneral").trigger("click");

}


function ModalAcciones(titulo, mensaje) {

    var html = '<div id="ModalAcciones" class="modal fade" role="dialog">' +
        '<div class="modal-dialog modal-dialog-fix modal-md">' +
        '<div class="modal-content">' +
        '<div class="modal-header text-center modal-bg">' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
        '&times;</button>' +
        '<h4  class="modal-title">' + titulo + '</h4>' +
        '</div>' +
        '<div class="modal-body ajustar-texto">' +
        '<h5 id="pMessage">' +
        '</h5>' +
        '<div class="row">' +
        '<div id="icon" class="col-md-2">' +
        '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
        '</div>' +
        '<div class="col-md-10">' +
        '<h3>  ' + mensaje + '</h3>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="modal-footer">' +
        '<a data-dismiss="modal" id="btnModalAccionAceptar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> OK</a>' +
        '<a data-dismiss="modal" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
        '</div>' +
        '</div>' +
        '</div></div>';

    var _this = $(html);
    _this.modal("show");
    //    $("#dialogalertaGeneral").trigger("click");
}

function getFecha() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    return dd + '/' + mm + '/' + yyyy;
}

function getFechaCeros() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    return (dd < 10 ? ('0' + dd) : dd) + '/' + (mm < 10 ? ('0' + mm) : mm) + '/' + yyyy;
}

function getFechaServer() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();


    return yyyy + '-' + mm + '-' + dd;
}

function getEstatus(estatus) {
    return (estatus == "ACTIVO") ? 1 : 0;
}


File.prototype.convertToBase64 = function (callback) {
    var FR = new FileReader();
    FR.onload = function (e) {
        callback(e.target.result)
    };

    FR.readAsDataURL(this);
}


function ajaxindicatorstart(text) {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading" style="display:none"><div><img src="../../../Content/img/ajax-loader.gif"><div>' + text + '</div></div><div class="bg"></div></div>');
    }

    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });

    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });

    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'

    });

    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(500);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}

/*
jQuery(document).ajaxStart(function () {
    //show ajax indicator
    ajaxindicatorstart('Cargando información.. espere un momento..');
}).ajaxStop(function () {
    //hide ajax indicator
    ajaxindicatorstop();
});*/

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}
function getFechaddMMyyyy(valor) {
    var result = "";
    result = $.datepicker.formatDate('dd/mm/yy', new Date(valor));
    return result;
}
/**
 * Asiga el valor a un MonthPicker
 *
 * @param {number} inp Input MonthPicker.
 * @param {number} fecha Valor a asignar.
 */
function seMonthPickerValor(inp, fecha) {
    let currentMonth = new Date().getMonth()
        , currentYear = new Date().getFullYear()
        , mes = fecha.getMonth()
        , anio = fecha.getFullYear();
    inp.MonthPicker({ SelectedMonth: (mes - currentMonth), SelectedYear: (anio - currentYear) });
}
function setResoluciones(width, height) {
    $.ajax({
        url: '/home/getResolucion',
        type: 'POST',
        data: { width: width, height: height },
        success: function (response) {

        },
        error: function (response) {
        }
    });
};
function validarCampo(_this) {
    var r = false;
    if (_this.val() == '' || _this.val() == '0') {
        if (!_this.hasClass("errorClass")) {
            _this.addClass("errorClass")
        }
        r = false;
    }
    else if (_this.hasClass("CustomDecimalEvent")) {
        if (_this.getVal(2) == 0) {
            if (!_this.hasClass("errorClass")) {
                _this.addClass("errorClass")
            }
            r = false;
        }
        else {
            _this.addClass("errorClass");
            r = true;
        }
    }
    else {
        if (_this.hasClass("errorClass")) {
            _this.removeClass("errorClass")
        }
        r = true;
    }
    return r;
}

function validarCampoVacio(_this) {
    var r = false;

    if (_this.val() == '') {
        if (!_this.hasClass("errorClass")) {
            _this.addClass("errorClass")
        }

        r = false;
    } else if (_this.hasClass("CustomDecimalEvent")) {
        if (_this.getVal(2) == 0) {
            if (!_this.hasClass("errorClass")) {
                _this.addClass("errorClass")
            }

            r = false;
        } else {
            _this.addClass("errorClass");
            r = true;
        }
    } else {
        if (_this.hasClass("errorClass")) {
            _this.removeClass("errorClass")
        }

        r = true;
    }

    return r;
}

function aceptaSoloNumero2D(input, event) {
    var $this = $(input);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
        ((event.which < 48 || event.which > 57) &&
            (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(input).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(input)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
}

function permitePegarSoloNumero2D(input, event) {
    var text = event.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            event.preventDefault();
            $(input).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        event.preventDefault();
    }
}

function aceptaSoloNumeroXD(input, event, lenght) { //PROBAR
    var $this = $(input);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
        ((event.which < 48 || event.which > 57) &&
            (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(input).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > lenght) &&
        (event.which != 0 && event.which != 8) &&
        ($(input)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
}

function permitePegarSoloNumeroXD(input, event, length) { //PROBAR
    var text = event.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > length) && (text.indexOf('.') > -1)) {
            event.preventDefault();
            $(input).val(text.substring(0, text.indexOf('.') + 7));
        }
    }
    else {
        event.preventDefault();
    }
}

function aceptaSoloNumero0D(input, event) {
    if (
        (
            (
                (event.which > 47 && event.which < 58) ||
                (event.which > 95 && event.which < 106) ||
                event.which == 0 ||
                event.which == 8 ||
                event.which == 46 ||
                event.which == 35 ||
                event.which == 36 ||
                event.which == 37 ||
                event.which == 39 ||
                event.which == 9
            )
        )
    ) {
    } else {
        event.preventDefault();
    }
}

function permitePegarSoloNumero0D(input, event) {
    var text = event.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length >= 0) && (text.indexOf('.') > -1)) {
            event.preventDefault();
        }
    }
    else {
        event.preventDefault();
    }
}

function aceptaSoloNumeroXDIntMax(input, event, maxLength) {
    if ($(input).val().length == maxLength) {
        event.preventDefault();
    }
    if (((event.which < 48 || event.which > 57) &&
        (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }
}

function permitePegarSoloNumeroXDIntMax(input, event, maxLength) {
    var text = event.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text) && text.length <= maxLength) {
        if ((text.substring(text.indexOf('.')).length >= 0) && (text.indexOf('.') > -1)) {
            event.preventDefault();
        }
    }
    else {
        event.preventDefault();
    }
}

function convertToMultiselect(selector) {
    $(selector).multiselect('destroy');
    $(selector).multiselect({
        buttonWidth: '100%',
        maxHeight: 400,
        numberDisplayed: 1,
        onChange: function (option, checked, select) {
            if (option[0].value == "Todos") {
                $(selector).multiselect('deselect', "Todos")
                var total = $(selector + ' option').length;
                var seleccionados = $(selector + ' option:selected').length;
                if (seleccionados == (total - 1)) {
                    $(selector).multiselect('deselectAll', true);
                }
                else {
                    $(selector).multiselect('selectAll', true);
                    $(selector).multiselect('deselect', "Todos")
                }
            }
        }
    });
}
function convertToMultiselectSelectAll(selector) {
    $(selector).multiselect('destroy');
    $(selector).multiselect({
        buttonWidth: '100%',
        maxHeight: 400,
        numberDisplayed: 1,
        includeSelectAllOption: true,
        selectAllText: ' Seleccionar Todos',
        nonSelectedText: 'Ninguno Seleccionado',
        allSelectedText: 'Todos Seleccionados',
        onChange: function (option, checked, select) {
            if (option[0].value == "Todos") {
                $(selector).multiselect('deselect', "Todos")
                var total = $(selector + ' option').length;
                var seleccionados = $(selector + ' option:selected').length;
                if (seleccionados == (total - 1)) {
                    $(selector).multiselect('deselectAll', true);
                }
                else {
                    $(selector).multiselect('selectAll', true);
                    $(selector).multiselect('deselect', "Todos")
                }
            }
        }
    });
}
function getValoresMultiples(selector) {
    var _tempObj = $(selector + ' option:selected').map(function (a, item) { return item.value; });
    var _tempArrObj = new Array();
    $.each(_tempObj, function (i, e) {
        _tempArrObj.push(e);
    });
    return _tempArrObj;
}

function getTextosMultiples(selector) {
    var _tempObj = $(selector + ' option:selected').map(function (a, item) { return item.text.trim(); });
    var _tempArrObj = new Array();
    $.each(_tempObj, function (i, e) {
        _tempArrObj.push(e);
    });
    return _tempArrObj;
}

function openCRModal() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Base/setResolutionResult",
        async: false,
        data: { width: screen.width, height: screen.height },
        success: function (r) {
            //$("#reportViewerModal").show(function () {
            //    $(window).scrollTop(0);
            //    $("body").css("width", "100%");
            //    $("body").css("height", "105%");
            //});
            $(window).scrollTop(0);
            $("body").css("overflow", "hidden");
            $("#reportViewerModal").css("width", "100%");
            $("#reportViewerModal").css("height", "105%");
            $.unblockUI();
        },
        error: function (ex) {
        }
    });
}

function openCRModalRemote(m) {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Base/setResolutionResult",
        async: false,
        data: { width: screen.width, height: screen.height },
        success: function (r) {
            $(window).scrollTop(0);
            $("body").css("overflow", "hidden");
            $("#reportViewerModal").css("width", "100%");
            $("#reportViewerModal").css("height", "105%");
            $.unblockUI();
            m.modal('show');
        },
        error: function (ex) {
        }
    });
}
jQuery.fn.extend({
    setVal: function (valor) {
        var _this = $(this);
        try {
            var t = $(this)[0].nodeName;
            if (t == 'INPUT' || t == 'SELECT') {
                _this.val(valor);
            }
            else {
                _this.html(valor);
            }
            _this.click();
        }
        catch (err) {
            _this.click();
        }
    },
    getVal: function (valor) {
        try {
            var t = $(this)[0].nodeName;
            var result = 0;
            if (t == 'INPUT' || t == 'SELECT') {
                result = isNaN(removeCommas(this.val())) ? Number((0)) : Number(removeCommas(this.val()));
            }
            else {
                result = isNaN(removeCommas(this.html())) ? Number((0)) : Number(removeCommas(this.html()));
            }
            return Number(isNaN(result)) ? Number(Number(0)) : Number(Number(result));
        }
        catch (err) {
            return Number(isNaN(result)) ? Number(Number(0)) : Number(Number(result));
        }

        /*
             try {
            var t = $(this)[0].nodeName;
            var result = 0;
            if (t == 'INPUT' || t == 'SELECT') {
                result = isNaN(removeCommas(this.val())) ? Number((0).toFixed(valor)) : Number(removeCommas(this.val())).toFixed(valor);
            }
            else {
                result = isNaN(removeCommas(this.html())) ? Number((0).toFixed(valor)) : Number(removeCommas(this.html())).toFixed(valor);
            }
            return Number(isNaN(result)) ? Number(Number(0).toFixed(valor)) : Number(Number(result).toFixed(valor));
        }
        catch (err) {
            return Number(isNaN(result)) ? Number(Number(0).toFixed(valor)) : Number(Number(result).toFixed(valor));
        }

        */
    },
    getAgrupador: function () {
        var _this = $(this);

        return _this.val();
    },
    getEmpresa: function () {
        var _this = $(this);
        var empresa = $("option:selected", _this).attr("empresa");
        return empresa;
    },
    getMultiSeg: function () {
        var _this = $(this);
        var regs = $(this).find(':selected');
        var data = [];

        $.each(regs, function (i, e) {
            var obj = {};
            obj.idEmpresa = $(e).attr('empresa');
            obj.idAgrupacion = $(e).val();
            data.push(obj);
        });
        return data;
    },
    DecimalFixPs: function (valor, predeterminado) {
        var _this = $(this);
        var pre = predeterminado == undefined ? 0 : predeterminado;
        if (!_this.hasClass("CustomDecimalEvent")) {
            _this.addClass("CustomDecimalEvent");
        }
        var t = $(this)[0].nodeName;
        if (t == 'INPUT' || t == 'SELECT') {
            if (_this.val() == '' || _this.html() == '0%')
                _this.val("$" + (pre).toFixed(valor));

            $(this).on({
                'change': function (e) {
                    _this.val("$" + (isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)));
                },
                'click': function (e) {
                    _this.val("$" + (isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)));
                }
            });
        }
        else {
            if (_this.html() == '')
                _this.html("$" + (pre).toFixed(valor));

            $(this).on({
                'change': function (e) {
                    _this.html("$" + (isNaN(Number(removeCommas(_this.html()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)));
                },
                'click': function (e) {
                    _this.html("$" + (isNaN(Number(removeCommas(_this.html()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)));
                }
            });
        }
    },
    DecimalFixPr: function (valor, predeterminado) {
        var _this = $(this);
        var pre = predeterminado == undefined ? 0 : predeterminado;
        if (!_this.hasClass("CustomDecimalEvent")) {
            _this.addClass("CustomDecimalEvent");
        }
        var t = $(this)[0].nodeName;
        if (t == 'INPUT' || t == 'SELECT') {
            if (_this.val() == '' || _this.val() == '0%')
                _this.val((pre).toFixed(valor) + "%");

            $(this).on({
                'change': function (e) {
                    _this.val((isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)) + "%");
                },
                'click': function (e) {
                    _this.val((isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)) + "%");
                }
            });
        }
        else {
            if (_this.html() == '' || _this.html() == '0%')
                _this.html((0).toFixed(valor) + "%");

            $(this).on({
                'change': function (e) {
                    _this.html((isNaN(Number(removeCommas(_this.html()))) ? (0).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)) + "%");
                },
                'click': function (e) {
                    _this.html((isNaN(Number(removeCommas(_this.html()))) ? (0).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)) + "%");
                }
            });
        }
    },
    DecimalFixNS: function (valor, predeterminado) {
        var _this = $(this);
        var pre = predeterminado == undefined || predeterminado == null ? 0 : predeterminado;
        if (!_this.hasClass("CustomDecimalEvent")) {
            _this.addClass("CustomDecimalEvent");
        }
        var t = $(this)[0].nodeName;
        if (t == 'INPUT' || t == 'SELECT') {
            if (_this.val() == '' || _this.val() == '0')
                _this.val((pre).toFixed(valor));

            $(this).on({
                'change': function (e) {
                    _this.val((isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)));
                },
                'click': function (e) {
                    _this.val((isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.val())).toFixed(valor), valor)));
                }
            });
        }
        else {
            if (_this.html() == '' || _this.html() == '0')
                _this.html((pre).toFixed(valor));

            $(this).on({
                'change': function (e) {
                    _this.html((isNaN(Number(removeCommas(_this.html()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)));
                },
                'click': function (e) {
                    _this.html((isNaN(Number(removeCommas(_this.html()))) ? (pre).toFixed(valor) : formatValue(Number(removeCommas(_this.html())).toFixed(valor), valor)));
                }
            });
        }
    }
});
function formatInput(input) {
    input.value = formatValues(input.value);
}
function formatValues(valor) {

    var pre = predeterminado == undefined || predeterminado == null ? 0 : predeterminado;
    if (!_this.hasClass("CustomDecimalEvent")) {
        _this.addClass("CustomDecimalEvent");
    }
    return _this.val((isNaN(Number(removeCommas(_this.val()))) ? (pre).toFixed(2) : formatValue(Number(removeCommas(_this.val())).toFixed(2))));;
}
function fnReloadCustomDecialEvent() {
    var all = $(".CustomDecimalEvent").click();
}
function formatInput(input) {
    input.value = formatValue(input.value);
}
function formatValue(valor, fix) {

    var svalor = "" + valor;
    var result = "";

    var num = removeMenos(svalor.replace(/\,$/g, ''));
    num = num.replace("%", "");
    if (!isNaN(num)) {

        var tempArray = num.split('.');
        var num1 = 0;
        var decimales = 0;
        if (tempArray.length == 1) {
            num = tempArray[0].toString().split('').reverse().join('').replace(/(?=\d*\,?)(\d{3})/g, '$1,');
            num = num.split('').reverse().join('').replace(/^[\,]/, '');
        }
        else if (tempArray.length == 2) {
            num = tempArray[0].toString().split('').reverse().join('').replace(/(?=\d*\,?)(\d{3})/g, '$1,');
            num = num.split('').reverse().join('').replace(/^[\,]/, '');

            num = num + '.' + tempArray[1];
        }
        //if(fix=='4'){
        //    num = num.toString().split('').reverse().join('').replace(/(?=\d*\,?)(\d{4})/g, '$1,');
        //}
        //else{

        //}
        result = num;
    }
    else {
        result = removeMenos(svalor.replace(/[^\d\,]*/g, ''));
    }
    return valor < 0 ? "-" + result : result;
}
function removeCommas(str) {
    if (str == null) {
        return "error";
    }
    else {
        while (str.search(",") >= 0) {
            str = (str + "").replace(',', '');
        }

        str = (str + "").replace('$', '');
        str = (str + "").replace('%', '');
        return str == '' ? "error" : str;
    }
};
function removeMenos(str) {
    while (str.search("-") >= 0) {
        str = (str + "").replace('-', '');
    }
    return str;
};
function GetPeriodoMeses(selectorPeriodo, selectorMesInicio) {

    var periodo = selectorPeriodo.val();
    var MesInicio = selectorMesInicio.val();
    var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
        "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
    var tituloMeses = [];

    var count = 0;
    for (var i = MesInicio; i < 12; i++) {
        count++;

        tituloMeses.push(months[i] + " " + periodo);
    }

    for (var i = 0; i < MesInicio; i++) {

        tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
    }
    return tituloMeses;

}
function parametrosImpresion(pdfTitle, printTitle) {
    return [
        {
            extend: 'excelHtml5',
            exportOptions: { orthogonal: 'export' }
        },
        {
            extend: 'pdfHtml5',
            message: pdfTitle,
            header: true,
            title: '',
            exportOptions: {
                columns: ':visible'
            },
            customize: function (doc) {
                doc.content.splice(1, 0, {
                    margin: [0, 0, 0, 12],
                    alignment: 'left',
                    image: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAAA2CAYAAAAxpDyoAAAMuElEQVR4Xu1cebSdVXX//fYhyQshTAERgUqKQ0WECLVCTFhgXXWgUIc6gEqdoAQ1ThRCCOkLCAlRSquwSltHokitolaXaFtQSiyyypBlLXNTBnEhFrIY897LO/vXte/97uW7N+/d6c2snL/uXd8++9tn/84+e59z9v6INs3MPifyxe3oOn1O4CEB+3VK34ZuUDm/eZx4TQs2bCcFU/pPAr/fjq6j59Jl7v4RpHQypZUkX9pRvxGIJLnM/gjDw9f2ymM69ps0QATcrJyXABisKyKlD1JaQfJFPSjnHz3nd/XQb1p3mRRAJD0h91cAeGBEbZidRuBMkgs70Zak/5X773ZCO9NoJgUQz/kYANe3VU5KHyiWsoNGo5U0IPf9ATzalt8MJJh4QKSV7r62K92kdAqlVSR/p6GflB34GNwv64rfDCKeUEAEXKucXw8g96STKjDnkHxh9BfwY+X8hp54zZBOEwaIpMflHkvP2JYWs5VGXiDpYV/9shcO9e36UiLZdNev27Zn+s686Z5u5ZwoQNxzjr3L5m4FaqL/A0vpJknDcj8cwH9p/THPH/Lh74I8AsBOY+Q/gd318zlnbTyq2xdMCCAE3p1zvrKFMAlAOOaHAAyPQjfPUroPwAKXzoN7f5lOpx4xa9tBc38q8XAQfd0OfOLppwkgAq5Tzn/YasBmtgbkakkPyP1NAO4A4OU+ZnYxyE8KuEU5t9yYDl609PuAXg9w1sQrutM3TANAJG2V+24AtrUQ+0hL6caG2S49JPfjYkkqgDnWUrpO0qNy36tTFQysW/pXJE4HMKfTPhNHNw0A8ZxDeS2duJk9CnLPkRQRjltmbzPpXwDM86qlXdet0gbWLb2ExMe77Te+9FMMiJNxrvSvrQbFlK4h0C5sFYDwbRs855N7VZL6Xz57sG/BckJrwalw/lMLyBc95w+1VJ7ZOUaeXyi7Jamk24ujlga/0gs4AczQnN2Oh9kVAHfuhUdvfaYOkCc85z2anXLTIF5Cs1+SHTndIc85zrR+3ZsiRu6l/gP7hvoOeDOIvwWw+3jyHuWNUxP2es5zAQy0XKrMHiB5QCdKcOBU5PwPo9FGuPvMwtmHzDv7xts64ddMc/OpR8w67EU7L3bHN0Ds2wuPzvpMtoXEuZL7K4vIaFQZmdIPCEQE1bYJ+K5yfksrwsH1R6+CfA3AzZIv71vxs2vaMh6BIJaybbP3eKUSvg5w1MPMXnhX+0w+IGe4+8Vt/MZHjPwbAG2POiT9T+E3to7Gc+v5Ry+02Srt/iUBDwJc23fWDZf3ojy9HWnb4UsWyfglAIf2wmPKlyxJm1S1jlbtAJrdTnKXDgYZfmMxgFtGXar6j9lpqC/fAWKUyyzdD+EL37tl49p3/FP3h5nqh22bc9Rhbjt9nsBrOpC5DckkWUhxHxH7iFFnckhKszs7vaJ1qR8ey9DobeCiJacTbH/sLj0E8spfb31w9cL++1r6tpHeFsA8PWvxobNm2XkQj+8dmEkARNKQ3A8B0MkpZqsd9mya3Upyn+JIPY5PWoa4Avhk/xELOlXQ/D0ff5LL7332urjTjiW6wQuXHoykVQBP7L77JADiwCnI+QvdC9fYw8wuBflhSf9X+I2Hx8pzIvsPrF98EJRWEGi912oQYoIB6SQC6lApb7WUvgkgec5vBPCjDvtNOZkuWLrvkGEZDOe2F2YCAYG0p9xfBmCovSAtKebR7C6S+0Fa6+4rx8hvSrrrs8fsNZiHzyCwHGDsw0ZoEwfIT5Xz+wDE3cSYmpldBfKdkm6Ve0RVY1rjxyTMOHTWZw6dN5R3XQHwjO3vZCYIEAChuP8Ys/wpnWSITRie8ZwjZL57zDynEYOh9Us+5uIqAkUwM3GAjMewFxRh8F4OnIScvzEa0ycvXLp3SpW7+BnZEm0FFCcTunlCrnAnUytxTjV00M6xOYykuhnepreFdKTcoYuWnK5ONn8dcZtqohkOiC569f5DmH3v9Lh+HQ8wZzAgcVwxNDcSFRA79udIm8GADK5d8hYYr36OIFEMY4YCsqV/0e7z5s6/W8DeOwCpJhNMaRtcd9QrxJ3G4bh7Soex3cvp+s2cszd+p1upphyQbgV+rtPvAGSaIbwDkB2ATDMNTDNxdljINAZkMcxOhbQA5A/hHgllzW0uzD4J6UiQm+D+WQCPl4jiZLiWYbIRwPMAvKR4/lsAdxW/94XZaZAOB/kA3L8GoJaAHfcuo13Vxn1M5GO9ukmweFdzi4rfWosk7iiPi0TwaM8AuLXIMtm1RPcEgF+U/sezcibKJgBPlZ5HguDLm14c+oj3RVsEoJbkEXI/DSDqXGoZlDcB2KeoMItT4sfDQubTbDPJhjtwSVnAp+AeaTywlD4P4MPNqaBy3yhpadDQbCvJSq2G53wAUnqdAV8uhLvKcz7RzC4XcArJcmqQitKEA5nSdwiM+DGASjK2+yJLaaQrX5d7FPdECC1LKXKEKy2KTpnSpwlUQJJ0Z1y4MaUbCRzZoNCofwfiavnAUJ6lVAfbc34VgJtr9BaTitxu4nrOUUiUaXYbyQAlXnqBu6+i2R0kf6+Qa1+SG2j2SHH5N8Da/XZdKOlBFFmGkp6S+/yYXZbS/SXBo9Cm/jWGivKBX5UBgbTMyYFmQGj2NFnPsY0ZWZ+hLp1G8g09AlJTfszY23sGpDZI6XJ3/1orQJjS1QS2S+zznGNMT5YBKbJ1dilSauuAIKXjTFpAcksOCykjVmSAnGMp1WdB5TIppYUGVI42ivKy+ZbSs2lA0vnuvroMiKRHRJ7VBMhqS6l+MRVlbzT7t1pRp0sr4P6zlNJ+Dixn9XIs3vlbIz+aI2U155+XLYTAHwv4QWlCnevun+4WEAEbBXzJgEiYiwLTa5TzBS0BMXuE5HYnDJ5zLEOPNFhI1VIPptnVZQsB0GDtAcg9tS8pCPiRcv6EpRQVTbUZ96cppd0FVLJNRgSEXOfDw2c3WEhhpiDPKS1Za5p4n8iUIsXmBYUSLkXOXy2WyLjEqnypQdJ9cq99VGCfMiBGHuvST+qAkH/tw8Mxhq6WLAFxVX2ppfStQpbQRQA72pI121J69gpaugvFp0I851ju7m8GBNK5At49NkCkZYnc1gsgkeFYX0OB8CEnWUrhFCtOLcyYwN+5+3bFNZZSR4BAinW8nkZK4OSc84ZJAGRXS6ke0ASgBOIDCZFg9n7k/JVmQMIFhEXUDMBzjmTvthbyNgDhvGrtnpTSG3sBpMFZVgEJp17JySo/Kxx61LPfWXeYnQLSyGizu0dU9HTXgEgReQ2RrJYqSN9098+1sJDnWUq/KVnmGkh/Wfz/uuf8nu0spPowavaj6DWWsI4AiVyphpZS+uB4AVIRxuyjRq4HUC/SLL6HEhFPZbns2EJKkhY5xxHxPdUtIM1jduAE5PxYC0AaAp0IRCJ3rbD8X8k9cpufjbKaX9AFIB9iSpfW+ou8PEn7dwtImGdTonXFQkpyvZgpbWB5TyFd4e5/1g0gzT7EgT9Hzn/fLSDxQRsCX4waSXcPv/FLAK9pAcifWEoVAIrZvrelFHut2v85NLuptmRHYNIcAHRqIY1OHXhXAnbpAZBItq6Ed0WrLlllZwu8idKFdaGBG5Tz0WMBBGNz6sc2TeRRATGz9SD/oj5xpfANzy8BshvNri/tQ86LUvAy/9EA+W+SB1dMrRplrbSUYhdbQ/pVKaXDugUEEcKS61oCQh5rUlhEJOKFk6+XOXS6ZI0aZZkNgpxdzN62G8MiyuoYkBE3laXBes4voNkPa4B4zotodgPJ2NfVdLu9D7GUrgLwzkIh9wrYYGStNCB7znHcsNBSqh4HSO5kJL1FvxrjSo5u0059F0tpS8lPVCyEZjGTIk4PXp8RcHzNkoqP1bxuPCyEZltqDtqlyxjWWHyPS8D3lfMJZaV2AoikcOLbRK4xoKHszqWPE7iYZM1hBwBfaQLkyyTrdTUjWgiA4y2lf24y1aq+qrF5zJq5NLubZHwOo7kNFKANNQESe5zI462dZdWirCtAvnek97m0DO6VEHbMFlKK0prf5TkH6Nd2C0idj7QS5IW1//WjGLNBFlYJ6QwB7ykDYmYngDyvpYUUD9/KlCJ5+LDYI0jaLOBKuEcZcy3BendL6WJIxyHqOqTHQP67cl5Wi6WZ0o8BVJaJANLMzhX52vhP6Tqv8psFszMJnEgyjjmGJf1C5CXIOQ4ZK62hL/BwKSDYI44s6srI+RNM6ZLaf0rf8tr3tMw+ReADhQWapNvkHgqpOGNL6TIBleWa5KbYUDaBdwirZ3gNzYCvOlAJPoqxbXL3kKM+fkrXg9xPqFZ8qVo2voUpfbske2wxHisz/3/xibW5KGQcdAAAAABJRU5ErkJggg=='

                });

            },
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        {
            extend: 'print',
            text: 'Imprimir',
            header: true,
            title: '',
            exportOptions: {
                columns: ':visible',
            },
            message: printTitle,
            customize: function (win) {
                $(win.document.body)
                    .css('font-size', '10pt')
                    .prepend(
                        '<img src="http://10.1.0.126/Content/img/logo/logo.png" alt="algo" style="position:absolute; top:0px; left:20px;width:100px !important;height;100px !important;" />'
                    );

                $(win.document.body).find('table')
                    .addClass('compact').css('text-align', 'center')
                    .css('font-size', 'inherit').css('margin-top', '50px').css('border', '1px solid black');
                $(win.document.body).find('table tr > th').css('text-align', 'center')
                    .css('font-size', 'inherit').css('margin-top', '50px').css('border', '1px solid black');
                $(win.document.body).find('table tr > td')
                    .css('font-size', 'inherit').css('margin-top', '50px').css('border', '1px solid black');
            },
            orientation: 'landscape',
            pageSize: 'LEGAL'
        }
    ];
}
function maskNumero2DCompras(numeroInt) {
    let numeroString = parseFloat(numeroInt).toFixed(2);
    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = '$' + enteros + '.' + decimales;

    return resultado;
}
function maskNumero2DComprasN(numeroInt) {
    let numeroString = parseFloat(numeroInt).toFixed(2);
    let simboloNegativo = '';

    if (numeroInt < 0) {
        simboloNegativo = '-';
        numeroString = numeroString.replace('-', '');
    }

    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = '$' + enteros + '.' + decimales;

    return simboloNegativo + resultado;
}
function unmaskNumero6D(numeroString) {
    return parseFloat(numeroString).toFixed(6).replace(/[^0-9.-]+/g, "");
}
function unmaskNumero6DCompras(numeroString) {
    return +((+(numeroString.replace(/[^0-9.-]+/g, ""))).toFixed(6));
}
function maskNumero6DCompras(numeroInt) {
    let numeroString = parseFloat(numeroInt).toFixed(6);
    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = '$' + enteros + '.' + decimales;

    return resultado;
}
function maskNumero6DComprasColombia(numeroInt) {
    let numeroString = parseFloat(numeroInt).toFixed(6);
    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = 'COP ' + enteros + '.' + decimales;

    return resultado;
}
function maskNumero6D(numeroInt) {
    return "$" + parseFloat(numeroInt).toFixed(6).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1");
}
function unmaskNumero(numeroString) {
    return +(numeroString.replace(/[^0-9.-]+/g, ""));
}
function maskNumero(numeroInt) {
    return "$" + parseFloat(numeroInt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}
function maskNumero_NoMonedaNoDecimal(numeroInt) {
    return parseFloat(numeroInt).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}
function maskNumero2D(numeroInt) {
    return parseFloat(numeroInt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}
function maskNumeroNM(numeroInt) {
    return parseFloat(numeroInt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "1,");
}
function maskNumeroPorcentaje(numeroInt) {
    return parseFloat(numeroInt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '%';
}
function maskNumeroXD(numeroInt, digitos) {
    return "$" + parseFloat(numeroInt).toFixed(digitos).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1");
}
function maskNumero_NoDecimal(numeroInt) {
    return "$" + parseFloat(numeroInt).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}
function maskNumero2DCompras_PERU(numero) {
    let numeroString = parseFloat(numero).toFixed(2);
    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = 'S/ ' + enteros + '.' + decimales;

    return resultado;
}
function maskNumero6DCompras_PERU(numero) {
    let numeroString = parseFloat(numero).toFixed(6);
    let listaString = numeroString.split(".", 2);
    let enteros = listaString[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    let decimales = listaString[1];
    let resultado = 'S/ ' + enteros + '.' + decimales;

    return resultado;
}
function bestRouting(redirect) {
    var host = window.location.hostname;
    let port = window.location.port;
    if (port == "8085" || port == "8086") {
        setBestRouting(4);
    } else if (host == '10.1.0.126') {
        setBestRouting(2);
    }
    else if (host == 'localhost') {
        setBestRouting(1);
    }
    else if (host == '66.175.239.161') {
        setBestRouting(3);
    }
    else {
        AlertaGeneral("Alerta", "Favor de revisar su conexión");
    }

}

function setBestRouting(routing) {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Base/setBestRouting",
        async: false,
        data: { routing: routing },
        success: function (r) {

        },
        error: function (ex) {

        }
    });
}
function originURL(url) {
    return new URL(window.location.origin + url);
}
var headerFetchJson = () => {
    header = new Headers();
    header.method = 'POST';
    header.headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json;charset=utf-8',
    };
    return header;
};
function ejectFetchJson(url, param) {
    $.blockUI({ message: 'Procesando...', baseZ: 9999 });
    let header = headerFetchJson();
    if (param !== null && param !== undefined) {
        header.body = JSON.stringify(param);
    }
    let response = fetch(url, header)
        .then(response => {
            $.unblockUI();
            return response.json();
        }).catch(o_O => { $.unblockUI(); });
    return response;
}
function ejectFetchJsonNoBlock(url, param) {
    let header = headerFetchJson();
    if (param !== null && param !== undefined) {
        header.body = JSON.stringify(param);
    }
    let response = fetch(url, header)
        .then(response => {
            return response.json();
        }).catch(o_O => { });
    return response;
}
var dtDicEsp = {
    "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    }
};
var mpDicEsp = {
    year: 'Año',
    buttonText: 'botón siguiente',
    prevYear: "Año anterior",
    nextYear: "Año siguiente",
    next12Years: 'Siguientes 12 años',
    prev12Years: 'Anteriores 12 años',
    nextLabel: "Siguiente",
    prevLabel: "Anterior",
    jumpYears: "Brincar años",
    backTo: "Selecionado",
    months: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"]
};
var highChartsDicEsp = {
    viewFullscreen: 'Ver en Pantalla Completa',
    exitFullscreen: 'Salir de la Pantalla Completa',
    printChart: 'Imprimir Gráfica',
    downloadPNG: 'Descargar Imagen PNG',
    downloadJPEG: 'Descargar Imagen JPEG',
    downloadPDF: 'Descargar Documento PDF',
    downloadSVG: 'Descargar Imagen SVG',
    downloadCSV: 'Descargar Archivo CSV',
    downloadXLS: 'Descargar Archivo XLS',
    viewData: 'Ver Tabla de Datos',
    hideData: 'Ocultar Tabla de Datos'
};
function seleccionarTodosMultiselect(selector) {
    $(selector).multiselect('selectAll', false);
    $(selector).multiselect('deselect', "Todos");
}
function limpiarMultiselect(selector) {
    $(selector).multiselect("deselectAll", false).multiselect("refresh");
}
function beatHeart(times) {
    var interval = setInterval(function () {
        $(".heartbeat").fadeIn(500, function () {
            $(".heartbeat").fadeOut(500);
        });
    }, 1000); // beat every second

    // after n times, let's clear the interval (adding 100ms of safe gap)
    setTimeout(function () { clearInterval(interval); }, (1000 * times) + 100);
}
function setHeartbeat() {
    setTimeout("heartbeat()", 300000); // every 5 min
}

function heartbeat() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/Home/ProcessRequest",
        async: false,
        success: function (r) {
            beatHeart(2)
            setHeartbeat();
        },
        error: function (ex) {

        }
    });
}
function closeRemoteModal() {
    $(".btnGenericAuthPanel").click();
}
function isRemoteModal() {
    var r = false;
    if ($("#mdlPanelAuth").is(":visible")) {
        r = true;
    }
    return r;
}
var idCRReporte = 0;
var showCRModa = () => {
    let report = document.getElementById('report');
    report.src = `/Reportes/Vista.aspx?inMemory=1&idReporte=${idCRReporte}`;
    report.onload = () => {
        openCRModal();
    }
}
function getArraySplit(myArray, chunk_size) {
    var results = [];

    while (myArray.length) {
        results.push(myArray.splice(0, chunk_size));
    }

    return results;
}

async function _sp_SaveSplit(myArray, chunk_size, url, fnResponse) {

    var response = {};
    response.success = new Array();
    response.error = new Array();
    response.status = 0;
    response.errorJson = "";
    var count = 1;
    while (myArray.length) {
        var data = myArray.splice(0, chunk_size);
        var obj = {};
        obj.loop = count;
        obj.success = await _sp_saveMethod(url, data);
        obj.dataRequest = data;
        if (obj.success) {
            response.success.push(obj);
        }
        else {
            response.error.push(obj);
        }
        count = count++;
    }
    response.errorJson = JSON.stringify(response.error);

    var success = response.success.length;
    var error = response.error.length;

    if (success > 0 && error == 0) {
        response.status = 1;
    }
    else if (success > 0 && error > 0) {
        response.status = 2;
    }
    else {
        response.status = 3;
    }

    _sp_responseMethod(response, fnResponse);
    return response;
}
async function _sp_saveMethod(url, data) {
    try {
        response = await ejectFetchJson(url, data);
        return response.success;
    } catch (o_O) {
        return false;
    }
}
function _sp_responseMethod(data, fnResponse) {
    if (data.status == 1) {
        fnResponse();
        AlertaGeneral("Aviso", "Datos guardados correctamente.");
    }
    else if (data.status == 2) {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "¡No todos los datos se guardaron!, no cierre este mensaje y hable al personal de TI.");
    }
    else {
        console.log(data.error);
        console.log(data.errorJson);
        AlertaGeneral("Aviso", "Ocurrio un error al guardar, no cierre este mensaje y hable al personal de TI.");
    }
}

function animateValue(id, start, end, duration) {
    start = Math.floor(start);
    end = Math.floor(end);
    var range = end - start;
    var current = start;
    var increment = end > start ? 1 : -1;
    var stepTime = Math.abs(Math.floor(duration / range));
    var obj = document.getElementById(id);
    if (start == end) {
        obj.innerHTML = end;
        return;
    }
    var timer = setInterval(function () {
        current += increment;
        obj.innerHTML = current;
        if (current == end) {
            clearInterval(timer);
        }
    }, stepTime);
}
//Tamaño


function Unidades(num) {

    switch (num) {
        case 1: return 'UN';
        case 2: return 'DOS';
        case 3: return 'TRES';
        case 4: return 'CUATRO';
        case 5: return 'CINCO';
        case 6: return 'SEIS';
        case 7: return 'SIETE';
        case 8: return 'OCHO';
        case 9: return 'NUEVE';
    }

    return '';
}//Unidades()

function Decenas(num) {

    decena = Math.floor(num / 10);
    unidad = num - (decena * 10);

    switch (decena) {
        case 1:
            switch (unidad) {
                case 0: return 'DIEZ';
                case 1: return 'ONCE';
                case 2: return 'DOCE';
                case 3: return 'TRECE';
                case 4: return 'CATORCE';
                case 5: return 'QUINCE';
                default: return 'DIECI' + Unidades(unidad);
            }
        case 2:
            switch (unidad) {
                case 0: return 'VEINTE';
                default: return 'VEINTI' + Unidades(unidad);
            }
        case 3: return DecenasY('TREINTA', unidad);
        case 4: return DecenasY('CUARENTA', unidad);
        case 5: return DecenasY('CINCUENTA', unidad);
        case 6: return DecenasY('SESENTA', unidad);
        case 7: return DecenasY('SETENTA', unidad);
        case 8: return DecenasY('OCHENTA', unidad);
        case 9: return DecenasY('NOVENTA', unidad);
        case 0: return Unidades(unidad);
    }
}//Unidades()

function DecenasY(strSin, numUnidades) {
    if (numUnidades > 0)
        return strSin + ' Y ' + Unidades(numUnidades)

    return strSin;
}//DecenasY()

function Centenas(num) {
    centenas = Math.floor(num / 100);
    decenas = num - (centenas * 100);

    switch (centenas) {
        case 1:
            if (decenas > 0)
                return 'CIENTO ' + Decenas(decenas);
            return 'CIEN';
        case 2: return 'DOSCIENTOS ' + Decenas(decenas);
        case 3: return 'TRESCIENTOS ' + Decenas(decenas);
        case 4: return 'CUATROCIENTOS ' + Decenas(decenas);
        case 5: return 'QUINIENTOS ' + Decenas(decenas);
        case 6: return 'SEISCIENTOS ' + Decenas(decenas);
        case 7: return 'SETECIENTOS ' + Decenas(decenas);
        case 8: return 'OCHOCIENTOS ' + Decenas(decenas);
        case 9: return 'NOVECIENTOS ' + Decenas(decenas);
    }

    return Decenas(decenas);
}//Centenas()

function Seccion(num, divisor, strSingular, strPlural) {
    cientos = Math.floor(num / divisor)
    resto = num - (cientos * divisor)

    letras = '';

    if (cientos > 0)
        if (cientos > 1)
            letras = Centenas(cientos) + ' ' + strPlural;
        else
            letras = strSingular;

    if (resto > 0)
        letras += '';

    return letras;
}//Seccion()

function Miles(num) {
    divisor = 1000;
    cientos = Math.floor(num / divisor)
    resto = num - (cientos * divisor)

    strMiles = Seccion(num, divisor, 'UN MIL', 'MIL');
    strCentenas = Centenas(resto);

    if (strMiles == '')
        return strCentenas;

    return strMiles + ' ' + strCentenas;
}//Miles()

function Millones(num) {
    divisor = 1000000;
    cientos = Math.floor(num / divisor)
    resto = num - (cientos * divisor)

    strMillones = Seccion(num, divisor, 'UN MILLON DE', 'MILLONES DE');
    strMiles = Miles(resto);

    if (strMillones == '')
        return strMiles;

    return strMillones + ' ' + strMiles;
}//Millones()

function NumeroALetras(num) {
    var data = {
        numero: num,
        enteros: Math.floor(num),
        centavos: (((Math.round(num * 100)) - (Math.floor(num) * 100))),
        letrasCentavos: ' ',
        letrasMonedaPlural: 'PESOS 00/100 M.N.', //'Dólares', 'Bolívares', 'etcs'
        letrasMonedaSingular: 'PESO 00/100 M.N.', //'PESO', 'Dólar', 'Bolivar', 'etc'
        letrasMonedaCentavoPlural: 'CENTAVOS',
        letrasMonedaCentavoSingular: 'CENTAVO'
    };

    if (data.centavos > 0) {
        data.letrasCentavos = 'CON ' + (function () {
            if (data.centavos == 1)
                return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoSingular;
            else
                return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoPlural;
        })();
    };

    if (data.enteros == 0)
        return 'CERO ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
    if (data.enteros == 1)
        return Millones(data.enteros) + ' ' + data.letrasMonedaSingular + ' ' + data.letrasCentavos;
    else
        return Millones(data.enteros) + ' ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
}//NumeroALetras()

/*
    page: https://sweetalert2.github.io/
    Funciones con SweetAlert2, generales.
*/

/**
    * El comentario comienza con una barra y dos asteriscos.
    * Cada nueva línea lleva un asterisco al comienzo.
        * @param {string} nombre indica que una función recibe un parámetro de tipo string y que el nombre del parámetro es nombre.
        * @descriptor Cada descriptor que añadamos irá en una línea independiente.
*/
function Alert2General(strTitle, strMensaje) {
    Swal.fire({
        position: "center",
        title: strTitle,
        text: strMensaje,
        timer: 5000,
        width: '35%',
        timerProgressBar: true,
        showConfirmButton: false,
        showCloseButton: false
    });
}

function Alert2GeneralPermanente(strTitle, strMensaje) {
    Swal.fire({
        position: "center",
        title: strTitle,
        text: strMensaje,
        width: '35%',
        showConfirmButton: false,
        showCloseButton: true
    });
}

function Alert2Exito(strMensaje) {
    Swal.fire({
        position: "top-end",
        icon: "success",
        title: strMensaje,
        timer: 5000,
        //width: '35%',
        backdrop: false,
        //backdrop: 'swal2-backdrop-hide',
        timerProgressBar: true,
        showConfirmButton: false,
        showCloseButton: true
    });
}

//FUNCIÓN PARA ENVIAR ALERTA QUE LA ACCIÓN QUE SE DESEABA REALIZAR, NO TUVO ÉXITO.
function Alert2Error(strMensaje) {
    Swal.fire({
        position: "center",
        icon: "error",
        title: strMensaje,
        width: '35%',
        showConfirmButton: true,
        showCloseButton: true,
        confirmButtonText: "Cerrar",
        confirmButtonColor: "#6b6b6b"
    });
}

//FUNCIÓN PARA ENVIAR ALERTA DE PRECAUCIÓN.
function Alert2Warning(strMensaje) {
    Swal.fire({
        position: "center",
        icon: "warning",
        title: strMensaje,
        width: '35%',
        showConfirmButton: true,
        showCloseButton: true,
        confirmButtonText: "Cerrar",
        confirmButtonColor: "#6b6b6b"
    });
}

//FUNCIÓN PARA ENVIAR ALERTA DE PRECAUCIÓN.
function Alert2Info(strMensaje) {
    Swal.fire({
        position: "center",
        icon: "info",
        title: strMensaje,
        width: '35%',
        showConfirmButton: true,
        showCloseButton: true,
        confirmButtonText: "Cerrar",
        confirmButtonColor: "#6b6b6b"
    });
}

//
function fillCboCCEnKontrol(inputs) {
    axios.get('/Administrativo/IndicadoresSeguridad/ObtenerComboCCAmbasEmpresas')
        .then(response => {
            let { success, items, message } = response.data;
            if (success) {
                $.each(inputs, function (i, e) {
                    $(e).append('<option value="">--Seleccione--</option>');
                });
                //selectCCRegistro.append('<option value="">--Seleccione--</option>');
                //selectCCFiltros.append('<option value="">--Seleccione--</option>');
                items.forEach(x => {
                    let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                    x.options.forEach(y => {
                        groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                    });
                    //selectCCRegistro.append(groupOption);
                    //selectCCFiltros.append(groupOption);
                    $.each(inputs, function (i, e) {
                        $(e).append(groupOption);
                    });
                });
            } else {
                Alert2Error(`Alerta`, message);
            }
        }).catch(error => Alert2Error(`Alerta`, error.message));
}

function Alert2AccionConfirmar(strTitle, strText, strConfirmar, strCancelar, cbAceptar, strIcono) {
    mdlbtnAceptar = $("#mdlbtnAceptar");

    if (strIcono == null || strIcono == undefined || strIcono == '') {
        strIcono = 'warning';
    }

    Swal.fire({
        position: "center",
        icon: strIcono,
        title: strTitle,
        width: '35%',
        showCancelButton: true,
        html: "<h3>" + strText + "</h3>",
        confirmButtonText: strConfirmar,
        confirmButtonColor: "#5cb85c",
        cancelButtonText: strCancelar,
        cancelButtonColor: "#d9534f",
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            if (cbAceptar) {
                cbAceptar();
            }
        }
    });
}

function Alert2AccionConfirmarInput(strTitle, strText, strConfirmar, strCancelar, cbAceptar, strIcono) {
    mdlbtnAceptar = $("#mdlbtnAceptar");

    if (strIcono == null || strIcono == undefined || strIcono == '') {
        strIcono = 'warning';
    }

    Swal.fire({
        position: "center",
        icon: strIcono,
        title: strTitle,
        input: 'text',
        width: '35%',
        showCancelButton: true,
        html: "<h3>" + strText + "</h3>",
        confirmButtonText: strConfirmar,
        confirmButtonColor: "#5cb85c",
        cancelButtonText: strCancelar,
        cancelButtonColor: "#d9534f",
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            if (cbAceptar) {
                cbAceptar(`${result.value}`);
            }
        }
    });
}

function Alert2AccionConfirmarGeneral(strIcono, strTitle, strText, strConfirmar, strCancelar, cbAceptar, cbCancelar) {
    mdlbtnAceptar = $("#mdlbtnAceptar");
    Swal.fire({
        position: "center",
        icon: strIcono, //warning, info
        title: strTitle,
        width: '35%',
        showCancelButton: true,
        html: "<h3>" + strText + "</h3>",
        confirmButtonText: strConfirmar,
        confirmButtonColor: "#5cb85c",
        cancelButtonText: strCancelar,
        cancelButtonColor: "#d9534f",
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            if (cbAceptar) {
                cbAceptar();
            }
        }
    });
}

function generarImagenGraficaHighcharts(grafica_id, fnCallback) {
    let chart = $(`#${grafica_id}`).highcharts();

    let canvas = document.createElement('canvas');
    canvas.width = 1000;
    canvas.height = canvas.width * chart.chartHeight / chart.chartWidth;

    let image = new Image;

    image.onload = function () {
        canvas.getContext('2d').drawImage(this, 0, 0, canvas.width, canvas.height);

        if (fnCallback) {
            fnCallback([canvas.toDataURL("image/png")]);
        }
    };

    image.src = 'data:image/svg+xml;base64,' + window.btoa(unescape(encodeURIComponent(chart.getSVG({
        exporting: { sourceWidth: chart.chartWidth, sourceHeight: chart.chartHeight }
    }))));
}

function fncDefaultCtrls(selector, esSelect) {
    if (esSelect) {
        $(`#select2-${selector}-container`).css("border", "1px solid #CCC");
    } else {
        $(`#${selector}`).css("border", "1px solid #CCC");
    }
}

function fncValidacionCtrl(selector, esSelect, mensajeError) {
    if (esSelect) {
        $(`#select2-${selector}-container`).css('border', '2px solid red')
    } else {
        $(`#${selector}`).css('border', '2px solid red')
    }
    Alert2Warning(mensajeError)
}