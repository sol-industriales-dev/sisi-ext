using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.Principal.Menus;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Menus;
using Data.Factory.Encuestas;
using Data.Factory.Maquinaria.Reporte;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Menus;
using Data.Factory.Principal.Usuarios;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using SIGOPLAN.Controllers.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

namespace SIGOPLAN.Controllers
{
    public class BaseController : Controller
    {
        #region Atributos
        private readonly bool _configuracionBD = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        public readonly string SUCCESS = "success";
        public readonly string MESSAGE = "message";
        public const string PAGE = "page";
        public const string TOTAL_PAGE = "total";
        public const string ROWS = "rows";
        public const string ITEMS = "items";
        public const string AUTORIZANTES = "autorizantes";

        //ACCIONES PERMITIDAS EN LA ENCUESTA

        private List<string> ACCIONES_LST = new List<string>() {
            "Entrevista", "CrearEditarEntrevista", "GetDatosPersona", "FillCboPreguntas", "FillCboEstados", "FillCboMunicipios", "FillCboEstadosCiviles", "FillCboEscolaridades", "GetCapturada", "GetBaja"
        };

        public const string RUTA_VISTA_ERROR_DESCARGA = "~/Views/Shared/ErrorDescarga.cshtml";

        public int currentViewID { get; set; }
        #endregion
        private readonly string OBJECT = "object";
        public struct stMethods
        {
            public string controllerName { get; set; }
            public string actionName { get; set; }
        }
        #region Factory
        AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
        UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        EncabezadoFactoryServices encabezadoFD = new EncabezadoFactoryServices();
        EncuestasProveedoresFactoryServices encuestasProvFS = new EncuestasProveedoresFactoryServices();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AmbienteEnkontrol();

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            menuFactoryServices = new MenuFactoryServices();
            if (vSesiones.sesionUsuarioDTO != null)
            {
                if (controllerName.Equals("Encuesta") && actionName.Equals("Responder"))
                {
                    var cadena = filterContext.HttpContext.Request.QueryString["blob"];

                    if (string.IsNullOrEmpty(cadena))
                    {
                        //..nada
                    }
                    else
                    {
                        var encuesta = filterContext.HttpContext.Request.QueryString["encuesta"];
                        var empresa = 1;
                        try
                        {
                            empresa = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["empresa"]);
                            if (empresa == 0)
                                empresa = 1;
                        }
                        catch (Exception e)
                        {
                            empresa = 1;
                        }
                        UsuarioController uc = new UsuarioController();
                        var cadenaArr = cadena.Split('@');
                        var usuario = Encriptacion.desencriptar(cadenaArr[0].ToString());
                        var pass = Encriptacion.desencriptar(cadenaArr[1].ToString());
                        uc.getLoginNP(usuario, pass, empresa);
                    }
                }
                else
                {
                    if (controllerName.Equals("Sistema") && actionName.Equals("GetReedireccion"))
                    {
                        var cadena = filterContext.HttpContext.Request.QueryString["blob"];
                        var empresa = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["empresa"]);
                        var vistaID = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["vistaID"]);
                        var routing = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["routing"]);
                        UsuarioController uc = new UsuarioController();
                        var cadenaArr = cadena.Split('@');
                        var usuario = Encriptacion.desencriptar(cadenaArr[0].ToString());
                        var pass = Encriptacion.desencriptar(cadenaArr[1].ToString());
                        uc.getLoginSISI(usuario, pass, empresa, routing, vistaID);
                    }
                    else if (vSesiones.sesionEmpresaActual != 0)
                    {
                        //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                        //string actionName = filterContext.ActionDescriptor.ActionName;
                        string urlPath = filterContext.HttpContext.Request.Path;
                        if (!controllerName.ToUpper().Equals("BASE"))
                        {
                            var list = GetAllRoutes();

                            var urlExist = list.Any(x => x.controllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase) && x.actionName.Equals(actionName, StringComparison.OrdinalIgnoreCase));

                            if (urlExist)
                            {
                                var dto = vSesiones.sesionUsuarioDTO;
                                var p = dto.permisos.Where(x => x.tipo == (int)TipoMenuEnum.VISTA && x.visible);
                                var url = "/" + controllerName + "/" + actionName;
                                var isView = menuFactoryServices.getMenuService().checkURLExistence(urlPath);
                                //var r = p.Any(x => x.url.Equals(url));

                                urlPath = urlPath.TrimEnd('/');
                                var r = p.FirstOrDefault(x => x.url.Equals(url) || x.url.Equals(urlPath));
                                if (r == null)
                                {
                                    if ((controllerName.ToUpper().Equals("HOME") && actionName.ToUpper().Equals("INDEX")) || !isView)
                                    {
                                        //var id = filterContext.Controller.ControllerContext.HttpContext.Request.QueryString["id"];
                                        //filterContext.Result = Redirect("/Home/Index/?id=" + id); 
                                    }
                                    else
                                    {
                                        filterContext.Result = Redirect("/Home/Index/?id=" + vSesiones.sesionSistemaActual);
                                    }
                                }
                                else
                                {
                                    vSesiones.sesionCurrentView = r.id;
                                    menuFactoryServices.getMenuService().setSistemaActual(vSesiones.sesionCurrentView);
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                if (controllerName.Equals("Encuesta") && actionName.Equals("Responder"))
                {
                    var cadena = filterContext.HttpContext.Request.QueryString["blob"];
                    var encuesta = filterContext.HttpContext.Request.QueryString["encuesta"];
                    var empresa = 1;
                    try
                    {
                        empresa = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["empresa"]);
                        if (empresa == 0)
                            empresa = 1;
                    }
                    catch (Exception e)
                    {
                        empresa = 1;
                    }
                    UsuarioController uc = new UsuarioController();
                    var cadenaArr = cadena.Split('@');
                    var usuario = Encriptacion.desencriptar(cadenaArr[0].ToString());
                    var pass = Encriptacion.desencriptar(cadenaArr[1].ToString());
                    uc.getLoginNP(usuario, pass, empresa);
                }
                else if (controllerName.Equals("Sistema") && actionName.Equals("GetReedireccion"))
                {
                    var cadena = filterContext.HttpContext.Request.QueryString["blob"];
                    var empresa = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["empresa"]);
                    var routing = Convert.ToInt32(filterContext.HttpContext.Request.QueryString["routing"]);
                    UsuarioController uc = new UsuarioController();
                    var cadenaArr = cadena.Split('@');
                    var usuario = Encriptacion.desencriptar(cadenaArr[0].ToString());
                    var pass = Encriptacion.desencriptar(cadenaArr[1].ToString());
                    uc.getLoginNPCE(usuario, pass, empresa, routing);
                }
                else if (controllerName.Equals("Base"))
                { 
                }
                //else if (controllerName.Equals("BajasPersonalEntrevista") && ACCIONES_LST.Contains(actionName))
                else if ((controllerName.Equals("BajasPersonalEntrevista") && ACCIONES_LST.Contains(actionName)) || (controllerName.Equals("BajasPersonalEntrevista") && actionName.Contains("getEmpleadosGeneral")))
                {
                }
                else if (controllerName.Equals("ProveedorCuadroComparativo") && ACCIONES_LST.Contains(actionName))
                {
                    //// SE VERIFICA SI EL PROVEEDOR CUENTA CON ACCESO A LA VISTA DE "PROVEEDOR CUADRO COMPARATIVO" PARA GENERAR SU COTIZACIÓN
                    //Data.Factory.Enkontrol.Compras.ProveedorCuadroComparativoFactoryService objProvCP = new Data.Factory.Enkontrol.Compras.ProveedorCuadroComparativoFactoryService();
                    //string hash = filterContext.HttpContext.Request.QueryString["hash"];
                    //Dictionary<string, object> resultado = objProvCP.GetProvCuadroComprativoFS().VerificarProveedorRelHash(hash);

                    //bool tieneAcceso = Convert.ToBoolean(resultado[SUCCESS]);
                    //if (tieneAcceso)
                    //{
                    //    UsuarioController uc = new UsuarioController();
                    //    //uc.getLoginNP("proveedor.cuadrocomparativo", "RL1RD2QK", 1);
                    //    uc.getLoginNP("omar.nunez", "1123419", 1);
                    //}
                }
                else
                {
                    filterContext.Result = RedirectToAction("Login", "Usuario", new { area = "" });
                }
            }
            //XmlDocument licLocal = new XmlDocument();
            //licLocal.LoadXml("<License><Owner>Doconut Trial - 30 days</Owner><Key>@KS@qYqbu65rqHLBh65ql3PI8yqTOP+/ngQMx6EuhfKfFJw=</Key><Type>@KS@rJI/TbbjwiBbsCdzKBVgYw==</Type><Domain>@KS@YKm+QFiHwaa559HiT2QOhw==</Domain><Annotation>@KS@258YH8vm61bNfDGhoN5a8Q==</Annotation><Search>@KS@5gkcYfKhXSWBoaXLI+vLPg==</Search><Updates>@KS@tJC+li52KrhKzAAMsOTZLA==</Updates><EULA>Trial version. Limited use.</EULA></License>");
            //XmlDocument licInterna = new XmlDocument();
            //licInterna.LoadXml("<License><Owner>Construplan</Owner><Key>@KS@EtJingbGSGi2UKQX7BXm3A==</Key><Type>@KS@alzHcIqJQzwHdGtYmGgYAA==</Type><Domain>@KS@wPYQrxaGPqXBrgtukaW4bg==</Domain><Annotation>@KS@LnL9n6riMIYvabYwqdPlXg==</Annotation><Search>@KS@sF60HJ9kLPgmqczxCkJJjQ==</Search><Updates>@KS@IHX/TN7hB9lSIsPMViu4mg==</Updates><EULA>Professional license. Issued to domain 10.1.0.2.</EULA></License>");
            //XmlDocument licPublica = new XmlDocument();
            //licPublica.LoadXml("<License><Owner>Construplan</Owner><Key>@KS@EtJingbGSGi2UKQX7BXm3A==</Key><Type>@KS@alzHcIqJQzwHdGtYmGgYAA==</Type><Domain>@KS@2wA8haO0J/Fn9NlqB6JAZDtDVWTUFndPivruDKhN4Vc=</Domain><Annotation>@KS@LnL9n6riMIYvabYwqdPlXg==</Annotation><Search>@KS@sF60HJ9kLPgmqczxCkJJjQ==</Search><Updates>@KS@IHX/TN7hB9lSIsPMViu4mg==</Updates><EULA>Professional license. Issued to domain construplan.com.mx.</EULA></License>");


            //if (vSesiones.sesionBestRouting == 1)
            //{
            //    DocViewer.DoconutLicense(licLocal);
            //}
            //else if (vSesiones.sesionBestRouting == 2)
            //{
            //    DocViewer.DoconutLicense(licInterna);
            //}
            //else if (vSesiones.sesionBestRouting == 3)
            //{
            //    DocViewer.DoconutLicense(licPublica);
            //}
            //else
            //{
            //    DocViewer.DoconutLicense(licLocal);
            //}
        }
        MenuFactoryServices menuFactoryServices;
        public UsuarioDTO user { get; set; }
        public UsuarioDTO getUsuario()
        {
            user = vSesiones.sesionUsuarioDTO;
            return user;
        }
        public bool isUserSistemas()
        {
            bool tiene = false;
            user = vSesiones.sesionUsuarioDTO;
            if (user.idPerfil == 1 && !string.IsNullOrEmpty(user.cc) && user.cc.Equals("012") && user.estatus)
            {
                tiene = true;
            }
            else
            {
                tiene = false;
            }

            return tiene;
        }
        public string getNombreSistemaActual()
        {
            UsuarioFactoryServices u = new UsuarioFactoryServices();
            return u.getUsuarioService().getNombreSistemaActual();
        }
        public string getMenuString()
        {
            menuFactoryServices = new MenuFactoryServices();
            var result = "";
            if (string.IsNullOrEmpty(vSesiones.sesionMenuHTML))
            {
                result = menuFactoryServices.getMenuService().getMenuTreeByCurrectSystemString();
            }
            else
            {
                result = vSesiones.sesionMenuHTML;
            }
            return result;

        }
        public string getMenuStringNew()
        {
            menuFactoryServices = new MenuFactoryServices();
            var result = "";
            if (string.IsNullOrEmpty(vSesiones.sesionMenuHTML))
            {
                result = menuFactoryServices.getMenuService().getMenuTreeByCurrectSystemStringNew();
            }
            else
            {
                result = vSesiones.sesionMenuHTML;
            }
            return result;

        }

        public string getMenuEmpresa()
        {
            string objURLSistema = "";

            int empresaID = 0;

            if (vSesiones.sesionEmpresaActual != 0)
            {
                // objURLSistema = usuarioFactoryServices.getUsuarioService().getURLEmpresa(vSesiones.sesionEmpresaActual);

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    empresaID = 2;
                }
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    empresaID = 1;
                }

                var objURL = usuarioFactoryServices.getUsuarioService().getURLEmpresa(empresaID);
                if (objURL != null)
                {
                    objURLSistema = objURL.url;

                    objURLSistema += "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.contrasena) + "&empresa=" + empresaID + "&routing=" + vSesiones.sesionBestRouting;

                }

                //return objURLSistema;

                var cambio = getEmpresaCambio();
                return cambio.Count > 0 ? "SI" : "";
            }

            return "";

        }
        public List<tblP_EmpresasDTO> getEmpresaCambio()
        {
            List<tblP_EmpresasDTO> result = new List<tblP_EmpresasDTO>();
            result = vSesiones.sesionUsuarioDTO.empresas.Where(x => x.id != vSesiones.sesionEmpresaActual).ToList();


            foreach (var i in result)
            {
                i.urlRedireccion = i.url + "/SISTEMA/Sistema/GetReedireccion/?blob=" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.nombreUsuario) + "@" + Encriptacion.encriptar(vSesiones.sesionUsuarioDTO.contrasena) + "&empresa=" + i.id + "&routing=" + vSesiones.sesionBestRouting;
            }
            return result;
        }

        public string getImagen()
        {
            string objURLImagen = "";

            int empresaID = 0;

            if (vSesiones.sesionEmpresaActual != 0)
            {

                var objURL = usuarioFactoryServices.getUsuarioService().getURLEmpresa(vSesiones.sesionEmpresaActual);

                objURLImagen = objURL.icono;

                return objURLImagen;
            }

            return "";
        }


        public string getBreadCrums()
        {
            var result = "";
            result = menuFactoryServices.getMenuService().getBreadCrums();
            return result;
        }
        public List<tblP_Sistema> getSistemas()
        {
            return getUsuario().sistemas;
        }
        public List<tblP_Alerta> getAlertasByUsuarioAndSistema(int usuarioID, int sistemaID)
        {
            return alertaFactoryServices.getAlertaService().getAlertasByUsuarioAndSistema(usuarioID, sistemaID);
        }
        public List<tblP_Alerta> getAlertasByUsuario(int usuarioID)
        {
            return alertaFactoryServices.getAlertaService().getAlertasByUsuario(usuarioID);
        }
        public ActionResult setMenu(string menu, string id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                vSesiones.sesionMenuHTML = menu;
                vSesiones.sesionCurrentMenu = id;
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
        public IEnumerable<stMethods> GetAllRoutes()
        {
            //Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            //Get all classes that inherit from the Controller class that are public and not abstract
            //Replace Controller with ApiController for Web Api
            var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Controller)) && t.IsPublic && !t.IsAbstract);
            foreach (var type in types)
            {
                //Get the Controller Name minus the word Controller
                string controllerName = type.Name.Substring(0, type.Name.IndexOf("Controller", System.StringComparison.InvariantCulture));

                //Get all Methods within the inherited class
                var methods = type.GetMethods().Where(x => x.IsPublic && x.DeclaringType.Equals(type));
                foreach (var method in methods)
                {
                    //Construct the initial url pattern which will contain the Controller and Method name.
                    //string url = string.Format("{0}/{1}", controllerName, method.Name);

                    //Create a new Dictionary and add the controller name and method name
                    //var routeDictionary = new Dictionary<string, object>();
                    //routeDictionary.Add("controller", controllerName);
                    //routeDictionary.Add("action", method.Name);

                    // Get all method parameters and add them to the dictionary
                    //var paramInfo = method.GetParameters();
                    //if (paramInfo.Count() > 0)
                    //{
                    //    foreach (var parameter in paramInfo)
                    //    {
                    //        //Append the url format with the parameter name
                    //        url += "/{" + parameter.Name + "}";

                    //        //Check if parameter is optional and add the name and value to the dictionary
                    //        if (parameter.IsOptional)
                    //            routeDictionary.Add(parameter.Name, UrlParameter.Optional);
                    //        else
                    //            routeDictionary.Add(parameter.Name, parameter.Name);
                    //    }
                    //}
                    yield return new stMethods { controllerName = controllerName, actionName = method.Name };
                }
            }
        }
        public ResolutionDTO getResolution()
        {
            return vSesiones.sesionCurrentResolution;
        }
        public ActionResult setResolutionResult(int width, int height)
        {
            var result = new Dictionary<string, object>();
            try
            {
                vSesiones.sesionCurrentResolution.width = width;
                vSesiones.sesionCurrentResolution.height = height;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setCurrentSystem(int systemID)
        {
            //tblP_Usuario objUsuario;
            vSesiones.sesionSistemaActual = systemID;
            var result = new Dictionary<string, object>();

            try
            {

                try
                {
                    UsuarioExtDTO ObjUsuarioExt = new UsuarioExtDTO();
                    var auxSistemas = usuarioFactoryServices.getUsuarioService().ListSistemasAll();
                    var Externo = auxSistemas.Where(w => w.ext == true && w.id == systemID).ToList();
                    var Virtual = auxSistemas.Where(w => w.esVirtual == true && w.id == systemID).ToList();

                    ObjUsuarioExt.id = systemID;
                    ObjUsuarioExt.contrasena = vSesiones.sesionUsuarioDTO.contrasena;
                    ObjUsuarioExt.nombreUsuario = vSesiones.sesionUsuarioDTO.nombreUsuario;
                    ObjUsuarioExt.nombreCompleto = vSesiones.sesionUsuarioDTO.nombre;
                    if (systemID == 10)
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoSeguridad;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.usuarioSeguridad;
                    }
                    else if (systemID == 18)
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoPatoos;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.externoPatoosNombre;
                    }
                    else
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoSGC;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.usuarioSGC;
                    }
                    
                    ObjUsuarioExt.sistemas = getSistemas();

                    if (Externo.Count > 0 && Externo[0].ext == true)
                    {

                        result.Add("objExt", ObjUsuarioExt);
                        //result.Add(System.Web.HttpContext.Current, true);
                        result.Add("externo", true);
                    }
                    if (Virtual.Count > 0 && Virtual[0].esVirtual == true)
                    {
                        result.Add("esVirtual", true);
                        result.Add("sistemaID", systemID);
                        var sistemas = vSesiones.sesionUsuarioDTO.sistemas;
                        var auxSistema = sistemas.FirstOrDefault(x => x.id == Virtual[0].id);
                        result.Add("auxURL", auxSistema == null ? auxSistemas[0].url : auxSistema.url);
                    }


                    if (vSesiones.sesionUsuarioDTO.esCliente == false)
                    {
                        if ((int)SistemasEnum.ALMACEN == systemID || (int)SistemasEnum.ENKONTROL == systemID)
                        {
                            var necesitaIngresarDatosEnKontrol = usuarioFactoryServices.getUsuarioService().NecesitaIngresarDatosEnKontrol();
                            if (necesitaIngresarDatosEnKontrol)
                            {
                                result.Add("necesitaIngresarDatosEnKontrol", necesitaIngresarDatosEnKontrol);
                                result.Add("formularioURL", "/Usuarios/FormularioDatosEnKontrol");
                            }
                            // Encuestas top20
                            else
                            {
                                var res_encuestas = encuestasProvFS.getEncuestasProveedoresFactoryServices().GetProveedoresTop20(vSesiones.sesionUsuarioDTO.id, 1, true);
                                if (res_encuestas.Success && res_encuestas.Value is bool)
                                {
                                    result.Add("necesitaIngresarDatosEnKontrol", (bool)res_encuestas.Value);
                                    result.Add("formularioURL", "/Encuestas/EncuestasProveedor/dashboard?realizar=true");
                                    //result.Remove("esVirtual");
                                    //result.Add("esVirtual", false);
                                    Session["realizarEncuestaTop20PorCompras"] = (bool)res_encuestas.Value;
                                }
                                else
                                {
                                    result.Add(SUCCESS, false);
                                    result.Add(MESSAGE, res_encuestas.Message);

                                    if (Session["realizarEncuestaTop20PorCompras"] != null)
                                    {
                                        Session.Remove("realizarEncuestaTop20PorCompras");
                                    }
                                }
                            }
                            //
                        }
                        else
                        {
                            if (Session["realizarEncuestaTop20PorCompras"] != null)
                            {
                                Session.Remove("realizarEncuestaTop20PorCompras");
                            }
                        }
                    }
                    
                }
                catch (Exception)
                {
                    //result.Add(strExt, false);
                    result.Add(SUCCESS, true);

                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUsuarioExt()
        {
            //tblP_Usuario objUsuario;
            var result = new Dictionary<string, object>();

            try
            {

                try
                {
                    UsuarioExtDTO ObjUsuarioExt = new UsuarioExtDTO();
                    var Externo = usuarioFactoryServices.getUsuarioService().ListSistemasAll().Where(w => w.ext == true && w.id == vSesiones.sesionSistemaActual).ToList();
                    ObjUsuarioExt.id = vSesiones.sesionSistemaActual;
                    ObjUsuarioExt.contrasena = vSesiones.sesionUsuarioDTO.contrasena;
                    ObjUsuarioExt.nombreUsuario = vSesiones.sesionUsuarioDTO.nombreUsuario;
                    ObjUsuarioExt.nombreCompleto = vSesiones.sesionUsuarioDTO.nombre;
                    if (vSesiones.sesionSistemaActual == (int)SistemasEnum.SEGURIDAD)
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoSeguridad;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.usuarioSeguridad;
                    }
                    else if (vSesiones.sesionSistemaActual == (int)SistemasEnum.PATOOS)
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoPatoos;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.externoPatoosNombre;
                    }
                    else
                    {
                        ObjUsuarioExt.tipoSGC = vSesiones.sesionUsuarioDTO.tipoSGC;
                        ObjUsuarioExt.usuarioSGC = vSesiones.sesionUsuarioDTO.usuarioSGC;
                    }

                    ObjUsuarioExt.sistemas = getSistemas();

                    result.Add("objExt", ObjUsuarioExt);

                }
                catch (Exception)
                {
                    //result.Add(strExt, false);
                    result.Add(SUCCESS, true);

                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool getAction(string accion)
        {
            bool result = false;
            result = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, accion);
            return result;
        }

        //public List<tblP_SistemaDTO> getSistemasDTO()
        //{
        //    var sistemas = menuFactoryServices.getMenuService().getSistemas();
        //    return sistemas;
        //}
        public List<tblP_CC_Usuario> GetListaCentroCostos()
        {
            var idUsuario = getUsuario().id;
            return usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);
        }
        public bool isLiberado()
        {
            menuFactoryServices = new MenuFactoryServices();
            var id = vSesiones.sesionCurrentView;
            return id == 0 ? true : menuFactoryServices.getMenuService().isLiberado(id);
        }
        public ActionResult getEmpresa()
        {
            return Json(vSesiones.sesionEmpresaActual, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getVistaActual()
        {
            return Json(vSesiones.sesionCurrentView, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getVistasExcepcionPalabraCC()
        {
            var r = menuFactoryServices.getMenuService().getVistasExcepcionPalabraCC();
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public int getEmpresaID()
        {
            return vSesiones.sesionEmpresaActual;
        }

        public int getSistemaID()
        {
            return vSesiones.sesionSistemaActual;
        }
        public string getEmpresaDescripcion()
        {
            return EnumExtensions.GetDescription((EmpresaEnum)vSesiones.sesionEmpresaActual);
        }
        public string getEmpresaActualNombre()
        {
            return vSesiones.sesionEmpresaActualNombre;
        }
        public ActionResult validateBestRouting()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setBestRouting(int routing)
        {
            var result = new Dictionary<string, object>();
            try
            {
                vSesiones.sesionBestRouting = routing;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public string getSessionMainUrl()
        {
            return vSesiones.sesionUsuarioDTO.empresas.FirstOrDefault().url;
        }
        public string getEmpresaNombre()
        {
            return encabezadoFD.getEncabezadoServices().getEncabezadoDatos().nombreEmpresa;
        }

        private void AmbienteEnkontrol()
        {
            if (_configuracionBD)
            {
                vSesiones.sesionAmbienteEnkontrolAdm = EnkontrolAmbienteEnum.Prod;
                vSesiones.sesionAmbienteEnkontrolRh = EnkontrolAmbienteEnum.Rh;
            }
            else
            {
                vSesiones.sesionAmbienteEnkontrolAdm = EnkontrolAmbienteEnum.Prueba;
                vSesiones.sesionAmbienteEnkontrolRh = EnkontrolAmbienteEnum.PruebaRh;
            }
        }

        public ActionResult ColocarVistoAlerta(int alerta_id)
        {
            return Json(alertaFactoryServices.getAlertaService().ColocarVistoAlerta(alerta_id), JsonRequestBehavior.AllowGet);
        }
    }
}