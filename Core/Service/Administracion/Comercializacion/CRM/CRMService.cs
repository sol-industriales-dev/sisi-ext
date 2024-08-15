using Core.DAO.Comercializacion.CRM;
using Core.DTO.Administracion.Comercializacion.CRM;
using Core.DTO.Principal.Generales;
using Core.Enum.Administracion.Comercializacion.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Comercializacion.CRM
{
    public class CRMService : ICRMDAO
    {
        #region INIT
        public ICRMDAO _CRM { get; set; }
        public ICRMDAO CRM
        {
            get { return _CRM; }
            set { _CRM = value; }
        }
        public CRMService(ICRMDAO CRM)
        {
            this.CRM = CRM;
        }
        #endregion

        #region USUARIOS CRM
        public Dictionary<string, object> GetUsuariosCRM(UsuarioCRMDTO objParamsDTO)
        {
            return _CRM.GetUsuariosCRM(objParamsDTO);
        }

        public Dictionary<string, object> CrearUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            return _CRM.CrearUsuarioCRM(objParamsDTO);
        }

        public Dictionary<string, object> EliminarUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            return _CRM.EliminarUsuarioCRM(objParamsDTO);
        }
        #endregion

        #region PROYECTOS
        public Dictionary<string, object> GetProyectos(ProyectoDTO objParamsDTO)
        {
            return _CRM.GetProyectos(objParamsDTO);
        }

        public Dictionary<string, object> CrearProyecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.CrearProyecto(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.ActualizarProyecto(objParamsDTO);
        }

        public Dictionary<string, object> EliminarProyecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.EliminarProyecto(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarProyecto(objParamsDTO);
        }

        public Dictionary<string, object> FillCboClientes()
        {
            return _CRM.FillCboClientes();
        }

        public Dictionary<string, object> FillCboPrioridades()
        {
            return _CRM.FillCboPrioridades();
        }

        public Dictionary<string, object> FillCboPrioridadesEstatus(PrioridadEstatusDTO objParamsDTO)
        {
            return _CRM.FillCboPrioridadesEstatus(objParamsDTO);
        }

        public Dictionary<string, object> FillCboEscenarios()
        {
            return _CRM.FillCboEscenarios();
        }

        public Dictionary<string, object> FillCboResponsables()
        {
            return _CRM.FillCboResponsables();
        }

        public Dictionary<string, object> FillCboTipoFiltros()
        {
            return _CRM.FillCboTipoFiltros();
        }

        public Dictionary<string, object> FillCboTipoBusqueda(int tipoFiltroEnum)
        {
            return _CRM.FillCboTipoBusqueda(tipoFiltroEnum);
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoProspeccion()
        {
            return _CRM.FillCboEtapas_ProyectoProspeccion();
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoLaborVenta()
        {
            return _CRM.FillCboEtapas_ProyectoLaborVenta();
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoCotizacion()
        {
            return _CRM.FillCboEtapas_ProyectoCotizacion();
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoNegociacion()
        {
            return _CRM.FillCboEtapas_ProyectoNegociacion();
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoCierre()
        {
            return _CRM.FillCboEtapas_ProyectoCierre();
        }

        #region RESUMEN PROYECTO
        public Dictionary<string, object> GetResumenProyecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.GetResumenProyecto(objParamsDTO);
        }
        #endregion

        #region COMENTARIOS PROYECTO
        public Dictionary<string, object> GetResumenComentarios(ComentarioProyectoDTO objParamsDTO)
        {
            return _CRM.GetResumenComentarios(objParamsDTO);
        }

        public Dictionary<string, object> CrearComentario(ComentarioProyectoDTO objParamsDTO)
        {
            return _CRM.CrearComentario(objParamsDTO);
        }

        public Dictionary<string, object> EliminarResumenComentario(ComentarioProyectoDTO objParamsDTO)
        {
            return _CRM.EliminarResumenComentario(objParamsDTO);
        }
        #endregion

        #region ACCIONES PROYECTO
        public Dictionary<string, object> GetResumenAcciones(ProximaAccionDTO objParamsDTO)
        {
            return _CRM.GetResumenAcciones(objParamsDTO);
        }

        public Dictionary<string, object> CrearAccion(ProximaAccionDTO objParamsDTO)
        {
            return _CRM.CrearAccion(objParamsDTO);
        }

        public Dictionary<string, object> EliminarResumenAccion(ProximaAccionDTO objParamsDTO)
        {
            return _CRM.EliminarResumenAccion(objParamsDTO);
        }

        public Dictionary<string, object> FinalizarAccionPrincipal(ProximaAccionDTO objParamsDTO)
        {
            return _CRM.FinalizarAccionPrincipal(objParamsDTO);
        }

        #region ACCIONES DETALLE PROYECTO
        public Dictionary<string, object> GetResumenDetalleAccion(ProximaAccionDetalleDTO objParamsDTO)
        {
            return _CRM.GetResumenDetalleAccion(objParamsDTO);
        }

        public Dictionary<string, object> CrearAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return _CRM.CrearAccionDetalle(objParamsDTO);
        }

        public Dictionary<string, object> EliminarResumenAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return _CRM.EliminarResumenAccionDetalle(objParamsDTO);
        }

        public Dictionary<string, object> FinalizarAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            return _CRM.FinalizarAccionDetalle(objParamsDTO);
        }
        #endregion
        #endregion

        #region COTIZACIONES
        public Dictionary<string, object> GetCotizaciones(CotizacionDTO objParamsDTO)
        {
            return _CRM.GetCotizaciones(objParamsDTO);
        }

        public Dictionary<string, object> CrearCotizacion(CotizacionDTO objParamsDTO)
        {
            return _CRM.CrearCotizacion(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            return _CRM.ActualizarCotizacion(objParamsDTO);
        }

        public Dictionary<string, object> EliminarCotizacion(CotizacionDTO objParamsDTO)
        {
            return _CRM.EliminarCotizacion(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarCotizacion(objParamsDTO);
        }

        public Dictionary<string, object> VerificarUltimaCotizacion(CotizacionDTO objParamsDTO)
        {
            return _CRM.VerificarUltimaCotizacion(objParamsDTO);
        }
        #endregion
        #endregion

        #region CLIENTES
        public Dictionary<string, object> GetClientes(ClienteDTO objParamsDTO)
        {
            return _CRM.GetClientes(objParamsDTO);
        }

        public Dictionary<string, object> CrearCliente(ClienteDTO objParamsDTO)
        {
            return _CRM.CrearCliente(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarCliente(ClienteDTO objParamsDTO)
        {
            return _CRM.ActualizarCliente(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarCliente(ClienteDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarCliente(objParamsDTO);
        }

        public Dictionary<string, object> EliminarCliente(ClienteDTO objParamsDTO)
        {
            return _CRM.EliminarCliente(objParamsDTO);
        }

        public Dictionary<string, object> FillCboFiltro_Clientes_Divisiones()
        {
            return _CRM.FillCboFiltro_Clientes_Divisiones();
        }

        public Dictionary<string, object> EnviarClienteHistorial(ClienteDTO objParamsDTO)
        {
            return _CRM.EnviarClienteHistorial(objParamsDTO);
        }

        public Dictionary<string, object> ActivarCliente(ClienteDTO objParamsDTO)
        {
            return _CRM.ActivarCliente(objParamsDTO);
        }

        #region CONTACTOS CLIENTES
        public Dictionary<string, object> GetContactos(ContactoDTO objParamsDTO)
        {
            return _CRM.GetContactos(objParamsDTO);
        }

        public Dictionary<string, object> CrearContacto(ContactoDTO objParamsDTO)
        {
            return _CRM.CrearContacto(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarContacto(ContactoDTO objParamsDTO)
        {
            return _CRM.ActualizarContacto(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarContacto(ContactoDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarContacto(objParamsDTO);
        }

        public Dictionary<string, object> EliminarContacto(ContactoDTO objParamsDTO)
        {
            return _CRM.EliminarContacto(objParamsDTO);
        }

        public Dictionary<string, object> EnviarContactoHistorial(ContactoDTO objParamsDTO)
        {
            return _CRM.EnviarContactoHistorial(objParamsDTO);
        }

        public Dictionary<string, object> ActivarContacto(ContactoDTO objParamsDTO)
        {
            return _CRM.ActivarContacto(objParamsDTO);
        }
        #endregion
        #endregion

        #region PROSPECTOS CLIENTES
        public Dictionary<string, object> GetProspectosClientes(ProyectoDTO objParamsDTO)
        {
            return _CRM.GetProspectosClientes(objParamsDTO);
        }

        public Dictionary<string, object> CrearProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return _CRM.CrearProspectoCliente(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return _CRM.ActualizarProspectoCliente(objParamsDTO);
        }

        public Dictionary<string, object> EliminarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return _CRM.EliminarProspectoCliente(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarProspectoCliente(objParamsDTO);
        }

        public Dictionary<string, object> FillCboFiltro_Prospectos_Divisiones()
        {
            return _CRM.FillCboFiltro_Prospectos_Divisiones();
        }

        public Dictionary<string, object> FillCboFiltro_Prospectos_Clientes()
        {
            return _CRM.FillCboFiltro_Prospectos_Clientes();
        }

        public Dictionary<string, object> EnviarProspectoHistorial(ProyectoDTO objParamsDTO)
        {
            return _CRM.EnviarProspectoHistorial(objParamsDTO);
        }

        public Dictionary<string, object> ActivarProspecto(ProyectoDTO objParamsDTO)
        {
            return _CRM.ActivarProspecto(objParamsDTO);
        }

        public Dictionary<string, object> EnviarProspectosProyecto(List<ProyectoDTO> lstProspectosDTO)
        {
            return _CRM.EnviarProspectosProyecto(lstProspectosDTO);
        }
        #endregion

        #region CANALES
        public Dictionary<string, object> GetCanales(CanalDTO objParamsDTO)
        {
            return _CRM.GetCanales(objParamsDTO);
        }

        public Dictionary<string, object> CrearCanal(CanalDTO objParamsDTO)
        {
            return _CRM.CrearCanal(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarCanal(CanalDTO objParamsDTO)
        {
            return _CRM.ActualizarCanal(objParamsDTO);
        }
        public Dictionary<string, object> EliminarCanal(CanalDTO objParamsDTO)
        {
            return _CRM.EliminarCanal(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarCanal(CanalDTO objParamsDTO)
        {
            return _CRM.GetDatosActualizarCanal(objParamsDTO);
        }

        #region CANALES DIVISIONES
        public Dictionary<string, object> GetCanalesDivisiones(CanalDivisionDTO objParamsDTO)
        {
            return _CRM.GetCanalesDivisiones(objParamsDTO);
        }

        public Dictionary<string, object> CrearCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            return _CRM.CrearCanalDivision(objParamsDTO);
        }

        public Dictionary<string, object> EliminarCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            return _CRM.EliminarCanalDivision(objParamsDTO);
        }
        #endregion
        #endregion

        #region TRACKING VENTAS
        public Dictionary<string, object> GetTrackingVentas(TrackingVentaDTO objParamsDTO)
        {
            return _CRM.GetTrackingVentas(objParamsDTO);
        }
        #endregion

        #region GENERAL
        public Dictionary<string, object> FillCboPaises()
        {
            return _CRM.FillCboPaises();
        }

        public Dictionary<string, object> FillCboEstados(int FK_Pais)
        {
            return _CRM.FillCboEstados(FK_Pais);
        }

        public Dictionary<string, object> FillCboMunicipios(int FK_Estado)
        {
            return _CRM.FillCboMunicipios(FK_Estado);
        }

        public Dictionary<string, object> FillCboDivisiones()
        {
            return _CRM.FillCboDivisiones();
        }

        public Dictionary<string, object> FillCboTipoClientes()
        {
            return _CRM.FillCboTipoClientes();
        }

        public Dictionary<string, object> FillCboRiesgos()
        {
            return _CRM.FillCboRiesgos();
        }

        public Dictionary<string, object> FillCboCanales()
        {
            return _CRM.FillCboCanales();
        }

        public Dictionary<string, object> FillCboHistorial()
        {
            return _CRM.FillCboHistorial();
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return _CRM.FillCboUsuarios();
        }

        public Dictionary<string, object> FillCboMenus()
        {
            return _CRM.FillCboMenus();
        }
        #endregion
    }
}