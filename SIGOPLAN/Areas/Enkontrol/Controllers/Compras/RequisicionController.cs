using Core.DTO;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Utils.DataTable;
using Core.Entity.Enkontrol.Compras;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Enum.Enkontrol.Requisicion;
using Data.Factory.Enkontrol.Compras;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Compras
{
    public class RequisicionController : BaseController
    {
        // GET: Enkontrol/Requisicion
        private RequisicionFactoryServices rfs;
        private UsuarioFactoryServices ufs;
        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            rfs = new RequisicionFactoryServices();
            ufs = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #region Captura
        public ActionResult Solicitar(int? id)
        {
            Session["servicioRequi"] = id.HasValue && id.Value == 2 ? true : false;
            ViewBag.tipoRequisicion = id.HasValue && id.Value == 2 ? true : false;

            return View();
        }
        public ActionResult Editar()
        {
            return View();
        }
        public ActionResult Surtido()
        {
            return View();
        }
        public ActionResult Entradas()
        {
            return View();
        }
        public ActionResult Salidas()
        {
            return View();
        }
        public ActionResult checkEditar()
        {
            var result = new Dictionary<string, object>();

            var empresa = vSesiones.sesionEmpresaActual;
            var menuID = vSesiones.sesionCurrentView;

            bool check = false;

            if (empresa == 1 && menuID == 7246)
            {
                check = true;
            }

            if (empresa == 2 && menuID == 7244)
            {
                check = true;
            }

            result.Add("check", check);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _renglonVacio()
        {
            return PartialView();
        }
        public ActionResult _mdlUsuarioEnkontrol()
        {
            return PartialView();
        }
        public ActionResult SeguimientoRequisiciones()
        {
            return View();
        }
        public ActionResult _renglonNuevo(int partida, bool nuevo, bool cancelado)
        {
            ViewBag.partida = partida;
            ViewBag.nuevo = nuevo;
            ViewBag.cancelado = cancelado;

            if (vSesiones.sesionCurrentView == 7221)
            {
                ViewBag.vistaAutorizar = true;
            }
            else
            {
                ViewBag.vistaAutorizar = false;
            }

            //Condición para los dos ID's porque no coinciden las tablas "tblP_Menu" en las dos bases de datos
            if (vSesiones.sesionCurrentView == 7252 || vSesiones.sesionCurrentView == 7251)
            {
                ViewBag.vistaSurtido = true;
            }
            else
            {
                ViewBag.vistaSurtido = false;
            }

            return PartialView();
        }
        public ActionResult guardar(tblCom_Req req, List<tblCom_ReqDet> det, List<ReqDetalleComentarioDTO> comentarios)
        {
            if (Session["servicioRequi"] != null)
            {
                bool servicio = (bool)Session["servicioRequi"];
                req.PERU_tipoRequisicion = servicio ? "RS" : "RQ";
            }

            if (req.idBL > 0)
                req.PERU_tipoRequisicion = "RQ";

            return Json(rfs.getReqService().guardar(req, det, comentarios), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getNewReq(string cc)
        {
            string tpRequi = string.Empty;
            if (Session["servicioRequi"] != null)
            {
                bool servicio = (bool)Session["servicioRequi"];
                tpRequi = servicio ? "RS" : "RQ";
            }

            return Json(rfs.getReqService().getNewReq(cc, tpRequi), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUltimaRequisicionSIGOPLAN(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var req = rfs.getReqService().getUltimaRequisicionSIGOPLAN(cc);

                result.Add("numero", req != null ? req[0].numero.Value : 0);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add("numero", 0);
                result.Add("solicitoNom", "Default");
                result.Add("solicito", 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRequisicion(string cc, int num, bool esServicio = false)
        {
            return Json(rfs.getReqService().getRequisicion(cc, num, esServicio), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReq(string cc, int num)
        {
            return Json(rfs.getReqService().getReq(cc, num), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFolio(string folio, int tipo)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var existe = rfs.getReqService().getFolio(folio, tipo);

                result.Add("existe", existe);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistenciaInsumo(int insumo, string cc, int almacen)
        {
            result = rfs.getReqService().getExistenciaInsumo(insumo, cc, almacen);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistenciaInsumoDetalle(int insumo)
        {
            result = rfs.getReqService().getExistenciaInsumoDetalle(insumo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistenciaInsumoDetalleTotal(int insumo)
        {
            result = rfs.getReqService().getExistenciaInsumoDetalleTotal(insumo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistenciaInsumoDetalleAlmacenFisico(int insumo)
        {
            result = rfs.getReqService().getExistenciaInsumoDetalleAlmacenFisico(insumo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistenciaInsumoDetalleTotalAlmacenFisico(int insumo)
        {
            result = rfs.getReqService().getExistenciaInsumoDetalleTotalAlmacenFisico(insumo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TiempoProcesoOC()
        {
            return View();
        }        
        #endregion
        #region Autorizacion
        public ActionResult Autorizacion()
        {
            return View();
        }
        public ActionResult setAuth(List<tblCom_Req> lst)
        {
            return Json(rfs.getReqService().setAuth(lst), JsonRequestBehavior.AllowGet);
        }
        public ActionResult isEmpAdmin()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("isEmpAdmin", rfs.getReqService().isEmpAdmin());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add("isEmpAdmin", false);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncReq(List<string> cc, bool isAuth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstReq = rfs.getReqService().getReq(isAuth, cc);
                result.Add("lstReq", lstReq);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult CalcularExistenciasRequisicion(int almacen, List<int> listaInsumos)
        {
            return Json(rfs.getReqService().CalcularExistenciasRequisicion(almacen, listaInsumos), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region cbo
        public ActionResult FillComboLab()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboLab());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region autoComplete
        public ActionResult getInsumos(string term, string cc, bool esServicio = false)
        {
            var items = rfs.getReqService().getInsumos(term, cc, esServicio);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosDesc(string term, string cc, bool esServicio = false)
        {
            var items = rfs.getReqService().getInsumosDesc(term, cc, esServicio);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosByAlmacen(string term, string cc, int almacen)
        {
            var items = rfs.getReqService().getInsumosByAlmacen(term, cc, almacen);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosDescByAlmacen(string term, string cc, int almacen)
        {
            var items = rfs.getReqService().getInsumosDescByAlmacen(term, cc, almacen);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosByAlmacenEntrada(string term, string cc, int almacen)
        {
            var items = rfs.getReqService().getInsumosByAlmacenEntrada(term, cc, almacen);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosDescByAlmacenEntrada(string term, string cc, int almacen)
        {
            var items = rfs.getReqService().getInsumosDescByAlmacenEntrada(term, cc, almacen);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosTraspaso(string term, string cc)
        {
            var items = rfs.getReqService().getInsumos(term, cc, false);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosTraspasoDesc(string term, string cc)
        {
            var items = rfs.getReqService().getInsumosDesc(term, cc, false);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult FillComboTipoReq()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboTipoReq());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboResponsablePorCc(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboResponsablePorCc(cc));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboAreaCuenta(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboAreaCuenta(cc));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcReq(bool isAuth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboCcReq(isAuth));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcAsigReq()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboCcAsigReq());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcTodos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboCcTodos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoFolio()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboTipoFolio());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTxtFolio(int tipo = 0)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboTxtFolio(tipo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboAlmacenSurtir()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboAlmacenSurtir());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAlmacenSurtirAcceso()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboAlmacenSurtirAcceso());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAlmacenSurtirTodos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboAlmacenSurtirTodos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboEstatusSurtidoRequisicion()
        {
            var result = new Dictionary<string, object>();
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<EstatusSurtidoRequisicionEnum>());
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoInsumo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboTipoInsumo());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboProveedores());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboProveedoresConsignaLicitacionConvenio(TipoConsignaLicitacionConvenioEnum tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboProveedoresConsignaLicitacionConvenio(tipo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Usuario
        public ActionResult getThisUsuarioEnkontrol()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var empleado = rfs.getReqService().getThisEmpleadoEnkontrol();
                result.Add("empleado", (int)empleado[0].empleado);
                result.Add("nombre", (string)empleado[0].nom);
                result.Add("ekUsuario", (string)empleado[0].num);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                result.Add("nombre", string.Format("{0} {1} {2}", usuario.nombre, usuario.apellidoPaterno, usuario.apellidoMaterno));
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult InsumosConsignaLicitacionConvenio()
        {
            return View();
        }

        #region SALIDA POR CONSUMO
        public ActionResult SalidaConsumo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetReqSalidasConsumo(string cc, int tipo)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getReqSalidasConsumo(cc, tipo);
                var djson = JsonConvert.SerializeObject(data);
                result.Add(SUCCESS, true);
                result.Add("data", djson);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }
        public ActionResult GetReqDetSalidasConsumo(string cc, int req, int almacen)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getReqDetSalidasConsumo(cc, req, almacen);

                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarConsumo(bool salidaNormal, List<SurtidoDetDTO> salidasConsumo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var flagMaquinaStandBy = false;
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    flagMaquinaStandBy = rfs.getReqService().checkMaquinaStandBy(salidasConsumo[0].cc);
                }

                List<salidasAlmacenDTO> folioSalidaConsumo = new List<salidasAlmacenDTO>();
                List<salidasAlmacenDTO> folioSalida = new List<salidasAlmacenDTO>();
                List<entradasAlmacenDTO> folioEntrada = new List<entradasAlmacenDTO>();
                bool seGuardo = false;

                //SI ES SALIDA NORMAL, SOLO SE HACE UN MOVIMIENTO DE SALIDA POR CONSUMO
                if (salidaNormal)
                {
                    folioSalidaConsumo = rfs.getReqService().guardarSalidasConsumo(salidasConsumo, salidaNormal);

                    seGuardo = folioSalidaConsumo.Count > 0;
                }
                else
                {
                    rfs.getReqService().checarUbicacionesValidas(salidasConsumo);

                    folioSalida = rfs.getReqService().GuardarSalidasC(salidasConsumo);
                    folioEntrada = rfs.getReqService().GuardarEntradasConsumo(salidasConsumo);
                    folioSalidaConsumo = rfs.getReqService().guardarSalidasConsumo(salidasConsumo, salidaNormal);

                    seGuardo = folioSalida.Count > 0 && folioEntrada.Count > 0 && folioSalidaConsumo.Count > 0;
                }

                result.Add(SUCCESS, seGuardo);
                result.Add("flagMaquinaStandBy", flagMaquinaStandBy);

                if (seGuardo)
                {
                    Session["folioSalidaConsumo"] = folioSalidaConsumo;
                };
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GuardarSalidasConsumo(List<SurtidoDetDTO> salidas)
        //{
        //    var result = new Dictionary<string, object>();

        //    try
        //    {
        //        List<salidasAlmacenDTO> folios = rfs.getReqService().guardarSalidasConsumo(salidas);

        //        result.Add(SUCCESS, true);
        //        Session["foliosSalidas"] = folios;
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion
        #region Origen Por Stock

        public ActionResult OrigenStock()
        {
            return View();
        }


        #endregion

        #region Pendiente por Surtir
        public ActionResult PendienteSurtir()
        {
            return View();
        }

        public ActionResult ObtenerRequisicionesPendientes(List<string> listaCC, List<int> listaAlmacenes, int estatus, int validadoAlmacen, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().ObtenerRequisicionesPendientes(listaCC, listaAlmacenes, estatus, validadoAlmacen, fechaInicio, fechaFin);
                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Ocurrió un error interno.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ConfirmarRequisiciones()
        {
            return View();
        }
        public ActionResult MesaAnalisis()
        {
            return View();
        }
        public ActionResult GuardarSurtido(RequisicionDTO info, List<SurtidoDTO> lstSurtido)
        {
            var result = new Dictionary<string, object>();

            try
            {
                rfs.getReqService().GuardarSurtido(info, lstSurtido);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalidas(int almacenOrigenID, int almacenDestinoID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getSalidas(almacenOrigenID, almacenDestinoID);

                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSalidas(List<SurtidoDetDTO> salidas)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var flagMaquinaStandBy = false;
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    flagMaquinaStandBy = rfs.getReqService().checkMaquinaStandBy(salidas[0].cc);
                }

                List<salidasAlmacenDTO> folios = rfs.getReqService().GuardarSalidas(salidas);

                result.Add(SUCCESS, true);
                result.Add("flagMaquinaStandBy", flagMaquinaStandBy);
                Session["foliosSalidasTraspasos"] = folios;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntradas(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getEntradas(almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino);

                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalidaTraspaso(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino, int folioTraspaso)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getSalidaTraspaso(almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino, folioTraspaso);

                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEntradas(List<SurtidoDetDTO> entradas, int folio_traspaso, int almacenDestinoOriginal)
        {
            var result = new Dictionary<string, object>();

            try
            {
                rfs.getReqService().checarUbicacionesValidas(entradas);

                var data = rfs.getReqService().GuardarEntradas(entradas, folio_traspaso, almacenDestinoOriginal);

                if (data == null)
                {
                    Session["entradaConsultaTraspaso"] = null;
                }
                else
                {
                    Session["entradaConsultaTraspaso"] = data;
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMovSalidaAlmacen(int almacen_id, string cc, int folioSalida)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var movimiento = rfs.getReqService().getMovSalidaAlmacen(almacen_id, cc, folioSalida);

                result.Add(SUCCESS, true);
                result.Add("movimiento", movimiento);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarSurtido(string cc, int numero)
        {
            return Json(rfs.getReqService().validarSurtido(cc, numero), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarSurtidoCompras(string cc, int numero)
        {
            return Json(rfs.getReqService().validarSurtidoCompras(cc, numero), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequisicionesPorUsuarioProcesadas(List<string> listCC)
        {
            return Json(rfs.getReqService().getRequisicionesPorUsuarioProcesadas(listCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidacionesRequisitor(string cc, List<int> numeros)
        {
            return Json(rfs.getReqService().validacionesRequisitor(cc, numeros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUbicacionDetalle(string cc, int almacenID, int insumo)
        {
            return Json(rfs.getReqService().getUbicacionDetalle(cc, almacenID, insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUbicacionPorRequisicion(RequisicionDTO requisicion)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getUbicacionPorRequisicion(requisicion);

                List<UbicacionDetalleDTO> listUbicacionDetalle = new List<UbicacionDetalleDTO>();

                if ((List<UbicacionDetalleDTO>)Session["borradorDetalle"] != null)
                {
                    listUbicacionDetalle.AddRange((List<UbicacionDetalleDTO>)Session["borradorDetalle"]);
                }
                if (data != null)
                {
                    listUbicacionDetalle.AddRange(data);
                }

                Session["borradorDetalle"] = listUbicacionDetalle;

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LimpiarSesionBorrador()
        {
            var result = new Dictionary<string, object>();

            try
            {
                Session["borradorDetalle"] = null;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmarRequisicion(RequisicionDTO requisicion)
        {
            return Json(rfs.getReqService().confirmarRequisicion(requisicion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getInsumosAutoComplete(string term)
        {
            var items = rfs.getReqService().getInsumosAutoComplete(term);

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInsumosDescAutoComplete(string term)
        {
            var items = rfs.getReqService().getInsumosDescAutoComplete(term);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoInformacion(int insumo, bool esServicio = false)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", rfs.getReqService().getInsumoInformacion(insumo, esServicio));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoInformacionByAlmacen(int insumo, int almacen)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", rfs.getReqService().getInsumoInformacionByAlmacen(insumo, almacen));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoInformacionByAlmacenEntrada(int insumo, int almacen)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", rfs.getReqService().getInsumoInformacionByAlmacenEntrada(insumo, almacen));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSurtidoDetalle(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", rfs.getReqService().getSurtidoDetalle(cc, numero));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarRequisicionesMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                rfs.getReqService().BorrarRequisicionesMasivo();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarRequisicion(string cc, int numero)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool servicio = false;
                if (Session["servicioRequi"] != null)
                    servicio = (bool)Session["servicioRequi"];

                rfs.getReqService().borrarRequisicion(cc, numero, servicio ? "RS" : "RQ");

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequisicionesSeguimiento(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, int consigna, int compradorSugeridoEnReq = 0)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getRequisicionesSeguimiento(listaCC, listaTipoInsumo, fechaInicial, fechaFinal, estatus, requisitor, compradorSugeridoEnReq)/*;.Where(x =>
                    requisitor != 0 ? x.sugeridoNum == requisitor : true
                ).ToList()*/;

                switch (consigna)
                {
                    case 1: //CONSIGNA
                        data = data.Where(x => x.consigna).ToList();
                        break;
                    case 2: //NO CONSIGNA
                        data = data.Where(x => !x.consigna).ToList();
                        break;
                    default:
                        break;
                }

                Session["seguimientoRequisiciones"] = data;

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;

            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region REPORTE TIEMPO DE PROCESO DE OC
        public ActionResult GetTiempoProcesoOC(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int comprador, int requisitor, int consigna, List<string> claveProveedor)
        {
            var result = new Dictionary<string, object>();
            
            try 
	        {
                var data = rfs.getReqService().GetTiempoProcesoOC(listaCC, listaTipoInsumo, fechaInicial, fechaFinal, estatus, requisitor, claveProveedor).Where(x =>
                    comprador != 0 ? x.sugeridoNum == comprador : true
                ).ToList();

                switch (consigna)
                {
                    case 1: //CONSIGNA
                        data = data.Where(x => x.consigna).ToList();
                        break;
                    case 2: //NO CONSIGNA
                        data = data.Where(x => !x.consigna).ToList();
                        break;
                    default:
                        break;
                }

                Session["tiempoProcesoOC"] = data;

                result.Add("data", data);
                result.Add(SUCCESS, true);

	        }
	        catch (Exception e)
	        {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
	        }

            var json = Json(result, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult FillComboProveedoresReporteProcesoOC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().FillComboProveedoresReporteProcesoOC());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult CancelarValidado(string cc, int numero)
        {
            return Json(rfs.getReqService().cancelarValidado(cc, numero), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteSurtidoRequisicion(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = rfs.getReqService().getReporteSurtidoRequisicion(cc, numero);

                Session["surtidoRequisicion"] = data;

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboRequisitores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, rfs.getReqService().getRequisitores().OrderBy(x => x.Text).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool esAdministrador()
        {
            return vSesiones.sesionUsuarioDTO.idPerfil == 1;
        }

        public bool permisoAuditoriaEliminarReqOC()
        {
            var acciones = new UsuarioFactoryServices().getUsuarioService().getLstAccionesActual();

            return acciones.Any(x => x.Accion == "Eliminar/Cancelar - Req/OC - Auditoría");
        }

        public ActionResult GetInsumoInformacionSurtido(int insumo, string cc, int numero_requisicion)
        {
            return Json(rfs.getReqService().getInsumoInformacionSurtido(insumo, cc, numero_requisicion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUbicacionDetalleSurtido(string cc, int numero_requisicion, int almacenID, int partida, int insumo)
        {
            return Json(rfs.getReqService().getUbicacionDetalleSurtido(cc, numero_requisicion, almacenID, partida, insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadoEnKontrolAutocomplete(string term)
        {
            return Json(rfs.getReqService().getEmpleadoEnKontrolAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        #region CRUD Insumos Consignación - Licitación - Convenio
        #region Consigna
        public ActionResult GetInsumosConsigna(DataTablesParam param)
        {
            var json = Json(rfs.getReqService().GetInsumosConsigna(param)["datos"], JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GuardarNuevoInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return Json(rfs.getReqService().GuardarNuevoInsumoConsigna(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return Json(rfs.getReqService().EditarInsumoConsigna(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return Json(rfs.getReqService().EliminarInsumoConsigna(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarExcelInsumosConsigna()
        {
            return Json(rfs.getReqService().CargarExcelInsumosConsigna(Request.Files), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelInsumosConsigna()
        {
            var stream = rfs.getReqService().DescargarExcelInsumosConsigna();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Insumos Consigna.xlsx"));
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

        #region Licitación
        public ActionResult GetInsumosLicitacion(DataTablesParam param)
        {
            var json = Json(rfs.getReqService().GetInsumosLicitacion(param)["datos"], JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GuardarNuevoInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return Json(rfs.getReqService().GuardarNuevoInsumoLicitacion(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return Json(rfs.getReqService().EditarInsumoLicitacion(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return Json(rfs.getReqService().EliminarInsumoLicitacion(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarExcelInsumosLicitacion()
        {
            return Json(rfs.getReqService().CargarExcelInsumosLicitacion(Request.Files), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelInsumosLicitacion()
        {
            var stream = rfs.getReqService().DescargarExcelInsumosLicitacion();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Insumos Licitacion.xlsx"));
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

        #region Convenio
        public ActionResult GetInsumosConvenio(DataTablesParam param)
        {
            var json = Json(rfs.getReqService().GetInsumosConvenio(param)["datos"], JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GuardarNuevoInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return Json(rfs.getReqService().GuardarNuevoInsumoConvenio(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return Json(rfs.getReqService().EditarInsumoConvenio(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return Json(rfs.getReqService().EliminarInsumoConvenio(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarExcelInsumosConvenio()
        {
            return Json(rfs.getReqService().CargarExcelInsumosConvenio(Request.Files), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelInsumosConvenio()
        {
            var stream = rfs.getReqService().DescargarExcelInsumosConvenio();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Insumos Convenio.xlsx"));
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
        #endregion

        public ActionResult GetArticulosConsignaLicitacionConvenioPorProveedor(int proveedor, TipoConsignaLicitacionConvenioEnum tipo)
        {
            return Json(rfs.getReqService().GetArticulosConsignaLicitacionConvenioPorProveedor(proveedor, tipo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoProveedorConsigna(int insumo, int proveedor)
        {
            return Json(rfs.getReqService().GetInsumoProveedorConsigna(insumo, proveedor), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoProveedorConvenio(int insumo, int proveedor)
        {
            return Json(rfs.getReqService().GetInsumoProveedorConvenio(insumo, proveedor), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoProveedorLicitacion(int insumo, int proveedor)
        {
            return Json(rfs.getReqService().GetInsumoProveedorLicitacion(insumo, proveedor), JsonRequestBehavior.AllowGet);
        }
    }
}
