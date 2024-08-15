using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGOPLAN.Controllers;
using System.Globalization;
using Core.Entity.Maquinaria.BackLogs;
using Core.DAO.Maquinaria.BackLogs;
using Data.Factory.Maquinaria.BackLogs;
using Core.Enum;
using Infrastructure.Utils;
using System.IO;
using Core.Enum.Maquinaria.BackLogs;
using Core.DTO.Maquinaria.BackLogs;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
using Core.Entity.Maquinaria;
using Core.DTO.Maquinaria;
using Core.Enum.Maquinaria;
using Core.DTO.Principal.Generales;
using DotnetDaddy.DocumentViewer;

namespace SIGOPLAN.Controllers.Maquinaria.BackLogs
{
    public class BackLogsController : BaseController
    {
        #region INIT
        IBackLogsDAO BackLogsFactoryServices;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BackLogsFactoryServices = new BackLogsFactoryServices().GetBackLogs();
        }

        public PartialViewResult _visorGrid()
        {
            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "", Zoom = 40, TimeOut = 20 };

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return PartialView();
        }
        #endregion

        #region BACKLOGS OBRA
        bool BanderaCrearEditarBackLog = false;
        bool BanderaCrearEditarParte = false;
        bool BanderaCrearEditarManoObra = false;
        bool FolioBLDisponible = false;
        string folioBL = string.Empty;

        public ActionResult Index(string areaCuenta)
        {
            BackLogsFactoryServices.VerificarEntradaAlmacenOC();
            BackLogsFactoryServices.VerificarTraspasosBL(areaCuenta);
            return View();
        }

        public ActionResult RegistroBacklogsObra()
        {
            return View();
        }

        public ActionResult ProgramaInspeccionBackLogs()
        {
            return View();
        }

        public ActionResult InformeBackLogsRehabilitacion()
        {
            return View();
        }

        public ActionResult ReporteIndicadoresObra()
        {
            return View();
        }

        public ActionResult IndicadoresRehabilitacionTMC()
        {
            return View();
        }

        #region INDEX
        public ActionResult GetBackLogsGraficaIndex(BackLogsDTO objDTO)
        {
            return Json(BackLogsFactoryServices.GetBackLogsGraficaIndex(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReporte(GenerarReporteDTO objGenerarReporteDTO)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                Session["lstTablaDatosBLActuales"] = objGenerarReporteDTO.lstTablaDatosBLActuales.ToList();
                Session["lstTablaDatosBLPorc"] = objGenerarReporteDTO.lstTablaDatosBLPorc.ToList();
                Session["lstTablaDatosBLTiempoPromedio"] = objGenerarReporteDTO.lstTablaDatosBLTiempoPromedio.ToList();
                Session["lstTendenciaBLRegistrados"] = objGenerarReporteDTO.lstTendenciaBLRegistrados.ToList();
                Session["lstTendenciaBLCerrados"] = objGenerarReporteDTO.lstTendenciaBLCerrados.ToList();
                Session["lstTendenciaBLAcumulados"] = objGenerarReporteDTO.lstTendenciaBLAcumulados.ToList();
                Session["img"] = objGenerarReporteDTO.grafica;
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REGISTRO BACKLOGS
        public ActionResult GetBackLogsFiltros(BackLogsDTO objBackLog, bool esObra)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (!string.IsNullOrEmpty(objBackLog.areaCuenta))
                {
                    BackLogsFactoryServices.VerificarEntradaAlmacenOC();
                    BackLogsFactoryServices.VerificarTraspasosBL(objBackLog.areaCuenta);
                }

                var lstBackLogs = BackLogsFactoryServices.GetBackLogsFiltros(objBackLog, esObra);
                resultado.Add("lstBackLogs", lstBackLogs);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetTotalOCRehabilitacion(GetTotalMXDTO objTotalDTO)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                decimal totalMX = BackLogsFactoryServices.GetTotalOCRehabilitacion(objTotalDTO);
                resultado.Add("totalMX", totalMX);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumos(InsumosDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstInsumos = BackLogsFactoryServices.GetInsumos(objFiltro); // TO DO
                resultado.Add("lstInsumos", lstInsumos);
                resultado.Add(SUCCESS, true);

                var json = Json(resultado, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDatosGraficasBLObra(BackLogsDTO objBL)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                if (objBL != null)
                {
                    var resultadoGraficas = BackLogsFactoryServices.GetDatosGraficasBLObra(objBL);
                    if (resultadoGraficas != null)
                    {
                        resultados.Add(SUCCESS, true);
                        resultados.Add("resultadosCantEstatus", resultadoGraficas["resultadosCantEstatus"]);
                        resultados.Add("resultadosCantEstatusLineas", resultadoGraficas["resultadosCantEstatusLineas"]);
                        resultados.Add("resultadosAñosAnteriores", resultadoGraficas["resultadosAñosAnteriores"]);
                        resultados.Add("resultadosTiempoPromedio", resultadoGraficas["resultadosTiempoPromedio"]);
                    }
                    else
                        resultadoGraficas.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return Json(resultados, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNumBackLogs(string areaCuenta)
        {
            List<int> lstBackLogs = new List<int>();
            var resultado = new Dictionary<string, object>();

            int estatus20 = 0, estatus40 = 0, estatus50 = 0, estatus60 = 0,
                estatus80 = 0, estatus90 = 0, estatus100 = 0, TotalBackLogs = 0;

            int eneroBLRegistrado = 0, febreroBLRegistrado = 0, marzoBLRegistrado = 0, abrilBLRegistrado = 0,
                mayoBLRegistrado = 0, junioBLRegistrado = 0, julioBLRegistrado = 0, agostoBLRegistrado = 0,
                septiembreBLRegistrado = 0, octubreBLRegistrado = 0, noviembreBLRegistrado = 0, diciembreBLRegistrado = 0;

            int eneroBLCerrado = 0, febreroBLCerrado = 0, marzoBLCerrado = 0, abrilBLCerrado = 0,
                mayoBLCerrado = 0, junioBLCerrado = 0, julioBLCerrado = 0, agostoBLCerrado = 0,
                septiembreBLCerrado = 0, octubreBLCerrado = 0, noviembreBLCerrado = 0, diciembreBLCerrado = 0;

            try
            {
                var BackLogs = BackLogsFactoryServices.GetNumBackLogs(areaCuenta);
                var selBackLogs = BackLogs.Select(x => new
                {
                    fechaInspeccion = x.fechaInspeccion.Month,
                    estatus = x.idEstatus
                }).ToList();

                foreach (var item in selBackLogs)
                {
                    switch (item.estatus)
                    {
                        case 1:
                            estatus20++;
                            break;
                        case 2:
                            estatus40++;
                            break;
                        case 3:
                            estatus50++;
                            break;
                        case 4:
                            estatus60++;
                            break;
                        case 5:
                            estatus80++;
                            break;
                        case 6:
                            estatus90++;
                            break;
                        case 7:
                            estatus100++;
                            break;
                    }

                    switch (item.fechaInspeccion)
                    {
                        case 1:
                            eneroBLRegistrado++;
                            if (item.estatus == 7)
                                eneroBLCerrado++;
                            break;
                        case 2:
                            febreroBLRegistrado++;
                            if (item.estatus == 7)
                                febreroBLCerrado++;
                            break;
                        case 3:
                            marzoBLRegistrado++;
                            if (item.estatus == 7)
                                marzoBLCerrado++;
                            break;
                        case 4:
                            abrilBLRegistrado++;
                            if (item.estatus == 7)
                                abrilBLCerrado++;
                            break;
                        case 5:
                            mayoBLRegistrado++;
                            if (item.estatus == 7)
                                mayoBLCerrado++;
                            break;
                        case 6:
                            junioBLRegistrado++;
                            if (item.estatus == 7)
                                junioBLCerrado++;
                            break;
                        case 7:
                            julioBLRegistrado++;
                            if (item.estatus == 7)
                                julioBLCerrado++;
                            break;
                        case 8:
                            agostoBLRegistrado++;
                            if (item.estatus == 7)
                                agostoBLCerrado++;
                            break;
                        case 9:
                            septiembreBLRegistrado++;
                            if (item.estatus == 7)
                                septiembreBLCerrado++;
                            break;
                        case 10:
                            octubreBLRegistrado++;
                            if (item.estatus == 7)
                                octubreBLCerrado++;
                            break;
                        case 11:
                            noviembreBLRegistrado++;
                            if (item.estatus == 7)
                                noviembreBLCerrado++;
                            break;
                        case 12:
                            diciembreBLRegistrado++;
                            if (item.estatus == 7)
                                diciembreBLCerrado++;
                            break;
                    }
                }
                TotalBackLogs = selBackLogs.Count();

                lstBackLogs.Add(estatus20);
                lstBackLogs.Add(estatus40);
                lstBackLogs.Add(estatus50);
                lstBackLogs.Add(estatus60);
                lstBackLogs.Add(estatus80);
                lstBackLogs.Add(estatus90);
                lstBackLogs.Add(estatus100);
                lstBackLogs.Add(TotalBackLogs); //7

                lstBackLogs.Add(eneroBLRegistrado);
                lstBackLogs.Add(febreroBLRegistrado);
                lstBackLogs.Add(marzoBLRegistrado);
                lstBackLogs.Add(abrilBLRegistrado);
                lstBackLogs.Add(mayoBLRegistrado);
                lstBackLogs.Add(junioBLRegistrado);
                lstBackLogs.Add(julioBLRegistrado);
                lstBackLogs.Add(agostoBLRegistrado);
                lstBackLogs.Add(septiembreBLRegistrado);
                lstBackLogs.Add(octubreBLRegistrado);
                lstBackLogs.Add(noviembreBLRegistrado);
                lstBackLogs.Add(diciembreBLRegistrado); //19

                lstBackLogs.Add(eneroBLCerrado);
                lstBackLogs.Add(febreroBLCerrado);
                lstBackLogs.Add(marzoBLCerrado);
                lstBackLogs.Add(abrilBLCerrado);
                lstBackLogs.Add(mayoBLCerrado);
                lstBackLogs.Add(junioBLCerrado);
                lstBackLogs.Add(julioBLCerrado);
                lstBackLogs.Add(agostoBLCerrado);
                lstBackLogs.Add(septiembreBLCerrado);
                lstBackLogs.Add(octubreBLCerrado);
                lstBackLogs.Add(noviembreBLCerrado);
                lstBackLogs.Add(diciembreBLCerrado); //31

                resultado.Add("lstBackLogs", lstBackLogs);
                resultado.Add(SUCCESS, true);
                GetNumBacklogsAniosAnteriores(areaCuenta);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNumBacklogsAniosAnteriores(string areaCuenta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var BackLogsAniosAnteriores = BackLogsFactoryServices.GetNumBacklogsAniosAnteriores(areaCuenta);

                resultado.Add("BackLogsAniosAnteriores", BackLogsAniosAnteriores);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCostoPromedio(int almacen, int insumo)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                decimal costoPromedio = BackLogsFactoryServices.GetCostoPromedio(almacen, insumo);
                resultado.Add("costoPromedio", costoPromedio);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RetornarAlmacen(string areaCuenta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                int costoPromedio = BackLogsFactoryServices.RetornarAlmacen(areaCuenta);
                resultado.Add(ITEMS, costoPromedio);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarBackLog(tblBL_CatBackLogs objBL, List<tblBL_Partes> datosPartes, List<tblBL_ManoObra> datosManoObra, bool esParte, bool esManoObra, bool esActualizarCC, bool esObra, int idUsuarioResponsable)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objBL != null)
                {
                    FolioBLDisponible = false;
                    while (!FolioBLDisponible)
                    {
                        GenerarFormatoFolio(esObra);
                    }

                    objBL.folioBL = folioBL;
                    ValidarBanderaCrearBackLog(objBL, esObra);
                    if (BanderaCrearEditarBackLog)
                    {
                        if (objBL.id > 0)
                        {
                            bool ActualizarBackLog = BackLogsFactoryServices.ActualizarBackLog(objBL, esActualizarCC);
                            if (!ActualizarBackLog)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al actualizar el BackLog.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                                resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            var CrearBackLog = BackLogsFactoryServices.CrearBackLog(objBL, datosPartes, datosManoObra, esParte, esManoObra, esObra, idUsuarioResponsable);
                            if (!CrearBackLog)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al crear el BackLog.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                            }
                        }
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al crear el BackLog.");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillAreasCuentas()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboProyectos = BackLogsFactoryServices.FillAreasCuentas();
                resultado.Add(ITEMS, cboProyectos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCC(string areaCuenta, bool esObra)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboCC = BackLogsFactoryServices.FillCboCC(areaCuenta, esObra);
                resultado.Add(ITEMS, cboCC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboConjunto()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboConjunto = BackLogsFactoryServices.FillCboConjunto();
                resultado.Add(ITEMS, cboConjunto);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSubconjunto(int idConjunto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboSubconjunto = BackLogsFactoryServices.FillCboSubconjunto(idConjunto);
                resultado.Add(ITEMS, cboSubconjunto);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModelo()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboModelo = BackLogsFactoryServices.FillCboModelo();
                resultado.Add(ITEMS, cboModelo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupo(BackLogsDTO objParamsDTO)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboModelo = BackLogsFactoryServices.FillCboGrupo(objParamsDTO);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, cboModelo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMaquina(string areaCuenta, string noEconomico)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var Maquina = BackLogsFactoryServices.GetMaquina(areaCuenta, noEconomico);
                var lstMaquina = Maquina.Select(x => new
                {
                    modeloEquipoID = x.modeloEquipoID,
                    grupoMaquinariaID = x.grupoMaquinariaID,
                    modeloEquipo = x.modeloEquipo.descripcion,
                    grupoMaquinaria = x.grupoMaquinaria.descripcion
                }).ToList();

                string strMensajeError = string.Empty;
                if (lstMaquina.Count() > 0)
                {
                    string modelo = !string.IsNullOrEmpty(lstMaquina[0].modeloEquipo) ? lstMaquina[0].modeloEquipo : string.Empty;
                    string grupoMaquinaria = !string.IsNullOrEmpty(lstMaquina[0].grupoMaquinaria) ? lstMaquina[0].grupoMaquinaria : string.Empty;

                    if (string.IsNullOrEmpty(modelo))
                        strMensajeError += "No se encuentra el modelo del CC seleccionado.";
                    if (string.IsNullOrEmpty(grupoMaquinaria))
                        strMensajeError += "No se encuentra el grupo del CC seleccionado.";

                    if (!string.IsNullOrEmpty(strMensajeError))
                    {
                        resultado.Add(MESSAGE, strMensajeError);
                        resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        resultado.Add("lstMaquina", lstMaquina);
                        resultado.Add(SUCCESS, true);
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encuentra información sobre el CC seleccionado.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHorometroActual(string areaCuenta, string noEconomico)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                tblM_CapHorometro objHorometro = BackLogsFactoryServices.GetHorometroActual(areaCuenta, noEconomico);
                if (objHorometro.Horometro > -1)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("horometroActual", objHorometro.Horometro);
                }
                else
                    throw new Exception("Ocurrió un error al obtener el horometro del No. Económico: " + noEconomico);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCantBLObligatorios(string areaCuenta, string noEconomico)
        {
            #region COMENTADO
            //var resultado = new Dictionary<string, object>();
            //try
            //{
            //    int intCantBLPorRegistrar = BackLogsFactoryServices.GetCantBLObligatorios(areaCuenta, noEconomico);
            //    if (intCantBLPorRegistrar > 0)
            //    {
            //        resultado.Add(SUCCESS, true);
            //        resultado.Add("cantBL", intCantBLPorRegistrar);
            //    }
            //    else
            //    {
            //        resultado.Add(SUCCESS, false);
            //        resultado.Add(MESSAGE, "No hay BackLogs pendientes por registrar del económico seleccionado.");
            //    }
            //}
            //catch (Exception e)
            //{
            //    resultado.Add(MESSAGE, e.Message);
            //    resultado.Add(SUCCESS, false);
            //}
            //return Json(resultado, JsonRequestBehavior.AllowGet);
            #endregion

            return Json(BackLogsFactoryServices.GetCantBLObligatorios(areaCuenta, noEconomico), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUltimoFolio(bool esObra)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                while (!FolioBLDisponible)
                {
                    GenerarFormatoFolio(esObra);
                }

                resultado.Add("folio", folioBL);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarBackLog(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    var EliminarBackLog = BackLogsFactoryServices.EliminarBackLog(id);

                    if (!EliminarBackLog)
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al eliminar el BackLog.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPartes(int idBackLog)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                int i = 1;
                var Partes = BackLogsFactoryServices.GetPartes(idBackLog);
                var lstPartes = Partes.Select(x => new
                {
                    id = x.id,
                    partida = i++,
                    insumo = x.insumo,
                    cantidad = x.cantidad,
                    parte = x.parte,
                    articulo = x.articulo,
                    unidad = x.unidad,
                    tipoMoneda = x.unidad,
                    costoPromedio = x.costoPromedio
                }).ToList();

                resultado.Add("lstPartes", lstPartes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarParte(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    var EliminarParte = BackLogsFactoryServices.EliminarParte(id);

                    if (!EliminarParte)
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al eliminar el insumo.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarParte(tblBL_Partes datosParte)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (datosParte != null)
                {
                    ValidarBanderaPartes(datosParte);
                    if (BanderaCrearEditarParte)
                    {
                        if (datosParte.id > 0)
                        {
                            var ActualizarParte = BackLogsFactoryServices.ActualizarParte(datosParte);
                            if (!ActualizarParte)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al actualizar el insumo.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                            }
                        }
                        else
                        {
                            var CrearParte = BackLogsFactoryServices.CrearParte(datosParte);
                            if (!CrearParte)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al crear/editar el insumo.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                            }
                        }
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al crear/editar el insumo.");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al crear el BackLog.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarManoObra(tblBL_ManoObra datosManoObra)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (datosManoObra != null)
                {
                    ValidarBanderaManoObra(datosManoObra);
                    if (BanderaCrearEditarManoObra)
                    {
                        if (datosManoObra.id > 0)
                        {
                            var ActualizarManoObra = BackLogsFactoryServices.ActualizarManoObra(datosManoObra);
                            if (!ActualizarManoObra)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al actualizar la mano de obra.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                            }
                        }
                        else
                        {
                            var CrearManoObra = BackLogsFactoryServices.CrearManoObra(datosManoObra);
                            if (!CrearManoObra)
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al crear la mano de obra.");
                                resultado.Add(SUCCESS, false);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                            }
                        }
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al crear/editar la mano de obra.");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al crear/actualizar la mano de obra.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetManoObra(int idBackLog)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                int i = 1;
                var ManoObra = BackLogsFactoryServices.GetManoObra(idBackLog);
                var lstManoObra = ManoObra.Select(x => new
                {
                    id = x.id,
                    partida = i++,
                    descripcion = x.descripcion
                }).ToList();

                resultado.Add("lstManoObra", lstManoObra);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarManoObra(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    var EliminarManoObra = BackLogsFactoryServices.EliminarManoObra(id);

                    if (!EliminarManoObra)
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al eliminar la mano de obra.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                        resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private void GenerarFormatoFolio(bool esObra)
        {
            var BackLogs = BackLogsFactoryServices.GetUltimoFolio(esObra);
            var lstFolios = BackLogs.Select(x => new
            {
                folio = x.folioBL
            }).ToList();

            if (lstFolios.Count > 0)
            {
                folioBL = lstFolios[0].ToString();
                string rplFolio = folioBL.Replace("}", "");
                string strFolio = string.Empty;
                if (esObra)
                {
                    strFolio = rplFolio.Substring(13).Trim();
                }
                else
                {
                    strFolio = rplFolio.Substring(14).Trim();
                }
                int incrementarFolio = Convert.ToInt32(strFolio);
                incrementarFolio++;

                string folioNumbers = incrementarFolio.ToString();
                string numFolioEntero = incrementarFolio.ToString();
                int lenFolio = 5 - numFolioEntero.Length;
                string FormatoFolio = string.Empty;

                switch (lenFolio)
                {
                    case 4:
                        FormatoFolio = "0000" + numFolioEntero;
                        break;
                    case 3:
                        FormatoFolio = "000" + numFolioEntero;
                        break;
                    case 2:
                        FormatoFolio = "00" + numFolioEntero;
                        break;
                    case 1:
                        FormatoFolio = "0" + numFolioEntero;
                        break;
                    case 0:
                        FormatoFolio = numFolioEntero;
                        break;
                    default:
                        FormatoFolio = numFolioEntero;
                        break;
                }

                if (esObra)
                    folioBL = "AB-" + FormatoFolio;
                else
                    folioBL = "TMC-" + FormatoFolio;
            }
            else
            {
                if (esObra)
                    folioBL = "AB-00001";
                else
                    folioBL = "TMC-00001";
            }

            var VerificarDisponibilidadFolio = BackLogsFactoryServices.VerificarDisponibilidadFolio(folioBL);
            if (VerificarDisponibilidadFolio.Count <= 0)
            {
                FolioBLDisponible = true;
            }
        }

        private void ValidarBanderaCrearBackLog(tblBL_CatBackLogs datosFormulario, bool esObra)
        {
            if (!string.IsNullOrEmpty(datosFormulario.areaCuenta) && (datosFormulario.id <= 0 ? !string.IsNullOrEmpty(datosFormulario.noEconomico) : true) && datosFormulario.fechaInspeccion != null &&
                !string.IsNullOrEmpty(datosFormulario.descripcion) && datosFormulario.idSubconjunto > 0 && !string.IsNullOrEmpty(datosFormulario.folioBL) && datosFormulario.horas > -1)
            {
                BanderaCrearEditarBackLog = true;
            }
            else
            {
                if (string.IsNullOrEmpty(datosFormulario.folioBL))
                {
                    GetUltimoFolio(esObra);
                    datosFormulario.folioBL = folioBL;
                    BanderaCrearEditarBackLog = true;
                }
                else
                    BanderaCrearEditarBackLog = false;
            }
        }

        private void ValidarBanderaPartes(tblBL_Partes datosParte)
        {
            if (datosParte.cantidad > 0 && !string.IsNullOrEmpty(datosParte.parte) && !string.IsNullOrEmpty(datosParte.articulo) && datosParte.insumo > 0)
                BanderaCrearEditarParte = true;
        }

        private void ValidarBanderaManoObra(tblBL_ManoObra datosManoObra)
        {
            if (datosManoObra.descripcion != string.Empty)
                BanderaCrearEditarManoObra = true;
        }

        public ActionResult ConfirmarRehabilitacionProgramada(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");

                bool esActualizarEstatusBL = BackLogsFactoryServices.ConfirmarRehabilitacionProgramada(idBL);
                if (esActualizarEstatusBL)
                    resultado.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmarProcesoInstalacion(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");

                bool esActualizarEstatusBL = BackLogsFactoryServices.ConfirmarProcesoInstalacion(idBL);
                if (esActualizarEstatusBL)
                    resultado.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmarBackLogInstalado(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");

                bool ExisteEvidenciaLiberacion = BackLogsFactoryServices.ExisteEvidenciaLiberacion(idBL);
                if (ExisteEvidenciaLiberacion)
                {
                    bool esActualizarBL = BackLogsFactoryServices.ConfirmarBackLogInstalado(idBL);
                    if (esActualizarBL)
                        resultado.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                }
                else
                    throw new Exception("El BackLog no cuenta con evidencia de liberación.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarRequisicion(tblBL_MotivoCancelacionReq objMotivo)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objMotivo.idBL <= 0)
                    throw new Exception("Ocurrió un error al registrar el motivo de la cancelación de la requisición.");

                bool esCancelarRequisicion = BackLogsFactoryServices.CancelarRequisicion(objMotivo);
                if (esCancelarRequisicion)
                    resultado.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al registrar el motivo de la cancelación de la requisición.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIDOT(int idBL)
        {
            return Json(BackLogsFactoryServices.GetIDOT(idBL), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoBL(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                    throw new Exception("Ocurrió un error al obtener el tipo del BackLog.");

                bool esObra = BackLogsFactoryServices.GetTipoBL(idBL);
                resultado.Add("esObra", esObra);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MostrarEvidencia(int idEvidencia)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            resultado = BackLogsFactoryServices.MostrarEvidencia(idEvidencia);
            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;
                var fileData = Tuple.Create(bytesArchivo, extension);
                Session["archivoVisor"] = fileData;
            }
            else
                Session["archivoVisor"] = null;

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFoliosTraspasos(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                    throw new Exception("Ocurrió un error al obtener el listado de folios.");

                List<FoliosTraspasosDTO> lstFoliosTraspasos = BackLogsFactoryServices.GetFoliosTraspasos(idBL);
                resultado.Add("lstFoliosTraspasos", lstFoliosTraspasos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarTraspasoFolio(FoliosTraspasosDTO objCE)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                if (objCE.almacenID <= 0 || objCE.folioTraspaso <= 0)
                {
                    resultado.Add(MESSAGE, "Es necesario llenar todos los campos.");
                    resultado.Add(SUCCESS, false);
                }
                else
                {
                    bool esCrearEditar = BackLogsFactoryServices.CrearEditarTraspasoFolio(objCE);
                    if (!esCrearEditar)
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al registrar el folio del traspaso.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                        resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarTraspasosBL(string areaCuenta)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                bool esExito = BackLogsFactoryServices.VerificarTraspasosBL(areaCuenta);
                if (!esExito)
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al verificar los Folios.");
                    resultado.Add(SUCCESS, false);
                }
                else
                    resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFolioTraspaso(int idFolioTraspaso)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                if (idFolioTraspaso <= 0)
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al eliminar el registro.");
                    resultado.Add(SUCCESS, false);
                }

                bool esEliminar = BackLogsFactoryServices.EliminarFolioTraspaso(idFolioTraspaso);
                if (!esEliminar)
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al eliminar el registro.");
                    resultado.Add(SUCCESS, false);
                }
                else
                    resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambiarEstatusBL90a80(int idBL)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                if (idBL <= 0)
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al actualizar el BL.");
                    resultado.Add(SUCCESS, false);
                }

                bool esActualizar = BackLogsFactoryServices.CambiarEstatusBL90a80(idBL);
                if (!esActualizar)
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al actualizar el BL.");
                    resultado.Add(SUCCESS, false);
                }
                else
                    resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarEvidencia(int idEvidencia)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            resultado = BackLogsFactoryServices.VisualizarEvidencia(idEvidencia);

            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;

                var fileData = Tuple.Create(bytesArchivo, extension);

                Session["archivoVisor"] = fileData;
            }
            else
            {
                Session["archivoVisor"] = null;
            }

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO CONJUNTOS
        public ActionResult GetConjuntos()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<CatConjuntosDTO> lstConjuntos = BackLogsFactoryServices.GetConjuntos();
                resultado.Add("lstConjuntos", lstConjuntos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarConjunto(tblBL_CatConjuntos objConjunto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objConjunto != null)
                {
                    if (objConjunto.id > 0)
                    {
                        bool esEditar = BackLogsFactoryServices.ActualizarConjunto(objConjunto);
                        if (esEditar)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        bool esCrear = BackLogsFactoryServices.CrearConjunto(objConjunto);
                        if (esCrear)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarConjunto(int idConjunto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idConjunto > 0)
                {
                    bool EliminarConjunto = BackLogsFactoryServices.EliminarConjunto(idConjunto);
                    if (EliminarConjunto)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO SUBCONJUNTOS
        public ActionResult GetSubconjuntos()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstCatSubconjuntos = BackLogsFactoryServices.GetSubconjuntos();
                resultado.Add("lstCatSubconjuntos", lstCatSubconjuntos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearEditarSubconjunto(tblBL_CatSubconjuntos objSubconjunto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objSubconjunto != null)
                {
                    if (objSubconjunto.id > 0)
                    {
                        bool esEditar = BackLogsFactoryServices.ActualizarSubconjunto(objSubconjunto);
                        if (esEditar)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        bool esCrear = BackLogsFactoryServices.CrearSubconjunto(objSubconjunto);
                        if (esCrear)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarSubconjunto(int idSubconjunto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idSubconjunto > 0)
                {
                    bool EliminarSubconjunto = BackLogsFactoryServices.EliminarSubconjunto(idSubconjunto);
                    if (EliminarSubconjunto)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REQUISICIONES
        public ActionResult GetRequisiciones(RequisicionesDTO objRequisiciones)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstRequisiciones = BackLogsFactoryServices.GetRequisiciones(objRequisiciones);
                resultado.Add("lstRequisiciones", lstRequisiciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRequisicion(tblBL_Requisiciones objRequisicion)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objRequisicion != null && objRequisicion.idBackLog > 0)
                {
                    //SE VERIFICA QUE EL NÚMERO DE REQUISICION EXISTA
                    bool existeNumRequisicion = BackLogsFactoryServices.ValidaNumeroRequisicion(objRequisicion);
                    if (existeNumRequisicion)
                    {
                        if (objRequisicion.id > 0)
                        {
                            bool ActualizarRequisicion = BackLogsFactoryServices.ActualizarRequisicion(objRequisicion);
                            if (ActualizarRequisicion)
                                resultado.Add(SUCCESS, true);
                            else
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al intentar actualizar el número de requisición.");
                                resultado.Add(SUCCESS, false);
                            }
                        }
                        else
                        {
                            bool CrearRequisicion = BackLogsFactoryServices.CrearRequisicion(objRequisicion);
                            if (CrearRequisicion)
                                resultado.Add(SUCCESS, true);
                            else
                            {
                                resultado.Add(MESSAGE, "Ocurrió un error al intentar registar el número de requisición.");
                                resultado.Add(SUCCESS, false);
                            }
                        }
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "No se encuentra el número de requisición ingresado.");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRequisicion(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    bool EliminarRequisicion = BackLogsFactoryServices.EliminarRequisicion(id);
                    if (EliminarRequisicion)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllRequisiciones(RequisicionesDTO objReq)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objReq.idBackLog > 0)
                {
                    var lstRequisicionesEK = BackLogsFactoryServices.GetAllRequisiciones(objReq);
                    resultado.Add("lstRequisicionesEK", lstRequisicionesEK);
                    resultado.Add(SUCCESS, true);
                }
                else
                    throw new Exception("Ocurrió un error al intentar obtener las requisiciones el CC.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllDetRequisiciones(RequisicionesDTO objReq)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objReq.idBackLog > 0 && !string.IsNullOrEmpty(objReq.numero))
                {
                    var lstDetRequisicionesEK = BackLogsFactoryServices.GetAllDetRequisiciones(objReq);
                    resultado.Add("lstDetRequisicionesEK", lstDetRequisicionesEK);
                    resultado.Add(SUCCESS, true);
                }
                else
                    throw new Exception("Ocurrió un error al intentar consultar las partidas de la requisición.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosBL(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL > 0)
                {
                    BackLogsDTO objBL = BackLogsFactoryServices.GetDatosBL(idBL);
                    resultado.Add("objBL", objBL);
                    resultado.Add(SUCCESS, true);
                }
                else
                    throw new Exception("Ocurrió un error al obtener la información del BackLog.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMotivosCancelacion(int idBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idBL > 0)
                {
                    List<MotivoCancelacionReq> lstMotivoCancelacionReq = BackLogsFactoryServices.GetMotivosCancelacion(idBL);
                    resultado.Add("lstMotivoCancelacionReq", lstMotivoCancelacionReq);
                    resultado.Add(SUCCESS, true);
                }
                else
                    throw new Exception("Ocurrió un error al obtener la información");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ORDENES DE COMPRA
        public ActionResult GetOrdenesCompra(OrdenCompraDTO objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstOC = BackLogsFactoryServices.GetOrdenesCompra(objOC);
                resultado.Add("lstOC", lstOC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearOC(List<tblBL_OrdenesCompra> objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objOC != null)
                {
                    bool CrearOC = BackLogsFactoryServices.CrearOC(objOC);
                    if (CrearOC)
                    {
                        resultado.Add(SUCCESS, true);
                        BackLogsFactoryServices.VerificarEntradaAlmacenOC();
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al intentar registar el número de Orden de Compra.");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarOC(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    bool EliminarOC = BackLogsFactoryServices.EliminarOC(id);
                    if (EliminarOC)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllOrdenesCompra(OrdenCompraDTO objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstOC = BackLogsFactoryServices.GetAllOrdenesCompra(objOC);
                resultado.Add("lstOC", lstOC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstOcReq(OrdenCompraDTO objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstOCEK = BackLogsFactoryServices.GetLstOcReq(objOC);
                resultado.Add("lstOCEK", lstOCEK);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLstDetOcReq(OrdenCompraDTO objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstDetOCEK = BackLogsFactoryServices.GetLstDetOcReq(objOC);
                resultado.Add("lstDetOCEK", lstDetOCEK);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarOC(OrdenCompraDTO objOC)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool existeOC = BackLogsFactoryServices.VerificarDuplicadoOC(objOC);
                if (!existeOC)
                {
                    bool crearOC = BackLogsFactoryServices.GuardarOC(objOC);
                    if (crearOC)
                        resultado.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al guardar la OC en el BackLog.");
                }
                else
                    throw new Exception("La OC seleccionada, ya se encuentra registrada en este BackLog.");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PROGRAMA DE INSPECCIÓN
        public ActionResult GetInspecciones(InspeccionesDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstInspecciones = BackLogsFactoryServices.GetInspecciones(objFiltro);
                resultado.Add("lstInspecciones", lstInspecciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarInspeccionObra(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool EliminarInspeccion = BackLogsFactoryServices.EliminarInspeccionObra(id);
                if (EliminarInspeccion)
                    resultado.Add(SUCCESS, true);
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al eliminar la inspección.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboNoEconomico(string areaCuenta, List<int> lstGrupos)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (lstGrupos.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener el listado de No. Economicos.");

                var cboNoEconomico = BackLogsFactoryServices.FillCboNoEconomico(areaCuenta, lstGrupos);
                resultado.Add(ITEMS, cboNoEconomico);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillPeriodos(int anio)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstPeriodos = BackLogsFactoryServices.FillPeriodos(anio);
                resultado.Add(ITEMS, lstPeriodos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriodoActual()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var periodoActual = BackLogsFactoryServices.GetPeriodoActual();
                resultado.Add("periodoActual", periodoActual);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarInspecciones(List<tblBL_Inspecciones> objInspecciones)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objInspecciones != null)
                {
                    bool GuardarInspeccion = BackLogsFactoryServices.GuardarInspecciones(objInspecciones);
                    if (GuardarInspeccion)
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al intentar guardar las inspecciones.");
                    }
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarInspeccion(tblBL_Inspecciones objInspecciones)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objInspecciones != null)
                {
                    if (objInspecciones.id == 0)
                        throw new Exception("Ocurrió un error al intentar actualizar la inspección.");

                    bool ActualizarInspeccion = BackLogsFactoryServices.ActualizarInspeccion(objInspecciones);
                    if (ActualizarInspeccion)
                        resultado.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al intentar actualizar la inspección.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult postObtenerTablaInspecciones(InspeccionesDTO parametros)
        {
            try
            {
                resultado.Add(ITEMS, BackLogsFactoryServices.postObtenerTablaInspecciones(parametros, true));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                throw;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetVariablesSesion(BackLogsDTO objParamsDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(objParamsDTO.noEconomico)) { throw new Exception("Es necesario seleccionar un CC."); }
                if (objParamsDTO.anio <= 0) { throw new Exception("Es necesario seleccionar un año."); }
                if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta."); }

                Session["noEconomico"] = objParamsDTO.noEconomico;
                Session["areaCuenta"] = objParamsDTO.areaCuenta;
                Session["anio"] = objParamsDTO.anio;

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream GenerarExcelInspeccionesObras()
        {
            BackLogsDTO objParamsDTO = new BackLogsDTO();
            objParamsDTO.noEconomico = Session["noEconomico"].ToString();
            objParamsDTO.areaCuenta = Session["areaCuenta"].ToString();
            objParamsDTO.anio = (int)Session["anio"];
            var stream = BackLogsFactoryServices.GenerarExcelInspeccionesObras(objParamsDTO);
            if (stream != null)
            {
                this.Response.Clear();
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Inspecciones.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["parametros"] = null;
                return stream;
            }
            else
                return null;
        }

        public MemoryStream crearExcelInspeccionesTMC()
        {
            string AreaCuenta = (string)Session["AreaCuenta"];
            List<string> lstNoEconomicos = new List<string>();
            string[] NoEconomicosSplit = AreaCuenta.Split(',');
            MemoryStream stream = new System.IO.MemoryStream();
            foreach (var item in NoEconomicosSplit)
            {
                lstNoEconomicos.Add(item);
                stream = BackLogsFactoryServices.crearExcelInspeccionesTMC(item);

                if (stream != null)
                {
                    this.Response.Clear();
                    //this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Inspecciones.xlsx"));
                    this.Response.BinaryWrite(stream.ToArray());
                    this.Response.End();

                    Session["parametros"] = null;
                }
            }
            return stream;
        }
        #endregion

        #region REPORTE E INDICADORES
        public ActionResult FillCboResponsables()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboResponsables = BackLogsFactoryServices.FillCboResponsables();
                resultado.Add(ITEMS, cboResponsables);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGraficaBLDias(BackLogsDTO objParamsDTO)
        {
            return Json(BackLogsFactoryServices.GetGraficaBLDias(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGraficaConjuntos(BackLogsDTO objParamsDTO)
        {
            return Json(BackLogsFactoryServices.GetGraficaConjuntos(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPorEquipoDet(BackLogsDTO objFiltro)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstPorEquipoDet = BackLogsFactoryServices.GetPorEquipoDet(objFiltro);
                resultado.Add("lstPorEquipoDet", lstPorEquipoDet);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboResponsablesAnalisisBLResponsable(string areaCuenta)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> cboResponsables = BackLogsFactoryServices.FillCboResponsablesAnalisisBLResponsable(areaCuenta);
                resultado.Add(ITEMS, cboResponsables);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLOTVacia(int idBL)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                BackLogsDTO objBL = BackLogsFactoryServices.GetBLOTVacia(idBL);
                if (objBL != null)
                {
                    resultado.Add("objBL", objBL);
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalInfoEconomicoBL(BackLogsDTO objParamsDTO)
        {
            return Json(BackLogsFactoryServices.GetTotalInfoEconomicoBL(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region BACKLOGS TMC
        public ActionResult IndexTMC()
        {
            BackLogsFactoryServices.VerificarEntradaAlmacenOC();
            return View();
        }

        public ActionResult PresupuestoRehabilitacionTMC()
        {
            return View();
        }

        public ActionResult InformeTMC()
        {
            return View();
        }

        public ActionResult FrenteTMC()
        {
            return View();
        }

        public ActionResult ProgramaInspTMC()
        {
            return View();
        }

        public ActionResult SeguimientoDePresupuestoTMC()
        {
            return View();
        }

        public ActionResult FillAreasCuentasTMC()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboProyectos = BackLogsFactoryServices.FillAreasCuentasTMC();
                resultado.Add(ITEMS, cboProyectos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillTipoMaquinariaTMC()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboTipoMaquinariaTMC = BackLogsFactoryServices.FillTipoMaquinariaTMC();
                resultado.Add(ITEMS, cboTipoMaquinariaTMC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatosGraficasBLTMC(BackLogsDTO objBL)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                if (objBL != null)
                {
                    var resultadoGraficas = BackLogsFactoryServices.GetDatosGraficasBLTMC(objBL);
                    if (resultadoGraficas != null)
                    {
                        resultados.Add(SUCCESS, true);
                        resultados.Add("resultadosCantEstatusTMC", resultadoGraficas["resultadosCantEstatusTMC"]);
                        resultados.Add("resultadosCantEstatusLineasTMC", resultadoGraficas["resultadosCantEstatusLineasTMC"]);
                        resultados.Add("resultadosAñosAnterioresTMC", resultadoGraficas["resultadosAñosAnterioresTMC"]);
                        resultados.Add("resultadosTiempoPromedio", resultadoGraficas["resultadosTiempoPromedio"]);
                    }
                    else
                        resultadoGraficas.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return Json(resultados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBackLogsFiltrosTMC(BackLogsDTO objBackLog)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstBackLogs = BackLogsFactoryServices.GetBackLogsFiltrosTMC(objBackLog);
                resultado.Add("lstBackLogs", lstBackLogs);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModeloTMC()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboModelo = BackLogsFactoryServices.FillCboModeloTMC();
                resultado.Add(ITEMS, cboModelo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupoTMC()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboModelo = BackLogsFactoryServices.FillCboGrupoTMC();
                resultado.Add(ITEMS, cboModelo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgramacionInspeccionTMC(ProgramaInspTMCDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstProgramacionInspTMC = BackLogsFactoryServices.GetProgramacionInspeccionTMC(objFiltro);
                resultado.Add("lstProgramacionInspTMC", lstProgramacionInspTMC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarProgramaInspTMC(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool esEliminar = BackLogsFactoryServices.EliminarProgramaInspTMC(id);
                if (esEliminar)
                    resultado.Add(SUCCESS, true);
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al eliminar la inspección.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarProgramacionTMC(List<tblBL_InspeccionesTMC> objInsp)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objInsp != null)
                {
                    bool CrearProgramacion = BackLogsFactoryServices.CrearProgramacionTMC(objInsp);
                    if (!CrearProgramacion)
                    {
                        resultado.Add(MESSAGE, "Ocurrió un error al crear la programación.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                    }
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProgramaInspeccionTMC(ProgramaInspTMCDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstProgramaInspTMC = BackLogsFactoryServices.GetProgramaInspeccionTMC(objFiltro);
                resultado.Add("lstProgramaInspTMC", lstProgramaInspTMC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarProgramaInspeccionTMC(tblBL_InspeccionesTMC objInsp)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objInsp != null && objInsp.id > 0)
                {
                    bool esActualizar = BackLogsFactoryServices.ActualizarProgramaInspeccionTMC(objInsp);
                    if (esActualizar)
                        resultado.Add(SUCCESS, true);
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al actualizar el registro.");
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al actualizar el registro.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillTablaSolicitudPpto(BackLogsDTO objBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstSolicitudPpto = BackLogsFactoryServices.FillTablaSolicitudPpto(objBL);
                resultado.Add("lstSolicitudPpto", lstSolicitudPpto);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolicitarAutorizacion(int idInsp, SeguimientoPptoDTO objSegPpto, List<string> lstFoliosBL)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool esSolicitarAutorizacion = BackLogsFactoryServices.SolicitarAutorizacion(idInsp, getUsuario().id, objSegPpto, lstFoliosBL);
                if (esSolicitarAutorizacion)
                    resultado.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al solicitar la autorización");
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBLPptos(int idSegPpto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<BackLogsDTO> lstBL = BackLogsFactoryServices.GetBLPptos(idSegPpto);
                resultado.Add("lstBL", lstBL);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModificarEstatus(int id, int Autorizante, int Estatus, string Descripcion)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (getUsuario().id == (int)AutorizadoresEnum.vobo1 || getUsuario().id == (int)AutorizadoresEnum.vobo2 || getUsuario().id == (int)AutorizadoresEnum.Autorizado)
                {
                    var cboFrentes = BackLogsFactoryServices.ModificarEstatus(id, getUsuario().id, Autorizante, Estatus, Descripcion);
                    resultado.Add(ITEMS, cboFrentes);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    if (Estatus == 1)
                    {
                        resultado.Add(ITEMS, "No tiene permisos para autorizar.");
                        resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        resultado.Add(ITEMS, "No tiene permisos para rechazar.");
                        resultado.Add(SUCCESS, false);
                    }
                }

            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region CATÁLOGO DE FRENTES
        public ActionResult FillcboUsuarios()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboUsuarios = BackLogsFactoryServices.FillcboUsuarios();
                resultado.Add(ITEMS, cboUsuarios);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFrente()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<CatFrentesDTO> lstFrentes = BackLogsFactoryServices.GetFrentes();
                resultado.Add("lstFrentes", lstFrentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarFrente(tblBL_CatFrentes objFrente)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (objFrente != null)
                {
                    if (objFrente.id > 0)
                    {
                        bool esEditar = BackLogsFactoryServices.ActualizarFrente(objFrente);
                        if (esEditar)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                    else
                    {
                        bool esCrear = BackLogsFactoryServices.CrearFrente(objFrente);
                        if (esCrear)
                            resultado.Add(SUCCESS, true);
                        else
                            resultado.Add(SUCCESS, false);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFrente(int idFrente)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idFrente > 0)
                {
                    bool EliminarFrente = BackLogsFactoryServices.EliminarFrente(idFrente);
                    if (EliminarFrente)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SEGUIMIENTO PPTO
        public ActionResult GetSeguimientoPpto(string AreaCuenta, string motivo, int estatusPpto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstSeguimiento = BackLogsFactoryServices.GetSeguimientoPpto(AreaCuenta, motivo, estatusPpto);
                resultado.Add("lstSeguimiento", lstSeguimiento);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeguimientoPptoFrentes(string AreaCuenta, string ObraoRenta, int idFrente)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstSeguimientoFrente = BackLogsFactoryServices.GetSeguimientoPptoFrentes(AreaCuenta, ObraoRenta, idFrente);
                resultado.Add("lstSeguimientoFrente", lstSeguimientoFrente);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetSeguimientoPptoFiltros(tblBL_SeguimientoPptos obj)
        //{
        //    var resultado = new Dictionary<string, object>();
        //    try
        //    {
        //        var lstSeguimiento = BackLogsFactoryServices.GetSeguimientoPptoFiltros(obj);
        //        resultado.Add("lstSeguimiento", lstSeguimiento);
        //        resultado.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.Add(MESSAGE, e.Message);
        //        resultado.Add(SUCCESS, false);
        //    }
        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult FillCboFrentes()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cboFrentes = BackLogsFactoryServices.FillCboFrentes();
                resultado.Add(ITEMS, cboFrentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DETALLE FRENTE
        public ActionResult GetDetFrentes(string AreaCuenta, List<int> lstFrentes, int estatusSeguimientoFrente)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<DetFrentesDTO> lstDetFrentes = BackLogsFactoryServices.GetDetFrentes(AreaCuenta, lstFrentes, estatusSeguimientoFrente);
                resultado.Add("lstDetFrentes", lstDetFrentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearDetFrentes2(List<Seguimiento2DTO> parametros)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var CrearDetFrentes = BackLogsFactoryServices.CrearDetFrentes(parametros);
                resultado.Add("CrearDetFrentes", CrearDetFrentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarDetFrente(int idFrente)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (idFrente > 0)
                {
                    bool EliminarFrente = BackLogsFactoryServices.EliminarDetFrente(idFrente);
                    if (EliminarFrente)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region INFORME

        public ActionResult obtenerArchivoCODescargas(int idFormatoCambio)
        {
            var resultado = new Dictionary<string, object>();
            try
            {

                resultado = BackLogsFactoryServices.obtenerArchivoCODescargas(idFormatoCambio);


                resultado.Add(SUCCESS, resultado.Count > 0);
            }
            catch (Exception o_O)
            {
                resultado.Add("message", o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult postSubirArchivos(List<HttpPostedFileBase> archivo, int tipoEvidencia)
        {
            int id = (JsonConvert.DeserializeObject<int>(Request.Form["id"])).ParseInt();

            resultado = BackLogsFactoryServices.postSubirArchivos(id, archivo, tipoEvidencia);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivos(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                List<BackLogs_ArchivosDTO> lstArchivos = BackLogsFactoryServices.GetArchivos(id);
                resultado.Add("lstArchivos", lstArchivos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivos(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id > 0)
                {
                    bool EliminarArchivo = BackLogsFactoryServices.EliminarArchivos(id);
                    if (EliminarArchivo)
                        resultado.Add(SUCCESS, true);
                    else
                        resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarLiberacion(string areaCuenta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var actualizarLiberacion = BackLogsFactoryServices.ActualizarLiberacion(areaCuenta);
                resultado.Add("actualizarLiberacion", actualizarLiberacion);
                if (actualizarLiberacion)
                {
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);

                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModeloGrupoCCSeleccionado(string noEconomico)
        {
            return Json(BackLogsFactoryServices.GetModeloGrupoCCSeleccionado(noEconomico), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE GENERAL
        public ActionResult GetReporteGeneral(int tipoBL, DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            resultado = BackLogsFactoryServices.GetReporteGeneral(tipoBL, fechaInicio, fechaFin, ac);

            if ((bool)resultado[SUCCESS])
            {
                var stream = (MemoryStream)resultado["excel"];

                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=RehabilitacionGeneral.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();

                return null;
            }
            else
                ViewBag.mensajeErrorDescarga = (string)resultado[MESSAGE];
                return View("ErrorDescargaConMensaje");
        }
        #endregion

        #region REHABILITACION DE INSPECCIÓN TMC
        [HttpPost]
        public JsonResult GetIndicadoresRehabilitacionTMC(IndicadoresRehabilitacionTMCDTO objFiltro)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstIndicadores = BackLogsFactoryServices.GetIndicadoresRehabilitacionTMC(objFiltro);
                if (lstIndicadores.Count() > 0)
                {
                    resultado.Add("lstDataTbl1", lstIndicadores["lstDataTbl1"]);
                    resultado.Add("categorieGrafica1", lstIndicadores["categorieGrafica1"]);
                    resultado.Add("dataGrafica1", lstIndicadores["dataGrafica1"]);

                    resultado.Add("lstDataTbl2", lstIndicadores["lstDataTbl2"]);
                    resultado.Add("dataPorcGlobal", lstIndicadores["dataPorcGlobal"]);

                    resultado.Add("lstDataTbl3", lstIndicadores["lstDataTbl3"]);
                    resultado.Add("lstGrafica3", lstIndicadores["lstGrafica3"]);

                    resultado.Add("lstDataTbl4", lstIndicadores["lstDataTbl4"]);
                    resultado.Add("lstCantLiberados", lstIndicadores["lstCantLiberados"]);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add("lstDataTbl1", 0);
                    resultado.Add("categorieGrafica1", 0);
                    resultado.Add("dataGrafica1", 0);

                    resultado.Add("lstDataTbl2", 0);
                    resultado.Add("dataPorcGlobal", 0);

                    resultado.Add("lstDataTbl3", 0);
                    resultado.Add("lstGrafica3", 0);

                    resultado.Add("lstDataTbl4", 0);
                    resultado.Add("lstCantLiberados", 0);
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBackLogs(int id)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                List<BackLogsDTO> lstBL = BackLogsFactoryServices.GetBackLogs(id);
                resultado.Add(SUCCESS, true);
                resultado.Add("lstBL", lstBL);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE E INDICADORES
        public JsonResult GetReporteIndicadores(BackLogsDTO objParamsDTO)
        {
            return Json(BackLogsFactoryServices.GetReporteIndicadores(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReporteIndicadoresGrafica(string areaCuenta)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstReporteIndicadoresGrafica = BackLogsFactoryServices.GetReporteIndicadoresGrafica(areaCuenta);
                if (lstReporteIndicadoresGrafica.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstGraficaResposable", lstReporteIndicadoresGrafica["lstGraficaResposable"]);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoGraficaResponsables(BackLogsDTO objParamsDTO)
        {
            return Json(BackLogsFactoryServices.GetInfoGraficaResponsables(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportePorResponsables(BackLogsDTO objFiltro, DateTime fechaInicio, DateTime fechaFin)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstReporteIndicadores = BackLogsFactoryServices.GetReportePorResponsables(objFiltro, fechaInicio, fechaFin);
                if (lstReporteIndicadores.Count() > 0)
                {
                    resultado.Add("lstResponsables", lstReporteIndicadores["lstResponsables"]);
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndicadorBacklogPorEquipo(string AreaCuenta, DateTime fechaInicio, DateTime fechaFin, TipoMaquinaEnum tipoEquipo = 0, EstatusBackLogEnum estatus = 0)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstReporteIndicadores = BackLogsFactoryServices.GetIndicadorBacklogPorEquipo(AreaCuenta, fechaInicio, fechaFin, tipoEquipo, estatus);
                if (lstReporteIndicadores.Count() > 0)
                {
                    resultado.Add("lstEquipos", lstReporteIndicadores["lstEquipos"]);
                    resultado.Add("catGraficaPorEquipo", lstReporteIndicadores["catGraficaPorEquipo"]);
                    resultado.Add("dataGraficaCostoHora", lstReporteIndicadores["dataGraficaCostoHora"]);
                    resultado.Add("dataGraficaCostoMes", lstReporteIndicadores["dataGraficaCostoMes"]);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("No se encontraron registros correspondientes");
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

                public ActionResult GetBackLogsPresupuesto(string noEconomico)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lstBackLogsPresupuesto = BackLogsFactoryServices.GetBackLogsPresupuesto(noEconomico);
                resultado.Add("lstBackLogsPresupuesto", lstBackLogsPresupuesto);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region GRAFICA POR RESPOSABLES
        public JsonResult GetGraficaResponsables(string areaCuenta, List<int> lstMeses)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var lstResposables = BackLogsFactoryServices.GetReporteIndicadores(null);
                if (lstResposables.Count() > 0)
                {
                    resultado.Add("lstResponsables", lstResposables["lstResponsables"]);

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetBackLogsGraficaresponsable(int inicioMes, int finMes, string areaCuenta, List<int> _lstResponsables, int inicioAnio, int finAnio)
        {
            var resultado = BackLogsFactoryServices.GetBackLogsGraficaresponsable(inicioMes, finMes, areaCuenta, _lstResponsables, inicioAnio, finAnio);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EXCEL CARGO NOMINA
        public ActionResult FillComboCentroCostoBackLogs()
        {
            return Json(BackLogsFactoryServices.FillComboCentroCostoBackLogs(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGraficaCargoNomina(List<string> listaEconomicos, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(BackLogsFactoryServices.GetGraficaCargoNomina(listaEconomicos, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelCargoNomina(string imagenString)
        {
            var stream = BackLogsFactoryServices.DescargarExcelCargoNomina(imagenString);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Cargo Nómina.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GENERALES
        public ActionResult FillCboTipoMonedas()
        {
            return Json(BackLogsFactoryServices.FillCboTipoMonedas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAC()
        {
            return Json(BackLogsFactoryServices.FillCboAC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoEquipo()
        {
            return Json(BackLogsFactoryServices.FillCboTipoEquipo(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}