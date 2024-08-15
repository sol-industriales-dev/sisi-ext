(() => {
    $.namespace('Administracion.Contabilidad.Propuesta.ResumenProveedor');
    ResumenProveedor = function () {
        const btnEnvioCorreos = $("#btnEnvioCorreos");
        const selPropCC = $('#selPropCC');
        const txtPropFac = $('#txtPropFac');
        const txtPropFOC = $('#txtPropFOC');
        const txtProvMin = $('#txtProvMin');
        const txtProvMax = $('#txtProvMax');
        const txtPropFSol = $('#txtPropFSol');
        const txtPropFPag = $('#txtPropFPag');
        const txtPropFMon = $('#txtPropFMon');
        const dpPropCorte = $('#dpPropCorte');
        const txtPropHSel = $('#txtPropHSel');
        const txtPropHSelDll = $('#txtPropHSelDll');
        const txtPropFReci = $('#txtPropFReci');
        const btnProvLimit = $('#btnProvLimit');
        const btnPropBuscar = $('#btnPropBuscar');
        const txtPropHTotal = $('#txtPropHTotal');
        const txtPropHTotalDll = $('#txtPropHTotalDll');
        const btnPropGuardar = $('#btnPropGuardar');
        const inputGroupBtn = $('.input-group-btn');
        const tblPropFacturas = $('#tblPropFacturas');
        const selPropTipoProceso = $('#selPropTipoProceso');
        const chkManual = $("#chkManual");
        let lstAutorizado = [], itemsGiro = [], dtPropFacturas, dlldia = 1;
        let init = () => {
            initForm();
            btnProvLimit.click(setDataLimit);
            btnPropGuardar.click(saveSplitted);
            inputGroupBtn.click(chngSetAllSelOpt);
            btnPropBuscar.click(setLstFacturasProv);
            inputGroupBtn.each((i, btn) => $(btn).click());
            btnEnvioCorreos.click(enviarCorreos);
            setItemsGiro();
        }
        const getDolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const FillComboGiro = new URL(window.location.origin + '/Administrativo/Reportes/FillComboGiro');
        const guardarGastosProv = new URL(window.location.origin + '/Administrativo/Propuesta/guardarGastosProv');
        const getLstFacturasProv = new URL(window.location.origin + '/Administrativo/Propuesta/getLstFacturasProv');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');

        function saveSplitted() {
            let obj = getLstTblGastos();
            //var obj = {};
            //var lst = [];
            //obj.valid= true;
            //var temp = [{"loop":31,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":5834,"numproPeru":null,"proveedor":"CARLOS CARRILLO MAYORGA","referenciaoc":"1588","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"07/10/2023","factura":"1339","saldo":23193.04,"monto_plan":23193.04,"concepto":"H-4898 DIABLITO DE DESPACHADOR DE TAMBOS","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"06/09/2023","fechaValidacion":"07/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5834,"numproPeru":null,"proveedor":"CARLOS CARRILLO MAYORGA","referenciaoc":"1687","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"09/10/2023","factura":"1343","saldo":2722.52,"monto_plan":2722.52,"concepto":"HORNO MICROONDAS&#XA;WINIA 0.7 PIES","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5890,"numproPeru":null,"proveedor":"ISOLTEC DEL DESIERTO S DE RL DE CV\r\n","referenciaoc":"9342","cc":"227","centroCostos":"227","tm":1,"tmDesc":"1  MATERIALES","vence":"13/10/2023","factura":"4807","saldo":445.77,"monto_plan":445.77,"concepto":"CABLE THW #12 NEGRO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"06/09/2023","fechaValidacion":"13/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5890,"numproPeru":null,"proveedor":"ISOLTEC DEL DESIERTO S DE RL DE CV\r\n","referenciaoc":"9386","cc":"227","centroCostos":"227","tm":1,"tmDesc":"1  MATERIALES","vence":"13/10/2023","factura":"4812","saldo":485.24,"monto_plan":485.24,"concepto":"5890 ISOLTEC DEL DESIERTO S DE RL DE CV\r","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"13/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5901,"numproPeru":null,"proveedor":"AUTOS DE CALIDAD DE ZACATECAS SA DE CV\r\n","referenciaoc":"83","cc":"LF5","centroCostos":"LF5","tm":1,"tmDesc":"1  MATERIALES","vence":"13/09/2023","factura":"66937","saldo":7590.01,"monto_plan":7590.01,"concepto":"PLACA PORTADORA - CAJA DE CONEXION","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"11/09/2023","fechaValidacion":"13/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"3192","cc":"213","centroCostos":"213","tm":1,"tmDesc":"1  MATERIALES","vence":"05/10/2023","factura":"4178","saldo":4701.9,"monto_plan":4701.9,"concepto":"5903 RO-CAMS S DE RL DE CV\r\n","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"31/08/2023","fechaValidacion":"05/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"3199","cc":"213","centroCostos":"213","tm":7,"tmDesc":"7  REFACCIONES","vence":"05/10/2023","factura":"4179","saldo":152.56,"monto_plan":152.56,"concepto":"FUSIBLE FUSIBLE ENCHUFE MINI 10 A. ROJO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"01/09/2023","fechaValidacion":"05/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"3194","cc":"213","centroCostos":"213","tm":1,"tmDesc":"1  MATERIALES","vence":"09/10/2023","factura":"4185","saldo":8069.41,"monto_plan":8069.41,"concepto":"5903 RO-CAMS S DE RL DE CV\r\n","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"05/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"65","cc":"HG2","centroCostos":"HG2","tm":7,"tmDesc":"7  REFACCIONES","vence":"08/10/2023","factura":"4186","saldo":3554.94,"monto_plan":3554.94,"concepto":"CINTA CINTA ANTIDERRAPANTE - 6&QUOT; X 6","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"05/09/2023","fechaValidacion":"08/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"3203","cc":"213","centroCostos":"213","tm":1,"tmDesc":"1  MATERIALES","vence":"09/10/2023","factura":"4193","saldo":4956.54,"monto_plan":4956.54,"concepto":"5903 RO-CAMS S DE RL DE CV\r\n","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"06/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":32,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":5903,"numproPeru":null,"proveedor":"RO-CAMS S DE RL DE CV\r\n","referenciaoc":"3211","cc":"213","centroCostos":"213","tm":1,"tmDesc":"1  MATERIALES","vence":"11/10/2023","factura":"4194","saldo":528,"monto_plan":528,"concepto":"5903 RO-CAMS S DE RL DE CV\r\n","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"11/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5904,"numproPeru":null,"proveedor":"ENRIQUE GALLEGOS ECHEVERRIA","referenciaoc":"45","cc":"NF5","centroCostos":"NF5","tm":1,"tmDesc":"1  MATERIALES","vence":"14/10/2023","factura":"6575","saldo":1044,"monto_plan":1044,"concepto":"5904 ENRIQUE GALLEGOS ECHEVERRIA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"25/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5904,"numproPeru":null,"proveedor":"ENRIQUE GALLEGOS ECHEVERRIA","referenciaoc":"58","cc":"ICC","centroCostos":"ICC","tm":1,"tmDesc":"1  MATERIALES","vence":"14/10/2023","factura":"6576","saldo":1044,"monto_plan":1044,"concepto":"5904 ENRIQUE GALLEGOS ECHEVERRIA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"25/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"419","cc":"A79","centroCostos":"A79","tm":6,"tmDesc":"6  SERVICIOS","vence":"26/10/2023","factura":"422","saldo":3076.85,"monto_plan":3076.85,"concepto":"1 MULTIPURPOSE HOSE RED","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"25/08/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"271","cc":"CZ3","centroCostos":"CZ3","tm":6,"tmDesc":"6  SERVICIOS","vence":"30/09/2023","factura":"426","saldo":6296.33,"monto_plan":6296.33,"concepto":"HYD HOSE, 3/4 EXTENDED","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"30/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"755","cc":"A36","centroCostos":"A36","tm":6,"tmDesc":"6  SERVICIOS","vence":"30/09/2023","factura":"427","saldo":4881.95,"monto_plan":4881.95,"concepto":"R12 FLEXTRAL 1-1/4&QUOT; MYLA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"30/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"754","cc":"A36","centroCostos":"A36","tm":6,"tmDesc":"6  SERVICIOS","vence":"30/09/2023","factura":"428","saldo":9327.43,"monto_plan":9327.43,"concepto":"HYD HOSE, 1-1/2 EXTENDED","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"30/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"477","cc":"E09","centroCostos":"E09","tm":6,"tmDesc":"6  SERVICIOS","vence":"30/09/2023","factura":"429","saldo":5143.17,"monto_plan":5143.17,"concepto":"HYD HOSE, 1/4 ID, 6000","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"30/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"751","cc":"A36","centroCostos":"A36","tm":6,"tmDesc":"6  SERVICIOS","vence":"08/10/2023","factura":"432","saldo":5238.47,"monto_plan":5238.47,"concepto":"R12 FLEXTRAL 1/2&QUOT; MYLAR","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"08/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":5934,"numproPeru":null,"proveedor":"ESPECIALISTAS, PALAS Y PERFORADORAS, SA DE CV\r\n","referenciaoc":"378","cc":"A80","centroCostos":"A80","tm":6,"tmDesc":"6  SERVICIOS","vence":"26/10/2023","factura":"433","saldo":13264.86,"monto_plan":13264.86,"concepto":"R12 FLEXTRAL 3/8&QUOT; MYLAR","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":33,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":5985,"numproPeru":null,"proveedor":"MVG CONSULTING AND CONSTRUCTION SAS DE CV\r\n","referenciaoc":"3691","cc":"205","centroCostos":"205","tm":1,"tmDesc":"1  MATERIALES","vence":"07/09/2023","factura":"212","saldo":1115.51,"monto_plan":1115.51,"concepto":"BOMBA FUMIGADORA DE 5 LTS.","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"07/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6053,"numproPeru":null,"proveedor":"MAINOMAC S DE RL DE CV","referenciaoc":"382","cc":"QJ1","centroCostos":"QJ1","tm":1,"tmDesc":"1  MATERIALES","vence":"23/09/2023","factura":"1996","saldo":22777.76,"monto_plan":22777.76,"concepto":"REPARACIÓN UNIDAD DE ROTACIÓN JUMBO LINE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"06/09/2023","fechaValidacion":"08/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6053,"numproPeru":null,"proveedor":"MAINOMAC S DE RL DE CV","referenciaoc":"165","cc":"Y07","centroCostos":"Y07","tm":1,"tmDesc":"1  MATERIALES","vence":"29/09/2023","factura":"2008","saldo":45977.76,"monto_plan":45977.76,"concepto":"BOMBA DE AGUA 3217925550","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"14/09/2023","fechaValidacion":"14/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6084,"numproPeru":null,"proveedor":"HRC SUMINISTROS SA DE CV\r\n","referenciaoc":"27","cc":"OUJ","centroCostos":"OUJ","tm":7,"tmDesc":"7  REFACCIONES","vence":"10/10/2023","factura":"1028","saldo":4122.64,"monto_plan":4122.64,"concepto":"BATERIA LTH","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"20/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6084,"numproPeru":null,"proveedor":"HRC SUMINISTROS SA DE CV\r\n","referenciaoc":"99","cc":"IA4","centroCostos":"IA4","tm":7,"tmDesc":"7  REFACCIONES","vence":"13/10/2023","factura":"1042","saldo":638,"monto_plan":638,"concepto":"FILTRO AIRE HILUX","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"21/09/2023","fechaValidacion":"23/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6088,"numproPeru":null,"proveedor":"JESUS DAVID JIMENEZ RODRIGUEZ","referenciaoc":"98","cc":"LEP","centroCostos":"LEP","tm":6,"tmDesc":"6  SERVICIOS","vence":"19/09/2023","factura":"857","saldo":34160,"monto_plan":34160,"concepto":"TORNEAR Y RECONSTRUIR MASAS, PONER BALER","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"19/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6089,"numproPeru":null,"proveedor":"ROBERTO EVERARDO BADILLA DELGADO","referenciaoc":"105","cc":"IA9","centroCostos":"IA9","tm":6,"tmDesc":"6  SERVICIOS","vence":"21/08/2023","factura":"181","saldo":10800,"monto_plan":10800,"concepto":"MANO DE OBRA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"14/08/2023","fechaValidacion":"21/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6089,"numproPeru":null,"proveedor":"ROBERTO EVERARDO BADILLA DELGADO","referenciaoc":"152","cc":"LT8","centroCostos":"LT8","tm":6,"tmDesc":"6  SERVICIOS","vence":"27/09/2023","factura":"211","saldo":93744,"monto_plan":93744,"concepto":"REPARACION COMPLETA DE TRANSMISION DE FO","moneda":"MN","autorizado":"E","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"20/09/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"E"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"159","cc":"LEZ","centroCostos":"LEZ","tm":7,"tmDesc":"7  REFACCIONES","vence":"26/09/2023","factura":"779","saldo":1432.6,"monto_plan":1432.6,"concepto":"DUPLICADO DE LLAVE DE VEHICULO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"29/08/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"107","cc":"IBQ","centroCostos":"IBQ","tm":7,"tmDesc":"7  REFACCIONES","vence":"08/09/2023","factura":"790","saldo":5794.2,"monto_plan":5794.2,"concepto":"BOMBA HIDRAULICA HILUX 16-20 2.7L/2.8 L","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"04/09/2023","fechaValidacion":"08/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":34,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"220","cc":"CA5","centroCostos":"CA5","tm":1,"tmDesc":"1  MATERIALES","vence":"22/09/2023","factura":"792","saldo":9798.87,"monto_plan":9798.87,"concepto":"MARCHA DELCO 40MT 24V 12DT LESTER 3948","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"04/09/2023","fechaValidacion":"22/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"2120","cc":"262","centroCostos":"262","tm":1,"tmDesc":"1  MATERIALES","vence":"22/09/2023","factura":"796","saldo":12133.19,"monto_plan":12133.19,"concepto":"DESENGRASANTE EN SPRAY HDX","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"13/09/2023","fechaValidacion":"22/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"4282","cc":"206","centroCostos":"206","tm":7,"tmDesc":"7  REFACCIONES","vence":"25/09/2023","factura":"797","saldo":5387.61,"monto_plan":5387.61,"concepto":"ACEITE 5W30","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"13/09/2023","fechaValidacion":"25/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"186","cc":"LTB","centroCostos":"LTB","tm":7,"tmDesc":"7  REFACCIONES","vence":"27/09/2023","factura":"802","saldo":2088,"monto_plan":2088,"concepto":"POLEA DE FIERRO T.LIG 4.0 X 2RB BP","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"14/09/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"36","cc":"ICL","centroCostos":"ICL","tm":7,"tmDesc":"7  REFACCIONES","vence":"27/09/2023","factura":"803","saldo":3045,"monto_plan":3045,"concepto":"3401B0718 BALERO CARDAN","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"412","cc":"LOI","centroCostos":"LOI","tm":6,"tmDesc":"6  SERVICIOS","vence":"19/09/2023","factura":"804","saldo":12093,"monto_plan":12093,"concepto":"TAPIZADO DE ASIENTO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"19/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"513","cc":"LTI","centroCostos":"LTI","tm":6,"tmDesc":"6  SERVICIOS","vence":"19/09/2023","factura":"805","saldo":12093,"monto_plan":12093,"concepto":"TAPIZADO DE ASIENTO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"19/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6117,"numproPeru":null,"proveedor":"KARLA DENISSE ESTRADA MARTINEZ","referenciaoc":"81","cc":"ICD","centroCostos":"ICD","tm":7,"tmDesc":"7  REFACCIONES","vence":"26/09/2023","factura":"815","saldo":22910,"monto_plan":22910,"concepto":"3401B041 FLECHA CARDAN 4X4","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"21/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"89","cc":"MM1","centroCostos":"MM1","tm":7,"tmDesc":"7  REFACCIONES","vence":"11/10/2023","factura":"607","saldo":3066.77,"monto_plan":3066.77,"concepto":"4500608500830000GN G-49 BATERIA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"05/09/2023","fechaValidacion":"11/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"5","cc":"295","centroCostos":"295","tm":6,"tmDesc":"6  SERVICIOS","vence":"23/09/2023","factura":"300742","saldo":397.3,"monto_plan":397.3,"concepto":"0140000000160000GT 16PC1FS FERULA PC 1 A","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"04/08/2023","fechaValidacion":"24/08/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":35,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"7","cc":"295","centroCostos":"295","tm":6,"tmDesc":"6  SERVICIOS","vence":"23/09/2023","factura":"301892","saldo":32521.66,"monto_plan":32521.66,"concepto":"0111110200160160GT 16G-16MP CONEX PERM 1","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/08/2023","fechaValidacion":"24/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"13","cc":"295","centroCostos":"295","tm":6,"tmDesc":"6  SERVICIOS","vence":"15/10/2023","factura":"306732","saldo":2350.28,"monto_plan":2350.28,"concepto":"0140000000160000MA M03400-16 FERRUL P/MA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"23/08/2023","fechaValidacion":"15/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"18","cc":"295","centroCostos":"295","tm":6,"tmDesc":"6  SERVICIOS","vence":"15/10/2023","factura":"309888","saldo":361.34,"monto_plan":361.34,"concepto":"0430101480112120SF AM8 SFF075 CONEXIÃ“N ","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"01/09/2023","fechaValidacion":"15/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"2","cc":"OFV","centroCostos":"OFV","tm":6,"tmDesc":"6  SERVICIOS","vence":"15/10/2023","factura":"310863","saldo":352.06,"monto_plan":352.06,"concepto":"0140000000160000GT 16PC1FS FERULA PC 1 A","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"05/09/2023","fechaValidacion":"15/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6143,"numproPeru":null,"proveedor":"MANGUERAS Y CONEXIONES DE MEXICALI SA DE CV\r\n","referenciaoc":"22","cc":"295","centroCostos":"295","tm":6,"tmDesc":"6  SERVICIOS","vence":"15/10/2023","factura":"311921","saldo":359.6,"monto_plan":359.6,"concepto":"0140000000160000MA M03400-16 FERRUL P/MA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"15/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6150,"numproPeru":null,"proveedor":"CARLOS ALBERTO FIERRO NEVAREZ","referenciaoc":"47","cc":"LTK","centroCostos":"LTK","tm":1,"tmDesc":"1  MATERIALES","vence":"11/10/2023","factura":"132","saldo":44718,"monto_plan":44718,"concepto":"SERVICIO REPARACION DE CAJA SECA FOR F45","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"15/09/2023","fechaValidacion":"21/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6172,"numproPeru":null,"proveedor":"MARIA DEL ROSARIO RAMIREZ CASTRO","referenciaoc":"49","cc":"288","centroCostos":"288","tm":6,"tmDesc":"6  SERVICIOS","vence":"29/09/2023","factura":"479","saldo":11600,"monto_plan":11600,"concepto":"SERVICIO DE UNIDAD PILOTO DE HERMOSILLO ","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"02/10/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6172,"numproPeru":null,"proveedor":"MARIA DEL ROSARIO RAMIREZ CASTRO","referenciaoc":"50","cc":"288","centroCostos":"288","tm":6,"tmDesc":"6  SERVICIOS","vence":"29/09/2023","factura":"480","saldo":11600,"monto_plan":11600,"concepto":"SERVICIO DE UNIDAD PILOTO DE MINADO LA H","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"02/10/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6192,"numproPeru":null,"proveedor":"SUBMIN, S DE RL  DE CV ","referenciaoc":"1689","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"07/09/2023","factura":"2119","saldo":40600,"monto_plan":40600,"concepto":"TUBO DE BOLIS (70821228)","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"07/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6192,"numproPeru":null,"proveedor":"SUBMIN, S DE RL  DE CV ","referenciaoc":"1699","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"09/09/2023","factura":"2121","saldo":40600,"monto_plan":40600,"concepto":"TUBO DE BOLIS (70821228)","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"09/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":36,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6192,"numproPeru":null,"proveedor":"SUBMIN, S DE RL  DE CV ","referenciaoc":"1744","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"19/09/2023","factura":"2127","saldo":40600,"monto_plan":40600,"concepto":"TUBO DE BOLIS (70821228)","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"19/09/2023","fechaValidacion":"19/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6206,"numproPeru":null,"proveedor":"ALVA MARICELA ZAPATA ZAVALA","referenciaoc":"41","cc":"IC1","centroCostos":"IC1","tm":1,"tmDesc":"1  MATERIALES","vence":"06/10/2023","factura":"100007","saldo":10358.8,"monto_plan":10358.8,"concepto":"MTTO. EQUIPO DE TRANSPO SERVICIO DE AFIN","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"19/09/2023","fechaValidacion":"21/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6235,"numproPeru":null,"proveedor":"MESTECH SA DE CV\r\n","referenciaoc":"2636","cc":"236","centroCostos":"236","tm":1,"tmDesc":"1  MATERIALES","vence":"25/10/2023","factura":"230","saldo":52200,"monto_plan":52200,"concepto":"REACONDICIONAR CROMO 6.997&QUOT; CON PRO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"22/09/2023","fechaValidacion":"25/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6235,"numproPeru":null,"proveedor":"MESTECH SA DE CV\r\n","referenciaoc":"2637","cc":"236","centroCostos":"236","tm":1,"tmDesc":"1  MATERIALES","vence":"25/10/2023","factura":"231","saldo":93612,"monto_plan":93612,"concepto":"REACONDICIONAR DIAMETRO DAÃ‘ADO 8.997&QU","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"22/09/2023","fechaValidacion":"25/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6235,"numproPeru":null,"proveedor":"MESTECH SA DE CV\r\n","referenciaoc":"2638","cc":"236","centroCostos":"236","tm":1,"tmDesc":"1  MATERIALES","vence":"25/10/2023","factura":"232","saldo":93612,"monto_plan":93612,"concepto":"REACONDICIONAR DIAMETRO DAÃ‘ADO 8.997&QU","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"22/09/2023","fechaValidacion":"25/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6292,"numproPeru":null,"proveedor":"LLANTAS CHIHUAHUA DE JUANITO S DE RL DE CV\r\n","referenciaoc":"42","cc":"ICE","centroCostos":"ICE","tm":1,"tmDesc":"1  MATERIALES","vence":"22/09/2023","factura":"1409","saldo":15312,"monto_plan":15312,"concepto":"265/75R16 ECOVISION VI-186MT MUD","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"20/09/2023","fechaValidacion":"22/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6313,"numproPeru":null,"proveedor":"OPERADORA DE PRODUCTOS MENFIS SA DE CV\r\n","referenciaoc":"3611","cc":"205","centroCostos":"205","tm":7,"tmDesc":"7  REFACCIONES","vence":"28/09/2023","factura":"11165","saldo":243.6,"monto_plan":243.6,"concepto":"FLEXOMETRO AC INOX 8MX30MM","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"03/08/2023","fechaValidacion":"28/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6313,"numproPeru":null,"proveedor":"OPERADORA DE PRODUCTOS MENFIS SA DE CV\r\n","referenciaoc":"565","cc":"284","centroCostos":"284","tm":1,"tmDesc":"1  MATERIALES","vence":"28/09/2023","factura":"11329","saldo":1470.75,"monto_plan":1470.75,"concepto":"6313 OPERADORA DE PRODUCTOS MENFIS SA DE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"16/08/2023","fechaValidacion":"28/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6313,"numproPeru":null,"proveedor":"OPERADORA DE PRODUCTOS MENFIS SA DE CV\r\n","referenciaoc":"547","cc":"284","centroCostos":"284","tm":7,"tmDesc":"7  REFACCIONES","vence":"28/09/2023","factura":"11473","saldo":316.1,"monto_plan":316.1,"concepto":"WURTH LIMPIADOR DE CONTACTOS 300ML","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"28/08/2023","fechaValidacion":"28/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6313,"numproPeru":null,"proveedor":"OPERADORA DE PRODUCTOS MENFIS SA DE CV\r\n","referenciaoc":"590","cc":"284","centroCostos":"284","tm":7,"tmDesc":"7  REFACCIONES","vence":"28/09/2023","factura":"11765","saldo":362.79,"monto_plan":362.79,"concepto":"DONALDSON FILTRO P550779","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"22/09/2023","fechaValidacion":"28/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":37,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6362,"numproPeru":null,"proveedor":"ALVARO GERARDO ADAME AMADOR","referenciaoc":"90","cc":"LEN","centroCostos":"LEN","tm":6,"tmDesc":"6  SERVICIOS","vence":"28/09/2023","factura":"1351","saldo":15660,"monto_plan":15660,"concepto":"LITRO DE ACEITE 80W 90","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"19/09/2023","fechaValidacion":"28/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6373,"numproPeru":null,"proveedor":"OSMAN DOMINGUEZ GOMEZ","referenciaoc":"84","cc":"IB9","centroCostos":"IB9","tm":1,"tmDesc":"1  MATERIALES","vence":"27/09/2023","factura":"19","saldo":11020,"monto_plan":11020,"concepto":"HECHURA Y PROGRAMACIÃ“N DE LLAVE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"26/08/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6373,"numproPeru":null,"proveedor":"OSMAN DOMINGUEZ GOMEZ","referenciaoc":"51","cc":"IC5","centroCostos":"IC5","tm":1,"tmDesc":"1  MATERIALES","vence":"27/09/2023","factura":"20","saldo":53360,"monto_plan":53360,"concepto":"TRANSMISIÃ“N L-200 (2022)","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"29/08/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6378,"numproPeru":null,"proveedor":"SOCOFHI SA DE CV\r\n","referenciaoc":"704","cc":"A28","centroCostos":"A28","tm":7,"tmDesc":"7  REFACCIONES","vence":"29/09/2023","factura":"34703","saldo":3136.19,"monto_plan":3136.19,"concepto":"MANGUERA GH781-16 X 2.40 MTS Y CONEX.","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"27/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6378,"numproPeru":null,"proveedor":"SOCOFHI SA DE CV\r\n","referenciaoc":"46","cc":"LF6","centroCostos":"LF6","tm":7,"tmDesc":"7  REFACCIONES","vence":"29/09/2023","factura":"34704","saldo":313.07,"monto_plan":313.07,"concepto":"CONEX MUESTR. 2501-6-6, 2405-8-8, 6500-8","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"27/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6378,"numproPeru":null,"proveedor":"SOCOFHI SA DE CV\r\n","referenciaoc":"591","cc":"284","centroCostos":"284","tm":7,"tmDesc":"7  REFACCIONES","vence":"29/09/2023","factura":"34705","saldo":2369.04,"monto_plan":2369.04,"concepto":"CONEXIONES SEGUN MUESTRA, MANOMETRO","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"27/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6378,"numproPeru":null,"proveedor":"SOCOFHI SA DE CV\r\n","referenciaoc":"596","cc":"284","centroCostos":"284","tm":7,"tmDesc":"7  REFACCIONES","vence":"29/09/2023","factura":"34706","saldo":526.74,"monto_plan":526.74,"concepto":"CONEXION 1A8DK4","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"27/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6378,"numproPeru":null,"proveedor":"SOCOFHI SA DE CV\r\n","referenciaoc":"601","cc":"284","centroCostos":"284","tm":7,"tmDesc":"7  REFACCIONES","vence":"29/09/2023","factura":"34707","saldo":6363.99,"monto_plan":6363.99,"concepto":"PISTOLA P/ DIESEL DFN100-B","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"27/09/2023","fechaValidacion":"29/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6391,"numproPeru":null,"proveedor":"NAYELI ADRIANA TORRES CHAVEZ","referenciaoc":"507","cc":"279","centroCostos":"279","tm":1,"tmDesc":"1  MATERIALES","vence":"13/10/2023","factura":"4","saldo":3076.26,"monto_plan":3076.26,"concepto":"BOLSAS ZIPLIC GRANDES","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"20/09/2023","fechaValidacion":"23/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6417,"numproPeru":null,"proveedor":"3F LOGISTICA S DE RL DE CV\r\n","referenciaoc":"834","cc":"A19","centroCostos":"A19","tm":6,"tmDesc":"6  SERVICIOS","vence":"03/10/2023","factura":"269","saldo":51040,"monto_plan":51040,"concepto":"RENTA DE MAQUINARIA Y&#XA;&#XA;EQUIPO&#X","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"18/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":38,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6417,"numproPeru":null,"proveedor":"3F LOGISTICA S DE RL DE CV\r\n","referenciaoc":"921","cc":"AIT","centroCostos":"AIT","tm":6,"tmDesc":"6  SERVICIOS","vence":"29/09/2023","factura":"272","saldo":69600,"monto_plan":69600,"concepto":"RENTA DE MAQUINARIA Y&#XA;&#XA;EQUIPO&#X","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"12/09/2023","fechaValidacion":"14/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6417,"numproPeru":null,"proveedor":"3F LOGISTICA S DE RL DE CV\r\n","referenciaoc":"1102","cc":"AIA","centroCostos":"AIA","tm":6,"tmDesc":"6  SERVICIOS","vence":"29/09/2023","factura":"275","saldo":9860,"monto_plan":9860,"concepto":"RENTA DE MAQUINARIA Y&#XA;&#XA;EQUIPO&#X","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"12/09/2023","fechaValidacion":"14/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6436,"numproPeru":null,"proveedor":"JULIO CESAR ROMERO ESQUIVEL","referenciaoc":"61","cc":"S02","centroCostos":"S02","tm":1,"tmDesc":"1  MATERIALES","vence":"11/09/2023","factura":"8","saldo":13773.84,"monto_plan":13773.84,"concepto":"MANTENIMIENTO DE MAQUINARIA Y EQUIPO REP","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"06/09/2023","fechaValidacion":"11/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6496,"numproPeru":null,"proveedor":"MARIO ROBLES GARCIA","referenciaoc":"81","cc":"OUG","centroCostos":"OUG","tm":6,"tmDesc":"6  SERVICIOS","vence":"12/10/2023","factura":"919","saldo":66560.8,"monto_plan":66560.8,"concepto":"MTTO. EQUIPO DE TRANSPORTE VÃ�STAGO DE C","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"01/09/2023","fechaValidacion":"27/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6507,"numproPeru":null,"proveedor":"GUSTAVO ALONSO MENCHACA ELICERIO","referenciaoc":"1724","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"19/09/2023","factura":"179","saldo":3016,"monto_plan":3016,"concepto":"TRAPOS 4 BOLSAS DE TRAPO DE 25KG ORDEN D","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"19/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6508,"numproPeru":null,"proveedor":"RICARDO SAUL MATANCILLAS ESPARZA","referenciaoc":"368","cc":"QI1","centroCostos":"QI1","tm":1,"tmDesc":"1  MATERIALES","vence":"09/09/2023","factura":"32","saldo":6670,"monto_plan":6670,"concepto":"CARTRIDGE LH410","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6508,"numproPeru":null,"proveedor":"RICARDO SAUL MATANCILLAS ESPARZA","referenciaoc":"209","cc":"QJ3","centroCostos":"QJ3","tm":1,"tmDesc":"1  MATERIALES","vence":"09/09/2023","factura":"34","saldo":22376.25,"monto_plan":22376.25,"concepto":"6508 RICARDO SAUL MATANCILLAS ESPARZA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"09/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6508,"numproPeru":null,"proveedor":"RICARDO SAUL MATANCILLAS ESPARZA","referenciaoc":"208","cc":"QJ3","centroCostos":"QJ3","tm":1,"tmDesc":"1  MATERIALES","vence":"08/09/2023","factura":"35","saldo":14343.75,"monto_plan":14343.75,"concepto":"INSUMOS DEL BUJE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"08/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6521,"numproPeru":null,"proveedor":"MARCO ANTONIO GARCIA QUIHUIS","referenciaoc":"1143","cc":"AI7","centroCostos":"AI7","tm":6,"tmDesc":"6  SERVICIOS","vence":"26/09/2023","factura":"262","saldo":52838,"monto_plan":52838,"concepto":"SERVICIO DE GRUA 15TON &#13;&#10;PARA RE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"05/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6537,"numproPeru":null,"proveedor":"ESTRATEGIA EMPRESARIAL AADRA Y ASOCIADOS SA DE CV","referenciaoc":"1596","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"21/09/2023","factura":"781","saldo":2557.8,"monto_plan":2557.8,"concepto":"PANTALONES HOMBRE TALLA 36 ESTÃ¡NDAR CON","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"14/09/2023","fechaValidacion":"21/09/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":39,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6537,"numproPeru":null,"proveedor":"ESTRATEGIA EMPRESARIAL AADRA Y ASOCIADOS SA DE CV","referenciaoc":"1594","cc":"269","centroCostos":"269","tm":1,"tmDesc":"1  MATERIALES","vence":"21/09/2023","factura":"782","saldo":4604.04,"monto_plan":4604.04,"concepto":"CAMISOLA TALLA M HOMBRE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"14/09/2023","fechaValidacion":"21/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6538,"numproPeru":null,"proveedor":"JOSE ALFREDO DE LEON MENDEZ","referenciaoc":"67","cc":"IB8","centroCostos":"IB8","tm":1,"tmDesc":"1  MATERIALES","vence":"11/09/2023","factura":"60","saldo":80782.4,"monto_plan":80782.4,"concepto":"REPARACION DE CLUTCH, FRENOS,INSPECCION ","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"04/09/2023","fechaValidacion":"11/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6538,"numproPeru":null,"proveedor":"JOSE ALFREDO DE LEON MENDEZ","referenciaoc":"59","cc":"LF8","centroCostos":"LF8","tm":1,"tmDesc":"1  MATERIALES","vence":"15/09/2023","factura":"62","saldo":580,"monto_plan":580,"concepto":"SWITCH INTERRUPTOR DE FRENOS FORD F450 2","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"15/09/2023","fechaValidacion":"15/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"139","cc":"LTC","centroCostos":"LTC","tm":6,"tmDesc":"6  SERVICIOS","vence":"27/07/2023","factura":"111","saldo":11853.12,"monto_plan":11853.12,"concepto":"MANTENIMIENTO EQUIPO DE TRANSPORTE SONDE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"26/07/2023","fechaValidacion":"27/07/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"84","cc":"LEO","centroCostos":"LEO","tm":6,"tmDesc":"6  SERVICIOS","vence":"27/07/2023","factura":"112","saldo":9525.47,"monto_plan":9525.47,"concepto":"MANTENIMIENTO EQUIPO DE TRANSPORTE, RUTE","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"26/07/2023","fechaValidacion":"27/07/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"86","cc":"LEO","centroCostos":"LEO","tm":6,"tmDesc":"6  SERVICIOS","vence":"31/08/2023","factura":"162","saldo":9676.89,"monto_plan":9676.89,"concepto":"MANO DE OBRA REMPLAZO DE BALATAS TRASERA","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"31/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"9534","cc":"227","centroCostos":"227","tm":1,"tmDesc":"1  MATERIALES","vence":"26/09/2023","factura":"175","saldo":431.27,"monto_plan":431.27,"concepto":"6549 FERNANDO ALCALA TORRES","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"18/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"187","cc":"LTB","centroCostos":"LTB","tm":1,"tmDesc":"1  MATERIALES","vence":"26/09/2023","factura":"176","saldo":1346.12,"monto_plan":1346.12,"concepto":"6549 FERNANDO ALCALA TORRES","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"19/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6549,"numproPeru":null,"proveedor":"FERNANDO ALCALA TORRES","referenciaoc":"116","cc":"S01","centroCostos":"S01","tm":6,"tmDesc":"6  SERVICIOS","vence":"20/09/2023","factura":"177","saldo":87882.36,"monto_plan":87882.36,"concepto":"SERVICIO REPARACION DE TRANSMICION PARA ","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"19/09/2023","fechaValidacion":"20/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6675,"numproPeru":null,"proveedor":"JORGE VALDEZ CHAIDEZ","referenciaoc":"935","cc":"A10","centroCostos":"A10","tm":6,"tmDesc":"6  SERVICIOS","vence":"30/09/2023","factura":"1","saldo":11948,"monto_plan":11948,"concepto":"O.C. A10-000935 MTTO. MAQUINARIA Y EQUIP","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"29/08/2023","fechaValidacion":"31/08/2023","tp":null,"estatus":"A"}],"manual":false}},{"loop":40,"success":false,"dataRequest":{"lst":[{"id":0,"numpro":6706,"numproPeru":null,"proveedor":"GS GRUPO INDUSTRIAL SANTOS SA DE CV","referenciaoc":"599","cc":"LT4","centroCostos":"LT4","tm":6,"tmDesc":"6  SERVICIOS","vence":"26/10/2023","factura":"25","saldo":2842,"monto_plan":2842,"concepto":"SERVICIO DE SOLDADURA EN REPARACION DE P","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"08/09/2023","fechaValidacion":"26/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6713,"numproPeru":null,"proveedor":"JUAN CARLOS TOVAR HERREJON","referenciaoc":"15","cc":"L21","centroCostos":"L21","tm":6,"tmDesc":"6  SERVICIOS","vence":"14/10/2023","factura":"1564","saldo":3000.01,"monto_plan":3000.01,"concepto":"AFINACION","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"14/09/2023","tp":null,"estatus":"A"},{"id":0,"numpro":6713,"numproPeru":null,"proveedor":"JUAN CARLOS TOVAR HERREJON","referenciaoc":"14","cc":"L21","centroCostos":"L21","tm":6,"tmDesc":"6  SERVICIOS","vence":"14/10/2023","factura":"1565","saldo":11350,"monto_plan":11350,"concepto":"AMORTIGUADOR DELANTERO MARCA SYD","moneda":"MN","autorizado":"A","tipocambio":1,"idGiro":13,"bloqueado":false,"descripcionBloqueo":null,"activo_fijo":false,"fechaTimbrado":"07/09/2023","fechaValidacion":"14/09/2023","tp":null,"estatus":"A"}],"manual":false}}];
            //$.each(temp,function(i,e){
            //    $.each(e.dataRequest.lst,function(i2,e2){
            //        lst.push(e2);
            //    });

            //});
            //obj.data = lst;
            if (obj.valid) {
                if (obj.data.length > 0) {
                    var scheme = { lst: new Array(), manual: chkManual.is(":checked") };
                    $.sm_SplittedSave(guardarGastosProv, obj.data, scheme, 10, setLstFacturasProv);
                }
                else {
                    AlertaGeneral("Alerta", "¡Debe seleccionar almenos un registro para continuar!");
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Faltaron datos por indicar los cuales fueron marcados de color rojo!");
            }
        }
        function enviarCorreos() {
            $.ajax({
                url: '/Administrativo/Reportes/enviarCorreosGerardoPropuesta',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral('Confirmación', 'Se Enviaron los correos');
                    } else {
                        AlertaGeneral('Alerta', 'No hay factura existente');
                    }
                },
                error: function (response) {
                    // AlertaGeneral("Alerta", response.message);
                }
            });
        }

        async function guardarLstGastos() {
            try {
                let lst = getLstTblGastos();

                response = await ejectFetchJson(guardarGastosProv, lst);
                if (response.success) {
                    setLstFacturasProv();
                    AlertaGeneral("Aviso", "Datos guardados correctamente.");

                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function setLstFacturasProv() {
            try {
                //setDolarDelDia();
                dtPropFacturas.clear().draw();
                txtPropHSel.text(maskNumero(0));
                txtPropHTotal.text(maskNumero(0));
                txtPropHSelDll.text(maskNumero(0));
                txtPropHTotalDll.text(maskNumero(0));
                let busq = getForm()
                    , response = await ejectFetchJson(getLstFacturasProv, busq);
                if (response.success) {
                    $.each(response.lst, function(key, value) {
                        if (value.numproPeru != null && value.numproPeru != "") value.numpro = value.numproPeru;
                    });
                    dtPropFacturas.rows.add(response.lst).draw();
                    txtPropHTotal.text(maskNumero(response.total));
                    txtPropHTotalDll.text(maskNumero(response.totaldll));
                    setSaldosTotales();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function setDolarDelDia() {
            try {
                response = await ejectFetchJson(getDolarDelDia, { fecha: dpPropCorte.val() });
                if (response.success) {
                    dlldia = response.dll;
                }
            } catch (o_O) { dlldia = 1; }
        }
        async function getLimitProv() {
            try {
                response = await ejectFetchJson(getLimitNoProveedores);
                if (response.success) {
                    txtProvMin.val(response.limit[0]);
                    txtProvMin.data().prov = response.limit[0];
                    txtProvMax.val(response.limit[1]);
                    txtProvMax.data().prov = response.limit[1];
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function setDataLimit() {
            txtProvMin.val(txtProvMin.data().prov);
            txtProvMax.val(txtProvMax.data().prov);
        }
        async function setItemsGiro() {
            response = await ejectFetchJson(FillComboGiro);
            if (response.success) {
                itemsGiro = response.items;
            }
        }
        function getLstTblGastos() {
            var obj = {};
            var lst = [];
            var valid = true;
            dtPropFacturas.rows().every(function (i) {
                let node = $(this.node())
                    , autorizado = " "
                    , idGiro = 0
                    , cbo = node.find('td:eq(11) select')
                    , esBloq = cbo.prop('disabled');
                cbo.prop('disabled', false);
                autorizado = cbo.val();
                autorizado = autorizado === null ? " " : autorizado;
                cbo.prop('disabled', esBloq);
                //idGiro = +(node.find('td:eq(9) select').val());
                if (autorizado != ' ') {
                    idGiro = 13;
                }
                else {
                    idGiro = '';
                }
                if (autorizado == 'B') {
                    let data = this.data();
                    data.estatus = autorizado;
                    if (!data.bloqueado) lst.push(data);
                    node.find('td:eq(11) select').removeClass("alert-danger");
                    //node.find('td:eq(9) select').removeClass("alert-danger");
                }
                else if (autorizado != ' ' && (idGiro != undefined && idGiro != '')) {
                    let data = this.data();
                    data.estatus = autorizado;
                    data.idGiro = idGiro;
                    if (!data.bloqueado) lst.push(data);
                    node.find('td:eq(11) select').removeClass("alert-danger");
                    //node.find('td:eq(9) select').removeClass("alert-danger");
                }
                else if (autorizado != ' ' && (idGiro == undefined || idGiro == '')) {
                    node.find('td:eq(11) select').removeClass("alert-danger");
                    //node.find('td:eq(9) select').addClass("alert-danger");
                    valid = false;
                }
                else if (autorizado == ' ' && (idGiro != undefined && idGiro != '')) {
                    node.find('td:eq(11) select').addClass("alert-danger");
                    //node.find('td:eq(9) select').removeClass("alert-danger");
                    valid = false;
                }
            });
            obj.data = lst;
            obj.valid = valid;
            return obj;
        }
        function getForm() {
            return {
                fechaCorte: dpPropCorte.val()
                , tipoProceso: selPropTipoProceso.val()
                , min: txtProvMin.val()
                , max: txtProvMax.val()
                , lstCc: selPropCC.val()
                , tipo: chkManual.is(":checked")
            };
        }
        function setFooter({ cc, referenciaoc, saldo, moneda }) {
            txtPropFOC.val(`${cc} - ${referenciaoc}`);
            txtPropFSol.val(maskNumero(saldo));
            txtPropFac.val(maskNumero(saldo));
            txtPropFReci.val(maskNumero(saldo));
            txtPropFPag.val(maskNumero(0));
            txtPropFMon.val(moneda);
        }
        function setSaldosTotales() {
            let sumaSaldo = 0;
            let sumaSaldoDll = 0;
            dtPropFacturas.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data()
                    , autorizado = " "
                    , idGiro = 0
                    , cbo = node.find('td:eq(11) select')
                    , esBloq = cbo.prop('disabled');
                cbo.prop('disabled', false);
                autorizado = cbo.val();
                autorizado = autorizado === null ? " " : autorizado;
                cbo.prop('disabled', esBloq);
                //idGiro += +(node.find('td:eq(9) select').val())
                if ((autorizado === "A" || autorizado === "E")/* && idGiro > 0*/) {
                    let tc = data.tipocambio === null ? dlldia : data.tipocambio;
                    if (data.moneda == 'MN') {
                        sumaSaldo += (unmaskNumero(node.find('.inputAPagar').val()));
                    }
                    else {
                        sumaSaldoDll += (unmaskNumero(node.find('.inputAPagar').val()) * tc);
                    }
                }
            });
            txtPropHSel.text(maskNumero(sumaSaldo));
            txtPropHSelDll.text(maskNumero(sumaSaldoDll));
        }
        function setSaldoProv({ numpro }) {
            let lstProv = dtPropFacturas.data().filter(prov => numpro === prov.numpro).toArray()
                , sumGroup = lstProv.filter((prov, a, b) => {
                    return (prov.autorizado === "A" || prov.autorizado === "E");
                }).reduce((sumGroup, current) => sumGroup + (current.monto_plan), 0);
            tblPropFacturas.find(`tr.prov${numpro} td:eq(4)`).text(maskNumero(sumGroup));
        }
        function initDataTblPropFacturas() {
            dtPropFacturas = tblPropFacturas.DataTable({
                paging: false
                , destroy: true
                , ordering: false
                , language: dtDicEsp
                , "sScrollX": "100%"
                , "sScrollXInner": "100%"
                , "bScrollCollapse": true
                , scrollY: '65vh'
                , scrollCollapse: true
                , "bLengthChange": false
                , "searching": false
                , "bFilter": true
                , "bInfo": true
                , "bAutoWidth": false
                , "createdRow": function (row, data, dataIndex) {
                    if (data.bloqueado) {
                        $(row).css('background-color', '#e8403d');
                    }
                }
                , drawCallback: function (settings) {
                    var api = this.api(),
                        rows = api.rows({ page: 'current' }).nodes(),
                        head = null;
                    api.column({ page: 'current' }).data().each((group, i, dtable) => {
                        const dataBefore = dtable.data()[i - 1];
                        const data = dtable.data()[i];
                        if (data.activo_fijo) {
                            $(rows).eq(i).addClass('activo_fijo');
                        }
                        if (data.numproPeru != null && data.numproPeru != "") {
                            $(rows).eq(i).addClass('peru');
                        }
                        if (i > 0) {
                            if (dataBefore.numpro !== data.numpro) {
                                let lstProv = dtable.data().filter(prov => dataBefore.numpro === prov.numpro);
                                let suma = lstProv.reduce((suma, current) => suma + current.saldo, 0);
                                let sumProv = lstProv.filter(prov => {
                                    return (prov.autorizado === "A" || prov.autorizado === "E");
                                }).reduce((suma, current) => suma + current.saldo, 0);
                                $(rows).eq(i).before(`<tr class="prov${dataBefore.numpro}">
                                                            <td colspan="9" class="fondoTotal">Cuenta Bancaria:<span style="float:right">Saldo del proveedor</span></td>
                                                            <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal fondoAPagar">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal">${maskNumero(sumProv)}</td>
                                                            <td></td>
                                                        </tr>`);
                            }
                            if (i == dtable.length - 1) {
                                let lstProv = dtable.data().filter(prov => data.numpro === prov.numpro);
                                let suma = lstProv.reduce((suma, current) => suma + current.saldo, 0);
                                let sumProv = lstProv.filter(prov => {
                                    return (prov.autorizado === "A" || prov.autorizado === "E");
                                }).reduce((suma, current) => suma + current.saldo, 0);
                                $(rows).eq(i).after(`<tr class="prov${data.numpro} fondoTotal">
                                                            <td colspan="9">Cuenta Bancaria:<span style="float:right">Saldo del proveedor</span></td>
                                                            <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal fondoAPagar">${maskNumero(suma)}</td>
                                                            <td class="text-right fondoTotal">${maskNumero(sumProv)}</td>
                                                            <td></td>
                                                        </tr>`);
                            }
                        }
                    });
                }
                , columns: [
                    { data: 'proveedor', createdCell: (td, data, rowData) => $(td).html(`${rowData.numpro} - ${data}`) },
                    { data: 'factura', width: "50px" },
                    { data: 'vence' },
                    { data: 'fechaTimbrado' },
                    { data: 'fechaValidacion' },
                    { data: 'tmDesc' },
                    { data: 'centroCostos', createdCell: (td, data, rowData) => $(td).html(`${rowData.cc} - ${data}`) },
                    { data: 'referenciaoc' },
                    { data: 'concepto' },
                    { data: 'saldo', class: 'text-right', createdCell: (td, data, rowData) => $(td).html(`${maskNumero(data)} ${rowData.moneda}`) },
                    { data: 'monto_plan', class: 'text-right' },
                    {
                        data: 'autorizado', width: "150px", createdCell: (td, data, rowData) => {
                            if (!rowData.bloqueado) {
                                let cbo = $(`<div class='input-group'>
                                            <select class='form-control auth'></select>
                                            <span class='input-group-btn'>
                                                <button type='button' class='btn btn-default'> B</button>
                                            </span>
                                        </div>`);
                                cbo.find('select').addClass("form-control");
                                cbo.find('select').fillComboItems(lstAutorizado, true, data);
                                cbo.find('select').prop("disabled", data === "B");
                                $(td).html(cbo);
                            } else {
                                let text = rowData.descripcionBloqueo;
                                $(td).html(text);
                            }

                        }
                    },
                    //{ 
                    //    data: 'idGiro' ,createdCell: (td, data) => { 
                    //        let cbo = $(`<select>`);
                    //        cbo.addClass("form-control giro");
                    //        cbo.fillComboItems(itemsGiro, null, data);
                    //        $(td).html(cbo);
                    //    }
                    //},
                ],
                columnDefs: [
                    {
                        targets: [10],
                        render: function (data, type, row) {
                            let input = '<input type="text" class="form-control inputAPagar" value="' + maskNumero(data) + '"' + (row.bloqueado ? 'disabled' : '') + ' />';
                            return input;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblPropFacturas.on('change', '.giro', function (event) {
                        setSaldosTotales();
                    });
                    tblPropFacturas.on('change', '.auth', function (event) {
                        let data = dtPropFacturas.row($(this).closest('tr')).data()
                            , auth = this.value;
                        if (auth === 'B') {
                            $(this).prop('disabled', true);
                        }
                        data.autorizado = auth;
                        setSaldoProv(data);
                        setSaldosTotales();
                        if (auth != 'B') {
                            $(this).parent().parent().parent().find(".giro").val("");
                        }

                    });
                    tblPropFacturas.on('click', '.btn', function (event) {
                        let select = $(this).closest(`div`).find("select")
                            , esDisabled = select.prop('disabled');
                        select.prop('disabled', !esDisabled);
                        select.val(" ");
                        setSaldoProv(dtPropFacturas.row($(this).closest('tr')).data());
                        setSaldosTotales();
                    });
                    tblPropFacturas.find('tbody').on('click', 'tr', function () {
                        let data = dtPropFacturas.row(this).data();
                        if (data !== undefined) {
                            setFooter(data);
                        }
                    });

                    tblPropFacturas.find('tbody').on('change', '.inputAPagar', function (event) {
                        const datos = tblPropFacturas.DataTable().row($(this).closest('tr')).data();
                        datos.monto_plan = unmaskNumero($(this).val());

                        const valorAnterior = unmaskNumero(event.currentTarget.defaultValue);
                        event.currentTarget.defaultValue = unmaskNumero($(this).val());

                        $(this).val(maskNumero(unmaskNumero($(this).val())));

                        const tdTotalAPagar = unmaskNumero(tblPropFacturas.find('.prov' + datos.numpro).find('td.fondoAPagar').text());
                        tblPropFacturas.find('.prov' + datos.numpro).find('td.fondoAPagar').html(maskNumero(tdTotalAPagar - valorAnterior + datos.monto_plan));

                        if (datos.autorizado == 'A' || datos.autorizado == 'E') {
                            const tdTotalAPagar = unmaskNumero(tblPropFacturas.find('.prov' + datos.numpro).find('td').eq(3).text());
                            tblPropFacturas.find('.prov' + datos.numpro).find('td').eq(3).html(maskNumero(tdTotalAPagar - valorAnterior + datos.monto_plan));
                        }

                        setSaldosTotales();

                        if (datos.moneda == 'MN') {
                            txtPropHTotal.text(maskNumero(unmaskNumero(txtPropHTotal.text()) - valorAnterior + datos.monto_plan));
                        } else {
                            txtPropHTotalDll.text(maskNumero(unmaskNumero(txtPropHTotalDll.val()) - valorAnterior + datos.monto_plan));
                        }
                    });
                }
            });
        }
        function chngSetAllSelOpt() {
            let estodo = !this.value,
                select = $(this).next().find("select[multiple]");
            this.value = estodo;
            limpiarMultiselect(select);
            if (estodo) {
                let lstValor = $(`#${select.attr("id")}`).find(`option`).toArray().map(option => option.value);
                select.val(lstValor);
                convertToMultiselect(select);
            }
        }
        function initForm() {
            getLimitProv();
            lstAutorizado.push({ Text: "Pendiente", Value: " " });
            lstAutorizado.push({ Text: "A. Cheque", Value: "A" });
            lstAutorizado.push({ Text: "Eletrónico", Value: "E" });
            lstAutorizado.push({ Text: "Bloqueado", Value: "B" });
            dpPropCorte.datepicker();
            if (_gpEmpresa != 2) {
                selPropCC.fillCombo('/Administrativo/Poliza/getCC', null, true, null);
                convertToMultiselect(selPropCC);
            }
            else {
                convertToMultiselect(selPropCC);
                $(".selPropCC").hide();
            }

            selPropTipoProceso.fillCombo('/Administrativo/Propuesta/cboTipoOperacion', null, true, null);

            inputGroupBtn.val(false);
            initDataTblPropFacturas();
            setFooter({ cc: "", referenciaoc: "", saldo: 0, moneda: "" });
            txtPropHTotal.text(maskNumero(0));
            txtPropHSel.text(maskNumero(0));
            txtPropHTotalDll.text(maskNumero(0));
            txtPropHSelDll.text(maskNumero(0));
        }
        init();
    }
    $(document).ready(() => {
        Administracion.Contabilidad.Propuesta.ResumenProveedor = new ResumenProveedor();
    });
})();