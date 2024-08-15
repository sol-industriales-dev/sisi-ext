using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum DocumentosEnum
    {
        [DescriptionAttribute("Formato_Cambio")]
        Formato_Cambio = 1,
        [DescriptionAttribute("Facultamiento Orden Compras")]
        Facultamiento_Orden_Compras = 2,
        [DescriptionAttribute("Reporte de remoción Overhaul")]
        Remocion_Overhual = 3,
        [DescriptionAttribute("Reporte de Facultamientos")]
        Facultamiento_General = 4,
        [DescriptionAttribute("Aditiva/Deductiva")]
        AditivaDeductiva = 5,
        [DescriptionAttribute("PlantillaPersonal")]
        PlantillaPersonal = 6,
        [DescriptionAttribute("Reporte de Falla")]
        Reporte_Falla = 7,
        [DescriptionAttribute("Reporte de Cadenas Productivas")]
        Reporte_CadenaProductiva = 8,
        [DescriptionAttribute("Bono Administrativo")]
        BonoAdministrativo = 9,
        [DescriptionAttribute("Formato de Autorización de Capacitación")]
        FormatoAutorizacionCapacitacion = 10,
        [DescriptionAttribute("Programa de Inversión Anual")]
        ProgramaInversionAnual = 11,
        [DescriptionAttribute("AutorizacionFinanciero")]
        AutorizacionFinanciero = 12,
        [DescriptionAttribute("CuadroComparativoAdquisicionYRenta")]
        CuadroComparativoAdquisicionYRenta = 13,
        [DescriptionAttribute("BackLogs")]
        BackLogs = 14,
        [DescriptionAttribute("Prenomina")]
        Prenomina = 15,
        [DescriptionAttribute("Caratula")]
        Caratula = 16,
        [DescriptionAttribute("AutorizacionEvaluacion")]
        AutorizacionEvaluacion = 17,
        [DescriptionAttribute("Autorizacion de orden de cambio")]
        AutorizacionDeOrdenDeCambio = 18,
        [DescriptionAttribute("Rechazo de orden de cambio")]
        RechazarOrdenDeCambio = 19,
        [DescriptionAttribute("Solicitud de Cheque")]
        AutorizacionSolicitudCheque = 20,
        [DescriptionAttribute("Liberacion Contabilidad")]
        LiberacionContabilidad = 21,
        [DescriptionAttribute("Actividades Seg. Candidatos")]
        ActividadesSeguimiento = 22,
        [DescriptionAttribute("Reporte Prestamos")]
        FirmasPrestamos = 23,
        [DescriptionAttribute("Reporte Lactancia")]
        FirmasLactancia = 24,
        [DescriptionAttribute("Firma Orden Cambio")]
        FirmaOC = 25,
        [DescriptionAttribute("Firma Gestion Vacaciones")]
        FirmaGestionVacaciones = 26,
        [DescriptionAttribute("Tabuladores de puestos")]
        TabuladoresPuestos = 27
    }
}
