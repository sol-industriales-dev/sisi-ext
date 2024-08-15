using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Factory.Maquinaria.Captura;
using System.Web.Mvc;
using System.Reflection;

using Core.Enum.Maquinaria.Reportes;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Utils;
using Data.Factory.Maquinaria.Reporte;
using Reportes.Reports;
using Core.DTO.Maquinaria.Reporte;
using System.Globalization;
using System.IO;
using Data.Factory.Encuestas;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Proyecciones;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.SeguimientoAcuerdos;
using Data.Factory.RecursosHumanos.Captura;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Maquinaria.Inventario.ControlCalidad;
using Data.Factory.Maquinaria.Overhaul;
using Data.Factory.Facturacion.Prefacturacion;
using Data.Factory.Contabilidad;
using CrystalDecisions.Shared;
using Core.Entity.Maquinaria.Catalogo;
using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System.Windows.Forms;
using Core.Entity.Maquinaria;
using Core.DTO.Captura;
using Reportes.Reports.SeguimientoAcuerdos;
using Reportes.Reports.Maquinaria;
using Data.Factory.SeguimientoAcuerdos;
using Core.DTO.SeguimientoAcuerdos;
using Reportes.Reports.Captura;
using Core.DTO.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Usuarios;

using Data.Factory.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Core.DTO.RecursosHumanos;
using Core.Enum.RecursosHumanos;

using Reportes.Reports.Inventario;
using Data.Factory.Maquinaria.Inventario;
using Core.Enum.Maquinaria;
using Core.DTO;
using Core.Entity.RecursosHumanos.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using Reportes.Reports.RecursosHumanos;
using Core.DTO.Maquinaria;
using Data.Factory.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Reportes.Reports.Contabilidad;
using Core.DTO.Contabilidad;
using Reportes.Reports.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Catalogos;
using Reportes.Reports.Proyecciones;
using Core.DTO.Proyecciones;
using Data.Factory.Proyecciones;
using Reportes.Reports.Overhaul;
using Data.Factory.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Catalogo;
using Reportes.Reports.Administracion.ControlInterno;
using Data.Factory.Administracion.ControlInterno.Reporte;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System.Text.RegularExpressions;
using Core.Entity.Maquinaria.Reporte;
using System.Web.Script.Serialization;
using Data.Factory.Facturacion.Prefacturacion;
using Reportes.Reports.Facturacion.Prefacturacion;
using Core.Entity.Maquinaria.Inventario;
using Core.DTO.Reportes;
using Core.DTO.Maquinaria.Overhaul;
using Core.DAO.Maquinaria.Rastreo;
using Core.DTO.Maquinaria.Rastreo;
using System.Drawing.Printing;
using Reportes.Reports.Administracion.Cotizaciones;
using Reportes.DataSet.Administracion.Cotizaciones;
using Core.DTO.RecursosHumanos.aditivadeductivaDTO;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria.Captura.aceites;
using Data.Factory.Contabilidad;
using System.Drawing;
using Reportes.Reports.Captura.OT;
using Reportes.Reports.Encuestas.Proveedores;
using Data.Factory.Encuestas;
using Core.DTO.Encuestas.Proveedores.Reportes;
using Reportes.Reports.Encuestas.Construccion;
using Reportes.Reports.Administracion.Facultamiento;
using Data.Factory.Administracion.Facultamiento;
using Core.Entity.Administrativo.Facultamiento;
using Core.Enum.Administracion.Facultamiento;
using Reportes.Reports.Maquinaria.HorasHombre;
using Reportes.DataSet.Maquinaria.Captura.HorasHombre;
using Core.Enum;
using Data.Factory.Maquinaria.Captura.HorasHombre;
using Core.DTO.Administracion.ControlInterno;
using Infrastructure.DTO;
using Reportes.Reports.Maquinaria.Mantenimiento;
using Data.Factory.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.DTO.Maquinaria.Mantenimiento;
using Core.Enum.Multiempresa;
using Reportes.Reports.Captura.ConciliacionHorometros;
using Core.Enum.Maquinaria.ConciliacionHorometros;
using Core.DTO.MAZDA;
using Reportes.Reports.Mazda;
using Data.Factory.MAZDA;
using Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion;
using Reportes.Reports.Maquinaria.Conciliacion;
using Newtonsoft.Json.Converters;
using Core.DTO.Maquinaria.Captura.conciliacion;
using Core.DAO.Administracion.FacultamientosDpto;
using Data.Factory.Administracion.FacultamientsDpto;
using Core.DTO.Administracion.Facultamiento;
using Data.Factory.Principal.Bitacora;
using Core.Enum.Principal;
using Core.DAO.Principal.Bitacoras;
using Core.Enum.Principal.Bitacoras;

namespace SIGOPLAN.Reportes
{
    public partial class VistaNoButtons : System.Web.UI.Page
    {

        #region Factory
        ConciliacionFactoryServices cfs = new ConciliacionFactoryServices();
        CapHorasHombreFactoryServices capHorasHombreFactoryServices = new CapHorasHombreFactoryServices();
        EncuestasSubContratistasFactoryServices encuestasSubContratistasFactoryServices = new EncuestasSubContratistasFactoryServices();
        EncuestasProveedoresFactoryServices encuestasProveedoresFactoryServices = new EncuestasProveedoresFactoryServices();
        MotivosParoFactoryServices motivosParoFactoryServices = new MotivosParoFactoryServices();
        AsignacionEquiposFactoryServices asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
        AutorizaMovimientosInternosFactoryServices autorizaMovimientosInternosFactoryServices = new AutorizaMovimientosInternosFactoryServices();
        ControlInternoMovimientoFactoryServices controlInternoMovimientoFactoryServices = new ControlInternoMovimientoFactoryServices();
        StandbyDetFactoryServices standbyDetFactoryServices = new StandbyDetFactoryServices();
        StandbyFactoryServices standbyFactoryServices = new StandbyFactoryServices();
        CapturaOTFactoryServices capturaOTFactoryServices = new CapturaOTFactoryServices();
        CapturaOTDetFactoryServices capturaOTDetFactoryServices = new CapturaOTDetFactoryServices();
        ResguardoEquipoFactoryServices resguardoEquipoFactoryServices = new ResguardoEquipoFactoryServices();
        RespuestaResguardoVehiculosFactoryServices respuestaResguardoVehiculosFactoryServices = new RespuestaResguardoVehiculosFactoryServices();
        CapturaHorometroFactoryServices capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
        TESTDATAFactoryServices tESTDATAFactoryServices = new TESTDATAFactoryServices();
        CapturaCombustibleFactoryServices capturaCombustibleFactoryServices = new CapturaCombustibleFactoryServices();
        RepComparativaTiposFactoryServices repComparativaTiposFactoryServices = new RepComparativaTiposFactoryServices();
        EncabezadoFactoryServices encabezadoFactoryServices = new EncabezadoFactoryServices();
        SeguimientoAcuerdosFactoryServices seguimientoAcuerdosFactoryServices = new SeguimientoAcuerdosFactoryServices();
        CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        FormatoCambioFactoryService capturaFormatoCambioFactoryServices = new FormatoCambioFactoryService();
        UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        AutorizacionFormatoCambioFactoryService capturaAutorizacionFormatoCambioService = new AutorizacionFormatoCambioFactoryService();
        ControlEnvioyRecepcionFactoryServices controlEnvioyRecepcionFactoryServices = new ControlEnvioyRecepcionFactoryServices();
        ControlCalidadFactoryServices ControlCalidadService = new ControlCalidadFactoryServices();
        GrupoPreguntasFactoryServices GrupoPreguntasCalidad = new GrupoPreguntasFactoryServices();
        PreguntasCalidadFactoryServices PreguntasCalidad = new PreguntasCalidadFactoryServices();
        RespuestasCalidadFactoryServices RespuestasCalidadService = new RespuestasCalidadFactoryServices();
        AutorizacionStandByFactoryServices autorizacionStandByFactoryServices = new AutorizacionStandByFactoryServices();
        CapturadeObrasFactoryServices capturadeObrasFactoryServices = new CapturadeObrasFactoryServices();
        NotaCreditoFactoryServices notaCreditoFactoryServices = new NotaCreditoFactoryServices();
        MaquinariaRentadaFactoryServices maquinariaRentadaFactoryServices = new MaquinariaRentadaFactoryServices();
        RepTraspasoFactoryServices RepTraspasoFactoryService = new RepTraspasoFactoryServices();
        EficienciaFactoryService EficienciaFactoryService = new EficienciaFactoryService();
        GrupoMaquinariaFactoryServices GrupoMaquinariaFactoryServices = new GrupoMaquinariaFactoryServices();
        private MaquinaFactoryServices MaquinaFactory = new MaquinaFactoryServices();
        RptIndicadorFactoryServices RptIndicadorFactoryServices = new RptIndicadorFactoryServices();
        PrefacturacionFactoryServices PrefacturacionFactoryServices = new PrefacturacionFactoryServices();
        RepPrefacturacionFactoryService RepPrefacturacionFactoryService = new RepPrefacturacionFactoryService();
        AceitesLubricantesFactoryService AceitesFactory = new AceitesLubricantesFactoryService();
        MaquinariaAceitesLubricantesFactoryService MaqAceiteFactory = new MaquinariaAceitesLubricantesFactoryService();
        KPIFactoryServices kpiFactoryServices = new KPIFactoryServices();
        private AditivaDeductivaFactoryService objAditivaDeductivaFactoryServices = new AditivaDeductivaFactoryService();
        private AutorizacionAditivaDeductivaFactoryService objAutadivaDeductivaFactoryService = new AutorizacionAditivaDeductivaFactoryService();
        private AditivaDeductivaDetFactoryService objAditivaDeductivaDetFactoryService = new AditivaDeductivaDetFactoryService();
        FacultamientoFactoryServices facultamientofs = new FacultamientoFactoryServices();
        PolizaFactoryServices polizaFS = new PolizaFactoryServices();
        FiniquitoFactoryService finiquitoFactoryServices = new FiniquitoFactoryService();
        private MantenimientoFactoryServices objMantenimientoFactory = new MantenimientoFactoryServices();
        MAZDAFactoryServices MAZDAFactoryServices = new MAZDAFactoryServices();
        RepAnalisisUtilizacionFactorySerrvices RepAnalisiUsos = new RepAnalisisUtilizacionFactorySerrvices();
        private FacultamientosFactoryServices facultamientosFactoryServices = new FacultamientosFactoryServices();

        ///// Service para poder loguear errores.
        private LogErrorFactoryServices logErrorFactoryServices = new LogErrorFactoryServices();
        #endregion
        ParameterFields paramFields;
        tblM_CatMaquina objMaquinaria;
        private ReportDocument rd = new ReportDocument();



        private List<FechasDTO> GetQuincenas()
        {
            var ListaFechas = new List<FechasDTO>();
            var FechaFin = new DateTime();
            var FechaInicio = new DateTime(DateTime.Now.Year, 01, 01);
            var anioActual = FechaInicio.Year;
            var diasSem = 7;
            var i = 0;
            FechaInicio = FechaInicio.AddDays(diasSem);
            while (FechaInicio.Year == anioActual)
            {
                int diasMiercoles = ((int)DayOfWeek.Wednesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaInicio = FechaInicio.AddDays(diasMiercoles);
                FechaFin = FechaInicio.AddDays(diasSem);
                int diasMartes = ((int)DayOfWeek.Tuesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaFin = FechaFin.AddDays(diasMartes);
                ListaFechas.Add(new FechasDTO()
                {
                    Value = ++i,
                    Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                });
                FechaInicio = FechaFin.AddDays(1);
            }
            return ListaFechas;
        }

        private List<FechasDTO> GetFechas(DateTime fecha)
        {

            List<FechasDTO> ListaFechas = new List<FechasDTO>();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFin = new DateTime();

            for (int i = 1; i <= 52; i++)
            {
                if (i == 1)
                {
                    var diaSemana = (int)fecha.DayOfWeek;
                    FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 4);
                    int diasViernes = ((int)DayOfWeek.Tuesday - (int)fecha.DayOfWeek + 7) % 7;
                    FechaFin = fecha.AddDays(diasViernes);

                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });

                }
                else
                {
                    var TempFecha = FechaFin.AddDays(1);

                    FechaInicio = TempFecha;
                    FechaFin = TempFecha.AddDays(6);

                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                }

            }

            return ListaFechas;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet dataT = new DataSet();
            paramFields = new ParameterFields();

            try
            {
                int reporte = Convert.ToInt32(Request.QueryString["idReporte"]);
                if (!IsPostBack)
                {
                    SetInfoReporte(reporte);

                }
                crvReporteEstandar.ReportSource = (ReportDocument)Session["reporte"];
                crvReporteEstandar.DataBind();

                crvReporteEstandar.DisplayGroupTree = false;
                crvReporteEstandar.EnableDrillDown = false;
                crvReporteEstandar.SeparatePages = false;
                crvReporteEstandar.DisplayToolbar = true;
                crvReporteEstandar.HasToggleGroupTreeButton = false;
                crvReporteEstandar.HasToggleParameterPanelButton = false;
                crvReporteEstandar.EnterpriseLogon = false;
                crvReporteEstandar.HasSearchButton = false;
                crvReporteEstandar.HasCrystalLogo = false;
                crvReporteEstandar.HasDrillUpButton = false;
                crvReporteEstandar.HasDrilldownTabs = false;
                crvReporteEstandar.ParameterFieldInfo = paramFields;
                crvReporteEstandar.EnableDatabaseLogonPrompt = false;
                crvReporteEstandar.DisplayToolbar = false;



            }
            catch (Exception)
            {


            }

        }

        private ParameterFields SetInfoReporte(int reporte)
        {
            paramFields = new ParameterFields();
            dsStandbyParmDTO StandbyParmDTO = new dsStandbyParmDTO();
            //Parametros *-
            string pCC = "";
            DateTime pFechaInicio = new DateTime();
            DateTime pFechaFin = new DateTime();
            string pEconomico = "";
            int pTurno = 0;
            string pNombreCC = "";

            string responseString = "";

            crvReporteEstandar.Dispose();
            rd.Close();
            rd.Dispose();
            switch ((ReportesEnum)reporte)
            {
                case ReportesEnum.semanal_preventivoEjecutar:
                    {
                        rd = new rptPlaneacionSemanal();

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Direccion de Maquinaria y Equipo", "Planificación semanal mantenimiento preventivo a ejectuar"));




                        Session.Add("reporte", rd);
                        break;
                    }


                case ReportesEnum.pm_Ejecutado:
                    {

                        rd = new rptPrecisionMantenimiento();
                        setMedidasReporte("HO");

                        string pAreaCuenta = Request.QueryString["pAreaCuenta"].ToString();
                        DateTime pFechaInicioPm = Convert.ToDateTime(Request.QueryString["pFechaInicioPm"].ToString());
                        DateTime pFechaFinPm = Convert.ToDateTime(Request.QueryString["pFechaFinPm"].ToString());
                        string pEconomico1 = Request.QueryString["pEconomico1"].ToString();

                        var programaSemanal = objMantenimientoFactory.getMantenimientoService().getPlaneacionSemanal(pAreaCuenta, pFechaInicioPm, pFechaFinPm, pEconomico1, false).Where(x => x.estatusPM == 3).ToList();

                        var pAdministradorMaquinaria = cfs.getConciliacionServices().getAuth(9, pAreaCuenta);
                        var pGerenteProyecto = cfs.getConciliacionServices().getAuth(1, pAreaCuenta);
                        var pProyecto = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pAreaCuenta);

                        var presision = programaSemanal.Select(x => new
                        {
                            economico = x.economico,
                            tipoServicio = x.tipoServicio,
                            fechaProgramado = x.fechaProgramado.ToShortDateString(),
                            horometroProgramado = x.horometroProgramado,
                            fechaEjecutado = x.fechaEjecutado.ToShortDateString(),
                            horometroEjecutado = x.horometroEjecutado,
                            observacion = x.observacion,
                            diferencia = x.horometroProgramado - x.horometroEjecutado,
                            porError = (x.horometroProgramado - x.horometroEjecutado) / 250,
                        }).ToList();

                        int countDesfasados = presision.Where(x => x.diferencia > 25 || x.diferencia < -25).Count();
                        int countTotal = presision.Where(x => x.porError > 10).Count();


                        decimal pPorCenError = countTotal != 0 ? ((countDesfasados / countTotal)) : (countDesfasados * 100);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Direccion de Maquinaria y Equipo", "Planeacion Semanal de mantenimiento a ejecutar"));
                        rd.Database.Tables[1].SetDataSource(ToDataTable(presision));

                        rd.SetParameterValue("pAdministradorMaquinaria", pAdministradorMaquinaria.usuario.nombre + " " + pAdministradorMaquinaria.usuario.apellidoPaterno);
                        rd.SetParameterValue("pPorCenError", Math.Round(pPorCenError, 2));
                        rd.SetParameterValue("pProyecto", pProyecto);
                        rd.SetParameterValue("pGerenteProyecto", pGerenteProyecto.usuario.nombre + " " + pGerenteProyecto.usuario.apellidoPaterno);
                        rd.SetParameterValue("pPeriodoFechas", "Del " + pFechaInicioPm.ToShortDateString() + " AL " + pFechaFinPm.ToShortDateString());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.pm_planeacionSemanal:
                    {

                        rd = new rptPlanificacionSemanal();
                        setMedidasReporte("HO");

                        string pAreaCuenta = Request.QueryString["pAreaCuenta"].ToString();
                        DateTime pFechaInicioPm = Convert.ToDateTime(Request.QueryString["pFechaInicioPm"].ToString());
                        DateTime pFechaFinPm = Convert.ToDateTime(Request.QueryString["pFechaFinPm"].ToString());
                        string pEconomico1 = Request.QueryString["pEconomico1"].ToString();

                        var programaSemanal = objMantenimientoFactory.getMantenimientoService().getPlaneacionSemanal(pAreaCuenta, pFechaInicioPm, pFechaFinPm, pEconomico1, true).Where(x => x.estatusPM > 0).ToList();

                        var pAdministradorMaquinaria = cfs.getConciliacionServices().getAuth(9, pAreaCuenta);
                        var pGerenteProyecto = cfs.getConciliacionServices().getAuth(1, pAreaCuenta);
                        var pProyecto = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pAreaCuenta);



                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Direccion de Maquinaria y Equipo", "Planeacion Semanal de mantenimiento a ejecutar"));
                        rd.Database.Tables[1].SetDataSource(ToDataTable(programaSemanal.Select(x => new
                        {
                            economico = x.economico,
                            tipoServicio = x.tipoServicio,
                            fechaProgramado = x.fechaProgramado.ToShortDateString(),
                            horometroProgramado = x.horometroProgramado,
                            fechaEjecutado = "",// x.fechaEjecutado.ToShortDateString(),
                            horometroEjecutado = 0, //  x.horometroEjecutado,
                            observacion = x.observacion
                        }).ToList()));

                        rd.SetParameterValue("pAdministradorMaquinaria", pAdministradorMaquinaria.usuario.nombre + " " + pAdministradorMaquinaria.usuario.apellidoPaterno);

                        rd.SetParameterValue("pProyecto", pProyecto);
                        rd.SetParameterValue("pGerenteProyecto", pGerenteProyecto.usuario.nombre + " " + pGerenteProyecto.usuario.apellidoPaterno);
                        rd.SetParameterValue("pPeriodoFechas", "Del " + pFechaInicioPm.ToShortDateString() + " AL " + pFechaFinPm.ToShortDateString());
                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.RPTCARATULAPRECIOS:
                    {
                        rd = new rptAutorizacionCaratula();
                        setMedidasReporte("NOMODAL");

                        int pCaratulaID = Convert.ToInt32(Request.QueryString["pCaratulaID"]);
                        int pIDAutoriza = Convert.ToInt32(Request.QueryString["pIDAutoriza"]);
                        var data = cfs.getConciliacionServices().loadAutorizacionCaratula(pIDAutoriza);

                        var listaCaratula = cfs.getConciliacionServices().getCaratulaByID(pCaratulaID);


                        var caratula = (tblAutorizaCaratulaDTO)Session["infoAutorizacionCaratula"];
                        var obra = cfs.getConciliacionServices().GetNameObra(caratula.obraID);
                        string pProyecto = obra;

                        string pNombElabora = caratula.usuarioElaboraNombre;
                        string pCadenaFirma = caratula.cadenaElabora;
                        string pNombreVobo1 = caratula.usuarioVobo1Nombre;
                        string pCadenaFirmaVobo1 = caratula.cadenaVobo1;
                        string pNombreVobo2 = caratula.usuarioVobo2Nombre;
                        string pCadenaVobo2 = caratula.cadenaVobo2;
                        string pNombreAutoriza1 = caratula.usuarioAutorizaNombre;
                        string pCadenaFirmaAutoriza = caratula.cadenaAutoriza;

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Direccion de Maquinaria y Equipo", "CARATULA CONCILIACION DE PRECIOS"));
                        rd.Database.Tables[1].SetDataSource(listaCaratula);

                        rd.SetParameterValue("pNombElabora", pNombElabora);
                        rd.SetParameterValue("pCadenaFirma", pCadenaFirma);

                        rd.SetParameterValue("pNombreVobo1", pNombreVobo1);
                        rd.SetParameterValue("pCadenaFirmaVobo1", pCadenaFirmaVobo1);
                        rd.SetParameterValue("pNombreVobo2", pNombreVobo2);
                        rd.SetParameterValue("pCadenaVobo2", pCadenaVobo2);
                        rd.SetParameterValue("pNombreAutoriza1", pNombreAutoriza1);
                        rd.SetParameterValue("pCadenaFirmaAutoriza", pCadenaFirmaAutoriza);

                        rd.SetParameterValue("pNombElabora", pNombElabora);
                        rd.SetParameterValue("pProyecto", pProyecto);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Prueba:
                    {
                        rd = new rptPrueba();

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", "Reporte de Insumos de Almacén"));
                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.CONCILIACIONHOROMETROS:
                    {
                        if (Convert.ToInt32(Request.QueryString["pTipoVista"]) == 1)
                        {
                            setMedidasReporte("HO");
                        }
                        else
                        {
                            setMedidasReporte("NOMODAL");
                        }


                        rd = new rptConciliacionHorometros();
                        int pConciliacionID = Convert.ToInt32(Request.QueryString["pConciliacionID"]);
                        var auth = cfs.getConciliacionServices().loadAutorizacionFromConciliacacionId(pConciliacionID);
                        var encConciliacion = cfs.getConciliacionServices().getCapEncConciliacion(pConciliacionID);
                        var conciliaciones = cfs.getConciliacionServices().getConciliaciones(pConciliacionID);
                        var gerente = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaGerenteID);
                        var admin = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaAdmin);
                        var director = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaDirector);
                        var centroCostos = centroCostosFactoryServices.getCentroCostosService().getEntityCCConstruplan(encConciliacion.centroCostosID);
                        var encCaratula = cfs.getConciliacionServices().getEncabezado(encConciliacion.centroCostosID);
                        var caratula = cfs.getConciliacionServices().getLstPrecios(encCaratula.id);
                        string moneda = "";


                        var sumaTotal = conciliaciones.Sum(x => x.total);

                        if (encCaratula != null)
                        {
                            moneda = encCaratula.moneda == 1 ? "M.N." : "USD";


                        }
                        DateTime fecha = DateTime.Now;
                        DateTime FechaSend = new DateTime(fecha.Year, 01, 01);
                        var Data = encConciliacion.esQuincena ? GetQuincenas().FirstOrDefault(x => x.Value == encConciliacion.fechaID) : GetFechas(FechaSend).FirstOrDefault(x => x.Value == encConciliacion.fechaID);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", "CONCILIACIÓN DE HOROMETROS"));
                        rd.Database.Tables[1].SetDataSource(conciliaciones.Select(c => new
                        {
                            no = c.numero,
                            economico = c.economico,
                            descripcion = c.descripcion,
                            hi = c.horometroInicial,
                            hf = c.horometroFinal,
                            he = c.horometroEfectivo,
                            unidad = c.unidad == 1 ? "HORAS" : "DÍA",
                            costoHora = c.costo,
                            costoTotal = c.total,
                            carga = EnumHelper.GetDescription((EmpresaEnum)c.idEmpresa).ToString(),
                            observaciones = c.observaciones

                        }).OrderBy(c => c.economico).ToList());
                        if (Data != null)
                        {
                            var ArraySplit = Data.Text.Split('-');
                            FechaSend = Convert.ToDateTime(ArraySplit[1]);
                            rd.SetParameterValue("FechaInicio", ArraySplit[0]);
                            rd.SetParameterValue("fechaFinal", ArraySplit[1]);
                        }
                        rd.SetParameterValue("pProyecto", centroCostos.cc + " " + centroCostos.descripcion);
                        rd.SetParameterValue("nombreGerente", string.Format("{0} {1} {2}", gerente.nombre, gerente.apellidoPaterno, gerente.apellidoMaterno));
                        rd.SetParameterValue("cadenaGerente", auth.firmaGerente);
                        rd.SetParameterValue("cadenaAdministrador", auth.firmaAdmin);
                        rd.SetParameterValue("nombreAdministrador", string.Format("{0} {1} {2}", admin.nombre, admin.apellidoPaterno, admin.apellidoMaterno));
                        rd.SetParameterValue("cadenaDirector", auth.firmaDirector);
                        rd.SetParameterValue("nombreDirector", string.Format("{0} {1} {2}", director.nombre, director.apellidoPaterno, director.apellidoMaterno));
                        rd.SetParameterValue("moneda", moneda);


                        Session.Add("reporte", rd);
                    }
                    break;

                case ReportesEnum.RPTDESERVICIO:
                    {
                        setMedidasReporte("HO");
                        rd = new rptServicioProgramado();
                        int idMant = Int32.Parse(Request.QueryString["fId"]);
                        tblM_ParamReport objmantenimiento = new tblM_ParamReport();

                        objmantenimiento = objMantenimientoFactory.getMantenimientoService().ConsultarMantenimientobyID(idMant);

                        var reporteProg = objMantenimientoFactory.getMantenimientoService().GetReporteProgramado(idMant);

                        var leyendas = reporteProg.leyendas.Where(x => !x.Contains("Inspección")).Select(x => new
                        {
                            leyenda = x
                        });

                        var leyendas2 = reporteProg.leyendas.Where(x => x.Contains("Inspección")).Select(x => new ReporteActExtDNsDTO
                        {
                            descripcion = x,
                            check = "x"
                        });

                        
                        

                        rd.Database.Tables[0].SetDataSource(reporteProg.miscelaneos);
                        rd.Database.Tables[1].SetDataSource(leyendas2);
                        rd.Database.Tables[2].SetDataSource(leyendas);
                        rd.Database.Tables[3].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Reporte de Servicio"));

                        rd.SetParameterValue("tipopm", objmantenimiento.tipopm ?? "");
                        rd.SetParameterValue("solicitado", reporteProg.enterado);
                        rd.SetParameterValue("modelo", objmantenimiento.modelo ?? "");
                        rd.SetParameterValue("economico", objmantenimiento.economico ?? "");
                        rd.SetParameterValue("fechaImpresion", DateTime.Today.Date.ToShortDateString());
                        rd.SetParameterValue("horometroActual", reporteProg.horometroActual);

                        rd.SetParameterValue("componentes1", reporteProg.componentes1);
                        rd.SetParameterValue("componentes2", reporteProg.componentes2);
                        rd.SetParameterValue("componentes3", reporteProg.componentes3);
                        rd.SetParameterValue("componentes4", reporteProg.componentes4);
                        rd.SetParameterValue("componentes5", reporteProg.componentes5);

                        rd.SetParameterValue("check1si", reporteProg.componentes1 != "" ? "x" : "");
                        rd.SetParameterValue("check1no", reporteProg.componentes1 != "" ? "" : "x");
                        rd.SetParameterValue("check2si", reporteProg.componentes2 != "" ? "x" : "");
                        rd.SetParameterValue("check2no", reporteProg.componentes2 != "" ? "" : "x");
                        rd.SetParameterValue("check3si", reporteProg.componentes3 != "" ? "x" : "");
                        rd.SetParameterValue("check3no", reporteProg.componentes3 != "" ? "" : "x");
                        rd.SetParameterValue("check4si", reporteProg.componentes4 != "" ? "x" : "");
                        rd.SetParameterValue("check4no", reporteProg.componentes4 != "" ? "" : "x");
                        rd.SetParameterValue("check5si", reporteProg.componentes5 != "" ? "x" : "");
                        rd.SetParameterValue("check5no", reporteProg.componentes5 != "" ? "" : "x");

                        //rd.SetParameterValue("realizo", reporteProg.realizo);
                        rd.SetParameterValue("realizo", reporteProg.realizo);
                        rd.SetParameterValue("comentarios", reporteProg.comentarios);

                        rd.SetParameterValue("enterado", reporteProg.enterado);
                        rd.SetParameterValue("fechaFooter", reporteProg.fechaFooter);

                        var A = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idMant);

                        var modeloID = objMantenimientoFactory.getMantenimientoService().ConsultaModelobyMantenimiento(idMant);

                        #region Actividades Lubricantes
                        var obj = objMantenimientoFactory.getMantenimientoService().ConsultarJGEstructura2(modeloID);
                        var objHist = objMantenimientoFactory.getMantenimientoService().ConsultarJGHis(idMant);
                        var objProy = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idMant);
                        var objComponentes = objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidades();
                        var objSuministros = obj.Select(x => x.aceiteDTO.Select(y => y.edadSuministro).Select(f => f));

                        var actividadesLubricantes = obj.Select(x => new
                        {
                            objComponente = x.componenteMantenimiento,
                            componente = x.descripcion,
                            Suministros = x.aceiteDTO.Where(y => y.componenteID == x.componenteMantenimiento.idCompVis),
                            TipoPrueba = "",
                            VidaUtil = "",
                            Info = "",
                            VidaConsumida = "",
                            VidaRestante = "",
                            Programar = "",
                            objHis = objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis),
                            proyectado = objProy.FirstOrDefault(p => p.idComp == x.componenteMantenimiento.idCompVis),
                            idComponente = x.aceiteDTO.FirstOrDefault().componenteID
                        });
                        #endregion

                        #region Actividades Extras
                        var objActividadesExtras = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtras(modeloID);
                        var objActividadesExtrashis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtrashis(idMant);
                        var objProyActExt = objMantenimientoFactory.getMantenimientoService().CargaDeAEProyectado(idMant);

                        var actividadesExtras = objActividadesExtras.Select(x => new
                        {
                            actividad = x.descripcion,
                            vidaUtil = x.perioricidad,
                            info = "",
                            vidaConsumida = "",
                            vidaRestante = "",
                            programar = "",
                            x.Componente,
                            x.descripcion,
                            x.id,
                            x.idAct,
                            x.idformato,
                            x.idTipo,
                            x.leyenda,
                            x.orden,
                            x.perioricidad,
                            x.PM,
                            x.Tipo,
                            hrsAplico = objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)).Hrsaplico,
                            proyectado = objProyActExt.FirstOrDefault(y => y.idAct == x.idAct)
                        });
                        #endregion

                        #region Actividades DN's
                        var objActividadesDN = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDN(modeloID, 1);
                        var objActividadesDNHis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDNhis(idMant);
                        var objDNProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeDNProyectado(idMant);

                        var actividadesDNs = objActividadesDN.Select(x => new
                        {
                            actividad = x.descripcion,
                            vidaUtil = x.perioricidad,
                            info = "",
                            vidaConsumida = "",
                            vidaRestante = "",
                            programar = "",
                            proyectado = objDNProyectado.FirstOrDefault(y => y.idAct == x.idAct),
                            Hrsaplico = objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)).Hrsaplico
                        });
                        #endregion



                        Session.Add("reporte", rd);
                    }
                    break;
                case ReportesEnum.RPTINSUMOSCONSULTA:
                    {
                        rd = new rptListadoInsumos();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        //var sendData = (List<insumosDTO>)Session["rptInsumosConstruplanArrendadora"];

                        string pInsumoC = Request.QueryString["pInsumoC"];
                        string pDescripcionC = Request.QueryString["pDescripcionC"];
                        decimal pCantidadConstruplan = Convert.ToDecimal(Request.QueryString["pCantidadConstruplan"]);
                        decimal pCantidadArrendadora = Convert.ToDecimal(Request.QueryString["pCantidadArrendadora"]);

                        decimal Suma = pCantidadArrendadora + pCantidadConstruplan;

                        var DataConstruplan = (List<rptInsumosDTO>)Session["rptInsumosConstruplan"];


                        var DataArrendadora = (List<rptInsumosDTO>)Session["rptInsumosArrendadora"];

                        rd.Database.Tables[0].SetDataSource(DataConstruplan);
                        rd.Database.Tables[1].SetDataSource(DataArrendadora);
                        rd.Database.Tables[2].SetDataSource(getInfoEnca("", "Reporte de Insumos de Almacén"));


                        rd.SetParameterValue("pInsumoC", pInsumoC);
                        rd.SetParameterValue("pDescripcionC", pDescripcionC);
                        rd.SetParameterValue("pSuma", Suma);
                        rd.SetParameterValue("pCantidadConstruplan", pCantidadConstruplan);
                        rd.SetParameterValue("pCantidadArrendadora", pCantidadArrendadora);


                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.RPTHORASHOMBREEQUIPO:
                    {
                        rd = new rptHorasHombreMaquina();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        var sendData = Session["HoraHombreENmaquinaria"];

                        string pFechaInicio1 = Request.QueryString["pFechainicio"];
                        string pFechaFin1 = Request.QueryString["pFechaFin"];
                        string cc = Request.QueryString["pCC"];

                        string nombreCentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCArrendadoraRH(cc);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Dirección de Maquinaria y Equipo"));
                        rd.Database.Tables[1].SetDataSource(sendData);


                        rd.SetParameterValue("pFechaInicio", pFechaInicio1);
                        rd.SetParameterValue("pFechaFin", pFechaFin1);
                        rd.SetParameterValue("pCC", cc + "- " + nombreCentroCostos);
                        rd.SetParameterValue("hCC", setTitleCC());


                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.CONCENTRADOHHPDETALLE:
                    {
                        rd = new rptPuestosGenerales();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        var sendData = Session["rptDetalleGeneralPorPuesto"];

                        var periodo = Request.QueryString["pPerioriodo"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "REPORTE CONCENTRADO POR PUESTO."));
                        rd.Database.Tables[1].SetDataSource(sendData);


                        rd.SetParameterValue("Periodo", "Periodo : " + periodo);
                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.UTLIZACION:
                    {
                        rd = new rptUtilizacionDetallePuesto();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        var sendData = Session["DetallePersonaDT"];
                        var puestoID = Request.QueryString["pPuestoID"];

                        var Puesto = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getListaPuestos().FirstOrDefault(x => x.Value == puestoID);

                        var periodo = Request.QueryString["pPerioriodo"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "REPORTE UTILIZACION"));
                        rd.Database.Tables[1].SetDataSource(sendData);

                        rd.SetParameterValue("puesto", Puesto != null ? Puesto.Text : "");
                        rd.SetParameterValue("Periodo", "Periodo : " + periodo);
                        Session.Add("reporte", rd);


                    }
                    break;
                case ReportesEnum.CONCENTRADOHHPUESTOS:
                    {
                        rd = new verReportePuestosDistribucion();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        var sendData = Session["ConcentradoGeneral"];

                        var puestoID = Request.QueryString["pPuestoID"];

                        var Puesto = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getListaPuestos().FirstOrDefault(x => x.Value == puestoID);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "REPORTE DISTRIBUCION POR PUESTO"));
                        rd.Database.Tables[1].SetDataSource(sendData);

                        var periodo = Request.QueryString["pPerioriodo"];
                        rd.SetParameterValue("Periodo", "Periodo : " + periodo);
                        rd.SetParameterValue("puesto", Puesto != null ? Puesto.Text : "");
                        Session.Add("reporte", rd);

                    }
                    break;

                case ReportesEnum.CONCENTRADOHHEMPLEADO:
                    {
                        rd = new rptConcentradoHHEmpleado();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        var sendData = Session["DetallePersonaDTSession"];

                        string pPuesto = Request.QueryString["pPuesto"];
                        string pNombre = Request.QueryString["pNombre"];
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.Database.Tables[1].SetDataSource(sendData);

                        rd.SetParameterValue("pNombre", pNombre);
                        rd.SetParameterValue("pPuesto", pPuesto);

                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.EvaluacionaSubContratistas:
                    {
                        rd = new rptEvaluacionSubContratista();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        List<InfoEncabezadoEvaluacion> EncabezadoReproteListo = new List<InfoEncabezadoEvaluacion>();
                        var rEncuestaID = Convert.ToInt16(Request.QueryString["rEncuestaID"]);


                        var Data = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetEncuestaContestada(rEncuestaID);
                        var DataInfoEncabezado = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetDetalleEncuesta(rEncuestaID);

                        var usuariop = usuarioFactoryServices.getUsuarioService().ListUsersById(DataInfoEncabezado.evaluador).FirstOrDefault();
                        rd.Database.Tables[1].SetDataSource(GetRespuestas(Data));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        string servicioContratado = DataInfoEncabezado.descripcionServicio;
                        string comentario = DataInfoEncabezado.comentarios != null ? DataInfoEncabezado.comentarios : "";
                        rd.SetParameterValue("fechaEvaluacion", Data.FirstOrDefault().fecha.Date.ToShortDateString());
                        rd.SetParameterValue("nombreContratista", DataInfoEncabezado.nombreSubContratista);
                        rd.SetParameterValue("noContrato", DataInfoEncabezado.noContrato != null ? DataInfoEncabezado.noContrato : "");
                        rd.SetParameterValue("servicioContratado", servicioContratado);
                        rd.SetParameterValue("nombreProyecto", DataInfoEncabezado.nombreProyecto);
                        rd.SetParameterValue("evaluador", usuariop != null ? usuariop.nombre + " " + usuariop.apellidoPaterno : "");
                        rd.SetParameterValue("comentario", comentario);

                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.EvaluacionProveedoresServicio:
                    {
                        rd = new rptEvaludacionProveedoresServicio();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        List<InfoEncabezadoEvaluacion> EncabezadoReproteListo = new List<InfoEncabezadoEvaluacion>();
                        var pencuestaProveedorDet = Convert.ToInt16(Request.QueryString["encuestaProveedorDet"]);
                        var ptipo = Convert.ToInt16(Request.QueryString["tipo"]);

                        var Data = encuestasProveedoresFactoryServices.getEncuestasProveedoresFactoryServices().GetEncuestaContestada(pencuestaProveedorDet, ptipo);
                        var DataInfoEncabezado = encuestasProveedoresFactoryServices.getEncuestasProveedoresFactoryServices().GetEncabezadoEncuesta(pencuestaProveedorDet, ptipo);

                        rd.Database.Tables[1].SetDataSource(GetRespuestas(Data));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.SetParameterValue("Comentarios", DataInfoEncabezado.comentarios);


                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.EncuestaEvaluacionContinuaProveedor:
                    {
                        rd = new rptEvaluacionContinuaProveedores();
                        InfoEncabezadoEvaluacion EncabezadoReprote = new InfoEncabezadoEvaluacion();
                        List<InfoEncabezadoEvaluacion> EncabezadoReproteListo = new List<InfoEncabezadoEvaluacion>();
                        var pencuestaProveedorDet = Convert.ToInt16(Request.QueryString["encuestaProveedorDet"]);
                        var ptipo = Convert.ToInt16(Request.QueryString["tipo"]);
                        var Data = encuestasProveedoresFactoryServices.getEncuestasProveedoresFactoryServices().GetEncuestaContestada(pencuestaProveedorDet, ptipo);
                        var DataInfoEncabezado = encuestasProveedoresFactoryServices.getEncuestasProveedoresFactoryServices().GetEncabezadoEncuesta(pencuestaProveedorDet, ptipo);

                        var FechaEvaluacion = encuestasProveedoresFactoryServices.getEncuestasProveedoresFactoryServices().getEncuestaByFolioIDOC(DataInfoEncabezado.id);


                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.Database.Tables[1].SetDataSource(GetRespuestas(Data));

                        //  rd.Database.Tables[2].SetDataSource(ToDataTable(EncabezadoReproteListo.ToList()));



                        EncabezadoReprote.antiguedadDelProveedor = DataInfoEncabezado.fechaAntiguedad.ToShortDateString();
                        EncabezadoReprote.comentario = DataInfoEncabezado.comentarios != null ? DataInfoEncabezado.comentarios : "";
                        EncabezadoReprote.evaluador = DataInfoEncabezado.centrocostos;
                        EncabezadoReprote.fechaEvaluacion = DataInfoEncabezado.fechaOC != null ? DataInfoEncabezado.fechaOC.Value.ToShortDateString() : DataInfoEncabezado.fechaEvaluacion.Value.ToShortDateString();
                        EncabezadoReprote.firmaEvaluador = "";
                        EncabezadoReprote.nombreProveedor = DataInfoEncabezado.nombreProveedor;
                        EncabezadoReprote.tipodeProveedor = DataInfoEncabezado.tipoProveedor != null ? DataInfoEncabezado.tipoProveedor : "";

                        //rd.SetParameterValue("pPersonalDetalle", ptituloGrafica);
                        rd.SetParameterValue("antiguedadDelProveedor", DataInfoEncabezado.fechaAntiguedad.ToShortDateString());
                        rd.SetParameterValue("comentario", EncabezadoReprote.comentario);
                        var nombreEvaluador = usuarioFactoryServices.getUsuarioService().ListUsersById(DataInfoEncabezado.evaluadorID).FirstOrDefault();

                        rd.SetParameterValue("evaluador", nombreEvaluador != null ? nombreEvaluador.nombre + " " + nombreEvaluador.apellidoPaterno + " " + nombreEvaluador.apellidoMaterno : "");

                        rd.SetParameterValue("fechaEvaluacion", FechaEvaluacion);
                        rd.SetParameterValue("firmaEvaluador", "");
                        rd.SetParameterValue("tipodeProveedor", DataInfoEncabezado.tipoMoneda);
                        rd.SetParameterValue("nombreProveedor", DataInfoEncabezado.nombreProveedor);
                        rd.SetParameterValue("ubicacionProveedor", DataInfoEncabezado.ubicacionProveedor);

                        Session.Add("reporte", rd);

                    }
                    break;
                case ReportesEnum.ReporteMantenimientosFrecuenciaparo:
                    {
                        rd = new rptMantenimientoFrecuencia();
                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];
                        var ptituloGrafica = Request.QueryString["ptituloGrafica"];

                        var ListaGraficas = (List<string>)Session["base64FileGraficaList"];
                        DataTable tableEncabezado = new DataTable();

                        tableEncabezado.Columns.Add("Grafica01", System.Type.GetType("System.Byte[]"));
                        tableEncabezado.Columns.Add("Grafica02", System.Type.GetType("System.Byte[]"));

                        string resultIMG1 = ListaGraficas[0].Replace("data:image/png;base64,", "");
                        string resultIMG2 = ListaGraficas[1].Replace("data:image/png;base64,", "");

                        byte[] bytes1 = System.Convert.FromBase64String(resultIMG1);
                        byte[] bytes2 = System.Convert.FromBase64String(resultIMG2);

                        DataRow dr = tableEncabezado.NewRow();
                        tableEncabezado.Rows.Add(dr);
                        tableEncabezado.Rows[0]["Grafica01"] = bytes1;
                        tableEncabezado.Rows[0]["Grafica02"] = bytes2;

                        //    var listaRes = Session["generalManttoSession"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.Database.Tables[1].SetDataSource(tableEncabezado);
                        //   rd.Database.Tables[2].SetDataSource(listaRes);//bien
                        //rd.SetParameterValue("pPersonalDetalle", ptituloGrafica);
                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.GeneralHorasHombre:
                    {
                        rd = new rptGeneralHorasHombre();
                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];
                        var ptituloGrafica = Request.QueryString["ptituloGrafica"];

                        var ListaGraficas = (List<string>)Session["base64FileGraficaList"];
                        DataTable tableEncabezado = new DataTable();



                        tableEncabezado.Columns.Add("Grafica01", System.Type.GetType("System.Byte[]"));
                        tableEncabezado.Columns.Add("Grafica02", System.Type.GetType("System.Byte[]"));
                        tableEncabezado.Columns.Add("Grafica03", System.Type.GetType("System.Byte[]"));


                        string resultIMG1 = ListaGraficas[0].Replace("data:image/png;base64,", "");
                        string resultIMG2 = ListaGraficas[1].Replace("data:image/png;base64,", "");
                        // string resultIMG3 = ListaGraficas[2].Replace("data:image/png;base64,", "");

                        byte[] bytes1 = System.Convert.FromBase64String(resultIMG1);
                        byte[] bytes2 = System.Convert.FromBase64String(resultIMG2);
                        // byte[] bytes3 = System.Convert.FromBase64String(resultIMG3);



                        DataRow dr = tableEncabezado.NewRow();
                        tableEncabezado.Rows.Add(dr);
                        tableEncabezado.Rows[0]["Grafica01"] = bytes1;
                        tableEncabezado.Rows[0]["Grafica02"] = bytes2;
                        // tableEncabezado.Rows[0]["Grafica03"] = bytes3;

                        var listaRes = Session["reshorashombreDTO"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.Database.Tables[1].SetDataSource(tableEncabezado);
                        rd.Database.Tables[2].SetDataSource(listaRes);//bien
                        //rd.SetParameterValue("pPersonalDetalle", ptituloGrafica);
                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.PersonalDetallePuesto:
                    {
                        rd = new rptDetallePersonal();
                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];
                        var ptituloGrafica = Request.QueryString["ptituloGrafica"];

                        var listDetallePersonalDTO = ((List<tblDetallePersonalDTO>)Session["GetOTEmpleado"]).Select(x => new
                        {
                            Folio = x.folio,
                            noEconomico = x.economico,
                            MotivoParo = x.motivoParo,
                            HoraEntrada = x.inicioParo,
                            HoraSalida = x.finParo
                        }).ToList();

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ""));
                        rd.Database.Tables[1].SetDataSource(listDetallePersonalDTO.ToList());//bien
                        rd.SetParameterValue("pPersonalDetalle", ptituloGrafica);
                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);

                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.PuestoDetalle:
                    {
                        rd = new rptPuestoFrecuencias();
                        string stringInBase64 = "";
                        DataTable tableEncabezado = new DataTable();

                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];
                        var ptituloGrafica = Request.QueryString["ptituloGrafica"];

                        stringInBase64 = Session["base64FileGrafica"].ToString();
                        tableEncabezado.Columns.Add("IMGDATA", System.Type.GetType("System.Byte[]"));
                        string resultIMG = stringInBase64.Replace("data:image/png;base64,", "");
                        byte[] bytes = System.Convert.FromBase64String(resultIMG);

                        tableEncabezado.Rows.Add(bytes);

                        var dtDetalle = ((List<tblHorasHombreDetDTO>)Session["rpttblHorasHombreDetDTO"]).Select(x => new DetallePuestoDTO
                        {
                            nombrePersonal = x.personalNombre,
                            hrsPreventivo = x.hrasPreventivo,
                            hrsPredictivo = x.hrasPredictivo,
                            hrsCorrectivo = x.hrasCorrectivo,
                            cantidadOT = x.cantidadOT,
                            totalhrsOT = x.promedioHrasOT

                        });

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ptituloGrafica));

                        rd.Database.Tables[2].SetDataSource(tableEncabezado);//bien
                        rd.Database.Tables[1].SetDataSource(dtDetalle.ToList());//bien

                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.impresion:
                    {
                        rd = new rptGraficasFrecuenciaParos();
                        string stringInBase64 = "";
                        DataTable tableEncabezado = new DataTable();

                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];
                        var ptituloGrafica = Request.QueryString["ptituloGrafica"];

                        stringInBase64 = Session["base64FileGrafica"].ToString();
                        tableEncabezado.Columns.Add("IMGDATA", System.Type.GetType("System.Byte[]"));
                        string resultIMG = stringInBase64.Replace("data:image/png;base64,", "");
                        byte[] bytes = System.Convert.FromBase64String(resultIMG);

                        tableEncabezado.Rows.Add(bytes);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", ptituloGrafica));
                        rd.Database.Tables[1].SetDataSource(tableEncabezado);//bien


                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.DetalleFrecuenciaParos:
                    {
                        setMedidasReporte("HO");
                        rd = new rptDetFrecuenciaParo();

                        var data = (List<detFrecuenciaParoDTO>)Session["rptdetFrecuenciaParoDTO"];
                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Reporte detalle de Horas hombre"));//bien

                        rd.Database.Tables[1].SetDataSource(data);//bien
                        /* rd.SetParameterValue("pFIncio", pFIncio);
                         rd.SetParameterValue("pFFin", pFFin);
                         rd.SetParameterValue("pMotivoParo", pMotivoParo);
                         rd.SetParameterValue("pCodicionParo", pCodicionParo);
                         rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                         rd.SetParameterValue("pEconomico1", pEconomico1);
                         rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                         rd.SetParameterValue("pTipoParo", pTipoParo);*/
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.DetalleHorasHombre:
                    {
                        setMedidasReporte("HO");
                        rd = new rptHorasHombre();

                        var data = (List<tblHorasHombreDetDTO>)Session["rpttblHorasHombreDetDTO"];
                        var pFIncio = Request.QueryString["pFIncio"];
                        var pFFin = Request.QueryString["pFFin"];
                        var pTipoParo = Request.QueryString["pTipoParo"];
                        var pMotivoParo = Request.QueryString["pMotivoParo"];
                        var pCodicionParo = Request.QueryString["pCodicionParo"];
                        var pGrupoMaquinaria = Request.QueryString["pGrupoMaquinaria"];
                        var pEconomico1 = Request.QueryString["pEconomico"];
                        var pCodicionEquipo = Request.QueryString["pCodicionEquipo"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Reporte detalle de Horas hombre"));//bien

                        rd.Database.Tables[1].SetDataSource(data);//bien
                        rd.SetParameterValue("pFIncio", pFIncio);
                        rd.SetParameterValue("pFFin", pFFin);
                        rd.SetParameterValue("pMotivoParo", pMotivoParo);
                        rd.SetParameterValue("pCodicionParo", pCodicionParo);
                        rd.SetParameterValue("pGrupoMaquinaria", pGrupoMaquinaria);
                        rd.SetParameterValue("pEconomico1", pEconomico1);
                        rd.SetParameterValue("pCodicionEquipo", pCodicionEquipo);
                        rd.SetParameterValue("pTipoParo", pTipoParo);
                        rd.SetParameterValue("hEco", setTitleEco());

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.pendientessustitucion:
                    {
                        setMedidasReporte("HO");
                        rd = new rptEquiposPendientesAsignacion();
                        var datos1 = (List<EquiposPendientesDTO>)Session["rptEquiposPendientesDTO"];
                        var datos2 = (List<EquiposPendientesReemplazoDTO>)Session["rptEquiposPendientesReemplazoDTO"];
                        rd.Database.Tables[1].SetDataSource(datos1);
                        rd.Database.Tables[2].SetDataSource(datos2);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección general de Maquinaria y Equipo", "Reporte de tiempos solicitudes"));
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.TiemposAsignacion:
                    {
                        setMedidasReporte("HO");
                        rd = new rptTiemposAsignacion();
                        var datos = (List<rptTiemposEntreAutorizaciones>)Session["rptTiemposAutorizacion"];
                        string pPeriodo = Request.QueryString["pPeriodo"];

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Reporte de tiempos de autorización de solicitudes de equipo"));
                        rd.Database.Tables[1].SetDataSource(datos);

                        rd.SetParameterValue("pPeriodo", pPeriodo);

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.CONTROLINTERNOMAQUINARIA:
                    {
                        setMedidasReporte("HorizontalCarta_NoModal");
                        rd = new rptMovimientosControlInterno();
                        int idControlInterno = Convert.ToInt32(Request.QueryString["idControlInterno"]);
                        rptControlInternoMaquinariaDTO data = new rptControlInternoMaquinariaDTO();
                        List<rptControlInternoMaquinariaDTO> datos = new List<rptControlInternoMaquinariaDTO>();
                        string nombreRecibe = "";
                        string nombreEnvia = "";
                        string nombreEnterado = "";

                        string CadenaEnvia = "";
                        string CadenaRecibe = "";
                        string CadenaEnterado = "";
                        var Autorizadores = autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GetAutorizadores(idControlInterno);


                        if (Autorizadores != null)
                        {
                            var GetControlInterno = Autorizadores.ControMovimientoInterno;
                            var GetEconomico = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GetDataEconomicoID(GetControlInterno.EconomicoID);

                            data.Baterias = GetControlInterno.Bateria;
                            data.Combustible = GetControlInterno.Combustible;
                            data.Observaciones = GetControlInterno.Comentario;
                            data.noEconomico = GetEconomico.noEconomico;
                            data.Envio = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(GetControlInterno.Envio);
                            data.Destino = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(GetControlInterno.Destino); //GetControlInterno.Destino == 1 ? "PATIO MAQUINARIA HERMOSILLO" : "TALLER MECANICO CENTRAL"; //"TALLER MECANICO CENTRAL" : "PATIO MAQUINARIA HERMOSILLO";
                            data.Horometro = GetControlInterno.Horometro.ToString();
                            data.Marca = GetEconomico.marca.descripcion;
                            data.Folio = GetControlInterno.Folio;
                            data.Marca1 = GetControlInterno.Marca2;
                            data.Modelo = GetEconomico.modeloEquipo.descripcion;
                            data.Registro = GetControlInterno.Registro;
                            data.Serie = GetEconomico.noSerie;
                            data.Serie1 = GetControlInterno.Serie2;
                            data.Fecha = GetControlInterno.FechaCaptura.ToShortDateString();
                            data.Estatus = GetControlInterno.Estatus.ToString();

                            var UsuarioRecibe = usuarioFactoryServices.getUsuarioService().ListUsersById(Autorizadores.usuarioRecibe).FirstOrDefault();
                            var UsuarioEnvia = usuarioFactoryServices.getUsuarioService().ListUsersById(Autorizadores.usuarioEnvio).FirstOrDefault();
                            var UsuarioEnterado = usuarioFactoryServices.getUsuarioService().ListUsersById(Autorizadores.usuarioValida).FirstOrDefault();



                            if (UsuarioRecibe != null)
                            {
                                nombreRecibe = UsuarioRecibe.nombre + " " + UsuarioRecibe.apellidoPaterno + " " + UsuarioRecibe.apellidoMaterno;
                                CadenaRecibe = Autorizadores.cadenafirmaRecibe;

                            }
                            if (UsuarioEnvia != null)
                            {
                                nombreEnvia = UsuarioEnvia.nombre + " " + UsuarioEnvia.apellidoPaterno + " " + UsuarioEnvia.apellidoMaterno;
                                CadenaEnvia = Autorizadores.cadenafirmaEnvia;
                            }
                            if (UsuarioEnterado != null)
                            {
                                nombreEnterado = UsuarioEnterado.nombre + " " + UsuarioEnterado.apellidoPaterno + " " + UsuarioEnterado.apellidoMaterno;
                                CadenaEnterado = Autorizadores.cadenafirmaEnterado;

                            }


                            datos.Add(data);
                        }
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección general de Maquinaria y Equipo", "Inventario General de Maquinaria y Equipo"));
                        rd.Database.Tables[1].SetDataSource(datos);

                        rd.SetParameterValue("UsuarioRecibe", nombreRecibe);
                        rd.SetParameterValue("UsuarioEnvia", nombreEnvia);
                        rd.SetParameterValue("UsuarioEnterado", nombreEnterado);

                        rd.SetParameterValue("CadenaEnvia", CadenaEnvia);
                        rd.SetParameterValue("CadenaRecibe", CadenaRecibe);
                        rd.SetParameterValue("CadenaEnterado", CadenaEnterado);
                        rd.SetParameterValue("hEco", setTitleEco());

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.PreviewStandby:
                    {
                        int medidas = Convert.ToInt32(Request.QueryString["size"]);
                        if (medidas == 1)
                        {
                            setMedidasReporte("HorizontalCarta_NoModal");
                        }
                        else
                        {
                            setMedidasReporte("HO");
                        }

                        setMedidasReporte("HO");
                        rd = new rptConciliacionSemanal();

                        int pID = Convert.ToInt32(Request.QueryString["pID"]);

                        var data = standbyFactoryServices.getStandbyFactoryServices().getStandByID(pID);
                        var result = standbyDetFactoryServices.getStandbyDetFactoryServices().getListaDetStandBy(pID);
                        var Autorizadores = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacionByIDStanby(pID);
                        //     var data = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetReporte(CC, FechaInicio, FechaFin).ToList();

                        string CentroCostosNombre = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(data.CC);
                        var diaInicio = data.FechaInicio.Day;
                        var diaFin = data.FechaFin.Day;
                        var Mes = data.FechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        var mes2 = data.FechaFin.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        var year = data.FechaFin.Year;

                        string descripcionFechas = "Relación de maquinaria que aplicará stand by, en la semana del " + data.FechaInicio.Day + " de " + Mes + " de " + data.FechaFin.Year + " al " + data.FechaFin.Day + " de " + mes2 + " de " + year;


                        List<rptConciliacionDTO> rptConciliacionDTOList = new List<rptConciliacionDTO>();
                        foreach (var item in result)
                        {
                            rptConciliacionDTO rptConciliacionOBJ = new rptConciliacionDTO();

                            var economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(item.noEconomicoID).FirstOrDefault();

                            rptConciliacionOBJ.DiaParo = item.DiaParo.ToShortDateString();
                            rptConciliacionOBJ.Economico = economico.noEconomico;
                            rptConciliacionOBJ.Descripcion = economico.descripcion;
                            rptConciliacionOBJ.HorometroInicio = item.HorometroInicial.ToString();
                            rptConciliacionOBJ.HorometroFinal = item.HorometroFinal.ToString();
                            rptConciliacionOBJ.TipoConsideracion = getTipo(item.TipoConsideracion);


                            rptConciliacionDTOList.Add(rptConciliacionOBJ);
                        }
                        var elabora1 = usuarioFactoryServices.getUsuarioService().ListUsersById(data.UsuarioElabora).FirstOrDefault();
                        var elabora2 = usuarioFactoryServices.getUsuarioService().ListUsersById(data.UsuarioGerente).FirstOrDefault();
                        string CadenaElabora1 = "";
                        string CadenaElabora2 = "";

                        if (Autorizadores != null)
                        {
                            CadenaElabora1 = Autorizadores.CadenaElabora;
                            CadenaElabora2 = Autorizadores.CadenaGerente;
                        }


                        StandbyParmDTO.Descripcion = descripcionFechas;
                        StandbyParmDTO.Centro_Costos = CentroCostosNombre;
                        StandbyParmDTO.elabora1 = elabora1.nombre + " " + elabora1.apellidoPaterno + " " + elabora1.apellidoMaterno;
                        StandbyParmDTO.elabora2 = elabora2.nombre + " " + elabora2.apellidoPaterno + " " + elabora2.apellidoMaterno;

                        rd.Database.Tables[1].SetDataSource(rptConciliacionDTOList);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", ""));
                        Session.Add("reporte", rd);
                        rd.SetParameterValue("Descripcion", descripcionFechas);
                        rd.SetParameterValue("Centro_Costos", CentroCostosNombre);

                        rd.SetParameterValue("CadenaElabora1", CadenaElabora1);
                        rd.SetParameterValue("CadenaElabora2", CadenaElabora2);

                        rd.SetParameterValue("elabora1", elabora1.nombre + " " + elabora1.apellidoPaterno + " " + elabora1.apellidoMaterno);
                        rd.SetParameterValue("elabora2", elabora2.nombre + " " + elabora2.apellidoPaterno + " " + elabora2.apellidoMaterno);
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("hCC", setTitleCC());
                        break;
                    }
                case ReportesEnum.OT:
                    {
                        setMedidasReporte("HO");
                        rd = new rptOrdenTrabajo();

                        int idOT = Convert.ToInt32(Request.QueryString["idOT"]);
                        OrdenTrabajoDTO objOT = new OrdenTrabajoDTO();
                        List<OrdenTrabajoDTO> listOtobj = new List<OrdenTrabajoDTO>();
                        List<dtsMotivosParoDTO> dtsMotivosParoDTO = new List<dtsMotivosParoDTO>();


                        var objOTServer = capturaOTFactoryServices.getCapturaOTFactoryServices().GetOTbyID(idOT);
                        var objOTDetServer = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().getListaOTDet(idOT);
                        var objMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(objOTServer.EconomicoID).FirstOrDefault();

                        var HoraEntrada = objOTServer.FechaEntrada.ToString("hh:mm tt");

                        var horaS = ((DateTime)objOTServer.FechaSalida);

                        var horaSalida = horaS == null ? "" : horaS.ToString("hh:mm tt");

                        objOT.Comentarios = objOTServer.Comentario;
                        objOT.DescripcionTM = objOTServer.DescripcionTiempoMuerto;
                        objOT.Economico = objMaquina.noEconomico;
                        objOT.Fecha = objOTServer.FechaCreacion.ToShortDateString();
                        objOT.HoraEntrada = HoraEntrada;
                        objOT.HoraSalida = horaSalida;
                        var usuarioRealizo = usuarioFactoryServices.getUsuarioService().ListUsersById(objOTServer.usuarioCapturaID).FirstOrDefault();

                        var usuarioEnterado = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1, objOTServer.CC).FirstOrDefault();

                        string Enterado = "";
                        string Realizo = "";
                        if (usuarioEnterado != null)
                        {
                            var tempuser = usuarioFactoryServices.getUsuarioService().ListUsersById(objOTServer.usuarioCapturaID).FirstOrDefault();

                            if (tempuser != null)
                            {
                                Enterado = tempuser.nombre + " " + tempuser.apellidoPaterno + " " + tempuser.apellidoMaterno; // + " "
                            }
                        }
                        if (Realizo != null)
                        {
                            Realizo = usuarioRealizo.nombre + " " + usuarioRealizo.apellidoPaterno + " " + usuarioRealizo.apellidoMaterno; // + " "
                        }

                        var GetInfoParo = motivosParoFactoryServices.getMotivosParoFactoryServices().getMotivosParo(objOTServer.MotivoParo);

                        if (GetInfoParo != null)
                        {
                            dtsMotivosParoDTO.Add(infoMotivosParo(GetInfoParo.id, GetInfoParo.TiempoMantenimiento, GetInfoParo.TipoParo, objOTServer.TipoParo3.ToString()));
                        }



                        objOT.Horometro = objOTServer.horometro;
                        objOT.Modelo = objMaquina.modeloEquipo.descripcion;
                        objOT.MotivoParo = GetInfoParo != null ? GetInfoParo.id.ToString() : "";
                        objOT.Obra = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(objOTServer.CC);
                        objOT.TiempoMuerto = objOTServer.TiempoMuerto.ToString();
                        objOT.TiempoReparacion = objOTServer.TiempoReparacion.ToString();
                        objOT.TiempoTotal = objOTServer.TiempoTotalParo.ToString();
                        objOT.TipoParo1 = objOTServer.TipoParo1.ToString();
                        objOT.TipoParo2 = objOTServer.TipoParo2.ToString();
                        objOT.TipoParo3 = objOTServer.TipoParo3.ToString();
                        objOT.Turno = objOTServer.Turno.ToString();
                        //objOT.MotivoParo = "1";
                        listOtobj.Add(objOT);
                        rd.Database.Tables[1].SetDataSource(listOtobj);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección general de Maquinaria y Equipo", "Orden de Trabajo"));
                        var det = new List<PersonalOTDTO>();

                        if (objOTDetServer != null)
                        {

                            foreach (var x in objOTDetServer)
                            {
                                PersonalOTDTO objOTDTO = new PersonalOTDTO();
                                var catEmpleado = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().getCatEmpleados(x.PersonalID.ToString());

                                if (catEmpleado != null)
                                {
                                    objOTDTO.Nombre = catEmpleado.Nombre;
                                    objOTDTO.Puesto = catEmpleado.Puesto;
                                    objOTDTO.Horas = (x.HoraFin - x.HoraInicio).ToString().Substring(0, 5);

                                    det.Add(objOTDTO);
                                }

                            }
                        }
                        var folio = "";
                        folio = idOT.ToString().PadLeft(6, '0');
                        rd.Database.Tables[2].SetDataSource(det);
                        rd.Database.Tables[3].SetDataSource(dtsMotivosParoDTO);


                        rd.SetParameterValue("Folio", folio);
                        rd.SetParameterValue("H1", objOTServer.TiempoHorasTotal);
                        rd.SetParameterValue("H2", objOTServer.TiempoHorasReparacion);
                        rd.SetParameterValue("H3", objOTServer.TiempoHorasMuerto);
                        rd.SetParameterValue("M1", objOTServer.TiempoMinutosTotal);
                        rd.SetParameterValue("M2", objOTServer.TiempoMinutosReparacion);
                        rd.SetParameterValue("M3", objOTServer.TiempoMinutosMuerto);
                        rd.SetParameterValue("Enterado", Enterado);
                        rd.SetParameterValue("Realizo", Realizo);
                        rd.SetParameterValue("FechaCaptura", objOTServer.FechaSalida.ToString());
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.CHECKLISTRESGUARDOS:
                    {
                        int medidas = Convert.ToInt32(Request.QueryString["size"]);
                        if (medidas == 1)
                        {
                            setMedidasReporte("HorizontalCarta_NoModal");
                        }
                        else
                        {
                            setMedidasReporte("HO");
                        }
                        rd = new rptcheckListResguardo();
                        int idResguardo = Convert.ToInt32(Request.QueryString["idReguardo"]);

                        var RespuestasResguardo = respuestaResguardoVehiculosFactoryServices.getRespuestaResguardoVehiculosServices().GetResguardoRespuestas(idResguardo);
                        var RespuestasResguardoDocumentos = respuestaResguardoVehiculosFactoryServices.getRespuestaResguardoVehiculosServices().getDocumentos(idResguardo);
                        var objResguardoVehiculo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(idResguardo);



                        rd.Database.Tables[0].SetDataSource(RespuestasResguardo);
                        rd.Database.Tables[1].SetDataSource(RespuestasResguardoDocumentos);
                        //   rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección general de Maquinaria y Equipo", "Inventario General de Maquinaria y Equipo"));
                        rd.SetParameterValue("Comentario", objResguardoVehiculo.Comentario);

                        string FechaVersionInforme = objResguardoVehiculo.Fecha.Date >= new DateTime(2018, 07, 01) ? "Ver. 1, 01-08-2018" : "Ver. 2,20-02-2017"; //"Ver. 1, 01-08-2018";

                        rd.SetParameterValue("VersionInforme", FechaVersionInforme);

                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.INVENTARIOGENERALMAQUINARIA:
                    {
                        setMedidasReporte("HO");
                        rd = new rptInventarioGeneral();

                        List<inventarioGeneralDTO> objAutorizaionResguardo = (List<inventarioGeneralDTO>)Session["GetListaInventario"];

                        rd.Database.Tables[1].SetDataSource(objAutorizaionResguardo.OrderBy(x => x.Economico));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección general de Maquinaria y Equipo", "Inventario General de Maquinaria y Equipo"));
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ASIGNACIONDEVEHICULOS:
                    {
                        int medidas = Convert.ToInt32(Request.QueryString["size"]);
                        if (medidas == 1)
                        {
                            setMedidasReporte("HorizontalCarta_NoModal");
                        }
                        else
                        {
                            setMedidasReporte("HO");
                        }

                        rd = new ResguardoVehiculos();
                        List<AutorizacionResguardoDTO> datos = new List<AutorizacionResguardoDTO>();
                        List<AutorizantesDTO> datos2 = new List<AutorizantesDTO>();


                        AutorizacionResguardoDTO objAutorizaionResguardo = (AutorizacionResguardoDTO)Session["rptAutoriacionResguardos"];

                        AutorizantesDTO objAutorizantesDTO = (AutorizantesDTO)Session["rptAutorizantesDTO"];

                        string tipoResguardo = Request.QueryString["tipoResguardo"];

                        string TituloResguardo = "";
                        if (tipoResguardo == "2")
                        {
                            TituloResguardo = "Asignacion de resguardo de vehículo de servicio";
                        }
                        else if (tipoResguardo == "3")
                        {
                            TituloResguardo = "Liberacion de resguardo de vehículo de servicio";
                        }

                        datos2.Add(objAutorizantesDTO);
                        datos.Add(objAutorizaionResguardo);
                        rd.Database.Tables[0].SetDataSource(datos);
                        rd.Database.Tables[1].SetDataSource(getInfoEnca(TituloResguardo, "Dirección de Maquinaria y Equipo"));
                        rd.Database.Tables[2].SetDataSource(datos2);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        // string FechaVersionInforme = objResguardoVehiculo.Fecha.Date >= new DateTime(2018, 07, 01) ? "Ver. 1, 01-08-2018" : "Ver. 2,20-02-2017"; //"Ver. 1, 01-08-2018";

                        rd.SetParameterValue("VersionInforme", "Ver. 1, 01-08-2018");
                        break;
                    }
                case ReportesEnum.SOLICITUDEQUIPOREEMPLAZO:
                    {
                        setMedidasReporte("HorizontalCarta_NoModal");
                        rd = new rptReempazoEquipo();

                        var datos = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipoReemplazoDTO"];


                        AutorizadoresReemplazoDTO autorizadores = new AutorizadoresReemplazoDTO();

                        var pAutorizadores = (AutorizadoresReemplazoDTO)Session["rptAutorizadores"];
                        var pFirmasAutorizadores = (AutorizadoresReemplazoDTO)Session["firmasAutorizadores"];
                        string nombreGerente = "";
                        string nombreElabora = "";
                        string nombreAsigna = "";
                        string ElaboraFirma = "";
                        string AsignaFirma = "";
                        string GerenteFirma = "";


                        if (pAutorizadores != null)
                        {

                            nombreGerente = pAutorizadores.nombreGerente;
                            nombreElabora = pAutorizadores.nombreElabora;
                            nombreAsigna = pAutorizadores.nombreasigna;
                        }

                        if (pFirmasAutorizadores != null)
                        {

                            if (pFirmasAutorizadores.nombreElabora != null)
                            {
                                ElaboraFirma = pFirmasAutorizadores.nombreElabora;
                                // setParametro("ElaboraFirma", pFirmasAutorizadores.nombreElabora);
                            }

                            if (pFirmasAutorizadores.nombreasigna != null)
                            {
                                AsignaFirma = pFirmasAutorizadores.nombreasigna;
                                //  setParametro("AsignaFirma", pFirmasAutorizadores.nombreasigna);
                            }
                            if (pFirmasAutorizadores.nombreGerente != null)
                            {
                                GerenteFirma = pFirmasAutorizadores.nombreGerente;
                                //setParametro("GerenteFirma", pFirmasAutorizadores.nombreGerente);
                            }
                        }
                        string CC = Request.QueryString["pCC"];

                        if (string.IsNullOrWhiteSpace(CC))
                        {
                            CC = Session["rptCC"].ToString();
                        }

                        string CentroCostosNombre = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(CC);

                        // setParametro("Centro_Costos", CentroCostosNombre);
                        //  setParametro("Folio", Session["FolioSolReemplazo"].ToString());

                        rd.Database.Tables[1].SetDataSource(datos);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("SOLICITUD DE SUSTITUCIÓN DE EQUIPO", "DIRECCION DE MAQUINARIA Y EQUIPO"));

                        rd.SetParameterValue("ElaboraFirma", ElaboraFirma);
                        rd.SetParameterValue("AsignaFirma", AsignaFirma);
                        rd.SetParameterValue("GerenteFirma", GerenteFirma);
                        rd.SetParameterValue("Centro_Costos", CentroCostosNombre);
                        rd.SetParameterValue("Gerente", nombreGerente);
                        rd.SetParameterValue("Elabora", nombreElabora);
                        rd.SetParameterValue("Asigna", nombreAsigna);

                        rd.SetParameterValue("Folio", Session["FolioSolReemplazo"].ToString());

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.Solicitud_EquipoNoModal:
                    {

                        setParametro("VersionDocumento", "Ver. 1, 01-08-2018");
                        setMedidasReporte("HorizontalCarta_NoModal");
                        //setMedidasReporte("HO");
                        rd = new rptElaboracionSolicitudEquipo();
                        List<SolicitudEquipoDTO> rptData = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];

                        var autorizadores = (AutorizadoresIDDTO)Session["rptAutorizadores"];
                        var FolioSolicitud = "";

                        if (rptData != null)
                        {
                            FolioSolicitud = rptData.FirstOrDefault().Folio;
                        }



                        var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(autorizadores.usuarioElaboro)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorGerente = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.gerenteObra)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorGerenteDirector = usuarioFactoryServices.getUsuarioService()
                          .ListUsersById(autorizadores.GerenteDirector)
                          .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDirector = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.directorDivision)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDireccion = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.altaDireccion)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        string CadenaDireccion = "";
                        string CadenaDirector = "";
                        string CadenaGerente = "";
                        string CadenaElabora = "";
                        string CadenaGerenteDirector = "";

                        if (Session["rptCadenaAutorizacion"] != null)
                        {
                            var cadena = (CadenaAutorizacionDTO)Session["rptCadenaAutorizacion"];

                            CadenaDireccion = cadena.CadenaDireccion == null ? "" : cadena.CadenaDireccion;
                            CadenaDirector = cadena.CadenaDirector == null ? "" : cadena.CadenaDirector;
                            CadenaGerente = cadena.CadenaGerente == null ? "" : cadena.CadenaGerente;
                            CadenaElabora = cadena.CadenaElabora == null ? "" : cadena.CadenaElabora;
                            CadenaGerenteDirector = cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector;

                            setParametro("CadenaDireccion", FolioSolicitud);
                            setParametro("CadenaDirector", cadena.CadenaDirector == null ? "" : cadena.CadenaDirector);
                            setParametro("CadenaGerente", cadena.CadenaGerente == null ? "" : cadena.CadenaGerente);
                            setParametro("CadenaElabora", cadena.CadenaElabora == null ? "" : cadena.CadenaElabora);
                            setParametro("CadenaGerenteDirector", cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector);

                        }


                        setParametro("Elaboro", AutorizadorElabora.nombre);
                        setParametro("Solicito", AutorizadorGerente.nombre);
                        setParametro("valido", AutorizadorDirector.nombre);
                        setParametro("autorizo", AutorizadorDireccion.nombre);
                        setParametro("Valido2", AutorizadorGerenteDirector.nombre);

                        var pCentroCostosVal = Request.QueryString["pCC"].ToString();

                        if (!string.IsNullOrEmpty(pCentroCostosVal))
                        {
                            pCC = Request.QueryString["pCC"];
                            pNombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);
                        }
                        else
                        {
                            pNombreCC = "";
                        }



                        //setParametro("CentroCostos", pNombreCC);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("DIRECCIÓN DE MAQUINARIA Y EQUIPO", "SOLICITUD DE EQUIPO"));
                        rd.Database.Tables[1].SetDataSource(ToDataTable(rptData));

                        rd.SetParameterValue("FolioDocumento", FolioSolicitud);
                        rd.SetParameterValue("CentroCostos", pNombreCC);
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper() + ":");
                        rd.SetParameterValue("CadenaDireccion", FolioSolicitud);


                        Session.Add("reporte", rd);


                    }
                    break;
                case ReportesEnum.NOTASCREDITOPendientesRechazadas:
                    {
                        setMedidasReporte("HO");
                        rd = new rptNotaCreditoPendientesRechazadas();

                        DateTime FechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        DateTime FechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);
                        int TipoControl = Convert.ToInt32(Request.QueryString["tipoControl"]);
                        int Estatus = Convert.ToInt32(Request.QueryString["estatus"]);
                        string cc = Request.QueryString["cc"];
                        string almacen = Request.QueryString["almacen"];
                        string TipoNCheader = TipoControl == 1 ? "notas de crédito" : TipoControl == 2 ? "casco reman" : "notas de crédito y casco reman";
                        string ConcatDescr = "";
                        string tipo = TipoControl == 1 ? "Notas de crédito" : TipoControl == 2 ? "Casco reman" : "Notas de crédito y casco reman";

                        switch (Estatus)
                        {
                            case 3:
                                ConcatDescr = " rechazadas";
                                break;
                            case 1:
                                ConcatDescr = " pendientes";
                                break;
                            default:
                                break;
                        }

                        string TotalPeriodo = ConcatDescr + " Del periodo " + FechaInicio.ToShortDateString() + " Al " + FechaFin.ToShortDateString() + "  :";
                        // setParametro("TotalPeriodo", TotalPeriodo);
                        var dataRaw = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(FechaInicio, FechaFin, TipoControl, Estatus,cc,almacen);

                        List<RPTNotasCreditoDTO> listaData = new List<RPTNotasCreditoDTO>();
                        foreach (var x in dataRaw)
                        {
                            listaData.Add(new RPTNotasCreditoDTO
                            {
                                Generador = x.Generador,
                                cc = x.cc,
                                OC = x.OC,
                                Equipo = GetInfoMaquinaria(x.idEconomico),
                                Modelo = objMaquinaria.modeloEquipo.descripcion,
                                SerieEquipo = objMaquinaria.noSerie,
                                SerieComponente = x.SerieComponente,
                                Descripcion = x.Descripcion,
                                Fecha = x.Fecha.ToShortDateString(),
                                CausaRemosion = x.CausaRemosion == 1 ? "Programada" : "Falla",
                                HorometroEquipo = x.HorometroEconomico.ToString(),
                                HorometroComponente = x.HorometroComponente.ToString(),
                                MontoPesos = x.MontoPesos.ToString(),
                                MontoDLL = x.MontoDLL.ToString(),
                                AbonoDLL = x.AbonoDLL.ToString(),
                                NoCredito = x.ClaveCredito,
                                Comentario = x.Estado == 1 ? "Proceso" : x.Estado == 2 ? "Abonado" : "Denegado",
                                GrupoMes = x.Fecha.Month.ToString(),
                                DescripcionMes = x.Fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                                Anio = x.Fecha.Year.ToString(),
                                fechaCierre = x.FechaCaptura.ToShortDateString(),
                                Comentario2 = notaCreditoFactoryServices.getNotaCredito().GetComentario(x.id),
                                TipoNC = x.TipoNC == 1 ? "Nota de crédito" : x.TipoNC == 2 ? "Casco Reman" : ""
                            });

                        }
                        rd.Database.Tables[1].SetDataSource(listaData.OrderBy(x => x.GrupoMes));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Control de " + TipoNCheader + ConcatDescr, "DIRECCION DE MAQUINARIA Y EQUIPO ADMINISTRACION DE OVERHAUL"));
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.NOTASCREDITO:
                    {
                        setMedidasReporte("HO");
                        rd = new rptNotasCredito();

                        DateTime FechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        DateTime FechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);
                        int TipoControl = Convert.ToInt32(Request.QueryString["tipoControl"]);
                        int Estatus = Convert.ToInt32(Request.QueryString["estatus"]);
                        string cc = Request.QueryString["cc"];
                        string almacen = Request.QueryString["almacen"];
                        string TipoNCheader = TipoControl == 1 ? "notas de crédito" : TipoControl == 2 ? "casco reman" : "notas de crédito y casco reman";
                        switch (Estatus)
                        {
                            case 2:
                                TipoNCheader += " abonadas";
                                break;
                            case 4:
                                TipoNCheader += " aplicadas";
                                break;
                            default:
                                break;
                        }

                        //DateTime FechaInicio = new DateTime(2017, 6, 1);//Convert.ToDateTime("01/06/2017");
                        //DateTime FechaFin = new DateTime(2017, 6, 20);

                        string TotalPeriodo = "Total del periodo " + FechaInicio.ToShortDateString() + " Al " + FechaFin.ToShortDateString() + "  :";
                        var dataRaw = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(FechaInicio, FechaFin, TipoControl, Estatus,cc,almacen);
                        var data = dataRaw.Select(
                            x => new
                            {
                                Generador = x.Generador,
                                OC = x.OC,
                                cc= x.cc,
                                Equipo = GetInfoMaquinaria(x.idEconomico),
                                Modelo = objMaquinaria.modeloEquipo.descripcion,
                                SerieEquipo = objMaquinaria.noSerie,
                                SerieComponente = x.SerieComponente,
                                Descripcion = x.Descripcion,
                                Fecha = x.Fecha.ToShortDateString(),
                                CausaRemosion = x.CausaRemosion == 1 ? "Programada" : "Falla",
                                HorometroEquipo = x.HorometroEconomico,
                                HorometroComponente = x.HorometroComponente,
                                MontoPesos = x.MontoPesos,
                                MontoDLL = x.MontoDLL,
                                AbonoDLL = x.AbonoDLL,
                                NoCredito = x.ClaveCredito,
                                Comentario = x.Estado == 1 ? "Proceso" : x.Estado == 2 ? "Abonado" : "Denegado",
                                GrupoMes = x.FechaCaptura.Month,
                                DescripcionMes = x.FechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                                Anio = x.FechaCaptura.Year,
                                fechaCierre = x.FechaCaptura.ToShortDateString()

                            }).ToList();



                        rd.Database.Tables[1].SetDataSource(data.OrderBy(x => x.GrupoMes));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Control de " + TipoNCheader, "DIRECCION DE MAQUINARIA Y EQUIPO ADMINISTRACION DE OVERHAUL"));

                        rd.SetParameterValue("TotalPeriodo", TotalPeriodo);
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.ConciliacionSemanalStandBy:
                    {
                        setMedidasReporte("HO");
                        rd = new rptConciliacionSemanal();

                        string CC = Request.QueryString["pCC"];
                        DateTime FechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        DateTime FechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);

                        var data = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetReporte(CC, FechaInicio, FechaFin).ToList();
                        var data2 = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizadoresStandby(CC, FechaInicio, FechaFin).ToList();
                        string CentroCostosNombre = centroCostosFactoryServices.getCentroCostosService().getNombreCC(Convert.ToInt32(CC));
                        var diaInicio = FechaInicio.Day;
                        var diaFin = FechaFin.Day;
                        var Mes = FechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        var mes2 = FechaFin.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        var year = FechaFin.Year;

                        string descripcionFechas = "Relación de maquinaria que aplicará stand by, en la semana del " + FechaInicio.Day + " de " + Mes + " de " + FechaFin.Year + " al " + FechaFin.Day + " de " + mes2 + " de " + FechaFin.Year;

                        string elabora1P = "";
                        string elabora2P = "";

                        if (data2.Count > 0)
                        {
                            var elabora1 = usuarioFactoryServices.getUsuarioService().ListUsersById(data2[0]).FirstOrDefault();
                            var elabora2 = usuarioFactoryServices.getUsuarioService().ListUsersById(data2[1]).FirstOrDefault();

                            elabora1P = elabora1.nombre + " " + elabora1.apellidoPaterno + " " + elabora1.apellidoMaterno;
                            elabora2P = elabora2.nombre + " " + elabora2.apellidoPaterno + " " + elabora2.apellidoMaterno;


                        }

                        rd.Database.Tables[1].SetDataSource(ToDataTable(data));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", ""));
                        rd.SetParameterValue("Descripcion", descripcionFechas);
                        rd.SetParameterValue("elabora1", elabora1P);
                        rd.SetParameterValue("elabora2", elabora2P);
                        rd.SetParameterValue("Centro_Costos", CentroCostosNombre);
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("hCC", setTitleCC());
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.PROYECCIONES1:
                    {
                        setMedidasReporte("HO");
                        rd = new rtpEstadoResultados();
                        var pEscenario = Convert.ToInt32(Request.QueryString["Escenario"]);
                        var pDivisor = Convert.ToInt32(Request.QueryString["Divisor"]);
                        var pMes = Convert.ToInt32(Request.QueryString["meses"]);
                        var pAnio = Convert.ToInt32(Request.QueryString["anio"]);
                        var dtCObras = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(pEscenario, pDivisor, pMes, pAnio);
                        //Ventas
                        var JFluEngresosM = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("FlujodeIngresosM")).Value);
                        var lstFluEngresosM = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConjuntoDatosDTO1>>(JFluEngresosM);
                        /*VentasNetas*/
                        var JVentasNetas = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("VentasNetas")).Value);
                        var objVentasNetas = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JVentasNetas);
                        //Costo de ventas
                        var JValIngreos = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("FlujodeIngresos")).Value);
                        var lstValIngreos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConjuntoDatosDTO1>>(JValIngreos);
                        //Agregar totales
                        var JCostoGastoOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CostoDeVenta")).Value);
                        var objCostoGastoOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCostoGastoOperacion);
                        //Contribución marginal
                        var JContribucionMarginal = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("ContribucionMarginal")).Value);
                        var objContribucionMarginal = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JContribucionMarginal);
                        //Gastos de operación
                        var JTotalGtoOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("TotalGtoOperacion")).Value);
                        var objTotalGtoOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JTotalGtoOperacion);
                        var JTotalGtosOperacionR1 = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("TotalGtosOperacionR1")).Value);
                        var objTotalGtosOperacionR1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JTotalGtosOperacionR1);
                        //Utilidad de operación
                        var JUtilidadOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("UtilidadOperacion")).Value);
                        var objUtilidadOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JUtilidadOperacion);
                        var JCostoIntegral = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CostoIntegral")).Value);
                        var objCostoIntegral = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCostoIntegral);
                        //Utilidades
                        var JUtilidadAntesImp = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("UtilidadAntesImp")).Value);
                        var objUtilidadAntesImp = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JUtilidadAntesImp);
                        var JUtilidadNeta = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("UtilidadNeta")).Value);
                        var objUtilidadNeta = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JUtilidadNeta);

                        var Jimpuestos = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("impuestos")).Value);
                        var objimpuestos = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(Jimpuestos);

                        List<string> MesesSet = metodo(pMes, pAnio);
                        MesesDTO ParmMeses = new MesesDTO();
                        ParmMeses.MES1 = MesesSet[0];
                        ParmMeses.MES2 = MesesSet[1];
                        ParmMeses.MES3 = MesesSet[2];
                        ParmMeses.MES4 = MesesSet[3];
                        ParmMeses.MES5 = MesesSet[4];
                        ParmMeses.MES6 = MesesSet[5];
                        ParmMeses.MES7 = MesesSet[6];
                        ParmMeses.MES8 = MesesSet[7];
                        ParmMeses.MES9 = MesesSet[8];
                        ParmMeses.MES10 = MesesSet[9];
                        ParmMeses.MES11 = MesesSet[10];
                        ParmMeses.MES12 = MesesSet[11];

                        List<MesesDTO> listMesesPtm = new List<MesesDTO>();
                        listMesesPtm.Add(ParmMeses);

                        lstValIngreos.Add(objVentasNetas);
                        var lstCompleta = lstValIngreos.Concat(lstFluEngresosM).ToList();
                        lstCompleta.Add(objCostoGastoOperacion);
                        lstCompleta.Add(objContribucionMarginal);
                        lstCompleta.Add(objTotalGtoOperacion);
                        lstCompleta.Add(objTotalGtosOperacionR1);
                        lstCompleta.Add(objUtilidadOperacion);
                        lstCompleta.Add(objCostoIntegral);
                        lstCompleta.Add(objUtilidadAntesImp);
                        lstCompleta.Add(objimpuestos);
                        lstCompleta.Add(objUtilidadNeta);



                        var Anio = DateTime.Now.Year;
                        var anioNext = Anio + 1;

                        rd.Database.Tables[1].SetDataSource(getInfoEnca("ESTADO DE RESULTADOS 2017-2018", "( Cifras en Miles de Pesos )"));
                        rd.Database.Tables[0].SetDataSource(lstCompleta);
                        rd.Database.Tables[2].SetDataSource(ToDataTable(listMesesPtm));
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.PROYECCIONES2:
                    {
                        setMedidasReporte("HO");
                        rd = new rtpEstadoResultados();
                        var pEscenario = Convert.ToInt32(Request.QueryString["Escenario"]);
                        var pDivisor = Convert.ToInt32(Request.QueryString["Divisor"]);
                        var pMes = Convert.ToInt32(Request.QueryString["meses"]);
                        var pAnio = Convert.ToInt32(Request.QueryString["anio"]);
                        var dtCObras = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(pEscenario, pDivisor, pMes, pAnio);
                        //Ingresos de Operación
                        var JIngresosVentas = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("IngresosVentas")).Value);
                        var objIngresosVentas = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JIngresosVentas);
                        var JIngresosVentasMaq = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("IngresosVentasMaq")).Value);
                        var objIngresosVentasMaq = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JIngresosVentasMaq);
                        //Costos y gastos de operación
                        var JTotalGtoOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("TotalGtoOperacion")).Value);
                        var objTotalGtoOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JTotalGtoOperacion);
                        var JProveedorAcreedor = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("ProveedorAcreedor")).Value);
                        var objProveedorAcreedor = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JProveedorAcreedor);
                        var JCostoVentaTotal = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CostoVentaTotal")).Value);
                        var objCostoVentaTotal = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCostoVentaTotal);
                        var JCostoGastoOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CostoGastoOperacion")).Value);
                        var objCostoGastoOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCostoGastoOperacion);
                        var JCostoGastoOperacionTotal = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CostoGastoOperacionTotal")).Value);
                        var objCostoGastoOperacionTotal = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCostoGastoOperacionTotal);
                        //Flujo de operaciónes
                        var JFlujoOperacion = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("FlujoOperacion")).Value);
                        var objFlujoOperacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JFlujoOperacion);
                        var JInversionesFisicas = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("InversionesFisicas")).Value);
                        var objInversionesFisicas = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JInversionesFisicas);
                        //Flujo despues de inversiones
                        var JFlujoDespuesInversiones = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("FlujoDespuesInversiones")).Value);
                        var objFlujoDespuesInversiones = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JFlujoDespuesInversiones);
                        var JInteresesGastoDeuda = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("InteresesGastoDeuda")).Value);
                        var objInteresesGastoDeuda = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JInteresesGastoDeuda);
                        //Flujo despues de intereses
                        var JFlujoDespuesIntereses = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("FlujoDespuesIntereses")).Value);
                        var objFlujoDespuesIntereses = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JFlujoDespuesIntereses);
                        var JPagosDiversos = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("PagosDiversos")).Value);
                        var objPagosDiversos = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JPagosDiversos);
                        var JRCyCD = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("RCyCD")).Value);
                        var objRCyCD = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JRCyCD);
                        //De caja
                        var JDeCaja = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("DeCaja")).Value);
                        var objDeCaja = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JDeCaja);
                        //Aportaciones de capital
                        var JAportacionesCapital = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("AportacionesCapital")).Value);
                        var objAportacionesCapital = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JAportacionesCapital);
                        //Disposiciones de créditos
                        var JCreditosBancarios = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("CreditosBancarios")).Value);
                        var objCreditosBancarios = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JCreditosBancarios);
                        //Reservas
                        var JReservas = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("Reservas")).Value);
                        var objReservas = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JReservas);
                        //Saldos
                        var JSaldoInicial = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("SaldoInicial")).Value);
                        var objSaldoInicial = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JSaldoInicial);
                        var JSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("SaldoFinalFlujoEfectivo")).Value);
                        var objSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JSaldoFinalFlujoEfectivo);

                        var lstCompleta = new List<ConjuntoDatosDTO1>();
                        lstCompleta.Add(objIngresosVentas);
                        lstCompleta.Add(objIngresosVentasMaq);

                        lstCompleta.Add(objCostoGastoOperacionTotal);

                        lstCompleta.Add(objCostoVentaTotal);
                        lstCompleta.Add(objCostoGastoOperacion);
                        lstCompleta.Add(objProveedorAcreedor);

                        lstCompleta.Add(objFlujoOperacion);

                        lstCompleta.Add(objInversionesFisicas);
                        lstCompleta.Add(objFlujoDespuesInversiones);

                        lstCompleta.Add(objInteresesGastoDeuda);

                        lstCompleta.Add(objFlujoDespuesIntereses);
                        lstCompleta.Add(objPagosDiversos);
                        lstCompleta.Add(objRCyCD);

                        lstCompleta.Add(objDeCaja);

                        lstCompleta.Add(objAportacionesCapital);

                        lstCompleta.Add(objCreditosBancarios);

                        lstCompleta.Add(objReservas);

                        lstCompleta.Add(objSaldoInicial);
                        lstCompleta.Add(objSaldoFinalFlujoEfectivo);
                        lstCompleta.Add(objCostoGastoOperacionTotal);

                        List<string> MesesSet = metodo(pMes, pAnio);
                        MesesDTO ParmMeses = new MesesDTO();
                        ParmMeses.MES1 = MesesSet[0];
                        ParmMeses.MES2 = MesesSet[1];
                        ParmMeses.MES3 = MesesSet[2];
                        ParmMeses.MES4 = MesesSet[3];
                        ParmMeses.MES5 = MesesSet[4];
                        ParmMeses.MES6 = MesesSet[5];
                        ParmMeses.MES7 = MesesSet[6];
                        ParmMeses.MES8 = MesesSet[7];
                        ParmMeses.MES9 = MesesSet[8];
                        ParmMeses.MES10 = MesesSet[9];
                        ParmMeses.MES11 = MesesSet[10];
                        ParmMeses.MES12 = MesesSet[11];

                        List<MesesDTO> listMesesPtm = new List<MesesDTO>();
                        listMesesPtm.Add(ParmMeses);


                        rd.Database.Tables[1].SetDataSource(getInfoEnca("FLUJO DE EFECTIVO 2018 - 2019", "( Cifras en Miles de Pesos )"));
                        rd.Database.Tables[0].SetDataSource(lstCompleta);
                        rd.Database.Tables[2].SetDataSource(ToDataTable(listMesesPtm));
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.PROYECCIONES3:
                    {
                        var pEscenario = Convert.ToInt32(Request.QueryString["Escenario"]);
                        var pDivisor = Convert.ToInt32(Request.QueryString["Divisor"]);
                        var pMes = Convert.ToInt32(Request.QueryString["meses"]);
                        var pAnio = Convert.ToInt32(Request.QueryString["anio"]);
                        setMedidasReporte("HO");
                        rd = new rptFlujoEfectivo();
                        var dtCObras = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(pEscenario, pDivisor, pMes, pAnio);
                        //Activo circulante
                        EstadoResultadosDTO EfectivoInversiones = (EstadoResultadosDTO)dtCObras["EfectivoInversiones"];
                        EstadoResultadosDTO Clientes = (EstadoResultadosDTO)dtCObras["Clientes"];
                        EstadoResultadosDTO OtrosDeudores = (EstadoResultadosDTO)dtCObras["OtrosDeudores"];
                        EstadoResultadosDTO SumaCuentasPorCobrar = (EstadoResultadosDTO)dtCObras["SumaCuentasPorCobrar"];
                        EstadoResultadosDTO Inventarios = (EstadoResultadosDTO)dtCObras["Inventarios"];
                        EstadoResultadosDTO OtrosActivos = (EstadoResultadosDTO)dtCObras["OtrosActivos"];
                        EstadoResultadosDTO SumaActivoCirculante = (EstadoResultadosDTO)dtCObras["SumaActivoCirculante"];
                        //Activo no circulante
                        EstadoResultadosDTO ActivoNoCirculante = (EstadoResultadosDTO)dtCObras["ActivoNoCirculante"];
                        //Activo fijo-neto
                        EstadoResultadosDTO ActivoFijoNeto = (EstadoResultadosDTO)dtCObras["ActivoFijoNeto"];
                        //Activo Diferido
                        EstadoResultadosDTO ActivoDiferido = (EstadoResultadosDTO)dtCObras["ActivoDiferido"];
                        //suma activo total
                        EstadoResultadosDTO SumaActivoTotal = (EstadoResultadosDTO)dtCObras["SumaActivoTotal"];
                        //FIJO
                        EstadoResultadosDTO DocumentosInteresPorPagar = (EstadoResultadosDTO)dtCObras["DocumentosInteresPorPagar"];
                        EstadoResultadosDTO ProveedoresContratistas = (EstadoResultadosDTO)dtCObras["ProveedoresContratistas"];
                        EstadoResultadosDTO ImpuestosDerechoPorPagar = (EstadoResultadosDTO)dtCObras["ImpuestosDerechoPorPagar"];
                        EstadoResultadosDTO GastosAcomulados = (EstadoResultadosDTO)dtCObras["GastosAcomulados"];
                        EstadoResultadosDTO AcreedoresDiversos = (EstadoResultadosDTO)dtCObras["AcreedoresDiversos"];
                        EstadoResultadosDTO SumaPasivosCP = (EstadoResultadosDTO)dtCObras["SumaPasivosCP"];
                        EstadoResultadosDTO SumaPasivosTotal = (EstadoResultadosDTO)dtCObras["SumaPasivosTotal"];
                        //Capital contable
                        EstadoResultadosDTO CapitalSocial = (EstadoResultadosDTO)dtCObras["CapitalSocial"];
                        EstadoResultadosDTO AportFuturo = (EstadoResultadosDTO)dtCObras["AportFuturo"];
                        EstadoResultadosDTO ResultadoAcomulado = (EstadoResultadosDTO)dtCObras["ResultadoAcomulado"];
                        EstadoResultadosDTO ExcesoEnActualizacion = (EstadoResultadosDTO)dtCObras["ExcesoEnActualizacion"];
                        EstadoResultadosDTO ResultadoEjercicio = (EstadoResultadosDTO)dtCObras["ResultadoEjercicio"];
                        EstadoResultadosDTO SumaCapitalContable = (EstadoResultadosDTO)dtCObras["SumaCapitalContable"];
                        EstadoResultadosDTO SumaPasivoCapital = (EstadoResultadosDTO)dtCObras["SumaPasivoCapital"];
                        EstadoResultadosDTO Cuadre = (EstadoResultadosDTO)dtCObras["Cuadre"];

                        var lstCompleta = new List<EstadoResultadosDTO>();
                        lstCompleta.Add(EfectivoInversiones);
                        lstCompleta.Add(Clientes);
                        lstCompleta.Add(OtrosDeudores);
                        lstCompleta.Add(SumaCuentasPorCobrar);
                        lstCompleta.Add(Inventarios);
                        lstCompleta.Add(OtrosActivos);
                        lstCompleta.Add(SumaActivoCirculante);
                        //lstCompleta.Add(ActivoNoCirculante);
                        lstCompleta.Add(ActivoFijoNeto);
                        lstCompleta.Add(ActivoDiferido);
                        lstCompleta.Add(SumaActivoTotal);
                        lstCompleta.Add(DocumentosInteresPorPagar);
                        lstCompleta.Add(ProveedoresContratistas);
                        lstCompleta.Add(ImpuestosDerechoPorPagar);
                        lstCompleta.Add(GastosAcomulados);
                        lstCompleta.Add(AcreedoresDiversos);
                        lstCompleta.Add(SumaPasivosCP);
                        lstCompleta.Add(SumaPasivosTotal);
                        lstCompleta.Add(CapitalSocial);
                        lstCompleta.Add(AportFuturo);
                        lstCompleta.Add(ResultadoAcomulado);
                        lstCompleta.Add(ResultadoEjercicio);
                        lstCompleta.Add(ExcesoEnActualizacion);
                        lstCompleta.Add(SumaCapitalContable);
                        lstCompleta.Add(SumaPasivoCapital);
                        lstCompleta.Add(Cuadre);

                        List<string> MesesSet = metodo(pMes, pAnio);
                        MesesDTO ParmMeses = new MesesDTO();
                        ParmMeses.MES1 = MesesSet[0];
                        ParmMeses.MES2 = MesesSet[1];
                        ParmMeses.MES3 = MesesSet[2];
                        ParmMeses.MES4 = MesesSet[3];
                        ParmMeses.MES5 = MesesSet[4];
                        ParmMeses.MES6 = MesesSet[5];
                        ParmMeses.MES7 = MesesSet[6];
                        ParmMeses.MES8 = MesesSet[7];
                        ParmMeses.MES9 = MesesSet[8];
                        ParmMeses.MES10 = MesesSet[9];
                        ParmMeses.MES11 = MesesSet[10];
                        ParmMeses.MES12 = MesesSet[11];

                        List<MesesDTO> listMesesPtm = new List<MesesDTO>();
                        listMesesPtm.Add(ParmMeses);


                        rd.Database.Tables[0].SetDataSource(ToDataTable(lstCompleta));
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("ESTADO DE POSICION FINANCIERA", "( Cifras en Miles de Pesos )"));
                        rd.Database.Tables[2].SetDataSource(ToDataTable(listMesesPtm));
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.CifrasPrincipales:
                    {
                        setMedidasReporte("HO");
                        rd = new rptCifrasppal();

                        var pEscenario = Convert.ToInt32(Request.QueryString["Escenario"]);
                        var pDivisor = Convert.ToInt32(Request.QueryString["Divisor"]);
                        var pMes = Convert.ToInt32(Request.QueryString["meses"]);
                        var pAnio = Convert.ToInt32(Request.QueryString["anio"]);

                        var data = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(pEscenario, pDivisor, pMes, pAnio);


                        EstadoResultadosDTO ContribucionMarginal = (EstadoResultadosDTO)data["ContribucionMarginal"];
                        EstadoResultadosDTO VentasNetas = (EstadoResultadosDTO)data["VentasNetas"];
                        EstadoResultadosDTO UtilidadOperacion = (EstadoResultadosDTO)data["UtilidadOperacion"];
                        //EstadoResultadosDTO UtilidadNETA = (EstadoResultadosDTO)data["UtilidadNeta"];
                        EstadoResultadosDTO TotalGtoOperacion = (EstadoResultadosDTO)data["TotalGtoOperacion"];
                        EstadoResultadosDTO SaldoFinalFlujoEfectivo = (EstadoResultadosDTO)data["SaldoFinalFlujoEfectivo"];


                        EstadoResultadosDTO UtilidadNeta = (EstadoResultadosDTO)data["UtilidadNeta"];
                        CifrasPrincipalesDTO obj = new CifrasPrincipalesDTO();
                        List<DivisionesVentasDTO> listaDivisiones = (List<DivisionesVentasDTO>)data["ListaDivisionesVentas"];
                        List<CifrasPrincipalesDTO> dataSend = new List<CifrasPrincipalesDTO>();
                        //UTILIDAD BRUTA;



                        //>>>IMPORTE
                        obj.UImporteMes1 = (ContribucionMarginal.Fecha1).ToString("#,##0.##");
                        obj.UImporteMes2 = (ContribucionMarginal.Fecha2).ToString("#,##0.##");
                        obj.UImporteMes3 = (ContribucionMarginal.Fecha3).ToString("#,##0.##");
                        obj.UImporteMes4 = (ContribucionMarginal.Fecha4).ToString("#,##0.##");
                        obj.UImporteMes5 = (ContribucionMarginal.Fecha5).ToString("#,##0.##");
                        obj.UImporteMes6 = (ContribucionMarginal.Fecha6).ToString("#,##0.##");
                        obj.UImporteMes7 = (ContribucionMarginal.Fecha7).ToString("#,##0.##");
                        obj.UImporteMes8 = (ContribucionMarginal.Fecha8).ToString("#,##0.##");
                        obj.UImporteMes9 = (ContribucionMarginal.Fecha9).ToString("#,##0.##");
                        obj.UImporteMes10 = (ContribucionMarginal.Fecha10).ToString("#,##0.##");
                        obj.UImporteMes11 = (ContribucionMarginal.Fecha11).ToString("#,##0.##");
                        obj.UImporteMes12 = (ContribucionMarginal.Fecha12).ToString("#,##0.##");

                        //>>>VENTAS
                        obj.UsVentasMes1 = ((ContribucionMarginal.Fecha1 / VentasNetas.Fecha1) * 100).ToString("#,##0.##");
                        obj.UsVentasMes2 = ((ContribucionMarginal.Fecha2 / VentasNetas.Fecha2) * 100).ToString("#,##0.##");
                        obj.UsVentasMes3 = ((ContribucionMarginal.Fecha3 / VentasNetas.Fecha3) * 100).ToString("#,##0.##");
                        obj.UsVentasMes4 = ((ContribucionMarginal.Fecha4 / VentasNetas.Fecha4) * 100).ToString("#,##0.##");
                        obj.UsVentasMes5 = ((ContribucionMarginal.Fecha5 / VentasNetas.Fecha5) * 100).ToString("#,##0.##");
                        obj.UsVentasMes6 = ((ContribucionMarginal.Fecha6 / VentasNetas.Fecha6) * 100).ToString("#,##0.##");
                        obj.UsVentasMes7 = ((ContribucionMarginal.Fecha7 / VentasNetas.Fecha7) * 100).ToString("#,##0.##");
                        obj.UsVentasMes8 = ((ContribucionMarginal.Fecha8 / VentasNetas.Fecha8) * 100).ToString("#,##0.##");
                        obj.UsVentasMes9 = ((ContribucionMarginal.Fecha9 / VentasNetas.Fecha9) * 100).ToString("#,##0.##");
                        obj.UsVentasMes10 = ((ContribucionMarginal.Fecha10 / VentasNetas.Fecha10 * 100).ToString("#,##0.##"));
                        obj.UsVentasMes11 = ((ContribucionMarginal.Fecha11 / VentasNetas.Fecha11 * 100).ToString("#,##0.##"));
                        obj.UsVentasMes12 = (ContribucionMarginal.Fecha12 / VentasNetas.Fecha12 * 100).ToString("#,##0.##");


                        //Total Utilidad Bruta
                        var TotalUtilidadBruta = ContribucionMarginal.Fecha1 + ContribucionMarginal.Fecha2
                            + ContribucionMarginal.Fecha3
                            + ContribucionMarginal.Fecha4
                            + ContribucionMarginal.Fecha5
                            + ContribucionMarginal.Fecha6
                            + ContribucionMarginal.Fecha7
                            + ContribucionMarginal.Fecha8
                            + ContribucionMarginal.Fecha9
                            + ContribucionMarginal.Fecha10
                            + ContribucionMarginal.Fecha11
                            + ContribucionMarginal.Fecha12;
                        //UTILIDAD OPERACION
                        //>> IMPORTE
                        obj.UtilidadOperacionIMes1 = (UtilidadOperacion.Fecha1).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes2 = (UtilidadOperacion.Fecha2).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes3 = (UtilidadOperacion.Fecha3).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes4 = (UtilidadOperacion.Fecha4).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes5 = (UtilidadOperacion.Fecha5).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes6 = (UtilidadOperacion.Fecha6).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes7 = (UtilidadOperacion.Fecha7).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes8 = (UtilidadOperacion.Fecha8).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes9 = (UtilidadOperacion.Fecha9).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes10 = (UtilidadOperacion.Fecha10).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes11 = (UtilidadOperacion.Fecha11).ToString("#,##0.##");
                        obj.UtilidadOperacionIMes12 = (UtilidadOperacion.Fecha12).ToString("#,##0.##");

                        var TotalUtilidaOperacion = UtilidadOperacion.Fecha1 + UtilidadOperacion.Fecha2
                            + UtilidadOperacion.Fecha3
                            + UtilidadOperacion.Fecha4
                            + UtilidadOperacion.Fecha5
                            + UtilidadOperacion.Fecha6
                            + UtilidadOperacion.Fecha7
                            + UtilidadOperacion.Fecha8
                            + UtilidadOperacion.Fecha9
                            + UtilidadOperacion.Fecha10
                            + UtilidadOperacion.Fecha11
                            + UtilidadOperacion.Fecha12;
                        //>> VENTAS
                        obj.UtilidadOperacionVMes1 = ((UtilidadOperacion.Fecha1 / VentasNetas.Fecha1) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes2 = ((UtilidadOperacion.Fecha2 / VentasNetas.Fecha2) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes3 = ((UtilidadOperacion.Fecha3 / VentasNetas.Fecha3) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes4 = ((UtilidadOperacion.Fecha4 / VentasNetas.Fecha4) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes5 = ((UtilidadOperacion.Fecha5 / VentasNetas.Fecha5) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes6 = ((UtilidadOperacion.Fecha6 / VentasNetas.Fecha6) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes7 = ((UtilidadOperacion.Fecha7 / VentasNetas.Fecha7) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes8 = ((UtilidadOperacion.Fecha8 / VentasNetas.Fecha8) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes9 = ((UtilidadOperacion.Fecha9 / VentasNetas.Fecha9) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes10 = ((UtilidadOperacion.Fecha10 / VentasNetas.Fecha10) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes11 = ((UtilidadOperacion.Fecha11 / VentasNetas.Fecha11) * 100).ToString("#,##0.##");
                        obj.UtilidadOperacionVMes12 = ((UtilidadOperacion.Fecha12 / VentasNetas.Fecha12) * 100).ToString("#,##0.##");

                        ///UTILIDAD NETA
                        ///>>>Acumulada
                        ///qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww

                        obj.UMensualMes1 = (UtilidadOperacion.Fecha1).ToString("#,##0.##");
                        obj.UMensualMes2 = (UtilidadOperacion.Fecha2).ToString("#,##0.##");
                        obj.UMensualMes3 = (UtilidadOperacion.Fecha3).ToString("#,##0.##");
                        obj.UMensualMes4 = (UtilidadOperacion.Fecha4).ToString("#,##0.##");
                        obj.UMensualMes5 = (UtilidadOperacion.Fecha5).ToString("#,##0.##");
                        obj.UMensualMes6 = (UtilidadOperacion.Fecha6).ToString("#,##0.##");
                        obj.UMensualMes7 = (UtilidadOperacion.Fecha7).ToString("#,##0.##");
                        obj.UMensualMes8 = (UtilidadOperacion.Fecha8).ToString("#,##0.##");
                        obj.UMensualMes9 = (UtilidadOperacion.Fecha9).ToString("#,##0.##");
                        obj.UMensualMes10 = (UtilidadOperacion.Fecha10).ToString("#,##0.##");
                        obj.UMensualMes11 = (UtilidadOperacion.Fecha11).ToString("#,##0.##");
                        obj.UMensualMes12 = (UtilidadOperacion.Fecha12).ToString("#,##0.##");

                        List<string> AcumuladoUtilidaNeta = getAcumulado(pMes, UtilidadNeta);
                        obj.UAcumuladaMes1 = AcumuladoUtilidaNeta[0]; //(UtilidadOperacion.Fecha1).ToString("#,##0.##");
                        obj.UAcumuladaMes2 = AcumuladoUtilidaNeta[1];//(Convert.ToDecimal(obj.UMensualMes2) + Convert.ToDecimal(obj.UAcumuladaMes1)).ToString("#,##0.##");
                        obj.UAcumuladaMes3 = AcumuladoUtilidaNeta[2];//(Convert.ToDecimal(obj.UMensualMes3) + Convert.ToDecimal(obj.UAcumuladaMes2)).ToString("#,##0.##");
                        obj.UAcumuladaMes4 = AcumuladoUtilidaNeta[3];//(Convert.ToDecimal(obj.UMensualMes4) + Convert.ToDecimal(obj.UAcumuladaMes3)).ToString("#,##0.##");
                        obj.UAcumuladaMes5 = AcumuladoUtilidaNeta[4];//(Convert.ToDecimal(obj.UMensualMes5) + Convert.ToDecimal(obj.UAcumuladaMes4)).ToString("#,##0.##");
                        obj.UAcumuladaMes6 = AcumuladoUtilidaNeta[5];///(Convert.ToDecimal(obj.UMensualMes6) + Convert.ToDecimal(obj.UAcumuladaMes5)).ToString("#,##0.##");
                        obj.UAcumuladaMes7 = AcumuladoUtilidaNeta[6];// (Convert.ToDecimal(obj.UMensualMes7) + Convert.ToDecimal(obj.UAcumuladaMes6)).ToString("#,##0.##");
                        obj.UAcumuladaMes8 = AcumuladoUtilidaNeta[7];//(Convert.ToDecimal(obj.UMensualMes8) + Convert.ToDecimal(obj.UAcumuladaMes7)).ToString("#,##0.##");
                        obj.UAcumuladaMes9 = AcumuladoUtilidaNeta[8];///(Convert.ToDecimal(obj.UMensualMes9) + Convert.ToDecimal(obj.UAcumuladaMes8)).ToString("#,##0.##");

                        obj.UAcumuladaMes10 = AcumuladoUtilidaNeta[9];// (Convert.ToDecimal(obj.UMensualMes10) + Convert.ToDecimal(obj.UAcumuladaMes9)).ToString("#,##0.##");
                        obj.UAcumuladaMes11 = AcumuladoUtilidaNeta[10];//(Convert.ToDecimal(obj.UMensualMes11) + Convert.ToDecimal(obj.UAcumuladaMes10)).ToString("#,##0.##");

                        obj.UAcumuladaMes12 = AcumuladoUtilidaNeta[11];// (Convert.ToDecimal(obj.UMensualMes12) + Convert.ToDecimal(obj.UAcumuladaMes11)).ToString("#,##0.##");

                        ///>>>Mensual
                        //>>Margen vs Ventas 
                        obj.UMargeVentas1 = ((UtilidadOperacion.Fecha1 / VentasNetas.Fecha1) * 100).ToString("#,##0.##");
                        obj.UMargeVentas2 = ((UtilidadOperacion.Fecha2 / VentasNetas.Fecha2) * 100).ToString("#,##0.##");
                        obj.UMargeVentas3 = ((UtilidadOperacion.Fecha3 / VentasNetas.Fecha3) * 100).ToString("#,##0.##");
                        obj.UMargeVentas4 = ((UtilidadOperacion.Fecha4 / VentasNetas.Fecha4) * 100).ToString("#,##0.##");
                        obj.UMargeVentas5 = ((UtilidadOperacion.Fecha5 / VentasNetas.Fecha5) * 100).ToString("#,##0.##");
                        obj.UMargeVentas6 = ((UtilidadOperacion.Fecha6 / VentasNetas.Fecha6) * 100).ToString("#,##0.##");
                        obj.UMargeVentas7 = ((UtilidadOperacion.Fecha7 / VentasNetas.Fecha7) * 100).ToString("#,##0.##");
                        obj.UMargeVentas8 = ((UtilidadOperacion.Fecha8 / VentasNetas.Fecha8) * 100).ToString("#,##0.##");
                        obj.UMargeVentas9 = ((UtilidadOperacion.Fecha9 / VentasNetas.Fecha9) * 100).ToString("#,##0.##");
                        obj.UMargeVentas10 = ((UtilidadOperacion.Fecha10 / VentasNetas.Fecha10) * 100).ToString("#,##0.##");
                        obj.UMargeVentas11 = ((UtilidadOperacion.Fecha11 / VentasNetas.Fecha11) * 100).ToString("#,##0.##");
                        obj.UMargeVentas12 = ((UtilidadOperacion.Fecha12 / VentasNetas.Fecha12) * 100).ToString("#,##0.##");

                        //VENTAS
                        //>>Mensual.
                        obj.VMensualMes1 = (VentasNetas.Fecha1).ToString("#,##0.##");
                        obj.VMensualMes2 = (VentasNetas.Fecha2).ToString("#,##0.##");
                        obj.VMensualMes3 = (VentasNetas.Fecha3).ToString("#,##0.##");
                        obj.VMensualMes4 = (VentasNetas.Fecha4).ToString("#,##0.##");
                        obj.VMensualMes5 = (VentasNetas.Fecha5).ToString("#,##0.##");
                        obj.VMensualMes6 = (VentasNetas.Fecha6).ToString("#,##0.##");
                        obj.VMensualMes7 = (VentasNetas.Fecha7).ToString("#,##0.##");
                        obj.VMensualMes8 = (VentasNetas.Fecha8).ToString("#,##0.##");
                        obj.VMensualMes9 = (VentasNetas.Fecha9).ToString("#,##0.##");
                        obj.VMensualMes10 = (VentasNetas.Fecha10).ToString("#,##0.##");
                        obj.VMensualMes11 = (VentasNetas.Fecha11).ToString("#,##0.##");
                        obj.VMensualMes12 = (VentasNetas.Fecha12).ToString("#,##0.##");

                        var TotalVentasMensual = VentasNetas.Fecha1 + VentasNetas.Fecha2
                        + VentasNetas.Fecha3
                        + VentasNetas.Fecha4
                        + VentasNetas.Fecha5
                        + VentasNetas.Fecha6
                        + VentasNetas.Fecha7
                        + VentasNetas.Fecha8
                        + VentasNetas.Fecha9
                        + VentasNetas.Fecha10
                        + VentasNetas.Fecha11
                        + VentasNetas.Fecha12;

                        //>>Acumulado.

                        obj.VAcumuladaMes1 = (obj.VMensualMes1);
                        obj.VAcumuladaMes2 = (Convert.ToDecimal(obj.VAcumuladaMes1) + Convert.ToDecimal(obj.VMensualMes2)).ToString("#,##0.##");
                        obj.VAcumuladaMes3 = (Convert.ToDecimal(obj.VAcumuladaMes2) + Convert.ToDecimal(obj.VMensualMes3)).ToString("#,##0.##");
                        obj.VAcumuladaMes4 = (Convert.ToDecimal(obj.VAcumuladaMes3) + Convert.ToDecimal(obj.VMensualMes4)).ToString("#,##0.##");
                        obj.VAcumuladaMes5 = (Convert.ToDecimal(obj.VAcumuladaMes4) + Convert.ToDecimal(obj.VMensualMes5)).ToString("#,##0.##");
                        obj.VAcumuladaMes6 = (Convert.ToDecimal(obj.VAcumuladaMes5) + Convert.ToDecimal(obj.VMensualMes6)).ToString("#,##0.##");
                        obj.VAcumuladaMes7 = (Convert.ToDecimal(obj.VAcumuladaMes6) + Convert.ToDecimal(obj.VMensualMes7)).ToString("#,##0.##");
                        obj.VAcumuladaMes8 = (Convert.ToDecimal(obj.VAcumuladaMes7) + Convert.ToDecimal(obj.VMensualMes8)).ToString("#,##0.##");
                        obj.VAcumuladaMes9 = (Convert.ToDecimal(obj.VAcumuladaMes8) + Convert.ToDecimal(obj.VMensualMes9)).ToString("#,##0.##");
                        obj.VAcumuladaMes10 = (Convert.ToDecimal(obj.VAcumuladaMes9) + Convert.ToDecimal(obj.VMensualMes10)).ToString("#,##0.##");
                        obj.VAcumuladaMes11 = (Convert.ToDecimal(obj.VAcumuladaMes10) + Convert.ToDecimal(obj.VMensualMes11)).ToString("#,##0.##");
                        obj.VAcumuladaMes12 = (Convert.ToDecimal(obj.VAcumuladaMes11) + Convert.ToDecimal(obj.VMensualMes12)).ToString("#,##0.##");

                        //GTOS DE OPERACION
                        ///
                        ///   
                        /// 
                        obj.GOMensualMes1 = (TotalGtoOperacion.Fecha1).ToString("#,##0.##");
                        obj.GOMensualMes2 = (TotalGtoOperacion.Fecha2).ToString("#,##0.##");
                        obj.GOMensualMes3 = (TotalGtoOperacion.Fecha3).ToString("#,##0.##");
                        obj.GOMensualMes4 = (TotalGtoOperacion.Fecha4).ToString("#,##0.##");
                        obj.GOMensualMes5 = (TotalGtoOperacion.Fecha5).ToString("#,##0.##");
                        obj.GOMensualMes6 = (TotalGtoOperacion.Fecha6).ToString("#,##0.##");
                        obj.GOMensualMes7 = (TotalGtoOperacion.Fecha7).ToString("#,##0.##");
                        obj.GOMensualMes8 = (TotalGtoOperacion.Fecha8).ToString("#,##0.##");
                        obj.GOMensualMes9 = (TotalGtoOperacion.Fecha9).ToString("#,##0.##");
                        obj.GOMensualMes10 = (TotalGtoOperacion.Fecha10).ToString("#,##0.##");
                        obj.GOMensualMes11 = (TotalGtoOperacion.Fecha11).ToString("#,##0.##");
                        obj.GOMensualMes12 = (TotalGtoOperacion.Fecha12).ToString("#,##0.##");

                        var GastosOperacionTotal = TotalGtoOperacion.Fecha1 +
                            TotalGtoOperacion.Fecha2 +
                            TotalGtoOperacion.Fecha3 +
                            TotalGtoOperacion.Fecha4 +
                            TotalGtoOperacion.Fecha5 +
                            TotalGtoOperacion.Fecha6 +
                            TotalGtoOperacion.Fecha7 +
                            TotalGtoOperacion.Fecha8 +
                            TotalGtoOperacion.Fecha9 +
                            TotalGtoOperacion.Fecha10 +
TotalGtoOperacion.Fecha11 +
TotalGtoOperacion.Fecha12;
                        ///
                        obj.GOAcumuladaMes1 = (Convert.ToDecimal(obj.GOMensualMes1)).ToString("#,##0.##");

                        obj.GOAcumuladaMes2 = (Convert.ToDecimal(obj.GOAcumuladaMes1) + Convert.ToDecimal(obj.GOMensualMes2)).ToString("#,##0.##");
                        obj.GOAcumuladaMes3 = (Convert.ToDecimal(obj.GOAcumuladaMes2) + Convert.ToDecimal(obj.GOMensualMes3)).ToString("#,##0.##");
                        obj.GOAcumuladaMes4 = (Convert.ToDecimal(obj.GOAcumuladaMes3) + Convert.ToDecimal(obj.GOMensualMes4)).ToString("#,##0.##");
                        obj.GOAcumuladaMes5 = (Convert.ToDecimal(obj.GOAcumuladaMes4) + Convert.ToDecimal(obj.GOMensualMes5)).ToString("#,##0.##");
                        obj.GOAcumuladaMes6 = (Convert.ToDecimal(obj.GOAcumuladaMes5) + Convert.ToDecimal(obj.GOMensualMes6)).ToString("#,##0.##");
                        obj.GOAcumuladaMes7 = (Convert.ToDecimal(obj.GOAcumuladaMes6) + Convert.ToDecimal(obj.GOMensualMes7)).ToString("#,##0.##");
                        obj.GOAcumuladaMes8 = (Convert.ToDecimal(obj.GOAcumuladaMes7) + Convert.ToDecimal(obj.GOMensualMes8)).ToString("#,##0.##");
                        obj.GOAcumuladaMes9 = (Convert.ToDecimal(obj.GOAcumuladaMes8) + Convert.ToDecimal(obj.GOMensualMes9)).ToString("#,##0.##");
                        obj.GOAcumuladaMes10 = (Convert.ToDecimal(obj.GOAcumuladaMes9) + Convert.ToDecimal(obj.GOMensualMes10)).ToString("#,##0.##");
                        obj.GOAcumuladaMes11 = (Convert.ToDecimal(obj.GOAcumuladaMes10) + Convert.ToDecimal(obj.GOMensualMes11)).ToString("#,##0.##");
                        obj.GOAcumuladaMes12 = (Convert.ToDecimal(obj.GOAcumuladaMes11) + Convert.ToDecimal(obj.GOMensualMes12)).ToString("#,##0.##");

                        /////
                        obj.GOMargenVentasMes1 = (Convert.ToDecimal(obj.GOMensualMes1) / Convert.ToDecimal(obj.VMensualMes1) * 100).ToString("#,##0.##");


                        obj.GOMargenVentasMes2 = (Convert.ToDecimal(obj.GOMensualMes2) / Convert.ToDecimal(obj.VMensualMes2) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes3 = (Convert.ToDecimal(obj.GOMensualMes3) / Convert.ToDecimal(obj.VMensualMes3) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes4 = (Convert.ToDecimal(obj.GOMensualMes4) / Convert.ToDecimal(obj.VMensualMes4) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes5 = (Convert.ToDecimal(obj.GOMensualMes5) / Convert.ToDecimal(obj.VMensualMes5) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes6 = (Convert.ToDecimal(obj.GOMensualMes6) / Convert.ToDecimal(obj.VMensualMes6) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes7 = (Convert.ToDecimal(obj.GOMensualMes7) / Convert.ToDecimal(obj.VMensualMes7) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes8 = (Convert.ToDecimal(obj.GOMensualMes8) / Convert.ToDecimal(obj.VMensualMes8) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes9 = (Convert.ToDecimal(obj.GOMensualMes9) / Convert.ToDecimal(obj.VMensualMes9) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes10 = (Convert.ToDecimal(obj.GOMensualMes10) / Convert.ToDecimal(obj.VMensualMes10) * 100).ToString("#,##0.##");

                        obj.GOMargenVentasMes11 = (Convert.ToDecimal(obj.GOMensualMes11) / Convert.ToDecimal(obj.VMensualMes11) * 100).ToString("#,##0.##");
                        obj.GOMargenVentasMes12 = (Convert.ToDecimal(obj.GOMensualMes12) / Convert.ToDecimal(obj.VMensualMes12) * 100).ToString("#,##0.##");

                        var TotalGastosOperacionMensual = TotalGtoOperacion.Fecha1 + TotalGtoOperacion.Fecha2
                                                    + TotalGtoOperacion.Fecha3
                                                    + TotalGtoOperacion.Fecha4
                                                    + TotalGtoOperacion.Fecha5
                                                    + TotalGtoOperacion.Fecha6
                                                    + TotalGtoOperacion.Fecha7
                                                    + TotalGtoOperacion.Fecha8
                                                    + TotalGtoOperacion.Fecha9
                                                    + TotalGtoOperacion.Fecha10
                                                    + TotalGtoOperacion.Fecha11
                                                    + TotalGtoOperacion.Fecha12;



                        ////

                        obj.FMMes1 = (SaldoFinalFlujoEfectivo.Fecha1).ToString("#,##0.##");
                        obj.FMMes2 = (SaldoFinalFlujoEfectivo.Fecha2).ToString("#,##0.##");
                        obj.FMMes3 = (SaldoFinalFlujoEfectivo.Fecha3).ToString("#,##0.##");
                        obj.FMMes4 = (SaldoFinalFlujoEfectivo.Fecha4).ToString("#,##0.##");
                        obj.FMMes5 = (SaldoFinalFlujoEfectivo.Fecha5).ToString("#,##0.##");
                        obj.FMMes6 = (SaldoFinalFlujoEfectivo.Fecha6).ToString("#,##0.##");
                        obj.FMMes7 = (SaldoFinalFlujoEfectivo.Fecha7).ToString("#,##0.##");
                        obj.FMMes8 = (SaldoFinalFlujoEfectivo.Fecha8).ToString("#,##0.##");
                        obj.FMMes9 = (SaldoFinalFlujoEfectivo.Fecha9).ToString("#,##0.##");
                        obj.FMMes10 = (SaldoFinalFlujoEfectivo.Fecha10).ToString("#,##0.##");
                        obj.FMMes11 = (SaldoFinalFlujoEfectivo.Fecha11).ToString("#,##0.##");
                        obj.FMMes12 = (SaldoFinalFlujoEfectivo.Fecha12).ToString("#,##0.##");

                        //obj.FMMes1 = (74071).ToString("#,##0.##");
                        //obj.FMMes2 = (176346).ToString("#,##0.##");
                        //obj.FMMes3 = (223683).ToString("#,##0.##");
                        //obj.FMMes4 = (235985).ToString("#,##0.##");
                        //obj.FMMes5 = (239544).ToString("#,##0.##");
                        //obj.FMMes6 = (242993).ToString("#,##0.##");
                        //obj.FMMes7 = (245479).ToString("#,##0.##");
                        //obj.FMMes8 = (239714).ToString("#,##0.##");
                        //obj.FMMes9 = (243693).ToString("#,##0.##");
                        //obj.FMMes10 = (249036).ToString("#,##0.##");
                        //obj.FMMes11 = (253019).ToString("#,##0.##");
                        //obj.FMMes12 = (256853).ToString("#,##0.##");

                        List<string> MesesSet = metodo(pMes, pAnio);
                        MesesDTO ParmMeses = new MesesDTO();
                        ParmMeses.MES1 = MesesSet[0];
                        ParmMeses.MES2 = MesesSet[1];
                        ParmMeses.MES3 = MesesSet[2];
                        ParmMeses.MES4 = MesesSet[3];
                        ParmMeses.MES5 = MesesSet[4];
                        ParmMeses.MES6 = MesesSet[5];
                        ParmMeses.MES7 = MesesSet[6];
                        ParmMeses.MES8 = MesesSet[7];
                        ParmMeses.MES9 = MesesSet[8];
                        ParmMeses.MES10 = MesesSet[9];
                        ParmMeses.MES11 = MesesSet[10];
                        ParmMeses.MES12 = MesesSet[11];




                        List<MesesDTO> listMesesPtm = new List<MesesDTO>();
                        listMesesPtm.Add(ParmMeses);
                        dataSend.Add(obj);
                        //var listaEnviarDivisiones = listaDivisiones.Select(x=>new {x.Lugar,x.Obra, Venta = x.Venta.ToString("0.##"), Procentaje = (x.Procentaje*100)}).Take(3);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("CIFRAS PRINCIPALES DE PROYECCION " + pAnio.ToString() + " - " + (pAnio + 1).ToString() + "", ""));
                        rd.Database.Tables[2].SetDataSource(listaDivisiones);
                        rd.Database.Tables[1].SetDataSource(ToDataTable(dataSend));
                        rd.Database.Tables[3].SetDataSource(ToDataTable(listMesesPtm));

                        rd.SetParameterValue("UtilidadBrutaTotal", TotalUtilidadBruta);
                        rd.SetParameterValue("UtilidadBrutaPorcentaje", (TotalUtilidadBruta / VentasNetas.Total) * 100);
                        rd.SetParameterValue("UtilidadOperacionTotal", TotalUtilidaOperacion);
                        rd.SetParameterValue("UtilidadOperacionPorcentaje", (TotalUtilidaOperacion / VentasNetas.Total) * 100);
                        rd.SetParameterValue("UtilidadNetaTotal", UtilidadNeta.Total);
                        rd.SetParameterValue("UtilidadNetaPorcentaje", (UtilidadNeta.Total / VentasNetas.Total) * 100);
                        rd.SetParameterValue("VentaTotal", TotalVentasMensual);
                        rd.SetParameterValue("GastosOperacionTotal", GastosOperacionTotal);
                        rd.SetParameterValue("MargenVsVentas", (GastosOperacionTotal / TotalVentasMensual) * 100);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.FICHATECNICA:
                    {
                        setMedidasReporte("HC");
                        rd = new rtpFichaTecnicaMaquinariayEquipo();
                        List<FichaTecnicaAltaDTO> datos = new List<FichaTecnicaAltaDTO>();


                        datos = (List<FichaTecnicaAltaDTO>)Session["RFichaTecnica"];

                        var enviar = datos.Select(x => new
                        {
                            horometro = x.horometro,
                            añoEquipo = x.añoEquipo,
                            Proveedor = x.Proveedor,
                            EntregaSitio = x.EntregaSitio,
                            LugarEntrega = x.LugarEntrega,
                            OrdenCompra = x.OrdenCompra,
                            CostoEquipo = x.CostoEquipo,
                            Descripcion = x.Descripcion,
                            Marca = x.Marca,
                            Modelo = x.Modelo,
                            NoSerie = x.NoSerie,
                            Arreglo = x.Arreglo,
                            MarcaMotor = x.MarcaMotor,
                            ModeloMotor = x.ModeloMotor,
                            SerieMotor = x.SerieMotor,
                            ArregloMotor = x.ArregloMotor,
                            CodicionesUso = x.CodicionesUso,
                            AdquisicionEquipo1 = x.Adquisicion == "1" ? "1" : "",
                            AdquisicionEquipo2 = x.Adquisicion == "0" ? "1" : "",
                            EquipoMayor = x.TipoEquipo == "1" ? "1" : "",
                            EquipoMenor = x.TipoEquipo == "2" ? "1" : "",
                            EquipoTransporte = x.TipoEquipo == "3" ? "1" : "",
                            Fabricacion1 = x.LugarFabricacion == "1" ? "1" : "",
                            Fabricacion2 = x.LugarFabricacion == "0" ? "1" : "",
                            Pedimento = x.Pedimento,
                            Economico = x.Economico,
                            LibreAbordo = x.LibreAbordo
                        }).ToList();

                        rd.Database.Tables[1].SetDataSource(ToDataTable(enviar));
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("FICHA TÉCNICA DE MAQUINARIA Y EQUIPO", "Gerencia de Adquisición y Renta de Maquinaria"));
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("FechaElaboracion", datos.First().fechaAdquisicion);


                        rd.SetParameterValue("Version", "Ver. 1, 01-08-2018");

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.CadenaProductiva:
                    {
                        setMedidasReporte("HO");
                        rd = new rptCadenaPro();
                        //  var datos = (DataTable)Session["dtReporteComparativaTipos"];

                        var array = (List<VencimientoDTO>)Session["ListaVencimiento"];
                        var proveedor = array.Select(x => x.proveedor).Distinct().ToList();

                        rd.Database.Tables[0].SetDataSource(array.Select(x => new { factura = x.factura.ToString(), concepto = x.proveedor, monto = changeFormat(x.saldoFactura), tipoCambio = x.tipoMoneda.ToString(), centro_costos = x.nombCC, CC = x.centro_costos, iva = changeFormat(x.saldoFactura) }));
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Reporte Cadena Produtiva", "Contabilidad"));

                        rd.SetParameterValue("proveedor", string.Join(", ", proveedor));
                        rd.SetParameterValue("totalDocumentos", array.Count());
                        List<decimal> montos = new List<decimal>();
                        array.ForEach(x => montos.Add(changeFormat(x.saldoFactura)));
                        rd.SetParameterValue("montoMinimo", montos.Min().ToString("C2"));
                        rd.SetParameterValue("montoMaximo", montos.Max().ToString("C2"));
                        rd.SetParameterValue("hCC", setTitleCC());
                        Session.Add("reporte", rd);
                    }
                    break;
                case ReportesEnum.Gastos_Maquinaria:
                    //var datos = repComparativaTiposFactoryServices.getComparativoTiposService().getDataPrueba().Take(10).ToList();
                    //dataT.Tables.Add(ToDataTable(datos));
                    //rd = new CrystalReport4();
                    //rd.SetDataSource(dataT);
                    break;
                case ReportesEnum.Comparativa_Tipos:
                    {
                        setMedidasReporte("HC");
                        string area = Request.QueryString["area"];
                        string fechaInicio = Request.QueryString["fechaInicio"];
                        string fechaFin = Request.QueryString["fechaFin"];
                        responseString = "area=" + area + "&" + "fechaInicio=" + fechaInicio + "&" + "fechaFin=" + fechaFin;
                        var dict = HttpUtility.ParseQueryString(responseString);
                        string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));

                        // RepGastosFiltrosDTO obj = JsonConvert.DeserializeObject<RepGastosFiltrosDTO>(json);

                        //  lblTittle.Text = EnumExtensions.GetDescription((ReportesEnum)reporte);

                        rd = new rptComparativaTipos();
                        var datos = (DataTable)Session["dtReporteComparativaTipos"];
                        rd.Database.Tables[0].SetDataSource(datos);
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Reporte Comparativa de Tipos", "Dirección de Maquinaria y Equipo"));
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.captura_diaria:
                    {
                        setMedidasReporte("VC");
                        pCC = Request.QueryString["CC"];
                        pTurno = Convert.ToInt32(Request.QueryString["pTurno"]);
                        pFechaInicio = Convert.ToDateTime(Request.QueryString["pFecha"].ToString());

                        responseString = "CC=" + pCC + "&" + "turno=" + pTurno + "&" + "Fecha=" + pFechaInicio;

                        tblM_CapHorometro objHorometro = new tblM_CapHorometro
                        {
                            CC = pCC,
                            turno = pTurno,
                            Fecha = pFechaInicio
                        };


                        rd = new rptCapturaDiaria();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Captura diaria", "Dirección de Maquinaria y Equipo"));

                        List<CapHorometroDTO> data = (List<CapHorometroDTO>)Session["ReporteHorometro"];
                        /* List<CapHorometroDTO> data = capturaHorometroFactoryServices
                                                     .getCapturaHorometroServices()
                                                     .getReporteDiario(objHorometro);*/

                        rd.Database.Tables[1].SetDataSource(ToDataTable(data));
                        rd.SetParameterValue("hCC", setTitleCCCorto());
                        Session.Add("reporte", rd);

                        break;
                    }
                //raguilar reporte resguardo 11/04/18 
                case ReportesEnum.ReporteGralResguardo:
                    {
                        setMedidasReporte("VC");
                        string CC = Request.QueryString["CC"];
                        int tipoResguardo = Convert.ToInt32(Request.QueryString["tipoResguardo"]);
                        var listAutorizaciones = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetListaAutorizacionesPendientes(CC, tipoResguardo);
                        int idUsuario = vSesiones.sesionUsuarioDTO.id;
                        var ListaCCUsuarios = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario).Select(x => x.cc).ToList();
                        var resFinal = listAutorizaciones.Where(x => ListaCCUsuarios.Contains(x.Obra)).Select(x => new
                        {
                            //id = x.id,
                            NoEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.MaquinariaID).FirstOrDefault().noEconomico,
                            Obra = x.Obra.ToString() + "-" + centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.Obra),
                            fechaCaptura = x.Fecha.ToShortDateString(),
                            UsuarioResguardo = x.nombEmpleado,
                            DescripcionEquipo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.MaquinariaID).FirstOrDefault().grupoMaquinaria.descripcion,
                            DiasLicencia = Convert.ToString(x.fechaVencimiento.Subtract(DateTime.Today).Days),

                        });
                        rd = new rptResguardoGral();
                        rd.Database.Tables[0].SetDataSource(resFinal.ToList());
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Resguardo De Unidades", ""));//cabecera
                        string CCTxt = Request.QueryString["CCTxt"];
                        string tipoResguardoTxt = Request.QueryString["tipoResguardoTxt"];
                        if (CCTxt == "--Seleccione--")
                            CCTxt = "Todos los " + setTitleCC();
                        rd.SetParameterValue("CentroCostoText", CCTxt);
                        rd.SetParameterValue("tipoResguardoText", tipoResguardoTxt);
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.minuta_reunion:
                    {
                        setMedidasReporte("HO");
                        rd = new rptMinutaReunion();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Minuta de reunión", "Sistema de Gestión de Calidad"));

                        int minuta = int.Parse(Request.QueryString["minuta"]);

                        var objMinuta = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinuta(minuta, 0);
                        var objMinutaPrint = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinutaPrint(minuta, 0);

                        rd.Database.Tables[1].SetDataSource(ToDataTable(objMinutaPrint));

                        rd.SetParameterValue("Fecha", Convert.ToDateTime(objMinuta.fecha).ToString("dd/MM/yyyy"));
                        rd.SetParameterValue("Lugar", objMinuta.lugar);
                        rd.SetParameterValue("Evento", objMinuta.titulo);
                        rd.SetParameterValue("Inicio", objMinuta.horaInicio);
                        rd.SetParameterValue("Termino", objMinuta.horaFin);
                        rd.SetParameterValue("Asuntos", objMinuta.descripcion);


                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.lista_asistencia:
                    {
                        setMedidasReporte("HO");
                        rd = new rptMinutaListaAsistencia();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Lista de asistencia", "Sistema de Gestión de Calidad"));

                        int minuta = int.Parse(Request.QueryString["minuta"]);
                        var objMinuta = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinuta(minuta, 0);
                        var objListaAsistencia = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getListaAsistenciaMinutaPrint(minuta, 0);

                        rd.Database.Tables[1].SetDataSource(ToDataTable(objListaAsistencia));
                        rd.SetParameterValue("Fecha", Convert.ToDateTime(objMinuta.fecha).ToString("dd/MM/yyyy"));
                        rd.SetParameterValue("Evento", objMinuta.titulo);
                        rd.SetParameterValue("Inicio", objMinuta.horaInicio);
                        rd.SetParameterValue("Termino", objMinuta.horaFin);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Carga_Combustible:
                    {
                        rd = new rptCargaMensualCombustible();
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("DIRECCION DE MAQUINARIA Y EQUIPO", "REPORTE DE CARGA DE COMBUSTIBLE MENSUAL"));
                        setMedidasReporte("HO");
                        string centro_costosValue = Request.QueryString["cc"];
                        pFechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);

                        var dataCommbustible = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getDataReporteCombustibleMensual(centro_costosValue, pFechaInicio);
                        var dts = (DataTable)Session["reporteCombustiblesMensual"];//getInfoRep(dataCommbustible, pFechaInicio);

                        string centro_costos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(centro_costosValue);
                        rd.Database.Tables[0].SetDataSource(dts);

                        rd.SetParameterValue("hCC", setTitleEco());
                        rd.SetParameterValue("OBRA", centro_costos);
                        rd.SetParameterValue("MES", pFechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.redimiento_combustible:
                    {
                        setMedidasReporte("HO");
                        rd = new rptRendimientroCombustible();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("REPORTE DE RENDIMIENTO DE COMBUSTIBLE", "GERENCIA DE MAQUINARIA Y EQUIPO"));
                        string cc = Request.QueryString["cc"];
                        pFechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        pFechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);
                        var rendimiento = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getReporteRendimientoComb(cc, pFechaInicio, pFechaFin);
                        string CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(cc);
                        rd.Database.Tables[1].SetDataSource(ToDataTable(rendimiento));

                        rd.SetParameterValue("CentroCostos", CCName);
                        rd.SetParameterValue("hCC", setTitleCC() + ":");
                        rd.SetParameterValue("hEco", setTitleEco() + ":");
                        string RangoFechas = "";
                        var Mes = pFechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));



                        RangoFechas = "Relación de combustible del día" + pFechaInicio.Day + " al " + pFechaFin.Day + " del mes " + Mes + " de " + pFechaFin.Year;
                        rd.SetParameterValue("RangoFechas", RangoFechas);
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.captura_horometros:
                    {
                        setMedidasReporte("VC");
                        rd = new rptCapturaHorometro();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("REPORTE CAPTURA DE HOROMETROS", "GERENCIA DE MAQUINARIA Y EQUIPO"));

                        var cc = string.IsNullOrEmpty(Request.QueryString["pCC"].ToString()) ? "0" : Request.QueryString["pCC"].ToString();
                        pCC = cc;
                        string CentroCostosNombre = "";
                        if (pCC != "0")
                        {
                            CentroCostosNombre = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);
                        }

                        pTurno = int.Parse(Request.QueryString["pTurno"]);
                        pFechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        pFechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);
                        pEconomico = Request.QueryString["pEconomico"].ToString();
                        List<CapHorometroDTO> horometros = new List<CapHorometroDTO>();
                        horometros = (List<CapHorometroDTO>)Session["ReporteHorometro"];
                        //  DataTable c = ToDataTable(horometros);
                        rd.Database.Tables[1].SetDataSource(horometros.OrderBy(x => x.Fecha).ThenBy(x => x.Horometro).ThenBy(x => x.turno));

                        rd.SetParameterValue("nomCC", CentroCostosNombre);
                        rd.SetParameterValue("hCC", setTitleCC() + ":");
                        rd.SetParameterValue("hEco", setTitleEco() + ":");
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.captura_combustibles:
                    {
                        setMedidasReporte("VC");
                        rd = new rptCapturaCombustibles();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("REPORTE CAPTURA DE COMBUSTIBLES", "GERENCIA DE MAQUINARIA Y EQUIPO"));
                        var cc = string.IsNullOrEmpty(Request.QueryString["pCC"].ToString()) ? "0" : Request.QueryString["pCC"].ToString();
                        pCC = cc;
                        pTurno = int.Parse(Request.QueryString["pTurno"]);
                        pFechaInicio = Convert.ToDateTime(Request.QueryString["pFechaInicio"]);
                        pFechaFin = Convert.ToDateTime(Request.QueryString["pFechaFin"]);
                        pEconomico = Request.QueryString["pEconomico"].ToString();
                        string CentroCostosNombre = "";
                        if (pCC != "0")
                        {
                            CentroCostosNombre = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);
                        }

                        List<capCombusitbleDTO> combustibles = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getTableInfoCombustibles(pCC, pTurno, pFechaInicio, pFechaFin, pEconomico).Select(c => new capCombusitbleDTO
                        {

                            formatFecha = c.fecha.ToString("dd/MM/yyyy"),
                            Economico = c.Economico,
                            volumen_carga = c.volumne_carga,
                            turno = c.turno,
                            id = c.id,
                            Carga1 = c.Carga1
                        }).ToList();
                        string Litros = combustibles.Sum(x => x.volumen_carga).ToString("0,0.0", CultureInfo.InvariantCulture);


                        rd.Database.Tables[1].SetDataSource(ToDataTable(combustibles));
                        rd.SetParameterValue("nomCC", CentroCostosNombre);
                        rd.SetParameterValue("TotalLitros", Litros);
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper() + ":");
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.Solicitud_Equipo:
                    {
                        //setMedidasReporte("HorizontalCarta_NoModal");
                        setMedidasReporte("HO");
                        rd = new rptElaboracionSolicitudEquipo();
                        List<SolicitudEquipoDTO> rptData = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];

                        var autorizadores = (AutorizadoresIDDTO)Session["rptAutorizadores"];
                        var FolioSolicitud = "";

                        if (rptData != null)
                        {
                            FolioSolicitud = rptData.FirstOrDefault().Folio;
                        }
                        var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(autorizadores.usuarioElaboro)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorGerente = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.gerenteObra)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();


                        var AutorizadorGerenteDirector = usuarioFactoryServices.getUsuarioService()
                          .ListUsersById(autorizadores.GerenteDirector)
                          .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDirector = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.directorDivision)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDireccion = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadores.altaDireccion)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();


                        string CadenaDireccion = "";
                        string CadenaDirector = "";
                        string CadenaGerente = "";
                        string CadenaElabora = "";
                        string CadenaGerenteDirector = "";

                        if (Session["rptCadenaAutorizacion"] != null)
                        {
                            var cadena = (CadenaAutorizacionDTO)Session["rptCadenaAutorizacion"];

                            CadenaDireccion = cadena.CadenaDireccion == null ? "" : cadena.CadenaDireccion;
                            CadenaDirector = cadena.CadenaDirector == null ? "" : cadena.CadenaDirector;
                            CadenaGerente = cadena.CadenaGerente == null ? "" : cadena.CadenaGerente;
                            CadenaElabora = cadena.CadenaElabora == null ? "" : cadena.CadenaElabora;
                            CadenaGerenteDirector = cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector;


                        }
                        var CC = Request.QueryString["pCC"];

                        pCC = Request.QueryString["pCC"];

                        pNombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("SOLICITUD DE EQUIPO", "GERENCIA DE MAQUINARIA Y EQUIPO"));
                        rd.Database.Tables[1].SetDataSource(ToDataTable(rptData));
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper() + ":");
                        rd.SetParameterValue("CentroCostos", pNombreCC);
                        rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");

                        rd.SetParameterValue("Elaboro", AutorizadorElabora.nombre);
                        rd.SetParameterValue("Solicito", AutorizadorGerente.nombre);
                        rd.SetParameterValue("valido", AutorizadorDirector.nombre);
                        rd.SetParameterValue("autorizo", AutorizadorDireccion.nombre);
                        rd.SetParameterValue("Valido2", AutorizadorGerenteDirector.nombre);

                        rd.SetParameterValue("CadenaDireccion", CadenaGerenteDirector);
                        rd.SetParameterValue("CadenaDirector", CadenaDirector);
                        rd.SetParameterValue("CadenaGerente", CadenaGerente);
                        rd.SetParameterValue("CadenaElabora", CadenaElabora);
                        rd.SetParameterValue("CadenaGerenteDirector", CadenaGerenteDirector);

                        rd.SetParameterValue("FolioDocumento", FolioSolicitud);


                        Session.Add("reporte", rd);
                    }
                    break;
                case ReportesEnum.Formato_Cambio:
                    {
                        setMedidasReporte("VC");
                        rd = new rptFormatoCambio();
                        var dto = new List<tblRH_AutorizacionFormatoCambio>();
                        var objEmploados = capturaFormatoCambioFactoryServices.getFormatoCambioService().getListFormatosCambioPendientes(int.Parse(Request.QueryString["fId"]), "", 0, 1, "", 0);
                        var objEmploadosEnkontrol = new tblRH_FormatoCambio()
                        {
                            Ape_Materno = objEmploados.FirstOrDefault().Ape_Materno,
                            Ape_Paterno = objEmploados.FirstOrDefault().Ape_Paterno,
                            Aprobado = objEmploados.FirstOrDefault().Aprobado,
                            Bono = objEmploados.FirstOrDefault().Bono,
                            BonoAnt = objEmploados.FirstOrDefault().BonoAnt,
                            CCAnt = objEmploados.FirstOrDefault().CCAnt,
                            CCAntID = objEmploados.FirstOrDefault().CCAntID,
                            ComplementoAnt = objEmploados.FirstOrDefault().ComplementoAnt,
                            Fecha_Alta = objEmploados.FirstOrDefault().Fecha_Alta,
                            PuestoAnt = objEmploados.FirstOrDefault().PuestoAnt,
                            SalarioAnt = objEmploados.FirstOrDefault().SalarioAnt,
                            CamposCambiados = objEmploados.FirstOrDefault().CamposCambiados,
                            CcID = objEmploados.FirstOrDefault().CcID,
                            Clave_Empleado = objEmploados.FirstOrDefault().Clave_Empleado,
                            Clave_Jefe_Inmediato = objEmploados.FirstOrDefault().Clave_Jefe_Inmediato,
                            Complemento = objEmploados.FirstOrDefault().Complemento,
                            editable = objEmploados.FirstOrDefault().editable,
                            fechaCaptura = objEmploados.FirstOrDefault().fechaCaptura,
                            FechaInicioCambio = objEmploados.FirstOrDefault().FechaInicioCambio,
                            folio = objEmploados.FirstOrDefault().folio,
                            id = objEmploados.FirstOrDefault().id,
                            InicioNomina = objEmploados.FirstOrDefault().InicioNomina,
                            Justificacion = objEmploados.FirstOrDefault().Justificacion,
                            Nombre = objEmploados.FirstOrDefault().Nombre,
                            Nombre_Jefe_Inmediato = objEmploados.FirstOrDefault().Nombre_Jefe_InmediatoAnt,
                            nomUsuarioCap = objEmploados.FirstOrDefault().nomUsuarioCap,
                            PuestoID = objEmploados.FirstOrDefault().PuestoID,
                            Rechazado = objEmploados.FirstOrDefault().Rechazado,
                            RegistroPatronal = objEmploados.FirstOrDefault().RegistroPatronalAnt,
                            RegistroPatronalID = objEmploados.FirstOrDefault().RegistroPatronalID,
                            Salario_Base = objEmploados.FirstOrDefault().Salario_Base,
                            TipoNomina = objEmploados.FirstOrDefault().TipoNominaAnt,
                            TipoNominaID = objEmploados.FirstOrDefault().TipoNominaID,
                            usuarioCap = objEmploados.FirstOrDefault().usuarioCap,
                            Puesto = objEmploados.FirstOrDefault().PuestoAnt,
                            CC = objEmploados.FirstOrDefault().CCAntID + "-" + objEmploados.FirstOrDefault().CCAnt,
                            TipoNominaAnt = objEmploados.FirstOrDefault().TipoNominaAnt
                        };
                        string TipoNomina = "";
                        string SalarioMesEmpleadoCambio = "";
                        string ComplementoMesEmpleadoCambio = "";
                        string BonoMesEmpleadoCambio = "";

                        foreach (var objEmp in objEmploados)
                        {
                            objEmp.CC = objEmp.CcID + " - " + objEmp.CC;
                            if (objEmp.TipoNominaID == (int)Tipo_NominaEnum.SEMANAL)
                            {
                                TipoNomina = "Semana";

                                SalarioMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Salario_Base) / 7) * 30.4), 2).ToString();
                                ComplementoMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Complemento) / 7) * 30.4), 2).ToString();
                                BonoMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Bono) / 7) * 30.4), 2).ToString();
                            }
                            else
                            {
                                TipoNomina = "Quincena";

                                SalarioMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Salario_Base) * 2), 2).ToString();
                                ComplementoMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Complemento) * 2), 2).ToString();
                                BonoMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Bono) * 2), 2).ToString();
                            }
                            var objAutorizacion = capturaAutorizacionFormatoCambioService.getAutorizacionFormatoCambioService().getAutorizacion(objEmp.id);
                            int formatoCambio = 0;
                            if (objAutorizacion.Count > 0)
                                formatoCambio = objAutorizacion.FirstOrDefault().Id_FormatoCambio;
                            var dto2 = new List<tblRH_AutorizacionFormatoCambio>();
                            for (int i = 1; i <= 8; i++)
                            {
                                var d = new tblRH_AutorizacionFormatoCambio();
                                d.Orden = i;
                                switch (i)
                                {
                                    case 1:
                                        d.Responsable = "Solicitante";
                                        d.PuestoAprobador = "Responsable del Centro de Costos";
                                        d.Orden = 1;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;

                                        break;
                                    case 2:
                                        d.Responsable = "VoBo";
                                        d.PuestoAprobador = "Recursos Humanos";
                                        d.Orden = 8;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 3:
                                        d.Responsable = "Autorización 1";
                                        d.PuestoAprobador = "Gerente / Director de Area 1";
                                        d.Orden = 2;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 4:
                                        d.Responsable = "Autorización 1";
                                        d.PuestoAprobador = "Gerente / Director de Área  2";
                                        d.Orden = 3;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 5:
                                        d.Responsable = "Autorización 2";
                                        d.PuestoAprobador = "Director de División 1";
                                        d.Orden = 4;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 6:
                                        d.Responsable = "Autorización 2";
                                        d.PuestoAprobador = "Director de División 2";
                                        d.Orden = 5;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 7:
                                        d.Responsable = "Autorización 3";
                                        d.PuestoAprobador = "Alta Dirección 1";
                                        d.Orden = 6;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 8:
                                        d.Responsable = "Autorización 3";
                                        d.PuestoAprobador = "Alta Dirección 2";
                                        d.Orden = 7;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    default:
                                        break;
                                }
                                dto2.Add(d);
                            }
                            bool bandera = false;

                            for (int i = 0; i < dto2.ToArray().Length; i++)
                            {
                                bandera = false;
                                foreach (var item in objAutorizacion)
                                {
                                    if (dto2[i].Responsable == item.Responsable && dto2[i].PuestoAprobador == item.PuestoAprobador)
                                    {
                                        item.PuestoAprobador = item.PuestoAprobador.TrimEnd('1');
                                        item.PuestoAprobador = item.PuestoAprobador.TrimEnd('2');

                                        dto2[i] = item;
                                        dto.Add(dto2[i]);

                                        bandera = true;
                                    }
                                }

                                if (!bandera)
                                {
                                    dto2[i].PuestoAprobador = dto2[i].PuestoAprobador.TrimEnd('1');
                                    dto2[i].PuestoAprobador = dto2[i].PuestoAprobador.TrimEnd('2');
                                    dto.Add(dto2[i]);
                                }

                            }

                        }
                        string SalarioMesEmpleadoOriginal = "";
                        string ComplementoMesEmpleadoOriginal = "";
                        string BonoMesEmpleadoOriginal = "";
                        if ((objEmploadosEnkontrol.TipoNominaAnt == "QUINCENAL" ? 4 : 1) == (int)Tipo_NominaEnum.SEMANAL)
                        {

                            SalarioMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.SalarioAnt) / 7) * 30.4), 2).ToString();
                            ComplementoMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) / 7) * 30.4), 2).ToString();
                            BonoMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.BonoAnt) / 7) * 30.4), 2).ToString();

                        }
                        else
                        {
                            BonoMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.BonoAnt) * 2), 2).ToString();
                            SalarioMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.SalarioAnt) * 2), 2).ToString();
                            ComplementoMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) * 2), 2).ToString();

                            // setParametro("SalarioMesEmpleadoOriginal", );
                            // setParametro("ComplementoMesEmpleadoOriginal", Math.Round((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) * 2), 2));
                        }


                        var lstEnkontrol = new List<tblRH_FormatoCambio>() { objEmploadosEnkontrol };
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("FORMATO DE CAMBIO", "RECURSOS HUMANOS"));
                        rd.Database.Tables[1].SetDataSource(dto.ToList());
                        rd.Database.Tables[2].SetDataSource(ToDataTable(objEmploados));
                        rd.Database.Tables[3].SetDataSource(ToDataTable(lstEnkontrol));

                        rd.SetParameterValue("TipoNomina", TipoNomina);
                        rd.SetParameterValue("SalarioMesEmpleadoCambio", SalarioMesEmpleadoCambio);
                        rd.SetParameterValue("ComplementoMesEmpleadoCambio", ComplementoMesEmpleadoCambio);
                        rd.SetParameterValue("BonoMesEmpleadoCambio", BonoMesEmpleadoCambio);
                        rd.SetParameterValue("SalarioMesEmpleadoOriginal", SalarioMesEmpleadoOriginal);
                        rd.SetParameterValue("ComplementoMesEmpleadoOriginal", ComplementoMesEmpleadoOriginal);
                        rd.SetParameterValue("BonoMesEmpleadoOriginal", BonoMesEmpleadoOriginal);
                        rd.SetParameterValue("NombreCompleto", objEmploadosEnkontrol.Nombre + " " + objEmploadosEnkontrol.Ape_Paterno + " " + objEmploadosEnkontrol.Ape_Materno);
                        rd.SetParameterValue("hCC", setTitleCC() + ":");


                        Session.Add("reporte", rd);
                    }
                    break;
                case ReportesEnum.Solicitud_Equipo_Asignado:
                    {
                        setMedidasReporte("HorizontalCarta_NoModal");
                        rd = new rptElaboracionSolicitudEquipoAsignado();
                        var autorizadoresA = (AutorizadoresIDDTO)Session["rptAutorizadores"];
                        List<SolicitudEquipoDTO> rptData1 = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];
                        var FolioSolicitud = "";

                        if (rptData1 != null)
                        {
                            FolioSolicitud = rptData1.FirstOrDefault().Folio;
                        }

                        //falta nueva autorizacion....
                        var Asigna = Session["rptAsigna"] != null ? Session["rptAsigna"].ToString() : "";
                        string nombre = "";
                        if (Asigna != null)
                        {
                            nombre = Asigna;
                        }


                        var AutorizadorElaboraA = usuarioFactoryServices.getUsuarioService().ListUsersById(autorizadoresA.usuarioElaboro)
                               .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorGerenteA = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadoresA.gerenteObra)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDirectorA = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadoresA.directorDivision)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorGerenteDirector = usuarioFactoryServices.getUsuarioService()
                          .ListUsersById(autorizadoresA.GerenteDirector)
                          .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                        var AutorizadorDireccionA = usuarioFactoryServices.getUsuarioService()
                            .ListUsersById(autorizadoresA.altaDireccion)
                            .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();
                        pCC = Request.QueryString["pCC"];

                        pNombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("SOLICITUD DE EQUIPO", "DIRECCION DE MAQUINARIA Y EQUIPO"));
                        rd.Database.Tables[1].SetDataSource(ToDataTable(rptData1));


                        if (Session["rptCadenaAutorizacion"] != null)
                        {
                            var cadena = (CadenaAutorizacionDTO)Session["rptCadenaAutorizacion"];

                            rd.SetParameterValue("CadenaDireccion", cadena.CadenaDireccion == null ? "" : cadena.CadenaDireccion);
                            rd.SetParameterValue("CadenaDirector", cadena.CadenaDirector == null ? "" : cadena.CadenaDirector);
                            rd.SetParameterValue("CadenaGerente", cadena.CadenaGerente == null ? "" : cadena.CadenaGerente);
                            rd.SetParameterValue("CadenaElabora", cadena.CadenaElabora == null ? "" : cadena.CadenaElabora);
                            rd.SetParameterValue("CadenaGerenteDirector", cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector);
                        }
                        else
                        {
                            rd.SetParameterValue("CadenaDireccion", "");
                            rd.SetParameterValue("CadenaDirector", "");
                            rd.SetParameterValue("CadenaGerente", "");
                            rd.SetParameterValue("CadenaElabora", "");
                            rd.SetParameterValue("CadenaGerenteDirector", "");
                        }

                        rd.SetParameterValue("CentroCostos", pNombreCC);
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper() + ":");
                        rd.SetParameterValue("Elaboro", AutorizadorElaboraA.nombre);
                        rd.SetParameterValue("Solicito", AutorizadorGerenteA.nombre);
                        rd.SetParameterValue("valido", AutorizadorDirectorA.nombre);
                        rd.SetParameterValue("autorizo", AutorizadorDireccionA.nombre);
                        rd.SetParameterValue("Asigno", nombre);
                        rd.SetParameterValue("Valido2", AutorizadorGerenteDirector.nombre);
                        rd.SetParameterValue("FolioDocumento", FolioSolicitud);

                        rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");


                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.CONTROLENVIORECEPCION:
                    {

                        setMedidasReporte("VC");
                        ;
                        rd = new rptControlEnvioRecepcion();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Control de envío / recepción", "Dirección de Maquinaria y Equipo"));
                        var pidControl = int.Parse(Request.QueryString["pidRegistro"]);
                        var tipoControl = int.Parse(Request.QueryString["ptipoControl"]);

                        Session["downloadPDF"] = null;


                        string nombreCC = "";
                        var dto = controlEnvioyRecepcionFactoryServices.getControlEnvioyRecepcionFactoryServices().getReporteEnvio(pidControl);
                        var controlCalida = ControlCalidadService.getControlCalidadFactoryServices().getByIDAsignacion(dto.asignacionEquipoId);
                        var Asignacion = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetAsiganacionById(dto.asignacionEquipoId);

                        var Economico = maquinaFactoryServices.getMaquinaServices().GetMaquina(dto.noEconomico);
                        var Clase = Economico.grupoMaquinaria.descripcion;
                        var noEconomico = Economico.noEconomico;

                        if (dto.lugar == "1010")
                        {
                            nombreCC = "TALLER MAQUINARIA CENTRAL";
                        }
                        else if (dto.lugar == "1015")
                        {
                            nombreCC = "PATIO MAQUINARIA HERMOSILLO";
                        }
                        else
                        {
                            nombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(dto.lugar);

                        }
                        var marca = Economico.marca != null ? Economico.marca.descripcion : "";
                        var serie = Economico.noSerie;
                        var modelo = Economico.modeloEquipo != null ? Economico.modeloEquipo.descripcion : "";


                        string REnvio = ""; //Reponsable Envio
                        string Rrecepcion = "";//Responsable Recepcion
                        int kmE = 0; // Kilometraje Envio
                        decimal horometrosE = 0; //Horometros Envio
                        int kmR = 0; // Kilometraje Recepcion
                        decimal horometrosR = 0; //Kilometraje Recepcion
                        string FechaEnvio = ""; //Fecha Envio
                        string FechaRecepcion = ""; // Fecha Recepcion
                        string CompaniaEnvio = "";
                        string CompaniaRecpecion = "";

                        if (tipoControl == 1 || tipoControl == 3)
                        {
                            REnvio = dto.nombreResponsable;
                            kmE = dto.kilometraje;
                            horometrosE = dto.horometros;
                            FechaEnvio = dto.fechaRecepcionEmbarque.ToString("dd/MM/yyyy");
                            CompaniaEnvio = dto.compañiaResponsable;

                        }
                        else
                        {

                            var ob = controlEnvioyRecepcionFactoryServices.getControlEnvioyRecepcionFactoryServices().getReporteRecepcion(dto.noEconomico, dto.solicitudEquipoID,tipoControl);
                            Rrecepcion = dto.nombreResponsable;
                            kmR = dto.kilometraje;
                            horometrosR = dto.horometros;
                            FechaRecepcion = dto.fechaRecepcionEmbarque.ToString("dd/MM/yyyy");

                            REnvio = ob == null ? "" : ob.nombreResponsable;
                            kmE = ob == null ? 0 : ob.kilometraje;
                            horometrosE = ob == null ? 0 : ob.horometros;
                            FechaEnvio = ob == null ? DateTime.Now.ToString("dd/MM/yyyy") : ob.fechaRecepcionEmbarque.ToString("dd/MM/yyyy");
                            CompaniaRecpecion = ob == null ? "" : dto.compañiaResponsable;
                            CompaniaEnvio = ob == null ? "" : ob.compañiaResponsable;

                        }
                        ControlEnvioRecepcionDTO controldto = new ControlEnvioRecepcionDTO
                        {


                            Aduana = dto.pedAduana ? "SI" : "NO",
                            Bitacora = dto.bitacora ? "SI" : "NO",
                            //  CatPartes = "SI",//dto.catpartes
                            Clase = Clase,
                            CompaniaEnvio = dto.compañiaResponsableEnvio,
                            CompaniaRecepcion = dto.compañiaResponsableRecepcion,
                            CompaniaTransporte = dto.companiaTransporte,
                            ControlCalidad = dto.controlCalidad ? "SI" : "NO",
                            CopiaFactura = dto.copiaFactura ? "SI" : "NO",
                            DiasTransalado = dto.diasTranslado.ToString(),
                            Economico = noEconomico,
                            Fecha = dto.fechaElaboracion.ToString("dd/MM/yyyy"),

                            FechaEmbarque = FechaEnvio,
                            FechaRecepcion = FechaRecepcion,

                            HorometroEnvio = horometrosE.ToString(),
                            HorometroRecepcion = horometrosR.ToString(),

                            KmEnvio = kmE.ToString(),
                            KmRecepcion = kmR.ToString(),

                            Lugar = nombreCC,
                            ManualMantto = dto.manualMant ? "SI" : "NO",
                            ManualOperacion = dto.manualOperacion ? "SI" : "NO",
                            ManualServicios = "NO",
                            CatPartes = "NO",
                            Marca = marca,
                            Modelo = modelo,
                            NombreEnvio = dto.nombreResponsableEnvio,
                            NombreRecepcion = dto.nombreResponsableRecepcion,
                            Notas = dto.nota,

                            Placas = dto.placas ? "SI" : "NO",
                            ReporteFalla = dto.ReporteFalla ? "SI" : "NO",
                            ResponsableTransporte = dto.responsableTrasnporte,
                            Serie = serie,
                            Tanque1 = dto.tanque1.ToString(),
                            Tanque2 = dto.tanque2.ToString(),
                            TipoControl = dto.tipoControl == 1 || dto.tipoControl == 3 ? "E" : "R",
                            Transporte = dto.Transporte

                        };


                        List<ControlEnvioRecepcionDTO> listRes = new List<ControlEnvioRecepcionDTO>();
                        listRes.Add(controldto);
                        rd.Database.Tables[1].SetDataSource(listRes);

                        if (tipoControl <= 2)
                        {
                            rd.SetParameterValue("LugarRecepcion", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(Asignacion.cc));
                            rd.SetParameterValue("LugarEnvio", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(Asignacion.CCOrigen));
                        }
                        else
                        {
                            if (Economico.estatus != 0)
                            {
                                rd.SetParameterValue("LugarRecepcion", nombreCC = "PATIO MAQUINARIA HERMOSILLO");
                                rd.SetParameterValue("LugarEnvio", nombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(dto.lugar));
                            }
                            else
                            {
                                rd.SetParameterValue("LugarRecepcion", dto.compañiaResponsable);
                                rd.SetParameterValue("LugarEnvio", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(dto.lugar));
                            }

                        }
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("Fecha", dto.fechaElaboracion.ToShortDateString());
                        rd.SetParameterValue("txtLugarEnvio", "Lugar de envio o recepción");
                        rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RHREPALTAS:
                    {
                        setMedidasReporte("VC");
                        rd = new rptAltas();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Altas de empleado por " + setTitleCC() + " del:", "Recursos Humanos"));
                        var oReportData = JsonConvert.DeserializeObject<List<RepAltasDTO>>(Session["cr" + ReportesEnum.RHREPALTAS].ToString());
                        string fechaInicio = Request.QueryString["fechaInicio"];
                        string fechaFin = Request.QueryString["fechaFin"];

                        List<string> ccs = oReportData.Select(c => c.cC).Distinct().ToList();


                        rd.Database.Tables[1].SetDataSource(ToDataTable(oReportData));

                        rd.SetParameterValue("hCC", setTitleCC() + ":");
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));
                        rd.SetParameterValue("Inicio", fechaInicio);
                        rd.SetParameterValue("Termino", fechaFin);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RHREPBAJAS:
                    {
                        setMedidasReporte("HC");
                        rd = new rptBajas();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Baja de empleado por " + setTitleCC() + " del:", "Recursos Humanos"));
                        var oReportData = JsonConvert.DeserializeObject<List<RepBajasDTO>>(Session["cr" + ReportesEnum.RHREPBAJAS].ToString());
                        string fechaInicio = Request.QueryString["fechaInicio"];
                        string fechaFin = Request.QueryString["fechaFin"];

                        List<string> ccs = oReportData.Select(c => c.cC).Distinct().ToList();
                        rd.Database.Tables[1].SetDataSource(ToDataTable(oReportData));

                        rd.SetParameterValue("hCC", setTitleCC() + ":");
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));
                        rd.SetParameterValue("Inicio", fechaInicio);
                        rd.SetParameterValue("Termino", fechaFin);


                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RHREPCAMBIOS:
                    {
                        setMedidasReporte("HC");
                        rd = new rptCambios();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Cambios realizador por " + setTitleCC() + " del:", "Recursos Humanos"));
                        var oReportData = JsonConvert.DeserializeObject<List<RepCambiosDTO>>(Session["cr" + ReportesEnum.RHREPCAMBIOS].ToString());
                        string fechaInicio = Request.QueryString["fechaInicio"];
                        string fechaFin = Request.QueryString["fechaFin"];

                        List<string> ccs = oReportData.Select(c => c.cC).Distinct().ToList();


                        rd.Database.Tables[1].SetDataSource(ToDataTable(oReportData));
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));
                        rd.SetParameterValue("Inicio", fechaInicio);
                        rd.SetParameterValue("Termino", fechaFin);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RHREPMODIFICACIONES:
                    {
                        setMedidasReporte("HC");
                        rd = new rptModificaciones();
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Modificaciones a plantilla por " + setTitleCC() + " del:", "Recursos Humanos"));
                        var oReportData = JsonConvert.DeserializeObject<List<RepModificacionesDTO>>(Session["cr" + ReportesEnum.RHREPMODIFICACIONES].ToString());
                        string fechaInicio = Request.QueryString["fechaInicio"];
                        string fechaFin = Request.QueryString["fechaFin"];

                        List<string> ccs = oReportData.Select(c => c.cC).Distinct().ToList();

                        rd.Database.Tables[1].SetDataSource(ToDataTable(oReportData));
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));
                        rd.SetParameterValue("Inicio", fechaInicio);
                        rd.SetParameterValue("Termino", fechaFin);


                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.RHREPINCIDENCIAS:
                    {
                        setMedidasReporte("VC");
                        rd = new rptIncidencias();

                        var oReportData = JsonConvert.DeserializeObject<List<CatCantIncidencias>>(Session["cr" + ReportesEnum.RHREPINCIDENCIAS].ToString());
                        rd.Database.Tables[0].SetDataSource(ToDataTable(oReportData));

                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Incidencias cometidas por " + setTitleCC(), "Recursos Humanos"));

                        List<string> ccs = oReportData.Select(c => c.CC).Distinct().ToList();

                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));

                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.RHREPDETINCIDENCIAS:
                    {
                        setMedidasReporte("VC");
                        rd = new rptDetalleIncidencias();

                        var oReportData = JsonConvert.DeserializeObject<List<catDetalleIncidencias>>(Session["cr" + ReportesEnum.RHREPDETINCIDENCIAS].ToString());
                        rd.Database.Tables[0].SetDataSource(ToDataTable(oReportData));

                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Incidencias por fecha cometidas por empleados", "Recursos Humanos"));

                        List<string> ccs = oReportData.Select(c => c.cc).Distinct().ToList();
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));

                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.RHREPACTIVOS:
                    {
                        setMedidasReporte("HC");
                        rd = new rptActivos();

                        var oReportData = JsonConvert.DeserializeObject<List<RepActivosDTO>>(Session["cr" + ReportesEnum.RHREPACTIVOS].ToString());

                        List<string> ccs = oReportData.Select(c => c.cC).Distinct().ToList();

                        rd.Database.Tables[0].SetDataSource(ToDataTable(oReportData));

                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Reporte Activos", "Recursos Humanos"));
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        rd.SetParameterValue("CCs", string.Join(",", ccs));


                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.MRepFichaTecnica:
                    {
                        setMedidasReporte("VC");
                        rd = new rptFichaTecnica();

                        var oReportData = JsonConvert.DeserializeObject<List<FichaTecnicaDTO>>(Session["cr" + ReportesEnum.MRepFichaTecnica].ToString());
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Ficha Tecnica", "Maquinaria"));

                        rd.Database.Tables[1].SetDataSource(oReportData);
                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.MConsumoDiesel:
                    {
                        setMedidasReporte("HC");
                        rd = new rptConsumoDiesel();

                        var oReportData = JsonConvert.DeserializeObject<List<ConsumoDieselDTO>>(Session["cr" + ReportesEnum.MConsumoDiesel].ToString());
                        rd.Database.Tables[0].SetDataSource(oReportData);
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("Consumo de Diesel", "Maquinaria"));

                        rd.SetParameterValue("total1", Session["crTotalConsumo"]);
                        rd.SetParameterValue("total2", Session["crtotalEnKontrol"]);
                        rd.SetParameterValue("total3", Session["crtotalContratistas"]);
                        rd.SetParameterValue("total4", Session["crtotalProvisionar"]);
                        rd.SetParameterValue("CCs", Session["crtotalCCs"]);
                        rd.SetParameterValue("hCC", setTitleCC());
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.MControlCalidad:
                    {
                        setMedidasReporte("VC");
                        rd = new rptControlCalidad();

                        var idAsignacion = Int32.Parse(Request.QueryString["idAsignacion"]);
                        var TipoControl = Int32.Parse(Request.QueryString["TipoControl"]);

                        List<tblM_CatControlCalidad> controlCalidad = new List<tblM_CatControlCalidad>();
                        controlCalidad.Add(ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(idAsignacion, TipoControl));
                        var lstGrupos = GrupoPreguntasCalidad.getGrupoPreguntasFactoryServices().getListGrupoPreguntas();
                        var lstPreguntas = PreguntasCalidad.getPreguntasFactoryServices().getListPreguntasCalidad();
                        var objMaquina = controlEnvioyRecepcionFactoryServices.getControlEnvioyRecepcionFactoryServices().GetInfoMaquinaria(controlCalidad.FirstOrDefault().IdEconomico);
                        var lstRespuestas = RespuestasCalidadService.getRespuestasCalidadFactoryServices().getListRespuestasCalidad(controlCalidad.FirstOrDefault().id);
                        List<RespuestasCalidadDTO> LstRespuestaCompleto = new List<RespuestasCalidadDTO>();

                        int count = 0;

                        foreach (var objRespuesta in lstRespuestas)
                        {
                            RespuestasCalidadDTO objrespuestaCompleto = new RespuestasCalidadDTO();

                            objrespuestaCompleto.Pregunta = lstPreguntas[count].Pregunta;
                            objrespuestaCompleto.NoTiene = objRespuesta.Respuesta == 0 ? "X" : " ";
                            objrespuestaCompleto.Bueno = objRespuesta.Respuesta == 1 ? "X" : " ";
                            objrespuestaCompleto.Regular = objRespuesta.Respuesta == 2 ? "X" : " ";
                            objrespuestaCompleto.Malo = objRespuesta.Respuesta == 3 ? "X" : " ";
                            objrespuestaCompleto.Cantidad = objRespuesta.Cantidad.ToString();
                            objrespuestaCompleto.Serie = objRespuesta.Serie;
                            objrespuestaCompleto.Medida = objRespuesta.Medida;
                            objrespuestaCompleto.VidaUtil = objRespuesta.VidaUtil;
                            objrespuestaCompleto.Marca = objRespuesta.Marca;
                            objrespuestaCompleto.Grupo = lstGrupos.FirstOrDefault(x => x.Id == lstPreguntas[count].IdGrupo).Descripcion;
                            objrespuestaCompleto.TipoPregunta = lstPreguntas[count].TipoPregunta.ToString();
                            objrespuestaCompleto.idGrupo = lstGrupos.FirstOrDefault(x => x.Id == lstPreguntas[count].IdGrupo).Id.ToString();
                            objrespuestaCompleto.idGrupoInt = lstGrupos.FirstOrDefault(x => x.Id == lstPreguntas[count].IdGrupo).Id;

                            count++;
                            LstRespuestaCompleto.Add(objrespuestaCompleto);
                        }

                        List<RespuestasCalidadDTO> LstRespuestaCompleto1 = new List<RespuestasCalidadDTO>();
                        List<RespuestasCalidadDTO> LstRespuestaCompleto2 = new List<RespuestasCalidadDTO>();

                        foreach (var objGrupo in lstGrupos)
                        {
                            List<RespuestasCalidadDTO> LstRespuestaGrupo = LstRespuestaCompleto.Where(x => x.idGrupoInt == objGrupo.Id).ToList();

                            if (objGrupo.Id != 4)
                            {
                                int countGrupo = LstRespuestaGrupo.Count;
                                bool impar = (countGrupo % 2) == 1;

                                countGrupo = impar ? countGrupo + 1 : countGrupo;

                                int mitad = countGrupo / 2;
                                int aux = 1;
                                int auxBaterias = 1;


                                foreach (var objrespuesta in LstRespuestaGrupo)
                                {
                                    if (aux <= mitad)
                                    {
                                        if (objrespuesta.TipoPregunta.Equals("2"))
                                        {
                                            objrespuesta.noPregunta = (aux - 1).ToString() + "." + auxBaterias.ToString();
                                            auxBaterias++;
                                            aux--;
                                            mitad--;
                                        }
                                        else { objrespuesta.noPregunta = aux.ToString(); }

                                        LstRespuestaCompleto1.Add(objrespuesta);
                                    }
                                    else
                                    {
                                        objrespuesta.noPregunta = aux.ToString();
                                        LstRespuestaCompleto2.Add(objrespuesta);

                                        if (impar && aux == countGrupo - 1)
                                        {

                                            LstRespuestaCompleto2.Add(new RespuestasCalidadDTO() { idGrupo = objGrupo.Id.ToString(), idGrupoInt = objGrupo.Id });
                                        }
                                    }

                                    aux++;
                                }
                            }
                            else
                            {
                                count = 1;
                                foreach (var objrespuesta in LstRespuestaGrupo)
                                {
                                    objrespuesta.noPregunta = count.ToString();
                                    LstRespuestaCompleto1.Add(objrespuesta);

                                    LstRespuestaCompleto2.Add(new RespuestasCalidadDTO() { idGrupo = objGrupo.Id.ToString(), idGrupoInt = objGrupo.Id });

                                }
                            }

                        }

                        List<RespuestasCalidadDTO> lstRespuestaCompletaDividida = new List<RespuestasCalidadDTO>();
                        count = 0;
                        foreach (var obj in LstRespuestaCompleto1)
                        {
                            RespuestasCalidadDTO objrespuestaCompleto = new RespuestasCalidadDTO();
                            objrespuestaCompleto.noPregunta = obj.noPregunta;
                            objrespuestaCompleto.Pregunta = obj.Pregunta;
                            objrespuestaCompleto.NoTiene = obj.NoTiene;
                            objrespuestaCompleto.Bueno = obj.Bueno;
                            objrespuestaCompleto.Regular = obj.Regular;
                            objrespuestaCompleto.Malo = obj.Malo;
                            objrespuestaCompleto.Cantidad = obj.Cantidad;
                            objrespuestaCompleto.Serie = obj.Serie;
                            objrespuestaCompleto.Medida = obj.Medida;
                            objrespuestaCompleto.VidaUtil = obj.VidaUtil;
                            objrespuestaCompleto.Marca = obj.Marca;
                            objrespuestaCompleto.Grupo = obj.Grupo;
                            objrespuestaCompleto.TipoPregunta = obj.TipoPregunta;
                            objrespuestaCompleto.idGrupo = obj.idGrupo;
                            objrespuestaCompleto.idGrupoInt = Int32.Parse(obj.idGrupo);
                            objrespuestaCompleto.noPregunta2 = LstRespuestaCompleto2[count].noPregunta;
                            objrespuestaCompleto.Pregunta2 = LstRespuestaCompleto2[count].Pregunta;
                            objrespuestaCompleto.NoTiene2 = LstRespuestaCompleto2[count].NoTiene;
                            objrespuestaCompleto.Bueno2 = LstRespuestaCompleto2[count].Bueno;
                            objrespuestaCompleto.Regular2 = LstRespuestaCompleto2[count].Regular;
                            objrespuestaCompleto.Malo2 = LstRespuestaCompleto2[count].Malo;
                            objrespuestaCompleto.Cantidad2 = LstRespuestaCompleto2[count].Cantidad;
                            objrespuestaCompleto.Serie2 = LstRespuestaCompleto2[count].Serie;
                            objrespuestaCompleto.Medida2 = LstRespuestaCompleto2[count].Medida;
                            objrespuestaCompleto.VidaUtil2 = LstRespuestaCompleto2[count].VidaUtil;
                            objrespuestaCompleto.Marca2 = LstRespuestaCompleto2[count].Marca;

                            count++;
                            lstRespuestaCompletaDividida.Add(objrespuestaCompleto);

                        }


                        string descripcion = "";
                        string marcaDEscripcion = "";
                        string modeloDescripcion = "";
                        string serieDescripcion = "";

                        if (objMaquina != null)
                        {
                            descripcion = objMaquina.descripcion;
                            marcaDEscripcion = objMaquina.modeloEquipo.descripcion;
                            modeloDescripcion = objMaquina.modeloEquipo.descripcion;
                            serieDescripcion = objMaquina.noSerie;
                        }

                        string recepcion = " ";
                        string Envio = "";

                        if (TipoControl == 1 || TipoControl == 3)
                        {
                            Envio = "X";
                            recepcion = "";
                        }
                        else
                        {
                            Envio = " ";
                            recepcion = "X";
                        }


                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Control de Calidad", "Direccion Maquinaria y Equipo"));
                        rd.Database.Tables[1].SetDataSource(controlCalidad);
                        rd.Database.Tables[2].SetDataSource(lstRespuestaCompletaDividida);

                        rd.SetParameterValue("Descripcion", descripcion);
                        rd.SetParameterValue("Marca", marcaDEscripcion);
                        rd.SetParameterValue("Modelo", modeloDescripcion);
                        rd.SetParameterValue("Serie", serieDescripcion);
                        rd.SetParameterValue("Recepcion", recepcion);
                        rd.SetParameterValue("Envio", Envio);
                        rd.SetParameterValue("hEco", setTitleEco());
                        rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ReporteProvicional:
                    {
                        setMedidasReporte("HO");
                        rd = new rptProvision();

                        var vCC = Convert.ToInt32(Request.QueryString["CC"]);
                        var vFechaCorte = Convert.ToDateTime(Request.QueryString["fechaCorte"]);
                        var vTC = Convert.ToDecimal(Request.QueryString["TC"]);
                        var vTodoReporte = Convert.ToBoolean(Request.QueryString["todoReporte"]);

                        var centroCosto = centroCostosFactoryServices.getCentroCostosService().getNombreCC(vCC);
                        var data = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getRptProvisionalInfo(vCC, vFechaCorte, vTC, vTodoReporte);

                        var Equipos = (List<RepProvisionDTO>)data["lstEquipos"];
                        var TotalDefault = new totalProvisionDTO()
                        {
                            strTC = "$0.00",
                            strTotalDlls = "$0.00",
                            strTotalMN = "$0.00",
                            strTotalPesos = "$0.00"
                        };
                        var Total = TotalDefault;
                        var TotalExtra = TotalDefault;
                        var Resumen = TotalDefault;
                        if (Equipos.Count == 0)
                        {
                            Equipos.Add(new RepProvisionDTO()
                            {
                                Equipo = string.Empty,
                                NoEconomico = string.Empty,
                                Moneda = string.Empty,
                                Inicio = string.Empty,
                                Termino = string.Empty,
                                strCostoRenta = "$0.00",
                                strPU = "$0.00",
                                strImporteConsumido = "$0.00",
                                strImporteTotal = "$0.00",
                                FacturaExtra = string.Empty,
                                strPUHrsExtra = "$0.00",
                                strImporteConsumidoExtra = "$0.00",
                                strImporteTotalExtra = "$0.00",
                                strAnotacion = string.Empty
                            }

                            );
                        }
                        else
                        {
                            Total = (totalProvisionDTO)data["lstTotal"];
                            TotalExtra = (totalProvisionDTO)data["lstTotalExtra"];
                            Resumen = (totalProvisionDTO)data["lstResumen"];
                        }

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("EQUIPO DE RENTA " + centroCosto, vFechaCorte.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(Equipos);
                        rd.SetParameterValue("TotalDlls", Total.strTotalDlls);
                        rd.SetParameterValue("TotalTC", Total.strTC);
                        rd.SetParameterValue("TotalPesos", Total.strTotalPesos);
                        rd.SetParameterValue("TotalMN", Total.strTotalMN);
                        rd.SetParameterValue("ExtralDlls", TotalExtra.strTotalDlls);
                        rd.SetParameterValue("ExtralTC", TotalExtra.strTC);
                        rd.SetParameterValue("ExtralPesos", TotalExtra.strTotalPesos);
                        rd.SetParameterValue("ExtralMN", TotalExtra.strTotalMN);
                        rd.SetParameterValue("ResumenDlls", Resumen.strTotalDlls);
                        rd.SetParameterValue("ResumenTC", Resumen.strTC);
                        rd.SetParameterValue("ResumenPesos", Resumen.strTotalPesos);
                        rd.SetParameterValue("ResumenMN", Resumen.strTotalMN);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.MovimientoAbierto:
                    {
                        setMedidasReporte("HO");
                        rd = new MovimientoAlmacen();

                        var vCC = Request.QueryString["CC"];
                        var vFolio = Request.QueryString["folio"];
                        var vAlmacen = Request.QueryString["almacen"];
                        var vfechaIni = Convert.ToDateTime(Request.QueryString["fechaIni"]);
                        var vvFechaFin = Convert.ToDateTime(Request.QueryString["fechaFin"]);
                        string strCC, strFolio, strAlmacen;

                        if (vCC != null) { strCC = vCC; } else { strCC = ""; }
                        if (vFolio != null) { strFolio = vFolio; } else { strFolio = ""; }
                        if (vAlmacen != null) { strAlmacen = vAlmacen; } else { strAlmacen = ""; }

                        var abierto = RepTraspasoFactoryService.getRepTraspasoServices().getLstMovAbiertos(strCC, strFolio, strAlmacen, vfechaIni, vvFechaFin);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("TRASPASOS ABIERTOS DE ALMACEN", DateTime.Today.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(abierto);
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.MovimientoCerado:
                    {
                        setMedidasReporte("HO");
                        rd = new MovimientoCerrado();

                        var vCC = Request.QueryString["CC"];
                        var vFolio = Request.QueryString["folio"];
                        var vAlmacen = Request.QueryString["almacen"];
                        var vfechaIni = Convert.ToDateTime(Request.QueryString["fechaIni"]);
                        var vvFechaFin = Convert.ToDateTime(Request.QueryString["fechaFin"]);
                        string strCC, strFolio, strAlmacen;

                        if (vCC != null) { strCC = vCC; } else { strCC = ""; }
                        if (vFolio != null) { strFolio = vFolio; } else { strFolio = ""; }
                        if (vAlmacen != null) { strAlmacen = vAlmacen; } else { strAlmacen = ""; }

                        var cerrado = RepTraspasoFactoryService.getRepTraspasoServices().getLstMovCerrados(strCC, strFolio, strAlmacen, vfechaIni, vvFechaFin);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("TRASPASOS CERRADOS DE ALMACEN", DateTime.Today.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(cerrado);
                        rd.SetParameterValue("hCC", setTitleCC().ToUpper());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.EficienciaObra:
                    {
                        setMedidasReporte("HO");
                        rd = new rptEficienciaObra();

                        var vCC = Request.QueryString["CC"];
                        var vFechaInicio = Convert.ToDateTime(Request.QueryString["FechaInicio"]);
                        var vFechaFin = Convert.ToDateTime(Request.QueryString["FechaFin"]);

                        var eficiencia = EficienciaFactoryService.getEficienciaService().getEficienciaObraInfo(vFechaInicio, vFechaFin, vCC);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("GRUPO CONSTRUCCIONES PLANIFICADAS, S.A. DE C.V. \r\n ANALISIS SEMANAL DETALLADO DE EFICIENCIA DE MAQUINARIA", eficiencia[0].Semana + "\r\n" + DateTime.Today.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(eficiencia);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.EficienciaGeneral:
                    {
                        setMedidasReporte("HO");
                        rd = new rptEficienciaGeneral();

                        var vCC = Request.QueryString["CC"];
                        var vFechaInicio = Convert.ToDateTime(Request.QueryString["FechaInicio"]);
                        var vFechaFin = Convert.ToDateTime(Request.QueryString["FechaFin"]);

                        var eficiencia = EficienciaFactoryService.getEficienciaService().getEficienciaGeneralInfo(vFechaInicio, vFechaFin);

                        RepEficienciaGeneralDTO TotalPromedio = new RepEficienciaGeneralDTO();
                        TotalPromedio.Frente = "EFICIENCIA PROMEDIO";
                        var cuenta1 = eficiencia.Where(x => x.Prom1 > 0);
                        var cuenta2 = eficiencia.Where(x => x.Prom2 > 0);
                        var cuenta3 = eficiencia.Where(x => x.Prom3 > 0);
                        var cuenta4 = eficiencia.Where(x => x.Prom4 > 0);
                        var cuenta5 = eficiencia.Where(x => x.Prom5 > 0);
                        var cuenta6 = eficiencia.Where(x => x.Prom6 > 0);
                        var cuenta7 = eficiencia.Where(x => x.Prom7 > 0);
                        var cuenta8 = eficiencia.Where(x => x.Prom8 > 0);
                        var cuenta9 = eficiencia.Where(x => x.Prom9 > 0);
                        var cuenta10 = eficiencia.Where(x => x.Prom10 > 0);
                        var cuenta11 = eficiencia.Where(x => x.Prom11 > 0);
                        var cuenta12 = eficiencia.Where(x => x.Prom12 > 0);
                        var cuenta13 = eficiencia.Where(x => x.Prom13 > 0);
                        var cuenta14 = eficiencia.Where(x => x.Prom14 > 0);
                        var cuenta15 = eficiencia.Where(x => x.Prom15 > 0);
                        var cuentaE = eficiencia.Where(x => x.Eficiencia > 0);

                        TotalPromedio.Prom1 = cuenta1.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom1) / cuenta1.Count();
                        TotalPromedio.Prom2 = cuenta2.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom2) / cuenta2.Count();
                        TotalPromedio.Prom3 = cuenta3.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom3) / cuenta3.Count();
                        TotalPromedio.Prom4 = cuenta4.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom4) / cuenta4.Count();
                        TotalPromedio.Prom5 = cuenta5.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom5) / cuenta5.Count();
                        TotalPromedio.Prom6 = cuenta6.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom6) / cuenta6.Count();
                        TotalPromedio.Prom7 = cuenta7.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom7) / cuenta7.Count();
                        TotalPromedio.Prom8 = cuenta8.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom8) / cuenta8.Count();
                        TotalPromedio.Prom9 = cuenta9.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom9) / cuenta9.Count();
                        TotalPromedio.Prom10 = cuenta10.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom10) / cuenta10.Count();
                        TotalPromedio.Prom11 = cuenta11.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom11) / cuenta11.Count();
                        TotalPromedio.Prom12 = cuenta12.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom12) / cuenta12.Count();
                        TotalPromedio.Prom13 = cuenta13.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom13) / cuenta13.Count();
                        TotalPromedio.Prom14 = cuenta14.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom14) / cuenta14.Count();
                        TotalPromedio.Prom15 = cuenta15.Count() == 0 ? 0 : eficiencia.Sum(x => x.Prom15) / cuenta15.Count();
                        TotalPromedio.Eficiencia = cuentaE.Count() == 0 ? 0 : eficiencia.Sum(x => x.Eficiencia) / cuentaE.Count();
                        var lstTotalProemdio = new List<RepEficienciaGeneralDTO>();
                        lstTotalProemdio.Add(TotalPromedio);
                        List<string> lstGrupo = new List<string>();
                        for (int i = 0; i < 15; i++)
                        {
                            try
                            {
                                var grupo = GrupoMaquinariaFactoryServices.getGrupoMaquinariaService().getDataGrupo(eficiencia.ToArray()[i].IdGrupo).prefijo;
                                lstGrupo.Add(grupo.Equals("") ? "-" : grupo.Trim());
                            }
                            catch (Exception)
                            {
                                lstGrupo.Add("-");
                            }
                        }

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("ANÁLISIS SEMANAL DE EFICIENCIA DE MAQUINARIA POR OBRA Y GRUPO DE EQUIPO", "\r\n" + DateTime.Today.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(eficiencia);
                        rd.Database.Tables[2].SetDataSource(lstTotalProemdio);
                        rd.SetParameterValue("Grupo1", lstGrupo[0]);
                        rd.SetParameterValue("Grupo2", lstGrupo[1]);
                        rd.SetParameterValue("Grupo3", lstGrupo[2]);
                        rd.SetParameterValue("Grupo4", lstGrupo[3]);
                        rd.SetParameterValue("Grupo5", lstGrupo[4]);
                        rd.SetParameterValue("Grupo6", lstGrupo[5]);
                        rd.SetParameterValue("Grupo7", lstGrupo[6]);
                        rd.SetParameterValue("Grupo8", lstGrupo[7]);
                        rd.SetParameterValue("Grupo9", lstGrupo[8]);
                        rd.SetParameterValue("Grupo10", lstGrupo[9]);
                        rd.SetParameterValue("Grupo11", lstGrupo[10]);
                        rd.SetParameterValue("Grupo12", lstGrupo[11]);
                        rd.SetParameterValue("Grupo13", lstGrupo[12]);
                        rd.SetParameterValue("Grupo14", lstGrupo[13]);
                        rd.SetParameterValue("Grupo15", lstGrupo[14]);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.IndicadorContrarecibo:
                    {
                        setMedidasReporte("HO");
                        rd = new rptIndicadorContrarecibo();

                        var vCC = Request.QueryString["CC"];
                        var lstCC = new List<string>();
                        lstCC.Add("");
                        var vFechaDel = Convert.ToDateTime(Request.QueryString["fechaDel"]);
                        var vfechaAl = Convert.ToDateTime(Request.QueryString["fechaAl"]);
                        var vConclusion = Request.QueryString["CRConclucion"];
                        var lst = new List<tblM_MaquinariaRentada>();

                        var objReporte = RptIndicadorFactoryServices.getRptIndicadorService().getReporte((int)ReportesEnum.IndicadorContrarecibo, vFechaDel, vfechaAl, vCC);
                        if (objReporte.id > 0)
                        {
                            var lstConsulta = JsonConvert.DeserializeObject<List<RepTblIndicadorContrarecibo>>(objReporte.datosJson);
                            foreach (var item in lstConsulta)
                            {
                                if (item.ContraRecibo.Equals(string.Empty) || item.ContraRecibo.ToUpper().Equals("N/A"))
                                {

                                }
                                else
                                {
                                    var obj = new tblM_MaquinariaRentada();
                                    obj.Folio = item.Obra;
                                    obj.RecepcionFactura = item.FechaRecepFact;
                                    obj.NoFactura = item.NoFactura;
                                    obj.ContraRecibo = item.ContraRecibo;
                                    obj.FechaContraRecibo = item.FechaCR;
                                    lst.Add(obj);

                                }
                            }
                        }
                        else
                        {
                            if (lstCC[0].Equals("148") || lstCC[0].Equals("114"))
                            {
                                lstCC[0] = "148";
                                var data148 = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentada(lstCC, string.Empty, vFechaDel, vfechaAl).ToList();
                                lstCC[0] = "114";
                                var data114 = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentada(lstCC, string.Empty, vFechaDel, vfechaAl).ToList();
                                lst.AddRange(data114);
                                lst.AddRange(data148);
                            }
                            else
                            {
                                lst = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentada(lstCC, string.Empty, vFechaDel, vfechaAl).ToList();
                            }
                        }



                        var lstTabla = new List<RepTblIndicadorContrarecibo>();
                        foreach (var item in lst)
                        {
                            if (item.ContraRecibo.Equals(string.Empty) || item.ContraRecibo.ToUpper().Equals("N/A"))
                            {

                            }
                            else
                            {
                                var obj = new RepTblIndicadorContrarecibo();
                                obj.Obra = item.Folio;
                                obj.FechaRecepFact = item.RecepcionFactura;
                                obj.NoFactura = item.NoFactura;
                                obj.ContraRecibo = item.ContraRecibo;
                                obj.FechaCR = item.FechaContraRecibo;
                                obj.Desface = CalcularDiasDeDiferencia(item.FechaContraRecibo, item.RecepcionFactura);
                                if (obj.Desface < 100 && obj.Desface > -1)
                                {
                                    lstTabla.Add(obj);
                                }
                            }
                        }

                        var save = new tblM_RptIndicador();
                        var jsonSerialiser = new JavaScriptSerializer();
                        save.id = objReporte.id;
                        save.Tipo = (int)ReportesEnum.IndicadorContrarecibo;
                        save.FechaInicio = vFechaDel;
                        save.FechaFin = vfechaAl;
                        save.datosJson = jsonSerialiser.Serialize(lstTabla);
                        save.Conclusion = vConclusion;
                        save.CC = vCC;

                        RptIndicadorFactoryServices.getRptIndicadorService().SaveReporte(save);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", vFechaDel.ToShortDateString() + "-" + vfechaAl.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(lstTabla);
                        rd.SetParameterValue("Conclucion", vConclusion);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.IndicadorProceso:
                    {
                        setMedidasReporte("HO");
                        rd = new rptIndicadorProceso();

                        var vCC = Request.QueryString["CC"];
                        var lstCC = new List<string>();
                        lstCC.Add(vCC);
                        var vFechaDel = Convert.ToDateTime(Request.QueryString["fechaDel"]);
                        var vfechaAl = Convert.ToDateTime(Request.QueryString["fechaAl"]);
                        var vConclusion = Request.QueryString["ProcespConclucion"];
                        var vTc = Convert.ToDouble(Request.QueryString["TC"]);

                        var lst = new List<tblM_MaquinariaRentada>();
                        var objReporte = RptIndicadorFactoryServices.getRptIndicadorService().getReporte((int)ReportesEnum.IndicadorProceso, vFechaDel, vfechaAl, vCC);
                        if (objReporte.id > 0)
                        {
                            var lstConsulta = JsonConvert.DeserializeObject<List<RepTblIndicadorContrarecibo>>(objReporte.datosJson);
                            foreach (var item in lstConsulta)
                            {
                                var obj = new tblM_MaquinariaRentada();
                                obj.NoEconomico = item.NoEconomico;
                                obj.PrecioMes = item.PrecioMes;
                                obj.Moneda = item.Moneda;
                                lst.Add(obj);
                            }
                        }
                        else
                        {
                            if (lstCC[0].Equals("148") || lstCC[0].Equals("114"))
                            {
                                lstCC[0] = "148";
                                var data148 = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentadaPorFacturacion(lstCC, vFechaDel, vfechaAl).ToList();
                                lstCC[0] = "114";
                                var data114 = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentadaPorFacturacion(lstCC, vFechaDel, vfechaAl).ToList();
                                lst.AddRange(data114);
                                lst.AddRange(data148);
                            }
                            else
                            {
                                lst = maquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getMaquinariaRentadaPorFacturacion(lstCC, vFechaDel, vfechaAl).ToList();
                            }
                        }



                        int i = 0;
                        var lstPesos = lst
                            .Where(dinero => dinero.Moneda == true)
                            .GroupBy(maquina => maquina.NoEconomico)
                            .Select(grafica => new RepTblIndicadorContrarecibo
                            {
                                Obra = grafica.FirstOrDefault().NoEconomico,
                                Desface = (double)grafica.FirstOrDefault().PrecioMes / 6
                                //Desface = i++
                            })
                            .ToList();
                        i = 0;
                        var lstDlls = lst
                            .Where(dinero => dinero.Moneda == false)
                            .GroupBy(maquina => maquina.NoEconomico)
                            .Select(grafica => new RepTblIndicadorContrarecibo
                            {
                                Obra = grafica.FirstOrDefault().NoEconomico,
                                Desface = (double)grafica.FirstOrDefault().PrecioMes / 24
                                //Desface = i++
                            })
                            .ToList();
                        var CentroCosto = lstCC[0].Equals(string.Empty) ? "" : centroCostosFactoryServices.getCentroCostosService().getNombreCC(Convert.ToInt32(vCC));

                        var totalPesos2 = lst.Where(x => x.Moneda == true).Sum(x => x.PrecioMes);
                        var totalDlls2 = lst.Where(x => x.Moneda == false).Sum(x => x.PrecioMes);
                        var totalDllsPesos = totalDlls2 * (decimal)vTc;
                        var totalTotal2 = totalPesos2 + totalDllsPesos;

                        var save = new tblM_RptIndicador();
                        var jsonSerialiser = new JavaScriptSerializer();
                        save.id = objReporte.id;
                        save.Tipo = (int)ReportesEnum.IndicadorProceso;
                        save.FechaInicio = vFechaDel;
                        save.FechaFin = vfechaAl;
                        save.datosJson = jsonSerialiser.Serialize(lst);
                        save.Conclusion = vConclusion;
                        save.Tc = (decimal)vTc;
                        save.CC = vCC;

                        RptIndicadorFactoryServices.getRptIndicadorService().SaveReporte(save);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca(CentroCosto, vFechaDel.ToShortDateString() + "-" + vfechaAl.ToShortDateString()));
                        rd.Database.Tables[1].SetDataSource(lstPesos);
                        rd.Database.Tables[2].SetDataSource(lstDlls);
                        rd.SetParameterValue("totalPesos", totalPesos2);
                        rd.SetParameterValue("totalDlls", totalDlls2);
                        rd.SetParameterValue("totalTotal", totalTotal2);
                        rd.SetParameterValue("tc", vTc);
                        rd.SetParameterValue("conclusion", vConclusion);
                        rd.SetParameterValue("totalDllsPesos", totalDllsPesos);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Prefactura:
                    {
                        setMedidasReporte("HO");
                        rd = new rptPrefactura();


                        int id = Convert.ToInt32(Request.QueryString["idPrefactura"]);

                        var obj = RepPrefacturacionFactoryService.getRepPrefacturacionService().getPrefactura(id).FirstOrDefault();

                        var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getPrefactura(id)
                            .Select(x => new
                            {
                                Unidad = HttpUtility.HtmlDecode(x.Unidad),
                                Cantidad = x.Cantidad,
                                Precio = x.Precio,
                                Importe = x.Importe
                            });
                        ;
                        var centroCosto = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(obj.CC);


                        decimal subtotal = restabla.Sum(x => x.Importe);
                        decimal iva = subtotal * .16M;
                        decimal importeTotal = subtotal + iva;

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", ""));
                        rd.Database.Tables[1].SetDataSource(restabla);
                        rd.SetParameterValue("folio", obj.Folio);
                        rd.SetParameterValue("nombre", obj.Nombre);
                        rd.SetParameterValue("direccion", obj.Direccion);
                        rd.SetParameterValue("cp", obj.CP);
                        rd.SetParameterValue("ciudad", obj.Ciudad);
                        rd.SetParameterValue("rfc", obj.RFC);
                        rd.SetParameterValue("fecha", obj.Fecha.ToShortDateString());
                        rd.SetParameterValue("metodoPago", obj.MetodoPago);
                        rd.SetParameterValue("tipoMoneda", obj.TipoMoneda);
                        rd.SetParameterValue("centroCosto", centroCosto);
                        rd.SetParameterValue("subtotal", subtotal);
                        rd.SetParameterValue("iva", iva);
                        rd.SetParameterValue("importeTotal", importeTotal);
                        rd.SetParameterValue("hCC", setTitleCC());

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RESUMENSEMANAL:
                    {

                        var lstResultado = (List<VencimientoDTO>)Session["lstResultado"];
                        rd = new rptResumenSemanal();
                        var divisa = lstResultado.FirstOrDefault().tipoMoneda == 1 ? "MX" : "DLLS";
                        var lstImprimir = lstResultado
                            .Select(x => new
                            {
                                proveedor = x.proveedor,
                                factura = x.factura.ToString(),
                                nombCC = x.nombCC,
                                saldoFactura = changeFormat(x.saldoFactura),
                                fecha = x.fecha,
                                fechaVencimiento = x.fechaVencimiento,
                                suma = lstResultado.Where(y => y.fechaVencimiento == x.fechaVencimiento && y.proveedor.Equals(x.proveedor)).Sum(s => changeFormat(s.saldoFactura))
                            }).ToList();

                        setMedidasReporte("HO");
                        rd.Database.Tables[0].SetDataSource(getInfoEnca(lstResultado.FirstOrDefault().banco.ToUpper() + " " + divisa, lstResultado.FirstOrDefault().factoraje));
                        rd.Database.Tables[1].SetDataSource(lstImprimir);
                        rd.SetParameterValue("hCC", setTitleCC());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.BajaMaquinaria:
                    {
                        setMedidasReporte("HO");
                        rd = new rptBajaEquipo();

                        var lstBajaMaquina = (List<BajaMaquinaDTO>)Session["lstBajaMaquina"];

                        rd.Database.Tables[1].SetDataSource(lstBajaMaquina);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Inventario General de Bajas de Maquinaria y Equipo"));
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.BajaActivosFijos:
                    {
                        setMedidasReporte("HO");
                        rd = new rptBajaActivoFijo();
                        var lstContador = new List<RepRelacionBajaDTO>();
                        var lstRelativo = new List<RepPorcentajeBaja>();
                        var lstBajaMaquina = (List<RepBajaMaquinaria>)Session["lstBajaMaquina"];
                        lstContador.Add((RepRelacionBajaDTO)Session["lstContador"]);
                        lstRelativo.Add((RepPorcentajeBaja)Session["lstRelativo"]);
                        var fecha = (string)Session["fecha"];
                        var lstReporte = lstBajaMaquina.Select(x => new
                        {
                            Economico = x.Economico,
                            GrupoID = x.GrupoID,
                            Descripcion = x.Descripcion,
                            Horometro = x.Horometro,
                            Promedio = x.Promedio,
                            NoAsignado = x.NoAsignado ? "✓" : string.Empty,
                            VentaInterna = x.VentaInterna ? "✓ " : string.Empty,
                            VentaExterna = x.VentaExterna ? "✓ " : string.Empty,
                            TerminoVida = x.TerminoVida ? "✓ " : string.Empty,
                            Siniestro = x.Siniestro ? "✓ " : string.Empty,
                            Robo = x.Robo ? "✓ " : string.Empty
                        }).OrderBy(x => x.GrupoID).ThenBy(x => x.Economico);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Relación de bajas del módulo de activo fijo"));
                        rd.Database.Tables[1].SetDataSource(lstReporte);
                        rd.Database.Tables[2].SetDataSource(lstRelativo);
                        rd.Database.Tables[3].SetDataSource(lstContador);
                        rd.SetParameterValue(0, fecha);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ConsumoAceitesLubricantes:
                    {
                        setMedidasReporte("HO");
                        rd = new rptRepCapturaLubricanes();

                        var lstLubricante = (List<tblM_MaquinariaAceitesLubricantes>)Session["lstMaq"];
                        var cc = (string)Session["cc"];
                        var fecha = (DateTime)Session["fecha"];
                        var turno = (string)Session["turno"];
                        var consumo = (string)Session["consumo"];
                        var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");

                        var lstReporte = lstLubricante.Select(x => new
                        {
                            Economico = x.Economico,
                            Horometro = x.Horometro,
                            Fecha = x.Fecha.ToShortDateString(),
                            Turno = x.Turno == 1 ? "1RA" : x.Turno == 2 ? "2DA" : "3RA",
                            CC = string.Format("{0} - {1}", x.CC, centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CC.Replace(" ", string.Empty))),
                            Rotacion = x.Rotacion == true ? "SI" : "NO",
                            Sopleteo = x.Sopleteo == true ? "SI" : "NO",
                            AK = x.AK == true ? "SI" : "NO",
                            Lubricacion = x.Lubricacion == true ? "SI" : "NO",
                            Antifreeze = x.Antifreeze,
                            MotorId = lstAceites.Where(z => z.id == x.MotorId).FirstOrDefault().Descripcion,
                            MotorVal = x.MotorVal,
                            TransmisionID = lstAceites.Where(z => z.id == x.TransmisionID).FirstOrDefault().Descripcion,
                            TransmisionVal = x.TransmisionVal,
                            HidraulicoID = lstAceites.Where(z => z.id == x.HidraulicoID).FirstOrDefault().Descripcion,
                            HidraulicoVal = x.HidraulicoVal,
                            DiferencialId = lstAceites.Where(z => z.id == x.DiferencialId).FirstOrDefault().Descripcion,
                            DiferencialVal = x.DiferencialVal,
                            MFTIzqId = lstAceites.Where(z => z.id == x.MFTIzqId).FirstOrDefault().Descripcion,
                            MFTDerId = lstAceites.Where(z => z.id == x.MFTDerId).FirstOrDefault().Descripcion,
                            MFTIzqVal = x.MFTIzqVal,
                            MFTDerVal = x.MFTDerVal,
                            MDIzqID = lstAceites.Where(z => z.id == x.MDIzqID).FirstOrDefault().Descripcion,
                            MDDerID = lstAceites.Where(z => z.id == x.MDDerID).FirstOrDefault().Descripcion,
                            MDIzqVal = x.MDIzqVal,
                            MDDerVal = x.MDDerVal,
                            DirId = lstAceites.Where(z => z.id == x.DirId).FirstOrDefault().Descripcion,
                            DirVal = x.DirVal,
                            Grasa = x.Grasa,
                            Firma = string.IsNullOrEmpty(x.Firma) ? "TALLER" : x.Firma
                        })
                        .ToList();

                        rd.Database.Tables[1].SetDataSource(getInfoEnca(cc, fecha.ToShortDateString()));
                        rd.Database.Tables[0].SetDataSource(lstReporte);
                        rd.SetParameterValue("turno", turno);
                        rd.SetParameterValue("hEcho", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ConsumoAceitesLubricantesDetalle:
                    {
                        setMedidasReporte("HO");
                        rd = new rptRepCapturaLubricanes();

                        var lstLubricante = (List<rptAceitesLubricantesDTO>)Session["rptdetallelubicantesDTO"];
                        var cc = (string)Session["cc"];
                        var fecha = (DateTime)Session["fecha"];
                        var turno = (string)Session["turno"];
                        var consumo = (string)Session["consumo"];
                        var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");

                        rd.Database.Tables[1].SetDataSource(getInfoEnca(cc, fecha.ToShortDateString()));
                        rd.Database.Tables[0].SetDataSource(lstLubricante);
                        rd.SetParameterValue("turno", turno);
                        rd.SetParameterValue("hEcho", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RepConsumoLubricente:
                    {
                        setMedidasReporte("HO");
                        rd = new rptRepCapturaLubricanes();

                        var lstLubricante = (List<RptAceitesLubricantes>)Session["lstMaq"];
                        var cc = (string)Session["cc"];
                        var fecha = (string)Session["fecha"];
                        var turno = (string)Session["turno"];
                        var economico = (string)Session["economico"];
                        var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");


                        var lstReporte = lstLubricante.Select(x => new
                        {
                            noEconomico = x.noEconomico,
                            horasTrabajadas = x.horasTrabajadas,
                            motor = x.motor,
                            trans = x.trans,
                            hidraulico = x.hidraulico,
                            diferenciales = x.diferenciales,
                            mandoFinal = x.mandoFinal,
                            direccion = x.direccion,
                            Antifreeze = x.Antifreeze,
                            otros1 = x.otros1,
                            otros2 = x.otros2,
                            otros3 = x.otros3,
                            otros4 = x.otros4,
                            motorDes = lstAceites.Where(z => z.id == x.motorDes).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.motorDes).FirstOrDefault().Descripcion : "N/A",
                            transDescr = lstAceites.Where(z => z.id == x.transDescr).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.transDescr).FirstOrDefault().Descripcion : "N/A",
                            hidraulicoDesc = lstAceites.Where(z => z.id == x.hidraulicoDesc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.hidraulicoDesc).FirstOrDefault().Descripcion : "N/A",
                            difDesc = lstAceites.Where(z => z.id == x.difDesc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.difDesc).FirstOrDefault().Descripcion : "N/A",
                            mandoFinalDesc = lstAceites.Where(z => z.id == x.mandoFinalDesc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.mandoFinalDesc).FirstOrDefault().Descripcion : "N/A",
                            direccionDesc = lstAceites.Where(z => z.id == x.direccionDesc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.direccionDesc).FirstOrDefault().Descripcion : "N/A",
                            otro1Desc = lstAceites.Where(z => z.id == x.otro1Desc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.otro1Desc).FirstOrDefault().Descripcion : "N/A",
                            otro2Desc = lstAceites.Where(z => z.id == x.otro2Desc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.otro2Desc).FirstOrDefault().Descripcion : "N/A",
                            otro3Desc = lstAceites.Where(z => z.id == x.otro3Desc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.otro3Desc).FirstOrDefault().Descripcion : "N/A",
                            otro4Desc = lstAceites.Where(z => z.id == x.otro4Desc).FirstOrDefault() != null ? lstAceites.Where(z => z.id == x.otro4Desc).FirstOrDefault().Descripcion : "N/A"

                        }).ToList();

                        rd.Database.Tables[0].SetDataSource(getInfoEnca(cc, fecha));
                        rd.Database.Tables[1].SetDataSource(lstReporte);
                        rd.SetParameterValue("turno", turno);
                        rd.SetParameterValue("hEco", setTitleEco());
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.COTIZACIONCAPTURA:
                    {
                        setMedidasReporte("HO");
                        rd = new rptCotizacion();
                        var lista = vSesiones.ReporteCotizacionDTO;
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Cotizaciones", "Tracking a cotizaciones"));
                        rd.Database.Tables[1].SetDataSource(lista);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.KPIEQUIPO:
                    {
                        setMedidasReporte("HO");
                        rd = new rptKPIGeneral();
                        var id = int.Parse(Request.QueryString["id"]);
                        var stringCC = (Request.QueryString["cc"]);
                        var cc = stringCC.Split(',').ToList();
                        var anio = Convert.ToDateTime(Request.QueryString["anio"].ToString());
                        var mes = Convert.ToDateTime(Request.QueryString["mes"]);

                        var mesPalabra = Request.QueryString["mesPalabra"];
                        var ccNombre = Request.QueryString["ccNombre"];
                        var data = kpiFactoryServices.getKPIFactoryService().getKPIReporteEquipo(id, cc, anio, mes);

                        var kpiInfoGeneral = new List<kpiInfoGeneralDTO>();
                        var kpiTipoMantenimiento = new List<kpiTipoMantenimientoDTO>();
                        var kpiMotivosParo = new List<kpiMotivosParoDTO>();
                        var kpiFrecuenciaParos = new List<kpiFrecuenciaParosDTO>();
                        var kpiMTTOyParo = new List<kpiMTTOyParoDTO>();


                        kpiInfoGeneral.Add(data.kpiInfoGeneral);
                        kpiTipoMantenimiento.Add(data.kpiTipoMantenimiento);
                        kpiMotivosParo.AddRange(data.kpiMotivosParo);
                        kpiFrecuenciaParos.Add(data.kpiFrecuenciaParos);

                        foreach (var item in data.kpiMTTOyParo)
                        {
                            if (item.MTBS == 0)
                            {
                                item.MTTR = 0;
                            }
                        }
                        kpiMTTOyParo.AddRange(data.kpiMTTOyParo);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Indicador Mensual de Disponibilidad, Tipos y Frecuencia de Paros."));//bien
                        rd.Database.Tables[1].SetDataSource(kpiInfoGeneral);//bien
                        rd.Database.Tables[2].SetDataSource(kpiTipoMantenimiento);
                        rd.Database.Tables[3].SetDataSource(kpiFrecuenciaParos);//bien
                        rd.Database.Tables[4].SetDataSource(kpiMotivosParo);
                        rd.Database.Tables[5].SetDataSource(kpiMTTOyParo);

                        rd.SetParameterValue("CentroCostos", ccNombre);
                        rd.SetParameterValue("RangoFechas", mesPalabra);

                        //rd.SetParameterValue("VersionDocumento", "Ver. 1, 01-08-2018");
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.KPIMENSUAL:
                    {
                        setMedidasReporte("HO");
                        rd = new rptKPIMensual();
                        var tipo = int.Parse(Request.QueryString["tipo"]);
                        var modelo = int.Parse(Request.QueryString["modelo"]);
                        var stringCC = (Request.QueryString["cc"]);
                        var cc = stringCC.Split(',').ToList();
                        var anio = Convert.ToDateTime(Request.QueryString["anio"]);
                        var mes = Convert.ToDateTime(Request.QueryString["mes"]);
                        var mesPalabra = Request.QueryString["mesPalabra"];
                        var ccNombre = Request.QueryString["ccNombre"];
                        var data = kpiFactoryServices.getKPIFactoryService().getKPIGeneral(cc, tipo, modelo, anio, mes);
                        var dataGrafica = kpiFactoryServices.getKPIFactoryService().getKPIRepGraficas(cc, tipo, modelo, anio, mes);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Indicadores Claves de Desempeño"));//bien


                        var TotalTrabajadas = data.Sum(x => Convert.ToDecimal(x.horasTrabajado));
                        var totalHorasHombre = data.Sum(x => Convert.ToDecimal(x.horasHombre));
                        var promedioTotal = data.Select(x => x.pDisponibilidad).ToList();
                        decimal sum = 0;
                        int count = 0;
                        foreach (var item in promedioTotal)
                        {
                            count++;
                            var porcentaje = Convert.ToDecimal(item.TrimEnd('%'));

                            sum += porcentaje;

                        }

                        var totalPorcentaje = (sum / count);

                        rd.Database.Tables[1].SetDataSource(data);//bien
                        rd.Database.Tables[2].SetDataSource(dataGrafica.GraficaFamiliasDTO);//bien
                        rd.Database.Tables[3].SetDataSource(dataGrafica.MotivosParoDTO);//bien
                        rd.SetParameterValue("CentroCostos", ccNombre);

                        string fullMonthName = mes.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        rd.SetParameterValue("RangoFechas", fullMonthName);

                        rd.SetParameterValue("TotalHT", TotalTrabajadas);
                        rd.SetParameterValue("TotalHP", totalHorasHombre);
                        rd.SetParameterValue("ProDisponibilidad", totalPorcentaje);

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.KPIMETRICAS:
                    {
                        setMedidasReporte("HO");
                        rd = new rptKPIMetricasRendimiento();
                        var tipo = int.Parse(Request.QueryString["tipo"]);
                        var modelo = int.Parse(Request.QueryString["modelo"]);
                        var stringCC = (Request.QueryString["cc"]);
                        var cc = stringCC.Split(',').ToList();
                        var anio = Convert.ToDateTime(Request.QueryString["anio"]);
                        var mes = Convert.ToDateTime(Request.QueryString["mes"]);
                        var mesPalabra = Request.QueryString["mesPalabra"];
                        var ccNombre = Request.QueryString["ccNombre"];
                        var data = kpiFactoryServices.getKPIFactoryService().getKPIRepMetricasDTO(cc, tipo, modelo, anio, mes);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "METRICAS DE RENDIMIENTO Y TIPO DE MANTENIMIENTO"));//bien

                        rd.Database.Tables[1].SetDataSource(data.AnualDTO.ToList());//bien
                        rd.Database.Tables[2].SetDataSource(data.kpiMTGraficaTiemposParo.ToList());//bien
                        rd.Database.Tables[3].SetDataSource(data.kpiMTGraficaDisponibilidad.ToList());//bien
                        rd.Database.Tables[4].SetDataSource(data.kpiMTGraficaTendenciaMTTO.ToList());//bien
                        rd.Database.Tables[5].SetDataSource(data.kpiMTGraficaTiposMTTO.ToList());//bien

                        rd.SetParameterValue("CentroCostos", ccNombre);
                        rd.SetParameterValue("RangoFechas", "REPORTE DEL AÑO " + anio.Year);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.KPIGRAFICAS:
                    {
                        setMedidasReporte("HO");
                        rd = new rptKPIMensual();
                        var tipo = int.Parse(Request.QueryString["tipo"]);
                        var modelo = int.Parse(Request.QueryString["modelo"]);
                        var stringCC = (Request.QueryString["cc"]);
                        var cc = stringCC.Split(',').ToList();
                        var anio = Convert.ToDateTime(Request.QueryString["anio"]);
                        var mes = Convert.ToDateTime(Request.QueryString["mes"]);
                        var mesPalabra = Request.QueryString["mesPalabra"];
                        var ccNombre = Request.QueryString["ccNombre"];
                        var data = kpiFactoryServices.getKPIFactoryService().getKPIRepGraficas(cc, tipo, modelo, anio, mes);

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Indicadores Claves de Desempeño"));//bien

                        rd.Database.Tables[1].SetDataSource(data.GraficaFamiliasDTO);//bien
                        rd.Database.Tables[2].SetDataSource(data.MotivosParoDTO);//bien

                        rd.SetParameterValue("CentroCostos", ccNombre);
                        string fullMonthName = mes.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                        rd.SetParameterValue("RangoFechas", fullMonthName);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.FormatoAditivaDeductiva://raguilar
                    {
                        setMedidasReporte("HO");
                        rd = new rptFormatoAditivaDeductiva();
                        int fId = int.Parse(Request.QueryString["fId"]);
                        List<tblRH_AditivaDeductiva> lstobjAditivaDeduc = new List<tblRH_AditivaDeductiva>();
                        tblRH_AditivaDeductiva objAditivaDeductiva = new tblRH_AditivaDeductiva();
                        List<tblRH_AditivaDeductivaDet> lstobjAditivaDeducDet = new List<tblRH_AditivaDeductivaDet>();
                        List<tblRH_AditivaDeductivaDet> lstobjAditivaDeducDet2 = new List<tblRH_AditivaDeductivaDet>();
                        List<tblRH_AutorizacionAditivaDeductiva> lstobjAutAditivaDeducDet = new List<tblRH_AutorizacionAditivaDeductiva>();

                        lstobjAditivaDeduc = objAditivaDeductivaFactoryServices.getAditivaDeductivaService().getListAditivaDeductivaPendientes(fId, "", "", 1);
                        string fechaAlta = "";
                        string fechaCaptura = "";
                        foreach (var item in lstobjAditivaDeduc)
                        {
                            fechaAlta = item.fecha_Alta.ToShortDateString();
                            fechaCaptura = item.fechaCaptura.ToShortDateString();
                        }
                        lstobjAutAditivaDeducDet = objAutadivaDeductivaFactoryService.getAutAditivaDeductivaService().getAutorizacion(fId);
                        lstobjAditivaDeducDet = objAditivaDeductivaDetFactoryService.getAditivaDeductivaDetService().getAditivaDeductivaDet(fId).OrderBy(x => x.id).ToList();
                        string puestoCat = "";
                        int idpuestoCat = 0;
                        List<rptAditivaDeductivaDTO> lstobjrptAditivaDeductivaDTO = new List<rptAditivaDeductivaDTO>();
                        lstobjAditivaDeducDet2 = lstobjAditivaDeducDet.ToList();
                        lstobjAditivaDeducDet2 = lstobjAditivaDeducDet2.GroupBy(g => new { g.puesto }, (k, g) => new tblRH_AditivaDeductivaDet()
                        {
                            aditiva = g.FirstOrDefault().aditiva,
                            deductiva = g.FirstOrDefault().deductiva,
                            puesto = k.puesto,
                            justificacion = g.FirstOrDefault().justificacion,
                            numPersTotal = g.FirstOrDefault().numPersTotal,
                            lugaresPlantilla = g.FirstOrDefault().lugaresPlantilla,
                            personalFaltante = g.FirstOrDefault().personalFaltante,
                            personalExistente = g.FirstOrDefault().personalExistente,
                            personalNecesario = g.FirstOrDefault().personalNecesario,
                            categoria = g.FirstOrDefault().categoria,
                            id = g.FirstOrDefault().id,
                            id_AditivaDeductiva = g.FirstOrDefault().id_AditivaDeductiva
                        }).ToList();
                        List<personalDTO> lstpersonal = new List<personalDTO>();//incluye existente necesario faltante
                        foreach (var item in lstobjAditivaDeducDet2)
                        {
                            idpuestoCat = item.id;
                            puestoCat = item.puesto;
                            rptAditivaDeductivaDTO objrptAditivaDeductivaDTO = new rptAditivaDeductivaDTO();
                            objrptAditivaDeductivaDTO.id = item.id;
                            objrptAditivaDeductivaDTO.puesto = item.puesto;
                            objrptAditivaDeductivaDTO.personalFaltante = item.personalFaltante;
                            objrptAditivaDeductivaDTO.lugaresPlantilla = item.lugaresPlantilla;
                            objrptAditivaDeductivaDTO.numPersTotal = item.numPersTotal;
                            objrptAditivaDeductivaDTO.aditiva = item.aditiva;
                            objrptAditivaDeductivaDTO.deductiva = item.deductiva;
                            objrptAditivaDeductivaDTO.justificacion = item.justificacion;
                            var grupos = lstobjAditivaDeducDet.Select(n => n).Where(n => n.puesto == item.puesto).ToList();

                            foreach (var objResults in grupos)
                            {
                                personalDTO objpersonal = new personalDTO();
                                objpersonal.id = idpuestoCat;
                                objpersonal.personalFaltante = objResults.personalFaltante;
                                objpersonal.personalNecesario = objResults.personalNecesario;
                                objpersonal.personalExistente = objResults.personalExistente;
                                objpersonal.categoria = objResults.categoria;
                                lstpersonal.Add(objpersonal);
                            }
                            lstobjrptAditivaDeductivaDTO.Add(objrptAditivaDeductivaDTO);
                        }
                        lstobjrptAditivaDeductivaDTO.Sort((p, q) => string.Compare(p.puesto, q.puesto));
                        rd.Database.Tables[0].SetDataSource(lstobjrptAditivaDeductivaDTO.ToList());//rptaditivadeductiva
                        rd.Database.Tables[1].SetDataSource(getInfoEnca("FORMATO ADITIVA-DEDUCTIVA PERSONAL", "RECURSOS HUMANOS"));//cabecera
                        rd.Database.Tables[2].SetDataSource(lstobjAditivaDeduc.ToList());//existente
                        rd.Database.Tables[3].SetDataSource(lstobjAutAditivaDeducDet.ToList());//autorizaciones
                        rd.Database.Tables[4].SetDataSource(lstpersonal.ToList());//categor
                        //parametros
                        rd.SetParameterValue("fechaAlta", fechaAlta);
                        rd.SetParameterValue("fechaCaptura", fechaCaptura);
                        rd.SetParameterValue("hCC", setTitleCC());
                        Session.Add("reporte", rd);
                        //var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                        break;
                    }

                case ReportesEnum.Facultamiento:
                    {
                        Session["downloadPDF"] = null;
                        var isCRModal = Convert.ToBoolean(Request.QueryString["isCRModal"]);
                        setMedidasReporte(isCRModal ? "HO" : "HorizontalCarta_NoModal");
                        var id = Request.QueryString["id"].ParseInt();
                        var obj = facultamientofs.getFacutamientoService().getCuadro(id);
                        var cc = obj.cc;
                        var isAdmin = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) || cc.ParseInt(101) < 100;
                        if (isAdmin)
                            rd = new rptFacultamientoAdmin();
                        else
                            rd = new rptFacultamiento();

                        var lst = new List<tblFa_CatFacultamiento>();
                        obj.cc = string.Format("CC {0} - {1}", obj.cc, facultamientofs.getFacutamientoService().getNombreCC(obj.cc));
                        lst.Add(obj);
                        var lstAutorizacion = new List<tblFa_CatAutorizacion>();
                        var lstMonto = facultamientofs.getFacutamientoService().getMonto(obj.id, cc).OrderBy(o => o.min).ToList();
                        var lstPuesto = facultamientofs.getFacutamientoService().GetLstPuesto(obj.id);
                        lstMonto
                            .Where(w => vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) ? w.idTabla.Equals((int)TipoTablaEnum.Administrativo) : true)
                            .ToList().ForEach(e =>
                        {
                            lstAutorizacion.AddRange(facultamientofs.getFacutamientoService().getAutorizacion(e.id, e.renglon));
                        });
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("", ""));
                        rd.Database.Tables[1].SetDataSource(lst.Select(x => new
                        {
                            cc = x.cc,
                            fecha = x.fecha,
                            obra = isAdmin ? string.Empty : (x.obra ?? string.Empty)
                        }));
                        Session["lstAutorizacion"] = lstAutorizacion;
                        if (isAdmin)
                        {
                            var dsMonto = new Dictionary<int, string>();
                            lstAutorizacion.Where(w => !string.IsNullOrEmpty(w.nombre)).GroupBy(g => g.renglon).ToList().ForEach(r =>
                            {
                                var vistoBueno = string.Empty;
                                var vistoBueno2 = string.Empty;
                                var autoriza = string.Empty;
                                r.ToList().ForEach(a =>
                                {
                                    if (a.idTipoAutorizacion == 2)
                                        if (vistoBueno.Length == 0)
                                            vistoBueno += a.nombre;
                                        else
                                            vistoBueno += (" / " + a.nombre);
                                });
                                if (vistoBueno.Length > 0)
                                {
                                    if (r.Any(w => w.idTipoAutorizacion.Equals(2)))
                                        vistoBueno += " ";
                                    if (r.Any(w => w.idTipoAutorizacion.Equals(3)))
                                        vistoBueno += " Y ";
                                }
                                r.ToList().ForEach(a =>
                                {
                                    if (a.idTipoAutorizacion == 3)
                                        if (vistoBueno2.Length == 0)
                                            vistoBueno2 += a.nombre;
                                        else
                                            vistoBueno2 += (" / " + a.nombre);
                                });
                                if (vistoBueno2.Length > 0 || vistoBueno.Length > 0)
                                    vistoBueno2 += r.Where(w => w.idTipoAutorizacion > 1).Count() > 1 ? " dan Vobo. " : " da Vobo. ";
                                r.ToList().ForEach(a =>
                                {
                                    if (a.idTipoAutorizacion == 1)
                                        if (autoriza.Length == 0)
                                            autoriza += a.nombre;
                                        else
                                            autoriza += (" / " + a.nombre);
                                });
                                if (autoriza.Length > 0)
                                    autoriza += r.Where(w => w.idTipoAutorizacion == 1).Count() > 1 ? " autorizan." : " autoriza.";
                                dsMonto.Add(r.FirstOrDefault().renglon, (vistoBueno + vistoBueno2 + autoriza).ToUpper());
                            });
                            rd.Database.Tables[2].SetDataSource(lstAutorizacion.Where(w => !string.IsNullOrEmpty(w.descPuesto)).GroupBy(g => g.nombre)
                            .Select(x => new
                            {
                                empleado = (EnumHelper.GetDescription((TituloEnum)x.FirstOrDefault().idTitulo)) + " " + x.FirstOrDefault().nombre,
                                puesto = x.FirstOrDefault().descPuesto
                            }));
                            rd.Database.Tables[3].SetDataSource(lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Administrativo)
                            .Select(x => new
                            {
                                monto = x.max == 0 ? string.Format("Del {0:C2} en adelante", x.min) : string.Format("Del {0:C2} al {1:C2}", x.min, x.max),
                                refacciones = dsMonto[x.renglon]
                            }));
                        }
                        else
                        {
                            var lstRefac = new List<tblFa_CatAutorizacion>();
                            var lstMaterial = new List<tblFa_CatAutorizacion>();
                            lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).ToList().ForEach(m =>
                                        {
                                            lstRefac.AddRange(lstAutorizacion.Where(a => a.idMonto == m.id));
                                        });
                            lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).ToList().ForEach(m =>
                            {
                                lstMaterial.AddRange(lstAutorizacion.Where(a => a.idMonto == m.id));
                            });
                            var dsRefac = new Dictionary<int, string>();
                            var dsMaterial = new Dictionary<int, string>();
                            var dsMatMonto = lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).ToList();
                            var dsRefacMonto = lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).ToList();
                            lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).ToList().ForEach(m =>
                            {
                                lstRefac.AddRange(lstAutorizacion.Where(a => a.idMonto == m.id));
                            });
                            lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).ToList().ForEach(m =>
                            {
                                lstMaterial.AddRange(lstAutorizacion.Where(a => a.idMonto == m.id));
                            });
                            lstMonto.GroupBy(g => g.idTabla).ToList().ForEach(m =>
                            {
                                lstAutorizacion.Where(w => m.ToList().Exists(e => e.id == w.idMonto && !string.IsNullOrEmpty(w.nombre))).GroupBy(g => g.renglon).ToList().ForEach(r =>
                                {
                                    var vistoBueno = string.Empty;
                                    var vistoBueno2 = string.Empty;
                                    var autoriza = string.Empty;
                                    r.ToList().ForEach(a =>
                                    {
                                        if (a.idTipoAutorizacion == 2)
                                            if (vistoBueno.Length == 0)
                                                vistoBueno += a.nombre;
                                            else
                                                vistoBueno += (" / " + a.nombre);
                                    });
                                    if (vistoBueno.Length > 0)
                                    {
                                        if (r.Any(w => w.idTipoAutorizacion.Equals(2)))
                                            vistoBueno += " ";
                                        if (r.Any(w => w.idTipoAutorizacion.Equals(3)))
                                            vistoBueno += " Y ";
                                    }
                                    r.ToList().ForEach(a =>
                                    {
                                        if (a.idTipoAutorizacion == 3)
                                            if (vistoBueno2.Length == 0)
                                                vistoBueno2 += a.nombre;
                                            else
                                                vistoBueno2 += (" / " + a.nombre);
                                    });
                                    if (vistoBueno.Length > 0 || vistoBueno2.Length > 0)
                                        vistoBueno2 += r.Where(w => w.idTipoAutorizacion > 1).Count() > 1 ? " dan Vobo. " : " da Vobo. ";
                                    r.ToList().ForEach(a =>
                                    {
                                        if (a.idTipoAutorizacion == 1)
                                            if (autoriza.Length == 0)
                                                autoriza += a.nombre;
                                            else
                                                autoriza += (" / " + a.nombre);
                                    });
                                    if (autoriza.Length > 0)
                                        autoriza += r.Where(w => w.idTipoAutorizacion == 1).Count() > 1 ? " autorizan." : " autoriza.";
                                    if (m.FirstOrDefault().idTabla == 1)
                                        dsRefac.Add(r.FirstOrDefault().renglon, (vistoBueno + vistoBueno2 + autoriza).ToUpper());
                                    else
                                        dsMaterial.Add(r.FirstOrDefault().renglon, (vistoBueno + vistoBueno2 + autoriza).ToUpper());
                                });
                            });
                            if (dsMaterial.Count == 0)
                            {
                                var i = 0;
                                dsMatMonto.ForEach(e =>
                                {
                                    dsMaterial.Add(++i, string.Empty);
                                });
                            }
                            rd.Database.Tables[2].SetDataSource(dsRefacMonto.Select(x => new
                            {
                                monto = x.max == 0 ? string.Format("Del {0:C2} en adelante", x.min) : string.Format("Del {0:C2} al {1:C2}", x.min, x.max),
                                refacciones = dsRefac[x.renglon]
                            }));
                            rd.Database.Tables[4].SetDataSource(dsMatMonto.Select(x => new
                            {
                                matMonto = x.max == 0 ? string.Format("Del {0:C2} en adelante", x.min) : string.Format("Del {0:C2} al {1:C2}", x.min, x.max),
                                materiales = dsMaterial[x.renglon]
                            }));
                            rd.Database.Tables[3].SetDataSource(lstRefac.Where(w => !string.IsNullOrEmpty(w.descPuesto)).GroupBy(g => g.nombre)
                            .Select(x => new
                            {
                                empleado = (EnumHelper.GetDescription((TituloEnum)x.FirstOrDefault().idTitulo)) + " " + x.FirstOrDefault().nombre,
                                puesto = x.FirstOrDefault().descPuesto
                            }));
                            rd.Database.Tables[5].SetDataSource(lstMaterial.Where(w => !string.IsNullOrEmpty(w.descPuesto)).GroupBy(g => g.nombre)
                            .Select(x => new
                            {
                                matEmpleado = (EnumHelper.GetDescription((TituloEnum)x.FirstOrDefault().idTitulo)) + " " + x.FirstOrDefault().nombre,
                                matPuesto = x.FirstOrDefault().descPuesto
                            }));
                        }
                        rd.Database.Tables[isAdmin ? 4 : 6].SetDataSource(
                            lstPuesto
                            .Where(p => !string.IsNullOrEmpty(p.puesto))
                            .OrderBy(o => o.idTabla)
                            .ThenBy(o => o.orden)
                            .Select(p => new
                            {
                                orden = (EnumHelper.GetDescription((TipoAutorizacionEnum)p.orden)),
                                idTabla = (EnumHelper.GetDescription((TipoPuestoEnum)p.idTabla)),
                                puesto = p.puesto.ToUpper()
                            }));
                        Session.Add("reporte", rd);
                        break;
                    }

                case ReportesEnum.Poliza:
                    {
                        setMedidasReporte("HO");
                        var isResumen = Convert.ToBoolean(Request.QueryString["isResumen"]);
                        var isCC = Convert.ToBoolean(Request.QueryString["isCC"]);
                        var isPorHoja = Convert.ToBoolean(Request.QueryString["isPorHoja"]);
                        var isFirma = Convert.ToBoolean(Request.QueryString["isFirma"]);
                        var Estatus = Request.QueryString["Estatus"];
                        var icc = Request.QueryString["icc"];
                        var fcc = Request.QueryString["fcc"];
                        var iPol = Convert.ToInt32(Request.QueryString["iPol"]);
                        var fPol = Convert.ToInt32(Request.QueryString["fPol"]);
                        var iPer = Request.QueryString["iPer"];
                        var fPer = Request.QueryString["fPer"];
                        var iTp = Request.QueryString["iTp"];
                        var fTp = Request.QueryString["fTp"];
                        var firma1 = Request.QueryString["firma1"];
                        var firma2 = Request.QueryString["firma2"];
                        if (!isCC)
                        {
                            icc = "001";
                            fcc = "C72";
                        }
                        if (isResumen)
                        {
                            rd = new rptPoliza();
                            var lstPol = polizaFS.getPolizaService().getPolizaEk(Estatus, iPol, fPol, iPer, fPer, iTp, fTp);
                            rd.Database.Tables[0].SetDataSource(getInfoEnca(vSesiones.sesionUsuarioDTO.nombreUsuario, string.Empty));
                            rd.Database.Tables[1].SetDataSource(lstPol);
                        }
                        else
                        {
                            if (isPorHoja)
                            {
                                if (isFirma)
                                    rd = new rptMovPolFirma();
                                else
                                    rd = new rptMovPolHoja();
                            }
                            else
                                rd = new rptMovPoliza();
                            IList<Core.DTO.Contabilidad.Poliza.RepMovPoliza2DTO> lstPol = polizaFS.getPolizaService().getMovPolizaEk(Estatus, iPol, fPol, iPer, fPer, iTp, fTp, icc, fcc);
                            rd.Database.Tables[0].SetDataSource(getInfoEnca(vSesiones.sesionUsuarioDTO.nombreUsuario, string.Empty));
                            rd.Database.Tables[1].SetDataSource(lstPol);
                            if (isPorHoja && isFirma)
                            {
                                rd.SetParameterValue("Firma1", firma1);
                                rd.SetParameterValue("Firma2", firma2);
                            }
                            rd.SetParameterValue("hCCCorto", setTitleCCCorto());
                        }
                        var strCC = "Del Centro de Costos : (" + icc + "): " + centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(icc) + " AL: (" + fcc + "): " + centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(fcc);
                        var strPer = "Periodo del " + iPer + " Al " + fPer;
                        rd.SetParameterValue("RangoCC", strCC);
                        rd.SetParameterValue("RangoPeriodo", strPer);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ReporteFiniquitoRH:
                    {
                        setMedidasReporte("HC");
                        rd = new rptFiniquito();

                        var finiquitoID = Int32.Parse(Request.QueryString["fId"]);
                        var finiquito = finiquitoFactoryServices.getFiniquitoService().GetDetalleFin(finiquitoID);
                        var conceptos = finiquitoFactoryServices.getFiniquitoService().getConceptos();
                        var conceptosPlus = conceptos.Where(x => x.operador == true).Select(y => y.id).ToList();
                        var conceptosMinus = conceptos.Where(x => x.operador == false).Select(y => y.id).ToList();

                        var RepFiniquitoTabla1DTO = finiquito.detalle.Where(x => x.conceptoID == 1 || x.conceptoID == 2).Select(y => new
                        {
                            concepto = y.conceptoNombre + " " + y.conceptoInfo,
                            info = "",
                            valor1 = y.operacion1,
                            valor2 = y.operacion2,
                            valor3 = y.operacion3,
                            valor4 = "$" + y.operacion4.ToString(),
                            valor5 = "$" + y.resultado.ToString()
                        }).ToList();

                        var RepFiniquitoTabla2DTO = finiquito.detalle.Where(x => x.conceptoID != 1 && x.conceptoID != 2).Select(y => new
                        {
                            concepto = (y.conceptoID == 9 && y.conceptoNombre.Contains("OTROS")) ? y.conceptoInfo : y.conceptoNombre + " " + y.conceptoInfo,
                            valor1 = "",
                            valor2 = (y.resultado.ToString() != "") ? y.conceptoDetalle + "     =" : y.conceptoDetalle,
                            valor3 = (conceptosPlus.Contains(y.conceptoID)) ? "$" + y.resultado.ToString() : "(-) $" + y.resultado.ToString()
                        }).ToList();

                        rd.Database.Tables[0].SetDataSource(RepFiniquitoTabla1DTO);
                        rd.Database.Tables[1].SetDataSource(RepFiniquitoTabla2DTO);
                        rd.Database.Tables[2].SetDataSource(getInfoEnca(string.Empty, string.Empty));
                        rd.SetParameterValue("fechaHoy", DateTime.Today.Date);
                        rd.SetParameterValue("ubicacion", "HERMOSILLO, SON");
                        rd.SetParameterValue("fechaIngreso", finiquito.fechaAlta.ToShortDateString());
                        rd.SetParameterValue("fechaEgreso", finiquito.fechaBaja.HasValue ? finiquito.fechaBaja.Value.ToShortDateString() : "");

                        double sueldoMensual = 0;
                        double complementoMensual = 0;
                        if (finiquito.tipoNominaID == 1)
                        {
                            sueldoMensual = ((float)finiquito.salarioBase / 7) * 30.4;
                            complementoMensual = ((float)finiquito.complemento / 7) * 30.4;
                        }
                        else if (finiquito.tipoNominaID == 4)
                        {
                            sueldoMensual = (float)finiquito.salarioBase * 2;
                            complementoMensual = (float)finiquito.complemento * 2;
                        }
                        var sueldoFinal = sueldoMensual + complementoMensual;
                        rd.SetParameterValue("sueldoMensual", sueldoFinal.ToString("0.00"));

                        rd.SetParameterValue("puesto", finiquito.puesto);
                        rd.SetParameterValue("area", finiquito.cc);

                        rd.SetParameterValue("total", finiquito.total);
                        rd.SetParameterValue("puesto2", finiquito.puesto);
                        rd.SetParameterValue("empleado", finiquito.nombre + " " + finiquito.ape_paterno + " " + finiquito.ape_materno);

                        var formuloEmpleado = finiquitoFactoryServices.getFiniquitoService().getUsuario(finiquito.formuloID);
                        rd.SetParameterValue("formuloEmpleado", finiquito.formuloNombre);
                        rd.SetParameterValue("formuloPuesto", formuloEmpleado.puesto.descripcion);

                        var voboEmpleado = finiquitoFactoryServices.getFiniquitoService().getUsuario(finiquito.voboID);
                        rd.SetParameterValue("voboEmpleado", finiquito.voboNombre != null ? finiquito.voboNombre : "");
                        rd.SetParameterValue("voboPuesto", voboEmpleado != null ? voboEmpleado.puesto.descripcion : "");

                        var autorizoEmpleado = finiquitoFactoryServices.getFiniquitoService().getUsuario(finiquito.autorizoID);
                        rd.SetParameterValue("autorizoEmpleado", finiquito.autorizoNombre != null ? finiquito.autorizoNombre : "");
                        rd.SetParameterValue("autorizoPuesto", autorizoEmpleado != null ? autorizoEmpleado.puesto.descripcion : "");

                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.ReporteCargoNominaCCArrendadora:
                    {
                        setMedidasReporte("HC");
                        rd = new rptCargoNominaCCArrendadora();
                        var periodoInicial = vSesiones.sesionPeriodoInicial;
                        var periodoFinal = vSesiones.sesionPeriodoFinal;
                        var nominaSemanal = vSesiones.sesionNominaSemanal;
                        var nominaSemanalDecimal = Convert.ToDecimal(nominaSemanal, CultureInfo.InvariantCulture);
                        var id = Int32.Parse(Request.QueryString["idNomina"]);
                        var arrProyectos = vSesiones.sesionArrProyectos;
                        var proyectosString = string.Empty;
                        proyectosString = arrProyectos.Count == 0 ? maquinaFactoryServices.getMaquinaServices().getNominaCCProyectos(id) : maquinaFactoryServices.getMaquinaServices().GetProyectosString(arrProyectos);
                        var data = maquinaFactoryServices.getMaquinaServices().GetEconomicos(arrProyectos, periodoInicial, periodoFinal);
                        if (!id.Equals(0))
                            data = maquinaFactoryServices.getMaquinaServices().getNominaCCDet(id.ParseInt()).Select(d => new RepCargoNominaCCArreDTO
                            {
                                cargoMaquina = d.cargoD,
                                descripcion = d.descripcion,
                                economicoID = d.idEconomico,
                                noEconomico = d.economico,
                                hhPeriodo = d.hh,
                                porcentajeCargo = d.cargoP,
                                cc = d.cc
                            }).ToList();
                        var sumaHHPeriodo = data.Select(x => x.hhPeriodo).Sum();

                        var ReporteCargoNominaCCTablaDTO = data.Select(y => new
                        {
                            economico = y.noEconomico,
                            descripcion = y.descripcion,
                            cc = y.cc,
                            hhPeriodo = y.hhPeriodo.ToString(),
                            porcentajeCargo = (sumaHHPeriodo != 0 ? (y.hhPeriodo / sumaHHPeriodo) * 100 : 0).ToString("0.00") + "%",
                            cargoMaquina = "$" + ((sumaHHPeriodo != 0 ? (y.hhPeriodo / sumaHHPeriodo) : 0) * nominaSemanalDecimal).ToString("#,##0.00")
                        }).ToList();

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Cargo de nómina por Centros de Costo.", string.Empty));
                        rd.Database.Tables[1].SetDataSource(ReporteCargoNominaCCTablaDTO);

                        rd.SetParameterValue("proyectos", proyectosString);
                        rd.SetParameterValue("periodoInicial", periodoInicial);
                        rd.SetParameterValue("periodoFinal", periodoFinal);
                        rd.SetParameterValue("nominaSemanal", ("$" + nominaSemanalDecimal.ToString("#,##0.00")));
                        rd.SetParameterValue("totalesHH", sumaHHPeriodo);

                        //Valor asignado de la nomina semanal en vez de la suma de todos los cargos individuales
                        rd.SetParameterValue("totalesCargo", ("$" + nominaSemanalDecimal.ToString("#,##0.00")));

                        Session.Add("reporte", rd);

                        // var memory = Request.QueryString["inMemory"];

                        //   if (memory != null)
                        //  {
                        //   var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                        //      maquinaFactoryServices.getMaquinaServices().GuardarCargoNominaCC(downloadPDF);
                        // }

                        break;
                    }
                case ReportesEnum.Mazda_PlanMaestro:
                    {
                        setMedidasReporte("HO");
                        rd = new rptPlanMaestro();

                        var lstMaestro = (List<PlanMaestroDTO>)Session["lstMaestro"];

                        var lstMaestroMesesDesordenados = lstMaestro.Select(x => new PlanMaestroDTO
                        {
                            id = x.id,
                            descripcion = x.descripcion,
                            periodo = x.periodo,
                            periodoDesc = x.periodoDesc,
                            areaID = x.areaID,
                            area = x.area,
                            cuadrillaID = x.cuadrillaID,
                            cuadrilla = x.cuadrilla,
                            mes1 = x.mes4,
                            mes2 = x.mes5,
                            mes3 = x.mes6,
                            mes4 = x.mes7,
                            mes5 = x.mes8,
                            mes6 = x.mes9,
                            mes7 = x.mes10,
                            mes8 = x.mes11,
                            mes9 = x.mes12,
                            mes10 = x.mes1,
                            mes11 = x.mes2,
                            mes12 = x.mes3
                        }).ToList();

                        var ahora = DateTime.Now;
                        List<string> MesesSet = metodo(3, ahora.Year);
                        var lstMes = new List<MesesDTO>() {
                            new MesesDTO(){
                                MES1 = MesesSet[0].Substring(0, 3),
                                MES2 = MesesSet[1].Substring(0, 3),
                                MES3 = MesesSet[2].Substring(0, 3),
                                MES4 = MesesSet[3].Substring(0, 3),
                                MES5 = MesesSet[4].Substring(0, 3),
                                MES6 = MesesSet[5].Substring(0, 3),
                                MES7 = MesesSet[6].Substring(0, 3),
                                MES8 = MesesSet[7].Substring(0, 3),
                                MES9 = MesesSet[8].Substring(0, 3),
                                MES10 = MesesSet[9].Substring(0, 3),
                                MES11 = MesesSet[10].Substring(0, 3),
                                MES12 = MesesSet[11].Substring(0, 3)
                            }
                        };
                        rd.Database.Tables[0].SetDataSource(getInfoEnca(string.Format("PLAN MAESTRO {0}", ahora.Year), string.Empty));
                        rd.Database.Tables[1].SetDataSource(lstMes);
                        rd.Database.Tables[2].SetDataSource(lstMaestroMesesDesordenados);
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Mazda_PlanMensual:
                    {
                        setMedidasReporte("HO");

                        rd = new rptPlanMensual();

                        var planMes = (PlanMesDTO)Session["planMes"];
                        var lstDias = (List<ComboDTO>)Session["lstDias"];
                        var year = (int)Session["year"];
                        var month = (int)Session["month"];
                        var mes = new DateTime(year, month, 1);

                        List<ReportePlanMensualDTO> list = new List<ReportePlanMensualDTO>();

                        //List<string> listDiasSemana = new List<string>();
                        //List<string> listDiasMes = new List<string>();

                        foreach (var pmDet in planMes.detalle)
                        {
                            var diasCheck = pmDet.dias;

                            list.Add(new ReportePlanMensualDTO
                            {
                                //equipoArea = pmDet.equipoAreaDesc,
                                dia1 = diasCheck.Contains(1) ? "🡺" : "",
                                dia2 = diasCheck.Contains(2) ? "🡺" : "",
                                dia3 = diasCheck.Contains(3) ? "🡺" : "",
                                dia4 = diasCheck.Contains(4) ? "🡺" : "",
                                dia5 = diasCheck.Contains(5) ? "🡺" : "",
                                dia6 = diasCheck.Contains(6) ? "🡺" : "",
                                dia7 = diasCheck.Contains(7) ? "🡺" : "",
                                dia8 = diasCheck.Contains(8) ? "🡺" : "",
                                dia9 = diasCheck.Contains(9) ? "🡺" : "",
                                dia10 = diasCheck.Contains(10) ? "🡺" : "",
                                dia11 = diasCheck.Contains(11) ? "🡺" : "",
                                dia12 = diasCheck.Contains(12) ? "🡺" : "",
                                dia13 = diasCheck.Contains(13) ? "🡺" : "",
                                dia14 = diasCheck.Contains(14) ? "🡺" : "",
                                dia15 = diasCheck.Contains(15) ? "🡺" : "",
                                dia16 = diasCheck.Contains(16) ? "🡺" : "",
                                dia17 = diasCheck.Contains(17) ? "🡺" : "",
                                dia18 = diasCheck.Contains(18) ? "🡺" : "",
                                dia19 = diasCheck.Contains(19) ? "🡺" : "",
                                dia20 = diasCheck.Contains(20) ? "🡺" : "",
                                dia21 = diasCheck.Contains(21) ? "🡺" : "",
                                dia22 = diasCheck.Contains(22) ? "🡺" : "",
                                dia23 = diasCheck.Contains(23) ? "🡺" : "",
                                dia24 = diasCheck.Contains(24) ? "🡺" : "",
                                dia25 = diasCheck.Contains(25) ? "🡺" : "",
                                dia26 = diasCheck.Contains(26) ? "🡺" : "",
                                dia27 = diasCheck.Contains(27) ? "🡺" : "",
                                dia28 = diasCheck.Contains(28) ? "🡺" : "",
                                dia29 = diasCheck.Contains(29) ? "🡺" : "",
                                dia30 = diasCheck.Contains(30) ? "🡺" : "",
                                dia31 = diasCheck.Contains(31) ? "🡺" : ""
                            });
                        }


                        rd.Database.Tables[0].SetDataSource(getInfoEnca(string.Format("PLAN MENSUAL {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year).ToUpper(), string.Empty));
                        rd.Database.Tables[1].SetDataSource(list);

                        var cuadrilla = Session["planMesCuadrilla"];
                        var periodo = Session["planMesPeriodo"];

                        rd.SetParameterValue("cuadrilla", cuadrilla);
                        rd.SetParameterValue("periodo", periodo);
                        rd.SetParameterValue("mes", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToUpper());

                        for (var i = 0; i < 31; i++)
                        {
                            if (lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault() != null)
                            {
                                //listDiasSemana.Add(lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Text);
                                //listDiasMes.Add(lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Value.ToString());

                                rd.SetParameterValue(("diaSem" + (i + 1)), lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Text);
                                rd.SetParameterValue(("diaMes" + (i + 1)), lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Value);
                            }
                            else
                            {
                                //listDiasSemana.Add("");
                                //listDiasMes.Add("");

                                rd.SetParameterValue(("diaSem" + (i + 1)), "");
                                rd.SetParameterValue(("diaMes" + (i + 1)), "");
                            }
                        }

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.Mazda_RevisionAC:
                    {
                        setMedidasReporte("HC");
                        rd = new rptRevisionAC();

                        //var cuadrillaID = Int32.Parse(Request.QueryString["cuadrillaID"]);
                        var revisionID = Int32.Parse(Request.QueryString["revisionID"]);

                        var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionAC(revisionID);

                        var condensador = revision.detalle.Where(x => x.actividadID <= 12).Select(y => new
                        {
                            descripcion = y.actividad,
                            realizo = y.realizo == true ? "SÍ" : "NO",
                            observaciones = y.observaciones
                        }).ToList();

                        var evaporador = revision.detalle.Where(x => x.actividadID > 12).Select(y => new
                        {
                            descripcion = y.actividad,
                            realizo = y.realizo == true ? "SI" : "NO",
                            observaciones = y.observaciones
                        }).ToList();

                        var fecha = revision.fechaCaptura;
                        rd.Database.Tables[0].SetDataSource(getInfoEnca(string.Format("REVISIÓN AC {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month), fecha.Year).ToUpper(), string.Empty));
                        rd.Database.Tables[1].SetDataSource(condensador);
                        rd.Database.Tables[2].SetDataSource(evaporador);

                        rd.SetParameterValue("equipo", revision.equipo);
                        rd.SetParameterValue("tonelaje", (float.Parse(revision.tonelaje) != 1 ? revision.tonelaje + " toneladas." : revision.tonelaje + " tonelada."));
                        rd.SetParameterValue("area", revision.area);
                        rd.SetParameterValue("periodo", revision.periodo);
                        rd.SetParameterValue("tecnico", revision.tecnico);
                        rd.SetParameterValue("ayudantes", revision.ayudantes);
                        rd.SetParameterValue("observaciones", revision.observaciones);

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.Mazda_RevisionCua:
                    {
                        setMedidasReporte("HC");
                        rd = new rptRevisionCua();

                        var cuadrillaID = Int32.Parse(Request.QueryString["cuadrillaID"]);
                        var revisionID = Int32.Parse(Request.QueryString["revisionID"]);

                        List<int> arrCuadrillas = new List<int>();
                        arrCuadrillas.Add(cuadrillaID);

                        var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionCua(arrCuadrillas.FirstOrDefault(), revisionID);
                        var actividades = MAZDAFactoryServices.getPlanActividadesService().getActividades(0, 0, "", "");
                        var areas = MAZDAFactoryServices.getPlanActividadesService().getAreas(0, "");

                        var revisionDetalle = revision.detalle.Select(x => new
                        {
                            frecuencia = actividades.Where(y => y.id == x.actividadID).FirstOrDefault().periodoDesc,
                            area = areas.Where(y => y.id == actividades.Where(w => w.id == x.actividadID).FirstOrDefault().areaID).FirstOrDefault().descripcion,
                            actividades = x.actividad,
                            realizo = x.realizo == true ? "SI" : "NO",
                            estatus = x.estadoString
                        });

                        var fecha = revision.fechaCaptura;
                        rd.Database.Tables[0].SetDataSource(getInfoEnca(string.Format("REVISIÓN {0} {1} {2}", revision.cuadrilla.ToUpper(), CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month), fecha.Year).ToUpper(), string.Empty));
                        rd.Database.Tables[1].SetDataSource(revisionDetalle);

                        rd.SetParameterValue("mes", revision.mesDesc);
                        rd.SetParameterValue("tecnico", revision.tecnico);
                        rd.SetParameterValue("ayudantes", revision.ayudantes);
                        rd.SetParameterValue("observaciones", revision.observaciones);

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.RepAnalisisUtilizacion:
                    {
                        setMedidasReporte("HC");
                        rd = new rptAnalisisUtilizacion();
                        var busq = new BusqAnalisiDTO()
                        {
                            cc = int.Parse(Request.QueryString["cc"]),
                            fin = DateTime.Parse(Request.QueryString["fin"])
                        };
                        var lstAnalisis = RepAnalisiUsos.getUsoService().getRepAnalisisUtilizacion(busq);
                        var cc = centroCostosFactoryServices.getCentroCostosService().getEntityCCConstruplan(busq.cc);
                        rd.Database.Tables[0].SetDataSource(getInfoEnca("Dirección de Maquinaria y Equipo", "Análisis de utilización de equipo"));
                        rd.Database.Tables[1].SetDataSource(lstAnalisis);
                        rd.SetParameterValue("obra", cc.descripcion);
                        rd.SetParameterValue("periodo", busq.fin);
                        rd.SetParameterValue("totalProgramado", lstAnalisis.Sum(s => s.requerido));
                        rd.SetParameterValue("totalAdicional", lstAnalisis.Sum(s => s.adicional));
                        rd.SetParameterValue("totalExistente", lstAnalisis.Sum(s => s.existente));
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.RepDiarioMAZDA:
                    {
                        setMedidasReporte("HO");
                        rd = new rptDiarioMAZDA();

                        var fecha = Request.QueryString["fecha"];

                        var equipos = MAZDAFactoryServices.getPlanActividadesService().getEquipos("", "", 0);

                        var reporteDiario = MAZDAFactoryServices.getPlanActividadesService().getReporteDiario(fecha);

                        var reporteDiarioTabla = reporteDiario.Select(x => new
                        {
                            cuadrilla = !string.IsNullOrEmpty(x.cuadrilla) ? x.cuadrilla : " ",
                            actividad = !string.IsNullOrEmpty(x.actividad) ? x.actividad : " ",
                            equipo = x.equiposID != null ? string.Join(", ", equipos.Where(y => x.equiposID.Contains(y.id)).Select(z => z.descripcion).ToArray()) : " ",
                            ultMant = !string.IsNullOrEmpty(x.ultMantenimiento) ? x.ultMantenimiento : " ",
                            sigMant = !string.IsNullOrEmpty(x.sigMantenimiento) ? x.sigMantenimiento : " ",
                            area = !string.IsNullOrEmpty(x.areaEjecucion) ? x.areaEjecucion : " ",
                            descripcionActividad = !string.IsNullOrEmpty(x.descripcionActividad) ? x.descripcionActividad : " ",
                            semaforo = x.semaforo == 1 ? "Realizado" : x.semaforo == 2 ? "Pendiente" : x.semaforo == 3 ? "No se realizó" : " ",
                            reprogramacion = !string.IsNullOrEmpty(x.reprogramacion) ? x.reprogramacion : " ",
                            estatus = !string.IsNullOrEmpty(x.estatus) ? x.estatus : " "
                        }).ToList();

                        List<string> evidencias = new List<string>();
                        List<byte[]> evidenciasByte = new List<byte[]>();
                        List<object> imagenesEvi = new List<object>();

                        foreach (var repD in reporteDiario)
                        {
                            if (repD.revisionDetID != 0)
                            {
                                evidencias.AddRange(MAZDAFactoryServices.getPlanActividadesService().getEvidenciasReporte(repD.revisionDetID));
                            }
                        }

                        foreach (var evi in evidencias)
                        {
                            var eviFormateado = evi.Replace("data:image/png;base64,", "");
                            var eviFormateado2 = eviFormateado.Replace("data:image/jpg;base64,", "");
                            var eviByte = Convert.FromBase64String(eviFormateado2);

                            using (var ms = new MemoryStream(eviByte))
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                var tupla = new Tuple<int, int>(img.Width, img.Height);

                                if (tupla.Item1 > 700)
                                {
                                    double porcentaje = (700 * 100) / tupla.Item1;
                                    var nuevoWidth = (int)Math.Floor(img.Width * (porcentaje / 100));
                                    var nuevoHeight = (int)Math.Floor(img.Height * (porcentaje / 100));

                                    eviByte = MAZDAFactoryServices.getPlanActividadesService().ResizeImageToByteArray(eviByte, nuevoWidth, nuevoHeight);
                                }
                            }

                            imagenesEvi.Add(new
                            {
                                imagen = eviByte,
                                nombre = "",
                                ruta = "",
                                extension = ""
                            });
                        }

                        List<string> referencias = new List<string>();
                        List<byte[]> referenciasByte = new List<byte[]>();
                        List<object> imagenesRefe = new List<object>();

                        var equiposIDLista = reporteDiario.Where(x => x.equiposID != null).Select(y => y.equiposID).ToList();
                        foreach (var eqs in equiposIDLista)
                        {
                            referencias.AddRange(MAZDAFactoryServices.getPlanActividadesService().getReferencias(eqs));
                        }

                        foreach (var refe in referencias)
                        {
                            var refeFormateado = refe.Replace("data:image/png;base64,", "");
                            var refeFormateado2 = refeFormateado.Replace("data:image/jpg;base64,", "");
                            var refeByte = Convert.FromBase64String(refeFormateado2);

                            using (var ms = new MemoryStream(refeByte))
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                var tupla = new Tuple<int, int>(img.Width, img.Height);

                                if (tupla.Item1 > 700)
                                {
                                    double porcentaje = (700 * 100) / tupla.Item1;
                                    var nuevoWidth = (int)Math.Floor(img.Width * (porcentaje / 100));
                                    var nuevoHeight = (int)Math.Floor(img.Height * (porcentaje / 100));

                                    refeByte = MAZDAFactoryServices.getPlanActividadesService().ResizeImageToByteArray(refeByte, nuevoWidth, nuevoHeight);
                                }
                            }

                            imagenesRefe.Add(new
                            {
                                imagen = refeByte,
                                nombre = "",
                                ruta = "",
                                extension = ""
                            });
                        }

                        rd.Database.Tables[0].SetDataSource(getInfoEnca("REPORTE ACTIVIDADES", string.Empty));
                        rd.Database.Tables[1].SetDataSource(reporteDiarioTabla);
                        rd.Database.Tables[2].SetDataSource(imagenesEvi);
                        rd.Database.Tables[3].SetDataSource(imagenesRefe);

                        rd.SetParameterValue("fecha", fecha);

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.Mazda_PlanMensualGeneral:
                    {
                        setMedidasReporte("HO");

                        rd = new rptPlanMensualGeneral();

                        var planesMesGeneral = (List<PlanMesDTO>)Session["planMesGeneral"];
                        var lstDias = (List<ComboDTO>)Session["lstDias"];
                        var year = (int)Session["year"];
                        var month = (int)Session["month"];
                        var mes = new DateTime(year, month, 1);

                        List<ReportePlanMensualDTO> list = new List<ReportePlanMensualDTO>();

                        foreach (var pm in planesMesGeneral)
                        {
                            foreach (var pmDet in pm.detalle)
                            {
                                var diasCheck = pmDet.dias;

                                list.Add(new ReportePlanMensualDTO
                                {
                                    cuadrilla = pmDet.cuadrilla,
                                    periodo = pmDet.periodo,
                                    //equipoArea = pmDet.equipoAreaDesc,
                                    dia1 = diasCheck.Contains(1) ? "🡺" : "",
                                    dia2 = diasCheck.Contains(2) ? "🡺" : "",
                                    dia3 = diasCheck.Contains(3) ? "🡺" : "",
                                    dia4 = diasCheck.Contains(4) ? "🡺" : "",
                                    dia5 = diasCheck.Contains(5) ? "🡺" : "",
                                    dia6 = diasCheck.Contains(6) ? "🡺" : "",
                                    dia7 = diasCheck.Contains(7) ? "🡺" : "",
                                    dia8 = diasCheck.Contains(8) ? "🡺" : "",
                                    dia9 = diasCheck.Contains(9) ? "🡺" : "",
                                    dia10 = diasCheck.Contains(10) ? "🡺" : "",
                                    dia11 = diasCheck.Contains(11) ? "🡺" : "",
                                    dia12 = diasCheck.Contains(12) ? "🡺" : "",
                                    dia13 = diasCheck.Contains(13) ? "🡺" : "",
                                    dia14 = diasCheck.Contains(14) ? "🡺" : "",
                                    dia15 = diasCheck.Contains(15) ? "🡺" : "",
                                    dia16 = diasCheck.Contains(16) ? "🡺" : "",
                                    dia17 = diasCheck.Contains(17) ? "🡺" : "",
                                    dia18 = diasCheck.Contains(18) ? "🡺" : "",
                                    dia19 = diasCheck.Contains(19) ? "🡺" : "",
                                    dia20 = diasCheck.Contains(20) ? "🡺" : "",
                                    dia21 = diasCheck.Contains(21) ? "🡺" : "",
                                    dia22 = diasCheck.Contains(22) ? "🡺" : "",
                                    dia23 = diasCheck.Contains(23) ? "🡺" : "",
                                    dia24 = diasCheck.Contains(24) ? "🡺" : "",
                                    dia25 = diasCheck.Contains(25) ? "🡺" : "",
                                    dia26 = diasCheck.Contains(26) ? "🡺" : "",
                                    dia27 = diasCheck.Contains(27) ? "🡺" : "",
                                    dia28 = diasCheck.Contains(28) ? "🡺" : "",
                                    dia29 = diasCheck.Contains(29) ? "🡺" : "",
                                    dia30 = diasCheck.Contains(30) ? "🡺" : "",
                                    dia31 = diasCheck.Contains(31) ? "🡺" : ""
                                });
                            }
                        }

                        rd.Database.Tables[0].SetDataSource(getInfoEnca(string.Format("PLAN MENSUAL {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year).ToUpper(), string.Empty));
                        rd.Database.Tables[1].SetDataSource(list);

                        rd.SetParameterValue("mes", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToUpper());

                        for (var i = 0; i < 31; i++)
                        {
                            if (lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault() != null)
                            {
                                rd.SetParameterValue(("diaSem" + (i + 1)), lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Text);
                                rd.SetParameterValue(("diaMes" + (i + 1)), lstDias.Where(x => x.Value == (i + 1)).FirstOrDefault().Value);
                            }
                            else
                            {
                                rd.SetParameterValue(("diaSem" + (i + 1)), "");
                                rd.SetParameterValue(("diaMes" + (i + 1)), "");
                            }
                        }

                        Session.Add("reporte", rd);

                        break;
                    }
                case ReportesEnum.REMOCIONCOMPONENTE:

                    DataTable dataTable1 = new DataTable();
                    DataTable dataTable2 = new DataTable();

                    dataTable1.Columns.Add("imgRemovido", System.Type.GetType("System.Byte[]"));
                    dataTable1.Columns.Add("imgInstalado", System.Type.GetType("System.Byte[]"));
                    dataTable2.Columns.Add("personal", typeof(string));
                    dataTable1.Rows.Add((byte[])Session["imgRemovido"], (byte[])Session["imgInstalado"]);
                    List<string> personal = (List<string>)Session["personal"];
                    foreach (var item in personal) 
                    {
                        dataTable2.Rows.Add(item);
                    }
                    setMedidasReporte("HO");
                    var fecha1 = Request.QueryString["fecha"].Trim() == "" ? DateTime.Now.ToString() : Request.QueryString["fecha"];
                    var noEconomico1 = Request.QueryString["noEconomico"].Trim() == "" ? "N/A" : Request.QueryString["noEconomico"];
                    var modelo1 = Request.QueryString["modelo"].Trim() == "" ? "N/A" : Request.QueryString["modelo"];
                    var horasmaquina1 = Request.QueryString["horasmaquina"].Trim() == "" ? "N/A" : Request.QueryString["horasmaquina"];
                    var seriemaquina1 = Request.QueryString["seriemaquina"].Trim() == "" ? "N/A" : Request.QueryString["seriemaquina"];
                    var descripcion1 = Request.QueryString["descripcion"].Trim() == "" ? "N/A" : Request.QueryString["descripcion"];
                    var numparte1 = Request.QueryString["numparte"].Trim() == "" ? "N/A" : Request.QueryString["numparte"];
                    var nocomponenteremovido1 = Request.QueryString["nocomponenteremovido"].Trim() == "" ? "N/A" : Request.QueryString["nocomponenteremovido"];
                    var horascomponenteremovido1 = Request.QueryString["horascomponenteremovido"].Trim() == "" ? "N/A" : Request.QueryString["horascomponenteremovido"];
                    var nocomponenteinstalado1 = Request.QueryString["nocomponenteinstalado"].Trim() == "" ? "SIN ESPECIFICAR" : Request.QueryString["nocomponenteinstalado"];
                    var garantia1 = Request.QueryString["garantia"].Trim() == "" ? "N/A" : Request.QueryString["garantia"];
                    var empresaresponsable1 = Request.QueryString["empresaresponsable"].Trim() == "" ? "N/A" : Request.QueryString["empresaresponsable"];
                    var motivo1 = Request.QueryString["motivo"].Trim() == "" ? "N/A" : Request.QueryString["motivo"];
                    var comentario1 = ((string)Session["comentario"]).Trim() == "" ? "Sin comentario" : (string)Session["comentario"];
                    var realiza1 = ((string)Session["realiza"]).Trim() == "" ? "N/A" : (string)Session["realiza"];
                    rd = new RemocionComponente();
                    rd.Database.Tables[0].SetDataSource(getInfoEnca("Remoción de componentes", "DIRECCION DE MAQUINARIA Y EQUIPO ADMINISTRACION DE OVERHAUL"));
                    rd.Database.Tables[1].SetDataSource(dataTable1);
                    rd.Database.Tables[2].SetDataSource(dataTable2);
                    rd.SetParameterValue("fecha", fecha1);
                    rd.SetParameterValue("noEconomico", noEconomico1);
                    rd.SetParameterValue("modelo", modelo1);
                    rd.SetParameterValue("horasmaquina", horasmaquina1);
                    rd.SetParameterValue("seriemaquina", seriemaquina1);
                    rd.SetParameterValue("descripcion", descripcion1);
                    rd.SetParameterValue("numparte", numparte1);
                    rd.SetParameterValue("nocomponenteremovido", nocomponenteremovido1);
                    rd.SetParameterValue("horascomponenteremovido", horascomponenteremovido1);
                    rd.SetParameterValue("nocomponenteinstalado", nocomponenteinstalado1);
                    rd.SetParameterValue("garantia", garantia1);
                    rd.SetParameterValue("motivo", motivo1);
                    rd.SetParameterValue("comentario", comentario1);
                    rd.SetParameterValue("realiza", realiza1);
                    rd.SetParameterValue("admin", "ING. JOSE PEDRO LOPEZ PROVENCIO");
                    rd.SetParameterValue("empresaresponsable", empresaresponsable1);
                    Session.Add("reporte", rd);
                    break;

                case ReportesEnum.REMOCIONCOMPONENTEGRUPO:
                    rd = new rptRemocionPorGrupos();
                    
                    var bitacoraComponentesRemovidos = (DataTable)Session["bitacoraComponentesRemovidos"];
                    rd.Database.Tables[0].SetDataSource(getInfoEnca("Bitácora de componentes removidos", "DIRECCION DE MAQUINARIA Y EQUIPO ADMINISTRACION DE OVERHAUL"));
                    rd.Database.Tables[1].SetDataSource(bitacoraComponentesRemovidos);
                    Session.Add("reporte", rd);
                    break;

                case ReportesEnum.REPORTEFALLA:
                    rd = new rptReporteFalla();
                    rd.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de falla", "DIRECCION DE MAQUINARIA Y EQUIPO ADMINISTRACION DE OVERHAUL"));

                    Session.Add("reporte", rd);
                    break;
                case ReportesEnum.Reporte_Facultamientos:
                    {
                        Session["downloadPDF"] = null;
                        try
                        {
                            // Obtención de variables del request.
                            var isCRModal = Convert.ToBoolean(Request.QueryString["isCRModal"]);
                            setMedidasReporte(isCRModal ? "HO" : "HorizontalCarta_NoModal");
                            var paqueteID = Request.QueryString["id"].ParseInt();

                            rd = new rptFacultamientosDpto();

                            // Datasource
                            Dictionary<string, object> diccionarioResultado = facultamientosFactoryServices.getFacultamientosService()
                                .ObtenerPaqueteActualizar(paqueteID, true);

                            var paqueteFacultamientos = (PaqueteFaDTO)diccionarioResultado["paqueteFacultamientos"];

                            rd.Database.Tables[0].SetDataSource(getInfoEnca(String.Empty, String.Empty));

                            // LLenado de autorizantes.
                            int contadorVoBo = 1;
                            rd.Database.Tables[1].SetDataSource(
                                paqueteFacultamientos.ListaAutorizantes
                                .Where(x => x.Nombre != "")
                                .Select(x => new
                                {
                                    tipo = (x.EsAutorizante) ? "Autorizante" : "VoBo " + contadorVoBo++,
                                    nombre = x.Nombre,
                                    estado = (x.Autorizado ?? false) ? "Autorizado" : "Pendiente",
                                    firma = (x.Firma != null) ? x.Firma : "S/F"
                                })
                                .ToList()
                            );

                            List<RptFacultamientoDTO> listaFa = new List<RptFacultamientoDTO>();

                            // Llenado de empleados.
                            paqueteFacultamientos.listaFacultamientos
                                .Where(x => x.Aplica)
                                .ToList()
                                .ForEach(x =>
                                {
                                    x.ListaEmpleados
                                        .Where(y => y.Aplica)
                                        .ToList()
                                        .ForEach(y =>
                                    {
                                        var facultamiento = new RptFacultamientoDTO
                                        {
                                            titulo = x.Titulo,
                                            concepto = y.Concepto,
                                            tipo = (y.EsAutorizante) ? "Autorizante" : "VoBo",
                                            nombre = (y.Aplica) ? (y.NombreEmpleado != null) ? y.NombreEmpleado : "PENDIENTE" : "N/A"
                                        };
                                        listaFa.Add(facultamiento);
                                    });
                                });

                            rd.Database.Tables[2].SetDataSource(listaFa);

                            // Se envían valores de parámetros.
                            rd.SetParameterValue("fecha", paqueteFacultamientos.Fecha);
                            rd.SetParameterValue("obra", paqueteFacultamientos.Obra.Trim());
                            rd.SetParameterValue("cc", paqueteFacultamientos.CentroCostos);

                            // Se evalúa si es una versión antigüa o no activa.
                            switch (paqueteFacultamientos.EsActivo)
                            {
                                case null:
                                    rd.SetParameterValue("versionAntigua", "* El paquete está en proceso de autorización y por lo tanto no es oficial.");
                                    break;
                                case true:
                                    rd.SetParameterValue("versionAntigua", "");
                                    break;
                                case false:
                                    rd.SetParameterValue("versionAntigua", "* Este paquete no es la versión mas actual y por lo tanto no es oficial.");
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            // En caso de error, se loguea a la bd.
                            logErrorFactoryServices.getLogErrorService()
                                .LogError(7, 0, "", "ReportesEnum.Reporte_Facultamientos", e, AccionEnum.REPORTE, 0, null);
                        }
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Reporte_Facultamientos_Empleado_General:
                    {
                        Session["downloadPDF"] = null;
                        try
                        {
                            // Variables del request.
                            var claveEmpleado = Request.QueryString["id"].ParseInt();

                            setMedidasReporte("HO");

                            rd = new rptFacultamientoGeneralEmpleado();


                            Dictionary<string, object> diccionarioResultado = facultamientosFactoryServices.getFacultamientosService()
                                .ObtenerFacultamientosEmpleado(claveEmpleado, 0);

                            string nombreEmpleado = facultamientosFactoryServices.getFacultamientosService()
                                .ObtenerNombreEmpleadoPorClave(claveEmpleado);

                            var listaFacultamientos = (List<CatalogoEmpleadoDTO>)diccionarioResultado["listaFacultamientos"];

                            rd.Database.Tables[0].SetDataSource(getInfoEnca(String.Empty, String.Empty));

                            // Se envían los datos al subreporte.
                            rd.Database.Tables[1].SetDataSource(
                                listaFacultamientos
                                .Select(x => new
                                {
                                    cc = x.CentroCostos,
                                    obra = x.Descripcion,
                                    titulo = x.Titulo,
                                    puesto = x.Puesto,
                                    fecha = x.Fecha,
                                    estado = x.Estatus,
                                    tipo = x.TipoAutorizacion
                                })
                                .ToList()
                            );

                            // Se envián los parámetros.
                            rd.SetParameterValue("nombreEmpleado", nombreEmpleado);
                            rd.SetParameterValue("claveEmpleado", claveEmpleado);
                            rd.SetParameterValue("fechaImpresion", String.Format("{0:dd-MM-yyyy}", DateTime.Now));
                        }
                        catch (Exception e)
                        {
                            // Si hay un error, se loguea a la bd.
                            logErrorFactoryServices.getLogErrorService()
                                .LogError(7, 0, "", "ReportesEnum.Reporte_Facultamientos", e, AccionEnum.REPORTE, 0, null);
                        }
                        Session.Add("reporte", rd);
                        break;
                    }
                case ReportesEnum.Reporte_Facultamiento_Empleado:
                    {
                        Session["downloadPDF"] = null;
                        try
                        {
                            setMedidasReporte("HO");

                            // Variables del request.
                            var claveEmpleado = Request.QueryString["id"].ParseInt();
                            var facultamientoID = Request.QueryString["facultamientoID"].ParseInt();

                            rd = new rptFacultamientoEmpleado();

                            //Dictionary<string, object> diccionarioResultado = facultamientosFactoryServices.getFacultamientosService()
                            //    .ObtenerFacultamientosEmpleado(claveEmpleado, ccID);

                            // Datasource principal.
                            Dictionary<string, object> diccionarioResultado = facultamientosFactoryServices.getFacultamientosService()
                            .ObtenerFacultamiento(facultamientoID);

                            var facultamiento = (FacultamientoDTO)diccionarioResultado["facultamiento"];

                            // Se obtiene el nombre completo del empleado en base a su clave.
                            string nombreEmpleado = facultamientosFactoryServices.getFacultamientosService()
                                .ObtenerNombreEmpleadoPorClave(claveEmpleado);

                            List<RptFacultamientoDTO> listaFa = new List<RptFacultamientoDTO>();

                            // Llenado de empleados.
                            facultamiento.ListaEmpleados
                                .ToList()
                                .ForEach(y =>
                                {
                                    var facultamientoReporte = new RptFacultamientoDTO
                                    {
                                        concepto = y.Concepto,
                                        tipo = (y.EsAutorizante) ? "Autorizante" : "VoBo",
                                        nombre = (y.Aplica) ? (y.NombreEmpleado != null) ? y.NombreEmpleado.Trim() : "PENDIENTE" : "N/A"
                                    };
                                    listaFa.Add(facultamientoReporte);
                                });

                            rd.Database.Tables[0].SetDataSource(getInfoEnca(String.Empty, String.Empty));
                            rd.Database.Tables[1].SetDataSource(listaFa);

                            // Se envían los parámetros.
                            rd.SetParameterValue("titulo", facultamiento.Titulo);
                            rd.SetParameterValue("nombreEmpleado", nombreEmpleado.Trim());
                            rd.SetParameterValue("obra", facultamiento.Obra.Trim());
                            rd.SetParameterValue("cc", facultamiento.CentroCostos.Trim());
                            rd.SetParameterValue("fecha", facultamiento.Fecha.Remove(10));
                            rd.SetParameterValue("clave", claveEmpleado);
                        }
                        catch (Exception e)
                        {
                            // En caso de error se loguea a la bd.
                            logErrorFactoryServices.getLogErrorService()
                                .LogError(7, 0, "", "ReportesEnum.Reporte_Facultamiento_Empleado", e, AccionEnum.REPORTE, 0, null);
                        }
                        Session.Add("reporte", rd);
                        break;
                    }
            }


            var inMemory = Request.QueryString["inMemory"];
            if (inMemory != null)
            {
                //Session["downloadPDF"] = null;
                if (((ReportesEnum)reporte) == ReportesEnum.minuta_reunion)
                {
                    int minuta = int.Parse(Request.QueryString["minuta"]);
                    var objMinuta = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinuta(minuta, 0);
                    var objMinutaPrint = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinutaPrint(minuta, 0);
                    rd.SetParameterValue("Fecha", Convert.ToDateTime(objMinuta.fecha).ToString("dd/MM/yyyy"));
                    rd.SetParameterValue("Lugar", objMinuta.lugar);
                    rd.SetParameterValue("Evento", objMinuta.titulo);
                    rd.SetParameterValue("Inicio", objMinuta.horaInicio);
                    rd.SetParameterValue("Termino", objMinuta.horaFin);
                    rd.SetParameterValue("Asuntos", objMinuta.descripcion);
                }
                else if (((ReportesEnum)reporte) == ReportesEnum.lista_asistencia)
                {

                    int minuta = int.Parse(Request.QueryString["minuta"]);
                    var objMinuta = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinuta(minuta, 0);
                    var objListaAsistencia = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getListaAsistenciaMinutaPrint(minuta, 0);

                    rd.SetParameterValue("Fecha", Convert.ToDateTime(objMinuta.fecha).ToString("dd/MM/yyyy"));
                    rd.SetParameterValue("Evento", objMinuta.titulo);
                    rd.SetParameterValue("Inicio", objMinuta.horaInicio);
                    rd.SetParameterValue("Termino", objMinuta.horaFin);
                }
                else if (((ReportesEnum)reporte) == ReportesEnum.Formato_Cambio)
                {

                    Session["downloadPDF"] = null;
                    int fId = int.Parse(Request.QueryString["fId"]);
                    List<tblRH_FormatoCambio> objEmploadosEnkontrol = new List<tblRH_FormatoCambio>();
                    List<tblRH_FormatoCambio> objEmploados = capturaFormatoCambioFactoryServices.getFormatoCambioService().getListFormatosCambioPendientes(fId, "", 0, 1, "", 0);

                    List<tblRH_AutorizacionFormatoCambio> objAutorizacion = new List<tblRH_AutorizacionFormatoCambio>();

                    objEmploadosEnkontrol.Add(capturaFormatoCambioFactoryServices.getFormatoCambioService().getEmpleadoForId(objEmploados[0].Clave_Empleado,false));
                    foreach (var objEmp in objEmploados)
                    {
                        if (objEmp.TipoNominaID == (int)Tipo_NominaEnum.SEMANAL)
                        {
                            rd.SetParameterValue("TipoNomina", "Semana");
                            rd.SetParameterValue("SalarioMesEmpleadoCambio", Math.Round(((Convert.ToDouble(objEmp.Salario_Base) / 7) * 30.4), 2));
                            rd.SetParameterValue("ComplementoMesEmpleadoCambio", Math.Round(((Convert.ToDouble(objEmp.Complemento) / 7) * 30.4), 2));
                            rd.SetParameterValue("BonoMesEmpleadoCambio", Math.Round(((Convert.ToDouble(objEmp.Bono) / 7) * 30.4), 2));

                        }
                        else
                        {
                            rd.SetParameterValue("TipoNomina", "Quincena");
                            rd.SetParameterValue("SalarioMesEmpleadoCambio", Math.Round((Convert.ToDouble(objEmp.Salario_Base) * 2), 2));
                            rd.SetParameterValue("ComplementoMesEmpleadoCambio", Math.Round((Convert.ToDouble(objEmp.Complemento) * 2), 2));
                            rd.SetParameterValue("BonoMesEmpleadoCambio", Math.Round((Convert.ToDouble(objEmp.Bono) * 2), 2));
                        }
                        objAutorizacion = capturaAutorizacionFormatoCambioService.getAutorizacionFormatoCambioService().getAutorizacion(objEmp.id);
                    }

                    foreach (var objEmpKon in objEmploadosEnkontrol)
                    {
                        if (objEmpKon.TipoNominaID == (int)Tipo_NominaEnum.SEMANAL)
                        {
                            rd.SetParameterValue("SalarioMesEmpleadoOriginal", Math.Round(((Convert.ToDouble(objEmpKon.Salario_Base) / 7) * 30.4), 2));
                            rd.SetParameterValue("ComplementoMesEmpleadoOriginal", Math.Round(((Convert.ToDouble(objEmpKon.Complemento) / 7) * 30.4), 2));
                            rd.SetParameterValue("BonoMesEmpleadoOriginal", Math.Round(((Convert.ToDouble(objEmpKon.Bono) / 7) * 30.4), 2));
                        }
                        else
                        {
                            rd.SetParameterValue("BonoMesEmpleadoOriginal", Math.Round((Convert.ToDouble(objEmpKon.Bono) * 2), 2));
                            rd.SetParameterValue("SalarioMesEmpleadoOriginal", Math.Round((Convert.ToDouble(objEmpKon.Salario_Base) * 2), 2));
                            rd.SetParameterValue("ComplementoMesEmpleadoOriginal", Math.Round((Convert.ToDouble(objEmpKon.Complemento) * 2), 2));
                        }

                    }
                }
                else if (((ReportesEnum)reporte) == ReportesEnum.Solicitud_EquipoNoModal)
                {
                    Session["downloadPDF"] = null;

                    rd.SetParameterValue("VersionDocumento", "Ver. 1, 30-09-2016");


                    List<SolicitudEquipoDTO> rptData = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];

                    var autorizadores = (AutorizadoresIDDTO)Session["rptAutorizadores"];
                    var FolioSolicitud = "";

                    if (rptData != null)
                    {
                        FolioSolicitud = rptData.FirstOrDefault().Folio;
                    }

                    rd.SetParameterValue("FolioDocumento", FolioSolicitud);

                    var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(autorizadores.usuarioElaboro)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorGerente = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.gerenteObra)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorGerenteDirector = usuarioFactoryServices.getUsuarioService()
                      .ListUsersById(autorizadores.GerenteDirector)
                      .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorDirector = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.directorDivision)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorDireccion = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.altaDireccion)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();


                    if (Session["rptCadenaAutorizacion"] != null)
                    {
                        var cadena = (CadenaAutorizacionDTO)Session["rptCadenaAutorizacion"];

                        rd.SetParameterValue("CadenaDireccion", cadena.CadenaDireccion == null ? "" : cadena.CadenaDireccion);
                        rd.SetParameterValue("CadenaDirector", cadena.CadenaDirector == null ? "" : cadena.CadenaDirector);
                        rd.SetParameterValue("CadenaGerente", cadena.CadenaGerente == null ? "" : cadena.CadenaGerente);
                        rd.SetParameterValue("CadenaElabora", cadena.CadenaElabora == null ? "" : cadena.CadenaElabora);
                        rd.SetParameterValue("CadenaGerenteDirector", cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector);

                    }
                    else
                    {
                        rd.SetParameterValue("CadenaDireccion", "");
                        rd.SetParameterValue("CadenaDirector", "");
                        rd.SetParameterValue("CadenaGerente", "");
                        rd.SetParameterValue("CadenaElabora", "");
                        rd.SetParameterValue("CadenaGerenteDirector", "");

                    }
                    rd.SetParameterValue("Elaboro", AutorizadorElabora.nombre);
                    rd.SetParameterValue("Solicito", AutorizadorGerente.nombre);
                    rd.SetParameterValue("valido", AutorizadorDirector.nombre);
                    rd.SetParameterValue("autorizo", AutorizadorDireccion.nombre);
                    rd.SetParameterValue("Valido2", AutorizadorGerenteDirector.nombre);

                    var pCentroCostosVal = Request.QueryString["pCC"].ToString();

                    if (!string.IsNullOrEmpty(pCentroCostosVal))
                    {
                        pCC = Request.QueryString["pCC"];
                        pNombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);
                    }
                    else
                    {
                        pNombreCC = "";
                    }
                    rd.SetParameterValue("CentroCostos", pNombreCC);
                }
                else if (((ReportesEnum)reporte) == ReportesEnum.PreviewStandby)
                {
                    Session["downloadPDF"] = null;
                    rd.SetParameterValue("Descripcion", StandbyParmDTO.Descripcion);
                    rd.SetParameterValue("Centro_Costos", StandbyParmDTO.Centro_Costos);
                    rd.SetParameterValue("elabora1", StandbyParmDTO.elabora1);
                    rd.SetParameterValue("elabora2", StandbyParmDTO.elabora2);

                }
                else if (((ReportesEnum)reporte) == ReportesEnum.Solicitud_Equipo_Asignado)
                {

                    Session["downloadPDF"] = null;

                    rd.SetParameterValue("VersionDocumento", "Ver. 1, 30-09-2016");


                    List<SolicitudEquipoDTO> rptData = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];

                    var autorizadores = (AutorizadoresIDDTO)Session["rptAutorizadores"];
                    var FolioSolicitud = "";

                    var Asigna = Session["rptAsigna"] != null ? Session["rptAsigna"].ToString() : "";
                    string nombre = "";
                    if (Asigna != null)
                    {
                        nombre = Asigna;
                    }
                    if (rptData != null)
                    {
                        FolioSolicitud = rptData.FirstOrDefault().Folio;
                    }

                    rd.SetParameterValue("FolioDocumento", FolioSolicitud);

                    var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(autorizadores.usuarioElaboro)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorGerente = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.gerenteObra)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorGerenteDirector = usuarioFactoryServices.getUsuarioService()
                      .ListUsersById(autorizadores.GerenteDirector)
                      .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorDirector = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.directorDivision)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();

                    var AutorizadorDireccion = usuarioFactoryServices.getUsuarioService()
                        .ListUsersById(autorizadores.altaDireccion)
                        .Select(x => new { nombre = (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno) }).FirstOrDefault();


                    if (Session["rptCadenaAutorizacion"] != null)
                    {
                        var cadena = (CadenaAutorizacionDTO)Session["rptCadenaAutorizacion"];

                        rd.SetParameterValue("CadenaDireccion", cadena.CadenaDireccion == null ? "" : cadena.CadenaDireccion);
                        rd.SetParameterValue("CadenaDirector", cadena.CadenaDirector == null ? "" : cadena.CadenaDirector);
                        rd.SetParameterValue("CadenaGerente", cadena.CadenaGerente == null ? "" : cadena.CadenaGerente);
                        rd.SetParameterValue("CadenaElabora", cadena.CadenaElabora == null ? "" : cadena.CadenaElabora);
                        rd.SetParameterValue("CadenaGerenteDirector", cadena.CadenaGerenteDirector == null ? "" : cadena.CadenaGerenteDirector);

                    }
                    else
                    {
                        rd.SetParameterValue("CadenaDireccion", "");
                        rd.SetParameterValue("CadenaDirector", "");
                        rd.SetParameterValue("CadenaGerente", "");
                        rd.SetParameterValue("CadenaElabora", "");
                        rd.SetParameterValue("CadenaGerenteDirector", "");

                    }


                    rd.SetParameterValue("Asigno", nombre);
                    rd.SetParameterValue("Elaboro", AutorizadorElabora.nombre);
                    rd.SetParameterValue("Solicito", AutorizadorGerente.nombre);
                    rd.SetParameterValue("valido", AutorizadorDirector.nombre);
                    rd.SetParameterValue("autorizo", AutorizadorDireccion.nombre);
                    rd.SetParameterValue("Valido2", AutorizadorGerenteDirector.nombre);

                    var pCentroCostosVal = Request.QueryString["pCC"].ToString();

                    if (!string.IsNullOrEmpty(pCentroCostosVal))
                    {
                        pCC = Request.QueryString["pCC"];
                        pNombreCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(pCC);
                    }
                    else
                    {
                        pNombreCC = "";
                    }
                    rd.SetParameterValue("CentroCostos", pNombreCC);

                }
                //prueba 18/01/18 08:14
                else if (((ReportesEnum)reporte) == ReportesEnum.FormatoAditivaDeductiva)
                {
                    Session["downloadPDF"] = null;
                    List<tblRH_AditivaDeductiva> lstobjAditivaDeduc = new List<tblRH_AditivaDeductiva>();
                    int fId = int.Parse(Request.QueryString["fId"]);
                    lstobjAditivaDeduc = objAditivaDeductivaFactoryServices.getAditivaDeductivaService().getListAditivaDeductivaPendientes(fId, "", "", 1);
                    string fechaAlta = "";
                    string fechaCaptura = "";
                    foreach (var item in lstobjAditivaDeduc)
                    {
                        fechaAlta = item.fecha_Alta.ToShortDateString();
                        fechaCaptura = item.fechaCaptura.ToShortDateString();
                    }
                    rd.SetParameterValue("fechaAlta", fechaAlta);
                    rd.SetParameterValue("fechaCaptura", fechaCaptura);
                    //Session.Add("reporte", rd);                   
                }
                else if (((ReportesEnum)reporte) == ReportesEnum.CONCILIACIONHOROMETROS)
                {
                    int pConciliacionID = Convert.ToInt32(Request.QueryString["pConciliacionID"]);
                    var auth = cfs.getConciliacionServices().loadAutorizacionFromConciliacacionId(pConciliacionID);
                    var encConciliacion = cfs.getConciliacionServices().getCapEncConciliacion(pConciliacionID);
                    var conciliaciones = cfs.getConciliacionServices().getConciliaciones(pConciliacionID);
                    var gerente = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaGerenteID);
                    var admin = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaAdmin);
                    var director = usuarioFactoryServices.getUsuarioService().getPassByID(auth.autorizaDirector);
                    var centroCostos = centroCostosFactoryServices.getCentroCostosService().getEntityCCConstruplan(encConciliacion.centroCostosID);
                    var encCaratula = cfs.getConciliacionServices().getEncabezado(encConciliacion.centroCostosID);
                    var caratula = cfs.getConciliacionServices().getLstPrecios(encCaratula.id);
                    DateTime fecha = DateTime.Now;
                    DateTime FechaSend = new DateTime(fecha.Year, 01, 01);

                    string moneda = "";

                    if (encCaratula != null)
                    {
                        moneda = encCaratula.moneda == 1 ? "M.N." : "USD";


                    }

                    //    var Data = GetFechas(FechaSend).FirstOrDefault(x => x.Value == encConciliacion.fechaID);
                    var Data = GetQuincenas().FirstOrDefault(r => r.Value == encConciliacion.fechaID);
                    rd.Database.Tables[0].SetDataSource(getInfoEnca("", "CONCILIACIÓN DE HOROMETROS"));
                    rd.Database.Tables[1].SetDataSource(conciliaciones.Select(c => new
                    {
                        no = c.numero,
                        economico = c.economico,
                        descripcion = c.descripcion,
                        hi = c.horometroInicial,
                        hf = c.horometroFinal,
                        he = c.horometroEfectivo,
                        unidad = c.unidad == 1 ? "HORAS" : "DÍA",
                        costoHora = c.costo,
                        costoTotal = c.total,
                        carga = EnumHelper.GetDescription((EmpresaEnum)c.idEmpresa).ToString(),
                        observaciones = c.observaciones
                    }));
                    if (Data != null)
                    {
                        var ArraySplit = Data.Text.Split('-');
                        FechaSend = Convert.ToDateTime(ArraySplit[1]);
                        rd.SetParameterValue("FechaInicio", ArraySplit[0]);
                        rd.SetParameterValue("fechaFinal", ArraySplit[1]);
                    }
                    rd.SetParameterValue("pProyecto", centroCostos.cc + " " + centroCostos.descripcion);
                    rd.SetParameterValue("nombreGerente", string.Format("{0} {1} {2}", gerente.nombre, gerente.apellidoPaterno, gerente.apellidoMaterno));
                    rd.SetParameterValue("cadenaGerente", auth.firmaGerente);
                    rd.SetParameterValue("cadenaAdministrador", auth.firmaAdmin);
                    rd.SetParameterValue("nombreAdministrador", string.Format("{0} {1} {2}", admin.nombre, admin.apellidoPaterno, admin.apellidoMaterno));
                    rd.SetParameterValue("cadenaDirector", auth.firmaDirector);
                    rd.SetParameterValue("nombreDirector", string.Format("{0} {1} {2}", director.nombre, director.apellidoPaterno, director.apellidoMaterno));
                    rd.SetParameterValue("moneda", moneda);
                    Session["downloadPDF"] = null;
                    Session.Add("reporte", rd);

                }
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                byte[] downloadPDF = null;
                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);
                    downloadPDF = streamReader.ToArray();

                }
                if (Session["downloadPDF"] != null)
                {
                    List<byte[]> files = (List<byte[]>)Session["downloadPDF"];
                    files.Add(downloadPDF);
                    Session["downloadPDF"] = files;
                }
                else
                {
                    List<byte[]> files = new List<byte[]>();
                    files.Add(downloadPDF);
                    Session["downloadPDF"] = files;
                }

                GuardarReporte(reporte);
            }
            else
            {
                Session["downloadPDF"] = null;
            }

            return paramFields;
        }




        private void GuardarReporte(int reporte)
        {
            switch (reporte)
            {
                case 86:
                    //maquinaFactoryServices.getMaquinaServices().GuardarCargoNominaCC((List<byte[]>)Session["downloadPDF"]);
                    //Session["downloadPDF"] = null;
                    break;
            }
        }

        private DataTable GetRespuestas(List<RespuestasEncuestasDTO> Data)
        {
            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("Calificacion", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("TipoPregunta", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("Pregunta", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("Comentario", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("DescripcionTipo", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("CalificacionDescripcion", System.Type.GetType("System.String"));

            foreach (var item in Data)
            {

                string path = "";
                switch ((int)item.Calificacion)
                {
                    case 0:
                        path = @"\Content\img\Encuestas\S_Value0.png";
                        break;
                    case 1:
                        path = @"\Content\img\Encuestas\S_Value1.png";
                        break;
                    case 2:
                        path = @"\Content\img\Encuestas\S_Value2.png";
                        break;
                    case 3:
                        path = @"\Content\img\Encuestas\S_Value3.png";
                        break;
                    case 4:
                        path = @"\Content\img\Encuestas\S_Value4.png";
                        break;
                    case 5:
                        path = @"\Content\img\Encuestas\S_Value5.png";
                        break;

                    default:
                        break;
                }

                byte[] imgdata = File.ReadAllBytes(MapPath(path));
                string empresa = "Grupo Construcciones Planificadas S.A. de C.V.";
                tableEncabezado.Rows.Add(imgdata, item.tipoPregunta, item.Pregunta, item.Comentario, item.DescripcionTipo, item.CalificacionDescripcion);
            }
            return tableEncabezado;

        }

        private dtsMotivosParoDTO infoMotivosParo(int p, string TM, string TP, string motivoParo)
        {
            dtsMotivosParoDTO dtsMotivosParoDTO = new dtsMotivosParoDTO();
            switch (motivoParo)
            {
                case "2":
                    dtsMotivosParoDTO.trabajando = "X";

                    break;

                case "1":
                    dtsMotivosParoDTO.stanby = "X";
                    break;
                case "3":
                    dtsMotivosParoDTO.tramo = "X";
                    break;
                default:
                    break;
            }

            switch (p)
            {
                case 1:
                    dtsMotivosParoDTO.MP1 = "X";

                    break;
                case 2:
                    dtsMotivosParoDTO.MP2 = "X";
                    break;
                case 3:
                    dtsMotivosParoDTO.MP3 = "X";
                    break;
                case 4:
                    dtsMotivosParoDTO.MP4 = "X";
                    break;
                case 5:
                    dtsMotivosParoDTO.MP5 = "X";
                    break;
                case 6:
                    dtsMotivosParoDTO.MP6 = "X";
                    break;
                case 7:
                    dtsMotivosParoDTO.MP7 = "X";
                    break;
                case 8:
                    dtsMotivosParoDTO.MP8 = "X";
                    break;
                case 9:
                    dtsMotivosParoDTO.MP9 = "X";
                    break;
                case 10:
                    dtsMotivosParoDTO.MP10 = "X";
                    break;
                case 11:
                    dtsMotivosParoDTO.MP11 = "X";
                    break;
                case 12:
                    dtsMotivosParoDTO.MP12 = "X";
                    break;
                case 13:
                    dtsMotivosParoDTO.MP13 = "X";
                    break;
                case 14:
                    dtsMotivosParoDTO.MP14 = "X";
                    break;
                case 15:
                    dtsMotivosParoDTO.MP15 = "X";
                    break;
                case 16:
                    dtsMotivosParoDTO.MP16 = "X";
                    break;
                case 17:
                    dtsMotivosParoDTO.MP17 = "X";
                    break;
                case 18:
                    dtsMotivosParoDTO.MP18 = "X";
                    break;
                case 19:
                    dtsMotivosParoDTO.MP19 = "X";
                    break;
                case 20:
                    dtsMotivosParoDTO.MP20 = "X";
                    break;
                case 21:
                    dtsMotivosParoDTO.MP21 = "X";
                    break;
                case 22:
                    dtsMotivosParoDTO.MP22 = "X";
                    break;
                case 23:
                    dtsMotivosParoDTO.MP23 = "X";
                    break;
                case 24:
                    dtsMotivosParoDTO.MP24 = "X";
                    break;
                case 25:
                    dtsMotivosParoDTO.MP25 = "X";
                    break;
                case 26:
                    dtsMotivosParoDTO.MP26 = "X";
                    break;
                case 27:
                    dtsMotivosParoDTO.MP27 = "X";
                    break;
                default:
                    break;
            }

            switch (TM)
            {
                case "Preventivo":
                    dtsMotivosParoDTO.preventivo = "X";
                    break;
                case "Correctivo":
                    dtsMotivosParoDTO.correctivo = "X";
                    break;
                case "Predictivo":
                    dtsMotivosParoDTO.predictivo = "X";
                    break;
                default:
                    break;
            }

            switch (TP)
            {
                case "Programado":
                    dtsMotivosParoDTO.programado = "X";
                    break;
                case "No Programado":
                    dtsMotivosParoDTO.noProgramado = "X";
                    break;
                default:
                    break;
            }

            return dtsMotivosParoDTO;
        }

        private string getTipo(int p)
        {
            switch (p)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";

                default:
                    return "";
            }
        }

        private string GetInfoMaquinaria(int id)
        {
            objMaquinaria = new tblM_CatMaquina();

            objMaquinaria = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(id).FirstOrDefault();
            return objMaquinaria.noEconomico;
        }

        private void setMedidasReporte(string p)
        {
            var r = vSesiones.sesionCurrentResolution;

            switch (p)
            {
                case "VC":
                    //crvReporteEstandar.Width = 920;
                    //crvReporteEstandar.Height = r.height - 320; //<1080
                    crvReporteEstandar.Width = r.width;
                    crvReporteEstandar.Height = r.height - 150; //<1080
                    break;
                case "HO":
                    //crvReporteEstandar.Width = r.width - 130;
                    //crvReporteEstandar.Height = r.height - 320;
                    crvReporteEstandar.Width = r.width;
                    crvReporteEstandar.Height = r.height - 150;
                    break;
                case "HC":
                    //crvReporteEstandar.Width = 1230;
                    //crvReporteEstandar.Height = r.height - 320;
                    crvReporteEstandar.Width = r.width;
                    crvReporteEstandar.Height = r.height - 150;
                    break;
                case "NOMODAL":
                    crvReporteEstandar.Width = 1230;
                    crvReporteEstandar.Height = 520;
                    break;

                case "HorizontalCarta_NoModal":
                    crvReporteEstandar.Width = 1230;
                    if (r.height >= 1050)
                    {
                        crvReporteEstandar.Width = 1300;
                        crvReporteEstandar.Height = 720;
                    }

                    if (r.height >= 800 && r.height < 1050)
                    {
                        crvReporteEstandar.Height = 530;
                    }
                    if (r.height >= 768 && r.height < 800)
                    {
                        crvReporteEstandar.Height = 390;
                    }
                    if (r.height >= 600 && r.height < 768)
                    {
                        crvReporteEstandar.Height = 340;
                    }


                    break;
                default:
                    break;
            }





        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        /// <summary>
        /// Método de reporte de gastos, obtiene la información para el reporte arma el dataset y sus parámetros.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private DataTable getInfoRep(RepGastosFiltrosDTO obj)
        {


            DataTable table = new DataTable();
            //Se obtiene el conjunto de datos de la consulta.
            var raw = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyType(obj);
            //se obtiene el conjunto de datos de overhual.
            var rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaul(obj);
            //se obtiene la lista de los numeros economicos
            var listNoEco = raw.Select(x => x.noEco).Distinct().ToList().OrderBy(x => x);

            var fechainicio = Convert.ToDateTime(obj.fechaInicio);
            var fechafin = Convert.ToDateTime(obj.fechaFin);

            int count = 0;
            try
            {
                //Esta lista es de las columnas.
                var listTipos = raw.Select(x => x.descripcion).Distinct().ToList();
                var listTiposFinal = new List<string>();
                listTiposFinal.Add("NO. ECO");
                listTiposFinal.AddRange(listTipos);
                listTiposFinal.Add("TOTAL");
                listTiposFinal.Add("TOTAL SIN OVERHAUL");
                listTiposFinal.Add("HORAS");
                listTiposFinal.Add("COSTO HORARIO");

                setParametro("noEconomico", "");
                setParametro("COSTOH", "");
                setParametro("MAQUINARIA", "");
                setParametro("MATERIALES", "");
                setParametro("ADMINISTRATIVOS", "");
                setParametro("Vacio", "");
                setParametro("TOTAL", "");
                setParametro("TOTALSNOV", "");
                setParametro("HORAS", "");
                setParametro("Vacio", "");


                foreach (var i in listTiposFinal)
                {
                    if (i.Equals("NO. ECO"))
                    {
                        setParametro("noEconomico", i);
                    }
                    else if (i.Equals("COSTO HORARIO"))
                    {
                        setParametro("COSTOH", i);
                    }
                    else if (i.Equals("MATERIALES"))
                    {
                        setParametro("MATERIALES", i);
                    }
                    else if (i.Equals("MAQUINARIA"))
                    {
                        setParametro("MAQUINARIA", i);
                    }
                    else if (i.Equals("ADMINISTRATIVOS"))
                    {
                        setParametro("ADMINISTRATIVOS", i);
                    }
                    else if (i.Equals("TOTAL"))
                    {
                        setParametro("TOTAL", i);
                    }
                    else if (i.Equals("TOTAL SIN OVERHAUL"))
                    {
                        setParametro("TOTALSNOV", i);
                    }
                    else if (i.Equals("HORAS"))
                    {
                        setParametro("HORAS", i);
                    }
                    else
                    {
                        // setParametro("dato"+count, i);
                        setParametro("Vacio", i);
                        count++;
                    }
                }

                int columns = 0;
                foreach (var array in listTiposFinal)
                {
                    if (array.Length > columns)
                    {
                        columns = array.Length;
                    }
                }

                foreach (var i in listTiposFinal)
                {
                    if (i.Equals("NO. ECO"))
                    {
                        table.Columns.Add("noEconomico");
                        // setParametro("noEconomico", i);
                    }
                    else if (i.Equals("COSTO HORARIO"))
                    {

                        table.Columns.Add("COSTOH");
                        // setParametro("COSTOH", i);
                    }
                    else if (i.Equals("MATERIALES"))
                    {
                        table.Columns.Add("MATERIALES");
                        //setParametro("MATERIALES", i);
                    }
                    else if (i.Equals("MAQUINARIA"))
                    {
                        table.Columns.Add("MAQUINARIA");
                        // setParametro("MAQUINARIA", i);
                    }
                    else if (i.Equals("ADMINISTRATIVOS"))
                    {
                        table.Columns.Add("ADMINISTRATIVOS");
                        // setParametro("ADMINISTRATIVOS", i);
                    }
                    else if (i.Equals("TOTAL"))
                    {
                        table.Columns.Add("TOTAL");
                        // setParametro("TOTAL", i);
                    }
                    else if (i.Equals("TOTAL SIN OVERHAUL"))
                    {
                        table.Columns.Add("TOTALSNOV");
                        //setParametro("TOTALSNOV", i);
                    }
                    else if (i.Equals("HORAS"))
                    {
                        table.Columns.Add("HORAS");
                        //setParametro("HORAS", i);
                    }
                    else
                    {
                        table.Columns.Add("Vacio");
                        // setParametro("Vacio", i);
                    }

                }


                foreach (var x in listNoEco)
                {
                    decimal total = 0;
                    decimal totalsoh = 0;
                    List<string> Row = new List<string>();
                    Row.Add(x);
                    foreach (var y in listTipos)
                    {

                        var valor = raw.FirstOrDefault(z => z.noEco.Equals(x) && z.descripcion.Equals(y));
                        var valorsoh = rawsoh.FirstOrDefault(z => z.noEco.Equals(x) && z.descripcion.Equals(y));
                        if (valor != null)
                        {
                            ///  var val = Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C2");
                            Row.Add(Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C2"));
                        }
                        else
                        {
                            Row.Add(0.ToString("C2"));
                        }

                        total += valor == null ? 0 : Convert.ToDecimal(valor.importe);
                        totalsoh += valorsoh == null ? 0 : Convert.ToDecimal(valorsoh.importe);
                    }
                    //Total
                    Row.Add(total.ToString("C2"));

                    //Total sin overhaul
                    Row.Add(totalsoh.ToString("C2"));

                    //Horas
                    var horometros = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTableByRangeDate(x, fechainicio, fechafin);
                    var horas = horometros.Sum(z => z.HorasTrabajo);

                    Row.Add(horas.ToString("0,0", CultureInfo.InvariantCulture));

                    //Costo horario
                    Row.Add((horas > 0 ? (totalsoh / horas).ToString("C2") : "$0"));

                    table.Rows.Add(Row.ToArray());
                }


            }
            catch (Exception)
            {

                throw;
            }


            return table;
        }

        private decimal changeFormat(string dato)
        {
            decimal num = 0;

            string result2 = dato;

            //int index1 = dato.IndexOf('$');
            //if (index1 != -1)
            //{
            //    result2 = dato.Remove(index1, 1); // Use integer from IndexOf.
            //}
            //int index2 = dato.IndexOf(',');
            //if (index2 != -1)
            //{
            //    result2 = result2.Remove(index2 - 1, index2 - 1); // Use integer from IndexOf.
            //}
            result2 = result2.Replace("$", "");
            result2 = result2.Replace(",", "");
            num = Convert.ToDecimal(result2);
            return num;
        }

        private DataTable getInfoRep(List<InfoCombustibleDTO> raw, DateTime pfecha)
        {
            DataTable table = new DataTable();
            List<string> Row;
            decimal totalGeneral = 0;
            var diasMes = new List<int>();

            var noEconomico = raw.GroupBy(x => x.noEconomico);

            int dias = DateTime.DaysInMonth(pfecha.Year, pfecha.Month);
            table.Columns.Add("Economico");
            table.Columns.Add("Descripcion");
            table.Columns.Add("Serie");

            for (int i = 1; i <= dias; i++)
            {
                table.Columns.Add("D" + i);
                diasMes.Add(i);
            }
            table.Columns.Add("Total");

            foreach (var item in noEconomico)
            {

                var noSerie = raw.FirstOrDefault(x => x.noEconomico.Equals(item.Key)).noSerie;
                Row = new List<string>();
                Row.Add(item.Key);
                Row.Add(item.Key);
                Row.Add(noSerie);

                decimal Total = 0;
                foreach (var j in diasMes)
                {

                    DateTime current = new DateTime(pfecha.Year, pfecha.Month, j);

                    var celda = raw.FirstOrDefault(x => x.noEconomico.Equals(item.Key) && x.fecha.Equals(current));

                    if (celda != null)
                    {
                        Total += celda.total;
                        ///  var val = Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C2");
                        ///
                        Row.Add(Convert.ToDecimal(celda == null ? 0 : celda.total).ToString("0,0", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        Row.Add(0.ToString("0,0", CultureInfo.InvariantCulture));
                    }


                }
                totalGeneral += Total;
                Row.Add(Total.ToString());
                table.Rows.Add(Row.ToArray());

            }

            Row = new List<string>();
            Row.Add("");
            Row.Add("");
            Row.Add("Total Por Dia");
            for (int i = 1; i <= dias; i++)
            {
                decimal totalesDia = 0;
                // var field1 = table.Columns["D1"].ToString();
                foreach (DataRow dtRow in table.Rows)
                {

                    decimal totalDiario = Convert.ToDecimal(dtRow["D" + i + ""]);
                    totalesDia += totalDiario;
                }
                Row.Add(totalesDia.ToString());
            }
            Row.Add(totalGeneral.ToString());



            return table;
        }
        private void setParametro(string nombre, object value)
        {
            ParameterField paramField;
            ParameterDiscreteValue paramDiscreteValue;

            paramField = new ParameterField();
            paramField.Name = nombre;
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = value;
            paramField.CurrentValues.Add(paramDiscreteValue);
            //Agrega el parámetro que se utilizara en el reporte.
            paramFields.Add(paramField);
        }

        /// <summary>
        /// Método utilizado para cargar el encabezado de los reportes, contiene dos parámetros.
        /// </summary>
        /// <param name="nombreReporte">Este parámetro recibe el nombre del reporte.</param>
        /// <param name="area">Este parámetro es del área a la que pertenece el reporte.</param>
        /// <returns></returns>
        private DataTable getInfoEnca(string nombreReporte, string area)
        {
            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoServices().getEncabezadoDatos();
            string path = data.logo;
            byte[] imgdata = File.ReadAllBytes(MapPath(path));
            string empresa = data.nombreEmpresa;
            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rd != null)
            {
                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
        }
        private string HoraToString(int hora)
        {
            string result = "";
            switch (hora)
            {
                case 0:
                    result = "00:00";
                    break;
                case 1:
                    result = "01:00";
                    break;
                case 2:
                    result = "02:00";
                    break;
                case 3:
                    result = "03:00";
                    break;
                case 4:
                    result = "04:00";
                    break;
                case 5:
                    result = "05:00";
                    break;
                case 6:
                    result = "06:00";
                    break;
                case 7:
                    result = "07:00";
                    break;
                case 8:
                    result = "08:00";
                    break;
                case 9:
                    result = "09:00";
                    break;
                case 10:
                    result = "10:00";
                    break;
                case 11:
                    result = "11:00";
                    break;
                case 12:
                    result = "12:00";
                    break;
                case 13:
                    result = "13:00";
                    break;
                case 14:
                    result = "14:00";
                    break;
                case 15:
                    result = "15:00";
                    break;
                case 16:
                    result = "16:00";
                    break;
                case 17:
                    result = "17:00";
                    break;
                case 18:
                    result = "18:00";
                    break;
                case 19:
                    result = "19:00";
                    break;
                case 20:
                    result = "20:00";
                    break;
                case 21:
                    result = "21:00";
                    break;
                case 22:
                    result = "22:00";
                    break;
                case 23:
                    result = "23:00";
                    break;
                default:
                    break;
            }
            return result;
        }
        private List<ConjuntoDatosDTO1> ReturnData(int d)
        {
            List<ConjuntoDatosDTO1> listaResulto = new List<ConjuntoDatosDTO1>();


            ConjuntoDatosDTO1 ns = new ConjuntoDatosDTO1();


            return listaResulto;

        }
        private List<string> metodo(int mes, int anio)
        {

            var periodo = anio;
            var MesInicio = mes;
            List<string> tituloMeses = new List<string>();

            var count = 0;
            for (var i = MesInicio; i < 12; i++)
            {
                count++;

                string fullMonthName = new DateTime(2015, i + 1, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

                var MesFormat = fullMonthName.Substring(0, 3).ToUpper();

                tituloMeses.Add(MesFormat + " " + anio);
            }

            for (var i = 0; i < MesInicio; i++)
            {
                anio = periodo;
                string fullMonthName = new DateTime(2015, i + 1, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

                var MesFormat = fullMonthName.Substring(0, 3).ToUpper();
                anio += 1;
                tituloMeses.Add(MesFormat + " " + anio);
            }

            return tituloMeses;
        }
        public List<string> getAcumulado(int mes, EstadoResultadosDTO UtilidadNeta)
        {
            decimal Temp1 = UtilidadNeta.Fecha1;
            string acumulado = Temp1.ToString("#,##0.##");
            var count = 0;

            List<decimal> ListaUtilidadNeta = new List<decimal>();
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha1);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha2);
            ListaUtilidadNeta.Add(42370);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha4);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha5);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha6);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha7);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha8);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha9);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha10);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha11);
            ListaUtilidadNeta.Add(UtilidadNeta.Fecha12);

            List<string> Acumulado = new List<string>();
            Acumulado.Add(acumulado);

            decimal TotalData = Temp1;
            for (var i = mes; i < 11; i++)
            {
                if (count == 2)
                {
                    TotalData = 42370;
                    acumulado = TotalData.ToString("#,##0.##");
                    Acumulado.Add(acumulado);
                    count++;
                }
                else
                {
                    TotalData += ListaUtilidadNeta[count];
                    acumulado = TotalData.ToString("#,##0.##");
                    Acumulado.Add(acumulado);
                    count++;
                }

            }
            TotalData = 0;
            TotalData += ListaUtilidadNeta[count];
            count++;
            acumulado = TotalData.ToString("#,##0.##");
            Acumulado.Add(acumulado);

            for (var i = 1; i < mes; i++)
            {
                TotalData += ListaUtilidadNeta[count];
                acumulado = TotalData.ToString("#,##0.##");
                Acumulado.Add(acumulado);
                count++;
                // Temp1 += Temp1;

            }

            return Acumulado;
        }
        public double CalcularDiasDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;

            return diferencia.Days;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int reporte = Convert.ToInt32(Request.QueryString["idReporte"]);
            SetInfoReporte(reporte);
            rd.ExportToHttpResponse
           (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Reporte");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int reporte = Convert.ToInt32(Request.QueryString["idReporte"]);
            SetInfoReporte(reporte);
            rd.ExportToHttpResponse
            (CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "Reporte");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int reporte = Convert.ToInt32(Request.QueryString["idReporte"]);
            SetInfoReporte(reporte);
            rd.ExportToHttpResponse
           (CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, "Reporte");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

            int reporte = Convert.ToInt32(Request.QueryString["idReporte"]);
            SetInfoReporte(reporte);
            rd.PrintOptions.PrinterName = "LANIER MP 3350B/LD433B";
            rd.PrintToPrinter(1, false, 0, 0);
        }
        protected string setTitleCCCorto()
        {
            return vSesiones.sesionEmpresaActual == 1 ? "CC" : "AC";
        }
        protected string setTitleCC()
        {
            return vSesiones.sesionEmpresaActual == 1 ? "Centro Costos" : "Area Cuenta";
        }
        protected string setTitleEco()
        {
            return vSesiones.sesionEmpresaActual == 1 ? "No Economico" : "Centro Costos";
        }

    }
}