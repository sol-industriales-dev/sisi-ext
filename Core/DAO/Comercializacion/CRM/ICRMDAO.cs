using Core.DTO.Administracion.Comercializacion.CRM;
using Core.DTO.Principal.Generales;
using Core.Enum.Administracion.Comercializacion.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Comercializacion.CRM
{
    public interface ICRMDAO
    {
        #region USUARIOS CRM
        Dictionary<string, object> GetUsuariosCRM(UsuarioCRMDTO objParamsDTO);

        Dictionary<string, object> CrearUsuarioCRM(UsuarioCRMDTO objParamsDTO);

        Dictionary<string, object> EliminarUsuarioCRM(UsuarioCRMDTO objParamsDTO);
        #endregion

        #region PROYECTOS
        Dictionary<string, object> GetProyectos(ProyectoDTO objParamsDTO);

        Dictionary<string, object> CrearProyecto(ProyectoDTO objParamsDTO);

        Dictionary<string, object> ActualizarProyecto(ProyectoDTO objParamsDTO);

        Dictionary<string, object> EliminarProyecto(ProyectoDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarProyecto(ProyectoDTO objParamsDTO);

        Dictionary<string, object> FillCboClientes();

        Dictionary<string, object> FillCboPrioridades();

        Dictionary<string, object> FillCboPrioridadesEstatus(PrioridadEstatusDTO objParamsDTO);

        Dictionary<string, object> FillCboEscenarios();

        Dictionary<string, object> FillCboResponsables();

        Dictionary<string, object> FillCboTipoFiltros();

        Dictionary<string, object> FillCboTipoBusqueda(int tipoFiltroEnum);

        Dictionary<string, object> FillCboEtapas_ProyectoProspeccion();

        Dictionary<string, object> FillCboEtapas_ProyectoLaborVenta();

        Dictionary<string, object> FillCboEtapas_ProyectoCotizacion();

        Dictionary<string, object> FillCboEtapas_ProyectoNegociacion();

        Dictionary<string, object> FillCboEtapas_ProyectoCierre();

        #region RESUMEN PROYECTO
        Dictionary<string, object> GetResumenProyecto(ProyectoDTO objParamsDTO);
        #endregion

        #region COMENTARIOS PROYECTO
        Dictionary<string, object> GetResumenComentarios(ComentarioProyectoDTO objParamsDTO);

        Dictionary<string, object> CrearComentario(ComentarioProyectoDTO objParamsDTO);

        Dictionary<string, object> EliminarResumenComentario(ComentarioProyectoDTO objParamsDTO);
        #endregion

        #region ACCIONES PROYECTO
        Dictionary<string, object> GetResumenAcciones(ProximaAccionDTO objParamsDTO);

        Dictionary<string, object> CrearAccion(ProximaAccionDTO objParamsDTO);

        Dictionary<string, object> EliminarResumenAccion(ProximaAccionDTO objParamsDTO);

        Dictionary<string, object> FinalizarAccionPrincipal(ProximaAccionDTO objParamsDTO);

        #region ACCIONES DETALLE PROYECTO
        Dictionary<string, object> GetResumenDetalleAccion(ProximaAccionDetalleDTO objParamsDTO);

        Dictionary<string, object> CrearAccionDetalle(ProximaAccionDetalleDTO objParamsDTO);

        Dictionary<string, object> EliminarResumenAccionDetalle(ProximaAccionDetalleDTO objParamsDTO);

        Dictionary<string, object> FinalizarAccionDetalle(ProximaAccionDetalleDTO objParamsDTO);
        #endregion
        #endregion

        #region COTIZACIONES
        Dictionary<string, object> GetCotizaciones(CotizacionDTO objParamsDTO);

        Dictionary<string, object> CrearCotizacion(CotizacionDTO objParamsDTO);

        Dictionary<string, object> ActualizarCotizacion(CotizacionDTO objParamsDTO);

        Dictionary<string, object> EliminarCotizacion(CotizacionDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarCotizacion(CotizacionDTO objParamsDTO);

        Dictionary<string, object> VerificarUltimaCotizacion(CotizacionDTO objParamsDTO);
        #endregion
        #endregion

        #region CLIENTES
        Dictionary<string, object> GetClientes(ClienteDTO objParamsDTO);

        Dictionary<string, object> CrearCliente(ClienteDTO objParamsDTO);

        Dictionary<string, object> ActualizarCliente(ClienteDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarCliente(ClienteDTO objParamsDTO);

        Dictionary<string, object> EliminarCliente(ClienteDTO objParamsDTO);

        Dictionary<string, object> FillCboFiltro_Clientes_Divisiones();

        Dictionary<string, object> EnviarClienteHistorial(ClienteDTO objParamsDTO);

        Dictionary<string, object> ActivarCliente(ClienteDTO objParamsDTO);

        #region CONTACTOS CLIENTES
        Dictionary<string, object> GetContactos(ContactoDTO objParamsDTO);

        Dictionary<string, object> CrearContacto(ContactoDTO objParamsDTO);

        Dictionary<string, object> ActualizarContacto(ContactoDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarContacto(ContactoDTO objParamsDTO);

        Dictionary<string, object> EliminarContacto(ContactoDTO objParamsDTO);

        Dictionary<string, object> EnviarContactoHistorial(ContactoDTO objParamsDTO);

        Dictionary<string, object> ActivarContacto(ContactoDTO objParamsDTO);
        #endregion
        #endregion

        #region PROSPECTOS CLIENTES
        Dictionary<string, object> GetProspectosClientes(ProyectoDTO objParamsDTO);

        Dictionary<string, object> CrearProspectoCliente(ProyectoDTO objParamsDTO);

        Dictionary<string, object> ActualizarProspectoCliente(ProyectoDTO objParamsDTO);

        Dictionary<string, object> EliminarProspectoCliente(ProyectoDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarProspectoCliente(ProyectoDTO objParamsDTO);

        Dictionary<string, object> FillCboFiltro_Prospectos_Divisiones();

        Dictionary<string, object> FillCboFiltro_Prospectos_Clientes();

        Dictionary<string, object> EnviarProspectoHistorial(ProyectoDTO objParamsDTO);

        Dictionary<string, object> ActivarProspecto(ProyectoDTO objParamsDTO);

        Dictionary<string, object> EnviarProspectosProyecto(List<ProyectoDTO> lstProspectosDTO);
        #endregion

        #region CANALES
        Dictionary<string, object> GetCanales(CanalDTO objParamsDTO);

        Dictionary<string, object> CrearCanal(CanalDTO objParamsDTO);

        Dictionary<string, object> ActualizarCanal(CanalDTO objParamsDTO);

        Dictionary<string, object> EliminarCanal(CanalDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarCanal(CanalDTO objParamsDTO);

        #region CANALES DIVISIONES
        Dictionary<string, object> GetCanalesDivisiones(CanalDivisionDTO objParamsDTO);

        Dictionary<string, object> CrearCanalDivision(CanalDivisionDTO objParamsDTO);

        Dictionary<string, object> EliminarCanalDivision(CanalDivisionDTO objParamsDTO);
        #endregion
        #endregion

        #region TRACKING VENTAS
        Dictionary<string, object> GetTrackingVentas(TrackingVentaDTO objParamsDTO);
        #endregion

        #region GENERAL
        Dictionary<string, object> FillCboPaises();

        Dictionary<string, object> FillCboEstados(int FK_Pais);

        Dictionary<string, object> FillCboMunicipios(int FK_Estado);

        Dictionary<string, object> FillCboDivisiones();

        Dictionary<string, object> FillCboTipoClientes();

        Dictionary<string, object> FillCboRiesgos();

        Dictionary<string, object> FillCboCanales();

        Dictionary<string, object> FillCboHistorial();

        Dictionary<string, object> FillCboUsuarios();

        Dictionary<string, object> FillCboMenus();
        #endregion
    }
}