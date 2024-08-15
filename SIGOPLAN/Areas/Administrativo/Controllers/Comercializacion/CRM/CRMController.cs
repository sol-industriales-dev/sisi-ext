using Core.DAO.Comercializacion.CRM;
using Core.DTO.Administracion.Comercializacion.CRM;
using Core.DTO.Principal.Generales;
using Core.Enum.Administracion.Comercializacion.CRM;
using Data.Factory.Administracion.Comercilizacion.CRM;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Comercializacion.CRM
{
    public class CRMController : BaseController
    {
        #region INIT
        public ICRMDAO _FS_CRM;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _FS_CRM = new CRMFactoryService().GetCRMService();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region VISTAS
        public ActionResult Proyectos()
        {
            return View();
        }

        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult ProspectosClientes()
        {
            return View();
        }

        public ActionResult Canales()
        {
            return View();
        }

        public ActionResult TrackingVentas()
        {
            return View();
        }

        public ActionResult UsuariosCRM()
        {
            return View();
        }
        #endregion

        #region USUARIOS CRM
        public ActionResult GetUsuariosCRM(UsuarioCRMDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetUsuariosCRM(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearUsuarioCRM(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarUsuarioCRM(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PROYECTOS
        public ActionResult GetProyectos(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetProyectos(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearProyecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearProyecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarProyecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarProyecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarProyecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarProyecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboClientes()
        {
            return Json(_FS_CRM.FillCboClientes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPrioridades()
        {
            return Json(_FS_CRM.FillCboPrioridades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPrioridadesEstatus(PrioridadEstatusDTO objParamsDTO)
        {
            return Json(_FS_CRM.FillCboPrioridadesEstatus(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEscenarios()
        {
            return Json(_FS_CRM.FillCboEscenarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboResponsables()
        {
            return Json(_FS_CRM.FillCboResponsables(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoFiltros()
        {
            return Json(_FS_CRM.FillCboTipoFiltros(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoBusqueda(int tipoFiltroEnum)
        {
            return Json(_FS_CRM.FillCboTipoBusqueda(tipoFiltroEnum), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEtapas_ProyectoProspeccion()
        {
            return Json(_FS_CRM.FillCboEtapas_ProyectoProspeccion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEtapas_ProyectoLaborVenta()
        {
            return Json(_FS_CRM.FillCboEtapas_ProyectoLaborVenta(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEtapas_ProyectoCotizacion()
        {
            return Json(_FS_CRM.FillCboEtapas_ProyectoCotizacion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEtapas_ProyectoNegociacion()
        {
            return Json(_FS_CRM.FillCboEtapas_ProyectoNegociacion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEtapas_ProyectoCierre()
        {
            return Json(_FS_CRM.FillCboEtapas_ProyectoCierre(), JsonRequestBehavior.AllowGet);
        }

        #region RESUMEN PROYECTO
        public ActionResult GetResumenProyecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetResumenProyecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region COMENTARIOS PROYECTO
        public ActionResult GetResumenComentarios(ComentarioProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetResumenComentarios(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearComentario(ComentarioProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearComentario(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarResumenComentario(ComentarioProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarResumenComentario(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ACCIONES PROYECTO
        public ActionResult GetResumenAcciones(ProximaAccionDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetResumenAcciones(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAccion(ProximaAccionDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearAccion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarResumenAccion(ProximaAccionDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarResumenAccion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalizarAccionPrincipal(ProximaAccionDTO objParamsDTO)
        {
            return Json(_FS_CRM.FinalizarAccionPrincipal(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        #region ACCIONES DETALLE PROYECTO
        public ActionResult GetResumenDetalleAccion(ProximaAccionDetalleDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetResumenDetalleAccion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearAccionDetalle(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarResumenAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarResumenAccionDetalle(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalizarAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return Json(_FS_CRM.FinalizarAccionDetalle(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region COTIZACIONES
        public ActionResult GetCotizaciones(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetCotizaciones(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearCotizacion(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearCotizacion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarCotizacion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCotizacion(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarCotizacion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarCotizacion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarUltimaCotizacion(CotizacionDTO objParamsDTO)
        {
            return Json(_FS_CRM.VerificarUltimaCotizacion(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region CLIENTES
        public ActionResult GetClientes(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetClientes(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearCliente(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarCliente(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCliente(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCliente(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltro_Clientes_Divisiones()
        {
            return Json(_FS_CRM.FillCboFiltro_Clientes_Divisiones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarClienteHistorial(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.EnviarClienteHistorial(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivarCliente(ClienteDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActivarCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        #region CONTACTOS CLIENTES
        public ActionResult GetContactos(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetContactos(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearContacto(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearContacto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarContacto(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarContacto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarContacto(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarContacto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarContacto(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarContacto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarContactoHistorial(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EnviarContactoHistorial(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivarContacto(ContactoDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActivarContacto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region PROSPECTOS CLIENTES
        public ActionResult GetProspectosClientes(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetProspectosClientes(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearProspectoCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarProspectoCliente(objParamsDTO), JsonRequestBehavior.AllowGet); 
        }

        public ActionResult EliminarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarProspectoCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarProspectoCliente(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltro_Prospectos_Divisiones()
        {
            return Json(_FS_CRM.FillCboFiltro_Prospectos_Divisiones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltro_Prospectos_Clientes()
        {
            return Json(_FS_CRM.FillCboFiltro_Prospectos_Clientes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarProspectoHistorial(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.EnviarProspectoHistorial(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActivarProspecto(ProyectoDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActivarProspecto(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarProspectosProyecto(List<ProyectoDTO> lstProspectosDTO)
        {
            return Json(_FS_CRM.EnviarProspectosProyecto(lstProspectosDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CANALES
        public ActionResult GetCanales(CanalDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetCanales(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearCanal(CanalDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearCanal(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarCanal(CanalDTO objParamsDTO)
        {
            return Json(_FS_CRM.ActualizarCanal(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCanal(CanalDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarCanal(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCanal(CanalDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetDatosActualizarCanal(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        #region CANALES DIVISIONES
        public ActionResult GetCanalesDivisiones(CanalDivisionDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetCanalesDivisiones(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            return Json(_FS_CRM.CrearCanalDivision(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            return Json(_FS_CRM.EliminarCanalDivision(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region TRACKING VENTAS
        public ActionResult GetTrackingVentas(TrackingVentaDTO objParamsDTO)
        {
            return Json(_FS_CRM.GetTrackingVentas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERAL
        public ActionResult FillCboPaises()
        {
            return Json(_FS_CRM.FillCboPaises(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstados(int FK_Pais)
        {
            return Json(_FS_CRM.FillCboEstados(FK_Pais), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMunicipios(int FK_Estado)
        {
            return Json(_FS_CRM.FillCboMunicipios(FK_Estado), JsonRequestBehavior.AllowGet); 
        }

        public ActionResult FillCboDivisiones()
        {
            return Json(_FS_CRM.FillCboDivisiones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoClientes()
        {
            return Json(_FS_CRM.FillCboTipoClientes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboRiesgos()
        {
            return Json(_FS_CRM.FillCboRiesgos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCanales()
        {
            return Json(_FS_CRM.FillCboCanales(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboHistorial()
        {
            return Json(_FS_CRM.FillCboHistorial(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(_FS_CRM.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMenus()
        {
            return Json(_FS_CRM.FillCboMenus(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}