(() => {
$.namespace('subContratistas.ControlObra');

    const contenido = $('#contenido');
    const btnGuardar = $('#btnGuardar');

    ControlObra = function (){
        let init = () => {
            CargarConfiguracionVista();
            fncButtons();
        }
        init();
    }
    function fncButtons() {
        btnGuardar.click(function () {
            obtenerValuesInputs(btnGuardar.attr('data-id'));
        })
    }
    function CargarConfiguracionVista() {
        axios.post('obtenerDiviciones', {})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                        fncGenerandoDivicion(items);
                }
            });
    }
    function fncGenerandoDivicion(lstDatos) {
     let html = `   <div class="col-lg-2 col-md-2 col-sm-2" style="margin-top:15px;">
                        <div class=" row">
                            <div class="col-lg-12 col-md-12 col-sm-12" style="padding: 0;">
                                <fieldset class="fieldset-custm">
                                    <legend class="legend-custm">Evaluaciones : </legend>`;
                        for (let index = 0; index < lstDatos.length; index++) {
                            html += `<button class="btn btn-block btn-social btn-primary-menu boton-menu" id="${lstDatos[index].idbutton}" data-id="${lstDatos[index].id}" title="${lstDatos[index].toltips}">
                                        <i class="fa fa-clipboard-check icono-menu"></i> ${lstDatos[index].descripcion}
                                    </button>`;
                        }

            html +=   ` 
                        </fieldset >
                            <fieldset class="fieldset-custm" style="display:none;">
                                <legend class="legend-custm">Indicadores : </legend>
                                <button class="btn btn-block boton-estatus boton-pesimo">
                                    Pesimo (0-25)
                                </button>
                                <button class="btn btn-block boton-estatus boton-malo">
                                    Malo (26-50)
                                </button>
                                <button class="btn btn-block boton-estatus boton-regular">
                                    Regular (51-70)
                                </button>
                                <button class="btn btn-block boton-estatus boton-aceptable">
                                    Aceptable (71-90)
                                </button>
                                <button class="btn btn-block boton-estatus boton-excediendo">
                                    Excediendo las expectativas (91-100)
                                </button>
                            </fieldset>
                        </div>
                    </div>
                </div>`;
                html += `<div class="col-lg-10 col-md-10 col-sm-10" >`;
                for (let index = 0; index < lstDatos.length; index++) {
                
                html += `
                <div id="${lstDatos[index].idsection}"  class="col-md-12">
                    <fieldset class="fieldset-custm">
                        <legend class="legend-custm">${lstDatos[index].descripcion} : <span id="lblErrorProyecto"></span></legend>
                        <div id="${lstDatos[index].idsection+lstDatos[index].id}" ></div>
                    </fieldset>
                </div>`;
                
            }
            html += ` </div>`;
        contenido.append(html);

        for (let index = 0; index < lstDatos.length; index++) {
            $(`#${lstDatos[index].idsection}`).css('display','none');
            $(`#${lstDatos[index].idbutton}`).click(function () {
                for (let i = 0; i < lstDatos.length; i++) {
                    if (lstDatos[index].idbutton == lstDatos[i].idbutton) {
                        $(`#${lstDatos[i].idsection}`).css('display','block');
                        btnGuardar.attr('data-id',lstDatos[i].id);
                        obtenerInputs(lstDatos[index].id,lstDatos[index].idsection+lstDatos[index].id);
                    }else{
                        $(`#${lstDatos[i].idsection}`).css('display','none');
                    }
                    
                }
            })
        }
        if (lstDatos.length != 0) {
            $(`#${lstDatos[0].idsection}`).css('display','block');
            btnGuardar.attr('data-id',lstDatos[0].id);
            obtenerInputs(lstDatos[0].id,lstDatos[0].idsection+lstDatos[0].id);
        }

    }
    function obtenerInputs(id,idsection) {
        axios.post('obtenerRequerimientos', {idDiv:id})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    generarInputs(items,idsection);
                }
            });
    }
    function generarInputs(lstDatos,idsection) {
        console.log(lstDatos)
        console.log(idsection)
        $(`#${idsection}`).find('div').remove();
        if (lstDatos.length != 0) {
            let html = ``;
                for (let index = 0; index < lstDatos.length; index++) {
                    html += `
                    <div class="col-md-12">
                        <div class="col-md-5">
                            <br>
                            <label for="">${lstDatos[index].texto} </label>
                        </div>
                        <div class="col-md-5">
                            <label class="inputs pointer" for='${lstDatos[index].inputFile}'>
                            <img src="https://byspel.com/wp-content/uploads/2017/01/upload-cloud.png" style="width:15%" /> 
                            </label>
                            <input id='${lstDatos[index].inputFile}' type="file" style="display:none;" />
                            <label id="${lstDatos[index].lblInput}">Ningún archivo seleccionado</label>
                        </div>
                     
                    </div>
                        `;
                }
          $(`#${idsection}`).append(html);

          for (let index = 0; index < lstDatos.length; index++) {
            $(`#${lstDatos[index].inputFile}`).change(function () {
                $(`#${lstDatos[index].lblInput}`).text($(this)[0].files[0].name);
            });
              
          }
        }
        console.log('cargarDatos')
        CargarArchivosXSubcontratista(lstDatos)
    }
    function getParameters() {
        let item = {
            tipoEvaluacion:btnGuardar.attr('data-id'),
        }
        return item;
    }
    function CargarArchivosXSubcontratista(lstDatos) {
        let parametros = getParameters();
        axios.post('CargarArchivosXSubcontratista', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    for (let i = 0; i < items.length; i++) {
                        $(`#${lstDatos[i].lblInput}`).text(items[i].rutaArchivo);                        
                        $(`#${lstDatos[i].lblInput}`).attr('data-idEvaluacion',items[i].id);
                                                
                    }
                }
            });
    }


    function obtenerValuesInputs(id) {
        axios.post('obtenerRequerimientos', {idDiv:id})
        .catch(o_O => AlertaGeneral(o_O.message))
        .then(response => {
            let { success, items} = response.data;
            if (success) {
                fncGuardar(items);
            }
        });  
    }
    function CrearFormData(lstDatos) {
        let formData = new FormData();
        let idRow = 0;
            for (let index = 0; index < lstDatos.length; index++) {
                idRow++;
                formData.append("parametros["+index+"][idEvaluacion]",$(`#${lstDatos[index].lblInput}`).attr('data-idEvaluacion'));
                formData.append("parametros["+index+"][idRow]",idRow);
                formData.append("parametros["+index+"][tipoEvaluacion]",btnGuardar.attr('data-id'));
                if (document.getElementById(`${lstDatos[index].inputFile}`).files[0] != null) {
                    formData.append("parametros["+index+"][Archivo]",document.getElementById(`${lstDatos[index].inputFile}`).files[0].name);
                    formData.append("Archivo",document.getElementById(`${lstDatos[index].inputFile}`).files[0]);
                    }
                
            }  

                console.log(formData);
        return formData;
    }
    function fncGuardar(lstDatos){
        let parametros = CrearFormData(lstDatos);
        axios.post('addEditSubContratista', parametros , { headers: { 'Content-Type': 'multipart/form-data' } })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
            });
    }

    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }


    $(document).ready(() => {
        subContratistas.ControlObra = new ControlObra();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();