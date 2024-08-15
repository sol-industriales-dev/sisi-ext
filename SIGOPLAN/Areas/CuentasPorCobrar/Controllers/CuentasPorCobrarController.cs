using Core.DAO.CuentasPorCobrar;
using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Contabilidad.Facturacion;
using Core.DTO.CuentasPorCobrar;
using Core.DTO.Maquinaria.Rentabilidad;
using Core.DTO.Maquinaria.Reporte.Kubrix;
using Core.DTO.Principal.Generales;
using Core.Entity.CuentasPorCobrar;
using Core.Enum.CuentasPorCobrar;
using Data.Factory.CuentasPorCobrar;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.CuentasPorCobrar.Controllers
{
    public class CuentasPorCobrarController : BaseController
    {
        IRentabilidadDAO rentabilidadFS;
        ICuentasPorCobrarDAO cxcInterfaz;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            rentabilidadFS = new RentabilidadFactoryServices().getRentabilidadDAO();
            cxcInterfaz = new CuentasPorCobrarFactoryService().getCuentasPorCobrarService();
            base.OnActionExecuting(filterContext);
        }

        #region VIEWS
        public ActionResult ControlEstimaciones()
        {
            return View();
        }

        public ActionResult Convenios()
        {
            return View();
        }

        public ActionResult GestionCobranza()
        {
            return View();
        }

        public ActionResult CuentasCobrar()
        {
            ViewBag.esAutorizar = cxcInterfaz.esAutorizarCXC();
            return View();
        }

        public ActionResult Kardex()
        {
            return View();
        }
        #endregion

        #region CxC
        public JsonResult getCXC(DateTime fecha, List<string> areaCuenta, int? idDivision)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaFin = fecha;
                busq.ccEnkontrol = areaCuenta;
                busq.idDivision = idDivision;
                var CXC = rentabilidadFS.getLstCXC(busq);

                Session["dataGetLstCXC"] = CXC;
                Session["fechaCorteSaldoClientes"] = fecha;

                result.Add("CXC", CXC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        public MemoryStream DescargarExcelCXC()
        {
            var data = (List<CuentasPendientesDTO>)Session["dataGetLstCXC"];
            var fechaCorte = (DateTime)Session["fechaCorteSaldoClientes"];
            var stream = rentabilidadFS.DescargarExcelCXC(data, fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Saldos de Clientes.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public JsonResult getCXCReporte(DateTime fecha, List<string> areaCuenta, int? idDivision)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaFin = fecha;
                busq.ccEnkontrol = areaCuenta;
                busq.idDivision = idDivision;
                result = rentabilidadFS.getLstCXCReporte(busq);
                //var CXC = rentabilidadFS.getLstCXCReporte(busq);
                //result.Add("CXC", CXC);
                //result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        public ActionResult getLstCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var lstCC = rentabilidadFS.getListaCCByUsuario(usuarioID, 0).Select(x => new
                {
                    areaCuenta = x.Value,
                    descripcion = x.Text,
                    guardado = x.Prefijo == "1"
                });
                result.Add("lstCC", lstCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult checkResponsable(int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(usuarioID, responsableID);
                result.Add(SUCCESS, true);
                result.Add("responsable", responsable);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult fillComboDivision()
        {
            //var result = new Dictionary<string, object>();
            //try
            //{
            //    //var lst = rentabilidadFS.getDivisiones().Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.id.ToString(), Text = x.division });
            //    var lst = 

            //    result.Add(ITEMS, lst);
            //}
            //catch (Exception)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            //}
            //var json = Json(result, JsonRequestBehavior.AllowGet);
            //return json;

            return Json(cxcInterfaz.fillComboDivision(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillComboResponsable()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var lst = rentabilidadFS.getResponsabilesAC(usuarioID)
                    .Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.usuarioResponsableID.ToString(), Text = x.usuarioResponsable.nombre + " " + x.usuarioResponsable.apellidoPaterno + " " + x.usuarioResponsable.apellidoMaterno });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult cboObraEstimados(int divisionID = -1, int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(-1, usuarioID);
                var cbo = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (divisionID == -1 && responsableID == -1 && !responsable) cbo.Add(new Core.DTO.Principal.Generales.ComboDTO { Value = "S/A", Text = "SIN AREA CUENTA", Prefijo = "-1" });
                //var auxCbo = rentabilidadFS.getListaCCByUsuario(usuarioID, 0).ToList();
                //var detallesDiv = rentabilidadFS.getACDivision(divisionID);

                var auxCbo = cxcInterfaz.FillComboCCGestion(divisionID)["items"] as List<ComboDTO>;

                //var detallesDiv = cxcInterfaz.GetDivisionDetByDivision(divisionID);
                var detallesResp = rentabilidadFS.getACResponsable(responsableID);
                //if (divisionID != -1) auxCbo = auxCbo.Where(x => detallesDiv.Contains(x.Value)).ToList();
                if (responsableID != -1) auxCbo = auxCbo.Where(x => detallesResp.Contains(x.Value)).ToList();
                foreach (var item in auxCbo)
                {
                    item.Value = item.Text.Split('-')[0].Split(' ')[0];
                }
                cbo.AddRange(auxCbo.OrderBy(e => e.Value));

                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTION DE COBRANZA
        public ActionResult GetConvenios(tblCXC_Convenios objFiltro)
        {
            return Json(cxcInterfaz.GetConvenios(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAcuerdoById(int idAcuerdo)
        {
            return Json(cxcInterfaz.GetAcuerdoById(idAcuerdo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAcuerdoByFactura(string idFactura)
        {
            return Json(cxcInterfaz.GetAcuerdoByFactura(idFactura));
        }

        public ActionResult RemoveAcuerdoDet(int idAcuerdoDet)
        {
            return Json(cxcInterfaz.RemoveAcuerdoDet(idAcuerdoDet), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarConvenios(CuentasPorCobrarDTO objConvenio)
        {
            return Json(cxcInterfaz.CrearEditarConvenios(objConvenio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarConvenio(int idConvenio)
        {
            return Json(cxcInterfaz.EliminarConvenio(idConvenio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoFacturaById(string idFactura)
        {
            return Json(cxcInterfaz.GetInfoFacturaById(idFactura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFacturasByCliente(int idCliente)
        {
            return Json(cxcInterfaz.GetFacturasByCliente(idCliente), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAutorizantesCC(string cc)
        {
            return Json(cxcInterfaz.GetAutorizantesCC(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboPeriodos()
        {
            return Json(cxcInterfaz.FillComboPeriodos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCC()
        {
            return Json(cxcInterfaz.FillComboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarEstatusConvenio(int idConvenio, EstatusConvenioEnum estatus)
        {
            return Json(cxcInterfaz.ActualizarEstatusConvenio(idConvenio, estatus), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarCorte(DateTime fechaCorte, List<string> lstFacturas)
        {
            return Json(cxcInterfaz.CrearEditarCorte(fechaCorte, lstFacturas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFactura(string idFactura, string comentarioRemove)
        {
            return Json(cxcInterfaz.RemoveFactura(idFactura, comentarioRemove), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarComentarios(cxcComentariosDTO objFiltro)
        {
            return Json(cxcInterfaz.CrearEditarComentarios(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComentarios(cxcComentariosDTO objFiltro)
        {
            return Json(cxcInterfaz.GetComentarios(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComentariosVencer()
        {
            return Json(cxcInterfaz.GetComentariosVencer(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTiposComentarios()
        {
            return Json(cxcInterfaz.FillComboTiposComentarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKardex(List<string> lstFiltroCC)
        {
            var json = Json(cxcInterfaz.GetKardex(lstFiltroCC), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GetKardexDet(string cc)
        {
            var json = Json(cxcInterfaz.GetKardexDet(cc), JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult VerificarCXC(DateTime fechaInicial, DateTime fechaFinal)
        {
            return Json(cxcInterfaz.VerificarCXC(fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCXC(List<EstClieFacturaDTO> lstFacturas, DateTime fechaInicial, DateTime fechaFinal)
        {
            return Json(cxcInterfaz.GuardarCXC(lstFacturas, fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarCXC(DateTime fechaInicial)
        {
            return Json(cxcInterfaz.CancelarCXC(fechaInicial), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarFacturaMod(string factura, DateTime fechaVencimientoOG, DateTime fechaNueva)
        {
            return Json(cxcInterfaz.GuardarFacturaMod(factura, fechaVencimientoOG, fechaNueva), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFacturaMod(string factura)
        {
            return Json(cxcInterfaz.EliminarFacturaMod(factura), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}