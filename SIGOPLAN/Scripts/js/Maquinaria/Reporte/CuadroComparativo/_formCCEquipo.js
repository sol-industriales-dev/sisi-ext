(() => {
    $.namespace('Maquinaria.Reportes._formCCEquipo');
    //#region Elementos
    let itemsProv, itemsMarca, itemsModelo;
    const tblEquipoLenght = 4, tblCaracteristicasLenght = 8;
    const lblObra = $('#lblObra');
    const lblEquipo = $('#lblEquipo');
    const tblDetalles = $('#tblDetalles');
    const divAdquisicion = $('#divAdquisicion');
    const fechaElaboracion = $('#fechaElaboracion');
    const initFormEquipo = originURL('/CuadroComparativo/initFormEquipo');
    const CuadroComparativoEquipo = originURL('/CuadroComparativo/CuadroComparativoEquipo');
    //#endregion
    _formCCEquipo = function () {
        (() => {
            // initForm();
            $('#mdlFormCCEquipo').on('hidden.bs.modal', function () {
                setCuadroFormDefault();
            });
        })();
        //#region http
        function initForm() {
            axios.get(initFormEquipo)
                .then(response => {
                    let { success, optProveedor, lstAdquisicion, optMarca, optModelo, lstCatalogo } = response.data;
                    if (success) {
                        itemsProv = optProveedor;
                        itemsMarca = optMarca;
                        itemsModelo = optModelo;
                        initRadiosAdquisicion(lstAdquisicion);
                        initTblDetalles(lstCatalogo);
                        setCuadroFormDefault();
                    }
                }).catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        setCuadroComparativoEquipo = idAsignacion => {
            axios.get(CuadroComparativoEquipo.href, { params: { idAsignacion } })
                .then(response => {
                    let { success, cuadro } = response.data;
                    if (success) {
                        setCuadroForm(cuadro);
                    }
                }).then(() => $("#mdlFormCCEquipo").modal("show"))
                .catch(o_O => AlertaGeneral('Aviso', o_O.message));
        }
        //#endregion
        //#region init formularios
        function initRadiosAdquisicion(lstAdquisicion) {
            divAdquisicion.empty();
            lstAdquisicion.forEach(adquisicion => {
                let label = $("<label>", {
                    text: adquisicion.Text
                }),
                    chbAdquision = $(`<input>`, {
                        type: "radio",
                        name: "optAdquisicion",
                        Value: adquisicion.Value
                    });
                label.append(chbAdquision);
                divAdquisicion.append(label);
            });
        }
        function initTblDetalles(lstCatalogo) {
            let lstIdBlanco = [6, 20],
                tblCuerpo = tblDetalles.find("tbody");
            tblCuerpo.empty();
            lstCatalogo.forEach(catalogo => {
                //#region Concepto
                let tr = $(`<tr>`),
                    descripcion = $(`<td>`, {
                        text: catalogo.Descripcion,
                        width: '20em'
                    });
                tr.data(catalogo);
                tr.append(descripcion);
                //#endregion
                //#region Inputs
                for (let i = 0; i < tblEquipoLenght; i++) {
                    let td = $(`<td>`);
                    switch (catalogo.TipoDato) {
                        case 6://Decimal
                            let iptNumber = $(`<input>`, {
                                type: "number",
                                class: `form-control cpto${catalogo.Id} col${i}`
                            });
                            td.append(iptNumber);
                            break;
                        default:
                        case 22://Varchar
                            let iptText = $(`<input>`, {
                                type: "text",
                                class: `form-control cpto${catalogo.Id} col${i}`
                            });
                            td.append(iptText);
                            break;
                        case 25: //combobox
                            let select = $(`<select>`, {
                                class: `form-control cpto${catalogo.Id} col${i}`,
                                width: "100%"
                            });
                            let options = [];
                            switch (catalogo.Id) {
                                case 1:
                                    options = itemsProv;
                                    break;
                                case 2:
                                    options = itemsModelo;
                                case 3:
                                    options = itemsMarca;
                            }
                            select.fillComboItems(options, undefined);
                            td.append(select);
                            break;
                    }
                    tr.append(td);
                }
                tblCuerpo.append(tr);
                //#endregion
                if (lstIdBlanco.includes(catalogo.Id)) {
                    //#region espacio
                    let trBlanco = $(`<tr>`);
                    for (let j = 0; j < tblEquipoLenght + 1; j++) {
                        trBlanco.append("<td>");
                    }
                    tblCuerpo.append(trBlanco);
                    //#endregion
                    //#region Caracteristicas
                    if (catalogo.Id === 20) {
                        let trBlanco = $(`<tr>`);
                        trBlanco.append($("<td>", { text: "Caracteristicas del Equipo" }));
                        for (let j = 0; j < tblEquipoLenght; j++) {
                            trBlanco.append("<td>");
                        }
                        tblCuerpo.append(trBlanco);
                        for (let j = 0; j < tblCaracteristicasLenght; j++) {
                            let trCaracteristica = $(`<tr>`);
                            for (let i = 0; i < tblEquipoLenght + 1; i++) {
                                let td = $(`<td>`);
                                let clase = i === 0 ? "desc" : `caract${j + 1}`
                                let iptText = $(`<input>`, {
                                    type: "text",
                                    class: `form-control ${clase} col${i}`
                                });
                                td.append(iptText);
                                trCaracteristica.append(td);
                            }
                            tblCuerpo.append(trCaracteristica);
                        }
                        tblCuerpo.find("tr:last td:first").html("Aclaraciones");
                    }
                    //#endregion
                }
                tblCuerpo.find("select").select2({
                    width: 'resolve' // need to override the changed default
                });
            });
        }
        //#endregion
        //#region Carga Formulario
        function setCuadroForm(cuadro) {
            divAdquisicion.data({
                Id: cuadro.Id,
                IdAsignacion: cuadro.IdAsignacion,
                Estado: cuadro.Estado
            });
            divAdquisicion.find(`input:radio[name=optAdquisicion][value='${cuadro.IdAdquisicion}']`).prop('checked', true);
            setCuadroFormCaracteristicas(cuadro);
            setCuadroFormConceptos(cuadro);
        }
        function setCuadroFormCaracteristicas({ Equipos }) {
            //#region Descripcion
            let tdDescripciones = tblDetalles.find(`tbody tr  td:first-child input`);
            for (let i = 0; i < tblCaracteristicasLenght - 1; i++) {
                let input = tdDescripciones[i],
                    caracteristica = Equipos[0].Caracteristicas.find(caract => caract.Orden === i + 1);
                if (caracteristica !== undefined) {
                    input.value = caracteristica.Descripcion;
                }
            }
            //#endregion
            //#region Caracteristicas
            Equipos.forEach((equipo, i) => {
                equipo.Caracteristicas.forEach(caracteristica => {
                    let tdInput = tblDetalles.find(`tbody tr td input.caract${caracteristica.Orden}.col${i + 1}`)[0];
                    tdInput.data({
                        Id: caracteristica.Id,
                        Orden: caracteristica.Orden
                    });
                    tdInput.value = caracteristica.Valor;
                });
            });
            //#endregion
        }
        function setCuadroFormConceptos({ Equipos }) {
            let trCptos = [...tblDetalles.find(`tbody tr`)];
            Equipos.forEach((equipo, i) => {
                //#region Equipo
                tblDetalles.find(`tbody tr:eq(0) td:eq(${i + 1})`).data({
                    Id: equipo.Id,
                    IdCuadro: equipo.IdCuadro,
                    EsSeleccionado: equipo.EsSeleccionado
                });
                tblDetalles.find(`tbody tr:eq(0) td:eq(${i + 1}) select`).first().val(equipo.IdProveedor).change();
                tblDetalles.find(`tbody tr:eq(1) td:eq(${i + 1}) select`).first().val(equipo.IdMarca).change();
                tblDetalles.find(`tbody tr:eq(2) td:eq(${i + 1}) select`).first().val(equipo.IdModelo).change();
                //#endregion
                //#region Valores conceptos
                trCptos.filter(trCpto => trCpto.rowIndex > 2)
                    .forEach(trCpto => {
                        let cptoData = $(trCpto).data(),
                            tdInput = $(trCpto).find(`td:eq(${i + 1})`),
                            cptoCuadro = equipo.Valores.find(detalle => detalle.IdConcepto === cptoData.Id);
                        if (cptoCuadro !== undefined) {
                            tdInput.data({
                                Id: cptoCuadro.Id,
                            });
                            switch (cptoData.TipoDato) {
                                case 6:
                                case 22:
                                    $(tdInput).find('input').val(cptoCuadro.Valor);
                                    break;
                                case 25:
                                    $(tdInput).find(`select`).val(cptoCuadro.Valor).change();
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                //#endregion
            });
        }
        //#endregion
        //#region  Obetener Data de formularios
        function getCuadroDataDesdeForm() {
            let dataDiv = divAdquisicion.data();
            let cuadro = {
                Id: dataDiv.Id,
                IdAsignacion: dataDiv.IdAsignacion,
                IdAdquisicion: $('input:radio[name=optAdquisicion]:checked').val(),
                Estado: dataDiv.Estado,
                FechaElaboracion: new Date(),
                Equipos: getEquiposData(),
                Autorizantes: []
            };
            return cuadro;
        }
        function getEquiposData() {
            let equipos = [];
            let trCptos = [...tblDetalles.find(`tbody tr`)];
            for (let i = 1; i < tblEquipoLenght + 1; i++) {
                //#region  Equipo
                let equipoData = tblDetalles.find(`tbody tr:eq(0) td:eq(${i + 1})`).data();
                let equipo = {
                    Id: equipoData.Id,
                    IdCuadro: equipoData.IdCuadro,
                    IdProveedor: tblDetalles.find(`tbody tr:eq(0) td:eq(${i}) select`).first().val(),
                    IdMarca: tblDetalles.find(`tbody tr:eq(1) td:eq(${i}) select`).first().val(),
                    IdModelo: tblDetalles.find(`tbody tr:eq(2) td:eq(${i}) select`).first().val(),
                    Valores: [],
                    Caracteristicas: [],
                    EsSeleccionado: equipoData.EsSeleccionado,
                };
                //#endregion
                //#region Conceptos 
                //#region Valores
                trCptos.filter(trCpto => trCpto.rowIndex > 2 && trCpto.rowIndex < 19)
                    .forEach(trCpto => {
                        let cptoData = $(trCpto).data(),
                            tdInput = $(trCpto).find(`td:eq(${i + 1})`),
                            valorData = tdInput.data(),
                            valor = "";
                        switch (cptoData.TipoDato) {
                            case 6:
                            case 22:
                                valor = $(tdInput).find('input').val();
                                break;
                            case 25:
                                valor = $(tdInput).find(`select`).val();
                                break;
                            default:
                                break;
                        }
                    });
                equipo.Valores.push({
                    Id: valorData.Id,
                    IdEquipo: equipo.Id,
                    IdConcepto: cptoData.Id,
                    Valor: valor,
                });
                //#endregion
                //#region Caracteristicas
                trCptos.filter(trCaract => { trCpto.rowIndex > 19 })
                    .forEach(trCaract => {
                        let tdInput = $(trCaract).find(`td:eq(${i + 1})`),
                            caractData = $(tdInput).data();
                        
                    });
                //#endregion
                //#endregion
                equipos.push(equipo);
            }
            return equipos;
        }
        //#endregion
        //#region FormDefault
        function setCuadroFormDefault() {
            let cuadro = CuadroDefault();
            setCuadroForm(cuadro);
        }
        function CuadroDefault() {
            return {
                Id: 0,
                IdAsignacion: 0,
                IdAdquisicion: 0,
                FechaElaboracion: new Date(),
                Estado: 0,
                Equipos: EquiposDefault(),
                Autorizantes: []
            }
        }
        function EquiposDefault() {
            let equipos = [];
            for (let i = 0; i < tblEquipoLenght; i++) {
                equipos.push({
                    Id: 0,
                    IdCuadro: 0,
                    IdProveedor: 0,
                    IdMarca: 0,
                    IdModelo: 0,
                    EsSeleccionado: false,
                    Valores: ValoresDefault(),
                    Caracteristicas: CaracteristicasDefault()
                });
            }
            return equipos;
        }
        function ValoresDefault() {
            let Valores = [];
            for (let i = 1; i < 18; i++) {
                Valores.push({
                    Id: 0,
                    IdEquipo: 0,
                    IdConcepto: i,
                    Valor: ""
                });
            }
            return Valores;
        }
        function CaracteristicasDefault() {
            let Caracteristicas = [];
            for (let i = 1; i < tblCaracteristicasLenght + 1; i++) {
                Caracteristicas.push({
                    Id: 0,
                    IdEquipo: 0,
                    Orden: i,
                    Descripcion: "",
                    Valor: ""
                });
            }
            return Caracteristicas;
        }
        //#endregion

    }
    $(document).ready(() => {
        Maquinaria.Reportes._formCCEquipo = new _formCCEquipo();
    });
})();