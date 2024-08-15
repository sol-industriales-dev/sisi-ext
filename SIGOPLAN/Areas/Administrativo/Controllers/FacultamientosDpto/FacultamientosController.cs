using Core.DAO.Administracion.FacultamientosDpto;
using Core.DTO.Administracion.Facultamiento;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Enum.Administracion.Facultamiento;
using Core.Enum.Principal.Bitacoras;
using Data.Factory.Administracion.FacultamientsDpto;
using Data.Factory.Principal.Bitacora;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Facultamiento
{
    public class FacultamientosController : BaseController
    {
        IFacultamientosDAO facultamientoService;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            facultamientoService = new FacultamientosFactoryServices().getFacultamientosService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        #region metodos catalogo
        [HttpPost]
        public ActionResult LlenarComboDepartamentos()
        {
            result.Clear();
            List<ComboDTO> listaUsuarios;
            listaUsuarios = facultamientoService.ObtenerDepartamentos();
            if (listaUsuarios.Count > 0)
            {
                result.Add(ITEMS, listaUsuarios);
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarPlantilla(string titulo, List<int> listaDepartamentos, List<ConceptoDTO> listaConceptos)
        {
            result = facultamientoService.GuardarPlantilla(titulo, listaDepartamentos, listaConceptos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizarPlantilla(string nuevoTitulo, List<int> nuevosDepartamentos, List<ConceptoDTO> nuevosConceptos, int plantillaID, bool esActualizar)
        {
            result = facultamientoService.ActualizarPlantilla(nuevoTitulo, nuevosDepartamentos, nuevosConceptos, plantillaID, esActualizar);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerCatalogo(int departamentoID)
        {
            result = facultamientoService.ObtenerCatalogo(departamentoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerPlantilla(int plantillaID)
        {
            result = facultamientoService.ObtenerPlantilla(plantillaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region metodos asignacion
        [HttpGet]
        public ActionResult ObtenerPaquetes(int departamentoID, int obraID, int estado)
        {
            result = facultamientoService.ObtenerPaquetes(departamentoID, obraID, estado);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LlenarComboCC()
        {
            result.Clear();
            List<ComboDTO> listaCC;
            listaCC = facultamientoService.ObtenerCentrosCostos();
            if (listaCC.Count > 0)
            {
                result.Add(ITEMS, listaCC);
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LlenarComboObras()
        {
            result.Clear();
            List<ComboDTO> listaObras;
            listaObras = facultamientoService.ObtenerObras();
            if (listaObras.Count > 0)
            {
                result.Add(ITEMS, listaObras);
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LlenarComboEstadoPaquetes()
        {
            result.Clear();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<EstadoPaqueteFaEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, new List<ComboDTO>());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarPlantillasCC(int centroCostosID)
        {
            result = facultamientoService.CargarPlantillasCC(centroCostosID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AsignarFacultamientos(int centroCostosID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            result = facultamientoService.AsignarFacultamientos(centroCostosID, listaFacultamientos, listaAutorizantes, todoCompleto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerPaqueteActualizar(int paqueteID)
        {
            result = facultamientoService.ObtenerPaqueteActualizar(paqueteID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizarFacultamientos(int paqueteID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            result = facultamientoService.ActualizarFacultamientos(paqueteID, listaFacultamientos, listaAutorizantes, todoCompleto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region metodos autorizacion
        [HttpGet]
        public ActionResult ObtenerPaquetesPorAutorizar()
        {
            result = facultamientoService.ObtenerPaquetesPorAutorizar();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerAutorizantes(int paqueteID)
        {
            result = facultamientoService.ObtenerAutorizantes(paqueteID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AutorizarPaquete(int paqueteID)
        {
            result = facultamientoService.AutorizarPaquete(paqueteID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RechazarPaquete(int paqueteID, string comentario)
        {
            result = facultamientoService.RechazarPaquete(paqueteID, comentario);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarCorreoAutorizacion(int paqueteID, int ordenVoBo)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }
            result = facultamientoService.EnviarCorreoAutorizacion(paqueteID, ordenVoBo, pdf);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarCorreoAutorizacionCompleta(int paqueteID)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }
            result = facultamientoService.EnviarCorreoAutorizacionCompleta(paqueteID, pdf);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarCorreoRechazo(int paqueteID, string comentario)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }
            result = facultamientoService.EnviarCorreoRechazo(paqueteID, comentario, pdf);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region metodos historico
        [HttpGet]
        public ActionResult ObtenerHistorico(int ccID)
        {
            result = facultamientoService.ObtenerHistorico(ccID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region metodos por empleado
        [HttpGet]
        public ActionResult ObtenerFacultamientosEmpleado(int claveEmpleado, int centroCostosID)
        {
            result = facultamientoService.ObtenerFacultamientosEmpleado(claveEmpleado, centroCostosID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Metodos Catalogo Grupos
        public ActionResult getTblCC(int grupoID)
        {
            result = facultamientoService.ObtenerCCGrupo(grupoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarCCGrupo(int ccID,int? grupoID)
        {
            result = facultamientoService.GuardarCCGrupo(ccID,grupoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTblGrupo()
        {
            result = facultamientoService.getTblGrupo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult delGrupo(int id)
        {
            result = facultamientoService.delGrupo(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarGrupo(string grupo)
        {
            result = facultamientoService.GuardarGrupo(grupo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult delPuesto(int id)
        {
            result = facultamientoService.delPuesto(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}