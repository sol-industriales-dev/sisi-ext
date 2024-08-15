(function () {
    $.namespace('maquinaria.reporte.gestionCargoNomCCArre');
    gestionCargoNomCCArre = function () {
        dicEsp = {
            sProcessing: "Procesando...",
            sLengthMenu: "Mostrar _MENU_ registros",
            sZeroRecords: "No se encontraron resultados",
            sEmptyTable: "Ningún dato disponible en esta tabla",
            sInfo: "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            sInfoEmpty: "Mostrando registros del 0 al 0 de un total de 0 registros",
            sInfoFiltered: "(filtrado de un total de _MAX_ registros)",
            sInfoPostFix: "",
            sSearch: "Buscar:",
            sUrl: "",
            sInfoThousands: ",",
            sLoadingRecords: "Cargando...",
            oPaginate: {
                sFirst: "Primero",
                sLast: "Último",
                sNext: "Siguiente",
                sPrevious: "Anterior"
            },
            oAria: {
                sSortAscending: ": Activar para ordenar la columna de manera ascendente",
                sSortDescending: ": Activar para ordenar la columna de manera descendente"
            }
        };
        cboCentroCostos = $("#cboCentroCostos"),
        cboPeriodos = $("#cboPeriodos"),
        cboStatus = $("#cboStatus"),
        tblDataCargosNomina = $('#tblDataCargosNomina');
        ireport = $("#report");
        btnAplicarFiltros = $('#btnAplicarFiltros');
        mdlCalculo = $("#mdlCalculo");
        tblDet = $("#tblDet");
        btnGuardar = $("#btnGuardar");
        btnImprimir = $("#btnImprimir");
        btnVerificar = $("#btnVerificar");
        txtNominaSemanal = $("#txtNominaSemanal");
        txtProyectos = $("#txtProyectos");
        spVerificado = $("#spVerificado");
        txtTotalHH = $("#txtTotalHH");
        txtTotalCargo = $("#txtTotalCargo");

      
        function init() {
            initCbo();
            initTable();
            initTableDet();
            $('[data-toggle="tooltip"]').tooltip();
            $('#fltCaptura').datepicker({ dateFormat: 'dd/mm/yy' });
            btnImprimir.click(verReporte);
            btnAplicarFiltros.click(fnBuscar);  
            btnGuardar.click(ActualizarCargoNominaCC);      
            btnVerificar.click(ActualizarCargoNominaCC);    
            txtNominaSemanal.change(setCalculo);
        }  
        function setCalculo() {
            let nomSem = unmaskNumero(txtNominaSemanal.val()),
                sumaHHPeriodo = 0,
                totalCargoMaquina = 0;
            txtNominaSemanal.val(maskNumero(nomSem));
            dtDet.rows().iterator('row', function (context, i) {
                let node = $(this.row(i).node());
                sumaHHPeriodo += +(node.find("td").eq(3).text());
            });
            dtDet.rows().iterator('row', function (context, i) {
                let node = $(this.row(i).node()),
                    hhPeriodo = +(node.find("td").eq(3).text()),
                    porcentajeCargo = sumaHHPeriodo == 0 ? 0 : (hhPeriodo / sumaHHPeriodo) * 100,
                    porcentajeCargoStringConDosDecimales = (Math.round(100 * porcentajeCargo) / 100).toString() + '%',
                    previo = (porcentajeCargo / 100) * nomSem;
                node.find("td").eq(4).text(porcentajeCargoStringConDosDecimales);
                node.find("td").eq(5).text(maskNumero(previo));
                totalCargoMaquina += previo;
            });
            txtTotalHH.val(sumaHHPeriodo);
            txtTotalCargo.val(maskNumero(totalCargoMaquina));
        }
        function fnBuscar() {
            tblDataCargosNomina.ajax.reload(null, false);
        }
        function initCbo() {
            cboCentroCostos.fillCombo('/CatObra/cboCentroCostosSIGOPLAN', null,false, null);
            cboPeriodos.fillCombo('/CatInventario/FillCboSemanas', null, false, null);
        }
        function initTable() {
            tblDataCargosNomina = $("#tblDataCargosNomina").DataTable({
                ajax: {
                    url: '/RepCargoNominaCCArrendadora/GetGuardados',
                    type: 'POST',
                    dataSrc: 'data',
                    data: function (d) {
                        d.fechaCaptura =  $("#cboPeriodos option:selected").text(),
                        d.proyecto = $('#cboCentroCostos').val(),
                        d.estatus = $("#cboStatus").val()
                    }
                },
                language: dicEsp,
                retrieve: true,
                rowId: 'id',
                scrollX: "100%",
                scrollCollapse: true,
                deferRender: true,
                order: [0, 'asc'],
                initComplete: function (settings, json) {
                    tblDataCargosNomina.on('click', '.btn-descargar', function () {
                        var rowData = tblDataCargosNomina.row($(this).closest('tr')).data();
                        window.location.href = '/RepCargoNominaCCArrendadora/GetArchivo?archivo=' + rowData["archivo"];
                    });
                    tblDataCargosNomina.on('click', '.btn-detalles', function () {
                        setModalDetalle(this.value);
                    });
                },
                columns: [
                    { data: 'proyectosString', title: "Proyectos" },
                    { data: 'periodoInicial', title: "Periodo Inicial" },
                    { data: 'periodoFinal', title: "Periodo Final" },
                    { data: 'totalHH', title: "Horas Hombre Totales" },
                    {
                        data: 'nominaSemanal', title: "Nómina Semanal", render: function (data, type, row, meta) {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'lblVerificado', title: "Estado",
                    },
                    {
                        sortable: false, title: "Detalles",
                        render: function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-detalles btn btn-success glyphicon glyphicon-th-list" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            return html;
                        }
                    },
                    {
                        sortable: false, title: "PDF",
                        render: function (data, type, row, meta) {
                            return `<button class="btn-descargar btn btn-primary glyphicon glyphicon-print ${row.isVerificado ? "" : "disabled"}" type="button" value="${row.id}" style="margin-right: 5px;"></button>`;
                        }
                    }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [1, 2]
                    },
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] }
                ]
            });
        }
        function setModalDetalle(id) {
            $.post('/RepCargoNominaCCArrendadora/getNominaCCYDet', { id: id }).done(function (response) {
                if (response.success) {
                    AddRows(tblDet, response.lstDet);
                    txtProyectos.val(response.lstCC);
                    txtNominaSemanal.val(maskNumero(response.nomina.nominaSemanal));
                    txtNominaSemanal.data().id = id;
                    setCalculo();
                    mdlCalculo.modal("show");
                    setLabelVerificado(response.nomina.isVerificado, response.lblEstatus);
                    btnVerificar.prop("disabled", response.nomina.isVerificado);
                    btnGuardar.prop("disabled", response.nomina.isVerificado);
                    btnImprimir.prop("disabled", response.nomina.nominaSemanal == 0);
                }
            });
        }
        function setLabelVerificado(isVerificado, lblEstatus) {
            spVerificado.removeClass();
            spVerificado.addClass("label");
            spVerificado.addClass(isVerificado ? "label-primary": "label-default");
            spVerificado.text(`${isVerificado ? "" : "No"} ${lblEstatus}`);
        }
        function initTableDet() {
            dtDet = tblDet.DataTable({
                language: dicEsp,
                columns: [
                    { data: 'economico' },
                    { data: 'descripcion' },
                    { data: 'cc' },
                    { data: 'hh', className: 'text-right' },
                    { data: 'cargoP', className: 'text-right' },
                    { data: 'cargoD', className: 'text-right' },
                ]
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        function verReporte(e, selector, data) {
            ireport.attr("src", `/Reportes/Vista.aspx?idReporte=86&idNomina=${txtNominaSemanal.data().id}`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
            e.preventDefault();
        }
        function ActualizarCargoNominaCC() {
            let isVerifica = $(this).data().isverifica;
            ireport.attr("src", `/Reportes/Vista.aspx?idReporte=86&inMemory=1&idNomina=${txtNominaSemanal.data().id}`);
            document.getElementById('report').onload = function () {
                $.post('/RepCargoNominaCCArrendadora/ActualizarCargoNominaCC', { 
                    nomina: getNomina(isVerifica),
                    lstDet: getlstDet()
                }).done(function (response){
                    setModalDetalle(response.id);
                    if(response.isVerificado)
                        AlertaGeneral("Alerta", "Datos guardados. Se a enviado notificaci�n de verificaci�n.");
                    else
                        AlertaGeneral("Alerta", "Datos guardados.");
                });
            };
        }
        function getNomina(isVerifica){
            return {
                id: txtNominaSemanal.data().id,
                nominaSemanal: unmaskNumero(txtNominaSemanal.val()),
                isVerificado: isVerifica
            }
        }
        function getlstDet(){
            let lst = [];
            dtDet.rows().iterator('row', function (context, i) {
                let node = $(this.row(i).node());
                lst.push({
                    id: this.row(i).data().id,
                    cargoP: unmaskNumero(node.find("td").eq(4).text()),
                    cargoD: unmaskNumero(node.find("td").eq(5).text())
                });
            });
            return lst;
        }
        init();
    };
    $(document).ready(function () {
        maquinaria.reporte.gestionCargoNomCCArre = new gestionCargoNomCCArre();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();