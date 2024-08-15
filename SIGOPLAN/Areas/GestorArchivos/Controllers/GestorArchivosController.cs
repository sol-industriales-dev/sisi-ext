using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Data.Factory.GestorArchivos;
using Core.DTO.GestorArchivos;
using SIGOPLAN.Controllers;
using Data.Factory.RecursosHumanos.ReportesRH;
using Core.DTO.Principal.Generales;
using System.IO;
using Core.DTO;
using Newtonsoft.Json;


namespace SIGOPLAN.Areas.GestorArchivos.Controllers
{
    public class GestorArchivosController : BaseController
    {

        private GestorArchivosFactoryServices GestorArchivosFactoryServices;
        private ReportesRHFactoryServices reportesRHFactoryServices;
        private int usuarioIDPRUEBA = 1;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GestorArchivosFactoryServices = new GestorArchivosFactoryServices();
            reportesRHFactoryServices = new ReportesRHFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: GestorArchivos/GestorArchivos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Permisos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerificarAccesoPrincipal()
        {
            bool ok = GestorArchivosFactoryServices.GetGestorCorporativoService().VerificarAccesoPrincipal(ObtenerUsuarioLogueado());
            return Json(ok, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarDirectorios()
        {

            DirectorioDTO estructuraDirectorios = new DirectorioDTO();
            try
            {
                estructuraDirectorios = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerEstructuraDirectorios(ObtenerUsuarioLogueado());
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Json(estructuraDirectorios, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarEstructuraPermisos(int userID)
        {

            List<EstructuraVistasDTO> estructuraPermisos = new List<EstructuraVistasDTO>();
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                estructuraPermisos.Add(GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerEstructuraPermisos(userID, ObtenerUsuarioLogueado()));

                result.Add("menuCompleto", estructuraPermisos);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarVistasAcciones(List<EstructuraVistasDTO> carpetas, int usuarioID)
        {
            bool resultadoVistas = GestorArchivosFactoryServices.GetGestorCorporativoService().GuardarVistasAccionesUsuario(carpetas, usuarioID, ObtenerUsuarioLogueado());
            return Json(resultadoVistas, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult LlenarComboUsuarios(int departamentoID)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            List<ComboDTO> listaUsuarios;

            try
            {
                listaUsuarios = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerUsuariosPorDepartamento(ObtenerUsuarioLogueado(), departamentoID);
                result.Add(ITEMS, listaUsuarios);
                result.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LlenarComboDepartamentos()
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            List<ComboDTO> listaUsuarios;

            try
            {
                listaUsuarios = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerDepartamentos(ObtenerUsuarioLogueado());
                result.Add(ITEMS, listaUsuarios);
                result.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerHistorialVeriones()
        {

            int archivoID;
            List<DirectorioDTO> listaVersiones;

            try
            {
                archivoID = Convert.ToInt32(Request.Params["id"].ToString());
                listaVersiones = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerHistorialVersiones(archivoID);
            }

            catch (Exception e)
            {
                listaVersiones = new List<DirectorioDTO>();
                Console.WriteLine(e.Message);
            }

            return Json(listaVersiones, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubirArchivo(int folderID)
        {
            List<DirectorioDTO> listaDTOs;

            try
            {
                List<HttpPostedFileBase> listaArchivos = new List<HttpPostedFileBase>();


                if ((null != Session["listaArchivosFolder"]) && (null != folderID))
                {
                    listaArchivos = Session["listaArchivosFolder"] as List<HttpPostedFileBase>;
                    listaDTOs = GestorArchivosFactoryServices.GetGestorCorporativoService().SubirArchivo(listaArchivos, folderID, ObtenerUsuarioLogueado(), obtenerEmpresaSistema());
                }
                else
                {
                    listaDTOs = new List<DirectorioDTO>();
                    return Json(listaDTOs, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                listaDTOs = new List<DirectorioDTO>();
                return Json(listaDTOs, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                Session["listaArchivosFolder"] = null;
            }

            return Json(listaDTOs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubirFolder(int folderID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if ((null != folderID) && (null != Session["estructuraFolder"]) && (null != Session["listaArchivosFolder"]))
                {
                    DirectorioDTO directorio = Session["estructuraFolder"] as DirectorioDTO;
                    List<HttpPostedFileBase> listaArchivos = Session["listaArchivosFolder"] as List<HttpPostedFileBase>;
                    result = GestorArchivosFactoryServices.GetGestorCorporativoService().SubirFolder(directorio, listaArchivos, folderID, ObtenerUsuarioLogueado(), obtenerEmpresaSistema());
                }
            }
            catch (Exception e)
            {
                result.Add("exito", false);
                result.Add("error", e.Message);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                Session["estructuraFolder"] = null;
                Session["listaArchivosFolder"] = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubirArchivosFolder()
        {
            var result = new Dictionary<string, object>();
            var listaArchivos = new List<HttpPostedFile>();
            var listaTemp = new List<HttpPostedFileBase>();
            if (null == Session["listaArchivosFolder"])
            {
                Session["listaArchivosFolder"] = new List<HttpPostedFileBase>();
                listaTemp = Session["listaArchivosFolder"] as List<HttpPostedFileBase>;
            }
            else
            {
                listaTemp = Session["listaArchivosFolder"] as List<HttpPostedFileBase>;
            }
            try
            {

                HttpPostedFileBase archivo = Request.Files["listaArchivosFolder"];
                listaTemp.Add(archivo);
                //int folderID = Convert.ToInt32(Request.Params["folderID"].ToString());
            }
            catch (Exception e)
            {
                result.Add("exito", false);
                result.Add("error", e.Message);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubirEstructuraFolder(DirectorioDTO carpeta)
        {
            if (carpeta.data.Count > 0)
            {
                Session["estructuraFolder"] = carpeta;
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActualizarArchivo()
        {
            int archivoID;
            HttpPostedFileBase archivo;
            DirectorioDTO resultado;
            try
            {
                archivoID = Convert.ToInt32(Request.Params["archivoID"].ToString());
                archivo = Request.Files["archivo"];
                resultado = GestorArchivosFactoryServices.GetGestorCorporativoService().ActualizarArchivo(archivo, archivoID, ObtenerUsuarioLogueado(), obtenerEmpresaSistema());
            }

            catch (System.Exception)
            {
                resultado = new DirectorioDTO();
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult DescargarArchivo()
        {

            string ruta;
            string nombreArchivo;
            try
            {
                string action = Request.Params["action"].ToString();
                int archivoID = Convert.ToInt32(Request.Params["source"].ToString());
                bool esVersion = Equals("downloadVersion", action);
                ruta = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerRutaArchivo(archivoID, esVersion, obtenerEmpresaSistema());
            }

            catch (System.Exception)
            {
                ruta = "";
                nombreArchivo = "";
            }
            nombreArchivo = Path.GetFileName(ruta);

            return File(ruta, MimeMapping.GetMimeMapping(nombreArchivo), nombreArchivo);
        }

        [HttpPost]
        public ActionResult BorrarArchivo()
        {
            int archivoID;
            bool ok;
            try
            {
                archivoID = Convert.ToInt32(Request.Params["source"]);
                ok = GestorArchivosFactoryServices.GetGestorCorporativoService().EliminarArchivo(archivoID, obtenerEmpresaSistema());
            }

            catch (System.Exception)
            {
                ok = false;
            }

            return Json(ok, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RenombrarArchivo()
        {

            int archivoID;
            string nuevoNombre;
            string nombre;
            DirectorioDTO directorio = new DirectorioDTO();

            try
            {
                archivoID = Convert.ToInt32(Request.Params["source"]);
                nuevoNombre = Request.Params["target"].ToString();
                nombre = GestorArchivosFactoryServices.GetGestorCorporativoService().RenombrarArchivo(nuevoNombre, archivoID, obtenerEmpresaSistema());
                directorio.id = archivoID;
                directorio.value = nombre;
            }

            catch (System.Exception)
            {
                directorio.value = "e_r_r_o_r";
            }

            return Json(directorio, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpeta()
        {

            var directorio = new DirectorioDTO();
            var resultado = new Dictionary<string, object>();
            try
            {
                string value = Request.Params["value"].ToString();
                int carpetaPadreID = Convert.ToInt32(Request.Params["target"].ToString());
                resultado = GestorArchivosFactoryServices.GetGestorCorporativoService().AgregarCarpeta(carpetaPadreID, ObtenerUsuarioLogueado(), getEmpresaID());
                int id = (int)resultado["id"];
                if ((int)resultado["id"] > 0)
                {
                    directorio.id = (int)resultado["id"];
                    directorio.value = (string)resultado["nombre"];
                }
                else
                {
                    directorio.id = -1;
                    directorio.value = "error";
                }
            }

            catch (System.Exception)
            {
                directorio.id = -2;
                directorio.value = "error";
            }

            return Json(directorio, JsonRequestBehavior.AllowGet);
        }

        private int ObtenerUsuarioLogueado()
        {
            return vSesiones.sesionUsuarioDTO.id;
        }

        private int obtenerEmpresaSistema()
        {
            return vSesiones.sesionEmpresaActual; //1 construplan y 2 arrendadora
        }

        [HttpPost]
        public ActionResult DescargarCarpetaComprimida()
        {
            string ruta;
            int id;
            try
            {
                id = Convert.ToInt32(Request.Params["source"].ToString());
                ruta = GestorArchivosFactoryServices.GetGestorCorporativoService().ObtenerRutaCarpetaComprimida(id, obtenerEmpresaSistema());
            }
            catch (System.Exception)
            {
                id = 0;
                ruta = "";
            }
            if (!ruta.Equals(""))
            {
                return File(ruta, System.Net.Mime.MediaTypeNames.Application.Zip, Path.GetFileName(ruta));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }

}

// #region Código cambiado / pendiente
//[HttpPost]
//public ActionResult obtenerAccionesUsuarioPorCarpeta(int carpetaID, int usuarioID)
//{

//    PermisosDTO acciones;
//    try
//    {
//        acciones = GestorArchivosFactoryServices.getDirectorioService().obtenerPermisosUsuarioPorCarpeta(carpetaID, usuarioID);

//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//        acciones = new PermisosDTO();
//    }

//    return Json(acciones, JsonRequestBehavior.AllowGet);
//}

//[HttpGet]
//public ActionResult obtenerPermisosUsuario()
//{

//    PermisosDTO permisos = new PermisosDTO();
//    int id = 1; //vSesiones.sesionUsuarioDTO.id;
//    try
//    {
//        permisos = GestorArchivosFactoryServices.getDirectorioService().obtenerPermisosUsuario(ObtenerUsuarioLogueado());
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//    }

//    return Json(permisos, JsonRequestBehavior.AllowGet);
//}
// #endregion