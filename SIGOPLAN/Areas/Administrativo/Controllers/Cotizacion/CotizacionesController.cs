using Core.DTO;
using Core.DTO.Administracion.Cotizaciones;
using Core.Entity.Administrativo.cotizaciones;
using Core.Enum.Administracion.Cotizaciones;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Administracion.Cotizaciones;
using Infrastructure.Utils;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Cotizacion
{
    public class CotizacionesController : BaseController
    {
        // GET: Administrativo/Cotizaciones
        CotizacionFactoryServices cotizacionFactoryServices;


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cotizacionFactoryServices = new CotizacionFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult CapturaCotizacion()
        {
            return View();
        }
        #region Cotizacion
        public ActionResult obtenerCotizacion(string folio, string cc, string cliente, string proyecto, int estatus, DateTime fechaI, DateTime fechaF)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var centroCostos = GetListaCentroCostos();
                var ListaCentroCostos = centroCostos.Select(c => c.cc).ToList();
                var obj = new CotizacionDTO();
                obj.folio = folio;
                obj.cc = cc;
                obj.cliente = cliente;
                obj.proyecto = proyecto;
                obj.vEstatus = estatus;

                var objDB = cotizacionFactoryServices.
                            getCotizacionService().
                            obtenerCotizacion(obj, fechaI, fechaF).                         
                            //Where(c => ListaCentroCostos.Contains(c.cc));
                            Where(c => cc == "" ? true : c.cc == cc);
               

                var data = new List<dynamic>();
                var ud = new UsuarioDAO();
                var isSoloconsulta = ud.getViewAction(vSesiones.sesionCurrentView, "SoloConsulta");
                foreach (var x in objDB)
                {
                    string btnActivo = "";

                    if (x.vEstatus == 5)
                    {
                        btnActivo = "<button class='btn btn-xs hide' title='Editar registro' onclick='clickActualizar(" + x.id + ",\"" + x.folio + "\",\"" + x.cliente.Replace("\n", "") + "\",\"" + x.proyecto.Replace("\n", "") + "\"," + x.vMonto + "," + "\"" + x.Margen + ",'" + x.fechaStatus + "'," + x.vEstatus + ",\"" + x.cc + "\" ," + (x.noRevision) + "," + x.tipoMoneda + "," + x.fechaProbableF + "," + x.contacto + ")'><i class='glyphicon glyphicon-edit'></<i></button>";
                        ;
                    }
                    else
                    {
                        btnActivo = "<button class='btn btn-xs' title='Editar registro' onclick='clickActualizar(" + x.id + ",\"" + x.folio + "\",\"" + x.cliente.Replace("\n", "") + "\",\"" + x.proyecto.Replace("\n", "") + "\"," + x.vMonto + "," + "\"" + x.Margen + "\",\"" + x.fechaStatus + "\"," + x.vEstatus + ",\"" + x.cc + "\" ," + (x.noRevision) + "," + x.tipoMoneda + ",\"" + x.fechaProbableF + "\",\"" + x.contacto + "\")'><i class='glyphicon glyphicon-edit'></<i></button>";
                        
                    }
                    data.Add(
                        new
                        {
                            id = x.id,
                            folio = x.folio,
                            cc = x.cc,
                            cliente = x.cliente,
                            proyecto = x.proyecto,
                            monto = x.monto,
                            Margen = x.Margen.ToString() + "%",
                            fechaStatus = x.fechaStatus,
                            estatus = x.estatus,
                            fechaProbableF = x.fechaProbableF,
                            contacto = x.contacto,
                            tipoMoneda = EnumExtensions.GetDescription((TipoMonedaEnum)x.tipoMoneda),
                            btnSeguimiento = "<button class='btn btn-xs' title='Seguimiento' onclick='clickComentarior(" + x.id + ")'><i class='glyphicon glyphicon-comment'></<i></button>",
                            btnEditar = btnActivo
                        }
                    );
                }
                result.Add("dataMain", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarCotizacion(tblAD_Cotizaciones obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                cotizacionFactoryServices.getCotizacionService().guardarCotizacion(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarCotizacion(List<int> lista)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cotizacionFactoryServices.getCotizacionService().eliminarCotizacion(lista);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarComentario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = JsonConvert.DeserializeObject<tblAD_CotizacionComentariosDTO>(Request.Form["obj"]);
                HttpPostedFileBase file = Request.Files["fupAdjunto"];
                var objS = new tblAD_CotizacionComentarios();
                objS.id = obj.id;
                objS.cotizacionID = obj.cotizacionID;
                objS.comentario = obj.comentario;
                objS.usuarioNombre = vSesiones.sesionUsuarioDTO.nombre;
                objS.usuarioID = vSesiones.sesionUsuarioDTO.id;
                objS.fecha = DateTime.Now;
                objS.tipo = obj.tipo;
                objS.adjuntoNombre = obj.adjuntoNombre;
                var data = cotizacionFactoryServices.getCotizacionService().guardarComentario(objS, file);
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

        public ActionResult GetFolioCotizacion(string CC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = cotizacionFactoryServices.getCotizacionService().getFolioCotizaciones(CC);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerComentarios(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = cotizacionFactoryServices.getCotizacionService().obtenerComentarios(id);
                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getComentarioArchivoAdjunto()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var o = cotizacionFactoryServices.getCotizacionService().getComentarioByID(id);
            return File(o.adjunto, "multipart/form-data", o.adjuntoNombre);
        }

        public ActionResult FillCboStatus()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<CotizacionEnum>());
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
    }
}