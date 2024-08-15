using Core.DAO.Comercializacion.CRM;
using Core.DTO.Administracion.Comercializacion.CRM;
using Core.Entity.Administrativo.Comercializacion.CRM;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using Infrastructure.Utils;
using System.Linq;
using Core.DTO;
using Core.DTO.Utils.Data;
using Core.Enum.Administracion.Comercializacion.CRM;
using Core.Enum;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contratistas;
using System.Threading.Tasks;
using Core.DTO.Principal.Usuarios;

namespace Data.DAO.Administracion.Comercializacion.CRM
{
    public class CRMDAO : GenericDAO<tblP_Usuario>, ICRMDAO
    {
        #region INIT
        private const string _NOMBRE_CONTROLADOR = "CRMController";
        private const int _SISTEMA = (int)SistemasEnum.ADMINISTRACION_FINANZAS;
        private const int _FK_DIVISION_SIN_DEFINIR = 1;
        private const int _FK_RIESGO_SIN_DEFINIR = 1;
        private const int _FK_CANAL_SIN_DEFINIR = 1;
        private const int _JOSE_PEDRO_GONZALEZ_ID = 1043;
        private const string _CORREO_OMAR_NUNEZ = "omar.nunez@construplan.com.mx";
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        #endregion

        #region USUARIOS CRM
        public Dictionary<string, object> GetUsuariosCRM(UsuarioCRMDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                // INIT
                List<UsuarioDTO> lstUsuariosDTO = GetListaUsuarios().ToList();
                List<UsuarioCRMDTO> lstUsuariosCRMDTO = GetListaUsuariosCRM().ToList();

                List<UsuarioCRMDTO> lstUsuariosCRMDTO_Retornar = new List<UsuarioCRMDTO>();
                UsuarioCRMDTO objUsuarioCRMDTO = new UsuarioCRMDTO();
                foreach (var item in lstUsuariosCRMDTO)
                {
                    if (!lstUsuariosCRMDTO_Retornar.Any(w => w.FK_Usuario == item.FK_Usuario))
                    {
                        UsuarioDTO objUsuarioDTO = lstUsuariosDTO.Where(w => w.id == item.FK_Usuario).FirstOrDefault();
                        if (objUsuarioDTO == null)
                            throw new Exception("Ocurrió un error al obtener el nombre del usuario.");

                        objUsuarioCRMDTO = new UsuarioCRMDTO();
                        objUsuarioCRMDTO.id = item.id;
                        objUsuarioCRMDTO.FK_Usuario = item.FK_Usuario;
                        objUsuarioCRMDTO.nombreUsuario = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno));

                        List<int> lstFK_Menu = lstUsuariosCRMDTO.Where(w => w.FK_Usuario == item.FK_Usuario).Select(s => s.FK_Menu).ToList();
                        foreach (var FK_Menu in lstFK_Menu)
                            objUsuarioCRMDTO.htmlMenus += string.Format("<button class='btn btn-xs btn-primary btnMenuDT' title='{0}'>{0}</button>&nbsp;", EnumHelper.GetDescription((MenuEnum)FK_Menu));

                        lstUsuariosCRMDTO_Retornar.Add(objUsuarioCRMDTO);
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstUsuariosCRMDTO_Retornar);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearUsuarioCRM = ValidacionesCrearUsuarioCRM(objParamsDTO);
                if (!(bool)resValidacionesCrearUsuarioCRM[SUCCESS])
                    throw new Exception((string)resValidacionesCrearUsuarioCRM[MESSAGE]);
                #endregion

