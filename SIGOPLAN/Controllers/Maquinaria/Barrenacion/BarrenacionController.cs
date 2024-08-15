using Core.DAO.Maquinaria.Barrenacion;
using Core.DTO;
using Core.DTO.Maquinaria.Captura.conciliacion;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using Core.Enum.Maquinaria.ConciliacionHorometros;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Factory.Maquinaria.Barrenacion;
using Core.Entity.Maquinaria.Barrenacion;
using Core.Enum.Maquinaria.Barrenacion;
using System.IO;

namespace SIGOPLAN.Controllers.Maquinaria.Barrenacion
{
    public class BarrenacionController : BaseController
    {
        IBarrenacionDAO barrenacionService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            barrenacionService = new BarrenacionFactoryService().GetBarrenacionService();
            base.OnActionExecuting(filterContext);
        }

        #region Combos

        
        [HttpPost]
        public ActionResult ObtenerAC()
        {
            return Json(barrenacionService.ObtenerAC(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Mano de Obra
        // GET: Barrenacion/ManoObra
        public ViewResult ManoObra()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerBarrenadorasOperadores(string areaCuenta, int estatusOperadores)
        {
            return Json(barrenacionService.ObtenerBarrenadorasOperadores(areaCuenta, estatusOperadores), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerOperadoresBarrenadora(int barrenadoraID, int turno)
        {
            return Json(barrenacionService.ObtenerOperadoresBarrenadora(barrenadoraID, turno), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEmpleadosEnKontrol(string term, bool porDesc)
        {
            return Json(barrenacionService.ObtenerEmpleadosEnKontrol(term, porDesc), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarOperadoresBarrenadora(List<tblB_ManoObra> listaOperadores)
        {
            return Json(barrenacionService.GuardarOperadoresBarrenadora(listaOperadores), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Piezas por barrenadora
        // GET: Barrenacion/PiezasBarrenadora
        public ViewResult PiezasBarrenadora()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerBarrenadorasPiezas(string areaCuenta, int estatusPiezas)
        {
            return Json(barrenacionService.ObtenerBarrenadorasPiezas(areaCuenta, estatusPiezas), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerInsumosPorPiezaConPrecio(string areaCuenta)
        {
            return Json(barrenacionService.ObtenerInsumosPorPiezaPrecio(areaCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerPiezasBarrenadora(int barrenadoraID, string areaCuenta)
        {
            return Json(barrenacionService.ObtenerPiezasBarrenadora(barrenadoraID, areaCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getInsumo(string term, bool porDesc)
        {
            return Json(barrenacionService.getInsumo(term, porDesc), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarPiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPiezas, string desechoMartillo, string desechoBarra, bool pzasCompletas)
        {
            return Json(barrenacionService.GuardarPiezasBarrenadora(listaPiezas, desechoMartillo, desechoBarra, pzasCompletas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerSerieMartilloReparadoNoAsignado(string term, int tipoPieza)
        {
            var res = new object();
            try
            {
                res = barrenacionService.ObtenerSerieMartilloReparadoNoAsignado(term);
            }
            catch (Exception e)
            {
                res = null;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerBarrenadorasAutocomplete(string term)
        {
            return Json(barrenacionService.ObtenerBarrenadorasAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarNuevaBarrenadora(int maquinaID)
        {
            return Json(barrenacionService.GuardarNuevaBarrenadora(maquinaID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerTiposCapturas(int maquinaID)
        {
            return Json(GlobalUtils.ParseEnumToCombo<TipoCapturaEnum>(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Captura Diaria
        public ViewResult CapturaDiaria()
        {
            return View();
        }
        public ActionResult ObtenerBarrenadorasCaptura(string areaCuenta, int turno, DateTime fecha)
        {
            var result = barrenacionService.ObtenerBarrenadorasCaptura(areaCuenta, turno, fecha);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarCapturaDiaria(List<tblB_CapturaDiaria> listaCaptura, DateTime fecha)
        {
            return Json(barrenacionService.GuardarCapturaDiaria(listaCaptura, fecha), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reparacion
        public ViewResult Reparacion()
        {
            return View();
        }

        public ActionResult ObtenerPiezasPorReparar()
        {
            return Json(barrenacionService.ObtenerPiezasPorReparar(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult setAceptarPieza(int idRepara)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var esGuardado = idRepara > 0;
                if (esGuardado)
                {
                    esGuardado = barrenacionService.ActualizarPiezaEstadoReparacion(TipoMovimientoPiezaEnum.ReparacionMartillo, idRepara, null);
                }
                resultado.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setRechazoPieza(int idRechaza, string comentario)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var esGuardado = idRechaza > 0 && comentario.Count() > 0;
                if (esGuardado)
                {
                    esGuardado = barrenacionService.ActualizarPiezaEstadoReparacion(TipoMovimientoPiezaEnum.BajaDesecho, idRechaza, comentario);
                }
                resultado.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Catálogo de Piezas
        public ViewResult CatalogoPieza()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerInsumosPorPieza(string areaCuenta)
        {
            return Json(barrenacionService.ObtenerInsumosPorPieza(areaCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AgregarInsumoPieza(tblB_CatalogoPieza nuevoInsumoPieza)
        {
            return Json(barrenacionService.AgregarInsumoPieza(nuevoInsumoPieza), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarInsumoPieza(int id)
        {
            return Json(barrenacionService.EliminarInsumoPieza(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerTiposBroca()
        {
            return Json(GlobalUtils.ParseEnumToCombo<TipoBrocaEnum>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrUpdatePieza(List<tblB_PiezaBarrenadora> obj)
        {
            return Json(barrenacionService.SaveOrUpdatePieza(obj), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reporte de captura diaria
        // GET: Barrenacion/ReporteCapturaDiaria
        public ViewResult ReporteCapturaDiaria()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargarCapturasDiarias(List<string> areaCuenta, List<int> barrenadoraID, List<int> turnos, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(barrenacionService.CargarCapturasDiarias(areaCuenta, barrenadoraID, turnos, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reporte de rendimiento por pieza
        // GET: Barrenacion/ReporteRendimiento
        public ViewResult ReporteRendimiento()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerTiposPieza()
        {
            return Json(GlobalUtils.ParseEnumToCombo<TipoPiezaEnum>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarRendimientoPiezas(string areaCuenta, List<int> tipoPieza, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(barrenacionService.CargarRendimientoPiezas(areaCuenta, tipoPieza, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ObtenerBarrenadorasPorCC(string areaCuenta)
        {
            return Json(barrenacionService.ObtenerBarrenadorasPorCC(areaCuenta), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Catalogo de banco
        public ViewResult CatalogoBanco()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarBanco(tblB_CatalogoBanco nuevoBanco)
        {
            return Json(barrenacionService.AgregarBanco(nuevoBanco), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerBancos(string areaCuenta)
        {
            return Json(barrenacionService.ObtenerBancos(areaCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetComboBancos(string areaCuenta)
        {
            return Json(barrenacionService.GetComboBancos(areaCuenta), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region otros
        [HttpGet]
        public ActionResult GetPiezaID(int piezaID)
        {
            return Json(barrenacionService.GetPiezaID(piezaID), JsonRequestBehavior.AllowGet);
        }

        //raguilar 13/01/20 
        public ViewResult ReporteBarrenacionCosto()
        {
            return View();
        }

        public ActionResult GetBarrenacionCosto()
        {
            return Json(barrenacionService.GetBarrenacionCosto(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GuardarBarrenacionCosto(tblB_BarrenacionCosto registroInformacion, List<tblB_BarrenacionCostoPiezaDetalle> lstPiezaDetalle, List<tblB_BarrenacionCostoOtroDetalle> lstOtroDetalle)
        {
            return Json(barrenacionService.GuardarBarrenacionCosto(registroInformacion, lstOtroDetalle, lstPiezaDetalle), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoInsumo(int barrenadoraID, int insumo, string areaCuenta)
        {
            return Json(barrenacionService.getInfoInsumo(barrenadoraID, insumo, areaCuenta), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPiezaNueva(int insumo, string areaCuenta)
        {
            return Json(barrenacionService.getPiezaNueva(insumo, areaCuenta), JsonRequestBehavior.AllowGet);
        }
        public ActionResult setInfoCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas)
        {
            return Json(barrenacionService.SaveOrUpdateCapturaDiaria(listaCapturaDiaria, listaPiezas), JsonRequestBehavior.AllowGet);
        }
        public ViewResult reporteEjecutivo()
        {
            return View();
        }
        public ActionResult setInfoRptEjecutivo(string pfechaInicio, string pfechaFin, List<string> areaCuentas, List<int> barrenadoras)
        {
            DateTime fechaInicio = Convert.ToDateTime(pfechaInicio);
            DateTime fechaFin = Convert.ToDateTime(pfechaFin);
            return Json(barrenacionService.setReporteEjecutivo(fechaInicio, fechaFin, areaCuentas, barrenadoras), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCapturaDiaria(int barrenadoraID, string pfechaInicio, int turno)
        {
            DateTime fechaInicio = Convert.ToDateTime(pfechaInicio);
            return Json(barrenacionService.ObtenerCapturaDiaria(barrenadoraID, fechaInicio, turno), JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarAgua(string areaCuenta, int turno, string fechaCaptura, decimal litros, int id)
        {
            return Json(barrenacionService.guardarAgua(areaCuenta, turno, fechaCaptura, litros, id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarOtrosPrecios(string areaCuenta, int turno, string fechaCaptura, decimal monto, int id)
        {
            return Json(barrenacionService.guardarOtrosPrecios(areaCuenta, turno, fechaCaptura, monto, id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCaptura(int capturaID)
        {
            return Json(barrenacionService.EliminarCaptura(capturaID), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reporte General Capturas
        public ViewResult ReporteGeneralCapturas()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CargarRptGeneralCapturas(List<string> areaCuenta, List<int> turno, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(barrenacionService.CargarRptGeneralCapturas(areaCuenta, turno, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Reporte de Estadistico Operadores

        public ViewResult ReporteEstadisticoOperadores()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerOperadores(string areaCuenta)
        {
            return Json(barrenacionService.ObtenerOperadores(areaCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarRptOperadores(List<string> areaCuenta, List<int> operadores, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(barrenacionService.CargarRptOperadores(areaCuenta, operadores, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Reporte De equipos.
        public ViewResult ReporteEquiposStandby()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargarRptEquiposstanby(List<string> areaCuenta, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(barrenacionService.CargarRptEquiposstanby(areaCuenta, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SetReporteStandby()
        {
            HttpPostedFileBase archivoContrato = Request.Files["picture"];

            byte[] imgDataTest = null;
            Stream streamD = archivoContrato.InputStream;
            using (var streamReader = new MemoryStream())
            {
                streamD.CopyTo(streamReader);
                imgDataTest = streamReader.ToArray();
            }
            Session["imgRptStandbyBarrenadora"] = imgDataTest;

            return Json(true, JsonRequestBehavior.AllowGet); ;
        }

        #endregion

        #region Captura de Precios

        public ViewResult CapturaCostos()
        {
            return View();
        }

        public ActionResult guardarPagoMensual(string areaCuenta, DateTime fechaCaptura, decimal cantidad, int id)
        {
            return Json(barrenacionService.guardarPagoMensual(areaCuenta, fechaCaptura, cantidad, id), JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult SaveOrUpdatePiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPzas, bool pzasCompletas)
        {
            return Json(barrenacionService.SaveOrUpdatePiezasBarrenadora(listaPzas, pzasCompletas), JsonRequestBehavior.AllowGet);
        }
    }
}