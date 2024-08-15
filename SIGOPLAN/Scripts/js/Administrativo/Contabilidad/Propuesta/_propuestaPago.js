(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta._propuestaPago');
    _propuestaPago = function () {
        let startDate, endDate;
        const tblPropuestaPago = $('#tblPropuestaPago');
        const txtFechaPago = $('#txtFechaPago');
        let init = () => {
            initForm();
            initDataTblPropuestaPago();
        }
        function initForm() {
            txtFechaPago.datepicker({
                firstDay: 1,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    initDataTblPropuestaPago();
                    setSemanaGlobalSelecionada();
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= startDate && date <= endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                },
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            setSemanaGlobalSelecionada();
        }
        const getPropuestaPago = new URL(window.location.origin + '/Administrativo/Propuesta/getPropuestaPago');
        async function initDataTblPropuestaPago() {
            try {
                response = await ejectFetchJson(getPropuestaPago,  {busq: getBusq()});
                if (response.success) {
                    let ultimo = response.lst.pop().cc;
                        setHeader(ultimo, response.lstCC);
                        setPropuesta(response.lst);
                        SumarBody();
                        crearTFoodTotal(ultimo);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function getBusq(){
            let date = txtFechaPago.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7);
            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            };
        }
        tblPropuestaPago.on("mouseenter", "tbody td", function () {
            paintCells($(this).data());
        });
        tblPropuestaPago.on("mouseleave", "tbody td", function () {
            unpaintCells($(this).data());
        });
        tblPropuestaPago.on("click", "input[type=checkbox]", function (e) {
            tblPropuestaPago.find(`tbody th.${this.className}`).not(`.Suma`).closest('tr').toggle("slow");
        });
        tblPropuestaPago.on("keydown", "input[type=text]", function (e) {
            if (event.which === 13) {
                let eq = $(this).parents('td')[0].cellIndex - 1;
                $(this).parents('tr').next().find('input[type=text]').eq(eq).focus().select();
            }
        });
        function paintCells({ cc, grupo }) {
            if (grupo !== undefined && grupo.length > 0)
                tblPropuestaPago.find(`tbody tr td.cc${cc}.${grupo}`).addClass("pintarGrupo")
        }
        function unpaintCells({ cc, grupo }) {
            if (grupo !== undefined && grupo.length > 0)
                tblPropuestaPago.find(`tbody tr td.cc${cc}.${grupo}`).removeClass("pintarGrupo");
        }
        function SumarBody(){
            let lstNoSaldo = ["Encabezado", "SinSaldo", "total"];
            tblPropuestaPago.find(`tbody tr`).not(`td.total`).each((i, tr) => {
                let total = 0;
                Array.from(tr.children).forEach((td, j) => {
                    if(td.tagName !== "TH" && !lstNoSaldo.some(cls => Array.from(td.classList).includes(cls))){
                        total += unmaskNumero(td.innerText);
                    }
                });                
                if(!lstNoSaldo.some(cls => Array.from(tr.children[0].classList).includes(cls))){
                    $(tr).find(`td.total`).text(maskNumero2D(total));
                }
            });
        }
        function SumarFooter() {
            let total = 0;
            tblPropuestaPago.find(`tfoot th.text-right`).not(":last").toArray().forEach(tfood => {
                total += getTdValue(tfood);
            });
            tblPropuestaPago.find(`tfoot th:last`)[0].innerText = maskNumero2D(total);
        }
        function getTdValue(td) {
                return td.children.length > 0 ? unmaskNumero(td.children[0].value) : unmaskNumero(td.innerText);
        }
        function setHeader(lst, lstCC) {
            tblPropuestaPago.find('thead').empty();
            let tr = document.createElement("tr");
            td = document.createElement("th");
            tr.appendChild(td);
            lst.forEach((value, key) => {
                let td = document.createElement("th"),
                    cc = lstCC.find(cc => value.cc === cc.cc);
                td.innerText = cc === undefined ? '' : `${cc.cc} ${cc.descripcion}`;
                tr.appendChild(td);
            });
            let tt = document.createElement("th");
            tt.innerText = "TOTAL";
            tr.appendChild(tt);
            tblPropuestaPago.find('thead').append(tr);
        }
        function crearTFoodTotal(lst) {
            tblPropuestaPago.find('tfoot').empty();
            let tr = document.createElement("tr");
            td = document.createElement("th");
            tr.appendChild(td);
            lst.forEach(({cc, saldo}, key) => {
                let td = document.createElement("th");
                td.className = `cc${cc} text-right`
                td.innerText = maskNumero2D(saldo);
                tr.appendChild(td);
            });
            let tt = document.createElement("th");
            tt.className = `text-right`
            tt.innerText = maskNumero2D(0);
            tr.appendChild(tt);
            tblPropuestaPago.find('tfoot').append(tr);
            SumarFooter();
        }
        function esCbBrupo({ esEscondido, clase }) {
            let esSuma = esEscondido && clase === 'Suma';
            return esSuma;
        }
        function crearCbBrupo({ grupo, desc }) {
            let label = document.createElement("label"),
                input = document.createElement("input");
            input.type = 'checkbox';
            input.className = grupo;
            label.innerHTML = `${input.outerHTML} ${desc}`;
            return label;
        }
        function crearTbodyTh(PropuestaPagoDTO) {
            let th = document.createElement("th");
            th.className = ''
            $(th).data({
                clase: PropuestaPagoDTO.clase,
                desc: PropuestaPagoDTO.desc,
                esEscondido: PropuestaPagoDTO.esEscondido,
                grupo: PropuestaPagoDTO.grupo
            });
            th.innerText = PropuestaPagoDTO.desc;
            th.className = `${PropuestaPagoDTO.clase} ${PropuestaPagoDTO.grupo}`;
            if (esCbBrupo(PropuestaPagoDTO)) {
                th.innerText = '';
                th.appendChild(crearCbBrupo(PropuestaPagoDTO));
            }
            return th;
        }
        function crearTextCell(td, { saldo }) {
            td.innerText = maskNumero2D(saldo);
            td.className += " text-right";
            return td;
        }
        function crearInputCell(td, { saldo, cc, grupo }) {
            let input = document.createElement("input");
            input.value = maskNumero2D(saldo);
            input.className = `cc${cc} text-right `;
            input.type = 'text';
            $(input).data({ cc, saldo, grupo });
            td.appendChild(input);
            return td;
        }
        function crearTbodyTd({ clase, grupo, lstGrupoConca, nivelSuma }, cc) {
            let td = document.createElement("td"),
                claseLst = lstGrupoConca.join(" ");
            td.className = `${grupo} ${clase} cc${cc.cc} nivel${nivelSuma} ${claseLst}`;
            $(td).data({
                clase,
                grupo,
                lstGrupoConca,
                nivelSuma,
                cc: cc.cc
            });
            switch (clase) {
                case "SinSaldo": break;
                case "Saldo":
                    td = crearTextCell(td, cc);
                    break;
                case "SaldoEncabezado":
                    td = crearTextCell(td, cc);
                    break;
                case "Suma":
                    td = crearTextCell(td, cc);
                    break;
                case "Input":
                    td = crearInputCell(td, cc, grupo);
                    break;
                case "InputEncabezado":
                    td = crearInputCell(td, cc, grupo);
                    break;
                default:
                    break;
            }
            return td;
        }
        function crearTbodyTdGrupoTotal({ clase, grupo }, cc) {
            let td = document.createElement("td");
            td.className = `total`;
            if (clase === `Saldo` || clase === `Suma` || clase === `Input` || clase === `InputEncabezado` || clase === `SaldoEncabezado`) {
                td = crearTextCell(td, cc);
            }
            return td;
        }
        function setPropuesta(lst) {
            tblPropuestaPago.find('tbody').empty();
            lst.forEach(PropuestaPagoDTO => {
                var tr = document.createElement("tr");
                let th = crearTbodyTh(PropuestaPagoDTO);
                tr.appendChild(th);
                PropuestaPagoDTO.cc.forEach(PropuestaCCDTO => {
                    let td = crearTbodyTd(PropuestaPagoDTO, PropuestaCCDTO);
                    tr.appendChild(td);
                });
                let tt = crearTbodyTdGrupoTotal(PropuestaPagoDTO, PropuestaPagoDTO.cc[0]);
                tr.appendChild(tt);
                tblPropuestaPago.find('tbody').append(tr);
            });
        }
        setSemanaGlobalSelecionada = () => {
            let date = txtFechaPago.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7),
                diaNombre = endDate.toLocaleDateString("es-MX", { weekday: 'long' }).toUpperCase(),
                diaNumero = endDate.getDate(),
                mesNombre = endDate.toLocaleDateString("es-MX", { month: 'long' }).toUpperCase(),
                anio = endDate.getFullYear();
                txtFechaPago.val(`${diaNombre}, ${diaNumero} DE ${mesNombre} DE ${anio}`);
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                txtFechaPago.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta._propuestaPago = new _propuestaPago();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();