                #region CREAR USUARIO CRM
                List<tblAF_CRM_UsuariosCRM> lstCrearUsuariosCRMEF = new List<tblAF_CRM_UsuariosCRM>();
                tblAF_CRM_UsuariosCRM objCrearUsuarioCRMEF = new tblAF_CRM_UsuariosCRM();
                foreach (var FK_Menu in objParamsDTO.lstFK_Menu)
                {
                    objCrearUsuarioCRMEF = new tblAF_CRM_UsuariosCRM();
                    objCrearUsuarioCRMEF.FK_Usuario = objParamsDTO.FK_Usuario;
                    objCrearUsuarioCRMEF.FK_Menu = FK_Menu;
                    objCrearUsuarioCRMEF.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objCrearUsuarioCRMEF.fechaCreacion = DateTime.Now;
                    objCrearUsuarioCRMEF.registroActivo = true;
                    lstCrearUsuariosCRMEF.Add(objCrearUsuarioCRMEF);
                }
                _context.tblAF_CRM_UsuariosCRM.AddRange(lstCrearUsuariosCRMEF);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (objParamsDTO.FK_Usuario <= 0) { throw new Exception("Es necesario seleccionar un usuario."); }
                if (objParamsDTO.lstFK_Menu.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un menú."); }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarUsuarioCRM(UsuarioCRMDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el usuario.";
                if (objParamsDTO.FK_Usuario <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR USUARIO
                List<tblAF_CRM_UsuariosCRM> lstEliminarUsuarioCRM = _context.tblAF_CRM_UsuariosCRM.Where(w => w.FK_Usuario == objParamsDTO.FK_Usuario && w.registroActivo).ToList();
                if (lstEliminarUsuarioCRM.Count() <= 0)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                foreach (var item in lstEliminarUsuarioCRM)
                {
                    item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    item.fechaModificacion = DateTime.Now;
                    item.registroActivo = false;
                }
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito el usuario.");
                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarMenuRelUsuario(UsuarioCRMDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el menú seleccionado.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR MENU REL USUARIO
                tblAF_CRM_UsuariosCRM objEliminarMenuCRM = _context.tblAF_CRM_UsuariosCRM.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objEliminarMenuCRM == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objEliminarMenuCRM.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objEliminarMenuCRM.fechaModificacion = DateTime.Now;
                objEliminarMenuCRM.registroActivo = false;
                _context.SaveChanges();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region PROYECTOS
        public Dictionary<string, object> GetProyectos(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region PROYECTOS
                string htmlProyectosProspeccion = string.Empty;
                string htmlProyectosLaborVenta = string.Empty;
                string htmlProyectosCotizacion = string.Empty;
                string htmlProyectosNegociacion = string.Empty;
                string htmlProyectosCierre = string.Empty;
                foreach (var item in GetListaProyectos(new ProyectoDTO
                {
                    FK_Cliente = objParamsDTO.FK_Cliente,
                    FK_Division = objParamsDTO.FK_Division,
                    FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable,
                    FK_Prioridad = objParamsDTO.FK_Prioridad,
                    esProspecto = false
                }).ToList())
                {
                    #region INIT INFORMACIÓN
                    ClienteDTO objClienteDTO = GetListaClientes(new ClienteDTO { id = item.FK_Cliente }).FirstOrDefault();
                    if (objClienteDTO == null)
                        throw new Exception("Ocurrió un error al obtener la información del cliente.");

                    UsuarioDTO objUsuarioDTO = GetListaUsuarios(new UsuarioDTO { id = item.FK_UsuarioResponsable }).FirstOrDefault();
                    if (objUsuarioDTO == null)
                        throw new Exception("Ocurrió un error al obtener la información del responsable.");
                    else
                    {
                        objParamsDTO.nombreCompletoResponsable = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}",
                            objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno));
                    }

                    string proximaAccion = string.Empty;
                    ProximaAccionDTO objProximaAccionDTO = GetListaProximasAcciones(new ProximaAccionDTO { id = item.id }).FirstOrDefault();
                    if (objProximaAccionDTO != null)
                        proximaAccion = objProximaAccionDTO.accion;
                    #endregion

                    #region HTML PROYECTOS
                    string htmlProyecto = string.Format(
                                @"<div class='divCuadroProyecto verde'>
                                    <b title='{0}'>Proyecto:&nbsp;{0}</b><br>
                                    <b title='{1}'>Cliente:&nbsp;{1}</b><br>
                                    <b title='{2}'>Acción:&nbsp;{2}</b><br>
                                    <b title='{3}'>Responsable:&nbsp;{3}</b><br>
                                    <b>Fecha de acción:&nbsp;{4}/{5}/{6}</b>
                                    <button class='btn btn-xs pull-right boton' data-id='{7}'><i class='fas fa-plus boton icoProyectos' data-id='{7}'></i></button>
                                </div><br>", item.nombreProyecto, objClienteDTO.nombreCliente, proximaAccion, objParamsDTO.nombreCompletoResponsable, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, item.id);

                    switch (item.FK_Prioridad)
                    {
                        case (int)PrioridadesEnum.PROSPECCION:
                            #region PROSPECCIÓN
                            if (objParamsDTO.FK_Estatus_Prospeccion > 0)
                            {
                                if (item.FK_Estatus == objParamsDTO.FK_Estatus_Prospeccion)
                                    htmlProyectosProspeccion += htmlProyecto;
                            }
                            else
                                htmlProyectosProspeccion += htmlProyecto;
                            #endregion
                            break;
                        case (int)PrioridadesEnum.LABOR_DE_VENTA:
                            #region LABOR DE VENTA
                            if (objParamsDTO.FK_Estatus_LaborVenta > 0)
                            {
                                if (item.FK_Estatus == objParamsDTO.FK_Estatus_LaborVenta)
                                    htmlProyectosLaborVenta += htmlProyecto;
                            }
                            else
                                htmlProyectosLaborVenta += htmlProyecto;
                            #endregion
                            break;
                        case (int)PrioridadesEnum.COTIZACION:
                            #region COTIZACIÓN
                            if (objParamsDTO.FK_Estatus_Cotizacion > 0)
                            {
                                if (item.FK_Estatus == objParamsDTO.FK_Estatus_Cotizacion)
                                    htmlProyectosCotizacion += htmlProyecto;
                            }
                            else
                                htmlProyectosCotizacion += htmlProyecto;
                            #endregion
                            break;
                        case (int)PrioridadesEnum.NEGOCIACION:
                            #region NEGOCIACIÓN
                            if (objParamsDTO.FK_Estatus_Negociacion > 0)
                            {
                                if (item.FK_Estatus == objParamsDTO.FK_Estatus_Negociacion)
                                    htmlProyectosNegociacion += htmlProyecto;
                            }
                            else
                                htmlProyectosNegociacion += htmlProyecto;
                            #endregion
                            break;
                        case (int)PrioridadesEnum.CIERRE:
                            #region CIERRE
                            if (objParamsDTO.FK_Estatus_Cierre > 0)
                            {
                                if (item.FK_Estatus == objParamsDTO.FK_Estatus_Cierre)
                                    htmlProyectosCierre += htmlProyecto;
                            }
                            else
                                htmlProyectosCierre += htmlProyecto;
                            #endregion
                            break;
                        default:
                            break;
                    }
                    #endregion
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add("htmlProyectosProspeccion", htmlProyectosProspeccion);
                resultado.Add("htmlProyectosLaborVenta", htmlProyectosLaborVenta);
                resultado.Add("htmlProyectosCotizacion", htmlProyectosCotizacion);
                resultado.Add("htmlProyectosNegociacion", htmlProyectosNegociacion);
                resultado.Add("htmlProyectosCierre", htmlProyectosCierre);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearProyecto(ProyectoDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    Dictionary<string, object> resValidacionesCrearEditarProyecto = ValidacionesCrearEditarProyecto(objParamsDTO);
                    if (!(bool)resValidacionesCrearEditarProyecto[SUCCESS])
                        throw new Exception((string)resValidacionesCrearEditarProyecto[MESSAGE]);
                    #endregion

                    #region CREAR PROYECTO
                    if (objParamsDTO.esCrearClienteDesdeProyectos)
                    {
                        Dictionary<string, object> resCrearCliente = CrearCliente(new ClienteDTO { esCrearClienteDesdeProyectos = true, nombreCliente = objParamsDTO.nombreCliente, FK_Division = _FK_DIVISION_SIN_DEFINIR });
                        if (!(bool)resCrearCliente[SUCCESS])
                            throw new Exception((string)resCrearCliente[MESSAGE]);
                        else
                            objParamsDTO.FK_Cliente = (int)resCrearCliente["FK_Cliente"];

                        Dictionary<string, object> resCrearContacto = CrearContacto(new ContactoDTO { esCrearClienteDesdeProyectos = true, FK_Cliente = objParamsDTO.FK_Cliente, nombreContacto = objParamsDTO.nombreContacto });
                        if (!(bool)resCrearContacto[SUCCESS])
                            throw new Exception((string)resCrearContacto[MESSAGE]);
                    }

                    tblAF_CRM_Proyectos objCrearProyecto = new tblAF_CRM_Proyectos();
                    objCrearProyecto.nombreProyecto = objParamsDTO.nombreProyecto.Trim();
                    objCrearProyecto.FK_Cliente = objParamsDTO.FK_Cliente;
                    objCrearProyecto.FK_Prioridad = objParamsDTO.FK_Prioridad;
                    objCrearProyecto.FK_Division = objParamsDTO.FK_Division;
                    objCrearProyecto.FK_Municipio = objParamsDTO.FK_Municipio;
                    objCrearProyecto.importeCotizadoAprox = objParamsDTO.importeCotizadoAprox;
                    objCrearProyecto.fechaInicio = objParamsDTO.fechaInicio;
                    objCrearProyecto.FK_Estatus = objParamsDTO.FK_Estatus;
                    objCrearProyecto.FK_Escenario = objParamsDTO.FK_Escenario;
                    objCrearProyecto.FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable;
                    objCrearProyecto.FK_Riesgo = objParamsDTO.FK_Riesgo;
                    objCrearProyecto.descripcionObra = objParamsDTO.descripcionObra.Trim();
                    objCrearProyecto.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objCrearProyecto.fechaCreacion = DateTime.Now;
                    objCrearProyecto.registroActivo = true;
                    _context.tblAF_CRM_Proyectos.Add(objCrearProyecto);
                    _context.SaveChanges();
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito");
                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> ActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objParamsDTO.id <= 0)
                        throw new Exception("Ocurrió un error al actualizar la información.");

                    Dictionary<string, object> resValidacionesCrearEditarProyecto = ValidacionesCrearEditarProyecto(objParamsDTO);
                    if (!(bool)resValidacionesCrearEditarProyecto[SUCCESS])
                        throw new Exception((string)resValidacionesCrearEditarProyecto[MESSAGE]);
                    #endregion

                    #region ACTUALIZAR PROYECTO
                    tblAF_CRM_Proyectos objActualizarProyecto = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objActualizarProyecto == null)
                        throw new Exception("Ocurrió un error al obtener la información del proyecto.");

                    if (objParamsDTO.esCrearClienteDesdeProyectos)
                    {
                        Dictionary<string, object> resCrearCliente =
                            CrearCliente(new ClienteDTO { esCrearClienteDesdeProyectos = true, nombreCliente = objParamsDTO.nombreCliente, FK_Division = _FK_DIVISION_SIN_DEFINIR });
                        if (!(bool)resCrearCliente[SUCCESS])
                            throw new Exception((string)resCrearCliente[MESSAGE]);
                        else
                            objParamsDTO.FK_Cliente = (int)resCrearCliente["FK_Cliente"];

                        Dictionary<string, object> resCrearContacto =
                            CrearContacto(new ContactoDTO { esCrearClienteDesdeProyectos = true, FK_Cliente = objParamsDTO.FK_Cliente, nombreContacto = objParamsDTO.nombreContacto });
                        if (!(bool)resCrearContacto[SUCCESS])
                            throw new Exception((string)resCrearContacto[MESSAGE]);
                    }

                    objActualizarProyecto.nombreProyecto = objParamsDTO.nombreProyecto.Trim();
                    objActualizarProyecto.FK_Cliente = objParamsDTO.FK_Cliente;
                    objActualizarProyecto.FK_Prioridad = objParamsDTO.FK_Prioridad;
                    objActualizarProyecto.FK_Division = objParamsDTO.FK_Division;
                    objActualizarProyecto.FK_Municipio = objParamsDTO.FK_Municipio;
                    objActualizarProyecto.importeCotizadoAprox = objParamsDTO.importeCotizadoAprox;
                    objActualizarProyecto.fechaInicio = objParamsDTO.fechaInicio;
                    objActualizarProyecto.FK_Estatus = objParamsDTO.FK_Estatus;
                    objActualizarProyecto.FK_Escenario = objParamsDTO.FK_Escenario;
                    objActualizarProyecto.FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable;
                    objActualizarProyecto.FK_Riesgo = objParamsDTO.FK_Riesgo;
                    objActualizarProyecto.descripcionObra = objParamsDTO.descripcionObra.Trim();
                    objActualizarProyecto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objActualizarProyecto.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        private Dictionary<string, object> ValidacionesCrearEditarProyecto(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(objParamsDTO.nombreProyecto)) { throw new Exception("Es necesario indicar el nombre del proyecto."); }

                if (objParamsDTO.esCrearClienteDesdeProyectos)
                {
                    if (string.IsNullOrEmpty(objParamsDTO.nombreCliente)) { throw new Exception("Es necesario indicar el nombre del cliente."); }
                    if (string.IsNullOrEmpty(objParamsDTO.nombreContacto)) { throw new Exception("Es necesario indicar el nombre del contacto."); }
                }
                else
                {
                    if (objParamsDTO.FK_Cliente <= 0)
                        throw new Exception("Es necesario seleccionar el cliente.");
                }

                if (objParamsDTO.FK_Prioridad <= 0) { throw new Exception("Es necesario seleccionar la prioridad."); }
                if (objParamsDTO.FK_Division <= 0) { throw new Exception("Es necesario seleccionar la división."); }
                if (objParamsDTO.FK_Municipio <= 0) { throw new Exception("Es necesario seleccionar un municipio."); }
                if (objParamsDTO.importeCotizadoAprox == 0) { throw new Exception("Es necesario indicar el importe."); }
                if (objParamsDTO.importeCotizadoAprox <= -1) { throw new Exception("Es necesario indicar el importe con valor positivo."); }
                if (objParamsDTO.fechaInicio == null) { throw new Exception("Es necesario indicar la fecha de inicio."); }
                if (objParamsDTO.FK_Estatus <= 0) { throw new Exception("Es necesario seleccionar el estatus."); }
                if (objParamsDTO.FK_Escenario <= 0) { throw new Exception("Es necesario selecionar el escenario."); }
                if (objParamsDTO.FK_UsuarioResponsable <= 0) { throw new Exception("Es necesario seleccionar al responsable."); }
                if (objParamsDTO.FK_Riesgo <= 0) { throw new Exception("Es necesario seleccionar un riesgo."); }
                if (string.IsNullOrEmpty(objParamsDTO.descripcionObra)) { throw new Exception("Es necesario indicar la descripción de la obra."); }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarProyecto(ProyectoDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el registro.";

                    #region VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    #endregion

                    #region SE ELIMINA EL PROYECTO Y SU INFO RELACIONADA
                    // PROYECTOS
                    tblAF_CRM_Proyectos objEliminarProyecto = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminarProyecto == null)
                        throw new Exception(MENSAJE_ERROR_GENERAL);

                    objEliminarProyecto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objEliminarProyecto.fechaModificacion = DateTime.Now;
                    objEliminarProyecto.registroActivo = false;
                    _context.SaveChanges();

                    // COMENTARIOS
                    Dictionary<string, object> resEliminarComentarios = EliminarResumenComentarios(new ComentarioProyectoDTO { FK_Proyecto = objParamsDTO.id });
                    if (!(bool)resEliminarComentarios[SUCCESS])
                        throw new Exception((string)resEliminarComentarios[MESSAGE]);

                    // PROXIMAS ACCIONES
                    Dictionary<string, object> resEliminarProximasAcciones = EliminarResumenAcciones(new ProximaAccionDTO { FK_Proyecto = objParamsDTO.id });
                    if (!(bool)resEliminarProximasAcciones[SUCCESS])
                        throw new Exception((string)resEliminarProximasAcciones[MESSAGE]);

                    // ELIMINAR COTIZACIONES
                    Dictionary<string, object> resEliminarResumenCotizaciones = EliminarResumenCotizaciones(new CotizacionDTO { FK_Proyecto = objParamsDTO.id });
                    if (!(bool)resEliminarResumenCotizaciones[SUCCESS])
                        throw new Exception((string)resEliminarResumenCotizaciones[MESSAGE]);
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el proyecto.");
                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> GetDatosActualizarProyecto(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información del proyecto";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region OBTENER INFORMACIÓN DEL PROYECTO
                ProyectoDTO objProyecto = GetListaProyectos(new ProyectoDTO { id = objParamsDTO.id }).FirstOrDefault();
                if (objProyecto == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                ProyectoDTO objProyectoDTO = new ProyectoDTO();
                objProyectoDTO.nombreProyecto = objProyecto.nombreProyecto;
                objProyectoDTO.FK_Cliente = objProyecto.FK_Cliente;
                objProyectoDTO.FK_Prioridad = objProyecto.FK_Prioridad;
                objProyectoDTO.FK_Division = objProyecto.FK_Division;
                objProyectoDTO.FK_Estatus = objProyecto.FK_Estatus;
                objProyectoDTO.importeCotizadoAprox = objProyecto.importeCotizadoAprox;
                objProyectoDTO.fechaInicio = objProyecto.fechaInicio;
                objProyectoDTO.FK_Escenario = objProyecto.FK_Escenario;
                objProyectoDTO.FK_Municipio = objProyecto.FK_Municipio;
                objProyectoDTO.FK_Estado = GetListaMunicipios(new MunicipioDTO { idMunicipio = objProyecto.FK_Municipio }).Select(s => s.idEstado).FirstOrDefault();
                objProyectoDTO.FK_Pais = GetListaEstados(new EstadoDTO { idEstado = objProyectoDTO.FK_Estado }).Select(s => s.idPais).FirstOrDefault();
                objProyectoDTO.FK_UsuarioResponsable = objProyecto.FK_UsuarioResponsable;
                objProyectoDTO.FK_Riesgo = objProyecto.FK_Riesgo;
                objProyectoDTO.descripcionObra = objProyecto.descripcionObra;

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objProyectoDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPrioridades()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPrioridadesCboDTO = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS VALUE, prioridad AS TEXT FROM tblAF_CRM_CatPrioridades WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                foreach (var item in lstPrioridadesCboDTO)
                    item.Text = PersonalUtilities.PrimerLetraMayuscula(item.Text);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPrioridadesCboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPrioridadesEstatus(PrioridadEstatusDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Prioridad <= 0) { throw new Exception("Ocurrió un error al obtener el listado de prioridades."); }
                #endregion

                #region LISTADO DE PRIORIDADES
                List<ComboDTO> lstPrioridadEstatus = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS VALUE, estatus AS TEXT FROM tblAF_CRM_CatPrioridadEstatus WHERE FK_Prioridad = @FK_Prioridad AND registroActivo = @registroActivo",
                    parametros = new { FK_Prioridad = objParamsDTO.FK_Prioridad, registroActivo = true }
                }).ToList();

                foreach (var item in lstPrioridadEstatus)
                    item.Text = PersonalUtilities.PrimerLetraMayuscula(item.Text);
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPrioridadEstatus);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEscenarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEscenarios = new List<ComboDTO>();
                ComboDTO objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.A).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.A));
                lstEscenarios.Add(objEscenario);

                objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.B).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.B));
                lstEscenarios.Add(objEscenario);

                objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.C).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.C));
                lstEscenarios.Add(objEscenario);

                objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.D).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.D));
                lstEscenarios.Add(objEscenario);

                objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.E).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.E));
                lstEscenarios.Add(objEscenario);

                objEscenario = new ComboDTO();
                objEscenario.Value = Convert.ToInt32(EscenariosEnum.T).ToString();
                objEscenario.Text = EnumHelper.GetDescription((EscenariosEnum.T));
                lstEscenarios.Add(objEscenario);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEscenarios);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboResponsables()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEmpleados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS VALUE, CONCAT(nombre, ' ', apellidoPaterno, ' ', apellidoMaterno) AS TEXT FROM tblP_Usuario WHERE estatus = 1 ORDER BY TEXT"
                }).ToList();

                foreach(var item in lstEmpleados)
                {
                    item.Text = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(item.Text);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEmpleados);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EliminarResumenComentarios(ComentarioProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al eliminar los comentarios del proyecto."); }
                #endregion

                #region ELIMINAR COMENTARIOS
                List<tblAF_CRM_ComentariosProyectos> lstComentarios = _context.tblAF_CRM_ComentariosProyectos.Where(w => w.FK_Proyecto == objParamsDTO.FK_Proyecto && w.registroActivo).ToList();
                if (lstComentarios.Count() > 0)
                {
                    foreach (var item in lstComentarios)
                    {
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();
                }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EliminarResumenAcciones(ProximaAccionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al eliminar las proximas acciones del proyecto."); }
                #endregion

                #region ELIMINAR ACCIONES
                List<tblAF_CRM_ProximasAcciones> lstProximasAcciones = _context.tblAF_CRM_ProximasAcciones.Where(w => w.FK_Proyecto == objParamsDTO.FK_Proyecto && w.registroActivo).ToList();
                if (lstProximasAcciones.Count() > 0)
                {
                    foreach (var item in lstProximasAcciones)
                    {
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();
                }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EliminarResumenCotizaciones(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al eliminar las cotizaciones del proyecto."); }
                #endregion

                #region ELIMINAR ACCIONES
                List<tblAF_CRM_Cotizaciones> lstCotizaciones = _context.tblAF_CRM_Cotizaciones.Where(w => w.FK_Proyecto == objParamsDTO.FK_Proyecto && w.registroActivo).ToList();
                if (lstCotizaciones.Count() > 0)
                {
                    foreach (var item in lstCotizaciones)
                    {
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();
                }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoFiltros()
        {
            resultado = new Dictionary<string, object>();
            List<ComboDTO> lstTipoFiltros = new List<ComboDTO>();
            try
            {
                ComboDTO objTipoFiltro = new ComboDTO();
                objTipoFiltro.Value = Convert.ToInt32(TipoFiltrosEnum.CLIENTE).ToString();
                objTipoFiltro.Text = EnumHelper.GetDescription((TipoFiltrosEnum.CLIENTE));
                lstTipoFiltros.Add(objTipoFiltro);

                objTipoFiltro = new ComboDTO();
                objTipoFiltro.Value = Convert.ToInt32(TipoFiltrosEnum.DIVISION).ToString();
                objTipoFiltro.Text = EnumHelper.GetDescription((TipoFiltrosEnum.DIVISION));
                lstTipoFiltros.Add(objTipoFiltro);

                objTipoFiltro = new ComboDTO();
                objTipoFiltro.Value = Convert.ToInt32(TipoFiltrosEnum.RESPONSABLE).ToString();
                objTipoFiltro.Text = EnumHelper.GetDescription((TipoFiltrosEnum.RESPONSABLE));
                lstTipoFiltros.Add(objTipoFiltro);

                objTipoFiltro = new ComboDTO();
                objTipoFiltro.Value = Convert.ToInt32(TipoFiltrosEnum.PRIORIDAD).ToString();
                objTipoFiltro.Text = EnumHelper.GetDescription((TipoFiltrosEnum.PRIORIDAD));
                lstTipoFiltros.Add(objTipoFiltro);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstTipoFiltros);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoBusqueda(int tipoFiltroEnum)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstFiltroBusqueda = new List<ComboDTO>();

                switch (tipoFiltroEnum)
                {
                    case (int)TipoFiltrosEnum.CLIENTE:
                        lstFiltroBusqueda.AddRange((List<ComboDTO>)FillCboFiltro_Proyectos_Clientes()[ITEMS]);
                        break;
                    case (int)TipoFiltrosEnum.DIVISION:
                        lstFiltroBusqueda.AddRange((List<ComboDTO>)FillCboFiltro_Clientes_Divisiones()[ITEMS]);
                        break;
                    case (int)TipoFiltrosEnum.RESPONSABLE:
                        lstFiltroBusqueda.AddRange((List<ComboDTO>)FillCboFiltro_Proyectos_Responsables()[ITEMS]);
                        break;
                    case (int)TipoFiltrosEnum.PRIORIDAD:
                        lstFiltroBusqueda.AddRange((List<ComboDTO>)FillCboFiltro_Proyectos_Prioridades()[ITEMS]);
                        break;
                    default:
                        break;
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstFiltroBusqueda);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { tipoFiltroEnum = tipoFiltroEnum });
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoProspeccion()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaPrioridadEstatus(new PrioridadEstatusDTO { FK_Prioridad = (int)PrioridadesEnum.PROSPECCION }).ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.estatus;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoLaborVenta()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaPrioridadEstatus(new PrioridadEstatusDTO { FK_Prioridad = (int)PrioridadesEnum.LABOR_DE_VENTA }).ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.estatus;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoCotizacion()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaPrioridadEstatus(new PrioridadEstatusDTO { FK_Prioridad = (int)PrioridadesEnum.COTIZACION }).ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.estatus;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoNegociacion()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaPrioridadEstatus(new PrioridadEstatusDTO { FK_Prioridad = (int)PrioridadesEnum.NEGOCIACION }).ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.estatus;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEtapas_ProyectoCierre()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaPrioridadEstatus(new PrioridadEstatusDTO { FK_Prioridad = (int)PrioridadesEnum.CIERRE }).ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.estatus;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> FillCboFiltro_Proyectos_Clientes()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstFK_ClientesUtilizados = GetListaProyectos(new ProyectoDTO { esProspecto = false }).Select(s => s.FK_Cliente).ToList();
                List<ClienteDTO> lstClientesDTO = new List<ClienteDTO>();
                if (lstFK_ClientesUtilizados.Count() > 0)
                    lstClientesDTO = GetListaClientes(new ClienteDTO { lstID_Clientes = lstFK_ClientesUtilizados }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstClientesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.nombreCliente;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> FillCboFiltro_Proyectos_Responsables()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstFK_ResponsablesUtilizados = GetListaProyectos(new ProyectoDTO { esProspecto = false }).Select(s => s.FK_UsuarioResponsable).ToList();
                List<UsuarioDTO> lstUsuariosDTO = new List<UsuarioDTO>();
                if (lstFK_ResponsablesUtilizados.Count() > 0)
                    lstUsuariosDTO = GetListaUsuarios(new UsuarioDTO { lstID_Usuarios = lstFK_ResponsablesUtilizados }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstUsuariosDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", item.nombre, item.apellidoPaterno, item.apellidoMaterno));
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> FillCboFiltro_Proyectos_Prioridades()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstFK_PrioridadesUtilizadas = GetListaProyectos(new ProyectoDTO { esProspecto = false }).Select(s => s.FK_Prioridad).ToList();
                List<PrioridadDTO> lstPrioridadesDTO = new List<PrioridadDTO>();
                if (lstFK_PrioridadesUtilizadas.Count() > 0)
                    lstPrioridadesDTO = GetListaPrioridades(new PrioridadDTO { lstID_Prioridades = lstFK_PrioridadesUtilizadas }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPrioridadesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.prioridad;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        #region RESUMEN PROYECTO
        public Dictionary<string, object> GetResumenProyecto(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al obtener el resumen del proyecto."); }
                #endregion

                #region RESUMEN DEL PROYECTO
                Dictionary<string, object> resGetResumenProyecto_Resumen = GetResumenProyecto_Resumen(objParamsDTO);
                if (!(bool)resGetResumenProyecto_Resumen[SUCCESS])
                    throw new Exception((string)resGetResumenProyecto_Resumen[MESSAGE]);

                Dictionary<string, object> resGetResumenProyecto_UltimoComentario = GetResumenProyecto_UltimoComentario(objParamsDTO);
                if (!(bool)resGetResumenProyecto_UltimoComentario[SUCCESS])
                    throw new Exception((string)resGetResumenProyecto_UltimoComentario[MESSAGE]);

                Dictionary<string, object> resGetResumenProyecto_ProximaAccion = GetResumenProyecto_ProximaAccion(objParamsDTO);
                if (!(bool)resGetResumenProyecto_ProximaAccion[SUCCESS])
                    throw new Exception((string)resGetResumenProyecto_ProximaAccion[MESSAGE]);
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add("objProyectoDTO", (ProyectoDTO)resGetResumenProyecto_Resumen[ITEMS]);
                resultado.Add("objUltimoComentarioDTO", (ComentarioProyectoDTO)resGetResumenProyecto_UltimoComentario[ITEMS]);
                resultado.Add("objProximaAccionDTO", (ProximaAccionDTO)resGetResumenProyecto_ProximaAccion[ITEMS]);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> GetResumenProyecto_Resumen(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información del proyecto.";

                #region INIT INFORMACIÓN
                ProyectoDTO objProyectoDTO = GetListaProyectos(new ProyectoDTO { id = objParamsDTO.id }).FirstOrDefault();
                if (objProyectoDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                ClienteDTO objClienteDTO = GetListaClientes(new ClienteDTO { id = objProyectoDTO.FK_Cliente }).FirstOrDefault();
                if (objClienteDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                List<ContactoDTO> lstContactosDTO = GetListaContactos(new ContactoDTO { FK_Cliente = objClienteDTO.id }).ToList();

                DivisionDTO objDivisionDTO = GetListaDivisiones(new DivisionDTO { id = objProyectoDTO.FK_Division }).FirstOrDefault();
                if (objDivisionDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                MunicipioDTO objMunicipioDTO = GetListaMunicipios(new MunicipioDTO { idMunicipio = objProyectoDTO.FK_Municipio }).FirstOrDefault();
                if (objMunicipioDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                EstadoDTO objEstadoDTO = GetListaEstados(new EstadoDTO { idEstado = objMunicipioDTO.idEstado }).FirstOrDefault();
                if (objEstadoDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                PaisDTO objPaisDTO = GetListaPaises(new PaisDTO { idPais = objEstadoDTO.idPais }).FirstOrDefault();
                if (objPaisDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                PrioridadEstatusDTO objPrioridadEstatusDTO = GetListaPrioridadEstatus(new PrioridadEstatusDTO { id = objProyectoDTO.FK_Prioridad }).FirstOrDefault();
                if (objPrioridadEstatusDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                PrioridadDTO objPrioridadDTO = GetListaPrioridades(new PrioridadDTO { id = objProyectoDTO.FK_Prioridad }).FirstOrDefault();
                if (objPrioridadEstatusDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                RiesgoDTO objRiesgoDTO = GetListaRiesgos(new RiesgoDTO { id = objProyectoDTO.FK_Riesgo }).FirstOrDefault();
                if (objRiesgoDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                UsuarioDTO objUsuarioDTO = GetListaUsuarios(new UsuarioDTO { id = objProyectoDTO.FK_UsuarioResponsable }).FirstOrDefault();
                if (objUsuarioDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);
                #endregion

                #region RESUMEN PROYECTO
                objProyectoDTO.nombreCliente = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(objClienteDTO.nombreCliente);
                objProyectoDTO.tipoCliente = EnumHelper.GetDescription((TipoClientesEnum)objClienteDTO.FK_TipoCliente);
                objProyectoDTO.division = string.Format("[{0}] {1}", objDivisionDTO.numDivision, objDivisionDTO.division);
                objProyectoDTO.strImporteCotizadoAprox = objProyectoDTO.importeCotizadoAprox.ToString("C");
                objProyectoDTO.ubicacion = string.Format("{0}, {1}, {2}", objMunicipioDTO.Municipio, objEstadoDTO.Estado, objPaisDTO.Pais);
                objProyectoDTO.estatus = objPrioridadEstatusDTO.estatus;
                objProyectoDTO.escenario = EnumHelper.GetDescription((EscenariosEnum)objProyectoDTO.FK_Escenario);
                objProyectoDTO.prioridad = objPrioridadDTO.prioridad;
                objProyectoDTO.nombreCompletoResponsable = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno));
                objProyectoDTO.riesgo = objRiesgoDTO.riesgo;

                if (lstContactosDTO.Count() > 0)
                {
                    int numContacto = 1;
                    objProyectoDTO.htmlContactos = string.Empty;
                    foreach (var item in lstContactosDTO)
                    {
                        objProyectoDTO.htmlContactos += string.Format(
                            @"<!-- CONTACTO #{0} -->
                            <div class='row'>
                                <div class='col-lg-12'>
                                    <fieldset class='fieldset-custm'>
                                        <legend class='legend-custm'><span class='badge'>Contacto #{0}</span></legend>
                                        <div class='row'>
                                            <div class='col-lg-12'><b>Nombre:&nbsp;{1}</b><span></span></div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-12'><b>Puesto:&nbsp;{2}</b><span></span></div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-6'><b>Teléfono:&nbsp;{3}</b><span></span></div>
                                            <div class='col-lg-6'><b>Celular:&nbsp;{4}</b><span></span></div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-12'><b>Correo:&nbsp;{5}</b><span></span></div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <!-- END: CONTACTO #{0} -->", numContacto, item.nombreContacto, item.puesto, item.telefono, item.celular, item.correo);
                        numContacto++;
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objProyectoDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> GetResumenProyecto_UltimoComentario(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region ULTIMO COMENTARIO DEL PROYECTO
                ComentarioProyectoDTO objUltimoComentarioDTO = GetListaComentarios(new ComentarioProyectoDTO { FK_Proyecto = objParamsDTO.id }).OrderByDescending(o => o.id).FirstOrDefault();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objUltimoComentarioDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> GetResumenProyecto_ProximaAccion(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region PROXIMA ACCIÓN DEL PROYECTO
                ProximaAccionDTO objProximaAccionDTO = GetListaProximasAcciones(new ProximaAccionDTO { FK_Proyecto = objParamsDTO.id }).OrderByDescending(o => o.id).FirstOrDefault();

                if (objProximaAccionDTO != null)
                {
                    if (objProximaAccionDTO.FK_UsuarioResponsable > 0)
                    {
                        UsuarioDTO objUsuarioResponsableAccionDTO = GetListaUsuarios(new UsuarioDTO { id = objProximaAccionDTO.FK_UsuarioResponsable }).FirstOrDefault();
                        if (objUsuarioResponsableAccionDTO != null)
                            objProximaAccionDTO.nombreCompletoResponsableAccion = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(
                                string.Format("{0} {1} {2}", objUsuarioResponsableAccionDTO.nombre, objUsuarioResponsableAccionDTO.apellidoPaterno, objUsuarioResponsableAccionDTO.apellidoMaterno));
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objProximaAccionDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region COMENTARIOS PROYECTO
        public Dictionary<string, object> GetResumenComentarios(ComentarioProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                string strComentarios = string.Empty;
                foreach (var item in GetListaComentarios().ToList())
                {
                    UsuarioDTO objUsuarioDTO = GetListaUsuarios(new UsuarioDTO { id = item.FK_UsuarioComentario }).FirstOrDefault();
                    if (objUsuarioDTO == null)
                        throw new Exception("Ocurrió un error al obtener los comentarios registrados.");

                    if (item.FK_UsuarioComentario != vSesiones.sesionUsuarioDTO.id)
                    {
                        strComentarios += string.Format(
                            @"<div class='row'>
                                <div class='col-lg-6'>
                                    <b class='pull-left'>{0}:</b><br>
                                    <div class='pull-left btn_Comentario_Contenido textoIzquierda'>{1}<br>
                                        <p class='pull-left fechaComentario'>{2}</p>
                                    </div><br>
                                </div>
                            </div>", PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno)),
                                        item.ultimoComentario,
                                        string.Format("{0}/{1}/{2}", item.fechaComentario.Day, item.fechaComentario.Month, item.fechaComentario.Year));
                    }
                    else if (item.FK_UsuarioComentario == vSesiones.sesionUsuarioDTO.id)
                    {
                        strComentarios += string.Format(
                            @"<div class='row'>
                                <div class='col-lg-6'></div>
                                <div class='col-lg-6'>
                                    <b class='pull-right'>
                                        <!--<button class='btn btn-danger btn-xs btnEliminarComentario' title='Eliminar comentario.' data-id='{0}'><i class='fas fa-trash btnEliminarComentario' data-id='{0}'></i></button>-->
                                        {1}:
                                    </b><br>
                                    <div class='pull-right btn_Comentario_Contenido textoDerecha'>{2}<br>
                                        <p class='pull-right fechaComentario'>{3}</p>
                                    </div>
                                </div>
                            </div>", item.id,
                                        PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno)),
                                        item.ultimoComentario,
                                        string.Format("{0}/{1}/{2}", item.fechaComentario.Day, item.fechaComentario.Month, item.fechaComentario.Year));
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, strComentarios);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearComentario(ComentarioProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al registrar el comentario."); }
                if (string.IsNullOrEmpty(objParamsDTO.ultimoComentario)) { throw new Exception("Es necesario indicar el comentario."); }
                #endregion

                #region CREAR COMENTARIO
                tblAF_CRM_ComentariosProyectos objCrearComentario = new tblAF_CRM_ComentariosProyectos();
                objCrearComentario.FK_Proyecto = objParamsDTO.FK_Proyecto;
                objCrearComentario.ultimoComentario = objParamsDTO.ultimoComentario.Trim();
                objCrearComentario.fechaComentario = DateTime.Now;
                objCrearComentario.FK_UsuarioComentario = vSesiones.sesionUsuarioDTO.id;
                objCrearComentario.fechaCreacion = DateTime.Now;
                objCrearComentario.registroActivo = true;
                _context.tblAF_CRM_ComentariosProyectos.Add(objCrearComentario);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito.");
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarResumenComentario(ComentarioProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el comentario.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR COMENTARIO
                tblAF_CRM_ComentariosProyectos objComentario = _context.tblAF_CRM_ComentariosProyectos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objComentario == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objComentario.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objComentario.fechaModificacion = DateTime.Now;
                objComentario.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region ACCIONES PROYECTO
        public Dictionary<string, object> GetResumenAcciones(ProximaAccionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al obtener el listado de acciones."); }
                #endregion

                #region GET ACCIONES
                List<ProximaAccionDTO> lstProximasAccionesDTO = new List<ProximaAccionDTO>();
                ProximaAccionDTO objProximaAccionDTO = new ProximaAccionDTO();
                int numAccion = 0;
                foreach (var item in GetListaProximasAcciones(new ProximaAccionDTO { FK_Proyecto = objParamsDTO.FK_Proyecto }).ToList())
                {
                    UsuarioDTO objUsuarioDTO = GetListaUsuarios(new UsuarioDTO { id = item.FK_UsuarioResponsable }).FirstOrDefault();
                    if (objUsuarioDTO == null)
                        throw new Exception("Ocurrió un error al obtener al responsable de la acción.");

                    objProximaAccionDTO = new ProximaAccionDTO();
                    objProximaAccionDTO.id = item.id;
                    numAccion += 1;
                    objProximaAccionDTO.numAccion = numAccion;
                    objProximaAccionDTO.accionFinalizada = item.accionFinalizada;
                    objProximaAccionDTO.accion = string.Format(
                            @"<div class='row'>
                                <div class='col-lg-12'>
                                    <b>Próxima acción</b>: {0} 
                                </div>
                            </div>
                            <div class='row'>
                                <div class='col-lg-4'>
                                    <b>Fecha</b>: {1}/{2}/{3} 
                                </div>
                                <div class='col-lg-4'>
                                    <b>Responsable</b>: {4} 
                                </div>
                                <div class='col-lg-4'>
                                    <b>Progreso</b>: {5} 
                                </div>
                            </div>", item.accion,
                                     item.fechaProximaAccion.Day.ToString("00"),
                                     item.fechaProximaAccion.Month.ToString("00"),
                                     item.fechaProximaAccion.Year.ToString("00"),
                                     PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuarioDTO.nombre, objUsuarioDTO.apellidoPaterno, objUsuarioDTO.apellidoMaterno)),
                                     string.Format("{0}%", item.progreso));

                    lstProximasAccionesDTO.Add(objProximaAccionDTO);
                }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstProximasAccionesDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearAccion(ProximaAccionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al registrar la acción."); }
                if (string.IsNullOrEmpty(objParamsDTO.accion)) { throw new Exception("Es necesario indicar la proxima acción."); }
                if (objParamsDTO.fechaProximaAccion == null) { throw new Exception("Es necesario indicar la fecha."); }
                if (objParamsDTO.FK_UsuarioResponsable <= 0) { throw new Exception("Es necesario seleccionar al responsable."); }
                if (objParamsDTO.progreso <= -1) { throw new Exception("Ocurrió un error al registrar la acción."); }
                #endregion

                #region CREAR COMENTARIO
                tblAF_CRM_ProximasAcciones objCrearProximaAccion = new tblAF_CRM_ProximasAcciones();
                objCrearProximaAccion.FK_Proyecto = objParamsDTO.FK_Proyecto;
                objCrearProximaAccion.accion = objParamsDTO.accion.Trim();
                objCrearProximaAccion.fechaProximaAccion = objParamsDTO.fechaProximaAccion;
                objCrearProximaAccion.FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable;
                objCrearProximaAccion.progreso = objParamsDTO.progreso;
                objCrearProximaAccion.accionFinalizada = false;
                objCrearProximaAccion.fechaCreacion = DateTime.Now;
                objCrearProximaAccion.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearProximaAccion.registroActivo = true;
                _context.tblAF_CRM_ProximasAcciones.Add(objCrearProximaAccion);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito.");

                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarResumenAccion(ProximaAccionDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar la proxima acción.";

                    #region VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    #endregion

                    #region ELIMINAR PROXIMA ACCIÓN
                    tblAF_CRM_ProximasAcciones objProximaAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objProximaAccion == null)
                        throw new Exception(MENSAJE_ERROR_GENERAL);

                    objProximaAccion.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objProximaAccion.fechaModificacion = DateTime.Now;
                    objProximaAccion.registroActivo = false;
                    _context.SaveChanges();

                    // ELIMINAR DETALLE PROXIMA ACCIÓN
                    Dictionary<string, object> resEliminarAccionDetalle = EliminarProximasAccionesDet(objParamsDTO.id);
                    if (!(bool)resEliminarAccionDetalle[SUCCESS])
                        throw new Exception((string)resEliminarAccionDetalle[MESSAGE]);
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FinalizarAccionPrincipal(ProximaAccionDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al finalizar la acción.";
                    if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    #endregion

                    tblAF_CRM_ProximasAcciones objFinalizarAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objFinalizarAccion == null)
                        throw new Exception(MENSAJE_ERROR_GENERAL);
                    else
                    {
                        objFinalizarAccion.progreso = 100;
                        objFinalizarAccion.accionFinalizada = true;
                        objFinalizarAccion.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objFinalizarAccion.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        Dictionary<string, object> resFinalizarAccionDetalle = FinalizarAccionesDetalle(objParamsDTO.id);
                        if (!(bool)resFinalizarAccionDetalle[SUCCESS])
                            throw new Exception((string)resFinalizarAccionDetalle[MESSAGE]);
                    }

                    #region SET PROGRESO ACCIÓN PRINCIPAL
                    Dictionary<string, object> resSetPromedioAccion = SetPromedioAccion(new ProximaAccionDetalleDTO { FK_Accion = objParamsDTO.id });
                    if (!(bool)resSetPromedioAccion[SUCCESS])
                        throw new Exception((string)resSetPromedioAccion[MESSAGE]);

                    tblAF_CRM_ProximasAcciones objActualizarProximaAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objActualizarProximaAccion == null)
                        throw new Exception("Ocurrió un error al actualizar el progreso de la acción.");
                    else
                    {
                        objActualizarProximaAccion.progreso = (decimal)resSetPromedioAccion[ITEMS];
                        _context.SaveChanges();
                    }
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha finalizado con éxito.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        #region ACCIONES DETALLE PROYECTO
        public Dictionary<string, object> GetResumenDetalleAccion(ProximaAccionDetalleDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Accion <= 0) { throw new Exception("Ocurrió un error al obtener las tareas asignadas a la acción."); }
                #endregion

                #region GET TAREAS ACCIÓN
                List<ProximaAccionDetalleDTO> lstProximasAccionesDetDTO = new List<ProximaAccionDetalleDTO>();
                ProximaAccionDetalleDTO objProximaAccionDetDTO = new ProximaAccionDetalleDTO();
                int numAccion = 0;
                foreach (var item in GetListaProximasAccionesDetalle().Where(w => w.FK_Accion == objParamsDTO.FK_Accion).ToList())
                {
                    UsuarioDTO objUsuario = GetListaUsuarios(new UsuarioDTO { id = item.FK_UsuarioResponsable }).FirstOrDefault();
                    if (objUsuario == null)
                        throw new Exception("Ocurrió un error al obtener el nombre completo del responsable de la acción.");

                    objProximaAccionDetDTO = new ProximaAccionDetalleDTO();
                    objProximaAccionDetDTO.id = item.id;
                    numAccion += 1;
                    objProximaAccionDetDTO.numAccionDet = numAccion;
                    objProximaAccionDetDTO.accionFinalizada = item.accionFinalizada;
                    objProximaAccionDetDTO.accion =
                        string.Format(@"<div class='row'>
                                            <div class='col-lg-12'>
                                                <b>Próxima acción</b>: {0} 
                                            </div>
                                        </div>
                                        <div class='row'>
                                            <div class='col-lg-4'>
                                                <b>Fecha</b>: {1}/{2}/{3} 
                                            </div>
                                            <div class='col-lg-4'>
                                                <b>Responsable</b>: {4} 
                                            </div>
                                            <div class='col-lg-4'>
                                                <b>Progreso</b>: {5} 
                                            </div>
                                        </div>", item.accion,
                                                    item.fechaProximaAccion.Day.ToString("00"),
                                                    item.fechaProximaAccion.Month.ToString("00"),
                                                    item.fechaProximaAccion.Year.ToString("00"),
                                                    PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno)),
                                                    item.progreso);

                    lstProximasAccionesDetDTO.Add(objProximaAccionDetDTO);
                }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstProximasAccionesDetDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Accion <= 0) { throw new Exception("Ocurrió un error al registrar la acción."); }
                if (string.IsNullOrEmpty(objParamsDTO.accion)) { throw new Exception("Es necesario indicar la proxima acción."); }
                if (objParamsDTO.fechaProximaAccion == null) { throw new Exception("Es necesario indicar la fecha."); }
                if (objParamsDTO.FK_UsuarioResponsable <= 0) { throw new Exception("Es necesario seleccionar al responsable."); }
                if (objParamsDTO.progreso <= -1) { throw new Exception("Ocurrió un error al registrar la acción detalle."); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region CREAR DETALLE ACCIÓN
                        tblAF_CRM_ProximasAccionesDetalle objCrearProximaAccionDet = new tblAF_CRM_ProximasAccionesDetalle();
                        objCrearProximaAccionDet.FK_Accion = objParamsDTO.FK_Accion;
                        objCrearProximaAccionDet.accion = objParamsDTO.accion.Trim();
                        objCrearProximaAccionDet.fechaProximaAccion = objParamsDTO.fechaProximaAccion;
                        objCrearProximaAccionDet.FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable;
                        objCrearProximaAccionDet.progreso = objParamsDTO.progreso;
                        objCrearProximaAccionDet.accionFinalizada = false;
                        objCrearProximaAccionDet.fechaCreacion = DateTime.Now;
                        objCrearProximaAccionDet.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objCrearProximaAccionDet.registroActivo = true;
                        _context.tblAF_CRM_ProximasAccionesDetalle.Add(objCrearProximaAccionDet);
                        _context.SaveChanges();
                        #endregion

                        #region SET PROGRESO ACCIÓN PRINCIPAL
                        Dictionary<string, object> resSetPromedioAccion = SetPromedioAccion(new ProximaAccionDetalleDTO { FK_Accion = objParamsDTO.FK_Accion });
                        if (!(bool)resSetPromedioAccion[SUCCESS])
                            throw new Exception((string)resSetPromedioAccion[MESSAGE]);

                        tblAF_CRM_ProximasAcciones objActualizarProximaAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.FK_Accion && w.registroActivo).FirstOrDefault();
                        if (objActualizarProximaAccion == null)
                            throw new Exception("Ocurrió un error al actualizar el progreso de la acción.");
                        else
                        {
                            objActualizarProximaAccion.progreso = (decimal)resSetPromedioAccion[ITEMS];
                            _context.SaveChanges();
                        }
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }

                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarResumenAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el detalle de la proxima acción.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region ELIMINAR PROXIMA ACCIÓN
                        tblAF_CRM_ProximasAccionesDetalle objProximaAccionDet = _context.tblAF_CRM_ProximasAccionesDetalle.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                        if (objProximaAccionDet == null)
                            throw new Exception(MENSAJE_ERROR_GENERAL);

                        objProximaAccionDet.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objProximaAccionDet.fechaModificacion = DateTime.Now;
                        objProximaAccionDet.registroActivo = false;
                        _context.SaveChanges();
                        #endregion

                        #region SET PROGRESO ACCIÓN PRINCIPAL
                        Dictionary<string, object> resSetPromedioAccion = SetPromedioAccion(new ProximaAccionDetalleDTO { FK_Accion = objParamsDTO.FK_Accion });
                        if (!(bool)resSetPromedioAccion[SUCCESS])
                            throw new Exception((string)resSetPromedioAccion[MESSAGE]);

                        tblAF_CRM_ProximasAcciones objActualizarProximaAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.FK_Accion && w.registroActivo).FirstOrDefault();
                        if (objActualizarProximaAccion == null)
                            throw new Exception("Ocurrió un error al actualizar el progreso de la acción.");
                        else
                        {
                            objActualizarProximaAccion.progreso = (decimal)resSetPromedioAccion[ITEMS];
                            _context.SaveChanges();
                        }
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EliminarProximasAccionesDet(int FK_Accion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE ELIMINA DETALLE
                List<tblAF_CRM_ProximasAccionesDetalle> lstAccionesDetalle = _context.tblAF_CRM_ProximasAccionesDetalle.Where(w => w.FK_Accion == FK_Accion && w.registroActivo).ToList();
                if (lstAccionesDetalle.Count() > 0)
                {
                    foreach (var item in lstAccionesDetalle)
                    {
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();

                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(FK_Accion));
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, 0, new { FK_Accion = FK_Accion });
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> FinalizarAccionesDetalle(int FK_Accion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblAF_CRM_ProximasAccionesDetalle> lstProximasAccionesDet = _context.tblAF_CRM_ProximasAccionesDetalle.Where(w => w.FK_Accion == FK_Accion && w.registroActivo).ToList();
                if (lstProximasAccionesDet.Count() > 0)
                {
                    foreach(var item in lstProximasAccionesDet)
                    {
                        item.progreso = 100;
                        item.accionFinalizada = true;
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                    }
                    _context.SaveChanges();
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, 0, new { FK_Accion = FK_Accion });
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FinalizarAccionDetalle(ProximaAccionDetalleDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al finalizar la acción del detalle.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region SE FINALIZA LA TAREA
                        tblAF_CRM_ProximasAccionesDetalle objFinalizarAccionDet = _context.tblAF_CRM_ProximasAccionesDetalle.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                        if (objFinalizarAccionDet == null)
                            throw new Exception(MENSAJE_ERROR_GENERAL);

                        objFinalizarAccionDet.progreso = 100;
                        objFinalizarAccionDet.accionFinalizada = true;
                        objFinalizarAccionDet.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objFinalizarAccionDet.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion

                        #region SET PROGRESO ACCIÓN PRINCIPAL
                        Dictionary<string, object> resSetPromedioAccion = SetPromedioAccion(new ProximaAccionDetalleDTO { FK_Accion = objParamsDTO.FK_Accion });
                        if (!(bool)resSetPromedioAccion[SUCCESS])
                            throw new Exception((string)resSetPromedioAccion[MESSAGE]);

                        tblAF_CRM_ProximasAcciones objActualizarProximaAccion = _context.tblAF_CRM_ProximasAcciones.Where(w => w.id == objParamsDTO.FK_Accion && w.registroActivo).FirstOrDefault();
                        if (objActualizarProximaAccion == null)
                            throw new Exception("Ocurrió un error al actualizar el progreso de la acción.");
                        else
                        {
                            objActualizarProximaAccion.progreso = (decimal)resSetPromedioAccion[ITEMS];
                            _context.SaveChanges();
                        }
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha finalizado con éxito.");
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> SetPromedioAccion(ProximaAccionDetalleDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Accion <= 0) { throw new Exception("Ocurrió un error al obtener el promedio de la acción."); }
                #endregion

                List<tblAF_CRM_ProximasAccionesDetalle> lstProximasAccionesDetalleDTO = _context.tblAF_CRM_ProximasAccionesDetalle.Where(w => w.FK_Accion == objParamsDTO.FK_Accion && w.registroActivo).ToList();
                int cantTareasAsignadas = lstProximasAccionesDetalleDTO.Count();
                int cantTareasFinalizadas = lstProximasAccionesDetalleDTO.Where(w => w.accionFinalizada).Count();
                decimal promedioAccion = cantTareasFinalizadas > 0 && cantTareasAsignadas > 0 ? (((decimal)cantTareasFinalizadas / (decimal)cantTareasAsignadas) * 100) : 0;

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, promedioAccion);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region COTIZACIONES
        public Dictionary<string, object> GetCotizaciones(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<CotizacionDTO> lstCotizacionesDTO = GetListaCotizaciones(new CotizacionDTO { FK_Proyecto = objParamsDTO.FK_Proyecto }).OrderByDescending(o => o.importeRevN).ToList();
                foreach (var item in lstCotizacionesDTO)
                {
                    UsuarioDTO objUsuario = GetListaUsuarios(new UsuarioDTO { id = item.FK_ResponsableCotizacion }).FirstOrDefault();
                    if (objUsuario == null)
                        throw new Exception("Ocurrió un error al obtener el listado de cotizaciones.");

                    item.nombreCompletoResponsable = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno));
                    item.strImporteFinal = item.importeFinal.ToString("C");
                    item.strImporteOriginal = item.importeOriginal.ToString("C");
                };

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstCotizacionesDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        
        public Dictionary<string, object> CrearCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacion = ValidacionesCrearEditarCotizacion(objParamsDTO);
                if (!(bool)resValidacion[SUCCESS])
                    throw new Exception((string)resValidacion[MESSAGE]);
                #endregion

                #region CREAR COTIZACIÓN
                tblAF_CRM_Cotizaciones objCrearCotizacion = new tblAF_CRM_Cotizaciones();
                objCrearCotizacion.FK_Proyecto = objParamsDTO.FK_Proyecto;
                objCrearCotizacion.FK_ResponsableCotizacion = objParamsDTO.FK_ResponsableCotizacion;
                objCrearCotizacion.importeFinal = objParamsDTO.importeFinal;
                objCrearCotizacion.fechaFinal = objParamsDTO.fechaFinal;
                objCrearCotizacion.importeRevN = objParamsDTO.importeRevN;
                objCrearCotizacion.fechaRevN = objParamsDTO.fechaRevN;
                objCrearCotizacion.importeOriginal = objParamsDTO.importeOriginal;
                objCrearCotizacion.fechaOriginal = objParamsDTO.fechaOriginal;
                objCrearCotizacion.comentario = objParamsDTO.comentario;
                objCrearCotizacion.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearCotizacion.fechaCreacion = DateTime.Now;
                objCrearCotizacion.registroActivo = true;
                _context.tblAF_CRM_Cotizaciones.Add(objCrearCotizacion);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito la cotización.");

                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al actualizar la cotización.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                Dictionary<string, object> resValidacion = ValidacionesCrearEditarCotizacion(objParamsDTO);
                if (!(bool)resValidacion[SUCCESS])
                    throw new Exception((string)resValidacion[MESSAGE]);
                #endregion

                #region ACTUALIZAR COTIZACIÓN
                tblAF_CRM_Cotizaciones objActualizarCotizacion = _context.tblAF_CRM_Cotizaciones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objActualizarCotizacion == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);
                else
                {
                    objActualizarCotizacion.FK_ResponsableCotizacion = objParamsDTO.FK_ResponsableCotizacion;
                    objActualizarCotizacion.importeFinal = objParamsDTO.importeFinal;
                    objActualizarCotizacion.fechaFinal = objParamsDTO.fechaFinal;
                    objActualizarCotizacion.importeRevN = objParamsDTO.importeRevN;
                    objActualizarCotizacion.fechaRevN = objParamsDTO.fechaRevN;
                    objActualizarCotizacion.importeOriginal = objParamsDTO.importeOriginal;
                    objActualizarCotizacion.fechaOriginal = objParamsDTO.fechaOriginal;
                    objActualizarCotizacion.comentario = objParamsDTO.comentario;
                    objActualizarCotizacion.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objActualizarCotizacion.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha actualizado con éxito la cotización.");

                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEditarCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al registrar la cotización."); }
                if (objParamsDTO.FK_ResponsableCotizacion <= 0) { throw new Exception("Es necesario seleccionar al responsable."); }

                if (objParamsDTO.importeFinal == 0) { throw new Exception("Es necesario indicar el importe final."); }
                if (objParamsDTO.importeFinal <= -1) { throw new Exception("Es necesario indicar el importe final con importe positivo."); }
                if (objParamsDTO.fechaFinal == null) { throw new Exception("Es necesario indicar la fecha final."); }

                if (objParamsDTO.importeRevN == 0) { throw new Exception("Es necesario indicar el importe Rev N."); }
                if (objParamsDTO.importeRevN <= -1) { throw new Exception("Es necesario indicar el importe Rev N con importe positivo."); }
                if (objParamsDTO.fechaRevN == null) { throw new Exception("Es necesario indicar la fecha Rev N."); }

                if (objParamsDTO.importeOriginal == 0) { throw new Exception("Es necesario indicar el importe original."); }
                if (objParamsDTO.importeOriginal <= -1) { throw new Exception("Es necesario indicar el importe original con importe positivo."); }
                if (objParamsDTO.fechaOriginal == null) { throw new Exception("Es necesario indicar la fecha original."); }
                if (string.IsNullOrEmpty(objParamsDTO.comentario)) { throw new Exception("Es necesario indicar el comentario."); }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar la cotización";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR COTIZACIÓN
                tblAF_CRM_Cotizaciones objEliminarCotizacion = _context.tblAF_CRM_Cotizaciones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objEliminarCotizacion == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);
                else
                {
                    objEliminarCotizacion.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objEliminarCotizacion.fechaModificacion = DateTime.Now;
                    objEliminarCotizacion.registroActivo = false;
                    _context.SaveChanges();

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito la cotización.");
                }
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información de la cotización";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region INFORMACIÓN COTIZACIÓN
                CotizacionDTO objCotizacion = GetListaCotizaciones(new CotizacionDTO { id = objParamsDTO.id }).FirstOrDefault();
                if (objCotizacion == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objCotizacion);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> VerificarUltimaCotizacion(CotizacionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Proyecto <= 0) { throw new Exception("Ocurrió un error al verificar las cotizaciones relacionadas al proyecto."); }
                #endregion

                #region INFO ULTIMA COTIZACIÓN
                CotizacionDTO objUltimaCotizacionDTO = GetListaCotizaciones(new CotizacionDTO { FK_Proyecto = objParamsDTO.FK_Proyecto }).OrderByDescending(o => o.id).FirstOrDefault();

                CotizacionDTO objProximaCotizacionDTO = new CotizacionDTO();
                objProximaCotizacionDTO = new CotizacionDTO();

                if (objUltimaCotizacionDTO != null)
                {
                    int importeRevN = objUltimaCotizacionDTO.importeRevN + 1;
                    objProximaCotizacionDTO.importeRevN = importeRevN;
                }
                else
                    objProximaCotizacionDTO.importeRevN = 1;

                if (objProximaCotizacionDTO.importeRevN == 2)
                {
                    objProximaCotizacionDTO.strImporteOriginal = objUltimaCotizacionDTO != null ? objUltimaCotizacionDTO.importeOriginal.ToString("C") : "$0.00";
                    objProximaCotizacionDTO.fechaOriginal = objUltimaCotizacionDTO != null ? objUltimaCotizacionDTO.fechaOriginal : DateTime.Now;
                }
                else if (objProximaCotizacionDTO.importeRevN >= 3)
                {
                    objProximaCotizacionDTO.strImporteOriginal = objUltimaCotizacionDTO != null ? objUltimaCotizacionDTO.importeFinal.ToString("C") : "$0.00";
                    objProximaCotizacionDTO.fechaOriginal = objUltimaCotizacionDTO != null ? objUltimaCotizacionDTO.fechaFinal : DateTime.Now;
                }

                objProximaCotizacionDTO.fechaRevN = objUltimaCotizacionDTO != null ? objUltimaCotizacionDTO.fechaRevN : DateTime.Now;
                objProximaCotizacionDTO.hayCotizacionAnterior = objUltimaCotizacionDTO != null ? true : false;

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objProximaCotizacionDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region CLIENTES
        public Dictionary<string, object> GetClientes(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_EstatusHistorial <= 0) { throw new Exception("Es necesario seleccionar el estatus."); }
                #endregion

                #region CATALOGOS
                List<DivisionDTO> lstDivisionesDTO = GetListaDivisiones().ToList();
                List<MunicipioDTO> lstMunicipiosDTO = GetListaMunicipios().ToList();
                List<EstadoDTO> lstEstadosDTO = GetListaEstados().ToList();
                List<PaisDTO> lstPaisesDTO = GetListaPaises().ToList();
                #endregion

                #region LISTADO DE CLIENTES
                List<ClienteDTO> lstClientesDTO = new List<ClienteDTO>();
                ClienteDTO objClienteDTO = new ClienteDTO();
                foreach (var item in GetListaClientes(new ClienteDTO { FK_EstatusHistorial = objParamsDTO.FK_EstatusHistorial }).ToList())
                {
                    objClienteDTO = new ClienteDTO();
                    objClienteDTO.id = item.id;
                    objClienteDTO.nombreCliente = item.nombreCliente;
                    objClienteDTO.division = lstDivisionesDTO.Where(w => w.id == item.FK_Division).Select(s => s.division).FirstOrDefault();
                    objClienteDTO.ubicacion = GetUbicacion(item.FK_Municipio, lstMunicipiosDTO, lstEstadosDTO, lstPaisesDTO);
                    objClienteDTO.paginaWeb = item.paginaWeb;
                    lstClientesDTO.Add(objClienteDTO);
                };

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstClientesDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearCliente(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearEditarCliente = ValidacionesCrearEditarCliente(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarCliente[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarCliente[MESSAGE]);
                #endregion

                #region CREAR CLIENTE
                tblAF_CRM_Clientes objCrearCliente = new tblAF_CRM_Clientes();
                objCrearCliente.nombreCliente = objParamsDTO.nombreCliente.Trim();
                objCrearCliente.FK_Division = objParamsDTO.FK_Division;
                objCrearCliente.FK_Municipio = objParamsDTO.FK_Municipio;
                objCrearCliente.paginaWeb = !string.IsNullOrEmpty(objParamsDTO.paginaWeb) ? objParamsDTO.paginaWeb.Trim() : string.Empty;
                objCrearCliente.FK_TipoCliente = objParamsDTO.FK_TipoCliente;
                objCrearCliente.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                objCrearCliente.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearCliente.fechaCreacion = DateTime.Now;
                objCrearCliente.registroActivo = true;
                _context.tblAF_CRM_Clientes.Add(objCrearCliente);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                resultado.Add("FK_Cliente", objCrearCliente.id);

                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarCliente(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al actualizar el registro."); }
                Dictionary<string, object> resValidacionesCrearEditarCliente = ValidacionesCrearEditarCliente(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarCliente[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarCliente[MESSAGE]);
                #endregion

                #region ACTUALIZAR CLIENTE
                tblAF_CRM_Clientes objActualizarCliente = _context.tblAF_CRM_Clientes.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objActualizarCliente == null)
                    throw new Exception("Ocurrió un error al obtener la información del cliente para actualizar.");

                objActualizarCliente.nombreCliente = objParamsDTO.nombreCliente.Trim();
                objActualizarCliente.FK_Division = objParamsDTO.FK_Division;
                objActualizarCliente.FK_Municipio = objParamsDTO.FK_Municipio;
                objActualizarCliente.paginaWeb = !string.IsNullOrEmpty(objParamsDTO.paginaWeb) ? objParamsDTO.paginaWeb.Trim() : string.Empty;
                objActualizarCliente.FK_TipoCliente = objParamsDTO.FK_TipoCliente;
                objActualizarCliente.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objActualizarCliente.fechaModificacion = DateTime.Now;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha actualizado con éxito.");

                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCliente(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información del cliente para actualizar.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0)
                    throw new Exception(MENSAJE_ERROR_GENERAL);
                #endregion

                #region INFORMACIÓN DEL CLIENTE
                ClienteDTO objClienteEF = GetListaClientes(new ClienteDTO { id = objParamsDTO.id }).FirstOrDefault();
                if (objClienteEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                ClienteDTO objClienteDTO = new ClienteDTO();
                objClienteDTO.nombreCliente = objClienteEF.nombreCliente;
                objClienteDTO.FK_Division = objClienteEF.FK_Division;
                objClienteDTO.FK_Municipio = objClienteEF.FK_Municipio;
                objClienteDTO.FK_Estado = objClienteDTO.FK_Estado > 0 ? GetListaMunicipios(new MunicipioDTO { idMunicipio = objClienteEF.FK_Municipio }).Select(s => s.idEstado).FirstOrDefault() : 0;
                objClienteDTO.FK_Pais = objClienteDTO.FK_Estado > 0 ? GetListaEstados(new EstadoDTO { idEstado = objClienteDTO.FK_Estado }).Select(s => s.idPais).FirstOrDefault() : 0;
                objClienteDTO.paginaWeb = objClienteEF.paginaWeb;
                objClienteDTO.FK_TipoCliente = objClienteEF.FK_TipoCliente;

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objClienteDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEditarCliente(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrEmpty(objParamsDTO.nombreCliente)) { throw new Exception("Es necesario indicar el nombre del cliente."); }

                if (!objParamsDTO.esCrearClienteDesdeProyectos)
                {
                    if (objParamsDTO.FK_Division <= 0) { throw new Exception("Es necesario seleccionar una división."); }
                    if (objParamsDTO.FK_Municipio <= 0) { throw new Exception("Es necesario seleccionar un municipio."); }
                    if (objParamsDTO.FK_TipoCliente <= 0) { throw new Exception("Es necesario seleccionar el tipo de cliente."); }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCliente(ClienteDTO objParamsDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el cliente.";

                    #region VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    if (GetListaProyectos(new ProyectoDTO { FK_Cliente = objParamsDTO.id, esProspecto = false }).Any()) { throw new Exception("No se puede eliminar el cliente, ya que se encuentra registrado en un proyecto."); }
                    #endregion

                    #region ELIMINAR CLIENTE
                    tblAF_CRM_Clientes objEliminarCliente = _context.tblAF_CRM_Clientes.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminarCliente == null)
                        throw new Exception(MENSAJE_ERROR_GENERAL);

                    objEliminarCliente.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objEliminarCliente.fechaModificacion = DateTime.Now;
                    objEliminarCliente.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FillCboFiltro_Clientes_Divisiones()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstDivisionesUtilizadas = GetListaProyectos(new ProyectoDTO { esProspecto = false }).Select(s => s.FK_Division).ToList();
                List<DivisionDTO> lstDivisionesDTO = new List<DivisionDTO>();
                if (lstDivisionesUtilizadas.Count() > 0)
                    lstDivisionesDTO = GetListaDivisiones(new DivisionDTO { lstID_Divisiones = lstDivisionesUtilizadas }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstDivisionesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = string.Format("[{0}] {1}", item.numDivision, item.division);
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarClienteHistorial(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al enviar el cliente a historial.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region ENVIAR A HISTORIAL CLIENTE
                        tblAF_CRM_Clientes objClienteEF = _context.tblAF_CRM_Clientes.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                        if (objClienteEF == null)
                            throw new Exception(MENSAJE_ERROR_GENERAL);

                        objClienteEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objClienteEF.fechaModificacion = DateTime.Now;
                        objClienteEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.HISTORIAL;
                        _context.SaveChanges();
                        #endregion

                        #region ENVIAR A HISTORIAL SUS CONTACTOS
                        Dictionary<string, object> resEnviarContactosHistorial = EnviarContactosHistorial(new ClienteDTO { id = objParamsDTO.id });
                        if (!(bool)resEnviarContactosHistorial[SUCCESS])
                            throw new Exception((string)resEnviarContactosHistorial[MESSAGE]);
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha enviado el cliente a historial con éxito.");
                        dbContextTransaction.Commit();

                        // SAVE BITACORA
                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActivarCliente(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al activar el cliente.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region ACTIVAR CLIENTE
                        tblAF_CRM_Clientes objClienteEF = _context.tblAF_CRM_Clientes.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                        if (objClienteEF == null)
                            throw new Exception(MENSAJE_ERROR_GENERAL);

                        objClienteEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objClienteEF.fechaModificacion = DateTime.Now;
                        objClienteEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                        _context.SaveChanges();
                        #endregion

                        #region ACTIVAR CONTACTOS
                        Dictionary<string, object> resActivarContactos = ActivarContactos(new ContactoDTO { FK_Cliente = objParamsDTO.id });
                        if (!(bool)resActivarContactos[SUCCESS])
                            throw new Exception((string)resActivarContactos[MESSAGE]);
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha activado el cliente con éxito.");
                        dbContextTransaction.Commit();

                        // SAVE BITACORA
                        SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EnviarContactosHistorial(ClienteDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al enviar los contactos del cliente a historial."); }
                #endregion

                #region ENVIAR CONTACTOS HISTORIAL
                bool hayContactos = false;
                List<tblAF_CRM_Contactos> lstContactosEF = _context.tblAF_CRM_Contactos.Where(w => w.FK_Cliente == objParamsDTO.id && w.registroActivo).ToList();
                foreach (var item in lstContactosEF)
                {
                    hayContactos = true;
                    item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    item.fechaModificacion = DateTime.Now;
                    item.FK_EstatusHistorial = (int)EstatusHistorialEnum.HISTORIAL;
                }

                if (hayContactos)
                {
                    _context.SaveChanges();
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                }
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ActivarContactos(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Cliente <= 0) { throw new Exception("Ocurrió un error al activar los contactos del cliente."); }
                #endregion

                #region ACTIVAR CONTACTOS
                List<tblAF_CRM_Contactos> lstContactosAF = _context.tblAF_CRM_Contactos.Where(w => w.FK_Cliente == objParamsDTO.FK_Cliente && w.registroActivo).ToList();
                bool hayContactos = false;
                foreach (var item in lstContactosAF)
                {
                    hayContactos = true;
                    item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    item.fechaModificacion = DateTime.Now;
                    item.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                }

                if (hayContactos)
                {
                    _context.SaveChanges();
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                }
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        #region CONTACTOS CLIENTES
        public Dictionary<string, object> GetContactos(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_EstatusHistorial <= 0) { throw new Exception("Es necesario seleccionar el estatus."); }
                #endregion

                #region CATALOGOS
                List<ClienteDTO> lstClientes = GetListaClientes(new ClienteDTO { id = objParamsDTO.FK_Cliente, FK_Division = objParamsDTO.FK_Division }).ToList();
                List<DivisionDTO> lstDivisiones = GetListaDivisiones().ToList();
                List<MunicipioDTO> lstMunicipios = GetListaMunicipios().ToList();
                List<EstadoDTO> lstEstados = GetListaEstados().ToList();
                List<PaisDTO> lstPaises = GetListaPaises().ToList();
                #endregion

                #region LISTADO DE CONTACTOS
                List<ContactoDTO> lstContactosDTO = new List<ContactoDTO>();
                ContactoDTO objContactoDTO = new ContactoDTO();
                if (lstClientes.Count() > 0)
                {
                    foreach (var item in GetListaContactos(new ContactoDTO { lstFK_Clientes = lstClientes.Select(s => s.id).ToList(), FK_EstatusHistorial = objParamsDTO.FK_EstatusHistorial }).ToList())
                    {
                        ClienteDTO objCliente = lstClientes.Where(w => w.id == item.FK_Cliente).FirstOrDefault();
                        if (objCliente == null)
                            throw new Exception("Ocurrió un error al obtener la información del cliente.");

                        objContactoDTO = new ContactoDTO();
                        objContactoDTO.id = item.id;
                        objContactoDTO.division = GetDivision(objCliente.FK_Division, lstDivisiones);
                        objContactoDTO.nombreCliente = objCliente.nombreCliente;
                        objContactoDTO.ubicacion = GetUbicacion(objCliente.FK_Municipio, lstMunicipios, lstEstados, lstPaises);
                        objContactoDTO.nombreContacto = item.nombreContacto;
                        objContactoDTO.puesto = item.puesto;
                        objContactoDTO.correo = item.correo;
                        objContactoDTO.telefono = item.telefono;
                        objContactoDTO.extension = item.extension;
                        objContactoDTO.celular = item.celular;
                        lstContactosDTO.Add(objContactoDTO);
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstContactosDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearEditarContacto = ValidacionesCrearEditarContacto(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarContacto[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarContacto[MESSAGE]);
                #endregion

                #region CREAR CONTACTO
                tblAF_CRM_Contactos objCrearContacto = new tblAF_CRM_Contactos();
                objCrearContacto.FK_Cliente = objParamsDTO.FK_Cliente;
                objCrearContacto.nombreContacto = objParamsDTO.nombreContacto.Trim();
                objCrearContacto.puesto = !objParamsDTO.esCrearClienteDesdeProyectos ? objParamsDTO.puesto.Trim() : string.Empty;
                objCrearContacto.correo = !objParamsDTO.esCrearClienteDesdeProyectos ? objParamsDTO.correo.Trim() : string.Empty;
                objCrearContacto.telefono = !objParamsDTO.esCrearClienteDesdeProyectos ? objParamsDTO.telefono.Trim() : string.Empty;
                objCrearContacto.extension = !objParamsDTO.esCrearClienteDesdeProyectos ? objParamsDTO.extension.Trim() : string.Empty;
                objCrearContacto.celular = !objParamsDTO.esCrearClienteDesdeProyectos ? objParamsDTO.celular.Trim() : string.Empty;
                objCrearContacto.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                objCrearContacto.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearContacto.fechaCreacion = DateTime.Now;
                objCrearContacto.registroActivo = true;
                _context.tblAF_CRM_Contactos.Add(objCrearContacto);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito.");

                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al actualizar el registro."); }
                Dictionary<string, object> resValidacionesCrearEditarContacto = ValidacionesCrearEditarContacto(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarContacto[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarContacto[MESSAGE]);
                #endregion

                #region ACTUALIZAR CONTACTO
                tblAF_CRM_Contactos objActualizarContacto = _context.tblAF_CRM_Contactos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objActualizarContacto == null)
                    throw new Exception("Ocurrió un error al obtener la información del contacto para actualizar.");

                objActualizarContacto.nombreContacto = objParamsDTO.nombreContacto.Trim();
                objActualizarContacto.puesto = objParamsDTO.puesto.Trim();
                objActualizarContacto.correo = objParamsDTO.correo.Trim();
                objActualizarContacto.telefono = objParamsDTO.telefono.Trim();
                objActualizarContacto.extension = objParamsDTO.extension.Trim();
                objActualizarContacto.celular = objParamsDTO.celular.Trim();
                objActualizarContacto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objActualizarContacto.fechaModificacion = DateTime.Now;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha actualizado con éxito.");

                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información del contacto para actualizar.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0)
                    throw new Exception(MENSAJE_ERROR_GENERAL);
                #endregion

                #region INFORMACIÓN DEL CONTACTO
                ContactoDTO objContactoDTO = GetListaContactos(new ContactoDTO { id = objParamsDTO.id }).FirstOrDefault();
                if (objContactoDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                ClienteDTO objClienteDTO = GetListaClientes(new ClienteDTO { id = objContactoDTO.FK_Cliente }).FirstOrDefault();
                if (objClienteDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objContactoDTO.FK_Estado = GetListaMunicipios(new MunicipioDTO { idMunicipio = objClienteDTO.FK_Municipio }).Select(s => s.idEstado).FirstOrDefault();
                objContactoDTO.FK_Pais = GetListaEstados(new EstadoDTO { idEstado = objContactoDTO.FK_Estado }).Select(s => s.idPais).FirstOrDefault();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objContactoDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEditarContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (objParamsDTO.FK_Cliente <= 0) { throw new Exception("Es necesario seleccionar un cliente."); }
                if (string.IsNullOrEmpty(objParamsDTO.nombreContacto)) { throw new Exception("Es necesario indicar el nombre del contacto."); }

                if (!objParamsDTO.esCrearClienteDesdeProyectos)
                {
                    if (string.IsNullOrEmpty(objParamsDTO.puesto)) { throw new Exception("Es necesario indicar el puesto."); }
                    if (string.IsNullOrEmpty(objParamsDTO.correo)) { throw new Exception("Es necesario indicar el correo."); }
                    if (string.IsNullOrEmpty(objParamsDTO.telefono)) { throw new Exception("Es necesario indicar el teléfono."); }
                    if (string.IsNullOrEmpty(objParamsDTO.extension)) { throw new Exception("Es necesario indicar la extensión."); }
                    if (string.IsNullOrEmpty(objParamsDTO.celular)) { throw new Exception("Es necesario indicar el celular."); }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el contacto.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR CONTACTO
                tblAF_CRM_Contactos objEliminarContacto = _context.tblAF_CRM_Contactos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objEliminarContacto == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objEliminarContacto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objEliminarContacto.fechaModificacion = DateTime.Now;
                objEliminarContacto.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarContactoHistorial(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al enviar el cliente a historial.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ENVIAR A HISTORIAL CLIENTE
                tblAF_CRM_Contactos objContactoEF = _context.tblAF_CRM_Contactos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objContactoEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objContactoEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objContactoEF.fechaModificacion = DateTime.Now;
                objContactoEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.HISTORIAL;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha enviado el contacto a historial con éxito.");

                // SAVE BITACORA
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActivarContacto(ContactoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al activar el contacto.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ACTIVAR CONTACTO
                tblAF_CRM_Contactos objContactoEF = _context.tblAF_CRM_Contactos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objContactoEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objContactoEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objContactoEF.fechaModificacion = DateTime.Now;
                objContactoEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha activado el contacto con éxito.");

                // SAVE BITACORA
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region PROSPECTOS CLIENTES
        public Dictionary<string, object> GetProspectosClientes(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_EstatusHistorial <= 0) { throw new Exception("Es necesario seleccionar el estatus."); }
                #endregion

                #region PROSPECTO INACTIVO
                EnviarProspectosHistorial();
                #endregion

                #region CATALOGOS
                List<DivisionDTO> lstDivisionesDTO = GetListaDivisiones().ToList();
                List<ClienteDTO> lstClientesDTO = GetListaClientes().ToList();
                List<MunicipioDTO> lstMunicipiosDTO = GetListaMunicipios().ToList();
                List<EstadoDTO> lstEstadosDTO = GetListaEstados().ToList();
                List<PaisDTO> lstPaisesDTO = GetListaPaises().ToList();
                List<CanalDTO> lstCanalesDTO = GetListaCanales().ToList();
                #endregion

                List<ProyectoDTO> lstProyectosDTO = GetListaProyectos(new ProyectoDTO
                {
                    esProspecto = true,
                    FK_Division = objParamsDTO.FK_Division,
                    FK_Cliente = objParamsDTO.FK_Cliente,
                    FK_EstatusHistorial = objParamsDTO.FK_EstatusHistorial
                }).ToList();

                foreach (var item in lstProyectosDTO)
                {
                    #region INIT INFORMACIÓN
                    DivisionDTO objDivisionDTO = lstDivisionesDTO.Where(w => w.id == item.FK_Division).FirstOrDefault();
                    if (objDivisionDTO == null)
                        throw new Exception("Ocurrió un error al obtener la información de la división.");

                    ClienteDTO objClienteDTO = lstClientesDTO.Where(w => w.id == item.FK_Cliente).FirstOrDefault();
                    if (objClienteDTO == null)
                        throw new Exception("Ocurrió un error al obtener la información del cliente.");

                    CanalDTO objCanalDTO = lstCanalesDTO.Where(w => w.id == item.FK_Canal).FirstOrDefault();
                    if (objCanalDTO == null)
                        throw new Exception("Ocurrió un error al obtener la información del canal.");
                    #endregion

                    #region LISTA PROSPECTOS
                    item.division = string.Format("[{0}] {1}", objDivisionDTO.numDivision, objDivisionDTO.division);
                    item.nombreCliente = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(objClienteDTO.nombreCliente);
                    item.tipoCliente = EnumHelper.GetDescription((TipoClientesEnum)objClienteDTO.FK_TipoCliente);
                    item.ubicacion = GetUbicacion(item.FK_Municipio, lstMunicipiosDTO, lstEstadosDTO, lstPaisesDTO);
                    item.strImporteCotizadoAprox = item.importeCotizadoAprox.ToString("C");
                    item.canal = objCanalDTO.canal;
                    #endregion
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstProyectosDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearProspectoCliente(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearEditarProspectoCliente = ValidacionesCrearEditarProspectoCliente(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarProspectoCliente[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarProspectoCliente[MESSAGE]);
                #endregion

                #region CREAR PROSPECTO
                tblAF_CRM_Proyectos objCrearProspecto = new tblAF_CRM_Proyectos();
                objCrearProspecto.FK_Division = objParamsDTO.FK_Division;
                objCrearProspecto.nombreProyecto = objParamsDTO.nombreProyecto;
                objCrearProspecto.FK_Cliente = objParamsDTO.FK_Cliente;
                objCrearProspecto.FK_Municipio = objParamsDTO.FK_Municipio;
                objCrearProspecto.importeCotizadoAprox = objParamsDTO.importeCotizadoAprox;
                objCrearProspecto.fechaInicio = objParamsDTO.fechaInicio;
                objCrearProspecto.FK_Canal = objParamsDTO.FK_Canal;
                objCrearProspecto.esProspecto = true;
                objCrearProspecto.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                objCrearProspecto.fechaCreacion = DateTime.Now;
                objCrearProspecto.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearProspecto.registroActivo = true;
                _context.tblAF_CRM_Proyectos.Add(objCrearProspecto);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito el prospecto.");
                SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                string MENSAJE_ERROR_GENERAL = "Ocurrió un error al actualizar la información del prospecto.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                Dictionary<string, object> resValidacionesCrearEditarProspectoCliente = ValidacionesCrearEditarProspectoCliente(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarProspectoCliente[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarProspectoCliente[MESSAGE]);
                #endregion

                #region ACTUALIZAR PROSPECTO CLIENTE
                tblAF_CRM_Proyectos objActualizarProspecto = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                if (objActualizarProspecto == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objActualizarProspecto.FK_Division = objParamsDTO.FK_Division;
                objActualizarProspecto.nombreProyecto = objParamsDTO.nombreProyecto;
                objActualizarProspecto.FK_Cliente = objParamsDTO.FK_Cliente;
                objActualizarProspecto.FK_Municipio = objParamsDTO.FK_Municipio;
                objActualizarProspecto.importeCotizadoAprox = objParamsDTO.importeCotizadoAprox;
                objActualizarProspecto.fechaInicio = objParamsDTO.fechaInicio;
                objActualizarProspecto.FK_Canal = objParamsDTO.FK_Canal;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha actualizado con éxito el prospecto.");
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEditarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Division <= 0) { throw new Exception("Es necesario seleccionar una división."); }
                if (string.IsNullOrEmpty(objParamsDTO.nombreProyecto)) { throw new Exception("Es necesario indicar el nombre del proyecto."); }
                if (objParamsDTO.FK_Cliente <= 0) { throw new Exception("Es necesario seleccionar un cliente."); }
                if (objParamsDTO.FK_Municipio <= 0) { throw new Exception("Es necesario seleccionar un municipio."); }
                if (objParamsDTO.importeCotizadoAprox == 0) { throw new Exception("Es necesario indicar el importe cotizado aproximado."); }
                if (objParamsDTO.importeCotizadoAprox <= -1) { throw new Exception("Es necesario indicar el importe con valor positivo."); }
                if (objParamsDTO.fechaInicio == null) { throw new Exception("Es necesario indicar la fecha de inicio."); }
                if (objParamsDTO.FK_Canal <= 0) { throw new Exception("Es necesario seleccionar un canal."); }
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el prospecto.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR PROSPECTO CLIENTE
                tblAF_CRM_Proyectos objEliminarProspecto = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                if (objEliminarProspecto == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objEliminarProspecto.fechaModificacion = DateTime.Now;
                objEliminarProspecto.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objEliminarProspecto.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito el prospecto.");
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarProspectoCliente(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener la información del prospecto cliente.";

                #region VALIDACIONES
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region INFO PROSPECTO CLIENTE
                ProyectoDTO objProyectoDTO = GetListaProyectos(new ProyectoDTO { esProspecto = true, id = objParamsDTO.id }).FirstOrDefault();
                if (objProyectoDTO == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objProyectoDTO.FK_Estado = GetListaMunicipios(new MunicipioDTO { idMunicipio = objProyectoDTO.FK_Municipio }).Select(s => s.idEstado).FirstOrDefault();
                objProyectoDTO.FK_Pais = GetListaEstados(new EstadoDTO { idEstado = objProyectoDTO.FK_Estado }).Select(s => s.idPais).FirstOrDefault();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objProyectoDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboFiltro_Prospectos_Divisiones()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstFK_DivisionesUtilizadas = GetListaProyectos(new ProyectoDTO { esProspecto = true }).Select(s => s.FK_Division).ToList();
                List<DivisionDTO> lstDivisionesDTO = new List<DivisionDTO>();
                if (lstFK_DivisionesUtilizadas.Count() > 0)
                    lstDivisionesDTO = GetListaDivisiones(new DivisionDTO { lstID_Divisiones = lstFK_DivisionesUtilizadas }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstDivisionesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = string.Format("[{0}] {1}", item.numDivision, item.division);
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboFiltro_Prospectos_Clientes()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<int> lstFK_ClientesUtilizados = GetListaProyectos(new ProyectoDTO { esProspecto = true }).Select(s => s.FK_Cliente).ToList();
                List<ClienteDTO> lstClientesDTO = new List<ClienteDTO>();
                if (lstFK_ClientesUtilizados.Count() > 0)
                    lstClientesDTO = GetListaClientes(new ClienteDTO { lstID_Clientes = lstFK_ClientesUtilizados }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstClientesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(item.nombreCliente);
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarProspectoHistorial(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al enviar el prospecto a historial.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ENVIAR A HISTORIAL
                tblAF_CRM_Proyectos objProspectoEF = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objProspectoEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objProspectoEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objProspectoEF.fechaModificacion = DateTime.Now;
                objProspectoEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.HISTORIAL;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha enviado el prospecto a historial con éxito.");

                // SAVE BITACORA
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActivarProspecto(ProyectoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al activar el prospecto.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ACTIVAR PROSPECTO
                tblAF_CRM_Proyectos objProspectoEF = _context.tblAF_CRM_Proyectos.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objProspectoEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objProspectoEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objProspectoEF.fechaModificacion = DateTime.Now;
                objProspectoEF.FK_EstatusHistorial = (int)EstatusHistorialEnum.ACTIVO;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha activado el prospecto con éxito.");

                // SAVE BITACORA
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private void EnviarProspectosHistorial()
        {
            try
            {
                List<tblAF_CRM_Proyectos> lstProyectosEF = _context.tblAF_CRM_Proyectos.Where(w => w.FK_EstatusHistorial == (int)EstatusHistorialEnum.ACTIVO && w.esProspecto && w.registroActivo).ToList();
                bool hayProspectosInactivos = false;
                foreach (var item in lstProyectosEF)
                {
                    int cantDiasTranscurridos = (int)(DateTime.Now - item.fechaCreacion).TotalDays + 1;
                    if (cantDiasTranscurridos >= 14)
                    {
                        hayProspectosInactivos = true;
                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.FK_EstatusHistorial = (int)EstatusHistorialEnum.HISTORIAL;
                    }
                }

                if (hayProspectosInactivos)
                {
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(null));
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, 0, null);
                GlobalUtils.sendEmail(string.Format("{0}: Prospectos clientes", PersonalUtilities.GetNombreEmpresa()), "Error al enviar los prospectos inactivos a historial", new List<string> { _CORREO_OMAR_NUNEZ });
            }
        }

        public Dictionary<string, object> EnviarProspectosProyecto(List<ProyectoDTO> lstProspectosDTO)
        {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    Dictionary<string, object> resValidacionesCrearEnviarProspectosProyecto = ValidacionesCrearEnviarProspectosProyecto(lstProspectosDTO);
                    if (!(bool)resValidacionesCrearEnviarProspectosProyecto[SUCCESS])
                        throw new Exception((string)resValidacionesCrearEnviarProspectosProyecto[MESSAGE]);
                    #endregion

                    using (var dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        #region PROSPECTO A PROYECTO
                        try
                        {
                            List<int> lstProspectosID = lstProspectosDTO.Select(s => s.id).ToList();
                            List<tblAF_CRM_Proyectos> lstProyectosEF = _context.tblAF_CRM_Proyectos.Where(w => lstProspectosID.Contains(w.id) && w.esProspecto && w.registroActivo).ToList();
                            foreach (var item in lstProyectosEF)
                            {
                                ProyectoDTO objProyectoDTO = lstProspectosDTO.Where(w => w.id == item.id).FirstOrDefault();
                                if (objProyectoDTO == null)
                                    throw new Exception("Ocurrió un error al enviar el prospecto a historial.");

                                item.esProspecto = false;
                                item.FK_Prioridad = objProyectoDTO.FK_Prioridad;
                                item.FK_Estatus = objProyectoDTO.FK_Estatus;
                                item.FK_Escenario = objProyectoDTO.FK_Escenario;
                                item.FK_UsuarioResponsable = _JOSE_PEDRO_GONZALEZ_ID;
                                item.FK_Riesgo = _FK_RIESGO_SIN_DEFINIR;
                                item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                item.fechaModificacion = DateTime.Now;
                            }
                            _context.SaveChanges();
                            dbContextTransaction.Commit();

                            SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(lstProspectosDTO));
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            throw new Exception(e.Message);
                        }
                        #endregion
                    }

                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, string.Format("Se ha enviado {0} a proyectos", lstProspectosDTO.Count() > 1 ? "los prospectos" : "al prospecto"));
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, lstProspectosDTO);
                    resultado = new Dictionary<string, object>();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEnviarProspectosProyecto(List<ProyectoDTO> lstProspectosDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al enviar el prospecto a proyecto.";
                foreach (var item in lstProspectosDTO)
                {
                    if (item.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    if (item.FK_Prioridad <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    if (item.FK_Estatus <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                    if (item.FK_Escenario <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, lstProspectosDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region CANALES
        public Dictionary<string, object> GetCanales(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<CanalDivisionDTO> lstCanalesDivisionesDTO = GetListaCanalesDivisiones().ToList();
                List<DivisionDTO> lstDivisionesDTO = GetListaDivisiones().ToList();

                List<CanalDTO> lstCanalesDTO = GetListaCanales().ToList();
                foreach (var item in lstCanalesDTO)
                {
                    foreach (var item2 in lstCanalesDivisionesDTO.Where(w => w.FK_Canal == item.id).ToList())
                    {
                        item.htmlDivisiones +=
                            string.Format("<button class='btn btn-xs btn-primary divCanalDivision'><b title='{0}'>{0}</b></button>&nbsp;",
                            lstDivisionesDTO.Where(w => w.id == item2.FK_Division).Select(s => s.division).FirstOrDefault());
                    }

                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstCanalesDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearCanal(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearEditarCanal = ValidacionesCrearEditarCanal(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarCanal[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarCanal[MESSAGE]);
                #endregion

                #region CREAR CANAL
                tblAF_CRM_CatCanales objCrearCanalEF = new tblAF_CRM_CatCanales();
                objCrearCanalEF.canal = objParamsDTO.canal.Trim();
                objCrearCanalEF.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearCanalEF.fechaCreacion = DateTime.Now;
                objCrearCanalEF.registroActivo = true;
                _context.tblAF_CRM_CatCanales.Add(objCrearCanalEF);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito el canal.");
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarCanal(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al actualizar el canal";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                Dictionary<string, object> resValidacionesCrearEditarCanal = ValidacionesCrearEditarCanal(objParamsDTO);
                if (!(bool)resValidacionesCrearEditarCanal[SUCCESS])
                    throw new Exception((string)resValidacionesCrearEditarCanal[MESSAGE]);
                #endregion

                #region ACTUALIZAR CANAL
                tblAF_CRM_CatCanales objActualizarCanalEF = _context.tblAF_CRM_CatCanales.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objActualizarCanalEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objActualizarCanalEF.canal = objParamsDTO.canal.Trim();
                objActualizarCanalEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objActualizarCanalEF.fechaModificacion = DateTime.Now;
                _context.tblAF_CRM_CatCanales.Add(objActualizarCanalEF);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha actualizado con éxito el canal.");
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearEditarCanal(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrEmpty(objParamsDTO.canal)) { throw new Exception("Es necesario indicar el canal."); }

                if (objParamsDTO.id == _FK_CANAL_SIN_DEFINIR)
                    throw new Exception("El sistema no permite actualizar el canal seleccionado.");

                if (GetListaCanales(new CanalDTO { canal = objParamsDTO.canal }).Any())
                    throw new Exception("El canal ya se encuentra registrado.");

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCanal(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el canal";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                if (objParamsDTO.id == _FK_CANAL_SIN_DEFINIR) { throw new Exception("El sistema no permite eliminar el canal seleccionado."); }
                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        #region ELIMINAR CANAL
                        tblAF_CRM_CatCanales objEliminarCanalEF = _context.tblAF_CRM_CatCanales.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                        if (objEliminarCanalEF == null)
                            throw new Exception(MENSAJE_ERROR_GENERAL);

                        objEliminarCanalEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objEliminarCanalEF.fechaModificacion = DateTime.Now;
                        objEliminarCanalEF.registroActivo = false;
                        _context.SaveChanges();
                        #endregion

                        #region ELIMINAR CANAL DIVISIONES
                        Dictionary<string, object> resEliminarCanalDivisiones = EliminarCanalDivisiones(new CanalDivisionDTO { FK_Canal = objParamsDTO.id });
                        if (!(bool)resEliminarCanalDivisiones[SUCCESS])
                            throw new Exception((string)resEliminarCanalDivisiones[MESSAGE]);
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el canal.");
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito el canal.");

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCanal(CanalDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                CanalDTO objCanalDTO = GetListaCanales(new CanalDTO { id = objParamsDTO.id }).FirstOrDefault();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, objCanalDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> EliminarCanalDivisiones(CanalDivisionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region DIVISIONES
                if (objParamsDTO.FK_Canal <= 0) { throw new Exception("Ocurrió un error al eliminar las divisiones del canal."); }
                #endregion

                List<tblAF_CRM_CatCanalesDivisiones> lstCanalDivisiones = _context.tblAF_CRM_CatCanalesDivisiones.Where(w => w.FK_Canal == objParamsDTO.FK_Canal && w.registroActivo).ToList();
                foreach (var item in lstCanalDivisiones)
                {
                    item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    item.fechaModificacion = DateTime.Now;
                    item.registroActivo = false;
                }

                _context.SaveChanges();
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        #region CANALES DIVISIONES
        public Dictionary<string, object> GetCanalesDivisiones(CanalDivisionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objParamsDTO.FK_Canal <= 0) { throw new Exception("Ocurrió un error al obtener el listado de divisiones del canal."); }
                #endregion

                List<CanalDTO> lstCanalesDTO = GetListaCanales().ToList();
                List<CanalDivisionDTO> lstCanalesDivisionesDTO = GetListaCanalesDivisiones(new CanalDivisionDTO { FK_Canal = objParamsDTO.FK_Canal }).ToList();
                List<DivisionDTO> lstDivisionesDTO = GetListaDivisiones(new DivisionDTO { lstID_Divisiones = lstCanalesDivisionesDTO.Select(s => s.FK_Division).ToList() });
                foreach (var item in lstCanalesDivisionesDTO)
                {
                    item.canal = lstCanalesDTO.Where(w => w.id == item.FK_Canal).Select(s => s.canal).FirstOrDefault() ?? string.Empty;
                    item.division = lstDivisionesDTO.Where(w => w.id == item.FK_Division).Select(s => s.division).FirstOrDefault() ?? string.Empty;
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstCanalesDivisionesDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                Dictionary<string, object> resValidacionesCrearCanalDivision = ValidacionesCrearCanalDivision(objParamsDTO);
                if (!(bool)resValidacionesCrearCanalDivision[SUCCESS])
                    throw new Exception((string)resValidacionesCrearCanalDivision[MESSAGE]);
                #endregion

                #region CREAR CANAL DIVISION
                tblAF_CRM_CatCanalesDivisiones objCrearCanalDivisionEF = new tblAF_CRM_CatCanalesDivisiones();
                objCrearCanalDivisionEF.FK_Canal = objParamsDTO.FK_Canal;
                objCrearCanalDivisionEF.FK_Division = objParamsDTO.FK_Division;
                objCrearCanalDivisionEF.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objCrearCanalDivisionEF.fechaCreacion = DateTime.Now;
                objCrearCanalDivisionEF.registroActivo = true;
                _context.tblAF_CRM_CatCanalesDivisiones.Add(objCrearCanalDivisionEF);
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha registrado con éxito la división en el canal.");
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> ValidacionesCrearCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (objParamsDTO.FK_Canal <= 0) { throw new Exception("Ocurrió un error al registrar la división en el canal."); }
                if (objParamsDTO.FK_Division <= 0) { throw new Exception("Es necesario seleccionar la división."); }

                if (GetListaCanalesDivisiones(new CanalDivisionDTO { FK_Canal = objParamsDTO.FK_Canal, FK_Division = objParamsDTO.FK_Division }).Any())
                    throw new Exception("Ya se encuentra registrado la división en el canal seleccionado.");

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCanalDivision(CanalDivisionDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar la división del canal.";
                if (objParamsDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                #region ELIMINAR DIVISIÓN DEL CANAL
                tblAF_CRM_CatCanalesDivisiones objEliminarCanalDivisionEF = _context.tblAF_CRM_CatCanalesDivisiones.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                if (objEliminarCanalDivisionEF == null)
                    throw new Exception(MENSAJE_ERROR_GENERAL);

                objEliminarCanalDivisionEF.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                objEliminarCanalDivisionEF.fechaModificacion = DateTime.Now;
                objEliminarCanalDivisionEF.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito la división del canal.");

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region TRACKING VENTAS
        public Dictionary<string, object> GetTrackingVentas(TrackingVentaDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ProyectoDTO> lstProyectosDTO = GetListaProyectos(new ProyectoDTO
                {
                    FK_Cliente = objParamsDTO.FK_Cliente,
                    FK_Division = objParamsDTO.FK_Division,
                    FK_UsuarioResponsable = objParamsDTO.FK_UsuarioResponsable,
                    FK_Prioridad = objParamsDTO.FK_Prioridad,
                    esProspecto = false
                }).ToList();

                List<TrackingVentaDTO> lstTrackingVentasDTO = new List<TrackingVentaDTO>();
                if (lstProyectosDTO.Count() > 0)
                {
                    #region CATALOGOS
                    List<DivisionDTO> lstDivisionesDTO = GetListaDivisiones(new DivisionDTO { id = objParamsDTO.FK_Division > 0 ? objParamsDTO.FK_Division : 0 }).ToList();
                    List<PrioridadDTO> lstPrioridadesDTO = GetListaPrioridades(new PrioridadDTO { id = objParamsDTO.FK_Prioridad > 0 ? objParamsDTO.FK_Prioridad : 0 }).ToList();
                    List<ClienteDTO> lstClientesDTO = GetListaClientes(new ClienteDTO { id = objParamsDTO.FK_Cliente > 0 ? objParamsDTO.FK_Cliente : 0 }).ToList();
                    List<PrioridadEstatusDTO> lstPrioridadesEstatusDTO = GetListaPrioridadEstatus().ToList();
                    List<ProximaAccionDTO> lstProximasAccionesDTO = GetListaProximasAcciones().ToList();
                    List<UsuarioDTO> lstUsuariosDTO = GetListaUsuarios(new UsuarioDTO { id = objParamsDTO.FK_UsuarioResponsable > 0 ? objParamsDTO.FK_UsuarioResponsable : 0 }).ToList();
                    List<RiesgoDTO> lstRiesgosDTO = GetListaRiesgos().ToList();
                    List<MunicipioDTO> lstMunicipiosDTO = GetListaMunicipios().ToList();
                    List<EstadoDTO> lstEstadosDTO = GetListaEstados().ToList();
                    List<PaisDTO> lstPaisesDTO = GetListaPaises().ToList();
                    #endregion

                    lstTrackingVentasDTO = new List<TrackingVentaDTO>();
                    TrackingVentaDTO objTrackingVentasDTO = new TrackingVentaDTO();
                    foreach (var item in lstProyectosDTO)
                    {
                        #region INIT INFORMACIÓN
                        DivisionDTO objDivisionDTO = lstDivisionesDTO.Where(w => w.id == item.FK_Division).FirstOrDefault();
                        if (objDivisionDTO == null)
                            throw new Exception("Ocurrió un error al obtener la división del proyecto.");

                        PrioridadDTO objPrioridadDTO = lstPrioridadesDTO.Where(w => w.id == item.FK_Prioridad).FirstOrDefault();
                        if (objPrioridadDTO == null)
                            throw new Exception("Ocurrió un error al obtener la prioridad del proyecto.");

                        ClienteDTO objClienteDTO = lstClientesDTO.Where(w => w.id == item.FK_Cliente).FirstOrDefault();
                        if (objClienteDTO == null)
                            throw new Exception("Ocurrió un error al obtener el cliente del proyecto.");

                        string nombreResponsableAccion = string.Empty;
                        ProximaAccionDTO objProximaAccionDTO = lstProximasAccionesDTO.Where(w => w.FK_Proyecto == item.id && !w.accionFinalizada).OrderByDescending(o => o.fechaCreacion).FirstOrDefault();
                        if (objProximaAccionDTO != null)
                        {
                            UsuarioDTO objUsuarioResponsableAccion = lstUsuariosDTO.Where(w => w.id == objProximaAccionDTO.FK_UsuarioResponsable).FirstOrDefault();
                            if (objUsuarioResponsableAccion != null)
                                nombreResponsableAccion = string.Format("{0} {1} {2}", objUsuarioResponsableAccion.nombre, objUsuarioResponsableAccion.apellidoPaterno, objUsuarioResponsableAccion.apellidoMaterno);
                        }

                        string nombreResponsableProyecto = string.Empty;
                        UsuarioDTO objUsuarioResponsableProyectoDTO = lstUsuariosDTO.Where(w => w.id == item.FK_UsuarioResponsable).FirstOrDefault();
                        if (objUsuarioResponsableProyectoDTO == null)
                            throw new Exception("Ocurrió un error al obtener el nombre del responsable de proyecto.");
                        else
                            nombreResponsableProyecto = string.Format("{0} {1} {2}", objUsuarioResponsableProyectoDTO.nombre, objUsuarioResponsableProyectoDTO.apellidoPaterno, objUsuarioResponsableProyectoDTO.apellidoMaterno);

                        RiesgoDTO objRiesgoDTO = lstRiesgosDTO.Where(w => w.id == item.FK_Riesgo).FirstOrDefault();
                        if (objRiesgoDTO == null)
                            throw new Exception("Ocurrió un error al obtener el riesgo indicado en el proyecto.");
                        #endregion

                        objTrackingVentasDTO = new TrackingVentaDTO();
                        objTrackingVentasDTO.id = item.id;
                        objTrackingVentasDTO.division = string.Format("[{0}] {1}", objDivisionDTO.numDivision, objDivisionDTO.division);
                        objTrackingVentasDTO.esc = string.Empty;
                        objTrackingVentasDTO.prioridad = PersonalUtilities.PrimerLetraMayuscula(objPrioridadDTO.prioridad);
                        objTrackingVentasDTO.nombreCliente = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(objClienteDTO.nombreCliente);
                        objTrackingVentasDTO.nombreProyecto = item.nombreProyecto;
                        objTrackingVentasDTO.ubicacion = GetUbicacion(item.FK_Municipio, lstMunicipiosDTO, lstEstadosDTO, lstPaisesDTO);
                        objTrackingVentasDTO.estatusActual = string.Empty;
                        objTrackingVentasDTO.proximaAccion = objProximaAccionDTO != null ? objProximaAccionDTO.accion : string.Empty;
                        objTrackingVentasDTO.strFechaProximaAccion = objProximaAccionDTO != null ?
                            string.Format("{0}/{1}/{2}", objProximaAccionDTO.fechaProximaAccion.Day, objProximaAccionDTO.fechaProximaAccion.Month, objProximaAccionDTO.fechaProximaAccion.Year) : string.Empty;
                        objTrackingVentasDTO.nombreResponsableProyecto = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(nombreResponsableProyecto);
                        objTrackingVentasDTO.nombreResponsableAccion = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(nombreResponsableAccion);
                        objTrackingVentasDTO.riesgo = objRiesgoDTO.riesgo;
                        objTrackingVentasDTO.estatus = string.Empty;
                        objTrackingVentasDTO.porcCumplimiento = string.Empty;
                        objTrackingVentasDTO.go = string.Empty;
                        objTrackingVentasDTO.get = string.Empty;
                        lstTrackingVentasDTO.Add(objTrackingVentasDTO);
                    }
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstTrackingVentasDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region GENERAL
        #region PUBLIC
        public Dictionary<string, object> FillCboClientes()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in GetListaClientes().ToList())
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = PersonalUtilities.PrimerLetraMayuscula(item.nombreCliente);
                    lstComboDTO.Add(objComboDTO);
                };

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPaises()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPaises = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idPais AS VALUE, Pais AS TEXT FROM tblP_Pais WHERE registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { registroActivo = true }
                }).ToList();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPaises);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEstados(int FK_Pais)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEstados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idEstado AS VALUE, Estado AS TEXT FROM tblP_Estado WHERE idPais = @FK_Pais AND registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { FK_Pais = FK_Pais, registroActivo = true }
                }).ToList();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEstados);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, new { FK_Pais = FK_Pais });
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboMunicipios(int FK_Estado)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstMunicipios = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idMunicipio AS VALUE, Municipio AS TEXT FROM tblP_Municipios WHERE idEstado = @FK_Estado AND registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { FK_Estado = FK_Estado, registroActivo = true }
                }).ToList();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstMunicipios);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, new { FK_Estado = FK_Estado });
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboDivisiones()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO DIVISIONES
                List<DivisionDTO> lstDivisiones = GetListaDivisiones().ToList();

                List<ComboDTO> lstDivisionesDTO = new List<ComboDTO>();
                ComboDTO objDivisionDTO = new ComboDTO();
                foreach (var item in lstDivisiones)
                {
                    objDivisionDTO = new ComboDTO();
                    objDivisionDTO.Value = item.id.ToString();
                    objDivisionDTO.Text = string.Format("[{0}] {1}", item.numDivision, item.division);
                    objDivisionDTO.Prefijo = item.FK_UsuarioResponsable.ToString();
                    lstDivisionesDTO.Add(objDivisionDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstDivisionesDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoClientes()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstTipoClientes = new List<ComboDTO>();
                ComboDTO objTipoCliente = new ComboDTO();
                objTipoCliente.Value = Convert.ToInt32(TipoClientesEnum.P).ToString();
                objTipoCliente.Text = EnumHelper.GetDescription((TipoClientesEnum.P));
                lstTipoClientes.Add(objTipoCliente);

                objTipoCliente = new ComboDTO();
                string NP = TipoClientesEnum.NP.ToString();
                objTipoCliente.Value = Convert.ToInt32(TipoClientesEnum.NP).ToString();
                objTipoCliente.Text = EnumHelper.GetDescription((TipoClientesEnum.NP));
                lstTipoClientes.Add(objTipoCliente);

                objTipoCliente = new ComboDTO();
                objTipoCliente.Value = Convert.ToInt32(TipoClientesEnum.S).ToString();
                objTipoCliente.Text = EnumHelper.GetDescription((TipoClientesEnum.S));
                lstTipoClientes.Add(objTipoCliente);

                objTipoCliente = new ComboDTO();
                string NS = TipoClientesEnum.NS.ToString();
                objTipoCliente.Value = Convert.ToInt32(TipoClientesEnum.NS).ToString();
                objTipoCliente.Text = EnumHelper.GetDescription((TipoClientesEnum.NS));
                lstTipoClientes.Add(objTipoCliente);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstTipoClientes);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboRiesgos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO RIESGOS
                List<ComboDTO> lstRiesgos = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS VALUE, riesgo AS TEXT FROM tblAF_CRM_CatRiesgos WHERE registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { registroActivo = true }
                }).ToList();

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstRiesgos);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCanales()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<CanalDTO> lstCanalesDTO = GetListaCanales().ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCanalesDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = item.canal;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboHistorial()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = Convert.ToInt32(EstatusHistorialEnum.ACTIVO).ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusHistorialEnum.ACTIVO));
                lstComboDTO.Add(objComboDTO);

                objComboDTO = new ComboDTO();
                objComboDTO.Value = Convert.ToInt32(EstatusHistorialEnum.HISTORIAL).ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusHistorialEnum.HISTORIAL));
                lstComboDTO.Add(objComboDTO);

                resultado = new Dictionary<string, object>();
                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<UsuarioDTO> lstUsuariosDTO = GetListaUsuarios().OrderBy(o => o.nombre).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach(var item in lstUsuariosDTO)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", item.nombre, item.apellidoPaterno, item.apellidoMaterno));
                    lstComboDTO.Add(objComboDTO);
                };

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboMenus()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstMenus = new List<ComboDTO>();
                ComboDTO objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.DASHBOARD).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.DASHBOARD));
                lstMenus.Add(objMenu);

                objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.CLIENTES).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.CLIENTES));
                lstMenus.Add(objMenu);

                objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.PROSPECTOS).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.PROSPECTOS));
                lstMenus.Add(objMenu);

                objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.CANALES).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.CANALES));
                lstMenus.Add(objMenu);

                objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.TRACKING_VENTAS).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.TRACKING_VENTAS));
                lstMenus.Add(objMenu);

                objMenu = new ComboDTO();
                objMenu.Value = Convert.ToInt32(MenuEnum.USUARIOS_CRM).ToString();
                objMenu.Text = EnumHelper.GetDescription((MenuEnum.USUARIOS_CRM));
                lstMenus.Add(objMenu);

                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstMenus);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region PRIVATE
        private List<ClienteDTO> GetListaClientes(ClienteDTO objParamsDTO = null)
        {
            List<ClienteDTO> lstClientesDTO = new List<ClienteDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_Clientes WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Division > 0 ? string.Format(" AND FK_Division = {0}", objParamsDTO.FK_Division) : string.Empty;
                    strQuery += objParamsDTO.lstID_Clientes.Count() > 0 ? string.Format(" AND id IN ({0})", string.Join(",", objParamsDTO.lstID_Clientes)) : string.Empty;
                    strQuery += objParamsDTO.FK_EstatusHistorial > 0 ? string.Format(" AND FK_EstatusHistorial = {0}", objParamsDTO.FK_EstatusHistorial) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstClientesDTO = _context.Select<ClienteDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstClientesDTO;
        }

        private List<DivisionDTO> GetListaDivisiones(DivisionDTO objParamsDTO = null)
        {
            List<DivisionDTO> lstDivisiones = new List<DivisionDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_Divisiones WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.lstID_Divisiones.Count() > 0 ? string.Format(" AND id IN ({0})", string.Join(",", objParamsDTO.lstID_Divisiones)) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstDivisiones = _context.Select<DivisionDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstDivisiones;
        }

        private List<MunicipioDTO> GetListaMunicipios(MunicipioDTO objParamsDTO = null)
        {
            List<MunicipioDTO> lstMunicipios = new List<MunicipioDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblP_Municipios WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.idMunicipio > 0 ? string.Format(" AND idMunicipio = {0}", objParamsDTO.idMunicipio) : string.Empty;
                #endregion

                #region CONSULTA
                lstMunicipios = _context.Select<MunicipioDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.idMunicipio, objParamsDTO);
            }
            return lstMunicipios;
        }

        private List<EstadoDTO> GetListaEstados(EstadoDTO objParamsDTO = null)
        {
            List<EstadoDTO> lstEstados = new List<EstadoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblP_Estado WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.idEstado > 0 ? string.Format(" AND idEstado = {0}", objParamsDTO.idEstado) : string.Empty;
                #endregion

                #region CONSULTA
                lstEstados = _context.Select<EstadoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.idEstado, objParamsDTO);
            }
            return lstEstados;
        }

        private List<PaisDTO> GetListaPaises(PaisDTO objParamsDTO = null)
        {
            List<PaisDTO> lstPaises = new List<PaisDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblP_Pais WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.idPais > 0 ? string.Format(" AND idPais = {0}", objParamsDTO.idPais) : string.Empty;
                #endregion

                #region CONSULTA
                lstPaises = _context.Select<PaisDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.idPais, objParamsDTO);
            }
            return lstPaises;
        }

        private List<PrioridadEstatusDTO> GetListaPrioridadEstatus(PrioridadEstatusDTO objParamsDTO = null)
        {
            List<PrioridadEstatusDTO> lstPrioridadEstatus = new List<PrioridadEstatusDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_CatPrioridadEstatus WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Prioridad > 0 ? string.Format(" AND FK_Prioridad = {0}", objParamsDTO.FK_Prioridad) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstPrioridadEstatus = _context.Select<PrioridadEstatusDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstPrioridadEstatus;
        }

        private List<PrioridadDTO> GetListaPrioridades(PrioridadDTO objParamsDTO = null)
        {
            List<PrioridadDTO> lstPrioridades = new List<PrioridadDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_CatPrioridades WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.lstID_Prioridades.Count() > 0 ? string.Format(" AND id IN ({0})", string.Join(",", objParamsDTO.lstID_Prioridades)) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstPrioridades = _context.Select<PrioridadDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstPrioridades;
        }

        private List<RiesgoDTO> GetListaRiesgos(RiesgoDTO objParamsDTO = null)
        {
            List<RiesgoDTO> lstRiesgos = new List<RiesgoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_CatRiesgos WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstRiesgos = _context.Select<RiesgoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstRiesgos;
        }

        private List<UsuarioDTO> GetListaUsuarios(UsuarioDTO objParamsDTO = null)
        {
            List<UsuarioDTO> lstUsuarios = new List<UsuarioDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblP_Usuario WHERE estatus = @estatus";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.lstID_Usuarios.Count() > 0 ? string.Format(" AND id IN ({0})", string.Join(",", objParamsDTO.lstID_Usuarios)) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstUsuarios = _context.Select<UsuarioDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { estatus = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstUsuarios;
        }

        private List<ProyectoDTO> GetListaProyectos(ProyectoDTO objParamsDTO = null)
        {
            List<ProyectoDTO> lstProyectos = new List<ProyectoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_Proyectos WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Cliente > 0 ? string.Format(" AND FK_Cliente = {0}", objParamsDTO.FK_Cliente) : string.Empty;
                    strQuery += objParamsDTO.FK_Division > 0 ? string.Format(" AND FK_Division = {0}", objParamsDTO.FK_Division) : string.Empty;
                    strQuery += objParamsDTO.FK_UsuarioResponsable > 0 ? string.Format(" AND FK_UsuarioResponsable = {0}", objParamsDTO.FK_UsuarioResponsable) : string.Empty;
                    strQuery += objParamsDTO.FK_Prioridad > 0 ? string.Format(" AND FK_Prioridad = {0}", objParamsDTO.FK_Prioridad) : string.Empty;
                    strQuery += string.Format(" AND esProspecto = {0}", objParamsDTO.esProspecto ? 1 : 0);
                    strQuery += objParamsDTO.FK_EstatusHistorial > 0 ? string.Format(" AND FK_EstatusHistorial = {0}", objParamsDTO.FK_EstatusHistorial) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstProyectos = _context.Select<ProyectoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstProyectos;
        }

        private List<ContactoDTO> GetListaContactos(ContactoDTO objParamsDTO = null)
        {
            List<ContactoDTO> lstContactos = new List<ContactoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_Contactos WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.lstFK_Clientes.Count() > 0 ? string.Format(" AND FK_Cliente IN ({0})", string.Join(",", objParamsDTO.lstFK_Clientes)) : string.Empty;
                    strQuery += objParamsDTO.FK_Cliente > 0 ? string.Format(" AND FK_Cliente = {0}", objParamsDTO.FK_Cliente) : string.Empty;
                    strQuery += objParamsDTO.FK_EstatusHistorial > 0 ? string.Format(" AND FK_EstatusHistorial = {0}", objParamsDTO.FK_EstatusHistorial) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstContactos = _context.Select<ContactoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstContactos;
        }

        private string GetUbicacion(int FK_Municipio, List<MunicipioDTO> lstMunicipios, List<EstadoDTO> lstEstados, List<PaisDTO> lstPaises)
        {
            string ubicacion = string.Empty;
            try
            {
                MunicipioDTO objMunicipio = lstMunicipios.Where(w => w.idMunicipio == FK_Municipio).FirstOrDefault();
                if (objMunicipio == null)
                    throw new Exception("Ocurrió un error al obtener el municipio.");

                EstadoDTO objEstado = lstEstados.Where(w => w.idEstado == objMunicipio.idEstado).FirstOrDefault();
                if (objEstado == null)
                    throw new Exception("Ocurrió un error al obtener el estado.");

                PaisDTO objPais = lstPaises.Where(w => w.idPais == objEstado.idPais).FirstOrDefault();
                if (objPais == null)
                    throw new Exception("Ocurrió un error al obtener el país.");

                ubicacion = string.Format("{0}, {1}, {2}", objMunicipio.Municipio, objEstado.Estado, objPais.Pais);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { FK_Municipio = FK_Municipio, lstMunicipios = lstMunicipios, lstEstados = lstEstados, lstPaises = lstPaises });
            }
            return ubicacion;
        }

        private string GetDivision(int FK_Division, List<DivisionDTO> lstDivisiones)
        {
            string division = string.Empty;
            try
            {
                DivisionDTO objDivision = lstDivisiones.Where(w => w.id == FK_Division && w.registroActivo).FirstOrDefault();
                if (objDivision == null)
                    throw new Exception("Ocurrió un error al obtener la división.");

                division = string.Format("[{0}] {1}", objDivision.numDivision, objDivision.division);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { lstDivisiones = lstDivisiones });
            }
            return division;
        }

        private List<ProximaAccionDTO> GetListaProximasAcciones(ProximaAccionDTO objParamsDTO = null)
        {
            List<ProximaAccionDTO> lstProximasAcciones = new List<ProximaAccionDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_ProximasAcciones WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Proyecto > 0 ? string.Format(" AND FK_Proyecto = {0}", objParamsDTO.FK_Proyecto) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstProximasAcciones = _context.Select<ProximaAccionDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstProximasAcciones;
        }

        private List<ProximaAccionDetalleDTO> GetListaProximasAccionesDetalle(ProximaAccionDetalleDTO objParamsDTO = null)
        {
            List<ProximaAccionDetalleDTO> lstProximasAccionesDetalle = new List<ProximaAccionDetalleDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_ProximasAccionesDetalle WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Accion > 0 ? string.Format(" AND FK_Accion = {0}", objParamsDTO.FK_Accion) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstProximasAccionesDetalle = _context.Select<ProximaAccionDetalleDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstProximasAccionesDetalle;
        }

        private List<CotizacionDTO> GetListaCotizaciones(CotizacionDTO objParamsDTO = null)
        {
            List<CotizacionDTO> lstCotizaciones = new List<CotizacionDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblAF_CRM_Cotizaciones WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Proyecto > 0 ? string.Format(" AND FK_Proyecto = {0}", objParamsDTO.FK_Proyecto) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstCotizaciones = _context.Select<CotizacionDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCotizaciones;
        }

        private List<ComentarioProyectoDTO> GetListaComentarios(ComentarioProyectoDTO objParamsDTO = null)
        {
            List<ComentarioProyectoDTO> lstComentarios = new List<ComentarioProyectoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAF_CRM_ComentariosProyectos WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.FK_Proyecto > 0 ? string.Format(" AND FK_Proyecto = {0}", objParamsDTO.FK_Proyecto) : string.Empty;
                #endregion

                #region CONSULTA
                lstComentarios = _context.Select<ComentarioProyectoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstComentarios;
        }

        private List<CanalDTO> GetListaCanales(CanalDTO objParamsDTO = null)
        {
            List<CanalDTO> lstCanalesDTO = new List<CanalDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAF_CRM_CatCanales WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += string.Format(" AND id != {0}", _FK_CANAL_SIN_DEFINIR);
                    strQuery += !string.IsNullOrEmpty(objParamsDTO.canal) ? string.Format(" AND canal = UPPER(TRIM('{0}'))", objParamsDTO.canal.Trim().ToUpper()) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstCanalesDTO = _context.Select<CanalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCanalesDTO;
        }

        private List<CanalDivisionDTO> GetListaCanalesDivisiones(CanalDivisionDTO objParamsDTO = null)
        {
            List<CanalDivisionDTO> lstCanalesDivisionesDTO = new List<CanalDivisionDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAF_CRM_CatCanalesDivisiones WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_Canal > 0 ? string.Format(" AND FK_Canal = {0}", objParamsDTO.FK_Canal) : string.Empty;
                    strQuery += objParamsDTO.FK_Division > 0 ? string.Format(" AND FK_Division = {0}", objParamsDTO.FK_Division) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstCanalesDivisionesDTO = _context.Select<CanalDivisionDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).AsParallel().ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return lstCanalesDivisionesDTO;
        }

        private List<UsuarioCRMDTO> GetListaUsuariosCRM(UsuarioCRMDTO objParamsDTO = null)
        {
            List<UsuarioCRMDTO> lstUsuariosCRMDTO = new List<UsuarioCRMDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAF_CRM_UsuariosCRM WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstUsuariosCRMDTO = _context.Select<UsuarioCRMDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return lstUsuariosCRMDTO;
        }
        #endregion
        #endregion
    }
}