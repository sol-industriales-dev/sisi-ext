using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes
{
    public enum ReportesEnum
    {

        [DescriptionAttribute("pruebas")]
        Prueba = 9999, //ok

        [DescriptionAttribute("Reporte de Gastos Maquinaria")]
        Gastos_Maquinaria = 1, //ok
        [DescriptionAttribute("Reporte de Comparativa de Tipos")]
        Comparativa_Tipos = 2,//ok
        [DescriptionAttribute("Reporte de Captura Diaria Horometros")]
        captura_diaria = 3,//ok
        [DescriptionAttribute("Minuta de reunión")]
        minuta_reunion = 4, //ok
        [DescriptionAttribute("Lista asistencia")]
        lista_asistencia = 5, //ok
        [DescriptionAttribute("Carga Mensual de Combustible")]
        Carga_Combustible = 6, //ok
        [DescriptionAttribute("Rendimiento de Combustible")]
        redimiento_combustible = 7, //ok
        [DescriptionAttribute("Reporte de Captura de Horometros")]
        captura_horometros = 8, //ok
        [DescriptionAttribute("Reporte de Captura de Combustible")]
        captura_combustibles = 9, //ok
        [DescriptionAttribute("Solicitud de Equipo")]
        Solicitud_Equipo = 10, //ok
        [DescriptionAttribute("Formato de Cambio")]
        Formato_Cambio = 11, //ok
        [DescriptionAttribute("Reporte Solicitud Equipo Asignado")]
        Solicitud_Equipo_Asignado = 12,
        [DescriptionAttribute("Control de envio y recepcion")]
        CONTROLENVIORECEPCION = 13, //ok
        [DescriptionAttribute("RHRepAltas")]
        RHREPALTAS = 14, //ok
        [DescriptionAttribute("RHRepBajas")]
        RHREPBAJAS = 15, //ok
        [DescriptionAttribute("RHRepCambios")]
        RHREPCAMBIOS = 16, //ok
        [DescriptionAttribute("RHRepModificaciones")]
        RHREPMODIFICACIONES = 17, //ok
        [DescriptionAttribute("RHRepIncidencias")]
        RHREPINCIDENCIAS = 18, //ok
        [DescriptionAttribute("RHRepDetIncidencias")]
        RHREPDETINCIDENCIAS = 19, //ok
        [DescriptionAttribute("RHRepActivos")]
        RHREPACTIVOS = 20, //ok
        [DescriptionAttribute("MRepFichaTecnica")]
        MRepFichaTecnica = 21,
        [DescriptionAttribute("MControlCalidad")]
        MControlCalidad = 22, //ok
        [DescriptionAttribute("MConsumoDiesel")]
        MConsumoDiesel = 23, //ok
        [DescriptionAttribute("CadenaProductiva")]
        CadenaProductiva = 24,
        [DescriptionAttribute("FichaTecnica")]
        FICHATECNICA = 25,//ok
        [DescriptionAttribute("Estado Resultados")]
        PROYECCIONES1 = 26,//ok
        [DescriptionAttribute("FLUJO DE EFECTIVO")]
        PROYECCIONES2 = 27,//ok
        [DescriptionAttribute("ESTADO DE POSICION FINANCIERA")]
        PROYECCIONES3 = 28,// ok
        [DescriptionAttribute("Conciliacion semanal de equipo en stand by")]
        ConciliacionSemanalStandBy = 29, //ok
        [DescriptionAttribute("Solicitud de Equipo")]
        Solicitud_EquipoNoModal = 30, //ok
        [DescriptionAttribute("Cifras Principales")]
        CifrasPrincipales = 31, //ok
        [DescriptionAttribute("Notas de Credito")]
        NOTASCREDITO = 32, //ok
        [DescriptionAttribute("Reporte provisional de rentea de maquinaría")]
        ReporteProvicional = 33, //ok
        [DescriptionAttribute("Movimientos abiertos de almacen")]
        MovimientoAbierto = 35, //ok
        [DescriptionAttribute("Solicitud Reemplazo de Equipo")]
        SOLICITUDEQUIPOREEMPLAZO = 34,
        [DescriptionAttribute("Movimientos cerrados de almacen")]
        MovimientoCerado = 36, //ok
        [DescriptionAttribute("Eficiencia por semana de obra")]
        EficienciaObra = 37, //ok
        [DescriptionAttribute("Eficiecia por semana de todas las obras")]
        EficienciaGeneral = 38, //ok
        [DescriptionAttribute("Indicador de contra recibos")]
        IndicadorContrarecibo = 39, //ok
        [DescriptionAttribute("Indicador de proceso")]
        IndicadorProceso = 40,//ok
        [DescriptionAttribute("Prefactura")]
        Prefactura = 41,//ok
        [DescriptionAttribute("ASIGNACIONDEVEHICULOS")]
        ASIGNACIONDEVEHICULOS = 42,//ok
        [DescriptionAttribute("INVETARIO GENERAL DE MAQUINARIA.")]
        INVENTARIOGENERALMAQUINARIA = 43,//ok
        [DescriptionAttribute("CHECKLIST RESGUARDOS.")]
        CHECKLISTRESGUARDOS = 44,//ok
        [DescriptionAttribute("ORDEN DE TRABAJO")]
        OT = 45,//ok
        [DescriptionAttribute("Resumen semanal de cadena productiva")]
        RESUMENSEMANAL = 46,//ok
        [DescriptionAttribute("Vista Reporte Stand by")]
        PreviewStandby = 47, //ok
        [DescriptionAttribute("Baja de maquinaria")]
        BajaMaquinaria = 48, //ok
        [DescriptionAttribute("Consumo de aceites y lubricantes")]
        ConsumoAceitesLubricantes = 49, //ok
        [DescriptionAttribute("Reporte de Consumo de aceites y lubricantes")]
        RepConsumoLubricente = 50, //ok
        [DescriptionAttribute("CONTROL DE MOVIMIENTO INTERNO MAQUINARIA")]
        CONTROLINTERNOMAQUINARIA = 51, //ok
        [DescriptionAttribute("Baja de activos fijos de maquinaria")]
        BajaActivosFijos = 52, //ok
        [DescriptionAttribute("Plantilla de Maquinaria")]
        PlantillaSolicitudes = 53, //ok
        [DescriptionAttribute("Reporte de tiempos Asignacion")]
        TiemposAsignacion = 54, //ok
        [DescriptionAttribute("Reporte de Notas credito abonadas y pendientes")]
        NOTASCREDITOPendientesRechazadas = 55, //ok
        [DescriptionAttribute("Reporte de Notas credito abonadas y pendientes")]
        pendientessustitucion = 56, //ok
        [DescriptionAttribute("Cotización Captura")]
        COTIZACIONCAPTURA = 57, //ok
        [DescriptionAttribute("KPI Equipo")]
        KPIEQUIPO = 58, //ok
        [DescriptionAttribute("KPI Metricas")]
        KPIMETRICAS = 59, //ok
        [DescriptionAttribute("KPI Graficas")]
        KPIGRAFICAS = 60, //ok
        [DescriptionAttribute("KPI Mesnaul")]
        KPIMENSUAL = 61, //ok
        [DescriptionAttribute("Reporte de Aditivas y Deductivas De Personal")]
        FormatoAditivaDeductiva = 62, //ok
        [DescriptionAttribute("Reporte de captura de lubricantes por detalle")]
        ConsumoAceitesLubricantesDetalle = 63, //ok
        [DescriptionAttribute("Reportes de póliza")]
        Poliza = 64,
        [DescriptionAttribute("Reportes de Movimientos de póliza")]
        MovPoliza = 65,
        [DescriptionAttribute("Reporte General de Resguardos")]
        ReporteGralResguardo = 66, //ok
        [DescriptionAttribute("Reportes Detalle Horas Hombre")]
        DetalleHorasHombre = 67,
        [DescriptionAttribute("Reportes Detalle Frecuencia Paros")]
        DetalleFrecuenciaParos = 68,
        [DescriptionAttribute("Reporte de impresion Grafica.")]
        impresion = 69,
        [DescriptionAttribute("Reporte de detalle por Puesto")]
        PuestoDetalle = 70,
        [DescriptionAttribute("Reporte de detalle por Puesto por personal")]
        PersonalDetallePuesto = 71,
        [DescriptionAttribute("Reporte General de Horas Hombre")]
        GeneralHorasHombre = 72,
        [DescriptionAttribute("Reporte Mantenimientos en frecuencia de paros")]
        ReporteMantenimientosFrecuenciaparo = 73,
        [DescriptionAttribute("Reporte Encuesta de evaluacion contunia Proveedor")]
        EncuestaEvaluacionContinuaProveedor = 74,
        [DescriptionAttribute("Reporte Evaluacion de EvaluacionProveedoresServicio")]
        EvaluacionProveedoresServicio = 75,
        [DescriptionAttribute("Reporte Evaluacion de subContratista")]
        EvaluacionaSubContratistas = 76,
        [DescriptionAttribute("Reporte de Finiquito RH")]
        ReporteFiniquitoRH = 77,
        [DescriptionAttribute("Facultamiento")]
        Facultamiento = 78,
        [DescriptionAttribute("Concentrado de horas hombre por empleado")]
        CONCENTRADOHHEMPLEADO = 79,
        [DescriptionAttribute("Concentrado de horas hombre por empleado")]
        CONCENTRADOHHPUESTOS = 80,
        [DescriptionAttribute("HORAS HOMBRE UTILIZACION")]
        UTLIZACION = 81,
        [DescriptionAttribute("Concentrado de horas hombre por PUESTO DETALLE")]
        CONCENTRADOHHPDETALLE = 82,
        [DescriptionAttribute("Concentrado de horas hombre por PUESTO DETALLE")]
        RPTHORASHOMBREEQUIPO = 83,
        [DescriptionAttribute("Datos de Insumos Arrendadora Construplan")]
        RPTINSUMOSCONSULTA = 84,
        [DescriptionAttribute("Reporte De Servicio")]
        RPTDESERVICIO = 85,
        [DescriptionAttribute("Cargo de Nómina por Área Cuenta")]
        ReporteCargoNominaCCArrendadora = 86,
        [DescriptionAttribute("Conciliacion Horometros")]
        CONCILIACIONHOROMETROS = 87,
        [DescriptionAttribute("Mazda Plan Maestro")]
        Mazda_PlanMaestro = 88,
        [DescriptionAttribute("Mazda Plan Mensual")]
        Mazda_PlanMensual = 89,
        [DescriptionAttribute("Mazda Revisión AC")]
        Mazda_RevisionAC = 90,
        [DescriptionAttribute("Mazda Revisión Cuadrilla")]
        Mazda_RevisionCua = 91,
        [DescriptionAttribute("Reporte de análisis de utilización")]
        RepAnalisisUtilizacion = 92,
        [DescriptionAttribute("Reporte Diario MAZDA")]
        RepDiarioMAZDA = 93,
        [DescriptionAttribute("Caratula de precios")]
        RPTCARATULAPRECIOS = 94,
        [DescriptionAttribute("Mazda Plan Mensual General")]
        Mazda_PlanMensualGeneral = 95,
        [DescriptionAttribute("Planeacion Semanal")]
        pm_planeacionSemanal = 96, //ok
        [DescriptionAttribute("Planeacion Ejecutado")]
        pm_Ejecutado = 97, //ok
        [DescriptionAttribute("Planificacion semanal de mantenimiento preventivo a ejecutar")]
        semanal_preventivoEjecutar = 98,
        [DescriptionAttribute("Reporte de Facultamientos")]
        Reporte_Facultamientos = 99,
        [DescriptionAttribute("Reporte de Facultamientos por Empleado General")]
        Reporte_Facultamientos_Empleado_General = 100,
        [DescriptionAttribute("Reporte de Facultamientos por Empleado")]
        Reporte_Facultamiento_Empleado = 101,
        [DescriptionAttribute("Reporte de Remoción de Componente")]
        REMOCIONCOMPONENTE = 150,
        [DescriptionAttribute("Bitácora de componentes removidos")]
        REMOCIONCOMPONENTEGRUPO = 151,
        [DescriptionAttribute("Reporte de falla")]
        REPORTEFALLA = 152,
        [DescriptionAttribute("ControlObra Avance Fisico")]
        ControlObra_AvanceFisico = 102,
        [DescriptionAttribute("Reporte Histórico Almacen")]
        HISORICO_ALMACEN = 153,
        [DescriptionAttribute("Reporte Cargo Nomina Mensual CC")]
        CargoNominaMensualCC = 103,
        [DescriptionAttribute("Reporte de usuarios encuestas.")]
        rptGestionUsuariosEncuestas = 107,
        [DescriptionAttribute("Reporte de encuestas Usuarios Permisos")]
        rptEncuestaUsuarioPermisos = 106,
        [DescriptionAttribute("Plantilla Personal")]
        PlantillaPersonal = 104,
        [DescriptionAttribute("Caratula de precios Modal")]
        RPTCARATULAPRECIOSMODAL = 105,
        [DescriptionAttribute("Plantilla Personal EK")]
        PlantillaPersonalEK = 108,
        [DescriptionAttribute("Entradas de almacen")]
        entradaAlmacen = 109,
        [DescriptionAttribute("Salidas de almacen")]
        salidaAlmacen = 110,
        [DescriptionAttribute("Resguardo Almacen")]
        resguardoAlmacen = 111,
        [DescriptionAttribute("Requisición")]
        requisicionEK = 112,
        [DescriptionAttribute("Orden de Compra")]
        ordenCompraEK = 113,
        [DescriptionAttribute("Borrador")]
        borrador = 114,
        [DescriptionAttribute("programacionPagos")]
        programacionPagos = 115,
        [DescriptionAttribute("Salida Traspaso Almacen")]
        salidaTraspasoAlmacen = 116,
        [DescriptionAttribute("Cuadro Comparativo")]
        cuadroComparativo = 117,

        [DescriptionAttribute("Reporte Entrada Compra")]
        reporteEntradaCompra = 118,
        [DescriptionAttribute("Reporte Entrada No Inventariable")]
        reporteEntradaNoInventariable = 119,
        [DescriptionAttribute("Reporte Entrada Devolucion")]
        reporteEntradaDevolucion = 120,
        [DescriptionAttribute("Reporte Entrada Fisico")]
        reporteEntradaFisico = 121,
        [DescriptionAttribute("Reporte Salida Devolucion")]
        reporteSalidaDevolucion = 122,
        [DescriptionAttribute("Reporte Salida Fisico")]
        reporteSalidaFisico = 123,
        [DescriptionAttribute("Reporte Salida Consumo")]
        reporteSalidaConsumoSinOrigen = 124,
        [DescriptionAttribute("Reporte Seguimiento Requisiciones")]
        reporteSeguimientoRequisiciones = 125,

        [DescriptionAttribute("Reporte Dashboard Evaluación Desempeño")]
        reporteDashboardEvaluacionDesempeno = 126,
        [DescriptionAttribute("Reporte Dashboard Evaluación Desempeño")]
        reporteDashboardEvaluacionDinamico = 127,

        [DescriptionAttribute("Salidas Consulta Traspaso")]
        salidaConsultaTraspaso = 128,
        [DescriptionAttribute("Entrada Consulta Traspaso")]
        entradaConsultaTraspaso = 129,
        [DescriptionAttribute("Surtido Requisición")]
        reporteSurtidoRequisicion = 130,

        [DescriptionAttribute("Diagrama de Gantt")]
        DIAGRAMA_GANTT = 154,
        [DescriptionAttribute("Reporte Ejecutivo")]
        REPORTE_EJECUTIVO = 155,
        [DescriptionAttribute("Listado Maestro")]
        REPORTE_LISTADO_MAESTRO = 156,
        [DescriptionAttribute("Reporte Inventario Componentes")]
        REPORTE_INVENTARIO_COMP = 157,
        [DescriptionAttribute("Reporte Vida Útil")]
        REPORTE_VIDA_UTIL = 158,
        [DescriptionAttribute("Reporte Disponibilidad Overhaul")]
        REPORTE_DISPONIBILIDAD_OVERHAUL = 159,
        [DescriptionAttribute("Reporte Calendario Ejecutado Overhaul")]
        REPORTE_CALENDARIO_EJECUTADO = 160,
        [DescriptionAttribute("Reporte Precision Overhaul")]
        REPORTE_PRECISION_OVERHAUL = 161,
        [DescriptionAttribute("Resumen de conjuntos y subconjuntos")]
        CONJUNTOS_OVERHAUL = 162,
        [DescriptionAttribute("Reporte Vida Útil")]
        VIDA_UTIL_OVERHAUL = 163,
        [DescriptionAttribute("Reporte Desecho Overhaul")]
        DESECHO_OVERHAUL = 164,
        [DescriptionAttribute("Reporte de captura diaria barrenación")]
        Captura_Diaria_Barrenacion = 165,
        [DescriptionAttribute("Reporte de rendimiento por pieza barrenación")]
        Rendimiento_Pieza_Barrenacion = 166,
        [DescriptionAttribute("Reporte de captura diaria general barrenación")]
        Captura_Diaria_General_Barrenacion = 167,
        [DescriptionAttribute("Reporte de control de asistencia seguridad")]
        Control_Asistencia = 168,
        [DescriptionAttribute("Reporte de plantilla de bonos por cc")]
        rh_bono_plantilla_cc = 169,
        [DescriptionAttribute("Reportes Propuesta")]
        REPORTES_PROPUESTA = 200,
        [DescriptionAttribute("Reporte de formato de autorización para un curso de capacitación")]
        Formato_Autorizacion_Capacitacion = 170,
        [DescriptionAttribute("Licencia de habilidades - Seguridad")]
        Licencia_Habilidades = 171,
        [DescriptionAttribute("Reparaciones Pendientes por Surtir")]
        Reparaciones_Pendientes = 172,
        [DescriptionAttribute("Tiempos de reparación Ovheraul")]
        Tiempos_Reparacion = 173,
        [DescriptionAttribute("Informe Preliminar Incidente")]
        InformePreliminar = 174,
        [DescriptionAttribute("Reporte de Investigación de Accidentes (RIA)")]
        FormatoRIA = 175,
        [DescriptionAttribute("PRENOMINA")]
        PRENOMINA = 176,
        [DescriptionAttribute("Renta maquinaria - Tiempo requerido vs tiempo utilizado")]
        RN_MaquinariaTiempoVs = 199,
        [DescriptionAttribute("Reporte Incidentes Global Seguridad")]
        ReporteIncidentesGlobalSeguridad = 177,
        [DescriptionAttribute("Reporte Component List")]
        ReporteComponentList = 178,
        [DescriptionAttribute("Reporte Inventario Componentes")]
        ReporteInventarioComponentes = 179,
        [DescriptionAttribute("Reporte Tiempos CRC Administrativo")]
        ReporteTiemposCRCAdmin = 180,
        [DescriptionAttribute("Reporte ActivoFijo")]
        ReporteResumenCedulaActivoFijo = 181,
        [DescriptionAttribute("Reporte de Flujo Efectivo Directo")]
        ReporteFlujoEfectivoDirecto = 182,
        [DescriptionAttribute("Reporte de Flujo Efectivo Operativo")]
        ReporteFlujoEfectivoOperativo = 183,
        [DescriptionAttribute("Reporte ActivoFijo ResumenTabulador")]
        ReporteResumenTabuladorActivoFijo = 184,
        [DescriptionAttribute("Reporte Programa de inversión anual")]
        ReporteProgramaInversion = 185,
        [DescriptionAttribute("Reporte Programa de Cambio de Componentes")]
        ReporteProgramaCambioComponente = 186,
        [DescriptionAttribute("Reporte Chequeras")]
        CapturaCheque = 187,
        [DescriptionAttribute("Reporte Poliza pruebas")]
        PolizaPruebas = 188,
        [DescriptionAttribute("Reporte de Amortizacion")]
        Amortizacion = 189,
        [DescriptionAttribute("Ejecutivo")]
        EjecutivoBarrenacion = 190,
        [DescriptionAttribute("Reporte de formato de autorización general de capacitación")]
        Formato_Autorizacion_General_Capacitacion = 191,
        [DescriptionAttribute("Reporte de componentes en reparación")]
        ReporteComReparacion = 192,
        [DescriptionAttribute("Reportes adjuntos de control de maquinaria")]
        CONTROLDEEQUIPOS = 193,
        [DescriptionAttribute("Informe Preliminar Incidente")]
        InformePreliminarFormatoRIA = 194,
        [DescriptionAttribute("Propuesta de Pago")]
        PropuestaPago = 195,
        [DescriptionAttribute("Reporte de Poliza.")]
        ImpresionPoliza = 196,
        [DescriptionAttribute("Reporte de Poliza.")]
        adeudoFinanciero = 197,
        [DescriptionAttribute("Reporte de Poliza.")]
        detalleAdeudoFinanciero = 198,
        [DescriptionAttribute("Orden de Compra Consulta")]       
        ordenCompraConsultaEK = 201,
        [DescriptionAttribute("Reporte de Poliza entre empresas")]
        PolizasEmpresa = 202, 
        [DescriptionAttribute("Reporte Barrenacio Stanby")]
        rptEquiposStandby = 203,
        [DescriptionAttribute("Reporte Barrenacion Turnos")]
        rptBarrenacionGeneralTurnos = 204,
        [DescriptionAttribute("Reporte Barrenacion Operadores")]
        rptBarrenacionOperadores = 205,
        [DescriptionAttribute("Rerporte de Concentrado KPI")]
        rptConcentradoKPI = 206,
        [DescriptionAttribute("Reporte Lista Autorización")]
        rptListaAutorizacion = 207,
        [DescriptionAttribute("Reporte Lista Autorización Correo")]
        rptListaAutorizacionCorreo = 208,
        [DescriptionAttribute("Reporte Autorizacion de KPI")]
        rptAutorizacionKPI = 209,
        [DescriptionAttribute("Reporte de Avance General")]
        rptAvanceGeneral = 210,
        [DescriptionAttribute("Reporte de Necesidades Primarias")]
        rptNecesidadesPrimarias = 211,
        [DescriptionAttribute("Reporte de Recorridos")]
        rptRecorrido = 212,
        [DescriptionAttribute("Reporte de Colaborador Capacitación")]
        rptColaboradorCapacitacion = 213,
        [DescriptionAttribute("Reporte de Horas Hombre Capacitacion")]
        rptHorasHombreCapacitacion = 214,
        [DescriptionAttribute("Reporte almacen componentes")]
        rptAlmacenComponentes = 215,
        [DescriptionAttribute("Reporte Evaluacion Bonos")]
        rptBN_Evaluacion = 216,
        [DescriptionAttribute("Reporte Bitacora Resguardos")]
        rptBitacoraResguardos = 217,
        [DescriptionAttribute("Reporte Toma de Inventario Físico")]
        rptInventarioFisico = 218,
        [DescriptionAttribute("Reporte Obtener Existencia Inventario")]
        rptObtenerExistenciasInventario = 219,
        [DescriptionAttribute("Valuación de Inventario Físico")]
        rptValuacionInventarioFisico = 220,
        [DescriptionAttribute("Reporte Prenomina Totales")]
        rptPrenominaTotales = 221,
        [DescriptionAttribute("Reporte caratula")]
        rptCaratula = 222,
        [DescriptionAttribute("Reporte Solicitud Cheque Nomina")]
        rptSolicitudChequeNomina = 223,
        [DescriptionAttribute("Reporte Orden BackLog")]
        rptOrdenBackLog = 224,
        [DescriptionAttribute("Reporte Caratula CC")]
        rptCaratulaCC = 225,
        [DescriptionAttribute("Reporte Caratula CC Sin Obra")]
        rptCaratulaCCSinObra = 226,
        [DescriptionAttribute("Reporte Acto Condicion")]
        rptActoCondicion = 227,
        [DescriptionAttribute("Contratos CH")]
        rptDocumentosEmpleados = 228,
        [DescriptionAttribute("Estatus Diario Maquinaria")]
        rptESTATUS_DIARIO_MAQUINARIA = 229,
        [DescriptionAttribute("Acta administrativa")]
        rptActa = 230,
        [DescriptionAttribute("Correo Acto Condicion")]
        rptNotificacionActoCondicion = 231,
        [DescriptionAttribute("Datos Tendencia BL")]
        rptDatosTendenciaBL = 232,
        [DescriptionAttribute("Orden de Compra")]
        ordenCompraAuditoria = 240,
        [DescriptionAttribute("Orden de Compra Interna")]
        ordenCompraInterna = 241,
        [DescriptionAttribute("Estado de Resultados")]
        rptEstadoResultados = 233,
        [DescriptionAttribute("Gestion de Proyecto OC")]
        GestionDeProyecto = 242,
        [DescriptionAttribute("Estado posicion financiero")]
        estadoPosicionFinanciero = 243,
        [DescriptionAttribute("Historial Clinico")]
        historialClinico = 244,
        [DescriptionAttribute("SOCertificado")]
        SOCertificado = 245,
        [DescriptionAttribute("Atención Médica")]
        atencionMedica = 246,
        [DescriptionAttribute("Matriz de riesgo")]
        rptMatrizDeRiesgo = 247,
        [DescriptionAttribute("Solicitud de Cheque Banco 1")]
        rptSolicitudCheque1 = 248,
        [DescriptionAttribute("Evaluacion de Subcontratista")]
        EvaluaciondeSubcontratista = 249,
        [DescriptionAttribute("Fechas Vacaciones")]
        rptFechasVacaciones = 250,
        [DescriptionAttribute("Plan Maestro")]
        rptPlanMaestro = 251,
        [DescriptionAttribute("Solicitud de Cheque Banco 2")]
        rptSolicitudCheque2 = 252,
        [DescriptionAttribute("Solicitud de Cheque Banco 3")]
        rptSolicitudCheque3 = 253,
        [DescriptionAttribute("Credencial Alta Empleado")]
        rptCredencialEmpleados = 254,
        [DescriptionAttribute("Plan de Acción")]
        rptPlanAccion = 255,
        [DescriptionAttribute("Aviso de baja de personal")]
        rptAvisoBaja = 256,
        [DescriptionAttribute("Plan de Acción")]
        rptPlanAccionCC = 257,
        [DescriptionAttribute("Staffing")]
        rptStaffing = 258,
        [DescriptionAttribute("Cédula de Costos por Obra y/o Departamento")]
        rptCedulaCostosNomina = 259,
        [DescriptionAttribute("Cédula de Costos por Obra y/o Departamento")]
        rptCedulaCostosNomina2 = 260,
        [DescriptionAttribute("Póliza de vales de despensa")]
        rptPolizaOCSI = 261,
        [DescriptionAttribute("Solicitud de Cheque Banco 4")]
        rptSolicitudCheque4 = 262,
        [DescriptionAttribute("Cédula de Costos por Obra y/o Departamento")]
        rptCedulaCostosNomina3 = 263,
        [DescriptionAttribute("Cédula de Costos por Obra y/o Departamento")]
        rptCedulaCostosNomina4 = 264,
        [DescriptionAttribute("Prestamos")]
        rptPrestamos = 265,
        [DescriptionAttribute("Laboral")]
        rptLaboral = 266,
        [DescriptionAttribute("Liberacion")]
        rptLiberacion = 267,
        [DescriptionAttribute("Pagare")]
        rptPagare = 268,
        [DescriptionAttribute("Examen Médico")]
        rptExamenMedico = 269,
        [DescriptionAttribute("Fonacot")]
        rptFonacot = 270,
        [DescriptionAttribute("Guarderia")]
        rptGuarderia = 271,
        [DescriptionAttribute("Lactancia")]
        rptLactancia = 272,
        [DescriptionAttribute("Incapacidades")]
        rptIncapacidades = 273,
        [DescriptionAttribute("Reporte360")]
        rptReporte360 = 274,
        [DescriptionAttribute("Acto Condicion CH - Amonestación")]
        rptAmonestacion = 275,
        [DescriptionAttribute("Acto Condicion CH - Suspensión")]
        rptSuspension = 276,
        [DescriptionAttribute("Acto Condicion CH - Carta de responsabilidad")]
        rptCartaResponsabilidad = 277,
        [DescriptionAttribute("Acto Condicion CH - Acta administrativa")]
        rptActaAdministrativa = 278,
        [DescriptionAttribute("Calendario Subcontratistas")]
        rptCalendarioSubContratistas = 279,
        [DescriptionAttribute("Tabuladores")]
        rptTabuladores = 280,
        [DescriptionAttribute("Cuentas por cobrar")]
        rptCuentasPorCobrar = 281,
        [DescriptionAttribute("Seguimiento PlanAccion")]
        rptSeguimientoPlanAccion = 282,
        [DescriptionAttribute("Reporte Ejecutivo 5'S")]
        rptReporteEjecutivo5s = 283,
        [DescriptionAttribute("Calendario CincoS")]
        rptCincoSCalendario = 284,
        [DescriptionAttribute("Auditorias CincoS")]
        rptCincoSAuditorias = 285,
        [DescriptionAttribute("Certificado de trabajo")]
        rptCertificadoTrabajo = 286,
        [DescriptionAttribute("Reporte tabuladores")]
        rptRepTabuladores = 289,
        [DescriptionAttribute("Reporte tabuladores modificación")]
        rptRepTabuladoresModificacion = 290,
        [DescriptionAttribute("Reporte Panillas Peru")]
        PLANB_A4_PDT2_MODELO_CLOGO = 291,
        [DescriptionAttribute("Reporte Económicos Sin Horómetros")]
        rptEconomicosSinHorometros = 292,
        [DescriptionAttribute("Reporte Tiempo Proceso OC")]
        rptTiempoProcesoOC = 293,
        [DescriptionAttribute("Recibo Nomina Peru")]
        rptReciboNominaPeru = 294,
        [DescriptionAttribute("Justificaciones")]
        rptJustificaciones = 295,
    }
}