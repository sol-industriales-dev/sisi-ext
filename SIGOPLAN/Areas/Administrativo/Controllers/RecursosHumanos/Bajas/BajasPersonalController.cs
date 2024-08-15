using Core.DAO.RecursosHumanos.Bajas;
using Core.DAO.RecursosHumanos.Reclutamientos;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.Reclutamientos;
using Data.Factory.RecursosHumanos;
using DotnetDaddy.DocumentViewer;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Bajas
{
    public class BajasPersonalController : BaseController
    {
        private IBajasPersonalDAO r_BajasPersonalService;
        public IReclutamientosDAO reclutamientoFS;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            r_BajasPersonalService = new BajasPersonalFactoryServices().GetBajasPersonalService();
            reclutamientoFS = new ReclutamientosFactoryServices().getReclutamientosService();
            base.OnActionExecuting(filterContext);
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

        #region RETURN VISTAS
        public ActionResult Index()
        {
            ViewBag.UsuarioDeConsulta = r_BajasPersonalService.UsuarioDeConsulta();

            ViewBag.UsuarioDeConsultaTeorico = reclutamientoFS.UsuarioDeConsultaTeorico();
            ViewBag.UsuarioPermisoEditarDiasBajas = r_BajasPersonalService.GetPuedeEditarDiasDisponibles();
            ViewBag.UsuarioNotificar = r_BajasPersonalService.UsuarioNotificar();

            var diasDisponibles = r_BajasPersonalService.GetDiasDisponiblesParaLimitarFecha()[ITEMS] as DiasDisponiblesIngresosDTO;
            ViewBag.fechaMinima = diasDisponibles.anterior;
            ViewBag.fechaMaxima = diasDisponibles.posterior;
            ViewBag.empresaActual = vSesiones.sesionEmpresaActual;
            ViewBag.usuarioLiberarNominas = r_BajasPersonalService.UsuarioLiberarNominas();

            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Autorizacion()
        {
            ViewBag.empresaActual = vSesiones.sesionEmpresaActual;
            return View();
        }
        #endregion

        #region CRUD BAJAS
        public ActionResult GetBajasPersonal(List<string> listaCC, int contratable, int prioridad, DateTime? fechaInicio, DateTime? fechaFin,
            int? clave_empleado, string nombre_empleado, DateTime? fechaContaInicio, DateTime? fechaContaFin, string anticipada)
        {
            //json.MaxJsonLength = Int32.MaxValue;
            var json = Json(r_BajasPersonalService.GetBajasPersonal(listaCC, contratable, prioridad, fechaInicio, fechaFin, clave_empleado, nombre_empleado, fechaContaInicio, fechaContaFin, anticipada), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult CrearEditarBajaPersonal(BajaPersonalDTO objDTO)
        {
            return Json(r_BajasPersonalService.CrearEditarBajaPersonal(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarBajaPersonal(int idBaja)
        {
            return Json(r_BajasPersonalService.EliminarBajaPersonal(idBaja), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarBajaPersonal(int idBaja)
        {
            return Json(r_BajasPersonalService.GetDatosActualizarBajaPersonal(idBaja), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosPersona(int claveEmpleado, string nombre)
        {
            return Json(r_BajasPersonalService.GetDatosPersona(claveEmpleado, nombre), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDatosPersonaReporte(int claveEmpleado, string nombre)
        {
            return Json(r_BajasPersonalService.GetDatosPersonaReporte(claveEmpleado, nombre));
        }

        public ActionResult FillCboPreguntas(int idPregunta)
        {
            return Json(r_BajasPersonalService.FillCboPreguntas(idPregunta), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetEmpleadoCursosActos(int claveEmpleado)
        {
            return Json(r_BajasPersonalService.GetEmpleadoCursosActos(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFacultamientosAutorizante(string cc)
        {
            return Json(r_BajasPersonalService.GetFacultamientosAutorizante(cc), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FILL COMBOS
        public ActionResult GetCCs()
        {
            return Json(r_BajasPersonalService.GetCCs(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstados()
        {
            return Json(r_BajasPersonalService.FillCboEstados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMunicipios(int idEstado)
        {
            return Json(r_BajasPersonalService.FillCboMunicipios(idEstado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstadosEK()
        {
            return Json(r_BajasPersonalService.FillCboEstadosEK(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMunicipiosEK(int idEstado)
        {
            return Json(r_BajasPersonalService.FillCboMunicipiosEK(idEstado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstadosCiviles()
        {
            return Json(r_BajasPersonalService.FillCboEstadosCiviles(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEscolaridades()
        {
            return Json(r_BajasPersonalService.FillCboEscolaridades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCCByBajas()
        {
            return Json(r_BajasPersonalService.FillCboCCByBajas(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FUNCIONES GENERALES

        public ActionResult EnviarCorreo(string email, string link, int id)
        {
            return Json(r_BajasPersonalService.EnviarCorreo(email, link, id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoFiniquito(int idBaja, HttpPostedFileBase archivo, int tipoFiniquito, decimal monto)
        {
            return Json(r_BajasPersonalService.GuardarArchivoFiniquito(idBaja, archivo, tipoFiniquito, monto), JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarArchivoFiniquito()
        {
            try
            {
                int baja_id = Convert.ToInt32(Request.QueryString["baja_id"]);
                int tipoFiniquito = Convert.ToInt32(Request.QueryString["tipoFiniquito"]);
                //var lstBajas = r_BajasPersonalService.GetBajasPersonal(null, 0, 0, null, null)["lstBajas"] as List<BajaPersonalDTO>;
                //BajaPersonalDTO archivo = lstBajas.FirstOrDefault(e => e.id == baja_id);

                var objBaja = r_BajasPersonalService.GetDatosActualizarBajaPersonal(baja_id)["objBaja"] as BajaPersonalDTO;
                string rutaFiniquito = "";

                switch (tipoFiniquito)
                {
                    case 0:
                        rutaFiniquito = objBaja.rutaFiniquito;
#if DEBUG
                        string nombreDocumento = "07-07-2022 18-36-03 CH 0063315 CHENO MADRID HORACIO.pdf";
                        rutaFiniquito = string.Format("C:\\Proyecto\\SIGOPLAN\\CAPITAL_HUMANO\\BAJAS\\FINIQUITOS\\{0}", nombreDocumento);
                        //\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\BAJAS\FINIQUITOS\07-07-2022 18-36-03 CH 0063315 CHENO MADRID HORACIO.pdf
#endif
                        break;

                    case 1:
                        rutaFiniquito = objBaja.rutaRecalc;

                        break;

                    case 2:
                        rutaFiniquito = objBaja.rutaCierre;

                        break;
                    case 3:
                        rutaFiniquito = objBaja.rutaIMS;

                        break;

                    default:
                        break;
                }

                return File(rutaFiniquito, "multipart/form-data", Path.GetFileName(rutaFiniquito));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult VisualizarDocumento(int idBaja, int tipoFiniquito)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            resultado = r_BajasPersonalService.VisualizarDocumento(idBaja, tipoFiniquito);
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

        public ActionResult GetAutorizantes(string cc, int? clave_empleado, string nombre_empleado)
        {
            return Json(r_BajasPersonalService.GetAutorizantes(cc, clave_empleado, nombre_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificarBajas(List<int> lstClavesEmps)
        {
            return Json(r_BajasPersonalService.NotificarBajas(lstClavesEmps), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD

        public ActionResult getMesdeBaja(List<int> año, string idCC)
        {
            return Json(r_BajasPersonalService.getMesdeBaja(año, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMotivoSeparacion(List<int> año, bool filterData, string idCC)
        {
            return Json(r_BajasPersonalService.getMotivoSeparacion(año, filterData, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistorialCC(int cvEmpleado)
        {
            return Json(r_BajasPersonalService.GetHistorialCC(cvEmpleado),JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetalleAut(int id, int tipo)
        {
            return Json(r_BajasPersonalService.GetDetalleAut(id,tipo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AUTORIZACIÓN
        public ActionResult FillCboCCByBajasPermiso()
        {
            return Json(r_BajasPersonalService.FillCboCCByBajasPermiso(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBajasPersonalAutorizacion(List<string> listaCC)
        {
            var json = Json(r_BajasPersonalService.GetBajasPersonalAutorizacion(listaCC), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GuardarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            return Json(r_BajasPersonalService.GuardarAutorizacionBajas(objAutorizacion), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CRUD BAJAS (VER BAJA DE RECLUTAMIENTOS EN EL MODULOD DE BAJAS)
        public ActionResult GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado)
        {
            //return Json(r_BajasPersonalService.GetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado), JsonRequestBehavior.AllowGet);
            Dictionary<string, object> objResult = new Dictionary<string, object>();
            try
            {
                resultado.Clear();
                var resultDatos = r_BajasPersonalService.GetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado);
                if (resultDatos.Count() > 0)
                {
                    resultado.Add("lstDatos", resultDatos["lstDatos"]);
                    resultado.Add("lstGenerales", resultDatos["lstGenerales"]);
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

        public ActionResult GetFamiliares(int clave_empleado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FamiliaresDTO> lstFamiliaresEK = r_BajasPersonalService.GetFamiliares(clave_empleado);
                result.Add("lstFamiliaresEK", lstFamiliaresEK);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUniformes(int claveEmpleado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                UniformesDTO lstUniformes = r_BajasPersonalService.GetUniformes(claveEmpleado);
                result.Add("lstUniformes", lstUniformes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContratos(int clave_empleado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaContratosEK = r_BajasPersonalService.GetContratos(clave_empleado);
                result.Add("data", listaContratosEK);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivoExamenMedico(int claveEmpleado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = r_BajasPersonalService.GetArchivoExamenMedico(claveEmpleado);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladores(TabuladoresDTO objTabDTO)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<TabuladoresDTO> lstNomina = r_BajasPersonalService.GetTabuladores(objTabDTO);
                result.Add("lstNomina", lstNomina);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRegistroPatronal(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = r_BajasPersonalService.FillComboRegistroPatronal(cc);
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboDuracionContrato()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = r_BajasPersonalService.FillComboDuracionContrato();
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDepartamentos(string cc)
        {
            return Json(r_BajasPersonalService.FillDepartamentos(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocs(int? clave_empleado, int? id_candidato)
        {
            return Json(r_BajasPersonalService.GetDocs(clave_empleado, id_candidato), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region COMENTARIOS/CANCELACION DE BAJA

        public ActionResult CancelarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            return Json(r_BajasPersonalService.CancelarAutorizacionBajas(objAutorizacion), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DIAS PARA BAJAS
        public JsonResult GetDiasDisponibles()
        {
            return Json(r_BajasPersonalService.GetDiasDisponibles(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetDiasDisponibles(int anteriores, int posteriores)
        {
            return Json(r_BajasPersonalService.SetDiasDisponibles(anteriores, posteriores));
        }
        #endregion

        #region LIBERAR NOMINA
        public ActionResult SetNominaLiberada(int idBaja, string comentario)
        {
            return Json(r_BajasPersonalService.SetNominaLiberada(idBaja, comentario), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}