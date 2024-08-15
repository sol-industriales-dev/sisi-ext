using Core.DTO;
using Core.DTO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing; // Necesario para HttpUtility

namespace SIGOPLAN.Areas.SISTEMA.Controllers
{
    public class SistemaController : BaseController
    {
        private UsuarioFactoryServices usuarioFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index(int sistemaID = 0)
        {
            if (sistemaID > 0) 
            {
                if (Session["realizarEncuestaTop20PorCompras"] != null && (bool)Session["realizarEncuestaTop20PorCompras"])
                {
                    vSesiones.sesionSistemaActual = 8; //Encuestas
                    return RedirectToAction("dashboard", "EncuestasProveedor", new { area = "Encuestas" });
                }

                vSesiones.sesionSistemaActual = sistemaID;

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View();
        }
        public ActionResult _mdlAceptaRechaza()
        {
            return View();
        }

        // GET: SISTEMA/SISTEMAS
        public ActionResult SendReedireccion(int empresaID)
        {
            vSesiones.sesionEmpresaActual = empresaID;
            if (empresaID == 1) {
                vSesiones.sesionEmpresaActualNombre = "CONSTRUPLAN";
            }
            else if (empresaID == 2)
            {
                vSesiones.sesionEmpresaActualNombre = "ARRENDADORA";
            }
            else if (empresaID == 3)
            {
                vSesiones.sesionEmpresaActualNombre = "COLOMBIA";
            }
            else if (empresaID == 6)
            {
                vSesiones.sesionEmpresaActualNombre = "PERU";
            }
            else {
                vSesiones.sesionEmpresaActualNombre = "";
            }

            
            var result = new Dictionary<string, object>();

            try
            {

                try
                {
                    UsuarioExtDTO ObjUsuarioExt = new UsuarioExtDTO();
                    ObjUsuarioExt.id = empresaID;
                    ObjUsuarioExt.contrasena = vSesiones.sesionUsuarioDTO.contrasena;
                    ObjUsuarioExt.nombreUsuario = vSesiones.sesionUsuarioDTO.nombreUsuario;
                    ObjUsuarioExt.nombreCompleto = vSesiones.sesionUsuarioDTO.nombre;
                    ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoSGC;
                    ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.usuarioSGC;


                    string objURLSistema = "";

                    var objURL = usuarioFactoryServices.getUsuarioService().getURLEmpresa(empresaID);

                    objURLSistema = objURL.url;

                    objURLSistema += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting;
                    result.Add("objURLSistema", objURLSistema);
                    result.Add("objExt", ObjUsuarioExt);

                }
                catch (Exception)
                {
                    //result.Add(strExt, false);
                    result.Add("SUCCESS", true);

                }

            }
            catch (Exception e)
            {
                result.Add("MESSAGE", e.Message);
                result.Add("SUCCESS", false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReedireccion(int sistemaID = 0, string modulo = "")
        {
            //vSesiones.sesionEmpresaActual = ObjUsuarioExt.id;
            var result = new Dictionary<string, object>();

            //try
            //{
            //vSesiones.reset();
            UsuarioDTO usuarioDTO = usuarioFactoryServices.getUsuarioService().setCambioEmpresa(vSesiones.sesionUsuarioDTO.nombreUsuario, vSesiones.sesionUsuarioDTO.contrasena);
            if (!string.IsNullOrEmpty(usuarioDTO.id.ToString()))
            {

                vSesiones.sesionUsuarioDTO = usuarioDTO;

                result.Add("facultamientosFacturas", usuarioDTO.facultamientoFacturas);

                result.Add("sistemas", usuarioDTO.sistemas);
                result.Add("SUCCESS", true);

                if (Session["realizarEncuestaTop20PorCompras"] != null && (bool)Session["realizarEncuestaTop20PorCompras"])
                {
                    vSesiones.sesionSistemaActual = 8; //Encuestas
                    return RedirectToAction("dashboard", "EncuestasProveedor", new { area = "Encuestas" });
                }
            }
            else
            {
                result.Add("SUCCESS", false);
            }

            // Parsear la URL para separar la ruta de los parámetros de consulta
    //Uri moduloUri = new Uri("http://tempuri.org" + modulo); // Se agrega un dominio ficticio para crear un Uri válido
    //var partes = moduloUri.AbsolutePath.Trim('/').Split('/');
    //var queryParametros = HttpUtility.ParseQueryString(moduloUri.Query);

    //// Variables para almacenar los componentes de la ruta
    //string area = null, controlador, accion;

    //switch (partes.Length)
    //{
    //    case 2:
    //        // Formato esperado: "Controlador/Accion"
    //        controlador = partes[0];
    //        accion = partes[1];
    //        break;
    //    case 3:
    //        // Formato esperado: "Area/Controlador/Accion"
    //        area = partes[0];
    //        controlador = partes[1];
    //        accion = partes[2];
    //        break;
    //    default:
    //        return RedirectToAction("Index", "SISTEMA", new { sistemaID = sistemaID });
    //}

    //// Crear un diccionario para almacenar los parámetros de consulta
    //var routeValues = new RouteValueDictionary();
    //if (area != null)
    //{
    //    routeValues.Add("area", area);
    //}
    //foreach (string key in queryParametros.AllKeys)
    //{
    //    routeValues.Add(key, queryParametros[key]);
    //}

    //return RedirectToAction(accion, controlador, routeValues);

    if (!string.IsNullOrEmpty(modulo))
    {
        // Realiza la redirección a la URL especificada en 'modulo'
        return Redirect(modulo);
    }
    return RedirectToAction("Index", "Home");
}


        public ActionResult DescargarArchivosComprimidos()
        {
            var resultadoTupla = usuarioFactoryServices.getUsuarioService().DescargarArchivosComprimidos();

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View("ErrorDescarga");
            }
        }
    }
}