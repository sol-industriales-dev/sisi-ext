
(function () {

    $.namespace('administrativo.proyecciones.Obras');

    Obras = function () {

        var dialog, dialog1;
        TituloConfirmacion = $("#TituloConfirmacion");
        MensajeConfirmacion = $("#MensajeConfirmacion");
        var idGlobalRegistroObras, idRow;
        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        terminacionObraID = 0;
        fupAdjunto = $("#fupAdjunto"),
        tbModalBajaMontoUP = $("#tbModalBajaMontoUP"),
        btnTerminacion = $("#btnTerminacion"),
        modalDatosBaja = $("#modalDatosBaja"),
        ulComentarios = $("#ulComentarios"),

        txtComentarios = $("#txtComentarios"),
        btnAddComentario = $("#btnAddComentario"),

        divVerComentario = $("#divVerComentario"),
        lblNombreDescripcion = $("#lblNombreDescripcion"),
        tbModalAbreviatura = $("#tbModalAbreviatura"),
        tbComentario = $("#tbComentario"),
        tbTotalPorcentaje = $("#tbTotalPorcentaje"),
        tblPorcentajeAplicado = $("#tblPorcentajeAplicado"),
        MensajeConfirmacionModal = $("#MensajeConfirmacionModal"),
        idModalObraDescripcion = $("#idModalObraDescripcion"),
        //Modal Nueva Area*--*
        modalNewArea = $("#modalNewArea"),
        idModalDescripcionAreas = $("#idModalDescripcionAreas"),
        btnGuardarNuevaArea = $("#btnGuardarNuevaArea"),
        //**------*
        /*Modal de Registro nuevo*/
        btnGuardarRegistro = $("#btnGuardarRegistro"),
        idModalObraEscenario = $("#idModalObraEscenario"),
        idModalObraArea = $("#idModalObraArea"),
        modalNewRegistro = $("#modalNewRegistro"),
        ModalObraPrioridad = $("#ModalObraPrioridad"),

        //**-------*
        /*Tabla Principal*/
        tblCapturaObras = $("#tblCapturaObras"),
        lblEncabezadoObra = $("#lblEncabezadoObra"),
        /*Acciones*/
        btnAddArea = $("#btnAddArea"),
        btnAddRegistro = $("#btnAddRegistro");

        /*Selectores de */
        cboPeriodo = $("#cboPeriodo"),
        tbMesesInicio = $("#tbMesesInicio"),
        cboEscenario = $("#cboEscenario");
        /*Acciones*/
        btnGuardarCapturaObra = $("#btnGuardarCapturaObra");
        var myChart;
        /**/
        tbModalCliente = $("#tbModalCliente"),
        modalConfirmacionVerComentarios = $("#modalConfirmacionVerComentarios"),
        cboFiltroEscenarioTblObras = $("#cboFiltroEscenarioTblObras"),
        idModalPalletColorsCO = $("#idModalPalletColorsCO"),
        btnNuevoReponsable = $("#btnNuevoReponsable"),
        modalAltaResponsable = $("#modalAltaResponsable"),
        IDmodalReponsableCO = $("#IDmodalReponsableCO"),

        btnGuardarReponsableCO = $("#btnGuardarReponsableCO");

        btnGuardarNuevaArea = $("#btnGuardarNuevaArea"),

        cboCentroCostos = $("#cboCentroCostos"),
        ckFinanciamiento = $("#ckFinanciamiento"),
        tbPorcentajeFin = $("#tbPorcentajeFin"),
        tableDet = $('#tblCapturaObras').DataTable({});
        tblPorcentajeAplicadoGrid = $('#tblPorcentajeAplicado').DataTable({});
        btnConfirmacionEliminarModal = $("#btnConfirmacionEliminarModal"),
        modalConfirmacionDelete = $("#modalConfirmacionDelete"),
        btnGuardarRow = $("#btnGuardarRow"),
        ModalObraResponsables = $("#ModalObraResponsables"),
        ModalProbabilidad = $("#ModalProbabilidad"),
        ModalMargen = $("#ModalMargen"),
        ModalMonto = $("#ModalMonto"),
        DivMeses = $("#DivMeses");

        listaTd = new Array();
        var ElementoTR;

        $("#idModalPalletColorsCO").colorpicker({
            hideButton: true
        });

        lblModalTituloObra = $("#lblModalTituloObra");
        modalPorcentajeAplicado = $("#modalPorcentajeAplicado"),

        editRow = $(".editRow");

        btnBajaEquipo = $("#btnBajaEquipo");

        tbModalBajaDatosEconomico = $("#tbModalBajaDatosEconomico"),
        tbModalBajaCCotizo = $("#tbModalBajaCCotizo"),
        tbModalMontoUP = $("#tbModalMontoUP"),
        tbModalBajaPlantilla = $("#tbModalBajaPlantilla"),
        tbModalBajaMargen = $("#tbModalBajaMargen"),
        tbModalBajaAnticipoMonto = $("#tbModalBajaAnticipoMonto"),
        tbModalBajaPorcentaje = $("#tbModalBajaPorcentaje"),
        tbModalBajaRetenciones = $("#tbModalBajaRetenciones"),
        tbModalBajaBien = $("#tbModalBajaBien"),
        tbModalBajaMal = $("#tbModalBajaMal"),
        tbModalBajaContactos = $("#tbModalBajaContactos"),
        tbModalBajaComentarios = $("#tbModalBajaComentarios");


        /**/

        /*/*/
        function init() {
            ModalObraPrioridad.fillCombo('/Proyecciones/GetCboPrioridades');
            Chart.defaults.global.legend.display = false;

            idModalObraDescripcion.addClass('required');
            ModalObraPrioridad.addClass('required');
            idModalObraArea.addClass('required');
            cboFiltroEscenarioTblObras.change(FiltrarEscenario);
            // GetCadenaCapturaObras();
            btnGuardarReponsableCO.click(GuardarResponsable);
            //tbMesesInicio.change(LoadTableObras);
            //cboPeriodo.change(LoadTableObras);
            btnAddRegistro.click(OpenModal);
            //  InitModal();
            LoadTableObras();
            btnAddArea.click(OpenModalArea);
            btnGuardarNuevaArea.click(SaveAreas);
            idModalObraArea.fillCombo('/proyecciones/GetCboAreas');
            btnNuevoReponsable.click(OpenNewReponsable);
            btnCargarInfo.click(LoadTableObras);
            btnGuardarCapturaObra.click(getAllData);
            btnGuardarRow.click(guardarRow);
            btnConfirmacionEliminarModal.click(DeleteRow);
            ModalMonto.change(setFormato);

            btnMenuPrincipal.click(LoadTableObras);

            idModalObraEscenario.change(GetEscenario);
            btnAddComentario.click(GuardarComentario);
            btnTerminacion.click(SetInfoTerminacionObra);

            addValidacionesObra();
            cboFiltroEscenarioTblObras.fillCombo('/proyecciones/fillCboEscenarios', { tipo: 1 });

            idModalObraEscenario.fillCombo('/proyecciones/fillCboEscenarios', { tipo: 2 });
        }

        $("#TypeChange").on('keyup', function () {
            tableDet.search(this.value).draw();
        });

        function addValidacionesObra() {

            tbModalBajaDatosEconomico.addClass('required');
            tbModalBajaCCotizo.addClass('required');
            tbModalMontoUP.addClass('required');
            tbModalBajaPlantilla.addClass('required');
            tbModalBajaMargen.addClass('required');
            tbModalBajaAnticipoMonto.addClass('required');
            tbModalBajaPorcentaje.addClass('required');
            tbModalBajaRetenciones.addClass('required');
            tbModalBajaBien.addClass('required');
            tbModalBajaMal.addClass('required');
            tbModalBajaContactos.addClass('required');
            tbModalBajaComentarios.addClass('required');
            tbModalCliente.addClass('requiered');

        }

        function GetEscenario() {

            if (idModalObraEscenario.val() == "A") {
                cboCentroCostos.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Seleccione");

                //
                cboCentroCostos.attr('disabled', false);
            }
            else {

                cboCentroCostos.clearCombo();
                cboCentroCostos.attr('disabled', true);
                cboCentroCostos.append($('<option>', {
                    value: "NA",
                    text: 'NA'
                }));
            }
        }

        function setFormato() {

            var monto = Math.round(Number(removeCommas($(this).val())));
            var nf = new Intl.NumberFormat("es-MX");

            ModalMonto.val(nf.format(monto));
        }

        $('#ckFinanciamiento').change(function () {

            if ($('#ckFinanciamiento').is(":checked")) {
                tbPorcentajeFin.prop('disabled', false);
                tbPorcentajeFin.val(0);
            }
            else {
                tbPorcentajeFin.prop('disabled', true);
                tbPorcentajeFin.val(0);
            }

        });

        function guardarRow() {

            var id = $(this).attr('data-temprow');
            listaMesesInput = $(".CmesRegistro");

            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Area = idModalObraArea.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Codigo = ModalObraResponsables.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Descripcion = idModalObraDescripcion.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Escenario = idModalObraEscenario.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Abreviatura = $('#ModalObraResponsables option:selected').data("prefijo");
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES1 = $(listaMesesInput[0]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES2 = $(listaMesesInput[1]).val();;
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES3 = $(listaMesesInput[2]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES4 = $(listaMesesInput[3]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES5 = $(listaMesesInput[4]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES6 = $(listaMesesInput[5]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES7 = $(listaMesesInput[6]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES8 = $(listaMesesInput[7]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES9 = $(listaMesesInput[8]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES10 = $(listaMesesInput[9]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES11 = $(listaMesesInput[10]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().MES12 = $(listaMesesInput[11]).val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Margen = ModalMargen.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Monto = removeCommas(ModalMonto.val());
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Probabilidad = ModalProbabilidad.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().descripcionEmpleado = $("#ModalObraResponsables :selected").text();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().descripcionObra = $("#idModalObraArea :selected").text();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Comentario = tbComentario.val();
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().CentroCostos = cboCentroCostos.val() != "Seleccione" ? cboCentroCostos.val() : "NA";
            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Prioridad = ModalObraPrioridad.val();

            var cantidad = 0;
            for (var i = 0; i < 12; i++) {
                cantidad += Number($(listaMesesInput[i]).val());
            }

            tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().Total = cantidad;

            if ($('#ckFinanciamiento').is(":checked")) {
                tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().banderaFinanciamiento = true;
                tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().porcentaje = tbPorcentajeFin.val();
            }
            else {
                tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().banderaFinanciamiento = false;
                tableDet.row($(".editRow[data-idRow='" + id + "']").parents('td')).data().porcentaje = 0;
            }
            GetInfoObras(tableDet.data());
            //LoadTableObras();
            getAllData();

        }

        $(document).on('change', ".CmesRegistro", function () {
            cantidad = 0;
            listaMesesInput = $(".CmesRegistro");
            for (var i = 0; i < 12; i++) {
                cantidad += Number($(listaMesesInput[i]).val());

                if (cantidad <= 100) {
                    tbTotalPorcentaje.val(cantidad);
                }
                else {
                    $(listaMesesInput[i]).val(0);
                }

            }

            if (cantidad > 100) {
                $(this).val(0);
                cantidad = 0;
                for (var i = 0; i < 12; i++) {
                    cantidad += Number($(listaMesesInput[i]).val());
                    if (cantidad <= 100) {
                        tbTotalPorcentaje.val(cantidad);
                    }
                }
                AlertaGeneral('alerta', 'La sumatoria es mayor a 100 favor de verificar');
            }
        });

        function SetCenter() {

            idModalObraEscenario.addClass('CenterCombo');
            ModalObraResponsables.addClass('CenterCombo');
            idModalObraArea.addClass('CenterCombo');
            ModalObraPrioridad.addClass('CenterCombo');

            idModalObraDescripcion.addClass('text-center');
            ModalProbabilidad.addClass('text-center');            

            ModalMargen.addClass('text-center');
            ModalMonto.addClass('text-center');
            tbPorcentajeFin.addClass('text-center');
            $(".CmesRegistro").addClass('text-center');
        }

        function RemoveClass() {

            idModalObraEscenario.removeClass('CenterCombo');
            ModalObraResponsables.removeClass('CenterCombo');
            idModalObraArea.removeClass('CenterCombo');
            ModalObraPrioridad.removeClass('CenterCombo');

            idModalObraDescripcion.removeClass('text-center');
            ModalProbabilidad.removeClass('text-center');

            ModalMargen.removeClass('text-center');
            ModalMonto.removeClass('text-center');
            tbPorcentajeFin.removeClass('text-center');
            $(".CmesRegistro").removeClass('text-center');

        }

        $(document).on('click', ".removeRow", function () {
            ElementoTR = $(this).parents('td');
            modalConfirmacionDelete.modal('show');
        });

        $(document).on('click', ".msgRow", function () {

            ElementoTR = $(this).attr('data-idrow');
            btnAddComentario.attr('idRow', ElementoTR);
            LoadDataInfoChat();

        });

        $(document).on('click', ".finishRow", function () {

            ElementoTR = $(this).attr('data-idrow');


            LodaDataInfoBaja(ElementoTR);

            btnTerminacion.attr('data-idrow', ElementoTR);
        });

        $(document).on('click', ".vistaRowPor", function () {
            modalPorcentajeAplicado.modal('show');
            var Row = $(this).parents('td');
            var ObjetoRow = tableDet.row(Row).data();
            var array = [];
            var ObjRowPor = {};
            var ObjRowPor1 = {};
            var ObjRowPorMargen = {};
            var ObjRowPor2 = {};
            var DatosGrafica = [];

            lblNombreDescripcion.text(ObjetoRow.Descripcion);
            m1 = ObjetoRow.MES1 * ObjetoRow.Monto / 100;
            m2 = ObjetoRow.MES2 * ObjetoRow.Monto / 100;
            m3 = ObjetoRow.MES3 * ObjetoRow.Monto / 100;
            m4 = ObjetoRow.MES4 * ObjetoRow.Monto / 100;
            m5 = ObjetoRow.MES5 * ObjetoRow.Monto / 100;
            m6 = ObjetoRow.MES6 * ObjetoRow.Monto / 100;
            m7 = ObjetoRow.MES7 * ObjetoRow.Monto / 100;
            m8 = ObjetoRow.MES8 * ObjetoRow.Monto / 100;
            m9 = ObjetoRow.MES9 * ObjetoRow.Monto / 100;
            m10 = ObjetoRow.MES10 * ObjetoRow.Monto / 100;
            m11 = ObjetoRow.MES11 * ObjetoRow.Monto / 100;
            m12 = ObjetoRow.MES12 * ObjetoRow.Monto / 100;

            ObjRowPorMargen.id = 4;
            ObjRowPorMargen.MES1 = m1 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES2 = m2 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES3 = m3 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES4 = m4 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES5 = m5 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES6 = m6 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES7 = m7 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES8 = m8 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES9 = m9 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES10 = m10 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES11 = m11 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.MES12 = m12 == 0 ? "0%" : (ObjetoRow.Margen + "%"),
            ObjRowPorMargen.Total = "";

            ObjRowPor2.id = 3;
            ObjRowPor2.MES1 = RegresarFormatoEntreMilesPesos(((m1 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES2 = RegresarFormatoEntreMilesPesos(((m2 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES3 = RegresarFormatoEntreMilesPesos(((m3 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES4 = RegresarFormatoEntreMilesPesos(((m4 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES5 = RegresarFormatoEntreMilesPesos(((m5 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES6 = RegresarFormatoEntreMilesPesos(((m6 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES7 = RegresarFormatoEntreMilesPesos(((m7 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES8 = RegresarFormatoEntreMilesPesos(((m8 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES9 = RegresarFormatoEntreMilesPesos(((m9 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES10 = RegresarFormatoEntreMilesPesos(((m10 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES11 = RegresarFormatoEntreMilesPesos(((m11 * ObjetoRow.Margen) / 100));
            ObjRowPor2.MES12 = RegresarFormatoEntreMilesPesos(((m12 * ObjetoRow.Margen) / 100));
            ObjRowPor2.Total = RegresarFormatoEntreMilesPesos(((m1 * ObjetoRow.Margen) / 100) +
                               ((m2 * ObjetoRow.Margen) / 100) +
                               ((m3 * ObjetoRow.Margen) / 100) +
                               ((m4 * ObjetoRow.Margen) / 100) +
                               ((m5 * ObjetoRow.Margen) / 100) +
                               ((m6 * ObjetoRow.Margen) / 100) +
                               ((m7 * ObjetoRow.Margen) / 100) +
                               ((m8 * ObjetoRow.Margen) / 100) +
                               ((m9 * ObjetoRow.Margen) / 100) +
                               ((m10 * ObjetoRow.Margen) / 100) +
                               ((m11 * ObjetoRow.Margen) / 100) +
                               ((m12 * ObjetoRow.Margen) / 100));



            ObjRowPor.id = 1;
            ObjRowPor.MES1 = ObjetoRow.MES1 + "%",
            ObjRowPor.MES2 = ObjetoRow.MES2 + "%",
            ObjRowPor.MES3 = ObjetoRow.MES3 + "%",
            ObjRowPor.MES4 = ObjetoRow.MES4 + "%",
            ObjRowPor.MES5 = ObjetoRow.MES5 + "%",
            ObjRowPor.MES6 = ObjetoRow.MES6 + "%",
            ObjRowPor.MES7 = ObjetoRow.MES7 + "%",
            ObjRowPor.MES8 = ObjetoRow.MES8 + "%",
            ObjRowPor.MES9 = ObjetoRow.MES9 + "%",
            ObjRowPor.MES10 = ObjetoRow.MES10 + "%",
            ObjRowPor.MES11 = ObjetoRow.MES11 + "%",
            ObjRowPor.MES12 = ObjetoRow.MES12 + "%",
            ObjRowPor.Total = (Number(ObjetoRow.MES1) +
                              Number(ObjetoRow.MES2) +
                              Number(ObjetoRow.MES3) +
                              Number(ObjetoRow.MES4) +
                              Number(ObjetoRow.MES5) +
                              Number(ObjetoRow.MES6) +
                              Number(ObjetoRow.MES7) +
                              Number(ObjetoRow.MES8) +
                              Number(ObjetoRow.MES9) +
                              Number(ObjetoRow.MES10) +
                              Number(ObjetoRow.MES11) +
                              Number(ObjetoRow.MES12)) + "%";


            ObjRowPor1.MES1 = RegresarFormatoEntreMilesPesos((m1));
            ObjRowPor1.MES2 = RegresarFormatoEntreMilesPesos((m2));
            ObjRowPor1.MES3 = RegresarFormatoEntreMilesPesos((m3));
            ObjRowPor1.MES4 = RegresarFormatoEntreMilesPesos((m4));
            ObjRowPor1.MES5 = RegresarFormatoEntreMilesPesos((m5));
            ObjRowPor1.MES6 = RegresarFormatoEntreMilesPesos((m6));
            ObjRowPor1.MES7 = RegresarFormatoEntreMilesPesos((m7));
            ObjRowPor1.MES8 = RegresarFormatoEntreMilesPesos((m8));
            ObjRowPor1.MES9 = RegresarFormatoEntreMilesPesos((m9));
            ObjRowPor1.MES10 = RegresarFormatoEntreMilesPesos((m10));
            ObjRowPor1.MES11 = RegresarFormatoEntreMilesPesos((m11));
            ObjRowPor1.MES12 = RegresarFormatoEntreMilesPesos((m12));

            var Total =
                (ObjetoRow.MES1 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES2 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES3 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES4 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES5 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES6 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES7 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES8 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES9 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES10 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES11 * ObjetoRow.Monto) / 100 +
                (ObjetoRow.MES12 * ObjetoRow.Monto) / 100;

            ObjRowPor1.Total = RegresarFormatoEntreMilesPesos(Total)

            DatosGrafica.push(m1);
            DatosGrafica.push(m2);
            DatosGrafica.push(m3);
            DatosGrafica.push(m4);
            DatosGrafica.push(m5);
            DatosGrafica.push(m6);
            DatosGrafica.push(m7);
            DatosGrafica.push(m8);
            DatosGrafica.push(m9);
            DatosGrafica.push(m10);
            DatosGrafica.push(m11);
            DatosGrafica.push(m12);

            ObjRowPor1.id = 2;

            ObjRowPor1.Tipo = "Venta";
            ObjRowPor.Tipo = "Recuperación";
            ObjRowPorMargen.Tipo = "Margen";
            ObjRowPor2.Tipo = "Ut. Op.";
            array.push(ObjRowPor1);
            array.push(ObjRowPor);
            array.push(ObjRowPorMargen);
            array.push(ObjRowPor2);

            GetInfoPorcentajeAplicado(array);
            setGrafica(DatosGrafica);
        });

        function SetInfoTerminacionObra() {

            if (GetValidacionesTerminacion()) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/proyecciones/SetInfoTerminacionObra',
                    type: 'POST',
                    dataType: 'json',
                    data: { obj: getDataTerminacion() },
                    success: function (response) {


                        if (response.success) {
                            modalDatosBaja.modal('hide');
                            AlertaGeneral('El registro fue guardado correctamente');
                        }
                        else {
                            AlertaGeneral('Ocurrio un error al momento de guardar.');
                        }

                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }

        }

        function GetValidacionesTerminacion() {

            var state = true;
            if (!validarCampo(tbModalBajaDatosEconomico)) { state = false; }
            if (!validarCampo(tbModalBajaCCotizo)) { state = false; }
            if (!validarCampo(tbModalMontoUP)) { state = false; }
            if (!validarCampo(tbModalBajaPlantilla)) { state = false; }
            if (!validarCampo(tbModalBajaMargen)) { state = false; }
            if (!validarCampo(tbModalBajaAnticipoMonto)) { state = false; }
            if (!validarCampo(tbModalBajaPorcentaje)) { state = false; }
            if (!validarCampo(tbModalBajaRetenciones)) { state = false; }
            if (!validarCampo(tbModalBajaBien)) { state = false; }
            if (!validarCampo(tbModalBajaMal)) { state = false; }
            if (!validarCampo(tbModalBajaContactos)) { state = false; }
            if (!validarCampo(tbModalBajaComentarios)) { state = false; }

            return state;
        }

        function getDataTerminacion() {

            return {
                id: terminacionObraID,
                capturadeObrasID: idGlobalRegistroObras,
                registroID: btnTerminacion.attr('data-idrow'),
                fecha: new Date().toLocaleString(),
                DatosEconomicos: tbModalBajaDatosEconomico.val(),
                CuantoCotizo: tbModalBajaCCotizo.val(),
                MontoUtilidad: tbModalBajaMontoUP.val(),
                CantidadPersonal: tbModalBajaPlantilla.val(),
                Margen: tbModalBajaMargen.val(),
                AnticipoMonto: tbModalBajaAnticipoMonto.val(),
                MontoUtilidad: tbModalMontoUP.val(),
                Porcentaje: tbModalBajaPorcentaje.val(),
                Retenciones: tbModalBajaRetenciones.val(),
                Bien: tbModalBajaBien.val(),
                Mal: tbModalBajaMal.val(),
                Contactos: tbModalBajaContactos.val(),
                Comentarios: tbModalBajaComentarios.val(),
                Cliente: tbModalCliente.val()
            }

        }

        function LodaDataInfoBaja(rowID) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GetInfoTerminacionObra',
                type: 'POST',
                dataType: 'json',
                data: { id: idGlobalRegistroObras, idRow: rowID },
                success: function (response) {
                    modalDatosBaja.modal("show");
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function LoadDataInfoChat() {
            // idTituloContainer.text('Captura de Obra ' + mes[0]);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/SetComentariosObra',
                type: 'POST',
                dataType: 'json',
                data: { id: idGlobalRegistroObras, idRow: btnAddComentario.attr('idRow') },
                success: function (response) {

                    setComentarios(response.comentarios);
                    txtComentarios.val("");
                    divVerComentario.modal("show");
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarComentario1() {
            // idTituloContainer.text('Captura de Obra ' + mes[0]);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarComentario',
                type: 'POST',
                dataType: 'json',
                data: { obj: GetObjComentarios(btnAddComentario.attr('idRow')) },
                success: function (response) {

                    setComentarios(response.data);
                    txtComentarios.val("");

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function GuardarComentario(e) {
            if (true) {
                var obj = GetObjComentarios(btnAddComentario.attr('idRow'));

                var formData = new FormData();
                var file = document.getElementById("fupAdjunto").files[0];

                formData.append("fupAdjunto", file);
                formData.append("obj", JSON.stringify(obj));

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: "/proyecciones/guardarComentarioArchivo",
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        fupAdjunto.val("");
                        $.unblockUI();
                        setComentarios(response.obj);
                        txtComentarios.val("");
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function GetObjComentarios(idRow) {

            date = new Date();
            return {
                id: 0,
                capturadeObrasID: idGlobalRegistroObras,
                registroID: idRow,
                fecha: (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear(),
                comentario: txtComentarios.val(),
                usuarioNombre: "",
                usuarioID: 13,
                estatusComentario: true
            }
        }

        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                if (e.adjuntoNombre != null && e.adjuntoNombre != "") {
                    htmlComentario += "        <div class='timeline-footer'>";
                    htmlComentario += "             <a href='/Proyecciones/getComentarioArchivoAdjunto/?id=" + e.id + "' class='openComentarios'></span>Descargar: " + e.adjuntoNombre + "</a>";
                    htmlComentario += "        </div>";
                }
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }

        function setGrafica(DatosGrafica) {
            var datah = GetPeriodoMeses();
            var data = {
                labels: datah,
                datasets: [{
                    data: DatosGrafica,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                }]
            };
            var ctx = document.getElementById("LineWithLine");

            myChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true

                }
            });

            myChart.resize();
        }


        function RegresarFormato(valor) {
            var monto = Math.round(valor);
            var nf = new Intl.NumberFormat("es-MX");

            return nf.format(monto);
        }


        function RegresarFormatoEntreMilesPesos(valor) {

            var monto = Math.round(valor / 1000);
            var nf = new Intl.NumberFormat("es-MX");

            return nf.format(monto);
        }

        function DeleteRow() {

            modalConfirmacionDelete.modal('hide');
            tableDet.row(ElementoTR).remove().draw()
            getAllDataDelete();

        }

        function OpenModal() {
            idModalObraEscenario.fillCombo('/proyecciones/fillCboEscenarios', { tipo: 2 });
            tbTotalPorcentaje.val('');
            GetEscenario();
            var tipo = $(this).attr('data-tipo');
            if (tipo == "1") {
                lblModalTituloObra.text('Nueva Obra');
                btnGuardarRegistro.removeClass('hide');
                btnGuardarRow.addClass('hide');
            }
            else {
                lblModalTituloObra.text('Editar Obra');
                btnGuardarRegistro.addClass('hide');
                btnGuardarRow.removeClass('hide');
            }

            idModalObraArea.fillCombo('/Proyecciones/GetCboAreas');
            
            ModalObraResponsables.fillCombo('/proyecciones/GetResponsables');
            var listaMeses = GetPeriodoMeses();
            SetValueVacios();
            DivMeses.empty();
            for (var i = 0; i < listaMeses.length; i++) {

                var div = "<div class='col-xs-12 col-sm-12 col-md-2 col-lg-2 form-group'>" +
                        "<label>% " + listaMeses[i] + " </label>  <span class='glyphicon glyphicon-arrow-right autoFill'></span>" +
                        "<input type='number' max='100' min='0' class='form-control CmesRegistro text-center' data-id='" + i + "'>" +
                 "</div>";
                DivMeses.append(div);

            }
            $(".autoFill").on('click', function () {

                var valorInicial = Number($(this).siblings('input').attr('data-id'));
                var SetValor = Number($(this).siblings('input').val());

                for (var i = valorInicial ; i < listaMeses.length; i++) {
                    $("[data-id=" + i + "]").val(SetValor);
                }

                $(this).siblings('input').trigger('change')
            });

            modalNewRegistro.modal("show");


            if (tipo == "1") {
                SetCenter();
            }
        }

        function OpenModalArea() {
            idModalDescripcionAreas.val('');
            modalNewArea.modal("show");
        }

        function SetValueVacios() {

            idModalObraEscenario.val('A');
            idModalObraDescripcion.val('');
            ModalProbabilidad.val('');
            ModalMargen.val('');
            ModalMonto.val('');
            ModalObraPrioridad.val('');


        }

        function LoadTableObras() {
            var mes = GetPeriodoMeses();
            // idTituloContainer.text('Captura de Obra ' + mes[0]);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GetFillTable',
                type: 'POST',
                dataType: 'json',
                data: { escenario: cboFiltroEscenarioTblObras.val() != "" ? cboFiltroEscenarioTblObras.val() : 0, meses: tbMesesInicio.val(), anio: cboPeriodo.val() },
                //data: { escenario: '1', meses: 5, anio: 2017 },
                success: function (response) {
                    tableDet.clear().draw();
                    if (response.EstadoRegreso > 0) {
                        var dataRes = response.GetData;
                        idGlobalRegistroObras = response.id;
                        idRow = response.idRow;
                        GetInfoObras(dataRes);
                        if (response.EstadoRegreso == 1) {
                            btnGuardarCapturaObra.removeClass('hide')
                        } else {
                            btnGuardarCapturaObra.addClass('hide');
                        }
                    } else {
                        GetInfoObras(null);
                        btnGuardarCapturaObra.addClass('hide');
                    }

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SaveAreas() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarNuevaArea',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: idModalDescripcionAreas.val() }),
                success: function (response) {
                    idModalDescripcionAreas.val('');

                    $.unblockUI();
                    AlertaGeneral('Confirmacióon', 'El area guardada correctamente.');
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarResponsable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarResponsable',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: getPlainObject() }),
                success: function (response) {
                    $.unblockUI();

                    IDmodalReponsableCO.val('');
                    idModalPalletColorsCO.val('');
                    modalAltaResponsable.modal('hide');

                    AlertaGeneral('Confirmacióon', 'El responsable fue dado de alta correctamente.');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getPlainObject() {
            return {
                id: 0,
                responsableID: IDmodalReponsableCO.attr('data-id'),
                Descripcion: IDmodalReponsableCO.val(),
                Color: idModalPalletColorsCO.val(),
                Abreviatura: tbModalAbreviatura.val()
            }
        }

        function GetInfoObras(dataSet) {

            var tamañoY = '40vh';
            var clase = btnMenuPrincipal.attr('class');
            if (clase == "collapsed") {
                var Tamaño = ($(window).width() * 53) / 1366;
                tamañoY = Tamaño + 'vh';
            }
            else {
                var Tamaño = ($(window).width() * 37) / 1366;
                tamañoY = Tamaño + 'vh';
            }

            var tituloMeses = [];
            var date = new Date();
            lblEncabezadoObra.text("Obras Consideradas " + (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            tituloMeses = GetPeriodoMeses();
            tableDet = $('#tblCapturaObras').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                },
                destroy: true,
                scrollY: tamañoY,
                data: dataSet,
                columns: [
                     {
                         data: "accion", "width": "85px",
                         createdCell: function (td, cellData, rowData, row, col) {
                             $(td).css("padding", 0);
                         }
                     },
                     {
                         data: "Escenario", "width": "40px",
                         createdCell: function (td, cellData, rowData, row, col) {
                             $(td).addClass('CLSEscenario');
                             $(td).addClass(cellData);
                             $(td).addClass('text-center');
                         }
                     },
                    {
                        data: "Area", "width": "40px", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');



                            $(td).attr('title', rowData.descripcionObra);
                        }
                    },
                    {
                        data: "Prioridad", "width": "40px", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');

                        }
                    },
                    {
                        data: "Codigo", "width": "40px", createdCell: function (td, cellData, rowData, row, col) {
                            var empleado = rowData.descripcionEmpleado == "0" ? "" : rowData.descripcionEmpleado;
                            var abreviatura = rowData.Abreviatura == "" ? "" : rowData.Abreviatura;
                            $(td).addClass('CLSCodigo');
                            $(td).attr('title', empleado);
                            $(td).attr('data-codigo', cellData);
                            $(td).text(abreviatura);

                            $(td).css("background", cellData);
                        }
                    },

                    {
                        data: "Descripcion", "width": "200px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).attr('data-Fin', rowData.banderaFinanciamiento);
                            $(td).attr('data-Por', rowData.porcentaje);
                            $(td).attr('title', rowData.Comentario);
                            if (rowData.Comentario != null) {
                                $(td).css('color', 'blue');
                            }


                        }
                    },
                    {
                        data: "Probabilidad", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center')
                            $(td).text(cellData + "%");
                        }
                    },
                    {
                        data: "Margen", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-center');
                            $(td).text(cellData + "%");
                        }
                    },
                    {
                        data: "Monto", "width": "150px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-right');
                            var monto = Math.round(Number(cellData));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text('$' + nf.format(monto));
                        }
                    }
                ],
                "paging": false,
                "info": false,
                "drawCallback": function (settings) {
                    // $(".msgRow").hide();
                    // $(".finishRow").hide();
                },
                "footerCallback": function (row, data, start, end, display) {
                    var nf = new Intl.NumberFormat("es-MX");
                    var api = this.api(), data;


                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {

                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    // Total over all pages
                    total = api
                        .column(8)
                        .data()
                        .reduce(function (a, b) {

                            return intVal(a) + intVal(b);
                        }, 0);

                    // Total over this page

                    // Update footer
                    $(api.column(6).footer()).html(

                        '$' + nf.format(total)
                    );
                }
            });
            clickTd();
            var previous;

            tableDet.on('focusin', '.mes', function () {
                previous = $(this).val();
            }).on('change', '.mes', function () {
                elemento = $(this);
                sumar(elemento, previous);
            });


            $('#tblCapturaObras tbody').on('click', 'td.bg-green', function () {
                $(this).addClass('bg-white');
                $(this).removeClass('bg-green');
                $(this).attr('data-codigo', 1);
            });

            $('#tblCapturaObras tbody').on('click', 'td.bg-white', function () {
                $(this).addClass('bg-green');
                $(this).removeClass('bg-white');
                $(this).attr('data-codigo', 0);
            });

            FiltrarEscenario();

            $(".editRow").on('click', function () {

                tbTotalPorcentaje.val('');

                lblModalTituloObra.text('Editar Obra');


                var DatoRow = $(this).parents('td').children().children('.editRow').attr('data-idRow');

                btnGuardarRow.attr('data-tempRow', DatoRow);

                var ObjetoTable = tableDet.row($(this).parents('td')).data();

                OpenModal();
                var monto = Math.round(ObjetoTable.Monto);
                var nf = new Intl.NumberFormat("es-MX");
                id = ObjetoTable.id;

                idModalObraEscenario.val(ObjetoTable.Escenario)
                idModalObraDescripcion.val(ObjetoTable.Descripcion);
                idModalObraArea.val(ObjetoTable.Area);
                ModalObraResponsables.val(ObjetoTable.Codigo);
                ModalProbabilidad.val(ObjetoTable.Probabilidad);
                ModalMargen.val(ObjetoTable.Margen);
                ModalMonto.val(nf.format(monto));
                tbTotalPorcentaje.val(ObjetoTable.Total);

                if (ObjetoTable.banderaFinanciamiento) {
                    ckFinanciamiento.prop('checked', true);
                    tbPorcentajeFin.val(ObjetoTable.porcentaje).prop('disabled', false);
                }
                else {
                    ckFinanciamiento.prop('checked', false);
                    tbPorcentajeFin.val(0).prop('disabled', true);
                }

                listaMesesInput = $(".CmesRegistro");
                tbComentario.val(ObjetoTable.Comentario);
                $(listaMesesInput[0]).val(ObjetoTable.MES1);
                $(listaMesesInput[1]).val(ObjetoTable.MES2);
                $(listaMesesInput[2]).val(ObjetoTable.MES3);
                $(listaMesesInput[3]).val(ObjetoTable.MES4);
                $(listaMesesInput[4]).val(ObjetoTable.MES5);
                $(listaMesesInput[5]).val(ObjetoTable.MES6);
                $(listaMesesInput[6]).val(ObjetoTable.MES7);
                $(listaMesesInput[7]).val(ObjetoTable.MES8);
                $(listaMesesInput[8]).val(ObjetoTable.MES9);
                $(listaMesesInput[9]).val(ObjetoTable.MES10);
                $(listaMesesInput[10]).val(ObjetoTable.MES11);
                $(listaMesesInput[11]).val(ObjetoTable.MES12);
                cboCentroCostos.val(ObjetoTable.CentroCostos);
                ModalObraPrioridad.val(ObjetoTable.Prioridad);
            });
        }


        function GetInfoPorcentajeAplicado(dataSet) {
            var tituloMeses = [];
            var date = new Date();
            tituloMeses = GetPeriodoMeses();
            tblPorcentajeAplicadoGrid = $('#tblPorcentajeAplicado').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                },
                "ordering": false,
                "bFilter": false,
                destroy: true,
                data: dataSet,
                columns: [
                    { "title": "Descripción", data: "Tipo" },
                    { "title": tituloMeses[0], data: "MES1" },
                    { "title": tituloMeses[1], data: "MES2" },
                    { "title": tituloMeses[2], data: "MES3" },
                    { "title": tituloMeses[3], data: "MES4" },
                    { "title": tituloMeses[4], data: "MES5" },
                    { "title": tituloMeses[5], data: "MES6" },
                    { "title": tituloMeses[6], data: "MES7" },
                    { "title": tituloMeses[7], data: "MES8" },
                    { "title": tituloMeses[8], data: "MES9" },
                    { "title": tituloMeses[9], data: "MES10" },
                    { "title": tituloMeses[10], data: "MES11" },
                    { "title": tituloMeses[11], data: "MES12" },
                    { data: "Total" }
                ],

                "paging": false,
                "info": false
            });


        }

        $('#btnGuardarRegistro').on('click', function () {
            var state = true;
            if (!validarCampo(idModalObraDescripcion)) { state = false; }

            if (!validarCampo(idModalObraArea)) { state = false; }

            if (!validarCampo(ModalObraPrioridad)) { state = false; }

            if ($('#ckFinanciamiento').is(":checked")) {
                tbPorcentajeFin.addClass('required');
                if (!validarCampo(tbPorcentajeFin)) { state = false; }

            } else {
                tbPorcentajeFin.removeClass('required');
            }

            if (state) {
                AgregarRegistroGuardar(tableDet);
            }
            else {
                AlertaGeneral("Alerta", "Los Campos en rojo son requeridos");
            }

        });

        function FiltrarEscenario() {

            var html = $('.ClsTotal')[1]
            var nf = new Intl.NumberFormat("es-MX");
            var arreglo = $(".CLSEscenario");
            for (var i = 0 ; i < arreglo.length; i++) {
                if ($('option:selected', cboFiltroEscenarioTblObras).attr('data-prefijo') == undefined) {
                    $(arreglo[i]).parents('tr').removeClass('hide');
                }
                else {
                    if ($(arreglo[i]).hasClass($('option:selected', cboFiltroEscenarioTblObras).attr('data-prefijo'))) {
                        $(arreglo[i]).parents('tr').removeClass('hide');
                    } else { $(arreglo[i]).parents('tr').addClass('hide'); }
                }
            }

            if ($('option:selected', cboFiltroEscenarioTblObras).attr('data-prefijo') == undefined)
            { tableDet.draw(); }
            else {
                Total = GetTotal($('option:selected', cboFiltroEscenarioTblObras).attr('data-prefijo'));
                $(html).text(nf.format(Total));
            }



            /*  switch (cboFiltroEscenarioTblObras.val()) {
                  case "1":
                      $(".CLSEscenarioA").parents('tr').removeClass('hide');
                      $(".CLSEscenarioB").parents('tr').addClass('hide');
                      $(".CLSEscenarioC").parents('tr').addClass('hide');
                      $(".CLSEscenarioD").parents('tr').addClass('hide');
  
                      Total = GetTotal('A');
  
                      $(html).text(nf.format(Total));
                      break;
                  case "2":
                      $(".CLSEscenarioB").parents('tr').removeClass('hide');
                      $(".CLSEscenarioA").parents('tr').addClass('hide');
                      $(".CLSEscenarioC").parents('tr').addClass('hide');
                      $(".CLSEscenarioD").parents('tr').addClass('hide');
                      Total = GetTotal('B');
  
                      $(html).text(nf.format(Total));
                      break;
                  case "3":
                      $(".CLSEscenarioC").parents('tr').removeClass('hide');
                      $(".CLSEscenarioB").parents('tr').addClass('hide');
                      $(".CLSEscenarioA").parents('tr').addClass('hide');
                      $(".CLSEscenarioD").parents('tr').addClass('hide');
                      Total = GetTotal('C');
  
                      $(html).text(nf.format(Total));
                      break;
                  case "4":
                      $(".CLSEscenarioD").parents('tr').removeClass('hide');
                      $(".CLSEscenarioB").parents('tr').addClass('hide');
                      $(".CLSEscenarioC").parents('tr').addClass('hide');
                      $(".CLSEscenarioA").parents('tr').addClass('hide');
  
                      Total = GetTotal('D');
                      $(html).text(nf.format(Total));
                      break;
                  case "0":
                      $(".CLSEscenarioD").parents('tr').removeClass('hide');
                      $(".CLSEscenarioB").parents('tr').removeClass('hide');
                      $(".CLSEscenarioC").parents('tr').removeClass('hide');
                      $(".CLSEscenarioA").parents('tr').removeClass('hide');
                      tableDet.draw();
                      break;
                  default:
  
              }
              */
        }



        function GetTotal(Escenario) {
            var countTotal = 0;

            var Array = tableDet.data();
            var arr = $.map(Array, function (value, key) {
                if (value.Escenario == Escenario)
                    return countTotal += value.Monto;
            });

            return countTotal;
        }

        function ResetModel() {
            idModalObraDescripcion.val('');
            idModalObraArea.val('');
            idModalObraEscenario.val(1);
            ModalObraPrioridad.val('');
        }



        function AgregarRegistroGuardar(table) {
            ListaElementos = $('.CmesRegistro');

            idRow = GetMaxId();
            idRow += 1;
            objRow = {};

            objRow.id = idRow,
            objRow.accion = getAccion(idRow),
            objRow.Escenario = idModalObraEscenario.val(),
            objRow.Area = idModalObraArea.val(),
            objRow.Codigo = ModalObraResponsables.val(),
            objRow.Abreviatura = $('#ModalObraResponsables option:selected').data("prefijo"),
            objRow.Descripcion = idModalObraDescripcion.val(),
            objRow.Probabilidad = ModalProbabilidad.val(),
            objRow.Margen = ModalMargen.val(),
            objRow.Monto = removeCommas(ModalMonto.val());
            objRow.MES1 = $(ListaElementos[0]).val(),
            objRow.MES2 = $(ListaElementos[1]).val(),
            objRow.MES3 = $(ListaElementos[2]).val(),
            objRow.MES4 = $(ListaElementos[3]).val(),
            objRow.MES5 = $(ListaElementos[4]).val(),
            objRow.MES6 = $(ListaElementos[5]).val(),
            objRow.MES7 = $(ListaElementos[6]).val(),
            objRow.MES8 = $(ListaElementos[7]).val(),
            objRow.MES9 = $(ListaElementos[8]).val(),
            objRow.MES10 = $(ListaElementos[9]).val(),
            objRow.MES11 = $(ListaElementos[10]).val(),
            objRow.MES12 = $(ListaElementos[11]).val(),
            objRow.Total = ModalSumaMeses(ListaElementos),
            objRow.Comentario = tbComentario.val();
            objRow.CentroCostos = cboCentroCostos.val() != "Seleccione" ? cboCentroCostos.val() : "NA";
            objRow.Prioridad = ModalObraPrioridad.val();


            var ArrayD = tableDet.data();
            ArrayD.push(objRow);
            GetInfoObras(ArrayD);

            modalNewRegistro.modal('hide');
            getAllData();


        }

        function getAccion(id) {

            return "<div> <button class='btn btn-warning editRow' data-idrow='" + id + "' type='button' style='margin: 2px;'> " +
                                 "<span class='glyphicon glyphicon-edit'></span></button>" +
                                 "<button class='btn btn-danger removeRow'data-idrow='" + id + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-remove'></span></button> " +
                                 "<button class='btn btn-primary vistaRowPor' data-idrow='" + id + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-eye-open'></span></button>" +
                "</div>";
        }

        function ModalSumaMeses(ListaElementos) {
            var total = 0;

            for (var i = 0; i < ListaElementos.length; i++) {
                total += Number($(ListaElementos[i]).val());
            }

            return total;
        }

        function SetInput(tipo, value) {

            switch (tipo) {
                case 1:
                    return "<div class='input-group'>" +
                                         "<input type='number' class='form-control tbProbabilidad' value='" + value + "' max='100'>" +
                                            "<span class='input-group-addon'>%</span>" +
                                        "</div>";
                    break;
                case 2:
                    return "<div class='input-group'>" +
                                     "<span class='input-group-addon'>$</span>" +
                                "<input type='Text' class='form-control tbMonto' value='" + value + "'>";
                case 3:
                    return "<label class='lblTotal' data-idRow='" + idRow + "'>" + value + "%</label>"
                default:

            }
        }

        function sumar(elemento, previous) {

            var rowValue = elemento.parents('tr').children().find('.mes');
            lblTotal = elemento.parents('tr').children().find('.lblTotal');
            var Total = 0;
            for (var i = 0; i < rowValue.length; i++) {
                Total += Number($(rowValue[i]).val());
            }
            if (Total <= 100) {
                lblTotal.text(Total + " %");
            }
            else {
                $(elemento).val(previous);
                sumar(elemento, previous)
            }

        }

        $(document).on('change', '.tbMonto', function () {
            var valor = redondear($(this).val());
            $(this).val(valor);
        });

        function redondear(valor) {
            var sinComas = removeCommas(valor);
            var redondeado = Math.round(sinComas);
            var conCommas = addCommas(redondeado.toFixed(2));
            return conCommas;
        }

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        function GetPeriodoMeses() {
            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];

            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;
                //   $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + periodo);
            }

            for (var i = 0 ; i < MesInicio; i++) {
                //  $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
            }
            return tituloMeses;
        }

        function getAllData() {

            var Array = [];

            var tbl = $('table#tblCapturaObras tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    return $(cell);
                });
            });
            $.each(tableDet.data(), function (index, value) {
                var JsonData = {};
                if (value.length != 0) {
                    JsonData.id = value.id;
                    JsonData.Escenario = value.Escenario;
                    JsonData.Area = value.Area;
                    JsonData.Codigo = value.Codigo;
                    JsonData.Descripcion = value.Descripcion;
                    JsonData.Probabilidad = value.Probabilidad;

                    JsonData.Margen = value.Margen;
                    JsonData.Monto = value.Monto;
                    JsonData.Fecha1 = value.MES1;
                    JsonData.Fecha2 = value.MES2;
                    JsonData.Fecha3 = value.MES3;
                    JsonData.Fecha4 = value.MES4;
                    JsonData.Fecha5 = value.MES5;
                    JsonData.Fecha6 = value.MES6;
                    JsonData.Fecha7 = value.MES7;
                    JsonData.Fecha8 = value.MES8;
                    JsonData.Fecha9 = value.MES9;
                    JsonData.Fecha10 = value.MES10;
                    JsonData.Fecha11 = value.MES11;
                    JsonData.Fecha12 = value.MES12;
                    JsonData.Total = value.Total;

                    JsonData.banderaFinanciamiento = value.banderaFinanciamiento;
                    JsonData.porcentaje = value.porcentaje;
                    JsonData.Comentario = value.Comentario;
                    JsonData.CentroCostos = value.CentroCostos;
                    JsonData.Prioridad = value.Prioridad;
                    Array.push(JsonData);
                }

            });

            GuardarData(Array);
        }

        function getValueHtml(cadena) {
            return $(cadena).children().children('input').val();
        }

        function GuardarData(Array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/GuardarInfoTabla',
                type: 'POST',
                dataType: 'json',
                data: { obj: Array, obj1: cboEscenario.val(), obj2: tbMesesInicio.val(), obj3: cboPeriodo.val(), id: idGlobalRegistroObras },
                success: function (response) {
                    modalNewRegistro.modal('hide');
                    idModalDescripcionAreas.val('');
                    $.unblockUI();
                    AlertaGeneral('Confirmación', 'Los registros fueron actualizados correctamente');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getAllDataDelete() {

            var Array = [];
            $.each(tableDet.data(), function (index, value) {
                var JsonData = {};
                if (value.length != 0) {
                    JsonData.id = value.id
                    JsonData.Escenario = value.Escenario;
                    JsonData.Area = value.Area;
                    JsonData.Codigo = value.Codigo;
                    JsonData.Descripcion = value.Descripcion;
                    JsonData.Probabilidad = value.Probabilidad;
                    JsonData.Margen = value.Margen;
                    JsonData.Monto = value.Monto;
                    JsonData.Fecha1 = value.MES1;
                    JsonData.Fecha2 = value.MES2;
                    JsonData.Fecha3 = value.MES3;
                    JsonData.Fecha4 = value.MES4;
                    JsonData.Fecha5 = value.MES5;
                    JsonData.Fecha6 = value.MES6;
                    JsonData.Fecha7 = value.MES7;
                    JsonData.Fecha8 = value.MES8;
                    JsonData.Fecha9 = value.MES9;
                    JsonData.Fecha10 = value.MES10;
                    JsonData.Fecha11 = value.MES11;
                    JsonData.Fecha12 = value.MES12;
                    JsonData.Total = value.Total;
                    JsonData.Comentario = value.Comentario;
                    JsonData.CentroCostos = value.CentroCostos;
                    JsonData.Prioridad = value.Prioridad;
                    Array.push(JsonData);
                }

            });


            GuardarDataDelete(Array);
        }

        function GuardarDataDelete(Array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/GuardarInfoTabla',
                type: 'POST',
                dataType: 'json',
                data: { obj: Array, obj1: cboEscenario.val(), obj2: tbMesesInicio.val(), obj3: cboPeriodo.val(), id: idGlobalRegistroObras },
                //data: { obj: Array, obj1: "1", obj2: "2", obj3: "2016", id: 0 },//cboPeriodo.val() },
                success: function (response) {
                    modalNewRegistro.modal('hide');
                    idModalDescripcionAreas.val('');
                    $.unblockUI();
                    AlertaGeneral('Confirmación', 'El Registro Fue eliminado correctamente');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetMaxId() {
            var idRow = $('.editRow[data-idrow]').get().map(function (value) {
                return Number($(value).attr('data-idrow'));
            });
            Array.prototype.max = function () {
                return Math.max.apply(null, this);
            };
            return idRow.max();
        }
        function clickTd() {

            //$("#tblCapturaObras").find("td.CLSCodigo").on("click", function () {
            //    var td = $(this);

            //    var codigo = Number(td.attr('data-codigo')) + 1;

            //    if (codigo < 4) {
            //        td.attr('data-codigo', codigo);
            //    }
            //    else {
            //        td.attr('data-codigo', 0);
            //    }
            //});
        }

        function CallModal(titulo, mensaje) {
            TituloConfirmacion.text(titulo);
            MensajeConfirmacion.text(mensaje);
            MensajeConfirmacionModal.modal('show');
        }

        function OpenNewReponsable() {

            modalAltaResponsable.modal('show');
        }

        function removeCommas(str) {

            while (str.search(",") >= 0) {
                str = (str + "").replace(',', '');
            }
            return str;
        };

        init();

    };

    $(document).ready(function () {

        administrativo.proyecciones.Obras = new Obras();
        $(".msgRow").hide();
        $(".finishRow").hide();
    });
})();

