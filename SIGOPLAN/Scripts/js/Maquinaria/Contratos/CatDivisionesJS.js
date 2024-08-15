//Catalogo de divisiones 
//Funciones de cargar, agregar, modificar y deshabilitar campos en la base de datos
(() => {

    $.namespace('Contratos.CatDivisiones');
    const tblAF_DxP_Divisiones = $('#tblAF_DxP_Divisiones');
    const txtDivision = $("#txtDivision");
    const txtAbreviatura = $("#txtAbreviatura");
    const btnNuevo = $("#btnNuevo");
    const btnCrearEditarHorasHombre = $("#btnCrearEditarHorasHombre");
    let dtCatDivisiones;

    CatDivisiones = function () {
        (function init() {
            fncListeners();
            initCatDivisiones();
            fncGetDivisiones();
        })();
    }

    function fncListeners() {
        btnNuevo.click(function () {
            $(".modal-title").text("Nueva Division");
            fncLimpiarFormulario();
            txtDivision.attr("data-id", 0);
        });
        btnCrearEditarHorasHombre.click(function () {
            if (txtDivision.attr("data-id") == 0) {
                fncGuadarDivisiones();

            } else {
                fncEditarDivisiones();
            }

        });
    }

    function initCatDivisiones() {
        dtCatDivisiones = tblAF_DxP_Divisiones.DataTable({
            language: dtDicEsp,
            destroy: false,
            ordering: true,
            paging: true,
            searching: true,
            bFilter: true,
            info: false,
            columns: [
                { data: 'nombre', title: 'División' },
                { data: 'abreviacion', title: 'Abreviasión' },
                {
                    data: " ", render: (data, type, row) => {
                        let botones = ``;
                        botones += `<button type="button" class="btn btn-warning editarDivision" data-id="${row.id}"><i class="fa fa-pen"></i></button>&nbsp;`;
                        botones += `<button type="button" class="btn btn-danger eliminarDivision"  data-id="${row.id}"><i class="fa fa-trash"></i></button>`;
                        return botones;
                    }
                }

                //render: function (data, type, row) { }

            ],
            initComplete: function (settings, json) {
                tblAF_DxP_Divisiones.on('click', '.editarDivision', function () {
                    $("#mdlNuevaDivision").modal('show');
                    let rowData = dtCatDivisiones.row($(this).closest("tr")).data();
                    let id = parseFloat(rowData.id);
                    txtDivision.attr("data-id", id);
                    txtDivision.val(rowData.nombre);
                    txtAbreviatura.val(rowData.abreviacion);
                    $(".modal-title").text("");
                    $(".modal-title").text("EDITAR DIVISION");
                });
                tblAF_DxP_Divisiones.on('click', '.eliminarDivision', function () {
                    let rowData = dtCatDivisiones.row($(this).closest("tr")).data();
                    let id = parseFloat(rowData.id);
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarDivisiones(id));
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' }
            ],
        });
    }

    function fncGetDivisiones() {
        axios.post("GetDivisiones").then(response => {
            let { success, items, message } = response.data;
            console.log(response);
            if (success) {
                dtCatDivisiones.clear().draw();
                dtCatDivisiones.rows.add(response.data.lstCatDivisiones).draw();
            }
            else {
                Alert2Error(message)
            }

        }).catch(error => Alert2Error(error.message));
    }

    function GetParametros() {
        let returnParametros = {
            esActivo: true,
            id: txtDivision.attr("data-id"),
            nombre: txtDivision.val(),
            abreviacion: txtAbreviatura.val(),
        };

        return returnParametros;
    }

    function fncGuadarDivisiones() {
        let parametros = GetParametros();
        if (fncCamposVacios()) {
            Alert2Warning("Es necesario ingresar la división");
        } else {
            axios.post('GuardarDivisiones', parametros).then(response => {
                if (response.data.GuardarDivisiones.estatus==1) {

                    $("#mdlNuevaDivision").modal('hide');
                    Alert2Exito(response.data.GuardarDivisiones.mensaje);
                    fncGetDivisiones();   

                }else if(response.data.GuardarDivisiones.estatus==2){

                     Alert2Warning(response.data.GuardarDivisiones.mensaje);

                }else if(response.data.GuardarDivisiones.estatus==3){

                    Alert2Error(response.data.GuardarDivisiones.mensaje);
               }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncEditarDivisiones() {
        let parametros = GetParametros();
if (fncCamposVacios()) {
    Alert2Warning("Los Campos no pueden quedar vacios");

}
else{
    axios.post('EditarDivisiones', { parametros: parametros }).then(response => {
        let { success, items, message } = response.data;
        if (success) {
            $("#mdlNuevaDivision").modal('hide');
            Alert2Exito("Se ha modificado con éxito la división.");
            fncGetDivisiones();

        }
    }).catch(error => Alert2Error(error.message));
}
        
    }

    function fncCamposVacios() {
        let vacio = false;
        txtDivision.val() == "" ? vacio = true : vacio = false;
        txtAbreviatura.val()==""? vacio = true: vacio = false;     
        return vacio;
    }

    function fncEliminarDivisiones(id) {
        console.log(id)
        axios.post('EliminarDivisiones', { id: id }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                Alert2Exito("Se ha eliminado con éxito la división.");
                fncGetDivisiones();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncLimpiarFormulario() {
        txtDivision.attr("data-id", 0);
        txtDivision.val("");
        txtAbreviatura.val("");
    }

    $(document).ready(() => CatDivisiones = new CatDivisiones())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });

})();