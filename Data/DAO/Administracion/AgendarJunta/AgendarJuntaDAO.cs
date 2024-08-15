using Core.DTO.Administracion.AgendarJunta;
using Core.DAO.Administracion.AgendarJunta;
using Core.Entity.Administrativo.AgendarJunta;
using Infrastructure.DTO;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
using Data.DAO.Principal.Usuarios;
using Core.DTO;
using System.Data.Entity.Infrastructure;
using Infrastructure.Utils;
using System.Web.Mvc;
using Core.Enum.Principal.Bitacoras;
using Core.Entity.Administrativo.SalaJuntas;
using Core.Enum.Principal;
using Core.DTO.Administracion.SalaJuntas;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.AgendarJunta;
using Core.Enum;

namespace Data.DAO.Administracion.AgendarJunta
{
    public class AgendarJuntaDAO : GenericDAO<tblP_SalaJunta>, IAgendarJuntaDAO
    {
        #region INIT
        private const string _NOMBRE_CONTROLADOR = "SalaJuntasController";
        private const int _SISTEMA = (int)SistemasEnum.OTROS_SERVICIOS;
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        #endregion

        #region SALA JUNTAS
        public Dictionary<string, object> GetSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE REUNIONES
                    List<tblOS_SALAS_SalaJuntas> lstSalaJuntas = _ctx.Select<tblOS_SALAS_SalaJuntas>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.* 
                                    FROM tblOS_SALAS_SalaJuntas AS t1
                                    INNER JOIN tblOS_SALAS_CatEdificiosRelSalas AS t2 ON t1.FK_Sala = t2.id
                                    INNER JOIN tblOS_SALAS_CatEdificios AS t3 ON t3.id = t2.FK_Edificio
                                        WHERE t1.registroActivo = @registroActivo AND t2.FK_Edificio = @FK_Edificio AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo",
                        parametros = new { FK_Edificio = objParamsDTO.FK_Edificio, registroActivo = true }
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstSalaJuntas);
                    #endregion
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> CESalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    resultado = new Dictionary<string, object>();
                    try
                    {
                        #region VALIDACIONES
                        if (string.IsNullOrEmpty(objParamsDTO.asunto)) { throw new Exception("Es necesario indicar el asunto de la reunión."); }
                        if (objParamsDTO.FK_Sala == 0) { throw new Exception("Es seleccionar una sala de juntas."); }
                        #endregion

                        tblOS_SALAS_SalaJuntas objCE = _ctx.tblOS_SALAS_SalaJuntas.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                        if (objCE == null)
                        {
                            #region NUEVO REGISTRO
                            objCE = new tblOS_SALAS_SalaJuntas();
                            objCE.FK_Sala = objParamsDTO.FK_Sala;
                            objCE.asunto = objParamsDTO.asunto.Trim();
                            objCE.fechaInicio = objParamsDTO.fechaInicio;
                            objCE.fechaFin = objParamsDTO.fechaFin;
                            objCE.repeticion = objParamsDTO.repeticion;

                            switch (objCE.repeticion)
                            {
                                case "nunca":
                                    {
                                        objCE.fechaFinRepeticion = null;
                                        objCE.diasRepeticion = null;
                                        break;
                                    }
                                case "otro":
                                    {
                                        objCE.fechaFinRepeticion = objParamsDTO.fechaFinRepeticion;
                                        objCE.diasRepeticion = objParamsDTO.diasRepeticion;
                                        break;
                                    }
                                default:
                                    {
                                        objCE.fechaFinRepeticion = objParamsDTO.fechaFinRepeticion;
                                        objCE.diasRepeticion = null;
                                        break;
                                    }
                            }

                            objCE.comentarios = objParamsDTO.comentarios;
                            objCE.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE.fechaCreacion = DateTime.Now;
                            objCE.registroActivo = true;
                            _ctx.tblOS_SALAS_SalaJuntas.Add(objCE);
                            _ctx.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            if (PuedeActualizarEliminar())
                            {
                                #region ACTUALIZAR REGISTRO
                                objCE.FK_Sala = objParamsDTO.FK_Sala;
                                objCE.asunto = objParamsDTO.asunto.Trim();
                                objCE.fechaInicio = objParamsDTO.fechaInicio;
                                objCE.fechaFin = objParamsDTO.fechaFin;
                                objCE.repeticion = objParamsDTO.repeticion;

                                switch (objCE.repeticion)
                                {
                                    case "nunca":
                                        {
                                            objCE.fechaFinRepeticion = null;
                                            objCE.diasRepeticion = null;
                                            break;
                                        }
                                    case "otro":
                                        {
                                            objCE.fechaFinRepeticion = objParamsDTO.fechaFinRepeticion;
                                            objCE.diasRepeticion = objParamsDTO.diasRepeticion;
                                            break;
                                        }
                                    default:
                                        {
                                            objCE.fechaFinRepeticion = objParamsDTO.fechaFinRepeticion;
                                            objCE.diasRepeticion = null;
                                            break;
                                        }
                                }

                                objCE.comentarios = objParamsDTO.comentarios;
                                objCE.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objCE.fechaModificacion = DateTime.Now;
                                _ctx.SaveChanges();
                                #endregion
                            }
                            else
                                throw new Exception("No cuenta con los facultamientos necesarios para actualizar este evento.");
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, objParamsDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                        dbContextTransaction.Commit();

                        // SE REGISTRA BITACORA
                        SaveBitacora(0, objParamsDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objCE));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, objParamsDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                    return resultado;
                }
            }
        }

        public Dictionary<string, object> GetDatosActualizarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al obtener la información de la reunión."); }
                    #endregion

                    #region SE OBTIENE LA INFORMACIÓN DE LA REUNIÓN PARA ACTUALIZAR
                    tblOS_SALAS_SalaJuntas objSalaJuntas = _ctx.tblOS_SALAS_SalaJuntas.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                    if (objSalaJuntas == null)
                        throw new Exception("Ocurrió un error al obtener la información de la reunión.");

                    resultado.Add(SUCCESS, true);
                    resultado.Add("objSalaJuntas", objSalaJuntas);
                    #endregion
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> EliminarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    resultado = new Dictionary<string, object>();
                    try
                    {
                        #region VALIDACIONES
                        if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al eliminar la reunión."); }

                        List<tblOS_SALAS_SalaJuntas> lstSalaJuntas = _ctx.tblOS_SALAS_SalaJuntas.Where(w => w.FK_Sala == objParamsDTO.id && w.registroActivo).ToList();
                        if (lstSalaJuntas.Count() > 0)
                            throw new Exception("No se puede eliminar la reunión, ya que se encuentra en uso.");
                        #endregion

                        if (PuedeActualizarEliminar())
                        {
                            #region SE ELIMINA LA REUNIÓN SELECCIONADA
                            tblOS_SALAS_SalaJuntas objEliminar = _ctx.tblOS_SALAS_SalaJuntas.Where(w => w.id == objParamsDTO.id && w.registroActivo).FirstOrDefault();
                            if (objEliminar == null)
                                throw new Exception("Ocurrió un error al eliminar la reunión.");

                            objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objEliminar.fechaModificacion = DateTime.Now;
                            objEliminar.registroActivo = false;
                            _ctx.SaveChanges();
                            #endregion
                        }
                        else
                            throw new Exception("No cuenta con los facultamientos necesarios para eliminar este evento.");

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito la reunión.");
                        dbContextTransaction.Commit();

                        // SE REGISTRA BITACORA
                        SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                    return resultado;
                }
            }
        }

        public Dictionary<string, object> FillCboCatEdificios()
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region FILL COMBO EDIFICIOS
                    List<tblOS_SALAS_CatEdificios> lstCatEdificios = _ctx.tblOS_SALAS_CatEdificios.Where(w => w.registroActivo).OrderBy(o => o.edificio).ToList();

                    List<ComboDTO> lstCatEdificiosDTO = new List<ComboDTO>();
                    ComboDTO objCatEdificiosDTO = new ComboDTO();
                    foreach (var item in lstCatEdificios)
                    {
                        if (!string.IsNullOrEmpty(item.edificio))
                        {
                            objCatEdificiosDTO = new ComboDTO();
                            objCatEdificiosDTO.Value = item.id;
                            objCatEdificiosDTO.Text = PersonalUtilities.PrimerLetraMayuscula(item.edificio);
                            lstCatEdificiosDTO.Add(objCatEdificiosDTO);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstCatEdificiosDTO);
                    #endregion
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FillCboCatSalas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE SALAS DEL EDIFICIO
                    List<tblOS_SALAS_CatEdificiosRelSalas> lstCatSalas = _ctx.tblOS_SALAS_CatEdificiosRelSalas.Where(w => w.FK_Edificio == objParamsDTO.FK_Edificio && w.registroActivo).OrderBy(o => o.sala).ToList();

                    List<ComboDTO> lstCatSalasDTO = new List<ComboDTO>();
                    ComboDTO objCatSalasDTO = new ComboDTO();
                    foreach (var item in lstCatSalas)
                    {
                        if (!string.IsNullOrEmpty(item.sala))
                        {
                            objCatSalasDTO = new ComboDTO();
                            objCatSalasDTO.Value = item.id;
                            objCatSalasDTO.Text = PersonalUtilities.PrimerLetraMayuscula(item.sala);
                            lstCatSalasDTO.Add(objCatSalasDTO);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstCatSalasDTO);
                    #endregion
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FillSalas(SalaJuntasDTO objParamsDTO)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE SALAS DEL EDIFICIO
                    List<tblOS_SALAS_CatEdificiosRelSalas> lstCatSalas = _ctx.tblOS_SALAS_CatEdificiosRelSalas.Where(w => w.FK_Edificio == objParamsDTO.FK_Edificio && w.registroActivo).OrderBy(o => o.sala).ToList();

                    List<ComboDTO> lstCatSalasDTO = new List<ComboDTO>();
                    ComboDTO objCatSalasDTO = new ComboDTO();
                    foreach (var item in lstCatSalas)
                    {
                        if (!string.IsNullOrEmpty(item.sala))
                        {
                            objCatSalasDTO = new ComboDTO();
                            objCatSalasDTO.Value = item.id;
                            objCatSalasDTO.Text = PersonalUtilities.PrimerLetraMayuscula(item.sala);
                            objCatSalasDTO.Prefijo = item.capacidad.ToString();
                            lstCatSalasDTO.Add(objCatSalasDTO);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstCatSalasDTO);
                    #endregion
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        #region COMENTADO
        //private List<DateTime> getReunionesRelSala(DateTime fechaInicio, DateTime fechaFin, DateTime fechaFinRep, int FK_Sala)
        //{
        //    List<DateTime> lstReuniones = new List<DateTime>();
        //    try
        //    {
        //        List<tblOS_SALAS_SalaJuntas> lstReunionesAgendadas = _ctx.tblOS_SALAS_SalaJuntas.Where(w => w.FK_Sala == FK_Sala && w.registroActivo).ToList();
        //        foreach (var item in lstReunionesAgendadas)
        //        {
        //            switch (item.repeticion)
        //            {
        //                case "diario":
        //                    break;
        //                case "habiles":
        //                    break;
        //                case "semanal":
        //                    {
        //                        for (DateTime date = fechaInicio; date <= fechaFinRep; date = date.AddDays(1))
        //                        {
        //                            if (date.DayOfWeek == fechaInicio.DayOfWeek)
        //                            {
        //                                lstReuniones.Add(date);
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case "mensual":
        //                    {
        //                        for (DateTime date = fechaInicio; date <= fechaInicio; date = date.AddMonths(1) )
        //                        {
        //                            DateTime fecha = new DateTime(date.Year, date.Month, date.Day);
        //                            if (fecha >= fechaInicio && fecha <= fechaFinRep)
        //                            {
        //                                lstReuniones.Add(fecha);
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case "anual":
        //                    {

        //                    }
        //                    break;
        //                case "otro":
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
        //    }
        //    return lstReuniones;

        //}
        #endregion
        #endregion

        #region CATALOGO FACULTAMIENTOS
        public Dictionary<string, object> GetFacultamientos()
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    List<FacultamientoDTO> lstFacultamientos = _ctx.Select<FacultamientoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t2.nombre + ' ' + t2.apellidoPaterno + ' ' + t2.apellidoMaterno AS nombreCompleto, t1.tipoFacultamiento
	                                        FROM tblOS_SALAS_Facultamientos AS t1
	                                        INNER JOIN tblP_Usuario AS t2 ON t2.id = t1.FK_Usuario
		                                        WHERE t1.registroActivo = @registroActivo AND t2.estatus = @estatus",
                        parametros = new { registroActivo = true, estatus = true }
                    }).ToList();

                    foreach (var item in lstFacultamientos)
                    {
                        item.nombreCompleto = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(item.nombreCompleto);
                        item.facultamiento = PersonalUtilities.PrimerLetraMayuscula(EnumHelper.GetDescription(item.tipoFacultamiento));
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstFacultamientos);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> CrearFacultamiento(FacultamientoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    // VALIDACIONES
                    if (objParamsDTO.FK_Usuario <= 0) { throw new Exception("Es necesario seleccionar a un usuario."); }
                    if (objParamsDTO.tipoFacultamiento <= 0) { throw new Exception("Es necesario seleccionar el facultamiento."); }
                    if (VerificarUsuarioFacultamiento(objParamsDTO)) { throw new Exception("El usuario ya cuenta con facultamiento registrado."); }

                    // REGISTRAR FACULTAMIENTO
                    tblOS_SALAS_Facultamientos objGuardarFacultamiento = new tblOS_SALAS_Facultamientos();
                    objGuardarFacultamiento.FK_Usuario = objParamsDTO.FK_Usuario;
                    objGuardarFacultamiento.tipoFacultamiento = objParamsDTO.tipoFacultamiento;
                    objGuardarFacultamiento.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objGuardarFacultamiento.fechaCreacion = DateTime.Now;
                    objGuardarFacultamiento.registroActivo = true;
                    _ctx.tblOS_SALAS_Facultamientos.Add(objGuardarFacultamiento);
                    _ctx.SaveChanges();

                    // SAVE BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    // VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al actualizar el facultamiento."); }
                    if (objParamsDTO.FK_Usuario <= 0) { throw new Exception("Es necesario seleccionar a un usuario."); }
                    if (objParamsDTO.tipoFacultamiento <= 0) { throw new Exception("Es necesario seleccionar el facultamiento."); }

                    // ACTUALIZAR FACULTAMIENTO
                    tblOS_SALAS_Facultamientos objActualizarFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                    if (objActualizarFacultamiento == null)
                        throw new Exception("Ocurrió un error al buscar el facultamiento para actualizar.");

                    objActualizarFacultamiento.FK_Usuario = objParamsDTO.FK_Usuario;
                    objActualizarFacultamiento.tipoFacultamiento = objParamsDTO.tipoFacultamiento;
                    objActualizarFacultamiento.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objActualizarFacultamiento.fechaModificacion = DateTime.Now;
                    _ctx.SaveChanges();

                    // SAVE BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    // VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al eliminar el facultamiento."); }

                    // ELIMINAR FACULTAMIENTO
                    tblOS_SALAS_Facultamientos objEliminarFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                    if (objEliminarFacultamiento == null)
                        throw new Exception("Ocurrió un error al buscar el facultamiento para eliminar.");

                    objEliminarFacultamiento.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminarFacultamiento.fechaModificacion = DateTime.Now;
                    objEliminarFacultamiento.registroActivo = false;
                    _ctx.SaveChanges();

                    // SAVE BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamsDTO.id, JsonUtils.convertNetObjectToJson(objParamsDTO));

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    List<tblP_Usuario> lstUsuarios = _ctx.tblP_Usuario.Where(w => w.estatus).ToList();
                    if (lstUsuarios.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener el listado de usuarios.");

                    ComboDTO objComboDTO = new ComboDTO();
                    List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                    foreach (var item in lstUsuarios)
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id;
                        objComboDTO.Text = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(PersonalUtilities.NombreCompleto(item.nombre, item.apellidoPaterno, item.apellidoMaterno));
                        lstComboDTO.Add(objComboDTO);
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstComboDTO);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FillCboTipoFacultamientos()
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                    ComboDTO objComboDTO = new ComboDTO();
                    objComboDTO.Value = (int)TipoFacultamientosEnum.ADMINISTRADOR;
                    objComboDTO.Text = PersonalUtilities.PrimerLetraMayuscula(EnumHelper.GetDescription(TipoFacultamientosEnum.ADMINISTRADOR));
                    lstComboDTO.Add(objComboDTO);

                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = (int)TipoFacultamientosEnum.USUARIO;
                    objComboDTO.Text = PersonalUtilities.PrimerLetraMayuscula(EnumHelper.GetDescription(TipoFacultamientosEnum.USUARIO));
                    lstComboDTO.Add(objComboDTO);

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstComboDTO);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.FILLCOMBO, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> GetDatosActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    // VALIDACIONES
                    if (objParamsDTO.id <= 0) { throw new Exception("Ocurrió un error al obtener la información del facultamiento a actualizar."); }

                    // SE OBTIENE INFORMACIÓN A ACTUALIZAR
                    tblOS_SALAS_Facultamientos objFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.id == objParamsDTO.id).FirstOrDefault();
                    if (objFacultamiento == null)
                        throw new Exception("Ocurrió un error al obtener la información del facultamiento a actualizar.");

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, objFacultamiento);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        private bool VerificarUsuarioFacultamiento(FacultamientoDTO objParamsDTO)
        {
            bool existeFacultamiento = false;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    tblOS_SALAS_Facultamientos objFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.FK_Usuario == objParamsDTO.FK_Usuario && w.registroActivo).FirstOrDefault();
                    if (objFacultamiento != null)
                        existeFacultamiento = true;
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    existeFacultamiento = true;
                }
            }
            return existeFacultamiento;
        }
        #endregion

        #region GENERAL
        private bool PuedeActualizarEliminar()
        {
            bool puedeActualizarEliminar = false;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    tblOS_SALAS_Facultamientos objFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.FK_Usuario == (int)vSesiones.sesionUsuarioDTO.id &&
                                                                                                             w.tipoFacultamiento == TipoFacultamientosEnum.ADMINISTRADOR && w.registroActivo).FirstOrDefault();
                    if (objFacultamiento != null)
                        puedeActualizarEliminar = true;
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    puedeActualizarEliminar = false;
                }
            }
            return puedeActualizarEliminar;
        }

        public bool VerificarAcceso()
        {
            bool tieneAcceso = false;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    tblOS_SALAS_Facultamientos objFacultamiento = _ctx.tblOS_SALAS_Facultamientos.Where(w => w.FK_Usuario == (int)vSesiones.sesionUsuarioDTO.id && w.registroActivo).FirstOrDefault();
                    if (objFacultamiento != null)
                        tieneAcceso = true;
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    tieneAcceso = false;
                }
            }
            return tieneAcceso;
        }
        #endregion
    }
}