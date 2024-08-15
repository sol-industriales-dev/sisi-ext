(() => {
    $.namespace('Maquinaria.ActivoFijo.DepreciacionCuentas');
    depreciacionCuentas = function() {
        //Objetos
        const btnModificarDepCuentas = $('#btnModificarDepCuentas');

        //Eventos
        btnModificarDepCuentas.on('click', function() {
            var cuentas = $(this).data('cuentas');
            objDepCuentas = new Array();
            for (let index = 0; index < cuentas; index++) {
                objDepCuenta = {
                    Id: $('#idDepCuenta' + index).data('id'),
                    Cuenta: '',
                    Descripcion: '',
                    PorcentajeDepreciacion: $('#txtPorcentajeDepreciacion' + $('#idDepCuenta' + index).data('id')).val(),
                    MesesDeDepreciacion: $('#txtMesesDepreciacion' + $('#idDepCuenta' + index).data('id')).val()
                }
                objDepCuentas.push(objDepCuenta);
            }
            ModificarDepreciacionCuenta(objDepCuentas);
        });

        $(document).on('keypress', '.txtPorcentajeDepreciacion', function(event){
            aceptaSoloNumeroXDIntMax($(this), event, 2);
        });

        $(document).on('paste', '.txtPorcentajeDepreciacion', function(event){
            permitePegarSoloNumeroXDIntMax($(this), event, 2);
        });

        $(document).on('keypress', '.txtMesesDepreciacion', function(event){
            aceptaSoloNumeroXDIntMax($(this), event, 3);
        });

        $(document).on('paste', '.txtMesesDepreciacion', function(event){
            permitePegarSoloNumeroXDIntMax($(this), event, 3);
        });

        let init = () => {
            checkJS_VERSION();
            getDepreciacionCuentas();
        }

        //Inits
        function checkJS_VERSION() {
            let JS_VERSION = 1;
            if($('#JS_VERSION').val() != JS_VERSION) {
                alert('ELIMINAR CACHE');
            }
        }

        function armarVista(items) {
            var cuentas = 0;
            $('#panelDepreciacionCuentas').find('.panel-body').empty();
            for (let index = 0; index < items.length; index++) {
                $('#panelDepreciacionCuentas').find('.panel-body').append
                    (
                        '<div class="row">' +
                        '<div class="col-sm-6">' +
                        '<label id="idDepCuenta' + index + '" data-id="' + items[index].Id + '">' + items[index].Cuenta + ': ' + items[index].Descripcion + '</label>' +
                        '</div>' +
                        '</div>' +
                        '<div class="row">' +
                        '<div class="col-sm-6">' +
                        '<div class="input-group">' +
                        '<span class="input-group-addon">% Depreciación</span>' +
                        '<input type="text" class="form-control txtPorcentajeDepreciacion" id="txtPorcentajeDepreciacion' + items[index].Id + '" value="' + items[index].PorcentajeDepreciacion * 100 + '" />' +
                        '</div>' +
                        '</div>' +
                        '<div class="col-sm-6">' +
                        '<div class="input-group">' +
                        '<span class="input-group-addon">Meses de depreciación</span>' +
                        '<input type="text" class="form-control txtMesesDepreciacion" id="txtMesesDepreciacion' + items[index].Id + '" value="' + items[index].MesesDeDepreciacion + '" />' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '<br />'
                    );
                cuentas++;
            }
            btnModificarDepCuentas.data('cuentas', cuentas);
        }

        //LLamadas al servidor
        function getDepreciacionCuentas() {
            $.get('/ActivoFijo/GetDepreciacionCuenta', {
            }).always().then(response => {
                if (response.success) {
                    armarVista(response.items);
                }
                else {
                    alert('EROR: ' + response.message)
                }
            }, error => {
                alert('SERVER ERROR: ' + error.statusText);
            });
        }

        function ModificarDepreciacionCuenta(objDepMaquina) {
            $.post('/ActivoFijo/ModificarDepreciacionCuenta', {
                depCuentas: objDepMaquina
            }).always().then(response => {
                if (response.success) {
                    ConfirmacionGeneral('Confirmación', '¡Se actualizaron los datos!');
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + error.statusText);
            });
        }

        init();
    }

    $(document).ready(() => {
        Maquinaria.ActivoFijo.DepreciacionCuentas = new depreciacionCuentas();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();