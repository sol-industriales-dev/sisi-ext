using Core.DAO.GestorCorporativo;
using Core.Enum.GestorCorporativo;
using Data.Factory.GestorCorporativo;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.GestorCorporativo.Controllers
{
    public class GestorCorporativoController : BaseController
    {

        IGestorCorporativoDAO gestorCorporativoService;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        // GET: GestorCorporativo/GestorCorporativo
        public ActionResult Index()
        {
            return View();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado.Clear();
            gestorCorporativoService = new GestorCorporativoFactoryService().getDirectorioService();
            base.OnActionExecuting(filterContext);
        }

        [HttpGet]
        public ActionResult VerificarAccesoGestor()
        {
            return Json(gestorCorporativoService.VerificarAccesoGestor(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEstructuraDirectorios()
        {
            return Json(gestorCorporativoService.ObtenerEstructuraDirectorios(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase archivo, long padreID)
        {
            return Json(gestorCorporativoService.SubirArchivo(archivo, padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarArchivo(long source)
        {
            return Json(gestorCorporativoService.EliminarArchivo(source), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpeta(string nombreCarpeta, long padreID)
        {
            return Json(gestorCorporativoService.CrearCarpeta(nombreCarpeta, padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpetaSesion(GrupoCarpetaEnum grupoCarpeta)
        {
            return Json(gestorCorporativoService.CrearCarpetaSesion(grupoCarpeta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RenombrarArchivo(long source, string target)
        {
            return Json(gestorCorporativoService.RenombrarArchivo(target, source), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DescargarArchivo(long source)
        {
            resultado = gestorCorporativoService.DescargarArchivo(source);
            if (Convert.ToBoolean(resultado["success"]))
            {
                Stream fileStream = resultado["archivo"] as Stream;
                string nombreDescarga = resultado["nombreDescarga"].ToString();
                FileStreamResult fileStreamResult =
                    new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(nombreDescarga))
                    {
                        FileDownloadName = nombreDescarga
                    };
                return fileStreamResult;
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult DescargarCarpeta(long source)
        {
            resultado = gestorCorporativoService.DescargarCarpeta(source);
            if (Convert.ToBoolean(resultado["success"]))
            {
                string rutaDescarga = resultado["rutaDescarga"].ToString();
                string nombreDescarga = resultado["nombreDescarga"].ToString();
                return new FilePathResult(rutaDescarga, MimeMapping.GetMimeMapping(nombreDescarga))
                {
                    FileDownloadName = nombreDescarga
                };
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}