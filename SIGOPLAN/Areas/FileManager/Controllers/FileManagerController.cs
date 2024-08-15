using Core.DAO.FileManager;
using Core.DTO.FileManager;
using Core.Entity.FileManager;
using Data.Factory.FileManager;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System.Text;
using Core.Enum.FileManager;
using System.Linq;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.Enum;
using System.Threading.Tasks;
using System.Net.Http;

namespace SIGOPLAN.Areas.FileManager.Controllers
{
    public class FileManagerController : BaseController
    {

        IFileManagerDAO fileManagerService;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado.Clear();
            fileManagerService = new FileManagerFactoryServices().getFileManagerService();
            base.OnActionExecuting(filterContext);
        }

        // GET: FileManager/FileManager
        public ActionResult Index()
        {
            return View();
        }

        // GET: FileManager/FileManager/Permisos
        public ActionResult Permisos()
        {
            return View();
        }

        public ActionResult Envio()
        {
            return View();
        }

        #region FileManager
        [HttpGet]
        public ActionResult VerificarAccesoGestor()
        {
            return Json(fileManagerService.VerificarAccesoGestor(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult VerificarAccesoGestorDeprecado()
        {
            return Json(fileManagerService.VerificarAccesoGestorDeprecado(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult VerificarAccesoGestorHierarchy()
        {
            return Json(fileManagerService.VerificarAccesoGestorHierarchy(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult ObtenerTiposArchivos(long archivoID)
        {
            return Json(fileManagerService.ObtenerTiposArchivos(archivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerTodosTiposArchivos()
        {
            return Json(fileManagerService.ObtenerTodosTiposArchivos(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerArchivosActualizables(long padreID)
        {
            return Json(fileManagerService.ObtenerArchivosActualizables(padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirArchivo(List<HttpPostedFileBase> lstArchivos, long padreID, int tipoArchivoID)
        {
            return Json(fileManagerService.SubirArchivo(lstArchivos, padreID, tipoArchivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirVariosArchivos(List<HttpPostedFileBase> listaArchivos)
        {
            var resultado = new Dictionary<string, object>();

            Session["listaArchivos"] = null;

            if (listaArchivos.Count > 0)
            {
                Session["listaArchivos"] = listaArchivos;
                resultado.Add(SUCCESS, true);
            }
            else
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "La lista de archivos viene vacía.");
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirDatosVariosArchivos(List<ArchivoPorSubirDTO> listaDatosArchivos, long padreID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                // Checa si hay archivos en sesión

                var archivos = Session["listaArchivos"] as List<HttpPostedFileBase>;
                if (archivos.Count > 0)
                {
                    foreach (var datosArchivo in listaDatosArchivos)
                    {
                        datosArchivo.archivo = archivos.Find(x => x.FileName == datosArchivo.nombre);
                    }
                    Session["listaArchivos"] = null;
                }
                resultado = fileManagerService.SubirVariosArchivos(listaDatosArchivos, padreID);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error inesperado al intentar cargar los archivos.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizarArchivo(HttpPostedFileBase archivo, long archivoID)
        {
            return Json(fileManagerService.ActualizarArchivo(archivo, archivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEstructuraCarpeta()
        {
            long padreID = 0;
            try
            {
                padreID = Convert.ToInt64(Request.Params["parent"].ToString());
            }
            catch (Exception)
            {
                padreID = 0;
            }
            if (padreID == 0)
            {
                return Json(new DirectorioDTO { parent = "", data = new List<DirectorioDTO>() }, JsonRequestBehavior.AllowGet);
            }
            return Json(fileManagerService.ObtenerEstructuraCarpeta(padreID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerEstructuraCarpetaHierarchy(int obraCerrada = 0)
        {
            long padreID = 0;
            try
            {
                padreID = Convert.ToInt64(Request.Params["parent"].ToString());
            }
            catch (Exception)
            {
                padreID = 0;
            }
            if (padreID == 0)
            {
                return Json(new DirectorioDTO { parent = "", data = new List<DirectorioDTO>() }, JsonRequestBehavior.AllowGet);
            }
            return Json(fileManagerService.ObtenerEstructuraDirectoriosChildsHierarchy(padreID, obraCerrada), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEstructuraDirectorios()
        {
            return Json(fileManagerService.ObtenerEstructuraDirectorios(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEstructuraDirectoriosDeprecado()
        {
            return Json(fileManagerService.ObtenerEstructuraDirectoriosDeprecado(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEstructuraDirectoriosHierarchy()
        {
            //return Json(fileManagerService.ObtenerEstructuraDirectoriosHierarchy(), JsonRequestBehavior.AllowGet); // TODO
            var json = Json(fileManagerService.ObtenerEstructuraDirectoriosHierarchy(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        
        [HttpPost]
        public ActionResult EliminarArchivo(long source)
        {
            return Json(fileManagerService.EliminarArchivo(source), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpeta(string nombreCarpeta, long padreID, List<int> listaTiposArchivosID, bool considerarse = false, string abreviacion = "")
        {
            return Json(fileManagerService.CrearCarpeta(nombreCarpeta, padreID, listaTiposArchivosID, considerarse, abreviacion), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpetaContratista(long padreID, int divisionID)
        {
            // Si la división es industrial, se crea una carpeta con estructura distinta.
            if (divisionID == 2)
            {
                return Json(fileManagerService.CrearCarpetaContratistaIndustrial(padreID), JsonRequestBehavior.AllowGet);
            }

            return Json(fileManagerService.CrearCarpetaContratista(padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCarpetaEstimacion(long padreID, int divisionID)
        {
            // Si la división es industrial, se crea una carpeta con estructura distinta.
            if (divisionID == 2)
            {
                return Json(fileManagerService.CrearCarpetaEstimacionIndustrial(padreID), JsonRequestBehavior.AllowGet);
            }

            return Json(fileManagerService.CrearCarpetaEstimacion(padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RenombrarArchivo(long source, string target)
        {
            return Json(fileManagerService.RenombrarArchivo(target, source), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DescargarArchivo(string action, long source)
        {
            resultado = fileManagerService.DescargarArchivo(source, Equals("downloadVersion", action));
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

        //[HttpPost]
        public ActionResult DescargarCarpeta(long archivoID)
        {
            resultado = fileManagerService.DescargarCarpeta(archivoID);
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

        [HttpGet]
        public ActionResult ObtenerHistorialVersiones(long archivoID)
        {
            return Json(fileManagerService.ObtenerHistorialVersiones(archivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarUsuariosAsignados(long archivoID)
        {
            return Json(fileManagerService.CargarUsuariosAsignados(archivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarCambiosPermisos(List<PermisosDTO> listaUsuarios, long archivoID)
        {
            return Json(fileManagerService.GuardarCambiosPermisos(listaUsuarios, archivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearMesesSeguridad(int carpetaObraArchivoID)
        {
            return Json(fileManagerService.CrearMesesSeguridad(carpetaObraArchivoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerSubdivisiones(int divisionID)
        {
            return Json(fileManagerService.ObtenerSubdivisiones(divisionID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearSubdivision(int subdivisionID, long padreID)
        {
            return Json(fileManagerService.CrearSubdivision(subdivisionID, padreID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearEstructuraObraSubdivision(int subdivisionID, int ccID)
        {
            return Json(fileManagerService.CrearEstructuraObraSubdivision(subdivisionID, ccID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearEstructuraObraSubdivisionPorNombre(int subdivisionID, string nombre, string abreviacion)
        {
            return Json(fileManagerService.CrearEstructuraObraSubdivisionPorNombre(subdivisionID, nombre, abreviacion), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CargarDatosArchivoVisor(long archivoID)
        {
            resultado = fileManagerService.CargarDatosArchivoVisor(archivoID);

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

        #region Permisos
        [HttpGet]
        public ActionResult ObtenerUsuariosAutocompletado(string term)
        {
            return Json(fileManagerService.ObtenerUsuariosAutocompletado(term), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerDivisiones()
        {
            return Json(fileManagerService.ObtenerDivisiones(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerSubdivisionesPermiso()
        {
            return Json(fileManagerService.ObtenerSubdivisiones(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerObras()
        {
            return Json(fileManagerService.ObtenerObras(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerObrasPorDivision(List<int> listaDivisionesIDs)
        {
            return Json(fileManagerService.ObtenerObrasPorDivision(listaDivisionesIDs), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEstructuraPermisos(int userID)
        {
            return Json(fileManagerService.ObtenerEstructuraPermisos(userID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEstructuraCarpetaPermisos(int userID, long folderID)
        {
            return Json(fileManagerService.ObtenerEstructuraCarpetaPermisos(userID, folderID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarPermisos(int usuarioID, List<EstructuraVistasDTO> archivos)
        {
            return Json(fileManagerService.GuardarPermisos(usuarioID, archivos), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearEstructuraObra(int divisionID, int ccID)
        {
            return Json(fileManagerService.CrearEstructuraObra(divisionID, ccID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarPermisosEspeciales(List<tblFM_PermisoEspecial> permisosEspeciales)
        {
            return Json(fileManagerService.GuardarPermisosEspeciales(permisosEspeciales), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarPermisoEspecial(int permisoID)
        {
            return Json(fileManagerService.EliminarPermisoEspecial(permisoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerUsuariosPermisosEspeciales()
        {
            return Json(fileManagerService.ObtenerUsuariosPermisosEspeciales(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Envio
        public ActionResult CargarTblEnvios(int tipoDocumento = -1, string descripcion = "")
        {
            var result = new Dictionary<string, object>();
            var usuarioID = getUsuario().id;
            try
            {
                var documentos = fileManagerService.CargarTblEnvios(tipoDocumento, descripcion, usuarioID).Select(x => new
                {
                    descripcion = x.descripcion,
                    tipoDocumento = x.tipoDocumento,
                    tipoDocumentoDescripcion = Core.Enum.EnumHelper.GetDescription((tipoDocEnvioEnum) x.tipoDocumento),
                    documentoID = x.documentoID,
                    id = x.id,
                    usuarioID = x.usuarioID,
                    fecha = x.fecha.ToString("dd/MM/yyyy")
                }).OrderBy(x => x.tipoDocumentoDescripcion);               
                result.Add("documentos", documentos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpresaReporte(int envioID = 0) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                var envio = fileManagerService.GetEnvioByID(envioID);
                Session["downloadPDF"] = null;
                result.Add("empresa", envio.empresa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SubirArchivoAuto(long padreID, int tipoArchivoID, int envioID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                // Checa si hay archivos en sesión

                List<Byte[]> archivos = (List<Byte[]>)Session["downloadPDF"];
                resultado = fileManagerService.SubirArchivoAuto(archivos, padreID, tipoArchivoID, envioID);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error inesperado al intentar cargar los archivos.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoDocEnvio() 
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> items = new List<ComboDTO>();
                var values = Enum.GetValues(typeof(tipoDocEnvioEnum));
                foreach(var item in values)
                {
                    ComboDTO aux = new ComboDTO();
                    aux.Value = ((int)item).ToString();
                    aux.Text = EnumHelper.GetDescription((tipoDocEnvioEnum)item);
                    items.Add(aux);
                }
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEnvioDoc(int index) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = fileManagerService.EliminarEnvioDoc(index);
                result.Add("exito", exito);
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

        #region DUPLICAR CARPETAS
        public ActionResult FillCboCarpetasBases()
        {
            return Json(fileManagerService.FillCboCarpetasBases(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarCarpetaDuplicado(CarpetaBaseDTO objParamsDTO)
        {
            return Json(fileManagerService.GenerarCarpetaDuplicado(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CERRAR OBRAS
        public ActionResult FillCboCarpetasObras()
        {
            return Json(fileManagerService.FillCboCarpetasObras(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarObra(int idArchivo)
        {
            return Json(fileManagerService.CerrarObra(idArchivo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERAL
        public ActionResult GetPermisoAcciones()
        {
            return Json(fileManagerService.GetPermisoAcciones(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult copiadoBase()
        {
            return Json(fileManagerService.copiadoBase(), JsonRequestBehavior.AllowGet);
        }
    }
}