using Core.DTO;
using Core.DTO.Administracion.Seguridad.Evaluacion;
using Core.Entity.Administrativo.Seguridad.Evaluacion;
using Core.Enum.Administracion.Seguridad.Evaluacion;
using Core.Service.Administracion.Seguridad;
using Data.Factory.Administracion.Seguridad.Evaluacion;
using Data.Factory.Principal.Usuarios;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class EvaluacionController : BaseController
    {
        private EvaluacionFactoryService EvaluacionServices;
        private UsuarioFactoryServices usuarioFactoryServices;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            EvaluacionServices = new EvaluacionFactoryService();
            usuarioFactoryServices = new UsuarioFactoryServices();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        #region Vistas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Actividades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult Puestos(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult Empleados(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult Captura(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult Evaluacion(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult Dashboard(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult AgendaActividades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        #endregion

        #region Catálogos
        public ActionResult GetActividades()
        {
            result = EvaluacionServices.GetEvaluacionService().getActividades();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaActividad(tblSED_Actividad actividad)
        {
            result = EvaluacionServices.GetEvaluacionService().guardarNuevaActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarActividad(tblSED_Actividad actividad)
        {
            result = EvaluacionServices.GetEvaluacionService().editarActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarActividad(tblSED_Actividad actividad)
        {
            result = EvaluacionServices.GetEvaluacionService().eliminarActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestos()
        {
            result = EvaluacionServices.GetEvaluacionService().getPuestos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriasCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<CategoriaPuestoEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesCombo()
        {
            var data = EvaluacionServices.GetEvaluacionService().getActividades();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriodicidadCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<PeriodicidadEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesPuesto(int puestoID)
        {
            result = EvaluacionServices.GetEvaluacionService().getActividadesPuesto(puestoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            result = EvaluacionServices.GetEvaluacionService().guardarNuevoPuesto(puesto, actividades);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            result = EvaluacionServices.GetEvaluacionService().editarPuesto(puesto, actividades);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPuesto(tblSED_Puesto puesto)
        {
            result = EvaluacionServices.GetEvaluacionService().eliminarPuesto(puesto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleados()
        {
            result = EvaluacionServices.GetEvaluacionService().getEmpleados();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluadores()
        {
            result = EvaluacionServices.GetEvaluacionService().getEvaluadores();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluadoresEmpleado(int empleadoID)
        {
            result = EvaluacionServices.GetEvaluacionService().getEvaluadoresEmpleado(empleadoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestosCombo()
        {
            var data = EvaluacionServices.GetEvaluacionService().getPuestos();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            result = EvaluacionServices.GetEvaluacionService().guardarNuevoEmpleado(empleado, evaluadores);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            result = EvaluacionServices.GetEvaluacionService().editarEmpleado(empleado, evaluadores);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEmpleado(tblSED_Empleado empleado)
        {
            result = EvaluacionServices.GetEvaluacionService().eliminarEmpleado(empleado);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetEvaluadoresCombo()
        {
            var data = EvaluacionServices.GetEvaluacionService().getEvaluadores();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRolesCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<RolEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadoPorClave(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            result = EvaluacionServices.GetEvaluacionService().getEmpleadoPorClave(claveEmpleado, esContratista, idEmpresaContratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesCapturaCombo()
        {
            var data = EvaluacionServices.GetEvaluacionService().getActividadesCapturaCombo();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCaptura(EvaluacionDTO captura)
        {
            result = EvaluacionServices.GetEvaluacionService().guardarCaptura(captura);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturasEmpleado(DateTime fechaInicio, DateTime fechaFin, int evaluadorID, int estatus)
        {
            result = EvaluacionServices.GetEvaluacionService().getCapturasEmpleado(fechaInicio, fechaFin, evaluadorID, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult CargarDatosArchivoEvidencia(int capturaID)
        {
            var resultado = new Dictionary<string, object>();

            resultado = EvaluacionServices.GetEvaluacionService().cargarDatosArchivoEvidencia(capturaID);

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

        public ActionResult DescargarArchivoEvidencia(int capturaID)
        {
            var resultadoTupla = EvaluacionServices.GetEvaluacionService().descargarArchivoEvidencia(capturaID);

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
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        public ActionResult GetEmpleadosCombo()
        {
            var data = EvaluacionServices.GetEvaluacionService().getEmpleados();
            result.Add(ITEMS, data["dataComboEmpleados"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturasEvaluador(int empleadoID, int estatus)
        {
            result = EvaluacionServices.GetEvaluacionService().getCapturasEvaluador(empleadoID, estatus);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GuardarEvaluacion(EvaluacionDTO evaluacion)
        {
            result = EvaluacionServices.GetEvaluacionService().guardarEvaluacion(evaluacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCc()
        {
            var data = EvaluacionServices.GetEvaluacionService().FillComboCc();
            result.Add(ITEMS, data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboard(DateTime mes, int idEmpresa, int idAgrupador, List<int> categorias, int evaluadorID)
        {
            result = EvaluacionServices.GetEvaluacionService().cargarDashboard(mes, idEmpresa, idAgrupador, categorias, evaluadorID);

            Session["dashboardEvaluacionDesempeño"] = result["data"];
            Session["dataActividadesActuales"] = result["dataActividadesActuales"];
            Session["centroCostoDesc"] = result["centroCostoDesc"];

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAgendaActividades(DateTime mes)
        {
            result = EvaluacionServices.GetEvaluacionService().getAgendaActividades(mes);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}