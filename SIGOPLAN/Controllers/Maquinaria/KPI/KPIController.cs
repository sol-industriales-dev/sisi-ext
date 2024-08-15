using Core.DAO.Principal.Alertas;
using Core.DTO.Maquinaria.Captura.KPI;
using Core.DTO.Maquinaria.KPI.Autorizaciones;
using Core.DTO.Maquinaria.KPI.Captura;
using Core.DTO.Maquinaria.KPI.Dashboard;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Auth;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.KPI;
using Core.Entity.Principal.Alertas;
using Core.Enum.Maquinaria.KPI.CatalogoCodigo;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.KPI;
using Data.Factory.Maquinaria.Reporte;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Reportes.Reports.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.KPI
{
    public class KPIController : BaseController
    {

        private Dictionary<string, object> resultado;
        private KPIFactoryServices kpiFs;
        EncabezadoFactoryServices encabezadoFactoryServices;
    
        private CentroCostosFactoryServices centroCostosFactoryServices;
        private IAlertasDAO alertaFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            kpiFs = new KPIFactoryServices();
            alertaFS = new AlertaFactoryServices().getAlertaService();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            encabezadoFactoryServices = new EncabezadoFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        #region Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetInfoFiltro(FiltroDTO filtro)
        {
            var resultado = kpiFs.KPIFactoruServices().GetInfoFiltro(filtro);

            Session["excelKpiDashboard"] = resultado["excel"];

            resultado.Remove("excel");

            return Json(resultado);
        }

        public JsonResult GetInfoFiltroCodigo(FiltroDTO filtro, string codigo, int tipo)
        {
            var resultado = kpiFs.KPIFactoruServices().GetInfoFiltroCodigo(filtro, codigo, tipo);           

            return Json(resultado);
        }

        public ActionResult GetExcel()
        {
            if (Session["excelKpiDashboard"] != null)
            {
                var stream = (MemoryStream)Session["excelKpiDashboard"];

                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }
        #endregion

        #region Catalogo Codigos de Paro
        public ActionResult CatalogoCodigosParo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCodigoParo(tblM_KPI_CodigosParo codigoParo)
        {
            return Json(kpiFs.KPIFactoruServices().GuardarCodigoParo(codigoParo), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult tlbCodigoParo(string codigoParo)
        {
            return Json(kpiFs.KPIFactoruServices().CargarCodigosParo(codigoParo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCboTiposParo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Enum.GetValues(typeof(Tipo_ParoEnum)).Cast<Tipo_ParoEnum>().ToList().Select(x => new
                {
                    Text = x.GetDescription(),
                    Value = x.GetHashCode()
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Captura KPI homologado

        public ActionResult CapturaKPI()
        {
            return View();
        }
        public ActionResult reporteKPI()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CargarCodigosParo(BusqKpiDiariaDTO busq)
        {
            return View();
        }

        public ActionResult saveOrUpdateCapturaDiaria(List<tblM_KPI_Homologado> captura, List<tblM_CapHorometro> horometros)
        {

            return Json(kpiFs.KPIFactoruServices().saveOrUpdateCapturaDiaria(captura, horometros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturaDiaria(BusqKpiDiariaDTO busq)
        {

            var KpiDiarios = kpiFs.KPIFactoruServices().GetCapturaDiaria(busq);
            return Json(KpiDiarios, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CargarCapturaDiaria(BusqKpiDiariaDTO busq)
        {
            try
            {
                var kpiDiarios = kpiFs.KPIFactoruServices().CargarCapturaDiaria(busq);
                resultado.Add("lst", kpiDiarios);
                resultado.Add(SUCCESS, kpiDiarios.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboGrupoEquipos(string areaCuenta)
        {
            try
            {
                var cboGrupoEquipos = kpiFs.KPIFactoruServices().CboGrupoEquipos(areaCuenta).Select(r => new ComboDTO { Id = r.id.ToString(), Text = r.descripcion, Value = r.id.ToString() }).Distinct();
                resultado.Add(ITEMS, cboGrupoEquipos.OrderBy(r => r.Text));
                resultado.Add(SUCCESS, cboGrupoEquipos.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboSemanas()
        {
            try
            {
                List<ComboDTO> cbo = new List<ComboDTO>();
                int año = DateTime.Now.Year;

                for (int i = 1; i < 54; i++)
                {

                    DateTime fecha1 = DatetimeUtils.primerDiaSemana(año, i).AddDays(1);

                    DateTime fecha2 = fecha1.AddDays(6);

                    cbo.Add(new ComboDTO
                    {
                        Value = i.ToString(),
                        Text = fecha1.ToShortDateString() + " - " + fecha2.ToShortDateString()
                    });
                }
                resultado.Add(ITEMS, cbo);
                resultado.Add(SUCCESS, cbo.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboModeloEquipos(int grupoID)
        {
            try
            {
                var cboModelos = kpiFs.KPIFactoruServices().CboModeloEquipos(grupoID).Select(r => new ComboDTO { Id = r.id.ToString(), Text = r.descripcion, Value = r.id.ToString() });
                resultado.Add(ITEMS, cboModelos);
                resultado.Add(SUCCESS, cboModelos.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Autorizaciones
        public ActionResult Autorizaciones()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CargarAutorizantes(BusqKpiAuthDTO busq)
        {
            try
            {
                var areasCuenta = kpiFs.KPIFactoruServices().ComboAreaCuenta();
                var autorizantes = kpiFs.KPIFactoruServices().CargarAutorizantes(busq);
                var lst = from auth in autorizantes
                          select new
                          {
                              Id = auth.Id,
                              Año = auth.Año,
                              Semana = auth.Semana,
                              Folio = auth.Id.ToString("D10"),
                              AC = auth.AC,
                              Descripcion = areasCuenta.FirstOrDefault(ac => ac.Value == auth.AC).Text,
                              Periodo = auth.fechaInicio.ToShortDateString(),//string.Format("{0} - {1}", busq.min.ToShortDateString(), busq.max.ToShortDateString()),
                              Estatus = auth.AuthEstado.GetDescription(),
                              Comentario = auth.Comentario
                          };
                resultado.Add("lst", lst);
                resultado.Add(SUCCESS, autorizantes.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CargarPendientes(BusqKpiAuthDTO busq)
        {
            try
            {
                var areasCuenta = kpiFs.KPIFactoruServices().ComboAreaCuenta();
                var autorizantes = kpiFs.KPIFactoruServices().CargarPendientes(busq);
                var lst = from auth in autorizantes
                          select new
                          {
                              Id = auth.Id,
                              Año = auth.Año,
                              Semana = auth.Semana,
                              Folio = auth.Id.ToString("D10"),
                              AC = auth.AC,
                              Descripcion = areasCuenta.FirstOrDefault(ac => ac.Value == auth.AC).Text,
                              Periodo = auth.fechaInicio.ToShortDateString(),//string.Format("{0} - {1}", busq.min.ToShortDateString(), busq.max.ToShortDateString()),
                              Estatus = auth.AuthEstado.GetDescription(),
                              Comentario = auth.Comentario
                          };
                resultado.Add("lst", lst);
                resultado.Add(SUCCESS, autorizantes.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private string getPeriodo(int año, int semana)
        {
            var fecha = DatetimeUtils.primerDiaSemana(año, semana);
            var min = fecha.AddDays(1);
            var max = fecha.AddDays(6);

            return min.ToShortDateString() + "-" + max.ToShortDateString();
        }
        [HttpGet]
        public ActionResult CargarCaptura(int idCaptura)
        {
            try
            {
                var tabla = GenerarTablaPresentacion(idCaptura);
                var maxCol = tabla.Max(tbl => tbl.col);
                var maxRow = tabla.Max(tbl => tbl.row.ParseInt());
                var concentrado = GenerarTablaAutorizacionData(idCaptura);

                resultado.Add("lst", tabla);
                resultado.Add("maxCol", maxCol);
                resultado.Add("maxRow", maxRow);
                resultado.Add("concentrado", concentrado);

                resultado.Add(SUCCESS, tabla.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CargarCapturaKPI(DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            try
            {
                var tabla = GenerarTablaRPT(fechaInicio, fechaFin, ac);//GenerarTablaPresentacion(idCaptura);
                var maxCol = tabla.Max(tbl => tbl.col);
                var maxRow = tabla.Max(tbl => tbl.row.ParseInt());
                resultado.Add("lst", tabla);
                resultado.Add("maxCol", maxCol);
                resultado.Add("maxRow", maxRow);
                resultado.Add(SUCCESS, tabla.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AuthCargar(int id)
        {
            try
            {
                var autorizantes = kpiFs.KPIFactoruServices().AuthCargar(id);
                var folio = "Folio: " + id.ToString("D10");
                var comentario = autorizantes.Select(auth => auth.comentario).ToList().ToLine(" ").Replace("'", "").Trim();
                if (!string.IsNullOrEmpty(comentario))
                {
                    folio += " Motivo de Rechazo: " + comentario;
                }
                resultado.Add(AUTORIZANTES, autorizantes);
                resultado.Add(MESSAGE, folio);
                resultado.Add(SUCCESS, autorizantes.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AuthAprueba(authDTO EnTurno)
        {
            try
            {
                #region Actualiza la firma
                var auth = kpiFs.KPIFactoruServices().ConsultaAutorizante(EnTurno.idRegistro);
                var lstCorreosFacultamietos = kpiFs.KPIFactoruServices().GetLstCorreosFacultamientos(EnTurno.areaCuenta);
                var fechaFirma = DateTime.Now;
                EnTurno.authEstado = authEstadoEnum.Autorizado;
                EnTurno.firma = FirmarCadena(EnTurno, fechaFirma);
                switch (EnTurno.orden)
                {
                    case 1:
                        auth.FechaElaboracion = fechaFirma;
                        auth.CadenaElabora = EnTurno.firma;
                        auth.FirmaElabora++;
                        break;
                    case 2:
                        auth.FechaVobo1 = fechaFirma;
                        auth.CadenaVobo1 = EnTurno.firma;
                        auth.FirmaVobo1++;
                        break;
                    case 3:
                        auth.FechaVobo2 = fechaFirma;
                        auth.CadenaVobo2 = EnTurno.firma;
                        auth.FirmaVobo2++;
                        break;
                    case 4:
                        auth.FechaAutoriza = fechaFirma;
                        auth.CadenaAutoriza = EnTurno.firma;
                        auth.FirmaAutoriza++;
                        auth.AuthEstado = authEstadoEnum.Autorizado;
                        break;
                    default:
                        break;
                }
                #endregion
                #region Actualiza al autorizante
                var esGuardado = EsAutorizanteValido(auth);
                if (esGuardado)
                {
                    esGuardado = kpiFs.KPIFactoruServices().GuardarAutorizacion(auth);
                    if (esGuardado)
                    {
                        esGuardado = AlertarSiguienteAutorizante(auth, EnTurno);

                        #region CORREO

                        if (EnTurno.orden == 4)
                        {
                            #region DOCUMENTO PDF
                            ReportDocument rDoc = new rptAutorizacionesCorreo4();
                            string titulo = "";
                            string pFechaPeriodo = "";
                            string pAC = "";
                            var lstDatosDiarios = new List<DatosGraficasDTO>();
                            var lstDatosSemanales = new List<DatosGraficasDTO>();
                            var lstDatosMensuales = new List<DatosGraficasDTO>();

                            string descGraficaDiaria = "";
                            string descGraficaSemanal = "";
                            string descGraficaMensual = "";

                            titulo = "Autorización de KPI Homologado";

                            try
                            {
                                pFechaPeriodo = EnTurno.fechaInicio.ToString("dd/MM/yyyy");
                                pAC = EnTurno.descAreaCuenta;

                                var dictInfoGraficas = kpiFs.KPIFactoruServices().GetInfoGraficasPDF(new FiltroDTO() 
                                {
                                    areaCuenta = EnTurno.areaCuenta,
                                    fechaInicio = EnTurno.fechaInicio
                                });

                                lstDatosDiarios = dictInfoGraficas["gpx_disVsUti_modeloDiario"] as List<DatosGraficasDTO>;
                                lstDatosSemanales = dictInfoGraficas["gpx_disVsUti_modeloSemanal"] as List<DatosGraficasDTO>;
                                lstDatosMensuales = dictInfoGraficas["gpx_disVsUti_modeloMensual"] as List<DatosGraficasDTO>;

                                descGraficaDiaria = dictInfoGraficas["descPeriodoDia"] as string;
                                descGraficaSemanal = dictInfoGraficas["descPeriodoSemana"] as string;
                                descGraficaMensual = dictInfoGraficas["descPeriodoMes"] as string;

                                var reporteDTO = (List<kpiReporteConcentraadoDTO>)Session["rptListaCapturaDiaria"];
                                rDoc.Database.Tables[0].SetDataSource(getInfoEncaCplan(titulo, ""));
                                rDoc.Database.Tables[1].SetDataSource(reporteDTO);
                                rDoc.Database.Tables[2].SetDataSource(lstDatosDiarios);
                                rDoc.Database.Tables[3].SetDataSource(lstDatosMensuales);
                                rDoc.Database.Tables[4].SetDataSource(lstDatosSemanales);

                                rDoc.SetParameterValue("pFechaPeriodo", pFechaPeriodo);
                                rDoc.SetParameterValue("pAC", pAC);
                                rDoc.SetParameterValue("descGraficaDiaria", ("Captura" + (descGraficaDiaria ?? " ")));
                                rDoc.SetParameterValue("descGraficaSemanal", ("Captura semana" + (descGraficaSemanal ?? " ")));
                                rDoc.SetParameterValue("descGraficaMensual", ("Captura mes" + (descGraficaMensual ?? " ")));

                                Stream stream = rDoc.ExportToStream(ExportFormatType.PortableDocFormat);

                                string cuerpoCorreo = "Buen día,<br><br>Se ha autorizado kpi homologado para el dia: " + EnTurno.fechaInicio.ToString("dd/MM/yyyy") + ", Obra: " + EnTurno.descAreaCuenta + ".<br><br>" +
                               "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                               "Maquinaria > Mesa analisis > KPI Homologado > Autorización.<br><br>" +
                               "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                               "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.";

                                var lstCorreosNotificantes = new List<string>() 
                                {
                                    "e.encinas@construplan.com.mx",
                                    "oscar.roman@construplan.com.mx",
                                    //"jose.gaytan@construplan.com.mx",
                                    "oscar.roman@construplan.com.mx",
                                };

                                lstCorreosNotificantes.AddRange(lstCorreosFacultamietos);

#if DEBUG
                                lstCorreosNotificantes = new List<string>() { "miguel.buzani@construplan.com.mx" };
#endif

                                List<byte[]> downloadPDFs = new List<byte[]>();
                                using (var streamReader = new MemoryStream())
                                {
                                    stream.CopyTo(streamReader);
                                    downloadPDFs.Add(streamReader.ToArray());

                                    GlobalUtils.sendEmailAdjuntoInMemory2(("Se ha autorizado kpi homologado para el dia: " + EnTurno.fechaInicio.ToString("dd/MM/yyyy") + ", Obra: " + EnTurno.descAreaCuenta),
                                        cuerpoCorreo, lstCorreosNotificantes, downloadPDFs, "KpiPdf.pdf");

                                }
                            }
                            catch (Exception)
                            {
                                pFechaPeriodo = "";
                            }

                            #region V1
                            ////imagenBase64.Split(',')[1]
                            //byte[] imagenBytesDiaria = Convert.FromBase64String(EnTurno.imagen64GraficaDiario.Split(',')[1]);
                            //byte[] imagenBytesSemanal = Convert.FromBase64String(EnTurno.imagen64GraficaSemanal.Split(',')[1]);
                            //byte[] imagenBytesMensual = Convert.FromBase64String(EnTurno.imagen64GraficaMensual.Split(',')[1]);

                            //var objGraficaDiaria = new
                            //{
                            //    img = imagenBytesDiaria,
                            //    desc = ("Captura" + (EnTurno.descPeriodoDia ?? " ")),
                            //};

                            //var objGraficaSemanal = new
                            //{
                            //    img = imagenBytesSemanal,
                            //    desc = ("Captura semana" + (EnTurno.descPeriodoSemana ?? " ")),
                            //};

                            //var objGraficaMensual = new
                            //{
                            //    img = imagenBytesMensual,
                            //    desc = ("Captura mes" + (EnTurno.descPeriodoMes ?? " ")),
                            //};
                            #endregion

                            
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion
                resultado.Add(SUCCESS, esGuardado);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AuthRechaza(authDTO EnTurno)
        {
            try
            {
                #region Actualiza la firma
                var auth = kpiFs.KPIFactoruServices().ConsultaAutorizante(EnTurno.idRegistro);
                var fechaFirma = DateTime.Now;
                auth.AuthEstado = EnTurno.authEstado = authEstadoEnum.Rechazado;
                auth.Comentario = EnTurno.comentario;
                EnTurno.firma = FirmarCadena(EnTurno, fechaFirma);
                switch (EnTurno.orden)
                {
                    case 1:
                        auth.CadenaElabora = EnTurno.firma;
                        auth.FechaElaboracion = fechaFirma;
                        auth.FirmaElabora++;
                        break;
                    case 2:
                        auth.CadenaVobo1 = EnTurno.firma;
                        auth.FechaVobo1 = fechaFirma;
                        auth.FirmaVobo1++;
                        break;
                    case 3:
                        auth.CadenaVobo2 = EnTurno.firma;
                        auth.FechaVobo2 = fechaFirma;
                        auth.FirmaVobo2++;
                        break;
                    case 4:
                        auth.CadenaAutoriza = EnTurno.firma;
                        auth.FechaAutoriza = fechaFirma;
                        auth.FirmaAutoriza++;
                        break;
                    default:
                        break;
                }
                #endregion
                #region Actualiza al autorizante
                var esGuardado = EsAutorizanteValido(auth);
                if (esGuardado)
                {
                    esGuardado = kpiFs.KPIFactoruServices().GuardarAutorizacion(auth);
                    if (esGuardado)
                    {
                        esGuardado = QuitarAlertasAlRechazar(EnTurno);
                    }
                }
                #endregion
                resultado.Add(SUCCESS, esGuardado);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Auxiliares
        [HttpGet]
        public ActionResult CargarComboxAutorizantes()
        {
            try
            {
                var itemsAuth = from estado in EnumExtensions.ToCombo<authEstadoEnum>()
                                where estado.Value != 3
                                select estado;
                var itemsPeriodo = kpiFs.KPIFactoruServices().ComboPeriodo();
                var itemsAreaCuenta = kpiFs.KPIFactoruServices().ComboAreaCuenta();
                resultado.Add("itemsAuth", itemsAuth);
                resultado.Add("itemsPeriodo", itemsPeriodo);
                resultado.Add("itemsAreaCuenta", itemsAreaCuenta);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        private string FirmarCadena(authDTO auth, DateTime fechaFirma)
        {
            var letra = string.Empty;
            switch (auth.authEstado)
            {
                case authEstadoEnum.Autorizado:
                    letra = "A";
                    break;
                case authEstadoEnum.Rechazado:
                    letra = "R";
                    break;
                case authEstadoEnum.EnEspera:
                case authEstadoEnum.EnTurno:
                default:
                    return string.Empty;
            }
            var firma = string.Format("{0}|{1:dd}|{1:MM}|{1:yyyy}|{1:hh}|{1:mm}|{2}|{3}", auth.idRegistro, fechaFirma, auth.idAuth, letra);
            return firma;
        }
        private bool EsAutorizanteValido(tblM_KPI_AuthHomologado auth)
        {
            var mensaje = string.Empty;
            var estadosValidos = new List<authEstadoEnum>{
                authEstadoEnum.Autorizado,
                authEstadoEnum.EnEspera,
                authEstadoEnum.Rechazado
            };
            //if (auth.UsuarioElaboraID == 0 || auth.UsuarioVobo1 == 0 || auth.UsuarioVobo2 == 0 || auth.UsuarioAutoriza == 0)
            //{
            //    mensaje += "Faltan usuarios autorizantes por asignar. ";
            //}

            if (!estadosValidos.Contains(auth.AuthEstado))
            {
                mensaje += "El estado del registro no es valido. ";
            }
            if (auth.AuthEstado == authEstadoEnum.Rechazado && (string.IsNullOrEmpty(auth.Comentario) || auth.Comentario.Trim().Length < 5))
            {
                mensaje += "El comentario de rechazo es muy corto. ";
            }
            if (string.IsNullOrEmpty(auth.AC) || !auth.AC.Contains("-"))
            {
                mensaje += "Asigne un Area Cuenta existente. ";
            }
            if (!auth.Activo)
            {
                mensaje += "El registro no está activo. ";
            }
            if (mensaje.Length > 0)
            {
                throw new System.InvalidOperationException(mensaje);
            }
            return mensaje.Length == 0;
        }
        private bool AlertarSiguienteAutorizante(tblM_KPI_AuthHomologado auth, authDTO enTurno)
        {
            #region Verifica estatus
            if (auth.AuthEstado != authEstadoEnum.EnEspera)
            {
                return true;
            }
            #endregion
            #region Asignar visto a Alerta EnTurno
            var alertas = (from alert in alertaFS.getAlertasBySistema((int)SistemasEnum.MAQUINARIA)
                           where alert.moduloID == (int)BitacoraEnum.KPHomologadoAutorizacion && alert.objID == auth.Id && alert.userRecibeID == enTurno.idAuth
                           select alert).ToList();
            alertas.ForEach(alert => alertaFS.updateAlerta(alert));
            #endregion
            #region Crea al siguiente Autorizante
            var siguiente = new authDTO();
            switch (enTurno.orden)
            {
                case 1:
                    siguiente = new authDTO
                    {
                        idRegistro = auth.Id,
                        idPadre = auth.Id,
                        idAuth = auth.UsuarioVobo1,
                        orden = 2,
                        descripcion = "Vobo 1",
                        firma = auth.CadenaVobo1 ?? string.Empty,
                        clase = string.Empty,
                    };
                    break;
                case 2:
                    siguiente = new authDTO
                    {
                        idRegistro = auth.Id,
                        idPadre = auth.Id,
                        idAuth = auth.UsuarioVobo2,
                        orden = 3,
                        descripcion = "Vobo 2",
                        firma = auth.CadenaVobo2 ?? string.Empty,
                        clase = string.Empty,
                    };
                    break;
                case 3:
                    siguiente = new authDTO
                    {
                        idRegistro = auth.Id,
                        idPadre = auth.Id,
                        idAuth = auth.UsuarioAutoriza,
                        orden = 4,
                        descripcion = "Autorizante",
                        firma = auth.CadenaAutoriza ?? string.Empty,
                        clase = string.Empty,
                    };
                    break;
                case 4:
                default:
                    break;
            }
            #endregion
            #region Crear Alerta siguiente
            if (siguiente.idAuth > 0)
            {
                var alerta = new tblP_Alerta()
                {
                    msj = "Firma-KPI  " + auth.Id.ToString("D10"),
                    sistemaID = (int)SistemasEnum.MAQUINARIA,
                    documentoID = 0,
                    moduloID = (int)BitacoraEnum.KPHomologadoAutorizacion,
                    tipoAlerta = (int)AlertasEnum.REDIRECCION,
                    url = "/KPI/Autorizaciones?idAuth=" + auth.Id + "",
                    objID = auth.Id,
                    userEnviaID = enTurno.idAuth,
                    userRecibeID = siguiente.idAuth,
                    visto = false
                };
                alertaFS.saveAlerta(alerta);
            }
            #endregion
            return true;
        }
        private bool QuitarAlertasAlRechazar(authDTO enTurno)
        {
            #region Verificar estado
            if (enTurno.authEstado != authEstadoEnum.Rechazado)
            {
                return true;
            }
            #endregion
            #region Asignar visto a Alerta EnTurno
            var alertas = (from alert in alertaFS.getAlertasBySistema((int)SistemasEnum.MAQUINARIA)
                           where alert.moduloID == (int)BitacoraEnum.KPHomologadoAutorizacion && alert.objID == enTurno.idRegistro && alert.userRecibeID == enTurno.idAuth
                           select alert).ToList();
            alertas.ForEach(alert => alertaFS.updateAlerta(alert));
            #endregion
            return true;
        }

        public List<kpiReporteConcentraadoDTO> GenerarTablaAutorizacionData(int id)
        {
            var capturas = kpiFs.KPIFactoruServices().CargarCapturaBit(id);
            /*var codigos = kpiFs.KPIFactoruServices().CodigosParo(capturas.FirstOrDefault().ac);
            var idEconomicos = from cap in capturas select cap.idEconomico;
            var economicos = kpiFs.KPIFactoruServices().CargarMaquinas(idEconomicos.ToList());
            var grupos = (from eco in economicos
                          group eco by eco.grupoMaquinaria into gpo
                          orderby gpo.Key.descripcion
                          select gpo).ToList();*/
            var bitE = kpiFs.KPIFactoruServices().getHomologadobit(id);

            decimal tiempo = kpiFs.KPIFactoruServices().getHorasDiaDec(bitE.ac);

            var dataM = capturas;//_context.tblM_KPI_Homologado.Where(r => r.ac == ac && (grupoID != 0 ? r.idGrupo == grupoID : true) && (modeloID != 0 ? r.idModelo == modeloID : true) && (r.fecha >= fechaInicio && r.fecha <= fechaFin)).ToList();
            var infoGrupos = dataM.GroupBy(r => new { r.economico }).
                                Select(f => new kpiReporteConcentraadoDTO
                                {
                                    economico = f.Key.economico,
                                    horasTrabajadas = Math.Round(dataM.Where(r => r.economico == f.Key.economico && (int)Tipo_ParoEnum.trabajo == r.idTipoParo).Sum(s => s.valor), 2),
                                    horasMMTO = Math.Round(dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor), 2),//f.Where(r => r.idEconomico == f.Key.idEconomicos.FirstOrDefault() && ).Sum(s => s.valor),
                                    horasReserva = Math.Round(dataM.Where(r => r.economico == f.Key.economico && r.idTipoParo == (int)Tipo_ParoEnum.sin_utilizar).Sum(s => s.valor), 2),//f.Where(r => r.idEconomico == f.Key.idEconomicos.FirstOrDefault() && r.tipoParo == (int)Tipo_ParoEnum.).Sum(s => s.valor),
                                    horasDemora = Math.Round(dataM.Where(r => r.economico == f.Key.economico && r.idTipoParo == (int)Tipo_ParoEnum.demoras).Sum(s => s.valor), 2),
                                    disponibilidad = Math.Round(((tiempo - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor)) / tiempo), 2) * 100,
                                    utilizacion = (tiempo - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor)) == 0 ? 0 : Math.Round(dataM.Where(r => r.economico == f.Key.economico && (int)Tipo_ParoEnum.trabajo == r.idTipoParo).Sum(s => s.valor) / (tiempo - dataM.Where(r => r.economico == f.Key.economico && (r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_programado || r.idTipoParo == (int)Tipo_ParoEnum.mantenimiento_no_programado)).Sum(s => s.valor)), 2) * 100,
                                    horasTotales = tiempo
                                }).ToList();

            Session["rptListaCapturaDiaria"] = infoGrupos;
            Session["fechaTemp"] = bitE.fechaInicio.ToShortDateString();
            Session["acTemp"] = bitE.ac + ' ' + centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(bitE.ac); 
            return infoGrupos.ToList();
        }

        private List<KPITblPresentacionDTO> GenerarTablaPresentacion(int id)
        {
            var tabla = new List<KPITblPresentacionDTO>();
            var capturas = kpiFs.KPIFactoruServices().CargarCapturaBit(id);
            var codigos = kpiFs.KPIFactoruServices().CodigosParo(capturas.FirstOrDefault().ac);
            var idEconomicos = from cap in capturas select cap.idEconomico;
            var economicos = kpiFs.KPIFactoruServices().CargarMaquinas(idEconomicos.ToList());
            var grupos = (from eco in economicos
                          group eco by eco.grupoMaquinaria into gpo
                          orderby gpo.Key.descripcion
                          select gpo).ToList();
            #region Encabezado fijo
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "A",
                row = "5",
                valor = "Código",
                clase = "text-center",
                color = new ColorDTO().Amarillo()
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "1",
                clase = "text-center",
                valor = "Equipo"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "2",
                clase = "text-center",
                valor = "Modelo"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "3",
                clase = "text-center",
                valor = "No. Económico"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "5",
                valor = "Categoría / Descripción",
                clase = "text-center",
                color = new ColorDTO().Verde()
            });
            #endregion
            #region Conceptos
            var iCol = "B";
            var iRow = "7";
            var tipos = EnumExtensions.ToCombo<Tipo_ParoEnum>();
            tipos.ForEach(tipo =>
            {
                var tipoEnum = (Tipo_ParoEnum)tipo.Value;
                var colorTipo = new ColorDTO();
                var descripcionTotal = string.Empty;
                switch (tipoEnum)
                {
                    case Tipo_ParoEnum.trabajo:
                        colorTipo = colorTipo.AzulRey();
                        descripcionTotal = "Total Tiempo de Trabajo (WK)";
                        break;
                    case Tipo_ParoEnum.demoras:
                        colorTipo = colorTipo.VerdeClarito();
                        descripcionTotal = "Total Demoras Operativas (DL)";
                        break;
                    case Tipo_ParoEnum.sin_utilizar:
                        colorTipo = colorTipo.Amarillo();
                        descripcionTotal = "Total Sin Utilizar (Parado) - (ID)";
                        break;
                    case Tipo_ParoEnum.mantenimiento_programado:
                        colorTipo = colorTipo.Naranja();
                        descripcionTotal = "Total Mantenimiento Programado (SM)";
                        break;
                    case Tipo_ParoEnum.mantenimiento_no_programado:
                        colorTipo = colorTipo.VerdeOscuro();
                        descripcionTotal = "Total Mantenimiento No Programado (UM)";
                        break;
                    default:
                        break;
                }
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    clase = "text-center",
                    color = colorTipo
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    valor = tipoEnum.GetDescription(),
                    clase = "text-center",
                    color = colorTipo
                });
                iRow = RowSiguiente(iRow);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                iRow = RowSiguiente(iRow);
                var codigosTipo = codigos.Where(cod => cod.tipoParo == tipoEnum.ParseInt()).ToList();
                codigosTipo.ForEach(codigo =>
                {
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = "A",
                        row = iRow,
                        valor = codigo.codigo,
                        color = new ColorDTO().GrisAzulito()
                    });
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = "B",
                        row = iRow,
                        valor = codigo.descripcion,
                        color = new ColorDTO().GrisClaro()
                    });
                    iRow = RowSiguiente(iRow);
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                iRow = RowSiguiente(iRow);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    clase = "text-center",
                    color = new ColorDTO().GrisAzulito()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    valor = descripcionTotal,
                    clase = "text-center",
                    color = new ColorDTO().GrisAzulito()
                });
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
            });
            #endregion
            #region Grupos
            iCol = "D";
            var iRowProm = string.Empty;
            grupos.ForEach(grupo =>
            {
                iRow = "1";
                var gpoCount = grupo.Count();
                var gpoColor = new ColorDTO().Rosa();
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRow,
                    valor = grupo.Key.descripcion,
                    clase = "text-center",
                    colSpan = gpoCount,
                    color = gpoColor
                });
                grupo.ToList().ForEach(economico =>
                {
                    iRow = "2";
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRow,
                        valor = economico.modeloEquipo.descripcion,
                        clase = "text-center",
                        colSpan = gpoCount,
                        color = new ColorDTO().Rosa(),
                    });
                    iRow = RowSiguiente(iRow);
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRow,
                        valor = economico.noEconomico,
                        clase = "text-center",
                        color = new ColorDTO().VerdeClarito(),
                    });
                    iRow = (iRow.ParseInt() + 5).ToString();
                    iRowProm = iRow;
                    #region Cuerpo
                    tipos.ForEach(tipo =>
                    {
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            color = new ColorDTO().Gris(),
                        });
                        iRow = RowSiguiente(iRow);
                        var codigosTipo = codigos.Where(cod => cod.tipoParo == tipo.Value).ToList();
                        codigosTipo.ForEach(codigo =>
                        {
                            var valorEconomico = capturas.Any(cap => cap.idEconomico == economico.id && cap.codigoParo == codigo.codigo) ? capturas.Where(cap => cap.idEconomico == economico.id && cap.codigoParo == codigo.codigo).Sum(s => s.valor) : 0;
                            tabla.Add(new KPITblPresentacionDTO
                            {
                                col = iCol,
                                row = iRow,
                                clase = "text-right",
                                valor = valorEconomico == 0 ? "-" : valorEconomico.ToString(),
                                color = new ColorDTO().AzulBajito(),
                            });
                            iRow = RowSiguiente(iRow);
                        });
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            color = new ColorDTO().Gris(),
                        });
                        iRow = RowSiguiente(iRow);
                        var totalEconomico = capturas.Where(cap => cap.idEconomico == economico.id && codigosTipo.Any(ct => ct.codigo == cap.codigoParo)).Sum(s => s.valor);
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            clase = "text-right",
                            valor = totalEconomico == 0 ? "-" : totalEconomico.ToString(),
                            color = new ColorDTO().GrisAzulito(),
                        });
                        iRow = RowSiguiente(iRow);
                        iRow = RowSiguiente(iRow);
                        iRow = RowSiguiente(iRow);
                    });
                    #endregion
                    iCol = ColSiguiente(iCol);
                });
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = iCol,
                row = "3",
                valor = "Promedio",
                clase = "text-center",
                color = new ColorDTO().Gris(),
            });
            #region Promedio
            tipos.ForEach(tipo =>
            {
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    color = new ColorDTO().Gris(),
                });
                var totalPromedio = new List<decimal>();
                var codigosTipo = codigos.Where(cod => cod.tipoParo == tipo.Value).ToList();
                codigosTipo.ForEach(codigo =>
                {
                    iRowProm = RowSiguiente(iRowProm);
                    var totalTipo = capturas.Any(cap => cap.codigoParo == codigo.codigo) ? capturas.Where(cap => cap.codigoParo == codigo.codigo).Sum(s => s.valor) : 0;
                    var countTipo = (decimal)codigos.Count;
                    totalTipo = countTipo == 0 ? 0 : decimal.Divide(totalTipo, countTipo);
                    totalPromedio.Add(totalTipo);
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRowProm,
                        clase = "text-right",
                        valor = totalTipo == 0 ? "-" : totalTipo.ToString(),
                        color = new ColorDTO().AzulBajito(),
                    });
                });
                iRowProm = RowSiguiente(iRowProm);
                var totalTotalPromedio = totalPromedio.Sum();
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    color = new ColorDTO().Gris(),
                });
                iRowProm = RowSiguiente(iRowProm);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    clase = "text-right",
                    valor = totalTotalPromedio == 0 ? "-" : totalTotalPromedio.ToString(),
                    color = new ColorDTO().GrisAzulito(),
                });
                iRowProm = RowSiguiente(iRowProm);
                iRowProm = RowSiguiente(iRowProm);
                iRowProm = RowSiguiente(iRowProm);
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
            #endregion
            });
            iCol = ColSiguiente(iCol);
            #endregion
            var gpoTabla = (from tbl in tabla
                            group tbl by new { tbl.row, tbl.col } into gpo
                            where gpo.Count() > 1
                            select new
                            {
                                col = gpo.Key.col,
                                row = gpo.Key.row,
                                count = gpo.Count(),
                                lst = gpo.ToList(),
                            }).ToList();
            return tabla;
        }

        private List<KPITblPresentacionDTO> GenerarTablaRPT(DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            var tabla = new List<KPITblPresentacionDTO>();
            var capturas = kpiFs.KPIFactoruServices().CargarCapturaBitFechas(fechaInicio, fechaFin, ac);
            var codigos = kpiFs.KPIFactoruServices().CodigosParo(capturas.FirstOrDefault().ac);
            var idEconomicos = from cap in capturas select cap.idEconomico;
            var economicos = kpiFs.KPIFactoruServices().CargarMaquinas(idEconomicos.ToList());
            var grupos = (from eco in economicos
                          group eco by eco.grupoMaquinaria into gpo
                          orderby gpo.Key.descripcion
                          select gpo).ToList();
            #region Encabezado fijo
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "A",
                row = "5",
                valor = "Código",
                clase = "text-center",
                color = new ColorDTO().Amarillo()
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "1",
                clase = "text-center",
                valor = "Equipo"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "2",
                clase = "text-center",
                valor = "Modelo"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "3",
                clase = "text-center",
                valor = "No. Económico"
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = "B",
                row = "5",
                valor = "Categoría / Descripción",
                clase = "text-center",
                color = new ColorDTO().Verde()
            });
            #endregion
            #region Conceptos
            var iCol = "B";
            var iRow = "7";
            var tipos = EnumExtensions.ToCombo<Tipo_ParoEnum>();
            tipos.ForEach(tipo =>
            {
                var tipoEnum = (Tipo_ParoEnum)tipo.Value;
                var colorTipo = new ColorDTO();
                var descripcionTotal = string.Empty;
                switch (tipoEnum)
                {
                    case Tipo_ParoEnum.trabajo:
                        colorTipo = colorTipo.AzulRey();
                        descripcionTotal = "Total Tiempo de Trabajo (WK)";
                        break;
                    case Tipo_ParoEnum.demoras:
                        colorTipo = colorTipo.VerdeClarito();
                        descripcionTotal = "Total Demoras Operativas (DL)";
                        break;
                    case Tipo_ParoEnum.sin_utilizar:
                        colorTipo = colorTipo.Amarillo();
                        descripcionTotal = "Total Sin Utilizar (Parado) - (ID)";
                        break;
                    case Tipo_ParoEnum.mantenimiento_programado:
                        colorTipo = colorTipo.Naranja();
                        descripcionTotal = "Total Mantenimiento Programado (SM)";
                        break;
                    case Tipo_ParoEnum.mantenimiento_no_programado:
                        colorTipo = colorTipo.VerdeOscuro();
                        descripcionTotal = "Total Mantenimiento No Programado (UM)";
                        break;
                    default:
                        break;
                }
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    clase = "text-center",
                    color = colorTipo
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    valor = tipoEnum.GetDescription(),
                    clase = "text-center",
                    color = colorTipo
                });
                iRow = RowSiguiente(iRow);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                iRow = RowSiguiente(iRow);
                var codigosTipo = codigos.Where(cod => cod.tipoParo == tipoEnum.ParseInt()).ToList();
                codigosTipo.ForEach(codigo =>
                {
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = "A",
                        row = iRow,
                        valor = codigo.codigo,
                        color = new ColorDTO().GrisAzulito()
                    });
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = "B",
                        row = iRow,
                        valor = codigo.descripcion,
                        color = new ColorDTO().GrisClaro()
                    });
                    iRow = RowSiguiente(iRow);
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    color = new ColorDTO().Gris()
                });
                iRow = RowSiguiente(iRow);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "A",
                    row = iRow,
                    clase = "text-center",
                    color = new ColorDTO().GrisAzulito()
                });
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = "B",
                    row = iRow,
                    valor = descripcionTotal,
                    clase = "text-center",
                    color = new ColorDTO().GrisAzulito()
                });
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
            });
            #endregion
            #region Grupos
            iCol = "D";
            var iRowProm = string.Empty;
            grupos.ForEach(grupo =>
            {
                iRow = "1";
                var gpoCount = grupo.Count();
                var gpoColor = new ColorDTO().Rosa();
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRow,
                    valor = grupo.Key.descripcion,
                    clase = "text-center",
                    colSpan = gpoCount,
                    color = gpoColor
                });
                grupo.ToList().ForEach(economico =>
                {
                    iRow = "2";
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRow,
                        valor = economico.modeloEquipo.descripcion,
                        clase = "text-center",
                        colSpan = gpoCount,
                        color = new ColorDTO().Rosa(),
                    });
                    iRow = RowSiguiente(iRow);
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRow,
                        valor = economico.noEconomico,
                        clase = "text-center",
                        color = new ColorDTO().VerdeClarito(),
                    });
                    iRow = (iRow.ParseInt() + 5).ToString();
                    iRowProm = iRow;
                    #region Cuerpo
                    tipos.ForEach(tipo =>
                    {
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            color = new ColorDTO().Gris(),
                        });
                        iRow = RowSiguiente(iRow);
                        var codigosTipo = codigos.Where(cod => cod.tipoParo == tipo.Value).ToList();
                        codigosTipo.ForEach(codigo =>
                        {
                            var valorEconomico = capturas.Any(cap => cap.idEconomico == economico.id && cap.codigoParo == codigo.codigo) ? capturas.Where(cap => cap.idEconomico == economico.id && cap.codigoParo == codigo.codigo).Sum(s => s.valor) : 0;
                            tabla.Add(new KPITblPresentacionDTO
                            {
                                col = iCol,
                                row = iRow,
                                clase = "text-right",
                                valor = valorEconomico == 0 ? "-" : valorEconomico.ToString(),
                                color = new ColorDTO().AzulBajito(),
                            });
                            iRow = RowSiguiente(iRow);
                        });
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            color = new ColorDTO().Gris(),
                        });
                        iRow = RowSiguiente(iRow);
                        var totalEconomico = capturas.Where(cap => cap.idEconomico == economico.id && codigosTipo.Any(ct => ct.codigo == cap.codigoParo)).Sum(s => s.valor);
                        tabla.Add(new KPITblPresentacionDTO
                        {
                            col = iCol,
                            row = iRow,
                            clase = "text-right",
                            valor = totalEconomico == 0 ? "-" : totalEconomico.ToString(),
                            color = new ColorDTO().GrisAzulito(),
                        });
                        iRow = RowSiguiente(iRow);
                        iRow = RowSiguiente(iRow);
                        iRow = RowSiguiente(iRow);
                    });
                    #endregion
                    iCol = ColSiguiente(iCol);
                });
            });
            tabla.Add(new KPITblPresentacionDTO
            {
                col = iCol,
                row = "3",
                valor = "Promedio",
                clase = "text-center",
                color = new ColorDTO().Gris(),
            });
            #region Promedio
            tipos.ForEach(tipo =>
            {
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    color = new ColorDTO().Gris(),
                });
                var totalPromedio = new List<decimal>();
                var codigosTipo = codigos.Where(cod => cod.tipoParo == tipo.Value).ToList();
                codigosTipo.ForEach(codigo =>
                {
                    iRowProm = RowSiguiente(iRowProm);
                    var totalTipo = capturas.Any(cap => cap.codigoParo == codigo.codigo) ? capturas.Where(cap => cap.codigoParo == codigo.codigo).Sum(s => s.valor) : 0;
                    var countTipo = (decimal)codigos.Count;
                    totalTipo = countTipo == 0 ? 0 : decimal.Divide(totalTipo, countTipo);
                    totalPromedio.Add(totalTipo);
                    tabla.Add(new KPITblPresentacionDTO
                    {
                        col = iCol,
                        row = iRowProm,
                        clase = "text-right",
                        valor = totalTipo == 0 ? "-" : totalTipo.ToString(),
                        color = new ColorDTO().AzulBajito(),
                    });
                });
                iRowProm = RowSiguiente(iRowProm);
                var totalTotalPromedio = totalPromedio.Sum();
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    color = new ColorDTO().Gris(),
                });
                iRowProm = RowSiguiente(iRowProm);
                tabla.Add(new KPITblPresentacionDTO
                {
                    col = iCol,
                    row = iRowProm,
                    clase = "text-right",
                    valor = totalTotalPromedio == 0 ? "-" : totalTotalPromedio.ToString(),
                    color = new ColorDTO().GrisAzulito(),
                });
                iRowProm = RowSiguiente(iRowProm);
                iRowProm = RowSiguiente(iRowProm);
                iRowProm = RowSiguiente(iRowProm);
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
                iRow = RowSiguiente(iRow);
            #endregion
            });
            iCol = ColSiguiente(iCol);
            #endregion
            var gpoTabla = (from tbl in tabla
                            group tbl by new { tbl.row, tbl.col } into gpo
                            where gpo.Count() > 1
                            select new
                            {
                                col = gpo.Key.col,
                                row = gpo.Key.row,
                                count = gpo.Count(),
                                lst = gpo.ToList(),
                            }).ToList();
            return tabla;
        }
        #endregion
        private string ColSiguiente(string col)
        {
            if (col == "")
                return "A";
            string fPart = col.Substring(0, col.Length - 1);
            char lChar = col[col.Length - 1];
            if (lChar == 'Z')
                return ColSiguiente(fPart) + "A";
            return fPart + ++lChar;
        }
        private string RowSiguiente(string row)
        {
            return (row.ParseInt() + 1).ToString();
        }
        public ActionResult GuardarSemana(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            return Json(kpiFs.KPIFactoruServices().GuardarSemana(ac, fechaInicio, fechaFinal, semana), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidarConcentrado(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            return Json(kpiFs.KPIFactoruServices().ValidarConcentrado(ac, fechaInicio, fechaFinal, semana), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboCC = kpiFs.KPIFactoruServices().FillCboCC();
                result.Add(ITEMS, cboCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboGrupos = kpiFs.KPIFactoruServices().FillCboGrupos();
                result.Add(ITEMS, cboGrupos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModelos(List<int> lstGrupoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboModelos = kpiFs.KPIFactoruServices().FillCboModelos(lstGrupoID);
                result.Add(ITEMS, cboModelos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEconomico(List<string> lstCC, List<int> lstGrupoID, List<int> lstModeloID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboEconomico = kpiFs.KPIFactoruServices().FillCboEconomico(lstCC, lstGrupoID, lstModeloID);
                result.Add(ITEMS, cboEconomico);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboGruposEnCaptura(List<string> lstCC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboGrupos = kpiFs.KPIFactoruServices().FillCboGruposEnCaptura(lstCC);
                result.Add(ITEMS, cboGrupos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModelosEnCaptura(List<string> lstCC,List<int> lstGrupoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboModelos = kpiFs.KPIFactoruServices().FillCboModelosEnCaptura(lstCC,lstGrupoID);
                result.Add(ITEMS, cboModelos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region concentrado kpi

        public ActionResult ConcentradoRptKPI()
        {
            return View();
        }

        public ActionResult GetConcentradoKPI(string ac, int grupoID, int modeloID, string fechaInicio, string fechaFin)
        {

            DateTime fInicio = Convert.ToDateTime(fechaInicio);
            DateTime fFin = Convert.ToDateTime(fechaFin);
            return Json(kpiFs.KPIFactoruServices().GetConcentradoKPI(ac, grupoID, modeloID, fInicio, fFin), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getHorasDia(string ac)
        {

            return Json(kpiFs.KPIFactoruServices().getHorasDia(ac));
        }
        #endregion
        
        #region REPORTE AUTORIZACIONES
        public ActionResult GetInfoGraficasPDF(FiltroDTO filtro)
        {
            return Json(kpiFs.KPIFactoruServices().GetInfoGraficasPDF(filtro), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES CRISTALREPORTS
        public DataTable getInfoEncaCplan(string nombreReporte, string area)
        {
            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoServices().getEncabezadoDatosCplan();
            string path = data.logo;
            byte[] imgdata = System.IO.File.ReadAllBytes(Server.MapPath(path));
            string empresa = data.nombreEmpresa;

            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }
        #endregion
    }
}