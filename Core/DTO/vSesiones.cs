using Core.DTO.Administracion.Cotizaciones;
using Core.DTO.Principal.Generales;
using Core.DTO.Principal.Usuarios;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Core.DTO
{
    public static class vSesiones
    {
        private static HttpSessionState session { get { return HttpContext.Current.Session; } }
        public static UsuarioDTO sesionUsuarioDTO
        {
            get { return session["usuarioDTO"] as UsuarioDTO; }
            set { session["usuarioDTO"] = value; }
        }
        public static List<int> sesionVistaExcepcionPalabraCC
        {
            get { return session["VistaExcepcionPalabraCC"] as List<int>; }
            set { session["VistaExcepcionPalabraCC"] = value; }
        }
        public static int sesionBestRouting
        {
            get { return Convert.ToInt32(session["bestRouting"]); }
            set { session["bestRouting"] = value; }
        }
        public static int sesionEmpresaActual
        {
            get { return Convert.ToInt32(session["empresaActual"]); }
            set { session["empresaActual"] = value; }
        }
        public static string sesionEmpresaDBPregijo
        {
            get { return (Convert.ToInt32(session["empresaActual"])==3?"\"dba\".":"") as string; }
        }
        public static string sesionEmpresaActualNombre
        {
            get { return session["empresaActualNombre"] as string; }
            set { session["empresaActualNombre"] = value; }
        }
        public static int sesionSistemaActual
        {
            get { return Convert.ToInt32(session["sistemaActual"]); }
            set { session["sistemaActual"] = value; }
        }
        public static string sesionSistemaActualNombre
        {
            get { return session["sistemaActualNombre"] as string; }
            set { session["sistemaActualNombre"] = value; }
        }
        public static string sesionMenuHTML
        {
            get { return session["menuHTML"] as string; }
            set { session["menuHTML"] = value; }
        }
        public static string sesionBreadCrumsHTML
        {
            get { return session["BreadCrumsHTML"] as string; }
            set { session["BreadCrumsHTML"] = value; }
        }
        public static string sesionCurrentMenu
        {
            get { return session["CurrentMenu"] as string; }
            set { session["CurrentMenu"] = value; }
        }
        public static string sesionCurrentReport
        {
            get { return session["currentReport"] as string; }
            set { session["currentReport"] = value; }
        }
        public static string sesionCurrentReportDetIncidencias
        {
            get { return session["currentReportDetIncidencias"] as string; }
            set { session["currentReportDetIncidencias"] = value; }
        }
        public static DateTime sesionServerTime
        {
            get { return Convert.ToDateTime(session["serverTime"]); }
            set { session["serverTime"] = value; }
        }
        public static DateTime sesionUpdateSession
        {
            get { return Convert.ToDateTime(session["updateSession"]); }
            set { session["updateSession"] = value; }
        }
        public static ResolutionDTO sesionCurrentResolution
        {
            get { return session["currentResolution"] as ResolutionDTO; }
            set { session["currentResolution"] = value; }
        }
        public static int sesionCurrentView
        {
            get { return Convert.ToInt32(session["sesionCurrentView"]); }
            set { session["sesionCurrentView"] = value; }
        }
        public static List<CotizacionDTO> ReporteCotizacionDTO
        {
            get { return session["ReporteCotizacionDTO"] as List<CotizacionDTO>; }
            set { session["ReporteCotizacionDTO"] = value; }
        }

        public static List<string> sesionArrProyectos
        {
            get { return session["sesionArrProyectos"] as List<string>; }
            set { session["sesionArrProyectos"] = value; }
        }

        public static string sesionPeriodoInicial
        {
            get { return session["sesionPeriodoInicial"] as string; }
            set { session["sesionPeriodoInicial"] = value; }
        }

        public static string sesionPeriodoFinal
        {
            get { return session["sesionPeriodoFinal"] as string; }
            set { session["sesionPeriodoFinal"] = value; }
        }

        public static string sesionNominaSemanal
        {
            get { return session["sesionNominaSemanal"] as string; }
            set { session["sesionNominaSemanal"] = value; }
        }
        public static List<tblM_CapNominaCC_Detalles> sesionNominaCCDetalles
        {
            get { return session["sesionNominaCCDetalles"] as List<tblM_CapNominaCC_Detalles>; }
            set { session["sesionNominaCCDetalles"] = value; }
        }
        public static bool sesionUsuarioMAZDA
        {
            get { return (bool)session["sesionUsuarioMAZDA"]; }
            set { session["sesionUsuarioMAZDA"] = value; }
        }
        public static bool sesionUsuarioExternoSeguridad
        {
            get { return (bool)session["sesionUsuarioExternoSeguridad"]; }
            set { session["sesionUsuarioExternoSeguridad"] = value; }
        }
        public static bool sesionUsuarioExternoPatoos
        {
            get { return (bool)session["sesionUsuarioExternoPatoos"]; }
            set { session["sesionUsuarioExternoPatoos"] = value; }
        }
        public static int sesionDivisionActual
        {
            get { return Convert.ToInt32(session["divisionActual"]); }
            set { session["divisionActual"] = value; }
        }

        public static bool sesionCapacitacionOperativa
        {
            get { return (bool)session["sesionCapacitacionOperativa"]; }
            set { session["sesionCapacitacionOperativa"] = value; }
        }

        public static bool sesionVersionCompraOriginal
        {
            get { return (bool)session["sesionVersionCompraOriginal"]; }
            set { session["sesionVersionCompraOriginal"] = value; }
        }

        public static List<byte[]> downloadPDF
        {
            get { return (List<byte[]>)session["downloadPDF"]; }
            set { session["downloadPDF"] = value; }
        }

        public static EnkontrolAmbienteEnum sesionAmbienteEnkontrolAdm
        {
            get { return (EnkontrolAmbienteEnum)session["enkontrolAmbienteEnumAdm"]; }
            set { session["enkontrolAmbienteEnumAdm"] = value; }
        }

        public static EnkontrolAmbienteEnum sesionAmbienteEnkontrolRh
        {
            get { return (EnkontrolAmbienteEnum)session["enkontrolAmbienteEnumRh"]; }
            set { session["enkontrolAmbienteEnumRh"] = value; }
        }
        
        public static void clear()
        {
            session.Abandon();
            session.RemoveAll();
            session.Clear();
        }
        public static void reset()
        {
            session["usuarioDTO"] = null;
            session["empresaActual"] = null;
            session["sistemaActual"] = null;
            session["menuHTML"] = null;
            session["BreadCrumsHTML"] = null;
            session["CurrentMenu"] = null;
        }
    }
}