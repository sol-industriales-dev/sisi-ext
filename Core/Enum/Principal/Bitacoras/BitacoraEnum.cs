using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal.Bitacoras
{
    public enum BitacoraEnum : int
    {
        [DescriptionAttribute("Usuario")]
        USUARIO = 1,
        [DescriptionAttribute("Tipo Maquinaria")]
        TIPOMAQUINARIA = 2,
        [DescriptionAttribute("Grupo Maquinaria")]
        GRUPOMAQUINARIA = 3,
        [DescriptionAttribute("Conjunto")]
        CONJUNTO = 4,
        [DescriptionAttribute("SubConjunto")]
        SUBCONJUNTO = 5,
        [DescriptionAttribute("Componente")]
        COMPONENTE = 6,
        [DescriptionAttribute("Maquina")]
        MAQUINA = 7,
        [DescriptionAttribute("MARCA")]
        MARCA = 8,
        [DescriptionAttribute("MODELO")]
        MODELO = 9,
        [DescriptionAttribute("ASEGURADORA")]
        ASEGURADORA = 10,
        [DescriptionAttribute("MINADO")]
        MINADO = 11,
        [DescriptionAttribute("HOROMETROSCAPTURA")]
        HOROMETROSCAPTURA = 12,
        [DescriptionAttribute("RITMOHOROMETRO")]
        RITMOHOROMETRO = 13,
        [DescriptionAttribute("DESFASE")]
        DESFASE = 14,
        [DescriptionAttribute("CAPTURACOMBUSTIBLE")]
        CAPTURACOMBUSTIBLE = 15,
        [DescriptionAttribute("BASEDATOSSOS")]
        BASEDATOSSOS = 16,
        [DescriptionAttribute("MINUTA")]
        MINUTA = 17,
        [DescriptionAttribute("ACTIVIDAD")]
        ACTIVIDAD = 18,
        [DescriptionAttribute("COMENTARIO")]
        COMENTARIO = 19,
        [DescriptionAttribute("PARTICIPANTE")]
        PARTICIPANTE = 20,
        [DescriptionAttribute("INTERESADO")]
        INTERESADO = 21,
        [DescriptionAttribute("FORMATOCAMBIORH")]
        FORMATOCAMBIORH = 22,
        [DescriptionAttribute("SOLICITUD EQUIPO")]
        SOLICITUDEQUIPO = 23,
        [DescriptionAttribute("CONTROL ENVIO")]
        CONTROLENVIO = 24,
        [DescriptionAttribute("CONTROL RECEPCION")]
        CONTROLRECEPCION = 25,
        [DescriptionAttribute("ASIGNACION MAQUINARIA")]
        ASIGNACIONMAQUINARIA = 26,
        [DescriptionAttribute("CONTROL DE CALIDAD")]
        CONTROLCALIDAD = 27,
        [DescriptionAttribute("RESPUESTAS DE CALIDAD")]
        RESPUESTASCALIDAD = 28,
        [DescriptionAttribute("SOLICITUD DE STANDBY")]
        SOLICITUDSTANDBY = 29,
        [DescriptionAttribute("MAQUINARIA RENTADA")]
        MAQUINARIARENTADA = 30,
        [DescriptionAttribute("CADENA PRODUCTIVA")]
        CADENAPRODUCTIVA = 31,
        [DescriptionAttribute("ALTA OBRA PROYECCIONES")]
        ALTAOBRAPROYECCIONES = 32,
        [DescriptionAttribute("ALTA DEPARTAMENTOS")]
        ALTADEPARTAMENTOS = 33,
        [DescriptionAttribute("GUARDAR CAPTURA DE OBRAS")]
        GUARDARCAPTURAOBRA = 34,
        [DescriptionAttribute("Saldos Iniciales")]
        SALDOSINICIALES = 35,
        [DescriptionAttribute("CxC")]
        CxC = 36,
        [DescriptionAttribute("CATRESPONSABLES")]
        CATRESPONSABLES = 37,
        [DescriptionAttribute("NOTACREDITO")]
        NOTACREDITO = 38,
        [DescriptionAttribute("EFICIENCIA")]
        EFICIENCIA = 39,
        [DescriptionAttribute("NotasCreditoArchivo")]
        NotasCreditoArchivos = 40,
        [DescriptionAttribute("Solicitud de Reemplazo Detalle")]
        SolicitudReempazoDet = 41,
        [DescriptionAttribute("Solicitud de Reemplazo")]
        SolicitudReempazo = 42,
        [DescriptionAttribute("Reportes indicadores de maquinarias")]
        RptIndicador = 43,
        [DescriptionAttribute("Prefactura")]
        PREFACTURA = 44,
        [DescriptionAttribute("ResguardodeEquipo")]
        RESGUARDOEQUIPO = 45,
        [DescriptionAttribute("RepPrefactura")]
        REPPREFACTURA = 46,
        [DescriptionAttribute("FilePrefactura")]
        FilePrefactura = 47,
        [DescriptionAttribute("Archivos Modelos")]
        ModeloArchivos = 48,
        [DescriptionAttribute("respuestas resguardo")]
        ResguardoRespuestas = 49,
        [DescriptionAttribute("Autorizacion resguardo")]
        AutorizacionResguardo = 50,
        [DescriptionAttribute("Documentos resguardo")]
        DocumentosResguardo = 51,
        [DescriptionAttribute("Catpura de OT")]
        CAPTURAOT = 52,
        [DescriptionAttribute("Catpura de OT Det")]
        CAPTURAOTDET = 53,
        [DescriptionAttribute("Autorizacion Solicitud reemplazo")]
        AUTORIZACIONSOLICITUDREEMPLAZO = 54,
        [DescriptionAttribute("ResponsablesMinuta")]
        RESPONSABLESMINUTA = 55,
        [DescriptionAttribute("STAND BY")]
        STANDBY = 561,
        [DescriptionAttribute("STAND BY DETALLE ")]
        STANDBYDETALLE = 56,
        [DescriptionAttribute("CapturaImportes")]
        CAPIMPORTES = 57,
        [DescriptionAttribute("Validacion Standby")]
        VALIDASTANDBY = 58,
        [DescriptionAttribute("Maquina de aceite y lubricante")]
        MaquinaAceiteLubricante = 59,
        [DescriptionAttribute("AUTORIZACION MOVIMIENTO INTERNO")]
        AUTORIZACIONMOVIMIENTOINTERNO = 60,
        [DescriptionAttribute("CONTROL MOVIMIENTO INTERNO")]
        CONTROLMOVIMIENTOINTERNO = 61,
        [DescriptionAttribute("Inicio cadena productiva")]
        CadenaPrincipal = 62,
        [DescriptionAttribute("AUTORIZACION SOLICITUDES")]
        AUTORIZASOLICITUD = 63,
        [DescriptionAttribute("Número Nafin")]
        NumNafin = 64,
        [DescriptionAttribute("Reserva de cadena productiva")]
        ReservaCadenaProductiva = 65,
        [DescriptionAttribute("Catálogo CC saldo base")]
        CatCCBase = 66,
        [DescriptionAttribute("Catálogo tipo aceites")]
        TIPOACEITES = 67,
        [DescriptionAttribute("Comentarios de proyecciones")]
        COMENTARIOPROYECCIONES = 68,
        [DescriptionAttribute("Datos de finalizacion Obra")]
        TERMINACIONOBRA = 69,
        [DescriptionAttribute("Catalogo de escenarios proyecciones")]
        PROCATESCENARIOS = 70,
        [DescriptionAttribute("Formatos Aditiva-Deductiva")]
        AditivaPersonal = 71,
        [DescriptionAttribute("AutorizacionFormatos Aditiva-Deductiva")]
        AutorizacionAditivaPersonal = 72,
        [DescriptionAttribute("Formatos Aditiva-Deductiva Detalle")]
        AditivaPersonalDetalle = 73,
        [DescriptionAttribute("Captura Curso")]
        Curso = 74,
        [DescriptionAttribute("Captura Modulo")]
        Modulo = 75,
        [DescriptionAttribute("Captura Modulo Detalle")]
        ModuloDet = 76,
        [DescriptionAttribute("Captura Examen")]
        Examen = 77,
        [DescriptionAttribute("Poliza")]
        Poliza = 78,
        [DescriptionAttribute("Movimiento de póliza")]
        MovPoliza = 79,
        [DescriptionAttribute("REPORTE FALLAS OVERHAUL")]
        RepFallasOverhaul = 80,
        [DescriptionAttribute("COMPONENTES POR MODELO")]
        COMPONENTESXMODELO = 81,
        [DescriptionAttribute("ALTA CIFRAS PRINCIPALES")]
        ALTACIFRASPRINCIPALES = 82,
        [DescriptionAttribute("DOCUMENTOS ESPECIALES DE EQUIPOS")]
        DOCUMENTOSMAQUINARIA = 83,
        [DescriptionAttribute("FechaPago")]
        FechaPago = 84,
        [DescriptionAttribute("FacturaParcial")]
        FacturaParcial = 85,
        [DescriptionAttribute("Captura de horas hombres")]
        capHorasHombre = 86,
        [DescriptionAttribute("Facultamiento")]
        Facultamiento = 86,
        [DescriptionAttribute("Autorización de facultamiento")]
        FacultamientoAuto = 87,
        [DescriptionAttribute("Monto de facultamiento")]
        FacultamientoMonto = 88,
        [DescriptionAttribute("Relación de facultamiento")]
        FacultamientoRel = 89,
        [DescriptionAttribute("BAJA DE EMPLEADOS POR MEDIO DE LAYAUT BAJA")]
        LAYAUTBAJARH = 90,
        [DescriptionAttribute("REPORTE DE RENDIMIENTO DE COMBUSTIBLE")]
        RPTRENDIMIENTOCOMBUSTIBLE = 91,
        [DescriptionAttribute("DISPONIBILIDAD KPI")]
        RPTDISPONIBILIDADKPI = 92,
        [DescriptionAttribute("PARO DE FRECUENCIAS")]
        RPTFRECUENCIASKPI = 93,
        [DescriptionAttribute("CC")]
        CC = 94,
        [DescriptionAttribute("Requisicion")]
        Requisicion = 95,
        [DescriptionAttribute("Directorio")]
        Directorio = 96,
        [DescriptionAttribute("CAPTURA DE CARATULA")]
        CAPCARATULA = 97,
        [DescriptionAttribute("AUTORIZACION CARATULA")]
        AUTORIZACIONCARATULA = 98,
        [DescriptionAttribute("CAPTURA CARATULAS DET")]
        CAPCARATULADET = 99,
        [DescriptionAttribute("Rel de usuaruio sigoplan y enkontrol")]
        UsuarioEnkontrol = 100,
        [DescriptionAttribute("Plantilla de Facultamiento")]
        PlantillaFacultamiento = 101,
        [DescriptionAttribute("CAPTURA CONCILIACION")]
        CAPTURA_CONCILIACION = 102,
        [DescriptionAttribute("AUTORIZACION CONCILIACION")]
        AUTORIZACION_CONCILIACION = 103,
        [DescriptionAttribute("Reporte Remocion Componente")]
        REPORTEREMOCIONCOMPONENTE = 104,
        [DescriptionAttribute("PaqueteFacultamientos")]
        PaqueteFacultamientos = 105,
        [DescriptionAttribute("Intereses Nafin")]
        InteresesNafin = 106,
        [DescriptionAttribute("Catálogo de giro")]
        GiroGrupo = 107,
        [DescriptionAttribute("Catálogo de giro de proveedores")]
        GiroProveedor = 108,
        [DescriptionAttribute("Polizas de nómians")]
        NominaPoliza = 109,
        [DescriptionAttribute("Resumen de nóminas")]
        NominaResumen = 110,
        [DescriptionAttribute("Condensado de saldos del analítico de proveedores")]
        SaldoCondensado = 111,
        [DescriptionAttribute("Resumen de estimaciones del analítico de los clientes")]
        EstimacionesResumen = 112,
        [DescriptionAttribute("Autorización del Resumen de estimaciones del analítico de los clientes")]
        AuthEstimacionesResumen = 113,
        [DescriptionAttribute("GUARDAR TRACKING COMPONENTE")]
        GUARDARTRACKCOMPONENTE = 120,
        [DescriptionAttribute("CALENDARIO OVERHAUL")]
        CALENDARIO_OVERHAUL = 121,
        [DescriptionAttribute("ALTA TIPO SERVICIO OVERHAUL")]
        TIPO_SERVICIO_OVERHAUL = 122,
        [DescriptionAttribute("ALTA SERVICIO OVERHAUL")]
        SERVICIO_OVERHAUL = 123,
        [DescriptionAttribute("Propuesta Reserva")]
        PropuestaReserva = 124,
        [DescriptionAttribute("Propuesta Saldo Conciliado")]
        PropuestaSaldoConciliado = 125,
        [DescriptionAttribute("Plantilla Personal")]
        PlantillaPersonal = 126,
        [DescriptionAttribute("Metas")]
        Metas = 127,
        [DescriptionAttribute("KPI homologado Autorizacion")]
        KPHomologadoAutorizacion = 128,
        [DescriptionAttribute("Gestion de Aperturas de Obra")]
        AperuraObra = 1024,
        [DescriptionAttribute("Agrupacion de Componente Modelo")]
        ComponenteModeloAgrupacion = 1025,
        [DescriptionAttribute("Agrupacion de Componente Lubricantes")]
        ComponenteLubricanteAgrupacion = 1026,
        [DescriptionAttribute("Agrupacion de Componente filtros")]
        ComponenteFiltroAgrupacion = 1027,
        [DescriptionAttribute("Agrupacion de Esquemas")]
        AgrupacionEsquemas = 1028,
        [DescriptionAttribute("Catalogo de Examenes")]
        tblS_CatExamen = 1029,
        [DescriptionAttribute("Catalogo de Examenes")]
        tblS_PuestosSegurdidad = 1030,
        [DescriptionAttribute("Bono Administrativo")]
        BONO_ADMIN = 1031,
        [DescriptionAttribute("Solicitud Justificacion")]
        SOLICITUD_JUSTIFICACION = 1032,
        [DescriptionAttribute("AutorizacionFinanciero")]
        AutorizacionFinanciero = 1033,
        [DescriptionAttribute("CuadroComparativoAdquisicionYRenta")]
        CuadroComparativoAdquisicionYRenta = 1034,
        [DescriptionAttribute("Activo Fijo")]
        ActivoFijo = 1035,
        [DescriptionAttribute("CARATULAS")]
        CARATULAS = 1036,
    }
}
