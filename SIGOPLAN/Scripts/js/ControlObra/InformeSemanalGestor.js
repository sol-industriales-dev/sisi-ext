(function () {
    $.namespace('controlObra.InformeSemanalGestor');

    InformeSemanalGestor = function () {
        const tblPlantillas = $('#tblPlantillas');

        const getPlantillas = () => $.post('/ControlObra/ControlObra/GetPlantillas');


        function init() {
            initTablePlantillas();
            getPlantillas().done(function (response) {
                if (response.success) {
                    AddRows(tblPlantillas, response.items);
                }
            });
        }

        function initTablePlantillas() {
            tblPlantillas.DataTable({
                retrieve: true,
                paging: true,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                aaSorting: [2, 'desc'],
                rowId: 'id',
                scrollY: "600px",
                scrollCollapse: true,
                bLengthChange: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'division', title: 'División' },
                    { data: 'nombreArchivo', title: 'Nombre Archivo' },
                    { data: 'numeroDiapositivas', title: 'Numero diapositivas' },
                    {
                        sortable: false, data: 'id',
                        render: (data, type, row, meta) => `<button class="btn btn-success editar btn-sm" division=${row.division_id} id=${row.id}><i class="far fa-edit"></i> Editar</button>`,
                        title: ''
                    },
                    {
                        sortable: false, data: 'id',
                        render: (data, type, row, meta) => `<button class="btn btn-primary generar  btn-sm" division=${row.division_id} id=${row.id}><i class="far fa-file-powerpoint"></i> Generar archivo</button>`,
                        title: ''
                    }
                ],
                columnDefs: [
                    { targets: 2, width: 50 },
                    { targets: [3, 4], width: 60 }
                ],
                drawCallback: function (settings) {

                    $('button.editar').unbind().click(e => {
                        const boton = $(e.currentTarget);
                        const id = boton.attr('id');
                        const division = boton.attr('division');
                        if (id && division) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlPlantilla = baseUrl + `/ControlObra/ControlObra/InformeSemanalPlantilla?plantilla=${id}`;
                            window.location.href = urlPlantilla;
                        }
                    })

                    $('button.generar').unbind().click(e => {
                        const boton = $(e.currentTarget);
                        const id = boton.attr('id');
                        const division = boton.attr('division');
                        if (id && division) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlInforme = baseUrl + `/ControlObra/ControlObra/InformeSemanal?plantilla=${id}`;
                            window.location.href = urlInforme;
                        }
                    })
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }


        init();
    }


    $(document).ready(function () {
        controlObra.InformeSemanalGestor = new InformeSemanalGestor();
    });
})();